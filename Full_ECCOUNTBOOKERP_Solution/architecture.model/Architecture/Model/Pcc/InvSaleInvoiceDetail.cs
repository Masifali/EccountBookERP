using System;

namespace Architecture.Model.pcc;

public class InvSaleInvoiceDetail
{
	public bool CommOnSale { get; set; }

	public bool IsApproved { get; set; }

	public bool IsFOC { get; set; }

	public DateTime GpDate { get; set; }

	public decimal CommissionAmount { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal ExpenseAmount { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal FreightAmount { get; set; }

	public decimal ItemAmount { get; set; }

	public decimal ItemAmountWithDisc { get; set; }

	public decimal ItemDiscountAmount { get; set; }

	public decimal ItemDiscountRate { get; set; }

	public decimal ItemNetAmount { get; set; }

	public decimal ItemNetWeight { get; set; }

	public decimal ItemQty { get; set; }

	public decimal ItemRate { get; set; }

	public decimal ItemWeight { get; set; }

	public decimal JournalAmount { get; set; }

	public decimal RateCut { get; set; }

	public decimal RateCutAmount { get; set; }

	public decimal VarientEquivalent { get; set; }

	public decimal WagesAmount { get; set; }

	public decimal ItemRateWithOutAddLess { get; set; }

	public decimal RateAddLess { get; set; }

	public decimal CgsRate { get; set; }

	public int ScheduleId { get; set; }

	public int ActionTypeId { get; set; }

	public int CityId { get; set; }

	public int CurrencyId { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int InvGdnDetailId { get; set; }

	public int InvGdnId { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public int ItemAttributeVarientId { get; set; }

	public int ItemDiscountTypeId { get; set; }

	public int ItemUomId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int LineId { get; set; }

	public int SaleOrderDetailId { get; set; }

	public int SaleOrderId { get; set; }

	public int WarehouseId { get; set; }

	public string RemarksDetail { get; set; }

	public string VehicleNo { get; set; }

	public virtual int SaleGLAC { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual int GdnNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemAttributeVarient { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string CityName { get; set; }

	public virtual string ItemDiscountType { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackEquivalent { get; set; }

	public virtual bool ItemWithVarient { get; set; }
}
