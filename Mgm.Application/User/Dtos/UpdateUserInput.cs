using System.ComponentModel.DataAnnotations;

namespace Mgm.User.Dtos
{
    public class UpdateUserInput
    {
        [Required]
        public int Id { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public int IsActive { get; set; }
        public int UserGroupId { get; set; }
    }
}
