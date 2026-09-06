namespace Architecture.Model.Steel.Production;

public class InvFoodProductionDetail
{
	public int Id { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int FoodProductionId { get; set; }

	public int ProductionJobOrderId { get; set; }

	public int JobOrderOutPutId { get; set; }

	public int JobOrderInPutId { get; set; }

	public int EntryTypeId { get; set; }

	public int WarehouseId { get; set; }

	public int ItemId { get; set; }

	public int PackUomId { get; set; }

	public int JobLotId { get; set; }

	public int PackingtypeId { get; set; }

	public decimal Qty { get; set; }

	public int PackUnit { get; set; }

	public decimal Weight { get; set; }

	public decimal Rate { get; set; }

	public int RateUOMId { get; set; }

	public decimal Amount { get; set; }

	public string Remarks { get; set; }

	public decimal ItemOhCost { get; set; }

	public decimal GeneralOhCost { get; set; }

	public decimal NetRate { get; set; }

	public decimal TotalAmount { get; set; }

	public int ActionTypeId { get; set; }

	public int? DeleteFlag { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string EntryType { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string RateUOM { get; set; }

	public virtual double RateUomEquivalent { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string JobLotCode { get; set; }
}
