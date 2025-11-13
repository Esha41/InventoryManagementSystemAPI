using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using Ettad.Data.Enums;
using Ettad.CrossCutting.Comman.Models.Identity;
//using Ettad.Application.Common.DTOs.Identity;
//using Ettad.Infrastructure.Enums;
namespace Ettad.EntityFramework.Utiliies
{

 

    public static class PlainPermissionsGenerator
    {
        public const string Dashboard = "Dashboard";
        public const string SystemFeatures = "System Features";

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
