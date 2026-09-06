using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
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
namespace Architecture.WinApp.Export
{

public class ExpfrmFinancialInsturment : Form
{
	private bool DoHaveSaveRight;

	private bool DoHavePrintRights;

	private bool DoHaveUpdateRights;

	private bool DoHaveCanViewAllRecordRights;

	private Attachment AT = new Attachment();

	private double totalpercent;

	private double totalpaymentfcamount;

	private double GetFcBankUtlizedAmount;

	private int AdvanceUtilizationGrid;

	private int RecId;

	private int updateDetailIndex;

	private int updatePaymentDetailIndex;

	private int updateAdvancedPaymentDetailIndex;

	private bool UpdateMode;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtCommodity = new DataTable();

	private DataTable dtPayment = new DataTable();

	private DataTable dtPaymentutilization = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel6;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnNew;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnAttachment;

	private Label label1;

	private GroupBox groupBox1;

	private DateTimePicker txtfidate;

	private TextBox txtfiuniqueNo;

	private Label label4;

	internal UltraCombo cmbBank;

	private Label label2;

	private Label label5;

	private TextBox txtNetWeightkg;

	private Label label11;

	private TextBox txtAmountRs;

	private Label label10;

	private TextBox txtExchangeRate;

	private Label label8;

	private TextBox txtfivalue;

	private Label label9;

	internal UltraCombo cmbconsgneeocuntry;

	private Label label12;

	internal UltraCombo cmbFICode;

	private Label label7;

	internal UltraCombo cmbDeliverymode;

	private Label label6;

	internal UltraCombo cmbConsignee;

	private Label label3;

	private GroupBox groupBox4;

	internal UltraCombo cmbcurrency;

	private TextBox txtUtilizeAmount;

	private Button btnAddPaymentUtilization;

	private TextBox txtThisUtilize;

	private Label label25;

	private TextBox txtFcAmountPaymentUtilization;

	private Label label24;

	private Label label20;

	private Label label23;

	private Label label22;

	private TextBox txtBalance;

	private Label label21;

	private GroupBox groupBox3;

	internal UltraCombo cmbPaymentTerm;

	private Button btnAddPayment;

	private TextBox txtDueDays;

	private Label label19;

	private TextBox txtFcAmountPayment;

	private Label label16;

	private TextBox txtPaymentPercent;

	private Label label18;

	private Label label17;

	private GroupBox groupBox2;

	private Button btnCommodityAdd;

	private TextBox txtFCAmount;

	private Label label15;

	private TextBox txtNetweightDetail;

	private Label label14;

	private TextBox txtCommodity;

	private Label label13;

	private Button btnCancelPaymentUtilization;

	private Button btnUpdatePaymentUtilization;

	private Button btnCancelPayment;

	private Button btnUpdatePayment;

	private Button btnCancelComodity;

	private Button btnUpdateComodity;

	internal GridEX grdCommodity;

	internal GridEX grdPaymentTerm;

	internal GridEX grdPaymentUtilization;

	internal UltraCombo cmbBankReerence;

	internal UltraCombo cmbPortOfDischarge;

	private Label label27;

	internal UltraCombo cmbLoadingPort;

	private Label label26;

	private TextBox txtremarks;

	private Label label28;

	private Panel panel10;

	private Panel panel13;

	private Panel panel12;

	private Panel panel11;

	private ToolStrip toolStrip2;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton6;

	private ToolStripSeparator toolStripSeparator1;

	private Label label37;

	internal GridEX DataGridHistory;

	private Panel panel7;

	private Panel panel16;

	private Panel panel15;

	private Panel panel14;

	private Panel panel8;

	private Label label42;

	private Label label40;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label35;

	private Label label34;

	private Label label33;

	private Label label32;

	private Label label31;

	private Label label30;

	private Label label29;

	private DateTimePicker txtexpirydate;

	private Label label41;

	private TextBox txtconsigneeAddress;

	private Label label47;

	internal UltraCombo cmbpaymentmode;

	private Label label46;

	internal UltraCombo cmbTradeType;

	private Label label45;

	private Label label43;

	private TextBox txtIBAN;

	private Label label44;

	private Label label52;

	private Label label50;

	private TextBox txtBalnaceAmt;

	private Label label51;

	private TextBox txtconsigneeIban;

	private Label label49;

	private CheckBox RBIsActive;

	private CtrlGrdBar ctrlGrdBar1;

