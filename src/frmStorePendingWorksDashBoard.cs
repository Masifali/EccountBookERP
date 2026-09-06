using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.StoreManagement;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Dashboard
{

public class frmStorePendingWorksDashBoard : Form
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

	public frmStorePendingWorksDashBoard(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmStorePendingWorksDashBoard_Load(object sender, EventArgs e)
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
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		try
		{
			PurchaseDashboardFlowLayout.Controls.Clear();
			SaleDashboardFlowLayout.Controls.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = datFromDate.Value;
			obj.ToDate = datToDate.Value;
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
			DataTable dthistory = Dashboard.Dashboards_StorePendingWork(obj);
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
					where r.Field<string>("PendingWorkDepartment") == "Purchase Department"
					select r).CopyToDataTable();
			}
			if (dthistory.AsEnumerable().Count((DataRow r) => r.Field<string>("PendingWorkDepartment") == "Sale Department") > 0)
			{
				dtSaleDepartment = (from r in dthistory.AsEnumerable()
					where r.Field<string>("PendingWorkDepartment") == "Sale Department"
					select r).CopyToDataTable();
			}
			if (dtPurchaseDepartment.Rows.Count > 0)
			{
				PendingWorkCards[] PurchaseCardsList = new PendingWorkCards[dtPurchaseDepartment.Rows.Count];
				for (int j = 0; j < PurchaseCardsList.Length; j++)
				{
					PurchaseCardsList[j] = new PendingWorkCards();
					PurchaseCardsList[j].Id = Conversion.ToString(dtPurchaseDepartment.Rows[j]["PendingWorkDepartment"]);
					PurchaseCardsList[j].Title = Conversion.ToString(dtPurchaseDepartment.Rows[j]["PendingWorkCaption"]);
					PurchaseCardsList[j].Caption = Conversion.ToString(dtPurchaseDepartment.Rows[j]["PendingWorkCaption"]);
					PurchaseCardsList[j].Count = Conversion.ToString(dtPurchaseDepartment.Rows[j]["Count"]);
					PurchaseDashboardFlowLayout.Controls.Add(PurchaseCardsList[j]);
					PurchaseCardsList[j].UserControlClicked += UserControl_Click;
				}
			}
			if (dtSaleDepartment.Rows.Count > 0)
			{
				PendingWorkCards[] SaleCardsList = new PendingWorkCards[dtSaleDepartment.Rows.Count];
				for (int i = 0; i < SaleCardsList.Length; i++)
				{
					SaleCardsList[i] = new PendingWorkCards();
					SaleCardsList[i].Id = Conversion.ToString(dtSaleDepartment.Rows[i]["PendingWorkDepartment"]);
					SaleCardsList[i].Title = Conversion.ToString(dtSaleDepartment.Rows[i]["PendingWorkCaption"]);
					SaleCardsList[i].Caption = Conversion.ToString(dtSaleDepartment.Rows[i]["PendingWorkCaption"]);
					SaleCardsList[i].Count = Conversion.ToString(dtSaleDepartment.Rows[i]["Count"]);
					SaleDashboardFlowLayout.Controls.Add(SaleCardsList[i]);
					SaleCardsList[i].UserControlClicked += UserControl_Click;
				}
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

	private void frmStorePendingWorksDashBoard_FormClosed(object sender, FormClosedEventArgs e)
	{
		HistoryStack.Remove(this);
	}

	private void datToDate_ValueChanged(object sender, EventArgs e)
	{
		datFromDate.Value = datToDate.Value;
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		try
		{
			PendingWorkCards obj = (PendingWorkCards)sender;
			string DepartmentName = obj.Id;
			string pendingWorkName = obj.Title;
			_ = obj.Caption;
			if (Conversion.ToInt((object)obj.Count) > 0)
			{
				if (pendingWorkName == "Pending Pre-Bill For Delivery Challan" && DepartmentName == "Purchase Department")
				{
					frmPurchasePreBill frmPurchasePreBill = new frmPurchasePreBill(UserAccount);
					frmPurchasePreBill.OnlyPending = true;
					frmPurchasePreBill.tabControl1.SelectedIndex = 1;
					frmPurchasePreBill.FromDate = datFromDate.Value;
					frmPurchasePreBill.ToDate = datToDate.Value;
					frmPurchasePreBill.Show();
				}
				else if (pendingWorkName == "Pending Delivery Challan For GatePass" && DepartmentName == "Purchase Department")
				{
					frmDeliveryChallanAgainstPurchasePreBill frmDeliveryChallanAgainstPurchasePreBill = new frmDeliveryChallanAgainstPurchasePreBill(UserAccount);
					frmDeliveryChallanAgainstPurchasePreBill.OnlyPending = true;
					frmDeliveryChallanAgainstPurchasePreBill.tabControl1.SelectedIndex = 1;
					frmDeliveryChallanAgainstPurchasePreBill.FromDate = datFromDate.Value;
					frmDeliveryChallanAgainstPurchasePreBill.ToDate = datToDate.Value;
					frmDeliveryChallanAgainstPurchasePreBill.Show();
				}
				else if (pendingWorkName == "Pending Gate Passes For Grn" && DepartmentName == "Purchase Department")
				{
					frmGatePassGeneral frmGatePassGeneral = new frmGatePassGeneral(UserAccount);
					frmGatePassGeneral.OnlyPending = true;
					frmGatePassGeneral.FromDatePublic = datFromDate.Value;
					frmGatePassGeneral.ToDatePublic = datToDate.Value;
					frmGatePassGeneral.documentTypeId = 52;
					frmGatePassGeneral.Show();
				}
				else if (pendingWorkName == "Pending GRN For Invoice" && DepartmentName == "Purchase Department")
				{
					GrnStore grnStore = new GrnStore(UserAccount);
					grnStore.OnlyPending = true;
					grnStore.FromDate = datFromDate.Value;
					grnStore.ToDate = datToDate.Value;
					grnStore.tabControl1.SelectedIndex = 2;
					grnStore.Show();
				}
				else if (pendingWorkName == "Pending Gate Passes For Gdn" && DepartmentName == "Sale Department")
				{
					frmGPOutward frmGPOutward = new frmGPOutward(UserAccount);
					frmGPOutward.OnlyPending = true;
					frmGPOutward.FromDatePublic = datFromDate.Value;
					frmGPOutward.ToDatePublic = datToDate.Value;
					frmGPOutward.Show();
				}
				else if (pendingWorkName == "Pending GDN For Invoice" && DepartmentName == "Sale Department")
				{
					frmGoodsDispatchNotesRegisterStoreAndPm frmGoodsDispatchNotesRegisterStoreAndPm = new frmGoodsDispatchNotesRegisterStoreAndPm(UserAccount);
					frmGoodsDispatchNotesRegisterStoreAndPm.OnlyPending = true;
					frmGoodsDispatchNotesRegisterStoreAndPm.FromDate = datFromDate.Value;
					frmGoodsDispatchNotesRegisterStoreAndPm.ToDate = datToDate.Value;
					frmGoodsDispatchNotesRegisterStoreAndPm.Show();
				}
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
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
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
		this.SaleDashboardFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.PurchaseDashboardFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.Branches = new System.Windows.Forms.Label();
		this.CmbBranches = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel19.SuspendLayout();
		this.panel20.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1034, 795);
		this.panel1.TabIndex = 0;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel19);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 55);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1034, 740);
		this.panel3.TabIndex = 1;
		this.panel19.BackColor = System.Drawing.SystemColors.ControlLight;
		this.panel19.Controls.Add(this.SaleDashboardFlowLayout);
		this.panel19.Controls.Add(this.panel20);
		this.panel19.Location = new System.Drawing.Point(505, 7);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(500, 731);
		this.panel19.TabIndex = 7;
		this.SaleDashboardFlowLayout.AutoScroll = true;
		this.SaleDashboardFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.SaleDashboardFlowLayout.Location = new System.Drawing.Point(0, 46);
		this.SaleDashboardFlowLayout.Name = "SaleDashboardFlowLayout";
		this.SaleDashboardFlowLayout.Size = new System.Drawing.Size(500, 685);
		this.SaleDashboardFlowLayout.TabIndex = 1;
		this.panel20.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel20.Controls.Add(this.label4);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(500, 46);
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
		this.panel4.Controls.Add(this.PurchaseDashboardFlowLayout);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Location = new System.Drawing.Point(3, 7);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(500, 731);
		this.panel4.TabIndex = 6;
		this.PurchaseDashboardFlowLayout.AutoScroll = true;
		this.PurchaseDashboardFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PurchaseDashboardFlowLayout.Location = new System.Drawing.Point(0, 46);
		this.PurchaseDashboardFlowLayout.Name = "PurchaseDashboardFlowLayout";
		this.PurchaseDashboardFlowLayout.Size = new System.Drawing.Size(500, 685);
		this.PurchaseDashboardFlowLayout.TabIndex = 1;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(500, 46);
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
		this.panel2.Size = new System.Drawing.Size(1034, 55);
		this.panel2.TabIndex = 0;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1034, 795);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmStorePendingWorksDashBoard";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmStorePendingWorksDashBoard_FormClosed);
		base.Load += new System.EventHandler(frmStorePendingWorksDashBoard_Load);
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
		base.ResumeLayout(false);
	}
}

}
