using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.FinanceManagment;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.FinanceManagment;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.FinanceManagement;

public class frmLoanFacilityRegistration : Form
{
	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private bool UpdateMode;

	private int updateDetailIndex;

	private DataTable dtdetail = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel7;

	private Panel panel5;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton Print;

	internal UltraCombo cmbloanAggrement;

	private Label label5;

	private Label label2;

	private TextBox txtLoanCode;

	internal UltraCombo cmbBank;

	private Label label1;

	private GroupBox groupBox1;

	private Label label14;

	private TextBox txtAmount;

	private TextBox txtCaseNo;

	private Label label12;

	private Label label4;

	private DateTimePicker txtCaseDate;

	private Label label6;

	private DateTimePicker txtApprovelDate;

	private Label label9;

	private Label label10;

	private DateTimePicker txtExpiryDate;

	private Label label8;

	private TextBox txtIntrest;

	private Label label7;

	private TextBox txtDescription;

	private Label label11;

	private Button btnCancel;

	private Button btnGridUpdate;

	private Button btnAddinGrid;

	internal GridEX grdDetail;

	private Panel panel8;

	private Panel panel11;

	private Panel panel9;

	private ToolStrip toolStrip2;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton toolStripButton3;

	private ToolStripButton BtnPrint;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton4;

	private ToolStripButton toolStripButton5;

	private ToolStripButton helpToolStripButton;

	private TextBox txtvalidDays;

	private Panel panel4;

	internal GridEX DataGridHistoryDetail;

	private ToolStripButton toolStripButton7;

	internal GridEX DataGridHistory;

	private Panel panel10;

	private Button button3;

	private Label label15;

	internal UltraCombo cmbLoanCollectralType;

	private Label label16;

	private Label label3;

	private Label label13;

	private Label label21;

	private Label label20;

	private Label label19;

	private Label label18;

	private Label label17;

	private Label label22;

	private Label label24;

	private Label label23;

	private Label lbltypeBalance;

