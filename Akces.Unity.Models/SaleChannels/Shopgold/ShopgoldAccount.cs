
using Akces.Unity.Models.SaleChannels.Shopgold;

namespace Akces.Unity.Models.SaleChannels
{
    public class ShopgoldAccount : Account
    {
        public virtual ShopgoldConfiguration ShopgoldConfiguration { get; set; }

        public ShopgoldAccount()
        {
            AccountType = AccountType.shopGold;
            ShopgoldConfiguration = new ShopgoldConfiguration();
        }
    }
}
