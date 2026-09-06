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
namespace Architecture.WinApp.Inventory_Definition
{

public class DefineItemGroupUOMSchedule : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel17;

	private Label label58;

	private ToolStrip tsItemUOMSchedule;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private Panel panel1;

	internal UltraCombo cmbItemUOMScheduleUnit;

	internal UltraCombo CmbItemGroup;

	private TextBox txtItemUOMScheduleEquivalent;

	private Label label55;

	private Label label56;

	private Label label57;

	internal GridEX grdfrm;

	private Panel panel2;

	private Label History;

	private CtrlGrdBar ctrlGrdBar3;

	public DefineItemGroupUOMSchedule(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void InvDeffrmItemUomTaxSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMargin = new Size(516, 498);
			gridfill();
			ItemFill();
			ScheduleUnitFill();
			btnupdate.Visible = false;
			CmbItemGroup.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((UltraGridBase)CmbItemGroup).ActiveRow == null)
		{
			MessageBox.Show("Please Select Item Group");
			CmbItemGroup.Focus();
			return false;
		}
		if (txtItemUOMScheduleEquivalent.Text == "")
		{
			MessageBox.Show("Please Insert Equivalent");
			txtItemUOMScheduleEquivalent.Focus();
			return false;
		}
		return true;
	}

	private void gridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtgetall = ItemGroup_UOMSchedule.GetAll(new ItemGroup_UOMSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtgetall.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ItemGroupId", typeof(int));
				dt.Columns.Add("ItemGroupName", typeof(string));
				dt.Columns.Add("UOMId", typeof(string));
				dt.Columns.Add("ScheduleUnit", typeof(string));
				dt.Columns.Add("Equivalent", typeof(string));
				for (int i = 0; i < dtgetall.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtgetall.Rows[i]["Id"]), Conversion.ToInt(dtgetall.Rows[i]["ItemGroupId"]), Conversion.ToString(dtgetall.Rows[i]["ItemGroupName"]), Conversion.ToInt(dtgetall.Rows[i]["UOMId"]), Conversion.ToString(dtgetall.Rows[i]["ScheduleUnit"]), Conversion.ToString(dtgetall.Rows[i]["Equivalent"]));
				}
				grdfrm.DataSource = dt;
				grdfrm.RetrieveStructure();
				gridsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		try
		{
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["ItemGroupId"].Visible = false;
			grdfrm.RootTable.Columns["UOMId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdfrm);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = ItemGroup.GetAll(new ItemGroup
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("GroupId", typeof(int));
				dt.Columns.Add("ItemGroupName", typeof(string));
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["GroupId"], lst.Rows[i]["ItemGroupName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbItemGroup, "GroupId", "ItemGroupName", "Item Group Name", ZeroIndex: false);
					((UltraGridBase)CmbItemGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ScheduleUnitFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = UOM.Getall(new UOM
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("UOMCODE", typeof(string));
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["UOMCODE"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbItemUOMScheduleUnit, "Id", "UOMCODE", "UOMCODE", ZeroIndex: false);
					((UltraGridBase)cmbItemUOMScheduleUnit).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			txtItemUOMScheduleEquivalent.Clear();
			((Control)(object)cmbItemUOMScheduleUnit).Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			CmbItemGroup.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			ItemGroup_UOMSchedule cat = new ItemGroup_UOMSchedule();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				cat.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			cat.Equivalent = Conversion.ToString((object)txtItemUOMScheduleEquivalent.Text);
			cat.UOMId = Conversion.ToInt(cmbItemUOMScheduleUnit.Value);
			cat.ItemGroupId = Conversion.ToInt(CmbItemGroup.Value);
			cat.OrganizationId = UserAccount.OrganizationId;
			cat.CompanyId = UserAccount.CompanyId;
			ItemGroup_UOMSchedule.Save(cat);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully.");
			}
			else
			{
				MessageBox.Show("Record Save Successfully..");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ItemGroup_UOMSchedule cat = new ItemGroup_UOMSchedule();
			RecId = Conversion.ToInt((object)grdfrm.CurrentRow.Cells["Id"].Value.ToString());
			cat = ItemGroup_UOMSchedule.GetByID(RecId);
			txtItemUOMScheduleEquivalent.Text = Conversion.ToString((object)cat.Equivalent);
			cmbItemUOMScheduleUnit.Value = Conversion.ToInt((object)cat.UOMId);
			CmbItemGroup.Value = Conversion.ToInt((object)cat.ItemGroupId);
			btnsave.Visible = false;
			btnupdate.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
			((Control)(object)CmbItemGroup).Text = string.Empty;
			CmbItemGroup.Focus();
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

	private void InvDeffrmItemUomTaxSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (btnsave.Visible && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (btnupdate.Visible && e.Control && e.KeyCode == Keys.U)
			{
				btnupdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbItemGroup.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdfrm).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbItemGroup.Focus();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdfrm).Name);
			if (fs != null)
			{
				grdfrm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtItemUOMScheduleEquivalent_KeyPress(object sender, KeyPressEventArgs e)
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

	private void DefineItemGroupUOMSchedule_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.DefineItemGroupUOMSchedule));
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
		this.panel17 = new System.Windows.Forms.Panel();
		this.label58 = new System.Windows.Forms.Label();
		this.tsItemUOMSchedule = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.cmbItemUOMScheduleUnit = new UltraCombo();
		this.CmbItemGroup = new UltraCombo();
		this.txtItemUOMScheduleEquivalent = new System.Windows.Forms.TextBox();
		this.label57 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.grdfrm = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.History = new System.Windows.Forms.Label();
		this.panel17.SuspendLayout();
		this.tsItemUOMSchedule.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOMScheduleUnit).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label58);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 27);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(500, 25);
		this.panel17.TabIndex = 10;
		this.label58.AutoSize = true;
		this.label58.BackColor = System.Drawing.Color.Teal;
		this.label58.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(2, 3);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(228, 19);
		this.label58.TabIndex = 0;
		this.label58.Text = "Item Group UOM_Schedule";
		this.tsItemUOMSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemUOMSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnupdate, this.btnsave });
		this.tsItemUOMSchedule.Location = new System.Drawing.Point(0, 0);
		this.tsItemUOMSchedule.Name = "tsItemUOMSchedule";
		this.tsItemUOMSchedule.Size = new System.Drawing.Size(500, 27);
		this.tsItemUOMSchedule.TabIndex = 8;
		this.tsItemUOMSchedule.Text = "toolStrip6";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(79, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(65, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleUnit);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemGroup);
		this.panel1.Controls.Add(this.txtItemUOMScheduleEquivalent);
		this.panel1.Controls.Add(this.label57);
		this.panel1.Controls.Add(this.label55);
		this.panel1.Controls.Add(this.label56);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 52);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(500, 97);
		this.panel1.TabIndex = 14;
		this.cmbItemUOMScheduleUnit.AllowNull = (DefaultableBoolean)1;
		this.cmbItemUOMScheduleUnit.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemUOMScheduleUnit.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemUOMScheduleUnit.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemUOMScheduleUnit).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleUnit).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemUOMScheduleUnit.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleUnit).Location = new System.Drawing.Point(102, 36);
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleUnit).Name = "cmbItemUOMScheduleUnit";
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleUnit).Size = new System.Drawing.Size(307, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleUnit).TabIndex = 1;
		this.CmbItemGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbItemGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).Location = new System.Drawing.Point(102, 6);
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).Name = "CmbItemGroup";
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).Size = new System.Drawing.Size(307, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemGroup).TabIndex = 0;
		this.txtItemUOMScheduleEquivalent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemUOMScheduleEquivalent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemUOMScheduleEquivalent.Location = new System.Drawing.Point(102, 67);
		this.txtItemUOMScheduleEquivalent.Name = "txtItemUOMScheduleEquivalent";
		this.txtItemUOMScheduleEquivalent.Size = new System.Drawing.Size(307, 23);
		this.txtItemUOMScheduleEquivalent.TabIndex = 2;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(8, 71);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(62, 15);
		this.label57.TabIndex = 4;
		this.label57.Text = "Equivalent";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(8, 12);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(68, 15);
		this.label55.TabIndex = 8;
		this.label55.Text = "Item Group";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(8, 42);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(83, 15);
		this.label56.TabIndex = 6;
		this.label56.Text = "Schedule_Unit";
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 172);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.SkyBlue;
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(500, 287);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 14;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar3);
		this.panel2.Controls.Add(this.History);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 149);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(500, 23);
		this.panel2.TabIndex = 15;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(451, -1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdfrm;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar3.TabIndex = 21;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.History.AutoSize = true;
		this.History.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.History.ForeColor = System.Drawing.Color.White;
		this.History.Location = new System.Drawing.Point(13, 0);
		this.History.Name = "History";
		this.History.Size = new System.Drawing.Size(67, 19);
		this.History.TabIndex = 0;
		this.History.Text = "History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(500, 459);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel17);
		base.Controls.Add(this.tsItemUOMSchedule);
		base.KeyPreview = true;
		base.Name = "DefineItemGroupUOMSchedule";
		this.Text = "EccountBookERP";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DefineItemGroupUOMSchedule_FormClosing);
		base.Load += new System.EventHandler(InvDeffrmItemUomTaxSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvDeffrmItemUomTaxSchedule_KeyDown);
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.tsItemUOMSchedule.ResumeLayout(false);
		this.tsItemUOMSchedule.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOMScheduleUnit).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
