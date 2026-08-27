using ECommerce.Domain.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.entites
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

       

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
