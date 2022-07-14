using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Akces.Core.Nexo;
using Akces.Wpf.Helpers;
using Akces.Wpf.Extensions;
using Akces.Wpf.Models;
using Akces.Unity.Models;
using Akces.Unity.Models.Nexo;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess.Managers;
using Akces.Unity.DataAccess.NexoManagers;
using Akces.Unity.DataAccess;
using Akces.Unity.App.Operations;

namespace Akces.Unity.App.ViewModels
{
    public class ProductsPricesUpdateViewModel : ControlViewModel
    {
        private readonly NexoAssortmentManager nexoAssortmentManager;
        private readonly AccountsManager accountsManager;
        private readonly List<ProductAssortmentModel> downloadedProducts;
        private ObservableCollection<ProductAssortmentModel> products;

        private bool selectAll;
        private int currentPage = 1;
        private int pageCount = 1;
        private string searchstring;
        private SearchMethod searchMethod;

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
        public SearchMethod SearchMethod
        {
            get { return searchMethod; }
            set { searchMethod = value; OnPropertyChanged(); }
        }

        public ObservableCollection<SearchMethod> SearchMethods { get; set; }
        public ObservableCollection<PriceList> PriceLists { get; set; }
        public ObservableCollection<SelectableItem<Account>> Accounts { get; set; }
        public ObservableCollection<ProductAssortmentModel> Products { get => products; set { products = value; OnPropertyChanged(); } }
        public PriceList SelectedPriceList { get; set; }

        public ICommand LoadProductsCommand { get; set; }
        public ICommand LoadPricesCommand { get; set; }
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
            var accounts = accountsManager.Get().Select(x => new SelectableItem<Account>() { Item = x }).ToList();
            var priceLists = nexoAssortmentManager.GetPriceLists();
            Accounts = new ObservableCollection<SelectableItem<Account>>(accounts);
            PriceLists = new ObservableCollection<PriceList>(priceLists);
            Products = new ObservableCollection<ProductAssortmentModel>();
            downloadedProducts = new List<ProductAssortmentModel>();
            SelectedPriceList = PriceLists.FirstOrDefault();
            LoadProductsCommand = CreateAsyncCommand(LoadProductsAsync, (err) => Host.ShowError(err), null, true, "Ładowanie produktów...");
            ModifyPricesCommand = CreateAsyncCommand(ModifyPricesAsync, (err) => Host.ShowError(err), null, true, "Aktualizacja cen produktów...");
            OpenModificationWindowCommand = CreateCommand(OpenModificationWindow, (err) => Host.ShowError(err));
            SearchMethods = new ObservableCollection<SearchMethod>(Enum.GetValues(typeof(SearchMethod)).Cast<SearchMethod>());

            LoadPricesCommand = CreateAsyncCommand(LoadPricesAsync, (err) => Host.ShowError(err), null, true, "Pobieranie cen...");
            FirstPageCommand = CreateAsyncCommand(async () => { CurrentPage = 1; await LoadProductsAsync(); }, (err) => Host.ShowError(err), (a) => { return CurrentPage > 1; }, true, "Ładowanie produktów...");
            PreviousPageCommand = CreateAsyncCommand(async () => { CurrentPage--; await LoadProductsAsync(); }, (err) => Host.ShowError(err), (a) => { return CurrentPage > 1; }, true, "Ładowanie produktów...");
            NextPageCommand = CreateAsyncCommand(async () => { CurrentPage++; await LoadProductsAsync(); }, (err) => Host.ShowError(err), (a) => { return CurrentPage < PageCount; }, true, "Ładowanie produktów...");
            LastPageCommand = CreateAsyncCommand(async () => { CurrentPage = PageCount; await LoadProductsAsync(); }, (err) => Host.ShowError(err), (a) => { return CurrentPage < PageCount; }, true, "Ładowanie produktów...");
        }

