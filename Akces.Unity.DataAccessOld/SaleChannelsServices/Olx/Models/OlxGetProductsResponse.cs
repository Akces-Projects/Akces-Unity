using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akces.Unity.DataAccess.Services.Olx.Models
{
    public class Attribute
    {
        public string code { get; set; }
        public string value { get; set; }
        public object values { get; set; }
    }

    public class Contact
    {
        public string name { get; set; }
        public string phone { get; set; }
    }

    public class Image
    {
        public string url { get; set; }
    }

    public class Location
    {
        public int city_id { get; set; }
        public object district_id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Price
    {
        public decimal value { get; set; }
        public string currency { get; set; }
        public bool negotiable { get; set; }
        public bool trade { get; set; }
        public bool budget { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public string status { get; set; }
        public string url { get; set; }
        public string created_at { get; set; }
        public string activated_at { get; set; }
        public string valid_to { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int category_id { get; set; }
        public string advertiser_type { get; set; }
        public string external_id { get; set; }
        public string external_url { get; set; }
        public Contact contact { get; set; }
        public Location location { get; set; }
        public List<Image> images { get; set; }
        public Price price { get; set; }
        public object salary { get; set; }
        public List<Attribute> attributes { get; set; }
        public object courier { get; set; }
    }       

    public class OlxProductData
    {
        public string title { get; set; }
        public string description { get; set; }
        public int category_id { get; set; }
        public string advertiser_type { get; set; }
        public object external_id { get; set; }
        public object external_url { get; set; }
        public Contact contact { get; set; }
        public Location location { get; set; }
        public List<Image> images { get; set; }
        public Price price { get; set; }
        public string[] salary { get; set; }
        public List<Attribute> attributes { get; set; }
        public object courier { get; set; }
    }

    public class OlxGetProductsResponse
    {
        public List<Datum> data { get; set; }
    }    

    public class OlxGetProductResponse
    {
        public OlxProductData data { get; set; }
    }
}
