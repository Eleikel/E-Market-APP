using Emarket.Core.Application.ViewModels.Users;
using Emarket.Core.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace Emarket.Middleware
{

    //Validate wether an user has logged in or not
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasUser()
        {
            UserViewModel userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");

            if (userViewModel == null) return false;

            return true;
        }
    }
}
