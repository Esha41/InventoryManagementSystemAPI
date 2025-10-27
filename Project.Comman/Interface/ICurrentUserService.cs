namespace Ettad.Application.Common.Interfaces;


public interface ICurrentUserService
{
    bool IsHrRole { get; }
    string? UserId { get; }
    bool IsAdminRole { get; }
    string? UserName { get; }
    long? EmployeeId { get; }
    bool IsHrEmployee { get; }
    List<string>? Roles { get; }
    List<long> EntityIds { get; }
    bool IsTrainingEmployee { get; }
    bool IsResponsibleEmployee { get; }
    //EntityBaseInfo? EntityType { get; }
    List<long> ExtraEmployeesView { get; }
    List<long> ExtraRoleEmployeesView { get; }
    List<long> AllowedLeaveClausesIds { get; }
    List<long> AllowedPermissionClausesIds { get; }
    bool IncludeEmployeeViewInManagerDashboard { get; }

    bool IsUserHasClaim(string claimName);
    int? OrganizationId { get; }
    bool IsSuperAdmin { get; }
}
