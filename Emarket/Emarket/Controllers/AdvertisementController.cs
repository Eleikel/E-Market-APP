using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Advertisements;
using Emarket.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
                advertisementVm.ImageUrl = UploadFile(vm.File[0], advertisementVm.Id);


                var length = vm.File.Count;

                if (length >= 2 && vm.File[1] != null)
                {
                    advertisementVm.ImageUrl2 = UploadFile(vm.File[1], advertisementVm.Id);
                }

                if (length >= 3 && vm.File[2] != null)
                {
                    advertisementVm.ImageUrl3 = UploadFile(vm.File[2], advertisementVm.Id);
                }

                if (length >= 4 && vm.File[3] != null)
                {
                    advertisementVm.ImageUrl4 = UploadFile(vm.File[3], advertisementVm.Id);
                }

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

            SaveAdvertisementViewModel advertisementVm = await _advertisementService.GetByIdSaveViewModel(vm.Id);

            var length = vm.File != null ? vm.File.Count : 0;

            List<IFormFile> Images = new List<IFormFile>(4);

            int LimitFilesNumber = 4; //How many Files i want to upload

            for (int i = 0; i < LimitFilesNumber; i++)
            {
                if (length >= i + 1 && vm.File[i] != null)
                {
                    Images.Add(vm.File[i]);
                }
                else
                {
                    Images.Add(null);
                }
            }

            vm.ImageUrl = UploadFile(Images[0], vm.Id, true, advertisementVm.ImageUrl);
            vm.ImageUrl2 = UploadFile(Images[1], vm.Id, true, advertisementVm.ImageUrl2);
            vm.ImageUrl3 = UploadFile(Images[2], vm.Id, true, advertisementVm.ImageUrl3);
            vm.ImageUrl4 = UploadFile(Images[3], vm.Id, true, advertisementVm.ImageUrl4);

            vm.UserId = advertisementVm.UserId;

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
            if (isEditMode && imageUrl != "" && imageUrl != null)
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
