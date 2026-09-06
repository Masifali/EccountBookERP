using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImLcOrderShipmentSchedulePackingDetail
{
	public double InnerQty { get; set; }

	public decimal NetWeight { get; set; }

	public int CropYearId { get; set; }

	public int ExImLcOrderId { get; set; }

	public int ContractDetailId { get; set; }

	public int ExImLcOrderShipmentScheduleId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int NoOfBags { get; set; }

	public int PackingTypeId { get; set; }

	public int PackUomId { get; set; }

	public int ActionTypeId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual DateTime AttentiveLoadingDate { get; set; }

	public virtual string DestinationPort { get; set; }

	public virtual string ScheduleCode { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual double PackUomQtyEquivalent { get; set; }

	public virtual string Status { get; set; }

	public virtual string ReferredStatus { get; set; }

	public virtual List<ExImLcOrderShipmentSchedulePackingDetail> ExImLcOrderShipmentSchedulePackingDetailList { get; set; }
}
