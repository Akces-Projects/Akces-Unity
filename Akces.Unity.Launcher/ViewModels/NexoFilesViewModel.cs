using Akces.Core.Nexo;
using Akces.Wpf;
using Akces.Wpf.Helpers;
using Akces.Wpf.Models;
using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace Akces.Unity.Launcher.ViewModels
{
    public class NexoFilesViewModel : ControlViewModel
    {
        private string nexoFilesPath;

        public string NexoFilesPath { get => nexoFilesPath; set { nexoFilesPath = value; OnPropertyChanged(); } }
        public ICommand SelectNexoBinFolderCommand { get; set; }
        public ICommand GoToDatabaseConnectionCommand { get; set; }

        public NexoFilesViewModel(HostViewModel host) : base(host)
        {
            SelectNexoBinFolderCommand = CreateCommand(SelectNexoBinFolder);
            GoToDatabaseConnectionCommand = CreateCommand(() => Host.UpdateView<DatabaseConnectionViewModel>(), (err) => Host.ShowError(err));
            var nexoDatabase = ServicesProvider.GetService<NexoDatabase>();
            CheckNexoFilesLocation(nexoDatabase);
            NexoFilesPath = nexoDatabase.FilesPath;
        }

        private void SelectNexoBinFolder()
        {
            var dialog = new FolderBrowserDialog();
            var nexoFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"AppData\Local\InsERT\Deployments\Nexo\");

            if (Directory.Exists(nexoFolderPath))
            {
                dialog.SelectedPath = nexoFolderPath;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                NexoFilesPath = dialog.SelectedPath;
                ServicesProvider.GetService<NexoDatabase>().FilesPath = dialog.SelectedPath;
            }
        }
        private void CheckNexoFilesLocation(NexoDatabase nexoDatabase)
        {
            var nexoFilesPath = nexoDatabase.FilesPath ?? nexoDatabase.GetNexoFilesPath();

            if (!string.IsNullOrEmpty(nexoFilesPath))
                nexoDatabase.FilesPath = nexoFilesPath;
            else
                Host.ShowWarning($"Nie można zlokalizaować plików nexo.{Environment.NewLine}Należy wskazać odpowiedni folder ręcznie");
        }
    }
}
