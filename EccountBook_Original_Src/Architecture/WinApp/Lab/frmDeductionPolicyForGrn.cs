using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.pcc;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Lab;

public class frmDeductionPolicyForGrn : Form
{
	private int RecId;

	public bool UpdateMode;

	private bool SubsidiaryAccountAllownOnVouchers;

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private DataTable dtContr = new DataTable();

	private DataTable dtWagesGroup = new DataTable();

	private DataTable dtItems = new DataTable();

	private Rightsobjects formrights = new Rightsobjects();

	private IContainer components;

	private TabControl tabWagesSchedule;

	private TabPage tabPage1;

	internal GridEX grdWagesSchedule;

	private Panel panel2;

	private TextBox txtMeatureMTo;

	private TextBox txtMeatureMFrom;

	private Label label12;

	private Panel panel6;

	private Label label15;

	private ToolStrip btnAttachments;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private TabPage tabPage2;

	private DateTimePicker txtdate;

	private Label label3;

	private Panel panel3;

	internal GridEX gridhistory;

	private Panel panel1;

	private Label label4;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btnshortcutkeys;

	private Panel panel5;

	private Label label5;

	private Panel panel4;

	private GroupBox groupBox1;

	private BackgroundWorker backgroundWorker1;

	private ToolStripButton btnrefrsh;

	private ToolStripButton btnDelete;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private Label label6;

	private Button btnshowHistory;

	private DateTimePicker txtToDateHistory;

	private Label label20;

	private DateTimePicker FromDateHistory;

	private Label label21;

	internal UltraCombo CmbWagesAccountHistory;

	private Label label22;

	internal UltraCombo CmbContractoryHistory;

	private Label label23;

	private Panel panel12;

	private Label label24;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	internal UltraCombo CmbPolicyType;

	private CheckBox ChkIsActive;

	private Label label1;

	private TextBox txtConditionDescription;

	private Label label2;

