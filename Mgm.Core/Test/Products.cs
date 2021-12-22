using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mgm.Test
{
    [Table("Products")]
    public class Products : Entity
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string ProductName { get; set; }

        [ForeignKey(nameof(Id))]
        public ProductCategory AssignedName { get; set; }
        public int Id { get; set; }
    }
}
