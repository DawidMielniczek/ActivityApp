using ApkaTa.Models;
using ApkaTa.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.X509Certificates;
using System.Security.Claims;

namespace ApkaTa.Controllers
{
    [Authorize]

    public class AktywnośćController : Controller
    {
        readonly IAktywnośćRepository aktywnoscRepository = new AktywnośćRepository();
        readonly IUserRepository userRepository = new UserRepository();
        public UserViewModel _User { get; set; } = new();

        public List<AktywnośćUserModel> Zaaplanowane { get; } = new();
        public List<AktywnośćUserModel> ZaplanowanaSearch { get; } = new();
        public List<AktywnośćUserModel> Dostepna { get; } = new();
        public List<AktywnośćUserModel> DostepnaSearch { get; } = new();
        public List<AktywnośćUsr> LastAktywność { get; } = new();

        public List<SelectListItem> AktywnośćP { get; } = new();
        public async void GetInfo()
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await userRepository.GetUserId(ViewBag.IdU);
            _User = user;
            ViewBag.Nick = _User.Nick;

        }
        public async Task<IActionResult> Index()
        {
            
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
           
            var user = await userRepository.GetUserId(ViewBag.IdU);
            _User = user;
            return View(_User);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DodajAktywność(string NazwaAktywnosc, AktywnośćUser aktywność)
        {
            
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
            GetInfo();
            if (ModelState.IsValid)
            {

                if (aktywność.GodzinaOd > aktywność.GodzinaDo)
                {
                    TempData["Error"] = "Czas trwania wydarenia musi trwać min. 30 minut!";
                    if (ViewBag.NazwaAktywnosc is null)
                    {
                        var items = await aktywnoscRepository.GetAktywności();
                        ViewBag.NazwaAktywnosc = new List<SelectListItem>();

                        foreach (var it in items)
                        {
                            ViewBag.NazwaAktywnosc.Add(new SelectListItem { Text = it.Nazwa, Value = it.IdAktywności.ToString() });
                        }
                    }
                    return View(aktywność);
                }

                var TimeResult = aktywność.GodzinaDo - aktywność.GodzinaOd;
                TimeSpan time = new TimeSpan(0, 30, 0);
                if (TimeResult > time)
                {
                    var active = new AktywnośćUser()
                    {
                        IdAktywności = Convert.ToInt16(NazwaAktywnosc),
                        GodzinaOd = aktywność.GodzinaOd,
                        GodzinaDo = aktywność.GodzinaDo,
                        Data = aktywność.Data,
                        MiejsceDoc = aktywność.MiejsceDoc,
                        IlośćMiejsc = aktywność.IlośćMiejsc,
                        Opis = aktywność.Opis

                    };
                    if (await aktywnoscRepository.AddAktywnośćUser(active))
                    {
                        var result = await aktywnoscRepository.GetLastAktywność();
                        foreach (var pik in result)
                        {
                            LastAktywność.Add(pik);
                        }
                        LastAktywność[0].IdU = ViewBag.IdU;
                        if (await aktywnoscRepository.AddAktywnośćUsr(LastAktywność[0]))
                        {
                            TempData["succes"] = "Wydarzenie zostało dodane!";
                            return RedirectToAction("NadchodzaceWyd");
                        }
                        else
                        {
                            TempData["error"] = "Coś poszło nie tak.. Serwer jest niosiągalny!";
                            return RedirectToAction("NadchodzaceWyd");
                        }

                    }
                }

            }
            TempData["error"] = "Wypełnij wszystkie pola formularza!";

            if (ViewBag.NazwaAktywnosc is null)
            {
                var items = await aktywnoscRepository.GetAktywności();
                ViewBag.NazwaAktywnosc = new List<SelectListItem>();

                foreach (var it in items)
                {
                    ViewBag.NazwaAktywnosc.Add(new SelectListItem { Text = it.Nazwa, Value = it.IdAktywności.ToString() });
                }
            }
            return View(aktywność);
        }


        [HttpGet]
        public async Task<IActionResult> DodajAktywność()
        {
            GetInfo();

            if (ViewBag.NazwaAktywnosc is null)
            {
                var items = await aktywnoscRepository.GetAktywności();
                ViewBag.NazwaAktywnosc = new List<SelectListItem>();

                foreach (var it in items)
                {
                    ViewBag.NazwaAktywnosc.Add(new SelectListItem { Text = it.Nazwa, Value = it.IdAktywności.ToString() });
                }
            }
            ViewBag.DataMin = DateTime.Now.Date.ToString("yyyy-MM-dd");
            ViewBag.DateMax = DateTime.Now.Date.AddDays(13).ToString("yyyy-MM-dd");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DostepneAkty(string NazwaAktywnosc)
        {
            ViewBag.IdU=Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));

            GetInfo();


            if (ViewBag.NazwaAktywnosc is null)
            {
                var items = await aktywnoscRepository.GetAktywności();
                ViewBag.NazwaAktywnosc = new List<SelectListItem>();

                foreach (var it in items)
                {
                    ViewBag.NazwaAktywnosc.Add(new SelectListItem { Text = it.Nazwa, Value = it.Nazwa });
                }
            }


            var DosAkty = await aktywnoscRepository.GetDostępneAktywności(ViewBag.IdU);
            foreach (var dostepna in DosAkty)
            {
                Dostepna.Add(dostepna);
            }


            if (Dostepna.Count == 0)
            {
                return View();

            }
            else
            {
                if (string.IsNullOrEmpty(NazwaAktywnosc))
                {
                    TempData["MessageWpisy"] = "Wybierz kategorię aktywności!";
                    return View(Dostepna);
                }
                else
                {
                    if (DostepnaSearch != null) DostepnaSearch.Clear();
                    var result = Dostepna.Where(x => x.Nazwa == NazwaAktywnosc).ToList();
                    foreach (var item in result)
                    {
                        DostepnaSearch.Add(item);
                    }
                    return View(DostepnaSearch);

                }
            }
            return View(Dostepna);
        }

