using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Fcm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Fcm;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Credit_Financial_Management;

public class LoanFacilityAgreement : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtdetail = new DataTable();

	private DataTable dtGridHistory = new DataTable();

	private int updateDetailIndex = -1;

	private int RecId;

	private int DocumentTypeId = 1800;

	private bool UpdateMode;

	private bool FormInitialized;

	private string ScreenName = "LoanFacilityAgreement";

	private int DefaultDaysToLessFromHistoryFromDate;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private List<LoanAgreementDetail> lstRemoveRecord = new List<LoanAgreementDetail>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton BtnPrint;

	private Panel panel10;

	private Label label15;

	private Panel panel1;

	private GroupBox groupBox2;

	internal UltraCombo cmbFacilityType;

	internal UltraCombo cmbInterestRateType;

	private Label label1;

	internal UltraCombo cmbLoanType;

	internal UltraCombo cmbLoanAccount;

	private TextBox txtAgreementCode;

	private DateTimePicker txtvalidUpTo;

	private Label label2;

	private Label label10;

	private Label label4;

	private Label label9;

	private Label label5;

	private TextBox txtInterestRate;

	private TextBox txtApprovalAmount;

	private Label label6;

	private Label label7;

	private TextBox txtValidityDays;

	private Panel panel2;

	private GroupBox groupBox1;

	internal UltraCombo cmbLoanSecurityType;

	private Button btnCancel;

	private Button btnGridUpdate;

	private Button btnplus;

	private Label label17;

	private TextBox txtRate;

	private Label label16;

	private TextBox txtStockQty;

	private Label label14;

	private Label label11;

	private TextBox txtDetailAmount;

	private TextBox txtLoanSecurityDescription;

	private Label label12;

	private Panel panel3;

	internal GridEX grdDetail;

	private ToolStrip toolStrip2;

	private ToolStripSeparator toolStripSeparator1;

	private Panel panel6;

	private Panel panel7;

	private CtrlGrdBar HistoryCtrlGrdBar;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label18;

	private ToolStripButton btnDelete;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnAttachments;

	private ToolStripButton btnShortCut;

	internal UltraCombo cmbColetraltype;

	private Label label19;

	private DateTimePicker txtDocDate;

	private Label label21;

	private Label label20;

	private TextBox txtDocNo;

	private CheckBox chkPreview;

	private TextBox txtremarks;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator;

	internal GridEX DataGridHistoryDetail;

	private Panel panel4;

	private Label label13;

	internal GridEX DataGridHistory;

	private Panel panel9;

	private Label label8;

	private Panel panel8;

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

	public LoanFacilityAgreement(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void LoanFacilityAgreement_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(999, 700);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentCustom()
	{
		try
		{
			ScreenName = Conversion.ToString((object)base.Name);
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("LoanSecurityTypeId", typeof(int));
			dtdetail.Columns.Add("LoanSecurityType", typeof(string));
			dtdetail.Columns.Add("CollateralTypeId", typeof(int));
			dtdetail.Columns.Add("CollateralType", typeof(string));
			dtdetail.Columns.Add("fAmount", typeof(double));
			dtdetail.Columns.Add("fStockQty", typeof(double));
			dtdetail.Columns.Add("Rate", typeof(double));
			dtdetail.Columns.Add("LoanSecurityDescription", typeof(string));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			string documentNoData = DocumentNoDbCall();
			DataTable comboData = FcmCombDbCall();
			return new
			{
				DocumentNoData = documentNoData,
				ComboData = comboData
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				btnsave.Enabled = formright.DoHaveSaveRight;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				BtnPrint.Enabled = formright.DoHavePrintRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formright);
				GetConfigurationsFromGlobal();
				if (RecId == 0)
				{
					txtDocNo.Text = result.DocumentNoData;
				}
				FcmComboBind(result.ComboData);
				InterestRateTypeFill();
				DataTable dataTable = dtdetail;
				if (dataTable != null && dataTable.Columns.Count > 0)
				{
					grdDetail.DataSource = dtdetail;
					grdDetail.RetrieveStructure();
					GridDetailSetting();
				}
				txtFromdateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				txtToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				txtDocDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string DocumentNoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			return Conversion.ToString((object)LoanAgreementHeader.GenerateCode(new LoanAgreementHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void InterestRateTypeFill()
	{
		try
		{
			DataTable dttypes = new DataTable();
			dttypes.Columns.Add("Id");
			dttypes.Columns.Add("Type");
			dttypes.Rows.Add(1, "Fixed Rate");
			dttypes.Rows.Add(2, "Floating Rate");
			InfragisticsHelper.BindAndRetainSelection(cmbInterestRateType, dttypes, "Id", "Type", "Rate type", AllColumns: true, Conversion.ToInt((object)cmbInterestRateType));
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
			DataTable dtLoanType = dtLoanSecurityType.Clone();
			DataTable dtCreditFacilityType = dtLoanSecurityType.Clone();
			DataTable dtCollateralType = dtLoanSecurityType.Clone();
			foreach (DataRow row in dt.Rows)
			{
				string activity = Conversion.ToString(row["Activity"]);
				object id = row["Id"];
				object name = row["ReferenceName"];
				object code = row["ReferenceCode"];
				switch (activity)
				{
				case "LoanType":
					dtLoanType.Rows.Add(id, name, code);
					break;
				case "CreditFacilityType":
					dtCreditFacilityType.Rows.Add(id, name, code);
					break;
				case "LoanSecurityType":
					dtLoanSecurityType.Rows.Add(id, name, code);
					break;
				case "CollateralType":
					dtCollateralType.Rows.Add(id, name, code);
					break;
				case "LoanAccounts":
					dtLoanAccounts.Rows.Add(id, name, code);
					break;
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbLoanSecurityType, dtLoanSecurityType, "Id", "Name", "Loan Security Type", AllColumns: true, Conversion.ToInt(cmbLoanSecurityType.Value));
			InfragisticsHelper.BindAndRetainSelection(cmbLoanAccount, dtLoanAccounts, "Id", "Name", "Loan Account", AllColumns: true, Conversion.ToInt(cmbLoanAccount.Value));
			InfragisticsHelper.BindAndRetainSelection(CmbLoanAccountHistory, dtLoanAccounts, "Id", "Name", "Loan Account", AllColumns: true, Conversion.ToInt(CmbLoanAccountHistory.Value));
			InfragisticsHelper.BindAndRetainSelection(cmbLoanType, dtLoanType, "Id", "Name", "Loan Type", AllColumns: false, Conversion.ToInt(cmbLoanType.Value));
			InfragisticsHelper.BindAndRetainSelection(cmbFacilityType, dtCreditFacilityType, "Id", "Name", "Facility Type", AllColumns: false, Conversion.ToInt(cmbFacilityType.Value));
			InfragisticsHelper.BindAndRetainSelection(cmbColetraltype, dtCollateralType, "Id", "Name", "Collateral Type", AllColumns: false, Conversion.ToInt(cmbColetraltype.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			(txtDocNo, "Doc No", FormHelper.ValidType.INT),
			((Control)(object)cmbLoanAccount, "Loan Account", FormHelper.ValidType.INT),
			((Control)(object)cmbFacilityType, "Facility Type", FormHelper.ValidType.INT),
			((Control)(object)cmbLoanType, "Loan Type", FormHelper.ValidType.INT),
			(txtAgreementCode, "Agreement Code", FormHelper.ValidType.String),
			(txtValidityDays, "Validity Days", FormHelper.ValidType.Double),
			(txtInterestRate, "Interest Rate", FormHelper.ValidType.String),
			((Control)(object)cmbInterestRateType, "Interest Rate Type", FormHelper.ValidType.INT),
			(txtApprovalAmount, "Approval Amount", FormHelper.ValidType.Double)
		}))
		{
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)cmbLoanSecurityType, "Loan Security Type", FormHelper.ValidType.INT),
			((Control)(object)cmbColetraltype, "Collateral Type", FormHelper.ValidType.INT),
			(txtDetailAmount, "Amount", FormHelper.ValidType.Double),
			(txtStockQty, "Stock Qty", FormHelper.ValidType.Double),
			(txtRate, "Rate", FormHelper.ValidType.Double),
			(txtLoanSecurityDescription, "Loan Security Description", FormHelper.ValidType.String)
		}))
		{
			return false;
		}
		return true;
	}

	private void btnAddinGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtdetail.Rows.Add(0, Conversion.ToInt(cmbLoanSecurityType.Value), Conversion.ToString((object)((Control)(object)cmbLoanSecurityType).Text), Conversion.ToInt(cmbColetraltype.Value), Conversion.ToString((object)((Control)(object)cmbColetraltype).Text), txtDetailAmount.Text, txtStockQty.Text, txtRate.Text, txtLoanSecurityDescription.Text);
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

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				((Control)(object)cmbLoanSecurityType).Text = item.Cells["LoanSecurityTypeId"].Text.ToString();
				((Control)(object)cmbColetraltype).Text = item.Cells["CollateralTypeId"].Text.ToString();
				txtDetailAmount.Text = item.Cells["fAmount"].Value.ToString();
				txtStockQty.Text = item.Cells["fStockQty"].Value.ToString();
				txtRate.Text = item.Cells["Rate"].Value.ToString();
				txtLoanSecurityDescription.Text = item.Cells["LoanSecurityDescription"].Text.ToString();
				btnGridUpdate.Visible = true;
				btnCancel.Visible = true;
				btnplus.Visible = false;
				cmbLoanSecurityType.Focus();
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

	private void btnGridUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtdetail.Rows[updateDetailIndex]["LoanSecurityTypeId"] = cmbLoanSecurityType.Value;
				dtdetail.Rows[updateDetailIndex]["CollateralTypeId"] = cmbColetraltype.Value;
				dtdetail.Rows[updateDetailIndex]["fAmount"] = txtDetailAmount.Text;
				dtdetail.Rows[updateDetailIndex]["fStockQty"] = txtStockQty.Text;
				dtdetail.Rows[updateDetailIndex]["Rate"] = txtRate.Text;
				dtdetail.Rows[updateDetailIndex]["LoanSecurityDescription"] = txtLoanSecurityDescription.Text;
				grdDetail.DataSource = dtdetail;
				FormDetailReset();
			}
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
			List<string> hiddenColumns = new List<string> { "Id", "LoanSecurityTypeId", "CollateralTypeId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 3, 3, hiddenColumns);
			GridEX_Helper.AddButton(grdDetail, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(grdDetail, "Delete", "X", 20, 1);
			grdDetail.FrozenColumns = 2;
			HistoryCtrlGrdBar_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Edit")
				{
					grdDetail_DoubleClick(null, null);
				}
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex > -1)
			{
				MessageBox.Show("Please Reset Detail Box Data If You want to Delete Any row from the Grid....");
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				LoanAgreementDetail vd = new LoanAgreementDetail();
				vd.LoanAgreementDetailId = Id;
				FillDetailListCommonForInsertAndDelete(vd, r);
				vd.ActionTypeId = 3;
				lstRemoveRecord.Add(vd);
				dtdetail.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			else
			{
				r.Delete();
			}
			grdDetail.UpdateData();
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
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			lstRemoveRecord.Clear();
			RecId = 0;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			((Control)(object)cmbLoanAccount).Text = string.Empty;
			txtApprovalAmount.Text = string.Empty;
			txtAgreementCode.Text = string.Empty;
			((Control)(object)cmbLoanType).Text = string.Empty;
			txtValidityDays.Text = string.Empty;
			txtvalidUpTo.Value = DateTime.Now;
			txtInterestRate.Text = string.Empty;
			((Control)(object)cmbInterestRateType).Text = string.Empty;
			((Control)(object)cmbColetraltype).Text = string.Empty;
			FormDetailReset();
			dtdetail.Clear();
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
			txtDocNo.Text = DocumentNoDbCall();
			txtDocDate.Focus();
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
			((Control)(object)cmbLoanSecurityType).Text = string.Empty;
			txtLoanSecurityDescription.Text = string.Empty;
			txtDetailAmount.Text = string.Empty;
			txtStockQty.Text = string.Empty;
			txtRate.Text = string.Empty;
			updateDetailIndex = -1;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			btnplus.Visible = true;
			cmbLoanSecurityType.Focus();
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
			GetConfigurationsFromGlobal();
			FcmComboBind(FcmCombDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoanFacilityAgreement_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnplus_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "txtRate")
			{
				cmbLoanSecurityType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			double TotalHeaderDetail = 0.0;
			double TotalHeaderAmount = Conversion.ToDouble((object)txtApprovalAmount.Text.Trim());
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				TotalHeaderDetail += Conversion.ToDouble(item.Cells["fAmount"].Value);
			}
			if (TotalHeaderDetail != TotalHeaderAmount)
			{
				throw new Exception("Approval Amount And Detail Total Amount Is Not Equal Please Check This");
			}
			if (!FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			LoanAgreementHeader fcmLoanAgreement = new LoanAgreementHeader();
			fcmLoanAgreement.OrganizationId = UserAccount.OrganizationId;
			fcmLoanAgreement.CompanyId = UserAccount.CompanyId;
			fcmLoanAgreement.BranchId = UserAccount.BranchesId;
			fcmLoanAgreement.ProjectsId = UserAccount.BranchesId;
			fcmLoanAgreement.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			fcmLoanAgreement.Id = RecId;
			fcmLoanAgreement.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			fcmLoanAgreement.DocDate = Conversion.ToDateTime((object)txtDocDate.Value);
			fcmLoanAgreement.DocumentTypeId = DocumentTypeId;
			fcmLoanAgreement.EntryDate = DateTime.Now;
			fcmLoanAgreement.ModifyDate = DateTime.Now;
			fcmLoanAgreement.EntryUserId = UserAccount.ID;
			fcmLoanAgreement.ModifyUserId = UserAccount.ID;
			fcmLoanAgreement.ApprovedDate = DateTime.Now;
			fcmLoanAgreement.ApprovedUserId = UserAccount.ID;
			fcmLoanAgreement.LoanAccountsId = Conversion.ToInt(cmbLoanAccount.Value);
			fcmLoanAgreement.CreditFacilityTypeId = Conversion.ToInt(cmbFacilityType.Value);
			fcmLoanAgreement.LoanTypeId = Conversion.ToInt(cmbLoanType.Value);
			fcmLoanAgreement.AgreementFacilityCode = Conversion.ToString((object)txtAgreementCode.Text);
			fcmLoanAgreement.ValidDays = Conversion.ToInt((object)txtValidityDays.Text);
			fcmLoanAgreement.ExpiryDate = Conversion.ToDateTime((object)txtvalidUpTo.Value);
			fcmLoanAgreement.InterestRate = Conversion.ToDecimal((object)txtInterestRate.Text);
			fcmLoanAgreement.InterestType = Conversion.ToString((object)((Control)(object)cmbInterestRateType).Text);
			fcmLoanAgreement.ApprovedAmount = Conversion.ToDecimal((object)txtApprovalAmount.Text);
			fcmLoanAgreement.IsApproved = false;
			fcmLoanAgreement.LoanAgreementDetailsList = new List<LoanAgreementDetail>();
			if (lstRemoveRecord.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					fcmLoanAgreement.LoanAgreementDetailsList.Add(lstRemoveRecord[i]);
				}
			}
			rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				LoanAgreementDetail vd = new LoanAgreementDetail();
				vd.LoanAgreementDetailId = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.LoanAgreementDetailId <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd, r);
				FormHelper.ValidateField(vd.LoanSecutiryTypeId, "LoanSecurity Type", r.RowIndex);
				FormHelper.ValidateField(vd.CollateralTypeId, "Collateral Type", r.RowIndex);
				FormHelper.ValidateField(vd.fAmount, "Amount", r.RowIndex);
				FormHelper.ValidateField(vd.fStockQty, "StockQty", r.RowIndex);
				FormHelper.ValidateField(vd.fStockQty, "StockQty", r.RowIndex);
				FormHelper.ValidateField(vd.QtyRate, "Rate", r.RowIndex);
				FormHelper.ValidateField(vd.LoanSecurityDescription, "LoanSecurity Description", r.RowIndex);
				fcmLoanAgreement.LoanAgreementDetailsList.Add(vd);
			}
			fcmLoanAgreement.AttachmentsList = new List<DMSAttachments>();
			fcmLoanAgreement.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<LoanAgreementHeader>(fcmLoanAgreement, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			fcmLoanAgreement.AttachmentsValues = AttachmentsValues;
			fcmLoanAgreement.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = LoanAgreementHeader.Save(fcmLoanAgreement);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully.... ");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, DocumentTypeId, base.Name);
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
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
		try
		{
			FormReset();
			RecId = ID;
			LoanAgreementHeader fcmLoanAgreement = LoanAgreementHeader.ReadById(RecId);
			if (fcmLoanAgreement == null)
			{
				throw new Exception("Record no Found against this Id");
			}
			tabControl1.SelectedIndex = 0;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtDocNo.Text = Conversion.ToString((object)fcmLoanAgreement.DocNo);
			txtDocDate.Value = Conversion.ToDateTime((object)fcmLoanAgreement.DocDate);
			cmbLoanAccount.Value = Conversion.ToInt((object)fcmLoanAgreement.LoanAccountsId);
			cmbFacilityType.Value = Conversion.ToInt((object)fcmLoanAgreement.CreditFacilityTypeId);
			cmbLoanType.Value = Conversion.ToInt((object)fcmLoanAgreement.LoanTypeId);
			txtAgreementCode.Text = Conversion.ToString((object)fcmLoanAgreement.AgreementFacilityCode);
			txtValidityDays.Text = Conversion.ToString((object)fcmLoanAgreement.ValidDays.ToString());
			txtvalidUpTo.Value = Conversion.ToDateTime((object)fcmLoanAgreement.ExpiryDate);
			txtInterestRate.Text = fcmLoanAgreement.InterestRate.ToString(clsGlobalVariables.DecimalRateFormate);
			((Control)(object)cmbInterestRateType).Text = fcmLoanAgreement.InterestType.ToString();
			txtApprovalAmount.Text = fcmLoanAgreement.ApprovedAmount.ToString(clsGlobalVariables.stringFormatsingle);
			AttachmentsValues = fcmLoanAgreement.AttachmentsValues;
			CustomAttachmentsValues = fcmLoanAgreement.CustomAttachmentsValues;
			if (fcmLoanAgreement.LoanAgreementDetailsList.Count > 0)
			{
				for (int i = 0; i < fcmLoanAgreement.LoanAgreementDetailsList.Count; i++)
				{
					dtdetail.Rows.Add(fcmLoanAgreement.LoanAgreementDetailsList[i].LoanAgreementDetailId, fcmLoanAgreement.LoanAgreementDetailsList[i].LoanSecutiryTypeId, fcmLoanAgreement.LoanAgreementDetailsList[i].LoanSecutiryType, fcmLoanAgreement.LoanAgreementDetailsList[i].CollateralTypeId, fcmLoanAgreement.LoanAgreementDetailsList[i].CollateralType, fcmLoanAgreement.LoanAgreementDetailsList[i].fAmount, fcmLoanAgreement.LoanAgreementDetailsList[i].fStockQty, fcmLoanAgreement.LoanAgreementDetailsList[i].QtyRate, fcmLoanAgreement.LoanAgreementDetailsList[i].LoanSecurityDescription);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
				UpdateMode = true;
			}
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
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
					LoanAgreementHeader.DeleteByID(UserAccount.ID, RecId);
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

	private void FillDetailListCommonForInsertAndDelete(LoanAgreementDetail vd, GridEXRow a)
	{
		vd.LoanSecutiryTypeId = Conversion.ToInt(a.Cells["LoanSecurityTypeId"].Value);
		vd.CollateralTypeId = Conversion.ToInt(a.Cells["CollateralTypeId"].Value);
		vd.fAmount = Conversion.ToDecimal((object)a.Cells["fAmount"].Text);
		vd.fStockQty = Conversion.ToDecimal((object)a.Cells["fStockQty"].Text);
		vd.QtyRate = Conversion.ToDecimal((object)a.Cells["Rate"].Text);
		vd.LoanSecurityDescription = Conversion.ToString((object)a.Cells["LoanSecurityDescription"].Text);
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
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		try
		{
			DataTable lst = LoanAgreementHeader.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				EntryUser = UserAccount.ID,
				DocumentTypeId = DocumentTypeId,
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
				DataTable dataTable = dtGridHistory;
				if (dataTable != null && dataTable.Columns.Count == 0)
				{
					dtGridHistory.Columns.Add("RecordNo", typeof(int));
					dtGridHistory.Columns.Add("Id", typeof(int));
					dtGridHistory.Columns.Add("DocumentTypeId", typeof(int));
					dtGridHistory.Columns.Add("DocNo", typeof(int));
					dtGridHistory.Columns.Add("DocDate", typeof(DateTime));
					dtGridHistory.Columns.Add("LoanAccounts", typeof(string));
					dtGridHistory.Columns.Add("FacilityType", typeof(string));
					dtGridHistory.Columns.Add("Loantype", typeof(string));
					dtGridHistory.Columns.Add("AgreementFacilityCode", typeof(string));
					dtGridHistory.Columns.Add("ValidDays", typeof(int));
					dtGridHistory.Columns.Add("ExpiryDate", typeof(DateTime));
					dtGridHistory.Columns.Add("InterestRate", typeof(double));
					dtGridHistory.Columns.Add("InterestType", typeof(string));
					dtGridHistory.Columns.Add("ApprovedAmount", typeof(double));
					dtGridHistory.Columns.Add("EntryDate", typeof(DateTime));
					dtGridHistory.Columns.Add("EntryUser", typeof(string));
					dtGridHistory.Columns.Add("ModifyUser", typeof(string));
					dtGridHistory.Columns.Add("ModifyDate", typeof(DateTime));
					dtGridHistory.Columns.Add("IsApproved", typeof(bool));
					dtGridHistory.Columns.Add("ApprovedDate", typeof(DateTime));
					dtGridHistory.Columns.Add("NoOfAttachments", typeof(int));
				}
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtGridHistory.Rows.Add(Conversion.ToInt(lst.Rows[i]["RecordNo"]), Conversion.ToInt(lst.Rows[i]["Id"]), Conversion.ToInt(lst.Rows[i]["DocumentTypeId"]), Conversion.ToInt(lst.Rows[i]["DocNo"]), Conversion.ToDateTime(lst.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToString(lst.Rows[i]["LoanAccounts"]), Conversion.ToString(lst.Rows[i]["FacilityType"]), Conversion.ToString(lst.Rows[i]["LoanType"]), Conversion.ToString(lst.Rows[i]["AgreementFacilityCode"]), Conversion.ToInt(lst.Rows[i]["ValidDays"]), Conversion.ToDateTime(lst.Rows[i]["ExpiryDate"]), Conversion.ToDouble(lst.Rows[i]["InterestRate"]), Conversion.ToString(lst.Rows[i]["InterestType"]), Conversion.ToDouble(lst.Rows[i]["ApprovedAmount"]), Conversion.ToDateTime(lst.Rows[i]["EntryDate"]), Conversion.ToString(lst.Rows[i]["EntryUserName"]), Conversion.ToString(lst.Rows[i]["ModifyUserName"]), Conversion.ToDateTime(lst.Rows[i]["ModifyDate"]), Conversion.ToBool(lst.Rows[i]["IsApproved"]), Conversion.ToDateTime(lst.Rows[i]["ApprovedDate"]), Conversion.ToInt(lst.Rows[i]["NoOfAttachments"]));
				}
				DataGridHistory.DataSource = dtGridHistory;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.DataSource = null;
				DataGridHistoryDetail.DataSource = null;
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
			List<string> hiddenColumns = new List<string> { "Id", "RecordNo", "DocumentTypeId" };
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 3, 3, hiddenColumns);
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			if (formright.DoHaveUpdateRights)
			{
				GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 40, DataGridHistory.RootTable.Columns["DocNo"].Width - 1);
			}
			if (formright.DoHavePrintRights)
			{
				GridEX_Helper.AddButton(DataGridHistory, "Print", "Print", 40, DataGridHistory.RootTable.Columns["DocNo"].Width - 1);
			}
			DataGridHistory.FrozenColumns = 2;
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
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(RecId, DocumentTypeId);
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
			LoanAgreementHeader h = new LoanAgreementHeader();
			h = LoanAgreementHeader.ReadById(Id);
			if (h != null)
			{
				DataTable dtgrdhistorydetail = new DataTable();
				dtgrdhistorydetail.Columns.Add("LoanSecurityType", typeof(string));
				dtgrdhistorydetail.Columns.Add("CollateralType", typeof(string));
				dtgrdhistorydetail.Columns.Add("fAmount", typeof(double));
				dtgrdhistorydetail.Columns.Add("fStockQty", typeof(double));
				dtgrdhistorydetail.Columns.Add("Rate", typeof(double));
				dtgrdhistorydetail.Columns.Add("LoanSecurityDescription", typeof(string));
				dtgrdhistorydetail.Clear();
				for (int i = 0; i < h.LoanAgreementDetailsList.Count; i++)
				{
					dtgrdhistorydetail.Rows.Add(h.LoanAgreementDetailsList[i].LoanSecutiryType, h.LoanAgreementDetailsList[i].CollateralType, h.LoanAgreementDetailsList[i].fAmount, h.LoanAgreementDetailsList[i].fStockQty, h.LoanAgreementDetailsList[i].QtyRate, h.LoanAgreementDetailsList[i].LoanSecurityDescription);
				}
				DataGridHistoryDetail.DataSource = dtgrdhistorydetail;
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
			DataGridHistoryDetail.RootTable.Columns["fAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["fAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["fAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDetail.RootTable.Columns["fAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDetail.RootTable.Columns["fStockQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["fStockQty"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["fStockQty"].FormatString = "#,##0.###";
			DataGridHistoryDetail.RootTable.Columns["fStockQty"].TotalFormatString = "#,##0.###";
			DataGridHistoryDetail.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			DataGridHistoryDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistoryDetail.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(DataGridHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateAmount()
	{
	}

	private void txtDetailAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
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
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtValidityDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)txtValidityDays.Text) > 0)
			{
				int Days = Conversion.ToInt((object)txtValidityDays.Text);
				txtvalidUpTo.Value = Conversion.ToDateTime((object)txtDocDate.Value.AddDays(Days));
			}
			else
			{
				txtValidityDays.Text = 0.ToString();
				txtvalidUpTo.Value = txtDocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryCtrlGrdBar_Load(object sender, EventArgs e)
	{
		try
		{
			GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdDetail, dtdetail);
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
			GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, DataGridHistory, dtGridHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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

	private void txtValidityDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDetailAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtStockQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtInterestRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtApprovalAmount_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_0060: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = LoanAgreementHeader.LoanAgreementSlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId,
				DocumentTypeId = DocumentTypeId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "DocumentTypeId-Loan Facility Agreement_Slip.rpt");
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
					((Control)(object)DataGridHistory).Focus();
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc No");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Loan Security Type in Detail Box");
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdDetail.CurrentColumn.Key == "Edit")
				{
					grdDetail_DoubleClick(null, null);
				}
				if (grdDetail.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
					grdDetail.UpdateData();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
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
			int CurrId = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					ReadById(CurrId);
				}
				if (DataGridHistory.CurrentColumn.Key == "Print" && formright.DoHaveUpdateRights)
				{
					GeneratePrint(CurrId);
				}
				if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, DocumentTypeId);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				ReadById(CurrId);
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
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
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
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Expected O, but got Unknown
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Expected O, but got Unknown
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Expected O, but got Unknown
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_0b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b63: Expected O, but got Unknown
		//IL_47ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_47f6: Expected O, but got Unknown
		//IL_481a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4824: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Credit_Financial_Management.LoanFacilityAgreement));
		GridEXLayout DataGridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnplus = new System.Windows.Forms.Button();
		this.cmbColetraltype = new UltraCombo();
		this.cmbLoanSecurityType = new UltraCombo();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.label17 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtStockQty = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtDetailAmount = new System.Windows.Forms.TextBox();
		this.txtLoanSecurityDescription = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.cmbFacilityType = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbInterestRateType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbLoanType = new UltraCombo();
		this.cmbLoanAccount = new UltraCombo();
		this.txtAgreementCode = new System.Windows.Forms.TextBox();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.txtvalidUpTo = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtInterestRate = new System.Windows.Forms.TextBox();
		this.txtApprovalAmount = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.txtValidityDays = new System.Windows.Forms.TextBox();
		this.panel10 = new System.Windows.Forms.Panel();
		this.chkPreview = new System.Windows.Forms.CheckBox();
		this.label15 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachments = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortCut = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistoryDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.DataGridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label8 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
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
		this.HistoryCtrlGrdBar = new grid.User_Conrtols.CtrlGrdBar();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbColetraltype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanSecurityType).BeginInit();
		this.panel1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFacilityType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInterestRateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanAccount).BeginInit();
		this.panel10.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
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
		this.tabControl1.Size = new System.Drawing.Size(983, 661);
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
		this.tabPage1.Size = new System.Drawing.Size(975, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(2, 306);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(971, 324);
		this.panel3.TabIndex = 75;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 25);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(971, 299);
		this.panel7.TabIndex = 74;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(971, 299);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(971, 25);
		this.panel6.TabIndex = 73;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(915, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(51, 25);
		this.ctrlGrdBar1.TabIndex = 2;
		this.ctrlGrdBar1.Load += new System.EventHandler(HistoryCtrlGrdBar_Load);
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 191);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(971, 115);
		this.panel2.TabIndex = 74;
		this.groupBox1.Controls.Add(this.btnplus);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbColetraltype);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoanSecurityType);
		this.groupBox1.Controls.Add(this.btnCancel);
		this.groupBox1.Controls.Add(this.btnGridUpdate);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.txtStockQty);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtDetailAmount);
		this.groupBox1.Controls.Add(this.txtLoanSecurityDescription);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(963, 112);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(807, 80);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(40, 29);
		this.btnplus.TabIndex = 6;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnAddinGrid_Click);
		this.btnplus.Leave += new System.EventHandler(btnplus_Leave);
		this.cmbColetraltype.AllowNull = (DefaultableBoolean)1;
		this.cmbColetraltype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbColetraltype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbColetraltype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbColetraltype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbColetraltype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance76).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbColetraltype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbColetraltype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbColetraltype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbColetraltype).Location = new System.Drawing.Point(267, 35);
		((System.Windows.Forms.Control)(object)this.cmbColetraltype).Name = "cmbColetraltype";
		((System.Windows.Forms.Control)(object)this.cmbColetraltype).Size = new System.Drawing.Size(256, 26);
		((System.Windows.Forms.Control)(object)this.cmbColetraltype).TabIndex = 1;
		this.cmbLoanSecurityType.AllowNull = (DefaultableBoolean)1;
		this.cmbLoanSecurityType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoanSecurityType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoanSecurityType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoanSecurityType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoanSecurityType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbLoanSecurityType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoanSecurityType).Location = new System.Drawing.Point(8, 35);
		((System.Windows.Forms.Control)(object)this.cmbLoanSecurityType).Name = "cmbLoanSecurityType";
		((System.Windows.Forms.Control)(object)this.cmbLoanSecurityType).Size = new System.Drawing.Size(256, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoanSecurityType).TabIndex = 0;
		this.btnCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.ForeColor = System.Drawing.Color.White;
		this.btnCancel.Location = new System.Drawing.Point(884, 81);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(69, 27);
		this.btnCancel.TabIndex = 8;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnCancel.Leave += new System.EventHandler(btnplus_Leave);
		this.btnGridUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGridUpdate.ForeColor = System.Drawing.Color.White;
		this.btnGridUpdate.Location = new System.Drawing.Point(807, 81);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(74, 27);
		this.btnGridUpdate.TabIndex = 7;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = false;
		this.btnGridUpdate.Visible = false;
		this.btnGridUpdate.Click += new System.EventHandler(btnGridUpdate_Click);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(794, 18);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(30, 15);
		this.label17.TabIndex = 76;
		this.label17.Text = "Rate";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRate.Location = new System.Drawing.Point(793, 37);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(114, 23);
		this.txtRate.TabIndex = 4;
		this.txtRate.Text = "0";
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(670, 18);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(59, 15);
		this.label16.TabIndex = 74;
		this.label16.Text = "Stock Qty";
		this.txtStockQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStockQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtStockQty.Location = new System.Drawing.Point(668, 37);
		this.txtStockQty.Name = "txtStockQty";
		this.txtStockQty.Size = new System.Drawing.Size(122, 23);
		this.txtStockQty.TabIndex = 3;
		this.txtStockQty.Text = "0";
		this.txtStockQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtStockQty.TextChanged += new System.EventHandler(txtStockQty_TextChanged);
		this.txtStockQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtStockQty_KeyPress);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(264, 18);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(84, 15);
		this.label19.TabIndex = 72;
		this.label19.Text = "Collateral Type";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(527, 18);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 72;
		this.label14.Text = "Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(8, 18);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(107, 15);
		this.label11.TabIndex = 72;
		this.label11.Text = "Loan Security Type";
		this.txtDetailAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDetailAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDetailAmount.Location = new System.Drawing.Point(526, 37);
		this.txtDetailAmount.Name = "txtDetailAmount";
		this.txtDetailAmount.Size = new System.Drawing.Size(139, 23);
		this.txtDetailAmount.TabIndex = 2;
		this.txtDetailAmount.Text = "0";
		this.txtDetailAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDetailAmount.TextChanged += new System.EventHandler(txtDetailAmount_TextChanged);
		this.txtDetailAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDetailAmount_KeyPress);
		this.txtLoanSecurityDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLoanSecurityDescription.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLoanSecurityDescription.Location = new System.Drawing.Point(8, 83);
		this.txtLoanSecurityDescription.Name = "txtLoanSecurityDescription";
		this.txtLoanSecurityDescription.Size = new System.Drawing.Size(782, 23);
		this.txtLoanSecurityDescription.TabIndex = 5;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(5, 64);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(143, 15);
		this.label12.TabIndex = 62;
		this.label12.Text = "Loan Security Description";
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(2, 56);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(971, 135);
		this.panel1.TabIndex = 73;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFacilityType);
		this.groupBox2.Controls.Add(this.txtremarks);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInterestRateType);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoanType);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoanAccount);
		this.groupBox2.Controls.Add(this.txtAgreementCode);
		this.groupBox2.Controls.Add(this.txtDocDate);
		this.groupBox2.Controls.Add(this.txtvalidUpTo);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtInterestRate);
		this.groupBox2.Controls.Add(this.txtApprovalAmount);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.txtDocNo);
		this.groupBox2.Controls.Add(this.txtValidityDays);
		this.groupBox2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(4, 2);
		this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
		this.groupBox2.Size = new System.Drawing.Size(963, 130);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.cmbFacilityType.AllowNull = (DefaultableBoolean)1;
		this.cmbFacilityType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFacilityType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFacilityType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFacilityType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbFacilityType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFacilityType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFacilityType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbFacilityType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFacilityType).Location = new System.Drawing.Point(101, 74);
		((System.Windows.Forms.Control)(object)this.cmbFacilityType).Name = "cmbFacilityType";
		((System.Windows.Forms.Control)(object)this.cmbFacilityType).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.cmbFacilityType).TabIndex = 3;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(101, 102);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(848, 23);
		this.txtremarks.TabIndex = 4;
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(6, 106);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(52, 15);
		this.label10.TabIndex = 70;
		this.label10.Text = "Remarks";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbInterestRateType.AllowNull = (DefaultableBoolean)1;
		this.cmbInterestRateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInterestRateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInterestRateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInterestRateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbInterestRateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInterestRateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInterestRateType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbInterestRateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInterestRateType).Location = new System.Drawing.Point(781, 45);
		((System.Windows.Forms.Control)(object)this.cmbInterestRateType).Name = "cmbInterestRateType";
		((System.Windows.Forms.Control)(object)this.cmbInterestRateType).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.cmbInterestRateType).TabIndex = 10;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 51);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(81, 15);
		this.label1.TabIndex = 54;
		this.label1.Text = "Loan Account";
		this.cmbLoanType.AllowNull = (DefaultableBoolean)1;
		this.cmbLoanType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoanType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoanType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance78).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoanType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoanType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLoanType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoanType).Location = new System.Drawing.Point(456, 17);
		((System.Windows.Forms.Control)(object)this.cmbLoanType).Name = "cmbLoanType";
		((System.Windows.Forms.Control)(object)this.cmbLoanType).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoanType).TabIndex = 5;
		this.cmbLoanAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbLoanAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoanAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoanAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoanAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoanAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoanAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLoanAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoanAccount).Location = new System.Drawing.Point(101, 45);
		((System.Windows.Forms.Control)(object)this.cmbLoanAccount).Name = "cmbLoanAccount";
		((System.Windows.Forms.Control)(object)this.cmbLoanAccount).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoanAccount).TabIndex = 2;
		this.txtAgreementCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAgreementCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAgreementCode.Location = new System.Drawing.Point(456, 47);
		this.txtAgreementCode.Name = "txtAgreementCode";
		this.txtAgreementCode.Size = new System.Drawing.Size(215, 23);
		this.txtAgreementCode.TabIndex = 6;
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDocDate.Location = new System.Drawing.Point(193, 19);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(139, 23);
		this.txtDocDate.TabIndex = 1;
		this.txtvalidUpTo.Enabled = false;
		this.txtvalidUpTo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvalidUpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtvalidUpTo.Location = new System.Drawing.Point(556, 76);
		this.txtvalidUpTo.Name = "txtvalidUpTo";
		this.txtvalidUpTo.Size = new System.Drawing.Size(115, 23);
		this.txtvalidUpTo.TabIndex = 8;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 80);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(72, 15);
		this.label2.TabIndex = 56;
		this.label2.Text = "Facility Type";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(337, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(61, 15);
		this.label4.TabIndex = 58;
		this.label4.Text = "Loan Type";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(675, 51);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(75, 15);
		this.label21.TabIndex = 68;
		this.label21.Text = "Interest Type";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(675, 23);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(73, 15);
		this.label9.TabIndex = 68;
		this.label9.Text = "Interest Rate";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(337, 51);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(96, 15);
		this.label5.TabIndex = 60;
		this.label5.Text = "Agreement Code";
		this.txtInterestRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInterestRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInterestRate.Location = new System.Drawing.Point(781, 19);
		this.txtInterestRate.Name = "txtInterestRate";
		this.txtInterestRate.Size = new System.Drawing.Size(168, 23);
		this.txtInterestRate.TabIndex = 9;
		this.txtInterestRate.Text = "0";
		this.txtInterestRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInterestRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtInterestRate_KeyPress);
		this.txtApprovalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovalAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtApprovalAmount.Location = new System.Drawing.Point(781, 76);
		this.txtApprovalAmount.Name = "txtApprovalAmount";
		this.txtApprovalAmount.Size = new System.Drawing.Size(168, 23);
		this.txtApprovalAmount.TabIndex = 11;
		this.txtApprovalAmount.Text = "0";
		this.txtApprovalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtApprovalAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtApprovalAmount_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(675, 80);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(102, 15);
		this.label6.TabIndex = 62;
		this.label6.Text = "Approval Amount";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(6, 23);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(88, 15);
		this.label20.TabIndex = 64;
		this.label20.Text = "Doc No && Date";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(337, 80);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(115, 15);
		this.label7.TabIndex = 64;
		this.label7.Text = "Validity Days && Date";
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Enabled = false;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocNo.Location = new System.Drawing.Point(101, 19);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(90, 23);
		this.txtDocNo.TabIndex = 0;
		this.txtDocNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtValidityDays_KeyPress);
		this.txtValidityDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtValidityDays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtValidityDays.Location = new System.Drawing.Point(456, 76);
		this.txtValidityDays.Name = "txtValidityDays";
		this.txtValidityDays.Size = new System.Drawing.Size(98, 23);
		this.txtValidityDays.TabIndex = 7;
		this.txtValidityDays.TextChanged += new System.EventHandler(txtValidityDays_TextChanged);
		this.txtValidityDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtValidityDays_KeyPress);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.chkPreview);
		this.panel10.Controls.Add(this.label15);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 29);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(971, 27);
		this.panel10.TabIndex = 72;
		this.chkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPreview.AutoSize = true;
		this.chkPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPreview.ForeColor = System.Drawing.Color.White;
		this.chkPreview.Location = new System.Drawing.Point(884, 3);
		this.chkPreview.Name = "chkPreview";
		this.chkPreview.Size = new System.Drawing.Size(84, 20);
		this.chkPreview.TabIndex = 12;
		this.chkPreview.Text = "Preview";
		this.chkPreview.UseVisualStyleBackColor = true;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(5, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(207, 19);
		this.label15.TabIndex = 1;
		this.label15.Text = "Loan Facility Agreement";
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.toolStripSeparator3, this.btnnew, this.btnRefresh, this.toolStripSeparator2, this.btnsave, this.btnUpdate, this.btnDelete, this.toolStripSeparator4, this.btnAttachments, this.toolStripSeparator5,
			this.BtnPrint, this.toolStripSeparator, this.btnShortCut
		});
		this.toolStrip1.Location = new System.Drawing.Point(2, 2);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(971, 27);
		this.toolStrip1.TabIndex = 4;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
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
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnAttachments.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachments.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachments.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachments.Name = "btnAttachments";
		this.btnAttachments.Size = new System.Drawing.Size(116, 24);
		this.btnAttachments.Text = "Attachments";
		this.btnAttachments.Click += new System.EventHandler(btnAttachments_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.BtnPrint.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(107, 24);
		this.BtnPrint.Text = "1800-&Print";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnShortCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCut.Name = "btnShortCut";
		this.btnShortCut.Size = new System.Drawing.Size(126, 24);
		this.btnShortCut.Text = "ShortCut Keys";
		this.btnShortCut.Click += new System.EventHandler(btnShortCut_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(975, 632);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Location = new System.Drawing.Point(2, 457);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Name = "DataGridHistoryDetail";
		((ScrollableBase)this.DataGridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Size = new System.Drawing.Size(971, 173);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).TabIndex = 1;
		this.DataGridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label13);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 434);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(971, 23);
		this.panel4.TabIndex = 13;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(4, 4);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(211, 17);
		this.label13.TabIndex = 1;
		this.label13.Text = "Detail Of Above Selected Row";
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(2, 133);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(971, 301);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Controls.Add(this.label8);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(2, 108);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(971, 25);
		this.panel9.TabIndex = 11;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(924, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 23);
		this.ctrlGrdBar2.TabIndex = 6;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(4, 4);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(40, 17);
		this.label8.TabIndex = 1;
		this.label8.Text = "Main";
		this.panel8.Controls.Add(this.groupBox4);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 53);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(971, 55);
		this.panel8.TabIndex = 10;
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
		this.groupBox4.Location = new System.Drawing.Point(3, 4);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(962, 46);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(895, 15);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(602, 20);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(81, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Loan Account";
		this.CmbLoanAccountHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbLoanAccountHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoanAccountHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoanAccountHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoanAccountHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoanAccountHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLoanAccountHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoanAccountHistory).Location = new System.Drawing.Point(689, 14);
		((System.Windows.Forms.Control)(object)this.CmbLoanAccountHistory).Name = "CmbLoanAccountHistory";
		((System.Windows.Forms.Control)(object)this.CmbLoanAccountHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoanAccountHistory).TabIndex = 4;
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(235, 16);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtToDateHistory.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(184, 20);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(48, 15);
		this.label23.TabIndex = 171;
		this.label23.Text = "Date To";
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(5, 20);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(63, 15);
		this.label76.TabIndex = 171;
		this.label76.Text = "Date From";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(528, 16);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(72, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(69, 16);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtFromdateHistory.TabIndex = 0;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(483, 20);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(45, 15);
		this.label22.TabIndex = 173;
		this.label22.Text = "Doc To";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(348, 20);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(60, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "Doc From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(409, 16);
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
		this.panel5.Size = new System.Drawing.Size(971, 51);
		this.panel5.TabIndex = 6;
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.Teal;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label18.Location = new System.Drawing.Point(6, 30);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(270, 19);
		this.label18.TabIndex = 5;
		this.label18.Text = "Loan Facility Agreement History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.toolStripSeparator1 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(971, 27);
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
		this.HistoryCtrlGrdBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.HistoryCtrlGrdBar.FormName = this;
		this.HistoryCtrlGrdBar.Location = new System.Drawing.Point(1168, 3);
		this.HistoryCtrlGrdBar.Margin = new System.Windows.Forms.Padding(4);
		this.HistoryCtrlGrdBar.MyGrid = this.grdDetail;
		this.HistoryCtrlGrdBar.Name = "HistoryCtrlGrdBar";
		this.HistoryCtrlGrdBar.Size = new System.Drawing.Size(45, 26);
		this.HistoryCtrlGrdBar.TabIndex = 21;
		this.HistoryCtrlGrdBar.Load += new System.EventHandler(HistoryCtrlGrdBar_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(983, 661);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "LoanFacilityAgreement";
		this.Text = "LoanFacilityAgreement";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(LoanFacilityAgreement_FormClosed);
		base.Load += new System.EventHandler(LoanFacilityAgreement_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoanFacilityAgreement_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbColetraltype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanSecurityType).EndInit();
		this.panel1.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFacilityType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInterestRateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoanAccount).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel8.ResumeLayout(false);
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
