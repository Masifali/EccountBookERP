// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Reports.Accounts;
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

public class frmPostDatedChequeReports : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtHistory = new DataTable();

	private IContainer components;

	private Panel ParentPanel;

	private Panel FormPanel;

	private GroupBox groupBox1;

	private Panel panel1;

	private Label label1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnRefresh;

	private DateTimePicker ChequeToDate;

	private TextBox txtChequeNoTo;

	private Label label36;

	private Label label35;

	private TextBox txtChequeNoFrom;

	private DateTimePicker ChequeFromDate;

	private Label label27;

	private Label label33;

	internal UltraCombo CmbChequeType;

	private Label label4;

	internal UltraCombo CmbChequeAccount;

	private Label label3;

	internal UltraCombo CmbBankGlAccount;

	private Label label2;

	private RadioButton RBCancel;

	private RadioButton RBUnclear;

	private RadioButton RBClear;

	private Button btnshow;

	internal GridEX DataGridHistory;

	private ToolStripButton btnPrint;

	private CtrlGrdBar ctrlGrdBar1;

	public frmPostDatedChequeReports(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void frmPostDatedChequeReports_Load(object sender, EventArgs e)
	{
		try
		{
			ParentPanel.HorizontalScroll.Visible = true;
			ParentPanel.HorizontalScroll.Enabled = true;
			ChequeFromDate.Value = DateTime.Now;
			GlAccountFill();
			ChequeAccountFill();
			ChequeTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GlAccountFill()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("15");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbBankGlAccount, "Id", "AccountTitle", "Bank Gl Account Name", ZeroIndex: false);
				((UltraGridBase)CmbBankGlAccount).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChequeAccountFill()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,6,8");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbChequeAccount, "Id", "AccountTitle", "Account Name", ZeroIndex: false);
				((UltraGridBase)CmbChequeAccount).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChequeTypeFill()
	{
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = ChequeFromDate.Value;
			if (ChequeToDate.Checked)
			{
				obj.ToDate = ChequeToDate.Value;
			}
			obj.FromDocNo = Conversion.ToInt((object)txtChequeNoFrom.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtChequeNoTo.Text);
			obj.BankGlDrAc = Conversion.ToInt(CmbBankGlAccount.Value);
			if (RBUnclear.Checked)
			{
				obj.IsApproved = true;
			}
			else if (RBClear.Checked)
			{
				obj.IsApproved = false;
			}
			obj.AccountId = Conversion.ToInt(CmbChequeAccount.Value);
			DataSet ds = VoucherReports.PostDatedChecqueReport(obj);
			dtHistory = ds.Tables[0].Copy();
			if (dtHistory.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtHistory, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				DataTable dt = new DataTable();
				dt.Columns.Add("AccountId", typeof(int));
				dt.Columns.Add("AccountIdBank", typeof(int));
				dt.Columns.Add("VoucherDate", typeof(DateTime));
				dt.Columns.Add("VoucherNo", typeof(int));
				dt.Columns.Add("VoucherType", typeof(string));
				dt.Columns.Add("ChequeDate", typeof(DateTime));
				dt.Columns.Add("ChequeNo", typeof(int));
				dt.Columns.Add("ChequeType", typeof(string));
				dt.Columns.Add("AccountTitleBank", typeof(string));
				dt.Columns.Add("AccountTitle", typeof(string));
				dt.Columns.Add("ChequeStatus", typeof(string));
				dt.Columns.Add("DebitAmount", typeof(double));
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["AccountId"], Conversion.ToInt(dtHistory.Rows[i]["AccountIdBank"]), Conversion.ToDateTime(dtHistory.Rows[i]["VoucherDate"]), Conversion.ToInt(dtHistory.Rows[i]["VoucherNo"]), Conversion.ToString(dtHistory.Rows[i]["VoucherType"]), Conversion.ToDateTime(dtHistory.Rows[i]["CheqDate"]), Conversion.ToInt(dtHistory.Rows[i]["CheqNo"]), Conversion.ToString(dtHistory.Rows[i]["CheqType"]), Conversion.ToString(dtHistory.Rows[i]["AccountTitleBank"]), Conversion.ToString(dtHistory.Rows[i]["AccountTitle"]), Conversion.ToString(dtHistory.Rows[i]["ChequeStatus"]), Conversion.ToDouble(dtHistory.Rows[i]["DebitAmount"]));
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount);
				DataGridHistory.RootTable.Columns["VoucherDate"].Width = Constants.InventoryConstants.DateConstant;
				DataGridHistory.RootTable.Columns["VoucherNo"].Width = Constants.InventoryConstants.DocNoConstant;
				DataGridHistory.RootTable.Columns["VoucherType"].Width = Constants.InventoryConstants.DocumentTypeCode;
				DataGridHistory.RootTable.Columns["ChequeDate"].Width = Constants.InventoryConstants.DateConstant;
				DataGridHistory.RootTable.Columns["ChequeNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
				DataGridHistory.RootTable.Columns["ChequeType"].Width = Constants.InventoryConstants.DiscountType;
				DataGridHistory.RootTable.Columns["AccountTitleBank"].Width = Constants.InventoryConstants.AccountTitle + 20;
				DataGridHistory.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 20;
				DataGridHistory.RootTable.Columns["ChequeStatus"].Width = Constants.InventoryConstants.StatusConstant;
				DataGridHistory.RootTable.Columns["DebitAmount"].Width = Constants.InventoryConstants.AmountConstant;
				DataGridHistory.RootTable.Columns["AccountId"].Visible = false;
				DataGridHistory.RootTable.Columns["AccountIdBank"].Visible = false;
				DataGridHistory.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
				DataGridHistory.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
				DataGridHistory.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatboth;
				DataGridHistory.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
				ctrlGrdBar1_Load(null, null);
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtChequeNoFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtChequeNoTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtHistory.Rows.Count == 0 || dtHistory == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@PrintedBy", UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(dtHistory, "177-PostDatedChequeRegister.rpt");
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

	private void frmPostDatedChequeReports_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		this.ParentPanel = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.FormPanel = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.RBCancel = new System.Windows.Forms.RadioButton();
		this.RBUnclear = new System.Windows.Forms.RadioButton();
		this.RBClear = new System.Windows.Forms.RadioButton();
		this.CmbChequeType = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbChequeAccount = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbBankGlAccount = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.ChequeToDate = new System.Windows.Forms.DateTimePicker();
		this.txtChequeNoTo = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.txtChequeNoFrom = new System.Windows.Forms.TextBox();
		this.ChequeFromDate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.ParentPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.FormPanel.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBankGlAccount).BeginInit();
		this.panel1.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.ParentPanel.AutoScroll = true;
		this.ParentPanel.BackColor = System.Drawing.Color.White;
		this.ParentPanel.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.ParentPanel.Controls.Add(this.FormPanel);
		this.ParentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ParentPanel.Location = new System.Drawing.Point(0, 0);
		this.ParentPanel.Name = "ParentPanel";
		this.ParentPanel.Size = new System.Drawing.Size(945, 659);
		this.ParentPanel.TabIndex = 0;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.AlternatingColors = true;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 185);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(945, 474);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.FormPanel.Controls.Add(this.groupBox1);
		this.FormPanel.Controls.Add(this.panel1);
		this.FormPanel.Controls.Add(this.toolStrip3);
		this.FormPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.FormPanel.Location = new System.Drawing.Point(0, 0);
		this.FormPanel.Name = "FormPanel";
		this.FormPanel.Size = new System.Drawing.Size(945, 185);
		this.FormPanel.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(896, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.RBCancel);
		this.groupBox1.Controls.Add(this.RBUnclear);
		this.groupBox1.Controls.Add(this.RBClear);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChequeType);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChequeAccount);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBankGlAccount);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.ChequeToDate);
		this.groupBox1.Controls.Add(this.txtChequeNoTo);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtChequeNoFrom);
		this.groupBox1.Controls.Add(this.ChequeFromDate);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 57);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(681, 128);
		this.groupBox1.TabIndex = 3;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Criteria's";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(613, 95);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 26);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.RBCancel.AutoSize = true;
		this.RBCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RBCancel.Location = new System.Drawing.Point(523, 99);
		this.RBCancel.Name = "RBCancel";
		this.RBCancel.Size = new System.Drawing.Size(60, 19);
		this.RBCancel.TabIndex = 9;
		this.RBCancel.Text = "Cancel";
		this.RBCancel.UseVisualStyleBackColor = true;
		this.RBCancel.Visible = false;
		this.RBUnclear.AutoSize = true;
		this.RBUnclear.Checked = true;
		this.RBUnclear.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RBUnclear.Location = new System.Drawing.Point(395, 99);
		this.RBUnclear.Name = "RBUnclear";
		this.RBUnclear.Size = new System.Drawing.Size(65, 19);
		this.RBUnclear.TabIndex = 7;
		this.RBUnclear.TabStop = true;
		this.RBUnclear.Text = "Unclear";
		this.RBUnclear.UseVisualStyleBackColor = true;
		this.RBClear.AutoSize = true;
		this.RBClear.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RBClear.Location = new System.Drawing.Point(466, 99);
		this.RBClear.Name = "RBClear";
		this.RBClear.Size = new System.Drawing.Size(51, 19);
		this.RBClear.TabIndex = 8;
		this.RBClear.Text = "Clear";
		this.RBClear.UseVisualStyleBackColor = true;
		this.CmbChequeType.AllowNull = (DefaultableBoolean)1;
		this.CmbChequeType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChequeType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChequeType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChequeType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChequeType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChequeType).Location = new System.Drawing.Point(394, 67);
		((System.Windows.Forms.Control)(object)this.CmbChequeType).Name = "CmbChequeType";
		((System.Windows.Forms.Control)(object)this.CmbChequeType).Size = new System.Drawing.Size(281, 26);
		((System.Windows.Forms.Control)(object)this.CmbChequeType).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbChequeType).Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(289, 73);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(75, 15);
		this.label4.TabIndex = 163;
		this.label4.Text = "Cheque Type";
		this.label4.Visible = false;
		this.CmbChequeAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbChequeAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChequeAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChequeAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChequeAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChequeAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChequeAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChequeAccount).Location = new System.Drawing.Point(394, 40);
		((System.Windows.Forms.Control)(object)this.CmbChequeAccount).Name = "CmbChequeAccount";
		((System.Windows.Forms.Control)(object)this.CmbChequeAccount).Size = new System.Drawing.Size(281, 26);
		((System.Windows.Forms.Control)(object)this.CmbChequeAccount).TabIndex = 5;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(289, 46);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(95, 15);
		this.label3.TabIndex = 161;
		this.label3.Text = "Cheque Account";
		this.CmbBankGlAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbBankGlAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBankGlAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBankGlAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBankGlAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBankGlAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBankGlAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBankGlAccount).Location = new System.Drawing.Point(394, 13);
		((System.Windows.Forms.Control)(object)this.CmbBankGlAccount).Name = "CmbBankGlAccount";
		((System.Windows.Forms.Control)(object)this.CmbBankGlAccount).Size = new System.Drawing.Size(281, 26);
		((System.Windows.Forms.Control)(object)this.CmbBankGlAccount).TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(289, 19);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(98, 15);
		this.label2.TabIndex = 159;
		this.label2.Text = "Bank GL Account";
		this.ChequeToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChequeToDate.Checked = false;
		this.ChequeToDate.CustomFormat = "dd-MMM-yyyy";
		this.ChequeToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChequeToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ChequeToDate.Location = new System.Drawing.Point(135, 42);
		this.ChequeToDate.Name = "ChequeToDate";
		this.ChequeToDate.ShowCheckBox = true;
		this.ChequeToDate.Size = new System.Drawing.Size(153, 23);
		this.ChequeToDate.TabIndex = 1;
		this.txtChequeNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtChequeNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtChequeNoTo.Location = new System.Drawing.Point(135, 97);
		this.txtChequeNoTo.Name = "txtChequeNoTo";
		this.txtChequeNoTo.Size = new System.Drawing.Size(153, 23);
		this.txtChequeNoTo.TabIndex = 3;
		this.txtChequeNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtChequeNoTo_KeyPress);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(12, 101);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(82, 15);
		this.label36.TabIndex = 157;
		this.label36.Text = "Cheque No To";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(11, 73);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(97, 15);
		this.label35.TabIndex = 156;
		this.label35.Text = "Cheque No From";
		this.txtChequeNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtChequeNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtChequeNoFrom.Location = new System.Drawing.Point(135, 69);
		this.txtChequeNoFrom.Name = "txtChequeNoFrom";
		this.txtChequeNoFrom.Size = new System.Drawing.Size(153, 23);
		this.txtChequeNoFrom.TabIndex = 2;
		this.txtChequeNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtChequeNoFrom_KeyPress);
		this.ChequeFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChequeFromDate.CustomFormat = "dd-MMM-yyyy";
		this.ChequeFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChequeFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ChequeFromDate.Location = new System.Drawing.Point(135, 15);
		this.ChequeFromDate.Name = "ChequeFromDate";
		this.ChequeFromDate.Size = new System.Drawing.Size(153, 23);
		this.ChequeFromDate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(11, 19);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(106, 15);
		this.label27.TabIndex = 155;
		this.label27.Text = "Cheque From Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(11, 46);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(91, 15);
		this.label33.TabIndex = 154;
		this.label33.Text = "Cheque To Date";
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(945, 30);
		this.panel1.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 2);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(247, 25);
		this.label1.TabIndex = 3;
		this.label1.Text = "Post Dated Cheque Report";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnRefresh, this.btnPrint });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(945, 27);
		this.toolStrip3.TabIndex = 1;
		this.toolStrip3.Text = "toolStrip3";
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(70, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(79, 24);
		this.btnPrint.Text = "177-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(945, 659);
		base.Controls.Add(this.ParentPanel);
		base.Name = "frmPostDatedChequeReports";
		this.Text = "Post Dated Cheque Reports";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmPostDatedChequeReports_FormClosing);
		base.Load += new System.EventHandler(frmPostDatedChequeReports_Load);
		this.ParentPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.FormPanel.ResumeLayout(false);
		this.FormPanel.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBankGlAccount).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
