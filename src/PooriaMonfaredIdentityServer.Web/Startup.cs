using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using CacheManager.Core;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;
using EFSecondLevelCache.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PooriaMonfaredIdentityServer.DataLayer;
using PooriaMonfaredIdentityServer.DataLayer.Context;
using PooriaMonfaredIdentityServer.DataLayer.Migrations;

namespace PooriaMonfaredIdentityServer.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { set; get; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath)
                            .AddInMemoryCollection(new[]
                                {
                                    new KeyValuePair<string,string>("the-key", "the-value"),
                                })
                            .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                            .AddJsonFile($"appsettings.{env}.json", optional: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            // configure identity server with in-memory users, but EF stores for clients and resources
            var migrationsAssembly = typeof(BaseForDetectDtlayer).GetTypeInfo().Assembly.GetName().Name;

            // ASP.NET Identity DbContext
            // Use a MS SQL Server database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly(migrationsAssembly)), ServiceLifetime.Scoped);


            // Use a SQLite database
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlite(
            //        "Filename=./pmServerIdentity.db",
            //        b => b.MigrationsAssembly(migrationsAssembly)
            //    ), ServiceLifetime.Scoped
            //);


            // Use a PostgreSQL database
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseNpgsql(
            //        connectionString,
            //        b => b.MigrationsAssembly(migrationsAssembly)
            //    )
            //);

            // ASP.NET Identity Registrations
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer().AddOperationalStore(builder => builder.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)))
                //.AddInMemoryClients(Clients.Get())
                //.AddInMemoryScopes(Scopes.Get())
                .AddConfigurationStore(
                    builder => builder.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)))
                //.AddInMemoryUsers(Users.Get())
                .AddAspNetIdentity<IdentityUser>()
                .AddTemporarySigningCredential();

            services.AddMvc();
        }

        public void Configure(IServiceScopeFactory scopeFactory, IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseStatusCodePages();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            scopeFactory.Initialize();

            app.UseEFSecondLevelCache();

            app.UseIdentity();
            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                 name: "default",
                 template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "login",
                    template: "login",
                    defaults: new { controller = "Account", action = "Login" });
                routes.MapRoute(
                    name: "register",
                    template: "register",
                    defaults: new { controller = "Account", action = "register" });

                routes.MapSpaFallbackRoute("spa-fallback", new { controller = "Home", action = "Index" });
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
