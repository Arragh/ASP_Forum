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
            // Передаем в представление список всех имеющихся разделов
            return View(await db.Sections.ToListAsync());
        }

        #region Просмотр раздела форума
        // Просмотр раздела форума
        [HttpGet]
        public IActionResult ForumSection(int id)
        {
            // Создаем список, в который положим темы, относящиеся к выбранному разделу
            List<Topic> topics = new List<Topic>();

            // Сделаем выборку из БД по Id раздела
            foreach (var topic in db.Topics)
            {
                // Положим удовлетворяющие элементы в список
                if (topic.SectionId == id)
                {
                    topics.Add(topic);
                }
            }

            // Id раздела для использования в представлении создания новой темы
            ViewBag.SecId = id;

            // Передаем список тем в представление
            return View(topics);
        }
        #endregion

        #region Создание новой темы
        // Создание новой темы
        [Authorize]
        public IActionResult CreateTopic(int id, bool error = false)
        {
            // Создаем переменную для сообщения об ошибке входных данных, если таковая будет
            ViewBag.Error = error;

            // Создаем переменную для привязки темы к разделу по Id
            ViewBag.SecId = id;
            return View();
        }
        #endregion

        #region Добавление новой темы в БД
        // Добавление новой темы в БД
        [Authorize]
        public async Task<IActionResult> AddTopic(Topic topic)
        {
            // Проверка входных данных на null и присваивание им пустых полей
            if (topic.Name == null)
            {
                topic.Name = "";
            }
            if (topic.Body == null)
            {
                topic.Body = "";
            }

            // Проверяем входные данные на корректность. И если всё в порядке, создаем новую тему
            if (topic.Name != "" && topic.Name.Length >= 5 && topic.Body != "" && topic.Body.Length >= 5)
            {
                db.Topics.Add(topic);
                await db.SaveChangesAsync();

                //return RedirectToAction("ForumSection", "Home", new { @id = topic.SectionId });
                return RedirectToAction("ViewTopic", "Home", new { @id = topic.Id });
            }

            return RedirectToAction("CreateTopic", "Home", new { @id = topic.SectionId, @error = true });
        }
        #endregion

        #region Просмотр темы
        // Просмотр темы
        public async Task<IActionResult> ViewTopic(int id)
        {
            // Создаем новый список, куда положим ответы на тему
            List<Reply> replies = new List<Reply>();

            // Делаем выборку ответов из БД
            await foreach (var reply in db.Replies)
            {
                // Если ответ принадлежит теме, то добавляем его в список
                if (reply.TopicId == id)
                {
                    replies.Add(reply);
                }
            }

            // Делаем переменную, которую используем на html-странице для вывода ответов
            ViewBag.Replies = replies;

            // Находим тему в БД
            Topic topic = await db.Topics.SingleAsync(t => t.Id == id);

            // Передаем тему в представление
            return View(topic);
        }
        #endregion

        #region Создание ответа в теме
        // Создание ответа в теме
        [Authorize]
        public IActionResult CreateReply(int id, bool error = false)
        {
            // Создаем переменную для сообщения об ошибке входных данных, если таковая будет
            ViewBag.Error = error;

            // Создаем переменную для привязки ответа к теме по Id
            ViewBag.TopicId = id;
            return View();
        }
        #endregion

        #region Добавление ответа в БД
        // Добавление ответа в БД
        [Authorize]
        public async Task<IActionResult> AddReply(Reply reply)
        {
            // Проверка входных данных на null и присваивание им пустых полей
            if (reply.ReplyBody == null)
            {
                reply.ReplyBody = "";
            }

            // Проверка входных данных на корректность, и если все хорошо, то добавляем ответ в БД и делаем редирект обратно в тему
            if (reply.ReplyBody != "" && reply.ReplyBody.Length >= 5)
            {
                await db.Replies.AddAsync(reply);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewTopic", "Home", new { @id = reply.TopicId });
            }

            // Если данные не корректны, повторно загружаем страницу создания ответа с сообщением об ошибке
            return RedirectToAction("CreateReply", "Home", new { @id = reply.TopicId, @error = true });
        }
        #endregion
    }
}
