using IdeaExchange.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace IdeaExchange
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<IdeaExchangeContext>(x => x.UseNpgsql(connectionString));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                options => options.SignIn.RequireConfirmedEmail = false)
            .AddEntityFrameworkStores<IdeaExchangeContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication()
            .AddCookie();

            builder.Services.ConfigureApplicationCookie(opts =>
            {
                opts.LoginPath = "/ApplicationUser/Login";

            });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Information() // Set the minimum log level
             .WriteTo.File("logs/myapp-{Date}.log", LogEventLevel.Information)
             .CreateLogger();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "Default",
                pattern: "{Controller}/{action}/{id?}"
            );

            app.MapControllerRoute(
            name: "publicationDetails",
            pattern: "Publication/Details/{id}",
            defaults: new { controller = "Publication", action = "Details" }
            );

            app.Run();

            Log.CloseAndFlush();
        }

    }
}