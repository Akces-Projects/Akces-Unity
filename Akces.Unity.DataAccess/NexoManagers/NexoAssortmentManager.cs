using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using InsERT.Moria.Sfera;
using InsERT.Moria.Asortymenty;
using InsERT.Moria.CennikiICeny;
using Akces.Unity.Models.Nexo;

namespace Akces.Unity.DataAccess.NexoManagers
{
    public class NexoAssortmentManager 
    {
        private readonly Uchwyt sfera;

        public NexoAssortmentManager(Uchwyt sfera)
        {
            this.sfera = sfera;
        }

        public List<PriceList> GetPriceLists()
        {
            var priceLists = sfera.PodajObiektTypu<ICenniki>().Dane
                .Wszystkie()
                .Select(x => new PriceList()
                {
                    Id = x.Id,
                    Name = x.Tytul
                }).ToList();

            return priceLists;
        }
        public async Task<List<Assortment>> GetAssortmentsAsync(PriceList pricesFrom)
        {
            var assortments = await Task.Run(() =>
            {
                return sfera.PodajObiektTypu<IAsortymenty>().Dane
                    .Wszystkie()
                    .Select(x => new Assortment()
                    {
                        Id = x.Id,
                        Symbol = x.Symbol,
                        RegistrationPrice = x.CenaEwidencyjna,
                        Name = x.Nazwa,
                        Ean = x.PodstawowaJednostkaMiaryAsortymentu.PodstawowyKodKreskowy == null ? null : x.PodstawowaJednostkaMiaryAsortymentu.PodstawowyKodKreskowy.Kod,
                        Price =  x.PozycjeCennika.FirstOrDefault(p => p.Cennik != null && p.Cennik.Id == pricesFrom.Id).CenaBrutto,

                    })
                    .ToList();
            });

            return assortments;
        }
    }
}
