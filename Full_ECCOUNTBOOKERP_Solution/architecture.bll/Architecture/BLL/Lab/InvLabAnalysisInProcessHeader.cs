using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Lab;
using Architecture.Model;
using Architecture.Model.Lab;

namespace Architecture.BLL.Lab;

public class InvLabAnalysisInProcessHeader
{
	public static int Save(Architecture.Model.Lab.InvLabAnalysisInProcessHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Lab.InvLabAnalysisInProcessHeader().SetData(obj, "Sp_InvLabAnalysisInProcessHeader_Insert");
			}
			return new Architecture.DAL.Lab.InvLabAnalysisInProcessHeader().SetData(obj, "Sp_InvLabAnalysisInProcessHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Lab.InvLabAnalysisInProcessHeader obj)
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
				Value = "GenerateDocNo"
			});
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisInProcessHeader>("USp_InvLabAnalysisInProcessHeader_GetAllMethod", p).Rows[0]["DocNo"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = clsGlobalVariables.UserAccount.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = clsGlobalVariables.UserAccount.CompanyId
			});
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = clsGlobalVariables.UserAccount.ID
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisInProcessHeader>("USp_InvLabAnalysisInProcessHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetById(int Id)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisInProcessHeader>("USp_InvLabAnalysisInProcessHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PlantFillByDate(Architecture.Model.Lab.InvLabAnalysisInProcessHeader obj)
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
				Value = "GetPlantFromInProcessAnalysis"
			});
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDate",
					Value = obj.DocDate
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisInProcessHeader>("USp_InvLabAnalysisInProcessHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemFillByPlant(Architecture.Model.Lab.InvLabAnalysisInProcessHeader obj)
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
				Value = "GetItemFromInProcessAnalysis"
			});
			if (obj.Plant != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.Plant
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisInProcessHeader>("USp_InvLabAnalysisInProcessHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InProcessAnalysisRecovery(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter("@PlantId", obj.PlantId));
			}
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter("@JobOrderId", obj.JobOrderId));
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter("@ItemId", obj.ItemId));
			}
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter("@DocDate", obj.DocDate));
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisInProcessHeader>("USP_InProcessAnalysisRecovery_Register", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InProcessStepAnalysisRegister(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter("@PlantId", obj.PlantId));
			}
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter("@JobOrderId", obj.JobOrderId));
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter("@ItemId", obj.ItemId));
			}
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter("@DocDate", obj.DocDate));
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisInProcessHeader>("USP_InProcessAnalysisRegister", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DateFill(ReportsParameters obj)
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter("@ItemId", obj.ItemId));
			}
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter("@PlantId", obj.PlantId));
			}
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter("@JobOrderId", obj.JobOrderId));
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDocDateFromInProcessAnalysis"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisInProcessHeader>("USp_InvLabAnalysisInProcessHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
