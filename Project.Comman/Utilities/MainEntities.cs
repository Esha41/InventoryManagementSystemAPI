namespace Ettad.CrossCutting.Comman.Utilities;
using System.ComponentModel;

public enum CrudOperation
{
    Page,
    View,
    ViewAll,
    Create,
    Edit,
    Delete,
}

public static class MainEntities
{
    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] Departments = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] Propellants = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] Units = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] ProjectailMaterials = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] NatureOptions = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] PrimaryPurposes = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] Manufacturers = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };


    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] Depots = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.ViewAll,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] HazardDivisions = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] Countries = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] CaseTypes = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] Compatibilities = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] Colors = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] Supplier = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] Classifications = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] ItemTypes = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] Calibers = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] ItemDepartmentAssignment = new[]
  {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] LookupTables = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.UserManagement)]
    public static readonly CrudOperation[] Roles = new[]
    {
            CrudOperation.Page,
            CrudOperation.View,
            CrudOperation.Create,
            CrudOperation.Edit,
            CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.UserManagement)]
    public static readonly CrudOperation[] SystemUsers = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };


    [Category(CrudPermissionsGenerator.UserManagement)]
    public static readonly CrudOperation[] UserDelegations = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] Ammunition = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] Weapon = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] Explosive = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] AllowanceItem = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] Inventory = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] Asset = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] AssetSupply = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] Employee = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    // ==============================
    // DASHBOARD
    // ==============================
    [Category(CrudPermissionsGenerator.Dashboard)]
    public static readonly CrudOperation[] NotificationsPage = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        //CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Dashboard)]
    public static readonly CrudOperation[] Announcements = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Dashboard)]
    public static readonly CrudOperation[] HelpCenter = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Dashboard)]
    public static readonly CrudOperation[] ForecastPage = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Dashboard)]
    public static readonly CrudOperation[] Analytics = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
    };

    [Category(CrudPermissionsGenerator.Dashboard)]
    public static readonly CrudOperation[] AdminDashboard = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
    };

    // ==============================
    // REQUEST MANAGEMENT
    // ==============================
    [Category(CrudPermissionsGenerator.Requests)]
    public static readonly CrudOperation[] NewRequest = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Requests)]
    public static readonly CrudOperation[] ReturnRequest = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Requests)]
    public static readonly CrudOperation[] Discard = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Requests)]
    public static readonly CrudOperation[] Return = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Requests)]
    public static readonly CrudOperation[] Supply = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    // ==============================
    // INVENTORY MANAGEMENT
    // ==============================
    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] InventoryPage = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] SearchInventory = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] WarehousePage = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    
    [Category(CrudPermissionsGenerator.RequestManagement)]
    public static readonly CrudOperation[] Request = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.RequestManagement)]
    public static readonly CrudOperation[] RequestReciever = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.RequestManagement)]
    public static readonly CrudOperation[] Rank = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.RequestManagement)]
    public static readonly CrudOperation[] RequestPurpose = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.RequestManagement)]
    public static readonly CrudOperation[] Order = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };
    [Category(CrudPermissionsGenerator.Workflow)]
    public static readonly CrudOperation[] Workflow = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] EmailSettings = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Edit,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] LdapSettings = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] OrderAutoRejectSettings = new[]
    {
        CrudOperation.Page,
        CrudOperation.Edit,
    };

    [Category(CrudPermissionsGenerator.General)]
    public static readonly CrudOperation[] OrderAutoReject = new[]
    {
        CrudOperation.View,
    };

}
