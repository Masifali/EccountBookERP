using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.UserRightsManagement;

public class UserRightsEditing : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private List<tblUserRights> ModuleList = new List<tblUserRights>();

	private IContainer components;

	private Panel panel1;

	private Button BtnFetchRights;

	private Label label2;

	internal UltraCombo CmbCompanyName;

	private Label label1;

	internal UltraCombo CmbUserName;

	private Panel panel2;

	private Panel panel14;

	private ToolStrip toolStrip4;

	private ToolStripButton btnSave;

	private ToolStripSeparator toolStripSeparator8;

	private CtrlGrdBar ctrlGrdBar;

	internal GridEX grduserRights;

	public UserRightsEditing(UserAccount pUSer)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUSer;
	}

	private void UserRightsEditing_Load(object sender, EventArgs e)
	{
		try
		{
			UserNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserNameFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = UserAccount.ReadAll(new UserAccount
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbUserName, "Id", "UserName", "UserName", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbUserName_Leave(object sender, EventArgs e)
	{
		try
		{
			CompanyNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompanyNameFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			DataTable dt = UserAccountAllocation.GetAllCompaniesByUserId(new UserAccountAllocation
			{
				OrganizationId = UserAccount.OrganizationId,
				IsActive = 1,
				UserAccountId = Conversion.ToInt(CmbUserName.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCompanyName, "CompanyId", "CompName", "Company", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbUserName).ActiveRow == null)
		{
			MessageBox.Show("User Name Field is Required");
			CmbUserName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCompanyName).ActiveRow == null)
		{
			MessageBox.Show("Company Field is Required");
			CmbCompanyName.Focus();
			return false;
		}
		return true;
	}

	private void BtnFetchRights_Click(object sender, EventArgs e)
	{
		try
		{
			FormValidation();
			List<tblUserRights> lst = tblUserRights.GetbyId(Conversion.ToInt(CmbUserName.Value), Conversion.ToInt(CmbCompanyName.Value));
			GridFill(lst);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill(List<tblUserRights> lst)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("ModuleID");
			dt.Columns.Add("RightName");
			dt.Columns.Add("ModuleDescription");
			dt.Columns.Add("ScreenName");
			dt.Columns.Add("ScreenAlias");
			dt.Columns.Add("RightsID");
			dt.Columns.Add("ScreenID");
			dt.Columns.Add("Value");
			grduserRights.DataSource = null;
			dt.Clear();
			for (int i = 0; i < lst.Count; i++)
			{
				dt.Rows.Add(lst[i].ModuleID, lst[i].RightName, lst[i].ModuleDescription, lst[i].ScreenName, lst[i].ScreenAlias, lst[i].RightsID, lst[i].ScreenID, lst[i].Value);
			}
			grduserRights.DataSource = dt;
			grduserRights.RootTable.Columns["RightName"].EditType = (EditType)0;
			grduserRights.RootTable.Columns["ScreenAlias"].EditType = (EditType)0;
			grduserRights.RootTable.Columns["RightsID"].EditType = (EditType)0;
			grduserRights.RootTable.Columns["ScreenID"].EditType = (EditType)0;
			grduserRights.RootTable.Columns["Value"].EditType = (EditType)8;
			grduserRights.RootTable.Columns["RightName"].FilterEditType = (FilterEditType)1;
			grduserRights.RootTable.Columns["ScreenAlias"].FilterEditType = (FilterEditType)1;
			grduserRights.RootTable.Columns["RightsID"].FilterEditType = (FilterEditType)1;
			grduserRights.RootTable.Columns["ScreenID"].FilterEditType = (FilterEditType)1;
			grduserRights.RootTable.Columns["Value"].FilterEditType = (FilterEditType)7;
			grduserRights.RootTable.Columns["Value"].ColumnType = (ColumnType)3;
			grduserRights.RootTable.Columns["Value"].CheckBoxTrueValue = "True";
			grduserRights.RootTable.Columns["Value"].CheckBoxFalseValue = "False";
			grduserRights.RootTable.Columns["Value"].UseHeaderSelector = true;
			ctrlGrdBar_Load(null, null);
			grduserRights.CollapseGroups();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grduserRights_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grduserRights.UpdateData();
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		try
		{
			tblUserRights tblUser1 = new tblUserRights();
			tblUser1.CompanyId = UserAccount.CompanyId;
			tblUser1.UserId = Conversion.ToInt(CmbUserName.Value);
			tblUser1.TblUserRightslst = new List<tblUserRights>();
			GridEXRow[] dataRows = grduserRights.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				tblUserRights tblUserRights = new tblUserRights();
				tblUserRights.UserId = Conversion.ToInt(CmbUserName.Value);
				tblUserRights.ScreenID = Conversion.ToInt((object)r.Cells["ScreenID"].Value.ToString());
				tblUserRights.RightsID = Conversion.ToInt((object)r.Cells["RightsID"].Value.ToString());
				tblUserRights.Value = Conversion.ToBool((object)r.Cells["Value"].Value.ToString());
				tblUserRights.CompanyId = Conversion.ToInt(CmbCompanyName.Value);
				tblUser1.TblUserRightslst.Add(tblUserRights);
			}
			tblUserRights.Insert(tblUser1);
			MessageBox.Show("User Rights for " + ((Control)(object)CmbUserName).Text + " has been updated Successfully!");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grduserRights).Name);
			if (fs != null)
			{
				grduserRights.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void UserRightsEditing_FormClosed(object sender, FormClosedEventArgs e)
	{
		HistoryStack.Remove(this);
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
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Expected O, but got Unknown
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Expected O, but got Unknown
		GridEXLayout grduserRights_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.UserRightsManagement.UserRightsEditing));
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
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grduserRights = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
		this.ctrlGrdBar = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.CmbCompanyName = new UltraCombo();
		this.BtnFetchRights = new System.Windows.Forms.Button();
		this.CmbUserName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grduserRights).BeginInit();
		this.panel14.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCompanyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUserName).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grduserRights);
		this.panel1.Controls.Add(this.panel14);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(909, 596);
		this.panel1.TabIndex = 0;
		this.grduserRights.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grduserRights).BackColor = System.Drawing.Color.White;
		grduserRights_DesignTimeLayout.LayoutString = resources.GetString("grduserRights_DesignTimeLayout.LayoutString");
		this.grduserRights.DesignTimeLayout = grduserRights_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grduserRights).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grduserRights.DynamicFiltering = true;
		this.grduserRights.FilterMode = (FilterMode)2;
		this.grduserRights.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grduserRights).Font = new System.Drawing.Font("Verdana", 10.25f);
		this.grduserRights.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grduserRights).Location = new System.Drawing.Point(0, 78);
		((System.Windows.Forms.Control)(object)this.grduserRights).Name = "grduserRights";
		((ScrollableBase)this.grduserRights).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grduserRights).Size = new System.Drawing.Size(909, 518);
		((System.Windows.Forms.Control)(object)this.grduserRights).TabIndex = 48;
		this.grduserRights.TotalRow = (InheritableBoolean)1;
		this.grduserRights.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grduserRights).VisualStyle = (VisualStyle)4;
		this.grduserRights.CellValueChanged += new ColumnActionEventHandler(grduserRights_CellUpdated);
		this.grduserRights.CellEdited += new ColumnActionEventHandler(grduserRights_CellUpdated);
		this.grduserRights.CellUpdated += new ColumnActionEventHandler(grduserRights_CellUpdated);
		this.panel14.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel14.Controls.Add(this.toolStrip4);
		this.panel14.Controls.Add(this.ctrlGrdBar);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 46);
		this.panel14.Margin = new System.Windows.Forms.Padding(2);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(909, 32);
		this.panel14.TabIndex = 47;
		this.toolStrip4.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnSave, this.toolStripSeparator8 });
		this.toolStrip4.Location = new System.Drawing.Point(2, 2);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(96, 27);
		this.toolStrip4.TabIndex = 20;
		this.toolStrip4.Text = "toolStrip4";
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(78, 24);
		this.btnSave.Text = "&Update";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.toolStripSeparator8.Name = "toolStripSeparator8";
		this.toolStripSeparator8.Size = new System.Drawing.Size(6, 27);
		this.ctrlGrdBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar.FormName = this;
		this.ctrlGrdBar.Location = new System.Drawing.Point(860, 1);
		this.ctrlGrdBar.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar.MyGrid = this.grduserRights;
		this.ctrlGrdBar.Name = "ctrlGrdBar";
		this.ctrlGrdBar.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar.TabIndex = 19;
		this.ctrlGrdBar.Load += new System.EventHandler(ctrlGrdBar_Load);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCompanyName);
		this.panel2.Controls.Add(this.BtnFetchRights);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUserName);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(909, 46);
		this.panel2.TabIndex = 14;
		this.CmbCompanyName.AllowNull = (DefaultableBoolean)1;
		this.CmbCompanyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCompanyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCompanyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCompanyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Location = new System.Drawing.Point(298, 8);
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Name = "CmbCompanyName";
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Size = new System.Drawing.Size(346, 26);
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).TabIndex = 11;
		this.BtnFetchRights.BackColor = System.Drawing.Color.Teal;
		this.BtnFetchRights.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnFetchRights.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnFetchRights.ForeColor = System.Drawing.Color.White;
		this.BtnFetchRights.Location = new System.Drawing.Point(647, 8);
		this.BtnFetchRights.Name = "BtnFetchRights";
		this.BtnFetchRights.Size = new System.Drawing.Size(61, 27);
		this.BtnFetchRights.TabIndex = 13;
		this.BtnFetchRights.Text = "Fetch Rights";
		this.BtnFetchRights.UseVisualStyleBackColor = false;
		this.BtnFetchRights.Click += new System.EventHandler(BtnFetchRights_Click);
		this.CmbUserName.AllowNull = (DefaultableBoolean)1;
		this.CmbUserName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUserName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUserName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbUserName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUserName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbUserName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUserName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUserName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUserName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUserName).Location = new System.Drawing.Point(84, 8);
		((System.Windows.Forms.Control)(object)this.CmbUserName).Name = "CmbUserName";
		((System.Windows.Forms.Control)(object)this.CmbUserName).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbUserName).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbUserName).Leave += new System.EventHandler(CmbUserName_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(242, 14);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 12;
		this.label2.Text = "Company";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(14, 14);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(65, 15);
		this.label1.TabIndex = 10;
		this.label1.Text = "User Name";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(909, 596);
		base.Controls.Add(this.panel1);
		base.Name = "UserRightsEditing";
		this.Text = "dmin";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(UserRightsEditing_FormClosed);
		base.Load += new System.EventHandler(UserRightsEditing_Load);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grduserRights).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCompanyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUserName).EndInit();
		base.ResumeLayout(false);
	}
}
