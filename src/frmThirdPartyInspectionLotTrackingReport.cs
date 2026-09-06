// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Export;
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
namespace Architecture.WinApp.ExportReports
{

public class frmThirdPartyInspectionLotTrackingReport : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtFromDb = new DataTable();

	private DataTable dtCol = new DataTable();

	private DataTable dtCards = new DataTable();

	private DataTable dtTrack = new DataTable();

	private DataTable dt = new DataTable();

	private bool FormInitialized;

	private IContainer components;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKeys;

	private Panel panel1;

	private Panel panel2;

	internal UltraCombo cmbsupplierCustomer;

	private Label label7;

	private Label label8;

	internal UltraCombo CmbItemName;

	private Panel panel8;

	private Label label9;

	private Panel panel3;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private GroupBox groupBox1;

	private ToolStripButton btnPrintCurrent;

	internal UltraCombo CmbLotTrackingNo;

	private Label label2;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	internal GridEX grd;

	internal GridEX grdEtaDestination;

	private ToolStripButton BtnThirdPartyInspectionForm;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton BtnLabAgainstThirdPartyInspectionForm;

	private RadioButton RadNotReffered;

	private RadioButton RadReffered;

	private RadioButton RadAll;

	private Label label26;

	private CheckBox chkSkipZero;

	private Label label37;

	internal UltraCombo CmbDateType;

	private DateTimePicker datToDate;

	private DateTimePicker datFromDate;

	private Label label3;

	private RadioButton radioButton1;

