using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess.Managers;
using Akces.Unity.DataAccess.Services;

namespace Akces.Unity.DataAccess
{
    public static class UnityExtensions 
    {
        public static ISaleChannelService CreateService(this Account account) 
        {
            switch (account.AccountType)
            {
                case AccountType.Shoper:
                    var shoperConfiguration = (account as ShoperAccount).ShoperConfiguration;
                    return new ShoperService(shoperConfiguration);
                case AccountType.shopGold:
                    var shopgoldConfiguration = (account as ShopgoldAccount).ShopgoldConfiguration;
                    return new ShopgoldService(shopgoldConfiguration);
                case AccountType.Baselinker:
                    var baselinkerConfiguration = (account as BaselinkerAccount).BaselinkerConfiguration;
                    return new BaselinkerService(baselinkerConfiguration);
                case AccountType.Allegro:
                    var allegroConfiguration = (account as AllegroAccount).AllegroConfiguration;
                    return new AllegroService(allegroConfiguration);
                case AccountType.Olx:
                    var olxConfiguration = (account as OlxAccount).OlxConfiguration;
                    return new OlxService(olxConfiguration);
                default:
                    break;
            }

            return null;
        }
    }
}
