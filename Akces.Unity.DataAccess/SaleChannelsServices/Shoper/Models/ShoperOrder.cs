using Akces.Unity.Models.SaleChannels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;

namespace Unity.SaleChannels.Shoper.Models
{
    public class ShoperOrder
    {
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("status_date")]
        public string StatusDate { get; set; }

        [JsonPropertyName("confirm_date")]
        public string ConfirmDate { get; set; }

        [JsonPropertyName("delivery_date")]
        public string DeliveryDate { get; set; }

        [JsonPropertyName("status_id")]
        public string StatusId { get; set; }

        [JsonPropertyName("sum")]
        public string Sum { get; set; }

        [JsonPropertyName("payment_id")]
        public string PaymentId { get; set; }

        [JsonPropertyName("user_order")]
        public string UserOrder { get; set; }

        [JsonPropertyName("shipping_id")]
        public string ShippingId { get; set; }

        [JsonPropertyName("shipping_cost")]
        public string ShippingCost { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("delivery_code")]
        public string DeliveryCode { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("confirm")]
        public string Confirm { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("notes_priv")]
        public string NotesPriv { get; set; }

        [JsonPropertyName("notes_pub")]
        public string NotesPub { get; set; }

        [JsonPropertyName("currency_id")]
        public string CurrencyId { get; set; }

        [JsonPropertyName("currency_rate")]
        public string CurrencyRate { get; set; }

