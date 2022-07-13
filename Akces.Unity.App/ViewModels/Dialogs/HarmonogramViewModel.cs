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
        public List<TaskType> OperationTypes { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ActivateHarmonogramCommand { get; set; }
        public ICommand AddHarmonogramPositionCommand { get; set; }
        public ICommand RemoveHarmonogramPositionCommand { get; set; }

        public HarmonogramViewModel(HostViewModel host) : base(host)
        {
            SaveCommand = CreateCommand(Save, (err) => Host.ShowError(err));
            CancelCommand = CreateCommand(Cancel, (err) => Host.ShowError(err));
            ActivateHarmonogramCommand = CreateCommand(ActivateHarmonogram, (err) => Host.ShowError(err));
            AddHarmonogramPositionCommand = CreateCommand(AddHarmonogramPosition, (err) => Host.ShowError(err));
            RemoveHarmonogramPositionCommand = CreateCommand<HarmonogramPosition>(RemoveHarmonogramPosition, (err) => Host.ShowError(err));
            OperationTypes = Enum.GetValues(typeof(TaskType)).Cast<TaskType>().ToList();
            Accounts = (new AccountsManager()).Get();
        }

        public void LoadHarmonogramPositions()
        {
            foreach (var harmonogramPosition in Harmonogram.Data.Positions.Where(x => x.Account != null))
            {
                harmonogramPosition.Account = Accounts.FirstOrDefault(x => x.Id == harmonogramPosition.Account.Id);
            }

            Positions = new ObservableCollection<HarmonogramPosition>(Harmonogram.Data.Positions);
        }
        private void Save()
        {
            if (harmonogram.Data.Active)
                harmonogram.Activate();

            harmonogram.Save();
            Host.Window.Close();
            (Host.Window.Owner.GetHost().ControlViewModel as HarmonogramsViewModel).LoadHarmonograms();
        }
        private void Cancel()
        {
            harmonogram?.Dispose();
            Host.Window.Close();
        }
        private void ActivateHarmonogram() 
        {
            Harmonogram.Activate();
            Host.ShowInfo("Harmonogram został aktywowany");
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
