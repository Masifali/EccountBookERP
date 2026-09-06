using System;

namespace Architecture.Model.AssetSchema;

public class AssetRegister
{
	public bool isApproved { get; set; }

	public bool isDepreciable { get; set; }

	public DateTime approvedDate { get; set; }

	public DateTime entryDate { get; set; }

	public DateTime expiryDate { get; set; }

	public DateTime LastModifyDate { get; set; }

	public DateTime purchaseDate { get; set; }

	public double currentValue { get; set; }

	public double purchasePrice { get; set; }

	public int accumulateddepriciationAcId { get; set; }

	public int approvedUserId { get; set; }

	public int branchesId { get; set; }

	public int brandId { get; set; }

	public int capitalWIPAccountId { get; set; }

	public int ExpenseMaintenanceAccountId { get; set; }

	public int companyId { get; set; }

	public int departmentId { get; set; }

	public int depriciationExpenseAcId { get; set; }

	public int entryUserId { get; set; }

	public int glAccountId { get; set; }

	public int LastModifyById { get; set; }

	public int organizationId { get; set; }

	public int projectsId { get; set; }

	public int useableLifeMonth { get; set; }

	public long assetRegisterId { get; set; }

	public long categoryId { get; set; }

	public long itemId { get; set; }

	public long manufacturerId { get; set; }

	public long vendorId { get; set; }

	public string assetCode { get; set; }

	public string assetName { get; set; }

	public string assetStatus { get; set; }

	public string assetsTypeId { get; set; }

	public string brandName { get; set; }

	public int conditionId { get; set; }

	public string locationBranch { get; set; }

	public string makeDesc { get; set; }

	public string manufacturerName { get; set; }

	public string modelDesc { get; set; }

	public string pic1Path { get; set; }

	public string pic2Path { get; set; }

	public string serialNo { get; set; }

	public string vendorName { get; set; }

	public virtual int depreciationMethodScheduleId { get; set; }

	public virtual decimal depriciationrate { get; set; }
}
