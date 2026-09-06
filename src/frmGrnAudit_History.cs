// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Commentaries;
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
namespace Architecture.WinApp.Inventory_Reports
{

public class frmGrnAudit_History : Form
{
	private WaitForm waitForm = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dt = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtBindCombo = new DataTable();

	public DataTable dtViewAttachments = new DataTable();

	public bool RequestedByOtherDocument;

	public int ParentId;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Panel panel5;

	internal UltraCombo CmbSupplier;

	private Label label1;

	private Label label27;

	internal GridEX DataGridHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private ToolStripButton btn334_01Register;

	private ToolStripButton btnRefresh;

	private Panel panel6;

	private Label label4;

	private GroupBox groupBox1;

	private BackgroundWorker backgroundWorker1;

	private ToolStripButton btnShortCutKeys;

	internal UltraCombo cmbperemeter;

	private Label label12;

	public DateTimePicker GRNfromdate;

	public DateTimePicker GRNToDate;

	private Label label13;

	private Label label5;

	public UltraCombo cmbParentCategory;

	private Label label6;

	internal UltraCombo CmbCityName;

	private CheckBox chkOnlyDiscountedRows;

	private Label label7;

	private Label label3;

	public UltraCombo CmbPurchaseType;

	public UltraCombo CmbPoNo;

	private Label label2;

	private CheckBox chkFreightAuditbyCity;

