using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Author);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateViewModel postViewModel)
        {

            foreach (var category in postViewModel.SelectedCategories)
            {
                var categoryToAdd = await _context.Categories.FindAsync(category);
                if (categoryToAdd != null)
                {
                    postViewModel.Post.Categories.Add(categoryToAdd);
                }
            }
            

            ModelState.Remove("Post.Author");
            if (ModelState.IsValid)
            {
                _context.Add(postViewModel.Post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Email", postViewModel.Post.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View(postViewModel);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.
                                            //Include(a => a.Author).
                                            Include(c => c.Categories).
                                            FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            int[] selectedCategories = post.Categories.Select(i => i.Id).ToArray();
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Email", post.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", selectedCategories);
            return View(new PostCreateViewModel
            {
                Post = post,
                SelectedCategories = selectedCategories
            });
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostCreateViewModel postViewModel)
        {
            if (id != postViewModel.Post.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Post.Author");

            if (ModelState.IsValid)
            {
                try
                {
                    var postToUpdate = await _context.Posts
                        //.Include(a => a.Author)
                        .Include(p => p.Categories)
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (postToUpdate == null)
                    {
                        return NotFound();
                    }

                    postToUpdate.Title = postViewModel.Post.Title;
                    postToUpdate.Body = postViewModel.Post.Body;
                    postToUpdate.AuthorId = postViewModel.Post.AuthorId;

                    // Clear existing categories and add selected categories
                    postToUpdate.Categories.Clear();
                    foreach (var categoryId in postViewModel.SelectedCategories)
                    {
                        var categoryToAdd = await _context.Categories.FindAsync(categoryId);
                        if (categoryToAdd != null)
                        {
                            postToUpdate.Categories.Add(categoryToAdd);
                        }
                    }


                    _context.Update(postToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(postViewModel.Post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Email", postViewModel.Post.Author);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View(postViewModel);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
