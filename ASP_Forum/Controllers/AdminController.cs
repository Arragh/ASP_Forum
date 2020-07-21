using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP_Forum.Models;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult AddSection(Section section)
        {
            db.Sections.Add(section);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
