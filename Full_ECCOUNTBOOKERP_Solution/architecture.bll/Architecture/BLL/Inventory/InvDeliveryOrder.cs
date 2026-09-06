using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class InvDeliveryOrder
{
	public static int Save(Architecture.Model.Inventory.InvDeliveryOrder obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				if (obj.InvDeliveryOrderDetailList.Exists((InvDeliveryOrderdetail p) => p.ActionTypeId != 1))
				{
					throw new Exception("Record cannot be inserted because ActionTypeId not equal to 1");
				}
				obj.ActionId = 1;
				return new Architecture.DAL.Inventory.InvDeliveryOrder().SetData(obj, "Sp_InvDeliveryOrder_Insert");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.Inventory.InvDeliveryOrder().SetData(obj, "Sp_InvDeliveryOrder_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvDeliveryOrder GetByID(int Id)
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
			new List<Architecture.Model.Inventory.InvDeliveryOrder>();
			return new Architecture.DAL.Inventory.InvDeliveryOrder().GetData("Sp_InvDeliveryOrder_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Inventory.InvDeliveryOrder obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_InvDeliveryOrder_GetAllMethod", p);
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

	public static int ReadIdByDocNo(Architecture.Model.Inventory.InvDeliveryOrder obj)
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
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_InvDeliveryOrder_GetAllMethod", p);
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

	public static int DeliveryOrderNoCheckinGatePass(Architecture.Model.Inventory.InvDeliveryOrder obj)
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
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_InvDeliveryOrder_GetAllMethod", p);
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

	public static DataTable GetTotalWeightFromDeliveryOrder(Architecture.Model.Inventory.InvDeliveryOrder obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_InvDeliveryOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvDeliveryOrder_GetSaleOrderWithPaymentTerm(Architecture.Model.Inventory.InvDeliveryOrder obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("[dbo].[USP_InvDeliveryOrder_GetSaleOrderWithPaymentTerm]", p);
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("Sp_InvDeliveryOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryNew(ReportsParameters obj)
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
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
				});
			}
			if (obj.Activity != string.Empty && obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DeliveryOrderType",
					Value = obj.Activity
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DeliveryTypeId",
					Value = obj.ActionId
				});
			}
			if (obj.RequestedById != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RequestedById",
					Value = obj.RequestedById
				});
			}
			if (obj.ApprovedById != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedById",
					Value = obj.ApprovedById
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("SP_DeliveryOrderFormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryForPackingMaterial(ReportsParameters obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistoryForPackingMaterial"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("Sp_InvDeliveryOrder_GetAllMethod", p);
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("Sp_InvDeliveryOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrder_GetDataForGdnPP(int OrganizationId, int CompanyId, int FinancialYearId, int BranchesId, int StockPartyId, int GpId)
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
				ParameterName = "@FinancialYearId",
				Value = FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@StockPartyId",
				Value = StockPartyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@GpId",
				Value = GpId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("[dbo].[USP_InvDeliveryOrder_GetDataForGdnPP]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrderLoadForExportGdn(ReportsParameters obj)
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
				Value = "DeliveryOrderLoadForExportGdn"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("Sp_InvDeliveryOrder_GetAllMethod", p);
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("Sp_InvDeliveryOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CustomerLoadForExportForwardingByGpId(ReportsParameters obj)
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
				Value = "CustomerLoadForExportForwardingByGpId"
			});
			return GenericProvider.GetDataTableProc<ExImForwarding>("Sp_InvDeliveryOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingDeliveryOrderForIssuance(ReportsParameters obj)
	{
		try
		{
			new DataTable();
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
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseDemandHeader>("[dbo].[USP_GetPendingDeliveryOrderPmForIssuance]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static void ApprovedRecordByDeliveryOrderId(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = item.OrganizationId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = item.CompanyId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = item.EntryUser
				});
				if (item.ReqType != null && item.ReqType != string.Empty)
				{
					p.Add(new SqlParameter
					{
						ParameterName = "@ReqType",
						Value = item.ReqType
					});
				}
				if (item.ScreenName != null && item.ScreenName != string.Empty)
				{
					p.Add(new SqlParameter
					{
						ParameterName = "@ScreenName",
						Value = item.ScreenName
					});
				}
				if (item.Id != 0)
				{
					p.Add(new SqlParameter
					{
						ParameterName = "@Id",
						Value = item.Id
					});
				}
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "ApprovedRecordByDeliveryOrderId"
				});
				GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("Sp_InvDeliveryOrder_GetAllMethod", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet GetDeliveryOrderForApproval(ReportsParameters obj)
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
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			if (obj.Activity != string.Empty && obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DeliveryOrderType",
					Value = obj.Activity
				});
			}
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			if (obj.ExImInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImInvoiceId",
					Value = obj.ExImInvoiceId
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WbWeightStatus",
					Value = obj.ActionId
				});
			}
			if (obj.SkipZero != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DoReferedStatus",
					Value = obj.SkipZero
				});
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = obj.PageSize
				});
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = obj.PageNumber
				});
			}
			return GenericProvider.GetDataSetProc<Architecture.Model.Inventory.DeliverySchedule>("Sp_InvDeliveryOrderForApproval_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadIdByDeliveryOrderId(Architecture.Model.Inventory.InvDeliveryOrder obj)
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
				Value = "ReadIdByDeliveryOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_InvDeliveryOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportFormHistoryNew(ReportsParameters obj)
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
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.ExImInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvoiceId",
					Value = obj.ExImInvoiceId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("[dbo].[USP_DeliveryOrderExport_FormHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportFormHistory(ReportsParameters obj)
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
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = obj.PageNumber
				});
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = obj.PageSize
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ExportFormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("Sp_InvDeliveryOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TotalReceivedQtyByDeliveryOrderStockTransfer(ReportsParameters obj)
	{
		new DataTable();
		List<SqlParameter> p = new List<SqlParameter>();
		p.Add(new SqlParameter
		{
			ParameterName = "@PurchaseOrderId",
			Value = obj.OrderId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@Organizationid",
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
			Value = "TotalReceivedQtyByDeliveryOrderStockTransfer"
		});
		return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.GatePassGeneral>("Sp_InvDeliveryOrder_GetAllMethod", p);
	}

	public static DataTable DeliveryOrderDetailForExportGdnForwarding(ReportsParameters obj)
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
				Value = obj.ExImInvoiceId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@GpId",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeliveryOrderDetailForExportGdnForwarding"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_InvDeliveryOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrderDetailForGatePassOutward(ReportsParameters obj)
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
			if (Conversion.ToInt(obj.BranchesId) != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (obj.DocumentTypeIds != string.Empty && obj.DocumentTypeIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
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
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
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
				Value = "DeliveryOrderDetailForGatePassOutward"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.DeliverySchedule>("Sp_InvDeliveryOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetAvailableStockForDeliveryOrder(ReportsParameters obj)
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
				ParameterName = "@CropYear",
				Value = obj.CropYear
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
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.Item>("USP_GetAvailableStockForDeliveryOrder", p);
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

	public static double OrderBalanceWeight(ReportsParameters obj)
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
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.DeliverySchedule>("Sp_InvDeliveryOrder_GetAllMethod", p);
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.DocumentTypeIds != string.Empty && obj.DocumentTypeIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.BranchesIds != string.Empty && obj.BranchesIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			if (obj.Activity != string.Empty && obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			if (obj.ReqType != string.Empty && obj.ReqType != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DeliveryOrderType",
					Value = obj.ReqType
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("USP_GetDataForDropDownFromDeliveryOrder", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string DeliveryOrderStackWarningMessage(Architecture.Model.Inventory.InvDeliveryOrder obj)
	{
		try
		{
			string WarningMessage = "";
			DataTable dt = new DataTable();
			List<SqlParameter> s = new List<SqlParameter>();
			for (int i = 0; i < obj.InvDeliveryOrderDetailList.Count; i++)
			{
				s.Clear();
				s.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = obj.OrganizationId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = obj.CompanyId
				});
				if (obj.InvDeliveryOrderDetailList[i].ItemId != 0)
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@ItemId",
						Value = obj.InvDeliveryOrderDetailList[i].ItemId
					});
				}
				if (obj.InvDeliveryOrderDetailList[i].WarehouseId != 0)
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@WarehouseId",
						Value = obj.InvDeliveryOrderDetailList[i].WarehouseId
					});
				}
				if (obj.InvDeliveryOrderDetailList[i].JobLotId != 0)
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@JobLotId",
						Value = obj.InvDeliveryOrderDetailList[i].JobLotId
					});
				}
				if (obj.InvDeliveryOrderDetailList[i].InvPackingTypeId != 0)
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@InvPackingTypeId",
						Value = obj.InvDeliveryOrderDetailList[i].InvPackingTypeId
					});
				}
				if (obj.InvDeliveryOrderDetailList[i].PackUomId != 0)
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@PackUomId",
						Value = obj.InvDeliveryOrderDetailList[i].PackUomId
					});
				}
				if (!Conversion.CheckDateTimeNull(obj.DocDate))
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@DoDate",
						Value = obj.DocDate
					});
				}
				if (obj.InvDeliveryOrderDetailList[i].CropYear != null && obj.InvDeliveryOrderDetailList[i].CropYear != "")
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@CropYear",
						Value = obj.InvDeliveryOrderDetailList[i].CropYear
					});
				}
				s.Add(new SqlParameter
				{
					ParameterName = "@NetWeight",
					Value = obj.InvDeliveryOrderDetailList[i].DoWeight
				});
				dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("USP_DeliveryOrderStackWarningMessage", s);
				if (dt.Rows.Count > 0)
				{
					WarningMessage = Conversion.ToString(dt.Rows[0]["WarningMessage"]);
					break;
				}
			}
			return WarningMessage;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static double GetBalRetainBagQtyAgainstSupplierAndItem(ReportsParameters obj)
	{
		double RetainBalQty = 0.0;
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
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.DeliverySchedule>("[dbo].[USp_GetBalRetainBagQtyAgainstSupplierAndItem]", p);
			if (dt.Rows.Count > 0)
			{
				RetainBalQty = Conversion.ToDouble(dt.Rows[0]["RetainBalQty"]);
			}
			return RetainBalQty;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsByDeliveryOrderId(int OrganizationId, int CompanyId, int DeliveryOrderId, int GpId = 0)
	{
		try
		{
			if (DeliveryOrderId == 0 && GpId == 0)
			{
				throw new Exception("Delivery OrderId Or Gate Pass OutwardId not found");
			}
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
			if (DeliveryOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = DeliveryOrderId
				});
			}
			if (GpId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpId",
					Value = GpId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ItemsByDeliveryOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("Sp_InvDeliveryOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getInvoiceNoFromDeliveryOrderByGpId(int GpId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GpId",
				Value = GpId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("usp_getInvoiceNoFromDeliveryOrderByGpId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPartiesAndItemsFromDeliveryOrderByGpId(int GpId, int RecId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GpId",
				Value = GpId
			});
			if (RecId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "RecId",
					Value = RecId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("[dbo].[usp_getPartiesAndItemsFromDeliveryOrderByGpId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrderDetailForGatePassOutward_Engr(ReportsParameters obj)
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
			if (obj.DocumentTypeIds != string.Empty || obj.DocumentTypeIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.DeliverySchedule>("[dbo].[USP_SaleOrderDetailForGatePassOutward_Engr]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrderDetailForGatePassOutward_Engr(ReportsParameters obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			if (obj.DocumentTypeIds != string.Empty || obj.DocumentTypeIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.DeliverySchedule>("[dbo].[USP_DeliveryOrderDetailForGatePassOutward_Engr]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvDeliveryOrderSlip_Engr(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("USP_DeliveryOrder_Slip_Engr", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrderForApproval_Engr(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("USP_InvDeliveryOrderForApproval_Engr", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CustomerLoadForDeliveryOrderId_Engr(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("USP-CustomerLoadForDeliveryOrderId_Engr", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrderDataForGdn_Engr(ReportsParameters obj)
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
				ParameterName = "@DeliveryTypeId",
				Value = obj.DeliveryTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("USP_DeliveryOrderDataForGdn", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrder_PendingDataForGdn_Engr(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.DeliveryTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DeliveryTypeId",
					Value = obj.DeliveryTypeId
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("[dbo].[USP_DeliveryOrder_PendingDataForGdn_Engr]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingCustomerAndOrderForGDN(int OrganizationId, int CompanyId, int DoId, int RecId = 0)
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
				ParameterName = "@DoId",
				Value = DoId
			});
			if (RecId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = RecId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[USP_GetPendingCustomerAndOrderForGDN]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDeliveryOrderDataByPartyDoAndOrderId(int OrganizationId, int CompanyId, int SupplierCustomerId, int DoId)
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
				ParameterName = "@SupplierCustomerId",
				Value = SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DoId",
				Value = DoId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[USP_GetDeliveryOrderDataByPartyDoAndOrderId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable OutstandingDeliveryorderForDispatch(int OrganizationId, int CompanyId, int BranchesId, int DocumentTypeId)
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
				ParameterName = "@BranchesId",
				Value = BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[USP_OutstandingDeliveryorderForDispatch]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrder_GetAdvanceDo(int OrganizationId, int CompanyId, int BranchesId, int FinancialYearId, int SupplierCustomerId, int GpRecId = 0, int GdnRecId = 0)
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
				ParameterName = "@FinancialYearId",
				Value = FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = BranchesId
			});
			if (SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = SupplierCustomerId
				});
			}
			if (GpRecId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpRecId",
					Value = GpRecId
				});
			}
			if (GdnRecId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GdnRecId",
					Value = GdnRecId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[USP_DeliveryOrder_GetAdvanceDo]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrderRegister_MfgEng(ReportsParameters obj)
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesIds",
					Value = obj.InventoryParentCategories
				});
			}
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.ItemClassGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemClassGroupId",
					Value = obj.ItemClassGroupId
				});
			}
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
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
			if (obj.CastingTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CastingTypeId",
					Value = obj.CastingTypeId
				});
			}
			if (obj.VarientId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VariantId",
					Value = obj.VarientId
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
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.OrderNoFrom != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderNoFrom",
					Value = obj.OrderNoFrom
				});
			}
			if (obj.OrderNoTo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderNoTo",
					Value = obj.OrderNoTo
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsRefered",
					Value = obj.ActionId
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGdn>("[Mfg].[USP_DeliveryOrderRegister_Eng]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrderExortSlip_ByInvoiceId(int OrganizationId, int CompanyId, int DocumentTypeId, int EximInvoiceId)
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
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = EximInvoiceId
			});
			return GenericProvider.GetDataTableProc<ExImLcOrder>("Sp_InvDeliveryOrder_ExportSlipByInvoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AdvanceDeliveryOrderRegister(ReportsParameters obj)
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
				ParameterName = "@FromDate",
				Value = obj.FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			if (obj.SkipZero != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipZero",
					Value = obj.SkipZero
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.DeliverySchedule>("usp_AdvanceDeliveryOrderRegister", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
