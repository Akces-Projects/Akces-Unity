using System;
using System.Linq;
using System.Threading.Tasks;
using InsERT.Moria.Kasa;
using InsERT.Moria.Flagi;
using InsERT.Moria.Sfera;
using InsERT.Moria.Waluty;
using InsERT.Moria.Klienci;
using InsERT.Moria.Slowniki;
using InsERT.Moria.ModelDanych;
using InsERT.Moria.Asortymenty;
using InsERT.Moria.ModelOrganizacyjny;
using InsERT.Moria.Dokumenty.Logistyka;
using Akces.Unity.Models;
using Akces.Unity.Models.Communication;

namespace Akces.Unity.DataAccess.NexoManagers.Operations
{
    public class AddOrderOperation : IOperation<Order>
    {
        private readonly OperationResult operationResult;
        private readonly NexoConfiguration configuration;
        private readonly Uchwyt sfera;
        public Order Data { get; private set; }

        public AddOrderOperation(Order data, Uchwyt sfera, NexoConfiguration configuration)
        {
            Data = data;
            this.sfera = sfera;
            this.configuration = configuration;
            operationResult = new OperationResult();
            operationResult.ObjectName = data.Original;

            configuration.Units.ForEach(x => x.CountriesCodes = x.CountriesCodes ?? "");
            configuration.Warehouses.ForEach(x => x.CountriesCodes = x.CountriesCodes ?? "");
            configuration.Branches.ForEach(x => x.CountriesCodes = x.CountriesCodes ?? "");
            configuration.PaymentMethods.ForEach(x => x.CountriesCodes = x.CountriesCodes ?? "");
            configuration.DeliveryMethods.ForEach(x => x.CountriesCodes = x.CountriesCodes ?? "");
            configuration.TaxRates.ForEach(x => x.CountriesCodes = x.CountriesCodes ?? "");
            configuration.Transactions.ForEach(x => x.CountriesCodes = x.CountriesCodes ?? "");
        }

