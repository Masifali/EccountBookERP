using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
namespace Architecture.WinApp
{
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;

using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;

using UserAccount = global::Architecture.Model.UserAccount;
using SupCustCreditLimitSchedule = global::Architecture.Model.Inventory.SupCustCreditLimitSchedule;

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using global::Architecture.BLL.Inventory;
using global::Architecture.Common;
using global::Architecture.Model;
using global::Architecture.Model.Inventory;
// // using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

public class SupfrmSupplierLimits : Form
{
	public UserAccount UserAccount = new UserAccount();

	private SupCustCreditLimitSchedule sup = new SupCustCreditLimitSchedule();

	private int RecId;

	private bool updateMood;

	private DataTable table = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private PrintDialog printDialog1;

	private ContextMenuStrip contextMenuStrip1;

	private BindingSource eccountbookProductionDataSetBindingSource;

	private Panel panel1;

	private Panel panel3;

	private DateTimePicker txteffecteddate;

	private TextBox txtcredit;

	private TextBox txtdebit;

	private Label label4;

	private Label lblcr;

	private Label label3;

	private Label label1;

	private Panel panel4;

	private Label lblCaption;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton update;

	private ToolStripButton btnNew;

	internal GridEX datagridview;

	private ToolStripButton btnSave;

	internal UltraCombo cmbsup;

	private ToolStripButton btnRefresh;

	private Panel panel5;

	private Panel panel6;

	private Label label2;

	private CtrlGrdBar ctrlGrdBar1;

	public SupfrmSupplierLimits(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	public SupfrmSupplierLimits()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
	}

	private bool formvalidation()
	{
		if (((UltraGridBase)cmbsup).ActiveRow == null || Conversion.ToInt(cmbsup.Value) == 0)
		{
			MessageBox.Show("Please Select Supplier");
			cmbsup.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtdebit.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Please Select Debit Credit");
			txtdebit.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtcredit.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Please Select  Credit");
			txtcredit.Focus();
			return false;
		}
		if (txteffecteddate.Text.Trim() == "")
		{
			MessageBox.Show("Please Select Effected Date");
			txteffecteddate.Focus();
			return false;
		}
		return true;
	}

