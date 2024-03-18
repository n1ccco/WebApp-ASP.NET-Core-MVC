using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class HomeController(ApplicationDbContext context) : Controller
    {
	    public async Task<IActionResult> Index(string? sortType, string? search)
		{
			IOrderedQueryable<Models.Post> list;
			switch (sortType)
            { 
		            
                case "DateAscending":
                    list = context.Posts.Include(p => p.Author).OrderBy(c => c.CreatedDateTime);
                    break;
				case "NameAscending":
                    list = context.Posts.Include(p => p.Author).OrderBy(c => c.Title);
                    break;
				case "NameDescending":
					list = context.Posts.Include(p => p.Author).OrderByDescending(c => c.Title);
					break;
				default:
                    list = context.Posts.Include(p => p.Author).OrderByDescending(c => c.CreatedDateTime);
                    break;
            }

			if (search != null)
			{
				list = context.Posts.Include(p => p.Author).Where(s => s.Body.Contains(search)).OrderByDescending(c => c.CreatedDateTime);
			}
			return View(await list.ToListAsync());

		}
	}
}
