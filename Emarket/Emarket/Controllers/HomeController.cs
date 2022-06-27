using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Advertisements;
using Emarket.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Emarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdvertisementService _advertisementService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        private readonly ILogger<HomeController> _logger;
        public readonly ValidateUserSession _validateUserSession;


        public HomeController(ILogger<HomeController> logger, ValidateUserSession validateUserSession, IAdvertisementService advertisementService, ICategoryService categoryService, IUserService userService)
        {
            _logger = logger;
            _validateUserSession = validateUserSession;
            _advertisementService = advertisementService;
            _categoryService = categoryService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(FilterAdvertisementViewModel vm, string sortOrder, string searchString)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            ViewBag.Categories = await _categoryService.GetAllViewModel();
            ViewBag.Names = await _advertisementService.GetAllViewModelWithFilter(vm);

            return View(await _advertisementService.GetAllViewModelWithFilter(vm));
        }


        //CONTINUAR ACA
        public async Task<IActionResult> Detail(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }


            ViewBag.Categories = await _categoryService.GetAllViewModel();

            return View("Detail", await _advertisementService.GetByIdSaveViewModel(id));
        }

    }
}
