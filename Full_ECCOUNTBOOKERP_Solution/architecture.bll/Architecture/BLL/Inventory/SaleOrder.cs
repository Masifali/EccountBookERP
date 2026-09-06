using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class SaleOrder
{
	public static int Save(Architecture.Model.Inventory.SaleOrder obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				if (obj.SaleOrderDetailList.Exists((SaleOrderDetail p) => p.ActionTypeId != 1))
				{
					throw new Exception("Record cannot be inserted because ActionTypeId greater than 1");
				}
				obj.ActionId = 1;
				return new Architecture.DAL.Inventory.SaleOrder().SetData(obj, "Sp_SaleOrder_Insert");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.Inventory.SaleOrder().SetData(obj, "Sp_SaleOrder_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaleOrderDetailUpdate(Architecture.Model.Inventory.SaleOrder obj)
	{
		try
		{
			if (obj.Id > 0)
			{
				return new Architecture.DAL.Inventory.SaleOrder().SetDataDetail(obj, "Sp_SaleOrderDetail_Update");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.SaleOrder GetByID(int Id)
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
			new List<Architecture.Model.Inventory.SaleOrder>();
			return new Architecture.DAL.Inventory.SaleOrder().GetData("Sp_SaleOrder_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByPurchaseOrderIDNOrderItemId(Architecture.Model.Inventory.SaleOrder obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@SaleOrderId",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadBySaleOrderIDNOrderItemId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGpTypeandDEliveryTerm(Architecture.Model.Inventory.SaleOrder obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadFromSaleOrderBySaleOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierBySaleOrderNo(Architecture.Model.Inventory.SaleOrder obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSupplierBySaleOrderNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GeneratePurchaseOrderCodeByDocId(Architecture.Model.Inventory.SaleOrder obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateSaleOrderCodeByDocId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
			if (dt.Rows.Count > 0)
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

	public static int GenerateSaleOrderBranchCodeByDocId(Architecture.Model.Inventory.SaleOrder obj)
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
				Value = "GenerateSaleOrderBranchCodeByDocId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Code = Conversion.ToInt(dt.Rows[0]["BranchSrNo"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetHisoty(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDateTo",
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
					ParameterName = "@PoSrFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PoSrTo",
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
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			if (obj.BookingPersonId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BookingPersonId",
					Value = obj.BookingPersonId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "SaleOrderFormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetHisotyDetail(int Id)
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
				Value = "SaleOrderFormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSaleOrderNoinInvoice(Architecture.Model.Inventory.SaleOrder obj)
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
				ParameterName = "@Activity",
				Value = "GetSaleOrderNotInInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int ReadSaleOrderIdbyDocNo(Architecture.Model.Inventory.SaleOrder obj)
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
				Value = "GetSaleIdByDocNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
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

	public static DataSet SaleOrderHistory(ReportsParameters obj)
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
				ParameterName = "@AppId",
				Value = obj.AppId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CustomerUserId",
				Value = obj.EntryUser
			});
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
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
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesId",
					Value = obj.InventoryParentCategories
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
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
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CityId",
					Value = obj.CityId
				});
			}
			if (obj.RefPartyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReferencePartyId",
					Value = obj.RefPartyId
				});
			}
			if (obj.ExpiryDays != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExpiryDays",
					Value = obj.ExpiryDays
				});
			}
			if (obj.Status != null && obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = obj.Status
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
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			if (obj.BookingPersonId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BookingPersonId",
					Value = obj.BookingPersonId
				});
			}
			new DataSet();
			return GenericProvider.GetDataSetProc<Architecture.Model.Inventory.SaleOrder>("Sp_SalesSaleOrder_RiceAndPaddyRegister_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable SaleOrderTradingFormHistory(ReportsParameters obj)
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
				Value = "SaleOrderTradingFormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static void UpdateStatusandIsApprovedbyOrderId(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				if (item.ReqType != string.Empty)
				{
					p.Add(new SqlParameter
					{
						ParameterName = "@ReqType",
						Value = item.ReqType
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
					ParameterName = "@PostUser",
					Value = item.EntryUser
				});
				if (item.Comments != null && item.Comments != string.Empty)
				{
					p.Add(new SqlParameter
					{
						ParameterName = "@OrderStatusRemarks",
						Value = item.Comments
					});
				}
				if (!Conversion.CheckDateTimeNull(item.PostDate))
				{
					p.Add(new SqlParameter
					{
						ParameterName = "@DocDate",
						Value = item.PostDate
					});
				}
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "UpdateStatusandIsApprovedbyOrderId"
				});
				GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetSaleOrderDetailForDeliverySchedule(ReportsParameters obj)
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
			if (obj.DocNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNo",
					Value = obj.DocNo
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderSupCustId",
					Value = obj.SupplierCustomerId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDateTo",
					Value = obj.ToDate
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSaleOrderdetailforDeliverySchedule"
			});
			return GenericProvider.GetDataTableProc<SaleOrderDetail>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrderIdandNoGetForSupplierCustomerId(Architecture.Model.Inventory.SaleOrder obj)
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
				ParameterName = "@OrderSupCustId",
				Value = obj.OrderSupCustId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.Id
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "SaleOrderIdandNoGetForSupplierCustomerId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable OrderIdandNoForIssuanceGdnBySupplierCustomerId(Architecture.Model.Inventory.SaleOrder obj)
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
				ParameterName = "@OrderSupCustId",
				Value = obj.OrderSupCustId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (obj.Id > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.Id
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "OrderIdandNoForIssuanceGdnBySupplierCustomerId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable LoadSaleOrderForDeliveryOrder(ReportsParameters obj)
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
					ParameterName = "@DocDateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDateTo",
					Value = obj.ToDate
				});
			}
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("USP_GetPendingSaleOrderByDoAndGdn", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LoadSaleOrderForDeliveryOrderInDetail(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = obj.GdnIds
			});
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("USP_LoadSaleOrderForDeliveryOrderInDetail", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrderCustomerExpensesByOrderIds(string OrderIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrderIds",
				Value = OrderIds
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "SaleOrderCustomerExpensesByOrderIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LoadSaleOrderForGoodsDispatchNotes(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
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
				Value = "LoadSaleOrderForGoodsDispatchNotes"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSaleOrderNo(Architecture.Model.Inventory.SaleOrder obj)
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
			if (obj.OrderSupCustId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderSupCustId",
					Value = obj.OrderSupCustId
				});
			}
			if (obj.CurrencyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.CurrencyId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadSaleOrderNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetOrderDetailRecordByOrderId(Architecture.Model.Inventory.SaleOrder obj)
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
				ParameterName = "@SaleOrderId",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetOrderDetailRecordByOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrderLoadForSaleInvoice(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderSupCustId",
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
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDateTo",
					Value = obj.ToDate
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSaleOrderDetailForTraderProLoader"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double CHECKSUPPLIERCUSTOMERLIMITS(ReportsParameters obj)
	{
		try
		{
			double AvailableLimit = 0.0;
			List<SqlParameter> s = new List<SqlParameter>();
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
			s.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@Amount",
				Value = obj.Amount
			});
			if (obj.ActionId != 0)
			{
				s.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("SP_CHECKSUPPLIERCUSTOMERLIMITS", s);
			if (dt.Rows.Count > 0)
			{
				AvailableLimit = Conversion.ToDouble(dt.Rows[0]["AvailableLimit"]);
			}
			return AvailableLimit;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSaleOrderDetailForBookingInvoice(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderSupCustId",
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
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDateTo",
					Value = obj.ToDate
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSaleOrderDetailForBookingInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetOutstandingOrdersWithLedgerBalance(ReportsParameters obj)
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
				ParameterName = "@ActionId",
				Value = obj.ActionId
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("USP_GetOutstandingOrdersWithLedgerBalance", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromSaleOrder(ReportsParameters obj)
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
				ParameterName = "@AppId",
				Value = obj.AppId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
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
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("USP_GetDataForDropDownFromSaleOrder", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingSaleOrderDetailForDeliveryOrder(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCategoryId",
					Value = obj.ParentCategoryId
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
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Ids",
					Value = obj.Ids
				});
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
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
			if (obj.OrderDetailIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderDetailIds",
					Value = obj.OrderDetailIds
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
			if (obj.SortType != null && obj.SortType != "")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SortType",
					Value = obj.SortType
				});
			}
			if (obj.SortingCol != null && obj.SortingCol != "")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SortingCol",
					Value = obj.SortingCol
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("[USP_GetPendingSaleOrderDetailForDeliveryOrder]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrderSummaryRegister(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AppId",
				Value = obj.AppId
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
					ParameterName = "@DocNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderItemId",
					Value = obj.ItemId
				});
			}
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategories",
					Value = obj.ParentCategoryId
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderSupCustId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.PackSizeFrom != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackUom",
					Value = obj.PackSizeFrom
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
			if (obj.CropYear != null && obj.CropYear != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
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
			if (obj.DistrictId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DistrictId",
					Value = obj.DistrictId
				});
			}
			if (obj.PackingTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackingTypeId",
					Value = obj.PackingTypeId
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
			if (obj.RefPartyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReferencePartyId",
					Value = obj.RefPartyId
				});
			}
			if (obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActivityName",
					Value = obj.ReportType
				});
			}
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
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
			if (obj.IsOnQty != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsOnQty",
					Value = obj.IsOnQty
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
			if (!string.IsNullOrEmpty(obj.Status))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderStatus",
					Value = obj.Status
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
			if (obj.BookingPersonId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BookingPersonId",
					Value = obj.BookingPersonId
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[USP_SaleOrderSummaryRegister]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static string CheckOrderExist(Architecture.Model.Inventory.SaleOrder obj)
	{
		try
		{
			string IsExist = "";
			DataTable dt = new DataTable();
			List<SqlParameter> s = new List<SqlParameter>();
			for (int i = 0; i < obj.SaleOrderDetailList.Count; i++)
			{
				s.Clear();
				s.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = obj.OrganizationId
				});
				if (obj.Id != 0)
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@Id",
						Value = obj.Id
					});
				}
				s.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = obj.CompanyId
				});
				if (!Conversion.CheckDateTimeNull(obj.DocDate))
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@DocDate",
						Value = obj.DocDate
					});
				}
				if (obj.OrderSupCustId != 0)
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@OrderSupCustId",
						Value = obj.OrderSupCustId
					});
				}
				if (obj.SaleOrderDetailList[i].OrderItemId != 0)
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@OrderItemId",
						Value = obj.SaleOrderDetailList[i].OrderItemId
					});
				}
				s.Add(new SqlParameter
				{
					ParameterName = "@OrderItemQty",
					Value = obj.SaleOrderDetailList[i].OrderItemQty
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@OrderItemRate",
					Value = obj.SaleOrderDetailList[i].OrderItemRate
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@Amount",
					Value = obj.SaleOrderDetailList[i].Amount
				});
				dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("USP_SaleOrderValidations", s);
				if (dt.Rows.Count > 0)
				{
					IsExist = Conversion.ToString(dt.Rows[0]["WarningMessage"]);
					break;
				}
			}
			return IsExist;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static bool ItemExistsAgainstSaleOrderNo(int OrganizationId, int CompanyId, int ItemId, int OrderId)
	{
		bool Exists = false;
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
				ParameterName = "@Id",
				Value = OrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrderItemId",
				Value = ItemId
			});
			if (GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_ItemIdExistInSaleOrder]", p).Rows.Count > 0)
			{
				Exists = true;
			}
			return Exists;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LoadSaleOrderOnInvoiceForSalt(ReportsParameters obj)
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
					ParameterName = "@DocDateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDateTo",
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
					ParameterName = "@OrderSupCustId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Ids",
					Value = obj.Ids
				});
			}
			return GenericProvider.GetDataTableProc<PurchaseOrderDetail>("[dbo].[USP_LoadSaleOrderOnInvoiceForSalt]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LoadSaleOrderOnInvoiceForConcrete(ReportsParameters obj)
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
					ParameterName = "@Todate",
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
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
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
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesId",
					Value = obj.InventoryParentCategories
				});
			}
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Ids",
					Value = obj.Ids
				});
			}
			return GenericProvider.GetDataTableProc<PurchaseOrderDetail>("[pcc].[USP_LoadPendingSaleOrderOnInvoice]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsBySaleOrderId(int OrganizationId, int CompanyId, int SaleOrderId)
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
				ParameterName = "@Id",
				Value = SaleOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ItemsBySaleOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetOutstandOrdersAndPartiesForIssuanceGdn(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderSupCustId ",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId ",
					Value = obj.Id
				});
			}
			return GenericProvider.GetDataTableProc<PurchaseOrderDetail>("USP_GetOutstandOrdersAndPartiesForIssuanceGdn", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSaleOrderRemningAmountByOrderId(int OrganizationId, int CompanyId, int OrderId, int RecId = 0)
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
				ParameterName = "@OrderId ",
				Value = OrderId
			});
			if (RecId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId ",
					Value = RecId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("USP_GetSaleOrderRemningAmountByOrderId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSaleOrderStatus_Counters(ReportsParameters obj)
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
				ParameterName = "@ExpiryDays",
				Value = obj.ExpiryDays
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("USP_GetSaleOrderStatus_Counters", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrderHistoryForCustomer(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FromDate",
				Value = obj.FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Todate",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.Status != null && obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = obj.Status
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
			return GenericProvider.GetDataTableProc<PurchaseOrderDetail>("[dbo].[usp_SaleOrderHistoryForCustomer]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBranchesAllocatedToUserFromSaleOrder(int OrganizationId, int CompanyId, int UserId, int DocumentTypeId = 0)
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
				ParameterName = "@UserId",
				Value = UserId
			});
			if (DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = DocumentTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.BranchesAllocationToUser>("USP_GetBranchsAllocatedToUserFromSaleOrder", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrder_InfoByItemId(int OrganizationId, int CompanyId, int ItemId)
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
				ParameterName = "@ItemId",
				Value = ItemId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("USP_SaleOrder_InfoByItemId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPartyLimitAndOutstandingOrderBalance(int OrganizationId, int CompanyId, int SupplierCustomerId, int RecId = 0)
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
			if (RecId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = RecId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDeliveryOrder>("usp_getPartyLimitAndOutstandingOrderBalance", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SaleOrderAutoComplete(ReportsParameters obj)
	{
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				foreach (ReportsParameters item in obj.ReportsParameterslist)
				{
					new List<SqlParameter>();
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "[dbo].[USP_SaleOrderAutoComplete]";
					sqlCommand.Parameters.AddWithValue("@OrganizationId", item.OrganizationId);
					sqlCommand.Parameters.AddWithValue("@CompanyId", item.CompanyId);
					sqlCommand.Parameters.AddWithValue("@SaleOrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("@DocumentTypeId", item.DocumentTypeId);
					sqlCommand.Parameters.AddWithValue("@NetWeight", item.NetWeight);
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
		catch (Exception ex2)
		{
			throw new Exception(ex2.Message);
		}
	}

	public static void SetDetailCancellationStatus(ApprovalList obj)
	{
		if (obj == null || obj.AllApprovalLists == null || obj.AllApprovalLists.Count == 0)
		{
			return;
		}
		DataTable dt = new DataTable();
		dt.Columns.Add("OrganizationId", typeof(int));
		dt.Columns.Add("CompanyId", typeof(int));
		dt.Columns.Add("FinancialYearId", typeof(int));
		dt.Columns.Add("DocumentTypeId", typeof(int));
		dt.Columns.Add("MainId", typeof(int));
		dt.Columns.Add("DetailId", typeof(int));
		dt.Columns.Add("IsCanceled", typeof(bool));
		dt.Columns.Add("UserId", typeof(int));
		dt.Columns.Add("Remarks", typeof(string));
		foreach (ApprovalList item in obj.AllApprovalLists)
		{
			dt.Rows.Add(item.OrganizationId, item.CompanyId, item.FinancialYearId, item.DocumentTypeId, item.Id, item.DetailId, item.Status, item.EntryUser, ((object)item.Comments) ?? ((object)DBNull.Value));
		}
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		using SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			using (SqlCommand cmd = new SqlCommand("[dbo].[USP_SaleOrderDetail_CancelRestore]", sqlcon, sqlTrn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@Rows", dt).SqlDbType = SqlDbType.Structured;
				cmd.ExecuteNonQuery();
			}
			sqlTrn.Commit();
		}
		catch
		{
			sqlTrn.Rollback();
			throw;
		}
	}

	public static DataTable SaleOrderSlipAndRegister_Engr(ReportsParameters obj)
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
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesId",
					Value = obj.ParentCategoryId
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.Status != null && obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = obj.Status
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
			if (obj.ZeroBalanceType != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipZero",
					Value = obj.ZeroBalanceType
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.PurchaseOrder>("USP_SaleOrderSlipAndRegister_Engr", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrder_PendingForInvoice_Engr(ReportsParameters obj)
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
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesId",
					Value = obj.ParentCategoryId
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.Status != null && obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = obj.Status
				});
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
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
			if (obj.ZeroBalanceType != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipZero",
					Value = obj.ZeroBalanceType
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.PurchaseOrder>("USP_SaleOrder_PendingForInvoice_Engr", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrderLoadForDO_Engr(ReportsParameters obj)
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
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesId",
					Value = obj.ParentCategoryId
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.Status != null && obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = obj.Status
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
			if (obj.ZeroBalanceType != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipZero",
					Value = obj.ZeroBalanceType
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.PurchaseOrder>("USP_SaleOrderLoadForDO_Engr", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingSaleOrderForDoAndGdn_Engr(ReportsParameters obj)
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
					ParameterName = "@DocDateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDateTo",
					Value = obj.ToDate
				});
			}
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("[USP_GetPendingSaleOrderForDoAndGdn_Engr]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingSaleOrderDetailForDoAndGdn_Engr(ReportsParameters obj)
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
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Ids",
					Value = obj.Ids
				});
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
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
			if (obj.OrderDetailIds != null && obj.OrderDetailIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderDetailIds",
					Value = obj.OrderDetailIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("[dbo].[USP_GetPendingSaleOrderDetailForDoAndGdn_Engr]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrder_GetDatabyOrderId(int OrderId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = OrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadBySaleOrderHeaderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("Sp_SaleOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LoadSaleOrderForDOInDetail_Engr(int OrganizationId, int CompanyId, string OrderDetailIds)
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
				ParameterName = "@OrderDetailIds",
				Value = OrderDetailIds
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("[dbo].[USP_LoadSaleOrderForDOInDetail_Engr]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrderEng_SummaryRegister(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AppId",
				Value = obj.AppId
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
					ParameterName = "@DocNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderItemId",
					Value = obj.ItemId
				});
			}
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategories",
					Value = obj.ParentCategoryId
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderSupCustId",
					Value = obj.SupplierCustomerId
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
			if (obj.DistrictId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DistrictId",
					Value = obj.DistrictId
				});
			}
			if (obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActivityName",
					Value = obj.ReportType
				});
			}
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
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
			if (!string.IsNullOrEmpty(obj.Status))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderStatus",
					Value = obj.Status
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
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_SaleOrderEng_SummaryRegister]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable SaleOrderDetailRegister_Mfg(ReportsParameters obj)
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesId",
					Value = obj.InventoryParentCategories
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
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
			if (obj.Status != null && obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = obj.Status
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
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("[Mfg].[USP_SaleOrderDetailRegister_Eng]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable SaleOrderDetailRegister_Eng(ReportsParameters obj)
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesId",
					Value = obj.InventoryParentCategories
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
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
			if (obj.Status != null && obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = obj.Status
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
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("[dbo].[USP_SaleOrderDetailRegister_Eng]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataSet SalesOrder_OutstandingDeliverySchedule(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> s = new List<SqlParameter>();
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
			s.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@IncludeExpired",
				Value = obj.IncludeExpired
			});
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				s.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				s.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			if (obj.IntervalDays != 0)
			{
				s.Add(new SqlParameter
				{
					ParameterName = "@IntervalDays",
					Value = obj.IntervalDays
				});
			}
			return GenericProvider.GetDataSetProc<Architecture.Model.Inventory.SaleOrder>("[dbo].[Usp_SalesOrder_OutstandingDeliverySchedule]", s);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrderLoaderForGdnStoreAndPm(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.OrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderId",
					Value = obj.OrderId
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
			if (obj.ZeroBalanceType != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ZeroBalanceType
				});
			}
			return GenericProvider.GetDataTableProc<PurchaseOrderDetail>("USP_GetSaleOrderdLoaderForGdnStoreAndPm", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
