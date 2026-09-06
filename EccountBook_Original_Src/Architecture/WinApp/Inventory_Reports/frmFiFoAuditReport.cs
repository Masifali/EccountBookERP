using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Stocks_Report;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Inventory_Reports;

public class frmFiFoAuditReport : Form
{
	private DataTable dt = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private Panel panel2;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnRefresh;

	private ToolStripButton BtnPrintRegister;

	private Panel panel6;

	private Panel panel4;

	private Label label5;

	public frmFiFoAuditReport(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmGatePassReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 500);
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		try
		{
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dt.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dt = InventoryStockEvalautionDetail.GetFIFODataForAudit(obj);
			if (dt.Rows.Count > 0 && dt != null)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RefDocumentTypeId", typeof(int));
				dtcol.Columns.Add("RefDocIdNo", typeof(int));
				dtcol.Columns.Add("RefDocSubIdNo", typeof(int));
				dtcol.Columns.Add("DocumentTypeCode", typeof(string));
				dtcol.Columns.Add("DocCodeNo", typeof(string));
				dtcol.Columns.Add("ReferedDocNos", typeof(string));
				dtcol.Columns.Add("InQty", typeof(decimal));
				dtcol.Columns.Add("OutQty", typeof(decimal));
				dtcol.Columns.Add("BalQty", typeof(decimal));
				dtcol.Columns.Add("InWeight", typeof(decimal));
				dtcol.Columns.Add("OutWeight", typeof(decimal));
				dtcol.Columns.Add("BalWeight", typeof(decimal));
				dtcol.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcol.Rows.Add(dt.Rows[i]["RefDocumentTypeId"], dt.Rows[i]["RefDocIdNo"], dt.Rows[i]["RefDocSubIdNo"], dt.Rows[i]["DocumentTypeCode"], dt.Rows[i]["DocCodeNo"], dt.Rows[i]["ReferedDocNos"], dt.Rows[i]["InQty"], dt.Rows[i]["OutQty"], dt.Rows[i]["BalQty"], dt.Rows[i]["InWeight"], dt.Rows[i]["OutWeight"], dt.Rows[i]["BalWeight"]);
				}
				grdfrm.DataSource = dtcol;
				grdfrm.RetrieveStructure();
				gridSetting();
			}
			else
			{
				grdfrm.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridSetting()
	{
		try
		{
			grdfrm.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdfrm.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdfrm.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdfrm.RootTable.Columns["InQty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["InQty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["InQty"].FormatString = "#,#;(#,#.##);0";
			grdfrm.RootTable.Columns["InQty"].TotalFormatString = "#,#;(#,#.##);0";
			grdfrm.RootTable.Columns["OutQty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["OutQty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["OutQty"].FormatString = "#,#;(#,#.##);0";
			grdfrm.RootTable.Columns["OutQty"].TotalFormatString = "#,#;(#,#.##);0";
			grdfrm.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["BalQty"].FormatString = "#,#;(#,#.##);0";
			grdfrm.RootTable.Columns["BalQty"].TotalFormatString = "#,#;(#,#.##);0";
			grdfrm.RootTable.Columns["InWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["InWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["InWeight"].FormatString = "#,#;(#,#.##);0";
			grdfrm.RootTable.Columns["InWeight"].TotalFormatString = "#,#;(#,#.##);0";
			grdfrm.RootTable.Columns["OutWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["OutWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["OutWeight"].FormatString = "#,#;(#,#.##);0";
			grdfrm.RootTable.Columns["OutWeight"].TotalFormatString = "#,#;(#,#.##);0";
			grdfrm.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["BalWeight"].FormatString = "#,#;(#,#.##);0";
			grdfrm.RootTable.Columns["BalWeight"].TotalFormatString = "#,#;(#,#.##);0";
			grdfrm.RootTable.Columns.Add("TranDetail");
			grdfrm.RootTable.Columns["TranDetail"].Caption = "TranDetail";
			grdfrm.RootTable.Columns["TranDetail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdfrm.RootTable.Columns["TranDetail"].ButtonStyle = (ButtonStyle)5;
			grdfrm.RootTable.Columns["TranDetail"].ButtonText = "TranDetail";
			grdfrm.RootTable.Columns["TranDetail"].Width = 80;
			CommonServices.GridAutoAdjustmentNew(grdfrm);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			GridFill();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdfrm).Name);
			if (fs != null)
			{
				grdfrm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintRegister_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "390_GetFIFODataForAudit.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void IssuanceHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				toolStrip1.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				BtnPrintRegister_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StorePurchaseRegister_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "TranDetail")
			{
				StockTransactionDetail stockTransactionDetail = new StockTransactionDetail(UserAccount);
				stockTransactionDetail.RefDocumentTypeId = Conversion.ToInt(grdfrm.CurrentRow.Cells["RefDocumentTypeId"].Value);
				stockTransactionDetail.RefDocId = Conversion.ToInt(grdfrm.CurrentRow.Cells["RefDocIdNo"].Value);
				stockTransactionDetail.RefDocDetailId = Conversion.ToInt(grdfrm.CurrentRow.Cells["RefDocSubIdNo"].Value);
				stockTransactionDetail.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintRegister = new System.Windows.Forms.ToolStripButton();
		this.grdfrm = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 30);
		this.panel1.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnRefresh, this.BtnPrintRegister });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(964, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(59, 24);
		this.btnRefresh.Text = "&New";
		this.btnRefresh.Click += new System.EventHandler(btnNew_Click);
		this.BtnPrintRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintRegister.Name = "BtnPrintRegister";
		this.BtnPrintRegister.Size = new System.Drawing.Size(92, 24);
		this.BtnPrintRegister.Text = "Print-390";
		this.BtnPrintRegister.Click += new System.EventHandler(BtnPrintRegister_Click);
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(964, 602);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 30);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 631);
		this.panel2.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 29);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(964, 602);
		this.panel6.TabIndex = 1;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(964, 29);
		this.panel4.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(914, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Transparent;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label5.Location = new System.Drawing.Point(5, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(156, 19);
		this.label5.TabIndex = 1;
		this.label5.Text = "FIFO Audit Report";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmFiFoAuditReport";
		this.Text = "Eccount Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(StorePurchaseRegister_FormClosing);
		base.Load += new System.EventHandler(frmGatePassReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(IssuanceHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		base.ResumeLayout(false);
	}
}
