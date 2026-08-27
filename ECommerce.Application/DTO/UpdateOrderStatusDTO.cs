using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ECommerce.Domain.enums;
namespace ECommerce.Application.DTO
{
    public class UpdateOrderStatusDTO
    {
        public OrderStatus status { get; set; } 
    }
}
