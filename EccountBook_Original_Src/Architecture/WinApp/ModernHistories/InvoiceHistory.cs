using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.ContractorWages;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.ModernHistoryModels;
using Architecture.Reports;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using grid.User_Conrtols;

namespace Architecture.WinApp.ModernHistories;

public class InvoiceHistory : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtForWages = new DataTable();

	private DataTable dtHeaderCard = new DataTable();

	public int InvoiceType;

	public int DocumentTypeId;

	public string RequestFromScreen;

	public bool ApproveOrUnApprove;

	private int orderId;

	private int Customer_Supplier_GlAccount;

	private int CommissionAgent_GlAccount;

	private int Broker_GlAccount;

	private int Customer_Supplier_Id;

	private int CommissionAgent_Id;

	private int Broker_Id;

	private bool FiltersVisibilty;

	private int InvoiceIdForEdit;

	private int VoucherHeadIdForVoucherPrint;

	private List<int> ListForApproveOrUnapprove = new List<int>();

	private IContainer components;

	private Panel panel3;

	private Label CaptionForInvoiceTypeHistory;

	private Panel panel4;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel5;

	private Panel panel7;

	private Panel panel8;

	private PictureBox pictureBox1;

	private Panel panel10;

	private Label InvoiceNumber;

	private Label label1;

	private Panel panel9;

	private Label lblCompanyAddress;

	private Label lblCompanyName;

	private Label OrderNo;

	private Label InvoiceDueDate;

	private Label InvoiceDate;

	private Label lblOrder;

	private Label label4;

	private Label label3;

	private Panel panel11;

	private Panel SupplierPanel;

	private Label SupplierName;

	private Label lblSupplierOrCustomer;

	private Label label9;

	private Label label8;

	private Label Email;

	private Label PhoneNo;

	private Label Address;

	private Label label10;

	private Panel panel15;

	private Button button1;

	private Panel panel12;

	private Label label7;

	private Label label20;

	private Label label19;

	private Label label2;

	private Label label18;

	private Label label15;

	private Label label17;

	private FlowLayoutPanel flowLayoutPanel;

	private Panel BrokerPanel;

	private Label BrokerAmount;

	private Label BrokerPercentRate;

	private Label label29;

	private Label label30;

	private Label BrokerName;

	private Label label13;

	private Panel CommissionAgentPanel;

	private Label CommAmount;

	private Label CommPercentRate;

	private Label label26;

	private Label label27;

	private Label CommAgentName;

	private Label label21;

	private ToolStripButton BtnLoadAll;

	private Panel panel13;

	private Label label12;

	private Label lblTotalAmount;

	private Label lblTotalNetWeight;

	private Label lblTotalQuantity;

	private Label label5;

	private Label TotalHeaderRecords;

	private Panel WagesPanel;

	private Label label25;

	private Panel GrandTotalPanel;

	private Panel panel14;

	private Panel panel22;

	private Panel panel24;

	private Panel GrandTotalParentPanel;

	private Panel OtherInformationPanel;

	private Label label43;

	private Label label42;

	private PictureBox pictureBox2;

	private Label label41;

	private FlowLayoutPanel OtherInfoFlowLayout;

	private Panel ChargeToProductPanel;

	private Label label14;

	private Panel CTPHeaderPanel;

	private Label label11;

	private Label label22;

	private Label label16;

	private FlowLayoutPanel FlowLayoutCTP;

	private Panel SupplierAddlessPanel;

	private FlowLayoutPanel FlowLayoutSAL;

	private Label label24;

	private Panel panel17;

	private Label label31;

	private Label label35;

	private Label label39;

	private Panel OtherExpensePanel;

	private FlowLayoutPanel FlowLayoutOE;

	private Label label44;

	private Panel panel21;

	private Label label45;

	private Label label46;

	private Label label47;

	private Panel EmptyBagPanel;

	private FlowLayoutPanel FlowLayoutEBD;

	private Label label48;

	private Panel panel26;

	private Label label49;

	private Label label50;

	private Label label51;

	private Panel PanelItemTotalAmount;

	private Label LblItemTotalInTotals;

	private Label label28;

	private Panel PanelOtherExpenseTotalAmount;

	private Label OtherExpensesInGrandTotal;

	private Label label32;

	private Panel panelBrokeryTotalAmount;

	private Label BrokeryAmount;

	private Label label34;

	private Panel panelSupplierAddlessTotalAmount;

	private Label SupplierAddlessAmountInTotals;

	private Label label36;

	private Panel panelChargeToProductTotalAmount;

	private Label ChargeToProductAmountInTotals;

	private Label label38;

	private Panel panelEmptyBag;

	private Label EmptyBagAmountInTotal;

	private Label label33;

	private Panel panelCommissionAmountInGrandTotal;

	private Label CommissionAmountInTotal;

	private Label label37;

	private Panel panel23;

	private Label GrandTotal;

	private Label label40;

	private Panel panel16;

	private Label label58;

	private Label label57;

	private Label label56;

	private Label label55;

	private Label label52;

	private Label label53;

	private Label label54;

	private FlowLayoutPanel WagesFlowLayout;

	private Panel panel19;

	private Label HandleVisibilityOfFiletrPanel;

	private PictureBox FilterBoxIcon;

	private Panel FilterPanel;

	private Label label60;

	private DateTimePicker DateTo;

	private Label label61;

	private DateTimePicker DateFrom;

	private TextBox txtNumberTo;

	private TextBox txtNoFrom;

	private Label label63;

	private Label label62;

	private Label label64;

	internal UltraCombo cmbSupplierName;

	private FlowLayoutPanel HeaderFlowLayoutPanel;

	private Panel TotalRecordPanel;

	private Button FilterRecords;

	private Label ValueCommRateUom;

	private Label lblCommRateUom;

	private Label ValueCommType;

	private Label label23;

	private Label ValueBrokeryRateUom;

	private Label lblBrokeryRateUom;

	private Label ValueBrokeryType;

	private Label label69;

	private ToolStripButton btnApprove;

	private ToolStripLabel ApprovedCount;

	private ToolStripButton btnEdit;

	private ToolStripButton btnPurchaseInvoiceSlip220;

	private ToolStripButton btnPurchaseInvoiceSlip220A;

	private ToolStripButton btnPurchaseInvoice220bSummary;

	private ToolStripButton btnPurchaseSaleVoucherPrint;

	private ToolStripButton btn104Voucher;

	private ToolStripButton BtnSaleInvoice301Slip;

	private ToolStripButton BtnSaleInvoiceSlip303;

	private ToolStripButton btnPIDirectSlip225;

	private ToolStripButton btnPIDirect225PartySlip;

	private ToolStripButton btnVouvher118;

	private ToolStripButton btn294Print;

	private ToolStripButton btn294APrint;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnCheckAll;

	private ToolStripButton BtnLoadDefault;

	public InvoiceHistory(UserAccount pUserAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			if (DocumentTypeId == 56)
			{
				if (RequestFromScreen == "Purchase Invoice Approval")
				{
					if (ApproveOrUnApprove)
					{
						CaptionForInvoiceTypeHistory.Text = "PURCHASE INVOICE FOR UNAPPROVAL";
					}
					else
					{
						CaptionForInvoiceTypeHistory.Text = "PURCHASE INVOICE FOR APPROVAL";
					}
				}
				else
				{
					CaptionForInvoiceTypeHistory.Text = "PURCHASE INVOICE HISTORY";
				}
				btnPurchaseInvoiceSlip220.Visible = true;
				btnPurchaseInvoiceSlip220A.Visible = true;
				btnPurchaseInvoice220bSummary.Visible = true;
				btnPurchaseSaleVoucherPrint.Visible = true;
				btn104Voucher.Visible = true;
			}
			else if (DocumentTypeId == 57)
			{
				if (RequestFromScreen == "Purchase Invoice Approval")
				{
					if (ApproveOrUnApprove)
					{
						CaptionForInvoiceTypeHistory.Text = "PURCHASE INVOICE DIRECT FOR UNAPPROVAL";
					}
					else
					{
						CaptionForInvoiceTypeHistory.Text = "PURCHASE INVOICE DIRECT FOR APPROVAL";
					}
				}
				else
				{
					CaptionForInvoiceTypeHistory.Text = "Purchase Invoice Direct History";
				}
				btnPIDirectSlip225.Visible = true;
				btnPIDirectSlip225.Visible = true;
				btnVouvher118.Visible = true;
			}
			else if (DocumentTypeId == 95)
			{
				if (RequestFromScreen == "Sale Invoice Approval")
				{
					if (ApproveOrUnApprove)
					{
						CaptionForInvoiceTypeHistory.Text = "SALE INVOICE FOR UNAPPROVAL";
					}
					else
					{
						CaptionForInvoiceTypeHistory.Text = "SALE INVOICE FOR APPROVAL";
					}
				}
				else
				{
					CaptionForInvoiceTypeHistory.Text = "SALE INVOICE HISTORY";
				}
				BtnSaleInvoice301Slip.Visible = true;
				BtnSaleInvoiceSlip303.Visible = true;
				btnPurchaseSaleVoucherPrint.Visible = true;
			}
			else if (DocumentTypeId == 98)
			{
				if (RequestFromScreen == "Purchase Invoice Approval")
				{
					if (ApproveOrUnApprove)
					{
						CaptionForInvoiceTypeHistory.Text = "SALE INVOICE RETURN FOR UNAPPROVAL";
					}
					else
					{
						CaptionForInvoiceTypeHistory.Text = "SALE INVOICE RETURN FOR APPROVAL";
					}
				}
				else
				{
					CaptionForInvoiceTypeHistory.Text = "SALE INVOICE RETURN HISTORY";
				}
			}
			else if (DocumentTypeId == 99)
			{
				if (RequestFromScreen == "Sale Invoice Approval")
				{
					if (ApproveOrUnApprove)
					{
						CaptionForInvoiceTypeHistory.Text = "SALE INVOICE DIRECT FOR UNAPPROVAL";
					}
					else
					{
						CaptionForInvoiceTypeHistory.Text = "SALE INVOICE DIRECT FOR APPROVAL";
					}
				}
				else
				{
					CaptionForInvoiceTypeHistory.Text = "SALE INVOICE DIRECT HISTORY";
				}
				btn294Print.Visible = true;
				btnPurchaseSaleVoucherPrint.Visible = true;
			}
			if (RequestFromScreen == "Purchase Invoice Approval" || RequestFromScreen == "Sale Invoice Approval")
			{
				btnApprove.Visible = true;
				btnCheckAll.Visible = true;
				ApprovedCount.Visible = true;
				btnPurchaseSaleVoucherPrint.Visible = true;
				btnEdit.Visible = false;
				if (ApproveOrUnApprove)
				{
					btnApprove.Text = "UnApprove";
					btnApprove.Image = Resources.close_line;
				}
				else
				{
					btnApprove.Text = "Approve";
					btnApprove.Image = Resources.check_line;
				}
			}
			FiltersVisibilty = false;
			FilterBoxIcon.Image = Resources.arrow_down_s_line;
			FilterPanel.Hide();
			FilterRecords.Hide();
			DateFrom.Value = DateTime.Now;
			DateTo.Value = DateTime.Now;
			if (DocumentTypeId != 95)
			{
				GenerateHeaderCardDataOnLoad();
			}
			panel7.Hide();
			panelCommissionAmountInGrandTotal.Hide();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadIdForVoucherPrint = CommonServices.VoucherHeadIdGet(InvoiceIdForEdit, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateHeaderCardDataOnLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.NoOfRecords = 21;
			obj.DocumentTypeId = DocumentTypeId;
			obj.CanViewAllRecord = true;
			obj.EntryUser = UserAccount.ID;
			if (RequestFromScreen == "Purchase Invoice" || RequestFromScreen == "Sale Invoice")
			{
				obj.ApprovedFilter = "All";
			}
			if (RequestFromScreen == "Purchase Invoice Approval" || RequestFromScreen == "Sale Invoice Approval")
			{
				obj.IsApproved = ApproveOrUnApprove;
			}
			if (InvoiceType == 1)
			{
				dtHeaderCard = InvPurchaseInvoice.InvoiceHistoryHeaderForCustomPdfReport(obj);
			}
			else if (InvoiceType == 2)
			{
				dtHeaderCard = InvSaleInvoice.GetHistoryForPdfReport_HeaderData(obj);
			}
			if (dtHeaderCard.Rows.Count > 0)
			{
				GenerateHeaderCards(dtHeaderCard);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateHeaderCards(DataTable dt)
	{
		try
		{
			HeaderFlowLayoutPanel.Controls.Clear();
			InvoiceHeaderCard[] ItemHeaderCardsList = new InvoiceHeaderCard[dt.Rows.Count];
			for (int i = 0; i < ItemHeaderCardsList.Length; i++)
			{
				ItemHeaderCardsList[i] = new InvoiceHeaderCard();
				ItemHeaderCardsList[i].RequestFrom = RequestFromScreen;
				ItemHeaderCardsList[i].InvoiceType = InvoiceType;
				ItemHeaderCardsList[i].DocumentTypeId = DocumentTypeId;
				ItemHeaderCardsList[i].InvoiceId = Conversion.ToInt(dt.Rows[i]["InvoiceId"]);
				ItemHeaderCardsList[i].SupplierName = Conversion.ToString(dt.Rows[i]["Customer_Supplier_Name"]);
				ItemHeaderCardsList[i].InvoiceNumber = Conversion.ToString(dt.Rows[i]["InvoiceNumber"]);
				ItemHeaderCardsList[i].OrderNumber = Conversion.ToString(dt.Rows[i]["Sale_Purcahse_OrderNumber"]);
				if (InvoiceType == 1)
				{
					if (Conversion.ToInt(dt.Rows[i]["InvoiceTypeId"]) == 1)
					{
						ItemHeaderCardsList[i].CaptionForInvoiceType = "Regular Purchase";
					}
					else if (Conversion.ToInt(dt.Rows[i]["InvoiceTypeId"]) == 3)
					{
						ItemHeaderCardsList[i].CaptionForInvoiceType = "Gate Purchase";
					}
					else if (Conversion.ToInt(dt.Rows[i]["InvoiceTypeId"]) == 2)
					{
						ItemHeaderCardsList[i].CaptionForInvoiceType = "Market Purchase";
					}
				}
				if (!Conversion.ToBool(dt.Rows[i]["IsApproved"]))
				{
					ItemHeaderCardsList[i].ApprovedOrNot = "Not Approved";
				}
				else if (Conversion.ToBool(dt.Rows[i]["IsApproved"]))
				{
					ItemHeaderCardsList[i].ApprovedOrNot = "Approved";
				}
				ItemHeaderCardsList[i].DocDate = Conversion.ToDateTime(dt.Rows[i]["InvoiceDate"]).ToShortDateString();
				double billAmount = Conversion.ToDouble(dt.Rows[i]["GrandBillAmount"]);
				ItemHeaderCardsList[i].BillAmount = billAmount.ToString("0,0");
				HeaderFlowLayoutPanel.Controls.Add(ItemHeaderCardsList[i]);
				ItemHeaderCardsList[i].UserControlClicked += HeaderCard_Click;
			}
			TotalHeaderRecords.Text = Conversion.ToString((object)dt.Rows.Count);
			PartyComboFill(dt);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyComboFill(DataTable dt)
	{
		try
		{
			DataTable dtParty = new DataTable();
			dtParty.Columns.Add("Id", typeof(int));
			dtParty.Columns.Add("Name");
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			int i;
			for (i = 0; i < dt.Rows.Count; i++)
			{
				if (i == 0)
				{
					dtParty.Rows.Add(Conversion.ToInt(dt.Rows[i]["SupplierCustomerId"]), dt.Rows[i]["Customer_Supplier_Name"]);
				}
				if ((from c in dtParty.AsEnumerable()
					where c.Field<int>("Id").Equals(Conversion.ToInt(dt.Rows[i]["SupplierCustomerId"]))
					select c).Count() <= 0)
				{
					dtParty.Rows.Add(Conversion.ToInt(dt.Rows[i]["SupplierCustomerId"]), dt.Rows[i]["Customer_Supplier_Name"]);
				}
			}
			DDL.BindDDLNew(dtParty, cmbSupplierName, "Id", "Name", "Party Name", false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HeaderCard_Click(object sender, EventArgs e)
	{
		try
		{
			InvoiceHeaderCard obj = (InvoiceHeaderCard)sender;
			InvoiceIdForEdit = obj.InvoiceId;
			VoucherHeadIdGet();
			GenerateInvoiceHistoryAgainstInvoiceID(obj.InvoiceId);
			if ((!(RequestFromScreen == "Purchase Invoice Approval") && !(RequestFromScreen == "Sale Invoice Approval")) || !(Conversion.ToString(obj.Tag) == "FromCheckBox"))
			{
				return;
			}
			if (!obj.IsSelected)
			{
				int id = 0;
				if (ListForApproveOrUnapprove.Count > 0)
				{
					id = ListForApproveOrUnapprove.Find((int Id) => Id == obj.InvoiceId);
				}
				if (id > 0)
				{
					ListForApproveOrUnapprove.Remove(id);
				}
				ApprovedCount.Text = Conversion.ToString((object)ListForApproveOrUnapprove.Count);
				return;
			}
			int id2 = 0;
			if (ListForApproveOrUnapprove.Count > 0)
			{
				id2 = ListForApproveOrUnapprove.Find((int Id) => Id == obj.InvoiceId);
			}
			if (id2 > 0)
			{
				ListForApproveOrUnapprove.Remove(id2);
			}
			else
			{
				ListForApproveOrUnapprove.Add(obj.InvoiceId);
			}
			ApprovedCount.Text = Conversion.ToString((object)ListForApproveOrUnapprove.Count);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateInvoiceHistoryAgainstInvoiceID(int InvoiceId)
	{
		try
		{
			panel7.Show();
			List<Purchase_Sale_InvoiceHistory> lst = new List<Purchase_Sale_InvoiceHistory>();
			if (InvoiceType == 1)
			{
				lst = InvPurchaseInvoice.InoviceHistoryForCustomPdfReport(InvoiceId);
			}
			else if (InvoiceType == 2)
			{
				lst = InvSaleInvoice.InoviceHistoryForCustomPdfReport(InvoiceId);
			}
			if (lst.Count <= 0)
			{
				return;
			}
			double OtherExpensesAmountInGrandTotal = 0.0;
			double BrokeryAmountInGrandTotal = 0.0;
			double SupplierAddlessInGrandTotal = 0.0;
			double ChargeToProductInGrandTotal = 0.0;
			double EmptyBagInGrandTotal = 0.0;
			double CommissionAmountInGrandTotal = 0.0;
			double GrandTotalAmount = 0.0;
			int counter = 0;
			int counterForGrandTotalPanels = 1;
			btnAttachment.Text = lst[0].InvoiceHeader.NoOfAttachments.ToString();
			string Path = "";
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Logo").ToList();
			if (list.Count > 0)
			{
				Path = list[0].ConfigKey;
			}
			if (File.Exists(Path))
			{
				pictureBox1.Image = Image.FromFile(Path);
				pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			}
			lblCompanyName.Text = UserAccount.CompName;
			lblCompanyAddress.Text = UserAccount.CompAddress;
			InvoiceNumber.Text = Conversion.ToString((object)lst[0].InvoiceHeader.InvoiceNumber);
			InvoiceDate.Text = Conversion.ToString((object)lst[0].InvoiceHeader.InvoiceDate.ToShortDateString());
			if (!Conversion.CheckDateTimeNull(lst[0].InvoiceHeader.DueDate))
			{
				InvoiceDueDate.Text = Conversion.ToString((object)lst[0].InvoiceHeader.DueDate.ToShortDateString());
			}
			else
			{
				InvoiceDueDate.Text = Conversion.ToString((object)lst[0].InvoiceHeader.InvoiceDate.ToShortDateString());
			}
			if (lst[0].InvoiceHeader.Sale_Purchase_OrderId > 0)
			{
				lblOrder.Show();
				OrderNo.Show();
				OrderNo.Text = Conversion.ToString((object)lst[0].InvoiceHeader.Sale_Purcahse_OrderNumber);
				orderId = lst[0].InvoiceHeader.Sale_Purchase_OrderId;
			}
			else
			{
				lblOrder.Hide();
				OrderNo.Hide();
				orderId = 0;
			}
			if (InvoiceType == 1)
			{
				lblSupplierOrCustomer.Text = "Supplier";
			}
			else
			{
				lblSupplierOrCustomer.Text = "Customer";
			}
			SupplierName.Text = Conversion.ToString((object)lst[0].InvoiceHeader.Customer_Supplier_Name);
			Customer_Supplier_GlAccount = lst[0].InvoiceHeader.Customer_Supplier_GlAccountId;
			Customer_Supplier_Id = lst[0].InvoiceHeader.Customer_Supplier_Id;
			PhoneNo.Text = Conversion.ToString((object)lst[0].InvoiceHeader.Customer_Supplier_MobileNumber);
			Email.Text = Conversion.ToString((object)lst[0].InvoiceHeader.Customer_Supplier_Email);
			Address.Text = Conversion.ToString((object)lst[0].InvoiceHeader.Customer_Supplier_Address);
			if (lst[0].InvoiceHeader.ComissionPresent)
			{
				CommissionAgentPanel.Show();
				CommissionAgent_GlAccount = lst[0].InvoiceHeader.ComissionAgentGlId;
				CommissionAgent_Id = lst[0].InvoiceHeader.ComissionAgentId;
				CommAgentName.Text = Conversion.ToString((object)lst[0].InvoiceHeader.ComissionAgentName);
				ValueCommType.Text = Conversion.ToString((object)lst[0].InvoiceHeader.ComissionType);
				if (lst[0].InvoiceHeader.ComissionType == "Weight")
				{
					lblCommRateUom.Show();
					ValueCommRateUom.Show();
					ValueCommRateUom.Text = Conversion.ToString((object)lst[0].InvoiceHeader.ComissionRateUom);
				}
				else
				{
					lblCommRateUom.Hide();
					ValueCommRateUom.Hide();
				}
				CommPercentRate.Text = Conversion.ToString((object)lst[0].InvoiceHeader.ComissioniRate);
				CommAmount.Text = Conversion.ToString((object)lst[0].InvoiceHeader.ComissionAmount);
				if (lst[0].InvoiceHeader.Customer_Supplier_Id == lst[0].InvoiceHeader.ComissionAgentId)
				{
					panelCommissionAmountInGrandTotal.Show();
					counterForGrandTotalPanels++;
					if (InvoiceType == 1)
					{
						CommissionAmountInTotal.Text = lst[0].InvoiceHeader.ComissionAmount.ToString("0,0");
						CommissionAmountInGrandTotal = lst[0].InvoiceHeader.ComissionAmount;
					}
					else
					{
						CommAmount.Text = (0.0 - lst[0].InvoiceHeader.ComissionAmount).ToString("#,#;(#,#);0");
						CommissionAmountInTotal.Text = (0.0 - lst[0].InvoiceHeader.ComissionAmount).ToString("#,#;(#,#);0");
						CommissionAmountInGrandTotal = 0.0 - lst[0].InvoiceHeader.ComissionAmount;
					}
				}
				else
				{
					panelCommissionAmountInGrandTotal.Hide();
				}
			}
			else
			{
				CommissionAgentPanel.Hide();
				panelCommissionAmountInGrandTotal.Hide();
			}
			if (lst[0].InvoiceHeader.BorkeryPresent)
			{
				BrokerPanel.Show();
				Broker_GlAccount = lst[0].InvoiceHeader.BrokerGlId;
				Broker_Id = lst[0].InvoiceHeader.BrokerAgentId;
				BrokerName.Text = Conversion.ToString((object)lst[0].InvoiceHeader.BrokerAgentName);
				ValueBrokeryType.Text = Conversion.ToString((object)lst[0].InvoiceHeader.BrokeryType);
				if (lst[0].InvoiceHeader.BrokeryType == "Weight")
				{
					lblBrokeryRateUom.Show();
					ValueBrokeryRateUom.Show();
					ValueBrokeryRateUom.Text = Conversion.ToString((object)lst[0].InvoiceHeader.BrokeryRateUom);
				}
				else
				{
					lblBrokeryRateUom.Hide();
					ValueBrokeryRateUom.Hide();
				}
				BrokerPercentRate.Text = Conversion.ToString((object)lst[0].InvoiceHeader.BrokeryRate);
				BrokerAmount.Text = Conversion.ToString((object)lst[0].InvoiceHeader.BrokeryAmount);
				BrokeryAmountInGrandTotal = 0.0 - lst[0].InvoiceHeader.BrokeryAmount;
				BrokerAmount.Text = (0.0 - lst[0].InvoiceHeader.BrokeryAmount).ToString("#,#;(#,#);0");
				BrokeryAmount.Text = (0.0 - lst[0].InvoiceHeader.BrokeryAmount).ToString("#,#;(#,#);0");
				panelBrokeryTotalAmount.Show();
				counterForGrandTotalPanels++;
			}
			else
			{
				BrokerPanel.Hide();
				panelBrokeryTotalAmount.Hide();
			}
			flowLayoutPanel.Controls.Clear();
			double TotalQty = 0.0;
			double TotalWeight = 0.0;
			double TotalAmount = 0.0;
			if (lst[0].InvoiceDetail.Count > 0)
			{
				InvoiceHistoryItemInfoControl[] ItemCardsList = new InvoiceHistoryItemInfoControl[lst[0].InvoiceDetail.Count];
				for (int m = 0; m < ItemCardsList.Length; m++)
				{
					ItemCardsList[m] = new InvoiceHistoryItemInfoControl();
					TotalQty += lst[0].InvoiceDetail[m].ItemQty;
					TotalWeight += lst[0].InvoiceDetail[m].NetWeight;
					TotalAmount += lst[0].InvoiceDetail[m].ItemAmount;
					ItemCardsList[m].InvoiceType = InvoiceType;
					ItemCardsList[m].DocumentTypeId = DocumentTypeId;
					ItemCardsList[m].InvoiceDate = lst[0].InvoiceHeader.InvoiceDate;
					ItemCardsList[m].ItemId = lst[0].InvoiceDetail[m].ItemId;
					if (InvoiceType == 1)
					{
						ItemCardsList[m].ItemAccountId = lst[0].InvoiceDetail[m].PurchaseGLAC;
					}
					else
					{
						ItemCardsList[m].ItemAccountId = lst[0].InvoiceDetail[m].SaleGLAC;
					}
					ItemCardsList[m].itemName = lst[0].InvoiceDetail[m].ItemName;
					ItemCardsList[m].Warehouse = lst[0].InvoiceDetail[m].WarehouseName;
					ItemCardsList[m].Qty = Conversion.ToString((object)lst[0].InvoiceDetail[m].ItemQty.ToString("0,0"));
					ItemCardsList[m].uom = lst[0].InvoiceDetail[m].PackUom;
					ItemCardsList[m].NtWeight = Conversion.ToString((object)lst[0].InvoiceDetail[m].NetWeight.ToString("0,0"));
					ItemCardsList[m].NtWeight1 = Conversion.ToString((object)lst[0].InvoiceDetail[m].NetWeight.ToString("0,0"));
					ItemCardsList[m].rate = Conversion.ToString((object)lst[0].InvoiceDetail[m].Rate);
					ItemCardsList[m].itemAmount = Conversion.ToString((object)lst[0].InvoiceDetail[m].ItemAmount.ToString("0,0"));
					ItemCardsList[m].itemAmount1 = Conversion.ToString((object)lst[0].InvoiceDetail[m].ItemAmount.ToString("0,0"));
					ItemCardsList[m].Gdn_GrnId = lst[0].InvoiceDetail[m].Gdn_Grn_Id;
					if (InvoiceType == 1)
					{
						ItemCardsList[m].captionGdnGrn = "GRN #";
					}
					else
					{
						ItemCardsList[m].captionGdnGrn = "GDN #";
					}
					ItemCardsList[m].grnGdnNo = Conversion.ToString((object)lst[0].InvoiceDetail[m].Gdn_Grn_Number);
					ItemCardsList[m].GpId = lst[0].InvoiceDetail[m].GatePassId;
					ItemCardsList[m].GpNo = Conversion.ToString((object)lst[0].InvoiceDetail[m].GatePassNumber);
					ItemCardsList[m].jobLot = Conversion.ToString((object)lst[0].InvoiceDetail[m].JobLotName);
					ItemCardsList[m].cropYear = Conversion.ToString((object)lst[0].InvoiceDetail[m].CropYear);
					ItemCardsList[m].packingType = Conversion.ToString((object)lst[0].InvoiceDetail[m].PackingType);
					ItemCardsList[m].bagWeight = Conversion.ToString((object)lst[0].InvoiceDetail[m].BagWeight.ToString("0,0"));
					ItemCardsList[m].grossWeight = Conversion.ToString((object)lst[0].InvoiceDetail[m].GrossWeight.ToString("0,0"));
					ItemCardsList[m].stockWeight = Conversion.ToString((object)lst[0].InvoiceDetail[m].StockWeight.ToString("0,0"));
					ItemCardsList[m].rateUom = Conversion.ToString((object)lst[0].InvoiceDetail[m].RateUom);
					ItemCardsList[m].weightCut = Conversion.ToString((object)lst[0].InvoiceDetail[m].WeightCut.ToString("0,0"));
					ItemCardsList[m].rateCut = Conversion.ToString((object)lst[0].InvoiceDetail[m].RateCutAmount.ToString("0,0"));
					ItemCardsList[m].freightAmount = Conversion.ToString((object)lst[0].InvoiceDetail[m].FreightAmount.ToString("0,0"));
					ItemCardsList[m].expenseAmount = Conversion.ToString((object)lst[0].InvoiceDetail[m].ExpenseAmount.ToString("0,0"));
					ItemCardsList[m].itemNetAmount = Conversion.ToString((object)lst[0].InvoiceDetail[m].BillAmount.ToString("0,0"));
					flowLayoutPanel.Controls.Add(ItemCardsList[m]);
					ItemCardsList[m].UserControlClicked += UserControl_Click;
				}
			}
			lblTotalQuantity.Text = TotalQty.ToString("0,0");
			lblTotalNetWeight.Text = TotalWeight.ToString("0,0");
			lblTotalAmount.Text = TotalAmount.ToString("0,0");
			LblItemTotalInTotals.Text = TotalAmount.ToString("0,0");
			GetWagesInformation(InvoiceId);
			if (lst[0].ChargeToProductDetailPresent)
			{
				FlowLayoutCTP.Controls.Clear();
				OtherInfoCard1[] Cards4 = new OtherInfoCard1[lst[0].ChargeToProductDetail.Count];
				for (int l = 0; l < Cards4.Length; l++)
				{
					Cards4[l] = new OtherInfoCard1();
					Cards4[l].name = lst[0].ChargeToProductDetail[l].AccountName;
					Cards4[l].amount = Conversion.ToString((object)lst[0].ChargeToProductDetail[l].Amount.ToString("0,0"));
					Cards4[l].remarks = lst[0].ChargeToProductDetail[l].Remarks;
					ChargeToProductInGrandTotal += lst[0].ChargeToProductDetail[l].Amount;
					FlowLayoutCTP.Controls.Add(Cards4[l]);
				}
				ChargeToProductPanel.Show();
				panelChargeToProductTotalAmount.Show();
				ChargeToProductAmountInTotals.Text = ChargeToProductInGrandTotal.ToString("0,0");
				counter++;
				counterForGrandTotalPanels++;
			}
			else
			{
				ChargeToProductPanel.Hide();
				panelChargeToProductTotalAmount.Hide();
			}
			if (lst[0].Customer_Supplier_AddLessDetailPresent)
			{
				FlowLayoutSAL.Controls.Clear();
				double debit = 0.0;
				double credit = 0.0;
				OtherInfoCard2[] Cards3 = new OtherInfoCard2[lst[0].Customer_Supplier_AddLessDetail.Count];
				for (int k = 0; k < Cards3.Length; k++)
				{
					Cards3[k] = new OtherInfoCard2();
					Cards3[k].name = lst[0].Customer_Supplier_AddLessDetail[k].AccountName;
					Cards3[k].Debit = Conversion.ToString((object)lst[0].Customer_Supplier_AddLessDetail[k].DebitAmount.ToString("0,0"));
					Cards3[k].Credit = Conversion.ToString((object)lst[0].Customer_Supplier_AddLessDetail[k].CreditAmount.ToString("0,0"));
					debit += lst[0].Customer_Supplier_AddLessDetail[k].DebitAmount;
					credit += lst[0].Customer_Supplier_AddLessDetail[k].CreditAmount;
					FlowLayoutSAL.Controls.Add(Cards3[k]);
				}
				SupplierAddlessPanel.Show();
				panelSupplierAddlessTotalAmount.Show();
				SupplierAddlessInGrandTotal = debit - credit;
				SupplierAddlessAmountInTotals.Text = SupplierAddlessInGrandTotal.ToString("#,#;(#,#);0");
				counter++;
				counterForGrandTotalPanels++;
			}
			else
			{
				SupplierAddlessPanel.Hide();
				panelSupplierAddlessTotalAmount.Hide();
			}
			if (lst[0].EmptyBagsDetailPresent)
			{
				FlowLayoutEBD.Controls.Clear();
				OtherInfoCard1[] Cards2 = new OtherInfoCard1[lst[0].EmptyBagsDetail.Count];
				for (int j = 0; j < Cards2.Length; j++)
				{
					Cards2[j] = new OtherInfoCard1();
					Cards2[j].name = lst[0].EmptyBagsDetail[j].ItemName;
					Cards2[j].amount = Conversion.ToString((object)lst[0].EmptyBagsDetail[j].Amount.ToString("0,0"));
					Cards2[j].remarks = lst[0].EmptyBagsDetail[j].Remarks;
					EmptyBagInGrandTotal += lst[0].EmptyBagsDetail[j].Amount;
					FlowLayoutEBD.Controls.Add(Cards2[j]);
				}
				EmptyBagPanel.Show();
				panelEmptyBag.Show();
				EmptyBagAmountInTotal.Text = EmptyBagInGrandTotal.ToString("0,0");
				counter++;
				counterForGrandTotalPanels++;
			}
			else
			{
				EmptyBagPanel.Hide();
				panelEmptyBag.Hide();
			}
			if (lst[0].OtherExpensesDetailPresent)
			{
				FlowLayoutOE.Controls.Clear();
				OtherInfoCard1[] Cards = new OtherInfoCard1[lst[0].OtherExpensesDetail.Count];
				for (int i = 0; i < Cards.Length; i++)
				{
					Cards[i] = new OtherInfoCard1();
					Cards[i].name = lst[0].OtherExpensesDetail[i].ItemName;
					Cards[i].amount = Conversion.ToString((object)lst[0].OtherExpensesDetail[i].Amount.ToString("0,0"));
					Cards[i].remarks = lst[0].OtherExpensesDetail[i].Remarks;
					OtherExpensesAmountInGrandTotal += lst[0].OtherExpensesDetail[i].Amount;
					FlowLayoutOE.Controls.Add(Cards[i]);
				}
				OtherExpensePanel.Show();
				PanelOtherExpenseTotalAmount.Show();
				OtherExpensesInGrandTotal.Text = OtherExpensesAmountInGrandTotal.ToString("0,0");
				counter++;
				counterForGrandTotalPanels++;
			}
			else
			{
				OtherExpensePanel.Hide();
				PanelOtherExpenseTotalAmount.Hide();
			}
			if (counter < 3 && counter > 0)
			{
				OtherInformationPanel.Show();
				OtherInformationPanel.Height = 187;
			}
			else if (counter > 2 && counter < 5)
			{
				OtherInformationPanel.Height = 375;
				OtherInformationPanel.Show();
			}
			else if (counter == 0)
			{
				OtherInformationPanel.Hide();
			}
			switch (counterForGrandTotalPanels)
			{
			case 1:
				GrandTotalPanel.Height = 71;
				if (dtForWages.Rows.Count > 0)
				{
					WagesPanel.Show();
					GrandTotalParentPanel.Height = 145;
				}
				else
				{
					WagesPanel.Hide();
					GrandTotalParentPanel.Height = 75;
				}
				break;
			case 2:
				GrandTotalPanel.Height = 97;
				if (dtForWages.Rows.Count > 0)
				{
					WagesPanel.Show();
					GrandTotalParentPanel.Height = 145;
				}
				else
				{
					WagesPanel.Hide();
					GrandTotalParentPanel.Height = 97;
				}
				break;
			case 3:
				GrandTotalPanel.Height = 123;
				GrandTotalParentPanel.Height = 145;
				break;
			case 4:
				GrandTotalPanel.Height = 149;
				GrandTotalParentPanel.Height = 160;
				break;
			case 5:
				GrandTotalPanel.Height = 175;
				GrandTotalParentPanel.Height = 188;
				break;
			case 6:
				GrandTotalPanel.Height = 201;
				GrandTotalParentPanel.Height = 202;
				break;
			case 7:
				GrandTotalPanel.Height = 227;
				GrandTotalParentPanel.Height = 234;
				break;
			}
			GrandTotalAmount = TotalAmount + OtherExpensesAmountInGrandTotal + SupplierAddlessInGrandTotal + BrokeryAmountInGrandTotal + CommissionAmountInGrandTotal + EmptyBagInGrandTotal;
			GrandTotal.Text = GrandTotalAmount.ToString("0,0");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetWagesInformation(int InvoiceId)
	{
		try
		{
			dtForWages = InvContractorWagesBillHeader.ReadWagesSummaryByDocumentTypeAndDocumentId(DocumentTypeId, InvoiceId);
			if (dtForWages.Rows.Count > 0)
			{
				WagesPanel.Show();
				WagesFlowLayout.Controls.Clear();
				WagesInfoControl[] Cards = new WagesInfoControl[dtForWages.Rows.Count];
				for (int i = 0; i < Cards.Length; i++)
				{
					Cards[i] = new WagesInfoControl();
					Cards[i].ContractorId = Conversion.ToInt(dtForWages.Rows[i]["ContractorId"]);
					Cards[i].ContractorName = Conversion.ToString(dtForWages.Rows[i]["ContractorName"]);
					Cards[i].WagesAccountName = Conversion.ToString(dtForWages.Rows[i]["WagesAccountName"]);
					double qty = Conversion.ToDouble(dtForWages.Rows[i]["Quantity"]);
					Cards[i].Quantity = qty.ToString("0,0");
					double weight = Conversion.ToDouble(dtForWages.Rows[i]["Weight"]);
					Cards[i].Weight = weight.ToString("0,0");
					Cards[i].PackSize = Conversion.ToString(dtForWages.Rows[i]["PackSize"]);
					double Rate = Conversion.ToDouble(dtForWages.Rows[i]["Rate"]);
					Cards[i].Rate = Rate.ToString("0,0");
					double Amount = Conversion.ToDouble(dtForWages.Rows[i]["Amount"]);
					Cards[i].Amount = Amount.ToString("0,0");
					WagesFlowLayout.Controls.Add(Cards[i]);
					Cards[i].UserControlClicked += WegesContractorName_Click;
				}
			}
			else
			{
				WagesPanel.Hide();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WegesContractorName_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GoToGeneralLedgerFromLinkedEvent(((WagesInfoControl)sender).ContractorId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		_ = (InvoiceHistoryItemInfoControl)sender;
	}

	private void BtnLoadAll_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.NoOfRecords = 0;
			obj.DocumentTypeId = DocumentTypeId;
			obj.CanViewAllRecord = true;
			obj.EntryUser = UserAccount.ID;
			if (RequestFromScreen == "Purchase Invoice" || RequestFromScreen == "Sale Invoice")
			{
				obj.ApprovedFilter = "All";
				obj.IsApproved = false;
			}
			if (RequestFromScreen == "Purchase Invoice Approval" || RequestFromScreen == "Sale Invoice Approval")
			{
				obj.IsApproved = ApproveOrUnApprove;
			}
			if (InvoiceType == 1)
			{
				dtHeaderCard = InvPurchaseInvoice.InvoiceHistoryHeaderForCustomPdfReport(obj);
			}
			else
			{
				dtHeaderCard = InvSaleInvoice.GetHistoryForPdfReport_HeaderData(obj);
			}
			if (dtHeaderCard.Rows.Count > 0)
			{
				GenerateHeaderCards(dtHeaderCard);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HandleVisibilityOfFiletrPanel_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FiltersVisibilty)
			{
				FiltersVisibilty = true;
				FilterPanel.Show();
				FilterRecords.Show();
				FilterBoxIcon.Image = Resources.arrow_up_s_line;
				return;
			}
			FiltersVisibilty = false;
			txtNoFrom.Text = string.Empty;
			txtNumberTo.Text = string.Empty;
			((Control)(object)cmbSupplierName).Text = string.Empty;
			FilterPanel.Hide();
			FilterRecords.Hide();
			FilterBoxIcon.Image = Resources.arrow_down_s_line;
			if (ListForApproveOrUnapprove.Count == 0)
			{
				GenerateHeaderCardDataOnLoad();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FilterRecords_Click(object sender, EventArgs e)
	{
		try
		{
			if (dtHeaderCard.AsEnumerable().Count((DataRow r) => (r.Field<DateTime>("InvoiceDate") >= Conversion.ToDateTime((object)DateFrom.Value.ToShortDateString()) && r.Field<DateTime>("InvoiceDate") <= Conversion.ToDateTime((object)DateTo.Value.ToShortDateString()) && r.Field<int>("InvoiceNumber") >= Conversion.ToInt((object)txtNoFrom.Text) && r.Field<int>("InvoiceNumber") <= Conversion.ToInt((object)txtNumberTo.Text)) || r.Field<int>("SupplierCustomerId") == Conversion.ToInt(cmbSupplierName.Value)) > 0)
			{
				dtHeaderCard = (from r in dtHeaderCard.AsEnumerable()
					where (r.Field<DateTime>("InvoiceDate") >= Conversion.ToDateTime((object)DateFrom.Value.ToShortDateString()) && r.Field<DateTime>("InvoiceDate") <= Conversion.ToDateTime((object)DateTo.Value.ToShortDateString()) && r.Field<int>("InvoiceNumber") >= Conversion.ToInt((object)txtNoFrom.Text) && r.Field<int>("InvoiceNumber") <= Conversion.ToInt((object)txtNumberTo.Text)) || r.Field<int>("SupplierCustomerId") == Conversion.ToInt(cmbSupplierName.Value)
					select r).CopyToDataTable();
				GenerateHeaderCards(dtHeaderCard);
				return;
			}
			throw new Exception("No Record Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApprove_Click(object sender, EventArgs e)
	{
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		try
		{
			if (ListForApproveOrUnapprove.Count == 0)
			{
				throw new Exception("No Record Selected");
			}
			if (ApproveOrUnApprove)
			{
				if (MessageBox.Show("Are you sure to UnApprove Invoice?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				foreach (int Id in ListForApproveOrUnapprove)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.DocumentTypeId = DocumentTypeId;
					item.Id = Id;
					item.PostDate = DateTime.Now;
					item.EntryUser = UserAccount.ID;
					item.ReqType = "UP";
					obj.AllApprovalLists.Add(item);
				}
				if (RequestFromScreen == "Purchase Invoice Approval")
				{
					InvPurchaseInvoice.InvPurchaseInvoiceandVoucherApprove(obj);
				}
				else
				{
					InvSaleInvoice.InvSaleInvoiceandVoucherApprove(obj);
				}
				MessageBox.Show("Record UnApproved Successfully");
				ListForApproveOrUnapprove = new List<int>();
				ApprovedCount.Text = "0";
				panel7.Hide();
				BtnLoadAll_Click(null, null);
			}
			else
			{
				if (MessageBox.Show("Are you sure to Approve ?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ApprovalList obj2 = new ApprovalList();
				obj2.AllApprovalLists = new List<ApprovalList>();
				foreach (int Id2 in ListForApproveOrUnapprove)
				{
					ApprovalList item2 = new ApprovalList();
					item2.OrganizationId = UserAccount.OrganizationId;
					item2.CompanyId = UserAccount.CompanyId;
					item2.DocumentTypeId = DocumentTypeId;
					item2.Id = Id2;
					item2.PostDate = DateTime.Now;
					item2.EntryUser = UserAccount.ID;
					item2.ReqType = "AP";
					obj2.AllApprovalLists.Add(item2);
				}
				if (RequestFromScreen == "Purchase Invoice Approval")
				{
					InvPurchaseInvoice.InvPurchaseInvoiceandVoucherApprove(obj2);
				}
				else
				{
					InvSaleInvoice.InvSaleInvoiceandVoucherApprove(obj2);
				}
				MessageBox.Show("Record Approved Successfully");
				ListForApproveOrUnapprove = new List<int>();
				ApprovedCount.Text = "0";
				panel7.Hide();
				BtnLoadAll_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierName_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GoToCustomerLedgerFromLinkedEvent(Customer_Supplier_Id, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommAgentName_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GoToCustomerLedgerFromLinkedEvent(CommissionAgent_Id, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BrokerName_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GoToCustomerLedgerFromLinkedEvent(Broker_Id, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderNo_Click(object sender, EventArgs e)
	{
		try
		{
			if (DocumentTypeId == 56 || DocumentTypeId == 57 || DocumentTypeId == 172)
			{
				new ActivityDetails(UserAccount, orderId).Show();
			}
			else if (DocumentTypeId == 95 || DocumentTypeId == 99 || DocumentTypeId == 171)
			{
				new SaleActivityDetails(UserAccount, orderId).Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnEdit_Click(object sender, EventArgs e)
	{
		try
		{
			if (InvoiceIdForEdit > 0)
			{
				CommonServices.EditMethodFromLinked(DocumentTypeId, 0, InvoiceIdForEdit);
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPurchaseInvoiceSlip220_Click(object sender, EventArgs e)
	{
		try
		{
			if (InvoiceIdForEdit > 0)
			{
				GeneratereportSlip("220-InvRptPurchaseBillSupplierRiceSlip.rpt");
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneratereportSlip(string ReportName)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Reporting rpt = new Reporting();
			if (UserAccount.CompName.ToString() == string.Empty)
			{
				rpt.RptPerameter("@CompanyName", (string)null);
			}
			else
			{
				rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			}
			rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
			rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
			rpt.RptPerameter("@BranchesId", (string)null);
			rpt.RptPerameter("@FromDate", (string)null);
			rpt.RptPerameter("@ToDate", (string)null);
			rpt.RptPerameter("@FromDocNo", (string)null);
			rpt.RptPerameter("@ToDocNo", (string)null);
			rpt.RptPerameter("@SupplierCustomerId", (string)null);
			rpt.RptPerameter("@Id", InvoiceIdForEdit.ToString());
			rpt.RptPerameter("@PihId", InvoiceIdForEdit.ToString());
			rpt.ShowReport(ReportName);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPurchaseInvoiceSlip220A_Click(object sender, EventArgs e)
	{
		try
		{
			if (InvoiceIdForEdit > 0)
			{
				CommonServices.PurchaseInvoicettSlip_220A(InvoiceIdForEdit);
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPurchaseInvoice220bSummary_Click(object sender, EventArgs e)
	{
		try
		{
			if (InvoiceIdForEdit > 0)
			{
				GeneratereportSlip("220B-InvRptPurchaseBillSupplierRiceSummary.rpt");
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPurchaseSaleVoucherPrint_Click(object sender, EventArgs e)
	{
		try
		{
			if (InvoiceIdForEdit > 0)
			{
				GenerateReportVoucher103();
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReportVoucher103()
	{
		try
		{
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadIdForVoucherPrint, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn104Voucher_Click(object sender, EventArgs e)
	{
		try
		{
			if (InvoiceIdForEdit > 0)
			{
				GenerateReportVoucher104();
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReportVoucher104()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.VoucherSlipForInventoryReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				Id = VoucherHeadIdForVoucherPrint,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
			}
			else
			{
				new Reporting().ShowReportWithDataTable(dt, "104-AcRptGeneralJournalAcAndInventoryDetailSlip.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSaleInvoice301Slip_Click(object sender, EventArgs e)
	{
		try
		{
			if (InvoiceIdForEdit > 0)
			{
				CommonServices.SaleInvoicetSlip_301(InvoiceIdForEdit, Customer_Supplier_Id, Customer_Supplier_GlAccount, Conversion.ToDateTime((object)InvoiceDate), Conversion.ToDateTime((object)InvoiceDate));
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void BtnSaleInvoiceSlip303_Click(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		try
		{
			if (InvoiceIdForEdit > 0)
			{
				Reporting rpt = new Reporting();
				if (UserAccount.CompName.ToString() == string.Empty)
				{
					rpt.RptPerameter("CompanyName", string.Empty);
				}
				else
				{
					rpt.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				}
				rpt.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
				rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
				rpt.RptPerameter("@DocNoFrom", (string)null);
				rpt.RptPerameter("@DocNoTo", (string)null);
				rpt.RptPerameter("@PbmId", InvoiceIdForEdit.ToString());
				rpt.RptPerameter("@DocumentTypeId", "95");
				rpt.RptPerameter("@PihId", InvoiceIdForEdit.ToString());
				rpt.ShowReport("303-InvRepSaleBillCustomer-Format-II.rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void btnPIDirectSlip225_Click(object sender, EventArgs e)
	{
		try
		{
			if (InvoiceIdForEdit > 0)
			{
				CommonServices.PurchaseInvoicetDirectSlip_225(InvoiceIdForEdit);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPIDirect225PartySlip_Click(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (InvoiceIdForEdit > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
				val.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
				val.RptPerameter("@Id", InvoiceIdForEdit.ToString());
				val.RptPerameter("@PihId", InvoiceIdForEdit.ToString());
				val.ShowReport("225A-InvRepPurchaseBillDirectWithoutPo.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVouvher118_Click(object sender, EventArgs e)
	{
		try
		{
			if (InvoiceIdForEdit > 0)
			{
				CommonServices.VoucherReport_118(VoucherHeadIdForVoucherPrint, DocumentTypeId);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceNumber_Click(object sender, EventArgs e)
	{
		if (DocumentTypeId == 56 || DocumentTypeId == 57 || DocumentTypeId == 172)
		{
			new PurchaseInvoiceReverseActivity(UserAccount, InvoiceIdForEdit).Show();
		}
		else if (DocumentTypeId == 95 || DocumentTypeId == 99 || DocumentTypeId == 171)
		{
			new SaleInvoiceReverseActivity(UserAccount, InvoiceIdForEdit).Show();
		}
	}

	private void btn294Print_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SaleInvoiceDirectSlip_294("294-InvRptSaleBillDirectWithoutSO.rpt", InvoiceIdForEdit);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn294APrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SaleInvoiceDirectSlip_294("294A-InvRptSaleBillDirectWithoutSO.rpt", InvoiceIdForEdit);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(InvoiceIdForEdit, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCheckAll_Click(object sender, EventArgs e)
	{
		try
		{
			if (!(RequestFromScreen == "Purchase Invoice Approval") && !(RequestFromScreen == "Sale Invoice Approval"))
			{
				return;
			}
			ListForApproveOrUnapprove.Clear();
			foreach (Control control in HeaderFlowLayoutPanel.Controls)
			{
				if (control is InvoiceHeaderCard invoiceHeaderCard)
				{
					invoiceHeaderCard.CbApprove.Checked = true;
					ListForApproveOrUnapprove.Add(invoiceHeaderCard.InvoiceId);
					ApprovedCount.Text = Conversion.ToString((object)ListForApproveOrUnapprove.Count);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadDefault_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateHeaderCardDataOnLoad();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ModernHistories.InvoiceHistory));
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel3 = new System.Windows.Forms.Panel();
		this.CaptionForInvoiceTypeHistory = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel22 = new System.Windows.Forms.Panel();
		this.panel24 = new System.Windows.Forms.Panel();
		this.label43 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		this.label41 = new System.Windows.Forms.Label();
		this.GrandTotalParentPanel = new System.Windows.Forms.Panel();
		this.WagesPanel = new System.Windows.Forms.Panel();
		this.WagesFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.label25 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label58 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.GrandTotalPanel = new System.Windows.Forms.Panel();
		this.panel23 = new System.Windows.Forms.Panel();
		this.GrandTotal = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.panelCommissionAmountInGrandTotal = new System.Windows.Forms.Panel();
		this.CommissionAmountInTotal = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.panelEmptyBag = new System.Windows.Forms.Panel();
		this.EmptyBagAmountInTotal = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.panelChargeToProductTotalAmount = new System.Windows.Forms.Panel();
		this.ChargeToProductAmountInTotals = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.panelSupplierAddlessTotalAmount = new System.Windows.Forms.Panel();
		this.SupplierAddlessAmountInTotals = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.panelBrokeryTotalAmount = new System.Windows.Forms.Panel();
		this.BrokeryAmount = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.PanelOtherExpenseTotalAmount = new System.Windows.Forms.Panel();
		this.OtherExpensesInGrandTotal = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.PanelItemTotalAmount = new System.Windows.Forms.Panel();
		this.LblItemTotalInTotals = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.OtherInformationPanel = new System.Windows.Forms.Panel();
		this.OtherInfoFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.ChargeToProductPanel = new System.Windows.Forms.Panel();
		this.FlowLayoutCTP = new System.Windows.Forms.FlowLayoutPanel();
		this.label14 = new System.Windows.Forms.Label();
		this.CTPHeaderPanel = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.SupplierAddlessPanel = new System.Windows.Forms.Panel();
		this.FlowLayoutSAL = new System.Windows.Forms.FlowLayoutPanel();
		this.label24 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label31 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.OtherExpensePanel = new System.Windows.Forms.Panel();
		this.FlowLayoutOE = new System.Windows.Forms.FlowLayoutPanel();
		this.label44 = new System.Windows.Forms.Label();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label45 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.EmptyBagPanel = new System.Windows.Forms.Panel();
		this.FlowLayoutEBD = new System.Windows.Forms.FlowLayoutPanel();
		this.label48 = new System.Windows.Forms.Label();
		this.panel26 = new System.Windows.Forms.Panel();
		this.label49 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.lblTotalAmount = new System.Windows.Forms.Label();
		this.lblTotalNetWeight = new System.Windows.Forms.Label();
		this.lblTotalQuantity = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.label19 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.BrokerPanel = new System.Windows.Forms.Panel();
		this.ValueBrokeryRateUom = new System.Windows.Forms.Label();
		this.lblBrokeryRateUom = new System.Windows.Forms.Label();
		this.ValueBrokeryType = new System.Windows.Forms.Label();
		this.label69 = new System.Windows.Forms.Label();
		this.BrokerAmount = new System.Windows.Forms.Label();
		this.BrokerPercentRate = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.BrokerName = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.CommissionAgentPanel = new System.Windows.Forms.Panel();
		this.ValueCommRateUom = new System.Windows.Forms.Label();
		this.lblCommRateUom = new System.Windows.Forms.Label();
		this.ValueCommType = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.CommAmount = new System.Windows.Forms.Label();
		this.CommPercentRate = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.CommAgentName = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.SupplierPanel = new System.Windows.Forms.Panel();
		this.Address = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.Email = new System.Windows.Forms.Label();
		this.PhoneNo = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.SupplierName = new System.Windows.Forms.Label();
		this.lblSupplierOrCustomer = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.OrderNo = new System.Windows.Forms.Label();
		this.InvoiceDueDate = new System.Windows.Forms.Label();
		this.InvoiceDate = new System.Windows.Forms.Label();
		this.lblOrder = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.InvoiceNumber = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.lblCompanyAddress = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.lblCompanyName = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.HeaderFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.TotalRecordPanel = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.TotalHeaderRecords = new System.Windows.Forms.Label();
		this.FilterPanel = new System.Windows.Forms.Panel();
		this.label64 = new System.Windows.Forms.Label();
		this.cmbSupplierName = new UltraCombo();
		this.txtNumberTo = new System.Windows.Forms.TextBox();
		this.txtNoFrom = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.DateTo = new System.Windows.Forms.DateTimePicker();
		this.label61 = new System.Windows.Forms.Label();
		this.DateFrom = new System.Windows.Forms.DateTimePicker();
		this.panel19 = new System.Windows.Forms.Panel();
		this.FilterRecords = new System.Windows.Forms.Button();
		this.FilterBoxIcon = new System.Windows.Forms.PictureBox();
		this.HandleVisibilityOfFiletrPanel = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.BtnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnCheckAll = new System.Windows.Forms.ToolStripButton();
		this.btnApprove = new System.Windows.Forms.ToolStripButton();
		this.ApprovedCount = new System.Windows.Forms.ToolStripLabel();
		this.btnEdit = new System.Windows.Forms.ToolStripButton();
		this.btnPurchaseInvoiceSlip220 = new System.Windows.Forms.ToolStripButton();
		this.btnPurchaseInvoiceSlip220A = new System.Windows.Forms.ToolStripButton();
		this.btnPurchaseInvoice220bSummary = new System.Windows.Forms.ToolStripButton();
		this.btnPurchaseSaleVoucherPrint = new System.Windows.Forms.ToolStripButton();
		this.btn104Voucher = new System.Windows.Forms.ToolStripButton();
		this.BtnSaleInvoice301Slip = new System.Windows.Forms.ToolStripButton();
		this.BtnSaleInvoiceSlip303 = new System.Windows.Forms.ToolStripButton();
		this.btnPIDirectSlip225 = new System.Windows.Forms.ToolStripButton();
		this.btnPIDirect225PartySlip = new System.Windows.Forms.ToolStripButton();
		this.btnVouvher118 = new System.Windows.Forms.ToolStripButton();
		this.btn294Print = new System.Windows.Forms.ToolStripButton();
		this.btn294APrint = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadDefault = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel22.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		this.GrandTotalParentPanel.SuspendLayout();
		this.WagesPanel.SuspendLayout();
		this.panel16.SuspendLayout();
		this.GrandTotalPanel.SuspendLayout();
		this.panel23.SuspendLayout();
		this.panelCommissionAmountInGrandTotal.SuspendLayout();
		this.panelEmptyBag.SuspendLayout();
		this.panelChargeToProductTotalAmount.SuspendLayout();
		this.panelSupplierAddlessTotalAmount.SuspendLayout();
		this.panelBrokeryTotalAmount.SuspendLayout();
		this.PanelOtherExpenseTotalAmount.SuspendLayout();
		this.PanelItemTotalAmount.SuspendLayout();
		this.OtherInformationPanel.SuspendLayout();
		this.OtherInfoFlowLayout.SuspendLayout();
		this.ChargeToProductPanel.SuspendLayout();
		this.CTPHeaderPanel.SuspendLayout();
		this.SupplierAddlessPanel.SuspendLayout();
		this.panel17.SuspendLayout();
		this.OtherExpensePanel.SuspendLayout();
		this.panel21.SuspendLayout();
		this.EmptyBagPanel.SuspendLayout();
		this.panel26.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel11.SuspendLayout();
		this.BrokerPanel.SuspendLayout();
		this.CommissionAgentPanel.SuspendLayout();
		this.SupplierPanel.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.panel5.SuspendLayout();
		this.TotalRecordPanel.SuspendLayout();
		this.FilterPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).BeginInit();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.FilterBoxIcon).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.CaptionForInvoiceTypeHistory);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1406, 28);
		this.panel3.TabIndex = 4;
		this.CaptionForInvoiceTypeHistory.AutoSize = true;
		this.CaptionForInvoiceTypeHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.CaptionForInvoiceTypeHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CaptionForInvoiceTypeHistory.ForeColor = System.Drawing.Color.White;
		this.CaptionForInvoiceTypeHistory.Location = new System.Drawing.Point(4, 2);
		this.CaptionForInvoiceTypeHistory.Name = "CaptionForInvoiceTypeHistory";
		this.CaptionForInvoiceTypeHistory.Size = new System.Drawing.Size(217, 24);
		this.CaptionForInvoiceTypeHistory.TabIndex = 1;
		this.CaptionForInvoiceTypeHistory.Text = "Purchase Invoice History";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 55);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1406, 756);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Controls.Add(this.panel5);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1406, 756);
		this.panel6.TabIndex = 110;
		this.panel7.AutoScroll = true;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.panel22);
		this.panel7.Controls.Add(this.GrandTotalParentPanel);
		this.panel7.Controls.Add(this.OtherInformationPanel);
		this.panel7.Controls.Add(this.panel14);
		this.panel7.Controls.Add(this.panel15);
		this.panel7.Controls.Add(this.panel11);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(347, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1059, 756);
		this.panel7.TabIndex = 2;
		this.panel22.Controls.Add(this.panel24);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel22.Location = new System.Drawing.Point(0, 1111);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1042, 46);
		this.panel22.TabIndex = 11;
		this.panel24.BackColor = System.Drawing.Color.Teal;
		this.panel24.Controls.Add(this.label43);
		this.panel24.Controls.Add(this.label42);
		this.panel24.Controls.Add(this.pictureBox2);
		this.panel24.Controls.Add(this.label41);
		this.panel24.Location = new System.Drawing.Point(0, -1);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(974, 42);
		this.panel24.TabIndex = 0;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(778, 13);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(187, 17);
		this.label43.TabIndex = 3;
		this.label43.Text = "Synergic Corporate Solutions";
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.White;
		this.label42.Location = new System.Drawing.Point(702, 13);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(84, 24);
		this.label42.TabIndex = 2;
		this.label42.Text = "Powered By ";
		this.pictureBox2.Image = (System.Drawing.Image)resources.GetObject("pictureBox2.Image");
		this.pictureBox2.Location = new System.Drawing.Point(146, 2);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(50, 40);
		this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox2.TabIndex = 1;
		this.pictureBox2.TabStop = false;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.White;
		this.label41.Location = new System.Drawing.Point(21, 6);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(133, 32);
		this.label41.TabIndex = 0;
		this.label41.Text = "Thank You";
		this.GrandTotalParentPanel.Controls.Add(this.WagesPanel);
		this.GrandTotalParentPanel.Controls.Add(this.GrandTotalPanel);
		this.GrandTotalParentPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.GrandTotalParentPanel.Location = new System.Drawing.Point(0, 877);
		this.GrandTotalParentPanel.Name = "GrandTotalParentPanel";
		this.GrandTotalParentPanel.Size = new System.Drawing.Size(1042, 234);
		this.GrandTotalParentPanel.TabIndex = 9;
		this.WagesPanel.Controls.Add(this.WagesFlowLayout);
		this.WagesPanel.Controls.Add(this.label25);
		this.WagesPanel.Controls.Add(this.panel16);
		this.WagesPanel.Location = new System.Drawing.Point(1, 1);
		this.WagesPanel.Name = "WagesPanel";
		this.WagesPanel.Size = new System.Drawing.Size(647, 132);
		this.WagesPanel.TabIndex = 1;
		this.WagesFlowLayout.AutoScroll = true;
		this.WagesFlowLayout.Location = new System.Drawing.Point(2, 53);
		this.WagesFlowLayout.Name = "WagesFlowLayout";
		this.WagesFlowLayout.Size = new System.Drawing.Size(644, 79);
		this.WagesFlowLayout.TabIndex = 8;
		this.label25.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label25.Enabled = false;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.White;
		this.label25.Location = new System.Drawing.Point(1, 1);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(123, 27);
		this.label25.TabIndex = 6;
		this.label25.Text = "Wages Detail";
		this.panel16.BackColor = System.Drawing.Color.LightSeaGreen;
		this.panel16.Controls.Add(this.label58);
		this.panel16.Controls.Add(this.label57);
		this.panel16.Controls.Add(this.label56);
		this.panel16.Controls.Add(this.label55);
		this.panel16.Controls.Add(this.label52);
		this.panel16.Controls.Add(this.label53);
		this.panel16.Controls.Add(this.label54);
		this.panel16.Location = new System.Drawing.Point(1, 28);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(647, 25);
		this.panel16.TabIndex = 7;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(584, 5);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(58, 17);
		this.label58.TabIndex = 6;
		this.label58.Text = "Amount";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.White;
		this.label57.Location = new System.Drawing.Point(534, 5);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(35, 17);
		this.label57.TabIndex = 5;
		this.label57.Text = "Rate";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.Color.White;
		this.label56.Location = new System.Drawing.Point(462, 5);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(64, 17);
		this.label56.TabIndex = 4;
		this.label56.Text = "Pack Size";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(397, 5);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(53, 17);
		this.label55.TabIndex = 3;
		this.label55.Text = "Weight";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.White;
		this.label52.Location = new System.Drawing.Point(338, 5);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(30, 17);
		this.label52.TabIndex = 2;
		this.label52.Text = "Qty";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.White;
		this.label53.Location = new System.Drawing.Point(205, 5);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(102, 17);
		this.label53.TabIndex = 1;
		this.label53.Text = "Wages Account";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(3, 5);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(113, 17);
		this.label54.TabIndex = 0;
		this.label54.Text = "Contractor Name";
		this.GrandTotalPanel.BackColor = System.Drawing.Color.White;
		this.GrandTotalPanel.Controls.Add(this.panel23);
		this.GrandTotalPanel.Controls.Add(this.panelCommissionAmountInGrandTotal);
		this.GrandTotalPanel.Controls.Add(this.panelEmptyBag);
		this.GrandTotalPanel.Controls.Add(this.panelChargeToProductTotalAmount);
		this.GrandTotalPanel.Controls.Add(this.panelSupplierAddlessTotalAmount);
		this.GrandTotalPanel.Controls.Add(this.panelBrokeryTotalAmount);
		this.GrandTotalPanel.Controls.Add(this.PanelOtherExpenseTotalAmount);
		this.GrandTotalPanel.Controls.Add(this.PanelItemTotalAmount);
		this.GrandTotalPanel.Location = new System.Drawing.Point(654, 3);
		this.GrandTotalPanel.Name = "GrandTotalPanel";
		this.GrandTotalPanel.Size = new System.Drawing.Size(319, 228);
		this.GrandTotalPanel.TabIndex = 6;
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.GrandTotal);
		this.panel23.Controls.Add(this.label40);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 182);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(319, 50);
		this.panel23.TabIndex = 14;
		this.GrandTotal.AutoSize = true;
		this.GrandTotal.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GrandTotal.ForeColor = System.Drawing.Color.White;
		this.GrandTotal.Location = new System.Drawing.Point(168, 8);
		this.GrandTotal.Name = "GrandTotal";
		this.GrandTotal.Size = new System.Drawing.Size(23, 25);
		this.GrandTotal.TabIndex = 5;
		this.GrandTotal.Text = "0";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.White;
		this.label40.Location = new System.Drawing.Point(14, 8);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(121, 25);
		this.label40.TabIndex = 4;
		this.label40.Text = "Grand Total:";
		this.panelCommissionAmountInGrandTotal.Controls.Add(this.CommissionAmountInTotal);
		this.panelCommissionAmountInGrandTotal.Controls.Add(this.label37);
		this.panelCommissionAmountInGrandTotal.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelCommissionAmountInGrandTotal.Location = new System.Drawing.Point(0, 156);
		this.panelCommissionAmountInGrandTotal.Name = "panelCommissionAmountInGrandTotal";
		this.panelCommissionAmountInGrandTotal.Size = new System.Drawing.Size(319, 26);
		this.panelCommissionAmountInGrandTotal.TabIndex = 13;
		this.CommissionAmountInTotal.AutoSize = true;
		this.CommissionAmountInTotal.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CommissionAmountInTotal.Location = new System.Drawing.Point(170, 4);
		this.CommissionAmountInTotal.Name = "CommissionAmountInTotal";
		this.CommissionAmountInTotal.Size = new System.Drawing.Size(15, 17);
		this.CommissionAmountInTotal.TabIndex = 3;
		this.CommissionAmountInTotal.Text = "0";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(10, 4);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(142, 17);
		this.label37.TabIndex = 2;
		this.label37.Text = "Commission Amount:";
		this.panelEmptyBag.Controls.Add(this.EmptyBagAmountInTotal);
		this.panelEmptyBag.Controls.Add(this.label33);
		this.panelEmptyBag.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelEmptyBag.Location = new System.Drawing.Point(0, 130);
		this.panelEmptyBag.Name = "panelEmptyBag";
		this.panelEmptyBag.Size = new System.Drawing.Size(319, 26);
		this.panelEmptyBag.TabIndex = 12;
		this.EmptyBagAmountInTotal.AutoSize = true;
		this.EmptyBagAmountInTotal.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.EmptyBagAmountInTotal.Location = new System.Drawing.Point(170, 4);
		this.EmptyBagAmountInTotal.Name = "EmptyBagAmountInTotal";
		this.EmptyBagAmountInTotal.Size = new System.Drawing.Size(15, 17);
		this.EmptyBagAmountInTotal.TabIndex = 3;
		this.EmptyBagAmountInTotal.Text = "0";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(10, 4);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(78, 17);
		this.label33.TabIndex = 2;
		this.label33.Text = "Empty Bag:";
		this.panelChargeToProductTotalAmount.Controls.Add(this.ChargeToProductAmountInTotals);
		this.panelChargeToProductTotalAmount.Controls.Add(this.label38);
		this.panelChargeToProductTotalAmount.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelChargeToProductTotalAmount.Location = new System.Drawing.Point(0, 104);
		this.panelChargeToProductTotalAmount.Name = "panelChargeToProductTotalAmount";
		this.panelChargeToProductTotalAmount.Size = new System.Drawing.Size(319, 26);
		this.panelChargeToProductTotalAmount.TabIndex = 11;
		this.ChargeToProductAmountInTotals.AutoSize = true;
		this.ChargeToProductAmountInTotals.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChargeToProductAmountInTotals.Location = new System.Drawing.Point(170, 4);
		this.ChargeToProductAmountInTotals.Name = "ChargeToProductAmountInTotals";
		this.ChargeToProductAmountInTotals.Size = new System.Drawing.Size(15, 17);
		this.ChargeToProductAmountInTotals.TabIndex = 3;
		this.ChargeToProductAmountInTotals.Text = "0";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(10, 4);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(130, 17);
		this.label38.TabIndex = 2;
		this.label38.Text = "Charge To Product: ";
		this.panelSupplierAddlessTotalAmount.Controls.Add(this.SupplierAddlessAmountInTotals);
		this.panelSupplierAddlessTotalAmount.Controls.Add(this.label36);
		this.panelSupplierAddlessTotalAmount.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelSupplierAddlessTotalAmount.Location = new System.Drawing.Point(0, 78);
		this.panelSupplierAddlessTotalAmount.Name = "panelSupplierAddlessTotalAmount";
		this.panelSupplierAddlessTotalAmount.Size = new System.Drawing.Size(319, 26);
		this.panelSupplierAddlessTotalAmount.TabIndex = 10;
		this.SupplierAddlessAmountInTotals.AutoSize = true;
		this.SupplierAddlessAmountInTotals.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SupplierAddlessAmountInTotals.Location = new System.Drawing.Point(170, 4);
		this.SupplierAddlessAmountInTotals.Name = "SupplierAddlessAmountInTotals";
		this.SupplierAddlessAmountInTotals.Size = new System.Drawing.Size(15, 17);
		this.SupplierAddlessAmountInTotals.TabIndex = 3;
		this.SupplierAddlessAmountInTotals.Text = "0";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(10, 4);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(124, 17);
		this.label36.TabIndex = 2;
		this.label36.Text = "Supplier Add/Less:";
		this.panelBrokeryTotalAmount.Controls.Add(this.BrokeryAmount);
		this.panelBrokeryTotalAmount.Controls.Add(this.label34);
		this.panelBrokeryTotalAmount.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelBrokeryTotalAmount.Location = new System.Drawing.Point(0, 52);
		this.panelBrokeryTotalAmount.Name = "panelBrokeryTotalAmount";
		this.panelBrokeryTotalAmount.Size = new System.Drawing.Size(319, 26);
		this.panelBrokeryTotalAmount.TabIndex = 9;
		this.BrokeryAmount.AutoSize = true;
		this.BrokeryAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BrokeryAmount.Location = new System.Drawing.Point(170, 4);
		this.BrokeryAmount.Name = "BrokeryAmount";
		this.BrokeryAmount.Size = new System.Drawing.Size(15, 17);
		this.BrokeryAmount.TabIndex = 3;
		this.BrokeryAmount.Text = "0";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(10, 4);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(106, 17);
		this.label34.TabIndex = 2;
		this.label34.Text = "Brokery Amout:";
		this.PanelOtherExpenseTotalAmount.Controls.Add(this.OtherExpensesInGrandTotal);
		this.PanelOtherExpenseTotalAmount.Controls.Add(this.label32);
		this.PanelOtherExpenseTotalAmount.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelOtherExpenseTotalAmount.Location = new System.Drawing.Point(0, 26);
		this.PanelOtherExpenseTotalAmount.Name = "PanelOtherExpenseTotalAmount";
		this.PanelOtherExpenseTotalAmount.Size = new System.Drawing.Size(319, 26);
		this.PanelOtherExpenseTotalAmount.TabIndex = 8;
		this.OtherExpensesInGrandTotal.AutoSize = true;
		this.OtherExpensesInGrandTotal.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OtherExpensesInGrandTotal.Location = new System.Drawing.Point(170, 4);
		this.OtherExpensesInGrandTotal.Name = "OtherExpensesInGrandTotal";
		this.OtherExpensesInGrandTotal.Size = new System.Drawing.Size(15, 17);
		this.OtherExpensesInGrandTotal.TabIndex = 3;
		this.OtherExpensesInGrandTotal.Text = "0";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(10, 4);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(107, 17);
		this.label32.TabIndex = 2;
		this.label32.Text = "Other Expenses:";
		this.PanelItemTotalAmount.Controls.Add(this.LblItemTotalInTotals);
		this.PanelItemTotalAmount.Controls.Add(this.label28);
		this.PanelItemTotalAmount.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelItemTotalAmount.Location = new System.Drawing.Point(0, 0);
		this.PanelItemTotalAmount.Name = "PanelItemTotalAmount";
		this.PanelItemTotalAmount.Padding = new System.Windows.Forms.Padding(3);
		this.PanelItemTotalAmount.Size = new System.Drawing.Size(319, 26);
		this.PanelItemTotalAmount.TabIndex = 7;
		this.LblItemTotalInTotals.AutoSize = true;
		this.LblItemTotalInTotals.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.LblItemTotalInTotals.Location = new System.Drawing.Point(170, 4);
		this.LblItemTotalInTotals.Name = "LblItemTotalInTotals";
		this.LblItemTotalInTotals.Size = new System.Drawing.Size(15, 17);
		this.LblItemTotalInTotals.TabIndex = 1;
		this.LblItemTotalInTotals.Text = "0";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(10, 4);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(86, 17);
		this.label28.TabIndex = 0;
		this.label28.Text = "Item Amout:";
		this.OtherInformationPanel.Controls.Add(this.OtherInfoFlowLayout);
		this.OtherInformationPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.OtherInformationPanel.Location = new System.Drawing.Point(0, 502);
		this.OtherInformationPanel.Name = "OtherInformationPanel";
		this.OtherInformationPanel.Size = new System.Drawing.Size(1042, 375);
		this.OtherInformationPanel.TabIndex = 8;
		this.OtherInfoFlowLayout.Controls.Add(this.ChargeToProductPanel);
		this.OtherInfoFlowLayout.Controls.Add(this.SupplierAddlessPanel);
		this.OtherInfoFlowLayout.Controls.Add(this.OtherExpensePanel);
		this.OtherInfoFlowLayout.Controls.Add(this.EmptyBagPanel);
		this.OtherInfoFlowLayout.Location = new System.Drawing.Point(0, 0);
		this.OtherInfoFlowLayout.Name = "OtherInfoFlowLayout";
		this.OtherInfoFlowLayout.Size = new System.Drawing.Size(973, 372);
		this.OtherInfoFlowLayout.TabIndex = 0;
		this.ChargeToProductPanel.Controls.Add(this.FlowLayoutCTP);
		this.ChargeToProductPanel.Controls.Add(this.label14);
		this.ChargeToProductPanel.Controls.Add(this.CTPHeaderPanel);
		this.ChargeToProductPanel.Location = new System.Drawing.Point(3, 3);
		this.ChargeToProductPanel.Name = "ChargeToProductPanel";
		this.ChargeToProductPanel.Size = new System.Drawing.Size(474, 179);
		this.ChargeToProductPanel.TabIndex = 5;
		this.FlowLayoutCTP.AutoScroll = true;
		this.FlowLayoutCTP.Location = new System.Drawing.Point(1, 52);
		this.FlowLayoutCTP.Name = "FlowLayoutCTP";
		this.FlowLayoutCTP.Size = new System.Drawing.Size(473, 127);
		this.FlowLayoutCTP.TabIndex = 2;
		this.label14.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label14.Enabled = false;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(1, 2);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(160, 25);
		this.label14.TabIndex = 0;
		this.label14.Text = "Charge To Product";
		this.CTPHeaderPanel.BackColor = System.Drawing.Color.LightSeaGreen;
		this.CTPHeaderPanel.Controls.Add(this.label22);
		this.CTPHeaderPanel.Controls.Add(this.label16);
		this.CTPHeaderPanel.Controls.Add(this.label11);
		this.CTPHeaderPanel.Location = new System.Drawing.Point(1, 26);
		this.CTPHeaderPanel.Name = "CTPHeaderPanel";
		this.CTPHeaderPanel.Size = new System.Drawing.Size(473, 27);
		this.CTPHeaderPanel.TabIndex = 1;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(273, 6);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(60, 17);
		this.label22.TabIndex = 2;
		this.label22.Text = "Remarks";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(190, 6);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(58, 17);
		this.label16.TabIndex = 1;
		this.label16.Text = "Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(9, 4);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(98, 17);
		this.label11.TabIndex = 0;
		this.label11.Text = "Account Name";
		this.SupplierAddlessPanel.Controls.Add(this.FlowLayoutSAL);
		this.SupplierAddlessPanel.Controls.Add(this.label24);
		this.SupplierAddlessPanel.Controls.Add(this.panel17);
		this.SupplierAddlessPanel.Location = new System.Drawing.Point(483, 3);
		this.SupplierAddlessPanel.Name = "SupplierAddlessPanel";
		this.SupplierAddlessPanel.Size = new System.Drawing.Size(474, 179);
		this.SupplierAddlessPanel.TabIndex = 6;
		this.FlowLayoutSAL.AutoScroll = true;
		this.FlowLayoutSAL.Location = new System.Drawing.Point(1, 52);
		this.FlowLayoutSAL.Name = "FlowLayoutSAL";
		this.FlowLayoutSAL.Size = new System.Drawing.Size(473, 127);
		this.FlowLayoutSAL.TabIndex = 2;
		this.label24.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label24.Enabled = false;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(1, 2);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(160, 25);
		this.label24.TabIndex = 0;
		this.label24.Text = "Supplier Add/Lass";
		this.panel17.BackColor = System.Drawing.Color.LightSeaGreen;
		this.panel17.Controls.Add(this.label31);
		this.panel17.Controls.Add(this.label35);
		this.panel17.Controls.Add(this.label39);
		this.panel17.Location = new System.Drawing.Point(1, 26);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(473, 27);
		this.panel17.TabIndex = 1;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.White;
		this.label31.Location = new System.Drawing.Point(393, 6);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(45, 17);
		this.label31.TabIndex = 2;
		this.label31.Text = "Credit";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.White;
		this.label35.Location = new System.Drawing.Point(294, 6);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(42, 17);
		this.label35.TabIndex = 1;
		this.label35.Text = "Debit";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.White;
		this.label39.Location = new System.Drawing.Point(9, 4);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(98, 17);
		this.label39.TabIndex = 0;
		this.label39.Text = "Account Name";
		this.OtherExpensePanel.Controls.Add(this.FlowLayoutOE);
		this.OtherExpensePanel.Controls.Add(this.label44);
		this.OtherExpensePanel.Controls.Add(this.panel21);
		this.OtherExpensePanel.Location = new System.Drawing.Point(3, 188);
		this.OtherExpensePanel.Name = "OtherExpensePanel";
		this.OtherExpensePanel.Size = new System.Drawing.Size(474, 179);
		this.OtherExpensePanel.TabIndex = 6;
		this.FlowLayoutOE.AutoScroll = true;
		this.FlowLayoutOE.Location = new System.Drawing.Point(1, 52);
		this.FlowLayoutOE.Name = "FlowLayoutOE";
		this.FlowLayoutOE.Size = new System.Drawing.Size(473, 127);
		this.FlowLayoutOE.TabIndex = 2;
		this.label44.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label44.Enabled = false;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.ForeColor = System.Drawing.Color.White;
		this.label44.Location = new System.Drawing.Point(1, 2);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(160, 25);
		this.label44.TabIndex = 0;
		this.label44.Text = "Other Expenses";
		this.panel21.BackColor = System.Drawing.Color.LightSeaGreen;
		this.panel21.Controls.Add(this.label45);
		this.panel21.Controls.Add(this.label46);
		this.panel21.Controls.Add(this.label47);
		this.panel21.Location = new System.Drawing.Point(1, 26);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(473, 27);
		this.panel21.TabIndex = 1;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.White;
		this.label45.Location = new System.Drawing.Point(271, 6);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(60, 17);
		this.label45.TabIndex = 2;
		this.label45.Text = "Remarks";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.White;
		this.label46.Location = new System.Drawing.Point(185, 6);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(58, 17);
		this.label46.TabIndex = 1;
		this.label46.Text = "Amount";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.White;
		this.label47.Location = new System.Drawing.Point(9, 4);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(76, 17);
		this.label47.TabIndex = 0;
		this.label47.Text = "Item Name";
		this.EmptyBagPanel.Controls.Add(this.FlowLayoutEBD);
		this.EmptyBagPanel.Controls.Add(this.label48);
		this.EmptyBagPanel.Controls.Add(this.panel26);
		this.EmptyBagPanel.Location = new System.Drawing.Point(483, 188);
		this.EmptyBagPanel.Name = "EmptyBagPanel";
		this.EmptyBagPanel.Size = new System.Drawing.Size(474, 179);
		this.EmptyBagPanel.TabIndex = 7;
		this.FlowLayoutEBD.AutoScroll = true;
		this.FlowLayoutEBD.Location = new System.Drawing.Point(1, 52);
		this.FlowLayoutEBD.Name = "FlowLayoutEBD";
		this.FlowLayoutEBD.Size = new System.Drawing.Size(473, 127);
		this.FlowLayoutEBD.TabIndex = 2;
		this.label48.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label48.Enabled = false;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.ForeColor = System.Drawing.Color.White;
		this.label48.Location = new System.Drawing.Point(1, 2);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(160, 25);
		this.label48.TabIndex = 0;
		this.label48.Text = "Empty Bags Detail";
		this.panel26.BackColor = System.Drawing.Color.LightSeaGreen;
		this.panel26.Controls.Add(this.label49);
		this.panel26.Controls.Add(this.label50);
		this.panel26.Controls.Add(this.label51);
		this.panel26.Location = new System.Drawing.Point(1, 26);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(473, 27);
		this.panel26.TabIndex = 1;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.White;
		this.label49.Location = new System.Drawing.Point(271, 6);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(60, 17);
		this.label49.TabIndex = 2;
		this.label49.Text = "Remarks";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.White;
		this.label50.Location = new System.Drawing.Point(185, 6);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(58, 17);
		this.label50.TabIndex = 1;
		this.label50.Text = "Amount";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(9, 4);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(76, 17);
		this.label51.TabIndex = 0;
		this.label51.Text = "Item Name";
		this.panel14.Controls.Add(this.panel13);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 474);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1042, 28);
		this.panel14.TabIndex = 7;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label12);
		this.panel13.Controls.Add(this.lblTotalAmount);
		this.panel13.Controls.Add(this.lblTotalNetWeight);
		this.panel13.Controls.Add(this.lblTotalQuantity);
		this.panel13.Location = new System.Drawing.Point(1, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(972, 28);
		this.panel13.TabIndex = 3;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(47, 4);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(96, 20);
		this.label12.TabIndex = 8;
		this.label12.Text = "Detail Totals";
		this.lblTotalAmount.AutoSize = true;
		this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalAmount.ForeColor = System.Drawing.Color.White;
		this.lblTotalAmount.Location = new System.Drawing.Point(874, 4);
		this.lblTotalAmount.Name = "lblTotalAmount";
		this.lblTotalAmount.Size = new System.Drawing.Size(18, 20);
		this.lblTotalAmount.TabIndex = 8;
		this.lblTotalAmount.Text = "0";
		this.lblTotalNetWeight.AutoSize = true;
		this.lblTotalNetWeight.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalNetWeight.ForeColor = System.Drawing.Color.White;
		this.lblTotalNetWeight.Location = new System.Drawing.Point(682, 4);
		this.lblTotalNetWeight.Name = "lblTotalNetWeight";
		this.lblTotalNetWeight.Size = new System.Drawing.Size(18, 20);
		this.lblTotalNetWeight.TabIndex = 8;
		this.lblTotalNetWeight.Text = "0";
		this.lblTotalQuantity.AutoSize = true;
		this.lblTotalQuantity.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalQuantity.ForeColor = System.Drawing.Color.White;
		this.lblTotalQuantity.Location = new System.Drawing.Point(524, 4);
		this.lblTotalQuantity.Name = "lblTotalQuantity";
		this.lblTotalQuantity.Size = new System.Drawing.Size(18, 20);
		this.lblTotalQuantity.TabIndex = 4;
		this.lblTotalQuantity.Text = "0";
		this.panel15.Controls.Add(this.flowLayoutPanel);
		this.panel15.Controls.Add(this.panel12);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 248);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1042, 226);
		this.panel15.TabIndex = 2;
		this.flowLayoutPanel.AutoScroll = true;
		this.flowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.flowLayoutPanel.Location = new System.Drawing.Point(0, 34);
		this.flowLayoutPanel.Name = "flowLayoutPanel";
		this.flowLayoutPanel.Size = new System.Drawing.Size(973, 196);
		this.flowLayoutPanel.TabIndex = 9;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label7);
		this.panel12.Controls.Add(this.label20);
		this.panel12.Controls.Add(this.button1);
		this.panel12.Controls.Add(this.label19);
		this.panel12.Controls.Add(this.label2);
		this.panel12.Controls.Add(this.label18);
		this.panel12.Controls.Add(this.label15);
		this.panel12.Controls.Add(this.label17);
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(973, 35);
		this.panel12.TabIndex = 8;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(358, 7);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(87, 20);
		this.label7.TabIndex = 2;
		this.label7.Text = "Warehouse";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.White;
		this.label20.Location = new System.Drawing.Point(870, 7);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(67, 20);
		this.label20.TabIndex = 7;
		this.label20.Text = "Amount";
		this.button1.BackColor = System.Drawing.Color.White;
		this.button1.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.Teal;
		this.button1.Location = new System.Drawing.Point(7, 4);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(25, 24);
		this.button1.TabIndex = 0;
		this.button1.Text = "+";
		this.button1.UseVisualStyleBackColor = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(793, 7);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(41, 20);
		this.label19.TabIndex = 6;
		this.label19.Text = "Rate";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(47, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(88, 20);
		this.label2.TabIndex = 1;
		this.label2.Text = "Item Name";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(671, 7);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(90, 20);
		this.label18.TabIndex = 5;
		this.label18.Text = "Net Weight";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(524, 7);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(34, 20);
		this.label15.TabIndex = 3;
		this.label15.Text = "Qty";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(596, 7);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(43, 20);
		this.label17.TabIndex = 4;
		this.label17.Text = "Uom";
		this.panel11.Controls.Add(this.BrokerPanel);
		this.panel11.Controls.Add(this.CommissionAgentPanel);
		this.panel11.Controls.Add(this.SupplierPanel);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 81);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1042, 167);
		this.panel11.TabIndex = 1;
		this.BrokerPanel.Controls.Add(this.ValueBrokeryRateUom);
		this.BrokerPanel.Controls.Add(this.lblBrokeryRateUom);
		this.BrokerPanel.Controls.Add(this.ValueBrokeryType);
		this.BrokerPanel.Controls.Add(this.label69);
		this.BrokerPanel.Controls.Add(this.BrokerAmount);
		this.BrokerPanel.Controls.Add(this.BrokerPercentRate);
		this.BrokerPanel.Controls.Add(this.label29);
		this.BrokerPanel.Controls.Add(this.label30);
		this.BrokerPanel.Controls.Add(this.BrokerName);
		this.BrokerPanel.Controls.Add(this.label13);
		this.BrokerPanel.Dock = System.Windows.Forms.DockStyle.Left;
		this.BrokerPanel.Location = new System.Drawing.Point(639, 0);
		this.BrokerPanel.Name = "BrokerPanel";
		this.BrokerPanel.Size = new System.Drawing.Size(334, 167);
		this.BrokerPanel.TabIndex = 2;
		this.ValueBrokeryRateUom.AutoSize = true;
		this.ValueBrokeryRateUom.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueBrokeryRateUom.Location = new System.Drawing.Point(99, 106);
		this.ValueBrokeryRateUom.Name = "ValueBrokeryRateUom";
		this.ValueBrokeryRateUom.Size = new System.Drawing.Size(37, 17);
		this.ValueBrokeryRateUom.TabIndex = 18;
		this.ValueBrokeryRateUom.Text = "Uom";
		this.lblBrokeryRateUom.AutoSize = true;
		this.lblBrokeryRateUom.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBrokeryRateUom.ForeColor = System.Drawing.Color.Teal;
		this.lblBrokeryRateUom.Location = new System.Drawing.Point(9, 106);
		this.lblBrokeryRateUom.Name = "lblBrokeryRateUom";
		this.lblBrokeryRateUom.Size = new System.Drawing.Size(72, 17);
		this.lblBrokeryRateUom.TabIndex = 17;
		this.lblBrokeryRateUom.Text = "Rate Uom:";
		this.ValueBrokeryType.AutoSize = true;
		this.ValueBrokeryType.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueBrokeryType.Location = new System.Drawing.Point(99, 87);
		this.ValueBrokeryType.Name = "ValueBrokeryType";
		this.ValueBrokeryType.Size = new System.Drawing.Size(37, 17);
		this.ValueBrokeryType.TabIndex = 16;
		this.ValueBrokeryType.Text = "Type";
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.Teal;
		this.label69.Location = new System.Drawing.Point(9, 87);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(93, 17);
		this.label69.TabIndex = 15;
		this.label69.Text = "Brokery Type:";
		this.BrokerAmount.AutoSize = true;
		this.BrokerAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BrokerAmount.Location = new System.Drawing.Point(276, 106);
		this.BrokerAmount.Name = "BrokerAmount";
		this.BrokerAmount.Size = new System.Drawing.Size(43, 17);
		this.BrokerAmount.TabIndex = 10;
		this.BrokerAmount.Text = "13131";
		this.BrokerPercentRate.AutoSize = true;
		this.BrokerPercentRate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BrokerPercentRate.Location = new System.Drawing.Point(295, 87);
		this.BrokerPercentRate.Name = "BrokerPercentRate";
		this.BrokerPercentRate.Size = new System.Drawing.Size(15, 17);
		this.BrokerPercentRate.TabIndex = 9;
		this.BrokerPercentRate.Text = "1";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Teal;
		this.label29.Location = new System.Drawing.Point(207, 106);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(66, 17);
		this.label29.TabIndex = 8;
		this.label29.Text = "Amount: ";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Teal;
		this.label30.Location = new System.Drawing.Point(207, 87);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(95, 17);
		this.label30.TabIndex = 7;
		this.label30.Text = "Brokery Rate: ";
		this.BrokerName.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BrokerName.Dock = System.Windows.Forms.DockStyle.Top;
		this.BrokerName.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BrokerName.ForeColor = System.Drawing.Color.Blue;
		this.BrokerName.Location = new System.Drawing.Point(0, 35);
		this.BrokerName.Name = "BrokerName";
		this.BrokerName.Size = new System.Drawing.Size(334, 53);
		this.BrokerName.TabIndex = 2;
		this.BrokerName.Text = "Al Majeed Rice Traders Gujranwala 5446464 464646 ";
		this.BrokerName.Click += new System.EventHandler(BrokerName_Click);
		this.label13.Dock = System.Windows.Forms.DockStyle.Top;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Teal;
		this.label13.Location = new System.Drawing.Point(0, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(334, 35);
		this.label13.TabIndex = 1;
		this.label13.Text = "Broker ";
		this.label13.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
		this.CommissionAgentPanel.Controls.Add(this.ValueCommRateUom);
		this.CommissionAgentPanel.Controls.Add(this.lblCommRateUom);
		this.CommissionAgentPanel.Controls.Add(this.ValueCommType);
		this.CommissionAgentPanel.Controls.Add(this.label23);
		this.CommissionAgentPanel.Controls.Add(this.CommAmount);
		this.CommissionAgentPanel.Controls.Add(this.CommPercentRate);
		this.CommissionAgentPanel.Controls.Add(this.label26);
		this.CommissionAgentPanel.Controls.Add(this.label27);
		this.CommissionAgentPanel.Controls.Add(this.CommAgentName);
		this.CommissionAgentPanel.Controls.Add(this.label21);
		this.CommissionAgentPanel.Dock = System.Windows.Forms.DockStyle.Left;
		this.CommissionAgentPanel.Location = new System.Drawing.Point(318, 0);
		this.CommissionAgentPanel.Name = "CommissionAgentPanel";
		this.CommissionAgentPanel.Size = new System.Drawing.Size(321, 167);
		this.CommissionAgentPanel.TabIndex = 1;
		this.ValueCommRateUom.AutoSize = true;
		this.ValueCommRateUom.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueCommRateUom.Location = new System.Drawing.Point(94, 106);
		this.ValueCommRateUom.Name = "ValueCommRateUom";
		this.ValueCommRateUom.Size = new System.Drawing.Size(37, 17);
		this.ValueCommRateUom.TabIndex = 14;
		this.ValueCommRateUom.Text = "Uom";
		this.lblCommRateUom.AutoSize = true;
		this.lblCommRateUom.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCommRateUom.ForeColor = System.Drawing.Color.Teal;
		this.lblCommRateUom.Location = new System.Drawing.Point(7, 106);
		this.lblCommRateUom.Name = "lblCommRateUom";
		this.lblCommRateUom.Size = new System.Drawing.Size(72, 17);
		this.lblCommRateUom.TabIndex = 13;
		this.lblCommRateUom.Text = "Rate Uom:";
		this.ValueCommType.AutoSize = true;
		this.ValueCommType.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueCommType.Location = new System.Drawing.Point(94, 87);
		this.ValueCommType.Name = "ValueCommType";
		this.ValueCommType.Size = new System.Drawing.Size(37, 17);
		this.ValueCommType.TabIndex = 12;
		this.ValueCommType.Text = "Type";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Teal;
		this.label23.Location = new System.Drawing.Point(7, 87);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(85, 17);
		this.label23.TabIndex = 11;
		this.label23.Text = "Comm Type:";
		this.CommAmount.AutoSize = true;
		this.CommAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CommAmount.Location = new System.Drawing.Point(237, 104);
		this.CommAmount.Name = "CommAmount";
		this.CommAmount.Size = new System.Drawing.Size(43, 17);
		this.CommAmount.TabIndex = 10;
		this.CommAmount.Text = "13131";
		this.CommPercentRate.AutoSize = true;
		this.CommPercentRate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CommPercentRate.Location = new System.Drawing.Point(255, 87);
		this.CommPercentRate.Name = "CommPercentRate";
		this.CommPercentRate.Size = new System.Drawing.Size(15, 17);
		this.CommPercentRate.TabIndex = 9;
		this.CommPercentRate.Text = "1";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Teal;
		this.label26.Location = new System.Drawing.Point(168, 104);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(66, 17);
		this.label26.TabIndex = 8;
		this.label26.Text = "Amount: ";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Teal;
		this.label27.Location = new System.Drawing.Point(168, 87);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(87, 17);
		this.label27.TabIndex = 7;
		this.label27.Text = "Comm Rate: ";
		this.CommAgentName.Cursor = System.Windows.Forms.Cursors.Hand;
		this.CommAgentName.Dock = System.Windows.Forms.DockStyle.Top;
		this.CommAgentName.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CommAgentName.ForeColor = System.Drawing.Color.Blue;
		this.CommAgentName.Location = new System.Drawing.Point(0, 35);
		this.CommAgentName.Name = "CommAgentName";
		this.CommAgentName.Size = new System.Drawing.Size(321, 53);
		this.CommAgentName.TabIndex = 2;
		this.CommAgentName.Text = "Al Majeed Rice Traders Gujranwala 5446464 464646 ";
		this.CommAgentName.Click += new System.EventHandler(CommAgentName_Click);
		this.label21.Dock = System.Windows.Forms.DockStyle.Top;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.Teal;
		this.label21.Location = new System.Drawing.Point(0, 0);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(321, 35);
		this.label21.TabIndex = 1;
		this.label21.Text = "Commision Agent";
		this.label21.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
		this.SupplierPanel.Controls.Add(this.Address);
		this.SupplierPanel.Controls.Add(this.label10);
		this.SupplierPanel.Controls.Add(this.Email);
		this.SupplierPanel.Controls.Add(this.PhoneNo);
		this.SupplierPanel.Controls.Add(this.label9);
		this.SupplierPanel.Controls.Add(this.label8);
		this.SupplierPanel.Controls.Add(this.SupplierName);
		this.SupplierPanel.Controls.Add(this.lblSupplierOrCustomer);
		this.SupplierPanel.Dock = System.Windows.Forms.DockStyle.Left;
		this.SupplierPanel.Location = new System.Drawing.Point(0, 0);
		this.SupplierPanel.Name = "SupplierPanel";
		this.SupplierPanel.Size = new System.Drawing.Size(318, 167);
		this.SupplierPanel.TabIndex = 0;
		this.Address.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Address.Location = new System.Drawing.Point(73, 123);
		this.Address.Name = "Address";
		this.Address.Size = new System.Drawing.Size(248, 39);
		this.Address.TabIndex = 11;
		this.Address.Text = "jjhkjh ";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Teal;
		this.label10.Location = new System.Drawing.Point(3, 123);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(61, 17);
		this.label10.TabIndex = 10;
		this.label10.Text = "Address:";
		this.Email.AutoSize = true;
		this.Email.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Email.Location = new System.Drawing.Point(73, 106);
		this.Email.Name = "Email";
		this.Email.Size = new System.Drawing.Size(52, 17);
		this.Email.TabIndex = 6;
		this.Email.Text = "label12";
		this.PhoneNo.AutoSize = true;
		this.PhoneNo.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.PhoneNo.Location = new System.Drawing.Point(73, 88);
		this.PhoneNo.Name = "PhoneNo";
		this.PhoneNo.Size = new System.Drawing.Size(52, 17);
		this.PhoneNo.TabIndex = 5;
		this.PhoneNo.Text = "label11";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Teal;
		this.label9.Location = new System.Drawing.Point(4, 106);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(50, 17);
		this.label9.TabIndex = 3;
		this.label9.Text = "Email: ";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Teal;
		this.label8.Location = new System.Drawing.Point(4, 88);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(63, 17);
		this.label8.TabIndex = 2;
		this.label8.Text = "Phone #:";
		this.SupplierName.Cursor = System.Windows.Forms.Cursors.Hand;
		this.SupplierName.Dock = System.Windows.Forms.DockStyle.Top;
		this.SupplierName.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SupplierName.ForeColor = System.Drawing.Color.Blue;
		this.SupplierName.Location = new System.Drawing.Point(0, 35);
		this.SupplierName.Name = "SupplierName";
		this.SupplierName.Size = new System.Drawing.Size(318, 53);
		this.SupplierName.TabIndex = 1;
		this.SupplierName.Text = "Al Majeed Rice Traders Gujranwala gyhkihlh uhlk ";
		this.SupplierName.Click += new System.EventHandler(SupplierName_Click);
		this.lblSupplierOrCustomer.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblSupplierOrCustomer.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSupplierOrCustomer.ForeColor = System.Drawing.Color.Teal;
		this.lblSupplierOrCustomer.Location = new System.Drawing.Point(0, 0);
		this.lblSupplierOrCustomer.Name = "lblSupplierOrCustomer";
		this.lblSupplierOrCustomer.Size = new System.Drawing.Size(318, 35);
		this.lblSupplierOrCustomer.TabIndex = 0;
		this.lblSupplierOrCustomer.Text = "Supplier";
		this.lblSupplierOrCustomer.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
		this.panel8.Controls.Add(this.panel10);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1042, 81);
		this.panel8.TabIndex = 0;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.OrderNo);
		this.panel10.Controls.Add(this.InvoiceDueDate);
		this.panel10.Controls.Add(this.InvoiceDate);
		this.panel10.Controls.Add(this.lblOrder);
		this.panel10.Controls.Add(this.label4);
		this.panel10.Controls.Add(this.label3);
		this.panel10.Controls.Add(this.InvoiceNumber);
		this.panel10.Controls.Add(this.label1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(491, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(482, 81);
		this.panel10.TabIndex = 1;
		this.OrderNo.AutoSize = true;
		this.OrderNo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.OrderNo.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderNo.ForeColor = System.Drawing.Color.MediumBlue;
		this.OrderNo.Location = new System.Drawing.Point(380, 57);
		this.OrderNo.Name = "OrderNo";
		this.OrderNo.Size = new System.Drawing.Size(25, 19);
		this.OrderNo.TabIndex = 7;
		this.OrderNo.Text = "12";
		this.OrderNo.Click += new System.EventHandler(OrderNo_Click);
		this.InvoiceDueDate.AutoSize = true;
		this.InvoiceDueDate.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.InvoiceDueDate.ForeColor = System.Drawing.Color.White;
		this.InvoiceDueDate.Location = new System.Drawing.Point(379, 41);
		this.InvoiceDueDate.Name = "InvoiceDueDate";
		this.InvoiceDueDate.Size = new System.Drawing.Size(67, 19);
		this.InvoiceDueDate.TabIndex = 6;
		this.InvoiceDueDate.Text = "13-12-22";
		this.InvoiceDate.AutoSize = true;
		this.InvoiceDate.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.InvoiceDate.ForeColor = System.Drawing.Color.White;
		this.InvoiceDate.Location = new System.Drawing.Point(379, 24);
		this.InvoiceDate.Name = "InvoiceDate";
		this.InvoiceDate.Size = new System.Drawing.Size(67, 19);
		this.InvoiceDate.TabIndex = 5;
		this.InvoiceDate.Text = "13-12-22";
		this.lblOrder.AutoSize = true;
		this.lblOrder.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOrder.ForeColor = System.Drawing.Color.White;
		this.lblOrder.Location = new System.Drawing.Point(302, 57);
		this.lblOrder.Name = "lblOrder";
		this.lblOrder.Size = new System.Drawing.Size(61, 19);
		this.lblOrder.TabIndex = 4;
		this.lblOrder.Text = "Order #";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(302, 41);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(80, 19);
		this.label4.TabIndex = 3;
		this.label4.Text = "Due Date: ";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(302, 24);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(49, 19);
		this.label3.TabIndex = 2;
		this.label3.Text = "Date: ";
		this.InvoiceNumber.AutoSize = true;
		this.InvoiceNumber.Cursor = System.Windows.Forms.Cursors.Hand;
		this.InvoiceNumber.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.InvoiceNumber.ForeColor = System.Drawing.Color.Blue;
		this.InvoiceNumber.Location = new System.Drawing.Point(395, 1);
		this.InvoiceNumber.Name = "InvoiceNumber";
		this.InvoiceNumber.Size = new System.Drawing.Size(78, 26);
		this.InvoiceNumber.TabIndex = 1;
		this.InvoiceNumber.Text = "121211";
		this.InvoiceNumber.Click += new System.EventHandler(InvoiceNumber_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(301, 1);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(100, 26);
		this.label1.TabIndex = 0;
		this.label1.Text = "INVOICE #";
		this.panel9.BackColor = System.Drawing.Color.LightGray;
		this.panel9.Controls.Add(this.lblCompanyAddress);
		this.panel9.Controls.Add(this.pictureBox1);
		this.panel9.Controls.Add(this.lblCompanyName);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(491, 81);
		this.panel9.TabIndex = 0;
		this.lblCompanyAddress.AutoSize = true;
		this.lblCompanyAddress.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCompanyAddress.Location = new System.Drawing.Point(112, 40);
		this.lblCompanyAddress.Name = "lblCompanyAddress";
		this.lblCompanyAddress.Size = new System.Drawing.Size(122, 19);
		this.lblCompanyAddress.TabIndex = 2;
		this.lblCompanyAddress.Text = "Company Adress";
		this.pictureBox1.BackColor = System.Drawing.Color.White;
		this.pictureBox1.Location = new System.Drawing.Point(16, 4);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(90, 73);
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.lblCompanyName.AutoSize = true;
		this.lblCompanyName.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCompanyName.Location = new System.Drawing.Point(113, 18);
		this.lblCompanyName.Name = "lblCompanyName";
		this.lblCompanyName.Size = new System.Drawing.Size(117, 19);
		this.lblCompanyName.TabIndex = 1;
		this.lblCompanyName.Text = "Company Name";
		this.panel5.AutoScroll = true;
		this.panel5.Controls.Add(this.HeaderFlowLayoutPanel);
		this.panel5.Controls.Add(this.TotalRecordPanel);
		this.panel5.Controls.Add(this.FilterPanel);
		this.panel5.Controls.Add(this.panel19);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(347, 756);
		this.panel5.TabIndex = 1;
		this.HeaderFlowLayoutPanel.AutoScroll = true;
		this.HeaderFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.HeaderFlowLayoutPanel.Location = new System.Drawing.Point(0, 110);
		this.HeaderFlowLayoutPanel.Name = "HeaderFlowLayoutPanel";
		this.HeaderFlowLayoutPanel.Size = new System.Drawing.Size(347, 619);
		this.HeaderFlowLayoutPanel.TabIndex = 0;
		this.TotalRecordPanel.BackColor = System.Drawing.Color.White;
		this.TotalRecordPanel.Controls.Add(this.label5);
		this.TotalRecordPanel.Controls.Add(this.TotalHeaderRecords);
		this.TotalRecordPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.TotalRecordPanel.Location = new System.Drawing.Point(0, 729);
		this.TotalRecordPanel.Name = "TotalRecordPanel";
		this.TotalRecordPanel.Size = new System.Drawing.Size(347, 27);
		this.TotalRecordPanel.TabIndex = 7;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(96, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(95, 17);
		this.label5.TabIndex = 1;
		this.label5.Text = "Total Records: ";
		this.TotalHeaderRecords.AutoSize = true;
		this.TotalHeaderRecords.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalHeaderRecords.Location = new System.Drawing.Point(188, 5);
		this.TotalHeaderRecords.Name = "TotalHeaderRecords";
		this.TotalHeaderRecords.Size = new System.Drawing.Size(15, 17);
		this.TotalHeaderRecords.TabIndex = 2;
		this.TotalHeaderRecords.Text = "0";
		this.FilterPanel.BackColor = System.Drawing.Color.White;
		this.FilterPanel.Controls.Add(this.label64);
		this.FilterPanel.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierName);
		this.FilterPanel.Controls.Add(this.txtNumberTo);
		this.FilterPanel.Controls.Add(this.txtNoFrom);
		this.FilterPanel.Controls.Add(this.label63);
		this.FilterPanel.Controls.Add(this.label62);
		this.FilterPanel.Controls.Add(this.label60);
		this.FilterPanel.Controls.Add(this.DateTo);
		this.FilterPanel.Controls.Add(this.label61);
		this.FilterPanel.Controls.Add(this.DateFrom);
		this.FilterPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.FilterPanel.Location = new System.Drawing.Point(0, 26);
		this.FilterPanel.Name = "FilterPanel";
		this.FilterPanel.Size = new System.Drawing.Size(347, 84);
		this.FilterPanel.TabIndex = 6;
		this.label64.AutoSize = true;
		this.label64.BackColor = System.Drawing.Color.White;
		this.label64.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(3, 57);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(70, 15);
		this.label64.TabIndex = 213;
		this.label64.Text = "Party Name";
		this.cmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Location = new System.Drawing.Point(75, 54);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Name = "cmbSupplierName";
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Size = new System.Drawing.Size(266, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).TabIndex = 212;
		this.txtNumberTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNumberTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNumberTo.Location = new System.Drawing.Point(253, 28);
		this.txtNumberTo.Name = "txtNumberTo";
		this.txtNumberTo.Size = new System.Drawing.Size(88, 23);
		this.txtNumberTo.TabIndex = 210;
		this.txtNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoFrom.Location = new System.Drawing.Point(253, 3);
		this.txtNoFrom.Name = "txtNoFrom";
		this.txtNoFrom.Size = new System.Drawing.Size(88, 23);
		this.txtNoFrom.TabIndex = 208;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(176, 32);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(62, 15);
		this.label63.TabIndex = 211;
		this.label63.Text = "Invoice To";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(176, 7);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(77, 15);
		this.label62.TabIndex = 209;
		this.label62.Text = "Invoice From";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(3, 32);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(47, 15);
		this.label60.TabIndex = 207;
		this.label60.Text = "Date To";
		this.DateTo.CustomFormat = "dd-MMM-yy";
		this.DateTo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateTo.Location = new System.Drawing.Point(75, 28);
		this.DateTo.Name = "DateTo";
		this.DateTo.Size = new System.Drawing.Size(83, 23);
		this.DateTo.TabIndex = 205;
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(3, 7);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(63, 15);
		this.label61.TabIndex = 206;
		this.label61.Text = "Date From";
		this.DateFrom.CustomFormat = "dd-MMM-yy";
		this.DateFrom.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateFrom.Location = new System.Drawing.Point(75, 3);
		this.DateFrom.Name = "DateFrom";
		this.DateFrom.Size = new System.Drawing.Size(83, 23);
		this.DateFrom.TabIndex = 204;
		this.panel19.BackColor = System.Drawing.Color.White;
		this.panel19.Controls.Add(this.FilterRecords);
		this.panel19.Controls.Add(this.FilterBoxIcon);
		this.panel19.Controls.Add(this.HandleVisibilityOfFiletrPanel);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(347, 26);
		this.panel19.TabIndex = 5;
		this.FilterRecords.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.FilterRecords.Location = new System.Drawing.Point(256, 3);
		this.FilterRecords.Name = "FilterRecords";
		this.FilterRecords.Size = new System.Drawing.Size(85, 19);
		this.FilterRecords.TabIndex = 6;
		this.FilterRecords.Text = "Filter Records";
		this.FilterRecords.UseVisualStyleBackColor = true;
		this.FilterRecords.Click += new System.EventHandler(FilterRecords_Click);
		this.FilterBoxIcon.Cursor = System.Windows.Forms.Cursors.Hand;
		this.FilterBoxIcon.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.FilterBoxIcon.Location = new System.Drawing.Point(99, 3);
		this.FilterBoxIcon.Name = "FilterBoxIcon";
		this.FilterBoxIcon.Size = new System.Drawing.Size(33, 18);
		this.FilterBoxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.FilterBoxIcon.TabIndex = 5;
		this.FilterBoxIcon.TabStop = false;
		this.FilterBoxIcon.Click += new System.EventHandler(HandleVisibilityOfFiletrPanel_Click);
		this.HandleVisibilityOfFiletrPanel.AutoSize = true;
		this.HandleVisibilityOfFiletrPanel.Cursor = System.Windows.Forms.Cursors.Hand;
		this.HandleVisibilityOfFiletrPanel.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.HandleVisibilityOfFiletrPanel.Location = new System.Drawing.Point(130, 1);
		this.HandleVisibilityOfFiletrPanel.Name = "HandleVisibilityOfFiletrPanel";
		this.HandleVisibilityOfFiletrPanel.Size = new System.Drawing.Size(56, 21);
		this.HandleVisibilityOfFiletrPanel.TabIndex = 0;
		this.HandleVisibilityOfFiletrPanel.Text = "Filters";
		this.HandleVisibilityOfFiletrPanel.Click += new System.EventHandler(HandleVisibilityOfFiletrPanel_Click);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1406, 811);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1406, 27);
		this.panel2.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1583, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 1;
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[19]
		{
			this.BtnLoadDefault, this.BtnLoadAll, this.btnAttachment, this.btnCheckAll, this.btnApprove, this.ApprovedCount, this.btnEdit, this.btnPurchaseInvoiceSlip220, this.btnPurchaseInvoiceSlip220A, this.btnPurchaseInvoice220bSummary,
			this.btnPurchaseSaleVoucherPrint, this.btn104Voucher, this.BtnSaleInvoice301Slip, this.BtnSaleInvoiceSlip303, this.btnPIDirectSlip225, this.btnPIDirect225PartySlip, this.btnVouvher118, this.btn294Print, this.btn294APrint
		});
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1406, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.BtnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.BtnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadAll.Name = "BtnLoadAll";
		this.BtnLoadAll.Size = new System.Drawing.Size(74, 24);
		this.BtnLoadAll.Text = "Load All";
		this.BtnLoadAll.Click += new System.EventHandler(BtnLoadAll_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(37, 24);
		this.btnAttachment.Text = "0";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnCheckAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCheckAll.Image = (System.Drawing.Image)resources.GetObject("btnCheckAll.Image");
		this.btnCheckAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCheckAll.Name = "btnCheckAll";
		this.btnCheckAll.Size = new System.Drawing.Size(61, 24);
		this.btnCheckAll.Text = "Check All";
		this.btnCheckAll.Click += new System.EventHandler(btnCheckAll_Click);
		this.btnApprove.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnApprove.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApprove.Name = "btnApprove";
		this.btnApprove.Size = new System.Drawing.Size(79, 24);
		this.btnApprove.Text = "Approve ";
		this.btnApprove.Visible = false;
		this.btnApprove.Click += new System.EventHandler(btnApprove_Click);
		this.ApprovedCount.Name = "ApprovedCount";
		this.ApprovedCount.Size = new System.Drawing.Size(13, 24);
		this.ApprovedCount.Text = "0";
		this.ApprovedCount.Visible = false;
		this.btnEdit.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnEdit.Name = "btnEdit";
		this.btnEdit.Size = new System.Drawing.Size(51, 24);
		this.btnEdit.Text = "Edit";
		this.btnEdit.Click += new System.EventHandler(btnEdit_Click);
		this.btnPurchaseInvoiceSlip220.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPurchaseInvoiceSlip220.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchaseInvoiceSlip220.Name = "btnPurchaseInvoiceSlip220";
		this.btnPurchaseInvoiceSlip220.Size = new System.Drawing.Size(103, 24);
		this.btnPurchaseInvoiceSlip220.Text = "220-&SlipDetail";
		this.btnPurchaseInvoiceSlip220.Visible = false;
		this.btnPurchaseInvoiceSlip220.Click += new System.EventHandler(btnPurchaseInvoiceSlip220_Click);
		this.btnPurchaseInvoiceSlip220A.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPurchaseInvoiceSlip220A.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchaseInvoiceSlip220A.Name = "btnPurchaseInvoiceSlip220A";
		this.btnPurchaseInvoiceSlip220A.Size = new System.Drawing.Size(111, 24);
		this.btnPurchaseInvoiceSlip220A.Text = "220A-Party Slip";
		this.btnPurchaseInvoiceSlip220A.Visible = false;
		this.btnPurchaseInvoiceSlip220A.Click += new System.EventHandler(btnPurchaseInvoiceSlip220A_Click);
		this.btnPurchaseInvoice220bSummary.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPurchaseInvoice220bSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchaseInvoice220bSummary.Name = "btnPurchaseInvoice220bSummary";
		this.btnPurchaseInvoice220bSummary.Size = new System.Drawing.Size(112, 24);
		this.btnPurchaseInvoice220bSummary.Text = "220B-Summary";
		this.btnPurchaseInvoice220bSummary.Visible = false;
		this.btnPurchaseInvoice220bSummary.Click += new System.EventHandler(btnPurchaseInvoice220bSummary_Click);
		this.btnPurchaseSaleVoucherPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPurchaseSaleVoucherPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchaseSaleVoucherPrint.Name = "btnPurchaseSaleVoucherPrint";
		this.btnPurchaseSaleVoucherPrint.Size = new System.Drawing.Size(97, 24);
		this.btnPurchaseSaleVoucherPrint.Text = "103-&Voucher";
		this.btnPurchaseSaleVoucherPrint.Visible = false;
		this.btnPurchaseSaleVoucherPrint.Click += new System.EventHandler(btnPurchaseSaleVoucherPrint_Click);
		this.btn104Voucher.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn104Voucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn104Voucher.Name = "btn104Voucher";
		this.btn104Voucher.Size = new System.Drawing.Size(97, 24);
		this.btn104Voucher.Text = "104-Voucher";
		this.btn104Voucher.Visible = false;
		this.btn104Voucher.Click += new System.EventHandler(btn104Voucher_Click);
		this.BtnSaleInvoice301Slip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnSaleInvoice301Slip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaleInvoice301Slip.Name = "BtnSaleInvoice301Slip";
		this.BtnSaleInvoice301Slip.Size = new System.Drawing.Size(73, 24);
		this.BtnSaleInvoice301Slip.Text = "301-Slip";
		this.BtnSaleInvoice301Slip.Visible = false;
		this.BtnSaleInvoice301Slip.Click += new System.EventHandler(BtnSaleInvoice301Slip_Click);
		this.BtnSaleInvoiceSlip303.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnSaleInvoiceSlip303.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaleInvoiceSlip303.Name = "BtnSaleInvoiceSlip303";
		this.BtnSaleInvoiceSlip303.Size = new System.Drawing.Size(73, 24);
		this.BtnSaleInvoiceSlip303.Text = "303-Slip";
		this.BtnSaleInvoiceSlip303.Visible = false;
		this.BtnSaleInvoiceSlip303.Click += new System.EventHandler(BtnSaleInvoiceSlip303_Click);
		this.btnPIDirectSlip225.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPIDirectSlip225.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPIDirectSlip225.Name = "btnPIDirectSlip225";
		this.btnPIDirectSlip225.Size = new System.Drawing.Size(73, 24);
		this.btnPIDirectSlip225.Text = "225-Slip";
		this.btnPIDirectSlip225.Visible = false;
		this.btnPIDirectSlip225.Click += new System.EventHandler(btnPIDirectSlip225_Click);
		this.btnPIDirect225PartySlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPIDirect225PartySlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPIDirect225PartySlip.Name = "btnPIDirect225PartySlip";
		this.btnPIDirect225PartySlip.Size = new System.Drawing.Size(100, 24);
		this.btnPIDirect225PartySlip.Text = "225-PartySlip";
		this.btnPIDirect225PartySlip.Visible = false;
		this.btnPIDirect225PartySlip.Click += new System.EventHandler(btnPIDirect225PartySlip_Click);
		this.btnVouvher118.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVouvher118.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVouvher118.Name = "btnVouvher118";
		this.btnVouvher118.Size = new System.Drawing.Size(97, 24);
		this.btnVouvher118.Text = "118-Voucher";
		this.btnVouvher118.Visible = false;
		this.btnVouvher118.Click += new System.EventHandler(btnVouvher118_Click);
		this.btn294Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn294Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn294Print.Name = "btn294Print";
		this.btn294Print.Size = new System.Drawing.Size(49, 24);
		this.btn294Print.Text = "294";
		this.btn294Print.Visible = false;
		this.btn294Print.Click += new System.EventHandler(btn294Print_Click);
		this.btn294APrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn294APrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn294APrint.Name = "btn294APrint";
		this.btn294APrint.Size = new System.Drawing.Size(57, 24);
		this.btn294APrint.Text = "294A";
		this.btn294APrint.Visible = false;
		this.btn294APrint.Click += new System.EventHandler(btn294APrint_Click);
		this.BtnLoadDefault.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.BtnLoadDefault.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadDefault.Name = "BtnLoadDefault";
		this.BtnLoadDefault.Size = new System.Drawing.Size(98, 24);
		this.BtnLoadDefault.Text = "Load Default";
		this.BtnLoadDefault.Click += new System.EventHandler(BtnLoadDefault_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1406, 811);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "InvoiceHistory";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(VoucherValidation_Load);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		this.panel24.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		this.GrandTotalParentPanel.ResumeLayout(false);
		this.WagesPanel.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.GrandTotalPanel.ResumeLayout(false);
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.panelCommissionAmountInGrandTotal.ResumeLayout(false);
		this.panelCommissionAmountInGrandTotal.PerformLayout();
		this.panelEmptyBag.ResumeLayout(false);
		this.panelEmptyBag.PerformLayout();
		this.panelChargeToProductTotalAmount.ResumeLayout(false);
		this.panelChargeToProductTotalAmount.PerformLayout();
		this.panelSupplierAddlessTotalAmount.ResumeLayout(false);
		this.panelSupplierAddlessTotalAmount.PerformLayout();
		this.panelBrokeryTotalAmount.ResumeLayout(false);
		this.panelBrokeryTotalAmount.PerformLayout();
		this.PanelOtherExpenseTotalAmount.ResumeLayout(false);
		this.PanelOtherExpenseTotalAmount.PerformLayout();
		this.PanelItemTotalAmount.ResumeLayout(false);
		this.PanelItemTotalAmount.PerformLayout();
		this.OtherInformationPanel.ResumeLayout(false);
		this.OtherInfoFlowLayout.ResumeLayout(false);
		this.ChargeToProductPanel.ResumeLayout(false);
		this.CTPHeaderPanel.ResumeLayout(false);
		this.CTPHeaderPanel.PerformLayout();
		this.SupplierAddlessPanel.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.OtherExpensePanel.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.EmptyBagPanel.ResumeLayout(false);
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.BrokerPanel.ResumeLayout(false);
		this.BrokerPanel.PerformLayout();
		this.CommissionAgentPanel.ResumeLayout(false);
		this.CommissionAgentPanel.PerformLayout();
		this.SupplierPanel.ResumeLayout(false);
		this.SupplierPanel.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.panel5.ResumeLayout(false);
		this.TotalRecordPanel.ResumeLayout(false);
		this.TotalRecordPanel.PerformLayout();
		this.FilterPanel.ResumeLayout(false);
		this.FilterPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.FilterBoxIcon).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
