using Emarket.Core.Application.Services;
using Emarket.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<SaveUserViewModel, UserViewModel>
    {
        Task<UserViewModel> Login(LoginViewModel loginVm);

        Task<bool> Exist(string category);


    }
}
