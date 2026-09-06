namespace Architecture.Model.TaxProject.Export;

public class ExportVoucherDetail
{
	public decimal CostRate { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal LcyAmount { get; set; }

	public decimal Mton { get; set; }

	public decimal NetWeight { get; set; }

	public decimal Qty { get; set; }

	public decimal RatePrice { get; set; }

	public decimal StockValue { get; set; }

	public decimal StockWeight { get; set; }

	public int ContractId { get; set; }

	public int CropYearId { get; set; }

	public int CurrencyId { get; set; }

	public int ExImInvoiceDetailId { get; set; }

	public int ExportVoucherId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int LotJobId { get; set; }

	public int PackingTypeId { get; set; }

	public int RateUomId { get; set; }

	public int WarehouseId { get; set; }

	public int LineId { get; set; }

	public int EntryTypeId { get; set; }

	public string Remarks { get; set; }

	public virtual string ContractNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string PackUom { get; set; }

	public virtual string RateUom { get; set; }

	public virtual string JobLotDescription { get; set; }
}
