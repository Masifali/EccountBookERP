// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Lab;
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
namespace Architecture.WinApp.Lab
{

public class InProcessLabAnalysisRegister : Form
{
	private DataTable dtMainFromDb = new DataTable();

	private DataTable dtMainGrid = new DataTable();

	private DataTable dtGroupAnalysisFromDb = new DataTable();

	private DataTable dtGroupAnalysisGrid = new DataTable();

	private bool FormInitialized;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Label label27;

	private Label label1;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private Panel panel5;

	internal GridEX grdStepAnalysis;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private Label label2;

	internal UltraCombo cmbPlantName;

	internal UltraCombo cmbItemName;

	internal GridEX grdGroupAnalysis;

	private Panel panel12;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel9;

	private Label label4;

	private Label label3;

	internal UltraCombo cmbDocDate;

	internal UltraCombo CmbJobOrderNo;

	private Label label5;

	private ToolStripButton toolStripButton2;

	public InProcessLabAnalysisRegister(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 600);
			PlantComboFill(HistoryComboDbCall("Plant"));
			cmbPlantName.Focus();
			FormInitialized = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private DataTable HistoryComboDbCall(string Activity = "", int PlantId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			return LabInProcessAnalysisRawHeader.GetDataForDropDownFromLabInProcessAnalysisRawHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeIds = "306",
				Activity = Activity
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PlantComboFill(DataTable dtCombo)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbPlantName, dtCombo, "Id", "ReferenceName", "Plant", AllColumns: false, Conversion.ToInt(cmbPlantName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherComboFill(DataTable dt)
	{
		try
		{
			DataTable dtJobOrder = new DataTable();
			dtJobOrder.Columns.Add("Id");
			dtJobOrder.Columns.Add("name");
			DataTable dtItem = dtJobOrder.Clone();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "JobOrder")
					{
						dtJobOrder.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbItemName, dtItem, "Id", "name", "Item Name", AllColumns: true, Conversion.ToInt(cmbItemName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbJobOrderNo, dtJobOrder, "Id", "name", "Job Order", AllColumns: true, Conversion.ToInt(CmbJobOrderNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DateBindDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		try
		{
			DataTable dtpp = InvLabAnalysisInProcessHeader.DateFill(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				PlantId = Conversion.ToInt(cmbPlantName.Value),
				JobOrderId = Conversion.ToInt(CmbJobOrderNo.Value),
				ItemId = Conversion.ToInt(cmbItemName.Value)
			});
			DataTable dtDate = new DataTable();
			dtDate.Columns.Add("Id", typeof(int));
			dtDate.Columns.Add("DocDate");
			dtDate.Rows.Clear();
			for (int i = 0; i < dtpp.Rows.Count; i++)
			{
				dtDate.Rows.Add(i + 1, Conversion.ToDateTime(dtpp.Rows[i]["DocDate"]).ToShortDateString());
			}
			InfragisticsHelper.BindAndRetainSelection(cmbDocDate, dtDate, "Id", "DocDate", "DocDate", AllColumns: true, Conversion.ToInt(cmbDocDate.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbPlantName_Leave(object sender, EventArgs e)
	{
		try
		{
			OtherComboFill(HistoryComboDbCall(null, Conversion.ToInt(cmbPlantName.Value)));
			cmbItemName_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			DateBindDbCall();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void reset()
	{
		try
		{
			((Control)(object)CmbJobOrderNo).Text = string.Empty;
			((Control)(object)cmbItemName).Text = string.Empty;
			((Control)(object)cmbDocDate).Text = string.Empty;
			cmbPlantName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InProcessLabAnalysisRegister_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbPlantName).ActiveRow == null)
			{
				MessageBox.Show("Plant Name Feild Required...");
				cmbPlantName.Focus();
				return;
			}
			if (((UltraGridBase)cmbItemName).ActiveRow == null)
			{
				MessageBox.Show("Item Name Feild Required...");
				cmbItemName.Focus();
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocDate = Conversion.ToDateTime((object)((Control)(object)cmbDocDate).Text);
			obj.PlantId = Conversion.ToInt(cmbPlantName.Value);
			obj.JobOrderId = Conversion.ToInt(CmbJobOrderNo.Value);
			obj.ItemId = Conversion.ToInt(cmbItemName.Value);
			dtMainFromDb = InvLabAnalysisInProcessHeader.InProcessStepAnalysisRegister(obj);
			dtGroupAnalysisFromDb = InvLabAnalysisInProcessHeader.InProcessAnalysisRecovery(obj);
			dtMainGrid.Columns.Clear();
			dtGroupAnalysisGrid.Columns.Clear();
			if (dtMainFromDb.Rows.Count > 0)
			{
				string[] columnNames2 = (from DataColumn x in dtMainFromDb.Columns
					select x.ColumnName).ToArray();
				int lenthofcolumns2 = columnNames2.Length - 33;
				DataTable dataTable = dtMainGrid;
				if (dataTable != null && dataTable.Columns.Count == 0)
				{
					dtMainGrid.Columns.Add("ScheduleId");
					dtMainGrid.Columns.Add("PlantId");
					dtMainGrid.Columns.Add("StepId");
					dtMainGrid.Columns.Add("ParameterId");
					dtMainGrid.Columns.Add("StepName");
					dtMainGrid.Columns.Add("Parameter");
				}
				dtMainGrid.Clear();
				for (int l = 0; l < lenthofcolumns2; l++)
				{
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dtMainFromDb.Rows[0][columnNames2[l + 33]])))
					{
						dtMainGrid.Columns.Add(Conversion.ToDateTime(dtMainFromDb.Rows[0][columnNames2[l + 33]]).ToShortTimeString());
					}
				}
				string[] columnofnewdt2 = (from DataColumn x in dtMainGrid.Columns
					select x.ColumnName).ToArray();
				int lenthofcolumnsofnewdt2 = columnofnewdt2.Length - 6;
				for (int k = 0; k < dtMainFromDb.Rows.Count; k++)
				{
					DataRow dr2 = dtMainGrid.NewRow();
					dr2["ScheduleId"] = dtMainFromDb.Rows[k]["ScheduleId"];
					dr2["PlantId"] = dtMainFromDb.Rows[k]["PlantId"];
					dr2["StepId"] = dtMainFromDb.Rows[k]["LabInProcessAnalysisStepId"];
					dr2["ParameterId"] = dtMainFromDb.Rows[k]["AnalysisParameterId"];
					dr2["StepName"] = dtMainFromDb.Rows[k]["ProcessStepName"];
					dr2["Parameter"] = dtMainFromDb.Rows[k]["AnalysisParameterDescription"];
					for (int n = 0; n < lenthofcolumnsofnewdt2; n++)
					{
						dr2[columnofnewdt2[n + 6]] = dtMainFromDb.Rows[k][21 + n];
					}
					dtMainGrid.Rows.Add(dr2);
				}
				grdStepAnalysis.DataSource = dtMainGrid;
				grdStepAnalysis.RetrieveStructure();
				GridSettings();
			}
			else
			{
				grdStepAnalysis.DataSource = null;
				MessageBox.Show("No Record found For Display...");
			}
			grdGroupAnalysis.ClearStructure();
			if (dtGroupAnalysisFromDb.Rows.Count > 0)
			{
				string[] columnNames = (from DataColumn x in dtGroupAnalysisFromDb.Columns
					select x.ColumnName).ToArray();
				int lenthofcolumns = columnNames.Length - 30;
				DataTable dataTable2 = dtGroupAnalysisGrid;
				if (dataTable2 != null && dataTable2.Columns.Count == 0)
				{
					dtGroupAnalysisGrid.Columns.Add("ParameterId");
					dtGroupAnalysisGrid.Columns.Add("PlantName");
					dtGroupAnalysisGrid.Columns.Add("ItemName");
					dtGroupAnalysisGrid.Columns.Add("Parameter");
					dtGroupAnalysisGrid.Columns.Add("MinValue");
					dtGroupAnalysisGrid.Columns.Add("MaxValue");
				}
				dtGroupAnalysisGrid.Clear();
				for (int j = 0; j < lenthofcolumns; j++)
				{
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dtGroupAnalysisFromDb.Rows[0][columnNames[j + 30]])))
					{
						dtGroupAnalysisGrid.Columns.Add(Conversion.ToDateTime(dtGroupAnalysisFromDb.Rows[0][columnNames[j + 30]]).ToShortTimeString());
					}
				}
				string[] columnofnewdt = (from DataColumn x in dtGroupAnalysisGrid.Columns
					select x.ColumnName).ToArray();
				int lenthofcolumnsofnewdt = columnofnewdt.Length - 6;
				for (int i = 0; i < dtGroupAnalysisFromDb.Rows.Count; i++)
				{
					DataRow dr = dtGroupAnalysisGrid.NewRow();
					dr["ParameterId"] = dtGroupAnalysisFromDb.Rows[i]["AnalysisParameterId"];
					dr["PlantName"] = dtGroupAnalysisFromDb.Rows[i]["PlantName"];
					dr["ItemName"] = dtGroupAnalysisFromDb.Rows[i]["ItemName"];
					dr["Parameter"] = dtGroupAnalysisFromDb.Rows[i]["AnalysisParameterDescription"];
					dr["MinValue"] = dtGroupAnalysisFromDb.Rows[i]["MinValue"];
					dr["MaxValue"] = dtGroupAnalysisFromDb.Rows[i]["MaxValue"];
					for (int m = 0; m < lenthofcolumnsofnewdt; m++)
					{
						dr[columnofnewdt[m + 6]] = dtGroupAnalysisFromDb.Rows[i][18 + m];
					}
					dtGroupAnalysisGrid.Rows.Add(dr);
				}
				grdGroupAnalysis.DataSource = dtGroupAnalysisGrid;
				grdGroupAnalysis.RetrieveStructure();
				GridGroupAnalysisSettings();
			}
			else
			{
				grdGroupAnalysis.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridGroupAnalysisSettings()
	{
		grdGroupAnalysis.RootTable.Columns["ParameterId"].Visible = false;
		grdGroupAnalysis.RootTable.Columns["PlantName"].Width = 150;
		grdGroupAnalysis.RootTable.Columns["ItemName"].Width = 180;
		grdGroupAnalysis.RootTable.Columns["Parameter"].Width = 150;
	}

	private void GridSettings()
	{
		try
		{
			grdStepAnalysis.RootTable.Columns["ParameterId"].Visible = false;
			grdStepAnalysis.RootTable.Columns["StepId"].Visible = false;
			grdStepAnalysis.RootTable.Columns["PlantId"].Visible = false;
			grdStepAnalysis.RootTable.Columns["ScheduleId"].Visible = false;
			grdStepAnalysis.RootTable.Columns["StepName"].Width = 150;
			grdStepAnalysis.RootTable.Columns["Parameter"].Width = 150;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdStepAnalysis, dtMainGrid);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdGroupAnalysis, dtGroupAnalysisGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtMainFromDb.Rows.Count == 0 || dtMainFromDb == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(dtGroupAnalysisFromDb, "LabInprocessRegisterSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dtMainFromDb, "660-LabSInProcessAnalysisRegister.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
				HistoryStack.Remove(this);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdGroupAnalysis).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbPlantName.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbPlantName.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				print_Click_1(null, null);
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
			dt.Rows.Add("Ctrl+S", "For show data");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Date Type");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton2_Click(object sender, EventArgs e)
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
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		GridEXLayout grdGroupAnalysis_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdStepAnalysis_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lab.InProcessLabAnalysisRegister));
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.label23 = new System.Windows.Forms.Label();
		this.grdGroupAnalysis = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdStepAnalysis = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.cmbDocDate = new UltraCombo();
		this.cmbPlantName = new UltraCombo();
		this.CmbJobOrderNo = new UltraCombo();
		this.cmbItemName = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGroupAnalysis).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStepAnalysis).BeginInit();
		this.panel9.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDocDate).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPlantName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).BeginInit();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.toolStrip3);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1084, 55);
		this.panel3.TabIndex = 4;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.print, this.toolStripButton2 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1084, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(92, 24);
		this.print.Text = "660-Print";
		this.print.Click += new System.EventHandler(print_Click_1);
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources._55download;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(126, 24);
		this.toolStripButton2.Text = "ShortCut Keys";
		this.toolStripButton2.Click += new System.EventHandler(toolStripButton2_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 13f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 29);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(264, 22);
		this.label23.TabIndex = 1;
		this.label23.Text = "In Process Analysis Register";
		this.grdGroupAnalysis.AllowEdit = (InheritableBoolean)2;
		this.grdGroupAnalysis.AlternatingColors = true;
		((ScrollableBase)this.grdGroupAnalysis).BackColor = System.Drawing.Color.White;
		grdGroupAnalysis_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGroupAnalysis.DesignTimeLayout = grdGroupAnalysis_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGroupAnalysis).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdGroupAnalysis.DynamicFiltering = true;
		this.grdGroupAnalysis.FilterMode = (FilterMode)2;
		this.grdGroupAnalysis.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGroupAnalysis).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGroupAnalysis.GroupByBoxVisible = false;
		this.grdGroupAnalysis.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdGroupAnalysis).Location = new System.Drawing.Point(0, 311);
		((System.Windows.Forms.Control)(object)this.grdGroupAnalysis).Name = "grdGroupAnalysis";
		((ScrollableBase)this.grdGroupAnalysis).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGroupAnalysis).Size = new System.Drawing.Size(1084, 250);
		((System.Windows.Forms.Control)(object)this.grdGroupAnalysis).TabIndex = 2;
		this.grdGroupAnalysis.TotalRow = (InheritableBoolean)1;
		this.grdGroupAnalysis.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGroupAnalysis).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label4);
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel12.Location = new System.Drawing.Point(0, 286);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1084, 25);
		this.panel12.TabIndex = 1;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Verdana", 10.75f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(6, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(363, 18);
		this.label4.TabIndex = 108;
		this.label4.Text = "Plant's In Process Recovery Analysis Detail";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1035, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdGroupAnalysis;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar2.TabIndex = 0;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.grdStepAnalysis.AllowEdit = (InheritableBoolean)2;
		this.grdStepAnalysis.AlternatingColors = true;
		((ScrollableBase)this.grdStepAnalysis).BackColor = System.Drawing.Color.White;
		grdStepAnalysis_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdStepAnalysis.DesignTimeLayout = grdStepAnalysis_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdStepAnalysis).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdStepAnalysis.DynamicFiltering = true;
		this.grdStepAnalysis.FilterMode = (FilterMode)2;
		this.grdStepAnalysis.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdStepAnalysis).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdStepAnalysis.GroupByBoxVisible = false;
		this.grdStepAnalysis.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdStepAnalysis).Location = new System.Drawing.Point(0, 118);
		((System.Windows.Forms.Control)(object)this.grdStepAnalysis).Name = "grdStepAnalysis";
		((ScrollableBase)this.grdStepAnalysis).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdStepAnalysis).Size = new System.Drawing.Size(1084, 168);
		((System.Windows.Forms.Control)(object)this.grdStepAnalysis).TabIndex = 1;
		this.grdStepAnalysis.TotalRow = (InheritableBoolean)1;
		this.grdStepAnalysis.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdStepAnalysis).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label3);
		this.panel9.Controls.Add(this.ctrlGrdBar1);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 93);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1084, 25);
		this.panel9.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Verdana", 10.75f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(3, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(324, 18);
		this.label3.TabIndex = 107;
		this.label3.Text = "Plant's In Process Step Analysis Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1035, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdStepAnalysis;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDocDate);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPlantName);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNo);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemName);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 55);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1084, 38);
		this.panel5.TabIndex = 0;
		this.cmbDocDate.AllowNull = (DefaultableBoolean)1;
		this.cmbDocDate.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDocDate.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDocDate.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocDate).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocDate).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDocDate).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDocDate).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDocDate.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDocDate).Location = new System.Drawing.Point(869, 6);
		((System.Windows.Forms.Control)(object)this.cmbDocDate).Name = "cmbDocDate";
		((System.Windows.Forms.Control)(object)this.cmbDocDate).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.cmbDocDate).TabIndex = 3;
		this.cmbPlantName.AllowNull = (DefaultableBoolean)1;
		this.cmbPlantName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPlantName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPlantName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPlantName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Location = new System.Drawing.Point(73, 6);
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Name = "cmbPlantName";
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.cmbPlantName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Leave += new System.EventHandler(cmbPlantName_Leave);
		this.CmbJobOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Location = new System.Drawing.Point(306, 6);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Name = "CmbJobOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Leave += new System.EventHandler(cmbItemName_Leave);
		this.cmbItemName.AllowNull = (DefaultableBoolean)1;
		this.cmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Location = new System.Drawing.Point(578, 6);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Name = "cmbItemName";
		((System.Windows.Forms.Control)(object)this.cmbItemName).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Leave += new System.EventHandler(cmbItemName_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(226, 12);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(78, 15);
		this.label5.TabIndex = 104;
		this.label5.Text = "Job Order No";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(1020, 6);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 26);
		this.btnshow.TabIndex = 4;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(508, 12);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 104;
		this.label2.Text = "Item Name";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(69, 15);
		this.label1.TabIndex = 104;
		this.label1.Text = "Plant Name";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(811, 12);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(57, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "Doc Date";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1084, 561);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdStepAnalysis);
		base.Controls.Add(this.panel12);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdGroupAnalysis);
		base.Controls.Add(this.panel9);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.panel3);
		this.DoubleBuffered = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "InProcessLabAnalysisRegister";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InProcessLabAnalysisRegister_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGroupAnalysis).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStepAnalysis).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDocDate).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPlantName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).EndInit();
		base.ResumeLayout(false);
	}
}

}
