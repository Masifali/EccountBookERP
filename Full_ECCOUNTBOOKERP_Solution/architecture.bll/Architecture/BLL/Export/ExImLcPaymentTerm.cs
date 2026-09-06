using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model.Export;

namespace Architecture.BLL.Export;

public class ExImLcPaymentTerm
{
	public static int Save(Architecture.Model.Export.ExImLcPaymentTerm obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.PostDate = DateTime.Now;
			obj.PostState = false;
			obj.PostUser = 0;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Export.ExImLcPaymentTerm().SetData(obj, "Sp_ExImLcPaymentTerm_Insert");
			}
			return new Architecture.DAL.Export.ExImLcPaymentTerm().SetData(obj, "Sp_ExImLcPaymentTerm_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ExImLcPaymentTerm GetByID(int Id)
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
			new List<Architecture.Model.Export.ExImLcPaymentTerm>();
			return new Architecture.DAL.Export.ExImLcPaymentTerm().GetAll("Sp_ExImLcPaymentTerm_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.Export.ExImLcPaymentTerm obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImDeliveryTerm>("Sp_ExImLcPaymentTerm_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
