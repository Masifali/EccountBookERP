using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.HRM.AttendanceManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.HRM_Reports;

public class AttendenceStatusByEmployeeAndDepartment : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable Departmentdata = new DataTable();

	private DataTable dt = new DataTable();

	public bool RequestByEmployee;

	public bool RequestForMonthlyStatus;

	public int EmployeeId;

	public string EmployeeName = "";

	public int DepartmentId;

	public string DepartmentName = "";

	public DateTime filterDate;

	private IContainer components;

	private Panel FilterPanel;

	private GroupBox groupBox1;

	private Label label12;

	internal UltraCombo cmbDepartment;

	private Button btnshow;

	private Label lblCaption;

	internal GridEX grd;

	public AttendenceStatusByEmployeeAndDepartment(UserAccount userAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void AttendenceStatusMonthly_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 500);
			if (RequestForMonthlyStatus)
			{
				dt.Columns.Add("Sr#", typeof(int));
				dt.Columns.Add("Month", typeof(string));
				dt.Columns.Add("EmployeeName", typeof(string));
				dt.Columns.Add("DepartmentName", typeof(string));
				dt.Columns.Add("TotalDays", typeof(int));
				dt.Columns.Add("Holidays", typeof(int));
				dt.Columns.Add("WorkingDays", typeof(int));
				dt.Columns.Add("Present", typeof(int));
				dt.Columns.Add("Absent", typeof(int));
				dt.Columns.Add("LateComing", typeof(int));
				dt.Columns.Add("EarlyLeave", typeof(int));
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSettingForMonthlyStatus();
			}
			else
			{
				dt.Columns.Add("Sr#", typeof(string));
				dt.Columns.Add("DepartmentId", typeof(string));
				dt.Columns.Add("DepartmentName", typeof(string));
				dt.Columns.Add("EmployeeId", typeof(int));
				dt.Columns.Add("EmployeeName", typeof(string));
				dt.Columns.Add("ThisMonthPresents");
				dt.Columns.Add("ThisMonthAbsents");
				dt.Columns.Add("ThisMonthLates");
				dt.Columns.Add("PreviousMonthPresents");
				dt.Columns.Add("PreviousMonthAbsents");
				dt.Columns.Add("PreviousMonthLates");
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSetting();
			}
			if (RequestByEmployee)
			{
				FilterPanel.Hide();
				lblCaption.Text = EmployeeName;
			}
			else
			{
				DepartmentFill();
				FilterPanel.Show();
				lblCaption.Text = DepartmentName;
				cmbDepartment.Value = DepartmentId;
			}
			if (RequestForMonthlyStatus)
			{
				GetMonthlyStatusData();
			}
			else
			{
				GetThisMonthPreviousMonthEmployeeDepartmentStatus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSetting()
	{
		try
		{
			grd.RootTable.Columns["EmployeeId"].Visible = false;
			grd.RootTable.Columns["DepartmentId"].Visible = false;
			if (RequestByEmployee)
			{
				grd.RootTable.Columns["EmployeeName"].Visible = false;
			}
			else
			{
				grd.RootTable.Columns["DepartmentName"].Visible = false;
			}
			grd.RootTable.Columns["EmployeeName"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["DepartmentName"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns["EmployeeName"].Width = 300;
			grd.RootTable.Columns["DepartmentName"].Width = 300;
			grd.RootTable.Columns["ThisMonthPresents"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ThisMonthPresents"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ThisMonthAbsents"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ThisMonthAbsents"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ThisMonthLates"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ThisMonthLates"].FormatString = "#,##0.###";
			grd.RootTable.Columns["PreviousMonthPresents"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PreviousMonthPresents"].FormatString = "#,##0.###";
			grd.RootTable.Columns["PreviousMonthAbsents"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PreviousMonthAbsents"].FormatString = "#,##0.###";
			grd.RootTable.Columns["PreviousMonthLates"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PreviousMonthLates"].FormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingForMonthlyStatus()
	{
		try
		{
			if (RequestByEmployee)
			{
				grd.RootTable.Columns["EmployeeName"].Visible = false;
			}
			else
			{
				grd.RootTable.Columns["DepartmentName"].Visible = false;
			}
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns["EmployeeName"].Width = 300;
			grd.RootTable.Columns["DepartmentName"].Width = 300;
			grd.RootTable.Columns["TotalDays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TotalDays"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Holidays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Holidays"].FormatString = "#,##0.###";
			grd.RootTable.Columns["WorkingDays"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WorkingDays"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Present"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Present"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Absent"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Absent"].FormatString = "#,##0.###";
			grd.RootTable.Columns["LateComing"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LateComing"].FormatString = "#,##0.###";
			grd.RootTable.Columns["EarlyLeave"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EarlyLeave"].FormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DepartmentFill()
	{
		try
		{
			if (Departmentdata.Rows.Count > 0)
			{
				DDL.BindDDLNew(Departmentdata, cmbDepartment, "DepartmentId", "DepartmentName", "Department Name", false);
			}
			else
			{
				((UltraGridBase)cmbDepartment).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetMonthlyStatusData()
	{
		try
		{
			dt.Rows.Clear();
			DataTable dtData = new DataTable();
			dtData = ((!RequestByEmployee) ? hrmManualAttendance.EmployeeAttandanceStatusMonthly(UserAccount.OrganizationId, UserAccount.CompanyId, 0L, Conversion.ToInt(cmbDepartment.Value)) : hrmManualAttendance.EmployeeAttandanceStatusMonthly(UserAccount.OrganizationId, UserAccount.CompanyId, (long)EmployeeId, 0));
			if (dtData.Rows.Count > 0)
			{
				for (int i = 0; i < dtData.Rows.Count; i++)
				{
					dt.Rows.Add(i + 1, dtData.Rows[i]["Month"], dtData.Rows[i]["EmployeeName"], dtData.Rows[i]["DepartmentName"], dtData.Rows[i]["TotalDays"], dtData.Rows[i]["HoliDay"], dtData.Rows[i]["WorkingDays"], dtData.Rows[i]["Present"], dtData.Rows[i]["Absent"], dtData.Rows[i]["LateComing"], dtData.Rows[i]["EarlyLeave"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSettingForMonthlyStatus();
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

	private void GetThisMonthPreviousMonthEmployeeDepartmentStatus()
	{
		try
		{
			dt.Rows.Clear();
			DataTable Data = hrmManualAttendance.EmployeeAttandanceSummaryStatus(UserAccount.OrganizationId, UserAccount.CompanyId, filterDate, Conversion.ToInt(cmbDepartment.Value));
			if (Data.Rows.Count > 0)
			{
				for (int i = 0; i < Data.Rows.Count; i++)
				{
					dt.Rows.Add(i + 1, Data.Rows[i]["DepartmentId"], Data.Rows[i]["DepartmentName"], Data.Rows[i]["EmployeeId"], Data.Rows[i]["EmployeeName"], Data.Rows[i]["Present"], Data.Rows[i]["Absent"], Data.Rows[i]["ThisMonthLate"], Data.Rows[i]["Last30DaysPresent"], Data.Rows[i]["Last30DaysAbsent"], Data.Rows[i]["Last30DaysLate"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSetting();
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

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0 && e.Column.Key == "EmployeeName")
			{
				AttendenceStatusByEmployeeAndDepartment attendenceStatusByEmployeeAndDepartment = new AttendenceStatusByEmployeeAndDepartment(UserAccount);
				attendenceStatusByEmployeeAndDepartment.RequestByEmployee = true;
				attendenceStatusByEmployeeAndDepartment.RequestForMonthlyStatus = true;
				attendenceStatusByEmployeeAndDepartment.EmployeeId = Conversion.ToInt(row.Cells["EmployeeId"].Value);
				attendenceStatusByEmployeeAndDepartment.EmployeeName = Conversion.ToString(row.Cells["EmployeeName"].Value);
				attendenceStatusByEmployeeAndDepartment.WindowState = FormWindowState.Maximized;
				attendenceStatusByEmployeeAndDepartment.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			if (RequestForMonthlyStatus)
			{
				GetMonthlyStatusData();
			}
			else
			{
				GetThisMonthPreviousMonthEmployeeDepartmentStatus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDepartment_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbDepartment.Value) > 0)
			{
				lblCaption.Text = ((Control)(object)cmbDepartment).Text;
			}
			else
			{
				lblCaption.Text = "";
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Expected O, but got Unknown
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.FilterPanel = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbDepartment = new UltraCombo();
		this.lblCaption = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.FilterPanel.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		base.SuspendLayout();
		this.FilterPanel.Controls.Add(this.groupBox1);
		this.FilterPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.FilterPanel.Location = new System.Drawing.Point(0, 0);
		this.FilterPanel.Name = "FilterPanel";
		this.FilterPanel.Size = new System.Drawing.Size(984, 65);
		this.FilterPanel.TabIndex = 0;
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartment);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(313, 65);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filter";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(245, 34);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(57, 26);
		this.btnshow.TabIndex = 125;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(7, 15);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(71, 15);
		this.label12.TabIndex = 124;
		this.label12.Text = "Department";
		this.cmbDepartment.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Location = new System.Drawing.Point(7, 34);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Name = "cmbDepartment";
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).TabIndex = 123;
		this.cmbDepartment.ValueChanged += new System.EventHandler(cmbDepartment_ValueChanged);
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.Color.White;
		this.lblCaption.Location = new System.Drawing.Point(0, 65);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(984, 27);
		this.lblCaption.TabIndex = 1;
		this.lblCaption.Text = "Caption";
		this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 92);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(984, 369);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(984, 461);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.lblCaption);
		base.Controls.Add(this.FilterPanel);
		base.Name = "AttendenceStatusByEmployeeAndDepartment";
		this.Text = "MonthlyAttendenceStatus";
		base.Load += new System.EventHandler(AttendenceStatusMonthly_Load);
		this.FilterPanel.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		base.ResumeLayout(false);
	}
}
