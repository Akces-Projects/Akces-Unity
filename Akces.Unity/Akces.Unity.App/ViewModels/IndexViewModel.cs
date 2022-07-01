using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Akces.Wpf.Models;
using Akces.Wpf.Helpers;
using Akces.Wpf.Extensions;
using Akces.Unity.Core;
using Akces.Unity.Core.SaleChannels;
using Akces.Unity.Core.NexoData.Operations;
using System.Windows.Threading;

namespace Akces.Unity.App.ViewModels
{
    public class IndexViewModel : ControlViewModel
    {
        private readonly UnityService unityService;
        private string activeHarmonogramName;
        private bool harmonogramCreationDialogOpened;
        private bool accountCreationDialogOpened;
        private Harmonogram selectedHarmonogram;

        public ObservableCollection<Account> Accounts { get; set; }
        public ObservableCollection<Harmonogram> Harmonograms { get; set; }
        public ObservableCollection<HarmonogramPosition> HarmonogramPositions { get; set; }
        public string ActiveHarmonogramName { get => activeHarmonogramName; set { activeHarmonogramName = value; OnPropertyChanged(); } }
        public bool HarmonogramCreationDialogOpened { get => harmonogramCreationDialogOpened; set { harmonogramCreationDialogOpened = value; OnPropertyChanged(); } }
        public bool AccountCreationDialogOpened { get => accountCreationDialogOpened; set { accountCreationDialogOpened = value; OnPropertyChanged(); } }
        public Harmonogram SelectedHarmonogram 
        {
            get => selectedHarmonogram; 
            set 
            { 
                OnHarmonogramSelected(value); 
                OnPropertyChanged();
            } 
        }

        public ICommand ShowAccountCommand { get; set; }
        public ICommand ShowNexoConfigurationCommand { get; set; }
        public ICommand ShowAccountCreationCommand { get; set; }
        public ICommand HideAccountCreationCommand { get; set; }

        public ICommand ShowHarmonogramCreationCommand { get; set; }
        public ICommand HideHarmonogramCreationCommand { get; set; }
        public ICommand CreateHarmonogramCommand { get; set; }
        public ICommand SaveHarmonogramCommand { get; set; }
        public ICommand SetActiveHarmonogramCommand { get; set; }
        public ICommand DeleteHarmonogramCommand { get; set; }
        public ICommand DeleteAccountCommand { get; set; }
        public ICommand AddHarmonogramPositionCommand { get; set; }
        public ICommand RemoveHarmonogramPositionCommand { get; set; }
        public ICommand ShowRaportCommand { get; set; }


        public IndexViewModel(HostViewModel host) : base(host)
        {
            unityService = ServicesProvider.GetService<UnityService>();

            Accounts = new ObservableCollection<Account>();
            Harmonograms = new ObservableCollection<Harmonogram>();
            HarmonogramPositions = new ObservableCollection<HarmonogramPosition>();

            ShowAccountCommand = base.CreateCommand<ISaleChannelAccount>(ShowAccount);
            ShowNexoConfigurationCommand = base.CreateCommand<Account>(ShowNexoConfiguration, (err) => Host.ShowError(err));

            CreateHarmonogramCommand = base.CreateCommand<string>(CreateHarmonogram, (err) => Host.ShowWarning(err));
            SaveHarmonogramCommand = base.CreateCommand(SaveHarmonogram, (err) => Host.ShowWarning(err));
            SetActiveHarmonogramCommand = base.CreateCommand(SetActiveHarmonogram, (err) => Host.ShowWarning(err));
            DeleteHarmonogramCommand = base.CreateCommand(DeleteHarmonogram, (err) => Host.ShowWarning(err));

            ShowHarmonogramCreationCommand = base.CreateCommand(() => HarmonogramCreationDialogOpened = true);
            HideHarmonogramCreationCommand = base.CreateCommand(() => HarmonogramCreationDialogOpened = false);
            ShowAccountCreationCommand = base.CreateCommand(() => AccountCreationDialogOpened = true);
            HideAccountCreationCommand = base.CreateCommand(() => AccountCreationDialogOpened = false);

            AddHarmonogramPositionCommand = base.CreateCommand(AddHarmonogramPosition);
            RemoveHarmonogramPositionCommand = base.CreateCommand<HarmonogramPosition>(RemoveHarmonogramPosition);
            ShowRaportCommand = base.CreateCommand<HarmonogramPosition>(ShowRaport);

            unityService.OnOrdersImported += OnOrdersImported;

            LoadHarmonograms();
            LoadAccounts();
        }

        private void ShowNexoConfiguration(Account saleChannelAccount)
        {
            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(1100, 500);
            var vm = window.GetHost().UpdateView<NexoConfigurationViewModel>();
            vm.Configuration = saleChannelAccount.NexoConfiguration;
            vm.LoadConfigurationMembers();
            vm.LoadNexoOptions();
            window.Title = $"{saleChannelAccount.Name} ({saleChannelAccount.SaleChannelType}) - Opcje dla Subiekt nexo"; 
            window.Show();
        }

