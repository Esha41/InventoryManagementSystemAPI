using Ettad.Announcement.Service;
using Ettad.Announcement.Service.Dtos;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Common.Security;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Announcement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnnouncementController : ApiControllerBase
    {
        private readonly IAnnouncementService _announcementService;
        private readonly ICurrentUserService _currentUserService;

        public AnnouncementController(
            IAnnouncementService announcementService,
            ICurrentUserService currentUserService)
        {
            _announcementService = announcementService;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Get all announcements (Admin only)
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Announcements.View")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _announcementService.GetAllAnnouncementsAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get announcement by ID (Admin only)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Announcements.View")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _announcementService.GetAnnouncementByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get active announcements for current user
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActive()
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return ProcessResponse(APIOperationResponse<List<ActiveAnnouncementDto>>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized,
                    "User not authenticated"));
            }

            var result = await _announcementService.GetActiveAnnouncementsForUserAsync(userId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create new announcement (Admin only)
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Announcements.Create")]
        public async Task<IActionResult> Create([FromBody] CreateAnnouncementDto dto)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return ProcessResponse(APIOperationResponse<AnnouncementDto>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized,
                    "User not authenticated"));
            }

            var result = await _announcementService.CreateAnnouncementAsync(dto, userId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update announcement (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Announcements.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateAnnouncementDto dto)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return ProcessResponse(APIOperationResponse<AnnouncementDto>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized,
                    "User not authenticated"));
            }

            var result = await _announcementService.UpdateAnnouncementAsync(id, dto, userId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Delete announcement (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Announcements.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized,
                    "User not authenticated"));
            }

            var result = await _announcementService.DeleteAnnouncementAsync(id, userId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Dismiss announcement for current user
        /// </summary>
        [HttpPost("{id}/dismiss")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Dismiss(long id)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized,
                    "User not authenticated"));
            }

            var result = await _announcementService.DismissAnnouncementAsync(id, userId);
            return ProcessResponse(result);
        }
    }
}
