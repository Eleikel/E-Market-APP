using Emarket.Core.Application.Helpers;
using Emarket.Core.Application.Interfaces.Repositories;
using Emarket.Core.Application.Interfaces.Services;
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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAdvertisementRepository _advertisementRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;
        public CategoryService(ICategoryRepository categoryRepository, IUserRepository userRepository, IAdvertisementRepository advertisementRepository, IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _advertisementRepository = advertisementRepository;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user"); //Get the current User of the session
        }
        public async Task<bool> Exist(string category)
        {
            return await _categoryRepository.ExistAsync(category);
        }

        public async Task Update(SaveCategoryViewModel vm)
        {
            Category category = await _categoryRepository.GetByIdAsync(vm.Id);
            category.Id = vm.Id;
            category.Name = vm.Name;
            category.Description = vm.Description;

            await _categoryRepository.UpdateAsync(category);
        }

        public async Task<SaveCategoryViewModel> Add(SaveCategoryViewModel vm)
        {
            Category category = new();
            category.Name = vm.Name;
            category.Description = vm.Description;

            category = await _categoryRepository.AddAsync(category);

            SaveCategoryViewModel categoryVm = new();

            categoryVm.Id = category.Id;
            categoryVm.Name = category.Name;
            categoryVm.Description = category.Description;

            return categoryVm;
        }

        public async Task Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            await _categoryRepository.DeleteAsync(category);
        }

        public async Task<SaveCategoryViewModel> GetByIdSaveViewModel(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            SaveCategoryViewModel vm = new();
            vm.Id = category.Id;
            vm.Name = category.Name;
            vm.Description = category.Description;

            return vm;
        }

        public async Task<List<CategoryViewModel>> GetAllViewModel()
        {
            //Process to get the Quantity of distincts Users who are the Owners of the advertisement and using a certain category
            var categoryList = await _categoryRepository.GetAllWithIncludeAsync(new List<string> { "Advertisements" });
            var userList = await _userRepository.GetAllWithIncludeAsync(new List<string> { "Advertisements" });
            var advertisementList = await _advertisementRepository.GetAllWithIncludeAsync(new List<string> { "Category" });

            var IdCategories = categoryList.Select(cat => cat.Id).ToList();
            var CategoriesDictionary = new Dictionary<int, int>();

            foreach (var idCategory in IdCategories)
            {
                CategoriesDictionary.Add(idCategory, 0);
            }

            var IdUsers = userList.Select(user => user.Id).ToList();

            foreach (var IdUser in IdUsers)
            {

                List<int> CategoriesIdUsed = new();

                var CategoriesIdFromUser = advertisementList.Where(advertisement => advertisement.UserId == IdUser)
                                                        .Select(advertisement => advertisement.CategoryId).ToList();

                foreach (var item in CategoriesIdFromUser)
                {
                    var Found = IdCategories.Exists(a => a == item);

                    if (Found && !CategoriesIdUsed.Exists(a => a == item))
                    {
                        CategoriesIdUsed.Add(item);
                        CategoriesDictionary[item] = CategoriesDictionary[item] + 1;
                    }                  
                }
            }
            //


            return categoryList.Select(category => new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                AdvertisementsQuantity = category.Advertisements.Count(),
                UsersQuantity = CategoriesDictionary[category.Id]

            }).ToList();
        }
    }
}
