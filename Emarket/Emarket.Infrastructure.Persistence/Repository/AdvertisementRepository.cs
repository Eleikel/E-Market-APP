using Emarket.Core.Application.Interfaces.Repositories;
using Emarket.Core.Domain.Entities;
using Emarket.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Infrastructure.Persistence.Repository
{
    public class AdvertisementRepository : GenericRepository<Advertisement>, IAdvertisementRepository
    {

        private readonly ApplicationContext _dbContext;

        public AdvertisementRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
