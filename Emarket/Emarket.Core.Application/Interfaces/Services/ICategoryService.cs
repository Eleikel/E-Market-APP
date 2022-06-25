using Emarket.Core.Application.Services;
using Emarket.Core.Application.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.Interfaces.Services
{
    public interface ICategoryService : IGenericService<SaveCategoryViewModel, CategoryViewModel>
    {
        Task<bool> Exist(string name);

    }
}
