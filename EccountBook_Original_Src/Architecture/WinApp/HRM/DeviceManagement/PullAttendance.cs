using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.HRM.DeviceManagement;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.Model;
using Architecture.Model.HRM.DeviceManagement;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.HRM.DeviceManagement;

public class PullAttendance : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMood;

	private int Success;

	private int RecId;

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Panel panel4;

	internal GridEX grdAddress;

	private Button button1;

	private Panel panel2;

	private Label label1;

	public PullAttendance(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void AddressDetail_Load(object sender, EventArgs e)
	{
	}

	public void Reset()
	{
	}

	private void AddressDetail_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && !UpdateMood)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U && UpdateMood)
			{
				btnupdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtGrid = tblEmployeeAddressDetail.Getall(new tblEmployeeAddressDetail
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			grdAddress.DataSource = dtGrid;
			grdAddress.RetrieveStructure();
			grdAddress.RootTable.Columns["OrganizationId"].Visible = false;
			grdAddress.RootTable.Columns["CompanyId"].Visible = false;
			grdAddress.RootTable.Columns["Id"].Visible = false;
			grdAddress.RootTable.Columns["CountryId"].Visible = false;
			grdAddress.RootTable.Columns["CityId"].Visible = false;
			grdAddress.RootTable.Columns["AddressTypeId"].Visible = false;
			grdAddress.RootTable.Columns["ProvinceId"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RetrivedData()
	{
	}

	private void grdAddress_DoubleClick(object sender, EventArgs e)
	{
		RetrivedData();
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to PullAttendance?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				DataTable dt = hrmPullAttendance.Getall();
				if (dt.Rows.Count <= 0)
				{
					throw new Exception("Record Not Found For PullAttedance");
				}
				hrmPullAttendance obj = new hrmPullAttendance();
				obj.HrmPullAttendancesList = new List<hrmPullAttendance>();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					hrmPullAttendance hrmPull = new hrmPullAttendance();
					hrmPull.EmployeeNo = dt.Rows[i]["USERID"].ToString();
					hrmPull.CheckDateTime = Convert.ToDateTime(dt.Rows[i]["CHECKTIME"]);
					hrmPull.MachineDateTime = Convert.ToDateTime(dt.Rows[i]["CHECKTIME"]);
					hrmPull.CheckTime = Convert.ToDateTime(dt.Rows[i]["CHECKTIME"]);
					hrmPull.ActionTypeId = 1;
					hrmPull.AlteredById = UserAccount.ID;
					hrmPull.AlteredOn = DateTime.Now;
					hrmPull.CreatedById = UserAccount.ID;
					hrmPull.CreatedOn = DateTime.Now;
					obj.HrmPullAttendancesList.Add(hrmPull);
				}
				hrmPullAttendance.Save(obj);
				MessageBox.Show("Attendance Pull Successfully");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PullAttendance_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdAddress = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAddress).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(434, 561);
		this.panel1.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdAddress);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 80);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(434, 481);
		this.panel4.TabIndex = 2;
		this.grdAddress.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdAddress).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdAddress.FilterMode = (FilterMode)2;
		this.grdAddress.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdAddress).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAddress.GroupByBoxVisible = false;
		this.grdAddress.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdAddress).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdAddress).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdAddress).Name = "grdAddress";
		((ScrollableBase)this.grdAddress).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdAddress).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdAddress).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAddress).Size = new System.Drawing.Size(434, 481);
		((System.Windows.Forms.Control)(object)this.grdAddress).TabIndex = 4;
		this.grdAddress.TotalRow = (InheritableBoolean)1;
		this.grdAddress.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAddress).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdAddress).DoubleClick += new System.EventHandler(grdAddress_DoubleClick);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel2);
		this.panel3.Controls.Add(this.button1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(434, 80);
		this.panel3.TabIndex = 1;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(434, 36);
		this.panel2.TabIndex = 1;
		this.label1.Font = new System.Drawing.Font("Verdana", 8.55f);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(7, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(421, 32);
		this.label1.TabIndex = 0;
		this.label1.Text = "Click The Following Pull Attendance To Pull The Attandance From Zkt";
		this.button1.BackColor = System.Drawing.Color.Teal;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Location = new System.Drawing.Point(295, 42);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(132, 33);
		this.button1.TabIndex = 0;
		this.button1.Text = "Pull Attendance";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(434, 561);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PullAttendance";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "PullAttendance";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PullAttendance_FormClosing);
		base.Load += new System.EventHandler(AddressDetail_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AddressDetail_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAddress).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
