using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
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

public class DateTimeZoneSetting : Form
{
	private CameraController _controller;

	private IntPtr _camera = (IntPtr)0;

	private string _localTime;

	private string _localTimePrev;

	private uint _timeZone;

	private uint _timeZonePrev;

	private bool _summerTimeSetting;

	private bool _summerTimeSettingPrev;

	private EDSDK.EdsPropertyDesc _desc;

	protected Dictionary<uint, string> map = new Dictionary<uint, string>();

	private IContainer components;

	private Label label1;

	private ComboBox comboBox1;

	private CheckBox checkBox1;

	private GroupBox groupBox1;

	private Button button1;

	private Button button2;

	private TextBox textBox1;

	public DateTimeZoneSetting(ref CameraController controller)
	{
		map.Add(0u, "None");
		map.Add(1u, "Chatham Islands");
		map.Add(2u, "Wellington");
		map.Add(3u, "Solomon Islands");
		map.Add(4u, "Sydney");
		map.Add(5u, "Adelaide");
		map.Add(6u, "Tokyo");
		map.Add(7u, "Hong Kong");
		map.Add(8u, "Bangkok");
		map.Add(9u, "Yangon");
		map.Add(10u, "Dhaka");
		map.Add(11u, "Kathmandu");
		map.Add(12u, "Delhi");
		map.Add(13u, "Karachi");
		map.Add(14u, "Kabul");
		map.Add(15u, "Dubai");
		map.Add(16u, "Tehran");
		map.Add(17u, "Moscow");
		map.Add(18u, "Cairo");
		map.Add(19u, "Paris");
		map.Add(20u, "London");
		map.Add(21u, "Azores");
		map.Add(22u, "Fernando");
		map.Add(23u, "Sao Paulo");
		map.Add(24u, "Newfoundland");
		map.Add(25u, "Santiago");
		map.Add(26u, "Caracas");
		map.Add(27u, "New York");
		map.Add(28u, "Chicago");
		map.Add(29u, "Denver");
		map.Add(30u, "Los Angeles");
		map.Add(31u, "Anchorage");
		map.Add(32u, "Honolulu");
		map.Add(33u, "Samoa");
		map.Add(34u, "Riyadh");
		map.Add(35u, "Manaus");
		map.Add(256u, "UTC");
		map.Add(uint.MaxValue, "unknown");
		_controller = controller;
		_camera = _controller.GetModel().Camera;
		InitializeComponent();
		EDSDK.EdsGetPropertyData(_camera, 16777238u, 0, out EDSDK.EdsTime utcTime);
		EDSDK.EdsGetPropertyData(_camera, 16777239u, 0, out _timeZone);
		_desc = default(EDSDK.EdsPropertyDesc);
		EDSDK.EdsGetPropertyDesc(_camera, 16777239u, out _desc);
		comboBox1.Items.Clear();
		for (int i = 0; i < _desc.NumElements; i++)
		{
			if (map.TryGetValue((uint)_desc.PropDesc[i] >> 16, out var outString) && !outString.Equals("unknown"))
			{
				comboBox1.Items.Add(outString);
				if (_timeZone == _desc.PropDesc[i])
				{
					comboBox1.SelectedIndex = i;
					_timeZone = (uint)_desc.PropDesc[i];
					_timeZonePrev = _timeZone;
				}
			}
		}
		EDSDK.EdsGetPropertyData(_camera, 16777240u, 0, out uint summerTimeSetting);
		if (summerTimeSetting == 1)
		{
			checkBox1.Checked = true;
		}
		_summerTimeSetting = checkBox1.Checked;
		_summerTimeSettingPrev = _summerTimeSetting;
		DateTime dateTime = EdsTime2DateTime(utcTime);
		short timeDiff = (short)(_timeZone & 0xFFFF);
		TimeSpan timeSpan = new TimeSpan(0, Convert.ToInt32(checkBox1.Checked), timeDiff, 0);
		dateTime = dateTime.Add(timeSpan);
		utcTime = DateTime2EdsTime(dateTime);
		_localTime = EdsTime2StrTime(utcTime);
		_localTimePrev = _localTime;
		textBox1.Text = _localTime;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		int selectedItem = comboBox1.SelectedIndex;
		uint timeZone = (uint)_desc.PropDesc[selectedItem];
		if (_timeZone != timeZone)
		{
			EDSDK.EdsSetPropertyData(_camera, 16777239u, 0, 4, timeZone);
		}
		_timeZone = timeZone;
		string localTime = textBox1.Text;
		if (_localTime != localTime)
		{
			if (tryParseExact(out var dateTime))
			{
				short timeDiff = (short)(_timeZone & 0xFFFF);
				TimeSpan timeSpan = new TimeSpan(0, Convert.ToInt32(checkBox1.Checked), timeDiff, 0);
				dateTime = dateTime.Subtract(timeSpan);
				EDSDK.EdsTime edsDateTime = DateTime2EdsTime(dateTime);
				EDSDK.EdsSetPropertyData(_camera, 16777238u, 0, Marshal.SizeOf(typeof(EDSDK.EdsTime)), edsDateTime);
			}
			else
			{
				MessageBox.Show("Enter in the \"yyyy/MM/DD HH:mm\" format.");
				base.DialogResult = DialogResult.None;
			}
		}
		if (_summerTimeSetting != checkBox1.Checked)
		{
			EDSDK.EdsSetPropertyData(_camera, 16777240u, 0, 4, Convert.ToUInt32(checkBox1.Checked));
		}
	}

