using Emarket.Core.Application.Interfaces;
using Emarket.Core.Application.Interfaces.Repositories;
using Emarket.Infrastructure.Persistence.Contexts;
using Emarket.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Infrastructure.Persistence
{

    //Patron Decorator
    public static class ServiceRegistration
    {

        public static void AddPersistenceInfraestructure(this IServiceCollection services, IConfiguration configuration) 
        {

            #region Context
            services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            #endregion



            #region Repositories
            //Configuracion de las Dependency Injections
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAdvertisementRepository, AdvertisementRepository>();


            #endregion


        }
    }
}