        private void LoadAccounts()
        {
            var accounts = unityService.SaleChannelAccountsManager.GetAccounts();
            RefreshCollection(Accounts, accounts);
        }
        private void LoadHarmonograms()
        {
            var harmonograms = unityService.HarmonogramsManager.GetHarmonograms();
            RefreshCollection(Harmonograms, harmonograms);
            SelectedHarmonogram = Harmonograms.FirstOrDefault(x => x.Active);
            ActiveHarmonogramName = Harmonograms.FirstOrDefault(x => x.Active)?.Name;
        }
        private void ShowAccount(ISaleChannelAccount saleChannel)
        {
            Host.ShowInfo(saleChannel.Name);
        }
        public void CreateAccount(string name, SaleChannelType saleChannelType)
        {
            try
            {
                var account = unityService.SaleChannelAccountsManager.CreateAccount(saleChannelType);
                account.Name = name;
                unityService.SaleChannelAccountsManager.SaveAccount(account);
                account = unityService.SaleChannelAccountsManager.GetAccount(account.Id);
                Accounts.Add(account);
                AccountCreationDialogOpened = false;
            }
            catch (Exception e)
            {
                Host.ShowWarning(e.Message);
            }
        }
        private void DeleteAccount(Account saleChannelAccount)
        {
            if (saleChannelAccount == null)
                return;

            var result = MessageBox.Show("Czy usunąć konto?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                unityService.SaleChannelAccountsManager.DeleteAccount(saleChannelAccount);
                Accounts.Remove(saleChannelAccount);
            }
        }

        private void OnHarmonogramSelected(Harmonogram harmonogram)
        {
            selectedHarmonogram = harmonogram;

            if (selectedHarmonogram == null)
                return;

            var original = unityService.HarmonogramsManager.GetHarmonogram(harmonogram.Id);

            selectedHarmonogram.Id = original.Id;
            selectedHarmonogram.Name = original.Name;
            selectedHarmonogram.Active = original.Active;
            selectedHarmonogram.Created = original.Created;
            selectedHarmonogram.Modified = original.Modified;
            selectedHarmonogram.HarmonogramPositions = original.HarmonogramPositions;

            RefreshCollection(HarmonogramPositions, selectedHarmonogram.HarmonogramPositions);
        }
        private void CreateHarmonogram(string name)
        {
            var harmonogram = unityService.HarmonogramsManager.CreateHamonogram();
            harmonogram.Name = name;
            unityService.HarmonogramsManager.SaveHarmonogram(harmonogram);
            Harmonograms.Add(harmonogram);
            SelectedHarmonogram = harmonogram;
            HarmonogramCreationDialogOpened = false;
        }
        private void SaveHarmonogram()
        {
            if (SelectedHarmonogram == null)
                return;

            var result = MessageBox.Show("Czy zapisać harmonogram?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                unityService.HarmonogramsManager.SaveHarmonogram(SelectedHarmonogram);
        }
        private void SetActiveHarmonogram()
        {
            if (SelectedHarmonogram == null)
                return;

            var result = MessageBox.Show("Czy ustawić harmonogram jako aktywny?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                unityService.SetActiveHarmonogram(SelectedHarmonogram);
                ActiveHarmonogramName = SelectedHarmonogram.Name;
            }
        }
        private void DeleteHarmonogram()
        {
            if (SelectedHarmonogram == null)
                return;

            if (SelectedHarmonogram.Active) 
            {
                MessageBox.Show("Nie można usunąć aktywnego harmonogramu", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Czy usunąć harmonogram?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                unityService.HarmonogramsManager.DeleteHarmonogram(SelectedHarmonogram);
                Harmonograms.Remove(SelectedHarmonogram);
                SelectedHarmonogram = Harmonograms.FirstOrDefault();
            }
        }
        private void AddHarmonogramPosition()
        {
            if (SelectedHarmonogram == null)
                return;

            var position = new HarmonogramPosition()
            {
                StartTime = DateTime.Now.AddHours(1),
                Account = Accounts.FirstOrDefault(),
                AccountId = Accounts.FirstOrDefault()?.Id,
                Active = false,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Description = "",
                HarmonogramOperation = HarmonogramOperation.Import,
                LastLaunchTime = null,
                Repeat = false,
                RepeatAfterMinutes = 10,
                Results = new List<OperationResult>()
            };

            SelectedHarmonogram.HarmonogramPositions.Add(position);
            HarmonogramPositions.Add(position);
        }
        private void ShowRaport(HarmonogramPosition harmonogramPosition) 
        {
            if (harmonogramPosition.Results == null)
                return;

            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(1200,600);
            var vm = window.GetHost().UpdateView<RaportViewModel>();
            vm.OperationResults.Clear();

            foreach (var item in harmonogramPosition.Results)
            {
                vm.OperationResults.Add(item);
            }

            window.Show();
        }
        private void RemoveHarmonogramPosition(HarmonogramPosition harmonogramPosition)
        {
            if (SelectedHarmonogram == null)
                return;

            SelectedHarmonogram.HarmonogramPositions.Remove(harmonogramPosition);
            HarmonogramPositions.Remove(harmonogramPosition);
        }

        private void OnOrdersImported(HarmonogramPosition harmonogramPosition, DateTime from, int imported, int added, int failed)
        {
            var h = HarmonogramPositions.FirstOrDefault(x => x.Id == harmonogramPosition.Id);
            h.LastLaunchTime = harmonogramPosition.LastLaunchTime;
            h.Results = harmonogramPosition.Results;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => RefreshCollection(HarmonogramPositions)));

            //if (BalloonTipEnabled)
            //{
            //    App.NotifyIcon.BalloonTipTitle = $"Wykonano import zamówień: {harmonogramPosition.SaleChannelName} / {harmonogramPosition.AccountName}";
            //    App.NotifyIcon.BalloonTipText =
            //    $"Pobrano: {imported} od: {from:dd-MM-yyyy HH:mm:ss}" + System.Environment.NewLine +
            //    $"Dodano nowe: {added}" + System.Environment.NewLine +
            //    $"Niepowodzenie: {failed}";

            //    App.NotifyIcon.ShowBalloonTip(10000);
            //}
        }
    }
}
