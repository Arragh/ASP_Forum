using ASP_Forum.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
            ViewBag.Error = error;
            return View();
        }

        public async Task<IActionResult> AddSection(Section section)
        {
            if (section.Name == null)
            {
                section.Name = "";
            }

            if (section.Name != "" && section.Name.Length >= 5)
            {
                await db.Sections.AddAsync(section);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Admin", new { @error = true });
        }

        public async Task<IActionResult> DeleteSection(Section section)
        {
            Section sectionToDelete = db.Sections.FirstOrDefault(s => s.Name == section.Name);
            List<Topic> topics = new List<Topic>();
            List<Reply> replies = new List<Reply>();

            foreach (var topic in db.Topics)
            {
                if (topic.SectionId == sectionToDelete.Id)
                {
                    topics.Add(topic);
                }
            }
            foreach (var reply in db.Replies)
            {
                foreach (var topic in topics)
                {
                    if (reply.TopicId == topic.Id)
                    {
                        replies.Add(reply);
                    }
                }
            }

            db.Replies.RemoveRange(replies);
            db.Topics.RemoveRange(topics);
            db.Sections.Remove(sectionToDelete);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
