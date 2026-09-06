using System;

namespace Architecture.Model.Inventory;

public class InvGrnDetail
{
	public double AdLsWeight { get; set; }

	public double EBWPerUnit { get; set; }

	public double EBWTotal { get; set; }

	public double EbPurAgainstWeight { get; set; }

	public double GrossWeight { get; set; }

	public double SupplierQty { get; set; }

	public double ItemQty { get; set; }

	public double NetBillWeight { get; set; }

	public double StockWeight { get; set; }

	public double WtCut { get; set; }

	public double WtCutTotal { get; set; }

	public double ScaleKart { get; set; }

	public double ScaleShortWeight { get; set; }

	public double SupplierShortWeight { get; set; }

	public double FreightAmount { get; set; }

	public double StockEbUnit { get; set; }

	public double StockEbTotal { get; set; }

	public int Id { get; set; }

	public int InvGrnId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int JobLotId { get; set; }

	public int PackingTypeId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int PurchaseOrderId { get; set; }

	public int PurchaseOrderDetailId { get; set; }

	public int PurchaserOrderNo { get; set; }

	public int SupplySchedulId { get; set; }

	public int GatePassInwarDetailId { get; set; }

	public int WarehouseId { get; set; }

	public int WareHouseFromId { get; set; }

	public int InvPurchasedemondId { get; set; }

	public int PurchaseDemondDetailId { get; set; }

	public string AreaCity { get; set; }

	public string CommentsDetail { get; set; }

	public DateTime? PackingDate { get; set; }

	public DateTime? ExpiryDate { get; set; }

	public int CropYearId { get; set; }

	public string CropYear { get; set; }

	public string LabReportRef { get; set; }

	public int ContractorId { get; set; }

	public int CityId { get; set; }

	public int WbTicketId { get; set; }

	public int LineId { get; set; }

	public int WeightCutOnId { get; set; }

	public int LabId { get; set; }

	public double QtyForWtCut { get; set; }

	public int GdnId { get; set; }

	public int GdnDetailId { get; set; }

	public int GdnDocumentTypeId { get; set; }

	public int AssetId { get; set; }

	public int ConditionId { get; set; }

	public int DeliveryChallanId { get; set; }

	public int DeliveryChallanDetailId { get; set; }

	public int ItemConditionId { get; set; }

	public int RackId { get; set; }

	public virtual int PurchaseOrder { get; set; }

	public virtual int PurchaseDemandNo { get; set; }

	public virtual int GdnNo { get; set; }

	public virtual double UOM { get; set; }

	public virtual string Item { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string JobLot { get; set; }

	public virtual string PackingType { get; set; }

	public virtual int SupplyScheduleNo { get; set; }

	public virtual int GpNo { get; set; }

	public virtual string WareHouseCode { get; set; }

	public virtual string ConditionStatus { get; set; }

	public virtual string WareHouseFromCode { get; set; }

	public virtual string DeliveryTerm { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string Contractor { get; set; }

	public virtual string WeightCutOn { get; set; }

	public virtual double WeightCutUom { get; set; }

	public virtual int LabNo { get; set; }

	public virtual DateTime? PurchaseOrderDate { get; set; }

	public virtual string AssetName { get; set; }

	public virtual string ItemCondition { get; set; }

	public virtual string rackName { get; set; }

	public virtual DateTime? PurchaseDemandDocDate { get; set; }

	public virtual double PurchaseDemandQty { get; set; }

	public virtual double UsedDemandQtyInPreBill { get; set; }

	public virtual double UsedDemandQtyInGrn { get; set; }

	public virtual double TotalUsedDemandQty { get; set; }

	public virtual double BalDemandQty { get; set; }

	public virtual double PurchaseOrderQty { get; set; }

	public virtual double UsedOrderQtyInGrn { get; set; }

	public virtual double UsedOrderQtyInPreBill { get; set; }

	public virtual double TotalUsedOrderQty { get; set; }

	public virtual double BalOrderQty { get; set; }

	public virtual int DeliveryChallanNo { get; set; }

	public virtual DateTime? DeliveryChallanDate { get; set; }

	public virtual double DeliveryChallanQty { get; set; }

	public virtual double UsedDeliveryChallanQtyGrn { get; set; }

	public virtual double BalDeliveryChallanQty { get; set; }

	public virtual int PurchasePreBillId { get; set; }

	public virtual int PurchasePreBillDetailId { get; set; }

	public virtual int PurchasePreBillNo { get; set; }

	public virtual double PurchasePreBillQty { get; set; }
}
