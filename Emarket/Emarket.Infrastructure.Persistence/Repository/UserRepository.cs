using Emarket.Core.Application.Helpers;
using Emarket.Core.Application.Interfaces.Repositories;
using Emarket.Core.Application.ViewModels.Users;
using Emarket.Core.Domain.Entities;
using Emarket.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Infrastructure.Persistence.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationContext _dbContext;

        public UserRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }

        public override async Task<User> AddAsync(User entity)
        {
            //Encryting password
            entity.Password = PasswordEncryption.ComputedSha256Hash(entity.Password);
            await base.AddAsync(entity); //Mantener la funcionalidad del padre (GenericRepository)
            return entity;
        }


        public async Task<User> LoginAsync(LoginViewModel loginVm)
        {
            string passwordEncrypt = PasswordEncryption.ComputedSha256Hash(loginVm.Password);

            User user = await _dbContext.Set<User>()
                .FirstOrDefaultAsync(user => user.Username == loginVm.Username && user.Password == passwordEncrypt);
            return user;
        }


        public async Task<bool> ExistAync(string usuario)
        {
            return await _dbContext.Set<User>().AnyAsync(c => c.Username.ToLower().Trim() == usuario.ToLower().Trim());
        }
    }


}
