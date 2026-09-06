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

public class SupCustCreditLimitSchedule
{
	public static int Save(Architecture.Model.Inventory.SupCustCreditLimitSchedule obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.SupCustCreditLimitSchedule().SetData(obj, "Sp_SupCustCreditLimitSchedule_Insert");
			}
			return new Architecture.DAL.Inventory.SupCustCreditLimitSchedule().SetData(obj, "Sp_SupCustCreditLimitSchedule_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.SupCustCreditLimitSchedule GetByID(int Id)
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
			new List<Architecture.Model.Inventory.SupCustCreditLimitSchedule>();
			return new Architecture.DAL.Inventory.SupCustCreditLimitSchedule().GetData("Sp_SupCustCreditLimitSchedule_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.Inventory.SupCustCreditLimitSchedule obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupCustCreditLimitSchedule>("Sp_SupCustCreditLimitSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetSupplierCustomerCreditAndDebitLimit(ReportsParameters obj)
	{
		try
		{
			double Limit = 0.0;
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
				ParameterName = "@SuplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectedDate",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSupplierCustomerCreditAndDebitLimit"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupCustCreditLimitSchedule>("Sp_SupCustCreditLimitSchedule_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Limit = Conversion.ToDouble(dt.Rows[0]["PartyLimit"]);
			}
			return Limit;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
