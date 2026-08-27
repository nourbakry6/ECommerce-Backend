using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.entites
{
   

    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
