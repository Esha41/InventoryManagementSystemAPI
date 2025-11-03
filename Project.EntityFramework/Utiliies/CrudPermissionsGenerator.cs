namespace Ettad.Infrastructure.Utilities;

using System.ComponentModel;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Ettad.Application.Common.Models;
using Ettad.CrossCutting.Comman.Models.Identity;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Infrastructure.Enums;

public class CrudPermissionsGenerator
{

    private readonly ApplicationDbContext _context;

    public const string Dashboard = "Dashboard";
    public const string Requests = "Request Management";
    public const string Inventory = "Inventory Management";
    public const string UserManagement = "User Management";
    public const string Reports = "Reports & Analytics";


    public CrudPermissionsGenerator( ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CrudPermissions>> GenerateAllPermissions()
    {
       // var reports = new List<ReportItem>();
        var permissions = new List<CrudPermissions>();
        var employeeSettings = new EmployeeSettings();
        var fields = typeof(MainEntities).GetFields();
        //if (await _context.TableExistsAsync("Reports"))
        //{
        //    reports = await _context.Reports
        //        .AsNoTracking()
        //        .Where(item => item.IsPublic)
        //        .Select(item => new ReportItem()
        //        {
        //            Name = item.Name,
        //            DisplayName = item.DisplayName,
        //        })
        //        .ToListAsync();
        //}
        //if (await _context.TableExistsAsync("Settings"))
        //{
        //    employeeSettings = await _settings.GetEmployeeSettings();
        //}
        foreach (var field in fields)
        {
          var isEnabled = true;
        //    switch (field.Name.ToUpper())
        //    {
        //        case "JOBS":
        //            isEnabled = employeeSettings.IsEnabledJob;
        //            break;
        //        case "TEAMS":
        //            isEnabled = employeeSettings.IsEnabledTeam;
        //            break;
        //        case "GRADES":
        //            isEnabled = employeeSettings.IsEnabledGrade;
        //            break;
        //        case "FAMILIES":
        //            isEnabled = employeeSettings.IsEnabledFamily;
        //            break;
        //        case "PROJECTS":
        //            isEnabled = employeeSettings.IsEnabledProject;
        //            break;
        //        case "SECTIONS":
        //            isEnabled = employeeSettings.IsEnabledSection;
        //            break;
        //        case "COSTCENTERS":
        //            isEnabled = employeeSettings.IsEnabledCostCenter;
        //            break;
        //    }
            if (isEnabled)
            {
                permissions.Add(new CrudPermissions()
                {
                    EntityName = field.Name,
                    Category = field.GetCustomAttribute<CategoryAttribute>().Category,
                    PermissionsList = GeneratePermissionsList(field.Name, field.GetValue(null) as CrudOperation[]).Select(item => new CheckBox()
                    {
                        DisplayValue = item
                    }).ToList()
                });
            }
        }
 
        return permissions;
    }

    //private static List<string?> GeneratePermissionsList(string entityName, CrudOperation[] operations)
    //{
    //    return new List<string?>
    //    {
    //        operations.Contains(CrudOperation.Page) ? $"Permissions.{entityName}.Page" : null,
    //        operations.Contains(CrudOperation.View) ? $"Permissions.{entityName}.View" : null,
    //        operations.Contains(CrudOperation.Create) ? $"Permissions.{entityName}.Create" : null,
    //        operations.Contains(CrudOperation.Edit) ? $"Permissions.{entityName}.Edit" : null,
    //        operations.Contains(CrudOperation.Delete) ? $"Permissions.{entityName}.Delete" : null,
    //    };
    //}
    private static List<string> GeneratePermissionsList(string entityName, CrudOperation[]? operations)
    {
        var list = new List<string>();

        if (operations == null || operations.Length == 0)
            return list;

        if (operations.Contains(CrudOperation.Page))
            list.Add($"Permissions.{entityName}.Page");
        if (operations.Contains(CrudOperation.View))
            list.Add($"Permissions.{entityName}.View");
        if (operations.Contains(CrudOperation.Create))
            list.Add($"Permissions.{entityName}.Create");
        if (operations.Contains(CrudOperation.Edit))
            list.Add($"Permissions.{entityName}.Edit");
        if (operations.Contains(CrudOperation.Delete))
            list.Add($"Permissions.{entityName}.Delete");

        return list;
    }

}