	public frmGrnAudit_History(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmGRNHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1070, 600);
			waitForm.Show(this);
			AllDropDownBind();
			ParameterFill();
			if (!RequestedByOtherDocument)
			{
				cmbperemeter.Focus();
				if (Conversion.ToInt(cmbperemeter.Value) == 0)
				{
					((UltraGridBase)cmbperemeter).Rows[0].Activate();
				}
			}
			else
			{
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void AllDropDownBind()
	{
		try
		{
			DataTable dtAll = InvGrn.GetDataForDropDownFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, "46", (string)null, UserAccount.BranchesId.ToString(), clsGlobalVariables.ActiveYr.Id);
			if (dtAll.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rowsPurchaseTyoe = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "PurchaseType"
					select r;
				DataTable dtPurchaseTyoe = (rowsPurchaseTyoe.Any() ? rowsPurchaseTyoe.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsParentCategory = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "ParentCategory"
					select r;
				DataTable dtParentCategory = (rowsParentCategory.Any() ? rowsParentCategory.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsPo = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "PurchaseOrderNo"
					select r;
				DataTable dtPoNo = (rowsPo.Any() ? rowsPo.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rows = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Supplier"
					select r;
				DataTable dtParty = (rows.Any() ? rows.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsCity = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "City"
					select r;
				DataTable dtCity = (rowsCity.Any() ? rowsCity.CopyToDataTable() : dtAll.Clone());
				InfragisticsHelper.BindAndRetainSelection(CmbPurchaseType, dtPurchaseTyoe, "Id", "ReferenceName", "Purchase Type", AllColumns: false, Conversion.ToInt(CmbPurchaseType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)CmbPurchaseType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				InfragisticsHelper.BindAndRetainSelection(cmbParentCategory, dtParentCategory, "Id", "ReferenceName", "Parent Category", AllColumns: false, Conversion.ToInt(cmbParentCategory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)cmbParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				InfragisticsHelper.BindAndRetainSelection(CmbPoNo, dtPoNo, "Id", "ReferenceName", "Po No", AllColumns: false, Conversion.ToInt(CmbPoNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)CmbPoNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				InfragisticsHelper.BindAndRetainSelection(CmbSupplier, dtParty, "Id", "ReferenceName", "Supplier Name", AllColumns: false, Conversion.ToInt(CmbSupplier.Value), insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)CmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				InfragisticsHelper.BindAndRetainSelection(CmbCityName, dtCity, "Id", "ReferenceName", "CityName", AllColumns: false, Conversion.ToInt(CmbCityName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)CmbCityName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				GRNfromdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				GRNfromdate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				GRNfromdate.Value = first;
				GRNToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				GRNfromdate.Value = first2;
				GRNToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				GRNfromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridHisory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.BranchesId = UserAccount.BranchesId;
			grn.FromDate = Conversion.ToDateTime((object)GRNfromdate.Value);
			grn.ToDate = Conversion.ToDateTime((object)GRNToDate.Value);
			grn.RefDocumentTypeId = Conversion.ToInt(CmbPurchaseType.Value);
			grn.ParentCategoryId = Conversion.ToInt(cmbParentCategory.Value);
			grn.OrderId = Conversion.ToInt(CmbPoNo.Value);
			grn.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			grn.CityId = Conversion.ToInt(CmbCityName.Value);
			if (chkOnlyDiscountedRows.Checked)
			{
				grn.FreightDiscountAmount = 1.0;
			}
			if (chkFreightAuditbyCity.Checked)
			{
				grn.ActionId = 1;
			}
			dt = InvGrn.GetGrnDataForAudit(grn);
			if (dt.Rows.Count > 0)
			{
				DataTable dtHisGrid = new DataTable();
				dtHisGrid.Columns.Add("Id", typeof(int));
				dtHisGrid.Columns.Add("DocNo", typeof(int));
				dtHisGrid.Columns.Add("DocDate", typeof(DateTime));
				dtHisGrid.Columns.Add("VehicleNo", typeof(string));
				dtHisGrid.Columns.Add("BiltyNo", typeof(string));
				dtHisGrid.Columns.Add("InwardGatePassId", typeof(int));
				dtHisGrid.Columns.Add("GpSrNo", typeof(int));
				dtHisGrid.Columns.Add("OrderId", typeof(int));
				dtHisGrid.Columns.Add("OrderNo", typeof(int));
				dtHisGrid.Columns.Add("TransType", typeof(string));
				dtHisGrid.Columns.Add("SupplierCustomerId", typeof(int));
				dtHisGrid.Columns.Add("SupplierName", typeof(string));
				dtHisGrid.Columns.Add("SupplierWeight", typeof(double));
				dtHisGrid.Columns.Add("FactoryWeight", typeof(double));
				dtHisGrid.Columns.Add("WeightDiff", typeof(double));
				dtHisGrid.Columns.Add("ExcessShort", typeof(string));
				dtHisGrid.Columns.Add("WeightBridgeTolerance", typeof(double));
				dtHisGrid.Columns.Add("NoofBags", typeof(int));
				dtHisGrid.Columns.Add("PolicyName", typeof(string));
				dtHisGrid.Columns.Add("ShortWeight", typeof(double));
				dtHisGrid.Columns.Add("AllowShortage", typeof(double));
				dtHisGrid.Columns.Add("ApplyShortage", typeof(double));
				dtHisGrid.Columns.Add("GrossBillWeight", typeof(double));
				dtHisGrid.Columns.Add("BagType", typeof(string));
				dtHisGrid.Columns.Add("DeliveryTerm", typeof(string));
				dtHisGrid.Columns.Add("Variety", typeof(string));
				dtHisGrid.Columns.Add("EBTerm", typeof(string));
				dtHisGrid.Columns.Add("EBWeightCut", typeof(double));
				dtHisGrid.Columns.Add("QualityCutTouch", typeof(double));
				dtHisGrid.Columns.Add("AdLsWeight", typeof(double));
				dtHisGrid.Columns.Add("BillWeight", typeof(double));
				dtHisGrid.Columns.Add("GrossStockWeight", typeof(double));
				dtHisGrid.Columns.Add("LsEBWStock", typeof(double));
				dtHisGrid.Columns.Add("StockWeight", typeof(double));
				dtHisGrid.Columns.Add("BillStockDiff", typeof(double));
				dtHisGrid.Columns.Add("CityName", typeof(string));
				dtHisGrid.Columns.Add("Freight/40KG", typeof(double));
				dtHisGrid.Columns.Add("BiltyFreight", typeof(double));
				dtHisGrid.Columns.Add("ScaleCharges", typeof(double));
				dtHisGrid.Columns.Add("OtherDeduction", typeof(double));
				dtHisGrid.Columns.Add("AdvanceByParty", typeof(double));
				dtHisGrid.Columns.Add("AdvanceByFactory", typeof(double));
				dtHisGrid.Columns.Add("ShortageAmount", typeof(double));
				dtHisGrid.Columns.Add("DiscountAmount", typeof(double));
				dtHisGrid.Columns.Add("ChargeToPartyAmount", typeof(double));
				dtHisGrid.Columns.Add("FreightDeductionAmount", typeof(double));
				dtHisGrid.Columns.Add("TotalPayableAmount", typeof(double));
				dtHisGrid.Columns.Add("FreightVoucherId", typeof(int));
				dtHisGrid.Columns.Add("NetPaid", typeof(double));
				dtHisGrid.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHisGrid.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocNo"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), dt.Rows[i]["VehicleNo"], dt.Rows[i]["BiltyNo"], dt.Rows[i]["InwardGatePassId"], dt.Rows[i]["GpSrNo"], dt.Rows[i]["OrderId"], dt.Rows[i]["OrderNo"], dt.Rows[i]["TransType"], dt.Rows[i]["SupplierCustomerId"], dt.Rows[i]["SupplierName"], dt.Rows[i]["PartyWeight"], dt.Rows[i]["FactoryWeight"], dt.Rows[i]["WeightDiff"], dt.Rows[i]["ExcessShort"], dt.Rows[i]["WeightBridgeTolerance"], dt.Rows[i]["NoofBags"], dt.Rows[i]["PolicyName"], dt.Rows[i]["ShortWeight"], dt.Rows[i]["AllowShortage"], dt.Rows[i]["ApplyShortage"], dt.Rows[i]["GrossBillWeight"], dt.Rows[i]["BagType"], dt.Rows[i]["DeliveryTerm"], dt.Rows[i]["Variety"], dt.Rows[i]["EBTerm"], dt.Rows[i]["EBWeightCut"], dt.Rows[i]["QualityCut/Touch"], dt.Rows[i]["AdLsWeight"], dt.Rows[i]["BillWeight"], dt.Rows[i]["FactoryWeight"], dt.Rows[i]["LsEBWStock"], dt.Rows[i]["StockWeight"], dt.Rows[i]["BillStockDiff"], dt.Rows[i]["CityName"], dt.Rows[i]["FreightPer40Kg"], dt.Rows[i]["BiltyFreight"], dt.Rows[i]["ScaleCharges"], dt.Rows[i]["OtherDeduction"], dt.Rows[i]["AdvanceByParty"], dt.Rows[i]["AdvanceByFactory"], dt.Rows[i]["ShortageAmount"], dt.Rows[i]["DiscountAmount"], dt.Rows[i]["ChargeToPartyAmount"], dt.Rows[i]["FreightDeductionAmount"], dt.Rows[i]["TotalPayableAmount"], dt.Rows[i]["FreightVoucherId"], dt.Rows[i]["InvoiceFreight"]);
				}
				DataGridHistory.DataSource = dtHisGrid;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "SupplierCustomerId", "OrderId", "InwardGatePassId", "FreightVoucherId", "Variety" };
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 3, 2, hiddenColumns);
			DataGridHistory.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["GpSrNo"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["OrderNo"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["FactoryWeight"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["NetPaid"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Groups.Add("Variety");
			DataGridHistory.RootTable.GroupTotals = (GroupTotals)2;
			DataGridHistory.RootTable.Columns["DocDate"].Width = 73;
			DataGridHistory.RootTable.Columns["DocNo"].Width = 50;
			DataGridHistory.RootTable.Columns["SupplierName"].Width = 200;
			DataGridHistory.RootTable.Columns["TransType"].Width = 100;
			DataGridHistory.RootTable.Columns["DeliveryTerm"].Width = 62;
			DataGridHistory.RootTable.Columns["CityName"].Width = 62;
			DataGridHistory.RootTable.Columns["SupplierWeight"].Width = 75;
			DataGridHistory.RootTable.Columns["FactoryWeight"].Width = 75;
			DataGridHistory.RootTable.Columns["WeightDiff"].Width = 63;
			DataGridHistory.RootTable.Columns["ExcessShort"].Width = 68;
			DataGridHistory.RootTable.Columns["VehicleNo"].Width = 80;
			DataGridHistory.RootTable.Columns["BiltyNo"].Width = 45;
			DataGridHistory.RootTable.Columns["GpSrNo"].Width = 45;
			DataGridHistory.RootTable.Columns["NoofBags"].Width = 60;
			DataGridHistory.RootTable.Columns["BagType"].Width = 68;
			DataGridHistory.RootTable.Columns["GrossBillWeight"].Width = 75;
			DataGridHistory.RootTable.Columns["EBTerm"].Width = 90;
			DataGridHistory.RootTable.Columns["EBWeightCut"].Width = 65;
			DataGridHistory.RootTable.Columns["QualityCutTouch"].Width = 65;
			DataGridHistory.RootTable.Columns["AdLsWeight"].Width = 65;
			DataGridHistory.RootTable.Columns["BillWeight"].Width = 75;
			DataGridHistory.RootTable.Columns["GrossStockWeight"].Width = 75;
			DataGridHistory.RootTable.Columns["StockWeight"].Width = 75;
			DataGridHistory.RootTable.Columns["LsEBWStock"].Width = 65;
			DataGridHistory.RootTable.Columns["BillStockDiff"].Width = 80;
			DataGridHistory.RootTable.Columns["BiltyFreight"].Width = 73;
			DataGridHistory.RootTable.Columns["ScaleCharges"].Width = 73;
			DataGridHistory.RootTable.Columns["OtherDeduction"].Width = 73;
			DataGridHistory.RootTable.Columns["AdvanceByParty"].Width = 73;
			DataGridHistory.RootTable.Columns["AdvanceByFactory"].Width = 73;
			DataGridHistory.RootTable.Columns["TotalPayableAmount"].Width = 73;
			DataGridHistory.RootTable.Columns["NetPaid"].Width = 73;
			DataGridHistory.RootTable.Columns["WeightBridgeTolerance"].Width = 70;
			DataGridHistory.RootTable.Columns["ShortWeight"].Width = 70;
			DataGridHistory.RootTable.Columns["AllowShortage"].Width = 70;
			DataGridHistory.RootTable.Columns["ApplyShortage"].Width = 70;
			DataGridHistory.RootTable.Columns["ShortageAmount"].Width = 70;
			DataGridHistory.RootTable.Columns["FreightDeductionAmount"].Width = 70;
			DataGridHistory.RootTable.Columns["DiscountAmount"].Width = 70;
			DataGridHistory.RootTable.Columns["Freight/40KG"].Width = 70;
			DataGridHistory.RootTable.Columns["ChargeToPartyAmount"].Width = 70;
			DataGridHistory.RootTable.Columns["OrderNo"].Width = 45;
			DataGridHistory.RootTable.Columns["Variety"].Width = 55;
			DataGridHistory.RootTable.Columns["PolicyName"].Width = 70;
			DataGridHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.Columns["QualityCutTouch"].Caption = "Quality Cut / Touch";
			DataGridHistory.RootTable.Columns["EBWeightCut"].Caption = "EB Weight Cut";
			DataGridHistory.RootTable.Columns["NoofBags"].Caption = "No of Bags";
			DataGridHistory.RootTable.Columns["BillWeight"].Caption = "Net Bill Weight";
			DataGridHistory.RootTable.Columns["BillStockDiff"].Caption = "Bill And Stock Weight Diff";
			DataGridHistory.RootTable.Columns["AdLsWeight"].Caption = "Add Less Weight";
			DataGridHistory.RootTable.Columns["StockWeight"].Caption = "Net Stock Weight";
			DataGridHistory.RootTable.Columns["LsEBWStock"].Caption = "Less EB Weight Stock";
			DataGridHistory.RootTable.Columns["EBTerm"].Caption = "EB Term";
			DataGridHistory.RootTable.Columns["DocNo"].Caption = "Grn No";
			DataGridHistory.RootTable.Columns["ShortWeight"].Caption = "Total Short Weight";
			DataGridHistory.RootTable.Columns["AllowShortage"].Caption = "Against Policy Discount Allowed Weight";
			DataGridHistory.RootTable.Columns["ApplyShortage"].Caption = "Short Weight Deducted";
			DataGridHistory.RootTable.Columns["ShortageAmount"].Caption = "Shortage Applied Amount";
			DataGridHistory.RootTable.Columns["ChargeToPartyAmount"].Caption = "Shortage Charge To Supplier";
			DataGridHistory.RootTable.Columns["NetPaid"].Caption = "Net Payable Amount";
			DataGridHistory.RootTable.Columns["FreightDeductionAmount"].Caption = "Deduction From Freight";
			DataGridHistory.RootTable.Columns["PolicyName"].Caption = "Shortage Allow Policy";
			DataGridHistory.RootTable.Columns["DiscountAmount"].Caption = "Freight Discount";
			DataGridHistory.RootTable.Columns["NoofBags"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NoofBags"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NoofBags"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["NoofBags"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["SupplierWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["SupplierWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["SupplierWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["SupplierWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FactoryWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["WeightDiff"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WeightDiff"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WeightDiff"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["WeightDiff"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["GrossBillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["GrossBillWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["GrossBillWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["GrossBillWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["EBWeightCut"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["EBWeightCut"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["EBWeightCut"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["EBWeightCut"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BillWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BillWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["LsEBWStock"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["LsEBWStock"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["LsEBWStock"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["LsEBWStock"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BillStockDiff"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BillStockDiff"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BillStockDiff"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BillStockDiff"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BiltyFreight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BiltyFreight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BiltyFreight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BiltyFreight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["NetPaid"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetPaid"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetPaid"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["NetPaid"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["AdLsWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AdLsWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AdLsWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["AdLsWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ShortWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ShortWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ShortWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ShortWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["AllowShortage"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AllowShortage"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AllowShortage"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["AllowShortage"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ApplyShortage"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ApplyShortage"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ApplyShortage"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ApplyShortage"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ShortageAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ShortageAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ShortageAmount"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ShortageAmount"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ShortageAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ShortageAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ShortageAmount"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ShortageAmount"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["FreightDeductionAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FreightDeductionAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FreightDeductionAmount"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["FreightDeductionAmount"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["DiscountAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["DiscountAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["DiscountAmount"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["DiscountAmount"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ChargeToPartyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ChargeToPartyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ChargeToPartyAmount"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ChargeToPartyAmount"].TotalFormatString = "#,##0.##";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null)
			{
				return;
			}
			string col = e.Column.Key;
			int wagesId = Conversion.ToInt(item.Cells["Id"].Value);
			int inwardGatePassId = Conversion.ToInt(item.Cells["InwardGatePassId"].Value);
			int orderId = Conversion.ToInt(item.Cells["OrderId"].Value);
			int freightVoucherId = Conversion.ToInt(item.Cells["FreightVoucherId"].Value);
			switch (col)
			{
			case "DocNo":
				if (wagesId > 0)
				{
					CommonServices.GrnSlipWithSubReports(wagesId);
				}
				break;
			case "GpSrNo":
				if (inwardGatePassId > 0)
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(inwardGatePassId);
				}
				break;
			case "OrderNo":
				if (orderId > 0)
				{
					new ActivityDetails(UserAccount, orderId).Show();
				}
				break;
			case "FactoryWeight":
				if (inwardGatePassId > 0)
				{
					CommonServices.WbTransactionSlipByGpId(inwardGatePassId, 51);
				}
				break;
			case "NetPaid":
				if (freightVoucherId > 0)
				{
					CommonServices.FreightVoucherSlip241(freightVoucherId);
				}
				break;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void frmGRNHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Alt)
			{
				switch (e.KeyCode)
				{
				case Keys.D1:
					btn334Register_Click(null, null);
					break;
				case Keys.NumPad1:
					btn334Register_Click(null, null);
					break;
				}
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Shoe Record");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+1", "For Print 334-Register");
			dt.Rows.Add("Alt+2", "For Print 335-Register");
			dt.Rows.Add("Ctrl+F5", "For Focus on Combo Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid History");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date combo in Filters Box");
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

	private void Reset()
	{
		try
		{
			GRNfromdate.Focus();
			((Control)(object)CmbSupplier).Text = string.Empty;
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			waitForm.Show(this);
			gridHisory();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			waitForm.Show(this);
			AllDropDownBind();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "DataGridHistory")
			{
				GRNfromdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmGRNHistory_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btn334Register_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "334_01-GrnAuditReport.rpt");
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
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Expected O, but got Unknown
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Expected O, but got Unknown
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
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
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Reports.frmGrnAudit_History));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label13 = new System.Windows.Forms.Label();
		this.cmbperemeter = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.GRNfromdate = new System.Windows.Forms.DateTimePicker();
		this.CmbSupplier = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.GRNToDate = new System.Windows.Forms.DateTimePicker();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btn334_01Register = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbParentCategory = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbCityName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbPoNo = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbPurchaseType = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.chkOnlyDiscountedRows = new System.Windows.Forms.CheckBox();
		this.chkFreightAuditbyCity = new System.Windows.Forms.CheckBox();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPoNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPurchaseType).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1064, 561);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 167);
		this.panel5.Margin = new System.Windows.Forms.Padding(4);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1064, 394);
		this.panel5.TabIndex = 3;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1064, 365);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(4);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1064, 29);
		this.panel6.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1017, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 3;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 4);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(116, 19);
		this.label4.TabIndex = 2;
		this.label4.Text = "Audit History";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(4);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1064, 110);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add(this.chkFreightAuditbyCity);
		this.groupBox1.Controls.Add(this.chkOnlyDiscountedRows);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPurchaseType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPoNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentCategory);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityName);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.GRNfromdate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.GRNToDate);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1056, 104);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(6, 77);
		this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 591;
		this.label13.Text = "To Date";
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance62;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(72, 15);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 1;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(6, 21);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 588;
		this.label12.Text = "Date Type";
		this.GRNfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.CustomFormat = "dd-MMM-yy";
		this.GRNfromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNfromdate.Location = new System.Drawing.Point(72, 45);
		this.GRNfromdate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNfromdate.Name = "GRNfromdate";
		this.GRNfromdate.Size = new System.Drawing.Size(160, 23);
		this.GRNfromdate.TabIndex = 2;
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(603, 15);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 11;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(517, 21);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(86, 15);
		this.label1.TabIndex = 121;
		this.label1.Text = "Supplier Name";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(6, 49);
		this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 119;
		this.label27.Text = "From Date";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(763, 72);
		this.btnshow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(60, 24);
		this.btnshow.TabIndex = 15;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.GRNToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.CustomFormat = "dd-MMM-yy";
		this.GRNToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNToDate.Location = new System.Drawing.Point(72, 73);
		this.GRNToDate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNToDate.Name = "GRNToDate";
		this.GRNToDate.Size = new System.Drawing.Size(160, 23);
		this.GRNToDate.TabIndex = 3;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Margin = new System.Windows.Forms.Padding(4);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1064, 29);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 4);
		this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(248, 19);
		this.label23.TabIndex = 2;
		this.label23.Text = "Goods Receipt Audit Report -";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(4);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1064, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.btn334_01Register, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1064, 27);
		this.toolStrip3.TabIndex = 2;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btn334_01Register.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn334_01Register.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn334_01Register.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn334_01Register.Name = "btn334_01Register";
		this.btn334_01Register.Size = new System.Drawing.Size(139, 24);
		this.btn334_01Register.Text = "334_01-Register";
		this.btn334_01Register.Click += new System.EventHandler(btn334Register_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(121, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(235, 49);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(91, 15);
		this.label5.TabIndex = 595;
		this.label5.Text = "Parent Category";
		this.cmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Location = new System.Drawing.Point(343, 43);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Name = "cmbParentCategory";
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).TabIndex = 592;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(517, 49);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(59, 15);
		this.label6.TabIndex = 594;
		this.label6.Text = "CityName";
		this.CmbCityName.AllowNull = (DefaultableBoolean)1;
		this.CmbCityName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbCityName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCityName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Location = new System.Drawing.Point(603, 43);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Name = "CmbCityName";
		((System.Windows.Forms.Control)(object)this.CmbCityName).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityName).TabIndex = 593;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(253, 5);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(643, 17);
		this.label2.TabIndex = 2;
		this.label2.Text = "Supplier && Factory weight, Empty Bags deductions, Freight By City and Freight Audit Report";
		this.CmbPoNo.AllowNull = (DefaultableBoolean)1;
		this.CmbPoNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPoNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPoNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPoNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbPoNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPoNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPoNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPoNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPoNo).Location = new System.Drawing.Point(343, 71);
		((System.Windows.Forms.Control)(object)this.CmbPoNo).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbPoNo).Name = "CmbPoNo";
		((System.Windows.Forms.Control)(object)this.CmbPoNo).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbPoNo).TabIndex = 592;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(235, 77);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(107, 15);
		this.label3.TabIndex = 595;
		this.label3.Text = "Purchase Order No";
		this.CmbPurchaseType.AllowNull = (DefaultableBoolean)1;
		this.CmbPurchaseType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPurchaseType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPurchaseType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPurchaseType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbPurchaseType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPurchaseType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPurchaseType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPurchaseType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPurchaseType).Location = new System.Drawing.Point(343, 15);
		((System.Windows.Forms.Control)(object)this.CmbPurchaseType).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbPurchaseType).Name = "CmbPurchaseType";
		((System.Windows.Forms.Control)(object)this.CmbPurchaseType).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbPurchaseType).TabIndex = 592;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(235, 21);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(83, 15);
		this.label7.TabIndex = 595;
		this.label7.Text = "Purchase Type";
		this.chkOnlyDiscountedRows.AutoSize = true;
		this.chkOnlyDiscountedRows.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkOnlyDiscountedRows.Location = new System.Drawing.Point(827, 18);
		this.chkOnlyDiscountedRows.Name = "chkOnlyDiscountedRows";
		this.chkOnlyDiscountedRows.Size = new System.Drawing.Size(228, 19);
		this.chkOnlyDiscountedRows.TabIndex = 596;
		this.chkOnlyDiscountedRows.Text = "Show Only Freight-Discounted Rows";
		this.chkOnlyDiscountedRows.UseVisualStyleBackColor = true;
		this.chkFreightAuditbyCity.AutoSize = true;
		this.chkFreightAuditbyCity.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkFreightAuditbyCity.Location = new System.Drawing.Point(827, 46);
		this.chkFreightAuditbyCity.Name = "chkFreightAuditbyCity";
		this.chkFreightAuditbyCity.Size = new System.Drawing.Size(139, 19);
		this.chkFreightAuditbyCity.TabIndex = 596;
		this.chkFreightAuditbyCity.Text = "Freight Audit by City";
		this.chkFreightAuditbyCity.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1064, 561);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmGrnAudit_History";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmGRNHistory_FormClosing);
		base.Load += new System.EventHandler(frmGRNHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmGRNHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPoNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPurchaseType).EndInit();
		base.ResumeLayout(false);
	}
}

}
