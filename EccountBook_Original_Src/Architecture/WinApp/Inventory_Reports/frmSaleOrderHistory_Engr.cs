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
using Architecture.Reports;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Inventory_Reports;

public class frmSaleOrderHistory_Engr : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable grdlst = new DataTable();

	private DataTable dtSalesRegister = new DataTable();

	private DataTable dtSalesSummaryByItem = new DataTable();

	private DataTable dtSalesSummaryByItemandCity = new DataTable();

	private DataTable dtSalesSummaryByItemandPackSize = new DataTable();

	private DataTable dtSalesSummaryByItemNPackSizeCity = new DataTable();

	private DataTable dtSalesSummaryByCustomer = new DataTable();

	private DataTable dtSalesSummaryByCustomerandItem = new DataTable();

	private DataTable dtSalesSummaryByCustomerandOrderNo = new DataTable();

	private DataTable dtSalesSummaryByCustomerandCity = new DataTable();

	private DataTable dtSalesSummaryByCustomerandPackSize = new DataTable();

	private DataTable dtSalesSummaryByCustomerItemandCity = new DataTable();

	private DataTable dtGrid = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	public bool _AutoAdjust;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton print;

	private Label label23;

	internal UltraCombo CmbSupplier;

	private Label label1;

	internal UltraCombo CmbItem;

	private Label label2;

	private Label label33;

	private Label label4;

	private Label label3;

	private TextBox txtSrFrom;

	private TextBox txtsrTo;

	private Label label5;

	internal GridEX grdsaleorder;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar3;

	private Button btnshow;

	internal UltraCombo CmbParentCategory;

	private Label label7;

	internal UltraCombo CmbItemType;

	private Label label9;

	internal UltraCombo CmbItemCategory;

	private Label label8;

	private Label label10;

	private Label label11;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel5;

	private Label label6;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label12;

	internal GridEX GrdDetail;

	private CtrlGrdBar ctrlGrdBar4;

	private Label label13;

	internal GridEX GrdMain;

	private CheckBox chkincludeDo;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private Panel panel6;

	private TabPage tabSummary;

	private Panel panel9;

	internal GridEX DataGridHistory;

	private Label label26;

	private Panel panel7;

	internal UltraCombo cmbItemtypeSumm;

	private Label label16;

	internal UltraCombo CmbCategorySumm;

	private Label label17;

	internal UltraCombo cmbParentcategorySumm;

	private Label label18;

	private Button BtnShowSumm;

	private TextBox txtToDocNoSumm;

	private TextBox txtFromDocNoSumm;

	private DateTimePicker txtToDateSumm;

	private Label label21;

	private DateTimePicker txtFromDateSumm;

	private Label label22;

	internal UltraCombo cmbItemNameSumm;

	private Label label24;

	internal UltraCombo cmbCustomerSumm;

	private Label label25;

	private Panel panel8;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefreshSummary;

	private ToolStripButton btnPrintSumm;

	internal UltraCombo cmbcitySumm;

	private Label label30;

	internal UltraCombo cmbActivitySumm;

	private Label label14;

	internal UltraCombo cmbJobLotSumm;

	private Label label32;

	private CtrlGrdBar ctrlGrdBar5;

	private CheckBox ChkDoApprovedIncludSummary;

	public DateTimePicker fromdate;

	public DateTimePicker ToDate;

	public UltraCombo CmbIsApproved;

	public UltraCombo CmbOrderStatus;

	private CtrlGrdBar ctrlGrdBar6;

	private CtrlGrdBar ctrlGrdBar7;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private CheckBox chkSkipZero;

	private CheckBox chkSkipZeroDetail;

	private ToolStripButton btnRefreshDetail;

	private ToolStripButton btnNewSumm;

	public frmSaleOrderHistory_Engr(UserAccount user)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmSaleOrderHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 800);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			dtSalesRegister.Columns.Add("Id", typeof(int));
			dtSalesRegister.Columns.Add("DocumentTypeId", typeof(int));
			dtSalesRegister.Columns.Add("DocDate", typeof(DateTime));
			dtSalesRegister.Columns.Add("DocNo", typeof(int));
			dtSalesRegister.Columns.Add("PartyName", typeof(string));
			dtSalesRegister.Columns.Add("ItemCode", typeof(string));
			dtSalesRegister.Columns.Add("ItemName", typeof(string));
			dtSalesRegister.Columns.Add("PackUom", typeof(string));
			dtSalesRegister.Columns.Add("JobLot", typeof(string));
			dtSalesRegister.Columns.Add("OrderQty", typeof(double));
			dtSalesRegister.Columns.Add("DispatchedQty", typeof(double));
			dtSalesRegister.Columns.Add("BalQty", typeof(double));
			dtSalesRegister.Columns.Add("OrderAmount", typeof(double));
			dtSalesRegister.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesRegister.Columns.Add("BalAmount", typeof(double));
			dtSalesRegister.Columns.Add("AvgRate", typeof(double));
			dtSalesRegister.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesRegister.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByItem.Columns.Add("Id");
			dtSalesSummaryByItem.Columns.Add("ItemCode");
			dtSalesSummaryByItem.Columns.Add("ItemName");
			dtSalesSummaryByItem.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByItem.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByItem.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByItem.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByItem.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByItem.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByItem.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItem.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("Id");
			dtSalesSummaryByItemandCity.Columns.Add("ItemCode");
			dtSalesSummaryByItemandCity.Columns.Add("ItemName");
			dtSalesSummaryByItemandCity.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("CityName");
			dtSalesSummaryByItemandPackSize.Columns.Add("Id");
			dtSalesSummaryByItemandPackSize.Columns.Add("ItemCode");
			dtSalesSummaryByItemandPackSize.Columns.Add("ItemName");
			dtSalesSummaryByItemandPackSize.Columns.Add("PackUom");
			dtSalesSummaryByItemandPackSize.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("Id");
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("ItemCode");
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("ItemName");
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("PackUom");
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("CityName");
			dtSalesSummaryByCustomer.Columns.Add("Id");
			dtSalesSummaryByCustomer.Columns.Add("PartyName");
			dtSalesSummaryByCustomer.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByCustomerandOrderNo.Columns.Add("Id", typeof(int));
			dtSalesSummaryByCustomerandOrderNo.Columns.Add("PartyName");
			dtSalesSummaryByCustomerandOrderNo.Columns.Add("DocDate", typeof(DateTime));
			dtSalesSummaryByCustomerandOrderNo.Columns.Add("DocNo", typeof(int));
			dtSalesSummaryByCustomerandOrderNo.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByCustomerandOrderNo.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByCustomerandOrderNo.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByCustomerandOrderNo.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByCustomerandOrderNo.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByCustomerandOrderNo.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByCustomerandOrderNo.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandOrderNo.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("Id");
			dtSalesSummaryByCustomerandItem.Columns.Add("PartyName");
			dtSalesSummaryByCustomerandItem.Columns.Add("ItemCode");
			dtSalesSummaryByCustomerandItem.Columns.Add("ItemName");
			dtSalesSummaryByCustomerandItem.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("Id");
			dtSalesSummaryByCustomerandCity.Columns.Add("PartyName");
			dtSalesSummaryByCustomerandCity.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("CityName");
			dtSalesSummaryByCustomerItemandCity.Columns.Add("Id");
			dtSalesSummaryByCustomerItemandCity.Columns.Add("PartyName");
			dtSalesSummaryByCustomerItemandCity.Columns.Add("ItemCode");
			dtSalesSummaryByCustomerItemandCity.Columns.Add("ItemName");
			dtSalesSummaryByCustomerItemandCity.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("CityName");
			dtSalesSummaryByCustomerandPackSize.Columns.Add("Id");
			dtSalesSummaryByCustomerandPackSize.Columns.Add("PartyName");
			dtSalesSummaryByCustomerandPackSize.Columns.Add("PackUom");
			dtSalesSummaryByCustomerandPackSize.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("PercentOfTotal", typeof(double));
			AllDropDownBind();
			StatusFill();
			OrderStatusFill();
			ActivityFill();
			fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			txtFromDateSumm.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			PrintButtonManage();
			tabControl1.SelectedIndex = 1;
			if (((Control)(object)CmbOrderStatus).Text == string.Empty)
			{
				((UltraGridBase)CmbOrderStatus).Rows[0].Activate();
			}
			if (((Control)(object)CmbIsApproved).Text == string.Empty)
			{
				((UltraGridBase)CmbIsApproved).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllDropDownBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			DataTable dtAll = SaleOrder.GetDataForDropDownFromSaleOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID
			});
			if (dtAll.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Customer"
					select r;
				DataTable dtParty = (rows.Any() ? rows.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsitem = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Item"
					select r;
				DataTable dtItem = (rowsitem.Any() ? rowsitem.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsJob = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "JobLot"
					select r;
				DataTable dtJob = (rowsJob.Any() ? rowsJob.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsCity = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "City"
					select r;
				DataTable dtCity = (rowsCity.Any() ? rowsCity.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsPc = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "ParentCategory"
					select r;
				DataTable dtParentCat = (rowsPc.Any() ? rowsPc.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsIc = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "ItemCategory"
					select r;
				DataTable dtCategory = (rowsIc.Any() ? rowsIc.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsItmType = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "ItemType"
					select r;
				DataTable dtItemType = (rowsItmType.Any() ? rowsItmType.CopyToDataTable() : dtAll.Clone());
				DDL.BindDDL(dtParty, CmbSupplier, "Id", "ReferenceName", "Customer Name", ZeroIndex: false);
				((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				DDL.BindDDL(dtParty, cmbCustomerSumm, "Id", "ReferenceName", "Customer Name", ZeroIndex: false);
				((UltraGridBase)cmbCustomerSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				DDL.BindDDL(dtItem, CmbItem, "Id", "ReferenceName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				DDL.BindDDL(dtItem, cmbItemNameSumm, "Id", "ReferenceName", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItemNameSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				DDL.BindDDL(dtJob, cmbJobLotSumm, "Id", "ReferenceName", "JobLot", ZeroIndex: false);
				((UltraGridBase)cmbJobLotSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbJobLotSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtCity, cmbcitySumm, "Id", "ReferenceName", "CityName", ZeroIndex: false);
				((UltraGridBase)cmbcitySumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbcitySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtParentCat, cmbParentcategorySumm, "Id", "ReferenceName", "ParentCategory", ZeroIndex: false);
				((UltraGridBase)cmbParentcategorySumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbParentcategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtParentCat, CmbParentCategory, "Id", "ReferenceName", "ParentCategory", ZeroIndex: false);
				((UltraGridBase)CmbParentCategory).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtCategory, CmbItemCategory, "Id", "ReferenceName", "ItemCategory", ZeroIndex: false);
				((UltraGridBase)CmbItemCategory).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbItemCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtCategory, CmbCategorySumm, "Id", "ReferenceName", "ItemCategory", ZeroIndex: false);
				((UltraGridBase)CmbCategorySumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtItemType, CmbItemType, "Id", "ReferenceName", "ItemType", ZeroIndex: false);
				((UltraGridBase)CmbItemType).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbItemType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtItemType, cmbItemtypeSumm, "Id", "ReferenceName", "ItemType", ZeroIndex: false);
				((UltraGridBase)cmbItemtypeSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbItemtypeSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetDetailRegister()
	{
		try
		{
			chkincludeDo.Checked = false;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbParentCategory).Text = string.Empty;
			((Control)(object)CmbItemCategory).Text = string.Empty;
			((Control)(object)CmbItemType).Text = string.Empty;
			txtSrFrom.Clear();
			txtsrTo.Clear();
			((UltraGridBase)CmbIsApproved).Rows[0].Activate();
			((UltraGridBase)CmbOrderStatus).Rows[0].Activate();
			fromdate.Focus();
			gridHisory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		resetDetailRegister();
	}

	private void frmSaleOrderHistory_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnRefreshDetail_Click(object sender, EventArgs e)
	{
		try
		{
			AllDropDownBind();
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
			dt.Rows.Add(1, "UnApporve");
			dt.Rows.Add(2, "Approve");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, CmbIsApproved, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbIsApproved).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderStatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Cancel");
			dt.Rows.Add(3, "Complete");
			DDL.BindDDL(dt, CmbOrderStatus, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbOrderStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdsaleorder.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdsaleorder.CurrentColumn != null)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				double DispatchQty = Conversion.ToDouble(r.Cells["DispatchQty"].Value);
				DateTime date = Conversion.ToDateTime(r.Cells["ExpiryDate"].Value);
				if (grdsaleorder.CurrentColumn.Key == "IsApproved")
				{
					ApproveRecord(Id);
				}
				else if (grdsaleorder.CurrentColumn.Key == "Complete")
				{
					CompleteStatus(Id);
				}
				else if (grdsaleorder.CurrentColumn.Key == "Cancel")
				{
					CancelStatus(Id, DispatchQty);
				}
				else if (grdsaleorder.CurrentColumn.Key == "Update")
				{
					UpdateExpiryDate(Id, date);
				}
				else if (grdsaleorder.CurrentColumn.Key == "CustomerName")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(r.Cells["OrderSupCustId"].Value)), fromdate.Value, ToDate.Value);
				}
				else if (grdsaleorder.CurrentColumn.Key == "DocNo")
				{
					CommonServices.SaleOrderSlipEngr_1605(Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdsaleorder).Name);
			if (fs != null)
			{
				grdsaleorder.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdMain).Name);
			if (fs != null)
			{
				GrdMain.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdDetail).Name);
			if (fs != null)
			{
				GrdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdlst.Rows.Count == 0 || grdlst == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(grdlst, "299-SalesOrderRegistery.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void btnPrint271A_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdlst.Rows.Count == 0 || grdlst == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(grdlst, "271A-InvRptSalesOrderRegister.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void frmSaleOrderHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (tabControl2.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.F5)
				{
					fromdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					print_Click(null, null);
				}
				if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
				{
					Close();
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click_1(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshDetail_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					if (tabControl1.SelectedIndex == 0)
					{
						((Control)(object)grdsaleorder).Focus();
					}
					else if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGrid")
					{
						((Control)(object)GrdDetail).Focus();
					}
					else
					{
						((Control)(object)GrdMain).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.T)
				{
					tabControl2.SelectedIndex = 1;
					txtFromDateSumm.Focus();
				}
			}
			else if (tabControl2.SelectedIndex == 1)
			{
				if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
				{
					Close();
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtFromDateSumm.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnPrintSummary_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					BtnShowSumm_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewSumm_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshSummary_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.T)
				{
					tabControl2.SelectedIndex = 0;
					fromdate.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveRecord(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "Approve";
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Approve Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CompleteStatus(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Complete Status?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "Status";
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Complete Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CancelStatus(int Id, double dispatchQty)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		try
		{
			if (dispatchQty > 0.0)
			{
				MessageBox.Show("Record Not Cancel because Dispatch Qty gratter than zero");
				resetDetailRegister();
			}
			else if (MessageBox.Show("Are you sure to Cancel Status?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "Cancel";
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Cancel Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UpdateExpiryDate(int Id, DateTime postData)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Update ExpiryDate?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "UpdateExpiryDate";
				item.EntryUser = UserAccount.ID;
				item.PostDate = postData;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Update ExpiryDate Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdMain_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "IsApproved")
			{
				ApproveRecord(Conversion.ToInt(GrdMain.CurrentRow.Cells["Id"].Value));
			}
			else if (e.Column.Key == "Complete")
			{
				if (((Control)(object)CmbIsApproved).Text == "Approve")
				{
					CompleteStatus(Conversion.ToInt((object)GrdMain.CurrentRow.Cells["Id"].Value.ToString()));
				}
			}
			else if (e.Column.Key == "Cancel")
			{
				CancelStatus(Conversion.ToInt((object)GrdMain.CurrentRow.Cells["Id"].Value.ToString()), Conversion.ToDouble(GrdMain.CurrentRow.Cells["DispatchQty"].Value));
			}
			else if (e.Column.Key == "Update")
			{
				UpdateExpiryDate(Conversion.ToInt(GrdMain.CurrentRow.Cells["Id"].Value), Conversion.ToDateTime(GrdMain.CurrentRow.Cells["ExpiryDate"].Value));
			}
			if (e.Column.Key == "ApprovalDetail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (GrdMain.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = Conversion.ToInt(GrdMain.CurrentRow.Cells["DocumentTypeId"].Value);
					obj.RefDocNoId = Conversion.ToInt(GrdMain.CurrentRow.Cells["Id"].Value);
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdMain_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = GrdMain.CurrentRow;
			if (e.Column.Key == "DocNo")
			{
				CommonServices.SaleOrderSlipEngr_1605(Conversion.ToInt(item.Cells["Id"].Value));
			}
			else if (e.Column.Key == "CustomerName")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(item.Cells["SupplierCustomerId"].Value)), fromdate.Value, ToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = GrdMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && GrdMain.CurrentColumn != null)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				double DispatchQty = Conversion.ToDouble(r.Cells["DispatchQty"].Value);
				DateTime date = Conversion.ToDateTime(r.Cells["ExpiryDate"].Value);
				if (GrdMain.CurrentColumn.Key == "IsApproved")
				{
					ApproveRecord(Id);
				}
				else if (GrdMain.CurrentColumn.Key == "Complete")
				{
					CompleteStatus(Id);
				}
				else if (GrdMain.CurrentColumn.Key == "Cancel")
				{
					CancelStatus(Id, DispatchQty);
				}
				else if (GrdMain.CurrentColumn.Key == "Update")
				{
					UpdateExpiryDate(Id, date);
				}
				else if (GrdMain.CurrentColumn.Key == "CustomerName")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(r.Cells["SupplierCustomerId"].Value)), fromdate.Value, ToDate.Value);
				}
				else if (GrdMain.CurrentColumn.Key == "DocNo")
				{
					CommonServices.SaleOrderSlipEngr_1605(Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdMain_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdMain.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dtDetail = new DataTable();
				dtDetail.Columns.Add("ItemId");
				dtDetail.Columns.Add("ItemCode");
				dtDetail.Columns.Add("ItemName");
				dtDetail.Columns.Add("ItemDescription");
				dtDetail.Columns.Add("JobLot");
				dtDetail.Columns.Add("PackUom");
				dtDetail.Columns.Add("ItemQty", typeof(double));
				dtDetail.Columns.Add("DispatchQty", typeof(double));
				dtDetail.Columns.Add("BalQty", typeof(double));
				dtDetail.Columns.Add("ItemRate", typeof(double));
				dtDetail.Columns.Add("ItemAmount", typeof(double));
				dtDetail.Columns.Add("DispatchAmount", typeof(double));
				dtDetail.Columns.Add("BalAmount", typeof(double));
				dtDetail.Columns.Add("TaxType", typeof(string));
				dtDetail.Columns.Add("Tax%", typeof(double));
				dtDetail.Columns.Add("TaxAmount", typeof(double));
				dtDetail.Columns.Add("CityName");
				DataTable resultRows = (from row in grdlst.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dtDetail.Rows.Add(resultRows.Rows[i]["OrderItemId"], resultRows.Rows[i]["ItemCode"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["ItemDiscription"], resultRows.Rows[i]["JobLotCode"], resultRows.Rows[i]["UOMCodeItm"], resultRows.Rows[i]["OrderItemQty"], resultRows.Rows[i]["DispatchQty"], resultRows.Rows[i]["BalQty"], resultRows.Rows[i]["OrderItemRate"], resultRows.Rows[i]["Amount"], resultRows.Rows[i]["DispatchAmount"], resultRows.Rows[i]["BalAmount"], resultRows.Rows[i]["TaxName"], resultRows.Rows[i]["TaxPercent"], resultRows.Rows[i]["TaxAmount"], resultRows.Rows[i]["CityName"]);
				}
				GrdDetail.DataSource = dtDetail;
				GrdDetail.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void MainGridSetting()
	{
		try
		{
			GrdMain.RootTable.Columns["Id"].Visible = false;
			GrdMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			GrdMain.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GrdMain.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GrdMain.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GrdMain.RootTable.Columns["ItemQty"].FormatString = "#,##0.####";
			GrdMain.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.####";
			GrdMain.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			GrdMain.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			GrdMain.RootTable.Columns["DispatchQty"].FormatString = "#,##0.####";
			GrdMain.RootTable.Columns["DispatchQty"].TotalFormatString = "#,##0.####";
			GrdMain.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			GrdMain.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			GrdMain.RootTable.Columns["BalQty"].FormatString = "#,##0.####";
			GrdMain.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.####";
			GrdMain.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			GrdMain.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			GrdMain.RootTable.Columns["OrderAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdMain.RootTable.Columns["OrderAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdMain.RootTable.Columns["DispatchAmount"].AggregateFunction = (AggregateFunction)2;
			GrdMain.RootTable.Columns["DispatchAmount"].TextAlignment = (TextAlignment)3;
			GrdMain.RootTable.Columns["DispatchAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdMain.RootTable.Columns["DispatchAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdMain.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			GrdMain.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			GrdMain.RootTable.Columns["BalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdMain.RootTable.Columns["BalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdMain.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			GrdMain.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(GrdMain);
			if (((Control)(object)CmbIsApproved).Text == "Approve" && ((Control)(object)CmbOrderStatus).Text == "Open")
			{
				if (formright.CanChangeOrderStatusToComplete)
				{
					GrdMain.RootTable.Columns.Add("Complete");
					GrdMain.RootTable.Columns["Complete"].Caption = "Complete";
					GrdMain.RootTable.Columns["Complete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					GrdMain.RootTable.Columns["Complete"].ButtonStyle = (ButtonStyle)5;
					GrdMain.RootTable.Columns["Complete"].ButtonText = "Complete";
					GrdMain.RootTable.Columns["Complete"].Width = 80;
				}
				if (formright.CanChangeOrderStatusToCancel)
				{
					GrdMain.RootTable.Columns.Add("Cancel");
					GrdMain.RootTable.Columns["Cancel"].Caption = "Cancel";
					GrdMain.RootTable.Columns["Cancel"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					GrdMain.RootTable.Columns["Cancel"].ButtonStyle = (ButtonStyle)5;
					GrdMain.RootTable.Columns["Cancel"].ButtonText = "Cancel";
					GrdMain.RootTable.Columns["Cancel"].Width = 60;
				}
				if (formright.CanChangeOrderExpiryDate)
				{
					GrdMain.AllowEdit = (InheritableBoolean)1;
					GrdMain.RootTable.Columns["ExpiryDate"].EditType = (EditType)2;
					GrdMain.RootTable.Columns["ExpiryDate"].FilterEditType = (FilterEditType)6;
					GrdMain.RootTable.Columns.Add("Update");
					GrdMain.RootTable.Columns["Update"].Caption = "UpdateExpiryDate";
					GrdMain.RootTable.Columns["Update"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					GrdMain.RootTable.Columns["Update"].ButtonStyle = (ButtonStyle)5;
					GrdMain.RootTable.Columns["Update"].ButtonText = "UpdateExpiryDate";
					GrdMain.RootTable.Columns["Update"].Width = 120;
				}
			}
			if (((Control)(object)CmbIsApproved).Text == "UnApporve")
			{
				GrdMain.RootTable.Columns.Add("IsApproved");
				GrdMain.RootTable.Columns["IsApproved"].Caption = "IsApproved";
				GrdMain.RootTable.Columns["IsApproved"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				GrdMain.RootTable.Columns["IsApproved"].ButtonStyle = (ButtonStyle)5;
				GrdMain.RootTable.Columns["IsApproved"].ButtonText = "IsApproved";
				GrdMain.RootTable.Columns["IsApproved"].Width = 60;
				GrdMain.RootTable.Columns["IsApproved"].Visible = false;
			}
			ctrlGrdBar6_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings()
	{
		try
		{
			grdsaleorder.RootTable.Columns["Id"].Visible = false;
			grdsaleorder.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdsaleorder.RootTable.Columns["OrderSupCustId"].Visible = false;
			grdsaleorder.RootTable.Columns["ItemId"].Visible = false;
			grdsaleorder.RootTable.Columns["CustomerName"].Width = 200;
			grdsaleorder.RootTable.Columns["ItemName"].Width = 200;
			grdsaleorder.RootTable.Columns["ItemDescription"].Width = 300;
			grdsaleorder.RootTable.Columns["DetailRemarks"].Width = 300;
			grdsaleorder.RootTable.GroupTotals = (GroupTotals)2;
			grdsaleorder.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			grdsaleorder.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			grdsaleorder.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["DispatchAmount"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["DispatchAmount"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["DispatchAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["DispatchAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["BalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["BalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdsaleorder.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdsaleorder.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["DispatchQty"].FormatString = "#,##0.###";
			grdsaleorder.RootTable.Columns["DispatchQty"].TotalFormatString = "#,##0.###";
			grdsaleorder.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.###";
			grdsaleorder.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(grdsaleorder);
			if (((Control)(object)CmbIsApproved).Text == "Approve")
			{
				grdsaleorder.RootTable.Columns.Add("Complete");
				grdsaleorder.RootTable.Columns["Complete"].Caption = "Complete";
				grdsaleorder.RootTable.Columns["Complete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdsaleorder.RootTable.Columns["Complete"].ButtonStyle = (ButtonStyle)5;
				grdsaleorder.RootTable.Columns["Complete"].ButtonText = "Complete";
				grdsaleorder.RootTable.Columns["Complete"].Width = 80;
				grdsaleorder.RootTable.Columns.Add("Cancel");
				grdsaleorder.RootTable.Columns["Cancel"].Caption = "Cancel";
				grdsaleorder.RootTable.Columns["Cancel"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdsaleorder.RootTable.Columns["Cancel"].ButtonStyle = (ButtonStyle)5;
				grdsaleorder.RootTable.Columns["Cancel"].ButtonText = "Cancel";
				grdsaleorder.RootTable.Columns["Cancel"].Width = 60;
			}
			if (((Control)(object)CmbIsApproved).Text == "UnApporve")
			{
				grdsaleorder.RootTable.Columns.Add("IsApproved");
				grdsaleorder.RootTable.Columns["IsApproved"].Caption = "IsApproved";
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonStyle = (ButtonStyle)5;
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonText = "IsApproved";
				grdsaleorder.RootTable.Columns["IsApproved"].Width = 80;
				grdsaleorder.RootTable.Columns["IsApproved"].Visible = false;
			}
			if (((Control)(object)CmbOrderStatus).Text == "Open" && ((Control)(object)CmbIsApproved).Text == "Approve")
			{
				grdsaleorder.AllowEdit = (InheritableBoolean)1;
				grdsaleorder.RootTable.Columns["ExpiryDate"].EditType = (EditType)2;
				grdsaleorder.RootTable.Columns["ExpiryDate"].FilterEditType = (FilterEditType)6;
				grdsaleorder.RootTable.Columns.Add("Update");
				grdsaleorder.RootTable.Columns["Update"].Caption = "UpdateExpiryDate";
				grdsaleorder.RootTable.Columns["Update"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdsaleorder.RootTable.Columns["Update"].ButtonStyle = (ButtonStyle)5;
				grdsaleorder.RootTable.Columns["Update"].ButtonText = "UpdateExpiryDate";
				grdsaleorder.RootTable.Columns["Update"].Width = 120;
			}
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void gridHisory()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			grdlst.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = fromdate.Value;
			obj.ToDate = ToDate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtsrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.InventoryParentCategories = Conversion.ToInt(CmbParentCategory.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbItemCategory.Value);
			obj.ItemTypeId = Conversion.ToInt(CmbItemType.Value);
			obj.Status = Conversion.ToString((object)((Control)(object)CmbOrderStatus).Text);
			if (((Control)(object)CmbIsApproved).Text == "Approve")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)CmbIsApproved).Text == "UnApporve")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)CmbIsApproved).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			if (chkincludeDo.Checked)
			{
				obj.ActionId = 1;
			}
			if (chkSkipZeroDetail.Checked)
			{
				obj.SkipZero = 1;
			}
			grdlst = SaleOrder.SaleOrderDetailRegister_Eng(obj);
			if (grdlst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("OrderSupCustId", typeof(int));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("ReferenceNo", typeof(string));
				dt.Columns.Add("PaymentTerm", typeof(string));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemDescription", typeof(string));
				dt.Columns.Add("PackUom", typeof(string));
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("DispatchQty", typeof(double));
				dt.Columns.Add("BalQty", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("DispatchAmount", typeof(double));
				dt.Columns.Add("BalAmount", typeof(double));
				dt.Columns.Add("CityName", typeof(string));
				dt.Columns.Add("DetailRemarks", typeof(string));
				dt.Columns.Add("DueDays", typeof(int));
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("OrderStatus", typeof(string));
				dt.Columns.Add("ApprovedStatus", typeof(string));
				dt.Columns.Add("ExpiryDate", typeof(DateTime));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUser", typeof(string));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Clear();
				for (int j = 0; j < grdlst.Rows.Count; j++)
				{
					dt.Rows.Add(grdlst.Rows[j]["Id"], grdlst.Rows[j]["DocumentTypeId"], grdlst.Rows[j]["DocNo"], Convert.ToDateTime(grdlst.Rows[j]["DocDate"]).ToString("dd-MMM-yyyy"), grdlst.Rows[j]["OrderSupCustId"], grdlst.Rows[j]["CustomerName"], grdlst.Rows[j]["SupplierRefNo"], grdlst.Rows[j]["TermsDescription"], grdlst.Rows[j]["DeliveryTerm"], grdlst.Rows[j]["OrderItemId"], grdlst.Rows[j]["ItemCode"], grdlst.Rows[j]["ItemName"], grdlst.Rows[j]["ItemDiscription"], grdlst.Rows[j]["UOMCodeItm"], grdlst.Rows[j]["OrderItemQty"], grdlst.Rows[j]["DispatchQty"], grdlst.Rows[j]["BalQty"], grdlst.Rows[j]["OrderItemRate"], grdlst.Rows[j]["Amount"], grdlst.Rows[j]["DispatchAmount"], grdlst.Rows[j]["BalAmount"], grdlst.Rows[j]["CityName"], grdlst.Rows[j]["OrderRemarks"], grdlst.Rows[j]["OrderDueDays"], Convert.ToDateTime(grdlst.Rows[j]["OrderDueDate"]).ToString("dd-MMM-yy"), grdlst.Rows[j]["OrderStatus"], grdlst.Rows[j]["ApprovedStatus"], Convert.ToDateTime(grdlst.Rows[j]["OrderExpiryDate"]).ToString("dd-MMM-yy"), grdlst.Rows[j]["EntryDate"], grdlst.Rows[j]["UserNameEusr"], grdlst.Rows[j]["ModifyDate"], grdlst.Rows[j]["UserNameMusr"], grdlst.Rows[j]["PostDate"], grdlst.Rows[j]["UserNameAusr"], grdlst.Rows[j]["RemarksHeader"]);
				}
				grdsaleorder.DataSource = dt;
				grdsaleorder.RetrieveStructure();
				GridSettings();
				DataTable dtMain = new DataTable();
				dtMain.Columns.Add("Id", typeof(string));
				dtMain.Columns.Add("DocumentTypeId", typeof(int));
				dtMain.Columns.Add("DocNo", typeof(int));
				dtMain.Columns.Add("DocDate", typeof(DateTime));
				dtMain.Columns.Add("SupplierCustomerId", typeof(int));
				dtMain.Columns.Add("CustomerName", typeof(string));
				dtMain.Columns.Add("OrderStatus", typeof(string));
				dtMain.Columns.Add("ApprovedStatus", typeof(string));
				dtMain.Columns.Add("PaymentTerm", typeof(string));
				dtMain.Columns.Add("DueDays", typeof(int));
				dtMain.Columns.Add("DueDate", typeof(DateTime));
				dtMain.Columns.Add("DeliveryTerm", typeof(string));
				dtMain.Columns.Add("ItemQty", typeof(double));
				dtMain.Columns.Add("DispatchQty", typeof(double));
				dtMain.Columns.Add("BalQty", typeof(double));
				dtMain.Columns.Add("OrderAmount", typeof(double));
				dtMain.Columns.Add("DispatchAmount", typeof(double));
				dtMain.Columns.Add("BalAmount", typeof(double));
				dtMain.Columns.Add("ExpiryDate", typeof(DateTime));
				dtMain.Columns.Add("EntryDate", typeof(DateTime));
				dtMain.Columns.Add("EntryUser", typeof(string));
				dtMain.Columns.Add("ModifyDate", typeof(DateTime));
				dtMain.Columns.Add("ModifyUser", typeof(string));
				dtMain.Columns.Add("ApprovedDate", typeof(DateTime));
				dtMain.Columns.Add("ApprovedUser", typeof(string));
				dtMain.Columns.Add("Remarks", typeof(string));
				int i;
				for (i = 0; i < grdlst.Rows.Count; i++)
				{
					bool num = dtMain.AsEnumerable().Any((DataRow row) => row.Field<string>("Id") == Conversion.ToString(grdlst.Rows[i]["Id"]));
					double itemQty = Conversion.ToInt(grdlst.Compute("Sum(OrderItemQty)", "Id ='" + Conversion.ToString(grdlst.Rows[i]["Id"]) + "'"));
					double DispQty = Conversion.ToInt(grdlst.Compute("Sum(DispatchQty)", "Id ='" + Conversion.ToString(grdlst.Rows[i]["Id"]) + "'"));
					double BalQty = Conversion.ToInt(grdlst.Compute("Sum(BalQty)", "Id ='" + Conversion.ToString(grdlst.Rows[i]["Id"]) + "'"));
					double amount = Conversion.ToInt(grdlst.Compute("Sum(Amount)", "Id ='" + Conversion.ToString(grdlst.Rows[i]["Id"]) + "'"));
					double dispAmount = Conversion.ToInt(grdlst.Compute("Sum(DispatchAmount)", "Id ='" + Conversion.ToString(grdlst.Rows[i]["Id"]) + "'"));
					double balAmount = Conversion.ToInt(grdlst.Compute("Sum(BalAmount)", "Id ='" + Conversion.ToString(grdlst.Rows[i]["Id"]) + "'"));
					if (!num)
					{
						dtMain.Rows.Add(grdlst.Rows[i]["Id"], grdlst.Rows[i]["DocumentTypeId"], grdlst.Rows[i]["DocNo"], Convert.ToDateTime(grdlst.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), grdlst.Rows[i]["OrderSupCustId"], grdlst.Rows[i]["CustomerName"], grdlst.Rows[i]["OrderStatus"], grdlst.Rows[i]["ApprovedStatus"], grdlst.Rows[i]["TermsDescription"], grdlst.Rows[i]["OrderDueDays"], Convert.ToDateTime(grdlst.Rows[i]["OrderDueDate"]).ToString("dd-MMM-yy"), grdlst.Rows[i]["DeliveryTerm"], itemQty, DispQty, BalQty, amount, dispAmount, balAmount, Convert.ToDateTime(grdlst.Rows[i]["OrderExpiryDate"]).ToString("dd-MMM-yy"), grdlst.Rows[i]["EntryDate"], grdlst.Rows[i]["UserNameEusr"], grdlst.Rows[i]["ModifyDate"], grdlst.Rows[i]["UserNameMusr"], grdlst.Rows[i]["PostDate"], grdlst.Rows[i]["UserNameAusr"], grdlst.Rows[i]["RemarksHeader"]);
					}
				}
				GrdMain.DataSource = dtMain;
				GrdMain.RetrieveStructure();
				MainGridSetting();
			}
			else
			{
				grdsaleorder.DataSource = null;
				GrdMain.ClearStructure();
				GrdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnshow_Click_1(object sender, EventArgs e)
	{
		gridHisory();
	}

	private void grdsaleorder_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "IsApproved")
			{
				ApproveRecord(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Complete")
			{
				CompleteStatus(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Cancel")
			{
				CancelStatus(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value), Conversion.ToDouble(grdsaleorder.CurrentRow.Cells["DispatchQty"].Value));
			}
			if (e.Column.Key == "Update")
			{
				UpdateExpiryDate(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value), Conversion.ToDateTime(grdsaleorder.CurrentRow.Cells["ExpiryDate"].Value));
			}
			if (e.Column.Key == "ApprovalDetail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (grdsaleorder.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = Conversion.ToInt(grdsaleorder.CurrentRow.Cells["DocumentTypeId"].Value);
					obj.RefDocNoId = Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value);
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdsaleorder.CurrentRow;
			if (e.Column.Key == "DocNo")
			{
				CommonServices.SaleOrderSlipEngr_1605(Conversion.ToInt(item.Cells["Id"].Value));
			}
			else if (e.Column.Key == "CustomerName")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(item.Cells["OrderSupCustId"].Value)), fromdate.Value, ToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			GrdDetail.RootTable.Columns["ItemId"].Visible = false;
			GrdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["DispatchQty"].FormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["DispatchQty"].TotalFormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["BalQty"].FormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetail.RootTable.Columns["DispatchAmount"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["DispatchAmount"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["DispatchAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetail.RootTable.Columns["DispatchAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetail.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["BalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetail.RootTable.Columns["BalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GrdDetail.RootTable.Columns["Tax%"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["Tax%"].FormatString = "#,##0.####";
			GrdDetail.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetail.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(GrdDetail);
			ctrlGrdBar7_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = GrdDetail.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["SaleGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), fromdate.Value, ToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = GrdDetail.CurrentRow;
				if (r != null && (int)r.RowType == 0 && GrdDetail.CurrentColumn != null && GrdDetail.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["SaleGLAC"].Value);
					Conversion.ToInt(r.Cells["ItemId"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtSrFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsrTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ActivityFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Order Register");
			dt.Rows.Add(2, "Order Summary By Item");
			dt.Rows.Add(3, "Order Summary By Item & Pack Size");
			dt.Rows.Add(4, "Order Summary By Item & City");
			dt.Rows.Add(5, "Order Summary By Item,Pack Size & City");
			dt.Rows.Add(6, "Order Summary By Customer");
			dt.Rows.Add(7, "Order Summary By Customer & OrderNo");
			dt.Rows.Add(8, "Order Summary By Customer & Item");
			dt.Rows.Add(9, "Order Summary By Customer & Pack Size");
			dt.Rows.Add(10, "Order Summary By Customer & City");
			dt.Rows.Add(11, "Order Summary By Customer,Item & City");
			DDL.BindDDL(dt, cmbActivitySumm, "Id", "Name", "Activity", ZeroIndex: false);
			((UltraGridBase)cmbActivitySumm).Rows[0].Activate();
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
			AllDropDownBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewSumm_Click(object sender, EventArgs e)
	{
		try
		{
			txtFromDateSumm.Value = DateTime.Now;
			txtToDateSumm.Value = DateTime.Now;
			txtFromDocNoSumm.Clear();
			txtToDocNoSumm.Clear();
			((Control)(object)cmbParentcategorySumm).Text = string.Empty;
			((Control)(object)CmbCategorySumm).Text = string.Empty;
			((Control)(object)cmbItemtypeSumm).Text = string.Empty;
			((Control)(object)cmbCustomerSumm).Text = string.Empty;
			((Control)(object)cmbItemNameSumm).Text = string.Empty;
			((Control)(object)cmbcitySumm).Text = string.Empty;
			((Control)(object)cmbJobLotSumm).Text = string.Empty;
			((UltraGridBase)cmbActivitySumm).Rows[0].Activate();
			txtFromDateSumm.Focus();
			DataGridHistory.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowSumm_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbActivitySumm).ActiveRow != null)
			{
				GridSummaryFill();
				PrintButtonManage();
			}
			else
			{
				MessageBox.Show("Please Select Activity First...");
				cmbActivitySumm.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSummaryFill()
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
			obj.FromDate = Conversion.ToDateTime((object)txtFromDateSumm.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtToDateSumm.Value);
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoSumm.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoSumm.Text);
			obj.ParentCategoryId = Conversion.ToInt(cmbParentcategorySumm.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbCategorySumm.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtypeSumm.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbCustomerSumm.Value);
			obj.ItemId = Conversion.ToInt(cmbItemNameSumm.Value);
			obj.JobLotId = Conversion.ToInt(cmbJobLotSumm.Value);
			obj.CityId = Conversion.ToInt(cmbcitySumm.Value);
			obj.ReportType = ((Control)(object)cmbActivitySumm).Text;
			if (ChkDoApprovedIncludSummary.Checked)
			{
				obj.ActionId = 1;
			}
			if (chkSkipZero.Checked)
			{
				obj.SkipZero = 1;
			}
			obj.IsOnQty = 1;
			dtGrid = SaleOrder.SaleOrderSummaryRegister(obj);
			if (dtGrid.Rows.Count > 0)
			{
				if (((UltraGridBase)cmbActivitySumm).ActiveRow != null)
				{
					if (((Control)(object)cmbActivitySumm).Text == "Order Register")
					{
						((Control)(object)DataGridHistory).Name = "OrderRegister";
						dtSalesRegister.Rows.Clear();
						for (int i = 0; i < dtGrid.Rows.Count; i++)
						{
							dtSalesRegister.Rows.Add(dtGrid.Rows[i]["Id"], dtGrid.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(dtGrid.Rows[i]["DocDate"]).ToShortDateString(), dtGrid.Rows[i]["DocNo"], dtGrid.Rows[i]["Customer"], dtGrid.Rows[i]["ItemCode"], dtGrid.Rows[i]["ItemName"], dtGrid.Rows[i]["UOMCode"], dtGrid.Rows[i]["JobLotCode"], dtGrid.Rows[i]["OrderQty"], dtGrid.Rows[i]["DispatchQty"], dtGrid.Rows[i]["BalQty"], dtGrid.Rows[i]["OrderAmount"], dtGrid.Rows[i]["DispatchAmount"], dtGrid.Rows[i]["BalAmount"], dtGrid.Rows[i]["AvgRate"], dtGrid.Rows[i]["PrcntOfTotal"], dtGrid.Rows[i]["CityName"]);
						}
						DataGridHistory.DataSource = dtSalesRegister;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
						DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
					}
					else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByItem";
						dtSalesSummaryByItem.Rows.Clear();
						for (int j = 0; j < dtGrid.Rows.Count; j++)
						{
							dtSalesSummaryByItem.Rows.Add(dtGrid.Rows[j]["Id"], dtGrid.Rows[j]["ItemCode"], dtGrid.Rows[j]["ItemName"], dtGrid.Rows[j]["OrderQty"], dtGrid.Rows[j]["DispatchQty"], dtGrid.Rows[j]["BalQty"], dtGrid.Rows[j]["OrderAmount"], dtGrid.Rows[j]["DispatchAmount"], dtGrid.Rows[j]["BalAmount"], dtGrid.Rows[j]["AvgRate"], dtGrid.Rows[j]["PrcntOfTotal"]);
						}
						DataGridHistory.DataSource = dtSalesSummaryByItem;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item & Pack Size")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByItemAndPackSize";
						dtSalesSummaryByItemandPackSize.Rows.Clear();
						for (int k = 0; k < dtGrid.Rows.Count; k++)
						{
							dtSalesSummaryByItemandPackSize.Rows.Add(dtGrid.Rows[k]["Id"], dtGrid.Rows[k]["ItemCode"], dtGrid.Rows[k]["ItemName"], dtGrid.Rows[k]["UOMCode"], dtGrid.Rows[k]["OrderQty"], dtGrid.Rows[k]["DispatchQty"], dtGrid.Rows[k]["BalQty"], dtGrid.Rows[k]["OrderAmount"], dtGrid.Rows[k]["DispatchAmount"], dtGrid.Rows[k]["BalAmount"], dtGrid.Rows[k]["AvgRate"], dtGrid.Rows[k]["PrcntOfTotal"]);
						}
						DataGridHistory.DataSource = dtSalesSummaryByItemandPackSize;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item & City")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByItemAndCity";
						dtSalesSummaryByItemandCity.Rows.Clear();
						for (int l = 0; l < dtGrid.Rows.Count; l++)
						{
							dtSalesSummaryByItemandCity.Rows.Add(dtGrid.Rows[l]["Id"], dtGrid.Rows[l]["ItemCode"], dtGrid.Rows[l]["ItemName"], dtGrid.Rows[l]["OrderQty"], dtGrid.Rows[l]["DispatchQty"], dtGrid.Rows[l]["BalQty"], dtGrid.Rows[l]["OrderAmount"], dtGrid.Rows[l]["DispatchAmount"], dtGrid.Rows[l]["BalAmount"], dtGrid.Rows[l]["AvgRate"], dtGrid.Rows[l]["PrcntOfTotal"], dtGrid.Rows[l]["CityName"]);
						}
						DataGridHistory.DataSource = dtSalesSummaryByItemandCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item,Pack Size & City")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByItemPackSizeAndCity";
						dtSalesSummaryByItemNPackSizeCity.Rows.Clear();
						for (int m = 0; m < dtGrid.Rows.Count; m++)
						{
							dtSalesSummaryByItemNPackSizeCity.Rows.Add(dtGrid.Rows[m]["Id"], dtGrid.Rows[m]["ItemCode"], dtGrid.Rows[m]["ItemName"], dtGrid.Rows[m]["UOMCode"], dtGrid.Rows[m]["OrderQty"], dtGrid.Rows[m]["DispatchQty"], dtGrid.Rows[m]["BalQty"], dtGrid.Rows[m]["OrderAmount"], dtGrid.Rows[m]["DispatchAmount"], dtGrid.Rows[m]["BalAmount"], dtGrid.Rows[m]["AvgRate"], dtGrid.Rows[m]["PrcntOfTotal"], dtGrid.Rows[m]["CityName"]);
						}
						DataGridHistory.DataSource = dtSalesSummaryByItemNPackSizeCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomer";
						dtSalesSummaryByCustomer.Rows.Clear();
						for (int n = 0; n < dtGrid.Rows.Count; n++)
						{
							dtSalesSummaryByCustomer.Rows.Add(dtGrid.Rows[n]["Id"], dtGrid.Rows[n]["Customer"], dtGrid.Rows[n]["OrderQty"], dtGrid.Rows[n]["DispatchQty"], dtGrid.Rows[n]["BalQty"], dtGrid.Rows[n]["OrderAmount"], dtGrid.Rows[n]["DispatchAmount"], dtGrid.Rows[n]["BalAmount"], dtGrid.Rows[n]["AvgRate"], dtGrid.Rows[n]["PrcntOfTotal"]);
						}
						DataGridHistory.DataSource = dtSalesSummaryByCustomer;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & OrderNo")
					{
						((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerandOrderNo";
						dtSalesSummaryByCustomerandOrderNo.Rows.Clear();
						for (int i2 = 0; i2 < dtGrid.Rows.Count; i2++)
						{
							dtSalesSummaryByCustomerandOrderNo.Rows.Add(dtGrid.Rows[i2]["Id"], dtGrid.Rows[i2]["Customer"], dtGrid.Rows[i2]["DocDate"], dtGrid.Rows[i2]["DocNo"], dtGrid.Rows[i2]["OrderQty"], dtGrid.Rows[i2]["DispatchQty"], dtGrid.Rows[i2]["BalQty"], dtGrid.Rows[i2]["OrderAmount"], dtGrid.Rows[i2]["DispatchAmount"], dtGrid.Rows[i2]["BalAmount"], dtGrid.Rows[i2]["AvgRate"], dtGrid.Rows[i2]["PrcntOfTotal"]);
						}
						DataGridHistory.DataSource = dtSalesSummaryByCustomerandOrderNo;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & Item")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomerAndItem";
						dtSalesSummaryByCustomerandItem.Rows.Clear();
						for (int i3 = 0; i3 < dtGrid.Rows.Count; i3++)
						{
							dtSalesSummaryByCustomerandItem.Rows.Add(dtGrid.Rows[i3]["Id"], dtGrid.Rows[i3]["Customer"], dtGrid.Rows[i3]["ItemCode"], dtGrid.Rows[i3]["ItemName"], dtGrid.Rows[i3]["OrderQty"], dtGrid.Rows[i3]["DispatchQty"], dtGrid.Rows[i3]["BalQty"], dtGrid.Rows[i3]["OrderAmount"], dtGrid.Rows[i3]["DispatchAmount"], dtGrid.Rows[i3]["BalAmount"], dtGrid.Rows[i3]["AvgRate"], dtGrid.Rows[i3]["PrcntOfTotal"]);
						}
						DataGridHistory.DataSource = dtSalesSummaryByCustomerandItem;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & City")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomerAndCity";
						dtSalesSummaryByCustomerandCity.Rows.Clear();
						for (int i4 = 0; i4 < dtGrid.Rows.Count; i4++)
						{
							dtSalesSummaryByCustomerandCity.Rows.Add(dtGrid.Rows[i4]["Id"], dtGrid.Rows[i4]["Customer"], dtGrid.Rows[i4]["OrderQty"], dtGrid.Rows[i4]["DispatchQty"], dtGrid.Rows[i4]["BalQty"], dtGrid.Rows[i4]["OrderAmount"], dtGrid.Rows[i4]["DispatchAmount"], dtGrid.Rows[i4]["BalAmount"], dtGrid.Rows[i4]["AvgRate"], dtGrid.Rows[i4]["PrcntOfTotal"], dtGrid.Rows[i4]["CityName"]);
						}
						DataGridHistory.DataSource = dtSalesSummaryByCustomerandCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer,Item & City")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomerItemAndCity";
						dtSalesSummaryByCustomerItemandCity.Rows.Clear();
						for (int i5 = 0; i5 < dtGrid.Rows.Count; i5++)
						{
							dtSalesSummaryByCustomerItemandCity.Rows.Add(dtGrid.Rows[i5]["Id"], dtGrid.Rows[i5]["Customer"], dtGrid.Rows[i5]["ItemCode"], dtGrid.Rows[i5]["ItemName"], dtGrid.Rows[i5]["OrderQty"], dtGrid.Rows[i5]["DispatchQty"], dtGrid.Rows[i5]["BalQty"], dtGrid.Rows[i5]["OrderAmount"], dtGrid.Rows[i5]["DispatchAmount"], dtGrid.Rows[i5]["BalAmount"], dtGrid.Rows[i5]["AvgRate"], dtGrid.Rows[i5]["PrcntOfTotal"], dtGrid.Rows[i5]["CityName"]);
						}
						DataGridHistory.DataSource = dtSalesSummaryByCustomerItemandCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & Pack Size")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomerAndPackSize";
						dtSalesSummaryByCustomerandPackSize.Rows.Clear();
						for (int i6 = 0; i6 < dtGrid.Rows.Count; i6++)
						{
							dtSalesSummaryByCustomerandPackSize.Rows.Add(dtGrid.Rows[i6]["Id"], dtGrid.Rows[i6]["Customer"], dtGrid.Rows[i6]["UOMCode"], dtGrid.Rows[i6]["OrderQty"], dtGrid.Rows[i6]["DispatchQty"], dtGrid.Rows[i6]["BalQty"], dtGrid.Rows[i6]["OrderAmount"], dtGrid.Rows[i6]["DispatchAmount"], dtGrid.Rows[i6]["BalAmount"], dtGrid.Rows[i6]["AvgRate"], dtGrid.Rows[i6]["PrcntOfTotal"]);
						}
						DataGridHistory.DataSource = dtSalesSummaryByCustomerandPackSize;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
				}
				else
				{
					DataGridHistory.ClearStructure();
				}
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

	private void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["PercentOfTotal"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["PercentOfTotal"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["PercentOfTotal"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["PercentOfTotal"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OrderQty"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["OrderQty"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["DisPatchedQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["DisPatchedQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["DisPatchedQty"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["DisPatchedQty"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BalQty"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OrderAmount"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["OrderAmount"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["DisPatchedAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["DisPatchedAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["DisPatchedAmount"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["DisPatchedAmount"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BalAmount"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["BalAmount"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AvgRate"].FormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl2.SelectedIndex == 1)
		{
			txtFromDateSumm.Focus();
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPrintSummary_Click(object sender, EventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnPrintSumm.Text == "1605_01_OrderRegister")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtGrid, "1605_01_OrderRegister.rpt");
			}
			else if (btnPrintSumm.Text == "1605_02_OrderSummaryByItem&PackSize")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtGrid, "1605_02_OrderSummaryByItem&PackSize.rpt");
			}
			else if (btnPrintSumm.Text == "1605_03_OrderSummaryByItem,PackSize&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtGrid, "1605_03_OrderSummaryByItem,PackSize&City.rpt");
			}
			else if (btnPrintSumm.Text == "1605_04_OrderSummaryByCustomer&PackSize")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtGrid, "1605_04_OrderSummaryByCustomer&PackSize.rpt");
			}
			else if (btnPrintSumm.Text == "1605_05_OrderSummaryByItem")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtGrid, "1605_05_OrderSummaryByItem.rpt");
			}
			else if (btnPrintSumm.Text == "1605_06_OrderSummaryByItem&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtGrid, "1605_06_OrderSummaryByItem&City.rpt");
			}
			else if (btnPrintSumm.Text == "1605_07_OrderSummaryByCustomer")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtGrid, "1605_07_OrderSummaryByCustomer.rpt");
			}
			else if (btnPrintSumm.Text == "1605_08_OrderSummaryByCustomer&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val8 = new Reporting();
				val8.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val8.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val8.ShowReportWithDataTable(dtGrid, "1605_08_OrderSummaryByCustomer&City.rpt");
			}
			else if (btnPrintSumm.Text == "1605_09_OrderSummaryByCustomer&Item")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val9 = new Reporting();
				val9.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val9.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val9.ShowReportWithDataTable(dtGrid, "1605_09_OrderSummaryByCustomer&Item.rpt");
			}
			else if (btnPrintSumm.Text == "1605_10_OrderSummaryByCustomer,Item&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val10 = new Reporting();
				val10.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val10.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val10.ShowReportWithDataTable(dtGrid, "1605_10_OrderSummaryByCustomer,Item&City.rpt");
			}
			else if (btnPrintSumm.Text == "1605_11_OrderSummaryByCustomer&Order")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val11 = new Reporting();
				val11.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val11.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val11.ShowReportWithDataTable(dtGrid, "1605_11_OrderSummaryByCustomer&Order.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PrintButtonManage()
	{
		try
		{
			if (((UltraGridBase)cmbActivitySumm).ActiveRow != null)
			{
				btnPrintSumm.Visible = true;
				if (((Control)(object)cmbActivitySumm).Text == "Order Register")
				{
					btnPrintSumm.Text = "1605_01_OrderRegister";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item & Pack Size")
				{
					btnPrintSumm.Text = "1605_02_OrderSummaryByItem&PackSize";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item,Pack Size & City")
				{
					btnPrintSumm.Text = "1605_03_OrderSummaryByItem,PackSize&City";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & Pack Size")
				{
					btnPrintSumm.Text = "1605_04_OrderSummaryByCustomer&PackSize";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item")
				{
					btnPrintSumm.Text = "1605_05_OrderSummaryByItem";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item & City")
				{
					btnPrintSumm.Text = "1605_06_OrderSummaryByItem&City";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer")
				{
					btnPrintSumm.Text = "1605_07_OrderSummaryByCustomer";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Sales Summary By Customer & City")
				{
					btnPrintSumm.Text = "1605_08_OrderSummaryByCustomer&City";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & Item")
				{
					btnPrintSumm.Text = "1605_09_OrderSummaryByCustomer&Item";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer,Item & City")
				{
					btnPrintSumm.Text = "1605_10_OrderSummaryByCustomer,Item&City";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & OrderNo")
				{
					btnPrintSumm.Text = "1605_11_OrderSummaryByCustomer&Order";
				}
			}
			else
			{
				btnPrintSumm.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNo_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected O, but got Unknown
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Expected O, but got Unknown
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Expected O, but got Unknown
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Expected O, but got Unknown
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Expected O, but got Unknown
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Expected O, but got Unknown
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Expected O, but got Unknown
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Expected O, but got Unknown
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Expected O, but got Unknown
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Expected O, but got Unknown
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Expected O, but got Unknown
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Expected O, but got Unknown
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_0fbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc5: Expected O, but got Unknown
		//IL_0fd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdc: Expected O, but got Unknown
		//IL_1406: Unknown result type (might be due to invalid IL or missing references)
		//IL_1410: Expected O, but got Unknown
		//IL_15ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f6: Expected O, but got Unknown
		//IL_161a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1624: Expected O, but got Unknown
		//IL_4703: Unknown result type (might be due to invalid IL or missing references)
		//IL_470d: Expected O, but got Unknown
		GridEXLayout grdsaleorder_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdMain_DesignTimeLayout = new GridEXLayout();
		Appearance appearance118 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance191 = new Appearance();
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
		Appearance appearance204 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance125 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdsaleorder = new GridEX();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.GrdDetail = new GridEX();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.GrdMain = new GridEX();
		this.label13 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.chkSkipZeroDetail = new System.Windows.Forms.CheckBox();
		this.CmbItemType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.chkincludeDo = new System.Windows.Forms.CheckBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbItem = new UltraCombo();
		this.CmbOrderStatus = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.CmbItemCategory = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtSrFrom = new System.Windows.Forms.TextBox();
		this.CmbParentCategory = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtsrTo = new System.Windows.Forms.TextBox();
		this.CmbIsApproved = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.tabSummary = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridHistory = new GridEX();
		this.label26 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.chkSkipZero = new System.Windows.Forms.CheckBox();
		this.cmbItemtypeSumm = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.cmbCustomerSumm = new UltraCombo();
		this.ChkDoApprovedIncludSummary = new System.Windows.Forms.CheckBox();
		this.label24 = new System.Windows.Forms.Label();
		this.cmbActivitySumm = new UltraCombo();
		this.cmbItemNameSumm = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.cmbJobLotSumm = new UltraCombo();
		this.txtFromDateSumm = new System.Windows.Forms.DateTimePicker();
		this.label32 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtToDateSumm = new System.Windows.Forms.DateTimePicker();
		this.txtFromDocNoSumm = new System.Windows.Forms.TextBox();
		this.cmbcitySumm = new UltraCombo();
		this.txtToDocNoSumm = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.BtnShowSumm = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.cmbParentcategorySumm = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.CmbCategorySumm = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefreshSummary = new System.Windows.Forms.ToolStripButton();
		this.btnPrintSumm = new System.Windows.Forms.ToolStripButton();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.btnRefreshDetail = new System.Windows.Forms.ToolStripButton();
		this.btnNewSumm = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).BeginInit();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.GrdMain).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApproved).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabSummary.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivitySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1029, 680);
		this.panel1.TabIndex = 0;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabSummary);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 28);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1029, 652);
		this.tabControl2.TabIndex = 5;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabDetail.Controls.Add(this.panel6);
		this.tabDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabDetail.Location = new System.Drawing.Point(4, 22);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetail.Size = new System.Drawing.Size(1021, 626);
		this.tabDetail.TabIndex = 0;
		this.tabDetail.Text = "Detail Register";
		this.tabDetail.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.tabControl1);
		this.panel6.Controls.Add(this.panel4);
		this.panel6.Controls.Add(this.panel2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1015, 620);
		this.panel6.TabIndex = 6;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 168);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1015, 452);
		this.tabControl1.TabIndex = 4;
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.ctrlGrdBar3);
		this.tabPage1.Controls.Add(this.label6);
		this.tabPage1.Location = new System.Drawing.Point(4, 24);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1007, 424);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdsaleorder);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 29);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1001, 392);
		this.panel5.TabIndex = 25;
		this.grdsaleorder.AllowEdit = (InheritableBoolean)2;
		this.grdsaleorder.AlternatingColors = true;
		((ScrollableBase)this.grdsaleorder).BackColor = System.Drawing.Color.White;
		grdsaleorder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdsaleorder.DesignTimeLayout = grdsaleorder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdsaleorder.FilterMode = (FilterMode)2;
		this.grdsaleorder.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdsaleorder.GroupTotals = (GroupTotals)2;
		this.grdsaleorder.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Name = "grdsaleorder";
		((ScrollableBase)this.grdsaleorder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Size = new System.Drawing.Size(1001, 392);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).TabIndex = 0;
		this.grdsaleorder.TotalRow = (InheritableBoolean)1;
		this.grdsaleorder.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdsaleorder).VisualStyle = (VisualStyle)4;
		this.grdsaleorder.ColumnButtonClick += new ColumnActionEventHandler(grdsaleorder_ColumnButtonClick);
		this.grdsaleorder.LinkClicked += new ColumnActionEventHandler(grdsaleorder_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).KeyDown += new System.Windows.Forms.KeyEventHandler(grdsaleorder_KeyDown);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(957, 4);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdsaleorder;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar3.TabIndex = 24;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Dock = System.Windows.Forms.DockStyle.Top;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(3, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(1001, 26);
		this.label6.TabIndex = 1;
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage2.Controls.Add(this.ctrlGrdBar7);
		this.tabPage2.Controls.Add(this.ctrlGrdBar6);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDetail);
		this.tabPage2.Controls.Add(this.label13);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdMain);
		this.tabPage2.Controls.Add(this.label12);
		this.tabPage2.Controls.Add(this.ctrlGrdBar4);
		this.tabPage2.Controls.Add(this.ctrlGrdBar2);
		this.tabPage2.Location = new System.Drawing.Point(4, 24);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1007, 424);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Brief";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(964, 276);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(6);
		this.ctrlGrdBar7.MyGrid = this.GrdDetail;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar7.TabIndex = 32;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.GrdDetail.AllowEdit = (InheritableBoolean)2;
		this.GrdDetail.AlternatingColors = true;
		((ScrollableBase)this.GrdDetail).BackColor = System.Drawing.Color.White;
		GrdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDetail.DesignTimeLayout = GrdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdDetail.FilterMode = (FilterMode)2;
		this.GrdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDetail.GroupTotals = (GroupTotals)2;
		this.GrdDetail.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Location = new System.Drawing.Point(3, 298);
		((System.Windows.Forms.Control)(object)this.GrdDetail).Name = "GrdDetail";
		((ScrollableBase)this.GrdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Size = new System.Drawing.Size(1001, 123);
		((System.Windows.Forms.Control)(object)this.GrdDetail).TabIndex = 29;
		this.GrdDetail.TotalRow = (InheritableBoolean)1;
		this.GrdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDetail).VisualStyle = (VisualStyle)4;
		this.GrdDetail.LinkClicked += new ColumnActionEventHandler(GrdDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdDetail_KeyDown);
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(964, 4);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar6.MyGrid = this.GrdMain;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar6.TabIndex = 30;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.GrdMain.AllowEdit = (InheritableBoolean)2;
		this.GrdMain.AlternatingColors = true;
		((ScrollableBase)this.GrdMain).BackColor = System.Drawing.Color.White;
		GrdMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdMain.DesignTimeLayout = GrdMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdMain).Dock = System.Windows.Forms.DockStyle.Top;
		this.GrdMain.FilterMode = (FilterMode)2;
		this.GrdMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdMain.GroupTotals = (GroupTotals)2;
		this.GrdMain.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GrdMain).Location = new System.Drawing.Point(3, 26);
		((System.Windows.Forms.Control)(object)this.GrdMain).Name = "GrdMain";
		((ScrollableBase)this.GrdMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdMain).Size = new System.Drawing.Size(1001, 249);
		((System.Windows.Forms.Control)(object)this.GrdMain).TabIndex = 26;
		((System.Windows.Forms.Control)(object)this.GrdMain).Tag = "MasterGrid";
		this.GrdMain.TotalRow = (InheritableBoolean)1;
		this.GrdMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdMain).VisualStyle = (VisualStyle)4;
		this.GrdMain.ColumnButtonClick += new ColumnActionEventHandler(GrdMain_ColumnButtonClick);
		this.GrdMain.SelectionChanged += new System.EventHandler(GrdMain_SelectionChanged);
		this.GrdMain.LinkClicked += new ColumnActionEventHandler(GrdMain_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdMain).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdMain_KeyDown);
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Dock = System.Windows.Forms.DockStyle.Top;
		this.label13.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(3, 275);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(1001, 23);
		this.label13.TabIndex = 27;
		this.label13.Text = "Detail Grid";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label12.BackColor = System.Drawing.Color.Teal;
		this.label12.Dock = System.Windows.Forms.DockStyle.Top;
		this.label12.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(3, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(1001, 23);
		this.label12.TabIndex = 2;
		this.label12.Text = "Main Grid";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.ctrlGrdBar4.FormName = null;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(0, 0);
		this.ctrlGrdBar4.MyGrid = null;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(150, 150);
		this.ctrlGrdBar4.TabIndex = 33;
		this.ctrlGrdBar2.FormName = null;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(0, 0);
		this.ctrlGrdBar2.MyGrid = null;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(150, 150);
		this.ctrlGrdBar2.TabIndex = 31;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1015, 138);
		this.panel4.TabIndex = 1;
		this.groupBox1.Controls.Add(this.chkSkipZeroDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemType);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.chkincludeDo);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderStatus);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.fromdate);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.ToDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategory);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtSrFrom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategory);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtsrTo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbIsApproved);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1003, 130);
		this.groupBox1.TabIndex = 144;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filter Criteria's";
		this.chkSkipZeroDetail.AutoSize = true;
		this.chkSkipZeroDetail.Checked = true;
		this.chkSkipZeroDetail.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkSkipZeroDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSkipZeroDetail.Location = new System.Drawing.Point(544, 102);
		this.chkSkipZeroDetail.Name = "chkSkipZeroDetail";
		this.chkSkipZeroDetail.Size = new System.Drawing.Size(76, 19);
		this.chkSkipZeroDetail.TabIndex = 11;
		this.chkSkipZeroDetail.Text = "Skip Zero";
		this.chkSkipZeroDetail.UseVisualStyleBackColor = true;
		this.CmbItemType.AllowNull = (DefaultableBoolean)1;
		this.CmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance181).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Location = new System.Drawing.Point(319, 70);
		((System.Windows.Forms.Control)(object)this.CmbItemType).Name = "CmbItemType";
		((System.Windows.Forms.Control)(object)this.CmbItemType).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemType).TabIndex = 6;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(541, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 15);
		this.label1.TabIndex = 123;
		this.label1.Text = "Customer Name";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance267;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance268).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance268).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance268).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance269;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance271;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance272).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance273).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance273).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance273).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance273;
		((AppearanceBase)appearance274).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance275).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(638, 16);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 8;
		this.chkincludeDo.AutoSize = true;
		this.chkincludeDo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkincludeDo.Location = new System.Drawing.Point(638, 102);
		this.chkincludeDo.Name = "chkincludeDo";
		this.chkincludeDo.Size = new System.Drawing.Size(138, 19);
		this.chkincludeDo.TabIndex = 12;
		this.chkincludeDo.Text = "Include approved Do";
		this.chkincludeDo.UseVisualStyleBackColor = true;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(224, 104);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 125;
		this.label2.Text = "Item Name";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(541, 49);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(95, 15);
		this.label11.TabIndex = 141;
		this.label11.Text = "Approved Status";
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance254).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance254).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance255;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance256).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance256).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance256).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance257).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance260).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(319, 98);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 7;
		this.CmbOrderStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance194;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance195).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance195).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance199).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance200).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance201).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Location = new System.Drawing.Point(638, 70);
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Name = "CmbOrderStatus";
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).TabIndex = 10;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(6, 22);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(63, 15);
		this.label33.TabIndex = 127;
		this.label33.Text = "From Date";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(541, 76);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(40, 15);
		this.label10.TabIndex = 140;
		this.label10.Text = "Status";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(72, 18);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(149, 23);
		this.fromdate.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 49);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 15);
		this.label3.TabIndex = 129;
		this.label3.Text = "To Date";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(224, 76);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(60, 15);
		this.label9.TabIndex = 138;
		this.label9.Text = "Item Type";
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(72, 45);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(149, 23);
		this.ToDate.TabIndex = 1;
		this.CmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance219;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance224).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Location = new System.Drawing.Point(319, 43);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Name = "CmbItemCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).TabIndex = 5;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(6, 76);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(52, 15);
		this.label4.TabIndex = 130;
		this.label4.Text = "So From";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(224, 49);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(82, 15);
		this.label8.TabIndex = 138;
		this.label8.Text = "Item Category";
		this.txtSrFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSrFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSrFrom.Location = new System.Drawing.Point(72, 72);
		this.txtSrFrom.Name = "txtSrFrom";
		this.txtSrFrom.Size = new System.Drawing.Size(149, 23);
		this.txtSrFrom.TabIndex = 2;
		this.txtSrFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSrFrom_KeyPress);
		this.CmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance231;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance232).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance232).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance232).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance233).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance236).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance237).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Location = new System.Drawing.Point(319, 16);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Name = "CmbParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).TabIndex = 4;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 104);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(37, 15);
		this.label5.TabIndex = 132;
		this.label5.Text = "So To";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(224, 22);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(91, 15);
		this.label7.TabIndex = 138;
		this.label7.Text = "Parent Category";
		this.txtsrTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsrTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsrTo.Location = new System.Drawing.Point(72, 100);
		this.txtsrTo.Name = "txtsrTo";
		this.txtsrTo.Size = new System.Drawing.Size(149, 23);
		this.txtsrTo.TabIndex = 3;
		this.txtsrTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsrTo_KeyPress);
		this.CmbIsApproved.AllowNull = (DefaultableBoolean)1;
		this.CmbIsApproved.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbIsApproved.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbIsApproved.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Appearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance243;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance244).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance244).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance247;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance248).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance249).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance249).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance249).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbIsApproved.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Location = new System.Drawing.Point(638, 43);
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Name = "CmbIsApproved";
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).TabIndex = 9;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(795, 98);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(57, 26);
		this.btnshow.TabIndex = 13;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click_1);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1015, 30);
		this.panel2.TabIndex = 0;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.btnRefreshDetail, this.print });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1015, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(152, 24);
		this.print.Text = "299-RegisterDetail";
		this.print.Click += new System.EventHandler(print_Click);
		this.tabSummary.Controls.Add(this.panel9);
		this.tabSummary.Controls.Add(this.panel7);
		this.tabSummary.Controls.Add(this.panel8);
		this.tabSummary.Location = new System.Drawing.Point(4, 22);
		this.tabSummary.Name = "tabSummary";
		this.tabSummary.Padding = new System.Windows.Forms.Padding(3);
		this.tabSummary.Size = new System.Drawing.Size(1021, 626);
		this.tabSummary.TabIndex = 1;
		this.tabSummary.Text = "Summary";
		this.tabSummary.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.ctrlGrdBar5);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel9.Controls.Add(this.label26);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(3, 174);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1015, 449);
		this.panel9.TabIndex = 4;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(976, 1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar5.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar5.TabIndex = 25;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.AlternatingColors = true;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		this.DataGridHistory.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1015, 426);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.label26.BackColor = System.Drawing.Color.Teal;
		this.label26.Dock = System.Windows.Forms.DockStyle.Top;
		this.label26.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(0, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(1015, 23);
		this.label26.TabIndex = 3;
		this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 33);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1015, 141);
		this.panel7.TabIndex = 0;
		this.groupBox2.Controls.Add(this.chkSkipZero);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomerSumm);
		this.groupBox2.Controls.Add(this.ChkDoApprovedIncludSummary);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbActivitySumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemNameSumm);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLotSumm);
		this.groupBox2.Controls.Add(this.txtFromDateSumm);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.txtToDateSumm);
		this.groupBox2.Controls.Add(this.txtFromDocNoSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcitySumm);
		this.groupBox2.Controls.Add(this.txtToDocNoSumm);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add(this.BtnShowSumm);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategorySumm);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 4);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(910, 133);
		this.groupBox2.TabIndex = 159;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filter Criteria's";
		this.chkSkipZero.AutoSize = true;
		this.chkSkipZero.Checked = true;
		this.chkSkipZero.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkSkipZero.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSkipZero.Location = new System.Drawing.Point(687, 79);
		this.chkSkipZero.Name = "chkSkipZero";
		this.chkSkipZero.Size = new System.Drawing.Size(76, 19);
		this.chkSkipZero.TabIndex = 12;
		this.chkSkipZero.Text = "Skip Zero";
		this.chkSkipZero.UseVisualStyleBackColor = true;
		this.cmbItemtypeSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtypeSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtypeSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtypeSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemtypeSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Location = new System.Drawing.Point(408, 18);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Name = "cmbItemtypeSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).TabIndex = 6;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(589, 24);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(93, 15);
		this.label25.TabIndex = 123;
		this.label25.Text = "Customer Name";
		this.cmbCustomerSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomerSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomerSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomerSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance186).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance187).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomerSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Location = new System.Drawing.Point(686, 18);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Name = "cmbCustomerSumm";
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).TabIndex = 10;
		this.ChkDoApprovedIncludSummary.AutoSize = true;
		this.ChkDoApprovedIncludSummary.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkDoApprovedIncludSummary.Location = new System.Drawing.Point(768, 79);
		this.ChkDoApprovedIncludSummary.Name = "ChkDoApprovedIncludSummary";
		this.ChkDoApprovedIncludSummary.Size = new System.Drawing.Size(138, 19);
		this.ChkDoApprovedIncludSummary.TabIndex = 13;
		this.ChkDoApprovedIncludSummary.Text = "Include approved Do";
		this.ChkDoApprovedIncludSummary.UseVisualStyleBackColor = true;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(336, 108);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(67, 15);
		this.label24.TabIndex = 125;
		this.label24.Text = "Item Name";
		this.cmbActivitySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbActivitySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbActivitySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbActivitySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance207;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance215).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbActivitySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Location = new System.Drawing.Point(686, 46);
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Name = "cmbActivitySumm";
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).TabIndex = 11;
		this.cmbItemNameSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbItemNameSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemNameSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemNameSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemNameSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Location = new System.Drawing.Point(408, 102);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Name = "cmbItemNameSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).TabIndex = 9;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(589, 52);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(70, 15);
		this.label14.TabIndex = 155;
		this.label14.Text = "Report Type";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(7, 24);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(91, 15);
		this.label22.TabIndex = 127;
		this.label22.Text = "From && To Date";
		this.cmbJobLotSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLotSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLotSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLotSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance277).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance278).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance278).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance279).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance279;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobLotSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Location = new System.Drawing.Point(408, 46);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Name = "cmbJobLotSumm";
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).TabIndex = 7;
		this.txtFromDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtFromDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateSumm.Location = new System.Drawing.Point(112, 20);
		this.txtFromDateSumm.Name = "txtFromDateSumm";
		this.txtFromDateSumm.Size = new System.Drawing.Size(107, 23);
		this.txtFromDateSumm.TabIndex = 0;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(336, 52);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(46, 15);
		this.label32.TabIndex = 153;
		this.label32.Text = "Job Lot";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(7, 52);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(104, 15);
		this.label21.TabIndex = 129;
		this.label21.Text = "From && To DocNo";
		this.txtToDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtToDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateSumm.Location = new System.Drawing.Point(223, 20);
		this.txtToDateSumm.Name = "txtToDateSumm";
		this.txtToDateSumm.Size = new System.Drawing.Size(107, 23);
		this.txtToDateSumm.TabIndex = 1;
		this.txtFromDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoSumm.Location = new System.Drawing.Point(112, 48);
		this.txtFromDocNoSumm.Name = "txtFromDocNoSumm";
		this.txtFromDocNoSumm.Size = new System.Drawing.Size(107, 23);
		this.txtFromDocNoSumm.TabIndex = 2;
		this.txtFromDocNoSumm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNo_KeyPress);
		this.cmbcitySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbcitySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcitySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcitySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance119).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcitySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Location = new System.Drawing.Point(408, 74);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Name = "cmbcitySumm";
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).TabIndex = 8;
		this.txtToDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoSumm.Location = new System.Drawing.Point(223, 48);
		this.txtToDocNoSumm.Name = "txtToDocNoSumm";
		this.txtToDocNoSumm.Size = new System.Drawing.Size(107, 23);
		this.txtToDocNoSumm.TabIndex = 3;
		this.txtToDocNoSumm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNo_KeyPress);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(335, 80);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(62, 15);
		this.label30.TabIndex = 149;
		this.label30.Text = "City Name";
		this.BtnShowSumm.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowSumm.FlatAppearance.BorderSize = 0;
		this.BtnShowSumm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowSumm.ForeColor = System.Drawing.Color.White;
		this.BtnShowSumm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowSumm.Location = new System.Drawing.Point(686, 102);
		this.BtnShowSumm.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowSumm.Name = "BtnShowSumm";
		this.BtnShowSumm.Size = new System.Drawing.Size(57, 26);
		this.BtnShowSumm.TabIndex = 14;
		this.BtnShowSumm.Text = "Show";
		this.BtnShowSumm.UseVisualStyleBackColor = false;
		this.BtnShowSumm.Click += new System.EventHandler(BtnShowSumm_Click);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(7, 80);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(91, 15);
		this.label18.TabIndex = 138;
		this.label18.Text = "Parent Category";
		this.cmbParentcategorySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbParentcategorySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentcategorySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentcategorySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentcategorySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Location = new System.Drawing.Point(112, 74);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Name = "cmbParentcategorySumm";
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).TabIndex = 4;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(7, 108);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(82, 15);
		this.label17.TabIndex = 138;
		this.label17.Text = "Item Category";
		this.CmbCategorySumm.AllowNull = (DefaultableBoolean)1;
		this.CmbCategorySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategorySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategorySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategorySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Location = new System.Drawing.Point(112, 102);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Name = "CmbCategorySumm";
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).TabIndex = 5;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(336, 24);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(60, 15);
		this.label16.TabIndex = 138;
		this.label16.Text = "Item Type";
		this.panel8.Controls.Add(this.toolStrip1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1015, 30);
		this.panel8.TabIndex = 2;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewSumm, this.btnRefreshSummary, this.btnPrintSumm });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1015, 30);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefreshSummary.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshSummary.Name = "btnRefreshSummary";
		this.btnRefreshSummary.Size = new System.Drawing.Size(80, 27);
		this.btnRefreshSummary.Text = "&Refresh";
		this.btnRefreshSummary.Click += new System.EventHandler(btnRefreshSummary_Click);
		this.btnPrintSumm.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintSumm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSumm.Name = "btnPrintSumm";
		this.btnPrintSumm.Size = new System.Drawing.Size(61, 27);
		this.btnPrintSumm.Text = "Print";
		this.btnPrintSumm.Click += new System.EventHandler(btnPrintSummary_Click);
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1029, 28);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(156, 19);
		this.label23.TabIndex = 3;
		this.label23.Text = "Sale Order Report";
		this.btnRefreshDetail.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshDetail.Name = "btnRefreshDetail";
		this.btnRefreshDetail.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshDetail.Text = "&Refresh";
		this.btnRefreshDetail.Click += new System.EventHandler(btnRefreshDetail_Click);
		this.btnNewSumm.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewSumm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewSumm.Name = "btnNewSumm";
		this.btnNewSumm.Size = new System.Drawing.Size(68, 27);
		this.btnNewSumm.Text = "Reset";
		this.btnNewSumm.Click += new System.EventHandler(btnNewSumm_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1029, 680);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmSaleOrderHistory_Engr";
		this.Text = "frmSaleOrderHistory";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSaleOrderHistory_FormClosing);
		base.Load += new System.EventHandler(frmSaleOrderHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleOrderHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).EndInit();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.GrdMain).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApproved).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabSummary.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivitySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
	}
}
