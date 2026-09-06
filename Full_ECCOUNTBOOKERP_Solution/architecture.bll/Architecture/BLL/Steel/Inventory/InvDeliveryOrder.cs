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

namespace Architecture.BLL.Steel.Inventory;

public class InvDeliveryOrder
{
	public static int Save(Architecture.Model.Steel.Inventory.InvDeliveryOrder obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				if (obj.InvDeliveryOrderDetail.Exists((InvDeliveryOrderDetail p) => p.ActionTypeId != 1))
				{
					throw new Exception("Record cannot be inserted because ActionTypeId greater than 1");
				}
				obj.ActionId = 1;
				return new Architecture.DAL.Steel.Inventory.InvDeliveryOrder().SetData(obj, "ST.USP_InvDeliveryOrder_Insert");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.Steel.Inventory.InvDeliveryOrder().SetData(obj, "ST.USP_InvDeliveryOrder_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Steel.Inventory.InvDeliveryOrder GetByID(int Id)
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
			new List<Architecture.Model.Steel.Inventory.InvDeliveryOrder>();
			return new Architecture.DAL.Steel.Inventory.InvDeliveryOrder().GetData("[ST].[USP_DeliveryOrder_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCodeForSteel(Architecture.Model.Steel.Inventory.InvDeliveryOrder obj)
	{
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Steel.Inventory.SaleOrder>("[ST].[USP_DeliveryOrder_GetAllMethod]", p);
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

	public static int ReadIdByDocNoForSteel(Architecture.Model.Steel.Inventory.InvDeliveryOrder obj)
	{
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
				ParameterName = "@DocNo",
				Value = obj.DocNo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadIdByDocNo"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Steel.Inventory.SaleOrder>("[ST].[USP_DeliveryOrder_GetAllMethod]", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				code = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryForSteel(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
					ParameterName = "@EntryUserId",
					Value = obj.EntryUser
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("[ST].[USP_DeliveryOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrderSlipForSteel(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("[ST].[USP_DeliveryOrder_SlipReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetAvailableStockForDOForSteel(ReportsParameters obj)
	{
		try
		{
			double AvailableStock = 0.0;
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
				ParameterName = "@DocDateTo",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@WareHouseId",
				Value = obj.WarehouseId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@JobLotId",
				Value = obj.JobLotId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@InvPackingTypeId",
				Value = obj.PackingTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemUomId",
				Value = obj.stockUOM
			});
			DataTable dt = GenericProvider.GetDataTableProc<Item>("[ST].[USP_GetAvailableStockForDO]", p);
			if (dt.Rows.Count > 0)
			{
				AvailableStock = Conversion.ToDouble(dt.Rows[0]["AvailableStock"]);
			}
			return AvailableStock;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int DeliveryOrderNoCheckinGatePassForSteel(Architecture.Model.Inventory.InvDeliveryOrder obj)
	{
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
				ParameterName = "@DocNo",
				Value = obj.DocNo
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeliveryOrderNoCheckinGatePass"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("[ST].[USP_DeliveryOrder_GetAllMethod]", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				code = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetTotalWeightFromDeliveryOrderForSteel(Architecture.Model.Inventory.InvDeliveryOrder obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetTotalWeightFromDeliveryOrder"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("[ST].[USP_DeliveryOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double OrderBalanceWeightforSteel(ReportsParameters obj)
	{
		double BalWeight = 0.0;
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
			p.Add(new SqlParameter
			{
				ParameterName = "@SaleOrderDetailId",
				Value = obj.OrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDoBalWeightByOrder"
			});
			DataTable dt = GenericProvider.GetDataTableProc<DeliverySchedule>("[ST].[USP_DeliveryOrder_GetAllMethod]", p);
			if (dt.Rows.Count > 0)
			{
				BalWeight = Conversion.ToDouble(dt.Rows[0]["BalWeight"]);
			}
			return BalWeight;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrderLoad(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeliveryOrderLoad"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("[ST].[USP_DeliveryOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CustomerLoadForDeliveryOrderId(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "CustomerLoadForDeliveryOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("[ST].[USP_DeliveryOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromDeliveryOrder(ReportsParameters obj)
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
			if (obj.Activity != string.Empty || obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[ST].[USP_GetDataForDropDownFromDeliveryOrder]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrderRegister(ReportsParameters obj)
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			if (obj.ReportTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DeliveryOrderType",
					Value = obj.ReportTypeId
				});
			}
			if (obj.PendingForView != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PendingForView",
					Value = obj.PendingForView
				});
			}
			return GenericProvider.GetDataTableProc<DeliverySchedule>("[ST].[USp_DeliveryOrderRegister]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateDeliveryOrderStatusById(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedUserId",
					Value = item.EntryUser
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ReqType",
					Value = item.ReqType
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = item.Id
				});
				GenericProvider.GetDataTableProc<Architecture.Model.Steel.Inventory.InvDeliveryOrder>("[ST].[USp_UpdateDeliveryOrderStatusById]", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
