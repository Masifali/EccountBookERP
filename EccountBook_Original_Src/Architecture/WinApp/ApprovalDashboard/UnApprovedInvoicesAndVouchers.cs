using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Cmagt.Approvals;
using Architecture.WinApp.DynamicCards;

namespace Architecture.WinApp.ApprovalDashboard;

public class UnApprovedInvoicesAndVouchers : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DataTable dtGrid = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel6;

	private Panel panel5;

	private Panel panel7;

	private Label label1;

	private Panel panel8;

	private Label label2;

	private FlowLayoutPanel inventoryFlowLayout;

	private FlowLayoutPanel accountsFlowLayout;

	private Panel FilterPanel;

	private GroupBox groupBox3;

	private DateTimePicker datToDate;

	private Label label3;

	private DateTimePicker datFromDate;

	private Label label33;

	private Button BtnShow;

	private Panel PanelExportAndCommissionAgent;

	private FlowLayoutPanel flowLayoutPanel1;

	private Panel panel9;

	private Label label5;

	public UnApprovedInvoicesAndVouchers(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void frmSaleTaxSummaryRpt_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1450, 700);
			datFromDate.Value = DateTime.Now.AddDays(-7.0);
			datToDate.Value = DateTime.Now;
			DynamicallyGenerateCards();
		}
		catch (Exception)
		{
		}
	}

	private void DynamicallyGenerateCards()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		try
		{
			accountsFlowLayout.Controls.Clear();
			inventoryFlowLayout.Controls.Clear();
			flowLayoutPanel1.Controls.Clear();
			ReportsParameters obj = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				UserId = UserAccount.ID
			};
			if (datFromDate.Checked)
			{
				obj.FromDate = datFromDate.Value;
			}
			if (datToDate.Checked)
			{
				obj.ToDate = datToDate.Value;
			}
			DataTable source = Dashboard.AccountsForApprovedVouchers(obj);
			DataTable dtInventory = Dashboard.InventoryForApprovedVouchers(obj);
			EnumerableRowCollection<DataRow> accountRows = source.AsEnumerable();
			RenderCards(accountRows, accountsFlowLayout, (DataRow r) => r["VoucherType"]?.ToString());
			EnumerableRowCollection<DataRow> inventoryRows = from r in dtInventory.AsEnumerable()
				where Conversion.ToInt(r["ActionId"]) != 2
				select r;
			RenderCards(inventoryRows, inventoryFlowLayout, (DataRow r) => r["Description"]?.ToString());
			EnumerableRowCollection<DataRow> commissionRows = from r in dtInventory.AsEnumerable()
				where Conversion.ToInt(r["ActionId"]) == 2
				select r;
			RenderCards(commissionRows, flowLayoutPanel1, (DataRow r) => r["Description"]?.ToString());
			PanelExportAndCommissionAgent.Visible = commissionRows.Any();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		void RenderCards(IEnumerable<DataRow> rows, FlowLayoutPanel panel, Func<DataRow, string> titleSelector)
		{
			foreach (DataRow row in rows)
			{
				DynamicallyGenerateCards card = new DynamicallyGenerateCards
				{
					Id = row["TypeID"]?.ToString(),
					Title = titleSelector(row),
					Count = row["Count"]?.ToString()
				};
				card.UserControlClicked += UserControl_Click;
				panel.Controls.Add(card);
			}
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		int id = Conversion.ToInt((object)((DynamicallyGenerateCards)sender).Id);
		if ((id >= 0 && id <= 9) || id == 34 || id == 35)
		{
			PendingApprovalVouchersHistory obj = new PendingApprovalVouchersHistory(UserAccount);
			PendingApprovalVouchersHistory.Id = id;
			obj.flagAproved = true;
			if (datFromDate.Checked)
			{
				obj.RequestedFromDate = datFromDate.Value;
			}
			obj.RequestedToDate = datToDate.Value;
			obj.ShowDialog();
			obj.BringToFront();
		}
		else
		{
			switch (id)
			{
			case 23:
			{
				PendingApprovalPurchaseInvoice obj2 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 56;
				obj2.flagUnApprove = true;
				if (datFromDate.Checked)
				{
					obj2.RequestedFromDate = datFromDate.Value;
				}
				obj2.RequestedToDate = datToDate.Value;
				obj2.ShowDialog();
				obj2.BringToFront();
				break;
			}
			case 24:
			{
				PendingApprovalPurchaseInvoice obj3 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 57;
				obj3.flagUnApprove = true;
				if (datFromDate.Checked)
				{
					obj3.RequestedFromDate = datFromDate.Value;
				}
				obj3.RequestedToDate = datToDate.Value;
				obj3.ShowDialog();
				obj3.BringToFront();
				break;
			}
			case 32:
			{
				PendingApprovalSaleInvoice obj4 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 95;
				obj4.flagUP = true;
				if (datFromDate.Checked)
				{
					obj4.RequestedFromDate = datFromDate.Value;
				}
				obj4.RequestedToDate = datToDate.Value;
				obj4.ShowDialog();
				obj4.BringToFront();
				break;
			}
			case 33:
			{
				PendingApprovalSaleInvoice obj5 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 99;
				obj5.flagUP = true;
				if (datFromDate.Checked)
				{
					obj5.RequestedFromDate = datFromDate.Value;
				}
				obj5.RequestedToDate = datToDate.Value;
				obj5.ShowDialog();
				obj5.BringToFront();
				break;
			}
			case 36:
			{
				PendingApprovalPurchaseInvoice obj6 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 98;
				obj6.flagUnApprove = true;
				if (datFromDate.Checked)
				{
					obj6.RequestedFromDate = datFromDate.Value;
				}
				obj6.RequestedToDate = datToDate.Value;
				obj6.ShowDialog();
				obj6.BringToFront();
				break;
			}
			case 37:
			{
				PendingGRNForApproval_Engr obj7 = new PendingGRNForApproval_Engr(UserAccount);
				obj7.flagUnApprove = true;
				if (datFromDate.Checked)
				{
					obj7.RequestedFromDate = datFromDate.Value;
				}
				obj7.RequestedToDate = datToDate.Value;
				obj7.ShowDialog();
				obj7.BringToFront();
				break;
			}
			case 38:
			{
				PendingGDNForApproval_Engr obj8 = new PendingGDNForApproval_Engr(UserAccount);
				obj8.flagUnApprove = true;
				if (datFromDate.Checked)
				{
					obj8.RequestedFromDate = datFromDate.Value;
				}
				obj8.RequestedToDate = datToDate.Value;
				obj8.ShowDialog();
				obj8.BringToFront();
				break;
			}
			case 39:
			{
				PendingApprovalPurchaseInvoice obj9 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 702;
				obj9.flagUnApprove = true;
				if (datFromDate.Checked)
				{
					obj9.RequestedFromDate = datFromDate.Value;
				}
				obj9.RequestedToDate = datToDate.Value;
				obj9.ShowDialog();
				obj9.BringToFront();
				break;
			}
			case 62:
			{
				PendingJobOrderForApproval pendingJobOrderForApproval = new PendingJobOrderForApproval(UserAccount);
				pendingJobOrderForApproval.UnApprove = true;
				pendingJobOrderForApproval.ShowDialog();
				pendingJobOrderForApproval.BringToFront();
				break;
			}
			case 112:
			{
				frmProductionOutPutApprovalOrUnApproval obj10 = new frmProductionOutPutApprovalOrUnApproval(UserAccount);
				if (datFromDate.Checked)
				{
					obj10.RequestedFromDate = datFromDate.Value;
				}
				obj10.RequestedToDate = datToDate.Value;
				obj10.flagUnApprove = true;
				obj10.ShowDialog();
				obj10.BringToFront();
				break;
			}
			case 90:
			{
				frmDayBookApproval obj22 = new frmDayBookApproval(UserAccount);
				obj22.IsApproved = true;
				obj22.ShowDialog();
				obj22.BringToFront();
				break;
			}
			case 245:
			{
				PendingApprovalPurchaseInvoice obj11 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 245;
				obj11.flagUnApprove = true;
				if (datFromDate.Checked)
				{
					obj11.RequestedFromDate = datFromDate.Value;
				}
				obj11.RequestedToDate = datToDate.Value;
				obj11.ShowDialog();
				obj11.BringToFront();
				break;
			}
			case 142:
			{
				PendingFreightVoucherForApproval obj12 = new PendingFreightVoucherForApproval(UserAccount);
				obj12.DocumentTypeId = 28;
				obj12.flagUnApprove = true;
				if (datFromDate.Checked)
				{
					obj12.RequestedFromDate = datFromDate.Value;
				}
				obj12.RequestedToDate = datToDate.Value;
				obj12.ShowDialog();
				obj12.BringToFront();
				break;
			}
			case 1870:
			{
				frmWorkOrderApproval obj13 = new frmWorkOrderApproval(UserAccount);
				obj13.DocumentTypeId = 1870;
				obj13.flagUnApprove = true;
				if (datFromDate.Checked)
				{
					obj13.RequestedFromDate = datFromDate.Value;
				}
				obj13.RequestedToDate = datToDate.Value;
				obj13.ShowDialog();
				obj13.BringToFront();
				break;
			}
			case 188:
			{
				PendingStorePurchaseDemandForApproval obj14 = new PendingStorePurchaseDemandForApproval(UserAccount);
				obj14.ParentCategoryId = 8;
				obj14.flagUnApprove = true;
				if (datFromDate.Checked)
				{
					obj14.RequestedFromDate = datFromDate.Value;
				}
				obj14.RequestedToDate = datToDate.Value;
				obj14.ShowDialog();
				obj14.BringToFront();
				break;
			}
			case 141:
			{
				PendingStorePurchaseDemandForApproval obj15 = new PendingStorePurchaseDemandForApproval(UserAccount);
				obj15.ParentCategoryId = 7;
				obj15.flagUnApprove = true;
				if (datFromDate.Checked)
				{
					obj15.RequestedFromDate = datFromDate.Value;
				}
				obj15.RequestedToDate = datToDate.Value;
				obj15.ShowDialog();
				obj15.BringToFront();
				break;
			}
			case 138:
			{
				PendingApprovalPurchaseInvoice obj16 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 138;
				obj16.flagUnApprove = true;
				if (datFromDate.Checked)
				{
					obj16.RequestedFromDate = datFromDate.Value;
				}
				obj16.RequestedToDate = datToDate.Value;
				obj16.ShowDialog();
				obj16.BringToFront();
				break;
			}
			case 145:
			{
				PendingApprovalSaleInvoice obj17 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 145;
				obj17.flagUP = true;
				obj17.BaseDocumentTypeId = 1;
				if (datFromDate.Checked)
				{
					obj17.RequestedFromDate = datFromDate.Value;
				}
				obj17.RequestedToDate = datToDate.Value;
				obj17.ShowDialog();
				obj17.BringToFront();
				break;
			}
			case 146:
			{
				PendingApprovalSaleInvoice obj18 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 145;
				obj18.BaseDocumentTypeId = 2;
				obj18.flagUP = true;
				if (datFromDate.Checked)
				{
					obj18.RequestedFromDate = datFromDate.Value;
				}
				obj18.RequestedToDate = datToDate.Value;
				obj18.ShowDialog();
				obj18.BringToFront();
				break;
			}
			case 1053:
			{
				frmApprovalSaleOrderCmagt obj19 = new frmApprovalSaleOrderCmagt(UserAccount);
				obj19.flagUnApprove = true;
				obj19.DocumentTypeId = 1053;
				if (datFromDate.Checked)
				{
					obj19.RequestedFromDate = datFromDate.Value;
				}
				obj19.RequestedToDate = datToDate.Value;
				obj19.ShowDialog();
				obj19.BringToFront();
				break;
			}
			case 1052:
			{
				frmApprovalPurchaseOrderCmagt obj20 = new frmApprovalPurchaseOrderCmagt(UserAccount);
				obj20.flagUnApprove = true;
				obj20.DocumentTypeId = 1052;
				if (datFromDate.Checked)
				{
					obj20.RequestedFromDate = datFromDate.Value;
				}
				obj20.RequestedToDate = datToDate.Value;
				obj20.ShowDialog();
				obj20.BringToFront();
				break;
			}
			case 1056:
			{
				frmApprovalCommissionTradeBill obj21 = new frmApprovalCommissionTradeBill(UserAccount);
				obj21.flagUnApprove = true;
				obj21.DocumentTypeId = 1056;
				if (datFromDate.Checked)
				{
					obj21.RequestedFromDate = datFromDate.Value;
				}
				obj21.RequestedToDate = datToDate.Value;
				obj21.ShowDialog();
				obj21.BringToFront();
				break;
			}
			}
		}
		DynamicallyGenerateCards();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		DynamicallyGenerateCards();
	}

	private void UnApprovedInvoicesAndVouchers_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			DynamicallyGenerateCards();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.UnApprovedInvoicesAndVouchers));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.PanelExportAndCommissionAgent = new System.Windows.Forms.Panel();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.inventoryFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.accountsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.FilterPanel = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.BtnShow = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.PanelExportAndCommissionAgent.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		this.FilterPanel.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1434, 721);
		this.panel1.TabIndex = 3;
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Controls.Add(this.FilterPanel);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 31);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1434, 690);
		this.panel3.TabIndex = 4;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.PanelExportAndCommissionAgent);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 44);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1434, 646);
		this.panel4.TabIndex = 2;
		this.PanelExportAndCommissionAgent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelExportAndCommissionAgent.Controls.Add(this.flowLayoutPanel1);
		this.PanelExportAndCommissionAgent.Controls.Add(this.panel9);
		this.PanelExportAndCommissionAgent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelExportAndCommissionAgent.Location = new System.Drawing.Point(1173, 0);
		this.PanelExportAndCommissionAgent.Name = "PanelExportAndCommissionAgent";
		this.PanelExportAndCommissionAgent.Size = new System.Drawing.Size(261, 646);
		this.PanelExportAndCommissionAgent.TabIndex = 7;
		this.flowLayoutPanel1.AutoScroll = true;
		this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 30);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(259, 614);
		this.flowLayoutPanel1.TabIndex = 0;
		this.panel9.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel9.Controls.Add(this.label5);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(259, 30);
		this.panel9.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Cambria", 15.25f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(116, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(302, 25);
		this.label5.TabIndex = 0;
		this.label5.Text = "CommissionAgent Dashboard";
		this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel6.Controls.Add(this.inventoryFlowLayout);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(586, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(587, 646);
		this.panel6.TabIndex = 1;
		this.inventoryFlowLayout.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.inventoryFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.inventoryFlowLayout.Location = new System.Drawing.Point(0, 30);
		this.inventoryFlowLayout.Name = "inventoryFlowLayout";
		this.inventoryFlowLayout.Size = new System.Drawing.Size(585, 614);
		this.inventoryFlowLayout.TabIndex = 4;
		this.panel8.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel8.Controls.Add(this.label2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(585, 30);
		this.panel8.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Cambria", 15.25f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(99, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(340, 25);
		this.label2.TabIndex = 0;
		this.label2.Text = "Inventory UnApproval Dashboard";
		this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel5.Controls.Add(this.accountsFlowLayout);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(586, 646);
		this.panel5.TabIndex = 0;
		this.accountsFlowLayout.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.accountsFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.accountsFlowLayout.Location = new System.Drawing.Point(0, 30);
		this.accountsFlowLayout.Name = "accountsFlowLayout";
		this.accountsFlowLayout.Size = new System.Drawing.Size(584, 614);
		this.accountsFlowLayout.TabIndex = 3;
		this.panel7.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(584, 30);
		this.panel7.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Cambria", 15.25f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(80, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(336, 25);
		this.label1.TabIndex = 0;
		this.label1.Text = "Accounts UnApproval Dashboard";
		this.FilterPanel.BackColor = System.Drawing.Color.White;
		this.FilterPanel.Controls.Add(this.groupBox3);
		this.FilterPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.FilterPanel.Location = new System.Drawing.Point(0, 0);
		this.FilterPanel.Name = "FilterPanel";
		this.FilterPanel.Size = new System.Drawing.Size(1434, 44);
		this.FilterPanel.TabIndex = 7;
		this.groupBox3.Controls.Add(this.datToDate);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.datFromDate);
		this.groupBox3.Controls.Add(this.label33);
		this.groupBox3.Controls.Add(this.BtnShow);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(0, 0);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(469, 44);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(269, 14);
		this.datToDate.Name = "datToDate";
		this.datToDate.ShowCheckBox = true;
		this.datToDate.Size = new System.Drawing.Size(137, 23);
		this.datToDate.TabIndex = 131;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(215, 18);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 15);
		this.label3.TabIndex = 133;
		this.label3.Text = "To Date";
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(73, 14);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.ShowCheckBox = true;
		this.datFromDate.Size = new System.Drawing.Size(137, 23);
		this.datFromDate.TabIndex = 130;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(7, 18);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(63, 15);
		this.label33.TabIndex = 132;
		this.label33.Text = "From Date";
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(408, 14);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(55, 23);
		this.BtnShow.TabIndex = 1;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1434, 31);
		this.panel2.TabIndex = 3;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNew });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1434, 27);
		this.toolStrip3.TabIndex = 1;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1434, 721);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "UnApprovedInvoicesAndVouchers";
		this.Text = "UnApproved Vouchers";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(UnApprovedInvoicesAndVouchers_FormClosing);
		base.Load += new System.EventHandler(frmSaleTaxSummaryRpt_Load);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.PanelExportAndCommissionAgent.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.FilterPanel.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
