using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Account;
using Architecture.Model.Inventory;
using Architecture.Model.Main;

namespace Architecture.BLL.Main;

public class GlobalServicesMethods
{
	public static List<getGlobalAllItems> AllItemsWithModal(int OrganizationId, int CompanyId, int PageSize = 0, int PageNumber = 0, string Keyword = "")
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = PageSize
				});
			}
			if (PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = PageNumber
				});
			}
			if (Keyword != null && Keyword != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Keyword",
					Value = Keyword
				});
			}
			DataTable dataTableProc = GenericProvider.GetDataTableProc<getGlobalAllItems>("[dbo].[USP_Item_AllItemsWithModal]", p);
			List<getGlobalAllItems> lst = new List<getGlobalAllItems>();
			foreach (DataRow dr in dataTableProc.Rows)
			{
				getGlobalAllItems obj = new getGlobalAllItems();
				obj.Id = Conversion.ToInt(dr["Id"]);
				obj.ItemName = Conversion.ToString(dr["ItemName"]);
				obj.ItemCode = Conversion.ToString(dr["ItemCode"]);
				obj.InventoryParentCategoriesId = Conversion.ToInt(dr["InventoryParentCategoriesId"]);
				obj.ItemCategoryId = Conversion.ToInt(dr["ItemCategoryId"]);
				obj.ItemCategory = Conversion.ToString(dr["ItemCategory"]);
				obj.ItemTypeId = Conversion.ToInt(dr["ItemTypeId"]);
				obj.ItemType = Conversion.ToString(dr["ItemType"]);
				obj.ItemTypeOfTypeId = Conversion.ToInt(dr["ItemTypeOfTypeId"]);
				obj.ItemTypeOfType = Conversion.ToString(dr["ItemTypeOfType"]);
				obj.ItemProductionStageId = Conversion.ToInt(dr["ItemProductionStageId"]);
				obj.productionStageName = Conversion.ToString(dr["productionStageName"]);
				obj.FinishWeight = Conversion.ToDouble(dr["FinishWeight"]);
				obj.DyeWeight = Conversion.ToDouble(dr["DyeWeight"]);
				obj.SandWeight = Conversion.ToDouble(dr["SandWeight"]);
				obj.SampleNo = Conversion.ToString(dr["SampleNo"]);
				obj.HasModel = Conversion.ToBool(dr["HasModel"]);
				obj.itemModalId = Conversion.ToInt(dr["itemModalId"]);
				obj.ItemClassId = Conversion.ToInt(dr["ItemClassId"]);
				obj.LeadTimeDay = Conversion.ToInt(dr["LeadTimeDay"]);
				obj.WeightCapacity = Conversion.ToString(dr["WeightCapacity"]);
				obj.InvParentCateDescription = Conversion.ToString(dr["InvParentCateDescription"]);
				obj.MasterItemId = Conversion.ToInt(dr["MasterItemId"]);
				obj.MasterItem = Conversion.ToString(dr["MasterItem"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<getGlobalAllSerivesItems> Item_AllServiesItems(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			DataTable dataTableProc = GenericProvider.GetDataTableProc<getGlobalAllSerivesItems>("[lgstcm].[USP_Item_AllServiesItems]", p);
			List<getGlobalAllSerivesItems> lst = new List<getGlobalAllSerivesItems>();
			foreach (DataRow dr in dataTableProc.Rows)
			{
				getGlobalAllSerivesItems obj = new getGlobalAllSerivesItems();
				obj.Id = Conversion.ToInt(dr["Id"]);
				obj.ItemName = Conversion.ToString(dr["ItemName"]);
				obj.ItemCode = Conversion.ToString(dr["ItemCode"]);
				obj.InventoryParentCategoriesId = Conversion.ToInt(dr["InventoryParentCategoriesId"]);
				obj.InvParentCateDescription = Conversion.ToString(dr["InvParentCateDescription"]);
				obj.ItemCategoryId = Conversion.ToInt(dr["ItemCategoryId"]);
				obj.ItemCategory = Conversion.ToString(dr["ItemCategory"]);
				obj.ItemTypeId = Conversion.ToInt(dr["ItemTypeId"]);
				obj.ItemType = Conversion.ToString(dr["ItemType"]);
				obj.ItemTypeOfTypeId = Conversion.ToInt(dr["ItemTypeOfTypeId"]);
				obj.ItemTypeOfType = Conversion.ToString(dr["ItemTypeOfType"]);
				obj.ServicesMasterItemId = Conversion.ToInt(dr["ServicesMasterItemId"]);
				obj.ServicesMasterItem = Conversion.ToString(dr["ServiceMasterItem"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<getGlobalAllBrands> AllBrands(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			DataTable dataTableProc = GenericProvider.GetDataTableProc<getGlobalAllBrands>("[dbo].[usp_getBrands]", p);
			List<getGlobalAllBrands> lst = new List<getGlobalAllBrands>();
			foreach (DataRow dr in dataTableProc.Rows)
			{
				getGlobalAllBrands obj = new getGlobalAllBrands();
				obj.Id = Conversion.ToInt(dr["Id"]);
				obj.BrandName = Conversion.ToString(dr["BrandName"]);
				obj.BrandCode = Conversion.ToString(dr["BrandCode"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<GetJobLotGlIdsandName> GetJobLotGlIdsandName(int OrganizationId, int CompanyId)
	{
		try
		{
			return CommonServices.GetJobLotGlIdsandName(OrganizationId, CompanyId);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<Architecture.Model.Inventory.InvPackingType> getGlobalAllPackingType(int OrganizationId, int CompanyId)
	{
		try
		{
			List<Architecture.Model.Inventory.InvPackingType> lst = new List<Architecture.Model.Inventory.InvPackingType>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			foreach (DataRow dr in GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPackingType>("[dbo].[Sp_InvPackingType_GetAllMethod]", p).Rows)
			{
				Architecture.Model.Inventory.InvPackingType obj = new Architecture.Model.Inventory.InvPackingType();
				obj.Id = Conversion.ToInt(dr["Id"]);
				obj.PackTypeCode = Conversion.ToString(dr["PackTypeCode"]);
				obj.PackTypeDesc = Conversion.ToString(dr["PackTypeDesc"]);
				obj.MinEbWeight = Conversion.ToDouble(dr["MinEbWeight"]);
				obj.MaxEbWeight = Conversion.ToDouble(dr["MaxEbWeight"]);
				obj.MinEbStockWeight = Conversion.ToDouble(dr["MinEbStockWeight"]);
				obj.MaxEbStockWeight = Conversion.ToDouble(dr["MaxEbStockWeight"]);
				obj.IsActive = true;
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<GlobalWarehousesWithBranch> getGlobalActiveWarehouse(int OrganizationId, int CompanyId, int BranchesId)
	{
		try
		{
			List<GlobalWarehousesWithBranch> lst = new List<GlobalWarehousesWithBranch>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchId",
				Value = BranchesId
			});
			DataTable dt = GenericProvider.GetDataTableProc<GlobalWarehousesWithBranch>("USP_GetWarehousesAllocatedToBranch", p);
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					GlobalWarehousesWithBranch obj = new GlobalWarehousesWithBranch();
					obj.WarehouseId = Conversion.ToInt(dr["Id"]);
					obj.BranchId = Conversion.ToInt(dr["BranchId"]);
					obj.BranchName = Conversion.ToString(dr["BranchName"]);
					obj.WarehouseName = Conversion.ToString(dr["WarehouseName"]);
					obj.WareHouseType = Conversion.ToString(dr["WareHouseType"]);
					obj.WareHouseTypeId = Conversion.ToInt(dr["WareHouseTypeId"]);
					obj.IsActive = Conversion.ToBool(dr["IsActive"]);
					obj.PlantId = Conversion.ToInt(dr["PlantId"]);
					obj.PlantName = Conversion.ToString(dr["PlantName"]);
					lst.Add(obj);
				}
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<GlobalCropYear> getGlobalAllCropYear(int OrganizationId, int CompanyId)
	{
		try
		{
			List<GlobalCropYear> lst = new List<GlobalCropYear>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			foreach (DataRow dr in GenericProvider.GetDataTableProc<GlobalCropYear>("[dbo].[Sp_InvCropYear_GetAllMethod]", p).Rows)
			{
				GlobalCropYear obj = new GlobalCropYear();
				obj.Id = Convert.ToInt32(dr["Id"]);
				obj.CropYear = Convert.ToString(dr["CropYear"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<GlobalAllCity> getGlobalAllCity(int OrganizationId, int CompanyId)
	{
		try
		{
			List<GlobalAllCity> lst = new List<GlobalAllCity>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			foreach (DataRow dr in GenericProvider.GetDataTableProc<GlobalAllCity>("[dbo].[USP_City_GetAllWithCountryAndTehsil]", p).Rows)
			{
				GlobalAllCity obj = new GlobalAllCity();
				obj.Id = Conversion.ToInt(dr["Id"]);
				obj.CityCode = Conversion.ToString(dr["CityCode"]);
				obj.CityName = Conversion.ToString(dr["CityName"]);
				obj.CityNameOtherLingo = Conversion.ToString(dr["CityNameOtherLingo"]);
				obj.CountryId = Conversion.ToInt(dr["CountryId"]);
				obj.CountryName = Conversion.ToString(dr["CountryName"]);
				obj.TehsilId = Conversion.ToInt(dr["TehsilId"]);
				obj.Tehsil = Conversion.ToString(dr["Tehsil"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<GloablUomSchedule> getAllUomsByCompanyId(int OrganizationId, int CompanyId, int ItemId = 0, int Active = 0)
	{
		try
		{
			List<GloablUomSchedule> lst = new List<GloablUomSchedule>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = ItemId
				});
			}
			if (Active != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Active",
					Value = Active
				});
			}
			foreach (DataRow dr in GenericProvider.GetDataTableProc<GloablUomSchedule>("usp_getAllUomsByCompanyId", p).Rows)
			{
				GloablUomSchedule obj = new GloablUomSchedule();
				obj.Id = Conversion.ToInt(dr["Id"]);
				obj.UOMCode = Conversion.ToString(dr["UOMCode"]);
				obj.UOMDescription = Conversion.ToString(dr["UOMDescription"]);
				obj.Equivalent = Conversion.ToDouble(dr["Equivalent"]);
				obj.QtyEquivalent = Conversion.ToDouble(dr["QtyEquivalent"]);
				obj.BaseRateUom = Conversion.ToBool(dr["BaseRateUom"]);
				obj.BasePackUom = Conversion.ToBool(dr["BasePackUom"]);
				obj.BaseSecondaryUom = Conversion.ToBool(dr["BaseSecondaryUom"]);
				obj.ItemId = Conversion.ToInt(dr["ItemId"]);
				obj.ItemName = Conversion.ToString(dr["ItemName"]);
				obj.ItemCode = Conversion.ToString(dr["ItemCode"]);
				obj.Active = Conversion.ToBool(dr["Active"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<getGlobalVendorsAndCustomersForTransporter> getGlobalVendorsAndCustomersForTransporters(int OrganizationId, int CompanyId, int PageSize = 0, int PageNumber = 0, string Keyword = "")
	{
		try
		{
			List<getGlobalVendorsAndCustomersForTransporter> lst = new List<getGlobalVendorsAndCustomersForTransporter>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = PageSize
				});
			}
			if (PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = PageNumber
				});
			}
			if (Keyword != null && Keyword != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Keyword",
					Value = Keyword
				});
			}
			foreach (DataRow dr in GenericProvider.GetDataTableProc<getGlobalVendorsAndCustomersForTransporter>("[dbo].[USP_GetVendorsAndCustomersForTransporter]", p).Rows)
			{
				getGlobalVendorsAndCustomersForTransporter obj = new getGlobalVendorsAndCustomersForTransporter();
				obj.Id = Conversion.ToInt(dr["Id"]);
				obj.CompanyName = Conversion.ToString(dr["CompanyName"]);
				obj.PartyCode = Conversion.ToString(dr["PartyCode"]);
				obj.GlAccountId = Conversion.ToInt(dr["GlAccountId"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<getGlobalAllSupplierCustomer> getGlobalSupplierCustomer(int OrganizationId, int CompanyId, int PartyTypeId = 0, int PageSize = 0, int PageNumber = 0, string Keyword = "")
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (PartyTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyTypeId",
					Value = PartyTypeId
				});
			}
			if (PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = PageSize
				});
			}
			if (PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = PageNumber
				});
			}
			if (Keyword != null && Keyword != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Keyword",
					Value = Keyword
				});
			}
			DataTable dataTableProc = GenericProvider.GetDataTableProc<getGlobalAllSupplierCustomer>("USP_GetVendorsAndCustomersWithCityName", p);
			List<getGlobalAllSupplierCustomer> lst = new List<getGlobalAllSupplierCustomer>();
			foreach (DataRow dr in dataTableProc.Rows)
			{
				getGlobalAllSupplierCustomer obj = new getGlobalAllSupplierCustomer();
				obj.Id = Conversion.ToInt(dr["Id"]);
				obj.CompanyName = Conversion.ToString(dr["CompanyName"]);
				obj.PartyCode = Conversion.ToString(dr["PartyCode"]);
				obj.GlAccountId = Conversion.ToInt(dr["GlAccountId"]);
				obj.CityName = Conversion.ToString(dr["CityName"]);
				obj.MobilePersonal = Conversion.ToString(dr["MobilePersonal"]);
				obj.CityId = Conversion.ToInt(dr["CityId"]);
				obj.PartyTypeId = Conversion.ToInt(dr["PartyTypeId"]);
				obj.CustomerGroupId = Conversion.ToInt(dr["CustomerGroupId"]);
				obj.ParentsSupCustId = Conversion.ToInt(dr["ParentsSupCustId"]);
				obj.IsSubSupCust = Conversion.ToBool(dr["IsSubSupCust"]);
				obj.GlAccountCurrencyId = Conversion.ToInt(dr["GlAccountCurrencyId"]);
				obj.GlAccountCurrencyCode = Conversion.ToString(dr["GlAccountCurrencyCode"]);
				obj.NickName = Conversion.ToString(dr["NickName"]);
				obj.WhatsAppNo = Conversion.ToString(dr["WhatsAppNo"]);
				obj.CompanyIndividuals = Conversion.ToString(dr["CompanyIndividuals"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<getGlobalBranchesAllocateToUser> GetGlobalBranchsAllocatedToUser(int OrganizationId, int CompanyId, int UserId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = UserId
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.BranchesAllocationToUser>("[dbo].[USP_GetBranchsAllocatedToUser]", p);
			List<getGlobalBranchesAllocateToUser> lst = new List<getGlobalBranchesAllocateToUser>();
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					getGlobalBranchesAllocateToUser obj = new getGlobalBranchesAllocateToUser();
					obj.BranchId = Conversion.ToInt(dr["BranchId"]);
					obj.BranchName = Conversion.ToString(dr["BranchName"]);
					lst.Add(obj);
				}
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<getGlobalAllItemsWithCustomGroup> GetGlobalAllItemsWithCustomGroup(int OrganizationId, int CompanyId, int BranchesId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = BranchesId
			});
			DataTable dataTableProc = GenericProvider.GetDataTableProc<getGlobalAllItemsWithCustomGroup>("[dbo].[USP_GetAllItemFromCustomGroup]", p);
			List<getGlobalAllItemsWithCustomGroup> lst = new List<getGlobalAllItemsWithCustomGroup>();
			foreach (DataRow dr in dataTableProc.Rows)
			{
				getGlobalAllItemsWithCustomGroup obj = new getGlobalAllItemsWithCustomGroup();
				obj.ItemId = Conversion.ToInt(dr["ItemId"]);
				obj.ItemName = Conversion.ToString(dr["ItemName"]);
				obj.ItemCode = Conversion.ToString(dr["ItemCode"]);
				obj.GroupName = Conversion.ToString(dr["GroupName"]);
				obj.GroupCode = Conversion.ToString(dr["GroupCode"]);
				obj.CustomGroupId = Conversion.ToInt(dr["CustomGroupId"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<getGlobalAllAccountsWithCustomGroup> GetGlobalAllAccountsWithCustomGroup(int OrganizationId, int CompanyId, int PageSize = 0, int PageNumber = 0, string Keyword = "")
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = PageSize
				});
			}
			if (PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = PageNumber
				});
			}
			if (Keyword != null && Keyword != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Keyword",
					Value = Keyword
				});
			}
			DataTable dataTableProc = GenericProvider.GetDataTableProc<getGlobalAllAccountsWithCustomGroup>("[dbo].[USP_GETAllAccountsFromCustomGroups]", p);
			List<getGlobalAllAccountsWithCustomGroup> lst = new List<getGlobalAllAccountsWithCustomGroup>();
			foreach (DataRow dr in dataTableProc.Rows)
			{
				getGlobalAllAccountsWithCustomGroup obj = new getGlobalAllAccountsWithCustomGroup();
				obj.ChartOfAccountId = Conversion.ToInt(dr["ChartOfAccountId"]);
				obj.AccountCode = Conversion.ToString(dr["AccountCode"]);
				obj.AccountTitle = Conversion.ToString(dr["AccountTitle"]);
				obj.CustomGroupId = Conversion.ToInt(dr["CustomGroupId"]);
				obj.CustomGroup = Conversion.ToString(dr["CustomGroup"]);
				obj.AccountTypeId = Conversion.ToInt(dr["AccountTypeId"]);
				obj.ExistsInCustomGroup = Conversion.ToBool(dr["ExistsInCustomGroup"]);
				obj.PLNoteId = Conversion.ToInt(dr["PLNoteId"]);
				obj.BSNoteId = Conversion.ToInt(dr["BSNoteId"]);
				obj.AccountClass = Conversion.ToInt(dr["AccountClass"]);
				obj.ParentAccountId = Conversion.ToInt(dr["ParentCodeId"]);
				obj.ParentAccountTitle = Conversion.ToString(dr["ParentAccountTitle"]);
				obj.ParentAccountCode = Conversion.ToString(dr["ParentAccountCode"]);
				obj.AccountType = Conversion.ToString(dr["AccountType"]);
				obj.AccountClassName = Conversion.ToString(dr["AccountClassName"]);
				obj.CurrencyId = Conversion.ToInt(dr["CurrencyId"]);
				obj.CurrencyCode = Conversion.ToString(dr["CurrencyCode"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.MultiCurrency> GetAllMultiCurrency(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			DataTable dataTableProc = GenericProvider.GetDataTableProc<Architecture.Model.MultiCurrency>("Sp_MultiCurrency_GetAllMethod", p);
			List<Architecture.Model.MultiCurrency> lst = new List<Architecture.Model.MultiCurrency>();
			foreach (DataRow dr in dataTableProc.Rows)
			{
				Architecture.Model.MultiCurrency obj = new Architecture.Model.MultiCurrency();
				obj.Id = Conversion.ToInt(dr["Id"]);
				obj.CurrencyCode = Conversion.ToString(dr["CurrencyCode"]);
				obj.CurrencyName = Conversion.ToString(dr["CurrencyName"]);
				obj.CurrencySymbol = Conversion.ToString(dr["CurrencySymbol"]);
				obj.CurrencyRate = Conversion.ToDouble(dr["CurrencyRate"]);
				obj.CurrencyStatus = Conversion.ToBool(dr["CurrencyStatus"]);
				obj.PostState = Conversion.ToBool(dr["PostState"]);
				obj.EntryDate = Conversion.ToDateTime(dr["EntryDate"]);
				obj.ModifyDate = Conversion.ToDateTime(dr["ModifyDate"]);
				obj.PostDate = Conversion.ToDateTime(dr["PostDate"]);
				obj.CompanyId = Conversion.ToInt(dr["CompanyId"]);
				obj.EntryUser = Conversion.ToInt(dr["EntryUser"]);
				obj.ModifyUser = Conversion.ToInt(dr["ModifyUser"]);
				obj.OrganizationId = Conversion.ToInt(dr["OrganizationId"]);
				obj.PostUser = Conversion.ToInt(dr["PostUser"]);
				obj.Url = Conversion.ToString(dr["Url"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<RacksWithWarehouseAndItem> GetRacksWithWarehouseByItemId(int OrganizationId, int CompanyId, int BranchId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchId",
				Value = BranchId
			});
			DataTable dataTableProc = GenericProvider.GetDataTableProc<RacksWithWarehouseAndItem>("usp_getRackswithWarehouseByItemId", p);
			List<RacksWithWarehouseAndItem> lst = new List<RacksWithWarehouseAndItem>();
			foreach (DataRow dr in dataTableProc.Rows)
			{
				RacksWithWarehouseAndItem obj = new RacksWithWarehouseAndItem();
				obj.Id = Conversion.ToInt(dr["Id"]);
				obj.RackName = Conversion.ToString(dr["RackName"]);
				obj.WarehouseId = Conversion.ToInt(dr["invWarehouseId"]);
				obj.WareHouseName = Conversion.ToString(dr["WareHouseName"]);
				obj.ItemId = Conversion.ToInt(dr["ItemId"]);
				obj.ItemName = Conversion.ToString(dr["ItemName"]);
				obj.ItemCode = Conversion.ToString(dr["ItemCode"]);
				obj.BranchId = Conversion.ToInt(dr["BranchId"]);
				obj.BranchName = Conversion.ToString(dr["BranchName"]);
				obj.BaseRackId = Conversion.ToInt(dr["BaseRackId"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<GlobalItemCondition> GetItemCondtions()
	{
		try
		{
			DataTable dataTable = PackingMaterialItemsAllocateToTransactionFlow.ItemCondition();
			List<GlobalItemCondition> lst = new List<GlobalItemCondition>();
			foreach (DataRow dr in dataTable.Rows)
			{
				GlobalItemCondition obj = new GlobalItemCondition();
				obj.Id = Conversion.ToInt(dr["Id"]);
				obj.ConditionStatus = Conversion.ToString(dr["ConditionStatus"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<GlobalWarehouseRack> GetWarehouseRacks(int Organization, int CompanyId)
	{
		try
		{
			DataTable rackswithWarehouse = Architecture.BLL.Inventory.invWarehouseRack.GetRackswithWarehouse(Organization, CompanyId);
			List<GlobalWarehouseRack> lst = new List<GlobalWarehouseRack>();
			foreach (DataRow dr in rackswithWarehouse.Rows)
			{
				GlobalWarehouseRack obj = new GlobalWarehouseRack();
				obj.Id = Conversion.ToInt(dr["Id"]);
				obj.rackName = Conversion.ToString(dr["rackName"]);
				obj.invWarehouseId = Conversion.ToInt(dr["invWarehouseId"]);
				obj.WareHouseName = Conversion.ToString(dr["WareHouseName"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Inventory.InvDueTerms> getPaymentTermlist(int OrganizationId, int CompanyId)
	{
		try
		{
			new List<Architecture.Model.Inventory.InvDueTerms>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAll"
			});
			return new Architecture.DAL.Inventory.InvDueTerms().GetAll("Sp_InvDueTerms_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<DeliveryTerms> DeliveryTermList()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			DataTable dataTableProc = GenericProvider.GetDataTableProc<lookUp>("[dbo].[USP_DeliveryTerm_GetAllMethod]", p);
			List<DeliveryTerms> lst = new List<DeliveryTerms>();
			foreach (DataRow dr in dataTableProc.Rows)
			{
				DeliveryTerms obj = new DeliveryTerms();
				obj.Id = Conversion.ToInt(dr["Id"]);
				obj.Description = Conversion.ToString(dr["Description"]);
				obj.ValueDescription = Conversion.ToString(dr["ValueDescription"]);
				lst.Add(obj);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
