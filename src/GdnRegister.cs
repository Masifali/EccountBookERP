// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
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
namespace Architecture.WinApp.pcc.Reports
{

public class GdnRegister : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtGrid = new DataTable();

	public DataTable dtBindCombo = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Label label4;

	private TextBox txtOrderNoFrom;

	private TextBox txtOrderNoTo;

	private GroupBox groupBox1;

	private ToolStrip toolStrip1;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	private Label label9;

	internal UltraCombo cmbItem;

	internal UltraCombo cmbItemtype;

	private Label label5;

	internal UltraCombo cmbCategory;

	private Label label8;

	internal UltraCombo cmbParentCategory;

	private Label label6;

	private Label label3;

	internal UltraCombo cmbApproveStatus;

	private Label label13;

	internal UltraCombo cmbperemeter;

	private Label label15;

	private Label label1;

	private ToolStripButton printRegister;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	internal GridEX grdDetail;

	private Panel panel4;

	internal GridEX grdMain;

	private Panel panel2;

	internal GridEX grd;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton BtnRegister;

	private Label label7;

	private RadioButton RadWithCost;

	private RadioButton RadFOC;

	private RadioButton RadAll;

	public GdnRegister(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 700);
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			ComboFill();
			ParameterFill();
			StatusFill();
			FillDetailDataGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "UnApproved");
			dt.Rows.Add(2, "Approve");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, cmbApproveStatus, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbApproveStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
				((UltraGridBase)cmbperemeter).Rows[1].Activate();
			}
			else
			{
				((Control)(object)cmbperemeter).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				FromDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				FromDate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				FromDate.Value = first;
				Todate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				Todate.Value = first2;
				FromDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		try
		{
			DataTable dtParentCategory = new DataTable();
			dtParentCategory.Columns.Add("Id");
			dtParentCategory.Columns.Add("name");
			DataTable dtItemCategories = new DataTable();
			dtItemCategories.Columns.Add("Id");
			dtItemCategories.Columns.Add("name");
			DataTable dtItemType = new DataTable();
			dtItemType.Columns.Add("Id");
			dtItemType.Columns.Add("name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("name");
			DataTable dtJob = new DataTable();
			dtJob.Columns.Add("Id");
			dtJob.Columns.Add("name");
			DataTable dtDistrict = new DataTable();
			dtDistrict.Columns.Add("Id");
			dtDistrict.Columns.Add("name");
			DataTable dtCity = new DataTable();
			dtCity.Columns.Add("Id");
			dtCity.Columns.Add("name");
			DataTable dtSupplieerCustomer = new DataTable();
			dtSupplieerCustomer.Columns.Add("Id");
			dtSupplieerCustomer.Columns.Add("name");
			DataTable dtRefParty = new DataTable();
			dtRefParty.Columns.Add("Id");
			dtRefParty.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBindCombo = SaleOrder.GetDataForDropDownFromSaleOrder(obj);
			if (dtBindCombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtBindCombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "ParentCategory")
				{
					dtParentCategory.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "ItemCategory")
				{
					dtItemCategories.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "ItemType")
				{
					dtItemType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "Item")
				{
					dtItem.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "JobLot")
				{
					dtJob.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "District")
				{
					dtDistrict.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "City")
				{
					dtCity.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "Customer")
				{
					dtSupplieerCustomer.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "ReferencePartyName")
				{
					dtRefParty.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtParentCategory.Rows.Count > 0)
			{
				DDL.BindDDL(dtParentCategory, cmbParentCategory, "Id", "name", "Parent Category", ZeroIndex: false);
				((UltraGridBase)cmbParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItemCategories.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemCategories, cmbCategory, "Id", "name", "Item Categories", ZeroIndex: false);
				((UltraGridBase)cmbCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItemType.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemType, cmbItemtype, "Id", "name", "Item Type", ZeroIndex: false);
				((UltraGridBase)cmbItemtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "name", "Item", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtSupplieerCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplieerCustomer, CmbSupplier, "Id", "name", "Customer Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)cmbParentCategory).Text = string.Empty;
			((Control)(object)cmbCategory).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbItemtype).Text = string.Empty;
			((Control)(object)cmbApproveStatus).Text = string.Empty;
			txtOrderNoFrom.Text = string.Empty;
			txtOrderNoTo.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Hide();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			if (tabControl2.SelectedIndex == 0)
			{
				FillDetailDataGrid();
			}
			else if (tabControl2.SelectedIndex == 1)
			{
				FillMainGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailDataGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = Convert.ToDateTime(FromDate.Value);
			obj.ToDate = Convert.ToDateTime(Todate.Value);
			obj.DocumentTypeId = 1855;
			obj.FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemCategoryId = Conversion.ToInt(cmbCategory.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtype.Value);
			obj.InventoryParentCategories = Conversion.ToInt(cmbParentCategory.Value);
			if (((Control)(object)cmbApproveStatus).Text == "Approve")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)cmbApproveStatus).Text == "UnApproved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)cmbApproveStatus).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			if (RadFOC.Checked)
			{
				obj.ActionId = 1;
			}
			if (RadWithCost.Checked)
			{
				obj.ActionId = 2;
			}
			dtGrid = InvGdn.GdnRegister(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DetailId", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo", typeof(string));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DeliveryOrderId", typeof(int));
				dt.Columns.Add("DoOrderDetailId", typeof(int));
				dt.Columns.Add("DeliveryOrderNo", typeof(string));
				dt.Columns.Add("DeliveryOrderDate", typeof(DateTime));
				dt.Columns.Add("OrderId", typeof(int));
				dt.Columns.Add("OrderDetailId", typeof(int));
				dt.Columns.Add("SaleOrderNo", typeof(string));
				dt.Columns.Add("SaleOrderDate", typeof(DateTime));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("ReferenceNo", typeof(string));
				dt.Columns.Add("ReferencPartyName", typeof(string));
				dt.Columns.Add("ReferencPartyAddress", typeof(string));
				dt.Columns.Add("ReferencPartyCellNo", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("GpNo", typeof(string));
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("DriverName", typeof(string));
				dt.Columns.Add("DriverCellNo", typeof(string));
				dt.Columns.Add("DriverCNICNo", typeof(string));
				dt.Columns.Add("WareHouseName", typeof(string));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemArrtibuteVarient", typeof(string));
				dt.Columns.Add("JobLotDescription", typeof(string));
				dt.Columns.Add("CityName", typeof(string));
				dt.Columns.Add("NetWeight", typeof(decimal));
				dt.Columns.Add("ItemQty", typeof(int));
				dt.Columns.Add("IsFOC", typeof(bool));
				dt.Columns.Add("TransporterName", typeof(string));
				dt.Columns.Add("FrieghtAmountHeader", typeof(decimal));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("VisitedByName", typeof(string));
				dt.Columns.Add("RefSalesMan", typeof(string));
				dt.Columns.Add("IsApproved", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUserName", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUserName", typeof(string));
				dt.Columns.Add("BuildingStorey", typeof(string));
				dt.Columns.Add("BuildingHeight", typeof(string));
				dt.Columns.Add("BuildingArea", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Clear();
				foreach (DataRow row in dtGrid.Rows)
				{
					dt.Rows.Add(row["Id"], row["DetailId"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["DeliveryOrderId"], row["DoOrderDetailId"], row["DeliveryOrderNo"], row["DeliveryOrderDate"], row["OrderId"], row["OrderDetailId"], row["SaleOrderNo"], row["SaleOrderDate"], row["SupplierCustomerId"], row["CustomerName"], row["ReferenceNo"], row["ReferencPartyName"], row["ReferencPartyAddress"], row["ReferencPartyCellNo"], row["VehicleNo"], row["GpNo"], row["GpDate"], row["DriverName"], row["DriverCellNo"], row["DriverCNICNo"], row["WareHouseName"], row["ItemCode"], row["ItemName"], row["ItemArrtibuteVarient"], row["JobLotDescription"], row["CityName"], row["NetWeight"], row["ItemQty"], row["IsFOC"], row["TransporterName"], row["FrieghtAmountHeader"], row["DeliveryTerm"], row["VisitedByName"], row["RefSalesMan"], row["IsApproved"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["ApprovedDate"], row["ApprovedUserName"], row["BuildingStorey"], row["BuildingHeight"], row["BuildingArea"], row["NoOfAttachments"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdDetailDataSettings();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailDataSettings()
	{
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["DetailId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["DeliveryOrderId"].Visible = false;
			grd.RootTable.Columns["DoOrderDetailId"].Visible = false;
			grd.RootTable.Columns["OrderId"].Visible = false;
			grd.RootTable.Columns["OrderDetailId"].Visible = false;
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["NoOfAttachments"].Caption = "Attached";
			grd.RootTable.Columns["TransporterName"].Caption = "CarriageAccount";
			grd.RootTable.Columns["DeliveryOrderNo"].Caption = "DoNo";
			grd.RootTable.Columns["DeliveryOrderDate"].Caption = "DoDate";
			grd.RootTable.Columns["SaleOrderNo"].Caption = "SoNo";
			grd.RootTable.Columns["SaleOrderDate"].Caption = "SoDate";
			grd.RootTable.Columns["JobLotDescription"].Caption = "JobLot";
			grd.RootTable.Columns["ItemArrtibuteVarient"].Caption = "Varient";
			grd.RootTable.Columns["FrieghtAmountHeader"].Caption = "Carriage";
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grd.RootTable.Columns["IsFOC"], (ConditionOperator)0, (object)true);
			fc.FormatStyle.ForeColor = Color.Red;
			grd.RootTable.FormatConditions.Add(fc);
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].FormatString = "##,#.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["FrieghtAmountHeader"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FrieghtAmountHeader"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FrieghtAmountHeader"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["FrieghtAmountHeader"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Print");
			grd.RootTable.Columns["Print"].Caption = "Print";
			grd.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Print"].ButtonText = "Print";
			grd.RootTable.Columns["Print"].Width = 60;
			grd.RootTable.Columns["Print"].Position = 0;
			grd.RootTable.Columns["NoOfAttachments"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
	}

	private void FillMainGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = Convert.ToDateTime(FromDate.Value);
			obj.ToDate = Convert.ToDateTime(Todate.Value);
			obj.DocumentTypeId = 1855;
			obj.FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			if (((Control)(object)cmbApproveStatus).Text == "Approve")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)cmbApproveStatus).Text == "UnApproved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)cmbApproveStatus).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			if (RadFOC.Checked)
			{
				obj.ActionId = 1;
			}
			if (RadWithCost.Checked)
			{
				obj.ActionId = 2;
			}
			dtGrid = InvGdn.GdnRegister(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo", typeof(string));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DeliveryOrderId", typeof(int));
				dt.Columns.Add("DeliveryOrderNo", typeof(string));
				dt.Columns.Add("DeliveryOrderDate", typeof(DateTime));
				dt.Columns.Add("OrderId", typeof(int));
				dt.Columns.Add("SaleOrderNo", typeof(string));
				dt.Columns.Add("SaleOrderDate", typeof(DateTime));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("ReferenceNo", typeof(string));
				dt.Columns.Add("ReferencPartyName", typeof(string));
				dt.Columns.Add("ReferencPartyAddress", typeof(string));
				dt.Columns.Add("ReferencPartyCellNo", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("GpNo", typeof(string));
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("DriverName", typeof(string));
				dt.Columns.Add("DriverCellNo", typeof(string));
				dt.Columns.Add("DriverCNICNo", typeof(string));
				dt.Columns.Add("VisitedByName", typeof(string));
				dt.Columns.Add("RefSalesMan", typeof(string));
				dt.Columns.Add("OrderExpiryDate", typeof(DateTime));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("TransporterName", typeof(string));
				dt.Columns.Add("FrieghtAmountHeader", typeof(decimal));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("IsApproved", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUserName", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUserName", typeof(string));
				dt.Columns.Add("BuildingStorey", typeof(string));
				dt.Columns.Add("BuildingHeight", typeof(string));
				dt.Columns.Add("BuildingArea", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				foreach (DataRow row in dtGrid.Rows)
				{
					if (!dt.AsEnumerable().Any((DataRow r) => r.Field<int>("Id") == Conversion.ToInt(row["Id"])))
					{
						dt.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["DeliveryOrderId"], row["DeliveryOrderNo"], row["DeliveryOrderDate"], row["OrderId"], row["SaleOrderNo"], row["SaleOrderDate"], row["SupplierCustomerId"], row["CustomerName"], row["ReferenceNo"], row["ReferencPartyName"], row["ReferencPartyAddress"], row["ReferencPartyCellNo"], row["VehicleNo"], row["GpNo"], row["GpDate"], row["DriverName"], row["DriverCellNo"], row["DriverCNICNo"], row["VisitedByName"], row["RefSalesMan"], row["OrderExpiryDate"], row["DeliveryTerm"], row["TransporterName"], row["FrieghtAmountHeader"], row["RemarksHeader"], row["IsApproved"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["ApprovedDate"], row["ApprovedUserName"], Conversion.ToString(row["BuildingStorey"]), Conversion.ToString(row["BuildingHeight"]), Conversion.ToString(row["BuildingArea"]), row["NoOfAttachments"]);
					}
				}
				grdMain.DataSource = dt;
				grdMain.RetrieveStructure();
				grdSettings();
			}
			else
			{
				grdMain.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grdMain.RootTable.Columns["Id"].Visible = false;
			grdMain.RootTable.Columns["DeliveryOrderId"].Visible = false;
			grdMain.RootTable.Columns["OrderId"].Visible = false;
			grdMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdMain.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdMain.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdMain.RootTable.Columns["TransporterName"].Caption = "CarriageAccount";
			grdMain.RootTable.Columns["DeliveryOrderNo"].Caption = "DoNo";
			grdMain.RootTable.Columns["DeliveryOrderDate"].Caption = "DoDate";
			grdMain.RootTable.Columns["SaleOrderNo"].Caption = "SoNo";
			grdMain.RootTable.Columns["SaleOrderDate"].Caption = "SoDate";
			grdMain.RootTable.Columns["FrieghtAmountHeader"].Caption = "Carriage";
			grdMain.RootTable.Columns["NoOfAttachments"].Caption = "Attached";
			grdMain.RootTable.Columns["FrieghtAmountHeader"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["FrieghtAmountHeader"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["FrieghtAmountHeader"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdMain.RootTable.Columns["FrieghtAmountHeader"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdMain);
			grdMain.RootTable.Columns.Add("Print");
			grdMain.RootTable.Columns["Print"].Caption = "Print";
			grdMain.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdMain.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdMain.RootTable.Columns["Print"].ButtonText = "Print";
			grdMain.RootTable.Columns["Print"].Width = 60;
			grdMain.RootTable.Columns["Print"].Position = 0;
			grdMain.RootTable.Columns["NoOfAttachments"].Position = 1;
			grdMain.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMain_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1853);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMain_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Print")
			{
				int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				GenerateGdnSlip(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMain_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			DetailGridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridBind()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdMain.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (item.Cells.Count > 0)
				{
					DataTable dtOrder = new DataTable();
					dtOrder.Columns.Add("Id", typeof(int));
					dtOrder.Columns.Add("DetailId", typeof(int));
					dtOrder.Columns.Add("DeliveryOrderId", typeof(int));
					dtOrder.Columns.Add("DoOrderDetailId", typeof(int));
					dtOrder.Columns.Add("OrderId", typeof(int));
					dtOrder.Columns.Add("OrderDetailId", typeof(int));
					dtOrder.Columns.Add("ItemId", typeof(int));
					dtOrder.Columns.Add("SaleGLAC", typeof(int));
					dtOrder.Columns.Add("WareHouseName", typeof(string));
					dtOrder.Columns.Add("ItemCode", typeof(string));
					dtOrder.Columns.Add("ItemName", typeof(string));
					dtOrder.Columns.Add("ItemArrtibuteVarient", typeof(string));
					dtOrder.Columns.Add("VarientEquivalent", typeof(string));
					dtOrder.Columns.Add("JobLotDescription", typeof(string));
					dtOrder.Columns.Add("ItemQty", typeof(int));
					dtOrder.Columns.Add("ItemWeight", typeof(decimal));
					dtOrder.Columns.Add("NetWeight", typeof(decimal));
					dtOrder.Columns.Add("CityName", typeof(string));
					dtOrder.Columns.Add("IsFOC", typeof(bool));
					dtOrder.Columns.Add("RemarksDetail", typeof(string));
					dtOrder.Rows.Clear();
					DataTable resultRows = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
						select row).CopyToDataTable();
					for (int i = 0; i < resultRows.Rows.Count; i++)
					{
						dtOrder.Rows.Add(resultRows.Rows[i]["Id"], resultRows.Rows[i]["DetailId"], resultRows.Rows[i]["DeliveryOrderId"], resultRows.Rows[i]["DoOrderDetailId"], resultRows.Rows[i]["OrderId"], resultRows.Rows[i]["OrderDetailId"], resultRows.Rows[i]["ItemId"], resultRows.Rows[i]["SaleGLAC"], resultRows.Rows[i]["WareHouseName"], resultRows.Rows[i]["ItemCode"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["ItemArrtibuteVarient"], resultRows.Rows[i]["VarientEquivalent"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["ItemQty"], resultRows.Rows[i]["ItemWeight"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["CityName"], resultRows.Rows[i]["IsFOC"], resultRows.Rows[i]["RemarksDetail"]);
					}
					grdDetail.DataSource = dtOrder;
					grdDetail.RetrieveStructure();
					GridDetailSetting();
				}
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["DetailId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["SaleGLAC"].Visible = false;
			grdDetail.RootTable.Columns["DeliveryOrderId"].Visible = false;
			grdDetail.RootTable.Columns["DoOrderDetailId"].Visible = false;
			grdDetail.RootTable.Columns["OrderDetailId"].Visible = false;
			grdDetail.RootTable.Columns["OrderId"].Visible = false;
			grdDetail.RootTable.Columns["JobLotDescription"].Caption = "JobLot";
			grdDetail.RootTable.Columns["ItemArrtibuteVarient"].Caption = "Varient";
			grdDetail.RootTable.Columns["VarientEquivalent"].Caption = "VarientUnit";
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grdDetail.RootTable.Columns["IsFOC"], (ConditionOperator)0, (object)true);
			fc.FormatStyle.ForeColor = Color.Red;
			grdDetail.RootTable.FormatConditions.Add(fc);
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["VarientEquivalent"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["VarientEquivalent"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["VarientEquivalent"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["VarientEquivalent"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemWeight"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["ItemWeight"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetWeight"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.##";
			CommonServices.GridAutoAdjustmentNew(grdDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdMain).Name);
			if (fs != null)
			{
				grdMain.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOrderNoFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOrderNoTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
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
			dt.Rows.Add("Ctrl+L", "To Press Load Button");
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

	private void LoadPurchaseOrder_KeyDown(object sender, KeyEventArgs e)
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
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btngrnlod_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				btnReset_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.P)
			{
				BtnRegister_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDate.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdMain).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void printRegister_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid.Rows.Count == 0 || dtGrid == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGrid, "1855-GDNRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRegister_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid.Rows.Count == 0 || dtGrid == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGrid, "1855A-GDNRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GenerateGdnSlip(int PrintId)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvGdn.GdnConcrete_Slip(UserAccount.OrganizationId, UserAccount.CompanyId, PrintId);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvGdn.GdnWagesDetail_SubReport(PrintId), "InvGdn_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1855-InvGdn_Slip.rpt", lstSubReports);
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
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Expected O, but got Unknown
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_3afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b08: Expected O, but got Unknown
		//IL_3b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b36: Expected O, but got Unknown
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdMain_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRegister = new System.Windows.Forms.ToolStripButton();
		this.printRegister = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label23 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbperemeter = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbApproveStatus = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.cmbItemtype = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbCategory = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbParentCategory = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtOrderNoTo = new System.Windows.Forms.TextBox();
		this.CmbSupplier = new UltraCombo();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.txtOrderNoFrom = new System.Windows.Forms.TextBox();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdMain = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.RadWithCost = new System.Windows.Forms.RadioButton();
		this.RadFOC = new System.Windows.Forms.RadioButton();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdMain).BeginInit();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1070, 50);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.toolStripSeparator6, this.btnNew, this.BtnRegister, this.printRegister, this.btnRefresh, this.toolStripSeparator2, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1070, 25);
		this.toolStrip1.TabIndex = 27;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnReset_Click);
		this.BtnRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRegister.Name = "BtnRegister";
		this.BtnRegister.Size = new System.Drawing.Size(128, 22);
		this.BtnRegister.Text = "1855A-Register";
		this.BtnRegister.Click += new System.EventHandler(BtnRegister_Click);
		this.printRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.printRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.printRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.printRegister.Name = "printRegister";
		this.printRegister.Size = new System.Drawing.Size(119, 22);
		this.printRegister.Text = "1855-Register";
		this.printRegister.Click += new System.EventHandler(printRegister_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(122, 22);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 27);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(119, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "GDN Register";
		this.groupBox1.Controls.Add(this.RadWithCost);
		this.groupBox1.Controls.Add(this.RadFOC);
		this.groupBox1.Controls.Add(this.RadAll);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApproveStatus);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtype);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCategory);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentCategory);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtOrderNoTo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.btngrnlod);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.FromDate);
		this.groupBox1.Controls.Add(this.txtOrderNoFrom);
		this.groupBox1.Controls.Add(this.Todate);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, -1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1061, 105);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(69, 16);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(136, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(5, 22);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(60, 15);
		this.label15.TabIndex = 592;
		this.label15.Text = "Date Type";
		this.cmbApproveStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbApproveStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApproveStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApproveStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance160;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbApproveStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Location = new System.Drawing.Point(816, 44);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Name = "cmbApproveStatus";
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Size = new System.Drawing.Size(242, 26);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).TabIndex = 10;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.label13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label13.Location = new System.Drawing.Point(747, 50);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(64, 15);
		this.label13.TabIndex = 142;
		this.label13.Text = "        Status";
		this.cmbItemtype.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance195;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance200).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Location = new System.Drawing.Point(547, 44);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Name = "cmbItemtype";
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).TabIndex = 7;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(463, 50);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(60, 15);
		this.label5.TabIndex = 124;
		this.label5.Text = "Item Type";
		this.cmbCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance207;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Location = new System.Drawing.Point(547, 16);
		((System.Windows.Forms.Control)(object)this.cmbCategory).Name = "cmbCategory";
		((System.Windows.Forms.Control)(object)this.cmbCategory).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbCategory).TabIndex = 6;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(463, 22);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(82, 15);
		this.label8.TabIndex = 123;
		this.label8.Text = "Item Category";
		this.cmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance147;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance152).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Location = new System.Drawing.Point(302, 73);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Name = "cmbParentCategory";
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Size = new System.Drawing.Size(155, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).TabIndex = 5;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(209, 79);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(91, 15);
		this.label6.TabIndex = 122;
		this.label6.Text = "Parent Category";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(463, 79);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(32, 15);
		this.label9.TabIndex = 116;
		this.label9.Text = "Item";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance183;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance188).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(547, 73);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 8;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 50);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 79);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 15);
		this.label3.TabIndex = 12;
		this.label3.Text = "To Date";
		this.txtOrderNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoTo.Location = new System.Drawing.Point(302, 46);
		this.txtOrderNoTo.Name = "txtOrderNoTo";
		this.txtOrderNoTo.ShortcutsEnabled = false;
		this.txtOrderNoTo.Size = new System.Drawing.Size(155, 23);
		this.txtOrderNoTo.TabIndex = 4;
		this.txtOrderNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoTo_KeyPress);
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(816, 16);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(242, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 9;
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btngrnlod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(997, 73);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(61, 26);
		this.btngrnlod.TabIndex = 11;
		this.btngrnlod.Text = "Show";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(747, 22);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Customer";
		this.FromDate.CustomFormat = "dd-MM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(69, 46);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(136, 23);
		this.FromDate.TabIndex = 1;
		this.txtOrderNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoFrom.Location = new System.Drawing.Point(302, 18);
		this.txtOrderNoFrom.Name = "txtOrderNoFrom";
		this.txtOrderNoFrom.ShortcutsEnabled = false;
		this.txtOrderNoFrom.Size = new System.Drawing.Size(155, 23);
		this.txtOrderNoFrom.TabIndex = 3;
		this.txtOrderNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoFrom_KeyPress);
		this.Todate.CustomFormat = "dd-MM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(69, 75);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(136, 23);
		this.Todate.TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(209, 22);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 15);
		this.label7.TabIndex = 28;
		this.label7.Text = "Gdn No From";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(209, 50);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(64, 15);
		this.label4.TabIndex = 28;
		this.label4.Text = "Gdn No To";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1056, 26);
		this.panel3.TabIndex = 31;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1137, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1084, 664);
		this.tabControl1.TabIndex = 34;
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1076, 635);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail Register";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(3, 159);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1070, 473);
		this.tabControl2.TabIndex = 2;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1062, 444);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail Data";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1056, 412);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1056, 26);
		this.panel5.TabIndex = 33;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1008, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar2.TabIndex = 10;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage4.Controls.Add(this.panel4);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdMain);
		this.tabPage4.Controls.Add(this.panel3);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1062, 444);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Main Detail Data";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 294);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1056, 147);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 35;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 271);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1056, 23);
		this.panel4.TabIndex = 34;
		this.grdMain.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdMain).BackColor = System.Drawing.Color.White;
		grdMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMain.DesignTimeLayout = grdMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMain).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdMain.FilterMode = (FilterMode)2;
		this.grdMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMain.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdMain).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.grdMain).Name = "grdMain";
		((ScrollableBase)this.grdMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMain).Size = new System.Drawing.Size(1056, 242);
		((System.Windows.Forms.Control)(object)this.grdMain).TabIndex = 32;
		this.grdMain.TotalRow = (InheritableBoolean)1;
		this.grdMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMain).VisualStyle = (VisualStyle)4;
		this.grdMain.ColumnButtonClick += new ColumnActionEventHandler(grdMain_ColumnButtonClick);
		this.grdMain.SelectionChanged += new System.EventHandler(grdMain_SelectionChanged);
		this.grdMain.LinkClicked += new ColumnActionEventHandler(grdMain_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 53);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1070, 106);
		this.panel2.TabIndex = 1;
		this.RadWithCost.AutoSize = true;
		this.RadWithCost.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.RadWithCost.Location = new System.Drawing.Point(913, 76);
		this.RadWithCost.Name = "RadWithCost";
		this.RadWithCost.Size = new System.Drawing.Size(78, 21);
		this.RadWithCost.TabIndex = 598;
		this.RadWithCost.Text = "Not FOC";
		this.RadWithCost.UseVisualStyleBackColor = true;
		this.RadFOC.AutoSize = true;
		this.RadFOC.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.RadFOC.Location = new System.Drawing.Point(856, 76);
		this.RadFOC.Name = "RadFOC";
		this.RadFOC.Size = new System.Drawing.Size(51, 21);
		this.RadFOC.TabIndex = 597;
		this.RadFOC.Text = "FOC";
		this.RadFOC.UseVisualStyleBackColor = true;
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(809, 76);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(41, 21);
		this.RadAll.TabIndex = 596;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1084, 664);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "GdnRegister";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadInvoices_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadPurchaseOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdMain).EndInit();
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
