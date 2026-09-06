using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;

namespace Architecture.WinApp.Dashboard;

public class frmModules : Form
{
	private UserAccount UserAccount = new UserAccount();

	private string ModuleIconPath = "";

	private DashboardNew newdash;

	private DataTable dtForSK = new DataTable();

	private IContainer components;

	private Panel panel1;

	private FlowLayoutPanel accountsFlowLayout;

	private Panel panel2;

	private Label label1;

	private Panel panel3;

	public frmModules(UserAccount pUser, DashboardNew dashboardNew)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
		label1.Text = Conversion.ToString((object)UserAccount.CompName);
		newdash = dashboardNew;
	}

	private void ApprovalDashboard_Load(object sender, EventArgs e)
	{
		try
		{
			if (dtForSK.Columns.Count == 0)
			{
				dtForSK.Columns.Add("SrNo", typeof(int));
				dtForSK.Columns.Add("AppId", typeof(int));
				dtForSK.Columns.Add("App", typeof(string));
			}
			DynamicallyGenerateCardsForApp();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DynamicallyGenerateCardsForApp()
	{
		try
		{
			accountsFlowLayout.Controls.Clear();
			var modulelst = from x in clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.Value).ToList()
				group x by new { x.AppId, x.App } into lg
				select new
				{
					GroupColumn = lg.Key,
					TotalCount = lg.Count()
				};
			if (modulelst == null)
			{
				return;
			}
			ModuleDynamicallyGenerateCards[] AccountCardsList = new ModuleDynamicallyGenerateCards[modulelst.Count()];
			int i = 0;
			foreach (var Module in modulelst)
			{
				AccountCardsList[i] = new ModuleDynamicallyGenerateCards();
				AccountCardsList[i].Margin = new Padding(15, 5, 5, 5);
				AccountCardsList[i].Id = Conversion.ToString((object)Module.GroupColumn.AppId);
				AccountCardsList[i].Title = Conversion.ToString((object)Module.GroupColumn.App);
				AccountCardsList[i].Count = Conversion.ToString((object)"");
				accountsFlowLayout.Controls.Add(AccountCardsList[i]);
				AccountCardsList[i].UserControlClicked += UserControl_Click;
				dtForSK.Rows.Add(i, Conversion.ToInt((object)Module.GroupColumn.AppId), Conversion.ToString((object)Module.GroupColumn.App));
				i++;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DynamicallyGenerateCardsForModule()
	{
		try
		{
			accountsFlowLayout.Controls.Clear();
			var modulelst = from x in clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.Value).ToList()
				group x by new { x.ModuleID, x.ModuleDescription, x.IconUrl } into lg
				select new
				{
					GroupColumn = lg.Key,
					TotalCount = lg.Count()
				};
			if (modulelst == null)
			{
				return;
			}
			ModuleDynamicallyGenerateCards[] AccountCardsList = new ModuleDynamicallyGenerateCards[modulelst.Count()];
			int i = 0;
			foreach (var Module in modulelst)
			{
				AccountCardsList[i] = new ModuleDynamicallyGenerateCards();
				AccountCardsList[i].Margin = new Padding(15, 5, 5, 5);
				AccountCardsList[i].Id = Conversion.ToString((object)Module.GroupColumn.ModuleID);
				AccountCardsList[i].Title = Conversion.ToString((object)Module.GroupColumn.ModuleDescription);
				AccountCardsList[i].Count = Conversion.ToString((object)Module.TotalCount);
				if (Module.GroupColumn.IconUrl != null)
				{
					string ModuleIconUrl = ModuleIconPath;
					ModuleIconUrl = ModuleIconPath + Conversion.ToString((object)Module.GroupColumn.IconUrl);
					if (File.Exists(ModuleIconUrl))
					{
						AccountCardsList[i].Icon = new Bitmap(ModuleIconUrl);
					}
				}
				accountsFlowLayout.Controls.Add(AccountCardsList[i]);
				AccountCardsList[i].UserControlClicked += UserControl_Click;
				i++;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		ModuleDynamicallyGenerateCards objDg = (ModuleDynamicallyGenerateCards)sender;
		Conversion.ToInt((object)objDg.Id);
		frmMenue objForm = new frmMenue(UserAccount, Conversion.ToInt((object)objDg.Id), objDg.Title, newdash);
		objForm.TopLevel = false;
		objForm.AutoScroll = true;
		newdash.panel2.Controls.Add(objForm);
		HistoryStack.Push(objForm);
		objForm.Show();
	}

	private void frmModules_FormClosed(object sender, FormClosedEventArgs e)
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

	private void frmModules_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Shift && e.Alt)
			{
				int Id = 0;
				string Title = "";
				GrdPopUp grdPopUp = new GrdPopUp(dtForSK, "AppId", "App");
				grdPopUp.StartPosition = FormStartPosition.CenterScreen;
				grdPopUp.ShowDialog();
				grdPopUp.BringToFront();
				Id = Conversion.ToInt((object)grdPopUp.ReturnId);
				Title = Conversion.ToString((object)grdPopUp.ReturnName);
				if (Id > 0)
				{
					frmMenue objForm = new frmMenue(UserAccount, Id, Title, newdash);
					objForm.TopLevel = false;
					objForm.AutoScroll = true;
					newdash.panel2.Controls.Add(objForm);
					HistoryStack.Push(objForm);
					objForm.Show();
				}
			}
			if (e.Control)
			{
				_ = e.Alt;
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.accountsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1201, 686);
		this.panel1.TabIndex = 1;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.accountsFlowLayout);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 65);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1201, 621);
		this.panel3.TabIndex = 4;
		this.accountsFlowLayout.AutoScroll = true;
		this.accountsFlowLayout.BackColor = System.Drawing.Color.White;
		this.accountsFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.accountsFlowLayout.Location = new System.Drawing.Point(0, 0);
		this.accountsFlowLayout.Name = "accountsFlowLayout";
		this.accountsFlowLayout.Size = new System.Drawing.Size(1201, 621);
		this.accountsFlowLayout.TabIndex = 3;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1201, 65);
		this.panel2.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(495, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(92, 40);
		this.label1.TabIndex = 0;
		this.label1.Text = "label1";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1201, 686);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmModules";
		this.Text = "Modules";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmModules_FormClosed);
		base.Load += new System.EventHandler(ApprovalDashboard_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmModules_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}
