using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.DMS_FolderHierarchy;

public class DMSAttachmentsUploadControl : UserControl
{
	private string _Path;

	private string _FileName;

	private string _FileSize;

	private IContainer components;

	private Button btnDelete;

	private Label lblFileSize;

	private Label lblFileName;

	private PictureBox PictureBox;

	public string path
	{
		get
		{
			return _Path;
		}
		set
		{
			_Path = value;
		}
	}

	public string FileName
	{
		get
		{
			return _FileName;
		}
		set
		{
			_FileName = value;
			lblFileName.Text = value;
		}
	}

	public string FileSize
	{
		get
		{
			return _FileSize;
		}
		set
		{
			_FileSize = value;
			lblFileSize.Text = value;
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public DMSAttachmentsUploadControl()
	{
		InitializeComponent();
		PictureBox.Click += ControlClicked;
		base.Click += ControlClicked;
		btnDelete.Click += ControlClicked;
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void DMSAttachmentsUploadControl_Load(object sender, EventArgs e)
	{
		try
		{
			FileInfo file = new FileInfo(path);
			if (file.Extension.ToUpper() == ".JPG" || file.Extension.ToUpper() == ".JPEG" || file.Extension.ToUpper() == ".PNG" || file.Extension.ToUpper() == ".GIF" || file.Extension.ToUpper() == ".BMP" || file.Extension.ToUpper() == ".JFIF")
			{
				PictureBox.Image = Image.FromFile(path);
			}
			else if (file.Extension.ToUpper() == ".XLSX" || file.Extension.ToUpper() == ".XLSM" || file.Extension.ToUpper() == ".XLS" || file.Extension.ToUpper() == ".XLSB" || file.Extension.ToUpper() == ".XLTX")
			{
				PictureBox.Image = Resources.Microsoft_Excel_Icon;
			}
			else if (file.Extension.ToUpper() == ".DOCX" || file.Extension.ToUpper() == ".DOC")
			{
				PictureBox.Image = Resources.Microsoft_Word_Logo;
			}
			else if (file.Extension.ToUpper() == ".PDF")
			{
				PictureBox.Image = Resources.Pdf_Logo;
			}
			else if (file.Extension.ToUpper() == ".TXT")
			{
				PictureBox.Image = Resources.txtFile_Logo;
			}
			else if (file.Extension.ToUpper() == ".ZIP" || file.Extension.ToUpper() == ".RAR")
			{
				PictureBox.Image = Resources.Zip_Logo;
			}
			else
			{
				PictureBox.Image = Resources.No_Preview;
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
		this.btnDelete = new System.Windows.Forms.Button();
		this.lblFileSize = new System.Windows.Forms.Label();
		this.lblFileName = new System.Windows.Forms.Label();
		this.PictureBox = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.PictureBox).BeginInit();
		base.SuspendLayout();
		this.btnDelete.Location = new System.Drawing.Point(50, 167);
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(71, 23);
		this.btnDelete.TabIndex = 12;
		this.btnDelete.Text = "Delete";
		this.btnDelete.UseVisualStyleBackColor = true;
		this.lblFileSize.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFileSize.Location = new System.Drawing.Point(6, 138);
		this.lblFileSize.Name = "lblFileSize";
		this.lblFileSize.Size = new System.Drawing.Size(165, 26);
		this.lblFileSize.TabIndex = 11;
		this.lblFileSize.Text = "0";
		this.lblFileSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFileName.AutoEllipsis = true;
		this.lblFileName.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFileName.Location = new System.Drawing.Point(3, 4);
		this.lblFileName.Name = "lblFileName";
		this.lblFileName.Size = new System.Drawing.Size(168, 32);
		this.lblFileName.TabIndex = 10;
		this.lblFileName.Text = "Name";
		this.PictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
		this.PictureBox.Image = Architecture.WinApp.Properties.Resources.No_Preview;
		this.PictureBox.Location = new System.Drawing.Point(6, 39);
		this.PictureBox.Name = "PictureBox";
		this.PictureBox.Size = new System.Drawing.Size(165, 96);
		this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.PictureBox.TabIndex = 9;
		this.PictureBox.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.btnDelete);
		base.Controls.Add(this.lblFileSize);
		base.Controls.Add(this.lblFileName);
		base.Controls.Add(this.PictureBox);
		base.Name = "DMSAttachmentsUploadControl";
		base.Size = new System.Drawing.Size(179, 194);
		base.Load += new System.EventHandler(DMSAttachmentsUploadControl_Load);
		((System.ComponentModel.ISupportInitialize)this.PictureBox).EndInit();
		base.ResumeLayout(false);
	}
}
