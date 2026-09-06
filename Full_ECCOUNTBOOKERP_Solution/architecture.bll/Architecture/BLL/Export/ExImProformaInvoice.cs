using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model.Export;

namespace Architecture.BLL.Export;

public class ExImProformaInvoice
{
	public static int Save(Architecture.Model.Export.ExImProformaInvoice obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Export.ExImProformaInvoice().SetDate(obj, "Sp_ExImProformaInvoice_Insert");
			}
			return new Architecture.DAL.Export.ExImProformaInvoice().SetDate(obj, "Sp_ExImProformaInvoice_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetData(Architecture.Model.Export.ExImProformaInvoice obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImProformaInvoice>("Sp_ExImProformaInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Export.ExImProformaInvoice obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateDocNoByDocumentTypeId"
			});
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImProformaInvoice>("Sp_ExImProformaInvoice_GetAllMethod", p).Rows[0]["DocNo"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ExImProformaInvoice GetByID(int Id)
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
			new List<Architecture.Model.Export.ExImProformaInvoice>();
			return new Architecture.DAL.Export.ExImProformaInvoice().GetDate("Sp_ExImProformaInvoice_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
