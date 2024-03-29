﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Akces.Unity.App.ViewModels;

namespace Akces.Unity.App.Views
{
    public partial class ProductsPricesUpdateView : UserControl
    {
        public ProductsPricesUpdateView()
        {
            InitializeComponent();
        }

        //Robią to samo ale zostawiam jako 2 osobne jakby było trzeba dodać coś tylko do jednej, OnLoaded może przecież wiecej rzeczy robić jeśli zajdzie taka potrzeba 
        private void OnLoaded(object sender, EventArgs e)
        {
            ChangeAccountsFilterText();
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ChangeAccountsFilterText();
        }

        private void SearchMethodButtonClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            searchMethodContextMenu.IsOpen = !searchMethodContextMenu.IsOpen;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = DataContext as ProductsPricesUpdateViewModel;
            vm?.LoadPricesCommand.Execute(null);
        }

        private void SearchMethodContextMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var contextMenu = sender as ContextMenu;
            var menuItem = e.OriginalSource as MenuItem;
            var searchMethod = (SearchMethod)menuItem.DataContext;
            var vm = DataContext as ProductsPricesUpdateViewModel;
            vm.SearchMethod = searchMethod;
        }

        private void ChangeAccountsFilterText()
        {
            var vm = DataContext as ProductsPricesUpdateViewModel;
            var count = vm.Accounts.Count(x => x.Selected);

            if (count == 0)
            {
                accountsFilterText.Text = "Brak wybranych kont";
            }
            else if (count == 1)
            {
                var account = vm.Accounts.First(x => x.Selected).Item;
                accountsFilterText.Text = $"{account.Name} ({account.AccountType})";
            }
            else
            {
                accountsFilterText.Text = $"Wybranych kont: {count}";
            }
        }
    }
}
