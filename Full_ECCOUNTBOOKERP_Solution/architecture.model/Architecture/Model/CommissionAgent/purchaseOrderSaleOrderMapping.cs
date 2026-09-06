using System;

namespace Architecture.Model.CommissionAgent;

public class purchaseOrderSaleOrderMapping
{
	public decimal itemNetWeight { get; set; }

	public decimal itemQty { get; set; }

	public int buyerId { get; set; }

	public int itemId { get; set; }

	public int purchaseOrderDetailId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int purchaseOrderSaleOrderMappingId { get; set; }

	public int saleOrderDetailId { get; set; }

	public int saleOrderMasterId { get; set; }

	public int sortNo { get; set; }

	public int supplierId { get; set; }

	public int actionTypeId { get; set; }

	public string Remarks { get; set; }

	public virtual string BuyerName { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual DateTime SaleOrderDate { get; set; }

	public virtual DateTime ValidityDate { get; set; }

	public virtual double SaleOrderQty { get; set; }

	public virtual double UsedSaleOrderQtyInPo { get; set; }

	public virtual double SaleOrderBalQty { get; set; }

	public virtual double SaleOrderWeight { get; set; }

	public virtual double UsedSaleOrderWeightInPo { get; set; }

	public virtual double SaleOrderBalWeight { get; set; }

	public virtual int PurchaseOrderNo { get; set; }

	public virtual DateTime PurchaseOrderDate { get; set; }

	public virtual double PurchaseOrderQty { get; set; }

	public virtual double UsedPurchaseOrderQtyInSo { get; set; }

	public virtual double PurchaseOrderBalQty { get; set; }

	public virtual double PurchaseOrderWeight { get; set; }

	public virtual double UsedPurchaseOrderWeightInSo { get; set; }

	public virtual double PurchaseOrderBalWeight { get; set; }
}
