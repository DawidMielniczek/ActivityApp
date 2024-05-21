using ApkaTa.Models;
using ApkaTa.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace ApkaTa.Controllers
{
    public class RejestracjaController : Controller
    {

        public IUserRepository repo = new UserRepository();
        public IActionResult Loguj()
        {
            return RedirectToAction("Index", "Login");
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async  Task<IActionResult> Index(UserViewModel userViewModel)
        {
             List<UserViewModel> usr  = new List<UserViewModel>();
            ModelState["Płeć"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            ModelState["Bmi"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            ModelState["Waga"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            ModelState["Wzrost"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            
            
            if (ModelState.IsValid)
            {

                var temp = await repo.GetInfo();
                foreach (var i in temp)
                {
                    usr.Add(i);
                }
                var Temp = usr.Where(x => x.Email == userViewModel.Email || x.Nick == userViewModel.Nick).ToList();
                if (Temp.Count != 0)
                {
                    TempData["error"] = "Podane Email lub nick jest już zajęty!";

                    return View(userViewModel);
                }
                else if (userViewModel.Password != userViewModel.Bmi)
                {
                    TempData["error"] = "Podane hasła się różnią!";
                    return View(userViewModel);
                }
                else
                {
                    var user = new Users();
                    user.Email = userViewModel.Email;
                    user.Password = userViewModel.Password;
                    user.Nick = userViewModel.Nick;

                    var add = await repo.AddUserAccount(user);
                    if (add == false)
                    {
                        return View(userViewModel);
                    }
                    else
                    {
                        TempData["succes"] = "Konto zostało utworzone!";

                        return View();

                    }

                }
            }
            TempData["error"] = "Wprowadzone niepoprawne dane!!";
            return View(userViewModel);
        }
    }
}
