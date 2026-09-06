using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL;

public class Graph
{
	public static DataSet OrganizationSaleAnalytics(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			if (obj.CompanyIds != null && obj.CompanyIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyIds",
					Value = obj.CompanyIds
				});
			}
			if (obj.BranchIds != null && obj.BranchIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchIds",
					Value = obj.BranchIds
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			return GenericProvider.GetDataSetProc<InvSaleInvoice>("USP_OrgSalesAnalytics001A", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet MonthlySaleAnalytics(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			if (obj.CompanyIds != null && obj.CompanyIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyIds",
					Value = obj.CompanyIds
				});
			}
			if (obj.BranchIds != null && obj.BranchIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchIds",
					Value = obj.BranchIds
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter("@TopBottom", obj.NoOfRecords));
			}
			if (obj.ReqType != null && obj.ReqType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderByFlag",
					Value = obj.ReqType
				});
			}
			return GenericProvider.GetDataSetProc<InvSaleInvoice>("USP_OrgSalesAnalytics001B", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
