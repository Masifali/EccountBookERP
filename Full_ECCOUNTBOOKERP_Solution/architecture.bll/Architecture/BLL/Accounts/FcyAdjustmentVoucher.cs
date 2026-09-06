using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class FcyAdjustmentVoucher
{
	public static Architecture.Model.Accounts.VoucherHead MakeVoucher(Architecture.Model.Accounts.FcyAdjustmentVoucher obj)
	{
		Architecture.Model.Accounts.VoucherHead vh = new Architecture.Model.Accounts.VoucherHead();
		try
		{
			double VoucherAmt = 0.0;
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.DocNo);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = "";
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchId;
			vh.ProjectId = obj.ProjectId;
			vh.ManualBillNo = "";
			vh.DueDate = DateTime.Now;
			vh.DueDays = 0;
			vh.ExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
			vh.FcAmount = obj.FcyTotalAmount;
			vh.MultiCurrencyId = obj.FcyId;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUserId;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUserId;
			vh.PostUser = 0;
			vh.voucherDetailList = new List<VoucherDetail>();
			if (obj.FcyAdjustmentVoucherDetailslist.Count > 0)
			{
				foreach (FcyAdjustmentVoucherDetail r in obj.FcyAdjustmentVoucherDetailslist.ToList())
				{
					string Comments = "";
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = Conversion.ToInt(r.DebitAccountId);
					vd.AgainstAccountId = Conversion.ToInt(r.CreditAccountId);
					double HcyAmount = Conversion.ToDouble(r.FcyAmount) * Conversion.ToDouble(obj.ExchangeRate);
					Comments = ((!(r.DebitRemarks != string.Empty)) ? (r.DebitRemarks + " " + r.CreditAccountTitle + " ExchangeRate " + obj.ExchangeRate + " fcyAmount " + Conversion.ToString(r.FcyAmount)) : (r.CreditAccountTitle + " ExchangeRate " + obj.ExchangeRate + " fcyAmount " + Conversion.ToString(r.FcyAmount)));
					vd.Comments = Conversion.ToString(Comments);
					vd.DebitAmount = HcyAmount;
					vd.DCurrencyAmount = r.FcyAmount;
					vd.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
					vd.DMultiCurrencyId = obj.FcyId;
					vd.CreditAmount = 0.0;
					VoucherAmt += vd.DebitAmount;
					vh.voucherDetailList.Add(vd);
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = Conversion.ToInt(r.CreditAccountId);
					vd2.AgainstAccountId = Conversion.ToInt(r.DebitAccountId);
					Comments = ((!(r.CreditRemarks != string.Empty)) ? (r.CreditRemarks + "  " + r.DebitAccountTitle + " ExchangeRate " + obj.ExchangeRate + " fcyAmount " + Conversion.ToString(r.FcyAmount)) : (r.DebitAccountTitle + " ExchangeRate " + obj.ExchangeRate + " fcyAmount " + Conversion.ToString(r.FcyAmount)));
					vd2.Comments = Conversion.ToString(Comments);
					vd2.CreditAmount = HcyAmount;
					vd2.DCurrencyAmount = r.FcyAmount;
					vd2.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
					vd2.DMultiCurrencyId = obj.FcyId;
					vd2.DebitAmount = 0.0;
					vh.voucherDetailList.Add(vd2);
				}
				vh.VoucherAmount = VoucherAmt;
				vh.BillAmount = VoucherAmt;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return vh;
	}

	public static int Save(Architecture.Model.Accounts.FcyAdjustmentVoucher obj)
	{
		try
		{
			obj.EnteyDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Accounts.FcyAdjustmentVoucher().SetData(obj, "Sp_FcyAdjustmentVoucher_Insert");
			}
			return new Architecture.DAL.Accounts.FcyAdjustmentVoucher().SetData(obj, "Sp_FcyAdjustmentVoucher_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Accounts.FcyAdjustmentVoucher obj)
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
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.Accounts.FcyAdjustmentVoucher>("Sp_FcyAdjustmentVoucher_GetAllMethod", p).Rows[0]["DocNo"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Accounts.FcyAdjustmentVoucher GetByID(int Id)
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
			return new Architecture.DAL.Accounts.FcyAdjustmentVoucher().GetData("Sp_FcyAdjustmentVoucher_GetAllMethod", p)[0];
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
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.FcyAdjustmentVoucher>("Sp_FcyAdjustmentVoucher_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FcyAdjustmentSlipAndRegister(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherDateF",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherDateT",
					Value = obj.ToDate
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherCodeF",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherCodeT",
					Value = obj.ToDocNo
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
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProjectsId",
					Value = obj.ProjectsId
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DebitAccountId",
					Value = obj.AccountId
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DebitAccountId",
					Value = obj.AccountId
				});
			}
			if (obj.CreditAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CreditAccountId",
					Value = obj.CreditAccountId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.FcyAdjustmentVoucher>("USp_FcyAdjustmentVouchers_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
