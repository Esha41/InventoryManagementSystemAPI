namespace Ettad.Inventory.Service.Batches.Dtos
{
    /// <summary>
    /// Excel header → property on <see cref="BatchAssetExcelImportRowDto"/>.
    /// Hidden columns <c>_AssetId</c> / <c>_ItemId</c> support round-trip export; users work from dropdown columns only.
    /// </summary>
    public static class BatchAssetExcelColumnMappings
    {
        /// <summary>Ordered headers for export row 1 (hidden cols first). Includes technical + localized user headers.</summary>
        public static IReadOnlyList<string> GetExportHeaders(string language)
        {
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            return isAr ? ExportHeaderOrderAr : ExportHeaderOrderEn;
        }

        public static Dictionary<string, string> Get(string language)
        {
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            var m = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Hidden round-trip columns (same keys EN/AR)
            m["_AssetId"] = nameof(BatchAssetExcelImportRowDto.AssetId);
            m["_ItemId"] = nameof(BatchAssetExcelImportRowDto.ItemId);

            if (isAr)
            {
                m["اسم الصنف"] = nameof(BatchAssetExcelImportRowDto.ItemName);
                m["رقم التسلسل"] = nameof(BatchAssetExcelImportRowDto.SerialNumber);
                m["RFID"] = nameof(BatchAssetExcelImportRowDto.RFID);
                m["الحالة التشغيلية"] = nameof(BatchAssetExcelImportRowDto.StatusLabel);
                m["تاريخ الشراء"] = nameof(BatchAssetExcelImportRowDto.PurchaseDate);
                m["تاريخ انتهاء الضمان"] = nameof(BatchAssetExcelImportRowDto.WarrantyExpiryDate);
                m["سعر الشراء"] = nameof(BatchAssetExcelImportRowDto.PurchasePrice);
                m["إيصال التسليم"] = nameof(BatchAssetExcelImportRowDto.DeliveryReceipt);
                m["ملاحظات"] = nameof(BatchAssetExcelImportRowDto.Notes);
                m["وضع التعيين"] = nameof(BatchAssetExcelImportRowDto.AssignmentModeLabel);
                m["القسم"] = nameof(BatchAssetExcelImportRowDto.AssignmentDepartment);
                m["الموظف"] = nameof(BatchAssetExcelImportRowDto.AssignmentEmployee);
                m["ملاحظات التخصيص"] = nameof(BatchAssetExcelImportRowDto.AssignmentNotes);
            }
            else
            {
                m["Item Name"] = nameof(BatchAssetExcelImportRowDto.ItemName);
                m["Serial Number"] = nameof(BatchAssetExcelImportRowDto.SerialNumber);
                m["RFID"] = nameof(BatchAssetExcelImportRowDto.RFID);
                m["Status"] = nameof(BatchAssetExcelImportRowDto.StatusLabel);
                m["Purchase Date"] = nameof(BatchAssetExcelImportRowDto.PurchaseDate);
                m["Warranty Expiry Date"] = nameof(BatchAssetExcelImportRowDto.WarrantyExpiryDate);
                m["Purchase Price"] = nameof(BatchAssetExcelImportRowDto.PurchasePrice);
                m["Delivery Receipt"] = nameof(BatchAssetExcelImportRowDto.DeliveryReceipt);
                m["Notes"] = nameof(BatchAssetExcelImportRowDto.Notes);
                m["Assignment Mode"] = nameof(BatchAssetExcelImportRowDto.AssignmentModeLabel);
                m["Department"] = nameof(BatchAssetExcelImportRowDto.AssignmentDepartment);
                m["Employee"] = nameof(BatchAssetExcelImportRowDto.AssignmentEmployee);
                m["Assignment Notes"] = nameof(BatchAssetExcelImportRowDto.AssignmentNotes);
            }

            // Legacy files (pre-dropdown template)
            m["Update Assignment"] = nameof(BatchAssetExcelImportRowDto.AssignmentModeLabel);
            m["تحديث التخصيص"] = nameof(BatchAssetExcelImportRowDto.AssignmentModeLabel);
            m["Asset ID"] = nameof(BatchAssetExcelImportRowDto.AssetId);
            m["Item ID"] = nameof(BatchAssetExcelImportRowDto.ItemId);
            m["معرف الأصل"] = nameof(BatchAssetExcelImportRowDto.AssetId);
            m["معرف الصنف"] = nameof(BatchAssetExcelImportRowDto.ItemId);
            m["Item No"] = nameof(BatchAssetExcelImportRowDto.ItemNo);
            m["رقم الصنف"] = nameof(BatchAssetExcelImportRowDto.ItemNo);
            m["الوضع"] = nameof(BatchAssetExcelImportRowDto.StatusLabel);
            m["Assign To Department ID"] = nameof(BatchAssetExcelImportRowDto.AssignToDepartmentId);
            m["Assign To Employee ID"] = nameof(BatchAssetExcelImportRowDto.AssignToEmployeeId);
            m["معرف القسم"] = nameof(BatchAssetExcelImportRowDto.AssignToDepartmentId);
            m["معرف الموظف"] = nameof(BatchAssetExcelImportRowDto.AssignToEmployeeId);
            m["Department Name"] = nameof(BatchAssetExcelImportRowDto.AssignmentDepartment);
            m["Employee Name"] = nameof(BatchAssetExcelImportRowDto.AssignmentEmployee);
            m["اسم القسم"] = nameof(BatchAssetExcelImportRowDto.AssignmentDepartment);
            m["اسم الموظف"] = nameof(BatchAssetExcelImportRowDto.AssignmentEmployee);

            return m;
        }

        public static IReadOnlyList<string> ExportHeaderOrderEn => new[]
        {
            "_AssetId", "_ItemId",
            "Item Name", "Serial Number", "RFID", "Status",
            "Purchase Date", "Warranty Expiry Date", "Purchase Price", "Delivery Receipt", "Notes",
            "Assignment Mode", "Department", "Employee", "Assignment Notes"
        };

        public static IReadOnlyList<string> ExportHeaderOrderAr => new[]
        {
            "_AssetId", "_ItemId",
            "اسم الصنف", "رقم التسلسل", "RFID", "الحالة التشغيلية",
            "تاريخ الشراء", "تاريخ انتهاء الضمان", "سعر الشراء", "إيصال التسليم", "ملاحظات",
            "وضع التعيين", "القسم", "الموظف", "ملاحظات التخصيص"
        };

        public const int HiddenColumnCount = 2;

        /// <summary>Main grid worksheet name in batch export (import must read this sheet if tabs were reordered).</summary>
        public const string WorksheetName = "Batch Assets";
    }
}
