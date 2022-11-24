
using Akces.Unity.Models.SaleChannels;
using System;
using System.Collections.Generic;

namespace Akces.Unity.DataAccess.Services.Allegro.Models
{
    public class Input
    {
        public BuyNowPrice buyNowPrice { get; set; }
    }

    public class Address
    {
        public string street { get; set; }
        public string city { get; set; }
        public string postCode { get; set; }
        public string countryCode { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string zipCode { get; set; }
        public string companyName { get; set; }
        public string phoneNumber { get; set; }
        public DateTime modifiedAt { get; set; }
        public Company company { get; set; }
        public NaturalPerson naturalPerson { get; set; }
    }

    public class Buyer
    {
        public string id { get; set; }
        public string email { get; set; }
        public string login { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string companyName { get; set; }
        public bool guest { get; set; }
        public string personalIdentity { get; set; }
        public string phoneNumber { get; set; }
        public Preferences preferences { get; set; }
        public Address address { get; set; }
    }

    public class Company
    {
        public string name { get; set; }
        public string taxId { get; set; }
    }

    public class Cost
    {
        public decimal amount { get; set; }
        public string currency { get; set; }
    }

    public class Delivery
    {
        public Address address { get; set; }
        public Method method { get; set; }
        public PickupPoint pickupPoint { get; set; }
        public Cost cost { get; set; }
        public Time time { get; set; }
        public bool smart { get; set; }
        public int calculatedNumberOfPackages { get; set; }
    }

    public class Discount
    {
        public string type { get; set; }
    }

    public class External
    {
        public string id { get; set; }
    }

    public class Fulfillment
    {
        public string status { get; set; }
        public ShipmentSummary shipmentSummary { get; set; }
    }

    public class Guaranteed
    {
        public DateTime from { get; set; }
        public DateTime to { get; set; }
    }

    public class Invoice
    {
        public bool required { get; set; }
        public Address address { get; set; }
        public string dueDate { get; set; }
    }

    public class LineItem
    {
        public string id { get; set; }
        public Offer offer { get; set; }
        public int quantity { get; set; }
        public OriginalPrice originalPrice { get; set; }
        public Price price { get; set; }
        public Reconciliation reconciliation { get; set; }
        public List<SelectedAdditionalService> selectedAdditionalServices { get; set; }
        public DateTime boughtAt { get; set; }

        public Product ToProduct()
        {
            var product = new Product() 
            {
                DiscountPercentage = 0,
                Symbol = this.offer.external.id,
                Attributes = new Dictionary<string, object>(),
                CN = "",
                Currency = price.currency,
                Name = offer.name,
                EAN = "",
                Id = id,
                OriginalPrice = originalPrice.amount,
                Price = price.amount,
                Quantity = reconciliation.quantity
            };

            return product;
        }
    }

    public class Method
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class NaturalPerson
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class OriginalPrice
    {
        public decimal amount { get; set; }
        public string currency { get; set; }
    }

    public class PaidAmount
    {
        public decimal amount { get; set; }
        public string currency { get; set; }
    }

    public class Payment
    {
        public string id { get; set; }
        public string type { get; set; }
        public string provider { get; set; }
        public DateTime finishedAt { get; set; }
        public PaidAmount paidAmount { get; set; }
        public Reconciliation reconciliation { get; set; }
    }

    public class PickupPoint
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Address address { get; set; }
    }

    public class Preferences
    {
        public string language { get; set; }
    }

    public class Price
    {
        public decimal amount { get; set; }
        public string currency { get; set; }
    }

    public class Reconciliation
    {
        public string amount { get; set; }
        public string currency { get; set; }
        public Value value { get; set; }
        public string type { get; set; }
        public int quantity { get; set; }
    }

    public class AllegroGetOrdersResponse
    {
        public List<CheckoutForm> checkoutForms { get; set; }
        public int count { get; set; }
        public int totalCount { get; set; }
    }

    public class SelectedAdditionalService
    {
        public string definitionId { get; set; }
        public string name { get; set; }
        public Price price { get; set; }
        public int quantity { get; set; }
    }

    public class ShipmentSummary
    {
        public string lineItemsSent { get; set; }
    }

    public class Summary
    {
        public TotalToPay totalToPay { get; set; }
    }

    public class Surcharge
    {
        public string id { get; set; }
        public string type { get; set; }
        public string provider { get; set; }
        public DateTime finishedAt { get; set; }
        public PaidAmount paidAmount { get; set; }
        public Reconciliation reconciliation { get; set; }
    }

    public class Time
    {
        public Guaranteed guaranteed { get; set; }
    }

    public class TotalToPay
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class Value
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }
}
