using System;

namespace Architecture.Model.CommissionAgent;

public class SupplierOfferBuyerInquiryMappingDetail
{
	public decimal itemNetWeight { get; set; }

	public decimal itemQty { get; set; }

	public int actionTypeId { get; set; }

	public int buyerId { get; set; }

	public int inquiryBookingDetailId { get; set; }

	public int inquiryBookingMasterId { get; set; }

	public int itemId { get; set; }

	public int purchaseOrderDetailId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int sortNo { get; set; }

	public int supplierId { get; set; }

	public int SupplierOfferBuyerInquiryMappingDetailId { get; set; }

	public int supplierOfferDetailId { get; set; }

	public int supplierOfferMasterId { get; set; }

	public string Remarks { get; set; }

	public virtual string BuyerName { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual int InquiryBookingNo { get; set; }

	public virtual DateTime InquiryBookingDate { get; set; }

	public virtual double InquiryBookingQty { get; set; }

	public virtual double InquiryBookingWeight { get; set; }

	public virtual int SupplierOfferNo { get; set; }

	public virtual DateTime SupplierOfferDate { get; set; }

	public virtual double SupplierOfferQty { get; set; }

	public virtual double SupplierOfferWeight { get; set; }

	public virtual int PurchaseOrderNo { get; set; }

	public virtual DateTime PurchaseOrderDate { get; set; }

	public virtual double PurchaseOrderQty { get; set; }

	public virtual double PurchaseOrderWeight { get; set; }

	public virtual double UsedinquiryBookingQtyInOffer { get; set; }

	public virtual double UsedinquiryBookingWeightInOffer { get; set; }

	public virtual double UsedinquiryBookingQtyInPoOffer { get; set; }

	public virtual double UsedinquiryBookingWeightInPoOffer { get; set; }

	public virtual double inquiryBookingBalQty { get; set; }

	public virtual double inquiryBookingBalWeight { get; set; }

	public virtual double UsedSupplierOfferQtyInInquiry { get; set; }

	public virtual double UsedSupplierOfferWeightInInquiry { get; set; }

	public virtual double SupplierOfferBalQty { get; set; }

	public virtual double SupplierOfferBalWeight { get; set; }
}
