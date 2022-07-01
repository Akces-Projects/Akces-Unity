﻿using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Akces.Unity.Models;
using Akces.Unity.Models.Communication;
using Akces.Unity.Models.SaleChannels.Baselinker;
using Akces.Unity.Models.SaleChannels.Baselinker.Models;

namespace Akces.Unity.DataAccess.Services
{
    public class BaselinkerService : ISaleChannelService
    {
        public AccountType SaleChannelType => AccountType.Baselinker;

        private readonly HttpClient httpClient;
        private readonly BaselinkerConfiguration baselinkerConfiguration;
        private const int GET_ORDERS_LIMIT = 100;

        public BaselinkerService(BaselinkerConfiguration baselinkerConfiguration)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baselinkerConfiguration.BaseAddress);
            this.baselinkerConfiguration = baselinkerConfiguration;
        }

        public async Task<List<Order>> GetOrdersAsync(DateTime from)
        {
            var orders = new List<Order>();

            var dateFrom = ((DateTimeOffset)from).ToUnixTimeSeconds();

            while (true)
            {
                var baselinkerRequest = new BaseLinkerRequest(HttpMethod.Post, httpClient.BaseAddress, "getOrders", baselinkerConfiguration.Token);

                if (baselinkerConfiguration.GetUnconfirmedOrders)
                {
                    baselinkerRequest.SetParameter("date_from", dateFrom);
                    baselinkerRequest.SetParameter("get_unconfirmed_orders", true);
                }
                else
                {
                    baselinkerRequest.SetParameter("date_confirmed_from", dateFrom);
                    baselinkerRequest.SetParameter("get_unconfirmed_orders", false);
                }

                var httpResponse = await httpClient.SendAsync(baselinkerRequest);
                var baseLinkerResponse = await httpResponse.Content.ReadFromJsonAsync<BaseLinkerGetOrdersResponse>();

                if (baseLinkerResponse.Status == "ERROR")
                    throw new Exception("BaseLinker response zwrócił status ERROR");

                var downloadedOrders = baseLinkerResponse.Orders.Select(x => x.ToOrder()).ToList();
                orders.AddRange(downloadedOrders);

                if (downloadedOrders.Count < GET_ORDERS_LIMIT)
                    break;

                if (baselinkerConfiguration.GetUnconfirmedOrders)
                    dateFrom = baseLinkerResponse.Orders.OrderBy(x => x.DateAdd).Last().DateAdd + 1;
                else
                    dateFrom = baseLinkerResponse.Orders.OrderBy(x => x.DateConfirmed).Last().DateAdd + 1;
            }

            return orders.GroupBy(x => x.Original).Select(x => x.First()).ToList();
        }
        public async Task<Order> GetOrderAsync(object id)
        {
            var baselinkerRequest = new BaseLinkerRequest(HttpMethod.Post, httpClient.BaseAddress, "getOrders", baselinkerConfiguration.Token);
            baselinkerRequest.SetParameter("order_id", id);

            var httpResponse = await httpClient.SendAsync(baselinkerRequest);
            var baseLinkerResponse = await httpResponse.Content.ReadFromJsonAsync<BaseLinkerGetOrdersResponse>();
            var order = baseLinkerResponse.Orders.FirstOrDefault()?.ToOrder();

            return order;
        }
        public Task<bool> UpdateOrderAsync(object id, Order orderToUpdate)
        {
            throw new NotImplementedException();
        }
        public Task<bool> AuthenticateAsync()
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}