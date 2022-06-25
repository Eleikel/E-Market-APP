﻿using Emarket.Core.Application.Helpers;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;
        public CategoryService(ICategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
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
            //"Advertisement"
            var categoryList = await _categoryRepository.GetAllWithIncludeAsync(new List<string> { "Advertisements" });

            return categoryList.Select(category => new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                AdvertisementsQuantity = category.Advertisements.Count(),
                UsersQuantity = category.Advertisements.Where(advertisement => advertisement.Id == userViewModel.Id).Select(advertisement => new UserViewModel
                {
                    Id = userViewModel.Id
                }).Count()

                //AdvertisementsQuantity = = category.Users.Where(advertisement => advertisement.UserId == userViewModel.Id).Count()

            }).ToList();
        }
    }
}