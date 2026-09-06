using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class AccountNotes
{
	public static DataTable ReadByPLNote()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@AccountNotesType", "ReadAllMethodByPLNote"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountNotes>("SP_AccountNotes_ReadAllMethodBySPType", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByBSNote(int ClassId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@AccountNotesType", "ReadAllMethodByBSNote"));
			if (ClassId != 0)
			{
				p.Add(new SqlParameter("@AccountClassId", ClassId));
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountNotes>("SP_AccountNotes_ReadAllMethodBySPType", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int UpdateTitleById(Architecture.Model.Accounts.AccountNotes obj)
	{
		int Id = 0;
		DataTable dt = null;
		try
		{
			foreach (Architecture.Model.Accounts.AccountNotes item in obj.AccountNoteslist)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter("@Id", item.Id));
				p.Add(new SqlParameter("@Note", item.NoteTitle));
				p.Add(new SqlParameter("@AccountNotesType", "UpdateTitleById"));
				dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountNotes>("SP_AccountNotes_ReadAllMethodBySPType", p);
			}
			if (dt.Rows.Count > 0)
			{
				Id = Conversion.ToInt(dt.Rows[0][0]);
			}
			return Id;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAllNotes(string Note = "")
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@AccountNotesType", "ReadAllNotes"));
			if (Note != null && Note != "")
			{
				p.Add(new SqlParameter("@Note", Note));
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountNotes>("SP_AccountNotes_ReadAllMethodBySPType", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
