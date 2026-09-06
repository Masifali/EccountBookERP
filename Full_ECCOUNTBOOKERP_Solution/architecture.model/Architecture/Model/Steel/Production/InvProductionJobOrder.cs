using System;
using System.Collections.Generic;

namespace Architecture.Model.Steel.Production;

public class InvProductionJobOrder
{
	public int Id { get; set; }

	public int DocumentTypeId { get; set; }

	public DateTime DocDate { get; set; }

	public int DocNo { get; set; }

	public string ProductionJobOrderNo { get; set; }

	public string PlanType { get; set; }

	public int PlanTypeSrNo { get; set; }

	public string ProductionType { get; set; }

	public string LotReference { get; set; }

	public string ProductionInsturction { get; set; }

	public string PackingInstructions { get; set; }

	public string DeliveryInstructions { get; set; }

	public string OtherInstructions { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public string PlanStatus { get; set; }

	public int? WorkInProccessAcId { get; set; }

	public int? WipItemId { get; set; }

	public decimal? FinishGoodRate { get; set; }

	public int? ApprovedUserId { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public bool IsApproved { get; set; }

	public DateTime EntryDate { get; set; }

	public int EntryUserId { get; set; }

	public DateTime? ModifyDate { get; set; }

	public int? ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int ProjectsId { get; set; }

	public int FinancialYearId { get; set; }

	public int? ActionId { get; set; }

	public bool? PendingForView { get; set; }

	public virtual List<InvProductionJobOrderInput> invProductionJobOrderInput { get; set; }

	public virtual List<InvProductionJobOrderOutput> invProductionJobOrderOutput { get; set; }

	public virtual List<InvProductionJobOrderOverHeads> InvProductionJobOrderOverHeadsList { get; set; }

	public virtual List<InvProductionJobOrderPlant> InvProductionJobOrderPlantslist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
