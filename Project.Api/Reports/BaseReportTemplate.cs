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
        private XRLabel xrLabelReportFooter;

        public BaseReportTemplate()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.topMarginBand1 = new TopMarginBand();
            this.bottomMarginBand1 = new BottomMarginBand();
            this.reportHeaderBand = new ReportHeaderBand();
            this.pageHeaderBand = new PageHeaderBand();
            this.detailBand1 = new DetailBand();
            this.pageFooterBand = new PageFooterBand();
            this.reportFooterBand = new ReportFooterBand();

            this.xrLabelReportTitle = new XRLabel();
            this.xrLabelReportSubtitle = new XRLabel();
            this.xrLineHeaderSeparator = new XRLine();
            this.xrLabelPageHeader = new XRLabel();
            this.xrLabelCompanyName = new XRLabel();
            this.xrPageInfo = new XRPageInfo();
            this.xrLineFooterSeparator = new XRLine();
            this.xrLabelReportFooter = new XRLabel();

            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();

            // topMarginBand1
            this.topMarginBand1.HeightF = 25F;
            this.topMarginBand1.Name = "topMarginBand1";

            // bottomMarginBand1
            this.bottomMarginBand1.HeightF = 30F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";

            // reportHeaderBand
            this.reportHeaderBand.Controls.AddRange(new XRControl[] {
                this.xrLabelReportTitle,
                this.xrLabelReportSubtitle,
                this.xrLineHeaderSeparator });
            this.reportHeaderBand.HeightF = 80F;
            this.reportHeaderBand.Name = "reportHeaderBand";

            // xrLabelReportTitle
            this.xrLabelReportTitle.Font = new DXFont("Segoe UI", 16F, DXFontStyle.Bold);
            this.xrLabelReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabelReportTitle.Multiline = true;
            this.xrLabelReportTitle.Name = "xrLabelReportTitle";
            this.xrLabelReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelReportTitle.SizeF = new SizeF(650F, 28F);
            this.xrLabelReportTitle.Text = "Report Title";
            this.xrLabelReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;

            // xrLabelReportSubtitle (Date / Time / User - plain text, user edits in designer)
            this.xrLabelReportSubtitle.Font = new DXFont("Segoe UI", 9F);
            this.xrLabelReportSubtitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 32F);
            this.xrLabelReportSubtitle.Name = "xrLabelReportSubtitle";
            this.xrLabelReportSubtitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelReportSubtitle.SizeF = new SizeF(650F, 18F);
            this.xrLabelReportSubtitle.Text = "Date, time, and generated-by can be added in the designer.";
            this.xrLabelReportSubtitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;

            // xrLineHeaderSeparator
            this.xrLineHeaderSeparator.LineWidth = 2F;
            this.xrLineHeaderSeparator.LocationFloat = new DevExpress.Utils.PointFloat(0F, 72F);
            this.xrLineHeaderSeparator.Name = "xrLineHeaderSeparator";
            this.xrLineHeaderSeparator.SizeF = new SizeF(650F, 2F);

            // pageHeaderBand
            this.pageHeaderBand.Controls.AddRange(new XRControl[] { this.xrLabelPageHeader });
            this.pageHeaderBand.HeightF = 28F;
            this.pageHeaderBand.Name = "pageHeaderBand";

            // xrLabelPageHeader
            this.xrLabelPageHeader.BackColor = Color.FromArgb(248, 248, 248);
            this.xrLabelPageHeader.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabelPageHeader.Font = new DXFont("Segoe UI", 10F, DXFontStyle.Bold);
            this.xrLabelPageHeader.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabelPageHeader.Name = "xrLabelPageHeader";
            this.xrLabelPageHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 4, 4, 100F);
            this.xrLabelPageHeader.SizeF = new SizeF(650F, 24F);
            this.xrLabelPageHeader.Text = "Column Headers";
            this.xrLabelPageHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;

            // detailBand1
            this.detailBand1.HeightF = 25F;
            this.detailBand1.Name = "detailBand1";

            // pageFooterBand
            this.pageFooterBand.Controls.AddRange(new XRControl[] {
                this.xrLineFooterSeparator,
                this.xrLabelCompanyName,
                this.xrPageInfo });
            this.pageFooterBand.HeightF = 45F;
            this.pageFooterBand.Name = "pageFooterBand";

            // xrLineFooterSeparator
            this.xrLineFooterSeparator.LineWidth = 1F;
            this.xrLineFooterSeparator.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLineFooterSeparator.Name = "xrLineFooterSeparator";
            this.xrLineFooterSeparator.SizeF = new SizeF(650F, 2F);

            // xrLabelCompanyName
            this.xrLabelCompanyName.Font = new DXFont("Segoe UI", 8F);
            this.xrLabelCompanyName.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6F);
            this.xrLabelCompanyName.Name = "xrLabelCompanyName";
            this.xrLabelCompanyName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelCompanyName.SizeF = new SizeF(350F, 18F);
            this.xrLabelCompanyName.Text = "ETTAD System";
            this.xrLabelCompanyName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;

            // xrPageInfo - use control instead of expression to avoid deserialization issues
            this.xrPageInfo.Font = new DXFont("Segoe UI", 8F);
            this.xrPageInfo.LocationFloat = new DevExpress.Utils.PointFloat(350F, 6F);
            this.xrPageInfo.Name = "xrPageInfo";
            this.xrPageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo.PageInfo = DevExpress.XtraPrinting.PageInfo.NumberOfTotal;
            this.xrPageInfo.SizeF = new SizeF(300F, 18F);
            this.xrPageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrPageInfo.TextFormatString = "Page {0} of {1}";

            // reportFooterBand
            this.reportFooterBand.Controls.AddRange(new XRControl[] { this.xrLabelReportFooter });
            this.reportFooterBand.HeightF = 28F;
            this.reportFooterBand.Name = "reportFooterBand";

            // xrLabelReportFooter
            this.xrLabelReportFooter.Font = new DXFont("Segoe UI", 9F, DXFontStyle.Italic);
            this.xrLabelReportFooter.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4F);
            this.xrLabelReportFooter.Name = "xrLabelReportFooter";
            this.xrLabelReportFooter.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelReportFooter.SizeF = new SizeF(650F, 18F);
            this.xrLabelReportFooter.Text = "End of Report";
            this.xrLabelReportFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            // BaseReportTemplate
            this.Bands.AddRange(new Band[] {
                this.topMarginBand1,
                this.bottomMarginBand1,
                this.reportHeaderBand,
                this.pageHeaderBand,
                this.detailBand1,
                this.pageFooterBand,
                this.reportFooterBand });
            this.Margins = new DXMargins(25, 25, 25, 30);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = DXPaperKind.A4;
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
