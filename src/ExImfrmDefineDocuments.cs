using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
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
namespace Architecture.WinApp.Export
{

public class ExImfrmDefineDocuments : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecIdDoc;

	private int RecIdGrp;

	private int RecIdDocGrp;

	private bool UpdateModedocList;

	private bool UpdateModeGroupList;

	private bool UpdateModedocGroupList;

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Panel panel4;

	private Panel panel5;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton toolStripButton6;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private Panel panel7;

	private Panel panel6;

	private GroupBox groupBox1;

	private TextBox txtDocName;

	private Button btnAddinGrid;

	private Label label34;

	private TextBox txtDocCode;

	private Label label32;

	internal GridEX grdDocList;

	private Panel panel8;

	private GroupBox groupBox2;

	private Panel panel9;

	internal GridEX grdDocGroup;

	private TextBox txtgrpName;

	private Button btnSaveGroup;

	private Label label1;

	private TextBox txtGrpCode;

	private Label label2;

	private GroupBox groupBox3;

	private Button btnSavedoctoGroup;

	private Label label3;

	private Label label4;

	internal UltraCombo cmbDocName;

	internal UltraCombo cmbGroupCode;

	internal GridEX grdAssignDocToGrp;

	private Panel panel14;

	private Panel panel11;

	private Panel panel15;

	private Panel panel10;

	private Label label7;

	private TextBox txtcopies;

	private Label label6;

	private TextBox txtorginal;

