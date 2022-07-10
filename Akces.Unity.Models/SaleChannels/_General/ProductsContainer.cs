using System.Collections.Generic;

namespace Akces.Unity.Models.SaleChannels
{
    public class ProductsContainer 
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public List<Product> Products { get; set; }
    }
}