	public ExpfrmFinancialInsturment(UserAccount user)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void ExpfrmEformRegistration_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = base.Name,
				RightName = UserAccount.RoleName
			});
			if (lstRights.Count > 0)
			{
				List<tblUserRights> newList = lstRights.Where((tblUserRights x) => x.RightName == "Save").ToList();
				DoHaveSaveRight = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Print").ToList();
				DoHavePrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Update").ToList();
				DoHaveUpdateRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "CanView AllRecord").ToList();
				if (newList.Count > 0)
				{
					DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
				}
			}
			btnSave.Enabled = DoHaveSaveRight;
			btnUpdate.Enabled = DoHaveUpdateRights;
			cmbBank.Focus();
			suppliercustomer();
			PaymentTerm();
			currency();
			Country();
			issueBank();
			bindcmbLoadingPort();
			DeliveryTerm();
			bindReferenceBank();
			BindGridHisory();
			TradeTypeFill();
			dtCommodity.Columns.Add("Commodity");
			dtCommodity.Columns.Add("NetWeight", typeof(double));
			dtCommodity.Columns.Add("FcAmount", typeof(double));
			dtPayment.Columns.Add("PaymentTermId");
			dtPayment.Columns.Add("PaymentTerm");
			dtPayment.Columns.Add("PaymentPercent", typeof(double));
			dtPayment.Columns.Add("FcAmount", typeof(double));
			dtPayment.Columns.Add("DueDays");
			dtPaymentutilization.Columns.Add("BankReferenceId");
			dtPaymentutilization.Columns.Add("BankReference");
			dtPaymentutilization.Columns.Add("CurrencyCodeId");
			dtPaymentutilization.Columns.Add("CurrencyCode");
			dtPaymentutilization.Columns.Add("ThisUtilize", typeof(double));
			dtPaymentutilization.Columns.Add("Remarks");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindReferenceBank()
	{
	}

	private void bindcmbLoadingPort()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SeaPorts.Getall(new SeaPorts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbLoadingPort, "Id", "PortName", "Loading Port", false);
				DDL.BindDDLNew(dt, cmbPortOfDischarge, "Id", "PortName", "Destination Port", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void issueBank()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			List<Bank> lst = Bank.GetAll(new Bank
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("IssueBank");
			for (int i = 0; i < lst.Count; i++)
			{
				if (Convert.ToString(lst[i].IsHomeland) == "Home Country")
				{
					dt.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbBank, "Id", "IssueBank", "Bank ", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void suppliercustomer()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SupplierCustomer.GetCustomerIdByLcOrderAndFcReceipt(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbConsignee, "Id", "CompanyName", "Customer Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCustomer_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		try
		{
			DataTable dtFcBankReceipt = ExImFCBankReceipts.GetDataBySuppliercustomerId(new ExImFCBankReceipts
			{
				SupplierCustomerId = Conversion.ToInt(cmbConsignee.Value)
			});
			if (dtFcBankReceipt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtFcBankReceipt, cmbBankReerence, "Id", "BankFbpno", "BankFbpno", false);
				DDL.BindDDLNew(dtFcBankReceipt, cmbFICode, "MultiCurrencyId", "CurrencyCode", "CurrencyCode", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBankReerence_Leave(object sender, EventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		try
		{
			if (cmbBankReerence != null)
			{
				DataTable dt = ExImFCBankReceipts.GetDataBySuppliercustomerId(new ExImFCBankReceipts
				{
					Id = Conversion.ToInt(cmbBankReerence.Value)
				});
				if (dt.Rows.Count > 0)
				{
					txtFcAmountPaymentUtilization.Text = Conversion.ToDouble(dt.Rows[0]["FcNetAmount"]).ToString();
					txtUtilizeAmount.Text = Conversion.ToDouble(dt.Rows[0]["FcUtilizedAmount"]).ToString();
					double num = Conversion.ToDouble((object)txtFcAmountPaymentUtilization.Text.Trim());
					double UtilizedAmount = Conversion.ToDouble((object)txtUtilizeAmount.Text.Trim());
					txtBalance.Text = (num - UtilizedAmount).ToString();
					DDL.BindDDL(dt, cmbcurrency, "MultiCurrencyId", "CurrencyCode", "CurrencyCode ", ZeroIndex: false);
				}
			}
			GetTotalFcAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTerm()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImLcPaymentTerm.Getall(new ExImLcPaymentTerm
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbPaymentTerm, "Id", "LcOrderTerm", "Payment Term", false);
				DDL.BindDDLNew(dt, cmbpaymentmode, "Id", "LcOrderTerm", "Payment Mode", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TradeTypeFill()
	{
		try
		{
			DataTable dtstatus = new DataTable();
			dtstatus.Columns.Add("Id");
			dtstatus.Columns.Add("TradeType");
			dtstatus.Rows.Add(1, "Import");
			dtstatus.Rows.Add(2, "Export");
			DDL.BindDDL(dtstatus, cmbTradeType, "Id", "TradeType", "TradeType", ZeroIndex: false);
			((UltraGridBase)cmbTradeType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTerm()
	{
		try
		{
			DataTable dt = ExImDeliveryTerm.Getall();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbDeliverymode, "Id", "Description", "Delivery Term", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void currency()
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
				DDL.BindDDLNew(dt, cmbFICode, "Id", "CurrencyCode", "FI Currency", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Country()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = country.GetAll(new Country
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbconsgneeocuntry, "Id", "Description", "Country", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((Control)(object)cmbConsignee).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Customer Field Required");
			cmbConsignee.Focus();
			return false;
		}
		if (((Control)(object)cmbBank).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Issue Bank Field Required");
			cmbBank.Focus();
			return false;
		}
		if (((Control)(object)cmbDeliverymode).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Delivery Term Field Required");
			cmbDeliverymode.Focus();
			return false;
		}
		if (txtfivalue.Text.Trim() == string.Empty)
		{
			MessageBox.Show("ValueFc Field Required");
			txtfivalue.Focus();
			return false;
		}
		return true;
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Expected O, but got Unknown
		if (!FormValidation())
		{
			return;
		}
		try
		{
			ExImEFormRegistration obj = new ExImEFormRegistration();
			if (RecId > 0 && UpdateMode)
			{
				obj.Id = RecId;
			}
			obj.IssuingBankId = Conversion.ToInt(cmbBank.Value);
			obj.IbanNo = Conversion.ToString((object)txtIBAN.Text);
			obj.TradeType = Conversion.ToString((object)((Control)(object)cmbTradeType).Text);
			obj.PaymenttermId = Conversion.ToInt(cmbpaymentmode.Value);
			obj.EFormDate = txtfidate.Value;
			obj.EFormNo = txtfiuniqueNo.Text;
			obj.DocumentTypeId = 207;
			obj.Status = Conversion.ToBool((object)RBIsActive.Checked);
			obj.ConsigneeId = Conversion.ToInt(cmbConsignee.Value);
			obj.ConsigneeIban = Conversion.ToString((object)txtconsigneeIban.Text);
			obj.ConsigneeAddress = Conversion.ToString((object)txtconsigneeAddress.Text);
			obj.CountryId = Conversion.ToInt(cmbconsgneeocuntry.Value);
			obj.DestinationPortId = Conversion.ToInt(cmbPortOfDischarge.Value);
			obj.DeliveryTermId = Conversion.ToInt(cmbDeliverymode.Value);
			obj.FcurrencyId = Conversion.ToInt(cmbFICode.Value);
			obj.EFormValueTotal = Conversion.ToDouble((object)txtfivalue.Text);
			obj.BalanceAmount = Conversion.ToDouble((object)txtBalnaceAmt.Text);
			obj.ExpiryDate = Conversion.ToDateTime((object)txtexpirydate.Text);
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.EFormRegistrationComodityDetail = new List<ExImEFormRegistrationComodityDetail>();
			obj.EFormRegistrationPaymentTerms = new List<ExImEFormRegistrationPaymentTerms>();
			obj.EFormAdvancePaymentUtilizationDetail = new List<ExImEFormRegistrationAdvancePaymentUtilizationDetail>();
			if (grdCommodity.RowCount > 0)
			{
				double num = double.Parse(grdCommodity.GetTotal(grdCommodity.RootTable.Columns["FcAmount"], (AggregateFunction)2).ToString());
				double.Parse(grdCommodity.GetTotal(grdCommodity.RootTable.Columns["NetWeight"], (AggregateFunction)2).ToString());
				double ValueFc = Conversion.ToInt((object)txtfivalue.Text);
				Conversion.ToInt((object)txtNetWeightkg.Text);
				if (num == ValueFc)
				{
					GridEXRow[] rows = grdCommodity.GetRows();
					foreach (GridEXRow r3 in rows)
					{
						ExImEFormRegistrationComodityDetail exImEFormRegistrationComodity = new ExImEFormRegistrationComodityDetail();
						exImEFormRegistrationComodity.CommodityDescription = Conversion.ToString((object)r3.Cells["Commodity"].Text);
						exImEFormRegistrationComodity.FcAmount = Conversion.ToDouble((object)r3.Cells["NetWeight"].Text);
						exImEFormRegistrationComodity.Weight = Conversion.ToDouble((object)r3.Cells["FcAmount"].Text);
						obj.EFormRegistrationComodityDetail.Add(exImEFormRegistrationComodity);
					}
				}
			}
			if (grdPaymentTerm.RowCount > 0)
			{
				GridEXRow[] rows = grdPaymentTerm.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					ExImEFormRegistrationPaymentTerms exImEFormRegistrationPaymentTerms = new ExImEFormRegistrationPaymentTerms();
					exImEFormRegistrationPaymentTerms.ExmLcPaymentTermId = Conversion.ToInt(r2.Cells["PaymentTermId"].Value);
					exImEFormRegistrationPaymentTerms.PaymentPercent = Conversion.ToDouble((object)r2.Cells["PaymentPercent"].Text);
					exImEFormRegistrationPaymentTerms.FcAmount = Conversion.ToDouble((object)r2.Cells["FcAmount"].Text);
					exImEFormRegistrationPaymentTerms.DaDays = Conversion.ToInt((object)r2.Cells["DueDays"].Text);
					obj.EFormRegistrationPaymentTerms.Add(exImEFormRegistrationPaymentTerms);
					if (Conversion.ToInt(r2.Cells["PaymentTermId"].Value) == 1)
					{
						AdvanceUtilizationGrid = Conversion.ToInt(r2.Cells["PaymentTermId"].Value);
					}
				}
			}
			if (grdPaymentUtilization.RowCount > 0)
			{
				GridEXRow[] rows = grdPaymentUtilization.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExImEFormRegistrationAdvancePaymentUtilizationDetail AdvPayUtilize = new ExImEFormRegistrationAdvancePaymentUtilizationDetail();
					AdvPayUtilize.FcCurrencyId = Conversion.ToInt(r.Cells["CurrencyCodeId"].Value);
					AdvPayUtilize.ExImFCBankReceiptsId = Conversion.ToInt(r.Cells["BankReferenceId"].Value);
					AdvPayUtilize.FcUtilizedAmount = Conversion.ToInt(r.Cells["ThisUtilize"].Value);
					AdvPayUtilize.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					obj.EFormAdvancePaymentUtilizationDetail.Add(AdvPayUtilize);
				}
			}
			else if (AdvanceUtilizationGrid == 1)
			{
				MessageBox.Show("Please Utilize Advance Amount");
				return;
			}
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.ScreenName = base.Name;
				obj.AttachmentsList.Add(attach);
			}
			if (ExImEFormRegistration.Save(obj) > 0)
			{
				if (!UpdateMode)
				{
					MessageBox.Show("Save SuccessFully");
					FormReset();
				}
				if (UpdateMode)
				{
					MessageBox.Show("Update SuccessFully");
					FormReset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		btnSave_Click(null, null);
	}

	public void FormReset()
	{
		try
		{
			cmbBank.Focus();
			((Control)(object)cmbBank).Text = string.Empty;
			txtIBAN.Text = string.Empty;
			((Control)(object)cmbTradeType).Text = string.Empty;
			((Control)(object)cmbpaymentmode).Text = string.Empty;
			txtfidate.Value = DateTime.Now;
			txtfiuniqueNo.Text = string.Empty;
			RBIsActive.Checked = true;
			((Control)(object)cmbConsignee).Text = string.Empty;
			txtconsigneeIban.Text = string.Empty;
			((Control)(object)cmbconsgneeocuntry).Text = string.Empty;
			((Control)(object)cmbPortOfDischarge).Text = string.Empty;
			((Control)(object)cmbDeliverymode).Text = string.Empty;
			((Control)(object)cmbFICode).Text = string.Empty;
			txtfivalue.Text = Conversion.ToInt((object)0).ToString();
			txtBalance.Text = Conversion.ToInt((object)0).ToString();
			suppliercustomer();
			PaymentTerm();
			BindGridHisory();
			GetFcBankUtlizedAmount = 0.0;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			grdPaymentTerm.ClearStructure();
			dtPayment.Clear();
			grdCommodity.ClearStructure();
			dtCommodity.Clear();
			txtCommodity.Text = string.Empty;
			txtNetweightDetail.Text = string.Empty;
			txtFCAmount.Text = string.Empty;
			btnUpdateComodity.Visible = false;
			btnCancelComodity.Visible = false;
			btnCommodityAdd.Visible = true;
			((Control)(object)cmbPaymentTerm).Text = string.Empty;
			txtPaymentPercent.Text = string.Empty;
			txtFcAmountPayment.Text = string.Empty;
			txtDueDays.Text = string.Empty;
			btnUpdatePayment.Visible = false;
			btnCancelPayment.Visible = false;
			btnAddPayment.Visible = true;
			((Control)(object)cmbBankReerence).Text = string.Empty;
			txtBalance.Text = string.Empty;
			txtThisUtilize.Text = string.Empty;
			txtFcAmountPaymentUtilization.Text = string.Empty;
			((Control)(object)cmbcurrency).Text = string.Empty;
			txtUtilizeAmount.Text = string.Empty;
			btnUpdatePaymentUtilization.Visible = false;
			btnCancelPaymentUtilization.Visible = false;
			btnAddPaymentUtilization.Visible = true;
			grdPaymentUtilization.ClearStructure();
			dtPaymentutilization.Clear();
			AT = new Attachment();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void FormCommodityReset()
	{
		try
		{
			txtCommodity.Focus();
			txtCommodity.Text = string.Empty;
			txtNetweightDetail.Text = string.Empty;
			txtFCAmount.Text = string.Empty;
			btnUpdateComodity.Visible = false;
			btnCancelComodity.Visible = false;
			btnCommodityAdd.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void FormPayment()
	{
		try
		{
			cmbPaymentTerm.Focus();
			((Control)(object)cmbPaymentTerm).Text = string.Empty;
			txtPaymentPercent.Text = string.Empty;
			txtFcAmountPayment.Text = string.Empty;
			txtDueDays.Text = string.Empty;
			btnUpdatePayment.Visible = false;
			btnCancelPayment.Visible = false;
			btnAddPayment.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void FormPaymentUtilization()
	{
		try
		{
			((Control)(object)cmbBankReerence).Text = string.Empty;
			txtBalance.Text = string.Empty;
			txtThisUtilize.Text = string.Empty;
			txtFcAmountPaymentUtilization.Text = string.Empty;
			((Control)(object)cmbcurrency).Text = string.Empty;
			txtUtilizeAmount.Text = string.Empty;
			btnUpdatePaymentUtilization.Visible = false;
			btnCancelPaymentUtilization.Visible = false;
			btnAddPaymentUtilization.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)txtExchangeRate.Text) > 0)
			{
				int TotalLocalAmount = 0;
				int ExchangeRate = Conversion.ToInt((object)txtExchangeRate.Text);
				TotalLocalAmount = Conversion.ToInt((object)txtfivalue.Text) * ExchangeRate;
				txtAmountRs.Text = Convert.ToString(TotalLocalAmount);
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
			ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[1].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			ExImEFormRegistration eFormRegistration = new ExImEFormRegistration();
			RecId = Id;
			eFormRegistration = ExImEFormRegistration.GetByID(RecId);
			tabControl1.SelectedIndex = 0;
			cmbConsignee.Value = eFormRegistration.ConsigneeId;
			cmbBank.Value = eFormRegistration.IssuingBankId;
			txtfiuniqueNo.Text = eFormRegistration.EFormNo;
			txtfidate.Value = eFormRegistration.EFormDate;
			cmbDeliverymode.Value = eFormRegistration.DeliveryTermId;
			cmbpaymentmode.Value = eFormRegistration.PaymenttermId;
			((Control)(object)cmbTradeType).Text = eFormRegistration.TradeType;
			cmbFICode.Value = eFormRegistration.FcurrencyId;
			txtfivalue.Text = eFormRegistration.EFormValueTotal.ToString();
			txtExchangeRate.Text = eFormRegistration.ExchangeRate.ToString();
			txtAmountRs.Text = eFormRegistration.AmountRs.ToString();
			txtconsigneeIban.Text = eFormRegistration.ConsigneeIban.ToString();
			txtIBAN.Text = Conversion.ToString((object)eFormRegistration.IbanNo);
			txtNetWeightkg.Text = eFormRegistration.NetWeightTotal.ToString();
			cmbconsgneeocuntry.Value = eFormRegistration.CountryId;
			cmbLoadingPort.Value = eFormRegistration.LoadingPortId;
			cmbPortOfDischarge.Value = eFormRegistration.DestinationPortId;
			grdCommodity.ClearStructure();
			grdPaymentTerm.ClearStructure();
			grdPaymentUtilization.ClearStructure();
			dtCommodity.Clear();
			dtPayment.Clear();
			dtPaymentutilization.Clear();
			for (int k = 0; k < eFormRegistration.EFormRegistrationComodityDetail.Count; k++)
			{
				dtCommodity.Rows.Add(eFormRegistration.EFormRegistrationComodityDetail[k].CommodityDescription, eFormRegistration.EFormRegistrationComodityDetail[k].FcAmount, eFormRegistration.EFormRegistrationComodityDetail[k].Weight);
			}
			grdCommodity.DataSource = dtCommodity;
			grdCommodity.RetrieveStructure();
			for (int j = 0; j < eFormRegistration.EFormRegistrationPaymentTerms.Count; j++)
			{
				dtPayment.Rows.Add(eFormRegistration.EFormRegistrationPaymentTerms[j].ExmLcPaymentTermId, eFormRegistration.EFormRegistrationPaymentTerms[j].PaymentTermName, eFormRegistration.EFormRegistrationPaymentTerms[j].PaymentPercent, eFormRegistration.EFormRegistrationPaymentTerms[j].FcAmount, eFormRegistration.EFormRegistrationPaymentTerms[j].DaDays);
			}
			grdPaymentTerm.DataSource = dtPayment;
			grdPaymentTerm.RetrieveStructure();
			PaymentGridSetting();
			for (int i = 0; i < eFormRegistration.EFormAdvancePaymentUtilizationDetail.Count; i++)
			{
				dtPaymentutilization.Rows.Add(eFormRegistration.EFormAdvancePaymentUtilizationDetail[i].ExImFCBankReceiptsId, eFormRegistration.EFormAdvancePaymentUtilizationDetail[i].BankFbpNo, eFormRegistration.EFormAdvancePaymentUtilizationDetail[i].FcCurrencyId, eFormRegistration.EFormAdvancePaymentUtilizationDetail[i].CurrencyCode, eFormRegistration.EFormAdvancePaymentUtilizationDetail[i].FcUtilizedAmount, eFormRegistration.EFormAdvancePaymentUtilizationDetail[i].Remarks);
			}
			grdPaymentUtilization.DataSource = dtPaymentutilization;
			grdPaymentUtilization.RetrieveStructure();
			UtilizationPaymentGridSetting();
			cmbCustomer_Leave(null, null);
			UpdateMode = true;
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(Id, base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void PaymentGridSetting()
	{
		try
		{
			cmbPaymentTerm.Focus();
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].Visible = false;
			grdPaymentTerm.RootTable.Columns["PaymentPercent"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerm.RootTable.Columns["FcAmount"].AggregateFunction = (AggregateFunction)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CommodityGridSetting()
	{
		try
		{
			txtCommodity.Focus();
			grdCommodity.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdCommodity.RootTable.Columns["FcAmount"].AggregateFunction = (AggregateFunction)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void UtilizationPaymentGridSetting()
	{
		try
		{
			grdPaymentUtilization.RootTable.Columns["BankReferenceId"].Visible = false;
			grdPaymentUtilization.RootTable.Columns["CurrencyCodeId"].Visible = false;
			grdPaymentUtilization.RootTable.Columns["ThisUtilize"].AggregateFunction = (AggregateFunction)2;
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
			FormReset();
			FormCommodityReset();
			FormPayment();
			FormPaymentUtilization();
			suppliercustomer();
			PaymentTerm();
			Country();
			issueBank();
			bindcmbLoadingPort();
			DeliveryTerm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCommodityAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (txtCommodity.Text.Trim() == "")
			{
				MessageBox.Show("Please Insert Commodity");
				txtCommodity.Focus();
				return;
			}
			if (txtNetweightDetail.Text.Trim() == "")
			{
				MessageBox.Show("Please Insert Weight");
				txtNetweightDetail.Focus();
				return;
			}
			if (txtFCAmount.Text.Trim() == "")
			{
				MessageBox.Show("Please Insert FcAmount");
				txtFCAmount.Focus();
				return;
			}
			checkCommodityamountWeight();
			dtCommodity.Rows.Add(txtCommodity.Text, txtNetweightDetail.Text, txtFCAmount.Text);
			grdCommodity.DataSource = dtCommodity;
			grdCommodity.RetrieveStructure();
			CommodityGridSetting();
			txtCommodity.Text = "";
			txtNetweightDetail.Text = "";
			txtFCAmount.Text = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddPayment_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbPaymentTerm).Text.Trim() == "")
			{
				MessageBox.Show("Please Select PaymentTerm");
				cmbPaymentTerm.Focus();
				return;
			}
			if (txtPaymentPercent.Text.Trim() == "")
			{
				MessageBox.Show("Please Insert Payment Percent ");
				txtPaymentPercent.Focus();
				return;
			}
			if (txtFcAmountPayment.Text.Trim() == "")
			{
				MessageBox.Show("Please Insert FcAmount");
				txtFcAmountPayment.Focus();
				return;
			}
			dtPayment.Rows.Add(cmbPaymentTerm.Value, ((Control)(object)cmbPaymentTerm).Text, txtPaymentPercent.Text, txtFcAmountPayment.Text, txtDueDays.Text);
			grdPaymentTerm.DataSource = dtPayment;
			grdPaymentTerm.RetrieveStructure();
			PaymentGridSetting();
			((Control)(object)cmbPaymentTerm).Text = "";
			txtPaymentPercent.Text = "";
			txtFcAmountPayment.Text = "";
			txtDueDays.Text = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddPaymentUtilization_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbBankReerence).Text.Trim() == "")
			{
				MessageBox.Show("Please Select BankReference");
				cmbBankReerence.Focus();
			}
			else if (((Control)(object)cmbcurrency).Text.Trim() == "")
			{
				MessageBox.Show("Please Select Currency");
				cmbcurrency.Focus();
			}
			else if (txtThisUtilize.Text.Trim() == "")
			{
				MessageBox.Show("Please Insert ThisUtlized Amount");
				txtThisUtilize.Focus();
			}
			else if (grdPaymentTerm.RowCount > 0)
			{
				btnAddPaymentUtilization.Enabled = true;
				dtPaymentutilization.Rows.Add(cmbBankReerence.Value, ((Control)(object)cmbBankReerence).Text, cmbcurrency.Value, ((Control)(object)cmbcurrency).Text, txtThisUtilize.Text, txtremarks.Text);
				grdPaymentUtilization.DataSource = dtPaymentutilization;
				grdPaymentUtilization.RetrieveStructure();
				UtilizationPaymentGridSetting();
				checkutlizedamount();
				txtThisUtilize.Text = "";
				txtremarks.Text = "";
				GetTotalFcAmount();
			}
			else
			{
				MessageBox.Show("Please Add Payment Terms First");
				btnAddPaymentUtilization.Enabled = false;
				cmbPaymentTerm.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpfrmEformRegistration_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && !UpdateMode)
			{
				btnSave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					BtnNew_Click(null, null);
				}
				else
				{
					BtnNew_Click(null, null);
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
		catch (Exception)
		{
			throw;
		}
	}

	public void BindGridHisory()
	{
	}

	private void grdCommodity_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdCommodity.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				txtCommodity.Text = item.Cells["Commodity"].Text.ToString();
				txtFCAmount.Text = item.Cells["FcAmount"].Text.ToString();
				txtNetweightDetail.Text = item.Cells["NetWeight"].Text.ToString();
				btnUpdateComodity.Visible = true;
				btnCancelComodity.Visible = true;
				btnCommodityAdd.Visible = false;
				txtCommodity.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateComodity_Click(object sender, EventArgs e)
	{
		try
		{
			dtCommodity.Rows[updateDetailIndex]["Commodity"] = txtCommodity.Text;
			dtCommodity.Rows[updateDetailIndex]["FcAmount"] = txtFCAmount.Text;
			dtCommodity.Rows[updateDetailIndex]["NetWeight"] = txtNetweightDetail.Text;
			grdCommodity.DataSource = dtCommodity;
			grdCommodity.RetrieveStructure();
			CommodityGridSetting();
			txtCommodity.Text = string.Empty;
			txtNetweightDetail.Text = string.Empty;
			txtFCAmount.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelComodity_Click(object sender, EventArgs e)
	{
		FormCommodityReset();
	}

	private void grdPaymentTerm_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow itemBB = grdPaymentTerm.CurrentRow;
			if (itemBB != null)
			{
				updatePaymentDetailIndex = itemBB.RowIndex;
				cmbPaymentTerm.Value = itemBB.Cells["PaymentTermId"].Value;
				txtPaymentPercent.Text = itemBB.Cells["PaymentPercent"].Text;
				txtFcAmountPayment.Text = itemBB.Cells["FcAmount"].Text;
				txtDueDays.Text = itemBB.Cells["DueDays"].Text;
				grdPaymentTerm.DataSource = dtPayment;
				grdPaymentTerm.RetrieveStructure();
				PaymentGridSetting();
				btnUpdatePayment.Visible = true;
				btnCancelPayment.Visible = true;
				btnAddPayment.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdatePayment_Click(object sender, EventArgs e)
	{
		try
		{
			dtPayment.Rows[updatePaymentDetailIndex]["PaymentTermId"] = cmbPaymentTerm.Value;
			dtPayment.Rows[updatePaymentDetailIndex]["PaymentTerm"] = ((Control)(object)cmbPaymentTerm).Text;
			dtPayment.Rows[updatePaymentDetailIndex]["PaymentPercent"] = txtPaymentPercent.Text;
			dtPayment.Rows[updatePaymentDetailIndex]["FcAmount"] = txtFcAmountPayment.Text;
			dtPayment.Rows[updatePaymentDetailIndex]["DueDays"] = txtDueDays.Text;
			grdPaymentTerm.DataSource = dtPayment;
			grdPaymentTerm.RetrieveStructure();
			PaymentGridSetting();
			FormPayment();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelPayment_Click(object sender, EventArgs e)
	{
		FormPayment();
	}

	private void grdPaymentUtilization_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow itemSS = grdPaymentUtilization.CurrentRow;
			if (itemSS != null)
			{
				updateAdvancedPaymentDetailIndex = itemSS.RowIndex;
				cmbBankReerence.Value = itemSS.Cells["BankReferenceId"].Value;
				cmbcurrency.Value = itemSS.Cells["CurrencyCodeId"].Value;
				txtThisUtilize.Text = itemSS.Cells["ThisUtilize"].Text;
				txtremarks.Text = itemSS.Cells["Remarks"].Text;
				cmbBankReerence_Leave(null, null);
				grdPaymentUtilization.DataSource = dtPaymentutilization;
				grdPaymentUtilization.RetrieveStructure();
				UtilizationPaymentGridSetting();
				btnUpdatePaymentUtilization.Visible = true;
				btnCancelPaymentUtilization.Visible = true;
				btnAddPaymentUtilization.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdatePaymentUtilization_Click(object sender, EventArgs e)
	{
		try
		{
			dtPaymentutilization.Rows[updateAdvancedPaymentDetailIndex]["BankReferenceId"] = cmbBankReerence.Value;
			dtPaymentutilization.Rows[updateAdvancedPaymentDetailIndex]["BankReference"] = ((Control)(object)cmbBankReerence).Text;
			dtPaymentutilization.Rows[updateAdvancedPaymentDetailIndex]["CurrencyCodeId"] = cmbcurrency.Value;
			dtPaymentutilization.Rows[updateAdvancedPaymentDetailIndex]["CurrencyCode"] = ((Control)(object)cmbcurrency).Text;
			dtPaymentutilization.Rows[updateAdvancedPaymentDetailIndex]["ThisUtilize"] = txtThisUtilize.Text;
			dtPaymentutilization.Rows[updateAdvancedPaymentDetailIndex]["Remarks"] = txtremarks.Text;
			grdPaymentUtilization.DataSource = dtPaymentutilization;
			grdPaymentUtilization.RetrieveStructure();
			UtilizationPaymentGridSetting();
			calculations();
			FormPaymentUtilization();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelPaymentUtilization_Click(object sender, EventArgs e)
	{
		FormPaymentUtilization();
	}

	private void calculations()
	{
		double UtlizedAmount = 0.0;
		double balanceAmount = 0.0;
		UtlizedAmount = double.Parse(grdPaymentUtilization.GetTotal(grdPaymentUtilization.RootTable.Columns["ThisUtilize"], (AggregateFunction)2).ToString());
		balanceAmount = Conversion.ToDouble((object)txtFcAmountPaymentUtilization.Text) - UtlizedAmount;
		txtBalance.Text = balanceAmount.ToString();
	}

	private void checkpercent()
	{
		totalpercent = double.Parse(grdPaymentTerm.GetTotal(grdPaymentTerm.RootTable.Columns["PaymentPercent"], (AggregateFunction)2).ToString());
		totalpaymentfcamount = double.Parse(grdPaymentTerm.GetTotal(grdPaymentTerm.RootTable.Columns["FcAmount"], (AggregateFunction)2).ToString());
	}

	private void checkutlizedamount()
	{
		try
		{
			double BalanceAmount = 0.0;
			double num = Conversion.ToDouble((object)txtThisUtilize.Text);
			BalanceAmount = Conversion.ToDouble((object)txtBalance.Text);
			if (num > BalanceAmount)
			{
				MessageBox.Show("Your Utlized Amount Is Greater Than Balance Please Check");
				txtThisUtilize.Focus();
				btnAddPaymentUtilization.Enabled = false;
			}
			else
			{
				btnAddPaymentUtilization.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void checkpaymentamount()
	{
		try
		{
			double paymentamount = 0.0;
			double headerfcvalue = 0.0;
			double PaymentPercent = 0.0;
			if (txtfivalue.Text != string.Empty && txtPaymentPercent.Text != string.Empty)
			{
				headerfcvalue = Conversion.ToDouble((object)txtfivalue.Text);
				PaymentPercent = Conversion.ToDouble((object)txtPaymentPercent.Text);
				double FcPayment = headerfcvalue * PaymentPercent / 100.0;
				txtFcAmountPayment.Text = FcPayment.ToString();
				paymentamount = Conversion.ToDouble((object)txtFcAmountPayment.Text);
			}
			if (PaymentPercent > 100.0)
			{
				MessageBox.Show("Payment Percent Is Greater Than 100 Plese Check");
				txtPaymentPercent.Focus();
				btnAddPayment.Enabled = false;
			}
			else if (paymentamount > headerfcvalue)
			{
				MessageBox.Show("Your PaymentFc Amount Is Greater Than Fc Value Please Check");
				txtFcAmountPayment.Focus();
				btnAddPayment.Enabled = false;
			}
			else
			{
				btnAddPayment.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void checkCommodityamountWeight()
	{
		try
		{
			double CommodityFcamount = 0.0;
			double headerfcvalue = 0.0;
			double WeightHeader = 0.0;
			headerfcvalue = Conversion.ToDouble((object)txtfivalue.Text);
			CommodityFcamount = Conversion.ToDouble((object)txtFCAmount.Text);
			WeightHeader = Conversion.ToDouble((object)txtNetWeightkg.Text);
			if (Conversion.ToDouble((object)txtNetweightDetail.Text) > WeightHeader)
			{
				MessageBox.Show("Weight Is Greater Than Net Weight :Please Check");
				txtNetweightDetail.Focus();
				btnCommodityAdd.Enabled = false;
			}
			else if (CommodityFcamount > headerfcvalue)
			{
				MessageBox.Show("Your Fc Amount Is Greater Than Fc Value :Please Check");
				txtFCAmount.Focus();
				btnCommodityAdd.Enabled = false;
			}
			else
			{
				btnCommodityAdd.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetTotalFcAmount()
	{
		try
		{
			GridEXRow[] rows = grdPaymentUtilization.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(cmbBankReerence.Value) == Conversion.ToInt(r.Cells["BankReferenceId"].Value))
				{
					double utlizedamount = Conversion.ToSingle(r.Cells["ThisUtilize"].Value);
					GetFcBankUtlizedAmount += utlizedamount;
				}
			}
			double balanceAmount = 0.0;
			txtUtilizeAmount.Text = Conversion.ToString((object)GetFcBankUtlizedAmount);
			balanceAmount = Conversion.ToDouble((object)txtFcAmountPaymentUtilization.Text) - GetFcBankUtlizedAmount;
			txtBalance.Text = Conversion.ToString((object)balanceAmount);
			GetFcBankUtlizedAmount = 0.0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentUtilization_LocationChanged(object sender, EventArgs e)
	{
		checkutlizedamount();
	}

	private void grdPaymentUtilization_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		checkutlizedamount();
	}

	private void grdPaymentUtilization_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		checkutlizedamount();
	}

	private void txtThisUtilize_TextChanged(object sender, EventArgs e)
	{
		checkutlizedamount();
	}

	private void txtThisUtilize_Leave(object sender, EventArgs e)
	{
		checkutlizedamount();
	}

	private void txtFcAmountPayment_TextChanged(object sender, EventArgs e)
	{
		checkpaymentamount();
	}

	private void txtPaymentPercent_TextChanged(object sender, EventArgs e)
	{
		checkpaymentamount();
	}

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.CanViewAllRecord = DoHaveCanViewAllRecordRights;
			obj.NoOfRecords = 50;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			DataTable dt = ExImEFormRegistration.FormHistory(obj);
			if (dt.Rows.Count > 0)
			{
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				DataGridHistory.RootTable.Columns["Id"].Visible = false;
				DataGridHistory.RootTable.Columns["RecordNo"].Visible = false;
				DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
				ctrlGrdBar1_Load(null, null);
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

	private void txtNetweightDetail_TextChanged(object sender, EventArgs e)
	{
		checkCommodityamountWeight();
	}

	private void txtFCAmount_TextChanged(object sender, EventArgs e)
	{
		checkCommodityamountWeight();
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			HistoryFill();
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 207);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpfrmFinancialInsturment_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Expected O, but got Unknown
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Expected O, but got Unknown
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Expected O, but got Unknown
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Expected O, but got Unknown
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Expected O, but got Unknown
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Expected O, but got Unknown
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Expected O, but got Unknown
		//IL_0887: Unknown result type (might be due to invalid IL or missing references)
		//IL_0891: Expected O, but got Unknown
		//IL_089d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a7: Expected O, but got Unknown
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Expected O, but got Unknown
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Expected O, but got Unknown
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Expected O, but got Unknown
		//IL_0f4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f54: Expected O, but got Unknown
		//IL_0f61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6b: Expected O, but got Unknown
		//IL_7e54: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e5e: Expected O, but got Unknown
		GridEXLayout grdPaymentUtilization_DesignTimeLayout = new GridEXLayout();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		GridEXLayout grdCommodity_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentTerm_DesignTimeLayout = new GridEXLayout();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance12 = new Appearance();
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
		Appearance appearance25 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.ExpfrmFinancialInsturment));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdPaymentUtilization = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.cmbBankReerence = new UltraCombo();
		this.btnCancelPaymentUtilization = new System.Windows.Forms.Button();
		this.btnUpdatePaymentUtilization = new System.Windows.Forms.Button();
		this.cmbcurrency = new UltraCombo();
		this.txtUtilizeAmount = new System.Windows.Forms.TextBox();
		this.btnAddPaymentUtilization = new System.Windows.Forms.Button();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtThisUtilize = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtFcAmountPaymentUtilization = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtBalance = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdCommodity = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grdPaymentTerm = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.btnCancelPayment = new System.Windows.Forms.Button();
		this.btnUpdatePayment = new System.Windows.Forms.Button();
		this.cmbPaymentTerm = new UltraCombo();
		this.btnAddPayment = new System.Windows.Forms.Button();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtFcAmountPayment = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtPaymentPercent = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnCancelComodity = new System.Windows.Forms.Button();
		this.btnUpdateComodity = new System.Windows.Forms.Button();
		this.btnCommodityAdd = new System.Windows.Forms.Button();
		this.txtFCAmount = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtNetweightDetail = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtCommodity = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.RBIsActive = new System.Windows.Forms.CheckBox();
		this.label52 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.txtBalnaceAmt = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.txtconsigneeIban = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.txtexpirydate = new System.Windows.Forms.DateTimePicker();
		this.label41 = new System.Windows.Forms.Label();
		this.txtconsigneeAddress = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.cmbpaymentmode = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.cmbTradeType = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.txtIBAN = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.cmbPortOfDischarge = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.cmbLoadingPort = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.txtfidate = new System.Windows.Forms.DateTimePicker();
		this.txtfiuniqueNo = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbBank = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtNetWeightkg = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtAmountRs = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtfivalue = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbconsgneeocuntry = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbFICode = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbDeliverymode = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbConsignee = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridHistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label37 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentUtilization).BeginInit();
		this.panel7.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBankReerence).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).BeginInit();
		this.panel5.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCommodity).BeginInit();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).BeginInit();
		this.panel14.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).BeginInit();
		this.groupBox2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbpaymentmode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTradeType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPortOfDischarge).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbconsgneeocuntry).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFICode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDeliverymode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbConsignee).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel11.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(982, 293);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(982, 293);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(974, 267);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 585);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(968, 0);
		this.panel6.TabIndex = 4;
		this.panel6.Visible = false;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentUtilization);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel8.Location = new System.Drawing.Point(0, 99);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(770, 0);
		this.panel8.TabIndex = 1;
		this.grdPaymentUtilization.AllowDelete = (InheritableBoolean)1;
		this.grdPaymentUtilization.ColumnAutoResize = true;
		grdPaymentUtilization_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentUtilization.DesignTimeLayout = grdPaymentUtilization_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentUtilization).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentUtilization).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentUtilization.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentUtilization).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPaymentUtilization).Name = "grdPaymentUtilization";
		((ScrollableBase)this.grdPaymentUtilization).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentUtilization).Size = new System.Drawing.Size(770, 0);
		((System.Windows.Forms.Control)(object)this.grdPaymentUtilization).TabIndex = 10;
		this.grdPaymentUtilization.TotalRow = (InheritableBoolean)1;
		this.grdPaymentUtilization.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentUtilization).VisualStyle = (VisualStyle)4;
		this.grdPaymentUtilization.CellValueChanged += new ColumnActionEventHandler(grdPaymentUtilization_CellValueChanged);
		this.grdPaymentUtilization.CellUpdated += new ColumnActionEventHandler(grdPaymentUtilization_CellUpdated);
		((System.Windows.Forms.Control)(object)this.grdPaymentUtilization).LocationChanged += new System.EventHandler(grdPaymentUtilization_LocationChanged);
		((System.Windows.Forms.Control)(object)this.grdPaymentUtilization).DoubleClick += new System.EventHandler(grdPaymentUtilization_DoubleClick);
		this.panel7.Controls.Add(this.groupBox4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(968, 99);
		this.panel7.TabIndex = 0;
		this.groupBox4.BackColor = System.Drawing.Color.White;
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBankReerence);
		this.groupBox4.Controls.Add(this.btnCancelPaymentUtilization);
		this.groupBox4.Controls.Add(this.btnUpdatePaymentUtilization);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcurrency);
		this.groupBox4.Controls.Add(this.txtUtilizeAmount);
		this.groupBox4.Controls.Add(this.btnAddPaymentUtilization);
		this.groupBox4.Controls.Add(this.txtremarks);
		this.groupBox4.Controls.Add(this.txtThisUtilize);
		this.groupBox4.Controls.Add(this.label25);
		this.groupBox4.Controls.Add(this.label28);
		this.groupBox4.Controls.Add(this.txtFcAmountPaymentUtilization);
		this.groupBox4.Controls.Add(this.label24);
		this.groupBox4.Controls.Add(this.label20);
		this.groupBox4.Controls.Add(this.label23);
		this.groupBox4.Controls.Add(this.label22);
		this.groupBox4.Controls.Add(this.txtBalance);
		this.groupBox4.Controls.Add(this.label21);
		this.groupBox4.Location = new System.Drawing.Point(5, 6);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1306, 77);
		this.groupBox4.TabIndex = 2;
		this.groupBox4.TabStop = false;
		this.groupBox4.Visible = false;
		this.cmbBankReerence.AllowNull = (DefaultableBoolean)1;
		this.cmbBankReerence.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBankReerence.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBankReerence.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBankReerence).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.cmbBankReerence).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBankReerence).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbBankReerence.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBankReerence).Location = new System.Drawing.Point(6, 45);
		((System.Windows.Forms.Control)(object)this.cmbBankReerence).Name = "cmbBankReerence";
		((System.Windows.Forms.Control)(object)this.cmbBankReerence).Size = new System.Drawing.Size(164, 22);
		((System.Windows.Forms.Control)(object)this.cmbBankReerence).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBankReerence).Leave += new System.EventHandler(cmbBankReerence_Leave);
		this.btnCancelPaymentUtilization.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancelPaymentUtilization.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelPaymentUtilization.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelPaymentUtilization.Location = new System.Drawing.Point(1201, 45);
		this.btnCancelPaymentUtilization.Name = "btnCancelPaymentUtilization";
		this.btnCancelPaymentUtilization.Size = new System.Drawing.Size(56, 23);
		this.btnCancelPaymentUtilization.TabIndex = 9;
		this.btnCancelPaymentUtilization.Text = "Cancel";
		this.btnCancelPaymentUtilization.UseVisualStyleBackColor = true;
		this.btnCancelPaymentUtilization.Visible = false;
		this.btnCancelPaymentUtilization.Click += new System.EventHandler(btnCancelPaymentUtilization_Click);
		this.btnUpdatePaymentUtilization.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnUpdatePaymentUtilization.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdatePaymentUtilization.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdatePaymentUtilization.Location = new System.Drawing.Point(1138, 44);
		this.btnUpdatePaymentUtilization.Name = "btnUpdatePaymentUtilization";
		this.btnUpdatePaymentUtilization.Size = new System.Drawing.Size(56, 24);
		this.btnUpdatePaymentUtilization.TabIndex = 8;
		this.btnUpdatePaymentUtilization.Text = "Update";
		this.btnUpdatePaymentUtilization.UseVisualStyleBackColor = true;
		this.btnUpdatePaymentUtilization.Visible = false;
		this.btnUpdatePaymentUtilization.Click += new System.EventHandler(btnUpdatePaymentUtilization_Click);
		this.cmbcurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbcurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbcurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Location = new System.Drawing.Point(176, 45);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Name = "cmbcurrency";
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Size = new System.Drawing.Size(126, 22);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).TabIndex = 1;
		this.txtUtilizeAmount.BackColor = System.Drawing.Color.White;
		this.txtUtilizeAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtUtilizeAmount.Enabled = false;
		this.txtUtilizeAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtUtilizeAmount.Location = new System.Drawing.Point(436, 45);
		this.txtUtilizeAmount.Name = "txtUtilizeAmount";
		this.txtUtilizeAmount.ReadOnly = true;
		this.txtUtilizeAmount.Size = new System.Drawing.Size(97, 22);
		this.txtUtilizeAmount.TabIndex = 3;
		this.btnAddPaymentUtilization.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAddPaymentUtilization.Location = new System.Drawing.Point(1105, 45);
		this.btnAddPaymentUtilization.Name = "btnAddPaymentUtilization";
		this.btnAddPaymentUtilization.Size = new System.Drawing.Size(27, 23);
		this.btnAddPaymentUtilization.TabIndex = 7;
		this.btnAddPaymentUtilization.UseVisualStyleBackColor = true;
		this.btnAddPaymentUtilization.Click += new System.EventHandler(btnAddPaymentUtilization_Click);
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(807, 45);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(294, 22);
		this.txtremarks.TabIndex = 6;
		this.txtThisUtilize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtThisUtilize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtThisUtilize.Location = new System.Drawing.Point(666, 45);
		this.txtThisUtilize.Name = "txtThisUtilize";
		this.txtThisUtilize.Size = new System.Drawing.Size(135, 22);
		this.txtThisUtilize.TabIndex = 5;
		this.txtThisUtilize.TextChanged += new System.EventHandler(txtThisUtilize_TextChanged);
		this.txtThisUtilize.Leave += new System.EventHandler(txtThisUtilize_Leave);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(433, 21);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(100, 17);
		this.label25.TabIndex = 6;
		this.label25.Text = "Utilized Amount";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(807, 21);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(58, 17);
		this.label28.TabIndex = 10;
		this.label28.Text = "Remarks";
		this.txtFcAmountPaymentUtilization.BackColor = System.Drawing.Color.White;
		this.txtFcAmountPaymentUtilization.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcAmountPaymentUtilization.Enabled = false;
		this.txtFcAmountPaymentUtilization.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcAmountPaymentUtilization.Location = new System.Drawing.Point(307, 45);
		this.txtFcAmountPaymentUtilization.Name = "txtFcAmountPaymentUtilization";
		this.txtFcAmountPaymentUtilization.ReadOnly = true;
		this.txtFcAmountPaymentUtilization.Size = new System.Drawing.Size(123, 22);
		this.txtFcAmountPaymentUtilization.TabIndex = 2;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(666, 23);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(70, 17);
		this.label24.TabIndex = 10;
		this.label24.Text = "This Utilize";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(3, 21);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(97, 17);
		this.label20.TabIndex = 0;
		this.label20.Text = "Bank Reference";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(535, 23);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(52, 17);
		this.label23.TabIndex = 8;
		this.label23.Text = "Balance";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(307, 21);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(69, 17);
		this.label22.TabIndex = 4;
		this.label22.Text = "Fc Amount";
		this.txtBalance.BackColor = System.Drawing.Color.White;
		this.txtBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalance.Enabled = false;
		this.txtBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalance.Location = new System.Drawing.Point(538, 45);
		this.txtBalance.Name = "txtBalance";
		this.txtBalance.ReadOnly = true;
		this.txtBalance.Size = new System.Drawing.Size(122, 22);
		this.txtBalance.TabIndex = 4;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(173, 21);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(94, 17);
		this.label21.TabIndex = 2;
		this.label21.Text = "Currency Code";
		this.panel5.Controls.Add(this.panel16);
		this.panel5.Controls.Add(this.panel15);
		this.panel5.Controls.Add(this.panel14);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 312);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(968, 273);
		this.panel5.TabIndex = 3;
		this.panel5.Visible = false;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdCommodity);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel16.Location = new System.Drawing.Point(770, 93);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(621, 180);
		this.panel16.TabIndex = 12;
		this.grdCommodity.AllowDelete = (InheritableBoolean)1;
		this.grdCommodity.ColumnAutoResize = true;
		grdCommodity_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCommodity.DesignTimeLayout = grdCommodity_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCommodity).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdCommodity).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCommodity.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCommodity).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdCommodity).Name = "grdCommodity";
		((ScrollableBase)this.grdCommodity).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCommodity).Size = new System.Drawing.Size(621, 180);
		((System.Windows.Forms.Control)(object)this.grdCommodity).TabIndex = 8;
		this.grdCommodity.TotalRow = (InheritableBoolean)1;
		this.grdCommodity.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCommodity).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdCommodity).DoubleClick += new System.EventHandler(grdCommodity_DoubleClick);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTerm);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel15.Location = new System.Drawing.Point(0, 93);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(770, 180);
		this.panel15.TabIndex = 11;
		this.grdPaymentTerm.AllowDelete = (InheritableBoolean)1;
		this.grdPaymentTerm.ColumnAutoResize = true;
		grdPaymentTerm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTerm.DesignTimeLayout = grdPaymentTerm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTerm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Name = "grdPaymentTerm";
		((ScrollableBase)this.grdPaymentTerm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Size = new System.Drawing.Size(770, 180);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).TabIndex = 9;
		this.grdPaymentTerm.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTerm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTerm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).DoubleClick += new System.EventHandler(grdPaymentTerm_DoubleClick);
		this.panel14.Controls.Add(this.groupBox3);
		this.panel14.Controls.Add(this.groupBox2);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(968, 93);
		this.panel14.TabIndex = 10;
		this.panel14.Visible = false;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.btnCancelPayment);
		this.groupBox3.Controls.Add(this.btnUpdatePayment);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPaymentTerm);
		this.groupBox3.Controls.Add(this.btnAddPayment);
		this.groupBox3.Controls.Add(this.txtDueDays);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add(this.txtFcAmountPayment);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.txtPaymentPercent);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Location = new System.Drawing.Point(5, 0);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(737, 81);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.btnCancelPayment.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancelPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelPayment.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelPayment.Location = new System.Drawing.Point(623, 41);
		this.btnCancelPayment.Name = "btnCancelPayment";
		this.btnCancelPayment.Size = new System.Drawing.Size(56, 25);
		this.btnCancelPayment.TabIndex = 6;
		this.btnCancelPayment.Text = "Cancel";
		this.btnCancelPayment.UseVisualStyleBackColor = true;
		this.btnCancelPayment.Visible = false;
		this.btnCancelPayment.Click += new System.EventHandler(btnCancelPayment_Click);
		this.btnUpdatePayment.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnUpdatePayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdatePayment.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdatePayment.Location = new System.Drawing.Point(623, 13);
		this.btnUpdatePayment.Name = "btnUpdatePayment";
		this.btnUpdatePayment.Size = new System.Drawing.Size(56, 25);
		this.btnUpdatePayment.TabIndex = 5;
		this.btnUpdatePayment.Text = "Update";
		this.btnUpdatePayment.UseVisualStyleBackColor = true;
		this.btnUpdatePayment.Visible = false;
		this.btnUpdatePayment.Click += new System.EventHandler(btnUpdatePayment_Click);
		this.cmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Location = new System.Drawing.Point(6, 41);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Name = "cmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Size = new System.Drawing.Size(168, 22);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).TabIndex = 0;
		this.btnAddPayment.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAddPayment.Location = new System.Drawing.Point(590, 38);
		this.btnAddPayment.Name = "btnAddPayment";
		this.btnAddPayment.Size = new System.Drawing.Size(27, 23);
		this.btnAddPayment.TabIndex = 4;
		this.btnAddPayment.UseVisualStyleBackColor = true;
		this.btnAddPayment.Click += new System.EventHandler(btnAddPayment_Click);
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(449, 41);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(135, 22);
		this.txtDueDays.TabIndex = 3;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(449, 15);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(63, 17);
		this.label19.TabIndex = 6;
		this.label19.Text = "Due Days";
		this.txtFcAmountPayment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcAmountPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcAmountPayment.Location = new System.Drawing.Point(308, 41);
		this.txtFcAmountPayment.Name = "txtFcAmountPayment";
		this.txtFcAmountPayment.Size = new System.Drawing.Size(135, 22);
		this.txtFcAmountPayment.TabIndex = 2;
		this.txtFcAmountPayment.TextChanged += new System.EventHandler(txtFcAmountPayment_TextChanged);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(308, 15);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(69, 17);
		this.label16.TabIndex = 4;
		this.label16.Text = "Fc Amount";
		this.txtPaymentPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPaymentPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPaymentPercent.Location = new System.Drawing.Point(180, 41);
		this.txtPaymentPercent.Name = "txtPaymentPercent";
		this.txtPaymentPercent.Size = new System.Drawing.Size(122, 22);
		this.txtPaymentPercent.TabIndex = 1;
		this.txtPaymentPercent.TextChanged += new System.EventHandler(txtPaymentPercent_TextChanged);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(7, 15);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(90, 17);
		this.label18.TabIndex = 0;
		this.label18.Text = "Payment Term";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(177, 15);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(104, 17);
		this.label17.TabIndex = 2;
		this.label17.Text = "Payment Percent";
		this.groupBox2.BackColor = System.Drawing.Color.White;
		this.groupBox2.Controls.Add(this.btnCancelComodity);
		this.groupBox2.Controls.Add(this.btnUpdateComodity);
		this.groupBox2.Controls.Add(this.btnCommodityAdd);
		this.groupBox2.Controls.Add(this.txtFCAmount);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.txtNetweightDetail);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.txtCommodity);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Location = new System.Drawing.Point(786, 7);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(558, 74);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.btnCancelComodity.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancelComodity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelComodity.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelComodity.Location = new System.Drawing.Point(483, 41);
		this.btnCancelComodity.Name = "btnCancelComodity";
		this.btnCancelComodity.Size = new System.Drawing.Size(56, 22);
		this.btnCancelComodity.TabIndex = 5;
		this.btnCancelComodity.Text = "Cancel";
		this.btnCancelComodity.UseVisualStyleBackColor = true;
		this.btnCancelComodity.Visible = false;
		this.btnCancelComodity.Click += new System.EventHandler(btnCancelComodity_Click);
		this.btnUpdateComodity.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnUpdateComodity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateComodity.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateComodity.Location = new System.Drawing.Point(483, 12);
		this.btnUpdateComodity.Name = "btnUpdateComodity";
		this.btnUpdateComodity.Size = new System.Drawing.Size(56, 28);
		this.btnUpdateComodity.TabIndex = 4;
		this.btnUpdateComodity.Text = "Update";
		this.btnUpdateComodity.UseVisualStyleBackColor = true;
		this.btnUpdateComodity.Visible = false;
		this.btnUpdateComodity.Click += new System.EventHandler(btnUpdateComodity_Click);
		this.btnCommodityAdd.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnCommodityAdd.Location = new System.Drawing.Point(445, 41);
		this.btnCommodityAdd.Name = "btnCommodityAdd";
		this.btnCommodityAdd.Size = new System.Drawing.Size(27, 23);
		this.btnCommodityAdd.TabIndex = 3;
		this.btnCommodityAdd.UseVisualStyleBackColor = true;
		this.btnCommodityAdd.Click += new System.EventHandler(btnCommodityAdd_Click);
		this.txtFCAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFCAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFCAmount.Location = new System.Drawing.Point(304, 41);
		this.txtFCAmount.Name = "txtFCAmount";
		this.txtFCAmount.Size = new System.Drawing.Size(135, 22);
		this.txtFCAmount.TabIndex = 2;
		this.txtFCAmount.TextChanged += new System.EventHandler(txtFCAmount_TextChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(304, 13);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(65, 17);
		this.label15.TabIndex = 4;
		this.label15.Text = "FcAmount";
		this.txtNetweightDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetweightDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetweightDetail.Location = new System.Drawing.Point(176, 41);
		this.txtNetweightDetail.Name = "txtNetweightDetail";
		this.txtNetweightDetail.Size = new System.Drawing.Size(122, 22);
		this.txtNetweightDetail.TabIndex = 1;
		this.txtNetweightDetail.TextChanged += new System.EventHandler(txtNetweightDetail_TextChanged);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(173, 13);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(73, 17);
		this.label14.TabIndex = 2;
		this.label14.Text = "Net Weight";
		this.txtCommodity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommodity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommodity.Location = new System.Drawing.Point(6, 41);
		this.txtCommodity.Name = "txtCommodity";
		this.txtCommodity.Size = new System.Drawing.Size(164, 22);
		this.txtCommodity.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(3, 13);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(75, 17);
		this.label13.TabIndex = 0;
		this.label13.Text = "Commodity";
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 69);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(968, 243);
		this.panel4.TabIndex = 2;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.RBIsActive);
		this.groupBox1.Controls.Add(this.label52);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.txtBalnaceAmt);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Controls.Add(this.txtconsigneeIban);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.txtexpirydate);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.txtconsigneeAddress);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbpaymentmode);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTradeType);
		this.groupBox1.Controls.Add(this.label45);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.txtIBAN);
		this.groupBox1.Controls.Add(this.label44);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPortOfDischarge);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingPort);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.txtfidate);
		this.groupBox1.Controls.Add(this.txtfiuniqueNo);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBank);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtNetWeightkg);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtAmountRs);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtfivalue);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbconsgneeocuntry);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFICode);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDeliverymode);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbConsignee);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Location = new System.Drawing.Point(5, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(959, 188);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.RBIsActive.AutoSize = true;
		this.RBIsActive.Location = new System.Drawing.Point(312, 152);
		this.RBIsActive.Name = "RBIsActive";
		this.RBIsActive.Size = new System.Drawing.Size(64, 17);
		this.RBIsActive.TabIndex = 56;
		this.RBIsActive.Text = "IsActive";
		this.RBIsActive.UseVisualStyleBackColor = true;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.Red;
		this.label52.Location = new System.Drawing.Point(80, 70);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(14, 17);
		this.label52.TabIndex = 55;
		this.label52.Text = "*";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(738, 128);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(14, 17);
		this.label50.TabIndex = 15;
		this.label50.Text = "*";
		this.txtBalnaceAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalnaceAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalnaceAmt.Location = new System.Drawing.Point(765, 99);
		this.txtBalnaceAmt.Name = "txtBalnaceAmt";
		this.txtBalnaceAmt.Size = new System.Drawing.Size(178, 22);
		this.txtBalnaceAmt.TabIndex = 14;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(665, 102);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(52, 17);
		this.label51.TabIndex = 53;
		this.label51.Text = "Balance";
		this.txtconsigneeIban.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtconsigneeIban.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtconsigneeIban.Location = new System.Drawing.Point(478, 46);
		this.txtconsigneeIban.Name = "txtconsigneeIban";
		this.txtconsigneeIban.Size = new System.Drawing.Size(178, 22);
		this.txtconsigneeIban.TabIndex = 8;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(350, 49);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(101, 17);
		this.label49.TabIndex = 51;
		this.label49.Text = "Consignee IBAN";
		this.txtexpirydate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtexpirydate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtexpirydate.Location = new System.Drawing.Point(765, 124);
		this.txtexpirydate.Name = "txtexpirydate";
		this.txtexpirydate.Size = new System.Drawing.Size(178, 25);
		this.txtexpirydate.TabIndex = 15;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(665, 128);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(74, 17);
		this.label41.TabIndex = 48;
		this.label41.Text = "Expiry Date";
		this.txtconsigneeAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtconsigneeAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtconsigneeAddress.Location = new System.Drawing.Point(478, 73);
		this.txtconsigneeAddress.Name = "txtconsigneeAddress";
		this.txtconsigneeAddress.Size = new System.Drawing.Size(178, 22);
		this.txtconsigneeAddress.TabIndex = 8;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(350, 76);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(121, 17);
		this.label47.TabIndex = 46;
		this.label47.Text = "Consignee Address";
		this.cmbpaymentmode.AllowNull = (DefaultableBoolean)1;
		this.cmbpaymentmode.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbpaymentmode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbpaymentmode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbpaymentmode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbpaymentmode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbpaymentmode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbpaymentmode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbpaymentmode).Location = new System.Drawing.Point(119, 93);
		((System.Windows.Forms.Control)(object)this.cmbpaymentmode).Name = "cmbpaymentmode";
		((System.Windows.Forms.Control)(object)this.cmbpaymentmode).Size = new System.Drawing.Size(185, 22);
		((System.Windows.Forms.Control)(object)this.cmbpaymentmode).TabIndex = 3;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(8, 96);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(96, 17);
		this.label46.TabIndex = 44;
		this.label46.Text = "Payment Mode";
		this.cmbTradeType.AllowNull = (DefaultableBoolean)1;
		this.cmbTradeType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTradeType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTradeType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTradeType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.cmbTradeType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTradeType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbTradeType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTradeType).Location = new System.Drawing.Point(119, 67);
		((System.Windows.Forms.Control)(object)this.cmbTradeType).Name = "cmbTradeType";
		((System.Windows.Forms.Control)(object)this.cmbTradeType).Size = new System.Drawing.Size(185, 22);
		((System.Windows.Forms.Control)(object)this.cmbTradeType).TabIndex = 2;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(8, 70);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(72, 17);
		this.label45.TabIndex = 42;
		this.label45.Text = "Trade Type";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(100, 96);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(14, 17);
		this.label43.TabIndex = 40;
		this.label43.Text = "*";
		this.txtIBAN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtIBAN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtIBAN.Location = new System.Drawing.Point(119, 42);
		this.txtIBAN.Name = "txtIBAN";
		this.txtIBAN.Size = new System.Drawing.Size(185, 22);
		this.txtIBAN.TabIndex = 1;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(8, 45);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(36, 17);
		this.label44.TabIndex = 39;
		this.label44.Text = "IBAN";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(1524, 20);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(14, 17);
		this.label42.TabIndex = 37;
		this.label42.Text = "*";
		this.label40.AutoSize = true;
		this.label40.BackColor = System.Drawing.Color.Transparent;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.Red;
		this.label40.Location = new System.Drawing.Point(458, 128);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(14, 17);
		this.label40.TabIndex = 10;
		this.label40.Text = "*";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.Red;
		this.label39.Location = new System.Drawing.Point(1524, 38);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(14, 17);
		this.label39.TabIndex = 34;
		this.label39.Text = "*";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.Red;
		this.label38.Location = new System.Drawing.Point(1528, 65);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(14, 17);
		this.label38.TabIndex = 33;
		this.label38.Text = "*";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(1592, 130);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(14, 17);
		this.label36.TabIndex = 32;
		this.label36.Text = "*";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Red;
		this.label35.Location = new System.Drawing.Point(745, 49);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(14, 17);
		this.label35.TabIndex = 12;
		this.label35.Text = "*";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(738, 76);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(14, 17);
		this.label34.TabIndex = 13;
		this.label34.Text = "*";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Red;
		this.label33.Location = new System.Drawing.Point(751, 17);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(14, 17);
		this.label33.TabIndex = 11;
		this.label33.Text = "*";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(88, 124);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(14, 17);
		this.label32.TabIndex = 28;
		this.label32.Text = "*";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Red;
		this.label31.Location = new System.Drawing.Point(89, 152);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(14, 17);
		this.label31.TabIndex = 27;
		this.label31.Text = "*";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(82, 20);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(14, 17);
		this.label30.TabIndex = 26;
		this.label30.Text = "*";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(463, 17);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(14, 17);
		this.label29.TabIndex = 6;
		this.label29.Text = "*";
		this.cmbPortOfDischarge.AllowNull = (DefaultableBoolean)1;
		this.cmbPortOfDischarge.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPortOfDischarge.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPortOfDischarge.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPortOfDischarge).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbPortOfDischarge.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPortOfDischarge).Location = new System.Drawing.Point(478, 125);
		((System.Windows.Forms.Control)(object)this.cmbPortOfDischarge).Name = "cmbPortOfDischarge";
		((System.Windows.Forms.Control)(object)this.cmbPortOfDischarge).Size = new System.Drawing.Size(178, 22);
		((System.Windows.Forms.Control)(object)this.cmbPortOfDischarge).TabIndex = 10;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(350, 128);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(112, 17);
		this.label27.TabIndex = 24;
		this.label27.Text = "Port Of Discharge";
		this.cmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Location = new System.Drawing.Point(1588, 130);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Name = "cmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Size = new System.Drawing.Size(67, 22);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabIndex = 11;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(1512, 134);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(83, 17);
		this.label26.TabIndex = 22;
		this.label26.Text = "Loading Port";
		this.txtfidate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfidate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtfidate.Location = new System.Drawing.Point(119, 120);
		this.txtfidate.Name = "txtfidate";
		this.txtfidate.Size = new System.Drawing.Size(185, 25);
		this.txtfidate.TabIndex = 4;
		this.txtfiuniqueNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfiuniqueNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfiuniqueNo.Location = new System.Drawing.Point(118, 149);
		this.txtfiuniqueNo.Name = "txtfiuniqueNo";
		this.txtfiuniqueNo.Size = new System.Drawing.Size(185, 22);
		this.txtfiuniqueNo.TabIndex = 5;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(7, 152);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(84, 17);
		this.label4.TabIndex = 4;
		this.label4.Text = "FI Unique No";
		this.cmbBank.AllowNull = (DefaultableBoolean)1;
		this.cmbBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance124).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBank).Location = new System.Drawing.Point(119, 17);
		((System.Windows.Forms.Control)(object)this.cmbBank).Name = "cmbBank";
		((System.Windows.Forms.Control)(object)this.cmbBank).Size = new System.Drawing.Size(185, 22);
		((System.Windows.Forms.Control)(object)this.cmbBank).TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 20);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(74, 17);
		this.label2.TabIndex = 2;
		this.label2.Text = "Bank Name";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(8, 123);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 17);
		this.label5.TabIndex = 6;
		this.label5.Text = "FI Date";
		this.txtNetWeightkg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightkg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightkg.Location = new System.Drawing.Point(1541, 44);
		this.txtNetWeightkg.Name = "txtNetWeightkg";
		this.txtNetWeightkg.Size = new System.Drawing.Size(98, 22);
		this.txtNetWeightkg.TabIndex = 9;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1437, 45);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(93, 17);
		this.label11.TabIndex = 18;
		this.label11.Text = "Net Weight Kg";
		this.txtAmountRs.BackColor = System.Drawing.Color.White;
		this.txtAmountRs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountRs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountRs.Location = new System.Drawing.Point(1541, 71);
		this.txtAmountRs.Name = "txtAmountRs";
		this.txtAmountRs.ReadOnly = true;
		this.txtAmountRs.Size = new System.Drawing.Size(98, 22);
		this.txtAmountRs.TabIndex = 8;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(1464, 72);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(74, 17);
		this.label10.TabIndex = 16;
		this.label10.Text = "Amount Rs.";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(1537, 23);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(102, 22);
		this.txtExchangeRate.TabIndex = 7;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1433, 22);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(93, 17);
		this.label8.TabIndex = 14;
		this.label8.Text = "Exchange Rate";
		this.txtfivalue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfivalue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfivalue.Location = new System.Drawing.Point(765, 73);
		this.txtfivalue.Name = "txtfivalue";
		this.txtfivalue.Size = new System.Drawing.Size(178, 22);
		this.txtfivalue.TabIndex = 13;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(665, 76);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(52, 17);
		this.label9.TabIndex = 12;
		this.label9.Text = "FI Value";
		this.cmbconsgneeocuntry.AllowNull = (DefaultableBoolean)1;
		this.cmbconsgneeocuntry.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbconsgneeocuntry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbconsgneeocuntry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbconsgneeocuntry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbconsgneeocuntry.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbconsgneeocuntry).Location = new System.Drawing.Point(478, 99);
		((System.Windows.Forms.Control)(object)this.cmbconsgneeocuntry).Name = "cmbconsgneeocuntry";
		((System.Windows.Forms.Control)(object)this.cmbconsgneeocuntry).Size = new System.Drawing.Size(178, 22);
		((System.Windows.Forms.Control)(object)this.cmbconsgneeocuntry).TabIndex = 9;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(350, 102);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(118, 17);
		this.label12.TabIndex = 20;
		this.label12.Text = "Consignee Country";
		this.cmbFICode.AllowNull = (DefaultableBoolean)1;
		this.cmbFICode.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFICode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFICode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbFICode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFICode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFICode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFICode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.cmbFICode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFICode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbFICode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFICode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance97).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFICode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbFICode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFICode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFICode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbFICode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFICode).Location = new System.Drawing.Point(765, 46);
		((System.Windows.Forms.Control)(object)this.cmbFICode).Name = "cmbFICode";
		((System.Windows.Forms.Control)(object)this.cmbFICode).Size = new System.Drawing.Size(178, 22);
		((System.Windows.Forms.Control)(object)this.cmbFICode).TabIndex = 12;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(665, 49);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(72, 17);
		this.label7.TabIndex = 10;
		this.label7.Text = "FI Currency";
		this.cmbDeliverymode.AllowNull = (DefaultableBoolean)1;
		this.cmbDeliverymode.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDeliverymode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDeliverymode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDeliverymode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.cmbDeliverymode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance113).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDeliverymode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbDeliverymode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDeliverymode).Location = new System.Drawing.Point(765, 14);
		((System.Windows.Forms.Control)(object)this.cmbDeliverymode).Name = "cmbDeliverymode";
		((System.Windows.Forms.Control)(object)this.cmbDeliverymode).Size = new System.Drawing.Size(178, 22);
		((System.Windows.Forms.Control)(object)this.cmbDeliverymode).TabIndex = 11;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(662, 17);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(93, 17);
		this.label6.TabIndex = 8;
		this.label6.Text = "Delivery Mode";
		this.cmbConsignee.AllowNull = (DefaultableBoolean)1;
		this.cmbConsignee.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbConsignee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbConsignee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbConsignee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance78;
		((SpecialBoxBase)((UltraGridBase)this.cmbConsignee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance84).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbConsignee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbConsignee).Location = new System.Drawing.Point(478, 14);
		((System.Windows.Forms.Control)(object)this.cmbConsignee).Name = "cmbConsignee";
		((System.Windows.Forms.Control)(object)this.cmbConsignee).Size = new System.Drawing.Size(178, 22);
		((System.Windows.Forms.Control)(object)this.cmbConsignee).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbConsignee).Leave += new System.EventHandler(cmbCustomer_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(350, 17);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(108, 17);
		this.label3.TabIndex = 0;
		this.label3.Text = "Consignee Name";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 33);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(968, 36);
		this.panel3.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Trebuchet MS", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(11, 7);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(220, 27);
		this.label1.TabIndex = 1;
		this.label1.Text = "Financial Instruments";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(968, 30);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.BtnNew, this.btnSave, this.btnAttachment, this.btnUpdate });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(968, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(35, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.tabPage2.Controls.Add(this.ctrlGrdBar1);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(974, 267);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(922, 1);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(48, 29);
		this.ctrlGrdBar1.TabIndex = 1;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(968, 195);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 45;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel10.Controls.Add(this.panel13);
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(968, 261);
		this.panel10.TabIndex = 0;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 66);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(968, 195);
		this.panel13.TabIndex = 2;
		this.panel12.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel12.Controls.Add(this.label37);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 28);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(968, 38);
		this.panel12.TabIndex = 1;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.White;
		this.label37.Location = new System.Drawing.Point(15, 6);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 24);
		this.label37.TabIndex = 2;
		this.label37.Text = "History";
		this.panel11.Controls.Add(this.toolStrip2);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(968, 28);
		this.panel11.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripButton1, this.toolStripButton6, this.toolStripSeparator1 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(968, 27);
		this.toolStrip2.TabIndex = 53;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(55, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripButton6.Image = (System.Drawing.Image)resources.GetObject("toolStripButton6.Image");
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(56, 24);
		this.toolStripButton6.Text = "&Print";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(982, 293);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ExpfrmFinancialInsturment";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ExpfrmFinancialInsturment_FormClosing);
		base.Load += new System.EventHandler(ExpfrmEformRegistration_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExpfrmEformRegistration_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentUtilization).EndInit();
		this.panel7.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBankReerence).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCommodity).EndInit();
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).EndInit();
		this.panel14.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbpaymentmode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTradeType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPortOfDischarge).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbconsgneeocuntry).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFICode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDeliverymode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbConsignee).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
