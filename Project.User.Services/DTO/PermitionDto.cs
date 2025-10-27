using Ettad.CrossCutting.Comman.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.DTO
{
    internal class PermitionDto
    {
        public bool IsForReportDesinger { get; set; }
        public string Category { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public List<CheckBox> PermissionsList { get; set; } = new List<CheckBox>();
    }
}
