using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTO
{
    public class OrderItemDTO
    {
        public int Id { get; set; }

 

        public int ProductId { get; set; }
        public decimal Total => Price * Quantity;
        public string ProdactName{ get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

    }
}
