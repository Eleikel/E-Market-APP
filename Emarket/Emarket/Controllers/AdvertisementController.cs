using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Advertisements;
using Emarket.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Emarket.Controllers
{
    public class AdvertisementController : Controller
    {

        private readonly IAdvertisementService _advertisementService;
        private readonly ICategoryService _categoryService;
        private readonly ValidateUserSession _validateUserSession;


        public AdvertisementController(IAdvertisementService advertisementService, ICategoryService categoryService, ValidateUserSession validateUserSession)
        {
            _advertisementService = advertisementService;
            _categoryService = categoryService;
            _validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            return View(await _advertisementService.GetAllViewModel());
        }

        //Create
        public async Task<IActionResult> Create()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            SaveAdvertisementViewModel vm = new();
            vm.Categories = await _categoryService.GetAllViewModel();
            return View("SaveAdvertisement", vm);

        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveAdvertisementViewModel vm)
        {

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                vm.Categories = await _categoryService.GetAllViewModel();
                return View("SaveAdvertisement", vm);
            }


            SaveAdvertisementViewModel advertisementVm = await _advertisementService.Add(vm);

            if (advertisementVm.Id != 0 && advertisementVm != null)
            {
                advertisementVm.ImageUrl = UploadFile(vm.File, advertisementVm.Id);

                await _advertisementService.Update(advertisementVm);
            }

            return RedirectToRoute(new { controller = "Advertisement", action = "Index" });
        }

        //Edit
        public async Task<IActionResult> Edit(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            SaveAdvertisementViewModel vm = await _advertisementService.GetByIdSaveViewModel(id);
            vm.Categories = await _categoryService.GetAllViewModel();
            return View("SaveAdvertisement", vm);

        }


        [HttpPost]
        public async Task<IActionResult> Edit(SaveAdvertisementViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                vm.Categories = await _categoryService.GetAllViewModel();
                return View("SaveAdvertisement", vm);
            }

            SaveAdvertisementViewModel productVm = await _advertisementService.GetByIdSaveViewModel(vm.Id);
            vm.ImageUrl = UploadFile(vm.File, vm.Id, true, productVm.ImageUrl);
            await _advertisementService.Update(vm);


            return RedirectToRoute(new { controller = "Advertisement", action = "Index" });
        }


        //Delete

        public async Task<IActionResult> Delete(int id)
        {

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }


            return View(await _advertisementService.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            await _advertisementService.Delete(id);


            //get directory path
            string basePath = $"/Images/Advertisements/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");
            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                //Delete every file that exists in this Directory
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }

                //Delete every folder and file that exists in this Directory
                foreach (DirectoryInfo folder in directoryInfo.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }


            return RedirectToRoute(new { controller = "Advertisement", action = "Index" });
        }



        //Upload File
        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string imageUrl = "")
        {
            if (isEditMode && file == null)
            {
                return imageUrl;
            }

            //get directory path
            string basePath = $"/Images/Advertisements/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //Create folder if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //get file path
            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string filenameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(filenameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }


            //Borrar la Imagen Antigua cuando editamos
            if (isEditMode)
            {
                string[] oldImagePart = imageUrl.Split("/");
                string oldImageName = oldImagePart[^1];   // '^1' = Ultima posicion
                string completeImageOldPath = Path.Combine(path, oldImageName);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }

            return $"{basePath}/{fileName}";
        }


    }
}
