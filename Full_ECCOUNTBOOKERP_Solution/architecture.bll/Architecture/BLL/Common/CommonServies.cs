using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Common;

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
			DataTable dt = GenericProvider.GetDataTableProc<ConfigrationsAllocation>("Sp_ConfigrationsAllocation_GetAllMethod", p);
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
			DataTable dt = GenericProvider.GetDataTableProc<ErpFeaturesByCompany>("USP_GetERPFeaturesByCompanyId", p);
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

	public static List<Architecture.Model.Inventory.SupplierCustomerGlAccountId> GetSupplierCustomerListForFinancialEffects(int OrganizationId, int CompanyId)
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
			return new Architecture.DAL.Inventory.SupplierCustomerGlAccountId().GetData("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Inventory.GetItemGlIdsandItemName> GetItemListForFinancialEffects(int OrganizationId, int CompanyId)
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
			return new Architecture.DAL.Inventory.GetItemGlIdsandItemName().GetData("Sp_Item_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Inventory.GetItemGlIdsandItemName> GetPartyProcessingItemListForFinancialEffects(int OrganizationId, int CompanyId)
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
			return new Architecture.DAL.Inventory.GetItemGlIdsandItemName().GetData("Sp_Item_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Inventory.InventoryItemsOther> GetOtherItemForFinancialEffects(int OrganizationId, int CompanyId)
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
			return new Architecture.DAL.Inventory.InventoryItemsOther().GetAll("Sp_InventoryItemsOther_GetAllMethod", x);
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", x);
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
			return GenericProvider.GetDataTableProc<ConfigrationsAllocation>("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetWeightAvgRateByItem(ReportsParameters obj)
	{
		try
		{
			return Architecture.BLL.Inventory.Item.GetWeightAvgRateByItem(obj);
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
			return Architecture.BLL.Inventory.Item.GetEqvilentByItemId(obj);
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
			return Architecture.BLL.Inventory.Item.GetEqvilentByItemIdAndUomScheduleId(obj);
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
			return GenericProvider.GetDataTableProc<ConfigrationsAllocation>("USP_GetAvgRateAndStockInHand", p);
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
			return GetAvgRatesAndStockInHand.AvgRateOnlyForCGS(obj);
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
			return GenericProvider.GetDataTableProc<ConfigrationsAllocation>("USP_GetActivityGlIdAndName", p);
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
			return GenericProvider.GetDataTableProc<ConfigrationsAllocation>("[dbo].[USP_GetActivityGlIdAndNameForExtra]", p);
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
			return GenericProvider.GetDataTableProc<ConfigrationsAllocation>("USP_GetParentCategoriesForPurchaseAndSaleReports", p);
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
			DataTable dt = GenericProvider.GetDataTableProc<ConfigrationsAllocation>("usp_getAvgRateByJobOrder", p);
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

	public static double getCostRateFromExportVoucherByInvoice(ReportsParameters obj)
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
				ParameterName = "@JobLotId",
				Value = obj.JobLotId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@InvoiceId",
				Value = obj.InvoiceId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@InvoiceDetailId",
				Value = obj.InvoiceDetailId
			});
			if (!string.IsNullOrWhiteSpace(obj.CropYear))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
				});
			}
			if (obj.CropYearId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYearId",
					Value = obj.CropYearId
				});
			}
			if (obj.WarehouseId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.ItemUomId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemUomId",
					Value = obj.ItemUomId
				});
			}
			if (obj.PackingTypeId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvPackingTypeId",
					Value = obj.PackingTypeId
				});
			}
			DataTable dt = GenericProvider.GetDataTableProc<ConfigrationsAllocation>("usp_getCostRateFromExportVoucherByInvoice", p);
			if (dt != null && dt.Rows.Count > 0)
			{
				AvgRate = Conversion.ToDouble(dt.Rows[0]["CostRate"]);
			}
			return AvgRate;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SelectAndOptionallyRenameColumns(DataTable sourceTable, Dictionary<string, string> columnMappings)
	{
		try
		{
			if (columnMappings == null || columnMappings.Count == 0)
			{
				throw new ArgumentException("columnMappings must contain at least one entry.");
			}
			DataTable result = new DataTable();
			foreach (KeyValuePair<string, string> mapping in columnMappings)
			{
				string originalName = mapping.Key;
				string newName = (string.IsNullOrWhiteSpace(mapping.Value) ? originalName : mapping.Value);
				if (!sourceTable.Columns.Contains(originalName))
				{
					throw new ArgumentException("Column '" + originalName + "' does not exist in the source table.");
				}
				result.Columns.Add(newName, sourceTable.Columns[originalName].DataType);
			}
			foreach (DataRow row in sourceTable.Rows)
			{
				object[] values = columnMappings.Select(delegate(KeyValuePair<string, string> m)
				{
					object obj = row[m.Key];
					return (obj is string text) ? text.Replace("\r", " ").Replace("\n", " ").Trim() : obj;
				}).ToArray();
				result.Rows.Add(values);
			}
			return result;
		}
		catch (Exception ex)
		{
			throw new Exception("Error selecting and optionally renaming columns: " + ex.Message, ex);
		}
	}
}
