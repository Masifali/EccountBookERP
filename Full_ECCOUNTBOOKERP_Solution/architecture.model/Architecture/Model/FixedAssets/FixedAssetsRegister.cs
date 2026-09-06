using System;

namespace Architecture.Model.FixedAssets;

public class FixedAssetsRegister
{
	public int AssetsDepartmentId { get; set; }

	public int BranchesId { get; set; }

	public string Vendor { get; set; }

	public int OrganizationId { get; set; }

	public string AssetCondition { get; set; }

	public int AssetDepriciationAcId { get; set; }

	public string Manufacturer { get; set; }

	public int CompanyId { get; set; }

	public string Pic1Path { get; set; }

	public string Pic2Path { get; set; }

	public string AssetSerialNo { get; set; }

	public string AssetName { get; set; }

	public string Brand { get; set; }

	public int Id { get; set; }

	public double PurchasePrice { get; set; }

	public string AssetsType { get; set; }

	public string AssetLocationId { get; set; }

	public int ItemId { get; set; }

	public bool IsApproved { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime PurchaseDate { get; set; }

	public double CurrentValue { get; set; }

	public DateTime ApprovedDate { get; set; }

	public string AssetStatus { get; set; }

	public int AssetGLAccountId { get; set; }

	public int ExpenseMaintenanceAccountId { get; set; }

	public string MakeDesc { get; set; }

	public DateTime ExpiryDate { get; set; }

	public int EntryUserId { get; set; }

	public int FixedAssetsCategoryId { get; set; }

	public int ProjectsId { get; set; }

	public string ModelDesc { get; set; }

	public int ApprovedUserId { get; set; }

	public int AssetAcmltvAcId { get; set; }

	public int UseableLifeMonth { get; set; }

	public virtual string AssetsCategory { get; set; }

	public virtual string AssetAcmltvAc { get; set; }

	public virtual string AssetDepriciationAc { get; set; }

	public virtual string AssetGLAccount { get; set; }

	public virtual string DepartmentName { get; set; }

	public virtual string ItemName { get; set; }
}
