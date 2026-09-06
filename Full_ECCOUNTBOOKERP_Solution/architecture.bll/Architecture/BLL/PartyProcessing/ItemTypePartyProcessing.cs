using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.PartyProcessing;
using Architecture.Model.PartyProcessing;

namespace Architecture.BLL.PartyProcessing;

public class ItemTypePartyProcessing
{
	public static int Save(Architecture.Model.PartyProcessing.ItemTypePartyProcessing obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.PartyProcessing.ItemTypePartyProcessing().SetData(obj, "Sp_ItemTypePartyProcessing_Insert");
			}
			return new Architecture.DAL.PartyProcessing.ItemTypePartyProcessing().SetData(obj, "Sp_ItemTypePartyProcessing_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.PartyProcessing.ItemTypePartyProcessing GetByID(int Id)
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
			new List<Architecture.Model.PartyProcessing.ItemTypePartyProcessing>();
			return new Architecture.DAL.PartyProcessing.ItemTypePartyProcessing().GetAll("Sp_ItemTypePartyProcessing_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.PartyProcessing.ItemTypePartyProcessing obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.ItemTypePartyProcessing>("Sp_ItemTypePartyProcessing_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
