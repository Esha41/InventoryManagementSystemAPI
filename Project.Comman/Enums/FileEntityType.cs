namespace Ettad.Comman.Enums
{
    /// <summary>
    /// Enum to identify which domain entity a file belongs to.
    /// Extend this enum as needed (Item, Request, Order, etc.).
    /// </summary>
    public enum FileEntityType
    {
        Ammunition = 1,
        Order = 2,
        Workflow = 3,
        WorkflowApproval = 4,
        Supply = 5
       
        // Add other entities here (e.g. Request = 6, ...)
    }
}


