
namespace Akces.Unity.Models.SaleChannels.Baselinker
{
    public class BaselinkerConfiguration
    {
        public int Id { get; set; }
        public string BaseAddress { get; set; } = "https://api.baselinker.com/connector.php";
        public string Token { get; set; }
        public bool GetUnconfirmedOrders { get; set; } = false;
        public int ImportOrdersFromOffsetHours { get; set; } = 10;
    }
}
