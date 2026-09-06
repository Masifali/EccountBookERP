using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
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
namespace Architecture.WinApp.Export
{

public class LoadContractForSchedule : Form
{
	private bool FormInitialized;

	public int DocumentTypeId;

	public DataTable dtGrid = new DataTable();

	public DataTable dtLoader = new DataTable();

	public DataTable dtForLayout = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label13;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private Button btnLoadOnInvoice;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Label label4;

	private TextBox txtOrderNoFrom;

	private TextBox txtOrderNoTo;

	private Label label3;

	private Label label5;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton Btnreferesh;

	private ToolStripButton btnshortcutkeys;

	public LoadContractForSchedule(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void LoadPurchaseOrder_Load(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			SupplierNameFill();
			PendingDataForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierNameFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImLcOrder.GetDataForDropDownFromExportContract(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "Customer"
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbSupplier, "Id", "name", "Customer Name", true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			PendingDataForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingDataForLoad()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		try
		{
			dtGrid.Clear();
			dtForLayout.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Convert.ToDateTime(FromDate.Value);
			obj.ToDate = Convert.ToDateTime(Todate.Value);
			obj.FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			dtGrid = ExportContractSchedule.GetPendingContractsForSchedule(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DetailId", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("ContractNo");
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("CustomerName");
				dt.Columns.Add("NoOfContainers");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("MTon", typeof(double));
				dt.Columns.Add("UsedMTon", typeof(double));
				dt.Columns.Add("BalMTon", typeof(double));
				dt.Columns.Add("PackUom");
				dt.Columns.Add("NoOfBags", typeof(double));
				dt.Columns.Add("UsedNoOfBags", typeof(double));
				dt.Columns.Add("BalNoOfBags", typeof(double));
				dt.Columns.Add("PackingType");
				dtForLayout = dt.Clone();
				foreach (DataRow dr in dtGrid.Rows)
				{
					dt.Rows.Add(dr["Id"], dr["DetailId"], dr["LcOrderDate"], dr["LcOrderDocNo"], dr["ContractNo"], dr["SupplierCustomerId"], dr["CustomerName"], dr["NoOfContainers"], dr["ItemCode"], dr["ItemName"], dr["ItemQty"], dr["UsedQty"], dr["BalQty"], dr["PackUomCode"], dr["NoOfUnit"], dr["UsedNoOfUnit"], dr["BalNoOfUnit"], dr["PackingType"]);
				}
				dtForLayout = dt.Copy();
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSettings();
			}
			else
			{
				grd.ClearStructure();
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
			List<string> hiddenColumns = new List<string> { "Id", "DetailId", "SupplierCustomerId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns);
			grd.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["ContractNo"].Width = Constants.ExportConstants.ContractNoConstant;
			grd.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.PartyName + 20;
			grd.RootTable.Columns["NoOfContainers"].Width = Constants.ExportConstants.NoOfContainers;
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["MTon"].Width = Constants.ExportConstants.QtyConstant;
			grd.RootTable.Columns["UsedMTon"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["BalMTon"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["NoOfBags"].Width = Constants.ExportConstants.NoOfContainers;
			grd.RootTable.Columns["UsedNoOfBags"].Width = Constants.ExportConstants.NoOfContainers;
			grd.RootTable.Columns["BalNoOfBags"].Width = Constants.ExportConstants.NoOfContainers;
			grd.RootTable.Columns["PackingType"].Width = Constants.ExportConstants.PackingTypeConstant;
			grd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["MTon"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["MTon"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["MTon"].FormatString = "#,##0.###";
			grd.RootTable.Columns["MTon"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["UsedMTon"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["UsedMTon"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["UsedMTon"].FormatString = "#,##0.###";
			grd.RootTable.Columns["UsedMTon"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["BalMTon"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalMTon"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalMTon"].FormatString = "#,##0.###";
			grd.RootTable.Columns["BalMTon"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NoOfBags"].FormatString = "#,##0.###";
			grd.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["UsedNoOfBags"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["UsedNoOfBags"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["UsedNoOfBags"].FormatString = "#,##0.###";
			grd.RootTable.Columns["UsedNoOfBags"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["BalNoOfBags"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalNoOfBags"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalNoOfBags"].FormatString = "#,##0.###";
			grd.RootTable.Columns["BalNoOfBags"].TotalFormatString = "#,##0.###";
			GridEX_Helper.AddSelectButton(grd, "Select", 25, 0);
			grd.FrozenColumns = 1;
			((Control)(object)grd).Refresh();
			ctrlGrdBar1_Load(null, null);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtForLayout);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoad_Click(object sender, EventArgs e)
	{
		try
		{
			if (int.Parse(grd.GetCheckedRows().Count().ToString()) > 0)
			{
				int ContractId = 0;
				dtLoader = dtGrid.Clone();
				dtLoader.Rows.Clear();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					int Id = Conversion.ToInt(r.Cells["Id"].Value);
					if (ContractId == 0)
					{
						ContractId = Id;
					}
					if (ContractId != Id)
					{
						dtLoader.Rows.Clear();
						MessageBox.Show("You Can Only Select Rows Of Same Contract");
						return;
					}
					ContractId = Conversion.ToInt(r.Cells["Id"].Value);
					DataRow dr = dtGrid.Rows[r.RowIndex];
					dtLoader.Rows.Add(dr.ItemArray);
				}
				Hide();
			}
			else
			{
				MessageBox.Show("Check the Row first");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtLoader = null;
		Hide();
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		FromDate.Focus();
		((Control)(object)CmbSupplier).Text = string.Empty;
		txtOrderNoFrom.Text = string.Empty;
		txtOrderNoTo.Text = string.Empty;
		PendingDataForLoad();
	}

	private void Btnreferesh_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOrderNoTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOrderNoFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadPurchaseOrder_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				dtLoader = null;
				Hide();
			}
			else if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoad_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				btnReset_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				Btnreferesh_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDate.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDate.Focus();
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
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+L", "To Press Load Button");
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null && e.Control)
			{
				_ = e.KeyCode;
				_ = 32;
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.LoadContractForSchedule));
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.Btnreferesh = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.txtOrderNoTo = new System.Windows.Forms.TextBox();
		this.txtOrderNoFrom = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(884, 53);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.Btnreferesh, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(884, 27);
		this.toolStrip1.TabIndex = 25;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnReset_Click);
		this.Btnreferesh.Image = (System.Drawing.Image)resources.GetObject("Btnreferesh.Image");
		this.Btnreferesh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Btnreferesh.Name = "Btnreferesh";
		this.Btnreferesh.Size = new System.Drawing.Size(80, 24);
		this.Btnreferesh.Text = "&Refresh";
		this.Btnreferesh.Click += new System.EventHandler(Btnreferesh_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 30);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(232, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Contract Load For Schedule";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(835, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 21);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 130);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(884, 331);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.txtOrderNoTo);
		this.panel2.Controls.Add(this.txtOrderNoFrom);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel2.Controls.Add(this.btnLoadOnInvoice);
		this.panel2.Controls.Add(this.btngrnlod);
		this.panel2.Controls.Add(this.Todate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 53);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(884, 55);
		this.panel2.TabIndex = 0;
		this.txtOrderNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoTo.Location = new System.Drawing.Point(363, 26);
		this.txtOrderNoTo.Name = "txtOrderNoTo";
		this.txtOrderNoTo.Size = new System.Drawing.Size(93, 23);
		this.txtOrderNoTo.TabIndex = 3;
		this.txtOrderNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoTo_KeyPress);
		this.txtOrderNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoFrom.Location = new System.Drawing.Point(269, 26);
		this.txtOrderNoFrom.Name = "txtOrderNoFrom";
		this.txtOrderNoFrom.Size = new System.Drawing.Size(93, 23);
		this.txtOrderNoFrom.TabIndex = 2;
		this.txtOrderNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoFrom_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(363, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 28;
		this.label3.Text = "To Doc No";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(269, 6);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(79, 15);
		this.label4.TabIndex = 28;
		this.label4.Text = "From Doc No";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(460, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Customer Name";
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
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
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
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(457, 24);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(243, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 4;
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(763, 24);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(59, 26);
		this.btnLoadOnInvoice.TabIndex = 6;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoad_Click);
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(700, 24);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(64, 26);
		this.btngrnlod.TabIndex = 5;
		this.btngrnlod.Text = "Show";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btnShow_Click);
		this.Todate.CustomFormat = "dd-MMM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(139, 26);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(129, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(8, 26);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(129, 23);
		this.FromDate.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(139, 6);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 11;
		this.label13.Text = "To Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.ForeColor = System.Drawing.Color.White;
		this.panel3.Location = new System.Drawing.Point(0, 108);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(884, 22);
		this.panel3.TabIndex = 31;
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 1);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 18);
		this.label5.TabIndex = 24;
		this.label5.Text = "Filtered";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(884, 461);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "LoadContractForSchedule";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadPurchaseOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadPurchaseOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
