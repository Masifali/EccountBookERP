namespace Architecture.Model.pcc;

public class InvGdnDetail
{
	public int LineId { get; set; }

	public int Id { get; set; }

	public int InvGdnId { get; set; }

	public int OrderTypeId { get; set; }

	public int OrderId { get; set; }

	public int OrderDetailId { get; set; }

	public int DoOrderDetailId { get; set; }

	public int WarehouseId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int ScheduleId { get; set; }

	public int ItemAttributeVarientId { get; set; }

	public decimal VarientEquivalent { get; set; }

	public int JobLotId { get; set; }

	public decimal ItemQty { get; set; }

	public decimal ItemWeight { get; set; }

	public decimal ItemNetWeight { get; set; }

	public double ConfirmQty { get; set; }

	public int CityId { get; set; }

	public bool IsFOC { get; set; }

	public string RemarksDetail { get; set; }

	public int ActionTypeId { get; set; }

	public virtual int OrderNo { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemAttributeVarient { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string CityName { get; set; }

	public virtual string OrderType { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackEquivalent { get; set; }

	public virtual decimal DoQty { get; set; }
}
