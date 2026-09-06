using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.DAL.Common;
using Architecture.DAL.Import;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Import;

namespace Architecture.BLL.Import;

public class masterDocumentSerial
{
	private static void FillEntity(Architecture.Model.Import.masterDocumentSerial obj, ActionMode mod)
	{
		try
		{
			switch (mod)
			{
			case ActionMode.NEW:
				obj.AppActionId = 1;
				break;
			case ActionMode.EDIT:
				obj.AppActionId = 2;
				break;
			case ActionMode.DELETE:
				obj.AppActionId = 3;
				break;
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static object Delete(Architecture.Model.Import.masterDocumentSerial obj, ParameterObject p = null)
	{
		try
		{
			FillEntity(obj, ActionMode.DELETE);
			return masterDocumentSerialProvider.Set(obj, "Delete");
		}
		catch (Exception ex)
		{
			throw new Exception("Error deleting masterDocumentSerial", ex);
		}
	}

	public static object Save(Architecture.Model.Import.masterDocumentSerial obj)
	{
		try
		{
			string action = ((obj.masterDocumentSerialId <= 0) ? "Insert" : "UPDATE");
			FillEntity(obj, (obj.masterDocumentSerialId <= 0) ? ActionMode.NEW : ActionMode.EDIT);
			return masterDocumentSerialProvider.Set(obj, action);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Architecture.Model.Import.masterDocumentSerial ReadById(long masterDocumentSerialID)
	{
		try
		{
			List<ParameterList> p = new List<ParameterList>
			{
				new ParameterList
				{
					ParameterName = "@masterDocumentSerialID",
					ParameterValue = masterDocumentSerialID
				}
			};
			return masterDocumentSerialProvider.GetAll("ID", p)?.FirstOrDefault();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<Architecture.Model.Import.masterDocumentSerial> SEARCHHistoryList(ReportsParameters obj)
	{
		try
		{
			List<ParameterList> p = new List<ParameterList>
			{
				new ParameterList
				{
					ParameterName = "@Id",
					ParameterValue = obj.Id
				}
			};
			return masterDocumentSerialProvider.GetAll("SEARCH", p) ?? new List<Architecture.Model.Import.masterDocumentSerial>();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable SEARCHHistory(ReportsParameters obj)
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsPerforma",
					Value = obj.ActionId
				});
			}
			return GenericProvider.GetAllDataTableCore<masterDocumentSerial>("SEARCH", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ComboForInvoice(ReportsParameters obj)
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsPerforma",
					Value = obj.ActionId
				});
			}
			if (obj.RecId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@invoiceMasterId",
					Value = obj.RecId
				});
			}
			return GenericProvider.GetAllDataTableCore<masterDocumentSerial>("ComboForInvoice", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
