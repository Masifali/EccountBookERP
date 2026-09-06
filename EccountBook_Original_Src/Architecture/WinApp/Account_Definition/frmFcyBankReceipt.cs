using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Export;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class frmFcyBankReceipt : Form
{
	private int Id;

	private double PreviousBalance;

	private int VoucherHeadId;

	private int BrancheId;

	private int ProjectId;

	private int updateDetailIndex;

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtLCOrder = new DataTable();

	private DataTable table = new DataTable();

	private DataTable dtPaymentTermsDetail = new DataTable();

	private DataTable dtaccounts = new DataTable();

	private Attachment AT = new Attachment();

	private LoadExportInvoice LoadExportInvoice;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

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

	private Label label26;

	private Label label25;

	private Label label24;

	private Label label23;

	private Label label22;

	private Label label20;

	private Label label19;

	private Label label6;

	private Label label5;

	private Label label18;

	private Label label4;

	private TextBox txtexchangeRate;

	private Label label17;

	private TextBox txtNetAmount;

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

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnUpdate;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStrip toolStrip2;

	internal UltraCombo cmbranches;

	private Label label39;

	internal UltraCombo cmbproject;

	private Label label40;

	private Label label42;

	private Label label41;

	internal UltraCombo cmbPaymentTerm;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnRefresh;

	private Label label45;

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

	internal UltraCombo cmbaccounttype;

	private Label label56;

	private Label label57;

	private GroupBox groupBox2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton BtnLoadInvoice;

	private GroupBox groupBox4;

	private GroupBox groupBox3;

	private TextBox txtfcyAmountThirdParty;

	private Label label27;

	private TextBox txtHcyExchRthirdParty;

	private Label label21;

	private TextBox txtFcyExchRthirdParty;

	private Label label2;

	private Label label16;

	internal UltraCombo cmbFcycodeThirdParty;

	private CheckBox chkpreview;

	private Label label44;

	private TextBox txtChargePercent;

	internal UltraCombo cmbReferenceNo;

	private Label label43;

	private Panel panel4;

	private Panel panel5;

	private Label label68;

	private TextBox txtFBC;

	internal UltraCombo CmbFBCDebitAccount;

	private Label label69;

	internal UltraCombo CmbConsignee;

	private Label lblConsignee;

	private TextBox txtFINo;

	private Label labFiNo;

	internal GridEX DataGridHistory;

	private Panel panel9;

	private Label label70;

	private Panel panel15;

	private GroupBox groupBox6;

	private Button btnShowHistory;

	private Label label67;

	private Label label73;

	private Label label89;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label38;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private CtrlGrdBar ctrlGrdBar2;

	private TextBox txtReceiverFcyAmount;

	private Label label72;

	private Label label71;

	private TextBox txtReceiverExchangeRate;

	internal GridEX grdPaymentTerms;

	private Panel panel10;

	private Label label60;

	internal UltraCombo CmbReceiverAcHistory;

	private Label label59;

	internal UltraCombo CmbInvoiceNoHistory;

	private Label label58;

	internal UltraCombo CmbPaymentTermHistory;

	public frmFcyBankReceipt(UserAccount user)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InitializeEssentialComponents()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1700, 750);
			ConfigureRights();
			ConfigureControls();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ConfigureRights()
	{
		try
		{
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			cmbcurrency.ReadOnly = true;
			txtdate.Focus();
			chkpreview.Checked = true;
			lblConsignee.Visible = false;
			((Control)(object)CmbConsignee).Visible = false;
			labFiNo.Visible = false;
			txtFINo.Visible = false;
			txtRemarks.Width = 555;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ConfigureControls()
	{
		try
		{
			table.Columns.Add("TypeId");
			table.Columns.Add("Type");
			table.Columns.Add("ChargesTypeId");
			table.Columns.Add("ChargesType");
			table.Columns.Add("Prcnt", typeof(double));
			table.Columns.Add("Remarks");
			table.Columns.Add("FC-Amount", typeof(double));
			table.Columns.Add("Rs-Amount", typeof(double));
			dtPaymentTermsDetail.Columns.Add("InvoicePaymentDetailId", typeof(int));
			dtPaymentTermsDetail.Columns.Add("PaymentTermId", typeof(int));
			dtPaymentTermsDetail.Columns.Add("PaymentTerm", typeof(string));
			dtPaymentTermsDetail.Columns.Add("Amount", typeof(double));
			dtPaymentTermsDetail.Columns.Add("Received", typeof(double));
			dtPaymentTermsDetail.Columns.Add("Balance", typeof(double));
			dtPaymentTermsDetail.Columns.Add("ThisReceipt", typeof(double));
			grdPaymentTerms.DataSource = dtPaymentTermsDetail;
			grdPaymentTerms.RetrieveStructure();
			gridPaymentTermsSettings();
			((Control)(object)grdPaymentTerms).Visible = false;
			FromDateHistory.Value = DateTime.Now.AddDays(-7.0);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private async Task LoadNonEssentialDataAsync()
	{
		try
		{
			await Task.Run(delegate
			{
				BindData();
			});
			Invoke((Action)delegate
			{
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void RunOnUiThread(Action action)
	{
		if (base.InvokeRequired)
		{
			Invoke(action);
		}
		else
		{
			action();
		}
	}

	private void BindData()
	{
		try
		{
			CombBranchFill();
			CombProjectFill();
			GenerateDocumentNo();
			PaymentTermFill();
			CurrencyCOde();
			ReferenceNoFill();
			GetAllAccounts();
			Accounttype();
			GainLossAccountFill();
			ChargerType();
			ChargesTypeFill();
			FBCDebitAccountBind();
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Acfrmfcbankreceipt_Load(object sender, EventArgs e)
	{
		try
		{
			InitializeEssentialComponents();
			Task.Run(() => LoadNonEssentialDataAsync());
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
			RunOnUiThread(delegate
			{
				if (BranchList.Rows.Count > 0)
				{
					BrancheId = Conversion.ToInt(BranchList.Rows[0]["Id"]);
				}
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CombProjectFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				DataTable dataTable = CommonServices.ProjectServiceBind();
				if (dataTable.Rows.Count > 0 && dataTable != null)
				{
					ProjectId = Conversion.ToInt(dataTable.Rows[0]["Id"]);
				}
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GenerateDocumentNo()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Expected O, but got Unknown
				int num = ExImFCBankReceipts.GenerateCode(new ExImFCBankReceipts
				{
					DocumentTypeId = 203,
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				});
				if (num > 0)
				{
					txtdocNumber.Text = num.ToString();
				}
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PaymentTermFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				DataTable dt = new DataTable
				{
					Columns = { "Id", "PaymentTerm" },
					Rows = 
					{
						new object[2] { 1, "Invoice Payment" },
						new object[2] { 2, "Advanced Payment" }
					}
				};
				DDL.BindDDLNew(dt, cmbPaymentTerm, "Id", "PaymentTerm", "PaymentTerms", false);
				((UltraGridBase)cmbPaymentTerm).Rows[0].Activate();
			});
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
			((Control)(object)cmbInvoiceNo).TabStop = true;
			lblConsignee.Visible = false;
			((Control)(object)CmbConsignee).Visible = false;
			labFiNo.Visible = false;
			txtFINo.Visible = false;
			((Control)(object)grdPaymentTerms).Visible = false;
			txtRemarks.Width = 555;
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
				lblConsignee.Visible = true;
				((Control)(object)CmbConsignee).Visible = true;
				labFiNo.Visible = true;
				txtFINo.Visible = true;
				txtRemarks.Width = 234;
				if (cmbInvoiceNo.ReadOnly)
				{
					((Control)(object)cmbInvoiceNo).TabStop = false;
				}
				GetExportCustomerByAdvancePayment();
				LcOrder();
			}
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				cmbInvoiceNo.ReadOnly = false;
				cmbLCOrder.ReadOnly = true;
				((Control)(object)cmbLCOrder).Text = "";
				cmbcurrency.ReadOnly = true;
				((Control)(object)grdPaymentTerms).Visible = true;
				if (cmbLCOrder.ReadOnly)
				{
					((Control)(object)cmbLCOrder).TabStop = false;
				}
				CustomersFillByInvoicePayment();
				InvoiceNo();
			}
			if (((Control)(object)cmbPaymentTerm).Text == "Other Payment")
			{
				cmbLCOrder.ReadOnly = true;
				((Control)(object)cmbLCOrder).Text = "";
				cmbInvoiceNo.ReadOnly = true;
				cmbcurrency.ReadOnly = false;
				((Control)(object)cmbInvoiceNo).Text = "";
				txtInvoiceAmount.Text = "0";
				txtBalanceAmount.Text = "0";
				txtRecAmount.Text = "0";
				txtExportvoucherRate.Text = "0";
				((Control)(object)cmbInvoiceNo).TabStop = false;
				((Control)(object)cmbLCOrder).TabStop = false;
				((UltraGridBase)cmbLCOrder).DataSource = null;
				((UltraGridBase)cmbInvoiceNo).DataSource = null;
				GetAllExportCustomer();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAllExportCustomer()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SupplierCustomer.ReadByOrganizationCompanyIdForExport(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, comSuplier, "Id", "CompanyName", "Customer Name", false);
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
			DataTable dtLcOrder = ExImLcOrder.GetExportCustomerByAdvancePayment(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtLcOrder.Rows.Count > 0)
			{
				DDL.BindDDL(dtLcOrder, comSuplier, "SupCustId", "CompanyName", "Customer Name", ZeroIndex: false);
				DDL.BindDDL(dtLcOrder, CmbConsignee, "SupCustId", "CompanyName", "Consignee", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyCOde()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Expected O, but got Unknown
				DataTable all = MultiCurrency.GetAll(new MultiCurrency
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (all.Rows.Count > 0)
				{
					DDL.BindDDLNew(all, cmbcurrency, "Id", "CurrencyCode", "Fcy Code", false);
					DDL.BindDDLNew(all, cmbFcycodeThirdParty, "Id", "CurrencyCode", "Fcy Code", false);
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReferenceNoFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Expected O, but got Unknown
				DataTable referenceNo = ExImFCBankReceipts.GetReferenceNo(new ExImFCBankReceipts
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (referenceNo.Rows.Count > 0)
				{
					DataTable dataTable = new DataTable
					{
						Columns = { "Id", "ReferenceNo" }
					};
					for (int i = 0; i < referenceNo.Rows.Count; i++)
					{
						dataTable.Rows.Add(referenceNo.Rows[i]["ReferenceNo"], referenceNo.Rows[i]["ReferenceNo"]);
					}
					DDL.BindDDL(dataTable, cmbReferenceNo, "Id", "ReferenceNo", "Reference No", ZeroIndex: false);
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ChargesTypeFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				DataTable dt = new DataTable
				{
					Columns = { "Id", "ChargesType" },
					Rows = 
					{
						new object[2] { 1, "Local" },
						new object[2] { 2, "Foreign" }
					}
				};
				DDL.BindDDLNew(dt, CmbChargesType, "Id", "ChargesType", "ChargesType", false);
				((UltraGridBase)CmbChargesType).Rows[0].Activate();
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomersFillByInvoicePayment()
	{
		try
		{
			DataTable dtInvoice = CommonServices.GetInvoiceNoandPartiesForFcBankReceipts();
			if (dtInvoice.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("CompanyName");
				for (int i = 0; i < dtInvoice.Rows.Count; i++)
				{
					if (Conversion.ToString(dtInvoice.Rows[i]["FilterType"]) == "PartyName")
					{
						dt.Rows.Add(dtInvoice.Rows[i]["Id"], dtInvoice.Rows[i]["RefName"]);
					}
				}
				DDL.BindDDL(dt, comSuplier, "Id", "CompanyName", "Customer Name", ZeroIndex: false);
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
		try
		{
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				DataTable dtInvoice = CommonServices.GetInvoiceNoandPartiesForFcBankReceipts(Conversion.ToInt(comSuplier.Value));
				if (dtInvoice.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("InvoiceNo");
					for (int i = 0; i < dtInvoice.Rows.Count; i++)
					{
						if (Conversion.ToString(dtInvoice.Rows[i]["FilterType"]) == "InvoiceNo")
						{
							dt.Rows.Add(dtInvoice.Rows[i]["Id"], dtInvoice.Rows[i]["RefName"]);
						}
					}
					if (dt.Rows.Count > 0)
					{
						DDL.BindDDL(dt, cmbInvoiceNo, "Id", "InvoiceNo", "InvoiceNo", ZeroIndex: false);
						return;
					}
					((Control)(object)cmbInvoiceNo).Text = string.Empty;
					((UltraGridBase)cmbInvoiceNo).DataSource = null;
				}
				else
				{
					((Control)(object)cmbInvoiceNo).Text = string.Empty;
					((UltraGridBase)cmbInvoiceNo).DataSource = null;
				}
			}
			else
			{
				((Control)(object)cmbInvoiceNo).Text = string.Empty;
				((UltraGridBase)cmbInvoiceNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Accounttype()
	{
		try
		{
			RunOnUiThread(delegate
			{
				DataTable dt = new DataTable
				{
					Columns = { "Id", "AccountType" },
					Rows = 
					{
						new object[2] { 1, "Bank" },
						new object[2] { 2, "Parties" },
						new object[2] { 3, "Cash" },
						new object[2] { 4, "Expense" }
					}
				};
				DDL.BindDDL(dt, cmbaccounttype, "Id", "AccountType", "Account Type", ZeroIndex: false);
				((UltraGridBase)cmbaccounttype).Rows[0].Activate();
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbaccounttype_ValueChanged(object sender, EventArgs e)
	{
	}

	private void BankAccountDr(int accounttypeId)
	{
		try
		{
			if (dtaccounts.Rows.Count > 0)
			{
				DataTable dtdrac = new DataTable();
				dtdrac.Columns.Add("Id");
				dtdrac.Columns.Add("AccountTitle");
				for (int i = 0; i < dtaccounts.Rows.Count; i++)
				{
					if (Conversion.ToInt((object)accounttypeId) == 22)
					{
						if (Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 3 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 6 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 8 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 22)
						{
							dtdrac.Rows.Add(dtaccounts.Rows[i]["Id"], dtaccounts.Rows[i]["AccountTitle"]);
						}
					}
					else if (Conversion.ToInt((object)accounttypeId) == 11)
					{
						if (Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 11 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 13 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 14 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 20 || Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == 21)
						{
							dtdrac.Rows.Add(dtaccounts.Rows[i]["Id"], dtaccounts.Rows[i]["AccountTitle"]);
						}
					}
					else if (Conversion.ToInt(dtaccounts.Rows[i]["AccounttypeId"]) == Conversion.ToInt((object)accounttypeId))
					{
						dtdrac.Rows.Add(dtaccounts.Rows[i]["Id"], dtaccounts.Rows[i]["AccountTitle"]);
					}
				}
				if (dtdrac.Rows.Count > 0)
				{
					DDL.BindDDL(dtdrac, cmbbankAcc, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
					return;
				}
				((Control)(object)cmbbankAcc).Text = string.Empty;
				((UltraGridBase)cmbbankAcc).DataSource = null;
			}
			else
			{
				((Control)(object)cmbbankAcc).Text = string.Empty;
				((UltraGridBase)cmbbankAcc).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAllAccounts()
	{
		try
		{
			RunOnUiThread(delegate
			{
				dtaccounts = CommonServices.CoaAllocationGetAllServiceBind();
			});
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
			RunOnUiThread(delegate
			{
				DataTable dataTable = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,4,11,12,15");
				if (dataTable.Rows.Count > 0)
				{
					DDL.BindDDL(dataTable, CmbGainLossAc, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FBCDebitAccountBind()
	{
		try
		{
			RunOnUiThread(delegate
			{
				DataTable dataTable = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("11,13,14,20,21");
				if (dataTable.Rows.Count > 0)
				{
					DDL.BindDDL(dataTable, CmbFBCDebitAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChargerType()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Expected O, but got Unknown
				DataTable dataTable = ExImProceedsChargesType.Getall(new ExImProceedsChargesType
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (dataTable.Rows.Count > 0)
				{
					DDL.BindDDLNew(dataTable, CmbChargesAccountType, "Id", "ProceedsChargesTypeCode", "Charges Type", false);
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetChargesTypeByCustomerId(int ID)
	{
	}

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(Id, 203);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbPaymentTerm_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbPaymentTerm).Text != "Other Payment")
			{
				((Control)(object)comSuplier).Text = string.Empty;
				((UltraGridBase)comSuplier).DataSource = null;
			}
			PaymentTerm();
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
			if (((Control)(object)CmbChargesAccountType).Text.Trim() == string.Empty || Conversion.ToInt(CmbChargesAccountType.Value) == 0)
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
			if (((Control)(object)CmbChargesType).Text.Trim() == "" || Conversion.ToInt(CmbChargesType.Value) == 0)
			{
				MessageBox.Show("Please select Charges Type");
				CmbChargesType.Focus();
				return;
			}
			if (((Control)(object)CmbChargesType).Text == "Foreign" && (txtfcamount.Text.Trim() == string.Empty || txtfcamount.Text.Trim() == "0"))
			{
				throw new Exception("FcAmount field required");
			}
			table.Rows.Add(CmbChargesType.Value, ((Control)(object)CmbChargesType).Text.Trim(), CmbChargesAccountType.Value, ((Control)(object)CmbChargesAccountType).Text, Conversion.ToDouble((object)txtChargePercent.Text), txtRemarksDetail.Text.Trim(), Conversion.ToDouble((object)txtfcamount.Text.Trim()), Conversion.ToDouble((object)txtpkramount.Text.Trim()));
			grdFCBank.DataSource = table;
			grdFCBank.RetrieveStructure();
			gridsettings();
			GridEXRow[] rows = grdFCBank.GetRows();
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
			txtForeinchargesPkr.Text = ForeignAmount.ToString("#,##0.####");
			txtLocalAmount.Text = LocalAmount.ToString("#,##0.####");
			CmbChargesAccountType.Value = "";
			txtChargePercent.Text = "";
			txtfcamount.Text = "";
			txtpkramount.Text = "";
			txtRemarksDetail.Text = "";
			BankDebitAmountCalculate();
			NetAmountFC();
			CmbChargesType.Focus();
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
			if (((Control)(object)CmbChargesAccountType).Text.Trim() == "")
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
			if (((Control)(object)CmbChargesType).Text.Trim() == "" || Conversion.ToInt(CmbChargesType.Value) == 0)
			{
				MessageBox.Show("Please select Charges Type");
				CmbChargesType.Focus();
				return;
			}
			if (((Control)(object)CmbChargesType).Text == "Foreign" && (txtfcamount.Text.Trim() == string.Empty || txtfcamount.Text.Trim() == "0"))
			{
				throw new Exception("FcAmount field required");
			}
			table.Rows[updateDetailIndex]["TypeId"] = CmbChargesType.Value;
			table.Rows[updateDetailIndex]["Type"] = ((Control)(object)CmbChargesType).Text;
			table.Rows[updateDetailIndex]["ChargesTypeId"] = CmbChargesAccountType.Value;
			table.Rows[updateDetailIndex]["ChargesType"] = ((Control)(object)CmbChargesAccountType).Text;
			table.Rows[updateDetailIndex]["Prcnt"] = Conversion.ToDouble((object)txtChargePercent.Text);
			table.Rows[updateDetailIndex]["Remarks"] = txtRemarksDetail.Text;
			table.Rows[updateDetailIndex]["FC-Amount"] = Conversion.ToDouble((object)txtfcamount.Text);
			table.Rows[updateDetailIndex]["Rs-Amount"] = Conversion.ToDouble((object)txtpkramount.Text);
			grdFCBank.DataSource = table;
			grdFCBank.RetrieveStructure();
			gridsettings();
			resetdetail();
			BankDebitAmountCalculate();
			btnAddinGrid.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			GridEXRow[] rows = grdFCBank.GetRows();
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
			CmbChargesType.Focus();
			NetAmountFC();
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
				txtChargePercent.Text = Conversion.ToDouble((object)item.Cells["Prcnt"].Text).ToString("#,##0.####");
				txtRemarksDetail.Text = item.Cells["Remarks"].Text.ToString();
				txtfcamount.Text = Conversion.ToDouble(item.Cells["FC-Amount"].Value).ToString("#,##0.####");
				txtpkramount.Text = Conversion.ToDouble(item.Cells["Rs-Amount"].Value).ToString("#,##0.####");
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

	public void gridsettings()
	{
		try
		{
			grdFCBank.RootTable.Columns["TypeId"].Visible = false;
			grdFCBank.RootTable.Columns["ChargesTypeId"].Visible = false;
			grdFCBank.RootTable.GroupTotals = (GroupTotals)2;
			grdFCBank.RootTable.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdFCBank.RootTable.Columns["ChargesType"].FilterEditType = (FilterEditType)1;
			grdFCBank.RootTable.Columns["FC-Amount"].FilterEditType = (FilterEditType)1;
			grdFCBank.RootTable.Columns["Rs-Amount"].FilterEditType = (FilterEditType)1;
			grdFCBank.RootTable.Columns["ChargesType"].EditType = (EditType)0;
			grdFCBank.RootTable.Columns["FC-Amount"].EditType = (EditType)0;
			grdFCBank.RootTable.Columns["Rs-Amount"].EditType = (EditType)0;
			grdFCBank.RootTable.Columns["Rs-Amount"].AggregateFunction = (AggregateFunction)2;
			grdFCBank.RootTable.Columns["Rs-Amount"].TextAlignment = (TextAlignment)3;
			grdFCBank.RootTable.Columns["Rs-Amount"].FormatString = "#,##0.##";
			grdFCBank.RootTable.Columns["Rs-Amount"].TotalFormatString = "#,##0.##";
			grdFCBank.RootTable.Columns["FC-Amount"].AggregateFunction = (AggregateFunction)2;
			grdFCBank.RootTable.Columns["FC-Amount"].TextAlignment = (TextAlignment)3;
			grdFCBank.RootTable.Columns["FC-Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFCBank.RootTable.Columns["FC-Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFCBank.RootTable.Columns.Add("Delete");
			grdFCBank.RootTable.Columns["Delete"].Key = "Delete";
			grdFCBank.RootTable.Columns["Delete"].Caption = "X";
			grdFCBank.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFCBank.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdFCBank.RootTable.Columns["Delete"].ButtonText = "X";
			grdFCBank.RootTable.Columns["Delete"].Width = 20;
			grdFCBank.RootTable.Columns["Delete"].Position = 0;
			grdFCBank.FrozenColumns = 1;
			CommonServices.GridAutoAdjustmentNew(grdFCBank);
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
			MessageBox.Show("Customer  Field Required");
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
		if (Conversion.ToInt(cmbPaymentTerm.Value) == 2 && ((UltraGridBase)CmbConsignee).ActiveRow == null)
		{
			MessageBox.Show("Consignee Field Required");
			CmbConsignee.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbPaymentTerm.Value) == 2 && txtFINo.Text == string.Empty)
		{
			txtFINo.Focus();
			MessageBox.Show("FI No Field Required");
			return false;
		}
		if (Conversion.ToInt(cmbPaymentTerm.Value) == 1 && ((Control)(object)cmbInvoiceNo).Text == string.Empty)
		{
			MessageBox.Show("Invoice Field Required");
			cmbInvoiceNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbaccounttype).ActiveRow == null)
		{
			MessageBox.Show(" Account Type Field Required");
			cmbaccounttype.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFBC.Text) > 0.0 && ((UltraGridBase)CmbFBCDebitAccount).ActiveRow == null)
		{
			MessageBox.Show("FBC Debit Account Field Required");
			CmbFBCDebitAccount.Focus();
			return false;
		}
		if (((UltraGridBase)cmbbankAcc).ActiveRow == null)
		{
			MessageBox.Show(" Account (DR) Field Required");
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

	private void RESETMAIN()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
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
			((Control)(object)CmbConsignee).Text = "";
			txtFINo.Text = "";
			txtFBC.Text = "";
			((Control)(object)CmbFBCDebitAccount).Text = "";
			((Control)(object)cmbFcycodeThirdParty).Text = "";
			txtfcyAmountThirdParty.Text = "";
			txtFcyExchRthirdParty.Text = "";
			txtHcyExchRthirdParty.Text = "";
			txtReceiverExchangeRate.Text = "";
			txtReceiverFcyAmount.Text = "";
			txtFCGrosamount.Text = "";
			txtForeinchargesPkr.Text = "";
			txtNetAmountFC.Text = "";
			txtexchangeRate.Text = "";
			txtNetAmount.Text = "";
			((Control)(object)cmbaccounttype).Text = "";
			txtbankDrAmount.Text = "";
			txtLocalAmount.Text = "";
			((UltraGridBase)CmbChargesType).Rows[0].Activate();
			txtRemarks.Clear();
			grdFCBank.ClearStructure();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			VoucherHeadId = 0;
			BrancheId = 0;
			ProjectId = 0;
			txtExportvoucherRate.Text = "0";
			((Control)(object)cmbReferenceNo).Text = string.Empty;
			if (Conversion.ToInt(cmbPaymentTerm.Value) != 2)
			{
				lblConsignee.Visible = false;
				((Control)(object)CmbConsignee).Visible = false;
				labFiNo.Visible = false;
				txtFINo.Visible = false;
				txtRemarks.Width = 555;
			}
			((Control)(object)grdPaymentTerms).Visible = false;
			dtPaymentTermsDetail.Rows.Clear();
			grdPaymentTerms.DataSource = dtPaymentTermsDetail;
			grdPaymentTerms.RetrieveStructure();
			gridPaymentTermsSettings();
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
			GetAllAccounts();
			CurrencyCOde();
			ChargerType();
			ReferenceNoFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetdetail()
	{
		txtChargePercent.Text = "";
		txtRemarksDetail.Text = "";
		CmbChargesAccountType.Value = "";
		txtfcamount.Text = "";
		txtpkramount.Text = "";
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

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Expected O, but got Unknown
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0795: Expected O, but got Unknown
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Expected O, but got Unknown
		//IL_089b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a2: Expected O, but got Unknown
		//IL_09df: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a22: Expected O, but got Unknown
		//IL_0a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!FormValidation())
			{
				return;
			}
			ExImFCBankReceipts fcbank = new ExImFCBankReceipts();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				fcbank.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				double GrossAmount = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double FBC = Conversion.ToDouble((object)txtFBC.Text);
				double BalanceAmount = Conversion.ToDouble((object)txtBalanceAmount.Text);
				if (GrossAmount > BalanceAmount)
				{
					throw new Exception("FCY Amount cannot be greater than Balance Amount please check");
				}
				double ThisReciptTotal = Conversion.ToDouble(grdPaymentTerms.GetTotal(grdPaymentTerms.RootTable.Columns["ThisReceipt"], (AggregateFunction)2));
				if (Conversion.ToDecimal((object)ThisReciptTotal) != Conversion.ToDecimal((object)GrossAmount) + Conversion.ToDecimal((object)FBC))
				{
					throw new Exception($"Fcy Amount not equal to Payment term grid total. Fcy Amount is {GrossAmount} & Payment Grid Total is {ThisReciptTotal}.");
				}
			}
			if (Conversion.ToDouble((object)txtfcyAmountThirdParty.Text) > 0.0)
			{
				if (((UltraGridBase)cmbFcycodeThirdParty).ActiveRow == null || Conversion.ToInt(cmbFcycodeThirdParty.Value) == 0)
				{
					MessageBox.Show("Third Currency field is required");
					return;
				}
				if (Conversion.ToDouble((object)txtFcyExchRthirdParty.Text) == 0.0)
				{
					MessageBox.Show("Third Currency Customer ExchangeRate field is required");
					return;
				}
			}
			fcbank.ProjectId = Conversion.ToInt(cmbproject.Value);
			fcbank.BranchId = Conversion.ToInt(cmbranches.Value);
			fcbank.DocumentTypeId = 203;
			fcbank.DocumentNo = Conversion.ToInt((object)txtdocNumber.Text);
			fcbank.DocumentDate = Conversion.ToDateTime((object)txtdate.Value);
			fcbank.SupplierCustomerId = Conversion.ToInt(comSuplier.Value);
			fcbank.ExImLcOrderId = Conversion.ToInt(cmbLCOrder.Value);
			fcbank.ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			fcbank.DebitAccountTypeId = Conversion.ToInt(cmbaccounttype.Value);
			fcbank.BankGlDrAc = Conversion.ToInt(cmbbankAcc.Value);
			fcbank.BankFbpNo = Conversion.ToString((object)txtbankfbpNo.Text);
			fcbank.ExmLcPaymentTerm = ((Control)(object)cmbPaymentTerm).Text;
			fcbank.MultiCurrencyId = Conversion.ToInt(cmbcurrency.Value);
			fcbank.CurrencyCode = Conversion.ToString((object)((Control)(object)cmbcurrency).Text);
			fcbank.FcGrossAmount = Conversion.ToDouble((object)txtFCGrosamount.Text);
			fcbank.FcFBCharges = Conversion.ToDouble((object)txtFBC.Text);
			fcbank.FBCDebitAccountId = Conversion.ToInt(CmbFBCDebitAccount.Value);
			fcbank.FcNetAmount = Conversion.ToDouble((object)txtNetAmountFC.Text);
			fcbank.ThirdCurrencyId = Conversion.ToInt(cmbFcycodeThirdParty.Value);
			fcbank.ThirdCurrencyCode = Conversion.ToString((object)((Control)(object)cmbFcycodeThirdParty).Text);
			fcbank.ThirdCurrencyHcyExchangeRate = Conversion.ToDouble((object)txtHcyExchRthirdParty.Text);
			fcbank.ThirdCurrencyFcyExchangeRate = Conversion.ToDouble((object)txtFcyExchRthirdParty.Text);
			fcbank.ThirdCurrencyAmount = Conversion.ToDouble((object)txtfcyAmountThirdParty.Text);
			fcbank.ThirdCurrencyReceiverExchangeRate = Conversion.ToDouble((object)txtReceiverExchangeRate.Text);
			fcbank.ThirdCurrencyReceiverFcyAmount = Conversion.ToDouble((object)txtReceiverFcyAmount.Text);
			fcbank.ExchangeRate = Conversion.ToDouble((object)txtexchangeRate.Text);
			fcbank.NetAmountRs = Conversion.ToDouble((object)txtNetAmount.Text);
			fcbank.BankDrAmount = Conversion.ToDouble((object)txtbankDrAmount.Text);
			fcbank.VoucherHeadId = Conversion.ToInt(CmbGainLossAc.Value);
			fcbank.FcBankChargesAmountRs = fcbank.FcFBCharges * fcbank.ExchangeRate;
			fcbank.FcBankChargesTypeAcId = 0;
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
			fcbank.ReferenceNo = ((Control)(object)cmbReferenceNo).Text;
			fcbank.FcBankReceiptDetail = new List<ExImFcBankReceiptsDeductions>();
			fcbank.FcyBankReceiptBreakUpList = new List<FcyBankReceiptBreakUp>();
			fcbank.FcyBankReceiptBreakUpofPartyInvoiceslist = new List<FcyBankReceiptBreakUpofPartyInvoices>();
			fcbank.FcyPartyPaymentBreackUpForFinancialslist = new List<FcyPartyPaymentBreackUpForFinancials>();
			fcbank.ExImShipmentPartyAgainstAdvancesPaymentslist = new List<ExImShipmentPartyAgainstAdvancesPayment>();
			fcbank.FcyBankReceiptPaymentTermDetails = new List<FcyBankReceiptPaymentTermDetail>();
			double Fcamount = 0.0;
			double RsAmount = 0.0;
			GridEXRow[] dataRows = grdFCBank.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				ExImFcBankReceiptsDeductions brd = new ExImFcBankReceiptsDeductions();
				brd.Type = Conversion.ToString((object)r.Cells["Type"].Text);
				brd.ExImProceedsChargesTypeId = Conversion.ToInt(r.Cells["ChargesTypeId"].Value);
				brd.ProceedsRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
				brd.ChargesPercent = Conversion.ToDouble((object)r.Cells["Prcnt"].Text);
				brd.FcAmount = Conversion.ToDouble((object)r.Cells["FC-Amount"].Text);
				brd.RsAmount = Conversion.ToDouble((object)r.Cells["Rs-Amount"].Text);
				fcbank.FcBankReceiptDetail.Add(brd);
				Fcamount += brd.FcAmount;
				RsAmount += brd.RsAmount;
			}
			if (Fcamount >= Conversion.ToDouble((object)txtFCGrosamount.Text))
			{
				throw new Exception("Fcy Bank Charges Amount can not greater than or equal to Fcy Amount");
			}
			if (RsAmount >= Conversion.ToDouble((object)txtNetAmount.Text))
			{
				throw new Exception("Bank Charges Amount can not greater than or equal to Amount(PKR) Debit Amount");
			}
			if (((Control)(object)cmbPaymentTerm).Text == "Invoice Payment")
			{
				int LineId = 1;
				dataRows = grdPaymentTerms.GetRows();
				foreach (GridEXRow item in dataRows)
				{
					FcyBankReceiptPaymentTermDetail pd = new FcyBankReceiptPaymentTermDetail();
					pd.ExImInvoicePaymentTermsDetailId = Conversion.ToInt(item.Cells["InvoicePaymentDetailId"].Value);
					pd.ThisReceipt = Conversion.ToDouble(item.Cells["ThisReceipt"].Value);
					pd.LineId = LineId;
					fcbank.FcyBankReceiptPaymentTermDetails.Add(pd);
					LineId++;
				}
			}
			if (Conversion.ToInt(cmbPaymentTerm.Value) == 1 || Conversion.ToInt(cmbPaymentTerm.Value) == 2)
			{
				FcyBankReceiptBreakUp Break = new FcyBankReceiptBreakUp();
				Break.Id = 0;
				Break.SenderRefNo = "";
				Break.ReceiverRefNo = fcbank.BankFbpNo;
				Break.StepStatus = "Final Part";
				Break.RefDocRecordId = ((Conversion.ToInt(cmbPaymentTerm.Value) == 1) ? Conversion.ToInt(cmbInvoiceNo.Value) : ((Conversion.ToInt(cmbPaymentTerm.Value) == 2) ? Conversion.ToInt(cmbLCOrder.Value) : 0));
				Break.RefDocumentTypeId = ((Conversion.ToInt(cmbPaymentTerm.Value) == 1) ? 1 : ((Conversion.ToInt(cmbPaymentTerm.Value) == 2) ? 2 : 0));
				Break.RealizedAmount = Conversion.ToDecimal((object)fcbank.FcGrossAmount);
				Break.AgencyComm = 0m;
				Break.ActionTypeId = 1;
				Break.SortNo = 1;
				fcbank.FcyBankReceiptBreakUpList.Add(Break);
			}
			if (Conversion.ToInt(cmbPaymentTerm.Value) == 2)
			{
				ExImShipmentPartyAgainstAdvancesPayment shpAdvanec = new ExImShipmentPartyAgainstAdvancesPayment();
				shpAdvanec.ContractId = Conversion.ToInt(cmbLCOrder.Value);
				shpAdvanec.ConsigneeId = Conversion.ToInt(CmbConsignee.Value);
				shpAdvanec.FINo = Conversion.ToString((object)txtFINo.Text.Trim());
				fcbank.ExImShipmentPartyAgainstAdvancesPaymentslist.Add(shpAdvanec);
			}
			fcbank.AttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ExImFCBankReceipts>(fcbank, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = ExImFCBankReceipts.Save(fcbank);
			if (Id == 0)
			{
				MessageBox.Show("Record Save Successfully [" + fcbank.DocumentNo + "]");
			}
			else
			{
				MessageBox.Show("Record Update Successfully [" + fcbank.DocumentNo + "]");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(Id, base.Name, 203, 0);
				}
			}
			RESETMAIN();
			Id = success;
			VoucherHeadIdGet();
			if (chkpreview.Checked)
			{
				DataTable dt = new DataTable();
				dt = VoucherReports.VoucherReport(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 203,
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
			}
			Id = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		try
		{
			Id = ID;
			PreviousBalance = 0.0;
			ExImFCBankReceipts fcbank = new ExImFCBankReceipts();
			fcbank = ExImFCBankReceipts.GetByID(ID);
			if (fcbank != null)
			{
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				VoucherHeadIdGet();
				txtdocNumber.Text = fcbank.DocumentNo.ToString();
				tabControl1.SelectedIndex = 0;
				txtdate.Value = fcbank.DocumentDate;
				((Control)(object)cmbPaymentTerm).Text = fcbank.ExmLcPaymentTerm;
				txtFCGrosamount.Text = fcbank.FcGrossAmount.ToString("#,#.###");
				PreviousBalance = Conversion.ToDouble((object)txtFCGrosamount.Text.Trim());
				if (fcbank.ExmLcPaymentTerm == "Advanced Payment" || fcbank.ExImLcOrderId > 0)
				{
					GetExportCustomerByAdvancePayment();
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("LcOrderNo");
					dt.Rows.Add(fcbank.ExImLcOrderId, fcbank.LcOrderNo);
					DDL.BindDDL(dt, cmbLCOrder, "Id", "LcOrderNo", "Contract No", ZeroIndex: false);
					((UltraGridBase)cmbLCOrder).Rows[0].Activate();
					cmbLCOrder.ReadOnly = false;
					cmbInvoiceNo.ReadOnly = true;
					cmbPaymentTerm_Leave(null, null);
					if (fcbank.ExImShipmentPartyAgainstAdvancesPaymentslist.Count > 0)
					{
						lblConsignee.Visible = true;
						((Control)(object)CmbConsignee).Visible = true;
						labFiNo.Visible = true;
						txtFINo.Visible = true;
						txtRemarks.Width = 234;
						CmbConsignee.Value = Conversion.ToInt((object)fcbank.ExImShipmentPartyAgainstAdvancesPaymentslist[0].ConsigneeId);
						txtFINo.Text = Conversion.ToString((object)fcbank.ExImShipmentPartyAgainstAdvancesPaymentslist[0].FINo);
					}
				}
				if (Conversion.ToInt((object)fcbank.ExImInvoiceId) > 0)
				{
					CustomersFillByInvoicePayment();
					DataTable dtInvoice = new DataTable();
					dtInvoice.Columns.Add("Id");
					dtInvoice.Columns.Add("InvoiceNo");
					dtInvoice.Rows.Add(fcbank.ExImInvoiceId, fcbank.InvoiceNo);
					DDL.BindDDL(dtInvoice, cmbInvoiceNo, "Id", "InvoiceNo", "InvoiceNo", ZeroIndex: false);
					((UltraGridBase)cmbInvoiceNo).Rows[0].Activate();
					cmbInvoiceNo.ReadOnly = false;
					cmbLCOrder.ReadOnly = true;
					((Control)(object)grdPaymentTerms).Visible = true;
					cmbInvoiceNo_Leave(null, null);
				}
				if (fcbank.ExmLcPaymentTerm == "Other Payment")
				{
					GetAllExportCustomer();
				}
				if (Conversion.ToInt((object)fcbank.ExImInvoiceId) == 0 && Conversion.ToInt((object)fcbank.ExImLcOrderId) == 0)
				{
					CustomersFillByInvoicePayment();
				}
				comSuplier.Value = fcbank.SupplierCustomerId;
				cmbaccounttype.Value = fcbank.DebitAccountTypeId;
				cmbbankAcc.Value = fcbank.BankGlDrAc;
				txtbankfbpNo.Text = fcbank.BankFbpNo;
				cmbcurrency.Value = fcbank.MultiCurrencyId;
				txtNetAmountFC.Text = fcbank.FcNetAmount.ToString("#,#.###");
				txtexchangeRate.Text = Conversion.ToDouble((object)fcbank.ExchangeRate).ToString();
				cmbFcycodeThirdParty.Value = fcbank.ThirdCurrencyId;
				txtHcyExchRthirdParty.Text = fcbank.ThirdCurrencyHcyExchangeRate.ToString();
				txtFcyExchRthirdParty.Text = fcbank.ThirdCurrencyFcyExchangeRate.ToString();
				txtfcyAmountThirdParty.Text = fcbank.ThirdCurrencyAmount.ToString();
				txtReceiverExchangeRate.Text = fcbank.ThirdCurrencyReceiverExchangeRate.ToString();
				txtReceiverFcyAmount.Text = fcbank.ThirdCurrencyReceiverFcyAmount.ToString();
				txtNetAmount.Text = fcbank.NetAmountRs.ToString("#,#.##");
				txtbankDrAmount.Text = fcbank.BankDrAmount.ToString("#,#.##");
				CmbGainLossAc.Value = fcbank.VoucherHeadId;
				txtRemarks.Text = Conversion.ToString((object)fcbank.Remarks);
				((Control)(object)cmbReferenceNo).Text = fcbank.ReferenceNo;
				txtFBC.Text = Conversion.ToString((object)fcbank.FcFBCharges);
				CmbFBCDebitAccount.Value = Conversion.ToInt((object)fcbank.FBCDebitAccountId);
				if (Conversion.ToInt(cmbaccounttype.Value) == 1)
				{
					BankAccountDr(15);
				}
				if (Conversion.ToInt(cmbaccounttype.Value) == 2)
				{
					BankAccountDr(22);
				}
				if (Conversion.ToInt(cmbaccounttype.Value) == 3)
				{
					BankAccountDr(2);
				}
				table.Clear();
				double ForeignAmount = 0.0;
				double LocalAmount = 0.0;
				for (int i = 0; i < fcbank.FcBankReceiptDetail.Count; i++)
				{
					table.Rows.Add(1, fcbank.FcBankReceiptDetail[i].Type, fcbank.FcBankReceiptDetail[i].ExImProceedsChargesTypeId, fcbank.FcBankReceiptDetail[i].ProceedsChargesTypeCode, fcbank.FcBankReceiptDetail[i].ChargesPercent, fcbank.FcBankReceiptDetail[i].ProceedsRemarks, fcbank.FcBankReceiptDetail[i].FcAmount, fcbank.FcBankReceiptDetail[i].RsAmount);
					if (fcbank.FcBankReceiptDetail[i].Type == "Foreign")
					{
						ForeignAmount += fcbank.FcBankReceiptDetail[i].RsAmount;
					}
					else
					{
						LocalAmount += fcbank.FcBankReceiptDetail[i].RsAmount;
					}
				}
				txtForeinchargesPkr.Text = ForeignAmount.ToString("0,0");
				txtLocalAmount.Text = LocalAmount.ToString("0,0");
				grdFCBank.DataSource = table;
				grdFCBank.RetrieveStructure();
				gridsettings();
				FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
				NetAmountFC();
			}
			else
			{
				RESETMAIN();
			}
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
		else
		{
			txtdate.Focus();
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryCombosFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0106: Unknown result type (might be due to invalid IL or missing references)
				//IL_011c: Expected O, but got Unknown
				DataTable dataTable = new DataTable
				{
					Columns = 
					{
						{
							"Id",
							typeof(int)
						},
						{
							"Customer",
							typeof(string)
						}
					}
				};
				DataTable dataTable2 = new DataTable
				{
					Columns = 
					{
						{
							"Id",
							typeof(int)
						},
						{
							"Name",
							typeof(string)
						}
					}
				};
				DataTable dataTable3 = new DataTable
				{
					Columns = 
					{
						{
							"Id",
							typeof(int)
						},
						{
							"Name",
							typeof(string)
						}
					}
				};
				DataTable dataTable4 = new DataTable
				{
					Columns = 
					{
						{
							"Id",
							typeof(int)
						},
						{
							"Name",
							typeof(string)
						}
					}
				};
				DataTable dataForDropDownFromFcyBankReceipts = ExImFCBankReceipts.GetDataForDropDownFromFcyBankReceipts(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (dataForDropDownFromFcyBankReceipts.Rows.Count > 0)
				{
					for (int i = 0; i < dataForDropDownFromFcyBankReceipts.Rows.Count; i++)
					{
						if (Conversion.ToString(dataForDropDownFromFcyBankReceipts.Rows[i]["Activity"]) == "Customer")
						{
							dataTable.Rows.Add(dataForDropDownFromFcyBankReceipts.Rows[i]["Id"], dataForDropDownFromFcyBankReceipts.Rows[i]["ReferenceName"]);
						}
						else if (Conversion.ToString(dataForDropDownFromFcyBankReceipts.Rows[i]["Activity"]) == "PaymentTerm")
						{
							dataTable2.Rows.Add(dataForDropDownFromFcyBankReceipts.Rows[i]["Id"], dataForDropDownFromFcyBankReceipts.Rows[i]["ReferenceName"]);
						}
						else if (Conversion.ToString(dataForDropDownFromFcyBankReceipts.Rows[i]["Activity"]) == "ReceiverAccount")
						{
							dataTable4.Rows.Add(dataForDropDownFromFcyBankReceipts.Rows[i]["Id"], dataForDropDownFromFcyBankReceipts.Rows[i]["ReferenceName"]);
						}
						else if (Conversion.ToString(dataForDropDownFromFcyBankReceipts.Rows[i]["Activity"]) == "InvoiceNo")
						{
							dataTable3.Rows.Add(dataForDropDownFromFcyBankReceipts.Rows[i]["Id"], dataForDropDownFromFcyBankReceipts.Rows[i]["ReferenceName"]);
						}
					}
				}
				if (dataTable.Rows.Count > 0)
				{
					DDL.BindDDL(dataTable, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
				}
				if (dataTable2.Rows.Count > 0)
				{
					DDL.BindDDL(dataTable2, CmbPaymentTermHistory, "Id", "Name", "Payment Term", ZeroIndex: false);
					((UltraGridBase)CmbPaymentTermHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				if (dataTable3.Rows.Count > 0)
				{
					DDL.BindDDL(dataTable3, CmbInvoiceNoHistory, "Id", "Name", "Invoice No", ZeroIndex: false);
				}
				if (dataTable4.Rows.Count > 0)
				{
					DDL.BindDDL(dataTable4, CmbReceiverAcHistory, "Id", "Name", "Receiver/Bank Ac", ZeroIndex: false);
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			bindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void bindHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters br = new ReportsParameters();
			br.OrganizationId = UserAccount.OrganizationId;
			br.CompanyId = UserAccount.CompanyId;
			br.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			br.DocumentTypeId = 203;
			br.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!br.CanViewAllRecord)
			{
				br.EntryUser = UserAccount.ID;
			}
			if (FromDateHistory.Checked)
			{
				br.FromDate = FromDateHistory.Value;
			}
			if (ToDateHistory.Checked)
			{
				br.ToDate = ToDateHistory.Value;
			}
			br.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			br.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			br.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			br.PaymentTerm = Conversion.ToString((object)((Control)(object)CmbPaymentTermHistory).Text);
			br.InvoiceId = Conversion.ToInt(CmbInvoiceNoHistory.Value);
			br.ReceiverAcId = Conversion.ToInt(CmbReceiverAcHistory.Value);
			DataTable Historydt = ExImFCBankReceipts.HistoryForm(br);
			if (Historydt.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("VoucherHeadId", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocumentNo", typeof(int));
				dt.Columns.Add("DocumentDate", typeof(DateTime));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("InvoiceNo", typeof(string));
				dt.Columns.Add("PaymentTerm", typeof(string));
				dt.Columns.Add("AccountType", typeof(string));
				dt.Columns.Add("BankGLDrAc", typeof(string));
				dt.Columns.Add("BankFbpNo", typeof(string));
				dt.Columns.Add("BankDrAmount", typeof(double));
				dt.Columns.Add("CurrencyCode", typeof(string));
				dt.Columns.Add("FcGrossAmount", typeof(double));
				dt.Columns.Add("ReferenceNo", typeof(string));
				dt.Columns.Add("FBCAccount", typeof(string));
				dt.Columns.Add("FcFBCharges", typeof(double));
				dt.Columns.Add("FcNetAmount", typeof(double));
				dt.Columns.Add("ExchangeRate", typeof(double));
				dt.Columns.Add("NetAmountRs", typeof(double));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("UserName", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < Historydt.Rows.Count; i++)
				{
					if (Conversion.ToString(Historydt.Rows[i]["PaymentTerm"]) == "Invoice Payment" || Conversion.ToString(Historydt.Rows[i]["PaymentTerm"]) == "Advanced Payment")
					{
						dt.Rows.Add(Historydt.Rows[i]["Id"], Historydt.Rows[i]["VoucherHeadId"], Historydt.Rows[i]["DocumentTypeId"], Historydt.Rows[i]["DocumentNo"], Conversion.ToDateTime(Historydt.Rows[i]["DocumentDate"]), Historydt.Rows[i]["CustomerName"], Historydt.Rows[i]["InvoiceNo"], Historydt.Rows[i]["PaymentTerm"], Historydt.Rows[i]["AccountType"], Historydt.Rows[i]["BankGLDrAc"], Historydt.Rows[i]["BankFbpNo"], Historydt.Rows[i]["BankDrAmount"], Historydt.Rows[i]["CurrencyCode"], Historydt.Rows[i]["FcGrossAmount"], Historydt.Rows[i]["ReferenceNo"], Historydt.Rows[i]["FBCAccount"], Historydt.Rows[i]["FcFBCharges"], Historydt.Rows[i]["FcNetAmount"], Historydt.Rows[i]["ExchangeRate"], Historydt.Rows[i]["NetAmountRs"], Historydt.Rows[i]["EntryDate"], Historydt.Rows[i]["UserName"], Historydt.Rows[i]["NoOfAttachments"]);
					}
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
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

	private void HistoryGridSettings()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			DataGridHistory.RootTable.Columns["DocumentNo"].Width = Constants.ExportConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DocumentDate"].Width = Constants.ExportConstants.DateConstant;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			DataGridHistory.RootTable.Columns["PaymentTerm"].Width = Constants.ExportConstants.TermConstant;
			DataGridHistory.RootTable.Columns["AccountType"].Width = Constants.ExportConstants.CarrierTypeConstant;
			DataGridHistory.RootTable.Columns["BankGLDrAc"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["BankFbpNo"].Width = 120;
			DataGridHistory.RootTable.Columns["BankDrAmount"].Width = Constants.ExportConstants.LcyAmountConstant;
			DataGridHistory.RootTable.Columns["CurrencyCode"].Width = Constants.ExportConstants.FcyConstant;
			DataGridHistory.RootTable.Columns["FcGrossAmount"].Width = Constants.ExportConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["ReferenceNo"].Width = 120;
			DataGridHistory.RootTable.Columns["FBCAccount"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["FcFBCharges"].Width = Constants.ExportConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["FcNetAmount"].Width = Constants.ExportConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["ExchangeRate"].Width = Constants.ExportConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["NetAmountRs"].Width = Constants.ExportConstants.LcyAmountConstant;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["UserName"].Width = Constants.ExportConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants;
			DataGridHistory.RootTable.Columns["DocumentNo"].Caption = "Doc No";
			DataGridHistory.RootTable.Columns["DocumentDate"].Caption = "Doc Date";
			DataGridHistory.RootTable.Columns["CurrencyCode"].Caption = "Fcy Code";
			DataGridHistory.RootTable.Columns["DocumentDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Slip";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Slip";
			DataGridHistory.RootTable.Columns["Print"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("Voucher");
			DataGridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].Width = 65;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("GLBreakup");
			DataGridHistory.RootTable.Columns["GLBreakup"].Caption = "G&LBreakup";
			DataGridHistory.RootTable.Columns["GLBreakup"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["GLBreakup"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["GLBreakup"].ButtonText = "G&LBreakup";
			DataGridHistory.RootTable.Columns["GLBreakup"].Width = 60;
			DataGridHistory.RootTable.Columns["Voucher"].Position = 0;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.RootTable.Columns["Edit"].Position = 2;
			DataGridHistory.RootTable.Columns["GLBreakup"].Position = 3;
			DataGridHistory.FrozenColumns = 4;
			DataGridHistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BankDrAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BankDrAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BankDrAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["BankDrAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcGrossAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcGrossAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcGrossAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcGrossAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcFBCharges"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcFBCharges"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcFBCharges"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcFBCharges"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcNetAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcNetAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcNetAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcNetAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetAmountRs"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetAmountRs"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetAmountRs"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetAmountRs"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ExchangeRate"].FormatString = "#,##0.###";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
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

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Voucher")
			{
				DataTable dt2 = new DataTable();
				dt2 = VoucherReports.VoucherReport(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 203,
					Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["VoucherHeadId"].Value)
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
					Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value)
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
				RESETMAIN();
				Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
			if (e.Column.Key == "GLBreakup")
			{
				GainAndLossBreakUp gainAndLossBreakUp = new GainAndLossBreakUp(UserAccount);
				gainAndLossBreakUp.FcyReceiptId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				gainAndLossBreakUp.Show();
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
			Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(Id);
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ExImRptFCBankReceipts_516(Id);
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

	private void GenerateReportVoucher102()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.VoucherReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 203,
				Id = VoucherHeadId,
				ApprovedFilter = "All"
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

	private void Acfrmfcbankreceipt_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
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
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.Left)
				{
					CmbChargesType.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					((Control)(object)grdPaymentTerms).Focus();
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdate.Focus();
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnResetHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistory.Focus();
				}
			}
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		try
		{
			double InvoiceAmt = 0.0;
			double RecAmount = 0.0;
			double BalanceAmt = 0.0;
			txtInvoiceAmount.Text = "0";
			txtRecAmount.Text = "0";
			txtBalanceAmount.Text = "0";
			DataTable dt = ExImInvoice.GetFcYAmount(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbInvoiceNo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				InvoiceAmt = Conversion.ToDouble(dt.Rows[0]["InvoiceAmount"]);
				RecAmount = Conversion.ToDouble(dt.Rows[0]["FcyReceivedAmount"]);
				RecAmount -= PreviousBalance;
				BalanceAmt = InvoiceAmt - RecAmount;
			}
			txtInvoiceAmount.Text = InvoiceAmt.ToString("#,#.###");
			txtRecAmount.Text = RecAmount.ToString("#,#.###");
			txtBalanceAmount.Text = BalanceAmt.ToString("#,#.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtbankcharges_Leave(object sender, EventArgs e)
	{
		NetAmountFC();
	}

	private void ThirdExchRateByFcGross()
	{
		try
		{
			if (txtFCGrosamount.Text != string.Empty && txtfcyAmountThirdParty.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtfcyAmountThirdParty.Text);
				double FCGrosamount = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double FcyExchRthirdParty = num / FCGrosamount;
				txtFcyExchRthirdParty.Text = Conversion.ToString((object)Math.Round(FcyExchRthirdParty, 3));
			}
			else
			{
				txtFcyExchRthirdParty.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FcGrossAmtByThirdParty()
	{
		try
		{
			if (Conversion.ToDouble((object)txtFcyExchRthirdParty.Text) != 0.0 && Conversion.ToDouble((object)txtfcyAmountThirdParty.Text) != 0.0)
			{
				double num = Conversion.ToDouble((object)txtfcyAmountThirdParty.Text);
				double FcyExchRthirdParty = Conversion.ToDouble((object)txtFcyExchRthirdParty.Text);
				double ThirdFcAmt = num / FcyExchRthirdParty;
				txtFCGrosamount.Text = Conversion.ToString((object)Math.Round(ThirdFcAmt, 3));
			}
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
			CommonServices.CommasApplyWhileTyping(sender, e);
			if (txtFCGrosamount.Text != string.Empty && txtexchangeRate.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double b = Conversion.ToDouble((object)txtexchangeRate.Text);
				double c = num * b;
				txtNetAmount.Text = c.ToString("#,##0.####");
				txtbankDrAmount.Text = c.ToString("#,##0.####");
			}
			else
			{
				txtNetAmount.Text = "0";
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
				txtNetAmount.Text = (num * b).ToString("#,##0.####");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankDebitAmountCalculate()
	{
		try
		{
			if (txtFCGrosamount.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtNetAmount.Text);
				double c = double.Parse(grdFCBank.GetTotal(grdFCBank.RootTable.Columns["Rs-Amount"], (AggregateFunction)2).ToString());
				txtbankDrAmount.Text = (num - c).ToString("#,##0.####");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NetAmountFC()
	{
		try
		{
			if (!(txtexchangeRate.Text != string.Empty) || !(txtFCGrosamount.Text != string.Empty))
			{
				return;
			}
			double CommAmount = 0.0;
			double a = Conversion.ToDouble((object)txtexchangeRate.Text);
			double b = Conversion.ToDouble((object)txtFCGrosamount.Text);
			double c = a * b;
			txtbankDrAmount.Text = c.ToString("#,##0.####");
			txtNetAmountFC.Text = b.ToString("#,##0.####");
			double RsAmountDetail = Conversion.ToDouble((object)txtfcamount.Text) * a;
			txtpkramount.Text = RsAmountDetail.ToString("#,##0.####");
			if (grdFCBank.GetRows().Count() <= 0)
			{
				return;
			}
			double Fcamount = 0.0;
			double RsAmount = 0.0;
			GridEXRow[] dataRows = grdFCBank.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				r.BeginEdit();
				if (r.Cells["Type"].Text == "Foreign")
				{
					Fcamount += Conversion.ToDouble((object)r.Cells["FC-Amount"].Text);
				}
				if (Conversion.ToDouble((object)r.Cells["Rs-Amount"].Text.ToString()) > 0.0)
				{
					double prcnt = Conversion.ToDouble(r.Cells["Prcnt"].Value);
					if (prcnt > 0.0)
					{
						double amount = (b + CommAmount) * a * prcnt / 100.0;
						r.Cells["Rs-Amount"].Value = amount;
					}
					RsAmount += Conversion.ToDouble((object)r.Cells["Rs-Amount"].Text);
				}
				r.EndEdit();
			}
			double BankAmount = c - RsAmount;
			double FcyNetAmount = b - Fcamount;
			txtNetAmountFC.Text = FcyNetAmount.ToString("#,##0.####");
			txtbankDrAmount.Text = BankAmount.ToString("#,##0,####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comSuplier_Leave(object sender, EventArgs e)
	{
		PaymentTerm();
	}

	private void CmbGainLossAc_Leave(object sender, EventArgs e)
	{
		try
		{
			CmbChargesType.Focus();
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
			DataTable dt = ExImInvoice.GetExchangeRateandCurrencyFromVoucher(new ExImInvoice
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

	private void getPaymentTermDetailByInvoiceId(int InvoiceId)
	{
		try
		{
			dtPaymentTermsDetail.Rows.Clear();
			DataTable dt = ExImInvoice.getPaymentTermDetailByInvoiceId(InvoiceId, Id);
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow item in dt.Rows)
				{
					dtPaymentTermsDetail.Rows.Add(item["Id"], item["PaymentTermId"], item["PaymentTerm"], item["FcyAmount"], item["Received"], item["Balance"], item["ThisReceipt"]);
				}
				grdPaymentTerms.DataSource = dtPaymentTermsDetail;
				grdPaymentTerms.RetrieveStructure();
				gridPaymentTermsSettings();
			}
			else
			{
				grdPaymentTerms.DataSource = null;
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
				getPaymentTermDetailByInvoiceId(Conversion.ToInt(cmbInvoiceNo.Value));
			}
			else
			{
				dtPaymentTermsDetail.Rows.Clear();
				grdPaymentTerms.DataSource = null;
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

	private void txtpkramount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			CalculatePercntAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtfcamount_TextChanged(object sender, EventArgs e)
	{
		CommonServices.CommasApplyWhileTyping(sender, e);
	}

	private void txtFCGrosamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			if (txtFCGrosamount.Text != string.Empty && txtexchangeRate.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtFCGrosamount.Text);
				double b = Conversion.ToDouble((object)txtexchangeRate.Text);
				double c = num * b;
				txtNetAmount.Text = c.ToString("#,##0.####");
				txtbankDrAmount.Text = c.ToString("#,##0.####");
			}
			NetAmountFC();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SalesContractAmountGet()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImInvoice.GetFcYAmountByContractId(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbLCOrder.Value)
			});
			if (dt.Rows.Count > 0)
			{
				double InvoiceAmt = 0.0;
				double RecAmount = 0.0;
				double BalanceAmt = 0.0;
				InvoiceAmt = Conversion.ToDouble(dt.Rows[0]["InvoiceAmount"]);
				RecAmount = Conversion.ToDouble(dt.Rows[0]["FcyReceivedAmount"]);
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

	private void cmbLCOrder_Leave(object sender, EventArgs e)
	{
		try
		{
			SalesContractAmountGet();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbLCOrder_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			SalesContractAmountGet();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtfcyAmountThirdParty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			FcGrossAmtByThirdParty();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFcyExchRthirdParty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			FcGrossAmtByThirdParty();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFCGrosamount_Leave(object sender, EventArgs e)
	{
		try
		{
			ThirdExchRateByFcGross();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculatePercntAmount()
	{
		try
		{
			if (((UltraGridBase)CmbChargesType).ActiveRow != null)
			{
				double ChargePercent = Conversion.ToDouble((object)txtChargePercent.Text);
				double NetpkrAmount = Conversion.ToDouble((object)txtNetAmount.Text);
				Conversion.ToDouble((object)txtpkramount.Text);
				if (!(((Control)(object)CmbChargesType).Text == "Local") || !(NetpkrAmount > 0.0))
				{
					return;
				}
				if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "Percent")
				{
					if (ChargePercent > 0.0 && NetpkrAmount > 0.0)
					{
						double PkrAmount = NetpkrAmount * ChargePercent / 100.0;
						txtpkramount.Text = PkrAmount.ToString("#,##0.####");
					}
				}
				else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "PkrAmount")
				{
					txtChargePercent.Text = string.Empty;
				}
			}
			else
			{
				txtfcamount.Text = string.Empty;
				txtpkramount.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbChargesType_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculatePercntAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtChargePercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculatePercntAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtFCGrosamount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtexchangeRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtpkramount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtfcamount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtfcyAmountThirdParty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFcyExchRthirdParty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtHcyExchRthirdParty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtChargePercent_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtFBC_KeyPress(object sender, KeyPressEventArgs e)
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

	private void grdFCBank_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item2 = grdFCBank.CurrentRow;
				table.Rows[item2.RowIndex].Delete();
				if (item2 != null && (int)item2.RowType == 0)
				{
					item2.Delete();
				}
			}
			grdFCBank.UpdateData();
			BankDebitAmountCalculate();
			double LocalAmount = 0.0;
			double ForeignAmount = 0.0;
			GridEXRow[] rows = grdFCBank.GetRows();
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
			txtForeinchargesPkr.Text = ForeignAmount.ToString("#,##0.####");
			txtLocalAmount.Text = LocalAmount.ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LcOrder()
	{
		try
		{
			DataTable dtLcOrder = CommonServices.GetLcOrderNoBySupplierCustomerId(Conversion.ToInt(comSuplier.Value));
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

	private void BtnLoadInvoice_Click(object sender, EventArgs e)
	{
		try
		{
			LoadExportInvoice = new LoadExportInvoice(UserAccount);
			LoadExportInvoice.ShowDialog();
			LoadExportInvoice.BringToFront();
			BindInvoiceDataOnForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindInvoiceDataOnForm()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		try
		{
			if (LoadExportInvoice.dtSupply.Rows.Count > 0)
			{
				DataTable dt = ExImInvoice.GetInvoiceInformationByInvoiceIdForServices(new ExImInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(LoadExportInvoice.dtSupply.Rows[0]["InvoiceId"])
				});
				if (dt.Rows.Count > 0)
				{
					cmbPaymentTerm_Leave(null, null);
					comSuplier.Value = Conversion.ToInt(dt.Rows[0]["SupplierCustomerId"]);
					InvoiceNo();
					cmbInvoiceNo.Value = Conversion.ToInt(LoadExportInvoice.dtSupply.Rows[0]["InvoiceId"]);
					cmbInvoiceNo_Leave(null, null);
				}
				else
				{
					((Control)(object)cmbLCOrder).Text = string.Empty;
					((Control)(object)cmbInvoiceNo).Text = string.Empty;
					((Control)(object)comSuplier).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridPaymentTermsSettings()
	{
		try
		{
			grdPaymentTerms.RootTable.HeaderLines = 2;
			grdPaymentTerms.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdPaymentTerms.RootTable.Columns["Received"].EditType = (EditType)0;
			grdPaymentTerms.RootTable.Columns["Balance"].EditType = (EditType)0;
			grdPaymentTerms.RootTable.Columns["PaymentTerm"].EditType = (EditType)0;
			grdPaymentTerms.RootTable.Columns["PaymentTermId"].Visible = false;
			grdPaymentTerms.RootTable.Columns["InvoicePaymentDetailId"].Visible = false;
			grdPaymentTerms.RootTable.Columns["PaymentTerm"].Width = 70;
			grdPaymentTerms.RootTable.Columns["PaymentTerm"].HeaderAlignment = (TextAlignment)2;
			grdPaymentTerms.RootTable.Columns["Amount"].Width = 70;
			grdPaymentTerms.RootTable.Columns["Received"].Width = 70;
			grdPaymentTerms.RootTable.Columns["ThisReceipt"].Width = 70;
			grdPaymentTerms.RootTable.Columns["ThisReceipt"].HeaderAlignment = (TextAlignment)2;
			grdPaymentTerms.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerms.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPaymentTerms.RootTable.Columns["Amount"].FormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["Amount"].TotalFormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["Received"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerms.RootTable.Columns["Received"].TextAlignment = (TextAlignment)3;
			grdPaymentTerms.RootTable.Columns["Received"].FormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["Received"].TotalFormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["Balance"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerms.RootTable.Columns["Balance"].TextAlignment = (TextAlignment)3;
			grdPaymentTerms.RootTable.Columns["Balance"].FormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["Balance"].TotalFormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["ThisReceipt"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerms.RootTable.Columns["ThisReceipt"].TextAlignment = (TextAlignment)3;
			grdPaymentTerms.RootTable.Columns["ThisReceipt"].FormatString = "#,#.###";
			grdPaymentTerms.RootTable.Columns["ThisReceipt"].TotalFormatString = "#,#.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Acfrmfcbankreceipt_FormClosing(object sender, FormClosingEventArgs e)
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

	private void cmbaccounttype_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbaccounttype.Value) == 1)
			{
				BankAccountDr(15);
			}
			if (Conversion.ToInt(cmbaccounttype.Value) == 2)
			{
				BankAccountDr(22);
			}
			if (Conversion.ToInt(cmbaccounttype.Value) == 3)
			{
				BankAccountDr(2);
			}
			if (Conversion.ToInt(cmbaccounttype.Value) == 4)
			{
				BankAccountDr(11);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoicenobreakup_Leave(object sender, EventArgs e)
	{
		try
		{
			InvoiceAmountGet();
			GetExchangeRateandCurrencyFromVoucher();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ThirdPartyReceiverAmountCalculations()
	{
		try
		{
			txtReceiverFcyAmount.Text = "0";
			if (Conversion.ToDouble((object)txtfcyAmountThirdParty.Text) != 0.0 && Conversion.ToDouble((object)txtReceiverExchangeRate.Text) != 0.0)
			{
				double num = Conversion.ToDouble((object)txtfcyAmountThirdParty.Text);
				double ReciverExchRthirdParty = Conversion.ToDouble((object)txtReceiverExchangeRate.Text);
				double ThirdFcAmt = num / ReciverExchRthirdParty;
				txtReceiverFcyAmount.Text = Conversion.ToString((object)Math.Round(ThirdFcAmt, 3));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtReceiverExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			ThirdPartyReceiverAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerms_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdPaymentTerms.CurrentRow;
			if (!(e.Column.Key == "ThisReceipt"))
			{
				return;
			}
			if (double.TryParse(item.Cells["Balance"].Value?.ToString(), out var balance) && double.TryParse(item.Cells["ThisReceipt"].Value?.ToString(), out var thisReceipt))
			{
				if (thisReceipt > balance)
				{
					item.Cells["ThisReceipt"].Value = 0;
					MessageBox.Show($"This Receipt cannot be greater than the balance amount. Balance amount is {balance} and This Receipt is {thisReceipt}.");
				}
			}
			else
			{
				MessageBox.Show("Invalid input in This Receipt field.");
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
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Expected O, but got Unknown
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Expected O, but got Unknown
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Expected O, but got Unknown
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Expected O, but got Unknown
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Expected O, but got Unknown
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Expected O, but got Unknown
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Expected O, but got Unknown
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Expected O, but got Unknown
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f1: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Expected O, but got Unknown
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097d: Expected O, but got Unknown
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Expected O, but got Unknown
		//IL_099f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a9: Expected O, but got Unknown
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Expected O, but got Unknown
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2d: Expected O, but got Unknown
		//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a43: Expected O, but got Unknown
		//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9b: Expected O, but got Unknown
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Expected O, but got Unknown
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Expected O, but got Unknown
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b98: Expected O, but got Unknown
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bae: Expected O, but got Unknown
		//IL_0bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc4: Expected O, but got Unknown
		//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c95: Expected O, but got Unknown
		//IL_0ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ced: Expected O, but got Unknown
		//IL_0cf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d03: Expected O, but got Unknown
		//IL_0d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d19: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d66: Expected O, but got Unknown
		//IL_13e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ee: Expected O, but got Unknown
		//IL_1531: Unknown result type (might be due to invalid IL or missing references)
		//IL_153b: Expected O, but got Unknown
		//IL_a07e: Unknown result type (might be due to invalid IL or missing references)
		//IL_a088: Expected O, but got Unknown
		//IL_a095: Unknown result type (might be due to invalid IL or missing references)
		//IL_a09f: Expected O, but got Unknown
		GridEXLayout grdPaymentTerms_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFCBank_DesignTimeLayout = new GridEXLayout();
		Appearance appearance12 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance146 = new Appearance();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
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
		Appearance appearance52 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.frmFcyBankReceipt));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
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
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdPaymentTerms = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdFCBank = new GridEX();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label44 = new System.Windows.Forms.Label();
		this.txtChargePercent = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtfcamount = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.txtpkramount = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbChargesAccountType = new UltraCombo();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.CmbChargesType = new UltraCombo();
		this.label49 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label68 = new System.Windows.Forms.Label();
		this.txtFBC = new System.Windows.Forms.TextBox();
		this.cmbReferenceNo = new UltraCombo();
		this.label43 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtfcyAmountThirdParty = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtReceiverFcyAmount = new System.Windows.Forms.TextBox();
		this.txtHcyExchRthirdParty = new System.Windows.Forms.TextBox();
		this.label72 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label71 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbFcycodeThirdParty = new UltraCombo();
		this.txtReceiverExchangeRate = new System.Windows.Forms.TextBox();
		this.txtFcyExchRthirdParty = new System.Windows.Forms.TextBox();
		this.txtbankDrAmount = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtNetAmountFC = new System.Windows.Forms.TextBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label52 = new System.Windows.Forms.Label();
		this.txtInvoiceAmount = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.txtExportvoucherRate = new System.Windows.Forms.TextBox();
		this.txtRecAmount = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.txtBalanceAmount = new System.Windows.Forms.TextBox();
		this.label54 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.cmbaccounttype = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.CmbConsignee = new UltraCombo();
		this.lblConsignee = new System.Windows.Forms.Label();
		this.CmbFBCDebitAccount = new UltraCombo();
		this.label69 = new System.Windows.Forms.Label();
		this.CmbGainLossAc = new UltraCombo();
		this.txtForeinchargesPkr = new System.Windows.Forms.TextBox();
		this.label45 = new System.Windows.Forms.Label();
		this.txtLocalAmount = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.txtNetAmount = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.cmbInvoiceNo = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.comSuplier = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtFINo = new System.Windows.Forms.TextBox();
		this.labFiNo = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.cmbPaymentTerm = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.cmbranches = new UltraCombo();
		this.txtdate = new System.Windows.Forms.DateTimePicker();
		this.cmbproject = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.txtexchangeRate = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtFCGrosamount = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtbankfbpNo = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbcurrency = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbbankAcc = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbLCOrder = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtdocNumber = new System.Windows.Forms.TextBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.chkpreview = new System.Windows.Forms.CheckBox();
		this.panel7 = new System.Windows.Forms.Panel();
		this.btnAttachments = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLoadInvoice = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label70 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.label60 = new System.Windows.Forms.Label();
		this.CmbReceiverAcHistory = new UltraCombo();
		this.label59 = new System.Windows.Forms.Label();
		this.CmbInvoiceNoHistory = new UltraCombo();
		this.label58 = new System.Windows.Forms.Label();
		this.CmbPaymentTermHistory = new UltraCombo();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label67 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.label89 = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label38 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerms).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFCBank).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesAccountType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesType).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceNo).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFcycodeThirdParty).BeginInit();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbaccounttype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbConsignee).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFBCDebitAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGainLossAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comSuplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbankAcc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLCOrder).BeginInit();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		this.btnAttachments.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel15.SuspendLayout();
		this.groupBox6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReceiverAcHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1240, 749);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1240, 749);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1232, 720);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1226, 714);
		this.panel2.TabIndex = 1;
		this.panel4.Controls.Add(this.panel10);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 381);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1226, 333);
		this.panel4.TabIndex = 5;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTerms);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(578, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(428, 333);
		this.panel10.TabIndex = 1;
		grdPaymentTerms_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTerms.DesignTimeLayout = grdPaymentTerms_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerms).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerms).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTerms.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerms).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerms).Name = "grdPaymentTerms";
		((ScrollableBase)this.grdPaymentTerms).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerms).Size = new System.Drawing.Size(428, 333);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerms).TabIndex = 12434;
		this.grdPaymentTerms.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTerms.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTerms).VisualStyle = (VisualStyle)4;
		this.grdPaymentTerms.CellUpdated += new ColumnActionEventHandler(grdPaymentTerms_CellUpdated);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdFCBank);
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(578, 333);
		this.panel5.TabIndex = 0;
		grdFCBank_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFCBank.DesignTimeLayout = grdFCBank_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFCBank).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFCBank).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.grdFCBank).Location = new System.Drawing.Point(0, 118);
		((System.Windows.Forms.Control)(object)this.grdFCBank).Name = "grdFCBank";
		((ScrollableBase)this.grdFCBank).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFCBank).Size = new System.Drawing.Size(578, 215);
		((System.Windows.Forms.Control)(object)this.grdFCBank).TabIndex = 0;
		this.grdFCBank.TotalRow = (InheritableBoolean)1;
		this.grdFCBank.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFCBank).VisualStyle = (VisualStyle)4;
		this.grdFCBank.ColumnButtonClick += new ColumnActionEventHandler(grdFCBank_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFCBank).DoubleClick += new System.EventHandler(grdFCBank_DoubleClick);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Controls.Add(this.txtChargePercent);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Controls.Add(this.txtfcamount);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.txtpkramount);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChargesAccountType);
		this.groupBox2.Controls.Add(this.btnAddinGrid);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox2.Controls.Add(this.txtRemarksDetail);
		this.groupBox2.Controls.Add(this.label47);
		this.groupBox2.Controls.Add(this.button1);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChargesType);
		this.groupBox2.Controls.Add(this.label49);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(578, 118);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Export Charges";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(342, 21);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(35, 15);
		this.label44.TabIndex = 129;
		this.label44.Text = "Prcnt";
		this.txtChargePercent.BackColor = System.Drawing.Color.White;
		this.txtChargePercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtChargePercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtChargePercent.Location = new System.Drawing.Point(340, 41);
		this.txtChargePercent.Name = "txtChargePercent";
		this.txtChargePercent.Size = new System.Drawing.Size(49, 23);
		this.txtChargePercent.TabIndex = 3;
		this.txtChargePercent.Tag = "Percent";
		this.txtChargePercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtChargePercent.TextChanged += new System.EventHandler(txtChargePercent_TextChanged);
		this.txtChargePercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtChargePercent_KeyPress);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(6, 21);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(77, 15);
		this.label48.TabIndex = 126;
		this.label48.Text = "Charges Type";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(459, 21);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(12, 15);
		this.label32.TabIndex = 31;
		this.label32.Text = "*";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(169, 21);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(12, 15);
		this.label34.TabIndex = 34;
		this.label34.Text = "*";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(90, 21);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(78, 15);
		this.label35.TabIndex = 33;
		this.label35.Text = "Account Title";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(392, 21);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(67, 15);
		this.label33.TabIndex = 30;
		this.label33.Text = "FC Amount";
		this.txtfcamount.BackColor = System.Drawing.Color.White;
		this.txtfcamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfcamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfcamount.Location = new System.Drawing.Point(390, 41);
		this.txtfcamount.Name = "txtfcamount";
		this.txtfcamount.Size = new System.Drawing.Size(106, 23);
		this.txtfcamount.TabIndex = 4;
		this.txtfcamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfcamount.TextChanged += new System.EventHandler(txtfcamount_TextChanged);
		this.txtfcamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfcamount_KeyPress);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(6, 68);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(66, 15);
		this.label37.TabIndex = 36;
		this.label37.Text = "Rs Amount";
		this.txtpkramount.BackColor = System.Drawing.Color.White;
		this.txtpkramount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpkramount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpkramount.Location = new System.Drawing.Point(3, 85);
		this.txtpkramount.Name = "txtpkramount";
		this.txtpkramount.Size = new System.Drawing.Size(116, 23);
		this.txtpkramount.TabIndex = 5;
		this.txtpkramount.Tag = "PkrAmount";
		this.txtpkramount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtpkramount.TextChanged += new System.EventHandler(txtpkramount_TextChanged);
		this.txtpkramount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtpkramount_KeyPress);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(79, 68);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(12, 15);
		this.label36.TabIndex = 37;
		this.label36.Text = "*";
		this.CmbChargesAccountType.AllowNull = (DefaultableBoolean)1;
		this.CmbChargesAccountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChargesAccountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChargesAccountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChargesAccountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChargesAccountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).Location = new System.Drawing.Point(86, 39);
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).Name = "CmbChargesAccountType";
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.CmbChargesAccountType).TabIndex = 1;
		this.btnAddinGrid.BackColor = System.Drawing.Color.Teal;
		this.btnAddinGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddinGrid.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddinGrid.ForeColor = System.Drawing.Color.White;
		this.btnAddinGrid.Location = new System.Drawing.Point(358, 84);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(27, 25);
		this.btnAddinGrid.TabIndex = 7;
		this.btnAddinGrid.Text = "+";
		this.btnAddinGrid.UseVisualStyleBackColor = false;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(363, 85);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(65, 23);
		this.btnUpdateDetail.TabIndex = 8;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(429, 85);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 23);
		this.btnCancelUpdateDetial.TabIndex = 9;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.txtRemarksDetail.BackColor = System.Drawing.Color.White;
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(120, 85);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(238, 23);
		this.txtRemarksDetail.TabIndex = 6;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(120, 68);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(52, 15);
		this.label47.TabIndex = 123;
		this.label47.Text = "Remarks";
		this.button1.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.button1.Location = new System.Drawing.Point(313, 41);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(27, 23);
		this.button1.TabIndex = 2;
		this.button1.TabStop = false;
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.CmbChargesType.AllowNull = (DefaultableBoolean)1;
		this.CmbChargesType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChargesType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChargesType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChargesType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChargesType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Location = new System.Drawing.Point(7, 39);
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Name = "CmbChargesType";
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.CmbChargesType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbChargesType).Leave += new System.EventHandler(CmbChargesType_Leave);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(79, 21);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(12, 15);
		this.label49.TabIndex = 127;
		this.label49.Text = "*";
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 56);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1226, 325);
		this.panel3.TabIndex = 0;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.label68);
		this.groupBox1.Controls.Add(this.txtFBC);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReferenceNo);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.groupBox3);
		this.groupBox1.Controls.Add(this.txtbankDrAmount);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtNetAmountFC);
		this.groupBox1.Controls.Add(this.groupBox4);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbaccounttype);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbConsignee);
		this.groupBox1.Controls.Add(this.lblConsignee);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount);
		this.groupBox1.Controls.Add(this.label69);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGainLossAc);
		this.groupBox1.Controls.Add(this.txtForeinchargesPkr);
		this.groupBox1.Controls.Add(this.label45);
		this.groupBox1.Controls.Add(this.txtLocalAmount);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Controls.Add(this.label55);
		this.groupBox1.Controls.Add(this.txtNetAmount);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label56);
		this.groupBox1.Controls.Add(this.label57);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comSuplier);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtFINo);
		this.groupBox1.Controls.Add(this.labFiNo);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPaymentTerm);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbranches);
		this.groupBox1.Controls.Add(this.txtdate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbproject);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.txtexchangeRate);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.txtFCGrosamount);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtbankfbpNo);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcurrency);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbankAcc);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLCOrder);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtdocNumber);
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1001, 324);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Fcy Receipt";
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.Location = new System.Drawing.Point(2, 262);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(92, 15);
		this.label68.TabIndex = 12431;
		this.label68.Text = "Foreign Charges";
		this.txtFBC.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFBC.Location = new System.Drawing.Point(125, 257);
		this.txtFBC.Name = "txtFBC";
		this.txtFBC.Size = new System.Drawing.Size(234, 25);
		this.txtFBC.TabIndex = 12430;
		this.txtFBC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFBC_KeyPress);
		this.cmbReferenceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbReferenceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReferenceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReferenceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance226).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance226).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance227;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance235).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReferenceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReferenceNo).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbReferenceNo).Location = new System.Drawing.Point(125, 199);
		((System.Windows.Forms.Control)(object)this.cmbReferenceNo).Name = "cmbReferenceNo";
		((System.Windows.Forms.Control)(object)this.cmbReferenceNo).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbReferenceNo).TabIndex = 7;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(2, 205);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(72, 15);
		this.label43.TabIndex = 12428;
		this.label43.Text = "Reference # ";
		this.groupBox3.Controls.Add(this.txtfcyAmountThirdParty);
		this.groupBox3.Controls.Add(this.label27);
		this.groupBox3.Controls.Add(this.txtReceiverFcyAmount);
		this.groupBox3.Controls.Add(this.txtHcyExchRthirdParty);
		this.groupBox3.Controls.Add(this.label72);
		this.groupBox3.Controls.Add(this.label21);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label71);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFcycodeThirdParty);
		this.groupBox3.Controls.Add(this.txtReceiverExchangeRate);
		this.groupBox3.Controls.Add(this.txtFcyExchRthirdParty);
		this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(689, 117);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(293, 130);
		this.groupBox3.TabIndex = 19;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Third Currency Info";
		this.txtfcyAmountThirdParty.BackColor = System.Drawing.Color.White;
		this.txtfcyAmountThirdParty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfcyAmountThirdParty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfcyAmountThirdParty.Location = new System.Drawing.Point(76, 49);
		this.txtfcyAmountThirdParty.Name = "txtfcyAmountThirdParty";
		this.txtfcyAmountThirdParty.Size = new System.Drawing.Size(213, 23);
		this.txtfcyAmountThirdParty.TabIndex = 1;
		this.txtfcyAmountThirdParty.TextChanged += new System.EventHandler(txtfcyAmountThirdParty_TextChanged);
		this.txtfcyAmountThirdParty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfcyAmountThirdParty_KeyPress);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(1, 54);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(72, 15);
		this.label27.TabIndex = 12434;
		this.label27.Text = "Fcy Amount";
		this.txtReceiverFcyAmount.BackColor = System.Drawing.Color.White;
		this.txtReceiverFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReceiverFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReceiverFcyAmount.Location = new System.Drawing.Point(201, 102);
		this.txtReceiverFcyAmount.Name = "txtReceiverFcyAmount";
		this.txtReceiverFcyAmount.ReadOnly = true;
		this.txtReceiverFcyAmount.Size = new System.Drawing.Size(88, 23);
		this.txtReceiverFcyAmount.TabIndex = 5;
		this.txtReceiverFcyAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtHcyExchRthirdParty_KeyPress);
		this.txtHcyExchRthirdParty.BackColor = System.Drawing.Color.White;
		this.txtHcyExchRthirdParty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHcyExchRthirdParty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtHcyExchRthirdParty.Location = new System.Drawing.Point(201, 76);
		this.txtHcyExchRthirdParty.Name = "txtHcyExchRthirdParty";
		this.txtHcyExchRthirdParty.Size = new System.Drawing.Size(88, 23);
		this.txtHcyExchRthirdParty.TabIndex = 3;
		this.txtHcyExchRthirdParty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtHcyExchRthirdParty_KeyPress);
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(128, 107);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(67, 13);
		this.label72.TabIndex = 12432;
		this.label72.Text = "Fcy Amount";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(128, 81);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(65, 13);
		this.label21.TabIndex = 12432;
		this.label21.Text = "Hcy Ex.Rate";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1, 26);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(55, 15);
		this.label16.TabIndex = 12430;
		this.label16.Text = "Fcy Code";
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(1, 107);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(65, 13);
		this.label71.TabIndex = 12428;
		this.label71.Text = "Rec Ex.Rate";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(1, 81);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(70, 13);
		this.label2.TabIndex = 12428;
		this.label2.Text = "Cust Ex.Rate";
		this.cmbFcycodeThirdParty.AllowNull = (DefaultableBoolean)1;
		this.cmbFcycodeThirdParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFcycodeThirdParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFcycodeThirdParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance201;
		((SpecialBoxBase)((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance152).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance209).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFcycodeThirdParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFcycodeThirdParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbFcycodeThirdParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFcycodeThirdParty).Location = new System.Drawing.Point(76, 20);
		((System.Windows.Forms.Control)(object)this.cmbFcycodeThirdParty).Name = "cmbFcycodeThirdParty";
		((System.Windows.Forms.Control)(object)this.cmbFcycodeThirdParty).Size = new System.Drawing.Size(213, 26);
		((System.Windows.Forms.Control)(object)this.cmbFcycodeThirdParty).TabIndex = 0;
		this.txtReceiverExchangeRate.BackColor = System.Drawing.Color.White;
		this.txtReceiverExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReceiverExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReceiverExchangeRate.Location = new System.Drawing.Point(76, 102);
		this.txtReceiverExchangeRate.Name = "txtReceiverExchangeRate";
		this.txtReceiverExchangeRate.Size = new System.Drawing.Size(46, 23);
		this.txtReceiverExchangeRate.TabIndex = 4;
		this.txtReceiverExchangeRate.TextChanged += new System.EventHandler(txtReceiverExchangeRate_TextChanged);
		this.txtReceiverExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFcyExchRthirdParty_KeyPress);
		this.txtFcyExchRthirdParty.BackColor = System.Drawing.Color.White;
		this.txtFcyExchRthirdParty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyExchRthirdParty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyExchRthirdParty.Location = new System.Drawing.Point(76, 76);
		this.txtFcyExchRthirdParty.Name = "txtFcyExchRthirdParty";
		this.txtFcyExchRthirdParty.Size = new System.Drawing.Size(46, 23);
		this.txtFcyExchRthirdParty.TabIndex = 2;
		this.txtFcyExchRthirdParty.TextChanged += new System.EventHandler(txtFcyExchRthirdParty_TextChanged);
		this.txtFcyExchRthirdParty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFcyExchRthirdParty_KeyPress);
		this.txtbankDrAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtbankDrAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbankDrAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbankDrAmount.Location = new System.Drawing.Point(487, 171);
		this.txtbankDrAmount.Name = "txtbankDrAmount";
		this.txtbankDrAmount.ReadOnly = true;
		this.txtbankDrAmount.Size = new System.Drawing.Size(193, 23);
		this.txtbankDrAmount.TabIndex = 14;
		this.txtbankDrAmount.TabStop = false;
		this.txtbankDrAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(373, 147);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(80, 15);
		this.label17.TabIndex = 39;
		this.label17.Text = "Amount(PKR)";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(373, 175);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(96, 15);
		this.label12.TabIndex = 42;
		this.label12.Text = "Bank Dr Amount";
		this.txtNetAmountFC.BackColor = System.Drawing.SystemColors.Menu;
		this.txtNetAmountFC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetAmountFC.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetAmountFC.Location = new System.Drawing.Point(487, 198);
		this.txtNetAmountFC.Name = "txtNetAmountFC";
		this.txtNetAmountFC.ReadOnly = true;
		this.txtNetAmountFC.Size = new System.Drawing.Size(193, 23);
		this.txtNetAmountFC.TabIndex = 15;
		this.txtNetAmountFC.TabStop = false;
		this.txtNetAmountFC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.groupBox4.Controls.Add(this.label52);
		this.groupBox4.Controls.Add(this.txtInvoiceAmount);
		this.groupBox4.Controls.Add(this.label50);
		this.groupBox4.Controls.Add(this.txtExportvoucherRate);
		this.groupBox4.Controls.Add(this.txtRecAmount);
		this.groupBox4.Controls.Add(this.label53);
		this.groupBox4.Controls.Add(this.txtBalanceAmount);
		this.groupBox4.Controls.Add(this.label54);
		this.groupBox4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(689, 0);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(293, 116);
		this.groupBox4.TabIndex = 18;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Invoice Info";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(6, 18);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(126, 15);
		this.label52.TabIndex = 134;
		this.label52.Text = "Invoice Exchange Rate";
		this.txtInvoiceAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtInvoiceAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceAmount.Location = new System.Drawing.Point(132, 39);
		this.txtInvoiceAmount.Name = "txtInvoiceAmount";
		this.txtInvoiceAmount.ReadOnly = true;
		this.txtInvoiceAmount.Size = new System.Drawing.Size(157, 23);
		this.txtInvoiceAmount.TabIndex = 1800;
		this.txtInvoiceAmount.TabStop = false;
		this.txtInvoiceAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(6, 43);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(93, 15);
		this.label50.TabIndex = 129;
		this.label50.Text = "Invoice Amount";
		this.txtExportvoucherRate.BackColor = System.Drawing.SystemColors.Menu;
		this.txtExportvoucherRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExportvoucherRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExportvoucherRate.Location = new System.Drawing.Point(132, 14);
		this.txtExportvoucherRate.Name = "txtExportvoucherRate";
		this.txtExportvoucherRate.ReadOnly = true;
		this.txtExportvoucherRate.Size = new System.Drawing.Size(157, 23);
		this.txtExportvoucherRate.TabIndex = 1600;
		this.txtExportvoucherRate.TabStop = false;
		this.txtExportvoucherRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRecAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtRecAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRecAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRecAmount.Location = new System.Drawing.Point(132, 64);
		this.txtRecAmount.Name = "txtRecAmount";
		this.txtRecAmount.ReadOnly = true;
		this.txtRecAmount.Size = new System.Drawing.Size(157, 23);
		this.txtRecAmount.TabIndex = 1900;
		this.txtRecAmount.TabStop = false;
		this.txtRecAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(6, 68);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(101, 15);
		this.label53.TabIndex = 136;
		this.label53.Text = "Received Amount";
		this.txtBalanceAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtBalanceAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceAmount.Location = new System.Drawing.Point(132, 89);
		this.txtBalanceAmount.Name = "txtBalanceAmount";
		this.txtBalanceAmount.ReadOnly = true;
		this.txtBalanceAmount.Size = new System.Drawing.Size(157, 23);
		this.txtBalanceAmount.TabIndex = 2000;
		this.txtBalanceAmount.TabStop = false;
		this.txtBalanceAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(6, 93);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(116, 15);
		this.label54.TabIndex = 138;
		this.label54.Text = "Invoice Balance Amt";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(373, 202);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(96, 15);
		this.label13.TabIndex = 33;
		this.label13.Text = "Net Amount FCY";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(470, 175);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(12, 15);
		this.label28.TabIndex = 14;
		this.label28.Text = "*";
		this.cmbaccounttype.AllowNull = (DefaultableBoolean)1;
		this.cmbaccounttype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbaccounttype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbaccounttype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbaccounttype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.cmbaccounttype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance93).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbaccounttype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbaccounttype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbaccounttype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbaccounttype).Location = new System.Drawing.Point(125, 228);
		((System.Windows.Forms.Control)(object)this.cmbaccounttype).Name = "cmbaccounttype";
		((System.Windows.Forms.Control)(object)this.cmbaccounttype).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbaccounttype).TabIndex = 8;
		this.cmbaccounttype.ValueChanged += new System.EventHandler(cmbaccounttype_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbaccounttype).Leave += new System.EventHandler(cmbaccounttype_Leave);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Red;
		this.label31.Location = new System.Drawing.Point(470, 202);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(12, 15);
		this.label31.TabIndex = 13;
		this.label31.Text = "*";
		this.CmbConsignee.AllowNull = (DefaultableBoolean)1;
		this.CmbConsignee.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbConsignee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbConsignee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance220).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbConsignee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Location = new System.Drawing.Point(487, 284);
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Name = "CmbConsignee";
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.CmbConsignee).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Leave += new System.EventHandler(CmbGainLossAc_Leave);
		this.lblConsignee.AutoSize = true;
		this.lblConsignee.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblConsignee.Location = new System.Drawing.Point(373, 292);
		this.lblConsignee.Name = "lblConsignee";
		this.lblConsignee.Size = new System.Drawing.Size(62, 15);
		this.lblConsignee.TabIndex = 140;
		this.lblConsignee.Text = "Consignee";
		this.CmbFBCDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbFBCDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFBCDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFBCDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance164;
		((SpecialBoxBase)((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance230).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFBCDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFBCDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount).Location = new System.Drawing.Point(487, 255);
		((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount).Name = "CmbFBCDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.CmbFBCDebitAccount).Leave += new System.EventHandler(CmbGainLossAc_Leave);
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.Location = new System.Drawing.Point(373, 263);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(76, 15);
		this.label69.TabIndex = 140;
		this.label69.Text = "FBC Debit Ac";
		this.CmbGainLossAc.AllowNull = (DefaultableBoolean)1;
		this.CmbGainLossAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGainLossAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGainLossAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGainLossAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance100;
		((SpecialBoxBase)((UltraGridBase)this.CmbGainLossAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance141).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGainLossAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGainLossAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Location = new System.Drawing.Point(487, 225);
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Name = "CmbGainLossAc";
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).TabIndex = 16;
		((System.Windows.Forms.Control)(object)this.CmbGainLossAc).Leave += new System.EventHandler(CmbGainLossAc_Leave);
		this.txtForeinchargesPkr.BackColor = System.Drawing.SystemColors.Menu;
		this.txtForeinchargesPkr.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtForeinchargesPkr.Location = new System.Drawing.Point(805, 273);
		this.txtForeinchargesPkr.Name = "txtForeinchargesPkr";
		this.txtForeinchargesPkr.ReadOnly = true;
		this.txtForeinchargesPkr.Size = new System.Drawing.Size(173, 23);
		this.txtForeinchargesPkr.TabIndex = 1300;
		this.txtForeinchargesPkr.Text = "0";
		this.txtForeinchargesPkr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(687, 277);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(116, 15);
		this.label45.TabIndex = 120;
		this.label45.Text = "Foreign Charges PKR";
		this.txtLocalAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtLocalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLocalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLocalAmount.Location = new System.Drawing.Point(805, 248);
		this.txtLocalAmount.Name = "txtLocalAmount";
		this.txtLocalAmount.ReadOnly = true;
		this.txtLocalAmount.Size = new System.Drawing.Size(173, 23);
		this.txtLocalAmount.TabIndex = 1700;
		this.txtLocalAmount.TabStop = false;
		this.txtLocalAmount.Text = "0";
		this.txtLocalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(687, 252);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(104, 15);
		this.label51.TabIndex = 131;
		this.label51.Text = "Local Charges PKR";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(373, 234);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(76, 15);
		this.label55.TabIndex = 140;
		this.label55.Text = "Gain/Loss Ac";
		this.txtNetAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtNetAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetAmount.Location = new System.Drawing.Point(487, 143);
		this.txtNetAmount.Name = "txtNetAmount";
		this.txtNetAmount.ReadOnly = true;
		this.txtNetAmount.Size = new System.Drawing.Size(193, 23);
		this.txtNetAmount.TabIndex = 13;
		this.txtNetAmount.TabStop = false;
		this.txtNetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(470, 147);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(12, 15);
		this.label24.TabIndex = 10;
		this.label24.Text = "*";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.Color.Red;
		this.label56.Location = new System.Drawing.Point(111, 234);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(12, 15);
		this.label56.TabIndex = 143;
		this.label56.Text = "*";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(2, 234);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(77, 15);
		this.label57.TabIndex = 142;
		this.label57.Text = "AccountType";
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(125, 114);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		this.cmbInvoiceNo.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 4;
		this.cmbInvoiceNo.ValueChanged += new System.EventHandler(cmbInvoiceNo_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Leave += new System.EventHandler(cmbInvoiceNo_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(2, 120);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(59, 15);
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
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comSuplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSuplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.comSuplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSuplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.comSuplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comSuplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comSuplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.comSuplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comSuplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comSuplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comSuplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comSuplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comSuplier).Location = new System.Drawing.Point(125, 85);
		((System.Windows.Forms.Control)(object)this.comSuplier).Name = "comSuplier";
		((System.Windows.Forms.Control)(object)this.comSuplier).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.comSuplier).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.comSuplier).Leave += new System.EventHandler(comSuplier_Leave);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Red;
		this.label22.Location = new System.Drawing.Point(111, 120);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(12, 15);
		this.label22.TabIndex = 11;
		this.label22.Text = "*";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Red;
		this.label25.Location = new System.Drawing.Point(111, 91);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(12, 15);
		this.label25.TabIndex = 7;
		this.label25.Text = "*";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(2, 91);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(93, 15);
		this.label4.TabIndex = 6;
		this.label4.Text = "Customer Name";
		this.txtFINo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFINo.Location = new System.Drawing.Point(805, 298);
		this.txtFINo.Name = "txtFINo";
		this.txtFINo.Size = new System.Drawing.Size(173, 23);
		this.txtFINo.TabIndex = 20;
		this.labFiNo.AutoSize = true;
		this.labFiNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labFiNo.Location = new System.Drawing.Point(687, 303);
		this.labFiNo.Name = "labFiNo";
		this.labFiNo.Size = new System.Drawing.Size(114, 15);
		this.labFiNo.TabIndex = 121;
		this.labFiNo.Text = "Financial Instr (F.I) #";
		this.txtRemarks.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(125, 284);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(234, 23);
		this.txtRemarks.TabIndex = 18;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(2, 286);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(52, 15);
		this.label46.TabIndex = 121;
		this.label46.Text = "Remarks";
		this.cmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Location = new System.Drawing.Point(125, 54);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Name = "cmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Leave += new System.EventHandler(cmbPaymentTerm_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(1563, 67);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(12, 15);
		this.label42.TabIndex = 115;
		this.label42.Text = "*";
		this.label42.Visible = false;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(1554, 22);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(12, 15);
		this.label41.TabIndex = 8;
		this.label41.Text = "*";
		this.label41.Visible = false;
		this.cmbranches.AllowNull = (DefaultableBoolean)1;
		this.cmbranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.cmbranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance167).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance168).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbranches).Location = new System.Drawing.Point(1512, 46);
		((System.Windows.Forms.Control)(object)this.cmbranches).Name = "cmbranches";
		((System.Windows.Forms.Control)(object)this.cmbranches).Size = new System.Drawing.Size(89, 27);
		((System.Windows.Forms.Control)(object)this.cmbranches).TabIndex = 10000;
		((System.Windows.Forms.Control)(object)this.cmbranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbranches).Visible = false;
		this.txtdate.CustomFormat = "dd-MMM-yyyy";
		this.txtdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdate.Location = new System.Drawing.Point(210, 29);
		this.txtdate.Name = "txtdate";
		this.txtdate.Size = new System.Drawing.Size(149, 23);
		this.txtdate.TabIndex = 1;
		this.cmbproject.AllowNull = (DefaultableBoolean)1;
		this.cmbproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbproject).Location = new System.Drawing.Point(1512, 91);
		((System.Windows.Forms.Control)(object)this.cmbproject).Name = "cmbproject";
		((System.Windows.Forms.Control)(object)this.cmbproject).Size = new System.Drawing.Size(85, 27);
		((System.Windows.Forms.Control)(object)this.cmbproject).TabIndex = 12425;
		((System.Windows.Forms.Control)(object)this.cmbproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbproject).Visible = false;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(1509, 71);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(48, 17);
		this.label40.TabIndex = 114;
		this.label40.Text = "Project";
		this.label40.Visible = false;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(470, 61);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(12, 15);
		this.label30.TabIndex = 6;
		this.label30.Text = "*";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(1509, 21);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(47, 17);
		this.label39.TabIndex = 113;
		this.label39.Text = "Branch";
		this.label39.Visible = false;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(111, 60);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(12, 15);
		this.label29.TabIndex = 10;
		this.label29.Text = "*";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(470, 30);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(12, 15);
		this.label26.TabIndex = 5;
		this.label26.Text = "*";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Red;
		this.label23.Location = new System.Drawing.Point(470, 89);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(12, 16);
		this.label23.TabIndex = 8;
		this.label23.Text = "*";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(470, 118);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(12, 15);
		this.label20.TabIndex = 9;
		this.label20.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(111, 148);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(12, 15);
		this.label19.TabIndex = 11;
		this.label19.Text = "*";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(111, 177);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(12, 15);
		this.label6.TabIndex = 7;
		this.label6.Text = "*";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Red;
		this.label5.Location = new System.Drawing.Point(111, 30);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(12, 15);
		this.label5.TabIndex = 0;
		this.label5.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(373, 118);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(84, 15);
		this.label18.TabIndex = 36;
		this.label18.Text = "Exchange Rate";
		this.txtexchangeRate.BackColor = System.Drawing.Color.White;
		this.txtexchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtexchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtexchangeRate.Location = new System.Drawing.Point(487, 114);
		this.txtexchangeRate.Name = "txtexchangeRate";
		this.txtexchangeRate.Size = new System.Drawing.Size(193, 23);
		this.txtexchangeRate.TabIndex = 12;
		this.txtexchangeRate.Text = "0";
		this.txtexchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtexchangeRate.TextChanged += new System.EventHandler(txtexchangeRate_TextChanged);
		this.txtexchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtexchangeRate_KeyPress);
		this.txtexchangeRate.Leave += new System.EventHandler(txtexchangeRate_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(373, 90);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(74, 15);
		this.label14.TabIndex = 27;
		this.label14.Text = "FCY Amount";
		this.txtFCGrosamount.BackColor = System.Drawing.Color.White;
		this.txtFCGrosamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFCGrosamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFCGrosamount.Location = new System.Drawing.Point(487, 86);
		this.txtFCGrosamount.Name = "txtFCGrosamount";
		this.txtFCGrosamount.Size = new System.Drawing.Size(193, 23);
		this.txtFCGrosamount.TabIndex = 11;
		this.txtFCGrosamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFCGrosamount.TextChanged += new System.EventHandler(txtFCGrosamount_TextChanged);
		this.txtFCGrosamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFCGrosamount_KeyPress);
		this.txtFCGrosamount.Leave += new System.EventHandler(txtFCGrosamount_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(373, 61);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 21;
		this.label11.Text = "Bank FBP # ";
		this.txtbankfbpNo.BackColor = System.Drawing.Color.White;
		this.txtbankfbpNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbankfbpNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbankfbpNo.Location = new System.Drawing.Point(487, 57);
		this.txtbankfbpNo.Name = "txtbankfbpNo";
		this.txtbankfbpNo.Size = new System.Drawing.Size(193, 23);
		this.txtbankfbpNo.TabIndex = 10;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(2, 177);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(55, 15);
		this.label10.TabIndex = 24;
		this.label10.Text = "Fcy Code";
		this.cmbcurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbcurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance188;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Location = new System.Drawing.Point(125, 171);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Name = "cmbcurrency";
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).TabIndex = 6;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(373, 30);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(76, 15);
		this.label9.TabIndex = 18;
		this.label9.Text = "Account (Dr)";
		this.cmbbankAcc.AllowNull = (DefaultableBoolean)1;
		this.cmbbankAcc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbankAcc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbankAcc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbankAcc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.cmbbankAcc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbankAcc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbankAcc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).Location = new System.Drawing.Point(487, 24);
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).Name = "cmbbankAcc";
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.cmbbankAcc).TabIndex = 9;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(2, 148);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(101, 15);
		this.label7.TabIndex = 12;
		this.label7.Text = "Sales Contract No";
		this.cmbLCOrder.AllowNull = (DefaultableBoolean)1;
		this.cmbLCOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLCOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLCOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance239).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance239).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLCOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLCOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance240;
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
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLCOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Location = new System.Drawing.Point(125, 142);
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Name = "cmbLCOrder";
		this.cmbLCOrder.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).TabIndex = 5;
		this.cmbLCOrder.ValueChanged += new System.EventHandler(cmbLCOrder_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbLCOrder).Leave += new System.EventHandler(cmbLCOrder_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(2, 60);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(84, 15);
		this.label3.TabIndex = 9;
		this.label3.Text = "Payment Term";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(2, 30);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(110, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Voucher Date && No";
		this.txtdocNumber.BackColor = System.Drawing.SystemColors.Menu;
		this.txtdocNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocNumber.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocNumber.Location = new System.Drawing.Point(125, 28);
		this.txtdocNumber.Name = "txtdocNumber";
		this.txtdocNumber.ReadOnly = true;
		this.txtdocNumber.Size = new System.Drawing.Size(83, 23);
		this.txtdocNumber.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Controls.Add(this.chkpreview);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 28);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1226, 28);
		this.panel6.TabIndex = 0;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(11, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(107, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "FCY Receipt";
		this.chkpreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkpreview.AutoSize = true;
		this.chkpreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkpreview.ForeColor = System.Drawing.Color.White;
		this.chkpreview.Location = new System.Drawing.Point(1141, 3);
		this.chkpreview.Name = "chkpreview";
		this.chkpreview.Size = new System.Drawing.Size(84, 20);
		this.chkpreview.TabIndex = 20;
		this.chkpreview.Text = "Preview";
		this.chkpreview.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.btnAttachments);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1226, 28);
		this.panel7.TabIndex = 1;
		this.btnAttachments.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachments.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btnAttachments.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnsave, this.btnUpdate, this.btnPrint, this.btnAttachment, this.btnRefresh, this.btnSlip, this.toolStripSeparator1, this.BtnLoadInvoice });
		this.btnAttachments.Location = new System.Drawing.Point(0, 0);
		this.btnAttachments.Name = "btnAttachments";
		this.btnAttachments.Size = new System.Drawing.Size(1226, 27);
		this.btnAttachments.TabIndex = 0;
		this.btnAttachments.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = (System.Drawing.Image)resources.GetObject("btnPrint.Image");
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(108, 24);
		this.btnPrint.Text = "&Voucher102";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(116, 24);
		this.btnAttachment.Text = "Attachments";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(92, 24);
		this.btnSlip.Text = "516-Print";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnLoadInvoice.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnLoadInvoice.Image = (System.Drawing.Image)resources.GetObject("BtnLoadInvoice.Image");
		this.BtnLoadInvoice.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadInvoice.Name = "BtnLoadInvoice";
		this.BtnLoadInvoice.Size = new System.Drawing.Size(95, 24);
		this.BtnLoadInvoice.Text = "Load Invoice";
		this.BtnLoadInvoice.Click += new System.EventHandler(BtnLoadInvoice_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1232, 723);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 154);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1226, 566);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 49;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Controls.Add(this.ctrlGrdBar1);
		this.panel9.Controls.Add(this.label70);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 130);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1226, 24);
		this.panel9.TabIndex = 48;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1185, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 46;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1721, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(56, 26);
		this.ctrlGrdBar1.TabIndex = 45;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label70.AutoSize = true;
		this.label70.BackColor = System.Drawing.Color.Teal;
		this.label70.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label70.Location = new System.Drawing.Point(2, 2);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(142, 19);
		this.label70.TabIndex = 0;
		this.label70.Text = "Filtered Records";
		this.panel15.Controls.Add(this.groupBox6);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 55);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1226, 75);
		this.panel15.TabIndex = 0;
		this.groupBox6.Controls.Add(this.label60);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory);
		this.groupBox6.Controls.Add(this.label59);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory);
		this.groupBox6.Controls.Add(this.label58);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory);
		this.groupBox6.Controls.Add(this.btnShowHistory);
		this.groupBox6.Controls.Add(this.label67);
		this.groupBox6.Controls.Add(this.label73);
		this.groupBox6.Controls.Add(this.label89);
		this.groupBox6.Controls.Add(this.label90);
		this.groupBox6.Controls.Add(this.label91);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox6.Controls.Add(this.ToDocNoHistory);
		this.groupBox6.Controls.Add(this.FromDocNoHistory);
		this.groupBox6.Controls.Add(this.ToDateHistory);
		this.groupBox6.Controls.Add(this.FromDateHistory);
		this.groupBox6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox6.Location = new System.Drawing.Point(4, 1);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(1030, 72);
		this.groupBox6.TabIndex = 0;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Filters";
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(673, 41);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(65, 31);
		this.label60.TabIndex = 93;
		this.label60.Text = "Receiver / Bank Ac";
		this.CmbReceiverAcHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbReceiverAcHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReceiverAcHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReceiverAcHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance194;
		((SpecialBoxBase)((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance203).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance204).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReceiverAcHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbReceiverAcHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).Location = new System.Drawing.Point(739, 41);
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).Name = "CmbReceiverAcHistory";
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.CmbReceiverAcHistory).TabIndex = 7;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(397, 47);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(65, 15);
		this.label59.TabIndex = 91;
		this.label59.Text = "Invoice No";
		this.CmbInvoiceNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance135).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Location = new System.Drawing.Point(482, 41);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Name = "CmbInvoiceNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).TabIndex = 5;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(397, 20);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(84, 15);
		this.label58.TabIndex = 89;
		this.label58.Text = "Payment Term";
		this.CmbPaymentTermHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTermHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Location = new System.Drawing.Point(482, 14);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Name = "CmbPaymentTermHistory";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).TabIndex = 4;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(962, 41);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 8;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(673, 20);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(58, 15);
		this.label67.TabIndex = 87;
		this.label67.Text = "Customer";
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(214, 47);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(86, 15);
		this.label73.TabIndex = 86;
		this.label73.Text = "To Voucher No";
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(214, 20);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(101, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From Voucher No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(5, 47);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(5, 20);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance115;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(739, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 6;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(315, 43);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(315, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(69, 43);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Controls.Add(this.label38);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1226, 52);
		this.panel8.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1226, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.label38.AutoSize = true;
		this.label38.BackColor = System.Drawing.Color.Teal;
		this.label38.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label38.Location = new System.Drawing.Point(5, 29);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(165, 19);
		this.label38.TabIndex = 0;
		this.label38.Text = "Fcy Receipt History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1240, 749);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmFcyBankReceipt";
		this.Text = "Acfrmfcbankreceipt";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Acfrmfcbankreceipt_FormClosing);
		base.Load += new System.EventHandler(Acfrmfcbankreceipt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Acfrmfcbankreceipt_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerms).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFCBank).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesAccountType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesType).EndInit();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceNo).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFcycodeThirdParty).EndInit();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbaccounttype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbConsignee).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFBCDebitAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGainLossAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comSuplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbankAcc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLCOrder).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.btnAttachments.ResumeLayout(false);
		this.btnAttachments.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReceiverAcHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
