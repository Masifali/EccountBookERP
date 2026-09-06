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
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp;

public class DiscountPolicyForParty : Form
{
	private int RId;

	private DropDownBind DDL = new DropDownBind();

	private DataTable lstUOM = new DataTable();

	private Rightsobjects formrights = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel8;

	private DateTimePicker dtpEffectedDate;

	private TextBox txtDiscountRate;

	private TextBox txtPolicyname;

	private Label label6;

	private Label label3;

	private Label label4;

	private Label label1;

	private Panel panel3;

	private Label label13;

	private Panel panel2;

	private ToolStrip tsPriceSchedule;

	private ToolStripButton BtnNew;

	private ToolStripButton BtnEdit;

	private ToolStripButton BtnSave;

	private BindingNavigator miniToolStrip;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel5;

	internal GridEX grd;

	internal UltraCombo cmbDiscountType;

	private CheckBox chkIsActive;

	public DiscountPolicyForParty(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private bool FormValidation()
	{
		if (txtPolicyname.Text.Trim() == "")
		{
			MessageBox.Show("Policy Name Field is Required");
			txtPolicyname.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDiscountType).ActiveRow == null)
		{
			MessageBox.Show("Discount Type Field is Required");
			cmbDiscountType.Focus();
			return false;
		}
		if (txtDiscountRate.Text.Trim() == "")
		{
			MessageBox.Show("Discount Rate Field is Required");
			txtDiscountRate.Focus();
			return false;
		}
		return true;
	}

