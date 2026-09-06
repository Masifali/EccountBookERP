using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.DynamicCards;

namespace Architecture.WinApp.ApprovalDashboard;

public class PendingDocumentForMultipleApprovals : Form
{
	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Label label2;

	private Panel panel5;

	private Label label1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private GroupBox groupBox1;

	private GroupBox groupBox3;

	private FlowLayoutPanel inventoryFlowLayout;

	private FlowLayoutPanel accountsFlowLayout;

	public PendingDocumentForMultipleApprovals(UserAccount pUser)
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
		//IL_005d: Expected O, but got Unknown
		try
		{
			accountsFlowLayout.Controls.Clear();
			inventoryFlowLayout.Controls.Clear();
			DataTable dt = Dashboard.GetPendingDocumentApprovalsForDashboard(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
				where r.Field<string>("TransType") == "Accounts"
				select r;
			DataTable dtAccounts = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
			if (dtAccounts.Rows.Count > 0)
			{
				DynamicallyGenerateCards[] AccountCardsList = new DynamicallyGenerateCards[dtAccounts.Rows.Count];
				for (int j = 0; j < AccountCardsList.Length; j++)
				{
					AccountCardsList[j] = new DynamicallyGenerateCards();
					AccountCardsList[j].Id = Conversion.ToString(dtAccounts.Rows[j]["RefDocumentTypeId"]);
					AccountCardsList[j].TargetUrl = Conversion.ToString(dtAccounts.Rows[j]["TargetUrl"]);
					AccountCardsList[j].Title = Conversion.ToString(dtAccounts.Rows[j]["DocumentName"]);
					AccountCardsList[j].Count = Conversion.ToString(dtAccounts.Rows[j]["NoofApprovals"]);
					accountsFlowLayout.Controls.Add(AccountCardsList[j]);
					AccountCardsList[j].UserControlClicked += UserControl_Click;
				}
			}
			EnumerableRowCollection<DataRow> rowsForInventory = from r in dt.AsEnumerable()
				where r.Field<string>("TransType") == "Inventory"
				select r;
			DataTable dtInventory = (rowsForInventory.Any() ? rowsForInventory.CopyToDataTable() : dt.Clone());
			if (dtInventory.Rows.Count > 0)
			{
				DynamicallyGenerateCards[] InventoryCardsList = new DynamicallyGenerateCards[dtInventory.Rows.Count];
				for (int i = 0; i < InventoryCardsList.Length; i++)
				{
					InventoryCardsList[i] = new DynamicallyGenerateCards();
					InventoryCardsList[i].Id = Conversion.ToString(dtInventory.Rows[i]["RefDocumentTypeId"]);
					InventoryCardsList[i].TargetUrl = Conversion.ToString(dtInventory.Rows[i]["TargetUrl"]);
					InventoryCardsList[i].Title = Conversion.ToString(dtInventory.Rows[i]["DocumentName"]);
					InventoryCardsList[i].Count = Conversion.ToString(dtInventory.Rows[i]["NoofApprovals"]);
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
		DynamicallyGenerateCards objDg = (DynamicallyGenerateCards)sender;
		Conversion.ToInt((object)objDg.Id);
		string TargetUrl = objDg.TargetUrl;
		if (TargetUrl == string.Empty)
		{
			throw new Exception("Target URL not Configured properly... " + objDg.Title);
		}
		if (Type.GetType(TargetUrl) != null)
		{
			if (Activator.CreateInstance(Type.GetType(TargetUrl), UserAccount) is Form form)
			{
				form.Tag = objDg.Id;
				form.ShowDialog();
				form.BringToFront();
			}
			DynamicallyGenerateCards();
			return;
		}
		throw new Exception("Target URL not Configured properly... " + objDg.Title);
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

	private void PendingDocumentForMultipleApprovals_FormClosing(object sender, FormClosingEventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.PendingDocumentForMultipleApprovals));
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.inventoryFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.accountsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.panel5.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Controls.Add(this.groupBox3);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1217, 749);
		this.panel1.TabIndex = 1;
		this.groupBox1.Controls.Add(this.inventoryFlowLayout);
		this.groupBox1.Controls.Add(this.panel2);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(616, 27);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(592, 722);
		this.groupBox1.TabIndex = 4;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Inventory";
		this.inventoryFlowLayout.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.inventoryFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.inventoryFlowLayout.Location = new System.Drawing.Point(3, 67);
		this.inventoryFlowLayout.Margin = new System.Windows.Forms.Padding(0);
		this.inventoryFlowLayout.Name = "inventoryFlowLayout";
		this.inventoryFlowLayout.Size = new System.Drawing.Size(586, 652);
		this.inventoryFlowLayout.TabIndex = 3;
		this.panel2.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 21);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(586, 46);
		this.panel2.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Cambria", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(153, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(272, 32);
		this.label2.TabIndex = 0;
		this.label2.Text = "Inventory Dashboard";
		this.groupBox3.Controls.Add(this.accountsFlowLayout);
		this.groupBox3.Controls.Add(this.panel5);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(0, 27);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(616, 722);
		this.groupBox3.TabIndex = 3;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Accounts";
		this.accountsFlowLayout.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.accountsFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.accountsFlowLayout.Location = new System.Drawing.Point(3, 67);
		this.accountsFlowLayout.Name = "accountsFlowLayout";
		this.accountsFlowLayout.Size = new System.Drawing.Size(610, 652);
		this.accountsFlowLayout.TabIndex = 2;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 21);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(610, 46);
		this.panel5.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Cambria", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(153, 7);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(267, 32);
		this.label1.TabIndex = 0;
		this.label1.Text = "Accounts Dashboard";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNew });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1217, 27);
		this.toolStrip3.TabIndex = 2;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(Refresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1217, 749);
		base.Controls.Add(this.panel1);
		base.Name = "PendingDocumentForMultipleApprovals";
		this.Text = "Approval Dashboard";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PendingDocumentForMultipleApprovals_FormClosing);
		base.Load += new System.EventHandler(ApprovalDashboard_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
