using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.enums
{
    public enum OrderStatus
    {
        Pending,
        Sending,
        Shipped,
        Delivered,
        Cancelled
    }
}
