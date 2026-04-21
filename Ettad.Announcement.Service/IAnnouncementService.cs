using Ettad.Announcement.Service.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Announcement.Service
{
    public interface IAnnouncementService
    {
        Task<APIOperationResponse<List<AnnouncementDto>>> GetAllAnnouncementsAsync();
        Task<APIOperationResponse<AnnouncementDto>> GetAnnouncementByIdAsync(long id);
        Task<APIOperationResponse<List<ActiveAnnouncementDto>>> GetActiveAnnouncementsForUserAsync();
        Task<APIOperationResponse<AnnouncementDto>> CreateAnnouncementAsync(CreateAnnouncementDto dto);
        Task<APIOperationResponse<AnnouncementDto>> UpdateAnnouncementAsync(long id, UpdateAnnouncementDto dto);
        Task<APIOperationResponse<bool>> DeleteAnnouncementAsync(long id);
        Task<APIOperationResponse<bool>> DismissAnnouncementAsync(long announcementId);
        /// <summary>
        /// Clears all announcement dismissals for a user (e.g. on logout so banners reappear on next login).
        /// </summary>
        Task ClearDismissalsForUserAsync(string userId, CancellationToken cancellationToken = default);
    }
}
