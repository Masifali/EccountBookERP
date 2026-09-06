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
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class PaymentByInvoiceAccount : Form
{
	private bool DoHaveSaveRight;

	private bool DoHavePrintRights;

	private bool DoHaveUpdateRights;

	private bool DoHaveFieldChooserRights;

	private bool DoHaveSaveLayoutRights;

	private bool DoHaveGridPrintRights;

	private bool DoHaveGridExportRights;

	private bool DoHaveGridGroupExpandRights;

	private bool DoHaveGridGroupCollapseRights;

	private bool DoHaveReportExportRights;

	private bool DoHaveReportPrintRights;

	private bool DoHaveCanViewAllRecordRights;

	private bool DoHaveDeleteRights;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DataTable dt = new DataTable();

	private DataTable dtdetail = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtCoa = new DataTable();

	private int RecId;

	private int CustomerGlId;

	private int VoucherHeadId;

	private int updateDetailIndex;

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

	private Panel panel2;

	private TextBox txtgrandtotal;

	private TextBox txtdifference;

	private Label label5;

	private Label label10;

	internal GridEX grd;

	private Panel panel3;

	private Label label12;

	private DateTimePicker datcheqdate;

	private Label label11;

	private DateTimePicker datPaymentDate;

	private Label label8;

	private TextBox txtdocno;

	private TextBox txtpaytitle;

	private Label label6;

	internal UltraCombo CmbCreditAc;

	private Label label4;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnsave;

	private ToolStripButton print;

	private Panel panel4;

	private Panel panel6;

	internal UltraCombo CmbSupplierCustomer;

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

	private Panel panel8;

	internal GridEX Gridhistory;

	private Label label29;

	private ComboBox CmbStatus;

	private Label label28;

	private Label label27;

	private DateTimePicker DueDateTo;

	private DateTimePicker DueDateFrom;

	private CheckedListBox checkedListBox1;

	private Button button1;

	private ToolStripLabel toolStripLabel1;

	private TextBox txtRemarks;

	private Label label1;

	private TextBox txtAmount;

	private Label label23;

	private TextBox txtbalance;

	internal UltraCombo CmbTranType;

	private Label label3;

	private Button btnAdd;

	private TextBox txtCustomerBalance;

	private Label label7;

	internal UltraCombo CmbSupplierName;

	private Panel panel5;

	internal GridEX grdDetail;

	internal UltraCombo CmbCheqNo;

	private Label label2;

	private Label lblCustomer;

	private Label lblAmount;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnUpdate;

	private Label label9;

	private TextBox txtpdcCheqAmount;

	private Label lblTaxPercent;

	private Label lblWhtAmount;

	private ToolStripButton btnDelete;

	private ToolStripButton btnAttachment;

	internal UltraCombo CmbTaxType;

	private Label label13;

	public PaymentByInvoiceAccount(UserAccount user)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private bool Formvalidation()
	{
		if (((UltraGridBase)CmbSupplierName).ActiveRow == null)
		{
			MessageBox.Show("Please Select Supplier");
			CmbSupplierName.Focus();
			return false;
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
		if (((UltraGridBase)CmbTranType).ActiveRow == null)
		{
			MessageBox.Show("TransType field required");
			CmbTranType.Focus();
			return false;
		}
		if (((Control)(object)CmbTranType).Text == "PdcCheq" && ((UltraGridBase)CmbCheqNo).ActiveRow == null)
		{
			MessageBox.Show("ChequeNo field required");
			CmbCheqNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCreditAc).ActiveRow == null)
		{
			MessageBox.Show("Credit Account field required");
			CmbCreditAc.Focus();
			return false;
		}
		if (txtAmount.Text == string.Empty || Conversion.ToDouble((object)txtAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount field required");
			txtAmount.Focus();
			return false;
		}
		return true;
	}

	private void ReceiptByInvoice_Load(object sender, EventArgs e)
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
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Field Chooser").ToList();
				DoHaveFieldChooserRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Save Layout").ToList();
				DoHaveSaveLayoutRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Grid Print").ToList();
				DoHaveGridPrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Grid Export").ToList();
				DoHaveGridExportRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Group Collapse").ToList();
				DoHaveGridGroupCollapseRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Group Expand").ToList();
				DoHaveGridGroupExpandRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Report Export").ToList();
				DoHaveReportExportRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Report Print").ToList();
				DoHaveReportPrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "CanView AllRecord").ToList();
				DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Delete").ToList();
				DoHaveDeleteRights = Conversion.ToBool((object)newList[0].Value.ToString());
			}
			btnsave.Enabled = DoHaveSaveRight;
			print.Enabled = DoHavePrintRights;
			btnUpdate.Enabled = DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = DoHaveGridGroupExpandRights;
			btnDelete.Enabled = DoHaveDeleteRights;
			datPaymentDate.Focus();
			SupplierCustomer();
			DebitAccount();
			VoucherNofill();
			TransTypeFill();
			BindTaxTypes();
			lblCustomer.Visible = false;
			lblAmount.Visible = false;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			dtViewAttachments.Columns.Add("AttchmentPath");
			dt.Columns.Add("Id");
			dt.Columns.Add("DocNo");
			dt.Columns.Add("DocDate");
			dt.Columns.Add("InvoiceNo");
			dt.Columns.Add("DocumentType");
			dt.Columns.Add("DocumentTypeId");
			dt.Columns.Add("DueDate");
			dt.Columns.Add("ManualBillNo");
			dt.Columns.Add("AdvanceAmount", typeof(double));
			dt.Columns.Add("BillAmount", typeof(double));
			dt.Columns.Add("TotalPaid", typeof(double));
			dt.Columns.Add("Balance", typeof(double));
			dt.Columns.Add("Payment", typeof(double));
			dt.Columns.Add("Wht", typeof(double));
			dt.Columns.Add("Adjustment", typeof(double));
			dtdetail.Columns.Add("TransType");
			dtdetail.Columns.Add("GLAccountId");
			dtdetail.Columns.Add("AccountTitle");
			dtdetail.Columns.Add("Remarks");
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("CheqId");
			dtdetail.Columns.Add("CheqNo");
			dtdetail.Columns.Add("CheqDate");
			dtdetail.Columns.Add("Paytitle");
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
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
				txtbalance.Text = detail.Rows[0]["Balance"].ToString();
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
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerCurrentBalance()
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
			DataTable dt = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = DateTime.Now,
				RefAccountId = Conversion.ToInt((object)CustomerGlId)
			});
			if (dt.Rows.Count > 0)
			{
				txtCustomerBalance.Text = dt.Rows[0]["Balance"].ToString();
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
		//IL_0034: Expected O, but got Unknown
		try
		{
			int Code = PaymentByInvoiceHeader.GenerateCode(new PaymentByInvoiceHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 14
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
		try
		{
			dtSupplier = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, CmbSupplierName, "Id", "CompanyName", "Supplier Name", false);
				DDL.BindDDLNew(dtSupplier, CmbSupplierCustomer, "Id", "CompanyName", "Supplier Name", false);
				((UltraGridBase)CmbSupplierName).DisplayLayout.Bands[0].Columns[1].Width = 280;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierGLIdGet()
	{
		try
		{
			if (dtSupplier.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtSupplier.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtSupplier.Rows[i]["Id"]) == Conversion.ToInt(CmbSupplierName.Value))
				{
					CustomerGlId = Conversion.ToInt((object)dtSupplier.Rows[i]["GlAccountId"].ToString());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DebitAccount()
	{
		try
		{
			dtCoa = CommonServices.CoaAllocationGetAllServiceBind();
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
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "PartyName");
			dt.Rows.Add(2, "Cash");
			dt.Rows.Add(3, "Bank");
			dt.Rows.Add(4, "WhtHolding");
			dt.Rows.Add(5, "Advance");
			dt.Rows.Add(6, "PdcCheq");
			DDL.BindDDLNew(dt, CmbTranType, "Id", "Name", "TransType", false);
			((UltraGridBase)CmbTranType).Rows[0].Activate();
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
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, 14);
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

	private void cmbsupcust_Leave(object sender, EventArgs e)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbSupplierName.Value) == 0 || ((Control)(object)CmbSupplierName).Text.Trim() == string.Empty)
			{
				grd.ClearStructure();
				throw new Exception("Please select Supplier Name first");
			}
			CustomerCurrentBalance();
			if (!btnsave.Visible || !btnsave.Enabled)
			{
				return;
			}
			DataTable lst = VoucherHead.GetReceivablesByInvoice(new ReportsParameters
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
					dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["VoucherCode"], Convert.ToDateTime(lst.Rows[i]["VoucherDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["DocumentTypeSrNo"], lst.Rows[i]["DocumentTypeCode"], lst.Rows[i]["DocumentTypeId"], Convert.ToDateTime(lst.Rows[i]["DueDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["ManualBillNo"], lst.Rows[i]["AdvanceAmount"], lst.Rows[i]["BillAmount"], lst.Rows[i]["ReceivedPaidTotal"], Convert.ToDouble(lst.Rows[i]["BalanceAmount"]), 0, lst.Rows[i]["DWhtAmount"], lst.Rows[i]["DAdjustmentAmount"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GridSettings();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["InvoiceNo"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["Adjustment"].Visible = false;
			grd.RootTable.Columns["BillAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["Balance"].EditType = (EditType)0;
			grd.RootTable.Columns["TotalPaid"].EditType = (EditType)0;
			grd.RootTable.Columns["Wht"].EditType = (EditType)0;
			grd.RootTable.Columns["Adjustment"].EditType = (EditType)0;
			grd.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grd.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grd.RootTable.Columns["InvoiceNo"].EditType = (EditType)0;
			grd.RootTable.Columns["DocumentType"].EditType = (EditType)0;
			grd.RootTable.Columns["DueDate"].EditType = (EditType)0;
			grd.RootTable.Columns["ManualBillNo"].EditType = (EditType)0;
			grd.RootTable.Columns["Payment"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Balance"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AdvanceAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TotalPaid"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Wht"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Adjustment"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Payment"].FormatString = "#,#";
			grd.RootTable.Columns["BillAmount"].FormatString = "#,#";
			grd.RootTable.Columns["Balance"].FormatString = "#,#";
			grd.RootTable.Columns["AdvanceAmount"].FormatString = "#,#";
			grd.RootTable.Columns["TotalPaid"].FormatString = "#,#";
			grd.RootTable.Columns["Wht"].FormatString = "#,#";
			grd.RootTable.Columns["Adjustment"].FormatString = "#,#";
			grd.RootTable.Columns["Payment"].TotalFormatString = "#,#";
			grd.RootTable.Columns["BillAmount"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Balance"].TotalFormatString = "#,#";
			grd.RootTable.Columns["AdvanceAmount"].TotalFormatString = "#,#";
			grd.RootTable.Columns["TotalPaid"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Wht"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Adjustment"].TotalFormatString = "#,#";
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GridDetailSettings()
	{
		try
		{
			grdDetail.RootTable.Columns["GLAccountId"].Visible = false;
			grdDetail.RootTable.Columns["CheqId"].Visible = false;
			grdDetail.RootTable.Columns["AccountTitle"].Width = 120;
			grdDetail.RootTable.Columns["Amount"].Width = 80;
			grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			RecId = 0;
			txtpaytitle.Text = "";
			txtdocno.Text = "";
			datPaymentDate.Text = "";
			((Control)(object)CmbCheqNo).Text = "";
			txtAmount.Text = "";
			((Control)(object)CmbCreditAc).Text = "";
			((Control)(object)CmbSupplierName).Text = "";
			txtbalance.Text = string.Empty;
			txtCustomerBalance.Text = string.Empty;
			lblCustomer.Visible = false;
			lblAmount.Visible = false;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			VoucherNofill();
			dt.Rows.Clear();
			grd.DataSource = dt;
			dtdetail.Rows.Clear();
			grdDetail.DataSource = dtdetail;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		txtpaytitle.Text = "";
		lblTaxPercent.Text = "";
		lblWhtAmount.Text = "";
		txtpdcCheqAmount.Text = "";
		((Control)(object)CmbCheqNo).Text = "";
		txtAmount.Text = "";
		((Control)(object)CmbCreditAc).Text = "";
		txtbalance.Text = string.Empty;
		txtCustomerBalance.Text = string.Empty;
		lblCustomer.Visible = false;
		lblAmount.Visible = false;
		CmbTranType.Focus();
	}

	private void CmbTranType_Leave(object sender, EventArgs e)
	{
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		try
		{
			((Control)(object)CmbCreditAc).Text = string.Empty;
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			if ((((Control)(object)CmbTranType).Text == "PartyName" || ((Control)(object)CmbTranType).Text == "PdcCheq") && dtCoa.Rows.Count > 0)
			{
				for (int m = 0; m < dtCoa.Rows.Count; m++)
				{
					if (Convert.ToInt32(dtCoa.Rows[m]["AccountTypeId"]) == 3)
					{
						dt.Rows.Add(dtCoa.Rows[m]["Id"], dtCoa.Rows[m]["AccountTitle"]);
					}
				}
				DDL.BindDDL(dt, CmbCreditAc, "Id", "AccountTitle", "AccountTitle", ZeroIndex: false);
			}
			if (((Control)(object)CmbTranType).Text == "WhtHolding" && dtCoa.Rows.Count > 0)
			{
				for (int l = 0; l < dtCoa.Rows.Count; l++)
				{
					if (Convert.ToInt32(dtCoa.Rows[l]["AccountTypeId"]) == 3)
					{
						dt.Rows.Add(dtCoa.Rows[l]["Id"], dtCoa.Rows[l]["AccountTitle"]);
					}
				}
				DDL.BindDDL(dt, CmbCreditAc, "Id", "AccountTitle", "AccountTitle", ZeroIndex: false);
				TaxScheduleMain taxSchedule = new TaxScheduleMain();
				taxSchedule.OrganizationId = UserAccount.OrganizationId;
				taxSchedule.CompanyId = UserAccount.CompanyId;
				taxSchedule.EffectedDate = datPaymentDate.Value;
				taxSchedule.TaxNameId = Conversion.ToInt(CmbTaxType.Value);
				if (TaxScheduleMain.ReadTaxSchedule(taxSchedule).Rows.Count == 0)
				{
					((Control)(object)CmbCreditAc).Text = string.Empty;
					lblTaxPercent.Text = "0";
				}
				else
				{
					DataTable detail = TaxScheduleMain.ReadTaxSchedule(taxSchedule);
					lblTaxPercent.Text = detail.Rows[0]["TaxPercent"].ToString();
					if (btnsave.Text == "&Save")
					{
						CmbCreditAc.Value = detail.Rows[0]["TaxGLAccountId"];
						((Control)(object)CmbCreditAc).Text = detail.Rows[0]["AccountTitle"].ToString();
					}
					if (grd.GetCheckedRows().Count() <= 0)
					{
						throw new Exception("select rows first");
					}
					double whtTotalAmt = 0.0;
					GridEXRow[] checkedRows = grd.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						item.BeginEdit();
						whtTotalAmt += Convert.ToDouble(item.Cells["BillAmount"].Value);
						double wht = Math.Round(Convert.ToDouble(item.Cells["BillAmount"].Value) * Conversion.ToDouble((object)lblTaxPercent.Text) / 100.0, 2);
						item.Cells["Wht"].Value = wht;
						item.EndEdit();
					}
					double taxamount = Math.Round(whtTotalAmt * Conversion.ToDouble((object)lblTaxPercent.Text) / 100.0, 2);
					txtAmount.Text = taxamount.ToString();
					lblWhtAmount.Text = taxamount.ToString();
				}
			}
			if (((Control)(object)CmbTranType).Text == "Cash" && dtCoa.Rows.Count > 0)
			{
				for (int k = 0; k < dtCoa.Rows.Count; k++)
				{
					if (Convert.ToInt32(dtCoa.Rows[k]["AccountTypeId"]) == 2)
					{
						dt.Rows.Add(dtCoa.Rows[k]["Id"], dtCoa.Rows[k]["AccountTitle"]);
					}
				}
				DDL.BindDDL(dt, CmbCreditAc, "Id", "AccountTitle", "AccountTitle", ZeroIndex: false);
			}
			if (((Control)(object)CmbTranType).Text == "Bank" && dtCoa.Rows.Count > 0)
			{
				for (int j = 0; j < dtCoa.Rows.Count; j++)
				{
					if (Convert.ToInt32(dtCoa.Rows[j]["AccountTypeId"]) == 15)
					{
						dt.Rows.Add(dtCoa.Rows[j]["Id"], dtCoa.Rows[j]["AccountTitle"]);
					}
				}
				DDL.BindDDL(dt, CmbCreditAc, "Id", "AccountTitle", "AccountTitle", ZeroIndex: false);
			}
			if (!(((Control)(object)CmbTranType).Text == "Advance") || dtSupplier.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtsupcust = new DataTable();
			dtsupcust.Columns.Add("AdvanceGlAcId");
			dtsupcust.Columns.Add("AccountTitleAdvance");
			for (int i = 0; i < dtSupplier.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtSupplier.Rows[i]["Id"]) == Convert.ToInt32(CmbSupplierName.Value))
				{
					dtsupcust.Rows.Add(dtSupplier.Rows[i]["AdvanceGlAcId"], dtSupplier.Rows[i]["AccountTitleAdvance"]);
				}
			}
			if (dtsupcust.Rows.Count > 0)
			{
				DDL.BindDDL(dtsupcust, CmbCreditAc, "AdvanceGlAcId", "AccountTitleAdvance", "AccountTitle", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		try
		{
			string PurchaseNumber = "";
			List<ConfigrationsAllocation> PurchaseList = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "PaymentByInvoicePurchaseInvoice").ToList();
			if (PurchaseList.Count > 0)
			{
				PurchaseNumber = PurchaseList[0].ConfigKey;
			}
			DataTable lst = VoucherHead.GetReceivablesByInvoice(new ReportsParameters
			{
				SupplierCustomerId = Conversion.ToInt(CmbSupplierCustomer.Value),
				DueDateFrom = DueDateFrom.Value,
				DueDateTo = DueDateTo.Value,
				FromDate = BillDateFrom.Value,
				ToDate = BillDateTo.Value,
				FromDocNo = Conversion.ToInt((object)txtInvoiceNoFrom.Text.Trim()),
				ToDocNo = Conversion.ToInt((object)txtInvoiceNoTo.Text.Trim()),
				SaleInvoiceDocumentTypeIds = PurchaseNumber
			});
			if (lst.Rows.Count > 0)
			{
				Gridhistory.DataSource = lst;
				Gridhistory.RetrieveStructure();
				GridSettinghistory();
			}
			else
			{
				dt.Rows.Clear();
				Gridhistory.DataSource = dt;
			}
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

	private void GridSettinghistory()
	{
		try
		{
			Gridhistory.RootTable.Columns["Id"].Visible = false;
			Gridhistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			Gridhistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			Gridhistory.RootTable.Columns["AdvanceAmount"].AggregateFunction = (AggregateFunction)2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void Insert()
	{
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Expected O, but got Unknown
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Expected O, but got Unknown
		if (!Formvalidation())
		{
			return;
		}
		try
		{
			GrandTotal();
			double Difference = Conversion.ToDouble((object)txtdifference.Text);
			if (Difference > 0.0 || Difference < 0.0)
			{
				MessageBox.Show("Please Clear The Difference Amount");
				return;
			}
			if (Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2).ToString()) != Conversion.ToDouble((object)txtgrandtotal.Text.Trim()))
			{
				throw new Exception("Please Check Your Payment & Total Amount Is Not Equal");
			}
			double adv = 0.0;
			double wht = 0.0;
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				string obj = item.Cells["TransType"].Value.ToString();
				if (obj == "Advance")
				{
					adv += Convert.ToDouble(item.Cells["Amount"].Value.ToString());
				}
				if (obj == "WhtHolding")
				{
					wht += Convert.ToDouble(item.Cells["Amount"].Value.ToString());
				}
			}
			double Advance = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["AdvanceAmount"], (AggregateFunction)2).ToString());
			double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["Wht"], (AggregateFunction)2).ToString());
			if (adv != Advance)
			{
				throw new Exception("Please Check Your Advance Amount Is Not Equal");
			}
			if (num != wht)
			{
				throw new Exception("Please Check Your WhtHolding Amount Is Not Equal");
			}
			PaymentByInvoiceHeader receipts = new PaymentByInvoiceHeader();
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
			receipts.DocumentTypeId = 14;
			receipts.DocDate = Conversion.ToDateTime((object)datPaymentDate.Value);
			receipts.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			receipts.SupplierCustomerId = Conversion.ToInt(CmbSupplierName.Value);
			receipts.OrganizationId = UserAccount.OrganizationId;
			receipts.CompanyId = UserAccount.CompanyId;
			receipts.EntryUser = UserAccount.ID;
			receipts.ModifyUser = UserAccount.ID;
			receipts.EntryDate = DateTime.Now;
			receipts.ModifyDate = DateTime.Now;
			SupplierGLIdGet();
			receipts.PaymentByInvoiceDetailList = new List<PaymentByInvoiceDetail>();
			receipts.PaymentByInvoiceOtherDetailList = new List<PaymentByInvoiceOtherDetail>();
			rows = grdDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				PaymentByInvoiceOtherDetail vd2 = new PaymentByInvoiceOtherDetail();
				vd2.TransType = Conversion.ToString(r2.Cells["TransType"].Value);
				vd2.GLAccountId = Conversion.ToInt(r2.Cells["GLAccountId"].Value);
				vd2.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
				vd2.Amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
				vd2.CheqId = Conversion.ToInt(r2.Cells["CheqId"].Value);
				vd2.CheqNo = Conversion.ToString(r2.Cells["CheqNo"].Value);
				vd2.CheqDate = Conversion.ToDateTime(r2.Cells["CheqDate"].Value);
				vd2.PayTitle = Conversion.ToString(r2.Cells["Paytitle"].Value);
				receipts.PaymentByInvoiceOtherDetailList.Add(vd2);
			}
			bool b = false;
			if (grd.RowCount <= 0)
			{
				return;
			}
			double SupplierAmount = 0.0;
			rows = grd.GetCheckedRows();
			foreach (GridEXRow r in rows)
			{
				b = true;
				double TotalWht = 0.0;
				double TotalAdjustment = 0.0;
				double AdvanceAmount = 0.0;
				double num2 = Conversion.ToInt(r.Cells["Payment"].Value);
				TotalWht = Conversion.ToInt(r.Cells["Wht"].Value);
				TotalAdjustment = Conversion.ToSingle(r.Cells["Adjustment"].Value);
				AdvanceAmount = Conversion.ToSingle(r.Cells["AdvanceAmount"].Value);
				double SupAmount = num2 + TotalWht + TotalAdjustment + AdvanceAmount;
				SupplierAmount += SupAmount;
				PaymentByInvoiceDetail vd = new PaymentByInvoiceDetail();
				vd.RefDocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
				vd.RefDocNoId = Conversion.ToInt(r.Cells["InvoiceNo"].Value);
				vd.InvoiceNo = Conversion.ToInt(r.Cells["DocNo"].Value);
				vd.InvoiceDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
				vd.DueDate = Conversion.ToDateTime(r.Cells["DueDate"].Value);
				vd.Amount = Conversion.ToSingle(r.Cells["Payment"].Value);
				vd.DWhtAmount = Conversion.ToInt(r.Cells["Wht"].Value);
				vd.AdvanceAmount = Conversion.ToSingle(r.Cells["AdvanceAmount"].Value);
				vd.DAdjustmentAmount = Conversion.ToSingle(r.Cells["Adjustment"].Value);
				vd.MannualBillNo = Conversion.ToString(r.Cells["ManualBillNo"].Value);
				vd.BillAmount = Conversion.ToSingle(r.Cells["BillAmount"].Value);
				vd.TotalPaidAmount = Conversion.ToSingle(r.Cells["TotalPaid"].Value);
				vd.BalanceAmount = Conversion.ToSingle(r.Cells["Balance"].Value);
				receipts.PaymentByInvoiceDetailList.Add(vd);
			}
			if (!b)
			{
				MessageBox.Show("Please Checked Record For Save");
				return;
			}
			receipts.AttachmentsList = new List<DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.ScreenName = base.Name;
				receipts.AttachmentsList.Add(attach);
			}
			PaymentByInvoiceHeader.Save(receipts);
			if (RecId == 0)
			{
				MessageBox.Show("Record Save Successfully");
			}
			else
			{
				MessageBox.Show("Record Update Successfully");
			}
			Reset();
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

	public void ReadById(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			PaymentByInvoiceHeader receipts = new PaymentByInvoiceHeader();
			RecId = Conversion.ToInt((object)ID);
			receipts = PaymentByInvoiceHeader.GetByID(RecId);
			if (receipts == null)
			{
				return;
			}
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			datPaymentDate.Value = receipts.DocDate;
			CmbSupplierName.Value = receipts.SupplierCustomerId;
			txtdocno.Text = Conversion.ToString((object)receipts.DocNo);
			dt.Rows.Clear();
			dtdetail.Rows.Clear();
			VoucherhHeadIdGet();
			for (int j = 0; j < receipts.PaymentByInvoiceDetailList.Count; j++)
			{
				dt.Rows.Add(receipts.PaymentByInvoiceDetailList[j].Id, receipts.PaymentByInvoiceDetailList[j].InvoiceNo, Convert.ToDateTime(receipts.PaymentByInvoiceDetailList[j].InvoiceDate).ToString("dd-MMM-yyyyy"), receipts.PaymentByInvoiceDetailList[j].RefDocNoId, receipts.PaymentByInvoiceDetailList[j].DocumentTypeCode, receipts.PaymentByInvoiceDetailList[j].RefDocumentTypeId, Convert.ToDateTime(receipts.PaymentByInvoiceDetailList[j].DueDate).ToString("dd-MMM-yyyyy"), receipts.PaymentByInvoiceDetailList[j].MannualBillNo, receipts.PaymentByInvoiceDetailList[j].AdvanceAmount, receipts.PaymentByInvoiceDetailList[j].BillAmount, receipts.PaymentByInvoiceDetailList[j].TotalPaidAmount, receipts.PaymentByInvoiceDetailList[j].BalanceAmount, receipts.PaymentByInvoiceDetailList[j].Amount, receipts.PaymentByInvoiceDetailList[j].DWhtAmount, receipts.PaymentByInvoiceDetailList[j].DAdjustmentAmount);
			}
			grd.DataSource = dt;
			grd.RetrieveStructure();
			GridSettings();
			dtdetail.Rows.Clear();
			for (int i = 0; i < receipts.PaymentByInvoiceOtherDetailList.Count; i++)
			{
				dtdetail.Rows.Add(receipts.PaymentByInvoiceOtherDetailList[i].TransType, receipts.PaymentByInvoiceOtherDetailList[i].GLAccountId, receipts.PaymentByInvoiceOtherDetailList[i].AccountTitle, receipts.PaymentByInvoiceOtherDetailList[i].Remarks, receipts.PaymentByInvoiceOtherDetailList[i].Amount, receipts.PaymentByInvoiceOtherDetailList[i].CheqId, receipts.PaymentByInvoiceOtherDetailList[i].CheqNo, Convert.ToDateTime(receipts.PaymentByInvoiceOtherDetailList[i].CheqDate).ToString("dd-MMM-yyyyy"), receipts.PaymentByInvoiceOtherDetailList[i].PayTitle);
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
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void ReceiptByInvoiceAccount_KeyDown(object sender, KeyEventArgs e)
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
			btnNew_Click(null, null);
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
			Insert();
		}
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.VoucherReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 14,
				Id = VoucherHeadId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "102-AcRptPaymentReceiptsVoucherSlip.rpt");
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
			double TotalPayment = 0.0;
			double TotalWht = 0.0;
			double TotalAdjustment = 0.0;
			double GrandTotalamount = 0.0;
			if (grd.RootTable.Columns["Payment"] != null && grd.RootTable.Columns["Wht"] != null && grd.RootTable.Columns["Adjustment"] != null)
			{
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					TotalPayment += Conversion.ToDouble(item.Cells["Payment"].Value);
					TotalWht += Conversion.ToDouble(item.Cells["Wht"].Value);
					TotalAdjustment += Conversion.ToDouble(item.Cells["Adjustment"].Value);
				}
				GrandTotalamount = TotalPayment + TotalWht + TotalAdjustment;
				txtgrandtotal.Text = GrandTotalamount.ToString();
			}
			if (btnsave.Visible)
			{
				if (grdDetail.RootTable.Columns["Amount"] != null)
				{
					detailAmount = Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				}
			}
			else
			{
				GridEXRow[] checkedRows = grdDetail.GetRows();
				foreach (GridEXRow r in checkedRows)
				{
					detailAmount += Convert.ToDouble(r.Cells["Amount"].Value);
				}
			}
			double diff = GrandTotalamount - detailAmount;
			txtdifference.Text = diff.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grd.UpdateData();
	}

	private void grd_TextChanged(object sender, EventArgs e)
	{
		GrandTotal();
	}

	private void grd_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		GrandTotal();
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		grd.UpdateData();
		GrandTotal();
	}

	private void grd_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		GrandTotal();
	}

	private void grd_CellEdited(object sender, ColumnActionEventArgs e)
	{
		GridEXRow item = grd.CurrentRow;
		double Balance = 0.0;
		if (item.Cells["Payment"].Value != null && item.Cells["Balance"].Value != null)
		{
			double num = Conversion.ToDouble((object)item.Cells["Payment"].Value.ToString());
			Balance = Conversion.ToDouble((object)item.Cells["Balance"].Value.ToString());
			if (num > Balance)
			{
				item.Cells["Payment"].Value = 0;
				MessageBox.Show("Payment Amount Is Larger Than Balance Amount");
				return;
			}
		}
		GrandTotal();
	}

	private void cmbcashbankac_Leave(object sender, EventArgs e)
	{
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		try
		{
			((UltraGridBase)CmbCheqNo).DataSource = null;
			((Control)(object)CmbCheqNo).Text = string.Empty;
			txtpdcCheqAmount.Text = string.Empty;
			datcheqdate.Value = DateTime.Now;
			AccountCurrentBalance();
			if (((Control)(object)CmbTranType).Text == "Bank")
			{
				List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CheqBook Enabled").ToList();
				if (list.Count > 0)
				{
					if (bool.Parse(list[0].ConfigKey.ToString()))
					{
						CheqBookHeader val = new CheqBookHeader
						{
							BankId = Conversion.ToInt(CmbCreditAc.Value)
						};
						DataTable dtout = new DataTable();
						dtout = CheqBookHeader.OutstandingCheqNo(val);
						if (dtout.Rows.Count > 0)
						{
							DDL.BindDDLNew(dtout, CmbCheqNo, "Id", "CheqNo", "Cheq No", false);
							CmbCheqNo.LimitToList = true;
						}
						else
						{
							((UltraGridBase)CmbCheqNo).DataSource = null;
							((Control)(object)CmbCheqNo).Text = string.Empty;
							CmbCheqNo.LimitToList = false;
						}
					}
					else
					{
						MessageBox.Show("CheqBook Enabled Configuration False");
					}
				}
				else
				{
					MessageBox.Show("CheqBook Enabled Configuration not found");
				}
			}
			if (((Control)(object)CmbTranType).Text == "PdcCheq")
			{
				DataTable dtgetby = PdcInventory.GetByType(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(CmbCreditAc.Value)
				});
				if (dtgetby.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtgetby, CmbCheqNo, "Id", "CheqNo", "Cheq No", false);
					CmbCheqNo.LimitToList = true;
				}
				else
				{
					((UltraGridBase)CmbCheqNo).DataSource = null;
					CmbCheqNo.LimitToList = false;
					((Control)(object)CmbCheqNo).Text = string.Empty;
				}
			}
			if (((Control)(object)CmbTranType).Text == "Advance")
			{
				double balance = PaymentByInvoiceHeader.PaymentByInvoiceAdvanceBalance(new PaymentByInvoiceHeader
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					SupplierCustomerId = Convert.ToInt32(CmbCreditAc.Value)
				});
				if (balance > 0.0)
				{
					txtAmount.Text = balance.ToString();
				}
				else
				{
					txtAmount.Text = "0";
				}
			}
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
			int Id = PaymentByInvoiceHeader.GetPaymentbyInvoiceIdbyDocNo(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocNo = Conversion.ToInt((object)txtdocno.Text.Trim()),
				DocumentTypeId = 14
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			BillDateFrom.Value = DateTime.Now.AddDays(-30.0);
			DueDateFrom.Value = DateTime.Now.AddDays(-30.0);
			btnsearchhistory_Click(null, null);
			HistoryGridFill();
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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

	private void btnsearchhistory_Click(object sender, EventArgs e)
	{
		HistoryGridFill();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		try
		{
			string s = "";
			for (int i = 0; i <= checkedListBox1.Items.Count; i++)
			{
				if (checkedListBox1.GetItemChecked(i))
				{
					s = s + checkedListBox1.Items[i]?.ToString() + ",";
				}
			}
			MessageBox.Show(s);
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
			if (!FormValidationDetail())
			{
				return;
			}
			if (((Control)(object)CmbTranType).Text == "PdcCheq")
			{
				double num = Convert.ToDouble(txtAmount.Text);
				double PdcCheqAmount = Convert.ToDouble(txtpdcCheqAmount.Text);
				if (num != PdcCheqAmount)
				{
					txtAmount.Focus();
					MessageBox.Show("Amount & pdcChequeAmount not equal");
					return;
				}
			}
			string Type = "";
			int cheqid = 0;
			if (grdDetail.RowCount > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					Type = obj.Cells["TransType"].Value.ToString();
					if (((Control)(object)CmbTranType).Text.ToString() == "Advance" && Type == "Advance")
					{
						CmbTranType.Focus();
						throw new Exception("Advance Amount not add again");
					}
					if (((Control)(object)CmbTranType).Text.ToString() == "WhtHolding" && Type == "WhtHolding")
					{
						CmbTranType.Focus();
						throw new Exception("WhtHolding Amount not add again");
					}
					cheqid = Conversion.ToInt(obj.Cells["CheqId"].Value);
					if ((((Control)(object)CmbTranType).Text == "PdcCheq" || ((Control)(object)CmbTranType).Text == "Bank") && cheqid == Convert.ToInt32(CmbCheqNo.Value))
					{
						CmbCheqNo.Focus();
						throw new Exception("CheqNo already exist in grid");
					}
				}
			}
			if (grd.RowCount > 0 && ((Control)(object)CmbTranType).Text == "WhtHolding")
			{
				double num2 = Convert.ToDouble(grd.GetTotal(grd.RootTable.Columns["wht"], (AggregateFunction)2));
				double amount = Convert.ToDouble(txtAmount.Text);
				if (num2 != amount)
				{
					CmbTranType.Focus();
					throw new Exception("Wht Holding & Amount not equal");
				}
			}
			if (btnAdd.Text == "Update")
			{
				dtdetail.Rows[updateDetailIndex]["TransType"] = ((Control)(object)CmbTranType).Text;
				dtdetail.Rows[updateDetailIndex]["GLAccountId"] = CmbCreditAc.Value;
				dtdetail.Rows[updateDetailIndex]["AccountTitle"] = ((Control)(object)CmbCreditAc).Text;
				dtdetail.Rows[updateDetailIndex]["Remarks"] = txtRemarks.Text;
				dtdetail.Rows[updateDetailIndex]["Amount"] = txtAmount.Text;
				dtdetail.Rows[updateDetailIndex]["CheqId"] = CmbCheqNo.Value;
				dtdetail.Rows[updateDetailIndex]["CheqNo"] = ((Control)(object)CmbCheqNo).Text;
				dtdetail.Rows[updateDetailIndex]["CheqDate"] = datcheqdate.Value;
				dtdetail.Rows[updateDetailIndex]["Paytitle"] = txtpaytitle.Text;
				grdDetail.DataSource = dtdetail;
				btnAdd.Text = "+";
				grdDetail.RetrieveStructure();
				GridDetailSettings();
				ResetDetail();
				GrandTotal();
			}
			else
			{
				dtdetail.Rows.Add(((Control)(object)CmbTranType).Text, CmbCreditAc.Value, ((Control)(object)CmbCreditAc).Text, txtRemarks.Text.Trim(), txtAmount.Text, CmbCheqNo.Value, ((Control)(object)CmbCheqNo).Text, Convert.ToDateTime(datcheqdate.Value).ToString("dd-MMM-yyyy"), txtpaytitle.Text.Trim());
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				GridDetailSettings();
				ResetDetail();
				GrandTotal();
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

	private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		DebitAccount();
		SupplierCustomer();
		BindTaxTypes();
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grdDetail.UpdateData();
		GrandTotal();
	}

	private void grdDetail_CellEdited(object sender, ColumnActionEventArgs e)
	{
		GrandTotal();
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		Insert();
	}

	private void grdDetail_TextChanged(object sender, EventArgs e)
	{
		GrandTotal();
	}

	private void btnGenerate_Click(object sender, EventArgs e)
	{
		try
		{
			Convert.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2));
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			for (int i = 0; i < checkedRows.Length; i++)
			{
				_ = checkedRows[i];
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCheqNo_Leave(object sender, EventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		try
		{
			if (((Control)(object)CmbTranType).Text == "PdcCheq")
			{
				ReportsParameters val = new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(CmbCreditAc.Value),
					CheqId = Conversion.ToInt(CmbCheqNo.Value)
				};
				DataTable dtchqno = new DataTable();
				dtchqno = PdcInventory.GetByType(val);
				if (dtchqno.Rows.Count > 0)
				{
					datcheqdate.Value = Conversion.ToDateTime(dtchqno.Rows[0]["CheqDate"]);
					txtAmount.Text = dtchqno.Rows[0]["CheqAmount"].ToString();
					txtpdcCheqAmount.Text = dtchqno.Rows[0]["CheqAmount"].ToString();
				}
				else
				{
					txtAmount.Text = "0";
					txtpdcCheqAmount.Text = "0";
					datcheqdate.Value = DateTime.Now;
				}
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
				if (item.Cells["TransType"].Text == "WhtHolding")
				{
					throw new Exception("WhtHolding entry not Update please delete first");
				}
				((Control)(object)CmbTranType).Text = item.Cells["TransType"].Text.ToString();
				CmbTranType_Leave(null, null);
				CmbCreditAc.Value = item.Cells["GLAccountId"].Value.ToString();
				((Control)(object)CmbCreditAc).Text = item.Cells["AccountTitle"].Text.ToString();
				txtRemarks.Text = item.Cells["Remarks"].Text.ToString();
				txtAmount.Text = item.Cells["Amount"].Value.ToString();
				CmbCheqNo.Value = item.Cells["CheqId"].Value.ToString();
				((Control)(object)CmbCheqNo).Text = item.Cells["CheqNo"].Text.ToString();
				datcheqdate.Value = Convert.ToDateTime(item.Cells["CheqDate"].Value);
				txtpaytitle.Text = item.Cells["Paytitle"].Value.ToString();
				btnAdd.Text = "Update";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void Gridhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = Gridhistory.CurrentRow;
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

	private void CmbTaxType_Leave(object sender, EventArgs e)
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		try
		{
			if (!(((Control)(object)CmbTranType).Text == "WhtHolding") || dtCoa.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtCoa.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtCoa.Rows[i]["AccountTypeId"]) == 3)
				{
					dt.Rows.Add(dtCoa.Rows[i]["Id"], dtCoa.Rows[i]["AccountTitle"]);
				}
			}
			DDL.BindDDL(dt, CmbCreditAc, "Id", "AccountTitle", "AccountTitle", ZeroIndex: false);
			TaxScheduleMain taxSchedule = new TaxScheduleMain();
			taxSchedule.OrganizationId = UserAccount.OrganizationId;
			taxSchedule.CompanyId = UserAccount.CompanyId;
			taxSchedule.EffectedDate = datPaymentDate.Value;
			taxSchedule.TaxNameId = Conversion.ToInt(CmbTaxType.Value);
			if (TaxScheduleMain.ReadTaxSchedule(taxSchedule).Rows.Count == 0)
			{
				((Control)(object)CmbCreditAc).Text = string.Empty;
				lblTaxPercent.Text = "0";
				return;
			}
			DataTable detail = TaxScheduleMain.ReadTaxSchedule(taxSchedule);
			lblTaxPercent.Text = detail.Rows[0]["TaxPercent"].ToString();
			if (btnsave.Text == "&Save")
			{
				CmbCreditAc.Value = detail.Rows[0]["TaxGLAccountId"];
				((Control)(object)CmbCreditAc).Text = detail.Rows[0]["AccountTitle"].ToString();
			}
			if (grd.GetCheckedRows().Count() > 0)
			{
				double whtTotalAmt = 0.0;
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					item.BeginEdit();
					whtTotalAmt += Convert.ToDouble(item.Cells["BillAmount"].Value);
					double wht = Math.Round(Convert.ToDouble(item.Cells["BillAmount"].Value) * Conversion.ToDouble((object)lblTaxPercent.Text) / 100.0, 2);
					item.Cells["Wht"].Value = wht;
					item.EndEdit();
				}
				double taxamount = Math.Round(whtTotalAmt * Conversion.ToDouble((object)lblTaxPercent.Text) / 100.0, 2);
				txtAmount.Text = taxamount.ToString();
				lblWhtAmount.Text = taxamount.ToString();
				return;
			}
			throw new Exception("select rows first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnHeaderClick(object sender, ColumnActionEventArgs e)
	{
		grd.UpdateData();
		GrandTotal();
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
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Expected O, but got Unknown
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Expected O, but got Unknown
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Expected O, but got Unknown
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Expected O, but got Unknown
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_09e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f1: Expected O, but got Unknown
		//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Expected O, but got Unknown
		//IL_0a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1f: Expected O, but got Unknown
		//IL_0a2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a36: Expected O, but got Unknown
		//IL_1a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a6e: Expected O, but got Unknown
		//IL_1a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a85: Expected O, but got Unknown
		//IL_3bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3be0: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance31 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.PaymentByInvoiceAccount));
		GridEXLayout Gridhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.txtgrandtotal = new System.Windows.Forms.TextBox();
		this.txtdifference = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.CmbTaxType = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.lblWhtAmount = new System.Windows.Forms.Label();
		this.lblTaxPercent = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtpdcCheqAmount = new System.Windows.Forms.TextBox();
		this.lblAmount = new System.Windows.Forms.Label();
		this.lblCustomer = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.CmbCheqNo = new UltraCombo();
		this.txtCustomerBalance = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbSupplierName = new UltraCombo();
		this.txtbalance = new System.Windows.Forms.TextBox();
		this.CmbTranType = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.btnAdd = new System.Windows.Forms.Button();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
		this.label12 = new System.Windows.Forms.Label();
		this.datcheqdate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.datPaymentDate = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtpaytitle = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbCreditAc = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.Gridhistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbStatus = new System.Windows.Forms.ComboBox();
		this.btnsearchhistory = new System.Windows.Forms.Button();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label28 = new System.Windows.Forms.Label();
		this.txtdatetohistory = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.DueDateTo = new System.Windows.Forms.DateTimePicker();
		this.DueDateFrom = new System.Windows.Forms.DateTimePicker();
		this.BillDateTo = new System.Windows.Forms.DateTimePicker();
		this.BillDateFrom = new System.Windows.Forms.DateTimePicker();
		this.txtInvoiceNoTo = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.txtInvoiceNoFrom = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.CmbSupplierCustomer = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCheqNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTranType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAc).BeginInit();
		this.toolStrip3.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Gridhistory).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1341, 679);
		this.panel1.TabIndex = 2;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1341, 679);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.ctrlGrdBar3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.toolStrip3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1333, 651);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Payment By Invoice";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 244);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1327, 334);
		this.panel4.TabIndex = 57;
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1327, 334);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 55;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellValueChanged += new ColumnActionEventHandler(grd_CellValueChanged);
		this.grd.CellEdited += new ColumnActionEventHandler(grd_CellEdited);
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnHeaderClick += new ColumnActionEventHandler(grd_ColumnHeaderClick);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grd).TextChanged += new System.EventHandler(grd_TextChanged);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1281, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grd;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.txtgrandtotal);
		this.panel2.Controls.Add(this.txtdifference);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.label10);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel2.Location = new System.Drawing.Point(3, 578);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1327, 70);
		this.panel2.TabIndex = 56;
		this.txtgrandtotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgrandtotal.Enabled = false;
		this.txtgrandtotal.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtgrandtotal.Location = new System.Drawing.Point(1168, 8);
		this.txtgrandtotal.Name = "txtgrandtotal";
		this.txtgrandtotal.ReadOnly = true;
		this.txtgrandtotal.Size = new System.Drawing.Size(146, 25);
		this.txtgrandtotal.TabIndex = 47;
		this.txtgrandtotal.TabStop = false;
		this.txtdifference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdifference.Enabled = false;
		this.txtdifference.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdifference.Location = new System.Drawing.Point(1169, 39);
		this.txtdifference.Name = "txtdifference";
		this.txtdifference.ReadOnly = true;
		this.txtdifference.Size = new System.Drawing.Size(146, 25);
		this.txtdifference.TabIndex = 49;
		this.txtdifference.TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label5.Location = new System.Drawing.Point(1061, 11);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(76, 17);
		this.label5.TabIndex = 48;
		this.label5.Text = "Grand Total";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label10.Location = new System.Drawing.Point(1062, 42);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 17);
		this.label10.TabIndex = 50;
		this.label10.Text = "Difference";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxType);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add(this.lblWhtAmount);
		this.panel3.Controls.Add(this.lblTaxPercent);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add(this.txtpdcCheqAmount);
		this.panel3.Controls.Add(this.lblAmount);
		this.panel3.Controls.Add(this.lblCustomer);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCheqNo);
		this.panel3.Controls.Add(this.txtCustomerBalance);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierName);
		this.panel3.Controls.Add(this.txtbalance);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTranType);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.btnAdd);
		this.panel3.Controls.Add(this.txtRemarks);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.txtAmount);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.button1);
		this.panel3.Controls.Add(this.checkedListBox1);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add(this.datcheqdate);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.datPaymentDate);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.txtdocno);
		this.panel3.Controls.Add(this.txtpaytitle);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAc);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1327, 214);
		this.panel3.TabIndex = 52;
		this.CmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance42).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Location = new System.Drawing.Point(93, 182);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Name = "CmbTaxType";
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Size = new System.Drawing.Size(146, 24);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TabIndex = 72;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Leave += new System.EventHandler(CmbTaxType_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(9, 185);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(53, 15);
		this.label13.TabIndex = 71;
		this.label13.Text = "TaxType";
		this.lblWhtAmount.AutoSize = true;
		this.lblWhtAmount.Location = new System.Drawing.Point(308, 189);
		this.lblWhtAmount.Name = "lblWhtAmount";
		this.lblWhtAmount.Size = new System.Drawing.Size(37, 14);
		this.lblWhtAmount.TabIndex = 70;
		this.lblWhtAmount.Text = "label9";
		this.lblTaxPercent.AutoSize = true;
		this.lblTaxPercent.Location = new System.Drawing.Point(248, 189);
		this.lblTaxPercent.Name = "lblTaxPercent";
		this.lblTaxPercent.Size = new System.Drawing.Size(37, 14);
		this.lblTaxPercent.TabIndex = 69;
		this.lblTaxPercent.Text = "label9";
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(245, 112);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(87, 14);
		this.label9.TabIndex = 68;
		this.label9.Text = "PdcCheqAmount";
		this.txtpdcCheqAmount.BackColor = System.Drawing.Color.White;
		this.txtpdcCheqAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpdcCheqAmount.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpdcCheqAmount.Location = new System.Drawing.Point(245, 130);
		this.txtpdcCheqAmount.Name = "txtpdcCheqAmount";
		this.txtpdcCheqAmount.ReadOnly = true;
		this.txtpdcCheqAmount.Size = new System.Drawing.Size(107, 22);
		this.txtpdcCheqAmount.TabIndex = 67;
		this.txtpdcCheqAmount.TabStop = false;
		this.lblAmount.AutoSize = true;
		this.lblAmount.Location = new System.Drawing.Point(380, 109);
		this.lblAmount.Name = "lblAmount";
		this.lblAmount.Size = new System.Drawing.Size(37, 14);
		this.lblAmount.TabIndex = 65;
		this.lblAmount.Text = "label9";
		this.lblCustomer.AutoSize = true;
		this.lblCustomer.Location = new System.Drawing.Point(406, 36);
		this.lblCustomer.Name = "lblCustomer";
		this.lblCustomer.Size = new System.Drawing.Size(37, 14);
		this.lblCustomer.TabIndex = 64;
		this.lblCustomer.Text = "label9";
		this.label2.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(345, 9);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(65, 17);
		this.label2.TabIndex = 63;
		this.label2.Text = "Doc No";
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel5.Location = new System.Drawing.Point(561, 6);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(761, 170);
		this.panel5.TabIndex = 62;
		this.grdDetail.AllowDelete = (InheritableBoolean)1;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(761, 170);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 56;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellEdited += new ColumnActionEventHandler(grdDetail_CellEdited);
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		((System.Windows.Forms.Control)(object)this.grdDetail).TextChanged += new System.EventHandler(grdDetail_TextChanged);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.CmbCheqNo.AllowNull = (DefaultableBoolean)1;
		this.CmbCheqNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCheqNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCheqNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCheqNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.CmbCheqNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCheqNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).Location = new System.Drawing.Point(93, 129);
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).Name = "CmbCheqNo";
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).Size = new System.Drawing.Size(146, 25);
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).Leave += new System.EventHandler(CmbCheqNo_Leave);
		this.txtCustomerBalance.BackColor = System.Drawing.SystemColors.InactiveBorder;
		this.txtCustomerBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCustomerBalance.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerBalance.Location = new System.Drawing.Point(448, 31);
		this.txtCustomerBalance.Name = "txtCustomerBalance";
		this.txtCustomerBalance.Size = new System.Drawing.Size(107, 22);
		this.txtCustomerBalance.TabIndex = 60;
		this.txtCustomerBalance.TabStop = false;
		this.label7.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(9, 34);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(76, 17);
		this.label7.TabIndex = 59;
		this.label7.Text = "Supplier";
		this.CmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Location = new System.Drawing.Point(93, 29);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Name = "CmbSupplierName";
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Size = new System.Drawing.Size(309, 25);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).TabIndex = 2;
		this.CmbSupplierName.ValueChanged += new System.EventHandler(CmbSupplierName_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Leave += new System.EventHandler(cmbsupcust_Leave);
		this.txtbalance.BackColor = System.Drawing.SystemColors.InactiveBorder;
		this.txtbalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbalance.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbalance.Location = new System.Drawing.Point(423, 105);
		this.txtbalance.Name = "txtbalance";
		this.txtbalance.Size = new System.Drawing.Size(132, 22);
		this.txtbalance.TabIndex = 57;
		this.txtbalance.TabStop = false;
		this.CmbTranType.AllowNull = (DefaultableBoolean)1;
		this.CmbTranType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTranType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTranType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbTranType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTranType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTranType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTranType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.CmbTranType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTranType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbTranType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTranType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbTranType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTranType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTranType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTranType).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTranType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTranType).Location = new System.Drawing.Point(93, 55);
		((System.Windows.Forms.Control)(object)this.CmbTranType).Name = "CmbTranType";
		((System.Windows.Forms.Control)(object)this.CmbTranType).Size = new System.Drawing.Size(146, 25);
		((System.Windows.Forms.Control)(object)this.CmbTranType).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbTranType).Leave += new System.EventHandler(CmbTranType_Leave);
		this.label3.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(9, 59);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(76, 17);
		this.label3.TabIndex = 56;
		this.label3.Text = "Tran Type";
		this.btnAdd.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.Location = new System.Drawing.Point(509, 156);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(46, 23);
		this.btnAdd.TabIndex = 10;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = true;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(93, 82);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(462, 22);
		this.txtRemarks.TabIndex = 5;
		this.label1.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(9, 85);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(76, 17);
		this.label1.TabIndex = 53;
		this.label1.Text = "Remarks GL";
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(93, 105);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(146, 22);
		this.txtAmount.TabIndex = 6;
		this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.label23.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(9, 109);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(76, 17);
		this.label23.TabIndex = 51;
		this.label23.Text = "Amount";
		this.button1.Location = new System.Drawing.Point(1588, 11);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 48;
		this.button1.Text = "button1";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Visible = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.checkedListBox1.FormattingEnabled = true;
		this.checkedListBox1.Location = new System.Drawing.Point(1345, 11);
		this.checkedListBox1.Name = "checkedListBox1";
		this.checkedListBox1.Size = new System.Drawing.Size(220, 140);
		this.checkedListBox1.TabIndex = 47;
		this.checkedListBox1.Visible = false;
		this.label12.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(358, 132);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(65, 17);
		this.label12.TabIndex = 25;
		this.label12.Text = "Cheq Date";
		this.datcheqdate.CustomFormat = "dd-MMM-yy";
		this.datcheqdate.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.datcheqdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datcheqdate.Location = new System.Drawing.Point(423, 129);
		this.datcheqdate.Name = "datcheqdate";
		this.datcheqdate.Size = new System.Drawing.Size(132, 22);
		this.datcheqdate.TabIndex = 8;
		this.label11.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(9, 10);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(76, 17);
		this.label11.TabIndex = 23;
		this.label11.Text = "Payment Date";
		this.datPaymentDate.CustomFormat = "dd-MMM-yy";
		this.datPaymentDate.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.datPaymentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datPaymentDate.Location = new System.Drawing.Point(93, 6);
		this.datPaymentDate.Name = "datPaymentDate";
		this.datPaymentDate.Size = new System.Drawing.Size(135, 22);
		this.datPaymentDate.TabIndex = 0;
		this.label8.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(9, 132);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(76, 17);
		this.label8.TabIndex = 20;
		this.label8.Text = "Cheq Ref No";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(448, 8);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(107, 22);
		this.txtdocno.TabIndex = 1;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_Leave);
		this.txtpaytitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpaytitle.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpaytitle.Location = new System.Drawing.Point(93, 156);
		this.txtpaytitle.Name = "txtpaytitle";
		this.txtpaytitle.Size = new System.Drawing.Size(414, 22);
		this.txtpaytitle.TabIndex = 9;
		this.label6.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(9, 158);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(76, 17);
		this.label6.TabIndex = 16;
		this.label6.Text = "Pay Title";
		this.CmbCreditAc.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance14;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance19).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Location = new System.Drawing.Point(326, 55);
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Name = "CmbCreditAc";
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Size = new System.Drawing.Size(229, 25);
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Leave += new System.EventHandler(cmbcashbankac_Leave);
		this.label4.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(245, 59);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(75, 17);
		this.label4.TabIndex = 13;
		this.label4.Text = "Account (Cr)";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.toolStripLabel1, this.btnNew, this.btnsave, this.btnUpdate, this.print, this.btnRefresh, this.btnDelete, this.btnAttachment });
		this.toolStrip3.Location = new System.Drawing.Point(3, 3);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1327, 27);
		this.toolStrip3.TabIndex = 54;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripLabel1.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(203, 24);
		this.toolStripLabel1.Text = "Payment By Bills / Invoices          ";
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.print.Image = (System.Drawing.Image)resources.GetObject("print.Image");
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(56, 24);
		this.print.Text = "&Print";
		this.print.Click += new System.EventHandler(print_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(64, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(99, 24);
		this.btnAttachment.Text = "Attachments";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1333, 651);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.panel6);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1327, 645);
		this.panel7.TabIndex = 3;
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.Gridhistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 78);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1327, 567);
		this.panel8.TabIndex = 26;
		this.Gridhistory.AllowDelete = (InheritableBoolean)1;
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
		((System.Windows.Forms.Control)(object)this.Gridhistory).Size = new System.Drawing.Size(1327, 567);
		((System.Windows.Forms.Control)(object)this.Gridhistory).TabIndex = 56;
		this.Gridhistory.TotalRow = (InheritableBoolean)1;
		this.Gridhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.Gridhistory).VisualStyle = (VisualStyle)4;
		this.Gridhistory.LinkClicked += new ColumnActionEventHandler(Gridhistory_LinkClicked);
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.label29);
		this.panel6.Controls.Add(this.CmbStatus);
		this.panel6.Controls.Add(this.btnsearchhistory);
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label28);
		this.panel6.Controls.Add(this.txtdatetohistory);
		this.panel6.Controls.Add(this.label27);
		this.panel6.Controls.Add(this.label18);
		this.panel6.Controls.Add(this.DueDateTo);
		this.panel6.Controls.Add(this.DueDateFrom);
		this.panel6.Controls.Add(this.BillDateTo);
		this.panel6.Controls.Add(this.BillDateFrom);
		this.panel6.Controls.Add(this.txtInvoiceNoTo);
		this.panel6.Controls.Add(this.label21);
		this.panel6.Controls.Add(this.txtInvoiceNoFrom);
		this.panel6.Controls.Add(this.label19);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer);
		this.panel6.Controls.Add(this.label17);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1327, 78);
		this.panel6.TabIndex = 25;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label29.Location = new System.Drawing.Point(928, 46);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(43, 17);
		this.label29.TabIndex = 29;
		this.label29.Text = "Status";
		this.CmbStatus.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.FormattingEnabled = true;
		this.CmbStatus.Items.AddRange(new object[3] { "Pending", "Received", "All" });
		this.CmbStatus.Location = new System.Drawing.Point(1024, 42);
		this.CmbStatus.Name = "CmbStatus";
		this.CmbStatus.Size = new System.Drawing.Size(196, 25);
		this.CmbStatus.TabIndex = 7;
		this.btnsearchhistory.Location = new System.Drawing.Point(1226, 42);
		this.btnsearchhistory.Name = "btnsearchhistory";
		this.btnsearchhistory.Size = new System.Drawing.Size(61, 24);
		this.btnsearchhistory.TabIndex = 8;
		this.btnsearchhistory.Text = "Search";
		this.btnsearchhistory.UseVisualStyleBackColor = true;
		this.btnsearchhistory.Click += new System.EventHandler(btnsearchhistory_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1279, 0);
		this.ctrlGrdBar1.MyGrid = this.Gridhistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 28);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label28.Location = new System.Drawing.Point(621, 46);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(80, 17);
		this.label28.TabIndex = 27;
		this.label28.Text = "Due Date To";
		this.txtdatetohistory.AutoSize = true;
		this.txtdatetohistory.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdatetohistory.Location = new System.Drawing.Point(317, 17);
		this.txtdatetohistory.Name = "txtdatetohistory";
		this.txtdatetohistory.Size = new System.Drawing.Size(73, 17);
		this.txtdatetohistory.TabIndex = 27;
		this.txtdatetohistory.Text = "Bill Date To";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label27.Location = new System.Drawing.Point(309, 46);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(96, 17);
		this.label27.TabIndex = 27;
		this.label27.Text = "Due Date From";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label18.Location = new System.Drawing.Point(10, 17);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(89, 17);
		this.label18.TabIndex = 27;
		this.label18.Text = "Bill Date From";
		this.DueDateTo.CustomFormat = "dd-MMM-yy";
		this.DueDateTo.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.DueDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDateTo.Location = new System.Drawing.Point(724, 42);
		this.DueDateTo.Name = "DueDateTo";
		this.DueDateTo.Size = new System.Drawing.Size(196, 25);
		this.DueDateTo.TabIndex = 6;
		this.DueDateFrom.CustomFormat = "dd-MMM-yy";
		this.DueDateFrom.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.DueDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDateFrom.Location = new System.Drawing.Point(409, 42);
		this.DueDateFrom.Name = "DueDateFrom";
		this.DueDateFrom.Size = new System.Drawing.Size(196, 25);
		this.DueDateFrom.TabIndex = 5;
		this.BillDateTo.CustomFormat = "dd-MMM-yy";
		this.BillDateTo.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.BillDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.BillDateTo.Location = new System.Drawing.Point(409, 13);
		this.BillDateTo.Name = "BillDateTo";
		this.BillDateTo.Size = new System.Drawing.Size(196, 25);
		this.BillDateTo.TabIndex = 1;
		this.BillDateFrom.CustomFormat = "dd-MMM-yy";
		this.BillDateFrom.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.BillDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.BillDateFrom.Location = new System.Drawing.Point(107, 13);
		this.BillDateFrom.Name = "BillDateFrom";
		this.BillDateFrom.Size = new System.Drawing.Size(196, 25);
		this.BillDateFrom.TabIndex = 0;
		this.txtInvoiceNoTo.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtInvoiceNoTo.Location = new System.Drawing.Point(1024, 13);
		this.txtInvoiceNoTo.Name = "txtInvoiceNoTo";
		this.txtInvoiceNoTo.Size = new System.Drawing.Size(196, 25);
		this.txtInvoiceNoTo.TabIndex = 3;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label21.Location = new System.Drawing.Point(928, 17);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(88, 17);
		this.label21.TabIndex = 26;
		this.label21.Text = "Invoice No To";
		this.txtInvoiceNoFrom.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtInvoiceNoFrom.Location = new System.Drawing.Point(724, 13);
		this.txtInvoiceNoFrom.Name = "txtInvoiceNoFrom";
		this.txtInvoiceNoFrom.Size = new System.Drawing.Size(196, 25);
		this.txtInvoiceNoFrom.TabIndex = 2;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label19.Location = new System.Drawing.Point(611, 17);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(104, 17);
		this.label19.TabIndex = 26;
		this.label19.Text = "Invoice No From";
		this.CmbSupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Location = new System.Drawing.Point(107, 42);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Name = "CmbSupplierCustomer";
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Size = new System.Drawing.Size(196, 24);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).TabIndex = 4;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label17.Location = new System.Drawing.Point(10, 46);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(64, 17);
		this.label17.TabIndex = 13;
		this.label17.Text = "Customer";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1341, 679);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PaymentByInvoiceAccount";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ReceiptByInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ReceiptByInvoiceAccount_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCheqNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTranType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAc).EndInit();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.Gridhistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).EndInit();
		base.ResumeLayout(false);
	}
}
