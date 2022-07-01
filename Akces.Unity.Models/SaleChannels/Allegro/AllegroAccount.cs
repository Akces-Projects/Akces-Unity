using Akces.Unity.Models.SaleChannels.Allegro;

namespace Akces.Unity.Models.SaleChannels
{
    public class AllegroAccount : Account
    {
        public virtual AllegroConfiguration AllegroConfiguration { get; set; }

        public AllegroAccount()
        {
            AccountType = AccountType.Allegro;
            AllegroConfiguration = new AllegroConfiguration();
        }
    }
}
