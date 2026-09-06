using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.DynamicCards
{

public class FlowLayoutWithDescriptionAndScreens : UserControl
{
	private string _ModuleName;

	private const int CS_DROPSHADOW = 131072;

	private IContainer components;

	private Label lblModuleName;

	private Label label2;

	public FlowLayoutPanel flowLayoutPanel1;

	public Panel panel1;

	public string ModuleName
	{
		get
		{
			return _ModuleName;
		}
		set
		{
			_ModuleName = value;
			lblModuleName.Text = value;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams obj = base.CreateParams;
			obj.ClassStyle |= 131072;
			return obj;
		}
	}

	public FlowLayoutWithDescriptionAndScreens()
	{
		InitializeComponent();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.lblModuleName = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.lblModuleName);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(353, 35);
		this.panel1.TabIndex = 0;
		this.lblModuleName.AutoSize = true;
		this.lblModuleName.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModuleName.Location = new System.Drawing.Point(4, 11);
		this.lblModuleName.Name = "lblModuleName";
		this.lblModuleName.Size = new System.Drawing.Size(165, 19);
		this.lblModuleName.TabIndex = 1;
		this.lblModuleName.Text = "Module Description";
		this.label2.BackColor = System.Drawing.Color.Black;
		this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label2.Dock = System.Windows.Forms.DockStyle.Top;
		this.label2.Location = new System.Drawing.Point(0, 35);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(353, 2);
		this.label2.TabIndex = 2;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 37);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(353, 57);
		this.flowLayoutPanel1.TabIndex = 3;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.Controls.Add(this.flowLayoutPanel1);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.panel1);
		base.Name = "FlowLayoutWithDescriptionAndScreens";
		base.Size = new System.Drawing.Size(353, 96);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
