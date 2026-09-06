using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.PartyProcessing;
using Architecture.Model.PartyProcessing;

namespace Architecture.BLL.PartyProcessing;

public class InvWareHousePartyProcessing
{
	public static int Save(Architecture.Model.PartyProcessing.InvWareHousePartyProcessing obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.PartyProcessing.InvWareHousePartyProcessing().SetData(obj, "Sp_InvWareHousePartyProcessing_Insert");
			}
			return new Architecture.DAL.PartyProcessing.InvWareHousePartyProcessing().SetData(obj, "Sp_InvWareHousePartyProcessing_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.PartyProcessing.InvWareHousePartyProcessing GetByID(int Id)
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
			new List<Architecture.Model.PartyProcessing.InvWareHousePartyProcessing>();
			return new Architecture.DAL.PartyProcessing.InvWareHousePartyProcessing().GetData("Sp_InvWareHousePartyProcessing_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.PartyProcessing.InvWareHousePartyProcessing> GetList(Architecture.Model.PartyProcessing.InvWareHousePartyProcessing obj)
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
			new List<Architecture.Model.PartyProcessing.InvWareHousePartyProcessing>();
			return new Architecture.DAL.PartyProcessing.InvWareHousePartyProcessing().GetData("Sp_InvWareHousePartyProcessing_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.PartyProcessing.InvWareHousePartyProcessing obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.InvWareHousePartyProcessing>("Sp_InvWareHousePartyProcessing_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
