using Ettad.CrossCutting.Comman.Models;
using Ettad.Data.Entities;
using Ettad.Module.lookup.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Module.lookup.Interfaces
{
    public interface IDepotService : ILookupService<Depot, CreateUpdateDepotDto>
    {
        Task<APIOperationResponse<Depot>> DeleteDepotAsync(long id, CreateUpdateDepotDto item);

        Task<APIOperationResponse<PaginatedList<Depot>>> GetDepotsPaginatedAsync(PagedListRequest request);
    }
}
