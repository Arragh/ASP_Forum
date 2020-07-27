using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Forum.Models
{
    public class AuthorizationContext : IdentityDbContext<User>
    {
        public AuthorizationContext(DbContextOptions<AuthorizationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
