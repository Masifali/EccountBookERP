using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Lookups;
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
namespace Architecture.WinApp.Inventory_Definition
{

public class AcfrmDefineLots : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool AllowCoaAccountForJobLots;

	private int RecId;

	private bool IsPartyProcessingFeatureOn;

	private bool JobLotAllocateToBranche;

	private IContainer components;

	private ToolStripButton BtnSave;

	private ToolStripButton BtnEdit;

	private ToolStripButton BtnNew;

	private ToolStrip toolStrip1;

	private Panel panel1;

	private Label label1;

	private DateTimePicker dtpEndDate;

	private DateTimePicker dtpStartDate;

	private TextBox txtDescription;

	private TextBox txtCode;

	private Label label6;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label2;

	private BindingSource jobLotBindingSource;

	private BindingSource jobLotBindingSource1;

	private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn jobLotCodeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn jobLotDescriptionDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn startDateDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn endDateDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn jobStatusDataGridViewTextBoxColumn;

	private Panel panel3;

	private Panel panel4;

	internal GridEX GridDefinelots;

	internal UltraCombo cmbStatus;

	internal UltraCombo CmbJobType;

	private Label label7;

	private Panel panel5;

	private Label label8;

	private CtrlGrdBar ctrlGrdBar3;

	private CheckBox ChkIsThirdParty;

	private CheckBox ChkIsCompany;

	private ToolStripButton btnJobLotAllocateToBranche;

	internal UltraCombo CmbAccount;

	private Label label9;

	internal UltraCombo CmbRefDocNo;

	private Label label11;

	internal UltraCombo CmbRefDocumentType;

	private Label label10;

