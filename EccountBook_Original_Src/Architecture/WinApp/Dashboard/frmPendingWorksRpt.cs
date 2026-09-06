using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.ApprovalDashboard;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Inventory_Reports;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace Architecture.WinApp.Dashboard;

public class frmPendingWorksRpt : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool BranchFeature;

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Label label1;

	private Panel panel2;

	private Label label3;

	private Label label7;

	private Button btnshow;

	private Panel panel4;

	private Panel panel5;

	private Panel panel19;

	private Panel panel20;

	private Label label4;

	private FlowLayoutPanel SaleDashboardFlowLayout;

	private FlowLayoutPanel PurchaseDashboardFlowLayout;

	public DateTimePicker datToDate;

	public DateTimePicker datFromDate;

	internal UltraCombo CmbBranches;

	private Label Branches;

	private FlowLayoutPanel ExportCardsFlowLayoutPanel;

	public RadioButton RadPMOthers;

	public RadioButton RadPaddyRice;

	private FlowLayoutPanel PurchaseSeparateFlowLayout;

	private FlowLayoutPanel SaleSeparateFlowLayout;

	private Panel panel6;

	private Panel panel7;

	private Label label2;

	public frmPendingWorksRpt(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmPendingWorksRpt_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1350, 840);
			BranchFeature = CommonServices.GetERPFeatureById(11);
			datToDate.Focus();
			BranchesFill();
			AllCardsBind();
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
			DataTable dtBranch = BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, CmbBranches, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)CmbBranches).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbBranches.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbBranches.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbBranches.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbBranches.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbBranches).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)CmbBranches).Text = UserAccount.BranchName;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllCardsBind()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		try
		{
			PurchaseDashboardFlowLayout.Controls.Clear();
			PurchaseSeparateFlowLayout.Controls.Clear();
			SaleDashboardFlowLayout.Controls.Clear();
			ExportCardsFlowLayoutPanel.Controls.Clear();
			SaleSeparateFlowLayout.Controls.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = datFromDate.Value;
			obj.ToDate = datToDate.Value;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.ReportType = "FiguresOnly";
			obj.TypeId = (RadPaddyRice.Checked ? 1 : 2);
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = (BranchFeature ? BranchIds : "");
			}
			DataTable dthistory = Dashboard.PendingWorkDashboard(obj);
			if (dthistory.Rows.Count <= 0)
			{
				return;
			}
			int num = dthistory.AsEnumerable().Count((DataRow r) => r.Field<string>("PendingWorkDepartment") == "Purchase Department");
			DataTable dtPurchaseDepartment = new DataTable();
			DataTable dtSaleDepartment = new DataTable();
			if (num > 0)
			{
				dtPurchaseDepartment = (from r in dthistory.AsEnumerable()
					where r.Field<string>("PendingWorkDepartment") == "Purchase Department" && r.Field<int?>("InSeparatePanel").GetValueOrDefault() == 0
					select r).CopyToDataTable();
			}
			if (dthistory.AsEnumerable().Count((DataRow r) => r.Field<string>("PendingWorkDepartment") == "Sale Department") > 0)
			{
				dtSaleDepartment = (from r in dthistory.AsEnumerable()
					where r.Field<string>("PendingWorkDepartment") == "Sale Department" && r.Field<int?>("InSeparatePanel").GetValueOrDefault() == 0
					select r).CopyToDataTable();
			}
			EnumerableRowCollection<DataRow> ExportRows = from r in dthistory.AsEnumerable()
				where r.Field<string>("PendingWorkDepartment") == "Export Department"
				select r;
			DataTable dtExportDepartment = (ExportRows.Any() ? ExportRows.CopyToDataTable() : dthistory.Clone());
			EnumerableRowCollection<DataRow> PurchaseSeparateRows = from r in dthistory.AsEnumerable()
				where r.Field<string>("PendingWorkDepartment") == "Purchase Department" && r.Field<int?>("InSeparatePanel").GetValueOrDefault() == 1
				select r;
			DataTable dtSeparateRowsForPurchaseDepartment = (PurchaseSeparateRows.Any() ? PurchaseSeparateRows.CopyToDataTable() : dthistory.Clone());
			EnumerableRowCollection<DataRow> SaleSeparateRows = from r in dthistory.AsEnumerable()
				where r.Field<string>("PendingWorkDepartment") == "Sale Department" && r.Field<int?>("InSeparatePanel").GetValueOrDefault() == 1
				select r;
			DataTable dtSeparateRowsForSaleDepartment = (SaleSeparateRows.Any() ? SaleSeparateRows.CopyToDataTable() : dthistory.Clone());
			if (dtPurchaseDepartment.Rows.Count > 0)
			{
				PendingWorkCards[] PurchaseCardsList2 = new PendingWorkCards[dtPurchaseDepartment.Rows.Count];
				for (int m = 0; m < PurchaseCardsList2.Length; m++)
				{
					PurchaseCardsList2[m] = new PendingWorkCards();
					PurchaseCardsList2[m].Id = Conversion.ToString(dtPurchaseDepartment.Rows[m]["PendingWorkDepartment"]);
					PurchaseCardsList2[m].Title = Conversion.ToString(dtPurchaseDepartment.Rows[m]["PendingWorkCaption"]);
					PurchaseCardsList2[m].Caption = Conversion.ToString(dtPurchaseDepartment.Rows[m]["ActivityCaption"]);
					PurchaseCardsList2[m].Count = Conversion.ToString(dtPurchaseDepartment.Rows[m]["Count"]);
					PurchaseDashboardFlowLayout.Controls.Add(PurchaseCardsList2[m]);
					PurchaseCardsList2[m].UserControlClicked += UserControl_Click;
				}
			}
			if (dtSeparateRowsForPurchaseDepartment.Rows.Count > 0)
			{
				PendingWorkCards[] PurchaseCardsList = new PendingWorkCards[dtSeparateRowsForPurchaseDepartment.Rows.Count];
				for (int l = 0; l < PurchaseCardsList.Length; l++)
				{
					PurchaseCardsList[l] = new PendingWorkCards();
					PurchaseCardsList[l].CardTitle.BackColor = Color.DarkCyan;
					PurchaseCardsList[l].Id = Conversion.ToString(dtSeparateRowsForPurchaseDepartment.Rows[l]["PendingWorkDepartment"]);
					PurchaseCardsList[l].Title = Conversion.ToString(dtSeparateRowsForPurchaseDepartment.Rows[l]["PendingWorkCaption"]);
					PurchaseCardsList[l].Caption = Conversion.ToString(dtSeparateRowsForPurchaseDepartment.Rows[l]["ActivityCaption"]);
					PurchaseCardsList[l].Count = Conversion.ToString(dtSeparateRowsForPurchaseDepartment.Rows[l]["Count"]);
					PurchaseSeparateFlowLayout.Controls.Add(PurchaseCardsList[l]);
					PurchaseCardsList[l].UserControlClicked += UserControl_Click;
				}
			}
			if (dtSaleDepartment.Rows.Count > 0)
			{
				PendingWorkCards[] SaleCardsList = new PendingWorkCards[dtSaleDepartment.Rows.Count];
				for (int k = 0; k < SaleCardsList.Length; k++)
				{
					SaleCardsList[k] = new PendingWorkCards();
					SaleCardsList[k].Id = Conversion.ToString(dtSaleDepartment.Rows[k]["PendingWorkDepartment"]);
					SaleCardsList[k].Title = Conversion.ToString(dtSaleDepartment.Rows[k]["PendingWorkCaption"]);
					SaleCardsList[k].Caption = Conversion.ToString(dtSaleDepartment.Rows[k]["ActivityCaption"]);
					SaleCardsList[k].Count = Conversion.ToString(dtSaleDepartment.Rows[k]["Count"]);
					SaleDashboardFlowLayout.Controls.Add(SaleCardsList[k]);
					SaleCardsList[k].UserControlClicked += UserControl_Click;
				}
			}
			if (dtSeparateRowsForSaleDepartment.Rows.Count > 0)
			{
				PendingWorkCards[] CardsList2 = new PendingWorkCards[dtSeparateRowsForSaleDepartment.Rows.Count];
				for (int j = 0; j < CardsList2.Length; j++)
				{
					CardsList2[j] = new PendingWorkCards();
					CardsList2[j].CardTitle.BackColor = Color.DarkCyan;
					CardsList2[j].Id = Conversion.ToString(dtSeparateRowsForSaleDepartment.Rows[j]["PendingWorkDepartment"]);
					CardsList2[j].Title = Conversion.ToString(dtSeparateRowsForSaleDepartment.Rows[j]["PendingWorkCaption"]);
					CardsList2[j].Caption = Conversion.ToString(dtSeparateRowsForSaleDepartment.Rows[j]["ActivityCaption"]);
					CardsList2[j].Count = Conversion.ToString(dtSeparateRowsForSaleDepartment.Rows[j]["Count"]);
					SaleSeparateFlowLayout.Controls.Add(CardsList2[j]);
					CardsList2[j].UserControlClicked += UserControl_Click;
				}
			}
			if (dtExportDepartment.Rows.Count > 0)
			{
				PendingWorkCards[] CardsList = new PendingWorkCards[dtExportDepartment.Rows.Count];
				for (int i = 0; i < CardsList.Length; i++)
				{
					CardsList[i] = new PendingWorkCards();
					CardsList[i].Id = Conversion.ToString(dtExportDepartment.Rows[i]["PendingWorkDepartment"]);
					CardsList[i].Title = Conversion.ToString(dtExportDepartment.Rows[i]["PendingWorkCaption"]);
					CardsList[i].Caption = Conversion.ToString(dtExportDepartment.Rows[i]["ActivityCaption"]);
					CardsList[i].Count = Conversion.ToString(dtExportDepartment.Rows[i]["Count"]);
					ExportCardsFlowLayoutPanel.Controls.Add(CardsList[i]);
					CardsList[i].UserControlClicked += UserControl_Click;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		try
		{
			PendingWorkCards obj = (PendingWorkCards)sender;
			string DepartmentName = obj.Id;
			string pendingWorkName = obj.Title;
			string Caption = obj.Caption;
			if (Conversion.ToInt((object)obj.Count) <= 0)
			{
				return;
			}
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
			}
			if (pendingWorkName == "Gate Purchase Rate Not Applied" && DepartmentName == "Purchase Department")
			{
				LabPurchaseAnalysisForApproval labPurchaseAnalysisForApproval = new LabPurchaseAnalysisForApproval(UserAccount);
				labPurchaseAnalysisForApproval.RateApprovalGatePurchase = true;
				labPurchaseAnalysisForApproval.Show();
			}
			else if (pendingWorkName == "Purchase Order WeightCut Not Applied" && DepartmentName == "Purchase Department")
			{
				LabPurchaseAnalysisForApproval labPurchaseAnalysisForApproval2 = new LabPurchaseAnalysisForApproval(UserAccount);
				labPurchaseAnalysisForApproval2.WeightApprovalPurchaseOrder = true;
				labPurchaseAnalysisForApproval2.Show();
			}
			else if (pendingWorkName == "Vehicles Pending Clearance And Not Fully Forwarded" && DepartmentName == "Purchase Department")
			{
				frmGatePassForStockInTrust frmGatePassForStockInTrust = new frmGatePassForStockInTrust(UserAccount);
				frmGatePassForStockInTrust.FlagPendingWork = true;
				frmGatePassForStockInTrust.Show();
			}
			else
			{
				GetDetailPendingWorkForPurchase(pendingWorkName, DepartmentName, BranchIds, Caption);
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
			AllCardsBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailPendingWorkForPurchase(string PendingWorkName, string DepartmentName, string BranchesIds, string Caption)
	{
		try
		{
			frmPendingWorksDetail obj = new frmPendingWorksDetail(UserAccount);
			obj.FromDate = datFromDate.Value;
			obj.ToDate = datToDate.Value;
			obj.DepartmentName = DepartmentName;
			obj.PendingWorkName = PendingWorkName;
			obj.PendingWorkCaption = Caption;
			obj.BranchesIds = ((!BranchFeature) ? "" : ((((Control)(object)CmbBranches).Text != string.Empty) ? BranchesIds : ""));
			obj.TypeId = (RadPaddyRice.Checked ? 1 : 2);
			obj.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmPendingWorksRpt_FormClosed(object sender, FormClosedEventArgs e)
	{
		HistoryStack.Remove(this);
	}

	private void datToDate_ValueChanged(object sender, EventArgs e)
	{
		datFromDate.Value = datToDate.Value;
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
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		Appearance appearance37 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.ExportCardsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.SaleSeparateFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.SaleDashboardFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.PurchaseSeparateFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.PurchaseDashboardFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.RadPMOthers = new System.Windows.Forms.RadioButton();
		this.RadPaddyRice = new System.Windows.Forms.RadioButton();
		this.Branches = new System.Windows.Forms.Label();
		this.CmbBranches = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel19.SuspendLayout();
		this.panel20.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1409, 795);
		this.panel1.TabIndex = 0;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Controls.Add(this.panel19);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 55);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1409, 740);
		this.panel3.TabIndex = 1;
		this.panel19.BackColor = System.Drawing.SystemColors.ControlLight;
		this.panel19.Controls.Add(this.SaleSeparateFlowLayout);
		this.panel19.Controls.Add(this.SaleDashboardFlowLayout);
		this.panel19.Controls.Add(this.panel20);
		this.panel19.Location = new System.Drawing.Point(449, 7);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(445, 731);
		this.panel19.TabIndex = 7;
		this.ExportCardsFlowLayoutPanel.AutoScroll = true;
		this.ExportCardsFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ExportCardsFlowLayoutPanel.Location = new System.Drawing.Point(0, 46);
		this.ExportCardsFlowLayoutPanel.Name = "ExportCardsFlowLayoutPanel";
		this.ExportCardsFlowLayoutPanel.Size = new System.Drawing.Size(445, 685);
		this.ExportCardsFlowLayoutPanel.TabIndex = 3;
		this.SaleSeparateFlowLayout.AutoScroll = true;
		this.SaleSeparateFlowLayout.Dock = System.Windows.Forms.DockStyle.Top;
		this.SaleSeparateFlowLayout.Location = new System.Drawing.Point(0, 500);
		this.SaleSeparateFlowLayout.Name = "SaleSeparateFlowLayout";
		this.SaleSeparateFlowLayout.Size = new System.Drawing.Size(445, 228);
		this.SaleSeparateFlowLayout.TabIndex = 4;
		this.SaleDashboardFlowLayout.AutoScroll = true;
		this.SaleDashboardFlowLayout.Dock = System.Windows.Forms.DockStyle.Top;
		this.SaleDashboardFlowLayout.Location = new System.Drawing.Point(0, 46);
		this.SaleDashboardFlowLayout.Name = "SaleDashboardFlowLayout";
		this.SaleDashboardFlowLayout.Size = new System.Drawing.Size(445, 454);
		this.SaleDashboardFlowLayout.TabIndex = 1;
		this.panel20.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel20.Controls.Add(this.label4);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(445, 46);
		this.panel20.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Cambria", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(116, 7);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(255, 32);
		this.label4.TabIndex = 0;
		this.label4.Text = "Sale Pending Works";
		this.panel4.BackColor = System.Drawing.SystemColors.ControlLight;
		this.panel4.Controls.Add(this.PurchaseSeparateFlowLayout);
		this.panel4.Controls.Add(this.PurchaseDashboardFlowLayout);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Location = new System.Drawing.Point(3, 7);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(445, 731);
		this.panel4.TabIndex = 6;
		this.PurchaseSeparateFlowLayout.AutoScroll = true;
		this.PurchaseSeparateFlowLayout.Location = new System.Drawing.Point(0, 500);
		this.PurchaseSeparateFlowLayout.Name = "PurchaseSeparateFlowLayout";
		this.PurchaseSeparateFlowLayout.Size = new System.Drawing.Size(445, 228);
		this.PurchaseSeparateFlowLayout.TabIndex = 2;
		this.PurchaseDashboardFlowLayout.AutoScroll = true;
		this.PurchaseDashboardFlowLayout.Location = new System.Drawing.Point(0, 46);
		this.PurchaseDashboardFlowLayout.Name = "PurchaseDashboardFlowLayout";
		this.PurchaseDashboardFlowLayout.Size = new System.Drawing.Size(445, 454);
		this.PurchaseDashboardFlowLayout.TabIndex = 1;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(445, 46);
		this.panel5.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Cambria", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(74, 7);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(319, 32);
		this.label1.TabIndex = 0;
		this.label1.Text = "Purchase Pending Works";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.RadPMOthers);
		this.panel2.Controls.Add(this.RadPaddyRice);
		this.panel2.Controls.Add(this.Branches);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.datToDate);
		this.panel2.Controls.Add(this.datFromDate);
		this.panel2.Controls.Add(this.btnshow);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1409, 55);
		this.panel2.TabIndex = 0;
		this.RadPMOthers.AutoSize = true;
		this.RadPMOthers.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadPMOthers.Location = new System.Drawing.Point(794, 29);
		this.RadPMOthers.Name = "RadPMOthers";
		this.RadPMOthers.Size = new System.Drawing.Size(194, 17);
		this.RadPMOthers.TabIndex = 597;
		this.RadPMOthers.Text = "Packing Material && Others";
		this.RadPMOthers.UseVisualStyleBackColor = true;
		this.RadPaddyRice.AutoSize = true;
		this.RadPaddyRice.Checked = true;
		this.RadPaddyRice.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadPaddyRice.Location = new System.Drawing.Point(672, 29);
		this.RadPaddyRice.Name = "RadPaddyRice";
		this.RadPaddyRice.Size = new System.Drawing.Size(110, 17);
		this.RadPaddyRice.TabIndex = 596;
		this.RadPaddyRice.TabStop = true;
		this.RadPaddyRice.Text = "Paddy && Rice";
		this.RadPaddyRice.UseVisualStyleBackColor = true;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Branches.ForeColor = System.Drawing.Color.Black;
		this.Branches.Location = new System.Drawing.Point(339, 6);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(55, 15);
		this.Branches.TabIndex = 595;
		this.Branches.Text = "Branches";
		this.CmbBranches.AllowNull = (DefaultableBoolean)1;
		this.CmbBranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbBranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(339, 24);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(256, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(173, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 15);
		this.label3.TabIndex = 78;
		this.label3.Text = "To Date";
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(8, 6);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(63, 15);
		this.label7.TabIndex = 77;
		this.label7.Text = "From Date";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.datToDate.Location = new System.Drawing.Point(173, 26);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(163, 23);
		this.datToDate.TabIndex = 1;
		this.datToDate.ValueChanged += new System.EventHandler(datToDate_ValueChanged);
		this.datFromDate.Enabled = false;
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.datFromDate.Location = new System.Drawing.Point(8, 26);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(163, 23);
		this.datFromDate.TabIndex = 0;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(598, 24);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(57, 27);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.panel6.BackColor = System.Drawing.SystemColors.ControlLight;
		this.panel6.Controls.Add(this.ExportCardsFlowLayoutPanel);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Location = new System.Drawing.Point(896, 6);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(445, 731);
		this.panel6.TabIndex = 8;
		this.panel7.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(445, 46);
		this.panel7.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Cambria", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(85, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(288, 32);
		this.label2.TabIndex = 0;
		this.label2.Text = "Export Pending Works";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1409, 795);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmPendingWorksRpt";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmPendingWorksRpt_FormClosed);
		base.Load += new System.EventHandler(frmPendingWorksRpt_Load);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		base.ResumeLayout(false);
	}
}
