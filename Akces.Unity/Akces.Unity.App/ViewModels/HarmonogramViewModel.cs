using System;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Akces.Wpf.Models;
using Akces.Wpf.Extensions;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers;
using Akces.Unity.DataAccess.Managers.BusinessObjects;
using Akces.Wpf.Helpers;

namespace Akces.Unity.App.ViewModels
{
    internal class HarmonogramViewModel : ControlViewModel
    {
        private IHarmonogram harmonogram;
        public IHarmonogram Harmonogram { get => harmonogram; set { harmonogram = value; OnPropertyChanged(); } }

        public ObservableCollection<HarmonogramPosition> Positions { get; set; }
        public List<Account> Accounts { get; set; }
        public List<OperationType> OperationTypes { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand AddHarmonogramPositionCommand { get; set; }
        public ICommand RemoveHarmonogramPositionCommand { get; set; }

        public HarmonogramViewModel(HostViewModel host) : base(host)
        {
            SaveCommand = CreateCommand(Save, (err) => Host.ShowError(err));
            CancelCommand = CreateCommand(Cancel, (err) => Host.ShowError(err));
            AddHarmonogramPositionCommand = CreateCommand(AddHarmonogramPosition, (err) => Host.ShowError(err));
            RemoveHarmonogramPositionCommand = CreateCommand<HarmonogramPosition>(RemoveHarmonogramPosition, (err) => Host.ShowError(err));
            OperationTypes = Enum.GetValues(typeof(OperationType)).Cast<OperationType>().ToList();
            Accounts = (new AccountsManager()).Get();
        }

        public void LoadHarmonogramPositions()
        {
            Positions = new ObservableCollection<HarmonogramPosition>(Harmonogram.Data.Positions);
        }
        private void Save()
        {
            harmonogram?.Save();
            Host.Window.Close();
            (Host.Window.Owner.GetHost().ControlViewModel as HarmonogramsViewModel).LoadHarmonograms();

            if (harmonogram.Data.Active)
            {
                var harmonogramWorker = ServicesProvider.GetService<HarmonogramWorker>();
                harmonogramWorker.SetActiveHarmonogram(harmonogram.Data);
            }
        }
        private void Cancel()
        {
            harmonogram?.Dispose();
            Host.Window.Close();
        }
        private void AddHarmonogramPosition()
        {
            var position = Harmonogram.AddPosition();            
            Positions.Add(position);
        }
        private void RemoveHarmonogramPosition(HarmonogramPosition harmonogramPosition) 
        {
            Harmonogram.RemovePosition(harmonogramPosition);
            Positions.Remove(harmonogramPosition);
        }
    }
}
