using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.BLL.Accounts;
using Architecture.BLL.FeedMill.Financials.SaleInvoice;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.FeedMill.Sales;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.FeedMill.Sales;
using Architecture.Model.Inventory;

namespace Architecture.BLL.FeedMill.Sales;

public class InvSaleInvoice
{
	public static int Save(Architecture.Model.FeedMill.Sales.InvSaleInvoice obj)
	{
		try
		{
			obj.VoucherHeadInvoices = SaleInvoiceFinancial.MakeVoucherForSaleInvoice(obj);
			if (obj.Id == 0)
			{
				if (obj.InvSaleInvoiceDetaillist.Exists((Architecture.Model.FeedMill.Sales.InvSaleInvoiceDetail p) => p.Id > 0))
				{
					throw new Exception("Record cannot be inserted because detailId greater than zero");
				}
				obj.ModifyUserId = 0;
				return new Architecture.DAL.FeedMill.Sales.InvSaleInvoice().SetData(obj, "fed.USP_InvSaleInvoice_Insert");
			}
			obj.EntryUserId = 0;
			return new Architecture.DAL.FeedMill.Sales.InvSaleInvoice().SetData(obj, "fed.USP_InvSaleInvoice_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.FeedMill.Sales.InvSaleInvoice GetByID(int Id)
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
			return new Architecture.DAL.FeedMill.Sales.InvSaleInvoice().GetData("[fed].[usp_InvSaleInvoice_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.FeedMill.Sales.InvSaleInvoice obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Sales.InvSaleInvoice>("[fed].[usp_InvSaleInvoice_GetAllMethod]", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				code = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return code;
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
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
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
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
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
			if (obj.PaymenetTermId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PaymentTermId",
					Value = obj.PaymenetTermId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("fed.usp_InvSaleInvoice_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromSaleInvoice(ReportsParameters obj)
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
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Sales.InvSaleInvoice>("[fed].[USP_GetDataForDropDownFromSaleInvoice]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetLastSaleTaxAcId(ReportsParameters obj)
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
				ParameterName = "@Activity",
				Value = "GetLastSaleTaxAcId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("Sp_InvSaleInvoice_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["SaleTaxAcId"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void RemoveByID(Architecture.Model.Inventory.InvSaleInvoice obj)
	{
		try
		{
			DataTable lst = Architecture.BLL.Accounts.VoucherHead.RecordCheckTables(new ReportsParameters
			{
				OrganizationId = obj.OrganizationId,
				CompanyId = obj.CompanyId,
				DocumentTypeId = obj.DocumentTypeId,
				Id = obj.Id
			});
			if (lst.Rows.Count > 0)
			{
				if (Conversion.ToInt(lst.Rows[0]["RefId"].ToString()) > 0)
				{
					throw new Exception("Record Not Update beacuase Record has Exist");
				}
			}
			else
			{
				new Architecture.DAL.Inventory.InvSaleInvoice().AccountandInventoryRemoveById(obj);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvSaleInvoiceDirectSlip(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("fed.usp_InvSaleInvoiceDirectSlip", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrderCustomerExpensesBySaleOrderId(int Id)
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
				Value = "SaleOrderCustomerExpensesByHeaderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Sales.SaleOrder>("[fed].[usp_InvSaleInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrderPaymentTermDetailBySaleOrderId(int Id)
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
				Value = "SaleOrderPaymentTermDetailByHeaderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Sales.SaleOrder>("[fed].[usp_InvSaleInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrderCommissionDetailByHeaderId(int Id)
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
				Value = "SaleOrderCommissionDetailByHeaderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Sales.SaleOrder>("[fed].[usp_InvSaleInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingInvSaleInvoiceForApproval(ReportsParameters obj)
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
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("USp_PendingInvSaleInvoiceForApproval", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InvSaleInvoiceandVoucherApprove(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "InvSaleInvoiceandVoucherApprove"
				});
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
					ParameterName = "@DocumentTypeId",
					Value = item.DocumentTypeId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = item.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@PostDate",
					Value = item.PostDate
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = item.EntryUser
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ReqType",
					Value = item.ReqType
				});
				GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_InvSaleInvoice_GetAllMethod", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSaleInvoiceReturnForApproval(ReportsParameters obj)
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
				Value = "GetSaleInvoiceReturnForApproval"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_InvSaleInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCommissionRateByCustomerAndItemIdSFromCommPolicy(ReportsParameters obj)
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
				ParameterName = "@CustomerId",
				Value = obj.CustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemIds",
				Value = obj.ItemIds
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectiveDate",
				Value = obj.EffectedDate
			});
			if (obj.PolicyTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PolicyTypeId",
					Value = obj.PolicyTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[dbo].[USP_GetCommissionRateByCustomerAndItemIdFromCommPolicy]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleAnalyticsA_Report(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchId", obj.BranchesId));
			}
			p.Add(new SqlParameter("@FromDate", obj.FromDate));
			p.Add(new SqlParameter("@ToDate", obj.ToDate));
			p.Add(new SqlParameter("@SeasonStart", obj.StartDate));
			p.Add(new SqlParameter("@SeasonEnd", obj.EndDate));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("USP_SaleAnalyticsA_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleAnalyticsAB_Report(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchId", obj.BranchesId));
			}
			p.Add(new SqlParameter("@FromDate", obj.FromDate));
			p.Add(new SqlParameter("@ToDate", obj.ToDate));
			p.Add(new SqlParameter("@SeasonStart", obj.StartDate));
			p.Add(new SqlParameter("@SeasonEnd", obj.EndDate));
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter("@ParentCategoryIdsForReports", obj.Ids));
			}
			if (obj.SortNo != 0)
			{
				p.Add(new SqlParameter("@SortNo", obj.SortNo));
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter("@ItemId", obj.ItemId));
			}
			p.Add(new SqlParameter("@Activity", obj.Activity));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("USP_SaleAnalyticsAB_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InvoiceStatusUpdate(int Id, string CustomerRemarks)
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
				ParameterName = "@CustomerRemarks",
				Value = CustomerRemarks
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGdn>("USP_InvoiceStatusUpdate", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
