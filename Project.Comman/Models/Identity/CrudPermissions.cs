using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.CrossCutting.Comman.Models.Identity
{
    public class CrudPermissions
    {
        public bool IsForReportDesinger { get; set; }
        public string Category { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public List<CheckBox> PermissionsList { get; set; } = new List<CheckBox>();
    }
}
