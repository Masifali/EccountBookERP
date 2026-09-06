// using Attachment = global::Architecture.Model.Attachment;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Windows.Forms;
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
	public class ModuleDynamicallyGenerateCards : UserControl
	{
		private string _title;

		private string _Count;

		private Image _Icon;

		private string _Id;

		private object CardTitle;

		private IContainer components;

		private Label ModuleTitle;

		private PictureBox pictureBox1;

		private SqlCeCommandBuilder sqlCeCommandBuilder1;

		private Label CardCount;

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
				ModuleTitle.Text = value;
			}
		}

		public string Count
		{
			get
			{
				return _Count;
			}
			set
			{
				_Count = value;
				CardCount.Text = value;
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

		[Browsable(true)]
		public event EventHandler UserControlClicked;

		public ModuleDynamicallyGenerateCards()
		{
			InitializeComponent();
			base.Click += ControlClicked;
			CardCount.Click += ControlClicked;
			ModuleTitle.Click += ControlClicked;
			pictureBox1.Click += ControlClicked;
		}

		private void ControlClicked(object sender, EventArgs e)
		{
			if (this.UserControlClicked != null)
			{
				this.UserControlClicked(this, e);
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
			this.ModuleTitle = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.sqlCeCommandBuilder1 = new SqlCeCommandBuilder();
			this.CardCount = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.ModuleTitle.BackColor = System.Drawing.Color.White;
			this.ModuleTitle.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ModuleTitle.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.ModuleTitle.ForeColor = System.Drawing.Color.Black;
			this.ModuleTitle.Location = new System.Drawing.Point(4, 96);
			this.ModuleTitle.Name = "ModuleTitle";
			this.ModuleTitle.Size = new System.Drawing.Size(181, 51);
			this.ModuleTitle.TabIndex = 3;
			this.ModuleTitle.Text = "DashBoards";
			this.ModuleTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.pictureBox1.Image = Architecture.WinApp.Properties.Resources.Store_Management;
			this.pictureBox1.Location = new System.Drawing.Point(81, 53);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(29, 29);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			this.sqlCeCommandBuilder1.DataAdapter = null;
			((System.Data.Common.DbCommandBuilder)(object)this.sqlCeCommandBuilder1).QuotePrefix = "[";
			((System.Data.Common.DbCommandBuilder)(object)this.sqlCeCommandBuilder1).QuoteSuffix = "]";
			this.CardCount.BackColor = System.Drawing.Color.Teal;
			this.CardCount.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CardCount.Dock = System.Windows.Forms.DockStyle.Top;
			this.CardCount.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.CardCount.ForeColor = System.Drawing.Color.White;
			this.CardCount.Location = new System.Drawing.Point(0, 0);
			this.CardCount.Name = "CardCount";
			this.CardCount.Size = new System.Drawing.Size(188, 38);
			this.CardCount.TabIndex = 6;
			this.CardCount.Text = "label1";
			this.CardCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			base.Controls.Add(this.CardCount);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.ModuleTitle);
			this.Cursor = System.Windows.Forms.Cursors.Hand;
			this.MaximumSize = new System.Drawing.Size(200, 160);
			base.Name = "ModuleDynamicallyGenerateCards";
			base.Size = new System.Drawing.Size(188, 154);
			((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
