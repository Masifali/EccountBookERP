using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class InvDeliveryOrder
{
	public bool IsApproved { get; set; }

	public bool IsStockReserved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime? ExpiryDate { get; set; }

	public DateTime? ReturnableDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal DoTotalQty { get; set; }

	public int ApprovedUser { get; set; }

	public int TransporterId { get; set; }

	public int BranchesId { get; set; }

	public int ToBranchId { get; set; }

	public int FromBranchId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int EximInvoiceId { get; set; }

	public int FinancialYearId { get; set; }

	public int ActionId { get; set; }

	public int LoadingPortId { get; set; }

	public int SaleTypeId { get; set; }

	public int DepartmentFromId { get; set; }

	public int DepartmentToId { get; set; }

	public int RequestedByLookUpId { get; set; }

	public int ApprovedByLookUpId { get; set; }

	public string LoadingInstructions { get; set; }

	public string DeliveryOrderType { get; set; }

	public string OtherWeightRemarks { get; set; }

	public string AccountRemarks { get; set; }

	public string VehicleNo { get; set; }

	public string ScreenName { get; set; }

	public string VehicleType { get; set; }

	public double GrossWeight { get; set; }

	public double NetWeight { get; set; }

	public double PackingWeight { get; set; }

	public double OtherWeight { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual string LoadingPort { get; set; }

	public virtual List<InvDeliveryOrderdetail> InvDeliveryOrderDetailList { get; set; }

	public virtual List<InvDeliveryOrderExpense> InvDeliveryOrderExpenseslist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
