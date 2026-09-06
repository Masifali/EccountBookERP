// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Steel.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Steel.Reports.PurchaseReports
{

public class PurchaseInvoiceRegister : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	public DataTable dtViewAttachments = new DataTable();

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnPrintAvgByItem;

	private Label label23;

	private Panel panel5;

	internal GridEX DataGridHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private TextBox txtDocNoFrm;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Label label1;

	internal UltraCombo cmbItem;

	private Label label6;

	private TextBox txtDocNoTo;

	private DateTimePicker GRNToDate;

	private DateTimePicker GRNfromdate;

	private Label label27;

	private ToolStripButton btnPrintAvgBySupp;

	private Button btnshow;

	internal UltraCombo CmbReprotType;

	private Label label3;

	private Label label7;

	internal UltraCombo CmbCustomerItemGroup;

	internal UltraCombo CmbItemType;

	private Label label9;

	internal UltraCombo CmbItemCategory;

	private Label label8;

	internal UltraCombo CmbWareHouse;

	private Label label11;

	internal UltraCombo CmbCommissionAgent;

	private Label label10;

	internal UltraCombo CmbJobLot;

	private Label label12;

	internal UltraCombo CmbParentCategory;

	private Label label14;

	private ToolStripButton btn297PurchaseBill;

	private ToolStripDropDownButton tsDropDown;

	private DateTimePicker datDueToDate;

	private DateTimePicker datDueFromDate;

	private Label label16;

	private CheckBox ChkTodate;

	private CheckBox ChkFromdate;

	private TextBox txtrateto;

	private TextBox txtratefrom;

	private Label label18;

	private Label lblDrCrCommissionAgent;

	private Label lblbalanceCommissionAgent;

	internal UltraCombo CmbCustomGroup;

	private Label label5;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator2;

	public PurchaseInvoiceRegister(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmPurchaseInvoiceHistory_Load(object sender, EventArgs e)
	{
		try
		{
			lblbalanceCommissionAgent.Visible = false;
			lblDrCrCommissionAgent.Visible = false;
			dtViewAttachments.Columns.Add("AttchmentPath");
			ReportsLoadPurchaseDetail();
			GRNfromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			GRNfromdate.Focus();
			GetDataForPurchaseInvoiceRegisterDropdownBind();
			ReportTypeFill();
			SupplierGroupFill();
			CustomeGroupsDefine();
			btnPrintAvgByItem.Visible = false;
			btnPrintAvgBySupp.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplier_Leave(object sender, EventArgs e)
	{
	}

	private void CustomeGroupsDefine()
	{
		try
		{
			DataTable dtCustome = CommonServices.CustomeGroupsDefine(2);
			if (dtCustome.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCustome, CmbCustomGroup, "Id", "AcLookUpsDescription", "Custome Group", false);
				((UltraGridBase)CmbCustomGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDataForPurchaseInvoiceRegisterDropdownBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtAllData = InvPurchaseInvoice.AllComboAgainstPurchaseInvoiceForSteel(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtAllData.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtsup = new DataTable();
			DataTable dtCom = new DataTable();
			DataTable dtCat = new DataTable();
			DataTable dtParCat = new DataTable();
			DataTable dtType = new DataTable();
			DataTable dtwr = new DataTable();
			DataTable dtjob = new DataTable();
			DataTable dtItem = new DataTable();
			dtsup.Columns.Add("SupplierCustomerId");
			dtsup.Columns.Add("CompanyName");
			dtCom.Columns.Add("CommissionAgentId");
			dtCom.Columns.Add("CompanyName");
			dtCat.Columns.Add("ItemCategoryId");
			dtCat.Columns.Add("CategoryDescription");
			dtParCat.Columns.Add("ParentCategoryId");
			dtParCat.Columns.Add("ParentCategory");
			dtType.Columns.Add("ItemTypeId");
			dtType.Columns.Add("TypeDescription");
			dtwr.Columns.Add("WarehouseId");
			dtwr.Columns.Add("WareHouseName");
			dtjob.Columns.Add("JobLotId");
			dtjob.Columns.Add("JobLotDescription");
			dtItem.Columns.Add("ItemId");
			dtItem.Columns.Add("ItemName");
			for (int i = 0; i < dtAllData.Rows.Count; i++)
			{
				if (Conversion.ToString(dtAllData.Rows[i]["ActivityType"]) == "Supplier")
				{
					dtsup.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["Name"]);
				}
				if (Conversion.ToString(dtAllData.Rows[i]["ActivityType"]) == "CommissionAgent")
				{
					dtCom.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["Name"]);
				}
				if (Conversion.ToString(dtAllData.Rows[i]["ActivityType"]) == "ItemCategory")
				{
					dtCat.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["Name"]);
				}
				if (Conversion.ToString(dtAllData.Rows[i]["ActivityType"]) == "ParentItemCategory")
				{
					dtParCat.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["Name"]);
				}
				if (Conversion.ToString(dtAllData.Rows[i]["ActivityType"]) == "ItemType")
				{
					dtType.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["Name"]);
				}
				if (Conversion.ToString(dtAllData.Rows[i]["ActivityType"]) == "JobLot")
				{
					dtjob.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["Name"]);
				}
				if (Conversion.ToString(dtAllData.Rows[i]["ActivityType"]) == "ItemName")
				{
					dtItem.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["Name"]);
				}
				if (Conversion.ToString(dtAllData.Rows[i]["ActivityType"]) == "WareHouse")
				{
					dtwr.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["Name"]);
				}
			}
			DDL.BindDDL(dtsup, CmbSupplier, "SupplierCustomerId", "CompanyName", "Party Name", ZeroIndex: false);
			DDL.BindDDL(dtCom, CmbCommissionAgent, "CommissionAgentId", "CompanyName", "CommissionAgent", ZeroIndex: false);
			DDL.BindDDL(dtCat, CmbItemCategory, "ItemCategoryId", "CategoryDescription", "Item Category", ZeroIndex: false);
			DDL.BindDDL(dtParCat, CmbParentCategory, "ParentCategoryId", "ParentCategory", "Parent Category", ZeroIndex: false);
			DDL.BindDDL(dtType, CmbItemType, "ItemTypeId", "TypeDescription", "Item Type", ZeroIndex: false);
			DDL.BindDDL(dtwr, CmbWareHouse, "WarehouseId", "WareHouseName", "WareHouseName", ZeroIndex: false);
			DDL.BindDDL(dtjob, CmbJobLot, "JobLotId", "JobLotDescription", "JobLot", ZeroIndex: false);
			DDL.BindDDL(dtItem, cmbItem, "ItemId", "ItemName", "Item Name", ZeroIndex: false);
			((UltraGridBase)CmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbCommissionAgent).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbItemCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbItemType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbWareHouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbJobLot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ReportName");
			dt.Rows.Add(1, "PurchsaeDetail");
			dt.Rows.Add(2, "AvgRateByItem");
			dt.Rows.Add(3, "AvgRateBySupplier");
			DDL.BindDDL(dt, CmbReprotType, "Id", "ReportName", "Report Name", ZeroIndex: false);
			((UltraGridBase)CmbReprotType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbReprotType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierGroupFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("GroupName");
			dt.Rows.Add(1, "GroupByItem");
			dt.Rows.Add(2, "GroupBySupplier");
			DDL.BindDDL(dt, CmbCustomerItemGroup, "Id", "GroupName", "GroupName", ZeroIndex: false);
			((UltraGridBase)CmbCustomerItemGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbCustomerItemGroup).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbReprotType_TextChanged(object sender, EventArgs e)
	{
		if (((Control)(object)CmbReprotType).Text == "AvgRateByItem")
		{
			((Control)(object)CmbCustomerItemGroup).Visible = false;
			((Control)(object)CmbCustomerItemGroup).Enabled = false;
			label7.Visible = false;
			label7.Enabled = false;
			tsDropDown.Enabled = false;
			tsDropDown.Visible = false;
			btn297PurchaseBill.Visible = false;
			btnPrintAvgByItem.Visible = true;
			btnPrintAvgBySupp.Visible = false;
		}
		else if (((Control)(object)CmbReprotType).Text == "AvgRateBySupplier")
		{
			((Control)(object)CmbCustomerItemGroup).Visible = false;
			((Control)(object)CmbCustomerItemGroup).Enabled = false;
			label7.Visible = false;
			label7.Enabled = false;
			btn297PurchaseBill.Visible = false;
			tsDropDown.Enabled = false;
			tsDropDown.Visible = false;
			btnPrintAvgByItem.Visible = false;
			btnPrintAvgBySupp.Visible = true;
		}
		else
		{
			((Control)(object)CmbCustomerItemGroup).Visible = true;
			((Control)(object)CmbCustomerItemGroup).Enabled = true;
			label7.Visible = true;
			label7.Enabled = true;
			tsDropDown.Enabled = true;
			btn297PurchaseBill.Visible = true;
			tsDropDown.Visible = true;
			btnPrintAvgByItem.Visible = false;
			btnPrintAvgBySupp.Visible = false;
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void Reset()
	{
		try
		{
			txtDocNoFrm.Text = string.Empty;
			txtDocNoTo.Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)CmbCustomerItemGroup).Text = string.Empty;
			GRNfromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			GRNToDate.Value = DateTime.Today;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridHisoryDetail()
	{
		try
		{
			DataTable dtInvoice = CommonServicePurchaseDetailRegister();
			if (dtInvoice.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("InvoiceId", typeof(int));
				dt.Columns.Add("InvoiceDetailId", typeof(int));
				dt.Columns.Add("InvoiceType", typeof(string));
				dt.Columns.Add("InvoiceDate", typeof(DateTime));
				dt.Columns.Add("InvoiceNo", typeof(int));
				dt.Columns.Add("ManualBillNo", typeof(string));
				dt.Columns.Add("GrnId", typeof(int));
				dt.Columns.Add("GrnNo", typeof(int));
				dt.Columns.Add("GrnDate", typeof(DateTime));
				dt.Columns.Add("OrderId", typeof(int));
				dt.Columns.Add("OrderNo", typeof(int));
				dt.Columns.Add("OrderDate", typeof(DateTime));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("CommissionAgent", typeof(string));
				dt.Columns.Add("CommAmount", typeof(double));
				dt.Columns.Add("CreditAdd", typeof(double));
				dt.Columns.Add("DebitLess", typeof(double));
				dt.Columns.Add("GpId", typeof(int));
				dt.Columns.Add("GpNo", typeof(int));
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("BiltyNo", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("PurchaseGLAC", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("WareHouseName", typeof(string));
				dt.Columns.Add("JobLot", typeof(string));
				dt.Columns.Add("PackUom", typeof(string));
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("WcutTotal", typeof(double));
				dt.Columns.Add("AddLess", typeof(double));
				dt.Columns.Add("NetBillWeight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("RateCut", typeof(double));
				dt.Columns.Add("RateCutAmount", typeof(double));
				dt.Columns.Add("NetRate", typeof(double));
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("FreightAmt", typeof(double));
				dt.Columns.Add("ExpenseAmt", typeof(double));
				dt.Columns.Add("CommissionAmt", typeof(double));
				dt.Columns.Add("ItemNetAmount", typeof(double));
				dt.Columns.Add("BillAmount", typeof(double));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("IsApproved", typeof(string));
				dt.Columns.Add("VoucherHeadId", typeof(int));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Clear();
				for (int i = 0; i < dtInvoice.Rows.Count; i++)
				{
					dt.Rows.Add(dtInvoice.Rows[i]["Id"], dtInvoice.Rows[i]["InvPurchaseDetailId"], dtInvoice.Rows[i]["OrderType"], dtInvoice.Rows[i]["DocDate"], dtInvoice.Rows[i]["DocNo"], dtInvoice.Rows[i]["ManualBillNo"], dtInvoice.Rows[i]["GrnId"], dtInvoice.Rows[i]["DocNoGrn"], dtInvoice.Rows[i]["DocDateGrn"], dtInvoice.Rows[i]["OrderId"], dtInvoice.Rows[i]["DocNopo"], dtInvoice.Rows[i]["DocDatepo"], dtInvoice.Rows[i]["DocumentTypeId"], dtInvoice.Rows[i]["SupplierCustomerId"], dtInvoice.Rows[i]["SupplierName"], dtInvoice.Rows[i]["CommAgentName"], dtInvoice.Rows[i]["CommAmount"], dtInvoice.Rows[i]["CreditAdd"], dtInvoice.Rows[i]["DebitLess"], dtInvoice.Rows[i]["GpId"], dtInvoice.Rows[i]["GpSrNo"], dtInvoice.Rows[i]["GpDate"], dtInvoice.Rows[i]["VehicleNo"], dtInvoice.Rows[i]["BiltyNo"], dtInvoice.Rows[i]["ItemId"], dtInvoice.Rows[i]["PurchaseGLAC"], dtInvoice.Rows[i]["ItemName"], dtInvoice.Rows[i]["WareHouseCode"], dtInvoice.Rows[i]["JobLotCode"], dtInvoice.Rows[i]["UOMCode"], dtInvoice.Rows[i]["ItemQty"], dtInvoice.Rows[i]["GrossWeight"], dtInvoice.Rows[i]["WtCutTotal"], dtInvoice.Rows[i]["AdLsWeight"], dtInvoice.Rows[i]["NetBillWeight"], dtInvoice.Rows[i]["ItemRate"], dtInvoice.Rows[i]["RateCut"], dtInvoice.Rows[i]["RateCutAmount"], dtInvoice.Rows[i]["NetRate"], dtInvoice.Rows[i]["ItemAmount"], dtInvoice.Rows[i]["FreightAmount"], dtInvoice.Rows[i]["ExpenseAmount"], dtInvoice.Rows[i]["CommissionAmount"], dtInvoice.Rows[i]["ItemNetAmount"], dtInvoice.Rows[i]["BillAmountH"], dtInvoice.Rows[i]["EntryDate"], dtInvoice.Rows[i]["EnteryUserName"], dtInvoice.Rows[i]["IsApproved"], dtInvoice.Rows[i]["VoucherHeadId"], dtInvoice.Rows[i]["NoOfAttachments"]);
				}
				((Control)(object)DataGridHistory).Name = "gridHisoryDetail";
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				GridSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			if (((Control)(object)CmbCustomerItemGroup).Text == "GroupByItem")
			{
				DataGridHistory.RootTable.Groups.Add("ItemName");
				DataGridHistory.RootTable.Columns["ItemName"].Visible = false;
				DataGridHistory.RootTable.GroupTotals = (GroupTotals)2;
			}
			if (((Control)(object)CmbCustomerItemGroup).Text == "GroupBySupplier")
			{
				DataGridHistory.RootTable.Groups.Add("SupplierName");
				DataGridHistory.RootTable.Columns["SupplierName"].Visible = false;
				DataGridHistory.RootTable.GroupTotals = (GroupTotals)2;
			}
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Slip";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Slip";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Voucher");
			DataGridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			DataGridHistory.RootTable.Columns["InvoiceId"].Visible = false;
			DataGridHistory.RootTable.Columns["InvoiceDetailId"].Visible = false;
			DataGridHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["GrnId"].Visible = false;
			DataGridHistory.RootTable.Columns["GpId"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistory.RootTable.Columns["OrderId"].Visible = false;
			DataGridHistory.RootTable.Columns["PurchaseGLAC"].Visible = false;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			DataGridHistory.RootTable.Columns["SupplierName"].Width = 200;
			DataGridHistory.RootTable.Columns["ItemName"].Width = 180;
			DataGridHistory.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["GrnNo"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["OrderNo"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WcutTotal"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AddLess"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FreightAmt"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ExpenseAmt"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CommissionAmt"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CreditAdd"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["DebitLess"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			DataGridHistory.RootTable.Columns["NetRate"].AggregateFunction = (AggregateFunction)3;
			DataGridHistory.RootTable.Columns["RateCut"].AggregateFunction = (AggregateFunction)3;
			DataGridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WcutTotal"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AddLess"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FreightAmt"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommissionAmt"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ExpenseAmt"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CreditAdd"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["DebitLess"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["GrossWeight"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["WcutTotal"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["AddLess"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["NetBillWeight"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["RateCutAmount"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["ItemAmount"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["FreightAmt"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["ExpenseAmt"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["CommissionAmt"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["NetRate"].FormatString = "#,#.##";
			DataGridHistory.RootTable.Columns["RateCut"].FormatString = "#,#.##";
			DataGridHistory.RootTable.Columns["BillAmount"].FormatString = "#,#.##";
			DataGridHistory.RootTable.Columns["ItemNetAmount"].FormatString = "#,#.##";
			DataGridHistory.RootTable.Columns["CommAmount"].FormatString = "#,#.##";
			DataGridHistory.RootTable.Columns["DebitLess"].FormatString = "#,#.##";
			DataGridHistory.RootTable.Columns["CreditAdd"].FormatString = "#,#.##";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["WcutTotal"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["AddLess"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["RateCutAmount"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["ItemAmount"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["FreightAmt"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["ExpenseAmt"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["CommissionAmt"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["NetRate"].TotalFormatString = "#,#.##";
			DataGridHistory.RootTable.Columns["RateCut"].TotalFormatString = "#,#.##";
			DataGridHistory.RootTable.Columns["BillAmount"].TotalFormatString = "#,#.##";
			DataGridHistory.RootTable.Columns["ItemNetAmount"].TotalFormatString = "#,#.##";
			DataGridHistory.RootTable.Columns["CommAmount"].TotalFormatString = "#,#.##";
			DataGridHistory.RootTable.Columns["CreditAdd"].TotalFormatString = "#,#.##";
			DataGridHistory.RootTable.Columns["DebitLess"].TotalFormatString = "#,#.##";
			CommonServices.GridAutoAdjustment(AryColWidth, DataGridHistory);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private DataTable CommonServicePurchaseDetailRegister()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = GRNfromdate.Value;
			obj.ToDate = GRNToDate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtDocNoFrm.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtDocNoTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.CommissionAgentId = Conversion.ToInt(CmbCommissionAgent.Value);
			obj.JobLotId = Conversion.ToInt(CmbJobLot.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbItemCategory.Value);
			obj.ItemTypeId = Conversion.ToInt(CmbItemType.Value);
			obj.WarehouseId = Conversion.ToInt(CmbWareHouse.Value);
			obj.InventoryParentCategories = Conversion.ToInt(CmbParentCategory.Value);
			obj.RateFrom = Conversion.ToDouble((object)txtratefrom.Text.Trim());
			obj.RateTo = Conversion.ToDouble((object)txtrateto.Text.Trim());
			obj.CustomerGroupId = Conversion.ToInt(CmbCustomGroup.Value);
			if (ChkFromdate.Checked)
			{
				obj.DueDateFrom = datDueFromDate.Value;
			}
			if (ChkTodate.Checked)
			{
				obj.DueDateTo = datDueToDate.Value;
			}
			return InvPurchaseInvoice.PurchaseInvoiceRegisterSteel(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void gridHisoryAvgRateByItemId()
	{
		try
		{
			DataTable dtInvoice = GetAvgRateByItemCommonMethod();
			if (dtInvoice.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("InvoiceDate", typeof(DateTime));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("PurchaseGLAC", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemType", typeof(string));
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("NetBillWeight", typeof(double));
				dt.Columns.Add("WeightByItem", typeof(double));
				dt.Columns.Add("AvgRate", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("MisExp", typeof(double));
				dt.Columns.Add("MisExp40Kg", typeof(double));
				dt.Columns.Add("FreightExp", typeof(double));
				dt.Columns.Add("FreightExp40Kg", typeof(double));
				dt.Columns.Add("CommExp", typeof(double));
				dt.Columns.Add("CommExp40Kg", typeof(double));
				dt.Columns.Add("TotalExpense", typeof(double));
				dt.Columns.Add("TotalExpense40Kg", typeof(double));
				dt.Columns.Add("AvgRateWithExp", typeof(double));
				dt.Columns.Add("AmountWithExp", typeof(double));
				dt.Clear();
				for (int i = 0; i < dtInvoice.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToDateTime(dtInvoice.Rows[i]["DocDate"]).ToShortDateString(), dtInvoice.Rows[i]["ItemId"], dtInvoice.Rows[i]["PurchaseGLAC"], dtInvoice.Rows[i]["ItemName"], dtInvoice.Rows[i]["TypeDescription"], dtInvoice.Rows[i]["ItemQty"], dtInvoice.Rows[i]["NetBillWeight"], dtInvoice.Rows[i]["TotalWeightByItem"], dtInvoice.Rows[i]["AvgRate"], dtInvoice.Rows[i]["ItemAmount"], dtInvoice.Rows[i]["MiscExp"], dtInvoice.Rows[i]["MiscExp40Kg"], dtInvoice.Rows[i]["FreigtExp"], dtInvoice.Rows[i]["FrtExp40Kg"], dtInvoice.Rows[i]["CommExp"], dtInvoice.Rows[i]["CommExp40Kg"], dtInvoice.Rows[i]["TotalExpense"], dtInvoice.Rows[i]["Exp40Kg"], dtInvoice.Rows[i]["AvgRateExp"], dtInvoice.Rows[i]["ItemExpAmount"]);
				}
				((Control)(object)DataGridHistory).Name = "gridHisoryAvgRateByItemId";
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				GridAvgRateByItemSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridAvgRateByItemSettings()
	{
		try
		{
			DataGridHistory.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistory.RootTable.Columns["PurchaseGLAC"].Visible = false;
			DataGridHistory.RootTable.Columns["InvoiceDate"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WeightByItem"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AvgRate"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["MisExp"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["MisExp40Kg"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FreightExp"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FreightExp40Kg"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CommExp"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CommExp40Kg"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalExpense"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalExpense40Kg"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AvgRateWithExp"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AmountWithExp"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WeightByItem"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["MisExp"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["MisExp40Kg"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FreightExp"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FreightExp40Kg"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommExp"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommExp40Kg"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["TotalExpense"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["TotalExpense40Kg"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AvgRateWithExp"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AmountWithExp"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["NetBillWeight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["WeightByItem"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["AvgRate"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["Amount"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["MisExp"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["MisExp40Kg"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["FreightExp"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["FreightExp40Kg"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["CommExp"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["CommExp40Kg"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalExpense"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalExpense40Kg"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["AvgRateWithExp"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["AmountWithExp"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["NetBillWeight"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["WeightByItem"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["AvgRate"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["Amount"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["MisExp"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["MisExp40Kg"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["FreightExp"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["FreightExp40Kg"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["CommExp"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["CommExp40Kg"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalExpense"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalExpense40Kg"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["AvgRateWithExp"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["AmountWithExp"].FormatString = "0,0";
			CommonServices.GridAutoAdjustment(AryColWidth, DataGridHistory);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private DataTable GetAvgRateByItemCommonMethod()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		try
		{
			return InvPurchaseInvoice.PurchaseInvoiceAvgRateByItemForSteel(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = GRNfromdate.Value,
				ToDate = GRNToDate.Value,
				ItemId = Conversion.ToInt(cmbItem.Value),
				InventoryParentCategories = Conversion.ToInt(CmbParentCategory.Value),
				ItemCategoryId = Conversion.ToInt(CmbItemCategory.Value),
				ItemTypeId = Conversion.ToInt(CmbItemType.Value),
				JobLotId = Conversion.ToInt(CmbJobLot.Value)
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void gridHisoryAvgRateBySuplier()
	{
		try
		{
			DataTable dtInvoice = GetAvgRateBySupplierCommonMethod();
			if (dtInvoice.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("PurchaseGLAC", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("NetBillWeight", typeof(double));
				dt.Columns.Add("WeightByItem", typeof(double));
				dt.Columns.Add("AvgRate", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("MisExp", typeof(double));
				dt.Columns.Add("MisExp40Kg", typeof(double));
				dt.Columns.Add("FreightExp", typeof(double));
				dt.Columns.Add("FreightExp40Kg", typeof(double));
				dt.Columns.Add("CommExp", typeof(double));
				dt.Columns.Add("CommExp40Kg", typeof(double));
				dt.Columns.Add("TotalExpense", typeof(double));
				dt.Columns.Add("TotalExpense40Kg", typeof(double));
				dt.Columns.Add("AvgRateWithExp", typeof(double));
				dt.Columns.Add("AmountWithExp", typeof(double));
				dt.Clear();
				for (int i = 0; i < dtInvoice.Rows.Count; i++)
				{
					dt.Rows.Add(dtInvoice.Rows[i]["SupplierCustomerId"], dtInvoice.Rows[i]["CompanyName"], dtInvoice.Rows[i]["ItemId"], dtInvoice.Rows[i]["PurchaseGLAC"], dtInvoice.Rows[i]["ItemName"], dtInvoice.Rows[i]["ItemQty"], dtInvoice.Rows[i]["NetBillWeight"], dtInvoice.Rows[i]["TotalWeightByItem"], dtInvoice.Rows[i]["AvgRate"], dtInvoice.Rows[i]["ItemAmount"], dtInvoice.Rows[i]["MiscExp"], dtInvoice.Rows[i]["MiscExp40Kg"], dtInvoice.Rows[i]["FreigtExp"], dtInvoice.Rows[i]["FrtExp40Kg"], dtInvoice.Rows[i]["CommExp"], dtInvoice.Rows[i]["CommExp40Kg"], dtInvoice.Rows[i]["TotalExpense"], dtInvoice.Rows[i]["Exp40Kg"], dtInvoice.Rows[i]["AvgRateExp"], dtInvoice.Rows[i]["ItemExpAmount"]);
				}
				((Control)(object)DataGridHistory).Name = "gridHisoryAvgRateBySuplier";
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				GridAvgRateBySupplierSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridAvgRateBySupplierSettings()
	{
		try
		{
			DataGridHistory.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistory.RootTable.Columns["PurchaseGLAC"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WeightByItem"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AvgRate"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["MisExp"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["MisExp40Kg"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FreightExp"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FreightExp40Kg"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CommExp"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CommExp40Kg"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalExpense"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalExpense40Kg"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AvgRateWithExp"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AmountWithExp"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WeightByItem"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["MisExp"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["MisExp40Kg"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FreightExp"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FreightExp40Kg"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommExp"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommExp40Kg"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["TotalExpense"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["TotalExpense40Kg"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AvgRateWithExp"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AmountWithExp"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["NetBillWeight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["WeightByItem"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["AvgRate"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["Amount"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["MisExp"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["MisExp40Kg"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["FreightExp"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["FreightExp40Kg"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["CommExp"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["CommExp40Kg"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalExpense"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalExpense40Kg"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["AvgRateWithExp"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["AmountWithExp"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["NetBillWeight"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["WeightByItem"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["AvgRate"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["Amount"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["MisExp"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["MisExp40Kg"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["FreightExp"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["FreightExp40Kg"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["CommExp"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["CommExp40Kg"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalExpense"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalExpense40Kg"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["AvgRateWithExp"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["AmountWithExp"].FormatString = "0,0";
			CommonServices.GridAutoAdjustment(AryColWidth, DataGridHistory);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private DataTable GetAvgRateBySupplierCommonMethod()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		try
		{
			return InvPurchaseInvoice.PurchaseInvoiceAvgRateBySupplierForSteel(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = GRNfromdate.Value,
				ToDate = GRNToDate.Value,
				ItemId = Conversion.ToInt(cmbItem.Value),
				InventoryParentCategories = Conversion.ToInt(CmbParentCategory.Value),
				ItemCategoryId = Conversion.ToInt(CmbItemCategory.Value),
				ItemTypeId = Conversion.ToInt(CmbItemType.Value),
				JobLotId = Conversion.ToInt(CmbJobLot.Value)
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(item.Cells["VoucherHeadId"].Value), Conversion.ToInt(item.Cells["DocumentTypeId"].Value));
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.PurchaseInvoiceSteelSlip_1508(Conversion.ToInt(item.Cells["InvoiceId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (e.Column.Key == "InvoiceNo")
			{
				Conversion.ToInt(item.Cells["InvoiceId"].Value);
			}
			if (e.Column.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["SupplierCustomerId"].Value), GRNfromdate.Value, GRNToDate.Value);
			}
			_ = e.Column.Key == "OrderNo";
			if (e.Column.Key == "GrnNo")
			{
				CommonServices.GrnSlipSteelWithSubReports(Conversion.ToInt(item.Cells["GrnId"].Value));
			}
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(int.Parse(item.Cells["InvoiceId"].Value.ToString()), Conversion.ToInt(item.Cells["DocumentTypeId"].Value));
			}
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["PurchaseGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), GRNfromdate.Value, GRNToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportsLoadPurchaseDetail()
	{
		try
		{
			string[] files = CommonServices.DynamicReportsLoad("PurchaseInvoiceRegister");
			if (files != null && files.Length != 0)
			{
				for (int i = 0; i < files.Length; i++)
				{
					tsDropDown.DropDownItems.Add(Path.GetFileNameWithoutExtension(files[i]));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn297PurchaseBill_Click(object sender, EventArgs e)
	{
		try
		{
			PurchaseInvoiceRegisterDetail("1505-InvPurchaseInvoiceRegister.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("No Record Found");
		}
	}

	private void PurchaseInvoiceRegisterDetail(string ReportName)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = CommonServicePurchaseDetailRegister();
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, ReportName);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tsDropDown_DropDownItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
	{
		try
		{
			PurchaseInvoiceRegisterDetail(e.ClickedItem.Text + ".rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintSummery_Click(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GetAvgRateBySupplierCommonMethod();
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1506-PurchaseInvoiceRegisterAvgRateItemSupplier.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("No Record Found");
		}
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GetAvgRateByItemCommonMethod();
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1507-PurchaseInvoiceRegisterAvgRateByItem.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("No Record Found");
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			btnPrintAvgBySupp.Visible = false;
			btnPrintAvgByItem.Visible = false;
			if (((Control)(object)CmbReprotType).Text == string.Empty)
			{
				MessageBox.Show("Please Select Report Type");
				return;
			}
			if (((Control)(object)CmbReprotType).Text == "PurchsaeDetail")
			{
				gridHisoryDetail();
			}
			if (((Control)(object)CmbReprotType).Text == "AvgRateByItem")
			{
				gridHisoryAvgRateByItemId();
				btnPrintAvgByItem.Visible = true;
			}
			if (((Control)(object)CmbReprotType).Text == "AvgRateBySupplier")
			{
				gridHisoryAvgRateBySuplier();
				btnPrintAvgBySupp.Visible = true;
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void frmPurchaseInvoiceHistory_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.P)
		{
			print_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnNew_Click(null, null);
		}
	}

	private void txtRateUOM_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocNoFrm_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocNoTo_TextChanged(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtDocNoTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtratefrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtrateto_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseInvoiceRegister_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Expected O, but got Unknown
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Expected O, but got Unknown
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Expected O, but got Unknown
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Expected O, but got Unknown
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Expected O, but got Unknown
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Expected O, but got Unknown
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbCustomGroup = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.lblDrCrCommissionAgent = new System.Windows.Forms.Label();
		this.lblbalanceCommissionAgent = new System.Windows.Forms.Label();
		this.txtrateto = new System.Windows.Forms.TextBox();
		this.txtratefrom = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.ChkTodate = new System.Windows.Forms.CheckBox();
		this.label14 = new System.Windows.Forms.Label();
		this.ChkFromdate = new System.Windows.Forms.CheckBox();
		this.CmbParentCategory = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbCommissionAgent = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbItemType = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbItemCategory = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbCustomerItemGroup = new UltraCombo();
		this.CmbReprotType = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbItem = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.datDueToDate = new System.Windows.Forms.DateTimePicker();
		this.datDueFromDate = new System.Windows.Forms.DateTimePicker();
		this.GRNToDate = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.GRNfromdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.txtDocNoFrm = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsDropDown = new System.Windows.Forms.ToolStripDropDownButton();
		this.btn297PurchaseBill = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrintAvgBySupp = new System.Windows.Forms.ToolStripButton();
		this.btnPrintAvgByItem = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerItemGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReprotType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(999, 661);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 221);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(999, 440);
		this.panel5.TabIndex = 3;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		this.DataGridHistory.AlternatingColors = true;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		this.DataGridHistory.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(999, 440);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroup);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.lblDrCrCommissionAgent);
		this.panel4.Controls.Add(this.lblbalanceCommissionAgent);
		this.panel4.Controls.Add(this.txtrateto);
		this.panel4.Controls.Add(this.txtratefrom);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.ChkTodate);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.ChkFromdate);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategory);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgent);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemType);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategory);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerItemGroup);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReprotType);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.txtDocNoTo);
		this.panel4.Controls.Add(this.datDueToDate);
		this.panel4.Controls.Add(this.datDueFromDate);
		this.panel4.Controls.Add(this.GRNToDate);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add(this.GRNfromdate);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.txtDocNoFrm);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 56);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(999, 165);
		this.panel4.TabIndex = 0;
		this.CmbCustomGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCustomGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Location = new System.Drawing.Point(719, 69);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Name = "CmbCustomGroup";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).TabIndex = 18;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(634, 75);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(84, 15);
		this.label5.TabIndex = 221;
		this.label5.Text = "Custom Group";
		this.lblDrCrCommissionAgent.AutoSize = true;
		this.lblDrCrCommissionAgent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDrCrCommissionAgent.Location = new System.Drawing.Point(932, 44);
		this.lblDrCrCommissionAgent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lblDrCrCommissionAgent.Name = "lblDrCrCommissionAgent";
		this.lblDrCrCommissionAgent.Size = new System.Drawing.Size(45, 16);
		this.lblDrCrCommissionAgent.TabIndex = 218;
		this.lblDrCrCommissionAgent.Text = "label3";
		this.lblbalanceCommissionAgent.AutoSize = true;
		this.lblbalanceCommissionAgent.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.lblbalanceCommissionAgent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblbalanceCommissionAgent.Location = new System.Drawing.Point(932, 66);
		this.lblbalanceCommissionAgent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lblbalanceCommissionAgent.Name = "lblbalanceCommissionAgent";
		this.lblbalanceCommissionAgent.Size = new System.Drawing.Size(45, 16);
		this.lblbalanceCommissionAgent.TabIndex = 219;
		this.lblbalanceCommissionAgent.Text = "label3";
		this.txtrateto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtrateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtrateto.Location = new System.Drawing.Point(248, 101);
		this.txtrateto.Name = "txtrateto";
		this.txtrateto.Size = new System.Drawing.Size(117, 23);
		this.txtrateto.TabIndex = 9;
		this.txtrateto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtrateto_KeyPress);
		this.txtratefrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtratefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtratefrom.Location = new System.Drawing.Point(128, 101);
		this.txtratefrom.Name = "txtratefrom";
		this.txtratefrom.Size = new System.Drawing.Size(117, 23);
		this.txtratefrom.TabIndex = 8;
		this.txtratefrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtratefrom_KeyPress);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(9, 105);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(89, 15);
		this.label18.TabIndex = 216;
		this.label18.Text = "Rate From && To";
		this.ChkTodate.AutoSize = true;
		this.ChkTodate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkTodate.Location = new System.Drawing.Point(352, 45);
		this.ChkTodate.Name = "ChkTodate";
		this.ChkTodate.Size = new System.Drawing.Size(15, 14);
		this.ChkTodate.TabIndex = 5;
		this.ChkTodate.UseVisualStyleBackColor = true;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(9, 134);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(119, 15);
		this.label14.TabIndex = 213;
		this.label14.Text = "Item Parent Category";
		this.ChkFromdate.AutoSize = true;
		this.ChkFromdate.Location = new System.Drawing.Point(232, 45);
		this.ChkFromdate.Name = "ChkFromdate";
		this.ChkFromdate.Size = new System.Drawing.Size(15, 14);
		this.ChkFromdate.TabIndex = 3;
		this.ChkFromdate.UseVisualStyleBackColor = true;
		this.CmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Location = new System.Drawing.Point(128, 128);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Name = "CmbParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).TabIndex = 10;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(457, 128);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 15;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(375, 134);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(43, 15);
		this.label12.TabIndex = 209;
		this.label12.Text = "JobLot";
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(457, 99);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 14;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(375, 105);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 207;
		this.label11.Text = "WareHouse";
		this.CmbCommissionAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance106;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance112).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCommissionAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Location = new System.Drawing.Point(719, 39);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Name = "CmbCommissionAgent";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).TabIndex = 17;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(634, 45);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(78, 15);
		this.label10.TabIndex = 205;
		this.label10.Text = "Comm Agent";
		this.CmbItemType.AllowNull = (DefaultableBoolean)1;
		this.CmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Location = new System.Drawing.Point(457, 39);
		((System.Windows.Forms.Control)(object)this.CmbItemType).Name = "CmbItemType";
		((System.Windows.Forms.Control)(object)this.CmbItemType).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemType).TabIndex = 12;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(375, 45);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(60, 15);
		this.label9.TabIndex = 203;
		this.label9.Text = "Item Type";
		this.CmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance97).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Location = new System.Drawing.Point(457, 9);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Name = "CmbItemCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).TabIndex = 11;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(375, 15);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(82, 15);
		this.label8.TabIndex = 201;
		this.label8.Text = "Item Category";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(634, 134);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(56, 15);
		this.label7.TabIndex = 199;
		this.label7.Text = "Group By";
		this.CmbCustomerItemGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerItemGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerItemGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerItemGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerItemGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerItemGroup).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCustomerItemGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerItemGroup).Location = new System.Drawing.Point(719, 128);
		((System.Windows.Forms.Control)(object)this.CmbCustomerItemGroup).Name = "CmbCustomerItemGroup";
		((System.Windows.Forms.Control)(object)this.CmbCustomerItemGroup).Size = new System.Drawing.Size(144, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerItemGroup).TabIndex = 20;
		this.CmbReprotType.AllowNull = (DefaultableBoolean)1;
		this.CmbReprotType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReprotType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReprotType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReprotType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance78;
		((SpecialBoxBase)((UltraGridBase)this.CmbReprotType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance84).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReprotType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReprotType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbReprotType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReprotType).Location = new System.Drawing.Point(719, 99);
		((System.Windows.Forms.Control)(object)this.CmbReprotType).Name = "CmbReprotType";
		((System.Windows.Forms.Control)(object)this.CmbReprotType).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbReprotType).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.CmbReprotType).TextChanged += new System.EventHandler(CmbReprotType_TextChanged);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(634, 105);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(70, 15);
		this.label3.TabIndex = 149;
		this.label3.Text = "Report Type";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(868, 128);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(59, 26);
		this.btnshow.TabIndex = 21;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance125).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(457, 69);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 13;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(375, 75);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 147;
		this.label6.Text = "Item Name";
		this.txtDocNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoTo.Location = new System.Drawing.Point(248, 71);
		this.txtDocNoTo.Name = "txtDocNoTo";
		this.txtDocNoTo.Size = new System.Drawing.Size(117, 23);
		this.txtDocNoTo.TabIndex = 7;
		this.txtDocNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoTo_KeyPress);
		this.datDueToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDueToDate.CustomFormat = "dd-MMM-yy";
		this.datDueToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDueToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDueToDate.Location = new System.Drawing.Point(248, 41);
		this.datDueToDate.Name = "datDueToDate";
		this.datDueToDate.Size = new System.Drawing.Size(102, 23);
		this.datDueToDate.TabIndex = 4;
		this.datDueFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDueFromDate.CustomFormat = "dd-MMM-yy";
		this.datDueFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDueFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDueFromDate.Location = new System.Drawing.Point(128, 41);
		this.datDueFromDate.Name = "datDueFromDate";
		this.datDueFromDate.Size = new System.Drawing.Size(102, 23);
		this.datDueFromDate.TabIndex = 2;
		this.GRNToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.CustomFormat = "dd-MMM-yy";
		this.GRNToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNToDate.Location = new System.Drawing.Point(248, 11);
		this.GRNToDate.Name = "GRNToDate";
		this.GRNToDate.Size = new System.Drawing.Size(117, 23);
		this.GRNToDate.TabIndex = 1;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(9, 45);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(116, 15);
		this.label16.TabIndex = 139;
		this.label16.Text = "Due Date From && To";
		this.GRNfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.CustomFormat = "dd-MMM-yy";
		this.GRNfromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNfromdate.Location = new System.Drawing.Point(128, 11);
		this.GRNfromdate.Name = "GRNfromdate";
		this.GRNfromdate.Size = new System.Drawing.Size(117, 23);
		this.GRNfromdate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(9, 15);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(116, 15);
		this.label27.TabIndex = 139;
		this.label27.Text = "Doc Date From && To";
		this.txtDocNoFrm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrm.Location = new System.Drawing.Point(128, 71);
		this.txtDocNoFrm.Name = "txtDocNoFrm";
		this.txtDocNoFrm.Size = new System.Drawing.Size(117, 23);
		this.txtDocNoFrm.TabIndex = 6;
		this.txtDocNoFrm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoFrm_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(9, 75);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(107, 15);
		this.label2.TabIndex = 135;
		this.label2.Text = "Doc No From && To";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(719, 9);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 16;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Leave += new System.EventHandler(CmbSupplier_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(634, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(69, 15);
		this.label1.TabIndex = 132;
		this.label1.Text = "Party Name";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 26);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(999, 30);
		this.panel3.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(945, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(221, 19);
		this.label23.TabIndex = 4;
		this.label23.Text = "Purchase Invoice Register";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(999, 26);
		this.panel2.TabIndex = 0;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 8.75f);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.toolStripSeparator1, this.tsDropDown, this.btn297PurchaseBill, this.toolStripSeparator2, this.btnPrintAvgBySupp, this.btnPrintAvgByItem });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(999, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(79, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.tsDropDown.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDown.Name = "tsDropDown";
		this.tsDropDown.Size = new System.Drawing.Size(69, 24);
		this.tsDropDown.Text = "Print";
		this.tsDropDown.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDown_DropDownItemClicked_1);
		this.btn297PurchaseBill.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn297PurchaseBill.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn297PurchaseBill.Name = "btn297PurchaseBill";
		this.btn297PurchaseBill.Size = new System.Drawing.Size(156, 24);
		this.btn297PurchaseBill.Text = "1505-DetailRegister";
		this.btn297PurchaseBill.Click += new System.EventHandler(btn297PurchaseBill_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnPrintAvgBySupp.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintAvgBySupp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintAvgBySupp.Name = "btnPrintAvgBySupp";
		this.btnPrintAvgBySupp.Size = new System.Drawing.Size(186, 24);
		this.btnPrintAvgBySupp.Text = "1506-&AvgRateBySupplier";
		this.btnPrintAvgBySupp.Visible = false;
		this.btnPrintAvgBySupp.Click += new System.EventHandler(btnPrintSummery_Click);
		this.btnPrintAvgByItem.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintAvgByItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintAvgByItem.Name = "btnPrintAvgByItem";
		this.btnPrintAvgByItem.Size = new System.Drawing.Size(164, 24);
		this.btnPrintAvgByItem.Text = "1507-AvgRateByItem";
		this.btnPrintAvgByItem.Visible = false;
		this.btnPrintAvgByItem.Click += new System.EventHandler(print_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(999, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PurchaseInvoiceRegister";
		this.Text = "PurchaseInvoiceRegister";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PurchaseInvoiceRegister_FormClosing);
		base.Load += new System.EventHandler(frmPurchaseInvoiceHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPurchaseInvoiceHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerItemGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReprotType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
