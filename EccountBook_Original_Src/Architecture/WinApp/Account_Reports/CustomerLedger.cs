using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class CustomerLedger : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Label label33;

	private Label label27;

	private Label label29;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private Panel panel5;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	internal UltraCombo comSupplier;

	private Button btnshow;

	public DateTimePicker txttodate;

	public DateTimePicker txtfromdate;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKey;

	private Label label37;

	internal UltraCombo CmbDateType;

	private Panel PanelAccountDetail;

	private Label label24;

	private Label lblAccountClass;

	private Label lblAccountCode;

	private Label lblAccountType;

	private Label lblOpening;

	private Label OpeningDrCr;

	private Label lblCurrDebit;

	private Label label28;

	private Label lblCurrCredit;

	private Label label32;

	private Label lblClosingBal;

	private Label label26;

	private Label label21;

	private Label lblBalType;

	private Label label22;

	private Label label25;

	private GroupBox GlInfoBox;

	private GroupBox groupBox1;

	public CustomerLedger(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			GlInfoBox.Hide();
			suppliercustomer();
			DateTypeFill();
			CmbDateType.Focus();
			((UltraGridBase)CmbDateType).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DateTypeFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, CmbDateType, "Id", "Parameters", "Date Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void suppliercustomer()
	{
		try
		{
			DataTable dt = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, comSupplier, "Id", "CompanyName", "Party Name", ZeroIndex: false);
			}
			((UltraGridBase)comSupplier).DisplayLayout.Bands[0].Columns[1].Width = 350;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDateType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(CmbDateType.Value) == 1)
			{
				txtfromdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 2)
			{
				txtfromdate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtfromdate.Value = first;
				txttodate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtfromdate.Value = first2;
				txttodate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 5)
			{
				txtfromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
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
			CustomerLedgerFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LedgerDetailByAccountId()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int AccountId = Conversion.ToInt(((UltraDropDownBase)comSupplier).SelectedRow.Cells["GlAccountId"].Value);
			DataTable dt = ChartofAccount.GetCoaDetailByAccountId(new ChartofAccount
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = AccountId
			});
			if (dt.Rows.Count > 0)
			{
				GlInfoBox.Visible = true;
				lblAccountCode.Text = Conversion.ToString(dt.Rows[0]["AccountCode"]);
				lblAccountClass.Text = Conversion.ToString(dt.Rows[0]["ClassName"]);
				lblAccountType.Text = Conversion.ToString(dt.Rows[0]["AccountType"]);
			}
			else
			{
				GlInfoBox.Visible = false;
				lblAccountClass.Text = "";
				lblAccountType.Text = "";
				lblAccountCode.Text = "";
			}
			DataTable Baldt = CommonServices.GetAccountsBalancesOpeningCurrentAndClosing(AccountId, txtfromdate.Value, txttodate.Value);
			if (Baldt.Rows.Count > 0)
			{
				lblBalType.Visible = true;
				double Opening = Conversion.ToDouble(Baldt.Rows[0]["OpeningBalance"]);
				double CurrDebit = Conversion.ToDouble(Baldt.Rows[0]["DebitAmount"]);
				double CurrCredit = Conversion.ToDouble(Baldt.Rows[0]["CreditAmount"]);
				double Closing = Conversion.ToDouble(Baldt.Rows[0]["ClosingBalance"]);
				if (Closing > 0.0)
				{
					lblBalType.Text = "Dr";
				}
				else if (Closing < 0.0)
				{
					lblBalType.Text = "Cr";
				}
				else if (Closing == 0.0)
				{
					lblBalType.Text = "Nill";
				}
				if (Opening > 0.0)
				{
					OpeningDrCr.Text = "Dr";
				}
				else if (Opening < 0.0)
				{
					OpeningDrCr.Text = "Cr";
				}
				else if (Opening == 0.0)
				{
					OpeningDrCr.Text = "Nill";
				}
				if (Opening > 0.0 || CurrDebit > 0.0 || CurrCredit > 0.0 || Closing > 0.0)
				{
					GlInfoBox.Visible = true;
				}
				lblOpening.Text = Opening.ToString("#,#;(#,#);0");
				lblCurrDebit.Text = CurrDebit.ToString("#,#;(#,#);0");
				lblCurrCredit.Text = CurrCredit.ToString("#,#;(#,#);0");
				lblClosingBal.Text = Closing.ToString("#,#;(#,#);0");
			}
			else
			{
				GlInfoBox.Visible = false;
				lblOpening.Text = "0";
				lblCurrDebit.Text = "0";
				lblCurrCredit.Text = "0";
				lblClosingBal.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerLedgerFill()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(comSupplier.Value) <= 0)
			{
				MessageBox.Show("Party Name required. Please Check!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				comSupplier.Focus();
				return;
			}
			LedgerDetailByAccountId();
			dtGrid.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = txtfromdate.Value;
			obj.ToDate = txttodate.Value;
			obj.SupplierCustomerId = Conversion.ToInt(comSupplier.Value);
			dtGrid = InventoryStockEvalautionDetail.GeneralLedgerSupplierCustomerQuantative(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("DocumentTypeCode");
				dt.Columns.Add("V-Date", typeof(DateTime));
				dt.Columns.Add("V-No");
				dt.Columns.Add("RefDocNoId");
				dt.Columns.Add("RefDocumentTypeId");
				dt.Columns.Add("Description");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("OrderNo");
				dt.Columns.Add("RateCut", typeof(double));
				dt.Columns.Add("NetRate", typeof(double));
				dt.Columns.Add("DebitAmt", typeof(double));
				dt.Columns.Add("CreditAmt", typeof(double));
				dt.Columns.Add("RunningBalance", typeof(double));
				dt.Columns.Add("BalType", typeof(string));
				dt.Columns.Add("ManualBillNo");
				dt.Columns.Add("GpNo");
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Rows.Clear();
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					string BalStatus = "";
					if (Conversion.ToDouble(dtGrid.Rows[i]["RunningBalance"]) > 0.0)
					{
						BalStatus = "Dr";
					}
					else if (Conversion.ToDouble(dtGrid.Rows[i]["RunningBalance"]) < 0.0)
					{
						BalStatus = "Cr";
					}
					else if (Conversion.ToDouble(dtGrid.Rows[i]["RunningBalance"]) == 0.0)
					{
						BalStatus = "Nill";
					}
					dt.Rows.Add(dtGrid.Rows[i]["DocumentTypeCode"], Conversion.ToDateTime(dtGrid.Rows[i]["DocDate"]).ToShortDateString(), dtGrid.Rows[i]["DocNo"], dtGrid.Rows[i]["RefDocNoId"], dtGrid.Rows[i]["RefDocumentTypeId"], dtGrid.Rows[i]["ItemName"], dtGrid.Rows[i]["VehicleNo"], dtGrid.Rows[i]["QtyIn"], dtGrid.Rows[i]["WeightIn"], dtGrid.Rows[i]["OrderNo"], dtGrid.Rows[i]["RateCut"], dtGrid.Rows[i]["NetRate"], dtGrid.Rows[i]["DebitAmt"], dtGrid.Rows[i]["CreditAmt"], dtGrid.Rows[i]["RunningBalance"], BalStatus, dtGrid.Rows[i]["MnBillNo"], dtGrid.Rows[i]["GpNo"], dtGrid.Rows[i]["GpDate"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdCustomerLedgerSettings();
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

	private void grdCustomerLedgerSettings()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.AllowDelete = (InheritableBoolean)2;
			grd.AllowEdit = (InheritableBoolean)2;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["DocumentTypeCode"].Width = 55;
			grd.RootTable.Columns["V-Date"].Width = 70;
			grd.RootTable.Columns["V-No"].Width = 60;
			grd.RootTable.Columns["Description"].Width = 200;
			grd.RootTable.Columns["VehicleNo"].Width = 75;
			grd.RootTable.Columns["Qty"].Width = 75;
			grd.RootTable.Columns["Weight"].Width = 75;
			grd.RootTable.Columns["OrderNo"].Width = 65;
			grd.RootTable.Columns["RateCut"].Width = 65;
			grd.RootTable.Columns["NetRate"].Width = 65;
			grd.RootTable.Columns["DebitAmt"].Width = 100;
			grd.RootTable.Columns["CreditAmt"].Width = 100;
			grd.RootTable.Columns["RunningBalance"].Width = 100;
			grd.RootTable.Columns["BalType"].Width = 55;
			grd.RootTable.Columns["ManualBillNo"].Width = 70;
			grd.RootTable.Columns["GpNo"].Width = 60;
			grd.RootTable.Columns["GpDate"].Width = 75;
			grd.RootTable.Columns["RefDocNoId"].Visible = false;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeCode"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["DocumentTypeCode"].Caption = "V-Type";
			grd.RootTable.Columns["DebitAmt"].Caption = "Debit Amount";
			grd.RootTable.Columns["CreditAmt"].Caption = "Credit Amount";
			grd.RootTable.Columns["DebitAmt"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DebitAmt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DebitAmt"].FormatString = "#,#";
			grd.RootTable.Columns["DebitAmt"].TotalFormatString = "#,#";
			grd.RootTable.Columns["CreditAmt"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CreditAmt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CreditAmt"].FormatString = "#,#";
			grd.RootTable.Columns["CreditAmt"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,#";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].FormatString = "#,#";
			grd.RootTable.Columns["Weight"].TotalFormatString = "#,#";
			grd.RootTable.Columns["RunningBalance"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RunningBalance"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetRate"].FormatString = "#,#";
			grd.RootTable.Columns["Ratecut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCut"].FormatString = "#,#";
			grd.RootTable.Columns["OrderNo"].TextAlignment = (TextAlignment)3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grd.CurrentRow;
			int DocumentTypeId = Conversion.ToInt((object)currentRow.Cells["RefDocumentTypeId"].Value.ToString());
			int DocumentTypeIdSrNo = int.Parse(currentRow.Cells["RefDocNoId"].Value.ToString());
			if (e.Column.Key == "DocumentTypeCode")
			{
				CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeIdSrNo, DocumentTypeIdSrNo);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reset()
	{
		try
		{
			((UltraGridBase)CmbDateType).Rows[1].Activate();
			CmbDateType.Focus();
			((Control)(object)comSupplier).Text = string.Empty;
			GlInfoBox.Hide();
			grd.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			suppliercustomer();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton17_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void CustomerLedger_FormClosing(object sender, FormClosingEventArgs e)
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

	private void print_Click_1(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid.Rows.Count == 0 || dtGrid == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtGrid, "130-RptAcSupplierCustomerQuantativeGL.rpt");
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

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.P)
		{
			print_Click_1(null, null);
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
			btnshow_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.Down)
		{
			((Control)(object)grd).Focus();
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			CmbDateType.Focus();
		}
		if (e.Control && e.Alt)
		{
			MakeShortCutKeys();
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
			dt.Rows.Add("Ctrl+N", "For Reset");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+S", "For Show Data");
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grids");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn != null)
			{
				int DocumentTypeId = Conversion.ToInt((object)item.Cells["RefDocumentTypeId"].Value.ToString());
				int DocumentTypeIdSrNo = int.Parse(item.Cells["RefDocNoId"].Value.ToString());
				if (grd.CurrentColumn.Key == "DocumentTypeCode")
				{
					CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeIdSrNo, DocumentTypeIdSrNo);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comSupplier_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(comSupplier.Value) > 0)
			{
				LedgerDetailByAccountId();
			}
			else
			{
				GlInfoBox.Hide();
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
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.CustomerLedger));
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label37 = new System.Windows.Forms.Label();
		this.CmbDateType = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.comSupplier = new UltraCombo();
		this.txttodate = new System.Windows.Forms.DateTimePicker();
		this.label29 = new System.Windows.Forms.Label();
		this.txtfromdate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.PanelAccountDetail = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.lblAccountClass = new System.Windows.Forms.Label();
		this.lblAccountCode = new System.Windows.Forms.Label();
		this.lblAccountType = new System.Windows.Forms.Label();
		this.lblOpening = new System.Windows.Forms.Label();
		this.OpeningDrCr = new System.Windows.Forms.Label();
		this.lblCurrDebit = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.lblCurrCredit = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.lblClosingBal = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.lblBalType = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.GlInfoBox = new System.Windows.Forms.GroupBox();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comSupplier).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.PanelAccountDetail.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.GlInfoBox.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1047, 30);
		this.panel3.TabIndex = 4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1006, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1047, 567);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 44;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(119, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Party  Ledger";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1047, 666);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 99);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1047, 567);
		this.panel6.TabIndex = 108;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.GlInfoBox);
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1047, 99);
		this.panel5.TabIndex = 107;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(5, 18);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(60, 15);
		this.label37.TabIndex = 146;
		this.label37.Text = "Date Type";
		this.CmbDateType.AllowNull = (DefaultableBoolean)1;
		this.CmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateType).Location = new System.Drawing.Point(80, 12);
		((System.Windows.Forms.Control)(object)this.CmbDateType).Name = "CmbDateType";
		((System.Windows.Forms.Control)(object)this.CmbDateType).Size = new System.Drawing.Size(283, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateType).TabIndex = 0;
		this.CmbDateType.ValueChanged += new System.EventHandler(CmbDateType_ValueChanged);
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(367, 68);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 26);
		this.btnshow.TabIndex = 4;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.comSupplier.AllowNull = (DefaultableBoolean)1;
		this.comSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.comSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.comSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.comSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comSupplier).Location = new System.Drawing.Point(80, 67);
		((System.Windows.Forms.Control)(object)this.comSupplier).Name = "comSupplier";
		((System.Windows.Forms.Control)(object)this.comSupplier).Size = new System.Drawing.Size(283, 26);
		((System.Windows.Forms.Control)(object)this.comSupplier).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.comSupplier).Leave += new System.EventHandler(comSupplier_Leave);
		this.txttodate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttodate.CustomFormat = "dd-MMM-yy";
		this.txttodate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttodate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txttodate.Location = new System.Drawing.Point(247, 41);
		this.txttodate.Name = "txttodate";
		this.txttodate.Size = new System.Drawing.Size(116, 23);
		this.txttodate.TabIndex = 2;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(5, 73);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(69, 15);
		this.label29.TabIndex = 106;
		this.label29.Text = "Party Name";
		this.txtfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfromdate.CustomFormat = "dd-MMM-yy";
		this.txtfromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtfromdate.Location = new System.Drawing.Point(80, 41);
		this.txtfromdate.Name = "txtfromdate";
		this.txtfromdate.Size = new System.Drawing.Size(111, 23);
		this.txtfromdate.TabIndex = 1;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(195, 45);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "To Date";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(5, 45);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From Date";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1047, 723);
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
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.print, this.btnShortCutKey });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1047, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(92, 24);
		this.print.Text = "130-Print";
		this.print.Click += new System.EventHandler(print_Click_1);
		this.btnShortCutKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKey.Image");
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.PanelAccountDetail.Controls.Add(this.label24);
		this.PanelAccountDetail.Controls.Add(this.lblAccountClass);
		this.PanelAccountDetail.Controls.Add(this.lblAccountCode);
		this.PanelAccountDetail.Controls.Add(this.lblAccountType);
		this.PanelAccountDetail.Controls.Add(this.lblOpening);
		this.PanelAccountDetail.Controls.Add(this.OpeningDrCr);
		this.PanelAccountDetail.Controls.Add(this.lblCurrDebit);
		this.PanelAccountDetail.Controls.Add(this.label28);
		this.PanelAccountDetail.Controls.Add(this.lblCurrCredit);
		this.PanelAccountDetail.Controls.Add(this.label32);
		this.PanelAccountDetail.Controls.Add(this.lblClosingBal);
		this.PanelAccountDetail.Controls.Add(this.label26);
		this.PanelAccountDetail.Controls.Add(this.label21);
		this.PanelAccountDetail.Controls.Add(this.lblBalType);
		this.PanelAccountDetail.Controls.Add(this.label22);
		this.PanelAccountDetail.Controls.Add(this.label25);
		this.PanelAccountDetail.Location = new System.Drawing.Point(4, 16);
		this.PanelAccountDetail.Name = "PanelAccountDetail";
		this.PanelAccountDetail.Size = new System.Drawing.Size(473, 80);
		this.PanelAccountDetail.TabIndex = 108;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(237, 37);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(71, 15);
		this.label24.TabIndex = 6;
		this.label24.Text = "Total Credit";
		this.lblAccountClass.AutoSize = true;
		this.lblAccountClass.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAccountClass.Location = new System.Drawing.Point(92, 29);
		this.lblAccountClass.Name = "lblAccountClass";
		this.lblAccountClass.Size = new System.Drawing.Size(82, 15);
		this.lblAccountClass.TabIndex = 2;
		this.lblAccountClass.Text = "Account Class";
		this.lblAccountCode.AutoSize = true;
		this.lblAccountCode.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAccountCode.Location = new System.Drawing.Point(92, 11);
		this.lblAccountCode.Name = "lblAccountCode";
		this.lblAccountCode.Size = new System.Drawing.Size(83, 15);
		this.lblAccountCode.TabIndex = 2;
		this.lblAccountCode.Text = "Account Code";
		this.lblAccountType.AutoSize = true;
		this.lblAccountType.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAccountType.Location = new System.Drawing.Point(92, 47);
		this.lblAccountType.Name = "lblAccountType";
		this.lblAccountType.Size = new System.Drawing.Size(79, 15);
		this.lblAccountType.TabIndex = 3;
		this.lblAccountType.Text = "Account Type";
		this.lblOpening.AutoSize = true;
		this.lblOpening.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOpening.Location = new System.Drawing.Point(325, 1);
		this.lblOpening.Name = "lblOpening";
		this.lblOpening.Size = new System.Drawing.Size(82, 15);
		this.lblOpening.TabIndex = 4;
		this.lblOpening.Text = "Account Class";
		this.OpeningDrCr.AutoSize = true;
		this.OpeningDrCr.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OpeningDrCr.Location = new System.Drawing.Point(427, 1);
		this.OpeningDrCr.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.OpeningDrCr.Name = "OpeningDrCr";
		this.OpeningDrCr.Size = new System.Drawing.Size(40, 15);
		this.OpeningDrCr.TabIndex = 11;
		this.OpeningDrCr.Text = "label3";
		this.lblCurrDebit.AutoSize = true;
		this.lblCurrDebit.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCurrDebit.Location = new System.Drawing.Point(325, 19);
		this.lblCurrDebit.Name = "lblCurrDebit";
		this.lblCurrDebit.Size = new System.Drawing.Size(82, 15);
		this.lblCurrDebit.TabIndex = 5;
		this.lblCurrDebit.Text = "Account Class";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(3, 47);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(82, 15);
		this.label28.TabIndex = 8;
		this.label28.Text = "Account Type";
		this.lblCurrCredit.AutoSize = true;
		this.lblCurrCredit.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCurrCredit.Location = new System.Drawing.Point(325, 37);
		this.lblCurrCredit.Name = "lblCurrCredit";
		this.lblCurrCredit.Size = new System.Drawing.Size(82, 15);
		this.lblCurrCredit.TabIndex = 5;
		this.lblCurrCredit.Text = "Account Class";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(3, 11);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(84, 15);
		this.label32.TabIndex = 7;
		this.label32.Text = "Account Code";
		this.lblClosingBal.AutoSize = true;
		this.lblClosingBal.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblClosingBal.Location = new System.Drawing.Point(325, 54);
		this.lblClosingBal.Name = "lblClosingBal";
		this.lblClosingBal.Size = new System.Drawing.Size(80, 15);
		this.lblClosingBal.TabIndex = 5;
		this.lblClosingBal.Text = "Account Class";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(3, 29);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(82, 15);
		this.label26.TabIndex = 7;
		this.label26.Text = "Account Class";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(237, 1);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(54, 15);
		this.label21.TabIndex = 6;
		this.label21.Text = "Opening";
		this.lblBalType.AutoSize = true;
		this.lblBalType.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalType.Location = new System.Drawing.Point(429, 54);
		this.lblBalType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lblBalType.Name = "lblBalType";
		this.lblBalType.Size = new System.Drawing.Size(40, 15);
		this.lblBalType.TabIndex = 10;
		this.lblBalType.Text = "label3";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(237, 19);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(68, 15);
		this.label22.TabIndex = 6;
		this.label22.Text = "Total Debit";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(237, 54);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(46, 15);
		this.label25.TabIndex = 6;
		this.label25.Text = "Closing";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comSupplier);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add(this.txtfromdate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateType);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.txttodate);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(427, 99);
		this.groupBox1.TabIndex = 148;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.GlInfoBox.Controls.Add(this.PanelAccountDetail);
		this.GlInfoBox.Dock = System.Windows.Forms.DockStyle.Left;
		this.GlInfoBox.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GlInfoBox.Location = new System.Drawing.Point(427, 0);
		this.GlInfoBox.Name = "GlInfoBox";
		this.GlInfoBox.Size = new System.Drawing.Size(483, 99);
		this.GlInfoBox.TabIndex = 149;
		this.GlInfoBox.TabStop = false;
		this.GlInfoBox.Text = "GL Information";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1047, 723);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "CustomerLedger";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CustomerLedger_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.CmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comSupplier).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.PanelAccountDetail.ResumeLayout(false);
		this.PanelAccountDetail.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.GlInfoBox.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
