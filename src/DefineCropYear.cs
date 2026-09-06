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
using System.IO;
using System.Windows.Forms;
using global::Architecture.BLL.Inventory;
using global::Architecture.Common;
using global::Architecture.Model;
using global::Architecture.Model.Inventory;
// // using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

public class DefineCropYear : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMood;

	private IContainer components;

	private ToolStripButton btnsave;

	private ToolStripButton btnnew;

	private Panel panel4;

	internal GridEX grdcropyear;

	private Panel panel5;

	private Label label2;

	private TextBox txtcropyear;

	private ToolStrip toolStrip1;

	private ToolStripButton btnUpdate;

	private Label label23;

	private Panel panel3;

	private Panel panel2;

	private Panel panel1;

	private CheckBox chk;

	private Panel panel8;

	private Panel panel7;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btnshortcutkeys;

	public DefineCropYear(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void DefineCropYear_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(495, 489);
			bindgrdcropyear();
			btnUpdate.Visible = false;
			txtcropyear.Focus();
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
			if (!FormValidation())
			{
				return;
			}
			InvCropYear obj = new InvCropYear();
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
			obj.ModifyUserId = UserAccount.ID;
			obj.EntryUserId = UserAccount.ID;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.CropYear = Conversion.ToString((object)txtcropyear.Text);
			obj.DefaultKey = chk.Checked;
			InvCropYear.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully.");
			}
			else
			{
				MessageBox.Show("Record Save Successfully..");
			}
			formReFresh();
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

	private void ReadbyId(int Id)
	{
		try
		{
			RecId = Id;
			InvCropYear crop = InvCropYear.GetByID(RecId);
			if (crop != null)
			{
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				UpdateMood = true;
				txtcropyear.Text = crop.CropYear;
				chk.Checked = crop.DefaultKey;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindgrdcropyear()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = InvCropYear.Getall(new InvCropYear
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("CropYear", typeof(string));
				dt.Columns.Add("IsActive", typeof(bool));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Clear();
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["CropYear"], lst.Rows[i]["IsActive"], lst.Rows[i]["EntryUserName"], lst.Rows[i]["EntryDate"], lst.Rows[i]["ModifyUserName"], lst.Rows[i]["ModifyDate"]);
				}
				grdcropyear.DataSource = lst;
				grdcropyear.RetrieveStructure();
				GridSettings();
			}
			else
			{
				grdcropyear.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			grdcropyear.RootTable.Columns["Id"].Visible = false;
			grdcropyear.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdcropyear.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			CommonServices.GridAutoAdjustmentNew(grdcropyear);
			grdcropyear.RootTable.Columns.Add("Edit");
			grdcropyear.RootTable.Columns["Edit"].Key = "Edit";
			grdcropyear.RootTable.Columns["Edit"].Caption = "Edit";
			grdcropyear.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdcropyear.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdcropyear.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdcropyear.RootTable.Columns["Edit"].Width = 50;
			grdcropyear.RootTable.Columns["Edit"].Position = 0;
			grdcropyear.FrozenColumns = 1;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdcropyear_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdcropyear.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && e.Column.Key == "Edit")
			{
				grdcropyear_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdcropyear_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdcropyear.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				ReadbyId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (string.IsNullOrEmpty(txtcropyear.Text.Trim()))
		{
			MessageBox.Show("Crop Year Field Required");
			txtcropyear.Focus();
			return false;
		}
		return true;
	}

	public void formReFresh()
	{
		try
		{
			txtcropyear.Text = string.Empty;
			chk.Checked = false;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			bindgrdcropyear();
			txtcropyear.Focus();
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
			formReFresh();
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

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdcropyear).Name);
			if (fs != null)
			{
				grdcropyear.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DefineCropYear_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdcropyear).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtcropyear.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtcropyear.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save When on Entry form and For Show Data when on History Form");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Vehicle Type");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Vehicle Type");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcutkeys_Click(object sender, EventArgs e)
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

	private void grdcropyear_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdcropyear.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdcropyear.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdcropyear.CurrentColumn.Key == "Edit")
				{
					grdcropyear_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdcropyear_DoubleClick(null, null);
			}
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		GridEXLayout grdcropyear_DesignTimeLayout = new GridEXLayout();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdcropyear = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.chk = new System.Windows.Forms.CheckBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtcropyear = new System.Windows.Forms.TextBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.label23 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdcropyear).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add(this.panel8);
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 101);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(479, 349);
		this.panel4.TabIndex = 3;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdcropyear);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 24);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(479, 325);
		this.panel8.TabIndex = 8;
		this.grdcropyear.AllowEdit = (InheritableBoolean)2;
		grdcropyear_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdcropyear.DesignTimeLayout = grdcropyear_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdcropyear).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdcropyear.FilterMode = (FilterMode)2;
		this.grdcropyear.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdcropyear).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdcropyear.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdcropyear).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdcropyear).Name = "grdcropyear";
		((ScrollableBase)this.grdcropyear).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdcropyear).Size = new System.Drawing.Size(479, 325);
		((System.Windows.Forms.Control)(object)this.grdcropyear).TabIndex = 0;
		this.grdcropyear.TotalRow = (InheritableBoolean)1;
		this.grdcropyear.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdcropyear).VisualStyle = (VisualStyle)4;
		this.grdcropyear.ColumnButtonClick += new ColumnActionEventHandler(grdcropyear_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdcropyear).DoubleClick += new System.EventHandler(grdcropyear_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdcropyear).KeyDown += new System.Windows.Forms.KeyEventHandler(grdcropyear_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar3);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(479, 24);
		this.panel7.TabIndex = 7;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(431, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdcropyear;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar3.TabIndex = 26;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(2, 2);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 19);
		this.label1.TabIndex = 25;
		this.label1.Text = "History";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.chk);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.txtcropyear);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 52);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(479, 49);
		this.panel5.TabIndex = 0;
		this.chk.AutoSize = true;
		this.chk.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.chk.Location = new System.Drawing.Point(344, 13);
		this.chk.Name = "chk";
		this.chk.Size = new System.Drawing.Size(102, 20);
		this.chk.TabIndex = 1;
		this.chk.Text = "Default Key";
		this.chk.UseVisualStyleBackColor = true;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 16);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(57, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Crop Year";
		this.txtcropyear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcropyear.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcropyear.Location = new System.Drawing.Point(71, 12);
		this.txtcropyear.Name = "txtcropyear";
		this.txtcropyear.Size = new System.Drawing.Size(258, 23);
		this.txtcropyear.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnUpdate, this.btnsave, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(479, 27);
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
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(143, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Define CropYear";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(479, 25);
		this.panel3.TabIndex = 2;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(479, 27);
		this.panel2.TabIndex = 0;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(479, 450);
		this.panel1.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(479, 450);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DefineCropYear";
		this.Text = "DefineCropYear";
		base.Load += new System.EventHandler(DefineCropYear_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineCropYear_KeyDown);
		this.panel4.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdcropyear).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
