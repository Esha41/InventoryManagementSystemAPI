using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Services.DataTransferObject.Customer
{
    public class CustomerCreateModel
    {

        public string Phone { get; set; }

        public string? MappedId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } // Name of the customer
        public string Email { get; set; } // Email of the customer
    }

}
