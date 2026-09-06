using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Accounts;

public class PaymentByInvoiceHeader : DALBase
{
	public int SetDate(Architecture.Model.Accounts.PaymentByInvoiceHeader obj, string ProcName)
	{
		new List<SupplierCustomer>();
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (PaymentByInvoiceDetail item4 in obj.PaymentByInvoiceDetailList)
			{
				item4.PaymentByInvoiceHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_PaymentByInvoiceDetail_Insert"));
			}
			foreach (PaymentByInvoiceOtherDetail item3 in obj.PaymentByInvoiceOtherDetailList)
			{
				item3.PaymentByInvoiceHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_PaymentByInvoiceOtherDetail_Insert"));
			}
			List<SqlParameter> s = new List<SqlParameter>();
			s.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId"
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeSrNo",
				Value = obj.Id
			});
			DataTable dtvh = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", s);
			if (dtvh.Rows.Count > 0)
			{
				VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
				obj.VoucherHeadPaymenet.Id = VoucherHeadIdGet;
			}
			if (VoucherHeadIdGet == 0)
			{
				obj.VoucherHeadPaymenet.DocumentTypeSrNo = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadPaymenet, "Sp_VoucherHead_Insert"));
			}
			else
			{
				obj.VoucherHeadPaymenet.DocumentTypeSrNo = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadPaymenet, "Sp_VoucherHead_Update"));
			}
			if (VoucherHeadId > 0)
			{
				obj.VoucherHeadPaymenet.Id = VoucherHeadId;
			}
			else
			{
				VoucherHeadId = obj.VoucherHeadPaymenet.Id;
			}
			if (obj.VoucherHeadPaymenet.voucherDetailList.Count > 0 && obj.VoucherHeadPaymenet.voucherDetailList != null)
			{
				foreach (VoucherDetail item2 in obj.VoucherHeadPaymenet.voucherDetailList)
				{
					item2.VoucherHeadId = obj.VoucherHeadPaymenet.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
				}
			}
			int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadPaymenet, "Sp_VoucherHead_H_Insert"));
			foreach (VoucherDetail item in obj.VoucherHeadPaymenet.voucherDetailList)
			{
				item.VoucherHeadId = obj.VoucherHeadPaymenet.Id;
				item.DocumentTypeIdRef = Code;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Accounts.PaymentByInvoiceHeader> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.PaymentByInvoiceHeader> lst = new List<Architecture.Model.Accounts.PaymentByInvoiceHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Accounts.PaymentByInvoiceHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Accounts.PaymentByInvoiceHeader item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByIdDetail"));
						item2.PaymentByInvoiceDetailList = GenericProvider.Get<PaymentByInvoiceDetail>(sqlConnection2, "Sp_PaymentByInvoiceHeader_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Accounts.PaymentByInvoiceHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByIdOtherDetail"));
					item.PaymentByInvoiceOtherDetailList = GenericProvider.Get<PaymentByInvoiceOtherDetail>(sqlcon, "Sp_PaymentByInvoiceHeader_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}
}
