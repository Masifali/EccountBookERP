using System;
using System.ComponentModel;
using System.Drawing;
using System.Management;
using System.Windows.Forms;
using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;

namespace Architecture.PSDSMS;

public class frmMain : Form
{
	private delegate void delegateSendAllMessage(Message Param);

	public GsmCommMain gsmComm;

	public SmsSubmitPdu smsSubmitPdu;

	private IContainer components;

	private TabControl tbSMSControl;

	private TabPage tbSettings;

	private TabPage tbSendSMS;

	private GroupBox gbPortSettings;

	private Button btnConnect;

	private TextBox txtWriteTimeOut;

	private ComboBox cboBaudRate;

	private ComboBox cboPortName;

	private Label label7;

	private Label label2;

	private Label label1;

	private GroupBox gbSendSMS;

	private Button btnSend;

	private TextBox txtMessage;

	private TextBox txtNumber;

	private Label lblConnect;

	private Label label3;

	private Label lblNumber;

	private BackgroundWorker bgAddMessage;

	private Timer tmrAddMessage;

	private BackgroundWorker bgSendMessage;

	private Timer tmrSendMessage;

	public frmMain()
	{
		InitializeComponent();
		SQLCompact40.Instance.Init();
		LoadPortSettings();
	}

	private void LoadPortSettings()
	{
		using ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_POTSModem");
		foreach (ManagementObject queryObj in searcher1.Get())
		{
			queryObj.GetPropertyValue("Name").ToString();
			if (!((string)queryObj["Status"] == "OK"))
			{
				continue;
			}
			string PortNumber = queryObj["AttachedTo"].ToString();
			if (PortNumber.Contains("COM"))
			{
				gsmComm = new GsmCommMain(PortNumber, 115200, 120);
				try
				{
					cboPortName.Items.Add(PortNumber);
				}
				catch
				{
				}
			}
		}
	}

