using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.LeaveManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.ApprovalManagement;

public class PendingLeaveDetailForApproval : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dttable = new DataTable();

	public int EmployeeId;

	public int LeaveTypeProfileId;

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel4;

	internal GridEX grdHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label15;

	public PendingLeaveDetailForApproval(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void EmployeeFamilyInfo_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			HistoryForApproval(EmployeeId);
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
			HistoryForApproval(EmployeeId);
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

	private void HistoryForApproval(int EmployeeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		try
		{
			DataTable dt = genEmployeeLeave.GetLeaveDetailByEmployeeLeaveId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EmployeeId = EmployeeId,
				LeaveTypeProfileId = LeaveTypeProfileId
			});
			if (dt.Rows.Count > 0)
			{
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				grdPendingForLoanSetting();
			}
			else
			{
				grdHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingForLoanSetting()
	{
		try
		{
			grdHistory.RootTable.Columns["EmployeeLeaveDetailId"].Visible = false;
			grdHistory.RootTable.Columns["EmployeeId"].Visible = false;
			grdHistory.RootTable.Columns["LeaveTypeProfileId"].Visible = false;
			grdHistory.RootTable.Columns["EmployeeLeaveQuotaId"].Visible = false;
			grdHistory.RootTable.Columns["EmployeeLeaveQuotaId"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["EmployeeId"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["LeaveTypeProfileId"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["EmployeeLeaveQuotaId"].EditType = (EditType)0;
			grdHistory.RootTable.Columns.Add("Reject");
			grdHistory.RootTable.Columns["Reject"].Caption = "Reject";
			grdHistory.RootTable.Columns["Reject"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Reject"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Reject"].ButtonText = "Reject";
			grdHistory.RootTable.Columns["Reject"].Width = 50;
			grdHistory.RootTable.Columns.Add("Approve");
			grdHistory.RootTable.Columns["Approve"].Caption = "Approve";
			grdHistory.RootTable.Columns["Approve"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Approve"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Approve"].ButtonText = "Approve";
			grdHistory.RootTable.Columns["Approve"].Width = 50;
			grdHistory.RootTable.Columns["Approve"].Position = 0;
			grdHistory.RootTable.Columns["Reject"].Position = 1;
			grdHistory.FrozenColumns = 2;
			CommonServices.GridAutoAdjustment(AryColWidth, grdHistory);
			grdHistory.RootTable.Columns["Reason"].Width = 200;
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

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (Conversion.ToString(item.Cells["Reason"].Value) == string.Empty)
			{
				throw new Exception("Reason Field Required");
			}
			if (e.Column.Key == "Reject")
			{
				if (MessageBox.Show("Are you sure to Reject?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				genEmployeeLeave.LeaveApproved(new LeaveApproval
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					LeaveQuotaId = Conversion.ToInt(item.Cells["EmployeeLeaveQuotaId"].Value),
					EmployeeId = Conversion.ToInt(item.Cells["EmployeeId"].Value),
					LeaveTypeProfileId = Conversion.ToInt(item.Cells["LeaveTypeProfileId"].Value),
					EmployeeLeaveDetailId = Conversion.ToInt(item.Cells["EmployeeLeaveDetailId"].Value),
					EntryUser = UserAccount.ID,
					ActionId = 1
				});
			}
			else if (e.Column.Key == "Approve")
			{
				if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				genEmployeeLeave.LeaveApproved(new LeaveApproval
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					LeaveQuotaId = Conversion.ToInt(item.Cells["EmployeeLeaveQuotaId"].Value),
					EmployeeId = Conversion.ToInt(item.Cells["EmployeeId"].Value),
					LeaveTypeProfileId = Conversion.ToInt(item.Cells["LeaveTypeProfileId"].Value),
					EmployeeLeaveDetailId = Conversion.ToInt(item.Cells["EmployeeLeaveDetailId"].Value),
					EntryUser = UserAccount.ID,
					ActionId = 2
				});
			}
			Reset();
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
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(951, 683);
		this.panel1.TabIndex = 2;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(951, 683);
		this.tabControl1.TabIndex = 2;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(943, 657);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(937, 27);
		this.panel4.TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		this.grdHistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 30);
		((System.Windows.Forms.Control)(object)this.grdHistory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(937, 624);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(888, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(47, 24);
		this.ctrlGrdBar1.TabIndex = 114;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(3, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(58, 19);
		this.label15.TabIndex = 115;
		this.label15.Text = "Detail";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(951, 683);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PendingLeaveDetailForApproval";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(EmployeeFamilyInfo_Load);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		base.ResumeLayout(false);
	}
}
