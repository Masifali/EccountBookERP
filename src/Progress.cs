using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Threading;
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
namespace CameraControl
{

public class Progress : Form, IObserver
{
	private delegate void _Update(CameraEvent e);

	private CameraController _controller;

	private Progress frmProgress;

	private int _max;

	private int _val;

	private IContainer components;

	private System.Windows.Forms.ProgressBar progressBar1;

	private Label label2;

	private Button button2;

	private Label label1;

	public Progress getFormProgress => frmProgress;

	protected override CreateParams CreateParams
	{
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		get
		{
			CreateParams obj = base.CreateParams;
			obj.ClassStyle |= 512;
			return obj;
		}
	}

	public Progress(ref CameraController controller)
	{
		InitializeComponent();
		frmProgress = this;
		_controller = controller;
	}

	~Progress()
	{
	}

	public void Maximum(int max)
	{
		Maximum(max);
	}

	public void Update(Observable from, CameraEvent e)
	{
		Update(e);
	}

	private void Update(CameraEvent e)
	{
		if (base.InvokeRequired)
		{
			Invoke(new _Update(Update), e);
			return;
		}
		switch (e.GetEventType())
		{
		case CameraEvent.Type.DOWNLOAD_START:
			_max = (int)e.GetArg();
			_val = 0;
			frmProgress.progressBar1.Maximum = _max;
			frmProgress.progressBar1.Value = _val;
			frmProgress.label2.Text = "File Downloading ...";
			_controller.GetModel()._ExecuteStatus = CameraModel.Status.DOWNLOADING;
			frmProgress.label1.Text = _val + " / " + _max;
			break;
		case CameraEvent.Type.DELETE_START:
			_max = (int)e.GetArg();
			_val = 0;
			frmProgress.progressBar1.Maximum = _max;
			frmProgress.progressBar1.Value = _val;
			frmProgress.label2.Text = "File Deleting ...";
			_controller.GetModel()._ExecuteStatus = CameraModel.Status.DELETEING;
			frmProgress.label1.Text = _val + " / " + _max;
			break;
		case CameraEvent.Type.PROGRESS_REPORT:
			_val++;
			frmProgress.progressBar1.Value = _val;
			frmProgress.progressBar1.Invalidate();
			frmProgress.label1.Text = _val + " / " + _max;
			break;
		case CameraEvent.Type.DOWNLOAD_COMPLETE:
		case CameraEvent.Type.DELETE_COMPLETE:
			frmProgress.Refresh();
			Thread.Sleep(500);
			frmProgress.Close();
			frmProgress.Dispose();
			_controller.GetModel()._ExecuteStatus = CameraModel.Status.NONE;
			break;
		case CameraEvent.Type.SHUT_DOWN:
			_controller.GetModel()._ExecuteStatus = CameraModel.Status.CANCELING;
			frmProgress.label2.Text = "Canceling ...";
			frmProgress.Invalidate();
			break;
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		_controller.GetModel()._ExecuteStatus = CameraModel.Status.CANCELING;
		frmProgress.label2.Text = "Canceling ...";
		frmProgress.Invalidate();
	}

	private void FormProgress_FormClosing(object sender, FormClosingEventArgs e)
	{
		_controller.GetModel()._ExecuteStatus = CameraModel.Status.CANCELING;
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
		this.progressBar1 = new System.Windows.Forms.ProgressBar();
		this.label2 = new System.Windows.Forms.Label();
		this.button2 = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.progressBar1.Location = new System.Drawing.Point(69, 103);
		this.progressBar1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
		this.progressBar1.Name = "progressBar1";
		this.progressBar1.Size = new System.Drawing.Size(463, 34);
		this.progressBar1.TabIndex = 9;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(60, 52);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(151, 18);
		this.label2.TabIndex = 10;
		this.label2.Text = "File Downloading ...";
		this.button2.Location = new System.Drawing.Point(432, 205);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(171, 42);
		this.button2.TabIndex = 11;
		this.button2.Text = "Cancel";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(257, 150);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(45, 18);
		this.label1.TabIndex = 10;
		this.label1.Text = "0 / 0";
		base.AutoScaleDimensions = new System.Drawing.SizeF(10f, 18f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(637, 259);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.progressBar1);
		base.Name = "FormProgress";
		this.Text = "Camera Control";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FormProgress_FormClosing);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
