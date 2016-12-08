using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PooriaMonfaredIdentityServer.DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PooriaMonfaredIdentityServer.DataLayer.Configuration;
using IdentityServer4.EntityFramework.Mappers;

namespace PooriaMonfaredIdentityServer.DataLayer.Migrations
{
    public static class DbInitialization
    {
        public static void Initialize(this IServiceScopeFactory scopeFactory)
        {
        //    using (var serviceScope = scopeFactory.CreateScope())
        //    {
        //        var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        //        // Applies any pending migrations for the context to the database.
        //        // Will create the database if it does not already exist.
        //        context.Database.Migrate();
        //    }

            using (var scope = scopeFactory.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                if (!context.Clients.Any())
                {
                    foreach (var client in Clients.Get())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Resources.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Resources.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                if (!userManager.Users.Any())
                {
                    foreach (var inMemoryUser in Users.Get())
                    {
                        var identityUser = new IdentityUser(inMemoryUser.Username)
                        {
                            Id = inMemoryUser.Subject
                        };

                        foreach (var claim in inMemoryUser.Claims)
                        {
                            identityUser.Claims.Add(new IdentityUserClaim<string>
                            {
                                UserId = identityUser.Id,
                                ClaimType = claim.Type,
                                ClaimValue = claim.Value,
                            });
                        }

                        userManager.CreateAsync(identityUser, "Password1234").Wait();
                    }
                }
            }
        }
    }
}