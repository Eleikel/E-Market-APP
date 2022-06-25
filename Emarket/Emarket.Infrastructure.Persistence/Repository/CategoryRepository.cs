using Emarket.Core.Application.Interfaces.Repositories;
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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationContext _dbContext;

        public CategoryRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<bool> ExistAsync(string category)
        {
            return await _dbContext.Set<Category>().AnyAsync(c => c.Name.ToLower().Trim() == category.ToLower().Trim());
        }
    }

    
}
