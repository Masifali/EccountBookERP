using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Lab;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
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
namespace Architecture.WinApp.Lab
{

public class InvLabAnalysisItems : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMode;

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	internal GridEX grdfrm;

	private Panel panel3;

	private TextBox txtdescription;

	private Label label45;

	private Panel panel2;

	private ToolStrip tsItemTaxSchedule;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	internal UltraCombo CmbParentParameter;

	private Label label6;

	private CheckBox ChkIsSub;

	private Panel panel5;

	private Label label1;

	private GroupBox groupBox1;

	internal UltraCombo CmbMasterParameter;

	private Label label2;

	private TextBox txtMaxValue;

	private TextBox txtMinValue;

	private Label label4;

	private Label label3;

	public InvLabAnalysisItems(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private bool formvalidation()
	{
		if (txtdescription.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Description");
			txtdescription.Focus();
			return false;
		}
		if (ChkIsSub.Checked && ((UltraGridBase)CmbParentParameter).ActiveRow == null)
		{
			MessageBox.Show("Parent Parameter Field is Required");
			CmbParentParameter.Focus();
			return false;
		}
		return true;
	}

	private void refresh()
	{
		try
		{
			btnsave.Visible = true;
			btnupdate.Visible = false;
			txtdescription.Clear();
			((Control)(object)CmbParentParameter).Text = string.Empty;
			((Control)(object)CmbMasterParameter).Text = string.Empty;
			txtMinValue.Clear();
			txtMaxValue.Clear();
			ChkIsSub.Checked = false;
			((Control)(object)CmbParentParameter).Visible = false;
			label6.Visible = false;
			gridfill();
			txtdescription.Focus();
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
			InvLabAnalysisItems obj = new InvLabAnalysisItems();
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
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.AnalysisParameterCode = Conversion.ToString((object)txtdescription.Text);
			obj.AnalysisParameterDescription = Conversion.ToString((object)txtdescription.Text);
			obj.IsSub = Conversion.ToBool((object)ChkIsSub.Checked);
			obj.MasterParId = Conversion.ToInt(CmbMasterParameter.Value);
			obj.MinValue = Conversion.ToDouble((object)txtMinValue.Text);
			obj.MaxValue = Conversion.ToDouble((object)txtMaxValue.Text);
			if (obj.IsSub)
			{
				obj.ParentParameterId = Conversion.ToInt(CmbParentParameter.Value);
			}
			else
			{
				obj.ParentParameterId = 0;
			}
			InvLabAnalysisItems.Save(obj);
			if (RecId == 0)
			{
				MessageBox.Show("Save Successfully");
			}
			else
			{
				MessageBox.Show("Update Successfully");
			}
			refresh();
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

	private void btnnew_Click(object sender, EventArgs e)
	{
		refresh();
	}

	private void gridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtgetAll = InvLabAnalysisItems.GetAllOrById(new InvLabAnalysisItems
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable table = new DataTable();
			table.Columns.Add("Id");
			table.Columns.Add("AnalysisParameter");
			table.Columns.Add("ParentParameter");
			table.Columns.Add("MasterParId");
			table.Columns.Add("MasterParameter");
			table.Columns.Add("MinValue");
			table.Columns.Add("MaxValue");
			for (int i = 0; i < dtgetAll.Rows.Count; i++)
			{
				table.Rows.Add(dtgetAll.Rows[i]["Id"], dtgetAll.Rows[i]["AnalysisParameterDescription"], dtgetAll.Rows[i]["ParentParameter"], dtgetAll.Rows[i]["MasterParId"], dtgetAll.Rows[i]["MasterParameterName"], dtgetAll.Rows[i]["MinValue"], dtgetAll.Rows[i]["MaxValue"]);
			}
			if (table.Rows.Count > 0)
			{
				DDL.BindDDLNew(table, CmbParentParameter, "Id", "AnalysisParameter", "Parent Parameter", false);
			}
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			grdfrmSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrmSetting()
	{
		try
		{
			grdfrm.AllowEdit = (InheritableBoolean)2;
			grdfrm.AllowDelete = (InheritableBoolean)2;
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["MasterParId"].Visible = false;
			grdfrm.RootTable.Columns["AnalysisParameter"].Width = 250;
			grdfrm.RootTable.Columns["ParentParameter"].Width = 250;
			grdfrm.RootTable.Columns["MasterParameter"].Width = 250;
			grdfrm.RootTable.Columns["MinValue"].Width = 80;
			grdfrm.RootTable.Columns["MaxValue"].Width = 80;
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
			InvLabAnalysisItems obj = new InvLabAnalysisItems();
			btnsave.Visible = false;
			btnupdate.Visible = true;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = Conversion.ToInt((object)grdfrm.CurrentRow.Cells["Id"].Value.ToString());
			RecId = obj.Id;
			DataTable dtgetbyId = new DataTable();
			dtgetbyId = InvLabAnalysisItems.GetAllOrById(obj);
			if (dtgetbyId.Rows.Count > 0)
			{
				txtdescription.Text = Conversion.ToString(dtgetbyId.Rows[0]["AnalysisParameterDescription"]);
				CmbParentParameter.Value = Conversion.ToInt(dtgetbyId.Rows[0]["ParentParameterId"]);
				CmbMasterParameter.Value = Conversion.ToInt(dtgetbyId.Rows[0]["MasterParId"]);
				ChkIsSub.Checked = Conversion.ToBool(dtgetbyId.Rows[0]["IsSub"]);
				txtMinValue.Text = Conversion.ToString(dtgetbyId.Rows[0]["MinValue"]);
				txtMaxValue.Text = Conversion.ToString(dtgetbyId.Rows[0]["MaxValue"]);
				UpdateMode = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MasterParameters()
	{
		try
		{
			DataTable dt = InvLabAnalysisItems.getLabMasterParms();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbMasterParameter, "Id", "MasterParameterName", "Master Parameter", ZeroIndex: false);
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

	private void InvLabAnalysisItems_Load(object sender, EventArgs e)
	{
		try
		{
			ChkIsSub.Checked = false;
			label6.Visible = false;
			((Control)(object)CmbParentParameter).Visible = false;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			gridfill();
			MasterParameters();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void InvLabAnalysisItems_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
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
			if (e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnupdate_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChkIsSub_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (ChkIsSub.Checked)
			{
				((Control)(object)CmbParentParameter).Visible = true;
				label6.Visible = true;
			}
			else
			{
				((Control)(object)CmbParentParameter).Visible = false;
				label6.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvLabAnalysisItems_FormClosing(object sender, FormClosingEventArgs e)
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

	private void grdfrm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdfrm.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdfrm_DoubleClick(null, null);
				}
			}
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
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lab.InvLabAnalysisItems));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtMaxValue = new System.Windows.Forms.TextBox();
		this.txtMinValue = new System.Windows.Forms.TextBox();
		this.CmbMasterParameter = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbParentParameter = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.ChkIsSub = new System.Windows.Forms.CheckBox();
		this.txtdescription = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemTaxSchedule = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterParameter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentParameter).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tsItemTaxSchedule.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(637, 532);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 183);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(637, 349);
		this.panel4.TabIndex = 84;
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
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(637, 349);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdfrm_KeyDown);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(637, 155);
		this.panel3.TabIndex = 83;
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtMaxValue);
		this.groupBox1.Controls.Add(this.txtMinValue);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMasterParameter);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentParameter);
		this.groupBox1.Controls.Add(this.label45);
		this.groupBox1.Controls.Add(this.ChkIsSub);
		this.groupBox1.Controls.Add(this.txtdescription);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 27);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(408, 128);
		this.groupBox1.TabIndex = 36;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Information";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(238, 74);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(62, 15);
		this.label4.TabIndex = 42;
		this.label4.Text = "Max Value";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(10, 74);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(60, 15);
		this.label3.TabIndex = 41;
		this.label3.Text = "Min Value";
		this.txtMaxValue.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMaxValue.Location = new System.Drawing.Point(305, 70);
		this.txtMaxValue.Name = "txtMaxValue";
		this.txtMaxValue.Size = new System.Drawing.Size(94, 23);
		this.txtMaxValue.TabIndex = 4;
		this.txtMaxValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMaxValue_KeyPress);
		this.txtMinValue.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMinValue.Location = new System.Drawing.Point(138, 70);
		this.txtMinValue.Name = "txtMinValue";
		this.txtMinValue.Size = new System.Drawing.Size(94, 23);
		this.txtMinValue.TabIndex = 3;
		this.txtMinValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMinValue_KeyPress);
		this.CmbMasterParameter.AllowNull = (DefaultableBoolean)1;
		this.CmbMasterParameter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMasterParameter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMasterParameter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterParameter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterParameter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMasterParameter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMasterParameter).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMasterParameter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMasterParameter).Location = new System.Drawing.Point(138, 15);
		((System.Windows.Forms.Control)(object)this.CmbMasterParameter).Name = "CmbMasterParameter";
		((System.Windows.Forms.Control)(object)this.CmbMasterParameter).Size = new System.Drawing.Size(261, 26);
		((System.Windows.Forms.Control)(object)this.CmbMasterParameter).TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(10, 21);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(100, 15);
		this.label2.TabIndex = 36;
		this.label2.Text = "Master Parameter";
		this.CmbParentParameter.AllowNull = (DefaultableBoolean)1;
		this.CmbParentParameter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentParameter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentParameter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentParameter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentParameter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentParameter).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentParameter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentParameter).Location = new System.Drawing.Point(138, 96);
		((System.Windows.Forms.Control)(object)this.CmbParentParameter).Name = "CmbParentParameter";
		((System.Windows.Forms.Control)(object)this.CmbParentParameter).Size = new System.Drawing.Size(261, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentParameter).TabIndex = 5;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(10, 48);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(125, 15);
		this.label45.TabIndex = 5;
		this.label45.Text = "Parameter Description";
		this.ChkIsSub.AutoSize = true;
		this.ChkIsSub.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkIsSub.Location = new System.Drawing.Point(344, 47);
		this.ChkIsSub.Name = "ChkIsSub";
		this.ChkIsSub.Size = new System.Drawing.Size(56, 19);
		this.ChkIsSub.TabIndex = 2;
		this.ChkIsSub.Text = "IsSub";
		this.ChkIsSub.UseVisualStyleBackColor = true;
		this.ChkIsSub.CheckedChanged += new System.EventHandler(ChkIsSub_CheckedChanged);
		this.txtdescription.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdescription.Location = new System.Drawing.Point(138, 44);
		this.txtdescription.Name = "txtdescription";
		this.txtdescription.Size = new System.Drawing.Size(199, 23);
		this.txtdescription.TabIndex = 1;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(11, 102);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(98, 15);
		this.label6.TabIndex = 34;
		this.label6.Text = "Parent Parameter";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(637, 27);
		this.panel5.TabIndex = 35;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(176, 19);
		this.label1.TabIndex = 36;
		this.label1.Text = "Analysis Parameters";
		this.panel2.Controls.Add(this.tsItemTaxSchedule);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(637, 28);
		this.panel2.TabIndex = 0;
		this.tsItemTaxSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemTaxSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemTaxSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnupdate, this.btnsave });
		this.tsItemTaxSchedule.Location = new System.Drawing.Point(0, 0);
		this.tsItemTaxSchedule.Name = "tsItemTaxSchedule";
		this.tsItemTaxSchedule.Size = new System.Drawing.Size(637, 27);
		this.tsItemTaxSchedule.TabIndex = 82;
		this.tsItemTaxSchedule.Text = "toolStrip3";
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Control;
		base.ClientSize = new System.Drawing.Size(637, 532);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "InvLabAnalysisItems";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvLabAnalysisItems_FormClosing);
		base.Load += new System.EventHandler(InvLabAnalysisItems_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvLabAnalysisItems_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterParameter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentParameter).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemTaxSchedule.ResumeLayout(false);
		this.tsItemTaxSchedule.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
