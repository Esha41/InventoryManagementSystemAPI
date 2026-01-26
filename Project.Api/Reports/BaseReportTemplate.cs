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
            DevExpress.DataAccess.Sql.SelectQuery selectQuery85 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column1597 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1597 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table85 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column1598 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1598 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1599 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1599 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1600 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1600 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1601 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1601 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1602 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1602 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1603 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1603 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1604 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1604 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1605 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1605 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1606 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1606 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1607 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1607 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1608 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1608 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1609 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1609 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1610 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1610 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1611 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1611 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1612 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1612 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1613 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1613 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1614 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1614 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1615 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1615 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1616 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1616 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1617 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1617 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery86 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column1618 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1618 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table86 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column1619 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1619 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1620 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1620 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1621 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1621 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1622 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1622 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1623 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1623 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1624 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1624 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1625 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1625 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1626 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1626 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1627 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1627 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1628 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1628 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1629 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1629 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1630 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1630 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1631 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1631 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1632 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1632 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1633 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1633 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1634 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1634 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1635 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1635 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1636 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1636 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1637 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1637 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1638 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1638 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1639 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1639 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1640 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1640 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1641 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1641 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1642 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1642 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1643 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1643 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1644 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1644 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery87 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column1645 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1645 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table87 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column1646 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1646 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1647 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1647 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1648 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1648 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1649 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1649 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1650 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1650 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1651 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1651 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1652 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1652 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1653 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1653 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1654 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1654 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1655 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1655 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1656 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1656 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1657 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1657 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1658 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1658 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1659 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1659 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1660 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1660 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.SelectQuery selectQuery88 = new DevExpress.DataAccess.Sql.SelectQuery();
            DevExpress.DataAccess.Sql.Column column1661 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1661 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Table table88 = new DevExpress.DataAccess.Sql.Table();
            DevExpress.DataAccess.Sql.Column column1662 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1662 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1663 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1663 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1664 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1664 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1665 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1665 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1666 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1666 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1667 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1667 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1668 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1668 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1669 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1669 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1670 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1670 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1671 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1671 = new DevExpress.DataAccess.Sql.ColumnExpression();
            DevExpress.DataAccess.Sql.Column column1672 = new DevExpress.DataAccess.Sql.Column();
            DevExpress.DataAccess.Sql.ColumnExpression columnExpression1672 = new DevExpress.DataAccess.Sql.ColumnExpression();
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
            this.Language = new DevExpress.XtraReports.Parameters.Parameter();
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
            columnExpression1597.ColumnName = "Id";
            table85.Name = "Assets";
            columnExpression1597.Table = table85;
            column1597.Expression = columnExpression1597;
            columnExpression1598.ColumnName = "ItemId";
            columnExpression1598.Table = table85;
            column1598.Expression = columnExpression1598;
            columnExpression1599.ColumnName = "SerialNumber";
            columnExpression1599.Table = table85;
            column1599.Expression = columnExpression1599;
            columnExpression1600.ColumnName = "RFID";
            columnExpression1600.Table = table85;
            column1600.Expression = columnExpression1600;
            columnExpression1601.ColumnName = "DepotId";
            columnExpression1601.Table = table85;
            column1601.Expression = columnExpression1601;
            columnExpression1602.ColumnName = "CurrentAssignmentId";
            columnExpression1602.Table = table85;
            column1602.Expression = columnExpression1602;
            columnExpression1603.ColumnName = "Status";
            columnExpression1603.Table = table85;
            column1603.Expression = columnExpression1603;
            columnExpression1604.ColumnName = "AssetTag";
            columnExpression1604.Table = table85;
            column1604.Expression = columnExpression1604;
            columnExpression1605.ColumnName = "PurchaseDate";
            columnExpression1605.Table = table85;
            column1605.Expression = columnExpression1605;
            columnExpression1606.ColumnName = "WarrantyExpiryDate";
            columnExpression1606.Table = table85;
            column1606.Expression = columnExpression1606;
            columnExpression1607.ColumnName = "Condition";
            columnExpression1607.Table = table85;
            column1607.Expression = columnExpression1607;
            columnExpression1608.ColumnName = "PurchasePrice";
            columnExpression1608.Table = table85;
            column1608.Expression = columnExpression1608;
            columnExpression1609.ColumnName = "Notes";
            columnExpression1609.Table = table85;
            column1609.Expression = columnExpression1609;
            columnExpression1610.ColumnName = "CreationDate";
            columnExpression1610.Table = table85;
            column1610.Expression = columnExpression1610;
            columnExpression1611.ColumnName = "ModificationDate";
            columnExpression1611.Table = table85;
            column1611.Expression = columnExpression1611;
            columnExpression1612.ColumnName = "ModifiedBy";
            columnExpression1612.Table = table85;
            column1612.Expression = columnExpression1612;
            columnExpression1613.ColumnName = "CreatedBy";
            columnExpression1613.Table = table85;
            column1613.Expression = columnExpression1613;
            columnExpression1614.ColumnName = "IsDeleted";
            columnExpression1614.Table = table85;
            column1614.Expression = columnExpression1614;
            columnExpression1615.ColumnName = "DeletionDate";
            columnExpression1615.Table = table85;
            column1615.Expression = columnExpression1615;
            columnExpression1616.ColumnName = "DeletedBy";
            columnExpression1616.Table = table85;
            column1616.Expression = columnExpression1616;
            columnExpression1617.ColumnName = "IsAssigned";
            columnExpression1617.Table = table85;
            column1617.Expression = columnExpression1617;
            selectQuery85.Columns.Add(column1597);
            selectQuery85.Columns.Add(column1598);
            selectQuery85.Columns.Add(column1599);
            selectQuery85.Columns.Add(column1600);
            selectQuery85.Columns.Add(column1601);
            selectQuery85.Columns.Add(column1602);
            selectQuery85.Columns.Add(column1603);
            selectQuery85.Columns.Add(column1604);
            selectQuery85.Columns.Add(column1605);
            selectQuery85.Columns.Add(column1606);
            selectQuery85.Columns.Add(column1607);
            selectQuery85.Columns.Add(column1608);
            selectQuery85.Columns.Add(column1609);
            selectQuery85.Columns.Add(column1610);
            selectQuery85.Columns.Add(column1611);
            selectQuery85.Columns.Add(column1612);
            selectQuery85.Columns.Add(column1613);
            selectQuery85.Columns.Add(column1614);
            selectQuery85.Columns.Add(column1615);
            selectQuery85.Columns.Add(column1616);
            selectQuery85.Columns.Add(column1617);
            selectQuery85.Name = "Assets";
            selectQuery85.Tables.Add(table85);
            columnExpression1618.ColumnName = "Id";
            table86.Name = "AssetHistory";
            columnExpression1618.Table = table86;
            column1618.Expression = columnExpression1618;
            columnExpression1619.ColumnName = "AssetId";
            columnExpression1619.Table = table86;
            column1619.Expression = columnExpression1619;
            columnExpression1620.ColumnName = "ActionType";
            columnExpression1620.Table = table86;
            column1620.Expression = columnExpression1620;
            columnExpression1621.ColumnName = "ActionDate";
            columnExpression1621.Table = table86;
            column1621.Expression = columnExpression1621;
            columnExpression1622.ColumnName = "Description";
            columnExpression1622.Table = table86;
            column1622.Expression = columnExpression1622;
            columnExpression1623.ColumnName = "PreviousStatus";
            columnExpression1623.Table = table86;
            column1623.Expression = columnExpression1623;
            columnExpression1624.ColumnName = "NewStatus";
            columnExpression1624.Table = table86;
            column1624.Expression = columnExpression1624;
            columnExpression1625.ColumnName = "PreviousDepartmentId";
            columnExpression1625.Table = table86;
            column1625.Expression = columnExpression1625;
            columnExpression1626.ColumnName = "NewDepartmentId";
            columnExpression1626.Table = table86;
            column1626.Expression = columnExpression1626;
            columnExpression1627.ColumnName = "PreviousCustodianId";
            columnExpression1627.Table = table86;
            column1627.Expression = columnExpression1627;
            columnExpression1628.ColumnName = "NewCustodianId";
            columnExpression1628.Table = table86;
            column1628.Expression = columnExpression1628;
            columnExpression1629.ColumnName = "PreviousLocation";
            columnExpression1629.Table = table86;
            column1629.Expression = columnExpression1629;
            columnExpression1630.ColumnName = "NewLocation";
            columnExpression1630.Table = table86;
            column1630.Expression = columnExpression1630;
            columnExpression1631.ColumnName = "OrderId";
            columnExpression1631.Table = table86;
            column1631.Expression = columnExpression1631;
            columnExpression1632.ColumnName = "AssetSupplyId";
            columnExpression1632.Table = table86;
            column1632.Expression = columnExpression1632;
            columnExpression1633.ColumnName = "AssetAssignmentId";
            columnExpression1633.Table = table86;
            column1633.Expression = columnExpression1633;
            columnExpression1634.ColumnName = "PerformedByUserId";
            columnExpression1634.Table = table86;
            column1634.Expression = columnExpression1634;
            columnExpression1635.ColumnName = "PerformedByUserName";
            columnExpression1635.Table = table86;
            column1635.Expression = columnExpression1635;
            columnExpression1636.ColumnName = "Notes";
            columnExpression1636.Table = table86;
            column1636.Expression = columnExpression1636;
            columnExpression1637.ColumnName = "Metadata";
            columnExpression1637.Table = table86;
            column1637.Expression = columnExpression1637;
            columnExpression1638.ColumnName = "CreationDate";
            columnExpression1638.Table = table86;
            column1638.Expression = columnExpression1638;
            columnExpression1639.ColumnName = "ModificationDate";
            columnExpression1639.Table = table86;
            column1639.Expression = columnExpression1639;
            columnExpression1640.ColumnName = "ModifiedBy";
            columnExpression1640.Table = table86;
            column1640.Expression = columnExpression1640;
            columnExpression1641.ColumnName = "CreatedBy";
            columnExpression1641.Table = table86;
            column1641.Expression = columnExpression1641;
            columnExpression1642.ColumnName = "IsDeleted";
            columnExpression1642.Table = table86;
            column1642.Expression = columnExpression1642;
            columnExpression1643.ColumnName = "DeletionDate";
            columnExpression1643.Table = table86;
            column1643.Expression = columnExpression1643;
            columnExpression1644.ColumnName = "DeletedBy";
            columnExpression1644.Table = table86;
            column1644.Expression = columnExpression1644;
            selectQuery86.Columns.Add(column1618);
            selectQuery86.Columns.Add(column1619);
            selectQuery86.Columns.Add(column1620);
            selectQuery86.Columns.Add(column1621);
            selectQuery86.Columns.Add(column1622);
            selectQuery86.Columns.Add(column1623);
            selectQuery86.Columns.Add(column1624);
            selectQuery86.Columns.Add(column1625);
            selectQuery86.Columns.Add(column1626);
            selectQuery86.Columns.Add(column1627);
            selectQuery86.Columns.Add(column1628);
            selectQuery86.Columns.Add(column1629);
            selectQuery86.Columns.Add(column1630);
            selectQuery86.Columns.Add(column1631);
            selectQuery86.Columns.Add(column1632);
            selectQuery86.Columns.Add(column1633);
            selectQuery86.Columns.Add(column1634);
            selectQuery86.Columns.Add(column1635);
            selectQuery86.Columns.Add(column1636);
            selectQuery86.Columns.Add(column1637);
            selectQuery86.Columns.Add(column1638);
            selectQuery86.Columns.Add(column1639);
            selectQuery86.Columns.Add(column1640);
            selectQuery86.Columns.Add(column1641);
            selectQuery86.Columns.Add(column1642);
            selectQuery86.Columns.Add(column1643);
            selectQuery86.Columns.Add(column1644);
            selectQuery86.Name = "AssetHistory";
            selectQuery86.Tables.Add(table86);
            columnExpression1645.ColumnName = "Id";
            table87.Name = "Ammunitions";
            columnExpression1645.Table = table87;
            column1645.Expression = columnExpression1645;
            columnExpression1646.ColumnName = "AmmunitionType";
            columnExpression1646.Table = table87;
            column1646.Expression = columnExpression1646;
            columnExpression1647.ColumnName = "BulletDiameter";
            columnExpression1647.Table = table87;
            column1647.Expression = columnExpression1647;
            columnExpression1648.ColumnName = "BulletDiameterUnitId";
            columnExpression1648.Table = table87;
            column1648.Expression = columnExpression1648;
            columnExpression1649.ColumnName = "ArmNumber";
            columnExpression1649.Table = table87;
            column1649.Expression = columnExpression1649;
            columnExpression1650.ColumnName = "IsLinked";
            columnExpression1650.Table = table87;
            column1650.Expression = columnExpression1650;
            columnExpression1651.ColumnName = "Primer";
            columnExpression1651.Table = table87;
            column1651.Expression = columnExpression1651;
            columnExpression1652.ColumnName = "TotalWeight";
            columnExpression1652.Table = table87;
            column1652.Expression = columnExpression1652;
            columnExpression1653.ColumnName = "NatureOptionId";
            columnExpression1653.Table = table87;
            column1653.Expression = columnExpression1653;
            columnExpression1654.ColumnName = "PrimaryPurposId";
            columnExpression1654.Table = table87;
            column1654.Expression = columnExpression1654;
            columnExpression1655.ColumnName = "ProjectileColorId";
            columnExpression1655.Table = table87;
            column1655.Expression = columnExpression1655;
            columnExpression1656.ColumnName = "ProjectailMaterialId";
            columnExpression1656.Table = table87;
            column1656.Expression = columnExpression1656;
            columnExpression1657.ColumnName = "CaseTypeId";
            columnExpression1657.Table = table87;
            column1657.Expression = columnExpression1657;
            columnExpression1658.ColumnName = "PropellantId";
            columnExpression1658.Table = table87;
            column1658.Expression = columnExpression1658;
            columnExpression1659.ColumnName = "CompatibilityId";
            columnExpression1659.Table = table87;
            column1659.Expression = columnExpression1659;
            columnExpression1660.ColumnName = "HazardDivisionId";
            columnExpression1660.Table = table87;
            column1660.Expression = columnExpression1660;
            selectQuery87.Columns.Add(column1645);
            selectQuery87.Columns.Add(column1646);
            selectQuery87.Columns.Add(column1647);
            selectQuery87.Columns.Add(column1648);
            selectQuery87.Columns.Add(column1649);
            selectQuery87.Columns.Add(column1650);
            selectQuery87.Columns.Add(column1651);
            selectQuery87.Columns.Add(column1652);
            selectQuery87.Columns.Add(column1653);
            selectQuery87.Columns.Add(column1654);
            selectQuery87.Columns.Add(column1655);
            selectQuery87.Columns.Add(column1656);
            selectQuery87.Columns.Add(column1657);
            selectQuery87.Columns.Add(column1658);
            selectQuery87.Columns.Add(column1659);
            selectQuery87.Columns.Add(column1660);
            selectQuery87.Name = "Ammunitions";
            selectQuery87.Tables.Add(table87);
            columnExpression1661.ColumnName = "Id";
            table88.Name = "AllowanceItems";
            columnExpression1661.Table = table88;
            column1661.Expression = columnExpression1661;
            columnExpression1662.ColumnName = "ItemId";
            columnExpression1662.Table = table88;
            column1662.Expression = columnExpression1662;
            columnExpression1663.ColumnName = "DepartmentId";
            columnExpression1663.Table = table88;
            column1663.Expression = columnExpression1663;
            columnExpression1664.ColumnName = "Year";
            columnExpression1664.Table = table88;
            column1664.Expression = columnExpression1664;
            columnExpression1665.ColumnName = "Quantity";
            columnExpression1665.Table = table88;
            column1665.Expression = columnExpression1665;
            columnExpression1666.ColumnName = "CreationDate";
            columnExpression1666.Table = table88;
            column1666.Expression = columnExpression1666;
            columnExpression1667.ColumnName = "ModificationDate";
            columnExpression1667.Table = table88;
            column1667.Expression = columnExpression1667;
            columnExpression1668.ColumnName = "ModifiedBy";
            columnExpression1668.Table = table88;
            column1668.Expression = columnExpression1668;
            columnExpression1669.ColumnName = "CreatedBy";
            columnExpression1669.Table = table88;
            column1669.Expression = columnExpression1669;
            columnExpression1670.ColumnName = "IsDeleted";
            columnExpression1670.Table = table88;
            column1670.Expression = columnExpression1670;
            columnExpression1671.ColumnName = "DeletionDate";
            columnExpression1671.Table = table88;
            column1671.Expression = columnExpression1671;
            columnExpression1672.ColumnName = "DeletedBy";
            columnExpression1672.Table = table88;
            column1672.Expression = columnExpression1672;
            selectQuery88.Columns.Add(column1661);
            selectQuery88.Columns.Add(column1662);
            selectQuery88.Columns.Add(column1663);
            selectQuery88.Columns.Add(column1664);
            selectQuery88.Columns.Add(column1665);
            selectQuery88.Columns.Add(column1666);
            selectQuery88.Columns.Add(column1667);
            selectQuery88.Columns.Add(column1668);
            selectQuery88.Columns.Add(column1669);
            selectQuery88.Columns.Add(column1670);
            selectQuery88.Columns.Add(column1671);
            selectQuery88.Columns.Add(column1672);
            selectQuery88.Name = "AllowanceItems";
            selectQuery88.Tables.Add(table88);
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            selectQuery85,
            selectQuery86,
            selectQuery87,
            selectQuery88});
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
            this.xrLabel6.SizeF = new System.Drawing.SizeF(605.4384F, 18F);
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
            // Language
            // 
            this.Language.Description = "Language";
            this.Language.Name = "Language";
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
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Language});
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
