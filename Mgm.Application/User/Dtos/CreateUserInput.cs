using System.ComponentModel.DataAnnotations;

namespace Mgm.User.Dtos
{
    public class CreateUserInput
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int IsActive { get; set; }
        [Required]
        public int UserGroupId { get; set; }
    }
}
