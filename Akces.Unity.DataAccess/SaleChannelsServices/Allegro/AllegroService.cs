using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess.Services.Allegro.Models;
using System.Net;

namespace Akces.Unity.DataAccess.Services
{
    public class AllegroService : ISaleChannelService
    {
        public AccountType SaleChannelType => AccountType.Allegro;

        private readonly AllegroConfiguration allegroConfiguration;

        public AllegroService(AllegroConfiguration allegroConfiguration)
        {
            this.allegroConfiguration = allegroConfiguration;
        }

        public async Task<bool> AuthenticateAsync()
        {
            var httpClient = new HttpClient();
            var uri = allegroConfiguration.Sandbox ? allegroConfiguration.SandboxBaseAddress : allegroConfiguration.BaseAddress;

            string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                .GetBytes(allegroConfiguration.ClientId + ":" + allegroConfiguration.ClientSecret));

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{uri}/auth/oauth/device?client_id={allegroConfiguration.ClientId}");
            request.Headers.Add("Authorization", "Basic " + encoded);
            request.Content = new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return false;

            var deviceAuthResponse = await response.Content.ReadFromJsonAsync<AllegroDeviceAuthResponse>();

            Process.Start(deviceAuthResponse.verification_uri_complete);

            var repeatCount = 0;
            while (true) 
            {
                var request2 = new HttpRequestMessage(HttpMethod.Post, $"https://{uri}/auth/oauth/token?grant_type=urn:ietf:params:oauth:grant-type:device_code&device_code={deviceAuthResponse.device_code}");
                request2.Headers.Add("Authorization", "Basic " + encoded);
                request2.Content = new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded");
                var response2 = await httpClient.SendAsync(request2);

                if (!response2.IsSuccessStatusCode) 
                {
                    if (repeatCount == 6)
                        return false;

                    await Task.Delay(5000);
                    repeatCount++;
                    continue;
                }

                var allegroGetTokenResponse = await response2.Content.ReadFromJsonAsync<AllegroGetTokenResponse>();
                allegroConfiguration.AccessToken = allegroGetTokenResponse.access_token;
                allegroConfiguration.RefreshToken = allegroGetTokenResponse.refresh_token;
                break;
            }

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

                var allegroGetOffersResponse = await response.Content.ReadFromJsonAsync<AllegroGetOffersResponse>();

                var products = allegroGetOffersResponse.offers
                    .Select(x => new Product()
                    {
                        Id = x.id,
                        Symbol = x.external?.id == null ? "" : x.external.id.Split(' ').FirstOrDefault(),
                        EAN = "",
                        Currency = x.sellingMode.price.currency,
                        OriginalPrice = x.sellingMode.price.amount,
                        Price = x.sellingMode.price.amount,
                        Quantity = x.stock.available,
                        Name = x.name
                    })
                    .ToList();

                var container = new ProductsContainer()
                {
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    PageCount = (int)Math.Ceiling(allegroGetOffersResponse.totalCount / (decimal)pageSize),
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
            using (var httpClient = new HttpClient()) 
            {
                var request = BuildUpdateProductPriceRequest(id, currency, newPrice);
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
            if (allegroConfiguration == null || allegroConfiguration.Id == default)
                return;

            using (var context = new UnityDbContext())
            {
                context.Set<AllegroConfiguration>().Update(allegroConfiguration).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        public void Dispose()
        {
        }

        private async Task<bool> RefreshTokenAsync() 
        {
            var httpClient = new HttpClient();
            var uri = allegroConfiguration.Sandbox ? allegroConfiguration.SandboxBaseAddress : allegroConfiguration.BaseAddress;

            string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                .GetBytes(allegroConfiguration.ClientId + ":" + allegroConfiguration.ClientSecret));

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{uri}/auth/oauth/token?grant_type=refresh_token&refresh_token={allegroConfiguration.RefreshToken}");
            request.Headers.Add("Authorization", "Basic " + encoded);
            request.Content = new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return false;

            var allegroGetTokenResponse = await response.Content.ReadFromJsonAsync<AllegroGetTokenResponse>();

            allegroConfiguration.RefreshToken = allegroGetTokenResponse.refresh_token;
            allegroConfiguration.AccessToken = allegroGetTokenResponse.access_token;

            SaveConfiguration();
            return true;
        }
        private HttpRequestMessage BuildGetProductsRequest(int offset, int pageSize) 
        {
            var uri = allegroConfiguration.Sandbox ? allegroConfiguration.SandboxBaseAddress : allegroConfiguration.BaseAddress;
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.{uri}/sale/offers?offset={offset}&limit={pageSize}");

            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + allegroConfiguration.AccessToken);
            request.Headers.TryAddWithoutValidation("Accept", "application/vnd.allegro.public.v1+json");
            request.Headers.TryAddWithoutValidation("Content-Type", "application/vnd.allegro.public.v1+json");

            return request;
        }
        private HttpRequestMessage BuildUpdateProductPriceRequest(object productId, string currency, decimal newPrice)
        {
            var commandId = Guid.NewGuid().ToString();
            var uri = allegroConfiguration.Sandbox ? allegroConfiguration.SandboxBaseAddress : allegroConfiguration.BaseAddress;
            var request = new HttpRequestMessage(HttpMethod.Put, $"https://api.{uri}/offers/{productId}/change-price-commands/{commandId}");

            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + allegroConfiguration.AccessToken);
            request.Headers.TryAddWithoutValidation("Accept", "application/vnd.allegro.public.v1+json");
            request.Headers.TryAddWithoutValidation("Content-Type", "application/vnd.allegro.public.v1+json");

            var changePrice = new ChangePriceRequest() { id = commandId, input = new Input { buyNowPrice = new BuyNowPrice { amount = newPrice, currency = currency } } };
            var payload = JsonSerializer.Serialize(changePrice);

            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/vnd.allegro.public.v1+json");

            return request;
        }

    }
}
