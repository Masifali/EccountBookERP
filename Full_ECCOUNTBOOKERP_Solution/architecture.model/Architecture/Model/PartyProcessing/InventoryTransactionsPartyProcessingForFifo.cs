using System;
using System.Collections.Generic;

namespace Architecture.Model.PartyProcessing;

public class InventoryTransactionsPartyProcessingForFifo
{
	public bool IsApproved { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime? EntryDate { get; set; }

	public DateTime? ModifyDate { get; set; }

	public double AddLess { get; set; }

	public double BillWeightIn { get; set; }

	public double BillWeightOut { get; set; }

	public double EbTotal { get; set; }

	public double EbUnit { get; set; }

	public double FreightAmount { get; set; }

	public double GrossWeight { get; set; }

	public double QtyIn { get; set; }

	public double QtyOut { get; set; }

	public double StockWeightIn { get; set; }

	public double StockWeightOut { get; set; }

	public int BranchesId { get; set; }

	public int CityId { get; set; }

	public int CompanyId { get; set; }

	public int CropYearId { get; set; }

	public int DocCodeNo { get; set; }

	public int EntryUserId { get; set; }

	public int GpNo { get; set; }

	public int InvPackingTypeId { get; set; }

	public int ItemId { get; set; }

	public int ItemUom { get; set; }

	public int JobLotId { get; set; }

	public int JobOrderId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int RefDocIdNo { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefRefDocIdNo { get; set; }

	public int RefRefDocSubIdNo { get; set; }

	public int RefRefDocumentTypeId { get; set; }

	public int RefWarehouseId { get; set; }

	public int StockPartyId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TransporterId { get; set; }

	public int WarehouseId { get; set; }

	public long Id { get; set; }

	public int LineId { get; set; }

	public string CalcType { get; set; }

	public string DetailRemarks { get; set; }

	public string TranRemarks { get; set; }

	public string VehicleNo { get; set; }

	public virtual List<InventoryTransactionsPartyProcessingForFifo> InventoryTransactionsPartyProcessingForFIFOlist { get; set; }
}
