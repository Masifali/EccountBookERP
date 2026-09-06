using System;
using System.Data;
using Architecture.DAL.Common;
using Architecture.Model.ChequePrinting;

namespace Architecture.DAL.ChequePrinting;

public class ChequeType
{
	public static DataTable GetChequetyType()
	{
		try
		{
			return GenericProvider.GetDataTableProc<Architecture.Model.ChequePrinting.ChequeType>("[dbo].[sp_ChequeType]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
