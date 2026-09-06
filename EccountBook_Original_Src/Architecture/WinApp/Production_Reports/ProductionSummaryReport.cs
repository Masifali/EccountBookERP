using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production_Reports;

public class ProductionSummaryReport : Form
{
	private int VoucherHeadId;

	public static bool ValuesShowRights;

	private DataTable dtBranch = new DataTable();

	private DataTable dtSummerReport = new DataTable();

	private DataTable dtDocWiseSummery = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip3;

	private ToolStripButton toolStripButton1;

	private ToolStripSeparator toolStripSeparator2;

	internal UltraCombo cmbsummeryJobOrderNo;

	private Label label68;

	private ToolStripButton btnProductionRecoveryReport;

	private CtrlGrdBar ctrlGrdBar5;

	private Panel panel20;

	private Button btnShow;

	private Panel panel19;

	internal GridEX grdSummery;

	private Panel panel21;

	private Label label24;

	private Label label3;

	private DateTimePicker txtDateFromSummary;

	private DateTimePicker txtDateToSummary;

	private Label label39;

	private Label label36;

	internal UltraCombo cmbBranchName;

	private Label label13;

	internal UltraCombo CmbPlantNameSummary;

	private Label label87;

	private Panel panel2;

	private Panel panel3;

	private Panel panel86;

	private Label label173;

	private Label label177;

	private DateTimePicker txtSummaryJobStartedDate;

	private Label label180;

	private DateTimePicker txtSummarySettlementDate;

	private Label label179;

	private TextBox txtSummaryJobOrderNo;

	private Label label175;

	private TextBox txtSummaryJobOrderStatus;

	private Label label178;

	private TextBox txtSummarySettlementStatus;

	private Label label176;

	private TextBox txtSummaryLotCode;

	private TextBox txtSummaryjobOrderDocNo;

	private Panel panel4;

	private Panel panel50;

	private Label label174;

	internal GridEX grdSummaryExportScheduleInfo;

	private TabControl tabControl5;

	private TabPage tabPage7;

	internal GridEX grdSummaryDetail;

	private Panel panel47;

	private CtrlGrdBar ctrlGrdBar1;

	private TabPage tabPage8;

	internal GridEX grdSummaryDocWiseDetail;

	private Panel panel26;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btnPrintForDetailGridData;

	private ToolStripButton btnRefreshSummary;

	private ToolStripDropDownButton tsDropDowngrnWiseInput;

	private ToolStripButton BtnPrintWithLab602_01;

