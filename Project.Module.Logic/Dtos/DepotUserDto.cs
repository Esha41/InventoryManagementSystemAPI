namespace Ettad.Module.lookup.Dtos
{
    /// <summary>
    /// DTO for a user assigned to a depot.
    /// </summary>
    public class DepotUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? FullNameEn { get; set; }
        public string? FullNameAr { get; set; }
    }
}
