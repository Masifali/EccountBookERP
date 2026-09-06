using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp
{

using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;

using UserAccount = global::Architecture.Model.UserAccount;

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Speech.Synthesis;
using System.Windows.Forms;
using global::Architecture.BLL;
using global::Architecture.Common;
using global::Architecture.Model;
// using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

public class ConfigurationsByScreen : Form
{
	private UserAccount UserAccount = new UserAccount();

	public int ScreenId;

	private DropDownBind DDL = new DropDownBind();

	private SpeechSynthesizer synthesizer = new SpeechSynthesizer();

	private IContainer components;

	private Panel panel2;

	internal UltraCombo cmbScreenName;

	private Label label3;

	private Panel PanComboUserAgainstScreenRights;

	private Panel PanMainConfigurationsByScreen;

	private Panel panel1;

	internal GridEX grdScreenRights;

	private Panel panel5;

	private GroupBox groupBox1;

	private Button btnShow;

	private Label LabRightsHeader;

	private Label label2;

	private Panel panel6;

	private Panel panel3;

	private Label label1;

	private Button btnTextToSpeek;

	private RichTextBox txtTextForSpeak;

	private Panel panel4;

	internal GridEX grdERPVideos;

	private Panel panel7;

	private Label label4;

	public ConfigurationsByScreen(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmScreenDefinetion_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(990, 500);
			if (ScreenId > 0)
			{
				PanComboUserAgainstScreenRights.Visible = false;
			}
			else
			{
				PanComboUserAgainstScreenRights.Visible = true;
				ScreenNameFill();
			}
			FillConfigurationsbyScreenId(ScreenId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ScreenNameFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt = ScreenDefinition.GetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbScreenName, "Id", "ScreenAlias", "Screen Name", true);
				((UltraGridBase)cmbScreenName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmScreenDefinetion_KeyDown(object sender, KeyEventArgs e)
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

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			ScreenId = Conversion.ToInt(cmbScreenName.Value);
			FillConfigurationsbyScreenId(ScreenId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillConfigurationsbyScreenId(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dtAll = ScreenDefinition.GetConfigrationsScreenWise(new ReportsParameters
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				ScreenId = Id
			});
			if (dtAll.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ConfigId", typeof(int));
				dt.Columns.Add("ConfigDescription");
				dt.Columns.Add("ModuleId", typeof(int));
				dt.Columns.Add("ModuleDescription");
				dt.Columns.Add("ScreenId", typeof(int));
				dt.Columns.Add("ScreenDescription");
				dt.Columns.Add("ConfigExplaination");
				dt.Columns.Add("ConfigKey", typeof(string));
				dt.Columns.Add("ConfigValue", typeof(string));
				dt.Columns.Add("IsActive", typeof(bool));
				dt.Clear();
				for (int i = 0; i < dtAll.Rows.Count; i++)
				{
					dt.Rows.Add(dtAll.Rows[i]["ConfigrationsDefinitionId"], dtAll.Rows[i]["ConfigDescription"], dtAll.Rows[i]["ModuleId"], dtAll.Rows[i]["ConfigModuleDescription"], dtAll.Rows[i]["ScreenId"], dtAll.Rows[i]["ScreenDescription"], dtAll.Rows[i]["ConfigExplaination"], dtAll.Rows[i]["ConfigKey"], dtAll.Rows[i]["ConfigValue"], dtAll.Rows[i]["IsActive"]);
				}
				grdScreenRights.DataSource = dt;
				grdScreenRights.RetrieveStructure();
				grdScreenRights.RootTable.Columns["ConfigId"].Visible = false;
				grdScreenRights.RootTable.Columns["ModuleId"].Visible = false;
				grdScreenRights.RootTable.Columns["ScreenId"].Visible = false;
				grdScreenRights.RootTable.Columns["ConfigExplaination"].Visible = false;
				CommonServices.GridAutoAdjustmentNew(grdScreenRights);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdScreenRights_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdScreenRights.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				string ConfigExplaination = Conversion.ToString(grdScreenRights.CurrentRow.Cells["ConfigExplaination"].Value);
				if (ConfigExplaination != string.Empty)
				{
					txtTextForSpeak.Text = ConfigExplaination;
				}
				ERPVideos();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ERPVideos(int Id = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			DataTable dtAll = ScreenDefinition.ERPVideosAll(new ReportsParameters
			{
				ScreenId = Id
			});
			if (dtAll.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("VideoID", typeof(int));
				dtcol.Columns.Add("VideoTitle", typeof(string));
				dtcol.Columns.Add("VideoDescription", typeof(string));
				dtcol.Columns.Add("VideoFileName", typeof(string));
				dtcol.Columns.Add("ModuleId", typeof(int));
				dtcol.Columns.Add("ScreenId", typeof(int));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtAll.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtAll.Rows[i]["VideoID"], dtAll.Rows[i]["VideoTitle"], dtAll.Rows[i]["VideoDescription"], dtAll.Rows[i]["VideoFileName"], dtAll.Rows[i]["ModuleId"], dtAll.Rows[i]["ScreenId"]);
				}
				grdERPVideos.DataSource = dtcol;
				grdERPVideos.RetrieveStructure();
				grdERPVideosSetting();
			}
			else
			{
				grdERPVideos.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdERPVideosSetting()
	{
		try
		{
			grdERPVideos.RootTable.Columns["VideoID"].Visible = false;
			grdERPVideos.RootTable.Columns["ModuleId"].Visible = false;
			grdERPVideos.RootTable.Columns["ScreenId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdERPVideos);
			grdERPVideos.RootTable.Columns.Add("Play");
			grdERPVideos.RootTable.Columns["Play"].Caption = "Play";
			grdERPVideos.RootTable.Columns["Play"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdERPVideos.RootTable.Columns["Play"].ButtonStyle = (ButtonStyle)5;
			grdERPVideos.RootTable.Columns["Play"].ButtonText = "Play";
			grdERPVideos.RootTable.Columns["Play"].Width = 50;
			grdERPVideos.RootTable.Columns["Play"].Position = 0;
			grdERPVideos.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnTextToSpeek_Click(object sender, EventArgs e)
	{
		try
		{
			string textToSpeak = txtTextForSpeak.Text;
			synthesizer.SelectVoiceByHints(VoiceGender.Female);
			synthesizer.Volume = 100;
			synthesizer.SpeakAsync(textToSpeak);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void grdERPVideos_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			_ = e.Column.Key == "Play";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PlayVideo(string videoFileName)
	{
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
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_1146: Unknown result type (might be due to invalid IL or missing references)
		//IL_1150: Expected O, but got Unknown
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
		GridEXLayout grdScreenRights_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdERPVideos_DesignTimeLayout = new GridEXLayout();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbScreenName = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.PanComboUserAgainstScreenRights = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.PanMainConfigurationsByScreen = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.txtTextForSpeak = new System.Windows.Forms.RichTextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnTextToSpeek = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdScreenRights = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.LabRightsHeader = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdERPVideos = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbScreenName).BeginInit();
		this.PanComboUserAgainstScreenRights.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.PanMainConfigurationsByScreen.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdScreenRights).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdERPVideos).BeginInit();
		this.panel7.SuspendLayout();
		base.SuspendLayout();
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(984, 33);
		this.panel2.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(213, 19);
		this.label2.TabIndex = 28;
		this.label2.Text = "Configurations By Screen";
		this.cmbScreenName.AllowNull = (DefaultableBoolean)1;
		this.cmbScreenName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbScreenName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbScreenName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbScreenName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbScreenName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbScreenName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbScreenName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbScreenName).Location = new System.Drawing.Point(112, 23);
		((System.Windows.Forms.Control)(object)this.cmbScreenName).Name = "cmbScreenName";
		((System.Windows.Forms.Control)(object)this.cmbScreenName).Size = new System.Drawing.Size(350, 26);
		((System.Windows.Forms.Control)(object)this.cmbScreenName).TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(8, 27);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(98, 19);
		this.label3.TabIndex = 12;
		this.label3.Text = "Screen Names";
		this.PanComboUserAgainstScreenRights.BackColor = System.Drawing.Color.White;
		this.PanComboUserAgainstScreenRights.Controls.Add(this.groupBox1);
		this.PanComboUserAgainstScreenRights.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanComboUserAgainstScreenRights.Location = new System.Drawing.Point(0, 33);
		this.PanComboUserAgainstScreenRights.Name = "PanComboUserAgainstScreenRights";
		this.PanComboUserAgainstScreenRights.Size = new System.Drawing.Size(984, 63);
		this.PanComboUserAgainstScreenRights.TabIndex = 1;
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbScreenName);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(8, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(847, 55);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filter Rights By Screen";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(468, 23);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(75, 27);
		this.btnShow.TabIndex = 1;
		this.btnShow.Text = "&Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.PanMainConfigurationsByScreen.Controls.Add(this.panel4);
		this.PanMainConfigurationsByScreen.Controls.Add(this.panel6);
		this.PanMainConfigurationsByScreen.Controls.Add(this.panel1);
		this.PanMainConfigurationsByScreen.Controls.Add(this.PanComboUserAgainstScreenRights);
		this.PanMainConfigurationsByScreen.Controls.Add(this.panel2);
		this.PanMainConfigurationsByScreen.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanMainConfigurationsByScreen.Location = new System.Drawing.Point(0, 0);
		this.PanMainConfigurationsByScreen.Name = "PanMainConfigurationsByScreen";
		this.PanMainConfigurationsByScreen.Size = new System.Drawing.Size(984, 461);
		this.PanMainConfigurationsByScreen.TabIndex = 3;
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.txtTextForSpeak);
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(478, 96);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(274, 365);
		this.panel6.TabIndex = 27;
		this.txtTextForSpeak.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtTextForSpeak.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTextForSpeak.Location = new System.Drawing.Point(0, 33);
		this.txtTextForSpeak.Name = "txtTextForSpeak";
		this.txtTextForSpeak.Size = new System.Drawing.Size(274, 332);
		this.txtTextForSpeak.TabIndex = 6;
		this.txtTextForSpeak.Text = "Selected Configuration Decription";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.btnTextToSpeek);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(274, 33);
		this.panel3.TabIndex = 4;
		this.btnTextToSpeek.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnTextToSpeek.BackColor = System.Drawing.Color.Teal;
		this.btnTextToSpeek.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(192, 64, 0);
		this.btnTextToSpeek.FlatAppearance.BorderSize = 10;
		this.btnTextToSpeek.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnTextToSpeek.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnTextToSpeek.ForeColor = System.Drawing.Color.White;
		this.btnTextToSpeek.Location = new System.Drawing.Point(212, 1);
		this.btnTextToSpeek.Name = "btnTextToSpeek";
		this.btnTextToSpeek.Size = new System.Drawing.Size(61, 27);
		this.btnTextToSpeek.TabIndex = 13;
		this.btnTextToSpeek.Text = "&Speak";
		this.btnTextToSpeek.UseVisualStyleBackColor = false;
		this.btnTextToSpeek.Click += new System.EventHandler(btnTextToSpeek_Click);
		this.label1.Dock = System.Windows.Forms.DockStyle.Left;
		this.label1.Font = new System.Drawing.Font("Tahoma", 8.5f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(205, 33);
		this.label1.TabIndex = 28;
		this.label1.Text = "Explainaion Of Selected Configuration With Text && Voice";
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdScreenRights);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(0, 96);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(478, 365);
		this.panel1.TabIndex = 1;
		this.grdScreenRights.AllowEdit = (InheritableBoolean)2;
		grdScreenRights_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdScreenRights.DesignTimeLayout = grdScreenRights_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdScreenRights).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdScreenRights.FilterMode = (FilterMode)2;
		this.grdScreenRights.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdScreenRights).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdScreenRights.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdScreenRights).Location = new System.Drawing.Point(0, 32);
		((System.Windows.Forms.Control)(object)this.grdScreenRights).Name = "grdScreenRights";
		((ScrollableBase)this.grdScreenRights).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdScreenRights).Size = new System.Drawing.Size(478, 333);
		((System.Windows.Forms.Control)(object)this.grdScreenRights).TabIndex = 0;
		this.grdScreenRights.TotalRow = (InheritableBoolean)1;
		this.grdScreenRights.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdScreenRights).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdScreenRights).Click += new System.EventHandler(grdScreenRights_Click);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.LabRightsHeader);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(478, 32);
		this.panel5.TabIndex = 3;
		this.LabRightsHeader.AutoSize = true;
		this.LabRightsHeader.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.LabRightsHeader.ForeColor = System.Drawing.Color.White;
		this.LabRightsHeader.Location = new System.Drawing.Point(8, 6);
		this.LabRightsHeader.Name = "LabRightsHeader";
		this.LabRightsHeader.Size = new System.Drawing.Size(239, 17);
		this.LabRightsHeader.TabIndex = 28;
		this.LabRightsHeader.Text = "Configurations Of Current Screen";
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdERPVideos);
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(752, 96);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(232, 365);
		this.panel4.TabIndex = 28;
		this.grdERPVideos.AllowEdit = (InheritableBoolean)2;
		grdERPVideos_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdERPVideos.DesignTimeLayout = grdERPVideos_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdERPVideos).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdERPVideos.FilterMode = (FilterMode)2;
		this.grdERPVideos.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdERPVideos).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdERPVideos.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdERPVideos).Location = new System.Drawing.Point(0, 32);
		((System.Windows.Forms.Control)(object)this.grdERPVideos).Name = "grdERPVideos";
		((ScrollableBase)this.grdERPVideos).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdERPVideos).Size = new System.Drawing.Size(232, 333);
		((System.Windows.Forms.Control)(object)this.grdERPVideos).TabIndex = 0;
		this.grdERPVideos.TotalRow = (InheritableBoolean)1;
		this.grdERPVideos.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdERPVideos).VisualStyle = (VisualStyle)4;
		this.grdERPVideos.ColumnButtonClick += new ColumnActionEventHandler(grdERPVideos_ColumnButtonClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(232, 32);
		this.panel7.TabIndex = 3;
		this.label4.Dock = System.Windows.Forms.DockStyle.Left;
		this.label4.Font = new System.Drawing.Font("Tahoma", 8.5f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(0, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(205, 32);
		this.label4.TabIndex = 29;
		this.label4.Text = "Explainaion Of Selected Configuration With Video";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(984, 461);
		base.Controls.Add(this.PanMainConfigurationsByScreen);
		base.KeyPreview = true;
		base.Name = "ConfigurationsByScreen";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmScreenDefinetion_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmScreenDefinetion_KeyDown);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbScreenName).EndInit();
		this.PanComboUserAgainstScreenRights.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.PanMainConfigurationsByScreen.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdScreenRights).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdERPVideos).EndInit();
		this.panel7.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
