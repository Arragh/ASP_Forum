using ASP_Forum.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult Index(bool error = false)
        {
            // Переменная для вывода сообщения об ошибке
            ViewBag.Error = error;

            return View();
        }

        #region Добавление раздела
        // Добавление раздела
        [Authorize]
        public async Task<IActionResult> AddSection(Section section)
        {
            // Проверка входных данных на null
            if (section.Name == null)
            {
                section.Name = "";
            }

            // Проверка на дубликат названия раздела
            foreach (var item in db.Sections)
            {
                // Если раздел с таким именем существует, выводим сообщение об ошибке
                if (section.Name == item.Name)
                {
                    return RedirectToAction("Index", "Admin", new { @error = true });
                }
            }

            // Проверка входных данных на соответствие условиям
            if (section.Name != "" && section.Name.Length >= 5)
            {
                // Добавление в БД и редирект на главную страницу
                await db.Sections.AddAsync(section);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            // Срабатывает сообщение об ошибке, если входные данные не соответствуют заданным условиям
            return RedirectToAction("Index", "Admin", new { @error = true });
        }
        #endregion

        #region Удаление раздела
        // Удаление раздела
        [Authorize]
        public async Task<IActionResult> DeleteSection(Section section)
        {
            // Создаем аналог принятого в метод параметра, иначе будет ошибка при выполнении
            Section sectionToDelete = db.Sections.FirstOrDefault(s => s.Name == section.Name);

            // Создаем списки тем и сообщений в них
            List<Topic> topics = new List<Topic>();
            List<Reply> replies = new List<Reply>();

            // Делаем выборку из тем в БД, которые привязаны к удаляемому разделу и добавляем их в наш список
            foreach (var topic in db.Topics)
            {
                if (topic.SectionId == sectionToDelete.Id)
                {
                    topics.Add(topic);
                }
            }

            // Делаем выборку из ответов в БД, которые привязаны к темам удаляемого раздела, и добавляем их в список
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

            // Удаляем из БД записи в соответствии с созданными списками
            db.Replies.RemoveRange(replies);
            db.Topics.RemoveRange(topics);

            // Удаляем сам раздел через созданную копию
            db.Sections.Remove(sectionToDelete);
            await db.SaveChangesAsync();

            // Редирект на главную страницу
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
