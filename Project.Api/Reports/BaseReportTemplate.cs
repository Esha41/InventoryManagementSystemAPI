using DevExpress.Drawing;
using DevExpress.Drawing.Printing;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using System.Drawing;

namespace Ettad.Reporting.Reports
{
    /// <summary>
    /// Base template for all reports created from the frontend.
    /// This template provides a consistent layout with header, detail, and footer bands.
    /// </summary>
    public class BaseReportTemplate : XtraReport
    {
        private TopMarginBand topMarginBand1;
        private DetailBand detailBand1;
        private ReportHeaderBand ReportHeader;
        private ReportFooterBand ReportFooter;
        private XRLabel xrLabel1;
        private BottomMarginBand bottomMarginBand1;

        public BaseReportTemplate()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // detailBand1
            // 
            this.detailBand1.Name = "detailBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.ReportHeader.HeightF = 38.33333F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // ReportFooter
            // 
            this.ReportFooter.HeightF = 47.5F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrLabel1
            // 
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(134.1667F, 5.333328F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(307.5F, 23F);
            this.xrLabel1.Text = "This is Base report template";
            // 
            // BaseReportTemplate
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.detailBand1,
            this.bottomMarginBand1,
            this.ReportHeader,
            this.ReportFooter});
            this.Version = "25.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        /// <summary>
        /// Sets the report title
        /// </summary>
        public void SetReportTitle(string title)
        {
            var headerBand = this.Bands["ReportHeader"] as ReportHeaderBand;
            if (headerBand != null && headerBand.Controls.Count > 0)
            {
                var titleLabel = headerBand.Controls[0] as XRLabel;
                if (titleLabel != null)
                {
                    titleLabel.Text = title;
                }
            }
        }

        /// <summary>
        /// Adds a parameter to the report
        /// </summary>
        public void AddParameter(string name, Type type, object defaultValue = null)
        {
            var parameter = new Parameter
            {
                Name = name,
                Type = type,
                Visible = true
            };

            if (defaultValue != null)
            {
                parameter.Value = defaultValue;
            }

            this.Parameters.Add(parameter);
        }
    }
}
