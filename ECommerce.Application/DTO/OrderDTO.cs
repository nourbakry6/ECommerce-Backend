using ECommerce.Domain.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

       

        

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public decimal TotalAmount { get; set; }

        public List<OrderItemDTO> OrderItems { get; set; } 
    }
}
