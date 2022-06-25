using Emarket.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Domain.Entities
{
    public class User : AuditableBaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICollection<Advertisement> Advertisements { get; set; }

        //public int CategoryIds { get; set; }
        ////Navegation Property
        //public Category Category { get; set; }
    }
}
