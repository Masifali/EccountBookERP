using System;
using System.Collections.Generic;

namespace Architecture.Model.Production;

public class InvProductionJobOrder
{
	public bool IsApproved { get; set; }

	public DateTime EndDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PlanDate { get; set; }

	public DateTime StartDate { get; set; }

	public bool PendingForView { get; set; }

	public int ActionId { get; set; }

	public int DocumentTypeId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int InvProductionPlantId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PlanCode { get; set; }

	public int PlanTypeSrNo { get; set; }

	public int ProjectsId { get; set; }

	public int RefSalesOrderId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string DeliveryInstructions { get; set; }

	public string LotReference { get; set; }

	public string OtherInstructions { get; set; }

	public string PackingInstructions { get; set; }

	public string PlanStatus { get; set; }

	public string PlanType { get; set; }

	public string ProductionInsturction { get; set; }

	public string ProductionType { get; set; }

	public string RefInvoiceNo { get; set; }

	public int WorkInProccessAcId { get; set; }

	public int FinishGoodsAcId { get; set; }

	public int ByProductacId { get; set; }

	public int JobLotId { get; set; }

	public int WipItemId { get; set; }

	public int WipWareHouseId { get; set; }

	public int FinancialYearId { get; set; }

	public double FinishGoodRate { get; set; }

	public virtual List<InvProductionJobOrderInput> invProductionJobOrderInput { get; set; }

	public virtual List<InvProductionJobOrderOutput> invProductionJobOrderOutput { get; set; }

	public virtual List<InvProductionJobOrderOverHeads> InvProductionJobOrderOverHeadsList { get; set; }

	public virtual List<InvProductionJobOrderPackingMaterial> InvProductionJobOrderPackingMaterialList { get; set; }

	public virtual List<InvProductionJobOrderPlant> InvProductionJobOrderPlantslist { get; set; }

	public virtual List<JobOrderRateSchedule> JobOrderRateSchedulelist { get; set; }

	public virtual List<InvProductionJobOrderAndOrderAllocation> InvProductionJobOrderAndOrderAllocationList { get; set; }

	public virtual List<InvProductionJobOrderLabStandardDetail> InvProductionJobOrderLabStandardDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
