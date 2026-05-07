using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Ettad.Inventory.Service.Monitoring.Services
{
    public class CriticalStockEmailTemplateService
    {
        public string GenerateEmailHtmlContent(List<LowStockItemInfo> items)
        {
            var htmlContent = new StringBuilder();

            htmlContent.AppendLine(@"
                <div style='margin-top: 20px; margin-bottom: 30px; direction: rtl; text-align: right;'>
                    <h3 style='color: #8B0000; font-size: 18px; font-weight: 600; margin-bottom: 15px;'>تفاصيل المواد عند الحد الحرج للمخزون</h3>
                    <table border='1' cellpadding='8' cellspacing='0' style='border-collapse: collapse; width: 100%; direction: rtl;'>
                        <thead>
                            <tr style='background-color: #f2f2f2;'>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>اسم المادة</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>رقم المادة</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>NSN</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>الكمية الحرجة</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>إجمالي المخزون</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>إجمالي الكمية المحجوزة</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>إجمالي المصروف سابقا</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B; background-color: #ffcccc;'>المتبقي</th>
                            </tr>
                        </thead>
                        <tbody>
            ");

            foreach (var itemInfo in items)
            {
                var item = itemInfo.Item;
                htmlContent.AppendLine($@"
                            <tr>
                                <td style='padding: 8px; border: 1px solid #6B6B6B;'>{WebUtility.HtmlEncode(item.Name)}</td>
                                <td style='padding: 8px; border: 1px solid #6B6B6B;'>{WebUtility.HtmlEncode(item.ItemNo)}</td>
                                <td style='padding: 8px; border: 1px solid #6B6B6B;'>{WebUtility.HtmlEncode(item.Nsn ?? "N/A")}</td>
                                <td style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>{item.CriticalQuantity}</td>
                                <td style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>{itemInfo.TotalStock}</td>
                                <td style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>{itemInfo.HoldQuantity}</td>
                                <td style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>{itemInfo.SuppliedQuantity}</td>
                                <td style='text-align: right; padding: 8px; border: 1px solid #6B6B6B; font-weight: bold; background-color: #ffcccc;'>{itemInfo.Remaining}</td>
                            </tr>
                ");
            }

            htmlContent.AppendLine(@"
                        </tbody>
                    </table>
                </div>
            ");

            return htmlContent.ToString();
        }
    }
}
