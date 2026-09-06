// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Purchase;
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
namespace Architecture.WinApp.Dashboard
{

public class frmPendingWorksDetail : Form
{
	private UserAccount UserAccount = new UserAccount();

	public DateTime FromDate = DateTime.Now;

	public DateTime ToDate = DateTime.Now;

	public string DepartmentName = "";

	public string PendingWorkName = "";

	public string PendingWorkCaption = "";

	public int TypeId = 1;

	private DataTable dtGrid = new DataTable();

	private bool BranchFeature;

	public string BranchesIds = "";

	private IContainer components;

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

	private ToolStripButton btnPrint358;

	private ToolStripButton btnReportPendingForGrn;

	public frmPendingWorksDetail(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmPendingGrnRpt_Load(object sender, EventArgs e)
	{
		if (DepartmentName == "Sale Department" && (PendingWorkName == "Pending For Invoice" || PendingWorkName == "Pending For GDN"))
		{
			btnPrint358.Visible = true;
		}
		else
		{
			btnPrint358.Visible = false;
		}
		if (DepartmentName == "Purchase Department" && PendingWorkName == "Pending For GRN")
		{
			btnReportPendingForGrn.Visible = true;
		}
		else
		{
			btnReportPendingForGrn.Visible = false;
		}
		BranchFeature = CommonServices.GetERPFeatureById(11);
		if (PendingWorkName != "Pending For Unloading")
		{
			GridBind();
			return;
		}
		panel3.Visible = false;
		((Control)(object)GridDetail).Visible = false;
		GetInformationOfPendingForUnloading();
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
			obj.BranchesIds = ((!BranchFeature) ? "" : ((BranchesIds != string.Empty) ? BranchesIds : ""));
			obj.TypeId = TypeId;
			lblHeading.Text = PendingWorkCaption;
			dtGrid = Dashboard.PendingWorkDashboard(obj);
			if (dtGrid.Rows.Count <= 0)
			{
				return;
			}
			if (PendingWorkName == "Pending For Invoice" || PendingWorkName == "Pending GRN For Stock Evaluation" || PendingWorkName == "Pending For Wages" || PendingWorkName == "Pending For Wages Export")
			{
				DataTable dtMain = new DataTable();
				dtMain.Columns.Add("Id", typeof(int));
				dtMain.Columns.Add("BranchName");
				dtMain.Columns.Add("PendingWorkDepartment");
				dtMain.Columns.Add("PendingWorkCaption");
				dtMain.Columns.Add("DocDate", typeof(DateTime));
				dtMain.Columns.Add("DocNo", typeof(int));
				dtMain.Columns.Add("GpNo");
				dtMain.Columns.Add("PartyName");
				dtMain.Columns.Add("TotalQty", typeof(double));
				dtMain.Columns.Add("GrossWeight", typeof(double));
				dtMain.Columns.Add("NetWeight", typeof(double));
				dtMain.Columns.Add("StockWeight", typeof(double));
				dtMain.Columns.Add("LabId", typeof(int));
				dtMain.Columns.Add("LabNo", typeof(int));
				dtMain.Columns.Add("LabStatus", typeof(string));
				int k;
				for (k = 0; k < dtGrid.Rows.Count; k++)
				{
					bool num = dtMain.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[k]["Id"]));
					double TotalQty = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[k]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("ItemQuantity"));
					double TotalGrossWeight = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[k]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("GrossWeight"));
					double TotalNetWeight = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[k]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("NetWeight"));
					double TotalStockWeight = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[k]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("StockWeight"));
					if (!num)
					{
						dtMain.Rows.Add(dtGrid.Rows[k]["Id"], dtGrid.Rows[k]["BranchName"], dtGrid.Rows[k]["PendingWorkDepartment"], dtGrid.Rows[k]["PendingWorkCaption"], Conversion.ToDateTime(dtGrid.Rows[k]["DocDate"]).ToShortDateString(), dtGrid.Rows[k]["DocNo"], dtGrid.Rows[k]["GpNo"], dtGrid.Rows[k]["Supplier_Customer"], TotalQty, TotalGrossWeight, TotalNetWeight, TotalStockWeight, dtGrid.Rows[k]["LabId"], dtGrid.Rows[k]["LabNo"], dtGrid.Rows[k]["ApprovedStatus"]);
					}
				}
				grdfrm.DataSource = dtMain;
				grdfrm.RetrieveStructure();
				GridSetting();
				return;
			}
			if (PendingWorkName == "Total Invoices Pending for Voucher" || PendingWorkName == "Invoices Pending Clearance and Not Fully Forwarded" || PendingWorkName == "Invoices Awaiting Special Approval" || PendingWorkName == "Invoices Ready for Voucher")
			{
				DataTable dtMain2 = new DataTable();
				dtMain2.Columns.Add("PendingWorkDepartment");
				dtMain2.Columns.Add("PendingWorkCaption");
				dtMain2.Columns.Add("BranchName");
				dtMain2.Columns.Add("DocTypeId", typeof(int));
				dtMain2.Columns.Add("InvoiceId", typeof(int));
				dtMain2.Columns.Add("InvoiceDocNo", typeof(int));
				dtMain2.Columns.Add("InvoiceDate", typeof(DateTime));
				dtMain2.Columns.Add("InvoiceNo");
				dtMain2.Columns.Add("PartyName");
				dtMain2.Columns.Add("InvoiceNetWeight", typeof(double));
				dtMain2.Columns.Add("InvoiceGrossWeight", typeof(double));
				dtMain2.Columns.Add("DoWeight", typeof(double));
				dtMain2.Columns.Add("DispatchWeight", typeof(double));
				int j;
				for (j = 0; j < dtGrid.Rows.Count; j++)
				{
					if (!dtMain2.AsEnumerable().Any((DataRow row) => row.Field<int>("InvoiceId") == Conversion.ToInt(dtGrid.Rows[j]["InvoiceId"])))
					{
						dtMain2.Rows.Add(dtGrid.Rows[j]["PendingWorkDepartment"], dtGrid.Rows[j]["PendingWorkCaption"], dtGrid.Rows[j]["BranchName"], dtGrid.Rows[j]["DocTypeId"], dtGrid.Rows[j]["InvoiceId"], dtGrid.Rows[j]["InvoiceDocNo"], Conversion.ToDateTime(dtGrid.Rows[j]["InvoiceDate"]).ToShortDateString(), dtGrid.Rows[j]["InvoiceNo"], dtGrid.Rows[j]["Supplier_Customer"], dtGrid.Rows[j]["InvoiceNetWeight"], dtGrid.Rows[j]["InvoiceGrossWeight"], dtGrid.Rows[j]["DoWeight"], dtGrid.Rows[j]["DispatchWeight"]);
					}
				}
				((Control)(object)grdfrm).Name = "ExportNewCards";
				grdfrm.DataSource = dtMain2;
				grdfrm.RetrieveStructure();
				GridSettingForExportNewCards();
				return;
			}
			DataTable dtMain3 = new DataTable();
			dtMain3.Columns.Add("Id", typeof(int));
			dtMain3.Columns.Add("BranchName");
			dtMain3.Columns.Add("PendingWorkDepartment");
			dtMain3.Columns.Add("PendingWorkCaption");
			dtMain3.Columns.Add("DocDate", typeof(DateTime));
			dtMain3.Columns.Add("GpNo");
			dtMain3.Columns.Add("InTime", typeof(DateTime));
			dtMain3.Columns.Add("OutTime", typeof(DateTime));
			dtMain3.Columns.Add("GatePassType");
			dtMain3.Columns.Add("OrderType");
			if (DepartmentName == "Purchase Department" && PendingWorkName == "Pending For GRN")
			{
				dtMain3.Columns.Add("OrderNo", typeof(int));
				dtMain3.Columns.Add("DeliveryTerm", typeof(string));
				dtMain3.Columns.Add("Remarks", typeof(string));
			}
			dtMain3.Columns.Add("PartyName");
			dtMain3.Columns.Add("TotalQty", typeof(double));
			if (DepartmentName == "Sale Department" || DepartmentName == "Export Department")
			{
				dtMain3.Columns.Add("DoWeight", typeof(double));
			}
			else
			{
				dtMain3.Columns.Add("SupplierWeight", typeof(double));
			}
			dtMain3.Columns.Add("FactoryWeight", typeof(double));
			dtMain3.Columns.Add("DiffWeight", typeof(double));
			dtMain3.Columns.Add("Short/Excess");
			dtMain3.Columns.Add("Status", typeof(string));
			dtMain3.Columns.Add("LabId", typeof(int));
			dtMain3.Columns.Add("LabNo", typeof(int));
			dtMain3.Columns.Add("LabStatus", typeof(string));
			dtMain3.Columns.Add("CityName");
			dtMain3.Columns.Add("Freight", typeof(double));
			int i;
			for (i = 0; i < dtGrid.Rows.Count; i++)
			{
				if (!dtMain3.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[i]["Id"])))
				{
					int TotalQty2 = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[i]["Id"])
						select row).Sum((DataRow row) => row.Field<int>("Quanitity"));
					double DoOrSupplierWt = 0.0;
					DoOrSupplierWt = ((!(DepartmentName == "Sale Department") && !(DepartmentName == "Export Department")) ? (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[i]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("SupplierWeight")) : (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[i]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("DeliveryOrder_Weight")));
					double FactoryWeight = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[i]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("FactoryWeight"));
					double DiffWeight = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtGrid.Rows[i]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("DifferenceWeight"));
					DataRow newRow = dtMain3.NewRow();
					newRow["Id"] = dtGrid.Rows[i]["Id"];
					newRow["BranchName"] = dtGrid.Rows[i]["BranchName"];
					newRow["PendingWorkDepartment"] = dtGrid.Rows[i]["PendingWorkDepartment"];
					newRow["PendingWorkCaption"] = dtGrid.Rows[i]["PendingWorkCaption"];
					newRow["DocDate"] = Conversion.ToDateTime(dtGrid.Rows[i]["GpDate"]);
					newRow["InTime"] = dtGrid.Rows[i]["InDateTimeStamp"];
					newRow["OutTime"] = dtGrid.Rows[i]["OutDateTimeStamp"];
					newRow["GpNo"] = dtGrid.Rows[i]["GpNo"];
					newRow["GatePassType"] = dtGrid.Rows[i]["GatePassType"];
					newRow["OrderType"] = dtGrid.Rows[i]["OrderType"];
					if (DepartmentName == "Purchase Department" && PendingWorkName == "Pending For GRN")
					{
						newRow["OrderNo"] = dtGrid.Rows[i]["OrderNo"];
						newRow["DeliveryTerm"] = dtGrid.Rows[i]["DeliveryTerm"];
						newRow["Remarks"] = dtGrid.Rows[i]["OrderRemarksHeader"];
					}
					newRow["PartyName"] = ((DepartmentName == "Sale Department" || DepartmentName == "Export Department") ? dtGrid.Rows[i]["CustomerName"] : dtGrid.Rows[i]["SupplierName"]);
					newRow["TotalQty"] = TotalQty2;
					newRow[(DepartmentName == "Sale Department" || DepartmentName == "Export Department") ? "DoWeight" : "SupplierWeight"] = DoOrSupplierWt;
					newRow["FactoryWeight"] = FactoryWeight;
					newRow["DiffWeight"] = DiffWeight;
					newRow["Short/Excess"] = dtGrid.Rows[i]["Short/Excess"];
					newRow["Status"] = dtGrid.Rows[i]["Status"];
					newRow["LabId"] = dtGrid.Rows[i]["LabId"];
					newRow["LabNo"] = dtGrid.Rows[i]["LabNo"];
					newRow["LabStatus"] = dtGrid.Rows[i]["ApprovedStatus"];
					newRow["CityName"] = dtGrid.Rows[i]["CityName"];
					newRow["Freight"] = dtGrid.Rows[i]["Freight"];
					dtMain3.Rows.Add(newRow);
				}
			}
			grdfrm.DataSource = dtMain3;
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
			List<string> hiddenColumns = new List<string> { "Id", "PendingWorkDepartment", "PendingWorkCaption", "LabId" };
			if (DepartmentName != "Purchase Department")
			{
				hiddenColumns.AddRange(new string[2] { "LabNo", "LabStatus" });
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 2, hiddenColumns);
			grdfrm.RootTable.Columns["BranchName"].Visible = BranchFeature && BranchesIds != string.Empty;
			grdfrm.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdfrm.RootTable.Columns["PendingWorkDepartment"].Width = Constants.InventoryConstants.Caption;
			grdfrm.RootTable.Columns["PendingWorkCaption"].Width = Constants.InventoryConstants.Caption;
			grdfrm.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdfrm.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdfrm.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdfrm.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.StringConstant;
			grdfrm.RootTable.Columns["TotalQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdfrm.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdfrm.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdfrm.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdfrm.RootTable.Columns["LabNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdfrm.RootTable.Columns["LabStatus"].Width = Constants.InventoryConstants.StatusConstant;
			if (DepartmentName == "Purchase Department")
			{
				grdfrm.RootTable.Columns["LabNo"].ColumnType = (ColumnType)5;
			}
			grdfrm.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForExportNewCards()
	{
		try
		{
			grdfrm.AllowDelete = (InheritableBoolean)2;
			grdfrm.AllowEdit = (InheritableBoolean)2;
			List<string> hiddenColumns = new List<string> { "DocTypeId", "PendingWorkDepartment", "PendingWorkCaption", "InvoiceId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 2, hiddenColumns);
			grdfrm.RootTable.Columns["BranchName"].Visible = BranchFeature && BranchesIds != string.Empty;
			grdfrm.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdfrm.RootTable.Columns["PendingWorkDepartment"].Width = Constants.InventoryConstants.Caption;
			grdfrm.RootTable.Columns["PendingWorkCaption"].Width = Constants.InventoryConstants.Caption;
			grdfrm.RootTable.Columns["InvoiceDate"].Width = Constants.ExportConstants.DateConstant + 10;
			grdfrm.RootTable.Columns["InvoiceDocNo"].Width = Constants.ExportConstants.DocNoConstant + 15;
			grdfrm.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant + 20;
			grdfrm.RootTable.Columns["PartyName"].Width = Constants.ExportConstants.PartyName;
			grdfrm.RootTable.Columns["InvoiceNetWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdfrm.RootTable.Columns["InvoiceGrossWeight"].Width = Constants.InventoryConstants.WeightConstant + 10;
			grdfrm.RootTable.Columns["DoWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdfrm.RootTable.Columns["DispatchWeight"].Width = Constants.InventoryConstants.WeightConstant;
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
			List<string> hiddenColumns = new List<string> { "Id", "PendingWorkDepartment", "PendingWorkCaption", "LabId" };
			if (DepartmentName != "Purchase Department")
			{
				hiddenColumns.AddRange(new string[2] { "LabNo", "LabStatus" });
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 2, hiddenColumns);
			grdfrm.RootTable.Columns["BranchName"].Visible = BranchFeature && BranchesIds != string.Empty;
			grdfrm.RootTable.Columns["DocDate"].Caption = "GatePass Date";
			grdfrm.RootTable.Columns["GpNo"].Caption = "Gate No";
			grdfrm.RootTable.Columns["DiffWeight"].Caption = "DiffWeight (Fact-Supp)";
			grdfrm.RootTable.Columns["LabNo"].Caption = "Lab Analysis";
			grdfrm.RootTable.Columns["LabStatus"].Caption = "Accepted / Rejected";
			grdfrm.RootTable.Columns["Short/Excess"].Caption = "Short / Excess";
			grdfrm.RootTable.Columns["CityName"].Caption = "City Area";
			grdfrm.RootTable.Columns["Status"].Caption = "GatePass Status";
			grdfrm.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grdfrm.RootTable.Columns["InTime"].FormatString = "dd-MMM-yy hh:mm tt";
			grdfrm.RootTable.Columns["OutTime"].FormatString = "dd-MMM-yy hh:mm tt";
			grdfrm.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdfrm.RootTable.Columns["PendingWorkDepartment"].Width = Constants.InventoryConstants.Caption;
			grdfrm.RootTable.Columns["PendingWorkCaption"].Width = Constants.InventoryConstants.Caption;
			grdfrm.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdfrm.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdfrm.RootTable.Columns["OrderType"].Width = Constants.InventoryConstants.PackingType - 10;
			grdfrm.RootTable.Columns["GatePassType"].Width = Constants.InventoryConstants.TermConstant;
			grdfrm.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.StringConstant;
			grdfrm.RootTable.Columns["TotalQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdfrm.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdfrm.RootTable.Columns["DiffWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdfrm.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant;
			grdfrm.RootTable.Columns["LabNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdfrm.RootTable.Columns["LabStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdfrm.RootTable.Columns["LabStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdfrm.RootTable.Columns["InTime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdfrm.RootTable.Columns["OutTime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdfrm.RootTable.Columns["Short/Excess"].Width = Constants.InventoryConstants.StatusConstant - 10;
			if (DepartmentName != "Purchase Department")
			{
				grdfrm.RootTable.Columns["DoWeight"].Width = Constants.InventoryConstants.WeightConstant;
			}
			else
			{
				grdfrm.RootTable.Columns["SupplierWeight"].Width = Constants.InventoryConstants.WeightConstant;
				grdfrm.RootTable.Columns["LabNo"].ColumnType = (ColumnType)5;
			}
			grdfrm.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetInformationOfPendingForUnloading()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtGrid.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = clsGlobalVariables.ActiveYr.Start_Period;
			obj.ToDate = ToDate;
			lblHeading.Text = PendingWorkCaption;
			((Control)(object)grdfrm).Name = "PendingForUnloading";
			dtGrid = GatePassInward.PendingGpForUnloading(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id", typeof(int));
				dtHistory.Columns.Add("WbId", typeof(int));
				dtHistory.Columns.Add("LabId", typeof(int));
				dtHistory.Columns.Add("RowNo", typeof(int));
				dtHistory.Columns.Add("GpDate", typeof(DateTime));
				dtHistory.Columns.Add("GpNo", typeof(int));
				dtHistory.Columns.Add("VehicleNo", typeof(string));
				dtHistory.Columns.Add("Priority", typeof(string));
				dtHistory.Columns.Add("GpType", typeof(string));
				dtHistory.Columns.Add("PurchaseType", typeof(string));
				dtHistory.Columns.Add("GateIn", typeof(DateTime));
				dtHistory.Columns.Add("InWaiting", typeof(string));
				dtHistory.Columns.Add("LoadingFrom", typeof(string));
				dtHistory.Columns.Add("LabReport#", typeof(string));
				dtHistory.Columns.Add("LabStatus", typeof(string));
				dtHistory.Columns.Add("WbSlipNo", typeof(int));
				dtHistory.Columns.Add("Qty/Bags", typeof(double));
				dtHistory.Columns.Add("FirstWeight", typeof(double));
				dtHistory.Columns.Add("LabSortNo", typeof(int));
				dtHistory.Columns.Add("SupplierCustomerId", typeof(int));
				dtHistory.Columns.Add("SupplierName", typeof(string));
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dtGrid.Rows[i]["Id"], dtGrid.Rows[i]["WbId"], dtGrid.Rows[i]["LabId"], dtGrid.Rows[i]["RowNo"], Conversion.ToDateTime(dtGrid.Rows[i]["GpDate"]).ToShortDateString(), dtGrid.Rows[i]["GpSrNo"], dtGrid.Rows[i]["VehicleNo"], dtGrid.Rows[i]["Priority"], dtGrid.Rows[i]["GatepassType"], dtGrid.Rows[i]["PurchaseType"], dtGrid.Rows[i]["InDateTimeStamp"], dtGrid.Rows[i]["WaitingTime"], dtGrid.Rows[i]["CityName"], dtGrid.Rows[i]["LabNo"], dtGrid.Rows[i]["LabStatus"], dtGrid.Rows[i]["TicketNo"], dtGrid.Rows[i]["NoOfBags"], dtGrid.Rows[i]["FirstWeight"], dtGrid.Rows[i]["LabSortNo"], dtGrid.Rows[i]["SupplierCustomerId"], dtGrid.Rows[i]["SupplierName"]);
				}
				grdfrm.DataSource = dtHistory;
				grdfrm.RetrieveStructure();
				GridSettingPendingForUnloading();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingPendingForUnloading()
	{
		try
		{
			grdfrm.AllowDelete = (InheritableBoolean)2;
			grdfrm.AllowEdit = (InheritableBoolean)2;
			List<string> hiddenColumns = new List<string> { "Id", "WbId", "LabId", "SupplierCustomerId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 2, hiddenColumns);
			grdfrm.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdfrm.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdfrm.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdfrm.RootTable.Columns["Priority"].Width = Constants.InventoryConstants.StringConstant;
			grdfrm.RootTable.Columns["GpType"].Width = Constants.InventoryConstants.GpType;
			grdfrm.RootTable.Columns["PurchaseType"].Width = Constants.InventoryConstants.StringConstant + 50;
			grdfrm.RootTable.Columns["GateIn"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdfrm.RootTable.Columns["InWaiting"].Width = Constants.InventoryConstants.StringConstant;
			grdfrm.RootTable.Columns["LoadingFrom"].Width = Constants.InventoryConstants.StringConstant + 50;
			grdfrm.RootTable.Columns["LabReport#"].Width = Constants.InventoryConstants.DocNoConstant;
			grdfrm.RootTable.Columns["LabStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdfrm.RootTable.Columns["WbSlipNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdfrm.RootTable.Columns["Qty/Bags"].Width = Constants.InventoryConstants.QtyConstant;
			grdfrm.RootTable.Columns["FirstWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdfrm.RootTable.Columns["LabSortNo"].Width = Constants.InventoryConstants.StatusConstant;
			grdfrm.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grdfrm.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
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
			if (PendingWorkName == "Pending For Unloading")
			{
				int gpId = Conversion.ToInt(r.Cells["Id"].Value);
				int wbId = Conversion.ToInt(r.Cells["WbId"].Value);
				frmUnLoadingInformation obj = new frmUnLoadingInformation(_LabId: Conversion.ToInt(r.Cells["LabId"].Value), user: UserAccount, dt: dtGrid, _GpId: gpId, _WbId: wbId);
				obj.BringToFront();
				obj.ShowDialog();
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			string PendingWorkDepartment = Conversion.ToString(r.Cells["PendingWorkDepartment"].Value);
			Conversion.ToString(r.Cells["PendingWorkCaption"].Value);
			if (e.Column.Key == "GpNo")
			{
				if (PendingWorkDepartment == "Purchase Department")
				{
					if (PendingWorkName == "Pending For Invoice" || PendingWorkName == "Pending GRN For Stock Evaluation" || PendingWorkName == "Pending For Wages" || PendingWorkName == "Pending For Wages Export")
					{
						CommonServices.GrnSlipWithSubReports(Id);
					}
					else if (PendingWorkName == "Total Gate Passes" || PendingWorkName == "Total Open Gate Passes" || PendingWorkName == "Pending For Lab Analysis" || PendingWorkName == "Pending For First Lab" || PendingWorkName == "Pending For Second Lab" || PendingWorkName == "Pending For First Weight" || PendingWorkName == "Pending For Second Weight" || PendingWorkName == "Pending For Clearance" || PendingWorkName == "Accepted Gate Passes" || PendingWorkName == "Rejected Gate Passes" || PendingWorkName == "Pending For GRN")
					{
						CommonServices.GatePassInwardSlipAndRegisterReport(Id);
					}
					else if (PendingWorkName == "Accepted Lab Analysis" || PendingWorkName == "Rejected Lab Analysis")
					{
						CommonServices.LabAnalysisReport653(0, Id);
					}
				}
				else if (PendingWorkName == "Pending For Invoice" || PendingWorkName == "Pending For Wages")
				{
					CommonServices.InvGdnSlip260(Id);
				}
				else if (PendingWorkName == "Total Gate Passes" || PendingWorkName == "Total Open Gate Passes" || PendingWorkName == "Pending For Lab Analysis" || PendingWorkName == "Pending For First Lab" || PendingWorkName == "Pending For Second Lab" || PendingWorkName == "Pending For First Weight" || PendingWorkName == "Pending For Second Weight" || PendingWorkName == "Pending For Clearance" || PendingWorkName == "Accepted Gate Passes" || PendingWorkName == "Rejected Gate Passes" || PendingWorkName == "Pending For GDN" || PendingWorkName == "Pending For Forwarding")
				{
					CommonServices.GatePassOutwardSlipandRegister(Id);
				}
				else if (PendingWorkName == "Accepted Lab Analysis" || PendingWorkName == "Rejected Lab Analysis")
				{
					GenerateReport(Id);
				}
				else if (PendingWorkName == "Pending For Wages Export")
				{
					CommonServices.ForwardingSlip507(Id);
				}
			}
			if (e.Column.Key == "LabNo" && PendingWorkDepartment == "Purchase Department")
			{
				CommonServices.LabAnalysisReport653(Conversion.ToInt(r.Cells["LabId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_SelectionChanged(object sender, EventArgs e)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(PendingWorkName != "Pending For Unloading"))
			{
				return;
			}
			GridEXRow item = grdfrm.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			if (PendingWorkName == "Pending For Invoice" || PendingWorkName == "Pending GRN For Stock Evaluation" || PendingWorkName == "Pending For Wages" || PendingWorkName == "Pending For Wages Export")
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
				GridEX_Helper.GridWrappingAndColumnSettings(GridDetail, 2, 2);
				ctrlGrdBar2_Load(null, null);
				return;
			}
			if (PendingWorkName == "Total Invoices Pending for Voucher" || PendingWorkName == "Invoices Pending Clearance and Not Fully Forwarded" || PendingWorkName == "Invoices Awaiting Special Approval" || PendingWorkName == "Invoices Ready for Voucher")
			{
				DataTable dtDetail2 = new DataTable();
				dtDetail2.Columns.Add("ForwardingId", typeof(int));
				dtDetail2.Columns.Add("ForwardingDate", typeof(DateTime));
				dtDetail2.Columns.Add("ForwardingNo", typeof(int));
				dtDetail2.Columns.Add("GatePassOutwardId", typeof(int));
				dtDetail2.Columns.Add("GpNo", typeof(int));
				dtDetail2.Columns.Add("VehicleNo");
				dtDetail2.Columns.Add("BiltyNo");
				dtDetail2.Columns.Add("ItemName");
				dtDetail2.Columns.Add("TotalQuantity", typeof(double));
				dtDetail2.Columns.Add("GrossWeight", typeof(double));
				dtDetail2.Columns.Add("NetWeight", typeof(double));
				dtDetail2.Columns.Add("StockWeight", typeof(double));
				dtDetail2.Columns.Add("Status", typeof(string));
				dtDetail2.Columns.Add("InTime", typeof(DateTime));
				dtDetail2.Columns.Add("OutTime", typeof(DateTime));
				DataTable resultRows2 = (from row in dtGrid.AsEnumerable()
					where row.Field<int>("InvoiceId") == Conversion.ToInt(item.Cells["InvoiceId"].Value)
					select row).CopyToDataTable();
				for (int j = 0; j < resultRows2.Rows.Count; j++)
				{
					dtDetail2.Rows.Add(resultRows2.Rows[j]["ForwardingId"], Conversion.ToDateTime(resultRows2.Rows[j]["ForwardingDate"]).ToShortDateString(), resultRows2.Rows[j]["ForwardingNo"], resultRows2.Rows[j]["GatePassOutwardId"], resultRows2.Rows[j]["GpNo"], resultRows2.Rows[j]["VehicleNo"], resultRows2.Rows[j]["BiltyNo"], resultRows2.Rows[j]["ItemName"], resultRows2.Rows[j]["ItemQuantity"], resultRows2.Rows[j]["GrossWeight"], resultRows2.Rows[j]["NetWeight"], resultRows2.Rows[j]["StockWeight"], resultRows2.Rows[j]["Status"], resultRows2.Rows[j]["InDateTimeStamp"], resultRows2.Rows[j]["OutDateTimeStamp"]);
				}
				GridDetail.DataSource = dtDetail2;
				GridDetail.RetrieveStructure();
				((Control)(object)GridDetail).Name = "ExportNewCardsDetail";
				GridDetail.RootTable.Columns["ForwardingId"].Visible = false;
				GridDetail.RootTable.Columns["GatePassOutwardId"].Visible = false;
				GridEX_Helper.GridWrappingAndColumnSettings(GridDetail, 2, 2);
				GridDetail.RootTable.Columns["InTime"].FormatString = "dd-MMM-yyyy hh:mm tt";
				GridDetail.RootTable.Columns["OutTime"].FormatString = "dd-MMM-yyyy hh:mm tt";
				GridDetail.RootTable.Columns["ForwardingNo"].ColumnType = (ColumnType)5;
				GridDetail.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
				GridDetail.RootTable.Columns["ForwardingDate"].Width = Constants.ExportConstants.DateConstant + 10;
				GridDetail.RootTable.Columns["ForwardingNo"].Width = Constants.ExportConstants.DocNoConstant;
				GridDetail.RootTable.Columns["GpNo"].Width = Constants.ExportConstants.GpNoConstant;
				GridDetail.RootTable.Columns["VehicleNo"].Width = Constants.ExportConstants.VehicleNoConstant;
				GridDetail.RootTable.Columns["BiltyNo"].Width = Constants.ExportConstants.BiltyNoConstant;
				GridDetail.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.ItemNameConstants;
				GridDetail.RootTable.Columns["TotalQuantity"].Width = Constants.ExportConstants.QtyConstant;
				GridDetail.RootTable.Columns["GrossWeight"].Width = Constants.ExportConstants.WeightConstant;
				GridDetail.RootTable.Columns["NetWeight"].Width = Constants.ExportConstants.WeightConstant;
				GridDetail.RootTable.Columns["StockWeight"].Width = Constants.ExportConstants.WeightConstant;
				GridDetail.RootTable.Columns["Status"].Width = Constants.ExportConstants.StatusConstant;
				GridDetail.RootTable.Columns["InTime"].Width = Constants.ExportConstants.DateTimeConstantddMMM + 10;
				GridDetail.RootTable.Columns["OutTime"].Width = Constants.ExportConstants.DateTimeConstantddMMM + 10;
				ctrlGrdBar2_Load(null, null);
				return;
			}
			DataTable dtDetail3 = new DataTable();
			dtDetail3.Columns.Add("GpNo");
			dtDetail3.Columns.Add("VehicleNo");
			dtDetail3.Columns.Add("BiltyNo");
			dtDetail3.Columns.Add("PartyName");
			dtDetail3.Columns.Add("ItemName");
			if (DepartmentName == "Purchase Department" && PendingWorkName == "Pending For GRN")
			{
				dtDetail3.Columns.Add("OrderRate", typeof(double));
				dtDetail3.Columns.Add("OrderRemarksDetail");
			}
			dtDetail3.Columns.Add("TotalQty", typeof(double));
			if (DepartmentName == "Sale Department" || DepartmentName == "Export Department")
			{
				dtDetail3.Columns.Add("DoWeight", typeof(double));
			}
			else
			{
				dtDetail3.Columns.Add("SupplierWeight", typeof(double));
			}
			dtDetail3.Columns.Add("FactoryWeight", typeof(double));
			dtDetail3.Columns.Add("DiffWeight", typeof(double));
			DataTable resultRows3 = (from row in dtGrid.AsEnumerable()
				where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
				select row).CopyToDataTable();
			for (int k = 0; k < resultRows3.Rows.Count; k++)
			{
				DataRow newRow = dtDetail3.NewRow();
				newRow["GpNo"] = resultRows3.Rows[k]["GpNo"];
				newRow["VehicleNo"] = resultRows3.Rows[k]["VehicleNo"];
				newRow["BiltyNo"] = resultRows3.Rows[k]["BiltyNo"];
				newRow["PartyName"] = ((DepartmentName == "Sale Department" || DepartmentName == "Export Department") ? resultRows3.Rows[k]["CustomerName"] : resultRows3.Rows[k]["SupplierName"]);
				newRow["ItemName"] = resultRows3.Rows[k]["VarietyName"];
				if (DepartmentName == "Purchase Department" && PendingWorkName == "Pending For GRN")
				{
					newRow["OrderRate"] = resultRows3.Rows[k]["OrderItemRate"];
					newRow["OrderRemarksDetail"] = resultRows3.Rows[k]["OrderRemarksDetail"];
				}
				newRow["TotalQty"] = resultRows3.Rows[k]["Quanitity"];
				if (DepartmentName == "Sale Department" || DepartmentName == "Export Department")
				{
					newRow["DoWeight"] = resultRows3.Rows[k]["DeliveryOrder_Weight"];
				}
				else
				{
					newRow["SupplierWeight"] = resultRows3.Rows[k]["SupplierWeight"];
				}
				newRow["FactoryWeight"] = resultRows3.Rows[k]["FactoryWeight"];
				newRow["DiffWeight"] = resultRows3.Rows[k]["DifferenceWeight"];
				dtDetail3.Rows.Add(newRow);
			}
			GridDetail.DataSource = dtDetail3;
			GridDetail.RetrieveStructure();
			GridEX_Helper.GridWrappingAndColumnSettings(GridDetail, 2, 2);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
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

	private void frmPendingWorksDetail_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnPrint358_Click(object sender, EventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (DepartmentName == "Sale Department" && (PendingWorkName == "Pending For Invoice" || PendingWorkName == "Pending For GDN"))
			{
				DataTable dt = GatePassOutward.PendingGatePassForDelivery_prnt(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				});
				if (dt == null || dt.Rows.Count <= 0)
				{
					throw new Exception("No Record Found");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "358-PendingGatePassForDelivery.rpt");
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

	private void GridDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GridDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int GpId = Conversion.ToInt(r.Cells["GatePassOutwardId"].Value);
				int ForwardingId = Conversion.ToInt(r.Cells["ForwardingId"].Value);
				if (e.Column.Key == "ForwardingNo")
				{
					CommonServices.ForwardingSlip507(ForwardingId);
				}
				else if (e.Column.Key == "GpNo")
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(GpId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnReportPendingForGrn_Click(object sender, EventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (DepartmentName == "Purchase Department" && PendingWorkName == "Pending For GRN")
			{
				if (dtGrid == null || dtGrid.Rows.Count <= 0)
				{
					throw new Exception("No Record Found");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtGrid, "358_01_PendingGatePassForGRN.rpt");
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Expected O, but got Unknown
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Expected O, but got Unknown
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
		this.btnPrint358 = new System.Windows.Forms.ToolStripButton();
		this.btnReportPendingForGrn = new System.Windows.Forms.ToolStripButton();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetail).BackColor = System.Drawing.Color.White;
		this.GridDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
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
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(800, 243);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 8;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.GridDetail.LinkClicked += new ColumnActionEventHandler(GridDetail_LinkClicked);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel3.Location = new System.Drawing.Point(0, 373);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(800, 20);
		this.panel3.TabIndex = 7;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(755, 0);
		this.ctrlGrdBar2.MyGrid = this.GridDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(41, 19);
		this.ctrlGrdBar2.TabIndex = 11;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(1, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(97, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Detail Grid";
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		this.grdfrm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 54);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(800, 319);
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
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.toolStripSeparator1, this.btnnew, this.toolStripSeparator, this.btnPrint358, this.btnReportPendingForGrn });
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
		this.btnPrint358.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint358.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint358.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint358.Name = "btnPrint358";
		this.btnPrint358.Size = new System.Drawing.Size(92, 24);
		this.btnPrint358.Text = "358-Print";
		this.btnPrint358.Click += new System.EventHandler(btnPrint358_Click);
		this.btnReportPendingForGrn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReportPendingForGrn.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnReportPendingForGrn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnReportPendingForGrn.Name = "btnReportPendingForGrn";
		this.btnReportPendingForGrn.Size = new System.Drawing.Size(116, 24);
		this.btnReportPendingForGrn.Text = "358_01-Print";
		this.btnReportPendingForGrn.Click += new System.EventHandler(btnReportPendingForGrn_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 636);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel3);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		base.KeyPreview = true;
		base.Name = "frmPendingWorksDetail";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmPendingWorksDetail_FormClosed);
		base.Load += new System.EventHandler(frmPendingGrnRpt_Load);
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
