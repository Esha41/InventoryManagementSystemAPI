using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Data.Entities.Settings
{
    public class EmailConfiguration 
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
      //  public Organization Organization { get; set; }
        public int OrganizationId { get; set; }
    }
}
