using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTO
{
    public class CartItemDTO
    {
       public int ID { get; set; }
        public int ProductId { get; set; }

        public decimal ProductPrice { get; set; }
        public string IgammeUrl { get; set; }
         
        public string ProductName{ get; set; }
        public decimal Total => ProductPrice * Quantity;
        public int Quantity { get; set; }
    }
}
