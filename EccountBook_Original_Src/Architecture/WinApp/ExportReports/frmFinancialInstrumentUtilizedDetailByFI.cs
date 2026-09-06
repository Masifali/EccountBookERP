using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Reports.Export;
using Architecture.Model;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.ExportReports;

public class frmFinancialInstrumentUtilizedDetailByFI : Form
{
	private UserAccount UserAccount = new UserAccount();

	public int FIId;

	public int DocumentTypeId;

	private IContainer components;

	private Panel panel4;

	internal GridEX grd;

	private Label label23;

	private Panel panel3;

	private Panel panel1;

	public frmFinancialInstrumentUtilizedDetailByFI(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmFinancialInstrumentUtilizedDetailByFI_Load(object sender, EventArgs e)
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
			DataTable lst = ExportReports.getFIUtilizeInfoByFIId(FIId, DocumentTypeId);
			if (lst.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("FIId", typeof(int));
				dtcol.Columns.Add("FINo", typeof(string));
				dtcol.Columns.Add("FIDate", typeof(DateTime));
				dtcol.Columns.Add("EximInvoiceId", typeof(int));
				dtcol.Columns.Add("InvoiceNo", typeof(string));
				dtcol.Columns.Add("InvoiceDate", typeof(DateTime));
				dtcol.Columns.Add("GDId", typeof(int));
				dtcol.Columns.Add("RefDocumentTypeId", typeof(int));
				dtcol.Columns.Add("GdNo", typeof(string));
				dtcol.Columns.Add("SupplierCustomerId", typeof(int));
				dtcol.Columns.Add("CustomerName", typeof(string));
				dtcol.Columns.Add("ConsigneeId", typeof(int));
				dtcol.Columns.Add("Consignee", typeof(string));
				dtcol.Columns.Add("BankId", typeof(int));
				dtcol.Columns.Add("BankAccount", typeof(string));
				dtcol.Columns.Add("UtilizeAmount", typeof(decimal));
				dtcol.Columns.Add("FcyCode", typeof(string));
				dtcol.Columns.Add("ExchangeRate", typeof(double));
				dtcol.Columns.Add("LcyAmount", typeof(double));
				dtcol.Columns.Add("InvoiceMTon", typeof(double));
				dtcol.Columns.Add("InvoiceAmount", typeof(double));
				dtcol.Rows.Clear();
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtcol.Rows.Add(lst.Rows[i]["FIId"], lst.Rows[i]["FINo"], lst.Rows[i]["FIDate"], lst.Rows[i]["EximInvoiceId"], lst.Rows[i]["InvoiceNo"], lst.Rows[i]["InvoiceDate"], lst.Rows[i]["GDId"], lst.Rows[i]["RefDocumentTypeId"], lst.Rows[i]["GdNo"], lst.Rows[i]["SupplierCustomerId"], lst.Rows[i]["CustomerName"], lst.Rows[i]["ConsigneeId"], lst.Rows[i]["Consignee"], lst.Rows[i]["BankId"], lst.Rows[i]["BankAccount"], lst.Rows[i]["UtilizeAmount"], lst.Rows[i]["FcyCode"], lst.Rows[i]["ExchangeRate"], lst.Rows[i]["LcyAmount"], lst.Rows[i]["InvoiceMTon"], lst.Rows[i]["InvoiceAmount"]);
				}
				grd.DataSource = dtcol;
				grd.RetrieveStructure();
				List<string> hiddenColumns = new List<string> { "FIId", "EximInvoiceId", "GDId", "RefDocumentTypeId", "SupplierCustomerId", "ConsigneeId", "BankId" };
				GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 2, hiddenColumns);
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.frmFinancialInstrumentUtilizedDetailByFI));
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
		this.panel4.Size = new System.Drawing.Size(1261, 426);
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1261, 426);
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
		this.label23.Size = new System.Drawing.Size(289, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Financial Instrument Utilize Detail";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1261, 24);
		this.panel3.TabIndex = 2;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1261, 450);
		this.panel1.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1261, 450);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmFinancialInstrumentUtilizedDetailByFI";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "EccountBook ERP";
		base.Load += new System.EventHandler(frmFinancialInstrumentUtilizedDetailByFI_Load);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
