using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Steel.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Steel.Reports.SalesReports;

public class SaleInvoiceRegisterSteel : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtInvoice = new DataTable();

	private DataTable dtInvoiceDirect = new DataTable();

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Label label7;

	private ComboBox CmbIsApproved;

	internal UltraCombo cmbitem;

	private Label label6;

	private Label label4;

	private TextBox txtInvoiceNoto;

	private DateTimePicker ToDate;

	private DateTimePicker FromDate;

	private Label label27;

	private Label label33;

	private TextBox txtInvoiceNoFrom;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Label label1;

	private Panel panel5;

	internal GridEX DataGridHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private ToolStripDropDownButton tsDropDown;

	private Label label8;

	private TextBox txtsaleorderto;

	private TextBox txtsaleorderfrom;

	private Label label9;

	private Label label3;

	private TextBox txtrateto;

	private TextBox txtratefrom;

	private Label label5;

	private Label label10;

	internal UltraCombo cmbWareHouse;

	private Label label11;

	internal UltraCombo CmbJobLot;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Label label12;

	private TabPage tabPage2;

	private Label label13;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX DataGridHistoryDirect;

	private ToolStripDropDownButton tsDropDownForDirect;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar4;

	public SaleInvoiceRegisterSteel(UserAccount user)
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
			GetDataForSaleInvoiceRegisterDropdownBind();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			FromDate.Focus();
			ReportsLoad();
			gridHisory();
			DirectRegisterHistory();
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
			string[] files = CommonServices.DynamicReportsLoad("Sales_Steel");
			if (files != null && files.Length != 0)
			{
				for (int j = 0; j < files.Length; j++)
				{
					tsDropDown.DropDownItems.Add(Path.GetFileNameWithoutExtension(files[j]));
				}
			}
			string[] Directfiles = CommonServices.DynamicReportsLoad("SalesDirect_Steel ");
			if (Directfiles != null && Directfiles.Length != 0)
			{
				for (int i = 0; i < Directfiles.Length; i++)
				{
					tsDropDownForDirect.DropDownItems.Add(Path.GetFileNameWithoutExtension(Directfiles[i]));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDataForSaleInvoiceRegisterDropdownBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtAllData = InvSaleInvoice.GetDataForDropDownFromSaleInvoiceForSteel(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtAllData.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtsup = new DataTable();
			DataTable dtwr = new DataTable();
			DataTable dtjob = new DataTable();
			DataTable dtItem = new DataTable();
			dtsup.Columns.Add("SupplierCustomerId");
			dtsup.Columns.Add("CompanyName");
			dtwr.Columns.Add("WarehouseId");
			dtwr.Columns.Add("WareHouseName");
			dtjob.Columns.Add("JobLotId");
			dtjob.Columns.Add("JobLotDescription");
			dtItem.Columns.Add("ItemId");
			dtItem.Columns.Add("ItemName");
			for (int i = 0; i < dtAllData.Rows.Count; i++)
			{
				if (Conversion.ToString(dtAllData.Rows[i]["Activity"]) == "Customer")
				{
					dtsup.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtAllData.Rows[i]["Activity"]) == "JobLot")
				{
					dtjob.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtAllData.Rows[i]["Activity"]) == "Item")
				{
					dtItem.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtAllData.Rows[i]["Activity"]) == "Warehouse")
				{
					dtwr.Rows.Add(dtAllData.Rows[i]["Id"], dtAllData.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtsup, CmbSupplier, "SupplierCustomerId", "CompanyName", "Customer Name", ZeroIndex: false);
			DDL.BindDDL(dtwr, cmbWareHouse, "WarehouseId", "WareHouseName", "Warehouse", ZeroIndex: false);
			DDL.BindDDL(dtjob, CmbJobLot, "JobLotId", "JobLotDescription", "Job Lot", ZeroIndex: false);
			DDL.BindDDL(dtItem, cmbitem, "ItemId", "ItemName", "Item Name", ZeroIndex: false);
			((UltraGridBase)CmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbWareHouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbJobLot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbitem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			dtInvoice.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = FromDate.Value;
			obj.ToDate = ToDate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtInvoiceNoFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtInvoiceNoto.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(cmbitem.Value);
			obj.BillNoFrom = Conversion.ToInt((object)txtsaleorderfrom.Text.Trim());
			obj.BillNoTo = Conversion.ToInt((object)txtsaleorderto.Text.Trim());
			obj.RateFrom = Conversion.ToDouble((object)txtratefrom.Text.Trim());
			obj.RateTo = Conversion.ToDouble((object)txtrateto.Text.Trim());
			obj.WarehouseId = Conversion.ToInt(cmbWareHouse.Value);
			obj.JobLotId = Conversion.ToInt(CmbJobLot.Value);
			dtInvoice = InvSaleInvoice.SaleInvoiceRegister(obj);
			if (dtInvoice.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Id", typeof(int));
				dtGrid.Columns.Add("InvoiceDetailId", typeof(int));
				dtGrid.Columns.Add("DocumentTypeId", typeof(int));
				dtGrid.Columns.Add("VoucherHeadId", typeof(int));
				dtGrid.Columns.Add("DocDate", typeof(DateTime));
				dtGrid.Columns.Add("DocNo", typeof(int));
				dtGrid.Columns.Add("InvoiceType");
				dtGrid.Columns.Add("OrderId");
				dtGrid.Columns.Add("OrderDate", typeof(DateTime));
				dtGrid.Columns.Add("OrderNo", typeof(int));
				dtGrid.Columns.Add("GpId", typeof(int));
				dtGrid.Columns.Add("GpDate", typeof(DateTime));
				dtGrid.Columns.Add("GpSrNo", typeof(int));
				dtGrid.Columns.Add("GrnId", typeof(int));
				dtGrid.Columns.Add("GrnDate", typeof(DateTime));
				dtGrid.Columns.Add("GrnNo", typeof(int));
				dtGrid.Columns.Add("VehicleNo");
				dtGrid.Columns.Add("BiltyNo");
				dtGrid.Columns.Add("SupplierCustomerId", typeof(int));
				dtGrid.Columns.Add("CustomerName");
				dtGrid.Columns.Add("CommAgentName");
				dtGrid.Columns.Add("CommType");
				dtGrid.Columns.Add("CommRate", typeof(double));
				dtGrid.Columns.Add("CommAmount", typeof(double));
				dtGrid.Columns.Add("PartyBillAmount", typeof(double));
				dtGrid.Columns.Add("DueDate", typeof(DateTime));
				dtGrid.Columns.Add("ManualBillNo");
				dtGrid.Columns.Add("SaleGLAC");
				dtGrid.Columns.Add("ItemId", typeof(int));
				dtGrid.Columns.Add("ItemName");
				dtGrid.Columns.Add("WareHouseCode");
				dtGrid.Columns.Add("JobLotCode");
				dtGrid.Columns.Add("PackTypeCode");
				dtGrid.Columns.Add("UOMCode");
				dtGrid.Columns.Add("ItemQty", typeof(double));
				dtGrid.Columns.Add("GrossWeight", typeof(double));
				dtGrid.Columns.Add("WtCutPerUnit", typeof(double));
				dtGrid.Columns.Add("WtCutTotal", typeof(double));
				dtGrid.Columns.Add("AdLsWeight", typeof(double));
				dtGrid.Columns.Add("NetBillWeight", typeof(double));
				dtGrid.Columns.Add("NetStockWeight", typeof(double));
				dtGrid.Columns.Add("ItemRate", typeof(double));
				dtGrid.Columns.Add("NetRate", typeof(double));
				dtGrid.Columns.Add("RateUom");
				dtGrid.Columns.Add("RateCut", typeof(double));
				dtGrid.Columns.Add("RateCutAmount", typeof(double));
				dtGrid.Columns.Add("ItemAmount", typeof(double));
				dtGrid.Columns.Add("FreightAmount", typeof(double));
				dtGrid.Columns.Add("ExpenseAmount", typeof(double));
				dtGrid.Columns.Add("CommissionAmount", typeof(double));
				dtGrid.Columns.Add("ItemNetAmount", typeof(double));
				dtGrid.Columns.Add("EntryDate", typeof(DateTime));
				dtGrid.Columns.Add("EntryUser");
				dtGrid.Clear();
				for (int i = 0; i < dtInvoice.Rows.Count; i++)
				{
					dtGrid.Rows.Add(dtInvoice.Rows[i]["Id"], dtInvoice.Rows[i]["InvSaleDetailId"], dtInvoice.Rows[i]["DocumentTypeId"], dtInvoice.Rows[i]["VoucherHeadId"], Conversion.ToDateTime(dtInvoice.Rows[i]["DocDate"]).ToShortDateString(), dtInvoice.Rows[i]["DocNo"], dtInvoice.Rows[i]["OrderType"], dtInvoice.Rows[i]["OrderId"], Conversion.ToDateTime(dtInvoice.Rows[i]["DocDatepo"]).ToShortDateString(), dtInvoice.Rows[i]["DocNopo"], dtInvoice.Rows[i]["GpId"], Conversion.ToDateTime(dtInvoice.Rows[i]["GpDate"]).ToShortDateString(), dtInvoice.Rows[i]["GpSrNo"], dtInvoice.Rows[i]["GrnId"], Conversion.ToDateTime(dtInvoice.Rows[i]["DocDateGrn"]).ToShortDateString(), dtInvoice.Rows[i]["DocNoGrn"], dtInvoice.Rows[i]["VehicleNo"], dtInvoice.Rows[i]["BiltyNo"], dtInvoice.Rows[i]["SupplierCustomerId"], dtInvoice.Rows[i]["CustomerName"], dtInvoice.Rows[i]["CommAgentName"], dtInvoice.Rows[i]["CommType"], dtInvoice.Rows[i]["CommRate"], dtInvoice.Rows[i]["CommAmount"], dtInvoice.Rows[i]["BillAmountH"], dtInvoice.Rows[i]["DueDate"], dtInvoice.Rows[i]["ManualBillNo"], dtInvoice.Rows[i]["SaleGLAC"], dtInvoice.Rows[i]["ItemId"], dtInvoice.Rows[i]["ItemName"], dtInvoice.Rows[i]["WareHouseCode"], dtInvoice.Rows[i]["JobLotCode"], dtInvoice.Rows[i]["PackTypeCode"], dtInvoice.Rows[i]["UOMCode"], dtInvoice.Rows[i]["ItemQty"], dtInvoice.Rows[i]["GrossWeight"], dtInvoice.Rows[i]["WtCutPerUnit"], dtInvoice.Rows[i]["WtCutTotal"], dtInvoice.Rows[i]["AdLsWeight"], dtInvoice.Rows[i]["NetBillWeight"], dtInvoice.Rows[i]["NetStockWeight"], dtInvoice.Rows[i]["ItemRate"], dtInvoice.Rows[i]["NetRate"], dtInvoice.Rows[i]["RateUom"], dtInvoice.Rows[i]["RateCut"], dtInvoice.Rows[i]["RateCutAmount"], dtInvoice.Rows[i]["ItemAmount"], dtInvoice.Rows[i]["FreightAmount"], dtInvoice.Rows[i]["ExpenseAmount"], dtInvoice.Rows[i]["CommissionAmount"], dtInvoice.Rows[i]["ItemNetAmount"], dtInvoice.Rows[i]["EntryDate"], dtInvoice.Rows[i]["EntryUserName"]);
				}
				DataGridHistory.DataSource = dtGrid;
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
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			DataGridHistory.RootTable.Columns["InvoiceDetailId"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["OrderId"].Visible = false;
			DataGridHistory.RootTable.Columns["GpId"].Visible = false;
			DataGridHistory.RootTable.Columns["GrnId"].Visible = false;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			DataGridHistory.RootTable.Columns["SaleGLAC"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistory.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			DataGridHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["PartyBillAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["PartyBillAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["PartyBillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["PartyBillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["WtCutPerUnit"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WtCutPerUnit"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WtCutPerUnit"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["WtCutPerUnit"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["WtCutTotal"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WtCutTotal"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WtCutTotal"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["WtCutTotal"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["AdLsWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AdLsWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AdLsWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["AdLsWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetStockWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetStockWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetStockWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetStockWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FreightAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["FreightAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ExpenseAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ExpenseAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommissionAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["CommissionAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Voucher");
			DataGridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].Width = 60;
			DataGridHistory.RootTable.Columns["Voucher"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DirectRegisterHistory()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtInvoiceDirect.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = FromDate.Value;
			obj.ToDate = ToDate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtInvoiceNoFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtInvoiceNoto.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(cmbitem.Value);
			obj.BillNoFrom = Conversion.ToInt((object)txtsaleorderfrom.Text.Trim());
			obj.BillNoTo = Conversion.ToInt((object)txtsaleorderto.Text.Trim());
			obj.RateFrom = Conversion.ToDouble((object)txtratefrom.Text.Trim());
			obj.RateTo = Conversion.ToDouble((object)txtrateto.Text.Trim());
			obj.WarehouseId = Conversion.ToInt(cmbWareHouse.Value);
			obj.JobLotId = Conversion.ToInt(CmbJobLot.Value);
			dtInvoiceDirect = InvSaleInvoice.SaleInvoiceDirectRegister(obj);
			if (dtInvoiceDirect.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Id", typeof(int));
				dtGrid.Columns.Add("InvoiceDetailId", typeof(int));
				dtGrid.Columns.Add("DocumentTypeId", typeof(int));
				dtGrid.Columns.Add("VoucherHeadId", typeof(int));
				dtGrid.Columns.Add("DocDate", typeof(DateTime));
				dtGrid.Columns.Add("DocNo", typeof(int));
				dtGrid.Columns.Add("InvoiceType");
				dtGrid.Columns.Add("GpDate", typeof(DateTime));
				dtGrid.Columns.Add("GpSrNo", typeof(int));
				dtGrid.Columns.Add("VehicleNo");
				dtGrid.Columns.Add("SupplierCustomerId", typeof(int));
				dtGrid.Columns.Add("CustomerName");
				dtGrid.Columns.Add("CommAgentName");
				dtGrid.Columns.Add("CommType");
				dtGrid.Columns.Add("CommRate", typeof(double));
				dtGrid.Columns.Add("CommAmount", typeof(double));
				dtGrid.Columns.Add("PartyBillAmount", typeof(double));
				dtGrid.Columns.Add("DueDate", typeof(DateTime));
				dtGrid.Columns.Add("ManualBillNo");
				dtGrid.Columns.Add("SaleGLAC");
				dtGrid.Columns.Add("ItemId", typeof(int));
				dtGrid.Columns.Add("ItemName");
				dtGrid.Columns.Add("WareHouseCode");
				dtGrid.Columns.Add("JobLotCode");
				dtGrid.Columns.Add("PackTypeCode");
				dtGrid.Columns.Add("UOMCode");
				dtGrid.Columns.Add("ItemQty", typeof(double));
				dtGrid.Columns.Add("GrossWeight", typeof(double));
				dtGrid.Columns.Add("WtCutPerUnit", typeof(double));
				dtGrid.Columns.Add("WtCutTotal", typeof(double));
				dtGrid.Columns.Add("AdLsWeight", typeof(double));
				dtGrid.Columns.Add("NetBillWeight", typeof(double));
				dtGrid.Columns.Add("NetStockWeight", typeof(double));
				dtGrid.Columns.Add("ItemRate", typeof(double));
				dtGrid.Columns.Add("NetRate", typeof(double));
				dtGrid.Columns.Add("RateUom");
				dtGrid.Columns.Add("RateCut", typeof(double));
				dtGrid.Columns.Add("RateCutAmount", typeof(double));
				dtGrid.Columns.Add("ItemAmount", typeof(double));
				dtGrid.Columns.Add("FreightAmount", typeof(double));
				dtGrid.Columns.Add("ExpenseAmount", typeof(double));
				dtGrid.Columns.Add("CommissionAmount", typeof(double));
				dtGrid.Columns.Add("ItemNetAmount", typeof(double));
				dtGrid.Columns.Add("EntryDate", typeof(DateTime));
				dtGrid.Columns.Add("EntryUser");
				dtGrid.Clear();
				for (int i = 0; i < dtInvoiceDirect.Rows.Count; i++)
				{
					dtGrid.Rows.Add(dtInvoiceDirect.Rows[i]["Id"], dtInvoiceDirect.Rows[i]["InvSaleDetailId"], dtInvoiceDirect.Rows[i]["DocumentTypeId"], dtInvoiceDirect.Rows[i]["VoucherHeadId"], Conversion.ToDateTime(dtInvoiceDirect.Rows[i]["DocDate"]).ToShortDateString(), dtInvoiceDirect.Rows[i]["DocNo"], dtInvoiceDirect.Rows[i]["OrderType"], Conversion.ToDateTime(dtInvoiceDirect.Rows[i]["GpDate"]).ToShortDateString(), dtInvoiceDirect.Rows[i]["GpSrNo"], dtInvoiceDirect.Rows[i]["VehicleNo"], dtInvoiceDirect.Rows[i]["SupplierCustomerId"], dtInvoiceDirect.Rows[i]["CustomerName"], dtInvoiceDirect.Rows[i]["CommAgentName"], dtInvoiceDirect.Rows[i]["CommType"], dtInvoiceDirect.Rows[i]["CommRate"], dtInvoiceDirect.Rows[i]["CommAmount"], dtInvoiceDirect.Rows[i]["BillAmountH"], Conversion.ToDateTime(dtInvoiceDirect.Rows[i]["DueDate"]).ToShortDateString(), dtInvoiceDirect.Rows[i]["ManualBillNo"], dtInvoiceDirect.Rows[i]["SaleGLAC"], dtInvoiceDirect.Rows[i]["ItemId"], dtInvoiceDirect.Rows[i]["ItemName"], dtInvoiceDirect.Rows[i]["WareHouseCode"], dtInvoiceDirect.Rows[i]["JobLotCode"], dtInvoiceDirect.Rows[i]["PackTypeCode"], dtInvoiceDirect.Rows[i]["UOMCode"], dtInvoiceDirect.Rows[i]["ItemQty"], dtInvoiceDirect.Rows[i]["GrossWeight"], dtInvoiceDirect.Rows[i]["WtCutPerUnit"], dtInvoiceDirect.Rows[i]["WtCutTotal"], dtInvoiceDirect.Rows[i]["AdLsWeight"], dtInvoiceDirect.Rows[i]["NetBillWeight"], dtInvoiceDirect.Rows[i]["NetStockWeight"], dtInvoiceDirect.Rows[i]["ItemRate"], dtInvoiceDirect.Rows[i]["NetRate"], dtInvoiceDirect.Rows[i]["RateUom"], dtInvoiceDirect.Rows[i]["RateCut"], dtInvoiceDirect.Rows[i]["RateCutAmount"], dtInvoiceDirect.Rows[i]["ItemAmount"], dtInvoiceDirect.Rows[i]["FreightAmount"], dtInvoiceDirect.Rows[i]["ExpenseAmount"], dtInvoiceDirect.Rows[i]["CommissionAmount"], dtInvoiceDirect.Rows[i]["ItemNetAmount"], Conversion.ToDateTime(dtInvoiceDirect.Rows[i]["EntryDate"]).ToShortDateString(), dtInvoiceDirect.Rows[i]["EntryUserName"]);
				}
				DataGridHistoryDirect.DataSource = dtGrid;
				DataGridHistoryDirect.RetrieveStructure();
				GridDirectSettings();
			}
			else
			{
				DataGridHistoryDirect.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDirectSettings()
	{
		try
		{
			DataGridHistoryDirect.RootTable.Columns["Id"].Visible = false;
			DataGridHistoryDirect.RootTable.Columns["VoucherHeadId"].Visible = false;
			DataGridHistoryDirect.RootTable.Columns["InvoiceDetailId"].Visible = false;
			DataGridHistoryDirect.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistoryDirect.RootTable.Columns["SupplierCustomerId"].Visible = false;
			DataGridHistoryDirect.RootTable.Columns["SaleGLAC"].Visible = false;
			DataGridHistoryDirect.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistoryDirect.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			DataGridHistoryDirect.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			DataGridHistoryDirect.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistoryDirect.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["PartyBillAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["PartyBillAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["PartyBillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["PartyBillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["WtCutPerUnit"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["WtCutPerUnit"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["WtCutPerUnit"].FormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["WtCutPerUnit"].TotalFormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["WtCutTotal"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["WtCutTotal"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["WtCutTotal"].FormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["WtCutTotal"].TotalFormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["AdLsWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["AdLsWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["AdLsWeight"].FormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["AdLsWeight"].TotalFormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["NetStockWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["NetStockWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["NetStockWeight"].FormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["NetStockWeight"].TotalFormatString = "#,##0.###";
			DataGridHistoryDirect.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistoryDirect.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistoryDirect.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistoryDirect.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["FreightAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["FreightAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["ExpenseAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["ExpenseAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["CommissionAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["CommissionAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDirect.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDirect.RootTable.Columns["ItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDirect.RootTable.Columns["ItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, DataGridHistoryDirect);
			DataGridHistoryDirect.RootTable.Columns.Add("Print");
			DataGridHistoryDirect.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistoryDirect.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistoryDirect.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistoryDirect.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistoryDirect.RootTable.Columns["Print"].Width = 50;
			DataGridHistoryDirect.RootTable.Columns["Print"].Position = 0;
			DataGridHistoryDirect.RootTable.Columns.Add("Voucher");
			DataGridHistoryDirect.RootTable.Columns["Voucher"].Caption = "Voucher";
			DataGridHistoryDirect.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistoryDirect.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DataGridHistoryDirect.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DataGridHistoryDirect.RootTable.Columns["Voucher"].Width = 60;
			DataGridHistoryDirect.RootTable.Columns["Voucher"].Position = 1;
			DataGridHistoryDirect.FrozenColumns = 2;
			ctrlGrdBar4_Load(null, null);
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			gridHisory();
			DirectRegisterHistory();
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
			if (dtInvoice.Rows.Count == 0 || dtInvoice == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtInvoice, e.ClickedItem.Text + ".rpt");
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
			txtInvoiceNoFrom.Text = string.Empty;
			txtInvoiceNoto.Text = string.Empty;
			txtratefrom.Text = string.Empty;
			txtrateto.Text = string.Empty;
			txtsaleorderfrom.Text = string.Empty;
			txtsaleorderto.Text = string.Empty;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)cmbWareHouse).Text = string.Empty;
			((Control)(object)cmbitem).Text = string.Empty;
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			ToDate.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				CommonServices.SaleInvoiceSteelSlip_1514(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["VoucherHeadId"].Value), Conversion.ToInt(DataGridHistory.CurrentRow.Cells["DocumentTypeId"].Value));
			}
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
			if (e.Column.Key == "CustomerName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["SupplierCustomerId"].Value), FromDate.Value, ToDate.Value);
			}
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["SaleGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), FromDate.Value, ToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtInvoiceNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtInvoiceNoto_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsaleorderfrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsaleorderto_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtratefrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
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
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistoryDirect_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistoryDirect.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Print")
				{
					CommonServices.SaleInvoiceDirectSteelSlip_1516(Conversion.ToInt(r.Cells["Id"].Value));
				}
				if (e.Column.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(r.Cells["VoucherHeadId"].Value), Conversion.ToInt(r.Cells["DocumentTypeId"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistoryDirect_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistoryDirect.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "CustomerName")
				{
					CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["SupplierCustomerId"].Value), FromDate.Value, ToDate.Value);
				}
				if (e.Column.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["SaleGLAC"].Value);
					CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["ItemId"].Value), FromDate.Value, ToDate.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tsDropDownForDirect_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtInvoiceDirect.Rows.Count == 0 || dtInvoiceDirect == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtInvoiceDirect, e.ClickedItem.Text + ".rpt");
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
			GetDataForSaleInvoiceRegisterDropdownBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistoryDirect).Name);
			if (fs != null)
			{
				DataGridHistoryDirect.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void SaleInvoiceRegisterSteel_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_085e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_0c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c93: Expected O, but got Unknown
		//IL_0ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caa: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistoryDirect_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridHistory = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridHistoryDirect = new GridEX();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label13 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbWareHouse = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.txtsaleorderto = new System.Windows.Forms.TextBox();
		this.txtsaleorderfrom = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtrateto = new System.Windows.Forms.TextBox();
		this.txtratefrom = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbIsApproved = new System.Windows.Forms.ComboBox();
		this.cmbitem = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtInvoiceNoto = new System.Windows.Forms.TextBox();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtInvoiceNoFrom = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.tsDropDown = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsDropDownForDirect = new System.Windows.Forms.ToolStripDropDownButton();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDirect).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(1044, 681);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add(this.tabControl1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 181);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1044, 500);
		this.panel5.TabIndex = 3;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1044, 500);
		this.tabControl1.TabIndex = 4;
		this.tabPage1.Controls.Add(this.ctrlGrdBar3);
		this.tabPage1.Controls.Add(this.ctrlGrdBar1);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.label12);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1036, 471);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Flow Register";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(992, 6);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 12;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1030, 438);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1158, 6);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(44, 21);
		this.ctrlGrdBar1.TabIndex = 11;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 30);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1030, 438);
		this.panel6.TabIndex = 2;
		this.label12.BackColor = System.Drawing.Color.Teal;
		this.label12.Dock = System.Windows.Forms.DockStyle.Top;
		this.label12.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(3, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(1030, 27);
		this.label12.TabIndex = 1;
		this.label12.Text = "Sale Invoice Register";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage2.Controls.Add(this.ctrlGrdBar4);
		this.tabPage2.Controls.Add(this.ctrlGrdBar2);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDirect);
		this.tabPage2.Controls.Add(this.label13);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1036, 471);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Direct Register";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(992, 6);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar4.MyGrid = this.DataGridHistoryDirect;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 208;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.DataGridHistoryDirect.AllowDelete = (InheritableBoolean)1;
		DataGridHistoryDirect_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryDirect.DesignTimeLayout = DataGridHistoryDirect_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDirect).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistoryDirect.DynamicFiltering = true;
		this.DataGridHistoryDirect.FilterMode = (FilterMode)2;
		this.DataGridHistoryDirect.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDirect).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryDirect.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDirect).Location = new System.Drawing.Point(3, 31);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDirect).Name = "DataGridHistoryDirect";
		((ScrollableBase)this.DataGridHistoryDirect).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDirect).Size = new System.Drawing.Size(1030, 437);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDirect).TabIndex = 3;
		this.DataGridHistoryDirect.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDirect.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDirect).VisualStyle = (VisualStyle)4;
		this.DataGridHistoryDirect.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistoryDirect_ColumnButtonClick);
		this.DataGridHistoryDirect.LinkClicked += new ColumnActionEventHandler(DataGridHistoryDirect_LinkClicked);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1158, 5);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistoryDirect;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(44, 21);
		this.ctrlGrdBar2.TabIndex = 208;
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Dock = System.Windows.Forms.DockStyle.Top;
		this.label13.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(3, 3);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(1030, 28);
		this.label13.TabIndex = 2;
		this.label13.Text = "Sale Invoice Register Direct";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouse);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtsaleorderto);
		this.panel4.Controls.Add(this.txtsaleorderfrom);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtrateto);
		this.panel4.Controls.Add(this.txtratefrom);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.CmbIsApproved);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.txtInvoiceNoto);
		this.panel4.Controls.Add(this.ToDate);
		this.panel4.Controls.Add(this.FromDate);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Controls.Add(this.txtInvoiceNoFrom);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.panel4.Location = new System.Drawing.Point(0, 58);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1044, 123);
		this.panel4.TabIndex = 2;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(432, 97);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(43, 15);
		this.label11.TabIndex = 207;
		this.label11.Text = "JobLot";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(538, 91);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 11;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(432, 69);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(69, 15);
		this.label10.TabIndex = 205;
		this.label10.Text = "WareHouse";
		this.cmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Location = new System.Drawing.Point(538, 63);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Name = "cmbWareHouse";
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).TabIndex = 10;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(230, 41);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 15);
		this.label8.TabIndex = 203;
		this.label8.Text = "Sale Order To";
		this.txtsaleorderto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsaleorderto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsaleorderto.Location = new System.Drawing.Point(327, 37);
		this.txtsaleorderto.Name = "txtsaleorderto";
		this.txtsaleorderto.Size = new System.Drawing.Size(95, 23);
		this.txtsaleorderto.TabIndex = 5;
		this.txtsaleorderto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsaleorderto_KeyPress);
		this.txtsaleorderfrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsaleorderfrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsaleorderfrom.Location = new System.Drawing.Point(327, 9);
		this.txtsaleorderfrom.Name = "txtsaleorderfrom";
		this.txtsaleorderfrom.Size = new System.Drawing.Size(95, 23);
		this.txtsaleorderfrom.TabIndex = 4;
		this.txtsaleorderfrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsaleorderfrom_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(230, 13);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(93, 15);
		this.label9.TabIndex = 202;
		this.label9.Text = "Sale Order From";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(230, 97);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 15);
		this.label3.TabIndex = 199;
		this.label3.Text = "Rate To";
		this.txtrateto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtrateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtrateto.Location = new System.Drawing.Point(327, 93);
		this.txtrateto.Name = "txtrateto";
		this.txtrateto.Size = new System.Drawing.Size(95, 23);
		this.txtrateto.TabIndex = 7;
		this.txtrateto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtrateto_KeyPress);
		this.txtratefrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtratefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtratefrom.Location = new System.Drawing.Point(327, 65);
		this.txtratefrom.Name = "txtratefrom";
		this.txtratefrom.Size = new System.Drawing.Size(95, 23);
		this.txtratefrom.TabIndex = 6;
		this.txtratefrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtratefrom_KeyPress);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(230, 69);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(64, 15);
		this.label5.TabIndex = 198;
		this.label5.Text = "Rate From ";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(751, 91);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(67, 27);
		this.btnshow.TabIndex = 12;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(1044, 10);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(41, 15);
		this.label7.TabIndex = 195;
		this.label7.Text = "Status";
		this.label7.Visible = false;
		this.CmbIsApproved.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbIsApproved.FormattingEnabled = true;
		this.CmbIsApproved.Items.AddRange(new object[2] { "Yes ", "No" });
		this.CmbIsApproved.Location = new System.Drawing.Point(1101, 5);
		this.CmbIsApproved.Name = "CmbIsApproved";
		this.CmbIsApproved.Size = new System.Drawing.Size(162, 25);
		this.CmbIsApproved.TabIndex = 183;
		this.CmbIsApproved.Visible = false;
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(538, 35);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 9;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(432, 41);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 194;
		this.label6.Text = "Item Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(13, 97);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(78, 15);
		this.label4.TabIndex = 191;
		this.label4.Text = "InvoiceNo To";
		this.txtInvoiceNoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceNoto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNoto.Location = new System.Drawing.Point(108, 93);
		this.txtInvoiceNoto.Name = "txtInvoiceNoto";
		this.txtInvoiceNoto.Size = new System.Drawing.Size(116, 23);
		this.txtInvoiceNoto.TabIndex = 3;
		this.txtInvoiceNoto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtInvoiceNoto_KeyPress);
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(108, 37);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(116, 23);
		this.ToDate.TabIndex = 1;
		this.FromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(108, 9);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(116, 23);
		this.FromDate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(13, 13);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 190;
		this.label27.Text = "From Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(13, 41);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 189;
		this.label33.Text = "To Date";
		this.txtInvoiceNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNoFrom.Location = new System.Drawing.Point(108, 65);
		this.txtInvoiceNoFrom.Name = "txtInvoiceNoFrom";
		this.txtInvoiceNoFrom.Size = new System.Drawing.Size(116, 23);
		this.txtInvoiceNoFrom.TabIndex = 2;
		this.txtInvoiceNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtInvoiceNoFrom_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(13, 69);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 188;
		this.label2.Text = "InvoiceNo From";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(538, 7);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 8;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(432, 13);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 15);
		this.label1.TabIndex = 185;
		this.label1.Text = "Customer Name";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1044, 30);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(7, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(171, 19);
		this.label23.TabIndex = 6;
		this.label23.Text = "Sale Invoice History";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1044, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.tsDropDown, this.tsDropDownForDirect });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1044, 27);
		this.toolStrip3.TabIndex = 10;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(70, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.tsDropDown.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDown.Name = "tsDropDown";
		this.tsDropDown.Size = new System.Drawing.Size(113, 24);
		this.tsDropDown.Text = "Print For Flow";
		this.tsDropDown.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDown_DropDownItemClicked);
		this.tsDropDownForDirect.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDownForDirect.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDownForDirect.Name = "tsDropDownForDirect";
		this.tsDropDownForDirect.Size = new System.Drawing.Size(122, 24);
		this.tsDropDownForDirect.Text = "Print For Direct ";
		this.tsDropDownForDirect.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDownForDirect_DropDownItemClicked);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1044, 681);
		base.Controls.Add(this.panel1);
		base.Name = "SaleInvoiceRegisterSteel";
		this.Text = "Sale Invoice Register";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SaleInvoiceRegisterSteel_FormClosing);
		base.Load += new System.EventHandler(frmSaleInvoiceRegister_Load);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDirect).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
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
