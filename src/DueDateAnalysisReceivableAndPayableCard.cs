using System;
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

public class DueDateAnalysisReceivableAndPayableCard : UserControl
{
	private string _DateLable;

	private string _SaleReceivables;

	private string _ExportReceivables;

	private string _TotalReceivables;

	private string _TotalRunningReceivables;

	private string _PurchasePayables;

	private string _ImportPayables;

	private string _TotalPayables;

	private string _TotalRunningPayables;

	private string _ReceivablesMinusPayables;

	private string _RunningReceivables;

	private IContainer components;

	private Label lblDate;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label lblSale;

	private Label lblExport;

	private Label lblTotalReceiables;

	private Label lblTotalRunningReceiables;

	private Label label1;

	private Label label6;

	private Label label7;

	private Label label8;

	private Label lblPurchasePayable;

	private Label lblImportPayable;

	private Label lblTotalPayables;

	private Label lblRunningTotalPayables;

	private Label label9;

	private Label lblReceivablesMinusPayables;

	private Label label11;

	private Label lblRunningReceivables;

	public string Date
	{
		get
		{
			return _DateLable;
		}
		set
		{
			_DateLable = value;
			lblDate.Text = value;
		}
	}

	public string SaleReceivables
	{
		get
		{
			return _SaleReceivables;
		}
		set
		{
			_SaleReceivables = value;
			lblSale.Text = value;
		}
	}

	public string ExportReceivables
	{
		get
		{
			return _ExportReceivables;
		}
		set
		{
			_ExportReceivables = value;
			lblExport.Text = value;
		}
	}

	public string TotalReceivables
	{
		get
		{
			return _TotalReceivables;
		}
		set
		{
			_TotalReceivables = value;
			lblTotalReceiables.Text = value;
		}
	}

	public string TotalRunningReceivables
	{
		get
		{
			return _TotalRunningReceivables;
		}
		set
		{
			_TotalRunningReceivables = value;
			lblTotalRunningReceiables.Text = value;
		}
	}

	public string PurchasePayables
	{
		get
		{
			return _PurchasePayables;
		}
		set
		{
			_PurchasePayables = value;
			lblPurchasePayable.Text = value;
		}
	}

	public string ImportPayables
	{
		get
		{
			return _ImportPayables;
		}
		set
		{
			_ImportPayables = value;
			lblImportPayable.Text = value;
		}
	}

	public string TotalPayables
	{
		get
		{
			return _TotalPayables;
		}
		set
		{
			_TotalPayables = value;
			lblTotalPayables.Text = value;
		}
	}

	public string TotalRunningPayables
	{
		get
		{
			return _TotalRunningPayables;
		}
		set
		{
			_TotalRunningPayables = value;
			lblRunningTotalPayables.Text = value;
		}
	}

	public string ReceivablesMinusPayables
	{
		get
		{
			return _ReceivablesMinusPayables;
		}
		set
		{
			_ReceivablesMinusPayables = value;
			lblReceivablesMinusPayables.Text = value;
		}
	}

	public string RunningReceivables
	{
		get
		{
			return _RunningReceivables;
		}
		set
		{
			_RunningReceivables = value;
			lblRunningReceivables.Text = value;
		}
	}

	public DueDateAnalysisReceivableAndPayableCard()
	{
		InitializeComponent();
	}

