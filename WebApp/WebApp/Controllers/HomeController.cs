using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController(ApplicationDbContext context) : Controller
    {
	    public async Task<IActionResult> Index(string? sortType)
		{
			if (sortType is "Queryable")
			{
				var orderedQueryable =
					context.Posts.Include(p => p.Author).OrderBy(c => c.CreatedDateTime);
				return View(await orderedQueryable.ToListAsync());
			}
			var posts = context.Posts.Include(p => p.Author).OrderByDescending(c => c.CreatedDateTime);
			return View(await posts.ToListAsync());
		}
	}
}
