using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Inventory_Reports;

public class OrdersWithLedgerBalance : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private DataTable dtGridCurrLedger = new DataTable();

	private DataTable dtPartyWiseSummary = new DataTable();

	private IContainer components;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Button btnshow;

	private DateTimePicker txtToDate;

	private DateTimePicker txtFromDate;

	private Label label33;

	private Label label27;

	internal UltraCombo CmbSupplierCustomer;

	private Label label29;

	private ToolStripButton btnRefresh;

	internal GridEX grd;

	internal UltraCombo cmbperemeter;

	private Label label8;

	private ToolStripButton btnPrint;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel2;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel4;

	private Panel panel7;

	private Panel panel6;

	private Panel panel5;

	private Label label1;

	private TabPage tabPage2;

	private Panel panel8;

	internal GridEX grdCurrLedger;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel10;

	internal UltraCombo cmbCustomerCurrLedger;

	private Button btnShowCurrLedger;

	internal UltraCombo cmbperemeterCurrLedger;

	private Label label2;

	private Label label3;

	private Label label4;

	private DateTimePicker txtToDateCurrLedger;

	private DateTimePicker txtFromDateCurrLedger;

	private Label label5;

	private Panel panel11;

	private Label label6;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNewCurrLedger;

	private ToolStripButton btnRefreshcurrLedger;

	private ToolStripButton btnPrintcurrLedger;

	private TabPage tabPage3;

	private Panel panel1;

	private Label label7;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewPartyWiseTab;

	private ToolStripButton btnPrintPartyWiseTab;

	private Panel panel3;

	private Button btnShowPartyWiseTab;

	internal UltraCombo CmbDateTypePartyWiseTab;

	private Label label10;

	private Label label11;

	private DateTimePicker ToDatePartyWiseTab;

	private DateTimePicker FromDatePartyWiseTab;

	private Label label12;

	private GroupBox groupBox1;

	internal GridEX grdPartyWise;

	private CtrlGrdBar ctrlGrdBar3;

	public OrdersWithLedgerBalance(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void OrdersWithLedgerBalance_Load(object sender, EventArgs e)
	{
		try
		{
			suppliercustomer();
			DateTypeFill();
			cmbperemeter.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DateTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Parameters");
			dt.Rows.Add(1, "This Day");
			dt.Rows.Add(2, "This Week");
			dt.Rows.Add(3, "This Month");
			dt.Rows.Add(4, "This Year");
			dt.Rows.Add(5, "Financial Year");
			dt.Rows.Add(6, "All");
			DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
			((UltraGridBase)cmbperemeter).Rows[5].Activate();
			DDL.BindDDL(dt, cmbperemeterCurrLedger, "Id", "Parameters", "Parameters", ZeroIndex: false);
			((UltraGridBase)cmbperemeterCurrLedger).Rows[5].Activate();
			DDL.BindDDL(dt, CmbDateTypePartyWiseTab, "Id", "Parameters", "Date Type", ZeroIndex: false);
			((UltraGridBase)CmbDateTypePartyWiseTab).Rows[5].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void suppliercustomer()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SupplierCustomer.SupplierCustomerAgainstSaleOrder(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbSupplierCustomer, "Id", "CompanyName", "Customer Name", false);
				((UltraGridBase)CmbSupplierCustomer).DisplayLayout.Bands[0].Columns[1].Width = 350;
				DDL.BindDDLNew(dt, cmbCustomerCurrLedger, "Id", "CompanyName", "Customer Name", false);
				((UltraGridBase)cmbCustomerCurrLedger).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
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
			suppliercustomer();
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
			cmbperemeter.Focus();
			((UltraGridBase)cmbperemeter).Rows[0].Activate();
			cmbperemeter_ValueChanged(null, null);
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
				txtFromDate.Value = DateTime.Today;
				GetLedgerReport();
			}
			if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				txtFromDate.Value = DateTime.Today.AddDays(-7.0);
				GetLedgerReport();
			}
			if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first2 = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromDate.Value = first2;
				txtToDate.Value = DateTime.Today;
				GetLedgerReport();
			}
			if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first = new DateTime(year, 1, 1);
				txtFromDate.Value = first;
				txtToDate.Value = DateTime.Today;
				GetLedgerReport();
			}
			if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				txtFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				GetLedgerReport();
			}
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
			GetLedgerReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetLedgerReport()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbSupplierCustomer).ActiveRow != null)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.SupplierCustomerId = Conversion.ToInt(CmbSupplierCustomer.Value);
				obj.ActionId = 1;
				if (Conversion.ToInt(cmbperemeter.Value) != 6)
				{
					obj.FromDate = txtFromDate.Value;
					obj.ToDate = txtToDate.Value;
				}
				DataTable dt = SaleOrder.GetOutstandingOrdersWithLedgerBalance(obj);
				if (dt.Rows.Count > 0)
				{
					dtGrid.Rows.Clear();
					if (dtGrid.Columns.Count == 0)
					{
						dtGrid.Columns.Add("Id");
						dtGrid.Columns.Add("DocDate", typeof(DateTime));
						dtGrid.Columns.Add("DocNo", typeof(int));
						dtGrid.Columns.Add("SuppCustId");
						dtGrid.Columns.Add("CustomerName");
						dtGrid.Columns.Add("OrderQty", typeof(double));
						dtGrid.Columns.Add("DispatchQty", typeof(double));
						dtGrid.Columns.Add("BalQty", typeof(double));
						dtGrid.Columns.Add("OrderWeight", typeof(double));
						dtGrid.Columns.Add("DispatchWeight", typeof(double));
						dtGrid.Columns.Add("BalWeight", typeof(double));
						dtGrid.Columns.Add("OrderAmount", typeof(double));
						dtGrid.Columns.Add("DispatchAmount", typeof(double));
						dtGrid.Columns.Add("BalAmount", typeof(double));
						dtGrid.Columns.Add("LedgerBalance", typeof(double));
						dtGrid.Columns.Add("RunningBalance", typeof(double));
						dtGrid.Columns.Add("Remarks");
						dtGrid.Columns.Add("CustomerGlId");
					}
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtGrid.Rows.Add(dt.Rows[i]["Id"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), dt.Rows[i]["DocNo"], dt.Rows[i]["OrderSupCustId"], dt.Rows[i]["CustomerName"], dt.Rows[i]["OrderQty"], dt.Rows[i]["DispatchQty"], dt.Rows[i]["BalQty"], dt.Rows[i]["OrderWeight"], dt.Rows[i]["DispatchWeight"], dt.Rows[i]["BalWeight"], dt.Rows[i]["OrderAmount"], dt.Rows[i]["DispatchAmount"], dt.Rows[i]["OrderBalAmount"], dt.Rows[i]["LedgerBalance"], dt.Rows[i]["RunningBalance"], dt.Rows[i]["RemarksHeader"], dt.Rows[i]["CustomerGlId"]);
					}
					grd.DataSource = dtGrid;
					grd.RetrieveStructure();
					GridSetting();
				}
				else
				{
					dtGrid.Rows.Clear();
					grd.ClearStructure();
				}
			}
			else
			{
				dtGrid.Rows.Clear();
				grd.ClearStructure();
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
			grd.RootTable.Columns["CustomerGlId"].Visible = false;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["SuppCustId"].Visible = false;
			grd.RootTable.Columns["DocDate"].Width = 80;
			grd.RootTable.Columns["DocNo"].Width = 60;
			grd.RootTable.Columns["CustomerName"].Width = 200;
			grd.RootTable.Columns["CustomerName"].Caption = "PartyName";
			grd.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["OrderQty"].Width = 80;
			grd.RootTable.Columns["DispatchQty"].Width = 80;
			grd.RootTable.Columns["BalQty"].Width = 80;
			grd.RootTable.Columns["OrderWeight"].Width = 90;
			grd.RootTable.Columns["DispatchWeight"].Width = 90;
			grd.RootTable.Columns["BalWeight"].Width = 80;
			grd.RootTable.Columns["OrderAmount"].Width = 90;
			grd.RootTable.Columns["DispatchAmount"].Width = 90;
			grd.RootTable.Columns["BalAmount"].Width = 80;
			grd.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OrderQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["OrderQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DispatchQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["DispatchQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OrderWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["OrderWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["DispatchWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DispatchWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DispatchWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["DispatchWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OrderAmount"].FormatString = "#,##0.##";
			grd.RootTable.Columns["OrderAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["DispatchAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DispatchAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DispatchAmount"].FormatString = "#,##0.##";
			grd.RootTable.Columns["DispatchAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalAmount"].FormatString = "#,##0.##";
			grd.RootTable.Columns["BalAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["LedgerBalance"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LedgerBalance"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["RunningBalance"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RunningBalance"].FormatString = "#,#;(#,#);0";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "CustomerName")
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["CustomerGlId"].Value), clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
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
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtGrid, "146-OutstandingOrderWithLedgerBalance.rpt");
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

	private void btnRefreshcurrLedger_Click(object sender, EventArgs e)
	{
		try
		{
			suppliercustomer();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewCurrLedger_Click(object sender, EventArgs e)
	{
		try
		{
			cmbperemeterCurrLedger.Focus();
			((UltraGridBase)cmbperemeterCurrLedger).Rows[0].Activate();
			cmbperemeterCurrLedger_ValueChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbperemeterCurrLedger_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeterCurrLedger.Value) == 1)
			{
				txtFromDateCurrLedger.Value = DateTime.Today;
				GetCurrentLedgerReport();
			}
			if (Convert.ToInt32(cmbperemeterCurrLedger.Value) == 2)
			{
				txtFromDateCurrLedger.Value = DateTime.Today.AddDays(-7.0);
				GetCurrentLedgerReport();
			}
			if (Convert.ToInt32(cmbperemeterCurrLedger.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first2 = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromDateCurrLedger.Value = first2;
				txtToDateCurrLedger.Value = DateTime.Today;
				GetCurrentLedgerReport();
			}
			if (Convert.ToInt32(cmbperemeterCurrLedger.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first = new DateTime(year, 1, 1);
				txtFromDateCurrLedger.Value = first;
				txtToDateCurrLedger.Value = DateTime.Today;
				GetCurrentLedgerReport();
			}
			if (Convert.ToInt32(cmbperemeterCurrLedger.Value) == 5)
			{
				txtFromDateCurrLedger.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				GetCurrentLedgerReport();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowCurrLedger_Click(object sender, EventArgs e)
	{
		try
		{
			GetCurrentLedgerReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetCurrentLedgerReport()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbSupplierCustomer).ActiveRow != null)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.SupplierCustomerId = Conversion.ToInt(cmbCustomerCurrLedger.Value);
				obj.ActionId = 2;
				if (Conversion.ToInt(cmbperemeterCurrLedger.Value) != 6)
				{
					obj.FromDate = txtFromDateCurrLedger.Value;
					obj.ToDate = txtToDateCurrLedger.Value;
				}
				DataTable dt = SaleOrder.GetOutstandingOrdersWithLedgerBalance(obj);
				if (dt.Rows.Count > 0)
				{
					dtGridCurrLedger.Rows.Clear();
					if (dtGridCurrLedger.Columns.Count == 0)
					{
						dtGridCurrLedger.Columns.Add("Id");
						dtGridCurrLedger.Columns.Add("DocDate", typeof(DateTime));
						dtGridCurrLedger.Columns.Add("DocNo", typeof(int));
						dtGridCurrLedger.Columns.Add("SuppCustId");
						dtGridCurrLedger.Columns.Add("CustomerName");
						dtGridCurrLedger.Columns.Add("OrderQty", typeof(double));
						dtGridCurrLedger.Columns.Add("DispatchQty", typeof(double));
						dtGridCurrLedger.Columns.Add("BalQty", typeof(double));
						dtGridCurrLedger.Columns.Add("OrderWeight", typeof(double));
						dtGridCurrLedger.Columns.Add("DispatchWeight", typeof(double));
						dtGridCurrLedger.Columns.Add("BalWeight", typeof(double));
						dtGridCurrLedger.Columns.Add("OrderAmount", typeof(double));
						dtGridCurrLedger.Columns.Add("DispatchAmount", typeof(double));
						dtGridCurrLedger.Columns.Add("BalAmount", typeof(double));
						dtGridCurrLedger.Columns.Add("LedgerBalance", typeof(double));
						dtGridCurrLedger.Columns.Add("RunningBalance", typeof(double));
						dtGridCurrLedger.Columns.Add("Remarks");
						dtGridCurrLedger.Columns.Add("CustomerGlId");
					}
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtGridCurrLedger.Rows.Add(dt.Rows[i]["Id"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), dt.Rows[i]["DocNo"], dt.Rows[i]["OrderSupCustId"], dt.Rows[i]["CustomerName"], dt.Rows[i]["OrderQty"], dt.Rows[i]["DispatchQty"], dt.Rows[i]["BalQty"], dt.Rows[i]["OrderWeight"], dt.Rows[i]["DispatchWeight"], dt.Rows[i]["BalWeight"], dt.Rows[i]["OrderAmount"], dt.Rows[i]["DispatchAmount"], dt.Rows[i]["OrderBalAmount"], dt.Rows[i]["LedgerBalance"], dt.Rows[i]["RunningBalance"], dt.Rows[i]["RemarksHeader"], dt.Rows[i]["CustomerGlId"]);
					}
					grdCurrLedger.DataSource = dtGridCurrLedger;
					grdCurrLedger.RetrieveStructure();
					GridCurrLedgerSetting();
				}
				else
				{
					dtGridCurrLedger.Rows.Clear();
					grdCurrLedger.ClearStructure();
				}
			}
			else
			{
				dtGridCurrLedger.Rows.Clear();
				grdCurrLedger.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridCurrLedgerSetting()
	{
		try
		{
			grdCurrLedger.RootTable.Columns["CustomerGlId"].Visible = false;
			grdCurrLedger.RootTable.Columns["Id"].Visible = false;
			grdCurrLedger.RootTable.Columns["SuppCustId"].Visible = false;
			grdCurrLedger.RootTable.Columns["CustomerName"].Caption = "PartyName";
			grdCurrLedger.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			grdCurrLedger.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			grdCurrLedger.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			grdCurrLedger.RootTable.Columns["OrderQty"].FormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["OrderQty"].TotalFormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			grdCurrLedger.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			grdCurrLedger.RootTable.Columns["DispatchQty"].FormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["DispatchQty"].TotalFormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdCurrLedger.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdCurrLedger.RootTable.Columns["BalQty"].FormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			grdCurrLedger.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			grdCurrLedger.RootTable.Columns["OrderWeight"].FormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["OrderWeight"].TotalFormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["DispatchWeight"].AggregateFunction = (AggregateFunction)2;
			grdCurrLedger.RootTable.Columns["DispatchWeight"].TextAlignment = (TextAlignment)3;
			grdCurrLedger.RootTable.Columns["DispatchWeight"].FormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["DispatchWeight"].TotalFormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grdCurrLedger.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grdCurrLedger.RootTable.Columns["BalWeight"].FormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			grdCurrLedger.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			grdCurrLedger.RootTable.Columns["OrderAmount"].FormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["OrderAmount"].TotalFormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["DispatchAmount"].AggregateFunction = (AggregateFunction)2;
			grdCurrLedger.RootTable.Columns["DispatchAmount"].TextAlignment = (TextAlignment)3;
			grdCurrLedger.RootTable.Columns["DispatchAmount"].FormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["DispatchAmount"].TotalFormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			grdCurrLedger.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			grdCurrLedger.RootTable.Columns["BalAmount"].FormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["BalAmount"].TotalFormatString = "#,##0.##";
			grdCurrLedger.RootTable.Columns["LedgerBalance"].TextAlignment = (TextAlignment)3;
			grdCurrLedger.RootTable.Columns["LedgerBalance"].FormatString = "#,#;(#,#);0";
			grdCurrLedger.RootTable.Columns["RunningBalance"].TextAlignment = (TextAlignment)3;
			grdCurrLedger.RootTable.Columns["RunningBalance"].FormatString = "#,#;(#,#);0";
			CommonServices.GridAutoAdjustmentNew(grdCurrLedger);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCurrLedger_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "CustomerName")
			{
				GridEXRow item = grdCurrLedger.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["CustomerGlId"].Value), clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintcurrLedger_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGridCurrLedger.Rows.Count == 0 || dtGridCurrLedger == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtGridCurrLedger, "146-OutstandingOrderWithLedgerBalance.rpt");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdCurrLedger).Name);
			if (fs != null)
			{
				grdCurrLedger.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrdersWithLedgerBalance_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnNewPartyWiseTab_Click(object sender, EventArgs e)
	{
		try
		{
			((UltraGridBase)CmbDateTypePartyWiseTab).Rows[0].Activate();
			grdPartyWise.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintPartyWiseTab_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtPartyWiseSummary.Rows.Count == 0 || dtPartyWiseSummary == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtPartyWiseSummary, "199-GetOutstandingOrdersWithLedgerBalance.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowPartyWiseTab_Click(object sender, EventArgs e)
	{
		try
		{
			PartyWiseSummary();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyWiseSummary()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtPartyWiseSummary.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (Conversion.ToInt(CmbDateTypePartyWiseTab.Value) != 6)
			{
				obj.FromDate = FromDatePartyWiseTab.Value;
				obj.ToDate = ToDatePartyWiseTab.Value;
			}
			obj.ActionId = 3;
			dtPartyWiseSummary = SaleOrder.GetOutstandingOrdersWithLedgerBalance(obj);
			if (dtPartyWiseSummary.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("GlAccountId");
				dt.Columns.Add("SuppCustId");
				dt.Columns.Add("CustomerName");
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("LedgerBalance", typeof(double));
				dt.Columns.Add("Balance", typeof(double));
				for (int i = 0; i < dtPartyWiseSummary.Rows.Count; i++)
				{
					dt.Rows.Add(dtPartyWiseSummary.Rows[i]["GlAccountId"], dtPartyWiseSummary.Rows[i]["OrderSupCustId"], dtPartyWiseSummary.Rows[i]["CustomerName"], dtPartyWiseSummary.Rows[i]["BalQty"], dtPartyWiseSummary.Rows[i]["BalWeight"], dtPartyWiseSummary.Rows[i]["OrderBalAmount"], dtPartyWiseSummary.Rows[i]["LedgerBalance"], dtPartyWiseSummary.Rows[i]["RunningBalance"]);
				}
				grdPartyWise.DataSource = dt;
				grdPartyWise.RetrieveStructure();
				GridSettingPartyWiseTab();
			}
			else
			{
				grdPartyWise.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingPartyWiseTab()
	{
		try
		{
			grdPartyWise.RootTable.Columns["SuppCustId"].Visible = false;
			grdPartyWise.RootTable.Columns["GlAccountId"].Visible = false;
			grdPartyWise.RootTable.Columns["CustomerName"].Caption = "PartyName";
			grdPartyWise.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			grdPartyWise.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdPartyWise.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdPartyWise.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdPartyWise.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdPartyWise.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdPartyWise.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdPartyWise.RootTable.Columns["Weight"].FormatString = "#,##0.####";
			grdPartyWise.RootTable.Columns["Weight"].TotalFormatString = "#,##0.####";
			grdPartyWise.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPartyWise.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPartyWise.RootTable.Columns["Amount"].FormatString = "#,##0.####";
			grdPartyWise.RootTable.Columns["Amount"].TotalFormatString = "#,##0.####";
			grdPartyWise.RootTable.Columns["LedgerBalance"].TextAlignment = (TextAlignment)3;
			grdPartyWise.RootTable.Columns["LedgerBalance"].FormatString = "#,#;(#,#);0";
			grdPartyWise.RootTable.Columns["Balance"].TextAlignment = (TextAlignment)3;
			grdPartyWise.RootTable.Columns["Balance"].FormatString = "#,#;(#,#);0";
			CommonServices.GridAutoAdjustmentNew(grdPartyWise);
			ctrlGrdBar3_Load(null, null);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPartyWise).Name);
			if (fs != null)
			{
				grdPartyWise.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDateTypePartyWiseTab_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbDateTypePartyWiseTab.Value) == 1)
			{
				FromDatePartyWiseTab.Value = DateTime.Now;
				ToDatePartyWiseTab.Value = DateTime.Now;
			}
			if (Conversion.ToInt(CmbDateTypePartyWiseTab.Value) == 2)
			{
				FromDatePartyWiseTab.Value = DateTime.Today.AddDays(-7.0);
			}
			if (Conversion.ToInt(CmbDateTypePartyWiseTab.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first2 = now.Date.AddDays(-(now.Date.Day - 1));
				FromDatePartyWiseTab.Value = first2;
				ToDatePartyWiseTab.Value = DateTime.Today;
			}
			if (Conversion.ToInt(CmbDateTypePartyWiseTab.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first = new DateTime(year, 1, 1);
				FromDatePartyWiseTab.Value = first;
				ToDatePartyWiseTab.Value = DateTime.Today;
			}
			if (Conversion.ToInt(CmbDateTypePartyWiseTab.Value) == 5)
			{
				FromDatePartyWiseTab.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPartyWise_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "CustomerName")
			{
				GridEXRow item = grdPartyWise.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["GlAccountId"].Value), Conversion.ToDateTime((object)FromDatePartyWiseTab.Text), Conversion.ToDateTime((object)ToDatePartyWiseTab.Text));
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
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Expected O, but got Unknown
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Expected O, but got Unknown
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Expected O, but got Unknown
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Expected O, but got Unknown
		//IL_1cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd3: Expected O, but got Unknown
		//IL_30aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b4: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout grdCurrLedger_DesignTimeLayout = new GridEXLayout();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		GridEXLayout grdPartyWise_DesignTimeLayout = new GridEXLayout();
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
		this.grd = new GridEX();
		this.cmbperemeter = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbSupplierCustomer = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdCurrLedger = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel10 = new System.Windows.Forms.Panel();
		this.cmbCustomerCurrLedger = new UltraCombo();
		this.btnShowCurrLedger = new System.Windows.Forms.Button();
		this.cmbperemeterCurrLedger = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtToDateCurrLedger = new System.Windows.Forms.DateTimePicker();
		this.txtFromDateCurrLedger = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewCurrLedger = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshcurrLedger = new System.Windows.Forms.ToolStripButton();
		this.btnPrintcurrLedger = new System.Windows.Forms.ToolStripButton();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdPartyWise = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.FromDatePartyWiseTab = new System.Windows.Forms.DateTimePicker();
		this.btnShowPartyWiseTab = new System.Windows.Forms.Button();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbDateTypePartyWiseTab = new UltraCombo();
		this.ToDatePartyWiseTab = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label7 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewPartyWiseTab = new System.Windows.Forms.ToolStripButton();
		this.btnPrintPartyWiseTab = new System.Windows.Forms.ToolStripButton();
		this.panel2 = new System.Windows.Forms.Panel();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).BeginInit();
		this.toolStrip3.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCurrLedger).BeginInit();
		this.panel9.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerCurrLedger).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeterCurrLedger).BeginInit();
		this.panel11.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPartyWise).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypePartyWiseTab).BeginInit();
		this.panel1.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 179);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1005, 379);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(80, 6);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(15, 12);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(60, 15);
		this.label8.TabIndex = 115;
		this.label8.Text = "Date Type";
		this.CmbSupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Location = new System.Drawing.Point(328, 6);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Name = "CmbSupplierCustomer";
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Size = new System.Drawing.Size(293, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).TabIndex = 3;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(229, 12);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(93, 15);
		this.label29.TabIndex = 113;
		this.label29.Text = "Customer Name";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(563, 37);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 23);
		this.btnshow.TabIndex = 4;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.CustomFormat = "dd-MMM-yy";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDate.Location = new System.Drawing.Point(80, 65);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(143, 23);
		this.txtToDate.TabIndex = 2;
		this.txtFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.CustomFormat = "dd-MMM-yy";
		this.txtFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDate.Location = new System.Drawing.Point(80, 37);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(143, 23);
		this.txtFromDate.TabIndex = 1;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(15, 69);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 110;
		this.label33.Text = "To Date";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(15, 41);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 111;
		this.label27.Text = "From Date";
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.btnRefresh, this.btnPrint });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1005, 27);
		this.toolStrip3.TabIndex = 2;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.ForeColor = System.Drawing.Color.Black;
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(58, 24);
		this.btnNew.Text = "New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(84, 24);
		this.btnPrint.Text = "Print-146";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1019, 590);
		this.tabControl1.TabIndex = 1;
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1011, 564);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Date Wise Legder Balance";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1005, 558);
		this.panel4.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 149);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1005, 30);
		this.panel7.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(956, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer);
		this.panel6.Controls.Add(this.btnshow);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.panel6.Controls.Add(this.label29);
		this.panel6.Controls.Add(this.label8);
		this.panel6.Controls.Add(this.label27);
		this.panel6.Controls.Add(this.txtToDate);
		this.panel6.Controls.Add(this.txtFromDate);
		this.panel6.Controls.Add(this.label33);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 57);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1005, 92);
		this.panel6.TabIndex = 1;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.toolStrip3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1005, 57);
		this.panel5.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 33);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(236, 19);
		this.label1.TabIndex = 116;
		this.label1.Text = "Orders With Ledger Balance";
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1011, 564);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Current Legder Balance";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdCurrLedger);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Controls.Add(this.panel10);
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1005, 558);
		this.panel8.TabIndex = 1;
		this.grdCurrLedger.AllowDelete = (InheritableBoolean)1;
		this.grdCurrLedger.AllowEdit = (InheritableBoolean)2;
		this.grdCurrLedger.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdCurrLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCurrLedger.DesignTimeLayout = grdCurrLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCurrLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdCurrLedger.DynamicFiltering = true;
		this.grdCurrLedger.FilterMode = (FilterMode)2;
		this.grdCurrLedger.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdCurrLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCurrLedger.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdCurrLedger).Location = new System.Drawing.Point(0, 179);
		((System.Windows.Forms.Control)(object)this.grdCurrLedger).Name = "grdCurrLedger";
		((ScrollableBase)this.grdCurrLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCurrLedger).Size = new System.Drawing.Size(1005, 379);
		((System.Windows.Forms.Control)(object)this.grdCurrLedger).TabIndex = 0;
		this.grdCurrLedger.TotalRow = (InheritableBoolean)1;
		this.grdCurrLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCurrLedger).VisualStyle = (VisualStyle)4;
		this.grdCurrLedger.LinkClicked += new ColumnActionEventHandler(grdCurrLedger_LinkClicked);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 149);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1005, 30);
		this.panel9.TabIndex = 2;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(956, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdCurrLedger;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomerCurrLedger);
		this.panel10.Controls.Add(this.btnShowCurrLedger);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeterCurrLedger);
		this.panel10.Controls.Add(this.label2);
		this.panel10.Controls.Add(this.label3);
		this.panel10.Controls.Add(this.label4);
		this.panel10.Controls.Add(this.txtToDateCurrLedger);
		this.panel10.Controls.Add(this.txtFromDateCurrLedger);
		this.panel10.Controls.Add(this.label5);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 57);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1005, 92);
		this.panel10.TabIndex = 1;
		this.cmbCustomerCurrLedger.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomerCurrLedger.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomerCurrLedger.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomerCurrLedger.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomerCurrLedger).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomerCurrLedger).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomerCurrLedger.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomerCurrLedger).Location = new System.Drawing.Point(328, 6);
		((System.Windows.Forms.Control)(object)this.cmbCustomerCurrLedger).Name = "cmbCustomerCurrLedger";
		((System.Windows.Forms.Control)(object)this.cmbCustomerCurrLedger).Size = new System.Drawing.Size(293, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomerCurrLedger).TabIndex = 3;
		this.btnShowCurrLedger.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowCurrLedger.FlatAppearance.BorderSize = 0;
		this.btnShowCurrLedger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowCurrLedger.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowCurrLedger.ForeColor = System.Drawing.Color.White;
		this.btnShowCurrLedger.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowCurrLedger.Location = new System.Drawing.Point(563, 37);
		this.btnShowCurrLedger.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowCurrLedger.Name = "btnShowCurrLedger";
		this.btnShowCurrLedger.Size = new System.Drawing.Size(58, 23);
		this.btnShowCurrLedger.TabIndex = 4;
		this.btnShowCurrLedger.Text = "Show";
		this.btnShowCurrLedger.UseVisualStyleBackColor = false;
		this.btnShowCurrLedger.Click += new System.EventHandler(btnShowCurrLedger_Click);
		this.cmbperemeterCurrLedger.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeterCurrLedger.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeterCurrLedger.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeterCurrLedger.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeterCurrLedger).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeterCurrLedger).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeterCurrLedger.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeterCurrLedger).Location = new System.Drawing.Point(80, 6);
		((System.Windows.Forms.Control)(object)this.cmbperemeterCurrLedger).Name = "cmbperemeterCurrLedger";
		((System.Windows.Forms.Control)(object)this.cmbperemeterCurrLedger).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeterCurrLedger).TabIndex = 0;
		this.cmbperemeterCurrLedger.ValueChanged += new System.EventHandler(cmbperemeterCurrLedger_ValueChanged);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(229, 12);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 113;
		this.label2.Text = "Customer Name";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(15, 12);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(60, 15);
		this.label3.TabIndex = 115;
		this.label3.Text = "Date Type";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(15, 41);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 15);
		this.label4.TabIndex = 111;
		this.label4.Text = "From Date";
		this.txtToDateCurrLedger.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateCurrLedger.CustomFormat = "dd-MMM-yy";
		this.txtToDateCurrLedger.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateCurrLedger.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateCurrLedger.Location = new System.Drawing.Point(80, 65);
		this.txtToDateCurrLedger.Name = "txtToDateCurrLedger";
		this.txtToDateCurrLedger.Size = new System.Drawing.Size(143, 23);
		this.txtToDateCurrLedger.TabIndex = 2;
		this.txtFromDateCurrLedger.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateCurrLedger.CustomFormat = "dd-MMM-yy";
		this.txtFromDateCurrLedger.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateCurrLedger.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateCurrLedger.Location = new System.Drawing.Point(80, 37);
		this.txtFromDateCurrLedger.Name = "txtFromDateCurrLedger";
		this.txtFromDateCurrLedger.Size = new System.Drawing.Size(143, 23);
		this.txtFromDateCurrLedger.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(15, 69);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 110;
		this.label5.Text = "To Date";
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label6);
		this.panel11.Controls.Add(this.toolStrip1);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1005, 57);
		this.panel11.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(6, 33);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(236, 19);
		this.label6.TabIndex = 116;
		this.label6.Text = "Orders With Ledger Balance";
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewCurrLedger, this.btnRefreshcurrLedger, this.btnPrintcurrLedger });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1005, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNewCurrLedger.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewCurrLedger.ForeColor = System.Drawing.Color.Black;
		this.btnNewCurrLedger.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewCurrLedger.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewCurrLedger.Name = "btnNewCurrLedger";
		this.btnNewCurrLedger.Size = new System.Drawing.Size(58, 24);
		this.btnNewCurrLedger.Text = "New";
		this.btnNewCurrLedger.Click += new System.EventHandler(btnNewCurrLedger_Click);
		this.btnRefreshcurrLedger.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshcurrLedger.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshcurrLedger.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshcurrLedger.Name = "btnRefreshcurrLedger";
		this.btnRefreshcurrLedger.Size = new System.Drawing.Size(76, 24);
		this.btnRefreshcurrLedger.Text = "Refresh";
		this.btnRefreshcurrLedger.Click += new System.EventHandler(btnRefreshcurrLedger_Click);
		this.btnPrintcurrLedger.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintcurrLedger.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintcurrLedger.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintcurrLedger.Name = "btnPrintcurrLedger";
		this.btnPrintcurrLedger.Size = new System.Drawing.Size(84, 24);
		this.btnPrintcurrLedger.Text = "Print-146";
		this.btnPrintcurrLedger.Click += new System.EventHandler(btnPrintcurrLedger_Click);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdPartyWise);
		this.tabPage3.Controls.Add(this.panel3);
		this.tabPage3.Controls.Add(this.panel1);
		this.tabPage3.Controls.Add(this.toolStrip2);
		this.tabPage3.Location = new System.Drawing.Point(4, 22);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1011, 564);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Party Wise Summary";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grdPartyWise.AllowEdit = (InheritableBoolean)2;
		this.grdPartyWise.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPartyWise_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPartyWise.DesignTimeLayout = grdPartyWise_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPartyWise).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPartyWise.DynamicFiltering = true;
		this.grdPartyWise.FilterMode = (FilterMode)2;
		this.grdPartyWise.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPartyWise).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPartyWise.GroupByBoxVisible = false;
		this.grdPartyWise.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPartyWise).Location = new System.Drawing.Point(0, 131);
		((System.Windows.Forms.Control)(object)this.grdPartyWise).Name = "grdPartyWise";
		((ScrollableBase)this.grdPartyWise).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPartyWise).Size = new System.Drawing.Size(1011, 433);
		((System.Windows.Forms.Control)(object)this.grdPartyWise).TabIndex = 6;
		this.grdPartyWise.TotalRow = (InheritableBoolean)1;
		this.grdPartyWise.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPartyWise).VisualStyle = (VisualStyle)4;
		this.grdPartyWise.LinkClicked += new ColumnActionEventHandler(grdPartyWise_LinkClicked);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 53);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1011, 78);
		this.panel3.TabIndex = 5;
		this.groupBox1.Controls.Add(this.FromDatePartyWiseTab);
		this.groupBox1.Controls.Add(this.btnShowPartyWiseTab);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateTypePartyWiseTab);
		this.groupBox1.Controls.Add(this.ToDatePartyWiseTab);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(8, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(473, 71);
		this.groupBox1.TabIndex = 116;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.FromDatePartyWiseTab.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDatePartyWiseTab.CustomFormat = "dd-MMM-yyyy";
		this.FromDatePartyWiseTab.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDatePartyWiseTab.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDatePartyWiseTab.Location = new System.Drawing.Point(141, 41);
		this.FromDatePartyWiseTab.Name = "FromDatePartyWiseTab";
		this.FromDatePartyWiseTab.Size = new System.Drawing.Size(132, 23);
		this.FromDatePartyWiseTab.TabIndex = 1;
		this.btnShowPartyWiseTab.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowPartyWiseTab.FlatAppearance.BorderSize = 0;
		this.btnShowPartyWiseTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowPartyWiseTab.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowPartyWiseTab.ForeColor = System.Drawing.Color.White;
		this.btnShowPartyWiseTab.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowPartyWiseTab.Location = new System.Drawing.Point(403, 40);
		this.btnShowPartyWiseTab.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowPartyWiseTab.Name = "btnShowPartyWiseTab";
		this.btnShowPartyWiseTab.Size = new System.Drawing.Size(59, 25);
		this.btnShowPartyWiseTab.TabIndex = 4;
		this.btnShowPartyWiseTab.Text = "Show";
		this.btnShowPartyWiseTab.UseVisualStyleBackColor = false;
		this.btnShowPartyWiseTab.Click += new System.EventHandler(btnShowPartyWiseTab_Click);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(274, 21);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(48, 15);
		this.label12.TabIndex = 110;
		this.label12.Text = "To Date";
		this.CmbDateTypePartyWiseTab.AllowNull = (DefaultableBoolean)1;
		this.CmbDateTypePartyWiseTab.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDateTypePartyWiseTab.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDateTypePartyWiseTab.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateTypePartyWiseTab).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateTypePartyWiseTab).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateTypePartyWiseTab.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateTypePartyWiseTab).Location = new System.Drawing.Point(7, 39);
		((System.Windows.Forms.Control)(object)this.CmbDateTypePartyWiseTab).Name = "CmbDateTypePartyWiseTab";
		((System.Windows.Forms.Control)(object)this.CmbDateTypePartyWiseTab).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateTypePartyWiseTab).TabIndex = 0;
		this.CmbDateTypePartyWiseTab.ValueChanged += new System.EventHandler(CmbDateTypePartyWiseTab_ValueChanged);
		this.ToDatePartyWiseTab.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDatePartyWiseTab.CustomFormat = "dd-MMM-yyyy";
		this.ToDatePartyWiseTab.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDatePartyWiseTab.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDatePartyWiseTab.Location = new System.Drawing.Point(274, 41);
		this.ToDatePartyWiseTab.Name = "ToDatePartyWiseTab";
		this.ToDatePartyWiseTab.Size = new System.Drawing.Size(127, 23);
		this.ToDatePartyWiseTab.TabIndex = 2;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(7, 21);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(60, 15);
		this.label10.TabIndex = 115;
		this.label10.Text = "Date Type";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(141, 21);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(63, 15);
		this.label11.TabIndex = 111;
		this.label11.Text = "From Date";
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar3);
		this.panel1.Controls.Add(this.label7);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1011, 26);
		this.panel1.TabIndex = 4;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(971, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdPartyWise;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 117;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(4, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(178, 19);
		this.label7.TabIndex = 116;
		this.label7.Text = "Party Wise Summary";
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewPartyWiseTab, this.btnPrintPartyWiseTab });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1011, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewPartyWiseTab.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewPartyWiseTab.ForeColor = System.Drawing.Color.Black;
		this.btnNewPartyWiseTab.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewPartyWiseTab.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewPartyWiseTab.Name = "btnNewPartyWiseTab";
		this.btnNewPartyWiseTab.Size = new System.Drawing.Size(58, 24);
		this.btnNewPartyWiseTab.Text = "New";
		this.btnNewPartyWiseTab.Click += new System.EventHandler(btnNewPartyWiseTab_Click);
		this.btnPrintPartyWiseTab.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintPartyWiseTab.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintPartyWiseTab.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintPartyWiseTab.Name = "btnPrintPartyWiseTab";
		this.btnPrintPartyWiseTab.Size = new System.Drawing.Size(84, 24);
		this.btnPrintPartyWiseTab.Text = "Print-199";
		this.btnPrintPartyWiseTab.Click += new System.EventHandler(btnPrintPartyWiseTab_Click);
		this.panel2.Controls.Add(this.tabControl1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1019, 590);
		this.panel2.TabIndex = 2;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1019, 590);
		base.Controls.Add(this.panel2);
		base.Name = "OrdersWithLedgerBalance";
		this.Text = "Orders With Ledger Balance";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OrdersWithLedgerBalance_FormClosing);
		base.Load += new System.EventHandler(OrdersWithLedgerBalance_Load);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).EndInit();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCurrLedger).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerCurrLedger).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeterCurrLedger).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.tabPage3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPartyWise).EndInit();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypePartyWiseTab).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
