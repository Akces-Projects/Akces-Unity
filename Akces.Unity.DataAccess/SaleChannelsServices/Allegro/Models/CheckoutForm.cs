
using Akces.Unity.Models.SaleChannels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Akces.Unity.DataAccess.Services.Allegro.Models
{
    public class CheckoutForm
    {
        public string id { get; set; }
        public string messageToSeller { get; set; }
        public Buyer buyer { get; set; }
        public Payment payment { get; set; }
        public string status { get; set; }
        public Fulfillment fulfillment { get; set; }
        public Delivery delivery { get; set; }
        public Invoice invoice { get; set; }
        public List<LineItem> lineItems { get; set; }
        public List<Surcharge> surcharges { get; set; }
        public List<Discount> discounts { get; set; }
        public Summary summary { get; set; }
        public DateTime updatedAt { get; set; }
        public string revision { get; set; }

        public Order ToOrder()
        {
            var order = new Order
            {
                Annotation = this.messageToSeller,
                Original = this.id.ToString(),
                SourceSaleChannelName = "Allegro",
                OriginalDate = updatedAt,
                CompletionDate = this.delivery.time.guaranteed.to,
                Title = string.Empty,
                Subtitle = string.Empty,
                Branch = this.invoice.address.countryCode,
                OriginalPlace = string.IsNullOrEmpty(invoice?.address?.city) ? delivery.address.city : invoice.address.city,
                Warehouse = "",
                Confirmed = true,
                Currency = this.payment.reconciliation.currency,
                Products = this.lineItems.Select(x => x.ToProduct()).ToList(),
                Payments = new List<Unity.Models.SaleChannels.Payment>()
                {
                    new Unity.Models.SaleChannels.Payment()
                    {
                        PaymentMethod = this.payment.provider,
                        Currency = this.payment.paidAmount.currency,
                        TimeLimit = this.payment.finishedAt,
                        Value = this.payment.paidAmount.amount,
                        TransactionNumber = this.payment.id
                    }
                },

                Delivery = new Unity.Models.SaleChannels.Delivery()
                {
                    DeliveryCost = this.delivery.cost.amount,
                    DeliveryTax = "",
                    PackageNumber = "",
                    DeliveryMethod = this.delivery.method.name,
                    DeliveryAddress = new DeliveryAddress()
                    {
                        DeliveryPointName = this.delivery.pickupPoint.name,
                        DeliveryPointId = this.delivery.pickupPoint.id,
                        Name = (this.delivery.address.companyName ?? "" + " " + this.delivery.address.firstName + " " + delivery.address.lastName).Trim(),
                        Country = this.delivery.address.countryCode,
                        CountryCode = this.delivery.address.countryCode,
                        Line1 = string.IsNullOrEmpty(this.delivery.pickupPoint.id) ? this.delivery.pickupPoint.address.street : this.delivery.address.street,
                        Line2 = string.IsNullOrEmpty(this.delivery.pickupPoint.id) ? this.delivery.pickupPoint.address.zipCode : this.delivery.address.zipCode,
                        Line3 = string.IsNullOrEmpty(this.delivery.pickupPoint.id) ? this.delivery.pickupPoint.address.city : this.delivery.address.city
                    }
                },
                Purchaser = new Contractor()
                {
                    Email = this.buyer.email,
                    FirstName = this.buyer.firstName,
                    SecondName = "",
                    LastName = this.buyer.lastName,
                    PESEL = this.buyer.personalIdentity,
                    Line1 = this.buyer.address.street,
                    Line2 = this.buyer.address.zipCode,
                    Line3 = this.buyer.address.city,
                    Country = this.buyer.address.countryCode,
                    VATIN = this.invoice.address.company.taxId,
                    CountryCode = this.buyer.address.countryCode,
                    FullName = string.IsNullOrEmpty(this.buyer.companyName) ? this.buyer.firstName + " " + this.buyer.lastName : this.buyer.companyName,
                    Name = this.invoice.address.company.name,
                    PhoneNumber = this.buyer.phoneNumber,
                    Username = this.buyer.login,
                    Type = string.IsNullOrEmpty(this.invoice.address.company.taxId) ? ContractorType.Person : ContractorType.Company
                }
            };

            return order;
        }
    }
}
