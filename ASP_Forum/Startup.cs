using ASP_Forum.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP_Forum
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // подключаем контекст БД форума
            services.AddDbContext<ForumContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ForumConnection")));

            // Подключаем контекст БД для регистрации пользователей (Identity)
            services.AddDbContext<AuthorizationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            // Настраиваем сервисы Identity
            services.AddIdentity<User, IdentityRole>(options =>
            {
                // Только уникальные почтовые адреса
                options.User.RequireUniqueEmail = true;
                // Список разрешенных символов в имени пользователей
                options.User.AllowedUserNameCharacters = ".@0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            })
                .AddEntityFrameworkStores<AuthorizationContext>();

            // Добавляем возможность работать с моделью MVC и устанавливаем, на какую версию ядра ориентироваться
            services.AddControllersWithViews().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Подключаем аутентификацию и авторизацию
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
