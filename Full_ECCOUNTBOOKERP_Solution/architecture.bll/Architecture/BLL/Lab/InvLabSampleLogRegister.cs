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

public class InvLabSampleLogRegister
{
	public static int Save(Architecture.Model.Lab.InvLabSampleLogRegister obj)
	{
		try
		{
			obj.ModifyDate = DateTime.Now;
			obj.EntryDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Lab.InvLabSampleLogRegister().SetDate(obj, "Sp_InvLabSampleLogRegister_Insert");
			}
			return new Architecture.DAL.Lab.InvLabSampleLogRegister().SetDate(obj, "Sp_InvLabSampleLogRegister_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Lab.InvLabSampleLogRegister obj)
	{
		DataTable dt = null;
		try
		{
			int code = 0;
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleLogRegister>("Sp_InvLabSampleLogRegister_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				code = Conversion.ToInt(dt.Rows[0]["SampleNo"]);
			}
			return code;
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
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleLogRegister>("Sp_InvLabSampleLogRegister_GetAllMethod", p);
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleLogRegister>("Sp_InvLabSampleLogRegister_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetForCombo(Architecture.Model.Lab.InvLabSampleLogRegister obj)
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
				Value = "ReadForCombo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleLogRegister>("Sp_InvLabSampleLogRegister_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPrintSlipAndReport(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromSampleNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToSampleNo",
					Value = obj.ToDocNo
				});
			}
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
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProjectsId",
					Value = obj.ProjectsId
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.RefRefDocIdNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReferencePartyId",
					Value = obj.RefRefDocIdNo
				});
			}
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CityAreaId",
					Value = obj.CityId
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
			if (obj.CropYear != null && obj.CropYear != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
				});
			}
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleLogRegister>("Sp_InvLabSampleLogRegister_RiceSlipAndRegister_Rep", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
