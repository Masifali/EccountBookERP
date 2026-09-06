using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.DMS_FolderHierarchy;

public class FolderHierarchyAttachments : UserControl
{
	private string _FromUSer;

	private string _Path;

	private bool _IsSelected;

	private int _DocFileId;

	private string _FileName;

	private string _FileDate;

	private string _FileSize;

	private IContainer components;

	private Label lblFileName;

	private Label lblFileDate;

	private Label lblFileSize;

	private CheckBox CbImage;

	private Button BtnDelete;

	public PictureBox PictureBox;

	public string FromUSer
	{
		get
		{
			return _FromUSer;
		}
		set
		{
			_FromUSer = value;
		}
	}

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

	public bool IsSelected
	{
		get
		{
			return _IsSelected;
		}
		set
		{
			_IsSelected = value;
		}
	}

	public int DocFileId
	{
		get
		{
			return _DocFileId;
		}
		set
		{
			_DocFileId = value;
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

	public string FileDate
	{
		get
		{
			return _FileDate;
		}
		set
		{
			_FileDate = value;
			lblFileDate.Text = value;
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

	public FolderHierarchyAttachments()
	{
		InitializeComponent();
		PictureBox.Click += ControlClicked;
		CbImage.Click += ControlClicked;
		base.Click += ControlClicked;
		BtnDelete.Click += ControlClicked;
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void AttachmentPreviewControl_Load(object sender, EventArgs e)
	{
		try
		{
			CbImage.Checked = true;
			if (FromUSer == "SubUser")
			{
				BtnDelete.Hide();
				CbImage.Hide();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CbImage_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (CbImage.Checked)
			{
				IsSelected = true;
			}
			else
			{
				IsSelected = false;
			}
			base.Tag = "FormCheckBox";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PictureBox_Click(object sender, EventArgs e)
	{
		try
		{
			base.Tag = "FormPictureBox";
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
		this.lblFileName = new System.Windows.Forms.Label();
		this.lblFileDate = new System.Windows.Forms.Label();
		this.lblFileSize = new System.Windows.Forms.Label();
		this.PictureBox = new System.Windows.Forms.PictureBox();
		this.CbImage = new System.Windows.Forms.CheckBox();
		this.BtnDelete = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.PictureBox).BeginInit();
		base.SuspendLayout();
		this.lblFileName.AutoEllipsis = true;
		this.lblFileName.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFileName.Location = new System.Drawing.Point(3, 2);
		this.lblFileName.Name = "lblFileName";
		this.lblFileName.Size = new System.Drawing.Size(209, 27);
		this.lblFileName.TabIndex = 1;
		this.lblFileName.Text = "Name";
		this.lblFileDate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFileDate.Location = new System.Drawing.Point(120, 41);
		this.lblFileDate.Name = "lblFileDate";
		this.lblFileDate.Size = new System.Drawing.Size(113, 15);
		this.lblFileDate.TabIndex = 2;
		this.lblFileDate.Text = "Date";
		this.lblFileSize.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFileSize.Location = new System.Drawing.Point(120, 65);
		this.lblFileSize.Name = "lblFileSize";
		this.lblFileSize.Size = new System.Drawing.Size(113, 26);
		this.lblFileSize.TabIndex = 3;
		this.lblFileSize.Text = "0";
		this.PictureBox.AccessibleName = "PictureBox";
		this.PictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
		this.PictureBox.Image = Architecture.WinApp.Properties.Resources.Zip_Logo;
		this.PictureBox.Location = new System.Drawing.Point(3, 32);
		this.PictureBox.Name = "PictureBox";
		this.PictureBox.Size = new System.Drawing.Size(111, 80);
		this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.PictureBox.TabIndex = 0;
		this.PictureBox.TabStop = false;
		this.PictureBox.Tag = "PictureBox";
		this.PictureBox.Click += new System.EventHandler(PictureBox_Click);
		this.CbImage.AccessibleName = "CbImage";
		this.CbImage.AutoSize = true;
		this.CbImage.Location = new System.Drawing.Point(218, 3);
		this.CbImage.Name = "CbImage";
		this.CbImage.Size = new System.Drawing.Size(15, 14);
		this.CbImage.TabIndex = 4;
		this.CbImage.Tag = "CheckBox";
		this.CbImage.UseVisualStyleBackColor = true;
		this.CbImage.Visible = false;
		this.CbImage.CheckedChanged += new System.EventHandler(CbImage_CheckedChanged);
		this.BtnDelete.Location = new System.Drawing.Point(150, 90);
		this.BtnDelete.Name = "BtnDelete";
		this.BtnDelete.Size = new System.Drawing.Size(54, 23);
		this.BtnDelete.TabIndex = 5;
		this.BtnDelete.Text = "Delete";
		this.BtnDelete.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.BtnDelete);
		base.Controls.Add(this.CbImage);
		base.Controls.Add(this.lblFileSize);
		base.Controls.Add(this.lblFileDate);
		base.Controls.Add(this.lblFileName);
		base.Controls.Add(this.PictureBox);
		base.Name = "FolderHierarchyAttachments";
		base.Size = new System.Drawing.Size(238, 116);
		base.Load += new System.EventHandler(AttachmentPreviewControl_Load);
		((System.ComponentModel.ISupportInitialize)this.PictureBox).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
