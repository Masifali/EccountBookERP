using System;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.SystemUtilities;

namespace Architecture.DAL.SystemUtilities;

public class AttachmentsHistory
{
	public int SaveList(Architecture.Model.SystemUtilities.AttachmentsHistory obj)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.SystemUtilities.AttachmentsHistory item in obj.AttachmentHistoryList)
			{
				returnID = ((item.Id != 0) ? Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_AttachmentsHistory_Update")) : Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_AttachmentsHistory_Insert")));
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