	private void datagridviewform()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable getList = SupCustCreditLimitSchedule.Getall(new SupCustCreditLimitSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (getList.Rows.Count > 0)
			{
				DataTable dtSupplierLimits = new DataTable();
				dtSupplierLimits.Columns.Add("Id", typeof(int));
				dtSupplierLimits.Columns.Add("CompanyName", typeof(string));
				dtSupplierLimits.Columns.Add("CreditLimits", typeof(double));
				dtSupplierLimits.Columns.Add("DebitLimits", typeof(double));
				dtSupplierLimits.Columns.Add("EffectedDate", typeof(DateTime));
				dtSupplierLimits.Clear();
				for (int i = 0; i < getList.Rows.Count; i++)
				{
					dtSupplierLimits.Rows.Add(Conversion.ToInt(getList.Rows[i]["Id"]), Conversion.ToString(getList.Rows[i]["CompanyName"]), Conversion.ToDouble(getList.Rows[i]["CreditLimit"]), Conversion.ToDouble(getList.Rows[i]["DebitLimit"]), Conversion.ToDateTime(getList.Rows[i]["EffectedDate"]).ToShortDateString());
				}
				datagridview.DataSource = dtSupplierLimits;
				datagridview.RetrieveStructure();
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
		try
		{
			datagridview.RootTable.Columns["Id"].Visible = false;
			datagridview.RootTable.Columns["DebitLimits"].AggregateFunction = (AggregateFunction)2;
			datagridview.RootTable.Columns["DebitLimits"].TextAlignment = (TextAlignment)3;
			datagridview.RootTable.Columns["DebitLimits"].FormatString = clsGlobalVariables.stringFormatsingle;
			datagridview.RootTable.Columns["DebitLimits"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			datagridview.RootTable.Columns["CreditLimits"].AggregateFunction = (AggregateFunction)2;
			datagridview.RootTable.Columns["CreditLimits"].TextAlignment = (TextAlignment)3;
			datagridview.RootTable.Columns["CreditLimits"].FormatString = clsGlobalVariables.stringFormatsingle;
			datagridview.RootTable.Columns["CreditLimits"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(datagridview);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsup_Leave(object sender, EventArgs e)
	{
		try
		{
			datagridviewform();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsupplierfill()
	{
		try
		{
			DataTable dt = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbsup, "Id", "CompanyName", "Supplier/Customer", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupfrmSupplierLimits_Load(object sender, EventArgs e)
	{
		try
		{
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("Name", typeof(string));
			table.Columns.Add("Credit Limits", typeof(double));
			table.Columns.Add("Dredit Limits", typeof(double));
			table.Columns.Add("Effected Date", typeof(DateTime));
			btnSave.Visible = true;
			update.Visible = false;
			datagridviewform();
			cmbsupplierfill();
			cmbsup.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		if (!formvalidation())
		{
			return;
		}
		try
		{
			SupCustCreditLimitSchedule obj = new SupCustCreditLimitSchedule();
			if (RecId > 0)
			{
				obj.Id = RecId;
			}
			obj.SuplierCustomerId = Conversion.ToInt(cmbsup.Value);
			obj.DebitLimit = Conversion.ToDouble((object)txtdebit.Text.Trim());
			obj.CreditLimit = Conversion.ToDouble((object)txtcredit.Text.Trim());
			obj.EffectedDate = txteffecteddate.Value;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			SupCustCreditLimitSchedule.Save(obj);
			if (RecId == 0)
			{
				MessageBox.Show("Save Successfully");
			}
			else
			{
				MessageBox.Show("Update Successfully");
			}
			datagridviewform();
			Reset();
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

	private void update_Click(object sender, EventArgs e)
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

	private void datagridview_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnSave.Visible = false;
			update.Visible = true;
			SupCustCreditLimitSchedule bnk = new SupCustCreditLimitSchedule();
			RecId = Conversion.ToInt((object)datagridview.CurrentRow.Cells[0].Value.ToString());
			bnk = SupCustCreditLimitSchedule.GetByID(RecId);
			cmbsup.Value = bnk.SuplierCustomerId;
			txtcredit.Text = Conversion.ToString((object)bnk.CreditLimit);
			txtdebit.Text = Conversion.ToString((object)bnk.DebitLimit);
			txteffecteddate.Value = Conversion.ToDateTime((object)bnk.EffectedDate);
			datagridviewform();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			cmbsupplierfill();
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
			btnNew.Visible = true;
			Reset();
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
			RecId = 0;
			((Control)(object)cmbsup).Text = "";
			txtdebit.Text = string.Empty;
			txtcredit.Text = string.Empty;
			txteffecteddate.Value = DateTime.Now;
			update.Visible = false;
			btnSave.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupfrmSupplierLimits_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S && !updateMood)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U && updateMood)
			{
				update_Click(null, null);
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)datagridview).Name);
			if (fs != null)
			{
				datagridview.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtcredit_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtdebit_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.SupfrmSupplierLimits));
		this.printDialog1 = new System.Windows.Forms.PrintDialog();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.eccountbookProductionDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.datagridview = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.cmbsup = new UltraCombo();
		this.txteffecteddate = new System.Windows.Forms.DateTimePicker();
		this.txtcredit = new System.Windows.Forms.TextBox();
		this.txtdebit = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.lblcr = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.lblCaption = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.update = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		((System.ComponentModel.ISupportInitialize)this.eccountbookProductionDataSetBindingSource).BeginInit();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.datagridview).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsup).BeginInit();
		this.panel4.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.printDialog1.UseEXDialog = true;
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1135, 597);
		this.panel1.TabIndex = 1;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.datagridview);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 108);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1135, 489);
		this.panel5.TabIndex = 65;
		this.datagridview.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.datagridview).Dock = System.Windows.Forms.DockStyle.Fill;
		this.datagridview.FilterMode = (FilterMode)2;
		this.datagridview.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.datagridview).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.datagridview.GroupTotals = (GroupTotals)2;
		this.datagridview.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.datagridview).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.datagridview).Name = "datagridview";
		((ScrollableBase)this.datagridview).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.datagridview).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.datagridview).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.datagridview).Size = new System.Drawing.Size(1135, 461);
		((System.Windows.Forms.Control)(object)this.datagridview).TabIndex = 64;
		this.datagridview.TotalRow = (InheritableBoolean)1;
		this.datagridview.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.datagridview).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.datagridview).DoubleClick += new System.EventHandler(datagridview_DoubleClick);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1135, 28);
		this.panel6.TabIndex = 15;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1084, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.datagridview;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 25);
		this.ctrlGrdBar1.TabIndex = 1;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label2.Location = new System.Drawing.Point(6, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 19);
		this.label2.TabIndex = 0;
		this.label2.Text = "History";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsup);
		this.panel3.Controls.Add(this.txteffecteddate);
		this.panel3.Controls.Add(this.txtcredit);
		this.panel3.Controls.Add(this.txtdebit);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.lblcr);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 55);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1135, 53);
		this.panel3.TabIndex = 15;
		this.cmbsup.AllowNull = (DefaultableBoolean)1;
		this.cmbsup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbsup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbsup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbsup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbsup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsup).Location = new System.Drawing.Point(135, 15);
		((System.Windows.Forms.Control)(object)this.cmbsup).Name = "cmbsup";
		((System.Windows.Forms.Control)(object)this.cmbsup).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbsup).TabIndex = 0;
		this.txteffecteddate.CustomFormat = "dd-MMM-yy";
		this.txteffecteddate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txteffecteddate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txteffecteddate.Location = new System.Drawing.Point(910, 15);
		this.txteffecteddate.Name = "txteffecteddate";
		this.txteffecteddate.Size = new System.Drawing.Size(161, 23);
		this.txteffecteddate.TabIndex = 3;
		this.txtcredit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcredit.Location = new System.Drawing.Point(414, 15);
		this.txtcredit.Name = "txtcredit";
		this.txtcredit.Size = new System.Drawing.Size(163, 23);
		this.txtcredit.TabIndex = 1;
		this.txtcredit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcredit_KeyPress);
		this.txtdebit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdebit.Location = new System.Drawing.Point(644, 15);
		this.txtdebit.Name = "txtdebit";
		this.txtdebit.Size = new System.Drawing.Size(164, 23);
		this.txtdebit.TabIndex = 2;
		this.txtdebit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtdebit_KeyPress);
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(586, 19);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(36, 15);
		this.label4.TabIndex = 0;
		this.label4.Text = "Debit";
		this.lblcr.AutoSize = true;
		this.lblcr.BackColor = System.Drawing.Color.White;
		this.lblcr.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblcr.Location = new System.Drawing.Point(363, 19);
		this.lblcr.Name = "lblcr";
		this.lblcr.Size = new System.Drawing.Size(38, 15);
		this.lblcr.TabIndex = 0;
		this.lblcr.Text = "Credit";
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(815, 19);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(78, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "Effected Date";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(12, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(107, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Supplier/Customer";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.lblCaption);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 27);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1135, 28);
		this.panel4.TabIndex = 14;
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(6, 4);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(227, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Supplier / Customer Limits";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1135, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.update, this.btnSave, this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1135, 27);
		this.toolStrip1.TabIndex = 17;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnnew_Click);
		this.update.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.update.Image = (System.Drawing.Image)resources.GetObject("update.Image");
		this.update.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.update.Name = "update";
		this.update.Size = new System.Drawing.Size(78, 24);
		this.update.Text = "&Update";
		this.update.Click += new System.EventHandler(update_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnsave_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1135, 597);
		base.Controls.Add(this.panel1);
		base.Name = "SupfrmSupplierLimits";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(SupfrmSupplierLimits_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(SupfrmSupplierLimits_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.eccountbookProductionDataSetBindingSource).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.datagridview).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsup).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