	private void btnConnect_Click(object sender, EventArgs e)
	{
		try
		{
			if ((sender as Button).Text == "Disconnect")
			{
				gsmComm.Close();
				return;
			}
			gsmComm = new GsmCommMain(cboPortName.Text, Convert.ToInt32(cboBaudRate.Text), Convert.ToInt32(txtWriteTimeOut.Text));
			gsmComm.Open();
			if (gsmComm.IsOpen())
			{
				btnConnect.Enabled = false;
				btnConnect.Text = "Disconnect";
				lblConnect.Text = "Connected";
				lblConnect.ForeColor = Color.Red;
			}
			else
			{
				btnConnect.Enabled = true;
				btnConnect.Text = "Connect";
				lblConnect.Text = "Disconnected";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSend_Click(object sender, EventArgs e)
	{
		try
		{
			SmsSubmitPdu[] pdu = SmartMessageFactory.CreateConcatTextMessage(txtMessage.Text, unicode: true, txtNumber.Text);
			GsmCommMain gsmCommMain = gsmComm;
			OutgoingSmsPdu[] pdus = pdu;
			gsmCommMain.SendMessages(pdus);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			gsmComm.Close();
		}
		catch (Exception ex)
		{
			throw ex;
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
		this.components = new System.ComponentModel.Container();
		this.tbSMSControl = new System.Windows.Forms.TabControl();
		this.tbSettings = new System.Windows.Forms.TabPage();
		this.gbPortSettings = new System.Windows.Forms.GroupBox();
		this.lblConnect = new System.Windows.Forms.Label();
		this.btnConnect = new System.Windows.Forms.Button();
		this.txtWriteTimeOut = new System.Windows.Forms.TextBox();
		this.cboBaudRate = new System.Windows.Forms.ComboBox();
		this.cboPortName = new System.Windows.Forms.ComboBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.tbSendSMS = new System.Windows.Forms.TabPage();
		this.gbSendSMS = new System.Windows.Forms.GroupBox();
		this.label3 = new System.Windows.Forms.Label();
		this.lblNumber = new System.Windows.Forms.Label();
		this.btnSend = new System.Windows.Forms.Button();
		this.txtMessage = new System.Windows.Forms.TextBox();
		this.txtNumber = new System.Windows.Forms.TextBox();
		this.bgAddMessage = new System.ComponentModel.BackgroundWorker();
		this.tmrAddMessage = new System.Windows.Forms.Timer(this.components);
		this.bgSendMessage = new System.ComponentModel.BackgroundWorker();
		this.tmrSendMessage = new System.Windows.Forms.Timer(this.components);
		this.tbSMSControl.SuspendLayout();
		this.tbSettings.SuspendLayout();
		this.gbPortSettings.SuspendLayout();
		this.tbSendSMS.SuspendLayout();
		this.gbSendSMS.SuspendLayout();
		base.SuspendLayout();
		this.tbSMSControl.Controls.Add(this.tbSettings);
		this.tbSMSControl.Controls.Add(this.tbSendSMS);
		this.tbSMSControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tbSMSControl.Location = new System.Drawing.Point(0, 0);
		this.tbSMSControl.Name = "tbSMSControl";
		this.tbSMSControl.SelectedIndex = 0;
		this.tbSMSControl.Size = new System.Drawing.Size(303, 345);
		this.tbSMSControl.TabIndex = 0;
		this.tbSettings.Controls.Add(this.gbPortSettings);
		this.tbSettings.Location = new System.Drawing.Point(4, 22);
		this.tbSettings.Name = "tbSettings";
		this.tbSettings.Padding = new System.Windows.Forms.Padding(3);
		this.tbSettings.Size = new System.Drawing.Size(295, 319);
		this.tbSettings.TabIndex = 0;
		this.tbSettings.Text = "Settings";
		this.tbSettings.UseVisualStyleBackColor = true;
		this.gbPortSettings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.gbPortSettings.Controls.Add(this.lblConnect);
		this.gbPortSettings.Controls.Add(this.btnConnect);
		this.gbPortSettings.Controls.Add(this.txtWriteTimeOut);
		this.gbPortSettings.Controls.Add(this.cboBaudRate);
		this.gbPortSettings.Controls.Add(this.cboPortName);
		this.gbPortSettings.Controls.Add(this.label7);
		this.gbPortSettings.Controls.Add(this.label2);
		this.gbPortSettings.Controls.Add(this.label1);
		this.gbPortSettings.Location = new System.Drawing.Point(8, 9);
		this.gbPortSettings.Name = "gbPortSettings";
		this.gbPortSettings.Size = new System.Drawing.Size(280, 310);
		this.gbPortSettings.TabIndex = 0;
		this.gbPortSettings.TabStop = false;
		this.gbPortSettings.Text = "Port Settings";
		this.lblConnect.AutoSize = true;
		this.lblConnect.Location = new System.Drawing.Point(187, 189);
		this.lblConnect.Name = "lblConnect";
		this.lblConnect.Size = new System.Drawing.Size(0, 13);
		this.lblConnect.TabIndex = 7;
		this.btnConnect.Location = new System.Drawing.Point(111, 113);
		this.btnConnect.Name = "btnConnect";
		this.btnConnect.Size = new System.Drawing.Size(163, 23);
		this.btnConnect.TabIndex = 6;
		this.btnConnect.Text = "Connect";
		this.btnConnect.UseVisualStyleBackColor = true;
		this.btnConnect.Click += new System.EventHandler(btnConnect_Click);
		this.txtWriteTimeOut.Location = new System.Drawing.Point(111, 87);
		this.txtWriteTimeOut.MaxLength = 5;
		this.txtWriteTimeOut.Name = "txtWriteTimeOut";
		this.txtWriteTimeOut.Size = new System.Drawing.Size(163, 20);
		this.txtWriteTimeOut.TabIndex = 5;
		this.txtWriteTimeOut.Text = "300";
		this.cboBaudRate.FormattingEnabled = true;
		this.cboBaudRate.Items.AddRange(new object[13]
		{
			"110", "300", "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200",
			"230400", "460800", "921600"
		});
		this.cboBaudRate.Location = new System.Drawing.Point(111, 60);
		this.cboBaudRate.Name = "cboBaudRate";
		this.cboBaudRate.Size = new System.Drawing.Size(163, 21);
		this.cboBaudRate.TabIndex = 3;
		this.cboBaudRate.Text = "9600";
		this.cboPortName.FormattingEnabled = true;
		this.cboPortName.Location = new System.Drawing.Point(111, 33);
		this.cboPortName.Name = "cboPortName";
		this.cboPortName.Size = new System.Drawing.Size(163, 21);
		this.cboPortName.TabIndex = 1;
		this.cboPortName.Text = "COM1";
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(20, 90);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(73, 13);
		this.label7.TabIndex = 4;
		this.label7.Text = "Write Timeout";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(20, 63);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 13);
		this.label2.TabIndex = 2;
		this.label2.Text = "Baud Rate";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(20, 37);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(57, 13);
		this.label1.TabIndex = 0;
		this.label1.Text = "Port Name";
		this.tbSendSMS.Controls.Add(this.gbSendSMS);
		this.tbSendSMS.Location = new System.Drawing.Point(4, 22);
		this.tbSendSMS.Name = "tbSendSMS";
		this.tbSendSMS.Padding = new System.Windows.Forms.Padding(3);
		this.tbSendSMS.Size = new System.Drawing.Size(295, 319);
		this.tbSendSMS.TabIndex = 1;
		this.tbSendSMS.Text = "Send SMS";
		this.tbSendSMS.UseVisualStyleBackColor = true;
		this.gbSendSMS.Controls.Add(this.label3);
		this.gbSendSMS.Controls.Add(this.lblNumber);
		this.gbSendSMS.Controls.Add(this.btnSend);
		this.gbSendSMS.Controls.Add(this.txtMessage);
		this.gbSendSMS.Controls.Add(this.txtNumber);
		this.gbSendSMS.Location = new System.Drawing.Point(6, 6);
		this.gbSendSMS.Name = "gbSendSMS";
		this.gbSendSMS.Size = new System.Drawing.Size(281, 305);
		this.gbSendSMS.TabIndex = 0;
		this.gbSendSMS.TabStop = false;
		this.gbSendSMS.Text = "Send SMS";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(6, 63);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(50, 13);
		this.label3.TabIndex = 2;
		this.label3.Text = "Message";
		this.lblNumber.AutoSize = true;
		this.lblNumber.Location = new System.Drawing.Point(6, 37);
		this.lblNumber.Name = "lblNumber";
		this.lblNumber.Size = new System.Drawing.Size(55, 13);
		this.lblNumber.TabIndex = 0;
		this.lblNumber.Text = "Mobile No";
		this.btnSend.Location = new System.Drawing.Point(89, 255);
		this.btnSend.Name = "btnSend";
		this.btnSend.Size = new System.Drawing.Size(186, 23);
		this.btnSend.TabIndex = 4;
		this.btnSend.Text = "Send";
		this.btnSend.UseVisualStyleBackColor = true;
		this.btnSend.Click += new System.EventHandler(btnSend_Click);
		this.txtMessage.Location = new System.Drawing.Point(89, 60);
		this.txtMessage.Multiline = true;
		this.txtMessage.Name = "txtMessage";
		this.txtMessage.Size = new System.Drawing.Size(186, 189);
		this.txtMessage.TabIndex = 3;
		this.txtNumber.Location = new System.Drawing.Point(89, 34);
		this.txtNumber.Name = "txtNumber";
		this.txtNumber.Size = new System.Drawing.Size(186, 20);
		this.txtNumber.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(303, 345);
		base.Controls.Add(this.tbSMSControl);
		base.Name = "frmMain";
		this.Text = "MNC SMS Service";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmMain_FormClosing);
		this.tbSMSControl.ResumeLayout(false);
		this.tbSettings.ResumeLayout(false);
		this.gbPortSettings.ResumeLayout(false);
		this.gbPortSettings.PerformLayout();
		this.tbSendSMS.ResumeLayout(false);
		this.gbSendSMS.ResumeLayout(false);
		this.gbSendSMS.PerformLayout();
		base.ResumeLayout(false);
	}
}
