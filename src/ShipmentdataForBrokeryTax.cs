// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Export;
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
namespace Architecture.WinApp.ExportReports
{

public class ShipmentdataForBrokeryTax : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtHistory = new DataTable();

	private IContainer components;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnRegisterReport;

	private ToolStripButton btnShortCutKey;

	private Panel panel5;

	private Label label3;

	private Panel panel1;

	private GroupBox groupBox1;

	internal UltraCombo CmbDateType;

	private Label label11;

	private DateTimePicker txtdateto;

	private Label label5;

	private DateTimePicker txtdatefrom;

	private Label label51;

	private Button btnShow;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdShipment;

	private Label label1;

	private GroupBox groupBox2;

	private Label label4;

	private TextBox txtPercent;

	private Label lblTotalDiffPkr;

	private Label lblPercntPkr;

	private Label lblBrokeryPkr;

	private Label label6;

	private TextBox txtBrokeryPrcnt;

	public ShipmentdataForBrokeryTax(UserAccount pUser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
	}

	private void Commercial_Invoice_Shipments_Load(object sender, EventArgs e)
	{
		try
		{
			ParameterFill();
			((UltraGridBase)CmbDateType).Rows[2].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, CmbDateType, "Id", "Parameters", "Parameters", ZeroIndex: false);
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
				txtdatefrom.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 2)
			{
				txtdatefrom.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtdatefrom.Value = first;
				txtdateto.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtdatefrom.Value = first2;
				txtdateto.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 5)
			{
				txtdatefrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdShipment).Name);
			if (fs != null)
			{
				grdShipment.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Conversion.ToDateTime((object)txtdatefrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtdateto.Value);
			dtHistory = ExImInvoice.ShipmentdataForBrokeryTax(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("InvoiceId", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("PTYInvoiceNo");
				dt.Columns.Add("FCL", typeof(double));
				dt.Columns.Add("CustomerName");
				dt.Columns.Add("InvoiceAmount", typeof(double));
				dt.Columns.Add("InvoiceExRate", typeof(double));
				dt.Columns.Add("LcyAmount", typeof(double));
				dt.Columns.Add("BankName");
				dt.Columns.Add("GdNo");
				dt.Columns.Add("GdDate", typeof(DateTime));
				dt.Columns.Add("GdValue", typeof(double));
				dt.Columns.Add("ExchangeRate", typeof(double));
				dt.Columns.Add("LcyGdValue", typeof(double));
				dt.Columns.Add("DiffAmount", typeof(double));
				dt.Columns.Add("LcyDiffAmount", typeof(double));
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["DocumentTypeId"], dtHistory.Rows[i]["ExImInvoiceId"], Conversion.ToDateTime(dtHistory.Rows[i]["DocDate"]).ToShortDateString(), dtHistory.Rows[i]["PTYInvoiceNo"], dtHistory.Rows[i]["FCL"], dtHistory.Rows[i]["CustomerName"], dtHistory.Rows[i]["InvoiceAmount"], dtHistory.Rows[i]["InvExRate"], dtHistory.Rows[i]["LcyAmount"], dtHistory.Rows[i]["BankName"], dtHistory.Rows[i]["GDNO"], Conversion.ToDateTime(dtHistory.Rows[i]["GDDate"]).ToShortDateString(), dtHistory.Rows[i]["GDValue"], dtHistory.Rows[i]["ExchangeRate"], dtHistory.Rows[i]["LcyGdValue"], dtHistory.Rows[i]["DiffAmount"], dtHistory.Rows[i]["LcyDiffAmount"]);
				}
				grdShipment.DataSource = dt;
				grdShipment.RetrieveStructure();
				GridSetting();
				groupBox2.Show();
				double DiffPkrTotal = Conversion.ToDouble(dtHistory.Compute("Sum(LcyDiffAmount)", string.Empty));
				lblTotalDiffPkr.Text = DiffPkrTotal.ToString(clsGlobalVariables.stringFormatboth);
			}
			else
			{
				grdShipment.ClearStructure();
				groupBox2.Hide();
				lblTotalDiffPkr.Text = "0";
				lblPercntPkr.Text = "0";
				lblBrokeryPkr.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdShipment.RootTable.Columns["CustomerName"].Caption = "Consignee";
			grdShipment.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdShipment.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdShipment.RootTable.Columns["DocDate"].Width = Constants.ExportConstants.DateConstant;
			grdShipment.RootTable.Columns["PTYInvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdShipment.RootTable.Columns["FCL"].Width = Constants.ExportConstants.FclConstant;
			grdShipment.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.StringConstant;
			grdShipment.RootTable.Columns["InvoiceAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdShipment.RootTable.Columns["InvoiceExRate"].Width = Constants.ExportConstants.ExRateConstant;
			grdShipment.RootTable.Columns["LcyAmount"].Width = 110;
			grdShipment.RootTable.Columns["BankName"].Width = Constants.ExportConstants.StringConstant;
			grdShipment.RootTable.Columns["GdNo"].Width = 120;
			grdShipment.RootTable.Columns["GdDate"].Width = Constants.ExportConstants.DateConstant;
			grdShipment.RootTable.Columns["GdValue"].Width = 100;
			grdShipment.RootTable.Columns["ExchangeRate"].Width = Constants.ExportConstants.ExRateConstant;
			grdShipment.RootTable.Columns["LcyGdValue"].Width = 110;
			grdShipment.RootTable.Columns["DiffAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdShipment.RootTable.Columns["LcyDiffAmount"].Width = 110;
			grdShipment.RootTable.Columns["Id"].Visible = false;
			grdShipment.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdShipment.RootTable.Columns["InvoiceId"].Visible = false;
			grdShipment.RootTable.Columns["FCL"].AggregateFunction = (AggregateFunction)2;
			grdShipment.RootTable.Columns["FCL"].TextAlignment = (TextAlignment)3;
			grdShipment.RootTable.Columns["FCL"].FormatString = "#,##0.###";
			grdShipment.RootTable.Columns["FCL"].TotalFormatString = "#,##0.###";
			grdShipment.RootTable.Columns["InvoiceAmount"].AggregateFunction = (AggregateFunction)2;
			grdShipment.RootTable.Columns["InvoiceAmount"].TextAlignment = (TextAlignment)3;
			grdShipment.RootTable.Columns["InvoiceAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdShipment.RootTable.Columns["InvoiceAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdShipment.RootTable.Columns["InvoiceExRate"].TextAlignment = (TextAlignment)3;
			grdShipment.RootTable.Columns["InvoiceExRate"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdShipment.RootTable.Columns["LcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdShipment.RootTable.Columns["LcyAmount"].TextAlignment = (TextAlignment)3;
			grdShipment.RootTable.Columns["LcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdShipment.RootTable.Columns["LcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdShipment.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdShipment.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdShipment.RootTable.Columns["GdValue"].AggregateFunction = (AggregateFunction)2;
			grdShipment.RootTable.Columns["GdValue"].TextAlignment = (TextAlignment)3;
			grdShipment.RootTable.Columns["GdValue"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdShipment.RootTable.Columns["GdValue"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdShipment.RootTable.Columns["LcyGdValue"].AggregateFunction = (AggregateFunction)2;
			grdShipment.RootTable.Columns["LcyGdValue"].TextAlignment = (TextAlignment)3;
			grdShipment.RootTable.Columns["LcyGdValue"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdShipment.RootTable.Columns["LcyGdValue"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdShipment.RootTable.Columns["DiffAmount"].AggregateFunction = (AggregateFunction)2;
			grdShipment.RootTable.Columns["DiffAmount"].TextAlignment = (TextAlignment)3;
			grdShipment.RootTable.Columns["DiffAmount"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdShipment.RootTable.Columns["DiffAmount"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdShipment.RootTable.Columns["LcyDiffAmount"].AggregateFunction = (AggregateFunction)2;
			grdShipment.RootTable.Columns["LcyDiffAmount"].TextAlignment = (TextAlignment)3;
			grdShipment.RootTable.Columns["LcyDiffAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grdShipment.RootTable.Columns["LcyDiffAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			CmbDateType.Value = 3;
			txtdateto.Value = DateTime.Now;
			grdShipment.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
	}

	private void CommissionAgentFcyLedger_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdShipment).Focus();
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnRegisterReport_Click(null, null);
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRegisterReport_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtHistory == null || dtHistory.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
			}
			else
			{
				new Reporting().ShowReportWithDataTable(dtHistory, "570-ShipmentdataForBrokeryTax.rpt");
			}
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+S", "For Showing Data");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's of Grid (On Button Or Link)");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtPercent.Text) < 0.0 || Conversion.ToDouble((object)txtPercent.Text) > 99.0)
			{
				txtPercent.Text = "0";
				MessageBox.Show("Percent Can't greater than 99", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			double num = Conversion.ToDouble((object)lblTotalDiffPkr.Text);
			double Prct = Conversion.ToDouble((object)txtPercent.Text);
			double pkrPrcntAmount = num * (Prct / 100.0);
			lblPercntPkr.Text = pkrPrcntAmount.ToString(clsGlobalVariables.stringFormatboth);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPercent_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBrokeryPrcnt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBrokeryPrcnt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtBrokeryPrcnt.Text) < 0.0 || Conversion.ToDouble((object)txtBrokeryPrcnt.Text) > 99.0)
			{
				txtPercent.Text = "0";
				MessageBox.Show("Brokery Percent Can't greater than 99", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			double num = Conversion.ToDouble((object)lblPercntPkr.Text);
			double Prct = Conversion.ToDouble((object)txtBrokeryPrcnt.Text);
			double brokeryPrcntAmount = num * (Prct / 100.0);
			lblBrokeryPkr.Text = brokeryPrcntAmount.ToString(clsGlobalVariables.stringFormatboth);
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
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		Appearance appearance13 = new Appearance();
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
		Appearance appearance24 = new Appearance();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRegisterReport = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdShipment = new GridEX();
		this.label3 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.lblBrokeryPkr = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtBrokeryPrcnt = new System.Windows.Forms.TextBox();
		this.lblPercntPkr = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtPercent = new System.Windows.Forms.TextBox();
		this.lblTotalDiffPkr = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.CmbDateType = new UltraCombo();
		this.txtdateto = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtdatefrom = new System.Windows.Forms.DateTimePicker();
		this.label51 = new System.Windows.Forms.Label();
		this.tsItemType.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdShipment).BeginInit();
		this.panel1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateType).BeginInit();
		base.SuspendLayout();
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnRegisterReport, this.btnRefresh, this.btnShortCutKey });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(971, 27);
		this.tsItemType.TabIndex = 4;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRegisterReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRegisterReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegisterReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegisterReport.Name = "btnRegisterReport";
		this.btnRegisterReport.Size = new System.Drawing.Size(92, 24);
		this.btnRegisterReport.Text = "Print-570";
		this.btnRegisterReport.Click += new System.EventHandler(btnRegisterReport_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Visible = false;
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 27);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(971, 28);
		this.panel5.TabIndex = 20;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(930, 3);
		this.ctrlGrdBar1.MyGrid = this.grdShipment;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 4;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdShipment.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdShipment).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdShipment.DynamicFiltering = true;
		this.grdShipment.FilterMode = (FilterMode)2;
		this.grdShipment.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdShipment).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdShipment.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdShipment).Location = new System.Drawing.Point(0, 119);
		((System.Windows.Forms.Control)(object)this.grdShipment).Name = "grdShipment";
		((ScrollableBase)this.grdShipment).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdShipment).Size = new System.Drawing.Size(971, 315);
		((System.Windows.Forms.Control)(object)this.grdShipment).TabIndex = 22;
		((System.Windows.Forms.Control)(object)this.grdShipment).TabStop = false;
		this.grdShipment.TotalRow = (InheritableBoolean)1;
		this.grdShipment.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdShipment).VisualStyle = (VisualStyle)4;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(6, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(266, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Shipment Data For Brokery Tax";
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 55);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(971, 64);
		this.panel1.TabIndex = 21;
		this.groupBox2.Controls.Add(this.lblBrokeryPkr);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.txtBrokeryPrcnt);
		this.groupBox2.Controls.Add(this.lblPercntPkr);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.txtPercent);
		this.groupBox2.Controls.Add(this.lblTotalDiffPkr);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(433, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(530, 64);
		this.groupBox2.TabIndex = 2;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Extra Information ";
		this.groupBox2.Visible = false;
		this.lblBrokeryPkr.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBrokeryPkr.Location = new System.Drawing.Point(361, 40);
		this.lblBrokeryPkr.Name = "lblBrokeryPkr";
		this.lblBrokeryPkr.Size = new System.Drawing.Size(158, 18);
		this.lblBrokeryPkr.TabIndex = 599;
		this.lblBrokeryPkr.Text = "0";
		this.lblBrokeryPkr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(346, 16);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(60, 15);
		this.label6.TabIndex = 598;
		this.label6.Text = "Brokery %";
		this.txtBrokeryPrcnt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryPrcnt.Location = new System.Drawing.Point(410, 12);
		this.txtBrokeryPrcnt.Name = "txtBrokeryPrcnt";
		this.txtBrokeryPrcnt.Size = new System.Drawing.Size(58, 23);
		this.txtBrokeryPrcnt.TabIndex = 597;
		this.txtBrokeryPrcnt.TextChanged += new System.EventHandler(txtBrokeryPrcnt_TextChanged);
		this.txtBrokeryPrcnt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrokeryPrcnt_KeyPress);
		this.lblPercntPkr.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPercntPkr.Location = new System.Drawing.Point(181, 40);
		this.lblPercntPkr.Name = "lblPercntPkr";
		this.lblPercntPkr.Size = new System.Drawing.Size(158, 18);
		this.lblPercntPkr.TabIndex = 596;
		this.lblPercntPkr.Text = "0";
		this.lblPercntPkr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(208, 16);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(17, 15);
		this.label4.TabIndex = 595;
		this.label4.Text = "%";
		this.txtPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPercent.Location = new System.Drawing.Point(230, 12);
		this.txtPercent.Name = "txtPercent";
		this.txtPercent.Size = new System.Drawing.Size(58, 23);
		this.txtPercent.TabIndex = 3;
		this.txtPercent.TextChanged += new System.EventHandler(txtPercent_TextChanged);
		this.txtPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPercent_KeyPress);
		this.lblTotalDiffPkr.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalDiffPkr.Location = new System.Drawing.Point(6, 40);
		this.lblTotalDiffPkr.Name = "lblTotalDiffPkr";
		this.lblTotalDiffPkr.Size = new System.Drawing.Size(158, 18);
		this.lblTotalDiffPkr.TabIndex = 2;
		this.lblTotalDiffPkr.Text = "0";
		this.lblTotalDiffPkr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Teal;
		this.label1.Location = new System.Drawing.Point(36, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(91, 16);
		this.label1.TabIndex = 1;
		this.label1.Text = "Total Lcy Diff";
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateType);
		this.groupBox1.Controls.Add(this.txtdateto);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtdatefrom);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(433, 64);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(364, 32);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(60, 26);
		this.btnShow.TabIndex = 4;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.CmbDateType.AllowNull = (DefaultableBoolean)1;
		this.CmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateType).Location = new System.Drawing.Point(7, 32);
		((System.Windows.Forms.Control)(object)this.CmbDateType).Name = "CmbDateType";
		((System.Windows.Forms.Control)(object)this.CmbDateType).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateType).TabIndex = 0;
		this.CmbDateType.ValueChanged += new System.EventHandler(CmbDateType_ValueChanged);
		this.txtdateto.CustomFormat = "dd-MMM-yy";
		this.txtdateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdateto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdateto.Location = new System.Drawing.Point(249, 34);
		this.txtdateto.Name = "txtdateto";
		this.txtdateto.Size = new System.Drawing.Size(114, 23);
		this.txtdateto.TabIndex = 2;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(7, 14);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 594;
		this.label11.Text = "Date Type";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(250, 14);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 592;
		this.label5.Text = "Date To";
		this.txtdatefrom.CustomFormat = "dd-MMM-yy";
		this.txtdatefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdatefrom.Location = new System.Drawing.Point(127, 34);
		this.txtdatefrom.Name = "txtdatefrom";
		this.txtdatefrom.Size = new System.Drawing.Size(120, 23);
		this.txtdatefrom.TabIndex = 1;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(124, 14);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(63, 15);
		this.label51.TabIndex = 590;
		this.label51.Text = "Date From";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(971, 434);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdShipment);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.tsItemType);
		base.KeyPreview = true;
		base.Name = "ShipmentdataForBrokeryTax";
		this.Text = "Commercial_Invoice_Shipments";
		base.Load += new System.EventHandler(Commercial_Invoice_Shipments_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(CommissionAgentFcyLedger_KeyDown);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdShipment).EndInit();
		this.panel1.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateType).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
