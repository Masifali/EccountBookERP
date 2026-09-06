using System;
using System.Collections.Generic;

namespace Architecture.Model.pcc;

public class SaleOrder
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DeliveryStartDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime OrderDueDate { get; set; }

	public DateTime OrderExpiryDate { get; set; }

	public decimal CommissionAmount { get; set; }

	public decimal CommissionRate { get; set; }

	public decimal CommissionUom { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal FrieghtAmount { get; set; }

	public decimal OrderAmount { get; set; }

	public decimal OrderDiscountAmount { get; set; }

	public decimal OrderItemAmount { get; set; }

	public decimal OrderItemNetAmount { get; set; }

	public decimal OrderQty { get; set; }

	public decimal OrderWeight { get; set; }

	public decimal WageNetAmount { get; set; }

	public decimal AdjustDiscountAmount { get; set; }

	public decimal OtherWages { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CommissionAgentId { get; set; }

	public int CompanyId { get; set; }

	public int CurrencyId { get; set; }

	public int TransporterId { get; set; }

	public int DeliveryDays { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrderDueDays { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermsId { get; set; }

	public int ProjectsId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int VisitedById { get; set; }

	public int OtherWagesAccountId { get; set; }

	public int RefSalesManId { get; set; }

	public int ReferencePartyId { get; set; }

	public int BuildingHeightId { get; set; }

	public int BuildingStoreyId { get; set; }

	public string BuildingArea { get; set; }

	public string CommissionRemarks { get; set; }

	public string CommissionType { get; set; }

	public string DeliveryRemarks { get; set; }

	public string DeliveryTerm { get; set; }

	public string OrderStatus { get; set; }

	public string RefrenenceNo { get; set; }

	public string RemarksHeader { get; set; }

	public string Distance { get; set; }

	public string ReferencPartyName { get; set; }

	public string ReferencPartyAddress { get; set; }

	public string ReferencPartyCellNo { get; set; }

	public virtual List<SaleOrderDetail> SaleOrderDetailList { get; set; }

	public virtual List<SaleOrderWagesDetail> SaleOrderWagesDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
