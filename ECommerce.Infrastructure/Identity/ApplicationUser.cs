using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Identity
{// nhna emlna l identity b infrasstructure krml l clain architecture la2n domain ma by3rf identity bs infra so ha khli identity fi w a3ml mapping mae domain
    public class ApplicationUser:IdentityUser<int>
    {
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;//krml bs emhi user w ykun mwsul b chi ordeer aw cart ma ysir endi mchkel so b3mlu inactive
    }
}
