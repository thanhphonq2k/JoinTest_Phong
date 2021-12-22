using Abp.Domain.Entities;
using Mgm.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mgm.Product.Dtos
{
    public class CreateProduct : Entity
    {
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string ProductName { get; set; }

        //[ForeignKey(nameof(Id))]
        //public ProductCategory AssignedName { get; set; }
        //public int? Id { get; set; }
    }
}
