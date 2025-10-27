using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Ettad.Application.Common.Models;
using Ettad.Comman.Idenitity;
using Ettad.EntityFramework.DataBaseContext;
using System.Security.Claims;

namespace Ettad.Services.Helpers
{
    public class HelpureService : IHelpureService
    {
        #region fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        #endregion

        #region ctor
        public HelpureService(UserManager<ApplicationUser> userManager ,ApplicationDbContext applicationDbContext )
        {
            _userManager = userManager;
            _context = applicationDbContext;
        }
        #endregion

        #region GetUser
        public async Task<string> GetUserAsync(ClaimsPrincipal user)
        {
            var userData = await _userManager.GetUserAsync(user);

            if (userData == null)
                return string.Empty;

            return userData.Id;
        }
        #endregion

        #region GetEmailConfigration
        public async Task<EmailConfiguration?> GetEmailConfigrationAsync(int organizationId)
        {
            var emailEntity = _context.EmailConfigurations
                .FirstOrDefault(a => a.OrganizationId == organizationId && a.Key == "EmailConfiguration");

            if (emailEntity == null || string.IsNullOrEmpty(emailEntity.Value))
                return null;

            return JsonConvert.DeserializeObject<EmailConfiguration>(emailEntity.Value);
        }

        #endregion
    }
}
