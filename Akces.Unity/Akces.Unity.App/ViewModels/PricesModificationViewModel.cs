using Akces.Wpf.Models;
using System.Windows.Input;
using System.Collections.Generic;
using System;

namespace Akces.Unity.App.ViewModels
{
    public class PricesModificationViewModel : ControlViewModel
    {
        private bool usePercentageValue;
        private bool useAddPriceValue;

        public List<ProductAssortmentModel> Products { get; set; }
        public decimal PercentageValue { get; set; } = 100;
        public decimal AddPriceValue { get; set; }
        public bool UsePercentageValue { get => usePercentageValue; set { usePercentageValue = value; OnPropertyChanged(); } }
        public bool UseAddPriceValue { get => useAddPriceValue; set { useAddPriceValue = value; OnPropertyChanged(); } }

        public ICommand SetFromNexoCommand { get; set; }
        public ICommand SetFromOriginalCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public PricesModificationViewModel(HostViewModel host) : base(host)
        {
            SetFromNexoCommand = CreateCommand(SetFromNexo, (err) => Host.ShowError(err));
            SetFromOriginalCommand = CreateCommand(SetFromOriginal, (err) => Host.ShowError(err));
            ApplyCommand = CreateCommand(Apply, (err) => Host.ShowError(err));
            CloseCommand = CreateCommand(Close, (err) => Host.ShowError(err));
        }

        private void SetFromNexo() 
        {
            foreach (var product in Products)
            {
                if (!product.ErpPrice.HasValue)
                    continue;

                var newPrice = product.ErpPrice.Value;
                product.CurrentPrice = newPrice;
            }

            Close();
        }

        private void SetFromOriginal()
        {
            foreach (var product in Products)
            {
                var newPrice = product.OriginalPrice;
                product.CurrentPrice = newPrice;
            }

            Close();
        }

        private void Apply() 
        {
            foreach (var product in Products)
            {
                if (UsePercentageValue) 
                {
                    product.CurrentPrice = product.CurrentPrice * (PercentageValue / 100);
                    product.CurrentPrice = Math.Round(product.CurrentPrice, 2, MidpointRounding.AwayFromZero);
                }

                if (UseAddPriceValue) 
                {
                    product.CurrentPrice += AddPriceValue;
                    product.CurrentPrice = Math.Round(product.CurrentPrice, 2, MidpointRounding.AwayFromZero);
                }
            }

            Close();
        }

        private void Close() 
        {
            Host.Window.Close();
        }
    }
}
