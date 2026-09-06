using System;
using System.Collections.Generic;

namespace Architecture.Model.Import;

public class ImLcOrderScheduleHeader
{
	public DateTime AttentiveInspectionDate { get; set; }

	public DateTime AttentiveLoadingDate { get; set; }

	public DateTime AttentiveProductionDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal NetWeight { get; set; }

	public int BranchesId { get; set; }

	public int ActionId { get; set; }

	public int CompanyId { get; set; }

	public int DestinationPortId { get; set; }

	public int EntryUserId { get; set; }

	public int ImLcOrderId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int NoOfContainer { get; set; }

	public int OrganizationId { get; set; }

	public int FinancialYearId { get; set; }

	public int SortNo { get; set; }

	public int SupplierCustomerId { get; set; }

	public string ScheduleCode { get; set; }

	public string ScheduleRemarks { get; set; }

	public virtual string CustomerName { get; set; }

	public virtual string LcOrderNo { get; set; }

	public virtual string DestinationPort { get; set; }

	public virtual string EntryUser { get; set; }

	public virtual string ModifyUser { get; set; }

	public virtual List<ImLcOrderScheduleHeader> ImLcOrderScheduleHeaderList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
