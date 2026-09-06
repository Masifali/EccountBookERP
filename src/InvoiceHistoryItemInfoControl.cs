using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.WinApp.Common;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.ModernHistories
{

public class InvoiceHistoryItemInfoControl : UserControl
{
	private int _InvoiceType;

	private int _DocumentTypeId;

	private int _ItemId;

	private DateTime _InvoiceDate;

	private int _ItemAccountId;

	private int _Gdn_GrnId;

	private int _GpId;

	private string _CaptionGdnGrn;

	private string _ItemName;

	private string _WareHouse;

	private string _Qty;

	private string _Uom;

	private string _NetWeight;

	private string _NetWeight1;

	private string _Rate;

	private string _ItemAmount;

	private string _ItemAmount1;

	private string _Grn_GdnNo;

	private string _GpNo;

	private string _JobLot;

	private string _PackingType;

	private string _CropYear;

	private string _BagWeight;

	private string _GrossWeight;

	private string _StockWeight;

	private string _RateUom;

	private string _WeightCut;

	private string _RateCut;

	private string _FreightAmount;

	private string _ExpenseAmount;

	private string _ItemNetAmount;

	private IContainer components;

	private Panel panel12;

	private Label WareHouse;

	private Label ItemAmount;

	private Button ShowHidButton;

	private Label Rate;

	private Label ItemName;

	private Label NetWeight;

	private Label Quantity;

	private Label Uom;

	private Label CaptionGdnGrn;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label GrnGdnNo;

	private Label GatePassNo;

	private Label JobLot;

	private Label CropYear;

	private Label PackingType;

	private Label BagWeight;

	private Label label9;

	private Label RateCut;

	private Label label11;

	private Label WeightCut;

	private Label RateUom;

	private Label StockWeight;

	private Label NetWeight1;

	private Label GrossWeight;

	private Label label22;

	private Label label23;

	private Label label24;

	private Label label25;

	private Label label26;

	private Panel DetailPanel;

	private Label ItemNetAmount;

	private Label ExpenseAmount;

	private Label FreightAmount;

	private Label ItemAmount1;

	private Label label35;

	private Label label36;

	private Label label37;

	private Label label38;

	public int InvoiceType
	{
		get
		{
			return _InvoiceType;
		}
		set
		{
			_InvoiceType = value;
		}
	}

	public int DocumentTypeId
	{
		get
		{
			return _DocumentTypeId;
		}
		set
		{
			_DocumentTypeId = value;
		}
	}

	public int ItemId
	{
		get
		{
			return _ItemId;
		}
		set
		{
			_ItemId = value;
		}
	}

	public DateTime InvoiceDate
	{
		get
		{
			return _InvoiceDate;
		}
		set
		{
			_InvoiceDate = value;
		}
	}

	public int ItemAccountId
	{
		get
		{
			return _ItemAccountId;
		}
		set
		{
			_ItemAccountId = value;
		}
	}

	public int Gdn_GrnId
	{
		get
		{
			return _Gdn_GrnId;
		}
		set
		{
			_Gdn_GrnId = value;
		}
	}

	public int GpId
	{
		get
		{
			return _GpId;
		}
		set
		{
			_GpId = value;
		}
	}

	public string captionGdnGrn
	{
		get
		{
			return _CaptionGdnGrn;
		}
		set
		{
			_CaptionGdnGrn = value;
			CaptionGdnGrn.Text = value;
		}
	}

	public string itemName
	{
		get
		{
			return _ItemName;
		}
		set
		{
			_ItemName = value;
			ItemName.Text = value;
		}
	}

	public string Warehouse
	{
		get
		{
			return _WareHouse;
		}
		set
		{
			_WareHouse = value;
			WareHouse.Text = value;
		}
	}

	public string Qty
	{
		get
		{
			return _Qty;
		}
		set
		{
			_Qty = value;
			Quantity.Text = value;
		}
	}

	public string uom
	{
		get
		{
			return _Uom;
		}
		set
		{
			_Uom = value;
			Uom.Text = value;
		}
	}

	public string NtWeight
	{
		get
		{
			return _NetWeight;
		}
		set
		{
			_NetWeight = value;
			NetWeight.Text = value;
		}
	}

	public string NtWeight1
	{
		get
		{
			return _NetWeight1;
		}
		set
		{
			_NetWeight1 = value;
			NetWeight1.Text = value;
		}
	}

	public string rate
	{
		get
		{
			return _Rate;
		}
		set
		{
			_Rate = value;
			Rate.Text = value;
		}
	}

	public string itemAmount
	{
		get
		{
			return _ItemAmount;
		}
		set
		{
			_ItemAmount = value;
			ItemAmount.Text = value;
		}
	}

	public string itemAmount1
	{
		get
		{
			return _ItemAmount1;
		}
		set
		{
			_ItemAmount1 = value;
			ItemAmount1.Text = value;
		}
	}

	public string grnGdnNo
	{
		get
		{
			return _Grn_GdnNo;
		}
		set
		{
			_Grn_GdnNo = value;
			GrnGdnNo.Text = value;
		}
	}

	public string GpNo
	{
		get
		{
			return _GpNo;
		}
		set
		{
			_GpNo = value;
			GatePassNo.Text = value;
		}
	}

	public string jobLot
	{
		get
		{
			return _JobLot;
		}
		set
		{
			_JobLot = value;
			JobLot.Text = value;
		}
	}

	public string packingType
	{
		get
		{
			return _PackingType;
		}
		set
		{
			_PackingType = value;
			PackingType.Text = value;
		}
	}

	public string cropYear
	{
		get
		{
			return _CropYear;
		}
		set
		{
			_CropYear = value;
			CropYear.Text = value;
		}
	}

	public string bagWeight
	{
		get
		{
			return _BagWeight;
		}
		set
		{
			_BagWeight = value;
			BagWeight.Text = value;
		}
	}

	public string grossWeight
	{
		get
		{
			return _GrossWeight;
		}
		set
		{
			_GrossWeight = value;
			GrossWeight.Text = value;
		}
	}

	public string stockWeight
	{
		get
		{
			return _StockWeight;
		}
		set
		{
			_StockWeight = value;
			StockWeight.Text = value;
		}
	}

	public string rateUom
	{
		get
		{
			return _RateUom;
		}
		set
		{
			_RateUom = value;
			RateUom.Text = value;
		}
	}

	public string weightCut
	{
		get
		{
			return _WeightCut;
		}
		set
		{
			_WeightCut = value;
			WeightCut.Text = value;
		}
	}

	public string rateCut
	{
		get
		{
			return _RateCut;
		}
		set
		{
			_RateCut = value;
			RateCut.Text = value;
		}
	}

	public string freightAmount
	{
		get
		{
			return _FreightAmount;
		}
		set
		{
			_FreightAmount = value;
			FreightAmount.Text = value;
		}
	}

	public string expenseAmount
	{
		get
		{
			return _ExpenseAmount;
		}
		set
		{
			_ExpenseAmount = value;
			ExpenseAmount.Text = value;
		}
	}

	public string itemNetAmount
	{
		get
		{
			return _ItemNetAmount;
		}
		set
		{
			_ItemNetAmount = value;
			ItemNetAmount.Text = value;
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public InvoiceHistoryItemInfoControl()
	{
		InitializeComponent();
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void ShowHidButton_Click(object sender, EventArgs e)
	{
		try
		{
			if (ShowHidButton.Text == "+")
			{
				DetailPanel.Show();
				ShowHidButton.Text = "-";
				base.Height = 101;
			}
			else
			{
				DetailPanel.Hide();
				ShowHidButton.Text = "+";
				base.Height = 35;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseInvoiceHistoryControl_Load(object sender, EventArgs e)
	{
		try
		{
			ShowHidButton.Text = "+";
			DetailPanel.Hide();
			base.Height = 35;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrnGdnNo_Click(object sender, EventArgs e)
	{
		try
		{
			if (InvoiceType == 1)
			{
				CommonServices.GrnSlipWithSubReports(Gdn_GrnId);
			}
			else
			{
				CommonServices.InvGdnSlip260(Gdn_GrnId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassNo_Click(object sender, EventArgs e)
	{
		try
		{
			if (InvoiceType == 1)
			{
				CommonServices.GatePassInwardSlipAndRegisterReport(GpId);
			}
			else
			{
				CommonServices.GatePassOutwardSlipandRegister(GpId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemName_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(ItemId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
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
		this.panel12 = new System.Windows.Forms.Panel();
		this.WareHouse = new System.Windows.Forms.Label();
		this.ItemAmount = new System.Windows.Forms.Label();
		this.ShowHidButton = new System.Windows.Forms.Button();
		this.Rate = new System.Windows.Forms.Label();
		this.ItemName = new System.Windows.Forms.Label();
		this.NetWeight = new System.Windows.Forms.Label();
		this.Quantity = new System.Windows.Forms.Label();
		this.Uom = new System.Windows.Forms.Label();
		this.CaptionGdnGrn = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.GrnGdnNo = new System.Windows.Forms.Label();
		this.GatePassNo = new System.Windows.Forms.Label();
		this.JobLot = new System.Windows.Forms.Label();
		this.CropYear = new System.Windows.Forms.Label();
		this.PackingType = new System.Windows.Forms.Label();
		this.BagWeight = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.RateCut = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.WeightCut = new System.Windows.Forms.Label();
		this.RateUom = new System.Windows.Forms.Label();
		this.StockWeight = new System.Windows.Forms.Label();
		this.NetWeight1 = new System.Windows.Forms.Label();
		this.GrossWeight = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.DetailPanel = new System.Windows.Forms.Panel();
		this.ItemNetAmount = new System.Windows.Forms.Label();
		this.ExpenseAmount = new System.Windows.Forms.Label();
		this.FreightAmount = new System.Windows.Forms.Label();
		this.ItemAmount1 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.panel12.SuspendLayout();
		this.DetailPanel.SuspendLayout();
		base.SuspendLayout();
		this.panel12.BackColor = System.Drawing.Color.White;
		this.panel12.Controls.Add(this.WareHouse);
		this.panel12.Controls.Add(this.ItemAmount);
		this.panel12.Controls.Add(this.ShowHidButton);
		this.panel12.Controls.Add(this.Rate);
		this.panel12.Controls.Add(this.ItemName);
		this.panel12.Controls.Add(this.NetWeight);
		this.panel12.Controls.Add(this.Quantity);
		this.panel12.Controls.Add(this.Uom);
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(958, 35);
		this.panel12.TabIndex = 9;
		this.WareHouse.AutoEllipsis = true;
		this.WareHouse.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.WareHouse.ForeColor = System.Drawing.Color.Black;
		this.WareHouse.Location = new System.Drawing.Point(358, 7);
		this.WareHouse.Name = "WareHouse";
		this.WareHouse.Size = new System.Drawing.Size(145, 20);
		this.WareHouse.TabIndex = 2;
		this.WareHouse.Text = "Warehouse";
		this.ItemAmount.AutoSize = true;
		this.ItemAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ItemAmount.ForeColor = System.Drawing.Color.Black;
		this.ItemAmount.Location = new System.Drawing.Point(866, 7);
		this.ItemAmount.Name = "ItemAmount";
		this.ItemAmount.Size = new System.Drawing.Size(64, 20);
		this.ItemAmount.TabIndex = 7;
		this.ItemAmount.Text = "Amount";
		this.ShowHidButton.BackColor = System.Drawing.Color.Teal;
		this.ShowHidButton.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ShowHidButton.ForeColor = System.Drawing.Color.White;
		this.ShowHidButton.Location = new System.Drawing.Point(7, 4);
		this.ShowHidButton.Name = "ShowHidButton";
		this.ShowHidButton.Size = new System.Drawing.Size(25, 24);
		this.ShowHidButton.TabIndex = 0;
		this.ShowHidButton.Text = "+";
		this.ShowHidButton.UseVisualStyleBackColor = false;
		this.ShowHidButton.Click += new System.EventHandler(ShowHidButton_Click);
		this.Rate.AutoSize = true;
		this.Rate.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Rate.ForeColor = System.Drawing.Color.Black;
		this.Rate.Location = new System.Drawing.Point(793, 7);
		this.Rate.Name = "Rate";
		this.Rate.Size = new System.Drawing.Size(39, 20);
		this.Rate.TabIndex = 6;
		this.Rate.Text = "Rate";
		this.ItemName.AutoEllipsis = true;
		this.ItemName.AutoSize = true;
		this.ItemName.Cursor = System.Windows.Forms.Cursors.Hand;
		this.ItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ItemName.ForeColor = System.Drawing.Color.Blue;
		this.ItemName.Location = new System.Drawing.Point(47, 7);
		this.ItemName.Name = "ItemName";
		this.ItemName.Size = new System.Drawing.Size(84, 20);
		this.ItemName.TabIndex = 1;
		this.ItemName.Text = "Item Name";
		this.ItemName.Click += new System.EventHandler(ItemName_Click);
		this.NetWeight.AutoSize = true;
		this.NetWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.NetWeight.ForeColor = System.Drawing.Color.Black;
		this.NetWeight.Location = new System.Drawing.Point(671, 7);
		this.NetWeight.Name = "NetWeight";
		this.NetWeight.Size = new System.Drawing.Size(87, 20);
		this.NetWeight.TabIndex = 5;
		this.NetWeight.Text = "Net Weight";
		this.Quantity.AutoSize = true;
		this.Quantity.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Quantity.ForeColor = System.Drawing.Color.Black;
		this.Quantity.Location = new System.Drawing.Point(524, 7);
		this.Quantity.Name = "Quantity";
		this.Quantity.Size = new System.Drawing.Size(33, 20);
		this.Quantity.TabIndex = 3;
		this.Quantity.Text = "Qty";
		this.Uom.AutoSize = true;
		this.Uom.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Uom.ForeColor = System.Drawing.Color.Black;
		this.Uom.Location = new System.Drawing.Point(596, 7);
		this.Uom.Name = "Uom";
		this.Uom.Size = new System.Drawing.Size(42, 20);
		this.Uom.TabIndex = 4;
		this.Uom.Text = "Uom";
		this.CaptionGdnGrn.AutoSize = true;
		this.CaptionGdnGrn.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CaptionGdnGrn.ForeColor = System.Drawing.Color.Teal;
		this.CaptionGdnGrn.Location = new System.Drawing.Point(12, 3);
		this.CaptionGdnGrn.Name = "CaptionGdnGrn";
		this.CaptionGdnGrn.Size = new System.Drawing.Size(43, 15);
		this.CaptionGdnGrn.TabIndex = 10;
		this.CaptionGdnGrn.Text = "GRN #";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Teal;
		this.label3.Location = new System.Drawing.Point(167, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(33, 15);
		this.label3.TabIndex = 11;
		this.label3.Text = "GP #";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Teal;
		this.label4.Location = new System.Drawing.Point(332, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(52, 15);
		this.label4.TabIndex = 12;
		this.label4.Text = "Job/Lot:";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Teal;
		this.label5.Location = new System.Drawing.Point(507, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(63, 15);
		this.label5.TabIndex = 13;
		this.label5.Text = "Crop Year:";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Teal;
		this.label6.Location = new System.Drawing.Point(642, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(82, 15);
		this.label6.TabIndex = 14;
		this.label6.Text = "Packing Type:";
		this.GrnGdnNo.AutoSize = true;
		this.GrnGdnNo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.GrnGdnNo.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GrnGdnNo.ForeColor = System.Drawing.Color.Blue;
		this.GrnGdnNo.Location = new System.Drawing.Point(56, 3);
		this.GrnGdnNo.Name = "GrnGdnNo";
		this.GrnGdnNo.Size = new System.Drawing.Size(21, 15);
		this.GrnGdnNo.TabIndex = 15;
		this.GrnGdnNo.Text = "12";
		this.GrnGdnNo.Click += new System.EventHandler(GrnGdnNo_Click);
		this.GatePassNo.AutoSize = true;
		this.GatePassNo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.GatePassNo.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GatePassNo.ForeColor = System.Drawing.Color.Blue;
		this.GatePassNo.Location = new System.Drawing.Point(200, 3);
		this.GatePassNo.Name = "GatePassNo";
		this.GatePassNo.Size = new System.Drawing.Size(21, 15);
		this.GatePassNo.TabIndex = 16;
		this.GatePassNo.Text = "12";
		this.GatePassNo.Click += new System.EventHandler(GatePassNo_Click);
		this.JobLot.AutoSize = true;
		this.JobLot.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.JobLot.Location = new System.Drawing.Point(384, 3);
		this.JobLot.Name = "JobLot";
		this.JobLot.Size = new System.Drawing.Size(51, 15);
		this.JobLot.TabIndex = 17;
		this.JobLot.Text = "General";
		this.CropYear.AutoSize = true;
		this.CropYear.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CropYear.Location = new System.Drawing.Point(569, 3);
		this.CropYear.Name = "CropYear";
		this.CropYear.Size = new System.Drawing.Size(68, 15);
		this.CropYear.TabIndex = 18;
		this.CropYear.Text = "2022-2023";
		this.PackingType.AutoSize = true;
		this.PackingType.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.PackingType.Location = new System.Drawing.Point(720, 3);
		this.PackingType.Name = "PackingType";
		this.PackingType.Size = new System.Drawing.Size(60, 15);
		this.PackingType.TabIndex = 19;
		this.PackingType.Text = "Jute Bags";
		this.BagWeight.AutoSize = true;
		this.BagWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BagWeight.Location = new System.Drawing.Point(883, 3);
		this.BagWeight.Name = "BagWeight";
		this.BagWeight.Size = new System.Drawing.Size(14, 15);
		this.BagWeight.TabIndex = 21;
		this.BagWeight.Text = "1";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Teal;
		this.label9.Location = new System.Drawing.Point(806, 3);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(75, 15);
		this.label9.TabIndex = 20;
		this.label9.Text = "Bag Weight:";
		this.RateCut.AutoSize = true;
		this.RateCut.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RateCut.Location = new System.Drawing.Point(866, 24);
		this.RateCut.Name = "RateCut";
		this.RateCut.Size = new System.Drawing.Size(14, 15);
		this.RateCut.TabIndex = 33;
		this.RateCut.Text = "1";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.Teal;
		this.label11.Location = new System.Drawing.Point(806, 24);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(58, 15);
		this.label11.TabIndex = 32;
		this.label11.Text = "Rate Cut:";
		this.WeightCut.AutoSize = true;
		this.WeightCut.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.WeightCut.Location = new System.Drawing.Point(719, 24);
		this.WeightCut.Name = "WeightCut";
		this.WeightCut.Size = new System.Drawing.Size(38, 15);
		this.WeightCut.TabIndex = 31;
		this.WeightCut.Text = "1,121";
		this.RateUom.AutoSize = true;
		this.RateUom.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RateUom.Location = new System.Drawing.Point(565, 24);
		this.RateUom.Name = "RateUom";
		this.RateUom.Size = new System.Drawing.Size(35, 15);
		this.RateUom.TabIndex = 30;
		this.RateUom.Text = "40kg";
		this.StockWeight.AutoSize = true;
		this.StockWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.StockWeight.Location = new System.Drawing.Point(416, 24);
		this.StockWeight.Name = "StockWeight";
		this.StockWeight.Size = new System.Drawing.Size(14, 15);
		this.StockWeight.TabIndex = 29;
		this.StockWeight.Text = "1";
		this.NetWeight1.AutoSize = true;
		this.NetWeight1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.NetWeight1.ForeColor = System.Drawing.Color.Black;
		this.NetWeight1.Location = new System.Drawing.Point(241, 24);
		this.NetWeight1.Name = "NetWeight1";
		this.NetWeight1.Size = new System.Drawing.Size(14, 15);
		this.NetWeight1.TabIndex = 28;
		this.NetWeight1.Text = "1";
		this.GrossWeight.AutoSize = true;
		this.GrossWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GrossWeight.ForeColor = System.Drawing.Color.Black;
		this.GrossWeight.Location = new System.Drawing.Point(90, 24);
		this.GrossWeight.Name = "GrossWeight";
		this.GrossWeight.Size = new System.Drawing.Size(14, 15);
		this.GrossWeight.TabIndex = 27;
		this.GrossWeight.Text = "1";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Teal;
		this.label22.Location = new System.Drawing.Point(642, 24);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(73, 15);
		this.label22.TabIndex = 26;
		this.label22.Text = "Weight Cut:";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Teal;
		this.label23.Location = new System.Drawing.Point(507, 24);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(66, 15);
		this.label23.TabIndex = 25;
		this.label23.Text = "Rate Uom:";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Teal;
		this.label24.Location = new System.Drawing.Point(332, 24);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(86, 15);
		this.label24.TabIndex = 24;
		this.label24.Text = "Stock Weight:";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Teal;
		this.label25.Location = new System.Drawing.Point(167, 24);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(75, 15);
		this.label25.TabIndex = 23;
		this.label25.Text = "Net Weight:";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Teal;
		this.label26.Location = new System.Drawing.Point(12, 24);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(85, 15);
		this.label26.TabIndex = 22;
		this.label26.Text = "Gross Weight:";
		this.DetailPanel.Controls.Add(this.ItemNetAmount);
		this.DetailPanel.Controls.Add(this.ExpenseAmount);
		this.DetailPanel.Controls.Add(this.FreightAmount);
		this.DetailPanel.Controls.Add(this.ItemAmount1);
		this.DetailPanel.Controls.Add(this.label35);
		this.DetailPanel.Controls.Add(this.label36);
		this.DetailPanel.Controls.Add(this.label37);
		this.DetailPanel.Controls.Add(this.label38);
		this.DetailPanel.Controls.Add(this.label9);
		this.DetailPanel.Controls.Add(this.RateCut);
		this.DetailPanel.Controls.Add(this.CaptionGdnGrn);
		this.DetailPanel.Controls.Add(this.label11);
		this.DetailPanel.Controls.Add(this.label3);
		this.DetailPanel.Controls.Add(this.WeightCut);
		this.DetailPanel.Controls.Add(this.label4);
		this.DetailPanel.Controls.Add(this.RateUom);
		this.DetailPanel.Controls.Add(this.label5);
		this.DetailPanel.Controls.Add(this.StockWeight);
		this.DetailPanel.Controls.Add(this.label6);
		this.DetailPanel.Controls.Add(this.NetWeight1);
		this.DetailPanel.Controls.Add(this.GrnGdnNo);
		this.DetailPanel.Controls.Add(this.GrossWeight);
		this.DetailPanel.Controls.Add(this.GatePassNo);
		this.DetailPanel.Controls.Add(this.label22);
		this.DetailPanel.Controls.Add(this.JobLot);
		this.DetailPanel.Controls.Add(this.label23);
		this.DetailPanel.Controls.Add(this.CropYear);
		this.DetailPanel.Controls.Add(this.label24);
		this.DetailPanel.Controls.Add(this.PackingType);
		this.DetailPanel.Controls.Add(this.label25);
		this.DetailPanel.Controls.Add(this.BagWeight);
		this.DetailPanel.Controls.Add(this.label26);
		this.DetailPanel.Location = new System.Drawing.Point(18, 35);
		this.DetailPanel.Name = "DetailPanel";
		this.DetailPanel.Size = new System.Drawing.Size(929, 62);
		this.DetailPanel.TabIndex = 34;
		this.ItemNetAmount.AutoSize = true;
		this.ItemNetAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ItemNetAmount.Location = new System.Drawing.Point(610, 42);
		this.ItemNetAmount.Name = "ItemNetAmount";
		this.ItemNetAmount.Size = new System.Drawing.Size(14, 15);
		this.ItemNetAmount.TabIndex = 42;
		this.ItemNetAmount.Text = "1";
		this.ExpenseAmount.AutoSize = true;
		this.ExpenseAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ExpenseAmount.Location = new System.Drawing.Point(433, 42);
		this.ExpenseAmount.Name = "ExpenseAmount";
		this.ExpenseAmount.Size = new System.Drawing.Size(14, 15);
		this.ExpenseAmount.TabIndex = 41;
		this.ExpenseAmount.Text = "1";
		this.FreightAmount.AutoSize = true;
		this.FreightAmount.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.FreightAmount.ForeColor = System.Drawing.Color.Black;
		this.FreightAmount.Location = new System.Drawing.Point(260, 42);
		this.FreightAmount.Name = "FreightAmount";
		this.FreightAmount.Size = new System.Drawing.Size(14, 15);
		this.FreightAmount.TabIndex = 40;
		this.FreightAmount.Text = "1";
		this.ItemAmount1.AutoSize = true;
		this.ItemAmount1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ItemAmount1.ForeColor = System.Drawing.Color.Black;
		this.ItemAmount1.Location = new System.Drawing.Point(90, 42);
		this.ItemAmount1.Name = "ItemAmount1";
		this.ItemAmount1.Size = new System.Drawing.Size(14, 15);
		this.ItemAmount1.TabIndex = 39;
		this.ItemAmount1.Text = "1";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Teal;
		this.label35.Location = new System.Drawing.Point(507, 42);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(109, 15);
		this.label35.TabIndex = 37;
		this.label35.Text = "Item Net Amount:";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Teal;
		this.label36.Location = new System.Drawing.Point(332, 42);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(104, 15);
		this.label36.TabIndex = 36;
		this.label36.Text = "Expense Amount:";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.Teal;
		this.label37.Location = new System.Drawing.Point(167, 42);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(98, 15);
		this.label37.TabIndex = 35;
		this.label37.Text = "Freight Amount:";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.Teal;
		this.label38.Location = new System.Drawing.Point(12, 42);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(85, 15);
		this.label38.TabIndex = 34;
		this.label38.Text = "Item Amount:";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.DetailPanel);
		base.Controls.Add(this.panel12);
		base.Name = "InvoiceHistoryItemInfoControl";
		base.Size = new System.Drawing.Size(956, 97);
		base.Load += new System.EventHandler(PurchaseInvoiceHistoryControl_Load);
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.DetailPanel.ResumeLayout(false);
		this.DetailPanel.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
