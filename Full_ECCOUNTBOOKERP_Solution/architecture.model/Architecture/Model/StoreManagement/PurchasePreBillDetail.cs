using System;

namespace Architecture.Model.StoreManagement;

public class PurchasePreBillDetail
{
	public DateTime? PartyBillDate { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public double ItemAmountWithoutDiscount { get; set; }

	public double DiscountAmount { get; set; }

	public double ItemAmount { get; set; }

	public double ExpenseAmount { get; set; }

	public double ItemNetAmount { get; set; }

	public int ActionTypeId { get; set; }

	public int ItemId { get; set; }

	public int PurchaseDemandDetailId { get; set; }

	public int PurchaseDemandHeaderId { get; set; }

	public int PurchaseOrderDetailId { get; set; }

	public int PurchaseOrderHeaderId { get; set; }

	public int PurchasePreBillDetailId { get; set; }

	public int PurchasePreBillHeaderId { get; set; }

	public int ReferencePartyId { get; set; }

	public int UomId { get; set; }

	public int ItemConditionId { get; set; }

	public int VendorSupplierId { get; set; }

	public int WareHouseId { get; set; }

	public string BiltyNo { get; set; }

	public string PartyBillNo { get; set; }

	public string VehicleNo { get; set; }

	public string RemarksDetail { get; set; }

	public virtual int PurchaseDemandDocNo { get; set; }

	public virtual double PurchaseDemandQty { get; set; }

	public virtual double AlreadyUsedDemandQtyInGrn { get; set; }

	public virtual double AlreadyUsedDemandQtyInPreBill { get; set; }

	public virtual double TotalPurchasedForDemand { get; set; }

	public virtual double BalDemandQty { get; set; }

	public virtual int PurchaseOrderDocNo { get; set; }

	public virtual double PurchaseOrderQty { get; set; }

	public virtual double AlreadyUsedOrderQtyInGrn { get; set; }

	public virtual double AlreadyUsedOrderQtyInPreBill { get; set; }

	public virtual double TotalPurchasedForOrder { get; set; }

	public virtual double BalOrderQty { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string VendorSupplierName { get; set; }

	public virtual string ReferencePartyName { get; set; }

	public virtual string Uom { get; set; }

	public virtual string ItemCondition { get; set; }

	public virtual double UomEquivalent { get; set; }
}
