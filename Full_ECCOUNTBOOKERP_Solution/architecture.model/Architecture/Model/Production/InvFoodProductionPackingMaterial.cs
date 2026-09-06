using System;
using System.Collections.Generic;
using Architecture.Model.Inventory;

namespace Architecture.Model.Production;

public class InvFoodProductionPackingMaterial
{
	public DateTime DocDate { get; set; }

	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int DocNo { get; set; }

	public int Id { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int BaseDocumentTypeId { get; set; }

	public int DocumentTypeId { get; set; }

	public int ItemId { get; set; }

	public int PlantId { get; set; }

	public int WarehouseId { get; set; }

	public int itemUomSchId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public string pmRemarks { get; set; }

	public int EntryUser { get; set; }

	public int ModifyUser { get; set; }

	public int BrandId { get; set; }

	public int BrandUomId { get; set; }

	public int FinancialYearId { get; set; }

	public int JobOrderPackingMaterialId { get; set; }

	public int ItemConditionId { get; set; }

	public int RackId { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ContractScheduleId { get; set; }

	public int ExImInvoiceId { get; set; }

	public virtual string ContractScheduleNo { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual string UOMDescription { get; set; }

	public virtual string PmDetailRowsRemoveIds { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string PlantName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string BrandName { get; set; }

	public virtual string BrandUom { get; set; }

	public virtual bool IsApproved { get; set; }

	public virtual string PlanStatus { get; set; }

	public virtual string ItemCondition { get; set; }

	public virtual string rackName { get; set; }

	public virtual double BalQty { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }

	public virtual List<InvFoodProductionPackingMaterial> invFoodProductionPackingMaterialList { get; set; }
}
