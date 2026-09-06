using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EDSDKLib;

namespace CameraControl;

public class RecordFuncCardSetting : Form
{
	private CameraController _controller;

	private IntPtr _camera = (IntPtr)0;

	private EDSDK.EdsPropertyDesc _desc1;

	private EDSDK.EdsPropertyDesc _desc2;

	private EDSDK.EdsPropertyDesc _desc3;

	private EDSDK.EdsPropertyDesc _desc4;

	private EDSDK.EdsPropertyDesc _desc5;

	private uint _StillMovieDivideSetting;

	private uint _StillMovieDivideSettingPrev;

	private uint _CardExtension;

	private uint _CardExtensionPrev;

	private uint _MovieCardExtension;

	private uint _MovieCardExtensionPrev;

	private uint _StillCurrentMedia;

	private uint _StillCurrentMediaPrev;

	private uint _MovieCurrentMedia;

	private uint _MovieCurrentMediaPrev;

	protected Dictionary<uint, string> map1 = new Dictionary<uint, string>();

	protected Dictionary<uint, string> map2 = new Dictionary<uint, string>();

	protected Dictionary<uint, string> map3 = new Dictionary<uint, string>();

	protected Dictionary<uint, string> map4 = new Dictionary<uint, string>();

	private IContainer components;

	private Label label1;

	private GroupBox groupBox1;

	private Label label3;

	private Label label2;

	private GroupBox groupBox2;

	private Label label5;

	private Label label4;

	private ComboBox comboBox3;

	private ComboBox comboBox2;

	private ComboBox comboBox5;

	private ComboBox comboBox4;

	private ComboBox comboBox1;

