using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Interfaces.Repositories;
using Ettad.RequestManagement.Service.Common.Interfaces;

namespace Ettad.RequestManagement.Service.Common.Services
{
    public class RequestNoGeneratorService : IRequestNoGeneratorService
    {
        private readonly ICrossCuttingRepository<BaseRequest> _requestRepository;
        private readonly ICrossCuttingRepository<Department> _departmentRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RequestNoGeneratorService(
            ICrossCuttingRepository<BaseRequest> requestRepository,
            ICrossCuttingRepository<Department> departmentRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _requestRepository = requestRepository;
            _departmentRepository = departmentRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<string> GenerateRequestNoAsync(RequestType requestType, long departmentId)
        {
            // Get department code
            var department = await _departmentRepository.FindOneAsync(d => d.Id == departmentId && !d.IsDeleted);
            if (department == null)
                throw new ArgumentException($"Department with ID {departmentId} not found");

            var departmentCode = department.Code;

            // Get prefix based on request type
            var prefix = GetRequestTypePrefix(requestType);

            // Get the next sequential number for this request type
            var currentYear = _dateTimeProvider.Now.Year;
            var nextNumber = await GetNextSequenceNumberAsync(requestType, currentYear);

            // Format: PREFIX-Year-AutoNumber-DepartmentCode
            return $"{prefix}-{currentYear}-{nextNumber:D6}-{departmentCode}";
        }

        private string GetRequestTypePrefix(RequestType requestType)
        {
            return requestType switch
            {
                RequestType.Discard => "DISC",
                RequestType.Order => "ORD",
                RequestType.Return => "RTN",
                _ => throw new ArgumentException($"Unknown request type: {requestType}")
            };
        }

        private async Task<int> GetNextSequenceNumberAsync(RequestType requestType, int currentYear)
        {
            // Get all requests of this type that are not deleted
            var existingRequests = await _requestRepository.FindAsync(
                r => r.RequestType == requestType && !r.IsDeleted
            );

            if (existingRequests == null || !existingRequests.Any())
                return 1;

            // Extract numbers from existing RequestNo values for the same year
            var numbers = new List<int>();
            var prefix = GetRequestTypePrefix(requestType);

            foreach (var request in existingRequests)
            {
                if (string.IsNullOrEmpty(request.RequestNo))
                    continue;

                // Parse RequestNo format: PREFIX-Year-Number-DepartmentCode (new) or PREFIX-Number-DepartmentCode (legacy)
                var parts = request.RequestNo.Split('-');
                if (parts.Length >= 4 && parts[0] == prefix)
                {
                    if (int.TryParse(parts[1], out var year) && year == currentYear)
                    {
                        if (int.TryParse(parts[2], out var number))
                        {
                            numbers.Add(number);
                        }
                    }
                }
                // Legacy format without year is ignored so numbering restarts per year
            }

            // Return the next number (max + 1, or 1 if no numbers found)
            return numbers.Any() ? numbers.Max() + 1 : 1;
        }
    }
}

