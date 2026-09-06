using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Import;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Import;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Import;

public class frmMasterDocumentSerial : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtHistory = new DataTable();

	private long RecId;

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private Panel panel2;

	internal GridEX Grd;

	private Button BtnCancel;

	private Button BtnSavef;

	private Button btnUpdateF;

	private Label label3;

	private TextBox txtLcOrderNo;

	private Label label4;

	private TextBox txtInvoiceNo;

	private Label label2;

	private DateTimePicker txtLcOrderDate;

	private Label label1;

	private DateTimePicker txtInvoiceDate;

	private Label label5;

	private ToolStripButton btnShortCutKeys;

	public frmMasterDocumentSerial(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void frmMasterDocumentSerial_Load(object sender, EventArgs e)
	{
		try
		{
			base.WindowState = FormWindowState.Normal;
			base.Size = new Size(520, 480);
			base.StartPosition = FormStartPosition.Manual;
			if (base.Parent != null)
			{
				int centerX = base.Parent.Left + (base.Parent.Width - base.Width) / 2;
				int centerY = base.Parent.Top + (base.Parent.Height - base.Height) / 2;
				base.Location = new Point(centerX, centerY);
			}
			else
			{
				base.StartPosition = FormStartPosition.CenterParent;
			}
			BindHistory();
			btnUpdate.Visible = false;
			btnsave.Visible = true;
			btnUpdateF.Visible = false;
			BtnSavef.Visible = true;
			txtLcOrderNo.Focus();
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
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			BtnSavef.Visible = true;
			btnUpdateF.Visible = false;
			txtLcOrderNo.Text = string.Empty;
			txtInvoiceNo.Text = string.Empty;
			BindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
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

	private bool FormValiadation()
	{
		if (txtLcOrderNo.Text == string.Empty || txtLcOrderNo.Text == "0")
		{
			MessageBox.Show("LcOrder No Is Required");
			txtLcOrderNo.Focus();
			return false;
		}
		if (txtInvoiceNo.Text == string.Empty || txtInvoiceNo.Text == "0")
		{
			MessageBox.Show("InvoiceNo Field Is Required");
			txtInvoiceNo.Focus();
			return false;
		}
		if (Conversion.ToDateTime((object)txtInvoiceDate.Value) > Conversion.ToDateTime((object)txtLcOrderDate.Value))
		{
			MessageBox.Show("InvoiceDate Can  not Less than Lc Order Date Field Is Required");
			txtInvoiceDate.Focus();
			return false;
		}
		return true;
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0L;
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
			masterDocumentSerial obj = new masterDocumentSerial();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.masterDocumentSerialId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.locationBranchId = UserAccount.BranchesId;
			obj.invoiceNo = txtInvoiceNo.Text;
			obj.invoiceDate = txtInvoiceDate.Value;
			obj.locOrderNo = txtLcOrderNo.Text;
			obj.locOrderDate = txtLcOrderDate.Value;
			obj.createdUserId = UserAccount.ID;
			obj.approvedUserId = UserAccount.ID;
			obj.lastModifiedUserId = UserAccount.ID;
			obj.createdOn = DateTime.Now;
			obj.lastModifiedOn = DateTime.Now;
			obj.approvedOn = DateTime.Now;
			obj.proformaDate = DateTime.Now;
			masterDocumentSerial.Save(obj);
			if (RecId == 0L)
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
			if (RecId == 0L)
			{
				throw new Exception("RecId Not Found...");
			}
			Insert();
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
				RecId = Conversion.ToInt64(item.Cells["Id"].Value);
				txtLcOrderNo.Text = Conversion.ToString(item.Cells["LocOrderNo"].Value);
				txtLcOrderDate.Value = Conversion.ToDateTime(item.Cells["LocOrderDate"].Value);
				txtInvoiceNo.Text = Conversion.ToString(item.Cells["InvoiceNo"].Value);
				txtInvoiceDate.Value = Conversion.ToDateTime(item.Cells["InvoiceDate"].Value);
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

	private void BindHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtHistory = masterDocumentSerial.SEARCHHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtDocumentTracking = new DataTable();
				dtDocumentTracking.Columns.Add("Id", typeof(long));
				dtDocumentTracking.Columns.Add("RefDocumentTypeId", typeof(int));
				dtDocumentTracking.Columns.Add("LocOrderMasterId", typeof(int));
				dtDocumentTracking.Columns.Add("LocOrderNo", typeof(string));
				dtDocumentTracking.Columns.Add("LocOrderDate", typeof(DateTime));
				dtDocumentTracking.Columns.Add("InvoiceMasterId", typeof(int));
				dtDocumentTracking.Columns.Add("InvoiceDate", typeof(DateTime));
				dtDocumentTracking.Columns.Add("InvoiceNo", typeof(string));
				dtDocumentTracking.Columns.Add("CreatedUserName", typeof(string));
				dtDocumentTracking.Columns.Add("CreatedOn", typeof(DateTime));
				dtDocumentTracking.Columns.Add("LastModifyUserName", typeof(string));
				dtDocumentTracking.Columns.Add("LastModifiedOn", typeof(DateTime));
				dtDocumentTracking.Columns.Add("RowVersionLong", typeof(long));
				dtDocumentTracking.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					dtDocumentTracking.Rows.Add(row["MasterDocumentSerialId"], row["RefDocumentTypeId"], row["LocOrderMasterId"], row["LocOrderNo"], row["LocOrderDate"], row["InvoiceMasterId"], row["InvoiceDate"], row["InvoiceNo"], row["CreatedUserName"], row["CreatedOn"], row["LastModifyUserName"], row["LastModifiedOn"], row["RowVersionLong"]);
				}
				Grd.DataSource = dtDocumentTracking;
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
			Grd.RootTable.Columns["InvoiceMasterId"].Visible = false;
			Grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			Grd.RootTable.Columns["LocOrderMasterId"].Visible = false;
			Grd.RootTable.Columns["RowVersionLong"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(Grd, 3);
			Grd.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.ContractNoConstant;
			Grd.RootTable.Columns["InvoiceDate"].Width = Constants.ExportConstants.DateConstant;
			Grd.RootTable.Columns["LocOrderNo"].Width = Constants.ExportConstants.ContractNoConstant;
			Grd.RootTable.Columns["LocOrderDate"].Width = Constants.ExportConstants.DateConstant;
			Grd.RootTable.Columns["CreatedUserName"].Width = Constants.ExportConstants.EntryUserConstants;
			Grd.RootTable.Columns["CreatedOn"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			Grd.RootTable.Columns["LastModifyUserName"].Width = Constants.ExportConstants.ModifyUserConstants;
			Grd.RootTable.Columns["LastModifiedOn"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			Grd.RootTable.Columns.Add("Edit");
			Grd.RootTable.Columns["Edit"].Key = "Edit";
			Grd.RootTable.Columns["Edit"].Caption = "Edit";
			Grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			Grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			Grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			Grd.RootTable.Columns["Edit"].Width = 40;
			Grd.RootTable.Columns["Edit"].Position = 0;
			Grd.FrozenColumns = 1;
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

	private void Grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
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

	private void frmMasterDocumentSerial_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
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
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)Grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtLcOrderNo.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtLcOrderNo.Focus();
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
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on LcOrderNo");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On LcOrderNo");
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

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Expected O, but got Unknown
		GridEXLayout Grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Import.frmMasterDocumentSerial));
		this.panel1 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.Grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.txtInvoiceDate = new System.Windows.Forms.DateTimePicker();
		this.txtLcOrderDate = new System.Windows.Forms.DateTimePicker();
		this.txtLcOrderNo = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtInvoiceNo = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.btnUpdateF = new System.Windows.Forms.Button();
		this.BtnCancel = new System.Windows.Forms.Button();
		this.BtnSavef = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Grd).BeginInit();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(505, 52);
		this.panel1.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(3, 28);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(270, 19);
		this.label3.TabIndex = 25;
		this.label3.Text = "Generate LcOrder && Invoice Nos";
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnsave, this.btnUpdate, this.btnShortCutKeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(505, 27);
		this.toolStrip1.TabIndex = 0;
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
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.Grd.AllowEdit = (InheritableBoolean)2;
		Grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.Grd.DesignTimeLayout = Grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.Grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.Grd.DynamicFiltering = true;
		this.Grd.FilterMode = (FilterMode)2;
		this.Grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.Grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.Grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.Grd).Location = new System.Drawing.Point(0, 146);
		((System.Windows.Forms.Control)(object)this.Grd).Name = "Grd";
		((ScrollableBase)this.Grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.Grd).Size = new System.Drawing.Size(505, 265);
		((System.Windows.Forms.Control)(object)this.Grd).TabIndex = 1;
		this.Grd.TotalRow = (InheritableBoolean)1;
		this.Grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.Grd).VisualStyle = (VisualStyle)4;
		this.Grd.ColumnButtonClick += new ColumnActionEventHandler(Grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.Grd).DoubleClick += new System.EventHandler(Grd_DoubleClick);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.txtInvoiceDate);
		this.panel2.Controls.Add(this.txtLcOrderDate);
		this.panel2.Controls.Add(this.txtLcOrderNo);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.txtInvoiceNo);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.btnUpdateF);
		this.panel2.Controls.Add(this.BtnCancel);
		this.panel2.Controls.Add(this.BtnSavef);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 52);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(505, 94);
		this.panel2.TabIndex = 0;
		this.txtInvoiceDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtInvoiceDate.Location = new System.Drawing.Point(389, 35);
		this.txtInvoiceDate.Name = "txtInvoiceDate";
		this.txtInvoiceDate.Size = new System.Drawing.Size(111, 22);
		this.txtInvoiceDate.TabIndex = 3;
		this.txtLcOrderDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLcOrderDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtLcOrderDate.Location = new System.Drawing.Point(389, 9);
		this.txtLcOrderDate.Name = "txtLcOrderDate";
		this.txtLcOrderDate.Size = new System.Drawing.Size(111, 22);
		this.txtLcOrderDate.TabIndex = 1;
		this.txtLcOrderNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLcOrderNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLcOrderNo.Location = new System.Drawing.Point(89, 9);
		this.txtLcOrderNo.Name = "txtLcOrderNo";
		this.txtLcOrderNo.Size = new System.Drawing.Size(209, 23);
		this.txtLcOrderNo.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(10, 12);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(77, 17);
		this.label4.TabIndex = 607;
		this.label4.Text = "LcOrder No";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(302, 38);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(83, 17);
		this.label5.TabIndex = 605;
		this.label5.Text = "Invoice Date";
		this.txtInvoiceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNo.Location = new System.Drawing.Point(89, 35);
		this.txtInvoiceNo.Name = "txtInvoiceNo";
		this.txtInvoiceNo.Size = new System.Drawing.Size(209, 23);
		this.txtInvoiceNo.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(302, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(87, 17);
		this.label1.TabIndex = 605;
		this.label1.Text = "LcOrder Date";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(10, 38);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(73, 17);
		this.label2.TabIndex = 605;
		this.label2.Text = "Invoice No";
		this.btnUpdateF.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateF.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateF.ForeColor = System.Drawing.Color.White;
		this.btnUpdateF.Location = new System.Drawing.Point(426, 61);
		this.btnUpdateF.Name = "btnUpdateF";
		this.btnUpdateF.Size = new System.Drawing.Size(75, 28);
		this.btnUpdateF.TabIndex = 4;
		this.btnUpdateF.Text = "Update";
		this.btnUpdateF.UseVisualStyleBackColor = false;
		this.btnUpdateF.Visible = false;
		this.btnUpdateF.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnCancel.BackColor = System.Drawing.Color.Teal;
		this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancel.ForeColor = System.Drawing.Color.White;
		this.BtnCancel.Location = new System.Drawing.Point(351, 61);
		this.BtnCancel.Name = "BtnCancel";
		this.BtnCancel.Size = new System.Drawing.Size(75, 28);
		this.BtnCancel.TabIndex = 5;
		this.BtnCancel.Text = "Cancel";
		this.BtnCancel.UseVisualStyleBackColor = false;
		this.BtnCancel.Click += new System.EventHandler(BtnCancel_Click);
		this.BtnSavef.BackColor = System.Drawing.Color.Teal;
		this.BtnSavef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSavef.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSavef.ForeColor = System.Drawing.Color.White;
		this.BtnSavef.Location = new System.Drawing.Point(426, 61);
		this.BtnSavef.Name = "BtnSavef";
		this.BtnSavef.Size = new System.Drawing.Size(75, 28);
		this.BtnSavef.TabIndex = 4;
		this.BtnSavef.Text = "Save";
		this.BtnSavef.UseVisualStyleBackColor = false;
		this.BtnSavef.Click += new System.EventHandler(btnsave_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(505, 411);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.Grd);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmMasterDocumentSerial";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "EccountBook ERP";
		base.Load += new System.EventHandler(frmMasterDocumentSerial_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmMasterDocumentSerial_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.Grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}
