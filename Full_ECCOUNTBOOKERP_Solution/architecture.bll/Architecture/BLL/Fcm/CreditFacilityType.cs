using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Fcm;
using Architecture.Model.Fcm;

namespace Architecture.BLL.Fcm;

public class CreditFacilityType
{
	public static int save(Architecture.Model.Fcm.CreditFacilityType obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.CreditFacilityTypeId == 0)
			{
				return new Architecture.DAL.Fcm.CreditFacilityType().SetData(obj, "[Fcm].[USP_CreditFacilityType_Insert]");
			}
			return new Architecture.DAL.Fcm.CreditFacilityType().SetData(obj, "[Fcm].[USP_CreditFacilityType_Update]");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll(Architecture.Model.Fcm.CreditFacilityType obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			return GenericProvider.GetDataTableProc<Architecture.Model.Fcm.CreditFacilityType>("[Fcm].[Sp_CreditFacilityType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByStateProvinceId(Architecture.Model.Fcm.CreditFacilityType obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			return GenericProvider.GetDataTableProc<Architecture.Model.Fcm.CreditFacilityType>("[Fcm].[Sp_CreditFacilityType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