	public frmThirdPartyInspectionLotTrackingReport(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmThirdPartyInspectionLotTrackingReport_Load(object sender, EventArgs e)
	{
		try
		{
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 550);
			FormInitialized = true;
			dtTrack.Columns.Add("Id", typeof(int));
			dtTrack.Columns.Add("LotRefNo", typeof(string));
			dtTrack.Columns.Add("LodgeDate", typeof(DateTime));
			dtTrack.Columns.Add("ItemName", typeof(string));
			dtTrack.Columns.Add("MTons", typeof(double));
			dtTrack.Columns.Add("FarmingNTrade", typeof(string));
			if (dtCards.Columns.Count == 0)
			{
				dtCards.Columns.Add("Id", typeof(int));
				dtCards.Columns.Add("LotInspectionStage");
				dtCards.Columns.Add("NoOfLots", typeof(int));
				dtCards.Columns.Add("MTons", typeof(double));
			}
			DateTypeFill();
			AddCardsRows();
			TrackingFillFromSource();
			TrackingBind();
			AllcomboBind(AllcomboDbCall());
			CmbDateType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TrackingFillFromSource()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			dtTrack.Clear();
			DataTable dt = InvLabPreProductionExportLotInspectionHeader.GetData_LotInspectionNoForCombo(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				return;
			}
			foreach (DataRow row in dt.Rows)
			{
				dtTrack.Rows.Add(row["Id"], row["LotRefNo"], row["LodgeDate"], row["ItemName"], row["MTons"], row["FarmingNTrade"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable TackingNoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return InvLabPreProductionExportLotInspectionHeader.GetData_LotInspectionNoForCombo(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TrackingBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbLotTrackingNo.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbLotTrackingNo, dtTrack, "Id", "LotRefNo", "Tracking No", AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtTrack.Rows.Count > 0)
			{
				UltraGridBand obj = ((UltraGridBase)CmbLotTrackingNo).DisplayLayout.Bands[0];
				((HeaderBase)obj.Columns["LotRefNo"].Header).Caption = "Lot No";
				((HeaderBase)obj.Columns["ItemName"].Header).Caption = "Item Name";
				((HeaderBase)obj.Columns["FarmingNTrade"].Header).Caption = "Farming NTrade";
				((HeaderBase)obj.Columns["MTons"].Header).Caption = "M.Tons";
				((HeaderBase)obj.Columns["LodgeDate"].Header).Caption = "Lodge Date";
				obj.Columns["Id"].Hidden = true;
				obj.Columns["LotRefNo"].Width = 120;
				obj.Columns["ItemName"].Width = 200;
				obj.Columns["FarmingNTrade"].Width = 120;
				obj.Columns["MTons"].Width = 80;
				obj.Columns["LodgeDate"].Width = 120;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable AllcomboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return ExImLcOrder.GetDataForDropDownFromExportInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllcomboBind(DataTable dt)
	{
		try
		{
			int Id = Conversion.ToInt(cmbsupplierCustomer.Value);
			int Id2 = Conversion.ToInt(CmbItemName.Value);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("Customer");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Customer")
				{
					dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Items")
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
				}
			}
			DDL.BindDDL(dtSupplier, cmbsupplierCustomer, "Id", "Customer", "Customer", ZeroIndex: false);
			((UltraGridBase)cmbsupplierCustomer).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			CommonServices.SetComboValue(cmbsupplierCustomer, "Id", Id, dtSupplier);
			DDL.BindDDL(dtItem, CmbItemName, "Id", "ItemName", "ItemName", ZeroIndex: false);
			((UltraGridBase)CmbItemName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			CommonServices.SetComboValue(CmbItemName, "Id", Id2, dtItem);
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
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, CmbDateType, "Id", "Parameters", "Date Type", ZeroIndex: false);
			((UltraGridBase)CmbDateType).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Gridfill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0d62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6c: Expected O, but got Unknown
		try
		{
			dtFromDb.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			if (datFromDate.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)datFromDate.Value);
			}
			if (datToDate.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)datToDate.Value);
			}
			obj.SupplierCustomerId = Conversion.ToInt(cmbsupplierCustomer.Value);
			obj.ItemId = Conversion.ToInt(CmbItemName.Value);
			obj.RecId = Conversion.ToInt(CmbLotTrackingNo.Value);
			if (RadReffered.Checked)
			{
				obj.ActionId = 1;
			}
			else if (RadNotReffered.Checked)
			{
				obj.ActionId = 2;
			}
			else if (radioButton1.Checked)
			{
				obj.ActionId = 3;
			}
			obj.SkipZero = (chkSkipZero.Checked ? 1 : 0);
			dtFromDb = InvLabPreProductionExportLotInspectionHeader.ThirdPartyInspection_LotTrackingReport(obj);
			if (dtFromDb != null && dtFromDb.Rows.Count > 0)
			{
				if (dtCol.Columns.Count == 0)
				{
					dtCol.Columns.Add("Id", typeof(int));
					dtCol.Columns.Add("ExImLcOrderId", typeof(int));
					dtCol.Columns.Add("LotCodeTrackingNo", typeof(string));
					dtCol.Columns.Add("ItemName", typeof(string));
					dtCol.Columns.Add("InspectionAgency", typeof(string));
					dtCol.Columns.Add("TradeType", typeof(string));
					dtCol.Columns.Add("CustomerName", typeof(string));
					dtCol.Columns.Add("LcOrderNo", typeof(string));
					dtCol.Columns.Add("ScheduleCode", typeof(string));
					dtCol.Columns.Add("CustomerContractNo", typeof(string));
					dtCol.Columns.Add("InvoiceNo", typeof(string));
					dtCol.Columns.Add("LoadingScheduleDate", typeof(DateTime));
					dtCol.Columns.Add("InspectionScheduleDate", typeof(DateTime));
					dtCol.Columns.Add("MTons", typeof(double));
					dtCol.Columns.Add("LotMTons", typeof(double));
					dtCol.Columns.Add("UsedMTons", typeof(double));
					dtCol.Columns.Add("BalMTons", typeof(double));
					dtCol.Columns.Add("ContractScheduleId", typeof(int));
					dtCol.Columns.Add("InvoiceId", typeof(string));
					dtCol.Columns.Add("SupplierCustomerId", typeof(int));
					dtCol.Columns.Add("ExporterLotRefNo", typeof(string));
					dtCol.Columns.Add("ItemId", typeof(int));
					dtCol.Columns.Add("SamplingResponsibility", typeof(string));
					dtCol.Columns.Add("ProcessStatus", typeof(string));
					dtCol.Columns.Add("StockReservedDate", typeof(DateTime));
					dtCol.Columns.Add("SampleTakenDate", typeof(DateTime));
					dtCol.Columns.Add("StockSealedDate", typeof(DateTime));
					dtCol.Columns.Add("SampleDispatchedDate", typeof(DateTime));
					dtCol.Columns.Add("CourierNo", typeof(string));
					dtCol.Columns.Add("LabCountry", typeof(string));
					dtCol.Columns.Add("SampleETADestination", typeof(DateTime));
					dtCol.Columns.Add("SampleATADestination", typeof(DateTime));
					dtCol.Columns.Add("ReportReferenceNo", typeof(string));
					dtCol.Columns.Add("ReportDate", typeof(DateTime));
					dtCol.Columns.Add("DateOfInspection", typeof(DateTime));
					dtCol.Columns.Add("RequiredAnalysis", typeof(string));
					dtCol.Columns.Add("ResultStatus", typeof(string));
					dtCol.Columns.Add("ResultRemarks", typeof(string));
					dtCol.Columns.Add("SampleStatus", typeof(string));
					dtCol.Columns.Add("InstructionsOrRemarks", typeof(string));
				}
				dtCol.Clear();
				foreach (DataRow row2 in dtFromDb.Rows)
				{
					DataRow newRow = dtCol.NewRow();
					newRow["Id"] = Conversion.ToInt(row2["Id"]);
					newRow["ExImLcOrderId"] = Conversion.ToInt(row2["ExImLcOrderId"]);
					newRow["LotCodeTrackingNo"] = Conversion.ToString(row2["LotCodeTrackingNo"]);
					newRow["ItemName"] = Conversion.ToString(row2["ItemName"]);
					newRow["InspectionAgency"] = Conversion.ToString(row2["InspectionAgency"]);
					newRow["TradeType"] = Conversion.ToString(row2["TradeType"]);
					newRow["CustomerName"] = Conversion.ToString(row2["CustomerName"]);
					newRow["LcOrderNo"] = Conversion.ToString(row2["LcOrderNo"]);
					newRow["ScheduleCode"] = Conversion.ToString(row2["ScheduleCode"]);
					newRow["CustomerContractNo"] = Conversion.ToString(row2["CustomerContractNo"]);
					newRow["InvoiceNo"] = Conversion.ToString(row2["ExImInvoiceNo"]);
					newRow["LoadingScheduleDate"] = row2["LoadingScheduleDate"];
					newRow["InspectionScheduleDate"] = row2["InspectionScheduleDate"];
					newRow["MTons"] = Conversion.ToDouble(row2["MTons"]);
					newRow["LotMTons"] = Conversion.ToDouble(row2["LotMTons"]);
					newRow["UsedMTons"] = Conversion.ToDouble(row2["UsedMTons"]);
					newRow["BalMTons"] = Conversion.ToDouble(row2["BalanceMTon"]);
					newRow["ContractScheduleId"] = Conversion.ToInt(row2["ContractScheduleId"]);
					newRow["InvoiceId"] = Conversion.ToInt(row2["ExImInvoiceId"]);
					newRow["SupplierCustomerId"] = Conversion.ToInt(row2["SupplierCustomerId"]);
					newRow["ExporterLotRefNo"] = Conversion.ToString(row2["ExporterLotRefNo"]);
					newRow["ItemId"] = Conversion.ToInt(row2["ItemId"]);
					newRow["SamplingResponsibility"] = Conversion.ToString(row2["SamplingResponsibility"]);
					newRow["ProcessStatus"] = Conversion.ToString(row2["ProcessStatus"]);
					newRow["StockReservedDate"] = row2["StockReservedDate"];
					newRow["SampleTakenDate"] = row2["SampleTakenDate"];
					newRow["StockSealedDate"] = row2["StockSealedDate"];
					newRow["SampleDispatchedDate"] = row2["SampleDispatchedDate"];
					newRow["CourierNo"] = Conversion.ToString(row2["CourierNo"]);
					newRow["LabCountry"] = Conversion.ToString(row2["LabCountry"]);
					newRow["SampleETADestination"] = row2["SampleETADestination"];
					newRow["SampleATADestination"] = row2["SampleATADestination"];
					newRow["ReportReferenceNo"] = Conversion.ToString(row2["ReportReferenceNo"]);
					newRow["ReportDate"] = row2["ReportDate"];
					newRow["DateOfInspection"] = row2["DateOfInspection"];
					newRow["RequiredAnalysis"] = Conversion.ToString(row2["RequiredAnalysis"]);
					newRow["ResultStatus"] = Conversion.ToString(row2["ResultStatus"]);
					newRow["ResultRemarks"] = Conversion.ToString(row2["ResultRemarks"]);
					newRow["SampleStatus"] = Conversion.ToString(row2["SampleStatus"]);
					newRow["InstructionsOrRemarks"] = Conversion.ToString(row2["InstructionsOrRemarks"]);
					dtCol.Rows.Add(newRow);
				}
				grd.DataSource = dtCol;
				grd.RetrieveStructure();
				grdsetting();
				dtCards.Clear();
				foreach (var rec in from row in dtFromDb.AsEnumerable()
					group row by Conversion.ToInt(row["LotCurrentStageId"]) into g
					select new
					{
						LotCurrentStageId = g.Key,
						Description = g.Select((DataRow r) => r["LotCurrentStage"]?.ToString()).FirstOrDefault(),
						NoOfLots = (from r in g
							where r["LotCodeTrackingNo"] != DBNull.Value
							select r["LotCodeTrackingNo"].ToString()).Distinct().Count(),
						MTons = g.Sum((DataRow r) => (r["MTons"] != DBNull.Value) ? Conversion.ToDouble(r["MTons"]) : 0.0)
					})
				{
					dtCards.Rows.Add(rec.LotCurrentStageId, rec.Description, rec.NoOfLots, rec.MTons);
				}
				AddCardsRows();
				grdEtaDestination.DataSource = dtCards;
				grdEtaDestination.RetrieveStructure();
				grdEtaDestination.RootTable.Columns["LotInspectionStage"].Width = 220;
				grdEtaDestination.RootTable.Columns["NoOfLots"].Width = 90;
				grdEtaDestination.RootTable.Columns["MTons"].Width = 90;
				grdEtaDestination.RootTable.Columns["Id"].Visible = false;
				grdEtaDestination.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
				grdEtaDestination.RootTable.Columns["MTons"].AggregateFunction = (AggregateFunction)2;
				grdEtaDestination.RootTable.Columns["MTons"].FormatString = "#,##.##";
				grdEtaDestination.RootTable.Columns["MTons"].TotalFormatString = "#,##.##";
				grdEtaDestination.RootTable.SortKeys.Add(new GridEXSortKey(grdEtaDestination.RootTable.Columns["Id"], (SortOrder)1));
			}
			else
			{
				grd.DataSource = null;
				grdEtaDestination.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddCardsRows()
	{
		try
		{
			Dictionary<int, string> dictionary = (from r in ThirdPartyType.LotTrackingCurrentStage().AsEnumerable()
				where r["Id"] != DBNull.Value && r["Description"] != DBNull.Value
				select r).ToDictionary((DataRow r) => Conversion.ToInt(r["Id"]), (DataRow r) => r["Description"].ToString());
			HashSet<int> existingStages = new HashSet<int>(from row in dtCards.AsEnumerable()
				select row.Field<int>("Id") into id
				where id != 0
				select id);
			foreach (KeyValuePair<int, string> stage in dictionary)
			{
				if (!existingStages.Contains(stage.Key))
				{
					dtCards.Rows.Add(stage.Key, stage.Value, 0, 0);
				}
			}
			grdEtaDestination.DataSource = dtCards;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsetting()
	{
		try
		{
			grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["ContractScheduleId"].Visible = false;
			grd.RootTable.Columns["ExImLcOrderId"].Visible = false;
			grd.RootTable.Columns["InvoiceId"].Visible = false;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["LotMTons"].Caption = "Total MTons";
			grd.RootTable.Columns["MTons"].Caption = "Invoice MTons";
			grd.RootTable.Columns["BalMTons"].Caption = "Balance MTons";
			string[] array = new string[10] { "LoadingScheduleDate", "InspectionScheduleDate", "StockReservedDate", "SampleTakenDate", "StockSealedDate", "SampleDispatchedDate", "SampleETADestination", "SampleATADestination", "ReportDate", "DateOfInspection" };
			foreach (string col in array)
			{
				if (grd.RootTable.Columns.Contains(col))
				{
					grd.RootTable.Columns[col].FormatString = "dd-MMM-yy";
				}
			}
			List<string> NotTotalColumns = new List<string> { "LotMTons", "UsedMTons", "BalMTons" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 0, null, null, null, ColumnSetting: true, BracketsForamt: false, NotTotalColumns);
			grd.RootTable.Columns["LcOrderNo"].Caption = "Contract No";
			grd.RootTable.Columns["SampleETADestination"].Caption = "Sample ETA Destination";
			grd.RootTable.Columns["SampleATADestination"].Caption = "Sample ATA Destination";
			grd.RootTable.Columns.Add("Inspection");
			grd.RootTable.Columns["Inspection"].Key = "Inspection";
			grd.RootTable.Columns["Inspection"].Caption = "Inspection";
			grd.RootTable.Columns["Inspection"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Inspection"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Inspection"].ButtonText = "Inspection";
			grd.RootTable.Columns["Inspection"].Width = 70;
			grd.RootTable.Columns["Inspection"].Position = 0;
			grd.RootTable.Columns.Add("Lab");
			grd.RootTable.Columns["Lab"].Key = "Lab";
			grd.RootTable.Columns["Lab"].Caption = "Lab";
			grd.RootTable.Columns["Lab"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Lab"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Lab"].ButtonText = "Lab";
			grd.RootTable.Columns["Lab"].Width = 40;
			grd.RootTable.Columns["Lab"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			Gridfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Column.Key == "Inspection")
			{
				frmThirdPartyInspection obj2 = new frmThirdPartyInspection(UserAccount);
				if (Id > 0)
				{
					obj2.ReadById(Id);
				}
				obj2.BringToFront();
				obj2.ShowDialog();
			}
			if (e.Column.Key == "Lab")
			{
				frmLabAgainstThirdPartyInspection obj = new frmLabAgainstThirdPartyInspection(UserAccount);
				if (Id > 0)
				{
					obj.ReadById(Id);
				}
				obj.BringToFront();
				obj.ShowDialog();
			}
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
			((Control)(object)CmbLotTrackingNo).Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)cmbsupplierCustomer).Text = string.Empty;
			CmbLotTrackingNo.Focus();
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
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmThirdPartyInspectionLotTrackingReport_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			TrackingFillFromSource();
			TrackingBind();
			AllcomboBind(AllcomboDbCall());
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
			if (base.ActiveControl != null && base.ActiveControl.Name == "ctrlGrdBar1")
			{
				CmbLotTrackingNo.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmThirdPartyInspectionLotTrackingReport_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrintCurrent_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbsupplierCustomer.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbsupplierCustomer.Focus();
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
			dt.Columns.Add("Lot Inspection Stage");
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Supplier Customer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Supplier Customer in Filter");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtCol);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintCurrent_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtFromDb == null || dtFromDb.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtFromDb, "514_01_ThirdPartyInspectionLotTrackingReport.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnThirdPartyInspectionForm_Click(object sender, EventArgs e)
	{
		try
		{
			frmThirdPartyInspection frmThirdPartyInspection = new frmThirdPartyInspection(UserAccount);
			frmThirdPartyInspection.BringToFront();
			frmThirdPartyInspection.ShowDialog();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLabAgainstThirdPartyInspectionForm_Click(object sender, EventArgs e)
	{
		try
		{
			frmLabAgainstThirdPartyInspection frmLabAgainstThirdPartyInspection = new frmLabAgainstThirdPartyInspection(UserAccount);
			frmLabAgainstThirdPartyInspection.BringToFront();
			frmLabAgainstThirdPartyInspection.ShowDialog();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDateType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(CmbDateType.Value) == 1)
			{
				datFromDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 2)
			{
				datFromDate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				datFromDate.Value = first;
				datToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				datFromDate.Value = first2;
				datToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 5)
			{
				datFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
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
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_0a48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a52: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.frmThirdPartyInspectionLotTrackingReport));
		GridEXLayout grdEtaDestination_DesignTimeLayout = new GridEXLayout();
		Appearance appearance9 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
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
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrintCurrent = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnThirdPartyInspectionForm = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLabAgainstThirdPartyInspectionForm = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdEtaDestination = new GridEX();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.radioButton1 = new System.Windows.Forms.RadioButton();
		this.chkSkipZero = new System.Windows.Forms.CheckBox();
		this.label3 = new System.Windows.Forms.Label();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.RadReffered = new System.Windows.Forms.RadioButton();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.RadNotReffered = new System.Windows.Forms.RadioButton();
		this.label37 = new System.Windows.Forms.Label();
		this.CmbDateType = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.CmbLotTrackingNo = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbsupplierCustomer = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbItemName = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.toolStrip3.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEtaDestination).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLotTrackingNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplierCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.panel8.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.toolStripSeparator5, this.btnNew, this.btnRefresh, this.toolStripSeparator4, this.btnPrintCurrent, this.toolStripSeparator3, this.BtnThirdPartyInspectionForm, this.toolStripSeparator1, this.BtnLabAgainstThirdPartyInspectionForm, this.btnShortCutKeys,
			this.toolStripSeparator2
		});
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1087, 27);
		this.toolStrip3.TabIndex = 1;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnPrintCurrent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintCurrent.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintCurrent.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintCurrent.Name = "btnPrintCurrent";
		this.btnPrintCurrent.Size = new System.Drawing.Size(116, 24);
		this.btnPrintCurrent.Text = "514_01-Print";
		this.btnPrintCurrent.ToolTipText = "Print";
		this.btnPrintCurrent.Click += new System.EventHandler(btnPrintCurrent_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.BtnThirdPartyInspectionForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnThirdPartyInspectionForm.Image = Architecture.WinApp.Properties.Resources.Export_Reports;
		this.BtnThirdPartyInspectionForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnThirdPartyInspectionForm.Name = "BtnThirdPartyInspectionForm";
		this.BtnThirdPartyInspectionForm.Size = new System.Drawing.Size(178, 24);
		this.BtnThirdPartyInspectionForm.Text = "Third Party Inspection";
		this.BtnThirdPartyInspectionForm.Click += new System.EventHandler(BtnThirdPartyInspectionForm_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnLabAgainstThirdPartyInspectionForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLabAgainstThirdPartyInspectionForm.Image = Architecture.WinApp.Properties.Resources.Export_Reports;
		this.BtnLabAgainstThirdPartyInspectionForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLabAgainstThirdPartyInspectionForm.Name = "BtnLabAgainstThirdPartyInspectionForm";
		this.BtnLabAgainstThirdPartyInspectionForm.Size = new System.Drawing.Size(260, 24);
		this.BtnLabAgainstThirdPartyInspectionForm.Text = "Lab Against Third Party Inspection";
		this.BtnLabAgainstThirdPartyInspectionForm.Click += new System.EventHandler(BtnLabAgainstThirdPartyInspectionForm_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1087, 434);
		this.panel1.TabIndex = 2;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		grd_DesignTimeLayout.LayoutString = resources.GetString("grd_DesignTimeLayout.LayoutString");
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 286);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1087, 148);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 261);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1087, 25);
		this.panel3.TabIndex = 129;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1043, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 22);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 2);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(142, 19);
		this.label1.TabIndex = 1;
		this.label1.Text = "Filtered Records";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdEtaDestination);
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 30);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1087, 231);
		this.panel2.TabIndex = 0;
		this.grdEtaDestination.AllowEdit = (InheritableBoolean)2;
		this.grdEtaDestination.AlternatingColors = true;
		grdEtaDestination_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEtaDestination.DesignTimeLayout = grdEtaDestination_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Dock = System.Windows.Forms.DockStyle.Left;
		this.grdEtaDestination.DynamicFiltering = true;
		this.grdEtaDestination.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEtaDestination.GroupByBoxVisible = false;
		this.grdEtaDestination.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Location = new System.Drawing.Point(487, 0);
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Name = "grdEtaDestination";
		((ScrollableBase)this.grdEtaDestination).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Size = new System.Drawing.Size(412, 231);
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).TabIndex = 1;
		this.grdEtaDestination.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEtaDestination).VisualStyle = (VisualStyle)4;
		this.groupBox1.Controls.Add(this.radioButton1);
		this.groupBox1.Controls.Add(this.chkSkipZero);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.datToDate);
		this.groupBox1.Controls.Add(this.RadReffered);
		this.groupBox1.Controls.Add(this.datFromDate);
		this.groupBox1.Controls.Add(this.RadAll);
		this.groupBox1.Controls.Add(this.RadNotReffered);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateType);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(487, 231);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.radioButton1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.radioButton1.AutoSize = true;
		this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButton1.ForeColor = System.Drawing.Color.Black;
		this.radioButton1.Location = new System.Drawing.Point(177, 203);
		this.radioButton1.Name = "radioButton1";
		this.radioButton1.Size = new System.Drawing.Size(143, 19);
		this.radioButton1.TabIndex = 9;
		this.radioButton1.Text = "Rejected && Cancel";
		this.radioButton1.UseVisualStyleBackColor = true;
		this.chkSkipZero.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkSkipZero.AutoSize = true;
		this.chkSkipZero.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSkipZero.ForeColor = System.Drawing.Color.Black;
		this.chkSkipZero.Location = new System.Drawing.Point(388, 177);
		this.chkSkipZero.Name = "chkSkipZero";
		this.chkSkipZero.Size = new System.Drawing.Size(87, 19);
		this.chkSkipZero.TabIndex = 10;
		this.chkSkipZero.Text = "Skip Zero";
		this.chkSkipZero.UseVisualStyleBackColor = true;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(124, 15);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(252, 19);
		this.label3.TabIndex = 529;
		this.label3.Text = "Requested Lodge Date For Lot Code";
		this.datToDate.Checked = false;
		this.datToDate.CustomFormat = "dd-MMM-yyyy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(300, 66);
		this.datToDate.Name = "datToDate";
		this.datToDate.ShowCheckBox = true;
		this.datToDate.Size = new System.Drawing.Size(174, 23);
		this.datToDate.TabIndex = 2;
		this.RadReffered.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.RadReffered.AutoSize = true;
		this.RadReffered.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadReffered.ForeColor = System.Drawing.Color.Black;
		this.RadReffered.Location = new System.Drawing.Point(177, 177);
		this.RadReffered.Name = "RadReffered";
		this.RadReffered.Size = new System.Drawing.Size(159, 19);
		this.RadReffered.TabIndex = 8;
		this.RadReffered.Text = "Approved && Accepted";
		this.RadReffered.UseVisualStyleBackColor = true;
		this.datFromDate.Checked = false;
		this.datFromDate.CustomFormat = "dd-MMM-yyyy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(124, 66);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.ShowCheckBox = true;
		this.datFromDate.Size = new System.Drawing.Size(174, 23);
		this.datFromDate.TabIndex = 1;
		this.RadAll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadAll.ForeColor = System.Drawing.Color.Black;
		this.RadAll.Location = new System.Drawing.Point(15, 177);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(41, 19);
		this.RadAll.TabIndex = 6;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.RadNotReffered.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.RadNotReffered.AutoSize = true;
		this.RadNotReffered.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNotReffered.ForeColor = System.Drawing.Color.Black;
		this.RadNotReffered.Location = new System.Drawing.Point(15, 203);
		this.RadNotReffered.Name = "RadNotReffered";
		this.RadNotReffered.Size = new System.Drawing.Size(157, 19);
		this.RadNotReffered.TabIndex = 7;
		this.RadNotReffered.Text = "Pending && Inprocess";
		this.RadNotReffered.UseVisualStyleBackColor = true;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(12, 43);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(60, 15);
		this.label37.TabIndex = 526;
		this.label37.Text = "Date Type";
		this.CmbDateType.AllowNull = (DefaultableBoolean)1;
		this.CmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance12;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateType).Location = new System.Drawing.Point(124, 37);
		((System.Windows.Forms.Control)(object)this.CmbDateType).Name = "CmbDateType";
		((System.Windows.Forms.Control)(object)this.CmbDateType).Size = new System.Drawing.Size(351, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbDateType).Leave += new System.EventHandler(CmbDateType_Leave);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(10, 70);
		this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(91, 15);
		this.label26.TabIndex = 524;
		this.label26.Text = "From && To Date";
		this.CmbLotTrackingNo.AllowNull = (DefaultableBoolean)1;
		this.CmbLotTrackingNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLotTrackingNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLotTrackingNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).Location = new System.Drawing.Point(124, 144);
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).Name = "CmbLotTrackingNo";
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).Size = new System.Drawing.Size(350, 26);
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).TabIndex = 5;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(10, 150);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(91, 15);
		this.label2.TabIndex = 129;
		this.label2.Text = "Lot Tracking No";
		this.cmbsupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).Location = new System.Drawing.Point(124, 90);
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).Name = "cmbsupplierCustomer";
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).Size = new System.Drawing.Size(350, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).TabIndex = 3;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(388, 199);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(87, 26);
		this.btnshow.TabIndex = 11;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(124, 117);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(350, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 4;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(10, 96);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(58, 15);
		this.label8.TabIndex = 125;
		this.label8.Text = "Customer";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(10, 123);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 127;
		this.label7.Text = "Item Name";
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1087, 30);
		this.panel8.TabIndex = 123;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(7, 5);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(359, 19);
		this.label9.TabIndex = 1;
		this.label9.Text = "Third Party Inspection Lot Tracking Report";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1087, 461);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.toolStrip3);
		base.KeyPreview = true;
		base.Name = "frmThirdPartyInspectionLotTrackingReport";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmThirdPartyInspectionLotTrackingReport_FormClosed);
		base.Load += new System.EventHandler(frmThirdPartyInspectionLotTrackingReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmThirdPartyInspectionLotTrackingReport_KeyDown);
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEtaDestination).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLotTrackingNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplierCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
