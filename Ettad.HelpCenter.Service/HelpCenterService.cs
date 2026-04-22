using System.Net.Mail;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities;
using Ettad.Data.Interfaces.Repositories;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.HelpCenter.Service.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.EntityFrameworkCore;

namespace Ettad.HelpCenter.Service
{
    public class HelpCenterService : IHelpCenterService
    {
        private readonly ICrossCuttingRepository<HelpCenterArticle> _articleRepo;
        private readonly ICrossCuttingRepository<HelpCenterContactMessage> _contactRepo;
        private readonly ICrossCuttingRepository<HelpCenterTermsConditions> _termsRepo;
        private readonly IDateTimeProvider _dateTime;
        private readonly ApplicationDbContext _db;
        private readonly ITransactionManager _transactionManager;

        public HelpCenterService(
            ICrossCuttingRepository<HelpCenterArticle> articleRepo,
            ICrossCuttingRepository<HelpCenterContactMessage> contactRepo,
            ICrossCuttingRepository<HelpCenterTermsConditions> termsRepo,
            IDateTimeProvider dateTime,
            ApplicationDbContext db,
            ITransactionManager transactionManager)
        {
            _articleRepo = articleRepo;
            _contactRepo = contactRepo;
            _termsRepo = termsRepo;
            _dateTime = dateTime;
            _db = db;
            _transactionManager = transactionManager;
        }

        // ── Articles ──────────────────────────────────────────────────────────

