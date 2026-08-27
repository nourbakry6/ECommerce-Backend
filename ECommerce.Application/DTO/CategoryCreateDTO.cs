using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.Application.DTO
{
    public class CategoryCreateDTO
    {   [Required]
    [MinLength(3)]
        public string Name { get; set; } = string.Empty;
    }
}
