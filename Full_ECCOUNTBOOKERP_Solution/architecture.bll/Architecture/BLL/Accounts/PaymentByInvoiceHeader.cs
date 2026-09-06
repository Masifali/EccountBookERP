using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Accounts;

public class PaymentByInvoiceHeader
{
	public static Architecture.Model.Accounts.VoucherHead MakeVoucher(Architecture.Model.Accounts.PaymentByInvoiceHeader obj)
	{
		Architecture.Model.Accounts.VoucherHead vh = new Architecture.Model.Accounts.VoucherHead();
		try
		{
			List<Architecture.Model.Inventory.SupplierCustomer> dtSupplier = new List<Architecture.Model.Inventory.SupplierCustomer>();
			if (obj.DocumentTypeId == 14)
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
				dtSupplier = new Architecture.DAL.Inventory.SupplierCustomer().GetData("Sp_SupplierCustomer_GetAllMethod", p);
				if (dtSupplier.Count > 0)
				{
					List<Architecture.Model.Inventory.SupplierCustomer> find = dtSupplier.Where((Architecture.Model.Inventory.SupplierCustomer x) => x.Id == Conversion.ToInt(obj.SupplierCustomerId)).ToList();
					if (find.Count <= 0)
					{
						throw new Exception("Supplier GLAccountId not Found");
					}
					vh.RefAccountId = Conversion.ToInt(find[0].GlAccountId);
				}
				int FinancialYearId = 0;
				DataTable dt = CommonServies.GetActiveFinancialYear(obj.OrganizationId, obj.CompanyId);
				if (dt.Rows.Count <= 0)
				{
					throw new Exception("Financial Year Not Found Please Check");
				}
				FinancialYearId = Conversion.ToInt(dt.Rows[0]["Id"]);
				vh.FinancialYearId = FinancialYearId;
				vh.EntryDate = DateTime.Now;
				vh.EntryUser = obj.EntryUser;
				vh.ModifyDate = DateTime.Today;
				vh.ModifyUser = obj.ModifyUser;
				vh.OrganizationId = obj.OrganizationId;
				vh.CompanyId = obj.CompanyId;
				vh.DocumentTypeSrNo = obj.Id;
				vh.VoucherDate = Conversion.ToDateTime(obj.DocDate);
				vh.VoucherCode = Conversion.ToInt(obj.DocNo);
				vh.DocumentTypeId = obj.DocumentTypeId;
				vh.AgainstAccountId = vh.RefAccountId;
				vh.voucherDetailList = new List<VoucherDetail>();
				double TotalAmount = 0.0;
				foreach (PaymentByInvoiceOtherDetail item in obj.PaymentByInvoiceOtherDetailList.ToList())
				{
					if (Conversion.ToInt(item.Amount) > 0)
					{
						VoucherDetail vd = new VoucherDetail();
						vd.AccountId = Conversion.ToInt(item.GLAccountId);
						vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd.Comments = Conversion.ToString(item.Remarks);
						vd.DebitAmount = Conversion.ToDouble(0);
						vd.CreditAmount = Conversion.ToDouble(item.Amount);
						TotalAmount += vd.CreditAmount;
						if (item.TransType == "Advance")
						{
							vd.AdvanceAmount = Conversion.ToDouble(item.Amount);
							vd.PaymentType = item.TransType;
						}
						if (item.TransType == "PdcCheq")
						{
							vd.InvoiceNoRefId = item.CheqId;
							vd.DCheqDate = item.CheqDate;
							vd.CheqNoDetail = item.CheqNo;
						}
						else
						{
							vd.InvoiceNoRefId = item.CheqId;
							vd.DCheqDate = item.CheqDate;
							vd.CheqNoDetail = item.CheqNo;
						}
						vh.voucherDetailList.Add(vd);
						VoucherDetail vd2 = new VoucherDetail();
						vd2.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd2.AgainstAccountId = Conversion.ToInt(item.GLAccountId);
						vd2.Comments = Conversion.ToString(item.Remarks);
						vd2.CreditAmount = Conversion.ToDouble(0);
						vd2.DebitAmount = Conversion.ToDouble(item.Amount);
						vh.voucherDetailList.Add(vd2);
					}
				}
				vh.VoucherAmount = TotalAmount;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return vh;
	}

	public static int Save(Architecture.Model.Accounts.PaymentByInvoiceHeader obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.VoucherHeadPaymenet = MakeVoucher(obj);
				return new Architecture.DAL.Accounts.PaymentByInvoiceHeader().SetDate(obj, "Sp_PaymentByInvoiceHeader_Insert");
			}
			obj.VoucherHeadPaymenet = MakeVoucher(obj);
			return new Architecture.DAL.Accounts.PaymentByInvoiceHeader().SetDate(obj, "Sp_PaymentByInvoiceHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Accounts.PaymentByInvoiceHeader GetByID(int Id)
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
			return new Architecture.DAL.Accounts.PaymentByInvoiceHeader().GetDate("Sp_PaymentByInvoiceHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetPaymentbyInvoiceIdbyDocNo(ReportsParameters obj)
	{
		DataTable dt = null;
		try
		{
			int Id = 0;
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
				Value = "GetIdByDocNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.PaymentByInvoiceHeader>("Sp_PaymentByInvoiceHeader_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Id = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			return Id;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Accounts.PaymentByInvoiceHeader obj)
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
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.PaymentByInvoiceHeader>("Sp_PaymentByInvoiceHeader_GetAllMethod", p);
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

	public static double PaymentByInvoiceAdvanceBalance(Architecture.Model.Accounts.PaymentByInvoiceHeader obj)
	{
		DataTable dt = null;
		try
		{
			double code = 0.0;
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
				ParameterName = "@AccountId",
				Value = obj.SupplierCustomerId
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.PaymentByInvoiceHeader>("Sp_PaymentByInvoiceAdvanceBalance", p);
			if (dt.Rows.Count > 0)
			{
				code = Conversion.ToInt(dt.Rows[0]["BalanceAdvance"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
