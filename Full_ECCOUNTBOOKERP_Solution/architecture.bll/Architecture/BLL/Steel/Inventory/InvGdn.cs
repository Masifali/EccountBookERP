using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Steel.Inventory;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Steel.Inventory;
using Architecture.Model.pcc;

namespace Architecture.BLL.Steel.Inventory;

public class InvGdn
{
	public static int Save(Architecture.Model.Steel.Inventory.InvGdn obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				if (obj.invGdnDetail.Exists((Architecture.Model.Steel.Inventory.InvGdnDetail p) => p.ActionTypeId != 1))
				{
					throw new Exception("Record cannot be inserted because ActionTypeId greater than 1");
				}
				obj.ActionId = 1;
				return new Architecture.DAL.Steel.Inventory.InvGdn().SetData(obj, "ST.USP_InvGdn_Insert");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.Steel.Inventory.InvGdn().SetData(obj, "ST.USP_InvGdn_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Steel.Inventory.InvGdn ReadById(int Id)
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
			new List<Architecture.Model.Steel.Inventory.InvGdn>();
			return new Architecture.DAL.Steel.Inventory.InvGdn().GetData("ST.USP_InvGdn_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateInvGdnCode(Architecture.Model.Steel.Inventory.InvGdn obj)
	{
		DataTable dt = null;
		try
		{
			int Code = 0;
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.pcc.InvGdn>("ST.USP_InvGdn_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return Code;
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
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
					Value = obj.EntryUser
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
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
			if (!Conversion.CheckDateTimeNull(obj.EntryDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryFromDate",
					Value = obj.EntryDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryToDate",
					Value = obj.EntryDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyFromDate",
					Value = obj.ModifyDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyToDate",
					Value = obj.ModifyDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedFromDate",
					Value = obj.ApprovedDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedToDate",
					Value = obj.ApprovedDateTo
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoTo",
					Value = obj.ToDocNo
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("ST.USP_InvGdn_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int Id, int EntryUserId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Id", Id));
			p.Add(new SqlParameter("@EntryUserId", EntryUserId));
			p.Add(new SqlParameter("@Activity", "DeleteById"));
			GenericProvider.GetDataTableProc<Architecture.Model.Steel.Inventory.InvGrn>("ST.USP_InvGdn_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingGatepass(GatePassOutward obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Steel.Inventory.InvGrn>("ST.USP_GetOutstandingOutwardForDispatchNote", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByGpNoForGdnDeliveryOrder(GatePassOutward obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Steel.Inventory.InvGdn>("ST.USP_ReadByGpNoForGdnDeliveryOrder", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByGpNoForGdn(GatePassOutward obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			return GenericProvider.GetDataTableProc<GatePassOutward>("ST.USP_ReadByGpNoForGdn", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvGdnSlipAndRegister(ReportsParameters obj)
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
					ParameterName = "@GrnDateF",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GrnDateT",
					Value = obj.ToDate
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GrnNoF",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GrnNoT",
					Value = obj.ToDocNo
				});
			}
			if (obj.GpSrNoF != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpSrNoF",
					Value = obj.GpSrNoF
				});
			}
			if (obj.GpSrNoT != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpSrNoT",
					Value = obj.GpSrNoT
				});
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@gdnId",
					Value = obj.Id
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
				});
			}
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CityId",
					Value = obj.CityId
				});
			}
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("[ST].[USP_InvGdn_SlipAndRegisterReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetGdnGrossWeightForValidation(Architecture.Model.Steel.Inventory.InvGdn obj)
	{
		DataTable dt = null;
		try
		{
			double Code = 0.0;
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetGdnGrossWeightForValidation"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.pcc.InvGdn>("[ST].[USP_InvGdn_GetAllMethod]", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToDouble(dt.Rows[0]["GrossWeight"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromGdnForSteel(int OrganizationId, int CompanyId, string Activity = null)
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
			if (Activity != string.Empty || Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.InvGdn>("[ST].[USP_GetDataForDropDownFromGdnSteel]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
