using Ettad.Data.Entities;
using Ettad.Module.lookup.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Lookups.Services.Contracts
{
    public interface IDepotService : ILookupService<Depot, CreateUpdateDepotDto>
    {
        Task<APIOperationResponse<Depot>> DeleteDepotAsync(long id, CreateUpdateDepotDto item);
    }
}
