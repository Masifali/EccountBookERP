using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp
{
using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;


using UserAccount = global::Architecture.Model.UserAccount;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using global::Architecture.Model;
using AxWMPLib;

public class frmPlayVideo : Form
{
	private UserAccount UserAccount = new UserAccount();

	public string VideoFileName = "";

	private IContainer components;

	private Panel PanMainplayvideo;

	private AxWindowsMediaPlayer axWindowsMediaPlayer1;

	private Panel panel2;

	private Label label2;

	public frmPlayVideo(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmScreenDefinetion_Load(object sender, EventArgs e)
	{
		try
		{
			PlayVideo(VideoFileName);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PlayVideo(string videoFileName)
	{
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.frmPlayVideo));
		this.axWindowsMediaPlayer1 = new AxWindowsMediaPlayer();
		this.PanMainplayvideo = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.axWindowsMediaPlayer1).BeginInit();
		this.PanMainplayvideo.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		((System.Windows.Forms.Control)(object)this.axWindowsMediaPlayer1).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.AxHost)(object)this.axWindowsMediaPlayer1).Enabled = true;
		((System.Windows.Forms.Control)(object)this.axWindowsMediaPlayer1).Location = new System.Drawing.Point(0, 33);
		((System.Windows.Forms.Control)(object)this.axWindowsMediaPlayer1).Name = "axWindowsMediaPlayer1";
		((System.Windows.Forms.AxHost)(object)this.axWindowsMediaPlayer1).OcxState = (System.Windows.Forms.AxHost.State)resources.GetObject("axWindowsMediaPlayer1.OcxState");
		((System.Windows.Forms.Control)(object)this.axWindowsMediaPlayer1).Size = new System.Drawing.Size(984, 428);
		((System.Windows.Forms.Control)(object)this.axWindowsMediaPlayer1).TabIndex = 2;
		this.PanMainplayvideo.Controls.Add((System.Windows.Forms.Control)(object)this.axWindowsMediaPlayer1);
		this.PanMainplayvideo.Controls.Add(this.panel2);
		this.PanMainplayvideo.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanMainplayvideo.Location = new System.Drawing.Point(0, 0);
		this.PanMainplayvideo.Name = "PanMainplayvideo";
		this.PanMainplayvideo.Size = new System.Drawing.Size(984, 461);
		this.PanMainplayvideo.TabIndex = 3;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(984, 33);
		this.panel2.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(63, 19);
		this.label2.TabIndex = 28;
		this.label2.Text = "Videos";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(984, 461);
		base.Controls.Add(this.PanMainplayvideo);
		base.KeyPreview = true;
		base.Name = "frmPlayVideo";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmScreenDefinetion_Load);
		((System.ComponentModel.ISupportInitialize)this.axWindowsMediaPlayer1).EndInit();
		this.PanMainplayvideo.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
