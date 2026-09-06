using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Reports.Export;
using Architecture.Model;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.ExportReports
{

public class FIBalanceDetailByFI : Form
{
	private UserAccount UserAccount = new UserAccount();

	public int GdId;

	public int DocumentTypeId;

	private IContainer components;

	private Panel panel4;

	internal GridEX grd;

	private Label label23;

	private Panel panel3;

	private Panel panel1;

	public FIBalanceDetailByFI(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void GainAndLossBreakUp_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			BindGdbreakUp();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGdbreakUp()
	{
		try
		{
			DataTable lst = ExportReports.GetFIBalanceDetailByFI(UserAccount.OrganizationId, UserAccount.CompanyId, GdId);
			if (lst.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("RefDocumentTypeId", typeof(int));
				dtcol.Columns.Add("GDId", typeof(int));
				dtcol.Columns.Add("EximInvoiceId", typeof(int));
				dtcol.Columns.Add("InvoiceNo", typeof(string));
				dtcol.Columns.Add("FIId", typeof(int));
				dtcol.Columns.Add("FINo", typeof(string));
				dtcol.Columns.Add("GdNo", typeof(string));
				dtcol.Columns.Add("UtilizeAmount", typeof(decimal));
				dtcol.Rows.Clear();
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtcol.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["RefDocumentTypeId"], lst.Rows[i]["GDId"], lst.Rows[i]["EximInvoiceId"], lst.Rows[i]["InvoiceNo"], lst.Rows[i]["FIId"], lst.Rows[i]["FINo"], lst.Rows[i]["GdNo"], lst.Rows[i]["UtilizeAmount"]);
				}
				grd.DataSource = dtcol;
				grd.RetrieveStructure();
				grd.RootTable.Columns["Id"].Visible = false;
				grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
				grd.RootTable.Columns["EximInvoiceId"].Visible = false;
				grd.RootTable.Columns["FIId"].Visible = false;
				grd.RootTable.Columns["GDId"].Visible = false;
				grd.RootTable.Columns["UtilizeAmount"].AggregateFunction = (AggregateFunction)2;
				grd.RootTable.Columns["UtilizeAmount"].TextAlignment = (TextAlignment)3;
				grd.RootTable.Columns["UtilizeAmount"].FormatString = "#,#.###";
				grd.RootTable.Columns["UtilizeAmount"].TotalFormatString = "#,#.###";
				grd.AllowDelete = (InheritableBoolean)2;
				grd.AllowEdit = (InheritableBoolean)2;
				CommonServices.GridAutoAdjustmentNew(grd);
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.FIBalanceDetailByFI));
		this.panel4 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel3.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 24);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(761, 426);
		this.panel4.TabIndex = 3;
		this.grd.AllowDelete = (InheritableBoolean)1;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(761, 426);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(148, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "FI Balance Detail";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(761, 24);
		this.panel3.TabIndex = 2;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(761, 450);
		this.panel1.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(761, 450);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "FIBalanceDetailByFI";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "EccountBook ERP";
		base.Load += new System.EventHandler(GainAndLossBreakUp_Load);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
