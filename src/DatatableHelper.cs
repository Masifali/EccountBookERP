using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Main;
using Architecture.BLL.Mfg;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Helper
{
using Architecture.WinApp.Common;
using CommonServices = global::Architecture.WinApp.Common.CommonServices;
using Bank = global::Architecture.Model.Bank;
using City = global::Architecture.BLL.City;
using CheqBookHeader = global::Architecture.Model.Accounts.CheqBookHeader;
using CheqBookDetail = global::Architecture.Model.Accounts.CheqBookDetail;
using COAAllocation = global::Architecture.Model.Accounts.COAAllocation;
using ChartOfAccountMultiLingo = global::Architecture.Model.Accounts.ChartOfAccountMultiLingo;
using TaxesTypes = global::Architecture.Model.Inventory.TaxesTypes;
using SupplierCustomerShipToAddress = global::Architecture.Model.Inventory.SupplierCustomerShipToAddress;
using AccountsOpeningBalances = global::Architecture.BLL.Accounts.AccountsOpeningBalances;
using ChartofAccount = global::Architecture.BLL.Accounts.ChartofAccount;
using ShortCutKeyPopUp = global::Architecture.WinApp.Common.ShortCutKeyPopUp;
using Constants = global::Architecture.WinApp.Common.Constants;
using GridEX_Helper = global::Architecture.WinApp.Common.GridEX_Helper;
using InfragisticsHelper = global::Architecture.WinApp.Common.InfragisticsHelper;
using HierarchicalMode = Janus.Windows.GridEX.HierarchicalMode;
using SelfReferencingAggregationMode = Janus.Windows.GridEX.SelfReferencingAggregationMode;


public static class DatatableHelper
{
	public static DataTable AllShipToAddressDbCall(UserAccount userAccount)
	{
		try
		{
			return new DataTable();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TaxTypeDbCallByItemId(int ItemId, DateTime DocDate)
	{
		try
		{
			DataTable dtTax = new DataTable();
			dtTax.Columns.Add("Id");
			dtTax.Columns.Add("TaxType");
			dtTax.Columns.Add("TaxPrcnt");
			DataTable dt = new DataTable();
			if (dt.Rows.Count > 0)
			{
				dtTax.Rows.Add(dt.Rows[0]["TaxNameId"], dt.Rows[0]["TaxName"], dt.Rows[0]["TaxPercent"]);
			}
			return dtTax;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TaxTypesDbCall(UserAccount userAccount)
	{
		try
		{
			return new DataTable();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetTaxAccountsFromGlobal(int[] withTypeIds = null)
	{
		int[] defaultTypeIds = new int[7] { 3, 6, 8, 16, 17, 18, 19 };
		int[] finalTypeIds = ((withTypeIds != null && withTypeIds.Length != 0) ? defaultTypeIds.Concat(withTypeIds).Distinct().ToArray() : defaultTypeIds);
		DataTable dt = new DataTable();
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("AccountTitle", typeof(string));
		dt.Columns.Add("AccountCode", typeof(string));
		dt.Columns.Add("ParentAccountTitle", typeof(string));
		List<getGlobalAllAccountsWithCustomGroup> allAccounts = clsGlobalVariables.AllAccountsWithCustomGroupId;
		if (allAccounts == null || allAccounts.Count == 0)
		{
			return dt;
		}
		HashSet<int> typeSet = new HashSet<int>(finalTypeIds);
		foreach (getGlobalAllAccountsWithCustomGroup account in from a in allAccounts
			where typeSet.Contains(a.AccountTypeId)
			group a by a.ChartOfAccountId into g
			select g.First())
		{
			dt.Rows.Add(Conversion.ToInt((object)account.ChartOfAccountId), account.AccountTitle ?? string.Empty, account.AccountCode ?? string.Empty, account.ParentAccountTitle ?? string.Empty);
		}
		return dt;
	}

	public static DataTable GetAccountsFromGlobalByTypeIds(int[] withTypeIds = null, int[] withoutTypeIds = null, string accountTitle = null, int[] withClassIds = null, int[] withoutClassIds = null, int[] withPlNoteIds = null, int[] withOutPlNoteIds = null, int[] withCustomGroupIds = null)
	{
		DataTable dt = new DataTable();
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("AccountTitle", typeof(string));
		dt.Columns.Add("AccountCode", typeof(string));
		dt.Columns.Add("ParentAccountTitle", typeof(string));
		dt.Columns.Add("AccountClass", typeof(string));
		List<getGlobalAllAccountsWithCustomGroup> allAccounts = clsGlobalVariables.AllAccountsWithCustomGroupId;
		if (allAccounts == null)
		{
			return dt;
		}
		IEnumerable<getGlobalAllAccountsWithCustomGroup> filteredAccounts = allAccounts.AsEnumerable();
		int[] array = withTypeIds;
		if (array != null && array.Length != 0)
		{
			filteredAccounts = filteredAccounts.Where((getGlobalAllAccountsWithCustomGroup a) => withTypeIds.Contains(a.AccountTypeId));
		}
		int[] array2 = withoutTypeIds;
		if (array2 != null && array2.Length != 0)
		{
			filteredAccounts = filteredAccounts.Where((getGlobalAllAccountsWithCustomGroup a) => !withoutTypeIds.Contains(a.AccountTypeId));
		}
		int[] array3 = withClassIds;
		if (array3 != null && array3.Length != 0)
		{
			filteredAccounts = filteredAccounts.Where((getGlobalAllAccountsWithCustomGroup a) => withClassIds.Contains(a.AccountClass));
		}
		int[] array4 = withoutClassIds;
		if (array4 != null && array4.Length != 0)
		{
			filteredAccounts = filteredAccounts.Where((getGlobalAllAccountsWithCustomGroup a) => !withoutClassIds.Contains(a.AccountClass));
		}
		int[] array5 = withPlNoteIds;
		if (array5 != null && array5.Length != 0)
		{
			filteredAccounts = filteredAccounts.Where((getGlobalAllAccountsWithCustomGroup a) => withPlNoteIds.Contains(a.PLNoteId));
		}
		int[] array6 = withOutPlNoteIds;
		if (array6 != null && array6.Length != 0)
		{
			filteredAccounts = filteredAccounts.Where((getGlobalAllAccountsWithCustomGroup a) => !withOutPlNoteIds.Contains(a.PLNoteId));
		}
		int[] array7 = withCustomGroupIds;
		if (array7 != null && array7.Length != 0)
		{
			filteredAccounts = filteredAccounts.Where((getGlobalAllAccountsWithCustomGroup a) => withCustomGroupIds.Contains(a.CustomGroupId));
		}
		if (!string.IsNullOrWhiteSpace(accountTitle))
		{
			filteredAccounts = filteredAccounts.Where((getGlobalAllAccountsWithCustomGroup a) => a.AccountTitle.Equals(accountTitle, StringComparison.OrdinalIgnoreCase));
		}
		HashSet<int> seenIds = new HashSet<int>();
		foreach (getGlobalAllAccountsWithCustomGroup account in filteredAccounts)
		{
			int id = Conversion.ToInt((object)account.ChartOfAccountId);
			if (seenIds.Add(id))
			{
				dt.Rows.Add(id, account.AccountTitle, account.AccountCode, account.ParentAccountTitle, account.AccountClassName);
			}
		}
		return dt;
	}

	public static double GetColumnSum(DataTable table, string columnName)
	{
		double sum = 0.0;
		if (table == null)
		{
			return sum;
		}
		if (!table.Columns.Contains(columnName))
		{
			return sum;
		}
		foreach (DataRow row in table.Rows)
		{
			if (row[columnName] != DBNull.Value && double.TryParse(row[columnName].ToString(), out var value))
			{
				sum += value;
			}
		}
		return sum;
	}

	public static DataTable PopulateDataTableAndReturn<T>(List<T> list, params Func<T, object>[] selectors)
	{
		DataTable dt = new DataTable();
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("Description", typeof(string));
		if (list != null && list.Count > 0)
		{
			foreach (T item in list)
			{
				dt.Rows.Add(selectors.Select((Func<T, object> sel) => sel(item)).ToArray());
			}
		}
		return dt;
	}

	public static void PopulateDataTable<T>(DataTable table, List<T> list, params Func<T, object>[] selectors)
	{
		table.Clear();
		if (list == null || list.Count <= 0)
		{
			return;
		}
		foreach (T item in list)
		{
			table.Rows.Add(selectors.Select((Func<T, object> sel) => sel(item)).ToArray());
		}
	}

	public static bool DoesValueExistInColumn(DataTable dt, string columnName, object value)
	{
		if (dt == null || dt.Rows.Count == 0)
		{
			return false;
		}
		if (string.IsNullOrWhiteSpace(columnName))
		{
			return false;
		}
		if (!dt.Columns.Contains(columnName))
		{
			throw new ArgumentException("Column '" + columnName + "' not found in DataTable.");
		}
		return dt.AsEnumerable().Any((DataRow row) => row[columnName] != DBNull.Value && row[columnName].Equals(value));
	}

	public static DataTable GetSpecialRightsByScreenIdDbCall(int ScreenId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		try
		{
			return global::Architecture.BLL.SreenRightsUserWise.SpecialRightsByScreenIdAndUserId(new global::Architecture.Model.SreenRightsUserWise
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				ScreenId = ScreenId,
				UserId = clsGlobalVariables.UserAccount.ID
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GlobalSupplierCustomerListsFillDbCall(int organizationId, int companyId, int partyTypeId = 0)
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		try
		{
			clsGlobalVariables.globalAllSupplierCustomer = GlobalServicesMethods.getGlobalSupplierCustomer(organizationId, companyId, 0, 0, 0, "");
			int showBothVendorAndCustomerOnSalesPurchase = Conversion.ToInt((object)0);
			HashSet<int> excludedCustomerGroups = new HashSet<int> { 7, 8, 9, 10, 13 };
			HashSet<int> mustExcludeCustomerGroups = new HashSet<int> { 7, 9, 10 };
			List<getGlobalAllSupplierCustomer> list = clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer sp) => !mustExcludeCustomerGroups.Contains(sp.CustomerGroupId)).Where(delegate(getGlobalAllSupplierCustomer sp)
			{
				bool flag = excludedCustomerGroups.Contains(sp.CustomerGroupId);
				if (showBothVendorAndCustomerOnSalesPurchase == 1)
				{
					return !flag;
				}
				if (partyTypeId == 0)
				{
					return true;
				}
				if (partyTypeId == 1)
				{
					return sp.PartyTypeId == 1;
				}
				return partyTypeId == 2 && sp.PartyTypeId == 2 && !flag;
			}).ToList();
			List<getGlobalSupplierCustomer> lst = new List<getGlobalSupplierCustomer>();
			foreach (getGlobalAllSupplierCustomer dr in list)
			{
				getGlobalSupplierCustomer obj = new getGlobalSupplierCustomer
				{
					Id = Conversion.ToInt((object)dr.Id),
					CompanyName = Conversion.ToString((object)dr.CompanyName),
					PartyCode = Conversion.ToString((object)dr.PartyCode),
					GlAccountId = Conversion.ToInt((object)dr.GlAccountId),
					CityName = Conversion.ToString((object)dr.CityName),
					MobilePersonal = Conversion.ToString((object)dr.MobilePersonal),
					CityId = Conversion.ToInt((object)dr.CityId),
					PartyTypeId = Conversion.ToInt((object)dr.PartyTypeId),
					CustomerGroupId = Conversion.ToInt((object)dr.CustomerGroupId),
					NickName = dr.NickName,
					WhatsAppNo = dr.WhatsAppNo,
					CompanyIndividuals = dr.CompanyIndividuals
				};
				lst.Add(obj);
			}
			clsGlobalVariables.globalSupplierCustomer = lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GlobalServicesDbCall(UserAccount userAccount, IEnumerable<string> servicesToInitialize, CancellationToken token = default(CancellationToken))
	{
		if (userAccount == null || servicesToInitialize == null)
		{
			return;
		}
		int orgId = userAccount.OrganizationId;
		int compId = userAccount.CompanyId;
		int branchId = userAccount.BranchesId;
		Dictionary<string, Action> serviceMap = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
		{
			["BranchesAllocatedToUser"] = delegate
			{
				clsGlobalVariables.globalBranchesAllocateToUser = GlobalServicesMethods.GetGlobalBranchsAllocatedToUser(orgId, compId, userAccount.ID);
			},
			["AccountsWithCustomGroupId"] = delegate
			{
				clsGlobalVariables.AllAccountsWithCustomGroupId = GlobalServicesMethods.GetGlobalAllAccountsWithCustomGroup(orgId, compId, 0, 0, "");
			},
			["ItemsWithCustomGroupId"] = delegate
			{
				clsGlobalVariables.AllItemsWithCustomGroupId = GlobalServicesMethods.GetGlobalAllItemsWithCustomGroup(orgId, compId, branchId);
			},
			["LookUpsEngr"] = delegate
			{
				clsGlobalVariables.GetAllLookUps_Engr = LookUps.FormHistoryWithList();
			},
			["PaymentTerm"] = delegate
			{
				clsGlobalVariables.globalPaymentTerm = GlobalServicesMethods.getPaymentTermlist(orgId, compId);
			},
			["UomSchedule"] = delegate
			{
				clsGlobalVariables.globalUomSchedule = GlobalServicesMethods.getAllUomsByCompanyId(orgId, compId, 0, 1);
			},
			["SupplierCustomerLists"] = delegate
			{
				GlobalSupplierCustomerListsFillDbCall(orgId, compId);
			},
			["VendorsAndCustomersForTransporters"] = delegate
			{
				clsGlobalVariables.getGlobalVendorsAndCustomersForTransporters = GlobalServicesMethods.getGlobalVendorsAndCustomersForTransporters(orgId, compId, 0, 0, "");
			},
			["WarehousesWithBranches"] = delegate
			{
				clsGlobalVariables.globalWarehousesWithBranches = GlobalServicesMethods.getGlobalActiveWarehouse(orgId, compId, branchId);
			},
			["AllItems"] = delegate
			{
				clsGlobalVariables.getGlobalAllItems = GlobalServicesMethods.AllItemsWithModal(orgId, compId, 0, 0, "");
			},
			["AllSerivesItems"] = delegate
			{
				clsGlobalVariables.getGlobalAllSerivesItems = GlobalServicesMethods.Item_AllServiesItems(orgId, compId);
			},
			["MultiCurrency"] = delegate
			{
				clsGlobalVariables.globalMultiCurrency = GlobalServicesMethods.GetAllMultiCurrency(orgId, compId);
			},
			["AllCities"] = delegate
			{
				clsGlobalVariables.globalAllCities = GlobalServicesMethods.getGlobalAllCity(orgId, compId);
			},
			["CropYear"] = delegate
			{
				clsGlobalVariables.globalCropYear = GlobalServicesMethods.getGlobalAllCropYear(orgId, compId);
			},
			["JobLot"] = delegate
			{
				// clsGlobalVariables.globalJobLot = CommonServices.GetJobLotGlIdsandName(orgId, compId);
			},
			["InvPackingType"] = delegate
			{
				clsGlobalVariables.globalInvPackingType = GlobalServicesMethods.getGlobalAllPackingType(orgId, compId);
			},
			["AllBrands"] = delegate
			{
				clsGlobalVariables.getGlobalAllBrands = GlobalServicesMethods.AllBrands(orgId, compId);
			},
			["DriverBioInfo"] = delegate
			{
				clsGlobalVariables.globalAllDriverBioInfo = global::Architecture.BLL.Main.driverBiodata.ReadAll(orgId, compId);
			},
			["ItemConditions"] = delegate
			{
				clsGlobalVariables.globalItemConditions = GlobalServicesMethods.GetItemCondtions();
			},
			["RacksWithWarehouseAndItem"] = delegate
			{
				clsGlobalVariables.racksWithWarehouseAndItems = GlobalServicesMethods.GetRacksWithWarehouseByItemId(orgId, compId, branchId);
			},
			["WarehouseRacks"] = delegate
			{
				clsGlobalVariables.globalWarehouseRacks = GlobalServicesMethods.GetWarehouseRacks(orgId, compId);
			},
			["DeliveryTerm"] = delegate
			{
				clsGlobalVariables.globalDeliveryTermList = GlobalServicesMethods.DeliveryTermList();
			}
		};
		foreach (string key in servicesToInitialize)
		{
			if (token.IsCancellationRequested)
			{
				break;
			}
			if (serviceMap.TryGetValue(key, out var initializer))
			{
				try
				{
					initializer();
				}
				catch (Exception)
				{
				}
			}
		}
	}

	public static void ReplaceCompLogoColumn(DataTable dtToChange, DataTable dtData)
	{
		try
		{
			if (dtToChange == null || dtData == null || dtData.Rows.Count == 0)
			{
				return;
			}
			if (dtToChange.Columns.Contains("CompLogoImage"))
			{
				dtToChange.Columns["CompLogoImage"].ColumnName = "CompLogoImage2";
			}
			if (!dtToChange.Columns.Contains("CompLogoImage"))
			{
				dtToChange.Columns.Add("CompLogoImage", typeof(byte[]));
			}
			object logoValue = dtData.Rows[0][0];
			foreach (DataRow row in dtToChange.Rows)
			{
				row["CompLogoImage"] = ((logoValue != DBNull.Value) ? logoValue : null);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message + " Exception from adding comp logo from dataset");
		}
	}

	public static T GetColumnValueByIdFromDatatable<T>(DataTable table, string idColumnName, object idValue, string targetColumnName)
	{
		if (table == null)
		{
			throw new ArgumentNullException("table");
		}
		return (from r in table.AsEnumerable()
			where r.Field<object>(idColumnName).Equals(idValue)
			select r.Field<T>(targetColumnName)).FirstOrDefault();
	}

	public static string GetFirstColumnValue(DataTable table, string columnName, string whereColumnName, string whereColumnValue)
	{
		if (table == null || !table.Columns.Contains(columnName) || !table.Columns.Contains(whereColumnName))
		{
			return string.Empty;
		}
		return (from row in table.AsEnumerable()
			where row[whereColumnName] != DBNull.Value && row[columnName] != DBNull.Value && string.Equals(row[whereColumnName].ToString(), whereColumnValue, StringComparison.OrdinalIgnoreCase)
			select row[columnName].ToString()).FirstOrDefault() ?? string.Empty;
	}

	public static DataTable CreateTableForCombo()
	{
		return new DataTable
		{
			Columns = { "Id", "name" }
		};
	}
}

}