using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.DynamicCards;

public class TopImageNamePercent : UserControl
{
	private string _ImageName;

	private byte[] _memoryStream;

	private string _ItemName;

	private string _Percent;

	private IContainer components;

	private Label ItemName;

	private Label Percent;

	public PictureBox ItemImage;

	public string ImageName
	{
		get
		{
			return _ImageName;
		}
		set
		{
			_ImageName = value;
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

	public string lblItemName
	{
		get
		{
			return _ItemName;
		}
		set
		{
			_ItemName = value;
			ItemName.Text = value;
		}
	}

	public string lblPercent
	{
		get
		{
			return _Percent;
		}
		set
		{
			_Percent = value;
			Percent.Text = value;
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public TopImageNamePercent()
	{
		InitializeComponent();
		ItemImage.Click += ControlClicked;
		base.Click += ControlClicked;
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void TopImageNamePercent_Load(object sender, EventArgs e)
	{
		try
		{
			if (memoryStream != null)
			{
				try
				{
					if (ItemImage.Image != null)
					{
						ItemImage.Image.Dispose();
						ItemImage.Image = null;
					}
					string extension = Path.GetExtension(ImageName);
					if ((extension.ToUpper() == ".JPG" || extension.ToUpper() == ".JPEG" || extension.ToUpper() == ".PNG" || extension.ToUpper() == ".GIF" || extension.ToUpper() == ".JFIF") && memoryStream.Length != 0)
					{
						using (MemoryStream ms = new MemoryStream(memoryStream))
						{
							Image image = Image.FromStream(ms);
							ItemImage.Image = image;
							return;
						}
					}
					return;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Failed to load image: " + ex.Message);
					return;
				}
			}
			FileInfo file = new FileInfo(ImageName);
			if (file.Extension.ToUpper() == ".JPG" || file.Extension.ToUpper() == ".JPEG" || file.Extension.ToUpper() == ".PNG" || file.Extension.ToUpper() == ".GIF" || file.Extension.ToUpper() == ".JFIF")
			{
				ItemImage.Image = Image.FromFile(ImageName);
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
		this.ItemName = new System.Windows.Forms.Label();
		this.Percent = new System.Windows.Forms.Label();
		this.ItemImage = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.ItemImage).BeginInit();
		base.SuspendLayout();
		this.ItemName.AutoEllipsis = true;
		this.ItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ItemName.Location = new System.Drawing.Point(0, 56);
		this.ItemName.Name = "ItemName";
		this.ItemName.Size = new System.Drawing.Size(83, 32);
		this.ItemName.TabIndex = 20;
		this.ItemName.Text = "Husk";
		this.ItemName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.Percent.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Percent.Location = new System.Drawing.Point(3, 90);
		this.Percent.Name = "Percent";
		this.Percent.Size = new System.Drawing.Size(77, 17);
		this.Percent.TabIndex = 21;
		this.Percent.Text = "0";
		this.Percent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.ItemImage.Image = Architecture.WinApp.Properties.Resources.NoItemImageFound;
		this.ItemImage.InitialImage = Architecture.WinApp.Properties.Resources.NoItemImageFound;
		this.ItemImage.Location = new System.Drawing.Point(18, 3);
		this.ItemImage.Name = "ItemImage";
		this.ItemImage.Size = new System.Drawing.Size(50, 50);
		this.ItemImage.TabIndex = 0;
		this.ItemImage.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.Percent);
		base.Controls.Add(this.ItemName);
		base.Controls.Add(this.ItemImage);
		base.Name = "TopImageNamePercent";
		base.Size = new System.Drawing.Size(83, 110);
		base.Load += new System.EventHandler(TopImageNamePercent_Load);
		((System.ComponentModel.ISupportInitialize)this.ItemImage).EndInit();
		base.ResumeLayout(false);
	}
}
