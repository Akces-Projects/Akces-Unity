using System.Linq;
using System.Collections.Generic;
using InsERT.Moria.Kasa;
using InsERT.Moria.Sfera;
using InsERT.Moria.Slowniki;
using InsERT.Moria.Asortymenty;
using InsERT.Moria.ModelOrganizacyjny;
using InsERT.Moria.Dokumenty.Logistyka;
using Akces.Unity.Models.Nexo;

namespace Akces.Unity.DataAccess.NexoManagers
{
    public class NexoSettingsManager
    {
        private readonly Uchwyt sfera;
        public NexoSettingsManager(Uchwyt sfera)
        {
            this.sfera = sfera;
        }

        public List<Warehouse> GetWarehouses()
        {
            var warehouses = sfera.PodajObiektTypu<IMagazyny>().Dane
                .Wszystkie()
                .Select(x => new Warehouse()
                {
                    Id = x.Id,
                    Symbol = x.Symbol,
                    Name = x.Nazwa
                }).ToList();

            return warehouses;
        }
        public List<PaymentMethod> GetPaymentMethods()
        {
            var paymentMethods = sfera.PodajObiektTypu<IFormyPlatnosci>().Dane
                .Wszystkie()
                .Select(x => new PaymentMethod()
                {
                    Id = x.Id,
                    Name = x.Nazwa
                }).ToList();

            return paymentMethods;
        }
        public List<DeliveryMethod> GetDeliveryMethods()
        {
            var deliveryMethods = sfera.PodajObiektTypu<ISposobyDostaw>().Dane
                .Wszystkie()
                .Select(x => new DeliveryMethod()
                {
                    Id = x.Id,
                    Name = x.Nazwa
                }).ToList();

            return deliveryMethods;
        }
        public List<Transaction> GetTransactions()
        {
            var transactions = sfera.PodajObiektTypu<ITransakcjeHandlowe>().Dane
                .Wszystkie()
                .Select(x => new Transaction()
                {
                    Id = x.Id,
                    Symbol = x.Symbol,
                    Name = x.Nazwa
                }).ToList();

            return transactions;
        }
        public List<Branch> GetBranches()
        {
            var oddzialy = sfera.PodajObiektTypu<IOddzialy>().Dane.Wszystkie()
                .Select(x => new Branch()
                {
                    Id = x.Id,
                    Symbol = x.Symbol,
                    Name = x.Nazwa
                }).ToList();

            var centrale = sfera.PodajObiektTypu<ICentrale>().Dane.Wszystkie()
                .Select(x => new Branch()
                {
                    Id = x.Id,
                    Symbol = x.Symbol,
                    Name = x.Symbol
                }).ToList();

            oddzialy.AddRange(centrale);

            return oddzialy;
        }
        public List<Unit> GetUnits()
        {
            var units = sfera.PodajObiektTypu<IJednostkiMiar>().Dane
                .Wszystkie()
                .Select(x => new Unit()
                {
                    Id = x.Id,
                    Symbol = x.Symbol,
                    Name = x.Nazwa
                }).ToList();

            return units;
        }
        public List<TaxRate> GetTaxRates()
        {
            var taxRates = sfera.PodajObiektTypu<IStawkiVat>().Dane
                .Wszystkie()
                .Select(x => new TaxRate()
                {
                    Id = x.Id,
                    Symbol = x.Symbol,
                    Name = x.Nazwa
                }).ToList();

            return taxRates;
        }
    }
}
