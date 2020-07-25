using ASP_Forum.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public IActionResult Index(bool error)
        {
            if (error)
            {
                return Content("Название раздела должно содержать только строчные либо прописные символы русского алфавита, а так же не может быть короче 5 символов");
            }
            return View();
        }

        public async Task<IActionResult> AddSection(Section section)
        {
            bool error = false;

            if (section.Name == null)
            {
                section.Name = "";
            }

            foreach (var c in section.Name.ToArray())
            {
                if ((c < 'а' || c > 'я') && (c < 'А' || c > 'Я') && c != ' ')
                {
                    error = true;
                    break;
                }
            }

            if (section.Name != "" && section.Name.Length > 5 && error == false)
            {
                await db.Sections.AddAsync(section);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Admin", new { @error = true });
        }
    }
}
