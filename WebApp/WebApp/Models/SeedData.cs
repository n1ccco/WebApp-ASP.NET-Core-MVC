using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.MigrateAsync();

            if (!dbContext.Categories.Any())
            {
                dbContext.Categories.AddRange(
                    new Category { Name = "Country" },
                    new Category { Name = "City" }
                );
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Users.Any())
            {
                dbContext.Users.AddRange(
                    new User { Username = "Pablo", Email = "pablo.traveler@mexico.mx" }
                );
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Posts.Any())
            {
                dbContext.Posts.AddRange(
                    new Post { Title = "Embracing Nature's Wonders: Eco-Tourism Adventures", Body = "Dive into the world of eco-tourism and embark on sustainable adventures! Explore biodiverse rainforests in Costa Rica, snorkel in pristine coral reefs in Australia, or hike through ancient forests in Canada. Eco-tourism not only allows you to witness nature's wonders up close but also promotes conservation and supports local communities. Experience the beauty of our planet responsibly and leave a positive impact on the environment.", AuthorId = 1 }
                );
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Comments.Any())
            {
                dbContext.Comments.AddRange(
                    new Comment { Content = "Good Post", Name = "Joe", PostId = 1 }
                );
                await dbContext.SaveChangesAsync();
            }
        }
    }
}