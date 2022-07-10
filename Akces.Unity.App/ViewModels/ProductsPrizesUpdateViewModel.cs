using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Akces.Wpf.Models;
using Akces.Unity.Models;
using Akces.Unity.Models.Nexo;
using Akces.Unity.DataAccess.Managers;
using Akces.Unity.DataAccess.NexoManagers;
using Akces.Unity.DataAccess;
using System.Windows.Input;
using Akces.Wpf.Helpers;
using Akces.Core.Nexo;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using Akces.Wpf.Extensions;
using System.Collections.Generic;
using Akces.Unity.Models.SaleChannels;
using System;
using Akces.Unity.App.Operations;
using System.Windows;

namespace Akces.Unity.App.ViewModels
{

    public class ProductsPricesUpdateViewModel : ControlViewModel
    {
        private readonly NexoAssortmentManager nexoAssortmentManager;
        private readonly TaskReportsManager reportsManager;
        private readonly AccountsManager accountsManager;
        private List<ProductAssortmentModel> downloadedProducts;
        private ObservableCollection<ProductAssortmentModel> products;

        private bool selectAll;
        private int currentPage = 1;
        private int pageCount = 1;
        private string searchstring;

        public bool SelectAll
        {
            get { return selectAll; }
            set 
            { 
                selectAll = value; 
                OnPropertyChanged();
                OnSelectAll();
            }
        }
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                if (currentPage < 1 || currentPage > pageCount)
                    return;

