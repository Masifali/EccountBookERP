namespace Architecture.Model.PurchaseTrading;

public class InvStoreReturnDetail
{
	public double ItemAmount { get; set; }

	public double ItemQty { get; set; }

	public double ItemRate { get; set; }

	public int AssetsId { get; set; }

	public int CrAccountId { get; set; }

	public int DepartmentId { get; set; }

	public int Id { get; set; }

	public int InvStoreReturnId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int LineId { get; set; }

	public int Warehouseid { get; set; }

	public string ReamarksDetail { get; set; }

	public int ItemConditionId { get; set; }

	public int IssuanceId { get; set; }

	public int IssuanceDetailId { get; set; }

	public int RackId { get; set; }

	public int SecondaryUomId { get; set; }

	public decimal SecondaryUomQty { get; set; }

	public decimal SecondaryUomItemRate { get; set; }

	public virtual string AssetName { get; set; }

	public virtual string DepartmentName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual double Equivalent { get; set; }

	public virtual string UomCode { get; set; }

	public virtual double SecondaryUomEquivalent { get; set; }

	public virtual double PerItemSecondaryUomQty { get; set; }

	public virtual string SecondaryUomCode { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string CompanyName { get; set; }

	public virtual string CreditAc { get; set; }

	public virtual int IssuanceNo { get; set; }

	public virtual string ItemCondition { get; set; }

	public virtual double BalQty { get; set; }

	public virtual double IssuanceRate { get; set; }

	public virtual string rackName { get; set; }
}
