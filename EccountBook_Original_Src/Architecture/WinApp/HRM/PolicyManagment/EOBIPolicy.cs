using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.PolicyManagment;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.PolicyManagment;

public class EOBIPolicy : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMode;

	private DataTable dtdetail = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private DateTime DOB;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private Panel panel4;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar1;

	private TextBox txtemployeeshare;

	private Label label4;

	private TextBox txtagelimit;

	private Label label3;

	private DateTimePicker txtdateto;

	private DateTimePicker txtdatefrom;

	private Label label1;

	private Label label2;

	private TextBox txtcompanyshare;

	private TextBox txtpolicydescription;

	private Label label49;

	private Label label48;

	internal GridEX grdfrm;

	internal GridEX DataHistoryGrrid;

	private Button btnAddToEmployee;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel6;

	private Panel panel9;

	private Panel panel8;

	private Label label6;

	private Panel panel7;

	private Label label5;

	private Label label7;

	public EOBIPolicy(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void EOBIPolicy_Load(object sender, EventArgs e)
	{
		try
		{
			tabPage1.AutoScroll = true;
			tabPage1.AutoScrollMargin = new Size(20, 20);
			tabPage1.AutoScrollMinSize = new Size(980, 700);
			tabPage2.AutoScroll = true;
			tabPage2.AutoScrollMargin = new Size(20, 20);
			tabPage2.AutoScrollMinSize = new Size(980, 700);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtdetail.Columns.Add("EmployeeId");
			dtdetail.Columns.Add("EmployeeName");
			dtdetail.Columns.Add("DOB");
			dtdetail.Columns.Add("TotalSalary", typeof(double));
			dtdetail.Columns.Add("EmployeeShare", typeof(double));
			dtdetail.Columns.Add("CompanyShare", typeof(double));
			dtdetail.Columns.Add("TotalShare", typeof(double));
			grdfrm.DataSource = dtdetail;
			grdfrm.RetrieveStructure();
			grdEmployeeSetting();
			DetailGridFill();
			btnsave.Visible = true;
			btnupdate.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (txtpolicydescription.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Policy Description");
			txtpolicydescription.Focus();
			return false;
		}
		if (txtagelimit.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Age Limit");
			txtagelimit.Focus();
			return false;
		}
		if (txtemployeeshare.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Employee Share");
			txtemployeeshare.Focus();
			return false;
		}
		if (txtcompanyshare.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Company Share");
			txtcompanyshare.Focus();
			return false;
		}
		return true;
	}

	private void clear()
	{
		txtpolicydescription.Text = "";
		txtagelimit.Clear();
		txtcompanyshare.Clear();
		txtemployeeshare.Clear();
		btnupdate.Visible = false;
		UpdateMode = false;
		btnsave.Visible = true;
		btnAddToEmployee.Enabled = true;
		DetailGridFill();
		txtpolicydescription.Focus();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (txtdatefrom.Value > txtdateto.Value)
			{
				MessageBox.Show("Date From Is Greater Than Date To Please Check");
				txtdatefrom.Focus();
				return;
			}
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			hrmEOBIPolicy cat = new hrmEOBIPolicy();
			cat.EOBIPolicyId = RecId;
			cat.PolicyDescription = Conversion.ToString((object)txtpolicydescription.Text);
			cat.FromDate = Conversion.ToDateTime((object)txtdatefrom.Value);
			cat.ToDate = Conversion.ToDateTime((object)txtdateto.Value);
			cat.AgeLimit = Conversion.ToInt((object)txtagelimit.Text.Trim());
			cat.EmployeeShare = Conversion.ToDouble((object)txtemployeeshare.Text.Trim());
			cat.CompanyShare = Conversion.ToDouble((object)txtcompanyshare.Text.Trim());
			cat.OrganizationId = UserAccount.OrganizationId;
			cat.CompanyId = UserAccount.CompanyId;
			cat.AlteredOn = Conversion.ToDateTime((object)DateTime.Today);
			cat.AlteredById = Conversion.ToInt((object)UserAccount.ID);
			cat.CreatedOn = Conversion.ToDateTime((object)DateTime.Today);
			cat.CreatedById = Conversion.ToInt((object)UserAccount.ID);
			cat.ActionTypeId = Conversion.ToInt((object)1);
			cat.UserLogId = Conversion.ToInt((object)UserAccount.ID);
			cat.EOBIPolicyEmployeesList = new List<hrmEOBIPolicyEmployee>();
			GridEXRow[] rows = grdfrm.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToDouble(r.Cells["EmployeeShare"].Value) > 0.0)
				{
					hrmEOBIPolicyEmployee obj = new hrmEOBIPolicyEmployee();
					obj.EmployeeId = Conversion.ToInt(r.Cells["EmployeeId"].Value);
					obj.CompanyShare = Conversion.ToDecimal(r.Cells["CompanyShare"].Value);
					obj.EmployeeShare = Conversion.ToDecimal(r.Cells["EmployeeShare"].Value);
					cat.EOBIPolicyEmployeesList.Add(obj);
				}
			}
			hrmEOBIPolicy.Save(cat);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddToEmployee_Click(object sender, EventArgs e)
	{
		try
		{
			if (!formvalidation())
			{
				return;
			}
			grdfrm.UpdateData();
			GridEXRow[] rows = grdfrm.GetRows();
			foreach (GridEXRow r in rows)
			{
				int num = DateTime.Now.Year - Conversion.ToInt((object)Conversion.ToDateTime(r.Cells["DOB"].Value).Year);
				int AgeLimit = Conversion.ToInt((object)txtagelimit.Text);
				if (num > AgeLimit)
				{
					r.BeginEdit();
					r.Cells["EmployeeShare"].Value = Conversion.ToDouble((object)txtemployeeshare.Text);
					r.Cells["CompanyShare"].Value = Conversion.ToDouble((object)txtcompanyshare.Text);
					r.Cells["TotalShare"].Value = Conversion.ToInt(r.Cells["EmployeeShare"].Value) + Conversion.ToInt(r.Cells["CompanyShare"].Value);
					r.EndEdit();
				}
				else
				{
					r.BeginEdit();
					r.Cells["EmployeeShare"].Value = 0;
					r.Cells["CompanyShare"].Value = 0;
					r.Cells["TotalShare"].Value = Conversion.ToInt(r.Cells["EmployeeShare"].Value) + Conversion.ToInt(r.Cells["CompanyShare"].Value);
					r.EndEdit();
				}
			}
			grdfrm.UpdateData();
			grdEmployeeSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			genEmployeeHistory val = new genEmployeeHistory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			dtdetail.Rows.Clear();
			DataTable dt = genEmployeeHistory.GetAllEmployeesActiveFroEIOBPolicy(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtdetail.Rows.Add(dt.Rows[i]["EmployeeId"], dt.Rows[i]["EmployeeName"], Conversion.ToDateTime(dt.Rows[i]["DOB"]).ToString("dd-MMM-yyyy"), dt.Rows[i]["TotalSalary"], 0, 0, 0);
				}
				grdfrm.DataSource = dtdetail;
				grdfrm.RetrieveStructure();
				grdEmployeeSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeSetting()
	{
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Expected O, but got Unknown
		try
		{
			grdfrm.RootTable.Columns["EmployeeId"].Visible = false;
			grdfrm.RootTable.Columns["EmployeeName"].Width = 350;
			grdfrm.RootTable.Columns["TotalSalary"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["TotalSalary"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["TotalSalary"].FormatString = "0,0";
			grdfrm.RootTable.Columns["TotalSalary"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["EmployeeShare"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["EmployeeShare"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["EmployeeShare"].FormatString = "0,0";
			grdfrm.RootTable.Columns["EmployeeShare"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["CompanyShare"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["CompanyShare"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["CompanyShare"].FormatString = "0,0";
			grdfrm.RootTable.Columns["CompanyShare"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["TotalShare"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["TotalShare"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["TotalShare"].FormatString = "0,0";
			grdfrm.RootTable.Columns["TotalShare"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["DOB"].Width = 100;
			grdfrm.RootTable.Columns["CompanyShare"].Width = 120;
			grdfrm.RootTable.Columns["EmployeeShare"].Width = 120;
			grdfrm.RootTable.Columns["TotalShare"].Width = 120;
			new GridEXColumn();
			GridEXFormatCondition fc2 = new GridEXFormatCondition(grdfrm.RootTable.Columns["CompanyShare"], (ConditionOperator)2, (object)0);
			fc2.FormatStyle.ForeColor = Color.DarkRed;
			fc2.FormatStyle.FontBold = (TriState)2;
			grdfrm.RootTable.FormatConditions.Add(fc2);
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grdfrm.RootTable.Columns["EmployeeShare"], (ConditionOperator)2, (object)0);
			fc.FormatStyle.ForeColor = Color.DarkRed;
			fc.FormatStyle.FontBold = (TriState)2;
			grdfrm.RootTable.FormatConditions.Add(fc);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
	}

	private void gridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable getList = hrmEOBIPolicy.Getall(new hrmEOBIPolicy
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable table = new DataTable();
			table.Columns.Add("Id");
			table.Columns.Add("PolicyDescription");
			table.Columns.Add("FromDate");
			table.Columns.Add("ToDate");
			table.Columns.Add("AgeLimit");
			table.Columns.Add("CompanyShare");
			table.Columns.Add("EmployeeShare");
			for (int i = 0; i < getList.Rows.Count; i++)
			{
				table.Rows.Add(getList.Rows[i]["EOBIPolicyId"], getList.Rows[i]["PolicyDescription"], Conversion.ToDateTime(getList.Rows[i]["FromDate"]).ToShortDateString(), Conversion.ToDateTime(getList.Rows[i]["ToDate"]).ToShortDateString(), getList.Rows[i]["AgeLimit"], getList.Rows[i]["CompanyShare"], getList.Rows[i]["EmployeeShare"]);
			}
			DataHistoryGrrid.DataSource = table;
			DataHistoryGrrid.RetrieveStructure();
			GridHistorySetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistorySetting()
	{
		try
		{
			DataHistoryGrrid.RootTable.Columns["Id"].Visible = false;
			DataHistoryGrrid.RootTable.Columns["PolicyDescription"].Width = 220;
			DataHistoryGrrid.RootTable.Columns["FromDate"].Width = 100;
			DataHistoryGrrid.RootTable.Columns["ToDate"].Width = 100;
			DataHistoryGrrid.RootTable.Columns["AgeLimit"].Width = 100;
			DataHistoryGrrid.RootTable.Columns["CompanyShare"].Width = 120;
			DataHistoryGrrid.RootTable.Columns["EmployeeShare"].Width = 120;
			DataHistoryGrrid.RootTable.Columns.Add("Edit");
			DataHistoryGrrid.RootTable.Columns["Edit"].Key = "Edit";
			DataHistoryGrrid.RootTable.Columns["Edit"].Caption = "Edit";
			DataHistoryGrrid.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataHistoryGrrid.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataHistoryGrrid.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataHistoryGrrid.RootTable.Columns["Edit"].Width = 40;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				gridfill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataHistoryGrrid_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			hrmEOBIPolicy cat = new hrmEOBIPolicy();
			RecId = Conversion.ToInt((object)DataHistoryGrrid.CurrentRow.Cells["Id"].Value.ToString());
			cat = hrmEOBIPolicy.GetByID(RecId);
			txtpolicydescription.Text = Conversion.ToString((object)cat.PolicyDescription);
			txtdatefrom.Value = Conversion.ToDateTime((object)cat.FromDate);
			txtdateto.Value = Conversion.ToDateTime((object)cat.ToDate);
			txtagelimit.Text = Conversion.ToInt((object)cat.AgeLimit).ToString();
			txtemployeeshare.Text = Conversion.ToDouble((object)cat.EmployeeShare).ToString();
			txtcompanyshare.Text = Conversion.ToDouble((object)cat.CompanyShare).ToString();
			btnsave.Visible = false;
			UpdateMode = true;
			btnupdate.Visible = true;
			tabControl1.SelectedIndex = 0;
			btnAddToEmployee_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataHistoryGrrid_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				DataHistoryGrrid_DoubleClick(null, null);
			}
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataHistoryGrrid).Name);
			if (fs != null)
			{
				DataHistoryGrrid.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void EOBIPolicy_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
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
			if (e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnupdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtagelimit_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtemployeeshare_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcompanyshare_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
			{
				e.Handled = true;
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_1714: Unknown result type (might be due to invalid IL or missing references)
		//IL_171e: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.PolicyManagment.EOBIPolicy));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.btnAddToEmployee = new System.Windows.Forms.Button();
		this.txtemployeeshare = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtagelimit = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtdateto = new System.Windows.Forms.DateTimePicker();
		this.txtdatefrom = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtcompanyshare = new System.Windows.Forms.TextBox();
		this.txtpolicydescription = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.DataHistoryGrrid = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel8.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tsItemType.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataHistoryGrrid).BeginInit();
		this.panel5.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(710, 450);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(710, 450);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(702, 424);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.panel9);
		this.panel4.Controls.Add(this.panel8);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 201);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(696, 220);
		this.panel4.TabIndex = 0;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(696, 190);
		this.panel9.TabIndex = 25;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(696, 190);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 1;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar1);
		this.panel8.Controls.Add(this.label6);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(696, 30);
		this.panel8.TabIndex = 24;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(648, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(46, 27);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(6, 3);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(58, 19);
		this.label6.TabIndex = 24;
		this.label6.Text = "Detail";
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Controls.Add(this.btnAddToEmployee);
		this.panel3.Controls.Add(this.txtemployeeshare);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.txtagelimit);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.txtdateto);
		this.panel3.Controls.Add(this.txtdatefrom);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.txtcompanyshare);
		this.panel3.Controls.Add(this.txtpolicydescription);
		this.panel3.Controls.Add(this.label49);
		this.panel3.Controls.Add(this.label48);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 32);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(696, 169);
		this.panel3.TabIndex = 1;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label5);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(696, 30);
		this.panel7.TabIndex = 23;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(6, 3);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(117, 19);
		this.label5.TabIndex = 24;
		this.label5.Text = "E.O.B.I Policy";
		this.btnAddToEmployee.BackColor = System.Drawing.Color.Teal;
		this.btnAddToEmployee.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddToEmployee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddToEmployee.ForeColor = System.Drawing.Color.White;
		this.btnAddToEmployee.Location = new System.Drawing.Point(496, 119);
		this.btnAddToEmployee.Name = "btnAddToEmployee";
		this.btnAddToEmployee.Size = new System.Drawing.Size(100, 44);
		this.btnAddToEmployee.TabIndex = 22;
		this.btnAddToEmployee.Text = "Add To Employee";
		this.btnAddToEmployee.UseVisualStyleBackColor = false;
		this.btnAddToEmployee.Click += new System.EventHandler(btnAddToEmployee_Click);
		this.txtemployeeshare.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtemployeeshare.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtemployeeshare.Location = new System.Drawing.Point(427, 64);
		this.txtemployeeshare.Margin = new System.Windows.Forms.Padding(4);
		this.txtemployeeshare.Name = "txtemployeeshare";
		this.txtemployeeshare.Size = new System.Drawing.Size(169, 23);
		this.txtemployeeshare.TabIndex = 4;
		this.txtemployeeshare.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtemployeeshare_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(282, 68);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(147, 15);
		this.label4.TabIndex = 21;
		this.label4.Text = "Employee Share (Amount)";
		this.txtagelimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtagelimit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtagelimit.Location = new System.Drawing.Point(427, 37);
		this.txtagelimit.Margin = new System.Windows.Forms.Padding(4);
		this.txtagelimit.Name = "txtagelimit";
		this.txtagelimit.Size = new System.Drawing.Size(169, 23);
		this.txtagelimit.TabIndex = 3;
		this.txtagelimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtagelimit_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(282, 41);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 15);
		this.label3.TabIndex = 20;
		this.label3.Text = "Age (From)";
		this.txtdateto.CustomFormat = "dd-MMM-yy";
		this.txtdateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdateto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdateto.Location = new System.Drawing.Point(109, 92);
		this.txtdateto.Name = "txtdateto";
		this.txtdateto.Size = new System.Drawing.Size(169, 23);
		this.txtdateto.TabIndex = 2;
		this.txtdatefrom.CustomFormat = "dd-MMM-yy";
		this.txtdatefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdatefrom.Location = new System.Drawing.Point(109, 64);
		this.txtdatefrom.Name = "txtdatefrom";
		this.txtdatefrom.Size = new System.Drawing.Size(169, 23);
		this.txtdatefrom.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 68);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 19;
		this.label1.Text = "From Date";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 96);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 17;
		this.label2.Text = "To Date";
		this.txtcompanyshare.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcompanyshare.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcompanyshare.Location = new System.Drawing.Point(427, 92);
		this.txtcompanyshare.Margin = new System.Windows.Forms.Padding(4);
		this.txtcompanyshare.Name = "txtcompanyshare";
		this.txtcompanyshare.Size = new System.Drawing.Size(169, 23);
		this.txtcompanyshare.TabIndex = 5;
		this.txtcompanyshare.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcompanyshare_KeyPress);
		this.txtpolicydescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpolicydescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpolicydescription.Location = new System.Drawing.Point(109, 37);
		this.txtpolicydescription.Margin = new System.Windows.Forms.Padding(4);
		this.txtpolicydescription.Name = "txtpolicydescription";
		this.txtpolicydescription.Size = new System.Drawing.Size(169, 23);
		this.txtpolicydescription.TabIndex = 0;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(282, 96);
		this.label49.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(146, 15);
		this.label49.TabIndex = 13;
		this.label49.Text = "Company Share (Amount)";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(6, 41);
		this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(103, 15);
		this.label48.TabIndex = 11;
		this.label48.Text = "Policy Description";
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(696, 29);
		this.panel2.TabIndex = 0;
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnupdate, this.btnsave });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(696, 27);
		this.tsItemType.TabIndex = 2;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(79, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(65, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(702, 424);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.DataHistoryGrrid);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 31);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(696, 390);
		this.panel6.TabIndex = 4;
		this.DataHistoryGrrid.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.DataHistoryGrrid).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataHistoryGrrid.FilterMode = (FilterMode)2;
		this.DataHistoryGrrid.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataHistoryGrrid).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataHistoryGrrid.GroupTotals = (GroupTotals)2;
		this.DataHistoryGrrid.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.DataHistoryGrrid).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataHistoryGrrid).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.DataHistoryGrrid).Name = "DataHistoryGrrid";
		((ScrollableBase)this.DataHistoryGrrid).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.DataHistoryGrrid).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.DataHistoryGrrid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataHistoryGrrid).Size = new System.Drawing.Size(696, 390);
		((System.Windows.Forms.Control)(object)this.DataHistoryGrrid).TabIndex = 2;
		this.DataHistoryGrrid.TotalRow = (InheritableBoolean)1;
		this.DataHistoryGrrid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataHistoryGrrid).VisualStyle = (VisualStyle)4;
		this.DataHistoryGrrid.ColumnButtonClick += new ColumnActionEventHandler(DataHistoryGrrid_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataHistoryGrrid).DoubleClick += new System.EventHandler(DataHistoryGrrid_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label7);
		this.panel5.Controls.Add(this.ctrlGrdBar2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(696, 28);
		this.panel5.TabIndex = 3;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(6, 2);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 19);
		this.label7.TabIndex = 25;
		this.label7.Text = "History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(647, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.DataHistoryGrrid;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(44, 27);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(710, 450);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "EOBIPolicy";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(EOBIPolicy_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(EOBIPolicy_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataHistoryGrrid).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		base.ResumeLayout(false);
	}
}