	public ProductionSummaryReport(UserAccount user)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void ProductionSummaryReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 800);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			ValuesShowRights = formright.DoHaveCanRateandAmount;
			if (!formright.DoHaveCanRateandAmount)
			{
				ctrlGrdBar5.mGridChooseFielder.Enabled = false;
			}
			else
			{
				ctrlGrdBar5.mGridChooseFielder.Visible = false;
				ctrlGrdBar5.mGridSaveLayouts.Visible = false;
				ctrlGrdBar5.removeLayoutToolStripMenuItem.Visible = false;
			}
			BranchesFill();
			JobOrderNoFillForSummery();
			cmbsummeryJobOrderNo.Focus();
			ReportsLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			dtBranch = InvFoodProduction.GetBranchsAllocatedToUserFromProduction(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 0, "");
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)cmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				cmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbBranchName).Text != string.Empty && ((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				JobOrderNoFillForSummery();
				return;
			}
			((Control)(object)cmbsummeryJobOrderNo).Text = string.Empty;
			((UltraGridBase)cmbsummeryJobOrderNo).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderNoFillForSummery()
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		try
		{
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
			}
			DataTable dtPJO = InvProductionJobOrder.GetJobOrderNoWithInfo(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 403,
				BranchesIds = BranchIds
			});
			InfragisticsHelper.BindAndRetainSelection(cmbsummeryJobOrderNo, dtPJO, "Id", "JobOrderNo", "JobOrderNo", AllColumns: true, Conversion.ToInt(cmbsummeryJobOrderNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsummeryJobOrderNo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbsummeryJobOrderNo).ActiveRow != null)
			{
				DataTable dt = InvProductionJobOrder.GetPlantByJobOrderId(Conversion.ToInt(cmbsummeryJobOrderNo.Value), UserAccount.BranchesId);
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDLNew(dt, CmbPlantNameSummary, "PlantId", "PlantName", "Plant Name", false);
				}
			}
			else
			{
				((UltraGridBase)CmbPlantNameSummary).DataSource = null;
				((Control)(object)CmbPlantNameSummary).Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ProductionSummaryReport_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbsummeryJobOrderNo.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbsummeryJobOrderNo).Text = string.Empty;
			cmbsummeryJobOrderNo.Focus();
			grdSummery.ClearStructure();
			grdSummaryDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tsDropDowngrnWiseInput_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(cmbsummeryJobOrderNo.Value);
			if (txtDateFromSummary.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)txtDateFromSummary.Value);
			}
			if (txtDateToSummary.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)txtDateToSummary.Value);
			}
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
			}
			if (obj.JobOrderId > 0)
			{
				DataTable dt = InvFoodProductionReports.FoodProductionIssuanceGrnWiseByJobOrderId_607(obj);
				if (dt.Rows.Count == 0)
				{
					MessageBox.Show("Record Not Found For DisPlay");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, e.ClickedItem.Text + ".rpt");
			}
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
			string[] filesSelected = CommonServices.DynamicReportsLoad("GrnInvoiceWiseInput");
			if (filesSelected != null && filesSelected.Length != 0)
			{
				for (int i = 0; i < filesSelected.Length; i++)
				{
					tsDropDowngrnWiseInput.DropDownItems.Add(Path.GetFileNameWithoutExtension(filesSelected[i]));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnProductionRecoveryReport_Click(object sender, EventArgs e)
	{
		try
		{
			var (jobOrderId, plantId, branchIds, useDateFilter, fromDate, toDate) = ProductionRecoveryReportParams();
			CommonServices.ProductionRecoveryReport602(jobOrderId, plantId, branchIds, useDateFilter, fromDate, toDate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintWithLab602_01_Click(object sender, EventArgs e)
	{
		try
		{
			var (jobOrderId, plantId, branchIds, useDateFilter, fromDate, toDate) = ProductionRecoveryReportParams();
			CommonServices.ProductionRecoveryReportWithLab_602_01(jobOrderId, plantId, branchIds, useDateFilter, fromDate, toDate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private (int JobOrderId, int PlantId, string BranchIds, bool UseDateFilter, DateTime? FromDate, DateTime? ToDate) ProductionRecoveryReportParams()
	{
		DateTime? FromDate = null;
		DateTime? ToDate = null;
		bool useDateFilter = false;
		int JobOrderId = Conversion.ToInt(cmbsummeryJobOrderNo.Value);
		int PlantId = Conversion.ToInt(CmbPlantNameSummary.Value);
		if (txtDateFromSummary.Checked)
		{
			useDateFilter = true;
			FromDate = Conversion.ToDateTime((object)txtDateFromSummary.Value);
		}
		if (txtDateToSummary.Checked)
		{
			useDateFilter = true;
			ToDate = Conversion.ToDateTime((object)txtDateToSummary.Value);
		}
		string BranchIds = "";
		if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
		{
			string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
			foreach (string author in array)
			{
				DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
				if (dr.Length != 0)
				{
					BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
				}
			}
		}
		return (JobOrderId: JobOrderId, PlantId: PlantId, BranchIds: BranchIds, UseDateFilter: useDateFilter, FromDate: FromDate, ToDate: ToDate);
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			GrdSummaryValuesFill();
			SummaryJobOrderInformationFill();
			GridSummaryJobOrderScheduleDetail();
			GrdSummaryReportFill();
			grdDocWiseSummeryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdSummaryValuesFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters objSumry = new ReportsParameters();
			objSumry.OrganizationId = UserAccount.OrganizationId;
			objSumry.CompanyId = UserAccount.CompanyId;
			objSumry.JobOrderId = Conversion.ToInt(cmbsummeryJobOrderNo.Value);
			objSumry.PlantId = Conversion.ToInt(CmbPlantNameSummary.Value);
			if (txtDateFromSummary.Checked)
			{
				objSumry.FromDate = Conversion.ToDateTime((object)txtDateFromSummary.Value);
			}
			if (txtDateToSummary.Checked)
			{
				objSumry.ToDate = Conversion.ToDateTime((object)txtDateToSummary.Value);
			}
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				objSumry.BranchesIds = BranchIds;
			}
			DataTable dt = InvFoodProduction.GetSummeryValues(objSumry);
			if (dt.Rows.Count > 0 && dt != null)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("SortNo", typeof(int));
				dtcol.Columns.Add("TranType", typeof(string));
				dtcol.Columns.Add("WeightPrcnt", typeof(double));
				dtcol.Columns.Add("NetQty", typeof(double));
				dtcol.Columns.Add("NetWeight", typeof(double));
				dtcol.Columns.Add("Amount", typeof(double));
				dtcol.Columns.Add("AvgRate", typeof(double));
				dtcol.Columns.Add("AvgRateWoExp", typeof(double));
				dtcol.Columns.Add("OverHeadAmount", typeof(double));
				dtcol.Columns.Add("PackingMaterialAmount", typeof(double));
				dtcol.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcol.Rows.Add(dt.Rows[i]["SortNo"], dt.Rows[i]["TranType"], dt.Rows[i]["WeightPrcnt"], dt.Rows[i]["NetQty"], dt.Rows[i]["NetWeight"], dt.Rows[i]["Amount"], dt.Rows[i]["AvgRate"], dt.Rows[i]["AvgRateWoExp"], dt.Rows[i]["ByProductOverHeads"], dt.Rows[i]["ByProductPackingMaterial"]);
				}
				grdSummery.DataSource = dtcol;
				grdSummery.RetrieveStructure();
				GrdSummarySetting();
			}
			else
			{
				grdSummery.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdSummarySetting()
	{
		try
		{
			if (!formright.DoHaveCanRateandAmount)
			{
				grdSummery.RootTable.Columns["AvgRate"].Visible = false;
				grdSummery.RootTable.Columns["Amount"].Visible = false;
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grdSummery, 2, 2, null, null, null, ColumnSetting: true, BracketsForamt: false, null, ExcludeAllColumnsFromTotals: true);
			grdSummery.RootTable.Columns["SortNo"].Width = 35;
			grdSummery.RootTable.Columns["TranType"].Width = 125;
			grdSummery.RootTable.Columns["NetQty"].Width = 70;
			grdSummery.RootTable.Columns["NetWeight"].Width = 80;
			grdSummery.RootTable.Columns["Amount"].Width = 110;
			grdSummery.RootTable.Columns["AvgRate"].Width = 65;
			grdSummery.RootTable.Columns["AvgRateWoExp"].Width = 65;
			grdSummery.RootTable.Columns["OverHeadAmount"].Width = 90;
			grdSummery.RootTable.Columns["PackingMaterialAmount"].Width = 110;
			grdSummery.RootTable.Columns["WeightPrcnt"].Width = 55;
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SummaryJobOrderInformationFill()
	{
		try
		{
			int JobOrderId = Conversion.ToInt(cmbsummeryJobOrderNo.Value);
			if (((UltraGridBase)cmbsummeryJobOrderNo).ActiveRow == null || ((UltraDropDownBase)cmbsummeryJobOrderNo).SelectedRow == null || JobOrderId == 0)
			{
				txtSummaryJobOrderNo.Text = "";
				txtSummaryjobOrderDocNo.Text = "";
				txtSummaryJobStartedDate.Visible = false;
				txtSummaryJobOrderStatus.Text = "";
				txtSummarySettlementStatus.Text = "";
				txtSummarySettlementDate.Visible = false;
				txtSummaryLotCode.Text = "";
				return;
			}
			txtSummaryJobStartedDate.Visible = true;
			txtSummaryJobOrderNo.Text = Conversion.ToString(((UltraDropDownBase)cmbsummeryJobOrderNo).SelectedRow.Cells["JobOrderNo"].Value);
			txtSummaryjobOrderDocNo.Text = Conversion.ToString(((UltraDropDownBase)cmbsummeryJobOrderNo).SelectedRow.Cells["JobOrderDocNo"].Value);
			txtSummaryJobStartedDate.Value = Conversion.ToDateTime(((UltraDropDownBase)cmbsummeryJobOrderNo).SelectedRow.Cells["JobStartDate"].Value);
			txtSummaryJobOrderStatus.Text = Conversion.ToString(((UltraDropDownBase)cmbsummeryJobOrderNo).SelectedRow.Cells["JobOrderStatus"].Value);
			txtSummarySettlementStatus.Text = Conversion.ToString(((UltraDropDownBase)cmbsummeryJobOrderNo).SelectedRow.Cells["SettledStatus"].Value);
			if (txtSummarySettlementStatus.Text == "Settled")
			{
				txtSummarySettlementDate.Visible = true;
				txtSummarySettlementDate.Value = Conversion.ToDateTime(((UltraDropDownBase)cmbsummeryJobOrderNo).SelectedRow.Cells["ApprovedDate"].Value);
			}
			else
			{
				txtSummarySettlementDate.Visible = false;
			}
			txtSummaryLotCode.Text = Conversion.ToString(((UltraDropDownBase)cmbsummeryJobOrderNo).SelectedRow.Cells["LotCode"].Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSummaryJobOrderScheduleDetail()
	{
		try
		{
			DataTable dt = InvProductionJobOrder.InvProductionJobOrderAndOrderAllocation_SummaryByJobOrderId(Conversion.ToInt(cmbsummeryJobOrderNo.Value));
			if (dt != null && dt.Rows.Count == 0)
			{
				grdSummaryExportScheduleInfo.DataSource = null;
				return;
			}
			DataTable dtExportSchedule = new DataTable();
			dtExportSchedule.Columns.Add("ContractId", typeof(int));
			dtExportSchedule.Columns.Add("SaleContract", typeof(string));
			dtExportSchedule.Columns.Add("ScheduleId", typeof(int));
			dtExportSchedule.Columns.Add("ScheduleNo", typeof(string));
			dtExportSchedule.Columns.Add("SupCustId", typeof(int));
			dtExportSchedule.Columns.Add("CustomerName", typeof(string));
			dtExportSchedule.Rows.Clear();
			foreach (DataRow dr in dt.Rows)
			{
				dtExportSchedule.Rows.Add(dr["OrderId"], dr["ContractNo"], dr["ScheduleId"], dr["ScheduleNo"], dr["SupCustId"], dr["CustomerName"]);
			}
			grdSummaryExportScheduleInfo.DataSource = dtExportSchedule;
			grdSummaryExportScheduleInfo.RetrieveStructure();
			ScheduleGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ScheduleGridSetting()
	{
		try
		{
			grdSummaryExportScheduleInfo.RootTable.Columns["ContractId"].Visible = false;
			grdSummaryExportScheduleInfo.RootTable.Columns["ScheduleId"].Visible = false;
			grdSummaryExportScheduleInfo.RootTable.Columns["SupCustId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdSummaryExportScheduleInfo, 2, 2);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdSummaryReportFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(cmbsummeryJobOrderNo.Value);
			obj.PlantId = Conversion.ToInt(CmbPlantNameSummary.Value);
			if (txtDateFromSummary.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)txtDateFromSummary.Value);
			}
			if (txtDateToSummary.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)txtDateToSummary.Value);
			}
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
			}
			dtSummerReport = InvFoodProductionReports.InvFoodProductionRecoverySummeryReport(obj);
			if (dtSummerReport != null && dtSummerReport.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("TransactionType", typeof(string));
				dtcol.Columns.Add("PlantName", typeof(string));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("CropYear", typeof(string));
				dtcol.Columns.Add("PackSize", typeof(string));
				dtcol.Columns.Add("JobLot", typeof(string));
				dtcol.Columns.Add("QtyTotal", typeof(double));
				dtcol.Columns.Add("NetWeightTotal", typeof(double));
				dtcol.Columns.Add("Amount", typeof(double));
				dtcol.Columns.Add("TransactionRate", typeof(double));
				dtcol.Columns.Add("WeightPrcnt", typeof(double));
				dtcol.Columns.Add("NetRate", typeof(double));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtSummerReport.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtSummerReport.Rows[i]["TranType"], dtSummerReport.Rows[i]["PlantName"], dtSummerReport.Rows[i]["ItemName"], dtSummerReport.Rows[i]["CropBatch"], dtSummerReport.Rows[i]["UOMCode"], dtSummerReport.Rows[i]["JobLotCode"], dtSummerReport.Rows[i]["NetQty"], dtSummerReport.Rows[i]["NetWeight"], dtSummerReport.Rows[i]["Amount"], dtSummerReport.Rows[i]["TranRate"], dtSummerReport.Rows[i]["WeightPrcnt"], dtSummerReport.Rows[i]["NetRate"]);
				}
				grdSummaryDetail.DataSource = dtcol;
				grdSummaryDetail.RetrieveStructure();
				SummeryHistoryGridSetting();
			}
			else
			{
				grdSummaryDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SummeryHistoryGridSetting()
	{
		try
		{
			if (!formright.DoHaveCanRateandAmount)
			{
				grdSummaryDetail.RootTable.Columns["TransactionRate"].Visible = false;
				grdSummaryDetail.RootTable.Columns["Amount"].Visible = false;
				grdSummaryDetail.RootTable.Columns["NetRate"].Visible = false;
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grdSummaryDetail, 2, 2);
			grdSummaryDetail.GroupByBoxVisible = true;
			grdSummaryDetail.RootTable.Groups.Add("TransactionType");
			grdSummaryDetail.RootTable.GroupTotals = (GroupTotals)2;
			grdSummaryDetail.RootTable.Columns["TransactionType"].Visible = false;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDocWiseSummeryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(cmbsummeryJobOrderNo.Value);
			obj.PlantId = Conversion.ToInt(CmbPlantNameSummary.Value);
			if (txtDateFromSummary.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)txtDateFromSummary.Value);
			}
			if (txtDateToSummary.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)txtDateToSummary.Value);
			}
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
			}
			dtDocWiseSummery = InvFoodProductionReports.FoodProduction_DocWiseSummeryReport(obj);
			if (dtDocWiseSummery.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("SortNo", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("TransactionType", typeof(string));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("PackSize", typeof(string));
				dtcol.Columns.Add("CropYear", typeof(string));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Columns.Add("JobLot", typeof(string));
				dtcol.Columns.Add("NetQty", typeof(double));
				dtcol.Columns.Add("NetWeight", typeof(double));
				dtcol.Columns.Add("Amount", typeof(double));
				dtcol.Columns.Add("TranRate", typeof(double));
				dtcol.Columns.Add("WeightPrcnt", typeof(double));
				dtcol.Columns.Add("NetRate", typeof(double));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtDocWiseSummery.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtDocWiseSummery.Rows[i]["SortNo"], dtDocWiseSummery.Rows[i]["DocCode"], Conversion.ToDateTime(dtDocWiseSummery.Rows[i]["DocDate"]).ToShortDateString(), dtDocWiseSummery.Rows[i]["TranType"], dtDocWiseSummery.Rows[i]["ItemName"], dtDocWiseSummery.Rows[i]["UOMCode"], dtDocWiseSummery.Rows[i]["CropBatch"], dtDocWiseSummery.Rows[i]["remarks"], dtDocWiseSummery.Rows[i]["JobLotCode"], dtDocWiseSummery.Rows[i]["NetQty"], dtDocWiseSummery.Rows[i]["NetWeight"], dtDocWiseSummery.Rows[i]["Amount"], dtDocWiseSummery.Rows[i]["TranRate"], dtDocWiseSummery.Rows[i]["WeightPrcnt"], dtDocWiseSummery.Rows[i]["NetRate"]);
				}
				grdSummaryDocWiseDetail.DataSource = dtcol;
				grdSummaryDocWiseDetail.RetrieveStructure();
				grdDocWiseSummerySetting();
			}
			else
			{
				grdSummaryDocWiseDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDocWiseSummerySetting()
	{
		try
		{
			if (!formright.DoHaveCanRateandAmount)
			{
				grdSummaryDocWiseDetail.RootTable.Columns["TranRate"].Visible = false;
				grdSummaryDocWiseDetail.RootTable.Columns["TotalAmountBySortNo"].Visible = false;
				grdSummaryDocWiseDetail.RootTable.Columns["Amount"].Visible = false;
				grdSummaryDocWiseDetail.RootTable.Columns["NetRate"].Visible = false;
			}
			grdSummaryDocWiseDetail.GroupByBoxVisible = true;
			grdSummaryDocWiseDetail.GroupTotals = (GroupTotals)2;
			grdSummaryDocWiseDetail.RootTable.Groups.Add("TransactionType");
			GridEX_Helper.GridWrappingAndColumnSettings(grdSummaryDocWiseDetail, 2, 2);
			ctrlGrdBar2_Load(null, null);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdSummaryDetail).Name);
			if (fs != null)
			{
				grdSummaryDetail.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdSummaryDocWiseDetail).Name);
			if (fs != null)
			{
				grdSummaryDocWiseDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdSummery).Name);
			if (fs != null)
			{
				grdSummery.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProductionSummaryReport_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnRefreshSummary_Click(object sender, EventArgs e)
	{
		try
		{
			JobOrderNoFillForSummery();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintForDetailGridData_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtDocWiseSummery.Rows.Count > 0 && dtDocWiseSummery != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtDocWiseSummery, "625_FoodProduction_DocWiseSummeryReport.rpt");
			}
			else
			{
				MessageBox.Show("Record Not Found For DisPlay");
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
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Expected O, but got Unknown
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Expected O, but got Unknown
		GridEXLayout grdSummaryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSummaryDocWiseDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSummaryExportScheduleInfo_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSummery_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production_Reports.ProductionSummaryReport));
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance13 = new Appearance();
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
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl5 = new System.Windows.Forms.TabControl();
		this.tabPage7 = new System.Windows.Forms.TabPage();
		this.grdSummaryDetail = new GridEX();
		this.panel47 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage8 = new System.Windows.Forms.TabPage();
		this.grdSummaryDocWiseDetail = new GridEX();
		this.panel26 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdSummaryExportScheduleInfo = new GridEX();
		this.panel50 = new System.Windows.Forms.Panel();
		this.label174 = new System.Windows.Forms.Label();
		this.panel86 = new System.Windows.Forms.Panel();
		this.label173 = new System.Windows.Forms.Label();
		this.label177 = new System.Windows.Forms.Label();
		this.txtSummaryJobStartedDate = new System.Windows.Forms.DateTimePicker();
		this.label180 = new System.Windows.Forms.Label();
		this.txtSummarySettlementDate = new System.Windows.Forms.DateTimePicker();
		this.label179 = new System.Windows.Forms.Label();
		this.txtSummaryJobOrderNo = new System.Windows.Forms.TextBox();
		this.label175 = new System.Windows.Forms.Label();
		this.txtSummaryJobOrderStatus = new System.Windows.Forms.TextBox();
		this.label178 = new System.Windows.Forms.Label();
		this.txtSummarySettlementStatus = new System.Windows.Forms.TextBox();
		this.label176 = new System.Windows.Forms.Label();
		this.txtSummaryLotCode = new System.Windows.Forms.TextBox();
		this.txtSummaryjobOrderDocNo = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdSummery = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label39 = new System.Windows.Forms.Label();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel20 = new System.Windows.Forms.Panel();
		this.CmbPlantNameSummary = new UltraCombo();
		this.label87 = new System.Windows.Forms.Label();
		this.cmbBranchName = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtDateFromSummary = new System.Windows.Forms.DateTimePicker();
		this.txtDateToSummary = new System.Windows.Forms.DateTimePicker();
		this.btnShow = new System.Windows.Forms.Button();
		this.cmbsummeryJobOrderNo = new UltraCombo();
		this.label68 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.label36 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRefreshSummary = new System.Windows.Forms.ToolStripButton();
		this.btnProductionRecoveryReport = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintWithLab602_01 = new System.Windows.Forms.ToolStripButton();
		this.tsDropDowngrnWiseInput = new System.Windows.Forms.ToolStripDropDownButton();
		this.btnPrintForDetailGridData = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl5.SuspendLayout();
		this.tabPage7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummaryDetail).BeginInit();
		this.panel47.SuspendLayout();
		this.tabPage8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummaryDocWiseDetail).BeginInit();
		this.panel26.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummaryExportScheduleInfo).BeginInit();
		this.panel50.SuspendLayout();
		this.panel86.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummery).BeginInit();
		this.panel21.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantNameSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsummeryJobOrderNo).BeginInit();
		this.panel19.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl5);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel20);
		this.panel1.Controls.Add(this.panel19);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1289, 629);
		this.panel1.TabIndex = 0;
		this.tabControl5.Controls.Add(this.tabPage7);
		this.tabControl5.Controls.Add(this.tabPage8);
		this.tabControl5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl5.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl5.Location = new System.Drawing.Point(0, 329);
		this.tabControl5.Name = "tabControl5";
		this.tabControl5.SelectedIndex = 0;
		this.tabControl5.Size = new System.Drawing.Size(1289, 300);
		this.tabControl5.TabIndex = 126;
		this.tabPage7.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummaryDetail);
		this.tabPage7.Controls.Add(this.panel47);
		this.tabPage7.Location = new System.Drawing.Point(4, 25);
		this.tabPage7.Name = "tabPage7";
		this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage7.Size = new System.Drawing.Size(1281, 271);
		this.tabPage7.TabIndex = 0;
		this.tabPage7.Text = "Summary";
		this.tabPage7.UseVisualStyleBackColor = true;
		this.grdSummaryDetail.AllowEdit = (InheritableBoolean)2;
		grdSummaryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummaryDetail.DesignTimeLayout = grdSummaryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummaryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummaryDetail.DynamicFiltering = true;
		this.grdSummaryDetail.FilterMode = (FilterMode)2;
		this.grdSummaryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSummaryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummaryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSummaryDetail).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grdSummaryDetail).Name = "grdSummaryDetail";
		((ScrollableBase)this.grdSummaryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummaryDetail).Size = new System.Drawing.Size(1275, 244);
		((System.Windows.Forms.Control)(object)this.grdSummaryDetail).TabIndex = 0;
		this.grdSummaryDetail.TotalRow = (InheritableBoolean)1;
		this.grdSummaryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummaryDetail).VisualStyle = (VisualStyle)4;
		this.panel47.BackColor = System.Drawing.Color.Teal;
		this.panel47.Controls.Add(this.ctrlGrdBar1);
		this.panel47.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel47.Location = new System.Drawing.Point(3, 3);
		this.panel47.Name = "panel47";
		this.panel47.Size = new System.Drawing.Size(1275, 21);
		this.panel47.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1232, 0);
		this.ctrlGrdBar1.MyGrid = this.grdSummaryDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 20);
		this.ctrlGrdBar1.TabIndex = 60;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPage8.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummaryDocWiseDetail);
		this.tabPage8.Controls.Add(this.panel26);
		this.tabPage8.Location = new System.Drawing.Point(4, 25);
		this.tabPage8.Name = "tabPage8";
		this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage8.Size = new System.Drawing.Size(1281, 271);
		this.tabPage8.TabIndex = 1;
		this.tabPage8.Text = "Detail";
		this.tabPage8.UseVisualStyleBackColor = true;
		this.grdSummaryDocWiseDetail.AllowEdit = (InheritableBoolean)2;
		grdSummaryDocWiseDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummaryDocWiseDetail.DesignTimeLayout = grdSummaryDocWiseDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummaryDocWiseDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummaryDocWiseDetail.DynamicFiltering = true;
		this.grdSummaryDocWiseDetail.FilterMode = (FilterMode)2;
		this.grdSummaryDocWiseDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSummaryDocWiseDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummaryDocWiseDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdSummaryDocWiseDetail).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.grdSummaryDocWiseDetail).Name = "grdSummaryDocWiseDetail";
		((ScrollableBase)this.grdSummaryDocWiseDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummaryDocWiseDetail).Size = new System.Drawing.Size(1275, 243);
		((System.Windows.Forms.Control)(object)this.grdSummaryDocWiseDetail).TabIndex = 4;
		this.grdSummaryDocWiseDetail.TotalRow = (InheritableBoolean)1;
		this.grdSummaryDocWiseDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummaryDocWiseDetail).VisualStyle = (VisualStyle)4;
		this.panel26.BackColor = System.Drawing.Color.Teal;
		this.panel26.Controls.Add(this.ctrlGrdBar2);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel26.Location = new System.Drawing.Point(3, 3);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1275, 22);
		this.panel26.TabIndex = 5;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1231, 1);
		this.ctrlGrdBar2.MyGrid = this.grdSummaryDocWiseDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(41, 20);
		this.ctrlGrdBar2.TabIndex = 60;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel86);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 105);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1289, 224);
		this.panel2.TabIndex = 125;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummaryExportScheduleInfo);
		this.panel4.Controls.Add(this.panel50);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(998, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(291, 224);
		this.panel4.TabIndex = 65;
		this.grdSummaryExportScheduleInfo.AllowEdit = (InheritableBoolean)2;
		grdSummaryExportScheduleInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummaryExportScheduleInfo.DesignTimeLayout = grdSummaryExportScheduleInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummaryExportScheduleInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummaryExportScheduleInfo.DynamicFiltering = true;
		this.grdSummaryExportScheduleInfo.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdSummaryExportScheduleInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummaryExportScheduleInfo.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSummaryExportScheduleInfo).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdSummaryExportScheduleInfo).Name = "grdSummaryExportScheduleInfo";
		((ScrollableBase)this.grdSummaryExportScheduleInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummaryExportScheduleInfo).Size = new System.Drawing.Size(291, 201);
		((System.Windows.Forms.Control)(object)this.grdSummaryExportScheduleInfo).TabIndex = 3;
		this.grdSummaryExportScheduleInfo.TotalRow = (InheritableBoolean)1;
		this.grdSummaryExportScheduleInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummaryExportScheduleInfo).VisualStyle = (VisualStyle)4;
		this.panel50.BackColor = System.Drawing.Color.Teal;
		this.panel50.Controls.Add(this.label174);
		this.panel50.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel50.Location = new System.Drawing.Point(0, 0);
		this.panel50.Name = "panel50";
		this.panel50.Size = new System.Drawing.Size(291, 23);
		this.panel50.TabIndex = 2;
		this.label174.AutoSize = true;
		this.label174.BackColor = System.Drawing.Color.Teal;
		this.label174.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label174.ForeColor = System.Drawing.Color.White;
		this.label174.Location = new System.Drawing.Point(-1, 3);
		this.label174.Name = "label174";
		this.label174.Size = new System.Drawing.Size(207, 16);
		this.label174.TabIndex = 178;
		this.label174.Text = " (Export Schedule Information)";
		this.panel86.Controls.Add(this.label173);
		this.panel86.Controls.Add(this.label177);
		this.panel86.Controls.Add(this.txtSummaryJobStartedDate);
		this.panel86.Controls.Add(this.label180);
		this.panel86.Controls.Add(this.txtSummarySettlementDate);
		this.panel86.Controls.Add(this.label179);
		this.panel86.Controls.Add(this.txtSummaryJobOrderNo);
		this.panel86.Controls.Add(this.label175);
		this.panel86.Controls.Add(this.txtSummaryJobOrderStatus);
		this.panel86.Controls.Add(this.label178);
		this.panel86.Controls.Add(this.txtSummarySettlementStatus);
		this.panel86.Controls.Add(this.label176);
		this.panel86.Controls.Add(this.txtSummaryLotCode);
		this.panel86.Controls.Add(this.txtSummaryjobOrderDocNo);
		this.panel86.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel86.Location = new System.Drawing.Point(765, 0);
		this.panel86.Name = "panel86";
		this.panel86.Size = new System.Drawing.Size(233, 224);
		this.panel86.TabIndex = 64;
		this.label173.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label173.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label173.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label173.Location = new System.Drawing.Point(5, 4);
		this.label173.Name = "label173";
		this.label173.Size = new System.Drawing.Size(223, 23);
		this.label173.TabIndex = 62;
		this.label173.Text = "Job Order No";
		this.label173.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label177.AutoSize = true;
		this.label177.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label177.Location = new System.Drawing.Point(5, 58);
		this.label177.Name = "label177";
		this.label177.Size = new System.Drawing.Size(103, 15);
		this.label177.TabIndex = 62;
		this.label177.Text = "Job Order Doc No";
		this.txtSummaryJobStartedDate.Checked = false;
		this.txtSummaryJobStartedDate.CustomFormat = "dd-MMM-yy";
		this.txtSummaryJobStartedDate.Enabled = false;
		this.txtSummaryJobStartedDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtSummaryJobStartedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtSummaryJobStartedDate.Location = new System.Drawing.Point(110, 80);
		this.txtSummaryJobStartedDate.Name = "txtSummaryJobStartedDate";
		this.txtSummaryJobStartedDate.Size = new System.Drawing.Size(118, 22);
		this.txtSummaryJobStartedDate.TabIndex = 2;
		this.label180.AutoSize = true;
		this.label180.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label180.Location = new System.Drawing.Point(7, 183);
		this.label180.Name = "label180";
		this.label180.Size = new System.Drawing.Size(54, 15);
		this.label180.TabIndex = 62;
		this.label180.Text = "Lot Code";
		this.txtSummarySettlementDate.Checked = false;
		this.txtSummarySettlementDate.CustomFormat = "dd-MMM-yy";
		this.txtSummarySettlementDate.Enabled = false;
		this.txtSummarySettlementDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtSummarySettlementDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtSummarySettlementDate.Location = new System.Drawing.Point(110, 155);
		this.txtSummarySettlementDate.Name = "txtSummarySettlementDate";
		this.txtSummarySettlementDate.Size = new System.Drawing.Size(118, 22);
		this.txtSummarySettlementDate.TabIndex = 5;
		this.label179.AutoSize = true;
		this.label179.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label179.Location = new System.Drawing.Point(5, 159);
		this.label179.Name = "label179";
		this.label179.Size = new System.Drawing.Size(93, 15);
		this.label179.TabIndex = 62;
		this.label179.Text = "Settlement Date";
		this.txtSummaryJobOrderNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSummaryJobOrderNo.Enabled = false;
		this.txtSummaryJobOrderNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSummaryJobOrderNo.Location = new System.Drawing.Point(5, 29);
		this.txtSummaryJobOrderNo.Name = "txtSummaryJobOrderNo";
		this.txtSummaryJobOrderNo.Size = new System.Drawing.Size(223, 23);
		this.txtSummaryJobOrderNo.TabIndex = 0;
		this.label175.AutoSize = true;
		this.label175.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label175.Location = new System.Drawing.Point(5, 84);
		this.label175.Name = "label175";
		this.label175.Size = new System.Drawing.Size(95, 15);
		this.label175.TabIndex = 62;
		this.label175.Text = "Job Started Date";
		this.txtSummaryJobOrderStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSummaryJobOrderStatus.Enabled = false;
		this.txtSummaryJobOrderStatus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSummaryJobOrderStatus.Location = new System.Drawing.Point(110, 105);
		this.txtSummaryJobOrderStatus.Name = "txtSummaryJobOrderStatus";
		this.txtSummaryJobOrderStatus.Size = new System.Drawing.Size(118, 23);
		this.txtSummaryJobOrderStatus.TabIndex = 3;
		this.label178.AutoSize = true;
		this.label178.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label178.Location = new System.Drawing.Point(5, 134);
		this.label178.Name = "label178";
		this.label178.Size = new System.Drawing.Size(101, 15);
		this.label178.TabIndex = 62;
		this.label178.Text = "Settlement Status";
		this.txtSummarySettlementStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSummarySettlementStatus.Enabled = false;
		this.txtSummarySettlementStatus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSummarySettlementStatus.Location = new System.Drawing.Point(110, 130);
		this.txtSummarySettlementStatus.Name = "txtSummarySettlementStatus";
		this.txtSummarySettlementStatus.Size = new System.Drawing.Size(118, 23);
		this.txtSummarySettlementStatus.TabIndex = 4;
		this.label176.AutoSize = true;
		this.label176.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label176.Location = new System.Drawing.Point(5, 109);
		this.label176.Name = "label176";
		this.label176.Size = new System.Drawing.Size(95, 15);
		this.label176.TabIndex = 62;
		this.label176.Text = "Job Order Status";
		this.txtSummaryLotCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSummaryLotCode.Enabled = false;
		this.txtSummaryLotCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSummaryLotCode.Location = new System.Drawing.Point(110, 179);
		this.txtSummaryLotCode.Name = "txtSummaryLotCode";
		this.txtSummaryLotCode.Size = new System.Drawing.Size(118, 23);
		this.txtSummaryLotCode.TabIndex = 6;
		this.txtSummaryjobOrderDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSummaryjobOrderDocNo.Enabled = false;
		this.txtSummaryjobOrderDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSummaryjobOrderDocNo.Location = new System.Drawing.Point(110, 54);
		this.txtSummaryjobOrderDocNo.Name = "txtSummaryjobOrderDocNo";
		this.txtSummaryjobOrderDocNo.Size = new System.Drawing.Size(118, 23);
		this.txtSummaryjobOrderDocNo.TabIndex = 1;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummery);
		this.panel3.Controls.Add(this.panel21);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(765, 224);
		this.panel3.TabIndex = 0;
		this.grdSummery.AllowEdit = (InheritableBoolean)2;
		grdSummery_DesignTimeLayout.LayoutString = resources.GetString("grdSummery_DesignTimeLayout.LayoutString");
		this.grdSummery.DesignTimeLayout = grdSummery_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummery).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummery.DynamicFiltering = true;
		this.grdSummery.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdSummery).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummery.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSummery).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdSummery).Name = "grdSummery";
		((ScrollableBase)this.grdSummery).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummery).Size = new System.Drawing.Size(765, 202);
		((System.Windows.Forms.Control)(object)this.grdSummery).TabIndex = 1;
		this.grdSummery.TotalRow = (InheritableBoolean)1;
		this.grdSummery.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummery).VisualStyle = (VisualStyle)4;
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.label39);
		this.panel21.Controls.Add(this.ctrlGrdBar5);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 0);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(765, 22);
		this.panel21.TabIndex = 122;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label39.ForeColor = System.Drawing.Color.White;
		this.label39.Location = new System.Drawing.Point(8, 3);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(114, 16);
		this.label39.TabIndex = 178;
		this.label39.Text = "Summary Values";
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(721, 0);
		this.ctrlGrdBar5.MyGrid = this.grdSummery;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(41, 23);
		this.ctrlGrdBar5.TabIndex = 60;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantNameSummary);
		this.panel20.Controls.Add(this.label87);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.panel20.Controls.Add(this.label13);
		this.panel20.Controls.Add(this.label24);
		this.panel20.Controls.Add(this.label3);
		this.panel20.Controls.Add(this.txtDateFromSummary);
		this.panel20.Controls.Add(this.txtDateToSummary);
		this.panel20.Controls.Add(this.btnShow);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo);
		this.panel20.Controls.Add(this.label68);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 53);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1289, 52);
		this.panel20.TabIndex = 0;
		this.CmbPlantNameSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantNameSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantNameSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantNameSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantNameSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantNameSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantNameSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantNameSummary).Location = new System.Drawing.Point(461, 22);
		((System.Windows.Forms.Control)(object)this.CmbPlantNameSummary).Name = "CmbPlantNameSummary";
		((System.Windows.Forms.Control)(object)this.CmbPlantNameSummary).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantNameSummary).TabIndex = 2;
		this.label87.AutoSize = true;
		this.label87.BackColor = System.Drawing.Color.Transparent;
		this.label87.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label87.ForeColor = System.Drawing.Color.Black;
		this.label87.Location = new System.Drawing.Point(463, 5);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(69, 15);
		this.label87.TabIndex = 602;
		this.label87.Text = "Plant Name";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(6, 22);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(6, 5);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 600;
		this.label13.Text = "Branch Name";
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Black;
		this.label24.Location = new System.Drawing.Point(662, 5);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(66, 15);
		this.label24.TabIndex = 121;
		this.label24.Text = "From Date";
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(806, 5);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(50, 15);
		this.label3.TabIndex = 120;
		this.label3.Text = "To Date";
		this.txtDateFromSummary.Checked = false;
		this.txtDateFromSummary.CustomFormat = "dd-MMM-yy";
		this.txtDateFromSummary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFromSummary.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFromSummary.Location = new System.Drawing.Point(662, 24);
		this.txtDateFromSummary.Name = "txtDateFromSummary";
		this.txtDateFromSummary.ShowCheckBox = true;
		this.txtDateFromSummary.Size = new System.Drawing.Size(145, 23);
		this.txtDateFromSummary.TabIndex = 3;
		this.txtDateToSummary.Checked = false;
		this.txtDateToSummary.CustomFormat = "dd-MMM-yy";
		this.txtDateToSummary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateToSummary.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateToSummary.Location = new System.Drawing.Point(809, 24);
		this.txtDateToSummary.Name = "txtDateToSummary";
		this.txtDateToSummary.ShowCheckBox = true;
		this.txtDateToSummary.Size = new System.Drawing.Size(145, 23);
		this.txtDateToSummary.TabIndex = 4;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(954, 22);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(63, 26);
		this.btnShow.TabIndex = 5;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.cmbsummeryJobOrderNo.AllowNull = (DefaultableBoolean)1;
		this.cmbsummeryJobOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsummeryJobOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsummeryJobOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsummeryJobOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Location = new System.Drawing.Point(227, 22);
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Name = "cmbsummeryJobOrderNo";
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Size = new System.Drawing.Size(233, 26);
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Leave += new System.EventHandler(cmbsummeryJobOrderNo_TextChanged);
		this.label68.AutoSize = true;
		this.label68.BackColor = System.Drawing.Color.White;
		this.label68.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Black;
		this.label68.Location = new System.Drawing.Point(227, 5);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(81, 15);
		this.label68.TabIndex = 117;
		this.label68.Text = "Job Order No";
		this.panel19.BackColor = System.Drawing.Color.Teal;
		this.panel19.Controls.Add(this.label36);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 27);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1289, 26);
		this.panel19.TabIndex = 120;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Tahoma", 12.5f, System.Drawing.FontStyle.Bold);
		this.label36.ForeColor = System.Drawing.Color.White;
		this.label36.Location = new System.Drawing.Point(2, 2);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(190, 21);
		this.label36.TabIndex = 178;
		this.label36.Text = "Production Summary";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripButton1, this.toolStripSeparator2, this.btnRefreshSummary, this.btnProductionRecoveryReport, this.BtnPrintWithLab602_01, this.tsDropDowngrnWiseInput, this.btnPrintForDetailGridData });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1289, 27);
		this.toolStrip3.TabIndex = 58;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnRefreshSummary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshSummary.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshSummary.Name = "btnRefreshSummary";
		this.btnRefreshSummary.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshSummary.Text = "Refresh";
		this.btnRefreshSummary.Click += new System.EventHandler(btnRefreshSummary_Click);
		this.btnProductionRecoveryReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnProductionRecoveryReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnProductionRecoveryReport.Name = "btnProductionRecoveryReport";
		this.btnProductionRecoveryReport.Size = new System.Drawing.Size(166, 24);
		this.btnProductionRecoveryReport.Text = "602-RecoveryReport";
		this.btnProductionRecoveryReport.Click += new System.EventHandler(btnProductionRecoveryReport_Click);
		this.BtnPrintWithLab602_01.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintWithLab602_01.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintWithLab602_01.Name = "BtnPrintWithLab602_01";
		this.BtnPrintWithLab602_01.Size = new System.Drawing.Size(190, 24);
		this.BtnPrintWithLab602_01.Text = "602_01-RecoveryReport";
		this.BtnPrintWithLab602_01.Click += new System.EventHandler(BtnPrintWithLab602_01_Click);
		this.tsDropDowngrnWiseInput.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDowngrnWiseInput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDowngrnWiseInput.Name = "tsDropDowngrnWiseInput";
		this.tsDropDowngrnWiseInput.Size = new System.Drawing.Size(117, 24);
		this.tsDropDowngrnWiseInput.Text = "Input Detail";
		this.tsDropDowngrnWiseInput.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDowngrnWiseInput_DropDownItemClicked);
		this.btnPrintForDetailGridData.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintForDetailGridData.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintForDetailGridData.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintForDetailGridData.Name = "btnPrintForDetailGridData";
		this.btnPrintForDetailGridData.Size = new System.Drawing.Size(226, 24);
		this.btnPrintForDetailGridData.Text = "625-Print For Detail Grid Data";
		this.btnPrintForDetailGridData.Click += new System.EventHandler(btnPrintForDetailGridData_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1289, 629);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ProductionSummaryReport";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ProductionSummaryReport_FormClosed);
		base.Load += new System.EventHandler(ProductionSummaryReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ProductionSummaryReport_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.tabControl5.ResumeLayout(false);
		this.tabPage7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummaryDetail).EndInit();
		this.panel47.ResumeLayout(false);
		this.tabPage8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummaryDocWiseDetail).EndInit();
		this.panel26.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummaryExportScheduleInfo).EndInit();
		this.panel50.ResumeLayout(false);
		this.panel50.PerformLayout();
		this.panel86.ResumeLayout(false);
		this.panel86.PerformLayout();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummery).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantNameSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsummeryJobOrderNo).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
