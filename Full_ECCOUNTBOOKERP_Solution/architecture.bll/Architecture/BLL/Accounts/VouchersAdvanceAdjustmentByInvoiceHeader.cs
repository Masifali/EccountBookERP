using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class VouchersAdvanceAdjustmentByInvoiceHeader
{
	public static int Save(Architecture.Model.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader().SetDate(obj, "Sp_VouchersAdvanceAdjustmentByInvoiceHeader_Insert");
			}
			return new Architecture.DAL.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader().SetDate(obj, "Sp_VouchersAdvanceAdjustmentByInvoiceHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			return new Architecture.DAL.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader().GetDate("Sp_VouchersAdvanceAdjustmentByInvoiceHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader obj)
	{
		DataTable dt = null;
		try
		{
			int code = 0;
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
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader>("Sp_VouchersAdvanceAdjustmentByInvoiceHeader_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				code = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
