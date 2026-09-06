using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.WeightBridge
{

public class frmLiveStreaming : Form
{
	private HVUtility _cameraUtility;

	private IContainer components;

	private PictureBox _fullScreenPictureBox;

	public frmLiveStreaming(string CameraIp, short Port, string UserName, string Password)
	{
		InitializeComponent();
		_cameraUtility = new HVUtility(this, _fullScreenPictureBox);
		base.WindowState = FormWindowState.Maximized;
		base.FormBorderStyle = FormBorderStyle.None;
		_fullScreenPictureBox.Dock = DockStyle.Fill;
		_fullScreenPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
		StartPreview(CameraIp, Port, UserName, Password);
	}

	private void StartPreview(string CameraIp, short Port, string UserName, string Password)
	{
		if (!string.IsNullOrEmpty(CameraIp) && Port != 0 && !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
		{
			_cameraUtility.ConnectCamera(CameraIp, Port, UserName, Password);
			_cameraUtility.Preview();
			return;
		}
		Close();
		throw new Exception("Camera 01 Connection Error");
	}

	private void frmLiveStreaming_KeyDown(object sender, KeyEventArgs e)
	{
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
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
		this._fullScreenPictureBox = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this._fullScreenPictureBox).BeginInit();
		base.SuspendLayout();
		this._fullScreenPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
		this._fullScreenPictureBox.Location = new System.Drawing.Point(0, 0);
		this._fullScreenPictureBox.Name = "_fullScreenPictureBox";
		this._fullScreenPictureBox.Size = new System.Drawing.Size(800, 450);
		this._fullScreenPictureBox.TabIndex = 0;
		this._fullScreenPictureBox.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 450);
		base.Controls.Add(this._fullScreenPictureBox);
		base.Name = "frmLiveStreaming";
		this.Text = "frmLiveStreaming";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmLiveStreaming_KeyDown);
		((System.ComponentModel.ISupportInitialize)this._fullScreenPictureBox).EndInit();
		base.ResumeLayout(false);
	}
}

}
