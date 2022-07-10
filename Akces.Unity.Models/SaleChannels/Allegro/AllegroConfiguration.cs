
namespace Akces.Unity.Models.SaleChannels
{
    public class AllegroConfiguration
    {
        public int Id { get; set; }
        public string ClientId { get; set; } = "45be8524b28b443ab64e0cb024020d37"; // "bba3ff11691d4aeb98eaefbb280a2e87";
        public string ClientSecret { get; set; } = "JyEA1m41lPWcHgNTleXq069vPRTvbVLkltKtBYAGy9Vlad4iRPNNMwqBd04Jcg2w"; // Tcy1IwR4zOdZiywF7TmA9GuDaJ58ZpxbHrW3s19OTJWnOdrPFtMe0hfX9hcVW7p2";
        public bool Sandbox { get; set; } = false;
        public string BaseAddress { get; set; } = "allegro.pl"; 
        public string SandboxBaseAddress { get; set; } = "allegro.pl.allegrosandbox.pl";
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
