using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model.Authentication;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Lab;
using CameraControl;
using EDSDKLib;
using UserAccount = global::Architecture.Model.UserAccount;
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

public class DigitalCameraView : Form, IObserver
{
	private string fileSavePath = "";

	private string DriverPicPath = string.Empty;

	private string UniqFileName = string.Empty;

	private string saveDirectory = "";

	public static DataTable dtAttachments = new DataTable();

	public static List<TempAttachments> lst = new List<TempAttachments>();

	private CameraController _controller;

	private ActionSource _actionSource = new ActionSource();

	private List<ActionListener> _actionListenerList = new List<ActionListener>();

	private IObserver formAs;

	private Form _remoteCapture;

	private IntPtr[] _volumes = new IntPtr[2];

	private int _volume_count;

	private IContainer components;

	private Panel panel1;

	private Button Choose;

	private Panel panel2;

	private ContextMenuStrip contextMenuStrip1;

	private Button btnClose;

	private Label label2;

	private Button btnResetDriv;

	private Button LiveViewButton;

	private PictureBox LiveViewPicBox;

	private FlowLayoutPanel PicturesFlowLayout;

	public DigitalCameraView(ref CameraController controller)
	{
		formAs = this;
		InitializeComponent();
		_controller = controller;
		if (_controller == null)
		{
			return;
		}
		EDSDK.EdsGetChildCount(_controller.GetModel().Camera, out _volume_count);
		if (_volume_count > 0)
		{
			EDSDK.EdsGetChildAtIndex(_controller.GetModel().Camera, 0, out _volumes[0]);
			EDSDK.EdsGetVolumeInfo(_volumes[0], out var outVolumeInfo);
			_ = outVolumeInfo.StorageType;
			if (_volume_count > 1)
			{
				EDSDK.EdsGetChildAtIndex(_controller.GetModel().Camera, 1, out _volumes[1]);
				EDSDK.EdsGetVolumeInfo(_volumes[1], out outVolumeInfo);
				_ = outVolumeInfo.StorageType;
			}
		}
		_actionListenerList.Add(_controller);
		_actionListenerList.ForEach(delegate(ActionListener actionListener)
		{
			_actionSource.AddActionListener(ref actionListener);
		});
	}

