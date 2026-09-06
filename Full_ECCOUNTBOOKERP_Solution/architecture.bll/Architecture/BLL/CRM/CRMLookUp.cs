using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.CRM;
using Architecture.DAL.Common;
using Architecture.Model.CRM;

namespace Architecture.BLL.CRM;

public class CRMLookUp
{
	public static int Save(Architecture.Model.CRM.CRMLookUp obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.CRM.CRMLookUp().SetData(obj, "Sp_CRMLookUp_Insert");
			}
			return new Architecture.DAL.CRM.CRMLookUp().SetData(obj, "Sp_CRMLookUp_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.CRM.CRMLookUp obj)
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
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.CRMLookupTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CRMLookupTypeId",
					Value = obj.CRMLookupTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CRM.CRMLookUp>("Sp_CRMLookUp_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GenerateCode(Architecture.Model.CRM.CRMLookUp obj)
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
				ParameterName = "@CRMLookupTypeId",
				Value = obj.CRMLookupTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCodeByLookUpType"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CRM.CRMLookUp>("Sp_CRMLookUp_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
