using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.Application.DTO
{
    public class CartItemAddDTO
    { [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int Quantity { get; set; }
    }
}
