using System.Collections.Generic;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Module.lookup.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Module.lookup.Interfaces
{
    public interface IUnitLookupService : ILookupService<Unit, CreateUpdateUnitDto>
    {
        Task<APIOperationResponse<List<UnitDto>>> GetByItemTypeAsync(ItemType? itemType);
    }
}
