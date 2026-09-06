using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Templates;

namespace Architecture.DAL.Templates;

public class OrganizationTemplateRights
{
	public int SetData(Architecture.Model.Templates.OrganizationTemplateRights obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			string getXml = "";
			if (obj.OrgTemplatesRightsList.Count > 0)
			{
				getXml = Conversion.ConvertListToXmlSerializer(obj.OrgTemplatesRightsList.ToList());
			}
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = ProcName;
			sqlCommand.Parameters.AddWithValue("@FIFOXML", getXml);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Templates.OrganizationTemplateRights> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Templates.OrganizationTemplateRights> lst = new List<Architecture.Model.Templates.OrganizationTemplateRights>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Templates.OrganizationTemplateRights>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
