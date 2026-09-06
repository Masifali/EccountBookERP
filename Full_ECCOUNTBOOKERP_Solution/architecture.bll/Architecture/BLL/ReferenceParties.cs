using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class ReferenceParties
{
	public static int save(Architecture.Model.ReferenceParties obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.ReferenceParties().SetData(obj, "Sp_ReferenceParties_Insert");
			}
			return new Architecture.DAL.ReferenceParties().SetData(obj, "Sp_ReferenceParties_Update");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll(Architecture.Model.ReferenceParties obj)
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
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ReferenceParties>("[Sp_ReferenceParties_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.ReferenceParties obj)
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
			if (obj.ReferencePartyTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReferencePartyTypeId",
					Value = obj.ReferencePartyTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ReferenceParties>("[Sp_ReferenceParties_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId(Architecture.Model.ReferenceParties obj)
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
			if (obj.ReferencePartyTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReferencePartyTypeId",
					Value = obj.ReferencePartyTypeId
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ReferenceParties>("[Sp_ReferenceParties_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAllReferencePartyType(Architecture.Model.ReferenceParties obj)
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
				Value = "ReadAllReferencePartyType"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ReferenceParties>("[Sp_ReferenceParties_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.ReferenceParties GetById(int id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = id
			});
			new List<Architecture.Model.ReferenceParties>();
			return new Architecture.DAL.ReferenceParties().GetDate("Sp_ReferenceParties_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
