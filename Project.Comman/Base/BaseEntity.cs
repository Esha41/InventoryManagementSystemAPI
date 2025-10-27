using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moujam.Casiher.Comman.Base
{
    public class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
