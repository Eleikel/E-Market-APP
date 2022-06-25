using Emarket.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> ExistAsync(string category);

    }
}
