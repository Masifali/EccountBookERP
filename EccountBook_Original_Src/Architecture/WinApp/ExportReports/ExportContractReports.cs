using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;

namespace Architecture.WinApp.ExportReports;

public class ExportContractReports : Form
{
	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private TabControl tabControl1;

	public ExportContractReports(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void ExportContractReports_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (Focused && e.Control && e.KeyData == Keys.Right)
			{
				ShiftToNextTab();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShiftToNextTab()
	{
		try
		{
			if (tabControl1.TabPages.Count > 1)
			{
				int nextIndex = (tabControl1.SelectedIndex + 1) % tabControl1.TabPages.Count;
				tabControl1.SelectedIndex = nextIndex;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportContractReports_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1400, 700);
			List<tblUserRights> lst = clsGlobalVariables.ScreenViewReights;
			if (lst.Where((tblUserRights x) => x.ModuleDescription == "Export Reports" && x.Value).ToList().Count <= 0)
			{
				return;
			}
			List<tblUserRights> ContractRegisterRight = lst.Where((tblUserRights x) => x.ScreenName.Trim() == "SaleContractHistory").ToList();
			List<tblUserRights> list = lst.Where((tblUserRights x) => x.ScreenName.Trim() == "frmContractCurrentStatusReport").ToList();
			if (ContractRegisterRight.Count > 0)
			{
				tabControl1.TabPages.Add("Contract Register");
				ExImSaleContractRegister obj = new ExImSaleContractRegister(UserAccount);
				obj.TopLevel = false;
				obj.Visible = true;
				obj.BringToFront();
				obj.FormBorderStyle = FormBorderStyle.None;
				obj.Dock = DockStyle.Fill;
				tabControl1.TabPages[0].Controls.Add(obj);
			}
			if (list.Count > 0)
			{
				tabControl1.TabPages.Add("Contracts Current Status");
				frmContractCurrentStatusReport frm = new frmContractCurrentStatusReport(UserAccount);
				frm.TopLevel = false;
				frm.Visible = true;
				frm.FormBorderStyle = FormBorderStyle.None;
				frm.Dock = DockStyle.Fill;
				if (ContractRegisterRight.Count > 0)
				{
					tabControl1.TabPages[1].Controls.Add(frm);
				}
				else
				{
					tabControl1.TabPages[0].Controls.Add(frm);
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
		this.tabControl1 = new System.Windows.Forms.TabControl();
		base.SuspendLayout();
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(810, 638);
		this.tabControl1.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(810, 638);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "ExportContractReports";
		this.Text = "Export Contract Reports";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ExportContractReports_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExportContractReports_KeyDown);
		base.ResumeLayout(false);
	}
}
