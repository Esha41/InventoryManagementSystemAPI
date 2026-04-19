using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.Batches.Dtos
{
    /// <summary>Localized status strings for batch Excel dropdowns and parsing.</summary>
    public static class BatchAssetExcelStatusLabels
    {
        private static readonly IReadOnlyList<AssetStatus> Ordered = new[]
        {
            AssetStatus.ReadyToIssue,
            AssetStatus.NotReadyToIssue,
            AssetStatus.InMaintenance,
            AssetStatus.UnserviceableRepairable,
            AssetStatus.UnserviceableUnrepairable,
            AssetStatus.AwaitingDisposal,
            AssetStatus.Disposed
        };

        public static IReadOnlyList<string> GetLabelsForLanguage(string language)
        {
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            return isAr ? LabelsAr : LabelsEn;
        }

        public static string ToLabel(AssetStatus? status, string language)
        {
            if (!status.HasValue)
                return "";
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            return isAr ? ToLabelAr(status.Value) : ToLabelEn(status.Value);
        }

        public static bool TryParse(string? label, string language, out AssetStatus? status)
        {
            status = null;
            if (string.IsNullOrWhiteSpace(label))
                return true;
            var t = label.Trim();
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            foreach (var s in Ordered)
            {
                var en = ToLabelEn(s);
                var ar = ToLabelAr(s);
                if (string.Equals(t, en, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(t, ar, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(t, s.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    status = s;
                    return true;
                }
            }
            return false;
        }

        private static string ToLabelEn(AssetStatus s) => s switch
        {
            AssetStatus.ReadyToIssue => "Ready to Issue",
            AssetStatus.NotReadyToIssue => "Not Ready to Issue",
            AssetStatus.InMaintenance => "In Maintenance",
            AssetStatus.UnserviceableRepairable => "Unserviceable (Repairable)",
            AssetStatus.UnserviceableUnrepairable => "Unserviceable (Unrepairable)",
            AssetStatus.AwaitingDisposal => "Awaiting Disposal",
            AssetStatus.Disposed => "Disposed",
            _ => s.ToString()
        };

        private static string ToLabelAr(AssetStatus s) => s switch
        {
            AssetStatus.ReadyToIssue => "جاهز للصرف",
            AssetStatus.NotReadyToIssue => "غير جاهز للصرف",
            AssetStatus.InMaintenance => "قيد الصيانة",
            AssetStatus.UnserviceableRepairable => "غير صالح (قابل للإصلاح)",
            AssetStatus.UnserviceableUnrepairable => "غير صالح (غير قابل للإصلاح)",
            AssetStatus.AwaitingDisposal => "في انتظار الإتلاف",
            AssetStatus.Disposed => "تم التخلص منه",
            _ => s.ToString()
        };

        private static readonly IReadOnlyList<string> LabelsEn = Ordered.Select(ToLabelEn).ToList();
        private static readonly IReadOnlyList<string> LabelsAr = Ordered.Select(ToLabelAr).ToList();
    }
}