	public frmDeductionPolicyForGrn(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmContractWagesSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(815, 749);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			dtItems.Columns.Add("Id", typeof(int));
			dtItems.Columns.Add("Name");
			dtWagesGroup.Columns.Add("Id", typeof(int));
			dtWagesGroup.Columns.Add("Name");
			txtdate.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdate_ValueChanged(object sender, EventArgs e)
	{
	}

	private void FormRest()
	{
		try
		{
			RecId = 0;
			txtMeatureMTo.Text = "0";
			txtMeatureMFrom.Text = "0";
			grdWagesSchedule.ClearStructure();
			if (Conversion.ToInt(CmbPolicyType.Value) > 0)
			{
				BindgrdWagesSchedule();
			}
			txtdate.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			FormRest();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void isActive_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "ctrlGrdBar1")
			{
				txtdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnrefrsh_Click(object sender, EventArgs e)
	{
	}

	private void frmContractWagesSchedule_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtMeatureMFrom.Text.Trim() == string.Empty || txtMeatureMFrom.Text.Trim() == "0")
		{
			MessageBox.Show("Range From Field Required");
			txtMeatureMFrom.Focus();
			return false;
		}
		if (txtMeatureMTo.Text.Trim() == string.Empty || txtMeatureMTo.Text.Trim() == "0")
		{
			MessageBox.Show("Range To Field Required");
			txtMeatureMTo.Focus();
			return false;
		}
		return true;
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			ContractorWagesRateSchedule wagesSchedule = new ContractorWagesRateSchedule();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				wagesSchedule.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			wagesSchedule.OrganizationId = UserAccount.OrganizationId;
			wagesSchedule.CompanyId = UserAccount.CompanyId;
			wagesSchedule.BranchesId = UserAccount.BranchesId;
			wagesSchedule.ProjectsId = UserAccount.BranchesId;
			wagesSchedule.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			wagesSchedule.EntryDate = DateTime.Now;
			wagesSchedule.ModifyDate = DateTime.Now;
			wagesSchedule.EntryUserId = UserAccount.ID;
			wagesSchedule.ModifyUserId = UserAccount.ID;
			wagesSchedule.EffectiveDate = Conversion.ToDateTime((object)txtdate.Value);
			wagesSchedule.MeatureMFrom = Conversion.ToInt((object)txtMeatureMFrom.Text);
			wagesSchedule.MatureMTo = Conversion.ToInt((object)txtMeatureMTo.Text);
			wagesSchedule.IsActive = true;
			wagesSchedule.ActionId = 1;
			int success = ContractorWagesRateSchedule.Save(wagesSchedule);
			wagesSchedule.Id = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully.");
			}
			else
			{
				MessageBox.Show("Data Save Successfully.");
			}
			FormRest();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadbyId(int Id)
	{
		try
		{
			RecId = Id;
			ContractorWagesRateSchedule obj = ContractorWagesRateSchedule.ReadById(RecId);
			if (obj != null)
			{
				tabWagesSchedule.SelectedIndex = 0;
				txtdate.Value = Conversion.ToDateTime((object)obj.EffectiveDate);
				txtMeatureMFrom.Text = Conversion.ToDouble((object)obj.MeatureMFrom).ToString();
				UpdateMode = true;
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				btnDelete.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ContractorWagesRateSchedule.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Seccessfully");
					FormRest();
				}
			}
			else
			{
				MessageBox.Show("No Record Found For Deletion");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindgrdWagesSchedule(int WagesActivityId = 0, int ContractorId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		try
		{
			DataTable dtHistory = ContractorWagesRateSchedule.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				ContractorWagesActivityId = WagesActivityId,
				ContractorId = ContractorId,
				ActionId = 1
			});
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("EffectiveDate", typeof(DateTime));
				dt.Columns.Add("ContractorName", typeof(string));
				dt.Columns.Add("ContractorWagesActivityId", typeof(int));
				dt.Columns.Add("WagesActivity", typeof(string));
				dt.Columns.Add("ContractorWagesGroupId", typeof(int));
				dt.Columns.Add("ItemWagesGroupName", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("RangeUomId", typeof(int));
				dt.Columns.Add("RangeUom", typeof(string));
				dt.Columns.Add("RangeFrom", typeof(string));
				dt.Columns.Add("RangeTo", typeof(string));
				dt.Columns.Add("Qty", typeof(decimal));
				dt.Columns.Add("CompanyRate", typeof(decimal));
				dt.Columns.Add("WagesRate", typeof(decimal));
				dt.Clear();
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtHistory.Rows[i]["Id"]), Conversion.ToDateTime(dtHistory.Rows[i]["EffectiveDate"]).ToShortDateString(), Conversion.ToString(dtHistory.Rows[i]["ContractorName"]), Conversion.ToInt(dtHistory.Rows[i]["ContractorWagesActivityId"]), Conversion.ToString(dtHistory.Rows[i]["WagesActivity"]), Conversion.ToInt(dtHistory.Rows[i]["ContractorWagesGroupId"]), Conversion.ToString(dtHistory.Rows[i]["ItemWagesGroupName"]), Conversion.ToInt(dtHistory.Rows[i]["ItemId"]), Conversion.ToString(dtHistory.Rows[i]["ItemName"]), Conversion.ToInt(dtHistory.Rows[i]["RangeUomId"]), Conversion.ToString(dtHistory.Rows[i]["RangeUom"]), Conversion.ToInt(dtHistory.Rows[i]["MeatureMFrom"]) + "-" + Conversion.ToInt(dtHistory.Rows[i]["MeatureSFrom"]), Conversion.ToInt(dtHistory.Rows[i]["MatureMTo"]) + "-" + Conversion.ToInt(dtHistory.Rows[i]["MatureSTo"]), Conversion.ToDecimal(dtHistory.Rows[i]["Qty"]), Conversion.ToDecimal(dtHistory.Rows[i]["CompanyRate"]), Conversion.ToDecimal(dtHistory.Rows[i]["Rate"]));
				}
				grdWagesSchedule.DataSource = dt;
				grdWagesSchedule.RetrieveStructure();
				grdWagesSetting();
			}
			else
			{
				grdWagesSchedule.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWagesSchedule_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadbyId(Conversion.ToInt(grdWagesSchedule.CurrentRow.Cells["Id"].Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWagesSetting()
	{
		try
		{
			grdWagesSchedule.RootTable.Columns["Id"].Visible = false;
			grdWagesSchedule.RootTable.Columns["RangeUomId"].Visible = false;
			grdWagesSchedule.RootTable.Columns["ItemId"].Visible = false;
			grdWagesSchedule.RootTable.Columns["ContractorWagesActivityId"].Visible = false;
			grdWagesSchedule.RootTable.Columns["ContractorWagesGroupId"].Visible = false;
			grdWagesSchedule.RootTable.Columns["WagesRate"].AggregateFunction = (AggregateFunction)3;
			grdWagesSchedule.RootTable.Columns["WagesRate"].TextAlignment = (TextAlignment)3;
			grdWagesSchedule.RootTable.Columns["WagesRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdWagesSchedule.RootTable.Columns["WagesRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdWagesSchedule.RootTable.Columns["CompanyRate"].AggregateFunction = (AggregateFunction)3;
			grdWagesSchedule.RootTable.Columns["CompanyRate"].TextAlignment = (TextAlignment)3;
			grdWagesSchedule.RootTable.Columns["CompanyRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdWagesSchedule.RootTable.Columns["CompanyRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdWagesSchedule.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)3;
			grdWagesSchedule.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdWagesSchedule.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdWagesSchedule.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdWagesSchedule);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabWagesSchedule_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabWagesSchedule.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
		else
		{
			txtdate.Focus();
		}
	}

	private void Resethistory()
	{
		try
		{
			FromDateHistory.Text = string.Empty;
			txtToDateHistory.Text = string.Empty;
			((Control)(object)CmbContractoryHistory).Text = string.Empty;
			((Control)(object)CmbWagesAccountHistory).Text = string.Empty;
			FromDateHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			Resethistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
	}

	private void btnshowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			BindGridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGridHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (FromDateHistory.Checked)
			{
				obj.EffectedDate = Conversion.ToDateTime((object)FromDateHistory.Value);
			}
			if (txtToDateHistory.Checked)
			{
				obj.EffectedDateTo = Conversion.ToDateTime((object)txtToDateHistory.Value);
			}
			obj.ContractorId = Conversion.ToInt(CmbContractoryHistory.Value);
			obj.ContractorWagesActivityId = Conversion.ToInt(CmbWagesAccountHistory.Value);
			obj.ActionId = 1;
			DataTable dtgetAll = ContractorWagesRateSchedule.FormHistory(obj);
			if (dtgetAll.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("EffectiveDate", typeof(DateTime));
				dt.Columns.Add("ContractorName", typeof(string));
				dt.Columns.Add("ContractorWagesActivityId", typeof(int));
				dt.Columns.Add("WagesActivity", typeof(string));
				dt.Columns.Add("ContractorWagesGroupId", typeof(int));
				dt.Columns.Add("ItemWagesGroupName", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("RangeUomId", typeof(int));
				dt.Columns.Add("RangeUom", typeof(string));
				dt.Columns.Add("RangeFrom", typeof(string));
				dt.Columns.Add("RangeTo", typeof(string));
				dt.Columns.Add("Qty", typeof(decimal));
				dt.Columns.Add("CompanyRate", typeof(decimal));
				dt.Columns.Add("WagesRate", typeof(decimal));
				dt.Clear();
				for (int i = 0; i < dtgetAll.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtgetAll.Rows[i]["Id"]), Conversion.ToDateTime(dtgetAll.Rows[i]["EffectiveDate"]).ToShortDateString(), Conversion.ToString(dtgetAll.Rows[i]["ContractorName"]), Conversion.ToInt(dtgetAll.Rows[i]["ContractorWagesActivityId"]), Conversion.ToString(dtgetAll.Rows[i]["WagesActivity"]), Conversion.ToInt(dtgetAll.Rows[i]["ContractorWagesGroupId"]), Conversion.ToString(dtgetAll.Rows[i]["ItemWagesGroupName"]), Conversion.ToInt(dtgetAll.Rows[i]["ItemId"]), Conversion.ToString(dtgetAll.Rows[i]["ItemName"]), Conversion.ToInt(dtgetAll.Rows[i]["RangeUomId"]), Conversion.ToString(dtgetAll.Rows[i]["RangeUom"]), Conversion.ToInt(dtgetAll.Rows[i]["MeatureMFrom"]) + "-" + Conversion.ToInt(dtgetAll.Rows[i]["MeatureSFrom"]), Conversion.ToInt(dtgetAll.Rows[i]["MatureMTo"]) + "-" + Conversion.ToInt(dtgetAll.Rows[i]["MatureSTo"]), Conversion.ToDecimal(dtgetAll.Rows[i]["Qty"]), Conversion.ToDecimal(dtgetAll.Rows[i]["CompanyRate"]), Conversion.ToDecimal(dtgetAll.Rows[i]["Rate"]));
				}
				gridhistory.DataSource = dt;
				gridhistory.RetrieveStructure();
				grdHistorySetting();
			}
			else
			{
				gridhistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistorySetting()
	{
		try
		{
			gridhistory.RootTable.Columns["Id"].Visible = false;
			gridhistory.RootTable.Columns["ItemId"].Visible = false;
			gridhistory.RootTable.Columns["RangeUomId"].Visible = false;
			gridhistory.RootTable.Columns["ContractorWagesActivityId"].Visible = false;
			gridhistory.RootTable.Columns["ContractorWagesGroupId"].Visible = false;
			gridhistory.RootTable.Columns["WagesRate"].AggregateFunction = (AggregateFunction)3;
			gridhistory.RootTable.Columns["WagesRate"].TextAlignment = (TextAlignment)3;
			gridhistory.RootTable.Columns["WagesRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			gridhistory.RootTable.Columns["WagesRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			gridhistory.RootTable.Columns["CompanyRate"].AggregateFunction = (AggregateFunction)3;
			gridhistory.RootTable.Columns["CompanyRate"].TextAlignment = (TextAlignment)3;
			gridhistory.RootTable.Columns["CompanyRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			gridhistory.RootTable.Columns["CompanyRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			gridhistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)3;
			gridhistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			gridhistory.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			gridhistory.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(gridhistory);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadbyId(Conversion.ToInt((object)gridhistory.CurrentRow.Cells["Id"].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmContractWagesSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled && tabWagesSchedule.SelectedIndex == 0)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabWagesSchedule.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				else
				{
					btnnew_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabWagesSchedule.SelectedIndex == 1)
				{
					tabWagesSchedule.SelectedIndex = 0;
				}
				else
				{
					tabWagesSchedule.SelectedIndex = 1;
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnrefrsh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdWagesSchedule).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (tabWagesSchedule.SelectedIndex == 0)
				{
					grdWagesSchedule_DoubleClick(null, null);
				}
				else
				{
					gridhistory_DoubleClick(null, null);
				}
			}
			if (e.Control && e.Alt)
			{
				btnshortcutkeys_Click(null, null);
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Effective Date");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Combo in Effective Date");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcutkeys_Click(object sender, EventArgs e)
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdWagesSchedule).Name);
			if (fs != null)
			{
				grdWagesSchedule.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)gridhistory).Name);
			if (fs != null)
			{
				gridhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackUOMFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackUOMTO_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtMeatureSFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtMeatureSTo_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		GridEXLayout grdWagesSchedule_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lab.frmDeductionPolicyForGrn));
		GridEXLayout gridhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
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
		this.tabWagesSchedule = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdWagesSchedule = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtdate = new System.Windows.Forms.DateTimePicker();
		this.txtMeatureMTo = new System.Windows.Forms.TextBox();
		this.txtMeatureMFrom = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.btnAttachments = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnrefrsh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.gridhistory = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label20 = new System.Windows.Forms.Label();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbWagesAccountHistory = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbContractoryHistory = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.CmbPolicyType = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtConditionDescription = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.ChkIsActive = new System.Windows.Forms.CheckBox();
		this.tabWagesSchedule.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWagesSchedule).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.btnAttachments.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridhistory).BeginInit();
		this.panel3.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesAccountHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractoryHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPolicyType).BeginInit();
		base.SuspendLayout();
		this.tabWagesSchedule.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabWagesSchedule.Controls.Add(this.tabPage1);
		this.tabWagesSchedule.Controls.Add(this.tabPage2);
		this.tabWagesSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabWagesSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabWagesSchedule.Location = new System.Drawing.Point(0, 0);
		this.tabWagesSchedule.Name = "tabWagesSchedule";
		this.tabWagesSchedule.SelectedIndex = 0;
		this.tabWagesSchedule.Size = new System.Drawing.Size(964, 661);
		this.tabWagesSchedule.TabIndex = 0;
		this.tabWagesSchedule.SelectedIndexChanged += new System.EventHandler(tabWagesSchedule_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.btnAttachments);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdWagesSchedule);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 169);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 460);
		this.panel4.TabIndex = 71;
		this.grdWagesSchedule.AllowEdit = (InheritableBoolean)2;
		grdWagesSchedule_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdWagesSchedule.DesignTimeLayout = grdWagesSchedule_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWagesSchedule.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Name = "grdWagesSchedule";
		((ScrollableBase)this.grdWagesSchedule).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Size = new System.Drawing.Size(950, 460);
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).TabIndex = 0;
		this.grdWagesSchedule.TotalRow = (InheritableBoolean)1;
		this.grdWagesSchedule.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWagesSchedule).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).DoubleClick += new System.EventHandler(grdWagesSchedule_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 144);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(950, 25);
		this.panel5.TabIndex = 70;
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label5.Location = new System.Drawing.Point(7, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(339, 17);
		this.label5.TabIndex = 0;
		this.label5.Text = "History of Selected Sevice Activity or Contractor";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(904, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdWagesSchedule;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 23);
		this.ctrlGrdBar1.TabIndex = 69;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 60);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 84);
		this.panel2.TabIndex = 0;
		this.groupBox1.Controls.Add(this.ChkIsActive);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPolicyType);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtConditionDescription);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtdate);
		this.groupBox1.Controls.Add(this.txtMeatureMTo);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtMeatureMFrom);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(941, 78);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(4, 27);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 40;
		this.label6.Text = "Policy Type";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(4, 52);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(78, 15);
		this.label3.TabIndex = 23;
		this.label3.Text = "Effected Date";
		this.txtdate.CustomFormat = "dd-MMM-yy";
		this.txtdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdate.Location = new System.Drawing.Point(83, 48);
		this.txtdate.Name = "txtdate";
		this.txtdate.Size = new System.Drawing.Size(129, 23);
		this.txtdate.TabIndex = 0;
		this.txtdate.ValueChanged += new System.EventHandler(txtdate_ValueChanged);
		this.txtMeatureMTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMeatureMTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMeatureMTo.Location = new System.Drawing.Point(318, 48);
		this.txtMeatureMTo.Name = "txtMeatureMTo";
		this.txtMeatureMTo.ShortcutsEnabled = false;
		this.txtMeatureMTo.Size = new System.Drawing.Size(92, 23);
		this.txtMeatureMTo.TabIndex = 10;
		this.txtMeatureMTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMeatureMTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackUOMTO_KeyPress);
		this.txtMeatureMFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMeatureMFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMeatureMFrom.Location = new System.Drawing.Point(318, 23);
		this.txtMeatureMFrom.Name = "txtMeatureMFrom";
		this.txtMeatureMFrom.ShortcutsEnabled = false;
		this.txtMeatureMFrom.Size = new System.Drawing.Size(92, 23);
		this.txtMeatureMFrom.TabIndex = 8;
		this.txtMeatureMFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMeatureMFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackUOMFrom_KeyPress);
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.White;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(217, 27);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(100, 15);
		this.label12.TabIndex = 9;
		this.label12.Text = "Weight From(KG)";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 30);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(950, 30);
		this.panel6.TabIndex = 66;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(7, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(209, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Deduction Policy For Grn";
		this.btnAttachments.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btnAttachments.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnrefrsh, this.toolStripSeparator1, this.btnsave, this.btnUpdate, this.btnDelete, this.toolStripSeparator2, this.btnshortcutkeys, this.toolStripSeparator3 });
		this.btnAttachments.Location = new System.Drawing.Point(3, 3);
		this.btnAttachments.Name = "btnAttachments";
		this.btnAttachments.Size = new System.Drawing.Size(950, 27);
		this.btnAttachments.TabIndex = 65;
		this.btnAttachments.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnrefrsh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnrefrsh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnrefrsh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnrefrsh.Name = "btnrefrsh";
		this.btnrefrsh.Size = new System.Drawing.Size(80, 24);
		this.btnrefrsh.Text = "&Refresh";
		this.btnrefrsh.Click += new System.EventHandler(btnrefrsh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.gridhistory);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Controls.Add(this.panel1);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.gridhistory.AllowEdit = (InheritableBoolean)2;
		gridhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridhistory.DesignTimeLayout = gridhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridhistory.FilterMode = (FilterMode)2;
		this.gridhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.gridhistory).Location = new System.Drawing.Point(3, 147);
		((System.Windows.Forms.Control)(object)this.gridhistory).Name = "gridhistory";
		((ScrollableBase)this.gridhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridhistory).Size = new System.Drawing.Size(950, 482);
		((System.Windows.Forms.Control)(object)this.gridhistory).TabIndex = 1;
		this.gridhistory.TotalRow = (InheritableBoolean)1;
		this.gridhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridhistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.gridhistory).DoubleClick += new System.EventHandler(gridhistory_DoubleClick);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 121);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 26);
		this.panel3.TabIndex = 68;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(906, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ctrlGrdBar2.MyGrid = this.gridhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(39, 25);
		this.ctrlGrdBar2.TabIndex = 1;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label4.Location = new System.Drawing.Point(2, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(132, 18);
		this.label4.TabIndex = 0;
		this.label4.Text = "Filtered Records";
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.btnshowHistory);
		this.panel1.Controls.Add(this.txtToDateHistory);
		this.panel1.Controls.Add(this.label20);
		this.panel1.Controls.Add(this.FromDateHistory);
		this.panel1.Controls.Add(this.label21);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory);
		this.panel1.Controls.Add(this.label22);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractoryHistory);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Controls.Add(this.panel12);
		this.panel1.Controls.Add(this.toolStrip2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(950, 118);
		this.panel1.TabIndex = 0;
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(696, 87);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(68, 26);
		this.btnshowHistory.TabIndex = 4;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshowHistory_Click);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(73, 89);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(183, 23);
		this.txtToDateHistory.TabIndex = 1;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(7, 93);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(48, 15);
		this.label20.TabIndex = 159;
		this.label20.Text = "Date To";
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(73, 60);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(183, 23);
		this.FromDateHistory.TabIndex = 0;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(7, 64);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(63, 15);
		this.label21.TabIndex = 158;
		this.label21.Text = "Date From";
		this.CmbWagesAccountHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbWagesAccountHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWagesAccountHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWagesAccountHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWagesAccountHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).Location = new System.Drawing.Point(355, 87);
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).Name = "CmbWagesAccountHistory";
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).Size = new System.Drawing.Size(336, 26);
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).TabIndex = 3;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(258, 93);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(88, 15);
		this.label22.TabIndex = 157;
		this.label22.Text = "Service Activity";
		this.CmbContractoryHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbContractoryHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractoryHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractoryHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractoryHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractoryHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractoryHistory).Location = new System.Drawing.Point(355, 58);
		((System.Windows.Forms.Control)(object)this.CmbContractoryHistory).Name = "CmbContractoryHistory";
		((System.Windows.Forms.Control)(object)this.CmbContractoryHistory).Size = new System.Drawing.Size(336, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractoryHistory).TabIndex = 2;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(258, 64);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(98, 15);
		this.label23.TabIndex = 156;
		this.label23.Text = "Contractor Name";
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label24);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 27);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(950, 28);
		this.panel12.TabIndex = 155;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.Teal;
		this.label24.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Transparent;
		this.label24.Location = new System.Drawing.Point(2, 3);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(204, 19);
		this.label24.TabIndex = 154;
		this.label24.Text = "Wages Schedule History";
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(950, 27);
		this.toolStrip2.TabIndex = 150;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "&Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.CmbPolicyType.AllowNull = (DefaultableBoolean)1;
		this.CmbPolicyType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPolicyType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPolicyType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPolicyType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbPolicyType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPolicyType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPolicyType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPolicyType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPolicyType).Location = new System.Drawing.Point(83, 21);
		((System.Windows.Forms.Control)(object)this.CmbPolicyType).Name = "CmbPolicyType";
		((System.Windows.Forms.Control)(object)this.CmbPolicyType).Size = new System.Drawing.Size(129, 26);
		((System.Windows.Forms.Control)(object)this.CmbPolicyType).TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(217, 52);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(85, 15);
		this.label2.TabIndex = 9;
		this.label2.Text = "Weight To(KG)";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtConditionDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtConditionDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtConditionDescription.Location = new System.Drawing.Point(480, 23);
		this.txtConditionDescription.Multiline = true;
		this.txtConditionDescription.Name = "txtConditionDescription";
		this.txtConditionDescription.ShortcutsEnabled = false;
		this.txtConditionDescription.Size = new System.Drawing.Size(382, 48);
		this.txtConditionDescription.TabIndex = 8;
		this.txtConditionDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(412, 23);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(68, 45);
		this.label1.TabIndex = 9;
		this.label1.Text = "Condition Description";
		this.ChkIsActive.AutoSize = true;
		this.ChkIsActive.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkIsActive.Location = new System.Drawing.Point(868, 50);
		this.ChkIsActive.Name = "ChkIsActive";
		this.ChkIsActive.Size = new System.Drawing.Size(64, 21);
		this.ChkIsActive.TabIndex = 41;
		this.ChkIsActive.Text = "Active";
		this.ChkIsActive.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.tabWagesSchedule);
		base.KeyPreview = true;
		base.Name = "frmDeductionPolicyForGrn";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmContractWagesSchedule_FormClosing);
		base.Load += new System.EventHandler(frmContractWagesSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmContractWagesSchedule_KeyDown);
		this.tabWagesSchedule.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdWagesSchedule).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.btnAttachments.ResumeLayout(false);
		this.btnAttachments.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridhistory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesAccountHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractoryHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPolicyType).EndInit();
		base.ResumeLayout(false);
	}
}
