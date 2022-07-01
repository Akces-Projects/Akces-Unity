
using Akces.Unity.Models.SaleChannels.Baselinker;

namespace Akces.Unity.Models.SaleChannels
{
    public class BaselinkerAccount : Account
    {
        public virtual BaselinkerConfiguration BaselinkerConfiguration { get; set; }

        public BaselinkerAccount()
        {
            AccountType = AccountType.Baselinker;
            BaselinkerConfiguration = new BaselinkerConfiguration();
        }
    }
}
