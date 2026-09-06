// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.Account_Reports
{

public class FreightVoucherRegister : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable dtRegister = new DataTable();

	private DataTable dtRegisterGrid = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private Panel panel5;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private Panel panel7;

	private Label label1;

	private GroupBox groupBox3;

	internal UltraCombo CmbDebitAcRegister;

	private Label label22;

	internal UltraCombo CmbCreditAcRegister;

	private Label label19;

	private DateTimePicker datFromDate;

	internal UltraCombo CmbSupplierName;

	private Label label20;

	private Label label5;

	private TextBox txtVehicleRegister;

	private Label label10;

	private Label label3;

	private TextBox txtGpNoFrom;

	private Button btnshow;

	private TextBox txtGpNoTo;

	private DateTimePicker datToDate;

	private ToolStripButton btnRefreshRegister;

	private ToolStripButton btnRegister;

	private ToolStripButton btnResetRegister;

	public FreightVoucherRegister(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.E)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnResetRegister_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.R)
		{
			btnRefreshRegister_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.P)
		{
			btnRegister_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.S)
		{
			btnshow_Click(null, null);
		}
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			datFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			datToDate.Value = DateTime.Today;
			datFromDate.Focus();
			HistoryComboBind(InvFreightVoucher.GetDataForDropDownFromFreightVoucher(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton17_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
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
			if (item != null && (int)item.RowType == 0 && Conversion.ToInt(item.Cells["NoOfAttachments"].Value) > 0)
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(item.Cells["Id"].Value), base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetRegister_Click(object sender, EventArgs e)
	{
		try
		{
			txtGpNoFrom.Text = string.Empty;
			txtGpNoTo.Text = string.Empty;
			txtVehicleRegister.Text = string.Empty;
			((Control)(object)CmbSupplierName).Text = string.Empty;
			dtRegister.Rows.Clear();
			grd.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshRegister_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind(InvFreightVoucher.GetDataForDropDownFromFreightVoucher(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dt)
	{
		try
		{
			DataTable dtSupp = new DataTable();
			dtSupp.Columns.Add("Id", typeof(int));
			dtSupp.Columns.Add("Name", typeof(string));
			DataTable dtCrAc = dtSupp.Clone();
			DataTable dtDrAc = dtSupp.Clone();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
				{
					dtSupp.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "CreditAccount")
				{
					dtCrAc.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "DebitAccount")
				{
					dtDrAc.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbSupplierName, dtSupp, "Id", "Name", "Party Name", AllColumns: true, Conversion.ToInt(CmbSupplierName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCreditAcRegister, dtCrAc, "Id", "Name", "Credit Account", AllColumns: true, Conversion.ToInt(CmbCreditAcRegister.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDebitAcRegister, dtDrAc, "Id", "Name", "Debit Account", AllColumns: true, Conversion.ToInt(CmbDebitAcRegister.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRegister_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtRegister == null || dtRegister.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtRegister, "240-FreightVoucherRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		try
		{
			dtRegister.Rows.Clear();
			DataSet ds = InvFreightVoucher.FreightVoucherSlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 28,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = datFromDate.Value,
				ToDate = datToDate.Value,
				GpSrNoF = Conversion.ToInt((object)txtGpNoFrom.Text),
				GpSrNoT = Conversion.ToInt((object)txtGpNoTo.Text),
				VehicleNo = Conversion.ToString((object)txtVehicleRegister.Text),
				SupplierCustomerId = Conversion.ToInt(CmbSupplierName.Value),
				CreditAccountId = Conversion.ToInt(CmbCreditAcRegister.Value),
				DebitAccountId = Conversion.ToInt(CmbDebitAcRegister.Value)
			});
			dtRegister = ds?.Tables[0].Copy();
			if (dtRegister.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtRegister, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				if (dtRegisterGrid.Columns.Count == 0)
				{
					dtRegisterGrid.Columns.Add("Id", typeof(int));
					dtRegisterGrid.Columns.Add("VoucherHeadId", typeof(int));
					dtRegisterGrid.Columns.Add("DocDate", typeof(DateTime));
					dtRegisterGrid.Columns.Add("DocNo", typeof(int));
					dtRegisterGrid.Columns.Add("GpDate", typeof(DateTime));
					dtRegisterGrid.Columns.Add("GpNo", typeof(int));
					dtRegisterGrid.Columns.Add("SupplierName", typeof(string));
					dtRegisterGrid.Columns.Add("VehicleNo", typeof(string));
					dtRegisterGrid.Columns.Add("BiltyNo", typeof(string));
					dtRegisterGrid.Columns.Add("BiltyDate", typeof(DateTime));
					dtRegisterGrid.Columns.Add("DeliveryTerm", typeof(string));
					dtRegisterGrid.Columns.Add("SupplierWeight", typeof(double));
					dtRegisterGrid.Columns.Add("FactoryWeight", typeof(double));
					dtRegisterGrid.Columns.Add("ShortWeight", typeof(double));
					dtRegisterGrid.Columns.Add("AllowShortage", typeof(double));
					dtRegisterGrid.Columns.Add("ShortageApply", typeof(double));
					dtRegisterGrid.Columns.Add("ShortageAmount", typeof(double));
					dtRegisterGrid.Columns.Add("BiltyFreight", typeof(double));
					dtRegisterGrid.Columns.Add("DeductedAmount", typeof(double));
					dtRegisterGrid.Columns.Add("DiscountAmount", typeof(double));
					dtRegisterGrid.Columns.Add("ToleranceWeight", typeof(string));
					dtRegisterGrid.Columns.Add("WbCharges", typeof(double));
					dtRegisterGrid.Columns.Add("AdvanceByParty", typeof(double));
					dtRegisterGrid.Columns.Add("AdvanceByFactory", typeof(double));
					dtRegisterGrid.Columns.Add("TotalFreight", typeof(double));
					dtRegisterGrid.Columns.Add("ChargeToParty", typeof(double));
					dtRegisterGrid.Columns.Add("PaidAmount", typeof(double));
					dtRegisterGrid.Columns.Add("RemainingAmount", typeof(double));
					dtRegisterGrid.Columns.Add("PerMtonFreight", typeof(double));
					dtRegisterGrid.Columns.Add("PerMaundFreight", typeof(double));
					dtRegisterGrid.Columns.Add("CityName", typeof(string));
					dtRegisterGrid.Columns.Add("CashAccountCode", typeof(string));
					dtRegisterGrid.Columns.Add("CashAccountTitle", typeof(string));
					dtRegisterGrid.Columns.Add("DebitAccountCode", typeof(string));
					dtRegisterGrid.Columns.Add("DebitAccountTitle", typeof(string));
					dtRegisterGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtRegisterGrid.Columns.Add("EntryUser", typeof(string));
					dtRegisterGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtRegisterGrid.Columns.Add("ModifyUser", typeof(string));
					dtRegisterGrid.Columns.Add("ApprovedDate", typeof(DateTime));
					dtRegisterGrid.Columns.Add("ApprovalUser", typeof(string));
					dtRegisterGrid.Columns.Add("ApprovalStatus", typeof(string));
					dtRegisterGrid.Columns.Add("NoOfAttachments", typeof(int));
					dtRegisterGrid.Columns.Add("RemarksDetail", typeof(string));
					dtRegisterGrid.Columns.Add("RemarksHeader", typeof(string));
				}
				dtRegisterGrid.Clear();
				for (int i = 0; i < dtRegister.Rows.Count; i++)
				{
					dtRegisterGrid.Rows.Add(dtRegister.Rows[i]["Id"], dtRegister.Rows[i]["VoucherHeadId"], Conversion.ToDateTime(dtRegister.Rows[i]["DocDate"]).ToShortDateString(), dtRegister.Rows[i]["DocNo"], Conversion.ToDateTime(dtRegister.Rows[i]["GatePassDate"]).ToShortDateString(), dtRegister.Rows[i]["GatePassNo"], dtRegister.Rows[i]["SupplierName"], dtRegister.Rows[i]["VehicleNo"], dtRegister.Rows[i]["BiltyNo"], dtRegister.Rows[i]["BiltyDate"], dtRegister.Rows[i]["DeliveryTerm"], dtRegister.Rows[i]["SupplierWeight"], dtRegister.Rows[i]["FactoryWeight"], dtRegister.Rows[i]["ShortWeight"], dtRegister.Rows[i]["AllowShortage"], dtRegister.Rows[i]["ShortageApply"], dtRegister.Rows[i]["ShortageAmount"], dtRegister.Rows[i]["ToleranceWeight"], dtRegister.Rows[i]["BiltyFreight"], dtRegister.Rows[i]["DeductedAmount"], dtRegister.Rows[i]["DiscountAmount"], dtRegister.Rows[i]["ScaleCharges"], dtRegister.Rows[i]["AdvanceByParty"], dtRegister.Rows[i]["AdvanceByFactory"], dtRegister.Rows[i]["TotalFreight"], dtRegister.Rows[i]["ChargeToParty"], dtRegister.Rows[i]["PaidAmount"], dtRegister.Rows[i]["RemainingAmount"], dtRegister.Rows[i]["PerMtonFreight"], Conversion.ToDouble(dtRegister.Rows[i]["BiltyFreight"]) / Conversion.ToDouble(dtRegister.Rows[i]["FactoryWeight"]) * 40.0, dtRegister.Rows[i]["CityName"], dtRegister.Rows[i]["CashAcCode"], dtRegister.Rows[i]["CashAcTitle"], dtRegister.Rows[i]["DebAcCode"], dtRegister.Rows[i]["DebAcTitle"], Conversion.ToDateTime(dtRegister.Rows[i]["EntryDate"]), dtRegister.Rows[i]["EntryUser"], dtRegister.Rows[i]["ModifyDate"], dtRegister.Rows[i]["ModifyUser"], dtRegister.Rows[i]["ApprovedDate"], dtRegister.Rows[i]["ApprovalUser"], dtRegister.Rows[i]["ApprovalStatus"], dtRegister.Rows[i]["NoOfAttachments"], dtRegister.Rows[i]["RemarksDetail"], dtRegister.Rows[i]["RemarksHeader"]);
				}
				grd.DataSource = dtRegisterGrid;
				grd.RetrieveStructure();
				RegisterGridSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RegisterGridSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "VoucherHeadId", "RemainingAmount", "CashAccountCode", "DebitAccountCode" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, hiddenColumns);
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["EntryDate"].Width = 145;
			grd.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grd.RootTable.Columns["ModifyDate"].Width = 145;
			grd.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grd.RootTable.Columns["ApprovedDate"].Width = 145;
			grd.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grd.RootTable.Columns["DocDate"].Width = 80;
			grd.RootTable.Columns["DocNo"].Width = 55;
			grd.RootTable.Columns["GpDate"].Width = 80;
			grd.RootTable.Columns["GpNo"].Width = 55;
			grd.RootTable.Columns["DeliveryTerm"].Width = 65;
			grd.RootTable.Columns["VehicleNo"].Width = 75;
			grd.RootTable.Columns["BiltyNo"].Width = 70;
			grd.RootTable.Columns["SupplierName"].Width = 200;
			grd.RootTable.Columns["SupplierWeight"].Width = 70;
			grd.RootTable.Columns["FactoryWeight"].Width = 70;
			grd.RootTable.Columns["ToleranceWeight"].Width = 70;
			grd.RootTable.Columns["BiltyFreight"].Width = 70;
			grd.RootTable.Columns["PerMtonFreight"].Width = 75;
			grd.RootTable.Columns["WbCharges"].Width = 60;
			grd.RootTable.Columns["AdvanceByParty"].Width = 70;
			grd.RootTable.Columns["AdvanceByFactory"].Width = 70;
			grd.RootTable.Columns["TotalFreight"].Width = 80;
			grd.RootTable.Columns["ShortWeight"].Width = 65;
			grd.RootTable.Columns["AllowShortage"].Width = 100;
			grd.RootTable.Columns["ShortageAmount"].Width = 85;
			grd.RootTable.Columns["ShortageApply"].Width = 65;
			grd.RootTable.Columns["DeductedAmount"].Width = 80;
			grd.RootTable.Columns["DiscountAmount"].Width = 80;
			grd.RootTable.Columns["ChargeToParty"].Width = 80;
			grd.RootTable.Columns["PaidAmount"].Width = 80;
			grd.RootTable.Columns["CashAccountTitle"].Width = 150;
			grd.RootTable.Columns["DebitAccountTitle"].Width = 150;
			grd.RootTable.Columns["RemarksDetail"].Width = 150;
			grd.RootTable.Columns["NoOfAttachments"].Width = 90;
			grd.RootTable.Columns["ToleranceWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ToleranceWeight"].Caption = "Weigh Bridge Tolerance";
			grd.RootTable.Columns["DeductedAmount"].Caption = "Deduction From Freight";
			grd.RootTable.Columns["PaidAmount"].Caption = "Net Payable Amount";
			grd.RootTable.Columns["AdvanceByParty"].Caption = "Advance By Supplier";
			grd.RootTable.Columns["ChargeToParty"].Caption = "Shortage Charge To Supplier";
			grd.RootTable.Columns["AllowShortage"].Caption = "Against Policy Discount / Allowed Weight";
			grd.RootTable.Columns["ShortageApply"].Caption = "Shortage Applied Weight";
			grd.RootTable.Columns["ShortageAmount"].Caption = "Shortage Applied Amount";
			GridEX_Helper.AddButton(grd, "Print", "Print", 50, 0);
			GridEX_Helper.AddButton(grd, "Voucher", "Voucher", 70, 1);
			grd.FrozenColumns = 2;
			((Control)(object)grd).Refresh();
			ctrlGrdBar1_Load(null, null);
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
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Print")
				{
					CommonServices.FreightVoucherSlip241(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Voucher")
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Conversion.ToInt(r.Cells["VoucherHeadId"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			int ID = 0;
			int VoucherHeadId = 0;
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (grd.GetRows().Count() > 0)
			{
				ID = Conversion.ToInt(item.Cells["Id"].Value);
				VoucherHeadId = Conversion.ToInt(item.Cells["VoucherHeadId"].Value);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Print")
				{
					CommonServices.FreightVoucherSlip241(ID);
				}
				else if (grd.CurrentColumn.Key == "Voucher")
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(VoucherHeadId);
				}
				else if (grd.CurrentColumn.Key == "Attachment")
				{
					CommonServices.GetNoofAttachmentsByScreenName(ID, base.Name);
				}
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
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbDebitAcRegister = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbCreditAcRegister = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.CmbSupplierName = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtVehicleRegister = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtGpNoFrom = new System.Windows.Forms.TextBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtGpNoTo = new System.Windows.Forms.TextBox();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetRegister = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshRegister = new System.Windows.Forms.ToolStripButton();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAcRegister).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAcRegister).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1047, 26);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(227, 18);
		this.label23.TabIndex = 1;
		this.label23.Text = "Freight Voucher Register";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 53);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1047, 430);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 97);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1047, 333);
		this.panel6.TabIndex = 108;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		this.grd.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1047, 307);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 44;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1047, 26);
		this.panel7.TabIndex = 5;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(999, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(151, 18);
		this.label1.TabIndex = 1;
		this.label1.Text = "Filtered Records";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1047, 97);
		this.panel5.TabIndex = 107;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAcRegister);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAcRegister);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add(this.datFromDate);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierName);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.label5);
		this.groupBox3.Controls.Add(this.txtVehicleRegister);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.txtGpNoFrom);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.txtGpNoTo);
		this.groupBox3.Controls.Add(this.datToDate);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(0, 0);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(746, 97);
		this.groupBox3.TabIndex = 108;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.CmbDebitAcRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAcRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAcRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAcRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance29).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAcRegister).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAcRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAcRegister).Location = new System.Drawing.Point(437, 66);
		((System.Windows.Forms.Control)(object)this.CmbDebitAcRegister).Name = "CmbDebitAcRegister";
		((System.Windows.Forms.Control)(object)this.CmbDebitAcRegister).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAcRegister).TabIndex = 7;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(347, 72);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(84, 15);
		this.label22.TabIndex = 167;
		this.label22.Text = "Debit Account";
		this.CmbCreditAcRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAcRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAcRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAcRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAcRegister).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAcRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAcRegister).Location = new System.Drawing.Point(437, 39);
		((System.Windows.Forms.Control)(object)this.CmbCreditAcRegister).Name = "CmbCreditAcRegister";
		((System.Windows.Forms.Control)(object)this.CmbCreditAcRegister).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAcRegister).TabIndex = 6;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(347, 45);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(86, 15);
		this.label19.TabIndex = 165;
		this.label19.Text = "Credit Account";
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(109, 14);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(118, 23);
		this.datFromDate.TabIndex = 0;
		this.CmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance9;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Location = new System.Drawing.Point(437, 12);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Name = "CmbSupplierName";
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).TabIndex = 5;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(5, 18);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(91, 15);
		this.label20.TabIndex = 153;
		this.label20.Text = "Date From && To";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(347, 18);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(69, 15);
		this.label5.TabIndex = 163;
		this.label5.Text = "Party Name";
		this.txtVehicleRegister.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleRegister.Location = new System.Drawing.Point(109, 68);
		this.txtVehicleRegister.Name = "txtVehicleRegister";
		this.txtVehicleRegister.Size = new System.Drawing.Size(232, 23);
		this.txtVehicleRegister.TabIndex = 4;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 45);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(100, 15);
		this.label10.TabIndex = 159;
		this.label10.Text = "GP No From && To";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 72);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 161;
		this.label3.Text = "Vehicle No";
		this.txtGpNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNoFrom.Location = new System.Drawing.Point(109, 41);
		this.txtGpNoFrom.Name = "txtGpNoFrom";
		this.txtGpNoFrom.Size = new System.Drawing.Size(118, 23);
		this.txtGpNoFrom.TabIndex = 2;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(689, 67);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 24);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtGpNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNoTo.Location = new System.Drawing.Point(229, 41);
		this.txtGpNoTo.Name = "txtGpNoTo";
		this.txtGpNoTo.Size = new System.Drawing.Size(112, 23);
		this.txtGpNoTo.TabIndex = 3;
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(229, 14);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(112, 23);
		this.datToDate.TabIndex = 1;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1047, 483);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1047, 27);
		this.panel2.TabIndex = 3;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnResetRegister, this.btnRefreshRegister, this.btnRegister });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1047, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnResetRegister.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetRegister.Name = "btnResetRegister";
		this.btnResetRegister.Size = new System.Drawing.Size(68, 24);
		this.btnResetRegister.Text = "Reset";
		this.btnResetRegister.Click += new System.EventHandler(btnResetRegister_Click);
		this.btnRefreshRegister.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshRegister.Name = "btnRefreshRegister";
		this.btnRefreshRegister.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshRegister.Text = "Refresh";
		this.btnRefreshRegister.Click += new System.EventHandler(btnRefreshRegister_Click);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(115, 24);
		this.btnRegister.Text = "240-Register";
		this.btnRegister.Click += new System.EventHandler(btnRegister_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1047, 483);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "FreightVoucherRegister";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAcRegister).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAcRegister).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
