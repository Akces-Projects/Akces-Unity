using System.Collections.Generic;

namespace Akces.Unity.Models.SaleChannels.Baselinker.Models
{
    public class BaseLinkerGetOrdersResponse
    {
        public string Status { get; set; }
        public List<BaseLinkerOrder> Orders { get; set; }
    }
}
