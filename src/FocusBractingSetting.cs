using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EDSDKLib;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace CameraControl
{

public class FocusBractingSetting : Form
{
	private CameraController _controller;

	private IntPtr _camera = (IntPtr)0;

	private const uint _Version = 1u;

	private uint _FocusShiftFunction;

	private uint _FocusShiftFunctiongPrev;

	private uint _ShootingNumber;

	private uint _ShootingNumberPrev;

	private uint _StepWidth;

	private uint _StepWidthPrev;

	private uint _ExposureSmoothing;

	private uint _ExposureSmoothingPrev;

	protected Dictionary<uint, string> map1 = new Dictionary<uint, string>();

	protected Dictionary<uint, string> map2 = new Dictionary<uint, string>();

	protected int[] ary = Enumerable.Range(1, 10).ToArray();

	protected EDSDK.FocusShiftSetting _fssData;

	private IContainer components;

	private ComboBox focusBracketingComboBox;

	private Label label1;

	private ComboBox focusIncrementComboBox;

	private Label label2;

	private ComboBox exposureSmoothingComboBox;

	private Label label3;

	private Label label4;

	private Button button2;

	private Button button1;

	private TextBox textBoxShtNum;

	public FocusBractingSetting(ref CameraController controller)
	{
		_controller = controller;
		_camera = _controller.GetModel().Camera;
		InitializeComponent();
		focusBracketingComboBox.Items.Clear();
		focusBracketingComboBox.Items.Add("Disable");
		focusBracketingComboBox.Items.Add("Enable");
		exposureSmoothingComboBox.Items.Add("Disable");
		exposureSmoothingComboBox.Items.Add("Enable");
		focusIncrementComboBox.Items.Clear();
		for (int i = 0; i < ary.Length; i++)
		{
			focusIncrementComboBox.Items.Add(ary[i]);
		}
		if (EDSDK.EdsGetPropertyData(_camera, 16778327u, 0, out _fssData) == 0)
		{
			_FocusShiftFunctiongPrev = _fssData.FocusShiftFunction;
			_ShootingNumberPrev = _fssData.ShootingNumber;
			_StepWidthPrev = _fssData.StepWidth;
			_ExposureSmoothingPrev = _fssData.ExposureSmoothing;
		}
		focusBracketingComboBox.SelectedIndex = (int)_FocusShiftFunctiongPrev;
		exposureSmoothingComboBox.SelectedIndex = (int)_ExposureSmoothingPrev;
		focusIncrementComboBox.SelectedIndex = (int)(_StepWidthPrev - 1);
		textBoxShtNum.Text = _ShootingNumberPrev.ToString();
	}

	private void FocusBractingSetting_Load(object sender, EventArgs e)
	{
		_fssData.Version = 1u;
		_fssData.FocusShiftFunction = 0u;
		_fssData.ShootingNumber = 0u;
		_fssData.StepWidth = 0u;
		_fssData.ExposureSmoothing = 0u;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		string strShotNum = "";
		strShotNum = textBoxShtNum.Text;
		try
		{
			int _shootingNumber = int.Parse(strShotNum);
			if (isFrom2to999(_shootingNumber))
			{
				_FocusShiftFunction = (uint)focusBracketingComboBox.SelectedIndex;
				_ShootingNumber = (uint)_shootingNumber;
				_StepWidth = (uint)(focusIncrementComboBox.SelectedIndex + 1);
				_ExposureSmoothing = (uint)exposureSmoothingComboBox.SelectedIndex;
				_fssData.FocusShiftFunction = _FocusShiftFunction;
				_fssData.ShootingNumber = _ShootingNumber;
				_fssData.StepWidth = _StepWidth;
				_fssData.ExposureSmoothing = _ExposureSmoothing;
				int dataSize = Marshal.SizeOf(typeof(EDSDK.FocusShiftSetting));
				EDSDK.EdsSetPropertyData(_camera, 16778327u, 0, dataSize, _fssData);
			}
			else
			{
				MessageBox.Show("Please enter a number from 2 to 999 in the Number of shots field.");
				base.DialogResult = DialogResult.None;
			}
		}
		catch (FormatException)
		{
			MessageBox.Show("Please enter a number from 2 to 999 in the Number of shots field.");
			base.DialogResult = DialogResult.None;
		}
	}

	public bool isFrom2to999(int num)
	{
		if (num >= 2 && num <= 999)
		{
			return true;
		}
		return false;
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
		this.focusBracketingComboBox = new System.Windows.Forms.ComboBox();
		this.label1 = new System.Windows.Forms.Label();
		this.focusIncrementComboBox = new System.Windows.Forms.ComboBox();
		this.label2 = new System.Windows.Forms.Label();
		this.exposureSmoothingComboBox = new System.Windows.Forms.ComboBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.button2 = new System.Windows.Forms.Button();
		this.button1 = new System.Windows.Forms.Button();
		this.textBoxShtNum = new System.Windows.Forms.TextBox();
		base.SuspendLayout();
		this.focusBracketingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.focusBracketingComboBox.FormattingEnabled = true;
		this.focusBracketingComboBox.Location = new System.Drawing.Point(23, 43);
		this.focusBracketingComboBox.Name = "focusBracketingComboBox";
		this.focusBracketingComboBox.Size = new System.Drawing.Size(163, 20);
		this.focusBracketingComboBox.TabIndex = 5;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(12, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(95, 12);
		this.label1.TabIndex = 4;
		this.label1.Text = "Focus Bracketing";
		this.focusIncrementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.focusIncrementComboBox.FormattingEnabled = true;
		this.focusIncrementComboBox.Location = new System.Drawing.Point(23, 149);
		this.focusIncrementComboBox.Name = "focusIncrementComboBox";
		this.focusIncrementComboBox.Size = new System.Drawing.Size(163, 20);
		this.focusIncrementComboBox.TabIndex = 7;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(12, 134);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(90, 12);
		this.label2.TabIndex = 6;
		this.label2.Text = "Focus increment";
		this.exposureSmoothingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.exposureSmoothingComboBox.FormattingEnabled = true;
		this.exposureSmoothingComboBox.Location = new System.Drawing.Point(23, 195);
		this.exposureSmoothingComboBox.Name = "exposureSmoothingComboBox";
		this.exposureSmoothingComboBox.Size = new System.Drawing.Size(163, 20);
		this.exposureSmoothingComboBox.TabIndex = 9;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(12, 180);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(108, 12);
		this.label3.TabIndex = 8;
		this.label3.Text = "Exposure smoothing";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(12, 79);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(90, 12);
		this.label4.TabIndex = 10;
		this.label4.Text = "Number of shots";
		this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.button2.Location = new System.Drawing.Point(134, 241);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(109, 29);
		this.button2.TabIndex = 12;
		this.button2.Text = "Cancel";
		this.button2.UseVisualStyleBackColor = true;
		this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.button1.Location = new System.Drawing.Point(14, 241);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(109, 29);
		this.button1.TabIndex = 13;
		this.button1.Text = "OK";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.textBoxShtNum.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.textBoxShtNum.Location = new System.Drawing.Point(86, 103);
		this.textBoxShtNum.Name = "textBoxShtNum";
		this.textBoxShtNum.Size = new System.Drawing.Size(100, 19);
		this.textBoxShtNum.TabIndex = 14;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(255, 282);
		base.Controls.Add(this.textBoxShtNum);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.exposureSmoothingComboBox);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.focusIncrementComboBox);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.focusBracketingComboBox);
		base.Controls.Add(this.label1);
		base.Name = "FocusBractingSetting";
		this.Text = "Camera Control";
		base.Load += new System.EventHandler(FocusBractingSetting_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
