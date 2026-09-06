using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.pcc;
using Architecture.Model;
using Architecture.Model.pcc;

namespace Architecture.BLL.pcc;

public class ContractorWagesRateSchedule
{
	public static int Save(Architecture.Model.pcc.ContractorWagesRateSchedule obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.pcc.ContractorWagesRateSchedule().SetData(obj, "[pcc].[USP_ContractorWagesRateSchedule_Insert]");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.pcc.ContractorWagesRateSchedule().SetData(obj, "[pcc].[USP_ContractorWagesRateSchedule_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveList(Architecture.Model.pcc.ContractorWagesRateSchedule obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.pcc.ContractorWagesRateSchedule().SetDataList(obj, "[pcc].[USP_ContractorWagesRateSchedule_Insert]");
			}
			return new Architecture.DAL.pcc.ContractorWagesRateSchedule().SetDataList(obj, "[pcc].[USP_ContractorWagesRateSchedule_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.pcc.ContractorWagesRateSchedule ReadById(int Id)
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
			new List<Architecture.Model.pcc.ContractorWagesRateSchedule>();
			return new Architecture.DAL.pcc.ContractorWagesRateSchedule().GetData("[pcc].[USP_ContractorWagesRateSchedule_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(ReportsParameters obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			if (obj.InvConractorWagesAccountsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractorWagesGroupId",
					Value = obj.InvConractorWagesAccountsId
				});
			}
			if (obj.ContractorWagesActivityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractorWagesActivityId",
					Value = obj.ContractorWagesActivityId
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EffectedDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.EffectedDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EffectedDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.EffectedDateTo
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ContractorWagesRateSchedule>("[pcc].[USP_ContractorWagesRateSchedule_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromWagesSchedule(ReportsParameters obj)
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
			if (obj.ActivityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.ActivityId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ContractorWagesRateSchedule>("[pcc].[USP_GetDataForDropDownFromWagesSchedule]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetServiceActivityByItemId(int RefDocumentTypeId, int ItemId, DateTime EffectedDATE, int ContractorWagesActivityId = 0, int ContractorId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (ContractorWagesActivityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractorWagesActivityId",
					Value = ContractorWagesActivityId
				});
			}
			if (ContractorId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractorId",
					Value = ContractorId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeId",
				Value = RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectedDATE",
				Value = EffectedDATE
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetServiceActivityByItemId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ContractorWagesRateSchedule>("[pcc].[USP_ContractorWagesRateSchedule_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetServiceActivityByItemIdOrWagesGroupId(int ItemId, int WagesGroupId, DateTime EffectedDATE, int ContractorWagesActivityId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (ContractorWagesActivityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractorWagesActivityId",
					Value = ContractorWagesActivityId
				});
			}
			if (WagesGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WagesGroupId",
					Value = WagesGroupId
				});
			}
			if (ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = ItemId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectedDATE",
				Value = EffectedDATE
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ContractorWagesRateSchedule>("[pcc].[USP_GetServiceActivityByItemIdOrWagesGroupId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int EntryUserId, int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUserId",
				Value = EntryUserId
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
			GenericProvider.GetDataTableProc<Architecture.Model.pcc.ContractorWagesRateSchedule>("[pcc].[USP_ContractorWagesRateSchedule_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
