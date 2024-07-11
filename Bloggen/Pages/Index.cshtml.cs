using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bloggen.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        public IndexModel(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Models.Blog> Blogs { get; set; }


        [BindProperty]
        public Models.Blog Blog { get; set; }

        [BindProperty]
        public IFormFile UploadedImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int deleteid)
        {
            if(deleteid != 0)
            {
                Models.Blog blog = await _context.Blog.FindAsync(deleteid);

                if(blog != null)
                {
                    if (System.IO.File.Exists("./wwwroot/img/" + blog.Image))
                    {
                        System.IO.File.Delete("./wwwroot/img/" + blog.Image);
                    }
                    _context.Blog.Remove(blog);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
            }



            Blogs = await _context.Blog.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string fileName = string.Empty;

            if(UploadedImage != null)
            {
                Random rnd = new();
                fileName = rnd.Next(0, 100000).ToString() + UploadedImage.FileName;
                var file = "./wwwroot/img/" + fileName;
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await UploadedImage.CopyToAsync(fileStream);
                }
            }

            Blog.Date = DateTime.Now;
            Blog.Image = fileName;
            Blog.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _context.Add(Blog);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}