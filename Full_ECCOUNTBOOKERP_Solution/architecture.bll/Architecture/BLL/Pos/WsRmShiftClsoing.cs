using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Pos;
using Architecture.Model.Accounts;
using Architecture.Model.POS;

namespace Architecture.BLL.Pos;

public class WsRmShiftClsoing
{
	public static VoucherHead MakeVoucher(Architecture.Model.POS.WsRmShiftClsoing obj)
	{
		VoucherHead vh = new VoucherHead();
		try
		{
			int FinancialYearId = 0;
			DataTable dt = CommonServies.GetActiveFinancialYear(obj.OrganizationId, obj.CompanyId);
			if (dt.Rows.Count > 0)
			{
				FinancialYearId = Conversion.ToInt(dt.Rows[0]["Id"]);
				vh.DocumentTypeId = 109;
				vh.DocumentTypeSrNo = obj.Id;
				vh.RefDocNoId = obj.Id;
				vh.VoucherCode = Conversion.ToInt(obj.ClosingNo);
				vh.VoucherDate = obj.ClosingDate;
				vh.Remarks = Conversion.ToString(obj.ClosingRemarks);
				vh.RemarksOtherLingo = "";
				vh.ChequeDate = DateTime.Today;
				vh.IncludeWHT = false;
				vh.BranchId = 0;
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
				int CashSaleAccountId = 0;
				string ConfigKey = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "PosCashInHandAc");
				if (ConfigKey != string.Empty)
				{
					CashSaleAccountId = Conversion.ToInt(ConfigKey);
					int CashInHandAcId = 0;
					string CashAc = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CashInHandAc");
					if (CashAc != string.Empty)
					{
						CashInHandAcId = Conversion.ToInt(CashAc);
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(CashInHandAcId);
						vd3.AgainstAccountId = Conversion.ToInt(CashSaleAccountId);
						vd3.Comments = Conversion.ToString(obj.ClosingRemarks);
						vd3.DebitAmount = Conversion.ToDouble(obj.CashSubmitted);
						vd3.CreditAmount = 0.0;
						vh.voucherDetailList.Add(vd3);
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(CashSaleAccountId);
						vd4.AgainstAccountId = Conversion.ToInt(CashInHandAcId);
						vd4.Comments = Conversion.ToString(obj.ClosingRemarks);
						vd4.CreditAmount = Conversion.ToDouble(obj.CashSubmitted);
						vd4.DebitAmount = 0.0;
						vh.voucherDetailList.Add(vd4);
						return vh;
					}
					throw new Exception("Cash In Hand Account Not Map in Configuration Please Check!");
				}
				throw new Exception("CashSale Account Not Map in Configuration Please Check!");
			}
			throw new Exception("Financial Year Not Found Please Check");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static int Save(Architecture.Model.POS.WsRmShiftClsoing obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
				return new Architecture.DAL.Pos.WsRmShiftClsoing().SetData(obj, "Sp_WsRmShiftClsoing_Insert");
			}
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			return new Architecture.DAL.Pos.WsRmShiftClsoing().SetData(obj, "Sp_WsRmShiftClsoing_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.POS.WsRmShiftClsoing GetByID(int Id)
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
			new List<Architecture.Model.POS.WsRmShiftClsoing>();
			return new Architecture.DAL.Pos.WsRmShiftClsoing().GetData("Sp_WsRmShiftClsoing_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.POS.WsRmShiftClsoing obj)
	{
		try
		{
			int DocNo = 0;
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
				Value = "GenerateCode"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmShiftClsoing>("Sp_WsRmShiftClsoing_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				DocNo = Conversion.ToInt(dt.Rows[0]["ClosingNo"]);
			}
			return DocNo;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.POS.WsRmShiftClsoing obj)
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
				ParameterName = "@EntryUserId",
				Value = obj.EntryUserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmShiftClsoing>("Sp_WsRmShiftClsoing_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDateForDayCloasing(Architecture.Model.POS.WsRmShiftClsoing obj)
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
				ParameterName = "@ClosingDate",
				Value = obj.ClosingDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDateForDayCloasing"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmShiftClsoing>("Sp_WsRmShiftClsoing_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
