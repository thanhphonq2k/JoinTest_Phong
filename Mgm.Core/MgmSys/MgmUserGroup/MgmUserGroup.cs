using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mgm.MgmSys.MgmUserGroup
{
    [Table("MGMUserGroup")]
    public class MgmUserGroup : Entity
    {
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
