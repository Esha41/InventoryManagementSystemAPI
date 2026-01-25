using DevExpress.Drawing;
using DevExpress.Drawing.Printing;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using System;
using System.Drawing;
using System.IO;

namespace Ettad.Reporting.Reports
{
    /// <summary>
    /// Base template for all reports created from the frontend.
    /// Minimal layout to avoid serialization issues in Report Designer.
    /// Provides: Report header (title), Page header, Detail, Page footer (page numbers), Report footer.
    /// </summary>
    public class BaseReportTemplate : XtraReport
    {
        private TopMarginBand topMarginBand1;
        private BottomMarginBand bottomMarginBand1;
        private ReportHeaderBand reportHeaderBand;
        private PageHeaderBand pageHeaderBand;
        private DetailBand detailBand1;
        private PageFooterBand pageFooterBand;
        private ReportFooterBand reportFooterBand;

        private XRLabel xrLabelReportTitle;
        private XRLabel xrLabelReportSubtitle;
        private XRLine xrLineHeaderSeparator;
        private XRLabel xrLabelPageHeader;
        private XRLabel xrLabelCompanyName;
        private XRPageInfo xrPageInfo;
        private XRLine xrLineFooterSeparator;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private System.ComponentModel.IContainer components;
        private XRLabel xrLabelReportFooter;

