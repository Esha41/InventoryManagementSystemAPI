namespace Ettad.Data.Entities
{
    /// <summary>
    /// Singleton row (Id = 1): support email and phone shown on the user Help Center Contact tab.
    /// </summary>
    public class HelpCenterContactDisplaySettings
    {
        public long Id { get; set; }

        public string SupportEmail { get; set; } = string.Empty;

        public string SupportPhone { get; set; } = string.Empty;

        public DateTime? ModifiedAt { get; set; }

        public string? ModifiedBy { get; set; }
    }
}
