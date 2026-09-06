using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model;
using Architecture.Model.Export;

namespace Architecture.BLL.Export;

public class ExImLcContractScheduleDepartment
{
	public static int Save(Architecture.Model.Export.ExImLcContractScheduleDepartment obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.ActionTypeId = 1;
			}
			else
			{
				obj.ActionTypeId = 2;
			}
			return new Architecture.DAL.Export.ExImLcContractScheduleDepartment().SetData(obj, "[dbo].[USP_ExImLcContractScheduleDepartment_Insert]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDatForContractScheduleDepartment(ReportsParameters obj)
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
			if (obj.ContractScheduleId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractScheduleId",
					Value = obj.ContractScheduleId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcContractScheduleDepartment>("[dbo].[USP_GetDatForContractScheduleDepartment]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
