using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.FeedMill.Production;
using Architecture.Model.FeedMill.Production;

namespace Architecture.BLL.FeedMill.Production;

public class ProductionElectricityUnits
{
	public static int Save(Architecture.Model.FeedMill.Production.ProductionElectricityUnits obj)
	{
		try
		{
			if (obj.ProductionElectricityUnitsList.Count > 0)
			{
				obj.OrganizationId = obj.ProductionElectricityUnitsList[0].OrganizationId;
				obj.CompanyId = obj.ProductionElectricityUnitsList[0].CompanyId;
			}
			_ = obj.Id;
			return new Architecture.DAL.FeedMill.Production.ProductionElectricityUnits().SetData(obj, "[fed].[USP_ProductionElectricityUnits_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.FeedMill.Production.ProductionElectricityUnits GetByID(DateTime Date)
	{
		try
		{
			List<Architecture.Model.FeedMill.Production.ProductionElectricityUnits> lst = new List<Architecture.Model.FeedMill.Production.ProductionElectricityUnits>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = Date
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByDate"
			});
			lst = new Architecture.DAL.FeedMill.Production.ProductionElectricityUnits().GetDate("[fed].[USP_ProductionElectricityUnits_GetAllMethod]", p);
			return (lst != null && lst.Count > 0) ? lst[0] : null;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByID_MonthYear(int Month, int Year)
	{
		try
		{
			new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Month",
				Value = Month
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Year",
				Value = Year
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByMonthYear"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Production.ProductionElectricityUnits>("[fed].[USP_ProductionElectricityUnits_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
