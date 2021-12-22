using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Mgm.Utility.Authentication.Dtos
{
    public class JWTContainerModel : IAuthContainerModel
    {
        public string SecretKey { get; set; } = "TW9zaGVFcmV6UHJpdmF0ZUtleQ==";
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public int ExpireMinutes { get; set; } = 10080; // 7 days
        public Claim[] Claims { get; set; }
    }
}
