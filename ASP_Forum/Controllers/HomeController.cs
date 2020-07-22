using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP_Forum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return RedirectToAction("ForumSection", "Home", new {@id = topic.SectionId});
        }

        public IActionResult ViewTopic(int id)
        {
            ViewBag.TopicId = id;

            Topic topic = db.Topics.Single(t => t.Id == id);
            ViewBag.TopicName = topic.Name;
            ViewBag.UserName = topic.UserName;
            ViewBag.Body = topic.Body;
            return View();
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
            return RedirectToAction("Index", "Home");
        }
    }
}
