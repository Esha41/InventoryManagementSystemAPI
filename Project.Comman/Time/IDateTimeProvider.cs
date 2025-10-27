using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.CrossCutting.Comman.Time
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