        private async Task LoadProductsAsync()
        {
            var assortments = await nexoAssortmentManager.GetAssortmentsAsync(SelectedPriceList);
            downloadedProducts.Clear();

            var selectedAccounts = Accounts.Where(x => x.Selected).Select(x => x.Item);

            foreach (var selectedAccount in selectedAccounts)
            {
                var saleChannelService = selectedAccount.CreateService();
                await saleChannelService.ValidateConnectionAsync();
                var productsContainer = await saleChannelService.GetProductsAsync(all: true);
                CurrentPage = productsContainer.PageIndex + 1;
                PageCount = productsContainer.PageCount;

                foreach (var product in productsContainer.Products)
                {
                    var correctSymbol = product.Symbol == null ? "" : new string(product.Symbol.Where(c => char.IsDigit(c)).Take(4).ToArray());
                    var assortment = assortments.FirstOrDefault(x => x.Symbol == correctSymbol || x.Name == product.Name || x.Ean == product.EAN);
                    var model = new ProductAssortmentModel(product, assortment, selectedAccount.Name);
                    downloadedProducts.Add(model);
                }
            }

            Products = new ObservableCollection<ProductAssortmentModel>(downloadedProducts.OrderBy(x => x.SaleChannelSymbol));
        }
        private async Task ModifyPricesAsync()
        {
            var result = MessageBox.Show($"Czy na pewno chcesz wykonać aktualizację {Products.Count(x => x.Selected)} pozycji?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
                return;

            var selectedAccounts = Accounts.Where(x => x.Selected).Select(x => x.Item);

            foreach (var selectedAccount in selectedAccounts)
            {
                var saleChannelService = selectedAccount.CreateService();

                var productsToModify = Products
                    .Where(x => x.Selected && x.SourceName == selectedAccount.Name)
                    .Select(x => new Product()
                    {
                        Id = x.SaleChannelId,
                        Name = x.SaleChannelName,
                        Currency = x.SaleChannelCurrency,
                        OriginalPrice = x.OriginalPrice,
                        Price = x.CurrentPrice
                    }).ToList();

                var updateProductPricesOperation = new UpdateProductsPricesTask(selectedAccount, productsToModify);

                var window = Host.CreateWindow<ExtraWindow, MainViewModel>(500, 120);
                var vm = window.GetHost().UpdateView<OperationsProgressViewModel>();
                vm.Operation = updateProductPricesOperation;
                window.Title = $"Modyfikacja cen - {selectedAccount.Name} ({selectedAccount.AccountType})";
                window.WindowStyle = WindowStyle.None;
                window.Show();
                await vm.RunOperationsAsync();
            }
        }
        private void OpenModificationWindow() 
        {
            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(450,455);
            window.Title = "Zbiorcza modyfikacja cen";
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

            List<ProductAssortmentModel> filteredProducts = null;

            switch (SearchMethod)
            {
                case SearchMethod.Wszystko:
                    var searchstring = Searchstring?.ToLower();
                    filteredProducts = downloadedProducts.Where(x => string.IsNullOrEmpty(searchstring) || x.FullName.Contains(searchstring)).ToList();
                    break;
                case SearchMethod.Nazwa:
                    filteredProducts = downloadedProducts.Where(x => string.IsNullOrEmpty(Searchstring) || x.SaleChannelName.Contains(Searchstring)).ToList();
                    break;
                case SearchMethod.EAN:
                    filteredProducts = downloadedProducts.Where(x => string.IsNullOrEmpty(Searchstring) || x.SaleChannelEan.Contains(Searchstring)).ToList();
                    break;
                case SearchMethod.Id:
                    filteredProducts = downloadedProducts.Where(x => string.IsNullOrEmpty(Searchstring) || x.SaleChannelId.Contains(Searchstring)).ToList();
                    break;
                case SearchMethod.Symbol:
                    filteredProducts = downloadedProducts.Where(x => string.IsNullOrEmpty(Searchstring) || x.SaleChannelSymbol.Contains(Searchstring)).ToList();
                    break;
                case SearchMethod.Konto:
                    filteredProducts = downloadedProducts.Where(x => string.IsNullOrEmpty(Searchstring) || x.SourceName.Contains(Searchstring)).ToList();
                    break;
                default:
                    break;
            }

            if (filteredProducts == null)
                return;

            Products = new ObservableCollection<ProductAssortmentModel>(filteredProducts.OrderBy(x => x.SaleChannelSymbol));
        }
        private async Task LoadPricesAsync()
        {
            if (downloadedProducts.Count == 0)
                return;

            var assortments = await nexoAssortmentManager.GetAssortmentsAsync(SelectedPriceList);

            if (assortments == null)
                return;

            foreach (var product in Products)
            {
                string correctSymbol;

                if (product.SaleChannelSymbol == null) 
                {
                    correctSymbol = "";
                }
                else 
                {
                    var countDigits = product.SaleChannelSymbol.Count(c => char.IsDigit(c));
                    correctSymbol = new string(product.SaleChannelSymbol.Where(c => char.IsDigit(c)).Take(countDigits < 4 ? countDigits : 4).ToArray());
                }

                var assortment = assortments.FirstOrDefault(x => x.Symbol == correctSymbol || x.Name == product.SaleChannelName || x.Ean == product.SaleChannelEan);

                product.NexoPrice = assortment?.Price;
                product.NexoRegistrationPrice = assortment?.RegistrationPrice;
            }
        }
    }

    public class SelectableItem<T> 
    {
        public bool Selected { get; set; }
        public T Item { get; set; }
    }

    public class ProductAssortmentModel : INotifyPropertyChanged
    {
        private bool selected;
        private decimal currentPrice;
        private decimal? nexoPrice;
        private decimal? nexoRegistrationPrice;

        public bool Selected { get => selected; set { selected = value; OnProperyChanged(); } }
        public decimal CurrentPrice { get => currentPrice; set { currentPrice = Math.Round(value, 2, MidpointRounding.AwayFromZero); OnProperyChanged(); } }
        public decimal OriginalPrice { get; set; }
        public decimal? NexoPrice { get => nexoPrice; set { nexoPrice = value; OnProperyChanged(); } }
        public decimal? NexoRegistrationPrice { get => nexoRegistrationPrice; set { nexoRegistrationPrice = value; OnProperyChanged(); } }
        public string SaleChannelId { get; set; }
        public string SaleChannelName { get; set; }
        public string SaleChannelSymbol { get; set; }
        public string SaleChannelEan { get; set; }
        public string SaleChannelQuantity { get; set; }
        public string SaleChannelPrice { get; set; }
        public string SaleChannelCurrency { get; set; }
        public string SourceName { get; private set; }
        public string FullName { get; private set; }


        public ProductAssortmentModel(Product product, Assortment assortment, string sourceName)
        {
            SaleChannelId = product.Id;
            SaleChannelName = product.Name;
            SaleChannelSymbol = product.Symbol;
            SaleChannelEan = product.EAN;
            SaleChannelPrice = product.Price.ToString("F2");
            SaleChannelQuantity = product.Quantity.ToString("F2");
            SaleChannelCurrency = product.Currency;
            OriginalPrice = product.Price;
            CurrentPrice = product.Price;
            SourceName = sourceName;

            NexoPrice = assortment?.Price;
            NexoRegistrationPrice = assortment?.RegistrationPrice;

            FullName = ("" + sourceName + product.Id + product.Name + product.Symbol + product.EAN).ToLower();
            SourceName = sourceName;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnProperyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum SearchMethod 
    {
        Wszystko, Nazwa, EAN, Id, Symbol, Konto
    }
}
