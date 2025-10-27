using Ettad.Application.Common.Interfaces;

namespace Ettad.Application.Common.Models;



public class EmailConfiguration : ISettingsConfiguration
{
    public int Port { get; set; }
    public bool SSL { get; set; }
    public bool DisableAuthentication { get; set; }
    public string HostIp { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}

public class SMSConfiguration : ISettingsConfiguration
{
    public string SmsType { get; set; } = string.Empty;

    //email
    public bool EmailSSL { get; set; }
    public string EmailPort { get; set; } = string.Empty;
    public string EmailServer { get; set; } = string.Empty;
    public string EmailSubject { get; set; } = string.Empty;
    public string EmailUsername { get; set; } = string.Empty;
    public string EmailPassword { get; set; } = string.Empty;
    public string EmailToAddress { get; set; } = string.Empty;

    //web
    public string WebURL { get; set; } = string.Empty;

    //database
    public string DatabaseQuery { get; set; } = string.Empty;
    public string DatabaseConnection { get; set; } = string.Empty;
}

public class LdapConfiguration : ISettingsConfiguration
{
    public bool IsActive { get; set; }
    public bool SkipLogin { get; set; }
    public string LdapServer { get; set; } = string.Empty;
    public string LdapDomain { get; set; } = string.Empty;
    public string LdapEmpAttr { get; set; } = string.Empty;
    public string LdapPassword { get; set; } = string.Empty;
    public string LdapUsername { get; set; } = string.Empty;
}

public class ApplicationConfiguration : ISettingsConfiguration
{
    public bool EnableDefaultShift { get; set; }
    public bool EnableDefaultPolicy { get; set; }
    public int AttendanceJobDuration { get; set; }
    //public HomePageType? DefaultHomePage { get; set; }
    //public PunchMissType? DefaultPunchMiss { get; set; }
    //public PunchTypeBased? DefaultPunchTypeBasedOn { get; set; }
    public int? CustomPeriod { get; set; }
    public int MaintainSystemLogsDuration { get; set; }
    public int MaximumReportingManagerLevel { get; set; }
    public string SecurityAdminEmailAddress { get; set; } = string.Empty;
    public string SecurityAdminMobileNumber { get; set; } = string.Empty;
    public bool EnableBiometricDeviceUserIntegration { get; set; }
   // public BiometricDeviceType BiometricDeviceType { get; set; }
}

public class MessageTemplateConfiguration : ISettingsConfiguration
{
    public bool SmsEnabled { get; set; }
    public bool EmailEnabled { get; set; }
    public string MessageEvent { get; set; } = string.Empty;
    public string SmsMessageTemplate { get; set; } = string.Empty;
    public string EmailMessageTemplate { get; set; } = string.Empty;
    public string EmailSubjectTemplate { get; set; } = string.Empty;
}

public class AuthOTPConfiguration : ISettingsConfiguration
{
    public bool Status { get; set; }
    public string Validity { get; set; } = string.Empty;
    public string RetrySeconds { get; set; } = string.Empty;
}

public class CMSTemplate : ISettingsConfiguration
{
    public string PageName { get; set; } = string.Empty;
    public string PageContent { get; set; } = string.Empty;
}

public class DashboardSettings : ISettingsConfiguration
{
   // public LeaveBaseInfo LeaveBaseInfo { get; set; }
}

public class ReportSettings : ISettingsConfiguration
{
    public string LateColor { get; set; } = string.Empty;
    public string EarlyColor { get; set; } = string.Empty;
    public string DayStatusColor { get; set; } = string.Empty;
}

public class EmployeeSettings : ISettingsConfiguration
{
    public bool IsEnabledSection { get; set; }
    public bool IsMandatorySection { get; set; }
    public bool IsEnabledGrade { get; set; }
    public bool IsMandatoryGrade { get; set; }
    public bool IsEnabledJob { get; set; }
    public bool IsMandatoryJob { get; set; }
    public bool IsEnabledFamily { get; set; }
    public bool IsMandatoryFamily { get; set; }
    public bool IsEnabledProject { get; set; }
    public bool IsMandatoryProject { get; set; }
    public bool IsEnabledCostCenter { get; set; }
    public bool IsMandatoryCostCenter { get; set; }
    public bool IsEnabledTeam { get; set; }
    public bool IsMandatoryTeam { get; set; }
}


