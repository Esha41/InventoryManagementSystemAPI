using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Common.Security;
using Ettad.HelpCenter.Service;
using Ettad.HelpCenter.Service.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.HelpCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HelpCenterController : ApiControllerBase
    {
        private readonly IHelpCenterService _helpCenterService;
        private readonly ICurrentUserService _currentUserService;

        public HelpCenterController(
            IHelpCenterService helpCenterService,
            ICurrentUserService currentUserService)
        {
            _helpCenterService = helpCenterService;
            _currentUserService = currentUserService;
        }

        // ── Articles (public read, admin write) ──────────────────────────────

        /// <summary>Get all published articles (user-facing).</summary>
        [HttpGet("articles")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPublishedArticles()
        {
            var result = await _helpCenterService.GetAllArticlesAsync(publishedOnly: true);
            return ProcessResponse(result);
        }

        /// <summary>Get all articles including unpublished (admin).</summary>
        [HttpGet("articles/all")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.View")]
        public async Task<IActionResult> GetAllArticles()
        {
            var result = await _helpCenterService.GetAllArticlesAsync(publishedOnly: false);
            return ProcessResponse(result);
        }

        /// <summary>Get article by ID.</summary>
        [HttpGet("articles/{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetArticleById(long id)
        {
            var result = await _helpCenterService.GetArticleByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>Create a new help article (admin).</summary>
        [HttpPost("articles")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.HelpCenter.Create")]
        public async Task<IActionResult> CreateArticle([FromBody] CreateHelpCenterArticleDto dto)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ProcessResponse(APIOperationResponse<HelpCenterArticleDto>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, "User not authenticated"));

            var result = await _helpCenterService.CreateArticleAsync(dto, userId);
            return ProcessResponse(result);
        }

        /// <summary>Update a help article (admin).</summary>
        [HttpPut("articles/{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.Edit")]
        public async Task<IActionResult> UpdateArticle(long id, [FromBody] UpdateHelpCenterArticleDto dto)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ProcessResponse(APIOperationResponse<HelpCenterArticleDto>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, "User not authenticated"));

            var result = await _helpCenterService.UpdateArticleAsync(id, dto, userId);
            return ProcessResponse(result);
        }

        /// <summary>Delete a help article (admin, soft delete).</summary>
        [HttpDelete("articles/{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.Delete")]
        public async Task<IActionResult> DeleteArticle(long id)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, "User not authenticated"));

            var result = await _helpCenterService.DeleteArticleAsync(id, userId);
            return ProcessResponse(result);
        }

        // ── Contact Messages ─────────────────────────────────────────────────

        /// <summary>Submit a contact message (any authenticated user).</summary>
        [HttpPost("contact")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SubmitContactMessage([FromBody] SubmitContactMessageDto dto)
        {
            var result = await _helpCenterService.SubmitContactMessageAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>Get all contact messages (admin).</summary>
        [HttpGet("contact")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.View")]
        public async Task<IActionResult> GetAllContactMessages()
        {
            var result = await _helpCenterService.GetAllContactMessagesAsync();
            return ProcessResponse(result);
        }

        /// <summary>Get a single contact message by ID (admin).</summary>
        [HttpGet("contact/{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.View")]
        public async Task<IActionResult> GetContactMessageById(long id)
        {
            // Auto-mark as read when an admin opens a message
            await _helpCenterService.MarkContactMessageReadAsync(id);
            var result = await _helpCenterService.GetContactMessageByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>Reply to a contact message (admin).</summary>
        [HttpPost("contact/{id:long}/reply")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.Edit")]
        public async Task<IActionResult> ReplyContactMessage(long id, [FromBody] ReplyContactMessageDto dto)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, "User not authenticated"));

            var result = await _helpCenterService.ReplyContactMessageAsync(id, dto, userId);
            return ProcessResponse(result);
        }

        /// <summary>Delete a contact message (admin, soft delete).</summary>
        [HttpDelete("contact/{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.Delete")]
        public async Task<IActionResult> DeleteContactMessage(long id)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, "User not authenticated"));

            var result = await _helpCenterService.DeleteContactMessageAsync(id, userId);
            return ProcessResponse(result);
        }

        /// <summary>Support email and phone shown on the user Contact tab.</summary>
        [HttpGet("contact/display")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContactDisplaySettings()
        {
            var result = await _helpCenterService.GetContactDisplaySettingsAsync();
            return ProcessResponse(result);
        }

        /// <summary>Update support email and phone (admin).</summary>
        [HttpPut("contact/display")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.Edit")]
        public async Task<IActionResult> UpdateContactDisplaySettings([FromBody] UpdateHelpCenterContactDisplayDto dto)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ProcessResponse(APIOperationResponse<HelpCenterContactDisplayDto>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, "User not authenticated"));

            var result = await _helpCenterService.UpdateContactDisplaySettingsAsync(dto, userId);
            return ProcessResponse(result);
        }

        // ── Terms & Conditions ────────────────────────────────────────────────

        /// <summary>Whether the user must accept the current active terms (e.g. after a new version was published).</summary>
        [HttpGet("terms/acceptance-status")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTermsAcceptanceStatus()
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ProcessResponse(APIOperationResponse<TermsAcceptanceStatusDto>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, "User not authenticated"));

            var result = await _helpCenterService.GetTermsAcceptanceStatusAsync(userId);
            return ProcessResponse(result);
        }

        /// <summary>Record acceptance of the current active terms for the logged-in user.</summary>
        [HttpPost("terms/accept")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> AcceptActiveTerms()
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, "User not authenticated"));

            var result = await _helpCenterService.AcceptActiveTermsAsync(userId);
            return ProcessResponse(result);
        }

        /// <summary>Get active Terms & Conditions (any authenticated user).</summary>
        [HttpGet("terms")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActiveTerms()
        {
            var result = await _helpCenterService.GetActiveTermsAsync();
            return ProcessResponse(result);
        }

        /// <summary>Get all Terms & Conditions versions (admin).</summary>
        [HttpGet("terms/all")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.View")]
        public async Task<IActionResult> GetAllTermsVersions()
        {
            var result = await _helpCenterService.GetAllTermsVersionsAsync();
            return ProcessResponse(result);
        }

        /// <summary>Publish a new Terms & Conditions version (admin). Deactivates all previous versions.</summary>
        [HttpPost("terms")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.Create")]
        public async Task<IActionResult> PublishTermsVersion([FromBody] UpsertHelpCenterTermsDto dto)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ProcessResponse(APIOperationResponse<HelpCenterTermsDto>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, "User not authenticated"));

            var result = await _helpCenterService.PublishTermsVersionAsync(dto, userId);
            return ProcessResponse(result);
        }

        /// <summary>Activate a Terms &amp; Conditions version (admin). Deactivates all other versions.</summary>
        [HttpPost("terms/{id:long}/activate")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.Edit")]
        public async Task<IActionResult> ActivateTermsVersion(long id)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ProcessResponse(APIOperationResponse<HelpCenterTermsDto>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, "User not authenticated"));

            var result = await _helpCenterService.ActivateTermsVersionAsync(id, userId);
            return ProcessResponse(result);
        }

        /// <summary>Deactivate a Terms &amp; Conditions version (admin). Fails if it is the only active version.</summary>
        [HttpPost("terms/{id:long}/deactivate")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.Edit")]
        public async Task<IActionResult> DeactivateTermsVersion(long id)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, "User not authenticated"));

            var result = await _helpCenterService.DeactivateTermsVersionAsync(id, userId);
            return ProcessResponse(result);
        }

        /// <summary>Update a Terms &amp; Conditions version (admin).</summary>
        [HttpPut("terms/{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.Edit")]
        public async Task<IActionResult> UpdateTermsVersion(long id, [FromBody] UpdateHelpCenterTermsDto dto)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ProcessResponse(APIOperationResponse<HelpCenterTermsDto>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, "User not authenticated"));

            var result = await _helpCenterService.UpdateTermsVersionAsync(id, dto, userId);
            return ProcessResponse(result);
        }

        /// <summary>Soft-delete a Terms &amp; Conditions version (admin). Not allowed for the active version.</summary>
        [HttpDelete("terms/{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.HelpCenter.Delete")]
        public async Task<IActionResult> DeleteTermsVersion(long id)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, "User not authenticated"));

            var result = await _helpCenterService.DeleteTermsVersionAsync(id, userId);
            return ProcessResponse(result);
        }
    }
}
