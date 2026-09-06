using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Steel.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.ApprovalDashboard;
using Architecture.WinApp.DynamicCards;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Steel.Approvals
{

public class ApprovalDashboard_St : Form
{
	private UserAccount UserAccount = new UserAccount();

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

	public ApprovalDashboard_St(UserAccount pUser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
	}

	private void ApprovalDashboard_Load(object sender, EventArgs e)
	{
		DynamicallyGenerateCards();
	}

	private void DynamicallyGenerateCards()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0085: Expected O, but got Unknown
		try
		{
			accountsFlowLayout.Controls.Clear();
			inventoryFlowLayout.Controls.Clear();
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				UserId = UserAccount.ID
			};
			DataTable dtAccounts = Dashboard.ReadAccountsDashboardPendingForApproval(val);
			DataTable dtInventory = PurchaseOrder.ReadInventoryDashboardPendingForApprovalforSteel(val);
			if (dtAccounts.Rows.Count > 0)
			{
				DynamicallyGenerateCards[] AccountCardsList = new DynamicallyGenerateCards[dtAccounts.Rows.Count];
				for (int j = 0; j < AccountCardsList.Length; j++)
				{
					AccountCardsList[j] = new DynamicallyGenerateCards();
					AccountCardsList[j].Id = Conversion.ToString(dtAccounts.Rows[j]["TypeID"]);
					AccountCardsList[j].Title = Conversion.ToString(dtAccounts.Rows[j]["VoucherType"]);
					AccountCardsList[j].Count = Conversion.ToString(dtAccounts.Rows[j]["Count"]);
					accountsFlowLayout.Controls.Add(AccountCardsList[j]);
					AccountCardsList[j].UserControlClicked += UserControl_Click;
				}
			}
			if (dtInventory.Rows.Count > 0)
			{
				DynamicallyGenerateCards[] InventoryCardsList = new DynamicallyGenerateCards[dtInventory.Rows.Count];
				for (int i = 0; i < InventoryCardsList.Length; i++)
				{
					InventoryCardsList[i] = new DynamicallyGenerateCards();
					InventoryCardsList[i].Id = Conversion.ToString(dtInventory.Rows[i]["TypeID"]);
					InventoryCardsList[i].Title = Conversion.ToString(dtInventory.Rows[i]["Description"]);
					InventoryCardsList[i].Count = Conversion.ToString(dtInventory.Rows[i]["Count"]);
					inventoryFlowLayout.Controls.Add(InventoryCardsList[i]);
					InventoryCardsList[i].UserControlClicked += UserControl_Click;
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
		int id = Conversion.ToInt((object)((DynamicallyGenerateCards)sender).Id);
		if (id >= 0 && id <= 9)
		{
			PendingApprovalVouchersHistory pendingApprovalVouchersHistory = new PendingApprovalVouchersHistory(UserAccount);
			PendingApprovalVouchersHistory.Id = id;
			pendingApprovalVouchersHistory.ShowDialog();
			pendingApprovalVouchersHistory.BringToFront();
		}
		else
		{
			switch (id)
			{
			case 20:
			{
				PurchaseOrderForApproval_St purchaseOrderForApproval_St = new PurchaseOrderForApproval_St(UserAccount);
				purchaseOrderForApproval_St.ShowDialog();
				purchaseOrderForApproval_St.BringToFront();
				break;
			}
			case 27:
			{
				SaleInvoiceForApproval_St saleInvoiceForApproval_St2 = new SaleInvoiceForApproval_St(UserAccount);
				SaleInvoiceForApproval_St.DocumentTypeId = 1509;
				saleInvoiceForApproval_St2.ShowDialog();
				saleInvoiceForApproval_St2.BringToFront();
				break;
			}
			case 28:
			{
				SaleInvoiceForApproval_St saleInvoiceForApproval_St = new SaleInvoiceForApproval_St(UserAccount);
				SaleInvoiceForApproval_St.DocumentTypeId = 1510;
				saleInvoiceForApproval_St.ShowDialog();
				saleInvoiceForApproval_St.BringToFront();
				break;
			}
			case 25:
			{
				SaleOrderForApproval_St saleOrderForApproval_St = new SaleOrderForApproval_St(UserAccount);
				saleOrderForApproval_St.DocumentTypeId = 1505;
				saleOrderForApproval_St.ShowDialog();
				saleOrderForApproval_St.BringToFront();
				break;
			}
			case 30:
			{
				DeliveryOrderForApproval_St deliveryOrderForApproval_St = new DeliveryOrderForApproval_St(UserAccount);
				deliveryOrderForApproval_St.DocumentTypeId = 1506;
				deliveryOrderForApproval_St.DeliveryOrderTypeId = 1;
				deliveryOrderForApproval_St.ShowDialog();
				deliveryOrderForApproval_St.BringToFront();
				break;
			}
			}
		}
		DynamicallyGenerateCards();
	}

	private void Refresh_Click(object sender, EventArgs e)
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

	private void ApprovalDashboard_St_FormClosing(object sender, FormClosingEventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Steel.Approvals.ApprovalDashboard_St));
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.inventoryFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.accountsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel1.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel5.SuspendLayout();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1201, 749);
		this.panel1.TabIndex = 1;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNew });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1195, 27);
		this.toolStrip3.TabIndex = 2;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(Refresh_Click);
		this.groupBox2.Controls.Add(this.panel2);
		this.groupBox2.Controls.Add(this.inventoryFlowLayout);
		this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(599, 30);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(596, 768);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Inventory";
		this.panel2.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 21);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(590, 46);
		this.panel2.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Cambria", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(153, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(272, 32);
		this.label2.TabIndex = 0;
		this.label2.Text = "Inventory Dashboard";
		this.inventoryFlowLayout.AutoScroll = true;
		this.inventoryFlowLayout.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.inventoryFlowLayout.Location = new System.Drawing.Point(3, 73);
		this.inventoryFlowLayout.Name = "inventoryFlowLayout";
		this.inventoryFlowLayout.Size = new System.Drawing.Size(588, 713);
		this.inventoryFlowLayout.TabIndex = 0;
		this.groupBox1.Controls.Add(this.panel5);
		this.groupBox1.Controls.Add(this.accountsFlowLayout);
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 30);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(592, 768);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Accounts";
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 21);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(586, 46);
		this.panel5.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Cambria", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(153, 7);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(267, 32);
		this.label1.TabIndex = 0;
		this.label1.Text = "Accounts Dashboard";
		this.accountsFlowLayout.AutoScroll = true;
		this.accountsFlowLayout.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.accountsFlowLayout.Location = new System.Drawing.Point(3, 73);
		this.accountsFlowLayout.Name = "accountsFlowLayout";
		this.accountsFlowLayout.Size = new System.Drawing.Size(585, 713);
		this.accountsFlowLayout.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1201, 749);
		base.Controls.Add(this.panel1);
		base.Name = "ApprovalDashboard_St";
		this.Text = "Approval Dashboard";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ApprovalDashboard_St_FormClosing);
		base.Load += new System.EventHandler(ApprovalDashboard_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
