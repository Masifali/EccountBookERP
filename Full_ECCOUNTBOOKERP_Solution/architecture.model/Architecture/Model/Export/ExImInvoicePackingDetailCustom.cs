using System;

namespace Architecture.Model.Export;

public class ExImInvoicePackingDetailCustom
{
	public double FcAmount { get; set; }

	public double NetWeight { get; set; }

	public double OuterQty { get; set; }

	public double RatePrice { get; set; }

	public double RateAddLess { get; set; }

	public double RateWithoutAddLess { get; set; }

	public double PackingWeight { get; set; }

	public double TotalPackingWeight { get; set; }

	public double GrossWeight { get; set; }

	public double NoofContainers { get; set; }

	public double NoofBagsPerContainer { get; set; }

	public double MTon { get; set; }

	public double OtherRate { get; set; }

	public double OtherAmount { get; set; }

	public string OtherHsCode { get; set; }

	public int ExImFarmingNTradeId { get; set; }

	public int ExImTradeTypeId { get; set; }

	public int ExImFarmingTypeId { get; set; }

	public int LabInspectionId { get; set; }

	public string HsCode { get; set; }

	public int PmItemId { get; set; }

	public int BrandId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int ContractId { get; set; }

	public int ContractDetailId { get; set; }

	public int Id { get; set; }

	public double InnerQty { get; set; }

	public int InnerQtyUomId { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int CropYearId { get; set; }

	public int OuterQtyUomId { get; set; }

	public int PackingMaterialTypeId { get; set; }

	public int RateUomId { get; set; }

	public int JobLotId { get; set; }

	public int ItemSpicificationId { get; set; }

	public int PreInvoiceId { get; set; }

	public int PreInvoiceDetailId { get; set; }

	public int LineId { get; set; }

	public int ContractScheduleId { get; set; }

	public int ContractScheduleDetailId { get; set; }

	public int ActionTypeId { get; set; }

	public int AmountCalulationId { get; set; }

	public string ItemDescriptionManual { get; set; }

	public string PackingExpiryDate { get; set; }

	public string ProductionNo { get; set; }

	public string OuterPackDescription { get; set; }

	public string HealthPermitNoDetail { get; set; }

	public string ItemCommodityDetail { get; set; }

	public string BuyerLotNo { get; set; }

	public string BuyerHSCode { get; set; }

	public string ProductCode { get; set; }

	public string ContractNoShipmentWise { get; set; }

	public string Remarks { get; set; }

	public double PalletQty { get; set; }

	public virtual double ContractRate { get; set; }

	public virtual double InnerEquivalent { get; set; }

	public virtual double OuterEquivalent { get; set; }

	public virtual double OuterQtyEquivalent { get; set; }

	public virtual double ScheduleFCL { get; set; }

	public virtual string PackMaterilaType { get; set; }

	public virtual string FarmingNTrade { get; set; }

	public virtual int ItemCategoryId { get; set; }

	public virtual string ItemCategory { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string BrandName { get; set; }

	public virtual string BrandCode { get; set; }

	public virtual string PmItemName { get; set; }

	public virtual string PmItemCode { get; set; }

	public virtual string ItemSpecificationGroup { get; set; }

	public virtual string RateUOM { get; set; }

	public virtual double RateEquivalent { get; set; }

	public virtual string ItemUOM { get; set; }

	public virtual DateTime ProformaDocDate { get; set; }

	public virtual DateTime PreInvoiceDate { get; set; }

	public virtual DateTime AttentiveLoadingDate { get; set; }

	public virtual string InnerUOM { get; set; }

	public virtual string OuterUOM { get; set; }

	public virtual string CropYear { get; set; }

	public virtual int ContractType { get; set; }

	public virtual int ProformaDocNo { get; set; }

	public virtual string PriRefNo { get; set; }

	public virtual string PreInvoiceNo { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string ContractScheduleNo { get; set; }
}
