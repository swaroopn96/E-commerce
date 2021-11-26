using System;
using Ecommerce.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Ecommerce.Areas.Identity.IdentityHostingStartup))]
namespace Ecommerce.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<EcommerceIdentityDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("EcommerceIdentityDbContextConnection")));

                //Commented since we added identity in main startup.cs other wise it throws exception as InvalidOperationException: Scheme already exists: Identity.Application
                //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //    .AddEntityFrameworkStores<EcommerceIdentityDbContext>();
            });
        }
    }
}