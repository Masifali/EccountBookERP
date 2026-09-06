using System;

namespace Architecture.Model.Inventory;

public class InvGdnDetail
{
	public double AdLsWeight { get; set; }

	public double EBWPerUnit { get; set; }

	public double EBWTotal { get; set; }

	public double EbUnitStock { get; set; }

	public double EbTotalStock { get; set; }

	public double GrossWeight { get; set; }

	public double ItemQty { get; set; }

	public double NetBillWeight { get; set; }

	public double StockWeight { get; set; }

	public double WtCut { get; set; }

	public double WtCutTotal { get; set; }

	public double ShortWeight { get; set; }

	public double ItemRate { get; set; }

	public double ItemAmount { get; set; }

	public bool IsAssetItem { get; set; }

	public int DeliveryScheduleId { get; set; }

	public int Id { get; set; }

	public int InvGdnId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int ItemVariantId { get; set; }

	public int CastingTypeId { get; set; }

	public int ProductionStageId { get; set; }

	public int JobLotId { get; set; }

	public int PackingTypeId { get; set; }

	public int SaleOrderId { get; set; }

	public int SaleOrderDetailId { get; set; }

	public int RefPartyId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocIdNo { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int WarehouseId { get; set; }

	public int ItemConditionId { get; set; }

	public int GrnId { get; set; }

	public int GrnDetailId { get; set; }

	public int GrnDocumentTypeId { get; set; }

	public int WareHouseToId { get; set; }

	public int CityId { get; set; }

	public int LineId { get; set; }

	public int DeliveryTypeId { get; set; }

	public int DepartmentId { get; set; }

	public int AssetId { get; set; }

	public int WbTicketId { get; set; }

	public string AreaCity { get; set; }

	public string CommentsDetail { get; set; }

	public string ContainerNo { get; set; }

	public string CropYear { get; set; }

	public string LabReportRef { get; set; }

	public string VehicleNo { get; set; }

	public string ItemDescription { get; set; }

	public int GpNo { get; set; }

	public int RateUomId { get; set; }

	public int InvDeliveryOrderId { get; set; }

	public int InvDeliveryOrderDetailId { get; set; }

	public int ActionTypeId { get; set; }

	public int CropYearId { get; set; }

	public int OrderTypeId { get; set; }

	public int BrandItemId { get; set; }

	public int RackId { get; set; }

	public DateTime GpDate { get; set; }

	public int SecondaryUomId { get; set; }

	public decimal SecondaryUomQty { get; set; }

	public virtual double SecondaryUomEquivalent { get; set; }

	public virtual double PerItemSecondaryUomQty { get; set; }

	public virtual string SecondaryUomCode { get; set; }

	public virtual string rackName { get; set; }

	public virtual string SaleOrderNo { get; set; }

	public virtual string Item { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string BrandItemName { get; set; }

	public virtual string BrandItemCode { get; set; }

	public virtual string JobLot { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string WareHouseCode { get; set; }

	public virtual string WareHouseToCode { get; set; }

	public virtual double UOMEquivalent { get; set; }

	public virtual double ItemWeight { get; set; }

	public virtual int DeliveryScheduleNo { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string CastingType { get; set; }

	public virtual string VarientDescription { get; set; }

	public virtual string ProductionProcessStage { get; set; }

	public virtual string DepartmentName { get; set; }

	public virtual string AssetName { get; set; }

	public virtual string DeliveryType { get; set; }

	public virtual string RefDocType { get; set; }

	public virtual int RefDocNo { get; set; }

	public virtual int GrnNo { get; set; }

	public virtual int ItemCategoryId { get; set; }

	public virtual string ItemCategory { get; set; }

	public virtual string ItemCondition { get; set; }
}
