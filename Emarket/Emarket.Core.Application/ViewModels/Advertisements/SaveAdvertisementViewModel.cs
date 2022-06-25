using Emarket.Core.Application.ViewModels.Categories;
using Emarket.Core.Application.ViewModels.Users;
using Emarket.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.ViewModels.Advertisements
{
    public class SaveAdvertisementViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del Anuncio")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe ingresar la descripcion del Anuncio")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [DataType(DataType.Text)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Debe ingresar el precio del Anuncio")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Debe ingresar la categoria del Anuncio")]
        public int CategoryId { get; set; }

        public List<CategoryViewModel> Categories { get; set; }

        //
        public Category Category { get; set; }
        public User User { get; set; }
//

        public List<UserViewModel> Users { get; set; }



        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }
        public int UserId { get; set; }


        public string CategoryName { get; set;}
        public DateTime Created { get; set; }   
        public string PublishedBy { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
