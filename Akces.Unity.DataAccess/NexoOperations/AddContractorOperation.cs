using System;
using System.Linq;
using System.Threading.Tasks;
using InsERT.Moria.Sfera;
using InsERT.Moria.Klienci;
using InsERT.Moria.ModelDanych;
using Akces.Unity.Models.SaleChannels;

namespace Akces.Unity.DataAccess.NexoManagers.Operations
{
    public class AddContractorOperation : INexoOperation<Contractor>
    {
        private readonly NexoOperationResult operationResult;
        private readonly Uchwyt sfera;
        public Contractor Data { get; private set; }

        public AddContractorOperation(Contractor data, Uchwyt sfera)
        {
            Data = data;
            this.sfera = sfera;
            operationResult = new NexoOperationResult();
            operationResult.ObjectName = data.Name;
        }

        public async Task<NexoOperationResult> ExecuteAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    AddContractorToNexoIfNotExists();
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
        private void AddContractorToNexoIfNotExists()
        {
            var podmiotyMgr = sfera.PodajObiektTypu<IPodmioty>();
            var panstwaMgr = sfera.PodajObiektTypu<IPanstwa>();
            var alreadyExists = ZnajdzPodmiot(Data) != null;

            if (alreadyExists)
            {
                operationResult.Infos.Add($"Podmiot {Data.Username} istnieje w nexo");
                operationResult.NoChangesMade = true;
                return;
            }

            using (var podmiotOB = Data.Type == ContractorType.Company ? podmiotyMgr.UtworzFirme() : podmiotyMgr.UtworzOsobe())
            {
                if (Data.Type == ContractorType.Company)
                {
                    podmiotOB.Dane.NazwaSkrocona = Data.Name;
                    podmiotOB.Dane.Firma.Nazwa = Data.FullName;

                    if (!string.IsNullOrEmpty(Data.VATIN))
                    {
                        if (Data.CountryCode == "PL")
                        {
                            podmiotOB.Dane.NIP = Data.VATIN;
                        }
                        else if (Data.VATIN.StartsWith(Data.CountryCode))
                        {
                            podmiotOB.Dane.PanstwoRejestracji = panstwaMgr.Dane.Wszystkie().FirstOrDefault(x => x.KodPanstwaUE == Data.CountryCode);
                            podmiotOB.Dane.NIPUE = Data.VATIN;
                        }
                    }
                }
                else
                {
                    podmiotOB.Dane.Osoba.Imie = Data.FirstName;
                    podmiotOB.Dane.Osoba.Nazwisko = Data.LastName;
                    if (!string.IsNullOrEmpty(Data.SecondName)) podmiotOB.Dane.Osoba.DrugieImie = Data.SecondName;
                    if (!string.IsNullOrEmpty(Data.PESEL)) podmiotOB.Dane.Osoba.PESEL = Data.PESEL;
                }

                if (podmiotOB.Dane.AdresPodstawowy != null)
                {
                    podmiotOB.Dane.AdresPodstawowy.Szczegoly.Ulica = Data.Line1;
                    podmiotOB.Dane.AdresPodstawowy.Szczegoly.KodPocztowy = Data.Line2;
                    podmiotOB.Dane.AdresPodstawowy.Szczegoly.Miejscowosc = Data.Line3;
                    podmiotOB.Dane.AdresPodstawowy.Panstwo = panstwaMgr.Dane.Wszystkie().FirstOrDefault(x => x.KodPanstwaUE == Data.CountryCode);
                }
                else
                {
                    var typAdresu = sfera.PodajObiektTypu<ITypyAdresu>().DaneDomyslne.Glowny;
                    var adresPodstawowy = podmiotOB.DodajAdres(typAdresu);
                    adresPodstawowy.Szczegoly.Ulica = Data.Line1;
                    adresPodstawowy.Szczegoly.KodPocztowy = Data.Line2;
                    adresPodstawowy.Szczegoly.Miejscowosc = Data.Line3;
                    adresPodstawowy.Panstwo = panstwaMgr.Dane.Wszystkie().FirstOrDefault(x => x.KodPanstwaUE == Data.CountryCode);
                }


                if (!string.IsNullOrEmpty(Data.PhoneNumber))
                {
                    var telefon = sfera.PodajObiektTypu<Kontakt>();
                    podmiotOB.Dane.Kontakty.Add(telefon);
                    telefon.Rodzaj = sfera.PodajObiektTypu<IRodzajeKontaktu>().DaneDomyslne.Telefon;
                    telefon.Podstawowy = true;
                    telefon.Wartosc = Data.PhoneNumber;
                }

                if (!string.IsNullOrEmpty(Data.Email))
                {
                    var email = sfera.PodajObiektTypu<Kontakt>();
                    podmiotOB.Dane.Kontakty.Add(email);
                    email.Rodzaj = sfera.PodajObiektTypu<IRodzajeKontaktu>().DaneDomyslne.Email;
                    email.Podstawowy = true;
                    email.Wartosc = Data.Email;
                }

                if (!string.IsNullOrEmpty(Data.Username))
                {
                    var fax = sfera.PodajObiektTypu<Kontakt>();
                    podmiotOB.Dane.Kontakty.Add(fax);
                    fax.Rodzaj = sfera.PodajObiektTypu<IRodzajeKontaktu>().DaneDomyslne.Fax;
                    fax.Podstawowy = true;
                    fax.Wartosc = Data.Username;
                }

                var dodano = podmiotOB.Zapisz();

                if (!dodano)
                {
                    var t = podmiotOB.WypiszBledy();
                    operationResult.Errors.Add(t);
                    throw new Exception("Nie dodano podmiotu");
                }

                operationResult.Infos.Add($"Dodano podmiot [Id: {podmiotOB.Dane.Id}] Numer: {podmiotOB.Dane.NazwaSkrocona}");
            }
        }
        private PodmiotHistoria ZnajdzPodmiot(Contractor contractor)
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
                var rodzajFax = sfera.PodajObiektTypu<IRodzajeKontaktu>().DaneDomyslne.Fax;
                podmiot = podmioty.Where(x => x.Kontakty.Any(k => k.Rodzaj.Id == rodzajFax.Id && k.Wartosc == contractor.Username)).FirstOrDefault()?.Aktualny;
            }

            return podmiot;
        }
    }
}
