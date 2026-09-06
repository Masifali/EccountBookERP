using System;

namespace Architecture.Model.PartyProcessing;

public class InvGrnPartyProcessingDetail
{
	public double AdLsWeight { get; set; }

	public double EBWPerUnit { get; set; }

	public double EBWTotal { get; set; }

	public double GrossWeight { get; set; }

	public double ItemQty { get; set; }

	public double NetBillWeight { get; set; }

	public double StockWeight { get; set; }

	public int CityId { get; set; }

	public int CropYearId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int Id { get; set; }

	public int InvGrnPartyProcessingId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int JobLotId { get; set; }

	public int PackingTypeId { get; set; }

	public int WarehouseId { get; set; }

	public int ActionTypeId { get; set; }

	public string CommentsDetail { get; set; }

	public string ContainerNo { get; set; }

	public string SealNo { get; set; }

	public int LineId { get; set; }

	public int DeliveryOrderId { get; set; }

	public int DeliveryOrderDetailId { get; set; }

	public int DoDocumentTypeId { get; set; }

	public int InvoiceDocumentTypeId { get; set; }

	public int InvoiceId { get; set; }

	public int InvoiceDetailId { get; set; }

	public virtual int DeliveryOrderNo { get; set; }

	public virtual string InvoiceDocumentType { get; set; }

	public virtual int InvoiceDocNo { get; set; }

	public virtual string InvoiceManualBillNo { get; set; }

	public virtual double InvoiceQty { get; set; }

	public virtual double InvoiceWeight { get; set; }

	public virtual double InvoiceEbUnit { get; set; }

	public virtual double InvoiceEbTotal { get; set; }

	public virtual DateTime? InvoiceDocDate { get; set; }

	public virtual DateTime? DeliveryOrderDate { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual double Equivalent { get; set; }

	public virtual string CityName { get; set; }
}
