using System.ComponentModel;
using System.Reflection;
using Ettad.CrossCutting.Comman.Models.Identity;

namespace Ettad.CrossCutting.Comman.Utilities
{
    public static class PlainPermissionsGenerator
    {
        public const string Dashboard = "Dashboard";
        public const string SystemFeatures = "System Features";
        public const string Workflow = "Workflow";
        public const string Inventory = "Inventory";
        public const string UserManagement = "User Management";
        public const string Reports = "Reports";
        public const string BITool = "BITool";

        public static string GetCategory(this PlainPermissions source)
        {
            var fieldInfo = source.GetType().GetField(source.ToString());
            CategoryAttribute attribute = (CategoryAttribute)fieldInfo.GetCustomAttribute(typeof(CategoryAttribute), false);
            return attribute.Category;
        }

        public static List<CrudPermissions> GetPlainPermissionsWithGroup()
        {
            return Enum.GetValues(typeof(PlainPermissions)).Cast<PlainPermissions>()
                .Select(item => new
                {
                    Value = item.ToString(),
                    Category = item.GetCategory(),
                })
                .GroupBy(item => item.Category)
                .Select(item => new CrudPermissions()
                {
                    EntityName = item.Key,
                    PermissionsList = Enum.GetValues(typeof(PlainPermissions))
                        .Cast<PlainPermissions>()
                        .Where(obj => obj.GetCategory() == item.Key)
                        .Select(obj => new CheckBox()
                        {
                            DisplayValue = obj.ToString()
                        }).ToList()
                }).ToList();
        }
    }
}
