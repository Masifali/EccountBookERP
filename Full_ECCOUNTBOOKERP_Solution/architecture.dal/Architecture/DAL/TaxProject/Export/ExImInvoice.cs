using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.TaxProject.Common;
using Architecture.Model.TaxProject.Accounts;
using Architecture.Model.TaxProject.Export;

namespace Architecture.DAL.TaxProject.Export;

public class ExImInvoice
{
	public int SetData(Architecture.Model.TaxProject.Export.ExImInvoice obj, string ProcName)
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
			if (obj.ExImInvoiceDetail != null)
			{
				int LineIdForPDI2 = 0;
				foreach (ExImInvoicePackingDetail item11 in obj.ExImInvoiceDetail)
				{
					LineIdForPDI2 = (item11.LineId = LineIdForPDI2 + 1);
					item11.ExImInvoiceId = obj.Id;
					int code7 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item11, "Sp_ExImInvoicePackingDetail_Insert"));
					item11.Id = code7;
				}
			}
			if (obj.ExImInvoicePaymentTermsDetail != null)
			{
				foreach (ExImInvoicePaymentTermsDetail item10 in obj.ExImInvoicePaymentTermsDetail)
				{
					item10.ExImInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "Sp_ExImInvoicePaymentTermsDetail_Insert"));
				}
			}
			if (obj.ExImInvoiceOtherChargesDetailList != null)
			{
				foreach (ExImInvoiceOtherChargesDetail item9 in obj.ExImInvoiceOtherChargesDetailList)
				{
					item9.ExImInvoiceId = obj.Id;
					int code6 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "USP_ExImInvoiceOtherChargesDetail_Insert"));
					item9.Id = code6;
				}
			}
			if (obj.ExImInvoiceOtherItemslist != null)
			{
				foreach (ExImInvoiceOtherItems item8 in obj.ExImInvoiceOtherItemslist)
				{
					item8.ExImInvoiceId = obj.Id;
					int code5 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "Sp_ExImInvoiceOtherItems_Insert"));
					item8.Id = code5;
				}
			}
			if (obj.ExImInvoiceDetailCustom != null)
			{
				int LineIdForPDI = 0;
				foreach (ExImInvoicePackingDetailCustom item7 in obj.ExImInvoiceDetailCustom)
				{
					LineIdForPDI = (item7.LineId = LineIdForPDI + 1);
					item7.ExImInvoiceId = obj.Id;
					int code4 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "USP_ExImInvoicePackingDetailCustom_Insert"));
					item7.Id = code4;
				}
			}
			if (obj.ExImInvoicePaymentTermsDetailCustom != null)
			{
				foreach (ExImInvoicePaymentTermsDetailCustom item6 in obj.ExImInvoicePaymentTermsDetailCustom)
				{
					item6.ExImInvoiceId = obj.Id;
					int code3 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "USP_ExImInvoicePaymentTermsDetailCustom_Insert"));
					item6.Id = code3;
				}
			}
			if (obj.ExImInvoiceOtherChargesDetailCustomList != null)
			{
				foreach (ExImInvoiceOtherChargesDetailCustom item5 in obj.ExImInvoiceOtherChargesDetailCustomList)
				{
					item5.ExImInvoiceId = obj.Id;
					int code2 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "USP_ExImInvoiceOtherChargesDetailCustom_Insert"));
					item5.Id = code2;
				}
			}
			if (obj.ExImInvoiceSaleManCommissionList != null && obj.ExImInvoiceSaleManCommissionList.Count > 0)
			{
				foreach (ExImInvoiceSaleManCommission item4 in obj.ExImInvoiceSaleManCommissionList)
				{
					item4.exImInvoiceId = obj.Id;
					int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_exImInvoiceSaleManCommission_Insert"));
					item4.exImInvoiceSaleManCommissionId = code;
				}
			}
			if (obj.DocumentTypeId == 204)
			{
				List<int> uniqueScheduleIds = (from x in obj.ExImInvoiceDetail
					where x.ActionTypeId != 3
					select x.ContractScheduleId).Distinct().ToList();
				if (uniqueScheduleIds != null && uniqueScheduleIds.Count > 0)
				{
					foreach (int item3 in uniqueScheduleIds)
					{
						SqlCommand sqlCommand = new SqlCommand();
						sqlCommand.Connection = sqlcon;
						sqlCommand.Transaction = sqlTrn;
						sqlCommand.CommandType = CommandType.StoredProcedure;
						sqlCommand.CommandText = "sdt.usp_ShipmentDocumentSchedule_UpdateByScheduleId";
						sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
						sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
						sqlCommand.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
						sqlCommand.Parameters.AddWithValue("@Id", obj.Id);
						sqlCommand.Parameters.AddWithValue("@ScheduleId", item3);
						sqlCommand.ExecuteNonQuery();
					}
				}
			}
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
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadExImInvoices, "Sp_VoucherHead_Insert"));
				}
				else
				{
					obj.VoucherHeadExImInvoices.DocumentTypeSrNo = obj.Id;
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
				foreach (VoucherDetail item2 in obj.VoucherHeadExImInvoices.voucherDetailList)
				{
					item2.VoucherHeadId = obj.VoucherHeadExImInvoices.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
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
