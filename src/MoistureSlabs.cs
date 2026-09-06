using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
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

public class MoistureSlabs : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private IContainer components;

	private Panel ParentPanel;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private Panel panel6;

	private Label label23;

	private Panel panel5;

	private Label LblVehicleNo;

	private TextBox txtSlabCode;

	private Label label2;

	private TextBox txtSlabDescription;

	private Label label3;

	private TextBox txtMinValue;

	private Panel panel1;

	internal GridEX Grd;

	private Label label1;

	private TextBox txtMaxValue;

	private Label label4;

	private ToolStripButton btnshortcut;

	public MoistureSlabs(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void VehicleWeightLookUp_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(650, 500);
			GridFill();
			txtSlabCode.Focus();
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
			DataTable dtGrid = MoistureSlabs.ReadAll(new MoistureSlabs
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("SlabCode");
				dt.Columns.Add("SlabDescription");
				dt.Columns.Add("MinValue", typeof(double));
				dt.Columns.Add("MaxValue", typeof(double));
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dt.Rows.Add(dtGrid.Rows[i]["Id"], dtGrid.Rows[i]["MoistureSlabCode"], dtGrid.Rows[i]["MoistureSlabDescription"], dtGrid.Rows[i]["MinValue"], dtGrid.Rows[i]["MaxValue"]);
				}
				Grd.DataSource = dt;
				Grd.RetrieveStructure();
				Grd.RootTable.Columns["Id"].Visible = false;
				Grd.RootTable.Columns["SlabCode"].Width = 100;
				Grd.RootTable.Columns["SlabDescription"].Width = 200;
				Grd.RootTable.Columns["MinValue"].TextAlignment = (TextAlignment)3;
				Grd.RootTable.Columns["MinValue"].FormatString = "#,##0.###";
				Grd.RootTable.Columns["MinValue"].Width = 80;
				Grd.RootTable.Columns["MaxValue"].TextAlignment = (TextAlignment)3;
				Grd.RootTable.Columns["MaxValue"].FormatString = "#,##0.###";
				Grd.RootTable.Columns["MaxValue"].Width = 80;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = Grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				txtSlabCode.Text = Conversion.ToString(r.Cells["SlabCode"].Value);
				txtSlabDescription.Text = Conversion.ToString(r.Cells["SlabDescription"].Value);
				txtMinValue.Text = Conversion.ToString(r.Cells["MinValue"].Value);
				txtMaxValue.Text = Conversion.ToString(r.Cells["MaxValue"].Value);
				btnsave.Visible = false;
				btnUpdate.Visible = true;
			}
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
			txtSlabCode.Text = string.Empty;
			txtSlabDescription.Text = string.Empty;
			txtMinValue.Text = string.Empty;
			txtMaxValue.Text = string.Empty;
			btnUpdate.Visible = false;
			btnsave.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtSlabCode.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Moisture Slab Code Field is Required");
			txtSlabCode.Focus();
			return false;
		}
		if (txtSlabDescription.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Moisture Slab Description Field is Required");
			txtSlabDescription.Focus();
			return false;
		}
		if (txtMinValue.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Min Value Field is Required");
			txtMinValue.Focus();
			return false;
		}
		if (txtMaxValue.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Max Value Field is Required");
			txtMaxValue.Focus();
			return false;
		}
		return true;
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

	private void Insert()
	{
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (Conversion.ToDouble((object)txtMinValue.Text) == 0.0)
			{
				throw new Exception("Min Value Should Be Greater Than 0");
			}
			if (Conversion.ToDouble((object)txtMaxValue.Text) > 100.0)
			{
				throw new Exception("Max Value Should Be Less Than or Equal to 100");
			}
			if (Conversion.ToInt((object)txtMinValue.Text) > Conversion.ToInt((object)txtMaxValue.Text))
			{
				MessageBox.Show("'Min Value' Can't Be Greater Than 'Max Value'");
				return;
			}
			if (Grd.GetRows().Count() > 0)
			{
				GridEXRow[] rows = Grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Grd.CurrentRow.RowIndex != item.RowIndex && txtSlabCode.Text == Conversion.ToString(item.Cells["SlabCode"].Value))
					{
						if (Conversion.ToDouble((object)txtMinValue.Text) >= Conversion.ToDouble(item.Cells["MinValue"].Value) && Conversion.ToDouble((object)txtMaxValue.Text) <= Conversion.ToDouble(item.Cells["MaxValue"].Value))
						{
							MessageBox.Show("You Can't Add Record In Same Or Between The Values of Same Code!");
							return;
						}
						if (Conversion.ToDouble((object)txtMinValue.Text) >= Conversion.ToDouble(item.Cells["MinValue"].Value) && Conversion.ToDouble((object)txtMinValue.Text) <= Conversion.ToDouble(item.Cells["MaxValue"].Value))
						{
							MessageBox.Show("Min Value Shoud not between in Already Defined Range!");
							return;
						}
						if (Conversion.ToDouble((object)txtMaxValue.Text) >= Conversion.ToDouble(item.Cells["MinValue"].Value) && Conversion.ToDouble((object)txtMaxValue.Text) <= Conversion.ToDouble(item.Cells["MaxValue"].Value))
						{
							MessageBox.Show("Max Value Shoud not between in Already Defined Range!");
							return;
						}
					}
				}
			}
			MoistureSlabs obj = new MoistureSlabs();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.MoistureSlabCode = Conversion.ToString((object)txtSlabCode.Text);
			obj.MoistureSlabDescription = txtSlabDescription.Text;
			obj.MinValue = Conversion.ToDouble((object)txtMinValue.Text);
			obj.MaxValue = Conversion.ToDouble((object)txtMaxValue.Text);
			int success = MoistureSlabs.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully...[" + success + "]");
			}
			else
			{
				MessageBox.Show("Record Save Successfully...[" + success + "]");
			}
			Reset();
			GridFill();
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

	private void txtMinValue_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtMaxValue_KeyPress(object sender, KeyPressEventArgs e)
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

	private void VehicleWeightLookUp_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtSlabCode.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)Grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtSlabCode.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Grd_DoubleClick(null, null);
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Slab code");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Slab code");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcut_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		GridEXLayout Grd_DesignTimeLayout = new GridEXLayout();
		this.ParentPanel = new System.Windows.Forms.Panel();
		this.Grd = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.txtMaxValue = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtMinValue = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtSlabDescription = new System.Windows.Forms.TextBox();
		this.LblVehicleNo = new System.Windows.Forms.Label();
		this.txtSlabCode = new System.Windows.Forms.TextBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnshortcut = new System.Windows.Forms.ToolStripButton();
		this.ParentPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Grd).BeginInit();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.ParentPanel.BackColor = System.Drawing.Color.White;
		this.ParentPanel.Controls.Add((System.Windows.Forms.Control)(object)this.Grd);
		this.ParentPanel.Controls.Add(this.panel1);
		this.ParentPanel.Controls.Add(this.panel5);
		this.ParentPanel.Controls.Add(this.panel6);
		this.ParentPanel.Controls.Add(this.toolStrip1);
		this.ParentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ParentPanel.Location = new System.Drawing.Point(0, 0);
		this.ParentPanel.Name = "ParentPanel";
		this.ParentPanel.Size = new System.Drawing.Size(634, 461);
		this.ParentPanel.TabIndex = 0;
		this.Grd.AllowEdit = (InheritableBoolean)2;
		Grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.Grd.DesignTimeLayout = Grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.Grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.Grd.FilterMode = (FilterMode)2;
		this.Grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.Grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.Grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.Grd).Location = new System.Drawing.Point(0, 133);
		((System.Windows.Forms.Control)(object)this.Grd).Name = "Grd";
		((ScrollableBase)this.Grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.Grd).Size = new System.Drawing.Size(634, 328);
		((System.Windows.Forms.Control)(object)this.Grd).TabIndex = 1;
		this.Grd.TotalRow = (InheritableBoolean)1;
		this.Grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.Grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.Grd).DoubleClick += new System.EventHandler(Grd_DoubleClick);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 112);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(634, 21);
		this.panel1.TabIndex = 9;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 1);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 19);
		this.label4.TabIndex = 25;
		this.label4.Text = "Hsitory";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.txtMaxValue);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.txtMinValue);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.txtSlabDescription);
		this.panel5.Controls.Add(this.LblVehicleNo);
		this.panel5.Controls.Add(this.txtSlabCode);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 51);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(634, 61);
		this.panel5.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(532, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(62, 15);
		this.label1.TabIndex = 9;
		this.label1.Text = "Max Value";
		this.txtMaxValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMaxValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMaxValue.Location = new System.Drawing.Point(532, 29);
		this.txtMaxValue.Name = "txtMaxValue";
		this.txtMaxValue.Size = new System.Drawing.Size(86, 23);
		this.txtMaxValue.TabIndex = 3;
		this.txtMaxValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMaxValue_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(445, 8);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(60, 15);
		this.label3.TabIndex = 7;
		this.label3.Text = "Min Value";
		this.txtMinValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMinValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMinValue.Location = new System.Drawing.Point(444, 29);
		this.txtMinValue.Name = "txtMinValue";
		this.txtMinValue.Size = new System.Drawing.Size(86, 23);
		this.txtMinValue.TabIndex = 2;
		this.txtMinValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMinValue_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(152, 8);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(144, 15);
		this.label2.TabIndex = 4;
		this.label2.Text = "Moisture Slab Description";
		this.txtSlabDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSlabDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSlabDescription.Location = new System.Drawing.Point(150, 29);
		this.txtSlabDescription.Name = "txtSlabDescription";
		this.txtSlabDescription.Size = new System.Drawing.Size(291, 23);
		this.txtSlabDescription.TabIndex = 1;
		this.LblVehicleNo.AutoSize = true;
		this.LblVehicleNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.LblVehicleNo.Location = new System.Drawing.Point(8, 8);
		this.LblVehicleNo.Name = "LblVehicleNo";
		this.LblVehicleNo.Size = new System.Drawing.Size(110, 15);
		this.LblVehicleNo.TabIndex = 1;
		this.LblVehicleNo.Text = "Moisture Slab Code";
		this.txtSlabCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSlabCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSlabCode.Location = new System.Drawing.Point(8, 29);
		this.txtSlabCode.Name = "txtSlabCode";
		this.txtSlabCode.Size = new System.Drawing.Size(139, 23);
		this.txtSlabCode.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 27);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(634, 24);
		this.panel6.TabIndex = 7;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(120, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Moisture Slab";
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnsave, this.btnUpdate, this.btnshortcut });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(634, 27);
		this.toolStrip1.TabIndex = 4;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnshortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcut.Name = "btnshortcut";
		this.btnshortcut.Size = new System.Drawing.Size(126, 24);
		this.btnshortcut.Text = "ShortCut Keys";
		this.btnshortcut.Click += new System.EventHandler(btnshortcut_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(634, 461);
		base.Controls.Add(this.ParentPanel);
		base.KeyPreview = true;
		base.Name = "MoistureSlabs";
		this.Text = "Vehicle Weight LookUp";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(VehicleWeightLookUp_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VehicleWeightLookUp_KeyDown);
		this.ParentPanel.ResumeLayout(false);
		this.ParentPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.Grd).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
