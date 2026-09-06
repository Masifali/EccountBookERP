using System;
using System.Collections.Generic;

namespace Architecture.Model.FeedMill.Sales;

public class SaleOrder
{
	public bool IsAproved { get; set; }

	public bool IsValidate { get; set; }

	public bool PostState { get; set; }

	public DateTime DeliveryStartDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime OrderDueDate { get; set; }

	public DateTime OrderExpiryDate { get; set; }

	public DateTime PostDate { get; set; }

	public double CommAmount { get; set; }

	public double CommRate { get; set; }

	public double DiscountAmountHeader { get; set; }

	public double ExchangeRate { get; set; }

	public double FcyAmount { get; set; }

	public double ItemAmountHeader { get; set; }

	public double OrderAmount { get; set; }

	public double OrderQty { get; set; }

	public double OrderWeight { get; set; }

	public double TaxAmountHeader { get; set; }

	public int ActionId { get; set; }

	public int BillCalculateTypeId { get; set; }

	public int BranchesId { get; set; }

	public int CatagorySrNo { get; set; }

	public int CommissionAgentId { get; set; }

	public int CommRateUomId { get; set; }

	public int CompanyId { get; set; }

	public int CurrencyId { get; set; }

	public int DeliveryDays { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrderCatagoryId { get; set; }

	public int OrderDueDays { get; set; }

	public int OrderSupCustId { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermsId { get; set; }

	public int PostUserId { get; set; }

	public int ProjectsId { get; set; }

	public int RefrenenceParty { get; set; }

	public string CommissionRemarks { get; set; }

	public string CommissionType { get; set; }

	public string DeliveryRemarks { get; set; }

	public string DeliveryTerm { get; set; }

	public string OrderStatus { get; set; }

	public string OrderStatusRemarks { get; set; }

	public string OrderTaxable { get; set; }

	public string OrderType { get; set; }

	public string RemarksHeader { get; set; }

	public string SupplierRefNo { get; set; }

	public virtual List<SaleOrderDetail> SaleOrderDetailList { get; set; }

	public virtual List<SaleOrderCustomerExpenses> SaleOrderCustomerExpenseslist { get; set; }

	public virtual List<SaleOrderPaymentTermsDetail> SaleOrderPaymentTermsDetailslist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
