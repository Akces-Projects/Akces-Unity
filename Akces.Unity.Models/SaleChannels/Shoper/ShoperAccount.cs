
using Akces.Unity.Models.SaleChannels.Shoper;

namespace Akces.Unity.Models.SaleChannels
{
    public class ShoperAccount : Account
    {
        public virtual ShoperConfiguration ShoperConfiguration { get; set; }

        public ShoperAccount()
        {
            AccountType = AccountType.Shoper;
            ShoperConfiguration = new ShoperConfiguration();
        }
    }
}
