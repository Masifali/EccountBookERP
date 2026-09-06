using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.DAW;
using Architecture.Model.DAW;

namespace Architecture.BLL.DAW;

public class DocumentApprovalDetail
{
	public static int Save(Architecture.Model.DAW.DocumentApprovalDetail obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.DAW.DocumentApprovalDetail().SetData(obj, "DAW.USp_DocumentApprovalDetail_Insert");
			}
			return new Architecture.DAL.DAW.DocumentApprovalDetail().SetData(obj, "DAW.USp_DocumentApprovalDetail_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(int OrganizationId, int CompanyId)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.DAW.DocumentApprovalDetail>("DAW.USP_DocumentApprovalDetail_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.DAW.DocumentApprovalDetail GetByID(int Id)
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
			new List<Architecture.Model.DAW.DocumentApprovalDetail>();
			return new Architecture.DAL.DAW.DocumentApprovalDetail().GetAll("DAW.USP_DocumentApprovalDetail_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool MultiApprovalDocumentRightCheckAgainstUser(int OrganizationId, int CompanyId, int UserId, int DocumentTypeId)
	{
		try
		{
			bool RightStatus = false;
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
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.DAW.DocumentApprovalDetail>("USP_MultiApprovalDocumentRightCheckagainstUser", p);
			if (dt.Rows.Count > 0)
			{
				RightStatus = Conversion.ToBool(dt.Rows[0][0]);
			}
			return RightStatus;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
