using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
   public interface IJwtService
    {
        string GEtToken(int userid,string email,IList<string>role,string username);
    }
}
