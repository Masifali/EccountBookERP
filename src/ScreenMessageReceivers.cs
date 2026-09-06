using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.SMS;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.Configurations
{

public class ScreenMessageReceivers : Form
{
	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip2;

	private ToolStripButton New;

	private ToolStripButton Save;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private Panel panel6;

	private Panel panel5;

	private Button btnclose;

	private Label label23;

	internal UltraCombo ddlScreen;

	private Label label1;

	internal UltraCombo ddlModule;

	private Label label7;

	private Panel panel7;

	internal GridEX grd;

	public ScreenMessageReceivers(UserAccount User_Account)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = User_Account;
	}

	private void AppModule()
	{
		try
		{
			DataTable dt = tblUserRights.GetAllModules();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, ddlModule, "Id", "ModuleDescription", "Module", ZeroIndex: true);
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

	private void ScreenMessageReceivers_Load(object sender, EventArgs e)
	{
		try
		{
			AppModule();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ddlModule_Leave(object sender, EventArgs e)
	{
		try
		{
			DataTable dt = SMSTemplateConfigurations.GetScreens(Conversion.ToInt(ddlModule.Value));
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, ddlScreen, "Id", "ScreenAlias", "Screen Name", ZeroIndex: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridSettings()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ReferenceDocNo"].Visible = false;
			grd.RootTable.Columns["ReferenceDocTypeId"].Visible = false;
			grd.RootTable.Columns["OrganizationId"].Visible = false;
			grd.RootTable.Columns["CompanyId"].Visible = false;
			grd.RootTable.Columns["Date"].Visible = false;
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["ReferenceDocNo"].EditType = (EditType)0;
			grd.RootTable.Columns["ReferenceDocTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["OrganizationId"].EditType = (EditType)0;
			grd.RootTable.Columns["CompanyId"].EditType = (EditType)0;
			grd.RootTable.Columns["Date"].EditType = (EditType)0;
			grd.RootTable.Columns["Name"].EditType = (EditType)0;
			grd.RootTable.Columns["MobileNo"].EditType = (EditType)0;
			grd.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ReferenceDocNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ReferenceDocTypeId"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["OrganizationId"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["CompanyId"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Date"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Name"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["MobileNo"].FilterEditType = (FilterEditType)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ddlScreen_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable lstScreens = SMSTemplateConfigurations.GetScreensActiveUsers(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ScreenName = ((Control)(object)ddlScreen).Text
			});
			grd.DataSource = lstScreens;
			grd.RetrieveStructure();
			GridSettings();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Save_Click(object sender, EventArgs e)
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		try
		{
			int num = Conversion.ToInt(ddlScreen.Value);
			int Module = Conversion.ToInt(ddlModule.Value);
			if (num == 0 || Module == 0)
			{
				MessageBox.Show("Select Module and Screen");
				return;
			}
			SMSTemplateConfigurations.DeleteSMSReceivers(Conversion.ToInt(ddlScreen.Value), UserAccount.OrganizationId, UserAccount.CompanyId);
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				if (bool.Parse(r.Cells["Added"].Value.ToString()))
				{
					SMSTemplateConfigurations.SaveSMSReceivers(new SMSReceiverInsert
					{
						Screen_Id = Conversion.ToInt(ddlScreen.Value),
						ToCompanyId = int.Parse(r.Cells["Id"].Value.ToString()),
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						Date = DateTime.Now
					});
				}
			}
			MessageBox.Show("Record Saved Successfully!!!");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private new void Refresh()
	{
		try
		{
			ddlModule.Value = 0;
			ddlScreen.Value = 0;
			grd.DataSource = null;
			((Control)(object)grd).Refresh();
			grd.RetrieveStructure();
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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

	private void New_Click(object sender, EventArgs e)
	{
		Refresh();
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
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Configurations.ScreenMessageReceivers));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ddlScreen = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.ddlModule = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnclose = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.Save = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ddlScreen).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ddlModule).BeginInit();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1845, 947);
		this.panel1.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 150);
		this.panel7.Margin = new System.Windows.Forms.Padding(4);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1845, 797);
		this.panel7.TabIndex = 3;
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1845, 797);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.ddlScreen);
		this.panel6.Controls.Add(this.label1);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.ddlModule);
		this.panel6.Controls.Add(this.label7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 84);
		this.panel6.Margin = new System.Windows.Forms.Padding(4);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1845, 66);
		this.panel6.TabIndex = 2;
		this.ddlScreen.AllowNull = (DefaultableBoolean)1;
		this.ddlScreen.AutoCompleteMode = (AutoCompleteMode)4;
		this.ddlScreen.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ddlScreen.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.ddlScreen).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ddlScreen).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ddlScreen).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ddlScreen).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.ddlScreen).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ddlScreen).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.ddlScreen).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ddlScreen).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ddlScreen).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.ddlScreen).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ddlScreen).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ddlScreen).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ddlScreen).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ddlScreen.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ddlScreen).Location = new System.Drawing.Point(559, 14);
		((System.Windows.Forms.Control)(object)this.ddlScreen).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.ddlScreen).Name = "ddlScreen";
		((System.Windows.Forms.Control)(object)this.ddlScreen).Size = new System.Drawing.Size(293, 30);
		((System.Windows.Forms.Control)(object)this.ddlScreen).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.ddlScreen).Leave += new System.EventHandler(ddlScreen_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(489, 20);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(55, 18);
		this.label1.TabIndex = 4;
		this.label1.Text = "Screen";
		this.ddlModule.AllowNull = (DefaultableBoolean)1;
		this.ddlModule.AutoCompleteMode = (AutoCompleteMode)4;
		this.ddlModule.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ddlModule.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ddlModule).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.ddlModule).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ddlModule).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ddlModule).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ddlModule).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.ddlModule).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ddlModule).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.ddlModule).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ddlModule).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ddlModule).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.ddlModule).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ddlModule).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ddlModule).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ddlModule).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ddlModule.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ddlModule).Location = new System.Drawing.Point(112, 14);
		((System.Windows.Forms.Control)(object)this.ddlModule).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.ddlModule).Name = "ddlModule";
		((System.Windows.Forms.Control)(object)this.ddlModule).Size = new System.Drawing.Size(293, 30);
		((System.Windows.Forms.Control)(object)this.ddlModule).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.ddlModule).Leave += new System.EventHandler(ddlModule_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(27, 20);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 18);
		this.label7.TabIndex = 2;
		this.label7.Text = "Module";
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 38);
		this.panel4.Margin = new System.Windows.Forms.Padding(4);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1845, 46);
		this.panel4.TabIndex = 1;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.panel5.Controls.Add(this.btnclose);
		this.panel5.Controls.Add(this.label23);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1845, 44);
		this.panel5.TabIndex = 1;
		this.btnclose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnclose.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.btnclose.FlatAppearance.BorderSize = 0;
		this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnclose.Image = (System.Drawing.Image)resources.GetObject("btnclose.Image");
		this.btnclose.Location = new System.Drawing.Point(1781, 7);
		this.btnclose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnclose.Name = "btnclose";
		this.btnclose.Size = new System.Drawing.Size(59, 34);
		this.btnclose.TabIndex = 1;
		this.btnclose.UseVisualStyleBackColor = false;
		this.btnclose.Click += new System.EventHandler(btnclose_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 7);
		this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(378, 29);
		this.label23.TabIndex = 0;
		this.label23.Text = "SCREEN MESSAGE RECEIVERS";
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(4);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1845, 38);
		this.panel2.TabIndex = 0;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.toolStrip2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1845, 37);
		this.panel3.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1781, 7);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(60, 26);
		this.ctrlGrdBar1.TabIndex = 19;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.New, this.Save, this.toolStripSeparator2, this.toolStripSeparator3 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 4);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(191, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip2";
		this.New.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.New.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.New.Name = "New";
		this.New.Size = new System.Drawing.Size(63, 24);
		this.New.Text = "&New";
		this.New.Click += new System.EventHandler(New_Click);
		this.Save.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Save.Name = "Save";
		this.Save.Size = new System.Drawing.Size(64, 24);
		this.Save.Text = "&Save";
		this.Save.Click += new System.EventHandler(Save_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1845, 947);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "ScreenMessageReceivers";
		this.Text = "ScreenMessageReceivers";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ScreenMessageReceivers_Load);
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ddlScreen).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ddlModule).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
