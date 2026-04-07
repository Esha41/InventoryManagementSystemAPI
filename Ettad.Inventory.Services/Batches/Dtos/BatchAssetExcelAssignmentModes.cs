namespace Ettad.Inventory.Service.Batches.Dtos
{
    public enum BatchAssignmentMode
    {
        NoChange,
        AssignDepartment,
        AssignEmployee,
        RemoveAssignment
    }

    /// <summary>Localized assignment-mode strings for batch Excel dropdowns and parsing.</summary>
    public static class BatchAssetExcelAssignmentModes
    {
        private static readonly IReadOnlyList<BatchAssignmentMode> Ordered = new[]
        {
            BatchAssignmentMode.NoChange,
            BatchAssignmentMode.AssignDepartment,
            BatchAssignmentMode.AssignEmployee,
            BatchAssignmentMode.RemoveAssignment
        };

        public static IReadOnlyList<string> GetLabelsForLanguage(string language)
        {
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            return isAr ? LabelsAr : LabelsEn;
        }

        public static string ToLabel(BatchAssignmentMode mode, string language)
        {
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            return isAr ? ToLabelAr(mode) : ToLabelEn(mode);
        }

        public static bool TryParse(string? label, out BatchAssignmentMode mode)
        {
            mode = BatchAssignmentMode.NoChange;
            if (string.IsNullOrWhiteSpace(label))
                return true;

            var t = label.Trim();

            // Legacy Yes/No from old templates
            if (string.Equals(t, "Yes", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(t, "y", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(t, "true", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(t, "1", StringComparison.OrdinalIgnoreCase))
            {
                // Legacy "Yes" — caller must infer mode from Department/Employee columns
                mode = BatchAssignmentMode.AssignEmployee; // placeholder; enrichment overrides
                return true;
            }
            if (string.Equals(t, "No", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(t, "n", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(t, "false", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(t, "0", StringComparison.OrdinalIgnoreCase))
            {
                mode = BatchAssignmentMode.NoChange;
                return true;
            }

            foreach (var m in Ordered)
            {
                if (string.Equals(t, ToLabelEn(m), StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(t, ToLabelAr(m), StringComparison.OrdinalIgnoreCase))
                {
                    mode = m;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines the effective assignment mode for a legacy "Yes" value
        /// by looking at which assignment columns the user filled.
        /// </summary>
        public static BatchAssignmentMode InferLegacyYesMode(bool hasDept, bool hasEmp)
        {
            if (hasEmp) return BatchAssignmentMode.AssignEmployee;
            if (hasDept) return BatchAssignmentMode.AssignDepartment;
            return BatchAssignmentMode.RemoveAssignment;
        }

        private static string ToLabelEn(BatchAssignmentMode m) => m switch
        {
            BatchAssignmentMode.NoChange => "No Change",
            BatchAssignmentMode.AssignDepartment => "Assign to Department",
            BatchAssignmentMode.AssignEmployee => "Assign to Employee",
            BatchAssignmentMode.RemoveAssignment => "Remove Assignment",
            _ => m.ToString()
        };

        private static string ToLabelAr(BatchAssignmentMode m) => m switch
        {
            BatchAssignmentMode.NoChange => "بدون تغيير",
            BatchAssignmentMode.AssignDepartment => "تعيين لقسم",
            BatchAssignmentMode.AssignEmployee => "تعيين لموظف",
            BatchAssignmentMode.RemoveAssignment => "إزالة التعيين",
            _ => m.ToString()
        };

        private static readonly IReadOnlyList<string> LabelsEn = Ordered.Select(ToLabelEn).ToList();
        private static readonly IReadOnlyList<string> LabelsAr = Ordered.Select(ToLabelAr).ToList();
    }
}
