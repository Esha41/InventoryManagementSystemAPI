using Ettad.Inventory.Service.AllowanceItems.Dtos;

namespace Ettad.Inventory.Service.AllowanceItems
{
    /// <summary>
    /// Query service for fetching allowance items without permission checks.
    /// Used by DevExpress reports and other services that don't have HttpContext.
    /// </summary>
    public interface IAllowanceItemQueryService
    {
        /// <summary>
        /// Gets all allowance items without filtering by department or checking permissions.
        /// </summary>
        Task<List<AllowanceItemDto>> GetAllAsync();
    }
}
