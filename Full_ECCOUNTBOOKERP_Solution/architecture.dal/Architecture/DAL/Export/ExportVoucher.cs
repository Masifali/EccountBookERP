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

public class ExportVoucher
{
	public int SetDate(Architecture.Model.Export.ExportVoucher obj, string ProcName)
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
			if (obj.ExportVoucherDetailslist != null)
			{
				int LineId = 1;
				foreach (ExportVoucherDetail item7 in obj.ExportVoucherDetailslist)
				{
					item7.LineId = LineId;
					item7.ExportVoucherId = obj.Id;
					int code2 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "USP_ExportVoucherDetail_Insert"));
					item7.Id = code2;
					LineId++;
				}
			}
			if (obj.ExImInvoiceSaleManCommissionList != null && obj.ExImInvoiceSaleManCommissionList.Count > 0)
			{
				foreach (ExImInvoiceSaleManCommission item6 in obj.ExImInvoiceSaleManCommissionList)
				{
					item6.exImInvoiceId = obj.ExImInvoiceId;
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
					item5.EntryUser = obj.EntryUserId;
					item5.ModifyUser = obj.ModifyUserId;
					item5.OrganizationId = obj.OrganizationId;
					item5.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Proc_DMSAttachments_Insert"));
				}
			}
			if (CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5))
			{
				List<GetItemGlIdsandItemName> dtitem = (dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				foreach (ExportVoucherDetail item4 in obj.ExportVoucherDetailslist)
				{
					if (item4.EntryTypeId == 1)
					{
						continue;
					}
					List<InventoryStockEvalautionDetail> lstOtherRef = new List<InventoryStockEvalautionDetail>();
					lstOtherRef = CommonServices.GetInventoryStockEvalautionDetailByOtherIdsAndStockIdsForExportInoice(new ReportsParameters
					{
						OrganizationId = obj.OrganizationId,
						CompanyId = obj.CompanyId,
						OtherDocumentTypeId = 205,
						ExImInvoiceId = obj.ExImInvoiceId,
						InvoiceDetailId = item4.ExImInvoiceDetailId,
						ItemId = item4.ItemId,
						ItemUomId = item4.ItemUomId,
						CropYearId = item4.CropYearId,
						JobLotId = item4.LotJobId,
						PackingTypeId = item4.PackingTypeId,
						WarehouseId = item4.WarehouseId
					});
					if (lstOtherRef != null && lstOtherRef.Count > 0)
					{
						string RemarksCgs = "";
						foreach (InventoryStockEvalautionDetail stockdetail in lstOtherRef.ToList())
						{
							List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item4.ItemId)).ToList();
							if (find.Count == 0)
							{
								throw new Exception("ItemId Not Found Against CGS Transaction");
							}
							RemarksCgs = "ItemQty: " + stockdetail.QtyOut + "   " + Conversion.ToString(find[0].ItemName) + "  Net Weight: " + stockdetail.StockWeightOut + "  CGS Rate:" + stockdetail.CgsRate;
							VoucherDetail vd7 = new VoucherDetail();
							vd7.LineId = item4.LineId;
							vd7.IsCGS = 1;
							vd7.AccountId = Conversion.ToInt(find[0].COGSGLAC);
							vd7.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd7.Comments = Conversion.ToString(RemarksCgs);
							vd7.DebitAmount = Conversion.ToDouble(stockdetail.CgsAmount);
							vd7.ItemId = Conversion.ToInt(item4.ItemId);
							vd7.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
							vd7.WeightOut = Conversion.ToDouble(stockdetail.StockWeightOut);
							vd7.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
							vd7.ItemAmount = Conversion.ToDouble(stockdetail.AmountOut);
							vd7.JobLotId = Conversion.ToInt(item4.LotJobId);
							vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vd7.RefDocumentTypeId = Conversion.ToInt(stockdetail.RefRefDocumentTypeId);
							vd7.RefDocNoId = Conversion.ToInt(stockdetail.RefRefDocIdNo);
							vd7.RefDocNoDetailId = Conversion.ToInt(stockdetail.RefRefDocSubIdNo);
							obj.VoucherHeadExImInvoices.voucherDetailList.Add(vd7);
							VoucherDetail vd6 = new VoucherDetail();
							vd6.LineId = item4.LineId;
							vd6.IsCGS = 1;
							vd6.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd6.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
							vd6.Comments = Conversion.ToString(RemarksCgs);
							vd6.CreditAmount = Conversion.ToDouble(stockdetail.CgsAmount);
							vd6.ItemId = Conversion.ToInt(item4.ItemId);
							vd6.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
							vd6.WeightOut = Conversion.ToDouble(stockdetail.StockWeightOut);
							vd6.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
							vd6.JobLotId = Conversion.ToInt(item4.LotJobId);
							vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vd6.RefDocumentTypeId = Conversion.ToInt(stockdetail.RefRefDocumentTypeId);
							vd6.RefDocNoId = Conversion.ToInt(stockdetail.RefRefDocIdNo);
							vd6.RefDocNoDetailId = Conversion.ToInt(stockdetail.RefRefDocSubIdNo);
							obj.VoucherHeadExImInvoices.voucherDetailList.Add(vd6);
							stockdetail.OrganizationId = obj.OrganizationId;
							stockdetail.CompanyId = obj.CompanyId;
							stockdetail.DocDate = stockdetail.DocDate;
							stockdetail.DocCodeNo = stockdetail.DocCodeNo;
							stockdetail.SupplierCustomerId = obj.SupplierCustomerId;
							stockdetail.BranchesId = obj.BranchesId;
							stockdetail.RefDocumentTypeId = obj.DocumentTypeId;
							stockdetail.EntryUser = obj.EntryUserId;
							stockdetail.ModifyUser = obj.ModifyUserId;
							stockdetail.CalcType = "Weight";
							ExportVoucherDetail sid2 = obj.ExportVoucherDetailslist.Find((ExportVoucherDetail x) => x.ExImInvoiceDetailId == stockdetail.InvoiceDetailId && stockdetail.OtherDocumentTypeId == 205);
							if (sid2 != null)
							{
								double RateUom = CommonServices.GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
								{
									OrganizationId = obj.OrganizationId,
									CompanyId = obj.CompanyId,
									ItemId = item4.ItemId,
									Id = sid2.RateUomId
								});
								if (stockdetail.BillWeightOut > 0.0 && sid2.RatePrice > 0m && RateUom > 0.0)
								{
									double PkrRatePerKg = 0.0;
									PkrRatePerKg = Conversion.ToDouble(sid2.RatePrice) / RateUom * Conversion.ToDouble(obj.ExchangeRate);
									stockdetail.AmountOut = stockdetail.BillWeightOut * PkrRatePerKg;
									stockdetail.ItemRate = PkrRatePerKg;
								}
								stockdetail.RefDocIdNo = sid2.ExportVoucherId;
								stockdetail.RefDocSubIdNo = sid2.Id;
							}
							obj.InventoryStockEvalautionDetailslist.Add(stockdetail);
						}
						continue;
					}
					throw new Exception("InventoryStockEvalautionDetailslist not found against CGS other reference");
				}
				if (obj.InventoryStockEvalautionDetailslist != null && obj.InventoryStockEvalautionDetailslist.Count > 0)
				{
					foreach (InventoryStockEvalautionDetail item2 in obj.InventoryStockEvalautionDetailslist.ToList())
					{
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "USP_InventoryStockEvalautionDetailGdnReferences_Update"));
					}
				}
			}
			else
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "usp_StockEvaluationUpdateForExportForwarding";
				sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", obj.DocumentTypeId);
				sqlCommand.Parameters.AddWithValue("@RefDocIdNo", obj.Id);
				sqlCommand.Parameters.AddWithValue("@ActionId", 2);
				sqlCommand.ExecuteNonQuery();
			}
			SqlCommand sqlCommand2 = new SqlCommand();
			sqlCommand2.Connection = sqlcon;
			sqlCommand2.Transaction = sqlTrn;
			sqlCommand2.CommandType = CommandType.StoredProcedure;
			sqlCommand2.CommandText = "usp_StockInTransit_VoucherDelete_ByExportVoucherId";
			sqlCommand2.Parameters.AddWithValue("@Id", obj.Id);
			sqlCommand2.ExecuteNonQuery();
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
					ExportVoucherDetail sid = obj.ExportVoucherDetailslist.Find((ExportVoucherDetail x) => x.LineId == item3.LineId && x.LineId > 0 && item3.LineId > 0);
					if (sid != null)
					{
						item3.RefDocSubIdNo = sid.Id;
					}
					item3.VoucherHeadId = obj.VoucherHeadExImInvoices.Id;
					item3.BranchesId = obj.BranchesId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_VoucherDetail_Insert"));
				}
				if (obj.VoucherHeadExImInvoices.Id > 0)
				{
					SqlCommand sqlCommand3 = new SqlCommand();
					sqlCommand3.Connection = sqlcon;
					sqlCommand3.Transaction = sqlTrn;
					sqlCommand3.CommandType = CommandType.StoredProcedure;
					sqlCommand3.CommandText = "USP_VoucherBalanceCheck";
					sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand3.Parameters.AddWithValue("@Id", obj.VoucherHeadExImInvoices.Id);
					sqlCommand3.ExecuteScalar();
				}
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadExImInvoices, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item in obj.VoucherHeadExImInvoices.voucherDetailList)
				{
					item.VoucherHeadId = obj.VoucherHeadExImInvoices.Id;
					item.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
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

	public List<Architecture.Model.Export.ExportVoucher> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExportVoucher> lst = new List<Architecture.Model.Export.ExportVoucher>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Export.ExportVoucher>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Export.ExportVoucher item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.ExportVoucherDetailslist = GenericProvider.Get<ExportVoucherDetail>(sqlcon, "Sp_ExportVoucher_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public List<Architecture.Model.Export.ExportVoucher> GetDataForUpload(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExportVoucher> lst = new List<Architecture.Model.Export.ExportVoucher>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Export.ExportVoucher>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExportVoucher item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
						item4.ExportVoucherDetailslist = GenericProvider.Get<ExportVoucherDetail>(sqlConnection2, "Sp_ExportVoucher_GetAllMethod", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExportVoucher item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.ExImInvoiceId));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadExImInvoicePaymentTermsDetailByHeaderId"));
						item3.ExImInvoicePaymentTermsDetailList = GenericProvider.Get<ExImInvoicePaymentTermsDetail>(sqlConnection3, "Sp_ExImInvoice_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExportVoucher item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.ExImInvoiceId));
						lstPrm2.Add(new SqlParameter("@Activity", "ExImInvoiceOtherChargesDetail_ReadByHeaderId"));
						item2.ExImInvoiceOtherChargesDetailList = GenericProvider.Get<ExImInvoiceOtherChargesDetail>(sqlConnection4, "Sp_ExImInvoice_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Export.ExportVoucher item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.ExImInvoiceId));
					lstPrm.Add(new SqlParameter("@Activity", "EximInvoiceCommissionInfoByReadByHeaderId"));
					item.ExImInvoiceSaleManCommissionList = GenericProvider.Get<ExImInvoiceSaleManCommission>(sqlcon, "Sp_ExImInvoice_GetAllMethod", lstPrm);
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
