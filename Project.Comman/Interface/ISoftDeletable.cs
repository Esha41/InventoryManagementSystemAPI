namespace Ettad.CrossCutting.Comman.Interface
{
    /// <summary>
    /// Interface to mark entities that support soft delete functionality
    /// </summary>
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
        DateTime? DeletionDate { get; set; }
        string DeletedBy { get; set; }
    }
}
