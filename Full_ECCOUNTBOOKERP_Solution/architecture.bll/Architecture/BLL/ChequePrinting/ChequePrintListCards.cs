using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.ChequePrinting;

namespace Architecture.BLL.ChequePrinting;

public class ChequePrintListCards
{
	public static DataTable getChequePrintListCards(int OrganizationId, int CompanyId, int BankId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@BankId",
				Value = BankId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ChequePrinting.ChequePrintListCards>("[dbo].[sp_ChequePrintList]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getChequePrintListCardsByVoucherId(int VoucherId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@VoucherId",
				Value = VoucherId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ChequePrinting.ChequePrintListCards>("[dbo].[sp_ChequePrintListByVoucherId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
