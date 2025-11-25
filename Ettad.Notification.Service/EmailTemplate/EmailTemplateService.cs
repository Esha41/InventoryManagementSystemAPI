using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;

namespace Ettad.Notification.Service.EmailTemplate
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly ILogger<EmailTemplateService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmailTemplateService(
            ILogger<EmailTemplateService> logger,
            IServiceProvider serviceProvider,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> RenderEmailTemplateAsync(
            string title,
            string message,
            string? entityType = null,
            long? entityId = null,
            long notificationId = 0,
            DateTime? updateDate = null)
        {
            var now = updateDate ?? DateTime.UtcNow;
            var emailBody = new System.Text.StringBuilder();
            
            emailBody.AppendLine("<html><body style='font-family: Arial, sans-serif; line-height: 1.6; color: #1F3A5F; margin: 0; padding: 0; background-color: #F7F7F7;'>");
            emailBody.AppendLine("<div style='max-width: 800px; margin: 0 auto; padding: 20px; background-color: #ffffff;'>");
            
            // Logo Section - Embed as base64 to avoid email client blocking
            var logoBase64 = GetLogoAsBase64();
            if (!string.IsNullOrEmpty(logoBase64))
            {
                emailBody.AppendLine("<div style='text-align: center; margin-bottom: 30px; padding-bottom: 20px; border-bottom: 2px solid #6B6B6B;'>");
                emailBody.AppendLine($"<img src='data:image/png;base64,{logoBase64}' alt='ETTAD Logo' style='max-width: 200px; height: auto;' />");
                emailBody.AppendLine("</div>");
            }
            
            // Header with title
            emailBody.AppendLine("<div style='margin-bottom: 20px; padding-bottom: 15px; border-bottom: 2px solid #6B6B6B;'>");
            emailBody.AppendLine($"<h1 style='margin: 0; color: #1F3A5F; font-size: 24px; font-weight: 600;'>{title}</h1>");
            emailBody.AppendLine("</div>");
            
            // Introductory message
            emailBody.AppendLine($"<p style='font-size: 16px; color: #6B6B6B; margin-bottom: 30px;'>{message}</p>");
            
            // Summary Cards Section
            emailBody.AppendLine("<div style='display: grid; grid-template-columns: repeat(auto-fit, minmax(180px, 1fr)); gap: 15px; margin-bottom: 30px;'>");
            
            // UPDATE DATE Card
            emailBody.AppendLine("<div style='background-color: #F7F7F7; border: 1px solid #6B6B6B; border-radius: 8px; padding: 15px;'>");
            emailBody.AppendLine("<div style='color: #6B6B6B; font-size: 12px; font-weight: 600; text-transform: uppercase; margin-bottom: 8px;'>UPDATE DATE</div>");
            emailBody.AppendLine($"<div style='color: #1F3A5F; font-size: 14px; font-weight: 500;'>{now:MMM dd, yyyy}</div>");
            emailBody.AppendLine("</div>");
            
            // TIME Card
            emailBody.AppendLine("<div style='background-color: #F7F7F7; border: 1px solid #6B6B6B; border-radius: 8px; padding: 15px;'>");
            emailBody.AppendLine("<div style='color: #6B6B6B; font-size: 12px; font-weight: 600; text-transform: uppercase; margin-bottom: 8px;'>TIME</div>");
            emailBody.AppendLine($"<div style='color: #1F3A5F; font-size: 14px; font-weight: 500;'>{now:h:mm tt}</div>");
            emailBody.AppendLine("</div>");
            
            emailBody.AppendLine("</div>"); // End summary cards grid
            
            // View Details Link
            if (!string.IsNullOrEmpty(entityType) && entityId.HasValue)
            {
                var frontendUrl = "http://localhost:4200"; // Default frontend URL, can be configured
                var detailsUrl = $"{frontendUrl}/{entityType.ToLower()}/{entityId.Value}";
                
                emailBody.AppendLine("<div style='text-align: center; margin: 30px 0;'>");
                emailBody.AppendLine($"<a href='{detailsUrl}' style='display: inline-block; padding: 12px 30px; background-color: #2F5DFF; color: #ffffff; text-decoration: none; border-radius: 6px; font-weight: 600; font-size: 14px;'>View Details</a>");
                emailBody.AppendLine("</div>");
            }

            // Fetch and include full entity details if available
            if (!string.IsNullOrEmpty(entityType) && entityId.HasValue)
            {
                var entityDetails = await GetEntityDetailsAsync(entityType, entityId.Value);
                if (entityDetails != null)
                {
                    emailBody.AppendLine($"<div style='margin-top: 30px;'>");
                    emailBody.AppendLine($"<h2 style='color: #1F3A5F; font-size: 18px; font-weight: 600; margin-bottom: 20px; padding-bottom: 10px; border-bottom: 1px solid #6B6B6B;'>{entityType} details</h2>");
                    emailBody.AppendLine(entityDetails);
                    emailBody.AppendLine("</div>");
                }
                else
                {
                    // Fallback if entity details couldn't be fetched
                    emailBody.AppendLine("<div style='margin-top: 20px; padding: 15px; background-color: #F7F7F7; border-left: 4px solid #2F5DFF; border-radius: 4px;'>");
                    emailBody.AppendLine($"<strong>Related Entity:</strong> {entityType}<br>");
                    emailBody.AppendLine($"<strong>Entity ID:</strong> {entityId.Value}");
                    emailBody.AppendLine("</div>");
                }
            }

            emailBody.AppendLine("<hr style='border: none; border-top: 1px solid #6B6B6B; margin: 30px 0 20px 0;'>");
            emailBody.AppendLine("<p style='color: #6B6B6B; font-size: 12px; text-align: center; margin: 0;'>This is an automated notification email.</p>");
            emailBody.AppendLine("</div></body></html>");

            return emailBody.ToString();
        }

        private async Task<string?> GetEntityDetailsAsync(string entityType, long entityId)
        {
            try
            {
                // Handle "Request" entityType (used by workflow notifications)
                if (string.Equals(entityType, "Request", StringComparison.OrdinalIgnoreCase))
                {
                    var baseRequestRepository = _serviceProvider.GetService<ICrossCuttingRepository<BaseRequest>>();
                    if (baseRequestRepository != null)
                    {
                        var request = await baseRequestRepository.FindOneAsync(
                            r => r.Id == entityId && !r.IsDeleted,
                            false,
                            nameof(BaseRequest.Department),
                            nameof(BaseRequest.Requester),
                            nameof(BaseRequest.RequestPurpose),
                            $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}"
                        );

                        if (request != null)
                        {
                            return FormatRequestDetails(request);
                        }
                    }
                }
                else if (string.Equals(entityType, "Discard", StringComparison.OrdinalIgnoreCase))
                {
                    var discardRepository = _serviceProvider.GetService<ICrossCuttingRepository<Discard>>();
                    if (discardRepository != null)
                    {
                        var discard = await discardRepository.FindOneAsync(
                            d => d.Id == entityId && !d.IsDeleted,
                            false,
                            nameof(BaseRequest.Department),
                            nameof(BaseRequest.Requester),
                            nameof(BaseRequest.RequestPurpose),
                            $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}"
                        );

                        if (discard != null)
                        {
                            return FormatRequestDetails(discard);
                        }
                    }
                }
                else if (string.Equals(entityType, "Return", StringComparison.OrdinalIgnoreCase))
                {
                    var returnRepository = _serviceProvider.GetService<ICrossCuttingRepository<Return>>();
                    if (returnRepository != null)
                    {
                        var returnEntity = await returnRepository.FindOneAsync(
                            r => r.Id == entityId && !r.IsDeleted,
                            false,
                            nameof(BaseRequest.Department),
                            nameof(BaseRequest.Requester),
                            nameof(BaseRequest.RequestPurpose),
                            $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}"
                        );

                        if (returnEntity != null)
                        {
                            return FormatRequestDetails(returnEntity);
                        }
                    }
                }
                else if (string.Equals(entityType, "Order", StringComparison.OrdinalIgnoreCase))
                {
                    var orderRepository = _serviceProvider.GetService<ICrossCuttingRepository<Order>>();
                    if (orderRepository != null)
                    {
                        var order = await orderRepository.FindOneAsync(
                            o => o.Id == entityId && !o.IsDeleted,
                            false,
                            nameof(BaseRequest.Department),
                            nameof(BaseRequest.Requester),
                            nameof(BaseRequest.RequestPurpose),
                            $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}"
                        );

                        if (order != null)
                        {
                            return FormatRequestDetails(order);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch entity details for {EntityType} with ID {EntityId}", entityType, entityId);
            }

            return null;
        }

        private string FormatRequestDetails(BaseRequest request)
        {
            var details = new System.Text.StringBuilder();
            details.AppendLine("<div style='background-color: #ffffff;'>");
            
            // Horizontal table for main details
            details.AppendLine("<table style='width: 100%; border-collapse: collapse; margin-bottom: 20px;'>");
            
            // Row 1: REQUEST ID and DEPARTMENT
            details.AppendLine("<tr>");
            details.AppendLine("<td style='padding: 12px; border-bottom: 1px solid #6B6B6B; width: 50%; vertical-align: top;'>");
            details.AppendLine("<div style='color: #6B6B6B; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>REQUEST ID</div>");
            details.AppendLine($"<div style='color: #1F3A5F; font-size: 14px;'>{request.RequestNo ?? "N/A"}</div>");
            details.AppendLine("</td>");
            details.AppendLine("<td style='padding: 12px; border-bottom: 1px solid #6B6B6B; width: 50%; vertical-align: top;'>");
            details.AppendLine("<div style='color: #6B6B6B; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>DEPARTMENT</div>");
            details.AppendLine($"<div style='color: #1F3A5F; font-size: 14px;'>{GetDepartmentName(request.Department)}</div>");
            details.AppendLine("</td>");
            details.AppendLine("</tr>");
            
            // Row 2: REQUESTER and PRIORITY
            details.AppendLine("<tr>");
            details.AppendLine("<td style='padding: 12px; border-bottom: 1px solid #6B6B6B; width: 50%; vertical-align: top;'>");
            details.AppendLine("<div style='color: #6B6B6B; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>REQUESTER</div>");
            details.AppendLine($"<div style='color: #1F3A5F; font-size: 14px;'>{GetRequesterName(request)}</div>");
            details.AppendLine("</td>");
            details.AppendLine("<td style='padding: 12px; border-bottom: 1px solid #6B6B6B; width: 50%; vertical-align: top;'>");
            details.AppendLine("<div style='color: #6B6B6B; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>PRIORITY</div>");
            details.AppendLine($"<div style='color: #1F3A5F; font-size: 14px;'>{request.Priority}</div>");
            details.AppendLine("</td>");
            details.AppendLine("</tr>");
            
            // Row 3: STATUS and PURPOSE
            details.AppendLine("<tr>");
            details.AppendLine("<td style='padding: 12px; border-bottom: 1px solid #6B6B6B; width: 50%; vertical-align: top;'>");
            details.AppendLine("<div style='color: #6B6B6B; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>STATUS</div>");
            details.AppendLine($"<div style='color: #1F3A5F; font-size: 14px;'>{request.Status}</div>");
            details.AppendLine("</td>");
            details.AppendLine("<td style='padding: 12px; border-bottom: 1px solid #6B6B6B; width: 50%; vertical-align: top;'>");
            details.AppendLine("<div style='color: #6B6B6B; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>PURPOSE</div>");
            details.AppendLine($"<div style='color: #1F3A5F; font-size: 14px;'>{GetRequestPurposeName(request.RequestPurpose)}</div>");
            details.AppendLine("</td>");
            details.AppendLine("</tr>");
            
            // Row 4: REASON (if provided) - can span full width or be paired
            if (!string.IsNullOrEmpty(request.Reason))
            {
                details.AppendLine("<tr>");
                details.AppendLine("<td style='padding: 12px; border-bottom: 1px solid #6B6B6B; width: 50%; vertical-align: top;'>");
                details.AppendLine("<div style='color: #6B6B6B; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>REASON</div>");
                details.AppendLine($"<div style='color: #1F3A5F; font-size: 14px;'>{request.Reason}</div>");
                details.AppendLine("</td>");
                // If Notes exists, pair it with Reason, otherwise leave empty
                if (!string.IsNullOrEmpty(request.Notes))
                {
                    details.AppendLine("<td style='padding: 12px; border-bottom: 1px solid #6B6B6B; width: 50%; vertical-align: top;'>");
                    details.AppendLine("<div style='color: #6B6B6B; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>NOTES</div>");
                    details.AppendLine($"<div style='color: #1F3A5F; font-size: 14px;'>{request.Notes}</div>");
                    details.AppendLine("</td>");
                }
                else
                {
                    details.AppendLine("<td style='padding: 12px; border-bottom: 1px solid #6B6B6B; width: 50%; vertical-align: top;'></td>");
                }
                details.AppendLine("</tr>");
            }
            else if (!string.IsNullOrEmpty(request.Notes))
            {
                // Only Notes exists, no Reason
                details.AppendLine("<tr>");
                details.AppendLine("<td style='padding: 12px; border-bottom: 1px solid #6B6B6B; width: 50%; vertical-align: top;'>");
                details.AppendLine("<div style='color: #6B6B6B; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>NOTES</div>");
                details.AppendLine($"<div style='color: #1F3A5F; font-size: 14px;'>{request.Notes}</div>");
                details.AppendLine("</td>");
                details.AppendLine("<td style='padding: 12px; border-bottom: 1px solid #6B6B6B; width: 50%; vertical-align: top;'></td>");
                details.AppendLine("</tr>");
            }
            
            details.AppendLine("</table>");

            // Items
            if (request.RequestItems != null && request.RequestItems.Any())
            {
                details.AppendLine("<div style='margin-top: 20px; padding-top: 20px; border-top: 1px solid #6B6B6B;'>");
                details.AppendLine("<div style='color: #6B6B6B; font-size: 12px; font-weight: 600; margin-bottom: 15px;'>ITEMS</div>");
                foreach (var item in request.RequestItems)
                {
                    var itemName = item.Item?.Name ?? "N/A";
                    var itemNo = item.Item?.ItemNo ?? "N/A";
                    details.AppendLine("<div style='margin-bottom: 10px; padding: 10px; background-color: #F7F7F7; border-radius: 4px;'>");
                    details.AppendLine($"<div style='color: #1F3A5F; font-size: 14px; font-weight: 500; margin-bottom: 5px;'>{itemName}</div>");
                    details.AppendLine($"<div style='color: #6B6B6B; font-size: 12px;'>Quantity: {item.Quantity}</div>");
                    details.AppendLine("</div>");
                }
                details.AppendLine("</div>");
            }

            details.AppendLine("</div>");
            return details.ToString();
        }

        private string GetDepartmentName(Department? department)
        {
            if (department != null)
            {
                if (!string.IsNullOrEmpty(department.NameEn))
                    return department.NameEn;
                if (!string.IsNullOrEmpty(department.NameAr))
                    return department.NameAr;
            }
            return "N/A";
        }

        private string GetRequestPurposeName(RequestPurpose? requestPurpose)
        {
            if (requestPurpose != null)
            {
                if (!string.IsNullOrEmpty(requestPurpose.NameEn))
                    return requestPurpose.NameEn;
                if (!string.IsNullOrEmpty(requestPurpose.NameAr))
                    return requestPurpose.NameAr;
            }
            return "N/A";
        }

        private string GetRequesterName(BaseRequest request)
        {
            if (request.Requester != null)
            {
                if (!string.IsNullOrEmpty(request.Requester.FullNameEN))
                    return request.Requester.FullNameEN;
                if (!string.IsNullOrEmpty(request.Requester.FullNameAR))
                    return request.Requester.FullNameAR;
                if (!string.IsNullOrEmpty(request.Requester.UserName))
                    return request.Requester.UserName;
            }
            return "N/A";
        }

        private string GetLogoAsBase64()
        {
            try
            {
                var logoPath = Path.Combine(_webHostEnvironment.WebRootPath ?? "wwwroot", "Assets", "logo", "logo.png");
                
                if (File.Exists(logoPath))
                {
                    var logoBytes = File.ReadAllBytes(logoPath);
                    return Convert.ToBase64String(logoBytes);
                }
                else
                {
                    _logger.LogWarning("Logo file not found at path: {LogoPath}", logoPath);
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load logo file for email template");
                return string.Empty;
            }
        }
    }
}

