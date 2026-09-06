using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.ChequePrinting;
using Architecture.DAL.Common;
using Architecture.Model.ChequePrinting;

namespace Architecture.BLL.ChequePrinting;

public class ChequePrintList
{
	public static int Save(Architecture.Model.ChequePrinting.ChequePrintList obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.ChequePrinting.ChequePrintList().SetData(obj, "[cqp].[Sp_ChequePrintList_Insert]");
			}
			return new Architecture.DAL.ChequePrinting.ChequePrintList().SetData(obj, "[cqp].[Sp_ChequePrintList_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateChequePrintStatus(string Ids)
	{
		try
		{
			if (Ids != string.Empty)
			{
				new List<SqlParameter>().Add(new SqlParameter
				{
					ParameterName = "@Ids",
					Value = Ids
				});
				new Architecture.DAL.ChequePrinting.ChequePrintList().UpdateChequePrintStatus(Ids, "[dbo].[sp_ChequePrintList_UpdateStatus]");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.ChequePrinting.ChequePrintList ReadById(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return new Architecture.DAL.ChequePrinting.ChequePrintList().GetData("[cqp].[Sp_ChequePrintList_ReadAll]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable History(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ChequePrinting.ChequePrintList>("[cqp].[Sp_ChequePrintList_ReadAll]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PrintCheque(int VoucherDetailId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@VoucherDetailId",
				Value = VoucherDetailId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ChequePrinting.ChequePrintList>("[cqp].[sp_chequePrint]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DataForPrnt(string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Ids",
				Value = Ids
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ChequePrinting.ChequePrintList>("[dbo].[sp_ChequePrintList_Print]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
