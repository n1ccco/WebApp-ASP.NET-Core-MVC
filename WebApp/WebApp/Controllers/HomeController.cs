using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class HomeController(ApplicationDbContext context) : Controller
    {
	    public async Task<IActionResult> Index(string? sortType)
		{
            switch (sortType)
            {
                case "DateAscending":
                    var dateAscending =
                        context.Posts.Include(p => p.Author).OrderBy(c => c.CreatedDateTime);
                    return View(await dateAscending.ToListAsync());
				case "NameAscending":
                    var nameAscending =
                        context.Posts.Include(p => p.Author).OrderBy(c => c.Title);
                    return View(await nameAscending.ToListAsync());
				case "NameDescending":
					var nameDescending =
						context.Posts.Include(p => p.Author).OrderByDescending(c => c.Title);
					return View(await nameDescending.ToListAsync());
				default:
                    var posts = context.Posts.Include(p => p.Author).OrderByDescending(c => c.CreatedDateTime);
                    return View(await posts.ToListAsync());
            }
			
		}
	}
}
