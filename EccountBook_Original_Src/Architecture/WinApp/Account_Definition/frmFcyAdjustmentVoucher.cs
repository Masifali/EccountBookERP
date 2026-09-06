using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Export;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class frmFcyAdjustmentVoucher : Form
{
	private DataTable DtDetail = new DataTable();

	private DataTable DtDetailHistory = new DataTable();

	public DataTable dtViewAttachments = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private LoginNew log = new LoginNew();

	public bool UpdateMode;

	private Rightsobjects formright = new Rightsobjects();

	private int updateDetailIndex;

	private int RecId;

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel ParentPanel;

	private TabControl FcyAdjusTabControl;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel PanelMainTab1;

	private Panel PanelTab1feilds;

	private Panel PanelTab1ToolStrip;

	private Label label22;

	internal UltraCombo CmbRefDocumentType;

	private Label label19;

	internal GridEX grdDetail;

	private GroupBox groupBox1;

	private Label label15;

	private DateTimePicker txtvoucherdate;

	internal UltraCombo cmbFcyCode;

	private Label label1;

	private Label label2;

	private TextBox txtRemarksHeader;

	private TextBox txtHcyTotalAmount;

	private TextBox txtExchRate;

	private TextBox txtFcyTotalAmount;

	private TextBox txtVoucherNo;

	private GroupBox groupBox2;

	private TextBox txtFcyAmount;

	private TextBox txtRemarksDr;

	private TextBox txtRemarksCr;

	internal UltraCombo CmbCreditAc;

	internal UltraCombo CmbTransType;

	private Label label5;

	internal UltraCombo CmbRefDocNo;

	private Label label4;

	internal UltraCombo CmbDebitAc;

	private Label label6;

	private Label label3;

	private TextBox txtHcyAmount;

	private Button BtnCancelDetail;

	private Button BtnUpdateDetail;

	private Button BtnAddDetail;

	private Label label11;

	private Label label10;

	private Label label9;

	private Label label7;

	private Label label8;

	private Panel PanelTab1Grid;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnSave;

	private ToolStripButton BtnUpdate;

	private ToolStripButton BtnAttachment;

	private ToolStripButton BtnRefresh;

	private ToolStripLabel toolStripLabel1;

	private CtrlGrdBar ctrlGrdBarDetail;

	private ToolStripButton BtnNew;

	private ToolStripButton BtnPrint;

	private Panel panelHistoryMain;

	private Panel PanelHistoryDetail;

	internal GridEX grdHistoryDetail;

	private Panel PanelHistoryFill;

	internal GridEX grdHistory;

	private Panel PanelHistoryLabel;

	private CtrlGrdBar ctrlGrdBarHistory;

	private Label label12;

	public CheckBox chkPreview;

	private ToolStripButton btnPrint176;

	public frmFcyAdjustmentVoucher(UserAccount puserAccount)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void frmFcyAdjustmentVoucher_Load(object sender, EventArgs e)
	{
		try
		{
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			BtnSave.Enabled = formright.DoHaveSaveRight;
			BtnPrint.Enabled = formright.DoHavePrintRights;
			BtnUpdate.Enabled = formright.DoHaveUpdateRights;
			ParentPanel.HorizontalScroll.Visible = true;
			ParentPanel.HorizontalScroll.Enabled = true;
			PanelMainTab1.AutoScroll = true;
			PanelMainTab1.HorizontalScroll.Visible = true;
			PanelMainTab1.HorizontalScroll.Enabled = true;
			AccountTitleFill();
			CurrencyCodeFill();
			GenerateDocumentNoFill();
			RefDocumentTypeFill();
			TransactionReasonFill();
			DtDetail.Columns.Add("RefDocumentTypeId");
			DtDetail.Columns.Add("RefDocumentType");
			DtDetail.Columns.Add("RefDocumentNoId");
			DtDetail.Columns.Add("RefDocumentNo");
			DtDetail.Columns.Add("TransactionTypeId");
			DtDetail.Columns.Add("TransactionType");
			DtDetail.Columns.Add("CreditAccountId");
			DtDetail.Columns.Add("CreditAccount");
			DtDetail.Columns.Add("RemarksCredit");
			DtDetail.Columns.Add("DebitAccountId");
			DtDetail.Columns.Add("DebitAccount");
			DtDetail.Columns.Add("RemarksDebit");
			DtDetail.Columns.Add("FcyAmount", typeof(double));
			DtDetail.Columns.Add("HcyAmount", typeof(double));
			DtDetailHistory.Columns.Add("RefDocumentTypeId");
			DtDetailHistory.Columns.Add("RefDocumentType");
			DtDetailHistory.Columns.Add("RefDocumentNoId");
			DtDetailHistory.Columns.Add("RefDocumentNo");
			DtDetailHistory.Columns.Add("TransactionTypeId");
			DtDetailHistory.Columns.Add("TransactionType");
			DtDetailHistory.Columns.Add("CreditAccountId");
			DtDetailHistory.Columns.Add("CreditAccount");
			DtDetailHistory.Columns.Add("RemarksCredit");
			DtDetailHistory.Columns.Add("DebitAccountId");
			DtDetailHistory.Columns.Add("DebitAccount");
			DtDetailHistory.Columns.Add("RemarksDebit");
			DtDetailHistory.Columns.Add("FcyAmount", typeof(double));
			DtDetailHistory.Columns.Add("HcyAmount", typeof(double));
			dtViewAttachments.Columns.Add("AttchmentPath");
			BtnSave.Visible = true;
			BtnUpdate.Visible = false;
			BtnUpdateDetail.Visible = false;
			BtnCancelDetail.Visible = false;
			chkPreview.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyCodeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtCurrency = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtCurrency.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCurrency, cmbFcyCode, "Id", "CurrencyCode", "Fcy Code", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocumentNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			int Code = FcyAdjustmentVoucher.GenerateCode(new FcyAdjustmentVoucher
			{
				DocumentTypeId = 173,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (Code > 0)
			{
				txtVoucherNo.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefDocumentTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DocumentType");
			dt.Rows.Add("204", "Commercial Invoice");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbRefDocumentType, "Id", "DocumentType", "Document Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransactionReasonFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("TransactionType");
			dt.Rows.Add("1", "Discount Allowed");
			dt.Rows.Add("2", "Discount Received");
			dt.Rows.Add("3", "Shortage");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbTransType, "Id", "TransactionType", "Transaction Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTitleFill()
	{
		try
		{
			DataTable dtAc = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("22");
			if (dtAc.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtAc, CmbCreditAc, "Id", "AccountTitle", "Account Title", false);
				DDL.BindDDLNew(dtAc, CmbDebitAc, "Id", "AccountTitle", "Account Title", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceNoFill()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		try
		{
			if (((DisposableObjectCollectionBase)((UltraGridBase)CmbRefDocNo).Rows).Count == 0)
			{
				DataTable dtInvoice = ExImInvoice.GetInvoicesAndCustomers(new ExImInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 204,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				});
				if (dtInvoice.Rows.Count > 0)
				{
					DDL.BindDDL(dtInvoice, CmbRefDocNo, "Id", "InvoiceNo", "InvoiceNo", ZeroIndex: false);
					((UltraGridBase)CmbRefDocNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbRefDocNo).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				}
				else
				{
					((Control)(object)CmbRefDocNo).Text = string.Empty;
					((UltraGridBase)CmbRefDocNo).DataSource = null;
				}
			}
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
			if (((Control)(object)CmbRefDocumentType).Text == "Commercial Invoice")
			{
				InvoiceNoFill();
				return;
			}
			((Control)(object)CmbRefDocNo).Text = string.Empty;
			((UltraGridBase)CmbRefDocNo).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNew_Click(object sender, EventArgs e)
	{
		ResetFormDetail();
		ResetFormMain();
	}

	private void ResetFormMain()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			GenerateDocumentNoFill();
			RecId = 0;
			txtvoucherdate.Focus();
			txtvoucherdate.Text = Conversion.ToString((object)DateTime.Now);
			((Control)(object)cmbFcyCode).Text = string.Empty;
			UpdateMode = false;
			txtFcyTotalAmount.Text = "";
			txtHcyTotalAmount.Text = "";
			txtExchRate.Text = "";
			txtRemarksHeader.Text = "";
			BtnSave.Visible = true;
			BtnUpdate.Visible = false;
			DtDetail.Rows.Clear();
			grdDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetFormDetail()
	{
		try
		{
			CmbRefDocumentType.Focus();
			((Control)(object)CmbTransType).Text = string.Empty;
			((Control)(object)CmbCreditAc).Text = string.Empty;
			((Control)(object)CmbDebitAc).Text = string.Empty;
			txtFcyAmount.Text = "";
			txtHcyAmount.Text = "";
			txtRemarksCr.Text = "";
			txtRemarksDr.Text = "";
			BtnUpdateDetail.Visible = false;
			BtnCancelDetail.Visible = false;
			BtnAddDetail.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			FormRefresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormRefresh()
	{
		try
		{
			AccountTitleFill();
			CurrencyCodeFill();
			RefDocumentTypeFill();
			TransactionReasonFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["RefDocumentNoId"].Value) == Conversion.ToInt(CmbRefDocNo.Value) && Conversion.ToInt(r.Cells["TransactionTypeId"].Value) == Conversion.ToInt(CmbTransType.Value) && Conversion.ToInt(r.Cells["CreditAccountId"].Value) == Conversion.ToInt(CmbCreditAc.Value) && Conversion.ToInt(r.Cells["DebitAccountId"].Value) == Conversion.ToInt(CmbDebitAc.Value))
				{
					MessageBox.Show("You Can not Add Same Invoice No,Transaction Type ,Credit Account And Debit Account In Grid");
					return;
				}
			}
			DtDetail.Rows.Add(CmbRefDocumentType.Value, ((Control)(object)CmbRefDocumentType).Text.Trim(), CmbRefDocNo.Value, ((Control)(object)CmbRefDocNo).Text.Trim(), CmbTransType.Value, ((Control)(object)CmbTransType).Text.Trim(), CmbCreditAc.Value, ((Control)(object)CmbCreditAc).Text.Trim(), txtRemarksCr.Text.Trim(), CmbDebitAc.Value, ((Control)(object)CmbDebitAc).Text.Trim(), txtRemarksDr.Text.Trim(), Conversion.ToDouble((object)txtFcyAmount.Text), Conversion.ToDouble((object)txtHcyAmount.Text));
			grdDetail.DataSource = DtDetail;
			grdDetail.RetrieveStructure();
			DetailGridSetting();
			ResetFormDetail();
			CalculateTotalAmounts();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdDetail.RootTable.Columns["TransactionTypeId"].Visible = false;
			grdDetail.RootTable.Columns["RefDocumentNoId"].Visible = false;
			grdDetail.RootTable.Columns["CreditAccountId"].Visible = false;
			grdDetail.RootTable.Columns["DebitAccountId"].Visible = false;
			grdDetail.RootTable.Columns["RefDocumentType"].Width = 120;
			grdDetail.RootTable.Columns["RefDocumentNo"].Width = 120;
			grdDetail.RootTable.Columns["TransactionType"].Width = 150;
			grdDetail.RootTable.Columns["CreditAccount"].Width = 220;
			grdDetail.RootTable.Columns["RemarksCredit"].Width = 220;
			grdDetail.RootTable.Columns["DebitAccount"].Width = 220;
			grdDetail.RootTable.Columns["RemarksDebit"].Width = 220;
			grdDetail.RootTable.Columns["FcyAmount"].Width = 100;
			grdDetail.RootTable.Columns["HcyAmount"].Width = 100;
			grdDetail.RootTable.Columns["FcyAmount"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["HcyAmount"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["HcyAmount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["HcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 20;
			grdDetail.RootTable.Columns.Add("Edit");
			grdDetail.RootTable.Columns["Edit"].Key = "Edit";
			grdDetail.RootTable.Columns["Edit"].Caption = "Edit";
			grdDetail.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdDetail.RootTable.Columns["Edit"].Width = 50;
			ctrlGrdBarDetail_Load(null, null);
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
			DetailUpdate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailUpdate()
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				CmbRefDocumentType.Value = item.Cells["RefDocumentTypeId"].Value.ToString();
				((Control)(object)CmbRefDocNo).Text = item.Cells["RefDocumentNoId"].Value.ToString();
				CmbTransType.Value = item.Cells["TransactionTypeId"].Value.ToString();
				CmbCreditAc.Value = item.Cells["CreditAccountId"].Value.ToString();
				CmbDebitAc.Value = item.Cells["DebitAccountId"].Value.ToString();
				txtRemarksCr.Text = item.Cells["RemarksCredit"].Text.ToString();
				txtRemarksDr.Text = item.Cells["RemarksDebit"].Text.ToString();
				txtFcyAmount.Text = item.Cells["FcyAmount"].Text.ToString();
				txtHcyAmount.Text = item.Cells["HcyAmount"].Text.ToString();
				CmbRefDocumentType.Focus();
				BtnAddDetail.Visible = false;
				BtnUpdateDetail.Visible = true;
				BtnCancelDetail.Visible = true;
				grdDetail.DataSource = DtDetail;
				grdDetail.RetrieveStructure();
				DetailGridSetting();
				CmbRefDocumentType_Leave(null, null);
				CalculateTotalAmounts();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdDetail.CurrentRow;
				DtDetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			if (e.Column.Key == "Edit")
			{
				DetailUpdate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelDetail_Click(object sender, EventArgs e)
	{
		ResetFormDetail();
	}

	private void BtnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			if (Conversion.ToInt(DtDetail.Rows[updateDetailIndex]["RefDocumentNoId"]) != Conversion.ToInt(CmbRefDocNo.Value) || Conversion.ToInt(DtDetail.Rows[updateDetailIndex]["TransactionTypeId"]) != Conversion.ToInt(CmbTransType.Value) || Conversion.ToInt(DtDetail.Rows[updateDetailIndex]["CreditAccountId"]) != Conversion.ToInt(CmbCreditAc.Value) || Conversion.ToInt(DtDetail.Rows[updateDetailIndex]["DebitAccountId"]) != Conversion.ToInt(CmbDebitAc.Value))
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt(r.Cells["RefDocumentNoId"].Value) == Conversion.ToInt(CmbRefDocNo.Value) && Conversion.ToInt(r.Cells["TransactionTypeId"].Value) == Conversion.ToInt(CmbTransType.Value) && Conversion.ToInt(r.Cells["CreditAccountId"].Value) == Conversion.ToInt(CmbCreditAc.Value) && Conversion.ToInt(r.Cells["DebitAccountId"].Value) == Conversion.ToInt(CmbDebitAc.Value))
					{
						MessageBox.Show("You Can not Add Same Invoice No,Transaction Type ,Credit Account And Debit Account In Grid");
						return;
					}
				}
			}
			DtDetail.Rows[updateDetailIndex]["RefDocumentTypeId"] = CmbRefDocumentType.Value;
			DtDetail.Rows[updateDetailIndex]["RefDocumentType"] = ((Control)(object)CmbRefDocumentType).Text;
			DtDetail.Rows[updateDetailIndex]["RefDocumentNoId"] = CmbRefDocNo.Value;
			DtDetail.Rows[updateDetailIndex]["RefDocumentNo"] = ((Control)(object)CmbRefDocNo).Text;
			DtDetail.Rows[updateDetailIndex]["TransactionTypeId"] = CmbTransType.Value;
			DtDetail.Rows[updateDetailIndex]["TransactionType"] = ((Control)(object)CmbTransType).Text;
			DtDetail.Rows[updateDetailIndex]["CreditAccountId"] = CmbCreditAc.Value;
			DtDetail.Rows[updateDetailIndex]["CreditAccount"] = ((Control)(object)CmbCreditAc).Text;
			DtDetail.Rows[updateDetailIndex]["RemarksCredit"] = txtRemarksCr.Text;
			DtDetail.Rows[updateDetailIndex]["DebitAccountId"] = CmbDebitAc.Value;
			DtDetail.Rows[updateDetailIndex]["DebitAccount"] = ((Control)(object)CmbDebitAc).Text;
			DtDetail.Rows[updateDetailIndex]["RemarksDebit"] = txtRemarksDr.Text;
			DtDetail.Rows[updateDetailIndex]["FcyAmount"] = txtFcyAmount.Text;
			DtDetail.Rows[updateDetailIndex]["HcyAmount"] = txtHcyAmount.Text;
			grdDetail.DataSource = DtDetail;
			grdDetail.RetrieveStructure();
			DetailGridSetting();
			ResetFormDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbRefDocumentType).ActiveRow == null)
		{
			MessageBox.Show("Please Select Ref-Document Type...");
			CmbRefDocumentType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRefDocNo).ActiveRow == null)
		{
			MessageBox.Show("Please Select Ref-Document No...");
			CmbRefDocNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbTransType).ActiveRow == null)
		{
			MessageBox.Show("Please Select Transaction Type...");
			CmbTransType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCreditAc).ActiveRow == null)
		{
			MessageBox.Show("Please Select Credit Account....");
			CmbCreditAc.Focus();
			return false;
		}
		if (txtRemarksCr.Text == string.Empty || txtRemarksCr.Text.Trim() == "")
		{
			MessageBox.Show("Please Enter Credit Remarks....");
			txtRemarksCr.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDebitAc).ActiveRow == null)
		{
			MessageBox.Show("Please Select DebitAc Account....");
			CmbDebitAc.Focus();
			return false;
		}
		if (txtRemarksDr.Text == string.Empty || txtRemarksCr.Text.Trim() == "")
		{
			MessageBox.Show("Please Enter Debit Remarks....");
			txtRemarksDr.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFcyAmount.Text) == 0.0)
		{
			MessageBox.Show("Please Enter Fcy Amount...");
			txtFcyAmount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtExchRate.Text) == 0.0)
		{
			MessageBox.Show("Please Enter Exchange Rate... This Feild Is Rquired to Calculate HcyAmount In Detail");
			txtExchRate.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbDebitAc.Value) == Conversion.ToInt(CmbCreditAc.Value))
		{
			MessageBox.Show("Debit And Credit Account Can not be Same.....");
			CmbDebitAc.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationMain()
	{
		if (((UltraGridBase)cmbFcyCode).ActiveRow == null)
		{
			MessageBox.Show("Please Select Fcy Code...");
			cmbFcyCode.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtExchRate.Text) == 0.0)
		{
			MessageBox.Show("Please Enter Exchange Rate");
			txtExchRate.Focus();
			return false;
		}
		return true;
	}

	private void txtFcyAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtExchRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void BtnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Not Found For Update  " + RecId);
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record not found");
			}
			if (!FormValidationMain())
			{
				return;
			}
			FcyAdjustmentVoucher FcAdj = new FcyAdjustmentVoucher();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				FcAdj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			FcAdj.OrganizationId = UserAccount.OrganizationId;
			FcAdj.CompanyId = UserAccount.CompanyId;
			FcAdj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			FcAdj.ProjectId = UserAccount.BranchesId;
			FcAdj.BranchId = UserAccount.BranchesId;
			FcAdj.FcyId = Conversion.ToInt(cmbFcyCode.Value);
			FcAdj.ExchangeRate = Conversion.ToDecimal((object)txtExchRate.Text);
			FcAdj.FcyTotalAmount = Conversion.ToDouble((object)txtFcyTotalAmount.Text);
			FcAdj.HcyTotalAmount = Conversion.ToDouble((object)txtHcyTotalAmount.Text);
			FcAdj.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			FcAdj.ModifyDate = DateTime.Now;
			FcAdj.EnteyDate = DateTime.Now;
			FcAdj.ApprovedDate = DateTime.Now;
			FcAdj.EntryUserId = UserAccount.ID;
			FcAdj.ModifyUserId = UserAccount.ID;
			FcAdj.DocDate = Conversion.ToDateTime((object)txtvoucherdate.Text);
			FcAdj.DocNo = Conversion.ToInt((object)txtVoucherNo.Text);
			FcAdj.DocumentTypeId = 173;
			FcAdj.IsApproved = false;
			FcAdj.FcyAdjustmentVoucherDetailslist = new List<FcyAdjustmentVoucherDetail>();
			FcAdj.AttachmentsList = new List<DMSAttachments>();
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				FcyAdjustmentVoucherDetail FcyAdjustmentVoucherDetailslist = new FcyAdjustmentVoucherDetail();
				if (Conversion.ToDouble((object)FcAdj.ExchangeRate) == 0.0)
				{
					txtExchRate.Focus();
					throw new Exception("ExchangeRate field is required");
				}
				double HcyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value) * Conversion.ToDouble((object)FcAdj.ExchangeRate);
				if (Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value) == 0)
				{
					MessageBox.Show("RefDocumentType Required In Detail Grid");
					return;
				}
				FcyAdjustmentVoucherDetailslist.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
				if (Conversion.ToString(r.Cells["RefDocumentNo"].Value) == string.Empty)
				{
					MessageBox.Show("Ref-Document No Required In Detail Grid");
					return;
				}
				FcyAdjustmentVoucherDetailslist.RefDocNoId = Conversion.ToInt(r.Cells["RefDocumentNoId"].Value);
				FcyAdjustmentVoucherDetailslist.RefDocNo = Conversion.ToString((object)r.Cells["RefDocumentNo"].Text);
				if (Conversion.ToInt(r.Cells["TransactionTypeId"].Value) == 0)
				{
					MessageBox.Show("Transaction Type Required In Detail Grid");
					return;
				}
				FcyAdjustmentVoucherDetailslist.TransactionReasonId = Conversion.ToInt(r.Cells["TransactionTypeId"].Value);
				if (Conversion.ToInt(r.Cells["CreditAccountId"].Value) == 0)
				{
					MessageBox.Show("Credit Account Required In Detail Grid");
					return;
				}
				FcyAdjustmentVoucherDetailslist.CreditAccountId = Conversion.ToInt(r.Cells["CreditAccountId"].Value);
				FcyAdjustmentVoucherDetailslist.CreditAccountTitle = Conversion.ToString((object)r.Cells["CreditAccount"].Text);
				if (Conversion.ToInt(r.Cells["DebitAccountId"].Value) == 0)
				{
					MessageBox.Show("Debit Account Required In Detail Grid");
					return;
				}
				FcyAdjustmentVoucherDetailslist.DebitAccountId = Conversion.ToInt(r.Cells["DebitAccountId"].Value);
				FcyAdjustmentVoucherDetailslist.DebitAccountTitle = Conversion.ToString((object)r.Cells["DebitAccount"].Text);
				if (Conversion.ToDouble(r.Cells["FcyAmount"].Value) == 0.0)
				{
					MessageBox.Show("FcyAmount Required In Detail Grid");
					return;
				}
				FcyAdjustmentVoucherDetailslist.FcyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
				if (Conversion.ToString((object)r.Cells["RemarksCredit"].Text) == string.Empty)
				{
					MessageBox.Show("Remarks Credit Required In Detail Grid");
					return;
				}
				FcyAdjustmentVoucherDetailslist.CreditRemarks = Conversion.ToString(r.Cells["RemarksCredit"].Value);
				if (Conversion.ToString((object)r.Cells["RemarksDebit"].Text) == string.Empty)
				{
					MessageBox.Show("Remarks Debit Required In Detail Grid");
					return;
				}
				FcyAdjustmentVoucherDetailslist.DebitRemarks = Conversion.ToString(r.Cells["RemarksDebit"].Value);
				if (Conversion.ToDouble(r.Cells["HcyAmount"].Value) == 0.0)
				{
					MessageBox.Show("HcyAmount Required In Detail Grid");
					return;
				}
				FcyAdjustmentVoucherDetailslist.HcyAmount = HcyAmount;
				FcyAdjustmentVoucherDetailslist.DetailEntryUserId = UserAccount.ID;
				FcyAdjustmentVoucherDetailslist.DetailEntryDate = DateTime.Now;
				FcAdj.FcyAdjustmentVoucherDetailslist.Add(FcyAdjustmentVoucherDetailslist);
			}
			FcAdj.AttachmentsList = new List<DMSAttachments>();
			FcAdj.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<FcyAdjustmentVoucher>(FcAdj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			FcyAdjustmentVoucher.Save(FcAdj);
			if (RecId > 0)
			{
				MessageBox.Show("Voucher Update Successfully...[" + FcAdj.DocNo + "]");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 173, 0);
				}
			}
			else
			{
				MessageBox.Show("Voucher Save Successfully...[" + FcAdj.DocNo + "]");
			}
			if (chkPreview.Checked)
			{
				GenerateReport(RecId);
			}
			ResetFormDetail();
			ResetFormMain();
			grdDetail.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotalAmounts()
	{
		try
		{
			if (grdDetail.GetRows().Length != 0)
			{
				double TotalFcyAmount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				double TotalHcyAmount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["HcyAmount"], (AggregateFunction)2));
				txtFcyTotalAmount.Text = Conversion.ToString((object)TotalFcyAmount);
				txtHcyTotalAmount.Text = Conversion.ToString((object)TotalHcyAmount);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateHcyAmount()
	{
		try
		{
			double ExchangeRate = Conversion.ToDouble((object)txtExchRate.Text);
			double FcyAmount = Conversion.ToDouble((object)txtFcyAmount.Text);
			txtHcyAmount.Text = Conversion.ToString((object)(ExchangeRate * FcyAmount));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdDetailValueUpdate()
	{
		try
		{
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow obj in rows)
			{
				_ = grdDetail.GetRows().LongLength;
				obj.BeginEdit();
				double HcyAmount = Conversion.ToDouble(obj.Cells["FcyAmount"].Value) * Conversion.ToDouble((object)txtExchRate.Text);
				obj.Cells["HcyAmount"].Value = HcyAmount;
				obj.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFcyAmount_TextChanged(object sender, EventArgs e)
	{
		CalculateHcyAmount();
		CalculateTotalAmounts();
	}

	private void txtExchRate_TextChanged(object sender, EventArgs e)
	{
		GrdDetailValueUpdate();
		CalculateTotalAmounts();
		CalculateHcyAmount();
	}

	private void ctrlGrdBarDetail_Load(object sender, EventArgs e)
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

	private void ctrlGrdBarHistory_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void FcyAdjusTabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (FcyAdjusTabControl.SelectedIndex == 1)
		{
			HistoryFill(50);
		}
	}

	private void HistoryFill(int NoofRecords = 0)
	{
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		try
		{
			grdHistory.ClearStructure();
			DataTable dtHistory = new DataTable();
			dtHistory.Columns.Add("Id");
			dtHistory.Columns.Add("DocumentTypeId");
			dtHistory.Columns.Add("DocumentTypeCode");
			dtHistory.Columns.Add("VoucherCode");
			dtHistory.Columns.Add("VoucherDate");
			dtHistory.Columns.Add("Currency");
			dtHistory.Columns.Add("FcyTotalAmount", typeof(double));
			dtHistory.Columns.Add("HcyTotalAmount", typeof(double));
			dtHistory.Columns.Add("RemarksHeader");
			dtHistory.Columns.Add("Attachments");
			DataTable lst = FcyAdjustmentVoucher.FormHistory(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 173,
				PostState = formright.DoHaveCanViewAllRecordRights,
				NoOfRecords = NoofRecords
			});
			dtHistory.Rows.Clear();
			if (lst.Rows.Count > 0)
			{
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtHistory.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["DocumentTypeCode"], lst.Rows[i]["DocNo"], Conversion.ToDateTime(lst.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["CurrencyCode"], lst.Rows[i]["FcyTotalAmount"], lst.Rows[i]["HcyTotalAmount"], lst.Rows[i]["RemarksHeader"], lst.Rows[i]["NoOfAttachments"]);
				}
			}
			if (dtHistory.Rows.Count > 0)
			{
				grdHistory.DataSource = dtHistory;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			grdHistory.RootTable.Columns["DocumentTypeCode"].Width = 150;
			grdHistory.RootTable.Columns["VoucherCode"].Width = 100;
			grdHistory.RootTable.Columns["VoucherDate"].Width = 100;
			grdHistory.RootTable.Columns["Currency"].Width = 80;
			grdHistory.RootTable.Columns["FcyTotalAmount"].Width = 130;
			grdHistory.RootTable.Columns["HcyTotalAmount"].Width = 130;
			grdHistory.RootTable.Columns["RemarksHeader"].Width = 350;
			grdHistory.RootTable.Columns["Attachments"].Width = 100;
			grdHistory.RootTable.Columns["FcyTotalAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["HcyTotalAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["FcyTotalAmount"].FormatString = "0,0";
			grdHistory.RootTable.Columns["HcyTotalAmount"].FormatString = "0,0";
			grdHistory.RootTable.Columns["FcyTotalAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["HcyTotalAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns.Add("View");
			grdHistory.RootTable.Columns["View"].Caption = "Detail";
			grdHistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["View"].ButtonText = "Detail";
			grdHistory.RootTable.Columns["View"].Width = 50;
			grdHistory.RootTable.Columns.Add("Print");
			grdHistory.RootTable.Columns["Print"].Caption = "Print";
			grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdHistory.RootTable.Columns["Print"].Width = 50;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 50;
			grdHistory.RootTable.Columns["Print"].Position = 0;
			grdHistory.RootTable.Columns["View"].Position = 1;
			grdHistory.RootTable.Columns["Edit"].Position = 2;
			grdHistory.RootTable.Columns["DocumentTypeCode"].Position = 3;
			grdHistory.FrozenColumns = 3;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["Attachments"].ColumnType = (ColumnType)5;
			ctrlGrdBarHistory_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				RecId = int.Parse(grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				GenerateReport(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "View")
			{
				RecId = int.Parse(grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				GrdDetailHistoryFill(RecId);
				DetailGridHistorySetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 173);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = int.Parse(grdHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridHistorySetting()
	{
		try
		{
			grdHistoryDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["TransactionTypeId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["RefDocumentNoId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["CreditAccountId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["DebitAccountId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["RefDocumentType"].Width = 160;
			grdHistoryDetail.RootTable.Columns["RefDocumentNo"].Width = 120;
			grdHistoryDetail.RootTable.Columns["TransactionType"].Width = 150;
			grdHistoryDetail.RootTable.Columns["CreditAccount"].Width = 220;
			grdHistoryDetail.RootTable.Columns["RemarksCredit"].Width = 220;
			grdHistoryDetail.RootTable.Columns["DebitAccount"].Width = 220;
			grdHistoryDetail.RootTable.Columns["RemarksDebit"].Width = 220;
			grdHistoryDetail.RootTable.Columns["FcyAmount"].Width = 100;
			grdHistoryDetail.RootTable.Columns["HcyAmount"].Width = 100;
			grdHistoryDetail.RootTable.Columns["FcyAmount"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["HcyAmount"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["HcyAmount"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["HcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int Id)
	{
		try
		{
			Id = Conversion.ToInt((object)Id);
			FcyAdjustmentVoucher FcAd = FcyAdjustmentVoucher.GetByID(Id);
			if (FcAd != null)
			{
				BtnSave.Visible = false;
				BtnUpdate.Visible = true;
				UpdateMode = true;
				FcyAdjusTabControl.SelectedIndex = 0;
				txtVoucherNo.Text = Conversion.ToString((object)FcAd.DocNo);
				txtvoucherdate.Text = Conversion.ToString((object)FcAd.DocDate);
				cmbFcyCode.Value = FcAd.FcyId;
				txtExchRate.Text = Conversion.ToString((object)FcAd.ExchangeRate);
				txtRemarksHeader.Text = Conversion.ToString((object)FcAd.RemarksHeader);
				txtFcyTotalAmount.Text = Conversion.ToString((object)FcAd.FcyTotalAmount);
				txtHcyTotalAmount.Text = Conversion.ToString((object)FcAd.HcyTotalAmount);
				DtDetail.Rows.Clear();
				for (int i = 0; i < FcAd.FcyAdjustmentVoucherDetailslist.Count; i++)
				{
					DtDetail.Rows.Add(FcAd.FcyAdjustmentVoucherDetailslist[i].RefDocumentTypeId, FcAd.FcyAdjustmentVoucherDetailslist[i].RefDocumentType, FcAd.FcyAdjustmentVoucherDetailslist[i].RefDocNoId, FcAd.FcyAdjustmentVoucherDetailslist[i].RefDocNo, FcAd.FcyAdjustmentVoucherDetailslist[i].TransactionReasonId, FcAd.FcyAdjustmentVoucherDetailslist[i].TransactionType, FcAd.FcyAdjustmentVoucherDetailslist[i].CreditAccountId, FcAd.FcyAdjustmentVoucherDetailslist[i].CreditAccountTitle, FcAd.FcyAdjustmentVoucherDetailslist[i].CreditRemarks, FcAd.FcyAdjustmentVoucherDetailslist[i].DebitAccountId, FcAd.FcyAdjustmentVoucherDetailslist[i].DebitAccountTitle, FcAd.FcyAdjustmentVoucherDetailslist[i].DebitRemarks, FcAd.FcyAdjustmentVoucherDetailslist[i].FcyAmount, FcAd.FcyAdjustmentVoucherDetailslist[i].HcyAmount);
				}
				grdDetail.DataSource = DtDetail;
				grdDetail.RetrieveStructure();
				DetailGridSetting();
				CalculateTotalAmounts();
				List<DMSAttachments> byID = DMSAttachments.GetByID(RecId, base.Name);
				AT = new Attachment();
				{
					foreach (DMSAttachments Attachment in byID)
					{
						TempAttachments TA = new TempAttachments();
						TA.Id = 0;
						TA.ActionId = 2;
						TA.Path = Attachment.Attachment;
						TA.CustomFileName = Attachment.UploadedFileCustomName;
						AT.lst.Add(TA);
					}
					return;
				}
			}
			grdDetail.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdDetailHistoryFill(int Id)
	{
		try
		{
			grdDetail.ClearStructure();
			Id = Conversion.ToInt((object)Id);
			FcyAdjustmentVoucher FcAd = FcyAdjustmentVoucher.GetByID(Id);
			if (FcAd != null)
			{
				DtDetailHistory.Rows.Clear();
				for (int i = 0; i < FcAd.FcyAdjustmentVoucherDetailslist.Count; i++)
				{
					DtDetailHistory.Rows.Add(FcAd.FcyAdjustmentVoucherDetailslist[i].RefDocumentTypeId, FcAd.FcyAdjustmentVoucherDetailslist[i].RefDocumentType, FcAd.FcyAdjustmentVoucherDetailslist[i].RefDocNoId, FcAd.FcyAdjustmentVoucherDetailslist[i].RefDocNo, FcAd.FcyAdjustmentVoucherDetailslist[i].TransactionReasonId, FcAd.FcyAdjustmentVoucherDetailslist[i].TransactionType, FcAd.FcyAdjustmentVoucherDetailslist[i].CreditAccountId, FcAd.FcyAdjustmentVoucherDetailslist[i].CreditAccountTitle, FcAd.FcyAdjustmentVoucherDetailslist[i].CreditRemarks, FcAd.FcyAdjustmentVoucherDetailslist[i].DebitAccountId, FcAd.FcyAdjustmentVoucherDetailslist[i].DebitAccountTitle, FcAd.FcyAdjustmentVoucherDetailslist[i].DebitRemarks, FcAd.FcyAdjustmentVoucherDetailslist[i].FcyAmount, FcAd.FcyAdjustmentVoucherDetailslist[i].HcyAmount);
				}
				grdHistoryDetail.DataSource = DtDetailHistory;
				grdHistoryDetail.RetrieveStructure();
			}
			else
			{
				grdHistoryDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAttachment_Click(object sender, EventArgs e)
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

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(CommonServices.VoucherHeadIdGet(RecId, 173));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = FcyAdjustmentVoucher.FcyAdjustmentSlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 173,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "176-FcyAdjustmentVoucherSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint176_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReport(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTransType_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbDebitAc).Enabled = true;
			((Control)(object)CmbCreditAc).Enabled = true;
			if (((UltraGridBase)CmbRefDocNo).ActiveRow != null)
			{
				if (Conversion.ToInt(CmbTransType.Value) == 2)
				{
					CmbDebitAc.Value = Conversion.ToInt(((UltraDropDownBase)CmbRefDocNo).SelectedRow.Cells[2].Value);
					((Control)(object)CmbDebitAc).Enabled = false;
				}
				else
				{
					((Control)(object)CmbCreditAc).Enabled = false;
					CmbCreditAc.Value = Conversion.ToInt(((UltraDropDownBase)CmbRefDocNo).SelectedRow.Cells[2].Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmFcyAdjustmentVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (BtnSave.Visible && BtnSave.Enabled && e.Control && e.KeyCode == Keys.S && FcyAdjusTabControl.SelectedIndex == 0)
			{
				BtnSave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				BtnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (FcyAdjusTabControl.SelectedIndex == 1)
				{
					FcyAdjusTabControl.SelectedIndex = 0;
				}
				else
				{
					FcyAdjusTabControl.SelectedIndex = 1;
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (BtnUpdate.Visible && BtnUpdate.Enabled && FcyAdjusTabControl.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.U)
			{
				BtnUpdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmFcyAdjustmentVoucher_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Expected O, but got Unknown
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Expected O, but got Unknown
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Expected O, but got Unknown
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Expected O, but got Unknown
		//IL_099f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a9: Expected O, but got Unknown
		//IL_40b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_40c3: Expected O, but got Unknown
		//IL_40d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_40da: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance72 = new Appearance();
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
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
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
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.frmFcyAdjustmentVoucher));
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.ParentPanel = new System.Windows.Forms.Panel();
		this.FcyAdjusTabControl = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.PanelMainTab1 = new System.Windows.Forms.Panel();
		this.PanelTab1Grid = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.PanelTab1feilds = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.BtnCancelDetail = new System.Windows.Forms.Button();
		this.BtnUpdateDetail = new System.Windows.Forms.Button();
		this.BtnAddDetail = new System.Windows.Forms.Button();
		this.txtHcyAmount = new System.Windows.Forms.TextBox();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.txtRemarksDr = new System.Windows.Forms.TextBox();
		this.txtRemarksCr = new System.Windows.Forms.TextBox();
		this.CmbCreditAc = new UltraCombo();
		this.CmbTransType = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbRefDocNo = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbDebitAc = new UltraCombo();
		this.CmbRefDocumentType = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.chkPreview = new System.Windows.Forms.CheckBox();
		this.txtHcyTotalAmount = new System.Windows.Forms.TextBox();
		this.txtExchRate = new System.Windows.Forms.TextBox();
		this.txtFcyTotalAmount = new System.Windows.Forms.TextBox();
		this.txtVoucherNo = new System.Windows.Forms.TextBox();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtvoucherdate = new System.Windows.Forms.DateTimePicker();
		this.cmbFcyCode = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.PanelTab1ToolStrip = new System.Windows.Forms.Panel();
		this.ctrlGrdBarDetail = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnSave = new System.Windows.Forms.ToolStripButton();
		this.BtnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnAttachment = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrint176 = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panelHistoryMain = new System.Windows.Forms.Panel();
		this.PanelHistoryDetail = new System.Windows.Forms.Panel();
		this.grdHistoryDetail = new GridEX();
		this.PanelHistoryFill = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.PanelHistoryLabel = new System.Windows.Forms.Panel();
		this.ctrlGrdBarHistory = new grid.User_Conrtols.CtrlGrdBar();
		this.label12 = new System.Windows.Forms.Label();
		this.ParentPanel.SuspendLayout();
		this.FcyAdjusTabControl.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.PanelMainTab1.SuspendLayout();
		this.PanelTab1Grid.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.PanelTab1feilds.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocumentType).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFcyCode).BeginInit();
		this.PanelTab1ToolStrip.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panelHistoryMain.SuspendLayout();
		this.PanelHistoryDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.PanelHistoryFill.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.PanelHistoryLabel.SuspendLayout();
		base.SuspendLayout();
		this.ParentPanel.AutoScroll = true;
		this.ParentPanel.Controls.Add(this.FcyAdjusTabControl);
		this.ParentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ParentPanel.Location = new System.Drawing.Point(0, 0);
		this.ParentPanel.Name = "ParentPanel";
		this.ParentPanel.Size = new System.Drawing.Size(1115, 699);
		this.ParentPanel.TabIndex = 0;
		this.FcyAdjusTabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.FcyAdjusTabControl.Controls.Add(this.tabPage1);
		this.FcyAdjusTabControl.Controls.Add(this.tabPage2);
		this.FcyAdjusTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.FcyAdjusTabControl.Location = new System.Drawing.Point(0, 0);
		this.FcyAdjusTabControl.Name = "FcyAdjusTabControl";
		this.FcyAdjusTabControl.SelectedIndex = 0;
		this.FcyAdjusTabControl.Size = new System.Drawing.Size(1115, 699);
		this.FcyAdjusTabControl.TabIndex = 0;
		this.FcyAdjusTabControl.SelectedIndexChanged += new System.EventHandler(FcyAdjusTabControl_SelectedIndexChanged);
		this.tabPage1.AutoScroll = true;
		this.tabPage1.Controls.Add(this.PanelMainTab1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1107, 673);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.PanelMainTab1.AutoScroll = true;
		this.PanelMainTab1.Controls.Add(this.PanelTab1Grid);
		this.PanelMainTab1.Controls.Add(this.PanelTab1feilds);
		this.PanelMainTab1.Controls.Add(this.PanelTab1ToolStrip);
		this.PanelMainTab1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelMainTab1.Location = new System.Drawing.Point(3, 3);
		this.PanelMainTab1.Name = "PanelMainTab1";
		this.PanelMainTab1.Size = new System.Drawing.Size(1101, 667);
		this.PanelMainTab1.TabIndex = 0;
		this.PanelTab1Grid.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.PanelTab1Grid.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelTab1Grid.Location = new System.Drawing.Point(0, 281);
		this.PanelTab1Grid.Name = "PanelTab1Grid";
		this.PanelTab1Grid.Size = new System.Drawing.Size(1101, 386);
		this.PanelTab1Grid.TabIndex = 2;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1101, 386);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdDetail).TabStop = false;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.PanelTab1feilds.Controls.Add(this.groupBox2);
		this.PanelTab1feilds.Controls.Add(this.groupBox1);
		this.PanelTab1feilds.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelTab1feilds.Location = new System.Drawing.Point(0, 37);
		this.PanelTab1feilds.Name = "PanelTab1feilds";
		this.PanelTab1feilds.Size = new System.Drawing.Size(1101, 244);
		this.PanelTab1feilds.TabIndex = 1;
		this.groupBox2.Controls.Add(this.BtnCancelDetail);
		this.groupBox2.Controls.Add(this.BtnUpdateDetail);
		this.groupBox2.Controls.Add(this.BtnAddDetail);
		this.groupBox2.Controls.Add(this.txtHcyAmount);
		this.groupBox2.Controls.Add(this.txtFcyAmount);
		this.groupBox2.Controls.Add(this.txtRemarksDr);
		this.groupBox2.Controls.Add(this.txtRemarksCr);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAc);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransType);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefDocNo);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAc);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefDocumentType);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 112);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1083, 127);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.BtnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelDetail.Location = new System.Drawing.Point(1008, 94);
		this.BtnCancelDetail.Name = "BtnCancelDetail";
		this.BtnCancelDetail.Size = new System.Drawing.Size(58, 25);
		this.BtnCancelDetail.TabIndex = 11;
		this.BtnCancelDetail.Text = "Cancel";
		this.BtnCancelDetail.UseVisualStyleBackColor = false;
		this.BtnCancelDetail.Click += new System.EventHandler(BtnCancelDetail_Click);
		this.BtnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateDetail.Location = new System.Drawing.Point(941, 94);
		this.BtnUpdateDetail.Name = "BtnUpdateDetail";
		this.BtnUpdateDetail.Size = new System.Drawing.Size(63, 25);
		this.BtnUpdateDetail.TabIndex = 10;
		this.BtnUpdateDetail.Text = "Update";
		this.BtnUpdateDetail.UseVisualStyleBackColor = false;
		this.BtnUpdateDetail.Click += new System.EventHandler(BtnUpdateDetail_Click);
		this.BtnAddDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnAddDetail.ForeColor = System.Drawing.Color.White;
		this.BtnAddDetail.Location = new System.Drawing.Point(890, 94);
		this.BtnAddDetail.Name = "BtnAddDetail";
		this.BtnAddDetail.Size = new System.Drawing.Size(47, 25);
		this.BtnAddDetail.TabIndex = 9;
		this.BtnAddDetail.Text = "Add";
		this.BtnAddDetail.UseVisualStyleBackColor = false;
		this.BtnAddDetail.Click += new System.EventHandler(BtnAddDetail_Click);
		this.txtHcyAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtHcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHcyAmount.Enabled = false;
		this.txtHcyAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtHcyAmount.Location = new System.Drawing.Point(708, 94);
		this.txtHcyAmount.Name = "txtHcyAmount";
		this.txtHcyAmount.Size = new System.Drawing.Size(178, 25);
		this.txtHcyAmount.TabIndex = 8;
		this.txtHcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(568, 94);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(134, 25);
		this.txtFcyAmount.TabIndex = 7;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmount.TextChanged += new System.EventHandler(txtFcyAmount_TextChanged);
		this.txtFcyAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFcyAmount_KeyPress);
		this.txtRemarksDr.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtRemarksDr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDr.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDr.Location = new System.Drawing.Point(305, 94);
		this.txtRemarksDr.Name = "txtRemarksDr";
		this.txtRemarksDr.Size = new System.Drawing.Size(259, 25);
		this.txtRemarksDr.TabIndex = 6;
		this.txtRemarksCr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksCr.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksCr.Location = new System.Drawing.Point(708, 44);
		this.txtRemarksCr.Name = "txtRemarksCr";
		this.txtRemarksCr.Size = new System.Drawing.Size(358, 25);
		this.txtRemarksCr.TabIndex = 4;
		this.CmbCreditAc.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Location = new System.Drawing.Point(463, 43);
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Name = "CmbCreditAc";
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Size = new System.Drawing.Size(239, 27);
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).TabIndex = 3;
		this.CmbTransType.AllowNull = (DefaultableBoolean)1;
		this.CmbTransType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbTransType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbTransType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbTransType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransType).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransType).Location = new System.Drawing.Point(305, 43);
		((System.Windows.Forms.Control)(object)this.CmbTransType).Name = "CmbTransType";
		((System.Windows.Forms.Control)(object)this.CmbTransType).Size = new System.Drawing.Size(152, 27);
		((System.Windows.Forms.Control)(object)this.CmbTransType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbTransType).Leave += new System.EventHandler(CmbTransType_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(709, 75);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(75, 15);
		this.label11.TabIndex = 16;
		this.label11.Text = "Hcy Amount";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(572, 75);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(72, 15);
		this.label10.TabIndex = 16;
		this.label10.Text = "Fcy Amount";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(307, 75);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(76, 15);
		this.label9.TabIndex = 16;
		this.label9.Text = "Remarks (Dr)";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(709, 23);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(74, 15);
		this.label7.TabIndex = 16;
		this.label7.Text = "Remarks (Cr)";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(464, 23);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(86, 15);
		this.label5.TabIndex = 16;
		this.label5.Text = "Credit Account";
		this.CmbRefDocNo.AllowNull = (DefaultableBoolean)1;
		this.CmbRefDocNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefDocNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefDocNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRefDocNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).Location = new System.Drawing.Point(154, 43);
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).Name = "CmbRefDocNo";
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).Size = new System.Drawing.Size(145, 27);
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).TabIndex = 1;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(307, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(109, 15);
		this.label4.TabIndex = 16;
		this.label4.Text = "Transaction Reason";
		this.CmbDebitAc.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance54).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAc).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAc).Location = new System.Drawing.Point(3, 93);
		((System.Windows.Forms.Control)(object)this.CmbDebitAc).Name = "CmbDebitAc";
		((System.Windows.Forms.Control)(object)this.CmbDebitAc).Size = new System.Drawing.Size(296, 27);
		((System.Windows.Forms.Control)(object)this.CmbDebitAc).TabIndex = 5;
		this.CmbRefDocumentType.AllowNull = (DefaultableBoolean)1;
		this.CmbRefDocumentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefDocumentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefDocumentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance67).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRefDocumentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Location = new System.Drawing.Point(3, 43);
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Name = "CmbRefDocumentType";
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Size = new System.Drawing.Size(145, 27);
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Leave += new System.EventHandler(CmbRefDocumentType_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(-400, 140);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(82, 15);
		this.label8.TabIndex = 16;
		this.label8.Text = "Debit Account";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 72);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(84, 15);
		this.label6.TabIndex = 16;
		this.label6.Text = "Debit Account";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(157, 23);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(103, 15);
		this.label3.TabIndex = 16;
		this.label3.Text = "Ref Document No";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(5, 23);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(112, 15);
		this.label22.TabIndex = 16;
		this.label22.Text = "Ref Document Type";
		this.groupBox1.Controls.Add(this.chkPreview);
		this.groupBox1.Controls.Add(this.txtHcyTotalAmount);
		this.groupBox1.Controls.Add(this.txtExchRate);
		this.groupBox1.Controls.Add(this.txtFcyTotalAmount);
		this.groupBox1.Controls.Add(this.txtVoucherNo);
		this.groupBox1.Controls.Add(this.txtRemarksHeader);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.txtvoucherdate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFcyCode);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1083, 100);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.chkPreview.AutoSize = true;
		this.chkPreview.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPreview.Location = new System.Drawing.Point(1008, 77);
		this.chkPreview.Name = "chkPreview";
		this.chkPreview.Size = new System.Drawing.Size(74, 21);
		this.chkPreview.TabIndex = 1;
		this.chkPreview.Text = "Preview";
		this.chkPreview.UseVisualStyleBackColor = true;
		this.txtHcyTotalAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtHcyTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHcyTotalAmount.Enabled = false;
		this.txtHcyTotalAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtHcyTotalAmount.Location = new System.Drawing.Point(226, 71);
		this.txtHcyTotalAmount.Name = "txtHcyTotalAmount";
		this.txtHcyTotalAmount.Size = new System.Drawing.Size(133, 23);
		this.txtHcyTotalAmount.TabIndex = 29;
		this.txtHcyTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchRate.Location = new System.Drawing.Point(126, 71);
		this.txtExchRate.Name = "txtExchRate";
		this.txtExchRate.Size = new System.Drawing.Size(94, 23);
		this.txtExchRate.TabIndex = 2;
		this.txtExchRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchRate.TextChanged += new System.EventHandler(txtExchRate_TextChanged);
		this.txtExchRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchRate_KeyPress);
		this.txtFcyTotalAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtFcyTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyTotalAmount.Enabled = false;
		this.txtFcyTotalAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyTotalAmount.Location = new System.Drawing.Point(226, 45);
		this.txtFcyTotalAmount.Name = "txtFcyTotalAmount";
		this.txtFcyTotalAmount.Size = new System.Drawing.Size(133, 23);
		this.txtFcyTotalAmount.TabIndex = 27;
		this.txtFcyTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtVoucherNo.BackColor = System.Drawing.SystemColors.Menu;
		this.txtVoucherNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVoucherNo.Enabled = false;
		this.txtVoucherNo.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVoucherNo.Location = new System.Drawing.Point(273, 19);
		this.txtVoucherNo.Name = "txtVoucherNo";
		this.txtVoucherNo.Size = new System.Drawing.Size(86, 23);
		this.txtVoucherNo.TabIndex = 26;
		this.txtVoucherNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(365, 49);
		this.txtRemarksHeader.Multiline = true;
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(639, 49);
		this.txtRemarksHeader.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(4, 75);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(117, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Exch Rate && Amount";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(4, 23);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(110, 15);
		this.label15.TabIndex = 18;
		this.label15.Text = "Voucher Date && No";
		this.txtvoucherdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvoucherdate.CustomFormat = "dd-MMM-yy";
		this.txtvoucherdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvoucherdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtvoucherdate.Location = new System.Drawing.Point(126, 20);
		this.txtvoucherdate.Name = "txtvoucherdate";
		this.txtvoucherdate.Size = new System.Drawing.Size(142, 21);
		this.txtvoucherdate.TabIndex = 0;
		this.cmbFcyCode.AllowNull = (DefaultableBoolean)1;
		this.cmbFcyCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFcyCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFcyCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFcyCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.cmbFcyCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFcyCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFcyCode).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbFcyCode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFcyCode).Location = new System.Drawing.Point(126, 44);
		((System.Windows.Forms.Control)(object)this.cmbFcyCode).Name = "cmbFcyCode";
		((System.Windows.Forms.Control)(object)this.cmbFcyCode).Size = new System.Drawing.Size(94, 24);
		((System.Windows.Forms.Control)(object)this.cmbFcyCode).TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 49);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(114, 15);
		this.label1.TabIndex = 16;
		this.label1.Text = "Fcy Code && Amount";
		this.label19.BackColor = System.Drawing.Color.Gainsboro;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 10.75f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(365, 19);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(639, 23);
		this.label19.TabIndex = 14;
		this.label19.Text = "Remarks";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.PanelTab1ToolStrip.BackColor = System.Drawing.Color.Teal;
		this.PanelTab1ToolStrip.Controls.Add(this.ctrlGrdBarDetail);
		this.PanelTab1ToolStrip.Controls.Add(this.toolStrip1);
		this.PanelTab1ToolStrip.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelTab1ToolStrip.Location = new System.Drawing.Point(0, 0);
		this.PanelTab1ToolStrip.Name = "PanelTab1ToolStrip";
		this.PanelTab1ToolStrip.Size = new System.Drawing.Size(1101, 37);
		this.PanelTab1ToolStrip.TabIndex = 0;
		this.ctrlGrdBarDetail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBarDetail.BackColor = System.Drawing.SystemColors.Control;
		this.ctrlGrdBarDetail.FormName = this;
		this.ctrlGrdBarDetail.Location = new System.Drawing.Point(1048, 3);
		this.ctrlGrdBarDetail.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBarDetail.MyGrid = this.grdDetail;
		this.ctrlGrdBarDetail.Name = "ctrlGrdBarDetail";
		this.ctrlGrdBarDetail.Size = new System.Drawing.Size(45, 30);
		this.ctrlGrdBarDetail.TabIndex = 30;
		this.ctrlGrdBarDetail.Load += new System.EventHandler(ctrlGrdBarDetail_Load);
		this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.toolStripLabel1, this.BtnNew, this.BtnSave, this.BtnUpdate, this.BtnAttachment, this.BtnRefresh, this.BtnPrint, this.btnPrint176 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1101, 37);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.ForeColor = System.Drawing.Color.Black;
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(195, 34);
		this.toolStripLabel1.Text = "Fcy Adjustment Voucher";
		this.BtnNew.BackColor = System.Drawing.Color.Transparent;
		this.BtnNew.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.BtnNew.ForeColor = System.Drawing.Color.Black;
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(51, 34);
		this.BtnNew.Text = "New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.BtnSave.BackColor = System.Drawing.Color.Transparent;
		this.BtnSave.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.BtnSave.ForeColor = System.Drawing.Color.Black;
		this.BtnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSave.Name = "BtnSave";
		this.BtnSave.Size = new System.Drawing.Size(51, 34);
		this.BtnSave.Text = "Save";
		this.BtnSave.Click += new System.EventHandler(BtnSave_Click);
		this.BtnUpdate.BackColor = System.Drawing.Color.Transparent;
		this.BtnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.BtnUpdate.ForeColor = System.Drawing.Color.Black;
		this.BtnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnUpdate.Name = "BtnUpdate";
		this.BtnUpdate.Size = new System.Drawing.Size(65, 34);
		this.BtnUpdate.Text = "Update";
		this.BtnUpdate.Click += new System.EventHandler(BtnUpdate_Click);
		this.BtnAttachment.BackColor = System.Drawing.Color.Transparent;
		this.BtnAttachment.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.BtnAttachment.ForeColor = System.Drawing.Color.Black;
		this.BtnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.BtnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnAttachment.Name = "BtnAttachment";
		this.BtnAttachment.Size = new System.Drawing.Size(90, 34);
		this.BtnAttachment.Text = "Attachment";
		this.BtnAttachment.Click += new System.EventHandler(BtnAttachment_Click);
		this.BtnRefresh.BackColor = System.Drawing.Color.Transparent;
		this.BtnRefresh.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.BtnRefresh.ForeColor = System.Drawing.Color.Black;
		this.BtnRefresh.Image = (System.Drawing.Image)resources.GetObject("BtnRefresh.Image");
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(66, 34);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.BtnPrint.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.BtnPrint.ForeColor = System.Drawing.Color.Black;
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(70, 34);
		this.BtnPrint.Text = "Voucher";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.btnPrint176.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint176.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint176.Name = "btnPrint176";
		this.btnPrint176.Size = new System.Drawing.Size(75, 34);
		this.btnPrint176.Text = "176-Print";
		this.btnPrint176.Click += new System.EventHandler(btnPrint176_Click);
		this.tabPage2.Controls.Add(this.panelHistoryMain);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1107, 673);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panelHistoryMain.AutoScroll = true;
		this.panelHistoryMain.Controls.Add(this.PanelHistoryDetail);
		this.panelHistoryMain.Controls.Add(this.PanelHistoryFill);
		this.panelHistoryMain.Controls.Add(this.PanelHistoryLabel);
		this.panelHistoryMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelHistoryMain.Location = new System.Drawing.Point(3, 3);
		this.panelHistoryMain.Name = "panelHistoryMain";
		this.panelHistoryMain.Size = new System.Drawing.Size(1101, 667);
		this.panelHistoryMain.TabIndex = 0;
		this.PanelHistoryDetail.AutoScroll = true;
		this.PanelHistoryDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.PanelHistoryDetail.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelHistoryDetail.Location = new System.Drawing.Point(0, 403);
		this.PanelHistoryDetail.Name = "PanelHistoryDetail";
		this.PanelHistoryDetail.Size = new System.Drawing.Size(1101, 264);
		this.PanelHistoryDetail.TabIndex = 2;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryDetail).BackColor = System.Drawing.Color.White;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1101, 264);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabStop = false;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.PanelHistoryFill.AutoScroll = true;
		this.PanelHistoryFill.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.PanelHistoryFill.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelHistoryFill.Location = new System.Drawing.Point(0, 41);
		this.PanelHistoryFill.Name = "PanelHistoryFill";
		this.PanelHistoryFill.Size = new System.Drawing.Size(1101, 362);
		this.PanelHistoryFill.TabIndex = 1;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistory).BackColor = System.Drawing.Color.White;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1101, 362);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdHistory).TabStop = false;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		this.PanelHistoryLabel.AutoScroll = true;
		this.PanelHistoryLabel.BackColor = System.Drawing.Color.Teal;
		this.PanelHistoryLabel.Controls.Add(this.ctrlGrdBarHistory);
		this.PanelHistoryLabel.Controls.Add(this.label12);
		this.PanelHistoryLabel.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelHistoryLabel.Location = new System.Drawing.Point(0, 0);
		this.PanelHistoryLabel.Name = "PanelHistoryLabel";
		this.PanelHistoryLabel.Size = new System.Drawing.Size(1101, 41);
		this.PanelHistoryLabel.TabIndex = 0;
		this.ctrlGrdBarHistory.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBarHistory.FormName = this;
		this.ctrlGrdBarHistory.Location = new System.Drawing.Point(1048, 5);
		this.ctrlGrdBarHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBarHistory.MyGrid = this.grdHistory;
		this.ctrlGrdBarHistory.Name = "ctrlGrdBarHistory";
		this.ctrlGrdBarHistory.Size = new System.Drawing.Size(45, 30);
		this.ctrlGrdBarHistory.TabIndex = 31;
		this.ctrlGrdBarHistory.Load += new System.EventHandler(ctrlGrdBarHistory_Load);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(5, 9);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(78, 25);
		this.label12.TabIndex = 0;
		this.label12.Text = "History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1115, 699);
		base.Controls.Add(this.ParentPanel);
		base.KeyPreview = true;
		base.Name = "frmFcyAdjustmentVoucher";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmFcyAdjustmentVoucher_FormClosing);
		base.Load += new System.EventHandler(frmFcyAdjustmentVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmFcyAdjustmentVoucher_KeyDown);
		this.ParentPanel.ResumeLayout(false);
		this.FcyAdjusTabControl.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.PanelMainTab1.ResumeLayout(false);
		this.PanelTab1Grid.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.PanelTab1feilds.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocumentType).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFcyCode).EndInit();
		this.PanelTab1ToolStrip.ResumeLayout(false);
		this.PanelTab1ToolStrip.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panelHistoryMain.ResumeLayout(false);
		this.PanelHistoryDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.PanelHistoryFill.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.PanelHistoryLabel.ResumeLayout(false);
		this.PanelHistoryLabel.PerformLayout();
		base.ResumeLayout(false);
	}
}
