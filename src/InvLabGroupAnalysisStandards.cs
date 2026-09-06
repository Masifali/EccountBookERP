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
using Janus.Data;
using Janus.Windows.GridEX;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
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

public class InvLabGroupAnalysisStandards : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMode;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtgetAll = new DataTable();

	private IContainer components;

	private Panel panel2;

	private Panel panel1;

	private Panel panel5;

	internal GridEX grdfrm;

	private Panel panel4;

	private TextBox txtminvalue;

	private TextBox txtmaxvalue;

	private Label label1;

	private Label label2;

	private Label label45;

	private Label label44;

	private Panel panel3;

	private ToolStrip tsItemTaxSchedule;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	internal UltraCombo cmbgroup;

	internal UltraCombo cmbitem;

	private ToolStripButton btnRefresh;

	private Panel panel6;

	private Label label3;

	private GroupBox groupBox1;

	private CheckBox chkEditableAfterApproval;

	public InvLabGroupAnalysisStandards(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private bool formvalidation()
	{
		if (((Control)(object)cmbgroup).Text.Trim() == "")
		{
			MessageBox.Show("Please select Analysis Group");
			cmbgroup.Focus();
			return false;
		}
		if (((Control)(object)cmbitem).Text.Trim() == "")
		{
			MessageBox.Show("Please select Analysis Parameter");
			cmbitem.Focus();
			return false;
		}
		if (txtminvalue.Text.Trim() == "")
		{
			MessageBox.Show("Min Value Required");
			txtminvalue.Focus();
			return false;
		}
		if (txtmaxvalue.Text.Trim() == "")
		{
			MessageBox.Show("Max Value Required");
			txtmaxvalue.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtminvalue.Text) == Conversion.ToDouble((object)txtmaxvalue.Text))
		{
			MessageBox.Show("Min Value cant equal to Max Value");
			txtmaxvalue.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtminvalue.Text) > Conversion.ToDouble((object)txtmaxvalue.Text))
		{
			MessageBox.Show("Min Value cant Greater than Max Value");
			txtmaxvalue.Focus();
			return false;
		}
		return true;
	}

	private void refresh()
	{
		try
		{
			cmbgroup.Focus();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			((Control)(object)cmbgroup).Text = string.Empty;
			((Control)(object)cmbitem).Text = string.Empty;
			txtminvalue.Clear();
			txtmaxvalue.Clear();
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
		//IL_0032: Expected O, but got Unknown
		try
		{
			InvLabAnalysisItems val = new InvLabAnalysisItems
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtgetAllItem = new DataTable();
			dtgetAllItem = InvLabAnalysisItems.GetAllOrById(val);
			if (dtgetAllItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtgetAllItem, cmbitem, "Id", "AnalysisParameterDescription", "Analysis Parameter Item", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GroupFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtgetAllGroup = InvLabAnalysisGroup.GetAllOrById(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dtGroup = new DataTable();
			dtGroup.Columns.Add("Id");
			dtGroup.Columns.Add("Group");
			for (int i = 0; i < dtgetAllGroup.Rows.Count; i++)
			{
				dtGroup.Rows.Add(dtgetAllGroup.Rows[i]["Id"], dtgetAllGroup.Rows[i]["AnalysisGroupDescription"]);
			}
			if (dtGroup.Rows.Count > 0)
			{
				DDL.BindDDL(dtGroup, cmbgroup, "Id", "Group", "Analysis Group", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		try
		{
			if (formvalidation())
			{
				InvLabGroupAnalysisStandards.Save(new InvLabGroupAnalysisStandards
				{
					InvLabAnalysisGroup = Conversion.ToInt(cmbgroup.Value),
					InvLabAnalysisItems = Conversion.ToInt(cmbitem.Value),
					MinValue = Conversion.ToDouble((object)txtminvalue.Text),
					MaxValue = Conversion.ToDouble((object)txtmaxvalue.Text),
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					IsEditableAfterApproval = chkEditableAfterApproval.Checked
				});
				MessageBox.Show("Save Successfully");
				gridfill();
				refresh();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		try
		{
			if (formvalidation())
			{
				InvLabGroupAnalysisStandards.Save(new InvLabGroupAnalysisStandards
				{
					Id = Conversion.ToInt((object)RecId),
					InvLabAnalysisGroup = Conversion.ToInt(cmbgroup.Value),
					InvLabAnalysisItems = Conversion.ToInt(cmbitem.Value),
					MinValue = Conversion.ToDouble((object)txtminvalue.Text),
					MaxValue = Conversion.ToDouble((object)txtmaxvalue.Text),
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					IsEditableAfterApproval = chkEditableAfterApproval.Checked
				});
				MessageBox.Show("Update Successfully");
				gridfill();
				refresh();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtgetAll = InvLabGroupAnalysisStandards.GetAllOrById(obj);
			DataTable table = new DataTable();
			table.Columns.Add("Id");
			table.Columns.Add("AnalysisGroup");
			table.Columns.Add("AnalysisItem");
			table.Columns.Add("MinValue", typeof(double));
			table.Columns.Add("MaxValue", typeof(double));
			table.Columns.Add("IsEditableAfterApproval");
			grdfrm.DataSource = null;
			table.Clear();
			grdfrm.DataSource = table;
			for (int i = 0; i < dtgetAll.Rows.Count; i++)
			{
				table.Rows.Add(dtgetAll.Rows[i]["Id"], dtgetAll.Rows[i]["AnalysisGroupDescription"], dtgetAll.Rows[i]["AnalysisParameterDescription"], dtgetAll.Rows[i]["MinValue"], dtgetAll.Rows[i]["MaxValue"], Conversion.ToBool(dtgetAll.Rows[i]["IsEditableAfterApproval"]) ? "true" : "false");
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
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdfrm.RootTable.Groups.Add("AnalysisGroup");
			grdfrm.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdfrm.RootTable.Columns["AnalysisGroup"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["AnalysisItem"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["MinValue"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["MaxValue"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["AnalysisGroup"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["AnalysisItem"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["MinValue"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["MaxValue"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdfrm.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdfrm.RootTable.Columns["AnalysisGroup"].Width = 200;
			grdfrm.RootTable.Columns["AnalysisItem"].Width = 200;
			grdfrm.RootTable.Columns["MinValue"].Width = 65;
			grdfrm.RootTable.Columns["MaxValue"].Width = 65;
			grdfrm.RootTable.Columns["IsEditableAfterApproval"].Width = 80;
			grdfrm.RootTable.Columns["Id"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvLabAnalysisGroup_Load(object sender, EventArgs e)
	{
		try
		{
			gridfill();
			GroupFill();
			ItemFill();
			btnupdate.Visible = false;
			btnsave.Visible = true;
			cmbgroup.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		try
		{
			DataTable dtgetById = InvLabGroupAnalysisStandards.GetAllOrById(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(grdfrm.CurrentRow.Cells["Id"].Value)
			});
			if (dtgetById.Rows.Count > 0)
			{
				RecId = Conversion.ToInt(grdfrm.CurrentRow.Cells["Id"].Value);
				((Control)(object)cmbgroup).Text = Conversion.ToString(dtgetById.Rows[0]["AnalysisGroupDescription"]);
				((Control)(object)cmbitem).Text = Conversion.ToString(dtgetById.Rows[0]["AnalysisParameterDescription"]);
				txtminvalue.Text = Conversion.ToString(dtgetById.Rows[0]["MinValue"]);
				txtmaxvalue.Text = Conversion.ToString(dtgetById.Rows[0]["MaxValue"]);
				chkEditableAfterApproval.Checked = Conversion.ToBool(dtgetById.Rows[0]["IsEditableAfterApproval"]);
				btnsave.Visible = false;
				btnupdate.Visible = true;
			}
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

	private void InvLabGroupAnalysisStandards_KeyDown(object sender, KeyEventArgs e)
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		refresh();
		btnupdate.Visible = false;
		btnsave.Visible = true;
		gridfill();
		ItemFill();
		GroupFill();
		cmbgroup.Focus();
	}

	private void cmbgroup_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtgetAll.Rows.Count <= 0)
			{
				return;
			}
			DataTable table = new DataTable();
			table.Columns.Add("Id");
			table.Columns.Add("AnalysisGroup");
			table.Columns.Add("AnalysisItem");
			table.Columns.Add("MinValue");
			table.Columns.Add("MaxValue");
			table.Columns.Add("IsEditableAfterApproval");
			grdfrm.DataSource = null;
			table.Clear();
			grdfrm.DataSource = table;
			for (int i = 0; i < dtgetAll.Rows.Count; i++)
			{
				if (Conversion.ToString((object)((Control)(object)cmbgroup).Text) == Conversion.ToString(dtgetAll.Rows[i]["AnalysisGroupDescription"]))
				{
					table.Rows.Add(dtgetAll.Rows[i]["Id"], dtgetAll.Rows[i]["AnalysisGroupDescription"], dtgetAll.Rows[i]["AnalysisParameterDescription"], dtgetAll.Rows[i]["MinValue"], dtgetAll.Rows[i]["MaxValue"], Conversion.ToBool(dtgetAll.Rows[i]["IsEditableAfterApproval"]) ? "true" : "false");
				}
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

	private void InvLabGroupAnalysisStandards_FormClosing(object sender, FormClosingEventArgs e)
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

	private void txtminvalue_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtmaxvalue_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance17 = new Appearance();
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
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lab.InvLabGroupAnalysisStandards));
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbgroup = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtminvalue = new System.Windows.Forms.TextBox();
		this.cmbitem = new UltraCombo();
		this.txtmaxvalue = new System.Windows.Forms.TextBox();
		this.label45 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.tsItemTaxSchedule = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.chkEditableAfterApproval = new System.Windows.Forms.CheckBox();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbgroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.tsItemTaxSchedule.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(764, 532);
		this.panel2.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 161);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(764, 371);
		this.panel5.TabIndex = 80;
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
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(764, 371);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 77;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdfrm_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 31);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(764, 130);
		this.panel4.TabIndex = 79;
		this.groupBox1.Controls.Add(this.chkEditableAfterApproval);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbgroup);
		this.groupBox1.Controls.Add(this.label44);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtminvalue);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.groupBox1.Controls.Add(this.txtmaxvalue);
		this.groupBox1.Controls.Add(this.label45);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 27);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(537, 103);
		this.groupBox1.TabIndex = 22;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Information";
		this.cmbgroup.AllowNull = (DefaultableBoolean)1;
		this.cmbgroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbgroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbgroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbgroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbgroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbgroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbgroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbgroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbgroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbgroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbgroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbgroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbgroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbgroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbgroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbgroup).Location = new System.Drawing.Point(124, 18);
		((System.Windows.Forms.Control)(object)this.cmbgroup).Name = "cmbgroup";
		((System.Windows.Forms.Control)(object)this.cmbgroup).Size = new System.Drawing.Size(244, 26);
		((System.Windows.Forms.Control)(object)this.cmbgroup).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbgroup).TextChanged += new System.EventHandler(cmbgroup_TextChanged);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(9, 24);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(86, 15);
		this.label44.TabIndex = 17;
		this.label44.Text = "Analysis Group";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(9, 79);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(60, 15);
		this.label2.TabIndex = 19;
		this.label2.Text = "Min Value";
		this.txtminvalue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtminvalue.Location = new System.Drawing.Point(124, 75);
		this.txtminvalue.Name = "txtminvalue";
		this.txtminvalue.Size = new System.Drawing.Size(94, 23);
		this.txtminvalue.TabIndex = 2;
		this.txtminvalue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtminvalue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtminvalue_KeyPress);
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(124, 46);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(244, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 1;
		this.txtmaxvalue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtmaxvalue.Location = new System.Drawing.Point(288, 75);
		this.txtmaxvalue.Name = "txtmaxvalue";
		this.txtmaxvalue.Size = new System.Drawing.Size(80, 23);
		this.txtmaxvalue.TabIndex = 3;
		this.txtmaxvalue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtmaxvalue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtmaxvalue_KeyPress);
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label45.Location = new System.Drawing.Point(219, 78);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(68, 17);
		this.label45.TabIndex = 20;
		this.label45.Text = "Max Value";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(9, 52);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(107, 15);
		this.label1.TabIndex = 16;
		this.label1.Text = "Analysis Parameter";
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(764, 27);
		this.panel6.TabIndex = 21;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(9, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(214, 19);
		this.label3.TabIndex = 22;
		this.label3.Text = "Group Analysis Standard ";
		this.panel3.Controls.Add(this.tsItemTaxSchedule);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(764, 31);
		this.panel3.TabIndex = 0;
		this.tsItemTaxSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemTaxSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemTaxSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave });
		this.tsItemTaxSchedule.Location = new System.Drawing.Point(0, 0);
		this.tsItemTaxSchedule.Name = "tsItemTaxSchedule";
		this.tsItemTaxSchedule.Size = new System.Drawing.Size(764, 27);
		this.tsItemTaxSchedule.TabIndex = 78;
		this.tsItemTaxSchedule.Text = "toolStrip3";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
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
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(764, 532);
		this.panel1.TabIndex = 1;
		this.chkEditableAfterApproval.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkEditableAfterApproval.Location = new System.Drawing.Point(371, 78);
		this.chkEditableAfterApproval.Name = "chkEditableAfterApproval";
		this.chkEditableAfterApproval.Size = new System.Drawing.Size(163, 20);
		this.chkEditableAfterApproval.TabIndex = 4;
		this.chkEditableAfterApproval.Text = "Is Editable After Approval";
		this.chkEditableAfterApproval.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(764, 532);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "InvLabGroupAnalysisStandards";
		this.Text = "InvLabGroupAnalysisStandards";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvLabGroupAnalysisStandards_FormClosing);
		base.Load += new System.EventHandler(InvLabAnalysisGroup_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvLabGroupAnalysisStandards_KeyDown);
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbgroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.tsItemTaxSchedule.ResumeLayout(false);
		this.tsItemTaxSchedule.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
