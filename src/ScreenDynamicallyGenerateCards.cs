// using Attachment = global::Architecture.Model.Attachment;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Properties;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.DynamicCards
{
	public class ScreenDynamicallyGenerateCards : UserControl
	{
		private Architecture.Model.UserAccount UserAccount = new Architecture.Model.UserAccount();

		private bool _IsFavorite;

		private int _ScreenId;

		private int _ModuleId;

		private string _title;

		private Image _Icon;

		private string _Id;

		private string _ScreenName;

		private IContainer components;

		private Label MenueTitle;

		private PictureBox pictureBox1;

		private SqlCeCommandBuilder sqlCeCommandBuilder1;

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

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
				MenueTitle.Text = value;
			}
		}

		public Image Icon
		{
			get
			{
				return _Icon;
			}
			set
			{
				_Icon = value;
				pictureBox1.Image = value;
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

		[Browsable(true)]
		public event EventHandler UserControlClicked;

		public ScreenDynamicallyGenerateCards(Architecture.Model.UserAccount pUser)
		{
			InitializeComponent();
			UserAccount = pUser;
			base.Click += ControlClicked;
			MenueTitle.Click += ControlClicked;
		}

		private void ControlClicked(object sender, EventArgs e)
		{
			if (this.UserControlClicked != null)
			{
				this.UserControlClicked(this, e);
			}
		}

		private void ScreenDynamicallyGenerateCards_Load(object sender, EventArgs e)
		{
			try
			{
				if (!_IsFavorite)
				{
					pictureBox1.Image = Resources.Fav_Star_Empty;
				}
				else
				{
					pictureBox1.Image = Resources.Fav_Star_Fill;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			try
			{
				if (!_IsFavorite)
				{
					Architecture.BLL.FavoriteScreens.save(new Architecture.Model.FavoriteScreens
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						Id = 0,
						ScreenId = _ScreenId,
						ScreenName = _ScreenName,
						ScreenTitle = _title,
						TargetUrl = _Id,
						UserId = UserAccount.ID,
						ScreenRoute = ""
					});
					_IsFavorite = true;
					clsGlobalVariables.ScreenViewReights.Where((Architecture.Model.tblUserRights x) => Conversion.ToString((object)x.ScreenName) == _ScreenName && x.Value).ToList().ForEach(delegate(Architecture.Model.tblUserRights x)
					{
						x.IsFavorite = true;
					});
				}
				else
				{
					Architecture.BLL.FavoriteScreens.DeleteByScreenNameAndUserId(_ScreenName, UserAccount.ID);
					_IsFavorite = false;
					clsGlobalVariables.ScreenViewReights.Where((Architecture.Model.tblUserRights x) => Conversion.ToString((object)x.ScreenName) == _ScreenName && x.Value).ToList().ForEach(delegate(Architecture.Model.tblUserRights x)
					{
						x.IsFavorite = false;
					});
				}
				if (!_IsFavorite)
				{
					pictureBox1.Image = Resources.Fav_Star_Empty;
				}
				else
				{
					pictureBox1.Image = Resources.Fav_Star_Fill;
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
			this.MenueTitle = new System.Windows.Forms.Label();
			this.sqlCeCommandBuilder1 = new SqlCeCommandBuilder();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.MenueTitle.BackColor = System.Drawing.Color.White;
			this.MenueTitle.Cursor = System.Windows.Forms.Cursors.Hand;
			this.MenueTitle.Font = new System.Drawing.Font("Calibri", 13f, System.Drawing.FontStyle.Bold);
			this.MenueTitle.ForeColor = System.Drawing.Color.Black;
			this.MenueTitle.Location = new System.Drawing.Point(4, 34);
			this.MenueTitle.Name = "MenueTitle";
			this.MenueTitle.Size = new System.Drawing.Size(152, 75);
			this.MenueTitle.TabIndex = 3;
			this.MenueTitle.Text = "DashBoards";
			this.MenueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.sqlCeCommandBuilder1.DataAdapter = null;
			((System.Data.Common.DbCommandBuilder)(object)this.sqlCeCommandBuilder1).QuotePrefix = "[";
			((System.Data.Common.DbCommandBuilder)(object)this.sqlCeCommandBuilder1).QuoteSuffix = "]";
			this.pictureBox1.Image = Architecture.WinApp.Properties.Resources.Fav_Star_Empty;
			this.pictureBox1.Location = new System.Drawing.Point(67, 5);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(26, 26);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new System.EventHandler(pictureBox1_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.Color.White;
			base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.MenueTitle);
			this.Cursor = System.Windows.Forms.Cursors.Hand;
			this.MaximumSize = new System.Drawing.Size(200, 160);
			base.Name = "ScreenDynamicallyGenerateCards";
			base.Size = new System.Drawing.Size(159, 109);
			base.Load += new System.EventHandler(ScreenDynamicallyGenerateCards_Load);
			((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
