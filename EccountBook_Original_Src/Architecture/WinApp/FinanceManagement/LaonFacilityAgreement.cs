using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

public class LaonFacilityAgreement : Form
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

	private Panel panel2;

	private Panel panel7;

	private Panel panel6;

	private Panel panel5;

	private Panel panel3;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton Print;

	internal UltraCombo cmbBank;

	private Label label1;

	private Label label2;

	private TextBox txtFacilityCode;

	private Label label10;

	private Label label9;

	private TextBox txtIntrestRate;

	private Label label8;

	private Label label7;

	private TextBox txtValidityDays;

	private Label label6;

	private TextBox txtApprovelAmount;

	internal UltraCombo cmbfundNonFundBAse;

	private Label label5;

	private Label label4;

	private GroupBox groupBox1;

	private Label label11;

	private TextBox txtCollectralScurityDesc;

	private Label label12;

	private Label label17;

	private TextBox txtRate;

	private Label label16;

	private TextBox txtStockQty;

	private Label label14;

	private TextBox txtDetailAmount;

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

	private DateTimePicker txtvalidUpTo;

	private ToolStripButton toolStripButton6;

	private ToolStripButton toolStripButton7;

	private Panel panel4;

	internal GridEX DataGridHistoryDetail;

	internal GridEX DataGridHistory;

	private Panel panel10;

	private Button button3;

	private Label label15;

	private Label label20;

	private Label label19;

	private Label label18;

	private Label label13;

	private Label label3;

	private Label label41;

	private Label label25;

	private Label label24;

	private Label label23;

	private Label label22;

	private Label label21;

	internal UltraCombo cmbType;

	internal UltraCombo cmbFundType;

	internal UltraCombo cmbLoanCollectralType;

	public LaonFacilityAgreement(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void FundTypeFill()
	{
		try
		{
			DataTable dttypes = new DataTable();
			dttypes.Columns.Add("Id");
			dttypes.Columns.Add("Type");
			dttypes.Rows.Add(1, "Fund Base");
			dttypes.Rows.Add(2, "NonFund Base");
			DDL.BindDDL(dttypes, cmbFundType, "Type", "Type", "Fund type", ZeroIndex: false);
			((UltraGridBase)cmbFundType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RateTypeFill()
	{
		try
		{
			DataTable dttypes = new DataTable();
			dttypes.Columns.Add("Id");
			dttypes.Columns.Add("Type");
			dttypes.Rows.Add(1, "Fixed Rate");
			dttypes.Rows.Add(2, "Floating Rate");
			DDL.BindDDL(dttypes, cmbType, "Type", "Type", "Rate type", ZeroIndex: false);
			((UltraGridBase)cmbType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoanCollectrelTypeFill()
	{
		try
		{
			DataTable dttypes = new DataTable();
			dttypes.Columns.Add("Id");
			dttypes.Columns.Add("Type");
			dttypes.Rows.Add(1, "Hypothication");
			dttypes.Rows.Add(2, "Pledge");
			dttypes.Rows.Add(3, "Mortgage");
			DDL.BindDDL(dttypes, cmbLoanCollectralType, "Type", "Type", "Loan Type", ZeroIndex: false);
			((UltraGridBase)cmbLoanCollectralType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Bank()
	{
		try
		{
			DataTable dtbranch = CommonServices.CoaAllocationGetAllServiceBind();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountName");
			if (dtbranch.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtbranch.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtbranch.Rows[i]["AccountTypeId"]) == 15)
				{
					dt.Rows.Add(dtbranch.Rows[i]["ChartofAccountId"], dtbranch.Rows[i]["AccountTitle"]);
				}
			}
			DDL.BindDDL(dt, cmbBank, "Id", "AccountName", "Bank Name", ZeroIndex: false);
			((UltraGridBase)cmbBank).DisplayLayout.Bands[0].Columns[1].Width = 275;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LaonFacilityAgreement_Load(object sender, EventArgs e)
	{
		try
		{
			Bank();
			FundTypeFill();
			RateTypeFill();
			LoanCollectrelTypeFill();
			dtdetail.Columns.Add("LoanCollectralType");
			dtdetail.Columns.Add("CollectralSecurityDescription");
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("StockQty", typeof(double));
			dtdetail.Columns.Add("Rate", typeof(double));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		try
		{
			if (grdDetail.RowCount > 0)
			{
				if (FormValidation())
				{
					FmLoanAgreementHeader fmLoanAgreement = new FmLoanAgreementHeader();
					if (RecId > 0 && UpdateMode)
					{
						fmLoanAgreement.Id = RecId;
					}
					double TotalHeaderDetail = 0.0;
					double TotalHeaderAmount = Conversion.ToDouble((object)txtApprovelAmount.Text.Trim());
					GridEXRow[] rows = grdDetail.GetRows();
					foreach (GridEXRow item in rows)
					{
						TotalHeaderDetail += Conversion.ToDouble(item.Cells["Amount"].Value);
					}
					if (TotalHeaderDetail != TotalHeaderAmount)
					{
						throw new Exception("Approval Amount And Detail Total Amount Is Not Equal Please Check This");
					}
					fmLoanAgreement.BankGLAccountId = Conversion.ToInt(cmbBank.Value);
					fmLoanAgreement.BankId = Conversion.ToInt(cmbBank.Value);
					fmLoanAgreement.FacilityCode = Conversion.ToString((object)txtFacilityCode.Text);
					fmLoanAgreement.FmFundTypesId = Conversion.ToInt(cmbfundNonFundBAse.Value);
					fmLoanAgreement.ApprovedAmount = Conversion.ToDouble((object)txtApprovelAmount.Text);
					fmLoanAgreement.ValidDays = Conversion.ToInt((object)txtValidityDays.Text);
					fmLoanAgreement.InterestRate = Conversion.ToDouble((object)txtIntrestRate.Text);
					fmLoanAgreement.ExpiryDate = txtvalidUpTo.Value;
					fmLoanAgreement.InterestType = ((Control)(object)cmbType).Text;
					fmLoanAgreement.OrganizationId = UserAccount.OrganizationId;
					fmLoanAgreement.CompanyId = UserAccount.CompanyId;
					fmLoanAgreement.EntryDate = DateTime.Now;
					fmLoanAgreement.EntryUserId = UserAccount.ID;
					fmLoanAgreement.ModifyDate = DateTime.Now;
					fmLoanAgreement.ModifyUserId = UserAccount.ID;
					fmLoanAgreement.IsApproved = false;
					fmLoanAgreement.ApprovedDate = DateTime.Now;
					fmLoanAgreement.ApprovedUserId = 0;
					fmLoanAgreement.FmLoanAgreementDetails = new List<FmLoanAgreementDetail>();
					rows = grdDetail.GetRows();
					foreach (GridEXRow a in rows)
					{
						FmLoanAgreementDetail Detail = new FmLoanAgreementDetail();
						Detail.LoanColletralType = Conversion.ToString((object)a.Cells["LoanCollectralType"].Text);
						Detail.ColletralSecurityDesc = Conversion.ToString((object)a.Cells["CollectralSecurityDescription"].Text);
						Detail.fAmount = Conversion.ToDouble((object)a.Cells["Amount"].Text);
						Detail.fStockQty = Conversion.ToSingle((object)a.Cells["StockQty"].Text);
						Detail.QtyRate = Conversion.ToSingle((object)a.Cells["Rate"].Text);
						fmLoanAgreement.FmLoanAgreementDetails.Add(Detail);
					}
					int success = FmLoanAgreementHeader.Save(fmLoanAgreement);
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
			else
			{
				MessageBox.Show("Please Add Detail First");
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
			btnsave_Click(null, null);
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
				dtdetail.Rows.Add(((Control)(object)cmbLoanCollectralType).Text, txtCollectralScurityDesc.Text, txtDetailAmount.Text, txtStockQty.Text, txtRate.Text);
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

	private void bindhistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = FmLoanAgreementHeader.GetAll(new FmLoanAgreementHeader
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("BankName");
			dt.Columns.Add("FacilityCode");
			dt.Columns.Add("Fund/NonFundBase");
			dt.Columns.Add("ApprovelAmount");
			dt.Columns.Add("ValidityDays");
			dt.Columns.Add("ValidUpTo");
			dt.Columns.Add("IntrestRate");
			dt.Columns.Add("Type");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["BankName"], lst.Rows[i]["FacilityCode"], lst.Rows[i]["FmFundTypesId"], lst.Rows[i]["ApprovedAmount"], lst.Rows[i]["ValidDays"], Conversion.ToDateTime(lst.Rows[i]["ExpiryDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["InterestRate"], lst.Rows[i]["InterestType"]);
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

	private void FormReset()
	{
		try
		{
			((Control)(object)cmbBank).Text = string.Empty;
			txtApprovelAmount.Text = string.Empty;
			txtFacilityCode.Text = string.Empty;
			((Control)(object)cmbFundType).Text = string.Empty;
			((Control)(object)cmbfundNonFundBAse).Text = string.Empty;
			txtValidityDays.Text = string.Empty;
			txtvalidUpTo.Value = DateTime.Now;
			txtIntrestRate.Text = string.Empty;
			((Control)(object)cmbType).Text = string.Empty;
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
			cmbLoanCollectralType.Focus();
			((Control)(object)cmbLoanCollectralType).Text = string.Empty;
			txtCollectralScurityDesc.Text = string.Empty;
			txtDetailAmount.Text = string.Empty;
			txtStockQty.Text = string.Empty;
			txtRate.Text = string.Empty;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			btnAddinGrid.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbFundType).ActiveRow == null)
		{
			MessageBox.Show("Fund Type  Field Required");
			cmbFundType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbfundNonFundBAse).ActiveRow == null)
		{
			MessageBox.Show("Fund/NonFundBase  Field Required");
			cmbfundNonFundBAse.Focus();
			return false;
		}
		if (txtApprovelAmount.Text.Trim() == string.Empty || !(Convert.ToDouble(txtApprovelAmount.Text) > 0.0))
		{
			MessageBox.Show("Approvel Amount  Field Required");
			txtApprovelAmount.Focus();
			return false;
		}
		if (txtValidityDays.Text.Trim() == string.Empty || !(Convert.ToDouble(txtValidityDays.Text) > 0.0))
		{
			MessageBox.Show("Validity Days  Field Required");
			txtValidityDays.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((Control)(object)cmbLoanCollectralType).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Loan Collectral Type  Field Required");
			cmbLoanCollectralType.Focus();
			return false;
		}
		if (txtCollectralScurityDesc.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Collectral Security Description Field Required");
			txtCollectralScurityDesc.Focus();
			return false;
		}
		if (txtDetailAmount.Text.Trim() == string.Empty || !(Convert.ToDouble(txtDetailAmount.Text) > 0.0))
		{
			MessageBox.Show("Amount  Field Required");
			txtDetailAmount.Focus();
			return false;
		}
		if (txtStockQty.Text.Trim() == string.Empty || !(Convert.ToDouble(txtStockQty.Text) > 0.0))
		{
			MessageBox.Show("Stock Qty  Field Required");
			txtStockQty.Focus();
			return false;
		}
		if (txtRate.Text.Trim() == string.Empty || !(Convert.ToDouble(txtRate.Text) > 0.0))
		{
			MessageBox.Show("Rate  Field Required");
			txtRate.Focus();
			return false;
		}
		return true;
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
			FmLoanAgreementHeader fmLoanAgreement = new FmLoanAgreementHeader();
			RecId = Id;
			fmLoanAgreement = FmLoanAgreementHeader.GetByID(RecId);
			tabControl1.SelectedIndex = 0;
			cmbBank.Value = Conversion.ToString((object)fmLoanAgreement.BankId);
			txtFacilityCode.Text = fmLoanAgreement.FacilityCode;
			cmbfundNonFundBAse.Value = Conversion.ToString((object)fmLoanAgreement.FmFundTypesId);
			txtApprovelAmount.Text = fmLoanAgreement.ApprovedAmount.ToString();
			txtValidityDays.Text = fmLoanAgreement.ValidDays.ToString();
			txtIntrestRate.Text = fmLoanAgreement.InterestRate.ToString();
			txtvalidUpTo.Value = Conversion.ToDateTime((object)fmLoanAgreement.ExpiryDate);
			((Control)(object)cmbType).Text = fmLoanAgreement.InterestType;
			dtdetail.Clear();
			grdDetail.ClearStructure();
			for (int i = 0; i < fmLoanAgreement.FmLoanAgreementDetails.Count; i++)
			{
				dtdetail.Rows.Add(fmLoanAgreement.FmLoanAgreementDetails[i].LoanColletralType, fmLoanAgreement.FmLoanAgreementDetails[i].ColletralSecurityDesc, fmLoanAgreement.FmLoanAgreementDetails[i].fAmount, fmLoanAgreement.FmLoanAgreementDetails[i].fStockQty, fmLoanAgreement.FmLoanAgreementDetails[i].QtyRate);
			}
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
			UpdateMode = true;
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
				txtCollectralScurityDesc.Text = item.Cells["CollectralSecurityDescription"].Text.ToString();
				txtDetailAmount.Text = item.Cells["Amount"].Value.ToString();
				txtStockQty.Text = item.Cells["StockQty"].Value.ToString();
				txtRate.Text = item.Cells["Rate"].Value.ToString();
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
				dtdetail.Rows[updateDetailIndex]["CollectralSecurityDescription"] = txtCollectralScurityDesc.Text;
				dtdetail.Rows[updateDetailIndex]["Amount"] = txtDetailAmount.Text;
				dtdetail.Rows[updateDetailIndex]["StockQty"] = txtStockQty.Text;
				dtdetail.Rows[updateDetailIndex]["Rate"] = txtRate.Text;
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
		try
		{
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			btnAddinGrid.Visible = true;
			FormDeatilReset();
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
			grdDetail.RootTable.Columns["LoanCollectralType"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CollectralSecurityDescription"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["StockQty"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["StockQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Amount"].FormatString = "#,##0";
			grdDetail.RootTable.Columns["StockQty"].FormatString = "#,##0";
			grdDetail.RootTable.Columns["Rate"].FormatString = "#,##0.#";
			grdDetail.RootTable.Columns["LoanCollectralType"].Width = 130;
			grdDetail.RootTable.Columns["CollectralSecurityDescription"].Width = 300;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LaonFacilityAgreement_KeyDown(object sender, KeyEventArgs e)
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

	private void button3_Click(object sender, EventArgs e)
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

	private void cmbFundType_TextChanged(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable lst = FmFinanceType.GetByFundType(new FmFinanceType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FundNonFund = ((Control)(object)cmbFundType).Text
			});
			DataTable dtfund = new DataTable();
			dtfund.Columns.Add("Id");
			dtfund.Columns.Add("FundType");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dtfund.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["FundTypeDescription"]);
			}
			if (dtfund.Rows.Count > 0)
			{
				DDL.BindDDL(dtfund, cmbfundNonFundBAse, "Id", "FundType", "Fund Type", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculatAmount()
	{
		try
		{
			double ItemRate = 0.0;
			double ItemAmount = 0.0;
			double ItemQty = 0.0;
			ItemAmount = Conversion.ToDouble((object)txtDetailAmount.Text);
			ItemQty = Conversion.ToDouble((object)txtStockQty.Text);
			if (ItemAmount > 0.0 && ItemQty > 0.0)
			{
				ItemRate = ItemAmount / ItemQty;
				txtRate.Text = ItemRate.ToString();
			}
			else
			{
				txtRate.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDetailAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculatAmount();
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void txtStockQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculatAmount();
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			try
			{
				bindhistory();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
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
			FmLoanAgreementHeader h = new FmLoanAgreementHeader();
			h = FmLoanAgreementHeader.GetByID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells[0].Value));
			if (h != null)
			{
				DataTable dtgrdhistorydetail = new DataTable();
				dtgrdhistorydetail.Columns.Add("LoanCollectralType");
				dtgrdhistorydetail.Columns.Add("CollectralSecurityDescription");
				dtgrdhistorydetail.Columns.Add("Amount", typeof(double));
				dtgrdhistorydetail.Columns.Add("StockQty", typeof(double));
				dtgrdhistorydetail.Columns.Add("Rate", typeof(double));
				dtgrdhistorydetail.Clear();
				for (int i = 0; i < h.FmLoanAgreementDetails.Count; i++)
				{
					dtgrdhistorydetail.Rows.Add(h.FmLoanAgreementDetails[i].LoanColletralType, h.FmLoanAgreementDetails[i].ColletralSecurityDesc, h.FmLoanAgreementDetails[i].fAmount, h.FmLoanAgreementDetails[i].fStockQty, h.FmLoanAgreementDetails[i].QtyRate);
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
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Expected O, but got Unknown
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Expected O, but got Unknown
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Expected O, but got Unknown
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
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
		Appearance appearance25 = new Appearance();
		Appearance appearance29 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.FinanceManagement.LaonFacilityAgreement));
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout DataGridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbLoanCollectralType = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.label17 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtStockQty = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtDetailAmount = new System.Windows.Forms.TextBox();
		this.txtCollectralScurityDesc = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.cmbType = new UltraCombo();
		this.cmbFundType = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.button3 = new System.Windows.Forms.Button();
		this.label15 = new System.Windows.Forms.Label();
		this.txtvalidUpTo = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtIntrestRate = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtValidityDays = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtApprovelAmount = new System.Windows.Forms.TextBox();
		this.cmbfundNonFundBAse = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtFacilityCode = new System.Windows.Forms.TextBox();
		this.cmbBank = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
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
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel6.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanCollectralType).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFundType).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbfundNonFundBAse).BeginInit();
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
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1350, 749);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1350, 749);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1342, 723);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1336, 717);
		this.panel2.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 295);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1336, 422);
		this.panel7.TabIndex = 4;
		this.grdDetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1336, 422);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.groupBox1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 195);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1336, 100);
		this.panel6.TabIndex = 3;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoanCollectralType);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.btnCancel);
		this.groupBox1.Controls.Add(this.btnGridUpdate);
		this.groupBox1.Controls.Add(this.btnAddinGrid);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.txtStockQty);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtDetailAmount);
		this.groupBox1.Controls.Add(this.txtCollectralScurityDesc);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Location = new System.Drawing.Point(9, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1282, 83);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.cmbLoanCollectralType.AllowNull = (DefaultableBoolean)1;
		this.cmbLoanCollectralType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoanCollectralType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoanCollectralType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoanCollectralType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoanCollectralType).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbLoanCollectralType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoanCollectralType).Location = new System.Drawing.Point(10, 45);
		((System.Windows.Forms.Control)(object)this.cmbLoanCollectralType).Name = "cmbLoanCollectralType";
		((System.Windows.Forms.Control)(object)this.cmbLoanCollectralType).Size = new System.Drawing.Size(168, 27);
		((System.Windows.Forms.Control)(object)this.cmbLoanCollectralType).TabIndex = 0;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Red;
		this.label25.Location = new System.Drawing.Point(135, 24);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(14, 17);
		this.label25.TabIndex = 558;
		this.label25.Text = "*";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(827, 23);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(14, 17);
		this.label24.TabIndex = 557;
		this.label24.Text = "*";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Red;
		this.label23.Location = new System.Drawing.Point(902, 23);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(14, 17);
		this.label23.TabIndex = 556;
		this.label23.Text = "*";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Red;
		this.label22.Location = new System.Drawing.Point(717, 23);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(14, 17);
		this.label22.TabIndex = 555;
		this.label22.Text = "*";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.Red;
		this.label21.Location = new System.Drawing.Point(360, 23);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(14, 17);
		this.label21.TabIndex = 554;
		this.label21.Text = "*";
		this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.Location = new System.Drawing.Point(1049, 47);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(62, 25);
		this.btnCancel.TabIndex = 7;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGridUpdate.Location = new System.Drawing.Point(981, 47);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(62, 25);
		this.btnGridUpdate.TabIndex = 6;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = true;
		this.btnGridUpdate.Visible = false;
		this.btnGridUpdate.Click += new System.EventHandler(btnGridUpdate_Click);
		this.btnAddinGrid.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAddinGrid.Location = new System.Drawing.Point(958, 48);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(27, 22);
		this.btnAddinGrid.TabIndex = 5;
		this.btnAddinGrid.UseVisualStyleBackColor = true;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label17.Location = new System.Drawing.Point(862, 23);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(34, 17);
		this.label17.TabIndex = 76;
		this.label17.Text = "Rate";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtRate.Location = new System.Drawing.Point(861, 47);
		this.txtRate.Name = "txtRate";
		this.txtRate.ReadOnly = true;
		this.txtRate.Size = new System.Drawing.Size(92, 25);
		this.txtRate.TabIndex = 4;
		this.txtRate.Text = "0";
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label16.Location = new System.Drawing.Point(764, 23);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(63, 17);
		this.label16.TabIndex = 74;
		this.label16.Text = "Stock Qty";
		this.txtStockQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStockQty.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtStockQty.Location = new System.Drawing.Point(763, 47);
		this.txtStockQty.Name = "txtStockQty";
		this.txtStockQty.Size = new System.Drawing.Size(92, 25);
		this.txtStockQty.TabIndex = 3;
		this.txtStockQty.Text = "0";
		this.txtStockQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtStockQty.TextChanged += new System.EventHandler(txtStockQty_TextChanged);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label14.Location = new System.Drawing.Point(666, 23);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(53, 17);
		this.label14.TabIndex = 72;
		this.label14.Text = "Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label11.Location = new System.Drawing.Point(7, 21);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(125, 17);
		this.label11.TabIndex = 72;
		this.label11.Text = "Loan Collectral Type";
		this.txtDetailAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDetailAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtDetailAmount.Location = new System.Drawing.Point(665, 47);
		this.txtDetailAmount.Name = "txtDetailAmount";
		this.txtDetailAmount.Size = new System.Drawing.Size(92, 25);
		this.txtDetailAmount.TabIndex = 2;
		this.txtDetailAmount.Text = "0";
		this.txtDetailAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDetailAmount.TextChanged += new System.EventHandler(txtDetailAmount_TextChanged);
		this.txtCollectralScurityDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCollectralScurityDesc.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtCollectralScurityDesc.Location = new System.Drawing.Point(180, 47);
		this.txtCollectralScurityDesc.Name = "txtCollectralScurityDesc";
		this.txtCollectralScurityDesc.Size = new System.Drawing.Size(479, 25);
		this.txtCollectralScurityDesc.TabIndex = 1;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label12.Location = new System.Drawing.Point(185, 23);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(181, 17);
		this.label12.TabIndex = 62;
		this.label12.Text = "Collectral Security Description";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbType);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFundType);
		this.panel5.Controls.Add(this.label20);
		this.panel5.Controls.Add(this.label19);
		this.panel5.Controls.Add(this.label18);
		this.panel5.Controls.Add(this.label13);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.label41);
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Controls.Add(this.txtvalidUpTo);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.txtIntrestRate);
		this.panel5.Controls.Add(this.label8);
		this.panel5.Controls.Add(this.label7);
		this.panel5.Controls.Add(this.txtValidityDays);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Controls.Add(this.txtApprovelAmount);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbfundNonFundBAse);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.txtFacilityCode);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBank);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 26);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1336, 169);
		this.panel5.TabIndex = 2;
		this.cmbType.AllowNull = (DefaultableBoolean)1;
		this.cmbType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbType).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbType).Location = new System.Drawing.Point(662, 91);
		((System.Windows.Forms.Control)(object)this.cmbType).Name = "cmbType";
		((System.Windows.Forms.Control)(object)this.cmbType).Size = new System.Drawing.Size(181, 27);
		((System.Windows.Forms.Control)(object)this.cmbType).TabIndex = 7;
		this.cmbFundType.AllowNull = (DefaultableBoolean)1;
		this.cmbFundType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFundType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFundType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbFundType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFundType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFundType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFundType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbFundType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFundType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbFundType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFundType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbFundType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFundType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFundType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFundType).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbFundType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFundType).Location = new System.Drawing.Point(662, 48);
		((System.Windows.Forms.Control)(object)this.cmbFundType).Name = "cmbFundType";
		((System.Windows.Forms.Control)(object)this.cmbFundType).Size = new System.Drawing.Size(181, 27);
		((System.Windows.Forms.Control)(object)this.cmbFundType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbFundType).TextChanged += new System.EventHandler(cmbFundType_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbFundType).Leave += new System.EventHandler(cmbFundType_TextChanged);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(964, 96);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(14, 17);
		this.label20.TabIndex = 553;
		this.label20.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(647, 94);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 552;
		this.label19.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(967, 45);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(14, 17);
		this.label18.TabIndex = 551;
		this.label18.Text = "*";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Red;
		this.label13.Location = new System.Drawing.Point(644, 50);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(14, 17);
		this.label13.TabIndex = 550;
		this.label13.Text = "*";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Red;
		this.label3.Location = new System.Drawing.Point(67, 50);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(14, 17);
		this.label3.TabIndex = 549;
		this.label3.Text = "*";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(357, 47);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(14, 17);
		this.label41.TabIndex = 548;
		this.label41.Text = "*";
		this.panel10.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.panel10.Controls.Add(this.button3);
		this.panel10.Controls.Add(this.label15);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1336, 42);
		this.panel10.TabIndex = 71;
		this.button3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button3.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.button3.FlatAppearance.BorderSize = 0;
		this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button3.Image = (System.Drawing.Image)resources.GetObject("button3.Image");
		this.button3.Location = new System.Drawing.Point(1280, 7);
		this.button3.Margin = new System.Windows.Forms.Padding(2);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(44, 28);
		this.button3.TabIndex = 7;
		this.button3.UseVisualStyleBackColor = false;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(12, 8);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(213, 24);
		this.label15.TabIndex = 1;
		this.label15.Text = "Loan Facility Agreement";
		this.txtvalidUpTo.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtvalidUpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtvalidUpTo.Location = new System.Drawing.Point(176, 92);
		this.txtvalidUpTo.Name = "txtvalidUpTo";
		this.txtvalidUpTo.Size = new System.Drawing.Size(100, 25);
		this.txtvalidUpTo.TabIndex = 5;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label10.Location = new System.Drawing.Point(587, 96);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(65, 17);
		this.label10.TabIndex = 70;
		this.label10.Text = "Rate Type";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label9.Location = new System.Drawing.Point(292, 96);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(74, 17);
		this.label9.TabIndex = 68;
		this.label9.Text = "Intrest Rate";
		this.txtIntrestRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtIntrestRate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtIntrestRate.Location = new System.Drawing.Point(372, 92);
		this.txtIntrestRate.Name = "txtIntrestRate";
		this.txtIntrestRate.Size = new System.Drawing.Size(208, 25);
		this.txtIntrestRate.TabIndex = 6;
		this.txtIntrestRate.Text = "0";
		this.txtIntrestRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label8.Location = new System.Drawing.Point(173, 75);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(75, 17);
		this.label8.TabIndex = 66;
		this.label8.Text = "Valid Up To";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label7.Location = new System.Drawing.Point(8, 96);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(81, 17);
		this.label7.TabIndex = 64;
		this.label7.Text = "Validity Days";
		this.txtValidityDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtValidityDays.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtValidityDays.Location = new System.Drawing.Point(98, 92);
		this.txtValidityDays.Name = "txtValidityDays";
		this.txtValidityDays.Size = new System.Drawing.Size(65, 25);
		this.txtValidityDays.TabIndex = 4;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label6.Location = new System.Drawing.Point(856, 96);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(110, 17);
		this.label6.TabIndex = 62;
		this.label6.Text = "Approvel Amount";
		this.txtApprovelAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovelAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtApprovelAmount.Location = new System.Drawing.Point(986, 92);
		this.txtApprovelAmount.Name = "txtApprovelAmount";
		this.txtApprovelAmount.Size = new System.Drawing.Size(165, 25);
		this.txtApprovelAmount.TabIndex = 8;
		this.txtApprovelAmount.Text = "0";
		this.txtApprovelAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbfundNonFundBAse.AllowNull = (DefaultableBoolean)1;
		this.cmbfundNonFundBAse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbfundNonFundBAse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbfundNonFundBAse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbfundNonFundBAse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbfundNonFundBAse).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbfundNonFundBAse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbfundNonFundBAse).Location = new System.Drawing.Point(986, 47);
		((System.Windows.Forms.Control)(object)this.cmbfundNonFundBAse).Name = "cmbfundNonFundBAse";
		((System.Windows.Forms.Control)(object)this.cmbfundNonFundBAse).Size = new System.Drawing.Size(168, 27);
		((System.Windows.Forms.Control)(object)this.cmbfundNonFundBAse).TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label5.Location = new System.Drawing.Point(847, 50);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(125, 17);
		this.label5.TabIndex = 60;
		this.label5.Text = "Fund/NonFund Base";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label4.Location = new System.Drawing.Point(582, 51);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 17);
		this.label4.TabIndex = 58;
		this.label4.Text = "Fund Type";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label2.Location = new System.Drawing.Point(281, 53);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(81, 17);
		this.label2.TabIndex = 56;
		this.label2.Text = "Facility Code";
		this.txtFacilityCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFacilityCode.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtFacilityCode.Location = new System.Drawing.Point(372, 48);
		this.txtFacilityCode.Name = "txtFacilityCode";
		this.txtFacilityCode.Size = new System.Drawing.Size(208, 25);
		this.txtFacilityCode.TabIndex = 1;
		this.cmbBank.AllowNull = (DefaultableBoolean)1;
		this.cmbBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
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
		((System.Windows.Forms.Control)(object)this.cmbBank).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBank).Location = new System.Drawing.Point(93, 48);
		((System.Windows.Forms.Control)(object)this.cmbBank).Name = "cmbBank";
		((System.Windows.Forms.Control)(object)this.cmbBank).Size = new System.Drawing.Size(183, 27);
		((System.Windows.Forms.Control)(object)this.cmbBank).TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label1.Location = new System.Drawing.Point(26, 53);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(35, 17);
		this.label1.TabIndex = 54;
		this.label1.Text = "Bank";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1336, 26);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripButton6, this.btnnew, this.btnsave, this.btnUpdate, this.toolStripSeparator, this.Print });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1336, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton6.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(23, 24);
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
		this.tabPage2.Size = new System.Drawing.Size(1342, 723);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel4);
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1336, 717);
		this.panel8.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 366);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1336, 351);
		this.panel4.TabIndex = 3;
		this.DataGridHistoryDetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.DataGridHistoryDetail).BackColor = System.Drawing.Color.White;
		this.DataGridHistoryDetail.ColumnAutoResize = true;
		DataGridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryDetail.DesignTimeLayout = DataGridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Name = "DataGridHistoryDetail";
		((ScrollableBase)this.DataGridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Size = new System.Drawing.Size(1336, 351);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).TabIndex = 5;
		this.DataGridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 30);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1336, 336);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1336, 336);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 6;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Click += new System.EventHandler(DataGridHistory_Click);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel9.Controls.Add(this.toolStrip2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1336, 30);
		this.panel9.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.toolStripButton7, this.toolStripButton1, this.toolStripButton2, this.toolStripButton3, this.BtnPrint, this.toolStripSeparator1, this.toolStripButton4, this.toolStripButton5, this.helpToolStripButton });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1336, 28);
		this.toolStrip2.TabIndex = 3;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1350, 749);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "LaonFacilityAgreement";
		this.Text = "LaonFacilityAgreement";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LaonFacilityAgreement_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LaonFacilityAgreement_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel6.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanCollectralType).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFundType).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbfundNonFundBAse).EndInit();
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
		base.ResumeLayout(false);
	}
}
