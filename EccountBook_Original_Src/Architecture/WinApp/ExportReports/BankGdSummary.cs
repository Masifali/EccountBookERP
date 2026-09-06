using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ExportReports;

public class BankGdSummary : Form
{
	private DataTable dtGrid = new DataTable();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private Panel panel5;

	private Label label3;

	private Panel panel1;

	private Label label50;

	internal GridEX grdfrm;

	private Panel panel2;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnShow;

	private TextBox txtzerBalance;

	private ToolStripButton btnPrint;

	private ToolStripButton btnShortCutKey;

	public BankGdSummary(UserAccount pUserAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void BankGdSummary_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(816, 489);
			txtzerBalance.Focus();
			btnShow_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			txtzerBalance.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.Amount = Conversion.ToDouble((object)txtzerBalance.Text);
			dtGrid = ExportReports.BankGdSummary(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("GdId", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("CustomerName", typeof(string));
				dtcol.Columns.Add("InvoiceNo", typeof(string));
				dtcol.Columns.Add("InvoiceDate");
				dtcol.Columns.Add("InvoiceAmount", typeof(double));
				dtcol.Columns.Add("Mtons");
				dtcol.Columns.Add("GdNo", typeof(string));
				dtcol.Columns.Add("GdAmount", typeof(double));
				dtcol.Columns.Add("CommAmount", typeof(double));
				dtcol.Columns.Add("RealizedAmount", typeof(double));
				dtcol.Columns.Add("CommRealizeTotal", typeof(double));
				dtcol.Columns.Add("GdBalance", typeof(double));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dtcol.Rows.Add(Conversion.ToInt(dtGrid.Rows[i]["GdId"]), Conversion.ToInt(dtGrid.Rows[i]["DocumentTypeId"]), Conversion.ToString(dtGrid.Rows[i]["CustomerName"]), Conversion.ToString(dtGrid.Rows[i]["InvoiceNo"]), dtGrid.Rows[i]["InvoiceDate"], Conversion.ToDouble(dtGrid.Rows[i]["InvoiceAmount"]), dtGrid.Rows[i]["Mtons"], Conversion.ToString(dtGrid.Rows[i]["GdNo"]), Conversion.ToDouble(dtGrid.Rows[i]["GdAmount"]), Conversion.ToDouble(dtGrid.Rows[i]["CommAmount"]), Conversion.ToDouble(dtGrid.Rows[i]["RealizedAmount"]), Conversion.ToDouble(dtGrid.Rows[i]["CommRealizeTotal"]), Conversion.ToDouble(dtGrid.Rows[i]["GdBalance"]));
				}
				grdfrm.DataSource = dtcol;
				grdfrm.RetrieveStructure();
				grdsetting();
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

	private void grdsetting()
	{
		try
		{
			grdfrm.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grdfrm.RootTable.Columns["GdId"].Visible = false;
			grdfrm.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdfrm.RootTable.Columns["InvoiceAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["InvoiceAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["InvoiceAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["InvoiceAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["CustomerName"].Width = 300;
			grdfrm.RootTable.Columns["InvoiceNo"].Width = 120;
			grdfrm.RootTable.Columns["InvoiceDate"].Width = 120;
			grdfrm.RootTable.Columns["InvoiceAmount"].Width = 150;
			grdfrm.RootTable.Columns["Mtons"].Width = 120;
			grdfrm.RootTable.Columns["GdNo"].Width = 220;
			grdfrm.RootTable.Columns["GdAmount"].Width = 120;
			grdfrm.RootTable.Columns["CommAmount"].Width = 120;
			grdfrm.RootTable.Columns["RealizedAmount"].Width = 120;
			grdfrm.RootTable.Columns["CommRealizeTotal"].Width = 120;
			grdfrm.RootTable.Columns["GdBalance"].Width = 150;
			grdfrm.RootTable.Columns["Mtons"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Mtons"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["Mtons"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["GdAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["GdAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["GdAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["GdAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["RealizedAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["RealizedAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["RealizedAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["RealizedAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["CommRealizeTotal"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["CommRealizeTotal"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["CommRealizeTotal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["CommRealizeTotal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["GdBalance"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["GdBalance"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["GdBalance"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["GdBalance"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["GdNo"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid == null || dtGrid.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGrid, "289-BankGdsSummaryRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankGdSummary_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtzerBalance.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdfrm).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtzerBalance.Focus();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankGdSummary_FormClosed(object sender, FormClosedEventArgs e)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on From Balance");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Zero Balance");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "GdNo")
			{
				frmGDBreakUp frmGDBreakUp = new frmGDBreakUp(UserAccount);
				frmGDBreakUp.GdId = Conversion.ToInt(grdfrm.CurrentRow.Cells["GdId"].Value);
				frmGDBreakUp.DocumentTypeId = Conversion.ToInt(grdfrm.CurrentRow.Cells["DocumentTypeId"].Value);
				frmGDBreakUp.Show();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.BankGdSummary));
		GridEXLayout grdfrm_DesignTimeLayout = new GridEXLayout();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.txtzerBalance = new System.Windows.Forms.TextBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.grdfrm = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.tsItemType.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnPrint, this.btnShortCutKey });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(800, 27);
		this.tsItemType.TabIndex = 4;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "289-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnShortCutKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKey.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKey.Image");
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 27);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(800, 28);
		this.panel5.TabIndex = 20;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(4, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(232, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Bank Gd Summary Register";
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.txtzerBalance);
		this.panel1.Controls.Add(this.btnShow);
		this.panel1.Controls.Add(this.label50);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 55);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(800, 41);
		this.panel1.TabIndex = 0;
		this.txtzerBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtzerBalance.Location = new System.Drawing.Point(112, 9);
		this.txtzerBalance.Name = "txtzerBalance";
		this.txtzerBalance.ShortcutsEnabled = false;
		this.txtzerBalance.Size = new System.Drawing.Size(180, 23);
		this.txtzerBalance.TabIndex = 0;
		this.txtzerBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtzerBalance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox1_KeyPress);
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(301, 7);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(75, 26);
		this.btnShow.TabIndex = 1;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(13, 11);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(82, 15);
		this.label50.TabIndex = 24;
		this.label50.Text = "From  Balance";
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		grdfrm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdfrm.DesignTimeLayout = grdfrm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.DynamicFiltering = true;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 124);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(800, 326);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 1;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 96);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(800, 28);
		this.panel2.TabIndex = 23;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(754, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 23);
		this.ctrlGrdBar1.TabIndex = 1;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 450);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.tsItemType);
		base.KeyPreview = true;
		base.Name = "BankGdSummary";
		this.Text = "Form1";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(BankGdSummary_FormClosed);
		base.Load += new System.EventHandler(BankGdSummary_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(BankGdSummary_KeyDown);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
