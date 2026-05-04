using Ettad.Application.Common.Models;
using Ettad.Comman.Idenitity;
using Ettad.Data.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Ettad.Services.Helpers
{
    public class HelpureService : IHelpureService
    {
        #region fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICrossCuttingRepository<Ettad.Data.Entities.Settings.EmailConfiguration> _emailConfigurationRepository;
        #endregion

        #region ctor
        public HelpureService(
            UserManager<ApplicationUser> userManager,
            ICrossCuttingRepository<Ettad.Data.Entities.Settings.EmailConfiguration> emailConfigurationRepository)
        {
            _userManager = userManager;
            _emailConfigurationRepository = emailConfigurationRepository;
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
            var emailEntity = await _emailConfigurationRepository.FindOneAsync(
                a => a.OrganizationId == organizationId && a.Key == "EmailConfiguration");

            if (emailEntity == null || string.IsNullOrEmpty(emailEntity.Value))
                return null;

            return JsonConvert.DeserializeObject<EmailConfiguration>(emailEntity.Value);
        }

        #endregion
    }
}
