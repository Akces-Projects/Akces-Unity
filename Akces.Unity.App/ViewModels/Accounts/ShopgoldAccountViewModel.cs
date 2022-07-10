using Akces.Wpf.Models;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess.Managers.BusinessObjects;

namespace Akces.Unity.App.ViewModels
{
    internal class ShopgoldAccountViewModel : AccountViewModel<ShopgoldAccount>
    {
        private IAccount<ShopgoldAccount> account;
        public override IAccount<ShopgoldAccount> Account { get => account; set { account = value; OnPropertyChanged(); } }
        public ShopgoldAccountViewModel(HostViewModel host) : base(host) { }
    }
}