        public BaseReportTemplate()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery1 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column1 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table1 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column2 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression2 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column3 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression3 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column4 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression4 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column5 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression5 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column6 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression6 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column7 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression7 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column8 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression8 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column9 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression9 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column10 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression10 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column11 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression11 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column12 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression12 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column13 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression13 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column14 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression14 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column15 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression15 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column16 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression16 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column17 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression17 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column18 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression18 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column19 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression19 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column20 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression20 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column21 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression21 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery2 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column22 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression22 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table2 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column23 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression23 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column24 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression24 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column25 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression25 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column26 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression26 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column27 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression27 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column28 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression28 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column29 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression29 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column30 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression30 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column31 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression31 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column32 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression32 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column33 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression33 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column34 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression34 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column35 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression35 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column36 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression36 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column37 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression37 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column38 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression38 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column39 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression39 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column40 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression40 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column41 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression41 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column42 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression42 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column43 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression43 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column44 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression44 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column45 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression45 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column46 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression46 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column47 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression47 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column48 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression48 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery3 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column49 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression49 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table3 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column50 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression50 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column51 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression51 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column52 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression52 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column53 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression53 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column54 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression54 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column55 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression55 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column56 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression56 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column57 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression57 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column58 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression58 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column59 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression59 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column60 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression60 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column61 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression61 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column62 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression62 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column63 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression63 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column64 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression64 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery4 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column65 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression65 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table4 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column66 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression66 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column67 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression67 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column68 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression68 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column69 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression69 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column70 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression70 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column71 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression71 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column72 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression72 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column73 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression73 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column74 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression74 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column75 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression75 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column76 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression76 = new DevExpress.DataAccess.Sql.ColumnExpression();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseReportTemplate));
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.reportHeaderBand = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabelReportTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelReportSubtitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLineHeaderSeparator = new DevExpress.XtraReports.UI.XRLine();
            this.pageHeaderBand = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLabelPageHeader = new DevExpress.XtraReports.UI.XRLabel();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.pageFooterBand = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLineFooterSeparator = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabelCompanyName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo = new DevExpress.XtraReports.UI.XRPageInfo();
            this.reportFooterBand = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabelReportFooter = new DevExpress.XtraReports.UI.XRLabel();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.HeightF = 25F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.HeightF = 30F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // reportHeaderBand
            // 
            this.reportHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabelReportTitle,
            this.xrLabelReportSubtitle,
            this.xrLineHeaderSeparator});
            this.reportHeaderBand.HeightF = 80F;
            this.reportHeaderBand.Name = "reportHeaderBand";
            // 
            // xrLabelReportTitle
            // 
            this.xrLabelReportTitle.Font = new DevExpress.Drawing.DXFont("Segoe UI", 16F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabelReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabelReportTitle.Multiline = true;
            this.xrLabelReportTitle.Name = "xrLabelReportTitle";
            this.xrLabelReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabelReportTitle.SizeF = new System.Drawing.SizeF(650F, 28F);
            this.xrLabelReportTitle.Text = "Report Title";
            this.xrLabelReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabelReportSubtitle
            // 
            this.xrLabelReportSubtitle.Font = new DevExpress.Drawing.DXFont("Segoe UI", 9F);
            this.xrLabelReportSubtitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 32F);
            this.xrLabelReportSubtitle.Name = "xrLabelReportSubtitle";
            this.xrLabelReportSubtitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabelReportSubtitle.SizeF = new System.Drawing.SizeF(650F, 18F);
            this.xrLabelReportSubtitle.Text = "Date, time, and generated-by can be added in the designer.";
            this.xrLabelReportSubtitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLineHeaderSeparator
            // 
            this.xrLineHeaderSeparator.LineWidth = 2F;
            this.xrLineHeaderSeparator.LocationFloat = new DevExpress.Utils.PointFloat(0F, 72F);
            this.xrLineHeaderSeparator.Name = "xrLineHeaderSeparator";
            this.xrLineHeaderSeparator.SizeF = new System.Drawing.SizeF(650F, 2.083333F);
            // 
            // pageHeaderBand
            // 
            this.pageHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabelPageHeader});
            this.pageHeaderBand.HeightF = 28F;
            this.pageHeaderBand.Name = "pageHeaderBand";
            // 
            // xrLabelPageHeader
            // 
            this.xrLabelPageHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xrLabelPageHeader.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabelPageHeader.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabelPageHeader.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabelPageHeader.Name = "xrLabelPageHeader";
            this.xrLabelPageHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(4F, 4F, 4F, 4F, 100F);
            this.xrLabelPageHeader.SizeF = new System.Drawing.SizeF(650F, 24F);
            this.xrLabelPageHeader.Text = "Column Headers";
            this.xrLabelPageHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // detailBand1
            // 
            this.detailBand1.HeightF = 25F;
            this.detailBand1.Name = "detailBand1";
            // 
            // pageFooterBand
            // 
            this.pageFooterBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLineFooterSeparator,
            this.xrLabelCompanyName,
            this.xrPageInfo});
            this.pageFooterBand.HeightF = 45F;
            this.pageFooterBand.Name = "pageFooterBand";
            // 
            // xrLineFooterSeparator
            // 
            this.xrLineFooterSeparator.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLineFooterSeparator.Name = "xrLineFooterSeparator";
            this.xrLineFooterSeparator.SizeF = new System.Drawing.SizeF(650F, 2F);
            // 
            // xrLabelCompanyName
            // 
            this.xrLabelCompanyName.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8F);
            this.xrLabelCompanyName.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6F);
            this.xrLabelCompanyName.Name = "xrLabelCompanyName";
            this.xrLabelCompanyName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabelCompanyName.SizeF = new System.Drawing.SizeF(350F, 18F);
            this.xrLabelCompanyName.Text = "ETTAD System";
            this.xrLabelCompanyName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8F);
            this.xrPageInfo.LocationFloat = new DevExpress.Utils.PointFloat(350F, 6F);
            this.xrPageInfo.Name = "xrPageInfo";
            this.xrPageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrPageInfo.SizeF = new System.Drawing.SizeF(300F, 18F);
            this.xrPageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrPageInfo.TextFormatString = "Page {0} of {1}";
            // 
            // reportFooterBand
            // 
            this.reportFooterBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabelReportFooter});
            this.reportFooterBand.HeightF = 28F;
            this.reportFooterBand.Name = "reportFooterBand";
            // 
            // xrLabelReportFooter
            // 
            this.xrLabelReportFooter.Font = new DevExpress.Drawing.DXFont("Segoe UI", 9F, DevExpress.Drawing.DXFontStyle.Italic);
            this.xrLabelReportFooter.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4F);
            this.xrLabelReportFooter.Name = "xrLabelReportFooter";
            this.xrLabelReportFooter.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabelReportFooter.SizeF = new System.Drawing.SizeF(650F, 18F);
            this.xrLabelReportFooter.Text = "End of Report";
            this.xrLabelReportFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "DefaultConnection";
            this.sqlDataSource1.Name = "sqlDataSource1";
            columnExpression1.ColumnName = "Id";
            table1.Name = "Assets";
            columnExpression1.Table = table1;
            column1.Expression = columnExpression1;
            columnExpression2.ColumnName = "ItemId";
            columnExpression2.Table = table1;
            column2.Expression = columnExpression2;
            columnExpression3.ColumnName = "SerialNumber";
            columnExpression3.Table = table1;
            column3.Expression = columnExpression3;
            columnExpression4.ColumnName = "RFID";
            columnExpression4.Table = table1;
            column4.Expression = columnExpression4;
            columnExpression5.ColumnName = "DepotId";
            columnExpression5.Table = table1;
            column5.Expression = columnExpression5;
            columnExpression6.ColumnName = "CurrentAssignmentId";
            columnExpression6.Table = table1;
            column6.Expression = columnExpression6;
            columnExpression7.ColumnName = "Status";
            columnExpression7.Table = table1;
            column7.Expression = columnExpression7;
            columnExpression8.ColumnName = "AssetTag";
            columnExpression8.Table = table1;
            column8.Expression = columnExpression8;
            columnExpression9.ColumnName = "PurchaseDate";
            columnExpression9.Table = table1;
            column9.Expression = columnExpression9;
            columnExpression10.ColumnName = "WarrantyExpiryDate";
            columnExpression10.Table = table1;
            column10.Expression = columnExpression10;
            columnExpression11.ColumnName = "Condition";
            columnExpression11.Table = table1;
            column11.Expression = columnExpression11;
            columnExpression12.ColumnName = "PurchasePrice";
            columnExpression12.Table = table1;
            column12.Expression = columnExpression12;
            columnExpression13.ColumnName = "Notes";
            columnExpression13.Table = table1;
            column13.Expression = columnExpression13;
            columnExpression14.ColumnName = "CreationDate";
            columnExpression14.Table = table1;
            column14.Expression = columnExpression14;
            columnExpression15.ColumnName = "ModificationDate";
            columnExpression15.Table = table1;
            column15.Expression = columnExpression15;
            columnExpression16.ColumnName = "ModifiedBy";
            columnExpression16.Table = table1;
            column16.Expression = columnExpression16;
            columnExpression17.ColumnName = "CreatedBy";
            columnExpression17.Table = table1;
            column17.Expression = columnExpression17;
            columnExpression18.ColumnName = "IsDeleted";
            columnExpression18.Table = table1;
            column18.Expression = columnExpression18;
            columnExpression19.ColumnName = "DeletionDate";
            columnExpression19.Table = table1;
            column19.Expression = columnExpression19;
            columnExpression20.ColumnName = "DeletedBy";
            columnExpression20.Table = table1;
            column20.Expression = columnExpression20;
            columnExpression21.ColumnName = "IsAssigned";
            columnExpression21.Table = table1;
            column21.Expression = columnExpression21;
            selectQuery1.Columns.Add(column1);
            selectQuery1.Columns.Add(column2);
            selectQuery1.Columns.Add(column3);
            selectQuery1.Columns.Add(column4);
            selectQuery1.Columns.Add(column5);
            selectQuery1.Columns.Add(column6);
            selectQuery1.Columns.Add(column7);
            selectQuery1.Columns.Add(column8);
            selectQuery1.Columns.Add(column9);
            selectQuery1.Columns.Add(column10);
            selectQuery1.Columns.Add(column11);
            selectQuery1.Columns.Add(column12);
            selectQuery1.Columns.Add(column13);
            selectQuery1.Columns.Add(column14);
            selectQuery1.Columns.Add(column15);
            selectQuery1.Columns.Add(column16);
            selectQuery1.Columns.Add(column17);
            selectQuery1.Columns.Add(column18);
            selectQuery1.Columns.Add(column19);
            selectQuery1.Columns.Add(column20);
            selectQuery1.Columns.Add(column21);
            selectQuery1.Name = "Assets";
            selectQuery1.Tables.Add(table1);
            columnExpression22.ColumnName = "Id";
            table2.Name = "AssetHistory";
            columnExpression22.Table = table2;
            column22.Expression = columnExpression22;
            columnExpression23.ColumnName = "AssetId";
            columnExpression23.Table = table2;
            column23.Expression = columnExpression23;
            columnExpression24.ColumnName = "ActionType";
            columnExpression24.Table = table2;
            column24.Expression = columnExpression24;
            columnExpression25.ColumnName = "ActionDate";
            columnExpression25.Table = table2;
            column25.Expression = columnExpression25;
            columnExpression26.ColumnName = "Description";
            columnExpression26.Table = table2;
            column26.Expression = columnExpression26;
            columnExpression27.ColumnName = "PreviousStatus";
            columnExpression27.Table = table2;
            column27.Expression = columnExpression27;
            columnExpression28.ColumnName = "NewStatus";
            columnExpression28.Table = table2;
            column28.Expression = columnExpression28;
            columnExpression29.ColumnName = "PreviousDepartmentId";
            columnExpression29.Table = table2;
            column29.Expression = columnExpression29;
            columnExpression30.ColumnName = "NewDepartmentId";
            columnExpression30.Table = table2;
            column30.Expression = columnExpression30;
            columnExpression31.ColumnName = "PreviousCustodianId";
            columnExpression31.Table = table2;
            column31.Expression = columnExpression31;
            columnExpression32.ColumnName = "NewCustodianId";
            columnExpression32.Table = table2;
            column32.Expression = columnExpression32;
            columnExpression33.ColumnName = "PreviousLocation";
            columnExpression33.Table = table2;
            column33.Expression = columnExpression33;
            columnExpression34.ColumnName = "NewLocation";
            columnExpression34.Table = table2;
            column34.Expression = columnExpression34;
            columnExpression35.ColumnName = "OrderId";
            columnExpression35.Table = table2;
            column35.Expression = columnExpression35;
            columnExpression36.ColumnName = "AssetSupplyId";
            columnExpression36.Table = table2;
            column36.Expression = columnExpression36;
            columnExpression37.ColumnName = "AssetAssignmentId";
            columnExpression37.Table = table2;
            column37.Expression = columnExpression37;
            columnExpression38.ColumnName = "PerformedByUserId";
            columnExpression38.Table = table2;
            column38.Expression = columnExpression38;
            columnExpression39.ColumnName = "PerformedByUserName";
            columnExpression39.Table = table2;
            column39.Expression = columnExpression39;
            columnExpression40.ColumnName = "Notes";
            columnExpression40.Table = table2;
            column40.Expression = columnExpression40;
            columnExpression41.ColumnName = "Metadata";
            columnExpression41.Table = table2;
            column41.Expression = columnExpression41;
            columnExpression42.ColumnName = "CreationDate";
            columnExpression42.Table = table2;
            column42.Expression = columnExpression42;
            columnExpression43.ColumnName = "ModificationDate";
            columnExpression43.Table = table2;
            column43.Expression = columnExpression43;
            columnExpression44.ColumnName = "ModifiedBy";
            columnExpression44.Table = table2;
            column44.Expression = columnExpression44;
            columnExpression45.ColumnName = "CreatedBy";
            columnExpression45.Table = table2;
            column45.Expression = columnExpression45;
            columnExpression46.ColumnName = "IsDeleted";
            columnExpression46.Table = table2;
            column46.Expression = columnExpression46;
            columnExpression47.ColumnName = "DeletionDate";
            columnExpression47.Table = table2;
            column47.Expression = columnExpression47;
            columnExpression48.ColumnName = "DeletedBy";
            columnExpression48.Table = table2;
            column48.Expression = columnExpression48;
            selectQuery2.Columns.Add(column22);
            selectQuery2.Columns.Add(column23);
            selectQuery2.Columns.Add(column24);
            selectQuery2.Columns.Add(column25);
            selectQuery2.Columns.Add(column26);
            selectQuery2.Columns.Add(column27);
            selectQuery2.Columns.Add(column28);
            selectQuery2.Columns.Add(column29);
            selectQuery2.Columns.Add(column30);
            selectQuery2.Columns.Add(column31);
            selectQuery2.Columns.Add(column32);
            selectQuery2.Columns.Add(column33);
            selectQuery2.Columns.Add(column34);
            selectQuery2.Columns.Add(column35);
            selectQuery2.Columns.Add(column36);
            selectQuery2.Columns.Add(column37);
            selectQuery2.Columns.Add(column38);
            selectQuery2.Columns.Add(column39);
            selectQuery2.Columns.Add(column40);
            selectQuery2.Columns.Add(column41);
            selectQuery2.Columns.Add(column42);
            selectQuery2.Columns.Add(column43);
            selectQuery2.Columns.Add(column44);
            selectQuery2.Columns.Add(column45);
            selectQuery2.Columns.Add(column46);
            selectQuery2.Columns.Add(column47);
            selectQuery2.Columns.Add(column48);
            selectQuery2.Name = "AssetHistory";
            selectQuery2.Tables.Add(table2);
            columnExpression49.ColumnName = "Id";
            table3.Name = "Ammunitions";
            columnExpression49.Table = table3;
            column49.Expression = columnExpression49;
            columnExpression50.ColumnName = "AmmunitionType";
            columnExpression50.Table = table3;
            column50.Expression = columnExpression50;
            columnExpression51.ColumnName = "BulletDiameter";
            columnExpression51.Table = table3;
            column51.Expression = columnExpression51;
            columnExpression52.ColumnName = "BulletDiameterUnitId";
            columnExpression52.Table = table3;
            column52.Expression = columnExpression52;
            columnExpression53.ColumnName = "ArmNumber";
            columnExpression53.Table = table3;
            column53.Expression = columnExpression53;
            columnExpression54.ColumnName = "IsLinked";
            columnExpression54.Table = table3;
            column54.Expression = columnExpression54;
            columnExpression55.ColumnName = "Primer";
            columnExpression55.Table = table3;
            column55.Expression = columnExpression55;
            columnExpression56.ColumnName = "TotalWeight";
            columnExpression56.Table = table3;
            column56.Expression = columnExpression56;
            columnExpression57.ColumnName = "NatureOptionId";
            columnExpression57.Table = table3;
            column57.Expression = columnExpression57;
            columnExpression58.ColumnName = "PrimaryPurposId";
            columnExpression58.Table = table3;
            column58.Expression = columnExpression58;
            columnExpression59.ColumnName = "ProjectileColorId";
            columnExpression59.Table = table3;
            column59.Expression = columnExpression59;
            columnExpression60.ColumnName = "ProjectailMaterialId";
            columnExpression60.Table = table3;
            column60.Expression = columnExpression60;
            columnExpression61.ColumnName = "CaseTypeId";
            columnExpression61.Table = table3;
            column61.Expression = columnExpression61;
            columnExpression62.ColumnName = "PropellantId";
            columnExpression62.Table = table3;
            column62.Expression = columnExpression62;
            columnExpression63.ColumnName = "CompatibilityId";
            columnExpression63.Table = table3;
            column63.Expression = columnExpression63;
            columnExpression64.ColumnName = "HazardDivisionId";
            columnExpression64.Table = table3;
            column64.Expression = columnExpression64;
            selectQuery3.Columns.Add(column49);
            selectQuery3.Columns.Add(column50);
            selectQuery3.Columns.Add(column51);
            selectQuery3.Columns.Add(column52);
            selectQuery3.Columns.Add(column53);
            selectQuery3.Columns.Add(column54);
            selectQuery3.Columns.Add(column55);
            selectQuery3.Columns.Add(column56);
            selectQuery3.Columns.Add(column57);
            selectQuery3.Columns.Add(column58);
            selectQuery3.Columns.Add(column59);
            selectQuery3.Columns.Add(column60);
            selectQuery3.Columns.Add(column61);
            selectQuery3.Columns.Add(column62);
            selectQuery3.Columns.Add(column63);
            selectQuery3.Columns.Add(column64);
            selectQuery3.Name = "Ammunitions";
            selectQuery3.Tables.Add(table3);
            columnExpression65.ColumnName = "Id";
            table4.Name = "AllowanceItems";
            columnExpression65.Table = table4;
            column65.Expression = columnExpression65;
            columnExpression66.ColumnName = "ItemId";
            columnExpression66.Table = table4;
            column66.Expression = columnExpression66;
            columnExpression67.ColumnName = "DepartmentId";
            columnExpression67.Table = table4;
            column67.Expression = columnExpression67;
            columnExpression68.ColumnName = "Year";
            columnExpression68.Table = table4;
            column68.Expression = columnExpression68;
            columnExpression69.ColumnName = "Quantity";
            columnExpression69.Table = table4;
            column69.Expression = columnExpression69;
            columnExpression70.ColumnName = "CreationDate";
            columnExpression70.Table = table4;
            column70.Expression = columnExpression70;
            columnExpression71.ColumnName = "ModificationDate";
            columnExpression71.Table = table4;
            column71.Expression = columnExpression71;
            columnExpression72.ColumnName = "ModifiedBy";
            columnExpression72.Table = table4;
            column72.Expression = columnExpression72;
            columnExpression73.ColumnName = "CreatedBy";
            columnExpression73.Table = table4;
            column73.Expression = columnExpression73;
            columnExpression74.ColumnName = "IsDeleted";
            columnExpression74.Table = table4;
            column74.Expression = columnExpression74;
            columnExpression75.ColumnName = "DeletionDate";
            columnExpression75.Table = table4;
            column75.Expression = columnExpression75;
            columnExpression76.ColumnName = "DeletedBy";
            columnExpression76.Table = table4;
            column76.Expression = columnExpression76;
            selectQuery4.Columns.Add(column65);
            selectQuery4.Columns.Add(column66);
            selectQuery4.Columns.Add(column67);
            selectQuery4.Columns.Add(column68);
            selectQuery4.Columns.Add(column69);
            selectQuery4.Columns.Add(column70);
            selectQuery4.Columns.Add(column71);
            selectQuery4.Columns.Add(column72);
            selectQuery4.Columns.Add(column73);
            selectQuery4.Columns.Add(column74);
            selectQuery4.Columns.Add(column75);
            selectQuery4.Columns.Add(column76);
            selectQuery4.Name = "AllowanceItems";
            selectQuery4.Tables.Add(table4);
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            selectQuery1,
            selectQuery2,
            selectQuery3,
            selectQuery4});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // BaseReportTemplate
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.bottomMarginBand1,
            this.reportHeaderBand,
            this.pageHeaderBand,
            this.detailBand1,
            this.pageFooterBand,
            this.reportFooterBand});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "AllowanceItems";
            this.DataSource = this.sqlDataSource1;
            this.Margins = new DevExpress.Drawing.DXMargins(25F, 25F, 25F, 30F);
            this.PageHeightF = 1169.291F;
            this.PageWidthF = 826.7717F;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
            this.Version = "25.2.3";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        public void SetReportTitle(string title)
        {
            if (xrLabelReportTitle != null)
                xrLabelReportTitle.Text = title;
        }

        public void SetReportSubtitle(string subtitle)
        {
            if (xrLabelReportSubtitle != null)
                xrLabelReportSubtitle.Text = subtitle;
        }

        public void SetCompanyName(string companyName)
        {
            if (xrLabelCompanyName != null)
                xrLabelCompanyName.Text = companyName;
        }

        public void SetReportFooter(string footerText)
        {
            if (xrLabelReportFooter != null)
                xrLabelReportFooter.Text = footerText;
        }

        public void AddParameter(string name, Type type, object defaultValue = null, string description = null)
        {
            var p = new Parameter { Name = name, Type = type, Visible = true, Description = description };
            if (defaultValue != null) p.Value = defaultValue;
            this.Parameters.Add(p);
        }

        public void HidePageHeader()
        {
            if (pageHeaderBand != null) pageHeaderBand.HeightF = 0F;
        }

        public void HideReportFooter()
        {
            if (reportFooterBand != null) reportFooterBand.HeightF = 0F;
        }
    }
}
