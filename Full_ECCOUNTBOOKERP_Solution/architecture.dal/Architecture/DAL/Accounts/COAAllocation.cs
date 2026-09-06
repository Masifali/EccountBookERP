using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Accounts;

public class COAAllocation : DALBase
{
	public int SetData(Architecture.Model.Accounts.COAAllocation obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			bool SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 4);
			foreach (Architecture.Model.Accounts.COAAllocation item in obj.COAAllocationlist)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
				DataTable dt = new DataTable();
				int AccountTypeId = 0;
				int CustomerGroupId = 0;
				using (SqlCommand sqlCommandPS = new SqlCommand
				{
					Connection = sqlcon,
					Transaction = sqlTrn,
					CommandType = CommandType.StoredProcedure,
					CommandText = "Sp_ChartofAccount_GetAllMethodFromCOA"
				})
				{
					sqlCommandPS.Parameters.AddWithValue("@OrganizationId", item.OrganizationId);
					sqlCommandPS.Parameters.AddWithValue("@CompanyId", item.CompanyId);
					sqlCommandPS.Parameters.AddWithValue("@Id", item.ChartofAccountId);
					sqlCommandPS.Parameters.AddWithValue("@CoaType", "GetAccountTypeId");
					new SqlDataAdapter(sqlCommandPS).Fill(dt);
					if (dt.Rows.Count > 0)
					{
						AccountTypeId = Conversion.ToInt(dt.Rows[0]["AccountTypeId"]);
						CustomerGroupId = Conversion.ToInt(dt.Rows[0]["CustomerGroupId"]);
					}
				}
				if (!SubsidiaryAccountAllownOnVouchers && (AccountTypeId == 3 || AccountTypeId == 22))
				{
					SupplierCustomer supplier = new SupplierCustomer();
					supplier.OrganizationId = item.OrganizationId;
					supplier.CompanyId = item.CompanyId;
					supplier.EntryUser = item.EntryUserId;
					supplier.ModifyUser = item.EntryUserId;
					supplier.EntryDate = DateTime.Now;
					supplier.ModifyDate = DateTime.Now;
					supplier.PostDate = DateTime.Now;
					supplier.PostState = false;
					supplier.GlAccountId = item.ChartofAccountId;
					supplier.AdvanceGlAcId = item.ChartofAccountId;
					supplier.MobilePersonal = "";
					supplier.CompanyName = item.AccountTitle;
					supplier.ReportingTitle = item.AccountTitle;
					supplier.CustomerGroupId = ((CustomerGroupId > 0) ? CustomerGroupId : ((AccountTypeId == 3) ? 1 : 7));
					supplier.CityId = 0;
					supplier.CNIC_EXPIRY_DATE = DateTime.Now;
					supplier.Status = true;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, supplier, "Sp_SupplierCustomer_Insert"));
				}
				Architecture.Model.Accounts.AccountsOpeningBalances balances = new Architecture.Model.Accounts.AccountsOpeningBalances();
				balances.OrganizationId = item.OrganizationId;
				balances.CompanyId = item.CompanyId;
				balances.EntryUser = item.EntryUserId;
				balances.ModifyUser = item.EntryUserId;
				balances.EntryDate = DateTime.Now;
				balances.ModifyDate = DateTime.Now;
				balances.PostDate = DateTime.Now;
				balances.PostState = false;
				balances.ChartOfAccountId = item.ChartofAccountId;
				balances.ChartOfAccountTitle = item.AccountTitle;
				balances.FinancialYearId = item.FinancialYearId;
				balances.YearObDebit = 0.0;
				balances.YearObCredit = 0.0;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, balances, "Sp_AccountsOpeningBalances_Insert"));
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

	public List<Architecture.Model.Accounts.COAAllocation> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.COAAllocation> lst = new List<Architecture.Model.Accounts.COAAllocation>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Accounts.COAAllocation>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public int UpdateForPageNo(Architecture.Model.Accounts.COAAllocation coaa)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_COAAllocation_UpdatePageNoandIsActive");
			sqlDataBase.AddInParameter(cmd, "ChartofAccountId", SqlDbType.Int, coaa.ChartofAccountId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, coaa.CompanyId);
			sqlDataBase.AddInParameter(cmd, "GLPAGENO", SqlDbType.NVarChar, coaa.GLPageNo);
			sqlDataBase.AddInParameter(cmd, "ISACTIVE", SqlDbType.Bit, coaa.IsActive);
			sqlDataBase.AddInParameter(cmd, "EntryUser", SqlDbType.Bit, coaa.BranchId);
			sqlDataBase.ExecuteNonQuery(cmd);
			return coaa.Id;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
