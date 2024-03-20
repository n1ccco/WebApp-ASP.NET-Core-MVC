using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class HomeController(ApplicationDbContext context) : Controller
    {
		public async Task<IActionResult> Index(string? sortType, string? search, int? category, int? author)
		{
			IQueryable<Models.Post> list = context.Posts.Include(p => p.Author);

			switch (sortType)
			{
				case "DateAscending":
					list = list.OrderBy(c => c.CreatedDateTime);
					break;
				case "NameAscending":
					list = list.OrderBy(c => c.Title);
					break;
				case "NameDescending":
					list = list.OrderByDescending(c => c.Title);
					break;
				default:
					list = list.OrderByDescending(c => c.CreatedDateTime);
					break;
			}

			if (!string.IsNullOrEmpty(search))
			{
				list = list.Where(s => s.Body.Contains(search));
			}

			if (category.HasValue)
			{
				list = list.Where(p => p.Categories.Any(c => c.Id == category));
			}

			if (author.HasValue)
			{
				list = list.Where(p => p.Author.Id == author);
			}

			return View(await list.ToListAsync());
		}
	}
}
