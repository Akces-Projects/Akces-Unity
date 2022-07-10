
namespace Akces.Unity.DataAccess.Services.Allegro.Models
{
    public class AllegroGetTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public bool allegro_api { get; set; }
        public string jti { get; set; }
    }
}
