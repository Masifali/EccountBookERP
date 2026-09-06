using System;

namespace Architecture.Model.Production;

public class InvProductionJobOrderInput
{
	public double NetWeight { get; set; }

	public double Qty { get; set; }

	public double ItemRate { get; set; }

	public double ItemAmount { get; set; }

	public double WagesRate { get; set; }

	public double WagesAmount { get; set; }

	public int Id { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int UomSchIdQty { get; set; }

	public int WarehouseId { get; set; }

	public int RateUomId { get; set; }

	public int WagesAccountId { get; set; }

	public int PackingTypeId { get; set; }

	public int CropYearId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocIdNo { get; set; }

	public int RefDocSubIdNo { get; set; }

	public DateTime? DocDate { get; set; }

	public int ActionTypeId { get; set; }

	public int DocNo { get; set; }

	public string BatchCrop { get; set; }

	public string RemarksInputDt { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string JobLotCode { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual double UOMCodeEquivalent { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double RateUomEquivalent { get; set; }

	public virtual string WagesAccountName { get; set; }
}
