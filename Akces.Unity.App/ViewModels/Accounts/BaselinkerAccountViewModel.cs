using Akces.Wpf.Models;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess.Managers.BusinessObjects;

namespace Akces.Unity.App.ViewModels
{
    internal class BaselinkerAccountViewModel : AccountViewModel<BaselinkerAccount>
    {
        private IAccount<BaselinkerAccount> account;
        public override IAccount<BaselinkerAccount> Account { get => account; set { account = value; OnPropertyChanged(); } }
        public BaselinkerAccountViewModel(HostViewModel host) : base(host) { }
    }
}
