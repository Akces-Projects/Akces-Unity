using Akces.Unity.DataAccess;
using Akces.Unity.DataAccess.Managers;
using Akces.Unity.DataAccess.NexoManagers;
using Akces.Unity.Models;
using Akces.Unity.Models.ConfigurationMembers;
using Akces.Unity.Models.SaleChannels;
using InsERT.Moria.Sfera;
using InsERT.Mox.Product;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Akces.Unity.Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Console.ReadKey();
            }
        }

        

        public static Uchwyt UruchomSfere()
        {
            DanePolaczenia danePolaczenia = DanePolaczenia.Jawne("DESKTOP-KD0D5SK\\INSERTNEXO", "nexo_Demo_13", true);
            MenedzerPolaczen mp = new MenedzerPolaczen();
            Uchwyt sfera = mp.Polacz(danePolaczenia, ProductId.Subiekt);
            sfera.ZalogujOperatora("Szef", "robocze");
            return sfera;
        }

        public static void CreateAcc()
        {
            var accountsManager = new AccountsManager();
            using (var accountBO = accountsManager.Create<BaselinkerAccount>())
            {
                accountBO.Data.Name = "Baselinker1";
                accountBO.Data.BaselinkerConfiguration.Token = "3008690-3013746-QDCOQ0OZC24XRXH9A4IQLNRSK5C93U2DPUJLBGVKDATIVPWIGFKKJP2QIIFS9TK3";
                accountBO.Data.BaselinkerConfiguration.GetUnconfirmedOrders = false;

                accountBO.Data.NexoConfiguration.Warehouses.Add(new WarehouseConfigurationMember()
                {
                    ErpDocumentStatus = "B",
                    ErpWarehouseSymbol = "MAG",
                    Default = true
                });

                accountBO.Data.NexoConfiguration.Branches.Add(new BranchConfigurationMember
                {
                    ErpBranchSymbol = "CENTRALA",
                    Default = true
                });

                accountBO.Data.NexoConfiguration.PaymentMethods.Add(new PaymentMethodConfigurationMember()
                {
                    ErpPaymentMethod = "Gotówka",
                    Default = true
                });

                accountBO.Data.NexoConfiguration.TaxRates.Add(new TaxRateConfigurationMember()
                {
                    ErpTaxRateSymbol = "8",
                    Default = true
                });

                accountBO.Data.NexoConfiguration.Units.Add(new UnitConfigurationMember()
                {
                    ErpUnitSymbol = "kg",
                    Default = true
                });

                accountBO.Data.NexoConfiguration.DeliveryMethods.Add(new DeliveryMethodConfigurationMember()
                {
                    ErpDeliveryMethodName = "Kurier",
                    Default = true
                });

                accountBO.Data.NexoConfiguration.Transactions.Add(new TransactionConfigurationMember()
                {
                    ErpTranstactionSymbol = "WDT",
                    Default = true
                });

                accountBO.Save();
            }
        }

	}
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