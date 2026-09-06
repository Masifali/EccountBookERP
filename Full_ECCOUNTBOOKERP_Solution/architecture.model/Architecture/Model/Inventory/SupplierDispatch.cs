using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class SupplierDispatch
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DepartureDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ETAdestination { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal Freight { get; set; }

	public decimal OtherExpense { get; set; }

	public decimal SupplierWbWeight { get; set; }

	public decimal TotalAmount { get; set; }

	public decimal TotalQty { get; set; }

	public decimal TotalWeight { get; set; }

	public int BranchSrNo { get; set; }

	public int FinancialYearId { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CityId { get; set; }

	public int CompanyId { get; set; }

	public int CurrencyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PendingForView { get; set; }

	public int ProjectId { get; set; }

	public int TransitDays { get; set; }

	public string DeliveryTerm { get; set; }

	public string BiltyNo { get; set; }

	public string Driver1CellNo { get; set; }

	public string Driver2CellNo { get; set; }

	public string DriverName { get; set; }

	public string DriverPic { get; set; }

	public string RemarksHeader { get; set; }

	public string VehicleImage { get; set; }

	public string VehicleNo { get; set; }

	public string ManualBillNo { get; set; }

	public decimal AdvanceFreight { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public virtual List<SupplierDispatchDetail> SupplierDispatchDetailList { get; set; }

	public virtual List<SupplierDispatchCommDetail> SupplierDispatchCommDetailList { get; set; }

	public virtual List<SupplierDispatchExpense> SupplierDispatchExpenseList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
