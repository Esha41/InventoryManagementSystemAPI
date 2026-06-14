namespace Ettad.Inventory.Service.Batches.Dtos
{
    /// <summary>
    /// Reusable filter DTO for narrowing batch assets by item, supplier, manufacturer,
    /// and/or primary purpose. Used by summary and detail endpoints.
    /// </summary>
    public class BatchAssetFilterDto
    {
        public List<long>? ItemIds { get; set; }
        public List<long>? SupplierIds { get; set; }
        public List<long>? ManufacturerIds { get; set; }
        public List<long>? PrimaryPurposeIds { get; set; }
        public List<long>? CaliberIds { get; set; }

        public bool HasAnyFilter =>
            ItemIds is { Count: > 0 } ||
            SupplierIds is { Count: > 0 } ||
            ManufacturerIds is { Count: > 0 } ||
            PrimaryPurposeIds is { Count: > 0 } ||
            CaliberIds is { Count: > 0 };
    }
}
