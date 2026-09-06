using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
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

public class CameraSetting : Form, IObserver
{
	private delegate void _Update(CameraEvent e);

	private CameraController _controller;

	protected Dictionary<uint, string> map = new Dictionary<uint, string>();

	private EDSDK.EdsPropertyDesc _desc;

	private IntPtr[] _volumes = new IntPtr[2];

	private int _volume_count;

	private IContainer components;

	private ListView listView1;

	private TextBox textBox3;

	private Button button1;

	private Label label7;

	private Button button2;

	private ComboBox comboBox1;

	private Label label1;

	private Button button3;

	private GroupBox groupBox1;

	private Button button4;

	private Label label2;

	private ComboBox autoPowerOff1;

	private Button button5;

	private GroupBox groupBox2;

	private Button sendCommanBbutton;

	private ComboBox sensorCleaningComboBox;

	private Label label3;

	private Label label4;

	private Label label5;

	public CameraSetting(ref CameraController controller, ref IntPtr[] volumes, ref int volume_count)
	{
		_controller = controller;
		_volumes = volumes;
		_volume_count = volume_count;
		InitializeComponent();
		comboBox1.DrawMode = DrawMode.Normal;
		comboBox1.Items.AddRange(new object[3] { "Change Owner Name", "Change Artist", "Change Copyright" });
		comboBox1.SelectedIndex = 0;
		autoPowerOff1.Items.Clear();
		_desc = _controller.GetModel().AutoPowerOffDesc;
		int j = 0;
		while (_desc.NumElements == 0 && j < 5)
		{
			Thread.Sleep(1000);
			_desc = _controller.GetModel().AutoPowerOffDesc;
			j++;
		}
		for (int i = 0; i < _desc.NumElements; i++)
		{
			uint key = (uint)_desc.PropDesc[i];
			string value;
			switch (key)
			{
			case 0u:
				value = "Off";
				break;
			case uint.MaxValue:
				value = "Shutdown";
				break;
			case 1u:
			case 2u:
			case 3u:
			case 4u:
			case 5u:
			case 6u:
			case 7u:
			case 8u:
			case 9u:
			case 10u:
			case 11u:
			case 12u:
			case 13u:
			case 14u:
			case 15u:
			case 16u:
			case 17u:
			case 18u:
			case 19u:
			case 20u:
			case 21u:
			case 22u:
			case 23u:
			case 24u:
			case 25u:
			case 26u:
			case 27u:
			case 28u:
			case 29u:
			case 30u:
			case 31u:
			case 32u:
			case 33u:
			case 34u:
			case 35u:
			case 36u:
			case 37u:
			case 38u:
			case 39u:
			case 40u:
			case 41u:
			case 42u:
			case 43u:
			case 44u:
			case 45u:
			case 46u:
			case 47u:
			case 48u:
			case 49u:
			case 50u:
			case 51u:
			case 52u:
			case 53u:
			case 54u:
			case 55u:
			case 56u:
			case 57u:
			case 58u:
			case 59u:
				value = Convert.ToString(key, 10) + "\"";
				break;
			default:
				value = Convert.ToString(key / 60, 10) + "'";
				break;
			}
			map.Add(key, value);
			autoPowerOff1.Items.Add(value);
			if (_controller.GetModel().AutoPowerOff == _desc.PropDesc[i])
			{
				autoPowerOff1.SelectedIndex = i;
			}
		}
		if (map.Count == 0)
		{
			autoPowerOff1.Enabled = false;
		}
		if (_volume_count > 0)
		{
			EDSDK.EdsGetChildAtIndex(_controller.GetModel().Camera, 0, out _volumes[0]);
			EDSDK.EdsGetVolumeInfo(_volumes[0], out var outVolumeInfo);
			if (outVolumeInfo.StorageType != 0)
			{
				button1.Enabled = true;
				button1.Text = "Memory Card 1 (" + outVolumeInfo.szVolumeLabel + ")";
			}
			if (_volume_count > 1)
			{
				EDSDK.EdsGetChildAtIndex(_controller.GetModel().Camera, 1, out _volumes[1]);
				EDSDK.EdsGetVolumeInfo(_volumes[1], out outVolumeInfo);
				if (outVolumeInfo.StorageType != 0)
				{
					button3.Enabled = true;
					button3.Text = "Memory Card 2 (" + outVolumeInfo.szVolumeLabel + ")";
				}
			}
		}
		sensorCleaningComboBox.Items.Clear();
		sensorCleaningComboBox.Items.Add("0x00:Perform sensor cleaning.");
		sensorCleaningComboBox.Items.Add("0x01:Perform sensor cleaning (Clean now) process.");
		sensorCleaningComboBox.SelectedIndex = 0;
	}

