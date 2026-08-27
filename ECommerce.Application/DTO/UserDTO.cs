using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTO
{
    public class UserDTO
    {

        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;


        public bool IsActive { get; set; } = true;
        public string Role { get; set; } = "Customer";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
