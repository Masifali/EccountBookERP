using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.ContractorWages;

public class InvContractorWagesBillHeader
{
	public bool IsAproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double WeightTotal { get; set; }

	public double QtyTotal { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int FinancialYearId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int RefDocNo { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int StockPartyId { get; set; }

	public int JobOrderId { get; set; }

	public int ScaleSlipNo { get; set; }

	public string OtherRemarks { get; set; }

	public string RefDocument { get; set; }

	public virtual string StockPartyName { get; set; }

	public virtual string DocumentTypeDescription { get; set; }

	public virtual List<InvContractorWagesBillDetail> invContractWagesBillDateil { get; set; }

	public virtual VoucherHead VoucherHeadForContractorWages { get; set; }
}
