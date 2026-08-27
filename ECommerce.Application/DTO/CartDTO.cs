using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTO
{
    public  class CartDTO
    {
     public int Id{  get; set; }
     public List<CartItemDTO> cartItems{ get; set; }
    }
}
