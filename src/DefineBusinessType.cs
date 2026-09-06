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
using System.Threading.Tasks;
using System.Windows.Forms;
using global::Architecture.BLL;
using global::Architecture.Common;
using global::Architecture.Model;
// // using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

public class DefineBusinessType : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtHistoryGrid = new DataTable();

	private int RecId;

	private bool FormInitialized;

	private string ScreenName = "DefineBusinessType";

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel4;

	internal GridEX grdfrm;

	private Button btnAdd;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label label1;

	private TextBox txtDescription;

	private TextBox txtBrandName;

	private MenuStrip menuStrip1;

	private ToolStripButton btnnew;

	private ToolStripMenuItem btnSave;

	private ToolStripButton btnupdate;

	private ToolStripButton btnShortCutKeys;

	private CtrlGrdBar ctrlGrdBar1;

	private CheckBox chkIsActive;

	public DefineBusinessType(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 800);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			ScreenName = Conversion.ToString((object)base.Name);
			return new
			{
				HistoryData = BusinessType.FormHistory(UserAccount.OrganizationId, UserAccount.CompanyId, 0, false)
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				BindGrid(result.HistoryData);
				FormInitialized = true;
				btnSave.Visible = true;
				btnupdate.Visible = false;
				btnAdd.Text = "Save";
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private bool ValidateInputs()
	{
		if (string.IsNullOrWhiteSpace(txtDescription.Text))
		{
			MessageBox.Show("BusinessType Code is required");
			txtDescription.Focus();
			return false;
		}
		if (string.IsNullOrWhiteSpace(txtBrandName.Text))
		{
			MessageBox.Show("BusinessType Name is required");
			txtBrandName.Focus();
			return false;
		}
		if (GridEX_Helper.IsAnyCellValueHaveSameValueExcept(grdfrm, "Description", txtDescription.Text.Trim(), "Id", RecId))
		{
			MessageBox.Show("Business TypeType Code already exists.");
			txtDescription.Focus();
			return false;
		}
		if (GridEX_Helper.IsAnyCellValueHaveSameValueExcept(grdfrm, "BusinessTypeName", txtBrandName.Text.Trim(), "Id", RecId))
		{
			MessageBox.Show("Business TypeType Name already exists.");
			txtBrandName.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		try
		{
			if (ValidateInputs() && MessageBox.Show((RecId > 0) ? "Are you sure you want to update?" : "Are you sure you want to save?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				BusinessType.Save(new BusinessType
				{
					BusinessTypeID = RecId,
					organizationId = UserAccount.OrganizationId,
					companyId = UserAccount.CompanyId,
					entryUserId = UserAccount.ID,
					modifyUserId = UserAccount.ID,
					entryDate = DateTime.Now,
					modifyDate = DateTime.Now,
					BusinessTypeName = txtBrandName.Text.Trim(),
					Description = txtDescription.Text.Trim(),
					IsActive = chkIsActive.Checked
				});
				MessageBox.Show((RecId > 0) ? "Record updated successfully." : "Record saved successfully.");
				ResetForm();
				BindGrid(BusinessType.FormHistory(UserAccount.OrganizationId, UserAccount.CompanyId, 0, false));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
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

	private void ReadById(int Id)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			RecId = Id;
			GridEXRow item = grdfrm.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				txtBrandName.Text = Conversion.ToString(item.Cells["BusinessTypeName"].Value);
				txtDescription.Text = Conversion.ToString(item.Cells["Description"].Value);
				chkIsActive.Checked = Conversion.ToBool(item.Cells["Active"].Value);
				btnSave.Visible = false;
				btnupdate.Visible = true;
				btnAdd.Text = "Update";
				txtDescription.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
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

	public void BindGrid(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				if (dtHistoryGrid.Columns.Count == 0)
				{
					dtHistoryGrid.Columns.Add("Id", typeof(int));
					dtHistoryGrid.Columns.Add("BusinessTypeName");
					dtHistoryGrid.Columns.Add("Description");
					dtHistoryGrid.Columns.Add("EntryUser");
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ModifyUser");
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("Active", typeof(bool));
				}
				dtHistoryGrid.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHistoryGrid.Rows.Add(dt.Rows[i]["BusinessTypeID"], dt.Rows[i]["BusinessTypeName"], dt.Rows[i]["Description"], dt.Rows[i]["EntryUserName"], dt.Rows[i]["EntryDate"], dt.Rows[i]["ModifyUserName"], dt.Rows[i]["ModifyDate"], dt.Rows[i]["IsActive"]);
				}
				grdfrm.DataSource = dtHistoryGrid;
				grdfrm.RetrieveStructure();
				grdfrm.RootTable.Columns["Id"].Visible = false;
				grdfrm.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
				grdfrm.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
				GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2);
			}
			else
			{
				grdfrm.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdfrm.CurrentRow != null && (int)grdfrm.CurrentRow.RowType == 0)
			{
				ReadById(Conversion.ToInt(grdfrm.CurrentRow.Cells["Id"].Value));
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdfrm, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	public void ResetForm()
	{
		RecId = 0;
		txtDescription.Clear();
		txtBrandName.Clear();
		btnSave.Visible = true;
		btnupdate.Visible = false;
		btnAdd.Text = "Save";
	}

	private void DefineBusinessType_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnnew_Click(object sender, EventArgs e)
	{
		ResetForm();
	}

	private void DefineBusinessType_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
			{
				btnSave_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
			{
				btnupdate_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				ResetForm();
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				txtDescription.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdfrm).Focus();
			}
			else if (e.Control && e.Alt)
			{
				ShowShortcutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Shortcut error: " + ex.Message);
		}
	}

	private void ShowShortcutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Description");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Description");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcukeys_Click(object sender, EventArgs e)
	{
		try
		{
			ShowShortcutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdfrm.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Control && e.KeyCode == Keys.Return)
			{
				grdfrm_DoubleClick(null, null);
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
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.DefineBusinessType));
		this.panel1 = new System.Windows.Forms.Panel();
		this.chkIsActive = new System.Windows.Forms.CheckBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.txtBrandName = new System.Windows.Forms.TextBox();
		this.btnAdd = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.menuStrip1 = new System.Windows.Forms.MenuStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripMenuItem();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdfrm = new GridEX();
		this.label3 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.menuStrip1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.chkIsActive);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.txtDescription);
		this.panel1.Controls.Add(this.txtBrandName);
		this.panel1.Controls.Add(this.btnAdd);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(439, 138);
		this.panel1.TabIndex = 0;
		this.chkIsActive.AutoSize = true;
		this.chkIsActive.Checked = true;
		this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkIsActive.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIsActive.Location = new System.Drawing.Point(130, 110);
		this.chkIsActive.Name = "chkIsActive";
		this.chkIsActive.Size = new System.Drawing.Size(67, 18);
		this.chkIsActive.TabIndex = 2;
		this.chkIsActive.Text = "Active";
		this.chkIsActive.UseVisualStyleBackColor = true;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(15, 60);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(115, 15);
		this.label2.TabIndex = 5;
		this.label2.Text = "Business Type Name";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(15, 84);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(68, 15);
		this.label1.TabIndex = 5;
		this.label1.Text = "Description";
		this.txtDescription.Location = new System.Drawing.Point(130, 80);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(235, 22);
		this.txtDescription.TabIndex = 1;
		this.txtBrandName.Location = new System.Drawing.Point(130, 56);
		this.txtBrandName.Name = "txtBrandName";
		this.txtBrandName.Size = new System.Drawing.Size(235, 22);
		this.txtBrandName.TabIndex = 0;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(272, 105);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(93, 28);
		this.btnAdd.TabIndex = 3;
		this.btnAdd.Text = "Save";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.menuStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(439, 50);
		this.panel2.TabIndex = 4;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 28);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(182, 19);
		this.label4.TabIndex = 33;
		this.label4.Text = "Define Business Type";
		this.menuStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnSave, this.btnupdate, this.btnShortCutKeys });
		this.menuStrip1.Location = new System.Drawing.Point(0, 0);
		this.menuStrip1.Name = "menuStrip1";
		this.menuStrip1.Size = new System.Drawing.Size(439, 27);
		this.menuStrip1.TabIndex = 0;
		this.menuStrip1.Text = "menuStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 20);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(68, 23);
		this.btnSave.Text = "Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(74, 20);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(122, 20);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortcukeys_Click);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 138);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(439, 26);
		this.panel4.TabIndex = 6;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(397, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 34;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 164);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(439, 205);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 1;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdfrm_KeyDown);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(8, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 19);
		this.label3.TabIndex = 33;
		this.label3.Text = "History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(439, 369);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "DefineBusinessType";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(DefineBusinessType_FormClosed);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineBusinessType_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.menuStrip1.ResumeLayout(false);
		this.menuStrip1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		base.ResumeLayout(false);
	}
}

}
