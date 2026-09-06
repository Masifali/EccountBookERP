using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class FCYVoucherEntry : Form
{
	private int BaseCurrency;

	private double BaseRate;

	private Rightsobjects formright = new Rightsobjects();

	private string saveDirectory;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private bool UpdateMode;

	private int updateRowIndex;

	private int HeaderId;

	public bool _AutoAdjust;

	private int[] AryColWidth;

	private DataTable table = new DataTable();

	private DataTable dtDrAc = new DataTable();

	private DataTable dtCrAc = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private string AttachmentValues = "";

	private string CustomAttachmentValues = "";

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip2;

	private ToolStripButton New;

	private ToolStripButton btnUpdate;

	private ToolStripButton Save;

	private ToolStripButton btnAttachment;

	private Label label23;

	private Panel panel5;

	private Panel panel4;

	private BackgroundWorker backgroundWorker1;

	private TextBox txtdocno;

	private DateTimePicker DocDate;

	private Label label12;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Label LbCr;

	private Label lbDr;

	private Button btnplus;

	internal UltraCombo CmbCreditAccount;

	internal UltraCombo CmbDebitAccount;

	private Label label11;

	private Label label6;

	private TextBox txtCrRmks;

	private TextBox txtDrRmks;

	private Label label10;

	private Label label9;

	private Label label8;

	private Label label7;

	private TextBox txtAmount;

	private TextBox txtCheq;

	private TextBox txtCredit;

	private TextBox txtDebit;

	internal GridEX grd;

	internal GridEX DataGridHistory;

	private ToolStripButton Print;

	private ToolStripButton btnRefresh;

	internal GridEX grdDetail;

	private CheckBox ChkBoxPrintPreview;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private GroupBox groupBox2;

	private GroupBox groupBox1;

	private Label label1;

	private Panel panel11;

	private Panel panel10;

	private Label label2;

	private Label label3;

	private Panel panel12;

	private Label label4;

	private Panel panel14;

	private Panel panel9;

	private Panel panel8;

	private Panel panel6;

	private ToolStrip toolStrip1;

	private GroupBox groupBox3;

	private RadioButton rdconapproved;

	private RadioButton rdconmodify;

	private RadioButton rdcondoc;

	private RadioButton rdconentry;

	private Button btnShowHistory;

	private Label label25;

	private Label label26;

	private Label label28;

	private Label label29;

	internal UltraCombo cmbApproveStatus;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private GroupBox VoucherInfoBox;

	private Label label36;

	private Label lblUnApprovedVouchers;

	private Label lblTotalVouchers;

	private Label label38;

	private Label lblApprovedVouchers;

	private Label label37;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private TextBox txtExchangeRate;

	private Label lblExchangeRate;

	internal UltraCombo cmbCurrency;

	private Label lblFcyCode;

	private TextBox txtFCYAmount;

	private Label label14;

	private Label label13;

	private TextBox txtvoucherRemarks;

	private Label label15;

	public FCYVoucherEntry(UserAccount user)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void DayBookVoucher_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(930, 550);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			Save.Enabled = formright.DoHaveSaveRight;
			Print.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			dtViewAttachments.Columns.Add("AttchmentPath");
			table.Columns.Add("AccountId");
			table.Columns.Add("AccountTitle");
			table.Columns.Add("AgainstAccountId");
			table.Columns.Add("Remarks");
			table.Columns.Add("CheqNo");
			table.Columns.Add("AmountDr", typeof(double));
			table.Columns.Add("AmountCr", typeof(double));
			table.Columns.Add("DMultiCurrencyId", typeof(int));
			table.Columns.Add("DExchangeCurrencyRate", typeof(double));
			table.Columns.Add("DCurrencyAmount", typeof(double));
			table.Columns.Add("LineId");
			table.Columns.Add("OrderNo", typeof(int));
			table.Columns.Add("Attachments", typeof(int));
			grd.DataSource = table;
			grd.RetrieveStructure();
			gridsetting();
			dtDrAc.Columns.Add("Id");
			dtDrAc.Columns.Add("AccountTitle");
			dtCrAc.Columns.Add("Id");
			dtCrAc.Columns.Add("AccountTitle");
			DebitAccountTitleFill();
			StatusFillForHistory();
			lbDr.Visible = false;
			LbCr.Visible = false;
			txtCredit.Visible = false;
			txtDebit.Visible = false;
			ChkBoxPrintPreview.Checked = true;
			DocDate.Focus();
			Save.Visible = true;
			btnUpdate.Visible = false;
			VoucherCode();
			CurrencyFill();
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList();
			if (list.Count > 0)
			{
				saveDirectory = list[0].ConfigKey.ToString() + "\\";
			}
			else
			{
				MessageBox.Show("Attachment Folder Path not found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationMain()
	{
		if (txtdocno.Text == null || txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("Doc No Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
		{
			MessageBox.Show("Currency Code Field is Required");
			cmbCurrency.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtExchangeRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("ExchangeRate Field is Required");
			txtExchangeRate.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
		{
			MessageBox.Show("Currency Code Field is Required");
			cmbCurrency.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtExchangeRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("ExchangeRate Field is Required");
			txtExchangeRate.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDebitAccount).ActiveRow == null || Conversion.ToInt(CmbDebitAccount.Value) == 0)
		{
			MessageBox.Show("Account Debit Field is Required");
			CmbDebitAccount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCreditAccount).ActiveRow == null || Conversion.ToInt(CmbCreditAccount.Value) == 0)
		{
			MessageBox.Show("Account Credit Field is Required");
			CmbCreditAccount.Focus();
			return false;
		}
		if (txtCrRmks.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Credit Remarks Field is Required");
			txtCrRmks.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFCYAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("FCY Amount Field is Required");
			txtFCYAmount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount Field is Required");
			txtAmount.Focus();
			return false;
		}
		return true;
	}

	private void CurrencyFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyCode", "Currency", false);
				((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void StatusFillForHistory()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Not Apporved");
			dt.Rows.Add(2, "Approved");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, cmbApproveStatus, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbApproveStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbApproveStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DebitAccountTitleFill()
	{
		try
		{
			DataTable CoaList = CommonServices.CoaAllocationGetForComboServiceBind();
			if (CoaList.Rows.Count > 0)
			{
				if (dtDrAc.Columns.Count == 0)
				{
					dtDrAc.Columns.Add("Id");
					dtDrAc.Columns.Add("AccountTitle");
				}
				dtDrAc.Rows.Clear();
				for (int i = 0; i < CoaList.Rows.Count; i++)
				{
					dtDrAc.Rows.Add(CoaList.Rows[i]["Id"], CoaList.Rows[i]["AccountTitle"]);
				}
				if (dtDrAc.Rows.Count > 0)
				{
					DDL.BindDDL(dtDrAc, CmbDebitAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
					DDL.BindDDL(dtDrAc, CmbCreditAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			DataTable dt = VoucherHead.GenerateVoucherCodeByDocumentTypeId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 350
			});
			if (dt != null)
			{
				txtdocno.Text = dt.Rows[0]["VoucherCode"].ToString();
				return;
			}
			throw new Exception("Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDr_Leave(object sender, EventArgs e)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbCreditAccount).ActiveRow != null && ((UltraGridBase)CmbDebitAccount).ActiveRow != null && Conversion.ToInt(CmbCreditAccount.Value) == Conversion.ToInt(CmbDebitAccount.Value))
			{
				((Control)(object)CmbDebitAccount).Text = string.Empty;
				throw new Exception("This Account Is Credit Account");
			}
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = DocDate.Value,
				RefAccountId = Conversion.ToInt(CmbDebitAccount.Value)
			});
			if (detail.Rows.Count > 0)
			{
				txtDebit.Text = Math.Round(Conversion.ToDouble(detail.Rows[0]["Balance"])).ToString("0,0");
				double num = Conversion.ToDouble((object)txtDebit.Text);
				if (num > 0.0)
				{
					lbDr.Text = "Dr";
				}
				else
				{
					lbDr.Text = "Cr";
				}
				if (num == 0.0)
				{
					lbDr.Text = "Nill";
				}
				txtDebit.Visible = true;
				lbDr.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void CmbCr_Leave(object sender, EventArgs e)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbCreditAccount).ActiveRow != null && ((UltraGridBase)CmbDebitAccount).ActiveRow != null && Conversion.ToInt(CmbCreditAccount.Value) == Conversion.ToInt(CmbDebitAccount.Value))
			{
				((Control)(object)CmbCreditAccount).Text = string.Empty;
				throw new Exception("This Account Is Debit Account");
			}
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = DocDate.Value,
				RefAccountId = Conversion.ToInt(CmbCreditAccount.Value)
			});
			if (detail.Rows.Count > 0)
			{
				txtCredit.Text = Math.Round(Conversion.ToDouble(detail.Rows[0]["Balance"])).ToString("0,0");
				double num = Conversion.ToDouble((object)txtCredit.Text);
				if (num > 0.0)
				{
					LbCr.Text = "Dr";
				}
				else
				{
					LbCr.Text = "Cr";
				}
				if (num == 0.0)
				{
					LbCr.Text = "Nill";
				}
				txtCredit.Visible = true;
				LbCr.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentValues = "";
			CustomAttachmentValues = "";
			HeaderId = 0;
			((Control)(object)CmbDebitAccount).Text = string.Empty;
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			((Control)(object)cmbCurrency).Text = string.Empty;
			txtvoucherRemarks.Text = string.Empty;
			txtFCYAmount.Clear();
			txtExchangeRate.Clear();
			txtCrRmks.Clear();
			txtDrRmks.Clear();
			txtAmount.Clear();
			txtCheq.Clear();
			table.Clear();
			Save.Visible = true;
			btnUpdate.Visible = false;
			DocDate.Focus();
			VoucherCode();
			grd.ClearStructure();
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			((Control)(object)cmbCurrency).Enabled = true;
			txtExchangeRate.Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void New_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		DebitAccountTitleFill();
	}

	private void FCYVoucherEntry_FormClosed(object sender, FormClosedEventArgs e)
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

	private void Insert()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		try
		{
			if (grd.RowCount == 0)
			{
				throw new Exception("Grid Fields Required");
			}
			if (!FormValidationMain())
			{
				return;
			}
			VoucherHead vh = new VoucherHead();
			if (HeaderId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				vh.Id = HeaderId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToDouble(r3.Cells["AmountDr"].Value) > 0.0 && Conversion.ToInt(r3.Cells["AccountId"].Value) == 0)
				{
					MessageBox.Show("Please Select Debit Account Title First");
					return;
				}
			}
			rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToDouble(r2.Cells["AmountCr"].Value) > 0.0 && Conversion.ToInt(r2.Cells["AccountId"].Value) == 0)
				{
					MessageBox.Show("Please Select Credit Account Title First");
					return;
				}
			}
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.DocumentTypeId = 350;
			vh.DocumentTypeSrNo = 0;
			vh.EntryDate = DateTime.Now;
			vh.EntryUser = UserAccount.ID;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = UserAccount.ID;
			vh.VoucherCode = Conversion.ToInt((object)txtdocno.Text.Trim());
			vh.VoucherDate = DocDate.Value;
			vh.MultiCurrencyId = Conversion.ToInt(cmbCurrency.Value);
			vh.ExchangeCurrencyRate = Conversion.ToDouble((object)txtExchangeRate.Text);
			vh.Remarks = txtvoucherRemarks.Text.Trim().ToString();
			vh.RemarksOtherLingo = "";
			vh.ChequeNo = txtCheq.Text.Trim().ToString();
			vh.voucherDetailList = new List<VoucherDetail>();
			vh.AttachmentsList = new List<DMSAttachments>();
			double DebitAmount = 0.0;
			double CreditAmount = 0.0;
			double DCurrencyAmount = 0.0;
			rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				VoucherDetail vd = new VoucherDetail();
				vd.AccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
				vd.AccountTitle = Conversion.ToString(r.Cells["AccountTitle"].Value);
				vd.AgainstAccountId = Conversion.ToInt(r.Cells["AgainstAccountId"].Value);
				vd.Comments = Conversion.ToString(r.Cells["Remarks"].Value);
				vd.CheqNoDetail = Conversion.ToString(r.Cells["CheqNo"].Value);
				vd.DebitAmount = Conversion.ToDouble(r.Cells["AmountDr"].Value);
				DebitAmount += vd.DebitAmount;
				vd.CreditAmount = Conversion.ToDouble(r.Cells["AmountCr"].Value);
				CreditAmount += vd.CreditAmount;
				vd.LineId = Conversion.ToInt(r.Cells["LineId"].Value);
				vd.OrderNo = Conversion.ToInt(r.Cells["OrderNo"].Value);
				vd.DCurrencyAmount = Conversion.ToDouble(r.Cells["DCurrencyAmount"].Value);
				if (Conversion.ToDouble(r.Cells["AmountDr"].Value) > 0.0)
				{
					DCurrencyAmount += vd.DCurrencyAmount;
				}
				vd.DExchangeCurrencyRate = Conversion.ToDouble(r.Cells["DExchangeCurrencyRate"].Value);
				vd.DMultiCurrencyId = Conversion.ToInt(r.Cells["DMultiCurrencyId"].Value);
				vh.voucherDetailList.Add(vd);
			}
			if (CreditAmount != DebitAmount)
			{
				MessageBox.Show("Debit & Credit side not equal");
				return;
			}
			vh.VoucherAmount = DebitAmount;
			vh.FcAmount = DCurrencyAmount;
			FormHelper.UpdateAttachmentsForObject<VoucherHead>(vh, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			vh.AttachmentsValues = AttachmentValues;
			vh.CustomAttachmentsValues = CustomAttachmentValues;
			int success = VoucherHead.Save(vh);
			if (HeaderId == 0)
			{
				MessageBox.Show("Record Save Successfully...[" + vh.VoucherCode + "]");
			}
			else
			{
				MessageBox.Show("Record Update Successfully...[" + vh.VoucherCode + "]");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(HeaderId, base.Name, 350, 0);
				}
			}
			if (ChkBoxPrintPreview.Checked)
			{
				CommonServices.FcyJournalVoucherEntry350(success);
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Save_Click(object sender, EventArgs e)
	{
		try
		{
			HeaderId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			VoucherHead voucherHead = new VoucherHead();
			new VoucherDetail();
			HeaderId = Conversion.ToInt((object)ID);
			voucherHead = VoucherHead.GetByID(HeaderId);
			if (voucherHead == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			DocDate.Value = voucherHead.VoucherDate;
			txtCheq.Text = voucherHead.ChequeNo;
			txtdocno.Text = voucherHead.VoucherCode.ToString();
			txtvoucherRemarks.Text = Conversion.ToString((object)voucherHead.Remarks);
			cmbCurrency.Value = Conversion.ToInt((object)voucherHead.MultiCurrencyId);
			txtExchangeRate.Text = Conversion.ToString((object)voucherHead.ExchangeCurrencyRate);
			AttachmentValues = voucherHead.AttachmentsValues;
			CustomAttachmentValues = voucherHead.CustomAttachmentsValues;
			table.Columns.Clear();
			if (table.Columns.Count == 0)
			{
				table.Columns.Add("AccountId");
				table.Columns.Add("AccountTitle");
				table.Columns.Add("AgainstAccountId");
				table.Columns.Add("Remarks");
				table.Columns.Add("CheqNo");
				table.Columns.Add("AmountDr", typeof(double));
				table.Columns.Add("AmountCr", typeof(double));
				table.Columns.Add("DExchangeCurrencyRate", typeof(double));
				table.Columns.Add("DCurrencyAmount", typeof(double));
				table.Columns.Add("DMultiCurrencyId", typeof(int));
				table.Columns.Add("LineId");
				table.Columns.Add("OrderNo");
				table.Columns.Add("Attachments");
			}
			List<DMSAttachments> byID = DMSAttachments.GetByID(HeaderId, base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				TA.CustomFileName = Attachment.UploadedFileCustomName;
				TA.ActionId = 2;
				AT.lst.Add(TA);
			}
			table.Rows.Clear();
			int orderNo = 0;
			for (int i = 0; i < voucherHead.voucherDetailList.Count; i++)
			{
				int count = 0;
				if (voucherHead.voucherDetailList[i].OrderNo == 0 && (i + 2) % 2 == 0)
				{
					orderNo++;
				}
				table.Rows.Add(voucherHead.voucherDetailList[i].AccountId, voucherHead.voucherDetailList[i].AccountTitle, voucherHead.voucherDetailList[i].AgainstAccountId, voucherHead.voucherDetailList[i].Comments, voucherHead.voucherDetailList[i].CheqNoDetail, voucherHead.voucherDetailList[i].DebitAmount, voucherHead.voucherDetailList[i].CreditAmount, voucherHead.voucherDetailList[i].DExchangeCurrencyRate, voucherHead.voucherDetailList[i].DCurrencyAmount, voucherHead.voucherDetailList[i].DMultiCurrencyId, voucherHead.voucherDetailList[i].LineId, (voucherHead.voucherDetailList[i].OrderNo > 0) ? voucherHead.voucherDetailList[i].OrderNo : orderNo, count);
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			gridsetting();
			Save.Visible = false;
			btnUpdate.Visible = true;
			UpdateMode = true;
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
			if (HeaderId == 0)
			{
				throw new Exception("Record Not Update  " + HeaderId);
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			DebitAccountTitleFill();
			int UniqueIdToRecognizeCreditDebitRowInDetail = 1;
			int OrderNo = 1;
			grd.GetRows().Count();
			_ = 0;
			if (table.Rows.Count > 0)
			{
				UniqueIdToRecognizeCreditDebitRowInDetail = Conversion.ToInt((object)grd.GetRows().Max((GridEXRow o) => Conversion.ToInt(o.Cells["LineId"].Value)));
				UniqueIdToRecognizeCreditDebitRowInDetail++;
				OrderNo = Conversion.ToInt((object)grd.GetRows().Max((GridEXRow o) => Conversion.ToInt(o.Cells["OrderNo"].Value)));
				OrderNo++;
			}
			table.Rows.Add(CmbCreditAccount.Value, ((Control)(object)CmbCreditAccount).Text, CmbDebitAccount.Value, txtCrRmks.Text.Trim(), txtCheq.Text.Trim(), 0, txtAmount.Text.Trim(), Conversion.ToInt(cmbCurrency.Value), txtExchangeRate.Text.Trim(), Conversion.ToDouble((object)txtFCYAmount.Text), UniqueIdToRecognizeCreditDebitRowInDetail, OrderNo, 0);
			table.Rows.Add(CmbDebitAccount.Value, ((Control)(object)CmbDebitAccount).Text, CmbCreditAccount.Value, txtDrRmks.Text.Trim(), txtCheq.Text.Trim(), txtAmount.Text.Trim(), 0, Conversion.ToInt(cmbCurrency.Value), txtExchangeRate.Text.Trim(), Conversion.ToDouble((object)txtFCYAmount.Text), UniqueIdToRecognizeCreditDebitRowInDetail + 1, OrderNo, 0);
			grd.DataSource = table;
			grd.RetrieveStructure();
			gridsetting();
			txtDrRmks.Clear();
			txtCrRmks.Clear();
			txtAmount.Clear();
			txtFCYAmount.Clear();
			txtCheq.Clear();
			CmbCreditAccount.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			btnplus.Visible = false;
			btnUpdateDetail.Visible = true;
			btnCancelDetail.Visible = true;
			int UniqueId = Conversion.ToInt(grd.CurrentRow.Cells["LineId"].Value);
			if (UniqueId == 0)
			{
				throw new Exception("Record Not Update because LineId not found");
			}
			if (UniqueId % 2 == 0)
			{
				DebitAccountTitleFill();
				CmbCreditAccount.Value = grd.CurrentRow.Cells["AgainstAccountId"].Text.ToString();
				CmbDebitAccount.Value = grd.CurrentRow.Cells["AccountId"].Text.ToString();
				txtCheq.Text = grd.CurrentRow.Cells["CheqNo"].Text.ToString();
				txtDrRmks.Text = grd.CurrentRow.Cells["Remarks"].Text.ToString();
				txtAmount.Text = grd.CurrentRow.Cells["AmountDr"].Text.ToString();
				txtFCYAmount.Text = grd.CurrentRow.Cells["DCurrencyAmount"].Text.ToString();
				DataRow CrObject = null;
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if (Conversion.ToInt(table.Rows[i]["LineId"]) == UniqueId - 1)
					{
						CrObject = table.Rows[i];
						break;
					}
				}
				if (CrObject != null)
				{
					CmbCreditAccount.Value = CrObject.ItemArray[0];
					CmbDebitAccount.Value = CrObject.ItemArray[2];
					txtCheq.Text = Conversion.ToString(CrObject.ItemArray[4]);
					txtCrRmks.Text = Conversion.ToString(CrObject.ItemArray[3]);
					txtAmount.Text = Conversion.ToString(CrObject.ItemArray[6]);
				}
			}
			else
			{
				CmbCreditAccount.Value = grd.CurrentRow.Cells["AccountId"].Text.ToString();
				CmbDebitAccount.Value = grd.CurrentRow.Cells["AgainstAccountId"].Text.ToString();
				txtCheq.Text = grd.CurrentRow.Cells["CheqNo"].Text.ToString();
				txtCrRmks.Text = grd.CurrentRow.Cells["Remarks"].Text.ToString();
				txtAmount.Text = grd.CurrentRow.Cells["AmountCr"].Text.ToString();
				txtFCYAmount.Text = grd.CurrentRow.Cells["DCurrencyAmount"].Text.ToString();
				DataRow DrObject = null;
				for (int j = 0; j < table.Rows.Count; j++)
				{
					if (Conversion.ToInt(table.Rows[j]["LineId"]) == UniqueId + 1)
					{
						DrObject = table.Rows[j];
						break;
					}
				}
				if (DrObject != null)
				{
					CmbDebitAccount.Value = DrObject.ItemArray[0];
					CmbCreditAccount.Value = DrObject.ItemArray[2];
					txtCheq.Text = Conversion.ToString(DrObject.ItemArray[4]);
					txtDrRmks.Text = Conversion.ToString(DrObject.ItemArray[3]);
					txtAmount.Text = Conversion.ToString(DrObject.ItemArray[5]);
				}
			}
			updateRowIndex = Conversion.ToInt(grd.CurrentRow.Cells["LineId"].Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			if (updateRowIndex > -1)
			{
				bool IfUpdateRowIdIsOfDebitRow = updateRowIndex % 2 == 0;
				object[] debitRowValues = new object[9]
				{
					Conversion.ToInt(CmbDebitAccount.Value),
					((Control)(object)CmbDebitAccount).Text,
					Conversion.ToInt(CmbCreditAccount.Value),
					txtDrRmks.Text,
					txtCheq.Text,
					Conversion.ToDouble((object)txtAmount.Text),
					Conversion.ToDouble((object)0),
					Conversion.ToInt((object)(IfUpdateRowIdIsOfDebitRow ? updateRowIndex : (updateRowIndex + 1))),
					Conversion.ToDouble((object)txtFCYAmount.Text)
				};
				object[] creditRowValues = new object[9]
				{
					Conversion.ToInt(CmbCreditAccount.Value),
					((Control)(object)CmbCreditAccount).Text,
					Conversion.ToInt(CmbDebitAccount.Value),
					txtCrRmks.Text,
					txtCheq.Text,
					Conversion.ToDouble((object)0),
					Conversion.ToDouble((object)txtAmount.Text),
					Conversion.ToInt((object)((!IfUpdateRowIdIsOfDebitRow) ? updateRowIndex : (updateRowIndex - 1))),
					Conversion.ToDouble((object)txtFCYAmount.Text)
				};
				int updateIndexForDebitRow = 0;
				int updateIndexForCreditRow = 0;
				for (int j = 0; j < table.Rows.Count; j++)
				{
					if (Conversion.ToInt(table.Rows[j]["LineId"]) == Conversion.ToInt(debitRowValues[7]))
					{
						updateIndexForDebitRow = j;
						break;
					}
				}
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if (Conversion.ToInt(table.Rows[i]["LineId"]) == Conversion.ToInt(creditRowValues[7]))
					{
						updateIndexForCreditRow = i;
						break;
					}
				}
				table.Rows[updateIndexForDebitRow]["AccountId"] = debitRowValues[0];
				table.Rows[updateIndexForDebitRow]["AccountTitle"] = debitRowValues[1];
				table.Rows[updateIndexForDebitRow]["AgainstAccountId"] = debitRowValues[2];
				table.Rows[updateIndexForDebitRow]["Remarks"] = debitRowValues[3];
				table.Rows[updateIndexForDebitRow]["CheqNo"] = debitRowValues[4];
				table.Rows[updateIndexForDebitRow]["AmountDr"] = debitRowValues[5];
				table.Rows[updateIndexForDebitRow]["AmountCr"] = debitRowValues[6];
				table.Rows[updateIndexForDebitRow]["LineId"] = debitRowValues[7];
				table.Rows[updateIndexForDebitRow]["DCurrencyAmount"] = debitRowValues[8];
				table.Rows[updateIndexForCreditRow]["AccountId"] = creditRowValues[0];
				table.Rows[updateIndexForCreditRow]["AccountTitle"] = creditRowValues[1];
				table.Rows[updateIndexForCreditRow]["AgainstAccountId"] = creditRowValues[2];
				table.Rows[updateIndexForCreditRow]["Remarks"] = creditRowValues[3];
				table.Rows[updateIndexForCreditRow]["CheqNo"] = creditRowValues[4];
				table.Rows[updateIndexForCreditRow]["AmountDr"] = creditRowValues[5];
				table.Rows[updateIndexForCreditRow]["AmountCr"] = creditRowValues[6];
				table.Rows[updateIndexForCreditRow]["LineId"] = creditRowValues[7];
				table.Rows[updateIndexForCreditRow]["DCurrencyAmount"] = creditRowValues[8];
				updateRowIndex = -1;
			}
			((Control)(object)CmbDebitAccount).Text = string.Empty;
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			txtCrRmks.Clear();
			txtDrRmks.Clear();
			txtAmount.Clear();
			txtCheq.Clear();
			LbCr.Text = "";
			lbDr.Text = "";
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			((Control)(object)CmbDebitAccount).Text = string.Empty;
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			txtCrRmks.Clear();
			txtDrRmks.Clear();
			txtAmount.Clear();
			txtCheq.Clear();
			LbCr.Text = "";
			lbDr.Text = "";
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
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || !(e.Column.Key == "Delete"))
			{
				return;
			}
			int UniqueId = Conversion.ToInt(item.Cells["LineId"].Value);
			if (UniqueId % 2 == 0)
			{
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if (Conversion.ToInt(table.Rows[i]["LineId"]) == UniqueId - 1)
					{
						table.Rows[i].Delete();
						break;
					}
				}
				item.Delete();
				return;
			}
			for (int j = 0; j < table.Rows.Count; j++)
			{
				if (Conversion.ToInt(table.Rows[j]["LineId"]) == UniqueId + 1)
				{
					table.Rows[j].Delete();
					break;
				}
			}
			item.Delete();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		try
		{
			grd.AllowEdit = (InheritableBoolean)2;
			grd.AllowDelete = (InheritableBoolean)2;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["LineId"].Visible = false;
			grd.RootTable.Columns["OrderNo"].Visible = false;
			grd.RootTable.Columns["Attachments"].Visible = false;
			grd.RootTable.Columns["Attachments"].Width = 80;
			grd.RootTable.Columns["AgainstAccountId"].Visible = false;
			grd.RootTable.Columns["AgainstAccountId"].Width = 350;
			grd.RootTable.Columns["AccountTitle"].Width = 350;
			grd.RootTable.Columns["Remarks"].Width = 400;
			grd.RootTable.Columns["AmountDr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AmountDr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AmountDr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["AmountDr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["AmountCr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AmountCr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AmountCr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["AmountCr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DExchangeCurrencyRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DExchangeCurrencyRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DExchangeCurrencyRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DExchangeCurrencyRate"].Caption = "FCY Rate";
			grd.RootTable.Columns["DExchangeCurrencyRate"].Visible = false;
			grd.RootTable.Columns["DCurrencyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DCurrencyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DCurrencyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DCurrencyAmount"].Caption = "FCY Amount";
			grd.RootTable.Columns["DMultiCurrencyId"].Visible = false;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Caption = "Delete";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "Delete";
			grd.RootTable.Columns["Delete"].Width = 50;
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				_ = e.Column.Key == "Attachments";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			vh.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.Ids = "350";
			vh.PostState = formright.DoHaveCanViewAllRecordRights;
			if (!vh.PostState)
			{
				vh.EntryUser = UserAccount.ID;
			}
			if (rdcondoc.Checked)
			{
				if (FromDateHistory.Checked)
				{
					vh.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					vh.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdconentry.Checked)
			{
				if (FromDateHistory.Checked)
				{
					vh.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					vh.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdconmodify.Checked)
			{
				if (FromDateHistory.Checked)
				{
					vh.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					vh.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdconapproved.Checked)
			{
				if (FromDateHistory.Checked)
				{
					vh.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					vh.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			vh.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			vh.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			if (((Control)(object)cmbApproveStatus).Text == "Approved")
			{
				vh.IsApproved = true;
			}
			if (((Control)(object)cmbApproveStatus).Text == "Not Apporved")
			{
				vh.IsApproved = false;
			}
			if (((Control)(object)cmbApproveStatus).Text == "All")
			{
				vh.ApprovedFilter = "All";
			}
			DataTable lst = VoucherHead.VoucherFormHistory(vh);
			if (lst.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id");
				dtHistory.Columns.Add("VoucherCode", typeof(int));
				dtHistory.Columns.Add("DocumentTypeId");
				dtHistory.Columns.Add("DocumentTypeCode");
				dtHistory.Columns.Add("VoucherDate", typeof(DateTime));
				dtHistory.Columns.Add("ManualBillNo");
				dtHistory.Columns.Add("AccountTitle");
				dtHistory.Columns.Add("Remarks");
				dtHistory.Columns.Add("CurrencyCode");
				dtHistory.Columns.Add("VoucherAmount", typeof(double));
				dtHistory.Columns.Add("MultiCurrencyId", typeof(int));
				dtHistory.Columns.Add("ExchangeCurrencyRate", typeof(double));
				dtHistory.Columns.Add("FcAmount", typeof(double));
				dtHistory.Columns.Add("EntryUser");
				dtHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtHistory.Columns.Add("ModifyUser");
				dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtHistory.Columns.Add("ApprovedUser");
				dtHistory.Columns.Add("ApprovedDate", typeof(DateTime));
				dtHistory.Columns.Add("CheqNo");
				dtHistory.Columns.Add("Attachment");
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtHistory.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["VoucherCode"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["DocumentTypeCode"], Conversion.ToDateTime(lst.Rows[i]["VoucherDate"]).ToShortDateString(), lst.Rows[i]["ManualBillNo"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["Remarks"], lst.Rows[i]["CurrencyCode"], lst.Rows[i]["VoucherAmount"], lst.Rows[i]["MultiCurrencyId"], lst.Rows[i]["ExchangeCurrencyRate"], lst.Rows[i]["FcAmount"], lst.Rows[i]["UserName"], lst.Rows[i]["EntryDate"], lst.Rows[i]["ModifyUserName"], lst.Rows[i]["ModifyDate"], lst.Rows[i]["ApprovedUserName"], lst.Rows[i]["PostDate"], lst.Rows[i]["ChequeNo"], lst.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtHistory;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
				VoucherInfoBox.Visible = true;
				lblTotalVouchers.Text = Conversion.ToInt(lst.Rows[0]["TotalVouchers"]).ToString();
				lblApprovedVouchers.Text = Conversion.ToInt(lst.Rows[0]["TotalApprovedVoucher"]).ToString();
				lblUnApprovedVouchers.Text = Conversion.ToInt(lst.Rows[0]["TotalUnApprovedVoucher"]).ToString();
			}
			else
			{
				VoucherInfoBox.Visible = false;
				DataGridHistory.ClearStructure();
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			DataGridHistory.RootTable.Columns["VoucherCode"].Caption = "V.Code";
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Caption = "V.Type";
			DataGridHistory.RootTable.Columns["VoucherDate"].Caption = "V.Date";
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
			DataGridHistory.RootTable.Columns["AccountTitle"].Visible = false;
			DataGridHistory.RootTable.Columns["CheqNo"].Visible = false;
			DataGridHistory.RootTable.Columns["Remarks"].Visible = false;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns["AccountTitle"].Width = 300;
			DataGridHistory.RootTable.Columns["Remarks"].Width = 150;
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Width = 50;
			DataGridHistory.RootTable.Columns["VoucherAmount"].Width = 120;
			DataGridHistory.RootTable.Columns["VoucherAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["VoucherAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["VoucherAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["VoucherAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ExchangeCurrencyRate"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ExchangeCurrencyRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ExchangeCurrencyRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ExchangeCurrencyRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ExchangeCurrencyRate"].Caption = "FCY Rate";
			DataGridHistory.RootTable.Columns["FcAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["FcAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["MultiCurrencyId"].Visible = false;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridHistory.RootTable.Columns["Attachment"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Position = 2;
			DataGridHistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridDetailsetting()
	{
		try
		{
			grdDetail.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["AccountId"].HasValueList = true;
			grdDetail.RootTable.Columns["AccountId"].LimitToList = true;
			grdDetail.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["AccountId"].Caption = "Account Title";
			grdDetail.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtDrAc.DefaultView, "Id", "AccountTitle");
			grdDetail.RootTable.Columns["AgainstAccountId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["AgainstAccountId"].HasValueList = true;
			grdDetail.RootTable.Columns["AgainstAccountId"].LimitToList = true;
			grdDetail.RootTable.Columns["AgainstAccountId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["AgainstAccountId"].Caption = "Against Account";
			grdDetail.RootTable.Columns["AgainstAccountId"].ValueList.PopulateValueList((IEnumerable)dtDrAc.DefaultView, "Id", "AccountTitle");
			grdDetail.RootTable.Columns["AccountId"].Width = 350;
			grdDetail.RootTable.Columns["AgainstAccountId"].Width = 350;
			grdDetail.RootTable.Columns["Remarks"].Width = 550;
			grdDetail.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["CheqNo"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["AmountDr"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["AmountCr"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["AmountDr"].Caption = "Debit Amount";
			grdDetail.RootTable.Columns["AmountCr"].Caption = "Credit Amount";
			grdDetail.RootTable.Columns["AmountDr"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AmountDr"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AmountDr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["AmountDr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["AmountCr"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AmountCr"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AmountCr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["AmountCr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		HistoryGridFill();
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(item.Cells["Id"].Value), base.Name);
			}
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
			Save.Visible = false;
			btnUpdate.Visible = true;
			ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
	}

	private void DataGridHistory_ColumnButtonClick_1(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Print")
				{
					CommonServices.FcyJournalVoucherEntry350(CurrId);
				}
				if (e.Column.Key == "Edit")
				{
					HeaderId = CurrId;
					ReadById(HeaderId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((UltraGridBase)cmbApproveStatus).Rows[0].Activate();
			VoucherInfoBox.Visible = false;
			DataGridHistory.ClearStructure();
			grdDetail.ClearStructure();
			rdcondoc.Checked = true;
			FromDateHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			VoucherHead voucherHead = VoucherHead.GetByID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
			if (voucherHead != null)
			{
				DataTable table = new DataTable();
				table.Columns.Add("AccountTitle");
				table.Columns.Add("OffSetAccount");
				table.Columns.Add("Remarks");
				table.Columns.Add("CheqNo");
				table.Columns.Add("AmountDr", typeof(double));
				table.Columns.Add("AmountCr", typeof(double));
				table.Columns.Add("DExchangeCurrencyRate", typeof(double));
				table.Columns.Add("DCurrencyAmount", typeof(double));
				table.Rows.Clear();
				for (int i = 0; i < voucherHead.voucherDetailList.Count; i++)
				{
					table.Rows.Add(voucherHead.voucherDetailList[i].AccountTitle, voucherHead.voucherDetailList[i].AgainstAccount, voucherHead.voucherDetailList[i].Comments, voucherHead.voucherDetailList[i].CheqNoDetail, voucherHead.voucherDetailList[i].DebitAmount, voucherHead.voucherDetailList[i].CreditAmount, voucherHead.voucherDetailList[i].DExchangeCurrencyRate, voucherHead.voucherDetailList[i].DCurrencyAmount);
				}
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				grdDetailsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailsetting()
	{
		try
		{
			grdDetail.RootTable.Columns["AmountDr"].Caption = "Debit Amount";
			grdDetail.RootTable.Columns["AmountCr"].Caption = "Credit Amount";
			grdDetail.RootTable.Columns["AmountDr"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AmountDr"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AmountDr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["AmountDr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["AmountCr"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AmountCr"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AmountCr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["AmountCr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["DExchangeCurrencyRate"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DExchangeCurrencyRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DExchangeCurrencyRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["DExchangeCurrencyRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["DExchangeCurrencyRate"].Caption = "FCY Rate";
			grdDetail.RootTable.Columns["DCurrencyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DCurrencyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DCurrencyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["DCurrencyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["DCurrencyAmount"].Caption = "FCY Amount";
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			grdDetail.RootTable.Columns["Remarks"].Width = 300;
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
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

	private void DayBookVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (Save.Visible && Save.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
		{
			Save_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
		{
			New_Click(null, null);
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
		if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
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

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.FcyJournalVoucherEntry350(HeaderId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtCrRmks_Leave(object sender, EventArgs e)
	{
		try
		{
			if (txtCrRmks.Text != string.Empty)
			{
				txtDrRmks.Text = txtCrRmks.Text.Trim();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAmount_Leave(object sender, EventArgs e)
	{
		try
		{
			double a = Conversion.ToDouble((object)txtAmount.Text);
			txtAmount.Text = a.ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFCYAmount_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateLCYAmount();
		}
		catch (Exception)
		{
			MessageBox.Show("Invalid Exchange Rate or FCY Amount");
		}
	}

	public void CalculateLCYAmount()
	{
		try
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null)
			{
				MessageBox.Show("Currency Code Field is Required");
				cmbCurrency.Focus();
			}
			else if (txtExchangeRate.Text == string.Empty)
			{
				MessageBox.Show("Add Exchange Rate Frist");
				txtExchangeRate.Focus();
			}
			else
			{
				txtAmount.Text = Conversion.ToString((object)(Conversion.ToDouble((object)txtExchangeRate.Text) * Conversion.ToDouble((object)txtFCYAmount.Text)));
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void txtExchangeRate_Leave(object sender, EventArgs e)
	{
		try
		{
			if (table.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow item in table.Rows)
			{
				if (Conversion.ToDouble(item["AmountDr"]) > 0.0)
				{
					item["DCurrencyAmount"] = Conversion.ToDouble((object)(Conversion.ToDouble(item["AmountDr"]) * Conversion.ToDouble((object)txtExchangeRate.Text)));
				}
				else
				{
					item["DCurrencyAmount"] = Conversion.ToDouble((object)(Conversion.ToDouble(item["AmountCr"]) * Conversion.ToDouble((object)txtExchangeRate.Text)));
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Error");
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
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Expected O, but got Unknown
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac5: Expected O, but got Unknown
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adc: Expected O, but got Unknown
		//IL_3588: Unknown result type (might be due to invalid IL or missing references)
		//IL_3592: Expected O, but got Unknown
		//IL_35b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_35c0: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtFCYAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbCreditAccount = new UltraCombo();
		this.txtCredit = new System.Windows.Forms.TextBox();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.txtCheq = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.lbDr = new System.Windows.Forms.Label();
		this.txtDrRmks = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.LbCr = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbDebitAccount = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtCrRmks = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtDebit = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtvoucherRemarks = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.lblExchangeRate = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.lblFcyCode = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBoxPrintPreview = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Save = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.grdDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel9 = new System.Windows.Forms.Panel();
		this.VoucherInfoBox = new System.Windows.Forms.GroupBox();
		this.label36 = new System.Windows.Forms.Label();
		this.lblUnApprovedVouchers = new System.Windows.Forms.Label();
		this.lblTotalVouchers = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.lblApprovedVouchers = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.cmbApproveStatus = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.rdconapproved = new System.Windows.Forms.RadioButton();
		this.rdconmodify = new System.Windows.Forms.RadioButton();
		this.rdcondoc = new System.Windows.Forms.RadioButton();
		this.rdconentry = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label25 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel10.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.panel9.SuspendLayout();
		this.VoucherInfoBox.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).BeginInit();
		this.panel8.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(914, 701);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(914, 701);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(906, 672);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel5.Controls.Add(this.panel11);
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(2, 243);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(902, 427);
		this.panel5.TabIndex = 3;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 25);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(902, 402);
		this.panel11.TabIndex = 3;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(902, 402);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Controls.Add(this.label2);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(902, 25);
		this.panel10.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(855, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 22);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(9, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(46, 17);
		this.label2.TabIndex = 1;
		this.label2.Text = "Detail";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 53);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(902, 190);
		this.panel4.TabIndex = 0;
		this.groupBox2.Controls.Add(this.btnplus);
		this.groupBox2.Controls.Add(this.txtFCYAmount);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccount);
		this.groupBox2.Controls.Add(this.txtCredit);
		this.groupBox2.Controls.Add(this.btnCancelDetail);
		this.groupBox2.Controls.Add(this.txtCheq);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.txtAmount);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.lbDr);
		this.groupBox2.Controls.Add(this.txtDrRmks);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.LbCr);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAccount);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtCrRmks);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.txtDebit);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 76);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(888, 109);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(588, 81);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(36, 22);
		this.btnplus.TabIndex = 7;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtFCYAmount.BackColor = System.Drawing.Color.White;
		this.txtFCYAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFCYAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFCYAmount.Location = new System.Drawing.Point(679, 36);
		this.txtFCYAmount.Name = "txtFCYAmount";
		this.txtFCYAmount.Size = new System.Drawing.Size(101, 23);
		this.txtFCYAmount.TabIndex = 3;
		this.txtFCYAmount.Leave += new System.EventHandler(txtFCYAmount_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(685, 18);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(76, 15);
		this.label14.TabIndex = 7;
		this.label14.Text = "FCY-Amount";
		this.CmbCreditAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance22;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCreditAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Location = new System.Drawing.Point(6, 34);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Name = "CmbCreditAccount";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Size = new System.Drawing.Size(274, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Leave += new System.EventHandler(CmbCr_Leave);
		this.txtCredit.BackColor = System.Drawing.Color.White;
		this.txtCredit.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtCredit.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtCredit.Location = new System.Drawing.Point(154, 17);
		this.txtCredit.Name = "txtCredit";
		this.txtCredit.ReadOnly = true;
		this.txtCredit.Size = new System.Drawing.Size(130, 16);
		this.txtCredit.TabIndex = 2;
		this.txtCredit.TabStop = false;
		this.txtCredit.Text = "0";
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(660, 80);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(70, 24);
		this.btnCancelDetail.TabIndex = 9;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.txtCheq.BackColor = System.Drawing.Color.White;
		this.txtCheq.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCheq.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCheq.Location = new System.Drawing.Point(588, 36);
		this.txtCheq.Name = "txtCheq";
		this.txtCheq.Size = new System.Drawing.Size(90, 23);
		this.txtCheq.TabIndex = 2;
		this.txtCheq.UseWaitCursor = true;
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(588, 80);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(71, 24);
		this.btnUpdateDetail.TabIndex = 8;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(781, 36);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(101, 23);
		this.txtAmount.TabIndex = 4;
		this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.txtAmount.Leave += new System.EventHandler(txtAmount_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(593, 18);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(62, 15);
		this.label8.TabIndex = 5;
		this.label8.Text = "Cheq / Ref";
		this.lbDr.AutoSize = true;
		this.lbDr.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbDr.Location = new System.Drawing.Point(119, 64);
		this.lbDr.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lbDr.Name = "lbDr";
		this.lbDr.Size = new System.Drawing.Size(17, 15);
		this.lbDr.TabIndex = 13;
		this.lbDr.Text = "Bl";
		this.txtDrRmks.BackColor = System.Drawing.Color.White;
		this.txtDrRmks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDrRmks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDrRmks.Location = new System.Drawing.Point(283, 81);
		this.txtDrRmks.Name = "txtDrRmks";
		this.txtDrRmks.Size = new System.Drawing.Size(304, 23);
		this.txtDrRmks.TabIndex = 6;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(283, 64);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(81, 15);
		this.label7.TabIndex = 15;
		this.label7.Text = "Dr Comments";
		this.LbCr.AutoSize = true;
		this.LbCr.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.LbCr.Location = new System.Drawing.Point(120, 18);
		this.LbCr.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.LbCr.Name = "LbCr";
		this.LbCr.Size = new System.Drawing.Size(17, 15);
		this.LbCr.TabIndex = 37;
		this.LbCr.Text = "Bl";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(787, 18);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(76, 15);
		this.label9.TabIndex = 9;
		this.label9.Text = "LCY-Amount";
		this.CmbDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Location = new System.Drawing.Point(6, 79);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Name = "CmbDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Size = new System.Drawing.Size(274, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Leave += new System.EventHandler(CmbDr_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(6, 64);
		this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(84, 15);
		this.label6.TabIndex = 11;
		this.label6.Text = "Debit Account";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(6, 18);
		this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(86, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Credit Account";
		this.txtCrRmks.BackColor = System.Drawing.Color.White;
		this.txtCrRmks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCrRmks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCrRmks.Location = new System.Drawing.Point(283, 36);
		this.txtCrRmks.Name = "txtCrRmks";
		this.txtCrRmks.Size = new System.Drawing.Size(304, 23);
		this.txtCrRmks.TabIndex = 1;
		this.txtCrRmks.Leave += new System.EventHandler(txtCrRmks_Leave);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(283, 18);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(79, 15);
		this.label10.TabIndex = 3;
		this.label10.Text = "Cr Comments";
		this.txtDebit.BackColor = System.Drawing.Color.White;
		this.txtDebit.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtDebit.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDebit.Location = new System.Drawing.Point(150, 63);
		this.txtDebit.Name = "txtDebit";
		this.txtDebit.ReadOnly = true;
		this.txtDebit.Size = new System.Drawing.Size(130, 16);
		this.txtDebit.TabIndex = 36;
		this.txtDebit.TabStop = false;
		this.txtDebit.Text = "0";
		this.groupBox1.Controls.Add(this.txtvoucherRemarks);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.lblExchangeRate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox1.Controls.Add(this.lblFcyCode);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(888, 73);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtvoucherRemarks.BackColor = System.Drawing.Color.White;
		this.txtvoucherRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvoucherRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvoucherRemarks.Location = new System.Drawing.Point(521, 14);
		this.txtvoucherRemarks.Multiline = true;
		this.txtvoucherRemarks.Name = "txtvoucherRemarks";
		this.txtvoucherRemarks.Size = new System.Drawing.Size(361, 51);
		this.txtvoucherRemarks.TabIndex = 4;
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(285, 42);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(179, 23);
		this.txtExchangeRate.TabIndex = 3;
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.lblExchangeRate.AutoSize = true;
		this.lblExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblExchangeRate.Location = new System.Drawing.Point(201, 46);
		this.lblExchangeRate.Name = "lblExchangeRate";
		this.lblExchangeRate.Size = new System.Drawing.Size(84, 15);
		this.lblExchangeRate.TabIndex = 8;
		this.lblExchangeRate.Text = "Exchange Rate";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance41).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(285, 14);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 2;
		this.lblFcyCode.AutoSize = true;
		this.lblFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyCode.Location = new System.Drawing.Point(201, 20);
		this.lblFcyCode.Name = "lblFcyCode";
		this.lblFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblFcyCode.TabIndex = 6;
		this.lblFcyCode.Text = "Fcy Code";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(469, 20);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 4;
		this.label1.Text = "Remarks";
		this.DocDate.CustomFormat = "dd-MMM-yyyy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(59, 42);
		this.DocDate.Margin = new System.Windows.Forms.Padding(2);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(137, 23);
		this.DocDate.TabIndex = 1;
		this.txtdocno.BackColor = System.Drawing.SystemColors.Menu;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(59, 16);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(137, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(4, 20);
		this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 2;
		this.label13.Text = "Doc No";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(4, 46);
		this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(57, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Doc Date";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBoxPrintPreview);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 29);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(902, 24);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(176, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "FCY Journal Voucher";
		this.ChkBoxPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxPrintPreview.AutoSize = true;
		this.ChkBoxPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkBoxPrintPreview.Location = new System.Drawing.Point(816, 2);
		this.ChkBoxPrintPreview.Name = "ChkBoxPrintPreview";
		this.ChkBoxPrintPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkBoxPrintPreview.TabIndex = 3;
		this.ChkBoxPrintPreview.Text = "Preview";
		this.ChkBoxPrintPreview.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(902, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.New, this.btnRefresh, this.btnUpdate, this.Save, this.btnAttachment, this.Print });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(902, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.New.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.New.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.New.Name = "New";
		this.New.Size = new System.Drawing.Size(59, 24);
		this.New.Text = "&New";
		this.New.Click += new System.EventHandler(New_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.Save.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Save.Name = "Save";
		this.Save.Size = new System.Drawing.Size(64, 24);
		this.Save.Text = "&Save";
		this.Save.Click += new System.EventHandler(Save_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "350-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage2.Controls.Add(this.panel14);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(906, 672);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(2, 147);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(902, 269);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick_1);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label4);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel12.Location = new System.Drawing.Point(2, 416);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(902, 19);
		this.panel12.TabIndex = 53;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(6, 1);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(182, 16);
		this.label4.TabIndex = 1;
		this.label4.Text = "Detail Above Selected Row";
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(2, 435);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(902, 235);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 1;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar2);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(2, 125);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(902, 22);
		this.panel14.TabIndex = 52;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(857, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 23);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel9.Controls.Add(this.VoucherInfoBox);
		this.panel9.Controls.Add(this.groupBox3);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(2, 50);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(902, 75);
		this.panel9.TabIndex = 51;
		this.VoucherInfoBox.Controls.Add(this.label36);
		this.VoucherInfoBox.Controls.Add(this.lblUnApprovedVouchers);
		this.VoucherInfoBox.Controls.Add(this.lblTotalVouchers);
		this.VoucherInfoBox.Controls.Add(this.label38);
		this.VoucherInfoBox.Controls.Add(this.lblApprovedVouchers);
		this.VoucherInfoBox.Controls.Add(this.label37);
		this.VoucherInfoBox.Location = new System.Drawing.Point(699, 3);
		this.VoucherInfoBox.Name = "VoucherInfoBox";
		this.VoucherInfoBox.Size = new System.Drawing.Size(199, 69);
		this.VoucherInfoBox.TabIndex = 1;
		this.VoucherInfoBox.TabStop = false;
		this.VoucherInfoBox.Visible = false;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(2, 11);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(85, 15);
		this.label36.TabIndex = 1;
		this.label36.Text = "Total Vouchers";
		this.lblUnApprovedVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblUnApprovedVouchers.Location = new System.Drawing.Point(129, 47);
		this.lblUnApprovedVouchers.Name = "lblUnApprovedVouchers";
		this.lblUnApprovedVouchers.Size = new System.Drawing.Size(69, 17);
		this.lblUnApprovedVouchers.TabIndex = 6;
		this.lblUnApprovedVouchers.Text = "0";
		this.lblUnApprovedVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTotalVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalVouchers.Location = new System.Drawing.Point(129, 11);
		this.lblTotalVouchers.Name = "lblTotalVouchers";
		this.lblTotalVouchers.Size = new System.Drawing.Size(69, 17);
		this.lblTotalVouchers.TabIndex = 4;
		this.lblTotalVouchers.Text = "0";
		this.lblTotalVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(2, 47);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(126, 15);
		this.label38.TabIndex = 3;
		this.label38.Text = "UnApproved Vouchers";
		this.lblApprovedVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovedVouchers.Location = new System.Drawing.Point(129, 28);
		this.lblApprovedVouchers.Name = "lblApprovedVouchers";
		this.lblApprovedVouchers.Size = new System.Drawing.Size(69, 17);
		this.lblApprovedVouchers.TabIndex = 5;
		this.lblApprovedVouchers.Text = "0";
		this.lblApprovedVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(2, 28);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(111, 15);
		this.label37.TabIndex = 2;
		this.label37.Text = "Approved Vouchers";
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApproveStatus);
		this.groupBox3.Controls.Add(this.label15);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Controls.Add(this.rdconapproved);
		this.groupBox3.Controls.Add(this.rdconmodify);
		this.groupBox3.Controls.Add(this.rdcondoc);
		this.groupBox3.Controls.Add(this.rdconentry);
		this.groupBox3.Controls.Add(this.btnShowHistory);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add(this.label28);
		this.groupBox3.Controls.Add(this.label29);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(696, 69);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.cmbApproveStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbApproveStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApproveStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApproveStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbApproveStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Location = new System.Drawing.Point(409, 12);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Name = "cmbApproveStatus";
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Size = new System.Drawing.Size(114, 26);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).TabIndex = 4;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.label15.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label15.Location = new System.Drawing.Point(347, 18);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(64, 15);
		this.label15.TabIndex = 143;
		this.label15.Text = "        Status";
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(265, 42);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(79, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(265, 14);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(79, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(66, 42);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(136, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(66, 14);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(136, 23);
		this.FromDateHistory.TabIndex = 0;
		this.rdconapproved.AutoSize = true;
		this.rdconapproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdconapproved.Location = new System.Drawing.Point(599, 38);
		this.rdconapproved.Name = "rdconapproved";
		this.rdconapproved.Size = new System.Drawing.Size(98, 19);
		this.rdconapproved.TabIndex = 9;
		this.rdconapproved.TabStop = true;
		this.rdconapproved.Text = "Approve Date";
		this.rdconapproved.UseVisualStyleBackColor = true;
		this.rdconmodify.AutoSize = true;
		this.rdconmodify.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdconmodify.Location = new System.Drawing.Point(599, 16);
		this.rdconmodify.Name = "rdconmodify";
		this.rdconmodify.Size = new System.Drawing.Size(91, 19);
		this.rdconmodify.TabIndex = 8;
		this.rdconmodify.TabStop = true;
		this.rdconmodify.Text = "Modify Date";
		this.rdconmodify.UseVisualStyleBackColor = true;
		this.rdcondoc.AutoSize = true;
		this.rdcondoc.Checked = true;
		this.rdcondoc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdcondoc.Location = new System.Drawing.Point(525, 16);
		this.rdcondoc.Name = "rdcondoc";
		this.rdcondoc.Size = new System.Drawing.Size(75, 19);
		this.rdcondoc.TabIndex = 6;
		this.rdcondoc.TabStop = true;
		this.rdcondoc.Text = "Doc Date";
		this.rdcondoc.UseVisualStyleBackColor = true;
		this.rdconentry.AutoSize = true;
		this.rdconentry.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdconentry.Location = new System.Drawing.Point(525, 38);
		this.rdconentry.Name = "rdconentry";
		this.rdconentry.Size = new System.Drawing.Size(80, 19);
		this.rdconentry.TabIndex = 7;
		this.rdconentry.TabStop = true;
		this.rdconentry.Text = "Entry Date";
		this.rdconentry.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatAppearance.BorderSize = 0;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowHistory.Location = new System.Drawing.Point(467, 42);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(56, 23);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(205, 46);
		this.label25.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(45, 15);
		this.label25.TabIndex = 141;
		this.label25.Text = "To Doc";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(205, 18);
		this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(60, 15);
		this.label26.TabIndex = 140;
		this.label26.Text = "From Doc";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(4, 46);
		this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(48, 15);
		this.label28.TabIndex = 139;
		this.label28.Text = "To Date";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(4, 18);
		this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(63, 15);
		this.label29.TabIndex = 13;
		this.label29.Text = "From Date";
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label3);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 27);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(902, 23);
		this.panel8.TabIndex = 50;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(3, 1);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(203, 19);
		this.label3.TabIndex = 1;
		this.label3.Text = "Journal Voucher History";
		this.panel6.Controls.Add(this.toolStrip1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(2, 2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(902, 25);
		this.panel6.TabIndex = 49;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(902, 25);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(64, 22);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Visible = false;
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(914, 701);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "FCYVoucherEntry";
		this.Text = "FCY Journal Voucher";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FCYVoucherEntry_FormClosed);
		base.Load += new System.EventHandler(DayBookVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DayBookVoucher_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.VoucherInfoBox.ResumeLayout(false);
		this.VoucherInfoBox.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
