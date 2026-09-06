using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Model;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.Dashboard;

public class OrderManagementDashboardForSale : Form
{
	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Label label1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel5;

	private FlowLayoutPanel PurchaseFlowLayout;

	private Panel panel7;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnRefresh;

	public OrderManagementDashboardForSale(UserAccount pUser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
	}

	private void OrderManagementDashboard_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(950, 750);
			GetDataAndGenerateCards();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDataAndGenerateCards()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			PurchaseFlowLayout.Controls.Clear();
			DataTable dt = Dashboard.SPU_Invetnory_OrderManagementDashBoards(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ToDate = DateTime.Now
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
				where r.Field<string>("TranType") == "Sales"
				select r;
			DataTable dtSale = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
			if (dtSale.Rows.Count <= 0)
			{
				return;
			}
			OrderManageMentGrids[] CardsList = new OrderManageMentGrids[dtSale.AsEnumerable().Max((DataRow r) => r.Field<int>("CardCount"))];
			int i;
			for (i = 0; i < CardsList.Length; i++)
			{
				CardsList[i] = new OrderManageMentGrids(UserAccount);
				DataTable dtAtI = (from r in dtSale.AsEnumerable()
					where r.Field<int>("CardCount") == i + 1
					select r).CopyToDataTable();
				CardsList[i].dataTable = dtAtI;
				PurchaseFlowLayout.Controls.Add(CardsList[i]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			GetDataAndGenerateCards();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.PurchaseFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(933, 713);
		this.panel1.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(933, 683);
		this.panel4.TabIndex = 3;
		this.panel5.Controls.Add(this.PurchaseFlowLayout);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(933, 683);
		this.panel5.TabIndex = 0;
		this.PurchaseFlowLayout.AutoScroll = true;
		this.PurchaseFlowLayout.BackColor = System.Drawing.Color.White;
		this.PurchaseFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PurchaseFlowLayout.Location = new System.Drawing.Point(0, 34);
		this.PurchaseFlowLayout.Name = "PurchaseFlowLayout";
		this.PurchaseFlowLayout.Size = new System.Drawing.Size(933, 649);
		this.PurchaseFlowLayout.TabIndex = 5;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(933, 34);
		this.panel7.TabIndex = 2;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(933, 30);
		this.panel2.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Font = new System.Drawing.Font("Tahoma", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 2);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(384, 25);
		this.label1.TabIndex = 0;
		this.label1.Text = "Sale Order Management Dashboard";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(933, 27);
		this.toolStrip1.TabIndex = 4;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "&Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(933, 713);
		base.Controls.Add(this.panel1);
		base.Name = "OrderManagementDashboardForSale";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Order Management Dashboard";
		base.Load += new System.EventHandler(OrderManagementDashboard_Load);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
