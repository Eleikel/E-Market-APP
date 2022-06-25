using Emarket.Core.Application.Helpers;
using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Users;
using Emarket.Middleware;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Emarket.Controllers
{
    public class UserController : Controller
    {
        public readonly IUserService _userService;
        public readonly ValidateUserSession _validateUserSession;

        public UserController(IUserService userService, ValidateUserSession validateUserSession)
        {
            _userService = userService;
            _validateUserSession = validateUserSession;
        }

        //Iniciar Session or Log In
        public IActionResult Index()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginVm)
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }

            UserViewModel userVm = await _userService.Login(loginVm);

            if (userVm != null)
            {
                HttpContext.Session.Set<UserViewModel>("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                ModelState.AddModelError("userValidation", "Acceso a los datos incorrecto");
            }

            return View(loginVm);
        }



        public IActionResult Register()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            return View(new SaveUserViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel userVm)
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            if (!ModelState.IsValid)
            {
                return View(userVm);
            }

            if (await _userService.Exist(userVm.Username))
            {
                ModelState.AddModelError("", $"El Username {userVm.Username} ya existe.");
                return View("Register", userVm);
            }


            await _userService.Add(userVm);
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }


        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

    }
}
