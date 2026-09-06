using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Accounts;

public class ReceiptByInvoiceHeader
{
	public static Architecture.Model.Accounts.VoucherHead MakeVoucher(Architecture.Model.Accounts.ReceiptByInvoiceHeader obj)
	{
		Architecture.Model.Accounts.VoucherHead vh = new Architecture.Model.Accounts.VoucherHead();
		try
		{
			int AdvanceGlAcId = 0;
			List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
			if (obj.DocumentTypeId == 29)
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
					AdvanceGlAcId = Conversion.ToInt(find[0].AdvanceGlAcId);
				}
				vh.FinancialYearId = obj.FinancialYearId;
				vh.EntryDate = DateTime.Now;
				vh.EntryUser = obj.EntryUser;
				vh.ModifyDate = DateTime.Now;
				vh.PostDate = DateTime.Now;
				vh.DueDate = DateTime.Now;
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
				foreach (ReceiptByInvoiceOtherDetail item in obj.ReceiptByInvoiceOtherDetailList.ToList())
				{
					if (Conversion.ToInt(item.Amount) <= 0)
					{
						continue;
					}
					if (item.TransType == 2)
					{
						if (AdvanceGlAcId == 0)
						{
							throw new Exception("Advance GlAccountId Not Found");
						}
						VoucherDetail vd2 = new VoucherDetail();
						vd2.AccountId = Conversion.ToInt(item.GLAccountId);
						vd2.AgainstAccountId = Conversion.ToInt(AdvanceGlAcId);
						vd2.Comments = Conversion.ToString(item.Remarks);
						vd2.CreditAmount = Conversion.ToDouble(0);
						vd2.DebitAmount = Conversion.ToDouble(item.Amount);
						TotalAmount += vd2.DebitAmount;
						vh.voucherDetailList.Add(vd2);
						VoucherDetail vd5 = new VoucherDetail();
						vd5.AccountId = Conversion.ToInt(AdvanceGlAcId);
						vd5.AgainstAccountId = Conversion.ToInt(item.GLAccountId);
						vd5.Comments = Conversion.ToString(item.Remarks);
						vd5.DebitAmount = Conversion.ToDouble(0);
						vd5.CreditAmount = Conversion.ToDouble(item.Amount);
						vd5.SubsidiaryTypeId = 1;
						vd5.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
						vd5.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vh.voucherDetailList.Add(vd5);
					}
					else if (item.TransType != 3 && vh.RefAccountId != item.GLAccountId)
					{
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(item.GLAccountId);
						vd3.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd3.Comments = Conversion.ToString(item.Remarks);
						vd3.CreditAmount = Conversion.ToDouble(0);
						vd3.DebitAmount = Conversion.ToDouble(item.Amount);
						TotalAmount += vd3.DebitAmount;
						vh.voucherDetailList.Add(vd3);
						VoucherDetail vd6 = new VoucherDetail();
						vd6.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd6.AgainstAccountId = Conversion.ToInt(item.GLAccountId);
						vd6.Comments = Conversion.ToString(item.Remarks);
						vd6.DebitAmount = Conversion.ToDouble(0);
						vd6.CreditAmount = Conversion.ToDouble(item.Amount);
						vd6.SubsidiaryTypeId = 1;
						vd6.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
						vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vh.voucherDetailList.Add(vd6);
					}
				}
				if (obj.TaxAmount > 0.0)
				{
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = Conversion.ToInt(obj.TaxDebitAcId);
					vd.AgainstAccountId = Conversion.ToInt(obj.TaxCreditAcId);
					vd.Comments = Conversion.ToString(obj.TaxPrct) + obj.TaxAmount;
					vd.CreditAmount = Conversion.ToDouble(0);
					vd.DebitAmount = Conversion.ToDouble(obj.TaxAmount);
					vh.voucherDetailList.Add(vd);
					VoucherDetail vd4 = new VoucherDetail();
					vd4.AccountId = Conversion.ToInt(obj.TaxCreditAcId);
					vd4.AgainstAccountId = Conversion.ToInt(obj.TaxDebitAcId);
					vd4.Comments = Conversion.ToString(obj.TaxPrct) + obj.TaxAmount;
					vd4.DebitAmount = Conversion.ToDouble(0);
					vd4.CreditAmount = Conversion.ToDouble(obj.TaxAmount);
					vh.voucherDetailList.Add(vd4);
					TotalAmount += vd.DebitAmount;
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

	public static int Save(Architecture.Model.Accounts.ReceiptByInvoiceHeader obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.VoucherHeadPaymenet = MakeVoucher(obj);
				return new Architecture.DAL.Accounts.ReceiptByInvoiceHeader().SetDate(obj, "Sp_ReceiptByInvoiceHeader_Insert");
			}
			obj.VoucherHeadPaymenet = MakeVoucher(obj);
			return new Architecture.DAL.Accounts.ReceiptByInvoiceHeader().SetDate(obj, "Sp_ReceiptByInvoiceHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Accounts.ReceiptByInvoiceHeader GetByID(int Id)
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
			return new Architecture.DAL.Accounts.ReceiptByInvoiceHeader().GetDate("Sp_ReceiptByInvoiceHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetReceiptbyInvoiceIdbyDocNo(ReportsParameters obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ReceiptByInvoiceHeader>("Sp_ReceiptByInvoiceHeader_GetAllMethod", p);
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

	public static int GenerateCode(Architecture.Model.Accounts.ReceiptByInvoiceHeader obj)
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
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ReceiptByInvoiceHeader>("Sp_ReceiptByInvoiceHeader_GetAllMethod", p);
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

	public static double ReceiptByInvoiceAdvanceBalance(Architecture.Model.Accounts.ReceiptByInvoiceHeader obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ReceiptByInvoiceHeader>("Sp_ReceiptByInvoiceAdvanceBalance", p);
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
