using Ettad.Modules.EmailSystem.API.Models;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.Helpers;

namespace Ettad.Modules.EmailSystem.API.Services
{
    public class EmailDispatchService : IEmailDispatchService
    {
        private readonly IEmailSender _emailSender;

        public EmailDispatchService(IEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task<APIOperationResponse<bool>> SendAsync(
            SendEmailRequestDto request,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Request body is required");
            }

            try
            {
                await _emailSender.SendEmailAsync(
                    request.To.Trim(),
                    request.Subject ?? string.Empty,
                    request.Body ?? string.Empty,
                    request.IsHtml);

                return APIOperationResponse<bool>.Success(true, "Email sent successfully");
            }
            catch (InvalidOperationException ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError,
                    $"Email could not be sent: {ex.Message}");
            }
        }
    }
}

