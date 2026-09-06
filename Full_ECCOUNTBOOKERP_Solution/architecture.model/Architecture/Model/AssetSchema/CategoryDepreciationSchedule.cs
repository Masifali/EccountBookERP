namespace Architecture.Model.AssetSchema;

public class CategoryDepreciationSchedule
{
	public bool isActive { get; set; }

	public decimal rateofDepreciation { get; set; }

	public int categoryDepreciationScheduleId { get; set; }

	public int categoryId { get; set; }

	public int depreciationMethodScheduleId { get; set; }

	public int sortNo { get; set; }

	public int usefullLifeInMonths { get; set; }
}
