using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace RecycleBin.Model
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
    }
}
