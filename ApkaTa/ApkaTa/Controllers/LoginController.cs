using ApkaTa.Models;
using ApkaTa.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace ApkaTa.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public async  Task<IActionResult> Index()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index","Home");
            }
           

            return View();
        }
      

        [HttpPost]
        public async Task<IActionResult> Index(UserViewModel userView)
        {
            IUserRepository repo = new UserRepository();
            var userModel = await repo.GetUserInfo(userView.Email, userView.Password );

            if (userModel == null)
            {
                TempData["error"] = "Nieprawidłowe dane logowania!";

            }
            else
            {
                List<Claim> claims = new List<Claim>() {
                    new  Claim(ClaimTypes.NameIdentifier, userModel.IdU.ToString()),
                    new Claim("OtherProperties", "Example Role")
                    };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = userView.czyZalogowany


                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                return RedirectToAction("Index","Home", userModel.IdU);
            }
            return View(userView);
        }

        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult Rejestruj()
        {
            return RedirectToAction("Index", "Rejestracja");
        }
    }  
    
}
