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
        Supply = 5,
        Return = 6,
        Weapon = 7,
        Explosive = 8,
        Asset = 9,
        AssetSupply = 10
       
        // Add other entities here as needed
    }
}


