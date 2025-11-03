//namespace Ettad.Infrastructure.Enums;

//using System.ComponentModel;
//using Ettad.Infrastructure.Utilities;

//public enum CrudOperation
//{
//    Page,
//    View,
//    Create,
//    Edit,
//    Delete,
//}

//public static class MainEntities
//{

//    [Category(CrudPermissionsGenerator.Dashboard)]
//    public static readonly CrudOperation[] Dashboard = new[]
//    {
//        CrudOperation.Page,
//        CrudOperation.View
//    };

//    [Category(CrudPermissionsGenerator.Dashboard)]
//    public static readonly CrudOperation[] Forecast = new[]
//    {
//        CrudOperation.Page,
//        CrudOperation.View
//    };

//    // ========================================
//    // REQUEST MANAGEMENT
//    // ========================================
//    [Category(CrudPermissionsGenerator.Requests)]
//    public static readonly CrudOperation[] NewIssueRequest = new[]
//    {
//        CrudOperation.Page,
//        CrudOperation.View,
//        CrudOperation.Create
//    };

//    [Category(CrudPermissionsGenerator.Requests)]
//    public static readonly CrudOperation[] ReturnRequest = new[]
//    {
//        CrudOperation.Page,
//        CrudOperation.View,
//        CrudOperation.Create
//    };

//    [Category(CrudPermissionsGenerator.Requests)]
//    public static readonly CrudOperation[] RequestsManagement = new[]
//    {
//        CrudOperation.Page,
//        CrudOperation.View,
//        CrudOperation.Create,
//        CrudOperation.Edit,
//        CrudOperation.Delete
//    };

//    // ========================================
//    // INVENTORY MANAGEMENT
//    // ========================================
//    [Category(CrudPermissionsGenerator.Inventory)]
//    public static readonly CrudOperation[] AddAsset = new[]
//    {
//        CrudOperation.Page,
//        CrudOperation.View,
//        CrudOperation.Create
//    };

//    [Category(CrudPermissionsGenerator.Inventory)]
//    public static readonly CrudOperation[] AssetList = new[]
//    {
//        CrudOperation.Page,
//        CrudOperation.View,
//        CrudOperation.Create,
//        CrudOperation.Edit,
//        CrudOperation.Delete
//    };

//    [Category(CrudPermissionsGenerator.Inventory)]
//    public static readonly CrudOperation[] SearchInventory = new[]
//    {
//        CrudOperation.Page,
//        CrudOperation.View
//    };

//    // ========================================
//    // USER MANAGEMENT
//    // ========================================
//    [Category(CrudPermissionsGenerator.UserManagement)]
//    public static readonly CrudOperation[] SystemUsers = new[]
//    {
//        CrudOperation.Page,
//        CrudOperation.View,
//        CrudOperation.Create,
//        CrudOperation.Edit,
//        CrudOperation.Delete,
//    };

//    [Category(CrudPermissionsGenerator.UserManagement)]
//    public static readonly CrudOperation[] Roles = new[]
//    {
//        CrudOperation.Page,
//        CrudOperation.View,
//        CrudOperation.Create,
//        CrudOperation.Edit,
//        CrudOperation.Delete
//    };

//    // ========================================
//    // REPORTS & ANALYTICS (Future Features)
//    // ========================================
//    [Category(CrudPermissionsGenerator.Reports)]
//    public static readonly CrudOperation[] Reports = new[]
//    {
//        CrudOperation.Page,
//        CrudOperation.View
//    };

//    [Category(CrudPermissionsGenerator.Reports)]
//    public static readonly CrudOperation[] Analytics = new[]
//    {
//        CrudOperation.Page,
//        CrudOperation.View
//    };
//}
namespace Ettad.Infrastructure.Enums;

using System.ComponentModel;
using Ettad.Infrastructure.Utilities;

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
    [Category(CrudPermissionsGenerator.Inventory)]
    public static readonly CrudOperation[] SupplyManagement = new[]
   {
        CrudOperation.Page,
        CrudOperation.View,
        CrudOperation.Create,
        CrudOperation.Edit,
        CrudOperation.Delete
    };
    // ========================================
    // USER MANAGEMENT
    // ========================================
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
    public static readonly CrudOperation[] Roles = new[]
    {
            CrudOperation.Page,
            CrudOperation.View,
            CrudOperation.Create,
            CrudOperation.Edit,
            CrudOperation.Delete
    };


}
