using Ettad.HelpCenter.Service.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.HelpCenter.Service
{
    public interface IHelpCenterService
    {
        // ── Articles ──────────────────────────────────────────────────────────
        Task<APIOperationResponse<List<HelpCenterArticleDto>>> GetAllArticlesAsync(bool publishedOnly = false);
        Task<APIOperationResponse<HelpCenterArticleDto>> GetArticleByIdAsync(long id);
        Task<APIOperationResponse<HelpCenterArticleDto>> CreateArticleAsync(CreateHelpCenterArticleDto dto, string createdBy);
        Task<APIOperationResponse<HelpCenterArticleDto>> UpdateArticleAsync(long id, UpdateHelpCenterArticleDto dto, string updatedBy);
        Task<APIOperationResponse<bool>> DeleteArticleAsync(long id, string deletedBy);

        // ── Contact Messages ──────────────────────────────────────────────────
        Task<APIOperationResponse<List<HelpCenterContactMessageDto>>> GetAllContactMessagesAsync();
        Task<APIOperationResponse<HelpCenterContactMessageDto>> GetContactMessageByIdAsync(long id);
        Task<APIOperationResponse<bool>> SubmitContactMessageAsync(SubmitContactMessageDto dto);
        Task<APIOperationResponse<bool>> MarkContactMessageReadAsync(long id);
        Task<APIOperationResponse<bool>> ReplyContactMessageAsync(long id, ReplyContactMessageDto dto, string repliedBy);
        Task<APIOperationResponse<bool>> DeleteContactMessageAsync(long id, string deletedBy);

        Task<APIOperationResponse<HelpCenterContactDisplayDto>> GetContactDisplaySettingsAsync();
        Task<APIOperationResponse<HelpCenterContactDisplayDto>> UpdateContactDisplaySettingsAsync(
            UpdateHelpCenterContactDisplayDto dto, string modifiedBy);

        // ── Terms & Conditions ────────────────────────────────────────────────
        Task<APIOperationResponse<HelpCenterTermsDto>> GetActiveTermsAsync();
        Task<APIOperationResponse<List<HelpCenterTermsDto>>> GetAllTermsVersionsAsync();
        Task<APIOperationResponse<HelpCenterTermsDto>> PublishTermsVersionAsync(UpsertHelpCenterTermsDto dto, string createdBy);
        Task<APIOperationResponse<HelpCenterTermsDto>> ActivateTermsVersionAsync(long id, string modifiedBy);
        Task<APIOperationResponse<bool>> DeactivateTermsVersionAsync(long id, string modifiedBy);
        Task<APIOperationResponse<HelpCenterTermsDto>> UpdateTermsVersionAsync(long id, UpdateHelpCenterTermsDto dto, string modifiedBy);
        Task<APIOperationResponse<bool>> DeleteTermsVersionAsync(long id, string deletedBy);

        Task<APIOperationResponse<TermsAcceptanceStatusDto>> GetTermsAcceptanceStatusAsync(string userId);
        Task<APIOperationResponse<bool>> AcceptActiveTermsAsync(string userId);
    }
}
