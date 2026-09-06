// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Lab;
using Architecture.BLL.PartyProcessing;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.PartyProcessing.Dashboard
{

public class frmPendingWorksDetailPartyProcessing : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int[] AryColWidth;

	private DropDownBind DDL = new DropDownBind();

	public DateTime FromDate = DateTime.Now;

	public DateTime ToDate = DateTime.Now;

	public string DepartmentName = "";

	public string PendingWorkName = "";

	private DataTable dtGrid = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnnew;

	private ToolStripSeparator toolStripSeparator;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	private Label label1;

	internal GridEX GridDetail;

	private CtrlGrdBar ctrlGrdBar2;

	private Label lblHeading;

	public frmPendingWorksDetailPartyProcessing(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void GridBind()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtGrid.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = FromDate;
			obj.ToDate = ToDate;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.ReportType = "DetailByPendingWorkCaption";
			obj.DepartmentName = DepartmentName;
			obj.PendingWorkName = PendingWorkName;
			lblHeading.Text = PendingWorkName;
			dtGrid = InvFoodProductionPartyProcessing.PendingWorkDashboardPartyProcessing(obj);
			if (dtGrid.Rows.Count <= 0)
			{
				return;
			}
			if (PendingWorkName == "Pending For Wages")
			{
				DataTable dtMain = new DataTable();
				dtMain.Columns.Add("Id", typeof(int));
				dtMain.Columns.Add("PendingWorkDepartment");
				dtMain.Columns.Add("PendingWorkCaption");
				dtMain.Columns.Add("DocDate", typeof(DateTime));
				dtMain.Columns.Add("DocNo", typeof(int));
				dtMain.Columns.Add("GpNo");
				dtMain.Columns.Add("StockParty");
				dtMain.Columns.Add("TotalQty", typeof(double));
				dtMain.Columns.Add("GrossWeight", typeof(double));
				dtMain.Columns.Add("NetWeight", typeof(double));
				dtMain.Columns.Add("StockWeight", typeof(double));
				int j;
				for (j = 0; j < dtGrid.Rows.Count; j++)
				{
					bool num = dtMain.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[j]["Id"]));
					double TotalQty = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[j]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("ItemQuantity"));
					double TotalGrossWeight = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[j]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("GrossWeight"));
					double TotalNetWeight = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[j]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("NetWeight"));
					double TotalStockWeight = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[j]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("StockWeight"));
					if (!num)
					{
						dtMain.Rows.Add(dtGrid.Rows[j]["Id"], dtGrid.Rows[j]["PendingWorkDepartment"], dtGrid.Rows[j]["PendingWorkCaption"], Conversion.ToDateTime(dtGrid.Rows[j]["DocDate"]).ToShortDateString(), dtGrid.Rows[j]["DocNo"], dtGrid.Rows[j]["GpNo"], dtGrid.Rows[j]["StockParty"], TotalQty, TotalGrossWeight, TotalNetWeight, TotalStockWeight);
					}
				}
				grdfrm.DataSource = dtMain;
				grdfrm.RetrieveStructure();
				GridSetting();
				return;
			}
			DataTable dtMain2 = new DataTable();
			dtMain2.Columns.Add("Id", typeof(int));
			dtMain2.Columns.Add("PendingWorkDepartment");
			dtMain2.Columns.Add("PendingWorkCaption");
			dtMain2.Columns.Add("DocDate", typeof(DateTime));
			dtMain2.Columns.Add("GpNo");
			dtMain2.Columns.Add("StockParty");
			dtMain2.Columns.Add("TotalQty", typeof(double));
			dtMain2.Columns.Add("SupplierWeight", typeof(double));
			dtMain2.Columns.Add("FactoryWeight", typeof(double));
			dtMain2.Columns.Add("DiffWeight", typeof(double));
			dtMain2.Columns.Add("Status", typeof(string));
			int i;
			for (i = 0; i < dtGrid.Rows.Count; i++)
			{
				if (dtMain2.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[i]["Id"])))
				{
					continue;
				}
				double TotalQty2 = (from row in dtGrid.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[i]["Id"])
					select row).Sum((DataRow row) => row.Field<double>("Quanitity"));
				double DoOrSupplierWt = 0.0;
				if (!(DepartmentName == "Sale Department"))
				{
					DoOrSupplierWt = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[i]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("SupplierWeight"));
				}
				double FactoryWeight = (from row in dtGrid.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[i]["Id"])
					select row).Sum((DataRow row) => row.Field<double>("FactoryWeight"));
				double DiffWeight = (from row in dtGrid.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[i]["Id"])
					select row).Sum((DataRow row) => row.Field<double>("DifferenceWeight"));
				dtMain2.Rows.Add(dtGrid.Rows[i]["Id"], dtGrid.Rows[i]["PendingWorkDepartment"], dtGrid.Rows[i]["PendingWorkCaption"], Conversion.ToDateTime(dtGrid.Rows[i]["GpDate"]).ToShortDateString(), dtGrid.Rows[i]["GpNo"], dtGrid.Rows[i]["StockParty"], TotalQty2, DoOrSupplierWt, FactoryWeight, DiffWeight, dtGrid.Rows[i]["Status"]);
			}
			grdfrm.DataSource = dtMain2;
			grdfrm.RetrieveStructure();
			GridSettingForGatePasses();
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
			grdfrm.AllowDelete = (InheritableBoolean)2;
			grdfrm.AllowEdit = (InheritableBoolean)2;
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["PendingWorkDepartment"].Visible = false;
			grdfrm.RootTable.Columns["PendingWorkCaption"].Visible = false;
			grdfrm.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["Id"].Width = 60;
			grdfrm.RootTable.Columns["PendingWorkDepartment"].Width = 160;
			grdfrm.RootTable.Columns["PendingWorkCaption"].Width = 140;
			grdfrm.RootTable.Columns["DocDate"].Width = 90;
			grdfrm.RootTable.Columns["DocNo"].Width = 70;
			grdfrm.RootTable.Columns["GpNo"].Width = 60;
			grdfrm.RootTable.Columns["StockParty"].Width = 200;
			grdfrm.RootTable.Columns["TotalQty"].Width = 80;
			grdfrm.RootTable.Columns["GrossWeight"].Width = 90;
			grdfrm.RootTable.Columns["NetWeight"].Width = 90;
			grdfrm.RootTable.Columns["StockWeight"].Width = 90;
			grdfrm.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["TotalQty"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["GrossWeight"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["NetWeight"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["StockWeight"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForGatePasses()
	{
		try
		{
			grdfrm.AllowDelete = (InheritableBoolean)2;
			grdfrm.AllowEdit = (InheritableBoolean)2;
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["PendingWorkDepartment"].Visible = false;
			grdfrm.RootTable.Columns["PendingWorkCaption"].Visible = false;
			grdfrm.RootTable.Columns["SupplierWeight"].Visible = !(DepartmentName == "Sale Department");
			grdfrm.RootTable.Columns["DiffWeight"].Visible = !(DepartmentName == "Sale Department");
			grdfrm.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["Id"].Width = 60;
			grdfrm.RootTable.Columns["PendingWorkDepartment"].Width = 100;
			grdfrm.RootTable.Columns["PendingWorkCaption"].Width = 150;
			grdfrm.RootTable.Columns["DocDate"].Width = 90;
			grdfrm.RootTable.Columns["GpNo"].Width = 60;
			grdfrm.RootTable.Columns["StockParty"].Width = 200;
			grdfrm.RootTable.Columns["TotalQty"].Width = 80;
			grdfrm.RootTable.Columns["SupplierWeight"].Width = 100;
			grdfrm.RootTable.Columns["FactoryWeight"].Width = 100;
			grdfrm.RootTable.Columns["DiffWeight"].Width = 90;
			grdfrm.RootTable.Columns["Status"].Width = 70;
			grdfrm.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["TotalQty"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["SupplierWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["SupplierWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["SupplierWeight"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["SupplierWeight"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["FactoryWeight"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["DiffWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["DiffWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["DiffWeight"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["DiffWeight"].TotalFormatString = "#,##0";
			ctrlGrdBar1_Load(null, null);
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
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			string PendingWorkDepartment = Conversion.ToString(r.Cells["PendingWorkDepartment"].Value);
			Conversion.ToString(r.Cells["PendingWorkCaption"].Value);
			if (!(e.Column.Key == "GpNo"))
			{
				return;
			}
			if (PendingWorkDepartment == "Purchase Department")
			{
				if (PendingWorkName == "Pending For Wages")
				{
					CommonServices.PartyProcessingGRNSlip332(Id);
				}
				else if (PendingWorkName == "Total Gate Passes" || PendingWorkName == "Pending For First Weight" || PendingWorkName == "Pending For Second Weight" || PendingWorkName == "Pending For Clearance" || PendingWorkName == "Accepted Gate Passes" || PendingWorkName == "Rejected Gate Passes" || PendingWorkName == "Pending For GRN")
				{
					CommonServices.PartyProcessingInwardGatePassSlip299(Id);
				}
			}
			else if (PendingWorkName == "Pending For Wages")
			{
				CommonServices.PartyProcessingGDNSlip332_01(Id);
			}
			else if (PendingWorkName == "Total Gate Passes" || PendingWorkName == "Pending For First Weight" || PendingWorkName == "Pending For Second Weight" || PendingWorkName == "Pending For Clearance" || PendingWorkName == "Accepted Gate Passes" || PendingWorkName == "Rejected Gate Passes" || PendingWorkName == "Pending For GDN")
			{
				CommonServices.PartyProcessingOutwardGatePassSlip299(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdfrm.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			if (PendingWorkName == "Pending For Wages")
			{
				DataTable dtDetail = new DataTable();
				dtDetail.Columns.Add("GpNo");
				dtDetail.Columns.Add("VehicleNo");
				dtDetail.Columns.Add("BiltyNo");
				dtDetail.Columns.Add("ItemName");
				dtDetail.Columns.Add("TotalQty", typeof(double));
				dtDetail.Columns.Add("GrossWeight", typeof(double));
				dtDetail.Columns.Add("NetWeight", typeof(double));
				dtDetail.Columns.Add("StockWeight", typeof(double));
				DataTable resultRows = (from row in dtGrid.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dtDetail.Rows.Add(resultRows.Rows[i]["GpNo"], resultRows.Rows[i]["VehicleNo"], resultRows.Rows[i]["BiltyNo"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["ItemQuantity"], resultRows.Rows[i]["GrossWeight"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["StockWeight"]);
				}
				GridDetail.DataSource = dtDetail;
				GridDetail.RetrieveStructure();
				GridDetail.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
				GridDetail.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
				GridDetail.RootTable.Columns["TotalQty"].FormatString = "#,##0";
				GridDetail.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0";
				GridDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
				GridDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
				GridDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0";
				GridDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0";
				GridDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
				GridDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
				GridDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0";
				GridDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0";
				GridDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
				GridDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
				GridDetail.RootTable.Columns["StockWeight"].FormatString = "#,##0";
				GridDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0";
				CommonServices.GridAutoAdjustment(AryColWidth, GridDetail);
				ctrlGrdBar2_Load(null, null);
				return;
			}
			DataTable dtDetail2 = new DataTable();
			dtDetail2.Columns.Add("GpNo");
			dtDetail2.Columns.Add("VehicleNo");
			dtDetail2.Columns.Add("BiltyNo");
			dtDetail2.Columns.Add("StockParty");
			dtDetail2.Columns.Add("ItemName");
			dtDetail2.Columns.Add("TotalQty", typeof(double));
			dtDetail2.Columns.Add("SupplierWeight", typeof(double));
			dtDetail2.Columns.Add("FactoryWeight", typeof(double));
			dtDetail2.Columns.Add("DiffWeight", typeof(double));
			DataTable resultRows2 = (from row in dtGrid.AsEnumerable()
				where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
				select row).CopyToDataTable();
			for (int j = 0; j < resultRows2.Rows.Count; j++)
			{
				dtDetail2.Rows.Add(resultRows2.Rows[j]["GpNo"], resultRows2.Rows[j]["VehicleNo"], resultRows2.Rows[j]["BiltyNo"], resultRows2.Rows[j]["StockParty"], resultRows2.Rows[j]["VarietyName"], resultRows2.Rows[j]["Quanitity"], resultRows2.Rows[j]["SupplierWeight"], resultRows2.Rows[j]["FactoryWeight"], resultRows2.Rows[j]["DifferenceWeight"]);
			}
			GridDetail.DataSource = dtDetail2;
			GridDetail.RetrieveStructure();
			GridDetail.RootTable.Columns["SupplierWeight"].Visible = !(DepartmentName == "Sale Department");
			GridDetail.RootTable.Columns["DiffWeight"].Visible = !(DepartmentName == "Sale Department");
			GridDetail.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["TotalQty"].FormatString = "#,##0";
			GridDetail.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0";
			GridDetail.RootTable.Columns["SupplierWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["SupplierWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["SupplierWeight"].FormatString = "#,##0";
			GridDetail.RootTable.Columns["SupplierWeight"].TotalFormatString = "#,##0";
			GridDetail.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["FactoryWeight"].FormatString = "#,##0";
			GridDetail.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0";
			GridDetail.RootTable.Columns["DiffWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["DiffWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["DiffWeight"].FormatString = "#,##0";
			GridDetail.RootTable.Columns["DiffWeight"].TotalFormatString = "#,##0";
			CommonServices.GridAutoAdjustment(AryColWidth, GridDetail);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void frmPendingGrnRpt_Load(object sender, EventArgs e)
	{
		GridBind();
	}

	private void btnnew_Click(object sender, EventArgs e)
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

	private void frmPendingWorksDetailPartyProcessing_FormClosed(object sender, FormClosedEventArgs e)
	{
		HistoryStack.Remove(this);
	}

	private static void GenerateReport(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvLabAnalysisSaleHeader.SaleAnalysisPrint(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = Id
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "658-RptInvLabSaleAnalysisSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdfrm).Name);
			if (fs != null)
			{
				grdfrm.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetail).Name);
			if (fs != null)
			{
				GridDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Expected O, but got Unknown
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.GridDetail = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.grdfrm = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.lblHeading = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(800, 636);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel4.Controls.Add(this.panel3);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 54);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(800, 582);
		this.panel4.TabIndex = 2;
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetail).BackColor = System.Drawing.Color.White;
		this.GridDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetail.FilterMode = (FilterMode)2;
		this.GridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupTotals = (GroupTotals)2;
		this.GridDetail.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 393);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(800, 189);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 8;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 363);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(800, 30);
		this.panel3.TabIndex = 7;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(754, 2);
		this.ctrlGrdBar2.MyGrid = this.GridDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar2.TabIndex = 11;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(8, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(97, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Detail Grid";
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		this.grdfrm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(800, 363);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 6;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.SelectionChanged += new System.EventHandler(grdfrm_SelectionChanged);
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.lblHeading);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(800, 54);
		this.panel2.TabIndex = 0;
		this.lblHeading.AutoSize = true;
		this.lblHeading.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHeading.ForeColor = System.Drawing.Color.White;
		this.lblHeading.Location = new System.Drawing.Point(3, 30);
		this.lblHeading.Name = "lblHeading";
		this.lblHeading.Size = new System.Drawing.Size(177, 19);
		this.lblHeading.TabIndex = 12;
		this.lblHeading.Text = "Pending Work Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(756, 28);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripSeparator1, this.btnnew, this.toolStripSeparator });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(800, 27);
		this.toolStrip1.TabIndex = 9;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(80, 24);
		this.btnnew.Text = "&Refresh";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 636);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmPendingWorksDetailPartyProcessing";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmPendingWorksDetailPartyProcessing_FormClosed);
		base.Load += new System.EventHandler(frmPendingGrnRpt_Load);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
