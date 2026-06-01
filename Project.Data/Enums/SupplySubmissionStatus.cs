namespace Ettad.Data.Enums
{
    /// <summary>
    /// Represents the submission status of a Supply
    /// </summary>
    public enum SupplySubmissionStatus
    {
        /// <summary>
        /// Supply is in draft state, can be modified
        /// </summary>
        Draft = 1,
        
        /// <summary>
        /// Supply has been submitted and cannot be modified
        /// </summary>
        Submitted = 2
    }
}
