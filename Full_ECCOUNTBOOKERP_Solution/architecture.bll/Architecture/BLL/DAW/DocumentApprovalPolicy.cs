using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.DAW;
using Architecture.Model;
using Architecture.Model.DAW;

namespace Architecture.BLL.DAW;

public class DocumentApprovalPolicy
{
	public static long Save(Architecture.Model.DAW.DocumentApprovalPolicy obj)
	{
		try
		{
			if (obj.Id == 0L)
			{
				return new Architecture.DAL.DAW.DocumentApprovalPolicy().SetData(obj, "DAW.USp_DocumentApprovalPolicy_Insert");
			}
			return new Architecture.DAL.DAW.DocumentApprovalPolicy().SetData(obj, "DAW.USp_DocumentApprovalPolicy_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.DAW.DocumentApprovalPolicy obj)
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
				ParameterName = "@BranchId",
				Value = obj.BranchId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.DAW.DocumentApprovalLevel>("DAW.USp_DocumentApprovalPolicy_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.DAW.DocumentApprovalPolicy GetByID(int Id)
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
			new List<Architecture.Model.DAW.DocumentApprovalPolicy>();
			return new Architecture.DAL.DAW.DocumentApprovalPolicy().GetAll("DAW.USp_DocumentApprovalPolicy_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void MultiApproveAndRejectDocumentByUserId(ApprovalList obj)
	{
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				foreach (ApprovalList item in obj.AllApprovalLists)
				{
					new List<SqlParameter>();
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "USP_MultiApproveAndRejectDocumentByUserId";
					sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", item.DocumentTypeId);
					sqlCommand.Parameters.AddWithValue("@RefDocNoId", item.Id);
					sqlCommand.Parameters.AddWithValue("@UserId", item.EntryUser);
					sqlCommand.Parameters.AddWithValue("@ReqType", item.ReqType);
					sqlCommand.Parameters.AddWithValue("@Comments", item.Comments);
					sqlCommand.ExecuteNonQuery();
				}
				sqlTrn.Commit();
			}
			catch (Exception ex)
			{
				sqlTrn.Rollback();
				throw ex;
			}
		}
		catch (Exception ex2)
		{
			throw new Exception(ex2.Message);
		}
	}

	public static List<PolicyDocuments> GetDocumentsInPolicy(int OrganizationId, int CompanyId)
	{
		try
		{
			List<PolicyDocuments> lst = new List<PolicyDocuments>();
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
			DataTable dt = GenericProvider.GetDataTableProc<PolicyDocuments>("DAW.usp_getDocumentApprovalPolicy", p);
			if (dt.Rows.Count > 0)
			{
				lst = Conversion.ConvertDataTableToList<PolicyDocuments>(dt);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
