using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model;
using Architecture.Model.Export;

namespace Architecture.BLL.Export;

public class ExImEFormRegistration
{
	public static int Save(Architecture.Model.Export.ExImEFormRegistration obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.PostDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Export.ExImEFormRegistration().SetDate(obj, "Sp_ExImEFormRegistration_Insert");
			}
			return new Architecture.DAL.Export.ExImEFormRegistration().SetDate(obj, "Sp_ExImEFormRegistration_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetData(Architecture.Model.Export.ExImEFormRegistration obj)
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
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImEFormRegistration>("Sp_ExImEFormRegistration_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Export.ExImEFormRegistration obj)
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
				ParameterName = "@Activity",
				Value = "GenerateDocNoCompanyIdOrganizationId"
			});
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImEFormRegistration>("Sp_ExImEFormRegistration_GetAllMethod", p).Rows[0]["DocNo"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ExImEFormRegistration GetByID(int Id)
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
			new List<Architecture.Model.Export.ExImEFormRegistration>();
			return new Architecture.DAL.Export.ExImEFormRegistration().GetDate("Sp_ExImEFormRegistration_GetAllMethod", p)[0];
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
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
				});
			}
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImEFormRegistration>("Sp_ExImEFormRegistration_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FIAdvanceBalanceSummary(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.BankId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BankId",
					Value = obj.BankId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate ",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			if (obj.SkipZero != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipZero",
					Value = obj.SkipZero
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImEFormRegistration>("usp_FinancialInstrumentAdvanceBalanceSummary", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetFINoForInvoice(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.RecId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.RecId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImEFormRegistration>("usp_GetFINoForInvoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetFINoForCustomInvoice(int OrgId, int CompId, int RecId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrgId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompId
			});
			if (RecId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = RecId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImEFormRegistration>("usp_GetFINoForCustomInvoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static decimal GetFinancialInstrumentsBalance(ReportsParameters obj)
	{
		DataTable dt = null;
		try
		{
			decimal Balance = default(decimal);
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetFinancialInstrumentsBalance"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImEFormRegistration>("Sp_ExImEFormRegistration_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				return Conversion.ToDecimal(dt.Rows[0][0]);
			}
			return Balance;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
