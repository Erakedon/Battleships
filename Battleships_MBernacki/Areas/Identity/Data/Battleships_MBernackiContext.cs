using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleships_MBernacki.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Battleships_MBernacki.Models
{
    public class Battleships_MBernackiContext : IdentityDbContext<Battleships_MBernackiUser>
    {
        public Battleships_MBernackiContext(DbContextOptions<Battleships_MBernackiContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