	private void DueDateAnalysisReceivableAndPayableCard_Load(object sender, EventArgs e)
	{
		if (RunningReceivables.Contains("(") && RunningReceivables.Contains(")"))
		{
			lblRunningReceivables.ForeColor = Color.Red;
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
		this.lblDate = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.lblSale = new System.Windows.Forms.Label();
		this.lblExport = new System.Windows.Forms.Label();
		this.lblTotalReceiables = new System.Windows.Forms.Label();
		this.lblTotalRunningReceiables = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.lblPurchasePayable = new System.Windows.Forms.Label();
		this.lblImportPayable = new System.Windows.Forms.Label();
		this.lblTotalPayables = new System.Windows.Forms.Label();
		this.lblRunningTotalPayables = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.lblReceivablesMinusPayables = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.lblRunningReceivables = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblDate.BackColor = System.Drawing.Color.Teal;
		this.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblDate.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblDate.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDate.ForeColor = System.Drawing.Color.White;
		this.lblDate.Location = new System.Drawing.Point(0, 0);
		this.lblDate.Name = "lblDate";
		this.lblDate.Size = new System.Drawing.Size(266, 24);
		this.lblDate.TabIndex = 0;
		this.lblDate.Text = "Date";
		this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(3, 29);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(100, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Sale Receivable :";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 47);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(114, 15);
		this.label3.TabIndex = 2;
		this.label3.Text = "Export Receivable :";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(3, 65);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(109, 15);
		this.label4.TabIndex = 3;
		this.label4.Text = "Total Receivables :";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(3, 83);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(145, 15);
		this.label5.TabIndex = 4;
		this.label5.Text = "Cumulative Receivables :";
		this.lblSale.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSale.Location = new System.Drawing.Point(142, 27);
		this.lblSale.Name = "lblSale";
		this.lblSale.Size = new System.Drawing.Size(118, 18);
		this.lblSale.TabIndex = 6;
		this.lblSale.Text = "0";
		this.lblSale.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblExport.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblExport.Location = new System.Drawing.Point(142, 45);
		this.lblExport.Name = "lblExport";
		this.lblExport.Size = new System.Drawing.Size(118, 18);
		this.lblExport.TabIndex = 7;
		this.lblExport.Text = "0";
		this.lblExport.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblTotalReceiables.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalReceiables.Location = new System.Drawing.Point(142, 63);
		this.lblTotalReceiables.Name = "lblTotalReceiables";
		this.lblTotalReceiables.Size = new System.Drawing.Size(118, 18);
		this.lblTotalReceiables.TabIndex = 8;
		this.lblTotalReceiables.Text = "0";
		this.lblTotalReceiables.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblTotalRunningReceiables.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalRunningReceiables.Location = new System.Drawing.Point(142, 81);
		this.lblTotalRunningReceiables.Name = "lblTotalRunningReceiables";
		this.lblTotalRunningReceiables.Size = new System.Drawing.Size(118, 18);
		this.lblTotalRunningReceiables.TabIndex = 9;
		this.lblTotalRunningReceiables.Text = "0";
		this.lblTotalRunningReceiables.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 101);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(108, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "Purchase Payable :";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(3, 119);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(97, 15);
		this.label6.TabIndex = 2;
		this.label6.Text = "Import Payable :";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(3, 137);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(90, 15);
		this.label7.TabIndex = 3;
		this.label7.Text = "Total Payables :";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(3, 155);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(126, 15);
		this.label8.TabIndex = 4;
		this.label8.Text = "Cumulative Payables :";
		this.lblPurchasePayable.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPurchasePayable.Location = new System.Drawing.Point(142, 99);
		this.lblPurchasePayable.Name = "lblPurchasePayable";
		this.lblPurchasePayable.Size = new System.Drawing.Size(118, 18);
		this.lblPurchasePayable.TabIndex = 6;
		this.lblPurchasePayable.Text = "0";
		this.lblPurchasePayable.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblImportPayable.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblImportPayable.Location = new System.Drawing.Point(142, 117);
		this.lblImportPayable.Name = "lblImportPayable";
		this.lblImportPayable.Size = new System.Drawing.Size(118, 18);
		this.lblImportPayable.TabIndex = 7;
		this.lblImportPayable.Text = "0";
		this.lblImportPayable.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblTotalPayables.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalPayables.Location = new System.Drawing.Point(142, 135);
		this.lblTotalPayables.Name = "lblTotalPayables";
		this.lblTotalPayables.Size = new System.Drawing.Size(118, 18);
		this.lblTotalPayables.TabIndex = 8;
		this.lblTotalPayables.Text = "0";
		this.lblTotalPayables.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblRunningTotalPayables.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRunningTotalPayables.Location = new System.Drawing.Point(142, 153);
		this.lblRunningTotalPayables.Name = "lblRunningTotalPayables";
		this.lblRunningTotalPayables.Size = new System.Drawing.Size(118, 18);
		this.lblRunningTotalPayables.TabIndex = 9;
		this.lblRunningTotalPayables.Text = "0";
		this.lblRunningTotalPayables.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(3, 171);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(139, 30);
		this.label9.TabIndex = 4;
		this.label9.Text = "Net Balance (Receivables - Payables) :";
		this.lblReceivablesMinusPayables.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblReceivablesMinusPayables.Location = new System.Drawing.Point(142, 177);
		this.lblReceivablesMinusPayables.Name = "lblReceivablesMinusPayables";
		this.lblReceivablesMinusPayables.Size = new System.Drawing.Size(118, 18);
		this.lblReceivablesMinusPayables.TabIndex = 9;
		this.lblReceivablesMinusPayables.Text = "0";
		this.lblReceivablesMinusPayables.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(3, 202);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(119, 15);
		this.label11.TabIndex = 4;
		this.label11.Text = "Cumulative Balance:";
		this.lblRunningReceivables.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRunningReceivables.Location = new System.Drawing.Point(141, 200);
		this.lblRunningReceivables.Name = "lblRunningReceivables";
		this.lblRunningReceivables.Size = new System.Drawing.Size(119, 18);
		this.lblRunningReceivables.TabIndex = 9;
		this.lblRunningReceivables.Text = "0";
		this.lblRunningReceivables.TextAlign = System.Drawing.ContentAlignment.TopRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.lblRunningReceivables);
		base.Controls.Add(this.lblReceivablesMinusPayables);
		base.Controls.Add(this.lblRunningTotalPayables);
		base.Controls.Add(this.lblTotalRunningReceiables);
		base.Controls.Add(this.lblTotalPayables);
		base.Controls.Add(this.lblTotalReceiables);
		base.Controls.Add(this.lblImportPayable);
		base.Controls.Add(this.lblExport);
		base.Controls.Add(this.lblPurchasePayable);
		base.Controls.Add(this.label11);
		base.Controls.Add(this.label9);
		base.Controls.Add(this.label8);
		base.Controls.Add(this.lblSale);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.lblDate);
		base.Name = "DueDateAnalysisReceivableAndPayableCard";
		base.Size = new System.Drawing.Size(266, 221);
		base.Load += new System.EventHandler(DueDateAnalysisReceivableAndPayableCard_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
