using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL.Inventory;

public class InvDashboardsPendingWork
{
	public static DataTable DashboardsPendingWork(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_Dashboards_Inventory_PendingWork", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
