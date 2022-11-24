using Akces.Unity.DataAccess;
using Akces.Unity.DataAccess.Managers;
using Akces.Unity.DataAccess.NexoManagers;
using Akces.Unity.Models;
using Akces.Unity.Models.ConfigurationMembers;
using Akces.Unity.Models.SaleChannels;
using InsERT.Moria.Sfera;
using InsERT.Mox.Product;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CSharp;
using MySql.Data.MySqlClient;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace Akces.Unity.Tests
{
    //public class A
    //{
    //    public int Value { get; set; }
    //}
    //public class B
    //{
    //    public int Value { get; set; }
    //}

    //public class Vals
    //{
    //    public A ValA { get; set; }
    //    public B ValB { get; set; }
    //}
    //internal class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Test().Wait();
    //    }
    //    static async Task Test()
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/Akces-Projects/Akces-Unity/releases");
    //            request.Headers.TryAddWithoutValidation("User-Agent", "Updater");
    //            var response = await client.SendAsync(request);
    //            var content = await response.Content.ReadAsStringAsync();
    //        }
    //    }

    //    public static Uchwyt UruchomSfere()
    //    {
    //        DanePolaczenia danePolaczenia = DanePolaczenia.Jawne("DESKTOP-KD0D5SK\\INSERTNEXO", "nexo_Demo_13", true);
    //        MenedzerPolaczen mp = new MenedzerPolaczen();
    //        Uchwyt sfera = mp.Polacz(danePolaczenia, ProductId.Subiekt);
    //        sfera.ZalogujOperatora("Szef", "robocze");
    //        return sfera;
    //    }

    //    public static void CreateAcc()
    //    {
    //        var accountsManager = new AccountsManager();
    //        using (var accountBO = accountsManager.Create<BaselinkerAccount>())
    //        {
    //            accountBO.Data.Name = "Baselinker1";
    //            accountBO.Data.BaselinkerConfiguration.Token = "3008690-3013746-QDCOQ0OZC24XRXH9A4IQLNRSK5C93U2DPUJLBGVKDATIVPWIGFKKJP2QIIFS9TK3";
    //            accountBO.Data.BaselinkerConfiguration.GetUnconfirmedOrders = false;

    //            accountBO.Data.NexoConfiguration.Warehouses.Add(new WarehouseConfigurationMember()
    //            {
    //                NexoDocumentStatus = "B",
    //                NexoWarehouseSymbol = "MAG",
    //                Default = true
    //            });

    //            accountBO.Data.NexoConfiguration.Branches.Add(new BranchConfigurationMember
    //            {
    //                NexoBranchSymbol = "CENTRALA",
    //                Default = true
    //            });

    //            accountBO.Data.NexoConfiguration.PaymentMethods.Add(new PaymentMethodConfigurationMember()
    //            {
    //                NexoPaymentMethod = "Gotówka",
    //                Default = true
    //            });

    //            accountBO.Data.NexoConfiguration.TaxRates.Add(new TaxRateConfigurationMember()
    //            {
    //                NexoTaxRateSymbol = "8",
    //                Default = true
    //            });

    //            accountBO.Data.NexoConfiguration.Units.Add(new UnitConfigurationMember()
    //            {
    //                NexoUnitSymbol = "kg",
    //                Default = true
    //            });

    //            accountBO.Data.NexoConfiguration.DeliveryMethods.Add(new DeliveryMethodConfigurationMember()
    //            {
    //                NexoDeliveryMethodName = "Kurier",
    //                Default = true
    //            });

    //            accountBO.Data.NexoConfiguration.Transactions.Add(new TransactionConfigurationMember()
    //            {
    //                NexoTranstactionSymbol = "WDT",
    //                Default = true
    //            });

    //            accountBO.Save();
    //        }
    //    }

	//}
}




//UnityConnection.ConnectionString = "Data Source=..\\..\\Data\\unity.db";

//var accountsManager = new AccountsManager();
//var operationReportsManager = new OperationReportsManager();

//CreateAcc();

//var account = accountsManager.Get<BaselinkerAccount>(1);
//var service = account.CreateService();

//var orders = await service.GetOrdersAsync();

////3008690-3013746-QDCOQ0OZC24XRXH9A4IQLNRSK5C93U2DPUJLBGVKDATIVPWIGFKKJP2QIIFS9TK3

//var sfera = UruchomSfere();

//var nexoOrdersManager = new NexoOrdersManager(sfera);

//foreach (var order in orders)
//{
//    var operationResult = await nexoOrdersManager.AddIfNotExistsAsync(order, account.NexoConfiguration);

//    using (var reportBO = operationReportsManager.Create(OperationType.ImportZamowien))
//    {
//        reportBO.Data.Description = $"Zamówienie {account.AccountType} - {order.Original}";
//        operationResult.Infos.ForEach(x => reportBO.AddInfo(x, order.Original));
//        operationResult.Warrnings.ForEach(x => reportBO.AddWarn(x, order.Original));
//        operationResult.Errors.ForEach(x => reportBO.AddError(x, order.Original));
//        reportBO.Save();
//    }
//}