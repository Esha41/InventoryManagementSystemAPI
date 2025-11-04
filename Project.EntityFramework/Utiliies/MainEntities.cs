namespace Ettad.Infrastructure.Enums;
using Ettad.Infrastructure.Utilities;
using System.ComponentModel;

public enum CrudOperation
{
    Page,
    View,
    Create,
    Edit,
    Delete,
}

public static class MainEntities
{
    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] Departments = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] Propellants = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] Units = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] ProjectailMaterials = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] NatureOptions = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] Nsns = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] PrimaryPurposes = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] Manufacturers = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] Hccs = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] Depots = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] HazardDivisions = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] Countries = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] CaseTypes = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] Compatibilities = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] Colors = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.OrganizationSettings)]
    public static readonly CrudOperation[] Supplier = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete,
    };

    [Category(CrudPermissionsGenerator.EmployeeData)]
    public static readonly CrudOperation[] Employees = new[]
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
    public static readonly CrudOperation[] AzureSetup = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Edit
    };

    [Category(CrudPermissionsGenerator.SettingsSupport)]
    public static readonly CrudOperation[] AuditLogs = new[]
    {
        CrudOperation.Page,
        CrudOperation.View
    };

    [Category(CrudPermissionsGenerator.SettingsSupport)]
    public static readonly CrudOperation[] Translations = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Edit
    };

    [Category(CrudPermissionsGenerator.SettingsSupport)]
    public static readonly CrudOperation[] ApplicationSettings = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Edit
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

    // ==============================
    // DASHBOARD
    // ==============================
    [Category(CrudPermissionsGenerator.Dashboard)]
    public static readonly CrudOperation[] Dashboard = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Dashboard)]
    public static readonly CrudOperation[] NotificationsPage = new[]
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
    public static readonly CrudOperation[] ViewRequest = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Requests)]
    public static readonly CrudOperation[] EditRequestsComments = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Requests)]
    public static readonly CrudOperation[] ModifyRequestDetails = new[]
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

    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] AddNewAssetPage = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] AssetSelection = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };

    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] AssetDeliveryDetails = new[]
    {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };
}
