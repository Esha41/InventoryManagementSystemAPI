using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.DTO
{
    public record LoginInformation(string Username, string Password, bool IsLdap, string? CaptchaId = null, string? CaptchaCode = null, bool ForceLogin = false);
}
