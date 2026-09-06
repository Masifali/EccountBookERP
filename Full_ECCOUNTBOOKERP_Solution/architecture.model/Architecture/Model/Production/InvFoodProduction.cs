using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.Model.Production;

public class InvFoodProduction
{
	public bool IsApproved { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int BranchId { get; set; }

	public int PlantId { get; set; }

	public int DocumentTypeId { get; set; }

	public int CompanyId { get; set; }

	public int DocCode { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ContractScheduleId { get; set; }

	public int InvFoodProductionPlanId { get; set; }

	public int WIPAccountId { get; set; }

	public int WIPItemId { get; set; }

	public int WipWareHouseId { get; set; }

	public int EBDepartmentId { get; set; }

	public int InvJobOrderId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int FinancialYearId { get; set; }

	public int BaseDocumentTypeId { get; set; }

	public string EntryType { get; set; }

	public string InvJobOrderNo { get; set; }

	public string MainRemarks { get; set; }

	public bool StockHoldForLabApproval { get; set; }

	public virtual int RefDocumentTypeId { get; set; }

	public virtual string InputDetailRowsRemoveIds { get; set; }

	public virtual string OutputDetailRowsRemoveIds { get; set; }

	public virtual string PlanStatus { get; set; }

	public virtual List<InvFoodProductionDetail> invFoodProductionDetails { get; set; }

	public virtual List<InvFoodProductionConsumptionDetail> InvFoodProductionConsumptionDetailslist { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }
}
