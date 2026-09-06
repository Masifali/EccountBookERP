using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Lab;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ApprovalDashboard;

public class LabSampleAnalysisForApproval : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtStatus = new DataTable();

	public DataTable dtViewAttachments = new DataTable();

	private IContainer components;

	private ToolStripButton btnnew;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton6;

	private ToolStrip toolStrip1;

	private Panel panel2;

	internal GridEX grdHistory;

	private Panel panel4;

	private Panel panel1;

	private ToolStripButton btnApproved;

	private CtrlGrdBar ctrlGrdBar1;

	public LabSampleAnalysisForApproval(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmStockWithSupplierHistory_Load(object sender, EventArgs e)
	{
		try
		{
			StatusFill();
			GridFill();
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
			GridFill();
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

	private void StatusFill()
	{
		try
		{
			dtStatus.Columns.Add("Id");
			dtStatus.Columns.Add("Status");
			dtStatus.Rows.Add(1, "Accepted");
			dtStatus.Rows.Add(2, "Rejected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = InvLabAnalysisPurchaseHeader.GetLabDataForSampleApproval(new InvLabAnalysisPurchaseHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Id");
				dtGrid.Columns.Add("DocumentTypeId");
				dtGrid.Columns.Add("DocDate", typeof(DateTime));
				dtGrid.Columns.Add("DocNo", typeof(int));
				dtGrid.Columns.Add("ItemName");
				dtGrid.Columns.Add("CropYear");
				dtGrid.Columns.Add("AnalysisGroup");
				dtGrid.Columns.Add("LabStatus");
				dtGrid.Columns.Add("EntryDate", typeof(DateTime));
				dtGrid.Columns.Add("EntryUser");
				dtGrid.Columns.Add("IsAcceptedApproved");
				dtGrid.Columns.Add("NoOfAttachments");
				dtGrid.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtGrid.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), dt.Rows[i]["DocNo"], dt.Rows[i]["ItemName"], dt.Rows[i]["Crop"], dt.Rows[i]["AnalysisGroupDescription"], dt.Rows[i]["Lab_Status"], dt.Rows[i]["EntryDate"], dt.Rows[i]["UserName"], "", dt.Rows[i]["NoOfAttachments"]);
				}
				grdHistory.DataSource = dtGrid;
				grdHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdHistory.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdHistory.RootTable.Columns["AnalysisGroup"].Width = 70;
			grdHistory.RootTable.Columns["LabStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["IsAcceptedApproved"].Width = 80;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["Id"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["DocumentTypeId"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["NoOfAttachments"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["AnalysisGroup"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["EntryDate"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["EntryUser"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["LabStatus"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["IsAcceptedApproved"].ColumnType = (ColumnType)0;
			grdHistory.RootTable.Columns["IsAcceptedApproved"].HasValueList = true;
			grdHistory.RootTable.Columns["IsAcceptedApproved"].LimitToList = true;
			grdHistory.RootTable.Columns["IsAcceptedApproved"].EditType = (EditType)4;
			grdHistory.RootTable.Columns["IsAcceptedApproved"].Caption = "Status(@Approval)";
			grdHistory.RootTable.Columns["IsAcceptedApproved"].ValueList.PopulateValueList((IEnumerable)dtStatus.DefaultView, "Id", "Status");
			grdHistory.RootTable.Columns.Add("Select");
			grdHistory.RootTable.Columns["Select"].ActAsSelector = true;
			grdHistory.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdHistory.RootTable.Columns["Select"].Width = 40;
			grdHistory.RootTable.Columns["Select"].Position = 0;
			grdHistory.RootTable.Columns.Add("Print");
			grdHistory.RootTable.Columns["Print"].Caption = "Print";
			grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdHistory.RootTable.Columns["Print"].Width = 60;
			grdHistory.RootTable.Columns["Print"].Position = 1;
			grdHistory.RootTable.Columns["IsAcceptedApproved"].Position = 2;
			grdHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Print")
			{
				PrintSlip(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			if (grdHistory.CurrentColumn != null)
			{
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					PrintSlip(Id);
				}
				else if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (Conversion.ToInt(item.Cells["NoOfAttachments"].Value) > 0)
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(item.Cells["Id"].Value), "InvLabPurchaseAnalysis");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PrintSlip(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = Id;
			DataTable dt = InvLabAnalysisPurchaseHeader.RptInvLabPurchaseAnalysisSlip653(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvLabAnalysisPurchaseHeader.GetDataForLabPurchaseAnalysisSubReport(obj.Id), "LabPurchaseAnalysisSubParameterReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "653-RptInvLabPurchaseAnalysisSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			if (grdHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			ApprovalList obj = new ApprovalList();
			obj.AllApprovalLists = new List<ApprovalList>();
			GridEXRow[] checkedRows = grdHistory.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				ApprovalList detail = new ApprovalList();
				detail.OrganizationId = UserAccount.OrganizationId;
				detail.CompanyId = UserAccount.CompanyId;
				detail.EntryUser = UserAccount.ID;
				detail.Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				if (Conversion.ToInt(item.Cells["IsAcceptedApproved"].Value) == 1)
				{
					detail.ActionTypeId = true;
				}
				if (Conversion.ToInt(item.Cells["IsAcceptedApproved"].Value) == 2)
				{
					detail.ActionTypeId = false;
				}
				if (Conversion.ToInt(item.Cells["IsAcceptedApproved"].Value) == 0)
				{
					throw new Exception("Please Update Document Status!");
				}
				obj.AllApprovalLists.Add(detail);
			}
			InvLabAnalysisPurchaseHeader.RecordApprovedBySampleLabHeaderId(obj);
			MessageBox.Show("Record Approved Successfully");
			Reset();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.LabSampleAnalysisForApproval));
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnApproved = new System.Windows.Forms.ToolStripButton();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton6.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(171, 24);
		this.toolStripButton6.Text = "Lab Sample Analysis    ";
		this.toolStripButton6.ToolTipText = "Lab Purchase Analysis";
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripButton6, this.toolStripSeparator1, this.btnnew, this.btnApproved });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1033, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(70, 24);
		this.btnnew.Text = "&Refresh";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnApproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnApproved.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(83, 24);
		this.btnApproved.Text = "Approved";
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1033, 29);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(979, 0);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(48, 27);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1033, 550);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 48;
		((System.Windows.Forms.Control)(object)this.grdHistory).TabStop = false;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 29);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1033, 550);
		this.panel4.TabIndex = 2;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1033, 579);
		this.panel1.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1033, 579);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "LabSampleAnalysisForApproval";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmStockWithSupplierHistory_Load);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
