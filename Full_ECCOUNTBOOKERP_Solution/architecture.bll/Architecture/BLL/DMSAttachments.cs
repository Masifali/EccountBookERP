using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class DMSAttachments
{
	public static int Save(Architecture.Model.DMSAttachments obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new DMSAttachment().SetData(obj, "Proc_DMSAttachments_Insert");
			}
			return new DMSAttachment().SetData(obj, "Proc_DMSAttachments_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveList(Architecture.Model.DMSAttachments obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new DMSAttachment().SetDataList(obj, "Proc_DMSAttachments_Insert");
			}
			return new DMSAttachment().SetDataList(obj, "Proc_DMSAttachments_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.DMSAttachments> GetByID(int Id, string ScreenName)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ScreenName",
				Value = ScreenName
			});
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
			return new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.DMSAttachments> ReadAttachmentsbyRefDocumentTypeIdAndAccountId(int Id, int RefDocumentTypeId, int AccountId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeId",
				Value = RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AccountId",
				Value = AccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAttachmentsbyRefDocumentTypeIdAndAccountId"
			});
			return new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.DMSAttachments> ReadAttachmentsbyRefDocumentTypeId(int Id, int RefDocumentTypeId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeId",
				Value = RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAttachmentsbyRefDocumentTypeId"
			});
			return new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.DMSAttachments> ReadSingleOrMultipleAttachmentsByDocumentTypeId(int Id = 0, int RefDocumentTypeId = 0, string Ids = "")
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeId",
				Value = RefDocumentTypeId
			});
			if (Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = Id
				});
			}
			if (Ids != null && Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Ids",
					Value = Ids
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAttachmentsbyRefDocumentTypeId"
			});
			return new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.DMSAttachments> ReadSingleOrMultipleAttachmentsByScreenName(int Id = 0, string ScreenName = "", string Ids = "")
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ScreenName",
				Value = ScreenName
			});
			if (Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = Id
				});
			}
			if (Ids != null && Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Ids",
					Value = Ids
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			return new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void RemoveById(int Id, string ScreenName)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ScreenName",
				Value = ScreenName
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeleteById"
			});
			new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void RemoveByIdAndNames(int Id, string ScreenName, int RefDocumentTypeId = 0, int ActionId = 0)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommandInv = new SqlCommand
			{
				Connection = sqlcon,
				Transaction = sqlTrn,
				CommandType = CommandType.StoredProcedure,
				CommandText = "[dbo].[Sp_DMSAttachments_GetAllMethod]"
			};
			sqlCommandInv.Parameters.AddWithValue("@Id", Id);
			sqlCommandInv.Parameters.AddWithValue("@ScreenName", ScreenName);
			if (RefDocumentTypeId != 0)
			{
				sqlCommandInv.Parameters.AddWithValue("@RefDocumentTypeId", RefDocumentTypeId);
			}
			if (ActionId != 0)
			{
				sqlCommandInv.Parameters.AddWithValue("@ActionId", ActionId);
			}
			sqlCommandInv.Parameters.AddWithValue("@Activity", "RemoveByIdAndName");
			sqlCommandInv.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static void AttachmentDeleteByID(int AttachmentId)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "[Sp_DMSAttachments_GetAllMethod]";
			sqlCommand.Parameters.AddWithValue("@Id", AttachmentId);
			sqlCommand.Parameters.AddWithValue("@Activity", "AttachmentDeleteById");
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
