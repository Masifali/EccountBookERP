using System;

namespace Architecture.Model.pcc;

public class InvPurchaseInvoiceDetail
{
	public bool IsApproved { get; set; }

	public DateTime GpDate { get; set; }

	public decimal CommissionAmount { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal ExpenseAmount { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal FreightAmount { get; set; }

	public decimal ItemAmount { get; set; }

	public decimal ItemNetAmount { get; set; }

	public decimal ItemNetWeight { get; set; }

	public decimal ItemQty { get; set; }

	public decimal ItemRateWithoutRateCut { get; set; }

	public decimal ItemRate { get; set; }

	public decimal ItemWeight { get; set; }

	public decimal JournalAmount { get; set; }

	public decimal RateCut { get; set; }

	public decimal RateCutAmount { get; set; }

	public decimal TaxAmount { get; set; }

	public decimal TaxPercent { get; set; }

	public decimal VarientEquivalent { get; set; }

	public int ActionTypeId { get; set; }

	public int CityId { get; set; }

	public int CurrencyId { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int InvGrnDetailId { get; set; }

	public int InvGrnId { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public int ItemAttributeVarientId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int LineId { get; set; }

	public int PurchaseOrderDetailId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int TaxTypeId { get; set; }

	public int WarehouseId { get; set; }

	public string RemarksDetail { get; set; }

	public string VehicleNo { get; set; }

	public int ItemDiscountTypeId { get; set; }

	public double ItemDiscountRate { get; set; }

	public double ItemDiscountAmount { get; set; }

	public double ItemAmountWithOutDiscount { get; set; }

	public virtual int OrderNo { get; set; }

	public virtual int GrnNo { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string ItemAttributeVarient { get; set; }

	public virtual string TaxName { get; set; }

	public virtual string CityName { get; set; }

	public virtual string ItemDiscountType { get; set; }
}
