namespace Ettad.Module.lookup.Dtos
{
    /// <summary>
    /// Request DTO for setting user assignments to a depot.
    /// </summary>
    public class SetDepotUsersRequest
    {
        public List<string> UserIds { get; set; } = new();
    }
}
