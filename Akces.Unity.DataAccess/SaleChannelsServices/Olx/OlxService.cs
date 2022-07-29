using System;
using System.Linq;
using System.Text.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess.Services.Olx.Models;

namespace Akces.Unity.DataAccess.Services
{
    public class OlxService : ISaleChannelService
    {
        public AccountType SaleChannelType => AccountType.Olx;

        private HttpListener listener;
        private readonly OlxConfiguration olxConfiguration;
        private string authCode;

        public OlxService(OlxConfiguration olxConfiguration)
        {
            this.olxConfiguration = olxConfiguration;
        }

        public async Task<bool> AuthenticateAsync()
        {
            var getAuthCodeUri = $"https://www.olx.pl/oauth/authorize/?client_id={olxConfiguration.ClientId}&response_type=code&scope=read write v2";
            Process.Start(getAuthCodeUri);
            StartListener();

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(500);
                if (authCode != null)
                    break;
            }

            if (authCode == null)
                throw new Exception("Nie udało się pobrać kodu autoryzacyjnego");

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{olxConfiguration.BaseAddress}api/open/oauth/token");

            var dict = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "client_id", olxConfiguration.ClientId },
                { "client_secret", olxConfiguration.ClientSecret },
                { "code", authCode },
                { "scope", "v2 read write" }
            };

            request.Content = new FormUrlEncodedContent(dict);
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return false;

            var olxAuthResponse = await response.Content.ReadFromJsonAsync<OlxAuthResponse>();

            olxConfiguration.AccessToken = olxAuthResponse.access_token;
            olxConfiguration.RefreshToken = olxAuthResponse.refresh_token;

            httpClient?.Dispose();
            return true;
        }
        public Task<List<Order>> GetOrdersAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<ProductsContainer> GetProductsAsync(bool all, int pageIndex = 0)
        {
            if (all)
            {
                var container = new ProductsContainer();

                while (true)
                {
                    var part = await GetProductsPageAsync(pageIndex);
                    container.Products.AddRange(part.Products);
                    pageIndex++;

                    if (part.Products.Count < 1000)
                        break;
                }

                container.PageSize = container.Products.Count;
                container.PageIndex = 0;
                container.PageCount = 1;

                return container;
            }
            else
            {
                return await GetProductsPageAsync(pageIndex);
            }
        }
        private async Task<ProductsContainer> GetProductsPageAsync(int pageIndex) 
        {
            var pageSize = 1000;
            var offset = pageIndex * pageSize;

            using (var httpClient = new HttpClient())
            {
                var request = BuildGetProductsRequest(offset, pageSize);
                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Nie udało się pobrać produktów. Sprawdź poprawność połączenia");

                var olxGetProductsResponse = await response.Content.ReadFromJsonAsync<OlxGetProductsResponse>();

                var products = olxGetProductsResponse.data
                    .Select(x => new Product()
                    {
                        Id = x.id.ToString(),
                        Symbol = x.title,
                        EAN = "",
                        Currency = x.price?.currency,
                        OriginalPrice = x.price?.value ?? 0,
                        Price = x.price?.value ?? 0,
                        Quantity = 1,
                        Name = x.title
                    })
                    .ToList();


                var Product = products[0];

                var container = new ProductsContainer()
                {
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    PageCount = 100, // olx nie zwraca info o ilości
                    Products = products
                };

                return container;
            }
        }
        public Task<Order> GetOrderAsync(object id)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateOrderAsync(object id, Order orderToUpdate)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> UpdateProductPriceAsync(object id, string currency, decimal newPrice)
        {
            var olxProduct = await GetOlxProductAsync(id);
            olxProduct.price.value = newPrice;
            olxProduct.price.currency = currency;
            olxProduct.salary = olxProduct.salary == null ? new string[0] : olxProduct.salary;

            if (olxProduct.attributes != null) 
            {
                foreach (var attribute in olxProduct.attributes)
                {
                    attribute.values = attribute.values == null ? new string[0] : attribute.values;
                }
            }

            using (var httpClient = new HttpClient()) 
            {
                var request = new HttpRequestMessage(HttpMethod.Put, $"{olxConfiguration.BaseAddress}api/partner/adverts/{id}");

                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + olxConfiguration.AccessToken);
                request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                request.Headers.TryAddWithoutValidation("Version", "2.0");

                var payload = JsonSerializer.Serialize(olxProduct);
                request.Content = new StringContent(payload);
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    throw new Exception(content);
                }

                return response.IsSuccessStatusCode;                    
            }
        }
        public async Task<bool> ValidateConnectionAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var request = BuildGetProductsRequest(0, 1);
                var response = await httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    await RefreshTokenAsync();

                var repeatedRequest = BuildGetProductsRequest(0, 1);
                var repeatedResponse = await httpClient.SendAsync(repeatedRequest);
                var content = await response.Content.ReadAsStringAsync();

                if (!repeatedResponse.IsSuccessStatusCode)
                    throw new Exception("Nie udało się pobrać produktów. Sprawdź poprawność połączenia"
                        + Environment.NewLine + Environment.NewLine + content);

                return true;
            }
        }
        public void SaveConfiguration()
        {
            if (olxConfiguration == null || olxConfiguration.Id == default)
                return;

            using (var context = new UnityDbContext())
            {
                context.Set<OlxConfiguration>().Update(olxConfiguration).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        public void Dispose()
        {
            listener?.Stop();
            listener?.Close();
            listener = null;
        }

        private HttpRequestMessage BuildGetProductsRequest(int offset, int pageSize) 
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{olxConfiguration.BaseAddress}api/partner/adverts?offset={offset}&limit={pageSize}");

            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + olxConfiguration.AccessToken);
            request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
            request.Headers.TryAddWithoutValidation("Version", "2.0");

            return request;
        }

        private async Task<bool> RefreshTokenAsync() 
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{olxConfiguration.BaseAddress}api/open/oauth/token");

            var dict = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "client_id", olxConfiguration.ClientId },
                { "client_secret", olxConfiguration.ClientSecret },
                { "refresh_token", olxConfiguration.RefreshToken }
            };

            request.Content = new FormUrlEncodedContent(dict);
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return false;

            var olxAuthResponse = await response.Content.ReadFromJsonAsync<OlxAuthResponse>();

            olxConfiguration.AccessToken = olxAuthResponse.access_token;
            olxConfiguration.RefreshToken = olxAuthResponse.refresh_token;

            SaveConfiguration();
            httpClient?.Dispose();

            return true;
        }
        public async Task<OlxProductData> GetOlxProductAsync(object productId)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{olxConfiguration.BaseAddress}api/partner/adverts/{productId}");

                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + olxConfiguration.AccessToken);
                request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                request.Headers.TryAddWithoutValidation("Version", "2.0");

                var response = await httpClient.SendAsync(request); 

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Nie udało się pobrać produktów. Sprawdź poprawność połączenia");

                var getProductResponse = await response.Content.ReadFromJsonAsync<OlxGetProductResponse>();                

                return getProductResponse.data;
            }
        }

        public void StartListener()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://*/");
            listener.Start();
            listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
        }
        private void ListenerCallback(IAsyncResult result)
        {
            if (listener == null || !listener.IsListening)
                return;

            var context = listener.EndGetContext(result);
            authCode = context.Request.Url.OriginalString.Replace("code=", " ").Split(' ')[1];
            var response = context.Response;
            response.Redirect("https://www.olx.pl/");
            response.Close();
            listener.Stop();
        }
    }
}
