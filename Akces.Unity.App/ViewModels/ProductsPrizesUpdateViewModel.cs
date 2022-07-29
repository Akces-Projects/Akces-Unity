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
            LoadProductsCommand = CreateAsyncCommand(LoadProductsAsync, (err) => Host.ShowError(err), null, true, "Ładowanie cen...");
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
            downloadedProducts.Clear();

            var selectedAccounts = Accounts.Where(x => x.Selected).Select(x => x.Item);

            foreach (var selectedAccount in selectedAccounts)
            {
                var saleChannelService = selectedAccount.CreateMainService();
                await saleChannelService.ValidateConnectionAsync();
                var productsContainer = await saleChannelService.GetProductsAsync(all: true);
                CurrentPage = productsContainer.PageIndex + 1;
                PageCount = productsContainer.PageCount;

                foreach (var product in productsContainer.Products)
                {
                    var model = new ProductAssortmentModel(product, null, selectedAccount);
                    downloadedProducts.Add(model);
                }
            }

            Products = new ObservableCollection<ProductAssortmentModel>(downloadedProducts.OrderBy(x => x.Product.Symbol));

            await LoadPricesAsync();
        }
        private async Task ModifyPricesAsync()
        {
            var result = MessageBox.Show($"Czy na pewno chcesz wykonać aktualizację {Products.Count(x => x.Selected)} pozycji?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
                return;

            var selectedAccounts = Accounts.Where(x => x.Selected).Select(x => x.Item);

            foreach (var selectedAccount in selectedAccounts)
            {
                var saleChannelService = selectedAccount.CreateMainService();

                var productsToModify = Products
                    .Where(x => x.Selected && x.Account.Id == selectedAccount.Id)
                    .Select(x => new Product()
                    {
                        Id = x.Product.Id,
                        Name = x.Product.Name,
                        Currency = x.Product.Currency,
                        OriginalPrice = x.Product.OriginalPrice,
                        Price = x.CurrentPrice
                    }).ToList();

                var updateProductPricesTask = new UpdateProductsPricesTask(selectedAccount, productsToModify);

                var window = Host.CreateWindow<ExtraWindow, MainViewModel>(500, 120);
                var vm = window.GetHost().UpdateView<OperationsProgressViewModel>();
                vm.Operation = updateProductPricesTask;
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
                    filteredProducts = downloadedProducts.Where(x => string.IsNullOrEmpty(Searchstring) || x.Product.Name.Contains(Searchstring)).ToList();
                    break;
                case SearchMethod.EAN:
                    filteredProducts = downloadedProducts.Where(x => string.IsNullOrEmpty(Searchstring) || x.Product.EAN.Contains(Searchstring)).ToList();
                    break;
                case SearchMethod.Id:
                    filteredProducts = downloadedProducts.Where(x => string.IsNullOrEmpty(Searchstring) || x.Product.Id.Contains(Searchstring)).ToList();
                    break;
                case SearchMethod.Symbol:
                    filteredProducts = downloadedProducts.Where(x => string.IsNullOrEmpty(Searchstring) || x.Product.Symbol.Contains(Searchstring)).ToList();
                    break;
                case SearchMethod.Konto:
                    filteredProducts = downloadedProducts.Where(x => string.IsNullOrEmpty(Searchstring) || x.Account.Name.Contains(Searchstring)).ToList();
                    break;
                default:
                    break;
            }

            if (filteredProducts == null)
                return;

            Products = new ObservableCollection<ProductAssortmentModel>(filteredProducts.OrderBy(x => x.Product.Symbol));
        }
        private async Task LoadPricesAsync()
        {
            if (downloadedProducts.Count == 0)
                return;

            var assortments = await nexoAssortmentManager.GetAssortmentsAsync(SelectedPriceList);           

            Parallel.ForEach(Products, (model) =>
            {
                try
                {
                    var assortment = assortments.FirstOrDefault(x => x.Symbol == model.Product.Symbol || x.Ean == model.Product.EAN);
                    model.Assortment = assortment;
                }
                catch
                {
                }
            });
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
        private Assortment assortment;

        public bool Selected { get => selected; set { selected = value; OnProperyChanged(); } }
        public decimal CurrentPrice { get => currentPrice; set { currentPrice = Math.Round(value, 2, MidpointRounding.AwayFromZero); OnProperyChanged(); } }
        public decimal? NexoPrice { get => Assortment?.Price; }
        public decimal? NexoRegistrationPrice { get => Assortment?.RegistrationPrice; }
        public string FullName { get; private set; }

        public Product Product { get; set; }
        public Assortment Assortment { get => assortment; set { assortment = value; OnProperyChanged(nameof(NexoPrice)); OnProperyChanged(nameof(NexoRegistrationPrice)); } }
        public Account Account { get; set; }


        public ProductAssortmentModel(Product product, Assortment assortment, Account account)
        {
            CurrentPrice = product.Price;
            Assortment = assortment;
            Account = account;
            Product = product;

            FullName = ("" + account.Name + product.Id + product.Name + product.Symbol + product.EAN).ToLower();
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
