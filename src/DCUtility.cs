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
using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;


using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EOSDigital.API;
using EOSDigital.SDK;

public class DCUtility : Form
{
	public CanonAPI APIHandler;

	public Camera MainCamera;

	public CameraValue[] AvList;

	public CameraValue[] TvList;

	public CameraValue[] ISOList;

	public List<Camera> CamList;

	private bool IsInit;

	private Bitmap Evf_Bmp;

	private int LVBw;

	private int LVBh;

	private int w;

	private int h;

	private float LVBratio;

	private float LVration;

	private int ErrCount;

	private object ErrLock = new object();

	private object LvLock = new object();

	private ListBox CameraListBox;

	private ProgressBar MainProgressBar;

	private PictureBox LiveViewPicBox;

	private Button SessionButton;

	private Button LiveViewButton;

	private string DrivePath;

	public DCUtility(ListBox _CameraListBox, ProgressBar _MainProgressBar, PictureBox _LiveViewPicBox, Button _SessionButton, Button _LiveViewButton, string _DrivePath)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		CameraListBox = _CameraListBox;
		MainProgressBar = _MainProgressBar;
		LiveViewPicBox = _LiveViewPicBox;
		SessionButton = _SessionButton;
		LiveViewButton = _LiveViewButton;
		DrivePath = _DrivePath;
		APIHandler = new CanonAPI();
		APIHandler.CameraAdded += new CameraAddedHandler(APIHandler_CameraAdded);
		ErrorHandler.SevereErrorHappened += new GeneralExceptionHandler(ErrorHandler_SevereErrorHappened);
		ErrorHandler.NonSevereErrorHappened += new SDKExceptionHandler(ErrorHandler_NonSevereErrorHappened);
		LiveViewPicBox.Paint += LiveViewPicBox_Paint;
		LVBw = LiveViewPicBox.Width;
		LVBh = LiveViewPicBox.Height;
		RefreshCamera();
		IsInit = true;
	}

	public void StartLiveView(object sender, EventArgs e)
	{
		try
		{
			MainCamera.StartLiveView();
			MainCamera.SetSetting((PropertyID)11, (object)2, 0);
			MainCamera.SetCapacity(4096, int.MaxValue);
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	public void StopLiveView(object sender, EventArgs e)
	{
		try
		{
			MainCamera.StopLiveView(true);
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	public void RefreshButtonClick(object sender, EventArgs e)
	{
		try
		{
			RefreshCamera();
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	private void LiveViewPicBox_SizeChanged(object sender, EventArgs e)
	{
		try
		{
			LVBw = LiveViewPicBox.Width;
			LVBh = LiveViewPicBox.Height;
			LiveViewPicBox.Invalidate();
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	private void LiveViewPicBox_Paint(object sender, PaintEventArgs e)
	{
		if (MainCamera == null || !MainCamera.SessionOpen)
		{
			return;
		}
		if (!MainCamera.IsLiveViewOn)
		{
			e.Graphics.Clear(BackColor);
			return;
		}
		lock (LvLock)
		{
			if (Evf_Bmp != null)
			{
				LVBratio = (float)LVBw / (float)LVBh;
				LVration = (float)Evf_Bmp.Width / (float)Evf_Bmp.Height;
				if (LVBratio < LVration)
				{
					w = LVBw;
					h = (int)((float)LVBw / LVration);
				}
				else
				{
					w = (int)((float)LVBh * LVration);
					h = LVBh;
				}
				e.Graphics.DrawImage(Evf_Bmp, 0, 0, w, h);
			}
		}
	}

	private void FocusNear3Button_Click(object sender, EventArgs e)
	{
		try
		{
			MainCamera.SendCommand((CameraCommand)259, 3);
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	private void FocusNear2Button_Click(object sender, EventArgs e)
	{
		try
		{
			MainCamera.SendCommand((CameraCommand)259, 2);
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	private void FocusNear1Button_Click(object sender, EventArgs e)
	{
		try
		{
			MainCamera.SendCommand((CameraCommand)259, 1);
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	private void FocusFar1Button_Click(object sender, EventArgs e)
	{
		try
		{
			MainCamera.SendCommand((CameraCommand)259, 32769);
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	private void FocusFar2Button_Click(object sender, EventArgs e)
	{
		try
		{
			MainCamera.SendCommand((CameraCommand)259, 32770);
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	private void FocusFar3Button_Click(object sender, EventArgs e)
	{
		try
		{
			MainCamera.SendCommand((CameraCommand)259, 32771);
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	public void CloseSession()
	{
		MainCamera.CloseSession();
		DisposeSession();
		SessionButton.Text = "Open";
	}

	public void RefreshCamera()
	{
		CameraListBox.Items.Clear();
		CamList = APIHandler.GetCameraList();
		foreach (Camera cam in CamList)
		{
			CameraListBox.Items.Add(cam.DeviceName);
		}
		Camera mainCamera = MainCamera;
		if (mainCamera != null && mainCamera.SessionOpen)
		{
			CameraListBox.SelectedIndex = CamList.FindIndex((Camera t) => t.ID == MainCamera.ID);
		}
		else if (CamList.Count > 0)
		{
			CameraListBox.SelectedIndex = 0;
		}
	}

	public void DisposeSession()
	{
		try
		{
			IsInit = false;
			Camera mainCamera = MainCamera;
			if (mainCamera != null)
			{
				mainCamera.Dispose();
			}
			CanonAPI aPIHandler = APIHandler;
			if (aPIHandler != null)
			{
				aPIHandler.Dispose();
			}
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	public void OpenSession()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		if (CameraListBox.SelectedIndex >= 0)
		{
			MainCamera = CamList[CameraListBox.SelectedIndex];
			MainCamera.OpenSession();
			MainCamera.LiveViewUpdated += new LiveViewUpdate(MainCamera_LiveViewUpdated);
			MainCamera.ProgressChanged += new ProgressHandler(MainCamera_ProgressChanged);
			MainCamera.StateChanged += new StateChangeHandler(MainCamera_StateChanged);
			MainCamera.DownloadReady += new DownloadHandler(MainCamera_DownloadReady);
			SessionButton.Text = "Close Session";
		}
	}

	public void ReportError(string message, bool lockdown)
	{
		int errc;
		lock (ErrLock)
		{
			errc = ++ErrCount;
		}
		if (lockdown)
		{
			EnableUI(enable: false);
		}
		if (errc < 4)
		{
			MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		else if (errc == 4)
		{
			MessageBox.Show("Many errors happened!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		lock (ErrLock)
		{
			ErrCount--;
		}
	}

	public void EnableUI(bool enable)
	{
		if (base.InvokeRequired)
		{
			Invoke((Action)delegate
			{
				EnableUI(enable);
			});
		}
	}

	private void APIHandler_CameraAdded(CanonAPI sender)
	{
		try
		{
			Invoke((Action)delegate
			{
				RefreshCamera();
			});
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	private void MainCamera_StateChanged(Camera sender, StateEventID eventID, int parameter)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		try
		{
			if ((int)eventID == 769 && IsInit)
			{
				Invoke((Action)delegate
				{
					CloseSession();
				});
			}
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	private void MainCamera_ProgressChanged(object sender, int progress)
	{
		try
		{
			Invoke((Action)delegate
			{
				MainProgressBar.Value = progress;
			});
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	private void MainCamera_LiveViewUpdated(Camera sender, Stream img)
	{
		try
		{
			lock (LvLock)
			{
				Evf_Bmp?.Dispose();
				Evf_Bmp = new Bitmap(img);
			}
			LiveViewPicBox.Invalidate();
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	private void MainCamera_DownloadReady(Camera sender, DownloadInfo Info)
	{
		try
		{
			string dir = null;
			Invoke((Action)delegate
			{
				dir = "C:\\Users\\Administrator\\Pictures\\RemotePhoto";
			});
			sender.DownloadFile(Info);
			Invoke((Action)delegate
			{
				MainProgressBar.Value = 0;
			});
		}
		catch (Exception ex)
		{
			ReportError(ex.Message, lockdown: false);
		}
	}

	private void ErrorHandler_NonSevereErrorHappened(object sender, ErrorCode ex)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected I4, but got Unknown
		ReportError(string.Format("SDK Error code: {0} ({1})", ex, ((int)ex).ToString("X")), lockdown: false);
	}

	private void ErrorHandler_SevereErrorHappened(object sender, Exception ex)
	{
		ReportError(ex.Message, lockdown: true);
	}
}

}
