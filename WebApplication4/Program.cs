using ForumWebSite;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.RegularExpressions;
using WebApplication4;

namespace Program
{
    
    public class Program
    {
        public static void Main(params string[] args)
        {

            SQLDataProvider.ConnectionString = args.Aggregate((x,y)=> x+" "+y);
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddMvc();
            builder.Services.AddSession();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option => { option.AccessDeniedPath = "/Main/Login";option.LoginPath = "/Main/Login"; });
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<IDataProvider,SQLDataProvider>();
            var app = builder.Build();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseStaticFiles(); 
            app.UseSession();
            app.UseMiddleware<LoggerMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller}/{action}");
                endpoints.MapControllerRoute(name: "default", pattern: "{controller}/{action}/{param}");
                endpoints.Map("/", async (context) => context.Response.Redirect("/Main/Index"));
            });
            app.Run();
        }
    }
}

