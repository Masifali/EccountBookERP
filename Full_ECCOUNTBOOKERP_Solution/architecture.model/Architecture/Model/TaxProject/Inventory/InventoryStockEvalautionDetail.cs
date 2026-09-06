using System;
using System.Collections.Generic;

namespace Architecture.Model.TaxProject.Inventory;

public class InventoryStockEvalautionDetail
{
	public bool IsApproved { get; set; }

	public DateTime? DocDate { get; set; }

	public double AmountIn { get; set; }

	public double AmountOut { get; set; }

	public double BillWeightIn { get; set; }

	public double BillWeightOut { get; set; }

	public double CgsRate { get; set; }

	public double ExpenseAmountIn { get; set; }

	public double ItemRate { get; set; }

	public double QtyIn { get; set; }

	public double QtyOut { get; set; }

	public double StockWeightIn { get; set; }

	public double StockWeightOut { get; set; }

	public double CgsAmount { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocCodeNo { get; set; }

	public int GpNoDcNo { get; set; }

	public int Id { get; set; }

	public int InvPackingTypeId { get; set; }

	public int ItemId { get; set; }

	public int ItemUom { get; set; }

	public int JobLotId { get; set; }

	public int OrderNo { get; set; }

	public int OrganizationId { get; set; }

	public int PrdJobOrderNo { get; set; }

	public int ProjectsId { get; set; }

	public int RateUom { get; set; }

	public int RefDocIdNo { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int OtherDocumentTypeId { get; set; }

	public int OtherDocNoId { get; set; }

	public int OtherSubDocNoId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int WarehouseId { get; set; }

	public int RefWarehouseId { get; set; }

	public int CityId { get; set; }

	public int LineId { get; set; }

	public int RefRefDocumentTypeId { get; set; }

	public int RefRefDocIdNo { get; set; }

	public int RefRefDocSubIdNo { get; set; }

	public int EntryUser { get; set; }

	public int ModifyUser { get; set; }

	public int InvoiceId { get; set; }

	public int InvoiceDetailId { get; set; }

	public int VarientId { get; set; }

	public string CalcType { get; set; }

	public string CropBatch { get; set; }

	public string TranRemarks { get; set; }

	public string VehicleNo { get; set; }

	public string BiltyNo { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }
}
