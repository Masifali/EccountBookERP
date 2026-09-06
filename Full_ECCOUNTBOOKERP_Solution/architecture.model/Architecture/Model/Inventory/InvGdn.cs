using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class InvGdn
{
	public bool IsApproved { get; set; }

	public bool AddWages { get; set; }

	public bool ShortWeightDeductionApply { get; set; }

	public bool IsStockReserved { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime? GPDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? PostDate { get; set; }

	public DateTime? ReturnableDate { get; set; }

	public double CarriageAmount { get; set; }

	public double FactoryWeight { get; set; }

	public double OtherCharges { get; set; }

	public double PartyWeight { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int AutoUpdateId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int OutwardGatePassId { get; set; }

	public int PostUser { get; set; }

	public int ProjectsId { get; set; }

	public int ReferencePartyId { get; set; }

	public int StockPartyId { get; set; }

	public int RequestedByLookUpId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TransporterId { get; set; }

	public int TransporterSupCustId { get; set; }

	public int BillCalculateTypeId { get; set; }

	public string BiltyNo { get; set; }

	public string ContainerNo { get; set; }

	public string ContainerNo1 { get; set; }

	public string ReferenceDocNo { get; set; }

	public string RemarksHeader { get; set; }

	public string SupplierReference { get; set; }

	public string TransporterDocRef { get; set; }

	public string ScreenName { get; set; }

	public string VehicleNo { get; set; }

	public string DeliveryTerm { get; set; }

	public string VehicleType { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public int ActionId { get; set; }

	public int FinancialYearId { get; set; }

	public string DriverName { get; set; }

	public string DriverCNIC { get; set; }

	public string DriverCellNo { get; set; }

	public int VehicleTypeId { get; set; }

	public int OrderId { get; set; }

	public int OrderTypeId { get; set; }

	public int AdvanceDeliveryOrderId { get; set; }

	public virtual string CompanyName { get; set; }

	public virtual int OrderNo { get; set; }

	public virtual int CustomerGlAccountId { get; set; }

	public virtual string CustomerCityName { get; set; }

	public virtual string CustomerMobileNo { get; set; }

	public virtual string OtherSupCust { get; set; }

	public virtual string TicketNos { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<InvGdnDetail> invGdnDetail { get; set; }

	public virtual List<InvGdnExpense> InvGdnExpenseslist { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }
}
