using Emarket.Core.Application.Interfaces.Services;
using Emarket.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Emarket.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            #region Services
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAdvertisementService, AdvertisementService>();


            #endregion
        }
    }
}
