using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Account_Definition
{

public class frmAdvanceAdjustment : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtcoalst = new DataTable();

	private bool UpdateMode;

	private DataTable dtGetInvoices = new DataTable();

	private DataTable dtAdvanceAmount = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNew;

	private ToolStripButton btnSave;

	private ToolStripButton BtnPrint;

	private ToolStripButton toolStripButton1;

	private Panel panel4;

	private TextBox txtDocNo;

	private DateTimePicker txtDocDate;

	internal UltraCombo cmbSupplier;

	private Label label6;

	private Label label2;

	private Label label1;

	private Panel panel5;

	private Panel panel6;

	internal GridEX grdInvoices;

	internal GridEX grdAdvancesAdjustment;

	private TextBox txtTaxPercent;

	private Label label21;

	internal UltraCombo CmbWithHoldingAc;

	private Label label12;

	private CheckBox ChkBoxWthHolding;

	private TextBox txtTotalAmount;

	private Label label9;

	private TextBox txtTaxAmount;

	private Label label4;

	private Label label8;

	private TextBox txtValue;

	internal UltraCombo CmbAgainstAc;

	private Label label13;

	private Label label3;

	private TextBox txtAdjustedAmount;

	private Label label5;

	private TextBox txtRemarks;

	public frmAdvanceAdjustment(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmAdvanceAdjustment_Load(object sender, EventArgs e)
	{
		AccountsComboBind();
		SupplierCustomerFill();
		GenerateDocNo();
		cmbSupplier.Focus();
	}

	private void AccountsComboBind()
	{
		try
		{
			dtcoalst = CommonServices.CoaAllocationGetAllServiceBind();
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
				DDL.BindDDL(dt, CmbAgainstAc, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				DDL.BindDDL(dt, CmbWithHoldingAc, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierCustomerFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			DataTable dtSupplier = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dtSupplier.Rows.Count > 0)
			{
				for (int i = 0; i < dtSupplier.Rows.Count; i++)
				{
					dt.Rows.Add(dtSupplier.Rows[i]["GlAccountId"], dtSupplier.Rows[i]["CompanyName"]);
				}
				DDL.BindDDL(dt, cmbSupplier, "Id", "Name", "Supplier", ZeroIndex: false);
				((UltraGridBase)cmbSupplier).DisplayLayout.Bands[0].Columns[1].Width = 248;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			int DocNo = VouchersAdvanceAdjustmentByInvoiceHeader.GenerateCode(new VouchersAdvanceAdjustmentByInvoiceHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (DocNo > 0)
			{
				txtDocNo.Text = Conversion.ToString((object)DocNo);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetInvoiceNoByPaymentByInvoice()
	{
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("RefDocumentTypeId");
			dt.Columns.Add("RefDocNoId");
			dt.Columns.Add("VoucherDate");
			dt.Columns.Add("DocumentType");
			dt.Columns.Add("DueDate");
			dt.Columns.Add("ManualBillNo");
			dt.Columns.Add("BalanceAmount", typeof(double));
			dt.Columns.Add("PaidAmount", typeof(double));
			dt.Columns.Add("Remarks");
			dt.Columns.Add("Select");
			dt.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.AccountId = Conversion.ToInt(cmbSupplier.Value);
			dtGetInvoices = VoucherHead.GetInvoiceNoByPaymentByInvoice(obj);
			if (dtGetInvoices.Rows.Count > 0)
			{
				for (int i = 0; i < dtGetInvoices.Rows.Count; i++)
				{
					dt.Rows.Add(dtGetInvoices.Rows[i]["DocumentTypeId"], dtGetInvoices.Rows[i]["Id"], Conversion.ToDateTime(dtGetInvoices.Rows[i]["VoucherDate"]).ToString("dd-MMM-yyyy"), dtGetInvoices.Rows[i]["DocumentTypeCode"], Conversion.ToDateTime(dtGetInvoices.Rows[i]["DueDate"]).ToString("dd-MMM-yyyy"), dtGetInvoices.Rows[i]["ManualBillNo"], dtGetInvoices.Rows[i]["BillAmount"], 0, "", 0);
				}
				grdInvoices.DataSource = dt;
				grdInvoices.RetrieveStructure();
				grdInvoiceSetting();
			}
			else
			{
				grdAdvancesAdjustment.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAdvanceAmountByAccountId()
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("AccountId");
			dt.Columns.Add("AccountTitle");
			dt.Columns.Add("AvailableAmount", typeof(double));
			dt.Columns.Add("AdjustmentAmount", typeof(double));
			dt.Columns.Add("Remarks");
			dt.Columns.Add("Select");
			VoucherHead obj = new VoucherHead();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.RefAccountId = Conversion.ToInt(cmbSupplier.Value);
			dtAdvanceAmount = VoucherHead.GetAdvanceAmountByAccountId(obj);
			if (dtAdvanceAmount.Rows.Count > 0)
			{
				dt.Rows.Clear();
				for (int i = 0; i < dtAdvanceAmount.Rows.Count; i++)
				{
					dt.Rows.Add(dtAdvanceAmount.Rows[i]["AccountId"], dtAdvanceAmount.Rows[i]["AccountTitle"], dtAdvanceAmount.Rows[i]["AdvanceAmount"], 0, "", 0);
				}
				grdAdvancesAdjustment.DataSource = dt;
				grdAdvancesAdjustment.RetrieveStructure();
				grdAccoutsSetting();
			}
			else
			{
				grdAdvancesAdjustment.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvoiceSetting()
	{
		try
		{
			grdInvoices.RootTable.Columns["BalanceAmount"].EditType = (EditType)0;
			grdInvoices.RootTable.Columns["ManualBillNo"].EditType = (EditType)0;
			grdInvoices.RootTable.Columns["DueDate"].EditType = (EditType)0;
			grdInvoices.RootTable.Columns["DocumentType"].EditType = (EditType)0;
			grdInvoices.RootTable.Columns["VoucherDate"].EditType = (EditType)0;
			grdInvoices.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdInvoices.RootTable.Columns["RefDocNoId"].Visible = false;
			grdInvoices.RootTable.Columns["Remarks"].Width = 250;
			grdInvoices.RootTable.Columns["PaidAmount"].ColumnType = (ColumnType)0;
			grdInvoices.RootTable.Columns["PaidAmount"].EditType = (EditType)1;
			grdInvoices.RootTable.Columns["PaidAmount"].Caption = "Paid Amount";
			grdInvoices.RootTable.Columns["Remarks"].ColumnType = (ColumnType)0;
			grdInvoices.RootTable.Columns["Remarks"].EditType = (EditType)1;
			grdInvoices.RootTable.Columns["Remarks"].Caption = "Remarks";
			grdInvoices.RootTable.Columns["PaidAmount"].AggregateFunction = (AggregateFunction)2;
			grdInvoices.RootTable.Columns["PaidAmount"].TextAlignment = (TextAlignment)3;
			grdInvoices.RootTable.Columns["PaidAmount"].FormatString = "0,0";
			grdInvoices.RootTable.Columns["PaidAmount"].TotalFormatString = "#,##0.##";
			grdInvoices.RootTable.Columns["BalanceAmount"].AggregateFunction = (AggregateFunction)2;
			grdInvoices.RootTable.Columns["BalanceAmount"].TextAlignment = (TextAlignment)3;
			grdInvoices.RootTable.Columns["BalanceAmount"].FormatString = "0,0";
			grdInvoices.RootTable.Columns["BalanceAmount"].TotalFormatString = "#,##0.##";
			grdInvoices.RootTable.Columns["Select"].Key = "Select";
			grdInvoices.RootTable.Columns["Select"].ActAsSelector = true;
			grdInvoices.RootTable.Columns["Select"].UseHeaderSelector = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdAccoutsSetting()
	{
		grdAdvancesAdjustment.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
		grdAdvancesAdjustment.RootTable.Columns["AvailableAmount"].EditType = (EditType)0;
		grdAdvancesAdjustment.RootTable.Columns["AccountId"].Visible = false;
		grdAdvancesAdjustment.RootTable.Columns["AccountTitle"].Width = 200;
		grdAdvancesAdjustment.RootTable.Columns["Remarks"].Width = 250;
		grdAdvancesAdjustment.RootTable.Columns["AvailableAmount"].Width = 150;
		grdAdvancesAdjustment.RootTable.Columns["AdjustmentAmount"].Width = 150;
		grdAdvancesAdjustment.RootTable.Columns["AdjustmentAmount"].ColumnType = (ColumnType)0;
		grdAdvancesAdjustment.RootTable.Columns["AdjustmentAmount"].EditType = (EditType)1;
		grdAdvancesAdjustment.RootTable.Columns["AdjustmentAmount"].Caption = "Adjustment Amount";
		grdAdvancesAdjustment.RootTable.Columns["AvailableAmount"].AggregateFunction = (AggregateFunction)2;
		grdAdvancesAdjustment.RootTable.Columns["AvailableAmount"].TextAlignment = (TextAlignment)3;
		grdAdvancesAdjustment.RootTable.Columns["AvailableAmount"].FormatString = "0,0";
		grdAdvancesAdjustment.RootTable.Columns["AvailableAmount"].TotalFormatString = "#,##0.##";
		grdAdvancesAdjustment.RootTable.Columns["AdjustmentAmount"].AggregateFunction = (AggregateFunction)2;
		grdAdvancesAdjustment.RootTable.Columns["AdjustmentAmount"].TextAlignment = (TextAlignment)3;
		grdAdvancesAdjustment.RootTable.Columns["AdjustmentAmount"].FormatString = "0,0";
		grdAdvancesAdjustment.RootTable.Columns["AdjustmentAmount"].TotalFormatString = "#,##0.##";
		grdAdvancesAdjustment.RootTable.Columns["Remarks"].ColumnType = (ColumnType)0;
		grdAdvancesAdjustment.RootTable.Columns["Remarks"].EditType = (EditType)1;
		grdAdvancesAdjustment.RootTable.Columns["Remarks"].Caption = "Remarks";
		grdAdvancesAdjustment.RootTable.Columns["Select"].Key = "Select";
		grdAdvancesAdjustment.RootTable.Columns["Select"].ActAsSelector = true;
		grdAdvancesAdjustment.RootTable.Columns["Select"].UseHeaderSelector = true;
	}

	private void ChkBoxWthHolding_CheckedChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			if (ChkBoxWthHolding.Checked)
			{
				TaxScheduleMain taxSchedule = new TaxScheduleMain();
				taxSchedule.OrganizationId = UserAccount.OrganizationId;
				taxSchedule.CompanyId = UserAccount.CompanyId;
				taxSchedule.EffectedDate = txtDocDate.Value;
				taxSchedule.TaxNameId = 1;
				if (TaxScheduleMain.ReadTaxSchedule(taxSchedule).Rows.Count == 0)
				{
					((Control)(object)CmbAgainstAc).Enabled = false;
					((Control)(object)CmbWithHoldingAc).Enabled = false;
					((Control)(object)CmbWithHoldingAc).Text = string.Empty;
					txtTaxPercent.Text = "0";
					txtTaxAmount.Text = "0";
				}
				else
				{
					DataTable detail = TaxScheduleMain.ReadTaxSchedule(taxSchedule);
					txtTaxPercent.Text = detail.Rows[0]["TaxPercent"].ToString();
					if (!UpdateMode)
					{
						CmbWithHoldingAc.Value = detail.Rows[0]["TaxGLAccountId"];
						((Control)(object)CmbWithHoldingAc).Text = detail.Rows[0]["AccountTitle"].ToString();
					}
					double taxamount = Math.Round(Conversion.ToDouble((object)txtValue.Text) / (100.0 - Conversion.ToDouble((object)txtTaxPercent.Text)) * 100.0 * Conversion.ToDouble((object)txtTaxPercent.Text) / 100.0, 2);
					txtTaxAmount.Text = taxamount.ToString();
					double ValueAmt = Conversion.ToDouble((object)txtValue.Text);
					txtTotalAmount.Text = (taxamount + ValueAmt).ToString();
				}
				((Control)(object)CmbAgainstAc).Enabled = true;
				((Control)(object)CmbWithHoldingAc).Enabled = true;
			}
			else
			{
				((Control)(object)CmbAgainstAc).Enabled = false;
				((Control)(object)CmbWithHoldingAc).Enabled = false;
				((Control)(object)CmbWithHoldingAc).Text = string.Empty;
				txtTaxPercent.Text = "0";
				txtTaxAmount.Text = "0";
				double Amt = Conversion.ToDouble((object)txtValue.Text);
				double Tax = Conversion.ToDouble((object)txtTaxAmount.Text);
				txtTotalAmount.Text = (Amt + Tax).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		try
		{
			if (!validation())
			{
				return;
			}
			string AdjustmentAmount = grdAdvancesAdjustment.GetTotal(grdAdvancesAdjustment.RootTable.Columns["AdjustmentAmount"], (AggregateFunction)2).ToString();
			string AvailableAmount = grdAdvancesAdjustment.GetTotal(grdAdvancesAdjustment.RootTable.Columns["AvailableAmount"], (AggregateFunction)2).ToString();
			string PaidAmount = grdInvoices.GetTotal(grdInvoices.RootTable.Columns["PaidAmount"], (AggregateFunction)2).ToString();
			if (Conversion.ToDouble((object)AdjustmentAmount) > Conversion.ToDouble((object)AvailableAmount))
			{
				MessageBox.Show("Adjustment Amount Should not greater then Available Amount", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			GridEXRow[] dataRows = grdInvoices.GetDataRows();
			foreach (GridEXRow r2 in dataRows)
			{
				if (Conversion.ToInt(r2.Cells["PaidAmount"].Value) > Conversion.ToInt(r2.Cells["BalanceAmount"].Value))
				{
					MessageBox.Show("Please Check Paid Amount! This Should not Greater then Balance Amount", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			if (Conversion.ToDouble((object)AdjustmentAmount) != Conversion.ToDouble((object)PaidAmount))
			{
				MessageBox.Show("Please Check Paid Amount! This Should be Equal To Adjustment Amount", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			VouchersAdvanceAdjustmentByInvoiceHeader obj = new VouchersAdvanceAdjustmentByInvoiceHeader();
			obj.DocDate = txtDocDate.Value;
			obj.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			obj.AdjustedAmount = Conversion.ToDouble((object)txtAdjustedAmount.Text);
			obj.RemarksHeader = txtRemarks.Text;
			obj.GlAccountId = Conversion.ToInt(CmbAgainstAc.Value);
			obj.WhtTaxAccountId = Conversion.ToInt(CmbWithHoldingAc.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplier.Value);
			obj.TaxAmount = Conversion.ToDouble((object)txtTaxAmount.Text);
			obj.TaxPrcnt = Conversion.ToDouble((object)txtTaxPercent.Text);
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.EntryDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.PostDate = DateTime.Now;
			obj.PostState = false;
			obj.PostUser = UserAccount.ID;
			obj.AdjustedInvoicesDetails = new List<VouchersAdvanceAdjustedInvoicesDetail>();
			obj.AdjustedAccountsDetails = new List<VouchersAdvanceAdjustedAccountsDetail>();
			dataRows = grdInvoices.GetRows();
			foreach (GridEXRow r in dataRows)
			{
				if (Conversion.ToDouble(r.Cells["PaidAmount"].Value) > 0.0)
				{
					VouchersAdvanceAdjustedInvoicesDetail obj2 = new VouchersAdvanceAdjustedInvoicesDetail();
					obj2.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					obj2.RefDocNoId = Conversion.ToInt(r.Cells["RefDocNoId"].Value);
					obj2.BalanceAmount = Conversion.ToDouble(r.Cells["BalanceAmount"].Value);
					obj2.PaidAmount = Conversion.ToDouble(r.Cells["PaidAmount"].Value);
					obj2.SubRemarksInvoices = Conversion.ToString(r.Cells["Remarks"].Value);
					obj.AdjustedInvoicesDetails.Add(obj2);
					continue;
				}
				MessageBox.Show("Please Check Paid Amount", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			dataRows = grdAdvancesAdjustment.GetRows();
			foreach (GridEXRow g in dataRows)
			{
				VouchersAdvanceAdjustedAccountsDetail obj3 = new VouchersAdvanceAdjustedAccountsDetail();
				obj3.AccountId = Conversion.ToInt(g.Cells["AccountId"].Value);
				obj3.AvailableBalance = Conversion.ToDouble(g.Cells["AvailableAmount"].Value);
				obj3.AdjustedAmount = Conversion.ToDouble(g.Cells["AdjustmentAmount"].Value);
				obj3.RemarksAdjustAccounts = Conversion.ToString((object)g.Cells["Remarks"].Text);
				obj.AdjustedAccountsDetails.Add(obj3);
			}
			if (VouchersAdvanceAdjustmentByInvoiceHeader.Save(obj) > 0)
			{
				MessageBox.Show("SAVED SECCESSFULLY", "SAVE", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbSupplier_Leave(object sender, EventArgs e)
	{
		try
		{
			GetInvoiceNoByPaymentByInvoice();
			GetAdvanceAmountByAccountId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdAdvancesAdjustment_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (!(e.Column.Key == "Select"))
			{
				return;
			}
			double AvailableAmount = 0.0;
			GridEXRow[] rows = grdAdvancesAdjustment.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToDouble(r.Cells["AvailableAmount"].Value) > 0.0 && Conversion.ToBool(r.Cells["Select"].Value))
				{
					r.BeginEdit();
					AvailableAmount = Conversion.ToDouble(r.Cells["AvailableAmount"].Value);
					r.Cells["AdjustmentAmount"].Value = Conversion.ToDouble((object)AvailableAmount);
					r.EndEdit();
				}
				else
				{
					r.BeginEdit();
					r.Cells["AdjustmentAmount"].Value = 0.ToString();
					r.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdAdvancesAdjustment_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdAdvancesAdjustment.UpdateData();
			string AdjustmentAmount = grdAdvancesAdjustment.GetTotal(grdAdvancesAdjustment.RootTable.Columns["AdjustmentAmount"], (AggregateFunction)2).ToString();
			txtAdjustedAmount.Text = AdjustmentAmount;
			grdAdvancesAdjustment.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdAdvancesAdjustment_CellEdited(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdAdvancesAdjustment.UpdateData();
			string AdjustmentAmount = grdAdvancesAdjustment.GetTotal(grdAdvancesAdjustment.RootTable.Columns["AdjustmentAmount"], (AggregateFunction)2).ToString();
			txtAdjustedAmount.Text = AdjustmentAmount;
			grdAdvancesAdjustment.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvoices_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grdInvoices.UpdateData();
	}

	private void grdInvoices_CellEdited(object sender, ColumnActionEventArgs e)
	{
		grdInvoices.UpdateData();
	}

	private void Reset()
	{
		try
		{
			txtDocDate.Value = DateTime.Now;
			((Control)(object)cmbSupplier).Text = string.Empty;
			txtAdjustedAmount.Text = string.Empty;
			txtAdjustedAmount.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			GenerateDocNo();
			grdAdvancesAdjustment.ClearStructure();
			grdInvoices.ClearStructure();
			cmbSupplier.Focus();
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
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool validation()
	{
		if (txtDocNo.Text == string.Empty || Conversion.ToInt((object)txtDocNo.Text) == 0)
		{
			MessageBox.Show("Doc No Field is Required", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return false;
		}
		if (((Control)(object)cmbSupplier).Text == string.Empty || Conversion.ToInt(cmbSupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return false;
		}
		if (txtAdjustedAmount.Text == string.Empty || Conversion.ToInt((object)txtAdjustedAmount.Text) == 0)
		{
			MessageBox.Show("Adjustment Amount Field is Required", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return false;
		}
		if (grdAdvancesAdjustment.GetRows().Count() == 0)
		{
			MessageBox.Show("Please Check Advance Adjustment Grid", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return false;
		}
		if (grdInvoices.GetRows().Count() == 0)
		{
			MessageBox.Show("Please Check Invoices Grid", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return false;
		}
		return true;
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
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Expected O, but got Unknown
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Expected O, but got Unknown
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Expected O, but got Unknown
		//IL_088a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Expected O, but got Unknown
		GridEXLayout grdInvoices_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdAdvancesAdjustment_DesignTimeLayout = new GridEXLayout();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdInvoices = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdAdvancesAdjustment = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtAdjustedAmount = new System.Windows.Forms.TextBox();
		this.CmbAgainstAc = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbWithHoldingAc = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.ChkBoxWthHolding = new System.Windows.Forms.CheckBox();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.cmbSupplier = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtValue = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvoices).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAdvancesAdjustment).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAgainstAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWithHoldingAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1051, 625);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1051, 625);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1043, 599);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1037, 593);
		this.panel2.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvoices);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 339);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1037, 254);
		this.panel6.TabIndex = 3;
		this.grdInvoices.AllowDelete = (InheritableBoolean)1;
		grdInvoices_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvoices.DesignTimeLayout = grdInvoices_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvoices).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdInvoices.DynamicFiltering = true;
		this.grdInvoices.FilterMode = (FilterMode)2;
		this.grdInvoices.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdInvoices).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvoices.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvoices).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdInvoices).Name = "grdInvoices";
		((ScrollableBase)this.grdInvoices).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvoices).Size = new System.Drawing.Size(1037, 254);
		((System.Windows.Forms.Control)(object)this.grdInvoices).TabIndex = 0;
		this.grdInvoices.TotalRow = (InheritableBoolean)1;
		this.grdInvoices.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvoices).VisualStyle = (VisualStyle)4;
		this.grdInvoices.CellEdited += new ColumnActionEventHandler(grdInvoices_CellEdited);
		this.grdInvoices.CellUpdated += new ColumnActionEventHandler(grdInvoices_CellUpdated);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdAdvancesAdjustment);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 215);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1037, 124);
		this.panel5.TabIndex = 2;
		this.grdAdvancesAdjustment.AllowDelete = (InheritableBoolean)1;
		grdAdvancesAdjustment_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAdvancesAdjustment.DesignTimeLayout = grdAdvancesAdjustment_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAdvancesAdjustment).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdAdvancesAdjustment.DynamicFiltering = true;
		this.grdAdvancesAdjustment.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdAdvancesAdjustment).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAdvancesAdjustment.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAdvancesAdjustment).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdAdvancesAdjustment).Name = "grdAdvancesAdjustment";
		((ScrollableBase)this.grdAdvancesAdjustment).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAdvancesAdjustment).Size = new System.Drawing.Size(1037, 124);
		((System.Windows.Forms.Control)(object)this.grdAdvancesAdjustment).TabIndex = 0;
		this.grdAdvancesAdjustment.TotalRow = (InheritableBoolean)1;
		this.grdAdvancesAdjustment.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAdvancesAdjustment).VisualStyle = (VisualStyle)4;
		this.grdAdvancesAdjustment.CellValueChanged += new ColumnActionEventHandler(grdAdvancesAdjustment_CellValueChanged);
		this.grdAdvancesAdjustment.CellEdited += new ColumnActionEventHandler(grdAdvancesAdjustment_CellEdited);
		this.grdAdvancesAdjustment.CellUpdated += new ColumnActionEventHandler(grdAdvancesAdjustment_CellUpdated);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtAdjustedAmount);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAgainstAc);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.txtTaxPercent);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.txtDocNo);
		this.panel4.Controls.Add(this.ChkBoxWthHolding);
		this.panel4.Controls.Add(this.txtDocDate);
		this.panel4.Controls.Add(this.txtTotalAmount);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplier);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.txtTaxAmount);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtValue);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 28);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1037, 187);
		this.panel4.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(629, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(54, 15);
		this.label5.TabIndex = 51;
		this.label5.Text = "Remarks";
		this.txtRemarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(629, 25);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(254, 23);
		this.txtRemarks.TabIndex = 50;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(516, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(100, 15);
		this.label3.TabIndex = 49;
		this.label3.Text = "Adjusted Amount";
		this.txtAdjustedAmount.Enabled = false;
		this.txtAdjustedAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAdjustedAmount.Location = new System.Drawing.Point(516, 25);
		this.txtAdjustedAmount.Name = "txtAdjustedAmount";
		this.txtAdjustedAmount.Size = new System.Drawing.Size(111, 23);
		this.txtAdjustedAmount.TabIndex = 48;
		this.CmbAgainstAc.AllowNull = (DefaultableBoolean)1;
		this.CmbAgainstAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAgainstAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAgainstAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance25;
		((SpecialBoxBase)((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance31).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAgainstAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Location = new System.Drawing.Point(8, 117);
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Name = "CmbAgainstAc";
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Size = new System.Drawing.Size(232, 24);
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(8, 99);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(50, 15);
		this.label13.TabIndex = 47;
		this.label13.Text = "Account";
		this.label13.Visible = false;
		this.txtTaxPercent.BackColor = System.Drawing.Color.White;
		this.txtTaxPercent.Location = new System.Drawing.Point(441, 115);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.ReadOnly = true;
		this.txtTaxPercent.Size = new System.Drawing.Size(72, 20);
		this.txtTaxPercent.TabIndex = 6;
		this.txtTaxPercent.TabStop = false;
		this.txtTaxPercent.Visible = false;
		this.label21.AutoSize = true;
		this.label21.Location = new System.Drawing.Point(391, 118);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(36, 13);
		this.label21.TabIndex = 5;
		this.label21.Text = "Tax %";
		this.label21.Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(56, 15);
		this.label2.TabIndex = 7;
		this.label2.Text = "Doc Date";
		this.CmbWithHoldingAc.AllowNull = (DefaultableBoolean)1;
		this.CmbWithHoldingAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWithHoldingAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWithHoldingAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWithHoldingAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Location = new System.Drawing.Point(281, 141);
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Name = "CmbWithHoldingAc";
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Size = new System.Drawing.Size(232, 24);
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(162, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(46, 15);
		this.label1.TabIndex = 6;
		this.label1.Text = "Doc No";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(180, 146);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(87, 15);
		this.label12.TabIndex = 43;
		this.label12.Text = "WithHoldingAc";
		this.label12.Visible = false;
		this.txtDocNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(162, 25);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(100, 23);
		this.txtDocNo.TabIndex = 1;
		this.ChkBoxWthHolding.AutoSize = true;
		this.ChkBoxWthHolding.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkBoxWthHolding.Location = new System.Drawing.Point(281, 118);
		this.ChkBoxWthHolding.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBoxWthHolding.Name = "ChkBoxWthHolding";
		this.ChkBoxWthHolding.Size = new System.Drawing.Size(102, 17);
		this.ChkBoxWthHolding.TabIndex = 4;
		this.ChkBoxWthHolding.TabStop = false;
		this.ChkBoxWthHolding.Text = "WithHoldingTax";
		this.ChkBoxWthHolding.UseVisualStyleBackColor = true;
		this.ChkBoxWthHolding.Visible = false;
		this.ChkBoxWthHolding.CheckedChanged += new System.EventHandler(ChkBoxWthHolding_CheckedChanged);
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(8, 25);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(151, 23);
		this.txtDocDate.TabIndex = 0;
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(619, 155);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(142, 21);
		this.txtTotalAmount.TabIndex = 10;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalAmount.Visible = false;
		this.cmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Location = new System.Drawing.Point(265, 25);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Name = "cmbSupplier";
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Size = new System.Drawing.Size(248, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Leave += new System.EventHandler(cmbSupplier_Leave);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(526, 158);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(79, 15);
		this.label9.TabIndex = 42;
		this.label9.Text = "Total Amount";
		this.label9.Visible = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(265, 6);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(53, 15);
		this.label6.TabIndex = 3;
		this.label6.Text = "Supplier";
		this.txtTaxAmount.BackColor = System.Drawing.Color.White;
		this.txtTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(619, 128);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(142, 21);
		this.txtTaxAmount.TabIndex = 9;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxAmount.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(526, 104);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(49, 15);
		this.label4.TabIndex = 38;
		this.label4.Text = "Amount";
		this.label4.Visible = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(526, 131);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(72, 15);
		this.label8.TabIndex = 40;
		this.label8.Text = "Tax Amount";
		this.label8.Visible = false;
		this.txtValue.BackColor = System.Drawing.Color.White;
		this.txtValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtValue.Location = new System.Drawing.Point(619, 101);
		this.txtValue.Name = "txtValue";
		this.txtValue.ReadOnly = true;
		this.txtValue.Size = new System.Drawing.Size(142, 21);
		this.txtValue.TabIndex = 8;
		this.txtValue.TabStop = false;
		this.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtValue.Visible = false;
		this.panel3.Controls.Add(this.toolStrip2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1037, 28);
		this.panel3.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.BtnNew, this.btnSave, this.BtnPrint, this.toolStripButton1 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1037, 27);
		this.toolStrip2.TabIndex = 4;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(55, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(56, 24);
		this.BtnPrint.Text = "&Print";
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(50, 24);
		this.toolStripButton1.Text = "Refresh";
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1043, 424);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1051, 625);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmAdvanceAdjustment";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmAdvanceAdjustment_Load);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvoices).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAdvancesAdjustment).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAgainstAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWithHoldingAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
