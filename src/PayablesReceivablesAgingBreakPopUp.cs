using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
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
namespace Architecture.WinApp.Account_Reports
{

public class PayablesReceivablesAgingBreakPopUp : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable data = new DataTable();

	private DataTable dtGrid = new DataTable();

	private bool FormInitialized;

	public bool ReportType_OverDue;

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnPrint;

	private Panel panel4;

	internal GridEX GridAvgRateByItem;

	public Label lblFor;

	private CtrlGrdBar ctrlGrdBar1;

	public PayablesReceivablesAgingBreakPopUp(UserAccount pUser, DataTable dt)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		data = dt;
		UserAccount = pUser;
	}

	private void AvgRateByItemPopUp_Load(object sender, EventArgs e)
	{
		try
		{
			FormInitialized = true;
			if (data.Rows.Count > 0)
			{
				if (dtGrid.Columns.Count == 0)
				{
					dtGrid.Columns.Add("DocumentType", typeof(string));
					dtGrid.Columns.Add("VoucherCode", typeof(int));
					dtGrid.Columns.Add("VoucherDate", typeof(DateTime));
					dtGrid.Columns.Add("DueDays", typeof(int));
					dtGrid.Columns.Add("DueDate", typeof(DateTime));
					if (ReportType_OverDue)
					{
						dtGrid.Columns.Add("DaysOverdue", typeof(int));
					}
					dtGrid.Columns.Add("AccountId", typeof(int));
					dtGrid.Columns.Add("AccountTitle", typeof(string));
					dtGrid.Columns.Add("Amount", typeof(double));
				}
				foreach (DataRow row in data.Rows)
				{
					DataRow newRow = dtGrid.NewRow();
					newRow["DocumentType"] = row["DocumentTypeDescription"];
					newRow["VoucherCode"] = row["VoucherCode"];
					newRow["VoucherDate"] = row["VoucherDate"];
					newRow["DueDays"] = row["DueDays"];
					newRow["DueDate"] = row["DueDate"];
					if (ReportType_OverDue)
					{
						newRow["DaysOverdue"] = row["OverDueBy"];
					}
					newRow["AccountId"] = row["AccountId"];
					newRow["AccountTitle"] = row["AccountTitle"];
					newRow["Amount"] = row["Amount"];
					dtGrid.Rows.Add(newRow);
				}
				GridAvgRateByItem.DataSource = dtGrid;
				GridAvgRateByItem.RetrieveStructure();
				GridAvgRateByItemSettings();
			}
			else
			{
				GridAvgRateByItem.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridAvgRateByItemSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "AccountId" };
			GridEX_Helper.GridWrappingAndColumnSettings(GridAvgRateByItem, 2, 3, hiddenColumns);
			GridAvgRateByItem.RootTable.Groups.Add("AccountTitle");
			GridAvgRateByItem.GroupTotals = (GroupTotals)2;
			GridAvgRateByItem.HideColumnsWhenGrouped = (InheritableBoolean)1;
			GridAvgRateByItem.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentType;
			GridAvgRateByItem.RootTable.Columns["VoucherCode"].Width = Constants.InventoryConstants.VoucherNo;
			GridAvgRateByItem.RootTable.Columns["VoucherDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			GridAvgRateByItem.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			GridAvgRateByItem.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			GridAvgRateByItem.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant + 20;
			GridAvgRateByItem.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 30;
			GridAvgRateByItem.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 30;
			((Control)(object)GridAvgRateByItem).Refresh();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridAvgRateByItem_LinkClicked(object sender, ColumnActionEventArgs e)
	{
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, GridAvgRateByItem, dtGrid);
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
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		GridEXLayout GridAvgRateByItem_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.PayablesReceivablesAgingBreakPopUp));
		this.panel1 = new System.Windows.Forms.Panel();
		this.GridAvgRateByItem = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.lblFor = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAvgRateByItem).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.GridAvgRateByItem);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(800, 450);
		this.panel1.TabIndex = 0;
		this.GridAvgRateByItem.AllowEdit = (InheritableBoolean)2;
		this.GridAvgRateByItem.AlternatingColors = true;
		this.GridAvgRateByItem.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridAvgRateByItem_DesignTimeLayout.LayoutString = resources.GetString("GridAvgRateByItem_DesignTimeLayout.LayoutString");
		this.GridAvgRateByItem.DesignTimeLayout = GridAvgRateByItem_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridAvgRateByItem).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAvgRateByItem.DynamicFiltering = true;
		this.GridAvgRateByItem.FilterMode = (FilterMode)2;
		this.GridAvgRateByItem.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAvgRateByItem).Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GridAvgRateByItem.GroupTotals = (GroupTotals)2;
		this.GridAvgRateByItem.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GridAvgRateByItem).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.GridAvgRateByItem).Name = "GridAvgRateByItem";
		((ScrollableBase)this.GridAvgRateByItem).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAvgRateByItem).Size = new System.Drawing.Size(800, 422);
		((System.Windows.Forms.Control)(object)this.GridAvgRateByItem).TabIndex = 54;
		this.GridAvgRateByItem.TotalRow = (InheritableBoolean)1;
		this.GridAvgRateByItem.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAvgRateByItem).VisualStyle = (VisualStyle)4;
		this.GridAvgRateByItem.LinkClicked += new ColumnActionEventHandler(GridAvgRateByItem_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.lblFor);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(800, 28);
		this.panel4.TabIndex = 53;
		this.lblFor.AutoSize = true;
		this.lblFor.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFor.ForeColor = System.Drawing.Color.White;
		this.lblFor.Location = new System.Drawing.Point(4, 3);
		this.lblFor.Name = "lblFor";
		this.lblFor.Size = new System.Drawing.Size(40, 19);
		this.lblFor.TabIndex = 5;
		this.lblFor.Text = "Pop";
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(800, 25);
		this.toolStrip1.TabIndex = 52;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStrip1.Visible = false;
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(174, 22);
		this.btnPrint.Text = "221-&Avg Rate By Item";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(760, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.GridAvgRateByItem;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 23);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 450);
		base.Controls.Add(this.panel1);
		base.Name = "PayablesReceivablesAgingBreakPopUp";
		this.Text = "Pup Up";
		base.Load += new System.EventHandler(AvgRateByItemPopUp_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAvgRateByItem).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
