using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class SupplierCustomer
{
	public static int Save(Architecture.Model.Inventory.SupplierCustomer obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (Conversion.CheckDateTimeNull(obj.CNIC_EXPIRY_DATE))
			{
				obj.CNIC_EXPIRY_DATE = DateTime.Now;
			}
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.SupplierCustomer().SetData(obj, "Sp_SupplierCustomer_Insert");
			}
			return new Architecture.DAL.Inventory.SupplierCustomer().SetData(obj, "Sp_SupplierCustomer_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.SupplierCustomer GetByID(int Id)
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
				Value = "ReadById"
			});
			new List<Architecture.Model.Inventory.SupplierCustomer>();
			return new Architecture.DAL.Inventory.SupplierCustomer().GetData("Sp_SupplierCustomer_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierBySaleOrderNoForSteel(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@SaleOrderId",
				Value = obj.OrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpDate",
					Value = obj.DocDate
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[ST].[USP_GetSupplierBySaleOrderNo]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable Getall(Architecture.Model.Inventory.SupplierCustomer obj)
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
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetforComboBinding(Architecture.Model.Inventory.SupplierCustomer obj)
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
				ParameterName = "@Activity",
				Value = "ReadByOrganizationIdCompanyIdForBinding"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AllSupplierCustomerWithCityName(Architecture.Model.Inventory.SupplierCustomer obj)
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
				ParameterName = "@Activity",
				Value = "AllSupplierCustomerWithCityName"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGlAccountIdandCompanyNameBySupplierCustomerId(Architecture.Model.Inventory.SupplierCustomer obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.Id
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetGlAccountIdandCompanyNameBySupplierCustomerId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetGlAccountIdBySupplierCustomerId(ReportsParameters obj)
	{
		try
		{
			int Id = 0;
			DataTable dt = null;
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
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetGlAccountIdBySupplierCustomerId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Id = Conversion.ToInt(dt.Rows[0][0]);
			}
			return Id;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierustomerByCustomerGroupId(ReportsParameters obj)
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
			if (obj.ParentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentId",
					Value = obj.ParentId
				});
			}
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerGroupIds",
					Value = obj.Ids
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSupplierustomerByCustomerGroupId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSameGLAccountPartiesByPartyId(ReportsParameters obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSameGLAccountPartiesByPartyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Inventory.SupplierCustomer> GetAllList(Architecture.Model.Inventory.SupplierCustomer obj)
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
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyId"
			});
			return new Architecture.DAL.Inventory.SupplierCustomer().GetData("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(ReportsParameters obj)
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
			if (obj.PartyTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyTypeId",
					Value = obj.PartyTypeId
				});
			}
			if (obj.CustomerGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerGroupId",
					Value = obj.CustomerGroupId
				});
			}
			if (obj.GlAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GlAccountId",
					Value = obj.GlAccountId
				});
			}
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CityId",
					Value = obj.CityId
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsTaxable",
					Value = obj.IsTaxable
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByOrganizationCompanyIdForExport(Architecture.Model.Inventory.SupplierCustomer obj)
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
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyIdForExport"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByOrganizationCompanyIdForContractorWages(Architecture.Model.Inventory.SupplierCustomer obj)
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
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyIdForContractorWages"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PartyClassification()
	{
		DataTable dt = new DataTable();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			using SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[V_PartyClassification]", sqlcon);
			cmd.CommandType = CommandType.Text;
			using SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			return dt;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataTable TaxImplementationType()
	{
		DataTable dt = new DataTable();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			using SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[VTaxImplementationType]", sqlcon);
			cmd.CommandType = CommandType.Text;
			using SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			return dt;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataTable GetSupplierByPurchaseOrderNo(ReportsParameters obj)
	{
		try
		{
			new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@PurchaseOrderId",
				Value = obj.OrderId
			});
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpId",
					Value = obj.Id
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpDate",
					Value = obj.DocDate
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "SupplierByPurchaseOrderNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetSupplierBySaleOrderNo(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@SaleOrderId",
				Value = obj.OrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpDate",
					Value = obj.DocDate
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSupplierBySaleOrderNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetCustomerIdByLcOrderAndFcReceipt(Architecture.Model.Inventory.SupplierCustomer obj)
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
				ParameterName = "@Activity",
				Value = "GetCustomerIdByLcOrderAndFcReceipt"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetIdbyGlAccountId(Architecture.Model.Inventory.SupplierCustomer obj)
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
				ParameterName = "@GlAccountId",
				Value = obj.GlAccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetIdByGlAccountId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByOrganizationCompanyIdForExportShippinglineNClearingAgents(Architecture.Model.Inventory.SupplierCustomer obj)
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
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyIdForExportShippinglineNClearingAgents"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierByImportPurchaseContract(ReportsParameters obj)
	{
		try
		{
			new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ShipmentNo",
				Value = obj.InvoiceNo
			});
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSupplierByImportPurchaseOrderNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetSupplierCustomerFromOrderByInvoice(ReportsParameters obj)
	{
		try
		{
			new DataTable();
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSupplierCustomerFromOrderByInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetSalesManForSupplierustomer(ReportsParameters obj)
	{
		try
		{
			new DataTable();
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
				ParameterName = "@Activity",
				Value = "GetSalesManForSupplierustomer"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetSupplierustomerForPartyProcessing(Architecture.Model.Inventory.SupplierCustomer obj)
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
				ParameterName = "@Activity",
				Value = "GetSupplierustomerForPartyProcessing"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromGRNGDN(ReportsParameters obj)
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
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("USP_GetDataForDropDownFromGrnGdnStorePartyProcessing", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryForPartyProcessing(Architecture.Model.Inventory.SupplierCustomer obj)
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
				ParameterName = "@Activity",
				Value = "FormHistoryForPartyProcessing"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierCustomerAndCommissionAgentFromPurchaseAndSales(ReportsParameters obj)
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyType",
					Value = obj.Activity
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSupplierCustomerAndCommissionAgentFromPurchaseAndSales"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[Sp_SupplierCustomer_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierCustomerInfoByGLAccountId(Architecture.Model.Inventory.SupplierCustomer obj)
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
				ParameterName = "@GlAccountId",
				Value = obj.GlAccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSupplierCustomerInfoByGLAccountId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ContractorHistory(int OrganizationId, int CompanyId)
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
				Value = "ContractorHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SupplierCustomerDataUpdateById(Architecture.Model.Inventory.SupplierCustomer obj)
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
				ParameterName = "@MobilePersonal",
				Value = obj.MobilePersonal
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "SupplierCustomerDataUpdateById"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SupplierCustomerAgainstPurchaseInvoice(Architecture.Model.Inventory.SupplierCustomer obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Usp_SupplierCustomerAgainstPurchaseInvoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SupplierCustomerAgainstOutwarGeneralGP(Architecture.Model.Inventory.SupplierCustomer obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Usp_SupplierCustomerAgainstOutwarGeneralGP", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SupplierCustomerAgainstPurchaseOrder(ReportsParameters obj)
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Usp_SupplierCustomerAgainstPurchaseOrder", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SupplierCustomerAgainstOutwarGP(Architecture.Model.Inventory.SupplierCustomer obj)
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
			if (obj.BranchId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Usp_SupplierCustomerAgainstOutwarGP", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SupplierCustomerAgainstSaleOrder(Architecture.Model.Inventory.SupplierCustomer obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Usp_SupplierCustomerAgainstSaleOrder", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SupplierCustomerAgainstDeliveryOrder(Architecture.Model.Inventory.SupplierCustomer obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Usp_SupplierCustomerAgainstDeliveryOrder", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SupplierCustomerAgainstSaleInvoice(Architecture.Model.Inventory.SupplierCustomer obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Usp_SupplierCustomerAgainstSaleInvoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPartiesFromWbTransactions(int OrganizationId, int CompanyId)
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
				Value = "GetPartiesFromWbTransactions"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_WbTransation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PartyTypeGetAll(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[USP_GetAllSupplierCustomerType]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAccountsForSupplierCustomerByPartyTypeId(int OrganizationId, int CompanyId, int PartyTypeId, int GlRecId = 0)
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
				ParameterName = "@PartyTypeId",
				Value = PartyTypeId
			});
			if (GlRecId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GlRecId",
					Value = GlRecId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[USP_GetAccountsForSupplierCustomerByPartyType]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static string GeneratePartyCode(int OrganizationId, int CompanyId, int PartyTypeId)
	{
		try
		{
			string Code = null;
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
				ParameterName = "@PartyTypeId",
				Value = PartyTypeId
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("USP_GeneratePartyCode", p);
			if (dt.Rows.Count > 0)
			{
				Code = Conversion.ToString(dt.Rows[0][0]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetVendorsAndCustomers(int OrganizationId, int CompanyId, int PartyTypeId = 0)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("USP_GetVendorsAndCustomers", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetVendorsAndCustomersWithCityName(int OrganizationId, int CompanyId, int PartyTypeId = 0)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("USP_GetVendorsAndCustomersWithCityName", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetVendorsAndCustomersForTransporter(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[USP_GetVendorsAndCustomersForTransporter]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExportCustomerFromVouchers(ReportsParameters obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[USP_GetExportCustomerFromVouchers]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetSupplierCustomerIdbyGlAccountId(int GLAccountId)
	{
		int Id = 0;
		try
		{
			DataTable dt = null;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = clsGlobalVariables.UserAccount.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = clsGlobalVariables.UserAccount.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@GlAccountId",
				Value = GLAccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetIdByGlAccountId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Id = Conversion.ToInt(dt.Rows[0][0]);
			}
			return Id;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllTaxableSupplierCustomer(int OrganizationId, int CompanyId)
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
				Value = "GetAllTaxableSupplierCustomer"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPartyCustomGroupsByDocumentTypeId(int OrganizationId, int CompanyId, int DocumentTypeId)
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
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("usp_getPartyCustomGroupsByDocumentTypeId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierCustomerByCustomGroupId(int OrganizationId, int CompanyId, int CustomGroupId)
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
				ParameterName = "@CustomGroupId",
				Value = CustomGroupId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[usp_getSupplierCustomerByCustomGroupId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getSupplierCustomersForDropdown(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AppId",
				Value = obj.AppId
			});
			if (obj.PartyTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyTypeId",
					Value = obj.PartyTypeId
				});
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
				});
			}
			if (!string.IsNullOrEmpty(obj.CustomerGroupIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerGroupIds",
					Value = obj.CustomerGroupIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("usp_getSupplierCustomersForDropdown", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSaleOrderDataForOutward(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrderNo",
				Value = obj.DocNo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpDate",
					Value = obj.DocDate
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("ST.USP_GetSupplierBySaleOrderNo", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetDataForDropDownFrom_SupplierCustomer(ReportsParameters obj)
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
			if (obj.Activity != string.Empty && obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("USP_GetDataForDropDownFrom_SupplierCustomer", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
