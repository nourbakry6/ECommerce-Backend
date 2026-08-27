using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.entites
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public Cart Cart { get; set; } = null!;

        public int ProductId { get; set; }

        public decimal ProductPrice { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
    }

}
