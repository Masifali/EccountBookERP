using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.Lab;

public class LabImagesPreview : Form
{
	public Image ImageToShow;

	private IContainer components;

	public PictureBox pictureBox1;

	public LabImagesPreview()
	{
		InitializeComponent();
	}

	private void LabImagesPreview_Load(object sender, EventArgs e)
	{
		if (InvLabPurchaseAnalysis.imgpreview != null)
		{
			pictureBox1.Image = InvLabPurchaseAnalysis.imgpreview;
		}
		InvLabPurchaseAnalysis.imgpreview = null;
		if (InvLabAnalysisInProcess.imgpreview != null)
		{
			pictureBox1.Image = InvLabAnalysisInProcess.imgpreview;
		}
		InvLabAnalysisInProcess.imgpreview = null;
		if (InvLabSaleAnalysis.imgpreview != null)
		{
			pictureBox1.Image = InvLabSaleAnalysis.imgpreview;
		}
		InvLabSaleAnalysis.imgpreview = null;
		if (ImageToShow != null)
		{
			pictureBox1.Image = ImageToShow;
		}
	}

	private void LabImagesPreview_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			pictureBox1.Image = null;
			pictureBox1.Invalidate();
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
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pictureBox1.Location = new System.Drawing.Point(0, 0);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(795, 540);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(795, 540);
		base.Controls.Add(this.pictureBox1);
		base.Name = "LabImagesPreview";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(LabImagesPreview_FormClosing);
		base.Load += new System.EventHandler(LabImagesPreview_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}
}
