using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.DTO
{
    public class JwtOptions
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenExpireInMinutes { get; set; } = 60 * 24;
        public int RefreshTokenExpireInMinutes { get; set; } = 60 * 24; // default 1 day

        /// <summary>Expiry for the token used only to call select-role after login.</summary>
        public int RoleSelectionTokenExpireInMinutes { get; set; } = 15;
    }
}