	public frmLoanFacilityRegistration(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmLoanFacilityRegistration_Load(object sender, EventArgs e)
	{
		try
		{
			cmbBank.Focus();
			Bank();
			dtdetail.Columns.Add("Case#");
			dtdetail.Columns.Add("CaseDate");
			dtdetail.Columns.Add("ApprovelDate");
			dtdetail.Columns.Add("LoanCollectralType");
			dtdetail.Columns.Add("Description");
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("Interest");
			dtdetail.Columns.Add("ValidDays");
			dtdetail.Columns.Add("ExpiryDate");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Bank()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtbank = FmLoanAgreementHeader.GetAllBankFromLoanAgreement(new FmLoanAgreementHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtbank.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtbank, cmbBank, "BankGLAccountId", "BranchName", "Bank Name", false);
				((UltraGridBase)cmbBank).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBank_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			LoanAggrement(Conversion.ToInt(cmbBank.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoanAggrement(int bankId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtbank = FmLoanAgreementHeader.GetAllBankFromLoanAgreement(new FmLoanAgreementHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("FacilityCode");
			if (dtbank.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtbank.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtbank.Rows[i]["BankGLAccountId"]) == bankId)
				{
					dt.Rows.Add(dtbank.Rows[i]["Id"], dtbank.Rows[i]["FacilityCode"]);
				}
			}
			DDL.BindDDL(dt, cmbloanAggrement, "Id", "FacilityCode", "Loan Code", ZeroIndex: false);
			((UltraGridBase)cmbloanAggrement).DisplayLayout.Bands[0].Columns[1].Width = 275;
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
			dtdetail.Columns.Add("Case#");
			dtdetail.Columns.Add("CaseDate");
			dtdetail.Columns.Add("ApprovelDate");
			dtdetail.Columns.Add("LoanCollectralType");
			dtdetail.Columns.Add("Description");
			dtdetail.Columns.Add("Amount");
			dtdetail.Columns.Add("Interest");
			dtdetail.Columns.Add("ValidDays");
			dtdetail.Columns.Add("ExpiryDate");
			grdDetail.RootTable.Columns["Case#"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CaseDate"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ApprovelDate"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["LoanCollectralType"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Interest"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ValidDays"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ExpiryDate"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Amount"].FormatString = "#,##0";
			grdDetail.RootTable.Columns["Case#"].Width = 130;
			grdDetail.RootTable.Columns["LoanCollectralType"].Width = 300;
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
			FormReset();
			FormDeatilReset();
			grdDetail.ClearStructure();
			dtdetail.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddinGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtdetail.Rows.Add(txtCaseNo.Text, txtCaseDate.Value.ToString("dd-MMM-yyyy"), txtApprovelDate.Value.ToString("dd-MMM-yyyy"), ((Control)(object)cmbLoanCollectralType).Text, txtDescription.Text, txtAmount.Text, txtIntrest.Text, txtvalidDays.Text, txtExpiryDate.Value.ToString("dd-MMM-yyyy"));
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				FormDeatilReset();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() <= 0)
			{
				throw new Exception("Grid Record not found");
			}
			if (FormValidation())
			{
				FmLoanRegistrationHeader fmLoanRegistrationHeader = new FmLoanRegistrationHeader();
				if (RecId > 0 && UpdateMode)
				{
					fmLoanRegistrationHeader.Id = RecId;
				}
				fmLoanRegistrationHeader.OrganizationId = UserAccount.OrganizationId;
				fmLoanRegistrationHeader.CompanyId = UserAccount.CompanyId;
				fmLoanRegistrationHeader.BankGLAccountId = Conversion.ToInt(cmbBank.Value);
				fmLoanRegistrationHeader.LoanCodeNo = txtLoanCode.Text;
				fmLoanRegistrationHeader.FmFundTypesId = 0;
				fmLoanRegistrationHeader.FmLoanAgreementHeaderId = Conversion.ToInt(cmbloanAggrement.Value);
				fmLoanRegistrationHeader.EntryDate = DateTime.Now;
				fmLoanRegistrationHeader.EntryUserId = UserAccount.ID;
				fmLoanRegistrationHeader.ModifyDate = DateTime.Now;
				fmLoanRegistrationHeader.ModifyUserId = UserAccount.ID;
				fmLoanRegistrationHeader.IsApproved = false;
				fmLoanRegistrationHeader.ApprovedDate = DateTime.Now;
				fmLoanRegistrationHeader.ApprovedUserId = UserAccount.ID;
				fmLoanRegistrationHeader.FmLoanRegistrationDetails = new List<FmLoanRegistrationDetail>();
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow a in rows)
				{
					FmLoanRegistrationDetail Detail = new FmLoanRegistrationDetail();
					Detail.RegistrationCode = Conversion.ToString((object)a.Cells["Case#"].Text);
					Detail.RegistrationDate = Conversion.ToDateTime(a.Cells["CaseDate"].Value);
					Detail.ApprovalDate = Conversion.ToDateTime(a.Cells["ApprovelDate"].Value);
					Detail.LoanColletralType = Conversion.ToString((object)a.Cells["LoanCollectralType"].Text);
					Detail.caseDescription = Conversion.ToString((object)a.Cells["Description"].Text);
					Detail.cAmount = Conversion.ToDouble((object)a.Cells["Amount"].Value.ToString());
					Detail.cInterestRate = Conversion.ToDouble((object)a.Cells["Interest"].Text);
					Detail.cValidDays = Conversion.ToInt((object)a.Cells["ValidDays"].Text);
					Detail.cExpiryDate = Conversion.ToDateTime(a.Cells["ExpiryDate"].Value);
					fmLoanRegistrationHeader.FmLoanRegistrationDetails.Add(Detail);
				}
				int success = FmLoanRegistrationHeader.Save(fmLoanRegistrationHeader);
				if (!UpdateMode && success > 0)
				{
					MessageBox.Show("Save SuccessFully");
				}
				if (UpdateMode && success > 0)
				{
					MessageBox.Show("Update SuccessFully");
				}
				FormReset();
				FormDeatilReset();
				dtdetail.Clear();
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		btnsave_Click(null, null);
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = FmLoanRegistrationHeader.GetAll(new FmLoanRegistrationHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("BankName");
			dt.Columns.Add("LoanCode");
			dt.Columns.Add("LoanAgreement");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["BankName"], lst.Rows[i]["LoanCodeNo"], lst.Rows[i]["LoanAgreement"]);
			}
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbBank).ActiveRow == null)
		{
			MessageBox.Show("Bank  Field Required");
			cmbBank.Focus();
			return false;
		}
		if (txtLoanCode.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Lone Code  Field Required");
			txtLoanCode.Focus();
			return false;
		}
		if (((UltraGridBase)cmbloanAggrement).ActiveRow == null)
		{
			MessageBox.Show("Lone Aggreement  Field Required");
			cmbloanAggrement.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (grdDetail.GetRows().Count() > 0)
		{
			((Control)(object)cmbBank).Enabled = false;
			((Control)(object)cmbloanAggrement).Enabled = false;
			return false;
		}
		if (((UltraGridBase)cmbloanAggrement).ActiveRow == null)
		{
			MessageBox.Show("Please Select Loan Agreement Field First");
			cmbloanAggrement.Focus();
			return false;
		}
		if (txtCaseNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("CaseNo Field Required");
			txtCaseNo.Focus();
			return false;
		}
		if (((Control)(object)cmbLoanCollectralType).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Loan Collectral Type  Field Required");
			cmbLoanCollectralType.Focus();
			return false;
		}
		if (txtDescription.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Description Field Required");
			txtDescription.Focus();
			return false;
		}
		if (txtAmount.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtAmount.Text) > 0.0))
		{
			MessageBox.Show("Amount Must Be Greater Than 0");
			txtAmount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtAmount.Text) > Conversion.ToDouble((object)lbltypeBalance.Text))
		{
			MessageBox.Show("Amount Is Greater Than Balance Amount ");
			txtAmount.Focus();
			return false;
		}
		if (txtIntrest.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtIntrest.Text) > 0.0))
		{
			MessageBox.Show("Interest Must Be Greater Than 0");
			txtIntrest.Focus();
			return false;
		}
		if (txtvalidDays.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtIntrest.Text) > 0.0))
		{
			MessageBox.Show("Valid Days Must Be Greater Than 0");
			txtvalidDays.Focus();
			return false;
		}
		return true;
	}

	private void FormReset()
	{
		try
		{
			((Control)(object)cmbBank).Enabled = true;
			((Control)(object)cmbloanAggrement).Enabled = true;
			((Control)(object)cmbBank).Text = string.Empty;
			txtLoanCode.Text = string.Empty;
			((Control)(object)cmbloanAggrement).Text = string.Empty;
			txtCaseDate.Value = DateTime.Now;
			((Control)(object)cmbloanAggrement).Enabled = false;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			RecId = 0;
			cmbBank.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormDeatilReset()
	{
		try
		{
			txtCaseNo.Focus();
			((Control)(object)cmbLoanCollectralType).Text = string.Empty;
			txtCaseNo.Text = string.Empty;
			txtAmount.Text = 0.ToString();
			txtCaseDate.Value = DateTime.Now;
			txtApprovelDate.Value = DateTime.Now;
			txtvalidDays.Text = 0.ToString();
			txtExpiryDate.Value = DateTime.Now;
			txtDescription.Text = string.Empty;
			txtIntrest.Text = 0.ToString();
			lbltypeBalance.Text = 0.ToString();
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			btnAddinGrid.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			FmLoanRegistrationHeader fmLoanRegistration = new FmLoanRegistrationHeader();
			RecId = Id;
			fmLoanRegistration = FmLoanRegistrationHeader.GetByID(RecId);
			tabControl1.SelectedIndex = 0;
			cmbBank.Value = fmLoanRegistration.BankGLAccountId;
			txtLoanCode.Text = fmLoanRegistration.LoanCodeNo;
			cmbloanAggrement.Value = fmLoanRegistration.FmLoanAgreementHeaderId;
			dtdetail.Clear();
			grdDetail.ClearStructure();
			for (int i = 0; i < fmLoanRegistration.FmLoanRegistrationDetails.Count; i++)
			{
				dtdetail.Rows.Add(fmLoanRegistration.FmLoanRegistrationDetails[i].RegistrationCode, fmLoanRegistration.FmLoanRegistrationDetails[i].RegistrationDate.ToString("dd-MMM-yyyy"), fmLoanRegistration.FmLoanRegistrationDetails[i].ApprovalDate.ToString("dd-MMM-yyyy"), fmLoanRegistration.FmLoanRegistrationDetails[i].LoanColletralType, fmLoanRegistration.FmLoanRegistrationDetails[i].caseDescription, fmLoanRegistration.FmLoanRegistrationDetails[i].cAmount, fmLoanRegistration.FmLoanRegistrationDetails[i].cInterestRate, fmLoanRegistration.FmLoanRegistrationDetails[i].cValidDays, fmLoanRegistration.FmLoanRegistrationDetails[i].cExpiryDate.ToString("dd-MMM-yyyy"));
			}
			cmbBank_ValueChanged(null, null);
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			UpdateMode = true;
			GridDetailSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmLoanFacilityRegistration_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && !UpdateMode)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
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
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				((Control)(object)cmbLoanCollectralType).Text = item.Cells["LoanCollectralType"].Text.ToString();
				txtCaseNo.Text = item.Cells["Case#"].Text.ToString();
				txtCaseDate.Text = item.Cells["CaseDate"].Text;
				txtApprovelDate.Text = item.Cells["ApprovelDate"].Text;
				txtDescription.Text = item.Cells["Description"].Text;
				txtAmount.Text = item.Cells["Amount"].Value.ToString();
				txtIntrest.Text = item.Cells["Interest"].Text;
				txtvalidDays.Text = item.Cells["ValidDays"].Text;
				txtExpiryDate.Text = item.Cells["ExpiryDate"].Text;
				btnGridUpdate.Visible = true;
				btnCancel.Visible = true;
				btnAddinGrid.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGridUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtdetail.Rows[updateDetailIndex]["LoanCollectralType"] = ((Control)(object)cmbLoanCollectralType).Text;
				dtdetail.Rows[updateDetailIndex]["Case#"] = txtCaseNo.Text;
				dtdetail.Rows[updateDetailIndex]["CaseDate"] = txtCaseDate.Value.ToString("dd-MMM-yyyy");
				dtdetail.Rows[updateDetailIndex]["ApprovelDate"] = txtApprovelDate.Value.ToString("dd-MMM-yyyy");
				dtdetail.Rows[updateDetailIndex]["ExpiryDate"] = txtExpiryDate.Value.ToString("dd-MMM-yyyy");
				dtdetail.Rows[updateDetailIndex]["Description"] = txtDescription.Text;
				dtdetail.Rows[updateDetailIndex]["Amount"] = txtAmount.Text;
				dtdetail.Rows[updateDetailIndex]["Interest"] = txtIntrest.Text;
				dtdetail.Rows[updateDetailIndex]["ValidDays"] = txtvalidDays.Text;
				btnGridUpdate.Visible = false;
				btnCancel.Visible = false;
				btnAddinGrid.Visible = true;
				FormDeatilReset();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		btnGridUpdate.Visible = false;
		btnCancel.Visible = false;
		btnAddinGrid.Visible = true;
		FormDeatilReset();
	}

	private void button3_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void button3_Click_1(object sender, EventArgs e)
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

	private void cmbloanAggrement_Leave(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			DataTable dtLoanAggrement = new DataTable();
			if (Conversion.ToInt(cmbloanAggrement.Value) > 0)
			{
				dtLoanAggrement = FmLoanAgreementHeader.GetByAgreementId(new FmLoanAgreementHeader
				{
					Id = Conversion.ToInt(cmbloanAggrement.Value)
				});
				if (dtLoanAggrement.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtLoanAggrement, cmbLoanCollectralType, "FmLoanAgreementHeaderId", "LoanColletralType", "Loan Colletral Type", false);
					((UltraGridBase)cmbLoanCollectralType).DisplayLayout.Bands[0].Columns[1].Width = 300;
				}
				else
				{
					dtLoanAggrement.Rows.Clear();
				}
			}
			else
			{
				dtLoanAggrement.Rows.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbLoanCollectralType_ValueChanged(object sender, EventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbLoanCollectralType.Value) > 0)
			{
				DataTable dtbalance = FmLoanAgreementHeader.GetLoanAgreementBalanceAmountByCollectrelType(new ReportsParameters
				{
					Id = Conversion.ToInt(cmbloanAggrement.Value),
					ReqType = ((Control)(object)cmbLoanCollectralType).Text,
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (dtbalance.Rows.Count > 0)
				{
					lbltypeBalance.Text = Conversion.ToString(dtbalance.Rows[0]["BalanceAmount"]);
				}
				else
				{
					lbltypeBalance.Text = 0.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				GridBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtvalidDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(txtvalidDays.Text) > 0)
			{
				txtExpiryDate.Value = txtApprovelDate.Value.AddDays(Convert.ToInt32(txtvalidDays.Text));
				return;
			}
			txtvalidDays.Text = 0.ToString();
			txtExpiryDate.Value = txtApprovelDate.Value;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_Click(object sender, EventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.SelectedItems.Count <= 0)
			{
				return;
			}
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			FmLoanRegistrationHeader h = new FmLoanRegistrationHeader();
			h = FmLoanRegistrationHeader.GetByID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells[0].Value));
			if (h != null)
			{
				DataTable dtgrdhistorydetail = new DataTable();
				dtgrdhistorydetail.Columns.Add("Case#");
				dtgrdhistorydetail.Columns.Add("CaseDate");
				dtgrdhistorydetail.Columns.Add("ApprovelDate");
				dtgrdhistorydetail.Columns.Add("LoanCollectralType");
				dtgrdhistorydetail.Columns.Add("Description");
				dtgrdhistorydetail.Columns.Add("Amount");
				dtgrdhistorydetail.Columns.Add("Interest");
				dtgrdhistorydetail.Columns.Add("ValidDays");
				dtgrdhistorydetail.Columns.Add("ExpiryDate");
				dtgrdhistorydetail.Clear();
				for (int i = 0; i < h.FmLoanRegistrationDetails.Count; i++)
				{
					dtgrdhistorydetail.Rows.Add(h.FmLoanRegistrationDetails[i].RegistrationCode, h.FmLoanRegistrationDetails[i].RegistrationDate.ToString("dd-MMM-yyyy"), h.FmLoanRegistrationDetails[i].ApprovalDate.ToString("dd-MMM-yyyy"), h.FmLoanRegistrationDetails[i].LoanColletralType, h.FmLoanRegistrationDetails[i].caseDescription, h.FmLoanRegistrationDetails[i].cAmount, h.FmLoanRegistrationDetails[i].cInterestRate, h.FmLoanRegistrationDetails[i].cValidDays, h.FmLoanRegistrationDetails[i].cExpiryDate.ToString("dd-MMM-yyyy"));
				}
				DataGridHistoryDetail.DataSource = dtgrdhistorydetail;
				DataGridHistoryDetail.RetrieveStructure();
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
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Expected O, but got Unknown
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout DataGridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.FinanceManagement.frmLoanFacilityRegistration));
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtvalidDays = new System.Windows.Forms.TextBox();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtExpiryDate = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.txtIntrest = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtApprovelDate = new System.Windows.Forms.DateTimePicker();
		this.txtCaseDate = new System.Windows.Forms.DateTimePicker();
		this.label14 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.txtCaseNo = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.cmbloanAggrement = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtLoanCode = new System.Windows.Forms.TextBox();
		this.cmbBank = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.DataGridHistoryDetail = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.panel10 = new System.Windows.Forms.Panel();
		this.button3 = new System.Windows.Forms.Button();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbLoanCollectralType = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.lbltypeBalance = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.groupBox1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbloanAggrement).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBank).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanCollectralType).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1299, 749);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1299, 749);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1291, 723);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1285, 717);
		this.panel2.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 289);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1285, 428);
		this.panel7.TabIndex = 4;
		this.grdDetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		this.grdDetail.ColumnAutoResize = true;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1285, 428);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 4;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.lbltypeBalance);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoanCollectralType);
		this.groupBox1.Controls.Add(this.txtvalidDays);
		this.groupBox1.Controls.Add(this.btnCancel);
		this.groupBox1.Controls.Add(this.btnGridUpdate);
		this.groupBox1.Controls.Add(this.btnAddinGrid);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtExpiryDate);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtIntrest);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtDescription);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtApprovelDate);
		this.groupBox1.Controls.Add(this.txtCaseDate);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.txtCaseNo);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Location = new System.Drawing.Point(12, 86);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(979, 131);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.txtvalidDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvalidDays.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtvalidDays.Location = new System.Drawing.Point(205, 93);
		this.txtvalidDays.Name = "txtvalidDays";
		this.txtvalidDays.Size = new System.Drawing.Size(88, 25);
		this.txtvalidDays.TabIndex = 7;
		this.txtvalidDays.Text = "0";
		this.txtvalidDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtvalidDays.TextChanged += new System.EventHandler(txtvalidDays_TextChanged);
		this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.Location = new System.Drawing.Point(570, 93);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(62, 22);
		this.btnCancel.TabIndex = 11;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGridUpdate.Location = new System.Drawing.Point(502, 93);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(62, 26);
		this.btnGridUpdate.TabIndex = 10;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = true;
		this.btnGridUpdate.Visible = false;
		this.btnGridUpdate.Click += new System.EventHandler(btnGridUpdate_Click);
		this.btnAddinGrid.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAddinGrid.Location = new System.Drawing.Point(469, 93);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(27, 22);
		this.btnAddinGrid.TabIndex = 9;
		this.btnAddinGrid.UseVisualStyleBackColor = true;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label9.Location = new System.Drawing.Point(292, 71);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(74, 17);
		this.label9.TabIndex = 97;
		this.label9.Text = "Expiry Date";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label10.Location = new System.Drawing.Point(211, 71);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(68, 17);
		this.label10.TabIndex = 98;
		this.label10.Text = "Valid Days";
		this.txtExpiryDate.Enabled = false;
		this.txtExpiryDate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtExpiryDate.Location = new System.Drawing.Point(295, 94);
		this.txtExpiryDate.Name = "txtExpiryDate";
		this.txtExpiryDate.Size = new System.Drawing.Size(168, 25);
		this.txtExpiryDate.TabIndex = 8;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label8.Location = new System.Drawing.Point(105, 72);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(66, 17);
		this.label8.TabIndex = 94;
		this.label8.Text = "Interest %";
		this.txtIntrest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtIntrest.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtIntrest.Location = new System.Drawing.Point(105, 94);
		this.txtIntrest.Name = "txtIntrest";
		this.txtIntrest.Size = new System.Drawing.Size(94, 25);
		this.txtIntrest.TabIndex = 6;
		this.txtIntrest.Text = "0";
		this.txtIntrest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label7.Location = new System.Drawing.Point(606, 21);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(74, 17);
		this.label7.TabIndex = 92;
		this.label7.Text = "Description";
		this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtDescription.Location = new System.Drawing.Point(602, 44);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(363, 25);
		this.txtDescription.TabIndex = 4;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label11.Location = new System.Drawing.Point(354, 18);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(125, 17);
		this.label11.TabIndex = 90;
		this.label11.Text = "Loan Collectral Type";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label6.Location = new System.Drawing.Point(223, 20);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(92, 17);
		this.label6.TabIndex = 88;
		this.label6.Text = "Approvel Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label4.Location = new System.Drawing.Point(95, 20);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 17);
		this.label4.TabIndex = 88;
		this.label4.Text = "Case Date";
		this.txtApprovelDate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtApprovelDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtApprovelDate.Location = new System.Drawing.Point(226, 46);
		this.txtApprovelDate.Name = "txtApprovelDate";
		this.txtApprovelDate.Size = new System.Drawing.Size(122, 25);
		this.txtApprovelDate.TabIndex = 2;
		this.txtApprovelDate.ValueChanged += new System.EventHandler(txtvalidDays_TextChanged);
		this.txtCaseDate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtCaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtCaseDate.Location = new System.Drawing.Point(98, 46);
		this.txtCaseDate.Name = "txtCaseDate";
		this.txtCaseDate.Size = new System.Drawing.Size(122, 25);
		this.txtCaseDate.TabIndex = 1;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label14.Location = new System.Drawing.Point(9, 72);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(53, 17);
		this.label14.TabIndex = 83;
		this.label14.Text = "Amount";
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(9, 94);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(92, 25);
		this.txtAmount.TabIndex = 5;
		this.txtAmount.Text = "0";
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCaseNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCaseNo.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtCaseNo.Location = new System.Drawing.Point(5, 45);
		this.txtCaseNo.Name = "txtCaseNo";
		this.txtCaseNo.Size = new System.Drawing.Size(90, 25);
		this.txtCaseNo.TabIndex = 0;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label12.Location = new System.Drawing.Point(9, 20);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(48, 17);
		this.label12.TabIndex = 82;
		this.label12.Text = "Case #";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.label16);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.label13);
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbloanAggrement);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.txtLoanCode);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBank);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 29);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1285, 260);
		this.panel5.TabIndex = 2;
		this.cmbloanAggrement.AllowNull = (DefaultableBoolean)1;
		this.cmbloanAggrement.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbloanAggrement.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbloanAggrement.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbloanAggrement).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbloanAggrement).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbloanAggrement).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbloanAggrement.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbloanAggrement).Location = new System.Drawing.Point(757, 48);
		((System.Windows.Forms.Control)(object)this.cmbloanAggrement).Name = "cmbloanAggrement";
		((System.Windows.Forms.Control)(object)this.cmbloanAggrement).Size = new System.Drawing.Size(181, 22);
		((System.Windows.Forms.Control)(object)this.cmbloanAggrement).TabIndex = 5;
		this.cmbloanAggrement.ValueChanged += new System.EventHandler(cmbloanAggrement_Leave);
		((System.Windows.Forms.Control)(object)this.cmbloanAggrement).Leave += new System.EventHandler(cmbloanAggrement_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(641, 51);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(104, 17);
		this.label5.TabIndex = 89;
		this.label5.Text = "Loan Agreement";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(376, 53);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(59, 13);
		this.label2.TabIndex = 87;
		this.label2.Text = "Loan Code";
		this.txtLoanCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLoanCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoanCode.Location = new System.Drawing.Point(445, 48);
		this.txtLoanCode.Name = "txtLoanCode";
		this.txtLoanCode.Size = new System.Drawing.Size(191, 22);
		this.txtLoanCode.TabIndex = 4;
		this.cmbBank.AllowNull = (DefaultableBoolean)1;
		this.cmbBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance17;
		((SpecialBoxBase)((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBank).Location = new System.Drawing.Point(93, 48);
		((System.Windows.Forms.Control)(object)this.cmbBank).Name = "cmbBank";
		((System.Windows.Forms.Control)(object)this.cmbBank).Size = new System.Drawing.Size(277, 22);
		((System.Windows.Forms.Control)(object)this.cmbBank).TabIndex = 3;
		this.cmbBank.ValueChanged += new System.EventHandler(cmbBank_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbBank).Leave += new System.EventHandler(cmbBank_ValueChanged);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(9, 53);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(74, 17);
		this.label1.TabIndex = 86;
		this.label1.Text = "Bank Gl A/c";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1285, 29);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnsave, this.btnUpdate, this.toolStripSeparator, this.Print });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1285, 27);
		this.toolStrip1.TabIndex = 4;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(56, 24);
		this.Print.Text = "&Print";
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1291, 723);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel4);
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1285, 717);
		this.panel8.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 376);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1285, 341);
		this.panel4.TabIndex = 3;
		this.DataGridHistoryDetail.AllowDelete = (InheritableBoolean)1;
		this.DataGridHistoryDetail.ColumnAutoResize = true;
		DataGridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryDetail.DesignTimeLayout = DataGridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistoryDetail.DynamicFiltering = true;
		this.DataGridHistoryDetail.FilterMode = (FilterMode)2;
		this.DataGridHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Name = "DataGridHistoryDetail";
		((ScrollableBase)this.DataGridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Size = new System.Drawing.Size(1285, 341);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).TabIndex = 49;
		this.DataGridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 29);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1285, 347);
		this.panel11.TabIndex = 2;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		this.DataGridHistory.ColumnAutoResize = true;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1285, 347);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 5;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Click += new System.EventHandler(DataGridHistory_Click);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel9.Controls.Add(this.toolStrip2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1285, 29);
		this.panel9.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.toolStripButton7, this.toolStripButton1, this.toolStripButton2, this.toolStripButton3, this.BtnPrint, this.toolStripSeparator1, this.toolStripButton4, this.toolStripButton5, this.helpToolStripButton });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1285, 28);
		this.toolStrip2.TabIndex = 4;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripButton7.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton7.Name = "toolStripButton7";
		this.toolStripButton7.Size = new System.Drawing.Size(70, 25);
		this.toolStripButton7.Text = "History";
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(55, 25);
		this.toolStripButton1.Text = "&New";
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(55, 25);
		this.toolStripButton2.Text = "&Save";
		this.toolStripButton3.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(69, 25);
		this.toolStripButton3.Text = "&Update";
		this.toolStripButton3.Visible = false;
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(56, 25);
		this.BtnPrint.Text = "&Print";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(94, 25);
		this.toolStripButton4.Text = "&Attachment";
		this.toolStripButton5.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton5.Name = "toolStripButton5";
		this.toolStripButton5.Size = new System.Drawing.Size(54, 25);
		this.toolStripButton5.Text = "&SMS";
		this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.helpToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.helpToolStripButton.Name = "helpToolStripButton";
		this.helpToolStripButton.Size = new System.Drawing.Size(24, 25);
		this.helpToolStripButton.Text = "He&lp";
		this.panel10.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.panel10.Controls.Add(this.button3);
		this.panel10.Controls.Add(this.label15);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1285, 42);
		this.panel10.TabIndex = 98;
		this.button3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button3.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.button3.FlatAppearance.BorderSize = 0;
		this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button3.Image = (System.Drawing.Image)resources.GetObject("button3.Image");
		this.button3.Location = new System.Drawing.Point(1229, 7);
		this.button3.Margin = new System.Windows.Forms.Padding(2);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(44, 28);
		this.button3.TabIndex = 7;
		this.button3.UseVisualStyleBackColor = false;
		this.button3.Click += new System.EventHandler(button3_Click_1);
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(12, 8);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(155, 24);
		this.label15.TabIndex = 1;
		this.label15.Text = "Loan Registration";
		this.cmbLoanCollectralType.AllowNull = (DefaultableBoolean)1;
		this.cmbLoanCollectralType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoanCollectralType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoanCollectralType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoanCollectralType).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbLoanCollectralType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoanCollectralType).Location = new System.Drawing.Point(350, 44);
		((System.Windows.Forms.Control)(object)this.cmbLoanCollectralType).Name = "cmbLoanCollectralType";
		((System.Windows.Forms.Control)(object)this.cmbLoanCollectralType).Size = new System.Drawing.Size(246, 27);
		((System.Windows.Forms.Control)(object)this.cmbLoanCollectralType).TabIndex = 99;
		this.cmbLoanCollectralType.ValueChanged += new System.EventHandler(cmbLoanCollectralType_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbLoanCollectralType).TextChanged += new System.EventHandler(cmbLoanCollectralType_ValueChanged);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Red;
		this.label13.Location = new System.Drawing.Point(78, 48);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(14, 17);
		this.label13.TabIndex = 551;
		this.label13.Text = "*";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Red;
		this.label3.Location = new System.Drawing.Point(431, 48);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(14, 17);
		this.label3.TabIndex = 552;
		this.label3.Text = "*";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Red;
		this.label16.Location = new System.Drawing.Point(743, 48);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(14, 17);
		this.label16.TabIndex = 553;
		this.label16.Text = "*";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(476, 18);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(14, 17);
		this.label17.TabIndex = 554;
		this.label17.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(321, 16);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(14, 17);
		this.label18.TabIndex = 555;
		this.label18.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(279, 71);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 556;
		this.label19.Text = "*";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(59, 74);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(14, 17);
		this.label20.TabIndex = 557;
		this.label20.Text = "*";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.Red;
		this.label21.Location = new System.Drawing.Point(59, 20);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(14, 17);
		this.label21.TabIndex = 558;
		this.label21.Text = "*";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Red;
		this.label22.Location = new System.Drawing.Point(170, 73);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(14, 17);
		this.label22.TabIndex = 559;
		this.label22.Text = "*";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Red;
		this.label23.Location = new System.Drawing.Point(163, 20);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(14, 17);
		this.label23.TabIndex = 560;
		this.label23.Text = "*";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(686, 21);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(14, 17);
		this.label24.TabIndex = 561;
		this.label24.Text = "*";
		this.lbltypeBalance.AutoSize = true;
		this.lbltypeBalance.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.lbltypeBalance.Location = new System.Drawing.Point(489, 21);
		this.lbltypeBalance.Name = "lbltypeBalance";
		this.lbltypeBalance.Size = new System.Drawing.Size(15, 17);
		this.lbltypeBalance.TabIndex = 562;
		this.lbltypeBalance.Text = "0";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1299, 749);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmLoanFacilityRegistration";
		this.Text = "frmLoanFacilityRegistration";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmLoanFacilityRegistration_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmLoanFacilityRegistration_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbloanAggrement).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBank).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).EndInit();
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanCollectralType).EndInit();
		base.ResumeLayout(false);
	}
}
