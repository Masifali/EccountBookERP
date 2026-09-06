using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class AccountsPrematurePaymentsReceipts
{
	public static int Save(Architecture.Model.Accounts.AccountsPrematureReceipts obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.AccountsPrematureReceipts().SetData(obj, "");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Accounts.AccountsPrematureReceipts obj)
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
			if (obj.ChartOfAccountIdSender != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SenderGlId",
					Value = obj.ChartOfAccountIdSender
				});
			}
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountsPrematureReceipts>("Sp_AccountsPrematurePaymentsReceipts_GetAllMethod", p);
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
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
			if (obj.RepresentativeAcId != 0)
			{
				p.Add(new SqlParameter("@RepresentativeAcId", obj.RepresentativeAcId));
			}
			if (obj.SenderAcId != 0)
			{
				p.Add(new SqlParameter("@SenderAcId", obj.SenderAcId));
			}
			if (obj.ReceiverAcId != 0)
			{
				p.Add(new SqlParameter("@ReceiverAcId", obj.ReceiverAcId));
			}
			if (obj.VoucherTypeId != 0)
			{
				p.Add(new SqlParameter("@VoucherTypeId", obj.VoucherTypeId));
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter("@SenderPartyId", obj.SupplierCustomerId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter("@FromDocNo", obj.FromDocNo));
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter("@ToDocNo", obj.ToDocNo));
			}
			if (obj.BankId != 0)
			{
				p.Add(new SqlParameter("@BankId", obj.BankId));
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter("@CostCenterId", obj.CostCenterId));
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountsPrematureReceipts>("Sp_AccountsPrematurePaymentsReceipts_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByTrackingNo(string Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@TrackingSlipRef",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByTrackingNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountsPrematureReceipts>("Sp_AccountsPrematurePaymentsReceipts_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByMultiParams(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@TrackingSlipRef",
				Value = obj.TrackingNo
			});
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.DocDate));
			}
			if (obj.VoucherTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherTypeId",
					Value = obj.VoucherTypeId
				});
			}
			if (obj.ReceiverAcId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReceiverAcId",
					Value = obj.ReceiverAcId
				});
			}
			if (obj.SlipAmount != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SlipAmount",
					Value = obj.SlipAmount
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByTrackingNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountsPrematureReceipts>("Sp_AccountsPrematurePaymentsReceipts_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int UpdateRecord(Architecture.Model.Accounts.AccountsPrematureReceipts obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.AccountsPrematureReceipts().SetDataForUpdate(obj, "");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingRecords(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.EntryUser
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AppId",
				Value = obj.AppId
			});
			if (obj.RepresentativeAcId != 0)
			{
				p.Add(new SqlParameter("@RepresentativeAcId", obj.RepresentativeAcId));
			}
			if (obj.SenderAcId != 0)
			{
				p.Add(new SqlParameter("@SenderAcId", obj.SenderAcId));
			}
			if (obj.ReceiverAcId != 0)
			{
				p.Add(new SqlParameter("@ReceiverAcId", obj.ReceiverAcId));
			}
			if (obj.VoucherTypeId != 0)
			{
				p.Add(new SqlParameter("@VoucherTypeId", obj.VoucherTypeId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter("@PageNumber", obj.PageNumber));
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter("@PageSize", obj.PageSize));
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter("@CostCenterId", obj.CostCenterId));
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPendingRecords"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountsPrematureReceipts>("Sp_AccountsPrematurePaymentsReceipts_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDown(ReportsParameters obj)
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
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountsPrematureReceipts>("USP_GetDataForDropDownFromAccountsPrematurePaymentsReceipts", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