	public void ShowSettingDialog()
	{
		ShowDialog();
	}

	public void Update(Observable from, CameraEvent e)
	{
		Update(e);
	}

	private void Update(CameraEvent e)
	{
		if (base.InvokeRequired)
		{
			Invoke(new _Update(Update), e);
		}
		else if (e.GetEventType() == CameraEvent.Type.PROPERTY_CHANGED)
		{
			UpdateItems((uint)(int)e.GetArg());
		}
	}

	private uint UpdateItems(uint propertyID)
	{
		IntPtr camera = _controller.GetModel().Camera;
		uint err = 0u;
		string stItem = string.Empty;
		switch (propertyID)
		{
		case 4u:
			err = EDSDK.EdsGetPropertyData(camera, 4u, 0, out stItem);
			if (err != 0)
			{
				return err;
			}
			listView1.Items[3].SubItems[1].Text = stItem;
			break;
		case 1048u:
			err = EDSDK.EdsGetPropertyData(camera, 1048u, 0, out stItem);
			if (err != 0)
			{
				return err;
			}
			listView1.Items[4].SubItems[1].Text = stItem;
			break;
		case 1049u:
			err = EDSDK.EdsGetPropertyData(camera, 1049u, 0, out stItem);
			if (err != 0)
			{
				return err;
			}
			listView1.Items[5].SubItems[1].Text = stItem;
			break;
		}
		return err;
	}

	private void FormSetting_Load(object sender, EventArgs e)
	{
		listView1.FullRowSelect = true;
		listView1.GridLines = true;
		listView1.Columns.Add("Key", listView1.PreferredSize.Width / 2 - 21);
		listView1.Columns.Add("Value", listView1.PreferredSize.Width / 2);
		IntPtr camera = _controller.GetModel().Camera;
		string[] row = new string[2]
		{
			string.Empty,
			string.Empty
		};
		string stItem = string.Empty;
		EDSDK.EdsGetDeviceInfo(camera, out var _);
		EDSDK.EdsGetPropertyData(camera, 2u, 0, out stItem);
		row[0] = "Product Name";
		row[1] = stItem;
		listView1.Items.Add(new ListViewItem(row));
		EDSDK.EdsGetPropertyData(camera, 21u, 0, out stItem);
		row[0] = "Serial Number";
		row[1] = stItem;
		listView1.Items.Add(new ListViewItem(row));
		EDSDK.EdsGetPropertyData(camera, 7u, 0, out stItem);
		row[0] = "Firmware Version";
		row[1] = stItem;
		listView1.Items.Add(new ListViewItem(row));
		EDSDK.EdsGetPropertyData(camera, 4u, 0, out stItem);
		row[0] = "Owner Name";
		row[1] = stItem;
		listView1.Items.Add(new ListViewItem(row));
		textBox3.Text = stItem;
		EDSDK.EdsGetPropertyData(camera, 1048u, 0, out stItem);
		row[0] = "Artist";
		row[1] = stItem;
		listView1.Items.Add(new ListViewItem(row));
		EDSDK.EdsGetPropertyData(camera, 1049u, 0, out stItem);
		row[0] = "Copyright";
		row[1] = stItem;
		listView1.Items.Add(new ListViewItem(row));
	}

	private void button1_Click(object sender, EventArgs e)
	{
		_controller.FormatVolumeCommand(_volumes[0]);
	}

	private void button2_Click(object sender, EventArgs e)
	{
		uint[] property = new uint[3] { 4u, 1048u, 1049u };
		IntPtr camera = _controller.GetModel().Camera;
		string str = textBox3.Text;
		Encoding sjisEnc = Encoding.GetEncoding("EUC-JP");
		if (str.Length == sjisEnc.GetByteCount(str))
		{
			EDSDK.EdsSetPropertyData(camera, property[comboBox1.SelectedIndex], 0, textBox3.Text.Length + 1, str);
		}
	}

