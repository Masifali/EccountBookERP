namespace Architecture.Model.Inventory;

public class SupplierDispatchDetail
{
	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public double AdLsWeight { get; set; }

	public double Amount { get; set; }

	public double EBTotalWt { get; set; }

	public double EBWeight { get; set; }

	public double GrossWeight { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public double WeightKgs { get; set; }

	public int ActionTypeId { get; set; }

	public int BranchId { get; set; }

	public int CommissionAgentId { get; set; }

	public int CropYearId { get; set; }

	public int CurrencyId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int OrderId { get; set; }

	public int OrderDetailId { get; set; }

	public int PackingTypeId { get; set; }

	public int PackSizeId { get; set; }

	public int RateUomId { get; set; }

	public int ReferencePartyId { get; set; }

	public int SortNo { get; set; }

	public int SupplierDispatchId { get; set; }

	public int SupplierId { get; set; }

	public string Remarks { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual int OrderNo { get; set; }

	public virtual string CommissionAgentName { get; set; }

	public virtual string ReferencePartyName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string BranchName { get; set; }

	public virtual string PackUomCode { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual string RateUomCode { get; set; }

	public virtual double RateUomEquivalent { get; set; }
}
