// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Fcm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Fcm;
using Architecture.WinApp.Account_Definition;
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
namespace Architecture.WinApp.Credit_Financial_Management
{

public class LoanRegistrationHeader : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private bool UpdateMode;

	private int updateDetailIndex = -1;

	private bool FormInitialized;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private DataTable dtdetail = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private Rightsobjects formright = new Rightsobjects();

	private List<LoanRegistrationDetail> lstRemoveRecord = new List<LoanRegistrationDetail>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton BtnPrint;

	private Panel panel10;

	private Label label15;

	private Panel panel1;

	private GroupBox groupBox2;

	private Label label1;

	internal UltraCombo cmbLoanAgreement;

	internal UltraCombo cmbLoanGLAccount;

	private Label label2;

	private Label label4;

	private TextBox txtLoanCode;

	private Panel panel2;

	private GroupBox groupBox1;

	internal UltraCombo cmbLoanSecurityType;

	private TextBox txtvalidDays;

	private Button btnCancel;

	private Button btnGridUpdate;

	private Button btnAdd;

	private Label label9;

	private Label label10;

	private DateTimePicker txtExpiryDate;

	private Label label8;

	private TextBox txtInterest;

	private Label label7;

	private TextBox txtDescription;

	private Label label11;

	private Label label6;

	private Label label3;

	private DateTimePicker txtApprovalDate;

	private DateTimePicker txtCaseDate;

	private Label label14;

	private TextBox txtAmount;

	private TextBox txtCaseNo;

	private Label label12;

	private Panel panel3;

	internal GridEX grdDetail;

	private Panel panel8;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label13;

	private DateTimePicker txtDocDate;

	private Label label20;

	private TextBox txtDocNo;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label18;

	private ToolStrip toolStrip2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnDelete;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnAttachments;

	private ToolStripButton btnShortCut;

	private CheckBox chkPreview;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private TextBox txtremarks;

	internal GridEX DataGridHistoryDetail;

	private Panel panel9;

	private Label label17;

	internal GridEX DataGridHistory;

	private Panel panel4;

	private Label label16;

	private Panel panel6;

	private GroupBox groupBox4;

	private Button btnshow;

	private Label label73;

	internal UltraCombo CmbLoanAccountHistory;

	private DateTimePicker txtToDateHistory;

	private Label label23;

	private Label label76;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label22;

	private Label label77;

	private TextBox txtFromNoHistory;

	private ToolStripButton BtnNewHistory;

	public LoanRegistrationHeader(UserAccount user)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void LoanRegistration_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			BtnPrint.Enabled = formright.DoHavePrintRights;
			chkPreview.Enabled = formright.DoHavePrintRights;
			chkPreview.Checked = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("CaseNo", typeof(string));
			dtdetail.Columns.Add("CaseDate", typeof(DateTime));
			dtdetail.Columns.Add("ApprovalDate", typeof(DateTime));
			dtdetail.Columns.Add("ValidDays", typeof(int));
			dtdetail.Columns.Add("ExpiryDate", typeof(DateTime));
			dtdetail.Columns.Add("LoanSecurityTypeId", typeof(int));
			dtdetail.Columns.Add("LoanSecurityType", typeof(string));
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("Interest", typeof(double));
			dtdetail.Columns.Add("Description", typeof(string));
			FcmComboBind(FcmCombDbCall());
			LoanAgreementeBind();
			DocumentNoFill();
			txtDocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int code = LoanRegistrationHeader.GenerateCode(new LoanRegistrationHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1801,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			txtDocNo.Text = code.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoanAgreementeBind()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			int Idp = Conversion.ToInt(cmbLoanAgreement.Value);
			DataTable dtLoan = LoanAgreementHeader.GetAllDocNo(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dtLoan.Rows.Count > 0)
			{
				DDL.BindDDL(dtLoan, cmbLoanAgreement, "Id", "AgreementFacilityCode", "Agreement Code", ZeroIndex: false);
				((UltraGridBase)cmbLoanAgreement).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Idp > 0)
				{
					if (dtLoan.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idp))
					{
						cmbLoanAgreement.Value = Idp;
					}
					else
					{
						((Control)(object)cmbLoanAgreement).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbLoanAgreement).Text = string.Empty;
				((UltraGridBase)cmbLoanAgreement).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable FcmCombDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return LoanAgreementHeader.FcmAllLookups_ForComboFill(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			return null;
		}
	}

