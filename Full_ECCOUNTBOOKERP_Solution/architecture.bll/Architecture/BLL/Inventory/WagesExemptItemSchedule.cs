using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class WagesExemptItemSchedule
{
	public static int Save(Architecture.Model.Inventory.WagesExemptItemSchedule obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.WagesExemptItemSchedule().SetData(obj, "USp_WagesExemptItemSchedule_Insert");
			}
			return new Architecture.DAL.Inventory.WagesExemptItemSchedule().SetData(obj, "USp_WagesExemptItemSchedule_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.WagesExemptItemSchedule GetById(int Id)
	{
		try
		{
			new List<Architecture.Model.Inventory.WagesExemptItemSchedule>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return new Architecture.DAL.Inventory.WagesExemptItemSchedule().GetAll("USp_WagesExemptItemSchedule_ReadById", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Inventory.WagesExemptItemSchedule obj)
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
			if (!Conversion.CheckDateTimeNull(obj.EffectedFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EffectedFrom",
					Value = obj.EffectedFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EffectedTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EffectedTo",
					Value = obj.EffectedTo
				});
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocumentTypeId",
					Value = obj.RefDocumentTypeId
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.WagesExemptItemSchedule>("USp_WagesExemptItemSchedule_Form History", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
