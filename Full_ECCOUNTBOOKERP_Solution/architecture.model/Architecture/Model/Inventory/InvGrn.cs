using System;
using System.Collections.Generic;
using Architecture.Model.PartyProcessing;

namespace Architecture.Model.Inventory;

public class InvGrn
{
	public bool IsApproved { get; set; }

	public bool AddWages { get; set; }

	public bool ScaleShortWeightApply { get; set; }

	public bool SupplierShortWeightApply { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? PostDate { get; set; }

	public double CarriageAmount { get; set; }

	public double FreightDeduction { get; set; }

	public double BiltyFreight { get; set; }

	public double AdvanceByFactoryFreight { get; set; }

	public double AdvanceByPartyFreight { get; set; }

	public virtual double OtherFreightDeduction { get; set; }

	public double AccessWeight { get; set; }

	public double FactoryWeight { get; set; }

	public double OtherCharges { get; set; }

	public double PartyWeight { get; set; }

	public double ScaleKart { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int FinancialYearId { get; set; }

	public int DocNo { get; set; }

	public int SupplierDispatchId { get; set; }

	public int DocumentTypeId { get; set; }

	public int BaseDocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int InwardGatePassId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int ProjectsId { get; set; }

	public int ReferencePartyId { get; set; }

	public int StockPartyId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TransporterId { get; set; }

	public int TransporterSupCustId { get; set; }

	public int GhallaMandiId { get; set; }

	public int BillCalculateTypeId { get; set; }

	public int GrnTypeId { get; set; }

	public int ReqestedById { get; set; }

	public DateTime? ReturnableDate { get; set; }

	public string BiltyNo { get; set; }

	public string ReferenceDocNo { get; set; }

	public string RemarksHeader { get; set; }

	public string SupplierReference { get; set; }

	public string TransporterDocRef { get; set; }

	public string VehicleNo { get; set; }

	public int ActionId { get; set; }

	public string VehicleType { get; set; }

	public string DeliveryTerm { get; set; }

	public string Status { get; set; }

	public string TransType { get; set; }

	public string ScreenName { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public virtual int DeductionPolicyForGrnId { get; set; }

	public virtual int InvoiceId { get; set; }

	public virtual int InvoiceNo { get; set; }

	public virtual int RefDocumentTypeId { get; set; }

	public virtual int FreightId { get; set; }

	public virtual int LabWeightCutOnId { get; set; }

	public virtual string TicketNos { get; set; }

	public virtual string OrderType { get; set; }

	public virtual double ScaleCharges { get; set; }

	public virtual double ShortageAmountFV { get; set; }

	public virtual double DiscountAmountFV { get; set; }

	public virtual double ChargeToPartyAmountFV { get; set; }

	public virtual DateTime? PreviousDate { get; set; }

	public virtual List<InvGrnDetail> invGrnDetail { get; set; }

	public virtual List<InvgrnDetailEmptyBags> InvgrnDetailEmptyBagslist { get; set; }

	public virtual List<InvGrnPurchaseBreakUp> InvGrnPurchaseBreakUplist { get; set; }

	public virtual List<InventoryTransactionsPartyProcessingForFifo> InventoryTransactionsPartyProcessingForFIFOlist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
