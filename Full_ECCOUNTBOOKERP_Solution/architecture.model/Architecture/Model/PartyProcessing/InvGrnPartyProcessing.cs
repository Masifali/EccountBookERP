using System;
using System.Collections.Generic;

namespace Architecture.Model.PartyProcessing;

public class InvGrnPartyProcessing
{
	public bool IsApproved { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public double CarriageAmount { get; set; }

	public double FactoryWeight { get; set; }

	public double PartyWeight { get; set; }

	public int ActionId { get; set; }

	public int FinancialYearId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int GatePassId { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int ProjectsId { get; set; }

	public int StockPartyId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TransporterId { get; set; }

	public int TradeToCompanyId { get; set; }

	public string BiltyNo { get; set; }

	public string RemarksHeader { get; set; }

	public string VehicleNo { get; set; }

	public string VehicleType { get; set; }

	public string ScreenName { get; set; }

	public virtual string StockPartyName { get; set; }

	public virtual List<InvGrnPartyProcessingDetail> InvGrnPartyProcessingDetailslist { get; set; }

	public virtual List<InvGrnEmptyBagsPartyProcessing> InvGrnEmptyBagsPartyProcessingslist { get; set; }

	public virtual List<InventoryTransactionsPartyProcessingForFifo> InventoryTransactionsPartyProcessingForFIFOlist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
