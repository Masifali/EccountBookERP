using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class VouchersApprovedByEditorRemarks
{
	public static int Save(Architecture.Model.Accounts.VouchersApprovedByEditorRemarks obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Accounts.VouchersApprovedByEditorRemarks().SetDate(obj, "Sp_VouchersApprovedByEditorRemarks_Insert");
			}
			return new Architecture.DAL.Accounts.VouchersApprovedByEditorRemarks().SetDate(obj, "Sp_VouchersApprovedByEditorRemarks_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable HistoryByUserAndDocumentId(Architecture.Model.Accounts.VouchersApprovedByEditorRemarks obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocMasterRecordId",
				Value = obj.RefDocMasterRecordId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CommentsUserId",
				Value = obj.CommentsUserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "HistoryByUserAndDocumentId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader>("Sp_VouchersApprovedByEditorRemarks_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string ReadAsSingleString(Architecture.Model.Accounts.VouchersApprovedByEditorRemarks obj)
	{
		DataTable dt = null;
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocMasterRecordId",
				Value = obj.RefDocMasterRecordId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocTypeId",
				Value = obj.RefDocTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAsSingleString"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader>("Sp_VouchersApprovedByEditorRemarks_ReadAll", p);
			string Notes = "";
			if (dt.Rows.Count > 0)
			{
				Notes = Conversion.ToString(dt.Rows[0]["Notes"]);
			}
			return Notes;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
