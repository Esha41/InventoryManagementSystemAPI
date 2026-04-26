using Ettad.Announcement.Service.Dtos;
using Ettad.Announcement.Service.Interfaces;
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

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
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
            var result = await _announcementService.GetActiveAnnouncementsForUserAsync();
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
            var result = await _announcementService.CreateAnnouncementAsync(dto);
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
            var result = await _announcementService.UpdateAnnouncementAsync(id, dto);
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
            var result = await _announcementService.DeleteAnnouncementAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Dismiss announcement for current user
        /// </summary>
        [HttpPost("{id}/dismiss")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Dismiss(long id)
        {
            var result = await _announcementService.DismissAnnouncementAsync(id);
            return ProcessResponse(result);
        }
    }
}
