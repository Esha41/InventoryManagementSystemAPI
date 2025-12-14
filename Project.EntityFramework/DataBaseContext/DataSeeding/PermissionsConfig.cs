using Ettad.Data.Enums;
using Newtonsoft.Json;

namespace Ettad.EntityFramework.DataBaseContext.DataSeeding
{
    public static class PermissionConfig
    {
        public static List<string> ReadLookups = new()
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
            "Permissions.RequestPurpose.Page",
            "Permissions.RequestPurpose.View",
        };

        public static List<string> WriteLookups = new()
        {
            "Permissions.Departments.Create",
            "Permissions.Departments.Edit",
            "Permissions.Departments.Delete",
            "Permissions.Propellants.Create",
            "Permissions.Propellants.Edit",
            "Permissions.Propellants.Delete",
            "Permissions.Units.Create",
            "Permissions.Units.Edit",
            "Permissions.Units.Delete",
            "Permissions.ProjectailMaterials.Create",
            "Permissions.ProjectailMaterials.Edit",
            "Permissions.ProjectailMaterials.Delete",
            "Permissions.NatureOptions.Create",
            "Permissions.NatureOptions.Edit",
            "Permissions.NatureOptions.Delete",
            "Permissions.PrimaryPurposes.Create",
            "Permissions.PrimaryPurposes.Edit",
            "Permissions.PrimaryPurposes.Delete",
            "Permissions.Manufacturers.Create",
            "Permissions.Manufacturers.Edit",
            "Permissions.Manufacturers.Delete",
            "Permissions.Hccs.Create",
            "Permissions.Hccs.Edit",
            "Permissions.Hccs.Delete",
            "Permissions.Depots.Create",
            "Permissions.Depots.Edit",
            "Permissions.Depots.Delete",
            "Permissions.HazardDivisions.Create",
            "Permissions.HazardDivisions.Edit",
            "Permissions.HazardDivisions.Delete",
            "Permissions.Countries.Create",
            "Permissions.Countries.Edit",
            "Permissions.Countries.Delete",
            "Permissions.CaseTypes.Create",
            "Permissions.CaseTypes.Edit",
            "Permissions.CaseTypes.Delete",
            "Permissions.Compatibilities.Create",
            "Permissions.Compatibilities.Edit",
            "Permissions.Compatibilities.Delete",
            "Permissions.Colors.Create",
            "Permissions.Colors.Edit",
            "Permissions.Colors.Delete",
            "Permissions.Supplier.Create",
            "Permissions.Supplier.Edit",
            "Permissions.Supplier.Delete",
            "Permissions.RequestPurpose.Create",
            "Permissions.RequestPurpose.Edit",
            "Permissions.RequestPurpose.Delete",
        };

        public static List<string> ReadDepo = new()
        {
            "Permissions.Depots.Page",
            "Permissions.Depots.View",
        };

        public static List<string> WriteDepo = new()
        {
            "Permissions.Depots.Create",
            "Permissions.Depots.Edit",
            "Permissions.Depots.Delete",
        };

        public static List<string> ReadItems = new()
        {
            "Permissions.Ammunition.Page",
            "Permissions.Ammunition.View",
            "Permissions.Weapon.Page",
            "Permissions.Weapon.View",
            "Permissions.Explosive.Page",
            "Permissions.Explosive.View",
        };

        public static List<string> ReadItemsViewOnly = new()
        {
            "Permissions.Ammunition.View",
            "Permissions.Weapon.View",
            "Permissions.Explosive.View",
        };

        public static List<string> WriteItems = new()
        {
            "Permissions.Ammunition.Create",
            "Permissions.Ammunition.Edit",
            "Permissions.Ammunition.Delete",
            "Permissions.Weapon.Create",
            "Permissions.Weapon.Edit",
            "Permissions.Weapon.Delete",
            "Permissions.Explosive.Create",
            "Permissions.Explosive.Edit",
            "Permissions.Explosive.Delete",
        };

        public static List<string> ReadInventory = new()
        {
            "Permissions.Inventory.Page",
            "Permissions.Inventory.View",
        };

