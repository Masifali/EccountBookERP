using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
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
namespace Architecture.WinApp.DynamicCards
{

public class AttachmentPreviewControl : UserControl
{
	private string _Path;

	private string _FileName;

	private string _FileDate;

	private string _FileSize;

	private byte[] _memoryStream;

	private int _AttachmentId;

	private IContainer components;

	private Label lblFileName;

	private Label lblFileDate;

	private Label lblFileSize;

	public Button btnDelete;

	public PictureBox PictureBox;

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

	public byte[] memoryStream
	{
		get
		{
			return _memoryStream;
		}
		set
		{
			_memoryStream = value;
		}
	}

	public int AttachmentId
	{
		get
		{
			return _AttachmentId;
		}
		set
		{
			_AttachmentId = value;
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public AttachmentPreviewControl()
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

	private void AttachmentPreviewControl_Load(object sender, EventArgs e)
	{
		try
		{
			if (memoryStream != null)
			{
				try
				{
					if (PictureBox.Image != null)
					{
						PictureBox.Image.Dispose();
						PictureBox.Image = null;
					}
					string extension = Path.GetExtension(FileName);
					if (extension.ToUpper() == ".JPG" || extension.ToUpper() == ".JPEG" || extension.ToUpper() == ".PNG" || extension.ToUpper() == ".GIF" || extension.ToUpper() == ".JFIF")
					{
						if (memoryStream.Length != 0)
						{
							using (MemoryStream ms = new MemoryStream(memoryStream))
							{
								Image image = Image.FromStream(ms);
								Bitmap bmp = new Bitmap(image);
								bmp.Save(ms, image.RawFormat);
								PictureBox.Image = bmp;
								image.Dispose();
								ms.Position = 0L;
								ms.Dispose();
								return;
							}
						}
					}
					else if (extension.ToUpper() == ".XLSX" || extension.ToUpper() == ".XLSM" || extension.ToUpper() == ".XLS" || extension.ToUpper() == ".XLSB" || extension.ToUpper() == ".XLTX")
					{
						PictureBox.Image = Resources.Microsoft_Excel_Icon;
					}
					else if (extension.ToUpper() == ".DOCX" || extension.ToUpper() == ".DOC")
					{
						PictureBox.Image = Resources.Microsoft_Word_Logo;
					}
					else if (extension.ToUpper() == ".PDF")
					{
						PictureBox.Image = Resources.Pdf_Logo;
					}
					else if (extension.ToUpper() == ".TXT")
					{
						PictureBox.Image = Resources.txtFile_Logo;
					}
					else if (extension.ToUpper() == ".ZIP" || extension.ToUpper() == ".RAR")
					{
						PictureBox.Image = Resources.Zip_Logo;
					}
					else
					{
						PictureBox.Image = Resources.No_Preview;
					}
					return;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Failed to load image: " + ex.Message);
					return;
				}
			}
			FileInfo file = new FileInfo(path);
			if (file.Extension.ToUpper() == ".JPG" || file.Extension.ToUpper() == ".JPEG" || file.Extension.ToUpper() == ".PNG" || file.Extension.ToUpper() == ".GIF" || file.Extension.ToUpper() == ".JFIF")
			{
				using (MemoryStream stream = new MemoryStream())
				{
					Image image2 = Image.FromFile(path);
					Bitmap bmp2 = new Bitmap(image2);
					bmp2.Save(stream, image2.RawFormat);
					PictureBox.Image = bmp2;
					image2.Dispose();
					stream.Position = 0L;
					stream.Dispose();
					return;
				}
			}
			if (file.Extension.ToUpper() == ".XLSX" || file.Extension.ToUpper() == ".XLSM" || file.Extension.ToUpper() == ".XLS" || file.Extension.ToUpper() == ".XLSB" || file.Extension.ToUpper() == ".XLTX")
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
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
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
		this.btnDelete = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.PictureBox).BeginInit();
		base.SuspendLayout();
		this.lblFileName.AutoEllipsis = true;
		this.lblFileName.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFileName.Location = new System.Drawing.Point(215, 12);
		this.lblFileName.Name = "lblFileName";
		this.lblFileName.Size = new System.Drawing.Size(158, 18);
		this.lblFileName.TabIndex = 1;
		this.lblFileName.Text = "Name";
		this.lblFileDate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFileDate.Location = new System.Drawing.Point(217, 35);
		this.lblFileDate.Name = "lblFileDate";
		this.lblFileDate.Size = new System.Drawing.Size(149, 15);
		this.lblFileDate.TabIndex = 2;
		this.lblFileDate.Text = "Date";
		this.lblFileSize.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFileSize.Location = new System.Drawing.Point(220, 59);
		this.lblFileSize.Name = "lblFileSize";
		this.lblFileSize.Size = new System.Drawing.Size(141, 28);
		this.lblFileSize.TabIndex = 3;
		this.lblFileSize.Text = "0";
		this.PictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
		this.PictureBox.Image = Architecture.WinApp.Properties.Resources.Zip_Logo;
		this.PictureBox.Location = new System.Drawing.Point(3, 2);
		this.PictureBox.Name = "PictureBox";
		this.PictureBox.Size = new System.Drawing.Size(211, 110);
		this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.PictureBox.TabIndex = 0;
		this.PictureBox.TabStop = false;
		this.btnDelete.Location = new System.Drawing.Point(258, 89);
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(71, 23);
		this.btnDelete.TabIndex = 9;
		this.btnDelete.Text = "Delete";
		this.btnDelete.UseVisualStyleBackColor = true;
		this.btnDelete.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.btnDelete);
		base.Controls.Add(this.lblFileSize);
		base.Controls.Add(this.lblFileDate);
		base.Controls.Add(this.lblFileName);
		base.Controls.Add(this.PictureBox);
		base.Name = "AttachmentPreviewControl";
		base.Size = new System.Drawing.Size(376, 116);
		base.Load += new System.EventHandler(AttachmentPreviewControl_Load);
		((System.ComponentModel.ISupportInitialize)this.PictureBox).EndInit();
		base.ResumeLayout(false);
	}
}

}
