using Akces.Unity.DataAccess.Services;
using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;

namespace Akces.Unity.DataAccess
{
    public static class UnityExtensions 
    {
        public static ISaleChannelService CreateService(this Account account) 
        {
            switch (account.AccountType)
            {
                case AccountType.Shoper:
                    return new ShoperService((account as ShoperAccount).ShoperConfiguration);
                case AccountType.shopGold:
                    break;
                case AccountType.Baselinker:
                    return new BaselinkerService((account as BaselinkerAccount).BaselinkerConfiguration);
                case AccountType.Allegro:
                    break;
                default:
                    break;
            }

            return null;
        }
    }
}
