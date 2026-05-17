using System.ComponentModel;

namespace Ettad.CrossCutting.Comman.Utilities
{
    public enum PlainPermissions
    {
        [Category(PlainPermissionsGenerator.Dashboard)]
        dashboard_view,

        [Category(PlainPermissionsGenerator.Dashboard)]
        InventoryDashboard,

        [Category(PlainPermissionsGenerator.Dashboard)]
        Forecast_view,

        [Category(PlainPermissionsGenerator.SystemFeatures)]
        CanChangePassword,

        [Category(PlainPermissionsGenerator.SystemFeatures)]
        CanGenerateReport,

        [Category(PlainPermissionsGenerator.SystemFeatures)]
        CanImportData,

        [Category(PlainPermissionsGenerator.SystemFeatures)]
        EmailLogs,

        [Category(PlainPermissionsGenerator.SystemFeatures)]
        AdminImportExport,

        [Category(PlainPermissionsGenerator.SystemFeatures)]
        StockNotificationSettingsPage,

        /// <summary>Settings: workflow steps that notify the requester when order line quantity changes.</summary>
        [Category(PlainPermissionsGenerator.SystemFeatures)]
        RequesterQtyChangeNotificationSettingsPage,

        /// <summary>Admin dashboard: view all delegations, history, and delegation system settings.</summary>
        [Category(PlainPermissionsGenerator.SystemFeatures)]
        DelegationManagement,

        [Category(PlainPermissionsGenerator.Reports)]
        InventorySummaryReportPage,

        [Category(PlainPermissionsGenerator.Reports)]
        LowStockReportPage,

        [Category(PlainPermissionsGenerator.Reports)]
        CriticalStockReportPage,

        [Category(PlainPermissionsGenerator.Reports)]
        ExpiringLotsReportPage,

        [Category(PlainPermissionsGenerator.Workflow)]
        UpdateRequestAndSuggestLots,

        [Category(PlainPermissionsGenerator.Workflow)]
        UpdateRequestAndSupply,

        [Category(PlainPermissionsGenerator.Workflow)]
        SubmitSupply,

        [Category(PlainPermissionsGenerator.Workflow)]
        SetSupplyPickupDate,

        [Category(PlainPermissionsGenerator.Workflow)]
        ConfirmSupplyPickupDate,

        [Category(PlainPermissionsGenerator.Workflow)]
        ViewSupplyDate,

        [Category(PlainPermissionsGenerator.Workflow)]
        ViewWorkflowSupplySummary,

        [Category(PlainPermissionsGenerator.Workflow)]
        CannotRejectRequest,

        /// <summary>Cancel in-flight workflow-backed requests without being the current approver.</summary>
        [Category(PlainPermissionsGenerator.Workflow)]
        CanCancelRequest,

        [Category(PlainPermissionsGenerator.Workflow)]
        ReviewWeaponSupply,

        [Category(PlainPermissionsGenerator.Workflow)]
        SelectDepots,

        [Category(PlainPermissionsGenerator.Workflow)]
        UpdateRequestItems,

        [Category(PlainPermissionsGenerator.Workflow)]
        OrderIncreaseQuantity,

        [Category(PlainPermissionsGenerator.Workflow)]
        OrderDecreaseQuantity,

        [Category(PlainPermissionsGenerator.Workflow)]
        SetReturnDepot,

        [Category(PlainPermissionsGenerator.Workflow)]
        SetReturnDeliveryDate,

        [Category(PlainPermissionsGenerator.Workflow)]
        ProcessReturnItems,

        [Category(PlainPermissionsGenerator.Inventory)]
        AllowanceItemViewAllDepartments,

        [Category(PlainPermissionsGenerator.BITool)]
        ReportDesigner,

        [Category(PlainPermissionsGenerator.BITool)]
        ReportDashboard,

        [Category(PlainPermissionsGenerator.BITool)]
        ScheduledReports,
        [Category(PlainPermissionsGenerator.Inventory)]
        WarehouseMapView,
    }

}