        [HttpGet]
        public async Task<IActionResult> NadchodzaceWyd(string NazwaAktywnosc)
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
            GetInfo();
            if (ViewBag.NazwaAktywnosc is null)
            {
                var items = await aktywnoscRepository.GetAktywności();
                ViewBag.NazwaAktywnosc = new List<SelectListItem>();

                foreach (var it in items)
                {
                    ViewBag.NazwaAktywnosc.Add(new SelectListItem { Text = it.Nazwa, Value = it.Nazwa });
                }
            }


            var NadAkty = await aktywnoscRepository.GetNadchodzącaAktywności(ViewBag.IdU);
            foreach (var Nadch in NadAkty)
            {
                Zaaplanowane.Add(Nadch);
            }
          
            if (Zaaplanowane.Count == 0)
            {
                return View();

            }
            else
            {
                if (string.IsNullOrEmpty(NazwaAktywnosc))
                {
                    TempData["MessageWpisy"] = "Wybierz kategorię aktywności!";
                    return View(Zaaplanowane);
                }
                else
                {
                    if (ZaplanowanaSearch != null) ZaplanowanaSearch.Clear();
                    var result = Zaaplanowane.Where(x => x.Nazwa == NazwaAktywnosc).ToList();
                    foreach (var item in result)
                    {
                        ZaplanowanaSearch.Add(item);
                    }
                    return View(ZaplanowanaSearch);

                }
            }
        }


        [HttpGet]
        public async Task<IActionResult> DostepnaUserDetalies(int id)
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
            GetInfo();
            var Nadchodzaca = await aktywnoscRepository.GetDostępneAktywności(ViewBag.IdU);
            foreach (var aktyNad in Nadchodzaca)
            {
                Dostepna.Add(aktyNad);
            }
            var detaliesPost = Dostepna.Where(x => x.AktywnośćId == id).ToList();
            AktywnośćUserModel detalise = detaliesPost[0];
            return View(detalise);
        }


        [HttpPost]
        public async Task<IActionResult> Dolacz(int id, AktywnośćUserModel akty)
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (akty.IlośćMiejsc != akty.Count)
            {
                var aktywnosc = new AktywnośćUsr();
                aktywnosc.AktywnośćId = id;
                aktywnosc.IdU = ViewBag.IdU;
                if (await aktywnoscRepository.AddAktywnośćUsr(aktywnosc))
                {
                    TempData["succes"] = "Dołączyłeś do wydarzenia!";

                }
                else
                {
                    TempData["error"] = "Coś poszło nie tak!";

                }
            }
            else
            {
                TempData["error"] = "Coś poszło nie tak!";

            }
            return RedirectToAction("NadchodzaceWyd");
        }


        [HttpPost]
        public async Task<IActionResult> Odlacz(int id, AktywnośćUserModel akty)
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (await aktywnoscRepository.DeleteAktywnośćUsr(ViewBag.IdU, id))
            {
                if (akty.Count == 1)
                {
                   await aktywnoscRepository.DeleteAktywnośćUser(id);
                }
                TempData["succes"] = "Odłączyłeś z wydarzenia!";
            }
            else
            {
                TempData["error"] = "Coś poszło nie tak...Spróbuj ponownie później.";
            }
            return RedirectToAction("DostepneAkty");

        }

        [HttpGet]
        public async Task<IActionResult> NadchUserDetalies(int id)
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
            GetInfo();
            var Nadchodzaca = await aktywnoscRepository.GetNadchodzącaAktywności(ViewBag.IdU);
            foreach (var aktyNad in Nadchodzaca)
            {
                Zaaplanowane.Add(aktyNad);
            }
            var detaliesPost = Zaaplanowane.Where(x => x.AktywnośćId == id).ToList();
            AktywnośćUserModel detalise = detaliesPost[0];
            return View(detalise);
        }

        [HttpGet]
        public async Task<IActionResult> HistoriaWydarzeń(string NazwaAktywnosc)
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));

            GetInfo();


            if (ViewBag.NazwaAktywnosc is null)
            {
                var items = await aktywnoscRepository.GetAktywności();
                ViewBag.NazwaAktywnosc = new List<SelectListItem>();

                foreach (var it in items)
                {
                    ViewBag.NazwaAktywnosc.Add(new SelectListItem { Text = it.Nazwa, Value = it.Nazwa });
                }
            }


            var HisAkty = await aktywnoscRepository.GetHistoriaWyd(ViewBag.IdU);
            foreach (var historia in HisAkty)
            {
                Dostepna.Add(historia);
            }


            if (Dostepna.Count == 0)
            {
                return View();

            }
            else
            {
                if (string.IsNullOrEmpty(NazwaAktywnosc))
                {
                    TempData["MessageWpisy"] = "Wybierz kategorię aktywności!";
                    return View(Dostepna);
                }
                else
                {
                    if (DostepnaSearch != null) DostepnaSearch.Clear();
                    var result = Dostepna.Where(x => x.Nazwa == NazwaAktywnosc).ToList();
                    foreach (var item in result)
                    {
                        DostepnaSearch.Add(item);
                    }
                    return View(DostepnaSearch);

                }
            }

        }

        [HttpGet]
        public async Task<IActionResult> HistoriaUserDetalies(int id)
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
            GetInfo();
            var Nadchodzaca = await aktywnoscRepository.GetHistoriaWyd(ViewBag.IdU);
            foreach (var aktyNad in Nadchodzaca)
            {
                Dostepna.Add(aktyNad);
            }
            var detaliesPost = Dostepna.Where(x => x.AktywnośćId == id).ToList();
            if (detaliesPost.Any())
            {
                AktywnośćUserModel detalise = detaliesPost[0];
                return View(detalise);
            }
            else
            {
                TempData["error"] = "Coś poszło nie tak!";
                return View();
            }

        }

        [HttpGet]
        public async Task<IActionResult> LastHistoriaUserDetalies()
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
            GetInfo();
            var LastActive = await aktywnoscRepository.GetLastHistoryAktywność(ViewBag.IdU);
            foreach (var last in LastActive)
            {
                Dostepna.Add(last);
            }
            if (Dostepna.Count != 0)
            {
                AktywnośćUserModel detalise = LastActive[0];
                return View(detalise);
            }
            else
            {
                return View();
            }
        }

        public IActionResult Home()
        {
            return RedirectToAction("Index", "Home");

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
