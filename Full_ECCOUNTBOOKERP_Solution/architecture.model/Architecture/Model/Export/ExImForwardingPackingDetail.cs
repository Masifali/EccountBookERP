using System;

namespace Architecture.Model.Export;

public class ExImForwardingPackingDetail
{
	public double AdLsWeight { get; set; }

	public double GrossWeight { get; set; }

	public double InnerQty { get; set; }

	public double NetWeight { get; set; }

	public double StockWeight { get; set; }

	public double OuterQty { get; set; }

	public double EbUnit { get; set; }

	public double EbTotal { get; set; }

	public double InnerEbUnit { get; set; }

	public double InnerEbTotal { get; set; }

	public double TotalPackingWeight { get; set; }

	public int ExImForwardingId { get; set; }

	public int ExImLcOrderId { get; set; }

	public int DoDetailId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int LotJobId { get; set; }

	public int CropYearId { get; set; }

	public int PackingMaterialId { get; set; }

	public int UOMScheduleIdInner { get; set; }

	public int UOMScheduleIdOuter { get; set; }

	public int WarehouseId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocIdNo { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int WarehouseToId { get; set; }

	public int ContractDetailId { get; set; }

	public int InvoiceId { get; set; }

	public int InvoiceDetailId { get; set; }

	public int LineId { get; set; }

	public int ActionTypeId { get; set; }

	public int ContainerId { get; set; }

	public string ContainerNo { get; set; }

	public string ItemDescription { get; set; }

	public string LabReportNo { get; set; }

	public string OtherInfo { get; set; }

	public string OuterPackDescription { get; set; }

	public string SealNo { get; set; }

	public string ProductionNo { get; set; }

	public string PackingExpiryDate { get; set; }

	public virtual string InnerUOM { get; set; }

	public virtual string OuterUOM { get; set; }

	public virtual double InnerEquivalent { get; set; }

	public virtual double OuterEquivalent { get; set; }

	public virtual string Packtype { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string JobLotName { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string WareHouseToName { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string LcOrderNo { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual int RefDocNo { get; set; }

	public virtual string RefDocType { get; set; }

	public virtual DateTime? ContractDate { get; set; }
}
