using System;
using System.Collections.Generic;

namespace Architecture.Model.ContractorWages;

public class InvWagesSceduleForAtuoCalculationHeader
{
	public bool IsActive { get; set; }

	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EffectiveFrom { get; set; }

	public DateTime EffectiveTo { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public string RemarksHeader { get; set; }

	public string SheduleName { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public virtual List<InvWagesSceduleForAtuoCalculationDetail> WagesScheduleAutoCalculationDateil { get; set; }
}