        [JsonPropertyName("paid")]
        public string Paid { get; set; }

        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; }

        [JsonPropertyName("discount_client")]
        public string DiscountClient { get; set; }

        [JsonPropertyName("discount_group")]
        public string DiscountGroup { get; set; }

        [JsonPropertyName("discount_levels")]
        public string DiscountLevels { get; set; }

        [JsonPropertyName("discount_code")]
        public string DiscountCode { get; set; }

        [JsonPropertyName("code_id")]
        public object CodeId { get; set; }

        [JsonPropertyName("lang_id")]
        public string LangId { get; set; }

        [JsonPropertyName("origin")]
        public string Origin { get; set; }

        [JsonPropertyName("parent_order_id")]
        public object ParentOrderId { get; set; }

        [JsonPropertyName("registered")]
        public string Registered { get; set; }

        [JsonPropertyName("promo_code")]
        public string PromoCode { get; set; }

        [JsonPropertyName("additional_fields")]
        public List<ShoperAdditionalField> AdditionalFields { get; set; }

        [JsonPropertyName("tags")]
        public List<object> Tags { get; set; }

        [JsonPropertyName("is_paid")]
        public bool IsPaid { get; set; }

        [JsonPropertyName("is_overpayment")]
        public bool IsOverpayment { get; set; }

        [JsonPropertyName("is_underpayment")]
        public bool IsUnderpayment { get; set; }

        [JsonPropertyName("total_parcels")]
        public int TotalParcels { get; set; }

        [JsonPropertyName("total_products")]
        public int TotalProducts { get; set; }

        [JsonPropertyName("children")]
        public List<object> Children { get; set; }

        [JsonPropertyName("loyalty_cost")]
        public int LoyaltyCost { get; set; }

        [JsonPropertyName("loyalty_score")]
        public int LoyaltyScore { get; set; }

        [JsonPropertyName("vat_eu")]
        public bool VatEu { get; set; }

        [JsonPropertyName("shipping_tax_name")]
        public string ShippingTaxName { get; set; }

        [JsonPropertyName("shipping_tax_value")]
        public string ShippingTaxValue { get; set; }

        [JsonPropertyName("shipping_tax_id")]
        public string ShippingTaxId { get; set; }

        [JsonPropertyName("pickup_point")]
        public string PickupPoint { get; set; }

        [JsonPropertyName("delivery_address")]
        public ShoperDeliveryAddress DeliveryAddress { get; set; }

        [JsonPropertyName("billing_address")]
        public ShoperBillingAddress BillingAddress { get; set; }
        public List<ShoperOrderPosition> OrderPositions { get; internal set; }
        public ShoperShipping Shipping { get; internal set; }
        public ShoperPaymentMethod PaymentMethod { get; internal set; }
        public ShoperCurrency Currency { get; internal set; }

        public Order ToOrder() 
        {
            DateTime? deliveryDate = null;
            DateTime? confirmDate = null;

            if (DateTime.TryParse(this.DeliveryDate, out DateTime _deliveryDate))
                deliveryDate = _deliveryDate;

            if (DateTime.TryParse(this.ConfirmDate, out DateTime _confirmDate))
                confirmDate = _deliveryDate;

            var currencyRate = this.Currency.Name == "EUR" ? 4 : 1;

            var order = new Order
            {
                Annotation = this.Notes + " " + this.NotesPriv + " " + this.NotesPub,
                Original = this.OrderId,
                SourceSaleChannelName = "Shoper",
                OriginalDate = DateTime.Parse(this.Date),
                CompletionDate = deliveryDate,
                Title = this.Code,
                Subtitle = this.PickupPoint,
                Branch = this.BillingAddress.CountryCode,
                OriginalPlace = this.BillingAddress.City,
                Warehouse = "",
                Confirmed = this.Confirm == "1",
                Currency = this.Currency.Name,
                Products = this.OrderPositions.Select(x => x.ToProduct(currencyRate)).ToList(),
                Payments = new List<Payment>()
                {
                    new Payment()
                    {
                        PaymentMethod = (typeof(ShoperTranslations).GetProperties()
                            .FirstOrDefault(x => x.Name == BillingAddress.CountryCode)
                            .GetValue(PaymentMethod.Translations) as ShoperTranslation)
                            .Title,

                        Currency = this.Currency.Name,
                        TimeLimit = null,
                        Value = decimal.Parse(this.Sum, CultureInfo.InvariantCulture) / currencyRate
                    }
                },

                Delivery = new Delivery()
                {
                    DeliveryCost = decimal.Parse(this.ShippingCost, CultureInfo.InvariantCulture) / currencyRate,
                    DeliveryTax = this.ShippingTaxValue,
                    DeliveryMethod = this.Shipping.Name,
                    DeliveryAddress = new DeliveryAddress()
                    {
                        DeliveryPointName = this.PickupPoint,
                        DeliveryPointId = this.PickupPoint,
                        Name = (this.DeliveryAddress.Company + " " + this.DeliveryAddress.Firstname + " " + this.DeliveryAddress.Lastname + " ").Trim(),
                        Country = this.DeliveryAddress.Country,
                        CountryCode = this.DeliveryAddress.CountryCode,
                        Line1 = this.DeliveryAddress.Street1 + " " + this.DeliveryAddress.Street2,
                        Line2 = this.DeliveryAddress.Postcode,
                        Line3 = this.DeliveryAddress.City,
                    }
                },
                Purchaser = new Contractor()
                {
                    Email = this.Email,
                    FirstName = this.BillingAddress.Firstname,
                    SecondName = "",
                    LastName = this.BillingAddress.Lastname,
                    PESEL = this.BillingAddress.Pesel,
                    Country = this.BillingAddress.Country,
                    Line1 = (this.BillingAddress.Street1 + " " + this.BillingAddress.Street2).Trim(),
                    Line2 = this.BillingAddress.Postcode,
                    Line3 = this.BillingAddress.City,
                    VATIN = this.BillingAddress.TaxIdentificationNumber,
                    CountryCode = this.BillingAddress.CountryCode,
                    FullName = this.BillingAddress.Company?.Trim(),
                    Name = string.IsNullOrEmpty(this.BillingAddress.Company) ? (this.BillingAddress.Firstname + this.BillingAddress.Lastname).Trim() : this.BillingAddress.Company.Trim(),
                    PhoneNumber = this.BillingAddress.Phone,
                    Username = this.Email,
                    Type = string.IsNullOrEmpty(this.BillingAddress.Company) ? ContractorType.Person : ContractorType.Company
                }
            };

            order.Delivery.DeliveryCost = Math.Round(order.Delivery.DeliveryCost, 2, MidpointRounding.AwayFromZero);
            var wartosc = order.Products.Sum(x => Math.Round(x.Price, 2, MidpointRounding.AwayFromZero) * x.Quantity) + order.Delivery.DeliveryCost;
            var roznica = order.Payments.Sum(x => x.Value) - wartosc;

            order.Delivery.DeliveryCost += roznica;

            wartosc = order.Products.Sum(x => x.Price * x.Quantity) + order.Delivery.DeliveryCost;

            return order;
        }
    }
}
