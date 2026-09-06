using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class InventoryStockReserved
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double Amount { get; set; }

	public double NetWeight { get; set; }

	public double ItemRate { get; set; }

	public double ItemQty { get; set; }

	public int Id { get; set; }

	public int RefDocIdNo { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefRefDocIdNo { get; set; }

	public int RefRefDocSubIdNo { get; set; }

	public int RefRefDocumentTypeId { get; set; }

	public int DocNo { get; set; }

	public int SupplierCustomerId { get; set; }

	public string TranRemarks { get; set; }

	public string VehicleNo { get; set; }

	public string BiltyNo { get; set; }

	public int GpNo { get; set; }

	public int WarehouseId { get; set; }

	public int ItemId { get; set; }

	public int CropYearId { get; set; }

	public int JobLotId { get; set; }

	public int PackingTypeId { get; set; }

	public int ItemUomId { get; set; }

	public int RateUomId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int FinancialYearId { get; set; }

	public int ProjectsId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int ApprovedUserId { get; set; }

	public int ActionId { get; set; }

	public virtual List<InventoryStockReserved> InventoryStockReservedList { get; set; }
}
