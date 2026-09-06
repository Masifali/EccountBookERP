using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Tax_Definition;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Taxation;

public class InvfrmAddTaxSchedule : Form
{
	public bool UpdateMode;

	private int Id;

	private DataTable dt = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private int[] AryColWidth;

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabAddTaxTypeForm;

	private Panel panel4;

	private Panel panel6;

	internal GridEX HistoryGrd;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label2;

	private Panel panel2;

	private CheckBox ChkBoxIsActive;

	internal UltraCombo cmbTaxType;

	private TextBox txtTaxPercent;

	private DateTimePicker dtpEffectedDate;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label1;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnNew;

	private ToolStripButton BtnEdit;

	private ToolStripButton BtnSave;

	private ToolStrip miniToolStrip;

	private ToolStripButton btnDefineBusinessType;

	public InvfrmAddTaxSchedule(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbTaxType).ActiveRow == null)
		{
			MessageBox.Show("Tax Type Field Required");
			cmbTaxType.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtTaxPercent.Text) > 100.0)
		{
			MessageBox.Show("Tax Percent cannot be greater than 100");
			txtTaxPercent.Focus();
			return false;
		}
		if (txtTaxPercent.Text.Trim() == string.Empty || txtTaxPercent.Text.Trim() == "0")
		{
			MessageBox.Show("Tax Percent Field Required");
			txtTaxPercent.Focus();
			return false;
		}
		return true;
	}

	private void BtnSave_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				TaxScheduleMain scheduleMain = new TaxScheduleMain();
				scheduleMain.TaxNameId = Conversion.ToInt(cmbTaxType.Value);
				scheduleMain.EffectedDate = dtpEffectedDate.Value;
				scheduleMain.TaxPercent = Conversion.ToDouble((object)txtTaxPercent.Text.Trim());
				scheduleMain.EntryDate = DateTime.Now;
				scheduleMain.EntryUser = UserAccount.ID;
				scheduleMain.OrganizationId = UserAccount.OrganizationId;
				scheduleMain.CompanyId = UserAccount.CompanyId;
				if (ChkBoxIsActive.Checked)
				{
					scheduleMain.PostState = true;
				}
				else
				{
					scheduleMain.PostState = false;
				}
				scheduleMain.PostDate = DateTime.Now;
				scheduleMain.PostUser = UserAccount.ID;
				int success = TaxScheduleMain.Save(scheduleMain);
				if (success >= 1)
				{
					MessageBox.Show("Record Save Successfully...[" + success + "]");
				}
				reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnEdit_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				TaxScheduleMain scheduleMain = new TaxScheduleMain();
				scheduleMain.Id = Id;
				scheduleMain.TaxNameId = Conversion.ToInt(cmbTaxType.Value);
				scheduleMain.EffectedDate = dtpEffectedDate.Value;
				scheduleMain.TaxPercent = Conversion.ToDouble((object)txtTaxPercent.Text.Trim());
				scheduleMain.ModifyDate = DateTime.Now;
				scheduleMain.ModifyUser = UserAccount.ID;
				scheduleMain.OrganizationId = UserAccount.OrganizationId;
				scheduleMain.CompanyId = UserAccount.CompanyId;
				if (ChkBoxIsActive.Checked)
				{
					scheduleMain.PostState = true;
				}
				else
				{
					scheduleMain.PostState = false;
				}
				scheduleMain.PostDate = DateTime.Now;
				scheduleMain.PostUser = UserAccount.ID;
				int success = TaxScheduleMain.Save(scheduleMain);
				if (success >= 1)
				{
					MessageBox.Show("Record Update Successfully...[" + success + "]");
				}
				reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTaxTypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = TaxesTypes.Getall(new TaxesTypes
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dtTax = new DataTable();
			dtTax.Columns.Add("Id");
			dtTax.Columns.Add("TaxDescription");
			dtTax.Rows.Clear();
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dtTax.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["TaxDescription"]);
			}
			if (dtTax.Rows.Count > 0)
			{
				DDL.BindDDL(dtTax, cmbTaxType, "Id", "TaxDescription", "TaxDescription", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		try
		{
			TaxScheduleMain val = new TaxScheduleMain();
			DataTable dthistory = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"TaxName",
						typeof(string)
					},
					{
						"EffectedDate",
						typeof(DateTime)
					},
					{
						"TaxPercent",
						typeof(double)
					}
				}
			};
			DataTable lst = TaxScheduleMain.GetAll(val);
			if (lst.Rows.Count > 0)
			{
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dthistory.Rows.Add(Conversion.ToInt(lst.Rows[i]["Id"]), Conversion.ToString(lst.Rows[i]["TaxName"]), Convert.ToDateTime(lst.Rows[i]["EffectedDate"]).ToString("dd-MMM-yy"), Conversion.ToDouble(lst.Rows[i]["TaxPercent"]));
				}
			}
			HistoryGrd.DataSource = dthistory;
			HistoryGrd.RetrieveStructure();
			GridSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reset()
	{
		try
		{
			((Control)(object)cmbTaxType).Text = string.Empty;
			txtTaxPercent.Clear();
			HistoryGrd.ClearStructure();
			BtnSave.Visible = true;
			BtnEdit.Visible = false;
			HistoryGridFill();
			ChkBoxIsActive.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmAddTaxSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			ChkBoxIsActive.Checked = true;
			CmbTaxTypeFill();
			HistoryGridFill();
			BtnSave.Visible = true;
			BtnEdit.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void GridSettings()
	{
		try
		{
			HistoryGrd.RootTable.Columns["Id"].Visible = false;
			HistoryGrd.AllowEdit = (InheritableBoolean)2;
			HistoryGrd.AllowDelete = (InheritableBoolean)2;
			CommonServices.GridAutoAdjustment(AryColWidth, HistoryGrd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void InvfrmAddTaxSchedule_KeyDown_1(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.S && BtnSave.Visible && BtnSave.Enabled)
		{
			BtnSave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				BtnNew_Click(null, null);
			}
			else
			{
				BtnNew_Click(null, null);
			}
		}
		if (e.Control && e.KeyCode == Keys.T)
		{
			if (tabControl1.SelectedIndex == 1)
			{
				tabControl1.SelectedIndex = 0;
			}
			else
			{
				tabControl1.SelectedIndex = 1;
			}
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.U && BtnEdit.Visible && BtnEdit.Enabled)
		{
			BtnEdit_Click(null, null);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)HistoryGrd).Name);
			if (fs != null)
			{
				HistoryGrd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void HistoryGrd_DoubleClick_1(object sender, EventArgs e)
	{
		try
		{
			if (HistoryGrd.CurrentRow.RowIndex != -1)
			{
				Id = Conversion.ToInt(HistoryGrd.CurrentRow.Cells[0].Value);
				TaxScheduleMain main = TaxScheduleMain.GetByID(Id);
				cmbTaxType.Value = main.TaxNameId.ToString();
				dtpEffectedDate.Value = main.EffectedDate;
				txtTaxPercent.Text = main.TaxPercent.ToString();
				BtnSave.Visible = false;
				BtnEdit.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTaxPercent_KeyPress(object sender, KeyPressEventArgs e)
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

	private void InvfrmAddTaxSchedule_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnDefineBusinessType_Click(object sender, EventArgs e)
	{
		try
		{
			new frmTaxTypeAndBusinessTypeMapping(UserAccount).Show();
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
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		GridEXLayout HistoryGrd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Taxation.InvfrmAddTaxSchedule));
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabAddTaxTypeForm = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.HistoryGrd = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ChkBoxIsActive = new System.Windows.Forms.CheckBox();
		this.cmbTaxType = new UltraCombo();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.dtpEffectedDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnEdit = new System.Windows.Forms.ToolStripButton();
		this.BtnSave = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.btnDefineBusinessType = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabAddTaxTypeForm.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.HistoryGrd).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTaxType).BeginInit();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabAddTaxTypeForm);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 661);
		this.tabControl1.TabIndex = 1;
		this.tabAddTaxTypeForm.Controls.Add(this.panel4);
		this.tabAddTaxTypeForm.Controls.Add(this.panel2);
		this.tabAddTaxTypeForm.Controls.Add(this.panel1);
		this.tabAddTaxTypeForm.Location = new System.Drawing.Point(4, 4);
		this.tabAddTaxTypeForm.Name = "tabAddTaxTypeForm";
		this.tabAddTaxTypeForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabAddTaxTypeForm.Size = new System.Drawing.Size(956, 633);
		this.tabAddTaxTypeForm.TabIndex = 0;
		this.tabAddTaxTypeForm.Text = "Form";
		this.tabAddTaxTypeForm.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 120);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 510);
		this.panel4.TabIndex = 10;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.HistoryGrd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 31);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(950, 479);
		this.panel6.TabIndex = 4;
		this.HistoryGrd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.HistoryGrd).BackColor = System.Drawing.Color.White;
		this.HistoryGrd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		HistoryGrd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.HistoryGrd.DesignTimeLayout = HistoryGrd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.HistoryGrd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.HistoryGrd.FilterMode = (FilterMode)2;
		this.HistoryGrd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.HistoryGrd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.HistoryGrd.GroupByBoxVisible = false;
		this.HistoryGrd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.HistoryGrd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.HistoryGrd).Name = "HistoryGrd";
		((ScrollableBase)this.HistoryGrd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.HistoryGrd).Size = new System.Drawing.Size(950, 479);
		((System.Windows.Forms.Control)(object)this.HistoryGrd).TabIndex = 2;
		this.HistoryGrd.TotalRow = (InheritableBoolean)1;
		this.HistoryGrd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.HistoryGrd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.HistoryGrd).DoubleClick += new System.EventHandler(HistoryGrd_DoubleClick_1);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 31);
		this.panel3.TabIndex = 3;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(906, 2);
		this.ctrlGrdBar1.MyGrid = this.HistoryGrd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 27);
		this.ctrlGrdBar1.TabIndex = 28;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(1, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 19);
		this.label2.TabIndex = 27;
		this.label2.Text = "History";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.ChkBoxIsActive);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTaxType);
		this.panel2.Controls.Add(this.txtTaxPercent);
		this.panel2.Controls.Add(this.dtpEffectedDate);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 58);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 62);
		this.panel2.TabIndex = 8;
		this.ChkBoxIsActive.AutoSize = true;
		this.ChkBoxIsActive.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxIsActive.Location = new System.Drawing.Point(452, 34);
		this.ChkBoxIsActive.Name = "ChkBoxIsActive";
		this.ChkBoxIsActive.Size = new System.Drawing.Size(78, 21);
		this.ChkBoxIsActive.TabIndex = 8;
		this.ChkBoxIsActive.Text = "Is Active";
		this.ChkBoxIsActive.UseVisualStyleBackColor = true;
		this.cmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.cmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Location = new System.Drawing.Point(166, 33);
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Name = "cmbTaxType";
		((System.Windows.Forms.Control)(object)this.cmbTaxType).Size = new System.Drawing.Size(186, 26);
		((System.Windows.Forms.Control)(object)this.cmbTaxType).TabIndex = 1;
		this.txtTaxPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercent.Location = new System.Drawing.Point(355, 33);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.Size = new System.Drawing.Size(91, 23);
		this.txtTaxPercent.TabIndex = 2;
		this.txtTaxPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTaxPercent_KeyPress);
		this.dtpEffectedDate.CustomFormat = "dd-MMM-yyyy";
		this.dtpEffectedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpEffectedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dtpEffectedDate.Location = new System.Drawing.Point(13, 33);
		this.dtpEffectedDate.Name = "dtpEffectedDate";
		this.dtpEffectedDate.Size = new System.Drawing.Size(149, 23);
		this.dtpEffectedDate.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(358, 8);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(78, 17);
		this.label5.TabIndex = 6;
		this.label5.Text = "Tax Percent";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(163, 8);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(60, 17);
		this.label4.TabIndex = 5;
		this.label4.Text = "Tax Type";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(10, 8);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(88, 17);
		this.label3.TabIndex = 5;
		this.label3.Text = "Effected Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 30);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(155, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Add Tax Schedule";
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(950, 55);
		this.panel1.TabIndex = 6;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.BtnNew, this.BtnEdit, this.BtnSave, this.btnDefineBusinessType });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(950, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(59, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.BtnEdit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnEdit.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnEdit.Name = "BtnEdit";
		this.BtnEdit.Size = new System.Drawing.Size(78, 24);
		this.BtnEdit.Text = "&Update";
		this.BtnEdit.Click += new System.EventHandler(BtnEdit_Click);
		this.BtnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSave.Name = "BtnSave";
		this.BtnSave.Size = new System.Drawing.Size(64, 24);
		this.BtnSave.Text = "&Save";
		this.BtnSave.Click += new System.EventHandler(BtnSave_Click);
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(213, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(216, 27);
		this.miniToolStrip.TabIndex = 0;
		this.btnDefineBusinessType.Image = (System.Drawing.Image)resources.GetObject("btnDefineBusinessType.Image");
		this.btnDefineBusinessType.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineBusinessType.Name = "btnDefineBusinessType";
		this.btnDefineBusinessType.Size = new System.Drawing.Size(163, 24);
		this.btnDefineBusinessType.Text = "Tax Mapping With Business type";
		this.btnDefineBusinessType.Click += new System.EventHandler(btnDefineBusinessType_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "InvfrmAddTaxSchedule";
		this.Text = "Tax Schedule";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmAddTaxSchedule_FormClosing);
		base.Load += new System.EventHandler(InvfrmAddTaxSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmAddTaxSchedule_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabAddTaxTypeForm.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.HistoryGrd).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTaxType).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
