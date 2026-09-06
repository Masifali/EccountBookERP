using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.PurchaseTrading;
using Architecture.Model;
using Architecture.Model.PurchaseTrading;

namespace Architecture.BLL.PurchaseTrading;

public class SupplySchedule
{
	public static int Save(Architecture.Model.PurchaseTrading.SupplySchedule obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.PurchaseTrading.SupplySchedule().SetData(obj, "Sp_SupplySchedule_Insert");
			}
			return new Architecture.DAL.PurchaseTrading.SupplySchedule().SetData(obj, "Sp_SupplySchedule_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GenerateCode(Architecture.Model.PurchaseTrading.SupplySchedule obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateSupplyScheduleCode"
			});
			return Conversion.ToString(GenericProvider.GetDataTableProc<Architecture.Model.PurchaseTrading.SupplySchedule>("Sp_SupplySchedule_GetAllMethod", p).Rows[0]["DocNo"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll()
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PurchaseTrading.SupplySchedule>("Sp_SupplySchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.PurchaseTrading.SupplySchedule GetByID(int Id)
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
				Value = "ReadByID"
			});
			new List<Architecture.Model.PurchaseTrading.SupplySchedule>();
			return new Architecture.DAL.PurchaseTrading.SupplySchedule().GetData("Sp_SupplySchedule_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GetIdbyDocNo(Architecture.Model.PurchaseTrading.SupplySchedule obj)
	{
		DataTable dt = null;
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
			p.Add(new SqlParameter
			{
				ParameterName = "@ThisDocumentTypeId",
				Value = obj.ThisDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocNo",
				Value = obj.DocNo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadDocNobyId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.PurchaseTrading.SupplySchedule>("Sp_SupplySchedule_GetAllMethod", p);
			string Code = null;
			if (dt.Rows.Count > 0)
			{
				Code = dt.Rows[0]["Id"].ToString();
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplyScheduleDetailForGRN(ReportsParameters obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupCustId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.DocNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNo",
					Value = obj.DocNo
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
			p.Add(new SqlParameter
			{
				ParameterName = "@LoadingType",
				Value = obj.ApprovedFilter
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSupplyScheduledetailforGRN"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PurchaseTrading.SupplySchedule>("Sp_SupplySchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadItemandQty(Architecture.Model.PurchaseTrading.SupplySchedule obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@ThisDocumentTypeId",
				Value = obj.ThisDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSupplyScheduledetailforGRN"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PurchaseTrading.SupplySchedule>("Sp_SupplySchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
