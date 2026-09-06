using System;

namespace Architecture.Model.Inventory;

public class InvDeliveryOrderdetail
{
	public double DoQty { get; set; }

	public double DoWeight { get; set; }

	public double LoadingQty { get; set; }

	public double LoadingWeight { get; set; }

	public double PackingWeight { get; set; }

	public double TotalPackingWeight { get; set; }

	public double GrossWeight { get; set; }

	public double StockWeight { get; set; }

	public double OtherWeight { get; set; }

	public double InnerQty { get; set; }

	public double InnerUomId { get; set; }

	public double InnerEbUnit { get; set; }

	public double InnerEbTotal { get; set; }

	public double AccessWtSet { get; set; }

	public double OuterEbTotal { get; set; }

	public int Id { get; set; }

	public int InvDeliveryOrderId { get; set; }

	public int InvPackingTypeId { get; set; }

	public int ItemId { get; set; }

	public int PackUomId { get; set; }

	public int CastingTypeId { get; set; }

	public int ItemVariantId { get; set; }

	public int SaleOrderId { get; set; }

	public int SaleOrderDetailId { get; set; }

	public int InvoiceDetailId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int WarehouseId { get; set; }

	public int WareHouseToId { get; set; }

	public int JobLotId { get; set; }

	public int ToJobLotId { get; set; }

	public int RefPartyId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocIdNo { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int CropYearId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int ActionTypeId { get; set; }

	public int BagTypeId { get; set; }

	public int ContainerId { get; set; }

	public int DeliveryTypeId { get; set; }

	public int AssetId { get; set; }

	public string LoadingRemarks { get; set; }

	public string ContainerRemarks { get; set; }

	public string InspectionRemarks { get; set; }

	public string ItemDiscription { get; set; }

	public int DeliveryScheduleId { get; set; }

	public int DeliveryScheduleDetailId { get; set; }

	public int ThirdPartyAnalysisSubId { get; set; }

	public int ThirdPartyAnalysisId { get; set; }

	public bool IsAssetItem { get; set; }

	public virtual string ThirdPartyAnalysisNo { get; set; }

	public virtual string ThirdPartyAnalysisSubNo { get; set; }

	public virtual string DeliveryType { get; set; }

	public virtual string AssetName { get; set; }

	public virtual string VariantDescription { get; set; }

	public virtual string CastingType { get; set; }

	public virtual string Container { get; set; }

	public virtual string SupplierCustomer { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string WareHouseToName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string OrderNo { get; set; }

	public virtual int OrderPaymentTermId { get; set; }

	public virtual string OrderPaymentTerm { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string PackUOM { get; set; }

	public virtual string BagType { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual double PUomEquivalent { get; set; }

	public virtual string InnerPackUom { get; set; }

	public virtual string ToJobLot { get; set; }

	public virtual double InnerPackUomEquivalent { get; set; }

	public virtual double RateUom { get; set; }

	public virtual double Rate { get; set; }

	public virtual int DocumentTypeId { get; set; }

	public virtual int RefDocNo { get; set; }

	public virtual double RefDocQty { get; set; }

	public virtual double RefDocWeight { get; set; }

	public virtual string RefDocumentType { get; set; }

	public virtual string RefManualBillNo { get; set; }

	public virtual int DeliveryScheduleCustomerNo { get; set; }

	public virtual DateTime? DeliveryScheduleCustomerDate { get; set; }

	public virtual int ItemCategoryId { get; set; }

	public virtual string ItemCategory { get; set; }
}
