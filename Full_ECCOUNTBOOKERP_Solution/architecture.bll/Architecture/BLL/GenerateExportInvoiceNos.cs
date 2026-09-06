using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class GenerateExportInvoiceNos
{
	public static int save(Architecture.Model.GenerateExportInvoiceNos ctry)
	{
		try
		{
			return new Architecture.DAL.GenerateExportInvoiceNos().SetData(ctry, "USP_ExportInvoiceNos_Insert");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll(Architecture.Model.GenerateExportInvoiceNos obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter("@DocumentTypeId", obj.DocumentTypeId));
			}
			p.Add(new SqlParameter("@Activity", "FormHistory"));
			return GenericProvider.GetDataTableProc<Architecture.Model.GenerateExportInvoiceNos>("[dbo].[USP_ExportInvoiceNos_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetFinalInvoiceNosForCommercialInvoice(Architecture.Model.GenerateExportInvoiceNos obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.PrefixTypeId != 0)
			{
				p.Add(new SqlParameter("@PrefixTypeId", obj.PrefixTypeId));
			}
			if (obj.FinalInvoiceNo != null && obj.FinalInvoiceNo != string.Empty)
			{
				p.Add(new SqlParameter("@InvoiceNo", obj.FinalInvoiceNo));
			}
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter("@DocumentTypeId", obj.DocumentTypeId));
			}
			p.Add(new SqlParameter("@Activity", "GetFinalInvoiceNosForCommercialInvoice"));
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.GenerateExportInvoiceNos>("[dbo].[USP_ExportInvoiceNos_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetFinalInvoiceNosForJobOrder(Architecture.Model.GenerateExportInvoiceNos obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@DocumentTypeId", obj.DocumentTypeId));
			if (obj.PrefixTypeId != 0)
			{
				p.Add(new SqlParameter("@PrefixTypeId", obj.PrefixTypeId));
			}
			if (obj.FinalInvoiceNo != null && obj.FinalInvoiceNo != string.Empty)
			{
				p.Add(new SqlParameter("@InvoiceNo", obj.FinalInvoiceNo));
			}
			p.Add(new SqlParameter("@Activity", "GetFinalInvoiceNosForJobOrder"));
			return GenericProvider.GetDataTableProc<Architecture.Model.GenerateExportInvoiceNos>("[dbo].[USP_ExportInvoiceNos_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetFinalInvoiceNosForExportContract(Architecture.Model.GenerateExportInvoiceNos obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@DocumentTypeId", obj.DocumentTypeId));
			if (obj.PrefixTypeId != 0)
			{
				p.Add(new SqlParameter("@PrefixTypeId", obj.PrefixTypeId));
			}
			if (obj.FinalInvoiceNo != null && obj.FinalInvoiceNo != string.Empty)
			{
				p.Add(new SqlParameter("@InvoiceNo", obj.FinalInvoiceNo));
			}
			p.Add(new SqlParameter("@Activity", "GetFinalInvoiceNosForExportContract"));
			return GenericProvider.GetDataTableProc<Architecture.Model.GenerateExportInvoiceNos>("[dbo].[USP_ExportInvoiceNos_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.GenerateExportInvoiceNos GetById(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			new List<Architecture.Model.GenerateExportInvoiceNos>();
			return new Architecture.DAL.GenerateExportInvoiceNos().GetAll("USP_ExportInvoiceNos_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static int GetMainId(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.GenerateExportInvoiceNos>("[dbo].[GetMainIdForGenratingExportInvoiceNos]", p).Rows[0]["MainId"]);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
