using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.HRM.DeviceManagement;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.DeviceManagement;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.HRM.DeviceManagement;

public class PullAttendanceByMachine : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMood;

	private int Success;

	private Rightsobjects formright = new Rightsobjects();

	private int RecId;

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private Panel panel3;

	private Panel panel4;

	private Button btnPullAttendance;

	private Panel panel7;

	private Panel panel8;

	private Panel panel5;

	private Panel panel6;

	private Label label23;

	private Label label1;

	private Button btnDownloadTemplate;

	internal GridEX grdMachineData;

	private Panel panel9;

	private Panel panel12;

	private Panel panel13;

	private Panel panel10;

	private Panel panel11;

	private Label label3;

	private Label label2;

	private Button button1;

	private Button button2;

	internal GridEX grdEmployeeList;

	public PullAttendanceByMachine(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void Reset()
	{
		try
		{
			EmployeeGridbind();
			MachineDataGridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void AddressDetail_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
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

	private void MachineDataGridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtMachineData = mmProductDevice.Getall(new mmProductDevice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtMachineData.Rows.Count > 0)
			{
				DataTable table = new DataTable();
				table.Columns.Add("Id");
				table.Columns.Add("LocationId");
				table.Columns.Add("BranchName");
				table.Columns.Add("DeviceName");
				table.Columns.Add("DeviceLocation");
				table.Columns.Add("IPAddress");
				table.Columns.Add("PortNo");
				table.Columns.Add("IsActive");
				table.Clear();
				for (int i = 0; i < dtMachineData.Rows.Count; i++)
				{
					table.Rows.Add(dtMachineData.Rows[i]["Id"], dtMachineData.Rows[i]["LocationId"], dtMachineData.Rows[i]["BranchName"], dtMachineData.Rows[i]["DeviceName"], dtMachineData.Rows[i]["DeviceLocation"], dtMachineData.Rows[i]["IpAddress"], dtMachineData.Rows[i]["PortNo"], dtMachineData.Rows[i]["IsActive"]);
				}
				grdMachineData.DataSource = table;
				grdMachineData.RetrieveStructure();
				gridMachineSetting();
			}
			else
			{
				grdMachineData.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridMachineSetting()
	{
		try
		{
			grdMachineData.RootTable.Columns["Id"].Visible = false;
			grdMachineData.RootTable.Columns["LocationId"].Visible = false;
			grdMachineData.RootTable.Columns["BranchName"].Width = 150;
			grdMachineData.RootTable.Columns["DeviceName"].Width = 120;
			grdMachineData.RootTable.Columns["DeviceLocation"].Width = 180;
			grdMachineData.RootTable.Columns["IpAddress"].Width = 100;
			grdMachineData.RootTable.Columns["PortNo"].Width = 70;
			grdMachineData.RootTable.Columns["IsActive"].Width = 70;
			grdMachineData.RootTable.Columns.Add("PingButton");
			grdMachineData.RootTable.Columns["PingButton"].Key = "PingButton";
			grdMachineData.RootTable.Columns["PingButton"].Caption = "Ping";
			grdMachineData.RootTable.Columns["PingButton"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdMachineData.RootTable.Columns["PingButton"].ButtonStyle = (ButtonStyle)5;
			grdMachineData.RootTable.Columns["PingButton"].ButtonText = "Ping";
			grdMachineData.RootTable.Columns["PingButton"].Width = 50;
			grdMachineData.RootTable.Columns.Add("AtPull");
			grdMachineData.RootTable.Columns["AtPull"].ActAsSelector = true;
			grdMachineData.RootTable.Columns["AtPull"].UseHeaderSelector = false;
			grdMachineData.RootTable.Columns["AtPull"].Width = 50;
			grdMachineData.RootTable.Columns["AtPull"].Position = 0;
			grdMachineData.RootTable.Columns["PingButton"].Position = 1;
			grdMachineData.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeGridbind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtEmployee = genEmployeeHistory.EmployeeLoadForDutyRoaster(new genEmployeeHistory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtEmployee.Rows.Count > 0)
			{
				DataTable Emptable = new DataTable();
				Emptable.Columns.Add("EmployeeHistoryId", typeof(int));
				Emptable.Columns.Add("EmployeeId", typeof(int));
				Emptable.Columns.Add("EmployeeNo", typeof(int));
				Emptable.Columns.Add("EmployeeName", typeof(string));
				Emptable.Columns.Add("Designation", typeof(string));
				Emptable.Columns.Add("Department", typeof(string));
				Emptable.Columns.Add("ShiftName", typeof(string));
				Emptable.Columns.Add("Category", typeof(string));
				Emptable.Columns.Add("Section", typeof(string));
				Emptable.Columns.Add("FingerTemplate", typeof(string));
				Emptable.Columns.Add("FaceTemplate", typeof(string));
				Emptable.Clear();
				for (int i = 0; i < dtEmployee.Rows.Count; i++)
				{
					Emptable.Rows.Add(dtEmployee.Rows[i]["EmployeeHistoryId"], dtEmployee.Rows[i]["EmployeeId"], dtEmployee.Rows[i]["EmployeeNo"], dtEmployee.Rows[i]["EmployeeName"], dtEmployee.Rows[i]["Designation"], dtEmployee.Rows[i]["Department"], dtEmployee.Rows[i]["ShiftName"], dtEmployee.Rows[i]["EmployeeCategory"], dtEmployee.Rows[i]["SectionName"], dtEmployee.Rows[i]["FingerTemplate"], dtEmployee.Rows[i]["FaceTemplate"]);
				}
				grdEmployeeList.DataSource = Emptable;
				grdEmployeeList.RetrieveStructure();
				grdSetting();
			}
			else
			{
				grdEmployeeList.DataSource = null;
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
			grdEmployeeList.RootTable.Columns["EmployeeHistoryId"].Visible = false;
			grdEmployeeList.RootTable.Columns["EmployeeId"].Visible = false;
			grdEmployeeList.RootTable.Columns["EmployeeName"].Width = 220;
			grdEmployeeList.RootTable.Columns.Add("Active");
			grdEmployeeList.RootTable.Columns["Active"].ActAsSelector = true;
			grdEmployeeList.RootTable.Columns["Active"].UseHeaderSelector = true;
			grdEmployeeList.RootTable.Columns["Active"].Width = 50;
			grdEmployeeList.RootTable.Columns["Active"].Position = 0;
			grdEmployeeList.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PullAttendanceByMachine_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 500);
			MachineDataGridBind();
			EmployeeGridbind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeProductDeviceData(string JobTask)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		try
		{
			if (grdMachineData.GetCheckedRows().Count() == 1)
			{
				mmProductDeviceJob obj = new mmProductDeviceJob();
				obj.MmProductDeviceJobsList = new List<mmProductDeviceJob>();
				GridEXRow[] checkedRows = grdMachineData.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					if (grdEmployeeList.GetCheckedRows().Length != 0)
					{
						GridEXRow[] checkedRows2 = grdEmployeeList.GetCheckedRows();
						foreach (GridEXRow r in checkedRows2)
						{
							mmProductDeviceJob job = new mmProductDeviceJob();
							job.ProductDeviceId = Conversion.ToInt(item.Cells[0].Value);
							job.EmployeeNo = Conversion.ToString(r.Cells["EmployeeNo"].Value);
							job.JobTask = JobTask;
							job.JobStatus = "Pending";
							job.JobPriority = true;
							job.BiometricTypeProfileId = 1;
							job.CreatedById = UserAccount.ID;
							job.AlteredById = UserAccount.ID;
							job.CreatedOn = DateTime.Now;
							job.AlteredOn = DateTime.Now;
							job.RequestFromDate = DateTime.Now;
							job.RequestToDate = DateTime.Now;
							job.ActionTypeId = 0;
							job.OrganizationId = UserAccount.OrganizationId;
							job.CompanyId = UserAccount.CompanyId;
							obj.MmProductDeviceJobsList.Add(job);
						}
					}
					else
					{
						mmProductDeviceJob job2 = new mmProductDeviceJob();
						job2.ProductDeviceId = Conversion.ToInt(item.Cells[0].Value);
						job2.JobTask = JobTask;
						job2.JobStatus = "Pending";
						job2.JobPriority = true;
						job2.BiometricTypeProfileId = 1;
						job2.CreatedById = UserAccount.ID;
						job2.AlteredById = UserAccount.ID;
						job2.CreatedOn = DateTime.Now;
						job2.AlteredOn = DateTime.Now;
						job2.RequestFromDate = DateTime.Now;
						job2.RequestToDate = DateTime.Now;
						job2.ActionTypeId = 0;
						job2.OrganizationId = UserAccount.OrganizationId;
						job2.CompanyId = UserAccount.CompanyId;
						obj.MmProductDeviceJobsList.Add(job2);
					}
				}
				mmProductDeviceJob.Save(obj);
				MessageBox.Show("Request successfuly submit");
				return;
			}
			throw new Exception("Only one row select");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPullAttendance_Click(object sender, EventArgs e)
	{
		try
		{
			MakeProductDeviceData("PullLog");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDownloadTemplate_Click(object sender, EventArgs e)
	{
		try
		{
			MakeProductDeviceData("Download");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		try
		{
			MakeProductDeviceData("Upload");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		try
		{
			MakeProductDeviceData("Delete");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PullAttendanceByMachine_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.button1 = new System.Windows.Forms.Button();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.button2 = new System.Windows.Forms.Button();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdEmployeeList = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.btnDownloadTemplate = new System.Windows.Forms.Button();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.btnPullAttendance = new System.Windows.Forms.Button();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdMachineData = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeList).BeginInit();
		this.panel11.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMachineData).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 461);
		this.panel1.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panel9);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 221);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(964, 240);
		this.panel4.TabIndex = 2;
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(964, 240);
		this.panel9.TabIndex = 5;
		this.panel12.Controls.Add(this.button1);
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Controls.Add(this.button2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel12.Location = new System.Drawing.Point(819, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(143, 240);
		this.panel12.TabIndex = 1;
		this.button1.BackColor = System.Drawing.Color.Crimson;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Location = new System.Drawing.Point(8, 91);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(125, 38);
		this.button1.TabIndex = 3;
		this.button1.Text = "Delete Template";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.panel13.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel13.Controls.Add(this.label3);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(143, 30);
		this.panel13.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(40, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(59, 17);
		this.label3.TabIndex = 3;
		this.label3.Text = "Actions";
		this.button2.BackColor = System.Drawing.Color.Peru;
		this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button2.ForeColor = System.Drawing.Color.White;
		this.button2.Location = new System.Drawing.Point(8, 47);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(125, 38);
		this.button2.TabIndex = 2;
		this.button2.Text = "Upload Template";
		this.button2.UseVisualStyleBackColor = false;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployeeList);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(819, 240);
		this.panel10.TabIndex = 0;
		this.grdEmployeeList.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdEmployeeList).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployeeList.FilterMode = (FilterMode)2;
		this.grdEmployeeList.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployeeList).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployeeList.GroupTotals = (GroupTotals)2;
		this.grdEmployeeList.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeList).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grdEmployeeList).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployeeList).Name = "grdEmployeeList";
		((ScrollableBase)this.grdEmployeeList).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployeeList).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployeeList).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeList).Size = new System.Drawing.Size(819, 212);
		((System.Windows.Forms.Control)(object)this.grdEmployeeList).TabIndex = 6;
		this.grdEmployeeList.TotalRow = (InheritableBoolean)1;
		this.grdEmployeeList.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployeeList).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel11.Controls.Add(this.label2);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(819, 28);
		this.panel11.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(12, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(122, 19);
		this.label2.TabIndex = 2;
		this.label2.Text = "Employee List";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 193);
		this.panel3.TabIndex = 1;
		this.panel7.Controls.Add(this.btnDownloadTemplate);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.btnPullAttendance);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(819, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(143, 193);
		this.panel7.TabIndex = 2;
		this.btnDownloadTemplate.BackColor = System.Drawing.Color.MediumSeaGreen;
		this.btnDownloadTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDownloadTemplate.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDownloadTemplate.ForeColor = System.Drawing.Color.White;
		this.btnDownloadTemplate.Location = new System.Drawing.Point(8, 88);
		this.btnDownloadTemplate.Name = "btnDownloadTemplate";
		this.btnDownloadTemplate.Size = new System.Drawing.Size(125, 34);
		this.btnDownloadTemplate.TabIndex = 1;
		this.btnDownloadTemplate.Text = "Download Template";
		this.btnDownloadTemplate.UseVisualStyleBackColor = false;
		this.btnDownloadTemplate.Click += new System.EventHandler(btnDownloadTemplate_Click);
		this.panel8.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel8.Controls.Add(this.label1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(143, 32);
		this.panel8.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(7, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(108, 17);
		this.label1.TabIndex = 2;
		this.label1.Text = "General Action";
		this.btnPullAttendance.BackColor = System.Drawing.Color.RoyalBlue;
		this.btnPullAttendance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPullAttendance.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPullAttendance.ForeColor = System.Drawing.Color.White;
		this.btnPullAttendance.Location = new System.Drawing.Point(8, 47);
		this.btnPullAttendance.Name = "btnPullAttendance";
		this.btnPullAttendance.Size = new System.Drawing.Size(125, 34);
		this.btnPullAttendance.TabIndex = 0;
		this.btnPullAttendance.Text = "Pull Attendance";
		this.btnPullAttendance.UseVisualStyleBackColor = false;
		this.btnPullAttendance.Click += new System.EventHandler(btnPullAttendance_Click);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdMachineData);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(819, 193);
		this.panel5.TabIndex = 1;
		this.grdMachineData.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdMachineData).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdMachineData.FilterMode = (FilterMode)2;
		this.grdMachineData.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdMachineData).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMachineData.GroupByBoxVisible = false;
		this.grdMachineData.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdMachineData).Location = new System.Drawing.Point(0, 32);
		((System.Windows.Forms.Control)(object)this.grdMachineData).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdMachineData).Name = "grdMachineData";
		((ScrollableBase)this.grdMachineData).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdMachineData).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdMachineData).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMachineData).Size = new System.Drawing.Size(819, 161);
		((System.Windows.Forms.Control)(object)this.grdMachineData).TabIndex = 5;
		this.grdMachineData.TotalRow = (InheritableBoolean)1;
		this.grdMachineData.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMachineData).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(819, 32);
		this.panel6.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(173, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Data Syncronization";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnnew });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(964, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 461);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PullAttendanceByMachine";
		this.Text = "PullAttendance";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PullAttendanceByMachine_FormClosing);
		base.Load += new System.EventHandler(PullAttendanceByMachine_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AddressDetail_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeList).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdMachineData).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
