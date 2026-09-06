// using CameraControl;
// using EDSDKLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model.Authentication;
using Architecture.Model.VPS;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Properties;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Account_Definition
{

public class Attachment : Form
{
	public List<TempAttachments> RemovedAttachmentListInUpdateCase = new List<TempAttachments>();

	private bool IsVpsAttachmentsServiceOn;

	public List<TempAttachments> lst = new List<TempAttachments>();

	public static DataTable dtAttachments = new DataTable();

	private string fileSavePath = "";

	private string DriverPicPath = string.Empty;

	private string UniqFileName = string.Empty;

	public string saveDirectory = "";

	// private CameraController _controller;

	// private ActionSource _actionSource;

	// private List<ActionListener> _actionListenerList;

	// private object formAs;

	private dynamic _controller = null;
	private dynamic _remoteCapture = null;
	private dynamic _actionSource = null;


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

	private Label lblPercent;

	private System.Windows.Forms.ProgressBar progressBar1;

	private BackgroundWorker backgroundWorker;

	public Attachment()
	{
		InitializeComponent();
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
			IsVpsAttachmentsServiceOn = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("IsVpsAttachmentsServiceOn"));
			if (dtAttachments.Columns.Count == 0)
			{
				dtAttachments.Columns.Add("Id");
				dtAttachments.Columns.Add("Path");
				dtAttachments.Columns.Add("CustomFileName");
				dtAttachments.Columns.Add("ActionId");
			}
			saveDirectory = clsGlobalVariables.AttachmentFolderPath;
			lblPercent.Visible = false;
			progressBar1.Visible = false;
			AddToGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void AddToGrid()
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		try
		{
			dtAttachments.Rows.Clear();
			foreach (TempAttachments item in lst)
			{
				dtAttachments.Rows.Add(item.Id, item.Path, item.CustomFileName, item.ActionId);
			}
			if (lst.Count <= 0)
			{
				return;
			}
			PicturesFlowLayout.Controls.Clear();
			Choose.Enabled = false;
			PicturesFlowLayout.Enabled = false;
			backgroundWorker.RunWorkerAsync();
			progressBar1.Show();
			ServerCredentials Sc = new ServerCredentials();
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			AttachmentUploadControl[] AttachmentCards = new AttachmentUploadControl[lst.Count];
			for (int i = 0; i < AttachmentCards.Length; i++)
			{
				AttachmentCards[i] = new AttachmentUploadControl();
				string FileName = ((lst[i].CustomFileName == null || lst[i].CustomFileName == string.Empty) ? Path.GetFileName(lst[i].Path) : lst[i].CustomFileName);
				AttachmentCards[i].FileName = Path.GetFileName(lst[i].Path);
				AttachmentCards[i].ActionId = lst[i].ActionId;
				if (!IsVpsAttachmentsServiceOn)
				{
					string DestinationPath = Path.Combine(saveDirectory, FileName);
					FileInfo fileInfo = null;
					if (File.Exists(DestinationPath))
					{
						AttachmentCards[i].path = DestinationPath;
						fileInfo = new FileInfo(DestinationPath);
					}
					else
					{
						AttachmentCards[i].path = lst[i].Path;
						fileInfo = new FileInfo(Conversion.ToString((object)lst[i].Path));
						if (Conversion.ToInt((object)lst[i].ActionId) == 2 && !fileInfo.Exists)
						{
							continue;
						}
					}
					if (fileInfo.Extension.ToUpper() == ".JPG" || fileInfo.Extension.ToUpper() == ".JPEG" || fileInfo.Extension.ToUpper() == ".PNG" || fileInfo.Extension.ToUpper() == ".GIF" || fileInfo.Extension.ToUpper() == ".BMP" || fileInfo.Extension.ToUpper() == ".JFIF")
					{
						using MemoryStream stream = new MemoryStream();
						Image image = Image.FromFile(AttachmentCards[i].path);
						Bitmap bmp = new Bitmap(image);
						bmp.Save(stream, image.RawFormat);
						AttachmentCards[i].PictureBox.Image = bmp;
						image.Dispose();
						stream.Position = 0L;
						stream.Dispose();
					}
					else if (fileInfo.Extension.ToUpper() == ".XLSX" || fileInfo.Extension.ToUpper() == ".XLSM" || fileInfo.Extension.ToUpper() == ".XLS" || fileInfo.Extension.ToUpper() == ".XLSB" || fileInfo.Extension.ToUpper() == ".XLTX")
					{
						AttachmentCards[i].PictureBox.Image = Resources.Microsoft_Excel_Icon;
					}
					else if (fileInfo.Extension.ToUpper() == ".DOCX" || fileInfo.Extension.ToUpper() == ".DOC")
					{
						AttachmentCards[i].PictureBox.Image = Resources.Microsoft_Word_Logo;
					}
					else if (fileInfo.Extension.ToUpper() == ".PDF")
					{
						AttachmentCards[i].PictureBox.Image = Resources.Pdf_Logo;
					}
					else if (fileInfo.Extension.ToUpper() == ".TXT")
					{
						AttachmentCards[i].PictureBox.Image = Resources.txtFile_Logo;
					}
					else if (fileInfo.Extension.ToUpper() == ".ZIP" || fileInfo.Extension.ToUpper() == ".RAR")
					{
						AttachmentCards[i].PictureBox.Image = Resources.Zip_Logo;
					}
					else
					{
						AttachmentCards[i].PictureBox.Image = Resources.No_Preview;
					}
					AttachmentCards[i].FileDate = fileInfo.CreationTime.ToShortDateString();
					AttachmentCards[i].FileSize = ((double)fileInfo.Length / 1048576.0).ToString("0.00") + " Mb";
					PicturesFlowLayout.Controls.Add(AttachmentCards[i]);
					AttachmentCards[i].UserControlClicked += UserControl_Click;
					continue;
				}
				string fileExtension = "";
				if (Conversion.ToInt((object)lst[i].ActionId) == 2)
				{
					string uri = "ftp://" + Sc.ServerIp + "/" + FileName;
					try
					{
						FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
						request.Method = "RETR";
						request.UsePassive = true;
						request.UseBinary = true;
						request.KeepAlive = false;
						request.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using FtpWebResponse ftpWebResponse = (FtpWebResponse)request.GetResponse();
						using Stream responseStream = ftpWebResponse.GetResponseStream();
						using MemoryStream memoryStream = new MemoryStream();
						responseStream.CopyTo(memoryStream);
						double fileSizeInMb = (double)memoryStream.Length / 1048576.0;
						AttachmentCards[i].FileSize = fileSizeInMb.ToString("0.000") + " Mb";
						fileExtension = Path.GetExtension(Path.GetFileName(request.RequestUri.LocalPath));
						memoryStream.Position = 0L;
						if (fileExtension.ToUpper() == ".JPG" || fileExtension.ToUpper() == ".JPEG" || fileExtension.ToUpper() == ".PNG" || fileExtension.ToUpper() == ".GIF" || fileExtension.ToUpper() == ".BMP" || fileExtension.ToUpper() == ".JFIF")
						{
							using MemoryStream memoryStream2 = new MemoryStream();
							Image image2 = Image.FromStream(memoryStream);
							Bitmap bmp2 = new Bitmap(image2);
							bmp2.Save(memoryStream2, image2.RawFormat);
							AttachmentCards[i].PictureBox.Image = bmp2;
							image2.Dispose();
							memoryStream2.Position = 0L;
							memoryStream2.Dispose();
						}
						else if (fileExtension.ToUpper() == ".XLSX" || fileExtension.ToUpper() == ".XLSM" || fileExtension.ToUpper() == ".XLS" || fileExtension.ToUpper() == ".XLSB" || fileExtension.ToUpper() == ".XLTX")
						{
							AttachmentCards[i].PictureBox.Image = Resources.Microsoft_Excel_Icon;
						}
						else if (fileExtension.ToUpper() == ".DOCX" || fileExtension.ToUpper() == ".DOC")
						{
							AttachmentCards[i].PictureBox.Image = Resources.Microsoft_Word_Logo;
						}
						else if (fileExtension.ToUpper() == ".PDF")
						{
							AttachmentCards[i].PictureBox.Image = Resources.Pdf_Logo;
						}
						else if (fileExtension.ToUpper() == ".TXT")
						{
							AttachmentCards[i].PictureBox.Image = Resources.txtFile_Logo;
						}
						else if (fileExtension.ToUpper() == ".ZIP" || fileExtension.ToUpper() == ".RAR")
						{
							AttachmentCards[i].PictureBox.Image = Resources.Zip_Logo;
						}
						else
						{
							AttachmentCards[i].PictureBox.Image = Resources.No_Preview;
						}
						byte[] streamData = memoryStream.ToArray();
						AttachmentCards[i].ByteArray = streamData;
						AttachmentCards[i].FileDate = DateTime.Now.ToShortDateString();
						PicturesFlowLayout.Controls.Add(AttachmentCards[i]);
						AttachmentCards[i].UserControlClicked += UserControl_Click;
					}
					catch (WebException webEx)
					{
						if (!(webEx.Response is FtpWebResponse { StatusCode: FtpStatusCode.ActionNotTakenFileUnavailable }))
						{
							Console.WriteLine("[FTP ERROR] " + webEx.Message);
						}
						continue;
					}
					catch (Exception ex)
					{
						throw new Exception("Error: " + ex.Message);
					}
					AttachmentCards[i].path = Conversion.ToString((object)lst[i].CustomFileName);
					continue;
				}
				FileInfo fileInfo2 = null;
				AttachmentCards[i].path = Conversion.ToString((object)lst[i].Path);
				fileInfo2 = new FileInfo(Conversion.ToString((object)lst[i].Path));
				if (fileInfo2.Extension.ToUpper() == ".JPG" || fileInfo2.Extension.ToUpper() == ".JPEG" || fileInfo2.Extension.ToUpper() == ".PNG" || fileInfo2.Extension.ToUpper() == ".GIF" || fileInfo2.Extension.ToUpper() == ".JFIF")
				{
					using FileStream fs = new FileStream(AttachmentCards[i].path, FileMode.Open, FileAccess.Read);
					using Image original = Image.FromStream(fs);
					if (AttachmentCards[i].PictureBox.Image != null)
					{
						AttachmentCards[i].PictureBox.Image.Dispose();
					}
					AttachmentCards[i].PictureBox.Image = new Bitmap(original);
				}
				else if (fileInfo2.Extension.ToUpper() == ".XLSX" || fileInfo2.Extension.ToUpper() == ".XLSM" || fileInfo2.Extension.ToUpper() == ".XLS" || fileInfo2.Extension.ToUpper() == ".XLSB" || fileInfo2.Extension.ToUpper() == ".XLTX")
				{
					AttachmentCards[i].PictureBox.Image = Resources.Microsoft_Excel_Icon;
				}
				else if (fileInfo2.Extension.ToUpper() == ".DOCX" || fileInfo2.Extension.ToUpper() == ".DOC")
				{
					AttachmentCards[i].PictureBox.Image = Resources.Microsoft_Word_Logo;
				}
				else if (fileInfo2.Extension.ToUpper() == ".PDF")
				{
					AttachmentCards[i].PictureBox.Image = Resources.Pdf_Logo;
				}
				else if (fileInfo2.Extension.ToUpper() == ".TXT")
				{
					AttachmentCards[i].PictureBox.Image = Resources.txtFile_Logo;
				}
				else if (fileInfo2.Extension.ToUpper() == ".ZIP" || fileInfo2.Extension.ToUpper() == ".RAR")
				{
					AttachmentCards[i].PictureBox.Image = Resources.Zip_Logo;
				}
				else
				{
					AttachmentCards[i].PictureBox.Image = Resources.No_Preview;
				}
				AttachmentCards[i].FileDate = fileInfo2.CreationTime.ToShortDateString();
				AttachmentCards[i].FileSize = ((double)fileInfo2.Length / 1048576.0).ToString("0.00") + " Mb";
				PicturesFlowLayout.Controls.Add(AttachmentCards[i]);
				AttachmentCards[i].UserControlClicked += UserControl_Click;
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		AttachmentUploadControl obj = (AttachmentUploadControl)sender;
		string path = obj.path;
		int ActionId = obj.ActionId;
		_ = obj.FileName;
		try
		{
			if (obj.ActiveControl != null && obj.ActiveControl.Text == "Delete")
			{
				if (!IsVpsAttachmentsServiceOn)
				{
					if (File.Exists(Path.Combine(saveDirectory, Path.GetFileName(path))))
					{
						obj.PictureBox.Image = null;
						foreach (DataRow item in (from row in dtAttachments.AsEnumerable()
							where row.Field<string>("Path") == obj.FileName
							select row).ToList())
						{
							item.Delete();
						}
						TempAttachments TA = new TempAttachments();
						TA.Id = 0;
						TA.Path = obj.path;
						RemovedAttachmentListInUpdateCase.Add(TA);
						lst.RemoveAll((TempAttachments x) => x.Path == obj.FileName);
						PicturesFlowLayout.Controls.Remove(obj);
						return;
					}
					lst.RemoveAll((TempAttachments x) => x.Path == obj.path);
					PicturesFlowLayout.Controls.Remove(obj);
					{
						foreach (DataRow item2 in (from row in dtAttachments.AsEnumerable()
							where row.Field<string>("Path") == obj.path
							select row).ToList())
						{
							item2.Delete();
						}
						return;
					}
				}
				bool FileExist = false;
				if (ActionId == 2)
				{
					ServerCredentials Sc = CommonServices.GetServerCredentials();
					string uri = "ftp://" + Sc.ServerIp + "/" + path;
					try
					{
						FtpWebRequest obj2 = (FtpWebRequest)WebRequest.Create(uri);
						obj2.Method = "MDTM";
						obj2.UsePassive = true;
						obj2.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using ((FtpWebResponse)obj2.GetResponse())
						{
							FileExist = true;
						}
					}
					catch (WebException)
					{
						FileExist = false;
					}
				}
				if (FileExist)
				{
					obj.PictureBox.Image = null;
					foreach (DataRow item3 in (from row in dtAttachments.AsEnumerable()
						where row.Field<string>("Path") == obj.FileName
						select row).ToList())
					{
						item3.Delete();
					}
					TempAttachments TA2 = new TempAttachments();
					TA2.Id = 0;
					TA2.Path = obj.path;
					RemovedAttachmentListInUpdateCase.Add(TA2);
					lst.RemoveAll((TempAttachments x) => x.Path == obj.FileName);
					PicturesFlowLayout.Controls.Remove(obj);
					return;
				}
				lst.RemoveAll((TempAttachments x) => x.Path == obj.path);
				PicturesFlowLayout.Controls.Remove(obj);
				{
					foreach (DataRow item4 in (from row in dtAttachments.AsEnumerable()
						where row.Field<string>("Path") == obj.path
						select row).ToList())
					{
						item4.Delete();
					}
					return;
				}
			}
			if (!IsVpsAttachmentsServiceOn)
			{
				if (File.Exists(path))
				{
					Process.Start(path);
				}
				return;
			}
			if (ActionId == 2)
			{
				using (MemoryStream memoryStream = new MemoryStream(obj.ByteArray))
				{
					memoryStream.Position = 0L;
					string fileExtension = Path.GetExtension(obj.FileName).ToLower();
					string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + fileExtension);
					try
					{
						using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
						{
							memoryStream.CopyTo(fileStream);
						}
						Process.Start(new ProcessStartInfo(tempFilePath)
						{
							UseShellExecute = true,
							Verb = "open"
						});
						Task.Delay(10000).ContinueWith(delegate
						{
							try
							{
								if (File.Exists(tempFilePath))
								{
									File.Delete(tempFilePath);
								}
							}
							catch (IOException)
							{
							}
						});
						return;
					}
					catch (Exception ex)
					{
						MessageBox.Show("An error occurred while trying to open the file: " + ex.Message);
						return;
					}
				}
			}
			if (File.Exists(path))
			{
				Process.Start(path);
			}
		}
		catch (Exception ex3)
		{
			MessageBox.Show(ex3.Message);
		}
	}

	private void Choose_Click(object sender, EventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.jfif)|*.jpg;*.jpeg;*.png;*.gif;*.jfif|Excel Files (*.xlsx;*.xlsm;*.xls;*.xlsb;*.xltx)|*.xlsx;*.xlsm;*.xls;*.xlsb;*.xltx|Word Documents (*.docx;*.doc)|*.docx;*.doc|PDF Files (*.pdf)|*.pdf|Text Files (*.txt)|*.txt|Compressed Files (*.zip;*.rar)|*.zip;*.rar|All Files (*.*)|*.*";
			openFileDialog.Multiselect = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				progressBar1.Value = 0;
				lblPercent.Visible = true;
				string[] fileNames = openFileDialog.FileNames;
				foreach (string file in fileNames)
				{
					if (lst.Where((TempAttachments row) => row.Path.Contains(file)).ToList().Count == 0)
					{
						TempAttachments TA = new TempAttachments();
						TA.Id = 0;
						TA.Path = file;
						TA.ActionId = 1;
						lst.Add(TA);
					}
					else
					{
						MessageBox.Show(Path.GetFileName(file) + " File Already Present");
					}
				}
			}
			if (lst.Count > 0)
			{
				AddToGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			_ = DriverPicPath != string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Attachment_FormClosing(object sender, FormClosingEventArgs e)
	{
		Hide();
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Hide();
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
			File.Delete(saveDirectory + UniqFileName);
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
		try
		{
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Hide();
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				Choose_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	/* public void Update(Observable from, CameraEvent e)
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
	} */

	private void LiveViewButton_Click(object sender, EventArgs e)
	{
		try
		{
			string UniqPathAnalysis = "";
			if (_controller == null)
			{
				throw new Exception("Camera not connected,Checked the camera and reload the page");
			}
			_remoteCapture = new RemoteCapture(ref _controller, ref _actionSource);
			_remoteCapture.ShowDialog(this);
			_remoteCapture.Dispose();
			if (saveDirectory != string.Empty)
			{
				UniqFileName = UniqPathAnalysis;
				string DrvPicPath = saveDirectory + UniqFileName;
				if (File.Exists(DrvPicPath))
				{
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

	private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		try
		{
			for (int i = 1; i <= 100; i++)
			{
				Thread.Sleep(50);
				backgroundWorker.WorkerReportsProgress = true;
				backgroundWorker.ReportProgress(i);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		progressBar1.Value = e.ProgressPercentage;
		lblPercent.Text = e.ProgressPercentage + "%";
		if (lblPercent.Text == "100%")
		{
			lblPercent.Visible = false;
			progressBar1.Visible = false;
			Choose.Enabled = true;
			PicturesFlowLayout.Enabled = true;
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
		this.lblPercent = new System.Windows.Forms.Label();
		this.progressBar1 = new System.Windows.Forms.ProgressBar();
		this.btnClose = new System.Windows.Forms.Button();
		this.Choose = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.PicturesFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.btnResetDriv = new System.Windows.Forms.Button();
		this.LiveViewButton = new System.Windows.Forms.Button();
		this.LiveViewPicBox = new System.Windows.Forms.PictureBox();
		this.label2 = new System.Windows.Forms.Label();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.LiveViewPicBox).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.lblPercent);
		this.panel1.Controls.Add(this.progressBar1);
		this.panel1.Controls.Add(this.btnClose);
		this.panel1.Controls.Add(this.Choose);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(824, 646);
		this.panel1.TabIndex = 0;
		this.lblPercent.AutoSize = true;
		this.lblPercent.Location = new System.Drawing.Point(135, 11);
		this.lblPercent.Name = "lblPercent";
		this.lblPercent.Size = new System.Drawing.Size(24, 13);
		this.lblPercent.TabIndex = 16;
		this.lblPercent.Text = "0 %";
		this.lblPercent.UseWaitCursor = true;
		this.progressBar1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		this.progressBar1.ForeColor = System.Drawing.Color.Green;
		this.progressBar1.Location = new System.Drawing.Point(172, 4);
		this.progressBar1.Name = "progressBar1";
		this.progressBar1.Size = new System.Drawing.Size(501, 28);
		this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
		this.progressBar1.TabIndex = 15;
		this.progressBar1.UseWaitCursor = true;
		this.btnClose.Location = new System.Drawing.Point(766, 6);
		this.btnClose.Margin = new System.Windows.Forms.Padding(2);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(56, 27);
		this.btnClose.TabIndex = 4;
		this.btnClose.Text = "Close";
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		this.Choose.Location = new System.Drawing.Point(11, 6);
		this.Choose.Margin = new System.Windows.Forms.Padding(2);
		this.Choose.Name = "Choose";
		this.Choose.Size = new System.Drawing.Size(56, 26);
		this.Choose.TabIndex = 1;
		this.Choose.Text = "Browse";
		this.Choose.UseVisualStyleBackColor = true;
		this.Choose.Click += new System.EventHandler(Choose_Click);
		this.panel2.Controls.Add(this.PicturesFlowLayout);
		this.panel2.Controls.Add(this.btnResetDriv);
		this.panel2.Controls.Add(this.LiveViewButton);
		this.panel2.Controls.Add(this.LiveViewPicBox);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel2.Location = new System.Drawing.Point(0, 36);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(824, 610);
		this.panel2.TabIndex = 0;
		this.PicturesFlowLayout.AutoScroll = true;
		this.PicturesFlowLayout.BackColor = System.Drawing.Color.White;
		this.PicturesFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PicturesFlowLayout.Location = new System.Drawing.Point(0, 0);
		this.PicturesFlowLayout.Name = "PicturesFlowLayout";
		this.PicturesFlowLayout.Size = new System.Drawing.Size(824, 610);
		this.PicturesFlowLayout.TabIndex = 52;
		this.btnResetDriv.Location = new System.Drawing.Point(407, 571);
		this.btnResetDriv.Name = "btnResetDriv";
		this.btnResetDriv.Size = new System.Drawing.Size(62, 23);
		this.btnResetDriv.TabIndex = 51;
		this.btnResetDriv.Text = "Reset";
		this.btnResetDriv.UseVisualStyleBackColor = true;
		this.btnResetDriv.Visible = false;
		this.btnResetDriv.Click += new System.EventHandler(btnResetDriv_Click);
		this.LiveViewButton.Location = new System.Drawing.Point(343, 571);
		this.LiveViewButton.Name = "LiveViewButton";
		this.LiveViewButton.Size = new System.Drawing.Size(62, 23);
		this.LiveViewButton.TabIndex = 49;
		this.LiveViewButton.Text = "Camera";
		this.LiveViewButton.UseVisualStyleBackColor = true;
		this.LiveViewButton.Visible = false;
		this.LiveViewButton.Click += new System.EventHandler(LiveViewButton_Click);
		this.LiveViewPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.LiveViewPicBox.Location = new System.Drawing.Point(261, 410);
		this.LiveViewPicBox.Name = "LiveViewPicBox";
		this.LiveViewPicBox.Size = new System.Drawing.Size(290, 155);
		this.LiveViewPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.LiveViewPicBox.TabIndex = 48;
		this.LiveViewPicBox.TabStop = false;
		this.LiveViewPicBox.Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(373, 392);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(70, 15);
		this.label2.TabIndex = 47;
		this.label2.Text = "Attechment";
		this.label2.Visible = false;
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_DoWork);
		this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(824, 646);
		base.ControlBox = false;
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "Attachment";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Attachment";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Attachment_FormClosing);
		base.Load += new System.EventHandler(Attachment_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Attachment_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.LiveViewPicBox).EndInit();
		base.ResumeLayout(false);
	}
}

public class RemoteCapture : Form
{
	public RemoteCapture(ref dynamic controller, ref dynamic actionSource) { }
	public void controlFocusButton(bool b) { }
	public void updateFixedMovie(object obj) { }
	public void updateMirrorLockUpState(object obj) { }
	public void updateAngleInfoLabel(params object[] args) { }
	public void changeMouseCursor(bool onoff) { }
}

public class EDSDK
{
	public struct EdsCameraPos
	{
		public double position;
		public int rolling;
		public int pitching;
	}
	public static void EdsSendCommand(params object[] args) { }
	public static void EdsSendStatusCommand(params object[] args) { }
}

}

