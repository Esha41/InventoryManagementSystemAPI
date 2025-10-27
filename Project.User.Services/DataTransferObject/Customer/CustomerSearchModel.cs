using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Services.DataTransferObject.Customer
{
    public class CustomerSearchModel
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

    }
}
