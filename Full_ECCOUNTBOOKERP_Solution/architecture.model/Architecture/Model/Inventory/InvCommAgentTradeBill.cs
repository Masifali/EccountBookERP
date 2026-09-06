using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.Inventory;

public class InvCommAgentTradeBill
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime DueDate { get; set; }

	public DateTime? SaleDueDate { get; set; }

	public DateTime EnteryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double PurchaseBillAmount { get; set; }

	public double SaleBillAmount { get; set; }

	public double PLAmount { get; set; }

	public double BrokeryRate { get; set; }

	public double CommRate { get; set; }

	public double CommAmount { get; set; }

	public double BrokeryAmount { get; set; }

	public int ApprovalUserId { get; set; }

	public int BranchesId { get; set; }

	public int BrokerId { get; set; }

	public int CommissionAgentId { get; set; }

	public int CompanyId { get; set; }

	public int CustomerId { get; set; }

	public int TradingGlAccountId { get; set; }

	public int DeliveryTermPurchaseId { get; set; }

	public int DeliveryTermSaleId { get; set; }

	public string DeliveryTerm { get; set; }

	public string DeliveryTermSale { get; set; }

	public int DocNo { get; set; }

	public int BranchSrNo { get; set; }

	public int DueDays { get; set; }

	public int SaleDueDays { get; set; }

	public int EnteryUserId { get; set; }

	public int Id { get; set; }

	public int InvCommAgentOrderId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermId { get; set; }

	public int SalePaymentTermId { get; set; }

	public int ProjectsId { get; set; }

	public int SupplierId { get; set; }

	public int FinancialYearId { get; set; }

	public string RemarksHeader { get; set; }

	public int DocumentTypeId { get; set; }

	public int CommissionDebitGLId { get; set; }

	public int BrokeryDebitGLId { get; set; }

	public int CustCommissionAgentId { get; set; }

	public double CustCommRate { get; set; }

	public double CustCommAmount { get; set; }

	public int CustCommissionDebitGLId { get; set; }

	public int CustBrokerId { get; set; }

	public double CustBrokeryRate { get; set; }

	public double CustBrokeryAmount { get; set; }

	public int CustBrokeryDebitGLId { get; set; }

	public string ScreenName { get; set; }

	public int ActionId { get; set; }

	public decimal BuyerFirstWeight { get; set; }

	public decimal BuyerNetWeight { get; set; }

	public decimal BuyerSecondWeight { get; set; }

	public decimal SupplierFirstWeight { get; set; }

	public decimal SupplierNetWeight { get; set; }

	public decimal SupplierSecondWeight { get; set; }

	public decimal SupplierBillWeight { get; set; }

	public decimal BuyerBillWeight { get; set; }

	public int PurchaseTaxAccountId { get; set; }

	public int SaleTaxAccountId { get; set; }

	public string PurchaseExpenseDetailDescription { get; set; }

	public string SaleExpenseDetailDescription { get; set; }

	public string PurchaseFreightExpenseDetailDescription { get; set; }

	public string SaleExpenseCreditToReleventAccountDetailDescription { get; set; }

	public string PurchaseCommisionDetailDescription { get; set; }

	public string PurchaseBrokeryDetailDescription { get; set; }

	public string SaleCommisionDetailDescription { get; set; }

	public string SaleBrokeryDetailDescription { get; set; }

	public string PurchasePaymentDetailDescription { get; set; }

	public string SalePaymentDetailDescription { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual string CustomerName { get; set; }

	public virtual string PurchaseTaxAccount { get; set; }

	public virtual string SaleTaxAccount { get; set; }

	public virtual string TradingGlAccount { get; set; }

	public virtual List<InvCommAgentTradeBillDetail> InvCommAgentTradeBillDetailslist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<CommisionAgentBillPurchaseExpense> InvCommAgentTradePurchaseExpList { get; set; }

	public virtual List<CommisionAgentBillSaleExpense> InvCommAgentTradeSaleExpList { get; set; }

	public virtual List<CommisionAgentBillSaleExpenseCreditToReleventAc> CommisionAgentBillSaleExpenseCreditToReleventAcsList { get; set; }

	public virtual List<CommisionAgentBillPurchaseFreightExpense> InvCommAgentTradeFreightExpList { get; set; }

	public virtual List<CommisionAgentBillPaymentDetail> CommisionAgentBillPaymentDetailList { get; set; }

	public virtual List<CommisionAgentBillCommissionDetail> CommisionAgentBillCommissionDetailList { get; set; }

	public virtual List<CommisionAgentBillFreightDetail> CommisionAgentBillFreightDetailList { get; set; }

	public virtual List<CommisionAgentBillTaxDetail> CommisionAgentBillTaxDetailList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public InvCommAgentTradeBill()
	{
		InvCommAgentTradeBillDetailslist = new List<InvCommAgentTradeBillDetail>();
		AttachmentsList = new List<DMSAttachments>();
		DeleteAttachmentsList = new List<DMSAttachments>();
		InvCommAgentTradePurchaseExpList = new List<CommisionAgentBillPurchaseExpense>();
		InvCommAgentTradeSaleExpList = new List<CommisionAgentBillSaleExpense>();
		CommisionAgentBillSaleExpenseCreditToReleventAcsList = new List<CommisionAgentBillSaleExpenseCreditToReleventAc>();
		InvCommAgentTradeFreightExpList = new List<CommisionAgentBillPurchaseFreightExpense>();
		CommisionAgentBillPaymentDetailList = new List<CommisionAgentBillPaymentDetail>();
		CommisionAgentBillCommissionDetailList = new List<CommisionAgentBillCommissionDetail>();
		CommisionAgentBillFreightDetailList = new List<CommisionAgentBillFreightDetail>();
		CommisionAgentBillTaxDetailList = new List<CommisionAgentBillTaxDetail>();
	}
}
