namespace Ettad.ReportManagement.Service.Dtos
{
    /// <summary>
    /// Rows returned by <c>dbo.GetAuditorPendingApprovals</c>, mapped for DevExpress bindings
    /// (same field names as the report expects from the SqlDataSource query).
    /// </summary>
    public sealed class AuditorPendingApprovalReportRow
    {
        public long RequestId { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public string Priority { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime PendingFrom { get; set; }
        public string? PreviousApprover { get; set; }
        public string? PendingBy { get; set; }
        public string? NextApprover { get; set; }
        public string? NextApproverUserEmails { get; set; }
    }
}
