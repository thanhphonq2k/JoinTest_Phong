using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Mgm.UserGroup.Dtos
{
    public class UpdateUserGroupInput
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string GroupName { get; set; }
        public string Description { get; set; }
        [Required]
        public int IsActive { get; set; }
    }
}
