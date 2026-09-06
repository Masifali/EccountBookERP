using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.DashBoard;

public class ExportDashboardPopUp : Form
{
	private UserAccount UserAccount = new UserAccount();

	public string Activity;

	public DateTime FromDate;

	public DateTime ToDate;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	internal GridEX Grid;

	private ToolStripLabel lblTitle;

	public ExportDashboardPopUp(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void ExportDashboardPopUp_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		try
		{
			DataTable dt = Dashboard.ExportDashboard(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = FromDate,
				ToDate = ToDate,
				Activity = Activity
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			if (Activity == "ForwardedButBillofLadingNotReceivedDetailReport")
			{
				lblTitle.Text = "Forwarded But Bill of Lading Not Received";
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Customer");
				dtHistory.Columns.Add("InvoiceNo");
				dtHistory.Columns.Add("ContractNo");
				dtHistory.Columns.Add("M_Tons", typeof(double));
				dtHistory.Columns.Add("Fcy");
				dtHistory.Columns.Add("FcyRate", typeof(double));
				dtHistory.Columns.Add("FcyAmount", typeof(double));
				dtHistory.Columns.Add("PKRAmount", typeof(double));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dt.Rows[i]["Customer"], dt.Rows[i]["InvoiceNo"], dt.Rows[i]["LcOrderNo"], dt.Rows[i]["Forwarded_MTons"], dt.Rows[i]["FcyCode"], dt.Rows[i]["Fcy_Rate"], dt.Rows[i]["Fcy_Forwarded"], dt.Rows[i]["PKR_AmountBalance"]);
				}
				Grid.DataSource = dtHistory;
				Grid.RetrieveStructure();
				GridSettingForForwardedButBillofLadingNotReceived();
			}
			else if (Activity == "ForwardingInProcessDetailReport")
			{
				lblTitle.Text = "Forwarding In-Process";
				DataTable dtHistory2 = new DataTable();
				dtHistory2.Columns.Add("Customer");
				dtHistory2.Columns.Add("InvoiceNo");
				dtHistory2.Columns.Add("ContractNo");
				dtHistory2.Columns.Add("M_Tons", typeof(double));
				dtHistory2.Columns.Add("Fcy");
				dtHistory2.Columns.Add("FcyRate", typeof(double));
				dtHistory2.Columns.Add("FcyAmount", typeof(double));
				dtHistory2.Columns.Add("PKRAmount", typeof(double));
				for (int j = 0; j < dt.Rows.Count; j++)
				{
					dtHistory2.Rows.Add(dt.Rows[j]["Customer"], dt.Rows[j]["InvoiceNo"], dt.Rows[j]["LcOrderNo"], dt.Rows[j]["Forwarded_MTons"], dt.Rows[j]["FcyCode"], dt.Rows[j]["Fcy_Rate"], dt.Rows[j]["Fcy_Forwarded"], dt.Rows[j]["PKR_AmountBalance"]);
				}
				Grid.DataSource = dtHistory2;
				Grid.RetrieveStructure();
				GridSettingForForwardedButBillofLadingNotReceived();
			}
			else if (Activity == "BlReceivedButPaymentNotReceived")
			{
				lblTitle.Text = "Bl Received But Payment Not Received";
				DataTable dtHistory3 = new DataTable();
				dtHistory3.Columns.Add("Customer");
				dtHistory3.Columns.Add("InvoiceNo");
				dtHistory3.Columns.Add("ContractNo");
				dtHistory3.Columns.Add("Fcy");
				dtHistory3.Columns.Add("FcyRate", typeof(double));
				dtHistory3.Columns.Add("FcyAmount", typeof(double));
				dtHistory3.Columns.Add("PKRAmount", typeof(double));
				for (int k = 0; k < dt.Rows.Count; k++)
				{
					dtHistory3.Rows.Add(dt.Rows[k]["Customer"], dt.Rows[k]["InvoiceNo"], dt.Rows[k]["LcOrderNo"], dt.Rows[k]["FcyCode"], dt.Rows[k]["Fcy_Rate"], dt.Rows[k]["Fcy_Balance"], dt.Rows[k]["PKR_AmountBalance"]);
				}
				Grid.DataSource = dtHistory3;
				Grid.RetrieveStructure();
				GridSettingForBlReceivedButPaymentNotReceived();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForForwardedButBillofLadingNotReceived()
	{
		try
		{
			Grid.RootTable.Columns["M_Tons"].AggregateFunction = (AggregateFunction)2;
			Grid.RootTable.Columns["M_Tons"].TextAlignment = (TextAlignment)3;
			Grid.RootTable.Columns["M_Tons"].FormatString = "#,#.##";
			Grid.RootTable.Columns["M_Tons"].TotalFormatString = "#,#.##";
			Grid.RootTable.Columns["FcyRate"].TextAlignment = (TextAlignment)3;
			Grid.RootTable.Columns["FcyRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			Grid.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			Grid.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			Grid.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			Grid.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			Grid.RootTable.Columns["PKRAmount"].AggregateFunction = (AggregateFunction)2;
			Grid.RootTable.Columns["PKRAmount"].TextAlignment = (TextAlignment)3;
			Grid.RootTable.Columns["PKRAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			Grid.RootTable.Columns["PKRAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(Grid);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForBlReceivedButPaymentNotReceived()
	{
		try
		{
			Grid.RootTable.Columns["FcyRate"].TextAlignment = (TextAlignment)3;
			Grid.RootTable.Columns["FcyRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			Grid.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			Grid.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			Grid.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			Grid.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			Grid.RootTable.Columns["PKRAmount"].AggregateFunction = (AggregateFunction)2;
			Grid.RootTable.Columns["PKRAmount"].TextAlignment = (TextAlignment)3;
			Grid.RootTable.Columns["PKRAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			Grid.RootTable.Columns["PKRAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(Grid);
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
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		GridEXLayout Grid_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.Grid = new GridEX();
		this.lblTitle = new System.Windows.Forms.ToolStripLabel();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Grid).BeginInit();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.Grid);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1127, 655);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1127, 36);
		this.panel2.TabIndex = 5;
		this.toolStrip3.BackColor = System.Drawing.Color.Teal;
		this.toolStrip3.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.lblTitle });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1127, 28);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.Grid.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.Grid).BackColor = System.Drawing.Color.White;
		Grid_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.Grid.DesignTimeLayout = Grid_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.Grid).Dock = System.Windows.Forms.DockStyle.Fill;
		this.Grid.FilterMode = (FilterMode)2;
		this.Grid.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.Grid).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.Grid.GroupTotals = (GroupTotals)2;
		this.Grid.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.Grid).Location = new System.Drawing.Point(0, 36);
		((System.Windows.Forms.Control)(object)this.Grid).Name = "Grid";
		((ScrollableBase)this.Grid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.Grid).Size = new System.Drawing.Size(1127, 619);
		((System.Windows.Forms.Control)(object)this.Grid).TabIndex = 6;
		this.Grid.TotalRow = (InheritableBoolean)1;
		this.Grid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.Grid).VisualStyle = (VisualStyle)4;
		this.lblTitle.BackColor = System.Drawing.Color.Teal;
		this.lblTitle.ForeColor = System.Drawing.Color.White;
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(48, 25);
		this.lblTitle.Text = "Title";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1127, 655);
		base.Controls.Add(this.panel1);
		base.Name = "ExportDashboardPopUp";
		this.Text = "Export Dashboard PopUp";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ExportDashboardPopUp_Load);
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.Grid).EndInit();
		base.ResumeLayout(false);
	}
}
