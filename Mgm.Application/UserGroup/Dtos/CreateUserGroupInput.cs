using System.ComponentModel.DataAnnotations;

namespace Mgm.UserGroup.Dtos
{
    public class CreateUserGroupInput
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string GroupCode { get; set; }
        [Required]
        public string GroupName { get; set; }
        public string Description { get; set; }
        [Required]
        public int IsActive { get; set; }
    }
}
