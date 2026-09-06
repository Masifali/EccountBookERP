using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.DynamicCards;

public class AttachmentUploadControl : UserControl
{
	private int _ActionId;

	private byte[] _ByteArray;

	private string _Path;

	private string _FileName;

	private string _FileDate;

	private string _FileSize;

	private IContainer components;

	private Label lblFileSize;

	private Label lblFileDate;

	private Label lblFileName;

	private Button btnDelete;

	public PictureBox PictureBox;

	public int ActionId
	{
		get
		{
			return _ActionId;
		}
		set
		{
			_ActionId = value;
		}
	}

	public byte[] ByteArray
	{
		get
		{
			return _ByteArray;
		}
		set
		{
			_ByteArray = value;
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

	public AttachmentUploadControl()
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

	private void AttachmentUploadControl_Load(object sender, EventArgs e)
	{
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
		this.lblFileSize = new System.Windows.Forms.Label();
		this.lblFileDate = new System.Windows.Forms.Label();
		this.lblFileName = new System.Windows.Forms.Label();
		this.PictureBox = new System.Windows.Forms.PictureBox();
		this.btnDelete = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.PictureBox).BeginInit();
		base.SuspendLayout();
		this.lblFileSize.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFileSize.Location = new System.Drawing.Point(221, 54);
		this.lblFileSize.Name = "lblFileSize";
		this.lblFileSize.Size = new System.Drawing.Size(158, 26);
		this.lblFileSize.TabIndex = 7;
		this.lblFileSize.Text = "0";
		this.lblFileDate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFileDate.Location = new System.Drawing.Point(221, 31);
		this.lblFileDate.Name = "lblFileDate";
		this.lblFileDate.Size = new System.Drawing.Size(120, 23);
		this.lblFileDate.TabIndex = 6;
		this.lblFileDate.Text = "Date";
		this.lblFileName.AutoEllipsis = true;
		this.lblFileName.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFileName.Location = new System.Drawing.Point(221, 9);
		this.lblFileName.Name = "lblFileName";
		this.lblFileName.Size = new System.Drawing.Size(158, 17);
		this.lblFileName.TabIndex = 5;
		this.lblFileName.Text = "Name";
		this.PictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
		this.PictureBox.Image = Architecture.WinApp.Properties.Resources.No_Preview;
		this.PictureBox.Location = new System.Drawing.Point(3, 3);
		this.PictureBox.Name = "PictureBox";
		this.PictureBox.Size = new System.Drawing.Size(212, 110);
		this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.PictureBox.TabIndex = 4;
		this.PictureBox.TabStop = false;
		this.btnDelete.Location = new System.Drawing.Point(270, 85);
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(71, 23);
		this.btnDelete.TabIndex = 8;
		this.btnDelete.Text = "Delete";
		this.btnDelete.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.btnDelete);
		base.Controls.Add(this.lblFileSize);
		base.Controls.Add(this.lblFileDate);
		base.Controls.Add(this.lblFileName);
		base.Controls.Add(this.PictureBox);
		base.Name = "AttachmentUploadControl";
		base.Size = new System.Drawing.Size(385, 118);
		base.Load += new System.EventHandler(AttachmentUploadControl_Load);
		((System.ComponentModel.ISupportInitialize)this.PictureBox).EndInit();
		base.ResumeLayout(false);
	}
}
