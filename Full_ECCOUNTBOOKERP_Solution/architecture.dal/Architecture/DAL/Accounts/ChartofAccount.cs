using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Accounts;

public class ChartofAccount : DALBase
{
	public int SetData(Architecture.Model.Accounts.ChartofAccount obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			bool SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 4);
			bool BranchFeatureFinancial = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 17);
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
				if (obj.COAAllocationList != null && obj.COAAllocationList.Count > 0)
				{
					foreach (Architecture.Model.Accounts.COAAllocation item in obj.COAAllocationList)
					{
						item.ChartofAccountId = returnID;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_COAAllocation_Insert"));
						if (!SubsidiaryAccountAllownOnVouchers && obj.ActionId == 1 && (obj.AccountTypeId == 3 || (obj.AccountTypeId == 22 && obj.AccountGroup == "Detail")))
						{
							SupplierCustomer supplier = new SupplierCustomer();
							supplier.OrganizationId = obj.OrganizationId;
							supplier.CompanyId = item.CompanyId;
							supplier.EntryUser = obj.EntryUser;
							supplier.ModifyUser = obj.ModifyUser;
							supplier.EntryDate = DateTime.Now;
							supplier.ModifyDate = DateTime.Now;
							supplier.PostDate = DateTime.Now;
							supplier.PostState = false;
							supplier.GlAccountId = returnID;
							supplier.AdvanceGlAcId = returnID;
							supplier.MobilePersonal = obj.PhoneNumber;
							supplier.CompanyName = obj.AccountTitle;
							supplier.ReportingTitle = obj.AccountTitle;
							if (Conversion.ToInt(obj.CustomerGroupId) == 0)
							{
								throw new Exception("Supplier/Customer group field required");
							}
							supplier.CustomerGroupId = obj.CustomerGroupId;
							supplier.CityId = obj.CityId;
							supplier.CNIC_EXPIRY_DATE = DateTime.Now;
							supplier.Status = true;
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, supplier, "Sp_SupplierCustomer_Insert"));
						}
						if (obj.AccountGroup == "Detail")
						{
							Architecture.Model.Accounts.AccountsOpeningBalances balances = new Architecture.Model.Accounts.AccountsOpeningBalances();
							balances.OrganizationId = obj.OrganizationId;
							balances.CompanyId = item.CompanyId;
							balances.EntryUser = obj.EntryUser;
							balances.ModifyUser = obj.ModifyUser;
							balances.EntryDate = DateTime.Now;
							balances.ModifyDate = DateTime.Now;
							balances.PostDate = DateTime.Now;
							balances.PostState = false;
							balances.ChartOfAccountId = returnID;
							balances.ChartOfAccountTitle = obj.AccountTitle;
							balances.FinancialYearId = obj.FinancialYearId;
							balances.YearObDebit = (BranchFeatureFinancial ? 0.0 : obj.YearObDebit);
							balances.YearObCredit = (BranchFeatureFinancial ? 0.0 : obj.YearObCredit);
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, balances, "Sp_AccountsOpeningBalances_Insert"));
						}
					}
				}
			}
			else
			{
				returnID = obj.Id;
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

	public int SetDataList(Architecture.Model.Accounts.ChartofAccount obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			bool BranchFeatureFinancial = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 17);
			foreach (Architecture.Model.Accounts.ChartofAccount Coa in obj.ChartofAccountsList)
			{
				Coa.EntryDate = DateTime.Now;
				Coa.ModifyDate = DateTime.Now;
				Coa.PostDate = DateTime.Now;
				Coa.ActionId = 1;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, Coa, ProcName));
				if (returnID <= 0)
				{
					continue;
				}
				Coa.Id = Convert.ToInt32(returnID);
				if (Coa.COAAllocationList == null || Coa.COAAllocationList.Count <= 0)
				{
					continue;
				}
				foreach (Architecture.Model.Accounts.COAAllocation item in Coa.COAAllocationList)
				{
					item.ChartofAccountId = returnID;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_COAAllocation_Insert"));
					if (Coa.ActionId == 1)
					{
						if (Coa.AccountTypeId == 3 && Coa.AccountGroup == "Detail")
						{
							SupplierCustomer supplier2 = new SupplierCustomer();
							supplier2.OrganizationId = Coa.OrganizationId;
							supplier2.CompanyId = item.CompanyId;
							supplier2.EntryUser = Coa.EntryUser;
							supplier2.ModifyUser = Coa.ModifyUser;
							supplier2.EntryDate = DateTime.Now;
							supplier2.ModifyDate = DateTime.Now;
							supplier2.PostDate = DateTime.Now;
							supplier2.PostState = false;
							supplier2.GlAccountId = returnID;
							supplier2.AdvanceGlAcId = returnID;
							supplier2.MobilePersonal = Coa.PhoneNumber;
							supplier2.CompanyName = Coa.AccountTitle;
							supplier2.ReportingTitle = Coa.AccountTitle;
							supplier2.CustomerGroupId = 1;
							supplier2.CityId = Coa.CityId;
							supplier2.CNIC_EXPIRY_DATE = DateTime.Now;
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, supplier2, "Sp_SupplierCustomer_Insert"));
						}
						if (Coa.AccountTypeId == 22 && Coa.AccountGroup == "Detail")
						{
							SupplierCustomer supplier = new SupplierCustomer();
							supplier.OrganizationId = Coa.OrganizationId;
							supplier.CompanyId = item.CompanyId;
							supplier.EntryUser = Coa.EntryUser;
							supplier.ModifyUser = Coa.ModifyUser;
							supplier.EntryDate = DateTime.Now;
							supplier.ModifyDate = DateTime.Now;
							supplier.PostDate = DateTime.Now;
							supplier.PostState = false;
							supplier.GlAccountId = returnID;
							supplier.AdvanceGlAcId = returnID;
							supplier.MobilePersonal = Coa.PhoneNumber;
							supplier.CompanyName = Coa.AccountTitle;
							supplier.ReportingTitle = Coa.AccountTitle;
							supplier.CustomerGroupId = 7;
							supplier.CityId = Coa.CityId;
							supplier.CNIC_EXPIRY_DATE = DateTime.Now;
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, supplier, "Sp_SupplierCustomer_Insert"));
						}
					}
					if (Coa.AccountGroup == "Detail")
					{
						Architecture.Model.Accounts.AccountsOpeningBalances balances = new Architecture.Model.Accounts.AccountsOpeningBalances();
						balances.OrganizationId = Coa.OrganizationId;
						balances.CompanyId = item.CompanyId;
						balances.EntryUser = Coa.EntryUser;
						balances.ModifyUser = Coa.ModifyUser;
						balances.EntryDate = DateTime.Now;
						balances.ModifyDate = DateTime.Now;
						balances.PostDate = DateTime.Now;
						balances.PostState = false;
						balances.ChartOfAccountId = returnID;
						balances.ChartOfAccountTitle = Coa.AccountTitle;
						balances.FinancialYearId = Coa.FinancialYearId;
						balances.YearObDebit = (BranchFeatureFinancial ? 0.0 : Coa.YearObDebit);
						balances.YearObCredit = (BranchFeatureFinancial ? 0.0 : Coa.YearObCredit);
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, balances, "Sp_AccountsOpeningBalances_Insert"));
					}
				}
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

	public Architecture.Model.Accounts.ChartofAccount ReadByID(int Id)
	{
		Architecture.Model.Accounts.ChartofAccount chartofAccount = null;
		IDataReader reader = null;
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("SP_ChartofAccount_ReadByID");
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.Int, Id);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				chartofAccount = FillDTO(reader);
				if (chartofAccount.ParentAccountCode == "0")
				{
					chartofAccount.AccountDisplay = "";
				}
				else
				{
					chartofAccount.ParentAccountCodeTitle = Conversion.ToString(reader["ParentAccountCodeTitle"]);
					chartofAccount.AccountDisplay = chartofAccount.ParentAccountCode + " - " + chartofAccount.ParentAccountCodeTitle;
				}
				chartofAccount.YearObDebit = 0.0;
				chartofAccount.YearObCredit = 0.0;
				if (chartofAccount.AccountGroup == "Detail")
				{
					Architecture.Model.Accounts.AccountsOpeningBalances OpeningBalances = new AccountsOpeningBalances().ReadByChartOfAccountId(chartofAccount.Id);
					if (OpeningBalances != null)
					{
						chartofAccount.YearObDebit = OpeningBalances.YearObDebit;
						chartofAccount.YearObCredit = OpeningBalances.YearObCredit;
					}
				}
			}
			return chartofAccount;
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

	private Architecture.Model.Accounts.ChartofAccount FillDTO(IDataReader reader)
	{
		try
		{
			Architecture.Model.Accounts.ChartofAccount chartofAccount = new Architecture.Model.Accounts.ChartofAccount();
			chartofAccount.Id = Conversion.ToInt(reader["Id"]);
			chartofAccount.AccountCode = Conversion.ToString(reader["AccountCode"]);
			chartofAccount.AccountTitle = Conversion.ToString(reader["AccountTitle"]);
			chartofAccount.AccountTitleOtherLingo = Conversion.ToString(reader["AccountTitleOtherLingo"]);
			if (chartofAccount.AccountClassEnum == EnumAccountClass.Capital)
			{
				chartofAccount.AccountClass = 1;
			}
			if (chartofAccount.AccountClassEnum == EnumAccountClass.Assets)
			{
				chartofAccount.AccountClass = 2;
			}
			if (chartofAccount.AccountClassEnum == EnumAccountClass.Liabilities)
			{
				chartofAccount.AccountClass = 3;
			}
			if (chartofAccount.AccountClassEnum == EnumAccountClass.Expenses)
			{
				chartofAccount.AccountClass = 4;
			}
			if (chartofAccount.AccountClassEnum == EnumAccountClass.Revenue)
			{
				chartofAccount.AccountClass = 5;
			}
			chartofAccount.AccountTypeId = Conversion.ToInt(reader["AccountTypeId"]);
			chartofAccount.AccountGroup = Conversion.ToString(reader["AccountGroup"]);
			chartofAccount.Account_Level = Conversion.ToInt(reader["Account_Level"]);
			chartofAccount.PLNoteId = Conversion.ToInt(reader["PLNoteId"]);
			chartofAccount.BSNoteId = Conversion.ToInt(reader["BSNoteId"]);
			chartofAccount.ParentAccountCode = Conversion.ToString(reader["ParentAccountCode"]);
			chartofAccount.OtherErpCode = Conversion.ToString(reader["OtherErpCode"]);
			chartofAccount.QrCode = Conversion.ToString(reader["QrCode"]);
			chartofAccount.FinancialYearId = Conversion.ToInt(reader["FinancialYearId"]);
			chartofAccount.IsActive = Conversion.ToBool(reader["IsActive"]);
			chartofAccount.EntryDate = Conversion.ToDateTime(reader["EntryDate"]);
			chartofAccount.EntryUser = Conversion.ToInt(reader["EntryUser"]);
			chartofAccount.ModifyDate = Conversion.ToDateTime(reader["ModifyDate"]);
			chartofAccount.ModifyUser = Conversion.ToInt(reader["ModifyUser"]);
			chartofAccount.PostUser = Conversion.ToInt(reader["PostUser"]);
			chartofAccount.PostState = Conversion.ToBool(reader["PostState"]);
			chartofAccount.OrganizationId = Conversion.ToInt(reader["OrganizationId"]);
			chartofAccount.CompanyId = Conversion.ToInt(reader["CompanyId"]);
			chartofAccount.CurrencyId = Conversion.ToInt(reader["CurrencyId"]);
			chartofAccount.ContactNo = Conversion.ToString(reader["ContactNo"]);
			return chartofAccount;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public int DELETEANDUPDATEBYACCOUNTID(ReportsParameters obj)
	{
		try
		{
			int Id = 0;
			if (obj.ReportsParameterslist != null && obj.ReportsParameterslist.Count > 0)
			{
				foreach (ReportsParameters item in obj.ReportsParameterslist)
				{
					DbCommand cmd = sqlDataBase.GetStoredProcCommand("SP_ChartOfAccount_DeleteAndUpdatebyAccountId");
					sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, item.OrganizationId);
					sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, item.CompanyId);
					sqlDataBase.AddInParameter(cmd, "ReqType", SqlDbType.NVarChar, item.ReqType);
					sqlDataBase.AddInParameter(cmd, "ChartOfAccountId", SqlDbType.Int, item.ChartOfAccountId);
					sqlDataBase.AddInParameter(cmd, "AccountTitle", SqlDbType.NVarChar, item.AccountTitle);
					sqlDataBase.AddInParameter(cmd, "OtherErpCode", SqlDbType.NVarChar, item.OtherCode);
					sqlDataBase.AddInParameter(cmd, "CurrencyId", SqlDbType.Int, item.CurrencyId);
					sqlDataBase.AddInParameter(cmd, "IsActive", SqlDbType.Bit, item.IsApproved);
					sqlDataBase.AddInParameter(cmd, "EntryUser", SqlDbType.Int, item.EntryUser);
					if (item.ContactNo != null && item.ContactNo != string.Empty)
					{
						sqlDataBase.AddInParameter(cmd, "ContactNo", SqlDbType.NVarChar, item.ContactNo);
					}
					Id = sqlDataBase.ExecuteNonQuery(cmd);
				}
			}
			else
			{
				DbCommand cmd2 = sqlDataBase.GetStoredProcCommand("SP_ChartOfAccount_DeleteAndUpdatebyAccountId");
				sqlDataBase.AddInParameter(cmd2, "OrganizationId", SqlDbType.Int, obj.OrganizationId);
				sqlDataBase.AddInParameter(cmd2, "CompanyId", SqlDbType.Int, obj.CompanyId);
				sqlDataBase.AddInParameter(cmd2, "ReqType", SqlDbType.NVarChar, obj.ReqType);
				sqlDataBase.AddInParameter(cmd2, "ChartOfAccountId", SqlDbType.Int, obj.ChartOfAccountId);
				sqlDataBase.AddInParameter(cmd2, "AccountTitle", SqlDbType.NVarChar, obj.AccountTitle);
				sqlDataBase.AddInParameter(cmd2, "OtherErpCode", SqlDbType.NVarChar, obj.OtherCode);
				sqlDataBase.AddInParameter(cmd2, "CurrencyId", SqlDbType.Int, obj.CurrencyId);
				sqlDataBase.AddInParameter(cmd2, "IsActive", SqlDbType.Bit, obj.IsApproved);
				sqlDataBase.AddInParameter(cmd2, "EntryUser", SqlDbType.Int, obj.EntryUser);
				if (obj.ContactNo != null && obj.ContactNo != string.Empty)
				{
					sqlDataBase.AddInParameter(cmd2, "ContactNo", SqlDbType.NVarChar, obj.ContactNo);
				}
				Id = sqlDataBase.ExecuteNonQuery(cmd2);
			}
			return Id;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
