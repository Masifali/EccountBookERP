using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model.Export;

namespace Architecture.BLL.Export;

public class ExImShipmentDocuments
{
	public static int Save(Architecture.Model.Export.ExImShipmentDocuments obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Export.ExImShipmentDocuments().SetData(obj, "Sp_ExImShipmentDocuments_Insert");
			}
			return new Architecture.DAL.Export.ExImShipmentDocuments().SetData(obj, "Sp_ExImShipmentDocuments_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ExImShipmentDocuments GetByID(int Id)
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
			new List<Architecture.Model.Export.ExImShipmentDocuments>();
			return new Architecture.DAL.Export.ExImShipmentDocuments().GetAll("Sp_ExImShipmentDocuments_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImDeliveryTerm>("Sp_ExImShipmentDocuments_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
