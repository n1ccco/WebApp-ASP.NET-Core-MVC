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
                    new User { Username = "Pablo", Email = "pablo.drugs@mexico.mx" }
                );
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Posts.Any())
            {
                dbContext.Posts.AddRange(
                    new Post { Title = "Mexico", Body = "A good way to the USA", AuthorId = 1 }
                );
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Comments.Any())
            {
                dbContext.Comments.AddRange(
                    new Comment { Content = "Good description", AuthorId = 1, PostId = 1 }
                );
                await dbContext.SaveChangesAsync();
            }
        }
    }
}