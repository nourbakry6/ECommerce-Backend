using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.entites
{
    

    public class Cart
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        //ha nchila laen bdna cart truh a applicationuser
        //public User User { get; set; } = null!

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
