using ASP_Forum.Authorization;
using ASP_Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ASP_Forum.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Страница регистрации нового пользователя
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        #region Метод регистрации нового пользователя
        // Метод регистрации нового пользователя
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            // Если данные модели введены без ошибок, то заходим в тело if
            if (ModelState.IsValid)
            {
                // Создаем нового пользователя и присваиваем ему введенные UserName и Email
                User user = new User { UserName = model.UserName, Email = model.Email };

                // Добавляем нового пользователя в базу, присваивая ему введенный в форме пароль
                var result = await _userManager.CreateAsync(user, model.Password);

                // Если добавление пользователя в базу прошло успешно, то автоматически выполняем вход под ним
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                // Либо выводим сообщения об ошибках
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Возвращаем модель в представление (возможно model не обязательна, надо проверить)
            return View(model);
        }
        #endregion

        // Страница авторизации пользователя
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            // Возвращаем в представление модель с адресом, с которого нас перекинуло на авторизацию
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        #region Метод авторизации пользователя
        // Метод авторизации пользователя
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            // Если все введенные данные верно, то заходим в тело if
            if (ModelState.IsValid)
            {
                // Авторизируемся под пользователем
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                // Если авторизация прошла успешно, заходим в тело if
                if (result.Succeeded)
                {
                    // Если строка адреса не пустая и принадлежит нашему сайту, то перенаправляемся на неё
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    // Либо перенаправляемся на главную страницу нашего сайта
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                // Если в авторизации отказано, то выводим сообщения о причинах отказа
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }

            // model в представлении вроде не обязателен. Пока непонятно
            return View(model);
        }
        #endregion

        // Разлогинивание
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
