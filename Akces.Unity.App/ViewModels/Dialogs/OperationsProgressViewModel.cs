﻿using System.Windows.Input;
using Akces.Wpf.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Akces.Unity.App.Operations;
using System.Threading;

namespace Akces.Unity.App.ViewModels
{
    public class OperationsProgressViewModel : ControlViewModel 
    {
        private readonly CancellationTokenSource cancellationTokenSource;

        private int currenPosition;
        private int positionsCount;
        private string comment;
        public int CurrentPosition
        {
            get { return currenPosition; }
            set { currenPosition = value; OnPropertyChanged(); }
        }
        public int PositionsCount
        {
            get { return positionsCount; }
            set { positionsCount = value; OnPropertyChanged(); }
        }
        public string Comment
        {
            get { return comment; }
            set { comment = value; OnPropertyChanged(); }
        }

        public IUnityTask Operation { get; set; }
        public ICommand CancelOperationsCommand { get; set; }


        public OperationsProgressViewModel(HostViewModel host) : base(host)
        {
            Host.Window.Closed += (s, e) => cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            CancelOperationsCommand = CreateCommand(() => cancellationTokenSource.Cancel());
        }

        public async Task RunOperationsAsync()
        {
            PositionsCount = Operation.Processes;
            Operation.OnTaskProgress += OnOperationProgress;
            await Operation.ExecuteAsync(cancellationTokenSource.Token);
            Operation.OnTaskExecuted.Invoke(null, null);
            Host.Window.Close();
        }
        private void OnOperationProgress(int index, string description) 
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                CurrentPosition = index;
                Comment = description;
            }));
        }
    }
}
