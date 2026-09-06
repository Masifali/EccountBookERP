using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.DynamicCards;

public class ScreenNameDescriptionIsFavourite : UserControl
{
	public class ControlClickedEventArgs : EventArgs
	{
		public Control ClickedControl { get; set; }

		public ControlClickedEventArgs(Control clickedControl)
		{
			ClickedControl = clickedControl;
		}
	}

	private UserAccount UserAccount = new UserAccount();

	private bool _IsFavorite;

	private int _ScreenId;

	private int _ModuleId;

	private string _ScreenName;

	private string _Id;

	private string _title;

	private string _titleDescription;

	private IContainer components;

	private Label lblScreenName;

	private Label lblScreenDescription;

	private PictureBox pictureBox1;

	public bool IsFavorite
	{
		get
		{
			return _IsFavorite;
		}
		set
		{
			_IsFavorite = value;
		}
	}

	public int ScreenId
	{
		get
		{
			return _ScreenId;
		}
		set
		{
			_ScreenId = value;
		}
	}

	public int ModuleId
	{
		get
		{
			return _ModuleId;
		}
		set
		{
			_ModuleId = value;
		}
	}

	public string ScreenName
	{
		get
		{
			return _ScreenName;
		}
		set
		{
			_ScreenName = value;
		}
	}

	public string Id
	{
		get
		{
			return _Id;
		}
		set
		{
			_Id = value;
		}
	}

	public string Title
	{
		get
		{
			return _title;
		}
		set
		{
			_title = value;
			lblScreenName.Text = value;
		}
	}

	public string TitleDescription
	{
		get
		{
			return _titleDescription;
		}
		set
		{
			_titleDescription = value;
			lblScreenDescription.Text = value;
		}
	}

	public event EventHandler UserControlClicked;

	public ScreenNameDescriptionIsFavourite(UserAccount pUser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
		base.Click += ControlClicked;
		pictureBox1.Click += ControlClicked;
		lblScreenName.Click += ControlClicked;
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		Control clickedControl = (Control)sender;
		if (clickedControl == pictureBox1)
		{
			this.UserControlClicked?.Invoke(this, new ControlClickedEventArgs(pictureBox1));
		}
		else
		{
			this.UserControlClicked?.Invoke(this, new ControlClickedEventArgs(clickedControl));
		}
	}

	private void ScreenNameDescriptionIsFavourite_Load(object sender, EventArgs e)
	{
		try
		{
			if (!_IsFavorite)
			{
				pictureBox1.Image = Resources.star_line;
			}
			else
			{
				pictureBox1.Image = Resources.star_fill;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			FavoriteScreens obj = new FavoriteScreens();
			if (!_IsFavorite)
			{
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.Id = 0;
				obj.ScreenId = _ScreenId;
				obj.ScreenName = _ScreenName;
				obj.ScreenTitle = _title;
				obj.TargetUrl = _Id;
				obj.UserId = UserAccount.ID;
				obj.ScreenRoute = "";
				FavoriteScreens.save(obj);
				_IsFavorite = true;
				clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => Conversion.ToString((object)x.ScreenName) == _ScreenName && x.Value).ToList().ForEach(delegate(tblUserRights x)
				{
					x.IsFavorite = true;
				});
			}
			else
			{
				FavoriteScreens.DeleteByScreenNameAndUserId(_ScreenName, UserAccount.ID);
				_IsFavorite = false;
				clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => Conversion.ToString((object)x.ScreenName) == _ScreenName && x.Value).ToList().ForEach(delegate(tblUserRights x)
				{
					x.IsFavorite = false;
				});
			}
			if (!_IsFavorite)
			{
				pictureBox1.Image = Resources.star_line;
			}
			else
			{
				pictureBox1.Image = Resources.star_fill;
			}
			DashboardNew.FavouritScreens_Flag = true;
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
		this.lblScreenName = new System.Windows.Forms.Label();
		this.lblScreenDescription = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.lblScreenName.AutoSize = true;
		this.lblScreenName.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblScreenName.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblScreenName.ForeColor = System.Drawing.Color.FromArgb(14, 134, 212);
		this.lblScreenName.Location = new System.Drawing.Point(9, 1);
		this.lblScreenName.Name = "lblScreenName";
		this.lblScreenName.Size = new System.Drawing.Size(117, 16);
		this.lblScreenName.TabIndex = 0;
		this.lblScreenName.Text = "Account Reports";
		this.lblScreenDescription.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblScreenDescription.ForeColor = System.Drawing.Color.DimGray;
		this.lblScreenDescription.Location = new System.Drawing.Point(9, 19);
		this.lblScreenDescription.Name = "lblScreenDescription";
		this.lblScreenDescription.Size = new System.Drawing.Size(286, 30);
		this.lblScreenDescription.TabIndex = 1;
		this.lblScreenDescription.Text = "kjkljjjj klkjkjjlk kjlkjlk mnnn kjskdfjkjjhf nfdjfhdjkfhdsjfdkjfdjhfhjfdhjdf  hfdkjfdhfdshdfjhfdhfdhj jkhfdshfdjfdhjjdfsd jkhhlhjfsd";
		this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.pictureBox1.Image = Architecture.WinApp.Properties.Resources.star_fill;
		this.pictureBox1.Location = new System.Drawing.Point(301, 1);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(28, 25);
		this.pictureBox1.TabIndex = 2;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.Tag = "PictureBox";
		this.pictureBox1.Click += new System.EventHandler(pictureBox1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.lblScreenDescription);
		base.Controls.Add(this.lblScreenName);
		this.Cursor = System.Windows.Forms.Cursors.Default;
		base.Name = "ScreenNameDescriptionIsFavourite";
		base.Size = new System.Drawing.Size(330, 53);
		base.Load += new System.EventHandler(ScreenNameDescriptionIsFavourite_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
