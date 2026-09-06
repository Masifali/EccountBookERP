using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.SystemUtilities;
using Architecture.Model;
using Architecture.Model.SystemUtilities;

namespace Architecture.BLL.SystemUtilities;

public class AttachmentsHistory
{
	public static int SaveList(Architecture.Model.SystemUtilities.AttachmentsHistory obj)
	{
		try
		{
			return new Architecture.DAL.SystemUtilities.AttachmentsHistory().SaveList(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAttachmentsHistoryByIdOfRequiredFilter(ReportsParameters obj)
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
				ParameterName = "@Activity",
				Value = "GetAttachmentsHistoryByIdOfRequiredFilter"
			});
			if (obj.CompanyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = obj.CompanyId
				});
			}
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReferenceDocumentTypeId",
					Value = obj.RefDocumentTypeId
				});
			}
			if (obj.RefRefDocIdNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReferenceDocumentId",
					Value = obj.RefRefDocIdNo
				});
			}
			if (obj.ScreenName != null && obj.ScreenName != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = obj.ScreenName
				});
			}
			if (obj.FolderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FolderId",
					Value = obj.FolderId
				});
			}
			if (obj.MinFileSize != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MinFileSize",
					Value = obj.MinFileSize
				});
			}
			if (obj.MaxFileSize != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MaxFileSize",
					Value = obj.MaxFileSize
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
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
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_AttachmentsHistory_GetAllMethods", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetTotalsInfoByOrganizationCompanyId(ReportsParameters obj)
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
				ParameterName = "@Activity",
				Value = "GetTotalsInfoByOrganizationCompanyId"
			});
			if (obj.CompanyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = obj.CompanyId
				});
			}
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_AttachmentsHistory_GetAllMethods", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
