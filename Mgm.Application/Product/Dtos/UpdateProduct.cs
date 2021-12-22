using Mgm.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mgm.Product.Dtos
{
    public class UpdateProduct
    {
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string ProductName { get; set; }
        //public ProductCategory AssignedName { get; set; }
        //public int? Id { get; set; }
    }
}