        public static List<string> WriteInventory = new()
        {
            "Permissions.Inventory.Create",
            "Permissions.Inventory.Edit",
            "Permissions.Inventory.Delete",
        };

        public static List<string> ReadAllowanceItem = new()
        {
            "Permissions.AllowanceItem.Page",
            "Permissions.AllowanceItem.View",
        };

        public static List<string> Notifications = new()
        {
            "Permissions.NotificationsPage.Page",
            "Permissions.NotificationsPage.View",
            "Permissions.NotificationsPage.Edit",
        };

        public static List<string> Dashboard = new()
        {
            "dashboard_view"
        };

        public static List<string> Forecast = new()
        {
            "Forecast_view"
        };
        public static List<string> ReadOrder = new()
        {
            "Permissions.Order.Page",
            "Permissions.Order.View",
        };
        public static List<string> ReadDiscard = new()
        {
            "Permissions.Discard.Page",
            "Permissions.Discard.View",
        };
        public static List<string> ReadReturn = new()
        {
            "Permissions.Return.Page",
            "Permissions.Return.View",
        };
        public static List<string> ReadOrderDiscardReturn = new()
        {
            "Permissions.Discard.Page",
            "Permissions.Discard.View",

            "Permissions.Return.Page",
            "Permissions.Return.View",

            "Permissions.Order.Page",
            "Permissions.Order.View",
        };

        public static List<string> WriteOrder = new()
        {
            "Permissions.Order.Page",
            "Permissions.Order.View",
            "Permissions.Order.Create",
            "Permissions.Order.Edit",
            "Permissions.Order.Delete",
        };
        public static List<string> WriteDiscard = new()
        {
            "Permissions.Discard.Page",
            "Permissions.Discard.View",
            "Permissions.Discard.Create",
            "Permissions.Discard.Edit",
        };
        public static List<string> WriteReturn = new()
        {
            "Permissions.Return.Page",
            "Permissions.Return.View",
            "Permissions.Return.Create",
            "Permissions.Return.Edit",
        };

        public static List<string> WriteOrderDiscardReturn = new()
        {
            "Permissions.Discard.Page",
            "Permissions.Discard.View",
            "Permissions.Discard.Create",
            "Permissions.Discard.Edit",

            "Permissions.Return.Page",
            "Permissions.Return.View",
            "Permissions.Return.Create",
            "Permissions.Return.Edit",

            "Permissions.Order.Page",
            "Permissions.Order.View",
            "Permissions.Order.Create",
            "Permissions.Order.Edit",
            "Permissions.Order.Delete",
        };

        public static List<string> ReadRequestReciever = new()
        {
            "Permissions.RequestReciever.Page",
            "Permissions.RequestReciever.View",
        };  

        public static List<string> WriteRequestReciever = new()
        {
            "Permissions.RequestReciever.Create",
            "Permissions.RequestReciever.Edit",
        };

        public static List<string> ReadSupply = new()
        {
            "Permissions.Supply.View",
            "Permissions.Supply.Page",
        };

        public static List<string> WriteSupply = new()
        {
            "Permissions.Supply.Create",
            "Permissions.Supply.Edit",
            "Permissions.Supply.Delete",
        };

        public static List<string> Rank = new()
        {
            "Permissions.Rank.View",
            "Permissions.Rank.Page",
            "Permissions.Rank.Create",
            "Permissions.Rank.Edit",
            "Permissions.Rank.Delete",
        };
        public static List<string> WareHouse = new()
        {
            "Permissions.WarehousePage.View",
            "Permissions.WarehousePage.Page",
            "Permissions.WarehousePage.Create",
            "Permissions.WarehousePage.Edit",
            "Permissions.WarehousePage.Delete",
        };
       
        public static List<string> RequestReciever =
            ReadRequestReciever
                .Concat(WriteRequestReciever)
                .ToList();


        #region Actual grouping for roles baesd on application entities

        #region Order Requesting Entity

