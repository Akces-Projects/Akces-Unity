using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess.Managers;
using Akces.Unity.DataAccess.Services;
using Akces.Unity.DataAccess.Services.Shopgold;
using Akces.Unity.Models.SaleChannels.Olx;

namespace Akces.Unity.DataAccess
{
    public static class UnityExtensions 
    {
        public static ISaleChannelService CreateService(this Account account) 
        {
            var accountsManager = new AccountsManager();

            switch (account.AccountType)
            {
                case AccountType.Shoper:

                    var shoperConfiguration = account.Id == default ?
                        (account as ShoperAccount).ShoperConfiguration :
                        accountsManager.Get<ShoperAccount>(account.Id)?.ShoperConfiguration;

                    (account as ShoperAccount).ShoperConfiguration = shoperConfiguration;
                    return new ShoperService(shoperConfiguration);
                case AccountType.shopGold:

                    var shopgoldConfiguration = account.Id == default ?
                       (account as ShopgoldAccount).ShopgoldConfiguration :
                       accountsManager.Get<ShopgoldAccount>(account.Id)?.ShopgoldConfiguration;

                    (account as ShopgoldAccount).ShopgoldConfiguration = shopgoldConfiguration;
                    return new ShopgoldService(shopgoldConfiguration);
                case AccountType.Baselinker:

                    var baselinkerConfiguration = account.Id == default ?
                        (account as BaselinkerAccount).BaselinkerConfiguration :
                        accountsManager.Get<BaselinkerAccount>(account.Id)?.BaselinkerConfiguration;

                    (account as BaselinkerAccount).BaselinkerConfiguration = baselinkerConfiguration;
                    return new BaselinkerService(baselinkerConfiguration);
                case AccountType.Allegro:

                    var allegroConfiguration = account.Id == default ?
                        (account as AllegroAccount).AllegroConfiguration :
                        accountsManager.Get<AllegroAccount>(account.Id)?.AllegroConfiguration;

                    (account as AllegroAccount).AllegroConfiguration = allegroConfiguration;
                    return new AllegroService(allegroConfiguration);
                case AccountType.Olx:

                    var olxConfiguration = account.Id == default ?
                        (account as OlxAccount).OlxConfiguration :
                        accountsManager.Get<OlxAccount>(account.Id)?.OlxConfiguration;

                    (account as OlxAccount).OlxConfiguration = olxConfiguration;
                    return new OlxService(olxConfiguration);
                default:
                    break;
            }

            return null;
        }
    }
}
