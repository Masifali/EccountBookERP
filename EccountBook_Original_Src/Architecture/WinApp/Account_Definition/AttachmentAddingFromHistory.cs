using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.Account_Definition;

public class AttachmentAddingFromHistory : Form
{
	public List<TempAttachments> lst = new List<TempAttachments>();

	private string saveDirectory = "";

	private int DocId;

	private int DocumentTypeId;

	private string ScreenName = "";

	private UserAccount userAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Button Choose;

	private Button btnClose;

	private FlowLayoutPanel PicturesFlowLayout;

	private Panel panel2;

	private Button btnSave;

	private Panel panel3;

	public AttachmentAddingFromHistory(UserAccount user, int Id, int RefDocumentTypeId = 0, string screenName = "")
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		userAccount = user;
		DocId = Id;
		DocumentTypeId = RefDocumentTypeId;
		ScreenName = screenName;
	}

	private void Attachment_Load(object sender, EventArgs e)
	{
		try
		{
			saveDirectory = clsGlobalVariables.AttachmentFolderPath;
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
			if (lst.Count <= 0)
			{
				return;
			}
			PicturesFlowLayout.Controls.Clear();
			Choose.Enabled = false;
			PicturesFlowLayout.Enabled = false;
			AttachmentUploadControl[] AttachmentCards = new AttachmentUploadControl[lst.Count];
			for (int i = 0; i < AttachmentCards.Length; i++)
			{
				AttachmentCards[i] = new AttachmentUploadControl();
				string FileName = Path.GetFileName(lst[i].Path);
				AttachmentCards[i].path = Conversion.ToString((object)lst[i].Path);
				FileInfo fileInfo = new FileInfo(lst[i].Path);
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
				AttachmentCards[i].FileName = FileName;
				AttachmentCards[i].FileDate = fileInfo.CreationTime.ToShortDateString();
				AttachmentCards[i].FileSize = ((double)fileInfo.Length / 1048576.0).ToString("0.00") + " Mb";
				PicturesFlowLayout.Controls.Add(AttachmentCards[i]);
				AttachmentCards[i].UserControlClicked += UserControl_Click;
			}
			Choose.Enabled = true;
			PicturesFlowLayout.Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		AttachmentUploadControl obj = (AttachmentUploadControl)sender;
		string path = obj.path;
		try
		{
			if (obj.ActiveControl != null && obj.ActiveControl.Text == "Delete")
			{
				lst.RemoveAll((TempAttachments x) => x.Path == obj.path);
				PicturesFlowLayout.Controls.Remove(obj);
			}
			else if (File.Exists(path))
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
			openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.jfif)|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.jfif|Excel Files (*.xlsx;*.xlsm;*.xls;*.xlsb;*.xltx)|*.xlsx;*.xlsm;*.xls;*.xlsb;*.xltx|Word Documents (*.docx;*.doc)|*.docx;*.doc|PDF Files (*.pdf)|*.pdf|Text Files (*.txt)|*.txt|Compressed Files (*.zip;*.rar)|*.zip;*.rar|All Files (*.*)|*.*";
			openFileDialog.Title = "Select a File";
			openFileDialog.Multiselect = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string[] fileNames = openFileDialog.FileNames;
				foreach (string file in fileNames)
				{
					if (lst.Where((TempAttachments row) => row.Path.Contains(file)).ToList().Count == 0)
					{
						TempAttachments TA = new TempAttachments();
						TA.Id = 0;
						TA.Path = file;
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
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		if (lst.Count > 0 && MessageBox.Show("Are you sure to Close?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
		{
			return;
		}
		foreach (Control control in PicturesFlowLayout.Controls)
		{
			control.Click -= UserControl_Click;
			control.Dispose();
		}
		PicturesFlowLayout.Controls.Clear();
		lst.Clear();
		Dispose();
		Close();
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		try
		{
			if (lst.Count == 0)
			{
				throw new Exception("No record found for save");
			}
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			DMSAttachments obj = new DMSAttachments();
			obj.AttachmentList = new List<DMSAttachments>();
			for (int i = 0; i < lst.Count; i++)
			{
				double FileSize = 0.0;
				FileInfo info = new FileInfo(lst[i].Path);
				if (info.Exists)
				{
					FileSize = (double)info.Length / 1048576.0;
				}
				if (FileSize > 5.0)
				{
					MessageBox.Show("File Size Exceeds 5MB Of File: " + Path.GetFileName(lst[i].Path), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				DMSAttachments item = new DMSAttachments();
				item.RefAccountId = 0;
				item.RefDocumentTypeId = DocumentTypeId;
				item.ScreenName = ScreenName;
				item.RefDocumentNo = DocId;
				item.EntryDate = DateTime.Now;
				item.ModifyDate = DateTime.Now;
				item.EntryUser = userAccount.ID;
				item.ModifyUser = userAccount.ID;
				item.OrganizationId = userAccount.OrganizationId;
				item.CompanyId = userAccount.CompanyId;
				item.Attachment = lst[i].Path;
				obj.AttachmentList.Add(item);
			}
			DMSAttachments.SaveList(obj);
			MessageBox.Show("Save Successfully");
			lst.Clear();
			PicturesFlowLayout.Controls.Clear();
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
				foreach (Control control in PicturesFlowLayout.Controls)
				{
					control.Click -= UserControl_Click;
					control.Dispose();
				}
				PicturesFlowLayout.Controls.Clear();
				Dispose();
				Hide();
				lst.Clear();
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
		this.panel3 = new System.Windows.Forms.Panel();
		this.PicturesFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnSave = new System.Windows.Forms.Button();
		this.Choose = new System.Windows.Forms.Button();
		this.btnClose = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(820, 515);
		this.panel1.TabIndex = 0;
		this.panel3.Controls.Add(this.PicturesFlowLayout);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 38);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(820, 477);
		this.panel3.TabIndex = 54;
		this.PicturesFlowLayout.AutoScroll = true;
		this.PicturesFlowLayout.BackColor = System.Drawing.Color.White;
		this.PicturesFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PicturesFlowLayout.Location = new System.Drawing.Point(0, 0);
		this.PicturesFlowLayout.Name = "PicturesFlowLayout";
		this.PicturesFlowLayout.Size = new System.Drawing.Size(820, 477);
		this.PicturesFlowLayout.TabIndex = 52;
		this.panel2.Controls.Add(this.btnSave);
		this.panel2.Controls.Add(this.Choose);
		this.panel2.Controls.Add(this.btnClose);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(820, 38);
		this.panel2.TabIndex = 53;
		this.btnSave.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Location = new System.Drawing.Point(73, 6);
		this.btnSave.Margin = new System.Windows.Forms.Padding(2);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(65, 26);
		this.btnSave.TabIndex = 5;
		this.btnSave.Text = "Save";
		this.btnSave.UseVisualStyleBackColor = true;
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.Choose.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Choose.Location = new System.Drawing.Point(6, 6);
		this.Choose.Margin = new System.Windows.Forms.Padding(2);
		this.Choose.Name = "Choose";
		this.Choose.Size = new System.Drawing.Size(65, 26);
		this.Choose.TabIndex = 1;
		this.Choose.Text = "Browse";
		this.Choose.UseVisualStyleBackColor = true;
		this.Choose.Click += new System.EventHandler(Choose_Click);
		this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnClose.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnClose.Location = new System.Drawing.Point(760, 5);
		this.btnClose.Margin = new System.Windows.Forms.Padding(2);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(56, 27);
		this.btnClose.TabIndex = 4;
		this.btnClose.Text = "Close";
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(820, 515);
		base.ControlBox = false;
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "AttachmentAddingFromHistory";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Attachment";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Attachment_FormClosing);
		base.Load += new System.EventHandler(Attachment_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Attachment_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
