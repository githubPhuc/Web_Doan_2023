using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;

namespace Web_Doan_2023.Settings
{
    public class JsonLogin
    {
      
                   public string token { get; set; }
                   public DateTime expiration { get; set; }
                   public string id { get; set; }
                   public string email { get; set; }
                   public string phone { get; set; }
                   public string role { get; set; }
                   public string username { get; set; }
                   public string name { get; set; }
        
 
    }
}
