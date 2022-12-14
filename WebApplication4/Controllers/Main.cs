using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication4.Models;
#nullable disable
namespace WebApplication4.Controllers
{
    public class Main : Controller
    {
        private readonly IDataProvider _provider;
        public Main(IDataProvider dataProvider)
        {
            _provider=dataProvider;
        }
        public IActionResult Index()
        {
            List<ArticleModel> models = _provider.GetArticles();
            return View("Index",model: models);
        }
        public IActionResult Login()
        {
            return View("Login");
        }
        public IActionResult Register()
        {
            return View("Register");
        }
        public IActionResult CreateArticle()
        {   if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("CreateArticle");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CreateArticle(ArticleModel model)
        {
            if (ModelState.IsValid && HttpContext.User.Identity.IsAuthenticated)
            {
                _provider.CreateArticle(model, _provider.UserId(HttpContext.User.FindFirst(ClaimTypes.Email).Value));
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid && _provider.CheckUser(model.Email.ToLower(), model.Password))
            {
                List<Claim> claims = new List<Claim>() { new Claim(ClaimTypes.Email,model.Email.ToLower()),new Claim(ClaimTypes.Name,_provider.GetName(model))};
                ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid && _provider.CheckUser(model.Email.ToLower()))
            {
                _provider.CreateUser(model);
                return RedirectToAction("Index");
            }
            return View();
        }
        
        public IActionResult Article(int param)
        {
            return View("Article",model:_provider.GetArticle(param));
        }
        [HttpPost]
        public void Article(int param, CommentModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                model.ArticleId = param;
                model.AuthorId = _provider.UserId(HttpContext.User.FindFirst(ClaimTypes.Email).Value);
                _provider.CreateComment(model);
                HttpContext.Response.Redirect($"/Main/Article/{param}");
            }
        }
    }
}
