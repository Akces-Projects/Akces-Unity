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
            var accountsManager = new AccountsManager();

            switch (account.AccountType)
            {
                case AccountType.Shoper:

                    var shoperConfiguration = account.Id == default ?
                        (account as ShoperAccount).ShoperConfiguration :
                        accountsManager.Get<ShoperAccount>(account.Id)?.ShoperConfiguration;

                    return new ShoperService(shoperConfiguration);
                case AccountType.shopGold:
                    break;
                case AccountType.Baselinker:

                    var baselinkerConfiguration = account.Id == default ?
                        (account as BaselinkerAccount).BaselinkerConfiguration :
                        accountsManager.Get<BaselinkerAccount>(account.Id)?.BaselinkerConfiguration;

                    return new BaselinkerService(baselinkerConfiguration);
                case AccountType.Allegro:

                    var allegroConfiguration = account.Id == default ?
                        (account as AllegroAccount).AllegroConfiguration :
                        accountsManager.Get<AllegroAccount>(account.Id)?.AllegroConfiguration;

                    return new AllegroService(allegroConfiguration);
                default:
                    break;
            }

            return null;
        }
    }
}
