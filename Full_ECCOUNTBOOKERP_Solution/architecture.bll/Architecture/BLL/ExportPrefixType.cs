using System;
using System.Data;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class ExportPrefixType
{
	public static int Save(Architecture.Model.ExportPrefixType obj)
	{
		try
		{
			return new Architecture.DAL.ExportPrefixType().SetData(obj, "usp_ExportInvoiceNosPrefix_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory()
	{
		try
		{
			return GenericProvider.GetDataTableProc<Architecture.Model.ExportPrefixType>("usp_ExportInvoiceNosPrefix_History");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
