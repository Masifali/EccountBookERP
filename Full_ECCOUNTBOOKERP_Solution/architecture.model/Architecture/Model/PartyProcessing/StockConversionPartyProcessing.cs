using System;
using System.Collections.Generic;

namespace Architecture.Model.PartyProcessing;

public class StockConversionPartyProcessing
{
	public bool PostState { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public int ActionId { get; set; }

	public int StockPartyId { get; set; }

	public int BranchedId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int parentCategoryId { get; set; }

	public int PostUser { get; set; }

	public int ProjectsId { get; set; }

	public string DocManualRef { get; set; }

	public string ProductionNo { get; set; }

	public string Remarks { get; set; }

	public virtual List<StockConversionPartyProcessingDetail> StockConversionPartyProcessingDetail { get; set; }

	public virtual List<StockConversionPartyProcessingPackingMaterial> StockConversionPartyProcessingPackingMaterial { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
