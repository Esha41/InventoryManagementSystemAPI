using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ettad.CrossCutting.Comman;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities.Settings
{
    public class Settings : AuditEntity<int>
    {
        public string? Key { get; set; }

        public string? Value { get; set; }

        public string? Group { get; set; }

    }
}
