using System;
using Battleships_MBernacki.Areas.Identity.Data;
using Battleships_MBernacki.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Battleships_MBernacki.Areas.Identity.IdentityHostingStartup))]
namespace Battleships_MBernacki.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<Battleships_MBernackiContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("Battleships_MBernackiContextConnection")));

                services.AddDefaultIdentity<Battleships_MBernackiUser>()
                    .AddEntityFrameworkStores<Battleships_MBernackiContext>();
            });
        }
    }
}