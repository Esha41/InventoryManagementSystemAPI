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

        /// <summary>
        /// Inactivity window: each refresh slides the session's expiry forward by this much.
        /// No activity for this long (browser sleeping, tab idle) = refresh token expires = logged out on return.
        /// </summary>
        public int RefreshTokenExpireInMinutes { get; set; } = 15;

        /// <summary>
        /// Hard cap on total session length measured from login (SessionStartedAt).
        /// Refreshes can slide the inactivity window but never past this point,
        /// so a session cannot live forever by being refreshed periodically.
        /// </summary>
        public int AbsoluteSessionLifetimeMinutes { get; set; } = 60 * 24; // default 24 hours

        /// <summary>Expiry for the token used only to call select-role after login.</summary>
        public int RoleSelectionTokenExpireInMinutes { get; set; } = 15;
    }
}
