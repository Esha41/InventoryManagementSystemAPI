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
        private XRTable xrTable1;
        private XRTableRow xrTableRow1;
        private XRTableCell xrTableCell1;
        private XRTableCell xrTableCell2;
        private XRTableCell xrTableCell3;
        private XRTableCell xrTableCell9;
        private XRTableCell xrTableCell11;
        private XRTableCell xrTableCell10;
        private XRTableCell xrTableCell12;
        private XRTableCell xrTableCell13;
        private XRTable xrTable2;
        private XRTableRow xrTableRow2;
        private XRTableCell xrTableCell4;
        private XRTableCell xrTableCell5;
        private XRTableCell xrTableCell6;
        private XRTableCell xrTableCell7;
        private XRTableCell xrTableCell8;
        private XRTableCell xrTableCell14;
        private XRTableCell xrTableCell15;
        private XRTableCell xrTableCell16;
        private Parameter Language;
        private XRPictureBox xrPictureBoxLogo;

        public BaseReportTemplate()
        {
            InitializeComponent();
            // Set up logo loading at runtime to avoid serialization issues
            this.reportHeaderBand.BeforePrint += ReportHeaderBand_BeforePrint;
        }

        private void ReportHeaderBand_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Load logo if not already loaded
            if (xrPictureBoxLogo != null && xrPictureBoxLogo.ImageSource == null)
            {
                var logoPath = GetLogoPath();
                if (File.Exists(logoPath))
                {
                 //   xrPictureBoxLogo.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(logoPath);
                }
            }
            
            // Apply language localization
            //ApplyLanguageLocalization();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseReportTemplate));
            DevExpress.DataAccess.Sql.SelectQuery selectQuery21 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column381 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression381 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table21 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column382 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression382 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column383 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression383 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column384 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression384 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column385 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression385 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column386 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression386 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column387 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression387 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column388 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression388 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column389 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression389 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column390 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression390 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column391 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression391 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column392 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression392 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column393 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression393 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column394 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression394 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column395 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression395 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column396 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression396 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column397 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression397 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column398 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression398 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column399 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression399 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column400 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression400 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column401 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression401 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery22 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column402 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression402 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table22 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column403 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression403 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column404 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression404 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column405 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression405 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column406 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression406 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column407 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression407 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column408 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression408 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column409 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression409 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column410 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression410 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column411 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression411 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column412 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression412 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column413 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression413 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column414 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression414 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column415 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression415 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column416 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression416 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column417 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression417 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column418 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression418 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column419 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression419 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column420 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression420 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column421 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression421 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column422 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression422 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column423 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression423 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column424 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression424 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column425 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression425 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column426 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression426 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column427 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression427 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column428 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression428 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery23 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column429 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression429 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table23 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column430 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression430 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column431 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression431 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column432 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression432 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column433 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression433 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column434 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression434 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column435 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression435 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column436 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression436 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column437 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression437 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column438 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression438 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column439 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression439 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column440 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression440 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column441 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression441 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column442 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression442 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column443 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression443 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column444 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression444 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery24 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column445 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression445 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table24 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column446 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression446 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column447 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression447 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column448 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression448 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column449 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression449 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column450 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression450 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column451 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression451 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column452 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression452 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column453 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression453 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column454 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression454 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column455 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression455 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column456 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression456 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings6 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.reportHeaderBand = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBoxLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabelReportTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLineHeaderSeparator = new DevExpress.XtraReports.UI.XRLine();
            this.pageHeaderBand = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.pageFooterBand = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLineFooterSeparator = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabelCompanyName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo = new DevExpress.XtraReports.UI.XRPageInfo();
            this.reportFooterBand = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabelReportFooter = new DevExpress.XtraReports.UI.XRLabel();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Language = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.HeightF = 25F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.HeightF = 0F;
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
            // xrLabel5
            // 
            this.xrLabel5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'تم إنشاؤها بواسطة:\', \'Generated By:\')\n")});
            this.xrLabel5.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(597.9384F, 0F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(79.99999F, 23F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "Generated By:";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(677.9384F, 0F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.UserName;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(99.99998F, 23F);
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(597.9384F, 23.00001F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(180.8333F, 23F);
            this.xrPageInfo1.TextFormatString = "Generated on: {0:dd/MM/yyyy HH:mm}";
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
            this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'ل:\', \'To:\')\n")});
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
            this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'عنوان التقرير:\', \'From:\')")});
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
            this.xrLabelReportTitle.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(?Language == \'ar\', \'عنوان التقرير\', \'Report Title\')")});
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
            this.xrTable2});
            this.pageHeaderBand.HeightF = 28.83326F;
            this.pageHeaderBand.Name = "pageHeaderBand";
            // 
            // xrTable2
            // 
            this.xrTable2.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(778.7717F, 25F);
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6,
            this.xrTableCell7,
            this.xrTableCell8,
            this.xrTableCell14,
            this.xrTableCell15,
            this.xrTableCell16});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Multiline = true;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Text = "Header1";
            this.xrTableCell4.Weight = 1D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Multiline = true;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Text = "Header2";
            this.xrTableCell5.Weight = 1D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Multiline = true;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Text = "Header3";
            this.xrTableCell6.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Multiline = true;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Text = "Header4";
            this.xrTableCell7.Weight = 1D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Multiline = true;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Text = "Header5";
            this.xrTableCell8.Weight = 1D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Multiline = true;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Text = "Header6";
            this.xrTableCell14.Weight = 1D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Multiline = true;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Text = "Header7\r\n";
            this.xrTableCell15.Weight = 1D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Multiline = true;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Text = "Header8\r\n";
            this.xrTableCell16.Weight = 1D;
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.detailBand1.HeightF = 25F;
            this.detailBand1.Name = "detailBand1";
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(778.7717F, 25F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell11,
            this.xrTableCell10,
            this.xrTableCell1,
            this.xrTableCell9,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell12,
            this.xrTableCell13});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Multiline = true;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Text = "{{value}}";
            this.xrTableCell11.Weight = 1D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Multiline = true;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Text = "Column2";
            this.xrTableCell10.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "Column3";
            this.xrTableCell1.Weight = 1D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Multiline = true;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Text = "Column4";
            this.xrTableCell9.Weight = 1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Text = "Column5";
            this.xrTableCell2.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "Column6";
            this.xrTableCell3.Weight = 1D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Multiline = true;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Text = "Column7";
            this.xrTableCell12.Weight = 1D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Multiline = true;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.Text = "Column8";
            this.xrTableCell13.Weight = 1D;
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
            // xrLabel6
            // 
            this.xrLabel6.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8F, DevExpress.Drawing.DXFontStyle.Italic);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(80.83334F, 5.999959F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(605.4384F, 18F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Confidential – For internal use only";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
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
            columnExpression381.ColumnName = "Id";
            table21.Name = "Assets";
            columnExpression381.Table = table21;
            column381.Expression = columnExpression381;
            columnExpression382.ColumnName = "ItemId";
            columnExpression382.Table = table21;
            column382.Expression = columnExpression382;
            columnExpression383.ColumnName = "SerialNumber";
            columnExpression383.Table = table21;
            column383.Expression = columnExpression383;
            columnExpression384.ColumnName = "RFID";
            columnExpression384.Table = table21;
            column384.Expression = columnExpression384;
            columnExpression385.ColumnName = "DepotId";
            columnExpression385.Table = table21;
            column385.Expression = columnExpression385;
            columnExpression386.ColumnName = "CurrentAssignmentId";
            columnExpression386.Table = table21;
            column386.Expression = columnExpression386;
            columnExpression387.ColumnName = "Status";
            columnExpression387.Table = table21;
            column387.Expression = columnExpression387;
            columnExpression388.ColumnName = "AssetTag";
            columnExpression388.Table = table21;
            column388.Expression = columnExpression388;
            columnExpression389.ColumnName = "PurchaseDate";
            columnExpression389.Table = table21;
            column389.Expression = columnExpression389;
            columnExpression390.ColumnName = "WarrantyExpiryDate";
            columnExpression390.Table = table21;
            column390.Expression = columnExpression390;
            columnExpression391.ColumnName = "Condition";
            columnExpression391.Table = table21;
            column391.Expression = columnExpression391;
            columnExpression392.ColumnName = "PurchasePrice";
            columnExpression392.Table = table21;
            column392.Expression = columnExpression392;
            columnExpression393.ColumnName = "Notes";
            columnExpression393.Table = table21;
            column393.Expression = columnExpression393;
            columnExpression394.ColumnName = "CreationDate";
            columnExpression394.Table = table21;
            column394.Expression = columnExpression394;
            columnExpression395.ColumnName = "ModificationDate";
            columnExpression395.Table = table21;
            column395.Expression = columnExpression395;
            columnExpression396.ColumnName = "ModifiedBy";
            columnExpression396.Table = table21;
            column396.Expression = columnExpression396;
            columnExpression397.ColumnName = "CreatedBy";
            columnExpression397.Table = table21;
            column397.Expression = columnExpression397;
            columnExpression398.ColumnName = "IsDeleted";
            columnExpression398.Table = table21;
            column398.Expression = columnExpression398;
            columnExpression399.ColumnName = "DeletionDate";
            columnExpression399.Table = table21;
            column399.Expression = columnExpression399;
            columnExpression400.ColumnName = "DeletedBy";
            columnExpression400.Table = table21;
            column400.Expression = columnExpression400;
            columnExpression401.ColumnName = "IsAssigned";
            columnExpression401.Table = table21;
            column401.Expression = columnExpression401;
            selectQuery21.Columns.Add(column381);
            selectQuery21.Columns.Add(column382);
            selectQuery21.Columns.Add(column383);
            selectQuery21.Columns.Add(column384);
            selectQuery21.Columns.Add(column385);
            selectQuery21.Columns.Add(column386);
            selectQuery21.Columns.Add(column387);
            selectQuery21.Columns.Add(column388);
            selectQuery21.Columns.Add(column389);
            selectQuery21.Columns.Add(column390);
            selectQuery21.Columns.Add(column391);
            selectQuery21.Columns.Add(column392);
            selectQuery21.Columns.Add(column393);
            selectQuery21.Columns.Add(column394);
            selectQuery21.Columns.Add(column395);
            selectQuery21.Columns.Add(column396);
            selectQuery21.Columns.Add(column397);
            selectQuery21.Columns.Add(column398);
            selectQuery21.Columns.Add(column399);
            selectQuery21.Columns.Add(column400);
            selectQuery21.Columns.Add(column401);
            selectQuery21.Name = "Assets";
            selectQuery21.Tables.Add(table21);
            columnExpression402.ColumnName = "Id";
            table22.Name = "AssetHistory";
            columnExpression402.Table = table22;
            column402.Expression = columnExpression402;
            columnExpression403.ColumnName = "AssetId";
            columnExpression403.Table = table22;
            column403.Expression = columnExpression403;
            columnExpression404.ColumnName = "ActionType";
            columnExpression404.Table = table22;
            column404.Expression = columnExpression404;
            columnExpression405.ColumnName = "ActionDate";
            columnExpression405.Table = table22;
            column405.Expression = columnExpression405;
            columnExpression406.ColumnName = "Description";
            columnExpression406.Table = table22;
            column406.Expression = columnExpression406;
            columnExpression407.ColumnName = "PreviousStatus";
            columnExpression407.Table = table22;
            column407.Expression = columnExpression407;
            columnExpression408.ColumnName = "NewStatus";
            columnExpression408.Table = table22;
            column408.Expression = columnExpression408;
            columnExpression409.ColumnName = "PreviousDepartmentId";
            columnExpression409.Table = table22;
            column409.Expression = columnExpression409;
            columnExpression410.ColumnName = "NewDepartmentId";
            columnExpression410.Table = table22;
            column410.Expression = columnExpression410;
            columnExpression411.ColumnName = "PreviousCustodianId";
            columnExpression411.Table = table22;
            column411.Expression = columnExpression411;
            columnExpression412.ColumnName = "NewCustodianId";
            columnExpression412.Table = table22;
            column412.Expression = columnExpression412;
            columnExpression413.ColumnName = "PreviousLocation";
            columnExpression413.Table = table22;
            column413.Expression = columnExpression413;
            columnExpression414.ColumnName = "NewLocation";
            columnExpression414.Table = table22;
            column414.Expression = columnExpression414;
            columnExpression415.ColumnName = "OrderId";
            columnExpression415.Table = table22;
            column415.Expression = columnExpression415;
            columnExpression416.ColumnName = "AssetSupplyId";
            columnExpression416.Table = table22;
            column416.Expression = columnExpression416;
            columnExpression417.ColumnName = "AssetAssignmentId";
            columnExpression417.Table = table22;
            column417.Expression = columnExpression417;
            columnExpression418.ColumnName = "PerformedByUserId";
            columnExpression418.Table = table22;
            column418.Expression = columnExpression418;
            columnExpression419.ColumnName = "PerformedByUserName";
            columnExpression419.Table = table22;
            column419.Expression = columnExpression419;
            columnExpression420.ColumnName = "Notes";
            columnExpression420.Table = table22;
            column420.Expression = columnExpression420;
            columnExpression421.ColumnName = "Metadata";
            columnExpression421.Table = table22;
            column421.Expression = columnExpression421;
            columnExpression422.ColumnName = "CreationDate";
            columnExpression422.Table = table22;
            column422.Expression = columnExpression422;
            columnExpression423.ColumnName = "ModificationDate";
            columnExpression423.Table = table22;
            column423.Expression = columnExpression423;
            columnExpression424.ColumnName = "ModifiedBy";
            columnExpression424.Table = table22;
            column424.Expression = columnExpression424;
            columnExpression425.ColumnName = "CreatedBy";
            columnExpression425.Table = table22;
            column425.Expression = columnExpression425;
            columnExpression426.ColumnName = "IsDeleted";
            columnExpression426.Table = table22;
            column426.Expression = columnExpression426;
            columnExpression427.ColumnName = "DeletionDate";
            columnExpression427.Table = table22;
            column427.Expression = columnExpression427;
            columnExpression428.ColumnName = "DeletedBy";
            columnExpression428.Table = table22;
            column428.Expression = columnExpression428;
            selectQuery22.Columns.Add(column402);
            selectQuery22.Columns.Add(column403);
            selectQuery22.Columns.Add(column404);
            selectQuery22.Columns.Add(column405);
            selectQuery22.Columns.Add(column406);
            selectQuery22.Columns.Add(column407);
            selectQuery22.Columns.Add(column408);
            selectQuery22.Columns.Add(column409);
            selectQuery22.Columns.Add(column410);
            selectQuery22.Columns.Add(column411);
            selectQuery22.Columns.Add(column412);
            selectQuery22.Columns.Add(column413);
            selectQuery22.Columns.Add(column414);
            selectQuery22.Columns.Add(column415);
            selectQuery22.Columns.Add(column416);
            selectQuery22.Columns.Add(column417);
            selectQuery22.Columns.Add(column418);
            selectQuery22.Columns.Add(column419);
            selectQuery22.Columns.Add(column420);
            selectQuery22.Columns.Add(column421);
            selectQuery22.Columns.Add(column422);
            selectQuery22.Columns.Add(column423);
            selectQuery22.Columns.Add(column424);
            selectQuery22.Columns.Add(column425);
            selectQuery22.Columns.Add(column426);
            selectQuery22.Columns.Add(column427);
            selectQuery22.Columns.Add(column428);
            selectQuery22.Name = "AssetHistory";
            selectQuery22.Tables.Add(table22);
            columnExpression429.ColumnName = "Id";
            table23.Name = "Ammunitions";
            columnExpression429.Table = table23;
            column429.Expression = columnExpression429;
            columnExpression430.ColumnName = "AmmunitionType";
            columnExpression430.Table = table23;
            column430.Expression = columnExpression430;
            columnExpression431.ColumnName = "BulletDiameter";
            columnExpression431.Table = table23;
            column431.Expression = columnExpression431;
            columnExpression432.ColumnName = "BulletDiameterUnitId";
            columnExpression432.Table = table23;
            column432.Expression = columnExpression432;
            columnExpression433.ColumnName = "ArmNumber";
            columnExpression433.Table = table23;
            column433.Expression = columnExpression433;
            columnExpression434.ColumnName = "IsLinked";
            columnExpression434.Table = table23;
            column434.Expression = columnExpression434;
            columnExpression435.ColumnName = "Primer";
            columnExpression435.Table = table23;
            column435.Expression = columnExpression435;
            columnExpression436.ColumnName = "TotalWeight";
            columnExpression436.Table = table23;
            column436.Expression = columnExpression436;
            columnExpression437.ColumnName = "NatureOptionId";
            columnExpression437.Table = table23;
            column437.Expression = columnExpression437;
            columnExpression438.ColumnName = "PrimaryPurposId";
            columnExpression438.Table = table23;
            column438.Expression = columnExpression438;
            columnExpression439.ColumnName = "ProjectileColorId";
            columnExpression439.Table = table23;
            column439.Expression = columnExpression439;
            columnExpression440.ColumnName = "ProjectailMaterialId";
            columnExpression440.Table = table23;
            column440.Expression = columnExpression440;
            columnExpression441.ColumnName = "CaseTypeId";
            columnExpression441.Table = table23;
            column441.Expression = columnExpression441;
            columnExpression442.ColumnName = "PropellantId";
            columnExpression442.Table = table23;
            column442.Expression = columnExpression442;
            columnExpression443.ColumnName = "CompatibilityId";
            columnExpression443.Table = table23;
            column443.Expression = columnExpression443;
            columnExpression444.ColumnName = "HazardDivisionId";
            columnExpression444.Table = table23;
            column444.Expression = columnExpression444;
            selectQuery23.Columns.Add(column429);
            selectQuery23.Columns.Add(column430);
            selectQuery23.Columns.Add(column431);
            selectQuery23.Columns.Add(column432);
            selectQuery23.Columns.Add(column433);
            selectQuery23.Columns.Add(column434);
            selectQuery23.Columns.Add(column435);
            selectQuery23.Columns.Add(column436);
            selectQuery23.Columns.Add(column437);
            selectQuery23.Columns.Add(column438);
            selectQuery23.Columns.Add(column439);
            selectQuery23.Columns.Add(column440);
            selectQuery23.Columns.Add(column441);
            selectQuery23.Columns.Add(column442);
            selectQuery23.Columns.Add(column443);
            selectQuery23.Columns.Add(column444);
            selectQuery23.Name = "Ammunitions";
            selectQuery23.Tables.Add(table23);
            columnExpression445.ColumnName = "Id";
            table24.Name = "AllowanceItems";
            columnExpression445.Table = table24;
            column445.Expression = columnExpression445;
            columnExpression446.ColumnName = "ItemId";
            columnExpression446.Table = table24;
            column446.Expression = columnExpression446;
            columnExpression447.ColumnName = "DepartmentId";
            columnExpression447.Table = table24;
            column447.Expression = columnExpression447;
            columnExpression448.ColumnName = "Year";
            columnExpression448.Table = table24;
            column448.Expression = columnExpression448;
            columnExpression449.ColumnName = "Quantity";
            columnExpression449.Table = table24;
            column449.Expression = columnExpression449;
            columnExpression450.ColumnName = "CreationDate";
            columnExpression450.Table = table24;
            column450.Expression = columnExpression450;
            columnExpression451.ColumnName = "ModificationDate";
            columnExpression451.Table = table24;
            column451.Expression = columnExpression451;
            columnExpression452.ColumnName = "ModifiedBy";
            columnExpression452.Table = table24;
            column452.Expression = columnExpression452;
            columnExpression453.ColumnName = "CreatedBy";
            columnExpression453.Table = table24;
            column453.Expression = columnExpression453;
            columnExpression454.ColumnName = "IsDeleted";
            columnExpression454.Table = table24;
            column454.Expression = columnExpression454;
            columnExpression455.ColumnName = "DeletionDate";
            columnExpression455.Table = table24;
            column455.Expression = columnExpression455;
            columnExpression456.ColumnName = "DeletedBy";
            columnExpression456.Table = table24;
            column456.Expression = columnExpression456;
            selectQuery24.Columns.Add(column445);
            selectQuery24.Columns.Add(column446);
            selectQuery24.Columns.Add(column447);
            selectQuery24.Columns.Add(column448);
            selectQuery24.Columns.Add(column449);
            selectQuery24.Columns.Add(column450);
            selectQuery24.Columns.Add(column451);
            selectQuery24.Columns.Add(column452);
            selectQuery24.Columns.Add(column453);
            selectQuery24.Columns.Add(column454);
            selectQuery24.Columns.Add(column455);
            selectQuery24.Columns.Add(column456);
            selectQuery24.Name = "AllowanceItems";
            selectQuery24.Tables.Add(table24);
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            selectQuery21,
            selectQuery22,
            selectQuery23,
            selectQuery24});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // Language
            // 
            this.Language.Description = "Language";
            this.Language.Name = "Language";
            this.Language.ValueInfo = "en";
            staticListLookUpSettings6.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue("ar", "Arabic"));
            staticListLookUpSettings6.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue("en", "English"));
            this.Language.ValueSourceSettings = staticListLookUpSettings6;
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
            this.Margins = new DevExpress.Drawing.DXMargins(25F, 23F, 25F, 0F);
            this.PageHeightF = 1169.291F;
            this.PageWidthF = 826.7717F;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
            this.ParameterPanelLayoutItems.AddRange(new DevExpress.XtraReports.Parameters.ParameterPanelLayoutItem[] {
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.Language, DevExpress.XtraReports.Parameters.Orientation.Horizontal)});
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Language});
            this.Version = "25.2.3";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        public void SetReportTitle(string title)
        {
            if (xrLabelReportTitle != null)
                xrLabelReportTitle.Text = title;
        }

        //public void SetReportSubtitle(string subtitle)
        //{
        //    if (xrLabelReportSubtitle != null)
        //        xrLabelReportSubtitle.Text = subtitle;
        //}

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
