using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Bobbins.Frontend.Data;
using Bobbins.Frontend.Data.Models;
using Bobbins.Frontend.Models.Links;
using Bobbins.Frontend.Options;
using Bobbins.Frontend.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using StackExchange.Redis;

namespace Bobbins.Frontend
{
    [PublicAPI]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var dpRedis = Configuration.GetValue("DataProtection:Redis", string.Empty);
            if (!string.IsNullOrWhiteSpace(dpRedis))
            {
                services.AddDataProtection().PersistKeysToRedis(() => ConnectionMultiplexer.Connect(dpRedis).GetDatabase(), "Bobbins:Frontend:DP");
            }

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, BobbinsClaimsPrincipalFactory>();

            services.Configure<ServiceOptions>(Configuration.GetSection("Services"));
            services.AddSingleton<ILinkService, LinkService>();
            services.AddSingleton<ICommentService, CommentService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
