using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Common
{
    public interface IRequestNoGeneratorService
    {
        Task<string> GenerateRequestNoAsync(RequestType requestType, long departmentId);
    }
}

