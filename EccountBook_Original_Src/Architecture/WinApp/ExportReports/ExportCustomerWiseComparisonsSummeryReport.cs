using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Reports.Export;
using Architecture.BLL.Reports.General;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ExportReports;

public class ExportCustomerWiseComparisonsSummeryReport : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable dtdetail = new DataTable();

	private DataTable dtForLayout = new DataTable();

	private bool FormInitialized;

	public UserAccount UserAccount = new UserAccount();

	public bool RequestFromAnotherForm;

	public DateTime FromDate;

	public DateTime ToDate;

	public int StockAccountId;

	public int ItemId;

	private IContainer components;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	internal GridEX grdfrm;

	private Label label3;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label9;

	private Label label5;

	private Label label8;

	private Label label7;

	private Label label1;

	private Button BtnShow;

	private Label label6;

	private Label label50;

	private Label label2;

	private Panel panel5;

	private ToolStripButton btnShortcut;

	private ToolStripButton btnRefreshSummary;

	private Label label10;

	private ToolStripButton btn536Report;

	private Label label12;

	private ToolStripButton btnReport;

	private ToolStripButton btnFormat2;

	private ToolStripButton btnPrint536_01;

	private Label label4;

	public UltraCombo CmbSeaPort;

	public DateTimePicker datFromDate;

	public UltraCombo CmbItemType;

	public UltraCombo CmbItemCategory;

	public UltraCombo CmbReportType;

	public UltraCombo CmbItemName;

	public DateTimePicker datToDate;

	public UltraCombo CmbContractNo;

	public UltraCombo CmbCustomerName;

	public UltraCombo CmbInvoiceNo;

	public UltraCombo CmbSalePerson;

	public UltraCombo CmbItemStockAccountHistory;

	private Label label20;

	public ExportCustomerWiseComparisonsSummeryReport(UserAccount pUserAccount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void ExportCustomerWiseComparisonsSummeryReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			FormInitialized = true;
			datFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			ReportTypeBind();
			ComboFill();
			if (!RequestFromAnotherForm)
			{
				datFromDate.Value = DateTime.Now.AddDays(-15.0);
			}
			else
			{
				datFromDate.Value = FromDate;
				datToDate.Value = ToDate;
				if (StockAccountId > 0)
				{
					CmbItemStockAccountHistory.Value = StockAccountId;
				}
				if (ItemId > 0)
				{
					CmbItemName.Value = ItemId;
				}
			}
			GridBind();
			datFromDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportTypeBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		try
		{
			DataTable dt = GeneralReprots.StaticColumnNames(new ReportsParameters
			{
				Activity = "ExportComparisonsSummeryNew"
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbReportType, "ReportId", "ReportName", "Report Name", false);
				((UltraGridBase)CmbReportType).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		try
		{
			DataTable dtItemcategory = new DataTable();
			dtItemcategory.Columns.Add("Id");
			dtItemcategory.Columns.Add("name");
			DataTable dtItem = dtItemcategory.Clone();
			DataTable dtPort = dtItemcategory.Clone();
			DataTable dtCustomer = dtItemcategory.Clone();
			DataTable dtSalePerson = dtItemcategory.Clone();
			DataTable dtItemType = dtItemcategory.Clone();
			DataTable dtContractNo = dtItemcategory.Clone();
			DataTable dtInvoiceNo = dtItemcategory.Clone();
			DataTable Data = ExportVoucher.GetDataForDropDownFromExportInvoiceVoucher(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (Data.Rows.Count > 0)
			{
				for (int i = 0; i < Data.Rows.Count; i++)
				{
					if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "ItemCategory")
					{
						dtItemcategory.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
					}
					else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "ItemType")
					{
						dtItemType.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
					}
					else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "Items")
					{
						dtItem.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
					}
					else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "ContractNo")
					{
						dtContractNo.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
					}
					else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "Customer")
					{
						dtCustomer.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
					}
					else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "SalesPerson")
					{
						dtSalePerson.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
					}
					else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "DestinationPort")
					{
						dtPort.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
					}
					else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "Invoice")
					{
						dtInvoiceNo.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtItemcategory, CmbItemCategory, "Id", "name", "Item Category", ZeroIndex: false);
			((UltraGridBase)CmbItemCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtItemType, CmbItemType, "Id", "name", "Item Type", ZeroIndex: false);
			((UltraGridBase)CmbItemType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtItem, CmbItemName, "Id", "name", "Item Name", ZeroIndex: false);
			DDL.BindDDL(dtCustomer, CmbCustomerName, "Id", "name", "Customer Name", ZeroIndex: false);
			((UltraGridBase)CmbCustomerName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtSalePerson, CmbSalePerson, "Id", "name", "Sales Person", ZeroIndex: false);
			((UltraGridBase)CmbSalePerson).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtContractNo, CmbContractNo, "Id", "name", "Contract No", ZeroIndex: false);
			((UltraGridBase)CmbContractNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtInvoiceNo, CmbInvoiceNo, "Id", "name", "Invoice No", ZeroIndex: false);
			((UltraGridBase)CmbInvoiceNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtPort, CmbSeaPort, "Id", "name", "Port Name", ZeroIndex: false);
			((UltraGridBase)CmbSeaPort).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		try
		{
			btnReport.Visible = false;
			btnFormat2.Visible = false;
			btn536Report.Visible = false;
			btnPrint536_01.Visible = false;
			dtdetail.Clear();
			if (Conversion.ToInt(CmbReportType.Value) == 0 || ((UltraGridBase)CmbReportType).ActiveRow == null)
			{
				throw new Exception("Report Type filed is required");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (datFromDate.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)datFromDate.Value);
			}
			if (datToDate.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)datToDate.Value);
			}
			obj.ItemCategoryId = Conversion.ToInt(CmbItemCategory.Value);
			obj.ItemTypeId = Conversion.ToInt(CmbItemType.Value);
			obj.ItemId = Conversion.ToInt(CmbItemName.Value);
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerName.Value);
			obj.SalePersonId = Conversion.ToInt(CmbSalePerson.Value);
			obj.LcOrderId = Conversion.ToInt(CmbContractNo.Value);
			obj.ExImInvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			obj.DestinationPortId = Conversion.ToInt(CmbSeaPort.Value);
			obj.StockAccountId = Conversion.ToInt(CmbItemStockAccountHistory.Value);
			obj.ReportType = Conversion.ToString((object)((Control)(object)CmbReportType).Text.Trim());
			DataSet ds = new DataSet();
			if (obj.ReportType == "Detail")
			{
				ds = ExportReports.ExportSaleDetailReport(obj);
				btnPrint536_01.Visible = true;
			}
			else
			{
				ds = ExportReports.ExImInvoice_ExportComparisonsSummery_Report(obj);
				if (obj.ReportType == "Customer And Item Wise Comparison" || obj.ReportType == "Item And Customer Wise Comparison")
				{
					btnReport.Visible = true;
					btnFormat2.Visible = true;
					btn536Report.Visible = false;
				}
				else if (obj.ReportType != "Detail")
				{
					btnReport.Visible = false;
					btnFormat2.Visible = false;
					btn536Report.Visible = true;
				}
			}
			dtdetail = ds?.Tables?[0]?.Copy();
			if (dtdetail.Rows.Count > 0)
			{
				((Control)(object)grdfrm).Name = obj.ReportType;
				DatatableHelper.ReplaceCompLogoColumn(dtdetail, clsGlobalVariables.dtCompLogo);
				if (obj.ReportType != "Detail")
				{
					DataTable dtcol = new DataTable();
					dtcol.Columns.Add("GroupCaption", typeof(string));
					dtcol.Columns.Add("GroupName", typeof(string));
					bool hasSecondGroup = obj.ReportType == "Item And Customer Wise Comparison" || obj.ReportType == "Customer And Item Wise Comparison";
					if (hasSecondGroup)
					{
						dtcol.Columns.Add("SecondGroupCaption", typeof(string));
						dtcol.Columns.Add("SecondGroupName", typeof(string));
					}
					dtcol.Columns.Add("MTons", typeof(decimal));
					dtcol.Columns.Add("PrctExport", typeof(decimal));
					dtcol.Columns.Add("FcyCode", typeof(string));
					dtcol.Columns.Add("AvgPrice", typeof(decimal));
					dtcol.Columns.Add("Fcy_Amount", typeof(decimal));
					dtcol.Columns.Add("FcrAvgRate", typeof(decimal));
					dtcol.Columns.Add("PKR_Amount", typeof(decimal));
					dtForLayout = dtcol.Clone();
					dtcol.Rows.Clear();
					foreach (DataRow row in dtdetail.Rows)
					{
						DataRow newRow = dtcol.NewRow();
						if (hasSecondGroup)
						{
							newRow["GroupCaption"] = row["FirstGroupCaption"];
							newRow["GroupName"] = row["FirstGroupName"];
							newRow["SecondGroupCaption"] = row["SecondGroupCaption"];
							newRow["SecondGroupName"] = row["SecondeGroupName"];
						}
						else
						{
							newRow["GroupCaption"] = row["GroupCaption"];
							newRow["GroupName"] = row["GroupName"];
						}
						newRow["MTons"] = row["MTons"];
						newRow["PrctExport"] = row["PrctExport"];
						newRow["FcyCode"] = row["FcyCode"];
						newRow["AvgPrice"] = row["AvgPrice"];
						newRow["Fcy_Amount"] = row["Fcy_Amount"];
						newRow["FcrAvgRate"] = row["FcrAvgRate"];
						newRow["PKR_Amount"] = row["PKR_Amount"];
						dtcol.Rows.Add(newRow);
					}
					dtForLayout = dtcol.Copy();
					grdfrm.DataSource = dtcol;
					grdfrm.RetrieveStructure();
					GridSetting();
					return;
				}
				DataTable dt = new DataTable();
				dt.Columns.Add("VoucherDate", typeof(DateTime));
				dt.Columns.Add("VoucherNo", typeof(int));
				dt.Columns.Add("InvoiceDocTypeId", typeof(int));
				dt.Columns.Add("ExImInvoiceId", typeof(int));
				dt.Columns.Add("InvoiceNo", typeof(string));
				dt.Columns.Add("InvoiceDate", typeof(DateTime));
				dt.Columns.Add("ContractId", typeof(int));
				dt.Columns.Add("ContractNo", typeof(string));
				dt.Columns.Add("ContractDate", typeof(DateTime));
				dt.Columns.Add("EFormNo", typeof(string));
				dt.Columns.Add("EFormDate", typeof(DateTime));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("JobLot", typeof(string));
				dt.Columns.Add("PackingType", typeof(string));
				dt.Columns.Add("Qty", typeof(decimal));
				dt.Columns.Add("MTons", typeof(decimal));
				dt.Columns.Add("NoOfContainers", typeof(int));
				dt.Columns.Add("PackingSize", typeof(string));
				dt.Columns.Add("RatePrice", typeof(decimal));
				dt.Columns.Add("FcyCode", typeof(string));
				dt.Columns.Add("RateUom", typeof(string));
				dt.Columns.Add("FcyAmount", typeof(decimal));
				dt.Columns.Add("ExchangeRate", typeof(decimal));
				dt.Columns.Add("LcyAmount", typeof(decimal));
				dt.Columns.Add("PaymentTerms", typeof(string));
				dt.Columns.Add("DestinationPort", typeof(string));
				dt.Columns.Add("BLNumber", typeof(string));
				dt.Columns.Add("ContainerNos", typeof(string));
				dt.Columns.Add("DocStatus", typeof(string));
				dt.Columns.Add("PaymentStatus", typeof(string));
				dt.Columns.Add("ExImFarmingTypes", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dtForLayout = dt.Clone();
				foreach (DataRow dr in dtdetail.Rows)
				{
					DataRow newRow2 = dt.NewRow();
					newRow2["VoucherDate"] = dr["DocDate"];
					newRow2["VoucherNo"] = dr["DocNo"];
					newRow2["InvoiceDocTypeId"] = dr["InvoiceDocTypeId"];
					newRow2["ExImInvoiceId"] = dr["ExImInvoiceId"];
					newRow2["InvoiceNo"] = dr["InvoiceNo"];
					newRow2["InvoiceDate"] = dr["InvoiceDate"];
					newRow2["ContractId"] = dr["ContractId"];
					newRow2["ContractNo"] = dr["ContractNo"];
					newRow2["ContractDate"] = dr["ContractDate"];
					newRow2["EFormNo"] = dr["EFormNo"];
					newRow2["EFormDate"] = dr["EFormDate"];
					newRow2["CustomerName"] = dr["CustomerName"];
					newRow2["ItemId"] = dr["ItemId"];
					newRow2["ItemName"] = dr["ItemName"];
					newRow2["JobLot"] = dr["JobLotCode"];
					newRow2["PackingType"] = dr["PackingType"];
					newRow2["Qty"] = dr["Qty"];
					newRow2["MTons"] = dr["MTons"];
					newRow2["NoOfContainers"] = dr["NoOfContainers"];
					newRow2["PackingSize"] = dr["PackingSize"];
					newRow2["RatePrice"] = dr["RatePrice"];
					newRow2["FcyCode"] = dr["CurrencySymbol"];
					newRow2["RateUom"] = dr["RateUom"];
					newRow2["FcyAmount"] = dr["Fcy_Amount"];
					newRow2["ExchangeRate"] = dr["ExchangeRate"];
					newRow2["LcyAmount"] = dr["Lcy_Amount"];
					newRow2["PaymentTerms"] = dr["PaymentTerms"];
					newRow2["DestinationPort"] = dr["DestinationPort"];
					newRow2["BLNumber"] = dr["BLNumber"];
					newRow2["ContainerNos"] = dr["ContainerNos"];
					newRow2["DocStatus"] = dr["DocStatus"];
					newRow2["PaymentStatus"] = dr["PaymentStatus"];
					newRow2["ExImFarmingTypes"] = dr["ExImFarmingTypes"];
					newRow2["NoOfAttachments"] = dr["NoOfAttachments"];
					dt.Rows.Add(newRow2);
				}
				dtForLayout = dt.Copy();
				grdfrm.DataSource = dt;
				grdfrm.RetrieveStructure();
				GridSettingDetail();
			}
			else
			{
				grdfrm.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			grdfrm.GroupByBoxVisible = false;
			grdfrm.RootTable.Columns["GroupName"].Caption = Conversion.ToString(grdfrm.CurrentRow.Cells["GroupCaption"].Value);
			grdfrm.RootTable.Columns["GroupCaption"].Visible = false;
			if (Conversion.ToString((object)((Control)(object)CmbReportType).Text) == "Item And Customer Wise Comparison" || Conversion.ToString((object)((Control)(object)CmbReportType).Text) == "Customer And Item Wise Comparison")
			{
				((JanusCollectionBase)grdfrm.RootTable.Groups).Clear();
				grdfrm.RootTable.Columns["GroupName"].Visible = false;
				grdfrm.RootTable.Columns["SecondGroupCaption"].Visible = false;
				grdfrm.RootTable.Columns["SecondGroupName"].Caption = Conversion.ToString(grdfrm.CurrentRow.Cells["SecondGroupCaption"].Value);
				grdfrm.RootTable.Columns["SecondGroupName"].Width = 350;
				grdfrm.RootTable.Groups.Add("GroupName");
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
				grdfrm.RootTable.Columns["SecondGroupCaption"].Width = Constants.InventoryConstants.SupplierName;
				grdfrm.RootTable.Columns["SecondGroupName"].Width = Constants.InventoryConstants.SupplierName;
			}
			CommonServices.GridWrappingAndColumnSettings(grdfrm, 2);
			grdfrm.RootTable.Columns["GroupName"].Width = Constants.InventoryConstants.StringConstant + 50;
			grdfrm.RootTable.Columns["MTons"].Width = Constants.InventoryConstants.WeightConstant;
			grdfrm.RootTable.Columns["PrctExport"].Width = Constants.InventoryConstants.PercentOfTotal;
			grdfrm.RootTable.Columns["FcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			grdfrm.RootTable.Columns["AvgPrice"].Width = Constants.InventoryConstants.ExRateConstant;
			grdfrm.RootTable.Columns["Fcy_Amount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdfrm.RootTable.Columns["FcrAvgRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdfrm.RootTable.Columns["PKR_Amount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			((Control)(object)grdfrm).Refresh();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingDetail()
	{
		try
		{
			((JanusCollectionBase)grdfrm.RootTable.Groups).Clear();
			grdfrm.GroupByBoxVisible = true;
			grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			grdfrm.RootTable.Columns["InvoiceDocTypeId"].Visible = false;
			grdfrm.RootTable.Columns["ExImInvoiceId"].Visible = false;
			grdfrm.RootTable.Columns["ContractId"].Visible = false;
			grdfrm.RootTable.Columns["ItemId"].Visible = false;
			grdfrm.RootTable.Columns["JobLot"].Visible = false;
			grdfrm.RootTable.Columns["PackingType"].Visible = false;
			grdfrm.RootTable.Columns["BLNumber"].Caption = "BL Number";
			grdfrm.RootTable.Columns["DocStatus"].Caption = "Document Status";
			grdfrm.RootTable.Columns["ExImFarmingTypes"].Caption = "Farming Types";
			grdfrm.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["FcyCode"].TextAlignment = (TextAlignment)2;
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 3, null, null, null, ColumnSetting: false);
			grdfrm.RootTable.Columns["VoucherDate"].Width = Constants.ExportConstants.DateConstant + 15;
			grdfrm.RootTable.Columns["VoucherNo"].Width = Constants.ExportConstants.DocNoConstant;
			grdfrm.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdfrm.RootTable.Columns["InvoiceDate"].Width = Constants.ExportConstants.DateConstant + 15;
			grdfrm.RootTable.Columns["ContractNo"].Width = Constants.ExportConstants.ContractNoConstant;
			grdfrm.RootTable.Columns["ContractDate"].Width = Constants.ExportConstants.DateConstant + 15;
			grdfrm.RootTable.Columns["EFormNo"].Width = Constants.ExportConstants.EFormNoConstant + 50;
			grdfrm.RootTable.Columns["EFormDate"].Width = Constants.ExportConstants.DateConstant + 15;
			grdfrm.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.PartyName;
			grdfrm.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.ItemNameConstants - 30;
			grdfrm.RootTable.Columns["JobLot"].Width = Constants.ExportConstants.JobLotConstant;
			grdfrm.RootTable.Columns["PackingType"].Width = Constants.ExportConstants.PackingTypeConstant;
			grdfrm.RootTable.Columns["Qty"].Width = Constants.ExportConstants.QtyConstant;
			grdfrm.RootTable.Columns["MTons"].Width = Constants.ExportConstants.WeightConstant;
			grdfrm.RootTable.Columns["NoOfContainers"].Width = Constants.ExportConstants.NoOfContainers;
			grdfrm.RootTable.Columns["PackingSize"].Width = Constants.ExportConstants.UomConstant;
			grdfrm.RootTable.Columns["RatePrice"].Width = Constants.ExportConstants.ExRateConstant;
			grdfrm.RootTable.Columns["FcyCode"].Width = Constants.ExportConstants.FcyConstant;
			grdfrm.RootTable.Columns["RateUom"].Width = Constants.ExportConstants.UomConstant;
			grdfrm.RootTable.Columns["FcyAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["ExchangeRate"].Width = Constants.ExportConstants.ExRateConstant;
			grdfrm.RootTable.Columns["LcyAmount"].Width = Constants.ExportConstants.LcyAmountConstant;
			grdfrm.RootTable.Columns["PaymentTerms"].Width = Constants.ExportConstants.TermConstant + 30;
			grdfrm.RootTable.Columns["DestinationPort"].Width = Constants.ExportConstants.PortConstant;
			grdfrm.RootTable.Columns["BLNumber"].Width = Constants.ExportConstants.BLNumberConstant + 20;
			grdfrm.RootTable.Columns["ContainerNos"].Width = Constants.ExportConstants.ContainerNo;
			grdfrm.RootTable.Columns["DocStatus"].Width = Constants.ExportConstants.StatusConstant + 50;
			grdfrm.RootTable.Columns["PaymentStatus"].Width = Constants.ExportConstants.StatusConstant;
			grdfrm.RootTable.Columns["ExImFarmingTypes"].Width = Constants.ExportConstants.PartyName;
			grdfrm.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdfrm.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["ContractNo"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["ContainerNos"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdfrm.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdfrm.RootTable.Columns["MTons"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["MTons"].FormatString = "#,##0.###";
			grdfrm.RootTable.Columns["MTons"].TotalFormatString = "#,##0.###";
			grdfrm.RootTable.Columns["RatePrice"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["RatePrice"].FormatString = "#,##0.###";
			grdfrm.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdfrm.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdfrm.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ExchangeRate"].FormatString = "#,##0.###";
			grdfrm.RootTable.Columns["LcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["LcyAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["LcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["LcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			((Control)(object)grdfrm).Refresh();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			((Control)(object)CmbItemCategory).Text = string.Empty;
			((Control)(object)CmbItemType).Text = string.Empty;
			((Control)(object)CmbCustomerName).Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbContractNo).Text = string.Empty;
			((Control)(object)CmbSeaPort).Text = string.Empty;
			datToDate.Focus();
			if (((UltraGridBase)CmbReportType).DataSource != null)
			{
				((UltraGridBase)CmbReportType).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshSummary_Click(object sender, EventArgs e)
	{
		try
		{
			ComboFill();
			ReportTypeBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportCustomerWiseComparisonsSummeryReport_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdfrm, dtForLayout);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReport_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtdetail == null || dtdetail.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtdetail, "534-ExportComparisonSummaryByCustomer&ItemReport.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnFormat2_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtdetail == null || dtdetail.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@LoginUserName", UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(dtdetail, "535-ExportComparisonSummaryByCustomer&ItemReport.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn536Report_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtdetail == null || dtdetail.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtdetail, "536-ExportCustomerWiseComparisonReport.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportCustomerWiseComparisonsSummeryReport_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Hide();
			}
			else if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshSummary_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				BtnShow_Click(null, null);
			}
			else if ((e.Control && e.KeyCode == Keys.P) || (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)))
			{
				if (Conversion.ToString((object)((Control)(object)CmbReportType).Text) == "Item And Customer Wise Comparison" || Conversion.ToString((object)((Control)(object)CmbReportType).Text) == "Customer And Item Wise Comparison")
				{
					btnReport_Click(null, null);
				}
				else if (Conversion.ToString((object)((Control)(object)CmbReportType).Text) != "Detail")
				{
					btn536Report_Click(null, null);
				}
				else
				{
					btnPrint536_01_Click(null, null);
				}
			}
			else if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2))
			{
				if (Conversion.ToString((object)((Control)(object)CmbReportType).Text) == "Item And Customer Wise Comparison" || Conversion.ToString((object)((Control)(object)CmbReportType).Text) == "Customer And Item Wise Comparison")
				{
					btnFormat2_Click(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				datFromDate.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				datFromDate.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdfrm).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcutKeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint536_01_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtdetail == null || dtdetail.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtdetail, "536_01_ExportSaleDetailReport.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "InvoiceNo")
				{
					CommonServices.CommercialInvoiceSlip521(Conversion.ToInt(r.Cells["ExImInvoiceId"].Value));
				}
				else if (e.Column.Key == "ContractNo")
				{
					int ContractId = Conversion.ToInt(r.Cells["ContractId"].Value);
					frmContractDetailByContractId obj = new frmContractDetailByContractId(UserAccount);
					obj.ContractId = ContractId;
					obj.Show();
				}
				else if (e.Column.Key == "ContainerNos")
				{
					CommonServices.BookingInfoSlip560_AgainstInvoice(Conversion.ToInt(r.Cells["ExImInvoiceId"].Value));
				}
				else if (e.Column.Key == "NoOfAttachments")
				{
					int id = Conversion.ToInt(r.Cells["ExImInvoiceId"].Value);
					int InvoiceDocumentTypeId = Conversion.ToInt(r.Cells["InvoiceDocTypeId"].Value);
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, InvoiceDocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Expected O, but got Unknown
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Expected O, but got Unknown
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance28 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.ExportCustomerWiseComparisonsSummeryReport));
		this.grdfrm = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.CmbSalePerson = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbInvoiceNo = new UltraCombo();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.CmbSeaPort = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbItemType = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbItemCategory = new UltraCombo();
		this.CmbReportType = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.BtnShow = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbContractNo = new UltraCombo();
		this.label50 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbCustomerName = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshSummary = new System.Windows.Forms.ToolStripButton();
		this.btnReport = new System.Windows.Forms.ToolStripButton();
		this.btnFormat2 = new System.Windows.Forms.ToolStripButton();
		this.btn536Report = new System.Windows.Forms.ToolStripButton();
		this.btnPrint536_01 = new System.Windows.Forms.ToolStripButton();
		this.btnShortcut = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbItemStockAccountHistory = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSeaPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReportType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).BeginInit();
		this.panel2.SuspendLayout();
		this.tsItemType.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemStockAccountHistory).BeginInit();
		base.SuspendLayout();
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 193);
		((System.Windows.Forms.Control)(object)this.grdfrm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(1128, 318);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 1;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemStockAccountHistory);
		this.panel3.Controls.Add(this.label20);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalePerson);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNo);
		this.panel3.Controls.Add(this.datFromDate);
		this.panel3.Controls.Add(this.datToDate);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSeaPort);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemType);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategory);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReportType);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel3.Controls.Add(this.BtnShow);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractNo);
		this.panel3.Controls.Add(this.label50);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerName);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 53);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1128, 117);
		this.panel3.TabIndex = 0;
		this.CmbSalePerson.AllowNull = (DefaultableBoolean)1;
		this.CmbSalePerson.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalePerson.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalePerson.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSalePerson.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Location = new System.Drawing.Point(465, 32);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Name = "CmbSalePerson";
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Size = new System.Drawing.Size(248, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).TabIndex = 6;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(368, 38);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(73, 15);
		this.label4.TabIndex = 77;
		this.label4.Text = "Sales Person";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(368, 92);
		this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(65, 15);
		this.label12.TabIndex = 75;
		this.label12.Text = "Invoice No";
		this.CmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Location = new System.Drawing.Point(465, 86);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Name = "CmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Size = new System.Drawing.Size(248, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TabIndex = 8;
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(101, 7);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.ShowCheckBox = true;
		this.datFromDate.Size = new System.Drawing.Size(131, 23);
		this.datFromDate.TabIndex = 0;
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(233, 7);
		this.datToDate.Name = "datToDate";
		this.datToDate.ShowCheckBox = true;
		this.datToDate.Size = new System.Drawing.Size(131, 23);
		this.datToDate.TabIndex = 1;
		this.CmbSeaPort.AllowNull = (DefaultableBoolean)1;
		this.CmbSeaPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSeaPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSeaPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSeaPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbSeaPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSeaPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSeaPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSeaPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSeaPort).Location = new System.Drawing.Point(813, 5);
		((System.Windows.Forms.Control)(object)this.CmbSeaPort).Name = "CmbSeaPort";
		((System.Windows.Forms.Control)(object)this.CmbSeaPort).Size = new System.Drawing.Size(248, 26);
		((System.Windows.Forms.Control)(object)this.CmbSeaPort).TabIndex = 9;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(716, 11);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(93, 15);
		this.label9.TabIndex = 71;
		this.label9.Text = "Destination Port";
		this.CmbItemType.AllowNull = (DefaultableBoolean)1;
		this.CmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance100).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Location = new System.Drawing.Point(101, 59);
		((System.Windows.Forms.Control)(object)this.CmbItemType).Name = "CmbItemType";
		((System.Windows.Forms.Control)(object)this.CmbItemType).Size = new System.Drawing.Size(263, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemType).TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(716, 65);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(70, 15);
		this.label5.TabIndex = 57;
		this.label5.Text = "Report Type";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(7, 65);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(60, 15);
		this.label8.TabIndex = 68;
		this.label8.Text = "Item Type";
		this.CmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Location = new System.Drawing.Point(101, 32);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Name = "CmbItemCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Size = new System.Drawing.Size(263, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).TabIndex = 2;
		this.CmbReportType.AllowNull = (DefaultableBoolean)1;
		this.CmbReportType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReportType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReportType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbReportType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReportType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReportType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReportType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.CmbReportType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReportType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbReportType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReportType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbReportType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReportType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReportType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReportType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbReportType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReportType).Location = new System.Drawing.Point(813, 59);
		((System.Windows.Forms.Control)(object)this.CmbReportType).Name = "CmbReportType";
		((System.Windows.Forms.Control)(object)this.CmbReportType).Size = new System.Drawing.Size(248, 26);
		((System.Windows.Forms.Control)(object)this.CmbReportType).TabIndex = 11;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(7, 38);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(82, 15);
		this.label7.TabIndex = 67;
		this.label7.Text = "Item Category";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 11);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(91, 15);
		this.label1.TabIndex = 58;
		this.label1.Text = "From && To Date";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(101, 86);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(263, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 4;
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(1002, 86);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(59, 26);
		this.BtnShow.TabIndex = 12;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(7, 92);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 69;
		this.label6.Text = "Item Name";
		this.CmbContractNo.AllowNull = (DefaultableBoolean)1;
		this.CmbContractNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance120;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Location = new System.Drawing.Point(465, 59);
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Name = "CmbContractNo";
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Size = new System.Drawing.Size(248, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractNo).TabIndex = 7;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(368, 11);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(93, 15);
		this.label50.TabIndex = 62;
		this.label50.Text = "Customer Name";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(368, 65);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(71, 15);
		this.label2.TabIndex = 63;
		this.label2.Text = "Contract No";
		this.CmbCustomerName.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Location = new System.Drawing.Point(465, 5);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Name = "CmbCustomerName";
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Size = new System.Drawing.Size(248, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).TabIndex = 5;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(2, 29);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(288, 19);
		this.label3.TabIndex = 1;
		this.label3.Text = "Export Sale Report With Activities";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1128, 53);
		this.panel2.TabIndex = 0;
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnnew, this.btnRefreshSummary, this.btnReport, this.btnFormat2, this.btn536Report, this.btnPrint536_01, this.btnShortcut });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1128, 27);
		this.tsItemType.TabIndex = 3;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefreshSummary.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshSummary.Name = "btnRefreshSummary";
		this.btnRefreshSummary.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshSummary.Text = "&Refresh";
		this.btnRefreshSummary.Click += new System.EventHandler(btnRefreshSummary_Click);
		this.btnReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnReport.Name = "btnReport";
		this.btnReport.Size = new System.Drawing.Size(116, 24);
		this.btnReport.Text = "Format1-534";
		this.btnReport.Visible = false;
		this.btnReport.Click += new System.EventHandler(btnReport_Click);
		this.btnFormat2.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnFormat2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnFormat2.Name = "btnFormat2";
		this.btnFormat2.Size = new System.Drawing.Size(116, 24);
		this.btnFormat2.Text = "Format2-535";
		this.btnFormat2.Visible = false;
		this.btnFormat2.Click += new System.EventHandler(btnFormat2_Click);
		this.btn536Report.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn536Report.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn536Report.Name = "btn536Report";
		this.btn536Report.Size = new System.Drawing.Size(230, 24);
		this.btn536Report.Text = "536-CustomerWiseComparison";
		this.btn536Report.Click += new System.EventHandler(btn536Report_Click);
		this.btnPrint536_01.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint536_01.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint536_01.Name = "btnPrint536_01";
		this.btnPrint536_01.Size = new System.Drawing.Size(114, 24);
		this.btnPrint536_01.Text = "536_01 Print";
		this.btnPrint536_01.Click += new System.EventHandler(btnPrint536_01_Click);
		this.btnShortcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcut.Name = "btnShortcut";
		this.btnShortcut.Size = new System.Drawing.Size(126, 24);
		this.btnShortcut.Text = "ShortCut Keys";
		this.btnShortcut.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1081, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 21);
		this.ctrlGrdBar1.TabIndex = 23;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label10);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 170);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1128, 23);
		this.panel5.TabIndex = 21;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(5, 2);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(119, 17);
		this.label10.TabIndex = 24;
		this.label10.Text = "Filtered Records";
		this.CmbItemStockAccountHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemStockAccountHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemStockAccountHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemStockAccountHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance84).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance3).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemStockAccountHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemStockAccountHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemStockAccountHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemStockAccountHistory).Location = new System.Drawing.Point(813, 32);
		((System.Windows.Forms.Control)(object)this.CmbItemStockAccountHistory).Name = "CmbItemStockAccountHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemStockAccountHistory).Size = new System.Drawing.Size(248, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemStockAccountHistory).TabIndex = 10;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(716, 38);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(87, 15);
		this.label20.TabIndex = 182;
		this.label20.Text = "Item Stock A/c";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1128, 511);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "ExportCustomerWiseComparisonsSummeryReport";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ExportCustomerWiseComparisonsSummeryReport_FormClosed);
		base.Load += new System.EventHandler(ExportCustomerWiseComparisonsSummeryReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExportCustomerWiseComparisonsSummeryReport_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSeaPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReportType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemStockAccountHistory).EndInit();
		base.ResumeLayout(false);
	}
}
