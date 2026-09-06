using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.BLL.Pos.Financials;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Pos;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.POS;

namespace Architecture.BLL.Pos;

public class WsRmSaleInvoiceReturn
{
	public static VoucherHead MakeVoucher(Architecture.Model.POS.WsRmSaleInvoiceReturn obj)
	{
		string SupplierName = "";
		List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
		List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
		DataTable dtCoa = new DataTable();
		VoucherHead vh = new VoucherHead();
		try
		{
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.DocNo);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = Conversion.ToString(obj.RemarksHeader);
			vh.RemarksOtherLingo = "";
			vh.VoucherAmount = obj.BillAmount;
			if (obj.SupplierCustomerId == 0)
			{
				throw new Exception("SupplierCustomerId Not Found");
			}
			if (obj.RefDocumentTypeId == 100)
			{
				vh.RefAccountId = obj.SupplierCustomerId;
			}
			else
			{
				dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
				if (dtSupplier.Count > 0)
				{
					List<SupplierCustomerGlAccountId> find = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(obj.SupplierCustomerId)).ToList();
					if (find.Count <= 0)
					{
						throw new Exception("Supplier GLAccountId not Found");
					}
					vh.RefAccountId = Conversion.ToInt(find[0].GlAccountId);
					SupplierName = Conversion.ToString(find[0].CompanyName);
				}
			}
			CommonIdsForFinancials commonIds = new CommonIdsForFinancials();
			commonIds.SupplierGlAccountId = vh.RefAccountId;
			commonIds.CommissionGlAccountId = vh.AgainstAccountId;
			commonIds.CompanyName = SupplierName;
			obj.CommonIdsForFinancials = commonIds;
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.BillAmount = obj.BillAmount;
			vh.ManualBillNo = obj.ManualBillNo;
			vh.DueDate = DateTime.Now;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			bool CgsAllow = false;
			string ConfigKey = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CGSEntryAllow");
			if (ConfigKey != string.Empty)
			{
				CgsAllow = Conversion.ToBool(ConfigKey);
			}
			if (obj.DocumentTypeId == 116 && obj.RefDocumentTypeId == 106)
			{
				if (obj.PaymentTermsId == 1)
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
						Value = vh.RefAccountId
					});
					p.Add(new SqlParameter
					{
						ParameterName = "@CoaType",
						Value = "GetAccountTypeId"
					});
					dtCoa = GenericProvider.GetDataTableProc<ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
				}
				foreach (VoucherDetail item9 in WsRmSaleInvoiceReturnFinancialMethods.WsRmSaleInvoiceReturnCustomerAndSalesGL(obj, dtitem).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item9);
				}
				foreach (VoucherDetail item8 in WsRmSaleInvoiceReturnFinancialMethods.WsRmSaleInvoiceReturnCustomerAndDiscount(obj).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item8);
				}
				if (CgsAllow)
				{
					foreach (VoucherDetail item7 in WsRmSaleInvoiceReturnFinancialMethods.WsRmSaleInvoiceReturnCgsAndStock(obj, dtitem).voucherDetailList.ToList())
					{
						vh.voucherDetailList.Add(item7);
					}
				}
				foreach (VoucherDetail item6 in WsRmSaleInvoiceReturnFinancialMethods.WsRmSaleInvoiceReturnCashAndCustomer(obj, dtitem, dtCoa).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item6);
				}
			}
			if (obj.DocumentTypeId == 116 && obj.RefDocumentTypeId == 100)
			{
				foreach (VoucherDetail item5 in PosSaleInvoiceFinancialMethods.PosSaleInvoiceReturnCustomerAndSalesGL(obj, dtitem).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item5);
				}
				foreach (VoucherDetail item4 in PosSaleInvoiceFinancialMethods.PosSaleInvoiceReturnCustomerAndDiscount(obj).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item4);
				}
				if (CgsAllow)
				{
					foreach (VoucherDetail item3 in PosSaleInvoiceFinancialMethods.PosSaleInvoiceReturnCgsAndStock(obj, dtitem).voucherDetailList.ToList())
					{
						vh.voucherDetailList.Add(item3);
					}
				}
			}
			if (obj.DocumentTypeId == 116 && (obj.RefDocumentTypeId == 128 || obj.RefDocumentTypeId == 103))
			{
				foreach (VoucherDetail item2 in BookingSaleInvoiceFinancialMethods.BookingSaleInvoiceReturnCustomerAndSalesGL(obj, dtitem).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item2);
				}
				if (CgsAllow)
				{
					foreach (VoucherDetail item in BookingSaleInvoiceFinancialMethods.BookingSaleInvoiceReturnCgsAndStock(obj, dtitem).voucherDetailList.ToList())
					{
						vh.voucherDetailList.Add(item);
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		return vh;
	}

	public static int Save(Architecture.Model.POS.WsRmSaleInvoiceReturn obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
				return new Architecture.DAL.Pos.WsRmSaleInvoiceReturn().SetData(obj, "Sp_WsRmSaleInvoiceReturn_Insert");
			}
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			return new Architecture.DAL.Pos.WsRmSaleInvoiceReturn().SetData(obj, "Sp_WsRmSaleInvoiceReturn_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.POS.WsRmSaleInvoiceReturn GetByID(int Id)
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
			new List<Architecture.Model.POS.WsRmSaleInvoiceReturn>();
			return new Architecture.DAL.Pos.WsRmSaleInvoiceReturn().GetData("Sp_WsRmSaleInvoiceReturn_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateDocCode(Architecture.Model.POS.WsRmSaleInvoiceReturn obj)
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
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmSaleInvoiceReturn>("Sp_WsRmSaleInvoiceReturn_GetAllMethod", p);
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

	public static int GetIdByDocumentTypeIdAndDocNo(Architecture.Model.POS.WsRmSaleInvoiceReturn obj)
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
				ParameterName = "@DocNo",
				Value = obj.DocNo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetIdByDocumentTypeIdAndDocNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmSaleInvoiceReturn>("Sp_WsRmSaleInvoiceReturn_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Code = Conversion.ToInt(dt.Rows[0]["Id"]);
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
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.PosSaleInvoice>("Sp_WsRmSaleInvoiceReturn_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
