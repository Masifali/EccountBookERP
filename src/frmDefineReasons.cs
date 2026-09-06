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
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Lookups
{

public class frmDefineReasons : Form
{
	private UserAccount UserAccount = new UserAccount();

	private bool UpdateMood;

	private int RecId;

	public int RefDocumentTypeId;

	private IContainer components;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnnew;

	private ToolStrip toolStrip1;

	private ToolStripButton btnsave;

	private Label txtProvienceName;

	private Panel panel2;

	private Label label23;

	private TextBox txtReasonName;

	private Panel panel3;

	private Panel panel6;

	private Panel panel4;

	internal GridEX grdDefineDepartment;

	private Panel panel5;

	private Panel panel1;

	internal UltraCombo CmbRefDocumentType;

	private Label label20;

	public frmDefineReasons(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void Define_Department_Load(object sender, EventArgs e)
	{
		DocumentTypeFill();
		if (RefDocumentTypeId > 0)
		{
			CmbRefDocumentType.Value = RefDocumentTypeId;
		}
		CmbRefDocumentType.Focus();
		FormHistory();
	}

	private void DocumentTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Description", typeof(string));
			dt.Columns.Add("Code", typeof(string));
			DataTable dtDocType = DocumentType.FormHistory();
			if (dtDocType.Rows.Count > 0)
			{
				foreach (DataRow row in dtDocType.Rows)
				{
					dt.Rows.Add(row["Id"], row["DocumentTypeDescription"], row["DocumentTypeCode"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbRefDocumentType, dt, "Id", "Description", "Document Type", AllColumns: true, Conversion.ToInt(CmbRefDocumentType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			FormHistory();
			if (RefDocumentTypeId == 0)
			{
				((Control)(object)CmbRefDocumentType).Text = string.Empty;
			}
			txtReasonName.Text = string.Empty;
			CmbRefDocumentType.Focus();
			UpdateMood = false;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbRefDocumentType).ActiveRow == null || Conversion.ToInt(CmbRefDocumentType.Value) == 0)
		{
			MessageBox.Show("Reference Document Type Field Required");
			CmbRefDocumentType.Focus();
			return false;
		}
		if (txtReasonName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Reason Name Field Required");
			txtReasonName.Focus();
			return false;
		}
		return true;
	}

	private void FormHistory()
	{
		DataTable lst = Reasons.FormHistory();
		DataTable dt = new DataTable();
		dt.Columns.Add("ReasonId", typeof(int));
		dt.Columns.Add("RefDocumentTypeId", typeof(int));
		dt.Columns.Add("RefDocumentType");
		dt.Columns.Add("ReasonName");
		dt.Columns.Add("EntryDate", typeof(DateTime));
		dt.Columns.Add("EntryUser");
		dt.Columns.Add("ModifyDate", typeof(DateTime));
		dt.Columns.Add("ModifyUser");
		for (int i = 0; i < lst.Rows.Count; i++)
		{
			dt.Rows.Add(lst.Rows[i]["ReasonId"], lst.Rows[i]["RefDocumentTypeId"], lst.Rows[i]["RefDocumentType"], lst.Rows[i]["Reason"], lst.Rows[i]["EntryDate"], lst.Rows[i]["EntryUserName"], lst.Rows[i]["ModifyDate"], lst.Rows[i]["ModifyUserName"]);
		}
		grdDefineDepartment.DataSource = dt;
		grdDefineDepartment.RetrieveStructure();
		grdDefineDepartment.RootTable.Columns["ReasonId"].Visible = false;
		grdDefineDepartment.RootTable.Columns["RefDocumentTypeId"].Visible = false;
		grdDefineDepartment.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
		grdDefineDepartment.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
		grdDefineDepartment.RootTable.Columns["RefDocumentType"].Width = Constants.InventoryConstants.DocumentType;
		grdDefineDepartment.RootTable.Columns["ReasonName"].Width = Constants.InventoryConstants.StringConstant + 50;
		grdDefineDepartment.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
		grdDefineDepartment.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
		grdDefineDepartment.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
		grdDefineDepartment.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				MessageBox.Show("Record Update Successfully...[" + Reasons.Save(new Reasons
				{
					ReasonId = RecId,
					RefDocumentTypeId = Conversion.ToInt(CmbRefDocumentType.Value),
					Reason = Conversion.ToString((object)txtReasonName.Text),
					EntryDate = DateTime.Now,
					ModifyDate = DateTime.Now,
					EntryUserId = UserAccount.ID,
					ModifyUserId = UserAccount.ID
				}) + "]");
				Reset();
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
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				MessageBox.Show("Record Save Successfully...[" + Reasons.Save(new Reasons
				{
					RefDocumentTypeId = Conversion.ToInt(CmbRefDocumentType.Value),
					Reason = Conversion.ToString((object)txtReasonName.Text),
					EntryDate = DateTime.Now,
					ModifyDate = DateTime.Now,
					EntryUserId = UserAccount.ID,
					ModifyUserId = UserAccount.ID
				}) + "]");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void Define_Department_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S && !UpdateMood)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U && UpdateMood)
			{
				btnUpdate_Click(null, null);
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

	private void grdDefineCity_DoubleClick(object sender, EventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMood = true;
			RecId = Conversion.ToInt(grdDefineDepartment.CurrentRow.Cells["ReasonId"].Value);
			Reasons deprt = new Reasons();
			deprt = Reasons.GetByID(RecId);
			if (deprt != null)
			{
				CmbRefDocumentType.Value = deprt.RefDocumentTypeId;
				txtReasonName.Text = deprt.Reason;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
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
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		GridEXLayout grdDefineDepartment_DesignTimeLayout = new GridEXLayout();
		Appearance appearance2 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.txtProvienceName = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.txtReasonName = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdDefineDepartment = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.CmbRefDocumentType = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.toolStrip1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDefineDepartment).BeginInit();
		this.panel5.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocumentType).BeginInit();
		base.SuspendLayout();
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnUpdate, this.btnsave });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(612, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.txtProvienceName.AutoSize = true;
		this.txtProvienceName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProvienceName.Location = new System.Drawing.Point(8, 44);
		this.txtProvienceName.Name = "txtProvienceName";
		this.txtProvienceName.Size = new System.Drawing.Size(80, 15);
		this.txtProvienceName.TabIndex = 1;
		this.txtProvienceName.Text = "Reason Name";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(612, 27);
		this.panel2.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(135, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Define Reasons";
		this.txtReasonName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReasonName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReasonName.Location = new System.Drawing.Point(129, 38);
		this.txtReasonName.Multiline = true;
		this.txtReasonName.Name = "txtReasonName";
		this.txtReasonName.Size = new System.Drawing.Size(263, 26);
		this.txtReasonName.TabIndex = 1;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(612, 28);
		this.panel3.TabIndex = 2;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(612, 28);
		this.panel6.TabIndex = 6;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdDefineDepartment);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 123);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(612, 327);
		this.panel4.TabIndex = 3;
		this.grdDefineDepartment.AllowEdit = (InheritableBoolean)2;
		grdDefineDepartment_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDefineDepartment.DesignTimeLayout = grdDefineDepartment_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDefineDepartment.FilterMode = (FilterMode)2;
		this.grdDefineDepartment.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDefineDepartment.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).Name = "grdDefineDepartment";
		((ScrollableBase)this.grdDefineDepartment).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).Size = new System.Drawing.Size(612, 327);
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).TabIndex = 2;
		this.grdDefineDepartment.TotalRow = (InheritableBoolean)1;
		this.grdDefineDepartment.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDefineDepartment).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdDefineDepartment).DoubleClick += new System.EventHandler(grdDefineCity_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefDocumentType);
		this.panel5.Controls.Add(this.label20);
		this.panel5.Controls.Add(this.txtProvienceName);
		this.panel5.Controls.Add(this.txtReasonName);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 55);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(612, 68);
		this.panel5.TabIndex = 2;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(612, 450);
		this.panel1.TabIndex = 3;
		this.CmbRefDocumentType.AllowNull = (DefaultableBoolean)1;
		this.CmbRefDocumentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefDocumentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefDocumentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance2).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance13;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance8).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRefDocumentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Location = new System.Drawing.Point(129, 10);
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Name = "CmbRefDocumentType";
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Size = new System.Drawing.Size(263, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).TabIndex = 0;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(8, 8);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(119, 30);
		this.label20.TabIndex = 25;
		this.label20.Text = "Reference Document\r\nType";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(612, 450);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmDefineReasons";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(Define_Department_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Define_Department_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDefineDepartment).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocumentType).EndInit();
		base.ResumeLayout(false);
	}
}

}
