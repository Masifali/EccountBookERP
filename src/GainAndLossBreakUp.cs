using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp
{
using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;


using UserAccount = global::Architecture.Model.UserAccount;

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using global::Architecture.BLL.Export;
using global::Architecture.Model;
using Janus.Windows.GridEX;

public class GainAndLossBreakUp : Form
{
	private UserAccount UserAccount = new UserAccount();

	public int FcyReceiptId;

	private IContainer components;

	private Panel panel4;

	internal GridEX grd;

	private Label label23;

	private Panel panel6;

	private Panel panel3;

	private Panel panel1;

	public GainAndLossBreakUp(UserAccount user)
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
			BindGainAndLossbreakUp();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGainAndLossbreakUp()
	{
		try
		{
			DataTable lst = ExImFCBankReceipts.GetGainAndLossBreackup(FcyReceiptId);
			if (lst.Rows.Count > 0)
			{
				grd.DataSource = lst;
				grd.RetrieveStructure();
				grd.RootTable.Columns["Id"].Visible = false;
				grd.RootTable.Columns["FcyBankReceiptId"].Visible = false;
				grd.RootTable.Columns["Remarks"].Width = 700;
				grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
				grd.RootTable.Columns["LocalAmount"].AggregateFunction = (AggregateFunction)2;
				grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
				grd.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
				grd.RootTable.Columns["LocalAmount"].TextAlignment = (TextAlignment)3;
				grd.RootTable.Columns["FcyAmount"].FormatString = "#,#.###";
				grd.RootTable.Columns["ExchangeRate"].FormatString = "#,#.###";
				grd.RootTable.Columns["LocalAmount"].FormatString = "#,#.###";
				grd.RootTable.Columns["FcyAmount"].TotalFormatString = "#,#.###";
				grd.RootTable.Columns["ExchangeRate"].TotalFormatString = "#,#.###";
				grd.RootTable.Columns["LocalAmount"].TotalFormatString = "#,#.###";
				grd.AllowDelete = (InheritableBoolean)2;
				grd.AllowEdit = (InheritableBoolean)2;
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 46);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(761, 404);
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(761, 404);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 8);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(210, 24);
		this.label23.TabIndex = 25;
		this.label23.Text = "Gain And Loss BreakUp";
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(761, 48);
		this.panel6.TabIndex = 6;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(761, 46);
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
		base.KeyPreview = true;
		base.Name = "GainAndLossBreakUp";
		this.Text = "GainAndLossBreakUp";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(GainAndLossBreakUp_Load);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
