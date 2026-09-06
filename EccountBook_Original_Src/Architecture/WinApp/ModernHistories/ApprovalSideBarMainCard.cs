using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.ModernHistories;

public class ApprovalSideBarMainCard : UserControl
{
	private bool _IsSelected;

	private IContainer components;

	public CheckBox chkSelectionWithMainText;

	private Label labelLine1;

	public TableLayoutPanel FlpMain;

	private Panel pnlActive;

	public int DocumentId { get; set; }

	public bool IsSelected
	{
		get
		{
			return _IsSelected;
		}
		set
		{
			_IsSelected = value;
			BackColor = (value ? Color.FromArgb(230, 240, 255) : Color.White);
			pnlActive.BackColor = (value ? Color.RoyalBlue : Color.Transparent);
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public ApprovalSideBarMainCard()
	{
		InitializeComponent();
		base.Click += ControlClicked;
		chkSelectionWithMainText.Click += ControlClicked;
		FlpMain.Click += ControlClicked;
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void CbApprove_CheckedChanged(object sender, EventArgs e)
	{
		IsSelected = chkSelectionWithMainText.Checked;
	}

	private void ApprovalSideBarMainCard_Load(object sender, EventArgs e)
	{
	}

	private void ApprovalSideBarMainCard_Click(object sender, EventArgs e)
	{
		try
		{
			base.Tag = "UserControlClick";
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
		this.chkSelectionWithMainText = new System.Windows.Forms.CheckBox();
		this.labelLine1 = new System.Windows.Forms.Label();
		this.FlpMain = new System.Windows.Forms.TableLayoutPanel();
		this.pnlActive = new System.Windows.Forms.Panel();
		base.SuspendLayout();
		this.chkSelectionWithMainText.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
		this.chkSelectionWithMainText.Dock = System.Windows.Forms.DockStyle.Top;
		this.chkSelectionWithMainText.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold);
		this.chkSelectionWithMainText.Location = new System.Drawing.Point(5, 1);
		this.chkSelectionWithMainText.Name = "chkSelectionWithMainText";
		this.chkSelectionWithMainText.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
		this.chkSelectionWithMainText.Size = new System.Drawing.Size(315, 33);
		this.chkSelectionWithMainText.TabIndex = 31;
		this.chkSelectionWithMainText.Text = "Al Khan Rice Mill Narowal Rana Safarash 12121 121212 1212";
		this.chkSelectionWithMainText.TextAlign = System.Drawing.ContentAlignment.TopLeft;
		this.chkSelectionWithMainText.UseVisualStyleBackColor = true;
		this.chkSelectionWithMainText.CheckedChanged += new System.EventHandler(CbApprove_CheckedChanged);
		this.labelLine1.BackColor = System.Drawing.Color.Blue;
		this.labelLine1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.labelLine1.Dock = System.Windows.Forms.DockStyle.Top;
		this.labelLine1.ForeColor = System.Drawing.Color.Blue;
		this.labelLine1.Location = new System.Drawing.Point(0, 0);
		this.labelLine1.Name = "labelLine1";
		this.labelLine1.Size = new System.Drawing.Size(320, 1);
		this.labelLine1.TabIndex = 32;
		this.labelLine1.Text = " ";
		this.FlpMain.ColumnCount = 2;
		this.FlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273f));
		this.FlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.72727f));
		this.FlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.FlpMain.Location = new System.Drawing.Point(5, 34);
		this.FlpMain.Name = "FlpMain";
		this.FlpMain.RowCount = 1;
		this.FlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.FlpMain.Size = new System.Drawing.Size(315, 96);
		this.FlpMain.TabIndex = 34;
		this.pnlActive.Dock = System.Windows.Forms.DockStyle.Left;
		this.pnlActive.Location = new System.Drawing.Point(0, 1);
		this.pnlActive.Name = "pnlActive";
		this.pnlActive.Size = new System.Drawing.Size(5, 129);
		this.pnlActive.TabIndex = 35;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.FlpMain);
		base.Controls.Add(this.chkSelectionWithMainText);
		base.Controls.Add(this.pnlActive);
		base.Controls.Add(this.labelLine1);
		this.Cursor = System.Windows.Forms.Cursors.Hand;
		base.Name = "ApprovalSideBarMainCard";
		base.Size = new System.Drawing.Size(320, 130);
		base.Load += new System.EventHandler(ApprovalSideBarMainCard_Load);
		base.Click += new System.EventHandler(ApprovalSideBarMainCard_Click);
		base.ResumeLayout(false);
	}
}
