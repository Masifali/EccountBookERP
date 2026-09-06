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

namespace Architecture.WinApp.Steel.Production;

public class LoadInputDataByJobOrder : Form
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

	public LoadInputDataByJobOrder(UserAccount user)
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
			dtIssuance.Columns.Add("WarehouseId", typeof(int));
			dtIssuance.Columns.Add("WareHouseName", typeof(string));
			dtIssuance.Columns.Add("JobLotId", typeof(int));
			dtIssuance.Columns.Add("JobLot", typeof(string));
			dtIssuance.Columns.Add("ItemId", typeof(int));
			dtIssuance.Columns.Add("ItemName", typeof(string));
			dtIssuance.Columns.Add("PackingTypeId", typeof(int));
			dtIssuance.Columns.Add("PackingType", typeof(string));
			dtIssuance.Columns.Add("ItemQty", typeof(double));
			dtIssuance.Columns.Add("UomSchIdQty", typeof(int));
			dtIssuance.Columns.Add("PackUom", typeof(string));
			dtIssuance.Columns.Add("PackEquivalent", typeof(double));
			dtIssuance.Columns.Add("NetWeight", typeof(double));
			dtIssuance.Columns.Add("RateUomId", typeof(int));
			dtIssuance.Columns.Add("RateUom", typeof(string));
			dtIssuance.Columns.Add("RateEquivalent", typeof(double));
			dtIssuance.Columns.Add("ItemRate", typeof(double));
			dtIssuance.Columns.Add("ItemAmount", typeof(double));
			dtIssuance.Columns.Add("BalQty", typeof(double));
			dtIssuance.Columns.Add("BalWeight", typeof(double));
			dtIssuance.Columns.Add("BalAmount", typeof(double));
			dtIssuance.Columns.Add("Remarks", typeof(string));
			dtIssuance.Columns.Add("RefDocumentTypeId", typeof(int));
			dtIssuance.Columns.Add("RefDocumentType", typeof(string));
			dtIssuance.Columns.Add("RefDocIdNo", typeof(int));
			dtIssuance.Columns.Add("RefDocSubIdNo", typeof(int));
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
			DataTable dthis = InvProductionJobOrder.LoadInputDataByInvProductionJobOrder(new ReportsParameters
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
				dtgrdinput.Columns.Add("WarehouseId", typeof(int));
				dtgrdinput.Columns.Add("WareHouseName", typeof(string));
				dtgrdinput.Columns.Add("JobLotId", typeof(int));
				dtgrdinput.Columns.Add("JobLot", typeof(string));
				dtgrdinput.Columns.Add("ItemId", typeof(int));
				dtgrdinput.Columns.Add("ItemName", typeof(string));
				dtgrdinput.Columns.Add("PackingTypeId", typeof(int));
				dtgrdinput.Columns.Add("PackingType", typeof(string));
				dtgrdinput.Columns.Add("Qty", typeof(double));
				dtgrdinput.Columns.Add("PackUomId", typeof(int));
				dtgrdinput.Columns.Add("PackUom", typeof(string));
				dtgrdinput.Columns.Add("PackEquivalent", typeof(double));
				dtgrdinput.Columns.Add("NetWeight", typeof(double));
				dtgrdinput.Columns.Add("RateUomId", typeof(int));
				dtgrdinput.Columns.Add("RateUom", typeof(string));
				dtgrdinput.Columns.Add("RateEquivalent", typeof(double));
				dtgrdinput.Columns.Add("ItemRate", typeof(double));
				dtgrdinput.Columns.Add("ItemAmount", typeof(double));
				dtgrdinput.Columns.Add("UsedInProdQty", typeof(double));
				dtgrdinput.Columns.Add("UsedInProdWeight", typeof(double));
				dtgrdinput.Columns.Add("UsedInProdAmount", typeof(double));
				dtgrdinput.Columns.Add("balQty", typeof(double));
				dtgrdinput.Columns.Add("balWeight", typeof(double));
				dtgrdinput.Columns.Add("balAmount", typeof(double));
				dtgrdinput.Columns.Add("Remarks", typeof(string));
				dtgrdinput.Columns.Add("RefDocumentTypeId", typeof(int));
				dtgrdinput.Columns.Add("RefDocumentType", typeof(string));
				dtgrdinput.Columns.Add("RefDocIdNo", typeof(int));
				dtgrdinput.Columns.Add("RefDocSubIdNo", typeof(int));
				dtgrdinput.Clear();
				for (int i = 0; i < dthis.Rows.Count; i++)
				{
					dtgrdinput.Rows.Add(Conversion.ToInt(dthis.Rows[i]["Id"]), Conversion.ToInt(dthis.Rows[i]["InvProductionJobOrderId"]), Conversion.ToInt(dthis.Rows[i]["WarehouseId"]), Conversion.ToString(dthis.Rows[i]["WareHouseName"]), Conversion.ToInt(dthis.Rows[i]["JobLotId"]), Conversion.ToString(dthis.Rows[i]["JobLotDescription"]), Conversion.ToInt(dthis.Rows[i]["ItemId"]), Conversion.ToString(dthis.Rows[i]["ItemName"]), Conversion.ToInt(dthis.Rows[i]["PackingTypeId"]), Conversion.ToString(dthis.Rows[i]["PackTypeDesc"]), Conversion.ToDouble(dthis.Rows[i]["Qty"]), Conversion.ToInt(dthis.Rows[i]["PackUomId"]), Conversion.ToString(dthis.Rows[i]["PackUomCode"]), Conversion.ToDouble(dthis.Rows[i]["PackEquivalent"]), Conversion.ToDouble(dthis.Rows[i]["NetWeight"]), Conversion.ToInt(dthis.Rows[i]["RateUomId"]), Conversion.ToString(dthis.Rows[i]["RateUomCode"]), Conversion.ToDouble(dthis.Rows[i]["RateEquivalent"]), Conversion.ToDouble(dthis.Rows[i]["ItemRate"]), Conversion.ToDouble(dthis.Rows[i]["ItemAmount"]), Conversion.ToDouble(dthis.Rows[i]["UsedInProdQty"]), Conversion.ToDouble(dthis.Rows[i]["UsedInProdWeight"]), Conversion.ToDouble(dthis.Rows[i]["UsedInProdAmount"]), Conversion.ToDouble(dthis.Rows[i]["balQty"]), Conversion.ToDouble(dthis.Rows[i]["balWeight"]), Conversion.ToDouble(dthis.Rows[i]["balAmount"]), Conversion.ToString(dthis.Rows[i]["RemarksDetail"]), Conversion.ToInt(dthis.Rows[i]["RefDocumentTypeId"]), Conversion.ToString(dthis.Rows[i]["RefDocumentType"]), Conversion.ToInt(dthis.Rows[i]["RefDocIdNo"]), Conversion.ToInt(dthis.Rows[i]["RefDocSubIdNo"]));
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
			}
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["JobOrderId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["RefDocIdNo"].Visible = false;
			grd.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grd.RootTable.Columns["RateEquivalent"].Visible = false;
			grd.RootTable.Columns["PackEquivalent"].Visible = false;
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["UsedInProdQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["UsedInProdQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["UsedInProdQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["UsedInProdQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["balQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["balQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["balQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["balQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["PackEquivalent"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["PackEquivalent"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PackEquivalent"].FormatString = "#,##0.##";
			grd.RootTable.Columns["PackEquivalent"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["balWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["balWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["balWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["balWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["UsedInProdWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["UsedInProdWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["UsedInProdWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["UsedInProdWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["RateEquivalent"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RateEquivalent"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateEquivalent"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["RateEquivalent"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["UsedInProdAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["UsedInProdAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["UsedInProdAmount"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["UsedInProdAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["balAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["balAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["balAmount"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["balAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grd);
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
					dtIssuance.Rows.Add(Conversion.ToInt(r.Cells["Id"].Value), Conversion.ToInt(r.Cells["JobOrderId"].Value), Conversion.ToInt(r.Cells["WarehouseId"].Value), Conversion.ToString((object)r.Cells["WareHouseName"].Text), Conversion.ToInt(r.Cells["JobLotId"].Value), Conversion.ToString((object)r.Cells["JobLot"].Text), Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString((object)r.Cells["ItemName"].Text), Conversion.ToInt(r.Cells["PackingTypeId"].Value), Conversion.ToString((object)r.Cells["PackingType"].Text), Conversion.ToDouble(r.Cells["Qty"].Value), Conversion.ToInt(r.Cells["PackUomId"].Value), Conversion.ToString(r.Cells["PackUom"].Value), Conversion.ToDouble(r.Cells["PackEquivalent"].Value), Conversion.ToDouble(r.Cells["NetWeight"].Value), Conversion.ToInt(r.Cells["RateUomId"].Value), Conversion.ToString((object)r.Cells["RateUom"].Text), Conversion.ToDouble(r.Cells["RateEquivalent"].Value), Conversion.ToDouble(r.Cells["ItemRate"].Value), Conversion.ToDouble(r.Cells["ItemAmount"].Value), Conversion.ToDouble(r.Cells["balQty"].Value), Conversion.ToDouble(r.Cells["balWeight"].Value), Conversion.ToDouble(r.Cells["balAmount"].Value), Conversion.ToString((object)r.Cells["Remarks"].Text), Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value), Conversion.ToString((object)r.Cells["RefDocumentType"].Text), Conversion.ToInt(r.Cells["RefDocIdNo"].Value), Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value));
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
		this.label23.Size = new System.Drawing.Size(248, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Input Data Against Job Order";
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
		base.Name = "LoadInputDataByJobOrder";
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
