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
        private XRLine xrLineHeaderSeparator;
        private XRLabel xrLabelPageHeader;
        private XRLabel xrLabelCompanyName;
        private XRPageInfo xrPageInfo;
        private XRLine xrLineFooterSeparator;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private System.ComponentModel.IContainer components;
        private XRLabel xrLabelReportFooter;
        private XRLabel xrLabel1;
        private XRLabel xrLabel2;
        private XRLabel xrLabel4;
        private XRLabel xrLabel3;
        private XRPageInfo xrPageInfo1;
        private XRPageInfo xrPageInfo2;
        private XRLabel xrLabel5;
        private XRLabel xrLabel6;
        private XRPictureBox xrPictureBoxLogo;

        public BaseReportTemplate()
        {
            InitializeComponent();
            // Set up logo loading at runtime to avoid serialization issues
            this.reportHeaderBand.BeforePrint += ReportHeaderBand_BeforePrint;
        }

        private void ReportHeaderBand_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (xrPictureBoxLogo != null && xrPictureBoxLogo.ImageSource == null)
            {
                var logoPath = GetLogoPath();
                if (File.Exists(logoPath))
                {
                    xrPictureBoxLogo.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(logoPath);
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseReportTemplate));
            DevExpress.DataAccess.Sql.SelectQuery selectQuery25 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column457 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression457 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table25 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column458 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression458 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column459 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression459 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column460 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression460 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column461 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression461 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column462 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression462 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column463 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression463 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column464 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression464 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column465 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression465 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column466 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression466 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column467 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression467 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column468 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression468 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column469 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression469 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column470 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression470 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column471 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression471 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column472 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression472 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column473 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression473 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column474 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression474 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column475 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression475 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column476 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression476 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column477 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression477 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery26 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column478 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression478 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table26 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column479 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression479 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column480 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression480 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column481 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression481 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column482 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression482 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column483 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression483 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column484 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression484 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column485 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression485 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column486 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression486 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column487 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression487 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column488 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression488 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column489 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression489 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column490 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression490 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column491 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression491 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column492 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression492 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column493 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression493 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column494 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression494 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column495 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression495 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column496 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression496 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column497 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression497 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column498 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression498 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column499 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression499 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column500 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression500 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column501 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression501 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column502 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression502 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column503 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression503 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column504 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression504 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery27 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column505 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression505 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table27 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column506 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression506 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column507 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression507 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column508 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression508 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column509 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression509 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column510 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression510 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column511 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression511 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column512 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression512 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column513 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression513 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column514 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression514 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column515 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression515 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column516 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression516 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column517 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression517 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column518 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression518 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column519 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression519 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column520 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression520 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery28 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column521 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression521 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table28 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column522 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression522 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column523 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression523 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column524 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression524 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column525 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression525 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column526 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression526 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column527 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression527 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column528 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression528 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column529 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression529 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column530 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression530 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column531 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression531 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column532 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression532 = new DevExpress.DataAccess.Sql.ColumnExpression();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.reportHeaderBand = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBoxLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabelReportTitle = new DevExpress.XtraReports.UI.XRLabel();
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
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.HeightF = 25F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.HeightF = 22.16654F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // reportHeaderBand
            // 
            this.reportHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrPageInfo2,
            this.xrPageInfo1,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1,
            this.xrPictureBoxLogo,
            this.xrLabelReportTitle,
            this.xrLineHeaderSeparator});
            this.reportHeaderBand.HeightF = 133.3334F;
            this.reportHeaderBand.Name = "reportHeaderBand";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new DevExpress.Drawing.DXFont("Times New Roman", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(459.1665F, 43.33333F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(90F, 19.66667F);
            this.xrLabel4.StylePriority.UseFont = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new DevExpress.Drawing.DXFont("Times New Roman", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(305.8333F, 43.33333F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(83.33331F, 19.66667F);
            this.xrLabel3.StylePriority.UseFont = false;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new DevExpress.Drawing.DXFont("Times New Roman", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(422.4999F, 43.33333F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(36.6666F, 19.66667F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "To:";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Times New Roman", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(248.3333F, 43.33333F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(57.49995F, 19.66666F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "From:";
            // 
            // xrPictureBoxLogo
            // 
            this.xrPictureBoxLogo.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.TopLeft;
            this.xrPictureBoxLogo.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBoxLogo.ImageSource"));
            this.xrPictureBoxLogo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPictureBoxLogo.Name = "xrPictureBoxLogo";
            this.xrPictureBoxLogo.SizeF = new System.Drawing.SizeF(170F, 45.50002F);
            this.xrPictureBoxLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrLabelReportTitle
            // 
            this.xrLabelReportTitle.Font = new DevExpress.Drawing.DXFont("Segoe UI", 16F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabelReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(248.3333F, 0F);
            this.xrLabelReportTitle.Multiline = true;
            this.xrLabelReportTitle.Name = "xrLabelReportTitle";
            this.xrLabelReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabelReportTitle.SizeF = new System.Drawing.SizeF(300.8331F, 28F);
            this.xrLabelReportTitle.StylePriority.UseTextAlignment = false;
            this.xrLabelReportTitle.Text = "Report Title";
            this.xrLabelReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLineHeaderSeparator
            // 
            this.xrLineHeaderSeparator.LineWidth = 2F;
            this.xrLineHeaderSeparator.LocationFloat = new DevExpress.Utils.PointFloat(0.8333524F, 129.5834F);
            this.xrLineHeaderSeparator.Name = "xrLineHeaderSeparator";
            this.xrLineHeaderSeparator.SizeF = new System.Drawing.SizeF(778.7717F, 2.083336F);
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
            this.xrLabelPageHeader.SizeF = new System.Drawing.SizeF(778.7717F, 24F);
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
            this.xrLabel6,
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
            this.xrLineFooterSeparator.SizeF = new System.Drawing.SizeF(778.7717F, 2F);
            // 
            // xrLabelCompanyName
            // 
            this.xrLabelCompanyName.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8F);
            this.xrLabelCompanyName.LocationFloat = new DevExpress.Utils.PointFloat(0F, 5.999959F);
            this.xrLabelCompanyName.Name = "xrLabelCompanyName";
            this.xrLabelCompanyName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabelCompanyName.SizeF = new System.Drawing.SizeF(80.83334F, 18F);
            this.xrLabelCompanyName.Text = "ETTAD System";
            this.xrLabelCompanyName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8F);
            this.xrPageInfo.LocationFloat = new DevExpress.Utils.PointFloat(686.2717F, 5.999959F);
            this.xrPageInfo.Name = "xrPageInfo";
            this.xrPageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrPageInfo.SizeF = new System.Drawing.SizeF(92.50006F, 18F);
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
            this.xrLabelReportFooter.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4.000041F);
            this.xrLabelReportFooter.Name = "xrLabelReportFooter";
            this.xrLabelReportFooter.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabelReportFooter.SizeF = new System.Drawing.SizeF(778.7717F, 18F);
            this.xrLabelReportFooter.Text = "End of Report";
            this.xrLabelReportFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "DefaultConnection";
            this.sqlDataSource1.Name = "sqlDataSource1";
            columnExpression457.ColumnName = "Id";
            table25.Name = "Assets";
            columnExpression457.Table = table25;
            column457.Expression = columnExpression457;
            columnExpression458.ColumnName = "ItemId";
            columnExpression458.Table = table25;
            column458.Expression = columnExpression458;
            columnExpression459.ColumnName = "SerialNumber";
            columnExpression459.Table = table25;
            column459.Expression = columnExpression459;
            columnExpression460.ColumnName = "RFID";
            columnExpression460.Table = table25;
            column460.Expression = columnExpression460;
            columnExpression461.ColumnName = "DepotId";
            columnExpression461.Table = table25;
            column461.Expression = columnExpression461;
            columnExpression462.ColumnName = "CurrentAssignmentId";
            columnExpression462.Table = table25;
            column462.Expression = columnExpression462;
            columnExpression463.ColumnName = "Status";
            columnExpression463.Table = table25;
            column463.Expression = columnExpression463;
            columnExpression464.ColumnName = "AssetTag";
            columnExpression464.Table = table25;
            column464.Expression = columnExpression464;
            columnExpression465.ColumnName = "PurchaseDate";
            columnExpression465.Table = table25;
            column465.Expression = columnExpression465;
            columnExpression466.ColumnName = "WarrantyExpiryDate";
            columnExpression466.Table = table25;
            column466.Expression = columnExpression466;
            columnExpression467.ColumnName = "Condition";
            columnExpression467.Table = table25;
            column467.Expression = columnExpression467;
            columnExpression468.ColumnName = "PurchasePrice";
            columnExpression468.Table = table25;
            column468.Expression = columnExpression468;
            columnExpression469.ColumnName = "Notes";
            columnExpression469.Table = table25;
            column469.Expression = columnExpression469;
            columnExpression470.ColumnName = "CreationDate";
            columnExpression470.Table = table25;
            column470.Expression = columnExpression470;
            columnExpression471.ColumnName = "ModificationDate";
            columnExpression471.Table = table25;
            column471.Expression = columnExpression471;
            columnExpression472.ColumnName = "ModifiedBy";
            columnExpression472.Table = table25;
            column472.Expression = columnExpression472;
            columnExpression473.ColumnName = "CreatedBy";
            columnExpression473.Table = table25;
            column473.Expression = columnExpression473;
            columnExpression474.ColumnName = "IsDeleted";
            columnExpression474.Table = table25;
            column474.Expression = columnExpression474;
            columnExpression475.ColumnName = "DeletionDate";
            columnExpression475.Table = table25;
            column475.Expression = columnExpression475;
            columnExpression476.ColumnName = "DeletedBy";
            columnExpression476.Table = table25;
            column476.Expression = columnExpression476;
            columnExpression477.ColumnName = "IsAssigned";
            columnExpression477.Table = table25;
            column477.Expression = columnExpression477;
            selectQuery25.Columns.Add(column457);
            selectQuery25.Columns.Add(column458);
            selectQuery25.Columns.Add(column459);
            selectQuery25.Columns.Add(column460);
            selectQuery25.Columns.Add(column461);
            selectQuery25.Columns.Add(column462);
            selectQuery25.Columns.Add(column463);
            selectQuery25.Columns.Add(column464);
            selectQuery25.Columns.Add(column465);
            selectQuery25.Columns.Add(column466);
            selectQuery25.Columns.Add(column467);
            selectQuery25.Columns.Add(column468);
            selectQuery25.Columns.Add(column469);
            selectQuery25.Columns.Add(column470);
            selectQuery25.Columns.Add(column471);
            selectQuery25.Columns.Add(column472);
            selectQuery25.Columns.Add(column473);
            selectQuery25.Columns.Add(column474);
            selectQuery25.Columns.Add(column475);
            selectQuery25.Columns.Add(column476);
            selectQuery25.Columns.Add(column477);
            selectQuery25.Name = "Assets";
            selectQuery25.Tables.Add(table25);
            columnExpression478.ColumnName = "Id";
            table26.Name = "AssetHistory";
            columnExpression478.Table = table26;
            column478.Expression = columnExpression478;
            columnExpression479.ColumnName = "AssetId";
            columnExpression479.Table = table26;
            column479.Expression = columnExpression479;
            columnExpression480.ColumnName = "ActionType";
            columnExpression480.Table = table26;
            column480.Expression = columnExpression480;
            columnExpression481.ColumnName = "ActionDate";
            columnExpression481.Table = table26;
            column481.Expression = columnExpression481;
            columnExpression482.ColumnName = "Description";
            columnExpression482.Table = table26;
            column482.Expression = columnExpression482;
            columnExpression483.ColumnName = "PreviousStatus";
            columnExpression483.Table = table26;
            column483.Expression = columnExpression483;
            columnExpression484.ColumnName = "NewStatus";
            columnExpression484.Table = table26;
            column484.Expression = columnExpression484;
            columnExpression485.ColumnName = "PreviousDepartmentId";
            columnExpression485.Table = table26;
            column485.Expression = columnExpression485;
            columnExpression486.ColumnName = "NewDepartmentId";
            columnExpression486.Table = table26;
            column486.Expression = columnExpression486;
            columnExpression487.ColumnName = "PreviousCustodianId";
            columnExpression487.Table = table26;
            column487.Expression = columnExpression487;
            columnExpression488.ColumnName = "NewCustodianId";
            columnExpression488.Table = table26;
            column488.Expression = columnExpression488;
            columnExpression489.ColumnName = "PreviousLocation";
            columnExpression489.Table = table26;
            column489.Expression = columnExpression489;
            columnExpression490.ColumnName = "NewLocation";
            columnExpression490.Table = table26;
            column490.Expression = columnExpression490;
            columnExpression491.ColumnName = "OrderId";
            columnExpression491.Table = table26;
            column491.Expression = columnExpression491;
            columnExpression492.ColumnName = "AssetSupplyId";
            columnExpression492.Table = table26;
            column492.Expression = columnExpression492;
            columnExpression493.ColumnName = "AssetAssignmentId";
            columnExpression493.Table = table26;
            column493.Expression = columnExpression493;
            columnExpression494.ColumnName = "PerformedByUserId";
            columnExpression494.Table = table26;
            column494.Expression = columnExpression494;
            columnExpression495.ColumnName = "PerformedByUserName";
            columnExpression495.Table = table26;
            column495.Expression = columnExpression495;
            columnExpression496.ColumnName = "Notes";
            columnExpression496.Table = table26;
            column496.Expression = columnExpression496;
            columnExpression497.ColumnName = "Metadata";
            columnExpression497.Table = table26;
            column497.Expression = columnExpression497;
            columnExpression498.ColumnName = "CreationDate";
            columnExpression498.Table = table26;
            column498.Expression = columnExpression498;
            columnExpression499.ColumnName = "ModificationDate";
            columnExpression499.Table = table26;
            column499.Expression = columnExpression499;
            columnExpression500.ColumnName = "ModifiedBy";
            columnExpression500.Table = table26;
            column500.Expression = columnExpression500;
            columnExpression501.ColumnName = "CreatedBy";
            columnExpression501.Table = table26;
            column501.Expression = columnExpression501;
            columnExpression502.ColumnName = "IsDeleted";
            columnExpression502.Table = table26;
            column502.Expression = columnExpression502;
            columnExpression503.ColumnName = "DeletionDate";
            columnExpression503.Table = table26;
            column503.Expression = columnExpression503;
            columnExpression504.ColumnName = "DeletedBy";
            columnExpression504.Table = table26;
            column504.Expression = columnExpression504;
            selectQuery26.Columns.Add(column478);
            selectQuery26.Columns.Add(column479);
            selectQuery26.Columns.Add(column480);
            selectQuery26.Columns.Add(column481);
            selectQuery26.Columns.Add(column482);
            selectQuery26.Columns.Add(column483);
            selectQuery26.Columns.Add(column484);
            selectQuery26.Columns.Add(column485);
            selectQuery26.Columns.Add(column486);
            selectQuery26.Columns.Add(column487);
            selectQuery26.Columns.Add(column488);
            selectQuery26.Columns.Add(column489);
            selectQuery26.Columns.Add(column490);
            selectQuery26.Columns.Add(column491);
            selectQuery26.Columns.Add(column492);
            selectQuery26.Columns.Add(column493);
            selectQuery26.Columns.Add(column494);
            selectQuery26.Columns.Add(column495);
            selectQuery26.Columns.Add(column496);
            selectQuery26.Columns.Add(column497);
            selectQuery26.Columns.Add(column498);
            selectQuery26.Columns.Add(column499);
            selectQuery26.Columns.Add(column500);
            selectQuery26.Columns.Add(column501);
            selectQuery26.Columns.Add(column502);
            selectQuery26.Columns.Add(column503);
            selectQuery26.Columns.Add(column504);
            selectQuery26.Name = "AssetHistory";
            selectQuery26.Tables.Add(table26);
            columnExpression505.ColumnName = "Id";
            table27.Name = "Ammunitions";
            columnExpression505.Table = table27;
            column505.Expression = columnExpression505;
            columnExpression506.ColumnName = "AmmunitionType";
            columnExpression506.Table = table27;
            column506.Expression = columnExpression506;
            columnExpression507.ColumnName = "BulletDiameter";
            columnExpression507.Table = table27;
            column507.Expression = columnExpression507;
            columnExpression508.ColumnName = "BulletDiameterUnitId";
            columnExpression508.Table = table27;
            column508.Expression = columnExpression508;
            columnExpression509.ColumnName = "ArmNumber";
            columnExpression509.Table = table27;
            column509.Expression = columnExpression509;
            columnExpression510.ColumnName = "IsLinked";
            columnExpression510.Table = table27;
            column510.Expression = columnExpression510;
            columnExpression511.ColumnName = "Primer";
            columnExpression511.Table = table27;
            column511.Expression = columnExpression511;
            columnExpression512.ColumnName = "TotalWeight";
            columnExpression512.Table = table27;
            column512.Expression = columnExpression512;
            columnExpression513.ColumnName = "NatureOptionId";
            columnExpression513.Table = table27;
            column513.Expression = columnExpression513;
            columnExpression514.ColumnName = "PrimaryPurposId";
            columnExpression514.Table = table27;
            column514.Expression = columnExpression514;
            columnExpression515.ColumnName = "ProjectileColorId";
            columnExpression515.Table = table27;
            column515.Expression = columnExpression515;
            columnExpression516.ColumnName = "ProjectailMaterialId";
            columnExpression516.Table = table27;
            column516.Expression = columnExpression516;
            columnExpression517.ColumnName = "CaseTypeId";
            columnExpression517.Table = table27;
            column517.Expression = columnExpression517;
            columnExpression518.ColumnName = "PropellantId";
            columnExpression518.Table = table27;
            column518.Expression = columnExpression518;
            columnExpression519.ColumnName = "CompatibilityId";
            columnExpression519.Table = table27;
            column519.Expression = columnExpression519;
            columnExpression520.ColumnName = "HazardDivisionId";
            columnExpression520.Table = table27;
            column520.Expression = columnExpression520;
            selectQuery27.Columns.Add(column505);
            selectQuery27.Columns.Add(column506);
            selectQuery27.Columns.Add(column507);
            selectQuery27.Columns.Add(column508);
            selectQuery27.Columns.Add(column509);
            selectQuery27.Columns.Add(column510);
            selectQuery27.Columns.Add(column511);
            selectQuery27.Columns.Add(column512);
            selectQuery27.Columns.Add(column513);
            selectQuery27.Columns.Add(column514);
            selectQuery27.Columns.Add(column515);
            selectQuery27.Columns.Add(column516);
            selectQuery27.Columns.Add(column517);
            selectQuery27.Columns.Add(column518);
            selectQuery27.Columns.Add(column519);
            selectQuery27.Columns.Add(column520);
            selectQuery27.Name = "Ammunitions";
            selectQuery27.Tables.Add(table27);
            columnExpression521.ColumnName = "Id";
            table28.Name = "AllowanceItems";
            columnExpression521.Table = table28;
            column521.Expression = columnExpression521;
            columnExpression522.ColumnName = "ItemId";
            columnExpression522.Table = table28;
            column522.Expression = columnExpression522;
            columnExpression523.ColumnName = "DepartmentId";
            columnExpression523.Table = table28;
            column523.Expression = columnExpression523;
            columnExpression524.ColumnName = "Year";
            columnExpression524.Table = table28;
            column524.Expression = columnExpression524;
            columnExpression525.ColumnName = "Quantity";
            columnExpression525.Table = table28;
            column525.Expression = columnExpression525;
            columnExpression526.ColumnName = "CreationDate";
            columnExpression526.Table = table28;
            column526.Expression = columnExpression526;
            columnExpression527.ColumnName = "ModificationDate";
            columnExpression527.Table = table28;
            column527.Expression = columnExpression527;
            columnExpression528.ColumnName = "ModifiedBy";
            columnExpression528.Table = table28;
            column528.Expression = columnExpression528;
            columnExpression529.ColumnName = "CreatedBy";
            columnExpression529.Table = table28;
            column529.Expression = columnExpression529;
            columnExpression530.ColumnName = "IsDeleted";
            columnExpression530.Table = table28;
            column530.Expression = columnExpression530;
            columnExpression531.ColumnName = "DeletionDate";
            columnExpression531.Table = table28;
            column531.Expression = columnExpression531;
            columnExpression532.ColumnName = "DeletedBy";
            columnExpression532.Table = table28;
            column532.Expression = columnExpression532;
            selectQuery28.Columns.Add(column521);
            selectQuery28.Columns.Add(column522);
            selectQuery28.Columns.Add(column523);
            selectQuery28.Columns.Add(column524);
            selectQuery28.Columns.Add(column525);
            selectQuery28.Columns.Add(column526);
            selectQuery28.Columns.Add(column527);
            selectQuery28.Columns.Add(column528);
            selectQuery28.Columns.Add(column529);
            selectQuery28.Columns.Add(column530);
            selectQuery28.Columns.Add(column531);
            selectQuery28.Columns.Add(column532);
            selectQuery28.Name = "AllowanceItems";
            selectQuery28.Tables.Add(table28);
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            selectQuery25,
            selectQuery26,
            selectQuery27,
            selectQuery28});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(1.907349E-05F, 79.08331F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(180.8333F, 23F);
            this.xrPageInfo1.TextFormatString = "Generated on: {0:dd/MM/yyyy HH:mm}";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(80.83334F, 56.0833F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.UserName;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(99.99998F, 23F);
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0.8333524F, 56.0833F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(79.99999F, 23F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "Generated By:";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8F, DevExpress.Drawing.DXFontStyle.Italic);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(80.83334F, 5.999959F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(605.4384F, 23F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Confidential – For internal use only";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
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
            this.Margins = new DevExpress.Drawing.DXMargins(25F, 23F, 25F, 22.16654F);
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

        private string GetLogoPath()
        {
            // Try multiple possible paths for the logo
            var possiblePaths = new List<string>();
            
            // 1. Try relative to base directory (for deployed applications)
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            possiblePaths.Add(Path.Combine(baseDirectory, "wwwroot", "Assets", "logo", "logo.png"));
            
            // 2. Try relative to current directory
            possiblePaths.Add(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "logo", "logo.png"));
            
            // 3. Try absolute path (for development)
            possiblePaths.Add(@"C:\Users\mounira.tech\source\repos\EttadBackEnd\Project.Api\wwwroot\Assets\logo\logo.png");
            
            // 4. Try relative to executing assembly location
            var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            if (!string.IsNullOrEmpty(assemblyLocation))
            {
                var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
                if (!string.IsNullOrEmpty(assemblyDirectory))
                {
                    possiblePaths.Add(Path.Combine(assemblyDirectory, "wwwroot", "Assets", "logo", "logo.png"));
                }
            }
            
            // Return the first path that exists, or the absolute path as fallback
            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }
            
            // If none exist, return the absolute path as fallback (will show error if file doesn't exist)
            return possiblePaths[2];
        }
    }
}