	private void textBox1_Leave(object sender, EventArgs e)
	{
		string utcTime = textBox1.Text;
		if (!(_localTimePrev != utcTime))
		{
			return;
		}
		if (!tryParseExact(out var dateTime))
		{
			MessageBox.Show("Enter in the \"yyyy/MM/DD HH:mm\" format.");
			textBox1.Text = _localTimePrev;
			return;
		}
		if (!IsRange(dateTime.Year, 2019, 2050))
		{
			MessageBox.Show("Year must be between 2019 and 2050");
			textBox1.Text = _localTimePrev;
		}
		_localTimePrev = textBox1.Text;
	}

	private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
	{
		int selectedItem = comboBox1.SelectedIndex;
		uint timeZone = (uint)_desc.PropDesc[selectedItem];
		if (_timeZonePrev != timeZone && tryParseExact(out var dateTime))
		{
			short timeDiff = (short)(_timeZonePrev & 0xFFFF);
			TimeSpan timeSpan = new TimeSpan(0, 0, timeDiff, 0);
			dateTime = dateTime.Subtract(timeSpan);
			timeDiff = (short)(timeZone & 0xFFFF);
			timeSpan = new TimeSpan(0, 0, timeDiff, 0);
			dateTime = dateTime.Add(timeSpan);
			textBox1.Text = EdsTime2StrTime(DateTime2EdsTime(dateTime));
		}
		_timeZonePrev = timeZone;
	}

	private void checkBox1_CheckedChanged(object sender, EventArgs e)
	{
		if (_summerTimeSettingPrev != checkBox1.Checked && tryParseExact(out var dateTime))
		{
			TimeSpan timeSpan = new TimeSpan(0, 1, 0, 0);
			if (!checkBox1.Checked)
			{
				timeSpan = new TimeSpan(0, -1, 0, 0);
			}
			dateTime = dateTime.Add(timeSpan);
			textBox1.Text = EdsTime2StrTime(DateTime2EdsTime(dateTime));
		}
		_summerTimeSettingPrev = checkBox1.Checked;
	}

	private DateTime EdsTime2DateTime(EDSDK.EdsTime edsTime)
	{
		return new DateTime(edsTime.Year, edsTime.Month, edsTime.Day, edsTime.Hour, edsTime.Minute, edsTime.Second, edsTime.Milliseconds);
	}

	private EDSDK.EdsTime DateTime2EdsTime(DateTime dateTime)
	{
		EDSDK.EdsTime edsTime = default(EDSDK.EdsTime);
		edsTime.Year = dateTime.Year;
		edsTime.Month = dateTime.Month;
		edsTime.Day = dateTime.Day;
		edsTime.Hour = dateTime.Hour;
		edsTime.Minute = dateTime.Minute;
		edsTime.Second = dateTime.Second;
		edsTime.Milliseconds = dateTime.Millisecond;
		return edsTime;
	}

	private string EdsTime2StrTime(EDSDK.EdsTime edsTime)
	{
		return edsTime.Year.ToString("D4") + "/" + edsTime.Month.ToString("D2") + "/" + edsTime.Day.ToString("D2") + " " + edsTime.Hour.ToString("D2") + ":" + edsTime.Minute.ToString("D2");
	}

	private bool IsRange(int a, int from, int to)
	{
		if (from <= a)
		{
			return a <= to;
		}
		return false;
	}

	private bool tryParseExact(out DateTime dataTime)
	{
		string s = textBox1.Text;
		string format = "yyyy/MM/dd HH:mm";
		CultureInfo culture = CultureInfo.InvariantCulture;
		DateTimeStyles dts = DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal;
		return DateTime.TryParseExact(s, format, culture, dts, out dataTime);
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
		this.comboBox1 = new System.Windows.Forms.ComboBox();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(12, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(198, 12);
		this.label1.TabIndex = 0;
		this.label1.Text = "Date / Time : (ex : 2019/09/12 09:11)";
		this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox1.FormattingEnabled = true;
		this.comboBox1.Location = new System.Drawing.Point(6, 18);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Size = new System.Drawing.Size(209, 20);
		this.comboBox1.TabIndex = 5;
		this.comboBox1.SelectionChangeCommitted += new System.EventHandler(comboBox1_SelectionChangeCommitted);
		this.checkBox1.AutoSize = true;
		this.checkBox1.Location = new System.Drawing.Point(221, 22);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(133, 16);
		this.checkBox1.TabIndex = 6;
		this.checkBox1.Text = "Daylight Saving Time";
		this.checkBox1.UseVisualStyleBackColor = true;
		this.checkBox1.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
		this.groupBox1.Controls.Add(this.comboBox1);
		this.groupBox1.Controls.Add(this.checkBox1);
		this.groupBox1.Location = new System.Drawing.Point(12, 59);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(360, 59);
		this.groupBox1.TabIndex = 7;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Zone Setting";
		this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.button1.Location = new System.Drawing.Point(216, 127);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 8;
		this.button1.Text = "OK";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.button2.Location = new System.Drawing.Point(297, 127);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(75, 23);
		this.button2.TabIndex = 9;
		this.button2.Text = "Cancel";
		this.button2.UseVisualStyleBackColor = true;
		this.textBox1.Location = new System.Drawing.Point(14, 25);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(184, 19);
		this.textBox1.TabIndex = 10;
		this.textBox1.Leave += new System.EventHandler(textBox1_Leave);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(384, 162);
		base.Controls.Add(this.textBox1);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.label1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "DateTimeZoneSetting";
		this.Text = "Date / Time / Zone Setting";
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
