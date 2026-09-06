// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
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

public class CommissionAgentLedger : Form
{
	private DataTable dtCommissionAgent = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private Panel panel5;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdCommissionAgent;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKey;

	private GroupBox groupBox1;

	private RadioButton rdall;

	private RadioButton rdsale;

	private RadioButton rdpurchase;

	private Label lblDrCrCommissionAgent;

	private Label lblbalanceCommissionAgent;

	internal UltraCombo cmbpartyName;

	private Label label20;

	private DateTimePicker txtToDateCommissionAgent;

	private DateTimePicker txtFromDateCommissionAgent;

	private Button btnshow;

	internal UltraCombo cmbCommissionAgent;

	private Label label6;

	private Label label9;

	public CommissionAgentLedger(UserAccount user)
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
			txtFromDateCommissionAgent.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			CommissionAgentFill();
			rdall.Checked = true;
			rdpurchase.Checked = false;
			rdsale.Checked = false;
			lblbalanceCommissionAgent.Visible = false;
			lblDrCrCommissionAgent.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionAgentFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dt = SupplierCustomer.GetSupplierCustomerAndCommissionAgentFromPurchaseAndSales(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ActionId = 1
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtComm = new DataTable();
			dtComm.Columns.Add("Id");
			dtComm.Columns.Add("CompanyName");
			dtComm.Columns.Add("GlAccountId");
			DataTable dtSupp = new DataTable();
			dtSupp.Columns.Add("Id");
			dtSupp.Columns.Add("CompanyName");
			dtSupp.Columns.Add("GlAccountId");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["PartyType"]) == "CommissionAgent")
				{
					dtComm.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["CompanyName"], dt.Rows[i]["GlAccountId"]);
				}
				if (Conversion.ToString(dt.Rows[i]["PartyType"]) == "Supplier" || Conversion.ToString(dt.Rows[i]["PartyType"]) == "Customer")
				{
					dtSupp.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["CompanyName"], dt.Rows[i]["GlAccountId"]);
				}
			}
			DDL.BindDDL(dtComm, cmbCommissionAgent, "Id", "CompanyName", "Commission Agent", ZeroIndex: false);
			DDL.BindDDL(dtSupp, cmbpartyName, "Id", "CompanyName", "Party  Name ", ZeroIndex: false);
			((UltraGridBase)cmbCommissionAgent).DisplayLayout.Bands[0].Columns[1].Width = 300;
			((UltraGridBase)cmbCommissionAgent).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			((UltraGridBase)cmbpartyName).DisplayLayout.Bands[0].Columns[1].Width = 300;
			((UltraGridBase)cmbpartyName).DisplayLayout.Bands[0].Columns[2].Hidden = true;
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
			GridFillCommissionAgent();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFillCommissionAgent()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		lblbalanceCommissionAgent.Visible = true;
		lblDrCrCommissionAgent.Visible = true;
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = txtFromDateCommissionAgent.Value;
			obj.ToDate = txtToDateCommissionAgent.Value;
			obj.CommissionAgentId = Conversion.ToInt(cmbCommissionAgent.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbpartyName.Value);
			if (rdall.Checked)
			{
				obj.ActionId = 0;
			}
			if (rdpurchase.Checked)
			{
				obj.ActionId = 1;
			}
			if (rdsale.Checked)
			{
				obj.ActionId = 2;
			}
			dtCommissionAgent = GeneralReprots.GeneralLedgerCommissionAndBrokery(obj);
			if (dtCommissionAgent.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("CommissionFrom");
				dt.Columns.Add("CommissionAgentId", typeof(int));
				dt.Columns.Add("CommissionAgent", typeof(string));
				dt.Columns.Add("DocumentType");
				dt.Columns.Add("InvoiceDate", typeof(DateTime));
				dt.Columns.Add("InvoiceNo", typeof(int));
				dt.Columns.Add("OrderNo", typeof(int));
				dt.Columns.Add("PartyName", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("GpNo", typeof(string));
				dt.Columns.Add("ItemQty", typeof(decimal));
				dt.Columns.Add("GrossWeight", typeof(decimal));
				dt.Columns.Add("NetBillWeight", typeof(decimal));
				dt.Columns.Add("ItemAmount", typeof(decimal));
				dt.Columns.Add("CommissionType", typeof(string));
				dt.Columns.Add("CommRate", typeof(decimal));
				dt.Columns.Add("CommAmount", typeof(decimal));
				dt.Columns.Add("CommBrokery", typeof(string));
				dt.Columns.Add("LedgerBalance", typeof(decimal));
				foreach (DataRow row in dtCommissionAgent.Rows)
				{
					DataRow newRow = dt.NewRow();
					newRow["CommissionFrom"] = row["CommissionFrom"];
					newRow["CommissionAgentId"] = row["CommissionAgentId"];
					newRow["CommissionAgent"] = row["CommissionAgent"];
					newRow["DocumentType"] = row["DocumentTypeCode"];
					newRow["InvoiceDate"] = row["InvoiceDate"];
					newRow["InvoiceNo"] = row["InvoiceNo"];
					newRow["OrderNo"] = row["OrderNo"];
					newRow["PartyName"] = row["SupplierCustomerName"];
					newRow["VehicleNo"] = row["VehicleNo"];
					newRow["GpNo"] = row["GpNo"];
					newRow["ItemQty"] = row["ItemQty"];
					newRow["GrossWeight"] = row["GrossWeight"];
					newRow["NetBillWeight"] = row["NetBillWeight"];
					newRow["ItemAmount"] = row["ItemAmount"];
					newRow["CommissionType"] = row["CommissionType"];
					newRow["CommRate"] = row["CommRate"];
					newRow["CommAmount"] = row["CommAmount"];
					newRow["CommBrokery"] = row["CommBrokery"];
					newRow["LedgerBalance"] = row["LedgerBalance"];
					dt.Rows.Add(newRow);
				}
				grdCommissionAgent.DataSource = dt;
				grdCommissionAgent.RetrieveStructure();
				grdCommissionAgentSetting();
				if (Conversion.ToInt(cmbCommissionAgent.Value) > 0)
				{
					double Balance = Math.Round(CommonServices.GetCurrentGlBalance(Conversion.ToInt(((UltraDropDownBase)cmbCommissionAgent).SelectedRow.Cells[2].Value), txtToDateCommissionAgent.Value));
					lblbalanceCommissionAgent.Text = Balance.ToString("0,0");
					if (Balance > 0.0)
					{
						lblDrCrCommissionAgent.Text = "Dr";
					}
					else if (Balance < 0.0)
					{
						lblDrCrCommissionAgent.Text = "Cr";
					}
					else if (Balance == 0.0)
					{
						lblDrCrCommissionAgent.Text = "Nill";
					}
				}
				else
				{
					lblDrCrCommissionAgent.Text = "";
					lblbalanceCommissionAgent.Text = " 0";
				}
			}
			else
			{
				lblDrCrCommissionAgent.Text = "";
				lblbalanceCommissionAgent.Text = " 0";
				grdCommissionAgent.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCommissionAgentSetting()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdCommissionAgent.RootTable.Columns["CommissionAgentId"].Visible = false;
			grdCommissionAgent.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdCommissionAgent.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdCommissionAgent.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.CommissionAgent + 30;
			grdCommissionAgent.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentType - 80;
			grdCommissionAgent.RootTable.Columns["InvoiceDate"].Width = Constants.InventoryConstants.DateConstant;
			grdCommissionAgent.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdCommissionAgent.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grdCommissionAgent.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName + 30;
			grdCommissionAgent.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdCommissionAgent.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdCommissionAgent.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdCommissionAgent.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdCommissionAgent.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdCommissionAgent.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdCommissionAgent.RootTable.Columns["CommissionType"].Width = Constants.InventoryConstants.DiscountType + 20;
			grdCommissionAgent.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant + 30;
			grdCommissionAgent.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdCommissionAgent.RootTable.Columns["CommBrokery"].Width = 120;
			grdCommissionAgent.RootTable.Columns["LedgerBalance"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdCommissionAgent.RootTable.Columns["CommissionAgent"].Width = 300;
			grdCommissionAgent.RootTable.Columns["PartyName"].Width = 300;
			grdCommissionAgent.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdCommissionAgent.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdCommissionAgent.RootTable.Columns["ItemQty"].FormatString = "0,0";
			grdCommissionAgent.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdCommissionAgent.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdCommissionAgent.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdCommissionAgent.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			grdCommissionAgent.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			grdCommissionAgent.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdCommissionAgent.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdCommissionAgent.RootTable.Columns["NetBillWeight"].FormatString = "0,0";
			grdCommissionAgent.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.###";
			grdCommissionAgent.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdCommissionAgent.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdCommissionAgent.RootTable.Columns["ItemAmount"].FormatString = "0,0";
			grdCommissionAgent.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.###";
			grdCommissionAgent.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdCommissionAgent.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdCommissionAgent.RootTable.Columns["CommAmount"].FormatString = "0,0";
			grdCommissionAgent.RootTable.Columns["CommAmount"].TotalFormatString = "#,##0.###";
			grdCommissionAgent.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdCommissionAgent.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdCommissionAgent.RootTable.Columns["LedgerBalance"].TextAlignment = (TextAlignment)3;
			grdCommissionAgent.RootTable.Columns["LedgerBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
	}

	private void reset()
	{
		try
		{
			((Control)(object)cmbCommissionAgent).Text = string.Empty;
			txtFromDateCommissionAgent.Value = clsGlobalVariables.ActiveYr.Start_Period;
			rdall.Checked = true;
			rdpurchase.Checked = false;
			rdsale.Checked = false;
			grdCommissionAgent.ClearStructure();
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
			CommissionAgentFill();
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtCommissionAgent == null || dtCommissionAgent.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtCommissionAgent, "157-CommissionAndBrokary.rpt");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdCommissionAgent).Name);
			if (fs != null)
			{
				grdCommissionAgent.LoadLayoutFile((Stream)fs);
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
			((Control)(object)grdCommissionAgent).Focus();
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			txtFromDateCommissionAgent.Focus();
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
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
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
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Expected O, but got Unknown
		GridEXLayout grdCommissionAgent_DesignTimeLayout = new GridEXLayout();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.CommissionAgentLedger));
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdCommissionAgent = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdall = new System.Windows.Forms.RadioButton();
		this.rdsale = new System.Windows.Forms.RadioButton();
		this.rdpurchase = new System.Windows.Forms.RadioButton();
		this.lblDrCrCommissionAgent = new System.Windows.Forms.Label();
		this.lblbalanceCommissionAgent = new System.Windows.Forms.Label();
		this.cmbpartyName = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.txtToDateCommissionAgent = new System.Windows.Forms.DateTimePicker();
		this.txtFromDateCommissionAgent = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbCommissionAgent = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCommissionAgent).BeginInit();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbpartyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommissionAgent).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
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
		this.ctrlGrdBar1.MyGrid = this.grdCommissionAgent;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdCommissionAgent.AllowDelete = (InheritableBoolean)1;
		this.grdCommissionAgent.AllowEdit = (InheritableBoolean)2;
		this.grdCommissionAgent.AlternatingColors = true;
		grdCommissionAgent_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCommissionAgent.DesignTimeLayout = grdCommissionAgent_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCommissionAgent).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdCommissionAgent.DynamicFiltering = true;
		this.grdCommissionAgent.FilterMode = (FilterMode)2;
		this.grdCommissionAgent.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdCommissionAgent).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCommissionAgent.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCommissionAgent).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdCommissionAgent).Name = "grdCommissionAgent";
		((ScrollableBase)this.grdCommissionAgent).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCommissionAgent).Size = new System.Drawing.Size(1047, 542);
		((System.Windows.Forms.Control)(object)this.grdCommissionAgent).TabIndex = 44;
		this.grdCommissionAgent.TotalRow = (InheritableBoolean)1;
		this.grdCommissionAgent.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCommissionAgent).VisualStyle = (VisualStyle)4;
		this.grdCommissionAgent.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdCommissionAgent).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(226, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Commission Agent  Ledger";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1047, 666);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdCommissionAgent);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 124);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1047, 542);
		this.panel6.TabIndex = 108;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1047, 124);
		this.panel5.TabIndex = 107;
		this.groupBox1.Controls.Add(this.rdall);
		this.groupBox1.Controls.Add(this.rdsale);
		this.groupBox1.Controls.Add(this.rdpurchase);
		this.groupBox1.Controls.Add(this.lblDrCrCommissionAgent);
		this.groupBox1.Controls.Add(this.lblbalanceCommissionAgent);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbpartyName);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.txtToDateCommissionAgent);
		this.groupBox1.Controls.Add(this.txtFromDateCommissionAgent);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCommissionAgent);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(535, 124);
		this.groupBox1.TabIndex = 148;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.rdall.AutoSize = true;
		this.rdall.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdall.Location = new System.Drawing.Point(116, 99);
		this.rdall.Name = "rdall";
		this.rdall.Size = new System.Drawing.Size(45, 19);
		this.rdall.TabIndex = 4;
		this.rdall.TabStop = true;
		this.rdall.Text = "ALL";
		this.rdall.UseVisualStyleBackColor = true;
		this.rdsale.AutoSize = true;
		this.rdsale.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdsale.Location = new System.Drawing.Point(245, 99);
		this.rdsale.Name = "rdsale";
		this.rdsale.Size = new System.Drawing.Size(47, 19);
		this.rdsale.TabIndex = 6;
		this.rdsale.TabStop = true;
		this.rdsale.Text = "Sale";
		this.rdsale.UseVisualStyleBackColor = true;
		this.rdpurchase.AutoSize = true;
		this.rdpurchase.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdpurchase.Location = new System.Drawing.Point(167, 99);
		this.rdpurchase.Name = "rdpurchase";
		this.rdpurchase.Size = new System.Drawing.Size(73, 19);
		this.rdpurchase.TabIndex = 5;
		this.rdpurchase.TabStop = true;
		this.rdpurchase.Text = "Purcahse";
		this.rdpurchase.UseVisualStyleBackColor = true;
		this.lblDrCrCommissionAgent.AutoSize = true;
		this.lblDrCrCommissionAgent.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDrCrCommissionAgent.Location = new System.Drawing.Point(367, 48);
		this.lblDrCrCommissionAgent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lblDrCrCommissionAgent.Name = "lblDrCrCommissionAgent";
		this.lblDrCrCommissionAgent.Size = new System.Drawing.Size(39, 15);
		this.lblDrCrCommissionAgent.TabIndex = 140;
		this.lblDrCrCommissionAgent.Text = "label3";
		this.lblbalanceCommissionAgent.AutoSize = true;
		this.lblbalanceCommissionAgent.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.lblbalanceCommissionAgent.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblbalanceCommissionAgent.Location = new System.Drawing.Point(406, 45);
		this.lblbalanceCommissionAgent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.lblbalanceCommissionAgent.Name = "lblbalanceCommissionAgent";
		this.lblbalanceCommissionAgent.Size = new System.Drawing.Size(50, 20);
		this.lblbalanceCommissionAgent.TabIndex = 141;
		this.lblbalanceCommissionAgent.Text = "label3";
		this.cmbpartyName.AllowNull = (DefaultableBoolean)1;
		this.cmbpartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbpartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbpartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbpartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance219;
		((SpecialBoxBase)((UltraGridBase)this.cmbpartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance224).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbpartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbpartyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbpartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbpartyName).Location = new System.Drawing.Point(116, 69);
		((System.Windows.Forms.Control)(object)this.cmbpartyName).Name = "cmbpartyName";
		((System.Windows.Forms.Control)(object)this.cmbpartyName).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.cmbpartyName).TabIndex = 3;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(7, 75);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(69, 15);
		this.label20.TabIndex = 139;
		this.label20.Text = "Party Name";
		this.txtToDateCommissionAgent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateCommissionAgent.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDateCommissionAgent.Location = new System.Drawing.Point(241, 17);
		this.txtToDateCommissionAgent.Name = "txtToDateCommissionAgent";
		this.txtToDateCommissionAgent.Size = new System.Drawing.Size(124, 23);
		this.txtToDateCommissionAgent.TabIndex = 1;
		this.txtFromDateCommissionAgent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateCommissionAgent.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtFromDateCommissionAgent.Location = new System.Drawing.Point(116, 17);
		this.txtFromDateCommissionAgent.Name = "txtFromDateCommissionAgent";
		this.txtFromDateCommissionAgent.Size = new System.Drawing.Size(124, 23);
		this.txtFromDateCommissionAgent.TabIndex = 0;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(307, 96);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 25);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbCommissionAgent.AllowNull = (DefaultableBoolean)1;
		this.cmbCommissionAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCommissionAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCommissionAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance207;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCommissionAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).Location = new System.Drawing.Point(116, 42);
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).Name = "cmbCommissionAgent";
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).TabIndex = 2;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(7, 48);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(108, 15);
		this.label6.TabIndex = 138;
		this.label6.Text = "Commission Agent";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(7, 21);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(91, 15);
		this.label9.TabIndex = 137;
		this.label9.Text = "From && To Date";
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
		this.print.Text = "157-Print";
		this.print.Click += new System.EventHandler(print_Click_1);
		this.btnShortCutKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKey.Image");
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1047, 723);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "CommissionAgentLedger";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CustomerLedger_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCommissionAgent).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbpartyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommissionAgent).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