        public async Task<OperationResult> ExecuteAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    AddOrderToNexoIfNotExists();
                    operationResult.IsSuccess = true;
                }
                catch (Exception e)
                {
                    operationResult.Errors.Add(e.Message);
                    operationResult.IsSuccess = false;
                }
            });

            return operationResult;
        }
        private void AddOrderToNexoIfNotExists()
        {
            var zkManager = sfera.PodajObiektTypu<IZamowieniaOdKlientow>();
            var alreadyExists = zkManager.Dane.Wszystkie().Any(x => x.NumerZewnetrzny == Data.Original);

            if (alreadyExists)
            {
                operationResult.NoChangesMade = true;
                operationResult.Infos.Add($"Zamowienie {Data.Original} istnieje w nexo");
                return;
            }

            var konfiguracja = sfera.PodajObiektTypu<IKonfiguracje>().DaneDomyslne.ZamowienieOdKlienta;
            var miejscaSprzedazy = sfera.PodajObiektTypu<IMiejscaSprzedazy>().Dane.Wszystkie();
            var flagiWlasne = sfera.PodajObiektTypu<IFlagiWlasne>().Dane.Wszystkie();
            var waluty = sfera.PodajObiektTypu<IWaluty>().Dane.Wszystkie();

            using (var zkOB = zkManager.Utworz(konfiguracja))
            {
                #region Podstawowe

                zkOB.Dane.NumerZewnetrzny = Data.Original;
                zkOB.Dane.DataWprowadzenia = Data.OriginalDate;
                zkOB.Dane.TerminRealizacji = Data.CompletionDate > Data.OriginalDate ? Data.CompletionDate : Data.OriginalDate;
                zkOB.Dane.MiejsceWprowadzenia = DopasujOddzial(Data.Branch, Data.Purchaser.CountryCode);

                zkOB.Dane.PodmiotWybrany = DopasujPodmiot(Data.Purchaser);
                zkOB.Dane.Magazyn = DopasujMagazyn(null, Data.Warehouse, Data.Purchaser.CountryCode);
                zkOB.Dane.SposobDostawy = DopasujSposobDostawy(Data.Delivery.DeliveryMethod, Data.Purchaser.CountryCode);
                zkOB.Dane.TransakcjaHandlowa = DopasujTransakcjeHandlowa(Data.Purchaser.CountryCode);

                #endregion                               

                if (zkOB.Dane.SposobDostawy.WidocznyAdresDostawy)
                {
                    var panstwo = sfera.PodajObiektTypu<IPanstwa>().Dane.Wszystkie().FirstOrDefault(x => x.Nazwa == Data.Purchaser.Country || x.KodPanstwaUE == Data.Purchaser.CountryCode);
                    zkOB.Dane.MiejsceDostawyTyp = (byte)MiejsceDostawyTyp.Reczny;
                    zkOB.Dane.MiejsceDostawy = new AdresHistoria();
                    zkOB.Dane.MiejsceDostawy.Panstwo = panstwo;
                    zkOB.Dane.MiejsceDostawy.Nazwa = Data.Delivery.DeliveryAddress.Name;

                    if (!string.IsNullOrEmpty(Data.Delivery.DeliveryAddress.DeliveryPointName))
                    {
                        zkOB.Dane.MiejsceDostawy.Panstwo = panstwo;
                        zkOB.Dane.MiejsceDostawy.Nazwa = Data.Delivery.DeliveryAddress.Name;
                        zkOB.Dane.MiejsceDostawy.Linia1 = Data.Delivery.DeliveryAddress.Name;
                        zkOB.Dane.MiejsceDostawy.Linia2 = Data.Delivery.DeliveryAddress.Line1;
                        zkOB.Dane.MiejsceDostawy.Linia3 = (Data.Delivery.DeliveryAddress.Line2 + " " + Data.Delivery.DeliveryAddress.Line3).Trim();
                    }
                    else
                    {
                        zkOB.Dane.MiejsceDostawy.Panstwo = panstwo;
                        zkOB.Dane.MiejsceDostawy.Nazwa = Data.Delivery.DeliveryAddress.Name;
                        zkOB.Dane.MiejsceDostawy.Linia1 = Data.Delivery.DeliveryAddress.Name;
                        zkOB.Dane.MiejsceDostawy.Linia2 = Data.Delivery.DeliveryAddress.Line1;
                        zkOB.Dane.MiejsceDostawy.Linia3 = (Data.Delivery.DeliveryAddress.Line2 + " " + Data.Delivery.DeliveryAddress.Line3).Trim();
                    }
                }

                #region Asortyment

                zkOB.Dane.OperacjePrzeliczaniaPozycji = OperacjePrzeliczaniaPozycji.Brutto_ID;
                //zkOB.UstawCenyIPrzelicz();
                zkOB.Dane.StatusDokumentu = DopasujStatusDokumentu(Data.Warehouse, Data.Purchaser.CountryCode);
                zkOB.Dane.Waluta = waluty.FirstOrDefault(x => x.Symbol == Data.Currency || x.Nazwa == Data.Currency);

                foreach (var product in Data.Products)
                {
                    var asortyment = DopasujAsortyment(product.Symbol);
                    var jm = DopasujJednostkeMiary(product.Unit, Data.Purchaser.CountryCode, product.EAN);

                    if (asortyment != null)
                    {
                        var jma = asortyment.JednostkiMiar.FirstOrDefault(x => x.JednostkaMiary.Symbol == jm.Symbol);
                        var pozycja = zkOB.Pozycje.Dodaj(asortyment, product.Quantity, jma);
                        pozycja.StawkaVat = DopasujStawkeVAT(product.Tax, Data.Purchaser.CountryCode);
                        pozycja.Cena.BruttoPrzedRabatem = product.Price;
                        pozycja.Cena.RabatProcent = product.DiscountPercentage;
                    }
                    else
                    {
                        var pozycja = zkOB.Pozycje.Dodaj(product.Symbol, product.Name, product.Quantity, jm, jm.Precyzja, null, null);
                        pozycja.StawkaVat = DopasujStawkeVAT(product.Tax, Data.Purchaser.CountryCode);
                        pozycja.Cena.BruttoPrzedRabatem = product.Price;
                        pozycja.Cena.RabatProcent = product.DiscountPercentage / 100;
                    }
                }

                if (Data.Delivery.DeliveryCost > 0)
                {
                    var uslugaDostawy = DopasujAsortyment(Data.Delivery.DeliveryMethod);

                    if (uslugaDostawy != null)
                    {
                        var pozycja = zkOB.Pozycje.Dodaj(uslugaDostawy, 1, uslugaDostawy.JednostkiMiar.First());
                        pozycja.StawkaVat = DopasujStawkeVAT(Data.Delivery.DeliveryTax, Data.Purchaser.CountryCode);
                        pozycja.Cena.BruttoPrzedRabatem = Data.Delivery.DeliveryCost;
                    }
                    else
                    {
                        var jm = sfera.PodajObiektTypu<IJednostkiMiar>().DaneDomyslne.Usluga;
                        var pozycja = zkOB.Pozycje.Dodaj("US", Data.Delivery.DeliveryMethod, 1, jm, jm.Precyzja, null, null);
                        pozycja.StawkaVat = DopasujStawkeVAT(Data.Delivery.DeliveryTax, Data.Purchaser.CountryCode);
                        pozycja.Cena.BruttoPrzedRabatem = Data.Delivery.DeliveryCost;
                    }
                }
                #endregion

                #region Podsumowanie

                zkOB.Dane.Tytul = Data.Title;
                zkOB.Dane.Podtytul = Data.Subtitle ?? "";
                zkOB.Dane.Uwagi = Data.Annotation;

                foreach (var payment in Data.Payments)
                {
                    var formaPlatnosci = DopasujFormePlatnosci(payment.PaymentMethod, Data.Delivery.DeliveryMethod, payment.Currency, Data.Purchaser.CountryCode);

                    if (formaPlatnosci.TypPlatnosci.Odroczony)
                    {
                        var platnoscDokumentu = zkOB.Platnosci.DodajPlatnoscOdroczona(formaPlatnosci, payment.Value);
                        if (payment.TimeLimit != null) platnoscDokumentu.Last().Termin = payment.TimeLimit;
                    }
                    else
                    {
                        var platnoscDokumentu = zkOB.Platnosci.DodajPlatnoscNatychmiastowa(formaPlatnosci, Math.Round(payment.Value, 2, MidpointRounding.AwayFromZero));
                    }
                }

                #endregion
                var dodano = zkOB.Zapisz();

                if (!dodano)
                {
                    operationResult.Errors.Add(zkOB.WypiszBledy());
                    throw new Exception("Nie dodano zamówienia");
                }

                operationResult.Infos.Add($"Dodano zamówienie [Id: {zkOB.Dane.Id}] Numer: {zkOB.Dane.NumerWewnetrzny.PelnaSygnatura}");
            }
        }

        private FormaPlatnosci DopasujFormePlatnosci(string metodaPlatnosciNaZamowienia, string sposobDostawyNaZamowieniu, string walutaNaZamowieniu, string kodKraju)
        {
            var defaultPaymentMethodSymbol = configuration.PaymentMethods.First(x => x.Default).ErpPaymentMethod;
            var countryPaymentMethodSymbol = configuration.PaymentMethods.FirstOrDefault(x => x.CountriesCodes.Contains(kodKraju))?.ErpPaymentMethod;
            var countryAndCurrencyPaymentMethodSymbol = configuration.PaymentMethods.FirstOrDefault(x => x.CurrencyCode == walutaNaZamowieniu && x.CountriesCodes.Contains(kodKraju))?.ErpPaymentMethod;

            var paymentMethod = configuration.PaymentMethods.FirstOrDefault(x => x.ChannelPaymentMethod == metodaPlatnosciNaZamowienia && x.ChannelDeliveryMethod == sposobDostawyNaZamowieniu && x.CountriesCodes.Contains(kodKraju));
            paymentMethod = paymentMethod ?? configuration.PaymentMethods.FirstOrDefault(x => x.ChannelPaymentMethod == metodaPlatnosciNaZamowienia && x.ChannelDeliveryMethod == sposobDostawyNaZamowieniu);
            paymentMethod = paymentMethod ?? configuration.PaymentMethods.FirstOrDefault(x => x.ChannelPaymentMethod == metodaPlatnosciNaZamowienia);

            if (paymentMethod == null)
            {
                operationResult.Warrnings.Add("Wybrana forma płatności nie jest pierwszym wyborem");
            }

            paymentMethod = paymentMethod ?? configuration.PaymentMethods.FirstOrDefault(x => x.ChannelPaymentMethod == metodaPlatnosciNaZamowienia);

            var paymentMethodSymbol = paymentMethod?.ErpPaymentMethod ?? countryAndCurrencyPaymentMethodSymbol ?? countryPaymentMethodSymbol ?? defaultPaymentMethodSymbol;
            var formyPlatnosciMgr = sfera.PodajObiektTypu<IFormyPlatnosci>().Dane.Wszystkie();

            var formaPlatnosci = formyPlatnosciMgr.FirstOrDefault(x => x.Nazwa == paymentMethodSymbol);
            return formaPlatnosci;
        }
        private Magazyn DopasujMagazyn(string magazynNaPozycjiZamowienia, string magazynNaZamowieniu, string kodKraju)
        {
            var defaultWarehouseSymbol = configuration.Warehouses.First(x => x.Default).ErpWarehouseSymbol;
            var countryWarehouseSymbol = configuration.Warehouses.FirstOrDefault(x => x.CountriesCodes.Contains(kodKraju))?.ErpWarehouseSymbol;

            var warehouse = configuration.Warehouses.FirstOrDefault(x => x.ChannelWarehouse == magazynNaPozycjiZamowienia && x.CountriesCodes.Contains(kodKraju));
            warehouse = warehouse ?? configuration.Warehouses.FirstOrDefault(x => x.ChannelWarehouse == magazynNaPozycjiZamowienia);
            warehouse = warehouse ?? configuration.Warehouses.FirstOrDefault(x => x.ChannelWarehouse == magazynNaZamowieniu && x.CountriesCodes.Contains(kodKraju));
            warehouse = warehouse ?? configuration.Warehouses.FirstOrDefault(x => x.ChannelWarehouse == magazynNaZamowieniu);

            if (warehouse == null)
            {
                operationResult.Warrnings.Add("Wybrany magazyn nie jest pierwszym wyborem");
            }

            var warehouseSymbol = warehouse?.ErpWarehouseSymbol ?? countryWarehouseSymbol ?? defaultWarehouseSymbol;
            var magazynyMgr = sfera.PodajObiektTypu<IMagazyny>().Dane.Wszystkie();

            var magazyn = magazynyMgr.FirstOrDefault(x => x.Symbol == warehouseSymbol);
            return magazyn;
        }
        private SposobDostawy DopasujSposobDostawy(string sposobDostawyNaZamowieniu, string kodKraju)
        {
            var defaultDeliveryMethodName = configuration.DeliveryMethods.First(x => x.Default).ErpDeliveryMethodName;
            var countryDeliveryMethodName = configuration.DeliveryMethods.FirstOrDefault(x => x.CountriesCodes.Contains(kodKraju))?.ErpDeliveryMethodName;

            var deliveryMethod = configuration.DeliveryMethods.FirstOrDefault(x => x.ChannelDeliveryMethod == sposobDostawyNaZamowieniu && x.CountriesCodes.Contains(kodKraju));
            deliveryMethod = deliveryMethod ?? configuration.DeliveryMethods.FirstOrDefault(x => x.ChannelDeliveryMethod == sposobDostawyNaZamowieniu);

            if (deliveryMethod == null)
            {
                operationResult.Warrnings.Add("Wybrany sposób dostawy nie jest pierwszym wyborem");
            }

            var deliveryMethodName = deliveryMethod?.ErpDeliveryMethodName ?? countryDeliveryMethodName ?? defaultDeliveryMethodName;
            var sposobyDostawMgr = sfera.PodajObiektTypu<ISposobyDostaw>().Dane.Wszystkie();

            var sposobDostawy = sposobyDostawMgr.FirstOrDefault(x => x.Nazwa == deliveryMethodName);
            return sposobDostawy;
        }
        private StatusDokumentu DopasujStatusDokumentu(string magazynNaZamowieniu, string kodKraju)
        {
            var defaultErpDocumentStatus = configuration.Warehouses.First(x => x.Default).ErpDocumentStatus;
            var countryErpDocumentStatus = configuration.Warehouses.FirstOrDefault(x => x.CountriesCodes.Contains(kodKraju))?.ErpDocumentStatus;

            var warehouse = configuration.Warehouses.FirstOrDefault(x => x.ChannelWarehouse == magazynNaZamowieniu && x.CountriesCodes.Contains(kodKraju));
            warehouse = warehouse ?? configuration.Warehouses.FirstOrDefault(x => x.ChannelWarehouse == magazynNaZamowieniu);

            if (warehouse == null)
            {
                operationResult.Warrnings.Add("Wybrany status dokumentu nie jest pierwszym wyborem");
            }

            var erpDocumentStatus = warehouse?.ErpDocumentStatus ?? countryErpDocumentStatus ?? defaultErpDocumentStatus;
            var statusyDokumentowMgr = sfera.PodajObiektTypu<IStatusyDokumentow>().Dane.Wszystkie();

            var statusDokumentu = statusyDokumentowMgr.FirstOrDefault(x => x.Mnemonik == erpDocumentStatus && x.TypyDokumentow == 1);
            return statusDokumentu;
        }
        private JednostkaMiary DopasujJednostkeMiary(string jednostkaNaPozycji, string kodKraju, string ean)
        {
            var defaultUnitSymbol = configuration.Units.First(x => x.Default).ErpUnitSymbol;
            var countryUnitSymbol = configuration.Units.FirstOrDefault(x => x.CountriesCodes.Contains(kodKraju))?.ErpUnitSymbol;

            var unit = configuration.Units.FirstOrDefault(x => x.ChannelUnit == jednostkaNaPozycji && x.CountriesCodes.Contains(kodKraju));
            unit = unit ?? configuration.Units.FirstOrDefault(x => x.ChannelUnit == jednostkaNaPozycji);

            var asor = sfera.PodajObiektTypu<IAsortymenty>().Dane.Wszystkie().FirstOrDefault(x => x.JednostkiMiar.Any(jm => jm.KodyKreskowe.Any(k => k.Kod == ean)));
            var eanUnit = asor?.JednostkiMiar.FirstOrDefault(x => x.KodyKreskowe.Any(k => k.Kod == ean))?.JednostkaMiary?.Symbol;


            if (unit == null)
            {
                operationResult.Warrnings.Add("Wybrany jednostka miary nie jest pierwszym wyborem");
            }

            var unitSymbol = unit?.ErpUnitSymbol ?? eanUnit ?? countryUnitSymbol ?? defaultUnitSymbol;
            var jednostkiMiarMgr = sfera.PodajObiektTypu<IJednostkiMiar>().Dane.Wszystkie();

            var jednostkaMiary = jednostkiMiarMgr.FirstOrDefault(x => x.Symbol == unitSymbol) ?? asor?.JednostkiMiar.First().JednostkaMiary;
            return jednostkaMiary;
        }
        private JednostkaOrganizacyjna DopasujOddzial(string oddzialNaZamowieniu, string kodKraju)
        {
            var defaultBranchSymbol = configuration.Branches.First(x => x.Default).ErpBranchSymbol;
            var countryBranchSymbol = configuration.Branches.FirstOrDefault(x => x.CountriesCodes.Contains(kodKraju))?.ErpBranchSymbol;

            var branch = configuration.Branches.FirstOrDefault(x => x.ChannelBranch == oddzialNaZamowieniu && x.CountriesCodes.Contains(kodKraju));
            branch = branch ?? configuration.Branches.FirstOrDefault(x => x.ChannelBranch == oddzialNaZamowieniu);

            if (branch == null)
            {
                operationResult.Warrnings.Add("Wybrany oddział nie jest pierwszym wyborem");
            }

            var branchSymbol = branch?.ErpBranchSymbol ?? countryBranchSymbol ?? defaultBranchSymbol;
            var oddzialyMgr = sfera.PodajObiektTypu<IOddzialy>().Dane.Wszystkie();
            var centraleMgr = sfera.PodajObiektTypu<ICentrale>().Dane.Wszystkie();

            var oddzial = oddzialyMgr.FirstOrDefault(x => x.Symbol == branchSymbol);
            var centrala = centraleMgr.FirstOrDefault(x => x.Symbol == branchSymbol);

            var jo = (JednostkaOrganizacyjna)oddzial ?? centrala;
            return jo;
        }
        private StawkaVat DopasujStawkeVAT(string stawkaVatNaPozycji, string kodKraju)
        {
            var defaultTaxRate = configuration.TaxRates.First(w => w.Default).ErpTaxRateSymbol;
            var countryTaxRate = configuration.TaxRates.FirstOrDefault(x => x.CountriesCodes.Contains(kodKraju))?.ErpTaxRateSymbol;

            var taxRate = configuration.TaxRates.FirstOrDefault(x => x.ChannelTaxRate == stawkaVatNaPozycji && x.CountriesCodes.Contains(kodKraju));
            taxRate = taxRate ?? configuration.TaxRates.FirstOrDefault(x => x.ChannelTaxRate == stawkaVatNaPozycji);

            if (taxRate == null)
            {
                operationResult.Warrnings.Add("Wybrana stawka nie jest pierwszym wyborem");
            }

            var taxRateSymbol = taxRate?.ErpTaxRateSymbol ?? countryTaxRate ?? defaultTaxRate;
            var stawkiVatMgr = sfera.PodajObiektTypu<IStawkiVat>().Dane.Wszystkie();

            var stawkaVat = stawkiVatMgr.FirstOrDefault(x => x.Symbol == taxRateSymbol);
            return stawkaVat;
        }
        private Asortyment DopasujAsortyment(string asortymentNaZamowieniuSymbol, string ean = null)
        {
            var asortyment = sfera.PodajObiektTypu<IAsortymenty>().Dane.WyszukajPoSymbolu(asortymentNaZamowieniuSymbol);
            var asor = sfera.PodajObiektTypu<IAsortymenty>().Dane.Wszystkie().FirstOrDefault(x => x.JednostkiMiar.Any(jm => jm.KodyKreskowe.Any(k => k.Kod == ean)));
            return asortyment ?? asor;
        }
        private TransakcjaHandlowa DopasujTransakcjeHandlowa(string kodKraju, bool czynnyPodatnikVAT = false)
        {
            // od - miejsce dosstawy 
            // opcja w konfiguracji - dla podatnika

            var defaultTransactionSymbol = configuration.Transactions.First(x => x.Default).ErpTranstactionSymbol;
            var countryTransactionSymbol = configuration.Transactions.FirstOrDefault(x => x.CountriesCodes.Contains(kodKraju))?.ErpTranstactionSymbol;

            var transactionSymbol = countryTransactionSymbol ?? defaultTransactionSymbol;
            var transakcjeHandloweMgr = sfera.PodajObiektTypu<ITransakcjeHandlowe>().Dane.Wszystkie();

            var transakcjaHandlowa = transakcjeHandloweMgr.FirstOrDefault(x => x.Symbol == transactionSymbol);
            return transakcjaHandlowa;
        }
        private PodmiotHistoria DopasujPodmiot(Contractor contractor)
        {
            var podmioty = sfera.PodajObiektTypu<IPodmioty>().Dane.Wszystkie();

            PodmiotHistoria podmiot = null;

            //Szukanie po NIP
            if (!string.IsNullOrEmpty(contractor.VATIN))
            {
                podmiot = podmioty.Where(x => x.NIP == contractor.VATIN || x.NIPUE == contractor.VATIN).FirstOrDefault()?.Aktualny;
            }

            //Szukanie po PESEL
            if (podmiot == null && !string.IsNullOrEmpty(contractor.PESEL) && contractor.Type == ContractorType.Person)
            {
                podmiot = podmioty.Where(x => x.Osoba != null && x.Osoba.PESEL == contractor.PESEL).FirstOrDefault()?.Aktualny;
            }

            //Szukanie po username w kontaktach
            if (podmiot == null && !string.IsNullOrEmpty(contractor.Username))
            {
                podmiot = podmioty.Where(x => x.Kontakty.Any(k => k.Wartosc == contractor.Username)).FirstOrDefault()?.Aktualny;
            }

            return podmiot;
        }
    }
}
