using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.DashBoard;
using Architecture.WinApp.ExportReports;
using Architecture.WinApp.Properties;
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

public class ExportDashboardHeaderInfo : UserControl
{
	public UserAccount UserAccount = new UserAccount();

	private DataTable _dt;

	private string _Caption;

	private DateTime _Fromdate;

	private DateTime _ToDate;

	private IContainer components;

	private Panel panel5;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label lblTitle;

	private Button btnMoreInfo;

	private FlowLayoutPanel FlowLayoutPanel;

	public DataTable dataTable
	{
		get
		{
			return _dt;
		}
		set
		{
			_dt = value;
		}
	}

	public string Caption
	{
		get
		{
			return _Caption;
		}
		set
		{
			_Caption = value;
		}
	}

	public DateTime FromDate
	{
		get
		{
			return _Fromdate;
		}
		set
		{
			_Fromdate = value;
		}
	}

	public DateTime ToDate
	{
		get
		{
			return _ToDate;
		}
		set
		{
			_ToDate = value;
		}
	}

	public ExportDashboardHeaderInfo(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void ExportDashboardHeaderInfo_Load(object sender, EventArgs e)
	{
		try
		{
			FlowLayoutPanel.Controls.Clear();
			if (dataTable.Rows.Count > 0)
			{
				Caption = Regex.Replace(Conversion.ToString(dataTable.Rows[0]["CaptionDescription"]), "[A-Z]", " $0").Trim();
				lblTitle.Text = Caption;
				ExportDashboradInfoCards[] CardsList = new ExportDashboradInfoCards[dataTable.Rows.Count];
				for (int i = 0; i < CardsList.Length; i++)
				{
					CardsList[i] = new ExportDashboradInfoCards();
					double Mtons = Conversion.ToDouble(dataTable.Rows[i]["Contract_Mtons"]);
					CardsList[i].Mtons = Mtons.ToString("#,#.##;(#,#);0");
					CardsList[i].FcyCode = Conversion.ToString(dataTable.Rows[i]["FcyCode"]);
					double Rate = Conversion.ToDouble(dataTable.Rows[i]["Fcy_Rate"]);
					CardsList[i].FcyRate = Rate.ToString("#,#.##;(#,#);0");
					double FcyAmount = Conversion.ToDouble(dataTable.Rows[i]["Fcy_Amount"]);
					CardsList[i].FcyAmount = FcyAmount.ToString("#,#.##;(#,#);0");
					double pkrAmount = Conversion.ToDouble(dataTable.Rows[i]["PKR_AmountTotal"]);
					CardsList[i].PkrAmount = pkrAmount.ToString("#,#;(#,#);0");
					FlowLayoutPanel.Controls.Add(CardsList[i]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnMoreInfo_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToString(dataTable.Rows[0]["CaptionDescription"]) == "ExportContractsTotal")
			{
				ExImSaleContractRegister exImSaleContractRegister = new ExImSaleContractRegister(UserAccount);
				exImSaleContractRegister.cmbperemeter.Value = 5;
				exImSaleContractRegister.Show();
			}
			else if (Conversion.ToString(dataTable.Rows[0]["CaptionDescription"]) == "OutstandingExportContracts")
			{
				ExImSaleContractRegister exImSaleContractRegister2 = new ExImSaleContractRegister(UserAccount);
				exImSaleContractRegister2.cmbperemeter.Value = 5;
				exImSaleContractRegister2.Show();
			}
			else if (Conversion.ToString(dataTable.Rows[0]["CaptionDescription"]) == "Total Export")
			{
				ExImCommercialInvoiceHistory exImCommercialInvoiceHistory = new ExImCommercialInvoiceHistory(UserAccount);
				exImCommercialInvoiceHistory.cmbDateType.Value = 5;
				exImCommercialInvoiceHistory.tabControl1.SelectedIndex = 0;
				exImCommercialInvoiceHistory.Show();
			}
			else if (Conversion.ToString(dataTable.Rows[0]["CaptionDescription"]) == "TotalExportByCurrentMonth")
			{
				ExImCommercialInvoiceHistory exImCommercialInvoiceHistory2 = new ExImCommercialInvoiceHistory(UserAccount);
				exImCommercialInvoiceHistory2.cmbDateType.Value = 3;
				exImCommercialInvoiceHistory2.tabControl1.SelectedIndex = 0;
				exImCommercialInvoiceHistory2.Show();
			}
			else if (Conversion.ToString(dataTable.Rows[0]["CaptionDescription"]) == "Forwarding In Process")
			{
				ExportDashboardPopUp exportDashboardPopUp = new ExportDashboardPopUp(UserAccount);
				exportDashboardPopUp.Activity = "ForwardingInProcessDetailReport";
				exportDashboardPopUp.FromDate = FromDate;
				exportDashboardPopUp.ToDate = ToDate;
				exportDashboardPopUp.Show();
			}
			else if (Conversion.ToString(dataTable.Rows[0]["CaptionDescription"]) == "Forwarded But B/L Not Received")
			{
				ExportDashboardPopUp exportDashboardPopUp2 = new ExportDashboardPopUp(UserAccount);
				exportDashboardPopUp2.Activity = "ForwardedButBillofLadingNotReceivedDetailReport";
				exportDashboardPopUp2.FromDate = FromDate;
				exportDashboardPopUp2.ToDate = ToDate;
				exportDashboardPopUp2.Show();
			}
			else if (Conversion.ToString(dataTable.Rows[0]["CaptionDescription"]) == "Payment Not Received")
			{
				ExportDashboardPopUp exportDashboardPopUp3 = new ExportDashboardPopUp(UserAccount);
				exportDashboardPopUp3.Activity = "BlReceivedButPaymentNotReceived";
				exportDashboardPopUp3.FromDate = FromDate;
				exportDashboardPopUp3.ToDate = ToDate;
				exportDashboardPopUp3.Show();
			}
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
		this.panel5 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.lblTitle = new System.Windows.Forms.Label();
		this.FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.btnMoreInfo = new System.Windows.Forms.Button();
		this.panel5.SuspendLayout();
		base.SuspendLayout();
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 31);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(551, 37);
		this.panel5.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.8f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.Teal;
		this.label2.Location = new System.Drawing.Point(424, 9);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 19);
		this.label2.TabIndex = 10;
		this.label2.Text = "PKR Amount";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.8f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.Teal;
		this.label3.Location = new System.Drawing.Point(13, 9);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(56, 19);
		this.label3.TabIndex = 6;
		this.label3.Text = "M Tons";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.8f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.Teal;
		this.label4.Location = new System.Drawing.Point(304, 9);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(88, 19);
		this.label4.TabIndex = 9;
		this.label4.Text = "Fcy Amount";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.8f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.Teal;
		this.label5.Location = new System.Drawing.Point(195, 9);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(65, 19);
		this.label5.TabIndex = 8;
		this.label5.Text = "Fcy Rate";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.8f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.Teal;
		this.label6.Location = new System.Drawing.Point(119, 9);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(31, 19);
		this.label6.TabIndex = 7;
		this.label6.Text = "Fcy";
		this.lblTitle.BackColor = System.Drawing.Color.Teal;
		this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.ForeColor = System.Drawing.Color.White;
		this.lblTitle.Location = new System.Drawing.Point(0, 0);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(551, 31);
		this.lblTitle.TabIndex = 0;
		this.lblTitle.Text = "Title";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.FlowLayoutPanel.AutoScroll = true;
		this.FlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.FlowLayoutPanel.Location = new System.Drawing.Point(0, 68);
		this.FlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
		this.FlowLayoutPanel.Name = "FlowLayoutPanel";
		this.FlowLayoutPanel.Size = new System.Drawing.Size(551, 98);
		this.FlowLayoutPanel.TabIndex = 5;
		this.btnMoreInfo.BackColor = System.Drawing.Color.White;
		this.btnMoreInfo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnMoreInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnMoreInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnMoreInfo.Image = Architecture.WinApp.Properties.Resources.arrow_right_circle_fill;
		this.btnMoreInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnMoreInfo.Location = new System.Drawing.Point(0, 166);
		this.btnMoreInfo.Name = "btnMoreInfo";
		this.btnMoreInfo.Size = new System.Drawing.Size(551, 37);
		this.btnMoreInfo.TabIndex = 4;
		this.btnMoreInfo.Text = "More Info";
		this.btnMoreInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnMoreInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
		this.btnMoreInfo.UseVisualStyleBackColor = false;
		this.btnMoreInfo.Click += new System.EventHandler(btnMoreInfo_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.FlowLayoutPanel);
		base.Controls.Add(this.btnMoreInfo);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.lblTitle);
		base.Name = "ExportDashboardHeaderInfo";
		base.Size = new System.Drawing.Size(551, 203);
		base.Load += new System.EventHandler(ExportDashboardHeaderInfo_Load);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
