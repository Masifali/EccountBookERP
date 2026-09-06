using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Reconciliation;

public class AccountsReconciliation : Form
{
	private Attachment AT = new Attachment();

	public DataTable dtAttachmentsUpload = new DataTable();

	private int CompanyId;

	private int ChartofAccountId;

	private int Id;

	private DataTable dt = new DataTable();

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

	private ToolStripButton btnUpdate;

	private ToolStripButton Save;

	private ToolStripButton Print;

	private Label label23;

	private Panel panel4;

	private BackgroundWorker backgroundWorker1;

	private Panel panel6;

	internal GridEX grd;

	private Panel panel7;

	private Panel panel8;

	private ToolStrip toolStrip3;

	private ToolStripButton toolStripButton16;

	private ToolStripSeparator toolStripSeparator4;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX DataGridHistory;

	private Label lab;

	private TextBox txtDocNo;

	private CtrlGrdBar ctrlGrdBar1;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private Button btnSearch;

	internal UltraCombo CmbParentAccount;

	private Label label1;

	private ToolStripButton New;

	private Label label2;

	private DateTimePicker DocDate;

	private TextBox txtRemarks;

	private Label label4;

	private Label label3;

	private DateTimePicker UptoDate;

	private Label lbl;

	private ComboBox CmbReconcilationType;