	public AcfrmDefineLots(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void AcfrmDefineLots_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(667, 492);
			IsPartyProcessingFeatureOn = CommonServices.GetERPFeatureById(8);
			ChkIsCompany.Visible = IsPartyProcessingFeatureOn;
			ChkIsThirdParty.Visible = IsPartyProcessingFeatureOn;
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "AllowCoaAccountForJobLots").ToList();
			if (list.Count > 0)
			{
				AllowCoaAccountForJobLots = Conversion.ToBool((object)list[0].ConfigKey.ToString());
			}
			JobLotGridFill();
			RefDocumentsForJobLot();
			txtCode.Focus();
			BtnEdit.Visible = false;
			StatusFill();
			LookupsFill();
			if (AllowCoaAccountForJobLots)
			{
				AccountFill();
				((Control)(object)CmbAccount).Visible = true;
				label9.Visible = true;
			}
			else
			{
				((Control)(object)CmbAccount).Visible = false;
				label9.Visible = false;
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public void StatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Status", typeof(string));
			dt.Rows.Add(1, "Complete");
			dt.Rows.Add(2, "InComplete");
			dt.Rows.Add(3, "Close");
			DDL.BindDDL(dt, cmbStatus, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefDocumentsForJobLot()
	{
		try
		{
			DataTable dt = jobLot.getDocumentsForJobLot();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbRefDocumentType, "Id", "DocumentTypeDescription", "Document Type", ZeroIndex: false);
				return;
			}
			((Control)(object)CmbRefDocumentType).Text = string.Empty;
			((UltraGridBase)CmbRefDocumentType).DataSource = null;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LookupsFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dt = InvLookUp.GetLookupsByTypeIdDt(new InvLookUp
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InvLookupTypeId = 4
			});
			DDL.BindDDLNew(dt, CmbJobType, "Id", "LookupName", "LookupName", false);
			((UltraGridBase)CmbJobType).Rows[0].Activate();
			((UltraGridBase)CmbJobType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbAccount.Value);
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbAccount, "Id", "AccountTitle", "AccountTitle", true);
				((UltraGridBase)CmbAccount).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Id > 0)
				{
					CmbAccount.Value = Id;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtCode.Text == "")
		{
			MessageBox.Show("Code Field Required");
			txtCode.Focus();
			return false;
		}
		if (txtDescription.Text == "")
		{
			MessageBox.Show("Description Field Required");
			txtDescription.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobType).ActiveRow == null)
		{
			MessageBox.Show("JobType Field Required");
			CmbJobType.Focus();
			return false;
		}
		if (IsPartyProcessingFeatureOn && !ChkIsCompany.Checked && !ChkIsThirdParty.Checked)
		{
			MessageBox.Show("Please Check of the Check box ('Is Company' Or 'Is third Party') ");
			ChkIsCompany.Focus();
			return false;
		}
		if (((UltraGridBase)cmbStatus).ActiveRow == null)
		{
			MessageBox.Show("Status Required");
			cmbStatus.Focus();
			return false;
		}
		return true;
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
			JobLot job = new JobLot();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				job.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			job.JobLotCode = txtCode.Text.Trim();
			job.JobLotDescription = txtDescription.Text.Trim();
			job.StartDate = Conversion.ToDateTime((object)dtpStartDate.Value);
			job.EndDate = Conversion.ToDateTime((object)dtpEndDate.Value);
			job.JobTypeId = Conversion.ToInt(CmbJobType.Value);
			job.AccountId = Conversion.ToInt(CmbAccount.Value);
			job.IsApproved = false;
			job.JobStatus = Convert.ToString(((Control)(object)cmbStatus).Text);
			job.EntryDate = Conversion.ToDateTime((object)DateTime.Now);
			job.EntryUser = UserAccount.ID;
			job.ModifyDate = Conversion.ToDateTime((object)DateTime.Now);
			job.ModifyUser = 0;
			job.PostDate = Conversion.ToDateTime((object)DateTime.Now);
			job.PostUser = UserAccount.ID;
			job.PostState = false;
			job.OrganizationId = UserAccount.OrganizationId;
			job.CompanyId = UserAccount.CompanyId;
			if (IsPartyProcessingFeatureOn)
			{
				job.IsThirdParty = ChkIsThirdParty.Checked;
				job.IsCompany = ChkIsCompany.Checked;
			}
			else
			{
				job.IsThirdParty = false;
				job.IsCompany = true;
			}
			job.RefDocumentTypeId = Conversion.ToInt(CmbRefDocumentType.Value);
			job.RefDocNoId = Conversion.ToInt(CmbRefDocNo.Value);
			jobLot.Save(job);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully.");
			}
			else
			{
				MessageBox.Show("Record Save Successfully..");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSave_Click(object sender, EventArgs e)
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

	private void BtnEdit_Click(object sender, EventArgs e)
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

	private void JobLotGridFill()
	{
		try
		{
			DataTable dtGrid = CommonServices.JobLotGetAllService();
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dts = new DataTable();
				dts.Columns.Add("Id", typeof(int));
				dts.Columns.Add("JobLotCode", typeof(string));
				dts.Columns.Add("JobLotDescription", typeof(string));
				dts.Columns.Add("AccountId", typeof(int));
				dts.Columns.Add("AccountTitle", typeof(string));
				dts.Columns.Add("StartDate", typeof(DateTime));
				dts.Columns.Add("EndDate", typeof(DateTime));
				dts.Columns.Add("JobStatus", typeof(string));
				dts.Columns.Add("JobTypeId", typeof(int));
				dts.Columns.Add("JobType", typeof(string));
				dts.Columns.Add("RefDocumentType", typeof(string));
				dts.Columns.Add("RefDocNo", typeof(string));
				dts.Columns.Add("IsCompany", typeof(string));
				dts.Columns.Add("IsThirdParty", typeof(string));
				dts.Clear();
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dts.Rows.Add(Conversion.ToInt(dtGrid.Rows[i]["Id"]), Conversion.ToString(dtGrid.Rows[i]["JobLotCode"]), Conversion.ToString(dtGrid.Rows[i]["JobLotDescription"]), Conversion.ToInt(dtGrid.Rows[i]["AccountId"]), Conversion.ToString(dtGrid.Rows[i]["AccountTitle"]), Conversion.ToDateTime(dtGrid.Rows[i]["StartDate"]).ToShortDateString(), Conversion.ToDateTime(dtGrid.Rows[i]["EndDate"]).ToShortDateString(), Conversion.ToString(dtGrid.Rows[i]["JobStatus"]), Conversion.ToInt(dtGrid.Rows[i]["JobTypeId"]), Conversion.ToString(dtGrid.Rows[i]["JobType"]), Conversion.ToString(dtGrid.Rows[i]["RefDocumentType"]), Conversion.ToString(dtGrid.Rows[i]["RefDocNo"]), Conversion.ToString(dtGrid.Rows[i]["IsCompany"]), Conversion.ToString(dtGrid.Rows[i]["IsThirdParty"]));
				}
				GridDefinelots.DataSource = dts;
				GridDefinelots.RetrieveStructure();
				GridSetting();
			}
			else
			{
				GridDefinelots.DataSource = null;
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
			txtCode.Clear();
			txtDescription.Clear();
			txtCode.Focus();
			BtnSave.Visible = true;
			BtnEdit.Visible = false;
			((Control)(object)cmbStatus).Text = string.Empty;
			((Control)(object)CmbJobType).Text = string.Empty;
			((Control)(object)CmbAccount).Text = string.Empty;
			((Control)(object)CmbRefDocumentType).Text = string.Empty;
			((Control)(object)CmbRefDocNo).Text = string.Empty;
			((UltraGridBase)CmbRefDocNo).DataSource = null;
			JobLotGridFill();
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
			GridDefinelots.RootTable.Columns["IsThirdParty"].Visible = IsPartyProcessingFeatureOn;
			GridDefinelots.RootTable.Columns["IsCompany"].Visible = IsPartyProcessingFeatureOn;
			GridDefinelots.RootTable.Columns["Id"].Visible = false;
			GridDefinelots.RootTable.Columns["JobTypeId"].Visible = false;
			GridDefinelots.RootTable.Columns["AccountId"].Visible = false;
			GridDefinelots.RootTable.Columns["AccountTitle"].Visible = false;
			if (AllowCoaAccountForJobLots)
			{
				GridDefinelots.RootTable.Columns["AccountTitle"].Visible = true;
			}
			CommonServices.GridAutoAdjustmentNew(GridDefinelots);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNew_Click(object sender, EventArgs e)
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

	private void BtnDelete_Click(object sender, EventArgs e)
	{
		Conversion.ToInt((object)GridDefinelots.CurrentRow.Cells["Id"].Value.ToString());
		MessageBox.Show("Recored Deleted Successfully");
		JobLotGridFill();
		Reset();
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDefinelots).Name);
			if (fs != null)
			{
				GridDefinelots.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void AcfrmDefineLots_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.S)
			{
				BtnSave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U)
			{
				BtnEdit_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				BtnNew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtCode.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)GridDefinelots).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtCode.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				BtnDelete_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDefinelots_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			JobLot joblot = new JobLot();
			RecId = Conversion.ToInt((object)GridDefinelots.CurrentRow.Cells["Id"].Value.ToString());
			joblot = jobLot.GetByID(RecId);
			txtCode.Text = joblot.JobLotCode;
			txtDescription.Text = joblot.JobLotDescription;
			dtpStartDate.Value = joblot.StartDate;
			dtpEndDate.Value = joblot.EndDate;
			((Control)(object)cmbStatus).Text = Conversion.ToString((object)joblot.JobStatus);
			CmbJobType.Value = Conversion.ToInt((object)joblot.JobTypeId);
			if (Conversion.ToInt((object)joblot.AccountId) > 0)
			{
				CmbAccount.Value = Conversion.ToInt((object)joblot.AccountId);
			}
			if (IsPartyProcessingFeatureOn)
			{
				ChkIsThirdParty.Checked = joblot.IsThirdParty;
				ChkIsCompany.Checked = joblot.IsCompany;
			}
			if (joblot.RefDocumentTypeId > 0)
			{
				CmbRefDocumentType.Value = joblot.RefDocumentTypeId;
				CmbRefDocumentType_Leave(null, null);
			}
			if (joblot.RefDocNoId > 0)
			{
				CmbRefDocNo.Value = joblot.RefDocNoId;
			}
			JobLotGridFill();
			BtnEdit.Visible = true;
			BtnSave.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AcfrmDefineLots_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnJobLotAllocateToBranche_Click(object sender, EventArgs e)
	{
		try
		{
			new frmJobLotsAllocationToBranch(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRefDocumentType_Leave(object sender, EventArgs e)
	{
		try
		{
			BindRefDocNoAgainstRefDocumentType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindRefDocNoAgainstRefDocumentType()
	{
		try
		{
			int RefDocId = Conversion.ToInt(CmbRefDocNo.Value);
			int RefDocumentTypeId = Conversion.ToInt(CmbRefDocumentType.Value);
			if (RefDocumentTypeId > 0)
			{
				DataTable dt = jobLot.getReferenceNoByDocumentTypeIdFromJobLot(RefDocumentTypeId, RecId);
				DDL.BindDDL(dt, CmbRefDocNo, "Id", "ReferenceNo", "Ref Doc No", ZeroIndex: false);
				CommonServices.SetComboValue(CmbRefDocNo, "Id", RefDocId, dt);
			}
			else
			{
				((Control)(object)CmbRefDocNo).Text = string.Empty;
				((UltraGridBase)CmbRefDocNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
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
		GridEXLayout GridDefinelots_DesignTimeLayout = new GridEXLayout();
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
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnEdit = new System.Windows.Forms.ToolStripButton();
		this.BtnSave = new System.Windows.Forms.ToolStripButton();
		this.btnJobLotAllocateToBranche = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbJobType = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbStatus = new UltraCombo();
		this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
		this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.jobLotBindingSource = new System.Windows.Forms.BindingSource(this.components);
		this.jobLotBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
		this.panel3 = new System.Windows.Forms.Panel();
		this.CmbAccount = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.ChkIsThirdParty = new System.Windows.Forms.CheckBox();
		this.ChkIsCompany = new System.Windows.Forms.CheckBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.GridDefinelots = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbRefDocumentType = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbRefDocNo = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.jobLotBindingSource).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.jobLotBindingSource1).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccount).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDefinelots).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocumentType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocNo).BeginInit();
		base.SuspendLayout();
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.BtnNew, this.BtnEdit, this.BtnSave, this.btnJobLotAllocateToBranche });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(655, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(59, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.BtnEdit.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnEdit.Name = "BtnEdit";
		this.BtnEdit.Size = new System.Drawing.Size(78, 24);
		this.BtnEdit.Text = "&Update";
		this.BtnEdit.Click += new System.EventHandler(BtnEdit_Click);
		this.BtnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSave.Name = "BtnSave";
		this.BtnSave.Size = new System.Drawing.Size(64, 24);
		this.BtnSave.Text = "&Save";
		this.BtnSave.Click += new System.EventHandler(BtnSave_Click);
		this.btnJobLotAllocateToBranche.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnJobLotAllocateToBranche.Name = "btnJobLotAllocateToBranche";
		this.btnJobLotAllocateToBranche.Size = new System.Drawing.Size(183, 24);
		this.btnJobLotAllocateToBranche.Text = "JobLot Allocate To Branch";
		this.btnJobLotAllocateToBranche.Click += new System.EventHandler(btnJobLotAllocateToBranche_Click);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(655, 28);
		this.panel1.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(138, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Define Job/Lots";
		this.CmbJobType.AllowNull = (DefaultableBoolean)1;
		this.CmbJobType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbJobType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbJobType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbJobType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobType).Location = new System.Drawing.Point(393, 54);
		((System.Windows.Forms.Control)(object)this.CmbJobType).Name = "CmbJobType";
		((System.Windows.Forms.Control)(object)this.CmbJobType).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobType).TabIndex = 5;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(309, 60);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(51, 15);
		this.label7.TabIndex = 7;
		this.label7.Text = "JobType";
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(86, 54);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 4;
		this.dtpEndDate.CustomFormat = "dd-MM-yy";
		this.dtpEndDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dtpEndDate.Location = new System.Drawing.Point(393, 30);
		this.dtpEndDate.Name = "dtpEndDate";
		this.dtpEndDate.Size = new System.Drawing.Size(220, 23);
		this.dtpEndDate.TabIndex = 3;
		this.dtpStartDate.CustomFormat = "dd-MM-yy";
		this.dtpStartDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dtpStartDate.Location = new System.Drawing.Point(86, 30);
		this.dtpStartDate.Name = "dtpStartDate";
		this.dtpStartDate.Size = new System.Drawing.Size(220, 23);
		this.dtpStartDate.TabIndex = 2;
		this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(393, 5);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(220, 23);
		this.txtDescription.TabIndex = 1;
		this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCode.Location = new System.Drawing.Point(86, 5);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(220, 23);
		this.txtCode.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 60);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(40, 15);
		this.label6.TabIndex = 4;
		this.label6.Text = "Status";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(309, 34);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(55, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "End Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 34);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(60, 15);
		this.label4.TabIndex = 2;
		this.label4.Text = "Start Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(309, 9);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(68, 15);
		this.label3.TabIndex = 1;
		this.label3.Text = "Description";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 9);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(34, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Code";
		this.jobLotBindingSource.DataMember = "JobLot";
		this.jobLotBindingSource1.DataMember = "JobLot";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefDocNo);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefDocumentType);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccount);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add(this.ChkIsThirdParty);
		this.panel3.Controls.Add(this.ChkIsCompany);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobType);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.txtCode);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.dtpEndDate);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.dtpStartDate);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.txtDescription);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 55);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(655, 137);
		this.panel3.TabIndex = 0;
		this.CmbAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccount).Location = new System.Drawing.Point(86, 108);
		((System.Windows.Forms.Control)(object)this.CmbAccount).Name = "CmbAccount";
		((System.Windows.Forms.Control)(object)this.CmbAccount).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccount).TabIndex = 8;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 114);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(78, 15);
		this.label9.TabIndex = 8;
		this.label9.Text = "Account Title";
		this.ChkIsThirdParty.AutoSize = true;
		this.ChkIsThirdParty.Checked = true;
		this.ChkIsThirdParty.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkIsThirdParty.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkIsThirdParty.Location = new System.Drawing.Point(488, 112);
		this.ChkIsThirdParty.Name = "ChkIsThirdParty";
		this.ChkIsThirdParty.Size = new System.Drawing.Size(96, 19);
		this.ChkIsThirdParty.TabIndex = 10;
		this.ChkIsThirdParty.Text = "Is Third Party";
		this.ChkIsThirdParty.UseVisualStyleBackColor = true;
		this.ChkIsThirdParty.Visible = false;
		this.ChkIsCompany.AutoSize = true;
		this.ChkIsCompany.Checked = true;
		this.ChkIsCompany.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkIsCompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkIsCompany.Location = new System.Drawing.Point(393, 112);
		this.ChkIsCompany.Name = "ChkIsCompany";
		this.ChkIsCompany.Size = new System.Drawing.Size(89, 19);
		this.ChkIsCompany.TabIndex = 9;
		this.ChkIsCompany.Text = "Is Company";
		this.ChkIsCompany.UseVisualStyleBackColor = true;
		this.ChkIsCompany.Visible = false;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.GridDefinelots);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 192);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(655, 368);
		this.panel4.TabIndex = 8;
		this.GridDefinelots.AllowEdit = (InheritableBoolean)2;
		GridDefinelots_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDefinelots.DesignTimeLayout = GridDefinelots_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDefinelots).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDefinelots.FilterMode = (FilterMode)2;
		this.GridDefinelots.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDefinelots).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDefinelots.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridDefinelots).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.GridDefinelots).Name = "GridDefinelots";
		((ScrollableBase)this.GridDefinelots).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDefinelots).Size = new System.Drawing.Size(655, 343);
		((System.Windows.Forms.Control)(object)this.GridDefinelots).TabIndex = 0;
		this.GridDefinelots.TotalRow = (InheritableBoolean)1;
		this.GridDefinelots.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDefinelots).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.GridDefinelots).DoubleClick += new System.EventHandler(GridDefinelots_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar3);
		this.panel5.Controls.Add(this.label8);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(655, 25);
		this.panel5.TabIndex = 0;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(4, 2);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 19);
		this.label8.TabIndex = 0;
		this.label8.Text = "History";
		this.CmbRefDocumentType.AllowNull = (DefaultableBoolean)1;
		this.CmbRefDocumentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefDocumentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefDocumentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRefDocumentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Location = new System.Drawing.Point(86, 81);
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Name = "CmbRefDocumentType";
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).TabIndex = 6;
		this.CmbRefDocumentType.ValueChanged += new System.EventHandler(CmbRefDocumentType_Leave);
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Leave += new System.EventHandler(CmbRefDocumentType_Leave);
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(2, 79);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(85, 30);
		this.label10.TabIndex = 10;
		this.label10.Text = "Ref Document Type";
		this.CmbRefDocNo.AllowNull = (DefaultableBoolean)1;
		this.CmbRefDocNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefDocNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefDocNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRefDocNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).Location = new System.Drawing.Point(393, 81);
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).Name = "CmbRefDocNo";
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).TabIndex = 7;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(312, 87);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(68, 15);
		this.label11.TabIndex = 12;
		this.label11.Text = "Ref Doc No";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(615, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.GridDefinelots;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 21;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.WhiteSmoke;
		base.ClientSize = new System.Drawing.Size(655, 560);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.toolStrip1);
		base.KeyPreview = true;
		base.Name = "AcfrmDefineLots";
		this.Text = "Define Job/Lots";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AcfrmDefineLots_FormClosing);
		base.Load += new System.EventHandler(AcfrmDefineLots_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AcfrmDefineLots_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.jobLotBindingSource).EndInit();
		((System.ComponentModel.ISupportInitialize)this.jobLotBindingSource1).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccount).EndInit();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDefinelots).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocumentType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocNo).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
