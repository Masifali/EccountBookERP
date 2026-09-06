using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.HRM.ProfileManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.ProfileManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.HRM.ProfileManagement
{

public class genEmployeeGroup : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private bool UpdateMood;

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private TextBox txtGroupName;

	private Label label2;

	private Panel panel4;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private TextBox txtShortName;

	private Label label3;

	private Button btnUpdateF;

	private Button BtnCancel;

	private Button BtnSavef;

	private Panel panel2;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label4;

	private Label label1;

	public genEmployeeGroup(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void genDepartment_Load(object sender, EventArgs e)
	{
		try
		{
			GridFill();
			BtnSavef.Visible = true;
			btnUpdateF.Visible = false;
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
			genEmployeeGroup obj = new genEmployeeGroup();
			if (RecId > 0)
			{
				obj.EmployeeGroupId = RecId;
			}
			else if (grd.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					if (rows[i].Cells["Name"].Text == txtGroupName.Text)
					{
						MessageBox.Show("This Already already Exist");
						return;
					}
				}
			}
			obj.EmployeeGroupPrefix = Conversion.ToString((object)txtShortName.Text.Trim());
			obj.EmployeeGroupName = Conversion.ToString((object)txtGroupName.Text.Trim());
			if (obj.EmployeeGroupId > 0)
			{
				obj.ActionTypeId = 2;
			}
			else
			{
				obj.ActionTypeId = 1;
			}
			obj.AlteredById = UserAccount.ID;
			obj.AlteredOn = DateTime.Now;
			obj.OrginizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.CreatedById = UserAccount.ID;
			obj.CreatedOn = DateTime.Now;
			obj.UserLogId = UserAccount.ID;
			if (genEmployeeGroup.Save(obj) > 0)
			{
				if (RecId > 0)
				{
					MessageBox.Show("Update Successfully");
				}
				else
				{
					MessageBox.Show("Save Successfully");
				}
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ShortName");
			dt.Columns.Add("Name");
			DataTable dtGrid = genEmployeeGroup.Getall(new genEmployeeGroup
			{
				OrginizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtGrid.Rows.Count > 0)
			{
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dt.Rows.Add(dtGrid.Rows[i]["EmployeeGroupId"], dtGrid.Rows[i]["EmployeeGroupPrefix"], dtGrid.Rows[i]["EmployeeGroupName"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GridSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RetrivedData(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			genEmployeeGroup obj = new genEmployeeGroup();
			obj = genEmployeeGroup.GetByID(Id);
			RecId = Id;
			txtShortName.Text = Conversion.ToString((object)obj.EmployeeGroupPrefix);
			txtGroupName.Text = Conversion.ToString((object)obj.EmployeeGroupName);
			UpdateMood = true;
			btnsave.Visible = false;
			btnupdate.Visible = true;
			BtnSavef.Visible = false;
			btnUpdateF.Visible = true;
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
			RetrivedData(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value));
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
			txtGroupName.Text = string.Empty;
			txtShortName.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			BtnSavef.Visible = true;
			btnUpdateF.Visible = false;
			UpdateMood = false;
			RecId = 0;
			txtShortName.Focus();
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void genDepartment_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && !UpdateMood)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U && UpdateMood)
			{
				btnupdate_Click(null, null);
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

	private bool formvalidation()
	{
		if (txtShortName.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Pre Name");
			txtShortName.Focus();
			return false;
		}
		if (txtGroupName.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Group Name");
			txtGroupName.Focus();
			return false;
		}
		return true;
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.ProfileManagement.genEmployeeGroup));
		this.panel1 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnUpdateF = new System.Windows.Forms.Button();
		this.BtnCancel = new System.Windows.Forms.Button();
		this.BtnSavef = new System.Windows.Forms.Button();
		this.txtShortName = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtGroupName = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(434, 561);
		this.panel1.TabIndex = 0;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 197);
		((System.Windows.Forms.Control)(object)this.grd).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grd).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(434, 364);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 11;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 166);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(434, 31);
		this.panel2.TabIndex = 10;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(4, 8);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 19);
		this.label4.TabIndex = 26;
		this.label4.Text = "History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(386, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 28);
		this.ctrlGrdBar2.TabIndex = 25;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel3.Controls.Add(this.btnUpdateF);
		this.panel3.Controls.Add(this.BtnCancel);
		this.panel3.Controls.Add(this.BtnSavef);
		this.panel3.Controls.Add(this.txtShortName);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.txtGroupName);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 55);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(434, 111);
		this.panel3.TabIndex = 0;
		this.btnUpdateF.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateF.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateF.ForeColor = System.Drawing.Color.White;
		this.btnUpdateF.Location = new System.Drawing.Point(260, 67);
		this.btnUpdateF.Name = "btnUpdateF";
		this.btnUpdateF.Size = new System.Drawing.Size(75, 28);
		this.btnUpdateF.TabIndex = 2;
		this.btnUpdateF.Text = "Update";
		this.btnUpdateF.UseVisualStyleBackColor = false;
		this.btnUpdateF.Visible = false;
		this.BtnCancel.BackColor = System.Drawing.Color.Teal;
		this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancel.ForeColor = System.Drawing.Color.White;
		this.BtnCancel.Location = new System.Drawing.Point(181, 68);
		this.BtnCancel.Name = "BtnCancel";
		this.BtnCancel.Size = new System.Drawing.Size(75, 28);
		this.BtnCancel.TabIndex = 3;
		this.BtnCancel.Text = "Cancel";
		this.BtnCancel.UseVisualStyleBackColor = false;
		this.BtnSavef.BackColor = System.Drawing.Color.Teal;
		this.BtnSavef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSavef.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSavef.ForeColor = System.Drawing.Color.White;
		this.BtnSavef.Location = new System.Drawing.Point(260, 68);
		this.BtnSavef.Name = "BtnSavef";
		this.BtnSavef.Size = new System.Drawing.Size(75, 28);
		this.BtnSavef.TabIndex = 2;
		this.BtnSavef.Text = "Save";
		this.BtnSavef.UseVisualStyleBackColor = false;
		this.txtShortName.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShortName.Location = new System.Drawing.Point(123, 10);
		this.txtShortName.Name = "txtShortName";
		this.txtShortName.Size = new System.Drawing.Size(212, 22);
		this.txtShortName.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(14, 13);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(71, 15);
		this.label3.TabIndex = 8;
		this.label3.Text = "Short Name";
		this.txtGroupName.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGroupName.Location = new System.Drawing.Point(123, 39);
		this.txtGroupName.Name = "txtGroupName";
		this.txtGroupName.Size = new System.Drawing.Size(212, 22);
		this.txtGroupName.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(14, 42);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(95, 15);
		this.label2.TabIndex = 2;
		this.label2.Text = "Employee Group";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.toolStrip1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(434, 55);
		this.panel4.TabIndex = 6;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 31);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(142, 19);
		this.label1.TabIndex = 10;
		this.label1.Text = "Employee Group";
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnupdate, this.btnsave });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(434, 27);
		this.toolStrip1.TabIndex = 8;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(77, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		base.ClientSize = new System.Drawing.Size(434, 561);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "genEmployeeGroup";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "EccountBookERP";
		base.Load += new System.EventHandler(genDepartment_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(genDepartment_KeyDown);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
