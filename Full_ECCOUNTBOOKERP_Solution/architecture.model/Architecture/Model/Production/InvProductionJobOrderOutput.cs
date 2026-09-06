namespace Architecture.Model.Production;

public class InvProductionJobOrderOutput
{
	public double NetWeight { get; set; }

	public double QtyInner { get; set; }

	public double QtyOuter { get; set; }

	public double ItemRate { get; set; }

	public double ItemAmount { get; set; }

	public double WagesRate { get; set; }

	public double WagesAmount { get; set; }

	public double PMRate { get; set; }

	public double PMAmount { get; set; }

	public decimal OHExpense { get; set; }

	public decimal PMExpense { get; set; }

	public decimal OHGeneralExpense { get; set; }

	public decimal PMGeneralExpense { get; set; }

	public decimal NetRate { get; set; }

	public decimal NetAmount { get; set; }

	public int Id { get; set; }

	public int InvPackingTypeId { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int ItemId { get; set; }

	public int UomSchIdInner { get; set; }

	public int UomSchIdOuter { get; set; }

	public string PackRemarks { get; set; }

	public int JobLotId { get; set; }

	public int CropYearId { get; set; }

	public int WarehouseId { get; set; }

	public int RateUomId { get; set; }

	public int WagesAccountId { get; set; }

	public int PMItemId { get; set; }

	public int EntryTypeId { get; set; }

	public int ActionTypeId { get; set; }

	public virtual string PackTypeCode { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string InnerUomCode { get; set; }

	public virtual string InnerUOMDescription { get; set; }

	public virtual string outerUOMCode { get; set; }

	public virtual string OuterUOMDescription { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string WagesAccountName { get; set; }

	public virtual string PackingItem { get; set; }

	public virtual string RateUom { get; set; }

	public virtual string EntryType { get; set; }

	public virtual double Equivalent { get; set; }

	public virtual double InnerUomCodeEquivalent { get; set; }

	public virtual double outerUOMCodeEquivalent { get; set; }
}
