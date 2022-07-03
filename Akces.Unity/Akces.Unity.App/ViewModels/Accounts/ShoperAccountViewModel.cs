using Akces.Wpf.Models;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess.Managers.BusinessObjects;

namespace Akces.Unity.App.ViewModels
{
    internal class ShoperAccountViewModel : AccountViewModel<ShoperAccount>
    {
        private IAccount<ShoperAccount> account;
        public override IAccount<ShoperAccount> Account { get => account; set { account = value; OnPropertyChanged(); } }
        public ShoperAccountViewModel(HostViewModel host) : base(host) { }
    }
}
