namespace Architecture.Model.Export;

public class ExImLcOrderPackingDetail
{
	public double InnerQty { get; set; }

	public double NetWeight { get; set; }

	public double NoOfUnit { get; set; }

	public double OuterQty { get; set; }

	public double RatePrice { get; set; }

	public double TotalAmount { get; set; }

	public double NoofContainer { get; set; }

	public double PackingWeight { get; set; }

	public double NoofBagsPerContainer { get; set; }

	public double PackingTotalWeight { get; set; }

	public double GrossWeight { get; set; }

	public double PalletQty { get; set; }

	public int ExImItemId { get; set; }

	public int PmItemId { get; set; }

	public int ExImLcOrderId { get; set; }

	public int CropYearId { get; set; }

	public int Id { get; set; }

	public int AmountCalulationId { get; set; }

	public int InvPackingMaterialTypeId { get; set; }

	public int ItemSpecificationId { get; set; }

	public int ItemBrandId { get; set; }

	public int UOMScheduleIdInner { get; set; }

	public int UOMScheduleIdOuter { get; set; }

	public int UOMScheduleIdRate { get; set; }

	public int ActionTypeId { get; set; }

	public int ExImFarmingNTradeId { get; set; }

	public int ExImTradeTypeId { get; set; }

	public int ExImFarmingTypeId { get; set; }

	public string Other_Description { get; set; }

	public string OuterPackDescription { get; set; }

	public string ContainerSize { get; set; }

	public string PackingExpiryDate { get; set; }

	public string CommodityDetail { get; set; }

	public string PackingDetail { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PmItemCode { get; set; }

	public virtual string PmItemName { get; set; }

	public virtual string BrandName { get; set; }

	public virtual string BrandCode { get; set; }

	public virtual string ItemSpecificationGroup { get; set; }

	public virtual string ExImPackMaterialTypeName { get; set; }

	public virtual string CropYear { get; set; }

	public virtual double UOMScheduleIdOuterName { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double QtyEquivalent { get; set; }

	public virtual string InnerUom { get; set; }

	public virtual double UOMScheduleIdInnerName { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double UOMScheduleIdRateName { get; set; }

	public virtual string FarmingNTrade { get; set; }
}
