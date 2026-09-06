using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.DAL.Import;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Export;
using Architecture.Model.Import;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Import;

public class ExImFCBankPaymensts
{
	public static VoucherHead MakeVoucher(Architecture.Model.Import.ExImFCBankPaymensts obj)
	{
		List<Architecture.Model.Export.ExImProceedsChargesType> dtCharges = new List<Architecture.Model.Export.ExImProceedsChargesType>();
		List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
		VoucherHead vh = new VoucherHead();
		try
		{
			List<SqlParameter> z = new List<SqlParameter>();
			z.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			z.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			z.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyId"
			});
			dtCharges = new Architecture.DAL.Export.ExImProceedsChargesType().GetAll("Sp_ExImProceedsChargesType_GetAllMethod", z);
			double VoucherAmt = 0.0;
			double BillAmt = 0.0;
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.DocumentNo);
			vh.VoucherDate = obj.DocumentDate;
			vh.Remarks = "";
			vh.RemarksOtherLingo = "";
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (dtSupplier.Count > 0)
			{
				List<SupplierCustomerGlAccountId> find2 = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(obj.SupplierCustomerId)).ToList();
				if (find2.Count <= 0)
				{
					throw new Exception("Supplier GLAccountId not Found");
				}
				vh.RefAccountId = Conversion.ToInt(find2[0].GlAccountId);
			}
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchId;
			vh.ProjectId = obj.ProjectId;
			vh.ManualBillNo = "";
			vh.DueDate = DateTime.Now;
			vh.DueDays = 0;
			vh.ExchangeCurrencyRate = obj.ExchangeRate;
			vh.FcAmount = obj.FcNetAmount;
			vh.MultiCurrencyId = obj.MultiCurrencyId;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.PostUser = 0;
			vh.voucherDetailList = new List<VoucherDetail>();
			if (obj.DocumentTypeId == 204)
			{
				VoucherDetail vd4 = new VoucherDetail();
				vd4.AccountId = Conversion.ToInt(obj.BankGlDrAc);
				vd4.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
				vd4.CreditAmount = Conversion.ToDouble(obj.BankDrAmount);
				VoucherAmt += vd4.CreditAmount;
				vd4.DebitAmount = Conversion.ToDouble(0);
				vd4.Comments = obj.Remarks;
				vd4.DMultiCurrencyId = obj.MultiCurrencyId;
				vd4.DExchangeCurrencyRate = obj.ExchangeRate;
				double FcAmt = obj.BankDrAmount / obj.ExchangeRate;
				vd4.DCurrencyAmount = FcAmt;
				vh.voucherDetailList.Add(vd4);
				double PartyAmount = 0.0;
				double GainlossAmt = 0.0;
				double DeductionAmount = 0.0;
				foreach (ExImFcBankPaymentsDeductions r in obj.ExImFcBankPaymentsDeductionslst.ToList())
				{
					DeductionAmount += r.RsAmount;
				}
				if (obj.ExmLcPaymentTerm == "Invoice Payment" && obj.VoucherExchangeRate > 0.0)
				{
					PartyAmount = obj.FcGrossAmount * obj.VoucherExchangeRate;
					GainlossAmt = obj.BankDrAmount - PartyAmount;
					VoucherDetail vd5 = new VoucherDetail();
					vd5.AccountId = vh.RefAccountId;
					vd5.AgainstAccountId = Conversion.ToInt(obj.BankGlDrAc);
					vd5.CreditAmount = Conversion.ToDouble(0);
					vd5.DebitAmount = Conversion.ToDouble(PartyAmount);
					BillAmt += vd5.DebitAmount;
					vd5.Comments = obj.Remarks;
					vd5.DMultiCurrencyId = obj.MultiCurrencyId;
					vd5.DExchangeCurrencyRate = obj.ExchangeRate;
					double FcYAmtG = PartyAmount / obj.VoucherExchangeRate;
					vd5.DCurrencyAmount = FcYAmtG;
					vh.voucherDetailList.Add(vd5);
					VoucherDetail vd6 = new VoucherDetail();
					vd6.AccountId = obj.VoucherHeadId;
					vd6.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
					if (GainlossAmt > 0.0)
					{
						vd6.CreditAmount = Conversion.ToDouble(0);
						vd6.DebitAmount = Conversion.ToDouble(GainlossAmt);
					}
					else
					{
						vd6.DebitAmount = Conversion.ToDouble(0);
						vd6.CreditAmount = Math.Abs(Conversion.ToDouble(GainlossAmt));
					}
					vd6.Comments = obj.Remarks;
					vd6.DMultiCurrencyId = obj.MultiCurrencyId;
					vd6.DExchangeCurrencyRate = obj.ExchangeRate;
					double FcYAmtGain = GainlossAmt / obj.VoucherExchangeRate;
					vd6.DCurrencyAmount = FcYAmtGain;
					vh.voucherDetailList.Add(vd6);
				}
				else
				{
					VoucherDetail vd3 = new VoucherDetail();
					vd3.AccountId = vh.RefAccountId;
					vd3.AgainstAccountId = Conversion.ToInt(obj.BankGlDrAc);
					vd3.CreditAmount = Conversion.ToDouble(0);
					vd3.DebitAmount = Conversion.ToDouble(obj.BankDrAmount);
					BillAmt += vd3.DebitAmount;
					vd3.Comments = obj.Remarks;
					vd3.DMultiCurrencyId = obj.MultiCurrencyId;
					vd3.DExchangeCurrencyRate = obj.ExchangeRate;
					double FcYAmt = obj.BankDrAmount / obj.ExchangeRate;
					vd3.DCurrencyAmount = FcYAmt;
					vh.voucherDetailList.Add(vd3);
				}
				if (obj.ExImFcBankPaymentsDeductionslst.Count > 0)
				{
					foreach (ExImFcBankPaymentsDeductions r2 in obj.ExImFcBankPaymentsDeductionslst.ToList())
					{
						if (dtCharges.Count > 0)
						{
							List<Architecture.Model.Export.ExImProceedsChargesType> find = dtCharges.Where((Architecture.Model.Export.ExImProceedsChargesType x) => x.Id == Conversion.ToInt(r2.ExImProceedsChargesTypeId)).ToList();
							if (find.Count <= 0)
							{
								throw new Exception("Charges AccountType GLAccountId not Found");
							}
							VoucherDetail vd = new VoucherDetail();
							vd.AccountId = Conversion.ToInt(find[0].ProceedsChargesGlAccount);
							vd.AgainstAccountId = obj.BankGlDrAc;
							vd.Comments = r2.ProceedsRemarks;
							vd.DebitAmount = Conversion.ToDouble(r2.RsAmount);
							VoucherAmt += vd.DebitAmount;
							if (r2.Type == "Local")
							{
								double FYAmount = r2.RsAmount / obj.ExchangeRate;
								vd.DCurrencyAmount = FYAmount;
								vd.DExchangeCurrencyRate = obj.ExchangeRate;
								vd.DMultiCurrencyId = obj.MultiCurrencyId;
							}
							else
							{
								vd.DCurrencyAmount = r2.FcAmount;
								vd.DExchangeCurrencyRate = obj.ExchangeRate;
								vd.DMultiCurrencyId = obj.MultiCurrencyId;
							}
							vd.CreditAmount = Conversion.ToDouble(0);
							vh.voucherDetailList.Add(vd);
							VoucherDetail vd2 = new VoucherDetail();
							vd2.AccountId = obj.BankGlDrAc;
							vd2.AgainstAccountId = Conversion.ToInt(find[0].ProceedsChargesGlAccount);
							vd2.Comments = r2.ProceedsRemarks;
							vd2.DebitAmount = Conversion.ToDouble(0);
							vd2.CreditAmount = Conversion.ToDouble(r2.RsAmount);
							BillAmt += vd2.CreditAmount;
							if (r2.Type == "Local")
							{
								double FcYAmount = r2.RsAmount / obj.ExchangeRate;
								vd2.DCurrencyAmount = FcYAmount;
								vd2.DExchangeCurrencyRate = obj.ExchangeRate;
								vd2.DMultiCurrencyId = obj.MultiCurrencyId;
							}
							else
							{
								vd2.DCurrencyAmount = r2.FcAmount;
								vd2.DExchangeCurrencyRate = obj.ExchangeRate;
								vd2.DMultiCurrencyId = obj.MultiCurrencyId;
							}
							vh.voucherDetailList.Add(vd2);
						}
					}
					vh.VoucherAmount = VoucherAmt;
					vh.BillAmount = BillAmt;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return vh;
	}

	public static int Save(Architecture.Model.Import.ExImFCBankPaymensts obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.FcBankVoucherHead = MakeVoucher(obj);
				return new Architecture.DAL.Import.ExImFCBankPaymensts().SetDate(obj, "Sp_ExImFCBankPaymensts_Insert");
			}
			obj.FcBankVoucherHead = MakeVoucher(obj);
			return new Architecture.DAL.Import.ExImFCBankPaymensts().SetDate(obj, "Sp_ExImFCBankPaymensts_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Import.ExImFCBankPaymensts obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImFCBankPaymensts>("Sp_ExImFCBankPayments_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Import.ExImFCBankPaymensts obj)
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
				Value = "GenerateDocNo"
			});
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImFCBankPaymensts>("Sp_ExImFCBankPayments_GetAllMethod", p).Rows[0]["DocNo"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Import.ExImFCBankPaymensts GetByID(int Id)
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
			new List<Architecture.Model.Import.ExImFCBankPaymensts>();
			return new Architecture.DAL.Import.ExImFCBankPaymensts().GetDate("Sp_ExImFCBankPayments_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable HistoryForm(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImFCBankPaymensts>("Sp_ExImFCBankPayments_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
