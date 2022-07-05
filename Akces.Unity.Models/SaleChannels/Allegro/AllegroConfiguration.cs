
namespace Akces.Unity.Models.SaleChannels.Allegro
{
    public class AllegroConfiguration
    {
        public int Id { get; set; }
        public string ClientId { get; set; } = "bba3ff11691d4aeb98eaefbb280a2e87";
        public string ClientSecret { get; set; } = "Tcy1IwR4zOdZiywF7TmA9GuDaJ58ZpxbHrW3s19OTJWnOdrPFtMe0hfX9hcVW7p2";
        public bool Sandbox { get; set; } = true;
        public string BaseAddress { get; set; } = "allegro.pl"; 
        public string SandboxBaseAddress { get; set; } = "allegro.pl.allegrosandbox.pl";
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
