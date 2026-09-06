using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Taxation;

public class frmSaleTaxSummaryRpt : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DataTable dtGrid = new DataTable();

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Button btnShow;

	private Label label6;

	private DateTimePicker DateFrom;

	private Label label4;

	private DateTimePicker DateTo;

	private Panel panel2;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private ToolStripButton btnPrint;

	internal GridEX gridInput;

	internal GridEX gridOutput;

	private Panel panel5;

	private Panel panel6;

	private Panel panel8;

	private Panel panel9;

	private Label label3;

	private Label label2;

	private Label label1;

	private CtrlGrdBar OutputCtrlBar;

	private CtrlGrdBar InputCtrlBar;

	private Panel panel7;

	private Panel panel10;

	public frmSaleTaxSummaryRpt(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void frmSaleTaxSummaryRpt_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.FromDate = Conversion.ToDateTime((object)DateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)DateTo.Value);
			OutputGridBind(obj);
			InputGridBind(obj);
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
			DateFrom.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void OutputGridBind(ReportsParameters obj)
	{
		try
		{
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtGrid = TaxSalesNotesGLMaping.SaleTaxSummary(obj);
			DataTable dt = new DataTable();
			dt.Columns.Add("TaxTypes", typeof(string));
			dt.Columns.Add("TaxNotesHeading", typeof(string));
			dt.Columns.Add("DebitAmount", typeof(double));
			dt.Columns.Add("CreditAmount", typeof(double));
			dt.Columns.Add("Amount", typeof(double));
			for (int i = 0; i < dtGrid.Rows.Count; i++)
			{
				if (Convert.ToString(dtGrid.Rows[i]["TaxTypeInOut"]) == "Output")
				{
					dt.Rows.Add(Conversion.ToString(dtGrid.Rows[i]["TaxTypeInOut"]), Conversion.ToString(dtGrid.Rows[i]["TaxNotesHeading"]), Conversion.ToDouble(dtGrid.Rows[i]["DebitAmount"]), Conversion.ToDouble(dtGrid.Rows[i]["CreditAmount"]), Conversion.ToDouble(dtGrid.Rows[i]["Amount"]));
				}
			}
			gridOutput.DataSource = dt;
			gridOutput.RetrieveStructure();
			OutputGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void InputGridBind(ReportsParameters obj)
	{
		try
		{
			dtGrid = TaxSalesNotesGLMaping.SaleTaxSummary(obj);
			DataTable dt = new DataTable();
			dt.Columns.Add("TaxTypes", typeof(string));
			dt.Columns.Add("TaxNotesHeading", typeof(string));
			dt.Columns.Add("DebitAmount", typeof(double));
			dt.Columns.Add("CreditAmount", typeof(double));
			dt.Columns.Add("Amount", typeof(double));
			for (int i = 0; i < dtGrid.Rows.Count; i++)
			{
				if (Convert.ToString(dtGrid.Rows[i]["TaxTypeInOut"]) == "Input")
				{
					dt.Rows.Add(Conversion.ToString(dtGrid.Rows[i]["TaxTypeInOut"]), Conversion.ToString(dtGrid.Rows[i]["TaxNotesHeading"]), Conversion.ToDouble(dtGrid.Rows[i]["DebitAmount"]), Conversion.ToDouble(dtGrid.Rows[i]["CreditAmount"]), Conversion.ToDouble(dtGrid.Rows[i]["Amount"]));
				}
			}
			gridInput.DataSource = dt;
			gridInput.RetrieveStructure();
			InputGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void InputGridSetting()
	{
		try
		{
			gridInput.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			gridInput.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			gridInput.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			gridInput.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			gridInput.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			gridInput.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			gridInput.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			gridInput.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			gridInput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			gridInput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			gridInput.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			gridInput.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, gridInput);
			InputCtrlBar_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OutputGridSetting()
	{
		try
		{
			gridInput.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			gridInput.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			gridInput.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			gridInput.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			gridInput.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			gridInput.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			gridInput.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			gridInput.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			gridInput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			gridInput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			gridInput.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			gridInput.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, gridOutput);
			OutputCtrlBar_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleTaxSummaryRpt_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InputCtrlBar_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)gridInput).Name);
			if (fs != null)
			{
				gridInput.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OutputCtrlBar_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)gridOutput).Name);
			if (fs != null)
			{
				gridOutput.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.FromDate = Conversion.ToDateTime((object)DateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)DateTo.Value);
			OutputGridBind(obj);
			InputGridBind(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleTaxSummaryRpt_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.gridOutput = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.OutputCtrlBar = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.gridInput = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.InputCtrlBar = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.DateFrom = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.DateTo = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridOutput).BeginInit();
		this.panel8.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridInput).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tsItemType.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1238, 655);
		this.panel1.TabIndex = 3;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 110);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1238, 545);
		this.panel4.TabIndex = 2;
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1238, 545);
		this.panel5.TabIndex = 82;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.gridOutput);
		this.panel10.Controls.Add(this.panel8);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(615, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(620, 545);
		this.panel10.TabIndex = 82;
		this.gridOutput.AllowEdit = (InheritableBoolean)2;
		this.gridOutput.ColumnAutoResize = true;
		((System.Windows.Forms.Control)(object)this.gridOutput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridOutput.FilterMode = (FilterMode)2;
		this.gridOutput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridOutput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridOutput.GroupTotals = (GroupTotals)2;
		this.gridOutput.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.gridOutput).Location = new System.Drawing.Point(0, 31);
		((System.Windows.Forms.Control)(object)this.gridOutput).Name = "gridOutput";
		((ScrollableBase)this.gridOutput).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridOutput).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.gridOutput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridOutput).Size = new System.Drawing.Size(620, 514);
		((System.Windows.Forms.Control)(object)this.gridOutput).TabIndex = 80;
		this.gridOutput.TotalRow = (InheritableBoolean)1;
		this.gridOutput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridOutput).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.OutputCtrlBar);
		this.panel8.Controls.Add(this.label3);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(620, 31);
		this.panel8.TabIndex = 0;
		this.OutputCtrlBar.FormName = this;
		this.OutputCtrlBar.Location = new System.Drawing.Point(566, 1);
		this.OutputCtrlBar.MyGrid = this.gridOutput;
		this.OutputCtrlBar.Name = "OutputCtrlBar";
		this.OutputCtrlBar.Size = new System.Drawing.Size(48, 26);
		this.OutputCtrlBar.TabIndex = 3;
		this.OutputCtrlBar.Load += new System.EventHandler(OutputCtrlBar_Load);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(6, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(128, 19);
		this.label3.TabIndex = 2;
		this.label3.Text = "Output History";
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.gridInput);
		this.panel7.Controls.Add(this.panel6);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(615, 545);
		this.panel7.TabIndex = 81;
		this.gridInput.AllowEdit = (InheritableBoolean)2;
		this.gridInput.ColumnAutoResize = true;
		((System.Windows.Forms.Control)(object)this.gridInput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridInput.FilterMode = (FilterMode)2;
		this.gridInput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridInput.GroupTotals = (GroupTotals)2;
		this.gridInput.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.gridInput).Location = new System.Drawing.Point(0, 31);
		((System.Windows.Forms.Control)(object)this.gridInput).Name = "gridInput";
		((ScrollableBase)this.gridInput).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridInput).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.gridInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridInput).Size = new System.Drawing.Size(615, 514);
		((System.Windows.Forms.Control)(object)this.gridInput).TabIndex = 80;
		this.gridInput.TotalRow = (InheritableBoolean)1;
		this.gridInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridInput).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.InputCtrlBar);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(615, 31);
		this.panel6.TabIndex = 0;
		this.InputCtrlBar.FormName = this;
		this.InputCtrlBar.Location = new System.Drawing.Point(565, 2);
		this.InputCtrlBar.MyGrid = this.gridInput;
		this.InputCtrlBar.Name = "InputCtrlBar";
		this.InputCtrlBar.Size = new System.Drawing.Size(49, 26);
		this.InputCtrlBar.TabIndex = 2;
		this.InputCtrlBar.Load += new System.EventHandler(InputCtrlBar_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(116, 19);
		this.label2.TabIndex = 1;
		this.label2.Text = "Input History";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel9);
		this.panel3.Controls.Add(this.btnShow);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.DateFrom);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.DateTo);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1238, 85);
		this.panel3.TabIndex = 1;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label1);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1238, 29);
		this.panel9.TabIndex = 25;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(161, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Sale Tax Summary";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(394, 50);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(64, 25);
		this.btnShow.TabIndex = 2;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(13, 32);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(185, 15);
		this.label6.TabIndex = 24;
		this.label6.Text = "Date From";
		this.DateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.DateFrom.Location = new System.Drawing.Point(13, 51);
		this.DateFrom.Name = "DateFrom";
		this.DateFrom.Size = new System.Drawing.Size(185, 23);
		this.DateFrom.TabIndex = 0;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(203, 32);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(185, 15);
		this.label4.TabIndex = 20;
		this.label4.Text = "Date To";
		this.DateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.DateTo.Location = new System.Drawing.Point(203, 51);
		this.DateTo.Name = "DateTo";
		this.DateTo.Size = new System.Drawing.Size(185, 23);
		this.DateTo.TabIndex = 1;
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1238, 25);
		this.panel2.TabIndex = 0;
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnnew, this.btnPrint });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1238, 27);
		this.tsItemType.TabIndex = 2;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(23, 24);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1238, 655);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmSaleTaxSummaryRpt";
		this.Text = "Sale Tax Summary";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSaleTaxSummaryRpt_FormClosing);
		base.Load += new System.EventHandler(frmSaleTaxSummaryRpt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleTaxSummaryRpt_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridOutput).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridInput).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		base.ResumeLayout(false);
	}
}
