using System;

namespace Architecture.Model.Inventory;

public class InventoryTransactions
{
	public bool IsApproved { get; set; }

	public DateTime? DocDate { get; set; }

	public double AmountIn { get; set; }

	public double AmountOut { get; set; }

	public double BillWeightIn { get; set; }

	public double BillWeightOut { get; set; }

	public double ExpenseAmountIn { get; set; }

	public double ItemRate { get; set; }

	public double QtyIn { get; set; }

	public double QtyOut { get; set; }

	public double StockWeightIn { get; set; }

	public double StockWeightOut { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocCodeNo { get; set; }

	public int InvPackingTypeId { get; set; }

	public int ItemId { get; set; }

	public int ItemUom { get; set; }

	public int JobLotId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int RateUom { get; set; }

	public int RefDocIdNo { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int WarehouseId { get; set; }

	public long Id { get; set; }

	public string CalcType { get; set; }

	public string CropBatch { get; set; }

	public string TranRemarks { get; set; }
}
