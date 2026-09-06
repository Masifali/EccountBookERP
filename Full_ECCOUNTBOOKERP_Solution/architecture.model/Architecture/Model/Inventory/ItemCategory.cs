using System;
using System.Collections.Generic;
using Architecture.Model.AssetSchema;

namespace Architecture.Model.Inventory;

public class ItemCategory
{
	public bool CategoryStatus { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CGSAccountId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int InventoryAccountId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int RevenueAccountId { get; set; }

	public int SerialFrom { get; set; }

	public int SerialTo { get; set; }

	public int InventoryParentCategoriesId { get; set; }

	public int accumulatedDepreciationAcId { get; set; }

	public int ExpenseMaintenanceAccountId { get; set; }

	public int DepreciationExpenseAcId { get; set; }

	public int CapitalWipAcId { get; set; }

	public int ItemClassGroupId { get; set; }

	public string CategoryCode { get; set; }

	public string CategoryDescription { get; set; }

	public int ItemProductionStageId { get; set; }

	public int ItemVarietyNatureId { get; set; }

	public int usefullLifeInMonths { get; set; }

	public decimal depriciationrate { get; set; }

	public int AssetCategoryId { get; set; }

	public int depreciationMethodScheduleId { get; set; }

	public virtual List<CategoryAttribute> CategoryAttributeList { get; set; }

	public virtual List<CategoryDepreciationSchedule> CategoryDepreciationScheduleList { get; set; }

	public ItemCategory()
	{
		CategoryAttributeList = new List<CategoryAttribute>();
		CategoryDepreciationScheduleList = new List<CategoryDepreciationSchedule>();
	}
}
