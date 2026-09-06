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

public class ContractorWagesActivity
{
	public static int Save(Architecture.Model.pcc.ContractorWagesActivity obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.pcc.ContractorWagesActivity().SetData(obj, "[pcc].[USP_ContractorWagesActivity_Insert]");
			}
			return new Architecture.DAL.pcc.ContractorWagesActivity().SetData(obj, "[pcc].[USP_ContractorWagesActivity_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.pcc.ContractorWagesActivity ReadById(int Id)
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
			new List<Architecture.Model.pcc.ContractorWagesActivity>();
			return new Architecture.DAL.pcc.ContractorWagesActivity().GetData("[pcc].[USP_ContractorWagesActivity_GetAllMethod]", p)[0];
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
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ContractorWagesActivity>("[pcc].[USP_ContractorWagesActivity_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int Id)
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
				Value = "DeleteById"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.pcc.ContractorWagesActivity>("[pcc].[USP_ContractorWagesActivity_GetAllMethod]", p);
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = obj.Activity
			});
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsFromWages",
					Value = true
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[pcc].[USP_GetDataForDropDownFromWagesSchedule]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WagesActivity(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDocNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDocNo",
					Value = obj.ToDocNo
				});
			}
			if (obj.OtherDocNoId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoId",
					Value = obj.OtherDocNoId
				});
			}
			if (obj.RefRefDocIdNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocNoId",
					Value = obj.RefRefDocIdNo
				});
			}
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
				});
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
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
			if (obj.ContractorId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractorId",
					Value = obj.ContractorId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@ActivityName",
				Value = obj.Activity
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("[pcc].[USP_WagesReportWithActivities]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExtraWagesActivitiesCombo(int OrganizationId, int CompanyId)
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
				Value = "ExtraWagesActivitiesCombo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ContractorWagesActivity>("[pcc].[USP_ContractorWagesActivity_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
