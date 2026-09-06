using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Purchase;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ApprovalDashboard;

public class PendingApprovalSupplyOrder : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	public static int DocumentTypeId;

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Label label23;

	internal GridEX DataGridHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel5;

	private Panel panel6;

	internal GridEX grdDetail;

	public PendingApprovalSupplyOrder(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void frmPurchaseInvoiceHistory_Load(object sender, EventArgs e)
	{
		try
		{
			gridHisoryDetail();
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
			gridHisoryDetail();
			grdDetail.DataSource = null;
			grdDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void gridHisoryDetail()
	{
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DocNo");
			dt.Columns.Add("DocDate");
			dt.Columns.Add("ManualRefNo");
			dt.Columns.Add("BiltyNo");
			dt.Columns.Add("DestinationsLocation");
			dt.Columns.Add("VehicleNo");
			dt.Columns.Add("Status");
			dt.Columns.Add("EntryUser");
			dt.Columns.Add("TotalQuantity", typeof(double));
			dt.Columns.Add("TotalNetWeight", typeof(double));
			dt.Columns.Add("RemarksHeader");
			DataTable dtH = InvSupplyOrder.UnApprovedSupplyOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtH.Rows.Count > 0)
			{
				for (int i = 0; i < dtH.Rows.Count; i++)
				{
					dt.Rows.Add(dtH.Rows[i]["Id"], dtH.Rows[i]["DocNo"], Conversion.ToDateTime(dtH.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtH.Rows[i]["ManualRefNo"], dtH.Rows[i]["BiltyNos"], dtH.Rows[i]["DestinationsLocation"], dtH.Rows[i]["VehicleNos"], dtH.Rows[i]["SupplyOrderStatus"], dtH.Rows[i]["EntryUser"], dtH.Rows[i]["TotalQuantity"], dtH.Rows[i]["TotalNetWeight"], dtH.Rows[i]["RemarksHeader"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				GridSettings();
			}
			else
			{
				DataGridHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["TotalQuantity"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalQuantity"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["TotalQuantity"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalQuantity"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["TotalNetWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalNetWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["TotalNetWeight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalNetWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns.Add("Slip");
			DataGridHistory.RootTable.Columns["Slip"].Caption = "Slip";
			DataGridHistory.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Slip"].ButtonText = "Slip";
			DataGridHistory.RootTable.Columns["Slip"].Width = 60;
			DataGridHistory.RootTable.Columns["Slip"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Approve");
			DataGridHistory.RootTable.Columns["Approve"].Caption = "Approve";
			DataGridHistory.RootTable.Columns["Approve"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Approve"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Approve"].ButtonText = "Approve";
			DataGridHistory.RootTable.Columns["Approve"].Width = 60;
			DataGridHistory.RootTable.Columns["Approve"].Position = 1;
			DataGridHistory.RootTable.Columns.Add("Detail");
			DataGridHistory.RootTable.Columns["Detail"].Caption = "Detail";
			DataGridHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			DataGridHistory.RootTable.Columns["Detail"].Width = 60;
			DataGridHistory.RootTable.Columns["Detail"].Position = 2;
			DataGridHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void frmPurchaseInvoiceHistory_KeyDown(object sender, KeyEventArgs e)
	{
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnNew_Click(null, null);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			gridHisoryDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport()
	{
		try
		{
			CommonServices.PurchaseOrderSlipReport203(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		try
		{
			if (e.Column.Key == "Slip")
			{
				GenerateReport();
			}
			if (e.Column.Key == "Approve")
			{
				if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				InvSupplyOrder.ApproveSupplyOrder(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()),
					EntryUser = UserAccount.ID
				});
				MessageBox.Show("Record Post Successfully");
				gridHisoryDetail();
				grdDetail.ClearStructure();
			}
			if (e.Column.Key == "Detail")
			{
				GetDetailGrdByHeadId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		try
		{
			DataTable table = new DataTable();
			table.Columns.Add("OrderNo");
			table.Columns.Add("SupplierName");
			table.Columns.Add("ItemName");
			table.Columns.Add("CropYear");
			table.Columns.Add("PackSize");
			table.Columns.Add("Qty", typeof(double));
			table.Columns.Add("NetWeight", typeof(double));
			table.Columns.Add("EbWtUnit", typeof(double));
			table.Columns.Add("EbWtTotal", typeof(double));
			table.Columns.Add("GrossWeight", typeof(double));
			table.Columns.Add("Moisture");
			table.Columns.Add("RemarksDetail");
			InvSupplyOrder obj = new InvSupplyOrder();
			obj = InvSupplyOrder.GetByID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			if (obj.InvSupplyOrderDetailList.Count > 0)
			{
				table.Rows.Clear();
				for (int i = 0; i < obj.InvSupplyOrderDetailList.Count; i++)
				{
					table.Rows.Add(obj.InvSupplyOrderDetailList[i].OrderNo, obj.InvSupplyOrderDetailList[i].SupplierName, obj.InvSupplyOrderDetailList[i].ItemName, obj.InvSupplyOrderDetailList[i].CropYear, obj.InvSupplyOrderDetailList[i].PackSize, obj.InvSupplyOrderDetailList[i].Qty, obj.InvSupplyOrderDetailList[i].NetWeight, obj.InvSupplyOrderDetailList[i].EbWtUnit, obj.InvSupplyOrderDetailList[i].EbWtTotal, obj.InvSupplyOrderDetailList[i].GrossWeight, obj.InvSupplyOrderDetailList[i].Moisture, obj.InvSupplyOrderDetailList[i].RemarksDetail);
				}
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				DetailGridSetting();
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["ItemName"].Width = 200;
			grdDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Qty"].FormatString = "0,0";
			grdDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EbWtUnit"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EbWtUnit"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EbWtUnit"].FormatString = "0,0";
			grdDetail.RootTable.Columns["EbWtUnit"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EbWtTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EbWtTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EbWtTotal"].FormatString = "0,0";
			grdDetail.RootTable.Columns["EbWtTotal"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = int.Parse(DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
			if (clsGlobalVariables.ScreenViewReights.Count > 0)
			{
				SupplyOrderPurchase obj = new SupplyOrderPurchase(UserAccount);
				if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "SupplyOrderPurchase").ToList()[0].Value.ToString()))
				{
					obj.Show();
					obj.ReadById(Id);
				}
				else
				{
					MessageBox.Show("Please Check Screen Rights");
				}
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
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Expected O, but got Unknown
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.PendingApprovalSupplyOrder));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1155, 660);
		this.panel1.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 334);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1155, 326);
		this.panel6.TabIndex = 48;
		this.grdDetail.AllowDelete = (InheritableBoolean)1;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1155, 326);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 47;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 67);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1155, 267);
		this.panel5.TabIndex = 47;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1155, 267);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 46;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.CellValueChanged += new ColumnActionEventHandler(DataGridHistory_CellValueChanged);
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 26);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1155, 41);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 8);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(122, 24);
		this.label23.TabIndex = 4;
		this.label23.Text = "Supply Order";
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1155, 26);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1102, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNew });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1155, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1155, 660);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PendingApprovalSupplyOrder";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmPurchaseInvoiceHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPurchaseInvoiceHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
