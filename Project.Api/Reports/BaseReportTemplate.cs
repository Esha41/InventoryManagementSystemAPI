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
            DevExpress.DataAccess.Sql.SelectQuery selectQuery77 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column1445 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1445 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table77 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column1446 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1446 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1447 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1447 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1448 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1448 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1449 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1449 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1450 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1450 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1451 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1451 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1452 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1452 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1453 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1453 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1454 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1454 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1455 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1455 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1456 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1456 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1457 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1457 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1458 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1458 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1459 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1459 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1460 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1460 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1461 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1461 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1462 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1462 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1463 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1463 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1464 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1464 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1465 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1465 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery78 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column1466 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1466 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table78 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column1467 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1467 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1468 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1468 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1469 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1469 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1470 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1470 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1471 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1471 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1472 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1472 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1473 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1473 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1474 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1474 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1475 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1475 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1476 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1476 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1477 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1477 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1478 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1478 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1479 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1479 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1480 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1480 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1481 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1481 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1482 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1482 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1483 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1483 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1484 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1484 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1485 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1485 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1486 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1486 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1487 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1487 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1488 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1488 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1489 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1489 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1490 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1490 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1491 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1491 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1492 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1492 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery79 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column1493 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1493 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table79 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column1494 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1494 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1495 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1495 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1496 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1496 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1497 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1497 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1498 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1498 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1499 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1499 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1500 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1500 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1501 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1501 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1502 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1502 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1503 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1503 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1504 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1504 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1505 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1505 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1506 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1506 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1507 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1507 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1508 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1508 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery80 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column1509 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1509 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table80 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column1510 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1510 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1511 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1511 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1512 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1512 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1513 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1513 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1514 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1514 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1515 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1515 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1516 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1516 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1517 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1517 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1518 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1518 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1519 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1519 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1520 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1520 = new DevExpress.DataAccess.Sql.ColumnExpression();
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
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
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
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
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
            this.xrTable2});
            this.pageHeaderBand.HeightF = 28.83326F;
            this.pageHeaderBand.Name = "pageHeaderBand";
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
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
            columnExpression1445.ColumnName = "Id";
            table77.Name = "Assets";
            columnExpression1445.Table = table77;
            column1445.Expression = columnExpression1445;
            columnExpression1446.ColumnName = "ItemId";
            columnExpression1446.Table = table77;
            column1446.Expression = columnExpression1446;
            columnExpression1447.ColumnName = "SerialNumber";
            columnExpression1447.Table = table77;
            column1447.Expression = columnExpression1447;
            columnExpression1448.ColumnName = "RFID";
            columnExpression1448.Table = table77;
            column1448.Expression = columnExpression1448;
            columnExpression1449.ColumnName = "DepotId";
            columnExpression1449.Table = table77;
            column1449.Expression = columnExpression1449;
            columnExpression1450.ColumnName = "CurrentAssignmentId";
            columnExpression1450.Table = table77;
            column1450.Expression = columnExpression1450;
            columnExpression1451.ColumnName = "Status";
            columnExpression1451.Table = table77;
            column1451.Expression = columnExpression1451;
            columnExpression1452.ColumnName = "AssetTag";
            columnExpression1452.Table = table77;
            column1452.Expression = columnExpression1452;
            columnExpression1453.ColumnName = "PurchaseDate";
            columnExpression1453.Table = table77;
            column1453.Expression = columnExpression1453;
            columnExpression1454.ColumnName = "WarrantyExpiryDate";
            columnExpression1454.Table = table77;
            column1454.Expression = columnExpression1454;
            columnExpression1455.ColumnName = "Condition";
            columnExpression1455.Table = table77;
            column1455.Expression = columnExpression1455;
            columnExpression1456.ColumnName = "PurchasePrice";
            columnExpression1456.Table = table77;
            column1456.Expression = columnExpression1456;
            columnExpression1457.ColumnName = "Notes";
            columnExpression1457.Table = table77;
            column1457.Expression = columnExpression1457;
            columnExpression1458.ColumnName = "CreationDate";
            columnExpression1458.Table = table77;
            column1458.Expression = columnExpression1458;
            columnExpression1459.ColumnName = "ModificationDate";
            columnExpression1459.Table = table77;
            column1459.Expression = columnExpression1459;
            columnExpression1460.ColumnName = "ModifiedBy";
            columnExpression1460.Table = table77;
            column1460.Expression = columnExpression1460;
            columnExpression1461.ColumnName = "CreatedBy";
            columnExpression1461.Table = table77;
            column1461.Expression = columnExpression1461;
            columnExpression1462.ColumnName = "IsDeleted";
            columnExpression1462.Table = table77;
            column1462.Expression = columnExpression1462;
            columnExpression1463.ColumnName = "DeletionDate";
            columnExpression1463.Table = table77;
            column1463.Expression = columnExpression1463;
            columnExpression1464.ColumnName = "DeletedBy";
            columnExpression1464.Table = table77;
            column1464.Expression = columnExpression1464;
            columnExpression1465.ColumnName = "IsAssigned";
            columnExpression1465.Table = table77;
            column1465.Expression = columnExpression1465;
            selectQuery77.Columns.Add(column1445);
            selectQuery77.Columns.Add(column1446);
            selectQuery77.Columns.Add(column1447);
            selectQuery77.Columns.Add(column1448);
            selectQuery77.Columns.Add(column1449);
            selectQuery77.Columns.Add(column1450);
            selectQuery77.Columns.Add(column1451);
            selectQuery77.Columns.Add(column1452);
            selectQuery77.Columns.Add(column1453);
            selectQuery77.Columns.Add(column1454);
            selectQuery77.Columns.Add(column1455);
            selectQuery77.Columns.Add(column1456);
            selectQuery77.Columns.Add(column1457);
            selectQuery77.Columns.Add(column1458);
            selectQuery77.Columns.Add(column1459);
            selectQuery77.Columns.Add(column1460);
            selectQuery77.Columns.Add(column1461);
            selectQuery77.Columns.Add(column1462);
            selectQuery77.Columns.Add(column1463);
            selectQuery77.Columns.Add(column1464);
            selectQuery77.Columns.Add(column1465);
            selectQuery77.Name = "Assets";
            selectQuery77.Tables.Add(table77);
            columnExpression1466.ColumnName = "Id";
            table78.Name = "AssetHistory";
            columnExpression1466.Table = table78;
            column1466.Expression = columnExpression1466;
            columnExpression1467.ColumnName = "AssetId";
            columnExpression1467.Table = table78;
            column1467.Expression = columnExpression1467;
            columnExpression1468.ColumnName = "ActionType";
            columnExpression1468.Table = table78;
            column1468.Expression = columnExpression1468;
            columnExpression1469.ColumnName = "ActionDate";
            columnExpression1469.Table = table78;
            column1469.Expression = columnExpression1469;
            columnExpression1470.ColumnName = "Description";
            columnExpression1470.Table = table78;
            column1470.Expression = columnExpression1470;
            columnExpression1471.ColumnName = "PreviousStatus";
            columnExpression1471.Table = table78;
            column1471.Expression = columnExpression1471;
            columnExpression1472.ColumnName = "NewStatus";
            columnExpression1472.Table = table78;
            column1472.Expression = columnExpression1472;
            columnExpression1473.ColumnName = "PreviousDepartmentId";
            columnExpression1473.Table = table78;
            column1473.Expression = columnExpression1473;
            columnExpression1474.ColumnName = "NewDepartmentId";
            columnExpression1474.Table = table78;
            column1474.Expression = columnExpression1474;
            columnExpression1475.ColumnName = "PreviousCustodianId";
            columnExpression1475.Table = table78;
            column1475.Expression = columnExpression1475;
            columnExpression1476.ColumnName = "NewCustodianId";
            columnExpression1476.Table = table78;
            column1476.Expression = columnExpression1476;
            columnExpression1477.ColumnName = "PreviousLocation";
            columnExpression1477.Table = table78;
            column1477.Expression = columnExpression1477;
            columnExpression1478.ColumnName = "NewLocation";
            columnExpression1478.Table = table78;
            column1478.Expression = columnExpression1478;
            columnExpression1479.ColumnName = "OrderId";
            columnExpression1479.Table = table78;
            column1479.Expression = columnExpression1479;
            columnExpression1480.ColumnName = "AssetSupplyId";
            columnExpression1480.Table = table78;
            column1480.Expression = columnExpression1480;
            columnExpression1481.ColumnName = "AssetAssignmentId";
            columnExpression1481.Table = table78;
            column1481.Expression = columnExpression1481;
            columnExpression1482.ColumnName = "PerformedByUserId";
            columnExpression1482.Table = table78;
            column1482.Expression = columnExpression1482;
            columnExpression1483.ColumnName = "PerformedByUserName";
            columnExpression1483.Table = table78;
            column1483.Expression = columnExpression1483;
            columnExpression1484.ColumnName = "Notes";
            columnExpression1484.Table = table78;
            column1484.Expression = columnExpression1484;
            columnExpression1485.ColumnName = "Metadata";
            columnExpression1485.Table = table78;
            column1485.Expression = columnExpression1485;
            columnExpression1486.ColumnName = "CreationDate";
            columnExpression1486.Table = table78;
            column1486.Expression = columnExpression1486;
            columnExpression1487.ColumnName = "ModificationDate";
            columnExpression1487.Table = table78;
            column1487.Expression = columnExpression1487;
            columnExpression1488.ColumnName = "ModifiedBy";
            columnExpression1488.Table = table78;
            column1488.Expression = columnExpression1488;
            columnExpression1489.ColumnName = "CreatedBy";
            columnExpression1489.Table = table78;
            column1489.Expression = columnExpression1489;
            columnExpression1490.ColumnName = "IsDeleted";
            columnExpression1490.Table = table78;
            column1490.Expression = columnExpression1490;
            columnExpression1491.ColumnName = "DeletionDate";
            columnExpression1491.Table = table78;
            column1491.Expression = columnExpression1491;
            columnExpression1492.ColumnName = "DeletedBy";
            columnExpression1492.Table = table78;
            column1492.Expression = columnExpression1492;
            selectQuery78.Columns.Add(column1466);
            selectQuery78.Columns.Add(column1467);
            selectQuery78.Columns.Add(column1468);
            selectQuery78.Columns.Add(column1469);
            selectQuery78.Columns.Add(column1470);
            selectQuery78.Columns.Add(column1471);
            selectQuery78.Columns.Add(column1472);
            selectQuery78.Columns.Add(column1473);
            selectQuery78.Columns.Add(column1474);
            selectQuery78.Columns.Add(column1475);
            selectQuery78.Columns.Add(column1476);
            selectQuery78.Columns.Add(column1477);
            selectQuery78.Columns.Add(column1478);
            selectQuery78.Columns.Add(column1479);
            selectQuery78.Columns.Add(column1480);
            selectQuery78.Columns.Add(column1481);
            selectQuery78.Columns.Add(column1482);
            selectQuery78.Columns.Add(column1483);
            selectQuery78.Columns.Add(column1484);
            selectQuery78.Columns.Add(column1485);
            selectQuery78.Columns.Add(column1486);
            selectQuery78.Columns.Add(column1487);
            selectQuery78.Columns.Add(column1488);
            selectQuery78.Columns.Add(column1489);
            selectQuery78.Columns.Add(column1490);
            selectQuery78.Columns.Add(column1491);
            selectQuery78.Columns.Add(column1492);
            selectQuery78.Name = "AssetHistory";
            selectQuery78.Tables.Add(table78);
            columnExpression1493.ColumnName = "Id";
            table79.Name = "Ammunitions";
            columnExpression1493.Table = table79;
            column1493.Expression = columnExpression1493;
            columnExpression1494.ColumnName = "AmmunitionType";
            columnExpression1494.Table = table79;
            column1494.Expression = columnExpression1494;
            columnExpression1495.ColumnName = "BulletDiameter";
            columnExpression1495.Table = table79;
            column1495.Expression = columnExpression1495;
            columnExpression1496.ColumnName = "BulletDiameterUnitId";
            columnExpression1496.Table = table79;
            column1496.Expression = columnExpression1496;
            columnExpression1497.ColumnName = "ArmNumber";
            columnExpression1497.Table = table79;
            column1497.Expression = columnExpression1497;
            columnExpression1498.ColumnName = "IsLinked";
            columnExpression1498.Table = table79;
            column1498.Expression = columnExpression1498;
            columnExpression1499.ColumnName = "Primer";
            columnExpression1499.Table = table79;
            column1499.Expression = columnExpression1499;
            columnExpression1500.ColumnName = "TotalWeight";
            columnExpression1500.Table = table79;
            column1500.Expression = columnExpression1500;
            columnExpression1501.ColumnName = "NatureOptionId";
            columnExpression1501.Table = table79;
            column1501.Expression = columnExpression1501;
            columnExpression1502.ColumnName = "PrimaryPurposId";
            columnExpression1502.Table = table79;
            column1502.Expression = columnExpression1502;
            columnExpression1503.ColumnName = "ProjectileColorId";
            columnExpression1503.Table = table79;
            column1503.Expression = columnExpression1503;
            columnExpression1504.ColumnName = "ProjectailMaterialId";
            columnExpression1504.Table = table79;
            column1504.Expression = columnExpression1504;
            columnExpression1505.ColumnName = "CaseTypeId";
            columnExpression1505.Table = table79;
            column1505.Expression = columnExpression1505;
            columnExpression1506.ColumnName = "PropellantId";
            columnExpression1506.Table = table79;
            column1506.Expression = columnExpression1506;
            columnExpression1507.ColumnName = "CompatibilityId";
            columnExpression1507.Table = table79;
            column1507.Expression = columnExpression1507;
            columnExpression1508.ColumnName = "HazardDivisionId";
            columnExpression1508.Table = table79;
            column1508.Expression = columnExpression1508;
            selectQuery79.Columns.Add(column1493);
            selectQuery79.Columns.Add(column1494);
            selectQuery79.Columns.Add(column1495);
            selectQuery79.Columns.Add(column1496);
            selectQuery79.Columns.Add(column1497);
            selectQuery79.Columns.Add(column1498);
            selectQuery79.Columns.Add(column1499);
            selectQuery79.Columns.Add(column1500);
            selectQuery79.Columns.Add(column1501);
            selectQuery79.Columns.Add(column1502);
            selectQuery79.Columns.Add(column1503);
            selectQuery79.Columns.Add(column1504);
            selectQuery79.Columns.Add(column1505);
            selectQuery79.Columns.Add(column1506);
            selectQuery79.Columns.Add(column1507);
            selectQuery79.Columns.Add(column1508);
            selectQuery79.Name = "Ammunitions";
            selectQuery79.Tables.Add(table79);
            columnExpression1509.ColumnName = "Id";
            table80.Name = "AllowanceItems";
            columnExpression1509.Table = table80;
            column1509.Expression = columnExpression1509;
            columnExpression1510.ColumnName = "ItemId";
            columnExpression1510.Table = table80;
            column1510.Expression = columnExpression1510;
            columnExpression1511.ColumnName = "DepartmentId";
            columnExpression1511.Table = table80;
            column1511.Expression = columnExpression1511;
            columnExpression1512.ColumnName = "Year";
            columnExpression1512.Table = table80;
            column1512.Expression = columnExpression1512;
            columnExpression1513.ColumnName = "Quantity";
            columnExpression1513.Table = table80;
            column1513.Expression = columnExpression1513;
            columnExpression1514.ColumnName = "CreationDate";
            columnExpression1514.Table = table80;
            column1514.Expression = columnExpression1514;
            columnExpression1515.ColumnName = "ModificationDate";
            columnExpression1515.Table = table80;
            column1515.Expression = columnExpression1515;
            columnExpression1516.ColumnName = "ModifiedBy";
            columnExpression1516.Table = table80;
            column1516.Expression = columnExpression1516;
            columnExpression1517.ColumnName = "CreatedBy";
            columnExpression1517.Table = table80;
            column1517.Expression = columnExpression1517;
            columnExpression1518.ColumnName = "IsDeleted";
            columnExpression1518.Table = table80;
            column1518.Expression = columnExpression1518;
            columnExpression1519.ColumnName = "DeletionDate";
            columnExpression1519.Table = table80;
            column1519.Expression = columnExpression1519;
            columnExpression1520.ColumnName = "DeletedBy";
            columnExpression1520.Table = table80;
            column1520.Expression = columnExpression1520;
            selectQuery80.Columns.Add(column1509);
            selectQuery80.Columns.Add(column1510);
            selectQuery80.Columns.Add(column1511);
            selectQuery80.Columns.Add(column1512);
            selectQuery80.Columns.Add(column1513);
            selectQuery80.Columns.Add(column1514);
            selectQuery80.Columns.Add(column1515);
            selectQuery80.Columns.Add(column1516);
            selectQuery80.Columns.Add(column1517);
            selectQuery80.Columns.Add(column1518);
            selectQuery80.Columns.Add(column1519);
            selectQuery80.Columns.Add(column1520);
            selectQuery80.Name = "AllowanceItems";
            selectQuery80.Tables.Add(table80);
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            selectQuery77,
            selectQuery78,
            selectQuery79,
            selectQuery80});
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
            // xrTableCell1
            // 
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "Column3";
            this.xrTableCell1.Weight = 1D;
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
            // xrTableCell9
            // 
            this.xrTableCell9.Multiline = true;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Text = "Column4";
            this.xrTableCell9.Weight = 1D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Multiline = true;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Text = "Column2";
            this.xrTableCell10.Weight = 1D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Multiline = true;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Text = "{{value}}";
            this.xrTableCell11.Weight = 1D;
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
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
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
