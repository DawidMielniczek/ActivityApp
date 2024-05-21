using ApkaTa.Models;
using ApkaTa.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApkaTa.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly IUserRepository userRepository = new UserRepository();
        public UserViewModel _User { get; set; } = new();


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.IdU = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await userRepository.GetUserId(ViewBag.IdU);
            _User = user;
            return View(_User);
        }

        public IActionResult Aktywność()
        {

            return RedirectToAction("Index", "Aktywność");

        }
        public IActionResult Wpisy()
        {
            return RedirectToAction("Index", "Wpisy");
        }
        public IActionResult Profil()
        {
            return RedirectToAction("Profil", "Konto");
        }

        public IActionResult Ustawienia()
        {
            return RedirectToAction("Index", "Konto");
        }
        public IActionResult Home()
        {
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> About()
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await userRepository.GetUserId(ViewBag.IdU);
            _User = user;
            return View(_User);
        }
        public async Task<IActionResult> Wyloguj()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
         public async Task<IActionResult> Nadchodzace()
        {
            return RedirectToAction("NadchodzaceWyd", "Aktywność");
        }
        public async Task<IActionResult> HistoriaWydarzeń()
        {
            return RedirectToAction("HistoriaWydarzeń", "Aktywność");
        }
        public async Task<IActionResult> LastHistoriaUserDetalies()
        {
            return RedirectToAction("LastHistoriaUserDetalies", "Aktywność");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}