	public RecordFuncCardSetting(ref CameraController controller)
	{
		map1.Add(0u, "Disable");
		map1.Add(1u, "Enable");
		map1.Add(uint.MaxValue, "unknown");
		map2.Add(0u, "Standard");
		map2.Add(1u, "Rec. to multiple");
		map2.Add(2u, "Rec. separately");
		map2.Add(3u, "Auto switch card");
		map2.Add(uint.MaxValue, "unknown");
		map3.Add(0u, "Enable");
		map3.Add(1u, "slot1");
		map3.Add(2u, "slot2");
		map3.Add(uint.MaxValue, "unknown");
		map4.Add(0u, "Standard");
		map4.Add(1u, "Rec. to multiple");
		map4.Add(2u, "slot1=RAW and slot2=MP4");
		map4.Add(3u, "Auto switch card");
		map4.Add(uint.MaxValue, "unknown");
		_controller = controller;
		_camera = _controller.GetModel().Camera;
		InitializeComponent();
		_desc1 = default(EDSDK.EdsPropertyDesc);
		EDSDK.EdsGetPropertyData(_camera, 16778352u, 0, out _StillMovieDivideSetting);
		EDSDK.EdsGetPropertyDesc(_camera, 16778352u, out _desc1);
		comboBox1.Items.Clear();
		for (int m = 0; m < _desc1.NumElements; m++)
		{
			if (map1.TryGetValue((uint)_desc1.PropDesc[m], out var outString5) && !outString5.Equals("unknown"))
			{
				comboBox1.Items.Add(outString5);
				if (_StillMovieDivideSetting == _desc1.PropDesc[m])
				{
					comboBox1.SelectedIndex = m;
					_StillMovieDivideSetting = (uint)_desc1.PropDesc[m];
					_StillMovieDivideSettingPrev = _StillMovieDivideSetting;
				}
			}
		}
		_desc2 = default(EDSDK.EdsPropertyDesc);
		EDSDK.EdsGetPropertyData(_camera, 16778353u, 0, out _CardExtension);
		EDSDK.EdsGetPropertyDesc(_camera, 16778353u, out _desc2);
		comboBox2.Items.Clear();
		for (int l = 0; l < _desc2.NumElements; l++)
		{
			if (map2.TryGetValue((uint)_desc2.PropDesc[l], out var outString4) && !outString4.Equals("unknown"))
			{
				comboBox2.Items.Add(outString4);
				if (_CardExtension == _desc2.PropDesc[l])
				{
					comboBox2.SelectedIndex = l;
					_CardExtension = (uint)_desc2.PropDesc[l];
					_CardExtensionPrev = _CardExtension;
				}
			}
		}
		_desc3 = default(EDSDK.EdsPropertyDesc);
		EDSDK.EdsGetPropertyData(_camera, 16778355u, 0, out _StillCurrentMedia);
		EDSDK.EdsGetPropertyDesc(_camera, 16778355u, out _desc3);
		comboBox3.Items.Clear();
		for (int k = 0; k < _desc3.NumElements; k++)
		{
			if (map3.TryGetValue((uint)_desc3.PropDesc[k], out var outString3) && !outString3.Equals("unknown"))
			{
				comboBox3.Items.Add(outString3);
				if (_StillCurrentMedia == _desc3.PropDesc[k])
				{
					comboBox3.SelectedIndex = k;
					_StillCurrentMedia = (uint)_desc3.PropDesc[k];
					_StillCurrentMediaPrev = _StillCurrentMedia;
				}
			}
		}
		_desc4 = default(EDSDK.EdsPropertyDesc);
		EDSDK.EdsGetPropertyData(_camera, 16778354u, 0, out _MovieCardExtension);
		EDSDK.EdsGetPropertyDesc(_camera, 16778354u, out _desc4);
		comboBox4.Items.Clear();
		for (int j = 0; j < _desc4.NumElements; j++)
		{
			if (map4.TryGetValue((uint)_desc4.PropDesc[j], out var outString2) && !outString2.Equals("unknown"))
			{
				comboBox4.Items.Add(outString2);
				if (_MovieCardExtension == _desc4.PropDesc[j])
				{
					comboBox4.SelectedIndex = j;
					_MovieCardExtension = (uint)_desc4.PropDesc[j];
					_MovieCardExtensionPrev = _MovieCardExtension;
				}
			}
		}
		_desc5 = default(EDSDK.EdsPropertyDesc);
		EDSDK.EdsGetPropertyData(_camera, 16778356u, 0, out _MovieCurrentMedia);
		EDSDK.EdsGetPropertyDesc(_camera, 16778356u, out _desc5);
		comboBox5.Items.Clear();
		for (int i = 0; i < _desc5.NumElements; i++)
		{
			if (map3.TryGetValue((uint)_desc5.PropDesc[i], out var outString) && !outString.Equals("unknown"))
			{
				comboBox5.Items.Add(outString);
				if (_MovieCurrentMedia == _desc5.PropDesc[i])
				{
					comboBox5.SelectedIndex = i;
					_MovieCurrentMedia = (uint)_desc5.PropDesc[i];
					_MovieCurrentMediaPrev = _MovieCurrentMedia;
				}
			}
		}
	}

	private void RecordFuncCardSetting_Load(object sender, EventArgs e)
	{
	}

	private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
	{
		uint key = (uint)_desc1.PropDesc[comboBox1.SelectedIndex];
		EDSDK.EdsSetPropertyData(_camera, 16778352u, 0, 4, key);
		new RecordFuncCardSetting(ref _controller);
	}

	private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
	{
		uint key = (uint)_desc2.PropDesc[comboBox2.SelectedIndex];
		EDSDK.EdsSetPropertyData(_controller.GetModel().Camera, 16778353u, 0, 4, key);
	}

	private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
	{
		uint key = (uint)_desc3.PropDesc[comboBox3.SelectedIndex];
		EDSDK.EdsSetPropertyData(_controller.GetModel().Camera, 16778355u, 0, 4, key);
	}

	private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
	{
		uint key = (uint)_desc4.PropDesc[comboBox4.SelectedIndex];
		EDSDK.EdsSetPropertyData(_controller.GetModel().Camera, 16778354u, 0, 4, key);
	}