	private void FcmComboBind(DataTable dt)
	{
		try
		{
			DataTable dtLoanSecurityType = new DataTable();
			dtLoanSecurityType.Columns.Add("Id");
			dtLoanSecurityType.Columns.Add("Name");
			dtLoanSecurityType.Columns.Add("Code");
			DataTable dtLoanAccounts = dtLoanSecurityType.Clone();
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow row in dt.Rows)
				{
					string Activity = Conversion.ToString(row["Activity"]);
					if (Activity == "LoanSecurityType")
					{
						dtLoanSecurityType.Rows.Add(row["Id"], row["ReferenceName"], row["ReferenceCode"]);
					}
					else if (Activity == "LoanAccounts")
					{
						dtLoanAccounts.Rows.Add(row["Id"], row["ReferenceName"], row["ReferenceCode"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbLoanSecurityType, dtLoanSecurityType, "Id", "Name", "Loan Security Type", AllColumns: true, Conversion.ToInt(cmbLoanSecurityType.Value));
			InfragisticsHelper.BindAndRetainSelection(cmbLoanGLAccount, dtLoanAccounts, "Id", "Name", "Loan Account", AllColumns: true, Conversion.ToInt(cmbLoanGLAccount.Value));
			InfragisticsHelper.BindAndRetainSelection(CmbLoanAccountHistory, dtLoanAccounts, "Id", "Name", "Loan Account", AllColumns: true, Conversion.ToInt(CmbLoanAccountHistory.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbloanAggrement_Leave(object sender, EventArgs e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		try
		{
			long Idwa1 = Conversion.ToInt64(cmbLoanSecurityType.Value);
			if (Conversion.ToInt(cmbLoanAgreement.Value) > 0)
			{
				DataTable dtLoanAgreement = LoanRegistrationHeader.GetByAgreementId(new LoanRegistrationHeader
				{
					Id = Conversion.ToInt(cmbLoanAgreement.Value)
				});
				if (dtLoanAgreement.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtLoanAgreement, cmbLoanSecurityType, "FmLoanAgreementHeaderId", "LoanSecurityType", "Loan Security Type", false);
					((UltraGridBase)cmbLoanSecurityType).DisplayLayout.Bands[0].Columns[1].Width = 300;
					if (Idwa1 > 0)
					{
						if (dtLoanAgreement.AsEnumerable().Any((DataRow row) => row.Field<long>("FmLoanAgreementHeaderId") == Idwa1))
						{
							cmbLoanSecurityType.Value = Idwa1;
						}
						else
						{
							((Control)(object)cmbLoanSecurityType).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)cmbLoanSecurityType).Text = string.Empty;
					((UltraGridBase)cmbLoanSecurityType).DataSource = null;
				}
			}
			else
			{
				((Control)(object)cmbLoanSecurityType).Text = string.Empty;
				((UltraGridBase)cmbLoanSecurityType).DataSource = null;
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
				txtExpiryDate.Value = txtApprovalDate.Value.AddDays(Convert.ToInt32(txtvalidDays.Text));
				return;
			}
			txtvalidDays.Text = 0.ToString();
			txtExpiryDate.Value = txtApprovalDate.Value;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtDocNo.Text.Trim() == string.Empty || Conversion.ToInt((object)txtDocNo.Text) <= 0)
		{
			MessageBox.Show("Doc No  Field Required");
			txtDocNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLoanAgreement).ActiveRow == null || Conversion.ToInt(cmbLoanAgreement.Value) == 0)
		{
			MessageBox.Show("Loan Aggreement  Field Required");
			cmbLoanAgreement.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLoanGLAccount).ActiveRow == null || Conversion.ToInt(cmbLoanGLAccount.Value) == 0)
		{
			MessageBox.Show("Gl A/c Field Required");
			cmbLoanGLAccount.Focus();
			return false;
		}
		if (txtLoanCode.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Lone Code  Field Required");
			txtLoanCode.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (txtCaseNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("CaseNo Field Required");
			txtCaseNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLoanSecurityType).ActiveRow == null || Conversion.ToInt(cmbLoanSecurityType.Value) == 0)
		{
			MessageBox.Show("Loan Collectral Type  Field Required");
			cmbLoanSecurityType.Focus();
			return false;
		}
		if (txtAmount.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtAmount.Text) > 0.0))
		{
			MessageBox.Show("Amount Must Be Greater Than 0");
			txtAmount.Focus();
			return false;
		}
		if (txtInterest.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtInterest.Text) > 0.0))
		{
			MessageBox.Show("Interest Must Be Greater Than 0");
			txtInterest.Focus();
			return false;
		}
		if (txtvalidDays.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtInterest.Text) > 0.0))
		{
			MessageBox.Show("Valid Days Must Be Greater Than 0");
			txtvalidDays.Focus();
			return false;
		}
		if (txtDescription.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Description Field Required");
			txtDescription.Focus();
			return false;
		}
		return true;
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
			FormDetailReset();
			grdDetail.ClearStructure();
			dtdetail.Clear();
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
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			lstRemoveRecord.Clear();
			((Control)(object)cmbLoanGLAccount).Enabled = true;
			((Control)(object)cmbLoanAgreement).Enabled = true;
			((Control)(object)cmbLoanGLAccount).Text = string.Empty;
			txtLoanCode.Text = string.Empty;
			((Control)(object)cmbLoanAgreement).Text = string.Empty;
			txtCaseDate.Value = DateTime.Now;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			RecId = 0;
			updateDetailIndex = -1;
			grdDetail.ClearStructure();
			dtdetail.Clear();
			cmbLoanGLAccount.Focus();
			DocumentNoFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormDetailReset()
	{
		try
		{
			txtCaseNo.Text = string.Empty;
			txtCaseDate.Value = DateTime.Now;
			txtApprovalDate.Value = DateTime.Now;
			((Control)(object)cmbLoanSecurityType).Text = string.Empty;
			txtDescription.Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtInterest.Text = string.Empty;
			txtExpiryDate.Value = DateTime.Now;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			btnAdd.Visible = true;
			txtCaseNo.Focus();
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
			FcmComboBind(FcmCombDbCall());
			LoanAgreementeBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoanRegistrationHeader_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtdetail.Rows.Add(0, Conversion.ToString((object)txtCaseNo.Text), Conversion.ToDateTime((object)txtCaseDate.Value), Conversion.ToDateTime((object)txtApprovalDate.Value), Conversion.ToInt((object)txtvalidDays.Text), Conversion.ToDateTime((object)txtExpiryDate.Value), Conversion.ToInt(cmbLoanSecurityType.Value), Conversion.ToString((object)((Control)(object)cmbLoanSecurityType).Text), Conversion.ToDouble((object)txtAmount.Text), Conversion.ToDouble((object)txtInterest.Text), Conversion.ToString((object)txtDescription.Text));
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
				FormDetailReset();
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
				dtdetail.Rows[updateDetailIndex]["CaseNo"] = Conversion.ToString((object)txtCaseNo.Text);
				dtdetail.Rows[updateDetailIndex]["CaseDate"] = Conversion.ToDateTime((object)txtCaseDate.Value);
				dtdetail.Rows[updateDetailIndex]["ApprovalDate"] = Conversion.ToDateTime((object)txtApprovalDate.Value);
				dtdetail.Rows[updateDetailIndex]["ValidDays"] = Conversion.ToInt((object)txtvalidDays.Text);
				dtdetail.Rows[updateDetailIndex]["ExpiryDate"] = Conversion.ToDateTime((object)txtExpiryDate.Value);
				dtdetail.Rows[updateDetailIndex]["LoanSecurityTypeId"] = Conversion.ToInt(cmbLoanSecurityType.Value);
				dtdetail.Rows[updateDetailIndex]["LoanSecurityType"] = Conversion.ToString((object)((Control)(object)cmbLoanSecurityType).Text);
				dtdetail.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtAmount.Text);
				dtdetail.Rows[updateDetailIndex]["Interest"] = Conversion.ToDouble((object)txtInterest.Text);
				dtdetail.Rows[updateDetailIndex]["Description"] = Conversion.ToString((object)txtDescription.Text);
				grdDetail.DataSource = dtdetail;
				FormDetailReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				updateDetailIndex = item.RowIndex;
				txtCaseNo.Text = item.Cells["CaseNo"].Value.ToString();
				txtCaseDate.Text = item.Cells["CaseDate"].Value.ToString();
				txtApprovalDate.Text = item.Cells["ApprovalDate"].Value.ToString();
				txtvalidDays.Text = item.Cells["ValidDays"].Value.ToString();
				txtExpiryDate.Text = item.Cells["ExpiryDate"].Text;
				cmbLoanSecurityType.Value = Conversion.ToInt((object)item.Cells["LoanSecurityTypeId"].Value.ToString());
				txtAmount.Text = item.Cells["Amount"].Value.ToString();
				txtInterest.Text = item.Cells["Interest"].Value.ToString();
				txtDescription.Text = Conversion.ToString((object)item.Cells["Description"].Text);
				btnGridUpdate.Visible = true;
				btnCancel.Visible = true;
				btnAdd.Visible = false;
				txtCaseNo.Focus();
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
			FormDetailReset();
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
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["LoanSecurityTypeId"].Visible = false;
			grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			grdDetail.RootTable.Columns.Add("Edit");
			grdDetail.RootTable.Columns["Edit"].Key = "Edit";
			grdDetail.RootTable.Columns["Edit"].Caption = "Edit";
			grdDetail.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdDetail.RootTable.Columns["Edit"].Width = 50;
			grdDetail.RootTable.Columns["Edit"].Position = 0;
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 40;
			grdDetail.RootTable.Columns["Delete"].Position = 1;
			grdDetail.FrozenColumns = 2;
			ctrlGrdBar_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				grdDetail_DoubleClick(null, null);
			}
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			if (updateDetailIndex > -1)
			{
				MessageBox.Show("Please Reset Detail Box Data If You want to Delete Any row from the Grid....");
				return;
			}
			GridEXRow r = grdDetail.CurrentRow;
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					LoanRegistrationDetail vd = new LoanRegistrationDetail();
					vd.LoanRegistrationDetailId = Conversion.ToInt64((object)r.Cells["Id"].Text);
					vd.LoanCaseNo = Conversion.ToString((object)r.Cells["CaseNo"].Text);
					vd.LoanCaseDate = Conversion.ToDateTime(r.Cells["CaseDate"].Value);
					vd.ApprovalDate = Conversion.ToDateTime(r.Cells["ApprovalDate"].Value);
					vd.collateralSecurityTypeId = Conversion.ToInt((object)r.Cells["LoanSecurityTypeId"].Text);
					vd.Amount = Conversion.ToDecimal((object)r.Cells["Amount"].Value.ToString());
					vd.InterestRate = Conversion.ToDecimal((object)r.Cells["Interest"].Text);
					vd.ValidDays = Conversion.ToInt((object)r.Cells["ValidDays"].Text);
					vd.ExpiryDate = Conversion.ToDateTime(r.Cells["ExpiryDate"].Value);
					vd.caseDescription = Conversion.ToString((object)r.Cells["Description"].Text);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtdetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdDetail.UpdateData();
				}
			}
			else if (r != null && (int)r.RowType == 0)
			{
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() <= 0)
			{
				throw new Exception("Grid Record not found");
			}
			if (!FormValidation())
			{
				return;
			}
			LoanRegistrationHeader h = new LoanRegistrationHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				h.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			h.OrganizationId = UserAccount.OrganizationId;
			h.CompanyId = UserAccount.CompanyId;
			h.BranchesId = UserAccount.BranchesId;
			h.ProjectsId = UserAccount.ID;
			h.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			h.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			h.DocDate = Conversion.ToDateTime((object)txtDocDate.Value);
			h.EntryDate = DateTime.Now;
			h.EntryUserId = UserAccount.ID;
			h.ModifyDate = DateTime.Now;
			h.ModifyUserId = UserAccount.ID;
			h.DocumentTypeId = 1801;
			h.IsApproved = false;
			h.ApprovedDate = DateTime.Now;
			h.ApprovedUserId = UserAccount.ID;
			h.LoanAccountId = Conversion.ToInt(cmbLoanGLAccount.Value);
			h.LoanCaseNo = txtLoanCode.Text;
			h.LoanAgreementHeaderId = Conversion.ToInt(cmbLoanAgreement.Value);
			h.FcmLoanRegistrationDetails = new List<LoanRegistrationDetail>();
			h.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow a in rows)
			{
				LoanRegistrationDetail Detail = new LoanRegistrationDetail();
				Detail.LoanRegistrationDetailId = Conversion.ToInt(a.Cells["Id"].Value);
				if (Detail.LoanRegistrationDetailId > 0)
				{
					Detail.ActionTypeId = 2;
				}
				else
				{
					Detail.ActionTypeId = 1;
				}
				Detail.LoanCaseNo = Conversion.ToString((object)a.Cells["CaseNo"].Text);
				Detail.LoanCaseDate = Conversion.ToDateTime(a.Cells["CaseDate"].Value);
				Detail.ApprovalDate = Conversion.ToDateTime(a.Cells["ApprovalDate"].Value);
				Detail.ValidDays = Conversion.ToInt((object)a.Cells["ValidDays"].Text);
				Detail.ExpiryDate = Conversion.ToDateTime(a.Cells["ExpiryDate"].Value);
				Detail.collateralSecurityTypeId = Conversion.ToInt((object)a.Cells["LoanSecurityTypeId"].Text);
				Detail.Amount = Conversion.ToDecimal((object)a.Cells["Amount"].Value.ToString());
				Detail.InterestRate = Conversion.ToDecimal((object)a.Cells["Interest"].Text);
				Detail.caseDescription = Conversion.ToString((object)a.Cells["Description"].Text);
				h.FcmLoanRegistrationDetails.Add(Detail);
			}
			if (lstRemoveRecord.Count > 0)
			{
				for (int j = 0; j < lstRemoveRecord.Count; j++)
				{
					h.FcmLoanRegistrationDetails.Add(lstRemoveRecord[j]);
				}
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				h.AttachmentsList.Add(attach);
				if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
				string fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, attach.Attachment);
				if (!File.Exists(fileSavePath))
				{
					File.Copy(Attachment.dtAttachments.Rows[i][1].ToString(), fileSavePath, overwrite: true);
				}
			}
			int success = LoanRegistrationHeader.Save(h);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully.... ");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
				{
					for (int k = 0; k < AT.RemovedAttachmentListInUpdateCase.Count; k++)
					{
						using (FileStream fs = new FileStream(AT.RemovedAttachmentListInUpdateCase[k].Path, FileMode.Open, FileAccess.ReadWrite))
						{
							fs.Flush();
							fs.Close();
							fs.Dispose();
						}
						bool fileDeleted = false;
						int attempts = 0;
						while (!fileDeleted && attempts < 5)
						{
							try
							{
								File.Delete(AT.RemovedAttachmentListInUpdateCase[k].Path);
								fileDeleted = true;
							}
							catch (IOException)
							{
								attempts++;
								Thread.Sleep(500);
							}
						}
						if (!fileDeleted)
						{
							throw new Exception("Unable to delete file. It is in use by another process.");
						}
					}
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully.... ");
			}
			FormReset();
			if (chkPreview.Checked)
			{
				GeneratePrint(Conversion.ToInt((object)success));
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
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

	private void ReadById(int ID)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		try
		{
			dtdetail.Clear();
			RecId = ID;
			LoanRegistrationHeader h = new LoanRegistrationHeader();
			h = LoanRegistrationHeader.ReadById(RecId);
			if (h != null)
			{
				tabControl1.SelectedIndex = 0;
				txtDocNo.Text = Conversion.ToString((object)h.DocNo);
				txtDocDate.Value = Conversion.ToDateTime((object)h.DocDate);
				cmbLoanAgreement.Value = Conversion.ToInt((object)h.LoanAgreementHeaderId);
				cmbLoanGLAccount.Value = Conversion.ToInt((object)h.LoanAccountId);
				txtLoanCode.Text = Conversion.ToString((object)h.LoanCodeNo);
				txtremarks.Text = Conversion.ToString((object)h.Remarks);
				if (h.FcmLoanRegistrationDetails.Count > 0)
				{
					for (int i = 0; i < h.FcmLoanRegistrationDetails.Count; i++)
					{
						dtdetail.Rows.Add(Conversion.ToInt((object)h.FcmLoanRegistrationDetails[i].LoanRegistrationDetailId), Conversion.ToString((object)h.FcmLoanRegistrationDetails[i].LoanCaseNo), Conversion.ToDateTime((object)h.FcmLoanRegistrationDetails[i].LoanCaseDate), Conversion.ToDateTime((object)h.FcmLoanRegistrationDetails[i].ApprovalDate), Conversion.ToInt((object)h.FcmLoanRegistrationDetails[i].ValidDays), Conversion.ToDateTime((object)h.FcmLoanRegistrationDetails[i].ExpiryDate), Conversion.ToInt((object)h.FcmLoanRegistrationDetails[i].collateralSecurityTypeId), Conversion.ToString((object)h.FcmLoanRegistrationDetails[i].LoanSecutiryType), Conversion.ToDouble((object)h.FcmLoanRegistrationDetails[i].Amount), Conversion.ToDouble((object)h.FcmLoanRegistrationDetails[i].InterestRate), Conversion.ToString((object)h.FcmLoanRegistrationDetails[i].caseDescription));
					}
					grdDetail.DataSource = dtdetail;
					grdDetail.RetrieveStructure();
					GridDetailSetting();
					UpdateMode = true;
				}
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				btnDelete.Visible = true;
				List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(RecId.ToString()), base.Name);
				AT = new Attachment();
				{
					foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
					{
						TempAttachments TA = new TempAttachments();
						TA.Id = 0;
						TA.Path = Attachment.Attachment;
						AT.lst.Add(TA);
					}
					return;
				}
			}
			throw new Exception("Record no Found against this Id");
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
					LoanRegistrationHeader.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Seccessfully");
					FormReset();
				}
			}
			else
			{
				MessageBox.Show("No record found to Delete");
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
				txtFromdateHistory.Value = clsGlobalVariables.ActiveYr.Start_Period;
				txtFromdateHistory.Focus();
			}
			else
			{
				txtDocDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Resethistory()
	{
		try
		{
			txtFromNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbLoanAccountHistory).Text = string.Empty;
			txtFromdateHistory.Value = clsGlobalVariables.ActiveYr.Start_Period;
			txtFromdateHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewHistory_Click(object sender, EventArgs e)
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			BindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		try
		{
			DataTable lst = LoanRegistrationHeader.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				EntryUser = UserAccount.ID,
				DocumentTypeId = 1801,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value),
				ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value),
				FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text),
				ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text),
				AccountId = Conversion.ToInt(CmbLoanAccountHistory.Value),
				CanViewAllRecord = formright.DoHaveCanViewAllRecordRights
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(string));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("LoanAgreementNo", typeof(string));
				dtcol.Columns.Add("LoanAgreementDate", typeof(DateTime));
				dtcol.Columns.Add("LoanAccountTitle", typeof(string));
				dtcol.Columns.Add("LoanCodeNo", typeof(string));
				dtcol.Columns.Add("LoanCaseNo", typeof(string));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUserName", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("IsApproved", typeof(bool));
				dtcol.Columns.Add("ApprovedDate", typeof(DateTime));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow row in lst.Rows)
				{
					dtcol.Rows.Add(row["RecordNo"], row["Id"], row["DocNo"], row["DocDate"], row["DocumentTypeId"], row["LoanAgreementNo"], row["LoanAgreementDate"], row["LoanAccountTitle"], row["LoanCodeNo"], row["LoanCaseNo"], row["EntryUserName"], row["EntryDate"], row["ModifyUserName"], row["ModifyDate"], row["IsApproved"], row["ApprovedDate"], row["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.DataSource = null;
			}
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
			DataGridHistory.ColumnAutoResize = false;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["RecordNo"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			if (formright.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns.Add("Edit");
				DataGridHistory.RootTable.Columns["Edit"].Key = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			}
			if (formright.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns.Add("Print");
				DataGridHistory.RootTable.Columns["Print"].Key = "Print";
				DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
				DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
				DataGridHistory.RootTable.Columns["Print"].Width = 50;
			}
			if (formright.DoHaveUpdateRights && formright.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns["Print"].Position = 0;
				DataGridHistory.RootTable.Columns["Edit"].Position = 1;
				DataGridHistory.FrozenColumns = 2;
			}
			else if (formright.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			else if (formright.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns["Print"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			ctrlGrdBar2_Load(null, null);
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
			if (formright.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				txtDocDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit" && formright.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
			}
			if (e.Column.Key == "Print" && formright.DoHavePrintRights)
			{
				RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				GeneratePrint(RecId);
			}
			if (e.Column.Key == "NoOfAttachments")
			{
				RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(RecId, 1801);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(RecId, 1801);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (DataGridHistory.CurrentRow != null && DataGridHistory.CurrentRow.Cells.Count > 0 && (int)DataGridHistory.CurrentRow.RowType == 0)
			{
				GridDetailBind(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailBind(int Id)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
			LoanRegistrationHeader h = new LoanRegistrationHeader();
			h = LoanRegistrationHeader.ReadById(Id);
			if (h != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("CaseNo", typeof(string));
				dt.Columns.Add("CaseDate", typeof(DateTime));
				dt.Columns.Add("ApprovalDate", typeof(DateTime));
				dt.Columns.Add("ValidDays", typeof(int));
				dt.Columns.Add("ExpiryDate", typeof(DateTime));
				dt.Columns.Add("LoanSecurityTypeId", typeof(int));
				dt.Columns.Add("LoanSecurityType", typeof(string));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("Interest", typeof(double));
				dt.Columns.Add("Description", typeof(string));
				dt.Clear();
				for (int i = 0; i < h.FcmLoanRegistrationDetails.Count; i++)
				{
					dt.Rows.Add(Conversion.ToString((object)h.FcmLoanRegistrationDetails[i].LoanRegistrationDetailId), Conversion.ToString((object)h.FcmLoanRegistrationDetails[i].LoanCaseNo), Conversion.ToDateTime((object)h.FcmLoanRegistrationDetails[i].LoanCaseDate), Conversion.ToDateTime((object)h.FcmLoanRegistrationDetails[i].ApprovalDate), Conversion.ToInt((object)h.FcmLoanRegistrationDetails[i].ValidDays), Conversion.ToDateTime((object)h.FcmLoanRegistrationDetails[i].ExpiryDate), Conversion.ToInt((object)h.FcmLoanRegistrationDetails[i].collateralSecurityTypeId), Conversion.ToString((object)h.FcmLoanRegistrationDetails[i].LoanSecutiryType), Conversion.ToDouble((object)h.FcmLoanRegistrationDetails[i].Amount), Conversion.ToDouble((object)h.FcmLoanRegistrationDetails[i].InterestRate), Conversion.ToString((object)h.FcmLoanRegistrationDetails[i].caseDescription));
				}
				DataGridHistoryDetail.DataSource = dt;
				DataGridHistoryDetail.RetrieveStructure();
				GridDetSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetSetting()
	{
		try
		{
			DataGridHistoryDetail.ColumnAutoResize = false;
			DataGridHistoryDetail.RootTable.Columns["Id"].Visible = false;
			DataGridHistoryDetail.RootTable.Columns["LoanSecurityTypeId"].Visible = false;
			DataGridHistoryDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(DataGridHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachments_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneratePrint(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = LoanRegistrationHeader.LoanAgreementSlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId,
				DocumentTypeId = 1801
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1801_LoanRegistration_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			GeneratePrint(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtInterest_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtvalidDays_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void LoanFacilityAgreement_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtDocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					txtFromdateHistory.Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnsave.Visible && btnsave.Enabled && tabControl1.SelectedIndex == 0)
					{
						btnsave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDocDate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbLoanSecurityType.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					BtnPrint_Click(null, null);
				}
			}
			if (tabControl1.SelectedIndex != 1)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				if (formright.DoHavePrintRights)
				{
					GeneratePrint(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistoryDetail).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)DataGridHistory).Focus();
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
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Case No in Detail Box");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCut_Click(object sender, EventArgs e)
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

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdDetail.CurrentColumn.Key == "Edit")
				{
					grdDetail_DoubleClick(null, null);
				}
				if (grdDetail.CurrentColumn.Key == "Delete")
				{
					LoanRegistrationDetail vd = new LoanRegistrationDetail();
					vd.LoanRegistrationDetailId = Conversion.ToInt64((object)r.Cells["Id"].Text);
					vd.LoanCaseNo = Conversion.ToString((object)r.Cells["CaseNo"].Text);
					vd.LoanCaseDate = Conversion.ToDateTime(r.Cells["CaseDate"].Value);
					vd.ApprovalDate = Conversion.ToDateTime(r.Cells["ApprovalDate"].Value);
					vd.collateralSecurityTypeId = Conversion.ToInt((object)r.Cells["LoanSecurityTypeId"].Text);
					vd.Amount = Conversion.ToDecimal((object)r.Cells["Amount"].Value.ToString());
					vd.InterestRate = Conversion.ToDecimal((object)r.Cells["Interest"].Text);
					vd.ValidDays = Conversion.ToInt((object)r.Cells["ValidDays"].Text);
					vd.ExpiryDate = Conversion.ToDateTime(r.Cells["ExpiryDate"].Value);
					vd.caseDescription = Conversion.ToString((object)r.Cells["Description"].Text);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtdetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdDetail.UpdateData();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
					txtDocDate.Focus();
				}
				if (DataGridHistory.CurrentColumn.Key == "Print" && formright.DoHaveUpdateRights)
				{
					RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
					GeneratePrint(RecId);
				}
				if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(RecId, 1801);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				txtDocDate.Focus();
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
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Expected O, but got Unknown
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Expected O, but got Unknown
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Expected O, but got Unknown
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Expected O, but got Unknown
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a05: Expected O, but got Unknown
		//IL_3837: Unknown result type (might be due to invalid IL or missing references)
		//IL_3841: Expected O, but got Unknown
		//IL_3865: Unknown result type (might be due to invalid IL or missing references)
		//IL_386f: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Credit_Financial_Management.LoanRegistrationHeader));
		GridEXLayout DataGridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnAdd = new System.Windows.Forms.Button();
		this.cmbLoanSecurityType = new UltraCombo();
		this.txtvalidDays = new System.Windows.Forms.TextBox();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtExpiryDate = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.txtInterest = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtApprovalDate = new System.Windows.Forms.DateTimePicker();
		this.txtCaseDate = new System.Windows.Forms.DateTimePicker();
		this.label14 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.txtCaseNo = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.label20 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbLoanAgreement = new UltraCombo();
		this.cmbLoanGLAccount = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtLoanCode = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.chkPreview = new System.Windows.Forms.CheckBox();
		this.label15 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachments = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortCut = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistoryDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.DataGridHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label16 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.label73 = new System.Windows.Forms.Label();
		this.CmbLoanAccountHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label23 = new System.Windows.Forms.Label();
		this.label76 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label22 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel7.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanSecurityType).BeginInit();
		this.panel1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanAgreement).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanGLAccount).BeginInit();
		this.panel10.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLoanAccountHistory).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(967, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.toolStrip1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(959, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel8);
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(2, 266);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(955, 364);
		this.panel3.TabIndex = 76;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 24);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(955, 340);
		this.panel8.TabIndex = 75;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(955, 340);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(955, 24);
		this.panel7.TabIndex = 74;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(912, 0);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 23);
		this.ctrlGrdBar1.TabIndex = 2;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar_Load);
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 141);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(955, 125);
		this.panel2.TabIndex = 75;
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoanSecurityType);
		this.groupBox1.Controls.Add(this.txtvalidDays);
		this.groupBox1.Controls.Add(this.btnCancel);
		this.groupBox1.Controls.Add(this.btnGridUpdate);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtExpiryDate);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtInterest);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtDescription);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtApprovalDate);
		this.groupBox1.Controls.Add(this.txtCaseDate);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.txtCaseNo);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(8, 3);
		this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
		this.groupBox1.Size = new System.Drawing.Size(938, 119);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail Information";
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(758, 85);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(34, 27);
		this.btnAdd.TabIndex = 9;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.cmbLoanSecurityType.AllowNull = (DefaultableBoolean)1;
		this.cmbLoanSecurityType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoanSecurityType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoanSecurityType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance43).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoanSecurityType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbLoanSecurityType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoanSecurityType).Location = new System.Drawing.Point(572, 39);
		((System.Windows.Forms.Control)(object)this.cmbLoanSecurityType).Name = "cmbLoanSecurityType";
		((System.Windows.Forms.Control)(object)this.cmbLoanSecurityType).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoanSecurityType).TabIndex = 5;
		this.txtvalidDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvalidDays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvalidDays.Location = new System.Drawing.Point(384, 41);
		this.txtvalidDays.Name = "txtvalidDays";
		this.txtvalidDays.Size = new System.Drawing.Size(69, 23);
		this.txtvalidDays.TabIndex = 3;
		this.txtvalidDays.Text = "0";
		this.txtvalidDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtvalidDays.TextChanged += new System.EventHandler(txtvalidDays_TextChanged);
		this.txtvalidDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtvalidDays_KeyPress);
		this.btnCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.ForeColor = System.Drawing.Color.White;
		this.btnCancel.Location = new System.Drawing.Point(832, 86);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(74, 25);
		this.btnCancel.TabIndex = 11;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnGridUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGridUpdate.ForeColor = System.Drawing.Color.White;
		this.btnGridUpdate.Location = new System.Drawing.Point(758, 86);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(71, 25);
		this.btnGridUpdate.TabIndex = 10;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = false;
		this.btnGridUpdate.Visible = false;
		this.btnGridUpdate.Click += new System.EventHandler(btnGridUpdate_Click);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(455, 21);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 581;
		this.label9.Text = "Expiry Date";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(384, 21);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(62, 15);
		this.label10.TabIndex = 582;
		this.label10.Text = "Valid Days";
		this.txtExpiryDate.CustomFormat = "dd-MMM-yy";
		this.txtExpiryDate.Enabled = false;
		this.txtExpiryDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtExpiryDate.Location = new System.Drawing.Point(455, 41);
		this.txtExpiryDate.Name = "txtExpiryDate";
		this.txtExpiryDate.Size = new System.Drawing.Size(114, 23);
		this.txtExpiryDate.TabIndex = 4;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(863, 21);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(60, 15);
		this.label8.TabIndex = 580;
		this.label8.Text = "Interest %";
		this.txtInterest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInterest.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInterest.Location = new System.Drawing.Point(863, 41);
		this.txtInterest.Name = "txtInterest";
		this.txtInterest.Size = new System.Drawing.Size(71, 23);
		this.txtInterest.TabIndex = 7;
		this.txtInterest.Text = "0";
		this.txtInterest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInterest.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtInterest_KeyPress);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 67);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(68, 15);
		this.label7.TabIndex = 579;
		this.label7.Text = "Description";
		this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDescription.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDescription.Location = new System.Drawing.Point(4, 87);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(748, 23);
		this.txtDescription.TabIndex = 8;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(572, 21);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(107, 15);
		this.label11.TabIndex = 578;
		this.label11.Text = "Loan Security Type";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(268, 21);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(83, 15);
		this.label6.TabIndex = 576;
		this.label6.Text = "Approval Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(152, 21);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(59, 15);
		this.label3.TabIndex = 577;
		this.label3.Text = "Case Date";
		this.txtApprovalDate.CustomFormat = "dd-MMM-yy";
		this.txtApprovalDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtApprovalDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtApprovalDate.Location = new System.Drawing.Point(268, 41);
		this.txtApprovalDate.Name = "txtApprovalDate";
		this.txtApprovalDate.Size = new System.Drawing.Size(114, 23);
		this.txtApprovalDate.TabIndex = 2;
		this.txtCaseDate.CustomFormat = "dd-MMM-yy";
		this.txtCaseDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtCaseDate.Location = new System.Drawing.Point(152, 41);
		this.txtCaseDate.Name = "txtCaseDate";
		this.txtCaseDate.Size = new System.Drawing.Size(114, 23);
		this.txtCaseDate.TabIndex = 1;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(755, 21);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 575;
		this.label14.Text = "Amount";
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(755, 41);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(105, 23);
		this.txtAmount.TabIndex = 6;
		this.txtAmount.Text = "0";
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.txtCaseNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCaseNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCaseNo.Location = new System.Drawing.Point(4, 41);
		this.txtCaseNo.Name = "txtCaseNo";
		this.txtCaseNo.Size = new System.Drawing.Size(146, 23);
		this.txtCaseNo.TabIndex = 0;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(4, 21);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(41, 15);
		this.label12.TabIndex = 574;
		this.label12.Text = "Case #";
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(2, 57);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(955, 84);
		this.panel1.TabIndex = 74;
		this.groupBox2.Controls.Add(this.txtDocDate);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.txtDocNo);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoanAgreement);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoanGLAccount);
		this.groupBox2.Controls.Add(this.txtremarks);
		this.groupBox2.Controls.Add(this.txtLoanCode);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 4);
		this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
		this.groupBox2.Size = new System.Drawing.Size(944, 77);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDocDate.Location = new System.Drawing.Point(199, 19);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(114, 23);
		this.txtDocDate.TabIndex = 1;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(5, 23);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(88, 15);
		this.label20.TabIndex = 76;
		this.label20.Text = "Doc No && Date";
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Enabled = false;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(102, 19);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(95, 23);
		this.txtDocNo.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.White;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Black;
		this.label13.Location = new System.Drawing.Point(593, 19);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(52, 15);
		this.label13.TabIndex = 72;
		this.label13.Text = "Remarks";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(317, 23);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(69, 15);
		this.label1.TabIndex = 54;
		this.label1.Text = "Loan Gl A/c";
		this.cmbLoanAgreement.AllowNull = (DefaultableBoolean)1;
		this.cmbLoanAgreement.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoanAgreement.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoanAgreement.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Appearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance9;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance47).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoanAgreement).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoanAgreement).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLoanAgreement.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoanAgreement).Location = new System.Drawing.Point(102, 45);
		((System.Windows.Forms.Control)(object)this.cmbLoanAgreement).Name = "cmbLoanAgreement";
		((System.Windows.Forms.Control)(object)this.cmbLoanAgreement).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoanAgreement).TabIndex = 2;
		this.cmbLoanGLAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbLoanGLAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoanGLAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoanGLAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance4;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance41).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoanGLAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoanGLAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLoanGLAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoanGLAccount).Location = new System.Drawing.Point(388, 17);
		((System.Windows.Forms.Control)(object)this.cmbLoanGLAccount).Name = "cmbLoanGLAccount";
		((System.Windows.Forms.Control)(object)this.cmbLoanGLAccount).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoanGLAccount).TabIndex = 3;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(648, 17);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(291, 53);
		this.txtremarks.TabIndex = 5;
		this.txtLoanCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLoanCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoanCode.Location = new System.Drawing.Point(388, 47);
		this.txtLoanCode.Name = "txtLoanCode";
		this.txtLoanCode.Size = new System.Drawing.Size(201, 23);
		this.txtLoanCode.TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(317, 51);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(63, 15);
		this.label2.TabIndex = 56;
		this.label2.Text = "Loan Code";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 51);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(95, 15);
		this.label4.TabIndex = 58;
		this.label4.Text = "Loan Agreement";
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.chkPreview);
		this.panel10.Controls.Add(this.label15);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 29);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(955, 28);
		this.panel10.TabIndex = 73;
		this.chkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPreview.AutoSize = true;
		this.chkPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPreview.ForeColor = System.Drawing.Color.White;
		this.chkPreview.Location = new System.Drawing.Point(867, 6);
		this.chkPreview.Name = "chkPreview";
		this.chkPreview.Size = new System.Drawing.Size(84, 20);
		this.chkPreview.TabIndex = 6;
		this.chkPreview.Text = "Preview";
		this.chkPreview.UseVisualStyleBackColor = true;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(5, 6);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(154, 19);
		this.label15.TabIndex = 1;
		this.label15.Text = "Loan Registration";
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator2, this.btnsave, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnAttachments, this.toolStripSeparator3, this.BtnPrint,
			this.toolStripSeparator4, this.btnShortCut
		});
		this.toolStrip1.Location = new System.Drawing.Point(2, 2);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(955, 27);
		this.toolStrip1.TabIndex = 5;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(61, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(66, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(82, 24);
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
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnAttachments.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachments.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachments.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachments.Name = "btnAttachments";
		this.btnAttachments.Size = new System.Drawing.Size(116, 24);
		this.btnAttachments.Text = "Attachments";
		this.btnAttachments.Click += new System.EventHandler(btnAttachments_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.BtnPrint.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(107, 24);
		this.BtnPrint.Text = "1801-&Print";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnShortCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCut.Name = "btnShortCut";
		this.btnShortCut.Size = new System.Drawing.Size(126, 24);
		this.btnShortCut.Text = "ShortCut Keys";
		this.btnShortCut.Click += new System.EventHandler(btnShortCut_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(959, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistoryDetail).BackColor = System.Drawing.Color.White;
		this.DataGridHistoryDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryDetail.DesignTimeLayout = DataGridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistoryDetail.FilterMode = (FilterMode)2;
		this.DataGridHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryDetail.GroupByBoxVisible = false;
		this.DataGridHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Location = new System.Drawing.Point(2, 471);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Name = "DataGridHistoryDetail";
		((ScrollableBase)this.DataGridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Size = new System.Drawing.Size(955, 159);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).TabIndex = 1;
		this.DataGridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label17);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(2, 446);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(955, 25);
		this.panel9.TabIndex = 17;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 5);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(211, 17);
		this.label17.TabIndex = 1;
		this.label17.Text = "Detail Of Above Selected Row";
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(2, 130);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(955, 316);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar2);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 105);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(955, 25);
		this.panel4.TabIndex = 15;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(906, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 23);
		this.ctrlGrdBar2.TabIndex = 6;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(4, 5);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(40, 17);
		this.label16.TabIndex = 1;
		this.label16.Text = "Main";
		this.panel6.Controls.Add(this.groupBox4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(2, 53);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(955, 52);
		this.panel6.TabIndex = 14;
		this.groupBox4.Controls.Add(this.btnshow);
		this.groupBox4.Controls.Add(this.label73);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoanAccountHistory);
		this.groupBox4.Controls.Add(this.txtToDateHistory);
		this.groupBox4.Controls.Add(this.label23);
		this.groupBox4.Controls.Add(this.label76);
		this.groupBox4.Controls.Add(this.txtToDocNoHistory);
		this.groupBox4.Controls.Add(this.txtFromdateHistory);
		this.groupBox4.Controls.Add(this.label22);
		this.groupBox4.Controls.Add(this.label77);
		this.groupBox4.Controls.Add(this.txtFromNoHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(4, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(948, 46);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(885, 15);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 25);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(599, 20);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(81, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Loan Account";
		this.CmbLoanAccountHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbLoanAccountHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoanAccountHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoanAccountHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance30).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoanAccountHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLoanAccountHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoanAccountHistory).Location = new System.Drawing.Point(681, 14);
		((System.Windows.Forms.Control)(object)this.CmbLoanAccountHistory).Name = "CmbLoanAccountHistory";
		((System.Windows.Forms.Control)(object)this.CmbLoanAccountHistory).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoanAccountHistory).TabIndex = 4;
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(232, 16);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtToDateHistory.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(181, 20);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(48, 15);
		this.label23.TabIndex = 171;
		this.label23.Text = "Date To";
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(2, 20);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(63, 15);
		this.label76.TabIndex = 171;
		this.label76.Text = "Date From";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(525, 16);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(72, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(66, 16);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtFromdateHistory.TabIndex = 0;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(480, 20);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(45, 15);
		this.label22.TabIndex = 173;
		this.label22.Text = "Doc To";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(345, 20);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(60, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "Doc From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(406, 16);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(72, 23);
		this.txtFromNoHistory.TabIndex = 2;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label18);
		this.panel5.Controls.Add(this.toolStrip2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 2);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(955, 51);
		this.panel5.TabIndex = 10;
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.Teal;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label18.Location = new System.Drawing.Point(6, 30);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(217, 19);
		this.label18.TabIndex = 5;
		this.label18.Text = "Loan Registration History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.toolStripSeparator1 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(955, 27);
		this.toolStrip2.TabIndex = 4;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(967, 661);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "LoanRegistrationHeader";
		this.Text = "LoanRegistration";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(LoanRegistrationHeader_FormClosed);
		base.Load += new System.EventHandler(LoanRegistration_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoanFacilityAgreement_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanSecurityType).EndInit();
		this.panel1.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanAgreement).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanGLAccount).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLoanAccountHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
