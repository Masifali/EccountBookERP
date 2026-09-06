using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production;

public class LoadOverHeadsDataByJobOrder : Form
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

	public LoadOverHeadsDataByJobOrder(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = "ProductionAgainstPreCostingJobOrder",
				RightName = UserAccount.RoleName
			});
			if (lstRights.Count > 0)
			{
				List<tblUserRights> newList = lstRights.Where((tblUserRights x) => x.RightName == "Rate").ToList();
				if (newList.Any())
				{
					DoHaveCanRateandAmount = Conversion.ToBool((object)newList[0].Value.ToString());
				}
			}
			dtIssuance.Columns.Add("Id", typeof(int));
			dtIssuance.Columns.Add("InvProductionJobOrderId", typeof(int));
			dtIssuance.Columns.Add("CharOfAccountId", typeof(int));
			dtIssuance.Columns.Add("OverHeadAccount", typeof(string));
			dtIssuance.Columns.Add("Amount", typeof(double));
			dtIssuance.Columns.Add("ItemQty", typeof(double));
			dtIssuance.Columns.Add("ItemRate", typeof(double));
			dtIssuance.Columns.Add("BrandId", typeof(int));
			dtIssuance.Columns.Add("BrandName", typeof(string));
			dtIssuance.Columns.Add("BrandUomId", typeof(int));
			dtIssuance.Columns.Add("BrandUom", typeof(string));
			dtIssuance.Columns.Add("ohRemarks", typeof(string));
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
		//IL_003e: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = JobOrderId
			};
			DataTable dthis = new DataTable();
			dthis = InvProductionJobOrder.LoadOverHeadsDataByInvProductionJobOrder(val);
			if (dthis.Rows.Count > 0)
			{
				DataTable dtgrdinput = new DataTable();
				dtgrdinput.Columns.Add("Id", typeof(int));
				dtgrdinput.Columns.Add("InvProductionJobOrderId", typeof(int));
				dtgrdinput.Columns.Add("CharOfAccountId", typeof(int));
				dtgrdinput.Columns.Add("OverHeadAccount", typeof(string));
				dtgrdinput.Columns.Add("BrandId", typeof(int));
				dtgrdinput.Columns.Add("BrandName", typeof(string));
				dtgrdinput.Columns.Add("BrandUomId", typeof(int));
				dtgrdinput.Columns.Add("BrandUom", typeof(string));
				dtgrdinput.Columns.Add("ItemQty", typeof(double));
				dtgrdinput.Columns.Add("ItemRate", typeof(double));
				dtgrdinput.Columns.Add("Amount", typeof(double));
				dtgrdinput.Columns.Add("Remarks", typeof(string));
				dtgrdinput.Clear();
				for (int i = 0; i < dthis.Rows.Count; i++)
				{
					dtgrdinput.Rows.Add(Conversion.ToInt(dthis.Rows[i]["Id"]), Conversion.ToInt(dthis.Rows[i]["InvProductionJobOrderId"]), Conversion.ToInt(dthis.Rows[i]["CharOfAccountId"]), Conversion.ToString(dthis.Rows[i]["OverHeadAccount"]), Conversion.ToInt(dthis.Rows[i]["BrandId"]), Conversion.ToString(dthis.Rows[i]["BrandName"]), Conversion.ToInt(dthis.Rows[i]["BrandUomId"]), Conversion.ToString(dthis.Rows[i]["BrandUom"]), Conversion.ToDouble(dthis.Rows[i]["ItemQty"]), Conversion.ToDouble(dthis.Rows[i]["ItemRate"]), Conversion.ToDouble(dthis.Rows[i]["Amount"]), Conversion.ToString(dthis.Rows[i]["ohRemarks"]));
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
			if (!FoodProductionWithValues.ValuesShowRights)
			{
				grd.RootTable.Columns["Amount"].Visible = false;
				grd.RootTable.Columns["ItemRate"].Visible = false;
			}
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["InvProductionJobOrderId"].Visible = false;
			grd.RootTable.Columns["BrandUomId"].Visible = false;
			grd.RootTable.Columns["BrandId"].Visible = false;
			grd.RootTable.Columns["CharOfAccountId"].Visible = false;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,#.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Width = 40;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.FrozenColumns = 1;
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
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
					dtIssuance.Rows.Add(Conversion.ToInt(r.Cells["Id"].Value), Conversion.ToInt(r.Cells["InvProductionJobOrderId"].Value), Conversion.ToInt(r.Cells["CharOfAccountId"].Value), Conversion.ToString(r.Cells["OverHeadAccount"].Value), Conversion.ToDouble(r.Cells["Amount"].Value), Conversion.ToDouble(r.Cells["ItemQty"].Value), Conversion.ToDouble(r.Cells["ItemRate"].Value), Conversion.ToInt(r.Cells["BrandId"].Value), Conversion.ToString(r.Cells["BrandName"].Value), Conversion.ToInt(r.Cells["BrandUomId"].Value), Conversion.ToString(r.Cells["BrandUom"].Value), Conversion.ToString(r.Cells["Remarks"].Value));
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnJobOrderDefine = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1117, 30);
		this.panel1.TabIndex = 0;
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
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
