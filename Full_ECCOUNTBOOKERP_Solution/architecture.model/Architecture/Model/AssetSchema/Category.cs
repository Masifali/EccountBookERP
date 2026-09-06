using System;

namespace Architecture.Model.AssetSchema;

public class Category
{
	public bool IsActive { get; set; }

	public bool IsApproved { get; set; }

	public DateTime ApproveDate { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime LastModifyDate { get; set; }

	public decimal depriciationrate { get; set; }

	public int ApproveBy { get; set; }

	public int categoryId { get; set; }

	public int companyId { get; set; }

	public int CreatedBy { get; set; }

	public int depreciationMethodScheduleId { get; set; }

	public int LastModifyBy { get; set; }

	public int organizationId { get; set; }

	public int parentCategoryId { get; set; }

	public int SeriesFrom { get; set; }

	public int SeriesTo { get; set; }

	public int usefullLifeInMonths { get; set; }

	public long accumulatedAccountId { get; set; }

	public long depriciationAccountId { get; set; }

	public long expenseAccountId { get; set; }

	public long GLAccountId { get; set; }

	public string categoryCode { get; set; }

	public string categoryDescription { get; set; }

	public string moreDescription { get; set; }
}
