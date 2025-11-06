using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ettad.RequestManagement.Service.Common
{
    public class RequestNoGeneratorService : IRequestNoGeneratorService
    {
        private readonly ICrossCuttingRepository<BaseRequest> _requestRepository;
        private readonly ICrossCuttingRepository<Department> _departmentRepository;

        public RequestNoGeneratorService(
            ICrossCuttingRepository<BaseRequest> requestRepository,
            ICrossCuttingRepository<Department> departmentRepository)
        {
            _requestRepository = requestRepository;
            _departmentRepository = departmentRepository;
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
            var nextNumber = await GetNextSequenceNumberAsync(requestType);

            // Format: PREFIX-AutoNumber-DepartmentCode
            return $"{prefix}-{nextNumber:D6}-{departmentCode}";
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

        private async Task<int> GetNextSequenceNumberAsync(RequestType requestType)
        {
            // Get all requests of this type that are not deleted
            var existingRequests = await _requestRepository.FindAsync(
                r => r.RequestType == requestType && !r.IsDeleted
            );

            if (existingRequests == null || !existingRequests.Any())
                return 1;

            // Extract numbers from existing RequestNo values
            var numbers = new List<int>();
            var prefix = GetRequestTypePrefix(requestType);

            foreach (var request in existingRequests)
            {
                if (string.IsNullOrEmpty(request.RequestNo))
                    continue;

                // Parse RequestNo format: PREFIX-Number-DepartmentCode
                var parts = request.RequestNo.Split('-');
                if (parts.Length >= 2 && parts[0] == prefix)
                {
                    if (int.TryParse(parts[1], out var number))
                    {
                        numbers.Add(number);
                    }
                }
            }

            // Return the next number (max + 1, or 1 if no numbers found)
            return numbers.Any() ? numbers.Max() + 1 : 1;
        }
    }
}

