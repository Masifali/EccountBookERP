using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.ApprovalDashboard;
using Architecture.WinApp.Audit_Dashboard;
using Architecture.WinApp.Cmagt.Approvals;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace Architecture.WinApp.SpecialApprovalDashboard;

public class SpecialApprovalDashboard : Form
{
	private bool BranchFeature;

	private readonly UserAccount _userAccount;

	private IContainer components;

	private Panel panel1;

	private GroupBox groupBox2;

	private FlowLayoutPanel inventoryFlowLayout;

	private GroupBox groupBox1;

	private FlowLayoutPanel accountsFlowLayout;

	private Panel panel2;

	private Label label2;

	private Panel panel5;

	private Label label1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton BtnApprovalDashBoard;

	private Panel FilterPanel;

	private GroupBox groupBox3;

	private Button BtnShow;

	internal UltraCombo CmbBranch;

	private Label lblCompany;

	private Panel MainPanel;

	private GroupBox GroupBoxCommissionAgent;

	private FlowLayoutPanel CommissionFlowLayout;

	private Panel panel4;

	private Label label5;

	private Panel PanelInventoryAndCommission;

	public SpecialApprovalDashboard(UserAccount pUser)
	{
		InitializeComponent();
		_userAccount = pUser;
	}

	private void ApprovalDashboard_Load(object sender, EventArgs e)
	{
		try
		{
			BranchFeature = CommonServices.GetERPFeatureById(11);
			if (BranchFeature)
			{
				FilterPanel.Visible = true;
				groupBox1.Location = new Point(3, 70);
				groupBox2.Location = new Point(599, 70);
			}
			else
			{
				FilterPanel.Visible = false;
				groupBox1.Location = new Point(3, 30);
				groupBox2.Location = new Point(599, 30);
			}
			BranchesBind();
			LoadDashboardCards();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesBind()
	{
		try
		{
			List<getGlobalBranchesAllocateToUser> lst = clsGlobalVariables.globalBranchesAllocateToUser;
			DataTable dtBranch = new DataTable();
			dtBranch.Columns.Add("BranchId", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			if (lst.Count > 0)
			{
				for (int i = 0; i < lst.Count; i++)
				{
					dtBranch.Rows.Add(lst[i].BranchId, lst[i].BranchName);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbBranch, dtBranch, "BranchId", "BranchName", "Branch Name", AllColumns: true, Conversion.ToInt(CmbBranch.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtBranch.Rows.Count > 0)
			{
				CmbBranch.Value = _userAccount.BranchesId;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LoadDashboardCards()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		try
		{
			accountsFlowLayout.Controls.Clear();
			inventoryFlowLayout.Controls.Clear();
			CommissionFlowLayout.Controls.Clear();
			DataTable dtInventory = Dashboard.ReadDashboardPendingForSpecialApprovals(new ReportsParameters
			{
				OrganizationId = _userAccount.OrganizationId,
				CompanyId = _userAccount.CompanyId,
				UserId = _userAccount.ID,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = (BranchFeature ? Conversion.ToInt(CmbBranch.Value) : _userAccount.BranchesId)
			});
			PopulateCards(dtInventory);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading dashboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PopulateCards(DataTable dataTable)
	{
		if (dataTable == null || dataTable.Rows.Count <= 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			if (Conversion.ToInt(row["ActionId"]) == 2)
			{
				DynamicallyGenerateCards card3 = new DynamicallyGenerateCards
				{
					Id = Conversion.ToString(row["TypeID"]),
					Title = Conversion.ToString(row["Description"]),
					Count = Conversion.ToString(row["Count"]),
					Reason = Conversion.ToString(row["ReasonForSpecialApproval"]),
					ReasonId = Conversion.ToInt(row["ReasonId"])
				};
				card3.CardCount.Location = ((card3.Reason != string.Empty && card3.Reason != null) ? new Point(4, 95) : new Point(4, 81));
				card3.lblReason.Visible = ((card3.Reason != string.Empty && card3.Reason != null) ? true : false);
				inventoryFlowLayout.Controls.Add(card3);
				card3.UserControlClicked += UserControl_Click;
			}
			if (Conversion.ToInt(row["ActionId"]) == 3)
			{
				DynamicallyGenerateCards card = new DynamicallyGenerateCards
				{
					Id = Conversion.ToString(row["TypeID"]),
					Title = Conversion.ToString(row["Description"]),
					Count = Conversion.ToString(row["Count"]),
					Reason = Conversion.ToString(row["ReasonForSpecialApproval"]),
					ReasonId = Conversion.ToInt(row["ReasonId"])
				};
				card.CardCount.Location = ((card.Reason != string.Empty && card.Reason != null) ? new Point(4, 95) : new Point(4, 81));
				card.lblReason.Visible = ((card.Reason != string.Empty && card.Reason != null) ? true : false);
				CommissionFlowLayout.Controls.Add(card);
				card.UserControlClicked += UserControl_Click;
			}
			else if (Conversion.ToInt(row["ActionId"]) == 1)
			{
				DynamicallyGenerateCards card2 = new DynamicallyGenerateCards
				{
					Id = Conversion.ToString(row["TypeID"]),
					Title = Conversion.ToString(row["Description"]),
					Count = Conversion.ToString(row["Count"])
				};
				accountsFlowLayout.Controls.Add(card2);
				card2.UserControlClicked += UserControl_Click;
			}
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		if (!(sender is DynamicallyGenerateCards card))
		{
			return;
		}
		int id = Conversion.ToInt((object)card.Id);
		int reasonId = Conversion.ToInt((object)card.ReasonId);
		switch (id)
		{
		case 1:
		{
			using (StockTransferWeightAuditReport report = new StockTransferWeightAuditReport(_userAccount))
			{
				report.SpecialApproval = true;
				report.ShowDialog();
			}
			break;
		}
		case 2:
		{
			using (PendingForSpecialApprovalGpInward pendingForSpecialApprovalGpInward = new PendingForSpecialApprovalGpInward(_userAccount))
			{
				pendingForSpecialApprovalGpInward.ShowDialog();
			}
			break;
		}
		case 3:
		{
			using (PendingGRNForSpecialApproval pendingGRNForSpecialApproval = new PendingGRNForSpecialApproval(_userAccount))
			{
				pendingGRNForSpecialApproval.DocumentTypeId = 46;
				pendingGRNForSpecialApproval.SpecialApprovalGrn_ReasonId = reasonId;
				pendingGRNForSpecialApproval.BranchesId = (BranchFeature ? Conversion.ToInt(CmbBranch.Value) : _userAccount.BranchesId);
				base.Enabled = false;
				pendingGRNForSpecialApproval.StartPosition = FormStartPosition.CenterScreen;
				pendingGRNForSpecialApproval.BringToFront();
				pendingGRNForSpecialApproval.ShowDialog();
				base.Enabled = true;
			}
			break;
		}
		case 1056:
		{
			using (frmSpecialApprovalCommissionTradeBill frmSpecialApprovalCommissionTradeBill = new frmSpecialApprovalCommissionTradeBill(_userAccount))
			{
				frmSpecialApprovalCommissionTradeBill.DocumentTypeId = 1056;
				frmSpecialApprovalCommissionTradeBill.SpecialApproval_ReasonId = reasonId;
				frmSpecialApprovalCommissionTradeBill.BranchesId = (BranchFeature ? Conversion.ToInt(CmbBranch.Value) : _userAccount.BranchesId);
				base.Enabled = false;
				frmSpecialApprovalCommissionTradeBill.StartPosition = FormStartPosition.CenterScreen;
				frmSpecialApprovalCommissionTradeBill.BringToFront();
				frmSpecialApprovalCommissionTradeBill.ShowDialog();
				base.Enabled = true;
			}
			break;
		}
		case 4:
		{
			using (PendingJobOrderForApproval pendingJobOrderForApproval = new PendingJobOrderForApproval(_userAccount))
			{
				pendingJobOrderForApproval.SpecialApprovalId = 1;
				pendingJobOrderForApproval.ShowDialog();
			}
			break;
		}
		case 19:
		{
			CommercialInvoiceApproval commercialInvoiceApproval = new CommercialInvoiceApproval(_userAccount);
			commercialInvoiceApproval.ForSpecialApproval = true;
			commercialInvoiceApproval.BringToFront();
			commercialInvoiceApproval.ShowDialog();
			break;
		}
		case 27:
		{
			frmExportInvoiceVoucher frmExportInvoiceVoucher = new frmExportInvoiceVoucher(_userAccount);
			frmExportInvoiceVoucher.ForSpecialApproval = true;
			frmExportInvoiceVoucher.tabControl1.SelectedIndex = 1;
			frmExportInvoiceVoucher.BringToFront();
			frmExportInvoiceVoucher.ShowDialog();
			break;
		}
		}
		LoadDashboardCards();
	}

	private void Refresh_Click(object sender, EventArgs e)
	{
		LoadDashboardCards();
	}

	private void ApprovalDashboard_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error while closing the form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnApprovalDashBoard_Click(object sender, EventArgs e)
	{
		try
		{
			new Architecture.WinApp.ApprovalDashboard.ApprovalDashboard(_userAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error while closing the form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			LoadDashboardCards();
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
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.SpecialApprovalDashboard.SpecialApprovalDashboard));
		this.panel1 = new System.Windows.Forms.Panel();
		this.FilterPanel = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.BtnShow = new System.Windows.Forms.Button();
		this.CmbBranch = new UltraCombo();
		this.lblCompany = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnApprovalDashBoard = new System.Windows.Forms.ToolStripButton();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.inventoryFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.accountsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.MainPanel = new System.Windows.Forms.Panel();
		this.GroupBoxCommissionAgent = new System.Windows.Forms.GroupBox();
		this.CommissionFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.PanelInventoryAndCommission = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.FilterPanel.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		this.toolStrip3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.MainPanel.SuspendLayout();
		this.GroupBoxCommissionAgent.SuspendLayout();
		this.panel4.SuspendLayout();
		this.PanelInventoryAndCommission.SuspendLayout();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.Controls.Add(this.MainPanel);
		this.panel1.Controls.Add(this.FilterPanel);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1184, 709);
		this.panel1.TabIndex = 1;
		this.FilterPanel.BackColor = System.Drawing.Color.White;
		this.FilterPanel.Controls.Add(this.groupBox3);
		this.FilterPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.FilterPanel.Location = new System.Drawing.Point(0, 27);
		this.FilterPanel.Name = "FilterPanel";
		this.FilterPanel.Size = new System.Drawing.Size(1184, 41);
		this.FilterPanel.TabIndex = 5;
		this.groupBox3.Controls.Add(this.BtnShow);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		this.groupBox3.Controls.Add(this.lblCompany);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(0, 0);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(453, 41);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(393, 14);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(55, 23);
		this.BtnShow.TabIndex = 1;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(74, 12);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(316, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 0;
		this.lblCompany.AutoSize = true;
		this.lblCompany.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCompany.ForeColor = System.Drawing.Color.Black;
		this.lblCompany.Location = new System.Drawing.Point(10, 18);
		this.lblCompany.Name = "lblCompany";
		this.lblCompany.Size = new System.Drawing.Size(46, 15);
		this.lblCompany.TabIndex = 45;
		this.lblCompany.Text = "Branch";
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.BtnApprovalDashBoard });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1184, 27);
		this.toolStrip3.TabIndex = 2;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(80, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(Refresh_Click);
		this.BtnApprovalDashBoard.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnApprovalDashBoard.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnApprovalDashBoard.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnApprovalDashBoard.Name = "BtnApprovalDashBoard";
		this.BtnApprovalDashBoard.Size = new System.Drawing.Size(162, 24);
		this.BtnApprovalDashBoard.Text = "Approval DashBoard";
		this.BtnApprovalDashBoard.Click += new System.EventHandler(BtnApprovalDashBoard_Click);
		this.groupBox2.Controls.Add(this.inventoryFlowLayout);
		this.groupBox2.Controls.Add(this.panel2);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(590, 474);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Inventory";
		this.panel2.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 21);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(584, 30);
		this.panel2.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Cambria", 18.25f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(153, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(256, 30);
		this.label2.TabIndex = 0;
		this.label2.Text = "Inventory Dashboard";
		this.inventoryFlowLayout.AutoScroll = true;
		this.inventoryFlowLayout.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.inventoryFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.inventoryFlowLayout.Location = new System.Drawing.Point(3, 51);
		this.inventoryFlowLayout.Name = "inventoryFlowLayout";
		this.inventoryFlowLayout.Size = new System.Drawing.Size(584, 420);
		this.inventoryFlowLayout.TabIndex = 0;
		this.groupBox1.Controls.Add(this.accountsFlowLayout);
		this.groupBox1.Controls.Add(this.panel5);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(590, 641);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Accounts";
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 21);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(584, 30);
		this.panel5.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Cambria", 18.25f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(153, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(249, 30);
		this.label1.TabIndex = 0;
		this.label1.Text = "Accounts Dashboard";
		this.accountsFlowLayout.AutoScroll = true;
		this.accountsFlowLayout.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.accountsFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.accountsFlowLayout.Location = new System.Drawing.Point(3, 51);
		this.accountsFlowLayout.Name = "accountsFlowLayout";
		this.accountsFlowLayout.Size = new System.Drawing.Size(584, 587);
		this.accountsFlowLayout.TabIndex = 0;
		this.MainPanel.Controls.Add(this.PanelInventoryAndCommission);
		this.MainPanel.Controls.Add(this.groupBox1);
		this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.MainPanel.Location = new System.Drawing.Point(0, 68);
		this.MainPanel.Name = "MainPanel";
		this.MainPanel.Size = new System.Drawing.Size(1184, 641);
		this.MainPanel.TabIndex = 6;
		this.GroupBoxCommissionAgent.Controls.Add(this.CommissionFlowLayout);
		this.GroupBoxCommissionAgent.Controls.Add(this.panel4);
		this.GroupBoxCommissionAgent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.GroupBoxCommissionAgent.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GroupBoxCommissionAgent.Location = new System.Drawing.Point(0, 474);
		this.GroupBoxCommissionAgent.Name = "GroupBoxCommissionAgent";
		this.GroupBoxCommissionAgent.Size = new System.Drawing.Size(590, 167);
		this.GroupBoxCommissionAgent.TabIndex = 7;
		this.GroupBoxCommissionAgent.TabStop = false;
		this.GroupBoxCommissionAgent.Text = "Commission Agent";
		this.CommissionFlowLayout.AutoScroll = true;
		this.CommissionFlowLayout.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.CommissionFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.CommissionFlowLayout.Location = new System.Drawing.Point(3, 51);
		this.CommissionFlowLayout.Name = "CommissionFlowLayout";
		this.CommissionFlowLayout.Size = new System.Drawing.Size(584, 113);
		this.CommissionFlowLayout.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 21);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(584, 30);
		this.panel4.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(116, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(291, 23);
		this.label5.TabIndex = 0;
		this.label5.Text = "CommissionAgent Dashboard";
		this.PanelInventoryAndCommission.Controls.Add(this.GroupBoxCommissionAgent);
		this.PanelInventoryAndCommission.Controls.Add(this.groupBox2);
		this.PanelInventoryAndCommission.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelInventoryAndCommission.Location = new System.Drawing.Point(590, 0);
		this.PanelInventoryAndCommission.Name = "PanelInventoryAndCommission";
		this.PanelInventoryAndCommission.Size = new System.Drawing.Size(590, 641);
		this.PanelInventoryAndCommission.TabIndex = 8;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 709);
		base.Controls.Add(this.panel1);
		base.Name = "SpecialApprovalDashboard";
		this.Text = "Special Approval Dashboard";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ApprovalDashboard_FormClosing);
		base.Load += new System.EventHandler(ApprovalDashboard_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.FilterPanel.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.MainPanel.ResumeLayout(false);
		this.GroupBoxCommissionAgent.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.PanelInventoryAndCommission.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
