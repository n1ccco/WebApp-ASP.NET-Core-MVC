using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController(ApplicationDbContext context) : Controller
    {
	    public IActionResult Index()
		{
			var posts = context.Posts.Include(p => p.Author).ToList();
			return View(posts);
		}
	}
}