	public ExImfrmDefineDocuments(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmList_Load(object sender, EventArgs e)
	{
		GroupCodeBind();
		DocNameBind();
		DocListFill();
		GroupListFill();
		AssigndoctogrpFill();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		DocGroupListReset();
		DocListReset();
		GroupListReset();
		GroupCodeBind();
		DocNameBind();
	}

	private void DocListFill()
	{
		try
		{
			DataTable lst = ExImShipmentDocuments.Getall();
			DataTable dtDoclist = new DataTable();
			dtDoclist.Columns.Add("Id");
			dtDoclist.Columns.Add("DocCode");
			dtDoclist.Columns.Add("DocumentName");
			if (lst.Rows.Count > 0)
			{
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtDoclist.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["exImDocCode"], lst.Rows[i]["exImDocName"]);
					grdDocList.DataSource = dtDoclist;
					grdDocList.RetrieveStructure();
					grdDocList.RootTable.Columns["Id"].Visible = false;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GroupListFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = ExImShipmentDocGroup.Getall(new ExImShipmentDocGroup
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dtGrouplist = new DataTable();
			dtGrouplist.Columns.Add("Id");
			dtGrouplist.Columns.Add("GroupCode");
			dtGrouplist.Columns.Add("GroupName");
			if (lst.Rows.Count > 0)
			{
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtGrouplist.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["ExImDocGroupCode"], lst.Rows[i]["ExImDocGroupName"]);
					grdDocGroup.DataSource = dtGrouplist;
					grdDocGroup.RetrieveStructure();
					grdDocGroup.RootTable.Columns["Id"].Visible = false;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AssigndoctogrpFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = ExImShipmentDocGroupSchedule.Getall(new ExImShipmentDocGroupSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dtlist = new DataTable();
			dtlist.Columns.Add("Id");
			dtlist.Columns.Add("GroupCode");
			dtlist.Columns.Add("DocumentName");
			dtlist.Columns.Add("Copies");
			dtlist.Columns.Add("Orignial");
			if (lst.Rows.Count > 0)
			{
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtlist.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["ExImDocGroupCode"], lst.Rows[i]["exImDocName"], lst.Rows[i]["Copies"], lst.Rows[i]["Original"]);
					grdAssignDocToGrp.DataSource = dtlist;
					grdAssignDocToGrp.RetrieveStructure();
					grdAssignDocToGrp.RootTable.Columns["Id"].Visible = false;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GroupCodeBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = ExImShipmentDocGroup.Getall(new ExImShipmentDocGroup
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, cmbGroupCode, "Id", "ExImDocGroupCode", "Group Code", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocNameBind()
	{
		try
		{
			DataTable lst = ExImShipmentDocuments.Getall();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, cmbDocName, "Id", "exImDocName", "Document Name ", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddinGrid_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImShipmentDocuments shipmentDocuments = new ExImShipmentDocuments();
			if (RecIdDoc > 0 && UpdateModedocList)
			{
				shipmentDocuments.Id = RecIdDoc;
			}
			shipmentDocuments.exImDocCode = txtDocCode.Text;
			shipmentDocuments.exImDocName = txtDocName.Text;
			int success = ExImShipmentDocuments.Save(shipmentDocuments);
			if (!UpdateModedocList && success > 0)
			{
				MessageBox.Show("Save SuccessFully");
				DocListReset();
				txtDocCode.Focus();
			}
			if (UpdateModedocList && success > 0)
			{
				MessageBox.Show("Update SuccessFully");
				DocListReset();
				txtDocCode.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocListReset()
	{
		DocListFill();
		DocNameBind();
		txtDocCode.Text = string.Empty;
		txtDocName.Text = string.Empty;
		UpdateModedocList = false;
		RecIdDoc = 0;
	}

	private void grdDocList_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt((object)grdDocList.CurrentRow.Cells[0].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			ExImShipmentDocuments shipmentDocuments = new ExImShipmentDocuments();
			RecIdDoc = Id;
			shipmentDocuments = ExImShipmentDocuments.GetByID(RecIdDoc);
			if (shipmentDocuments != null)
			{
				txtDocCode.Text = shipmentDocuments.exImDocCode;
				txtDocName.Text = shipmentDocuments.exImDocName;
				UpdateModedocList = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveGroup_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImShipmentDocGroup shipmentDocuments = new ExImShipmentDocGroup();
			if (RecIdGrp > 0 && UpdateModeGroupList)
			{
				shipmentDocuments.Id = RecIdGrp;
			}
			shipmentDocuments.ExImDocGroupCode = txtGrpCode.Text;
			shipmentDocuments.ExImDocGroupName = txtgrpName.Text;
			int success = ExImShipmentDocGroup.Save(shipmentDocuments);
			if (!UpdateModeGroupList && success > 0)
			{
				MessageBox.Show("Save SuccessFully");
				GroupListReset();
				txtGrpCode.Focus();
			}
			if (UpdateModeGroupList && success > 0)
			{
				MessageBox.Show("Update SuccessFully");
				GroupListReset();
				txtGrpCode.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GroupListReset()
	{
		GroupListFill();
		GroupCodeBind();
		txtGrpCode.Text = string.Empty;
		txtgrpName.Text = string.Empty;
		UpdateModeGroupList = false;
		RecIdGrp = 0;
	}

	private void grdDocGroup_DoubleClick(object sender, EventArgs e)
	{
		ReadByGrpId(Conversion.ToInt((object)grdDocGroup.CurrentRow.Cells[0].Value.ToString()));
	}

	public void ReadByGrpId(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImShipmentDocGroup shipmentDocuments = new ExImShipmentDocGroup();
			RecIdGrp = Id;
			shipmentDocuments = ExImShipmentDocGroup.GetByID(RecIdGrp);
			if (shipmentDocuments != null)
			{
				txtGrpCode.Text = shipmentDocuments.ExImDocGroupCode;
				txtgrpName.Text = shipmentDocuments.ExImDocGroupName;
				UpdateModeGroupList = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSavedoctoGroup_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImShipmentDocGroupSchedule groupSchedule = new ExImShipmentDocGroupSchedule();
			if (RecIdDocGrp > 0 && UpdateModedocGroupList)
			{
				groupSchedule.Id = RecIdDocGrp;
			}
			groupSchedule.ExImShipmentDocGroupId = Conversion.ToInt(cmbGroupCode.Value);
			groupSchedule.ExImShipmentDocuments = Conversion.ToInt(cmbDocName.Value);
			groupSchedule.Copies = Conversion.ToInt((object)txtcopies.Text);
			groupSchedule.Original = Conversion.ToInt((object)txtorginal.Text);
			int success = ExImShipmentDocGroupSchedule.Save(groupSchedule);
			if (!UpdateModedocGroupList && success > 0)
			{
				MessageBox.Show("Save Successfully");
				DocGroupListReset();
				cmbGroupCode.Focus();
			}
			if (UpdateModedocGroupList && success > 0)
			{
				MessageBox.Show("Update SuccessFully");
				DocGroupListReset();
				cmbGroupCode.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocGroupListReset()
	{
		((Control)(object)cmbDocName).Text = string.Empty;
		((Control)(object)cmbGroupCode).Text = string.Empty;
		txtorginal.Text = string.Empty;
		txtcopies.Text = string.Empty;
		GroupCodeBind();
		DocNameBind();
		AssigndoctogrpFill();
		UpdateModedocGroupList = false;
		RecIdDocGrp = 0;
	}

	private void grdAssignDocToGrp_DoubleClick(object sender, EventArgs e)
	{
		ReadByDocGrpId(Conversion.ToInt((object)grdAssignDocToGrp.CurrentRow.Cells[0].Value.ToString()));
	}

	public void ReadByDocGrpId(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImShipmentDocGroupSchedule shipmentDocuments = new ExImShipmentDocGroupSchedule();
			RecIdDocGrp = Id;
			shipmentDocuments = ExImShipmentDocGroupSchedule.GetByID(RecIdDocGrp);
			if (shipmentDocuments != null)
			{
				cmbDocName.Value = shipmentDocuments.ExImShipmentDocuments;
				cmbGroupCode.Value = shipmentDocuments.ExImShipmentDocGroupId;
				txtcopies.Text = shipmentDocuments.Copies.ToString();
				txtorginal.Text = shipmentDocuments.Original.ToString();
				UpdateModedocGroupList = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbGroupCode_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
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
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		GridEXLayout grdAssignDocToGrp_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdDocGroup_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDocList_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.ExImfrmDefineDocuments));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grdAssignDocToGrp = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtcopies = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtorginal = new System.Windows.Forms.TextBox();
		this.cmbDocName = new UltraCombo();
		this.cmbGroupCode = new UltraCombo();
		this.btnSavedoctoGroup = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdDocGroup = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtgrpName = new System.Windows.Forms.TextBox();
		this.btnSaveGroup = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.txtGrpCode = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdDocList = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtDocName = new System.Windows.Forms.TextBox();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.label34 = new System.Windows.Forms.Label();
		this.txtDocCode = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAssignDocToGrp).BeginInit();
		this.panel11.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDocName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGroupCode).BeginInit();
		this.panel4.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDocGroup).BeginInit();
		this.panel8.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDocList).BeginInit();
		this.panel6.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel10);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1329, 436);
		this.panel1.TabIndex = 0;
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 436);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1329, 0);
		this.panel10.TabIndex = 2;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel14);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1329, 408);
		this.panel3.TabIndex = 1;
		this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel14.Controls.Add(this.panel15);
		this.panel14.Controls.Add(this.panel11);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel14.Location = new System.Drawing.Point(668, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(659, 408);
		this.panel14.TabIndex = 3;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdAssignDocToGrp);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(0, 101);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(657, 305);
		this.panel15.TabIndex = 8;
		this.grdAssignDocToGrp.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdAssignDocToGrp).BackColor = System.Drawing.Color.White;
		this.grdAssignDocToGrp.ColumnAutoResize = true;
		grdAssignDocToGrp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAssignDocToGrp.DesignTimeLayout = grdAssignDocToGrp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAssignDocToGrp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdAssignDocToGrp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAssignDocToGrp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAssignDocToGrp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdAssignDocToGrp).Name = "grdAssignDocToGrp";
		((ScrollableBase)this.grdAssignDocToGrp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAssignDocToGrp).Size = new System.Drawing.Size(657, 305);
		((System.Windows.Forms.Control)(object)this.grdAssignDocToGrp).TabIndex = 6;
		this.grdAssignDocToGrp.TotalRow = (InheritableBoolean)1;
		this.grdAssignDocToGrp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAssignDocToGrp).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdAssignDocToGrp).DoubleClick += new System.EventHandler(grdAssignDocToGrp_DoubleClick);
		this.panel11.BackColor = System.Drawing.Color.White;
		this.panel11.Controls.Add(this.groupBox3);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(657, 101);
		this.panel11.TabIndex = 0;
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.txtcopies);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.txtorginal);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDocName);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGroupCode);
		this.groupBox3.Controls.Add(this.btnSavedoctoGroup);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.label4);
		this.groupBox3.Location = new System.Drawing.Point(5, 6);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(649, 89);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Assign Documetn To Group";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(399, 33);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(48, 17);
		this.label7.TabIndex = 40;
		this.label7.Text = "Copies";
		this.txtcopies.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcopies.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcopies.Location = new System.Drawing.Point(399, 52);
		this.txtcopies.Name = "txtcopies";
		this.txtcopies.Size = new System.Drawing.Size(65, 22);
		this.txtcopies.TabIndex = 4;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(326, 33);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(51, 17);
		this.label6.TabIndex = 38;
		this.label6.Text = "Orignal";
		this.txtorginal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtorginal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtorginal.Location = new System.Drawing.Point(328, 53);
		this.txtorginal.Name = "txtorginal";
		this.txtorginal.Size = new System.Drawing.Size(65, 22);
		this.txtorginal.TabIndex = 3;
		this.cmbDocName.AllowNull = (DefaultableBoolean)1;
		this.cmbDocName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDocName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDocName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDocName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDocName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbDocName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDocName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDocName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbDocName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDocName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDocName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbDocName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDocName).Location = new System.Drawing.Point(169, 53);
		((System.Windows.Forms.Control)(object)this.cmbDocName).Name = "cmbDocName";
		((System.Windows.Forms.Control)(object)this.cmbDocName).Size = new System.Drawing.Size(153, 22);
		((System.Windows.Forms.Control)(object)this.cmbDocName).TabIndex = 1;
		this.cmbGroupCode.AllowNull = (DefaultableBoolean)1;
		this.cmbGroupCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGroupCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGroupCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGroupCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbGroupCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGroupCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbGroupCode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGroupCode).Location = new System.Drawing.Point(10, 53);
		((System.Windows.Forms.Control)(object)this.cmbGroupCode).Name = "cmbGroupCode";
		((System.Windows.Forms.Control)(object)this.cmbGroupCode).Size = new System.Drawing.Size(153, 22);
		((System.Windows.Forms.Control)(object)this.cmbGroupCode).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbGroupCode).TextChanged += new System.EventHandler(cmbGroupCode_TextChanged);
		this.btnSavedoctoGroup.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnSavedoctoGroup.Location = new System.Drawing.Point(470, 52);
		this.btnSavedoctoGroup.Name = "btnSavedoctoGroup";
		this.btnSavedoctoGroup.Size = new System.Drawing.Size(27, 22);
		this.btnSavedoctoGroup.TabIndex = 5;
		this.btnSavedoctoGroup.UseVisualStyleBackColor = true;
		this.btnSavedoctoGroup.Click += new System.EventHandler(btnSavedoctoGroup_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(167, 31);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(106, 17);
		this.label3.TabIndex = 32;
		this.label3.Text = "Document Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(7, 31);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(80, 17);
		this.label4.TabIndex = 31;
		this.label4.Text = "Group Code";
		this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel4.Controls.Add(this.panel9);
		this.panel4.Controls.Add(this.panel8);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(331, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(337, 408);
		this.panel4.TabIndex = 3;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdDocGroup);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 101);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(335, 305);
		this.panel9.TabIndex = 1;
		this.grdDocGroup.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdDocGroup).BackColor = System.Drawing.Color.White;
		this.grdDocGroup.ColumnAutoResize = true;
		grdDocGroup_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDocGroup.DesignTimeLayout = grdDocGroup_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDocGroup).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDocGroup).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDocGroup.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDocGroup).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDocGroup).Name = "grdDocGroup";
		((ScrollableBase)this.grdDocGroup).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDocGroup).Size = new System.Drawing.Size(335, 305);
		((System.Windows.Forms.Control)(object)this.grdDocGroup).TabIndex = 5;
		this.grdDocGroup.TotalRow = (InheritableBoolean)1;
		this.grdDocGroup.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDocGroup).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdDocGroup).DoubleClick += new System.EventHandler(grdDocGroup_DoubleClick);
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add(this.groupBox2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(335, 101);
		this.panel8.TabIndex = 0;
		this.groupBox2.BackColor = System.Drawing.Color.White;
		this.groupBox2.Controls.Add(this.txtgrpName);
		this.groupBox2.Controls.Add(this.btnSaveGroup);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.txtGrpCode);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Location = new System.Drawing.Point(6, 6);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(302, 89);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Document Group";
		this.txtgrpName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgrpName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgrpName.Location = new System.Drawing.Point(101, 50);
		this.txtgrpName.Name = "txtgrpName";
		this.txtgrpName.Size = new System.Drawing.Size(148, 22);
		this.txtgrpName.TabIndex = 1;
		this.btnSaveGroup.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnSaveGroup.Location = new System.Drawing.Point(255, 50);
		this.btnSaveGroup.Name = "btnSaveGroup";
		this.btnSaveGroup.Size = new System.Drawing.Size(27, 22);
		this.btnSaveGroup.TabIndex = 2;
		this.btnSaveGroup.UseVisualStyleBackColor = true;
		this.btnSaveGroup.Click += new System.EventHandler(btnSaveGroup_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(98, 26);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(84, 17);
		this.label1.TabIndex = 32;
		this.label1.Text = "Group Name";
		this.txtGrpCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrpCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrpCode.Location = new System.Drawing.Point(6, 49);
		this.txtGrpCode.Name = "txtGrpCode";
		this.txtGrpCode.Size = new System.Drawing.Size(89, 22);
		this.txtGrpCode.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 26);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(80, 17);
		this.label2.TabIndex = 31;
		this.label2.Text = "Group Code";
		this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(331, 408);
		this.panel5.TabIndex = 2;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDocList);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 101);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(329, 305);
		this.panel7.TabIndex = 1;
		this.grdDocList.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdDocList).BackColor = System.Drawing.Color.White;
		this.grdDocList.ColumnAutoResize = true;
		grdDocList_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDocList.DesignTimeLayout = grdDocList_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDocList).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDocList).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDocList.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDocList).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDocList).Name = "grdDocList";
		((ScrollableBase)this.grdDocList).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDocList).Size = new System.Drawing.Size(329, 305);
		((System.Windows.Forms.Control)(object)this.grdDocList).TabIndex = 4;
		this.grdDocList.TotalRow = (InheritableBoolean)1;
		this.grdDocList.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDocList).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdDocList).DoubleClick += new System.EventHandler(grdDocList_DoubleClick);
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.groupBox1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(329, 101);
		this.panel6.TabIndex = 0;
		this.groupBox1.Controls.Add(this.txtDocName);
		this.groupBox1.Controls.Add(this.btnAddinGrid);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.txtDocCode);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Location = new System.Drawing.Point(8, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(300, 89);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Document List";
		this.txtDocName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocName.Location = new System.Drawing.Point(102, 50);
		this.txtDocName.Name = "txtDocName";
		this.txtDocName.Size = new System.Drawing.Size(148, 22);
		this.txtDocName.TabIndex = 1;
		this.btnAddinGrid.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnAddinGrid.Location = new System.Drawing.Point(256, 50);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(27, 22);
		this.btnAddinGrid.TabIndex = 2;
		this.btnAddinGrid.UseVisualStyleBackColor = true;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(99, 26);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(106, 17);
		this.label34.TabIndex = 27;
		this.label34.Text = "Document Name";
		this.txtDocCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocCode.Location = new System.Drawing.Point(7, 49);
		this.txtDocCode.Name = "txtDocCode";
		this.txtDocCode.Size = new System.Drawing.Size(89, 22);
		this.txtDocCode.TabIndex = 0;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(6, 26);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(66, 17);
		this.label32.TabIndex = 26;
		this.label32.Text = "Doc Code";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1329, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.toolStripButton6, this.btnnew, this.btnsave, this.btnUpdate, this.toolStripSeparator });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1329, 28);
		this.toolStrip1.TabIndex = 6;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton6.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(142, 25);
		this.toolStripButton6.Text = "Define Documents";
		this.toolStripButton6.ToolTipText = "Define Documents    ";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 25);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 25);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 25);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 28);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1329, 436);
		base.Controls.Add(this.panel1);
		base.Name = "ExImfrmDefineDocuments";
		this.Text = "Define Documents";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmList_Load);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAssignDocToGrp).EndInit();
		this.panel11.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDocName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGroupCode).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDocGroup).EndInit();
		this.panel8.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDocList).EndInit();
		this.panel6.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
