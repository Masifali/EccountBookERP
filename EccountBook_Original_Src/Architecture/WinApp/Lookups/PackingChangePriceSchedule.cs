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
using Architecture.WinApp.Inventory_Definition;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Lookups;

public class PackingChangePriceSchedule : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtHistory = new DataTable();

	private int RecId;

	private int UpdateIndex;

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private Panel panel2;

	private DateTimePicker EffectiveDateTo;

	private Label label2;

	private DateTimePicker EffectiveDateFrom;

	private Label Effective;

	private Label label1;

	internal UltraCombo CmbPackUom;

	private TextBox TxtPackingAddLess;

	private Label label5;

	internal GridEX Grd;

	private Button BtnCancel;

	private Button BtnSavef;

	private Button btnUpdateF;

	private Panel panel3;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label23;

	private ToolStripButton btnPartyPackingPolicy;

	public PackingChangePriceSchedule(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void PackingChangePriceSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			BackColor = Color.Teal;
			PackUomBind();
			BindHistory();
			btnUpdate.Visible = false;
			btnsave.Visible = true;
			btnUpdateF.Visible = false;
			BtnSavef.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackUomBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtPack = UOM.Getall(new UOM
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtPack.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("Name");
				for (int i = 0; i < dtPack.Rows.Count; i++)
				{
					dt.Rows.Add(dtPack.Rows[i]["Id"], dtPack.Rows[i]["UOMDescription"]);
				}
				DDL.BindDDL(dt, CmbPackUom, "Id", "Name", "Uom", ZeroIndex: false);
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[1].Width = 120;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			PackingChangePriceSchedule obj = new PackingChangePriceSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtHistory = PackingChangePriceSchedule.FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("EffectiveDateFrom");
				dt.Columns.Add("EffectiveDateTo");
				dt.Columns.Add("UomId");
				dt.Columns.Add("Equivalent");
				dt.Columns.Add("Rate");
				dt.Columns.Add("EntryDate");
				dt.Columns.Add("EntryUser");
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["Id"], Conversion.ToDateTime(dtHistory.Rows[i]["EffectiveFrom"]).ToShortDateString(), Conversion.ToDateTime(dtHistory.Rows[i]["EffectiveTo"]).ToShortDateString(), dtHistory.Rows[i]["UomId"], dtHistory.Rows[i]["Equivalent"], dtHistory.Rows[i]["PackingAddLess"], Conversion.ToDateTime(dtHistory.Rows[i]["EntryDate"]).ToShortDateString(), dtHistory.Rows[i]["EntryUser"]);
				}
				Grd.DataSource = dt;
				Grd.RetrieveStructure();
				grdSettings();
			}
			else
			{
				Grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			Grd.RootTable.Columns["Id"].Visible = false;
			Grd.RootTable.Columns["UomId"].Visible = false;
			Grd.RootTable.Columns["Equivalent"].Width = 50;
			Grd.RootTable.Columns["Rate"].Width = 50;
			Grd.RootTable.Columns["EffectiveDateFrom"].Width = 80;
			Grd.RootTable.Columns["EffectiveDateTo"].Width = 80;
			Grd.RootTable.Columns["EntryUser"].Width = 80;
			Grd.RootTable.Columns["EntryDate"].Width = 80;
			Grd.RootTable.Columns["Equivalent"].Caption = "UOM";
			Grd.RootTable.Columns["EffectiveDateFrom"].Caption = "Date From";
			Grd.RootTable.Columns["EffectiveDateTo"].Caption = "Date To";
			Grd.RootTable.Columns.Add("Edit");
			Grd.RootTable.Columns["Edit"].Key = "Edit";
			Grd.RootTable.Columns["Edit"].Caption = "Edit";
			Grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			Grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			Grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			Grd.RootTable.Columns["Edit"].Width = 40;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = Grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				RecId = Conversion.ToInt(item.Cells["Id"].Value);
				EffectiveDateFrom.Value = Conversion.ToDateTime(item.Cells["EffectiveDateFrom"].Value);
				EffectiveDateTo.Value = Conversion.ToDateTime(item.Cells["EffectiveDateTo"].Value);
				CmbPackUom.Value = Conversion.ToInt(item.Cells["UomId"].Value);
				TxtPackingAddLess.Text = Conversion.ToString(item.Cells["Rate"].Value);
				btnUpdate.Visible = true;
				btnsave.Visible = false;
				btnUpdateF.Visible = true;
				BtnSavef.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		if (e.Column.Key == "Edit")
		{
			ReadById();
		}
	}

	private bool FormValiadation()
	{
		if (((UltraGridBase)CmbPackUom).ActiveRow == null)
		{
			MessageBox.Show("Pack Uom Field Is Required");
			CmbPackUom.Focus();
			return false;
		}
		if (TxtPackingAddLess.Text == string.Empty)
		{
			MessageBox.Show("Rate Field Is Required");
			TxtPackingAddLess.Focus();
			return false;
		}
		return true;
	}

	private void btnsave_Click(object sender, EventArgs e)
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

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValiadation())
			{
				return;
			}
			PackingChangePriceSchedule obj = new PackingChangePriceSchedule();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (EffectiveDateTo.Value.Date < EffectiveDateFrom.Value.Date)
			{
				throw new Exception("Effective Date From Can't Be Greater Than Effective Date To");
			}
			if (dtHistory.Rows.Count > 0)
			{
				GridEXRow[] rows = Grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (RecId != Conversion.ToInt(item.Cells["Id"].Value) && Conversion.ToInt(CmbPackUom.Value) == Conversion.ToInt(item.Cells["UomId"].Value))
					{
						if (Conversion.ToDateTime(item.Cells["EffectiveDateFrom"].Value).Date <= EffectiveDateFrom.Value.Date && EffectiveDateFrom.Value.Date <= Conversion.ToDateTime(item.Cells["EffectiveDateTo"].Value).Date)
						{
							throw new Exception("Effective Date From Of Uom " + ((Control)(object)CmbPackUom).Text + " Is Between The Range Of Already Defined Uom Schedule");
						}
						if (Conversion.ToDateTime(item.Cells["EffectiveDateFrom"].Value).Date <= EffectiveDateTo.Value.Date && EffectiveDateTo.Value.Date <= Conversion.ToDateTime(item.Cells["EffectiveDateTo"].Value).Date)
						{
							throw new Exception("Effective Date To Of Uom " + ((Control)(object)CmbPackUom).Text + " Is Between The Range Of Already Defined Uom Schedule");
						}
					}
				}
			}
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchId = UserAccount.BranchesId;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EffectiveFrom = EffectiveDateFrom.Value;
			obj.EffectiveTo = EffectiveDateTo.Value;
			obj.UomId = Conversion.ToInt(CmbPackUom.Value);
			obj.Equivalent = Conversion.ToDouble((object)((Control)(object)CmbPackUom).Text);
			if (Conversion.ToDouble((object)TxtPackingAddLess.Text) > 500.0)
			{
				throw new Exception("The Rate (PAcking Add Less Should Not Be Greater Than 500)");
			}
			obj.PackingAddLess = Conversion.ToDouble((object)TxtPackingAddLess.Text);
			int Id = PackingChangePriceSchedule.Save(obj);
			if (RecId == 0 && Id > 0)
			{
				MessageBox.Show("Save Successfully");
			}
			else if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			Reset();
			BindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
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

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
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
			EffectiveDateFrom.Focus();
			((Control)(object)CmbPackUom).Text = string.Empty;
			TxtPackingAddLess.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			BtnSavef.Visible = true;
			btnUpdateF.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void TxtPackingAddLess_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)Grd).Name);
			if (fs != null)
			{
				Grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPartyPackingPolicy_Click(object sender, EventArgs e)
	{
		try
		{
			frmSupplierCustomerUnAllocateFromDiscounPolicyForPackingCharges frmSupplierCustomerUnAllocateFromDiscounPolicyForPackingCharges = new frmSupplierCustomerUnAllocateFromDiscounPolicyForPackingCharges(UserAccount);
			frmSupplierCustomerUnAllocateFromDiscounPolicyForPackingCharges.ShowDialog();
			frmSupplierCustomerUnAllocateFromDiscounPolicyForPackingCharges.BringToFront();
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
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Expected O, but got Unknown
		GridEXLayout Grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lookups.PackingChangePriceSchedule));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.Grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnUpdateF = new System.Windows.Forms.Button();
		this.BtnCancel = new System.Windows.Forms.Button();
		this.BtnSavef = new System.Windows.Forms.Button();
		this.TxtPackingAddLess = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.EffectiveDateTo = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.EffectiveDateFrom = new System.Windows.Forms.DateTimePicker();
		this.Effective = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnPartyPackingPolicy = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Grd).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(479, 594);
		this.panel1.TabIndex = 0;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.Grd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 217);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(479, 377);
		this.panel3.TabIndex = 10;
		this.Grd.AllowEdit = (InheritableBoolean)2;
		this.Grd.ColumnAutoResize = true;
		Grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.Grd.DesignTimeLayout = Grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.Grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.Grd.DynamicFiltering = true;
		this.Grd.FilterMode = (FilterMode)2;
		this.Grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.Grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.Grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.Grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.Grd).Name = "Grd";
		((ScrollableBase)this.Grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.Grd).Size = new System.Drawing.Size(479, 377);
		((System.Windows.Forms.Control)(object)this.Grd).TabIndex = 8;
		this.Grd.TotalRow = (InheritableBoolean)1;
		this.Grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.Grd).VisualStyle = (VisualStyle)4;
		this.Grd.ColumnButtonClick += new ColumnActionEventHandler(Grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.Grd).DoubleClick += new System.EventHandler(Grd_DoubleClick);
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 186);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(479, 31);
		this.panel6.TabIndex = 9;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(430, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.Grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 26;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(273, 24);
		this.label23.TabIndex = 25;
		this.label23.Text = "Packing Price Schedule History";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.btnUpdateF);
		this.panel2.Controls.Add(this.BtnCancel);
		this.panel2.Controls.Add(this.BtnSavef);
		this.panel2.Controls.Add(this.TxtPackingAddLess);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.panel2.Controls.Add(this.EffectiveDateTo);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.EffectiveDateFrom);
		this.panel2.Controls.Add(this.Effective);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 27);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(479, 159);
		this.panel2.TabIndex = 1;
		this.btnUpdateF.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateF.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateF.ForeColor = System.Drawing.Color.White;
		this.btnUpdateF.Location = new System.Drawing.Point(346, 124);
		this.btnUpdateF.Name = "btnUpdateF";
		this.btnUpdateF.Size = new System.Drawing.Size(75, 28);
		this.btnUpdateF.TabIndex = 602;
		this.btnUpdateF.Text = "Update";
		this.btnUpdateF.UseVisualStyleBackColor = false;
		this.btnUpdateF.Visible = false;
		this.btnUpdateF.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnCancel.BackColor = System.Drawing.Color.Teal;
		this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancel.ForeColor = System.Drawing.Color.White;
		this.BtnCancel.Location = new System.Drawing.Point(267, 124);
		this.BtnCancel.Name = "BtnCancel";
		this.BtnCancel.Size = new System.Drawing.Size(75, 28);
		this.BtnCancel.TabIndex = 601;
		this.BtnCancel.Text = "Cancel";
		this.BtnCancel.UseVisualStyleBackColor = false;
		this.BtnCancel.Click += new System.EventHandler(BtnCancel_Click);
		this.BtnSavef.BackColor = System.Drawing.Color.Teal;
		this.BtnSavef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSavef.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSavef.ForeColor = System.Drawing.Color.White;
		this.BtnSavef.Location = new System.Drawing.Point(346, 124);
		this.BtnSavef.Name = "BtnSavef";
		this.BtnSavef.Size = new System.Drawing.Size(75, 28);
		this.BtnSavef.TabIndex = 600;
		this.BtnSavef.Text = "Save";
		this.BtnSavef.UseVisualStyleBackColor = false;
		this.BtnSavef.Click += new System.EventHandler(btnsave_Click);
		this.TxtPackingAddLess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.TxtPackingAddLess.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.TxtPackingAddLess.Location = new System.Drawing.Point(149, 95);
		this.TxtPackingAddLess.Name = "TxtPackingAddLess";
		this.TxtPackingAddLess.Size = new System.Drawing.Size(272, 23);
		this.TxtPackingAddLess.TabIndex = 3;
		this.TxtPackingAddLess.KeyPress += new System.Windows.Forms.KeyPressEventHandler(TxtPackingAddLess_KeyPress);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(46, 99);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(35, 17);
		this.label5.TabIndex = 599;
		this.label5.Text = "Rate";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(46, 69);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(69, 17);
		this.label1.TabIndex = 597;
		this.label1.Text = "Pack Uom";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(149, 64);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(272, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 2;
		this.EffectiveDateTo.CustomFormat = "dd-MMM-yy";
		this.EffectiveDateTo.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.EffectiveDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.EffectiveDateTo.Location = new System.Drawing.Point(149, 36);
		this.EffectiveDateTo.Name = "EffectiveDateTo";
		this.EffectiveDateTo.Size = new System.Drawing.Size(272, 23);
		this.EffectiveDateTo.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(46, 39);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(76, 17);
		this.label2.TabIndex = 595;
		this.label2.Text = "Effective To";
		this.EffectiveDateFrom.CustomFormat = "dd-MMM-yy";
		this.EffectiveDateFrom.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.EffectiveDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.EffectiveDateFrom.Location = new System.Drawing.Point(149, 8);
		this.EffectiveDateFrom.Name = "EffectiveDateFrom";
		this.EffectiveDateFrom.Size = new System.Drawing.Size(272, 23);
		this.EffectiveDateFrom.TabIndex = 0;
		this.Effective.AutoSize = true;
		this.Effective.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Effective.Location = new System.Drawing.Point(46, 11);
		this.Effective.Name = "Effective";
		this.Effective.Size = new System.Drawing.Size(94, 17);
		this.Effective.TabIndex = 594;
		this.Effective.Text = "Effective From";
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnsave, this.btnUpdate, this.btnPartyPackingPolicy });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(479, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnPartyPackingPolicy.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPartyPackingPolicy.Name = "btnPartyPackingPolicy";
		this.btnPartyPackingPolicy.Size = new System.Drawing.Size(126, 24);
		this.btnPartyPackingPolicy.Text = "Party Packing Policy";
		this.btnPartyPackingPolicy.Click += new System.EventHandler(btnPartyPackingPolicy_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(479, 594);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "PackingChangePriceSchedule";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Packing Change Price Schedule";
		base.Load += new System.EventHandler(PackingChangePriceSchedule_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.Grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