	private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
	{
		textBox3.Text = listView1.Items[3 + comboBox1.SelectedIndex].SubItems[1].Text;
	}

	private void button3_Click(object sender, EventArgs e)
	{
		_controller.FormatVolumeCommand(_volumes[1]);
	}

	private void button4_Click(object sender, EventArgs e)
	{
		DateTimeZoneSetting dateTimeZoneSetting = new DateTimeZoneSetting(ref _controller);
		dateTimeZoneSetting.ShowDialog(this);
		dateTimeZoneSetting.Dispose();
	}

	private void autoPowerOff1_SelectionChangeCommitted(object sender, EventArgs e)
	{
		uint key = (uint)_desc.PropDesc[autoPowerOff1.SelectedIndex];
		EDSDK.EdsSetPropertyData(_controller.GetModel().Camera, 16778334u, 0, 4, key);
	}

	private void button5_Click(object sender, EventArgs e)
	{
		RecordFuncCardSetting recordFuncCardSetting = new RecordFuncCardSetting(ref _controller);
		recordFuncCardSetting.ShowDialog(this);
		recordFuncCardSetting.Dispose();
	}

	private void autoPowerOff1_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void groupBox2_Enter(object sender, EventArgs e)
	{
	}

	private void sendCommanBbutton_Click(object sender, EventArgs e)
	{
		int value = sensorCleaningComboBox.SelectedIndex;
		label5.Text = "Running";
		uint err = 0u;
		err = EDSDK.EdsSendCommand(_controller.GetModel().Camera, 274u, value);
		if (err == 0)
		{
			label5.Text = $"SUCCESS!!!";
		}
		else
		{
			label5.Text = $"Err!!! ->0x{err,8:x8}<- ";
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
		this.listView1 = new System.Windows.Forms.ListView();
		this.textBox3 = new System.Windows.Forms.TextBox();
		this.button1 = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.button2 = new System.Windows.Forms.Button();
		this.comboBox1 = new System.Windows.Forms.ComboBox();
		this.label1 = new System.Windows.Forms.Label();
		this.button3 = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.button4 = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.autoPowerOff1 = new System.Windows.Forms.ComboBox();
		this.button5 = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label3 = new System.Windows.Forms.Label();
		this.sensorCleaningComboBox = new System.Windows.Forms.ComboBox();
		this.sendCommanBbutton = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		base.SuspendLayout();
		this.listView1.AllowColumnReorder = true;
		this.listView1.HideSelection = false;
		this.listView1.LabelWrap = false;
		this.listView1.Location = new System.Drawing.Point(11, 7);
		this.listView1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
		this.listView1.MultiSelect = false;
		this.listView1.Name = "listView1";
		this.listView1.Size = new System.Drawing.Size(456, 148);
		this.listView1.TabIndex = 1;
		this.listView1.TabStop = false;
		this.listView1.UseCompatibleStateImageBehavior = false;
		this.listView1.View = System.Windows.Forms.View.Details;
		this.textBox3.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
		this.textBox3.Location = new System.Drawing.Point(190, 174);
		this.textBox3.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
		this.textBox3.Name = "textBox3";
		this.textBox3.Size = new System.Drawing.Size(204, 19);
		this.textBox3.TabIndex = 2;
		this.button1.Enabled = false;
		this.button1.Location = new System.Drawing.Point(20, 18);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(198, 23);
		this.button1.TabIndex = 24;
		this.button1.Text = "Memory Card 1";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(23, 284);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(0, 12);
		this.label7.TabIndex = 22;
		this.button2.Location = new System.Drawing.Point(398, 172);
		this.button2.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(69, 23);
		this.button2.TabIndex = 3;
		this.button2.Text = "Change";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox1.FormattingEnabled = true;
		this.comboBox1.Location = new System.Drawing.Point(11, 174);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Size = new System.Drawing.Size(174, 20);
		this.comboBox1.TabIndex = 25;
		this.comboBox1.SelectionChangeCommitted += new System.EventHandler(comboBox1_SelectionChangeCommitted);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(261, 330);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(0, 12);
		this.label1.TabIndex = 26;
		this.button3.Enabled = false;
		this.button3.Location = new System.Drawing.Point(235, 18);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(198, 23);
		this.button3.TabIndex = 27;
		this.button3.Text = "Memory Card 2";
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Click += new System.EventHandler(button3_Click);
		this.groupBox1.Controls.Add(this.button1);
		this.groupBox1.Controls.Add(this.button3);
		this.groupBox1.Location = new System.Drawing.Point(12, 284);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(455, 55);
		this.groupBox1.TabIndex = 28;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Memory Card Format";
		this.button4.Location = new System.Drawing.Point(11, 208);
		this.button4.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(174, 23);
		this.button4.TabIndex = 29;
		this.button4.Text = "Date / Time / Zone Setting";
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(button4_Click);
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(9, 250);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(84, 12);
		this.label2.TabIndex = 30;
		this.label2.Text = "Auto Power Off";
		this.autoPowerOff1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.autoPowerOff1.FormattingEnabled = true;
		this.autoPowerOff1.Location = new System.Drawing.Point(99, 247);
		this.autoPowerOff1.Name = "autoPowerOff1";
		this.autoPowerOff1.Size = new System.Drawing.Size(86, 20);
		this.autoPowerOff1.TabIndex = 31;
		this.autoPowerOff1.SelectedIndexChanged += new System.EventHandler(autoPowerOff1_SelectedIndexChanged);
		this.autoPowerOff1.SelectionChangeCommitted += new System.EventHandler(autoPowerOff1_SelectionChangeCommitted);
		this.button5.Location = new System.Drawing.Point(247, 208);
		this.button5.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
		this.button5.Name = "button5";
		this.button5.Size = new System.Drawing.Size(174, 23);
		this.button5.TabIndex = 32;
		this.button5.Text = "Record func+card sel. Setting";
		this.button5.UseVisualStyleBackColor = true;
		this.button5.Click += new System.EventHandler(button5_Click);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.sensorCleaningComboBox);
		this.groupBox2.Controls.Add(this.sendCommanBbutton);
		this.groupBox2.Location = new System.Drawing.Point(12, 345);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(455, 108);
		this.groupBox2.TabIndex = 29;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Sensor Cleaning";
		this.groupBox2.Enter += new System.EventHandler(groupBox2_Enter);
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(6, 81);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(334, 12);
		this.label3.TabIndex = 33;
		this.label3.Text = "Note : When Send `0x01` Camera will shut down after execution";
		this.sensorCleaningComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.sensorCleaningComboBox.FormattingEnabled = true;
		this.sensorCleaningComboBox.Location = new System.Drawing.Point(6, 20);
		this.sensorCleaningComboBox.Name = "sensorCleaningComboBox";
		this.sensorCleaningComboBox.Size = new System.Drawing.Size(319, 20);
		this.sensorCleaningComboBox.TabIndex = 32;
		this.sendCommanBbutton.Location = new System.Drawing.Point(334, 18);
		this.sendCommanBbutton.Name = "sendCommanBbutton";
		this.sendCommanBbutton.Size = new System.Drawing.Size(112, 23);
		this.sendCommanBbutton.TabIndex = 27;
		this.sendCommanBbutton.Text = "Send Command";
		this.sendCommanBbutton.UseVisualStyleBackColor = true;
		this.sendCommanBbutton.Click += new System.EventHandler(sendCommanBbutton_Click);
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(6, 56);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(52, 12);
		this.label4.TabIndex = 34;
		this.label4.Text = "Result :  ";
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(60, 56);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(35, 12);
		this.label5.TabIndex = 35;
		this.label5.Text = "-----";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(470, 465);
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.button5);
		base.Controls.Add(this.autoPowerOff1);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.button4);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.comboBox1);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.textBox3);
		base.Controls.Add(this.listView1);
		base.Controls.Add(this.groupBox1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "CameraSetting";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		this.Text = "Camera Control";
		base.Load += new System.EventHandler(FormSetting_Load);
		this.groupBox1.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
