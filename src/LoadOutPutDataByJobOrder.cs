using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Steel.Production;
using Architecture.Common;
using Architecture.Model;
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
namespace Architecture.WinApp.Steel.Production
{

public class LoadOutPutDataByJobOrder : Form
{
	private bool DoHaveCanRateandAmount;

	private DropDownBind DDL = new DropDownBind();

	private int[] AryColWidth;

	public int JobOrderId;

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtIssuance = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnJobOrderDefine;

	public LoadOutPutDataByJobOrder(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		try
		{
			dtIssuance.Columns.Add("Id", typeof(int));
			dtIssuance.Columns.Add("JobOrderId", typeof(int));
			dtIssuance.Columns.Add("EntryTypeId", typeof(int));
			dtIssuance.Columns.Add("EntryType", typeof(string));
			dtIssuance.Columns.Add("WarehouseId", typeof(int));
			dtIssuance.Columns.Add("WareHouseName", typeof(string));
			dtIssuance.Columns.Add("JobLotId", typeof(int));
			dtIssuance.Columns.Add("JobLot", typeof(string));
			dtIssuance.Columns.Add("ItemId", typeof(int));
			dtIssuance.Columns.Add("ItemName", typeof(string));
			dtIssuance.Columns.Add("InvPackingTypeId", typeof(int));
			dtIssuance.Columns.Add("PackTypeDesc", typeof(string));
			dtIssuance.Columns.Add("Qty", typeof(double));
			dtIssuance.Columns.Add("PackUomId", typeof(int));
			dtIssuance.Columns.Add("OuterPackUom", typeof(string));
			dtIssuance.Columns.Add("OuterPackUomEquivalent", typeof(double));
			dtIssuance.Columns.Add("NetWeight", typeof(double));
			dtIssuance.Columns.Add("ItemRate", typeof(double));
			dtIssuance.Columns.Add("RateUomId", typeof(int));
			dtIssuance.Columns.Add("RateUom", typeof(string));
			dtIssuance.Columns.Add("RateUomEquivalent", typeof(double));
			dtIssuance.Columns.Add("ItemAmount", typeof(double));
			dtIssuance.Columns.Add("UsedInProdQty", typeof(double));
			dtIssuance.Columns.Add("UsedInProdWeight", typeof(double));
			dtIssuance.Columns.Add("UsedInProdAmount", typeof(double));
			dtIssuance.Columns.Add("BalQty", typeof(double));
			dtIssuance.Columns.Add("BalWeight", typeof(double));
			dtIssuance.Columns.Add("BalAmount", typeof(double));
			dtIssuance.Columns.Add("OHExpense", typeof(double));
			dtIssuance.Columns.Add("OHGeneralExpense", typeof(double));
			dtIssuance.Columns.Add("PMExpense", typeof(double));
			dtIssuance.Columns.Add("PMGeneralExpense", typeof(double));
			dtIssuance.Columns.Add("NetRate", typeof(double));
			dtIssuance.Columns.Add("NetAmount", typeof(double));
			dtIssuance.Columns.Add("UsedInProdNetAmount", typeof(double));
			dtIssuance.Columns.Add("BalNetAmount", typeof(double));
			dtIssuance.Columns.Add("RemarksDetail", typeof(string));
			PendingInventoryTransactionsForIssuanceLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingInventoryTransactionsForIssuanceLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		try
		{
			DataTable dthis = InvProductionJobOrder.LoadOutPutDataByInvProductionJobOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = JobOrderId,
				ActionId = 1
			});
			if (dthis.Rows.Count > 0)
			{
				DataTable dtgrdinput = new DataTable();
				dtgrdinput.Columns.Add("Id", typeof(int));
				dtgrdinput.Columns.Add("JobOrderId", typeof(int));
				dtgrdinput.Columns.Add("EntryTypeId", typeof(int));
				dtgrdinput.Columns.Add("EntryType", typeof(string));
				dtgrdinput.Columns.Add("WarehouseId", typeof(int));
				dtgrdinput.Columns.Add("WareHouseName", typeof(string));
				dtgrdinput.Columns.Add("JobLotId", typeof(int));
				dtgrdinput.Columns.Add("JobLot", typeof(string));
				dtgrdinput.Columns.Add("ItemId", typeof(int));
				dtgrdinput.Columns.Add("ItemName", typeof(string));
				dtgrdinput.Columns.Add("InvPackingTypeId", typeof(int));
				dtgrdinput.Columns.Add("PackingType", typeof(string));
				dtgrdinput.Columns.Add("Qty", typeof(double));
				dtgrdinput.Columns.Add("PackUomId", typeof(string));
				dtgrdinput.Columns.Add("OuterPackUom", typeof(string));
				dtgrdinput.Columns.Add("OuterPackUomEquivalent", typeof(int));
				dtgrdinput.Columns.Add("NetWeight", typeof(double));
				dtgrdinput.Columns.Add("ItemRate", typeof(double));
				dtgrdinput.Columns.Add("RateUomId", typeof(int));
				dtgrdinput.Columns.Add("RateUom", typeof(string));
				dtgrdinput.Columns.Add("RateUomEquivalent", typeof(double));
				dtgrdinput.Columns.Add("ItemAmount", typeof(double));
				dtgrdinput.Columns.Add("UsedInProdQty", typeof(double));
				dtgrdinput.Columns.Add("UsedInProdWeight", typeof(double));
				dtgrdinput.Columns.Add("UsedInProdAmount", typeof(double));
				dtgrdinput.Columns.Add("balQty", typeof(double));
				dtgrdinput.Columns.Add("balWeight", typeof(double));
				dtgrdinput.Columns.Add("balAmount", typeof(double));
				dtgrdinput.Columns.Add("OHExpense", typeof(double));
				dtgrdinput.Columns.Add("OHGeneralExpense", typeof(double));
				dtgrdinput.Columns.Add("PMExpense", typeof(double));
				dtgrdinput.Columns.Add("PMGeneralExpense", typeof(double));
				dtgrdinput.Columns.Add("NetRate", typeof(double));
				dtgrdinput.Columns.Add("NetAmount", typeof(double));
				dtgrdinput.Columns.Add("UsedInProdNetAmount", typeof(double));
				dtgrdinput.Columns.Add("balNetAmount", typeof(double));
				dtgrdinput.Columns.Add("Remarks", typeof(string));
				for (int i = 0; i < dthis.Rows.Count; i++)
				{
					dtgrdinput.Rows.Add(Conversion.ToInt(dthis.Rows[i]["Id"]), Conversion.ToInt(dthis.Rows[i]["InvProductionJobOrderId"]), Conversion.ToInt(dthis.Rows[i]["EntryTypeId"]), Conversion.ToString(dthis.Rows[i]["EntryType"]), Conversion.ToInt(dthis.Rows[i]["WarehouseId"]), Conversion.ToString(dthis.Rows[i]["WareHouseName"]), Conversion.ToInt(dthis.Rows[i]["JobLotId"]), Conversion.ToString(dthis.Rows[i]["JobLotDescription"]), Conversion.ToInt(dthis.Rows[i]["ItemId"]), Conversion.ToString(dthis.Rows[i]["ItemName"]), Conversion.ToInt(dthis.Rows[i]["InvPackingTypeId"]), Conversion.ToString(dthis.Rows[i]["PackTypeDesc"]), Conversion.ToDouble(dthis.Rows[i]["Qty"]), Conversion.ToString(dthis.Rows[i]["PackUomId"]), Conversion.ToString(dthis.Rows[i]["OuterPackUom"]), Conversion.ToInt(dthis.Rows[i]["OuterPackUomEquivalent"]), Conversion.ToDouble(dthis.Rows[i]["NetWeight"]), Conversion.ToDouble(dthis.Rows[i]["ItemRate"]), Conversion.ToInt(dthis.Rows[i]["RateUomId"]), Conversion.ToString(dthis.Rows[i]["RateUom"]), Conversion.ToDouble(dthis.Rows[i]["RateUomEquivalent"]), Conversion.ToDouble(dthis.Rows[i]["ItemAmount"]), Conversion.ToDouble(dthis.Rows[i]["UsedInProdQty"]), Conversion.ToDouble(dthis.Rows[i]["UsedInProdWeight"]), Conversion.ToDouble(dthis.Rows[i]["UsedInProdAmount"]), Conversion.ToDouble(dthis.Rows[i]["balQty"]), Conversion.ToDouble(dthis.Rows[i]["balWeight"]), Conversion.ToDouble(dthis.Rows[i]["balAmount"]), Conversion.ToDouble(dthis.Rows[i]["OHExpense"]), Conversion.ToDouble(dthis.Rows[i]["OHGeneralExpense"]), Conversion.ToDouble(dthis.Rows[i]["PMExpense"]), Conversion.ToDouble(dthis.Rows[i]["PMGeneralExpense"]), Conversion.ToDouble(dthis.Rows[i]["NetRate"]), Conversion.ToDouble(dthis.Rows[i]["NetAmount"]), Conversion.ToDouble(dthis.Rows[i]["UsedInProdNetAmount"]), Conversion.ToDouble(dthis.Rows[i]["balNetAmount"]), Conversion.ToString(dthis.Rows[i]["RemarksDetail"]));
				}
				grd.DataSource = dtgrdinput;
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdSettings()
	{
		try
		{
			if (!FoodProductionWithValuesNew_St.ValuesShowRights)
			{
				grd.RootTable.Columns["ItemAmount"].Visible = false;
				grd.RootTable.Columns["ItemRate"].Visible = false;
				grd.RootTable.Columns["OHExpense"].Visible = false;
				grd.RootTable.Columns["OHGeneralExpense"].Visible = false;
				grd.RootTable.Columns["PMExpense"].Visible = false;
				grd.RootTable.Columns["PMGeneralExpense"].Visible = false;
				grd.RootTable.Columns["NetRate"].Visible = false;
				grd.RootTable.Columns["NetAmount"].Visible = false;
			}
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["JobOrderId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["EntryTypeId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["RateUomEquivalent"].Visible = false;
			grd.RootTable.Columns["OuterPackUomEquivalent"].Visible = false;
			grd.RootTable.Columns["InvPackingTypeId"].Visible = false;
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["NetRate"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["NetRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["OHExpense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OHExpense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OHExpense"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["OHExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["OHGeneralExpense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OHGeneralExpense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OHGeneralExpense"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["OHGeneralExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["PMExpense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["PMExpense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PMExpense"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["PMExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["PMGeneralExpense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["PMGeneralExpense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PMGeneralExpense"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["PMGeneralExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["NetAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetAmount"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["NetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Width = 40;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
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

	private void btnReset_Click(object sender, EventArgs e)
	{
		PendingInventoryTransactionsForIssuanceLoad();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtIssuance = null;
		Hide();
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (int.Parse(grd.GetCheckedRows().Count().ToString()) > 0)
			{
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					dtIssuance.Rows.Add(Conversion.ToInt(r.Cells["Id"].Value), Conversion.ToInt(r.Cells["JobOrderId"].Value), Conversion.ToInt(r.Cells["EntryTypeId"].Value), Conversion.ToString((object)r.Cells["EntryType"].Text), Conversion.ToInt(r.Cells["WarehouseId"].Value), Conversion.ToString((object)r.Cells["WareHouseName"].Text), Conversion.ToInt(r.Cells["JobLotId"].Value), Conversion.ToString((object)r.Cells["JobLot"].Text), Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString((object)r.Cells["ItemName"].Text), Conversion.ToInt(r.Cells["InvPackingTypeId"].Value), Conversion.ToString((object)r.Cells["PackingType"].Text), Conversion.ToDouble(r.Cells["Qty"].Value), Conversion.ToInt(r.Cells["PackUomId"].Value), Conversion.ToString((object)r.Cells["OuterPackUom"].Text), Conversion.ToString((object)r.Cells["OuterPackUomEquivalent"].Text), Conversion.ToDouble(r.Cells["NetWeight"].Value), Conversion.ToDouble(r.Cells["ItemRate"].Value), Conversion.ToInt(r.Cells["RateUomId"].Value), Conversion.ToString((object)r.Cells["RateUom"].Text), Conversion.ToString((object)r.Cells["RateUomEquivalent"].Text), Conversion.ToDouble(r.Cells["ItemAmount"].Value), Conversion.ToDouble(r.Cells["UsedInProdQty"].Value), Conversion.ToDouble(r.Cells["UsedInProdWeight"].Value), Conversion.ToDouble(r.Cells["UsedInProdAmount"].Value), Conversion.ToDouble(r.Cells["balQty"].Value), Conversion.ToDouble(r.Cells["balWeight"].Value), Conversion.ToDouble(r.Cells["balAmount"].Value), Conversion.ToDouble(r.Cells["OHExpense"].Value), Conversion.ToDouble(r.Cells["OHGeneralExpense"].Value), Conversion.ToDouble(r.Cells["PMExpense"].Value), Conversion.ToDouble(r.Cells["PMGeneralExpense"].Value), Conversion.ToDouble(r.Cells["NetRate"].Value), Conversion.ToDouble(r.Cells["NetAmount"].Value), Conversion.ToDouble(r.Cells["UsedInProdNetAmount"].Value), Conversion.ToDouble(r.Cells["balNetAmount"].Value), Conversion.ToString((object)r.Cells["Remarks"].Text));
				}
				Hide();
			}
			else
			{
				MessageBox.Show("Please Select Row first");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadavailableTransactionsForIssuance_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadOnInvoice_Click_1(null, null);
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
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnJobOrderDefine = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1117, 30);
		this.panel1.TabIndex = 0;
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnnew, this.btnJobOrderDefine });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1117, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(69, 24);
		this.btnnew.Text = "&Reset";
		this.btnnew.Click += new System.EventHandler(btnReset_Click);
		this.btnJobOrderDefine.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnJobOrderDefine.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnJobOrderDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnJobOrderDefine.Name = "btnJobOrderDefine";
		this.btnJobOrderDefine.Size = new System.Drawing.Size(99, 24);
		this.btnJobOrderDefine.Text = "Load Data";
		this.btnJobOrderDefine.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1068, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
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
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1117, 614);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(260, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Output Data Against Job Order";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.label23);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 30);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1117, 30);
		this.panel2.TabIndex = 30;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 60);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1117, 614);
		this.panel3.TabIndex = 31;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1117, 674);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "LoadOutPutDataByJobOrder";
		this.Text = "Available Transactions For Issuance";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadInvoices_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadavailableTransactionsForIssuance_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
