using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class InvInLandFreightAgreement
{
	public static int Save(Architecture.Model.Accounts.InvInLandFreightAgreement obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Accounts.InvInLandFreightAgreement().SetData(obj, "Sp_InvInLandFreightAgreement_Insert");
			}
			return new Architecture.DAL.Accounts.InvInLandFreightAgreement().SetData(obj, "Sp_InvInLandFreightAgreement_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Accounts.InvInLandFreightAgreement obj)
	{
		try
		{
			int code = 0;
			DataTable dt = new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.InvInLandFreightAgreement>("Sp_InvInLandFreightAgreement_GetAll", p);
			if (dt.Rows.Count > 0)
			{
				code = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Architecture.Model.Accounts.InvInLandFreightAgreement GetByID(int Id)
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
			new List<Architecture.Model.Accounts.InvInLandFreightAgreement>();
			return new Architecture.DAL.Accounts.InvInLandFreightAgreement().GetAll("Sp_InvInLandFreightAgreement_GetAll", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@organizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.InvInLandFreightAgreement>("Sp_InvInLandFreightAgreement_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
