using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.ViewModels.Categories
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        // Quantity of Advertisements by category
        public int AdvertisementsQuantity { get; set; }
        public int UsersQuantity { get; set; }

        
    }
}
