using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.SMS;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.SMS;
using Architecture.WinApp.Common;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace Architecture.WinApp.Configurations;

public class CreateSMSTemplate : Form
{
	private DropDownBind DDL = new DropDownBind();

	public string ScreenName = string.Empty;

	public UserAccount UserAccount = new UserAccount();

	private List<MessageBody> lst;

	private DataTable dtlstmessage = new DataTable();

	private DataTable dtTemplate = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private TabPage tabPage2;

	internal ToolStrip ToolStrip1;

	internal ToolStripButton btnNew;

	internal ToolStripButton btnSave;

	internal ToolStripSeparator toolStripSeparator1;

	internal GroupBox GroupBox1;

	internal Label Label2;

	internal TextBox txtSMSTemplate;

	internal ListBox lstParameters;

	private Panel panel5;

	private Panel panel8;

	private Panel panel7;

	private Panel panel6;

	internal ToolStrip toolStrip2;

	internal ToolStripButton btnTemRefresh;

	internal ToolStripButton btnSaveTemplate;

	internal ToolStripSeparator toolStripSeparator2;

	internal UltraCombo cmbTemplate;

	private Label label1;

	internal UltraCombo cmbGroup;

	private Label label5;

	internal TextBox txtTemplateView;

	public CreateSMSTemplate(UserAccount User_Account)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = User_Account;
	}

	private void lstParameters_DragDrop(object sender, DragEventArgs e)
	{
		lstParameters.DoDragDrop(lstParameters.SelectedItem, DragDropEffects.Copy);
	}

	private void txtSMSTemplate_DragDrop(object sender, DragEventArgs e)
	{
		txtSMSTemplate.SelectedText = e.Data.GetData(DataFormats.Text).ToString() + " ";
	}

	private void txtSMSTemplate_DragOver(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.StringFormat))
		{
			e.Effect = DragDropEffects.Copy;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void CreateSMSTemplate_Load(object sender, EventArgs e)
	{
		BindGroup();
		BindTemplate();
		txtSMSTemplate.AllowDrop = true;
		GetAll();
	}

	private void lstParameters_MouseDown(object sender, MouseEventArgs e)
	{
		lstParameters.DoDragDrop(lstParameters.SelectedItem, DragDropEffects.Copy);
	}

	private void GetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new MessageBody
			{
				ScreenName = ScreenName,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		if (txtSMSTemplate.Text == string.Empty)
		{
			MessageBox.Show("Please Enter Template Description");
			txtSMSTemplate.Focus();
			return;
		}
		MessageBody.Save(new MessageBody
		{
			ScreenName = ScreenName,
			SMSBody = txtSMSTemplate.Text,
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId,
			GroupId = Conversion.ToInt(cmbGroup.Value)
		});
		Close();
	}

	private void btnSaveTemplate_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		try
		{
			if (tblAllocateTemplateToGroup.Save(new tblAllocateTemplateToGroup
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EntryUser = UserAccount.ID,
				EntryDate = DateTime.Now,
				GroupId = Conversion.ToInt(cmbGroup.Value),
				TemplateId = Conversion.ToInt(cmbTemplate.Value)
			}) > 0)
			{
				MessageBox.Show("Save Successfully");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void RefereshCombo()
	{
		try
		{
			((Control)(object)cmbGroup).Text = string.Empty;
			((Control)(object)cmbTemplate).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGroup()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			tblReceiverGroup val = new tblReceiverGroup
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtgroup = new DataTable();
			dtgroup = tblReceiverGroup.GetAllocateGroup(val);
			if (dtgroup.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("Group");
				for (int i = 0; i < dtgroup.Rows.Count; i++)
				{
					dt.Rows.Add(dtgroup.Rows[i]["Id"], dtgroup.Rows[i]["GroupName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbGroup, "Id", "Group", "Group", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindTemplate()
	{
	}

	private void BindAllocateTemplate()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			tblAllocateTemplateToGroup toGroup = new tblAllocateTemplateToGroup();
			toGroup.OrganizationId = UserAccount.OrganizationId;
			toGroup.CompanyId = UserAccount.CompanyId;
			toGroup.GroupId = Conversion.ToInt(cmbGroup.Value);
			dtTemplate = tblAllocateTemplateToGroup.GetTemplate(toGroup);
			if (dtTemplate.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTemplate, cmbTemplate, "Id", "SMSBody", "Template", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbTemplate_TextChanged(object sender, EventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(((Control)(object)cmbTemplate).Text != string.Empty))
			{
				return;
			}
			new MessageBody();
			if (dtlstmessage.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtlstmessage.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtlstmessage.Rows[i]["Id"]) == Conversion.ToInt(cmbTemplate.Value))
				{
					txtTemplateView.Text = dtlstmessage.Rows[i]["messageBody"].ToString();
					txtTemplateView.Visible = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbGroup_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbGroup).Text != string.Empty)
			{
				BindAllocateTemplate();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Configurations.CreateSMSTemplate));
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
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.GroupBox1 = new System.Windows.Forms.GroupBox();
		this.Label2 = new System.Windows.Forms.Label();
		this.txtSMSTemplate = new System.Windows.Forms.TextBox();
		this.lstParameters = new System.Windows.Forms.ListBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.txtTemplateView = new System.Windows.Forms.TextBox();
		this.panel7 = new System.Windows.Forms.Panel();
		this.cmbTemplate = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbGroup = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnTemRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSaveTemplate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.GroupBox1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.ToolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTemplate).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGroup).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(634, 397);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(634, 397);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(626, 371);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Template";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(620, 365);
		this.panel2.TabIndex = 0;
		this.panel4.Controls.Add(this.GroupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 28);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(620, 337);
		this.panel4.TabIndex = 1;
		this.GroupBox1.Controls.Add(this.Label2);
		this.GroupBox1.Controls.Add(this.txtSMSTemplate);
		this.GroupBox1.Controls.Add(this.lstParameters);
		this.GroupBox1.Location = new System.Drawing.Point(5, 6);
		this.GroupBox1.Name = "GroupBox1";
		this.GroupBox1.Size = new System.Drawing.Size(531, 297);
		this.GroupBox1.TabIndex = 5;
		this.GroupBox1.TabStop = false;
		this.GroupBox1.Text = "Template Setting";
		this.Label2.AutoSize = true;
		this.Label2.Location = new System.Drawing.Point(340, 21);
		this.Label2.Name = "Label2";
		this.Label2.Size = new System.Drawing.Size(60, 13);
		this.Label2.TabIndex = 3;
		this.Label2.Text = "Parameters";
		this.txtSMSTemplate.Location = new System.Drawing.Point(6, 19);
		this.txtSMSTemplate.Multiline = true;
		this.txtSMSTemplate.Name = "txtSMSTemplate";
		this.txtSMSTemplate.Size = new System.Drawing.Size(331, 270);
		this.txtSMSTemplate.TabIndex = 0;
		this.txtSMSTemplate.DragDrop += new System.Windows.Forms.DragEventHandler(txtSMSTemplate_DragDrop);
		this.txtSMSTemplate.DragOver += new System.Windows.Forms.DragEventHandler(txtSMSTemplate_DragOver);
		this.lstParameters.FormattingEnabled = true;
		this.lstParameters.Items.AddRange(new object[9] { "@GPNo", "@Date", "@VehicleNo", "@BilityNo", "@ContainerNo", "@SealNo", "@DriverName", "@DriverCellNo", "@NetWeight" });
		this.lstParameters.Location = new System.Drawing.Point(343, 46);
		this.lstParameters.Name = "lstParameters";
		this.lstParameters.Size = new System.Drawing.Size(182, 238);
		this.lstParameters.TabIndex = 4;
		this.lstParameters.DragDrop += new System.Windows.Forms.DragEventHandler(lstParameters_DragDrop);
		this.lstParameters.MouseDown += new System.Windows.Forms.MouseEventHandler(lstParameters_MouseDown);
		this.panel3.Controls.Add(this.ToolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(620, 28);
		this.panel3.TabIndex = 0;
		this.ToolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.btnSave, this.toolStripSeparator1 });
		this.ToolStrip1.Location = new System.Drawing.Point(0, 0);
		this.ToolStrip1.Name = "ToolStrip1";
		this.ToolStrip1.Size = new System.Drawing.Size(620, 27);
		this.ToolStrip1.TabIndex = 4;
		this.ToolStrip1.Text = "ToolStrip1";
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(626, 371);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Template Allocate To Group";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(620, 365);
		this.panel5.TabIndex = 0;
		this.panel8.Controls.Add(this.txtTemplateView);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 77);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(620, 288);
		this.panel8.TabIndex = 2;
		this.txtTemplateView.Location = new System.Drawing.Point(11, 30);
		this.txtTemplateView.Multiline = true;
		this.txtTemplateView.Name = "txtTemplateView";
		this.txtTemplateView.Size = new System.Drawing.Size(331, 193);
		this.txtTemplateView.TabIndex = 1;
		this.txtTemplateView.Visible = false;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTemplate);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGroup);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 29);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(620, 48);
		this.panel7.TabIndex = 1;
		this.cmbTemplate.AllowNull = (DefaultableBoolean)1;
		this.cmbTemplate.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTemplate.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTemplate.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Appearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTemplate).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance9;
		((SpecialBoxBase)((UltraGridBase)this.cmbTemplate).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTemplate).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbTemplate.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTemplate).Location = new System.Drawing.Point(318, 14);
		((System.Windows.Forms.Control)(object)this.cmbTemplate).Name = "cmbTemplate";
		((System.Windows.Forms.Control)(object)this.cmbTemplate).Size = new System.Drawing.Size(294, 22);
		((System.Windows.Forms.Control)(object)this.cmbTemplate).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.cmbTemplate).TextChanged += new System.EventHandler(cmbTemplate_TextChanged);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(259, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(51, 13);
		this.label1.TabIndex = 12;
		this.label1.Text = "Template";
		this.cmbGroup.AllowNull = (DefaultableBoolean)1;
		this.cmbGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGroup).Location = new System.Drawing.Point(50, 14);
		((System.Windows.Forms.Control)(object)this.cmbGroup).Name = "cmbGroup";
		((System.Windows.Forms.Control)(object)this.cmbGroup).Size = new System.Drawing.Size(203, 22);
		((System.Windows.Forms.Control)(object)this.cmbGroup).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.cmbGroup).Leave += new System.EventHandler(cmbGroup_Leave);
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(8, 19);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(36, 13);
		this.label5.TabIndex = 10;
		this.label5.Text = "Group";
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(620, 29);
		this.panel6.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnTemRefresh, this.btnSaveTemplate, this.toolStripSeparator2 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(620, 27);
		this.toolStrip2.TabIndex = 5;
		this.toolStrip2.Text = "toolStrip2";
		this.btnTemRefresh.Image = (System.Drawing.Image)resources.GetObject("btnTemRefresh.Image");
		this.btnTemRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnTemRefresh.Name = "btnTemRefresh";
		this.btnTemRefresh.Size = new System.Drawing.Size(55, 24);
		this.btnTemRefresh.Text = "&New";
		this.btnSaveTemplate.Image = (System.Drawing.Image)resources.GetObject("btnSaveTemplate.Image");
		this.btnSaveTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveTemplate.Name = "btnSaveTemplate";
		this.btnSaveTemplate.Size = new System.Drawing.Size(55, 24);
		this.btnSaveTemplate.Text = "&Save";
		this.btnSaveTemplate.Click += new System.EventHandler(btnSaveTemplate_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(634, 397);
		base.Controls.Add(this.panel1);
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "CreateSMSTemplate";
		this.Text = "CreateSMSTemplate";
		base.Load += new System.EventHandler(CreateSMSTemplate_Load);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.GroupBox1.ResumeLayout(false);
		this.GroupBox1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.ToolStrip1.ResumeLayout(false);
		this.ToolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTemplate).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGroup).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
