using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.ContractorWages;
using Architecture.Model;
using Architecture.Model.ContractorWages;
using Architecture.Model.pcc;

namespace Architecture.BLL.ContractorWages;

public class InvContractorWagesSchedule
{
	public static int Save(Architecture.Model.ContractorWages.InvContractorWagesSchedule obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.ContractorWages.InvContractorWagesSchedule().SetDate(obj, "Sp_InvContractorWagesSchedule_Insert");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.ContractorWages.InvContractorWagesSchedule().SetDate(obj, "Sp_InvContractorWagesSchedule_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet GetAll(Architecture.Model.ContractorWages.InvContractorWagesSchedule obj)
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
			if (!Conversion.CheckDateTimeNull(obj.EffectedDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EffectedDate",
					Value = obj.EffectedDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EffectedDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EffectedDateTo",
					Value = obj.EffectedDateTo
				});
			}
			if (obj.InvConractorWagesAccountsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvConractorWagesAccountsId",
					Value = obj.InvConractorWagesAccountsId
				});
			}
			if (obj.ContractorId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractorId",
					Value = obj.ContractorId
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataSetProc<Architecture.Model.ContractorWages.InvContractorWagesSchedule>("Sp_InvContractorWagesSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.ContractorWages.InvContractorWagesSchedule GetByID(int Id)
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
			new List<Architecture.Model.ContractorWages.InvContractorWagesSchedule>();
			return new Architecture.DAL.ContractorWages.InvContractorWagesSchedule().GetData("Sp_InvContractorWagesSchedule_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvContractorWagesSchedule_Validations(Architecture.Model.ContractorWages.InvContractorWagesSchedule obj)
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
				ParameterName = "@WagesAccountId",
				Value = obj.InvConractorWagesAccountsId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectedDateFrom",
				Value = obj.EffectedDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FromPackSize",
				Value = obj.PackUomFrom
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToPackSize",
				Value = obj.PackUomTo
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesSchedule>("SpInvContractorWagesSchedule_Validations", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static GetWagesSchedule GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize(ReportsParameters obj)
	{
		try
		{
			GetWagesSchedule d = new GetWagesSchedule();
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
				ParameterName = "@InvConractorWagesAccountsId",
				Value = obj.InvConractorWagesAccountsId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ContractorId",
				Value = obj.ContractorId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectedDate",
				Value = obj.EffectedDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PackUomFrom",
				Value = obj.PackUomFrom
			});
			if (obj.PackUomTo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackUomTo",
					Value = obj.PackUomTo
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesSchedule>("Sp_InvContractorWagesSchedule_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				d.WagesRate = Conversion.ToDouble(dt.Rows[0]["WageRate"]);
				d.ScheduleId = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			return d;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool CheckItemsFreeofcostforWages(ReportsParameters obj)
	{
		try
		{
			bool IsFoc = false;
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeId",
				Value = obj.RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@WagesAccountId",
				Value = obj.AccountId
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesSchedule>("USP_CheckItemsFreeofcostforWages", p);
			if (dt.Rows.Count > 0)
			{
				IsFoc = Conversion.ToBool(dt.Rows[0]["IsFreeocCost"]);
			}
			return IsFoc;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DropDownFillFromWagesSchedule(ReportsParameters obj)
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<InvSaleInvoice>("[dbo].[USP_GetDataForDropDownFromWagesSchedule]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ApproveUnApprove(ApprovalList obj)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "[dbo].[Sp_InvContractorWagesSchedule_GetAllMethod]";
				sqlCommand.Parameters.AddWithValue("@Id", item.Id);
				sqlCommand.Parameters.AddWithValue("@ReqType", item.ReqType);
				sqlCommand.Parameters.AddWithValue("@CompanyId", item.CompanyId);
				sqlCommand.Parameters.AddWithValue("@EntryUserId", item.EntryUser);
				sqlCommand.Parameters.AddWithValue("@OrganizationId", item.OrganizationId);
				sqlCommand.Parameters.AddWithValue("@Activity", "ApproveUnApprove");
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
}
