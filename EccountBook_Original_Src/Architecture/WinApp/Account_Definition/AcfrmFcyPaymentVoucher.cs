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
using Architecture.BLL.Import;
using Architecture.BLL.Reports.Accounts;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Import;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class AcfrmFcyPaymentVoucher : Form
{
	private bool DoHaveSaveRight;

	private bool DoHavePrintRights;

	private bool DoHaveUpdateRights;

	private bool DoHaveCanViewAllRecordRights;

	private int Id;

	private double PreviousBalance;

	private int VoucherHeadId;

	private int BrancheId;

	private int ProjectId;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtLCOrder = new DataTable();

	private DataTable table = new DataTable();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private int updateDetailIndex;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel5;

	internal GridEX grdFCBank;

	private Button btnAddinGrid;

	private Label label36;

	private Label label37;

	private TextBox txtpkramount;

	private Label label34;

	private Label label35;

	private Label label32;

	private Label label33;

	private TextBox txtfcamount;

	private Panel panel3;

	private GroupBox groupBox1;

	internal UltraCombo comSuplier;

	private DateTimePicker txtdate;

	private Label label31;

	private Label label30;

	private Label label29;

	private Label label28;

	private Label label27;

	private Label label26;

	private Label label25;

	private Label label24;

	private Label label23;

	private Label label22;

	private Label label21;

	private Label label20;

	private Label label19;

	private Label label6;

	private Label label5;

	private Label label18;

	private Label label4;

	private TextBox txtexchangeRate;

	private Label label17;

	private TextBox txtNetAmount;

	private Label label16;

	private TextBox txtbankcharges;

	private Label label14;

	private TextBox txtFCGrosamount;

	private Label label13;

	private TextBox txtNetAmountFC;

	private Label label12;

	private TextBox txtbankDrAmount;

	private Label label11;

	private TextBox txtbankfbpNo;

	private Label label10;

	internal UltraCombo cmbcurrency;

	private Label label9;

	internal UltraCombo cmbbankAcc;

	private Label label8;

	internal UltraCombo cmbInvoiceNo;

	private Label label7;

	internal UltraCombo cmbLCOrder;

	private Label label3;

	private Label label2;

	private Label label1;

	private TextBox txtdocNumber;

	private Panel panel6;

	private Label label15;

	private Panel panel7;

	private ToolStrip btnAttachments;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private TabPage tabPage2;

	private Panel panel8;

	private Panel panel9;

	private Label label38;

	internal GridEX DataGridHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnUpdate;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripSeparator toolStripSeparator2;

	internal UltraCombo cmbranches;

	private Label label39;

	internal UltraCombo cmbproject;

	private Label label40;

	private Label label42;

	private Label label41;

	internal UltraCombo cmbPaymentTerm;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnRefresh;

	internal UltraCombo CmbFcyBankChargesTypeAc;

	private Label label43;

	private Label label44;

	private Label label45;

	private TextBox txtFcyBankChargesRsAmount;

	private TextBox txtRemarks;

	private Label label46;

	private TextBox txtRemarksDetail;

	private Label label47;

	private ToolStripButton btnSlip;

	private Button button1;

	internal UltraCombo CmbChargesType;

	private Label label48;

	private Label label49;

	private Label label50;

	private TextBox txtInvoiceAmount;

	private Label label51;

	private TextBox txtLocalAmount;

	private TextBox txtForeinchargesPkr;

	private Label label52;

	private TextBox txtExportvoucherRate;

	internal UltraCombo CmbChargesAccountType;

	internal UltraCombo CmbGainLossAc;

	private Label label55;

	private Label label54;

	private TextBox txtBalanceAmount;

	private Label label53;

	private TextBox txtRecAmount;

	public AcfrmFcyPaymentVoucher(UserAccount user)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void Acfrmfcbankreceipt_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = "frmFcyPaymentVoucher",
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
				DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
			}
			btnsave.Enabled = DoHaveSaveRight;
			btnPrint.Enabled = DoHavePrintRights;
			btnUpdate.Enabled = DoHaveUpdateRights;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			cmbcurrency.ReadOnly = true;
			txtdate.Focus();
			GenerateDocumentNo();
			BankAccountDr();
			GainLossAccountFill();
			CurrencyCOde();
			ChargerType();
			PaymentTermFill();
			CombBranchFill();
			CombProjectFill();
			ChargesTypeFill();
			dtViewAttachments.Columns.Add("AttchmentPath");
			table.Columns.Add("TypeId");
			table.Columns.Add("Type");
			table.Columns.Add("ChargesTypeId");
			table.Columns.Add("ChargesType");
			table.Columns.Add("Remarks");
			table.Columns.Add("FC-Amount", typeof(double));
			table.Columns.Add("Rs-Amount", typeof(double));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CombBranchFill()
	{
		try
		{
			DataTable BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 0)
			{
				BrancheId = Conversion.ToInt(BranchList.Rows[0]["Id"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CombProjectFill()
	{
		try
		{
			DataTable dt = CommonServices.ProjectServiceBind();
			if (dt.Rows.Count > 0 && dt != null)
			{
				ProjectId = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTerm()
	{
		try
		{
			if (((Control)(object)cmbPaymentTerm).Text == "Advanced Payment")
			{
				cmbLCOrder.ReadOnly = false;
				cmbInvoiceNo.ReadOnly = true;
				cmbcurrency.ReadOnly = false;
				((Control)(object)cmbInvoiceNo).Text = "";
				txtInvoiceAmount.Text = "0";
				txtBalanceAmount.Text = "0";
				txtRecAmount.Text = "0";
				txtExportvoucherRate.Text = "0";
				GetExportCustomerByAdvancePayment();
				LcOrder();
			}
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				cmbInvoiceNo.ReadOnly = false;
				cmbLCOrder.ReadOnly = true;
				((Control)(object)cmbLCOrder).Text = "";
				cmbcurrency.ReadOnly = true;
				CustomersFillByInvoicePayment();
				InvoiceNo();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LcOrder()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		try
		{
			DataTable dtLcOrder = ImLcOrder.GetLcOrderNoBySupplierCustomerId(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				SupplierCustomerId = Conversion.ToInt(comSuplier.Value)
			});
			if (dtLcOrder.Rows.Count > 0)
			{
				DDL.BindDDL(dtLcOrder, cmbLCOrder, "Id", "LcOrderNo", "Contract No", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetExportCustomerByAdvancePayment()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtLcOrder = ImLcOrder.GetImportCustomerByAdvancePayment(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtLcOrder.Rows.Count > 0)
			{
				DDL.BindDDL(dtLcOrder, comSuplier, "SupplierCustomerId", "CompanyName", "Supplier Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("PaymentTerm");
			dt.Rows.Add(1, "Invoice Payment");
			dt.Rows.Add(2, "Advanced Payment");
			DDL.BindDDLNew(dt, cmbPaymentTerm, "Id", "PaymentTerm", "PaymentTerms", false);
			((UltraGridBase)cmbPaymentTerm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChargesTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ChargesType");
			dt.Rows.Add(1, "Local");
			dt.Rows.Add(2, "Foreign");
			DDL.BindDDLNew(dt, CmbChargesType, "Id", "ChargesType", "ChargesType", false);
			((UltraGridBase)CmbChargesType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomersFillByInvoicePayment()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtInvoice = ImInvoice.GetSupplierForFcyPaymentVoucher(new ImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtInvoice.Rows.Count > 0)
			{
				DDL.BindDDL(dtInvoice, comSuplier, "SupplierCustomerId", "CompanyName", "Supplier Name", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)comSuplier).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceNo()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				DataTable dtInvoice = ImInvoice.GetInvoiceNoForFcyPaymentVoucher(new ImInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					SupplierCustomerId = Conversion.ToInt(comSuplier.Value)
				});
				if (dtInvoice.Rows.Count > 0)
				{
					DDL.BindDDL(dtInvoice, cmbInvoiceNo, "Id", "InvoiceNo", "InvoiceNo", ZeroIndex: false);
					return;
				}
				((Control)(object)cmbInvoiceNo).Text = string.Empty;
				((UltraGridBase)cmbInvoiceNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankAccountDr()
	{
		try
		{
			DataTable dtBankAc = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtBankAc.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			for (int i = 0; i < dtBankAc.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtBankAc.Rows[i]["AccountTypeId"]) == 15)
				{
					dt.Rows.Add(dtBankAc.Rows[i]["Id"], dtBankAc.Rows[i]["AccountTitle"]);
				}
			}
			DDL.BindDDL(dt, cmbbankAcc, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GainLossAccountFill()
	{
		try
		{
			DataTable dtAccount = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtAccount.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			for (int i = 0; i < dtAccount.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtAccount.Rows[i]["AccountTypeId"]) != 15 && Convert.ToInt32(dtAccount.Rows[i]["AccountTypeId"]) != 11 && Convert.ToInt32(dtAccount.Rows[i]["AccountTypeId"]) != 12 && Convert.ToInt32(dtAccount.Rows[i]["AccountTypeId"]) != 2 && Convert.ToInt32(dtAccount.Rows[i]["AccountTypeId"]) != 4)
				{
					dt.Rows.Add(dtAccount.Rows[i]["Id"], dtAccount.Rows[i]["AccountTitle"]);
				}
			}
			DDL.BindDDL(dt, CmbGainLossAc, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyCOde()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			MultiCurrency val = new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtCurrency = new DataTable();
			dtCurrency = MultiCurrency.GetAll(val);
			if (dtCurrency.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCurrency, cmbcurrency, "Id", "CurrencyCode", "Fcy Code", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChargerType()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			ExImProceedsChargesType val = new ExImProceedsChargesType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtchargestype = new DataTable();
			dtchargestype = ExImProceedsChargesType.Getall(val);
			if (dtchargestype.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtchargestype, CmbChargesAccountType, "Id", "ProceedsChargesTypeCode", "Charges Type", false);
				DDL.BindDDLNew(dtchargestype, CmbFcyBankChargesTypeAc, "Id", "ProceedsChargesTypeCode", "Charges Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetChargesTypeByCustomerId(int ID)
	{
	}

	private void GenerateDocumentNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			int Code = ExImFCBankPaymensts.GenerateCode(new ExImFCBankPaymensts
			{
				DocumentTypeId = 204,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (Code > 0)
			{
				txtdocNumber.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbPaymentTerm_Leave(object sender, EventArgs e)
	{
		((Control)(object)comSuplier).Text = string.Empty;
		((UltraGridBase)comSuplier).DataSource = null;
		PaymentTerm();
	}

	private void txtbankcharges_Leave(object sender, EventArgs e)
	{
		NetAmountFC();
	}

	private void NetAmountFC()
	{
		try
		{
			if (txtbankcharges.Text != string.Empty && txtFCGrosamount.Text != string.Empty)
			{
				double a = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double b4 = Conversion.ToDouble((object)txtbankcharges.Text);
				if (b4 > a)
				{
					txtbankcharges.Text = "0";
					throw new Exception("FCY Bank Charges not gratter than FCY Amount");
				}
				txtNetAmountFC.Text = (a - b4).ToString("0,0");
			}
			if (txtexchangeRate.Text != string.Empty && txtfcamount.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtexchangeRate.Text);
				double b3 = Conversion.ToDouble((object)txtfcamount.Text);
				txtpkramount.Text = (num * b3).ToString("0,0");
			}
			if (txtexchangeRate.Text != string.Empty && txtbankcharges.Text != string.Empty)
			{
				double num2 = Conversion.ToDouble((object)txtexchangeRate.Text);
				double b2 = Conversion.ToDouble((object)txtbankcharges.Text);
				txtFcyBankChargesRsAmount.Text = (num2 * b2).ToString("0,0");
			}
			else
			{
				txtFcyBankChargesRsAmount.Text = "";
				txtbankDrAmount.Text = "";
			}
			if (!(txtFcyBankChargesRsAmount.Text != string.Empty) || !(txtNetAmount.Text != string.Empty))
			{
				return;
			}
			double num3 = Conversion.ToDouble((object)txtNetAmount.Text);
			double b = Conversion.ToDouble((object)txtFcyBankChargesRsAmount.Text);
			double c = num3 - b;
			txtbankDrAmount.Text = c.ToString("0,0");
			if (grdFCBank.GetRows().Count() <= 0)
			{
				return;
			}
			double Fcamount = 0.0;
			double RsAmount = 0.0;
			GridEXRow[] rows = grdFCBank.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (r.Cells["Type"].Text == "Foreign")
				{
					Fcamount += Conversion.ToDouble((object)r.Cells["FC-Amount"].Text);
				}
				if (Conversion.ToDouble((object)r.Cells["Rs-Amount"].Text.ToString()) > 0.0)
				{
					RsAmount += Conversion.ToDouble((object)r.Cells["Rs-Amount"].Text);
				}
			}
			double d = c - RsAmount;
			txtbankDrAmount.Text = d.ToString("0,0");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtbankcharges_TextChanged(object sender, EventArgs e)
	{
		NetAmountFC();
	}

	private void txtexchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtFCGrosamount.Text != string.Empty && txtexchangeRate.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double b2 = Conversion.ToDouble((object)txtexchangeRate.Text);
				double c = num * b2;
				txtNetAmount.Text = c.ToString("0,0");
				txtbankDrAmount.Text = c.ToString("0,0");
			}
			else
			{
				txtNetAmount.Text = "0";
				txtbankDrAmount.Text = "0";
			}
			if (txtFcyBankChargesRsAmount.Text != string.Empty && txtNetAmount.Text != string.Empty)
			{
				double num2 = Conversion.ToDouble((object)txtNetAmount.Text);
				double b = Conversion.ToDouble((object)txtFcyBankChargesRsAmount.Text);
				txtbankDrAmount.Text = (num2 - b).ToString("0,0");
			}
			else
			{
				txtbankDrAmount.Text = "0";
			}
			NetAmountFC();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtexchangeRate_Leave(object sender, EventArgs e)
	{
		try
		{
			if (txtFCGrosamount.Text != string.Empty && txtexchangeRate.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double b = Conversion.ToDouble((object)txtexchangeRate.Text);
				txtNetAmount.Text = (num * b).ToString("0,0");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankCreditAmount()
	{
		try
		{
			if (txtFCGrosamount.Text != string.Empty && txtFcyBankChargesRsAmount.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtNetAmount.Text);
				double b = Conversion.ToDouble((object)txtFcyBankChargesRsAmount.Text);
				double c = double.Parse(grdFCBank.GetTotal(grdFCBank.RootTable.Columns["Rs-Amount"], (AggregateFunction)2).ToString());
				txtbankDrAmount.Text = (num - b - c).ToString("0,0");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(Id, 204);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReportVoucher102()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.VoucherReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 204,
				Id = VoucherHeadId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress);
			val.RptPerameter("@CompanyName", UserAccount.CompName);
			val.ShowReportWithDataTable(dt, "102-AcRptPaymentReceiptsVoucherSlip.rpt");
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
			double ForeignAmount = 0.0;
			double LocalAmount = 0.0;
			if (((UltraGridBase)CmbChargesAccountType).ActiveRow == null)
			{
				MessageBox.Show("Please Select AccountTitle");
				CmbChargesAccountType.Focus();
				return;
			}
			if (txtpkramount.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter PKR Amount");
				txtpkramount.Focus();
				return;
			}
			if (((UltraGridBase)CmbChargesType).ActiveRow == null)
			{
				MessageBox.Show("Please select Chargestype");
				CmbChargesType.Focus();
				return;
			}
			double Fcamount = Conversion.ToDouble((object)txtfcamount.Text);
			double RsAmount = Conversion.ToDouble((object)txtpkramount.Text);
			GridEXRow[] rows = grdFCBank.GetRows();
			foreach (GridEXRow r in rows)
			{
				Fcamount += Conversion.ToDouble((object)r.Cells["FC-Amount"].Text);
				RsAmount += Conversion.ToDouble((object)r.Cells["Rs-Amount"].Text);
			}
			if (Fcamount >= Conversion.ToDouble((object)txtNetAmountFC.Text))
			{
				throw new Exception("Fcy Bank Charges Amount not gratter than FcyNetAmount");
			}
			if (RsAmount >= Conversion.ToDouble((object)txtbankDrAmount.Text))
			{
				throw new Exception("Bank Charges Amount not gratter than Bank Debit Amount");
			}
			if (((Control)(object)CmbChargesType).Text == "Foreign" && (txtfcamount.Text.Trim() == string.Empty || txtfcamount.Text.Trim() == "0"))
			{
				throw new Exception("FcAmount field required");
			}
			table.Rows.Add(CmbChargesType.Value, ((Control)(object)CmbChargesType).Text.Trim(), CmbChargesAccountType.Value, ((Control)(object)CmbChargesAccountType).Text, txtRemarksDetail.Text.Trim(), Conversion.ToDouble((object)txtfcamount.Text.Trim()), Conversion.ToDouble((object)txtpkramount.Text.Trim()));
			grdFCBank.DataSource = table;
			grdFCBank.RetrieveStructure();
			gridsettings();
			rows = grdFCBank.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToString((object)item.Cells["Type"].Text) == "Foreign")
				{
					ForeignAmount += Conversion.ToDouble(item.Cells["Rs-Amount"].Value);
				}
				else
				{
					LocalAmount += Conversion.ToDouble(item.Cells["Rs-Amount"].Value);
				}
			}
			txtForeinchargesPkr.Text = ForeignAmount.ToString();
			txtLocalAmount.Text = LocalAmount.ToString();
			CmbChargesAccountType.Value = "";
			txtfcamount.Text = "";
			txtpkramount.Text = "";
			txtRemarksDetail.Text = "";
			BankCreditAmount();
			CmbChargesAccountType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			double ForeignAmount = 0.0;
			double LocalAmount = 0.0;
			if (((UltraGridBase)CmbChargesAccountType).ActiveRow == null)
			{
				MessageBox.Show("Please Select AccountTitle");
				CmbChargesAccountType.Focus();
				return;
			}
			if (txtpkramount.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter PKR Amount");
				txtpkramount.Focus();
				return;
			}
			if (((UltraGridBase)CmbChargesType).ActiveRow == null)
			{
				MessageBox.Show("Please select Chargestype");
				CmbChargesType.Focus();
				return;
			}
			double Fcamount = Conversion.ToDouble((object)txtfcamount.Text);
			double RsAmount = Conversion.ToDouble((object)txtpkramount.Text);
			GridEXRow[] rows = grdFCBank.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				Fcamount += Conversion.ToDouble((object)r2.Cells["FC-Amount"].Text);
				RsAmount += Conversion.ToDouble((object)r2.Cells["Rs-Amount"].Text);
			}
			if (Fcamount >= Conversion.ToDouble((object)txtNetAmountFC.Text))
			{
				throw new Exception("Fcy Bank Charges Amount not gratter than FcyNetAmount");
			}
			if (RsAmount >= Conversion.ToDouble((object)txtbankDrAmount.Text))
			{
				throw new Exception("Bank Charges Amount not gratter than Bank Debit Amount");
			}
			if (((Control)(object)CmbChargesType).Text == "Foreign" && (txtfcamount.Text.Trim() == string.Empty || txtfcamount.Text.Trim() == "0"))
			{
				throw new Exception("FcAmount field required");
			}
			table.Rows[updateDetailIndex]["TypeId"] = CmbChargesType.Value;
			table.Rows[updateDetailIndex]["Type"] = ((Control)(object)CmbChargesType).Text;
			table.Rows[updateDetailIndex]["ChargesTypeId"] = CmbChargesAccountType.Value;
			table.Rows[updateDetailIndex]["ChargesType"] = ((Control)(object)CmbChargesAccountType).Text;
			table.Rows[updateDetailIndex]["Remarks"] = txtRemarksDetail.Text;
			table.Rows[updateDetailIndex]["FC-Amount"] = txtfcamount.Text;
			table.Rows[updateDetailIndex]["Rs-Amount"] = txtpkramount.Text;
			grdFCBank.DataSource = table;
			grdFCBank.RetrieveStructure();
			gridsettings();
			resetdetail();
			BankCreditAmount();
			btnAddinGrid.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			rows = grdFCBank.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToString((object)r.Cells["Type"].Text) == "Foreign")
				{
					ForeignAmount += Conversion.ToDouble(r.Cells["Rs-Amount"].Value);
				}
				else
				{
					LocalAmount += Conversion.ToDouble(r.Cells["Rs-Amount"].Value);
				}
			}
			txtForeinchargesPkr.Text = ForeignAmount.ToString();
			txtLocalAmount.Text = LocalAmount.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		resetdetail();
		btnAddinGrid.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void grdFCBank_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdFCBank.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				CmbChargesType.Value = item.Cells["TypeId"].Value.ToString();
				((Control)(object)CmbChargesType).Text = item.Cells["Type"].Text.ToString();
				CmbChargesAccountType.Value = item.Cells["ChargesTypeId"].Value.ToString();
				((Control)(object)CmbChargesAccountType).Text = item.Cells["ChargesType"].Text.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Text.ToString();
				txtfcamount.Text = item.Cells["FC-Amount"].Value.ToString();
				txtpkramount.Text = item.Cells["Rs-Amount"].Value.ToString();
				CmbChargesAccountType.Focus();
				btnAddinGrid.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				grdFCBank.DataSource = table;
				grdFCBank.RetrieveStructure();
				gridsettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsettings()
	{
		try
		{
			grdFCBank.RootTable.Columns["TypeId"].Visible = false;
			grdFCBank.RootTable.Columns["ChargesTypeId"].Visible = false;
			grdFCBank.RootTable.Columns["ChargesType"].FilterEditType = (FilterEditType)1;
			grdFCBank.RootTable.Columns["FC-Amount"].FilterEditType = (FilterEditType)1;
			grdFCBank.RootTable.Columns["Rs-Amount"].FilterEditType = (FilterEditType)1;
			grdFCBank.RootTable.Columns["ChargesType"].EditType = (EditType)0;
			grdFCBank.RootTable.Columns["FC-Amount"].EditType = (EditType)0;
			grdFCBank.RootTable.Columns["Rs-Amount"].EditType = (EditType)0;
			grdFCBank.RootTable.Columns["ChargesType"].Width = 300;
			grdFCBank.RootTable.Columns["FC-Amount"].Width = 100;
			grdFCBank.RootTable.Columns["Rs-Amount"].Width = 100;
			grdFCBank.RootTable.Columns["Rs-Amount"].AggregateFunction = (AggregateFunction)2;
			grdFCBank.RootTable.Columns["FC-Amount"].AggregateFunction = (AggregateFunction)2;
			grdFCBank.RootTable.Columns["FC-Amount"].FormatString = "0,0";
			grdFCBank.RootTable.Columns["Rs-Amount"].FormatString = "0,0";
			grdFCBank.RootTable.Columns["FC-Amount"].TotalFormatString = "#,##0.##";
			grdFCBank.RootTable.Columns["Rs-Amount"].TotalFormatString = "#,##0.##";
			grdFCBank.RootTable.Columns["FC-Amount"].TextAlignment = (TextAlignment)3;
			grdFCBank.RootTable.Columns["Rs-Amount"].TextAlignment = (TextAlignment)3;
			grdFCBank.RootTable.Columns.Add("Delete");
			grdFCBank.RootTable.Columns["Delete"].Key = "Delete";
			grdFCBank.RootTable.Columns["Delete"].Caption = "X";
			grdFCBank.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFCBank.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdFCBank.RootTable.Columns["Delete"].ButtonText = "X";
			grdFCBank.RootTable.Columns["Delete"].Width = 20;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtbankfbpNo.Text.Trim() == string.Empty || txtbankfbpNo.Text.Trim() == "0")
		{
			MessageBox.Show("FBP Bank Field Required");
			txtbankfbpNo.Focus();
			return false;
		}
		if (txtFCGrosamount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtFCGrosamount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Fc Gross Field Required");
			txtFCGrosamount.Focus();
			return false;
		}
		if (txtNetAmountFC.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtNetAmountFC.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Fc Net Amount Field Required");
			txtNetAmountFC.Focus();
			return false;
		}
		if (txtexchangeRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtexchangeRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Exchange Rate Field Required");
			txtexchangeRate.Focus();
			return false;
		}
		if (txtNetAmount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtNetAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Net Amount Field Required");
			txtNetAmount.Focus();
			return false;
		}
		if (((UltraGridBase)comSuplier).ActiveRow == null)
		{
			MessageBox.Show("customer  Field Required");
			comSuplier.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPaymentTerm).ActiveRow == null)
		{
			MessageBox.Show("Payment term Field Required");
			cmbPaymentTerm.Focus();
			return false;
		}
		if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment" && ((UltraGridBase)CmbGainLossAc).ActiveRow == null)
		{
			MessageBox.Show("GainLossAc Field Required");
			CmbGainLossAc.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbPaymentTerm.Value) == 2 && ((Control)(object)cmbLCOrder).Text == string.Empty)
		{
			MessageBox.Show("LC Order Field Required");
			cmbLCOrder.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbPaymentTerm.Value) == 1 && ((Control)(object)cmbInvoiceNo).Text == string.Empty)
		{
			MessageBox.Show("Invoice Field Required");
			cmbInvoiceNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbbankAcc).ActiveRow == null)
		{
			MessageBox.Show("Bank Account (DR) Field Required");
			cmbbankAcc.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcurrency).ActiveRow == null)
		{
			MessageBox.Show("Currency Code Field Required");
			cmbcurrency.Focus();
			return false;
		}
		return true;
	}

	private void formRefresh()
	{
		GenerateDocumentNo();
		BankAccountDr();
		CurrencyCOde();
		ChargerType();
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Expected O, but got Unknown
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (txtbankcharges.Text != string.Empty && txtFCGrosamount.Text != string.Empty)
			{
				double a = Conversion.ToDouble((object)txtFCGrosamount.Text);
				if (Conversion.ToDouble((object)txtbankcharges.Text) > a)
				{
					throw new Exception("FCY Bank Charges not gratter than FCY Amount");
				}
			}
			if (!FormValidation() || MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				double num = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double BalanceAmount = Conversion.ToDouble((object)txtBalanceAmount.Text);
				if (num > BalanceAmount)
				{
					throw new Exception("FCY Amount cannot be greater than Balance Amount please check");
				}
			}
			ExImFCBankPaymensts fcbank = new ExImFCBankPaymensts();
			fcbank.ProjectId = Conversion.ToInt(cmbproject.Value);
			fcbank.BranchId = Conversion.ToInt(cmbranches.Value);
			fcbank.DocumentTypeId = 204;
			fcbank.DocumentNo = Conversion.ToInt((object)txtdocNumber.Text);
			fcbank.DocumentDate = Conversion.ToDateTime((object)txtdate.Value);
			fcbank.SupplierCustomerId = Conversion.ToInt(comSuplier.Value);
			fcbank.ExImLcOrderId = Conversion.ToInt(cmbLCOrder.Value);
			fcbank.ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			fcbank.BankGlDrAc = Conversion.ToInt(cmbbankAcc.Value);
			fcbank.BankFbpNo = Conversion.ToString((object)txtbankfbpNo.Text);
			fcbank.ExmLcPaymentTerm = ((Control)(object)cmbPaymentTerm).Text;
			fcbank.MultiCurrencyId = Conversion.ToInt(cmbcurrency.Value);
			fcbank.FcGrossAmount = Conversion.ToDouble((object)txtFCGrosamount.Text);
			fcbank.FcFBCharges = Conversion.ToDouble((object)txtbankcharges.Text);
			fcbank.FcNetAmount = Conversion.ToDouble((object)txtNetAmountFC.Text);
			fcbank.ExchangeRate = Conversion.ToDouble((object)txtexchangeRate.Text);
			fcbank.NetAmountRs = Conversion.ToDouble((object)txtNetAmount.Text);
			fcbank.BankDrAmount = Conversion.ToDouble((object)txtbankDrAmount.Text);
			fcbank.VoucherHeadId = Conversion.ToInt(CmbGainLossAc.Value);
			fcbank.FcBankChargesTypeAcId = Conversion.ToInt(CmbFcyBankChargesTypeAc.Value);
			fcbank.FcBankChargesAmountRs = Conversion.ToDouble((object)txtFcyBankChargesRsAmount.Text);
			fcbank.VoucherExchangeRate = Conversion.ToDouble((object)txtExportvoucherRate.Text);
			fcbank.Remarks = Conversion.ToString((object)txtRemarks.Text);
			fcbank.OrganizationId = UserAccount.OrganizationId;
			fcbank.CompanyId = UserAccount.CompanyId;
			fcbank.BranchId = BrancheId;
			fcbank.ProjectId = ProjectId;
			fcbank.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			fcbank.EntryUser = UserAccount.ID;
			fcbank.ModifyUser = UserAccount.ID;
			fcbank.EntryDate = DateTime.Now;
			fcbank.ModifyDate = DateTime.Now;
			fcbank.PostDate = DateTime.Now;
			fcbank.ExImFcBankPaymentsDeductionslst = new List<ExImFcBankPaymentsDeductions>();
			GridEXRow[] rows = grdFCBank.GetRows();
			foreach (GridEXRow r in rows)
			{
				ExImFcBankPaymentsDeductions brd = new ExImFcBankPaymentsDeductions();
				brd.Type = Conversion.ToString((object)r.Cells["Type"].Text);
				brd.ExImProceedsChargesTypeId = Conversion.ToInt(r.Cells["ChargesTypeId"].Value);
				brd.ProceedsRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
				brd.FcAmount = Conversion.ToDouble((object)r.Cells["FC-Amount"].Text);
				brd.RsAmount = Conversion.ToDouble((object)r.Cells["Rs-Amount"].Text);
				fcbank.ExImFcBankPaymentsDeductionslst.Add(brd);
			}
			fcbank.AttachmentsList = new List<DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.ScreenName = base.Name;
				fcbank.AttachmentsList.Add(attach);
			}
			int success = ExImFCBankPaymensts.Save(fcbank);
			if (success > 0)
			{
				MessageBox.Show("Record Save Successfully [" + fcbank.DocumentNo + "]");
			}
			RESETMAIN();
			Id = success;
			VoucherHeadIdGet();
			DataTable dt = new DataTable();
			dt = VoucherReports.VoucherReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 204,
				Id = VoucherHeadId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
			}
			else
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress);
				val.RptPerameter("@CompanyName", UserAccount.CompName);
				val.ShowReportWithDataTable(dt, "102-AcRptPaymentReceiptsVoucherSlip.rpt");
			}
			Id = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void bindHistory()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			ReportsParameters br = new ReportsParameters();
			br.OrganizationId = UserAccount.OrganizationId;
			br.CompanyId = UserAccount.CompanyId;
			br.DocumentTypeId = 204;
			br.NoOfRecords = 50;
			br.CanViewAllRecord = DoHaveCanViewAllRecordRights;
			if (!br.CanViewAllRecord)
			{
				br.EntryUser = UserAccount.ID;
			}
			dt = ExImFCBankPaymensts.HistoryForm(br);
			if (dt.Rows.Count > 0)
			{
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
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
			DataGridHistory.RootTable.Columns[2].Visible = false;
			DataGridHistory.RootTable.Columns[0].Visible = false;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns.Add("Voucher");
			DataGridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].Width = 80;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 60;
			DataGridHistory.RootTable.Columns["DocumentDate"].FormatString = "dd-MMM-yyyy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy";
			DataGridHistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BankDrAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BankDrAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BankDrAmount"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["BankDrAmount"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["FcGrossAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcGrossAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcGrossAmount"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["FcGrossAmount"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["FcFBCharges"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcFBCharges"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcFBCharges"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["FcFBCharges"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["FcNetAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcNetAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcNetAmount"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["FcNetAmount"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["NetAmountRs"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetAmountRs"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetAmountRs"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["NetAmountRs"].TotalFormatString = "#,##0.##";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			bindHistory();
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Expected O, but got Unknown
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Expected O, but got Unknown
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0584: Expected O, but got Unknown
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Id == 0)
			{
				throw new Exception("Record Not Update  " + Id);
			}
			if (txtbankcharges.Text != string.Empty && txtFCGrosamount.Text != string.Empty)
			{
				double a = Conversion.ToDouble((object)txtFCGrosamount.Text);
				if (Conversion.ToDouble((object)txtbankcharges.Text) > a)
				{
					throw new Exception("FCY Bank Charges not gratter than FCY Amount");
				}
			}
			if (!FormValidation())
			{
				return;
			}
			ExImFCBankPaymensts fcbank = new ExImFCBankPaymensts();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				fcbank.Id = Id;
			}
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				double num = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double BalanceAmount = Conversion.ToDouble((object)txtBalanceAmount.Text) + PreviousBalance;
				if (num > BalanceAmount)
				{
					throw new Exception("FCY Amount cannot be greater than Balance Amount please check");
				}
			}
			fcbank.ProjectId = Conversion.ToInt(cmbproject.Value);
			fcbank.BranchId = Conversion.ToInt(cmbranches.Value);
			fcbank.DocumentTypeId = 204;
			fcbank.DocumentNo = Conversion.ToInt((object)txtdocNumber.Text);
			fcbank.DocumentDate = Conversion.ToDateTime((object)txtdate.Value);
			fcbank.SupplierCustomerId = Conversion.ToInt(comSuplier.Value);
			fcbank.ExImLcOrderId = Conversion.ToInt(cmbLCOrder.Value);
			fcbank.ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			fcbank.BankGlDrAc = Conversion.ToInt(cmbbankAcc.Value);
			fcbank.BankFbpNo = Conversion.ToString((object)txtbankfbpNo.Text);
			fcbank.ExmLcPaymentTerm = ((Control)(object)cmbPaymentTerm).Text;
			fcbank.MultiCurrencyId = Conversion.ToInt(cmbcurrency.Value);
			fcbank.FcGrossAmount = Conversion.ToDouble((object)txtFCGrosamount.Text);
			fcbank.FcFBCharges = Conversion.ToDouble((object)txtbankcharges.Text);
			fcbank.FcNetAmount = Conversion.ToDouble((object)txtNetAmountFC.Text);
			fcbank.ExchangeRate = Conversion.ToDouble((object)txtexchangeRate.Text);
			fcbank.NetAmountRs = Conversion.ToDouble((object)txtNetAmount.Text);
			fcbank.BankDrAmount = Conversion.ToDouble((object)txtbankDrAmount.Text);
			fcbank.VoucherHeadId = Conversion.ToInt(CmbGainLossAc.Value);
			fcbank.FcBankChargesTypeAcId = Conversion.ToInt(CmbFcyBankChargesTypeAc.Value);
			fcbank.FcBankChargesAmountRs = Conversion.ToDouble((object)txtFcyBankChargesRsAmount.Text);
			fcbank.VoucherExchangeRate = Conversion.ToDouble((object)txtExportvoucherRate.Text);
			fcbank.Remarks = Conversion.ToString((object)txtRemarks.Text);
			fcbank.OrganizationId = UserAccount.OrganizationId;
			fcbank.CompanyId = UserAccount.CompanyId;
			fcbank.BranchId = BrancheId;
			fcbank.ProjectId = ProjectId;
			fcbank.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			fcbank.EntryUser = UserAccount.ID;
			fcbank.ModifyUser = UserAccount.ID;
			fcbank.EntryDate = DateTime.Now;
			fcbank.ModifyDate = DateTime.Now;
			fcbank.PostDate = DateTime.Now;
			fcbank.ExImFcBankPaymentsDeductionslst = new List<ExImFcBankPaymentsDeductions>();
			GridEXRow[] rows = grdFCBank.GetRows();
			foreach (GridEXRow r in rows)
			{
				ExImFcBankPaymentsDeductions brd = new ExImFcBankPaymentsDeductions();
				brd.Type = Conversion.ToString((object)r.Cells["Type"].Text);
				brd.ExImProceedsChargesTypeId = Conversion.ToInt(r.Cells["ChargesTypeId"].Value);
				brd.ProceedsRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
				brd.FcAmount = Conversion.ToDouble((object)r.Cells["FC-Amount"].Text);
				brd.RsAmount = Conversion.ToDouble((object)r.Cells["Rs-Amount"].Text);
				fcbank.ExImFcBankPaymentsDeductionslst.Add(brd);
			}
			fcbank.AttachmentsList = new List<DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.ScreenName = base.Name;
				fcbank.AttachmentsList.Add(attach);
			}
			int success = ExImFCBankPaymensts.Save(fcbank);
			if (success > 0)
			{
				MessageBox.Show("Receord Update Successfully [" + fcbank.DocumentNo + "]");
			}
			RESETMAIN();
			Id = success;
			VoucherHeadIdGet();
			DataTable dt = new DataTable();
			dt = VoucherReports.VoucherReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 204,
				Id = VoucherHeadId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
			}
			else
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress);
				val.RptPerameter("@CompanyName", UserAccount.CompName);
				val.ShowReportWithDataTable(dt, "102-AcRptPaymentReceiptsVoucherSlip.rpt");
			}
			Id = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		try
		{
			Id = ID;
			ExImFCBankPaymensts fcbank = new ExImFCBankPaymensts();
			fcbank = ExImFCBankPaymensts.GetByID(ID);
			if (fcbank != null)
			{
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				VoucherHeadIdGet();
				txtdocNumber.Text = fcbank.DocumentNo.ToString();
				tabControl1.SelectedIndex = 0;
				txtdate.Value = fcbank.DocumentDate;
				((Control)(object)cmbPaymentTerm).Text = fcbank.ExmLcPaymentTerm;
				CustomersFillByInvoicePayment();
				comSuplier.Value = fcbank.SupplierCustomerId;
				txtFCGrosamount.Text = fcbank.FcGrossAmount.ToString("0,0");
				PreviousBalance = Conversion.ToDouble((object)txtFCGrosamount.Text.Trim());
				cmbLCOrder.Value = fcbank.ExImLcOrderId;
				cmbInvoiceNo.Value = fcbank.ExImInvoiceId;
				PaymentTerm();
				cmbbankAcc.Value = fcbank.BankGlDrAc;
				txtbankfbpNo.Text = fcbank.BankFbpNo;
				cmbcurrency.Value = fcbank.MultiCurrencyId;
				txtbankcharges.Text = fcbank.FcFBCharges.ToString("0,0");
				txtNetAmountFC.Text = fcbank.FcNetAmount.ToString("0,0");
				txtexchangeRate.Text = Conversion.ToDouble((object)fcbank.ExchangeRate).ToString();
				txtNetAmount.Text = fcbank.NetAmountRs.ToString("0,0");
				txtbankDrAmount.Text = fcbank.BankDrAmount.ToString("0,0");
				CmbGainLossAc.Value = fcbank.VoucherHeadId;
				txtFcyBankChargesRsAmount.Text = fcbank.FcBankChargesAmountRs.ToString("0,0");
				txtRemarks.Text = Conversion.ToString((object)fcbank.Remarks);
				table.Clear();
				double ForeignAmount = 0.0;
				double LocalAmount = 0.0;
				for (int i = 0; i < fcbank.ExImFcBankPaymentsDeductionslst.Count; i++)
				{
					table.Rows.Add(1, fcbank.ExImFcBankPaymentsDeductionslst[i].Type, fcbank.ExImFcBankPaymentsDeductionslst[i].ExImProceedsChargesTypeId, fcbank.ExImFcBankPaymentsDeductionslst[i].ProceedsChargesTypeCode, fcbank.ExImFcBankPaymentsDeductionslst[i].ProceedsRemarks, fcbank.ExImFcBankPaymentsDeductionslst[i].FcAmount, fcbank.ExImFcBankPaymentsDeductionslst[i].RsAmount);
					if (fcbank.ExImFcBankPaymentsDeductionslst[i].Type == "Foreign")
					{
						ForeignAmount = fcbank.ExImFcBankPaymentsDeductionslst[i].RsAmount;
					}
					else
					{
						LocalAmount = fcbank.ExImFcBankPaymentsDeductionslst[i].RsAmount;
					}
				}
				txtForeinchargesPkr.Text = ForeignAmount.ToString("0,0");
				txtLocalAmount.Text = LocalAmount.ToString("0,0");
				grdFCBank.DataSource = table;
				grdFCBank.RetrieveStructure();
				gridsettings();
				List<DMSAttachments> byID = DMSAttachments.GetByID(int.Parse(Id.ToString()), base.Name);
				AT = new Attachment();
				{
					foreach (DMSAttachments Attachment in byID)
					{
						TempAttachments TA = new TempAttachments();
						TA.Id = 0;
						TA.Path = Attachment.Attachment;
						AT.lst.Add(TA);
					}
					return;
				}
			}
			RESETMAIN();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RESETMAIN()
	{
		try
		{
			Id = 0;
			PreviousBalance = 0.0;
			table.Rows.Clear();
			GenerateDocumentNo();
			txtdate.Focus();
			((Control)(object)comSuplier).Text = "";
			((Control)(object)cmbLCOrder).Text = "";
			((Control)(object)cmbInvoiceNo).Text = "";
			((Control)(object)cmbbankAcc).Text = "";
			txtbankfbpNo.Text = "";
			((Control)(object)cmbcurrency).Text = "";
			txtFCGrosamount.Text = "";
			txtForeinchargesPkr.Text = "";
			txtNetAmountFC.Text = "";
			txtexchangeRate.Text = "";
			txtNetAmount.Text = "";
			txtbankDrAmount.Text = "";
			((Control)(object)CmbFcyBankChargesTypeAc).Text = "";
			txtLocalAmount.Text = "";
			((UltraGridBase)CmbChargesType).Rows[0].Activate();
			txtRemarks.Clear();
			grdFCBank.ClearStructure();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			VoucherHeadId = 0;
			BrancheId = 0;
			ProjectId = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetdetail()
	{
		txtRemarksDetail.Text = "";
		CmbChargesAccountType.Value = "";
		txtfcamount.Text = "";
		txtpkramount.Text = "";
		txtExportvoucherRate.Text = "0";
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		RESETMAIN();
		resetdetail();
		table.Clear();
		btnsave.Visible = true;
		btnUpdate.Visible = false;
		grdFCBank.ClearStructure();
	}

	private void txtpkramount_KeyPress(object sender, KeyPressEventArgs e)
	{
	}

	private void txtfcamount_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
		{
			e.Handled = true;
		}
	}

	private void txtfcamount_TextChanged(object sender, EventArgs e)
	{
		NetAmountFC();
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<DMSAttachments> dMSList = DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters br = new ReportsParameters();
			br.OrganizationId = UserAccount.OrganizationId;
			br.CompanyId = UserAccount.CompanyId;
			br.DocumentTypeId = 204;
			br.CanViewAllRecord = DoHaveCanViewAllRecordRights;
			if (!br.CanViewAllRecord)
			{
				br.EntryUser = UserAccount.ID;
			}
			DataTable dt = ExImFCBankPaymensts.HistoryForm(br);
			if (dt.Rows.Count > 0)
			{
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
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
			BankAccountDr();
			CurrencyCOde();
			ChargerType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		GenerateReportVoucher102();
	}

	private void comSuplier_Leave(object sender, EventArgs e)
	{
		PaymentTerm();
	}

	private void txtFCGrosamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtFCGrosamount.Text != string.Empty && txtexchangeRate.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double b2 = Conversion.ToDouble((object)txtexchangeRate.Text);
				double c = num * b2;
				txtNetAmount.Text = c.ToString("0,0");
				txtbankDrAmount.Text = c.ToString("0,0");
			}
			if (txtFcyBankChargesRsAmount.Text != string.Empty && txtNetAmount.Text != string.Empty)
			{
				double num2 = Conversion.ToDouble((object)txtNetAmount.Text);
				double b = Conversion.ToDouble((object)txtFcyBankChargesRsAmount.Text);
				txtbankDrAmount.Text = (num2 - b).ToString("0,0");
			}
			NetAmountFC();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Acfrmfcbankreceipt_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
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
			if (e.Control && e.KeyCode == Keys.F10)
			{
				AT.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Voucher")
			{
				DataTable dt2 = new DataTable();
				dt2 = VoucherReports.VoucherReport(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 204,
					Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells[2].Value)
				});
				if (dt2 == null || dt2.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
				}
				else
				{
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress);
					val.RptPerameter("@CompanyName", UserAccount.CompName);
					val.ShowReportWithDataTable(dt2, "102-AcRptPaymentReceiptsVoucherSlip.rpt");
				}
			}
			if (e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = ExportReports.ExImRptFCBankReceipts_516(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells[1].Value)
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
				}
				else
				{
					Reporting val2 = new Reporting();
					val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress);
					val2.RptPerameter("@CompanyName", UserAccount.CompName);
					val2.ShowReportWithDataTable(dt, "516-ExImRptFCBankReceipts.rpt");
				}
			}
			if (e.Column.Key == "Edit")
			{
				Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[1].Value.ToString());
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = ExportReports.ExImRptFCBankReceipts_516(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Id
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress);
			val.RptPerameter("@CompanyName", UserAccount.CompName);
			val.ShowReportWithDataTable(dt, "516-ExImRptFCBankReceipts.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		try
		{
			new FcYBankChargesTypes(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceAmountGet()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = ImInvoice.GetInvoiceAmountAndPaidAmount(new ImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbInvoiceNo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				double InvoiceAmt = 0.0;
				double RecAmount = 0.0;
				double BalanceAmt = 0.0;
				InvoiceAmt = Conversion.ToDouble(dt.Rows[0]["InvoiceAmount"]);
				RecAmount = Conversion.ToDouble(dt.Rows[0]["FcyPaidAmount"]);
				BalanceAmt = InvoiceAmt - RecAmount;
				txtInvoiceAmount.Text = InvoiceAmt.ToString("0,0");
				txtRecAmount.Text = RecAmount.ToString("0,0");
				txtBalanceAmount.Text = BalanceAmt.ToString("0,0");
			}
			else
			{
				txtInvoiceAmount.Text = "0";
				txtRecAmount.Text = "0";
				txtBalanceAmount.Text = "0";
				PreviousBalance = 0.0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetExchangeRateandCurrencyFromVoucher()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = ImInvoice.GetExchangeRateandCurrencyFromVoucher(new ImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbInvoiceNo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				txtExportvoucherRate.Text = Conversion.ToDouble(dt.Rows[0]["ExchangeCurrencyRate"]).ToString("#,#.##");
				cmbcurrency.Value = Conversion.ToInt(dt.Rows[0]["MultiCurrencyId"]);
			}
			else
			{
				txtExportvoucherRate.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoiceNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				InvoiceAmountGet();
				GetExchangeRateandCurrencyFromVoucher();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoiceNo_ValueChanged(object sender, EventArgs e)
	{
		InvoiceAmountGet();
		GetExchangeRateandCurrencyFromVoucher();
	}

	private void grdFCBank_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdFCBank.CurrentRow;
				table.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			grdFCBank.UpdateData();
			BankCreditAmount();
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
			RESETMAIN();
			Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[1].Value.ToString());
			ReadById(Id);
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
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Expected O, but got Unknown
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Expected O, but got Unknown
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Expected O, but got Unknown
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a7: Expected O, but got Unknown
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Expected O, but got Unknown
		//IL_0821: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Expected O, but got Unknown
		//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Expected O, but got Unknown
		//IL_0dd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de2: Expected O, but got Unknown
		//IL_7c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c53: Expected O, but got Unknown
		//IL_7c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c6a: Expected O, but got Unknown
		GridEXLayout grdFCBank_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance45 = new Appearance();
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
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.AcfrmFcyPaymentVoucher));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdFCBank = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbGainLossAc = new UltraCombo();
		this.label55 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.txtBalanceAmount = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.txtRecAmount = new System.Windows.Forms.TextBox();
		this.CmbChargesAccountType = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.txtExportvoucherRate = new System.Windows.Forms.TextBox();
		this.txtForeinchargesPkr = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.txtLocalAmount = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.cmbInvoiceNo = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.comSuplier = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtInvoiceAmount = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.CmbChargesType = new UltraCombo();
		this.label48 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.label47 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.cmbPaymentTerm = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label41 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.cmbranches = new UltraCombo();
		this.txtdate = new System.Windows.Forms.DateTimePicker();
		this.label31 = new System.Windows.Forms.Label();
		this.cmbproject = new UltraCombo();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.label40 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.txtpkramount = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.txtfcamount = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.txtexchangeRate = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.txtNetAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtFCGrosamount = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtNetAmountFC = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtbankDrAmount = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtbankfbpNo = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbcurrency = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbbankAcc = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbLCOrder = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtdocNumber = new System.Windows.Forms.TextBox();
		this.txtbankcharges = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.CmbFcyBankChargesTypeAc = new UltraCombo();
		this.txtFcyBankChargesRsAmount = new System.Windows.Forms.TextBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.btnAttachments = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label38 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFCBank).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGainLossAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesAccountType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comSuplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbankAcc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLCOrder).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyBankChargesTypeAc).BeginInit();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		this.btnAttachments.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1284, 668);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1284, 668);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1276, 642);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1270, 636);
		this.panel2.TabIndex = 1;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdFCBank);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 326);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1270, 310);
		this.panel5.TabIndex = 3;
		grdFCBank_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFCBank.DesignTimeLayout = grdFCBank_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFCBank).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFCBank).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFCBank.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFCBank).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdFCBank).Name = "grdFCBank";
		((ScrollableBase)this.grdFCBank).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFCBank).Size = new System.Drawing.Size(1270, 310);
		((System.Windows.Forms.Control)(object)this.grdFCBank).TabIndex = 1;
		this.grdFCBank.TotalRow = (InheritableBoolean)1;
		this.grdFCBank.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFCBank).VisualStyle = (VisualStyle)4;
		this.grdFCBank.ColumnButtonClick += new ColumnActionEventHandler(grdFCBank_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFCBank).DoubleClick += new System.EventHandler(grdFCBank_DoubleClick);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.txtbankcharges);
		this.panel3.Controls.Add(this.label16);
		this.panel3.Controls.Add(this.label27);
		this.panel3.Controls.Add(this.label44);
		this.panel3.Controls.Add(this.label43);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFcyBankChargesTypeAc);
		this.panel3.Controls.Add(this.txtFcyBankChargesRsAmount);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 68);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1270, 258);
		this.panel3.TabIndex = 1;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGainLossAc);
		this.groupBox1.Controls.Add(this.label55);
		this.groupBox1.Controls.Add(this.label54);
		this.groupBox1.Controls.Add(this.txtBalanceAmount);
		this.groupBox1.Controls.Add(this.label53);
		this.groupBox1.Controls.Add(this.txtRecAmount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChargesAccountType);
		this.groupBox1.Controls.Add(this.label52);
		this.groupBox1.Controls.Add(this.txtExportvoucherRate);
		this.groupBox1.Controls.Add(this.txtForeinchargesPkr);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Controls.Add(this.txtLocalAmount);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comSuplier);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.txtInvoiceAmount);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChargesType);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.button1);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add(this.txtRemarksDetail);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add(this.label45);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPaymentTerm);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbranches);
		this.groupBox1.Controls.Add(this.txtdate);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbproject);
		this.groupBox1.Controls.Add(this.btnAddinGrid);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.txtpkramount);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.txtfcamount);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.txtexchangeRate);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtNetAmount);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.txtFCGrosamount);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.txtNetAmountFC);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtbankDrAmount);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtbankfbpNo);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcurrency);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbankAcc);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLCOrder);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtdocNumber);
		this.groupBox1.Location = new System.Drawing.Point(-3, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1271, 248);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Bank Paymenet";
		this.CmbGainLossAc.AllowNull = (DefaultableBoolean)1;
		this.CmbGainLossAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGainLossAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGainLossAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGainLossAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.CmbGainLossAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance124).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGainLossAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Location = new System.Drawing.Point(1071, 156);
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Name = "CmbGainLossAc";
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).TabIndex = 139;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(947, 162);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(76, 15);
		this.label55.TabIndex = 140;
		this.label55.Text = "Gain/Loss Ac";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(947, 133);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(95, 15);
		this.label54.TabIndex = 138;
		this.label54.Text = "Balance Amount";
		this.txtBalanceAmount.BackColor = System.Drawing.Color.White;
		this.txtBalanceAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceAmount.Location = new System.Drawing.Point(1071, 129);
		this.txtBalanceAmount.Name = "txtBalanceAmount";
		this.txtBalanceAmount.ReadOnly = true;
		this.txtBalanceAmount.Size = new System.Drawing.Size(189, 23);
		this.txtBalanceAmount.TabIndex = 137;
		this.txtBalanceAmount.TabStop = false;
		this.txtBalanceAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(638, 133);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(77, 15);
		this.label53.TabIndex = 136;
		this.label53.Text = "Paid Amount";
		this.txtRecAmount.BackColor = System.Drawing.Color.White;
		this.txtRecAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRecAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRecAmount.Location = new System.Drawing.Point(750, 129);
		this.txtRecAmount.Name = "txtRecAmount";
		this.txtRecAmount.ReadOnly = true;
		this.txtRecAmount.Size = new System.Drawing.Size(189, 23);
		this.txtRecAmount.TabIndex = 135;
		this.txtRecAmount.TabStop = false;
		this.txtRecAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbChargesAccountType.AllowNull = (DefaultableBoolean)1;
		this.CmbChargesAccountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChargesAccountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChargesAccountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChargesAccountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).Location = new System.Drawing.Point(131, 208);
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).Name = "CmbChargesAccountType";
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).TabIndex = 18;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(947, 106);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(113, 15);
		this.label52.TabIndex = 134;
		this.label52.Text = "Export Voucher Rate";
		this.txtExportvoucherRate.BackColor = System.Drawing.Color.White;
		this.txtExportvoucherRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExportvoucherRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExportvoucherRate.Location = new System.Drawing.Point(1071, 102);
		this.txtExportvoucherRate.Name = "txtExportvoucherRate";
		this.txtExportvoucherRate.ReadOnly = true;
		this.txtExportvoucherRate.Size = new System.Drawing.Size(189, 23);
		this.txtExportvoucherRate.TabIndex = 133;
		this.txtExportvoucherRate.TabStop = false;
		this.txtExportvoucherRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtForeinchargesPkr.BackColor = System.Drawing.Color.White;
		this.txtForeinchargesPkr.Location = new System.Drawing.Point(129, 103);
		this.txtForeinchargesPkr.Name = "txtForeinchargesPkr";
		this.txtForeinchargesPkr.ReadOnly = true;
		this.txtForeinchargesPkr.Size = new System.Drawing.Size(193, 20);
		this.txtForeinchargesPkr.TabIndex = 132;
		this.txtForeinchargesPkr.Text = "0";
		this.txtForeinchargesPkr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(10, 133);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(81, 15);
		this.label51.TabIndex = 131;
		this.label51.Text = "Local Charges";
		this.txtLocalAmount.BackColor = System.Drawing.Color.White;
		this.txtLocalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLocalAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLocalAmount.Location = new System.Drawing.Point(129, 129);
		this.txtLocalAmount.Name = "txtLocalAmount";
		this.txtLocalAmount.ReadOnly = true;
		this.txtLocalAmount.Size = new System.Drawing.Size(191, 23);
		this.txtLocalAmount.TabIndex = 130;
		this.txtLocalAmount.TabStop = false;
		this.txtLocalAmount.Text = "0";
		this.txtLocalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(328, 133);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(92, 15);
		this.label50.TabIndex = 129;
		this.label50.Text = "Invoice Amount";
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(129, 43);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		this.cmbInvoiceNo.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 4;
		this.cmbInvoiceNo.ValueChanged += new System.EventHandler(cmbInvoiceNo_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Leave += new System.EventHandler(cmbInvoiceNo_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(10, 49);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(58, 15);
		this.label8.TabIndex = 15;
		this.label8.Text = "Invoice # ";
		this.comSuplier.AllowNull = (DefaultableBoolean)1;
		this.comSuplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comSuplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comSuplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comSuplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.comSuplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comSuplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comSuplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSuplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.comSuplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSuplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.comSuplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comSuplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.comSuplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comSuplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comSuplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comSuplier).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comSuplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comSuplier).Location = new System.Drawing.Point(1071, 13);
		((System.Windows.Forms.Control)(object)this.comSuplier).Name = "comSuplier";
		((System.Windows.Forms.Control)(object)this.comSuplier).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.comSuplier).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.comSuplier).Leave += new System.EventHandler(comSuplier_Leave);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Red;
		this.label22.Location = new System.Drawing.Point(108, 49);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(12, 15);
		this.label22.TabIndex = 11;
		this.label22.Text = "*";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Red;
		this.label25.Location = new System.Drawing.Point(1044, 19);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(12, 15);
		this.label25.TabIndex = 7;
		this.label25.Text = "*";
		this.txtInvoiceAmount.BackColor = System.Drawing.Color.White;
		this.txtInvoiceAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceAmount.Location = new System.Drawing.Point(442, 129);
		this.txtInvoiceAmount.Name = "txtInvoiceAmount";
		this.txtInvoiceAmount.ReadOnly = true;
		this.txtInvoiceAmount.Size = new System.Drawing.Size(189, 23);
		this.txtInvoiceAmount.TabIndex = 15;
		this.txtInvoiceAmount.TabStop = false;
		this.txtInvoiceAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(947, 19);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(85, 15);
		this.label4.TabIndex = 6;
		this.label4.Text = "Supplier Name";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(101, 188);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(12, 15);
		this.label49.TabIndex = 127;
		this.label49.Text = "*";
		this.CmbChargesType.AllowNull = (DefaultableBoolean)1;
		this.CmbChargesType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChargesType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChargesType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChargesType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Location = new System.Drawing.Point(12, 208);
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Name = "CmbChargesType";
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Size = new System.Drawing.Size(119, 26);
		((System.Windows.Forms.Control)(object)this.CmbChargesType).TabIndex = 17;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(11, 188);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(77, 15);
		this.label48.TabIndex = 126;
		this.label48.Text = "Charges Type";
		this.button1.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.button1.Location = new System.Drawing.Point(331, 210);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(27, 23);
		this.button1.TabIndex = 19;
		this.button1.TabStop = false;
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(366, 188);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(52, 15);
		this.label47.TabIndex = 123;
		this.label47.Text = "Remarks";
		this.txtRemarksDetail.BackColor = System.Drawing.Color.White;
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(360, 210);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(468, 23);
		this.txtRemarksDetail.TabIndex = 20;
		this.txtRemarks.Location = new System.Drawing.Point(129, 159);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(810, 20);
		this.txtRemarks.TabIndex = 16;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(10, 162);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(52, 15);
		this.label46.TabIndex = 121;
		this.label46.Text = "Remarks";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(10, 106);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(58, 15);
		this.label45.TabIndex = 120;
		this.label45.Text = "FC  Local ";
		this.cmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Location = new System.Drawing.Point(750, 13);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Name = "cmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Leave += new System.EventHandler(cmbPaymentTerm_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(1425, 66);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(12, 15);
		this.label42.TabIndex = 115;
		this.label42.Text = "*";
		this.label42.Visible = false;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Segoe UI", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1175, 210);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(55, 22);
		this.btnCancelUpdateDetial.TabIndex = 25;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(1416, 21);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(12, 15);
		this.label41.TabIndex = 8;
		this.label41.Text = "*";
		this.label41.Visible = false;
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(1115, 210);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(56, 22);
		this.btnUpdateDetail.TabIndex = 24;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.cmbranches.AllowNull = (DefaultableBoolean)1;
		this.cmbranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.cmbranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbranches).Location = new System.Drawing.Point(1374, 45);
		((System.Windows.Forms.Control)(object)this.cmbranches).Name = "cmbranches";
		((System.Windows.Forms.Control)(object)this.cmbranches).Size = new System.Drawing.Size(89, 22);
		((System.Windows.Forms.Control)(object)this.cmbranches).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbranches).Visible = false;
		this.txtdate.CustomFormat = "dd-MMM-yyyy";
		this.txtdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdate.Location = new System.Drawing.Point(442, 15);
		this.txtdate.Name = "txtdate";
		this.txtdate.Size = new System.Drawing.Size(189, 23);
		this.txtdate.TabIndex = 1;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Red;
		this.label31.Location = new System.Drawing.Point(425, 106);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(12, 15);
		this.label31.TabIndex = 13;
		this.label31.Text = "*";
		this.cmbproject.AllowNull = (DefaultableBoolean)1;
		this.cmbproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance78;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance84).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbproject).Location = new System.Drawing.Point(1374, 90);
		((System.Windows.Forms.Control)(object)this.cmbproject).Name = "cmbproject";
		((System.Windows.Forms.Control)(object)this.cmbproject).Size = new System.Drawing.Size(85, 22);
		((System.Windows.Forms.Control)(object)this.cmbproject).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbproject).Visible = false;
		this.btnAddinGrid.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAddinGrid.Location = new System.Drawing.Point(1085, 210);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(27, 23);
		this.btnAddinGrid.TabIndex = 23;
		this.btnAddinGrid.UseVisualStyleBackColor = true;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(1371, 70);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(48, 17);
		this.label40.TabIndex = 114;
		this.label40.Text = "Project";
		this.label40.Visible = false;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(733, 49);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(12, 15);
		this.label30.TabIndex = 6;
		this.label30.Text = "*";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(1371, 20);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(47, 17);
		this.label39.TabIndex = 113;
		this.label39.Text = "Branch";
		this.label39.Visible = false;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(733, 19);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(12, 15);
		this.label29.TabIndex = 10;
		this.label29.Text = "*";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(733, 106);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(12, 15);
		this.label28.TabIndex = 14;
		this.label28.Text = "*";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(1052, 189);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(12, 15);
		this.label36.TabIndex = 37;
		this.label36.Text = "*";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(429, 49);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(12, 15);
		this.label26.TabIndex = 5;
		this.label26.Text = "*";
		this.txtpkramount.BackColor = System.Drawing.Color.White;
		this.txtpkramount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpkramount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpkramount.Location = new System.Drawing.Point(962, 210);
		this.txtpkramount.Name = "txtpkramount";
		this.txtpkramount.Size = new System.Drawing.Size(116, 23);
		this.txtpkramount.TabIndex = 22;
		this.txtpkramount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtpkramount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtpkramount_KeyPress);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(966, 189);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(82, 15);
		this.label37.TabIndex = 36;
		this.label37.Text = "Local Amount";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(733, 78);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(12, 15);
		this.label24.TabIndex = 10;
		this.label24.Text = "*";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Red;
		this.label23.Location = new System.Drawing.Point(108, 77);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(13, 16);
		this.label23.TabIndex = 8;
		this.label23.Text = "*";
		this.txtfcamount.BackColor = System.Drawing.Color.White;
		this.txtfcamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfcamount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfcamount.Location = new System.Drawing.Point(831, 210);
		this.txtfcamount.Name = "txtfcamount";
		this.txtfcamount.Size = new System.Drawing.Size(128, 23);
		this.txtfcamount.TabIndex = 21;
		this.txtfcamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfcamount.TextChanged += new System.EventHandler(txtfcamount_TextChanged);
		this.txtfcamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfcamount_KeyPress);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.Red;
		this.label21.Location = new System.Drawing.Point(425, 19);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(12, 15);
		this.label21.TabIndex = 4;
		this.label21.Text = "*";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(425, 78);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(12, 15);
		this.label20.TabIndex = 9;
		this.label20.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(1056, 71);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(12, 15);
		this.label19.TabIndex = 11;
		this.label19.Text = "*";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(827, 189);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(68, 15);
		this.label33.TabIndex = 30;
		this.label33.Text = "FC Amount";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(1026, 49);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(12, 15);
		this.label6.TabIndex = 7;
		this.label6.Text = "*";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Red;
		this.label5.Location = new System.Drawing.Point(108, 19);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(12, 15);
		this.label5.TabIndex = 0;
		this.label5.Text = "*";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(135, 188);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(77, 15);
		this.label35.TabIndex = 33;
		this.label35.Text = "Account Title";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(328, 78);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(84, 15);
		this.label18.TabIndex = 36;
		this.label18.Text = "Exchange Rate";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(224, 188);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(12, 15);
		this.label34.TabIndex = 34;
		this.label34.Text = "*";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(894, 190);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(12, 15);
		this.label32.TabIndex = 31;
		this.label32.Text = "*";
		this.txtexchangeRate.BackColor = System.Drawing.Color.White;
		this.txtexchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtexchangeRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtexchangeRate.Location = new System.Drawing.Point(442, 74);
		this.txtexchangeRate.Name = "txtexchangeRate";
		this.txtexchangeRate.Size = new System.Drawing.Size(191, 23);
		this.txtexchangeRate.TabIndex = 9;
		this.txtexchangeRate.Text = "0";
		this.txtexchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtexchangeRate.TextChanged += new System.EventHandler(txtexchangeRate_TextChanged);
		this.txtexchangeRate.Leave += new System.EventHandler(txtexchangeRate_Leave);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(638, 78);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(82, 15);
		this.label17.TabIndex = 39;
		this.label17.Text = "Local Amount";
		this.txtNetAmount.BackColor = System.Drawing.Color.White;
		this.txtNetAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetAmount.Location = new System.Drawing.Point(750, 74);
		this.txtNetAmount.Name = "txtNetAmount";
		this.txtNetAmount.ReadOnly = true;
		this.txtNetAmount.Size = new System.Drawing.Size(189, 23);
		this.txtNetAmount.TabIndex = 10;
		this.txtNetAmount.TabStop = false;
		this.txtNetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(10, 78);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(75, 15);
		this.label14.TabIndex = 27;
		this.label14.Text = "FCY Amount";
		this.txtFCGrosamount.BackColor = System.Drawing.Color.White;
		this.txtFCGrosamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFCGrosamount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFCGrosamount.Location = new System.Drawing.Point(129, 74);
		this.txtFCGrosamount.Name = "txtFCGrosamount";
		this.txtFCGrosamount.Size = new System.Drawing.Size(191, 23);
		this.txtFCGrosamount.TabIndex = 8;
		this.txtFCGrosamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFCGrosamount.TextChanged += new System.EventHandler(txtFCGrosamount_TextChanged);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(328, 106);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(97, 15);
		this.label13.TabIndex = 33;
		this.label13.Text = "Net Amount FCY";
		this.txtNetAmountFC.BackColor = System.Drawing.Color.White;
		this.txtNetAmountFC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetAmountFC.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetAmountFC.Location = new System.Drawing.Point(442, 102);
		this.txtNetAmountFC.Name = "txtNetAmountFC";
		this.txtNetAmountFC.ReadOnly = true;
		this.txtNetAmountFC.Size = new System.Drawing.Size(189, 23);
		this.txtNetAmountFC.TabIndex = 13;
		this.txtNetAmountFC.TabStop = false;
		this.txtNetAmountFC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(638, 106);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(95, 15);
		this.label12.TabIndex = 42;
		this.label12.Text = "Bank Cr Amount";
		this.txtbankDrAmount.BackColor = System.Drawing.Color.White;
		this.txtbankDrAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbankDrAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbankDrAmount.Location = new System.Drawing.Point(750, 102);
		this.txtbankDrAmount.Name = "txtbankDrAmount";
		this.txtbankDrAmount.ReadOnly = true;
		this.txtbankDrAmount.Size = new System.Drawing.Size(189, 23);
		this.txtbankDrAmount.TabIndex = 17;
		this.txtbankDrAmount.TabStop = false;
		this.txtbankDrAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(638, 49);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 21;
		this.label11.Text = "Bank FBP # ";
		this.txtbankfbpNo.BackColor = System.Drawing.Color.White;
		this.txtbankfbpNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbankfbpNo.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbankfbpNo.Location = new System.Drawing.Point(750, 45);
		this.txtbankfbpNo.Name = "txtbankfbpNo";
		this.txtbankfbpNo.Size = new System.Drawing.Size(189, 23);
		this.txtbankfbpNo.TabIndex = 5;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(947, 49);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(56, 15);
		this.label10.TabIndex = 24;
		this.label10.Text = "Fcy Code";
		this.cmbcurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbcurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance113).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Location = new System.Drawing.Point(1071, 43);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Name = "cmbcurrency";
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).TabIndex = 6;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(328, 49);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(104, 15);
		this.label9.TabIndex = 18;
		this.label9.Text = "Bank Account (Cr)";
		this.cmbbankAcc.AllowNull = (DefaultableBoolean)1;
		this.cmbbankAcc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbankAcc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbankAcc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbankAcc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbbankAcc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbankAcc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).Location = new System.Drawing.Point(442, 43);
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).Name = "cmbbankAcc";
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).TabIndex = 4;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(947, 78);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(111, 15);
		this.label7.TabIndex = 12;
		this.label7.Text = "Import Contract No";
		this.cmbLCOrder.AllowNull = (DefaultableBoolean)1;
		this.cmbLCOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLCOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLCOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLCOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.cmbLCOrder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLCOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Location = new System.Drawing.Point(1071, 72);
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Name = "cmbLCOrder";
		this.cmbLCOrder.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).TabIndex = 7;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(638, 19);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(83, 15);
		this.label3.TabIndex = 9;
		this.label3.Text = "Payment Term";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(328, 19);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(77, 15);
		this.label2.TabIndex = 3;
		this.label2.Text = "Voucher Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(10, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(72, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Voucher No.";
		this.txtdocNumber.BackColor = System.Drawing.Color.White;
		this.txtdocNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocNumber.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocNumber.Location = new System.Drawing.Point(129, 15);
		this.txtdocNumber.Name = "txtdocNumber";
		this.txtdocNumber.ReadOnly = true;
		this.txtdocNumber.Size = new System.Drawing.Size(191, 23);
		this.txtdocNumber.TabIndex = 0;
		this.txtbankcharges.BackColor = System.Drawing.Color.White;
		this.txtbankcharges.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbankcharges.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbankcharges.Location = new System.Drawing.Point(541, 344);
		this.txtbankcharges.Name = "txtbankcharges";
		this.txtbankcharges.Size = new System.Drawing.Size(189, 23);
		this.txtbankcharges.TabIndex = 12;
		this.txtbankcharges.Text = "0";
		this.txtbankcharges.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtbankcharges.Visible = false;
		this.txtbankcharges.TextChanged += new System.EventHandler(txtbankcharges_TextChanged);
		this.txtbankcharges.Leave += new System.EventHandler(txtbankcharges_Leave);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(424, 348);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(103, 15);
		this.label16.TabIndex = 30;
		this.label16.Text = "FCY Bank Charges";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Red;
		this.label27.Location = new System.Drawing.Point(524, 348);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(12, 15);
		this.label27.TabIndex = 13;
		this.label27.Text = "*";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.ForeColor = System.Drawing.Color.Red;
		this.label44.Location = new System.Drawing.Point(208, 348);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(12, 15);
		this.label44.TabIndex = 118;
		this.label44.Text = "*";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(107, 348);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(91, 15);
		this.label43.TabIndex = 117;
		this.label43.Text = "FCY Charges Ac";
		this.CmbFcyBankChargesTypeAc.AllowNull = (DefaultableBoolean)1;
		this.CmbFcyBankChargesTypeAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFcyBankChargesTypeAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFcyBankChargesTypeAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance97).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFcyBankChargesTypeAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFcyBankChargesTypeAc).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFcyBankChargesTypeAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFcyBankChargesTypeAc).Location = new System.Drawing.Point(228, 342);
		((System.Windows.Forms.Control)(object)this.CmbFcyBankChargesTypeAc).Name = "CmbFcyBankChargesTypeAc";
		((System.Windows.Forms.Control)(object)this.CmbFcyBankChargesTypeAc).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbFcyBankChargesTypeAc).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbFcyBankChargesTypeAc).Visible = false;
		this.txtFcyBankChargesRsAmount.BackColor = System.Drawing.Color.White;
		this.txtFcyBankChargesRsAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyBankChargesRsAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyBankChargesRsAmount.Location = new System.Drawing.Point(795, 344);
		this.txtFcyBankChargesRsAmount.Name = "txtFcyBankChargesRsAmount";
		this.txtFcyBankChargesRsAmount.ReadOnly = true;
		this.txtFcyBankChargesRsAmount.Size = new System.Drawing.Size(191, 23);
		this.txtFcyBankChargesRsAmount.TabIndex = 12;
		this.txtFcyBankChargesRsAmount.TabStop = false;
		this.txtFcyBankChargesRsAmount.Text = "0";
		this.txtFcyBankChargesRsAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyBankChargesRsAmount.Visible = false;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 28);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1270, 40);
		this.panel6.TabIndex = 0;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(16, 8);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(144, 24);
		this.label15.TabIndex = 0;
		this.label15.Text = "FCY PAYMENT";
		this.panel7.Controls.Add(this.btnAttachments);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1270, 28);
		this.panel7.TabIndex = 1;
		this.btnAttachments.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btnAttachments.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnnew, this.btnsave, this.btnUpdate, this.btnPrint, this.btnAttachment, this.btnRefresh, this.btnSlip });
		this.btnAttachments.Location = new System.Drawing.Point(0, 0);
		this.btnAttachments.Name = "btnAttachments";
		this.btnAttachments.Size = new System.Drawing.Size(1270, 27);
		this.btnAttachments.TabIndex = 0;
		this.btnAttachments.Text = "toolStrip1";
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
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = (System.Drawing.Image)resources.GetObject("btnPrint.Image");
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "&Voucher102";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(99, 24);
		this.btnAttachment.Text = "Attachments";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(79, 24);
		this.btnSlip.Text = "516-Print";
		this.btnSlip.Visible = false;
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1276, 642);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.ctrlGrdBar1);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1270, 636);
		this.panel8.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1222, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 21);
		this.ctrlGrdBar1.TabIndex = 45;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 67);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1270, 569);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 44;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel9.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel9.Controls.Add(this.label38);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 27);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1270, 40);
		this.panel9.TabIndex = 2;
		this.label38.AutoSize = true;
		this.label38.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label38.Location = new System.Drawing.Point(16, 8);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(145, 24);
		this.label38.TabIndex = 0;
		this.label38.Text = "History FC-Bank";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnLoadAll, this.toolStripSeparator2 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1270, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1284, 668);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "AcfrmFcyPaymentVoucher";
		this.Text = "Fcy Payment";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(Acfrmfcbankreceipt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Acfrmfcbankreceipt_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFCBank).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGainLossAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesAccountType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comSuplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbankAcc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLCOrder).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyBankChargesTypeAc).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.btnAttachments.ResumeLayout(false);
		this.btnAttachments.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
