using System;

namespace Architecture.Model.CommissionAgent;

public class grnSupplierLoadingDetail
{
	public decimal addLessWeight { get; set; }

	public decimal ebwPerUnit { get; set; }

	public decimal ebwTotal { get; set; }

	public decimal loadingQty { get; set; }

	public decimal netBillWeight { get; set; }

	public decimal wbGrossWeight { get; set; }

	public int actionTypeId { get; set; }

	public int buyerId { get; set; }

	public int cropYearId { get; set; }

	public int DeliverToAddressId { get; set; }

	public int DeliverToPartyId { get; set; }

	public int grnSupplierLoadingDetailId { get; set; }

	public int grnSupplierLoadingMasterId { get; set; }

	public int inquiryBookingDetailId { get; set; }

	public int inquiryBookingMasterId { get; set; }

	public int inventoryParentCategoryId { get; set; }

	public int itemId { get; set; }

	public int loadingCityId { get; set; }

	public int modifyUserId { get; set; }

	public int packingTypeId { get; set; }

	public int packUomId { get; set; }

	public int purchaseOrderDetailId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int saleOrderDetailId { get; set; }

	public int saleOrderMasterId { get; set; }

	public int EBWeightDeductionTermId { get; set; }

	public int purchaseOrderSaleOrderMappingId { get; set; }

	public int sortNo { get; set; }

	public int supplierOfferDetailId { get; set; }

	public int supplierOfferId { get; set; }

	public int unloadingCityId { get; set; }

	public string cropYear { get; set; }

	public string remarks { get; set; }

	public string warningRemarks { get; set; }

	public string DeliverToAddress { get; set; }

	public virtual int PurchaseOrderNo { get; set; }

	public virtual DateTime? PODeliveryStartDate { get; set; }

	public virtual DateTime? POExpiryDate { get; set; }

	public virtual DateTime? SODeliveryStartDate { get; set; }

	public virtual DateTime? SOExpiryDate { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual string DeliverToPartyName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string InvParentCateDescription { get; set; }

	public virtual string PackSize { get; set; }

	public virtual double PEquivalent { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string BuyerName { get; set; }

	public virtual string EBWeightDeductionTerm { get; set; }
}
