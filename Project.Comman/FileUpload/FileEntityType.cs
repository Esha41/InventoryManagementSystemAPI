namespace Ettad.CrossCutting.Comman.FileUpload
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
        AssetSupply = 10,
        ReturnTrackingLine = 11,
        HelpCenter = 12,
        /// <summary>User manual PDFs/docs (singleton: use entityId = 1 for all manual files).</summary>
        HelpCenterUserManual = 13,
        Accessory = 14
        // Add other entities here as needed
    }
}


