using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Account_Reports
{

public class PostDatedCheqInformation : Form
{
	private UserAccount UserAccount = new UserAccount();

	public int AccountId;

	public DateTime ToDate = DateTime.Now;

	private IContainer components;

	private ToolStrip toolStrip6;

	private ToolStripButton BtnNewBanksummLegder;

	private Panel panel1;

	private Label label39;

	internal GridEX grd;

	public PostDatedCheqInformation(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void PostDatedCheqInformation_Load(object sender, EventArgs e)
	{
		try
		{
			GetData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetData()
	{
		try
		{
			DataTable dt = VoucherHead.getPostDatedCheqByAccountId(UserAccount.OrganizationId, UserAccount.CompanyId, AccountId, ToDate);
			if (dt.Rows.Count > 0)
			{
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("AccountId", typeof(int));
				dt2.Columns.Add("AccountCode", typeof(string));
				dt2.Columns.Add("AccountTitle", typeof(string));
				dt2.Columns.Add("ChequeNo", typeof(string));
				dt2.Columns.Add("CheqDate", typeof(DateTime));
				dt2.Columns.Add("CheqAmount", typeof(double));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dt2.Rows.Add(dt.Rows[i]["AccountId"], dt.Rows[i]["AccountCode"], dt.Rows[i]["AccountTitle"], dt.Rows[i]["ChequeNo"], dt.Rows[i]["CheqDate"], dt.Rows[i]["CheqAmount"]);
				}
				grd.DataSource = dt2;
				grd.RetrieveStructure();
				grd.RootTable.Columns["AccountId"].Visible = false;
				grd.RootTable.Columns["CheqAmount"].AggregateFunction = (AggregateFunction)2;
				grd.RootTable.Columns["CheqAmount"].TextAlignment = (TextAlignment)3;
				grd.RootTable.Columns["CheqAmount"].FormatString = clsGlobalVariables.stringFormatboth;
				grd.RootTable.Columns["CheqAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
				CommonServices.GridAutoAdjustmentNew(grd);
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
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.toolStrip6 = new System.Windows.Forms.ToolStrip();
		this.BtnNewBanksummLegder = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label39 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.toolStrip6.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		base.SuspendLayout();
		this.toolStrip6.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip6.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnNewBanksummLegder });
		this.toolStrip6.Location = new System.Drawing.Point(0, 0);
		this.toolStrip6.Name = "toolStrip6";
		this.toolStrip6.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip6.Size = new System.Drawing.Size(701, 27);
		this.toolStrip6.TabIndex = 24;
		this.toolStrip6.Text = "toolStrip6";
		this.BtnNewBanksummLegder.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewBanksummLegder.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewBanksummLegder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewBanksummLegder.Name = "BtnNewBanksummLegder";
		this.BtnNewBanksummLegder.Size = new System.Drawing.Size(68, 24);
		this.BtnNewBanksummLegder.Text = "&Reset";
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label39);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(701, 26);
		this.panel1.TabIndex = 25;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.White;
		this.label39.Location = new System.Drawing.Point(4, 5);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(218, 16);
		this.label39.TabIndex = 104;
		this.label39.Text = "Post Dated Cheques Information";
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 53);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(701, 342);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 45;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(701, 395);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.toolStrip6);
		base.Name = "PostDatedCheqInformation";
		this.Text = "PostDatedCheqBreakUp";
		base.Load += new System.EventHandler(PostDatedCheqInformation_Load);
		this.toolStrip6.ResumeLayout(false);
		this.toolStrip6.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
