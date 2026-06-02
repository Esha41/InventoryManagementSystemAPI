using DevExpress.DataAccess.ObjectBinding;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace Ettad.ReportManagement.Service.Reports.Templates
{
    public partial class AssetsReportTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
        private DevExpress.XtraReports.UI.DetailBand detailBand1;
        private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRLabel xrLabelReportFooter;
        private DevExpress.XtraReports.UI.XRLabel xrLabelCompanyName;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo;
        private IContainer components;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBoxLogo;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.Parameters.Parameter Language;
        private DevExpress.XtraReports.UI.XRLabel xrLabelReportTitle;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellSrNoHeader;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellSrNoDetail;
        [System.Xml.Serialization.XmlIgnore]
        [System.NonSerialized]
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.DataAccess.Sql.SqlDataSource AssetsDataSource;
        private DevExpress.DataAccess.Sql.SqlDataSource EttadDataSource;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.Parameters.Parameter Primary_Purpose;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell8;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.CalculatedField calculatedField1;
        private DevExpress.XtraReports.Parameters.Parameter Caliber_Category;
        private DevExpress.XtraReports.Parameters.Parameter Asset;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell13;
        private DevExpress.XtraReports.UI.CalculatedField CaliberCategoryNameLocalized;
        private int _rowCounter = 0;

        private void detailBand1_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _rowCounter++;
            var band = sender as DevExpress.XtraReports.UI.DetailBand;
            if (band != null)
            {
                // Alternate row colors for better readability
                if (_rowCounter % 2 == 0)
                {
                    band.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
                }
                else
                {
                    band.BackColor = System.Drawing.Color.White;
                }
            }
        }

        public AssetsReportTemplate()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery3 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetsReportTemplate));
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery4 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings4 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings2 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings5 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings6 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            this.EttadDataSource = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.AssetsDataSource = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCellSrNoDetail = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabelReportTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBoxLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCellSrNoHeader = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabelCompanyName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo = new DevExpress.XtraReports.UI.XRPageInfo();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabelReportFooter = new DevExpress.XtraReports.UI.XRLabel();
            this.Language = new DevExpress.XtraReports.Parameters.Parameter();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.Primary_Purpose = new DevExpress.XtraReports.Parameters.Parameter();
            this.calculatedField1 = new DevExpress.XtraReports.UI.CalculatedField();
            this.Caliber_Category = new DevExpress.XtraReports.Parameters.Parameter();
            this.Asset = new DevExpress.XtraReports.Parameters.Parameter();
            this.CaliberCategoryNameLocalized = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // EttadDataSource
            // 
            this.EttadDataSource.ConnectionName = "DefaultConnection";
            this.EttadDataSource.Name = "EttadDataSource";
            customSqlQuery3.Name = "PrimaryPurposes";
            customSqlQuery3.Sql = resources.GetString("customSqlQuery3.Sql");
            this.EttadDataSource.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery3});
            this.EttadDataSource.ResultSchemaSerializable = resources.GetString("EttadDataSource.ResultSchemaSerializable");
            // 
            // AssetsDataSource
            // 
            this.AssetsDataSource.ConnectionName = "DefaultConnection";
            this.AssetsDataSource.Name = "AssetsDataSource";
            customSqlQuery4.Name = "AssetsQuery_WeaponAmm";
            customSqlQuery4.Sql = resources.GetString("customSqlQuery4.Sql");
            this.AssetsDataSource.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery4});
            this.AssetsDataSource.ResultSchemaSerializable = resources.GetString("AssetsDataSource.ResultSchemaSerializable");
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.HeightF = 35.41667F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.detailBand1.HeightF = 25.69444F;
            this.detailBand1.Name = "detailBand1";
            this.detailBand1.BeforePrint += new DevExpress.XtraReports.UI.BeforePrintEventHandler(this.detailBand1_BeforePrint);
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTable1.BorderWidth = 1F;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0.6945027F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(790.3055F, 25F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCellSrNoDetail,
            this.xrTableCell11,
            this.xrTableCell10,
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell8,
            this.xrTableCell12,
            this.xrTableCell14});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCellSrNoDetail
            // 
            this.xrTableCellSrNoDetail.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCellSrNoDetail.BorderWidth = 1F;
            this.xrTableCellSrNoDetail.Multiline = true;
            this.xrTableCellSrNoDetail.Name = "xrTableCellSrNoDetail";
            this.xrTableCellSrNoDetail.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCellSrNoDetail.StylePriority.UseBorders = false;
            xrSummary2.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCellSrNoDetail.Summary = xrSummary2;
            this.xrTableCellSrNoDetail.Text = "0";
            this.xrTableCellSrNoDetail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCellSrNoDetail.Weight = 0.27125550273638843D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell11.BorderWidth = 1F;
            this.xrTableCell11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Name]")});
            this.xrTableCell11.Multiline = true;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell11.StylePriority.UseBorders = false;
            this.xrTableCell11.Text = "xrTableCell11";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell11.Weight = 0.97658426434231171D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell10.BorderWidth = 1F;
            this.xrTableCell10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ItemNo]")});
            this.xrTableCell10.Multiline = true;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Column2";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell10.Weight = 0.501528649173586D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell1.BorderWidth = 1F;
            this.xrTableCell1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PartNo]")});
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Column3";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell1.TextFormatString = "{0:N2}";
            this.xrTableCell1.Weight = 0.62750012518384291D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell2.BorderWidth = 1F;
            this.xrTableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MinimumQuantity]")});
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "xrTableCell2";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.47508004143648119D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell8.BorderWidth = 1F;
            this.xrTableCell8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', [NameAr], [NameEn])")});
            this.xrTableCell8.Multiline = true;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell8.StylePriority.UseBorders = false;
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "xrTableCell8";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell8.Weight = 0.72889300675735447D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell12.BorderWidth = 1F;
            this.xrTableCell12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CaliberCategoryNameLocalized]")});
            this.xrTableCell12.Multiline = true;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell12.StylePriority.UseBorders = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "xrTableCell12";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell12.Weight = 0.64547073411790545D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell14.BorderWidth = 1F;
            this.xrTableCell14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', [CaliberNameAr], [CaliberNameEn])\n")});
            this.xrTableCell14.Multiline = true;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell14.StylePriority.UseBorders = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "xrTableCell14";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell14.Weight = 0.64547073411790545D;
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.HeightF = 38.19444F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrPageInfo1,
            this.xrLabelReportTitle,
            this.xrPictureBoxLogo});
            this.ReportHeader.HeightF = 124.4445F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel1
            // 
            this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'تم الإنشاء بتاريخ :\', \'Generated on :\')")});
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(598.0834F, 84.50004F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(96.80557F, 23F);
            this.xrLabel1.Text = "Generated on :";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(694.889F, 84.50004F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(96.11111F, 23F);
            this.xrPageInfo1.TextFormatString = "{0:MM/dd/yyyy}";
            // 
            // xrLabelReportTitle
            // 
            this.xrLabelReportTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabelReportTitle.BorderWidth = 3F;
            this.xrLabelReportTitle.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'تقرير الأصول\', \'Assets Report\')")});
            this.xrLabelReportTitle.Font = new DevExpress.Drawing.DXFont("Segoe UI", 18F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabelReportTitle.ForeColor = System.Drawing.Color.Red;
            this.xrLabelReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(202.7583F, 9.999996F);
            this.xrLabelReportTitle.Multiline = true;
            this.xrLabelReportTitle.Name = "xrLabelReportTitle";
            this.xrLabelReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(10F, 10F, 15F, 10F, 100F);
            this.xrLabelReportTitle.SizeF = new System.Drawing.SizeF(376F, 50F);
            this.xrLabelReportTitle.StylePriority.UseBorders = false;
            this.xrLabelReportTitle.StylePriority.UseFont = false;
            this.xrLabelReportTitle.StylePriority.UseForeColor = false;
            this.xrLabelReportTitle.StylePriority.UseTextAlignment = false;
            this.xrLabelReportTitle.Text = "Users Report";
            this.xrLabelReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPictureBoxLogo
            // 
            this.xrPictureBoxLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.xrPictureBoxLogo.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.TopCenter;
            this.xrPictureBoxLogo.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBoxLogo.ImageSource"));
            this.xrPictureBoxLogo.LocationFloat = new DevExpress.Utils.PointFloat(0.8332994F, 0F);
            this.xrPictureBoxLogo.Name = "xrPictureBoxLogo";
            this.xrPictureBoxLogo.SizeF = new System.Drawing.SizeF(127.5001F, 79.22227F);
            this.xrPictureBoxLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Cover;
            this.xrPictureBoxLogo.StylePriority.UseBackColor = false;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.PageHeader.HeightF = 30F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.BorderWidth = 1F;
            this.xrTable2.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(791F, 30F);
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCellSrNoHeader,
            this.xrTableCell3,
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6,
            this.xrTableCell9,
            this.xrTableCell7,
            this.xrTableCell13});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCellSrNoHeader
            // 
            this.xrTableCellSrNoHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.xrTableCellSrNoHeader.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCellSrNoHeader.BorderWidth = 1F;
            this.xrTableCellSrNoHeader.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'السيد #\', \'Sr #\')")});
            this.xrTableCellSrNoHeader.ForeColor = System.Drawing.Color.White;
            this.xrTableCellSrNoHeader.Multiline = true;
            this.xrTableCellSrNoHeader.Name = "xrTableCellSrNoHeader";
            this.xrTableCellSrNoHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCellSrNoHeader.StylePriority.UseTextAlignment = false;
            this.xrTableCellSrNoHeader.Text = "Sr #";
            this.xrTableCellSrNoHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCellSrNoHeader.Weight = 0.266510781199649D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.xrTableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell3.BorderWidth = 1F;
            this.xrTableCell3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'اسم\', \'Name\')")});
            this.xrTableCell3.ForeColor = System.Drawing.Color.White;
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Name";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell3.Weight = 1.0621365571037751D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell4.BorderWidth = 1F;
            this.xrTableCell4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'رقم الصنف\', \'Item No\')")});
            this.xrTableCell4.ForeColor = System.Drawing.Color.White;
            this.xrTableCell4.Multiline = true;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "Item No";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.535503385904587D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.xrTableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell5.BorderWidth = 1F;
            this.xrTableCell5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'رقم الجزء\', \'Part No\')")});
            this.xrTableCell5.ForeColor = System.Drawing.Color.White;
            this.xrTableCell5.Multiline = true;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "Part No";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell5.Weight = 0.66703970086139142D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.xrTableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell6.BorderWidth = 1F;
            this.xrTableCell6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'مخزون منخفض\', \'Low Stock\')\n")});
            this.xrTableCell6.ForeColor = System.Drawing.Color.White;
            this.xrTableCell6.Multiline = true;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "Low Stock";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell6.Weight = 0.50939841798922569D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell9.BorderWidth = 1F;
            this.xrTableCell9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'الغرض الأساسي\', \'Primary Purpose\')\n")});
            this.xrTableCell9.ForeColor = System.Drawing.Color.White;
            this.xrTableCell9.Multiline = true;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "Primary Purpose";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell9.Weight = 0.77609551445392377D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.xrTableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell7.BorderWidth = 1F;
            this.xrTableCell7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'فئة العيار\', \'Caliber Category\')\n")});
            this.xrTableCell7.ForeColor = System.Drawing.Color.White;
            this.xrTableCell7.Multiline = true;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "Caliber Category";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell7.Weight = 0.68331562736976859D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.xrTableCell13.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell13.BorderWidth = 1F;
            this.xrTableCell13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'عيار\', \'Caliber\')\n")});
            this.xrTableCell13.ForeColor = System.Drawing.Color.White;
            this.xrTableCell13.Multiline = true;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 5F, 5F, 100F);
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.Text = "Caliber";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell13.Weight = 0.68331562736976859D;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabelCompanyName,
            this.xrLabel6,
            this.xrPageInfo});
            this.PageFooter.HeightF = 20.13889F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrLabelCompanyName
            // 
            this.xrLabelCompanyName.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'امداد\', \'Emdad\')\n")});
            this.xrLabelCompanyName.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8F);
            this.xrLabelCompanyName.LocationFloat = new DevExpress.Utils.PointFloat(0.6944444F, 0F);
            this.xrLabelCompanyName.Name = "xrLabelCompanyName";
            this.xrLabelCompanyName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabelCompanyName.SizeF = new System.Drawing.SizeF(96.11113F, 18F);
            this.xrLabelCompanyName.Text = "ETTAD System";
            this.xrLabelCompanyName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'سري - للاستخدام الداخلي فقط\', \'Confidential – For interna" +
                    "l use only\')")});
            this.xrLabel6.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8F, DevExpress.Drawing.DXFontStyle.Italic);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(96.80557F, 0F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(573.4939F, 18F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Confidential – For internal use only";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "TextFormatString", "Iif(?Language == \'ar\', \'صفحة {0} من {1}\', \'Page {0} of {1}\')")});
            this.xrPageInfo.Font = new DevExpress.Drawing.DXFont("Arial", 8.25F);
            this.xrPageInfo.LocationFloat = new DevExpress.Utils.PointFloat(670.2996F, 0F);
            this.xrPageInfo.Name = "xrPageInfo";
            this.xrPageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrPageInfo.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.No;
            this.xrPageInfo.SizeF = new System.Drawing.SizeF(120.7004F, 18F);
            this.xrPageInfo.StylePriority.UseFont = false;
            this.xrPageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrPageInfo.TextFormatString = "Page {0} of {1}";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabelReportFooter});
            this.ReportFooter.HeightF = 69.24994F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrLabelReportFooter
            // 
            this.xrLabelReportFooter.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'نهاية التقرير\', \'End of Report\')")});
            this.xrLabelReportFooter.Font = new DevExpress.Drawing.DXFont("Segoe UI", 9F, DevExpress.Drawing.DXFontStyle.Italic);
            this.xrLabelReportFooter.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabelReportFooter.Name = "xrLabelReportFooter";
            this.xrLabelReportFooter.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabelReportFooter.SizeF = new System.Drawing.SizeF(791F, 68.69444F);
            this.xrLabelReportFooter.Text = "End of Report";
            this.xrLabelReportFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // Language
            // 
            this.Language.Description = "Language";
            this.Language.Name = "Language";
            this.Language.ValueInfo = "en";
            staticListLookUpSettings4.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue("en", "English"));
            staticListLookUpSettings4.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue("ar", "Arabic"));
            this.Language.ValueSourceSettings = staticListLookUpSettings4;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.HeightF = 31.66661F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // Primary_Purpose
            // 
            this.Primary_Purpose.AllowNull = true;
            this.Primary_Purpose.Description = "Primary Purpose";
            this.Primary_Purpose.MultiValue = true;
            this.Primary_Purpose.Name = "Primary_Purpose";
            this.Primary_Purpose.SelectAllValues = true;
            this.Primary_Purpose.Type = typeof(int);
            dynamicListLookUpSettings2.DataMember = "PrimaryPurposes";
            dynamicListLookUpSettings2.DataSource = this.EttadDataSource;
            dynamicListLookUpSettings2.DisplayMember = "NameEn";
            dynamicListLookUpSettings2.SortMember = "NameEn";
            dynamicListLookUpSettings2.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            dynamicListLookUpSettings2.ValueMember = "Id";
            this.Primary_Purpose.ValueSourceSettings = dynamicListLookUpSettings2;
            // 
            // calculatedField1
            // 
            this.calculatedField1.DataMember = "WorkFlowType";
            this.calculatedField1.DataSource = this.EttadDataSource;
            this.calculatedField1.Name = "calculatedField1";
            // 
            // Caliber_Category
            // 
            this.Caliber_Category.AllowNull = true;
            this.Caliber_Category.Description = "Caliber Category";
            this.Caliber_Category.MultiValue = true;
            this.Caliber_Category.Name = "Caliber_Category";
            this.Caliber_Category.SelectAllValues = true;
            this.Caliber_Category.Type = typeof(int);
            staticListLookUpSettings5.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(1, "Small"));
            staticListLookUpSettings5.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(2, "Medium"));
            staticListLookUpSettings5.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(3, "Large"));
            this.Caliber_Category.ValueSourceSettings = staticListLookUpSettings5;
            // 
            // Asset
            // 
            this.Asset.Description = "Asset";
            this.Asset.MultiValue = true;
            this.Asset.Name = "Asset";
            this.Asset.SelectAllValues = true;
            this.Asset.Type = typeof(int);
            staticListLookUpSettings6.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(1, "Ammunitions"));
            staticListLookUpSettings6.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(2, "Weapons"));
            this.Asset.ValueSourceSettings = staticListLookUpSettings6;
            // 
            // CaliberCategoryNameLocalized
            // 
            this.CaliberCategoryNameLocalized.DataMember = "AssetsQuery_WeaponAmm";
            this.CaliberCategoryNameLocalized.Expression = resources.GetString("CaliberCategoryNameLocalized.Expression");
            this.CaliberCategoryNameLocalized.Name = "CaliberCategoryNameLocalized";
            // 
            // AssetsReportTemplate
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.detailBand1,
            this.bottomMarginBand1,
            this.ReportHeader,
            this.PageHeader,
            this.PageFooter,
            this.ReportFooter,
            this.GroupFooter1});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.calculatedField1,
            this.CaliberCategoryNameLocalized});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.AssetsDataSource,
            this.EttadDataSource});
            this.DataMember = "AssetsQuery_WeaponAmm";
            this.DataSource = this.AssetsDataSource;
            this.FilterString = resources.GetString("$this.FilterString");
            this.Margins = new DevExpress.Drawing.DXMargins(34F, 25F, 35.41667F, 38.19444F);
            this.ParameterPanelLayoutItems.AddRange(new DevExpress.XtraReports.Parameters.ParameterPanelLayoutItem[] {
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.Language, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.Asset, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.Primary_Purpose, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.Caliber_Category, DevExpress.XtraReports.Parameters.Orientation.Horizontal)});
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Language,
            this.Asset,
            this.Primary_Purpose,
            this.Caliber_Category});
            this.Version = "25.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}

