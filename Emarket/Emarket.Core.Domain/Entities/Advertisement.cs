using Emarket.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Domain.Entities
{
    public class Advertisement : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public string ImageUrl4 { get; set; }

        public double Price { get; set; }
        
        public int CategoryId { get; set;}
        //Navegation Property
        public Category Category { get; set; }

        public int UserId { get; set; }
        //Navegation Property
        public User User { get; set; }


    }
}
