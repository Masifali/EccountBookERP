using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class AccountsOpeningBalances : DALBase
{
	public int SetData(Architecture.Model.Accounts.AccountsOpeningBalances obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public int SetDataAllRecord(Architecture.Model.Accounts.AccountsOpeningBalances obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Accounts.AccountsOpeningBalances item in obj.OpeningBalanceList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Accounts.AccountsOpeningBalances> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.AccountsOpeningBalances> lst = new List<Architecture.Model.Accounts.AccountsOpeningBalances>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Accounts.AccountsOpeningBalances>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.Accounts.AccountsOpeningBalances> GetDataAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.AccountsOpeningBalances> lst = new List<Architecture.Model.Accounts.AccountsOpeningBalances>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.Get<Architecture.Model.Accounts.AccountsOpeningBalances>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public Architecture.Model.Accounts.AccountsOpeningBalances ReadByChartOfAccountId(int ChartOfAccountId)
	{
		Architecture.Model.Accounts.AccountsOpeningBalances openingBalance = null;
		IDataReader reader = null;
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("SP_AccountsOpeningBalances_ReadByChartOfAccountId");
			sqlDataBase.AddInParameter(cmd, "ChartOfAccountId", SqlDbType.Int, ChartOfAccountId);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				openingBalance = FillDTO(reader);
			}
			return openingBalance;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			if (reader != null && !reader.IsClosed)
			{
				reader.Close();
			}
		}
	}

	private Architecture.Model.Accounts.AccountsOpeningBalances FillDTO(IDataReader reader)
	{
		try
		{
			return new Architecture.Model.Accounts.AccountsOpeningBalances
			{
				ChartOfAccountId = Conversion.ToInt(reader["ChartOfAccountId"]),
				ChartOfAccountTitle = Conversion.ToString(reader["ChartOfAccountTitle"]),
				CompanyId = Conversion.ToInt(reader["CompanyId"]),
				EntryDate = Conversion.ToDateTime(reader["EntryDate"]),
				EntryUser = Conversion.ToInt(reader["EntryUser"]),
				FinancialYearId = Conversion.ToInt(reader["FinancialYearId"]),
				Id = Conversion.ToInt(reader["Id"]),
				ModifyDate = Conversion.ToDateTime(reader["ModifyDate"]),
				ModifyUser = Conversion.ToInt(reader["ModifyUser"]),
				OrganizationId = Conversion.ToInt(reader["OrganizationId"]),
				PostDate = Conversion.ToDateTime(reader["PostDate"]),
				PostState = Conversion.ToBool(reader["PostState"]),
				PostUser = Conversion.ToInt(reader["PostUser"]),
				YearObCredit = Conversion.ToDouble(reader["YearObCredit"]),
				YearObDebit = Conversion.ToDouble(reader["YearObDebit"]),
				AccountTitle = Conversion.ToString(reader["AccountTitle"]),
				Start_Period = Conversion.ToString(reader["Start_Period"]),
				End_Period = Conversion.ToString(reader["End_Period"]),
				FinancialYearCode = Conversion.ToString(reader["FinancialYearCode"])
			};
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
