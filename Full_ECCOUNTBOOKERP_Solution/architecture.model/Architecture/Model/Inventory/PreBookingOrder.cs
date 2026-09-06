using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class PreBookingOrder
{
	public enum Status
	{
		All = -1,
		Pending,
		Approved
	}

	public bool IsAproved { get; set; }

	public bool PostState { get; set; }

	public bool IsValidate { get; set; }

	public DateTime DeliveryStartDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime OrderDueDate { get; set; }

	public DateTime OrderExpiryDate { get; set; }

	public DateTime? PostDate { get; set; }

	public double CommAmount { get; set; }

	public double CommRate { get; set; }

	public double OrderQty { get; set; }

	public double OrderWeight { get; set; }

	public double OrderAmount { get; set; }

	public double ExchangeRate { get; set; }

	public double ItemAmountHeader { get; set; }

	public double DiscountAmountHeader { get; set; }

	public double TaxAmountHeader { get; set; }

	public double FcyAmount { get; set; }

	public int ActionId { get; set; }

	public int BranchesId { get; set; }

	public int BrokerAgentSupCustId { get; set; }

	public int CatagorySrNo { get; set; }

	public int CompanyId { get; set; }

	public int DeliveryDays { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int NoOfCntnr { get; set; }

	public int OrderCatagoryId { get; set; }

	public int OrderDueDays { get; set; }

	public int OrderSupCustId { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermsId { get; set; }

	public int PfiScId { get; set; }

	public int PostUser { get; set; }

	public int ProjectsId { get; set; }

	public int RefrenenceParty { get; set; }

	public int SupplierCustomerIdStockParty { get; set; }

	public int UomScheduleIdCmRate { get; set; }

	public int FinancialYearId { get; set; }

	public int BillCalculateTypeId { get; set; }

	public int CurrencyId { get; set; }

	public string CommissionRemarks { get; set; }

	public string CommissionType { get; set; }

	public string DeliveryRemarks { get; set; }

	public string DeliveryTerm { get; set; }

	public string ExportInvoiceNo { get; set; }

	public string ExportLotRef { get; set; }

	public string ExportOrderNo { get; set; }

	public string OrderStatus { get; set; }

	public string OrderType { get; set; }

	public string OrderTaxable { get; set; }

	public string RemarksHeader { get; set; }

	public string SupplierRefNo { get; set; }

	public int OtherCommissionAgentId { get; set; }

	public string OtherCommissionType { get; set; }

	public double OtherCommissionUom { get; set; }

	public double OtherCommissionRate { get; set; }

	public double OtherCommissionAmount { get; set; }

	public string OtherCommissionRemarks { get; set; }

	public virtual List<PreBookingOrderDetail> PreBookingOrderDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
