using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.CrossCutting.Comman
{
    public interface ILookup
    {
        [MaxLength(500)]
        string Name { get; set; }
        [MaxLength(500)]
        string NameSE { get; set; }
        bool IsDeleted { get; set; }

    }
}
