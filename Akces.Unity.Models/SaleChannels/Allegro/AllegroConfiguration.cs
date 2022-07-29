
namespace Akces.Unity.Models.SaleChannels
{
    public class AllegroConfiguration
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        private string clientId = "45be8524b28b443ab64e0cb024020d37";
        private string clientSecret = "JyEA1m41lPWcHgNTleXq069vPRTvbVLkltKtBYAGy9Vlad4iRPNNMwqBd04Jcg2w";
        private string clientId_sandbox = "bba3ff11691d4aeb98eaefbb280a2e87";
        private string clientSecret_sandbox = "Tcy1IwR4zOdZiywF7TmA9GuDaJ58ZpxbHrW3s19OTJWnOdrPFtMe0hfX9hcVW7p2";

        public string ClientId { get => Sandbox ? clientId_sandbox:clientId; set { if (Sandbox) clientId_sandbox = value; else clientId = value; } }
        public string ClientSecret { get => Sandbox ? clientSecret_sandbox : clientSecret; set { if (Sandbox) clientSecret_sandbox = value; else clientSecret = value; } }
        public bool Sandbox { get; set; } = false;
        public string BaseAddress { get; set; } = "allegro.pl"; 
        public string SandboxBaseAddress { get; set; } = "allegro.pl.allegrosandbox.pl";
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
