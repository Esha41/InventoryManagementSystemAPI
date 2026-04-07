namespace Ettad.Inventory.Service.Assets.Dtos
{
    public static class EmployeeExcelColumnMappings
    {
        private const string RequiredSuffix = " *";

        /// <summary>Headers that get a trailing " *" on the downloadable import template (matches UI required fields).</summary>
        private static readonly string[] TemplateRequiredHeadersEn =
        {
            "Name (English)", "Military ID", "Department", "Rank"
        };

        private static readonly string[] TemplateRequiredHeadersAr =
        {
            "الاسم بالإنجليزي", "الرقم العسكري", "القسم", "الرتبة"
        };

        public static IReadOnlyList<string> GetExportHeaders(string language)
        {
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            return isAr ? ExportHeaderOrderAr : ExportHeaderOrderEn;
        }

        /// <summary>Import template row headers; required columns include a visual * suffix (import accepts both with and without).</summary>
        public static IReadOnlyList<string> GetTemplateHeaders(string language)
        {
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            var headers = isAr ? ExportHeaderOrderAr : ExportHeaderOrderEn;
            var required = new HashSet<string>(
                isAr ? TemplateRequiredHeadersAr : TemplateRequiredHeadersEn,
                StringComparer.OrdinalIgnoreCase);

            var list = new List<string>(headers.Count);
            foreach (var h in headers)
            {
                if (h.StartsWith('_'))
                {
                    list.Add(h);
                    continue;
                }

                list.Add(required.Contains(h) ? h + RequiredSuffix : h);
            }

            return list;
        }

        public static Dictionary<string, string> Get(string language)
        {
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            var m = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            m["_EmployeeId"] = nameof(EmployeeExcelImportRowDto.EmployeeId);

            if (isAr)
            {
                m["الاسم بالعربي"] = nameof(EmployeeExcelImportRowDto.NameAr);
                m["الاسم بالإنجليزي"] = nameof(EmployeeExcelImportRowDto.NameEn);
                m["الرقم العسكري"] = nameof(EmployeeExcelImportRowDto.MilitaryId);
                m["القسم"] = nameof(EmployeeExcelImportRowDto.DepartmentName);
                m["الرتبة"] = nameof(EmployeeExcelImportRowDto.RankName);
                m["الهاتف"] = nameof(EmployeeExcelImportRowDto.Phone);
                m["البريد الإلكتروني"] = nameof(EmployeeExcelImportRowDto.Email);
                m["ملاحظات"] = nameof(EmployeeExcelImportRowDto.Notes);
            }
            else
            {
                m["Name (Arabic)"] = nameof(EmployeeExcelImportRowDto.NameAr);
                m["Name (English)"] = nameof(EmployeeExcelImportRowDto.NameEn);
                m["Military ID"] = nameof(EmployeeExcelImportRowDto.MilitaryId);
                m["Department"] = nameof(EmployeeExcelImportRowDto.DepartmentName);
                m["Rank"] = nameof(EmployeeExcelImportRowDto.RankName);
                m["Phone"] = nameof(EmployeeExcelImportRowDto.Phone);
                m["Email"] = nameof(EmployeeExcelImportRowDto.Email);
                m["Notes"] = nameof(EmployeeExcelImportRowDto.Notes);
            }

            AddTemplateHeaderAliases(m, isAr);
            return m;
        }

        private static void AddTemplateHeaderAliases(Dictionary<string, string> m, bool isAr)
        {
            var required = isAr ? TemplateRequiredHeadersAr : TemplateRequiredHeadersEn;
            foreach (var h in required)
            {
                if (m.TryGetValue(h, out var prop))
                    m[h + RequiredSuffix] = prop;
            }
        }

        public static IReadOnlyList<string> ExportHeaderOrderEn => new[]
        {
            "_EmployeeId",
            "Name (Arabic)", "Name (English)", "Military ID", "Department", "Rank",
            "Phone", "Email", "Notes"
        };

        public static IReadOnlyList<string> ExportHeaderOrderAr => new[]
        {
            "_EmployeeId",
            "الاسم بالعربي", "الاسم بالإنجليزي", "الرقم العسكري", "القسم", "الرتبة",
            "الهاتف", "البريد الإلكتروني", "ملاحظات"
        };

        public const int HiddenColumnCount = 1;
        public const string WorksheetName = "Employees";
    }
}
