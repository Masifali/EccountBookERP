using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.Export;

public class ExImInvoice
{
	public int SetData(Architecture.Model.Export.ExImInvoice obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			else
			{
				AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Attachment Folder Path");
				if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0 && !Directory.Exists(AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				obj.AttachmentsList = CommonServices.CommonMethodForAttachmentsToPhysicallySave(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.AttachmentsList, copiedFiles);
				obj.AttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.Attachment));
				obj.CustomAttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.UploadedFileCustomName));
			}
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
				foreach (ExImInvoicePackingDetail item13 in obj.ExImInvoiceDetail)
				{
					LineIdForPDI2 = (item13.LineId = LineIdForPDI2 + 1);
					item13.ExImInvoiceId = obj.Id;
					int code7 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item13, "Sp_ExImInvoicePackingDetail_Insert"));
					item13.Id = code7;
				}
			}
			if (obj.ExImInvoicePaymentTermsDetail != null)
			{
				foreach (ExImInvoicePaymentTermsDetail item12 in obj.ExImInvoicePaymentTermsDetail)
				{
					item12.ExImInvoiceId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item12, "Sp_ExImInvoicePaymentTermsDetail_Insert"));
				}
			}
			if (obj.ExImInvoiceOtherChargesDetailList != null)
			{
				foreach (ExImInvoiceOtherChargesDetail item11 in obj.ExImInvoiceOtherChargesDetailList)
				{
					item11.ExImInvoiceId = obj.Id;
					int code6 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item11, "USP_ExImInvoiceOtherChargesDetail_Insert"));
					item11.Id = code6;
				}
			}
			if (obj.ExImInvoiceOtherItemslist != null)
			{
				foreach (ExImInvoiceOtherItems item10 in obj.ExImInvoiceOtherItemslist)
				{
					item10.ExImInvoiceId = obj.Id;
					int code5 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "Sp_ExImInvoiceOtherItems_Insert"));
					item10.Id = code5;
				}
			}
			if (obj.ExImInvoiceDetailCustom != null)
			{
				int LineIdForPDI = 0;
				foreach (ExImInvoicePackingDetailCustom item9 in obj.ExImInvoiceDetailCustom)
				{
					LineIdForPDI = (item9.LineId = LineIdForPDI + 1);
					item9.ExImInvoiceId = obj.Id;
					int code4 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "USP_ExImInvoicePackingDetailCustom_Insert"));
					item9.Id = code4;
				}
			}
			if (obj.ExImInvoicePaymentTermsDetailCustom != null)
			{
				foreach (ExImInvoicePaymentTermsDetailCustom item8 in obj.ExImInvoicePaymentTermsDetailCustom)
				{
					item8.ExImInvoiceId = obj.Id;
					int code3 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "USP_ExImInvoicePaymentTermsDetailCustom_Insert"));
					item8.Id = code3;
				}
			}
			if (obj.ExImInvoiceOtherChargesDetailCustomList != null)
			{
				foreach (ExImInvoiceOtherChargesDetailCustom item7 in obj.ExImInvoiceOtherChargesDetailCustomList)
				{
					item7.ExImInvoiceId = obj.Id;
					int code2 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "USP_ExImInvoiceOtherChargesDetailCustom_Insert"));
					item7.Id = code2;
				}
			}
			if (obj.ExImInvoiceSaleManCommissionList != null && obj.ExImInvoiceSaleManCommissionList.Count > 0)
			{
				foreach (ExImInvoiceSaleManCommission item6 in obj.ExImInvoiceSaleManCommissionList)
				{
					item6.exImInvoiceId = obj.Id;
					int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "USP_exImInvoiceSaleManCommission_Insert"));
					item6.exImInvoiceSaleManCommissionId = code;
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = obj.AttachmentsList[0].ScreenName
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = returnID
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "DeleteById"
				});
				new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
				foreach (DMSAttachments item5 in obj.AttachmentsList.ToList())
				{
					item5.RefAccountId = obj.SupplierCustomerId;
					item5.RefDocumentTypeId = obj.DocumentTypeId;
					item5.RefDocumentNo = returnID;
					item5.EntryDate = DateTime.Now;
					item5.ModifyDate = DateTime.Now;
					item5.EntryUser = obj.EntryUser;
					item5.ModifyUser = obj.ModifyUser;
					item5.OrganizationId = obj.OrganizationId;
					item5.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Proc_DMSAttachments_Insert"));
				}
			}
			if (obj.DocumentTypeId == 204)
			{
				List<int> uniqueScheduleIds = (from x in obj.ExImInvoiceDetail
					where x.ActionTypeId != 3
					select x.ContractScheduleId).Distinct().ToList();
				if (uniqueScheduleIds != null && uniqueScheduleIds.Count > 0)
				{
					foreach (int item4 in uniqueScheduleIds)
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
						sqlCommand.Parameters.AddWithValue("@ScheduleId", item4);
						sqlCommand.ExecuteNonQuery();
					}
				}
			}
			if (obj.VoucherHeadExImInvoices != null && obj.VoucherHeadExImInvoices.voucherDetailList.Count > 0)
			{
				DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
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
				foreach (VoucherDetail item3 in obj.VoucherHeadExImInvoices.voucherDetailList)
				{
					item3.VoucherHeadId = obj.VoucherHeadExImInvoices.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_VoucherDetail_Insert"));
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
				foreach (VoucherDetail item2 in obj.VoucherHeadExImInvoices.voucherDetailList)
				{
					item2.VoucherHeadId = obj.VoucherHeadExImInvoices.Id;
					item2.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_H_Insert"));
				}
			}
			if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
			{
				CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex;
		}
	}

	public int SetDataForPackingListDetail(Architecture.Model.Export.ExImInvoice obj)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = obj.Id;
			if (obj.ExImInvoicePackingListDetailList != null)
			{
				foreach (ExImInvoicePackingListDetail item in obj.ExImInvoicePackingListDetailList)
				{
					item.ExImInvoiceId = obj.Id;
					int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "USP_ExImInvoicePackingListDetail_Insert"));
					item.Id = code;
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

	public int SetDataForPaymentTermsDetail(Architecture.Model.Export.ExImInvoice obj)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = 0;
			if (obj.ExImInvoicePaymentTermsDetail != null)
			{
				foreach (ExImInvoicePaymentTermsDetail item in obj.ExImInvoicePaymentTermsDetail)
				{
					returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_ExImInvoicePaymentTermsDetail_Insert"));
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

	public List<Architecture.Model.Export.ExImInvoice> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExImInvoice> lst = new List<Architecture.Model.Export.ExImInvoice>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.Get<Architecture.Model.Export.ExImInvoice>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImInvoice item10 in lst)
					{
						List<SqlParameter> lstPrm10 = new List<SqlParameter>();
						lstPrm10.Add(new SqlParameter("@Id", item10.Id));
						lstPrm10.Add(new SqlParameter("@Activity", "ReadExImInvoicePackingDetailByHeaderId"));
						item10.ExImInvoiceDetail = GenericProvider.Get<ExImInvoicePackingDetail>(sqlConnection2, "Sp_ExImInvoice_GetAllMethod", lstPrm10);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImInvoice item9 in lst)
					{
						List<SqlParameter> lstPrm9 = new List<SqlParameter>();
						lstPrm9.Add(new SqlParameter("@Id", item9.Id));
						lstPrm9.Add(new SqlParameter("@Activity", "ReadExImInvoicePaymentTermsDetailByHeaderId"));
						item9.ExImInvoicePaymentTermsDetail = GenericProvider.Get<ExImInvoicePaymentTermsDetail>(sqlConnection3, "Sp_ExImInvoice_GetAllMethod", lstPrm9);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImInvoice item8 in lst)
					{
						List<SqlParameter> lstPrm8 = new List<SqlParameter>();
						lstPrm8.Add(new SqlParameter("@Id", item8.Id));
						lstPrm8.Add(new SqlParameter("@Activity", "ReadExImInvoiceOtherItemsByHeaderId"));
						item8.ExImInvoiceOtherItemslist = GenericProvider.Get<ExImInvoiceOtherItems>(sqlConnection4, "Sp_ExImInvoice_GetAllMethod", lstPrm8);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImInvoice item7 in lst)
					{
						List<SqlParameter> lstPrm7 = new List<SqlParameter>();
						lstPrm7.Add(new SqlParameter("@Id", item7.Id));
						lstPrm7.Add(new SqlParameter("@Activity", "ExImInvoiceOtherChargesDetail_ReadByHeaderId"));
						item7.ExImInvoiceOtherChargesDetailList = GenericProvider.Get<ExImInvoiceOtherChargesDetail>(sqlConnection5, "Sp_ExImInvoice_GetAllMethod", lstPrm7);
					}
				}
			}
			using (SqlConnection sqlConnection6 = ConnectionObject.SQL())
			{
				sqlConnection6.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImInvoice item6 in lst)
					{
						List<SqlParameter> lstPrm6 = new List<SqlParameter>();
						lstPrm6.Add(new SqlParameter("@Id", item6.Id));
						lstPrm6.Add(new SqlParameter("@Activity", "ExImInvoicePackingListDetail_ReadByHeaderId"));
						item6.ExImInvoicePackingListDetailList = GenericProvider.Get<ExImInvoicePackingListDetail>(sqlConnection6, "Sp_ExImInvoice_GetAllMethod", lstPrm6);
					}
				}
			}
			using (SqlConnection sqlConnection7 = ConnectionObject.SQL())
			{
				sqlConnection7.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImInvoice item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@Id", item5.Id));
						item5.ExImPackingListDetailList = GenericProvider.Get<ExImPackingListDetail>(sqlConnection7, "[dbo].[USP_ExImPackingListDetail_ReadById]", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection8 = ConnectionObject.SQL())
			{
				sqlConnection8.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImInvoice item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "EximInvoiceCommissionInfoByReadByHeaderId"));
						item4.ExImInvoiceSaleManCommissionList = GenericProvider.Get<ExImInvoiceSaleManCommission>(sqlConnection8, "Sp_ExImInvoice_GetAllMethod", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection9 = ConnectionObject.SQL())
			{
				sqlConnection9.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImInvoice item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ExImInvoicePackingDetailCustom_ReadByHeaderId"));
						item3.ExImInvoiceDetailCustom = GenericProvider.Get<ExImInvoicePackingDetailCustom>(sqlConnection9, "Sp_ExImInvoice_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection10 = ConnectionObject.SQL())
			{
				sqlConnection10.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImInvoice item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ExImInvoiceOtherChargesDetailCustom_ReadByHeaderId"));
						item2.ExImInvoiceOtherChargesDetailCustomList = GenericProvider.Get<ExImInvoiceOtherChargesDetailCustom>(sqlConnection10, "Sp_ExImInvoice_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Export.ExImInvoice item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ExImInvoicePaymentTermsDetailCustom_ReadByHeaderId"));
					item.ExImInvoicePaymentTermsDetailCustom = GenericProvider.Get<ExImInvoicePaymentTermsDetailCustom>(sqlcon, "Sp_ExImInvoice_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public static int SetDataForCommercialInvoicePackingList(ExImPackingListDetail obj, int OrganizationId, int CompanyId, int DocumentTypeId, int InvoiceId, DateTime InvoiceDate, int DocNo, int SupplierCustomerId, int BranchesId, int UserId, double ExchangeRate, int FinancialYearId)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		VoucherHead vh = new VoucherHead();
		vh.voucherDetailList = new List<VoucherDetail>();
		Architecture.Model.Export.ExImInvoice objinv = new Architecture.Model.Export.ExImInvoice();
		List<SqlParameter> p = new List<SqlParameter>();
		p.Add(new SqlParameter
		{
			ParameterName = "@Id",
			Value = InvoiceId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@Activity",
			Value = "ReadById"
		});
		List<Architecture.Model.Export.ExImInvoice> lst = new List<Architecture.Model.Export.ExImInvoice>();
		lst = new ExImInvoice().GetData("Sp_ExImInvoice_GetAllMethod", p);
		if (lst.Count > 0)
		{
			objinv = lst[0];
			List<SupplierCustomerGlAccountId> find2 = (from c in CommonServices.GetSupplierCustomerListForFinancialEffects(OrganizationId, CompanyId)
				where c.Id == Conversion.ToInt(objinv.SupplierCustomerId)
				select c).ToList();
			if (find2.Count > 0)
			{
				vh.RefAccountId = Conversion.ToInt(find2[0].GlAccountId);
			}
			vh.OrganizationId = objinv.OrganizationId;
			vh.CompanyId = objinv.CompanyId;
			vh.DocumentTypeId = objinv.DocumentTypeId;
			vh.DocumentTypeSrNo = objinv.Id;
			vh.RefDocNoId = objinv.Id;
			vh.VoucherDate = objinv.DocDate;
			vh.VoucherCode = objinv.DocCode;
			vh.BranchId = objinv.BranchesId;
			vh.FinancialYearId = objinv.FinancialYearId;
			vh.ModifyUser = UserId;
			vh.ModifyDate = DateTime.Now;
			vh.EntryDate = DateTime.Now;
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.ProjectId = objinv.ProjectsId;
			vh.BillAmount = objinv.TotalAmount;
			vh.ManualBillNo = objinv.InvoiceNo;
			vh.DueDate = objinv.EFormDate;
			vh.DueDays = 0;
			vh.Remarks = Conversion.ToString(objinv.RemarksHeader);
			vh.RemarksOtherLingo = "";
			vh.VoucherAmount = objinv.EquivalentAmount;
			vh.FcAmount = objinv.TotalAmount;
			vh.ExchangeCurrencyRate = objinv.ConversionRate;
			vh.MultiCurrencyId = objinv.FcurrencyId;
		}
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.ExImPackingListDetailList != null && obj.ExImPackingListDetailList.Count > 0)
			{
				int LineIdForPDI = 0;
				foreach (ExImPackingListDetail item6 in obj.ExImPackingListDetailList)
				{
					LineIdForPDI = (item6.SortNo = LineIdForPDI + 1);
					returnID = (item6.Id = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "[dbo].[USP_ExportPackingListDetail_Insert]")));
				}
			}
			if (obj.ExImPackingListDetailList != null && obj.ExImPackingListDetailList.Count > 0)
			{
				bool FIFOCgs = CommonServices.GetERPFeaturesByCompanyId(OrganizationId, CompanyId, 5);
				if (FIFOCgs)
				{
					string RemarksCgs = "";
					List<GetItemGlIdsandItemName> dtitem = (dtitem = CommonServices.GetItemGlIdsandItemName(OrganizationId, CompanyId));
					obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
					foreach (ExImPackingListDetail item5 in obj.ExImPackingListDetailList.ToList())
					{
						List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item5.ItemId)).ToList();
						if (find.Count <= 0)
						{
							continue;
						}
						foreach (InventoryStockEvalautionDetail stockdetail in CommonServices.FIFOImplemention(new ReportsParameters
						{
							OrganizationId = OrganizationId,
							CompanyId = CompanyId,
							ItemId = item5.ItemId,
							WarehouseId = item5.WarehouseId,
							stockUOM = item5.UOMScheduleIdOuter,
							DocDate = InvoiceDate,
							JobLotId = item5.JobLotid,
							CropYearId = item5.InvCropYearId,
							CropYear = item5.CropYear,
							PackingTypeId = item5.ExImPackMaterilaTypeId,
							DocumentTypeId = DocumentTypeId,
							Id = InvoiceId,
							ItemQty = item5.OuterQty,
							NetWeight = item5.NetWeight,
							RateUOM = item5.RateUomId,
							LineId = item5.SortNo,
							ItemName = Conversion.ToString(find[0].ItemName)
						}, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
						{
							RemarksCgs = "ItemQty: " + stockdetail.QtyOut + "   " + Conversion.ToString(find[0].ItemName) + "  Net Weight: " + stockdetail.StockWeightOut + "  CGS Rate:" + stockdetail.CgsRate;
							VoucherDetail vd7 = new VoucherDetail();
							vd7.LineId = item5.SortNo;
							vd7.AccountId = Conversion.ToInt(find[0].COGSGLAC);
							vd7.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd7.Comments = Conversion.ToString(RemarksCgs);
							vd7.DebitAmount = Conversion.ToDouble(stockdetail.CgsAmount);
							vd7.DocumentTypeIdRef = 2;
							vd7.InvoiceNoRefId = InvoiceId;
							vd7.RefInvoiceNo = "CGS";
							vd7.IsCGS = 1;
							vd7.ItemId = Conversion.ToInt(item5.ItemId);
							vd7.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
							vd7.WeightOut = Conversion.ToDouble(stockdetail.StockWeightOut);
							vd7.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
							vd7.ItemAmount = Conversion.ToDouble(stockdetail.AmountOut);
							vd7.JobLotId = Conversion.ToInt(item5.JobLotid);
							vh.voucherDetailList.Add(vd7);
							VoucherDetail vd6 = new VoucherDetail();
							vd6.LineId = item5.SortNo;
							vd6.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd6.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
							vd6.Comments = Conversion.ToString(RemarksCgs);
							vd6.CreditAmount = Conversion.ToDouble(stockdetail.CgsAmount);
							vd6.DocumentTypeIdRef = 2;
							vd6.InvoiceNoRefId = InvoiceId;
							vd6.RefInvoiceNo = "CGS";
							vd6.IsCGS = 1;
							vd6.ItemId = Conversion.ToInt(item5.ItemId);
							vd6.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
							vd6.WeightOut = Conversion.ToDouble(stockdetail.StockWeightOut);
							vd6.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
							vd6.ItemAmount = Conversion.ToDouble(stockdetail.AmountOut);
							vd6.JobLotId = Conversion.ToInt(item5.JobLotid);
							vh.voucherDetailList.Add(vd6);
							obj.InventoryStockEvalautionDetailslist.Add(stockdetail);
						}
					}
					if (obj.InventoryStockEvalautionDetailslist != null && obj.InventoryStockEvalautionDetailslist.Count > 0)
					{
						SqlCommand sqlCommand = new SqlCommand();
						sqlCommand.Connection = sqlcon;
						sqlCommand.Transaction = sqlTrn;
						sqlCommand.CommandType = CommandType.StoredProcedure;
						sqlCommand.CommandText = "[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]";
						sqlCommand.Parameters.AddWithValue("@OrganizationId", OrganizationId);
						sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
						sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", DocumentTypeId);
						sqlCommand.Parameters.AddWithValue("@RefDocIdNo", InvoiceId);
						sqlCommand.ExecuteNonQuery();
						foreach (InventoryStockEvalautionDetail item4 in obj.InventoryStockEvalautionDetailslist)
						{
							item4.OrganizationId = OrganizationId;
							item4.CompanyId = CompanyId;
							item4.DocDate = InvoiceDate;
							item4.DocCodeNo = DocNo;
							item4.SupplierCustomerId = SupplierCustomerId;
							item4.BranchesId = BranchesId;
							item4.RefDocumentTypeId = DocumentTypeId;
							item4.EntryUser = UserId;
							item4.ModifyUser = UserId;
							item4.CalcType = "Weight";
							ExImPackingListDetail sid2 = obj.ExImPackingListDetailList.Find((ExImPackingListDetail x) => x.SortNo == item4.LineId && x.SortNo > 0 && item4.LineId > 0);
							if (sid2 != null)
							{
								double RateUom = CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
								{
									OrganizationId = OrganizationId,
									CompanyId = CompanyId,
									ItemId = item4.ItemId,
									Id = sid2.RateUomId
								});
								if (item4.BillWeightOut > 0.0 && sid2.RatePrice > 0.0 && RateUom > 0.0 && ExchangeRate > 0.0)
								{
									item4.AmountOut = item4.BillWeightOut / RateUom * sid2.RatePrice * ExchangeRate;
									item4.ItemRate = sid2.RatePrice * ExchangeRate;
								}
								item4.RefDocIdNo = InvoiceId;
								item4.RefDocSubIdNo = sid2.Id;
							}
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_InventoryStockEvalautionDetail_Insert"));
						}
					}
				}
				else
				{
					InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
					inventoryTransactions.OrganizationId = OrganizationId;
					inventoryTransactions.CompanyId = CompanyId;
					inventoryTransactions.RefDocumentTypeId = DocumentTypeId;
					inventoryTransactions.RefDocIdNo = InvoiceId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
				}
				InventoryTransactions transactions = new InventoryTransactions();
				transactions.OrganizationId = OrganizationId;
				transactions.CompanyId = CompanyId;
				transactions.RefDocumentTypeId = DocumentTypeId;
				transactions.RefDocIdNo = InvoiceId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
				foreach (ExImPackingListDetail item2 in obj.ExImPackingListDetailList)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "USP_InventoryValidation";
					sqlCommand2.Parameters.AddWithValue("@OrganizationId", OrganizationId);
					sqlCommand2.Parameters.AddWithValue("@CompanyId", CompanyId);
					sqlCommand2.Parameters.AddWithValue("@DocumentTypeId", DocumentTypeId);
					sqlCommand2.Parameters.AddWithValue("@DocDate", InvoiceDate);
					sqlCommand2.Parameters.AddWithValue("@ItemId", item2.ItemId);
					sqlCommand2.Parameters.AddWithValue("@WarehouseId", item2.WarehouseId);
					sqlCommand2.Parameters.AddWithValue("@JobLotId", item2.JobLotid);
					sqlCommand2.Parameters.AddWithValue("@CropYear", item2.CropYear);
					sqlCommand2.Parameters.AddWithValue("@InvPackingTypeId", item2.ExImPackMaterilaTypeId);
					sqlCommand2.Parameters.AddWithValue("@PackUomId", item2.UOMScheduleIdOuter);
					sqlCommand2.Parameters.AddWithValue("@NetWeight", item2.NetWeight);
					sqlCommand2.ExecuteNonQuery();
				}
				if (FIFOCgs)
				{
					obj.VoucherHeadInvoices = vh;
					DataTable dtvh = CommonServices.GetVoucherHeadId(OrganizationId, CompanyId, DocumentTypeId, InvoiceId);
					if (dtvh.Rows.Count > 0)
					{
						VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
						obj.VoucherHeadInvoices.Id = VoucherHeadIdGet;
					}
					if (VoucherHeadIdGet <= 0)
					{
						throw new Exception("VoucherHeadId Not Found against this InvoiceId " + InvoiceId);
					}
					obj.VoucherHeadInvoices.DocumentTypeSrNo = InvoiceId;
					obj.VoucherHeadInvoices.RefDocNoId = InvoiceId;
					obj.VoucherHeadInvoices.ActionId = 2;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Update"));
					_ = obj.VoucherHeadInvoices.Id;
					foreach (VoucherDetail item3 in obj.VoucherHeadInvoices.voucherDetailList)
					{
						ExImPackingListDetail sid = obj.ExImPackingListDetailList.Find((ExImPackingListDetail x) => x.SortNo == item3.LineId && x.SortNo > 0 && item3.LineId > 0);
						item3.VoucherHeadId = obj.VoucherHeadInvoices.Id;
						if (sid != null)
						{
							item3.RefDocSubIdNo = sid.Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_VoucherDetail_Insert"));
					}
					if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count == 0)
					{
						throw new Exception("Voucher Detail list Not Found");
					}
					SqlCommand sqlCommand3 = new SqlCommand();
					sqlCommand3.Connection = sqlcon;
					sqlCommand3.Transaction = sqlTrn;
					sqlCommand3.CommandType = CommandType.StoredProcedure;
					sqlCommand3.CommandText = "USP_VoucherBalanceCheck";
					sqlCommand3.Parameters.AddWithValue("@OrganizationId", OrganizationId);
					sqlCommand3.Parameters.AddWithValue("@CompanyId", CompanyId);
					sqlCommand3.Parameters.AddWithValue("@Id", obj.VoucherHeadInvoices.Id);
					sqlCommand3.ExecuteScalar();
					int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_H_Insert"));
					foreach (VoucherDetail item in obj.VoucherHeadInvoices.voucherDetailList)
					{
						item.VoucherHeadId = obj.VoucherHeadInvoices.Id;
						item.DocumentTypeIdRef = Code;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
					}
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

	public int SetDataGdBreakUp(ExImInvoiceBankGDBreakUp obj)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.ExImInvoiceBankGDBreakUpslist != null)
			{
				int InvoiceId = 0;
				int FormDocumentTypeId = 0;
				foreach (ExImInvoiceBankGDBreakUp item in obj.ExImInvoiceBankGDBreakUpslist)
				{
					InvoiceId = item.ExImInvoiceId;
					FormDocumentTypeId = item.FormDocumentTypeId;
					returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "USP_ExImInvoiceBankGDBreakUp_Insert"));
				}
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "USP_ExportGDBreakUpValidation";
				sqlCommand.Parameters.AddWithValue("@Id", InvoiceId);
				sqlCommand.Parameters.AddWithValue("@DocumentTypeId", FormDocumentTypeId);
				sqlCommand.ExecuteNonQuery();
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

	public int SetDataAdvanceUtilize(AdvanceUtilizeagainstGDs obj)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.AdvanceUtilizeagainstGDslist != null)
			{
				foreach (AdvanceUtilizeagainstGDs item2 in obj.AdvanceUtilizeagainstGDslist)
				{
					item2.EntryDate = DateTime.Now;
					item2.ModifyDate = DateTime.Now;
					returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "USP_AdvanceUtilizeagainstGDs_Insert"));
				}
				if (obj.Ids != null && obj.Ids != string.Empty)
				{
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "USP_AdvanceUtilizeagainstGDs_DeleteById";
					sqlCommand.Parameters.AddWithValue("@Ids", obj.Ids);
					sqlCommand.ExecuteNonQuery();
				}
				foreach (AdvanceUtilizeagainstGDs item in obj.AdvanceUtilizeagainstGDslist)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "USP_ExportAdvanceFinancialInstumentUtilizeValidation";
					sqlCommand2.Parameters.AddWithValue("@InvoiceId", item.EximInvoiceId);
					sqlCommand2.Parameters.AddWithValue("@FIId", item.FIId);
					sqlCommand2.Parameters.AddWithValue("@GdId", item.GDId);
					sqlCommand2.Parameters.AddWithValue("@RefDocumentTypeId", item.RefDocumentTypeId);
					sqlCommand2.Parameters.AddWithValue("@FIDocumentTypeId", item.DocumentTypeId);
					sqlCommand2.ExecuteNonQuery();
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

	public List<Architecture.Model.Export.ExImInvoice> GetAllDetail(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExImInvoice> lst = new List<Architecture.Model.Export.ExImInvoice>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Export.ExImInvoice>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
