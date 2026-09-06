using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Pos;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.POS;

namespace Architecture.BLL.Pos;

public class WsRmCreditNoteHeader
{
	public static VoucherHead MakeVoucher(Architecture.Model.POS.WsRmCreditNoteHeader obj)
	{
		VoucherHead vh = new VoucherHead();
		try
		{
			int FinancialYearId = 0;
			DataTable dt = CommonServies.GetActiveFinancialYear(obj.OrganizationId, obj.CompanyId);
			if (dt.Rows.Count <= 0)
			{
				throw new Exception("Financial Year Not Found Please Check");
			}
			FinancialYearId = Conversion.ToInt(dt.Rows[0]["Id"]);
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = Conversion.ToInt(obj.Id);
			vh.RefDocNoId = Conversion.ToInt(obj.Id);
			vh.VoucherCode = Conversion.ToInt(obj.DocNo);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = Conversion.ToString(obj.RemarksHeader);
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = 0;
			vh.DueDate = DateTime.Now;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = FinancialYearId;
			vh.EntryUser = obj.EntryUserId;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUserId;
			vh.voucherDetailList = new List<VoucherDetail>();
			if (obj.WsRmCreditNoteDetailsList.Count > 0)
			{
				_ = string.Empty;
				foreach (WsRmCreditNoteDetail item in obj.WsRmCreditNoteDetailsList.ToList())
				{
					VoucherDetail vd2 = new VoucherDetail();
					VoucherDetail vd3 = new VoucherDetail();
					vd3.AccountId = Conversion.ToInt(obj.DebitAccountId);
					vd3.AgainstAccountId = Conversion.ToInt(item.CreditAccountId);
					vd3.Comments = Conversion.ToString(item.RemarksDetail);
					vd3.DebitAmount = Conversion.ToDouble(item.IncentiveAmount);
					vh.voucherDetailList.Add(vd3);
					vd2.AccountId = Conversion.ToInt(item.CreditAccountId);
					vd2.AgainstAccountId = Conversion.ToInt(obj.DebitAccountId);
					vd2.Comments = Conversion.ToString(item.RemarksDetail);
					vd2.CreditAmount = Conversion.ToDouble(item.IncentiveAmount);
					vh.voucherDetailList.Add(vd2);
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		return vh;
	}

	public static long Save(Architecture.Model.POS.WsRmCreditNoteHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0L)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
				return new Architecture.DAL.Pos.WsRmCreditNoteHeader().SetData(obj, "Sp_WsRmCreditNoteHeader_Insert");
			}
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			return new Architecture.DAL.Pos.WsRmCreditNoteHeader().SetData(obj, "Sp_WsRmCreditNoteHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.POS.WsRmCreditNoteHeader obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmSalesOrder>("Sp_WsRmCreditNoteHeader_GetAllMethod", p);
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

	public static Architecture.Model.POS.WsRmCreditNoteHeader GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			new List<Architecture.Model.POS.WsRmCreditNoteHeader>();
			return new Architecture.DAL.Pos.WsRmCreditNoteHeader().GetData("Sp_WsRmCreditNoteHeader_GetAllMethod", p)[0];
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
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmCreditNoteHeader>("Sp_WsRmCreditNoteHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
