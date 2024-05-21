using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApkaTa.Services;
using ApkaTa.Models;
using System.Security.Claims;


namespace ApkaTa.Controllers
{
    public class Konto : Controller
    {
        

        readonly IUserRepository userRepository = new UserRepository();
        public UserViewModel _User { get; set; } = new();
        public List<UserViewModel> AllUser { get; set; } = new();
        public async void GetInfo()
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await userRepository.GetUserId(ViewBag.IdU);
            _User = user;
            ViewBag.Nick = _User.Nick;

        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
            GetInfo();
            var user = await userRepository.GetUserId(ViewBag.IdU);
            _User = user;
            return View(_User);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Index(UserViewModel usr)
        {

            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
            GetInfo();
            var user = await userRepository.GetUserId(ViewBag.IdU);
            _User = user;
            var all = await userRepository.GetUser();
           foreach( var x in all)
            {
                AllUser.Add(x);
            }

            if (AllUser.Where(x => x.Email == usr.Email && x.IdU == ViewBag.IdU).Any())
            {
                ModelState["Płeć"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

                ModelState["Bmi"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

                if (ModelState.IsValid)
                {
                    var userUpdate = new UserViewModel
                    {
                        IdU = ViewBag.IdU,
                        Nick = _User.Nick,
                        Password = usr.Password,
                        Email = usr.Email,
                        Waga = _User.Waga,
                        Wzrost = _User.Wzrost,
                        Płeć = _User.Płeć,
                        RokUrodzenia = _User.RokUrodzenia,
                        Bmi = "0"
                    };
                    if (await userRepository.UpdateUserInfo(userUpdate))
                    {
                        TempData["succes"] = "Dane zostały zaaktualizowane!";
                        return View(userUpdate);
                    }
                }
            }
            else
            {
                ModelState["Bmi"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                ModelState["Płeć"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;


                if (ModelState.IsValid)
                {
                    var userUpdate = new UserViewModel
                    {
                        IdU = ViewBag.IdU,
                        Nick = _User.Nick,
                        Password = usr.Password,
                        Email = usr.Email,
                        Waga = _User.Waga,
                        Wzrost = _User.Wzrost,
                        Płeć = _User.Płeć,
                        RokUrodzenia = _User.RokUrodzenia,
                        Bmi = "0"
                    };
                    if (await userRepository.UpdateUserInfo(userUpdate))
                    {
                        TempData["succes"] = "Dane zostały zaaktualizowane!";
                        return View(userUpdate);
                    }
                }
            }

            TempData["Error"] = "Niepoprawne dane!";
            _User = user;
            return View(_User);
        }

        [HttpGet]
        public async Task<IActionResult> Profil()
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await userRepository.GetUserId(ViewBag.IdU);
            _User = user;
            ViewBag.Nick = _User.Nick;
            return View(_User);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profil(UserViewModel userDetalies)
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var _user = await  userRepository.GetUserId(ViewBag.IdU);

            ModelState["Płeć"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            ModelState["Bmi"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            if (!ModelState.IsValid)
            {
                ViewBag.Nick = _user.Nick;
                TempData["Error"] = "Wprowadź poprawne dane w formularzu!";

                return View(userDetalies);

            }
            
                if (userDetalies.Płeć is null)
                {
                    userDetalies.Płeć = "";
                }
                var update = new UserViewModel()
                {
                    IdU = ViewBag.IdU,
                    Nick = userDetalies.Nick,
                    RokUrodzenia = userDetalies.RokUrodzenia,
                    Płeć = userDetalies.Płeć,
                    Waga = userDetalies.Waga,
                    Wzrost = userDetalies.Wzrost,
                    Email = _user.Email,
                    Password = _user.Password,
                    Bmi = "0"
                };

                if (await userRepository.UpdateUserInfo(update))
                {
                    TempData["AlertMessage"] = "Dane zostały zaaktualizowane!";
                     ViewBag.Nick = userDetalies.Nick;

                return View(userDetalies);
                }
                else
                {
                    TempData["Error"] = "Nie zaktualizowane pofilu!";

                }



            return View(userDetalies);
        }

        public IActionResult Home()
        {
            return RedirectToAction("Index", "Home");

        }
        public IActionResult Aktywność()
        {
            return RedirectToAction("Index", "Aktywność");

        }
        public IActionResult Wpisy()
        {
            return RedirectToAction("Index", "Wpisy");
        }
       

        public IActionResult Ustawienia()
        {
            return RedirectToAction("Index", "Konto");
        }
        public IActionResult About()
        {
            return RedirectToAction("About", "Home");
        }
        public async Task<IActionResult> Wyloguj()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
