using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.VPS;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;

namespace Architecture.WinApp;

public class AttachmentViewWithDeleteOption : Form
{
	private bool IsVpsAttachmentsServiceOn;

	private ServerCredentials Sc = new ServerCredentials();

	public bool ShowSaveButton;

	private int DocId;

	private int DocumentTypeId;

	private string ScreenName = "";

	private DataTable dtAttachment = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Label label1;

	private Panel panel2;

	private Panel panel4;

	private Label label5;

	private Label lblTotalFiles;

	private Label lblTotalSize;

	private Label label2;

	private FlowLayoutPanel FlowLayoutAttachmentPreview;

	private Panel panel5;

	private Button btnSave;

	private Button Choose;

	public AttachmentViewWithDeleteOption(UserAccount user, int Id, int RefDocumentTypeId = 0, string screenName = "")
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		DocId = Id;
		DocumentTypeId = RefDocumentTypeId;
		ScreenName = screenName;
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		try
		{
			panel5.Visible = ShowSaveButton;
			if (dtAttachment.Columns.Count == 0)
			{
				dtAttachment.Columns.Add("Id");
				dtAttachment.Columns.Add("AttachmentName");
				dtAttachment.Columns.Add("CustomName");
				dtAttachment.Columns.Add("EntryDate", typeof(DateTime));
			}
			GetSavedAttachmentsAgainstDocument();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetSavedAttachmentsAgainstDocument()
	{
		try
		{
			dtAttachment.Rows.Clear();
			FlowLayoutAttachmentPreview.Controls.Clear();
			List<DMSAttachments> dMSList = new List<DMSAttachments>();
			dMSList = ((DocumentTypeId <= 0) ? DMSAttachments.GetByID(DocId, ScreenName) : DMSAttachments.ReadAttachmentsbyRefDocumentTypeId(DocId, DocumentTypeId));
			if (dMSList.Count <= 0)
			{
				return;
			}
			foreach (DMSAttachments itemList in dMSList)
			{
				dtAttachment.Rows.Add(itemList.Id, itemList.Attachment, itemList.UploadedFileCustomName, itemList.EntryDate);
			}
			AddAttachmentsToDataTable();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddAttachmentsToDataTable()
	{
		try
		{
			if (dtAttachment.Rows.Count <= 0)
			{
				return;
			}
			FlowLayoutAttachmentPreview.Controls.Clear();
			lblTotalFiles.Text = Conversion.ToString((object)dtAttachment.Rows.Count);
			string saveDirectory = "";
			saveDirectory = clsGlobalVariables.AttachmentFolderPath;
			double TotalFileSizeInMbs = 0.0;
			foreach (Control control in FlowLayoutAttachmentPreview.Controls)
			{
				control.Click -= UserControl_Click;
				control.Dispose();
			}
			IsVpsAttachmentsServiceOn = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("IsVpsAttachmentsServiceOn"));
			AttachmentPreviewControl[] AttachmentCards = new AttachmentPreviewControl[dtAttachment.Rows.Count];
			for (int i = 0; i < AttachmentCards.Length; i++)
			{
				string ForPath = ((Conversion.ToString(dtAttachment.Rows[i]["CustomName"]) != null && Conversion.ToString(dtAttachment.Rows[i]["CustomName"]) != string.Empty) ? Conversion.ToString(dtAttachment.Rows[i]["CustomName"]) : Conversion.ToString(dtAttachment.Rows[i]["AttachmentName"]));
				AttachmentCards[i] = new AttachmentPreviewControl();
				AttachmentCards[i].btnDelete.Visible = true;
				string AttachmentName = Path.GetFileName(ForPath);
				string path = "";
				if (IsVpsAttachmentsServiceOn)
				{
					Sc = CommonServices.GetServerCredentials();
					string uri = "ftp://" + Sc.ServerIp + "/" + AttachmentName;
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
						TotalFileSizeInMbs += fileSizeInMb;
						AttachmentCards[i].FileSize = fileSizeInMb.ToString("0.000") + " Mb";
						Path.GetExtension(Path.GetFileName(request.RequestUri.LocalPath))?.ToUpper();
						memoryStream.Position = 0L;
						byte[] streamData = memoryStream.ToArray();
						AttachmentCards[i].memoryStream = streamData;
					}
					catch (WebException webEx)
					{
						if (webEx.Response is FtpWebResponse { StatusCode: FtpStatusCode.ActionNotTakenFileUnavailable })
						{
							Console.WriteLine("[INFO] Skipping missing FTP file: " + AttachmentName);
						}
						else
						{
							Console.WriteLine("[FTP ERROR] " + webEx.Message);
						}
						continue;
					}
					catch (Exception ex)
					{
						throw new Exception("Error: " + ex.Message);
					}
					AttachmentCards[i].path = "";
				}
				else
				{
					path = Path.Combine(saveDirectory, ForPath);
					AttachmentCards[i].path = path;
					FileInfo fileInfo = new FileInfo(path);
					if (!fileInfo.Exists)
					{
						continue;
					}
					TotalFileSizeInMbs += (double)fileInfo.Length / 1048576.0;
					AttachmentCards[i].FileSize = ((double)fileInfo.Length / 1048576.0).ToString("0.000") + " Mb";
				}
				AttachmentCards[i].AttachmentId = Conversion.ToInt(dtAttachment.Rows[i]["Id"]);
				AttachmentCards[i].FileName = Path.GetFileName(Conversion.ToString(dtAttachment.Rows[i]["AttachmentName"]));
				AttachmentCards[i].FileDate = Conversion.ToDateTime(dtAttachment.Rows[i]["EntryDate"]).ToString("dd-MMM-yyyy hh:mm tt");
				FlowLayoutAttachmentPreview.Controls.Add(AttachmentCards[i]);
				AttachmentCards[i].UserControlClicked += UserControl_Click;
			}
			lblTotalSize.Text = TotalFileSizeInMbs.ToString("0.000");
		}
		catch (Exception ex2)
		{
			throw new Exception(ex2.Message);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		try
		{
			AttachmentPreviewControl obj = (AttachmentPreviewControl)sender;
			string path = obj.path;
			int Id = obj.AttachmentId;
			if (obj.ActiveControl != null && obj.ActiveControl.Text == "Delete")
			{
				if (Id > 0)
				{
					if (MessageBox.Show("Are you sure to delete this attachment? This is permanent delete", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					DMSAttachments.AttachmentDeleteByID(Id);
					if (!IsVpsAttachmentsServiceOn)
					{
						if (File.Exists(path))
						{
							obj.PictureBox.Image = null;
							obj.PictureBox.Invalidate();
							using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
							{
								fs.Flush();
								fs.Close();
								fs.Dispose();
							}
							bool fileDeleted = false;
							int attempts = 0;
							while (!fileDeleted && attempts < 5)
							{
								try
								{
									File.Delete(path);
									fileDeleted = true;
								}
								catch (IOException)
								{
									attempts++;
									Thread.Sleep(500);
								}
							}
						}
					}
					else
					{
						string uri = "ftp://" + Sc.ServerIp + "/" + path;
						try
						{
							FtpWebRequest obj2 = (FtpWebRequest)WebRequest.Create(uri);
							obj2.Method = "DELE";
							obj2.UsePassive = true;
							obj2.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
							using FtpWebResponse deleteResponse = (FtpWebResponse)obj2.GetResponse();
							if (deleteResponse.StatusCode != FtpStatusCode.FileActionOK)
							{
								throw new Exception("Failed to delete file from VPS.");
							}
						}
						catch (Exception)
						{
						}
					}
				}
				obj.PictureBox = null;
				foreach (DataRow item in (from row in dtAttachment.AsEnumerable()
					where Path.GetFileName(Conversion.ToString(row["AttachmentName"])) == obj.FileName
					select row).ToList())
				{
					item.Delete();
				}
				FlowLayoutAttachmentPreview.Controls.Remove(obj);
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
			using MemoryStream memoryStream = new MemoryStream(obj.memoryStream);
			memoryStream.Position = 0L;
			string fileExtension = Path.GetExtension(obj.FileName).ToLower();
			string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + fileExtension);
			try
			{
				using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
				{
					memoryStream.CopyTo(fileStream);
				}
				ProcessStartInfo processStartInfo = new ProcessStartInfo(tempFilePath)
				{
					UseShellExecute = true,
					Verb = "open"
				};
				try
				{
					Process.Start(processStartInfo);
				}
				catch (Exception ex3)
				{
					MessageBox.Show("Could not open the file: " + ex3.Message);
					return;
				}
				Task.Run(async delegate
				{
					await Task.Delay(10000);
					for (int i = 0; i < 3; i++)
					{
						try
						{
							if (File.Exists(tempFilePath))
							{
								File.Delete(tempFilePath);
							}
							break;
						}
						catch (IOException)
						{
							await Task.Delay(1000);
						}
					}
				});
			}
			catch (Exception ex2)
			{
				MessageBox.Show("An error occurred while trying to open the file: " + ex2.Message);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("An unexpected error occurred: " + ex.Message);
		}
	}

	private void AttachmentView_FormClosing(object sender, FormClosingEventArgs e)
	{
		foreach (Control control in FlowLayoutAttachmentPreview.Controls)
		{
			control.Click -= UserControl_Click;
			control.Dispose();
		}
		FlowLayoutAttachmentPreview.Controls.Clear();
		Dispose();
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		try
		{
			if (!dtAttachment.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["Id"]) == 0))
			{
				throw new Exception("No record found for save");
			}
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			DMSAttachments obj = new DMSAttachments();
			obj.AttachmentList = new List<DMSAttachments>();
			foreach (DataRow item in from r in dtAttachment.AsEnumerable()
				where Conversion.ToInt(r["Id"]) == 0
				select r)
			{
				string filePath = Conversion.ToString(item["AttachmentName"]);
				FileInfo info = new FileInfo(filePath);
				if (info.Exists && (double)info.Length / 1048576.0 > 5.0)
				{
					MessageBox.Show("File Size Exceeds 5MB Of File: " + Path.GetFileName(filePath), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				obj.AttachmentList.Add(new DMSAttachments
				{
					RefAccountId = 0,
					RefDocumentTypeId = DocumentTypeId,
					ScreenName = ScreenName,
					RefDocumentNo = DocId,
					EntryDate = DateTime.Now,
					ModifyDate = DateTime.Now,
					EntryUser = UserAccount.ID,
					ModifyUser = UserAccount.ID,
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Attachment = filePath
				});
			}
			DMSAttachments.SaveList(obj);
			MessageBox.Show("Save Successfully");
			dtAttachment.Clear();
			FlowLayoutAttachmentPreview.Controls.Clear();
			GetSavedAttachmentsAgainstDocument();
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
			openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.jfif)|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.jfif|Excel Files (*.xlsx;*.xlsm;*.xls;*.xlsb;*.xltx)|*.xlsx;*.xlsm;*.xls;*.xlsb;*.xltx|Word Documents (*.docx;*.doc)|*.docx;*.doc|PDF Files (*.pdf)|*.pdf|Text Files (*.txt)|*.txt|Compressed Files (*.zip;*.rar)|*.zip;*.rar|All Files (*.*)|*.*";
			openFileDialog.Title = "Select a File";
			openFileDialog.Multiselect = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string[] fileNames = openFileDialog.FileNames;
				foreach (string file in fileNames)
				{
					if ((from row in dtAttachment.AsEnumerable()
						where Conversion.ToString(row["AttachmentName"]).Contains(file)
						select row).ToList().Count == 0)
					{
						dtAttachment.Rows.Add(0, file, "", DateTime.Now);
					}
					else
					{
						MessageBox.Show(Path.GetFileName(file) + " File Already Present");
					}
				}
			}
			if (dtAttachment.Rows.Count > 0)
			{
				AddAttachmentsToDataTable();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.FlowLayoutAttachmentPreview = new System.Windows.Forms.FlowLayoutPanel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.lblTotalFiles = new System.Windows.Forms.Label();
		this.lblTotalSize = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnSave = new System.Windows.Forms.Button();
		this.Choose = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.FlowLayoutAttachmentPreview);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(405, 700);
		this.panel1.TabIndex = 8;
		this.FlowLayoutAttachmentPreview.AutoScroll = true;
		this.FlowLayoutAttachmentPreview.Dock = System.Windows.Forms.DockStyle.Left;
		this.FlowLayoutAttachmentPreview.Location = new System.Drawing.Point(0, 99);
		this.FlowLayoutAttachmentPreview.Name = "FlowLayoutAttachmentPreview";
		this.FlowLayoutAttachmentPreview.Size = new System.Drawing.Size(404, 601);
		this.FlowLayoutAttachmentPreview.TabIndex = 7;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 43);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(405, 56);
		this.panel2.TabIndex = 6;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.lblTotalFiles);
		this.panel4.Controls.Add(this.lblTotalSize);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(404, 55);
		this.panel4.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(228, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(152, 25);
		this.label5.TabIndex = 3;
		this.label5.Text = "Total Size (Mbs)";
		this.lblTotalFiles.AutoSize = true;
		this.lblTotalFiles.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalFiles.ForeColor = System.Drawing.Color.White;
		this.lblTotalFiles.Location = new System.Drawing.Point(45, 28);
		this.lblTotalFiles.Name = "lblTotalFiles";
		this.lblTotalFiles.Size = new System.Drawing.Size(23, 25);
		this.lblTotalFiles.TabIndex = 2;
		this.lblTotalFiles.Text = "0";
		this.lblTotalSize.AutoSize = true;
		this.lblTotalSize.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalSize.ForeColor = System.Drawing.Color.White;
		this.lblTotalSize.Location = new System.Drawing.Point(266, 28);
		this.lblTotalSize.Name = "lblTotalSize";
		this.lblTotalSize.Size = new System.Drawing.Size(23, 25);
		this.lblTotalSize.TabIndex = 1;
		this.lblTotalSize.Text = "0";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(12, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(98, 25);
		this.label2.TabIndex = 0;
		this.label2.Text = "Total Files";
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(405, 43);
		this.panel3.TabIndex = 5;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(123, 25);
		this.label1.TabIndex = 0;
		this.label1.Text = "Attachments";
		this.panel5.Controls.Add(this.btnSave);
		this.panel5.Controls.Add(this.Choose);
		this.panel5.Location = new System.Drawing.Point(262, 4);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(140, 34);
		this.panel5.TabIndex = 1;
		this.panel5.Visible = false;
		this.btnSave.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Location = new System.Drawing.Point(72, 4);
		this.btnSave.Margin = new System.Windows.Forms.Padding(2);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(65, 26);
		this.btnSave.TabIndex = 7;
		this.btnSave.Text = "Save";
		this.btnSave.UseVisualStyleBackColor = true;
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.Choose.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Choose.Location = new System.Drawing.Point(5, 4);
		this.Choose.Margin = new System.Windows.Forms.Padding(2);
		this.Choose.Name = "Choose";
		this.Choose.Size = new System.Drawing.Size(65, 26);
		this.Choose.TabIndex = 6;
		this.Choose.Text = "Browse";
		this.Choose.UseVisualStyleBackColor = true;
		this.Choose.Click += new System.EventHandler(Choose_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(405, 700);
		base.Controls.Add(this.panel1);
		base.Name = "AttachmentViewWithDeleteOption";
		this.Text = "EccountBook";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AttachmentView_FormClosing);
		base.Load += new System.EventHandler(Form1_Load);
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel5.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
