using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.EntityFramework.DataBaseContext.DataSeeding
{
    public static class RequestorPermissionConfig
    {
        public static List<string> AllowedPermissions = new()
        {
            "Permissions.Departments.Page",
            "Permissions.Departments.View",
            "Permissions.Propellants.Page",
            "Permissions.Propellants.View",
            "Permissions.Units.Page",
            "Permissions.Units.View",
            "Permissions.ProjectailMaterials.Page",
            "Permissions.ProjectailMaterials.View",
            "Permissions.NatureOptions.Page",
            "Permissions.NatureOptions.View",
            "Permissions.PrimaryPurposes.Page",
            "Permissions.PrimaryPurposes.View",
            "Permissions.Manufacturers.Page",
            "Permissions.Manufacturers.View",
            "Permissions.Hccs.Page",
            "Permissions.Hccs.View",
            "Permissions.Depots.Page",
            "Permissions.Depots.View",
            "Permissions.HazardDivisions.Page",
            "Permissions.HazardDivisions.View",
            "Permissions.Countries.Page",
            "Permissions.Countries.View",
            "Permissions.CaseTypes.Page",
            "Permissions.CaseTypes.View",
            "Permissions.Compatibilities.Page",
            "Permissions.Compatibilities.View",
            "Permissions.Colors.Page",
            "Permissions.Colors.View",
            "Permissions.Supplier.Page",
            "Permissions.Supplier.View",
            "Permissions.Ammunition.Page",
            "Permissions.Ammunition.View",
            "Permissions.AllowanceItem.Page",
            "Permissions.AllowanceItem.View",
            "Permissions.NotificationsPage.Page",
            "Permissions.NotificationsPage.View",
            "Permissions.NewRequest.Page",
            "Permissions.NewRequest.View",
            "Permissions.NewRequest.Create",
            "Permissions.ReturnRequest.Page",
            "Permissions.ReturnRequest.View",
            "Permissions.ReturnRequest.Create",
            "Permissions.Discard.Page",
            "Permissions.Discard.View",
            "Permissions.Discard.Create",
            "Permissions.Return.Page",
            "Permissions.Return.View",
            "Permissions.Return.Create",
            "Permissions.ViewRequest.Page",
            "Permissions.ViewRequest.View",
            "Permissions.Request.Page",
            "Permissions.Request.View",
            "Permissions.Request.Create",
            "Permissions.RequestPurpose.Page",
            "Permissions.RequestPurpose.View",
            "Permissions.Order.Page",
            "Permissions.Order.View",
            "Permissions.Order.Create",
            "dashboard_view"
        };
    }

    // You can later add other roles here
    public static class ManagerPermissionConfig
    {
        public static List<string> AllowedPermissions = new()
        {
            // Manager permissions
        };
    }
}
