using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Import;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Import;

namespace Architecture.BLL.Import;

public class ImLcOrder
{
	public static int GenerateCode(Architecture.Model.Import.ImLcOrder obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateDocNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("[dbo].[USP_ImLcOrder_GetAllMethod]", p);
			if (dt.Rows.Count > 0)
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

	public static int Save(Architecture.Model.Import.ImLcOrder obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.Import.ImLcOrder().SetDate(obj, "[dbo].[USP_ImLcOrder_Insert]");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.Import.ImLcOrder().SetDate(obj, "[dbo].[USP_ImLcOrder_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Import.ImLcOrder ReadById(int Id)
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
			new List<Architecture.Model.Import.ImLcOrder>();
			return new Architecture.DAL.Import.ImLcOrder().GetDate("[dbo].[USP_ImLcOrder_GetAllMethod]", p)[0];
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
			GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("[dbo].[USP_ImLcOrder_GetAllMethod]", p);
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
					ParameterName = "@EntryUserId",
					Value = obj.EntryUser
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("[dbo].[USP_ImLcOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryDetail(int Id)
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
				Value = "FormHistoryDetail"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("[dbo].[USP_ImLcOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryReadByOrganizationCompanyId(Architecture.Model.Import.ImLcOrder obj)
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
				Value = "ReadByOrganizationCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("[dbo].[USP_ImLcOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ApporveImportContract(ReportsParameters obj)
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
				ParameterName = "@ApprovedDate",
				Value = obj.PostDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ApprovedUserId",
				Value = obj.EntryUser
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@IsApproved",
				Value = obj.IsApproved
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ApporveImportContract"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("[dbo].[USP_ImLcOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetImportLcOrderItems(int OrganizationId, int CompanyId, int Id)
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
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetImportLcOrderItems"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<ExImLcOrder>("[dbo].[USP_ImLcOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ImLcOrderPaymentDetail_SubReport(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ImLcOrderId",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("[dbo].[USP_ImLcOrderPaymentDetail_SubReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ImLcOrder_SlipAndRegister(ReportsParameters obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LcOrderId",
					Value = obj.Id
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromOrderDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToOrderDate",
					Value = obj.ToDate
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LcOrderNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LcOrderNoTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = obj.Status
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
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("[dbo].[USP_ImLcOrder_Register]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetIdByDocNo(ReportsParameters obj)
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
				ParameterName = "@LcOrderNo",
				Value = obj.OrderNo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetIdByDocNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("[dbo].[USP_ImLcOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLcOrderNo(ReportsParameters obj)
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
				Value = "GetLcOrderNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("[dbo].[USP_ImLcOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ContractNo(Architecture.Model.Import.ImLcOrder exImLcOrder)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = exImLcOrder.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = exImLcOrder.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetLcOrderIdReferedInShipmentSchedule"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("[dbo].[USP_ImLcOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ReadDataForShipmentScheduleBySalesContractId(ReportsParameters obj)
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
				Value = "ReadDataForShipmentScheduleBySalesContractId"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("Sp_ImLcOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable EximInvoiceRegister(ReportsParameters obj)
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
			if (obj.InvoiceNo != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvoiceNo",
					Value = obj.InvoiceNo
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
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("Sp_ImInvoice_ReceivedAndOutstandingHistory_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ExportDeliveryOrderInvoiceGatepassRegister(ReportsParameters obj)
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
			if (obj.ExImLcOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractId",
					Value = obj.ExImLcOrderId
				});
			}
			if (obj.ExImInvoiceNoId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExportInvoiceId",
					Value = obj.ExImInvoiceNoId
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("SpExportDeliveryOrderInvoiceGatepassRegister_rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetLcOrderNoBySupplierCustomerId(ReportsParameters obj)
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
				ParameterName = "@SupCustId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetLcOrderNoBySupplierCustomerId"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("Sp_ImLcOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ReadByExportSaleOrderIDNOrderItemId(ReportsParameters obj)
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
				Value = "ReadByExportSaleOrderIDNOrderItemId"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("Sp_ImLcOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetImportCustomerByAdvancePayment(ReportsParameters obj)
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
				Value = "GetImportCustomerByAdvancePayment"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrder>("Sp_ImLcOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Architecture.Model.Import.ImLcOrder GetDetailImLcOrderByImLcOrderId(int Id)
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
			new List<Architecture.Model.Import.ImLcOrder>();
			return new Architecture.DAL.Import.ImLcOrder().GetDetailImLcOrderByImLcOrderId("Sp_ImLcOrder_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
