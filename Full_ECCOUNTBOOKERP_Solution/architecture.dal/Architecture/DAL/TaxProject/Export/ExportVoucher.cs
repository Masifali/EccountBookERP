using System;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.TaxProject.Common;
using Architecture.Model.TaxProject.Accounts;
using Architecture.Model.TaxProject.Export;

namespace Architecture.DAL.TaxProject.Export;

public class ExportVoucher
{
	public int SetDate(Architecture.Model.TaxProject.Export.ExportVoucher obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		using SqlConnection sqlcon = ConnectionObject.SQLForServer();
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
			obj.ExImInvoiceId = obj.Id;
			if (obj.ExportVoucherDetailslist != null)
			{
				int LineId = 1;
				foreach (ExportVoucherDetail item7 in obj.ExportVoucherDetailslist)
				{
					item7.LineId = LineId;
					item7.ExportVoucherId = obj.Id;
					int code5 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "USP_ExportVoucherDetail_Insert"));
					item7.Id = code5;
					LineId++;
				}
			}
			if (obj.ExImInvoicePaymentTermsDetailList != null)
			{
				foreach (ExImInvoicePaymentTermsDetail item6 in obj.ExImInvoicePaymentTermsDetailList)
				{
					item6.ExImInvoiceId = obj.ExImInvoiceId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "Sp_ExImInvoicePaymentTermsDetail_Insert"));
				}
			}
			if (obj.ExImInvoiceOtherChargesDetailList != null)
			{
				foreach (ExImInvoiceOtherChargesDetail item5 in obj.ExImInvoiceOtherChargesDetailList)
				{
					item5.ExImInvoiceId = obj.ExImInvoiceId;
					int code4 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "USP_ExImInvoiceOtherChargesDetail_Insert"));
					item5.Id = code4;
				}
			}
			if (obj.ExImInvoicePaymentTermsDetailListCustom != null)
			{
				foreach (ExImInvoicePaymentTermsDetailCustom item4 in obj.ExImInvoicePaymentTermsDetailListCustom)
				{
					item4.ExImInvoiceId = obj.ExImInvoiceId;
					int code3 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_ExImInvoicePaymentTermsDetailCustom_Insert"));
					item4.Id = code3;
				}
			}
			if (obj.ExImInvoiceOtherChargesDetailListCustom != null)
			{
				foreach (ExImInvoiceOtherChargesDetailCustom item3 in obj.ExImInvoiceOtherChargesDetailListCustom)
				{
					item3.ExImInvoiceId = obj.ExImInvoiceId;
					int code2 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "USP_ExImInvoiceOtherChargesDetailCustom_Insert"));
					item3.Id = code2;
				}
			}
			if (obj.ExImInvoiceSaleManCommissionList != null && obj.ExImInvoiceSaleManCommissionList.Count > 0)
			{
				foreach (ExImInvoiceSaleManCommission item2 in obj.ExImInvoiceSaleManCommissionList)
				{
					item2.exImInvoiceId = obj.ExImInvoiceId;
					int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "USP_exImInvoiceSaleManCommission_Insert"));
					item2.exImInvoiceSaleManCommissionId = code;
				}
			}
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "usp_CommercialInvoiceInsertOrUpdateFromExportVoucher";
			sqlCommand.Parameters.AddWithValue("@Id", obj.Id);
			sqlCommand.ExecuteNonQuery();
			if (obj.VoucherHeadExImInvoices != null && obj.VoucherHeadExImInvoices.voucherDetailList.Count > 0)
			{
				DataTable dtvh = Architecture.DAL.TaxProject.Common.CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
				if (dtvh.Rows.Count > 0)
				{
					VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
					obj.VoucherHeadExImInvoices.Id = VoucherHeadIdGet;
				}
				if (VoucherHeadIdGet == 0)
				{
					obj.VoucherHeadExImInvoices.DocumentTypeSrNo = obj.Id;
					obj.VoucherHeadExImInvoices.RefDocNoId = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadExImInvoices, "Sp_VoucherHead_Insert"));
				}
				else
				{
					obj.VoucherHeadExImInvoices.DocumentTypeSrNo = obj.Id;
					obj.VoucherHeadExImInvoices.RefDocNoId = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadExImInvoices, "Sp_VoucherHead_Update"));
				}
				if (VoucherHeadId > 0)
				{
					obj.VoucherHeadExImInvoices.Id = VoucherHeadId;
				}
				else
				{
					VoucherHeadId = obj.VoucherHeadExImInvoices.Id;
				}
				if (obj.VoucherHeadExImInvoices.voucherDetailList.Count <= 0 || obj.VoucherHeadExImInvoices.voucherDetailList == null)
				{
					throw new Exception("VoucherDetail list Not Found");
				}
				foreach (VoucherDetail item8 in obj.VoucherHeadExImInvoices.voucherDetailList)
				{
					ExportVoucherDetail sid = obj.ExportVoucherDetailslist.Find((ExportVoucherDetail x) => x.LineId == item8.LineId && x.LineId > 0 && item8.LineId > 0);
					if (sid != null)
					{
						item8.RefDocSubIdNo = sid.Id;
					}
					item8.VoucherHeadId = obj.VoucherHeadExImInvoices.Id;
					item8.BranchesId = obj.BranchesId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "Sp_VoucherDetail_Insert"));
				}
				if (obj.VoucherHeadExImInvoices.Id > 0)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "USP_VoucherBalanceCheck";
					sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand2.Parameters.AddWithValue("@Id", obj.VoucherHeadExImInvoices.Id);
					sqlCommand2.ExecuteScalar();
				}
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadExImInvoices, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item in obj.VoucherHeadExImInvoices.voucherDetailList)
				{
					item.VoucherHeadId = obj.VoucherHeadExImInvoices.Id;
					item.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
				}
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
}
