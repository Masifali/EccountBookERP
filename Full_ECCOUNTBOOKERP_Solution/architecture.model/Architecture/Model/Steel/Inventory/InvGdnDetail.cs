using System;

namespace Architecture.Model.Steel.Inventory;

public class InvGdnDetail
{
	public DateTime GpDate { get; set; }

	public decimal AdLsWeight { get; set; }

	public decimal GrossWeight { get; set; }

	public decimal ItemQty { get; set; }

	public decimal NetBillWeight { get; set; }

	public decimal StockWeight { get; set; }

	public decimal WtCut { get; set; }

	public decimal WtCutTotal { get; set; }

	public int ActionTypeId { get; set; }

	public int CityId { get; set; }

	public int ContractorId { get; set; }

	public int GatePassOutWardId { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int InvGdnId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int JobLotId { get; set; }

	public int LabSaleAnalysisHeaderId { get; set; }

	public int PackingTypeId { get; set; }

	public int RefDocIdNo { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int ReferencePartyId { get; set; }

	public int SaleOrderDetailId { get; set; }

	public int SaleOrderId { get; set; }

	public int SaleOrderNo { get; set; }

	public int WarehouseId { get; set; }

	public string LabReportRef { get; set; }

	public string RemarksDetail { get; set; }

	public string VehicleNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual double PackUom { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string CityName { get; set; }
}
