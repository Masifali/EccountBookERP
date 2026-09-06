using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.PartyProcessing;
using Architecture.Model.PartyProcessing;

namespace Architecture.BLL.PartyProcessing;

public class JobLotPartyProcessing
{
	public static int Save(Architecture.Model.PartyProcessing.JobLotPartyProcessing obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.PartyProcessing.JobLotPartyProcessing().SetData(obj, "Sp_JobLotPartyProcessing_Insert");
			}
			return new Architecture.DAL.PartyProcessing.JobLotPartyProcessing().SetData(obj, "Sp_JobLotPartyProcessing_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.PartyProcessing.JobLotPartyProcessing GetByID(int Id)
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
				Value = "GetById"
			});
			new List<Architecture.Model.PartyProcessing.JobLotPartyProcessing>();
			return new Architecture.DAL.PartyProcessing.JobLotPartyProcessing().GetData("Sp_JobLotPartyProcessing_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.PartyProcessing.JobLotPartyProcessing> GetList(Architecture.Model.PartyProcessing.JobLotPartyProcessing obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@Activity", "GetAll"));
			return new Architecture.DAL.PartyProcessing.JobLotPartyProcessing().GetData("Sp_JobLotPartyProcessing_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.PartyProcessing.JobLotPartyProcessing obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@Activity", "GetAll"));
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.JobLotPartyProcessing>("Sp_JobLotPartyProcessing_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
