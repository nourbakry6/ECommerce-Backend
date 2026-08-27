using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTO
{
   public class UserUpdateDTO
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

     
     
    }
}
