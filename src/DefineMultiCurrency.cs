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
using System.Windows.Forms;
using global::Architecture.BLL;
using global::Architecture.Common;
using global::Architecture.Model;
using Janus.Windows.GridEX;

public class DefineMultiCurrency : Form
{
	private int RecId;

	private UserAccount UserAccount = new UserAccount();

	private bool UpdateMod;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel6;

	private Label label15;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripSeparator toolStripSeparator19;

	private Panel panel3;

	private GroupBox groupBox1;

	private TextBox txtCurrencyURL;

	private Label label5;

	private TextBox txtCurrencyName;

	private Label label4;

	private TextBox txtCurrencySymbol;

	private Label label1;

	private TextBox txtcurrencyRate;

	private Label label3;

	private TextBox txtCode;

	private Label label2;

	internal GridEX grdcurrency;

	public DefineMultiCurrency(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		formRefresh();
		btnupdate.Visible = false;
		btnsave.Visible = true;
	}

	private void DefineMultiCurrency_Load(object sender, EventArgs e)
	{
		BindGridCurrency();
		txtCode.Focus();
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				int success = MultiCurrency.save(new MultiCurrency
				{
					CurrencyCode = Conversion.ToString((object)txtCode.Text),
					CurrencyName = Conversion.ToString((object)txtCurrencyName.Text),
					CurrencyRate = Conversion.ToSingle((object)txtcurrencyRate.Text),
					CurrencySymbol = Conversion.ToString((object)txtCurrencySymbol.Text),
					Url = Conversion.ToString((object)txtCurrencyURL.Text),
					CurrencyStatus = true,
					EntryDate = DateTime.Now,
					EntryUser = UserAccount.ID,
					ModifyDate = DateTime.Now,
					ModifyUser = UserAccount.ID,
					PostDate = DateTime.Now,
					PostUser = 0,
					PostState = false,
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (success >= 1)
				{
					MessageBox.Show("Record Save Successfully...[" + success + "]");
				}
				BindGridCurrency();
				formRefresh();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				int success = MultiCurrency.save(new MultiCurrency
				{
					Id = RecId,
					CurrencyCode = Conversion.ToString((object)txtCode.Text),
					CurrencyName = Conversion.ToString((object)txtCurrencyName.Text),
					CurrencyRate = Conversion.ToSingle((object)txtcurrencyRate.Text),
					CurrencySymbol = Conversion.ToString((object)txtCurrencySymbol.Text),
					Url = Conversion.ToString((object)txtCurrencyURL.Text),
					CurrencyStatus = true,
					EntryDate = DateTime.Now,
					EntryUser = UserAccount.ID,
					ModifyDate = DateTime.Now,
					ModifyUser = UserAccount.ID,
					PostDate = DateTime.Now,
					PostUser = 0,
					PostState = false,
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (success >= 1)
				{
					MessageBox.Show("Record Update Successfully...[" + success + "]");
					btnsave.Visible = true;
					btnupdate.Visible = false;
				}
				BindGridCurrency();
				formRefresh();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdcurrency_DoubleClick(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnupdate.Visible = true;
			UpdateMod = true;
			MultiCurrency currency = new MultiCurrency();
			RecId = Conversion.ToInt((object)grdcurrency.CurrentRow.Cells[0].Value.ToString());
			currency = MultiCurrency.GetById(RecId);
			txtCode.Text = Conversion.ToString((object)currency.CurrencyCode);
			txtCurrencyName.Text = Conversion.ToString((object)currency.CurrencyName);
			txtcurrencyRate.Text = Conversion.ToString((object)currency.CurrencyRate);
			txtCurrencySymbol.Text = Conversion.ToString((object)currency.CurrencySymbol);
			txtCurrencyURL.Text = Conversion.ToString((object)currency.Url);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGridCurrency()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		DataTable lst = MultiCurrency.GetAll(new MultiCurrency
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId
		});
		DataTable table = new DataTable();
		table.Columns.Add("Id");
		table.Columns.Add("CurrencyCode");
		table.Columns.Add("CurrencyName");
		table.Columns.Add("CurrencyRate");
		table.Columns.Add("CurrencySymbol");
		table.Columns.Add("URL");
		table.Columns.Add("Status");
		table.Clear();
		grdcurrency.DataSource = table;
		for (int i = 0; i < lst.Rows.Count; i++)
		{
			table.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["CurrencyCode"], lst.Rows[i]["CurrencyName"], lst.Rows[i]["CurrencyRate"], lst.Rows[i]["CurrencySymbol"], lst.Rows[i]["Url"], lst.Rows[i]["CurrencyStatus"]);
		}
		grdcurrency.DataSource = table;
		grdcurrency.RetrieveStructure();
	}

	public void formRefresh()
	{
		txtCode.Focus();
		txtCode.Text = string.Empty;
		txtCurrencyName.Text = string.Empty;
		txtcurrencyRate.Text = string.Empty;
		txtCurrencySymbol.Text = string.Empty;
		txtCurrencyURL.Text = string.Empty;
		BindGridCurrency();
	}

	private bool FormValidation()
	{
		if (txtCode.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Currency Code Field Required");
			txtCode.Focus();
			return false;
		}
		if (txtCurrencyName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Currency Name Field Required");
			txtCurrencyName.Focus();
			return false;
		}
		if (txtcurrencyRate.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Currency Rate Field Required");
			txtcurrencyRate.Focus();
			return false;
		}
		return true;
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void DefineMultiCurrency_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnnew_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.S && !UpdateMod)
		{
			btnsave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.U && UpdateMod)
		{
			btnupdate_Click(null, null);
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.DefineMultiCurrency));
		GridEXLayout grdcurrency_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtCurrencyURL = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtCurrencyName = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtCurrencySymbol = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtcurrencyRate = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.grdcurrency = new GridEX();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdcurrency).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(762, 27);
		this.panel1.TabIndex = 2;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnupdate, this.btnsave, this.toolStripSeparator19 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(762, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(69, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.toolStripSeparator19.Name = "toolStripSeparator19";
		this.toolStripSeparator19.Size = new System.Drawing.Size(6, 27);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 27);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(762, 44);
		this.panel2.TabIndex = 3;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(762, 43);
		this.panel6.TabIndex = 46;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(16, 8);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(146, 24);
		this.label15.TabIndex = 0;
		this.label15.Text = "Define Currency";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 71);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(762, 122);
		this.panel3.TabIndex = 4;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.txtCurrencyURL);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtCurrencyName);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtCurrencySymbol);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtcurrencyRate);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtCode);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Location = new System.Drawing.Point(12, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(673, 112);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = " Currency";
		this.txtCurrencyURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCurrencyURL.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCurrencyURL.Location = new System.Drawing.Point(118, 81);
		this.txtCurrencyURL.Name = "txtCurrencyURL";
		this.txtCurrencyURL.Size = new System.Drawing.Size(527, 23);
		this.txtCurrencyURL.TabIndex = 4;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(12, 81);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(93, 19);
		this.label5.TabIndex = 7;
		this.label5.Text = "Currency URL";
		this.txtCurrencyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCurrencyName.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCurrencyName.Location = new System.Drawing.Point(458, 23);
		this.txtCurrencyName.Name = "txtCurrencyName";
		this.txtCurrencyName.Size = new System.Drawing.Size(187, 23);
		this.txtCurrencyName.TabIndex = 1;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(345, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(104, 19);
		this.label4.TabIndex = 5;
		this.label4.Text = "Currency Name";
		this.txtCurrencySymbol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCurrencySymbol.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCurrencySymbol.Location = new System.Drawing.Point(461, 52);
		this.txtCurrencySymbol.Name = "txtCurrencySymbol";
		this.txtCurrencySymbol.Size = new System.Drawing.Size(184, 23);
		this.txtCurrencySymbol.TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(345, 52);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(113, 19);
		this.label1.TabIndex = 5;
		this.label1.Text = "Currency Symbol";
		this.txtcurrencyRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcurrencyRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcurrencyRate.Location = new System.Drawing.Point(118, 52);
		this.txtcurrencyRate.Name = "txtcurrencyRate";
		this.txtcurrencyRate.Size = new System.Drawing.Size(220, 23);
		this.txtcurrencyRate.TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(12, 52);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(95, 19);
		this.label3.TabIndex = 3;
		this.label3.Text = "Currency Rate";
		this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCode.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCode.Location = new System.Drawing.Point(118, 23);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(220, 23);
		this.txtCode.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(12, 23);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(100, 19);
		this.label2.TabIndex = 3;
		this.label2.Text = "Currency Code";
		this.grdcurrency.AllowDelete = (InheritableBoolean)1;
		grdcurrency_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdcurrency.DesignTimeLayout = grdcurrency_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdcurrency).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdcurrency.FilterMode = (FilterMode)2;
		this.grdcurrency.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdcurrency).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdcurrency.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdcurrency).Location = new System.Drawing.Point(0, 193);
		((System.Windows.Forms.Control)(object)this.grdcurrency).Name = "grdcurrency";
		((ScrollableBase)this.grdcurrency).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdcurrency).Size = new System.Drawing.Size(762, 257);
		((System.Windows.Forms.Control)(object)this.grdcurrency).TabIndex = 5;
		this.grdcurrency.TotalRow = (InheritableBoolean)1;
		this.grdcurrency.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdcurrency).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdcurrency).DoubleClick += new System.EventHandler(grdcurrency_DoubleClick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(762, 450);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdcurrency);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DefineMultiCurrency";
		this.Text = "DefineMultiCurrency";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(DefineMultiCurrency_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineMultiCurrency_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdcurrency).EndInit();
		base.ResumeLayout(false);
	}
}

}
