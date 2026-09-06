using System;

namespace Architecture.Model.PartyProcessing;

public class InvFoodProductionPartyProcessingDetail
{
	public double Qty { get; set; }

	public double GrossWeight { get; set; }

	public double EbUnit { get; set; }

	public double EbTotal { get; set; }

	public double Weight { get; set; }

	public int ActionTypeId { get; set; }

	public int CropYearId { get; set; }

	public int Id { get; set; }

	public int InvFoodProductionPartyProcessingId { get; set; }

	public int InvJobOrderId { get; set; }

	public string InvJobOrderNo { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int JobLotId { get; set; }

	public int PackingtypeId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int ReferencePartyId { get; set; }

	public int StockPartyId { get; set; }

	public int WarehouseId { get; set; }

	public int ActionId { get; set; }

	public string EntryType { get; set; }

	public string Remarks { get; set; }

	public virtual string StockParty { get; set; }

	public virtual string ReferencePartyName { get; set; }

	public virtual string CropBatch { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double Equivalent { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ManualBillNo { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual DateTime DocDate { get; set; }

	public virtual int DocNo { get; set; }

	public virtual string JobLotCode { get; set; }

	public virtual string PackTypeCode { get; set; }
}
