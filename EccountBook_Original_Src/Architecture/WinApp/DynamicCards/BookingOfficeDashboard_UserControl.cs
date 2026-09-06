using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.DynamicCards;

public class BookingOfficeDashboard_UserControl : UserControl
{
	private IContainer components;

	private Label MainTitle;

	private Label Description;

	private Label label3;

	private Label label4;

	private Button button1;

	private PictureBox pictureBox1;

	private Label label5;

	public BookingOfficeDashboard_UserControl()
	{
		InitializeComponent();
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
		this.MainTitle = new System.Windows.Forms.Label();
		this.Description = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.MainTitle.AutoSize = true;
		this.MainTitle.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.MainTitle.Location = new System.Drawing.Point(15, 15);
		this.MainTitle.Name = "MainTitle";
		this.MainTitle.Size = new System.Drawing.Size(36, 17);
		this.MainTitle.TabIndex = 0;
		this.MainTitle.Text = "Title";
		this.Description.AutoSize = true;
		this.Description.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Description.Location = new System.Drawing.Point(15, 38);
		this.Description.Name = "Description";
		this.Description.Size = new System.Drawing.Size(68, 15);
		this.Description.TabIndex = 1;
		this.Description.Text = "Description";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(28, 76);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(56, 15);
		this.label3.TabIndex = 2;
		this.label3.Text = "SubTitle1";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(28, 104);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(58, 15);
		this.label4.TabIndex = 3;
		this.label4.Text = "SubTitle2";
		this.button1.FlatAppearance.BorderSize = 0;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.FromArgb(187, 181, 255);
		this.button1.Location = new System.Drawing.Point(306, 112);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(72, 23);
		this.button1.TabIndex = 4;
		this.button1.Text = "Detail  >";
		this.button1.UseVisualStyleBackColor = true;
		this.label5.BackColor = System.Drawing.Color.Gainsboro;
		this.label5.Location = new System.Drawing.Point(15, 73);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(6, 50);
		this.label5.TabIndex = 16;
		this.pictureBox1.Location = new System.Drawing.Point(318, 13);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(40, 40);
		this.pictureBox1.TabIndex = 5;
		this.pictureBox1.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.label5);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.Description);
		base.Controls.Add(this.MainTitle);
		base.Name = "BookingOfficeDashboard_UserControl";
		base.Size = new System.Drawing.Size(403, 143);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
