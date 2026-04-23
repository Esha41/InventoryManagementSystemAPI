using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.DTO.AuthenticationDto
{
    public record LoginRequest(string Email, string Password);
}