	private void Attachment_Load(object sender, EventArgs e)
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList();
			if (list.Count > 0)
			{
				InvLabPurchaseAnalysis.saveDirectory = list[0].ConfigKey.ToString() + "\\";
				saveDirectory = list[0].ConfigKey.ToString() + "\\";
				if (!Directory.Exists(saveDirectory))
				{
					Directory.CreateDirectory(saveDirectory);
				}
			}
			else
			{
				MessageBox.Show("Attachment Folder Path not found");
			}
			if (dtAttachments.Columns.Count == 0)
			{
				dtAttachments.Columns.Add("Id");
				dtAttachments.Columns.Add("Path");
			}
			AddToGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void AddToGrid()
	{
		try
		{
			dtAttachments.Rows.Clear();
			foreach (TempAttachments item in lst)
			{
				dtAttachments.Rows.Add(item.Id, item.Path);
			}
			if (dtAttachments.Rows.Count > 0)
			{
				PicturesFlowLayout.Controls.Clear();
				AttachmentUploadControl[] AttachmentCards = new AttachmentUploadControl[dtAttachments.Rows.Count];
				for (int i = 0; i < AttachmentCards.Length; i++)
				{
					AttachmentCards[i] = new AttachmentUploadControl();
					string path = saveDirectory + Conversion.ToString(dtAttachments.Rows[i]["Path"]);
					AttachmentCards[i].path = path;
					FileInfo fileInfo = new FileInfo(path);
					AttachmentCards[i].FileName = fileInfo.Name;
					AttachmentCards[i].FileDate = fileInfo.CreationTime.ToShortDateString();
					AttachmentCards[i].FileSize = ((double)fileInfo.Length / 1048576.0).ToString("0.00") + " Mb";
					PicturesFlowLayout.Controls.Add(AttachmentCards[i]);
					AttachmentCards[i].UserControlClicked += UserControl_Click;
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
		try
		{
			AttachmentUploadControl obj = (AttachmentUploadControl)sender;
			if (obj.ActiveControl != null && obj.ActiveControl.Text == "Delete")
			{
				lst.RemoveAll((TempAttachments x) => x.Path == obj.FileName);
				PicturesFlowLayout.Controls.Remove(obj);
				using FileStream fs = new FileStream(obj.path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				fs.Flush();
				fs.Close();
				fs.Dispose();
				File.Delete(obj.path);
				MessageBox.Show("Attachment Deleted successfully");
				return;
			}
			string path = obj.path;
			if (File.Exists(path))
			{
				Process.Start(path);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Choose_Click(object sender, EventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Text files (All files (*.*)|*.*";
			openFileDialog.Multiselect = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string[] fileNames = openFileDialog.FileNames;
				foreach (string obj in fileNames)
				{
					Path.GetFileName(obj);
					string Uniq = $"{Guid.NewGuid()}.Bmp";
					fileSavePath = Path.Combine(saveDirectory, Uniq);
					File.Copy(obj, fileSavePath, overwrite: true);
					TempAttachments TA = new TempAttachments();
					TA.Id = 0;
					TA.Path = Uniq;
					lst.Add(TA);
				}
			}
			AddToGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Add_Click(object sender, EventArgs e)
	{
	}

	private void Attachment_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			if (_controller != null)
			{
				_actionSource.FireEvent(ActionEvent.Command.CLOSING, IntPtr.Zero);
				_actionListenerList.ForEach(delegate(ActionListener actionListener)
				{
					_actionSource.RemoveActionListener(ref actionListener);
				});
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		Hide();
		lst.Clear();
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		LiveViewPicBox.Image = null;
		LiveViewPicBox.Invalidate();
		Hide();
		lst.Clear();
	}

	private void btnDriverImage_Click(object sender, EventArgs e)
	{
	}

	private void btnPreview_Click(object sender, EventArgs e)
	{
		try
		{
			if (LiveViewPicBox.Image != null)
			{
				List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList();
				if (list.Count > 0)
				{
					string DrvPicPath = list[0].ConfigKey.ToString() + "\\";
					UniqFileName = $"{Guid.NewGuid()}.Bmp";
					DriverPicPath = DrvPicPath + UniqFileName;
					if (File.Exists(DriverPicPath))
					{
						LiveViewPicBox.Image = Image.FromFile(DriverPicPath);
					}
				}
				else
				{
					MessageBox.Show("Please Select the Path");
				}
			}
			else
			{
				MessageBox.Show("Please Capture the Picture first");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetDriv_Click(object sender, EventArgs e)
	{
		try
		{
			DriverPicPath = "";
			LiveViewPicBox.Image = null;
			LiveViewPicBox.Invalidate();
			UniqFileName = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Attachment_KeyDown(object sender, KeyEventArgs e)
	{
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
	}

	public void Update(Observable from, CameraEvent e)
	{
		switch (e.GetEventType())
		{
		case CameraEvent.Type.SHUT_DOWN:
			MessageBox.Show("Camera is disconnected");
			if (_remoteCapture != null && !_remoteCapture.IsDisposed)
			{
				_remoteCapture.Close();
				_remoteCapture.Dispose();
			}
			Close();
			break;
		case CameraEvent.Type.PROPERTY_CHANGED:
			if (_remoteCapture != null)
			{
				int num = (int)e.GetArg();
				if (num == 1294)
				{
					(_remoteCapture as RemoteCapture).controlFocusButton(2 != _controller.GetModel().EvfAFMode);
				}
				if (num == 16778274)
				{
					(_remoteCapture as RemoteCapture).updateFixedMovie(_controller.GetModel().FixedMovie);
				}
				if (num == 16778273)
				{
					(_remoteCapture as RemoteCapture).updateMirrorLockUpState(_controller.GetModel().MirrorLockUpState);
				}
			}
			break;
		case CameraEvent.Type.ANGLEINFO:
			if (_remoteCapture != null)
			{
				IntPtr cameraPosPtr = e.GetArg();
				if (cameraPosPtr != IntPtr.Zero)
				{
					EDSDK.EdsCameraPos obj = (EDSDK.EdsCameraPos)Marshal.PtrToStructure(cameraPosPtr, typeof(EDSDK.EdsCameraPos));
					double pos = obj.position;
					double roll = (double)obj.rolling * 0.01;
					double pitc = (double)obj.pitching * 0.01;
					(_remoteCapture as RemoteCapture).updateAngleInfoLabel(pos.ToString(), roll.ToString(), pitc.ToString());
				}
				else
				{
					EDSDK.EdsSendCommand(_controller.GetModel().Camera, 265u, 1);
					(_remoteCapture as RemoteCapture).updateAngleInfoLabel("-", "-", "-");
				}
			}
			break;
		case CameraEvent.Type.MOUSE_CURSOR:
			if (_remoteCapture == null)
			{
				break;
			}
			if (e.GetArg() != IntPtr.Zero)
			{
				(_remoteCapture as RemoteCapture).changeMouseCursor(onoff: true);
				break;
			}
			(_remoteCapture as RemoteCapture).changeMouseCursor(onoff: false);
			if (_controller.ClickType == 1)
			{
				_actionSource.FireEvent(ActionEvent.Command.CLICK_MOUSE_WB, IntPtr.Zero);
			}
			if (_controller.ClickType == 2)
			{
				_actionSource.FireEvent(ActionEvent.Command.CLICK_MOUSE_AF, IntPtr.Zero);
			}
			break;
		}
	}

	private void InvLabPurchaseAnalysis_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (_controller != null)
		{
			_actionSource.FireEvent(ActionEvent.Command.CLOSING, IntPtr.Zero);
			_actionListenerList.ForEach(delegate(ActionListener actionListener)
			{
				_actionSource.RemoveActionListener(ref actionListener);
			});
		}
	}

	private void button3_Click(object sender, EventArgs e)
	{
		if (_controller != null)
		{
			IObserver frmSetting = new CameraSetting(ref _controller, ref _volumes, ref _volume_count);
			EDSDK.EdsSendStatusCommand(_controller.GetModel().Camera, 0u, 1);
			_controller.GetModel().Add(ref frmSetting);
			((CameraSetting)frmSetting).ShowSettingDialog();
			_controller.GetModel().Remove(ref frmSetting);
			EDSDK.EdsSendStatusCommand(_controller.GetModel().Camera, 1u, 1);
		}
	}

	private void LiveViewButton_Click(object sender, EventArgs e)
	{
		try
		{
			InvLabPurchaseAnalysis.UniqPathAnalysis = "";
			if (_controller == null)
			{
				throw new Exception("Camera not connected,Checked the camera and reload the page");
			}
			_remoteCapture = new RemoteCapture(ref _controller, ref _actionSource);
			_remoteCapture.ShowDialog(this);
			_remoteCapture.Dispose();
			if (InvLabPurchaseAnalysis.saveDirectory != string.Empty)
			{
				UniqFileName = InvLabPurchaseAnalysis.UniqPathAnalysis;
				string DrvPicPath = InvLabPurchaseAnalysis.saveDirectory + UniqFileName;
				if (File.Exists(DrvPicPath))
				{
					DriverPicPath = DrvPicPath;
					Image image = Image.FromFile(DrvPicPath);
					LiveViewPicBox.Image = image;
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
		this.components = new System.ComponentModel.Container();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btnClose = new System.Windows.Forms.Button();
		this.Choose = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.PicturesFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.btnResetDriv = new System.Windows.Forms.Button();
		this.LiveViewButton = new System.Windows.Forms.Button();
		this.LiveViewPicBox = new System.Windows.Forms.PictureBox();
		this.label2 = new System.Windows.Forms.Label();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.LiveViewPicBox).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.btnClose);
		this.panel1.Controls.Add(this.Choose);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(819, 607);
		this.panel1.TabIndex = 0;
		this.btnClose.Location = new System.Drawing.Point(754, 11);
		this.btnClose.Margin = new System.Windows.Forms.Padding(2);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(56, 27);
		this.btnClose.TabIndex = 4;
		this.btnClose.Text = "Close";
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		this.Choose.Location = new System.Drawing.Point(9, 11);
		this.Choose.Margin = new System.Windows.Forms.Padding(2);
		this.Choose.Name = "Choose";
		this.Choose.Size = new System.Drawing.Size(56, 26);
		this.Choose.TabIndex = 1;
		this.Choose.Text = "Brows";
		this.Choose.UseVisualStyleBackColor = true;
		this.Choose.Click += new System.EventHandler(Choose_Click);
		this.panel2.Controls.Add(this.PicturesFlowLayout);
		this.panel2.Controls.Add(this.btnResetDriv);
		this.panel2.Controls.Add(this.LiveViewButton);
		this.panel2.Controls.Add(this.LiveViewPicBox);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel2.Location = new System.Drawing.Point(0, 47);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(819, 560);
		this.panel2.TabIndex = 0;
		this.PicturesFlowLayout.AutoScroll = true;
		this.PicturesFlowLayout.BackColor = System.Drawing.Color.White;
		this.PicturesFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PicturesFlowLayout.Location = new System.Drawing.Point(0, 0);
		this.PicturesFlowLayout.Name = "PicturesFlowLayout";
		this.PicturesFlowLayout.Size = new System.Drawing.Size(819, 560);
		this.PicturesFlowLayout.TabIndex = 52;
		this.btnResetDriv.Location = new System.Drawing.Point(96, 525);
		this.btnResetDriv.Name = "btnResetDriv";
		this.btnResetDriv.Size = new System.Drawing.Size(62, 23);
		this.btnResetDriv.TabIndex = 51;
		this.btnResetDriv.Text = "Reset";
		this.btnResetDriv.UseVisualStyleBackColor = true;
		this.btnResetDriv.Click += new System.EventHandler(btnResetDriv_Click);
		this.LiveViewButton.Location = new System.Drawing.Point(32, 525);
		this.LiveViewButton.Name = "LiveViewButton";
		this.LiveViewButton.Size = new System.Drawing.Size(62, 23);
		this.LiveViewButton.TabIndex = 49;
		this.LiveViewButton.Text = "Camera";
		this.LiveViewButton.UseVisualStyleBackColor = true;
		this.LiveViewButton.Click += new System.EventHandler(LiveViewButton_Click);
		this.LiveViewPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.LiveViewPicBox.Location = new System.Drawing.Point(32, 449);
		this.LiveViewPicBox.Name = "LiveViewPicBox";
		this.LiveViewPicBox.Size = new System.Drawing.Size(120, 70);
		this.LiveViewPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.LiveViewPicBox.TabIndex = 48;
		this.LiveViewPicBox.TabStop = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(56, 431);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(70, 15);
		this.label2.TabIndex = 47;
		this.label2.Text = "Attechment";
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(819, 620);
		base.ControlBox = false;
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "DigitalCameraView";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Attachment";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Attachment_FormClosing);
		base.Load += new System.EventHandler(Attachment_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Attachment_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.LiveViewPicBox).EndInit();
		base.ResumeLayout(false);
	}
}

}
