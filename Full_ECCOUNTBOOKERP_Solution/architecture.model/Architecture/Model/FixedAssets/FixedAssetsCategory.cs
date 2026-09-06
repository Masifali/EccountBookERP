namespace Architecture.Model.FixedAssets;

public class FixedAssetsCategory
{
	public int AssetsAcmtDeprAccountId { get; set; }

	public int AssetsCateAccountId { get; set; }

	public int AssetsDeprAccountId { get; set; }

	public int ExpenseMaintenanceAccountId { get; set; }

	public int Id { get; set; }

	public int SerialNoFrom { get; set; }

	public int SerialNoTo { get; set; }

	public string AssetsCategoryCode { get; set; }

	public string AssetsCategoryDesc { get; set; }

	public string OtherDescription { get; set; }

	public virtual string AssetsCateAccount { get; set; }

	public virtual string AssetsDeprAccount { get; set; }

	public virtual string AssetsAcmtDeprAccount { get; set; }

	public virtual string ExpenseMaintenanceAccount { get; set; }
}
