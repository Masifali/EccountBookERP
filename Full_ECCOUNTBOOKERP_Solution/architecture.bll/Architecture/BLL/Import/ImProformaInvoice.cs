using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Import;
using Architecture.Model.Import;

namespace Architecture.BLL.Import;

public class ImProformaInvoice
{
	public static int Save(Architecture.Model.Import.ImProformaInvoice obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Import.ImProformaInvoice().SetDate(obj, "Sp_ImProformaInvoice_Insert");
			}
			return new Architecture.DAL.Import.ImProformaInvoice().SetDate(obj, "Sp_ImProformaInvoice_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetData(Architecture.Model.Import.ImProformaInvoice obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImProformaInvoice>("Sp_ImProformaInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Import.ImProformaInvoice obj)
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
				Value = "GenerateDocNoByDocumentTypeId"
			});
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.Import.ImProformaInvoice>("Sp_ImProformaInvoice_GetAllMethod", p).Rows[0]["DocNo"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Import.ImProformaInvoice GetByID(int Id)
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
			new List<Architecture.Model.Import.ImProformaInvoice>();
			return new Architecture.DAL.Import.ImProformaInvoice().GetDate("Sp_ImProformaInvoice_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