	private void BtnSave_Click(object sender, EventArgs e)
	{
		try
		{
			RId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (RId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			DiscountPolicy DiscountPolicy = new DiscountPolicy();
			DiscountPolicy.OrganizationId = UserAccount.OrganizationId;
			DiscountPolicy.CompanyId = UserAccount.CompanyId;
			DiscountPolicy.PolicyName = txtPolicyname.Text.Trim();
			DiscountPolicy.EffectiveDate = dtpEffectedDate.Value;
			DiscountPolicy.DiscountType = ((Control)(object)cmbDiscountType).Text;
			DiscountPolicy.Id = RId;
			if (chkIsActive.Checked)
			{
				DiscountPolicy.IsActive = true;
			}
			else
			{
				DiscountPolicy.IsActive = false;
			}
			DiscountPolicy.DiscountRate = Conversion.ToDouble((object)txtDiscountRate.Text.Trim());
			DiscountPolicy.Save(DiscountPolicy);
			if (RId > 0)
			{
				MessageBox.Show("Data Update Successfully....");
			}
			else
			{
				MessageBox.Show("Data Save Successfully....");
			}
			GridFill();
			reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "ErrorMessage", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnEdit_Click(object sender, EventArgs e)
	{
		try
		{
			if (RId == 0)
			{
				throw new Exception("Record not update because Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DiscountTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DiscountType");
			dt.Rows.Add(1, "Percent");
			dt.Rows.Add(1, "Weight");
			dt.Rows.Add(1, "Flat");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbDiscountType, "Id", "DiscountType", "Discount Type", false);
				((UltraGridBase)cmbDiscountType).Rows[0].Activate();
			}
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
			txtPolicyname.Clear();
			txtDiscountRate.Clear();
			BtnSave.Visible = true;
			BtnEdit.Visible = false;
			txtPolicyname.Focus();
			chkIsActive.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = DiscountPolicy.GetAll(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("EffectedDate", typeof(DateTime));
				dt.Columns.Add("PolicyName");
				dt.Columns.Add("DiscountType");
				dt.Columns.Add("DiscountRate", typeof(double));
				dt.Columns.Add("IsActive");
				grd.DataSource = dt;
				grd.RetrieveStructure();
				((Control)(object)grd).Refresh();
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["Id"], Conversion.ToDateTime(lst.Rows[i]["EffectiveDate"]).ToShortDateString(), lst.Rows[i]["PolicyName"], lst.Rows[i]["DiscountType"], lst.Rows[i]["DiscountRate"], lst.Rows[i]["IsActive"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grd.RootTable.Columns["EffectedDate"].FormatString = "dd-mmm-yyyy";
				ctrlGrdBar1_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmPriceSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			BtnSave.Enabled = formrights.DoHaveSaveRight;
			BtnEdit.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			GridFill();
			BtnEdit.Visible = false;
			dtpEffectedDate.Focus();
			DiscountTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNew_Click(object sender, EventArgs e)
	{
		try
		{
			reset();
			GridFill();
			DiscountTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RId = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value);
			DiscountPolicy DiscountPolicy = DiscountPolicy.GetByID(RId);
			txtPolicyname.Text = DiscountPolicy.PolicyName;
			dtpEffectedDate.Value = DiscountPolicy.EffectiveDate;
			((Control)(object)cmbDiscountType).Text = DiscountPolicy.DiscountType;
			txtDiscountRate.Text = Conversion.ToString((object)DiscountPolicy.DiscountRate);
			if (DiscountPolicy.IsActive)
			{
				chkIsActive.Checked = true;
			}
			else
			{
				chkIsActive.Checked = false;
			}
			BtnSave.Visible = false;
			BtnEdit.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmPriceSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.N)
			{
				BtnNew_Click(null, null);
			}
			if (BtnSave.Visible && BtnSave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				BtnSave_Click(null, null);
			}
			if (BtnEdit.Visible && BtnEdit.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				BtnEdit_Click(null, null);
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
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

	private void txtDiscountRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void DiscountPolicyForParty_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.DiscountPolicyForParty));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.chkIsActive = new System.Windows.Forms.CheckBox();
		this.cmbDiscountType = new UltraCombo();
		this.dtpEffectedDate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.txtPolicyname = new System.Windows.Forms.TextBox();
		this.txtDiscountRate = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tsPriceSchedule = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnEdit = new System.Windows.Forms.ToolStripButton();
		this.BtnSave = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.BindingNavigator(this.components);
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDiscountType).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tsPriceSchedule.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.miniToolStrip).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1042, 694);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 151);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1042, 543);
		this.panel5.TabIndex = 3;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1042, 543);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel4.Controls.Add(this.panel8);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 74);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1042, 77);
		this.panel4.TabIndex = 2;
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add(this.chkIsActive);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDiscountType);
		this.panel8.Controls.Add(this.dtpEffectedDate);
		this.panel8.Controls.Add(this.label4);
		this.panel8.Controls.Add(this.txtPolicyname);
		this.panel8.Controls.Add(this.txtDiscountRate);
		this.panel8.Controls.Add(this.label6);
		this.panel8.Controls.Add(this.label3);
		this.panel8.Controls.Add(this.label1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1042, 77);
		this.panel8.TabIndex = 0;
		this.chkIsActive.AutoSize = true;
		this.chkIsActive.Location = new System.Drawing.Point(688, 44);
		this.chkIsActive.Name = "chkIsActive";
		this.chkIsActive.Size = new System.Drawing.Size(67, 17);
		this.chkIsActive.TabIndex = 4;
		this.chkIsActive.Text = "Is Active";
		this.chkIsActive.UseVisualStyleBackColor = true;
		this.cmbDiscountType.AllowNull = (DefaultableBoolean)1;
		this.cmbDiscountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDiscountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDiscountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDiscountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbDiscountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDiscountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Location = new System.Drawing.Point(118, 37);
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Name = "cmbDiscountType";
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Size = new System.Drawing.Size(220, 25);
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).TabIndex = 2;
		this.dtpEffectedDate.CustomFormat = "dd-MMM-yyyy";
		this.dtpEffectedDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpEffectedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dtpEffectedDate.Location = new System.Drawing.Point(118, 8);
		this.dtpEffectedDate.Name = "dtpEffectedDate";
		this.dtpEffectedDate.Size = new System.Drawing.Size(220, 23);
		this.dtpEffectedDate.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label4.Location = new System.Drawing.Point(355, 41);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(88, 17);
		this.label4.TabIndex = 15;
		this.label4.Text = "Discount Rate";
		this.txtPolicyname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPolicyname.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPolicyname.Location = new System.Drawing.Point(462, 8);
		this.txtPolicyname.Name = "txtPolicyname";
		this.txtPolicyname.Size = new System.Drawing.Size(220, 23);
		this.txtPolicyname.TabIndex = 1;
		this.txtDiscountRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiscountRate.Location = new System.Drawing.Point(462, 38);
		this.txtDiscountRate.Name = "txtDiscountRate";
		this.txtDiscountRate.Size = new System.Drawing.Size(220, 23);
		this.txtDiscountRate.TabIndex = 3;
		this.txtDiscountRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDiscountRate_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label6.Location = new System.Drawing.Point(11, 11);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(86, 17);
		this.label6.TabIndex = 17;
		this.label6.Text = "Effected Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label3.Location = new System.Drawing.Point(11, 41);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(89, 17);
		this.label3.TabIndex = 17;
		this.label3.Text = "Discount Type";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label1.Location = new System.Drawing.Point(355, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(80, 17);
		this.label1.TabIndex = 15;
		this.label1.Text = "Policy Name";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 34);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1042, 40);
		this.panel3.TabIndex = 1;
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label13.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(8, 8);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(204, 23);
		this.label13.TabIndex = 0;
		this.label13.Text = "Discount Policy for Party";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.tsPriceSchedule);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1042, 34);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(995, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 28);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tsPriceSchedule.Dock = System.Windows.Forms.DockStyle.None;
		this.tsPriceSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsPriceSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.BtnNew, this.BtnEdit, this.BtnSave });
		this.tsPriceSchedule.Location = new System.Drawing.Point(1, 4);
		this.tsPriceSchedule.Name = "tsPriceSchedule";
		this.tsPriceSchedule.Size = new System.Drawing.Size(191, 27);
		this.tsPriceSchedule.TabIndex = 3;
		this.tsPriceSchedule.Text = "toolStrip1";
		this.BtnNew.Image = (System.Drawing.Image)resources.GetObject("BtnNew.Image");
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(55, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.BtnEdit.Image = (System.Drawing.Image)resources.GetObject("BtnEdit.Image");
		this.BtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnEdit.Name = "BtnEdit";
		this.BtnEdit.Size = new System.Drawing.Size(69, 24);
		this.BtnEdit.Text = "&Update";
		this.BtnEdit.Click += new System.EventHandler(BtnEdit_Click);
		this.BtnSave.Image = (System.Drawing.Image)resources.GetObject("BtnSave.Image");
		this.BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSave.Name = "BtnSave";
		this.BtnSave.Size = new System.Drawing.Size(55, 24);
		this.BtnSave.Text = "&Save";
		this.BtnSave.Click += new System.EventHandler(BtnSave_Click);
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AddNewItem = null;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.CountItem = null;
		this.miniToolStrip.DeleteItem = null;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(283, 1);
		this.miniToolStrip.MoveFirstItem = null;
		this.miniToolStrip.MoveLastItem = null;
		this.miniToolStrip.MoveNextItem = null;
		this.miniToolStrip.MovePreviousItem = null;
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.PositionItem = null;
		this.miniToolStrip.Size = new System.Drawing.Size(1623, 27);
		this.miniToolStrip.TabIndex = 7;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1042, 694);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DiscountPolicyForParty";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DiscountPolicyForParty_FormClosing);
		base.Load += new System.EventHandler(InvfrmPriceSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPriceSchedule_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDiscountType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsPriceSchedule.ResumeLayout(false);
		this.tsPriceSchedule.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.miniToolStrip).EndInit();
		base.ResumeLayout(false);
	}
}
