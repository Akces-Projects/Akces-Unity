using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Akces.Unity.Models.SaleChannels.Baselinker
{
    public class BaseLinkerOrder
    {
        #region Properties
        [JsonPropertyName("order_id")]
        public int OrderId { get; set; }

        [JsonPropertyName("shop_order_id")]
        public string ShopOrderId { get; set; }

        [JsonPropertyName("external_order_id")]
        public string ExternalOrderId { get; set; }

        [JsonPropertyName("order_source")]
        public string OrderSource { get; set; }

        [JsonPropertyName("order_source_id")]
        public string OrderSourceId { get; set; }

        [JsonPropertyName("order_source_info")]
        public string OrderSourceInfo { get; set; }

        [JsonPropertyName("order_status_id")]
        public int OrderStatusId { get; set; }

        [JsonPropertyName("date_add")]
        public int DateAdd { get; set; }

        [JsonPropertyName("date_confirmed")]
        public int DateConfirmed { get; set; }

        [JsonPropertyName("date_in_status")]
        public int DateInStatus { get; set; }

        [JsonPropertyName("user_login")]
        public string UserLogin { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("user_comments")]
        public string UserComments { get; set; }

        [JsonPropertyName("admin_comments")]
        public string AdminComments { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("payment_method_cod")]
        public string PaymentMethodCod { get; set; }

        [JsonPropertyName("payment_done")]
        public decimal PaymentDone { get; set; }

        [JsonPropertyName("payment_date")]
        public string PaymentDate { get; set; }

        [JsonPropertyName("delivery_method")]
        public string DeliveryMethod { get; set; }

        [JsonPropertyName("delivery_price")]
        public decimal DeliveryPrice { get; set; }

        [JsonPropertyName("delivery_package_module")]
        public string DeliveryPackageModule { get; set; }

        [JsonPropertyName("delivery_package_nr")]
        public string DeliveryPackageNr { get; set; }

        [JsonPropertyName("delivery_fullname")]
        public string DeliveryFullname { get; set; }

        [JsonPropertyName("delivery_company")]
        public string DeliveryCompany { get; set; }

        [JsonPropertyName("delivery_address")]
        public string DeliveryAddress { get; set; }

        [JsonPropertyName("delivery_city")]
        public string DeliveryCity { get; set; }

        [JsonPropertyName("delivery_postcode")]
        public string DeliveryPostcode { get; set; }

        [JsonPropertyName("delivery_country")]
        public string DeliveryCountry { get; set; }
        [JsonPropertyName("delivery_country_code")]
        public string DeliveryCountryCode { get; set; }

        [JsonPropertyName("delivery_point_id")]
        public string DeliveryPointId { get; set; }

        [JsonPropertyName("delivery_point_name")]
        public string DeliveryPointName { get; set; }

        [JsonPropertyName("delivery_point_address")]
        public string DeliveryPointAddress { get; set; }

        [JsonPropertyName("delivery_point_postcode")]
        public string DeliveryPointPostcode { get; set; }

        [JsonPropertyName("delivery_point_city")]
        public string DeliveryPointCity { get; set; }

        [JsonPropertyName("invoice_fullname")]
        public string InvoiceFullname { get; set; }

        [JsonPropertyName("invoice_company")]
        public string InvoiceCompany { get; set; }

        [JsonPropertyName("invoice_nip")]
        public string InvoiceNip { get; set; }

        [JsonPropertyName("invoice_address")]
        public string InvoiceAddress { get; set; }

        [JsonPropertyName("invoice_city")]
        public string InvoiceCity { get; set; }

        [JsonPropertyName("invoice_postcode")]
        public string InvoicePostcode { get; set; }

        [JsonPropertyName("invoice_country")]
        public string InvoiceCountry { get; set; }
        [JsonPropertyName("invoice_country_code")]
        public string InvoiceCountryCode { get; set; }

        [JsonPropertyName("want_invoice")]
        public string WantInvoice { get; set; }

        [JsonPropertyName("extra_field_1")]
        public string ExtraField1 { get; set; }

        [JsonPropertyName("extra_field_2")]
        public string ExtraField2 { get; set; }

        [JsonPropertyName("order_page")]
        public string OrderPage { get; set; }

        [JsonPropertyName("pick_status")]
        public string PickStatus { get; set; }

        [JsonPropertyName("pack_status")]
        public string PackStatus { get; set; }

        [JsonPropertyName("products")]
        public List<BaseLinkerProduct> Products { get; set; }
        #endregion

        public Order ToOrder()
        {
            var names = GetNamesArray(InvoiceFullname);

            var order = new Order
            {
                Annotation = this.UserComments,
                Original = this.OrderId.ToString(),
                SourceSaleChannelName = "BaseLinker/" + this.OrderSource,
                OriginalDate = DateTimeOffset.FromUnixTimeSeconds(DateAdd).LocalDateTime,
                CompletionDate = null,
                Title = this.ExtraField1,
                Subtitle = this.ExtraField2,
                Branch = this.DeliveryCountry,
                OriginalPlace = string.IsNullOrEmpty(InvoiceCity) ? DeliveryCity : InvoiceCity,
                Warehouse = "",
                Confirmed = this.DateConfirmed > 0,
                Currency = this.Currency,
                Products = this.Products.Select(x => x.ToProduct()).ToList(),
                Payments = new List<Payment>()
                {
                    new Payment()
                    {
                        PaymentMethod = this.PaymentMethod,
                        Currency = this.Currency,
                        TimeLimit = null,
                        Value = this.PaymentDone
                    }
                },
                
                Delivery = new Delivery()
                {
                    DeliveryCost = this.DeliveryPrice,
                    DeliveryTax = "",
                    PackageNumber = this.DeliveryPackageNr,
                    DeliveryMethod = this.DeliveryMethod,
                    DeliveryAddress = new DeliveryAddress()
                    {
                        DeliveryPointName = this.DeliveryPointName,
                        DeliveryPointId = this.DeliveryPointId,
                        Name = (this.DeliveryCompany + " " + this.DeliveryFullname).Trim(),
                        Country = this.DeliveryCountry,
                        CountryCode = this.DeliveryCountryCode,
                        Line1 = string.IsNullOrEmpty(this.DeliveryPointId) ? this.DeliveryCity : this.DeliveryPointAddress,
                        Line2 = string.IsNullOrEmpty(this.DeliveryPointId) ? this.DeliveryPostcode : this.DeliveryPointPostcode,
                        Line3 = string.IsNullOrEmpty(this.DeliveryPointId) ? this.DeliveryCity : this.DeliveryPointCity
                    }
                },
                Purchaser = new Contractor()
                {
                    Email = this.Email,
                    FirstName = names[0],
                    SecondName = names[1],
                    LastName = names[2],
                    PESEL = "",
                    Line1 = InvoiceAddress,
                    Line2 = InvoicePostcode,
                    Line3 = InvoiceCity,
                    Country = InvoiceCountry,
                    VATIN = InvoiceNip,
                    CountryCode = InvoiceCountryCode,
                    FullName = this.InvoiceFullname,
                    Name = this.InvoiceCompany,
                    PhoneNumber = this.Phone,
                    Username = this.Email,
                    Type = string.IsNullOrEmpty(this.InvoiceNip) ? ContractorType.Person : ContractorType.Company
                }
            };

            return order;
        }

        private string[] GetNamesArray(string fullName)
        {
            var splitedNames = fullName.Split(' ');

            var names = new string[3];

            switch (splitedNames.Length)
            {
                case 1:
                    names[0] = "";
                    names[1] = "";
                    names[2] = splitedNames[0];
                    break;
                case 2:
                    names[0] = splitedNames[0];
                    names[1] = "";
                    names[2] = splitedNames[1];
                    break;
                case 3:
                    names[0] = splitedNames[0];
                    names[1] = splitedNames[1];
                    names[2] = splitedNames[2];
                    break;
                default: // > 3
                    names[0] = splitedNames[0];
                    names[1] = splitedNames[1];
                    names[2] = string.Join(" ", splitedNames.Skip(2).ToArray());
                    break;
            }

            return names;
        }
    }
}
