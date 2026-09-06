// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
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

public class frmSaleInvoiceDirectRegister : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtSale = new DataTable();

	private DataTable dtBranch = new DataTable();

	public bool RequestFromAnotherForm;

	public DateTime FromDate;

	public DateTime ToDate;

	public int StockAccountId;

	public int ItemId;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton print;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Label label6;

	private Label label27;

	private Label label1;

	private Panel panel5;

	internal GridEX DataGridHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private Label label2;

	private Label label3;

	private ToolStripButton btnRefresh;

	private Panel panel6;

	private Label label4;

	private Label label5;

	private GroupBox groupBox1;

	private ToolStripButton btnShortcurkeys;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label8;

	private Label label7;

	private ToolStripDropDownButton tsDropDown;

	private Label label9;

	public UltraCombo cmbitem;

	public DateTimePicker GRNToDate;

	public DateTimePicker GRNfromdate;

	public UltraCombo CmbSupplier;

	public UltraCombo CmbJobLot;

	public UltraCombo CmbWarehouse;

	public UltraCombo cmbDateType;

	public UltraCombo CmbActivity;

	public UltraCombo CmbDocumentType;

	public UltraCombo CmbBranchName;

	internal UltraCombo CmbStockAccount;

	private Label label38;

	public frmSaleInvoiceDirectRegister(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmSaleInvoiceRegister_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(593, 690);
			ctrlGrdBar2.mGridSaveLayouts.Visible = false;
			ctrlGrdBar2.mGridChooseFielder.Visible = false;
			ctrlGrdBar2.removeLayoutToolStripMenuItem.Visible = false;
			ReportsLoad();
			BranchesFill();
			Datetypefill();
			AllComboBindAgainstSale();
			DocumentTypeFill();
			ActivityFill();
			if (!RequestFromAnotherForm)
			{
				((UltraGridBase)cmbDateType).Rows[2].Activate();
				GRNfromdate.Value = DateTime.Now.AddDays(-15.0);
			}
			else
			{
				GRNfromdate.Value = FromDate;
				GRNToDate.Value = ToDate;
				if (StockAccountId > 0)
				{
					CmbStockAccount.Value = StockAccountId;
				}
				if (ItemId > 0)
				{
					cmbitem.Value = ItemId;
				}
				((UltraGridBase)CmbActivity).Rows[1].Activate();
				gridHisory();
			}
			cmbDateType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportsLoad()
	{
		try
		{
			string[] files = CommonServices.DynamicReportsLoad("SaleDirectRegister");
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

	private void AllComboBindAgainstSale()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.UserId = UserAccount.ID;
			obj.DocumentTypeIds = "99,139,184";
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
			}
			DataTable dt = InvSaleInvoice.AllComboBindAgainstSaleInvoice(obj);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtsup = new DataTable();
			dtsup.Columns.Add("Id", typeof(int));
			dtsup.Columns.Add("SupplierName");
			DataTable dtWh = new DataTable();
			dtWh.Columns.Add("Id", typeof(int));
			dtWh.Columns.Add("Warehouse");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			DataTable dtSa = new DataTable();
			dtSa.Columns.Add("Id", typeof(int));
			dtSa.Columns.Add("Name");
			DataTable dtJob = new DataTable();
			dtJob.Columns.Add("Id", typeof(int));
			dtJob.Columns.Add("JobLot");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
				{
					dtsup.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "WareHouse")
				{
					dtWh.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ItemName")
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "JobLot")
				{
					dtJob.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "StockAccount")
				{
					dtSa.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtsup, CmbSupplier, "Id", "SupplierName", "Supplier Name", ZeroIndex: true);
			((UltraGridBase)CmbSupplier).Rows[0].Activate();
			DDL.BindDDL(dtWh, CmbWarehouse, "Id", "Warehouse", "Warehouse", ZeroIndex: true);
			((UltraGridBase)CmbWarehouse).Rows[0].Activate();
			DDL.BindDDL(dtItem, cmbitem, "Id", "ItemName", "Item Name", ZeroIndex: true);
			((UltraGridBase)cmbitem).Rows[0].Activate();
			DDL.BindDDL(dtJob, CmbJobLot, "Id", "JobLot", "Job Lot", ZeroIndex: true);
			((UltraGridBase)CmbJobLot).Rows[0].Activate();
			DDL.BindDDL(dtSa, CmbStockAccount, "Id", "Name", "Stock Account", ZeroIndex: true);
			((UltraGridBase)CmbStockAccount).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierCustomer()
	{
		try
		{
			DataTable dt = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbSupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		try
		{
			DataTable dt = CommonServices.ItemGetForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbitem, "Id", "ItemName", "Item Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFill()
	{
		try
		{
			DataTable dt = CommonServices.JobLotGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbJobLot, "Id", "JobLotDescription", "JobLotDescription", false);
				((UltraGridBase)CmbJobLot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFill()
	{
		try
		{
			DataTable dt = CommonServices.WareHouseGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbWarehouse, "Id", "WareHouseName", "WareHouseName", false);
				((UltraGridBase)CmbWarehouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			dtBranch = InvSaleInvoice.GetBranchsAllocatedToUserFromSaleInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 99);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, CmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)CmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)CmbBranchName).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ActivityFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Sale");
			dt.Rows.Add(2, "Sale Return");
			DDL.BindDDL(dt, CmbActivity, "Id", "Name", "Activity", ZeroIndex: true);
			((UltraGridBase)CmbActivity).Rows[0].Activate();
			((UltraGridBase)CmbActivity).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DocumentType");
			dt.Rows.Add(99, "SID");
			dt.Rows.Add(184, "SID-Auto");
			DDL.BindDDL(dt, CmbDocumentType, "Id", "DocumentType", "Document Type", ZeroIndex: true);
			((UltraGridBase)CmbDocumentType).Rows[0].Activate();
			((UltraGridBase)CmbDocumentType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateType, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridHisory()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtSale.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = GRNfromdate.Value;
			obj.ToDate = GRNToDate.Value;
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(cmbitem.Value);
			obj.JobLotId = Conversion.ToInt(CmbJobLot.Value);
			obj.WarehouseId = Conversion.ToInt(CmbWarehouse.Value);
			obj.DocumentTypeId = Conversion.ToInt(CmbDocumentType.Value);
			obj.StockAccountId = Conversion.ToInt(CmbStockAccount.Value);
			obj.Activity = Conversion.ToString((object)((Control)(object)CmbActivity).Text);
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
				dtSale = InvSaleInvoiceReports.InvSaleInvoiceDirectRegister(obj);
				if (dtSale.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("BranchSrNo", typeof(int));
					dt.Columns.Add("BranchName", typeof(string));
					dt.Columns.Add("TranType", typeof(string));
					dt.Columns.Add("DocumentType", typeof(string));
					dt.Columns.Add("DocNo", typeof(int));
					dt.Columns.Add("DocDate", typeof(DateTime));
					dt.Columns.Add("ManualBillNo", typeof(string));
					dt.Columns.Add("CustomerName", typeof(string));
					dt.Columns.Add("PaymentTerm", typeof(string));
					dt.Columns.Add("WareHouseName", typeof(string));
					dt.Columns.Add("ItemName", typeof(string));
					dt.Columns.Add("JobLot", typeof(string));
					dt.Columns.Add("CropYear", typeof(string));
					dt.Columns.Add("PackingType", typeof(string));
					dt.Columns.Add("ItemQty", typeof(string));
					dt.Columns.Add("PackUom", typeof(string));
					dt.Columns.Add("NetBillWeight", typeof(double));
					dt.Columns.Add("RateUom", typeof(string));
					dt.Columns.Add("ItemRate", typeof(double));
					dt.Columns.Add("RateCut", typeof(double));
					dt.Columns.Add("RateCutAmount", typeof(double));
					dt.Columns.Add("ItemAmount", typeof(double));
					dt.Columns.Add("DiscountType", typeof(string));
					dt.Columns.Add("Discount%", typeof(double));
					dt.Columns.Add("DiscountAmount", typeof(double));
					dt.Columns.Add("AmountWithDiscount", typeof(double));
					dt.Columns.Add("CityName", typeof(string));
					dt.Columns.Add("VehicleNo", typeof(string));
					dt.Columns.Add("FreightAmount", typeof(double));
					dt.Columns.Add("commissionAmount", typeof(double));
					dt.Columns.Add("ExpenseAmount", typeof(double));
					dt.Columns.Add("RemarksHeader", typeof(string));
					for (int i = 0; i < dtSale.Rows.Count; i++)
					{
						dt.Rows.Add(Conversion.ToInt(dtSale.Rows[i]["Id"]), Conversion.ToInt(dtSale.Rows[i]["BranchSrNo"]), Conversion.ToString(dtSale.Rows[i]["BranchName"]), Conversion.ToString(dtSale.Rows[i]["TransactionType"]), Conversion.ToString(dtSale.Rows[i]["DocumentType"]), Conversion.ToInt(dtSale.Rows[i]["DocNo"]), Conversion.ToDateTime(dtSale.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToString(dtSale.Rows[i]["ManualBillNo"]), Conversion.ToString(dtSale.Rows[i]["CustomerName"]), Conversion.ToString(dtSale.Rows[i]["TermsDescription"]), Conversion.ToString(dtSale.Rows[i]["WareHouseName"]), Conversion.ToString(dtSale.Rows[i]["ItemName"]), Conversion.ToString(dtSale.Rows[i]["JobLotDescription"]), Conversion.ToString(dtSale.Rows[i]["CropYear"]), Conversion.ToString(dtSale.Rows[i]["PackTypeDesc"]), Conversion.ToDouble(dtSale.Rows[i]["ItemQty"]), Conversion.ToString(dtSale.Rows[i]["ItemUom"]), Conversion.ToDouble(dtSale.Rows[i]["NetBillWeight"]), Conversion.ToString(dtSale.Rows[i]["RateUom"]), Conversion.ToDouble(dtSale.Rows[i]["ItemRate"]), Conversion.ToDouble(dtSale.Rows[i]["RateCut"]), Conversion.ToDouble(dtSale.Rows[i]["RateCutAmount"]), Conversion.ToDouble(dtSale.Rows[i]["ItemAmount"]), Conversion.ToString(dtSale.Rows[i]["DiscountType"]), Conversion.ToDouble(dtSale.Rows[i]["ItemDiscount"]), Conversion.ToDouble(dtSale.Rows[i]["ItemDiscountAmount"]), Conversion.ToDouble(dtSale.Rows[i]["ItemAmountWithDiscount"]), Conversion.ToString(dtSale.Rows[i]["CityName"]), Conversion.ToString(dtSale.Rows[i]["VehicleNo"]), Conversion.ToDouble(dtSale.Rows[i]["FreightAmount"]), Conversion.ToDouble(dtSale.Rows[i]["CommissionAmount"]), Conversion.ToDouble(dtSale.Rows[i]["ExpenseAmount"]), Conversion.ToString(dtSale.Rows[i]["RemarksHeader"]));
					}
					DataGridHistory.DataSource = dt;
					GridSettings();
				}
				else
				{
					DataGridHistory.DataSource = null;
				}
				return;
			}
			CmbBranchName.Focus();
			throw new Exception("Select Branch First");
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
			gridHisory();
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
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.ColumnSets["RateCut"].Visible = false;
			DataGridHistory.RootTable.ColumnSets["RateCutAmount"].Visible = false;
			DataGridHistory.RootTable.ColumnSets["FreightAmount"].Visible = false;
			DataGridHistory.RootTable.ColumnSets["commissionAmount"].Visible = false;
			DataGridHistory.RootTable.ColumnSets["ExpenseAmount"].Visible = false;
			DataGridHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.ColumnSets["BranchSrNo"].Width = 70;
			DataGridHistory.RootTable.ColumnSets["BranchName"].Width = 150;
			DataGridHistory.RootTable.ColumnSets["TranType"].Width = 60;
			DataGridHistory.RootTable.ColumnSets["DocumentType"].Width = 70;
			DataGridHistory.RootTable.ColumnSets["DocNo"].Width = 40;
			DataGridHistory.RootTable.ColumnSets["DocDate"].Width = 70;
			DataGridHistory.RootTable.ColumnSets["ManualBillNo"].Width = 70;
			DataGridHistory.RootTable.ColumnSets["CustomerName"].Width = 150;
			DataGridHistory.RootTable.ColumnSets["PaymentTerm"].Width = 70;
			DataGridHistory.RootTable.ColumnSets["WareHouseName"].Width = 120;
			DataGridHistory.RootTable.ColumnSets["ItemName"].Width = 150;
			DataGridHistory.RootTable.ColumnSets["JobLot"].Width = 100;
			DataGridHistory.RootTable.ColumnSets["CropYear"].Width = 60;
			DataGridHistory.RootTable.ColumnSets["PackingType"].Width = 80;
			DataGridHistory.RootTable.ColumnSets["ItemQty"].Width = 70;
			DataGridHistory.RootTable.ColumnSets["PackUom"].Width = 70;
			DataGridHistory.RootTable.ColumnSets["NetBillWeight"].Width = 80;
			DataGridHistory.RootTable.ColumnSets["RateUom"].Width = 50;
			DataGridHistory.RootTable.ColumnSets["ItemRate"].Width = 50;
			DataGridHistory.RootTable.ColumnSets["RateCut"].Width = 100;
			DataGridHistory.RootTable.ColumnSets["RateCutAmount"].Width = 100;
			DataGridHistory.RootTable.ColumnSets["ItemAmount"].Width = 80;
			DataGridHistory.RootTable.ColumnSets["DiscountType"].Width = 80;
			DataGridHistory.RootTable.ColumnSets["Discount%"].Width = 60;
			DataGridHistory.RootTable.ColumnSets["DiscountAmount"].Width = 80;
			DataGridHistory.RootTable.ColumnSets["AmountWithDiscount"].Width = 90;
			DataGridHistory.RootTable.ColumnSets["CityName"].Width = 90;
			DataGridHistory.RootTable.ColumnSets["VehicleNo"].Width = 80;
			DataGridHistory.RootTable.ColumnSets["FreightAmount"].Width = 120;
			DataGridHistory.RootTable.ColumnSets["commissionAmount"].Width = 120;
			DataGridHistory.RootTable.ColumnSets["ExpenseAmount"].Width = 120;
			DataGridHistory.RootTable.ColumnSets["RemarksHeader"].Width = 200;
			DataGridHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Discount%"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Discount%"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["DiscountAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["DiscountAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["DiscountAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["DiscountAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["AmountWithDiscount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AmountWithDiscount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AmountWithDiscount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["AmountWithDiscount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ExpenseAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ExpenseAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtSale.Rows.Count == 0 || dtSale == null)
			{
				MessageBox.Show("Record Not Found For Dispaly");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtSale, "302-InvSaleInvoice.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleInvoiceDirectRegister_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.P)
			{
				print_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl + P", "For print");
			dt.Rows.Add("Ctrl+F5", "For Focus On DateType ");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On DateType in Filter");
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

	private void btnShortcurkeys_Click(object sender, EventArgs e)
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			reset();
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
			WareHouseFill();
			JobLotFill();
			ItemDetailFill();
			SupplierCustomer();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void reset()
	{
		try
		{
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)cmbitem).Text = string.Empty;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)CmbWarehouse).Text = string.Empty;
			gridHisory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleInvoiceDirectRegister_FormClosing(object sender, FormClosingEventArgs e)
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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

	private void cmbDateType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateType.Value) == 1)
			{
				GRNfromdate.Value = DateTime.Today;
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 2)
			{
				GRNfromdate.Value = DateTime.Today.AddDays(-7.0);
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				GRNfromdate.Value = first;
				GRNToDate.Value = DateTime.Today;
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				GRNfromdate.Value = first2;
				GRNToDate.Value = DateTime.Today;
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 5)
			{
				GRNfromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "DataGridHistory")
			{
				cmbDateType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tsDropDown_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtSale.Rows.Count == 0 || dtSale == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtSale, e.ClickedItem.Text + ".rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbBranchName).Text != string.Empty && ((UltraGridBase)CmbBranchName).ActiveRow != null)
			{
				AllComboBindAgainstSale();
				return;
			}
			((Control)(object)CmbSupplier).Text = string.Empty;
			((UltraGridBase)CmbSupplier).DataSource = null;
			((Control)(object)cmbitem).Text = string.Empty;
			((UltraGridBase)cmbitem).DataSource = null;
			((Control)(object)CmbWarehouse).Text = string.Empty;
			((UltraGridBase)CmbWarehouse).DataSource = null;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((UltraGridBase)CmbJobLot).DataSource = null;
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
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Expected O, but got Unknown
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Reports.frmSaleInvoiceDirectRegister));
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
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
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbBranchName = new UltraCombo();
		this.CmbActivity = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbDocumentType = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbDateType = new UltraCombo();
		this.CmbSupplier = new UltraCombo();
		this.CmbWarehouse = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.GRNfromdate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.GRNToDate = new System.Windows.Forms.DateTimePicker();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbitem = new UltraCombo();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.tsDropDown = new System.Windows.Forms.ToolStripDropDownButton();
		this.btnShortcurkeys = new System.Windows.Forms.ToolStripButton();
		this.CmbStockAccount = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbActivity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccount).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(850, 651);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 215);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(850, 436);
		this.panel5.TabIndex = 3;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = resources.GetString("DataGridHistory_DesignTimeLayout.LayoutString");
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(850, 408);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(850, 28);
		this.panel6.TabIndex = 2;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(807, 2);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(41, 23);
		this.ctrlGrdBar2.TabIndex = 7;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(4, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 19);
		this.label4.TabIndex = 6;
		this.label4.Text = "History";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 56);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(850, 159);
		this.panel4.TabIndex = 2;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockAccount);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranchName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbActivity);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocumentType);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.GRNfromdate);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.GRNToDate);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(832, 157);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(11, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(79, 15);
		this.label9.TabIndex = 204;
		this.label9.Text = "Branch Name";
		this.CmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.CmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance62;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance67).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Location = new System.Drawing.Point(117, 13);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Name = "CmbBranchName";
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Leave += new System.EventHandler(CmbBranchName_Leave);
		this.CmbActivity.AllowNull = (DefaultableBoolean)1;
		this.CmbActivity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbActivity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbActivity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbActivity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbActivity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbActivity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbActivity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbActivity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbActivity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbActivity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbActivity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbActivity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbActivity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbActivity).Location = new System.Drawing.Point(419, 125);
		((System.Windows.Forms.Control)(object)this.CmbActivity).Name = "CmbActivity";
		((System.Windows.Forms.Control)(object)this.CmbActivity).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbActivity).TabIndex = 10;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(625, 125);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(63, 26);
		this.btnshow.TabIndex = 11;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(326, 131);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(47, 15);
		this.label8.TabIndex = 202;
		this.label8.Text = "Activity";
		this.CmbDocumentType.AllowNull = (DefaultableBoolean)1;
		this.CmbDocumentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocumentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocumentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDocumentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Location = new System.Drawing.Point(419, 97);
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Name = "CmbDocumentType";
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).TabIndex = 9;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(326, 103);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(92, 15);
		this.label7.TabIndex = 200;
		this.label7.Text = "Document Type";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance85;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance91).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(419, 68);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 8;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(11, 46);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(60, 15);
		this.label5.TabIndex = 198;
		this.label5.Text = "Date Type";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(11, 103);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 15);
		this.label1.TabIndex = 185;
		this.label1.Text = "Customer Name";
		this.cmbDateType.AllowNull = (DefaultableBoolean)1;
		this.cmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance98;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Location = new System.Drawing.Point(117, 40);
		((System.Windows.Forms.Control)(object)this.cmbDateType).Name = "cmbDateType";
		((System.Windows.Forms.Control)(object)this.cmbDateType).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateType).TabIndex = 1;
		this.cmbDateType.ValueChanged += new System.EventHandler(cmbDateType_ValueChanged);
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance25;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(117, 97);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 4;
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(117, 125);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 5;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(11, 131);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(69, 15);
		this.label3.TabIndex = 196;
		this.label3.Text = "WareHouse";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(11, 74);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(91, 15);
		this.label27.TabIndex = 190;
		this.label27.Text = "From && To Date";
		this.GRNfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.CustomFormat = "dd-MMM-yy";
		this.GRNfromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNfromdate.Location = new System.Drawing.Point(117, 70);
		this.GRNfromdate.Name = "GRNfromdate";
		this.GRNfromdate.Size = new System.Drawing.Size(102, 23);
		this.GRNfromdate.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(326, 74);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(46, 15);
		this.label2.TabIndex = 196;
		this.label2.Text = "Job Lot";
		this.GRNToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.CustomFormat = "dd-MMM-yy";
		this.GRNToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNToDate.Location = new System.Drawing.Point(221, 70);
		this.GRNToDate.Name = "GRNToDate";
		this.GRNToDate.Size = new System.Drawing.Size(102, 23);
		this.GRNToDate.TabIndex = 3;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(326, 19);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 194;
		this.label6.Text = "Item Name";
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance90).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(419, 13);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 6;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(850, 28);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(225, 19);
		this.label23.TabIndex = 6;
		this.label23.Text = "Sale Invoice Direct History";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(850, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnNew, this.btnRefresh, this.print, this.tsDropDown, this.btnShortcurkeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(850, 27);
		this.toolStrip3.TabIndex = 10;
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
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(92, 24);
		this.print.Text = "302-&Print";
		this.print.Click += new System.EventHandler(print_Click);
		this.tsDropDown.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsDropDown.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDown.Name = "tsDropDown";
		this.tsDropDown.Size = new System.Drawing.Size(70, 24);
		this.tsDropDown.Text = "Print";
		this.tsDropDown.ToolTipText = "SaleDirectRegister";
		this.tsDropDown.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDown_DropDownItemClicked);
		this.btnShortcurkeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortcurkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcurkeys.Image = (System.Drawing.Image)resources.GetObject("btnShortcurkeys.Image");
		this.btnShortcurkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcurkeys.Name = "btnShortcurkeys";
		this.btnShortcurkeys.Size = new System.Drawing.Size(106, 24);
		this.btnShortcurkeys.Text = "ShortCut Keys";
		this.btnShortcurkeys.Click += new System.EventHandler(btnShortcurkeys_Click);
		this.CmbStockAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbStockAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance22;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStockAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Location = new System.Drawing.Point(419, 40);
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Name = "CmbStockAccount";
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).TabIndex = 7;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(326, 46);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(59, 15);
		this.label38.TabIndex = 613;
		this.label38.Text = "Stock A/c";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(850, 651);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmSaleInvoiceDirectRegister";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSaleInvoiceDirectRegister_FormClosing);
		base.Load += new System.EventHandler(frmSaleInvoiceRegister_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleInvoiceDirectRegister_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbActivity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccount).EndInit();
		base.ResumeLayout(false);
	}
}

}
