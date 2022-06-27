using Emarket.Core.Application.Helpers;
using Emarket.Core.Application.Interfaces.Repositories;
using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.ViewModels.Advertisements;
using Emarket.Core.Application.ViewModels.Categories;
using Emarket.Core.Application.ViewModels.Users;
using Emarket.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;

        public AdvertisementService(IAdvertisementRepository advertisementRepository, IHttpContextAccessor httpContextAccessor, ICategoryRepository categoryRepository)
        {
            _advertisementRepository = advertisementRepository;
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user"); //Get the current User of the session
        }


        public async Task Update(SaveAdvertisementViewModel vm)
        {
            Advertisement advertisement = await _advertisementRepository.GetByIdAsync(vm.Id);
            advertisement.Id = vm.Id;
            advertisement.Name = vm.Name;
            advertisement.Description = vm.Description;
            advertisement.ImageUrl = vm.ImageUrl;
            advertisement.ImageUrl2 = vm.ImageUrl2;
            advertisement.ImageUrl3 = vm.ImageUrl3;
            advertisement.ImageUrl4 = vm.ImageUrl4;
            advertisement.Price = vm.Price;
            advertisement.CategoryId = vm.CategoryId;
            advertisement.UserId = vm.UserId;

            await _advertisementRepository.UpdateAsync(advertisement);
        }

        public async Task<SaveAdvertisementViewModel> Add(SaveAdvertisementViewModel vm)
        {
            Advertisement advertisement = new();
            advertisement.Id = vm.Id;
            advertisement.Name = vm.Name;
            advertisement.Price = vm.Price;
            advertisement.ImageUrl = vm.ImageUrl;
            advertisement.ImageUrl2 = vm.ImageUrl2;
            advertisement.ImageUrl3 = vm.ImageUrl3;
            advertisement.ImageUrl4 = vm.ImageUrl4;
            advertisement.Description = vm.Description;
            advertisement.CategoryId = vm.CategoryId;
            advertisement.UserId = userViewModel.Id;

            advertisement = await _advertisementRepository.AddAsync(advertisement);

            SaveAdvertisementViewModel advertisementVm = new();

            advertisementVm.Id = advertisement.Id;
            advertisementVm.Name = advertisement.Name;
            advertisementVm.Price = advertisement.Price;
            advertisementVm.ImageUrl = advertisement.ImageUrl;
            advertisementVm.ImageUrl2 = advertisement.ImageUrl2;
            advertisementVm.ImageUrl3 = advertisement.ImageUrl3;
            advertisementVm.ImageUrl4 = advertisement.ImageUrl4;
            advertisementVm.Description = advertisement.Description;
            advertisementVm.CategoryId = advertisement.CategoryId;
            advertisementVm.UserId = advertisement.UserId;


            return advertisementVm;       
        }

        public async Task Delete(int id)
        {
            var advertisement = await _advertisementRepository.GetByIdAsync(id);
            await _advertisementRepository.DeleteAsync(advertisement);
        }

        public async Task<SaveAdvertisementViewModel> GetByIdSaveViewModel(int id)
        {
            var advertisement = await _advertisementRepository.GetByIdAsync(id);

            SaveAdvertisementViewModel vm = new();
            vm.Id = advertisement.Id;
            vm.Name = advertisement.Name;
            vm.Price = advertisement.Price;
            vm.ImageUrl = advertisement.ImageUrl;
            vm.ImageUrl2 = advertisement.ImageUrl2;
            vm.ImageUrl3 = advertisement.ImageUrl3;
            vm.ImageUrl4 = advertisement.ImageUrl4;
            vm.Description = advertisement.Description;
            vm.CategoryId = advertisement.CategoryId;
            vm.Created = advertisement.Created;
            vm.UserId = advertisement.UserId;
            vm.Category = advertisement.Category; //Acceder al name
            vm.User = advertisement.User;

            return vm;
        }

        public async Task<List<AdvertisementViewModel>> GetAllViewModel()
        {
            var advertisementList = await _advertisementRepository.GetAllWithIncludeAsync(new List<string> { "Category" });

            return advertisementList.Where(advertisement => advertisement.UserId == userViewModel.Id).Select(advertisement => new AdvertisementViewModel
            {
                Id = advertisement.Id,
                Name = advertisement.Name,
                Description = advertisement.Description,
                ImageUrl = advertisement.ImageUrl,
                Price = advertisement.Price,
                CategoryId = advertisement.CategoryId,
                CategoryName = advertisement.Category.Name,
               
            }).ToList();
        }

        //GET ALL With Filter
        public async Task<List<AdvertisementViewModel>> GetAllViewModelWithFilter(FilterAdvertisementViewModel filters)
        {
            var advertisementList = await _advertisementRepository.GetAllAsync();


            //This us
            var listViewModels = advertisementList.Where(advertisement => advertisement.UserId != userViewModel.Id).Select(advertisement => new AdvertisementViewModel
            {
                Id = advertisement.Id,
                Name = advertisement.Name,
                Description = advertisement.Description,
                ImageUrl = advertisement.ImageUrl,
                Price = advertisement.Price,
                CategoryId = advertisement.CategoryId,
                CategoryName = advertisement.Category.Name,


            }).ToList();

            if (filters.CategoryId != null)
            {
                listViewModels = listViewModels.Where(category => category.CategoryId == filters.CategoryId.Value).ToList();
            }

            else if (filters.Id != null)
            {
                listViewModels = listViewModels.Where(advertisement => advertisement.Id == filters.Id.Value).ToList();
            }

            return listViewModels;

        }
    }
}
