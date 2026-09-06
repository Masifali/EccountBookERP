using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Themes_ERP;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;

namespace Architecture.WinApp.Dashboard;

public class frmMenue : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int ModuleId;

	private string ModuleIconPath = "";

	private DataTable dtForSKM = new DataTable();

	private DataTable dtForSKS = new DataTable();

	private DashboardNew newdash;

	private IContainer components;

	private Panel panel1;

	private Label lblModuleName;

	private FlowLayoutPanel accountsFlowLayout;

	private Panel panel2;

	private FlowLayoutPanel ScreensFlowlayout;

	public frmMenue(UserAccount pUser, int ModId, string ModuleName, DashboardNew dashboardNew)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
		ModuleId = ModId;
		lblModuleName.Text = ModuleName;
		newdash = dashboardNew;
	}

	private void ApprovalDashboard_Load(object sender, EventArgs e)
	{
		try
		{
			if (dtForSKM.Columns.Count == 0)
			{
				dtForSKM.Columns.Add("SrNo", typeof(int));
				dtForSKM.Columns.Add("AppId", typeof(int));
				dtForSKM.Columns.Add("App", typeof(string));
			}
			if (dtForSKS.Columns.Count == 0)
			{
				dtForSKS.Columns.Add("SrNo", typeof(int));
				dtForSKS.Columns.Add("ScreenId", typeof(int));
				dtForSKS.Columns.Add("ScreenName", typeof(string));
				dtForSKS.Columns.Add("ScreenAlias", typeof(string));
				dtForSKS.Columns.Add("TargetUrl", typeof(string));
				dtForSKS.Columns.Add("IsFavorite", typeof(bool));
			}
			DynamicallyGenerateCardsForModule();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DynamicallyGenerateCards(int ModId)
	{
		try
		{
			dtForSKS.Clear();
			ScreensFlowlayout.Controls.Clear();
			List<tblUserRights> lst = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ModuleID == ModId && x.Value).ToList();
			if (lst.Count > 0)
			{
				ScreenDynamicallyGenerateCards[] AccountCardsList = new ScreenDynamicallyGenerateCards[lst.Count];
				for (int i = 0; i < lst.Count; i++)
				{
					AccountCardsList[i] = new ScreenDynamicallyGenerateCards(UserAccount);
					AccountCardsList[i].Margin = new Padding(10, 5, 5, 5);
					AccountCardsList[i].IsFavorite = Conversion.ToBool((object)lst[i].IsFavorite);
					AccountCardsList[i].ScreenId = Conversion.ToInt((object)lst[i].ScreenID);
					AccountCardsList[i].ModuleId = Conversion.ToInt((object)lst[i].ModuleID);
					AccountCardsList[i].Id = Conversion.ToString((object)lst[i].TargetUrl);
					AccountCardsList[i].Title = Conversion.ToString((object)lst[i].ScreenAlias);
					AccountCardsList[i].ScreenName = Conversion.ToString((object)lst[i].ScreenName);
					ScreensFlowlayout.Controls.Add(AccountCardsList[i]);
					AccountCardsList[i].UserControlClicked += UserControl_Click1;
					dtForSKS.Rows.Add(i, Conversion.ToInt((object)lst[i].ScreenID), Conversion.ToString((object)lst[i].ScreenName), Conversion.ToString((object)lst[i].ScreenAlias), Conversion.ToString((object)lst[i].TargetUrl), Conversion.ToBool((object)lst[i].IsFavorite));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserControl_Click1(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			ScreenDynamicallyGenerateCards obj = (ScreenDynamicallyGenerateCards)sender;
			string TargetUrl = obj.Id;
			if (string.IsNullOrEmpty(TargetUrl))
			{
				throw new Exception("Target URL not Configured properly... " + obj.Title);
			}
			Type typ = Type.GetType(TargetUrl);
			if (typ != null)
			{
				UserAccount.UserAccountAllocationsList = new List<UserAccountAllocation>();
				UserAccountAllocation allocation = new UserAccountAllocation();
				allocation.ScreenName = obj.ScreenName;
				UserAccount.UserAccountAllocationsList.Add(allocation);
				Form existingForm = null;
				if (existingForm != null)
				{
					existingForm.BringToFront();
					existingForm.WindowState = FormWindowState.Maximized;
					return;
				}
				Form form = null;
				form = ((TargetUrl == "Architecture.WinApp.DashBoard.Reports") ? (Activator.CreateInstance(typ, UserAccount, newdash) as Form) : (Activator.CreateInstance(typ, UserAccount) as Form));
				if (form != null)
				{
					form.TopLevel = false;
					form.Text = obj.Title;
					form.Tag = obj.ScreenName;
					form.WindowState = FormWindowState.Maximized;
					newdash.panel2.Controls.Add(form);
					HistoryStack.Push(form);
					form.Show();
				}
				return;
			}
			throw new Exception("Target URL not Configured properly... " + obj.Title);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApplyThemeToAllControls(Control parent)
	{
		try
		{
			foreach (Control c in parent.Controls)
			{
				if (c is Panel panel && (panel.BackColor == Color.Teal || panel.BackColor == Color.FromArgb(10, 110, 110)))
				{
					ThemeSettings themeSettings = clsGlobalVariables.GetThemeSettings.FirstOrDefault((ThemeSettings row) => row.ScreenName == "Screens");
					if (themeSettings != null)
					{
						panel.BackColor = ColorTranslator.FromHtml(themeSettings.HeaderPanel_BackColor);
						panel.ForeColor = ColorTranslator.FromHtml(themeSettings.HeaderPanel_FontColor);
					}
				}
				else if (c is Button button && (button.BackColor == Color.Teal || button.BackColor == Color.FromArgb(10, 110, 110)))
				{
					ThemeSettings themeSettings2 = clsGlobalVariables.GetThemeSettings.FirstOrDefault((ThemeSettings row) => row.ScreenName == "Screens");
					if (themeSettings2 != null)
					{
						button.BackColor = ColorTranslator.FromHtml(themeSettings2.Button_BackColor);
						button.ForeColor = ColorTranslator.FromHtml(themeSettings2.Button_FontColor);
					}
				}
				if ((c is Form || c is Panel || c is Button) && c.HasChildren)
				{
					ApplyThemeToAllControls(c);
				}
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
			dtForSKM.Clear();
			List<tblUserRights> lst = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.AppId == ModuleId && x.Value).ToList();
			if (lst.Count <= 0)
			{
				return;
			}
			var modulelst = from x in lst
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
				accountsFlowLayout.Controls.Add(AccountCardsList[i]);
				AccountCardsList[i].UserControlClicked += UserControl_Click;
				if (modulelst.Count() == 1)
				{
					UserControl_Click(AccountCardsList[i], null);
				}
				dtForSKM.Rows.Add(i, Conversion.ToInt((object)Module.GroupColumn.ModuleID), Conversion.ToString((object)Module.GroupColumn.ModuleDescription));
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
		try
		{
			ModuleDynamicallyGenerateCards obj = (ModuleDynamicallyGenerateCards)sender;
			DynamicallyGenerateCards(Conversion.ToInt((object)obj.Id));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmMenue_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.E)
			{
				Close();
			}
			if (e.Shift && e.KeyCode == Keys.M)
			{
				int Id = 0;
				GrdPopUp grdPopUp = new GrdPopUp(dtForSKM, "AppId", "App");
				grdPopUp.StartPosition = FormStartPosition.CenterScreen;
				grdPopUp.ShowDialog();
				grdPopUp.BringToFront();
				Id = Conversion.ToInt((object)grdPopUp.ReturnId);
				Conversion.ToString((object)grdPopUp.ReturnName);
				if (Id > 0)
				{
					DynamicallyGenerateCards(Id);
				}
			}
			if (e.Shift && e.KeyCode == Keys.S)
			{
				int Id2 = 0;
				GrdPopUp obj = new GrdPopUp(dtForSKS, "ScreenId", "ScreenAlias");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id2 = Conversion.ToInt((object)obj.ReturnId);
				if (Id2 > 0)
				{
					DataRow foundRow = dtForSKS.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ScreenId") == Id2);
					string TargetUrl = ((foundRow != null) ? Conversion.ToString(foundRow["TargetUrl"]) : "");
					string Title = ((foundRow != null) ? Conversion.ToString(foundRow["ScreenAlias"]) : "");
					string ScreenName = ((foundRow != null) ? Conversion.ToString(foundRow["ScreenName"]) : "");
					if (TargetUrl == string.Empty)
					{
						throw new Exception("Target URL not Configured properly... " + Title);
					}
					if (!(Type.GetType(TargetUrl) != null))
					{
						throw new Exception("Target URL not Configured properly... " + Title);
					}
					if (Activator.CreateInstance(Type.GetType(TargetUrl), UserAccount) is Form form)
					{
						form.TopLevel = false;
						form.Text = Title;
						form.Tag = ScreenName;
						form.WindowState = FormWindowState.Maximized;
						newdash.panel2.Controls.Add(form);
						HistoryStack.Push(form);
						ApplyThemeToAllControls(form);
						form.Show();
					}
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

	private void frmMenue_FormClosed(object sender, FormClosedEventArgs e)
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.ScreensFlowlayout = new System.Windows.Forms.FlowLayoutPanel();
		this.accountsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.lblModuleName = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.Controls.Add(this.ScreensFlowlayout);
		this.panel1.Controls.Add(this.accountsFlowLayout);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1112, 654);
		this.panel1.TabIndex = 1;
		this.ScreensFlowlayout.AutoScroll = true;
		this.ScreensFlowlayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ScreensFlowlayout.Location = new System.Drawing.Point(0, 224);
		this.ScreensFlowlayout.Name = "ScreensFlowlayout";
		this.ScreensFlowlayout.Size = new System.Drawing.Size(1112, 430);
		this.ScreensFlowlayout.TabIndex = 4;
		this.accountsFlowLayout.AutoScroll = true;
		this.accountsFlowLayout.Dock = System.Windows.Forms.DockStyle.Top;
		this.accountsFlowLayout.Location = new System.Drawing.Point(0, 53);
		this.accountsFlowLayout.Name = "accountsFlowLayout";
		this.accountsFlowLayout.Size = new System.Drawing.Size(1112, 171);
		this.accountsFlowLayout.TabIndex = 3;
		this.panel2.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel2.Controls.Add(this.lblModuleName);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1112, 53);
		this.panel2.TabIndex = 2;
		this.lblModuleName.AutoSize = true;
		this.lblModuleName.Font = new System.Drawing.Font("Cambria", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModuleName.ForeColor = System.Drawing.Color.White;
		this.lblModuleName.Location = new System.Drawing.Point(11, 10);
		this.lblModuleName.Name = "lblModuleName";
		this.lblModuleName.Size = new System.Drawing.Size(109, 32);
		this.lblModuleName.TabIndex = 0;
		this.lblModuleName.Text = "Screens";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1112, 654);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmMenue";
		this.Text = "Screens";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmMenue_FormClosed);
		base.Load += new System.EventHandler(ApprovalDashboard_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmMenue_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}