        public async Task<APIOperationResponse<List<HelpCenterArticleDto>>> GetAllArticlesAsync(bool publishedOnly = false)
        {
            try
            {
                var articles = await _articleRepo.FindAsync(
                    a => !a.IsDeleted && (!publishedOnly || a.IsPublished), false);

                var dtos = articles
                    .OrderBy(a => a.Category ?? string.Empty)
                    .ThenBy(a => a.Title)
                    .Select(MapArticleToDto)
                    .ToList();

                return APIOperationResponse<List<HelpCenterArticleDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<HelpCenterArticleDto>>.Fail(
                    ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<HelpCenterArticleDto>> GetArticleByIdAsync(long id)
        {
            try
            {
                var article = await _articleRepo.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (article is null)
                    return APIOperationResponse<HelpCenterArticleDto>.Fail(ResponseType.NotFound, "Article not found");

                return APIOperationResponse<HelpCenterArticleDto>.Success(MapArticleToDto(article));
            }
            catch (Exception ex)
            {
                return APIOperationResponse<HelpCenterArticleDto>.Fail(
                    ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<HelpCenterArticleDto>> CreateArticleAsync(
            CreateHelpCenterArticleDto dto, string createdBy)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                    return APIOperationResponse<HelpCenterArticleDto>.Fail(ResponseType.BadRequest, "Title is required");

                if (string.IsNullOrWhiteSpace(dto.Content))
                    return APIOperationResponse<HelpCenterArticleDto>.Fail(ResponseType.BadRequest, "Content is required");

                var article = new HelpCenterArticle
                {
                    Title = dto.Title.Trim(),
                    Content = dto.Content,
                    Category = dto.Category?.Trim(),
                    SortOrder = 0,
                    IsPublished = dto.IsPublished,
                    CreationDate = _dateTime.Now,
                    CreatedBy = createdBy
                };

                var created = await _articleRepo.AddAsync(article);
                return APIOperationResponse<HelpCenterArticleDto>.Success(
                    MapArticleToDto(created), "Article created successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<HelpCenterArticleDto>.Fail(
                    ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<HelpCenterArticleDto>> UpdateArticleAsync(
            long id, UpdateHelpCenterArticleDto dto, string updatedBy)
        {
            try
            {
                var article = await _articleRepo.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (article is null)
                    return APIOperationResponse<HelpCenterArticleDto>.Fail(ResponseType.NotFound, "Article not found");

                if (dto.Title is not null)
                {
                    if (string.IsNullOrWhiteSpace(dto.Title))
                        return APIOperationResponse<HelpCenterArticleDto>.Fail(ResponseType.BadRequest, "Title cannot be empty");
                    article.Title = dto.Title.Trim();
                }

                if (dto.Content is not null)
                {
                    if (string.IsNullOrWhiteSpace(dto.Content))
                        return APIOperationResponse<HelpCenterArticleDto>.Fail(ResponseType.BadRequest, "Content cannot be empty");
                    article.Content = dto.Content;
                }

                if (dto.Category is not null) article.Category = dto.Category.Trim();
                if (dto.IsPublished.HasValue) article.IsPublished = dto.IsPublished.Value;

                article.ModificationDate = _dateTime.Now;
                article.ModifiedBy = updatedBy;

                var updated = await _articleRepo.UpdateAsync(article);
                return APIOperationResponse<HelpCenterArticleDto>.Success(
                    MapArticleToDto(updated), "Article updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<HelpCenterArticleDto>.Fail(
                    ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteArticleAsync(long id, string deletedBy)
        {
            try
            {
                var article = await _articleRepo.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (article is null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Article not found");

                article.IsDeleted = true;
                article.DeletionDate = _dateTime.Now;
                article.DeletedBy = deletedBy;

                await _articleRepo.UpdateAsync(article);
                return APIOperationResponse<bool>.Success(true, "Article deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        // ── Contact Messages ──────────────────────────────────────────────────

        public async Task<APIOperationResponse<List<HelpCenterContactMessageDto>>> GetAllContactMessagesAsync()
        {
            try
            {
                var messages = await _contactRepo.FindAsync(m => !m.IsDeleted, false);
                var dtos = messages
                    .OrderByDescending(m => m.CreationDate)
                    .Select(MapContactToDto)
                    .ToList();

                return APIOperationResponse<List<HelpCenterContactMessageDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<HelpCenterContactMessageDto>>.Fail(
                    ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<HelpCenterContactMessageDto>> GetContactMessageByIdAsync(long id)
        {
            try
            {
                var message = await _contactRepo.FindOneAsync(m => m.Id == id && !m.IsDeleted);
                if (message is null)
                    return APIOperationResponse<HelpCenterContactMessageDto>.Fail(ResponseType.NotFound, "Message not found");

                return APIOperationResponse<HelpCenterContactMessageDto>.Success(MapContactToDto(message));
            }
            catch (Exception ex)
            {
                return APIOperationResponse<HelpCenterContactMessageDto>.Fail(
                    ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<bool>> SubmitContactMessageAsync(SubmitContactMessageDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.SenderName))
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Name is required");
                if (string.IsNullOrWhiteSpace(dto.SenderEmail))
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Email is required");
                if (string.IsNullOrWhiteSpace(dto.Subject))
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Subject is required");
                if (string.IsNullOrWhiteSpace(dto.Body))
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Message body is required");

                var message = new HelpCenterContactMessage
                {
                    SenderName = dto.SenderName.Trim(),
                    SenderEmail = dto.SenderEmail.Trim().ToLower(),
                    Subject = dto.Subject.Trim(),
                    Body = dto.Body.Trim(),
                    IsRead = false,
                    CreationDate = _dateTime.Now
                };

                await _contactRepo.AddAsync(message);
                return APIOperationResponse<bool>.Success(true, "Message submitted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<bool>> MarkContactMessageReadAsync(long id)
        {
            try
            {
                var message = await _contactRepo.FindOneAsync(m => m.Id == id && !m.IsDeleted);
                if (message is null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Message not found");

                if (message.IsRead)
                    return APIOperationResponse<bool>.Success(true);

                message.IsRead = true;
                await _contactRepo.UpdateAsync(message);
                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<bool>> ReplyContactMessageAsync(
            long id, ReplyContactMessageDto dto, string repliedBy)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.AdminReply))
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Reply cannot be empty");

                var message = await _contactRepo.FindOneAsync(m => m.Id == id && !m.IsDeleted);
                if (message is null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Message not found");

                message.AdminReply = dto.AdminReply.Trim();
                message.RepliedAt = _dateTime.Now;
                message.RepliedBy = repliedBy;
                message.IsRead = true;

                await _contactRepo.UpdateAsync(message);
                return APIOperationResponse<bool>.Success(true, "Reply saved successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteContactMessageAsync(long id, string deletedBy)
        {
            try
            {
                var message = await _contactRepo.FindOneAsync(m => m.Id == id && !m.IsDeleted);
                if (message is null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Message not found");

                message.IsDeleted = true;
                message.DeletionDate = _dateTime.Now;
                message.DeletedBy = deletedBy;

                await _contactRepo.UpdateAsync(message);
                return APIOperationResponse<bool>.Success(true, "Message deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<HelpCenterContactDisplayDto>> GetContactDisplaySettingsAsync()
        {
            try
            {
                var row = await _db.HelpCenterContactDisplaySettings.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == 1);
                if (row is null)
                {
                    return APIOperationResponse<HelpCenterContactDisplayDto>.Success(
                        new HelpCenterContactDisplayDto());
                }

                return APIOperationResponse<HelpCenterContactDisplayDto>.Success(MapContactDisplay(row));
            }
            catch (Exception ex)
            {
                return APIOperationResponse<HelpCenterContactDisplayDto>.Fail(
                    ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<HelpCenterContactDisplayDto>> UpdateContactDisplaySettingsAsync(
            UpdateHelpCenterContactDisplayDto dto, string modifiedBy)
        {
            try
            {
                var email = (dto.SupportEmail ?? string.Empty).Trim();
                var phone = (dto.SupportPhone ?? string.Empty).Trim();

                if (email.Length > 200)
                    return APIOperationResponse<HelpCenterContactDisplayDto>.Fail(
                        ResponseType.BadRequest, "Support email is too long");

                if (phone.Length > 50)
                    return APIOperationResponse<HelpCenterContactDisplayDto>.Fail(
                        ResponseType.BadRequest, "Support phone is too long");

                if (!string.IsNullOrEmpty(email) && !IsPlausibleEmail(email))
                    return APIOperationResponse<HelpCenterContactDisplayDto>.Fail(
                        ResponseType.BadRequest, "Invalid support email format");

                var row = await _db.HelpCenterContactDisplaySettings.FirstOrDefaultAsync(x => x.Id == 1);
                if (row is null)
                {
                    row = new HelpCenterContactDisplaySettings { Id = 1 };
                    _db.HelpCenterContactDisplaySettings.Add(row);
                }

                row.SupportEmail = email;
                row.SupportPhone = phone;
                row.ModifiedAt = _dateTime.Now;
                row.ModifiedBy = modifiedBy;

                await _db.SaveChangesAsync();
                return APIOperationResponse<HelpCenterContactDisplayDto>.Success(MapContactDisplay(row));
            }
            catch (Exception ex)
            {
                return APIOperationResponse<HelpCenterContactDisplayDto>.Fail(
                    ResponseType.InternalServerError, ex.Message);
            }
        }

        private static HelpCenterContactDisplayDto MapContactDisplay(HelpCenterContactDisplaySettings x) =>
            new()
            {
                SupportEmail = x.SupportEmail ?? string.Empty,
                SupportPhone = x.SupportPhone ?? string.Empty
            };

        private static bool IsPlausibleEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return string.Equals(addr.Address, email, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        // ── Terms & Conditions ────────────────────────────────────────────────

        public async Task<APIOperationResponse<HelpCenterTermsDto>> GetActiveTermsAsync()
        {
            try
            {
                var terms = await _termsRepo.FindOneAsync(t => t.IsActive && !t.IsDeleted);
                if (terms is null)
                    return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.NotFound, "No active terms found");

                return APIOperationResponse<HelpCenterTermsDto>.Success(MapTermsToDto(terms));
            }
            catch (Exception ex)
            {
                return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<List<HelpCenterTermsDto>>> GetAllTermsVersionsAsync()
        {
            try
            {
                var allTerms = await _termsRepo.FindAsync(t => !t.IsDeleted, false);
                var dtos = allTerms
                    .OrderByDescending(t => t.EffectiveDate)
                    .Select(MapTermsToDto)
                    .ToList();

                return APIOperationResponse<List<HelpCenterTermsDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<HelpCenterTermsDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<HelpCenterTermsDto>> PublishTermsVersionAsync(
            UpsertHelpCenterTermsDto dto, string createdBy)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Version))
                    return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.BadRequest, "Version is required");
                if (string.IsNullOrWhiteSpace(dto.Content))
                    return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.BadRequest, "Content is required");

                var trimmedVersion = dto.Version.Trim();
                if (trimmedVersion.Length > 50)
                    return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.BadRequest,
                        "Version must be 50 characters or fewer");

                // DB has a unique index on Version (all rows, including soft-deleted). Reusing a label fails SaveChanges.
                var versionTaken = await _db.HelpCenterTermsConditions
                    .AnyAsync(t => t.Version == trimmedVersion);
                if (versionTaken)
                    return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.BadRequest,
                        "This version label is already in use. Enter a new version (for example 1.1). Labels cannot be reused even after a version is deleted.");

                await using var tx = await _transactionManager.BeginAsync();

                var activeTerms = await _db.HelpCenterTermsConditions
                    .Where(t => t.IsActive && !t.IsDeleted)
                    .ToListAsync();

                foreach (var existing in activeTerms)
                {
                    existing.IsActive = false;
                    existing.ModificationDate = _dateTime.Now;
                    existing.ModifiedBy = createdBy;
                }

                var newTerms = new HelpCenterTermsConditions
                {
                    Version = trimmedVersion,
                    Content = dto.Content,
                    IsActive = true,
                    EffectiveDate = dto.EffectiveDate,
                    CreationDate = _dateTime.Now,
                    CreatedBy = createdBy
                };

                _db.HelpCenterTermsConditions.Add(newTerms);
                await _db.SaveChangesAsync();
                await _transactionManager.CommitAsync();

                return APIOperationResponse<HelpCenterTermsDto>.Success(
                    MapTermsToDto(newTerms), "Terms published successfully");
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                if (inner.Contains("IX_HelpCenterTermsConditions_Version", StringComparison.OrdinalIgnoreCase)
                    || inner.Contains("UNIQUE KEY constraint", StringComparison.OrdinalIgnoreCase)
                    || inner.Contains("duplicate key", StringComparison.OrdinalIgnoreCase))
                {
                    return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.BadRequest,
                        "This version label is already in use. Enter a different version.");
                }

                return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.InternalServerError, inner);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<HelpCenterTermsDto>> ActivateTermsVersionAsync(long id, string modifiedBy)
        {
            try
            {
                await using var tx = await _transactionManager.BeginAsync();

                var target = await _db.HelpCenterTermsConditions
                    .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
                if (target is null)
                {
                    await _transactionManager.RollbackAsync();
                    return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.NotFound, "Terms version not found");
                }

                var others = await _db.HelpCenterTermsConditions
                    .Where(t => !t.IsDeleted && t.Id != id && t.IsActive)
                    .ToListAsync();

                foreach (var t in others)
                {
                    t.IsActive = false;
                    t.ModificationDate = _dateTime.Now;
                    t.ModifiedBy = modifiedBy;
                }

                target.IsActive = true;
                target.ModificationDate = _dateTime.Now;
                target.ModifiedBy = modifiedBy;

                await _db.SaveChangesAsync();
                await _transactionManager.CommitAsync();

                return APIOperationResponse<HelpCenterTermsDto>.Success(
                    MapTermsToDto(target), "Terms version activated");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<bool>> DeactivateTermsVersionAsync(long id, string modifiedBy)
        {
            try
            {
                var entity = await _db.HelpCenterTermsConditions
                    .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
                if (entity is null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Terms version not found");

                if (!entity.IsActive)
                    return APIOperationResponse<bool>.Success(true);

                var activeCount = await _db.HelpCenterTermsConditions
                    .CountAsync(t => !t.IsDeleted && t.IsActive);
                if (activeCount <= 1)
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Cannot deactivate the only active version. Activate another version first.");

                entity.IsActive = false;
                entity.ModificationDate = _dateTime.Now;
                entity.ModifiedBy = modifiedBy;
                await _db.SaveChangesAsync();

                return APIOperationResponse<bool>.Success(true, "Terms version deactivated");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<HelpCenterTermsDto>> UpdateTermsVersionAsync(
            long id, UpdateHelpCenterTermsDto dto, string modifiedBy)
        {
            try
            {
                if (dto.Version is null && dto.Content is null && !dto.EffectiveDate.HasValue)
                    return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.BadRequest,
                        "At least one field must be provided");

                var entity = await _db.HelpCenterTermsConditions
                    .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
                if (entity is null)
                    return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.NotFound, "Terms version not found");

                if (dto.Version is not null)
                {
                    if (string.IsNullOrWhiteSpace(dto.Version))
                        return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.BadRequest, "Version cannot be empty");
                    var trimmed = dto.Version.Trim();
                    if (trimmed.Length > 50)
                        return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.BadRequest, "Version must be 50 characters or fewer");
                    // Unique index applies to all rows (including soft-deleted).
                    var versionTaken = await _db.HelpCenterTermsConditions
                        .AnyAsync(t => t.Id != id && t.Version == trimmed);
                    if (versionTaken)
                        return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.BadRequest,
                            "This version label is already in use. Labels cannot be reused even after a version is deleted.");
                    entity.Version = trimmed;
                }

                if (dto.Content is not null)
                {
                    if (string.IsNullOrWhiteSpace(dto.Content))
                        return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.BadRequest, "Content cannot be empty");
                    entity.Content = dto.Content;
                }

                if (dto.EffectiveDate.HasValue)
                    entity.EffectiveDate = dto.EffectiveDate.Value;

                entity.ModificationDate = _dateTime.Now;
                entity.ModifiedBy = modifiedBy;
                await _db.SaveChangesAsync();

                return APIOperationResponse<HelpCenterTermsDto>.Success(
                    MapTermsToDto(entity), "Terms version updated");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<HelpCenterTermsDto>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteTermsVersionAsync(long id, string deletedBy)
        {
            try
            {
                var entity = await _db.HelpCenterTermsConditions
                    .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
                if (entity is null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Terms version not found");

                if (entity.IsActive)
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Deactivate this version before deleting it");

                entity.IsDeleted = true;
                entity.DeletionDate = _dateTime.Now;
                entity.DeletedBy = deletedBy;
                await _db.SaveChangesAsync();

                return APIOperationResponse<bool>.Success(true, "Terms version deleted");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<TermsAcceptanceStatusDto>> GetTermsAcceptanceStatusAsync(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    return APIOperationResponse<TermsAcceptanceStatusDto>.Fail(ResponseType.Unauthorized, "User not authenticated");

                var active = await _termsRepo.FindOneAsync(t => t.IsActive && !t.IsDeleted);
                if (active is null)
                {
                    return APIOperationResponse<TermsAcceptanceStatusDto>.Success(
                        new TermsAcceptanceStatusDto { MustAccept = false, Terms = null });
                }

                var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
                if (user is null)
                    return APIOperationResponse<TermsAcceptanceStatusDto>.Fail(ResponseType.NotFound, "User not found");

                var mustAccept = user.LastAcceptedTermsConditionsId != active.Id;
                var dto = new TermsAcceptanceStatusDto
                {
                    MustAccept = mustAccept,
                    Terms = mustAccept ? MapTermsToDto(active) : null
                };
                return APIOperationResponse<TermsAcceptanceStatusDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<TermsAcceptanceStatusDto>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<bool>> AcceptActiveTermsAsync(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    return APIOperationResponse<bool>.Fail(ResponseType.Unauthorized, "User not authenticated");

                var active = await _termsRepo.FindOneAsync(t => t.IsActive && !t.IsDeleted);
                if (active is null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "No active terms found");

                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user is null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "User not found");

                user.LastAcceptedTermsConditionsId = active.Id;
                await _db.SaveChangesAsync();
                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        // ── Mapping helpers ───────────────────────────────────────────────────

        private static HelpCenterArticleDto MapArticleToDto(HelpCenterArticle a) => new()
        {
            Id = a.Id,
            Title = a.Title,
            Content = a.Content,
            Category = a.Category,
            IsPublished = a.IsPublished,
            CreationDate = a.CreationDate,
            CreatedBy = a.CreatedBy,
            ModificationDate = a.ModificationDate,
            ModifiedBy = a.ModifiedBy
        };

        private static HelpCenterContactMessageDto MapContactToDto(HelpCenterContactMessage m) => new()
        {
            Id = m.Id,
            SenderName = m.SenderName,
            SenderEmail = m.SenderEmail,
            Subject = m.Subject,
            Body = m.Body,
            IsRead = m.IsRead,
            AdminReply = m.AdminReply,
            RepliedAt = m.RepliedAt,
            RepliedBy = m.RepliedBy,
            CreationDate = m.CreationDate
        };

        private static HelpCenterTermsDto MapTermsToDto(HelpCenterTermsConditions t) => new()
        {
            Id = t.Id,
            Version = t.Version,
            Content = t.Content,
            IsActive = t.IsActive,
            EffectiveDate = t.EffectiveDate,
            CreationDate = t.CreationDate,
            CreatedBy = t.CreatedBy
        };
    }
}
