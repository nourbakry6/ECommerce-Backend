using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.entites
{
    //lzm mehi user laenu ma ad est3mlu ha est3,l applicationuser 

    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Customer";

        public DateTime CreatedAt { get; set; }
        public Cart Cart { get; set; } = null!;
    }
}
