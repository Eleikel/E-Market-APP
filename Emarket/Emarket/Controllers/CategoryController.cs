using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Categories;
using Emarket.Middleware;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Emarket.Controllers
{
    public class CategoryController : Controller
    {
        public readonly ICategoryService _categoryService;
        public readonly ValidateUserSession _validateUserSession;

        public CategoryController(ICategoryService categoryService, ValidateUserSession validateUserSession)
        {
            _categoryService = categoryService;
            _validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            return View(await _categoryService.GetAllViewModel());
        }

        //Create
        public IActionResult Create()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }
            return View("SaveCategory", new SaveCategoryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCategoryViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View("SaveCategory", vm);
            }

            if (await _categoryService.Exist(vm.Name))
            {
                ModelState.AddModelError("", $"La categoria {vm.Name} ya existe.");
                return View("SaveCategory", vm);
            }


            await _categoryService.Add(vm);
            return RedirectToRoute(new { controller = "Category", action = "Index" });
        }

        //Edit
        public async Task<IActionResult> Edit(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }
            return View("SaveCategory", await _categoryService.GetByIdSaveViewModel(id));
        }


        [HttpPost]
        public async Task<IActionResult> Edit(SaveCategoryViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View("SaveCategory", vm);
            }

            if (await _categoryService.Exist(vm.Name))
            {
                ModelState.AddModelError("", $"La categoria {vm.Name} ya existe.");
                return View("SaveCategory", vm);
            }

            await _categoryService.Update(vm);
            return RedirectToRoute(new { controller = "Category", action = "Index" });
        }


        //Delete
        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            return View(await _categoryService.GetByIdSaveViewModel(id));
        }

        public async Task<IActionResult> DeletePost(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            await _categoryService.Delete(id);
            return RedirectToRoute(new { controller = "Category", action = "Index" });
        }

    }
}
