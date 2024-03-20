using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class PostsController(ApplicationDbContext context) : Controller
    {

		// GET: Posts/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await context.Posts
	            .Include(c => c.Categories)
                .Include(com => com.Comments.OrderByDescending(c=>c.CreatedDateTime))
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(new PostDetailsViewModel{
	            Post = post
	            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int? id, PostDetailsViewModel vm)
        {
	        ModelState.Remove("Post");
	        ModelState.Remove("Comment.Post");
			if (ModelState.IsValid && vm.Comment != null)
	        {
		        await context.AddAsync(vm.Comment);
		        await context.SaveChangesAsync();
	        }

	        return RedirectToAction(nameof(Details));
        }


		// GET: Posts/Create
		public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(context.Users, "Id", "Email");
            ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "Name");
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
                var categoryToAdd = await context.Categories.FindAsync(category);
                if (categoryToAdd != null)
                {
                    postViewModel.Post.Categories.Add(categoryToAdd);
                }
            }
            

            ModelState.Remove("Post.Author");
            if (ModelState.IsValid)
            {
                context.Add(postViewModel.Post);
                await context.SaveChangesAsync();
				return RedirectToAction("Index", "Home");
			}
            ViewData["AuthorId"] = new SelectList(context.Users, "Id", "Email", postViewModel.Post.AuthorId);
            ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "Name");
            return View(postViewModel);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await context.Posts.
                                            //Include(a => a.Author).
                                            Include(c => c.Categories).
                                            FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            int[] selectedCategories = post.Categories.Select(i => i.Id).ToArray();
            ViewData["AuthorId"] = new SelectList(context.Users, "Id", "Email", post.AuthorId);
            ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "Name", selectedCategories);
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
                    var postToUpdate = await context.Posts
                        .Include(p => p.Categories)
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (postToUpdate == null)
                    {
                        return NotFound();
                    }

                    postToUpdate.Title = postViewModel.Post.Title;
                    postToUpdate.Body = postViewModel.Post.Body;
                    postToUpdate.AuthorId = postViewModel.Post.AuthorId;

                    postToUpdate.Categories.Clear();
                    foreach (var categoryId in postViewModel.SelectedCategories)
                    {
                        var categoryToAdd = await context.Categories.FindAsync(categoryId);
                        if (categoryToAdd != null)
                        {
                            postToUpdate.Categories.Add(categoryToAdd);
                        }
                    }


                    context.Update(postToUpdate);
                    await context.SaveChangesAsync();
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
				return RedirectToAction("Index", "Home");
			}
            ViewData["AuthorId"] = new SelectList(context.Users, "Id", "Email", postViewModel.Post.Author);
            ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "Name");
            return View(postViewModel);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await context.Posts
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
            var post = await context.Posts.FindAsync(id);
            if (post != null)
            {
                context.Posts.Remove(post);
            }

            await context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }

        private bool PostExists(int id)
        {
            return context.Posts.Any(e => e.Id == id);
        }
    }
}
