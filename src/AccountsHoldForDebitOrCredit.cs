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
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
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
namespace Architecture.WinApp.Account_Definition
{

public class AccountsHoldForDebitOrCredit : Form
{
	private UserAccount userAccount = new UserAccount();

	public int RecId;

	private DropDownBind DDL = new DropDownBind();

	private int[] AryColWidth;

	private IContainer components;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private Panel panel1;

	private CheckBox checkBoxCredit;

	private CheckBox checkBoxDebit;

	internal UltraCombo CmbAccountTitle;

	private Panel panel2;

	private Label label6;

	private CtrlGrdBar ctrlGrdBar;

	private Button btnCancel;

	private Button btnSavef;

	private Button btnUpdatef;

	internal GridEX grdHistory;

	private Label label2;

	private TextBox txtCreditRemarks;

	private TextBox txtDebitRemarks;

	private Label label3;

	private Label label1;

	private Panel panel3;

	private Label label4;

	public AccountsHoldForDebitOrCredit(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		userAccount = user;
	}

	private void AccountsHoldForDebitOrCredit_Load(object sender, EventArgs e)
	{
		GridHistoryfill();
		AccountTitleCombo();
		btnSave.Visible = true;
		btnSavef.Visible = true;
		btnUpdate.Visible = false;
		btnUpdatef.Visible = false;
		CmbAccountTitle.Focus();
		txtDebitRemarks.Enabled = false;
		txtCreditRemarks.Enabled = false;
	}

