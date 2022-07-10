
namespace Akces.Unity.Models.SaleChannels
{
    public class OlxAccount : Account
    {
        public virtual OlxConfiguration OlxConfiguration { get; set; }

        public OlxAccount()
        {
            AccountType = AccountType.Olx;
            OlxConfiguration = new OlxConfiguration();
        }
    }
}
