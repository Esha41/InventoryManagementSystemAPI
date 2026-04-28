using System.Collections.Generic;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Module.lookup.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Module.lookup.Interfaces
{
    public interface ICaliberLookupService : ILookupService<Caliber, CreateUpdateCaliberDto>
    {
        Task<APIOperationResponse<List<CaliberDto>>> GetByItemTypeAsync(ItemType? itemType);
    }
}
