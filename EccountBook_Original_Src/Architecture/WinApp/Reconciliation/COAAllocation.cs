using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Reconciliation;

public class COAAllocation : Form
{
	private Attachment AT = new Attachment();

	public DataTable dtAttachmentsUpload = new DataTable();

	private int CompanyId;

	private int ChartofAccountId;

	private DataTable dtHistory = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip2;

	private ToolStripButton Print;

	private Label label23;

	private Panel panel4;

	private BackgroundWorker backgroundWorker1;

	private Panel panel6;

	internal GridEX grddetail;

	private Panel panel7;

	private Panel panel8;

	private ToolStrip toolStrip3;

	private ToolStripButton toolStripButton16;

	private ToolStripSeparator toolStripSeparator4;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX DataGridHistory;

	private Button btnAccountUpdate;

	internal UltraCombo CmbAccountTitle;

	private Label label11;

	private Label label9;

	private TextBox txtPageNo;

	private CtrlGrdBar ctrlGrdBar1;

	private GroupBox groupBox1;

	private CheckBox IsActive;

	private GroupBox groupBox2;

	private Button btnParentUpdate;

	private Button btnSearch;

	internal UltraCombo CmbParentAccount;

	private Label label1;

	private ToolStripButton New;

	public COAAllocation(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void AccountTitleFill()
	{
		try
		{
			DataTable dt = CommonServices.GetAllDetailAccount();
			if (dt.Rows.Count > 0 && dt != null)
			{
				DDL.BindDDL(dt, CmbAccountTitle, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParentAccountTitleFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		try
		{
			DataTable CoaList = ChartofAccount.ReadAllAccountgroup(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Account_Level = 3,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (CoaList.Rows.Count > 0)
			{
				DDL.BindDDLNew(CoaList, CmbParentAccount, "AccountCode", "AccountTitle", "Account Title", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void Reset()
	{
		txtPageNo.Clear();
		grddetail.DataSource = null;
		CmbAccountTitle.Focus();
	}

	private void DayBookVoucher_Load(object sender, EventArgs e)
	{
		CmbAccountTitle.Focus();
		dtHistory.Columns.Add("Id");
		dtHistory.Columns.Add("AccountCode");
		dtHistory.Columns.Add("AccountTitle");
		dtHistory.Columns.Add("PageNo");
		dtHistory.Columns.Add("CompanyId");
		ParentAccountTitleFill();
		AccountTitleFill();
	}

	private void DetailGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable lst = COAAllocation.ReadAllForAllocation(new ChartofAccount
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ParentAccountCode = CmbParentAccount.Value.ToString()
			});
			if (lst.Rows.Count > 0 && lst != null)
			{
				grddetail.DataSource = lst;
				grddetail.RetrieveStructure();
				DetialGridSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetialGridSettings()
	{
		try
		{
			grddetail.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grddetail.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grddetail.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grddetail.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grddetail.RootTable.Columns["GLPageNo"].FilterEditType = (FilterEditType)1;
			grddetail.RootTable.Columns["Id"].Visible = false;
			grddetail.RootTable.Columns["OrganizationId"].Visible = false;
			grddetail.RootTable.Columns["CompanyId"].Visible = false;
			grddetail.RootTable.Columns["ChartofAccountId"].Visible = false;
			grddetail.RootTable.Columns["ParentAccountCode"].Visible = false;
			grddetail.RootTable.Columns["IsActive"].Visible = false;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grddetail).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grddetail).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grddetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void New_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void DayBookVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
		{
			New_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		DetailGridFill();
	}

	private void Print_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Reporting rpt = new Reporting();
			if (UserAccount.CompAddress.ToString() == string.Empty)
			{
				rpt.RptPerameter("CompanyAddress", "");
			}
			else
			{
				rpt.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			}
			if (UserAccount.CompName.ToString() == string.Empty)
			{
				rpt.RptPerameter("CompanyName", (string)null);
			}
			else
			{
				rpt.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			}
			rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
			rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
			rpt.RptPerameter("@FinancialYearId", clsGlobalVariables.ActiveYr.Id.ToString());
			rpt.RptPerameter("@VoucherDateF", string.Empty);
			rpt.RptPerameter("@VoucherDateT", string.Empty);
			rpt.RptPerameter("@Id", grddetail.CurrentRow.Cells[0].Value.ToString());
			rpt.RptPerameter("@VoucherCodeF", "0");
			rpt.RptPerameter("@VoucherCodeT", "0");
			rpt.RptPerameter("@DocumentTypeId", "8");
			rpt.RptPerameter("@BranchesId", "0");
			rpt.RptPerameter("@ProjectsId", "0");
			rpt.ShowReport("AcRptVoucherSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		DetailGridFill();
	}

	private void CmbAccountTitle_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable lst = COAAllocation.GetPageNoandStatusbyAccountId(new COAAllocation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(CmbAccountTitle.Value)
			});
			if (lst.Rows.Count > 0)
			{
				txtPageNo.Text = lst.Rows[0]["GLPageNo"].ToString();
				if (Convert.ToBoolean(lst.Rows[0]["IsActive"]))
				{
					IsActive.Checked = true;
				}
				else
				{
					IsActive.Checked = false;
				}
				CompanyId = Convert.ToInt32(lst.Rows[0]["CompanyId"]);
				ChartofAccountId = Convert.ToInt32(lst.Rows[0]["ChartofAccountId"]);
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void btnParentUpdate_Click(object sender, EventArgs e)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				GridEXRow[] rows = grddetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					COAAllocation.UpdatePageNo(new COAAllocation
					{
						ChartofAccountId = int.Parse(r.Cells["Id"].Value.ToString()),
						CompanyId = int.Parse(r.Cells["CompanyId"].Value.ToString()),
						GLPageNo = r.Cells["PageNo"].Value.ToString(),
						IsActive = true,
						BranchId = UserAccount.ID
					});
				}
				MessageBox.Show("Record Update Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAccountUpdate_Click(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				COAAllocation coaa = new COAAllocation();
				coaa.ChartofAccountId = ChartofAccountId;
				coaa.CompanyId = CompanyId;
				coaa.GLPageNo = txtPageNo.Text;
				coaa.BranchId = UserAccount.ID;
				if (IsActive.Checked)
				{
					coaa.IsActive = true;
				}
				else
				{
					coaa.IsActive = false;
				}
				COAAllocation.UpdatePageNo(coaa);
				MessageBox.Show("Record Update Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void COAAllocation_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		GridEXLayout grddetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grddetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnParentUpdate = new System.Windows.Forms.Button();
		this.btnSearch = new System.Windows.Forms.Button();
		this.CmbParentAccount = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.IsActive = new System.Windows.Forms.CheckBox();
		this.btnAccountUpdate = new System.Windows.Forms.Button();
		this.CmbAccountTitle = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.txtPageNo = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton16 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetail).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccount).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(734, 451);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(734, 451);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(726, 425);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grddetail);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 173);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(722, 250);
		this.panel6.TabIndex = 4;
		this.grddetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grddetail).BackColor = System.Drawing.Color.White;
		grddetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetail.DesignTimeLayout = grddetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetail.DynamicFiltering = true;
		this.grddetail.FilterMode = (FilterMode)2;
		this.grddetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grddetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grddetail).Name = "grddetail";
		((ScrollableBase)this.grddetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetail).Size = new System.Drawing.Size(722, 250);
		((System.Windows.Forms.Control)(object)this.grddetail).TabIndex = 2;
		this.grddetail.TotalRow = (InheritableBoolean)1;
		this.grddetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetail).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 67);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(722, 106);
		this.panel4.TabIndex = 2;
		this.groupBox2.Controls.Add(this.btnParentUpdate);
		this.groupBox2.Controls.Add(this.btnSearch);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentAccount);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Location = new System.Drawing.Point(10, 54);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(702, 46);
		this.groupBox2.TabIndex = 40;
		this.groupBox2.TabStop = false;
		this.btnParentUpdate.BackColor = System.Drawing.Color.White;
		this.btnParentUpdate.FlatAppearance.BorderSize = 0;
		this.btnParentUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnParentUpdate.Location = new System.Drawing.Point(446, 12);
		this.btnParentUpdate.Name = "btnParentUpdate";
		this.btnParentUpdate.Size = new System.Drawing.Size(64, 26);
		this.btnParentUpdate.TabIndex = 2;
		this.btnParentUpdate.Text = "Update";
		this.btnParentUpdate.UseVisualStyleBackColor = false;
		this.btnParentUpdate.Click += new System.EventHandler(btnParentUpdate_Click);
		this.btnSearch.BackColor = System.Drawing.Color.White;
		this.btnSearch.FlatAppearance.BorderSize = 0;
		this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSearch.Location = new System.Drawing.Point(364, 12);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(64, 26);
		this.btnSearch.TabIndex = 1;
		this.btnSearch.Text = "Search";
		this.btnSearch.UseVisualStyleBackColor = false;
		this.btnSearch.Click += new System.EventHandler(btnSearch_Click);
		this.CmbParentAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbParentAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance16).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Location = new System.Drawing.Point(98, 12);
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Name = "CmbParentAccount";
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(8, 18);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(81, 13);
		this.label1.TabIndex = 23;
		this.label1.Text = "Parent Account";
		this.groupBox1.Controls.Add(this.IsActive);
		this.groupBox1.Controls.Add(this.btnAccountUpdate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitle);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtPageNo);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Location = new System.Drawing.Point(10, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(702, 49);
		this.groupBox1.TabIndex = 39;
		this.groupBox1.TabStop = false;
		this.IsActive.AutoSize = true;
		this.IsActive.Location = new System.Drawing.Point(537, 19);
		this.IsActive.Name = "IsActive";
		this.IsActive.Size = new System.Drawing.Size(67, 17);
		this.IsActive.TabIndex = 2;
		this.IsActive.Text = "Is Active";
		this.IsActive.UseVisualStyleBackColor = true;
		this.btnAccountUpdate.BackColor = System.Drawing.Color.White;
		this.btnAccountUpdate.FlatAppearance.BorderSize = 0;
		this.btnAccountUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAccountUpdate.Location = new System.Drawing.Point(623, 15);
		this.btnAccountUpdate.Name = "btnAccountUpdate";
		this.btnAccountUpdate.Size = new System.Drawing.Size(64, 26);
		this.btnAccountUpdate.TabIndex = 3;
		this.btnAccountUpdate.Text = "Update";
		this.btnAccountUpdate.UseVisualStyleBackColor = false;
		this.btnAccountUpdate.Click += new System.EventHandler(btnAccountUpdate_Click);
		this.CmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Location = new System.Drawing.Point(98, 13);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Name = "CmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Leave += new System.EventHandler(CmbAccountTitle_Leave);
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(8, 19);
		this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(70, 13);
		this.label11.TabIndex = 23;
		this.label11.Text = "Account Title";
		this.txtPageNo.BackColor = System.Drawing.Color.White;
		this.txtPageNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPageNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPageNo.Location = new System.Drawing.Point(413, 15);
		this.txtPageNo.Name = "txtPageNo";
		this.txtPageNo.Size = new System.Drawing.Size(113, 23);
		this.txtPageNo.TabIndex = 1;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(350, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(55, 15);
		this.label9.TabIndex = 32;
		this.label9.Text = "Page No";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 29);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(722, 38);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 8);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(138, 24);
		this.label23.TabIndex = 1;
		this.label23.Text = "COA Allocation";
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(722, 27);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(671, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grddetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.New, this.Print });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(722, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.New.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.New.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.New.Name = "New";
		this.New.Size = new System.Drawing.Size(55, 24);
		this.New.Text = "&New";
		this.New.Click += new System.EventHandler(New_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(56, 24);
		this.Print.Text = "&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(726, 425);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 28);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(722, 395);
		this.panel8.TabIndex = 1;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(722, 395);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.toolStrip3);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(722, 26);
		this.panel7.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(675, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(42, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripButton16, this.toolStripSeparator4 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(722, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripButton16.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton16.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton16.Name = "toolStripButton16";
		this.toolStripButton16.Size = new System.Drawing.Size(56, 24);
		this.toolStripButton16.Text = "&Print";
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(734, 451);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "COAAllocation";
		this.Text = "Eccount Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(COAAllocation_FormClosing);
		base.Load += new System.EventHandler(DayBookVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DayBookVoucher_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grddetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccount).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
