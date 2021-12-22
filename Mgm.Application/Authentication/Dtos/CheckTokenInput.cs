using System.ComponentModel.DataAnnotations;

namespace Mgm.Authentication.Dtos
{
    public class CheckTokenInput
    {
        [Required]
        public string Token { get; set; }
    }
}
