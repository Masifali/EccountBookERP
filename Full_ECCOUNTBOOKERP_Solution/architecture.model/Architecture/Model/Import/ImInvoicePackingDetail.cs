namespace Architecture.Model.Import;

public class ImInvoicePackingDetail
{
	public double FcAmount { get; set; }

	public double NetWeight { get; set; }

	public double OuterQty { get; set; }

	public double RatePrice { get; set; }

	public double AddLessAmount { get; set; }

	public double Expenses { get; set; }

	public int BrandId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int Id { get; set; }

	public int InnerQty { get; set; }

	public int InnerQtyUomId { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int OuterQtyUomId { get; set; }

	public int PackingMaterialTypeId { get; set; }

	public int RateUomId { get; set; }

	public int WarehouseId { get; set; }

	public int JobLotId { get; set; }

	public string ItemDescriptionManual { get; set; }

	public string OuterPackDescription { get; set; }

	public virtual string PackMaterilaType { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual double RateUOM { get; set; }

	public virtual double ItemUOM { get; set; }

	public virtual double InnerUOM { get; set; }

	public virtual double OuterUOM { get; set; }
}
