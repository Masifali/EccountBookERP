// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
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
namespace Architecture.WinApp.Common
{

public class DynamicReports : Form
{
	public DataTable parameters = new DataTable();

	private DataTable dtFiles = new DataTable();

	public string FolderName = string.Empty;

	public string reportPath = string.Empty;

	private IContainer components;

	internal GridEX grd;

	public DynamicReports()
	{
		InitializeComponent();
	}

	private void grd_RowDoubleClick(object sender, RowActionEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Reporting rpt = new Reporting();
			for (int i = 0; i < parameters.Rows.Count; i++)
			{
				rpt.RptPerameter(parameters.Rows[i][0].ToString(), parameters.Rows[i][1].ToString());
			}
			rpt.ShowReport(FolderName + "\\" + grd.CurrentRow.Cells[1].Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DynamicReports_Load(object sender, EventArgs e)
	{
		try
		{
			dtFiles.Columns.Add("Path");
			dtFiles.Columns.Add("Report");
			dtFiles.Columns.Add("Report Name");
			string ConnStringReportPath = ConfigurationManager.ConnectionStrings["TPSSReportDb"].ConnectionString.ToString();
			if (ConnStringReportPath != string.Empty)
			{
				reportPath = ConnStringReportPath + "\\";
				string[] files = Directory.GetFiles(reportPath + FolderName, "*rpt");
				for (int i = 0; i < files.Length; i++)
				{
					dtFiles.Rows.Add(files[i], Path.GetFileName(files[i]), Path.GetFileNameWithoutExtension(files[i]));
				}
				grd.DataSource = dtFiles;
				grd.RetrieveStructure();
				grd.RootTable.Columns["Path"].Visible = false;
				grd.RootTable.Columns["Report"].Visible = false;
				grd.RootTable.Columns["Report Name"].EditType = (EditType)0;
				grd.RootTable.Columns["Report Name"].FilterEditType = (FilterEditType)1;
				return;
			}
			throw new Exception("Report Path Not Found");
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
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.grd = new GridEX();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		base.SuspendLayout();
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		this.grd.FilterRowUpdateMode = (FilterRowUpdateMode)2;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(611, 379);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.RowDoubleClick += new RowActionEventHandler(grd_RowDoubleClick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(611, 379);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "DynamicReports";
		this.Text = "DynamicReports";
		base.Load += new System.EventHandler(DynamicReports_Load);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		base.ResumeLayout(false);
	}
}

}
