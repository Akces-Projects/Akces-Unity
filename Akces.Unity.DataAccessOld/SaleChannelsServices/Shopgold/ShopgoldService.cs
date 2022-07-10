using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;

namespace Akces.Unity.DataAccess.Services
{
    public class ShopgoldService : ISaleChannelService
    {
        private readonly ShopgoldConfiguration shopgoldConfiguration;
        public AccountType SaleChannelType { get => AccountType.shopGold; }


        public ShopgoldService(ShopgoldConfiguration shopgoldConfiguration)
        {
            this.shopgoldConfiguration = shopgoldConfiguration;
        }

        public Task<Order> GetOrderAsync(object id)
        {
            throw new NotImplementedException();
        }
        public Task<List<Order>> GetOrdersAsync()
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateOrderAsync(object id, Order orderToUpdate)
        {
            throw new NotImplementedException();
        }
        public async Task<ProductsContainer> GetProductsAsync(int pageIndex)
        {
            using (var sqlConnection = new MySqlConnection(shopgoldConfiguration.ConnectionString))
            {
                var pageSize = 1000;
                var offset = pageIndex * pageSize;

                await sqlConnection.OpenAsync();
                var countCommand = sqlConnection.CreateCommand();
                countCommand.CommandText = "select count(products_id) FROM products";

                var t = countCommand.ExecuteScalar();
                var pageCount = int.Parse(t.ToString());

                var cmd = sqlConnection.CreateCommand();

                cmd.CommandText = @"SELECT 
                                        p.products_id,
                                        p.products_ean,
                                        c.code,    
                                        p.products_price_tax,
                                        p.products_quantity,
                                        d.products_name,
                                        p.products_id_private
                                    FROM                                    
                                        products as p
                                        left join products_description as d on d.products_id = p.products_id
                                        left join currencies as c on c.currencies_id = p.products_currencies_id " +
                                    $"LIMIT {pageSize} OFFSET {offset}";

                var reader = await cmd.ExecuteReaderAsync();

                var products = new List<Product>();

                while (reader.Read()) 
                {
                    var product = new Product
                    {
                        Id = reader.GetValue(0)?.ToString(),
                        EAN = reader.GetValue(1)?.ToString(),
                        Currency = reader.GetValue(2)?.ToString(),
                        Price = decimal.Parse(reader.GetValue(3)?.ToString() ?? "0.0"),
                        Quantity = decimal.Parse(reader.GetValue(4)?.ToString() ?? "0.0"),
                        Name = reader.GetValue(5)?.ToString(),
                        Symbol = reader.GetValue(6)?.ToString()
                    };
                    products.Add(product);
                }

                var container = new ProductsContainer()
                {
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    PageCount = (int)Math.Ceiling(pageCount / (decimal)pageSize),
                    Products = products
                };

                return container;
            }
        }
        public async Task<bool> UpdateProductPriceAsync(object id, string currency, decimal newPrice)
        {
            return await Task.Run(async () => 
            {
                using (var sqlConnection = new MySqlConnection(shopgoldConfiguration.ConnectionString))
                {
                    sqlConnection.Open();
                    var getProdCmd = sqlConnection.CreateCommand();

                    getProdCmd.CommandText = "SELECT t.tax_rate, products_price_tax " +
                        "FROM products p " +
                        "LEFT JOIN tax_rates t ON t.tax_rates_id = p.products_tax_class_id " +
                        $"WHERE products_id = '{id}'";

                    var reader = await getProdCmd.ExecuteReaderAsync();
                    reader.Read();
                    var tax_rate = decimal.Parse(reader.GetValue(0)?.ToString());
                    var price_tax_incl = decimal.Parse(reader.GetValue(1)?.ToString());
                    reader.Close();

                    if (Math.Abs(price_tax_incl - newPrice) < 0.005m)
                        return true;

                    var price_tax_excl = Math.Round(newPrice / (1 + tax_rate / 100), 2, MidpointRounding.AwayFromZero);
                    price_tax_incl = Math.Round(newPrice, 2, MidpointRounding.AwayFromZero);
                    var price_tax = price_tax_incl - price_tax_excl;

                    var updatePriceCmd = sqlConnection.CreateCommand();
                    updatePriceCmd.CommandText = string.Format("UPDATE products SET " +
                        "products_price_tax = '{0}', " +
                        "products_price = '{1}', " +
                        "products_tax = '{2}' " +
                        "WHERE products_id = '{3}' " +
                        "LIMIT 1",

                        price_tax_incl.ToString().Replace(',', '.'),
                        price_tax_excl.ToString().Replace(',', '.'),
                        price_tax.ToString().Replace(',', '.'),
                        id);

                    var result = await updatePriceCmd.ExecuteNonQueryAsync();

                    return result > 0;
                }
            });
        }
        public async Task<bool> AuthenticateAsync()
        {
            using (var sqlConnection = new MySqlConnection(shopgoldConfiguration.ConnectionString)) 
            {
                await sqlConnection.OpenAsync();
                var cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "select * from products limit 1";
                var counter = (int)cmd.ExecuteScalar();
                return counter > 0;
            }
        }
        public async Task<bool> TestConnectionAsync()
        {
            using (var sqlConnection = new MySqlConnection(shopgoldConfiguration.ConnectionString))
            {
                await sqlConnection.OpenAsync();
                var cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "select * from products limit 1";
                var counter = (int)cmd.ExecuteScalar();
                return counter > 0;
            }
        }
        public void SaveConfiguration()
        {
            if (shopgoldConfiguration == null || shopgoldConfiguration.Id == default)
                return;

            using (var context = new UnityDbContext())
            {
                context.Set<ShopgoldConfiguration>().Update(shopgoldConfiguration).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        public void Dispose()
        {
            
        }
    }
}
