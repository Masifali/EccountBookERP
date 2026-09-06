namespace Architecture.Model.Inventory;

public class GatePassGeneralDetail
{
	public decimal ItemQty { get; set; }

	public decimal Weight { get; set; }

	public int CropYearId { get; set; }

	public int GatePassGeneralId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int JobLotId { get; set; }

	public int PackingTypeId { get; set; }

	public int RefDocDetailId { get; set; }

	public int RefDocId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int WareHouseId { get; set; }

	public string RemarksDetail { get; set; }

	public string ItemName { get; set; }

	public string ItemCode { get; set; }

	public virtual string RefDocumentType { get; set; }

	public virtual int RefDocNo { get; set; }

	public virtual string ItemUomCode { get; set; }

	public virtual double ItemUomEquivalent { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string CropYear { get; set; }
}
