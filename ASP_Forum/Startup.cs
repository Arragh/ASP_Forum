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
            // ���������� �������� �� ������
            services.AddDbContext<ForumContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ForumConnection")));

            // ���������� �������� �� ��� ����������� ������������� (Identity)
            services.AddDbContext<AuthorizationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            // ����������� ������� Identity
            services.AddIdentity<User, IdentityRole>(options =>
            {
                // ������ ���������� �������� ������
                options.User.RequireUniqueEmail = true;
                // ������ ����������� �������� � ����� �������������
                options.User.AllowedUserNameCharacters = ".@0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz�����Ũ����������������������������������������������������������";
            })
                .AddEntityFrameworkStores<AuthorizationContext>();

            // ��������� ����������� �������� � ������� MVC � �������������, �� ����� ������ ���� ���������������
            services.AddControllersWithViews().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // ���������� �������������� � �����������
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