	public AccountsReconciliation(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private bool FormValidation()
	{
		if (txtDocNo.Text.Trim() == string.Empty || txtDocNo.Text.Trim() == "0")
		{
			MessageBox.Show("Doc No Field is Required");
			txtDocNo.Focus();
			return false;
		}
		if (((Control)(object)CmbParentAccount).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Account Title Field is Required");
			CmbParentAccount.Focus();
			return false;
		}
		if (CmbReconcilationType.Text.Trim() == string.Empty || CmbReconcilationType.Text.Trim() == "0")
		{
			MessageBox.Show("Reconciliation Type Field is Required");
			CmbReconcilationType.Focus();
			return false;
		}
		return true;
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
				DDL.BindDDLNew(CoaList, CmbParentAccount, "Id", "AccountTitle", "Account Title", false);
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
		try
		{
			Id = 0;
			txtRemarks.Clear();
			((Control)(object)CmbParentAccount).Text = string.Empty;
			GenerateCode();
			grd.DataSource = null;
			Save.Visible = true;
			btnUpdate.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		try
		{
			int DocNo = AccountsReconciliationHeader.GenerateCode();
			if (DocNo > 0)
			{
				txtDocNo.Text = DocNo.ToString();
				return;
			}
			throw new Exception("Max Number not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DayBookVoucher_Load(object sender, EventArgs e)
	{
		try
		{
			dt.Columns.Add("AccountId");
			dt.Columns.Add("AccountCode");
			dt.Columns.Add("AccountTitle");
			dt.Columns.Add("PageNo");
			dt.Columns.Add("SystemDebit", typeof(double));
			dt.Columns.Add("SystemCredit", typeof(double));
			dt.Columns.Add("ManualBalance", typeof(double));
			dt.Columns.Add("Difference", typeof(double));
			DocDate.Focus();
			ParentAccountTitleFill();
			CmbReconcilationType.SelectedIndex = 0;
			GenerateCode();
			Save.Visible = true;
			btnUpdate.Visible = false;
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		try
		{
			DataTable lst = AccountsReconciliationHeader.AccountsReconciliationGetBalances(new ReportsParameters
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				GroupAccountId = Conversion.ToInt(CmbParentAccount.Value),
				FromDate = DocDate.Value,
				ToDate = DocDate.Value,
				Id = Id
			});
			if (lst.Rows.Count > 0)
			{
				dt.Rows.Clear();
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["AccountId"], lst.Rows[i]["AccountCode"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["GLPageNo"], Math.Round(Conversion.ToDouble(lst.Rows[i]["ClDebit"])), Math.Round(Conversion.ToDouble(lst.Rows[i]["ClCredit"])), Math.Round(Conversion.ToDouble(lst.Rows[i]["MannualBalance"])), Math.Round(Conversion.ToDouble(lst.Rows[i]["ClDebit"])) + Math.Round(Conversion.ToDouble(lst.Rows[i]["ClCredit"])) - Math.Abs(Math.Round(Conversion.ToDouble(lst.Rows[i]["MannualBalance"]))));
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
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
			grd.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grd.RootTable.Columns["PageNo"].EditType = (EditType)0;
			grd.RootTable.Columns["SystemDebit"].EditType = (EditType)0;
			grd.RootTable.Columns["SystemCredit"].EditType = (EditType)0;
			grd.RootTable.Columns["Difference"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountId"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PageNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["SystemDebit"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["SystemCredit"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ManualBalance"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Difference"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["SystemDebit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["SystemCredit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ManualBalance"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Difference"].AggregateFunction = (AggregateFunction)2;
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grd.LoadLayoutFile((Stream)fs);
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

	private void btnSearch_Click(object sender, EventArgs e)
	{
		DetailGridFill();
	}

	private void txtDocNo_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		int DocNo = AccountsReconciliationHeader.GetIdByDocNo(new AccountsReconciliationHeader
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId,
			DocNo = Conversion.ToInt((object)txtDocNo.Text.Trim())
		});
		if (DocNo > 0)
		{
			Id = DocNo;
			ReadById_Update(Id);
		}
	}

	public void ReadById_Update(int ID)
	{
		try
		{
			Id = Conversion.ToInt((object)ID);
			AccountsReconciliationHeader header = AccountsReconciliationHeader.ReadById(Id);
			DocDate.Value = header.DocDate;
			txtDocNo.Text = header.DocNo.ToString();
			UptoDate.Value = header.BalanceDate;
			txtRemarks.Text = header.RemarksHeader;
			CmbReconcilationType.Text = header.ReconciliationType;
			CmbParentAccount.Value = header.AccountId;
			Save.Visible = false;
			btnUpdate.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			AccountsReconciliationHeader vh = new AccountsReconciliationHeader();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				vh.Id = Id;
			}
			else
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GenerateCode();
			}
			if (grd.RowCount > 0)
			{
				vh.DocNo = int.Parse(txtDocNo.Text);
				vh.DocDate = DocDate.Value;
				vh.RemarksHeader = txtRemarks.Text.Trim();
				vh.BalanceDate = UptoDate.Value;
				vh.ReconciliationType = CmbReconcilationType.Text;
				vh.AccountId = Conversion.ToInt(CmbParentAccount.Value);
				vh.OrganizationId = UserAccount.OrganizationId;
				vh.CompanyId = UserAccount.CompanyId;
				vh.EntryUser = UserAccount.CompanyId;
				vh.ModifyUser = UserAccount.CompanyId;
				vh.AccountsReconiliationDetailList = new List<AccountsReconciliationDetail>();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					AccountsReconciliationDetail vd = new AccountsReconciliationDetail();
					vd.ChartOfAccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
					vd.ManualBalance = Conversion.ToSingle(r.Cells["ManualBalance"].Value);
					vd.BalanceDiff = Conversion.ToSingle(r.Cells["Difference"].Value);
					vh.AccountsReconiliationDetailList.Add(vd);
				}
				AccountsReconciliationHeader.Save(vh);
				if (Id > 0)
				{
					MessageBox.Show("Record Update Successfully...[" + vh.DocNo + "]");
				}
				else
				{
					MessageBox.Show("Record Save Successfully...[" + vh.DocNo + "]");
				}
				Reset();
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Save_Click(object sender, EventArgs e)
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

	private void btnUpdate_Click(object sender, EventArgs e)
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
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnSearch = new System.Windows.Forms.Button();
		this.CmbParentAccount = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.UptoDate = new System.Windows.Forms.DateTimePicker();
		this.lbl = new System.Windows.Forms.Label();
		this.CmbReconcilationType = new System.Windows.Forms.ComboBox();
		this.label2 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.lab = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Save = new System.Windows.Forms.ToolStripButton();
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
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccount).BeginInit();
		this.groupBox1.SuspendLayout();
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
		this.panel1.Size = new System.Drawing.Size(1206, 609);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1206, 609);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1198, 583);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 197);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1194, 384);
		this.panel6.TabIndex = 4;
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1194, 384);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 67);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1194, 130);
		this.panel4.TabIndex = 2;
		this.groupBox2.Controls.Add(this.btnSearch);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentAccount);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Location = new System.Drawing.Point(10, 77);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1178, 46);
		this.groupBox2.TabIndex = 40;
		this.groupBox2.TabStop = false;
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
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance45).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance49;
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
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.UptoDate);
		this.groupBox1.Controls.Add(this.lbl);
		this.groupBox1.Controls.Add(this.CmbReconcilationType);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtDocNo);
		this.groupBox1.Controls.Add(this.lab);
		this.groupBox1.Location = new System.Drawing.Point(10, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1178, 69);
		this.groupBox1.TabIndex = 39;
		this.groupBox1.TabStop = false;
		this.txtRemarks.BackColor = System.Drawing.Color.White;
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(619, 38);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(536, 23);
		this.txtRemarks.TabIndex = 4;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(616, 14);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(57, 15);
		this.label4.TabIndex = 40;
		this.label4.Text = "Remarks";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(444, 14);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(65, 15);
		this.label3.TabIndex = 38;
		this.label3.Text = "Up to Date";
		this.UptoDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.UptoDate.CustomFormat = "dd-MM-yy";
		this.UptoDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.UptoDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.UptoDate.Location = new System.Drawing.Point(447, 38);
		this.UptoDate.Name = "UptoDate";
		this.UptoDate.Size = new System.Drawing.Size(166, 22);
		this.UptoDate.TabIndex = 3;
		this.lbl.AutoSize = true;
		this.lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl.Location = new System.Drawing.Point(319, 14);
		this.lbl.Name = "lbl";
		this.lbl.Size = new System.Drawing.Size(111, 15);
		this.lbl.TabIndex = 36;
		this.lbl.Text = "ReconicilationType";
		this.CmbReconcilationType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbReconcilationType.FormattingEnabled = true;
		this.CmbReconcilationType.Items.AddRange(new object[4] { "General", "Bank", "Payables", "Receivables" });
		this.CmbReconcilationType.Location = new System.Drawing.Point(322, 37);
		this.CmbReconcilationType.Name = "CmbReconcilationType";
		this.CmbReconcilationType.Size = new System.Drawing.Size(121, 24);
		this.CmbReconcilationType.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(19, 14);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(55, 15);
		this.label2.TabIndex = 34;
		this.label2.Text = "DocDate";
		this.DocDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.CustomFormat = "dd-MM-yy";
		this.DocDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(22, 38);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(173, 22);
		this.DocDate.TabIndex = 0;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.txtDocNo.BackColor = System.Drawing.Color.White;
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(203, 38);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(113, 23);
		this.txtDocNo.TabIndex = 1;
		this.txtDocNo.Leave += new System.EventHandler(txtDocNo_Leave);
		this.lab.AutoSize = true;
		this.lab.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lab.Location = new System.Drawing.Point(200, 14);
		this.lab.Name = "lab";
		this.lab.Size = new System.Drawing.Size(48, 15);
		this.lab.TabIndex = 32;
		this.lab.Text = "Doc No";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 29);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1194, 38);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 8);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(212, 24);
		this.label23.TabIndex = 1;
		this.label23.Text = "Accounts Reconciliation";
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1194, 27);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1143, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.New, this.btnUpdate, this.Save, this.Print });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1194, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.New.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.New.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.New.Name = "New";
		this.New.Size = new System.Drawing.Size(55, 24);
		this.New.Text = "&New";
		this.New.Click += new System.EventHandler(New_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.Save.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Save.Name = "Save";
		this.Save.Size = new System.Drawing.Size(55, 24);
		this.Save.Text = "&Save";
		this.Save.Click += new System.EventHandler(Save_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(56, 24);
		this.Print.Text = "&Print";
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1198, 583);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 28);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1194, 553);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1194, 553);
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
		this.panel7.Size = new System.Drawing.Size(1194, 26);
		this.panel7.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1147, 0);
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
		this.toolStrip3.Size = new System.Drawing.Size(1194, 27);
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
		base.ClientSize = new System.Drawing.Size(1206, 609);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "AccountsReconciliation";
		this.Text = "Eccount Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(DayBookVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DayBookVoucher_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccount).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
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
