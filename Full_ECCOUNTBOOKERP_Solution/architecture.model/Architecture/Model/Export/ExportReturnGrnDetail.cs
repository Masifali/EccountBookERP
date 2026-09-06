using System;

namespace Architecture.Model.Export;

public class ExportReturnGrnDetail
{
	public bool IsApproved { get; set; }

	public decimal AdLsWeight { get; set; }

	public decimal EbTotal { get; set; }

	public decimal EbUnit { get; set; }

	public decimal GrossWeight { get; set; }

	public decimal ItemQty { get; set; }

	public decimal NetWeight { get; set; }

	public decimal StockWeight { get; set; }

	public decimal CostRate { get; set; }

	public int ActionTypeId { get; set; }

	public int CropYearId { get; set; }

	public int ExportReturnGrnId { get; set; }

	public int Id { get; set; }

	public int ContractId { get; set; }

	public int ContractDetailId { get; set; }

	public int InvoiceDetailId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int ExImForwardingDetailId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int LineId { get; set; }

	public int PackingTypeId { get; set; }

	public int PackUomId { get; set; }

	public int WarehouseId { get; set; }

	public string ContainerNo { get; set; }

	public string ItemDescription { get; set; }

	public string RemarksDetail { get; set; }

	public string SealNo { get; set; }

	public int ReturnInvoiceId { get; set; }

	public int ReturnInvoiceDetailId { get; set; }

	public virtual int ReturnInvoiceNo { get; set; }

	public virtual string ContractNo { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string JobLot { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual string PackUomCode { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual DateTime ContractDate { get; set; }
}
