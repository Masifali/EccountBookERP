using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Main;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Inventory_Definition;

public class InvDeffrmItemUomSchedule : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private List<GloablUomSchedule> uomList = new List<GloablUomSchedule>();

	private IContainer components;

	private Panel panel17;

	private Label label58;

	private ToolStrip tsItemUOMSchedule;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private Panel panel1;

	private TextBox txtItemUOMScheduleEquivalent;

	private Label label55;

	private Label label56;

	private Label label57;

	private Panel panel2;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo cmbItemUOMScheduleItem;

	internal UltraCombo cmbItemUOMScheduleUnit;

	private Panel panel4;

	private Panel panel3;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar2;

	private TextBox txtQtyEquivalent;

	private Label label2;

	private CheckBox chkstatus;

	private CheckBox chkActive;

	private CheckBox chkBasePackUom;

	private CheckBox chkBaseSecondaryUom;

	public InvDeffrmItemUomSchedule(UserAccount pUserAccount)
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
			base.AutoScrollMinSize = new Size(900, 568);
			gridfill();
			ItemFill();
			ScheduleUnitFill();
			cmbItemUOMScheduleItem.Focus();
			btnupdate.Visible = false;
			cmbItemUOMScheduleItem.Focus();
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
			UOMSchedule cat = new UOMSchedule();
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
			cat.OrganizationId = UserAccount.OrganizationId;
			cat.CompanyId = UserAccount.CompanyId;
			cat.EntryUser = UserAccount.ID;
			cat.ModifyUser = UserAccount.ID;
			cat.EntryDate = DateTime.Now;
			cat.ModifyDate = DateTime.Now;
			cat.Equivalent = Conversion.ToDouble((object)txtItemUOMScheduleEquivalent.Text.Trim());
			cat.QtyEquivalent = Conversion.ToDouble((object)txtQtyEquivalent.Text);
			cat.ScheduleUnitId = Conversion.ToInt(cmbItemUOMScheduleUnit.Value);
			cat.ItemId = Conversion.ToInt(cmbItemUOMScheduleItem.Value);
			cat.BaseRateUom = chkstatus.Checked;
			cat.BasePackUom = chkBasePackUom.Checked;
			cat.BaseSecondaryUom = chkBaseSecondaryUom.Checked;
			cat.Active = chkActive.Checked;
			UOMSchedule.Save(cat);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully..");
			}
			else
			{
				MessageBox.Show("Record Save Successfully..");
			}
			ResetForm();
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

	private void ItemFill()
	{
		try
		{
			DataTable dt = CommonServices.ReadAllItemsWithPackingAndStore();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbItemUOMScheduleItem, "Id", "ItemName", "Item Name", false);
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
			DataTable dt = UOM.Getall(new UOM
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtuom = new DataTable();
				dtuom.Columns.Add("Id", typeof(int));
				dtuom.Columns.Add("UOMCode", typeof(string));
				dtuom.Columns.Add("Equivalent", typeof(int));
				dtuom.Columns.Add("QtyEquivalent", typeof(int));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtuom.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["UOMCode"], dt.Rows[i]["Equivalent"], dt.Rows[i]["QtyEquivalent"]);
				}
				DDL.BindDDL(dtuom, cmbItemUOMScheduleUnit, "Id", "UOMCode", "UomCode", ZeroIndex: false);
				((UltraGridBase)cmbItemUOMScheduleUnit).DisplayLayout.Bands[0].Columns[1].Width = 120;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((Control)(object)cmbItemUOMScheduleItem).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Item");
			cmbItemUOMScheduleItem.Focus();
			return false;
		}
		if (((Control)(object)cmbItemUOMScheduleUnit).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Schedule Unit");
			cmbItemUOMScheduleUnit.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemUOMScheduleEquivalent.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Please Insert Equivalent");
			txtItemUOMScheduleEquivalent.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtQtyEquivalent.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Please Insert QtyEquivalent");
			txtQtyEquivalent.Focus();
			return false;
		}
		return true;
	}

	private DataTable CreateUomTable(IEnumerable<dynamic> data)
	{
		DataTable table = new DataTable();
		table.Columns.Add("Id", typeof(int));
		table.Columns.Add("Item", typeof(string));
		table.Columns.Add("Schedule Unit", typeof(string));
		table.Columns.Add("Equivalent", typeof(double));
		table.Columns.Add("QtyEquivalent", typeof(double));
		table.Columns.Add("BaseRateUom", typeof(bool));
		table.Columns.Add("BasePackUom", typeof(bool));
		table.Columns.Add("BaseSecondaryUom", typeof(bool));
		table.Columns.Add("Active", typeof(bool));
		foreach (dynamic item in data)
		{
			table.Rows.Add(item.Id, item.ItemName, item.UOMDescription, item.Equivalent, item.QtyEquivalent, item.BaseRateUom, item.BasePackUom, item.BaseSecondaryUom, item.Active);
		}
		return table;
	}

	private void UOMFillItemBase()
	{
		try
		{
			int itemId = Conversion.ToInt(cmbItemUOMScheduleItem.Value);
			List<GloablUomSchedule> filtered = uomList.Where((GloablUomSchedule r) => r.ItemId == itemId).ToList();
			if (filtered.Any())
			{
				grdfrm.DataSource = CreateUomTable(filtered);
				grdfrm.RetrieveStructure();
				gridsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridfill()
	{
		try
		{
			uomList.Clear();
			uomList = GlobalServicesMethods.getAllUomsByCompanyId(UserAccount.OrganizationId, UserAccount.CompanyId, 0, 0);
			List<GloablUomSchedule> data = uomList;
			if (data != null && data.Any())
			{
				grdfrm.DataSource = CreateUomTable(data);
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
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 4);
			ctrlGrdBar1_Load(null, null);
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
			UOMSchedule cat = new UOMSchedule();
			RecId = Conversion.ToInt((object)grdfrm.CurrentRow.Cells["Id"].Value.ToString());
			cat = UOMSchedule.GetByID(RecId);
			if (cat != null)
			{
				txtItemUOMScheduleEquivalent.Text = Conversion.ToString((object)cat.Equivalent);
				txtQtyEquivalent.Text = Conversion.ToString((object)cat.QtyEquivalent);
				cmbItemUOMScheduleUnit.Value = Conversion.ToInt((object)cat.ScheduleUnitId);
				cmbItemUOMScheduleItem.Value = Conversion.ToInt((object)cat.ItemId);
				chkstatus.Checked = cat.BaseRateUom;
				chkBasePackUom.Checked = cat.BasePackUom;
				chkBaseSecondaryUom.Checked = cat.BaseSecondaryUom;
				chkActive.Checked = cat.Active;
				btnupdate.Visible = true;
				btnsave.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetForm()
	{
		try
		{
			txtItemUOMScheduleEquivalent.Clear();
			txtQtyEquivalent.Clear();
			((Control)(object)cmbItemUOMScheduleUnit).Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			chkstatus.Checked = false;
			chkActive.Checked = true;
			cmbItemUOMScheduleItem.Focus();
			gridfill();
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
			ResetForm();
			((Control)(object)cmbItemUOMScheduleItem).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvDeffrmItemUomSchedule_FormClosing(object sender, FormClosingEventArgs e)
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

	private void cmbItemUOMScheduleItem_TextChanged(object sender, EventArgs e)
	{
		try
		{
			UOMFillItemBase();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemUOMScheduleUnit_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbItemUOMScheduleUnit).ActiveRow != null)
			{
				txtItemUOMScheduleEquivalent.Text = ((UltraDropDownBase)cmbItemUOMScheduleUnit).SelectedRow.Cells[2].Value.ToString();
				txtQtyEquivalent.Text = ((UltraDropDownBase)cmbItemUOMScheduleUnit).SelectedRow.Cells[3].Value.ToString();
			}
			else
			{
				txtItemUOMScheduleEquivalent.Text = "0";
				txtQtyEquivalent.Text = "0";
			}
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
				cmbItemUOMScheduleItem.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdfrm).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbItemUOMScheduleItem.Focus();
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
			MessageBox.Show(ex.Message);
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
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.InvDeffrmItemUomSchedule));
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
		this.chkActive = new System.Windows.Forms.CheckBox();
		this.chkBasePackUom = new System.Windows.Forms.CheckBox();
		this.chkstatus = new System.Windows.Forms.CheckBox();
		this.cmbItemUOMScheduleUnit = new UltraCombo();
		this.cmbItemUOMScheduleItem = new UltraCombo();
		this.txtQtyEquivalent = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtItemUOMScheduleEquivalent = new System.Windows.Forms.TextBox();
		this.label57 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.chkBaseSecondaryUom = new System.Windows.Forms.CheckBox();
		this.panel17.SuspendLayout();
		this.tsItemUOMSchedule.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOMScheduleUnit).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOMScheduleItem).BeginInit();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label58);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 27);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(884, 26);
		this.panel17.TabIndex = 10;
		this.label58.AutoSize = true;
		this.label58.BackColor = System.Drawing.Color.Teal;
		this.label58.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(6, 4);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(174, 19);
		this.label58.TabIndex = 0;
		this.label58.Text = "Item UOM_Schedule";
		this.tsItemUOMSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemUOMSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnupdate, this.btnsave });
		this.tsItemUOMSchedule.Location = new System.Drawing.Point(0, 0);
		this.tsItemUOMSchedule.Name = "tsItemUOMSchedule";
		this.tsItemUOMSchedule.Size = new System.Drawing.Size(884, 27);
		this.tsItemUOMSchedule.TabIndex = 8;
		this.tsItemUOMSchedule.Text = "toolStrip6";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.chkActive);
		this.panel1.Controls.Add(this.chkBaseSecondaryUom);
		this.panel1.Controls.Add(this.chkBasePackUom);
		this.panel1.Controls.Add(this.chkstatus);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleUnit);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleItem);
		this.panel1.Controls.Add(this.txtQtyEquivalent);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.txtItemUOMScheduleEquivalent);
		this.panel1.Controls.Add(this.label57);
		this.panel1.Controls.Add(this.label55);
		this.panel1.Controls.Add(this.label56);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 53);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(884, 55);
		this.panel1.TabIndex = 0;
		this.chkActive.AutoSize = true;
		this.chkActive.Checked = true;
		this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkActive.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkActive.Location = new System.Drawing.Point(748, 26);
		this.chkActive.Name = "chkActive";
		this.chkActive.Size = new System.Drawing.Size(59, 19);
		this.chkActive.TabIndex = 7;
		this.chkActive.Text = "Active";
		this.chkActive.UseVisualStyleBackColor = true;
		this.chkBasePackUom.AutoSize = true;
		this.chkBasePackUom.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkBasePackUom.Location = new System.Drawing.Point(635, 26);
		this.chkBasePackUom.Name = "chkBasePackUom";
		this.chkBasePackUom.Size = new System.Drawing.Size(107, 19);
		this.chkBasePackUom.TabIndex = 5;
		this.chkBasePackUom.Text = "Base Pack Uom";
		this.chkBasePackUom.UseVisualStyleBackColor = true;
		this.chkstatus.AutoSize = true;
		this.chkstatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkstatus.Location = new System.Drawing.Point(635, 6);
		this.chkstatus.Name = "chkstatus";
		this.chkstatus.Size = new System.Drawing.Size(105, 19);
		this.chkstatus.TabIndex = 4;
		this.chkstatus.Text = "Base Rate Uom";
		this.chkstatus.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleUnit).Location = new System.Drawing.Point(272, 22);
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleUnit).Name = "cmbItemUOMScheduleUnit";
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleUnit).Size = new System.Drawing.Size(154, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleUnit).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleUnit).Leave += new System.EventHandler(cmbItemUOMScheduleUnit_Leave);
		this.cmbItemUOMScheduleItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItemUOMScheduleItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemUOMScheduleItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemUOMScheduleItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemUOMScheduleItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemUOMScheduleItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleItem).Location = new System.Drawing.Point(4, 22);
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleItem).Name = "cmbItemUOMScheduleItem";
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleItem).Size = new System.Drawing.Size(266, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItemUOMScheduleItem).Leave += new System.EventHandler(cmbItemUOMScheduleItem_TextChanged);
		this.txtQtyEquivalent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyEquivalent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyEquivalent.Location = new System.Drawing.Point(529, 24);
		this.txtQtyEquivalent.Name = "txtQtyEquivalent";
		this.txtQtyEquivalent.Size = new System.Drawing.Size(100, 23);
		this.txtQtyEquivalent.TabIndex = 3;
		this.txtQtyEquivalent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemUOMScheduleEquivalent_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(529, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(84, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Qty Equivalent";
		this.txtItemUOMScheduleEquivalent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemUOMScheduleEquivalent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemUOMScheduleEquivalent.Location = new System.Drawing.Point(428, 24);
		this.txtItemUOMScheduleEquivalent.Name = "txtItemUOMScheduleEquivalent";
		this.txtItemUOMScheduleEquivalent.Size = new System.Drawing.Size(100, 23);
		this.txtItemUOMScheduleEquivalent.TabIndex = 2;
		this.txtItemUOMScheduleEquivalent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemUOMScheduleEquivalent_KeyPress);
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(428, 3);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(62, 15);
		this.label57.TabIndex = 0;
		this.label57.Text = "Equivalent";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(4, 3);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(67, 15);
		this.label55.TabIndex = 2;
		this.label55.Text = "Item Name";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(269, 3);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(83, 15);
		this.label56.TabIndex = 1;
		this.label56.Text = "Schedule_Unit";
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 108);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(884, 421);
		this.panel2.TabIndex = 15;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 26);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(884, 395);
		this.panel4.TabIndex = 16;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(884, 395);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(884, 26);
		this.panel3.TabIndex = 15;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(838, 1);
		this.ctrlGrdBar2.MyGrid = this.grdfrm;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(42, 23);
		this.ctrlGrdBar2.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "History";
		this.chkBaseSecondaryUom.AutoSize = true;
		this.chkBaseSecondaryUom.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkBaseSecondaryUom.Location = new System.Drawing.Point(748, 6);
		this.chkBaseSecondaryUom.Name = "chkBaseSecondaryUom";
		this.chkBaseSecondaryUom.Size = new System.Drawing.Size(138, 19);
		this.chkBaseSecondaryUom.TabIndex = 6;
		this.chkBaseSecondaryUom.Text = "Base Secondary Uom";
		this.chkBaseSecondaryUom.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(884, 529);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel17);
		base.Controls.Add(this.tsItemUOMSchedule);
		base.KeyPreview = true;
		base.Name = "InvDeffrmItemUomSchedule";
		this.Text = "InvDeffrmItemUomTaxSchedule";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvDeffrmItemUomSchedule_FormClosing);
		base.Load += new System.EventHandler(InvDeffrmItemUomTaxSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvDeffrmItemUomTaxSchedule_KeyDown);
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.tsItemUOMSchedule.ResumeLayout(false);
		this.tsItemUOMSchedule.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOMScheduleUnit).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOMScheduleItem).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
