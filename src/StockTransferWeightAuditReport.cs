using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.Audit_Dashboard
{

public class StockTransferWeightAuditReport : Form
{
	private UserAccount UserAccount = new UserAccount();

	public bool SpecialApproval;

	private IContainer components;

	private Panel panel1;

	private Label lblCaption;

	private ToolStrip toolStrip1;

	private Panel panel3;

	private Panel panel5;

	private Label label4;

	private Label label2;

	private DateTimePicker ToDate;

	private DateTimePicker FromDate;

	private Panel panel2;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private ToolStripButton toolStripButton1;

	public StockTransferWeightAuditReport(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void Reset()
	{
		try
		{
			FromDate.Focus();
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			ToDate.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockTransferWeightAuditReport_Load(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			ToDate.Value = DateTime.Now;
			lblCaption.Text = "Stock Transfer Weight Audit Report";
			if (SpecialApproval)
			{
				lblCaption.Text = "Stock Transfer Special Approval";
				GridFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		try
		{
			DataSet ds = InvStockTransferHeader.StockTransferWeightAudit(UserAccount.OrganizationId, UserAccount.CompanyId, FromDate.Value, ToDate.Value, SpecialApproval ? 1 : 0);
			DataTable dt = ds?.Tables?[0];
			if (dt != null && dt.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dt, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				DataTable table = new DataTable();
				table.Columns.Add("InvStockTransferHeaderId", typeof(int));
				table.Columns.Add("InwardTransferId", typeof(int));
				table.Columns.Add("GatePassId", typeof(int));
				table.Columns.Add("InwardId", typeof(int));
				table.Columns.Add("TransferFromLocation", typeof(string));
				table.Columns.Add("ReceivedLocation", typeof(string));
				table.Columns.Add("TransferFromDate", typeof(DateTime));
				table.Columns.Add("TransferFromDocNo", typeof(int));
				table.Columns.Add("OutwardGpNo", typeof(int));
				table.Columns.Add("InwardGpNo", typeof(int));
				table.Columns.Add("ReceivedDate", typeof(DateTime));
				table.Columns.Add("ReceivedNo", typeof(int));
				table.Columns.Add("ItemName", typeof(string));
				table.Columns.Add("TransferWeight", typeof(double));
				table.Columns.Add("ReceivedWeight", typeof(double));
				table.Columns.Add("DifferenceWeight", typeof(double));
				table.Columns.Add("DifferenceAmount", typeof(double));
				table.Columns.Add("Less Excess", typeof(string));
				foreach (DataRow row in dt.Rows)
				{
					table.Rows.Add(row["InvStockTransferHeaderId"], row["InwardTransferId"], row["GatePassId"], row["InwardId"], row["TransferFromLocation"], row["ReceivedLocation"], row["TransferFromDate"], row["TransferFromDocNo"], row["OutwardGpNo"], row["InwardGpNo"], row["ReceivedDate"], row["ReceivedNo"], row["ItemName"], row["TransferWeight"], row["ReceivedWeight"], row["DifferenceWeight"], row["DifferenceAmount"], row["Less/Excess"]);
				}
				grdfrm.DataSource = table;
				grdfrm.RetrieveStructure();
				gridSetting();
			}
			else
			{
				grdfrm.DataSource = null;
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
			grdfrm.RootTable.Columns["InvStockTransferHeaderId"].Visible = false;
			grdfrm.RootTable.Columns["InwardTransferId"].Visible = false;
			grdfrm.RootTable.Columns["GatePassId"].Visible = false;
			grdfrm.RootTable.Columns["InwardId"].Visible = false;
			grdfrm.RootTable.Columns["TransferFromDocNo"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["ReceivedNo"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["OutwardGpNo"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["InwardGpNo"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["TransferFromLocation"].Width = 120;
			grdfrm.RootTable.Columns["TransferFromDate"].Width = 80;
			grdfrm.RootTable.Columns["TransferFromDocNo"].Width = 90;
			grdfrm.RootTable.Columns["TransferWeight"].Width = 80;
			grdfrm.RootTable.Columns["OutwardGpNo"].Width = 80;
			grdfrm.RootTable.Columns["ReceivedLocation"].Width = 100;
			grdfrm.RootTable.Columns["ReceivedDate"].Width = 80;
			grdfrm.RootTable.Columns["ReceivedNo"].Width = 80;
			grdfrm.RootTable.Columns["InwardGpNo"].Width = 80;
			grdfrm.RootTable.Columns["ItemName"].Width = 300;
			grdfrm.RootTable.Columns["ReceivedWeight"].Width = 80;
			grdfrm.RootTable.Columns["DifferenceWeight"].Width = 80;
			grdfrm.RootTable.Columns["DifferenceAmount"].Width = 80;
			grdfrm.RootTable.Columns["Less Excess"].Width = 60;
			grdfrm.RootTable.HeaderLines = 2;
			grdfrm.RootTable.Columns["TransferFromDate"].HeaderAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["TransferFromLocation"].HeaderAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["TransferFromDocNo"].HeaderAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["OutwardGpNo"].HeaderAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["InwardGpNo"].HeaderAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["ReceivedNo"].HeaderAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["ReceivedDate"].HeaderAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["ReceivedLocation"].HeaderAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["Less Excess"].HeaderAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["TransferWeight"].HeaderAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ReceivedWeight"].HeaderAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["DifferenceWeight"].HeaderAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["DifferenceAmount"].HeaderAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Less Excess"].TextAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["TransferFromDocNo"].TextAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["OutwardGpNo"].TextAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["InwardGpNo"].TextAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["ReceivedNo"].TextAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["TransferWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["TransferWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["TransferWeight"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["TransferWeight"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["ReceivedWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["ReceivedWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ReceivedWeight"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["ReceivedWeight"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["DifferenceWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["DifferenceWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["DifferenceWeight"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["DifferenceWeight"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["DifferenceAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["DifferenceAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["DifferenceAmount"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["DifferenceAmount"].TotalFormatString = "#,##0";
			if (SpecialApproval)
			{
				grdfrm.RootTable.Columns.Add("Approve");
				grdfrm.RootTable.Columns["Approve"].Caption = "Approve";
				grdfrm.RootTable.Columns["Approve"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdfrm.RootTable.Columns["Approve"].ButtonStyle = (ButtonStyle)5;
				grdfrm.RootTable.Columns["Approve"].ButtonText = "Approve";
				grdfrm.RootTable.Columns["Approve"].Width = 70;
				grdfrm.RootTable.Columns["Approve"].Position = 0;
			}
			else
			{
				grdfrm.RootTable.Columns.Remove("Approve");
			}
			ctrlGrdBar1_Load(null, null);
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

	private void toolStripButton1_Click(object sender, EventArgs e)
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

	private void StockTransferWeightAuditReport_FormClosing(object sender, FormClosingEventArgs e)
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

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdfrm.CurrentRow;
			if (e.Column.Key == "TransferFromDocNo")
			{
				CommonServices.StockTransferSlip406(Conversion.ToInt(item.Cells["InvStockTransferHeaderId"].Value));
			}
			else if (e.Column.Key == "ReceivedNo")
			{
				CommonServices.StockTransferSlip406(Conversion.ToInt(item.Cells["InwardTransferId"].Value));
			}
			else if (e.Column.Key == "OutwardGpNo")
			{
				CommonServices.OutwardGatePassWithWb(Conversion.ToInt(item.Cells["GatePassId"].Value));
			}
			else if (e.Column.Key == "InwardGpNo")
			{
				CommonServices.GatePassInwardSlipAndRegisterReport(Conversion.ToInt(item.Cells["InwardId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_008b: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdfrm.CurrentRow;
			if (e.Column.Key == "Approve" && MessageBox.Show("Are you sure to Approve Stock Transfer?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				InvStockTransferHeader.SpecialApproveStockTransfer(new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>
					{
						new ApprovalList
						{
							Id = Conversion.ToInt(item.Cells["InwardTransferId"].Value),
							EntryUser = UserAccount.ID
						}
					}
				});
				MessageBox.Show("Record Approved Successfully");
				GridFill();
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Expected O, but got Unknown
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdfrm = new GridEX();
		this.lblCaption = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnshow = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(916, 30);
		this.panel1.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.toolStripButton1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(916, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(868, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		this.grdfrm.DefaultFilterRowComparison = (FilterConditionOperator)7;
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
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(916, 494);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 4;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(4, 2);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(301, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Stock Transfer Weight Audit Report";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.lblCaption);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 30);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(916, 27);
		this.panel3.TabIndex = 3;
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.ToDate);
		this.panel5.Controls.Add(this.FromDate);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 57);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(916, 59);
		this.panel5.TabIndex = 4;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(248, 24);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 26);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnSearch_Click);
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(127, 8);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 15);
		this.label4.TabIndex = 61;
		this.label4.Text = "Date To";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(10, 8);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(63, 15);
		this.label2.TabIndex = 59;
		this.label2.Text = "Date From";
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(130, 27);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(115, 23);
		this.ToDate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(13, 27);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(115, 23);
		this.FromDate.TabIndex = 0;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 116);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(916, 494);
		this.panel2.TabIndex = 5;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(916, 610);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel1);
		base.Name = "StockTransferWeightAuditReport";
		this.Text = "Eccount Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(StockTransferWeightAuditReport_FormClosing);
		base.Load += new System.EventHandler(StockTransferWeightAuditReport_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
