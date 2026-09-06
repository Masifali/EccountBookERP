namespace Architecture.Model.Export;

public class ExportReturnInvoiceDetail
{
	public bool IsApproved { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal LcyAmount { get; set; }

	public decimal AddLessAmount { get; set; }

	public decimal FcyAmountWithOutAddLess { get; set; }

	public decimal Mton { get; set; }

	public decimal NetWeight { get; set; }

	public decimal Qty { get; set; }

	public decimal RatePrice { get; set; }

	public decimal StockWeight { get; set; }

	public decimal CostRate { get; set; }

	public decimal CostAmount { get; set; }

	public int ActionTypeId { get; set; }

	public int CurrencyId { get; set; }

	public int Id { get; set; }

	public int LineId { get; set; }

	public int ExportReturnInvoiceId { get; set; }

	public int ExImInvoiceDetailId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int GrnId { get; set; }

	public int GrnDetailId { get; set; }

	public int WarehouseId { get; set; }

	public int ItemId { get; set; }

	public int CropYearId { get; set; }

	public int JobLotId { get; set; }

	public int PackingTypeId { get; set; }

	public int PackUomId { get; set; }

	public int RateUomId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual int GrnNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string JobLot { get; set; }

	public virtual string PackingType { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual string PackUomCode { get; set; }

	public virtual double RateUomEquivalent { get; set; }

	public virtual string RateUomCode { get; set; }
}