        public static List<string> Requester_OrderRequestingEntity =
            ReadLookups
                .Concat(ReadAllowanceItem)
                .Concat(ReadItemsViewOnly)
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)                
                .Concat(WriteOrderDiscardReturn)
                .Concat(ReadRequestReciever)
                .ToList();

        public static List<string> SupplyOfficer_OrderRequestingEntity =
            ReadLookups
                .Concat(ReadAllowanceItem)               
                .Concat(ReadItems)
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        public static List<string> RequestingEntityCommander_OrderRequestingEntity = 
            ReadLookups
                .Concat(ReadAllowanceItem)               
                .Concat(ReadItems)
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        #endregion

        #region Military Training Entity

        public static List<string> MilitaryTrainingAuditor_MilitaryTrainingEntity =
            ReadLookups
                .Concat(Dashboard)               
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        public static List<string> MilitaryTrainingOfficer_MilitaryTrainingEntity =
            ReadLookups               
                .Concat(Dashboard)
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        public static List<string> HeadOfMiltaryTraining_MilitaryTrainingEntity =
            ReadLookups
                .Concat(Dashboard)               
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        #endregion

        #region Directorate of Ammunition entity

        public static List<string> Auditor_DirectorateOfAmmunitionEntity =
            ReadLookups
                .Concat(ReadItems)
                .Concat(ReadAllowanceItem)                
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)               
                .Concat(WriteOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        public static List<string> HeadOfDivision_DirectorateOfAmmunitionEntity =
            ReadLookups
                .Concat(ReadItems)
                .Concat(ReadAllowanceItem)
                .Concat(ReadInventory)
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)               
                .Concat(WriteOrderDiscardReturn)
                .Concat(RequestReciever)
                .Concat(ReadSupply)
                .Concat(WriteSupply)
                .Concat(new List<string> 
                    { 
                        PlainPermissions.UpdateRequestAndSuggestLots.ToString(),
                        PlainPermissions.InventoryDashboard.ToString(),
                    }
                )
                .ToList();

        public static List<string> DirectorOfArmament_DirectorateOfAmmunitionEntity =
            ReadLookups
                .Concat(ReadItems)
                .Concat(ReadAllowanceItem)                
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(WriteOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        public static List<string> HeadOfLogistics_DirectorateOfAmmunitionEntity =
            ReadLookups
                .Concat(ReadItems)
                .Concat(ReadAllowanceItem)
                .Concat(ReadInventory)
                .Concat(Dashboard)
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(WriteReturn)
                .Concat(WriteDiscard)
                .Concat(RequestReciever)
                .Concat(ReadSupply)
                .Concat(new List<string>
                    {
                        PlainPermissions.InventoryDashboard.ToString(),
                    }
                )
                .ToList();

        #endregion

        #region Military Operations Entity

        public static List<string> Auditor_MilitaryOperationsEntity =
            ReadLookups
                .Concat(Dashboard)
                .Concat(ReadItems)
                .Concat(ReadAllowanceItem)
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(WriteOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        public static List<string> Officer_MilitaryOperationsEntity =
            ReadLookups
                .Concat(Dashboard)
                .Concat(ReadItems)
                .Concat(ReadAllowanceItem)
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(WriteOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        public static List<string> HeadOfMilitaryOperations_MilitaryOperationsEntity =
            ReadLookups
                .Concat(ReadItems)
                .Concat(ReadAllowanceItem)              
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        #endregion

        #region Chief of Staff (CoS) Office Entity

        public static List<string> Auditor_ChiefOfStaffOfficeEntity =
            ReadLookups
                .Concat(ReadItems)
                .Concat(ReadAllowanceItem)               
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        public static List<string> AuditorOfDeputy_ChiefOfStaffOfficeEntity =
            ReadLookups
                .Concat(ReadItems)
                .Concat(ReadAllowanceItem)
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        public static List<string> DeputyChiefOfStaff_ChiefOfStaffOfficeEntity =
            ReadLookups
                .Concat(ReadItems)
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        public static List<string> ChiefOfStaff_ChiefOfStaffOfficeEntity =
            ReadLookups
                .Concat(ReadItems)                           
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .ToList();

        #endregion

        #region Inventory Entity

        public static List<string> AuditorOfAuditDepo_InventoryEntity =
            ReadLookups
                .Concat(WriteLookups)
                .Concat(ReadItems)
                .Concat(WriteItems)
                .Concat(ReadInventory)
                .Concat(WriteInventory)
                .Concat(ReadAllowanceItem)               
                .Concat(ReadOrderDiscardReturn)
                .Concat(WriteOrder)
                .Concat(RequestReciever)
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadSupply)
                .Concat(WriteSupply)
                .Concat(WareHouse)
                .Concat(ReadDepo)
                .Concat(new List<string>
                    {
                        PlainPermissions.UpdateRequestAndSupply.ToString(),
                        PlainPermissions.InventoryDashboard.ToString(),
                        PlainPermissions.CannotRejectRequest.ToString(),
                    }
                )
                .ToList();

        public static List<string> HeadOfAuditDepo_InventoryEntity =
            ReadLookups
                .Concat(WriteLookups)
                .Concat(ReadItems)
                .Concat(ReadInventory)
                .Concat(ReadAllowanceItem)                
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadSupply)
                .Concat(WriteSupply)
                .Concat(WareHouse)
                .Concat(ReadDepo)
                .Concat(new List<string>
                    {
                        PlainPermissions.UpdateRequestAndSupply.ToString(),
                        PlainPermissions.InventoryDashboard.ToString(),
                        PlainPermissions.CannotRejectRequest.ToString(),
                    }
                )
                .ToList();

        public static List<string> DepoCommander_InventoryEntity =
            ReadLookups
                .Concat(WriteLookups)
                .Concat(ReadItems)
                .Concat(ReadInventory)
                .Concat(ReadAllowanceItem)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadSupply)
                .Concat(WareHouse)
                .Concat(ReadDepo)
                .Concat(new List<string>
                    {
                        PlainPermissions.UpdateRequestAndSupply.ToString(),                       
                    }
                )
                .ToList();

        public static List<string> AuditorOfDepoDivision_InventoryEntity =
            ReadLookups
                .Concat(WriteLookups)
                .Concat(ReadItems)
                .Concat(WriteItems)
                .Concat(ReadInventory)
                .Concat(WriteInventory)
                .Concat(ReadAllowanceItem)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadSupply)
                .Concat(WareHouse)
                .Concat(ReadDepo)
                .Concat(new List<string>
                    {
                        PlainPermissions.SetSupplyPickupDate.ToString(),
                        PlainPermissions.InventoryDashboard.ToString(),
                        PlainPermissions.CannotRejectRequest.ToString(),
                    }
                )
                .ToList();

        public static List<string> HeadOfDepoDivision_InventoryEntity =
            ReadLookups
                .Concat(WriteLookups)
                .Concat(ReadItems)                
                .Concat(ReadInventory)
                .Concat(ReadAllowanceItem)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadSupply)
                .Concat(WareHouse)
                .Concat(ReadDepo)
                .Concat(new List<string>
                    {
                        PlainPermissions.ConfirmSupplyPickupDate.ToString(),
                        PlainPermissions.InventoryDashboard.ToString(),
                        PlainPermissions.CannotRejectRequest.ToString(),
                    }
                )
                .ToList();

        public static List<string> DepoOfficer_InventoryEntity =
            ReadLookups
                .Concat(WriteLookups)
                .Concat(ReadItems)                
                .Concat(ReadInventory)
                .Concat(ReadAllowanceItem)
                .Concat(ReadOrderDiscardReturn)
                .Concat(RequestReciever)
                .Concat(Dashboard)
                .Concat(Forecast)
                .Concat(Notifications)
                .Concat(ReadSupply)
                .Concat(WareHouse)
                .Concat(ReadDepo)
                .Concat(Rank)
                .Concat(new List<string>
                    {
                        PlainPermissions.SubmitSupply.ToString(),
                        PlainPermissions.InventoryDashboard.ToString(),
                        PlainPermissions.CannotRejectRequest.ToString(),
                    }
                )
                .ToList();

        #endregion

        #endregion
    }

    // You can later add other roles here
    public static class ManagerPermissionConfig
    {
        public static List<string> AllowedPermissions = new()
        {
        };
    }
}
