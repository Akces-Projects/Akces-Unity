namespace Akces.Unity.Models.SaleChannels.Olx
{
    public class OlxConfiguration
    {
        public int Id { get; set; }
        public string ClientId { get; set; } = "201717";
        public string ClientSecret { get; set; } = "I1IbFGo2YwAooOwkvZZAIgGfpq3wAgZN3ujI1jwq3aD9dRhX";
        public string BaseAddress { get; set; } = "https://www.olx.pl/";
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
