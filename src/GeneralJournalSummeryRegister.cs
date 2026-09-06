using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
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
namespace Architecture.WinApp.Account_Reports
{

public class GeneralJournalSummeryRegister : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGrd = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnPrint;

	private Panel panel4;

	private Label label2;

	private Label label1;

	private Button btnshow;

	private DateTimePicker txtDateFrom;

	private DateTimePicker txtDateTo;

	internal GridEX grdfrm;

	internal UltraCombo cmbRegisterType;

	private Label label8;

	public GeneralJournalSummeryRegister(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void GeneralJournalSummeryRegister_Load(object sender, EventArgs e)
	{
		GridBind();
		RegisterTypeFill();
		cmbRegisterType.Focus();
	}

	public void RegisterTypeFill()
	{
		try
		{
			DataTable dtRegister = new DataTable();
			dtRegister.Columns.Add("Id");
			dtRegister.Columns.Add("RegisterType");
			dtRegister.Rows.Add(1, "Summery");
			dtRegister.Rows.Add(2, "Detail");
			DDL.BindDDL(dtRegister, cmbRegisterType, "Id", "RegisterType", "Register Type", ZeroIndex: false);
			((UltraGridBase)cmbRegisterType).DisplayLayout.Bands[0].Columns[1].Width = 182;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtDateTo.Value);
			dtGrd = VoucherReports.GeneralJournalReport(obj);
			DataTable dt = new DataTable();
			if (dtGrd.Rows.Count > 0)
			{
				dt.Columns.Add("RegisterType");
				dt.Columns.Add("Document Type");
				dt.Columns.Add("Voucher No");
				dt.Columns.Add("VoucherDate");
				dt.Columns.Add("RefAccountTitle");
				dt.Columns.Add("VoucherAmount");
				dt.Columns.Add("Remarks");
				dt.Columns.Add("ChequeNo");
				dt.Columns.Add("AgainstAccountTitle");
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate");
				dt.Columns.Add("Is_Approved");
				dt.Columns.Add("PostDate");
				for (int i = 0; i < dtGrd.Rows.Count; i++)
				{
					dt.Rows.Add(dtGrd.Rows[i]["RegisterType"], dtGrd.Rows[i]["V_Type"], dtGrd.Rows[i]["V_No"], Conversion.ToDateTime(dtGrd.Rows[i]["VoucherDate"]).ToString("dd-MMM-yyyy"), dtGrd.Rows[i]["RefAccountTitle"], dtGrd.Rows[i]["VoucherAmount"], dtGrd.Rows[i]["Remarks"], dtGrd.Rows[i]["ChequeNo"], dtGrd.Rows[i]["AgainstAccountTitle"], dtGrd.Rows[i]["EntryUser"], Conversion.ToDateTime(dtGrd.Rows[i]["EntryDate"]).ToString("dd-MMM-yyyy"), dtGrd.Rows[i]["Is_Approved"], Conversion.ToDateTime(dtGrd.Rows[i]["PostDate"]).ToString("dd-MMM-yyyy"));
					grdfrm.DataSource = dt;
					grdfrm.RetrieveStructure();
					gridSetting();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Reset()
	{
		try
		{
			((Control)(object)cmbRegisterType).Text = string.Empty;
			txtDateFrom.Value = clsGlobalVariables.ActiveYr.Start_Period;
			txtDateTo.Value = DateTime.Now;
			GridBind();
			cmbRegisterType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneralJournalSummeryRegister_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnRefresh_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void gridSetting()
	{
		grdfrm.RootTable.Columns["RegisterType"].Width = 100;
		grdfrm.RootTable.Columns["Voucher No"].Width = 90;
		grdfrm.RootTable.Columns["VoucherDate"].Width = 100;
		grdfrm.RootTable.Columns["RefAccountTitle"].Width = 200;
		grdfrm.RootTable.Columns["VoucherAmount"].Width = 110;
		grdfrm.RootTable.Columns["Remarks"].Width = 120;
		grdfrm.RootTable.Columns["ChequeNo"].Width = 80;
		grdfrm.RootTable.Columns["AgainstAccountTitle"].Width = 200;
		grdfrm.RootTable.Columns["EntryUser"].Width = 110;
		grdfrm.RootTable.Columns["EntryDate"].Width = 100;
		grdfrm.RootTable.Columns["Is_Approved"].Width = 100;
		grdfrm.RootTable.Columns["PostDate"].Width = 100;
		grdfrm.RootTable.Columns["Document Type"].Visible = false;
		grdfrm.RootTable.Columns["VoucherAmount"].TextAlignment = (TextAlignment)3;
		grdfrm.RootTable.Columns["VoucherAmount"].FormatString = "0,0";
		grdfrm.RootTable.Columns["VoucherAmount"].AggregateFunction = (AggregateFunction)2;
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		GridBind();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
	}

	private void cmbRegisterType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DataTable dt = new DataTable();
			if (dtGrd.Rows.Count <= 0)
			{
				return;
			}
			dt.Columns.Add("RegisterType");
			dt.Columns.Add("Document Type");
			dt.Columns.Add("Voucher No");
			dt.Columns.Add("VoucherDate");
			dt.Columns.Add("RefAccountTitle");
			dt.Columns.Add("VoucherAmount");
			dt.Columns.Add("Remarks");
			dt.Columns.Add("ChequeNo");
			dt.Columns.Add("AgainstAccountTitle");
			dt.Columns.Add("EntryUser");
			dt.Columns.Add("EntryDate");
			dt.Columns.Add("Is_Approved");
			dt.Columns.Add("PostDate");
			if (((Control)(object)cmbRegisterType).Text != string.Empty)
			{
				for (int i = 0; i < dtGrd.Rows.Count; i++)
				{
					if (Convert.ToString(dtGrd.Rows[i]["RegisterType"]) == Convert.ToString(((Control)(object)cmbRegisterType).Text))
					{
						dt.Rows.Add(dtGrd.Rows[i]["RegisterType"], dtGrd.Rows[i]["V_Type"], dtGrd.Rows[i]["V_No"], Conversion.ToDateTime(dtGrd.Rows[i]["VoucherDate"]).ToString("dd-MMM-yyyy"), dtGrd.Rows[i]["RefAccountTitle"], dtGrd.Rows[i]["VoucherAmount"], dtGrd.Rows[i]["Remarks"], dtGrd.Rows[i]["ChequeNo"], dtGrd.Rows[i]["AgainstAccountTitle"], dtGrd.Rows[i]["EntryUser"], Conversion.ToDateTime(dtGrd.Rows[i]["EntryDate"]).ToString("dd-MMM-yyyy"), dtGrd.Rows[i]["Is_Approved"], Conversion.ToDateTime(dtGrd.Rows[i]["PostDate"]).ToString("dd-MMM-yyyy"));
						grdfrm.DataSource = dt;
						grdfrm.RetrieveStructure();
						gridSetting();
					}
				}
			}
			else
			{
				GridBind();
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
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.cmbRegisterType = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.txtDateTo = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbRegisterType).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(994, 481);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 95);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(994, 386);
		this.panel4.TabIndex = 2;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(994, 386);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 5;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRegisterType);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.btnshow);
		this.panel3.Controls.Add(this.txtDateFrom);
		this.panel3.Controls.Add(this.txtDateTo);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(994, 68);
		this.panel3.TabIndex = 1;
		this.cmbRegisterType.AllowNull = (DefaultableBoolean)1;
		this.cmbRegisterType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRegisterType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRegisterType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRegisterType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbRegisterType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRegisterType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRegisterType).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRegisterType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRegisterType).Location = new System.Drawing.Point(7, 37);
		((System.Windows.Forms.Control)(object)this.cmbRegisterType).Name = "cmbRegisterType";
		((System.Windows.Forms.Control)(object)this.cmbRegisterType).Size = new System.Drawing.Size(182, 26);
		((System.Windows.Forms.Control)(object)this.cmbRegisterType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbRegisterType).TextChanged += new System.EventHandler(cmbRegisterType_TextChanged);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(4, 16);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 15);
		this.label8.TabIndex = 93;
		this.label8.Text = "Register Type";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(355, 16);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(47, 15);
		this.label2.TabIndex = 92;
		this.label2.Text = "Date To";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(191, 16);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 91;
		this.label1.Text = "Date From";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(524, 39);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(64, 23);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtDateFrom.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDateFrom.Location = new System.Drawing.Point(191, 39);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(166, 23);
		this.txtDateFrom.TabIndex = 1;
		this.txtDateTo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDateTo.Location = new System.Drawing.Point(358, 39);
		this.txtDateTo.Name = "txtDateTo";
		this.txtDateTo.Size = new System.Drawing.Size(164, 23);
		this.txtDateTo.TabIndex = 2;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(994, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnRefresh, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(994, 25);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(66, 22);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(52, 22);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(994, 481);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "GeneralJournalSummeryRegister";
		this.Text = "GeneralJournalSummeryRegister";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(GeneralJournalSummeryRegister_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(GeneralJournalSummeryRegister_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbRegisterType).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
