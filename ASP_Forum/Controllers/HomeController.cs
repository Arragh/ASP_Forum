using ASP_Forum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Forum.Controllers
{
    public class HomeController : Controller
    {
        ForumContext db;
        public HomeController(ForumContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Sections.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> ForumSection(int id)
        {
            ViewBag.SecId = id;
            return View(await db.Topics.ToListAsync());
        }

        public IActionResult CreateTopic(int id)
        {
            ViewBag.SecId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTopic(Topic topic)
        {
            db.Topics.Add(topic);
            await db.SaveChangesAsync();
            return RedirectToAction("ForumSection", "Home", new { @id = topic.SectionId });
        }

        public IActionResult ViewTopic(int id)
        {
            List<Reply> replies = new List<Reply>();
            foreach (var reply in db.Replies)
            {
                if (reply.TopicId == id)
                {
                    replies.Add(reply);
                }
            }
            ViewBag.Replies = replies;

            Topic topic = db.Topics.Single(t => t.Id == id);
            return View(topic);
        }

        public IActionResult CreateReply(int id)
        {
            ViewBag.TopicId = id;
            return View();
        }

        public async Task<IActionResult> AddReply(Reply reply)
        {
            db.Replies.Add(reply);
            await db.SaveChangesAsync();
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("ViewTopic", "Home", new { @id = reply.TopicId });
        }
    }
}
