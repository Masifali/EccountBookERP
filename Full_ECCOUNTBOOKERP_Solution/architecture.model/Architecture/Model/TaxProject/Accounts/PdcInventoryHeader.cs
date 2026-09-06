using System;
using System.Collections.Generic;

namespace Architecture.Model.TaxProject.Accounts;

public class PdcInventoryHeader
{
	public bool IsApproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int MGLAccountCrId { get; set; }

	public int MGLAccountDrId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int FinancialYearId { get; set; }

	public string RemarkHeader { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<PdcInventory> PdcInventoriesList { get; set; }
}