	private void Insert()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		AccountsHoldForDebitOrCredit obj = new AccountsHoldForDebitOrCredit();
		if (!Formvalidation())
		{
			return;
		}
		if (RecId > 0)
		{
			if (MessageBox.Show("Are you sure to Update Record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.Id = RecId;
		}
		else if (MessageBox.Show("Are you sure to Save Record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
		{
			return;
		}
		obj.OrganizationId = userAccount.OrganizationId;
		obj.BranchesId = userAccount.BranchesId;
		obj.CompanyId = userAccount.CompanyId;
		obj.CreatedById = userAccount.ID;
		obj.ModifyUserId = userAccount.ID;
		obj.CreatedDate = DateTime.Now;
		obj.ModifyDate = DateTime.Now;
		obj.CoaAccountId = Conversion.ToInt(CmbAccountTitle.Value);
		obj.CreditHoldStatus = checkBoxCredit.Checked;
		obj.DebitHoldStatus = checkBoxDebit.Checked;
		obj.CreditHoldReason = txtCreditRemarks.Text;
		obj.DebitHoldReason = txtDebitRemarks.Text;
		AccountsHoldForDebitOrCredit.Save(obj);
		if (RecId > 0)
		{
			MessageBox.Show("Record Updated Successfully!");
		}
		else
		{
			MessageBox.Show("Record Saved Successfully!");
		}
		btnUpdatef.Visible = false;
		btnUpdate.Visible = false;
		btnSave.Visible = true;
		btnSavef.Visible = true;
		Reset();
	}

	private bool Formvalidation()
	{
		if (((Control)(object)CmbAccountTitle).Text == "")
		{
			MessageBox.Show("Please enter a value for 'Account Title'.");
			CmbAccountTitle.Focus();
			return false;
		}
		if ((txtDebitRemarks.Text == "" || txtDebitRemarks.Text == null) && checkBoxDebit.Checked)
		{
			MessageBox.Show("Remarks / Reason Required For Holding Debit Entry.");
			txtDebitRemarks.Focus();
			return false;
		}
		if ((txtCreditRemarks.Text == "" || txtCreditRemarks.Text == null) && checkBoxCredit.Checked)
		{
			MessageBox.Show("Remarks / Reason Required For Holding Credit Entry.");
			txtCreditRemarks.Focus();
			return false;
		}
		return true;
	}

	private void GridHistoryfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable lst = AccountsHoldForDebitOrCredit.Formhistory(new ReportsParameters
			{
				OrganizationId = userAccount.OrganizationId,
				CompanyId = userAccount.CompanyId,
				BranchesId = userAccount.BranchesId
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("CoaAccountId", typeof(int));
				dt.Columns.Add("AccountTitle", typeof(string));
				dt.Columns.Add("CreditHoldStatus", typeof(string));
				dt.Columns.Add("CreditHoldReason", typeof(string));
				dt.Columns.Add("DebitHoldStatus", typeof(string));
				dt.Columns.Add("DebitHoldReason", typeof(string));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(string));
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(lst.Rows[i]["Id"]), Conversion.ToInt(lst.Rows[i]["CoaAccountId"]), Conversion.ToString(lst.Rows[i]["AccountTitle"]), Conversion.ToString(lst.Rows[i]["CreditHoldStatus"]), Conversion.ToString(lst.Rows[i]["CreditHoldReason"]), Conversion.ToString(lst.Rows[i]["DebitHoldStatus"]), Conversion.ToString(lst.Rows[i]["DebitHoldReason"]), Conversion.ToString(lst.Rows[i]["EntryUser"]), Conversion.ToDateTime(lst.Rows[i]["CreatedDate"]).ToShortDateString(), Conversion.ToString(lst.Rows[i]["ModifyUser"]), Conversion.ToDateTime(lst.Rows[i]["ModifyDate"]).ToShortDateString());
				}
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				GridSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		grdHistory.RootTable.Columns["Id"].Visible = false;
		grdHistory.RootTable.Columns["CoaAccountId"].Visible = false;
		CommonServices.GridAutoAdjustment(AryColWidth, grdHistory);
	}

	private void Reset()
	{
		((Control)(object)CmbAccountTitle).Text = string.Empty;
		txtCreditRemarks.Text = string.Empty;
		txtDebitRemarks.Text = string.Empty;
		btnSave.Visible = true;
		btnSavef.Visible = true;
		btnUpdate.Visible = false;
		btnUpdatef.Visible = false;
		txtDebitRemarks.Enabled = false;
		txtCreditRemarks.Enabled = false;
		checkBoxDebit.Checked = false;
		checkBoxCredit.Checked = false;
		GridHistoryfill();
		CmbAccountTitle.Focus();
	}

	private void ctrlGrdBar_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnn(object value1, object value2)
	{
		throw new NotImplementedException();
	}

	private void btnNew_Click(object sender, EventArgs e)
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

	private void btnSave_Click(object sender, EventArgs e)
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

	private void btnSavef_Click(object sender, EventArgs e)
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

	private void btnUpdatef_Click(object sender, EventArgs e)
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

	private void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		try
		{
			RecId = Id;
			AccountsHoldForDebitOrCredit obj = AccountsHoldForDebitOrCredit.GetById(RecId);
			if (obj != null)
			{
				CmbAccountTitle.Value = Conversion.ToInt((object)obj.CoaAccountId);
				checkBoxCredit.Checked = Conversion.ToBool((object)obj.CreditHoldStatus);
				checkBoxCredit_CheckedChanged(null, null);
				checkBoxDebit.Checked = Conversion.ToBool((object)obj.DebitHoldStatus);
				checkBoxDebit_CheckedChanged(null, null);
				txtCreditRemarks.Text = Conversion.ToString((object)obj.CreditHoldReason);
				txtDebitRemarks.Text = Conversion.ToString((object)obj.DebitHoldReason);
				btnUpdate.Visible = true;
				btnSave.Visible = false;
				btnUpdatef.Visible = true;
				btnSavef.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				ReadById(Conversion.ToInt(item.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTitleCombo()
	{
		try
		{
			DataTable dtAcc = CommonServices.CoaAllocationGetAllServiceBind();
			DDL.BindDDLNew(dtAcc, CmbAccountTitle, "Id", "AccountTitle", "Account Title", false);
			((UltraGridBase)CmbAccountTitle).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsHoldForDebitOrCredit_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnSave_Click(null, null);
			}
			if (btnUpdatef.Visible && btnUpdatef.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdatef_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnn(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void checkBoxDebit_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (checkBoxDebit.Checked)
			{
				txtDebitRemarks.Enabled = true;
				return;
			}
			txtDebitRemarks.Enabled = false;
			txtDebitRemarks.Text = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void checkBoxCredit_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (checkBoxCredit.Checked)
			{
				txtCreditRemarks.Enabled = true;
				return;
			}
			txtCreditRemarks.Enabled = false;
			txtCreditRemarks.Text = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsHoldForDebitOrCredit_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.txtCreditRemarks = new System.Windows.Forms.TextBox();
		this.txtDebitRemarks = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnSavef = new System.Windows.Forms.Button();
		this.btnUpdatef = new System.Windows.Forms.Button();
		this.checkBoxCredit = new System.Windows.Forms.CheckBox();
		this.checkBoxDebit = new System.Windows.Forms.CheckBox();
		this.CmbAccountTitle = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.ctrlGrdBar = new grid.User_Conrtols.CtrlGrdBar();
		this.grdHistory = new GridEX();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		base.SuspendLayout();
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.btnSave, this.btnUpdate });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(436, 25);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.txtCreditRemarks);
		this.panel1.Controls.Add(this.txtDebitRemarks);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.btnCancel);
		this.panel1.Controls.Add(this.btnSavef);
		this.panel1.Controls.Add(this.btnUpdatef);
		this.panel1.Controls.Add(this.checkBoxCredit);
		this.panel1.Controls.Add(this.checkBoxDebit);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitle);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 25);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(436, 261);
		this.panel1.TabIndex = 2;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(436, 28);
		this.panel3.TabIndex = 622;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(4, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(273, 19);
		this.label4.TabIndex = 26;
		this.label4.Text = "Accounts Hold For Debit / Credit";
		this.txtCreditRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCreditRemarks.Location = new System.Drawing.Point(12, 179);
		this.txtCreditRemarks.Multiline = true;
		this.txtCreditRemarks.Name = "txtCreditRemarks";
		this.txtCreditRemarks.Size = new System.Drawing.Size(412, 42);
		this.txtCreditRemarks.TabIndex = 621;
		this.txtDebitRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDebitRemarks.Location = new System.Drawing.Point(12, 114);
		this.txtDebitRemarks.Multiline = true;
		this.txtDebitRemarks.Name = "txtDebitRemarks";
		this.txtDebitRemarks.Size = new System.Drawing.Size(412, 42);
		this.txtDebitRemarks.TabIndex = 621;
		this.label3.BackColor = System.Drawing.Color.Gainsboro;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(12, 159);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(412, 17);
		this.label3.TabIndex = 620;
		this.label3.Text = "Credit Remarks";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.BackColor = System.Drawing.Color.Gainsboro;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(12, 94);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(412, 17);
		this.label1.TabIndex = 620;
		this.label1.Text = "Debit Remarks";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.BackColor = System.Drawing.Color.Gainsboro;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(12, 39);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(233, 17);
		this.label2.TabIndex = 620;
		this.label2.Text = "Account Title";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.ForeColor = System.Drawing.Color.White;
		this.btnCancel.Location = new System.Drawing.Point(269, 227);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(75, 28);
		this.btnCancel.TabIndex = 618;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnSavef.BackColor = System.Drawing.Color.Teal;
		this.btnSavef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSavef.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSavef.ForeColor = System.Drawing.Color.White;
		this.btnSavef.Location = new System.Drawing.Point(349, 227);
		this.btnSavef.Name = "btnSavef";
		this.btnSavef.Size = new System.Drawing.Size(75, 28);
		this.btnSavef.TabIndex = 617;
		this.btnSavef.Text = "Save";
		this.btnSavef.UseVisualStyleBackColor = false;
		this.btnSavef.Click += new System.EventHandler(btnSavef_Click);
		this.btnUpdatef.BackColor = System.Drawing.Color.Teal;
		this.btnUpdatef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdatef.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdatef.ForeColor = System.Drawing.Color.White;
		this.btnUpdatef.Location = new System.Drawing.Point(348, 227);
		this.btnUpdatef.Name = "btnUpdatef";
		this.btnUpdatef.Size = new System.Drawing.Size(75, 28);
		this.btnUpdatef.TabIndex = 619;
		this.btnUpdatef.Text = "Update";
		this.btnUpdatef.UseVisualStyleBackColor = false;
		this.btnUpdatef.Visible = false;
		this.btnUpdatef.Click += new System.EventHandler(btnUpdatef_Click);
		this.checkBoxCredit.AutoSize = true;
		this.checkBoxCredit.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkBoxCredit.Location = new System.Drawing.Point(269, 66);
		this.checkBoxCredit.Name = "checkBoxCredit";
		this.checkBoxCredit.Size = new System.Drawing.Size(96, 21);
		this.checkBoxCredit.TabIndex = 6;
		this.checkBoxCredit.Text = "Credit Hold";
		this.checkBoxCredit.UseVisualStyleBackColor = true;
		this.checkBoxCredit.CheckedChanged += new System.EventHandler(checkBoxCredit_CheckedChanged);
		this.checkBoxDebit.AutoSize = true;
		this.checkBoxDebit.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkBoxDebit.Location = new System.Drawing.Point(269, 39);
		this.checkBoxDebit.Name = "checkBoxDebit";
		this.checkBoxDebit.Size = new System.Drawing.Size(92, 21);
		this.checkBoxDebit.TabIndex = 5;
		this.checkBoxDebit.Text = "Debit Hold";
		this.checkBoxDebit.UseVisualStyleBackColor = true;
		this.checkBoxDebit.CheckedChanged += new System.EventHandler(checkBoxDebit_CheckedChanged);
		this.CmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Location = new System.Drawing.Point(12, 58);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Name = "CmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Size = new System.Drawing.Size(233, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).TabIndex = 4;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.ctrlGrdBar);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 286);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(436, 28);
		this.panel2.TabIndex = 3;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(4, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 19);
		this.label6.TabIndex = 26;
		this.label6.Text = "History";
		this.ctrlGrdBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar.FormName = null;
		this.ctrlGrdBar.Location = new System.Drawing.Point(393, 0);
		this.ctrlGrdBar.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar.MyGrid = null;
		this.ctrlGrdBar.Name = "ctrlGrdBar";
		this.ctrlGrdBar.Size = new System.Drawing.Size(43, 28);
		this.ctrlGrdBar.TabIndex = 27;
		this.ctrlGrdBar.Load += new System.EventHandler(ctrlGrdBar_Load);
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 314);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		this.grdHistory.SettingsKey = "grdWagesExemptItemSchedule";
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(436, 279);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 11;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(436, 593);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.toolStrip1);
		base.Name = "AccountsHoldForDebitOrCredit";
		this.Text = "AccountsHoldForDebitOrCredit";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AccountsHoldForDebitOrCredit_FormClosing);
		base.Load += new System.EventHandler(AccountsHoldForDebitOrCredit_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AccountsHoldForDebitOrCredit_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
