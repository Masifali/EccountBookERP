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
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class ReceiptByContract : Form
{
	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DataTable dt = new DataTable();

	private DataTable dtdetail = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtCoa = new DataTable();

	private DataTable dttransAgainst = new DataTable();

	private DataTable dtTransType = new DataTable();

	public bool UpdateMode;

	private Rightsobjects formright = new Rightsobjects();

	private int RecId;

	private int VoucherHeadId;

	private int updateDetailIndex;

	private double updateDetailIndexAmount;

	private bool HasSubsidiaryFeature;

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar1;

	private DateTimePicker datcheqdate;

	private Label label11;

	private DateTimePicker datPaymentDate;

	private Label lblChqNo;

	private TextBox txtpaytitle;

	private Label label6;

	internal UltraCombo CmbCreditAc;

	private Label label4;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnsave;

	private ToolStripButton print;

	internal UltraCombo CmbCustomerHistory;

	private Label label17;

	private Label txtdatetohistory;

	private Label label18;

	private DateTimePicker BillDateTo;

	private DateTimePicker BillDateFrom;

	private TextBox txtInvoiceNoFrom;

	private Label label19;

	private TextBox txtInvoiceNoTo;

	private Label label21;

	private Button btnsearchhistory;

	internal GridEX Gridhistory;

	private Label label29;

	private ComboBox CmbStatus;

	private Label label28;

	private Label label27;

	private DateTimePicker DueDateTo;

	private DateTimePicker DueDateFrom;

	private TextBox txtRemarks;

	private Label label1;

	private TextBox txtAmount;

	private Label label23;

	private TextBox txtbalance;

	internal UltraCombo CmbTranagainst;

	private Label label3;

	private Button btnAdd;

	private TextBox txtCustomerBalance;

	private Label label7;

	internal UltraCombo CmbSupplierName;

	private Label lblCustomer;

	private Label lblAmount;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnDelete;

	private ToolStripButton btnAttachment;

	internal UltraCombo CmbTaxType;

	private Label label13;

	private Panel panel3;

	private TextBox txtgrandtotal;

	private TextBox txtdifference;

	private Label label10;

	private Label label5;

	private Panel panel4;

	private Panel panel10;

	private Panel panel9;

	private GroupBox groupBox2;

	private Label lblChDate;

	private GroupBox groupBox1;

	private Label label2;

	private Panel panel11;

	private Panel panel2;

	private Label label14;

	private TextBox txtTaxPercent;

	private Label label15;

	internal UltraCombo CmbTaxDrAc;

	private CheckBox ChkBoxWthHolding;

	private TextBox txtTotalAmount;

	private Label label16;

	private TextBox txtTaxAmount;

	private Label label20;

	internal GridEX grdContract;

	internal GridEX grdDetail;

	private CtrlGrdBar ctrlGrdBar4;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel6;

	private Panel panel13;

	private Panel panel12;

	private Label label12;

	private TextBox txtCheqNo;

	private Label label22;

	private Label label24;

	internal UltraCombo CmbTaxCrAc;

	private Label label25;

	internal UltraCombo cmbTransType;

	private Label label8;

	private Label lblAdvance;

	private Label label26;

	private TextBox txtcustomerAdvBalance;

	internal UltraCombo cmbAdvanceAccount;

	private Panel panel5;

	private Label label9;

	private TextBox txtdocno;

	private ToolStripButton btnShortCutKey;

	public ReceiptByContract(UserAccount user)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private bool Formvalidation()
	{
		if (((Control)(object)CmbSupplierName).Text == "" || Convert.ToInt32(CmbSupplierName.Value) == 0)
		{
			MessageBox.Show("Please Select Supplier");
			CmbSupplierName.Focus();
			return false;
		}
		if (ChkBoxWthHolding.Checked)
		{
			if (((UltraGridBase)CmbTaxCrAc).ActiveRow == null)
			{
				MessageBox.Show("Tax Credit Ac field required");
				CmbTaxCrAc.Focus();
				return false;
			}
			if (((UltraGridBase)CmbTaxDrAc).ActiveRow == null)
			{
				MessageBox.Show("Tax Debit Ac field required");
				CmbTaxDrAc.Focus();
				return false;
			}
			if (txtTaxPercent.Text == "" || txtTaxPercent.Text == "0" || txtTaxPercent.Text == null)
			{
				MessageBox.Show("Tax Percent field required");
				txtTaxPercent.Focus();
				return false;
			}
			if (txtTaxAmount.Text == "" || txtTaxAmount.Text == "0" || txtTaxAmount.Text == null)
			{
				MessageBox.Show("Tax Amount field required");
				txtTaxAmount.Focus();
				return false;
			}
		}
		if (txtdocno.Text == "" || txtdocno.Text == "0" || txtdocno.Text == null)
		{
			MessageBox.Show("DocNo field required");
			txtdocno.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbTransType).ActiveRow == null)
		{
			MessageBox.Show("TransType field required");
			cmbTransType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbTranagainst).ActiveRow == null)
		{
			MessageBox.Show("Trans Against field required");
			CmbTranagainst.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCreditAc).ActiveRow == null)
		{
			MessageBox.Show("Credit Account field required");
			CmbCreditAc.Focus();
			return false;
		}
		if (txtAmount.Text == string.Empty || Convert.ToInt32(txtAmount.Text) == 0)
		{
			MessageBox.Show("Amount field required");
			txtAmount.Focus();
			return false;
		}
		if (((UltraGridBase)cmbTransType).ActiveRow == null && Conversion.ToInt(cmbTransType.Value) == 3 && Conversion.ToDouble((object)txtAmount.Text) > Conversion.ToDouble((object)txtbalance.Text))
		{
			MessageBox.Show("Amount Can not be Greater Than Balance Advanced Amount");
			txtAmount.Focus();
			return false;
		}
		if (Conversion.ToString((object)txtRemarks.Text) == "0" || txtRemarks.Text == string.Empty)
		{
			MessageBox.Show("Remarks field required");
			txtRemarks.Focus();
			return false;
		}
		return true;
	}

	private bool ValidateAmountWithContractBalnceAmt()
	{
		double TotalDetailGridAmount = 0.0;
		double TotalContractBalanceAmt = 0.0;
		if (grdDetail.GetRows().Count() > 0 && grdContract.GetRows().Count() > 0)
		{
			if (grdDetail.GetRows().Count() > 0)
			{
				TotalDetailGridAmount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2));
			}
			if (grdContract.GetRows().Count() > 0)
			{
				TotalContractBalanceAmt = Conversion.ToDouble(grdContract.GetTotal(grdContract.RootTable.Columns["BalanceAmount"], (AggregateFunction)2));
			}
			double Amount = Conversion.ToDouble((object)txtAmount.Text);
			if (TotalDetailGridAmount + Amount - updateDetailIndexAmount > TotalContractBalanceAmt)
			{
				MessageBox.Show("You Can not Add Amount Greater than Balance Total Amount");
				return false;
			}
			return true;
		}
		if (grdContract.GetRows().Count() > 0)
		{
			TotalContractBalanceAmt = Conversion.ToDouble(grdContract.GetTotal(grdContract.RootTable.Columns["BalanceAmount"], (AggregateFunction)2));
		}
		if (Conversion.ToDouble((object)txtAmount.Text) > TotalContractBalanceAmt)
		{
			MessageBox.Show("You Can not Add Amount Greater than Balance Total Amount");
			return false;
		}
		return true;
	}

	private void ReceiptByInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			tabPage1.AutoScroll = true;
			tabPage1.AutoScrollMargin = new Size(20, 20);
			tabPage1.AutoScrollMinSize = new Size(980, 700);
			string ScreenName = Conversion.ToString((object)base.Name);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			btnsave.Enabled = formright.DoHaveSaveRight;
			print.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar4.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar4.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar4.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar4.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar4.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar4.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			HasSubsidiaryFeature = CommonServices.GetERPFeatureById(4);
			datPaymentDate.Focus();
			SupplierCustomer();
			VoucherNofill();
			TransTypeFill();
			TransAgainstFill();
			BindTaxTypes();
			AccountsComboBind();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtCheqNo.Enabled = false;
			datcheqdate.Enabled = false;
			lblCustomer.Visible = false;
			lblAdvance.Visible = false;
			lblAmount.Visible = false;
			txtCustomerBalance.Visible = false;
			txtcustomerAdvBalance.Visible = false;
			txtbalance.Visible = false;
			dtViewAttachments.Columns.Add("AttchmentPath");
			dt.Columns.Add("Id");
			dt.Columns.Add("DocumentTypeId");
			dt.Columns.Add("PaymentTermsId");
			dt.Columns.Add("OrderDate");
			dt.Columns.Add("OrderNo");
			dt.Columns.Add("DueDate");
			dt.Columns.Add("PaymentTerm");
			dt.Columns.Add("OrderAmount", typeof(double));
			dt.Columns.Add("ReceivedAmount", typeof(double));
			dt.Columns.Add("BalanceAmount", typeof(double));
			dt.Columns.Add("TaxAmount", typeof(double));
			grdContract.DataSource = dt;
			grdContract.RetrieveStructure();
			GridContractSettings();
			dtdetail.Columns.Add("TransTypeId");
			dtdetail.Columns.Add("TransAgainstId");
			dtdetail.Columns.Add("GLAccountId");
			dtdetail.Columns.Add("AccountTitle");
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("CheqNo");
			dtdetail.Columns.Add("CheqDate");
			dtdetail.Columns.Add("Paytitle");
			dtdetail.Columns.Add("Remarks");
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			GridDetailSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountCurrentBalance()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = DateTime.Now,
				RefAccountId = Conversion.ToInt(CmbCreditAc.Value)
			});
			if (detail.Rows.Count > 0)
			{
				txtbalance.Text = Conversion.ToDouble(detail.Rows[0]["Balance"]).ToString("#,##0.#####");
				double num = Conversion.ToDouble((object)txtbalance.Text);
				if (num > 0.0)
				{
					lblAmount.Text = "Dr";
				}
				else
				{
					lblAmount.Text = "Cr";
				}
				if (num == 0.0)
				{
					lblAmount.Text = "Nill";
				}
				lblAmount.Visible = true;
				txtbalance.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerCurrentBalance()
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		try
		{
			DataTable dt1 = new DataTable();
			int GlId = 0;
			if (dtSupplier.Rows.Count > 0)
			{
				DataRow supplierRow = dtSupplier.AsEnumerable().FirstOrDefault((DataRow row) => Conversion.ToInt(row["Id"]) == Conversion.ToInt(CmbSupplierName.Value));
				if (supplierRow != null)
				{
					GlId = Conversion.ToInt(supplierRow["GlAccountId"]);
				}
			}
			if (!HasSubsidiaryFeature)
			{
				dt1 = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					VoucherDate = DateTime.Now,
					RefAccountId = GlId
				});
				txtCustomerBalance.Text = ((dt1.Rows.Count > 0) ? Conversion.ToDouble(dt1.Rows[0]["Balance"]).ToString("#,##0.#####") : "0");
			}
			else
			{
				dt1 = VoucherHead.GetGLAndSubsidiaryCurrentBalance(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ToDate = DateTime.Now,
					AccountId = GlId,
					SupplierCustomerId = Conversion.ToInt(CmbSupplierName.Value)
				});
				txtCustomerBalance.Text = ((dt1.Rows.Count > 0) ? Conversion.ToDouble(dt1.Rows[0]["BalanceAmount"]).ToString("#,##0.#####") : "0");
			}
			if (dt1.Rows.Count > 0)
			{
				double num = Conversion.ToDouble((object)txtCustomerBalance.Text);
				if (num > 0.0)
				{
					lblCustomer.Text = "Dr";
				}
				else
				{
					lblCustomer.Text = "Cr";
				}
				if (num == 0.0)
				{
					lblCustomer.Text = "Nill";
				}
				lblCustomer.Visible = true;
				txtCustomerBalance.Visible = true;
			}
			else
			{
				lblCustomer.Text = "";
				txtCustomerBalance.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AdvanceAccountCurrentBalance()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		try
		{
			DataTable dt1 = new DataTable();
			int GlId = 0;
			if (dtSupplier.Rows.Count > 0)
			{
				DataRow supplierRow = dtSupplier.AsEnumerable().FirstOrDefault((DataRow row) => Conversion.ToInt(row["Id"]) == Conversion.ToInt(CmbSupplierName.Value));
				if (supplierRow != null)
				{
					GlId = Conversion.ToInt((object)supplierRow["AdvanceGlAcId"].ToString());
				}
			}
			dt1 = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = DateTime.Now,
				RefAccountId = GlId
			});
			if (dt1.Rows.Count > 0)
			{
				txtcustomerAdvBalance.Text = ((dt1.Rows.Count > 0) ? Conversion.ToDouble(dt1.Rows[0]["Balance"]).ToString("#,##0.#####") : "0");
				double num = Conversion.ToDouble((object)txtcustomerAdvBalance.Text);
				if (num > 0.0)
				{
					lblAdvance.Text = "Dr";
				}
				else
				{
					lblAdvance.Text = "Cr";
				}
				if (num == 0.0)
				{
					lblAdvance.Text = "Nill";
				}
				lblAdvance.Visible = true;
				txtcustomerAdvBalance.Visible = true;
			}
			else
			{
				lblAdvance.Text = "";
				txtcustomerAdvBalance.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherNofill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			int Code = ReceiptByInvoiceHeader.GenerateCode(new ReceiptByInvoiceHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 29
			});
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierCustomer()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			SupplierCustomer obj = new SupplierCustomer();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtSupplier = SupplierCustomer.SupplierCustomerAgainstSaleOrder(obj);
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplier, CmbSupplierName, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
				DDL.BindDDL(dtSupplier, CmbCustomerHistory, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
				((UltraGridBase)CmbSupplierName).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbSupplierName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbSupplierName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbSupplierName).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				((UltraGridBase)CmbSupplierName).DisplayLayout.Bands[0].Columns[1].Width = 280;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierCustomerAdvanceAc()
	{
		try
		{
			if (dtSupplier.Rows.Count > 0)
			{
				if (((UltraGridBase)CmbSupplierName).ActiveRow != null)
				{
					DataTable dt = (from data in dtSupplier.AsEnumerable()
						where data.Field<int>("Id") == Conversion.ToInt(CmbSupplierName.Value)
						select data).CopyToDataTable();
					DDL.BindDDL(dt, cmbAdvanceAccount, "AdvanceGlAcId", "AdvanceAccount", "Account Name", ZeroIndex: false);
					((UltraGridBase)cmbAdvanceAccount).DisplayLayout.Bands[0].Columns[5].Width = 220;
					((UltraGridBase)cmbAdvanceAccount).DisplayLayout.Bands[0].Columns[1].Hidden = true;
					((UltraGridBase)cmbAdvanceAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)cmbAdvanceAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					((UltraGridBase)cmbAdvanceAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
					((UltraGridBase)cmbAdvanceAccount).Rows[0].Activate();
					cmbAdvanceAccount_Leave(null, null);
				}
				else
				{
					((Control)(object)cmbAdvanceAccount).Text = string.Empty;
					((UltraGridBase)cmbAdvanceAccount).DataSource = null;
				}
			}
			else
			{
				((Control)(object)cmbAdvanceAccount).Text = string.Empty;
				((UltraGridBase)cmbAdvanceAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransAgainstFill()
	{
		try
		{
			dttransAgainst.Columns.Add("Id");
			dttransAgainst.Columns.Add("Name");
			dttransAgainst.Rows.Add(1, "Cash");
			dttransAgainst.Rows.Add(2, "Bank");
			dttransAgainst.Rows.Add(3, "Party");
			DDL.BindDDLNew(dttransAgainst, CmbTranagainst, "Id", "Name", "Trans Against", false);
			((UltraGridBase)CmbTranagainst).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransTypeFill()
	{
		try
		{
			dtTransType.Columns.Add("Id");
			dtTransType.Columns.Add("Name");
			dtTransType.Rows.Add(1, "Regular");
			dtTransType.Rows.Add(2, "Advance");
			dtTransType.Rows.Add(3, "Advance Utilize");
			DDL.BindDDLNew(dtTransType, cmbTransType, "Id", "Name", "Trans Type", false);
			((UltraGridBase)cmbTransType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherhHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, 29);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindTaxTypes()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dt = TaxesTypes.GetForComboBind(new TaxesTypes
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Type = 1
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbTaxType, "Id", "TaxName", "TaxName", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplierName_ValueChanged(object sender, EventArgs e)
	{
	}

	private void AccountsComboBind()
	{
		try
		{
			DataTable dtcoalst = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtcoalst == null)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			dt.Clear();
			for (int i = 0; i < dtcoalst.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtcoalst.Rows[i]["AccountTypeId"]) != 2 && Convert.ToInt32(dtcoalst.Rows[i]["AccountTypeId"]) != 11 && Convert.ToInt32(dtcoalst.Rows[i]["AccountTypeId"]) != 15)
				{
					dt.Rows.Add(dtcoalst.Rows[i]["Id"], dtcoalst.Rows[i]["AccountTitle"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbTaxCrAc, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				DDL.BindDDL(dt, CmbTaxDrAc, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)CmbTaxDrAc).DisplayLayout.Bands[0].Columns[1].Width = 200;
				((UltraGridBase)CmbTaxCrAc).DisplayLayout.Bands[0].Columns[1].Width = 220;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTranType_Leave(object sender, EventArgs e)
	{
		try
		{
			dtCoa = CommonServices.CoaAllocationGetAllServiceBind();
			int id = Conversion.ToInt(CmbCreditAc.Value);
			((Control)(object)CmbCreditAc).Text = string.Empty;
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AccountTitle");
			if (((Control)(object)CmbTranagainst).Text == "Cash")
			{
				if (dtCoa.Rows.Count > 0)
				{
					for (int k = 0; k < dtCoa.Rows.Count; k++)
					{
						if (Convert.ToInt32(dtCoa.Rows[k]["AccountTypeId"]) == 2)
						{
							dt.Rows.Add(dtCoa.Rows[k]["Id"], dtCoa.Rows[k]["AccountTitle"]);
						}
					}
				}
				txtCheqNo.Enabled = false;
				datcheqdate.Enabled = false;
			}
			if (((Control)(object)CmbTranagainst).Text == "Bank")
			{
				if (dtCoa.Rows.Count > 0)
				{
					for (int j = 0; j < dtCoa.Rows.Count; j++)
					{
						if (Convert.ToInt32(dtCoa.Rows[j]["AccountTypeId"]) == 15)
						{
							dt.Rows.Add(dtCoa.Rows[j]["Id"], dtCoa.Rows[j]["AccountTitle"]);
						}
					}
				}
				txtCheqNo.Enabled = true;
				datcheqdate.Enabled = true;
			}
			if (((Control)(object)CmbTranagainst).Text == "Party" && dtCoa.Rows.Count > 0)
			{
				for (int i = 0; i < dtCoa.Rows.Count; i++)
				{
					if (Convert.ToInt32(dtCoa.Rows[i]["AccountTypeId"]) == 3)
					{
						dt.Rows.Add(dtCoa.Rows[i]["Id"], dtCoa.Rows[i]["AccountTitle"]);
					}
				}
			}
			DDL.BindDDL(dt, CmbCreditAc, "Id", "AccountTitle", "AccountTitle", ZeroIndex: false);
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbCreditAc.Value = id;
				}
				else
				{
					((Control)(object)CmbCreditAc).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsupcust_Leave(object sender, EventArgs e)
	{
		try
		{
			SupplierCustomerAdvanceAc();
			CustomerCurrentBalance();
			GridContractFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCreditAc_Leave(object sender, EventArgs e)
	{
		try
		{
			AccountCurrentBalance();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbAdvanceAccount_ValueChanged(object sender, EventArgs e)
	{
	}

	private void cmbTransType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbTransType).Text == "Advance")
			{
				((Control)(object)CmbTranagainst).Text = "Party";
				CmbTranType_Leave(null, null);
				((Control)(object)CmbTranagainst).Enabled = false;
				if (dtSupplier.Rows.Count > 0 && ((UltraGridBase)CmbSupplierName).ActiveRow != null)
				{
					DataTable dt = (from data in dtSupplier.AsEnumerable()
						where data.Field<int>("Id") == Conversion.ToInt(CmbSupplierName.Value)
						select data).CopyToDataTable();
					DDL.BindDDL(dt, CmbCreditAc, "AdvanceGlAcId", "AdvanceAccount", "Account Name", ZeroIndex: false);
					((UltraGridBase)CmbCreditAc).DisplayLayout.Bands[0].Columns[5].Width = 220;
					((UltraGridBase)CmbCreditAc).DisplayLayout.Bands[0].Columns[1].Hidden = true;
					((UltraGridBase)CmbCreditAc).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbCreditAc).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					((UltraGridBase)CmbCreditAc).DisplayLayout.Bands[0].Columns[4].Hidden = true;
					((UltraGridBase)CmbCreditAc).Rows[0].Activate();
				}
				else
				{
					MessageBox.Show("Please Select party first...");
					CmbSupplierName.Focus();
				}
			}
			else
			{
				((Control)(object)CmbTranagainst).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridContractFill()
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbSupplierName.Value) == 0 || ((Control)(object)CmbSupplierName).Text.Trim() == string.Empty)
			{
				grdContract.ClearStructure();
				CmbSupplierName.Focus();
				throw new Exception("Please select Customer Name first");
			}
			if (!btnsave.Visible || !btnsave.Enabled)
			{
				return;
			}
			DataTable lst = VoucherHead.GetPendingOrderForReceiptsVoucher(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SupplierCustomerId = Conversion.ToInt(CmbSupplierName.Value)
			});
			if (lst.Rows.Count > 0)
			{
				dt.Clear();
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["PaymentTermsId"], Convert.ToDateTime(lst.Rows[i]["OrderDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["OrderNo"], Convert.ToDateTime(lst.Rows[i]["DueDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["PaymentTerm"], lst.Rows[i]["OrderAmount"], lst.Rows[i]["ReceivedAmount"], Conversion.ToDouble(lst.Rows[i]["BalanceAmount"]), 0);
				}
				grdContract.DataSource = dt;
				grdContract.RetrieveStructure();
				GridContractSettings();
				TaxPropotionate();
			}
			else
			{
				grdContract.ClearStructure();
			}
			GrandTotal();
			TotalReceivingAmtForContractGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridContractSettings()
	{
		try
		{
			grdContract.RootTable.Columns["Id"].Visible = false;
			grdContract.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdContract.RootTable.Columns["PaymentTermsId"].Visible = false;
			grdContract.RootTable.Columns["OrderDate"].Width = 90;
			grdContract.RootTable.Columns["OrderNo"].Width = 75;
			grdContract.RootTable.Columns["DueDate"].Width = 90;
			grdContract.RootTable.Columns["PaymentTerm"].Width = 100;
			grdContract.RootTable.Columns["OrderAmount"].Width = 100;
			grdContract.RootTable.Columns["ReceivedAmount"].Width = 115;
			grdContract.RootTable.Columns["BalanceAmount"].Width = 110;
			grdContract.RootTable.Columns["TaxAmount"].Width = 100;
			grdContract.RootTable.Columns["Id"].EditType = (EditType)0;
			grdContract.RootTable.Columns["DocumentTypeId"].EditType = (EditType)0;
			grdContract.RootTable.Columns["PaymentTermsId"].EditType = (EditType)0;
			grdContract.RootTable.Columns["OrderDate"].EditType = (EditType)0;
			grdContract.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grdContract.RootTable.Columns["DueDate"].EditType = (EditType)0;
			grdContract.RootTable.Columns["PaymentTerm"].EditType = (EditType)0;
			grdContract.RootTable.Columns["OrderAmount"].EditType = (EditType)0;
			grdContract.RootTable.Columns["BalanceAmount"].EditType = (EditType)0;
			grdContract.RootTable.Columns["TaxAmount"].EditType = (EditType)0;
			grdContract.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			grdContract.RootTable.Columns["ReceivedAmount"].AggregateFunction = (AggregateFunction)2;
			grdContract.RootTable.Columns["BalanceAmount"].AggregateFunction = (AggregateFunction)2;
			grdContract.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grdContract.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			grdContract.RootTable.Columns["ReceivedAmount"].TextAlignment = (TextAlignment)3;
			grdContract.RootTable.Columns["BalanceAmount"].TextAlignment = (TextAlignment)3;
			grdContract.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grdContract.RootTable.Columns["OrderAmount"].FormatString = "#,#;(#,#);0";
			grdContract.RootTable.Columns["ReceivedAmount"].FormatString = "#,#;(#,#);0";
			grdContract.RootTable.Columns["BalanceAmount"].FormatString = "#,#;(#,#);0";
			grdContract.RootTable.Columns["TaxAmount"].FormatString = "#,#.##";
			grdContract.RootTable.Columns["OrderAmount"].TotalFormatString = "#,#;(#,#);0";
			grdContract.RootTable.Columns["ReceivedAmount"].TotalFormatString = "#,#;(#,#);0";
			grdContract.RootTable.Columns["BalanceAmount"].TotalFormatString = "#,#;(#,#);0";
			grdContract.RootTable.Columns["TaxAmount"].TotalFormatString = "#,#.##";
			grdContract.RootTable.Columns.Add("Select");
			grdContract.RootTable.Columns["Select"].Key = "Select";
			grdContract.RootTable.Columns["Select"].ActAsSelector = true;
			grdContract.RootTable.Columns["Select"].UseHeaderSelector = true;
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GrandTotal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellEdited(object sender, ColumnActionEventArgs e)
	{
		try
		{
			_ = grdContract.CurrentRow;
			GrandTotal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			int Id = ReceiptByInvoiceHeader.GetReceiptbyInvoiceIdbyDocNo(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocNo = Conversion.ToInt((object)txtdocno.Text.Trim()),
				DocumentTypeId = 29
			});
			if (Id > 0)
			{
				RecId = Id;
				ReadById(RecId);
			}
			else
			{
				Reset();
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdContract.UpdateData();
			GrandTotal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GrandTotal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			GrandTotal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			grdContract.UpdateData();
			GrandTotal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnHeaderClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Select")
			{
				grdContract.UpdateData();
				GrandTotal();
				TaxPropotionate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContract_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Select")
			{
				grdContract.UpdateData();
				GrandTotal();
				TaxPropotionate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
			ResetDetail();
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
			datcheqdate.Value = DateTime.Now;
			datPaymentDate.Value = DateTime.Now;
			AT = new Attachment();
			RecId = 0;
			txtpaytitle.Text = "";
			txtdocno.Text = "";
			datPaymentDate.Text = "";
			txtCheqNo.Text = "";
			txtAmount.Text = "";
			((Control)(object)CmbCreditAc).Text = "";
			((Control)(object)CmbSupplierName).Text = "";
			txtbalance.Text = string.Empty;
			txtCustomerBalance.Text = string.Empty;
			txtcustomerAdvBalance.Text = string.Empty;
			((Control)(object)CmbTaxCrAc).Text = string.Empty;
			((Control)(object)CmbTaxDrAc).Text = string.Empty;
			txtTaxPercent.Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			txtdifference.Text = string.Empty;
			txtgrandtotal.Text = string.Empty;
			((Control)(object)CmbTranagainst).Text = string.Empty;
			txtRemarks.Text = string.Empty;
			((Control)(object)CmbTaxType).Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			lblCustomer.Visible = false;
			lblAdvance.Visible = false;
			txtCustomerBalance.Visible = false;
			txtcustomerAdvBalance.Visible = false;
			VoucherNofill();
			dt.Rows.Clear();
			grdContract.DataSource = dt;
			dtdetail.Rows.Clear();
			grdDetail.DataSource = dtdetail;
			datPaymentDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			((Control)(object)CmbCreditAc).Text = "";
			lblAmount.Visible = false;
			txtbalance.Visible = false;
			txtbalance.Text = string.Empty;
			txtAmount.Text = "";
			txtCheqNo.Text = "";
			txtpaytitle.Text = "";
			CmbTranagainst.Focus();
			btnAdd.Text = "Add";
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
				BillDateFrom.Value = DateTime.Now.AddDays(-30.0);
				DueDateFrom.Value = DateTime.Now.AddDays(-30.0);
				btnsearchhistory_Click(null, null);
				HistoryGridFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsearchhistory_Click(object sender, EventArgs e)
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

	private void Gridhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt(Gridhistory.CurrentRow.Cells["Id"].Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Gridhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = Gridhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 29);
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
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		try
		{
			DataTable lst = VoucherHead.ReceiptByContract_FromHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value),
				FromDate = BillDateFrom.Value,
				ToDate = BillDateTo.Value,
				FromDocNo = Conversion.ToInt((object)txtInvoiceNoFrom.Text.Trim()),
				ToDocNo = Conversion.ToInt((object)txtInvoiceNoTo.Text.Trim())
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("DocumentTypeId");
				dt.Columns.Add("DocDate");
				dt.Columns.Add("DocNo");
				dt.Columns.Add("PartyName");
				dt.Columns.Add("TaxDebitAc");
				dt.Columns.Add("TaxCreditAc");
				dt.Columns.Add("TaxType");
				dt.Columns.Add("Tax%", typeof(double));
				dt.Columns.Add("TaxAmount", typeof(double));
				dt.Columns.Add("ReceivedAmount", typeof(double));
				dt.Columns.Add("NoOfAttachemtns");
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(lst.Rows[i]["DocDate"]).ToShortDateString(), lst.Rows[i]["DocNo"], lst.Rows[i]["CustomerName"], lst.Rows[i]["TaxDebitAccount"], lst.Rows[i]["TaxCreditAccount"], lst.Rows[i]["TaxName"], lst.Rows[i]["TaxPrct"], lst.Rows[i]["TaxAmount"], lst.Rows[i]["ReceivedAmount"], lst.Rows[i]["NoOfAttachemtns"]);
				}
				Gridhistory.DataSource = dt;
				Gridhistory.RetrieveStructure();
				GridSettinghistory();
			}
			else
			{
				Gridhistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettinghistory()
	{
		try
		{
			Gridhistory.RootTable.Columns["Id"].Visible = false;
			Gridhistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			Gridhistory.RootTable.Columns["NoOfAttachemtns"].ColumnType = (ColumnType)5;
			Gridhistory.RootTable.Columns["DocDate"].Width = 80;
			Gridhistory.RootTable.Columns["DocNo"].Width = 60;
			Gridhistory.RootTable.Columns["PartyName"].Width = 200;
			Gridhistory.RootTable.Columns["TaxCreditAc"].Width = 200;
			Gridhistory.RootTable.Columns["TaxDebitAc"].Width = 200;
			Gridhistory.RootTable.Columns["TaxType"].Width = 90;
			Gridhistory.RootTable.Columns["TaxAmount"].Width = 90;
			Gridhistory.RootTable.Columns["Tax%"].Width = 60;
			Gridhistory.RootTable.Columns["ReceivedAmount"].Width = 120;
			Gridhistory.RootTable.Columns["NoOfAttachemtns"].Width = 120;
			Gridhistory.RootTable.Columns["Tax%"].AggregateFunction = (AggregateFunction)2;
			Gridhistory.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			Gridhistory.RootTable.Columns["ReceivedAmount"].AggregateFunction = (AggregateFunction)2;
			Gridhistory.RootTable.Columns["Tax%"].TextAlignment = (TextAlignment)3;
			Gridhistory.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			Gridhistory.RootTable.Columns["ReceivedAmount"].TextAlignment = (TextAlignment)3;
			Gridhistory.RootTable.Columns["Tax%"].FormatString = "##,#.##";
			Gridhistory.RootTable.Columns["TaxAmount"].FormatString = "##,#.##";
			Gridhistory.RootTable.Columns["ReceivedAmount"].FormatString = "##,#.##";
			Gridhistory.RootTable.Columns["Tax%"].TotalFormatString = "##,#.##";
			Gridhistory.RootTable.Columns["TaxAmount"].TotalFormatString = "##,#.##";
			Gridhistory.RootTable.Columns["ReceivedAmount"].TotalFormatString = "##,#.##";
			Gridhistory.RootTable.Columns.Add("Edit");
			Gridhistory.RootTable.Columns["Edit"].Key = "Edit";
			Gridhistory.RootTable.Columns["Edit"].Caption = "Edit";
			Gridhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			Gridhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			Gridhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			Gridhistory.RootTable.Columns["Edit"].Width = 50;
			Gridhistory.RootTable.Columns["Edit"].Position = 0;
			Gridhistory.RootTable.Columns.Add("Print");
			Gridhistory.RootTable.Columns["Print"].Key = "Print";
			Gridhistory.RootTable.Columns["Print"].Caption = "Print";
			Gridhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			Gridhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			Gridhistory.RootTable.Columns["Print"].ButtonText = "Print";
			Gridhistory.RootTable.Columns["Print"].Width = 50;
			Gridhistory.RootTable.Columns["Print"].Position = 1;
			Gridhistory.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void Gridhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				ReadById(Conversion.ToInt(Gridhistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Expected O, but got Unknown
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Expected O, but got Unknown
		try
		{
			if (!Formvalidation())
			{
				return;
			}
			GrandTotal();
			double Difference = Conversion.ToDouble((object)txtdifference.Text);
			if (Difference > 0.0 || Difference < 0.0)
			{
				MessageBox.Show("Please Clear The Difference Amount");
				return;
			}
			double Amount = 0.0;
			double totalReceivedAmount = 0.0;
			if (grdDetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow item2 in rows)
				{
					if (Conversion.ToInt(item2.Cells["TransTypeId"].Value) != 2)
					{
						Amount += Conversion.ToDouble(item2.Cells["Amount"].Value);
					}
				}
				if (Amount != Conversion.ToDouble((object)txtgrandtotal.Text.Trim()))
				{
					MessageBox.Show("Please Check... Received & Total Amount Is Not Equal");
					return;
				}
			}
			if (grdContract.GetCheckedRows().Count() > 0)
			{
				GridEXRow[] rows = grdContract.GetCheckedRows();
				foreach (GridEXRow item in rows)
				{
					totalReceivedAmount += Conversion.ToDouble(item.Cells["ReceivedAmount"].Value);
				}
			}
			if (totalReceivedAmount != Amount)
			{
				MessageBox.Show("Please Check... Received & Total Amount Is Not Equal");
				return;
			}
			ReceiptByInvoiceHeader receipts = new ReceiptByInvoiceHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				receipts.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			receipts.DocumentTypeId = 29;
			receipts.DocDate = Conversion.ToDateTime((object)datPaymentDate.Value);
			receipts.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			receipts.SupplierCustomerId = Conversion.ToInt(CmbSupplierName.Value);
			receipts.OrganizationId = UserAccount.OrganizationId;
			receipts.CompanyId = UserAccount.CompanyId;
			receipts.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			receipts.EntryUser = UserAccount.ID;
			receipts.ModifyUser = UserAccount.ID;
			receipts.EntryDate = DateTime.Now;
			receipts.ModifyDate = DateTime.Now;
			receipts.TaxTypeId = Conversion.ToInt(CmbTaxType.Value);
			receipts.TaxPrct = Conversion.ToDouble((object)txtTaxPercent.Text);
			receipts.TaxDebitAcId = Conversion.ToInt(CmbTaxDrAc.Value);
			receipts.TaxCreditAcId = Conversion.ToInt(CmbTaxCrAc.Value);
			receipts.TaxAmount = Conversion.ToDouble((object)txtTaxAmount.Text);
			receipts.ReceiptByInvoiceDetailList = new List<ReceiptByInvoiceDetail>();
			receipts.ReceiptByInvoiceOtherDetailList = new List<ReceiptByInvoiceOtherDetail>();
			if (grdDetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					ReceiptByInvoiceOtherDetail vd2 = new ReceiptByInvoiceOtherDetail();
					vd2.TransType = Conversion.ToInt(r2.Cells["TransTypeId"].Value);
					vd2.TransAgainst = Conversion.ToString((object)r2.Cells["TransAgainstId"].Text);
					vd2.GLAccountId = Conversion.ToInt(r2.Cells["GLAccountId"].Value);
					vd2.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					vd2.Amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
					vd2.CheqNo = Conversion.ToString(r2.Cells["CheqNo"].Value);
					vd2.CheqDate = Conversion.ToDateTime(r2.Cells["CheqDate"].Value);
					vd2.PayTitle = Conversion.ToString(r2.Cells["Paytitle"].Value);
					receipts.ReceiptByInvoiceOtherDetailList.Add(vd2);
				}
				if (grdContract.GetRows().Count() > 0)
				{
					if (grdContract.GetCheckedRows().Count() > 0)
					{
						rows = grdContract.GetCheckedRows();
						foreach (GridEXRow r in rows)
						{
							ReceiptByInvoiceDetail vd = new ReceiptByInvoiceDetail();
							vd.RefDocNoId = Conversion.ToInt(r.Cells["Id"].Value);
							vd.RefDocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
							vd.PaymentTermsId = Conversion.ToInt(r.Cells["PaymentTermsId"].Value);
							vd.InvoiceDate = Conversion.ToDateTime(r.Cells["OrderDate"].Value);
							vd.InvoiceNo = Conversion.ToInt(r.Cells["OrderNo"].Value);
							vd.DueDate = Conversion.ToDateTime(r.Cells["DueDate"].Value);
							vd.BillAmount = Conversion.ToSingle(r.Cells["OrderAmount"].Value);
							vd.Amount = Conversion.ToSingle(r.Cells["ReceivedAmount"].Value);
							vd.BalanceAmount = Conversion.ToSingle(r.Cells["BalanceAmount"].Value);
							vd.DWhtAmount = Conversion.ToSingle(r.Cells["TaxAmount"].Value);
							receipts.ReceiptByInvoiceDetailList.Add(vd);
						}
						receipts.AttachmentsList = new List<DMSAttachments>();
						receipts.DeleteAttachmentsList = new List<DMSAttachments>();
						FormHelper.UpdateAttachmentsForObject<ReceiptByInvoiceHeader>(receipts, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
						ReceiptByInvoiceHeader.Save(receipts);
						if (RecId == 0)
						{
							MessageBox.Show("Record Save Successfully");
						}
						else
						{
							MessageBox.Show("Record Update Successfully");
							if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
							{
								DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 29, 0);
							}
						}
						Reset();
					}
					else
					{
						MessageBox.Show("Please Check any row in OutStanding Contract Grid First...");
					}
				}
				else
				{
					MessageBox.Show("Outstanding Order Grid Can not be Empty");
				}
			}
			else
			{
				MessageBox.Show("Outstanding Order Grid Can not be Empty");
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

	public void ReadById(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReceiptByInvoiceHeader receipts = new ReceiptByInvoiceHeader();
			RecId = Conversion.ToInt((object)ID);
			receipts = ReceiptByInvoiceHeader.GetByID(RecId);
			if (receipts == null)
			{
				return;
			}
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			datPaymentDate.Value = receipts.DocDate;
			CmbSupplierName.Value = receipts.SupplierCustomerId;
			txtdocno.Text = Conversion.ToString((object)receipts.DocNo);
			txtTaxPercent.Text = Conversion.ToString((object)receipts.TaxPrct);
			txtTaxAmount.Text = Conversion.ToString((object)receipts.TaxAmount);
			CmbTaxType.Value = Conversion.ToInt((object)receipts.TaxTypeId);
			((Control)(object)CmbTaxDrAc).Text = Conversion.ToString((object)receipts.TaxDebitAcId);
			((Control)(object)CmbTaxCrAc).Text = Conversion.ToString((object)receipts.TaxCreditAcId);
			if (Conversion.ToDouble((object)receipts.TaxPrct) > 0.0)
			{
				ChkBoxWthHolding.Checked = true;
			}
			dt.Rows.Clear();
			dtdetail.Rows.Clear();
			VoucherhHeadIdGet();
			for (int j = 0; j < receipts.ReceiptByInvoiceDetailList.Count; j++)
			{
				dt.Rows.Add(receipts.ReceiptByInvoiceDetailList[j].RefDocNoId, receipts.ReceiptByInvoiceDetailList[j].RefDocumentTypeId, receipts.ReceiptByInvoiceDetailList[j].PaymentTermsId, Convert.ToDateTime(receipts.ReceiptByInvoiceDetailList[j].InvoiceDate).ToString("dd-MMM-yyyyy"), receipts.ReceiptByInvoiceDetailList[j].InvoiceNo, receipts.ReceiptByInvoiceDetailList[j].DueDate, receipts.ReceiptByInvoiceDetailList[j].PaymentTerm, receipts.ReceiptByInvoiceDetailList[j].BillAmount, receipts.ReceiptByInvoiceDetailList[j].TotalReceivedAmount, receipts.ReceiptByInvoiceDetailList[j].BalanceAmount, 0);
			}
			grdContract.DataSource = dt;
			grdContract.RetrieveStructure();
			GridContractSettings();
			dtdetail.Rows.Clear();
			for (int i = 0; i < receipts.ReceiptByInvoiceOtherDetailList.Count; i++)
			{
				dtdetail.Rows.Add(receipts.ReceiptByInvoiceOtherDetailList[i].TransType, receipts.ReceiptByInvoiceOtherDetailList[i].TransAgainst, receipts.ReceiptByInvoiceOtherDetailList[i].GLAccountId, receipts.ReceiptByInvoiceOtherDetailList[i].AccountTitle, receipts.ReceiptByInvoiceOtherDetailList[i].Amount, receipts.ReceiptByInvoiceOtherDetailList[i].CheqNo, Convert.ToDateTime(receipts.ReceiptByInvoiceOtherDetailList[i].CheqDate).ToString("dd-MMM-yyyyy"), receipts.ReceiptByInvoiceOtherDetailList[i].PayTitle, receipts.ReceiptByInvoiceOtherDetailList[i].Remarks);
			}
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			GridDetailSettings();
			GrandTotal();
			List<DMSAttachments> byID = DMSAttachments.GetByID(int.Parse(RecId.ToString()), base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.ActionId = 2;
				TA.Path = Attachment.Attachment;
				TA.CustomFileName = Attachment.UploadedFileCustomName;
				AT.lst.Add(TA);
			}
			tabControl1.SelectedIndex = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReceiptByInvoiceAccount_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnsearchhistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
			{
				Insert();
			}
			if (e.Control && e.KeyCode == Keys.P && tabControl1.SelectedIndex == 0 && formright.DoHavePrintRights)
			{
				print_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10 && tabControl1.SelectedIndex == 0)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Alt && e.Control && tabControl1.SelectedIndex == 0)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.Down && tabControl1.SelectedIndex == 0)
			{
				((Control)(object)grdDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up && tabControl1.SelectedIndex == 0)
			{
				CmbTranagainst.Focus();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					datPaymentDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					BillDateFrom.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.VoucherReport_118(VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrandTotal()
	{
		try
		{
			double detailAmount = 0.0;
			double TotalReceived = 0.0;
			double GrandTotalamount = 0.0;
			if (grdContract.GetRows().Count() > 0 && grdContract.RootTable.Columns["ReceivedAmount"] != null)
			{
				GridEXRow[] checkedRows = grdContract.GetCheckedRows();
				foreach (GridEXRow item2 in checkedRows)
				{
					TotalReceived += Conversion.ToDouble(item2.Cells["ReceivedAmount"].Value);
				}
			}
			if (btnsave.Visible)
			{
				if (grdDetail.RootTable.Columns["Amount"] != null)
				{
					GridEXRow[] checkedRows = grdDetail.GetRows();
					foreach (GridEXRow item in checkedRows)
					{
						if (Conversion.ToInt(item.Cells["TransTypeId"].Value) != 2)
						{
							detailAmount += Conversion.ToDouble(item.Cells["Amount"].Value);
						}
					}
				}
			}
			else
			{
				GridEXRow[] checkedRows = grdDetail.GetRows();
				foreach (GridEXRow r in checkedRows)
				{
					if (Conversion.ToInt(r.Cells["TransTypeId"].Value) != 2)
					{
						detailAmount += Conversion.ToDouble(r.Cells["Amount"].Value);
					}
				}
			}
			GrandTotalamount = TotalReceived;
			txtgrandtotal.Text = GrandTotalamount.ToString("#,##0.######");
			double diff = GrandTotalamount - detailAmount;
			txtdifference.Text = diff.ToString("#,##0.#####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalReceivingAmtForContractGrid()
	{
		try
		{
			double TotalDetailGridAmount = 0.0;
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item2 in rows)
			{
				if (Conversion.ToInt(item2.Cells["TransTypeId"].Value) != 2)
				{
					TotalDetailGridAmount += Conversion.ToDouble(item2.Cells["Amount"].Value);
				}
			}
			if (TotalDetailGridAmount > 0.0)
			{
				rows = grdContract.GetRows();
				foreach (GridEXRow item in rows)
				{
					item.BeginEdit();
					if (TotalDetailGridAmount > 0.0)
					{
						double balanceAmount = Convert.ToDouble(item.Cells["BalanceAmount"].Value);
						if (TotalDetailGridAmount > balanceAmount)
						{
							item.Cells["ReceivedAmount"].Value = balanceAmount;
							TotalDetailGridAmount -= balanceAmount;
						}
						else
						{
							item.Cells["ReceivedAmount"].Value = TotalDetailGridAmount;
							TotalDetailGridAmount = 0.0;
						}
					}
					else
					{
						item.Cells["ReceivedAmount"].Value = TotalDetailGridAmount;
					}
					item.EndEdit();
				}
			}
			else
			{
				rows = grdContract.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["ReceivedAmount"].Value = 0;
					obj.EndEdit();
				}
			}
			grdContract.UpdateData();
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
			if (FormValidationDetail() && ValidateAmountWithContractBalnceAmt())
			{
				if (btnAdd.Text == "Update")
				{
					dtdetail.Rows[updateDetailIndex]["TransTypeId"] = cmbTransType.Value;
					dtdetail.Rows[updateDetailIndex]["TransAgainstId"] = CmbTranagainst.Value;
					dtdetail.Rows[updateDetailIndex]["GLAccountId"] = CmbCreditAc.Value;
					dtdetail.Rows[updateDetailIndex]["AccountTitle"] = ((Control)(object)CmbCreditAc).Text;
					dtdetail.Rows[updateDetailIndex]["Amount"] = txtAmount.Text;
					dtdetail.Rows[updateDetailIndex]["CheqNo"] = txtCheqNo.Text;
					dtdetail.Rows[updateDetailIndex]["CheqDate"] = Conversion.ToDateTime((object)datcheqdate.Value).ToString("dd-MMM-yyyy");
					dtdetail.Rows[updateDetailIndex]["Paytitle"] = txtpaytitle.Text;
					dtdetail.Rows[updateDetailIndex]["Remarks"] = txtRemarks.Text;
					grdDetail.DataSource = dtdetail;
					btnAdd.Text = "+";
					grdDetail.RetrieveStructure();
					GridDetailSettings();
					ResetDetail();
				}
				else
				{
					dtdetail.Rows.Add(cmbTransType.Value, CmbTranagainst.Value, CmbCreditAc.Value, ((Control)(object)CmbCreditAc).Text, txtAmount.Text, txtCheqNo.Text, Conversion.ToDateTime((object)datcheqdate.Value).ToString("dd-MMM-yyyy"), txtpaytitle.Text.Trim(), txtRemarks.Text.Trim());
					grdDetail.DataSource = dtdetail;
					grdDetail.RetrieveStructure();
					GridDetailSettings();
					ResetDetail();
				}
				GrandTotal();
				TotalReceivingAmtForContractGrid();
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
				((Control)(object)cmbTransType).Text = item.Cells["TransTypeId"].Value.ToString();
				((Control)(object)CmbTranagainst).Text = item.Cells["TransAgainstId"].Value.ToString();
				CmbTranType_Leave(null, null);
				CmbCreditAc.Value = item.Cells["GLAccountId"].Value.ToString();
				((Control)(object)CmbCreditAc).Text = item.Cells["AccountTitle"].Text.ToString();
				txtAmount.Text = item.Cells["Amount"].Value.ToString();
				txtCheqNo.Text = item.Cells["CheqNo"].Text.ToString();
				datcheqdate.Value = Convert.ToDateTime(item.Cells["CheqDate"].Value);
				txtpaytitle.Text = item.Cells["Paytitle"].Value.ToString();
				txtRemarks.Text = item.Cells["Remarks"].Text.ToString();
				updateDetailIndexAmount = Conversion.ToDouble((object)item.Cells["Amount"].Value.ToString());
				btnAdd.Text = "Update";
				CmbTranagainst.Focus();
			}
			AccountCurrentBalance();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdDetail.UpdateData();
			GrandTotal();
			TotalReceivingAmtForContractGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellEdited(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GrandTotal();
			TotalReceivingAmtForContractGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GrandTotal();
			TotalReceivingAmtForContractGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSettings()
	{
		try
		{
			grdDetail.RootTable.Columns["GLAccountId"].Visible = false;
			grdDetail.RootTable.Columns["TransTypeId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["TransTypeId"].HasValueList = true;
			grdDetail.RootTable.Columns["TransTypeId"].LimitToList = true;
			grdDetail.RootTable.Columns["TransTypeId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["TransTypeId"].Caption = "Trans Type";
			grdDetail.RootTable.Columns["TransTypeId"].ValueList.PopulateValueList((IEnumerable)dtTransType.DefaultView, "Id", "Name");
			grdDetail.RootTable.Columns["TransTypeId"].Width = 100;
			grdDetail.RootTable.Columns["TransAgainstId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["TransAgainstId"].HasValueList = true;
			grdDetail.RootTable.Columns["TransAgainstId"].LimitToList = true;
			grdDetail.RootTable.Columns["TransAgainstId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["TransAgainstId"].Caption = "Trans Against";
			grdDetail.RootTable.Columns["TransAgainstId"].ValueList.PopulateValueList((IEnumerable)dttransAgainst.DefaultView, "Id", "Name");
			grdDetail.RootTable.Columns["TransAgainstId"].Width = 100;
			grdDetail.RootTable.Columns["AccountTitle"].Width = 120;
			grdDetail.RootTable.Columns["Amount"].Width = 80;
			grdDetail.RootTable.Columns["CheqNo"].Width = 80;
			grdDetail.RootTable.Columns["CheqDate"].Width = 90;
			grdDetail.RootTable.Columns["Paytitle"].Width = 150;
			grdDetail.RootTable.Columns["Remarks"].Width = 150;
			grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Amount"].TotalFormatString = "#,#;(#,#);0";
			grdDetail.RootTable.Columns["Amount"].FormatString = "#,#;(#,#);0";
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 40;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete" && btnsave.Visible && btnsave.Enabled)
			{
				GridEXRow item = grdDetail.CurrentRow;
				dtdetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			GrandTotal();
			TotalReceivingAmtForContractGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdContract).Name);
		if (fs != null)
		{
			grdContract.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
		if (fs != null)
		{
			grdDetail.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)Gridhistory).Name);
			if (fs != null)
			{
				Gridhistory.LoadLayoutFile((Stream)fs);
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
			e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierCustomer();
			BindTaxTypes();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerate_Click(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
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

	private void ChkBoxWthHolding_CheckedChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		if (ChkBoxWthHolding.Checked)
		{
			TaxScheduleMain taxSchedule = new TaxScheduleMain();
			taxSchedule.OrganizationId = UserAccount.OrganizationId;
			taxSchedule.CompanyId = UserAccount.CompanyId;
			taxSchedule.EffectedDate = datPaymentDate.Value;
			taxSchedule.TaxNameId = Conversion.ToInt(CmbTaxType.Value);
			if (TaxScheduleMain.ReadTaxSchedule(taxSchedule).Rows.Count == 0)
			{
				((Control)(object)CmbTaxCrAc).Enabled = false;
				((Control)(object)CmbTaxDrAc).Enabled = false;
				((Control)(object)CmbTaxDrAc).Text = string.Empty;
				txtTaxPercent.Text = "0";
				txtTaxAmount.Text = "0";
			}
			else
			{
				DataTable detail = TaxScheduleMain.ReadTaxSchedule(taxSchedule);
				txtTaxPercent.Text = detail.Rows[0]["TaxPercent"].ToString();
				if (!UpdateMode)
				{
					CmbTaxDrAc.Value = detail.Rows[0]["TaxGLAccountId"];
					((Control)(object)CmbTaxDrAc).Text = detail.Rows[0]["AccountTitle"].ToString();
				}
				double taxamount = Math.Round(Conversion.ToDouble((object)txtgrandtotal.Text) / (100.0 - Conversion.ToDouble((object)txtTaxPercent.Text)) * 100.0 * Conversion.ToDouble((object)txtTaxPercent.Text) / 100.0, 2);
				txtTaxAmount.Text = taxamount.ToString();
				double ValueAmt = Conversion.ToDouble((object)txtgrandtotal.Text);
				txtTotalAmount.Text = (taxamount + ValueAmt).ToString("0,0.00");
			}
			((Control)(object)CmbTaxCrAc).Enabled = true;
			((Control)(object)CmbTaxDrAc).Enabled = true;
			TaxPropotionate();
		}
		else
		{
			((Control)(object)CmbTaxCrAc).Enabled = false;
			((Control)(object)CmbTaxDrAc).Enabled = false;
			((Control)(object)CmbTaxDrAc).Text = string.Empty;
			txtTaxPercent.Text = "0";
			txtTaxAmount.Text = "0";
			double Amt = Conversion.ToDouble((object)txtgrandtotal.Text);
			double Tax = Conversion.ToDouble((object)txtTaxAmount.Text);
			txtTotalAmount.Text = (Amt + Tax).ToString("0,0");
			TaxPropotionate();
		}
	}

	private void CmbTaxType_Leave(object sender, EventArgs e)
	{
		try
		{
			ChkBoxWthHolding_CheckedChanged(null, null);
			TaxPropotionate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaxPropotionate()
	{
		try
		{
			GridEXRow[] checkedRows = grdContract.GetCheckedRows();
			foreach (GridEXRow obj in checkedRows)
			{
				obj.BeginEdit();
				double taxamount = Math.Round(Conversion.ToDouble(obj.Cells["ReceivedAmount"].Value) / (100.0 - Conversion.ToDouble((object)txtTaxPercent.Text)) * 100.0 * Conversion.ToDouble((object)txtTaxPercent.Text) / 100.0, 2);
				obj.Cells["TaxAmount"].Value = taxamount.ToString();
				obj.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReceiptByContract_FormClosing(object sender, FormClosingEventArgs e)
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

	private void cmbAdvanceAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			AdvanceAccountCurrentBalance();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Tran Type in Detail Box");
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

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdDetail.CurrentColumn.Key == "Delete" && btnsave.Visible && btnsave.Enabled)
				{
					dtdetail.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					grdDetail.UpdateData();
					GrandTotal();
					TotalReceivingAmtForContractGrid();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete && btnsave.Visible && btnsave.Enabled)
			{
				dtdetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				grdDetail.UpdateData();
				GrandTotal();
				TotalReceivingAmtForContractGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Gridhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = Gridhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Gridhistory_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (Gridhistory.CurrentColumn.Key == "Edit")
				{
					ReadById(Conversion.ToInt(Gridhistory.CurrentRow.Cells["Id"].Value));
				}
				if (Gridhistory.CurrentColumn.Key == "NoOfAttachemtns")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(Gridhistory.CurrentRow.Cells["Id"].Value), 29);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRemarks_Enter(object sender, EventArgs e)
	{
		try
		{
			txtRemarks.SelectAll();
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
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Expected O, but got Unknown
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Expected O, but got Unknown
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Expected O, but got Unknown
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Expected O, but got Unknown
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Expected O, but got Unknown
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Expected O, but got Unknown
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Expected O, but got Unknown
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Expected O, but got Unknown
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Expected O, but got Unknown
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Expected O, but got Unknown
		//IL_0ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf7: Expected O, but got Unknown
		//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0e: Expected O, but got Unknown
		//IL_0d1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d25: Expected O, but got Unknown
		//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3c: Expected O, but got Unknown
		//IL_0d49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d53: Expected O, but got Unknown
		//IL_109e: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a8: Expected O, but got Unknown
		//IL_58ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_58c4: Expected O, but got Unknown
		//IL_58d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_58db: Expected O, but got Unknown
		GridEXLayout grdContract_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.ReceiptByContract));
		GridEXLayout Gridhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance30 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdContract = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbTaxCrAc = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.cmbTransType = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.txtCheqNo = new System.Windows.Forms.TextBox();
		this.lblChDate = new System.Windows.Forms.Label();
		this.CmbTranagainst = new UltraCombo();
		this.lblAmount = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtbalance = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.datcheqdate = new System.Windows.Forms.DateTimePicker();
		this.btnAdd = new System.Windows.Forms.Button();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.lblChqNo = new System.Windows.Forms.Label();
		this.txtpaytitle = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbCreditAc = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.lblAdvance = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.txtcustomerAdvBalance = new System.Windows.Forms.TextBox();
		this.cmbAdvanceAccount = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.lblCustomer = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtCustomerBalance = new System.Windows.Forms.TextBox();
		this.datPaymentDate = new System.Windows.Forms.DateTimePicker();
		this.CmbSupplierName = new UltraCombo();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtgrandtotal = new System.Windows.Forms.TextBox();
		this.CmbTaxDrAc = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.ChkBoxWthHolding = new System.Windows.Forms.CheckBox();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbTaxType = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.txtdifference = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.Gridhistory = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label29 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.CmbStatus = new System.Windows.Forms.ComboBox();
		this.label17 = new System.Windows.Forms.Label();
		this.btnsearchhistory = new System.Windows.Forms.Button();
		this.CmbCustomerHistory = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtInvoiceNoFrom = new System.Windows.Forms.TextBox();
		this.txtdatetohistory = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtInvoiceNoTo = new System.Windows.Forms.TextBox();
		this.BillDateFrom = new System.Windows.Forms.DateTimePicker();
		this.DueDateTo = new System.Windows.Forms.DateTimePicker();
		this.BillDateTo = new System.Windows.Forms.DateTimePicker();
		this.DueDateFrom = new System.Windows.Forms.DateTimePicker();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContract).BeginInit();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel2.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxCrAc).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTransType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTranagainst).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAc).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAdvanceAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxDrAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).BeginInit();
		this.toolStrip3.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Gridhistory).BeginInit();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel12.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1146, 741);
		this.panel1.TabIndex = 2;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1146, 741);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.ctrlGrdBar3);
		this.tabPage1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1138, 713);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel3.AutoScroll = true;
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Controls.Add(this.panel10);
		this.panel3.Controls.Add(this.panel9);
		this.panel3.Controls.Add(this.toolStrip3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1132, 707);
		this.panel3.TabIndex = 75;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdContract);
		this.panel4.Controls.Add(this.panel11);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 550);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1132, 157);
		this.panel4.TabIndex = 57;
		grdContract_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdContract.DesignTimeLayout = grdContract_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContract).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdContract.DynamicFiltering = true;
		this.grdContract.FilterMode = (FilterMode)2;
		this.grdContract.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdContract).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContract.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdContract).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdContract).Name = "grdContract";
		((ScrollableBase)this.grdContract).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContract).Size = new System.Drawing.Size(1132, 132);
		((System.Windows.Forms.Control)(object)this.grdContract).TabIndex = 0;
		this.grdContract.TotalRow = (InheritableBoolean)1;
		this.grdContract.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContract).VisualStyle = (VisualStyle)4;
		this.grdContract.CellValueChanged += new ColumnActionEventHandler(grd_CellValueChanged);
		this.grdContract.CellEdited += new ColumnActionEventHandler(grd_CellEdited);
		this.grdContract.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grdContract.ColumnButtonClick += new ColumnActionEventHandler(grdContract_ColumnButtonClick);
		this.grdContract.ColumnHeaderClick += new ColumnActionEventHandler(grd_ColumnHeaderClick);
		this.grdContract.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdContract).TextChanged += new System.EventHandler(grd_TextChanged);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label22);
		this.panel11.Controls.Add(this.ctrlGrdBar4);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1132, 25);
		this.panel11.TabIndex = 1;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Bold);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(3, 2);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(455, 20);
		this.label22.TabIndex = 0;
		this.label22.Text = "Outstanding Contracts Of Selected Party For receiving  Payment";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1092, 2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar4.MyGrid = this.grdContract;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 2;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel10.Controls.Add(this.panel2);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 257);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1132, 293);
		this.panel10.TabIndex = 56;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1132, 268);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).TextChanged += new System.EventHandler(grdDetail_TextChanged);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label24);
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1132, 25);
		this.panel2.TabIndex = 0;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Bold);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(2, 3);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(84, 20);
		this.label24.TabIndex = 0;
		this.label24.Text = "Detail Grid";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1093, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 1;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel9.Controls.Add(this.panel5);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxCrAc);
		this.panel9.Controls.Add(this.label25);
		this.panel9.Controls.Add(this.label14);
		this.panel9.Controls.Add(this.groupBox2);
		this.panel9.Controls.Add(this.txtTaxPercent);
		this.panel9.Controls.Add(this.groupBox1);
		this.panel9.Controls.Add(this.label15);
		this.panel9.Controls.Add(this.txtgrandtotal);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxDrAc);
		this.panel9.Controls.Add(this.label5);
		this.panel9.Controls.Add(this.ChkBoxWthHolding);
		this.panel9.Controls.Add(this.label10);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxType);
		this.panel9.Controls.Add(this.label13);
		this.panel9.Controls.Add(this.txtTotalAmount);
		this.panel9.Controls.Add(this.txtdifference);
		this.panel9.Controls.Add(this.label16);
		this.panel9.Controls.Add(this.label20);
		this.panel9.Controls.Add(this.txtTaxAmount);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 27);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1132, 230);
		this.panel9.TabIndex = 55;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label9);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1132, 28);
		this.panel5.TabIndex = 85;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(4, 4);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(171, 19);
		this.label9.TabIndex = 0;
		this.label9.Text = "Receipt By Contract";
		this.CmbTaxCrAc.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxCrAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxCrAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxCrAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance88).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxCrAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxCrAc).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbTaxCrAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxCrAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxCrAc).Location = new System.Drawing.Point(882, 90);
		((System.Windows.Forms.Control)(object)this.CmbTaxCrAc).Name = "CmbTaxCrAc";
		((System.Windows.Forms.Control)(object)this.CmbTaxCrAc).Size = new System.Drawing.Size(243, 24);
		((System.Windows.Forms.Control)(object)this.CmbTaxCrAc).TabIndex = 6;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(794, 95);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(56, 15);
		this.label25.TabIndex = 84;
		this.label25.Text = "Tax Cr Ac";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(794, 68);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(58, 15);
		this.label14.TabIndex = 82;
		this.label14.Text = "Tax Dr Ac";
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTransType);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.txtCheqNo);
		this.groupBox2.Controls.Add(this.lblChDate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTranagainst);
		this.groupBox2.Controls.Add(this.lblAmount);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.txtbalance);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.datcheqdate);
		this.groupBox2.Controls.Add(this.btnAdd);
		this.groupBox2.Controls.Add(this.txtRemarks);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.lblChqNo);
		this.groupBox2.Controls.Add(this.txtpaytitle);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAc);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add(this.txtAmount);
		this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 110);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(786, 117);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.cmbTransType.AllowNull = (DefaultableBoolean)1;
		this.cmbTransType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTransType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTransType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbTransType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTransType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTransType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTransType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.cmbTransType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTransType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbTransType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTransType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance94).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTransType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbTransType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTransType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTransType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTransType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbTransType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTransType).Location = new System.Drawing.Point(8, 41);
		((System.Windows.Forms.Control)(object)this.cmbTransType).Name = "cmbTransType";
		((System.Windows.Forms.Control)(object)this.cmbTransType).Size = new System.Drawing.Size(108, 26);
		((System.Windows.Forms.Control)(object)this.cmbTransType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbTransType).Leave += new System.EventHandler(cmbTransType_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(8, 20);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(58, 15);
		this.label8.TabIndex = 67;
		this.label8.Text = "Tran Type";
		this.txtCheqNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCheqNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCheqNo.Location = new System.Drawing.Point(571, 42);
		this.txtCheqNo.Name = "txtCheqNo";
		this.txtCheqNo.Size = new System.Drawing.Size(98, 23);
		this.txtCheqNo.TabIndex = 4;
		this.lblChDate.AutoSize = true;
		this.lblChDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblChDate.Location = new System.Drawing.Point(670, 20);
		this.lblChDate.Name = "lblChDate";
		this.lblChDate.Size = new System.Drawing.Size(75, 15);
		this.lblChDate.TabIndex = 57;
		this.lblChDate.Text = "Cheque Date";
		this.CmbTranagainst.AllowNull = (DefaultableBoolean)1;
		this.CmbTranagainst.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTranagainst.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTranagainst.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Appearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTranagainst).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.CmbTranagainst).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance77).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTranagainst).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTranagainst).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTranagainst.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTranagainst).Location = new System.Drawing.Point(117, 41);
		((System.Windows.Forms.Control)(object)this.CmbTranagainst).Name = "CmbTranagainst";
		((System.Windows.Forms.Control)(object)this.CmbTranagainst).Size = new System.Drawing.Size(103, 26);
		((System.Windows.Forms.Control)(object)this.CmbTranagainst).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbTranagainst).Leave += new System.EventHandler(CmbTranType_Leave);
		this.lblAmount.AutoSize = true;
		this.lblAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmount.Location = new System.Drawing.Point(308, 20);
		this.lblAmount.Name = "lblAmount";
		this.lblAmount.Size = new System.Drawing.Size(39, 15);
		this.lblAmount.TabIndex = 65;
		this.lblAmount.Text = "label9";
		this.lblAmount.Visible = false;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(117, 20);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(73, 15);
		this.label3.TabIndex = 56;
		this.label3.Text = "Tran Against";
		this.txtbalance.BackColor = System.Drawing.SystemColors.InactiveBorder;
		this.txtbalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbalance.Location = new System.Drawing.Point(350, 17);
		this.txtbalance.Name = "txtbalance";
		this.txtbalance.Size = new System.Drawing.Size(112, 23);
		this.txtbalance.TabIndex = 57;
		this.txtbalance.TabStop = false;
		this.txtbalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtbalance.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(408, 69);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(69, 15);
		this.label1.TabIndex = 53;
		this.label1.Text = "Remarks GL";
		this.datcheqdate.CustomFormat = "dd-MMM-yy";
		this.datcheqdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datcheqdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datcheqdate.Location = new System.Drawing.Point(670, 42);
		this.datcheqdate.Name = "datcheqdate";
		this.datcheqdate.Size = new System.Drawing.Size(110, 23);
		this.datcheqdate.TabIndex = 5;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(711, 85);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(69, 28);
		this.btnAdd.TabIndex = 8;
		this.btnAdd.Text = "Add";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(408, 88);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(303, 23);
		this.txtRemarks.TabIndex = 7;
		this.txtRemarks.Enter += new System.EventHandler(txtRemarks_Enter);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(10, 69);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(52, 15);
		this.label6.TabIndex = 16;
		this.label6.Text = "Pay Title";
		this.lblChqNo.AutoSize = true;
		this.lblChqNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblChqNo.Location = new System.Drawing.Point(571, 20);
		this.lblChqNo.Name = "lblChqNo";
		this.lblChqNo.Size = new System.Drawing.Size(66, 15);
		this.lblChqNo.TabIndex = 20;
		this.lblChqNo.Text = "Cheque No";
		this.txtpaytitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpaytitle.Location = new System.Drawing.Point(8, 88);
		this.txtpaytitle.Name = "txtpaytitle";
		this.txtpaytitle.Size = new System.Drawing.Size(399, 23);
		this.txtpaytitle.TabIndex = 6;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(221, 20);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(76, 15);
		this.label4.TabIndex = 13;
		this.label4.Text = "Account (Dr)";
		this.CmbCreditAc.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance100).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Location = new System.Drawing.Point(221, 41);
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Name = "CmbCreditAc";
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Size = new System.Drawing.Size(241, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Leave += new System.EventHandler(CmbCreditAc_Leave);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(463, 20);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(51, 15);
		this.label23.TabIndex = 51;
		this.label23.Text = "Amount";
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(463, 42);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(107, 23);
		this.txtAmount.TabIndex = 3;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.txtTaxPercent.BackColor = System.Drawing.Color.White;
		this.txtTaxPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercent.Location = new System.Drawing.Point(1063, 40);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.ReadOnly = true;
		this.txtTaxPercent.Size = new System.Drawing.Size(62, 21);
		this.txtTaxPercent.TabIndex = 80;
		this.txtTaxPercent.TabStop = false;
		this.txtTaxPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.groupBox1.Controls.Add(this.lblAdvance);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.txtcustomerAdvBalance);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAdvanceAccount);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.lblCustomer);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtCustomerBalance);
		this.groupBox1.Controls.Add(this.datPaymentDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierName);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 30);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(786, 75);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.lblAdvance.AutoSize = true;
		this.lblAdvance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAdvance.Location = new System.Drawing.Point(579, 23);
		this.lblAdvance.Name = "lblAdvance";
		this.lblAdvance.Size = new System.Drawing.Size(39, 15);
		this.lblAdvance.TabIndex = 68;
		this.lblAdvance.Text = "label9";
		this.lblAdvance.Visible = false;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(471, 23);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(104, 15);
		this.label26.TabIndex = 66;
		this.label26.Text = "Advance Account ";
		this.txtcustomerAdvBalance.BackColor = System.Drawing.SystemColors.InactiveBorder;
		this.txtcustomerAdvBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustomerAdvBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustomerAdvBalance.Location = new System.Drawing.Point(619, 19);
		this.txtcustomerAdvBalance.Name = "txtcustomerAdvBalance";
		this.txtcustomerAdvBalance.Size = new System.Drawing.Size(161, 23);
		this.txtcustomerAdvBalance.TabIndex = 67;
		this.txtcustomerAdvBalance.TabStop = false;
		this.txtcustomerAdvBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcustomerAdvBalance.Visible = false;
		this.cmbAdvanceAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbAdvanceAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAdvanceAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAdvanceAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAdvanceAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAdvanceAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAdvanceAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAdvanceAccount).Location = new System.Drawing.Point(472, 43);
		((System.Windows.Forms.Control)(object)this.cmbAdvanceAccount).Name = "cmbAdvanceAccount";
		((System.Windows.Forms.Control)(object)this.cmbAdvanceAccount).Size = new System.Drawing.Size(309, 26);
		((System.Windows.Forms.Control)(object)this.cmbAdvanceAccount).TabIndex = 3;
		this.cmbAdvanceAccount.ValueChanged += new System.EventHandler(cmbAdvanceAccount_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbAdvanceAccount).Leave += new System.EventHandler(cmbAdvanceAccount_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(77, 23);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(57, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Doc Date";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(9, 23);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(45, 15);
		this.label11.TabIndex = 23;
		this.label11.Text = "DocNo";
		this.lblCustomer.AutoSize = true;
		this.lblCustomer.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCustomer.Location = new System.Drawing.Point(263, 23);
		this.lblCustomer.Name = "lblCustomer";
		this.lblCustomer.Size = new System.Drawing.Size(39, 15);
		this.lblCustomer.TabIndex = 64;
		this.lblCustomer.Text = "label9";
		this.lblCustomer.Visible = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(184, 23);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(69, 15);
		this.label7.TabIndex = 59;
		this.label7.Text = "Party Name";
		this.txtCustomerBalance.BackColor = System.Drawing.SystemColors.InactiveBorder;
		this.txtCustomerBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCustomerBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerBalance.Location = new System.Drawing.Point(307, 19);
		this.txtCustomerBalance.Name = "txtCustomerBalance";
		this.txtCustomerBalance.Size = new System.Drawing.Size(163, 23);
		this.txtCustomerBalance.TabIndex = 60;
		this.txtCustomerBalance.TabStop = false;
		this.txtCustomerBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCustomerBalance.Visible = false;
		this.datPaymentDate.CustomFormat = "dd-MMM-yy";
		this.datPaymentDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datPaymentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datPaymentDate.Location = new System.Drawing.Point(77, 45);
		this.datPaymentDate.Name = "datPaymentDate";
		this.datPaymentDate.Size = new System.Drawing.Size(106, 23);
		this.datPaymentDate.TabIndex = 1;
		this.CmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Location = new System.Drawing.Point(184, 43);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Name = "CmbSupplierName";
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Size = new System.Drawing.Size(286, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).TabIndex = 2;
		this.CmbSupplierName.ValueChanged += new System.EventHandler(CmbSupplierName_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Leave += new System.EventHandler(cmbsupcust_Leave);
		this.txtdocno.BackColor = System.Drawing.SystemColors.Menu;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(8, 45);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(67, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_Leave);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(1022, 43);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(38, 15);
		this.label15.TabIndex = 4;
		this.label15.Text = "Tax %";
		this.txtgrandtotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgrandtotal.Enabled = false;
		this.txtgrandtotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgrandtotal.Location = new System.Drawing.Point(882, 120);
		this.txtgrandtotal.Name = "txtgrandtotal";
		this.txtgrandtotal.ReadOnly = true;
		this.txtgrandtotal.Size = new System.Drawing.Size(243, 23);
		this.txtgrandtotal.TabIndex = 47;
		this.txtgrandtotal.TabStop = false;
		this.txtgrandtotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbTaxDrAc.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxDrAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxDrAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxDrAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxDrAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxDrAc).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbTaxDrAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxDrAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxDrAc).Location = new System.Drawing.Point(882, 64);
		((System.Windows.Forms.Control)(object)this.CmbTaxDrAc).Name = "CmbTaxDrAc";
		((System.Windows.Forms.Control)(object)this.CmbTaxDrAc).Size = new System.Drawing.Size(243, 24);
		((System.Windows.Forms.Control)(object)this.CmbTaxDrAc).TabIndex = 5;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(794, 124);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(68, 15);
		this.label5.TabIndex = 48;
		this.label5.Text = "Grand Total";
		this.ChkBoxWthHolding.AutoSize = true;
		this.ChkBoxWthHolding.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxWthHolding.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkBoxWthHolding.Location = new System.Drawing.Point(970, 41);
		this.ChkBoxWthHolding.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBoxWthHolding.Name = "ChkBoxWthHolding";
		this.ChkBoxWthHolding.Size = new System.Drawing.Size(54, 19);
		this.ChkBoxWthHolding.TabIndex = 3;
		this.ChkBoxWthHolding.TabStop = false;
		this.ChkBoxWthHolding.Text = "WHT";
		this.ChkBoxWthHolding.UseVisualStyleBackColor = true;
		this.ChkBoxWthHolding.CheckedChanged += new System.EventHandler(ChkBoxWthHolding_CheckedChanged);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(794, 151);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(62, 15);
		this.label10.TabIndex = 50;
		this.label10.Text = "Difference";
		this.CmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance46;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Location = new System.Drawing.Point(882, 38);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Name = "CmbTaxType";
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Size = new System.Drawing.Size(86, 24);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Leave += new System.EventHandler(CmbTaxType_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(794, 43);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(50, 15);
		this.label13.TabIndex = 71;
		this.label13.Text = "TaxType";
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(882, 201);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(243, 23);
		this.txtTotalAmount.TabIndex = 77;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtdifference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdifference.Enabled = false;
		this.txtdifference.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdifference.Location = new System.Drawing.Point(882, 147);
		this.txtdifference.Name = "txtdifference";
		this.txtdifference.ReadOnly = true;
		this.txtdifference.Size = new System.Drawing.Size(243, 23);
		this.txtdifference.TabIndex = 49;
		this.txtdifference.TabStop = false;
		this.txtdifference.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(794, 205);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(80, 15);
		this.label16.TabIndex = 78;
		this.label16.Text = "Total Amount";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(794, 178);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(72, 15);
		this.label20.TabIndex = 76;
		this.label20.Text = "Tax Amount";
		this.txtTaxAmount.BackColor = System.Drawing.Color.White;
		this.txtTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(882, 174);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(243, 23);
		this.txtTaxAmount.TabIndex = 75;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnDelete, this.print, this.btnAttachment, this.btnShortCutKey });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1132, 27);
		this.toolStrip3.TabIndex = 54;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = (System.Drawing.Image)resources.GetObject("print.Image");
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(61, 24);
		this.print.Text = "&Print";
		this.print.Click += new System.EventHandler(print_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(116, 24);
		this.btnAttachment.Text = "Attachments";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.ctrlGrdBar3.FormName = null;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(0, 0);
		this.ctrlGrdBar3.MyGrid = null;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(150, 150);
		this.ctrlGrdBar3.TabIndex = 76;
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1138, 713);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel6);
		this.panel7.Controls.Add(this.panel13);
		this.panel7.Controls.Add(this.panel12);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1132, 707);
		this.panel7.TabIndex = 3;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.Gridhistory);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 146);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1132, 561);
		this.panel6.TabIndex = 29;
		this.Gridhistory.AllowEdit = (InheritableBoolean)2;
		Gridhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.Gridhistory.DesignTimeLayout = Gridhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.Gridhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.Gridhistory.DynamicFiltering = true;
		this.Gridhistory.FilterMode = (FilterMode)2;
		this.Gridhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.Gridhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.Gridhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.Gridhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.Gridhistory).Name = "Gridhistory";
		((ScrollableBase)this.Gridhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.Gridhistory).Size = new System.Drawing.Size(1132, 561);
		((System.Windows.Forms.Control)(object)this.Gridhistory).TabIndex = 56;
		this.Gridhistory.TotalRow = (InheritableBoolean)1;
		this.Gridhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.Gridhistory).VisualStyle = (VisualStyle)4;
		this.Gridhistory.ColumnButtonClick += new ColumnActionEventHandler(Gridhistory_ColumnButtonClick);
		this.Gridhistory.LinkClicked += new ColumnActionEventHandler(Gridhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.Gridhistory).DoubleClick += new System.EventHandler(Gridhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.Gridhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(Gridhistory_KeyDown);
		this.panel13.Controls.Add(this.label29);
		this.panel13.Controls.Add(this.label18);
		this.panel13.Controls.Add(this.CmbStatus);
		this.panel13.Controls.Add(this.label17);
		this.panel13.Controls.Add(this.btnsearchhistory);
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.panel13.Controls.Add(this.label19);
		this.panel13.Controls.Add(this.label28);
		this.panel13.Controls.Add(this.txtInvoiceNoFrom);
		this.panel13.Controls.Add(this.txtdatetohistory);
		this.panel13.Controls.Add(this.label21);
		this.panel13.Controls.Add(this.label27);
		this.panel13.Controls.Add(this.txtInvoiceNoTo);
		this.panel13.Controls.Add(this.BillDateFrom);
		this.panel13.Controls.Add(this.DueDateTo);
		this.panel13.Controls.Add(this.BillDateTo);
		this.panel13.Controls.Add(this.DueDateFrom);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 24);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1132, 122);
		this.panel13.TabIndex = 28;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(249, 94);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(40, 15);
		this.label29.TabIndex = 29;
		this.label29.Text = "Status";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(6, 10);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(82, 15);
		this.label18.TabIndex = 27;
		this.label18.Text = "Bill Date From";
		this.CmbStatus.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.FormattingEnabled = true;
		this.CmbStatus.Items.AddRange(new object[3] { "Pending", "Received", "All" });
		this.CmbStatus.Location = new System.Drawing.Point(346, 89);
		this.CmbStatus.Name = "CmbStatus";
		this.CmbStatus.Size = new System.Drawing.Size(220, 25);
		this.CmbStatus.TabIndex = 7;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(249, 66);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(93, 15);
		this.label17.TabIndex = 13;
		this.label17.Text = "Customer Name";
		this.btnsearchhistory.BackColor = System.Drawing.Color.Teal;
		this.btnsearchhistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnsearchhistory.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsearchhistory.ForeColor = System.Drawing.Color.White;
		this.btnsearchhistory.Location = new System.Drawing.Point(572, 89);
		this.btnsearchhistory.Name = "btnsearchhistory";
		this.btnsearchhistory.Size = new System.Drawing.Size(68, 25);
		this.btnsearchhistory.TabIndex = 8;
		this.btnsearchhistory.Text = "Search";
		this.btnsearchhistory.UseVisualStyleBackColor = false;
		this.btnsearchhistory.Click += new System.EventHandler(btnsearchhistory_Click);
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(346, 61);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(294, 24);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(249, 10);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(96, 15);
		this.label19.TabIndex = 26;
		this.label19.Text = "Invoice No From";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(6, 94);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(73, 15);
		this.label28.TabIndex = 27;
		this.label28.Text = "Due Date To";
		this.txtInvoiceNoFrom.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtInvoiceNoFrom.Location = new System.Drawing.Point(346, 5);
		this.txtInvoiceNoFrom.Name = "txtInvoiceNoFrom";
		this.txtInvoiceNoFrom.Size = new System.Drawing.Size(294, 25);
		this.txtInvoiceNoFrom.TabIndex = 2;
		this.txtdatetohistory.AutoSize = true;
		this.txtdatetohistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatetohistory.Location = new System.Drawing.Point(6, 38);
		this.txtdatetohistory.Name = "txtdatetohistory";
		this.txtdatetohistory.Size = new System.Drawing.Size(67, 15);
		this.txtdatetohistory.TabIndex = 27;
		this.txtdatetohistory.Text = "Bill Date To";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(249, 38);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(81, 15);
		this.label21.TabIndex = 26;
		this.label21.Text = "Invoice No To";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(6, 66);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(88, 15);
		this.label27.TabIndex = 27;
		this.label27.Text = "Due Date From";
		this.txtInvoiceNoTo.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtInvoiceNoTo.Location = new System.Drawing.Point(346, 33);
		this.txtInvoiceNoTo.Name = "txtInvoiceNoTo";
		this.txtInvoiceNoTo.Size = new System.Drawing.Size(294, 25);
		this.txtInvoiceNoTo.TabIndex = 3;
		this.BillDateFrom.CustomFormat = "dd-MMM-yy";
		this.BillDateFrom.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.BillDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.BillDateFrom.Location = new System.Drawing.Point(96, 5);
		this.BillDateFrom.Name = "BillDateFrom";
		this.BillDateFrom.Size = new System.Drawing.Size(142, 25);
		this.BillDateFrom.TabIndex = 0;
		this.DueDateTo.CustomFormat = "dd-MMM-yy";
		this.DueDateTo.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.DueDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDateTo.Location = new System.Drawing.Point(96, 89);
		this.DueDateTo.Name = "DueDateTo";
		this.DueDateTo.Size = new System.Drawing.Size(142, 25);
		this.DueDateTo.TabIndex = 6;
		this.BillDateTo.CustomFormat = "dd-MMM-yy";
		this.BillDateTo.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.BillDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.BillDateTo.Location = new System.Drawing.Point(96, 33);
		this.BillDateTo.Name = "BillDateTo";
		this.BillDateTo.Size = new System.Drawing.Size(142, 25);
		this.BillDateTo.TabIndex = 1;
		this.DueDateFrom.CustomFormat = "dd-MMM-yy";
		this.DueDateFrom.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.DueDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDateFrom.Location = new System.Drawing.Point(96, 61);
		this.DueDateFrom.Name = "DueDateFrom";
		this.DueDateFrom.Size = new System.Drawing.Size(142, 25);
		this.DueDateFrom.TabIndex = 5;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label12);
		this.panel12.Controls.Add(this.ctrlGrdBar1);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1132, 24);
		this.panel12.TabIndex = 27;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(3, 2);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(234, 19);
		this.label12.TabIndex = 1;
		this.label12.Text = "Receipt By Contract History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1093, 2);
		this.ctrlGrdBar1.MyGrid = this.Gridhistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1146, 741);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ReceiptByContract";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ReceiptByContract_FormClosing);
		base.Load += new System.EventHandler(ReceiptByInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ReceiptByInvoiceAccount_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContract).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxCrAc).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTransType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTranagainst).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAc).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAdvanceAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxDrAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).EndInit();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.Gridhistory).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		base.ResumeLayout(false);
	}
}
