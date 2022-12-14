using System.Security.Claims;
using WebApplication4;

namespace ForumWebSite
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            IDataProvider _provider = context.RequestServices.GetService<IDataProvider>();
            Console.WriteLine($"{context.Connection.RemoteIpAddress.MapToIPv4()} {_provider.UserId(context.User.Identities.First().FindFirst(ClaimTypes.Email)?.Value ?? "" )} requested page {context.Request.Path}");
            _provider.CreateLog(context.Connection.RemoteIpAddress.MapToIPv4().ToString(), $"requested page {context.Request.Path}", _provider.UserId(context.User.Identities.First().FindFirst(ClaimTypes.Email)?.Value ?? ""));
            await _next.Invoke(context);
        }
    }
}
