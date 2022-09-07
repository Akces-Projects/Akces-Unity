using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;
using System.Net.Http;
using Unity.SaleChannels.Shoper.Models;
using Unity.SaleChannels.Shoper.Responses;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.Json;

namespace Akces.Unity.DataAccess.Services
{
    public class ShoperService : ISaleChannelService
    {
        private HttpClient httpClient;
        private readonly ShoperConfiguration shoperConfiguration;

        public AccountType SaleChannelType { get => AccountType.Shoper; }
        public ShoperConfiguration Configuration { get; set; }


        public ShoperService(ShoperConfiguration shoperConfiguration)
        {
            this.shoperConfiguration = shoperConfiguration;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var shoperOrders = new List<ShoperOrder>();
            var currentPage = 1;
            var pagesCount = 1;

            using (httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Configuration.BaseAddress);

                var shippings = await GetShippingsAsync();
                var paymentMethods = await GetPaymentMethodsAsync();
                var currencies = await GetCurrienciesAsync();

                do
                {
                    var uri = $"webapi/rest/orders?page={currentPage}&limit=50";

                    if (Configuration.ImportOffset_Hours != default)
                    {
                        var from = DateTime.Now.AddHours(-Configuration.ImportOffset_Hours);
                        var dateFilter = $"{{\"date\":{{\">=\":\"{from:yyyy-MM-dd HH:mm:ss}\"}}}}";
                        uri += "&filters=" + dateFilter;
                    }

                    var getOrdersResponse = await CallAsync<GetOrdersResponse>(uri);
                    shoperOrders.AddRange(getOrdersResponse.Orders);
                    pagesCount = getOrdersResponse.Pages;
                    currentPage++;
                }
                while (currentPage <= pagesCount);

                if (Configuration.ImportOrders_OnlyCashOnDeliveryOrPaid) shoperOrders.RemoveAll(x => x.IsPaid == false && x.PaymentId != "2");
                if (Configuration.ImportOrders_OnlyConfirmed) shoperOrders.RemoveAll(x => x.Confirm == "0");

                foreach (var shoperOrder in shoperOrders)
                {
                    shoperOrder.OrderPositions = await GetOrderPositionsAsync(shoperOrder.OrderId);
                    shoperOrder.Shipping = shippings.FirstOrDefault(x => x.ShippingId == shoperOrder.ShippingId);
                    shoperOrder.PaymentMethod = paymentMethods.FirstOrDefault(x => x.PaymentId == shoperOrder.PaymentId);
                    shoperOrder.Currency = currencies.FirstOrDefault(x => x.CurrencyId == shoperOrder.CurrencyId);
                }

                var orders = new List<Order>();

                foreach (var shoperOrder in shoperOrders)
                {
                    try
                    {
                        var order = shoperOrder.ToOrder();
                        orders.Add(order);
                    }
                    catch (Exception e)
                    {
                    }
                }
                return orders;
            }
        }
        public async Task<Order> GetOrderAsync(object id)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> UpdateOrderAsync(object id, Order orderToUpdate)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> AuthenticateAsync()
        {
            using (httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Configuration.BaseAddress);

                var username = Configuration.ClientId;
                var password = Configuration.ClientSecret;

                var encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

                var uri = "webapi/rest/auth";
                var request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Headers.Add("Authorization", "Basic " + encoded);
                var response = await httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new Exception("Niepoprawny login lub hasło");

                var json = await response.Content.ReadAsStringAsync();
                var shoperAccessToken = JsonSerializer.Deserialize<ShoperAccessToken>(json);
                Configuration.Token = shoperAccessToken.Value;

                return true;
            }
        }
        public async Task<bool> ValidateConnectionAsync() 
        {
            try
            {
                await GetCurrienciesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void SaveConfiguration()
        {
            if (shoperConfiguration == null || shoperConfiguration.Id == default)
                return;

            using (var context = new UnityDbContext())
            {
                context.Set<ShoperConfiguration>().Update(shoperConfiguration).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        public Task<ProductsContainer> GetProductsAsync(bool all, int pageIndex = 0)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateProductPriceAsync(object id, string currency, decimal newPrice)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public async Task<T> CallAsync<T>(string uri) where T : class
        {
            T obj = null;

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                requestMessage.Headers.Add("Authorization", $"Bearer {Configuration.Token}");
                var response = await httpClient.SendAsync(requestMessage);
                var json = await response.Content.ReadAsStringAsync();
                obj = JsonSerializer.Deserialize<T>(json);

                if ((int)response.StatusCode == 429)
                {
                    await Task.Delay(5000);
                    return await CallAsync<T>(uri);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await AuthenticateAsync();
                    return await CallAsync<T>(uri);
                }
            }
            catch
            {
            }

            return obj;
        }
        private async Task<List<ShoperOrderPosition>> GetOrderPositionsAsync(string orderId)
        {
            var shoperOrderPositions = new List<ShoperOrderPosition>();
            var currentPage = 1;
            int pagesCount;

            do
            {
                var uri = $"webapi/rest/order-products?page={currentPage}&limit=50&filters={{\"order_id\": {orderId}}}";

                var getOrderPositionsResponse = await CallAsync<GetOrderPositionsResponse>(uri);

                shoperOrderPositions.AddRange(getOrderPositionsResponse.OrderPositions);
                pagesCount = getOrderPositionsResponse.Pages;
                currentPage++;
            }
            while (currentPage <= pagesCount);

            return shoperOrderPositions;
        }
        private async Task<ShoperProduct> GetProductDetailsAsync(string productId)
        {
            var uri = $"webapi/rest/products/{productId}";

            var shoperProduct = await CallAsync<ShoperProduct>(uri);

            return shoperProduct;
        }
        private async Task<List<ShoperPaymentMethod>> GetPaymentMethodsAsync()
        {
            var shoperPaymentMethods = new List<ShoperPaymentMethod>();
            var currentPage = 1;
            int pagesCount;

            do
            {
                var uri = $"webapi/rest/payments?page={currentPage}&limit=50";

                var getPaymentMethodsResponse = await CallAsync<GetPaymentMethodsResponse>(uri);

                shoperPaymentMethods.AddRange(getPaymentMethodsResponse.PaymentMethods);
                pagesCount = getPaymentMethodsResponse.Pages;
                currentPage++;
            }
            while (currentPage <= pagesCount);

            return shoperPaymentMethods;
        }
        private async Task<List<ShoperShipping>> GetShippingsAsync()
        {
            var shoperShippings = new List<ShoperShipping>();
            var currentPage = 1;
            int pagesCount;

            do
            {
                var uri = $"webapi/rest/shippings?page={currentPage}&limit=50";

                var getShippingsResponse = await CallAsync<GetShippingsResponse>(uri);

                shoperShippings.AddRange(getShippingsResponse.Shippings);
                pagesCount = getShippingsResponse.Pages;
                currentPage++;
            }
            while (currentPage <= pagesCount);

            return shoperShippings;
        }
        private async Task<List<ShoperCurrency>> GetCurrienciesAsync()
        {
            var shoperCurrencies = new List<ShoperCurrency>();
            var currentPage = 1;
            int pagesCount;

            do
            {
                var uri = $"webapi/rest/currencies?page={currentPage}&limit=50";

                var getCurrenciesResponse = await CallAsync<GetCurrenciesResponse>(uri);

                shoperCurrencies.AddRange(getCurrenciesResponse.Currencies);
                pagesCount = getCurrenciesResponse.Pages;
                currentPage++;
            }
            while (currentPage <= pagesCount);

            return shoperCurrencies;
        }
    }
}
