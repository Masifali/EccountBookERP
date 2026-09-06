using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL;
using Architecture.Model;

namespace Architecture.BLL;

public class BankBalanceManual
{
	public static int Save(Architecture.Model.BankBalanceManual obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.BankBalanceManual().SetData(obj, "USP_AcBankBalanceManualEntry_Insert");
			}
			return new Architecture.DAL.BankBalanceManual().SetData(obj, "USP_AcBankBalanceManualEntry_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.BankBalanceManual> GetAllMethod(Architecture.Model.BankBalanceManual obj)
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
			if (obj.ChartOfAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ChartOfAccountId",
					Value = obj.ChartOfAccountId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.BalanceDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceDate",
					Value = obj.BalanceDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.BalanceTime))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BalanceTime",
					Value = obj.BalanceTime
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryDate",
					Value = obj.EntryDate
				});
			}
			if (obj.BySrource != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BySrource",
					Value = obj.BySrource
				});
			}
			if (obj.BySourceDescription == null || obj.BySourceDescription == string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BySourceDescription",
					Value = obj.BySourceDescription
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			new List<Architecture.Model.BankBalanceManual>();
			return new Architecture.DAL.BankBalanceManual().GetAll("Sp_AcBankBalanceManualEntry_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