	private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
	{
		uint key = (uint)_desc5.PropDesc[comboBox5.SelectedIndex];
		EDSDK.EdsSetPropertyData(_controller.GetModel().Camera, 16778356u, 0, 4, key);
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
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.comboBox3 = new System.Windows.Forms.ComboBox();
		this.comboBox2 = new System.Windows.Forms.ComboBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.comboBox5 = new System.Windows.Forms.ComboBox();
		this.comboBox4 = new System.Windows.Forms.ComboBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.comboBox1 = new System.Windows.Forms.ComboBox();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(22, 25);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(156, 12);
		this.label1.TabIndex = 0;
		this.label1.Text = "[Still image/ Movie] Separate";
		this.groupBox1.Controls.Add(this.comboBox3);
		this.groupBox1.Controls.Add(this.comboBox2);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Location = new System.Drawing.Point(12, 94);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(228, 128);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Still image";
		this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox3.FormattingEnabled = true;
		this.comboBox3.Location = new System.Drawing.Point(21, 89);
		this.comboBox3.Name = "comboBox3";
		this.comboBox3.Size = new System.Drawing.Size(163, 20);
		this.comboBox3.TabIndex = 3;
		this.comboBox3.SelectedIndexChanged += new System.EventHandler(comboBox3_SelectedIndexChanged);
		this.comboBox3.SelectionChangeCommitted += new System.EventHandler(comboBox3_SelectionChangeCommitted);
		this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox2.FormattingEnabled = true;
		this.comboBox2.Location = new System.Drawing.Point(21, 39);
		this.comboBox2.Name = "comboBox2";
		this.comboBox2.Size = new System.Drawing.Size(163, 20);
		this.comboBox2.TabIndex = 2;
		this.comboBox2.SelectedIndexChanged += new System.EventHandler(comboBox2_SelectedIndexChanged);
		this.comboBox2.SelectionChangeCommitted += new System.EventHandler(comboBox2_SelectionChangeCommitted);
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(19, 24);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(66, 12);
		this.label3.TabIndex = 1;
		this.label3.Text = "Rec options";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(19, 74);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(68, 12);
		this.label2.TabIndex = 0;
		this.label2.Text = "Record/play";
		this.groupBox2.Controls.Add(this.comboBox5);
		this.groupBox2.Controls.Add(this.comboBox4);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Location = new System.Drawing.Point(265, 94);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(228, 128);
		this.groupBox2.TabIndex = 2;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Movie";
		this.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox5.FormattingEnabled = true;
		this.comboBox5.Location = new System.Drawing.Point(18, 87);
		this.comboBox5.Name = "comboBox5";
		this.comboBox5.Size = new System.Drawing.Size(163, 20);
		this.comboBox5.TabIndex = 5;
		this.comboBox5.SelectedIndexChanged += new System.EventHandler(comboBox5_SelectedIndexChanged);
		this.comboBox5.SelectionChangeCommitted += new System.EventHandler(comboBox5_SelectionChangeCommitted);
		this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox4.DropDownWidth = 163;
		this.comboBox4.FormattingEnabled = true;
		this.comboBox4.Location = new System.Drawing.Point(18, 41);
		this.comboBox4.Name = "comboBox4";
		this.comboBox4.Size = new System.Drawing.Size(163, 20);
		this.comboBox4.TabIndex = 4;
		this.comboBox4.SelectedIndexChanged += new System.EventHandler(comboBox4_SelectedIndexChanged);
		this.comboBox4.SelectionChangeCommitted += new System.EventHandler(comboBox4_SelectionChangeCommitted);
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(16, 26);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(66, 12);
		this.label5.TabIndex = 2;
		this.label5.Text = "Rec options";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(16, 72);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(68, 12);
		this.label4.TabIndex = 2;
		this.label4.Text = "Record/play";
		this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox1.FormattingEnabled = true;
		this.comboBox1.Location = new System.Drawing.Point(33, 49);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Size = new System.Drawing.Size(163, 20);
		this.comboBox1.TabIndex = 3;
		this.comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
		this.comboBox1.SelectionChangeCommitted += new System.EventHandler(comboBox1_SelectionChangeCommitted);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(505, 234);
		base.Controls.Add(this.comboBox1);
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.label1);
		base.Name = "RecordFuncCardSetting";
		this.Text = "Camera Control";
		base.Load += new System.EventHandler(RecordFuncCardSetting_Load);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
