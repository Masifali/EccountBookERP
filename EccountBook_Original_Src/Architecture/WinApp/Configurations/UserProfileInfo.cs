using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.VPS;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.Configurations;

public class UserProfileInfo : Form
{
	private ServerCredentials Sc = new ServerCredentials();

	private bool IsVpsAttachmentsServiceOn;

	private Image overlayImage;

	private IContainer components;

	private Panel panel2;

	private PictureBox UserPicture;

	private PictureBox PhonePicture;

	private PictureBox LockPicture;

	private PictureBox MailPicture;

	private Label PhoneNumber;

	private Label label8;

	private Label Password;

	private Label label6;

	private Label EmailAddress;

	private Label label4;

	private Label UserName1;

	private Label label1;

	private Button btnPicture;

	private BackgroundWorker backgroundWorker1;

	private Label label9;

	private Label userName;

	private PictureBox HiPicture;

	private new Label CompanyName;

	private Label label2;

	private Label label5;

	private Label label3;

	public UserProfileInfo()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		SetFormToLeftBottom();
		ImageLoading();
		BindOtherInfoData();
	}

	private void ImageLoading()
	{
		try
		{
			btnPicture.FlatStyle = FlatStyle.Flat;
			btnPicture.FlatAppearance.BorderSize = 0;
			btnPicture.Region = new Region(CreateRoundPath(btnPicture.Width, btnPicture.Height));
			IsVpsAttachmentsServiceOn = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("IsVpsAttachmentsServiceOn"));
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			UserAccount userAccount = clsGlobalVariables.UserAccount;
			object obj;
			if (userAccount == null)
			{
				obj = null;
			}
			else
			{
				UserProfile userProfileList = userAccount.UserProfileList;
				obj = ((userProfileList != null) ? userProfileList.ProfileImageFileName : null);
			}
			if (obj != null)
			{
				UserAccount userAccount2 = clsGlobalVariables.UserAccount;
				object obj2;
				if (userAccount2 == null)
				{
					obj2 = null;
				}
				else
				{
					UserProfile userProfileList2 = userAccount2.UserProfileList;
					obj2 = ((userProfileList2 != null) ? userProfileList2.ProfileImageFileName : null);
				}
				if ((string)obj2 != string.Empty)
				{
					if (!IsVpsAttachmentsServiceOn)
					{
						if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
						{
							throw new Exception("Attachment Folder Path Not Configure Properly");
						}
						Bitmap circularImage = CommonServices.MakeCircularImage(CommonServices.ResizeImage(new Bitmap(Path.Combine(clsGlobalVariables.AttachmentFolderPath, clsGlobalVariables.UserAccount.UserProfileList.ProfileImageFileName)), btnPicture.Width, btnPicture.Height));
						btnPicture.Image = circularImage;
						return;
					}
					string uri = "ftp://" + Sc.ServerIp + "/" + clsGlobalVariables.UserAccount.UserProfileList.ProfileImageFileName;
					try
					{
						FtpWebRequest obj3 = (FtpWebRequest)WebRequest.Create(uri);
						obj3.Method = "RETR";
						obj3.UsePassive = true;
						obj3.UseBinary = true;
						obj3.KeepAlive = false;
						obj3.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using FtpWebResponse ftpWebResponse = (FtpWebResponse)obj3.GetResponse();
						using Stream responseStream = ftpWebResponse.GetResponseStream();
						using MemoryStream memoryStream = new MemoryStream();
						responseStream.CopyTo(memoryStream);
						_ = (double)memoryStream.Length / 1048576.0;
						memoryStream.Position = 0L;
						using MemoryStream stream = new MemoryStream();
						Image image = (overlayImage = Image.FromStream(memoryStream));
						Bitmap bmp = new Bitmap(image);
						bmp.Save(stream, image.RawFormat);
						btnPicture.Image = bmp;
						image.Dispose();
						stream.Position = 0L;
						stream.Dispose();
						return;
					}
					catch (WebException webEx)
					{
						if (webEx.Response is FtpWebResponse response)
						{
							throw new Exception("FTP Error: " + response.StatusDescription + " - " + webEx.Message);
						}
						throw new Exception("Error: " + webEx.Message);
					}
					catch (Exception ex)
					{
						throw new Exception("Error: " + ex.Message);
					}
				}
			}
			Bitmap circularImage2 = CommonServices.MakeCircularImage(CommonServices.ResizeImage(new Bitmap(Resources.DummyUser), btnPicture.Width, btnPicture.Height));
			btnPicture.Image = circularImage2;
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private GraphicsPath CreateRoundPath(int width, int height)
	{
		GraphicsPath graphicsPath = new GraphicsPath();
		graphicsPath.AddEllipse(0, 0, width, height);
		return graphicsPath;
	}

	private void BindOtherInfoData()
	{
		try
		{
			userName.Text = "Hi," + clsGlobalVariables.UserAccount.UserName;
			UserName1.Text = clsGlobalVariables.UserAccount.UserName;
			string maskedString = MaskString(Encryption.UserPasswordDecryptString(clsGlobalVariables.UserAccount.Password));
			Password.Text = maskedString;
			CompanyName.Text = clsGlobalVariables.UserAccount.CompName;
			Label emailAddress = EmailAddress;
			UserProfile userProfileList = clsGlobalVariables.UserAccount.UserProfileList;
			emailAddress.Text = ((userProfileList != null) ? userProfileList.Email : null);
			Label phoneNumber = PhoneNumber;
			UserProfile userProfileList2 = clsGlobalVariables.UserAccount.UserProfileList;
			phoneNumber.Text = ((userProfileList2 != null) ? userProfileList2.CellNo : null);
			Bitmap Hi_Circular = CommonServices.MakeCircularImage(CommonServices.ResizeImage(new Bitmap(Resources.Hi_Hand), HiPicture.Width, HiPicture.Height));
			HiPicture.Image = Hi_Circular;
			HiPicture.SizeMode = PictureBoxSizeMode.CenterImage;
			Bitmap User_Circular = CommonServices.MakeCircularImage(CommonServices.ResizeImage(new Bitmap(Resources.user_line), UserPicture.Width, UserPicture.Height));
			UserPicture.Image = User_Circular;
			UserPicture.SizeMode = PictureBoxSizeMode.CenterImage;
			Bitmap Mail_Circular = CommonServices.MakeCircularImage(CommonServices.ResizeImage(new Bitmap(Resources.mail_line), MailPicture.Width, MailPicture.Height));
			MailPicture.Image = Mail_Circular;
			MailPicture.SizeMode = PictureBoxSizeMode.CenterImage;
			Bitmap Lock_Circular = CommonServices.MakeCircularImage(CommonServices.ResizeImage(new Bitmap(Resources.lock_line), LockPicture.Width, LockPicture.Height));
			LockPicture.Image = Lock_Circular;
			LockPicture.SizeMode = PictureBoxSizeMode.CenterImage;
			Bitmap Phone_Circular = CommonServices.MakeCircularImage(new Bitmap(Resources.phone_line));
			PhonePicture.Image = Phone_Circular;
			PhonePicture.SizeMode = PictureBoxSizeMode.CenterImage;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static string MaskString(string input)
	{
		return new string('*', input.Length);
	}

	private void SetFormToLeftBottom()
	{
		base.StartPosition = FormStartPosition.Manual;
		Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
		base.Location = new Point(workingArea.Left, workingArea.Bottom - base.Bounds.Height);
	}

	private void UserProfileInfo_Load(object sender, EventArgs e)
	{
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
		this.panel2 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CompanyName = new System.Windows.Forms.Label();
		this.HiPicture = new System.Windows.Forms.PictureBox();
		this.userName = new System.Windows.Forms.Label();
		this.PhoneNumber = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.Password = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.EmailAddress = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.UserName1 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.PhonePicture = new System.Windows.Forms.PictureBox();
		this.LockPicture = new System.Windows.Forms.PictureBox();
		this.MailPicture = new System.Windows.Forms.PictureBox();
		this.UserPicture = new System.Windows.Forms.PictureBox();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.label9 = new System.Windows.Forms.Label();
		this.btnPicture = new System.Windows.Forms.Button();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.HiPicture).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PhonePicture).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.LockPicture).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.MailPicture).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.UserPicture).BeginInit();
		base.SuspendLayout();
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.CompanyName);
		this.panel2.Controls.Add(this.HiPicture);
		this.panel2.Controls.Add(this.userName);
		this.panel2.Controls.Add(this.PhoneNumber);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add(this.Password);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.EmailAddress);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.UserName1);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.PhonePicture);
		this.panel2.Controls.Add(this.LockPicture);
		this.panel2.Controls.Add(this.MailPicture);
		this.panel2.Controls.Add(this.UserPicture);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(308, 360);
		this.panel2.TabIndex = 1;
		this.label5.BackColor = System.Drawing.Color.Gainsboro;
		this.label5.Location = new System.Drawing.Point(14, 306);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(280, 1);
		this.label5.TabIndex = 17;
		this.label3.BackColor = System.Drawing.Color.Gainsboro;
		this.label3.Location = new System.Drawing.Point(14, 266);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(280, 1);
		this.label3.TabIndex = 16;
		this.label2.BackColor = System.Drawing.Color.Gainsboro;
		this.label2.Location = new System.Drawing.Point(15, 228);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(280, 1);
		this.label2.TabIndex = 15;
		this.CompanyName.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CompanyName.ForeColor = System.Drawing.Color.Gray;
		this.CompanyName.Location = new System.Drawing.Point(11, 167);
		this.CompanyName.Name = "CompanyName";
		this.CompanyName.Size = new System.Drawing.Size(295, 17);
		this.CompanyName.TabIndex = 14;
		this.CompanyName.Text = "User Name";
		this.CompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.HiPicture.Image = Architecture.WinApp.Properties.Resources.Hi_Hand;
		this.HiPicture.Location = new System.Drawing.Point(245, 137);
		this.HiPicture.Name = "HiPicture";
		this.HiPicture.Size = new System.Drawing.Size(30, 30);
		this.HiPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.HiPicture.TabIndex = 13;
		this.HiPicture.TabStop = false;
		this.userName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.userName.Location = new System.Drawing.Point(83, 144);
		this.userName.Name = "userName";
		this.userName.Size = new System.Drawing.Size(156, 16);
		this.userName.TabIndex = 12;
		this.userName.Text = "UserName";
		this.userName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.PhoneNumber.AutoSize = true;
		this.PhoneNumber.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.PhoneNumber.Location = new System.Drawing.Point(72, 323);
		this.PhoneNumber.Name = "PhoneNumber";
		this.PhoneNumber.Size = new System.Drawing.Size(51, 16);
		this.PhoneNumber.TabIndex = 11;
		this.PhoneNumber.Text = "label7";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Gray;
		this.label8.Location = new System.Drawing.Point(71, 309);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(87, 13);
		this.label8.TabIndex = 10;
		this.label8.Text = "Phone Number";
		this.Password.AutoSize = true;
		this.Password.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Password.Location = new System.Drawing.Point(73, 285);
		this.Password.Name = "Password";
		this.Password.Size = new System.Drawing.Size(51, 16);
		this.Password.TabIndex = 9;
		this.Password.Text = "label5";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Gray;
		this.label6.Location = new System.Drawing.Point(72, 271);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(57, 13);
		this.label6.TabIndex = 8;
		this.label6.Text = "Password";
		this.EmailAddress.AutoSize = true;
		this.EmailAddress.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.EmailAddress.Location = new System.Drawing.Point(74, 247);
		this.EmailAddress.Name = "EmailAddress";
		this.EmailAddress.Size = new System.Drawing.Size(51, 16);
		this.EmailAddress.TabIndex = 7;
		this.EmailAddress.Text = "label3";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Gray;
		this.label4.Location = new System.Drawing.Point(73, 233);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(80, 13);
		this.label4.TabIndex = 6;
		this.label4.Text = "Email Address";
		this.UserName1.AutoSize = true;
		this.UserName1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.UserName1.Location = new System.Drawing.Point(74, 209);
		this.UserName1.Name = "UserName1";
		this.UserName1.Size = new System.Drawing.Size(51, 16);
		this.UserName1.TabIndex = 5;
		this.UserName1.Text = "label2";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Gray;
		this.label1.Location = new System.Drawing.Point(73, 195);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(64, 13);
		this.label1.TabIndex = 4;
		this.label1.Text = "User Name";
		this.PhonePicture.Image = Architecture.WinApp.Properties.Resources.phone_line;
		this.PhonePicture.Location = new System.Drawing.Point(41, 314);
		this.PhonePicture.Name = "PhonePicture";
		this.PhonePicture.Size = new System.Drawing.Size(25, 25);
		this.PhonePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.PhonePicture.TabIndex = 3;
		this.PhonePicture.TabStop = false;
		this.LockPicture.Image = Architecture.WinApp.Properties.Resources.lock_line;
		this.LockPicture.Location = new System.Drawing.Point(42, 276);
		this.LockPicture.Name = "LockPicture";
		this.LockPicture.Size = new System.Drawing.Size(25, 25);
		this.LockPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.LockPicture.TabIndex = 2;
		this.LockPicture.TabStop = false;
		this.MailPicture.Image = Architecture.WinApp.Properties.Resources.mail_line;
		this.MailPicture.Location = new System.Drawing.Point(43, 238);
		this.MailPicture.Name = "MailPicture";
		this.MailPicture.Size = new System.Drawing.Size(25, 25);
		this.MailPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.MailPicture.TabIndex = 1;
		this.MailPicture.TabStop = false;
		this.UserPicture.Image = Architecture.WinApp.Properties.Resources.user_line;
		this.UserPicture.InitialImage = Architecture.WinApp.Properties.Resources.user_line;
		this.UserPicture.Location = new System.Drawing.Point(43, 196);
		this.UserPicture.Name = "UserPicture";
		this.UserPicture.Size = new System.Drawing.Size(25, 25);
		this.UserPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.UserPicture.TabIndex = 0;
		this.UserPicture.TabStop = false;
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Dock = System.Windows.Forms.DockStyle.Top;
		this.label9.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(0, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(308, 92);
		this.label9.TabIndex = 6;
		this.btnPicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnPicture.Cursor = System.Windows.Forms.Cursors.Arrow;
		this.btnPicture.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnPicture.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnPicture.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnPicture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPicture.Image = Architecture.WinApp.Properties.Resources.DummyUser;
		this.btnPicture.Location = new System.Drawing.Point(111, 40);
		this.btnPicture.Margin = new System.Windows.Forms.Padding(0);
		this.btnPicture.Name = "btnPicture";
		this.btnPicture.Size = new System.Drawing.Size(100, 100);
		this.btnPicture.TabIndex = 2;
		this.btnPicture.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(308, 360);
		base.Controls.Add(this.btnPicture);
		base.Controls.Add(this.label9);
		base.Controls.Add(this.panel2);
		base.Name = "UserProfileInfo";
		this.Text = "UserProfileInfo";
		base.Load += new System.EventHandler(UserProfileInfo_Load);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.HiPicture).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PhonePicture).EndInit();
		((System.ComponentModel.ISupportInitialize)this.LockPicture).EndInit();
		((System.ComponentModel.ISupportInitialize)this.MailPicture).EndInit();
		((System.ComponentModel.ISupportInitialize)this.UserPicture).EndInit();
		base.ResumeLayout(false);
	}
}
