using ApkaTa.Models;
using ApkaTa.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Security.Claims;

namespace ApkaTa.Controllers
{
    [Authorize]
    public class WpisyController : Controller
    {
        readonly IPostRepository postRepository = new PostRepositroy();
        readonly IAktywnośćRepository aktywnoscRepository = new AktywnośćRepository();
        readonly IUserRepository userRepository = new UserRepository();
        public UserViewModel _User { get; set; } = new();

        public List<PostUser> Posty { get; } = new();
        public List<PostUser> YourPosty { get; } = new();
        public List<PostUser> PostySearch { get; } = new();
        public List<PostUser> TwojePostySearch { get; } = new();
        public List<Aktywność> Aktywość { get; } = new();

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

        public async Task<IActionResult> DodajPost(string NazwaAktywnosc,PostUsers post)
        {
            GetInfo();
            if (ModelState.IsValid)
            {

                ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var AddPost = new PostUsers();
                AddPost.DataWpisu = DateTime.Now.Date;
                AddPost.IdU = ViewBag.IdU;
                AddPost.IdAktywności = Convert.ToInt32(NazwaAktywnosc);
                AddPost.Opis = post.Opis;
                AddPost.Temat = post.Temat;

                if (await postRepository.AddPostUser(AddPost))
                {
                    TempData["succes"] = "Post został dodany!";
                    return RedirectToAction("TwojeWpisy");
                }
                else
                {
                    TempData["error"] = "Serwer tymczasowo niedostępny!";
                    return RedirectToAction("TwojeWpisy");

                }
            }
            if (ViewBag.NazwaAktywnosc is null)
            {
                var items = await aktywnoscRepository.GetAktywności();
                ViewBag.NazwaAktywnosc = new List<SelectListItem>();

                foreach (var it in items)
                {
                    ViewBag.NazwaAktywnosc.Add(new SelectListItem { Text = it.Nazwa, Value = it.IdAktywności.ToString() });
                }
            }
            TempData["error"] = "Wprowadź poprawnie wszystkie pole formularza!";

            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> DodajPost()
        {
            GetInfo();
            if (ViewBag.NazwaAktywnosc is null)
            {
                var items = await aktywnoscRepository.GetAktywności();
                ViewBag.NazwaAktywnosc = new List<SelectListItem>();

                foreach (var it in items)
                {
                    ViewBag.NazwaAktywnosc.Add(new SelectListItem { Text = it.Nazwa, Value =  it.IdAktywności.ToString() });
                }
            }
           
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> WpisyUsers(string NazwaAktywnosc)
        {
            GetInfo();

            var items = await aktywnoscRepository.GetAktywności();

            ViewBag.NazwaAktywnosc = new List<SelectListItem>();

            foreach (var it in items)
            {
                ViewBag.NazwaAktywnosc.Add(new SelectListItem { Text = it.Nazwa, Value = it.Nazwa });
            }
            var posty = await postRepository.GetAllPost();
            foreach (var post in posty)
            {
                Posty.Add(post);
            }


            if (Posty.Count == 0)
            {
                return View();

            }
            else
            {
                if (string.IsNullOrEmpty(NazwaAktywnosc))
                {
                    TempData["MessageWpisy"] = "Wybierz kategorię aktywności!";
                    return View(Posty);
                }
                else
                {
                    if (PostySearch != null) PostySearch.Clear();  
                    var result = Posty.Where(x => x.Nazwa == NazwaAktywnosc).ToList();
                    foreach(var item in result)
                    {
                        PostySearch.Add(item);
                    }
                    return View(PostySearch);
                        
                }
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> TwojeWpisy(string NazwaAktywnosc)
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
            GetInfo();

            if (ViewBag.NazwaAktywnosc is null) {
                var items = await aktywnoscRepository.GetAktywności();
                ViewBag.NazwaAktywnosc = new List<SelectListItem>();

                foreach (var it in items)
                {
                    ViewBag.NazwaAktywnosc.Add(new SelectListItem { Text = it.Nazwa, Value = it.Nazwa });
                }
            }
             
            
            var posty = await postRepository.GetYourPost(ViewBag.IdU);
            foreach (var post in posty)
            {
                YourPosty.Add(post);
            }


            if (YourPosty.Count == 0)
            {
                return View();

            }
            else
            {
                if (string.IsNullOrEmpty(NazwaAktywnosc))
                {
                    TempData["MessageWpisy"] = "Wybierz kategorię aktywności!";
                    return View(YourPosty);
                }
                else
                {
                    if (PostySearch != null) PostySearch.Clear();
                    var result = YourPosty.Where(x => x.Nazwa == NazwaAktywnosc).ToList();
                    foreach (var item in result)
                    {
                        TwojePostySearch.Add(item);
                    }
                    return View(TwojePostySearch);

                }
            }
            return View(YourPosty);
        }

        [HttpGet]
        public async Task <IActionResult> WpisUserDetalies(int id){

            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
            GetInfo();

            var posty = await postRepository.GetAllPost();
            foreach (var post in posty)
            {
                Posty.Add(post);
            }
            var detaliesPost = Posty.Where(x => x.IdPost == id).ToList();

            //dodowanie wyświetleń
            PostUsers view = new PostUsers()
            {
                IdU = ViewBag.IdU,
                IdPost = id,
                Temat ="",
                IdAktywności=0,
                Wyświetlenia = 0,
                DataWpisu= DateTime.Now,
                Opis=""
            };
            await postRepository.UpdateViews(view);
            PostUser detalise = detaliesPost[0];
            return View(detalise);
        }

        [HttpGet]
        public async Task<IActionResult> WpisUserUpdate(int id)
        {
            GetInfo();

            if (ViewBag.NazwaAktywnosc is null)
            {
                ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var items = await aktywnoscRepository.GetAktywności();
                ViewBag.NazwaAktywnosc = new List<SelectListItem>();

                foreach (var it in items)
                {
                    ViewBag.NazwaAktywnosc.Add(new SelectListItem { Text = it.Nazwa, Value = it.IdAktywności.ToString() });
                }
            }
            var posty = await postRepository.GetYourPost(ViewBag.IdU);
            foreach (var post in posty)
            {
                Posty.Add(post);
            }
            var detaliesPost = Posty.Where(x => x.IdPost == id).ToList();
            PostUser detalise = detaliesPost[0];
            return View(detalise);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WpisUserUpdate(string NazwaAktywnosc, int id, PostUser posts)
        {
            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
            GetInfo();

            var posty = await postRepository.GetYourPost(ViewBag.IdU);
            foreach (var post in posty)
            {
                Posty.Add(post);
            }
            var detaliesPost = Posty.Where(x => x.IdPost == id).ToList();
            PostUser detalise = new PostUser();
            if (NazwaAktywnosc is null)
                detalise = detaliesPost[0];
            else detalise.IdAktywności = Convert.ToInt16(NazwaAktywnosc);

            ModelState["NazwaAktywnosc"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

            if (ModelState.IsValid)
            {
 
                PostUsers update = new PostUsers()
                {
                    IdAktywności = detalise.IdAktywności,
                    Opis = posts.Opis,
                    Temat = posts.Temat,
                    IdU = ViewBag.IdU,
                    IdPost = id,
                    Wyświetlenia = posts.Wyświetlenia,
                    DataWpisu = posts.DataWpisu
                };
                if (await postRepository.UpdatePost(update))
                {
                    TempData["succes"] = "Post został Edytowany!";
                    return RedirectToAction("TwojeWpisy");

                }
                else
                {
                    TempData["error"] = "Coś poszło nie tak..Nieosiągalny serwer!";
                    return RedirectToAction("TwojeWpisy");
                }
            }
            TempData["error"] = "Błąd! Wprowadź poprawnie dane!";

            if (ViewBag.NazwaAktywnosc is null)
            {
                ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var items = await aktywnoscRepository.GetAktywności();
                ViewBag.NazwaAktywnosc = new List<SelectListItem>();

                foreach (var it in items)
                {
                    ViewBag.NazwaAktywnosc.Add(new SelectListItem { Text = it.Nazwa, Value = it.IdAktywności.ToString() });
                }
            }
          
            return View(detalise);

        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            ViewBag.IdU = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
            GetInfo();

            if (await postRepository.DeletePost(ViewBag.IdU,id))
            {
                TempData["succes"] = "Post został Usunięty!";
            }
            else
            {
                TempData["error"] = "Coś poszło nie tak!";
            }
            return RedirectToAction("TwojeWpisy");
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
