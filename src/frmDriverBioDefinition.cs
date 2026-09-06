// using Architecture.Reports;
// using Architecture.WinApp.Configurations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Architecture.BLL.Main;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.ZKTFingerPrint;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using libzkfpcsharp;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Sale
{

public class frmDriverBioDefinition : Form
{
	private int RECID;

	private bool UpdateMode;

	private string FingerPrintPicPath;

	public int RefDocumentTypeId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtHistoryForLayout = new DataTable();

	private string ScreenName = "";

	private bool Confiquration;

	private string saveDirectory = "";

	public static Image Logo;

	private VideoCaptureDevice videosource;

	private FilterInfoCollection captureDevice;

	private string DriverImagefileSavePath = "";

	private string DriverImageofd = string.Empty;

	private bool FormInitialized;

	private IntPtr mDevHandle = IntPtr.Zero;

	private IntPtr mDBHandle = IntPtr.Zero;

	private IntPtr FormHandle = IntPtr.Zero;

	private bool bIsTimeToDie;

	private bool IsRegister;

	private bool bIdentify = true;

	private byte[] FPBuffer;

	private byte[] DriverImageBytes;

	private byte[] FingerPrintImage;

	private int RegisterCount;

	private const int REGISTER_FINGER_COUNT = 3;

	private byte[][] RegTmps = new byte[3][];

	private byte[] RegTmp = new byte[2048];

	private byte[] CapTmp = new byte[2048];

	private int cbCapTmp = 2048;

	private int cbRegTmp;

	private int iFid = 1;

	private int mfpWidth;

	private int mfpHeight;

	private int mfpDpi;

	private const int MESSAGE_CAPTURED_OK = 1030;

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton BtnPrint;

	private Label label12;

	private TextBox txtRemarks;

	private Label label11;

	private Label label10;

	private Label label9;

	private Label label7;

	private TextBox txtFatherName;

	private Label label6;

	private GroupBox grp;

	private Button btnPreview;

	private Button btnDriverImage;

	private PictureBox picBoxDriver;

	private Panel panel7;

	private Panel panel9;

	internal GridEX DataGridHistory;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewHistory;

	private PictureBox picFPImg;

	private Button btnFpCapture;

	private Button btnFpStart;

	private GroupBox groupBox1;

	private Button btnReset;

	private Button btnResetDriv;

	private MaskedTextBox txtCNIC;

	private ComboBox comboBox1;

	private ToolStripButton btnattachment;

	private ToolStripButton btnSMS;

	private Label label14;

	private Panel panel12;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label16;

	private Button btnBrowse;

	private Label label2;

	private GroupBox groupBox2;

	private RadioButton rdmodifydate;

	private RadioButton rdentrydate;

	private Button btnShowHistory;

	private Label label53;

	private Label label54;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private CheckBox ChkBox;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripButton btnRefresh;

	private MaskedTextBox txtAlternateCellNo;

	private MaskedTextBox txtDriverCellNo;

	private TextBox txtDriverName;

	private MaskedTextBox txtWhatsAppNo;

	private Label label1;

	private Label label3;

	private MaskedTextBox txtFatherCNIC;

	private Panel panel5;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNewHistoryInFormTab;

	private Panel panel6;

	private Label label4;

	private Panel panel8;

	private GroupBox groupBox3;

	private RadioButton RdModifyDateFormTab;

	private RadioButton RdEntryDateFromTab;

	private Button BtnShowInFormTab;

	private Label label13;

	private Label label15;

	private DateTimePicker ToDateHistoryInFormTab;

	private DateTimePicker FromDateHistoryInFormTab;

	internal GridEX GridHistory;

	private Panel panel10;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label17;

	[DllImport("user32.dll", EntryPoint = "SendMessageA")]
	public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

	public frmDriverBioDefinition(UserAccount user)
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmDriverBioDefinition_Load(object sender, EventArgs e)
	{
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1150, 650);
			ScreenName = Conversion.ToString(base.Tag);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			btnsave.Enabled = formright.DoHaveSaveRight;
			BtnPrint.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			if (RefDocumentTypeId == 0)
			{
				if (ScreenName == "frmDriverBioDefinitionForInWard")
				{
					RefDocumentTypeId = 51;
				}
				else if (ScreenName == "frmDriverBioDefinitionForOutWard")
				{
					RefDocumentTypeId = 91;
				}
			}
			else if (RefDocumentTypeId == 51)
			{
				ScreenName = "frmDriverBioDefinitionForInWard";
			}
			else if (RefDocumentTypeId == 91)
			{
				ScreenName = "frmDriverBioDefinitionForOutWard";
			}
			captureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
			foreach (FilterInfo item in (CollectionBase)(object)captureDevice)
			{
				FilterInfo device = item;
				comboBox1.Items.Add(device.Name);
			}
			if (comboBox1.Text == null || comboBox1.Text == string.Empty)
			{
				comboBox1.Text = string.Empty;
			}
			else
			{
				comboBox1.SelectedIndex = 0;
			}
			FormHandle = base.Handle;
			btnFpStart.Enabled = true;
			btnFpCapture.Enabled = false;
			txtDriverName.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
			txtDriverCellNo.Text = "0092-3-";
			txtWhatsAppNo.Text = "0092-3-";
			txtAlternateCellNo.Text = "0092-3-";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtCNIC.Text.Trim() == "-       -" || txtCNIC.Text.Length < 15 || !txtCNIC.MaskFull)
		{
			MessageBox.Show("CNIC Field Required");
			txtCNIC.Focus();
			return false;
		}
		if (txtDriverCellNo.Text.Trim() == string.Empty || !txtDriverCellNo.MaskFull)
		{
			MessageBox.Show("Cell No. Field Required");
			txtDriverCellNo.Focus();
			return false;
		}
		if (txtDriverName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Driver Name  Field Required");
			txtDriverName.Focus();
			return false;
		}
		if (txtFatherName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Father Name  Field Required");
			txtFatherName.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			driverBiodata driverInfo = new driverBiodata();
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				driverInfo.driverBiodataId = RECID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			driverInfo.OrganizationId = UserAccount.OrganizationId;
			driverInfo.CompanyId = UserAccount.CompanyId;
			driverInfo.BranchId = UserAccount.BranchesId;
			driverInfo.DocumentTypeId = 146;
			driverInfo.EntryUserId = UserAccount.ID;
			driverInfo.ModifyUserId = UserAccount.ID;
			driverInfo.EntryDate = DateTime.Now;
			driverInfo.ModifyDate = DateTime.Now;
			driverInfo.ApprovedDate = DateTime.Now;
			driverInfo.driverName = txtDriverName.Text;
			driverInfo.cnicNo = txtCNIC.Text;
			driverInfo.cellNo = txtDriverCellNo.Text;
			driverInfo.alternateCellNo = txtAlternateCellNo.Text;
			driverInfo.whatsappNo = txtWhatsAppNo.Text;
			driverInfo.fatherName = txtFatherName.Text;
			driverInfo.fatherCnicNo = txtFatherCNIC.Text;
			driverInfo.driverPicture = Path.GetFileName(Conversion.ToString((object)DriverImagefileSavePath));
			driverInfo.driverFingerPrint = Path.GetFileName(FingerPrintPicPath);
			driverInfo.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			driverInfo.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<driverBiodata>(driverInfo, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = driverBiodata.Save(driverInfo);
			if (RECID > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + driverInfo.driverBiodataId);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RECID, driverInfo.DocumentTypeId, ScreenName);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + driverInfo.driverBiodataId);
			}
			if (!string.IsNullOrEmpty(DriverImageofd))
			{
				File.Copy(DriverImageofd, DriverImagefileSavePath, overwrite: true);
			}
			else if (picBoxDriver.Image != null && !File.Exists(DriverImagefileSavePath))
			{
				picBoxDriver.Image.Save(DriverImagefileSavePath, ImageFormat.Jpeg);
			}
			DeletePreviousDriverPicPhysically();
			DriverImageofd = string.Empty;
			DriverImagefileSavePath = string.Empty;
			picBoxDriver.Image = null;
			picBoxDriver.Invalidate();
			FormReset();
			if (ChkBox.Checked)
			{
				GenerateReport(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeletePreviousDriverPicPhysically()
	{
		try
		{
			string picNameInReadById = Conversion.ToString(picBoxDriver.Tag);
			if (string.IsNullOrEmpty(picNameInReadById) || Path.GetFileName(DriverImagefileSavePath) == picNameInReadById)
			{
				return;
			}
			string path = Path.Combine(clsGlobalVariables.AttachmentFolderPath, picNameInReadById);
			if (File.Exists(path))
			{
				using (new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
				{
				}
				File.Delete(path);
			}
		}
		catch (UnauthorizedAccessException)
		{
			MessageBox.Show("Access denied. The file may be in use or read-only.");
		}
		catch (IOException ex2)
		{
			MessageBox.Show("File is currently in use and cannot be deleted.\nDetails: " + ex2.Message);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			RECID = Id;
			driverBiodata obj = driverBiodata.ReadById(RECID);
			if (obj == null)
			{
				return;
			}
			UpdateMode = true;
			tabControl1.SelectedIndex = 0;
			txtDriverName.Text = obj.driverName;
			txtCNIC.Text = obj.cnicNo;
			txtDriverCellNo.Text = obj.cellNo;
			txtAlternateCellNo.Text = obj.alternateCellNo;
			txtWhatsAppNo.Text = obj.whatsappNo;
			txtFatherName.Text = obj.fatherName;
			txtFatherCNIC.Text = obj.fatherCnicNo;
			_ = obj.driverFingerPrint != "";
			string driverPic = Path.GetFileName(Conversion.ToString((object)obj.driverPicture));
			if (driverPic != "")
			{
				DriverImagefileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, driverPic);
				if (File.Exists(DriverImagefileSavePath))
				{
					picBoxDriver.Image = Image.FromFile(DriverImagefileSavePath);
					picBoxDriver.Tag = driverPic;
				}
				else
				{
					DriverImagefileSavePath = "";
					picBoxDriver.Image = null;
					picBoxDriver.Invalidate();
				}
			}
			else
			{
				DriverImagefileSavePath = "";
				picBoxDriver.Image = null;
				picBoxDriver.Invalidate();
			}
			FormHelper.LoadAttachmentsForObject(AT, RECID, ScreenName);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RECID = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RECID == 0)
			{
				throw new Exception("Record Not Update because RecId Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void FormReset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			RECID = 0;
			txtDriverName.Text = string.Empty;
			txtCNIC.Text = string.Empty;
			txtDriverCellNo.Text = string.Empty;
			txtWhatsAppNo.Text = string.Empty;
			txtAlternateCellNo.Text = string.Empty;
			txtDriverCellNo.Text = "0092-3-";
			txtWhatsAppNo.Text = "0092-3-";
			txtAlternateCellNo.Text = "0092-3-";
			txtFatherName.Text = string.Empty;
			txtFatherCNIC.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtDriverName.Focus();
			picBoxDriver.Image?.Dispose();
			picBoxDriver.Tag = "";
			btnResetDriv_Click(null, null);
			btnReset_Click(null, null);
			clsGlobalVariables.globalAllDriverBioInfo = driverBiodata.ReadAll(UserAccount.OrganizationId, UserAccount.CompanyId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDriverBioDefinition_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			btnReset_Click(null, null);
			btnResetDriv_Click(null, null);
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
	}

	public void definition()
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList();
			if (list.Count > 0)
			{
				string value = list[0].ConfigKey.ToString();
				if (value != null)
				{
					saveDirectory = value;
					Confiquration = true;
				}
			}
			else
			{
				MessageBox.Show("Please Map the Path in Configration");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void videosource_NewFrame(object sender, NewFrameEventArgs eventArgs)
	{
		picBoxDriver.Image = (Bitmap)eventArgs.Frame.Clone();
	}

	private void btnFpStart_Click(object sender, EventArgs e)
	{
		try
		{
			MessageBox.Show("Not Configured...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnFpCapture_Click(object sender, EventArgs e)
	{
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
	}

	public void Intilize()
	{
		try
		{
			List<string> cmbIdx = new List<string>();
			int ret = zkfperrdef.ZKFP_ERR_OK;
			if ((ret = zkfp2.Init()) == zkfperrdef.ZKFP_ERR_OK)
			{
				int nCount = zkfp2.GetDeviceCount();
				if (nCount > 0)
				{
					for (int i = 0; i < nCount; i++)
					{
						cmbIdx.Add(i.ToString());
					}
					return;
				}
				zkfp2.Terminate();
				throw new Exception("No device connected!");
			}
			throw new Exception("Initialize fail, ret=" + ret + " !");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Connect(int Idx)
	{
		try
		{
			_ = zkfp.ZKFP_ERR_OK;
			if (IntPtr.Zero == (mDevHandle = zkfp2.OpenDevice(Idx)))
			{
				throw new Exception("OpenDevice fail");
			}
			if (IntPtr.Zero == (mDBHandle = zkfp2.DBInit()))
			{
				zkfp2.CloseDevice(mDevHandle);
				mDevHandle = IntPtr.Zero;
				throw new Exception("Init DB fail");
			}
			RegisterCount = 0;
			cbRegTmp = 0;
			iFid = 1;
			for (int i = 0; i < 3; i++)
			{
				RegTmps[i] = new byte[2048];
			}
			byte[] paramValue = new byte[4];
			int size = 4;
			zkfp2.GetParameters(mDevHandle, 1, paramValue, ref size);
			zkfp2.ByteArray2Int(paramValue, ref mfpWidth);
			size = 4;
			zkfp2.GetParameters(mDevHandle, 2, paramValue, ref size);
			zkfp2.ByteArray2Int(paramValue, ref mfpHeight);
			FPBuffer = new byte[mfpWidth * mfpHeight];
			size = 4;
			zkfp2.GetParameters(mDevHandle, 3, paramValue, ref size);
			zkfp2.ByteArray2Int(paramValue, ref mfpDpi);
			Thread thread = new Thread(DoCapture);
			thread.IsBackground = true;
			thread.Start();
			bIsTimeToDie = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void DoCapture()
	{
		try
		{
			while (!bIsTimeToDie)
			{
				cbCapTmp = 2048;
				if (zkfp2.AcquireFingerprint(mDevHandle, FPBuffer, CapTmp, ref cbCapTmp) == zkfp.ZKFP_ERR_OK)
				{
					SendMessage(FormHandle, 1030, IntPtr.Zero, IntPtr.Zero);
				}
				Thread.Sleep(200);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	protected override void DefWndProc(ref Message m)
	{
		try
		{
			if (m.Msg == 1030)
			{
				MemoryStream ms = new MemoryStream();
				BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
				Bitmap bmp = new Bitmap(ms);
				picFPImg.Image = bmp;
				zkfp2.BlobToBase64(CapTmp, cbCapTmp);
				if (IsRegister)
				{
					int ret = zkfp.ZKFP_ERR_OK;
					int fid = 0;
					int score = 0;
					ret = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
					if (zkfp.ZKFP_ERR_OK == ret)
					{
						IsRegister = true;
						return;
					}
					if (RegisterCount > 0 && zkfp2.DBMatch(mDBHandle, CapTmp, RegTmps[RegisterCount - 1]) <= 0)
					{
						MessageBox.Show("Please press the same finger 3 times for the enrollment.\n");
					}
					Array.Copy(CapTmp, RegTmps[RegisterCount], cbCapTmp);
					zkfp2.Base64ToBlob(zkfp2.BlobToBase64(CapTmp, cbCapTmp));
					RegisterCount++;
					if (RegisterCount >= 3)
					{
						RegisterCount = 0;
						if (zkfp.ZKFP_ERR_OK == (ret = zkfp2.DBMerge(mDBHandle, RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref cbRegTmp)))
						{
							iFid++;
							btnFpCapture.Enabled = true;
						}
						else
						{
							MessageBox.Show("enroll fail, error code=" + ret + "\n");
						}
						IsRegister = false;
					}
					return;
				}
				if (cbRegTmp <= 0)
				{
					MessageBox.Show("Please register your finger first!\n");
				}
				if (bIdentify)
				{
					int ret2 = zkfp.ZKFP_ERR_OK;
					int fid2 = 0;
					int score2 = 0;
					ret2 = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid2, ref score2);
					if (zkfp.ZKFP_ERR_OK != ret2)
					{
						MessageBox.Show("Identify fail, ret= " + ret2 + "\n");
					}
				}
				else
				{
					int ret3 = zkfp2.DBMatch(mDBHandle, CapTmp, RegTmp);
					if (0 >= ret3)
					{
						MessageBox.Show("Match finger fail, ret= " + ret3 + "\n");
					}
				}
			}
			else
			{
				base.DefWndProc(ref m);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Disconnect()
	{
		try
		{
			zkfp2.Terminate();
			cbRegTmp = 0;
			bIsTimeToDie = true;
			RegisterCount = 0;
			Thread.Sleep(1000);
			zkfp2.CloseDevice(mDevHandle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CaptureBmp(string FileName)
	{
		try
		{
			if (!(FileName != "") || FileName == null || picFPImg.Image == null)
			{
				return;
			}
			Bitmap bmp = new Bitmap(picFPImg.Image.Width, picFPImg.Image.Height, PixelFormat.Format24bppRgb);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.DrawImage(picFPImg.Image, 0, 0, bmp.Width, bmp.Height);
			}
			Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
			BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
			IntPtr scan = bmpData.Scan0;
			int bytes = bmpData.Stride * bmpData.Height;
			byte[] rgbValues = new byte[bytes];
			Marshal.Copy(scan, rgbValues, 0, bytes);
			Rectangle rect2 = new Rectangle(0, 0, bmp.Width, bmp.Height);
			Bitmap bit = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format8bppIndexed);
			BitmapData bmpData2 = bit.LockBits(rect2, ImageLockMode.ReadWrite, bit.PixelFormat);
			IntPtr ptr2 = bmpData2.Scan0;
			int bytes2 = bmpData2.Stride * bmpData2.Height;
			byte[] rgbValues2 = new byte[bytes2];
			Marshal.Copy(ptr2, rgbValues2, 0, bytes2);
			double colorTemp = 0.0;
			for (int j = 0; j < bmpData.Height; j++)
			{
				for (int k = 0; k < bmpData.Width * 3; k += 3)
				{
					colorTemp = (double)(int)rgbValues[j * bmpData.Stride + k + 2] * 0.299 + (double)(int)rgbValues[j * bmpData.Stride + k + 1] * 0.578 + (double)(int)rgbValues[j * bmpData.Stride + k] * 0.114;
					rgbValues2[j * bmpData2.Stride + k / 3] = (byte)colorTemp;
				}
			}
			Marshal.Copy(rgbValues2, 0, ptr2, bytes2);
			bmp.UnlockBits(bmpData);
			ColorPalette tempPalette;
			using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
			{
				tempPalette = tempBmp.Palette;
			}
			for (int i = 0; i < 256; i++)
			{
				tempPalette.Entries[i] = Color.FromArgb(i, i, i);
			}
			bit.Palette = tempPalette;
			bit.UnlockBits(bmpData2);
			bit.Save(FileName, picFPImg.Image.RawFormat);
			bit.Dispose();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Enroll()
	{
		try
		{
			if (!IsRegister)
			{
				IsRegister = true;
				RegisterCount = 0;
				cbRegTmp = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReport(RECID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int PrintId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt = driverBiodata.WorkOrder_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting rpt = new Reporting();
			if (UserAccount.CompAddress != null)
			{
				rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			}
			if (UserAccount.CompName != null)
			{
				rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			}
			rpt.ShowReportWithDataTable(dt, "RptGatepassOutwardDriverInfoSlipA.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSMS_Click(object sender, EventArgs e)
	{
		try
		{
			CreateSMSTemplate createSMSTemplate = new CreateSMSTemplate(UserAccount);
			createSMSTemplate.ScreenName = ScreenName;
			createSMSTemplate.Show();
			createSMSTemplate.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 0)
			{
				FromDateHistory.Focus();
			}
			else
			{
				txtCNIC.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			DataGridHistory.DataSource = null;
			rdentrydate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistoryGrid.Rows.Clear();
			ReportsParameters driverInfo = new ReportsParameters();
			driverInfo.OrganizationId = UserAccount.OrganizationId;
			driverInfo.CompanyId = UserAccount.CompanyId;
			driverInfo.BranchesId = UserAccount.BranchesId;
			driverInfo.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			driverInfo.DocumentTypeId = 146;
			driverInfo.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!driverInfo.CanViewAllRecord)
			{
				driverInfo.EntryUser = UserAccount.ID;
			}
			if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					driverInfo.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					driverInfo.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					driverInfo.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					driverInfo.ModifyDateTo = ToDateHistory.Value;
				}
			}
			DataTable dtGetAll = driverBiodata.FormHistory(driverInfo);
			if (dtGetAll.Rows.Count > 0)
			{
				new DataTable();
				if (dtHistoryGrid.Columns.Count == 0)
				{
					dtHistoryGrid.Columns.Add("Id", typeof(int));
					dtHistoryGrid.Columns.Add("DocumentTypeId", typeof(int));
					dtHistoryGrid.Columns.Add("DriverName", typeof(string));
					dtHistoryGrid.Columns.Add("DriverCNIC", typeof(string));
					dtHistoryGrid.Columns.Add("DriverCellNo", typeof(string));
					dtHistoryGrid.Columns.Add("WhatsAppNo", typeof(string));
					dtHistoryGrid.Columns.Add("AlternateCellNo", typeof(string));
					dtHistoryGrid.Columns.Add("FatherName", typeof(string));
					dtHistoryGrid.Columns.Add("FatherCNIC", typeof(string));
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("EntryUser", typeof(string));
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ModifyUser", typeof(string));
					dtHistoryGrid.Columns.Add("NoOfAttachments", typeof(int));
				}
				foreach (DataRow row in dtGetAll.Rows)
				{
					dtHistoryGrid.Rows.Add(row["driverBiodataId"], row["DocumentTypeId"], row["driverName"], row["cnicNo"], row["cellNo"], row["whatsappNo"], row["alternateCellNo"], row["fatherName"], row["fatherCnicNo"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtHistoryGrid;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
			}
			else
			{
				DataGridHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridWrappingAndColumnSettings(DataGridHistory, 2, 3);
			DataGridHistory.RootTable.Columns["DriverName"].Width = Constants.InventoryConstants.DriverName;
			DataGridHistory.RootTable.Columns["DriverCNIC"].Width = Constants.InventoryConstants.DriverCnicNoConstant + 25;
			DataGridHistory.RootTable.Columns["DriverCellNo"].Width = Constants.InventoryConstants.CellNo + 20;
			DataGridHistory.RootTable.Columns["WhatsAppNo"].Width = Constants.InventoryConstants.CellNo + 20;
			DataGridHistory.RootTable.Columns["AlternateCellNo"].Width = Constants.InventoryConstants.CellNo + 20;
			DataGridHistory.RootTable.Columns["FatherName"].Width = Constants.InventoryConstants.DriverName;
			DataGridHistory.RootTable.Columns["FatherCNIC"].Width = Constants.InventoryConstants.DriverCnicNoConstant + 25;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 35;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 40;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			((Control)(object)DataGridHistory).Refresh();
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(id, ScreenName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				RECID = id;
				ReadById(RECID);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					RECID = id;
					ReadById(RECID);
				}
				if (e.Column.Key == "Print")
				{
					GenerateReport(id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, GridHistory, dtHistoryForLayout);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, DataGridHistory, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistoryInFormTab_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistoryInFormTab.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistoryInFormTab.Value = DateTime.Now;
			GridHistory.DataSource = null;
			RdEntryDateFromTab.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnShowInFormTab_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFillInFormTab();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFillInFormTab()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters driverInfo = new ReportsParameters();
			driverInfo.OrganizationId = UserAccount.OrganizationId;
			driverInfo.CompanyId = UserAccount.CompanyId;
			driverInfo.BranchesId = UserAccount.BranchesId;
			driverInfo.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			driverInfo.DocumentTypeId = 146;
			driverInfo.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!driverInfo.CanViewAllRecord)
			{
				driverInfo.EntryUser = UserAccount.ID;
			}
			if (RdEntryDateFromTab.Checked)
			{
				if (FromDateHistoryInFormTab.Checked)
				{
					driverInfo.EntryDateFrom = FromDateHistoryInFormTab.Value;
				}
				if (ToDateHistoryInFormTab.Checked)
				{
					driverInfo.EntryDateTo = ToDateHistoryInFormTab.Value;
				}
			}
			else if (RdModifyDateFormTab.Checked)
			{
				if (FromDateHistoryInFormTab.Checked)
				{
					driverInfo.ModifyDateFrom = FromDateHistoryInFormTab.Value;
				}
				if (ToDateHistoryInFormTab.Checked)
				{
					driverInfo.ModifyDateTo = ToDateHistoryInFormTab.Value;
				}
			}
			DataTable dtGetAll = driverBiodata.FormHistory(driverInfo);
			if (dtGetAll.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DriverName", typeof(string));
				dtcol.Columns.Add("DriverCNIC", typeof(string));
				dtcol.Columns.Add("DriverCellNo", typeof(string));
				dtcol.Columns.Add("WhatsAppNo", typeof(string));
				dtcol.Columns.Add("AlternateCellNo", typeof(string));
				dtcol.Columns.Add("FatherName", typeof(string));
				dtcol.Columns.Add("FatherCNIC", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtHistoryForLayout = dtcol.Clone();
				foreach (DataRow row in dtGetAll.Rows)
				{
					dtcol.Rows.Add(row["driverBiodataId"], row["DocumentTypeId"], row["driverName"], row["cnicNo"], row["cellNo"], row["whatsappNo"], row["alternateCellNo"], row["fatherName"], row["fatherCnicNo"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["NoOfAttachments"]);
				}
				dtHistoryForLayout = dtcol.Copy();
				GridHistory.DataSource = dtcol;
				GridHistory.RetrieveStructure();
				GridHistorySettingFormTab();
			}
			else
			{
				GridHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistorySettingFormTab()
	{
		try
		{
			GridHistory.RootTable.Columns["Id"].Visible = false;
			GridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			GridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridWrappingAndColumnSettings(GridHistory, 2, 3);
			GridHistory.RootTable.Columns["DriverName"].Width = Constants.InventoryConstants.DriverName;
			GridHistory.RootTable.Columns["DriverCNIC"].Width = Constants.InventoryConstants.DriverCnicNoConstant + 25;
			GridHistory.RootTable.Columns["DriverCellNo"].Width = Constants.InventoryConstants.CellNo + 20;
			GridHistory.RootTable.Columns["WhatsAppNo"].Width = Constants.InventoryConstants.CellNo + 20;
			GridHistory.RootTable.Columns["AlternateCellNo"].Width = Constants.InventoryConstants.CellNo + 20;
			GridHistory.RootTable.Columns["FatherName"].Width = Constants.InventoryConstants.DriverName;
			GridHistory.RootTable.Columns["FatherCNIC"].Width = Constants.InventoryConstants.DriverCnicNoConstant + 25;
			GridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			GridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			GridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			GridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GridHistory.RootTable.Columns.Add("Edit");
			GridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			GridHistory.RootTable.Columns["Edit"].Width = 35;
			GridHistory.RootTable.Columns["Edit"].Position = 0;
			GridHistory.RootTable.Columns.Add("Print");
			GridHistory.RootTable.Columns["Print"].Caption = "Print";
			GridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			GridHistory.RootTable.Columns["Print"].Width = 40;
			GridHistory.RootTable.Columns["Print"].Position = 1;
			GridHistory.FrozenColumns = 2;
			((Control)(object)GridHistory).Refresh();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(id, ScreenName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					RECID = id;
					ReadById(RECID);
				}
				if (e.Column.Key == "Print")
				{
					GenerateReport(id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				RECID = id;
				ReadById(RECID);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDriverCellNo_KeyPress(object sender, KeyPressEventArgs e)
	{
	}

	private void txtAlternateCellNo_KeyPress(object sender, KeyPressEventArgs e)
	{
	}

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnBrowse_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (Confiquration)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg;  *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					DriverImageofd = openFileDialog.FileName;
					string UniqFileName = $"{Guid.NewGuid()}.Bmp";
					DriverImagefileSavePath = Path.Combine(saveDirectory, UniqFileName);
					Bitmap image = new Bitmap(openFileDialog.FileName);
					picBoxDriver.Image = image;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDriverImage_Click(object sender, EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			if (comboBox1.Text != string.Empty)
			{
				videosource = new VideoCaptureDevice(captureDevice[comboBox1.SelectedIndex].MonikerString);
				videosource.NewFrame += new NewFrameEventHandler(videosource_NewFrame);
				videosource.Start();
				return;
			}
			comboBox1.Focus();
			throw new Exception("Please Select Web Cam ");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPreview_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (!Confiquration)
			{
				return;
			}
			if (picBoxDriver.Image != null)
			{
				if (videosource != null)
				{
					videosource.Stop();
				}
				string UniqFileName = $"{Guid.NewGuid()}.Bmp";
				DriverImagefileSavePath = Path.Combine(saveDirectory, UniqFileName);
				Image image = picBoxDriver.Image;
				picBoxDriver.Image = image;
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
			if (videosource != null)
			{
				videosource.Stop();
			}
			picBoxDriver.Image = null;
			picBoxDriver.Invalidate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDriverBioDefinition_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtCNIC.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDriverName.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtCNIC.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
				{
					Print_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formright.DoHavePrintRights)
				{
					Print_Click(null, null);
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					BtnNewHistory_Click(null, null);
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 82;
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDateHistory.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save in Form Tab and For Show History in History Tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Gp No");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Pending GP Grid when focus in form tab and for focus on history grid when in history tab");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on CNIC in Pending GP Grid when in Form tab and For focus on FromDate in history tab");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int CuurID = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				FormReset();
				ReadById(CuurID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					FormReset();
					ReadById(CuurID);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					GenerateReport(CuurID);
				}
				else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CuurID, ScreenName);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_2514: Unknown result type (might be due to invalid IL or missing references)
		//IL_251e: Expected O, but got Unknown
		//IL_252b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2535: Expected O, but got Unknown
		//IL_3850: Unknown result type (might be due to invalid IL or missing references)
		//IL_385a: Expected O, but got Unknown
		//IL_3867: Unknown result type (might be due to invalid IL or missing references)
		//IL_3871: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Sale.frmDriverBioDefinition));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtWhatsAppNo = new System.Windows.Forms.MaskedTextBox();
		this.txtAlternateCellNo = new System.Windows.Forms.MaskedTextBox();
		this.txtDriverCellNo = new System.Windows.Forms.MaskedTextBox();
		this.txtFatherCNIC = new System.Windows.Forms.MaskedTextBox();
		this.txtCNIC = new System.Windows.Forms.MaskedTextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtDriverName = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtFatherName = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.label14 = new System.Windows.Forms.Label();
		this.comboBox1 = new System.Windows.Forms.ComboBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnSMS = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.grp = new System.Windows.Forms.GroupBox();
		this.btnBrowse = new System.Windows.Forms.Button();
		this.picBoxDriver = new System.Windows.Forms.PictureBox();
		this.btnDriverImage = new System.Windows.Forms.Button();
		this.btnPreview = new System.Windows.Forms.Button();
		this.btnResetDriv = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.picFPImg = new System.Windows.Forms.PictureBox();
		this.btnFpStart = new System.Windows.Forms.Button();
		this.btnFpCapture = new System.Windows.Forms.Button();
		this.btnReset = new System.Windows.Forms.Button();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.label2 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistoryInFormTab = new System.Windows.Forms.ToolStripButton();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.RdModifyDateFormTab = new System.Windows.Forms.RadioButton();
		this.RdEntryDateFromTab = new System.Windows.Forms.RadioButton();
		this.BtnShowInFormTab = new System.Windows.Forms.Button();
		this.label13 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.ToDateHistoryInFormTab = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistoryInFormTab = new System.Windows.Forms.DateTimePicker();
		this.GridHistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.grp.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picBoxDriver).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picFPImg).BeginInit();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.panel9.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridHistory).BeginInit();
		this.panel10.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1157, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1157, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.tabPage1.Size = new System.Drawing.Size(1149, 633);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add(this.grp);
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel2.Location = new System.Drawing.Point(4, 3);
		this.panel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(341, 627);
		this.panel2.TabIndex = 0;
		this.panel4.Controls.Add(this.txtWhatsAppNo);
		this.panel4.Controls.Add(this.txtAlternateCellNo);
		this.panel4.Controls.Add(this.txtDriverCellNo);
		this.panel4.Controls.Add(this.txtFatherCNIC);
		this.panel4.Controls.Add(this.txtCNIC);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.txtDriverName);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.txtFatherName);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 53);
		this.panel4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(341, 204);
		this.panel4.TabIndex = 0;
		this.txtWhatsAppNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWhatsAppNo.Location = new System.Drawing.Point(94, 78);
		this.txtWhatsAppNo.Margin = new System.Windows.Forms.Padding(2);
		this.txtWhatsAppNo.Mask = "0092-300-0000000";
		this.txtWhatsAppNo.Name = "txtWhatsAppNo";
		this.txtWhatsAppNo.Size = new System.Drawing.Size(220, 23);
		this.txtWhatsAppNo.TabIndex = 3;
		this.txtAlternateCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAlternateCellNo.Location = new System.Drawing.Point(94, 104);
		this.txtAlternateCellNo.Margin = new System.Windows.Forms.Padding(2);
		this.txtAlternateCellNo.Mask = "0092-300-0000000";
		this.txtAlternateCellNo.Name = "txtAlternateCellNo";
		this.txtAlternateCellNo.Size = new System.Drawing.Size(220, 23);
		this.txtAlternateCellNo.TabIndex = 4;
		this.txtDriverCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverCellNo.Location = new System.Drawing.Point(94, 53);
		this.txtDriverCellNo.Margin = new System.Windows.Forms.Padding(2);
		this.txtDriverCellNo.Mask = "0092-300-0000000";
		this.txtDriverCellNo.Name = "txtDriverCellNo";
		this.txtDriverCellNo.Size = new System.Drawing.Size(220, 23);
		this.txtDriverCellNo.TabIndex = 2;
		this.txtFatherCNIC.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFatherCNIC.Location = new System.Drawing.Point(94, 154);
		this.txtFatherCNIC.Margin = new System.Windows.Forms.Padding(2);
		this.txtFatherCNIC.Mask = "00000-0000000-0";
		this.txtFatherCNIC.Name = "txtFatherCNIC";
		this.txtFatherCNIC.Size = new System.Drawing.Size(220, 23);
		this.txtFatherCNIC.TabIndex = 6;
		this.txtCNIC.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCNIC.Location = new System.Drawing.Point(94, 28);
		this.txtCNIC.Margin = new System.Windows.Forms.Padding(2);
		this.txtCNIC.Mask = "00000-0000000-0";
		this.txtCNIC.Name = "txtCNIC";
		this.txtCNIC.Size = new System.Drawing.Size(220, 23);
		this.txtCNIC.TabIndex = 1;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(7, 182);
		this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 75;
		this.label12.Text = "Remarks";
		this.label12.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 82);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(73, 15);
		this.label1.TabIndex = 73;
		this.label1.Text = "WhatsApp #";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(94, 178);
		this.txtRemarks.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(220, 23);
		this.txtRemarks.TabIndex = 7;
		this.txtRemarks.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(7, 108);
		this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(84, 15);
		this.label11.TabIndex = 73;
		this.label11.Text = "Alternate Cell#";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(7, 57);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(71, 15);
		this.label10.TabIndex = 71;
		this.label10.Text = "Driver Cell #";
		this.txtDriverName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverName.Location = new System.Drawing.Point(94, 3);
		this.txtDriverName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtDriverName.Name = "txtDriverName";
		this.txtDriverName.Size = new System.Drawing.Size(220, 23);
		this.txtDriverName.TabIndex = 0;
		this.txtDriverName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDriverCellNo_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(7, 32);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(41, 15);
		this.label9.TabIndex = 69;
		this.label9.Text = "CNIC#";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 158);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(70, 15);
		this.label3.TabIndex = 67;
		this.label3.Text = "Father CNIC";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(7, 133);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(75, 15);
		this.label7.TabIndex = 67;
		this.label7.Text = "Father Name";
		this.txtFatherName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFatherName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFatherName.Location = new System.Drawing.Point(94, 129);
		this.txtFatherName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtFatherName.Name = "txtFatherName";
		this.txtFatherName.Size = new System.Drawing.Size(220, 23);
		this.txtFatherName.TabIndex = 5;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(7, 7);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(74, 15);
		this.label6.TabIndex = 65;
		this.label6.Text = "Driver Name";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Controls.Add(this.label14);
		this.panel3.Controls.Add(this.comboBox1);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(341, 53);
		this.panel3.TabIndex = 0;
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(263, 33);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(78, 17);
		this.ChkBox.TabIndex = 85;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(2, 31);
		this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(90, 19);
		this.label14.TabIndex = 0;
		this.label14.Text = "Driver Bio";
		this.comboBox1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comboBox1.FormattingEnabled = true;
		this.comboBox1.Location = new System.Drawing.Point(94, 29);
		this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Size = new System.Drawing.Size(166, 24);
		this.comboBox1.TabIndex = 84;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.BtnPrint, this.btnattachment, this.btnSMS, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(341, 27);
		this.toolStrip1.TabIndex = 5;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnPrint.Text = "&146-Print";
		this.BtnPrint.Click += new System.EventHandler(Print_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnSMS.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSMS.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.btnSMS.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSMS.Name = "btnSMS";
		this.btnSMS.Size = new System.Drawing.Size(60, 24);
		this.btnSMS.Text = "&SMS";
		this.btnSMS.Visible = false;
		this.btnSMS.Click += new System.EventHandler(btnSMS_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.grp.Controls.Add(this.btnBrowse);
		this.grp.Controls.Add(this.picBoxDriver);
		this.grp.Controls.Add(this.btnDriverImage);
		this.grp.Controls.Add(this.btnPreview);
		this.grp.Controls.Add(this.btnResetDriv);
		this.grp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.grp.Location = new System.Drawing.Point(4, 258);
		this.grp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.grp.Name = "grp";
		this.grp.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.grp.Size = new System.Drawing.Size(310, 182);
		this.grp.TabIndex = 76;
		this.grp.TabStop = false;
		this.grp.Text = "Driver Picture";
		this.btnBrowse.BackColor = System.Drawing.Color.Teal;
		this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnBrowse.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnBrowse.ForeColor = System.Drawing.Color.White;
		this.btnBrowse.Location = new System.Drawing.Point(231, 155);
		this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnBrowse.Name = "btnBrowse";
		this.btnBrowse.Size = new System.Drawing.Size(75, 25);
		this.btnBrowse.TabIndex = 17;
		this.btnBrowse.Text = "Browse";
		this.btnBrowse.UseVisualStyleBackColor = false;
		this.btnBrowse.Click += new System.EventHandler(btnBrowse_Click);
		this.picBoxDriver.Dock = System.Windows.Forms.DockStyle.Top;
		this.picBoxDriver.Location = new System.Drawing.Point(4, 19);
		this.picBoxDriver.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.picBoxDriver.Name = "picBoxDriver";
		this.picBoxDriver.Size = new System.Drawing.Size(302, 134);
		this.picBoxDriver.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picBoxDriver.TabIndex = 0;
		this.picBoxDriver.TabStop = false;
		this.btnDriverImage.BackColor = System.Drawing.Color.Teal;
		this.btnDriverImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDriverImage.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnDriverImage.ForeColor = System.Drawing.Color.White;
		this.btnDriverImage.Location = new System.Drawing.Point(4, 155);
		this.btnDriverImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnDriverImage.Name = "btnDriverImage";
		this.btnDriverImage.Size = new System.Drawing.Size(75, 25);
		this.btnDriverImage.TabIndex = 14;
		this.btnDriverImage.Text = "Start";
		this.btnDriverImage.UseVisualStyleBackColor = false;
		this.btnDriverImage.Click += new System.EventHandler(btnDriverImage_Click);
		this.btnPreview.BackColor = System.Drawing.Color.Teal;
		this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPreview.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnPreview.ForeColor = System.Drawing.Color.White;
		this.btnPreview.Location = new System.Drawing.Point(80, 155);
		this.btnPreview.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnPreview.Name = "btnPreview";
		this.btnPreview.Size = new System.Drawing.Size(85, 25);
		this.btnPreview.TabIndex = 15;
		this.btnPreview.Text = "Capture";
		this.btnPreview.UseVisualStyleBackColor = false;
		this.btnPreview.Click += new System.EventHandler(btnPreview_Click);
		this.btnResetDriv.BackColor = System.Drawing.Color.Teal;
		this.btnResetDriv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnResetDriv.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnResetDriv.ForeColor = System.Drawing.Color.White;
		this.btnResetDriv.Location = new System.Drawing.Point(166, 155);
		this.btnResetDriv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnResetDriv.Name = "btnResetDriv";
		this.btnResetDriv.Size = new System.Drawing.Size(65, 25);
		this.btnResetDriv.TabIndex = 16;
		this.btnResetDriv.Text = "Reset";
		this.btnResetDriv.UseVisualStyleBackColor = false;
		this.btnResetDriv.Click += new System.EventHandler(btnResetDriv_Click);
		this.groupBox1.Controls.Add(this.picFPImg);
		this.groupBox1.Controls.Add(this.btnFpStart);
		this.groupBox1.Controls.Add(this.btnFpCapture);
		this.groupBox1.Controls.Add(this.btnReset);
		this.groupBox1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 440);
		this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.groupBox1.Size = new System.Drawing.Size(310, 183);
		this.groupBox1.TabIndex = 83;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "FingerPrint";
		this.picFPImg.Dock = System.Windows.Forms.DockStyle.Top;
		this.picFPImg.Location = new System.Drawing.Point(4, 19);
		this.picFPImg.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.picFPImg.Name = "picFPImg";
		this.picFPImg.Size = new System.Drawing.Size(302, 134);
		this.picFPImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picFPImg.TabIndex = 80;
		this.picFPImg.TabStop = false;
		this.btnFpStart.BackColor = System.Drawing.Color.Teal;
		this.btnFpStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnFpStart.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnFpStart.ForeColor = System.Drawing.Color.White;
		this.btnFpStart.Location = new System.Drawing.Point(4, 156);
		this.btnFpStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnFpStart.Name = "btnFpStart";
		this.btnFpStart.Size = new System.Drawing.Size(75, 25);
		this.btnFpStart.TabIndex = 18;
		this.btnFpStart.Text = "Start";
		this.btnFpStart.UseVisualStyleBackColor = false;
		this.btnFpStart.Click += new System.EventHandler(btnFpStart_Click);
		this.btnFpCapture.BackColor = System.Drawing.Color.Teal;
		this.btnFpCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnFpCapture.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnFpCapture.ForeColor = System.Drawing.Color.White;
		this.btnFpCapture.Location = new System.Drawing.Point(115, 156);
		this.btnFpCapture.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnFpCapture.Name = "btnFpCapture";
		this.btnFpCapture.Size = new System.Drawing.Size(85, 25);
		this.btnFpCapture.TabIndex = 19;
		this.btnFpCapture.Text = "Capture";
		this.btnFpCapture.UseVisualStyleBackColor = false;
		this.btnFpCapture.Click += new System.EventHandler(btnFpCapture_Click);
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(231, 156);
		this.btnReset.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(75, 25);
		this.btnReset.TabIndex = 20;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.tabPage2.Size = new System.Drawing.Size(1149, 633);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(4, 125);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1141, 505);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 46;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Controls.Add(this.label16);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(4, 98);
		this.panel12.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1141, 27);
		this.panel12.TabIndex = 86;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(5, 3);
		this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(142, 19);
		this.label16.TabIndex = 0;
		this.label16.Text = "Filtered Records";
		this.panel9.Controls.Add(this.groupBox2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(4, 55);
		this.panel9.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1141, 43);
		this.panel9.TabIndex = 1;
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 1);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(643, 41);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(548, 15);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Checked = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(462, 15);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.TabStop = true;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(395, 11);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 6;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(205, 17);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(3, 17);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Date";
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(254, 13);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(136, 23);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(66, 13);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(136, 23);
		this.FromDateHistory.TabIndex = 1;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(4, 3);
		this.panel7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1141, 52);
		this.panel7.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnNewHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1141, 27);
		this.toolStrip2.TabIndex = 6;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.BtnNewHistory.Text = "&Reset";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(4, 29);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(153, 19);
		this.label2.TabIndex = 0;
		this.label2.Text = "Driver Bio History";
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GridHistory);
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Controls.Add(this.toolStrip3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(345, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(800, 627);
		this.panel5.TabIndex = 1;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNewHistoryInFormTab });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(800, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNewHistoryInFormTab.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistoryInFormTab.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistoryInFormTab.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistoryInFormTab.Name = "btnNewHistoryInFormTab";
		this.btnNewHistoryInFormTab.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistoryInFormTab.Text = "&Reset";
		this.btnNewHistoryInFormTab.Click += new System.EventHandler(btnNewHistoryInFormTab_Click);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 27);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(800, 26);
		this.panel6.TabIndex = 8;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(5, 3);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(153, 19);
		this.label4.TabIndex = 1;
		this.label4.Text = "Driver Bio History";
		this.panel8.Controls.Add(this.groupBox3);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 53);
		this.panel8.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(800, 51);
		this.panel8.TabIndex = 9;
		this.groupBox3.Controls.Add(this.RdModifyDateFormTab);
		this.groupBox3.Controls.Add(this.RdEntryDateFromTab);
		this.groupBox3.Controls.Add(this.BtnShowInFormTab);
		this.groupBox3.Controls.Add(this.label13);
		this.groupBox3.Controls.Add(this.label15);
		this.groupBox3.Controls.Add(this.ToDateHistoryInFormTab);
		this.groupBox3.Controls.Add(this.FromDateHistoryInFormTab);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(643, 46);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.RdModifyDateFormTab.AutoSize = true;
		this.RdModifyDateFormTab.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdModifyDateFormTab.Location = new System.Drawing.Point(546, 17);
		this.RdModifyDateFormTab.Name = "RdModifyDateFormTab";
		this.RdModifyDateFormTab.Size = new System.Drawing.Size(91, 19);
		this.RdModifyDateFormTab.TabIndex = 9;
		this.RdModifyDateFormTab.Text = "Modify Date";
		this.RdModifyDateFormTab.UseVisualStyleBackColor = true;
		this.RdEntryDateFromTab.AutoSize = true;
		this.RdEntryDateFromTab.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdEntryDateFromTab.Location = new System.Drawing.Point(463, 17);
		this.RdEntryDateFromTab.Name = "RdEntryDateFromTab";
		this.RdEntryDateFromTab.Size = new System.Drawing.Size(80, 19);
		this.RdEntryDateFromTab.TabIndex = 8;
		this.RdEntryDateFromTab.Text = "Entry Date";
		this.RdEntryDateFromTab.UseVisualStyleBackColor = true;
		this.BtnShowInFormTab.BackColor = System.Drawing.Color.Teal;
		this.BtnShowInFormTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowInFormTab.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowInFormTab.ForeColor = System.Drawing.Color.White;
		this.BtnShowInFormTab.Location = new System.Drawing.Point(400, 13);
		this.BtnShowInFormTab.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowInFormTab.Name = "BtnShowInFormTab";
		this.BtnShowInFormTab.Size = new System.Drawing.Size(59, 26);
		this.BtnShowInFormTab.TabIndex = 6;
		this.BtnShowInFormTab.Text = "Show";
		this.BtnShowInFormTab.UseVisualStyleBackColor = false;
		this.BtnShowInFormTab.Click += new System.EventHandler(BtnShowInFormTab_Click);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(210, 19);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 84;
		this.label13.Text = "To Date";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(5, 19);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(63, 15);
		this.label15.TabIndex = 83;
		this.label15.Text = "From Date";
		this.ToDateHistoryInFormTab.CustomFormat = "dd-MMM-yy";
		this.ToDateHistoryInFormTab.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistoryInFormTab.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistoryInFormTab.Location = new System.Drawing.Point(262, 15);
		this.ToDateHistoryInFormTab.Name = "ToDateHistoryInFormTab";
		this.ToDateHistoryInFormTab.ShowCheckBox = true;
		this.ToDateHistoryInFormTab.Size = new System.Drawing.Size(136, 23);
		this.ToDateHistoryInFormTab.TabIndex = 2;
		this.FromDateHistoryInFormTab.CustomFormat = "dd-MMM-yy";
		this.FromDateHistoryInFormTab.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistoryInFormTab.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistoryInFormTab.Location = new System.Drawing.Point(70, 15);
		this.FromDateHistoryInFormTab.Name = "FromDateHistoryInFormTab";
		this.FromDateHistoryInFormTab.ShowCheckBox = true;
		this.FromDateHistoryInFormTab.Size = new System.Drawing.Size(136, 23);
		this.FromDateHistoryInFormTab.TabIndex = 1;
		this.GridHistory.AllowEdit = (InheritableBoolean)2;
		GridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridHistory.DesignTimeLayout = GridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridHistory.DynamicFiltering = true;
		this.GridHistory.FilterMode = (FilterMode)2;
		this.GridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridHistory).Location = new System.Drawing.Point(0, 131);
		((System.Windows.Forms.Control)(object)this.GridHistory).Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		((System.Windows.Forms.Control)(object)this.GridHistory).Name = "GridHistory";
		((ScrollableBase)this.GridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridHistory).Size = new System.Drawing.Size(800, 496);
		((System.Windows.Forms.Control)(object)this.GridHistory).TabIndex = 87;
		this.GridHistory.TotalRow = (InheritableBoolean)1;
		this.GridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridHistory).VisualStyle = (VisualStyle)4;
		this.GridHistory.ColumnButtonClick += new ColumnActionEventHandler(GridHistory_ColumnButtonClick);
		this.GridHistory.LinkClicked += new ColumnActionEventHandler(GridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridHistory).DoubleClick += new System.EventHandler(GridHistory_DoubleClick);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Controls.Add(this.label17);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 104);
		this.panel10.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(800, 27);
		this.panel10.TabIndex = 88;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(5, 3);
		this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(142, 19);
		this.label17.TabIndex = 0;
		this.label17.Text = "Filtered Records";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(759, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.GridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 11;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1092, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 24);
		this.ctrlGrdBar2.TabIndex = 11;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1157, 661);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		base.Name = "frmDriverBioDefinition";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmDriverBioDefinition_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmDriverBioDefinition_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.grp.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picBoxDriver).EndInit();
		this.groupBox1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picFPImg).EndInit();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
