using ASP_Forum.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Forum.Controllers
{
    public class AdminController : Controller
    {
        ForumContext db;
        public AdminController(ForumContext context)
        {
            db = context;
        }

        public IActionResult Index(bool error = false)
        {
            if (error)
            {
                return Content("Название раздела не может быть короче 5 символов");
            }
            return View();
        }

        public async Task<IActionResult> AddSection(Section section)
        {
            if (section.Name == null)
            {
                section.Name = "";
            }

            if (section.Name != "" && section.Name.Length > 5)
            {
                await db.Sections.AddAsync(section);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Admin", new { @error = true });
        }
    }
}
