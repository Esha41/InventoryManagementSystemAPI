using System.ComponentModel;

namespace Ettad.Data.Enums
{
    public enum PlainPermissions
    {
        [Category("Dashboard")]
        dashboard_view,

        [Category("Dashboard")]
        InventoryDashboard,

        [Category("Dashboard")]
        Forecast_view,

        [Category("System Features")]
        CanChangePassword,

        [Category("System Features")]
        CanGenerateReport,

        [Category("System Features")]
        CanImportData,

        [Category("System Features")]
        EmailLogs,

        [Category("System Features")]
        AdminImportExport,

        [Category("System Features")]
        StockNotificationSettingsPage,

        [Category("System Features")]
        DelegationManagement,

        [Category("Reports")]
        InventorySummaryReportPage,

        [Category("Reports")]
        LowStockReportPage,

        [Category("Reports")]
        ExpiringLotsReportPage,

        [Category("Workflow")]
        UpdateRequestAndSuggestLots,

        [Category("Workflow")]
        UpdateRequestAndSupply,

        [Category("Workflow")]
        SubmitSupply,

        [Category("Workflow")]
        SetSupplyPickupDate,

        [Category("Workflow")]
        ConfirmSupplyPickupDate,

        [Category("Workflow")]
        ViewSupplyDate,

        [Category("Workflow")]
        ViewWorkflowSupplySummary,

        [Category("Workflow")]
        CannotRejectRequest,

        [Category("Workflow")]
        ReviewWeaponSupply,

        [Category("Workflow")]
        SelectDepots,

        [Category("Workflow")]
        UpdateRequestItems,

        [Category("Workflow")]
        OrderIncreaseQuantity,

        [Category("Workflow")]
        OrderDecreaseQuantity,

        [Category("Workflow")]
        SetReturnDepot,

        [Category("Workflow")]
        SetReturnDeliveryDate,

        [Category("Workflow")]
        ProcessReturnItems,

        [Category("Inventory")]
        AllowanceItemViewAllDepartments,

        [Category("BITool")]
        ReportDesigner,

        [Category("BITool")]
        ReportDashboard,

        [Category("BITool")]
        ScheduledReports,

        [Category("Inventory")]
        WarehouseMapView,
    }
}
