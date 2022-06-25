using Emarket.Core.Application.Services;
using Emarket.Core.Application.ViewModels.Advertisements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.Interfaces.Services
{
    public interface IAdvertisementService : IGenericService<SaveAdvertisementViewModel, AdvertisementViewModel>
    {
        Task<List<AdvertisementViewModel>> GetAllViewModelWithFilter(FilterAdvertisementViewModel filters);
    }
}
