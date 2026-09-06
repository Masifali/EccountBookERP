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
namespace Architecture.WinApp.ApprovalDashboard
{

public class frmThirdPartyLotInspectionApproval : Form
{
	private bool FormInitialized;

	private DataTable dtCol = new DataTable();

	private DataTable dtCards = new DataTable();

	private DataTable dtFromDb = new DataTable();

	private DataTable dtDetailForLayout = new DataTable();

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

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

	private DateTimePicker datToDate;

	private DateTimePicker datFromDate;

	private ToolStripButton btnApprove;

	private Panel panel4;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label3;

	internal GridEX grdDetail;

	private RadioButton radRejectedStatus;

	private RadioButton radAwaitingStatus;

	private RadioButton radAcceptedStatus;

	private RadioButton radAllStatus;

	private Label label4;

	private Panel panel5;

	public frmThirdPartyLotInspectionApproval(UserAccount user)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmThirdPartyInspectionLotTrackingReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 550);
			FormInitialized = true;
			if (dtCards.Columns.Count == 0)
			{
				dtCards.Columns.Add("Id", typeof(int));
				dtCards.Columns.Add("Description");
				dtCards.Columns.Add("NoOfLots", typeof(int));
				dtCards.Columns.Add("MTons", typeof(double));
			}
			radAllStatus.Checked = true;
			AddCardsRows();
			TackingNoBind(TackingNoDbCall());
			AllcomboBind(AllcomboDbCall());
			cmbsupplierCustomer.Focus();
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			return InvLabPreProductionExportLotInspectionHeader.GetDataForDropDownFrom_LabPreExportLotInspection(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "TrackingNo"
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TackingNoBind(DataTable dt)
	{
		try
		{
			int Id = Conversion.ToInt(CmbLotTrackingNo.Value);
			if (dt.Rows.Count > 0)
			{
				DataTable dtTrack = new DataTable();
				dtTrack.Columns.Add("Id", typeof(int));
				dtTrack.Columns.Add("name", typeof(string));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtTrack.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
				}
				DDL.BindDDL(dtTrack, CmbLotTrackingNo, "Id", "name", "TrackingNo", ZeroIndex: false);
				CommonServices.SetComboValue(CmbLotTrackingNo, "Id", Id, dtTrack);
			}
			else
			{
				((UltraGridBase)CmbLotTrackingNo).DataSource = null;
				((Control)(object)CmbLotTrackingNo).Text = string.Empty;
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

	private void Gridfill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0c: Expected O, but got Unknown
		try
		{
			dtFromDb.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (datFromDate.Checked)
			{
				obj.FromDate = datFromDate.Value;
			}
			obj.ToDate = datToDate.Value;
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
			if (radAwaitingStatus.Checked)
			{
				obj.Status = "Awaiting";
			}
			else if (radAcceptedStatus.Checked)
			{
				obj.Status = "Accepted";
			}
			else if (radRejectedStatus.Checked)
			{
				obj.Status = "Rejected";
			}
			HashSet<int> seenIds = new HashSet<int>();
			dtFromDb = InvLabPreProductionExportLotInspectionHeader.ThirdPartyInspectionData_ForApproval(obj);
			if (dtFromDb != null && dtFromDb.Rows.Count > 0)
			{
				if (dtCol.Columns.Count == 0)
				{
					dtCol.Columns.Add("Id", typeof(int));
					dtCol.Columns.Add("LoadingScheduleDate", typeof(DateTime));
					dtCol.Columns.Add("InspectionScheduleDate", typeof(DateTime));
					dtCol.Columns.Add("LotCodeTrackingNo", typeof(string));
					dtCol.Columns.Add("ExporterLotRefNo", typeof(string));
					dtCol.Columns.Add("ItemId", typeof(int));
					dtCol.Columns.Add("ItemName", typeof(string));
					dtCol.Columns.Add("MTons", typeof(double));
					dtCol.Columns.Add("UsedMTons", typeof(double));
					dtCol.Columns.Add("BalanceMTons", typeof(double));
					dtCol.Columns.Add("InspectionAgency", typeof(string));
					dtCol.Columns.Add("SamplingResponsibility", typeof(string));
					dtCol.Columns.Add("ProcessStatus", typeof(string));
					dtCol.Columns.Add("StockReservedDate", typeof(DateTime));
					dtCol.Columns.Add("SampleTakenDate", typeof(DateTime));
					dtCol.Columns.Add("StockSealedDate", typeof(DateTime));
					dtCol.Columns.Add("SampleDispatchedDate", typeof(DateTime));
					dtCol.Columns.Add("CourierNo", typeof(string));
					dtCol.Columns.Add("LabCountry", typeof(string));
					dtCol.Columns.Add("TradeType", typeof(string));
					dtCol.Columns.Add("SampleETADestination", typeof(DateTime));
					dtCol.Columns.Add("SampleATADestination", typeof(DateTime));
					dtCol.Columns.Add("ReportReferenceNo", typeof(string));
					dtCol.Columns.Add("ReportDate", typeof(DateTime));
					dtCol.Columns.Add("DateOfInspection", typeof(DateTime));
					dtCol.Columns.Add("RequiredAnalysis", typeof(string));
					dtCol.Columns.Add("ResultStatus", typeof(string));
					dtCol.Columns.Add("ResultRemarks", typeof(string));
					dtCol.Columns.Add("SampleStatus", typeof(string));
				}
				dtCol.Clear();
				foreach (DataRow row2 in dtFromDb.Rows)
				{
					int id = Conversion.ToInt(row2["Id"]);
					if (seenIds.Add(id))
					{
						DataRow newRow = dtCol.NewRow();
						newRow["Id"] = id;
						newRow["LoadingScheduleDate"] = row2["LoadingScheduleDate"];
						newRow["InspectionScheduleDate"] = row2["InspectionScheduleDate"];
						newRow["LotCodeTrackingNo"] = Conversion.ToString(row2["LotCodeTrackingNo"]);
						newRow["ExporterLotRefNo"] = Conversion.ToString(row2["ExporterLotRefNo"]);
						newRow["ItemId"] = Conversion.ToInt(row2["ItemId"]);
						newRow["ItemName"] = Conversion.ToString(row2["ItemName"]);
						newRow["MTons"] = Conversion.ToDouble(row2["MTons"]);
						newRow["UsedMTons"] = Conversion.ToDouble(row2["UsedMTons"]);
						newRow["BalanceMTons"] = Conversion.ToDouble(row2["BalanceMTons"]);
						newRow["InspectionAgency"] = Conversion.ToString(row2["InspectionAgency"]);
						newRow["SamplingResponsibility"] = Conversion.ToString(row2["SamplingResponsibility"]);
						newRow["ProcessStatus"] = Conversion.ToString(row2["ProcessStatus"]);
						newRow["StockReservedDate"] = row2["StockReservedDate"];
						newRow["SampleTakenDate"] = row2["SampleTakenDate"];
						newRow["StockSealedDate"] = row2["StockSealedDate"];
						newRow["SampleDispatchedDate"] = row2["SampleDispatchedDate"];
						newRow["CourierNo"] = Conversion.ToString(row2["CourierNo"]);
						newRow["LabCountry"] = Conversion.ToString(row2["LabCountry"]);
						newRow["TradeType"] = Conversion.ToString(row2["TradeType"]);
						newRow["SampleETADestination"] = row2["SampleETADestination"];
						newRow["SampleATADestination"] = row2["SampleATADestination"];
						newRow["ReportReferenceNo"] = Conversion.ToString(row2["ReportReferenceNo"]);
						newRow["ReportDate"] = row2["ReportDate"];
						newRow["DateOfInspection"] = row2["DateOfInspection"];
						newRow["RequiredAnalysis"] = Conversion.ToString(row2["RequiredAnalysis"]);
						newRow["ResultStatus"] = Conversion.ToString(row2["ResultStatus"]);
						newRow["ResultRemarks"] = Conversion.ToString(row2["ResultRemarks"]);
						newRow["SampleStatus"] = Conversion.ToString(row2["SampleStatus"]);
						dtCol.Rows.Add(newRow);
					}
				}
				grd.DataSource = dtCol;
				grd.RetrieveStructure();
				grdsetting();
				dtCards.Clear();
				foreach (var rec in from row in dtFromDb.AsEnumerable()
					group row by row["LotCurrentStageId"] into g
					select new
					{
						LotCurrentStageId = g.Key,
						Description = g.First()["LotCurrentStage"]?.ToString(),
						NoOfLots = g.Count(),
						MTons = g.Sum(delegate(DataRow r)
						{
							object obj2 = r["MTons"];
							if (obj2 == DBNull.Value || obj2 == null)
							{
								return 0.0;
							}
							double result;
							return (!double.TryParse(obj2.ToString(), out result)) ? 0.0 : result;
						})
					})
				{
					dtCards.Rows.Add(rec.LotCurrentStageId, rec.Description, rec.NoOfLots, rec.MTons);
				}
				AddCardsRows();
				grdEtaDestination.DataSource = dtCards;
				grdEtaDestination.RetrieveStructure();
				grdEtaDestination.RootTable.Columns["Description"].Width = 220;
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
			Dictionary<int, string> obj = new Dictionary<int, string>
			{
				{ 1, "Pending For Inspection" },
				{ 2, "Sampling & Sealing" },
				{ 3, "Sample Dispatch & Courier Transit" },
				{ 4, "Inspection & Result Awaiting" },
				{ 5, "Report & Result" }
			};
			HashSet<int> existingStages = new HashSet<int>(from row in dtCards.AsEnumerable()
				select row.Field<int>("Id") into id
				where id != 0
				select id);
			foreach (KeyValuePair<int, string> stage in obj)
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
			string[] array = new string[10] { "LoadingScheduleDate", "InspectionScheduleDate", "StockReservedDate", "SampleTakenDate", "StockSealedDate", "SampleDispatchedDate", "SampleETADestination", "SampleATADestination", "ReportDate", "DateOfInspection" };
			foreach (string col in array)
			{
				if (grd.RootTable.Columns.Contains(col))
				{
					grd.RootTable.Columns[col].FormatString = "dd-MMM-yy";
				}
			}
			CommonServices.GridWrappingAndColumnSettings(grd, 3);
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
			GridEX_Helper.AddButton(grd, "Approve", "Approve", 60, 2);
			GridEX_Helper.AddSelectButton(grd, "Select", 30, 3);
			grd.FrozenColumns = 4;
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
				frmThirdPartyInspection obj = new frmThirdPartyInspection(UserAccount);
				if (Id > 0)
				{
					obj.ReadById(Id);
				}
				obj.BringToFront();
				obj.ShowDialog();
			}
			else if (e.Column.Key == "Lab")
			{
				frmLabAgainstThirdPartyInspection obj2 = new frmLabAgainstThirdPartyInspection(UserAccount);
				if (Id > 0)
				{
					obj2.ReadById(Id);
				}
				obj2.BringToFront();
				obj2.ShowDialog();
			}
			else if (e.Column.Key == "Approve")
			{
				ApproveSingleRecord();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveSingleRecord()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve Record?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value);
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				InvLabPreProductionExportLotInspectionHeader.ThirdPartyLotApproval(val);
				MessageBox.Show("Record Approve Successfully");
				Gridfill();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			TackingNoBind(TackingNoDbCall());
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
			if (e.Control && e.Shift && e.KeyCode == Keys.A)
			{
				btnApprove_Click(null, null);
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
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Supplier Customer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Supplier Customer in Filter");
			dt.Rows.Add("Ctrl+Shift+A", "For Approve Multi Records");
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

	private void btnApprove_Click(object sender, EventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select row's first");
			}
			if (MessageBox.Show("Are you sure to approve selected records?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.EntryUser = UserAccount.ID;
					obj.AllApprovalLists.Add(item);
				}
				InvLabPreProductionExportLotInspectionHeader.ThirdPartyLotApproval(obj);
				MessageBox.Show("Record's Approve Successfully");
				Gridfill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("ExImLcOrderId", typeof(int));
			dt.Columns.Add("LcOrderNo", typeof(string));
			dt.Columns.Add("ContractScheduleId", typeof(int));
			dt.Columns.Add("ScheduleCode", typeof(string));
			dt.Columns.Add("CustomerContractNo", typeof(string));
			dt.Columns.Add("InvoiceId", typeof(string));
			dt.Columns.Add("InvoiceNo", typeof(string));
			dt.Columns.Add("SupplierCustomerId", typeof(int));
			dt.Columns.Add("CustomerName", typeof(string));
			dt.Columns.Add("MTons", typeof(double));
			dtDetailForLayout = dt.Clone();
			foreach (DataRow row2 in (from row in dtFromDb.AsEnumerable()
				where row.Field<int>("Id") == Conversion.ToInt(r.Cells["Id"].Value)
				select row).CopyToDataTable().Rows)
			{
				DataRow newRow = dt.NewRow();
				newRow["ExImLcOrderId"] = Conversion.ToInt(row2["ExImLcOrderId"]);
				newRow["LcOrderNo"] = Conversion.ToString(row2["LcOrderNo"]);
				newRow["ContractScheduleId"] = Conversion.ToInt(row2["ContractScheduleId"]);
				newRow["ScheduleCode"] = Conversion.ToString(row2["ScheduleCode"]);
				newRow["CustomerContractNo"] = Conversion.ToString(row2["CustomerContractNo"]);
				newRow["SupplierCustomerId"] = Conversion.ToInt(row2["SupplierCustomerId"]);
				newRow["CustomerName"] = Conversion.ToString(row2["CustomerName"]);
				newRow["InvoiceId"] = Conversion.ToInt(row2["ExImInvoiceId"]);
				newRow["InvoiceNo"] = Conversion.ToString(row2["ExImInvoiceNo"]);
				newRow["MTons"] = Conversion.ToDouble(row2["DetailMTons"]);
				dt.Rows.Add(newRow);
			}
			dtDetailForLayout = dt.Copy();
			grdDetail.DataSource = dt;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			grdDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grdDetail.RootTable.Columns["ContractScheduleId"].Visible = false;
			grdDetail.RootTable.Columns["ExImLcOrderId"].Visible = false;
			grdDetail.RootTable.Columns["InvoiceId"].Visible = false;
			grdDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(grdDetail, 3, 2);
			grdDetail.RootTable.Columns["LcOrderNo"].Caption = "Contract No";
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdDetail, dtDetailForLayout);
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
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1f: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.frmThirdPartyLotInspectionApproval));
		GridEXLayout grdEtaDestination_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrintCurrent = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnApprove = new System.Windows.Forms.ToolStripButton();
		this.BtnThirdPartyInspectionForm = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLabAgainstThirdPartyInspectionForm = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdEtaDestination = new GridEX();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label26 = new System.Windows.Forms.Label();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.RadNotReffered = new System.Windows.Forms.RadioButton();
		this.RadReffered = new System.Windows.Forms.RadioButton();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.CmbLotTrackingNo = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbsupplierCustomer = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbItemName = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.grdDetail = new GridEX();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.radAwaitingStatus = new System.Windows.Forms.RadioButton();
		this.radAcceptedStatus = new System.Windows.Forms.RadioButton();
		this.radAllStatus = new System.Windows.Forms.RadioButton();
		this.radRejectedStatus = new System.Windows.Forms.RadioButton();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.toolStrip3.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEtaDestination).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLotTrackingNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplierCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.panel8.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.toolStripSeparator5, this.btnNew, this.btnRefresh, this.toolStripSeparator4, this.btnPrintCurrent, this.toolStripSeparator3, this.btnApprove, this.BtnThirdPartyInspectionForm, this.toolStripSeparator1, this.BtnLabAgainstThirdPartyInspectionForm,
			this.btnShortCutKeys, this.toolStripSeparator2
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
		this.btnApprove.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApprove.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnApprove.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApprove.Name = "btnApprove";
		this.btnApprove.Size = new System.Drawing.Size(85, 24);
		this.btnApprove.Text = "Approve";
		this.btnApprove.Click += new System.EventHandler(btnApprove_Click);
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
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1087, 646);
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
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 232);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1087, 204);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 130;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 207);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1087, 25);
		this.panel3.TabIndex = 129;
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
		this.panel2.Size = new System.Drawing.Size(1087, 177);
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
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Location = new System.Drawing.Point(360, 0);
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Name = "grdEtaDestination";
		((ScrollableBase)this.grdEtaDestination).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Size = new System.Drawing.Size(411, 177);
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).TabIndex = 6;
		this.grdEtaDestination.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEtaDestination).VisualStyle = (VisualStyle)4;
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.panel5);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.datToDate);
		this.groupBox1.Controls.Add(this.datFromDate);
		this.groupBox1.Controls.Add(this.RadNotReffered);
		this.groupBox1.Controls.Add(this.RadReffered);
		this.groupBox1.Controls.Add(this.RadAll);
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
		this.groupBox1.Size = new System.Drawing.Size(360, 177);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(10, 21);
		this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(91, 15);
		this.label26.TabIndex = 524;
		this.label26.Text = "From && To Date";
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(234, 17);
		this.datToDate.Margin = new System.Windows.Forms.Padding(4);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(116, 23);
		this.datToDate.TabIndex = 1;
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Checked = false;
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(114, 17);
		this.datFromDate.Margin = new System.Windows.Forms.Padding(4);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.ShowCheckBox = true;
		this.datFromDate.Size = new System.Drawing.Size(119, 23);
		this.datFromDate.TabIndex = 0;
		this.RadNotReffered.AutoSize = true;
		this.RadNotReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.RadNotReffered.Location = new System.Drawing.Point(143, 153);
		this.RadNotReffered.Name = "RadNotReffered";
		this.RadNotReffered.Size = new System.Drawing.Size(69, 19);
		this.RadNotReffered.TabIndex = 6;
		this.RadNotReffered.Text = "Pending";
		this.RadNotReffered.UseVisualStyleBackColor = true;
		this.RadReffered.AutoSize = true;
		this.RadReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.RadReffered.Location = new System.Drawing.Point(212, 153);
		this.RadReffered.Name = "RadReffered";
		this.RadReffered.Size = new System.Drawing.Size(76, 19);
		this.RadReffered.TabIndex = 7;
		this.RadReffered.Text = "Complete";
		this.RadReffered.UseVisualStyleBackColor = true;
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(102, 153);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(39, 19);
		this.RadAll.TabIndex = 5;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.CmbLotTrackingNo.AllowNull = (DefaultableBoolean)1;
		this.CmbLotTrackingNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLotTrackingNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLotTrackingNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
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
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).Location = new System.Drawing.Point(114, 95);
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).Name = "CmbLotTrackingNo";
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(10, 101);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(91, 15);
		this.label2.TabIndex = 129;
		this.label2.Text = "Lot Tracking No";
		this.cmbsupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
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
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).Location = new System.Drawing.Point(114, 41);
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).Name = "cmbsupplierCustomer";
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).TabIndex = 2;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(291, 149);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(59, 26);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(114, 68);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 3;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(10, 47);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(58, 15);
		this.label8.TabIndex = 125;
		this.label8.Text = "Customer";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(10, 74);
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
		this.label9.Size = new System.Drawing.Size(300, 19);
		this.label9.TabIndex = 1;
		this.label9.Text = "Third Party Inspection Lot Approval";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar2);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel4.Location = new System.Drawing.Point(0, 436);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1087, 25);
		this.panel4.TabIndex = 131;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(6, 2);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(263, 19);
		this.label3.TabIndex = 1;
		this.label3.Text = "Detail of above selected record";
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		this.grdDetail.AlternatingColors = true;
		grdDetail_DesignTimeLayout.LayoutString = resources.GetString("grdDetail_DesignTimeLayout.LayoutString");
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 461);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1087, 185);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 132;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1043, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 22);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1043, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 22);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.radAwaitingStatus.AutoSize = true;
		this.radAwaitingStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.radAwaitingStatus.Location = new System.Drawing.Point(40, 5);
		this.radAwaitingStatus.Name = "radAwaitingStatus";
		this.radAwaitingStatus.Size = new System.Drawing.Size(72, 19);
		this.radAwaitingStatus.TabIndex = 526;
		this.radAwaitingStatus.Text = "Awaiting";
		this.radAwaitingStatus.UseVisualStyleBackColor = true;
		this.radAcceptedStatus.AutoSize = true;
		this.radAcceptedStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.radAcceptedStatus.Location = new System.Drawing.Point(110, 5);
		this.radAcceptedStatus.Name = "radAcceptedStatus";
		this.radAcceptedStatus.Size = new System.Drawing.Size(75, 19);
		this.radAcceptedStatus.TabIndex = 527;
		this.radAcceptedStatus.Text = "Accepted";
		this.radAcceptedStatus.UseVisualStyleBackColor = true;
		this.radAllStatus.AutoSize = true;
		this.radAllStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.radAllStatus.Location = new System.Drawing.Point(1, 5);
		this.radAllStatus.Name = "radAllStatus";
		this.radAllStatus.Size = new System.Drawing.Size(39, 19);
		this.radAllStatus.TabIndex = 525;
		this.radAllStatus.Text = "All";
		this.radAllStatus.UseVisualStyleBackColor = true;
		this.radRejectedStatus.AutoSize = true;
		this.radRejectedStatus.Checked = true;
		this.radRejectedStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.radRejectedStatus.Location = new System.Drawing.Point(185, 5);
		this.radRejectedStatus.Name = "radRejectedStatus";
		this.radRejectedStatus.Size = new System.Drawing.Size(70, 19);
		this.radRejectedStatus.TabIndex = 528;
		this.radRejectedStatus.TabStop = true;
		this.radRejectedStatus.Text = "Rejected";
		this.radRejectedStatus.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.radAcceptedStatus);
		this.panel5.Controls.Add(this.radRejectedStatus);
		this.panel5.Controls.Add(this.radAwaitingStatus);
		this.panel5.Controls.Add(this.radAllStatus);
		this.panel5.Location = new System.Drawing.Point(92, 122);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(258, 26);
		this.panel5.TabIndex = 530;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(10, 127);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(75, 15);
		this.label4.TabIndex = 531;
		this.label4.Text = "Result Status";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1087, 673);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.toolStrip3);
		base.KeyPreview = true;
		base.Name = "frmThirdPartyLotInspectionApproval";
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
		((System.ComponentModel.ISupportInitialize)this.CmbLotTrackingNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplierCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
