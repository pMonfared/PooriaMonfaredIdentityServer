using Microsoft.Extensions.DependencyInjection;

namespace PooriaMonfaredIdentityServer.DataLayer.Migrations
{
    public static class ApplicationDbContextSeedData
    {
        public static void SeedData(this IServiceScopeFactory scopeFactory)
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                //var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                //if (context.Users.Any()) return;
                //var persons = new List<ApplicationUser>
                //{
                //    new ApplicationUser
                //    {
                //        FirstName = "Admin",
                //        LastName = "User",
                //    }
                //};
                //context.AddRange(persons);
                //context.SaveChanges();
            }
        }
    }
}