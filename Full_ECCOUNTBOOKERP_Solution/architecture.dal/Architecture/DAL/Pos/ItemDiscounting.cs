using System;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.POS;

namespace Architecture.DAL.Pos;

public class ItemDiscounting
{
	public int SetData(Architecture.Model.POS.ItemDiscounting obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.POS.ItemDiscounting item in obj.ItemDiscountingScheduleslist)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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
