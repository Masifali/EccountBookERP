using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WebHelper.Helper;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Inventory_Reports;

public class frmSaleOrderHistoryStatus : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable Itemlst = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton print;

	private Label label23;

	internal UltraCombo CmbSupplier;

	private Label label1;

	internal UltraCombo CmbItem;

	private Label label2;

	private DateTimePicker fromdate;

	private Label label33;

	private Label label4;

	private DateTimePicker ToDate;

	private Label label3;

	private TextBox txtSrFrom;

	private TextBox txtsrTo;

	private Label label5;

	private Label label6;

	private ComboBox CmbStatus;

	private Button btnshow;

	internal GridEX grdsaleorder;

	private Button btnclose;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private Label label7;

	private ComboBox cmbIsApproved;

	public frmSaleOrderHistoryStatus(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmSaleOrderHistory_Load(object sender, EventArgs e)
	{
		SupplierNameFill();
		ItemDetailFill();
		fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
		ToDate.Value = DateTime.Today;
		CmbStatus.SelectedIndex = 0;
		btnshow_Click_1(null, null);
	}

	private void SupplierNameFill()
	{
		try
		{
			DataTable dt = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbSupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		try
		{
			Itemlst = CommonServices.ItemGetForComboServiceBind();
			if (Itemlst.Rows.Count > 0)
			{
				DDL.BindDDL(Itemlst, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reset()
	{
		((Control)(object)CmbSupplier).Text = string.Empty;
		((Control)(object)CmbItem).Text = string.Empty;
		txtSrFrom.Clear();
		txtsrTo.Clear();
		fromdate.Focus();
		btnshow_Click_1(null, null);
	}

	public void gridHisory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.AppId = UserAccount.AppId;
			obj.UserId = UserAccount.ID;
			obj.EntryUser = UserAccount.ID;
			obj.FromDate = fromdate.Value;
			obj.ToDate = ToDate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtsrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.Status = CmbStatus.Text;
			if (cmbIsApproved.Text == "Approve")
			{
				obj.IsApproved = true;
			}
			if (cmbIsApproved.Text == "UnApporve")
			{
				obj.IsApproved = false;
			}
			if (cmbIsApproved.Text == "All")
			{
				obj.ApprovedFilter = "ALL";
			}
			DataSet ds = SaleOrder.SaleOrderHistory(obj);
			DataTable grdlst = ds?.Tables[0];
			grdlst.Clear();
			if (grdlst.Rows.Count > 0)
			{
				ReportHelper.ReplaceCompLogoColumn(grdlst, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
			}
			grdsaleorder.DataSource = grdlst;
			grdsaleorder.RetrieveStructure();
			GridSettings();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void GridSettings()
	{
		try
		{
			grdsaleorder.RootTable.Columns["Id"].Visible = false;
			grdsaleorder.RootTable.Columns.Add("Status");
			grdsaleorder.RootTable.Columns["Status"].ActAsSelector = true;
			grdsaleorder.RootTable.Columns["Status"].UseHeaderSelector = false;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Reporting rpt = new Reporting();
			rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
			rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
			rpt.RptPerameter("@OrderId", "0");
			rpt.RptPerameter("@StartOrderDate", fromdate.Value.ToString());
			rpt.RptPerameter("@EndOrderDate", ToDate.Value.ToString());
			if (txtSrFrom.Text.Trim() == string.Empty)
			{
				rpt.RptPerameter("@PoSrFrom", "0");
			}
			else
			{
				rpt.RptPerameter("@PoSrFrom", txtSrFrom.Text.Trim().ToString());
			}
			if (txtsrTo.Text.Trim() == string.Empty)
			{
				rpt.RptPerameter("@PoSrTo", "0");
			}
			else
			{
				rpt.RptPerameter("@PoSrTo", txtsrTo.Text.Trim().ToString());
			}
			if (((Control)(object)CmbSupplier).Text == string.Empty)
			{
				rpt.RptPerameter("@SupplierCustomerId", "0");
			}
			else
			{
				rpt.RptPerameter("@SupplierCustomerId", CmbSupplier.Value.ToString());
			}
			if (((Control)(object)CmbItem).Text == string.Empty)
			{
				rpt.RptPerameter("@ItemId", "0");
			}
			else
			{
				rpt.RptPerameter("@ItemId", CmbItem.Value.ToString());
			}
			if (CmbStatus.Text == "Approve")
			{
				rpt.RptPerameter("@IsApproved", "true");
			}
			else
			{
				rpt.RptPerameter("@IsApproved", "false");
			}
			rpt.RptPerameter("@DocId", "81");
			rpt.ShowReport("RptSaleOderRegister.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void frmSaleOrderHistory_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.P)
		{
			print_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnNew_Click(null, null);
		}
	}

	private void btnshow_Click_1(object sender, EventArgs e)
	{
		gridHisory();
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdsaleorder).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdsaleorder).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdsaleorder.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Reporting rpt = new Reporting();
			rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
			rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
			rpt.RptPerameter("@OrderId", grdsaleorder.CurrentRow.Cells[0].Value.ToString());
			rpt.RptPerameter("@StartOrderDate", fromdate.Value.ToString());
			rpt.RptPerameter("@EndOrderDate", ToDate.Value.ToString());
			rpt.RptPerameter("@PoSrFrom", "0");
			rpt.RptPerameter("@PoSrTo", "0");
			rpt.RptPerameter("@SupplierCustomerId", "0");
			rpt.RptPerameter("@ItemId", "0");
			if (CmbStatus.Text == "Approve")
			{
				rpt.RptPerameter("@IsApproved", "true");
			}
			else
			{
				rpt.RptPerameter("@IsApproved", "false");
			}
			rpt.RptPerameter("@DocId", "81");
			rpt.RptPerameter("@SaleOrderId", grdsaleorder.CurrentRow.Cells[0].Value.ToString());
			rpt.ShowReport("RptSaleOrderSlipWithDespatchDetail.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void toolStripButton2_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Reporting rpt = new Reporting();
			rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
			rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
			rpt.RptPerameter("@OrderId", "0");
			rpt.RptPerameter("@StartOrderDate", fromdate.Value.ToString());
			rpt.RptPerameter("@EndOrderDate", ToDate.Value.ToString());
			if (txtSrFrom.Text.Trim() == string.Empty)
			{
				rpt.RptPerameter("@PoSrFrom", "0");
			}
			else
			{
				rpt.RptPerameter("@PoSrFrom", txtSrFrom.Text.Trim().ToString());
			}
			if (txtsrTo.Text.Trim() == string.Empty)
			{
				rpt.RptPerameter("@PoSrTo", "0");
			}
			else
			{
				rpt.RptPerameter("@PoSrTo", txtsrTo.Text.Trim().ToString());
			}
			if (((Control)(object)CmbSupplier).Text == string.Empty)
			{
				rpt.RptPerameter("@SupplierCustomerId", "0");
			}
			else
			{
				rpt.RptPerameter("@SupplierCustomerId", CmbSupplier.Value.ToString());
			}
			if (((Control)(object)CmbItem).Text == string.Empty)
			{
				rpt.RptPerameter("@ItemId", "0");
			}
			else
			{
				rpt.RptPerameter("@ItemId", CmbItem.Value.ToString());
			}
			if (CmbStatus.Text == "Approve")
			{
				rpt.RptPerameter("@IsApproved", "true");
			}
			else
			{
				rpt.RptPerameter("@IsApproved", "false");
			}
			rpt.RptPerameter("@DocId", "81");
			rpt.ShowReport("RptSalesOrderRegisterA.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void grdsaleorder_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		if (e.Column.Key == "Status")
		{
			ApprovalList val = new ApprovalList
			{
				AllApprovalLists = new List<ApprovalList>()
			};
			ApprovalList item = new ApprovalList();
			item.OrganizationId = UserAccount.OrganizationId;
			item.CompanyId = UserAccount.CompanyId;
			item.Id = Conversion.ToInt((object)grdsaleorder.CurrentRow.Cells[0].Value.ToString());
			item.ReqType = "Status";
			item.EntryUser = UserAccount.ID;
			val.AllApprovalLists.Add(item);
			SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
			reset();
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
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Expected O, but got Unknown
		GridEXLayout grdsaleorder_DesignTimeLayout = new GridEXLayout();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Reports.frmSaleOrderHistoryStatus));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdsaleorder = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnshow = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbStatus = new System.Windows.Forms.ComboBox();
		this.txtsrTo = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtSrFrom = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.CmbItem = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnclose = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbIsApproved = new System.Windows.Forms.ComboBox();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1238, 582);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdsaleorder);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 199);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1238, 383);
		this.panel5.TabIndex = 3;
		this.grdsaleorder.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdsaleorder).BackColor = System.Drawing.Color.White;
		grdsaleorder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdsaleorder.DesignTimeLayout = grdsaleorder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdsaleorder.FilterMode = (FilterMode)2;
		this.grdsaleorder.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdsaleorder.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Name = "grdsaleorder";
		((ScrollableBase)this.grdsaleorder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Size = new System.Drawing.Size(1238, 383);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).TabIndex = 1;
		this.grdsaleorder.TotalRow = (InheritableBoolean)1;
		this.grdsaleorder.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdsaleorder).VisualStyle = (VisualStyle)4;
		this.grdsaleorder.CellValueChanged += new ColumnActionEventHandler(grdsaleorder_CellValueChanged);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.cmbIsApproved);
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.CmbStatus);
		this.panel4.Controls.Add(this.txtsrTo);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtSrFrom);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.ToDate);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.fromdate);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 70);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1238, 129);
		this.panel4.TabIndex = 2;
		this.btnshow.Location = new System.Drawing.Point(762, 95);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(46, 20);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = true;
		this.btnshow.Click += new System.EventHandler(btnshow_Click_1);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(26, 98);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(41, 15);
		this.label6.TabIndex = 135;
		this.label6.Text = "Status";
		this.CmbStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.CmbStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.CmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.CmbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.FormattingEnabled = true;
		this.CmbStatus.Items.AddRange(new object[3] { "Open", "Complete", "Cancel" });
		this.CmbStatus.Location = new System.Drawing.Point(127, 93);
		this.CmbStatus.Name = "CmbStatus";
		this.CmbStatus.Size = new System.Drawing.Size(238, 25);
		this.CmbStatus.TabIndex = 6;
		this.txtsrTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsrTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsrTo.Location = new System.Drawing.Point(473, 38);
		this.txtsrTo.Name = "txtsrTo";
		this.txtsrTo.Size = new System.Drawing.Size(269, 23);
		this.txtsrTo.TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(374, 41);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(36, 15);
		this.label5.TabIndex = 132;
		this.label5.Text = "Sr To";
		this.txtSrFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSrFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSrFrom.Location = new System.Drawing.Point(127, 38);
		this.txtSrFrom.Name = "txtSrFrom";
		this.txtSrFrom.Size = new System.Drawing.Size(238, 23);
		this.txtSrFrom.TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(26, 41);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(51, 15);
		this.label4.TabIndex = 130;
		this.label4.Text = "Sr From";
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MM-yy";
		this.ToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(473, 15);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(269, 21);
		this.ToDate.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(373, 17);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(50, 15);
		this.label3.TabIndex = 129;
		this.label3.Text = "To Date";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MM-yy";
		this.fromdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(127, 15);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(238, 21);
		this.fromdate.TabIndex = 0;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(26, 17);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(65, 15);
		this.label33.TabIndex = 127;
		this.label33.Text = "From Date";
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(473, 63);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(269, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 5;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(371, 67);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(68, 15);
		this.label2.TabIndex = 125;
		this.label2.Text = "Item Name";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(127, 63);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(26, 67);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(90, 15);
		this.label1.TabIndex = 123;
		this.label1.Text = "Supplier Name";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.panel3.Controls.Add(this.btnclose);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 30);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1238, 40);
		this.panel3.TabIndex = 1;
		this.btnclose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnclose.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.btnclose.FlatAppearance.BorderSize = 0;
		this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnclose.Image = (System.Drawing.Image)resources.GetObject("btnclose.Image");
		this.btnclose.Location = new System.Drawing.Point(1170, 7);
		this.btnclose.Margin = new System.Windows.Forms.Padding(2);
		this.btnclose.Name = "btnclose";
		this.btnclose.Size = new System.Drawing.Size(44, 28);
		this.btnclose.TabIndex = 4;
		this.btnclose.UseVisualStyleBackColor = false;
		this.btnclose.Click += new System.EventHandler(btnclose_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(12, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(163, 24);
		this.label23.TabIndex = 3;
		this.label23.Text = "Sale Order Histroy";
		this.panel2.Controls.Add(this.ctrlGrdBar3);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1238, 30);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1193, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdsaleorder;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar3.TabIndex = 24;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.print, this.toolStripButton1, this.toolStripButton2 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(427, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(103, 24);
		this.print.Text = "&RegisterDetail";
		this.print.Click += new System.EventHandler(print_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(133, 24);
		this.toolStripButton1.Text = "&PrintWithGdnDetail";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(124, 24);
		this.toolStripButton2.Text = "RegisterSummery";
		this.toolStripButton2.Click += new System.EventHandler(toolStripButton2_Click);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(374, 98);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(70, 15);
		this.label7.TabIndex = 137;
		this.label7.Text = "Is Approved";
		this.cmbIsApproved.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cmbIsApproved.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cmbIsApproved.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cmbIsApproved.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbIsApproved.FormattingEnabled = true;
		this.cmbIsApproved.Items.AddRange(new object[3] { "Approve", "UnApporve", "All" });
		this.cmbIsApproved.Location = new System.Drawing.Point(473, 93);
		this.cmbIsApproved.Name = "cmbIsApproved";
		this.cmbIsApproved.Size = new System.Drawing.Size(269, 25);
		this.cmbIsApproved.TabIndex = 136;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1238, 582);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmSaleOrderHistoryStatus";
		this.Text = "frmSaleOrderHistory";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmSaleOrderHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleOrderHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
