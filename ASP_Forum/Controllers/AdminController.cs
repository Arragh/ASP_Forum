using ASP_Forum.Models;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddSection(Section section)
        {
            await db.Sections.AddAsync(section);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
