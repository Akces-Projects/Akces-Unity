using Akces.Unity.DataAccess.Managers;
using Akces.Unity.DataAccess.Services;
using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;

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
                    var shoperConfiguration = accountsManager.Get<ShoperAccount>(account.Id)?.ShoperConfiguration;
                    return new ShoperService(shoperConfiguration);
                case AccountType.shopGold:
                    break;
                case AccountType.Baselinker:
                    var baselinkerConfiguration = accountsManager.Get<BaselinkerAccount>(account.Id)?.BaselinkerConfiguration;
                    return new BaselinkerService(baselinkerConfiguration);
                case AccountType.Allegro:
                    break;
                default:
                    break;
            }

            return null;
        }
    }
}
