using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.DashBoard;

public class Reports : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DashboardNew newdash;

	private IContainer components;

	private Panel panel1;

	private Label label1;

	private Panel panel2;

	private Panel panel3;

	private FlowLayoutPanel ReportsFlowLayout;

	private PictureBox btnRefresh;

	public Reports(UserAccount pUser, DashboardNew dashboardNew)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
		newdash = dashboardNew;
	}

	private void Reports_Load(object sender, EventArgs e)
	{
		try
		{
			DynamicallyGenerateFavCards();
			DynamicallyGenerateReportsCards();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DynamicallyGenerateFavCards()
	{
		try
		{
			panel2.Controls.Clear();
			FlowLayoutWithDescriptionAndScreens FavCard = new FlowLayoutWithDescriptionAndScreens();
			FavCard.ModuleName = "Favourites";
			FavCard.flowLayoutPanel1.Controls.Clear();
			List<tblUserRights> lstFav = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.IsFavorite).ToList();
			ScreenNameDescriptionIsFavourite[] ScreenCard = new ScreenNameDescriptionIsFavourite[lstFav.Count];
			for (int i = 0; i < lstFav.Count; i++)
			{
				ScreenCard[i] = new ScreenNameDescriptionIsFavourite(UserAccount);
				ScreenCard[i].IsFavorite = Conversion.ToBool((object)lstFav[i].IsFavorite);
				ScreenCard[i].ScreenId = Conversion.ToInt((object)lstFav[i].ScreenID);
				ScreenCard[i].ModuleId = Conversion.ToInt((object)lstFav[i].ModuleID);
				ScreenCard[i].Id = Conversion.ToString((object)lstFav[i].TargetUrl);
				ScreenCard[i].ScreenName = Conversion.ToString((object)lstFav[i].ScreenName);
				ScreenCard[i].Title = Conversion.ToString((object)lstFav[i].ScreenAlias);
				ScreenCard[i].TitleDescription = Conversion.ToString((object)lstFav[i].ModuleDescription);
				FavCard.flowLayoutPanel1.Controls.Add(ScreenCard[i]);
				ScreenCard[i].UserControlClicked += UserControl_Click;
			}
			FavCard.Width = panel2.Width;
			FavCard.Height = panel2.Height;
			FavCard.flowLayoutPanel1.Height = panel2.Height - FavCard.panel1.Height;
			FavCard.flowLayoutPanel1.Width = panel2.Width;
			FavCard.flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
			FavCard.flowLayoutPanel1.AutoScroll = true;
			panel2.AutoScroll = true;
			panel2.Controls.Add(FavCard);
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
			ScreenNameDescriptionIsFavourite.ControlClickedEventArgs obj2 = (ScreenNameDescriptionIsFavourite.ControlClickedEventArgs)e;
			ScreenNameDescriptionIsFavourite obj = (ScreenNameDescriptionIsFavourite)sender;
			if (obj2.ClickedControl.Name == "pictureBox1")
			{
				DynamicallyGenerateFavCards();
				DynamicallyGenerateReportsCards();
				return;
			}
			string TargetUrl = obj.Id;
			if (TargetUrl == string.Empty)
			{
				throw new Exception("Target URL not Configured properly... " + obj.Title);
			}
			if (Type.GetType(TargetUrl) != null)
			{
				Type form = Type.GetType(TargetUrl);
				if (form != null && Activator.CreateInstance(form, UserAccount) is Form frm)
				{
					frm.TopLevel = false;
					frm.Text = obj.Title;
					frm.Tag = obj.ScreenName;
					frm.WindowState = FormWindowState.Maximized;
					newdash.panel2.Controls.Add(frm);
					HistoryStack.Push(frm);
					frm.Show();
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

	private void DynamicallyGenerateReportsCards()
	{
		try
		{
			ReportsFlowLayout.Controls.Clear();
			List<tblUserRights> lstReports = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ModuleTypeId == 2).ToList();
			IEnumerable<int> DistinctIds = lstReports.Select((tblUserRights item) => item.ModuleID).Distinct();
			if (DistinctIds.ToList().Count <= 0)
			{
				return;
			}
			FlowLayoutWithDescriptionAndScreens[] FavCard = new FlowLayoutWithDescriptionAndScreens[DistinctIds.ToList().Count];
			int i;
			for (i = 0; i < DistinctIds.ToList().Count; i++)
			{
				List<tblUserRights> lstModuleReports = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ModuleID == Conversion.ToInt((object)DistinctIds.ToList()[i])).ToList();
				if (lstModuleReports.Count <= 0)
				{
					continue;
				}
				FavCard[i] = new FlowLayoutWithDescriptionAndScreens();
				FavCard[i].ModuleName = lstModuleReports[0].ModuleDescription;
				FavCard[i].flowLayoutPanel1.Controls.Clear();
				ScreenNameDescriptionIsFavourite[] ScreenCard = new ScreenNameDescriptionIsFavourite[lstModuleReports.Count];
				for (int j = 0; j < lstModuleReports.Count; j++)
				{
					ScreenCard[j] = new ScreenNameDescriptionIsFavourite(UserAccount);
					ScreenCard[j].IsFavorite = Conversion.ToBool((object)lstModuleReports[j].IsFavorite);
					ScreenCard[j].ScreenId = Conversion.ToInt((object)lstModuleReports[j].ScreenID);
					ScreenCard[j].ModuleId = Conversion.ToInt((object)lstModuleReports[j].ModuleID);
					ScreenCard[j].Id = Conversion.ToString((object)lstModuleReports[j].TargetUrl);
					ScreenCard[j].ScreenName = Conversion.ToString((object)lstModuleReports[j].ScreenName);
					ScreenCard[j].Title = Conversion.ToString((object)lstModuleReports[j].ScreenAlias);
					ScreenCard[j].TitleDescription = Conversion.ToString((object)lstModuleReports[j].ModuleDescription);
					ScreenCard[j].UserControlClicked += UserControl_Click;
					FavCard[i].flowLayoutPanel1.Controls.Add(ScreenCard[j]);
					if (lstModuleReports.Count > 5)
					{
						FavCard[i].Height = 338;
						FavCard[i].flowLayoutPanel1.Height = 300;
					}
					else if (j > 0)
					{
						FavCard[i].Height += 57;
						FavCard[i].flowLayoutPanel1.Height += 62;
					}
					else if (j == 0)
					{
						FavCard[i].Height = 95;
						FavCard[i].flowLayoutPanel1.Height = 57;
					}
				}
				if (FavCard[i].flowLayoutPanel1.Controls.Count > 5)
				{
					FavCard[i].flowLayoutPanel1.AutoScroll = true;
				}
				ReportsFlowLayout.Controls.Add(FavCard[i]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reports_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			DynamicallyGenerateFavCards();
			DynamicallyGenerateReportsCards();
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
		this.btnRefresh = new System.Windows.Forms.PictureBox();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ReportsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnRefresh).BeginInit();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.btnRefresh);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1066, 32);
		this.panel1.TabIndex = 0;
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.Location = new System.Drawing.Point(117, 4);
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(25, 25);
		this.btnRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
		this.btnRefresh.TabIndex = 1;
		this.btnRefresh.TabStop = false;
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(9, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(73, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Reports";
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 32);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1066, 230);
		this.panel2.TabIndex = 1;
		this.panel3.Controls.Add(this.ReportsFlowLayout);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 262);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1066, 401);
		this.panel3.TabIndex = 2;
		this.ReportsFlowLayout.AutoScroll = true;
		this.ReportsFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ReportsFlowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		this.ReportsFlowLayout.Location = new System.Drawing.Point(0, 0);
		this.ReportsFlowLayout.Name = "ReportsFlowLayout";
		this.ReportsFlowLayout.Size = new System.Drawing.Size(1066, 401);
		this.ReportsFlowLayout.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1066, 663);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Name = "Reports";
		this.Text = "Reports";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Reports_FormClosing);
		base.Load += new System.EventHandler(Reports_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnRefresh).EndInit();
		this.panel3.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
