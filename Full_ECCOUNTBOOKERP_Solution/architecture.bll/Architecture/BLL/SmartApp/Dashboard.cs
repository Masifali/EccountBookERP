using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.SmartApp;
using Architecture.Model;
using Architecture.Model.SmartApp;

namespace Architecture.BLL.SmartApp;

public class Dashboard
{
	public static List<Architecture.Model.SmartApp.Dashboard> ExectiveDashboard(ReportsParameters obj)
	{
		try
		{
			List<Architecture.Model.SmartApp.Dashboard> FinalList = new List<Architecture.Model.SmartApp.Dashboard>();
			DataTable dtObj = Architecture.BLL.Dashboard.DashboardDynamicObjects(obj);
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			if (obj.CompanyIds != null && obj.CompanyIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyIds",
					Value = obj.CompanyIds
				});
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@VoucherDateF", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@VoucherDateT", obj.ToDate));
			}
			p.Add(new SqlParameter("@ActivityId", obj.ActivityId));
			List<Architecture.Model.SmartApp.Dashboard> lst = new Architecture.DAL.SmartApp.Dashboard().GetAll("Sp_ExectiveDashboard", p);
			if (obj.ActivityId == 2)
			{
				return lst;
			}
			if (lst.Count > 0 && dtObj.Rows.Count > 0)
			{
				for (int i = 0; i < dtObj.Rows.Count; i++)
				{
					for (int j = 0; j < lst.Count; j++)
					{
						if (Conversion.ToString(dtObj.Rows[i]["CardIdentitiyName"]) == Conversion.ToString(lst[j].AccountDescription))
						{
							lst[j].SortingNo = Conversion.ToInt(dtObj.Rows[i]["SortingNo"]);
							FinalList.Add(lst[j]);
						}
					}
				}
			}
			return FinalList;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
