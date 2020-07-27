using ASP_Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<IActionResult> Index(bool error = false)
        {
            if (error)
            {
                return Content("Содержимое полей не может быть короче 5 символов");
            }
            return View(await db.Sections.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> ForumSection(int id)
        {
            ViewBag.SecId = id;
            return View(await db.Topics.ToListAsync());
        }

        [Authorize]
        public IActionResult CreateTopic(int id, bool error = false)
        {
            ViewBag.Error = error;
            ViewBag.SecId = id;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> AddTopic(Topic topic)
        {
            if (topic.Name == null)
            {
                topic.Name = "";
            }
            if (topic.UserName == null)
            {
                topic.UserName = "";
            }
            if (topic.Body == null)
            {
                topic.Body = "";
            }

            // Проверяем входные данные на корректность. И если всё в порядке, создаем новую тему
            if (topic.Name != "" && topic.Name.Length >= 5 && topic.UserName != "" && topic.UserName.Length >= 5 && topic.Body != "" && topic.Body.Length >= 5)
            {
                db.Topics.Add(topic);
                await db.SaveChangesAsync();
                //return RedirectToAction("ForumSection", "Home", new { @id = topic.SectionId });
                return RedirectToAction("ViewTopic", "Home", new { @id = topic.Id });
            }

            return RedirectToAction("CreateTopic", "Home", new { @id = topic.SectionId, @error = true });
        }

        public async Task<IActionResult> ViewTopic(int id)
        {
            List<Reply> replies = new List<Reply>();
            await foreach (var reply in db.Replies)
            {
                if (reply.TopicId == id)
                {
                    replies.Add(reply);
                }
            }
            ViewBag.Replies = replies;

            Topic topic = await db.Topics.SingleAsync(t => t.Id == id);
            return View(topic);
        }

        [Authorize]
        public IActionResult CreateReply(int id, bool error = false)
        {
            ViewBag.Error = error;
            ViewBag.TopicId = id;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> AddReply(Reply reply)
        {
            if (reply.UserName == null)
            {
                reply.UserName = "";
            }
            if (reply.ReplyBody == null)
            {
                reply.ReplyBody = "";
            }

            if (reply.UserName != "" && reply.UserName.Length >= 5 && reply.ReplyBody != "" && reply.ReplyBody.Length >= 5)
            {
                await db.Replies.AddAsync(reply);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewTopic", "Home", new { @id = reply.TopicId });
            }

            return RedirectToAction("CreateReply", "Home", new { @id = reply.TopicId, @error = true });
        }
    }
}
