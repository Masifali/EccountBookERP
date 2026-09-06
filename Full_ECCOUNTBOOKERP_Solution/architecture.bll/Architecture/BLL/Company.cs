using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Templates;

namespace Architecture.BLL;

public class Company
{
	public static int Save(Architecture.Model.Company obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Company().SetData(obj, "Proc_Company_Insert");
			}
			return new Architecture.DAL.Company().SetData(obj, "Proc_Company_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveAll(CompanyRegistration obj)
	{
		try
		{
			return new Architecture.DAL.Company().SetDataForAll(obj, "[dbo].[USP_NewCompanyRegister]");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAllCompaniesforCombo()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAllCompaniesforCombo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Company>("Sp_Company_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Company GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			new List<Architecture.Model.Company>();
			return new Architecture.DAL.Company().GetData("Proc_Company_ReadByID", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Company> GetAll(Architecture.Model.Company obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrgCompanyTypeId",
				Value = obj.OrgCompanyTypeId
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByOrganizationId"
			});
			return new Architecture.DAL.Company().GetData("Sp_Company_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAlldt(Architecture.Model.Company obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrgCompanyTypeId",
				Value = obj.OrgCompanyTypeId
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByOrganizationId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Company>("Sp_Company_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllCompanies()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Company>("Sp_Company_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CompanyHeaderSubReport(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Company>("Sp_CompanyHeaderSubReport", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCompaniesByTemplateId(int TemplateId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@TemplateId",
				Value = TemplateId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Company>("USP_Company_ByTemplateId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Company> GetHistory(Architecture.Model.Company obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrgCompanyTypeId",
				Value = obj.OrgCompanyTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetHistory"
			});
			return new Architecture.DAL.Company().GetData("Sp_Company_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(int OrgId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			if (OrgId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ORgCompanyTypeId",
					Value = OrgId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Company>("Sp_Company_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllScreensAgainstCompanyId(int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Company>("USP_getScreensFromUserRights", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
