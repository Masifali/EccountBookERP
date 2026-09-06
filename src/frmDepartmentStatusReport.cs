// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Production;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Stocks_Report;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.ExportReports
{

public class frmDepartmentStatusReport : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DataTable dtDepartmentDetailGrid = new DataTable();

	private int RecIdDepartmentDetail;

	private DateTime LastScheduleLoadingDate = DateTime.Now;

	private IContainer components;

	private TabPage tabPageDepartmentStatus;

	internal GridEX grdDepartmentDetail;

	private Panel panel1;

	private Panel panel28;

	private GroupBox groupBox3;

	private Label label37;

	private TextBox txtPackingMaterialScheduleDays;

	private Label label36;

	private TextBox txtInspectionDays;

	private Label label35;

	private TextBox txtProductionDays;

	private DateTimePicker txtPMDateDepartmentDetail;

	private DateTimePicker txtInspectionDateDepartmentDetail;

	private DateTimePicker txtProductionDateDepartmentDetail;

	private DateTimePicker txtLoadingDateDepartmentDetail;

	internal UltraCombo CmbCustomerDepartmentDetail;

	internal UltraCombo CmbScheduleLockStatusDepartmentDetail;

	internal UltraCombo CmbItemDepartmentDetail;

	internal UltraCombo CmbPortDepartmentDetail;

	private Button BtnCancelDepartmentDetail;

	private Button BtnUpdateDepartmentDetail;

	private Label label20;

	private Label label32;

	private TextBox txtItemMtonDepartmentDetail;

	private Label label31;

	private Label label21;

	private Label label30;

	private Label label22;

	private Label label33;

	private Label label28;

	private Label label23;

	private Label label29;

	private TextBox txtScheduleMtonDepartmentDetail;

	private TextBox txtFclDepartmentDetail;

	private Label label24;

	internal UltraCombo CmbContractDepartmentDetail;

	private Label label25;

	internal UltraCombo CmbPlantDepartmentDetail;

	private Label label34;

	internal UltraCombo CmbScheduleNoDepartmentDetail;

	private Label label26;

	private Panel panel29;

	private ToolStrip toolStrip5;

	private ToolStripButton BtnNewDepartmentDetail;

	private ToolStripButton BtnRefreshDepartmentDetail;

	private Label label27;

	private ToolStrip miniToolStrip;

	private TabControl tabControlMain;

	private Button BtnShowbyDates;

	private TextBox tztAddDays;

	private DateTimePicker txtDateFrom;

	private Label label1;

	private DateTimePicker txtToDate;

	private Label label2;

	private Label label12;

	private ToolStripButton BtnShortCutkeys;

	public frmDepartmentStatusReport(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmSaleContractSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1460, 700);
			dtDepartmentDetailGrid.Columns.Add("Id", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("ContractScheduleId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("ScheduleCode", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("ExImLcOrderId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("ContractNo", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("SupplierCustomerId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("CustomerName", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("FCL", typeof(decimal));
			dtDepartmentDetailGrid.Columns.Add("ScheduleMTon", typeof(decimal));
			dtDepartmentDetailGrid.Columns.Add("LoadingDate", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("ContractScheduleDetailItemId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("ItemCode", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("ItemName", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("ItemMTon", typeof(decimal));
			dtDepartmentDetailGrid.Columns.Add("InspectionSchedule", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("InspectionStart", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("LcLtNo", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("ProductionSchedule", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("ProductionStart", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("CurrentStatus", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("JobOrderId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("PreShipmentInspectionId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("PackMaterialSchedule", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("PackMaterialStart", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("PmStartRefDocumentTypeId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("PmStartRefId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("PlantId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("PlantName", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("ScheduleLockId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("ScheduleLockStatus", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("ProductionDays", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("DestinationPortId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("DestinationPort", typeof(string));
			grdDepartmentDetail.DataSource = dtDepartmentDetailGrid;
			GridDepartmentdetailSetting();
			PlantFeader();
			ScheduleLockStatusDepartmentDetailFill();
			txtDateFrom.Value = DateTime.Now;
			LastScheduleLoadingDate = ExImLcOrderShipmentSchedule.GetLatestAttentiveLoadDate(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (LastScheduleLoadingDate > DateTime.Now)
			{
				tztAddDays.Text = (DateTime.Now - LastScheduleLoadingDate).Days.ToString();
			}
			else
			{
				txtDateFrom.Value = LastScheduleLoadingDate;
			}
			txtToDate.Value = LastScheduleLoadingDate;
			DepartmentDetailGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PlantFeader()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			int BomId = Conversion.ToInt(CmbPlantDepartmentDetail.Value);
			DataTable dtProductionplant = new DataTable();
			dtProductionplant = InvProductionPlant.GetAll(new InvProductionPlant
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtProductionplant.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProductionplant, CmbPlantDepartmentDetail, "Id", "Description", "Plant", true);
				if (BomId > 0)
				{
					if (dtProductionplant.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BomId))
					{
						CmbPlantDepartmentDetail.Value = BomId;
					}
					else
					{
						((Control)(object)CmbPlantDepartmentDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbPlantDepartmentDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ScheduleLockStatusDepartmentDetailFill()
	{
		try
		{
			DataTable dtDepartmentLockStatus = new DataTable();
			dtDepartmentLockStatus.Columns.Add("Id");
			dtDepartmentLockStatus.Columns.Add("Status");
			dtDepartmentLockStatus.Rows.Add(1, "Confirmed");
			dtDepartmentLockStatus.Rows.Add(2, "Not_Confirmed");
			DDL.BindDDL(dtDepartmentLockStatus, CmbScheduleLockStatusDepartmentDetail, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbScheduleLockStatusDepartmentDetail).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (grdDepartmentDetail.RowCount == 0)
			{
				DepartmentDetailGridFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DepartmentDetailGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			if (txtDateFrom.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			}
			if (txtToDate.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)txtToDate.Value);
			}
			DataTable dtDepart = ExImLcContractScheduleDepartment.GetDatForContractScheduleDepartment(obj);
			if (dtDepart.Rows.Count > 0)
			{
				dtDepartmentDetailGrid.Clear();
				foreach (DataRow row in dtDepart.Rows)
				{
					DateTime LoadingDate = Conversion.ToDateTime(row["LoadingDate"]);
					DateTime ProductionDate = Conversion.ToDateTime(row["ProductionSchedule"]);
					int Days = (int)(LoadingDate.Date - ProductionDate.Date).TotalDays;
					dtDepartmentDetailGrid.Rows.Add(row["Id"], row["ContractScheduleId"], row["ScheduleCode"], row["ExImLcOrderId"], row["ContractNo"], row["SupplierCustomerId"], row["CustomerName"], row["FCL"], row["ScheduleMTon"], row["LoadingDate"], row["ContractScheduleDetailItemId"], row["ItemCode"], row["ItemName"], row["ItemMTon"], row["InspectionSchedule"], row["InspectionStart"], row["LcLtNo"], row["ProductionSchedule"], row["ProductionStart"], row["CurrentStatus"], row["JobOrderId"], row["PreShipmentInspectionId"], row["PackMaterialSchedule"], row["PackMaterialStart"], row["PmStartRefDocumentTypeId"], row["PmStartRefId"], row["PlantId"], row["PlantName"], row["ScheduleLockId"], row["ScheduleLockStatus"], Days, row["DestinationPortId"], row["DestinationPort"]);
				}
				grdDepartmentDetail.DataSource = dtDepartmentDetailGrid;
				grdDepartmentDetail.RetrieveStructure();
				GridDepartmentdetailSetting();
			}
			else
			{
				grdDepartmentDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDepartmentdetailSetting()
	{
		try
		{
			grdDepartmentDetail.RootTable.Columns["Id"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["ContractScheduleId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["ExImLcOrderId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["DestinationPortId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["ContractScheduleDetailItemId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["PlantId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["ScheduleLockId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["JobOrderId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["PreShipmentInspectionId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["PmStartRefDocumentTypeId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["PmStartRefId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["LoadingDate"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["ProductionSchedule"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["ProductionStart"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["InspectionSchedule"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["InspectionStart"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["PackMaterialSchedule"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["PackMaterialStart"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["ScheduleCode"].ColumnType = (ColumnType)5;
			grdDepartmentDetail.RootTable.Columns["ContractNo"].ColumnType = (ColumnType)5;
			grdDepartmentDetail.RootTable.Columns["InspectionStart"].ColumnType = (ColumnType)5;
			grdDepartmentDetail.RootTable.Columns["ProductionStart"].ColumnType = (ColumnType)5;
			grdDepartmentDetail.RootTable.Columns["PackMaterialStart"].ColumnType = (ColumnType)5;
			grdDepartmentDetail.RootTable.Columns["ItemCode"].ColumnType = (ColumnType)5;
			CommonServices.GridWrappingAndColumnSettings(grdDepartmentDetail, 3);
			grdDepartmentDetail.RootTable.Columns["ScheduleCode"].Width = 100;
			grdDepartmentDetail.RootTable.Columns["ContractNo"].Width = 120;
			grdDepartmentDetail.RootTable.Columns["SupplierCustomerId"].Width = 100;
			grdDepartmentDetail.RootTable.Columns["CustomerName"].Width = 170;
			grdDepartmentDetail.RootTable.Columns["DestinationPort"].Width = 120;
			grdDepartmentDetail.RootTable.Columns["FCL"].Width = 55;
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].Width = 70;
			grdDepartmentDetail.RootTable.Columns["LoadingDate"].Width = 90;
			grdDepartmentDetail.RootTable.Columns["ItemCode"].Width = 60;
			grdDepartmentDetail.RootTable.Columns["ItemName"].Width = 170;
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].Width = 60;
			grdDepartmentDetail.RootTable.Columns["ProductionDays"].Width = 70;
			grdDepartmentDetail.RootTable.Columns["ProductionSchedule"].Width = 90;
			grdDepartmentDetail.RootTable.Columns["ProductionStart"].Width = 80;
			grdDepartmentDetail.RootTable.Columns["InspectionSchedule"].Width = 90;
			grdDepartmentDetail.RootTable.Columns["InspectionStart"].Width = 80;
			grdDepartmentDetail.RootTable.Columns["PackMaterialSchedule"].Width = 90;
			grdDepartmentDetail.RootTable.Columns["PackMaterialStart"].Width = 90;
			grdDepartmentDetail.RootTable.Columns["PlantName"].Width = 90;
			grdDepartmentDetail.RootTable.Columns["ScheduleLockStatus"].Width = 100;
			grdDepartmentDetail.RootTable.Columns["LcLtNo"].Width = 100;
			grdDepartmentDetail.RootTable.Columns["CurrentStatus"].Width = 100;
			grdDepartmentDetail.RootTable.Columns["InspectionSchedule"].Caption = "Inspection Schedule Date";
			grdDepartmentDetail.RootTable.Columns["InspectionStart"].Caption = "Inspection Request Lodged";
			grdDepartmentDetail.RootTable.Columns["LcLtNo"].Caption = "Lab Code / Lab Lot No";
			grdDepartmentDetail.RootTable.Columns["ContractScheduleId"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ScheduleCode"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ExImLcOrderId"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ContractNo"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["CustomerName"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["DestinationPort"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["FCL"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["LoadingDate"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ContractScheduleDetailItemId"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ProductionSchedule"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ProductionStart"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["InspectionSchedule"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["InspectionStart"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["PackMaterialSchedule"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["PackMaterialStart"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["PlantId"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["PlantName"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ScheduleLockId"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ScheduleLockStatus"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ProductionDays"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ContractScheduleId"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ScheduleCode"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ExImLcOrderId"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ContractNo"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["CustomerName"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["DestinationPort"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["FCL"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["LoadingDate"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["ContractScheduleDetailItemId"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ProductionSchedule"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["ProductionStart"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["InspectionSchedule"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["InspectionStart"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["PackMaterialSchedule"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["PackMaterialStart"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["PlantId"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["PlantName"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ScheduleLockId"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ScheduleLockStatus"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ProductionDays"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["FCL"].AggregateFunction = (AggregateFunction)2;
			grdDepartmentDetail.RootTable.Columns["FCL"].TextAlignment = (TextAlignment)3;
			grdDepartmentDetail.RootTable.Columns["FCL"].FormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["FCL"].TotalFormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].AggregateFunction = (AggregateFunction)2;
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].TextAlignment = (TextAlignment)3;
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].FormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].TotalFormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].AggregateFunction = (AggregateFunction)2;
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].TextAlignment = (TextAlignment)3;
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].FormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].TotalFormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["ProductionDays"].TextAlignment = (TextAlignment)3;
			grdDepartmentDetail.RootTable.Columns["ProductionDays"].FormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["ProductionDays"].TotalFormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns.Add("Edit");
			grdDepartmentDetail.RootTable.Columns["Edit"].Caption = "Edit";
			grdDepartmentDetail.RootTable.Columns["Edit"].Key = "Edit";
			grdDepartmentDetail.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDepartmentDetail.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdDepartmentDetail.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdDepartmentDetail.RootTable.Columns["Edit"].Width = 50;
			grdDepartmentDetail.RootTable.Columns["Edit"].Position = 0;
			grdDepartmentDetail.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdDepartmentDetail_CellFormatting(object sender, RowLoadEventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		try
		{
			foreach (GridEXColumn item in (JanusCollectionBase)grdDepartmentDetail.RootTable.Columns)
			{
				GridEXColumn column = item;
				if (column.Key == "ProductionSchedule" || column.Key == "ProductionStart" || column.Key == "InspectionSchedule" || column.Key == "InspectionStart" || column.Key == "PackMaterialSchedule" || column.Key == "PackMaterialStart")
				{
					if (e.Row.Cells[column.Key].Value == DBNull.Value || e.Row.Cells[column.Key].Value == null)
					{
						e.Row.Cells[column.Key].Value = string.Empty;
					}
					else
					{
						e.Row.Cells[column.Key].Value = ((DateTime)e.Row.Cells[column.Key].Value).ToString("dd-MMM-yyyy");
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdDepartmentDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDepartmentDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Edit")
			{
				DepartmentDetailReadById();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDepartmentDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDepartmentDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				DepartmentDetailReadById();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDepartmentDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDepartmentDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int ContractScheduleId = Conversion.ToInt(r.Cells["ContractScheduleId"].Value);
			int ExImLcOrderId = Conversion.ToInt(r.Cells["ExImLcOrderId"].Value);
			int PreShipmentInspectionId = Conversion.ToInt(r.Cells["PreShipmentInspectionId"].Value);
			int JobOrderId = Conversion.ToInt(r.Cells["JobOrderId"].Value);
			int PmStartRefDocumentTypeId = Conversion.ToInt(r.Cells["PmStartRefDocumentTypeId"].Value);
			int PmStartRefId = Conversion.ToInt(r.Cells["PmStartRefId"].Value);
			int ItemId = Conversion.ToInt(r.Cells["ContractScheduleDetailItemId"].Value);
			if (e.Column.Key == "ContractNo")
			{
				if (ExImLcOrderId > 0)
				{
					CommonServices.SaleContractReportsSlipNew501(ExImLcOrderId);
				}
			}
			else if (e.Column.Key == "ScheduleCode")
			{
				if (ExImLcOrderId > 0 && ContractScheduleId > 0)
				{
					CommonServices.LcOrderShipmentScheduleLcOrderWise_Slip(ExImLcOrderId, ContractScheduleId);
				}
			}
			else if (e.Column.Key == "PackMaterialStart" && PmStartRefId > 0)
			{
				switch (PmStartRefDocumentTypeId)
				{
				case 700:
					CommonServices.PurchaseOrderPackingMaterialSlip215(PmStartRefId);
					break;
				case 245:
					CommonServices.PurchaseInvoiceDirectPM_PartySlip_245(PmStartRefId);
					break;
				case 807:
					CommonServices.StockTransferPackingMaterialAndStore_Slip415(PmStartRefId);
					break;
				case 111:
				{
					DataTable dt = InvFoodProductionReports.InvFoodProductionPackingAndOverHeadReportByJobOrder(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						JobOrderId = PmStartRefId,
						ReportType = "PackingMaterial"
					});
					if (dt.Rows.Count == 0)
					{
						MessageBox.Show("Record Not Found For DisPlay");
						break;
					}
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dt, "606-InvFoodProductionPackingAndOverHeadReportByJobOrder.rpt");
					break;
				}
				}
			}
			else if (e.Column.Key == "InspectionStart")
			{
				if (PreShipmentInspectionId > 0)
				{
					CommonServices.InvLabPreProductionExportLotInspection_Slip(PreShipmentInspectionId);
				}
			}
			else if (e.Column.Key == "ProductionStart" && JobOrderId > 0)
			{
				CommonServices.ProductionRecoveryReport602(JobOrderId);
			}
			else if (e.Column.Key == "ItemCode" || e.Column.Key == "ItemName")
			{
				ShowItemReport(ItemId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowItemReport(int itemId)
	{
		List<tblUserRights> rights = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "RptGenrateStocks" || x.ScreenName.Trim() == "frmStockReportWithValues").ToList();
		if (!rights.Any() || !rights.Any((tblUserRights r) => Conversion.ToBool((object)r.Value.ToString())))
		{
			MessageBox.Show("You don't have the right");
			return;
		}
		frmStockReportWithValues frmStockReportWithValues = new frmStockReportWithValues(UserAccount);
		frmStockReportWithValues.cmbItem.Value = itemId;
		frmStockReportWithValues.Show();
		frmStockReportWithValues.GridBind();
	}

	private void InsertDepartmentDetail()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!DepartmentDetailFormvalidation())
			{
				return;
			}
			ExImLcContractScheduleDepartment obj = new ExImLcContractScheduleDepartment();
			if (RecIdDepartmentDetail > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecIdDepartmentDetail;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (Conversion.ToInt(CmbScheduleLockStatusDepartmentDetail.Value) != 1 || MessageBox.Show("You are about to update the schedule with lock status (Confirmed). Are you sure you want to save?\nRecords with a confirmed status will not be updated afterward.", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				if (Conversion.ToInt(CmbItemDepartmentDetail.Value) > 0)
				{
					obj.ContractScheduleId = Conversion.ToInt(CmbScheduleNoDepartmentDetail.Value);
				}
				obj.ContractScheduleDetailItemId = Conversion.ToInt(CmbItemDepartmentDetail.Value);
				obj.Weight = Conversion.ToDecimal((object)txtItemMtonDepartmentDetail.Text) * 1000m;
				obj.ProductionSchedule = Conversion.ToDateTime((object)txtProductionDateDepartmentDetail.Value);
				obj.Plant = Conversion.ToInt(CmbPlantDepartmentDetail.Value);
				obj.InspectionSchedule = Conversion.ToDateTime((object)txtInspectionDateDepartmentDetail.Value);
				obj.PackMaterialSchedule = Conversion.ToDateTime((object)txtPMDateDepartmentDetail.Value);
				obj.ScheduleLockId = Conversion.ToInt(CmbScheduleLockStatusDepartmentDetail.Value);
				obj.InspectionStart = DateTime.Now;
				obj.PackMaterialStart = DateTime.Now;
				obj.ProductionStart = DateTime.Now;
				ExImLcContractScheduleDepartment.Save(obj);
				if (RecIdDepartmentDetail == 0)
				{
					MessageBox.Show("Detail Save SuccessFully");
				}
				else
				{
					MessageBox.Show("Detail Update SuccessFully");
				}
				DepartmentDetailFormReset();
				DepartmentDetailGridFill();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DepartmentDetailReadById()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDepartmentDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				RecIdDepartmentDetail = Conversion.ToInt(r.Cells["Id"].Value);
				if (Conversion.ToInt(r.Cells["ScheduleLockId"].Value) == 1)
				{
					throw new Exception("This Record has been Confirmed,So it can't be Updated...");
				}
				int ExImLcOrderId = Conversion.ToInt(r.Cells["ExImLcOrderId"].Value);
				string ContractNo = Conversion.ToString(r.Cells["ContractNo"].Value);
				if (ExImLcOrderId > 0)
				{
					DataTable dtExImLcOrderId = new DataTable();
					dtExImLcOrderId.Columns.Add("ExImLcOrderId", typeof(int));
					dtExImLcOrderId.Columns.Add("ContractNo");
					dtExImLcOrderId.Rows.Add(ExImLcOrderId, ContractNo);
					DDL.BindDDL(dtExImLcOrderId, CmbContractDepartmentDetail, "ExImLcOrderId", "ContractNo", "ContractNo", ZeroIndex: false);
					((UltraGridBase)CmbContractDepartmentDetail).Rows[0].Activate();
				}
				int ContractScheduleId = Conversion.ToInt(r.Cells["ContractScheduleId"].Value);
				string ScheduleCode = Conversion.ToString(r.Cells["ScheduleCode"].Value);
				if (ContractScheduleId > 0)
				{
					DataTable dtContractScheduleId = new DataTable();
					dtContractScheduleId.Columns.Add("ContractScheduleId", typeof(int));
					dtContractScheduleId.Columns.Add("ScheduleCode");
					dtContractScheduleId.Rows.Add(ContractScheduleId, ScheduleCode);
					DDL.BindDDL(dtContractScheduleId, CmbScheduleNoDepartmentDetail, "ContractScheduleId", "ScheduleCode", "ScheduleCode", ZeroIndex: false);
					((UltraGridBase)CmbScheduleNoDepartmentDetail).Rows[0].Activate();
				}
				int SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				string CustomerName = Conversion.ToString(r.Cells["CustomerName"].Value);
				if (SupplierCustomerId > 0)
				{
					DataTable dtSupplierCustomerId = new DataTable();
					dtSupplierCustomerId.Columns.Add("SupplierCustomerId", typeof(int));
					dtSupplierCustomerId.Columns.Add("CustomerName");
					dtSupplierCustomerId.Rows.Add(SupplierCustomerId, CustomerName);
					DDL.BindDDL(dtSupplierCustomerId, CmbCustomerDepartmentDetail, "SupplierCustomerId", "CustomerName", "CustomerName", ZeroIndex: false);
					((UltraGridBase)CmbCustomerDepartmentDetail).Rows[0].Activate();
				}
				int DestinationPortId = Conversion.ToInt(r.Cells["DestinationPortId"].Value);
				string DestinationPort = Conversion.ToString(r.Cells["DestinationPort"].Value);
				if (DestinationPortId > 0)
				{
					DataTable dtDestinationPortId = new DataTable();
					dtDestinationPortId.Columns.Add("DestinationPortId", typeof(int));
					dtDestinationPortId.Columns.Add("DestinationPort");
					dtDestinationPortId.Rows.Add(DestinationPortId, DestinationPort);
					DDL.BindDDL(dtDestinationPortId, CmbPortDepartmentDetail, "DestinationPortId", "DestinationPort", "DestinationPort", ZeroIndex: false);
					((UltraGridBase)CmbPortDepartmentDetail).Rows[0].Activate();
				}
				txtFclDepartmentDetail.Text = Conversion.ToString(r.Cells["FCL"].Value).ToString();
				txtScheduleMtonDepartmentDetail.Text = Conversion.ToDecimal(r.Cells["ScheduleMTon"].Value).ToString("#,##.####");
				txtLoadingDateDepartmentDetail.Value = Conversion.ToDateTime(r.Cells["LoadingDate"].Value);
				int ContractScheduleDetailItemId = Conversion.ToInt(r.Cells["ContractScheduleDetailItemId"].Value);
				Conversion.ToString(r.Cells["ItemCode"].Value);
				string ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
				if (ContractScheduleDetailItemId > 0)
				{
					DataTable dtItem = new DataTable();
					dtItem.Columns.Add("ContractScheduleDetailItemId", typeof(int));
					dtItem.Columns.Add("ItemName");
					dtItem.Rows.Add(ContractScheduleDetailItemId, ItemName);
					DDL.BindDDL(dtItem, CmbItemDepartmentDetail, "ContractScheduleDetailItemId", "ItemName", "ItemName", ZeroIndex: false);
					((UltraGridBase)CmbItemDepartmentDetail).Rows[0].Activate();
				}
				txtItemMtonDepartmentDetail.Text = Conversion.ToDecimal(r.Cells["ItemMTon"].Value).ToString("#,##.####");
				txtProductionDateDepartmentDetail.Value = Conversion.ToDateTime(r.Cells["ProductionSchedule"].Value);
				txtInspectionDateDepartmentDetail.Value = Conversion.ToDateTime(r.Cells["InspectionSchedule"].Value);
				txtPMDateDepartmentDetail.Value = Conversion.ToDateTime(r.Cells["PackMaterialSchedule"].Value);
				CmbPlantDepartmentDetail.Value = Conversion.ToInt(r.Cells["PlantId"].Value);
				CmbScheduleLockStatusDepartmentDetail.Value = Conversion.ToInt(r.Cells["ScheduleLockId"].Value);
				txtProductionDateDepartmentDetail_ValueChanged(null, null);
				txtPMDateDepartmentDetail_ValueChanged(null, null);
				txtInspectionDateDepartmentDetail_ValueChanged(null, null);
				txtProductionDays.Focus();
				BtnUpdateDepartmentDetail.Visible = true;
				BtnCancelDepartmentDetail.Visible = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnCancelDepartmentDetail_Click(object sender, EventArgs e)
	{
		try
		{
			DepartmentDetailFormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateDepartmentDetail_Click(object sender, EventArgs e)
	{
		try
		{
			InsertDepartmentDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DepartmentDetailFormReset()
	{
		try
		{
			RecIdDepartmentDetail = 0;
			BtnUpdateDepartmentDetail.Visible = false;
			BtnCancelDepartmentDetail.Visible = false;
			((UltraGridBase)CmbContractDepartmentDetail).DataSource = null;
			((Control)(object)CmbContractDepartmentDetail).Text = string.Empty;
			((UltraGridBase)CmbScheduleNoDepartmentDetail).DataSource = null;
			((Control)(object)CmbScheduleNoDepartmentDetail).Text = string.Empty;
			((UltraGridBase)CmbCustomerDepartmentDetail).DataSource = null;
			((Control)(object)CmbCustomerDepartmentDetail).Text = string.Empty;
			((UltraGridBase)CmbPortDepartmentDetail).DataSource = null;
			((Control)(object)CmbPortDepartmentDetail).Text = string.Empty;
			((UltraGridBase)CmbItemDepartmentDetail).DataSource = null;
			((Control)(object)CmbItemDepartmentDetail).Text = string.Empty;
			txtInspectionDays.Text = string.Empty;
			txtProductionDays.Text = string.Empty;
			txtPackingMaterialScheduleDays.Text = string.Empty;
			((Control)(object)CmbPlantDepartmentDetail).Text = string.Empty;
			((Control)(object)CmbScheduleLockStatusDepartmentDetail).Text = string.Empty;
			txtFclDepartmentDetail.Text = string.Empty;
			txtScheduleMtonDepartmentDetail.Text = string.Empty;
			txtItemMtonDepartmentDetail.Text = string.Empty;
			((Control)(object)grdDepartmentDetail).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnNewDepartmentDetail_Click(object sender, EventArgs e)
	{
		try
		{
			DepartmentDetailFormReset();
			DepartmentDetailGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshDepartmentDetail_Click(object sender, EventArgs e)
	{
		try
		{
			PlantFeader();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool DepartmentDetailFormvalidation()
	{
		if (((Control)(object)CmbContractDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbContractDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Contract No is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbContractDepartmentDetail.Focus();
			return false;
		}
		if (((Control)(object)CmbScheduleNoDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbScheduleNoDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Schedule No is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbScheduleNoDepartmentDetail.Focus();
			return false;
		}
		if (((Control)(object)CmbCustomerDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbCustomerDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Customer is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbCustomerDepartmentDetail.Focus();
			return false;
		}
		if (((Control)(object)CmbPortDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbPortDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Port is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbPortDepartmentDetail.Focus();
			return false;
		}
		if (txtScheduleMtonDepartmentDetail.Text == string.Empty || Conversion.ToDouble((object)txtScheduleMtonDepartmentDetail.Text) == 0.0)
		{
			MessageBox.Show("Schedule Mton Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtScheduleMtonDepartmentDetail.Focus();
			return false;
		}
		if (((Control)(object)CmbItemDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbItemDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Item is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbItemDepartmentDetail.Focus();
			return false;
		}
		if (txtItemMtonDepartmentDetail.Text == string.Empty || Conversion.ToDouble((object)txtItemMtonDepartmentDetail.Text) == 0.0)
		{
			MessageBox.Show("Item Mton Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtItemMtonDepartmentDetail.Focus();
			return false;
		}
		if (txtProductionDays.Text == string.Empty || Conversion.ToInt((object)txtProductionDays.Text) == 0)
		{
			MessageBox.Show("Production Days is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtProductionDays.Focus();
			return false;
		}
		if (((Control)(object)CmbPlantDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbPlantDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Plant is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbPlantDepartmentDetail.Focus();
			return false;
		}
		if (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)txtProductionDateDepartmentDetail.Value)))
		{
			MessageBox.Show("'Production Schedule Date' Is not Valid !", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtProductionDateDepartmentDetail.Focus();
			return false;
		}
		if (Conversion.ToDateTime((object)txtProductionDateDepartmentDetail.Value.Date) > Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value.Date))
		{
			MessageBox.Show("'Production Schedule Date' can not be Greater than 'Loading Date' !", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtProductionDateDepartmentDetail.Focus();
			return false;
		}
		if (txtInspectionDateDepartmentDetail.Checked && Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)txtInspectionDateDepartmentDetail.Value)))
		{
			MessageBox.Show("'Inspection Schedule Date' Is not Valid !\nPlease Correct the Date or Un check the Field", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtInspectionDateDepartmentDetail.Focus();
			return false;
		}
		if (txtInspectionDateDepartmentDetail.Checked && Conversion.ToDateTime((object)txtInspectionDateDepartmentDetail.Value.Date) > Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value.Date))
		{
			MessageBox.Show("'Inspection Schedule Date' can not be Greater than 'Loading Date' !", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtInspectionDateDepartmentDetail.Focus();
			return false;
		}
		if (txtPMDateDepartmentDetail.Checked && Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)txtPMDateDepartmentDetail.Value)))
		{
			MessageBox.Show("'Packing Material Schedule Date' Is not Valid !\nPlease Correct the Date or Un check the Field", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtPMDateDepartmentDetail.Focus();
			return false;
		}
		if (txtPMDateDepartmentDetail.Checked && Conversion.ToDateTime((object)txtPMDateDepartmentDetail.Value.Date) > Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value.Date))
		{
			MessageBox.Show("'Packing Material Schedule Date' can not be Greater than 'Loading Date' !", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtPMDateDepartmentDetail.Focus();
			return false;
		}
		if (((Control)(object)CmbScheduleLockStatusDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbScheduleLockStatusDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Schedule Lock Status is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbScheduleLockStatusDepartmentDetail.Focus();
			return false;
		}
		return true;
	}

	private void grdDepartmentDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDepartmentDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdDepartmentDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdDepartmentDetail.CurrentColumn.Key == "Edit")
				{
					DepartmentDetailReadById();
				}
				_ = grdDepartmentDetail.CurrentColumn.Key == "NoOfAttachments";
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				DepartmentDetailReadById();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtProductionDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int Days = Conversion.ToInt((object)txtProductionDays.Text);
			DateTime LoadingDate = Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value);
			txtProductionDateDepartmentDetail.Value = LoadingDate.AddDays(-Days);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtProductionDateDepartmentDetail_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime ProductionDate = Conversion.ToDateTime((object)txtProductionDateDepartmentDetail.Value);
			if (!Conversion.CheckDateTimeNull(ProductionDate))
			{
				int Days = (int)(Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value).Date - ProductionDate.Date).TotalDays;
				txtProductionDays.Text = Days.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtInspectionDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int Days = Conversion.ToInt((object)txtInspectionDays.Text);
			DateTime LoadingDate = Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value);
			txtInspectionDateDepartmentDetail.Value = LoadingDate.AddDays(-Days);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtInspectionDateDepartmentDetail_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime InspectionDate = Conversion.ToDateTime((object)txtInspectionDateDepartmentDetail.Value);
			if (!Conversion.CheckDateTimeNull(InspectionDate))
			{
				int Days = (int)(Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value).Date - InspectionDate.Date).TotalDays;
				txtInspectionDays.Text = Days.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingMaterialScheduleDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int Days = Conversion.ToInt((object)txtPackingMaterialScheduleDays.Text);
			DateTime LoadingDate = Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value);
			txtPMDateDepartmentDetail.Value = LoadingDate.AddDays(-Days);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPMDateDepartmentDetail_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime PMDateDepartmentDetail = Conversion.ToDateTime((object)txtPMDateDepartmentDetail.Value);
			if (!Conversion.CheckDateTimeNull(PMDateDepartmentDetail))
			{
				int Days = (int)(Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value).Date - PMDateDepartmentDetail.Date).TotalDays;
				txtPackingMaterialScheduleDays.Text = Days.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtProductionDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtInspectionDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackingMaterialScheduleDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void BtnShowbyDates_Click(object sender, EventArgs e)
	{
		try
		{
			DepartmentDetailGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tztAddDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void frmDepartmentStatusReport_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				BtnNewDepartmentDetail_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				BtnShowbyDates_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtDateFrom.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtDateFrom.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdDepartmentDetail).Focus();
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				BtnRefreshDepartmentDetail_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
	{
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show Grid data");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date ");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tztAddDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime ToDate = Conversion.ToDateTime((object)txtDateFrom.Value).AddDays(Conversion.ToInt((object)tztAddDays.Text));
			txtToDate.Value = ToDate;
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
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Expected O, but got Unknown
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Expected O, but got Unknown
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Expected O, but got Unknown
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Expected O, but got Unknown
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Expected O, but got Unknown
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Expected O, but got Unknown
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Expected O, but got Unknown
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Expected O, but got Unknown
		GridEXLayout grdDepartmentDetail_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.frmDepartmentStatusReport));
		Appearance appearance1 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
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
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		this.tabPageDepartmentStatus = new System.Windows.Forms.TabPage();
		this.grdDepartmentDetail = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel28 = new System.Windows.Forms.Panel();
		this.BtnShowbyDates = new System.Windows.Forms.Button();
		this.tztAddDays = new System.Windows.Forms.TextBox();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label37 = new System.Windows.Forms.Label();
		this.txtPackingMaterialScheduleDays = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.txtInspectionDays = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtProductionDays = new System.Windows.Forms.TextBox();
		this.txtPMDateDepartmentDetail = new System.Windows.Forms.DateTimePicker();
		this.txtInspectionDateDepartmentDetail = new System.Windows.Forms.DateTimePicker();
		this.txtProductionDateDepartmentDetail = new System.Windows.Forms.DateTimePicker();
		this.txtLoadingDateDepartmentDetail = new System.Windows.Forms.DateTimePicker();
		this.CmbCustomerDepartmentDetail = new UltraCombo();
		this.CmbScheduleLockStatusDepartmentDetail = new UltraCombo();
		this.CmbItemDepartmentDetail = new UltraCombo();
		this.CmbPortDepartmentDetail = new UltraCombo();
		this.BtnCancelDepartmentDetail = new System.Windows.Forms.Button();
		this.BtnUpdateDepartmentDetail = new System.Windows.Forms.Button();
		this.label20 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.txtItemMtonDepartmentDetail = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtScheduleMtonDepartmentDetail = new System.Windows.Forms.TextBox();
		this.txtFclDepartmentDetail = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbContractDepartmentDetail = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.CmbPlantDepartmentDetail = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbScheduleNoDepartmentDetail = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.panel29 = new System.Windows.Forms.Panel();
		this.toolStrip5 = new System.Windows.Forms.ToolStrip();
		this.BtnNewDepartmentDetail = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshDepartmentDetail = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.label27 = new System.Windows.Forms.Label();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.tabControlMain = new System.Windows.Forms.TabControl();
		this.tabPageDepartmentStatus.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDepartmentDetail).BeginInit();
		this.panel28.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerDepartmentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleLockStatusDepartmentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemDepartmentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPortDepartmentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractDepartmentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantDepartmentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleNoDepartmentDetail).BeginInit();
		this.panel29.SuspendLayout();
		this.toolStrip5.SuspendLayout();
		this.tabControlMain.SuspendLayout();
		base.SuspendLayout();
		this.tabPageDepartmentStatus.Controls.Add((System.Windows.Forms.Control)(object)this.grdDepartmentDetail);
		this.tabPageDepartmentStatus.Controls.Add(this.panel1);
		this.tabPageDepartmentStatus.Controls.Add(this.panel28);
		this.tabPageDepartmentStatus.Controls.Add(this.panel29);
		this.tabPageDepartmentStatus.Location = new System.Drawing.Point(4, 25);
		this.tabPageDepartmentStatus.Name = "tabPageDepartmentStatus";
		this.tabPageDepartmentStatus.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageDepartmentStatus.Size = new System.Drawing.Size(1362, 451);
		this.tabPageDepartmentStatus.TabIndex = 1;
		this.tabPageDepartmentStatus.Text = "Pre Shipment Planning";
		this.tabPageDepartmentStatus.UseVisualStyleBackColor = true;
		this.grdDepartmentDetail.AllowEdit = (InheritableBoolean)2;
		grdDepartmentDetail_DesignTimeLayout.LayoutString = resources.GetString("grdDepartmentDetail_DesignTimeLayout.LayoutString");
		this.grdDepartmentDetail.DesignTimeLayout = grdDepartmentDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDepartmentDetail.DynamicFiltering = true;
		this.grdDepartmentDetail.FilterMode = (FilterMode)2;
		this.grdDepartmentDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDepartmentDetail.GroupByBoxVisible = false;
		this.grdDepartmentDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).Location = new System.Drawing.Point(3, 194);
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).Name = "grdDepartmentDetail";
		((ScrollableBase)this.grdDepartmentDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).Size = new System.Drawing.Size(1356, 254);
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).TabIndex = 0;
		this.grdDepartmentDetail.TotalRow = (InheritableBoolean)1;
		this.grdDepartmentDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDepartmentDetail).VisualStyle = (VisualStyle)4;
		this.grdDepartmentDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDepartmentDetail_ColumnButtonClick);
		this.grdDepartmentDetail.LinkClicked += new ColumnActionEventHandler(grdDepartmentDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).DoubleClick += new System.EventHandler(grdDepartmentDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDepartmentDetail_KeyDown);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 171);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1356, 23);
		this.panel1.TabIndex = 8;
		this.panel28.Controls.Add(this.BtnShowbyDates);
		this.panel28.Controls.Add(this.tztAddDays);
		this.panel28.Controls.Add(this.txtDateFrom);
		this.panel28.Controls.Add(this.label1);
		this.panel28.Controls.Add(this.txtToDate);
		this.panel28.Controls.Add(this.label2);
		this.panel28.Controls.Add(this.label12);
		this.panel28.Controls.Add(this.groupBox3);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel28.Location = new System.Drawing.Point(3, 53);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1356, 118);
		this.panel28.TabIndex = 0;
		this.BtnShowbyDates.BackColor = System.Drawing.Color.Teal;
		this.BtnShowbyDates.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnShowbyDates.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowbyDates.ForeColor = System.Drawing.Color.White;
		this.BtnShowbyDates.Location = new System.Drawing.Point(1390, 88);
		this.BtnShowbyDates.Name = "BtnShowbyDates";
		this.BtnShowbyDates.Size = new System.Drawing.Size(55, 23);
		this.BtnShowbyDates.TabIndex = 4;
		this.BtnShowbyDates.Text = "Show";
		this.BtnShowbyDates.UseVisualStyleBackColor = false;
		this.BtnShowbyDates.Click += new System.EventHandler(BtnShowbyDates_Click);
		this.tztAddDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tztAddDays.Location = new System.Drawing.Point(1303, 35);
		this.tztAddDays.Name = "tztAddDays";
		this.tztAddDays.Size = new System.Drawing.Size(142, 23);
		this.tztAddDays.TabIndex = 2;
		this.tztAddDays.TextChanged += new System.EventHandler(tztAddDays_TextChanged);
		this.tztAddDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(tztAddDays_KeyPress);
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDateFrom.Location = new System.Drawing.Point(1303, 6);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.ShowCheckBox = true;
		this.txtDateFrom.Size = new System.Drawing.Size(142, 23);
		this.txtDateFrom.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(1198, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(109, 15);
		this.label1.TabIndex = 170;
		this.label1.Text = "From Date Loading";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(1303, 64);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.ShowCheckBox = true;
		this.txtToDate.Size = new System.Drawing.Size(142, 23);
		this.txtToDate.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(1198, 68);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(94, 15);
		this.label2.TabIndex = 173;
		this.label2.Text = "To Date Loading";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(1198, 39);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(58, 15);
		this.label12.TabIndex = 175;
		this.label12.Text = "Add Days";
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Controls.Add(this.txtPackingMaterialScheduleDays);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.txtInspectionDays);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.txtProductionDays);
		this.groupBox3.Controls.Add(this.txtPMDateDepartmentDetail);
		this.groupBox3.Controls.Add(this.txtInspectionDateDepartmentDetail);
		this.groupBox3.Controls.Add(this.txtProductionDateDepartmentDetail);
		this.groupBox3.Controls.Add(this.txtLoadingDateDepartmentDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbScheduleLockStatusDepartmentDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail);
		this.groupBox3.Controls.Add(this.BtnCancelDepartmentDetail);
		this.groupBox3.Controls.Add(this.BtnUpdateDepartmentDetail);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.label32);
		this.groupBox3.Controls.Add(this.txtItemMtonDepartmentDetail);
		this.groupBox3.Controls.Add(this.label31);
		this.groupBox3.Controls.Add(this.label21);
		this.groupBox3.Controls.Add(this.label30);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add(this.label33);
		this.groupBox3.Controls.Add(this.label28);
		this.groupBox3.Controls.Add(this.label23);
		this.groupBox3.Controls.Add(this.label29);
		this.groupBox3.Controls.Add(this.txtScheduleMtonDepartmentDetail);
		this.groupBox3.Controls.Add(this.txtFclDepartmentDetail);
		this.groupBox3.Controls.Add(this.label24);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantDepartmentDetail);
		this.groupBox3.Controls.Add(this.label34);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1193, 114);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(651, 64);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(55, 15);
		this.label37.TabIndex = 75;
		this.label37.Text = "P.M Days";
		this.txtPackingMaterialScheduleDays.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialScheduleDays.Location = new System.Drawing.Point(651, 83);
		this.txtPackingMaterialScheduleDays.Name = "txtPackingMaterialScheduleDays";
		this.txtPackingMaterialScheduleDays.Size = new System.Drawing.Size(72, 23);
		this.txtPackingMaterialScheduleDays.TabIndex = 14;
		this.txtPackingMaterialScheduleDays.Tag = "PackingMaterialScheduleDays";
		this.txtPackingMaterialScheduleDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingMaterialScheduleDays_KeyPress);
		this.txtPackingMaterialScheduleDays.Leave += new System.EventHandler(txtPackingMaterialScheduleDays_TextChanged);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(413, 64);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(92, 15);
		this.label36.TabIndex = 73;
		this.label36.Text = "Inspection Days";
		this.txtInspectionDays.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInspectionDays.Location = new System.Drawing.Point(413, 83);
		this.txtInspectionDays.Name = "txtInspectionDays";
		this.txtInspectionDays.Size = new System.Drawing.Size(98, 23);
		this.txtInspectionDays.TabIndex = 12;
		this.txtInspectionDays.Tag = "InspectionDays";
		this.txtInspectionDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtInspectionDays_KeyPress);
		this.txtInspectionDays.Leave += new System.EventHandler(txtInspectionDays_TextChanged);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(3, 64);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(95, 15);
		this.label35.TabIndex = 71;
		this.label35.Text = "Production Days";
		this.txtProductionDays.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionDays.Location = new System.Drawing.Point(3, 83);
		this.txtProductionDays.Name = "txtProductionDays";
		this.txtProductionDays.Size = new System.Drawing.Size(98, 23);
		this.txtProductionDays.TabIndex = 9;
		this.txtProductionDays.Tag = "ProductionDays";
		this.txtProductionDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtProductionDays_KeyPress);
		this.txtProductionDays.Leave += new System.EventHandler(txtProductionDays_TextChanged);
		this.txtPMDateDepartmentDetail.CalendarFont = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMDateDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMDateDepartmentDetail.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtPMDateDepartmentDetail.Location = new System.Drawing.Point(724, 83);
		this.txtPMDateDepartmentDetail.Name = "txtPMDateDepartmentDetail";
		this.txtPMDateDepartmentDetail.ShowCheckBox = true;
		this.txtPMDateDepartmentDetail.Size = new System.Drawing.Size(147, 23);
		this.txtPMDateDepartmentDetail.TabIndex = 15;
		this.txtPMDateDepartmentDetail.ValueChanged += new System.EventHandler(txtPMDateDepartmentDetail_ValueChanged);
		this.txtInspectionDateDepartmentDetail.CalendarFont = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInspectionDateDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInspectionDateDepartmentDetail.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtInspectionDateDepartmentDetail.Location = new System.Drawing.Point(514, 83);
		this.txtInspectionDateDepartmentDetail.Name = "txtInspectionDateDepartmentDetail";
		this.txtInspectionDateDepartmentDetail.ShowCheckBox = true;
		this.txtInspectionDateDepartmentDetail.Size = new System.Drawing.Size(136, 23);
		this.txtInspectionDateDepartmentDetail.TabIndex = 13;
		this.txtInspectionDateDepartmentDetail.ValueChanged += new System.EventHandler(txtInspectionDateDepartmentDetail_ValueChanged);
		this.txtProductionDateDepartmentDetail.CalendarFont = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionDateDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionDateDepartmentDetail.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtProductionDateDepartmentDetail.Location = new System.Drawing.Point(104, 83);
		this.txtProductionDateDepartmentDetail.Name = "txtProductionDateDepartmentDetail";
		this.txtProductionDateDepartmentDetail.Size = new System.Drawing.Size(125, 23);
		this.txtProductionDateDepartmentDetail.TabIndex = 10;
		this.txtProductionDateDepartmentDetail.ValueChanged += new System.EventHandler(txtProductionDateDepartmentDetail_ValueChanged);
		this.txtLoadingDateDepartmentDetail.CalendarFont = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadingDateDepartmentDetail.Enabled = false;
		this.txtLoadingDateDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadingDateDepartmentDetail.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtLoadingDateDepartmentDetail.Location = new System.Drawing.Point(789, 38);
		this.txtLoadingDateDepartmentDetail.Name = "txtLoadingDateDepartmentDetail";
		this.txtLoadingDateDepartmentDetail.Size = new System.Drawing.Size(121, 23);
		this.txtLoadingDateDepartmentDetail.TabIndex = 6;
		this.CmbCustomerDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail).Location = new System.Drawing.Point(276, 36);
		((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail).Name = "CmbCustomerDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail).TabIndex = 2;
		this.CmbScheduleLockStatusDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbScheduleLockStatusDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbScheduleLockStatusDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbScheduleLockStatusDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbScheduleLockStatusDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbScheduleLockStatusDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbScheduleLockStatusDepartmentDetail).Location = new System.Drawing.Point(873, 81);
		((System.Windows.Forms.Control)(object)this.CmbScheduleLockStatusDepartmentDetail).Name = "CmbScheduleLockStatusDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbScheduleLockStatusDepartmentDetail).Size = new System.Drawing.Size(181, 26);
		((System.Windows.Forms.Control)(object)this.CmbScheduleLockStatusDepartmentDetail).TabIndex = 16;
		this.CmbItemDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbItemDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail).Location = new System.Drawing.Point(912, 36);
		((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail).Name = "CmbItemDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail).TabIndex = 7;
		this.CmbPortDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPortDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPortDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPortDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance63;
		((SpecialBoxBase)((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance69).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPortDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail).Location = new System.Drawing.Point(479, 36);
		((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail).Name = "CmbPortDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail).Size = new System.Drawing.Size(156, 26);
		((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail).TabIndex = 3;
		this.BtnCancelDepartmentDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelDepartmentDetail.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.BtnCancelDepartmentDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnCancelDepartmentDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelDepartmentDetail.Location = new System.Drawing.Point(1126, 82);
		this.BtnCancelDepartmentDetail.Name = "BtnCancelDepartmentDetail";
		this.BtnCancelDepartmentDetail.Size = new System.Drawing.Size(63, 25);
		this.BtnCancelDepartmentDetail.TabIndex = 18;
		this.BtnCancelDepartmentDetail.Text = "Cancel";
		this.BtnCancelDepartmentDetail.UseVisualStyleBackColor = false;
		this.BtnCancelDepartmentDetail.Visible = false;
		this.BtnCancelDepartmentDetail.Click += new System.EventHandler(BtnCancelDepartmentDetail_Click);
		this.BtnUpdateDepartmentDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateDepartmentDetail.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.BtnUpdateDepartmentDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateDepartmentDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateDepartmentDetail.Location = new System.Drawing.Point(1057, 82);
		this.BtnUpdateDepartmentDetail.Name = "BtnUpdateDepartmentDetail";
		this.BtnUpdateDepartmentDetail.Size = new System.Drawing.Size(66, 25);
		this.BtnUpdateDepartmentDetail.TabIndex = 17;
		this.BtnUpdateDepartmentDetail.Text = "Update";
		this.BtnUpdateDepartmentDetail.UseVisualStyleBackColor = false;
		this.BtnUpdateDepartmentDetail.Visible = false;
		this.BtnUpdateDepartmentDetail.Click += new System.EventHandler(BtnUpdateDepartmentDetail_Click);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1110, 19);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(69, 15);
		this.label20.TabIndex = 69;
		this.label20.Text = "Item M.Ton";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(724, 64);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(147, 15);
		this.label32.TabIndex = 65;
		this.label32.Text = "Packing Material Schedule";
		this.txtItemMtonDepartmentDetail.Enabled = false;
		this.txtItemMtonDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemMtonDepartmentDetail.Location = new System.Drawing.Point(1110, 38);
		this.txtItemMtonDepartmentDetail.Name = "txtItemMtonDepartmentDetail";
		this.txtItemMtonDepartmentDetail.Size = new System.Drawing.Size(77, 23);
		this.txtItemMtonDepartmentDetail.TabIndex = 8;
		this.txtItemMtonDepartmentDetail.Tag = "WeightMton";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(514, 64);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(115, 15);
		this.label31.TabIndex = 65;
		this.label31.Text = "Inspection Schedule";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(637, 19);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(26, 15);
		this.label21.TabIndex = 67;
		this.label21.Text = "FCL";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(104, 64);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(118, 15);
		this.label30.TabIndex = 65;
		this.label30.Text = "Production Schedule";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(276, 19);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(93, 15);
		this.label22.TabIndex = 65;
		this.label22.Text = "Customer Name";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(873, 64);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(120, 15);
		this.label33.TabIndex = 65;
		this.label33.Text = "Schedule Lock Status";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(789, 19);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(78, 15);
		this.label28.TabIndex = 65;
		this.label28.Text = "Loading Date";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(713, 19);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 15);
		this.label23.TabIndex = 65;
		this.label23.Text = "M.Tons";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(912, 19);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(67, 15);
		this.label29.TabIndex = 65;
		this.label29.Text = "Item Name";
		this.txtScheduleMtonDepartmentDetail.Enabled = false;
		this.txtScheduleMtonDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtScheduleMtonDepartmentDetail.Location = new System.Drawing.Point(713, 38);
		this.txtScheduleMtonDepartmentDetail.Name = "txtScheduleMtonDepartmentDetail";
		this.txtScheduleMtonDepartmentDetail.Size = new System.Drawing.Size(74, 23);
		this.txtScheduleMtonDepartmentDetail.TabIndex = 5;
		this.txtScheduleMtonDepartmentDetail.Tag = "NoOfBags";
		this.txtFclDepartmentDetail.Enabled = false;
		this.txtFclDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFclDepartmentDetail.Location = new System.Drawing.Point(637, 38);
		this.txtFclDepartmentDetail.Name = "txtFclDepartmentDetail";
		this.txtFclDepartmentDetail.Size = new System.Drawing.Size(74, 23);
		this.txtFclDepartmentDetail.TabIndex = 4;
		this.txtFclDepartmentDetail.Tag = "NoOfBags";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(479, 19);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(29, 15);
		this.label24.TabIndex = 65;
		this.label24.Text = "Port";
		this.CmbContractDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbContractDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance78;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance3).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail).Location = new System.Drawing.Point(3, 36);
		((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail).Name = "CmbContractDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail).TabIndex = 0;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(3, 19);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(71, 15);
		this.label25.TabIndex = 65;
		this.label25.Text = "Contract No";
		this.CmbPlantDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantDepartmentDetail).Location = new System.Drawing.Point(232, 81);
		((System.Windows.Forms.Control)(object)this.CmbPlantDepartmentDetail).Name = "CmbPlantDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbPlantDepartmentDetail).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantDepartmentDetail).TabIndex = 11;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(232, 64);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(34, 15);
		this.label34.TabIndex = 65;
		this.label34.Text = "Plant";
		this.CmbScheduleNoDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbScheduleNoDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbScheduleNoDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbScheduleNoDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbScheduleNoDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail).Location = new System.Drawing.Point(140, 36);
		((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail).Name = "CmbScheduleNoDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail).TabIndex = 1;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(140, 19);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(75, 15);
		this.label26.TabIndex = 65;
		this.label26.Text = "Schedule No";
		this.panel29.BackColor = System.Drawing.Color.Teal;
		this.panel29.Controls.Add(this.toolStrip5);
		this.panel29.Controls.Add(this.label27);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel29.Location = new System.Drawing.Point(3, 3);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1356, 50);
		this.panel29.TabIndex = 6;
		this.toolStrip5.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.BtnNewDepartmentDetail, this.BtnRefreshDepartmentDetail, this.BtnShortCutkeys });
		this.toolStrip5.Location = new System.Drawing.Point(0, 0);
		this.toolStrip5.Name = "toolStrip5";
		this.toolStrip5.Size = new System.Drawing.Size(1356, 27);
		this.toolStrip5.TabIndex = 55;
		this.toolStrip5.Text = "toolStrip5";
		this.BtnNewDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewDepartmentDetail.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewDepartmentDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewDepartmentDetail.Name = "BtnNewDepartmentDetail";
		this.BtnNewDepartmentDetail.Size = new System.Drawing.Size(59, 24);
		this.BtnNewDepartmentDetail.Text = "&New";
		this.BtnNewDepartmentDetail.Click += new System.EventHandler(BtnNewDepartmentDetail_Click);
		this.BtnRefreshDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshDepartmentDetail.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshDepartmentDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshDepartmentDetail.Name = "BtnRefreshDepartmentDetail";
		this.BtnRefreshDepartmentDetail.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshDepartmentDetail.Text = "&Refresh";
		this.BtnRefreshDepartmentDetail.Click += new System.EventHandler(BtnRefreshDepartmentDetail_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(6, 30);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(261, 17);
		this.label27.TabIndex = 54;
		this.label27.Text = "Pre Shipment Planning and FollowUp";
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(148, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1370, 27);
		this.miniToolStrip.TabIndex = 55;
		this.tabControlMain.Controls.Add(this.tabPageDepartmentStatus);
		this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControlMain.Location = new System.Drawing.Point(0, 0);
		this.tabControlMain.Name = "tabControlMain";
		this.tabControlMain.SelectedIndex = 0;
		this.tabControlMain.Size = new System.Drawing.Size(1370, 480);
		this.tabControlMain.TabIndex = 1;
		this.tabControlMain.SelectedIndexChanged += new System.EventHandler(tabControlMain_SelectedIndexChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1370, 480);
		base.Controls.Add(this.tabControlMain);
		base.KeyPreview = true;
		base.Name = "frmDepartmentStatusReport";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmSaleContractSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmDepartmentStatusReport_KeyDown);
		this.tabPageDepartmentStatus.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDepartmentDetail).EndInit();
		this.panel28.ResumeLayout(false);
		this.panel28.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerDepartmentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleLockStatusDepartmentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemDepartmentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPortDepartmentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractDepartmentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantDepartmentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleNoDepartmentDetail).EndInit();
		this.panel29.ResumeLayout(false);
		this.panel29.PerformLayout();
		this.toolStrip5.ResumeLayout(false);
		this.toolStrip5.PerformLayout();
		this.tabControlMain.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