                currentPage = value;
                OnPropertyChanged();
            }
        }
        public int PageCount
        {
            get { return pageCount; }
            set
            {
                pageCount = value;
                OnPropertyChanged();
            }
        }
        public string Searchstring
        {
            get { return searchstring; }
            set
            {
                searchstring = value;
                OnPropertyChanged(); 
                OnSearchstringChanged();
            }
        }


        public ObservableCollection<PriceList> PriceLists { get; set; }
        public ObservableCollection<Account> Accounts { get; set; }
        public ObservableCollection<ProductAssortmentModel> Products { get => products; set { products = value; OnPropertyChanged(); } }
        public PriceList SelectedPriceList { get; set; }
        public Account SelectedAccount { get; set; }

        public ICommand LoadProductsCommand { get; set; }
        public ICommand OpenModificationWindowCommand { get; set; }
        public ICommand ModifyPricesCommand { get; set; }
        public ICommand FirstPageCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand LastPageCommand { get; set; }

        public ProductsPricesUpdateViewModel(HostViewModel host) : base(host)
        {
            accountsManager = new AccountsManager();
            nexoAssortmentManager = ServicesProvider.GetService<NexoContext>().GetManager<NexoAssortmentManager>();
            var accounts = accountsManager.Get();
            var priceLists = nexoAssortmentManager.GetPriceLists();
            Accounts = new ObservableCollection<Account>(accounts);
            PriceLists = new ObservableCollection<PriceList>(priceLists);
            Products = new ObservableCollection<ProductAssortmentModel>();
            downloadedProducts = new List<ProductAssortmentModel>();
            SelectedAccount = Accounts.FirstOrDefault();
            SelectedPriceList = PriceLists.FirstOrDefault();
            LoadProductsCommand = CreateAsyncCommand(LoadProductsAsync, (err) => Host.ShowError(err), null, true, "Ładowanie produktów...");
            ModifyPricesCommand = CreateAsyncCommand(ModifyPricesAsync, (err) => Host.ShowError(err), null, true, "Aktualizacja cen produktów...");
            OpenModificationWindowCommand = CreateCommand(OpenModificationWindow, (err) => Host.ShowError(err));

            FirstPageCommand = CreateAsyncCommand(async () => { CurrentPage = 1; await LoadProductsAsync(); }, (err) => Host.ShowError(err), (a) => { return CurrentPage > 1; }, true, "Ładowanie produktów...");
            PreviousPageCommand = CreateAsyncCommand(async () => { CurrentPage--; await LoadProductsAsync(); }, (err) => Host.ShowError(err), (a) => { return CurrentPage > 1; }, true, "Ładowanie produktów...");
            NextPageCommand = CreateAsyncCommand(async () => { CurrentPage++; await LoadProductsAsync(); }, (err) => Host.ShowError(err), (a) => { return CurrentPage < PageCount; }, true, "Ładowanie produktów...");
            LastPageCommand = CreateAsyncCommand(async () => { CurrentPage = PageCount; await LoadProductsAsync(); }, (err) => Host.ShowError(err), (a) => { return CurrentPage < PageCount; }, true, "Ładowanie produktów...");
        }

        private async Task LoadProductsAsync() 
        {
            var saleChannelService = SelectedAccount.CreateService();
            var assortments = await nexoAssortmentManager.GetAssortmentsAsync(SelectedPriceList);
            var productsContainer = await saleChannelService.GetProductsAsync(CurrentPage - 1);

            CurrentPage = productsContainer.PageIndex + 1;
            PageCount = productsContainer.PageCount;

            downloadedProducts.Clear();

            foreach (var product in productsContainer.Products)
            {
                var correctSymbol = product.Symbol;

                if (correctSymbol != null) 
                {
                    correctSymbol = new string(product.Symbol.Where(c => char.IsDigit(c)).Take(4).ToArray());
                }

                var assortment = assortments.FirstOrDefault(x => x.Symbol == correctSymbol || x.Name == product.Name || x.Ean == product.EAN);

                var model = new ProductAssortmentModel(product, assortment);

                downloadedProducts.Add(model);
                Products = new ObservableCollection<ProductAssortmentModel>(downloadedProducts);
            }
        }
        private async Task ModifyPricesAsync()
        {
            var result = MessageBox.Show($"Czy na pewno chcesz wykonać aktualizację {Products.Count(x => x.Selected)} pozycji?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
                return;

            var saleChannelService = SelectedAccount.CreateService();

            var productsToModify = Products.Where(x => x.Selected).Select(x => new Product() 
            {
                Id = x.SaleChannelId,
                Name = x.SaleChannelName,
                Currency = x.SaleChannelCurrency,
                Price = x.CurrentPrice
            }).ToList();

            var updateProductPricesOperation = new UpdateProductsPricesTask(SelectedAccount, productsToModify);

            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(500, 120);
            var vm = window.GetHost().UpdateView<OperationsProgressViewModel>();
            vm.Operation = updateProductPricesOperation;
            window.WindowStyle = System.Windows.WindowStyle.None;
            window.Show();
            await vm.RunOperationsAsync();
        }
        private void OpenModificationWindow() 
        {
            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(450,425);
            var vm = window.GetHost().UpdateView<PricesModificationViewModel>();
            vm.Products = Products.Where(x => x.Selected).ToList();
            window.Show();
        }
        private void OnSelectAll()
        {
            foreach (var product in Products)
            {
                product.Selected = SelectAll;
            }
        }
        private void OnSearchstringChanged() 
        {
            if (downloadedProducts == null)
                return;

            var searchstring = Searchstring?.ToLower();
            var filteredProducts = downloadedProducts
                .Where(x => string.IsNullOrEmpty(searchstring) || x.FullName.Contains(searchstring))
                .ToList();

            if (filteredProducts == null)
                return;

            Products = new ObservableCollection<ProductAssortmentModel>(filteredProducts);
        }
    }

    public class ProductAssortmentModel : INotifyPropertyChanged
    {
        private bool selected;
        private decimal currentPrice;

        public bool Selected { get => selected; set { selected = value; OnProperyChanged(); } }
        public decimal CurrentPrice { get => currentPrice; set { currentPrice = Math.Round(value, 2, MidpointRounding.AwayFromZero); OnProperyChanged(); } }
        public decimal OriginalPrice { get; set; }
        public decimal? ErpPrice { get; set; }
        public string SaleChannelId { get; set; }
        public string SaleChannelName { get; set; }
        public string SaleChannelSymbol { get; set; }
        public string SaleChannelEan { get; set; }
        public string SaleChannelQuantity { get; set; }
        public string SaleChannelPrice { get; set; }
        public string SaleChannelCurrency { get; set; }
        public string ErpRegistrationPrice { get; set; }
        public string ErpPriceText { get; set; }
        public string FullName { get; private set; }


        public ProductAssortmentModel(Product product, Assortment assortment)
        {
            SaleChannelId = product.Id;
            SaleChannelName = product.Name;
            SaleChannelSymbol = product.Symbol;
            SaleChannelEan = product.EAN;
            SaleChannelPrice = product.Price.ToString("F2");
            SaleChannelQuantity = product.Quantity.ToString("F2");
            SaleChannelCurrency = product.Currency;
            ErpPrice = assortment?.Price;
            ErpPriceText = assortment?.Price.ToString("F2") ?? "Nie znaleziono";
            ErpRegistrationPrice = assortment?.RegistrationPrice.ToString("F2") ?? "Nie znaleziono";
            OriginalPrice = product.Price;
            CurrentPrice = product.Price;

            FullName = ("" + product.Id + product.Name + product.Symbol + product.EAN).ToLower();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnProperyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
