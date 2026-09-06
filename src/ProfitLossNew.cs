// // using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
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
namespace Architecture.WinApp.Account_Reports
{

public class ProfitLossNew : Form
{
	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private DateTimePicker todate;

	private DateTimePicker fromdate;

	private Label label33;

	private Label label27;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private Panel panel5;

	private Button btnshow;

	private Label labelInterval01;

	private TextBox txtTotalSales;

	internal GridEX grd;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label9;

	private Label label8;

	private Label label4;

	private Label label7;

	private Label label3;

	private Label label6;

	private Label label2;

	private Label label5;

	private TextBox txtNetPl;

	private TextBox txtPurchaseSideTotal;

	private TextBox txtOpeningStock;

	private TextBox txtSaleSideTotal;

	private TextBox txtTotalExpense;

	private TextBox txtClosingStock;

	private TextBox txtTotalPurchase;

	private TextBox txtTotalIncome;

	private ToolStripButton btnshortcutkey;

	internal UltraCombo CmbBranches;

	private Label Branches;

	public ProfitLossNew(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void ProfitLossNew_Load(object sender, EventArgs e)
	{
		try
		{
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 500);
			DateTime currentDate = DateTime.Now;
			DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
			fromdate.Value = firstDayOfMonth;
			BranchFeature = CommonServices.GetERPFeatureById(17);
			BranchFeatureConsolidated = CommonServices.GetERPFeatureById(18);
			if (!BranchFeature)
			{
				Branches.Visible = false;
				((Control)(object)CmbBranches).Visible = false;
			}
			else
			{
				BranchesFill();
			}
			todate.Value = DateTime.Now;
			GridBind();
			fromdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			DataTable dt = VoucherHead.GetBranchesFromVouchersByAccountId(UserAccount.OrganizationId, UserAccount.CompanyId, "", 0);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbBranches, "Id", "BranchName", "Branch Name", ZeroIndex: false);
				if (BranchFeature && BranchFeatureConsolidated)
				{
					UltraGridColumn obj = ((UltraGridBase)CmbBranches).DisplayLayout.Bands[0].Columns.Add();
					((KeyedSubObjectBase)obj).Key = "Selected";
					((HeaderBase)obj.Header).Caption = string.Empty;
					obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
					obj.DataType = typeof(bool);
					((HeaderBase)obj.Header).VisiblePosition = 0;
					CmbBranches.LimitToList = false;
					CmbBranches.CheckedListSettings.CheckStateMember = "Selected";
					((EditorCheckedListSettings)CmbBranches.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
					((EditorCheckedListSettings)CmbBranches.CheckedListSettings).ListSeparator = ",";
					((EditorCheckedListSettings)CmbBranches.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				}
				((Control)(object)CmbBranches).Text = UserAccount.BranchName;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void reset()
	{
		try
		{
			txtTotalSales.Text = 0.ToString("#,#;(#,#);0");
			txtTotalIncome.Text = 0.ToString("#,#;(#,#);0");
			txtClosingStock.Text = 0.ToString("#,#;(#,#);0");
			txtSaleSideTotal.Text = 0.ToString("#,#;(#,#);0");
			txtTotalPurchase.Text = 0.ToString("#,#;(#,#);0");
			txtTotalExpense.Text = 0.ToString("#,#;(#,#);0");
			txtOpeningStock.Text = 0.ToString("#,#;(#,#);0");
			txtPurchaseSideTotal.Text = 0.ToString("#,#;(#,#);0");
			grd.ClearStructure();
			DateTime currentDate = DateTime.Now;
			DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
			fromdate.Value = firstDayOfMonth;
			todate.Value = DateTime.Now;
			fromdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void ProfitLossNew_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			GridBind();
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			wf.Close();
		}
	}

	private void GridBind()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtGrid.Clear();
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.FromDate = fromdate.Value;
			vh.ToDate = todate.Value;
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			vh.BranchesIds = ((BranchIds != "") ? BranchIds : "");
			dtGrid = VoucherReports.ProfitAndLoss_New(vh);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dtTarget = new DataTable();
				dtTarget.Columns.Add("SortNo", typeof(int));
				dtTarget.Columns.Add("NoteTitle", typeof(string));
				dtTarget.Columns.Add("AccountTitle", typeof(string));
				dtTarget.Columns.Add("Amount", typeof(decimal));
				foreach (DataRow row in dtGrid.Rows)
				{
					dtTarget.Rows.Add(row["SortNo"], row["NoteTitle"], row["AccountTitle"], row["Amount"]);
				}
				grd.DataSource = dtTarget;
				grd.RetrieveStructure();
				GrdSettings();
				double TSales = Conversion.ToDouble(dtGrid.Rows[0]["TotalSale"]);
				double TotalOtherIncome = Conversion.ToDouble(dtGrid.Rows[0]["TotalOtherIncome"]);
				double ClosingStock = Conversion.ToDouble(dtGrid.Rows[0]["ClosingStock"]);
				double SaleSideTotal = TSales + TotalOtherIncome + ClosingStock;
				double TotalPurchase = Conversion.ToDouble(dtGrid.Rows[0]["TotalPurchase"]);
				double TExpenses = Conversion.ToDouble(dtGrid.Rows[0]["Expenses"]);
				double OpeningStock = Conversion.ToDouble(dtGrid.Rows[0]["OpeningStock"]);
				double PurchaseSideTotal = TotalPurchase + TExpenses + OpeningStock;
				txtTotalSales.Text = TSales.ToString(clsGlobalVariables.stringFormatboth);
				txtTotalIncome.Text = TotalOtherIncome.ToString(clsGlobalVariables.stringFormatboth);
				txtClosingStock.Text = ClosingStock.ToString(clsGlobalVariables.stringFormatboth);
				txtSaleSideTotal.Text = SaleSideTotal.ToString(clsGlobalVariables.stringFormatboth);
				txtTotalPurchase.Text = TotalPurchase.ToString(clsGlobalVariables.stringFormatboth);
				txtTotalExpense.Text = TExpenses.ToString(clsGlobalVariables.stringFormatboth);
				txtOpeningStock.Text = OpeningStock.ToString(clsGlobalVariables.stringFormatboth);
				txtPurchaseSideTotal.Text = PurchaseSideTotal.ToString(clsGlobalVariables.stringFormatboth);
				txtNetPl.Text = (SaleSideTotal - PurchaseSideTotal).ToString(clsGlobalVariables.stringFormatboth);
			}
			else
			{
				txtTotalSales.Text = 0.ToString(clsGlobalVariables.stringFormatboth);
				txtTotalIncome.Text = 0.ToString(clsGlobalVariables.stringFormatboth);
				txtClosingStock.Text = 0.ToString(clsGlobalVariables.stringFormatboth);
				txtSaleSideTotal.Text = 0.ToString(clsGlobalVariables.stringFormatboth);
				txtTotalPurchase.Text = 0.ToString(clsGlobalVariables.stringFormatboth);
				txtTotalExpense.Text = 0.ToString(clsGlobalVariables.stringFormatboth);
				txtOpeningStock.Text = 0.ToString(clsGlobalVariables.stringFormatboth);
				txtPurchaseSideTotal.Text = 0.ToString(clsGlobalVariables.stringFormatboth);
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GrdSettings()
	{
		try
		{
			grd.RootTable.Columns["SortNo"].Visible = false;
			grd.RootTable.Columns["AccountTitle"].Width = 400;
			grd.RootTable.Columns["Amount"].Width = 200;
			grd.GroupByBoxVisible = true;
			grd.RootTable.Groups.Add("NoteTitle");
			grd.RootTable.Columns["NoteTitle"].Visible = false;
			grd.RootTable.Columns["NoteTitle"].AllowSort = false;
			grd.RootTable.GroupTotals = (GroupTotals)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid == null || dtGrid.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGrid, "420_ProfitLossReport.rpt");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.ToString());
		}
	}

	private void ProfitLossNew_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				BtnPrint_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				fromdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				fromdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcutkey_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl + P", "For print");
			dt.Rows.Add("Ctrl+F5", "For Focus On Date Type");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date Type in Filter");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn.Key == "AccountCode")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["AccountId"].Value), fromdate.Value, todate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "AccountCode")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["AccountId"].Value), fromdate.Value, todate.Value);
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
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Expected O, but got Unknown
		//IL_1a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a85: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.Branches = new System.Windows.Forms.Label();
		this.CmbBranches = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtNetPl = new System.Windows.Forms.TextBox();
		this.txtPurchaseSideTotal = new System.Windows.Forms.TextBox();
		this.labelInterval01 = new System.Windows.Forms.Label();
		this.txtOpeningStock = new System.Windows.Forms.TextBox();
		this.txtSaleSideTotal = new System.Windows.Forms.TextBox();
		this.txtTotalExpense = new System.Windows.Forms.TextBox();
		this.txtClosingStock = new System.Windows.Forms.TextBox();
		this.txtTotalPurchase = new System.Windows.Forms.TextBox();
		this.txtTotalIncome = new System.Windows.Forms.TextBox();
		this.txtTotalSales = new System.Windows.Forms.TextBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkey = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(984, 23);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(95, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Profit Loss";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.Branches);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.label8);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label7);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.txtNetPl);
		this.panel5.Controls.Add(this.txtPurchaseSideTotal);
		this.panel5.Controls.Add(this.labelInterval01);
		this.panel5.Controls.Add(this.txtOpeningStock);
		this.panel5.Controls.Add(this.txtSaleSideTotal);
		this.panel5.Controls.Add(this.txtTotalExpense);
		this.panel5.Controls.Add(this.txtClosingStock);
		this.panel5.Controls.Add(this.txtTotalPurchase);
		this.panel5.Controls.Add(this.txtTotalIncome);
		this.panel5.Controls.Add(this.txtTotalSales);
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Controls.Add(this.todate);
		this.panel5.Controls.Add(this.fromdate);
		this.panel5.Controls.Add(this.label33);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 50);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(984, 116);
		this.panel5.TabIndex = 0;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.Branches.Location = new System.Drawing.Point(7, 66);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(55, 15);
		this.Branches.TabIndex = 118;
		this.Branches.Text = "Branches";
		this.CmbBranches.AllowNull = (DefaultableBoolean)1;
		this.CmbBranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(71, 60);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 2;
		this.label9.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.label9.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(809, 6);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(169, 25);
		this.label9.TabIndex = 109;
		this.label9.Text = "Profit / Loss";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label8.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.label8.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(510, 87);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(100, 25);
		this.label8.TabIndex = 109;
		this.label8.Text = "Total";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label4.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.label4.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(226, 87);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(100, 25);
		this.label4.TabIndex = 109;
		this.label4.Text = "Totals";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label7.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.label7.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(510, 60);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(100, 25);
		this.label7.TabIndex = 109;
		this.label7.Text = "Opening Stock";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label3.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.label3.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(226, 60);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(100, 25);
		this.label3.TabIndex = 109;
		this.label3.Text = "Closing Stock";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label6.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.label6.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(510, 33);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(100, 25);
		this.label6.TabIndex = 109;
		this.label6.Text = "Expenses";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label2.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.label2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(226, 33);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(100, 25);
		this.label2.TabIndex = 109;
		this.label2.Text = "Other Income";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label5.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.label5.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(510, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(100, 25);
		this.label5.TabIndex = 109;
		this.label5.Text = "Purchases";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtNetPl.Enabled = false;
		this.txtNetPl.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetPl.Location = new System.Drawing.Point(809, 33);
		this.txtNetPl.Margin = new System.Windows.Forms.Padding(2);
		this.txtNetPl.Name = "txtNetPl";
		this.txtNetPl.Size = new System.Drawing.Size(169, 26);
		this.txtNetPl.TabIndex = 11;
		this.txtNetPl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPurchaseSideTotal.Enabled = false;
		this.txtPurchaseSideTotal.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchaseSideTotal.Location = new System.Drawing.Point(615, 87);
		this.txtPurchaseSideTotal.Margin = new System.Windows.Forms.Padding(2);
		this.txtPurchaseSideTotal.Name = "txtPurchaseSideTotal";
		this.txtPurchaseSideTotal.Size = new System.Drawing.Size(175, 26);
		this.txtPurchaseSideTotal.TabIndex = 10;
		this.txtPurchaseSideTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.labelInterval01.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.labelInterval01.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.labelInterval01.Location = new System.Drawing.Point(226, 6);
		this.labelInterval01.Name = "labelInterval01";
		this.labelInterval01.Size = new System.Drawing.Size(100, 25);
		this.labelInterval01.TabIndex = 109;
		this.labelInterval01.Text = "Sales";
		this.labelInterval01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtOpeningStock.Enabled = false;
		this.txtOpeningStock.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtOpeningStock.Location = new System.Drawing.Point(615, 60);
		this.txtOpeningStock.Margin = new System.Windows.Forms.Padding(2);
		this.txtOpeningStock.Name = "txtOpeningStock";
		this.txtOpeningStock.Size = new System.Drawing.Size(175, 25);
		this.txtOpeningStock.TabIndex = 9;
		this.txtOpeningStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSaleSideTotal.Enabled = false;
		this.txtSaleSideTotal.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleSideTotal.Location = new System.Drawing.Point(331, 87);
		this.txtSaleSideTotal.Margin = new System.Windows.Forms.Padding(2);
		this.txtSaleSideTotal.Name = "txtSaleSideTotal";
		this.txtSaleSideTotal.Size = new System.Drawing.Size(175, 26);
		this.txtSaleSideTotal.TabIndex = 6;
		this.txtSaleSideTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalExpense.Enabled = false;
		this.txtTotalExpense.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtTotalExpense.Location = new System.Drawing.Point(615, 33);
		this.txtTotalExpense.Margin = new System.Windows.Forms.Padding(2);
		this.txtTotalExpense.Name = "txtTotalExpense";
		this.txtTotalExpense.Size = new System.Drawing.Size(175, 25);
		this.txtTotalExpense.TabIndex = 8;
		this.txtTotalExpense.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtClosingStock.Enabled = false;
		this.txtClosingStock.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtClosingStock.Location = new System.Drawing.Point(331, 60);
		this.txtClosingStock.Margin = new System.Windows.Forms.Padding(2);
		this.txtClosingStock.Name = "txtClosingStock";
		this.txtClosingStock.Size = new System.Drawing.Size(175, 25);
		this.txtClosingStock.TabIndex = 5;
		this.txtClosingStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalPurchase.Enabled = false;
		this.txtTotalPurchase.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtTotalPurchase.Location = new System.Drawing.Point(615, 6);
		this.txtTotalPurchase.Margin = new System.Windows.Forms.Padding(2);
		this.txtTotalPurchase.Name = "txtTotalPurchase";
		this.txtTotalPurchase.Size = new System.Drawing.Size(175, 25);
		this.txtTotalPurchase.TabIndex = 7;
		this.txtTotalPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalIncome.Enabled = false;
		this.txtTotalIncome.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtTotalIncome.Location = new System.Drawing.Point(331, 33);
		this.txtTotalIncome.Margin = new System.Windows.Forms.Padding(2);
		this.txtTotalIncome.Name = "txtTotalIncome";
		this.txtTotalIncome.Size = new System.Drawing.Size(175, 25);
		this.txtTotalIncome.TabIndex = 4;
		this.txtTotalIncome.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalSales.Enabled = false;
		this.txtTotalSales.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtTotalSales.Location = new System.Drawing.Point(331, 6);
		this.txtTotalSales.Margin = new System.Windows.Forms.Padding(2);
		this.txtTotalSales.Name = "txtTotalSales";
		this.txtTotalSales.Size = new System.Drawing.Size(175, 25);
		this.txtTotalSales.TabIndex = 3;
		this.txtTotalSales.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(140, 88);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 26);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(71, 34);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(130, 23);
		this.todate.TabIndex = 1;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(71, 7);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(130, 23);
		this.fromdate.TabIndex = 0;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Black;
		this.label33.Location = new System.Drawing.Point(7, 38);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "To Date";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Black;
		this.label27.Location = new System.Drawing.Point(7, 11);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From Date";
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 166);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(984, 24);
		this.panel1.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(944, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 23);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 190);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(984, 271);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 1);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(142, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Filtered Records";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(984, 27);
		this.panel2.TabIndex = 3;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.print, this.btnshortcutkey });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(984, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(92, 24);
		this.print.Text = "420-Print";
		this.print.Click += new System.EventHandler(BtnPrint_Click);
		this.btnshortcutkey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkey.Name = "btnshortcutkey";
		this.btnshortcutkey.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkey.Text = "ShortCut Keys";
		this.btnshortcutkey.Click += new System.EventHandler(btnshortcutkey_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		base.ClientSize = new System.Drawing.Size(984, 461);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "ProfitLossNew";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ProfitLossNew_FormClosing);
		base.Load += new System.EventHandler(ProfitLossNew_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ProfitLossNew_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
