using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class CheqBookHeader
{
	public static int Save(Architecture.Model.Accounts.CheqBookHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.PostDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.Accounts.CheqBookHeader().SetDate(obj, "SP_CheqBookHeader_Insert");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetData(Architecture.Model.Accounts.CheqBookHeader obj)
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
				ParameterName = "@MethodType",
				Value = "GetAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.CheqBookHeader>("SP_CheqBookHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable OutstandingCheqNo(Architecture.Model.Accounts.CheqBookHeader obj)
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
				ParameterName = "@BankId",
				Value = obj.BankId
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.Id
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.ActionId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@MethodType",
				Value = "OutstandingCheqNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.CheqBookHeader>("SP_CheqBookHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int CheqBookDetailSave(Architecture.Model.Accounts.CheqBookHeader obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.CheqBookHeader().SetDataDetail(obj, "SP_CheqBookDetail_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CancelChequeHistory(Architecture.Model.Accounts.CheqBookHeader obj)
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
				ParameterName = "@MethodType",
				Value = "CancelChequeHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.CheqBookHeader>("SP_CheqBookHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static CheqBookDetail ReadByDetailId(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@RecId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@MethodType",
				Value = "ReadByDetailId"
			});
			new List<CheqBookDetail>();
			return new Architecture.DAL.Accounts.CheqBookHeader().GetDetailData("[SP_CheqBookHeader_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Accounts.CheqBookHeader ReadByHeaderId(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@RecId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@MethodType",
				Value = "ReadByHeaderId"
			});
			new List<Architecture.Model.Accounts.CheqBookHeader>();
			return new Architecture.DAL.Accounts.CheqBookHeader().GetDate("[SP_CheqBookHeader_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
