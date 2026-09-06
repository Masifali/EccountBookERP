using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.TaxProject.Inventory;

namespace Architecture.BLL.TaxProject.Common;

public class CommonServies
{
	public static string GetConfigurationFromAllocation(int OrganizationId, int CompanyId, string ConfigDescription)
	{
		try
		{
			string ConfigKey = "";
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
				ParameterName = "@ConfigDescription",
				Value = ConfigDescription
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetConfigurationByOrgCompandConfigDescription"
			});
			DataTable dt = GenericProvider.GetDataTableProcForServer<ConfigrationsAllocation>("Sp_ConfigrationsAllocation_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				ConfigKey = Conversion.ToString(dt.Rows[0]["ConfigKey"]);
			}
			return ConfigKey;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool GetERPFeatureById(int OrganizationId, int CompanyId, int FeatureId)
	{
		try
		{
			List<ErpFeaturesByCompany> lst = new List<ErpFeaturesByCompany>();
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
			DataTable dt = GenericProvider.GetDataTableProcForServer<ErpFeaturesByCompany>("USP_GetERPFeaturesByCompanyId", p);
			if (dt.Rows.Count > 0)
			{
				lst = Conversion.ConvertDataTableToList<ErpFeaturesByCompany>(dt);
			}
			if (lst.Where((ErpFeaturesByCompany x) => x.Id == FeatureId).ToList().Count > 0)
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.TaxProject.Inventory.SupplierCustomerGlAccountId> GetSupplierCustomerListForFinancialEffects(int OrganizationId, int CompanyId)
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
				Value = "GetGlAccountIdandCompanyNameBySupplierCustomerId"
			});
			return Conversion.ConvertDataTableToList<Architecture.Model.TaxProject.Inventory.SupplierCustomerGlAccountId>(GenericProvider.GetDataTableProcForServer<Architecture.Model.Company>("Sp_SupplierCustomer_GetAllMethod", p));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.TaxProject.Inventory.GetItemGlIdsandItemName> GetItemListForFinancialEffects(int OrganizationId, int CompanyId)
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
				Value = "GetItemGlIdsandItemName"
			});
			return Conversion.ConvertDataTableToList<Architecture.Model.TaxProject.Inventory.GetItemGlIdsandItemName>(GenericProvider.GetDataTableProcForServer<Architecture.Model.Company>("Sp_Item_GetAllMethod", p));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.TaxProject.Inventory.JobLot JobLotGetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetById"
			});
			return Conversion.ConvertDataTableToList<Architecture.Model.TaxProject.Inventory.JobLot>(GenericProvider.GetDataTableProcForServer<Architecture.Model.Company>("SP_JobLot_ReadMethod", p))[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.TaxProject.Inventory.GetItemGlIdsandItemName> GetPartyProcessingItemListForFinancialEffects(int OrganizationId, int CompanyId)
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
				Value = "GetPartyProcessingItemGlIdsandItemName"
			});
			return Conversion.ConvertDataTableToList<Architecture.Model.TaxProject.Inventory.GetItemGlIdsandItemName>(GenericProvider.GetDataTableProcForServer<Architecture.Model.Company>("Sp_Item_GetAllMethod", p));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.TaxProject.Inventory.InventoryItemsOther> GetOtherItemForFinancialEffects(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> x = new List<SqlParameter>();
			x.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			x.Add(new SqlParameter
			{
				ParameterName = "@organizationId",
				Value = OrganizationId
			});
			x.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			return Conversion.ConvertDataTableToList<Architecture.Model.TaxProject.Inventory.InventoryItemsOther>(GenericProvider.GetDataTableProcForServer<Architecture.Model.Company>("Sp_InventoryItemsOther_GetAllMethod", x));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAvgRateFromAvaiblableLoaderStock(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> x = new List<SqlParameter>();
			x.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			x.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			x.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.RefDocumentTypeId
			});
			x.Add(new SqlParameter
			{
				ParameterName = "@RefDocIdNo",
				Value = obj.RefRefDocIdNo
			});
			x.Add(new SqlParameter
			{
				ParameterName = "@RefDocSubIdNo",
				Value = obj.RefDocSubId
			});
			x.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAvgRateFromAvaiblableLoaderStock"
			});
			return GenericProvider.GetDataTableProcForServer<Architecture.Model.TaxProject.Inventory.InvSaleInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", x);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetActiveFinancialYear(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProcForServer<ConfigrationsAllocation>("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetEqvilentByItemId(ReportsParameters obj)
	{
		try
		{
			double PackUom = 0.0;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetEqvilentByItemId"
			});
			DataTable dt = GenericProvider.GetDataTableProcForServer<Architecture.Model.Inventory.Item>("Sp_Item_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				PackUom = Conversion.ToDouble(dt.Rows[0][0]);
			}
			return PackUom;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetEqvilentByItemIdAndUomScheduleId(ReportsParameters obj)
	{
		try
		{
			double PackUom = 0.0;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ScheduleId",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetEqvilentByItemIdAndUomScheduleId"
			});
			DataTable dt = GenericProvider.GetDataTableProcForServer<Architecture.Model.Inventory.Item>("Sp_Item_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				PackUom = Conversion.ToDouble(dt.Rows[0]["Equivalent"]);
			}
			return PackUom;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAvgRateAndStockInHand(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			return GenericProvider.GetDataTableProcForServer<ConfigrationsAllocation>("USP_GetAvgRateAndStockInHand", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double AvgRateOnlyForCGS(ReportsParameters obj)
	{
		try
		{
			double AvgRate = 0.0;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.Id
				});
			}
			if (obj.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
				});
			}
			if (obj.CropYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYearId",
					Value = obj.CropYearId
				});
			}
			if (!string.IsNullOrEmpty(obj.CropYear))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
				});
			}
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetOnlyAvgRateForCGS"
			});
			DataTable dt = GenericProvider.GetDataTableProcForServer<ReportsParameters>("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				AvgRate = Conversion.ToDouble(dt.Rows[0]["AvgRate"]);
			}
			return AvgRate;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<GetAccountsByEmployeeId> GetGlAccountsByEmployeeId(int OrganizationId, int CompanyId)
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
				Value = "GetGlAccountsByEmployeeId"
			});
			return new Architecture.DAL.Inventory.GetItemGlIdsandItemName().getdataForEmployee("Sp_genEmployeeHistory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetActivityGlIdAndName(int WagesScheduleId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = WagesScheduleId
			});
			return GenericProvider.GetDataTableProcForServer<ConfigrationsAllocation>("USP_GetActivityGlIdAndName", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetActivityGlIdAndNameForExtra(int WagesScheduleId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = WagesScheduleId
			});
			return GenericProvider.GetDataTableProcForServer<ConfigrationsAllocation>("[dbo].[USP_GetActivityGlIdAndNameForExtra]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetParentCategoriesForPurchaseAndSaleReports(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProcForServer<ConfigrationsAllocation>("USP_GetParentCategoriesForPurchaseAndSaleReports", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double getAvgRateByJobOrder(ReportsParameters obj)
	{
		try
		{
			double AvgRate = 0.0;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@JobLotId",
				Value = obj.JobLotId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			if (obj.RecId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.RecId
				});
			}
			DataTable dt = GenericProvider.GetDataTableProcForServer<ConfigrationsAllocation>("usp_getAvgRateByJobOrder", p);
			if (dt != null && dt.Rows.Count > 0)
			{
				AvgRate = Conversion.ToDouble(dt.Rows[0]["AvgRate"]);
			}
			return AvgRate;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
