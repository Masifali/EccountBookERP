using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp;

public class DefineProvince : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private IContainer components;

	private Label label;

	private Label label1;

	private TextBox txtProviencecode;

	private Label label23;

	private Panel panel6;

	private Panel panel5;

	private TextBox txtProvienceName;

	private ToolStripButton btnsave;

	private ToolStripButton btnnew;

	private ToolStrip toolStrip1;

	private ToolStripButton btnUpdate;

	private Panel panel4;

	internal GridEX grdProviencedefine;

	private Panel panel3;

	private Panel panel2;

	private Panel panel1;

	internal UltraCombo cmbCountry;

	private Label label2;

	public DefineProvince(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void DefineProvince_Load(object sender, EventArgs e)
	{
		CountryComboFill();
		ProvienceDefineGridFill();
	}

	private void CountryComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		try
		{
			DataTable lst = country.GetAll(new Country());
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Description");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["Description"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbCountry, "Id", "Description", "Country Name", ZeroIndex: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProvienceDefineGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		DataTable lst = StateProvince.GetAll(new StateProvince());
		DataTable dt = new DataTable();
		dt.Columns.Add("Id");
		dt.Columns.Add("Country");
		dt.Columns.Add("Province");
		dt.Columns.Add("ProvinceCode");
		grdProviencedefine.DataSource = dt;
		grdProviencedefine.RetrieveStructure();
		for (int i = 0; i < lst.Rows.Count; i++)
		{
			dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["CountryName"], lst.Rows[i]["Description"], lst.Rows[i]["Code"].ToString());
		}
		grdProviencedefine.DataSource = dt;
		grdProviencedefine.RootTable.Columns["Id"].Visible = false;
	}

	private bool FormValidation()
	{
		if (txtProviencecode.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Provience Code Field Required");
			txtProviencecode.Focus();
			return false;
		}
		if (txtProvienceName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Provience Name Field Required");
			txtProvienceName.Focus();
			return false;
		}
		if (((Control)(object)cmbCountry).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Country Field Required");
			cmbCountry.Focus();
			return false;
		}
		return true;
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				int success = StateProvince.Save(new StateProvince
				{
					Id = RecId,
					Code = Conversion.ToString((object)txtProviencecode.Text),
					Description = Conversion.ToString((object)txtProvienceName.Text),
					CountryId = Conversion.ToInt(cmbCountry.Value),
					EntryDate = Conversion.ToDateTime((object)DateTime.Now),
					ModifyDate = Conversion.ToDateTime((object)DateTime.Now),
					ModifyUser = UserAccount.ID,
					PostDate = Conversion.ToDateTime((object)DateTime.Now),
					PostUser = 0,
					PostState = false,
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (success >= 1)
				{
					MessageBox.Show("Record Update Successfully...[" + success + "]");
				}
				ProvienceDefineGridFill();
				countryformReFresh();
				btnUpdate.Visible = false;
				btnnew.Visible = true;
				btnsave.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				int success = StateProvince.Save(new StateProvince
				{
					Code = Conversion.ToString((object)txtProviencecode.Text),
					Description = Conversion.ToString((object)txtProvienceName.Text),
					CountryId = Conversion.ToInt(cmbCountry.Value),
					EntryDate = Conversion.ToDateTime((object)DateTime.Now),
					EntryUser = UserAccount.ID,
					ModifyDate = Conversion.ToDateTime((object)DateTime.Now),
					PostDate = Conversion.ToDateTime((object)DateTime.Now),
					PostUser = 0,
					PostState = false,
					OrganizationId = Conversion.ToInt((object)UserAccount.OrganizationId),
					CompanyId = Conversion.ToInt((object)UserAccount.CompanyId)
				});
				if (success >= 1)
				{
					MessageBox.Show("Record Save Successfully...[" + success + "]");
				}
				ProvienceDefineGridFill();
				countryformReFresh();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void countryformReFresh()
	{
		txtProvienceName.Text = string.Empty;
		txtProviencecode.Text = string.Empty;
		((Control)(object)cmbCountry).Text = string.Empty;
	}

	private void grdProviencedefine_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			StateProvince pro = new StateProvince();
			RecId = Conversion.ToInt((object)grdProviencedefine.CurrentRow.Cells[0].Value.ToString());
			pro = StateProvince.GetByID(RecId);
			txtProviencecode.Text = pro.Code;
			txtProvienceName.Text = pro.Description;
			cmbCountry.Value = pro.CountryId;
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

	private void btnnew_Click(object sender, EventArgs e)
	{
		btnsave.Visible = true;
		btnUpdate.Visible = false;
		countryformReFresh();
	}

	private void DefineProvince_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnnew_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.S)
		{
			btnsave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
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
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		GridEXLayout grdProviencedefine_DesignTimeLayout = new GridEXLayout();
		this.label = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtProviencecode = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.cmbCountry = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtProvienceName = new System.Windows.Forms.TextBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdProviencedefine = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCountry).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdProviencedefine).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.label.AutoSize = true;
		this.label.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label.Location = new System.Drawing.Point(399, 20);
		this.label.Name = "label";
		this.label.Size = new System.Drawing.Size(64, 17);
		this.label.TabIndex = 1;
		this.label.Text = "Provience";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(12, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(91, 17);
		this.label1.TabIndex = 1;
		this.label1.Text = "Select Country";
		this.txtProviencecode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProviencecode.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProviencecode.Location = new System.Drawing.Point(240, 42);
		this.txtProviencecode.Name = "txtProviencecode";
		this.txtProviencecode.Size = new System.Drawing.Size(151, 25);
		this.txtProviencecode.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(19, 12);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(154, 24);
		this.label23.TabIndex = 25;
		this.label23.Text = "Provience Define";
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(660, 48);
		this.panel6.TabIndex = 6;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCountry);
		this.panel5.Controls.Add(this.label);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.txtProvienceName);
		this.panel5.Controls.Add(this.txtProviencecode);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 73);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(660, 83);
		this.panel5.TabIndex = 2;
		this.cmbCountry.AllowNull = (DefaultableBoolean)1;
		this.cmbCountry.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCountry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCountry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbCountry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCountry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCountry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCountry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.cmbCountry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCountry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbCountry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCountry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbCountry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCountry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCountry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCountry).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCountry.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCountry).Location = new System.Drawing.Point(15, 41);
		((System.Windows.Forms.Control)(object)this.cmbCountry).Name = "cmbCountry";
		((System.Windows.Forms.Control)(object)this.cmbCountry).Size = new System.Drawing.Size(220, 27);
		((System.Windows.Forms.Control)(object)this.cmbCountry).TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(240, 17);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(99, 17);
		this.label2.TabIndex = 1;
		this.label2.Text = "Provience Code";
		this.txtProvienceName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProvienceName.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProvienceName.Location = new System.Drawing.Point(394, 42);
		this.txtProvienceName.Name = "txtProvienceName";
		this.txtProvienceName.Size = new System.Drawing.Size(220, 25);
		this.txtProvienceName.TabIndex = 2;
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnUpdate, this.btnsave });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(660, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdProviencedefine);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 156);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(660, 294);
		this.panel4.TabIndex = 3;
		this.grdProviencedefine.AllowDelete = (InheritableBoolean)1;
		grdProviencedefine_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdProviencedefine.DesignTimeLayout = grdProviencedefine_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdProviencedefine).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdProviencedefine.FilterMode = (FilterMode)2;
		this.grdProviencedefine.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdProviencedefine).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdProviencedefine.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdProviencedefine).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdProviencedefine).Name = "grdProviencedefine";
		((ScrollableBase)this.grdProviencedefine).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdProviencedefine).Size = new System.Drawing.Size(660, 294);
		((System.Windows.Forms.Control)(object)this.grdProviencedefine).TabIndex = 2;
		this.grdProviencedefine.TotalRow = (InheritableBoolean)1;
		this.grdProviencedefine.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdProviencedefine).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdProviencedefine).DoubleClick += new System.EventHandler(grdProviencedefine_DoubleClick);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(660, 46);
		this.panel3.TabIndex = 2;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(660, 27);
		this.panel2.TabIndex = 0;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(660, 450);
		this.panel1.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(660, 450);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DefineProvince";
		this.Text = "DefineProvince";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(DefineProvince_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineProvince_KeyDown);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCountry).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdProviencedefine).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
