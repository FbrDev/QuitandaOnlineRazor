using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuitandaOnline.Data;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Configuration;
using QuitandaOnline.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using QuitandaOnline.Entities;
using QuitandaOnline.Services;

namespace QuitandaOnline
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                //habilita a necessidade de consentimento para uso de cookie
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true; //default = false
                options.Password.RequireNonAlphanumeric = false; //default = true
                options.Password.RequireUppercase = false; //default = true
                options.Password.RequireLowercase = false; //default = true
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3); //default = 3
                options.Lockout.MaxFailedAccessAttempts = 3; //default = 5
                options.SignIn.RequireConfirmedAccount = false; //default = false
                options.SignIn.RequireConfirmedEmail = true; //default = false
                options.SignIn.RequireConfirmedPhoneNumber = false; //default = false
            })
                .AddEntityFrameworkStores<QuitandaOnlineContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Login";
                options.SlidingExpiration = true;
            });

            builder.Services.AddAuthorization(option =>
            {
                //adiciona politica de acesso chamada isAdmin
                option.AddPolicy("isAdmin", policy => policy.RequireRole("admin"));
            });

            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/Admin", "isAdmin");
                options.Conventions.AuthorizeFolder("/ProdutoCRUD", "isAdmin");
            })
                .AddCookieTempDataProvider(options =>
                {
                    options.Cookie.IsEssential = true;
                })
                .AddRazorRuntimeCompilation();

            builder.Services.AddMvc();

            builder.Services.AddDbContext<QuitandaOnlineContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));

            builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();

            app.Run();

            var defaultCulture = new CultureInfo("pt-BR");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            };
            app.UseRequestLocalization(localizationOptions);
        }
    }
}