using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.Common;
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

public class AttachmentForDetailRows : Form
{
	public List<TempAttachmentsDetail> lst = new List<TempAttachmentsDetail>();

	public List<TempAttachmentsDetail> RemovedAttachmentListInUpdateCase = new List<TempAttachmentsDetail>();

	public int ReferecneId;

	public int RefAccountId;

	private bool IsVpsAttachmentsServiceOn;

	private IContainer components;

	private Panel panel1;

	private Button Choose;

	private ContextMenuStrip contextMenuStrip1;

	private Button btnClose;

	private FlowLayoutPanel PicturesFlowLayout;

	private Label lblPercent;

	private ProgressBar progressBar1;

	private BackgroundWorker backgroundWorker;

	private Panel panel2;

	private Panel panel3;

	public AttachmentForDetailRows()
	{
		InitializeComponent();
	}

	private void Attachment_Load(object sender, EventArgs e)
	{
		try
		{
			IsVpsAttachmentsServiceOn = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("IsVpsAttachmentsServiceOn"));
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
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		try
		{
			PicturesFlowLayout.Controls.Clear();
			if (lst.Count <= 0)
			{
				return;
			}
			ServerCredentials Sc = new ServerCredentials();
			Sc = CommonServices.GetServerCredentials();
			List<TempAttachmentsDetail> lstForReference = new List<TempAttachmentsDetail>();
			lstForReference = lst.Where((TempAttachmentsDetail r) => r.DetailRefId == ReferecneId).ToList();
			if (lstForReference.Count <= 0)
			{
				return;
			}
			Choose.Enabled = false;
			PicturesFlowLayout.Enabled = false;
			backgroundWorker.RunWorkerAsync();
			progressBar1.Show();
			AttachmentUploadControl[] AttachmentCards = new AttachmentUploadControl[lstForReference.Count];
			for (int i = 0; i < AttachmentCards.Length; i++)
			{
				AttachmentCards[i] = new AttachmentUploadControl();
				string FileName = ((lstForReference[i].CustomPath == null || lstForReference[i].CustomPath == string.Empty) ? Path.GetFileName(Conversion.ToString((object)lstForReference[i].Path)) : lstForReference[i].CustomPath);
				AttachmentCards[i].FileName = Path.GetFileName(lstForReference[i].Path);
				AttachmentCards[i].ActionId = ((lstForReference[i].Id <= 0) ? 1 : 2);
				if (!IsVpsAttachmentsServiceOn)
				{
					string DestinationPath = clsGlobalVariables.AttachmentFolderPath + FileName;
					FileInfo fileInfo = null;
					if (File.Exists(DestinationPath))
					{
						AttachmentCards[i].path = DestinationPath;
						fileInfo = new FileInfo(DestinationPath);
					}
					else
					{
						AttachmentCards[i].path = Conversion.ToString((object)lstForReference[i].Path);
						fileInfo = new FileInfo(Conversion.ToString((object)lstForReference[i].Path));
						if (AttachmentCards[i].ActionId == 2 && !fileInfo.Exists)
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
				if (lstForReference[i].Id > 0)
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
						if (webEx.Response is FtpWebResponse response)
						{
							throw new Exception("FTP Error: " + response.StatusDescription + " - " + webEx.Message);
						}
						throw new Exception("Error: " + webEx.Message);
					}
					catch (Exception ex)
					{
						throw new Exception("Error: " + ex.Message);
					}
					AttachmentCards[i].path = lstForReference[i].CustomPath;
					continue;
				}
				FileInfo fileInfo2 = null;
				AttachmentCards[i].path = lstForReference[i].Path;
				fileInfo2 = new FileInfo(lstForReference[i].Path);
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

	public void DeleteAttachments(int ReferenceId)
	{
		try
		{
			List<TempAttachmentsDetail> lstToDelete = lst.Where((TempAttachmentsDetail r) => r.DetailRefId == ReferenceId).ToList();
			int i;
			for (i = 0; i < lstToDelete.Count; i++)
			{
				if (!IsVpsAttachmentsServiceOn)
				{
					if (File.Exists(clsGlobalVariables.AttachmentFolderPath + Path.GetFileName(lstToDelete[i].Path)))
					{
						TempAttachmentsDetail TA = new TempAttachmentsDetail();
						TA.Id = 0;
						TA.Path = Path.Combine(clsGlobalVariables.AttachmentFolderPath, lstToDelete[i].Path);
						TA.DetailRefId = lstToDelete[i].DetailRefId;
						RemovedAttachmentListInUpdateCase.Add(TA);
						lst.RemoveAll((TempAttachmentsDetail x) => x.Path == Path.GetFileName(lstToDelete[i].Path));
					}
					else
					{
						lst.RemoveAll((TempAttachmentsDetail item) => item.Path == lstToDelete[i].Path);
					}
					continue;
				}
				bool FileExist = false;
				if (lstToDelete[i].Id > 0)
				{
					ServerCredentials Sc = CommonServices.GetServerCredentials();
					string uri = "ftp://" + Sc.ServerIp + "/" + Path.GetFileName(lstToDelete[i].Path);
					try
					{
						FtpWebRequest obj = (FtpWebRequest)WebRequest.Create(uri);
						obj.Method = "MDTM";
						obj.UsePassive = true;
						obj.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using ((FtpWebResponse)obj.GetResponse())
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
					TempAttachmentsDetail TA2 = new TempAttachmentsDetail();
					TA2.Id = 0;
					TA2.Path = lstToDelete[i].Path;
					TA2.DetailRefId = lstToDelete[i].DetailRefId;
					RemovedAttachmentListInUpdateCase.Add(TA2);
					lst.RemoveAll((TempAttachmentsDetail x) => x.Path == Path.GetFileName(lstToDelete[i].Path));
				}
				else
				{
					lst.RemoveAll((TempAttachmentsDetail item) => item.Path == lstToDelete[i].Path);
				}
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
			string customPath = lst.Find((TempAttachmentsDetail x) => Path.GetFileName(x.Path) == obj.FileName && x.DetailRefId == ReferecneId).CustomPath;
			if (obj.ActiveControl != null && obj.ActiveControl.Text == "Delete")
			{
				if (!IsVpsAttachmentsServiceOn)
				{
					if (File.Exists(clsGlobalVariables.AttachmentFolderPath + customPath))
					{
						obj.PictureBox.Image = null;
						TempAttachmentsDetail TA = new TempAttachmentsDetail();
						TA.Id = 0;
						TA.Path = obj.path;
						TA.CustomPath = customPath;
						RemovedAttachmentListInUpdateCase.Add(TA);
						lst.RemoveAll((TempAttachmentsDetail x) => x.CustomPath == customPath);
						PicturesFlowLayout.Controls.Remove(obj);
					}
					else
					{
						PicturesFlowLayout.Controls.Remove(obj);
						lst.RemoveAll((TempAttachmentsDetail x) => x.Path == obj.path);
					}
					return;
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
					TempAttachmentsDetail TA2 = new TempAttachmentsDetail();
					TA2.Id = 0;
					TA2.Path = obj.path;
					TA2.CustomPath = customPath;
					RemovedAttachmentListInUpdateCase.Add(TA2);
					lst.RemoveAll((TempAttachmentsDetail x) => x.CustomPath == customPath);
					PicturesFlowLayout.Controls.Remove(obj);
				}
				else
				{
					lst.RemoveAll((TempAttachmentsDetail x) => x.Path == obj.path);
					PicturesFlowLayout.Controls.Remove(obj);
				}
				return;
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
			openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.jfif)|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.jfif|Excel Files (*.xlsx;*.xlsm;*.xls;*.xlsb;*.xltx)|*.xlsx;*.xlsm;*.xls;*.xlsb;*.xltx|Word Documents (*.docx;*.doc)|*.docx;*.doc|PDF Files (*.pdf)|*.pdf|Text Files (*.txt)|*.txt|Compressed Files (*.zip;*.rar)|*.zip;*.rar|All Files (*.*)|*.*";
			openFileDialog.Title = "Select a File";
			openFileDialog.Multiselect = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				progressBar1.Value = 0;
				lblPercent.Visible = true;
				string[] fileNames = openFileDialog.FileNames;
				foreach (string file in fileNames)
				{
					if (lst.Where((TempAttachmentsDetail row) => row.Path.Contains(file) && row.DetailRefId == ReferecneId).ToList().Count == 0)
					{
						TempAttachmentsDetail TA = new TempAttachmentsDetail();
						TA.Id = 0;
						TA.Path = file;
						TA.DetailRefId = ReferecneId;
						TA.CustomPath = "";
						TA.RefAccountId = RefAccountId;
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

	private void Attachment_FormClosing(object sender, FormClosingEventArgs e)
	{
		Hide();
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		try
		{
			if (backgroundWorker.IsBusy)
			{
				backgroundWorker.CancelAsync();
			}
			while (backgroundWorker.IsBusy)
			{
				Application.DoEvents();
			}
			backgroundWorker.Dispose();
			Hide();
			ReferecneId = 0;
			RefAccountId = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error while closing: " + ex.Message);
		}
	}

	private void Attachment_KeyDown(object sender, KeyEventArgs e)
	{
		if (!e.Control || e.KeyCode != Keys.E)
		{
			_ = e.KeyCode;
			_ = 27;
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
				backgroundWorker.WorkerSupportsCancellation = true;
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
		this.panel2 = new System.Windows.Forms.Panel();
		this.PicturesFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.progressBar1 = new System.Windows.Forms.ProgressBar();
		this.lblPercent = new System.Windows.Forms.Label();
		this.Choose = new System.Windows.Forms.Button();
		this.btnClose = new System.Windows.Forms.Button();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(824, 558);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.PicturesFlowLayout);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 39);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(824, 519);
		this.panel2.TabIndex = 53;
		this.PicturesFlowLayout.AutoScroll = true;
		this.PicturesFlowLayout.BackColor = System.Drawing.Color.White;
		this.PicturesFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PicturesFlowLayout.Location = new System.Drawing.Point(0, 0);
		this.PicturesFlowLayout.Name = "PicturesFlowLayout";
		this.PicturesFlowLayout.Size = new System.Drawing.Size(824, 519);
		this.PicturesFlowLayout.TabIndex = 52;
		this.panel3.Controls.Add(this.progressBar1);
		this.panel3.Controls.Add(this.lblPercent);
		this.panel3.Controls.Add(this.Choose);
		this.panel3.Controls.Add(this.btnClose);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(824, 39);
		this.panel3.TabIndex = 17;
		this.progressBar1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		this.progressBar1.ForeColor = System.Drawing.Color.Green;
		this.progressBar1.Location = new System.Drawing.Point(166, 5);
		this.progressBar1.Name = "progressBar1";
		this.progressBar1.Size = new System.Drawing.Size(501, 28);
		this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
		this.progressBar1.TabIndex = 15;
		this.progressBar1.UseWaitCursor = true;
		this.lblPercent.AutoSize = true;
		this.lblPercent.Location = new System.Drawing.Point(129, 12);
		this.lblPercent.Name = "lblPercent";
		this.lblPercent.Size = new System.Drawing.Size(24, 13);
		this.lblPercent.TabIndex = 16;
		this.lblPercent.Text = "0 %";
		this.lblPercent.UseWaitCursor = true;
		this.Choose.Location = new System.Drawing.Point(5, 7);
		this.Choose.Margin = new System.Windows.Forms.Padding(2);
		this.Choose.Name = "Choose";
		this.Choose.Size = new System.Drawing.Size(56, 26);
		this.Choose.TabIndex = 1;
		this.Choose.Text = "Browse";
		this.Choose.UseVisualStyleBackColor = true;
		this.Choose.Click += new System.EventHandler(Choose_Click);
		this.btnClose.Location = new System.Drawing.Point(760, 7);
		this.btnClose.Margin = new System.Windows.Forms.Padding(2);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(56, 27);
		this.btnClose.TabIndex = 4;
		this.btnClose.Text = "Close";
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_DoWork);
		this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(824, 558);
		base.ControlBox = false;
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "AttachmentForDetailRows";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Attachment";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Attachment_FormClosing);
		base.Load += new System.EventHandler(Attachment_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Attachment_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
