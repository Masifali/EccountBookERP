using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.ContractorWages;
using Architecture.Model.Accounts;
using Architecture.Model.ContractorWages;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.Production;

namespace Architecture.DAL.Production;

public class InvStockConversion
{
	public int SetData(Architecture.Model.Production.InvStockConversion obj, string ProcName)
	{
		List<GetItemGlIdsandItemName> dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
		VoucherHead vh = new VoucherHead();
		vh.voucherDetailList = new List<VoucherDetail>();
		int returnID = 0;
		int returnVoucherID = 0;
		int VoucherHeadId = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.invStockConversionDetails == null || obj.invStockConversionDetails.Count <= 0)
			{
				throw new Exception("Detail list not found");
			}
			bool ContractWagesChargetoProductForStockConversion = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ContractWagesChargetoProductForStockConversion"));
			bool FIFOCgs = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5);
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			int LineIdForPDI = 0;
			foreach (InvStockConversionPackingMaterial item10 in obj.invStockConversionPackings)
			{
				LineIdForPDI = (item10.LineId = LineIdForPDI + 1);
				item10.InvStockConversionId = obj.Id;
				int code2 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "Sp_InvStockConversionPackingMaterial_Insert"));
				item10.Id = code2;
			}
			foreach (InvStockConversionAddExpense item9 in obj.invStockConversionAddExpenses)
			{
				item9.InvStockConversionId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "Sp_InvStockConversionAddExpense_Insert"));
			}
			bool InventoryFinancialsEffectsInActive = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "InventoryFinancialsEffectsInActive"));
			if (!FIFOCgs && Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "AllowCoaAccountForJobLots")) && (from x in obj.invStockConversionDetails
				group x by x.JobLotId).Any((IGrouping<int, InvStockConversionDetail> g) => g.Select((InvStockConversionDetail x) => x.EntryType).Distinct().Count() > 1))
			{
				InventoryFinancialsEffectsInActive = true;
			}
			int LineId = 1;
			foreach (InvStockConversionDetail item8 in obj.invStockConversionDetails)
			{
				item8.LineId = LineId;
				item8.InvStockConversionId = obj.Id;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "Sp_InvStockConversionDetail_Insert"));
				item8.Id = code;
				LineId++;
			}
			if (obj.DocTypeId == 66)
			{
				if (obj.ActionId == 1)
				{
					InventoryStockEvalautionDetail inventoryTransactions2 = new InventoryStockEvalautionDetail();
					inventoryTransactions2.OrganizationId = obj.OrganizationId;
					inventoryTransactions2.CompanyId = obj.CompanyId;
					inventoryTransactions2.RefDocumentTypeId = obj.DocTypeId;
					inventoryTransactions2.RefDocIdNo = returnID;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions2, "Sp_InventoryStockEvalautionDetail_Update"));
				}
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "Sp_InventoryStockEvalautionDetail_Insert_InvStockConversionPackingMaterial";
				sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand.Parameters.AddWithValue("@DocumentTypeId", obj.DocTypeId);
				sqlCommand.Parameters.AddWithValue("@Id", obj.Id);
				sqlCommand.ExecuteNonQuery();
			}
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			if (obj.DocTypeId == 67 || obj.DocTypeId == 808)
			{
				InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
				inventoryTransactions.OrganizationId = obj.OrganizationId;
				inventoryTransactions.CompanyId = obj.CompanyId;
				inventoryTransactions.RefDocumentTypeId = obj.DocTypeId;
				inventoryTransactions.RefDocIdNo = returnID;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			}
			if (obj.DocTypeId == 66)
			{
				if (obj.InputDetailRowsRemoveIds != null && obj.InputDetailRowsRemoveIds != string.Empty)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "[dbo].[USP_InvStockConversionDetailRowsDeleteByIds]";
					sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand2.Parameters.AddWithValue("@DocumentTypeId", obj.DocTypeId);
					sqlCommand2.Parameters.AddWithValue("@Id", obj.Id);
					sqlCommand2.Parameters.AddWithValue("@DetailIds", obj.InputDetailRowsRemoveIds);
					sqlCommand2.ExecuteScalar();
				}
				SqlCommand sqlCommand3 = new SqlCommand();
				sqlCommand3.Connection = sqlcon;
				sqlCommand3.Transaction = sqlTrn;
				sqlCommand3.CommandType = CommandType.StoredProcedure;
				sqlCommand3.CommandText = "usp_StockConversionValidation";
				sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand3.Parameters.AddWithValue("@DocumentTypeId", obj.DocTypeId);
				sqlCommand3.Parameters.AddWithValue("@Id", obj.Id);
				sqlCommand3.ExecuteNonQuery();
			}
			foreach (InvStockConversionDetail item7 in obj.invStockConversionDetails)
			{
				if (item7.EntryType == "Issue" || item7.EntryType == "1")
				{
					SqlCommand sqlCommandInv = new SqlCommand
					{
						Connection = sqlcon,
						Transaction = sqlTrn,
						CommandType = CommandType.StoredProcedure,
						CommandText = "USP_InventoryValidation"
					};
					sqlCommandInv.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommandInv.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommandInv.Parameters.AddWithValue("@DocumentTypeId", obj.DocTypeId);
					sqlCommandInv.Parameters.AddWithValue("@DocDate", obj.DocDate);
					sqlCommandInv.Parameters.AddWithValue("@ItemId", item7.ItemId);
					sqlCommandInv.Parameters.AddWithValue("@WarehouseId", item7.WarehouseId);
					sqlCommandInv.Parameters.AddWithValue("@JobLotId", item7.JobLotId);
					sqlCommandInv.Parameters.AddWithValue("@CropYear", item7.CropBatch);
					sqlCommandInv.Parameters.AddWithValue("@InvPackingTypeId", item7.PackingtypeId);
					sqlCommandInv.Parameters.AddWithValue("@PackUomId", item7.ItemUomId);
					sqlCommandInv.Parameters.AddWithValue("@RefDocumentTypeId", item7.RefDocumentTypeId);
					sqlCommandInv.Parameters.AddWithValue("@RefDocNoId", item7.RefDocNoId);
					sqlCommandInv.Parameters.AddWithValue("@RefDocSubIdNo", item7.RefDocSubId);
					sqlCommandInv.Parameters.AddWithValue("@NetWeight", item7.Weight);
					if (obj.DocTypeId == 67 || obj.DocTypeId == 808)
					{
						sqlCommandInv.Parameters.AddWithValue("@ItemConditionId", item7.ItemConditionId);
					}
					sqlCommandInv.Parameters.AddWithValue("@RackId", item7.RackId);
					sqlCommandInv.ExecuteNonQuery();
				}
			}
			if (obj.invStockConversionPackings.Count > 0)
			{
				foreach (InvStockConversionPackingMaterial item6 in obj.invStockConversionPackings)
				{
					SqlCommand sqlCommand4 = new SqlCommand();
					sqlCommand4.Connection = sqlcon;
					sqlCommand4.Transaction = sqlTrn;
					sqlCommand4.CommandType = CommandType.StoredProcedure;
					sqlCommand4.CommandText = "USP_InventoryValidation";
					sqlCommand4.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand4.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand4.Parameters.AddWithValue("@DocDate", obj.DocDate);
					sqlCommand4.Parameters.AddWithValue("@ItemId", item6.ItemId);
					sqlCommand4.Parameters.AddWithValue("@WarehouseId", item6.WarehouseId);
					sqlCommand4.Parameters.AddWithValue("@NetWeight", item6.ItemQty);
					sqlCommand4.Parameters.AddWithValue("@ItemConditionId", item6.ItemConditionId);
					sqlCommand4.Parameters.AddWithValue("@RackId", item6.RackId);
					sqlCommand4.Parameters.AddWithValue("@Activity", "PM");
					sqlCommand4.ExecuteNonQuery();
				}
			}
			if (!InventoryFinancialsEffectsInActive)
			{
				double num2 = obj.invStockConversionDetails.Where((InvStockConversionDetail x) => x.EntryType == "Issue").Sum((InvStockConversionDetail x) => x.Amount);
				double OutValue = obj.invStockConversionDetails.Where((InvStockConversionDetail x) => x.EntryType != "Issue").Sum((InvStockConversionDetail x) => x.Amount);
				double DiffValue = num2 - OutValue;
				DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocTypeId, obj.Id);
				if (dtvh.Rows.Count > 0)
				{
					VoucherHeadId = (vh.Id = Conversion.ToInt(dtvh.Rows[0]["Id"]));
				}
				vh.DocumentTypeId = obj.DocTypeId;
				vh.DocumentTypeSrNo = returnID;
				vh.VoucherCode = obj.DocSrNo;
				vh.VoucherDate = obj.DocDate;
				vh.Remarks = obj.Remarks;
				vh.RemarksOtherLingo = "";
				vh.FinancialYearId = obj.FinancialYearId;
				vh.CheqId = 0;
				vh.ChequeNo = string.Empty;
				vh.ChequeDate = DateTime.Today;
				vh.PayTitle = "";
				vh.Remarks = "";
				vh.BankBranch = "";
				vh.ChequePrintId = 0;
				vh.Source = "";
				vh.DrCrNoteType = "";
				vh.IsApproved = false;
				vh.EntryDate = DateTime.Now;
				vh.EntryUser = obj.EntryUser;
				vh.ModifyUser = obj.ModifyUser;
				vh.ModifyDate = DateTime.Now;
				vh.PostDate = DateTime.Now;
				vh.PostUser = 0;
				vh.PostState = false;
				vh.OrganizationId = obj.OrganizationId;
				vh.CompanyId = obj.CompanyId;
				vh.IncludeWHT = false;
				vh.BranchId = 0;
				vh.ProjectId = 0;
				vh.BillAmount = 0.0;
				vh.ManualBillNo = obj.ProductionNo;
				vh.DueDate = obj.DocDate;
				vh.DueDays = 0;
				if (obj.invStockConversionDetails.Count > 0)
				{
					List<GetJobLotGlIdsandName> lstJobLot = CommonServices.GetJobLotGlIdsandName(obj.OrganizationId, obj.CompanyId);
					foreach (InvStockConversionDetail item4 in obj.invStockConversionDetails.ToList())
					{
						string Remarks2 = string.Empty;
						if (dtitem.Count > 0)
						{
							List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item4.ItemId)).ToList();
							if (find2.Count <= 0)
							{
								continue;
							}
							List<GetJobLotGlIdsandName> findJob = lstJobLot.Where((GetJobLotGlIdsandName x) => x.Id == Conversion.ToInt(item4.JobLotId)).ToList();
							if (item4.EntryType == "Issue" || item4.EntryType == "1")
							{
								string obj2 = (string.IsNullOrWhiteSpace(item4.Remarks) ? "" : (item4.Remarks + ", "));
								string remarksDetails = $"ItemName: {find2[0].ItemName}, Qty: {item4.Qty}";
								if (obj.DocTypeId == 66)
								{
									remarksDetails += $", Weight: {item4.Weight}";
								}
								remarksDetails += $", Rate: {item4.Rate}, ItemAmount: {item4.Amount}";
								Remarks2 = obj2 + remarksDetails;
								VoucherDetail vd5 = new VoucherDetail();
								vd5.LineId = item4.LineId;
								if (findJob != null && findJob.Count > 0 && findJob != null && findJob[0].AccountId > 0)
								{
									vd5.AccountId = Conversion.ToInt(findJob[0].AccountId);
								}
								else
								{
									vd5.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
								}
								vd5.AgainstAccountId = vd5.AccountId;
								vd5.DocumentTypeIdRef = item4.RefDocumentTypeId;
								vd5.InvoiceNoRefId = item4.RefDocNoId;
								vd5.RefInvoiceNo = Conversion.ToString(item4.RefDocSubId);
								vd5.Comments = Conversion.ToString(Remarks2);
								vd5.DebitAmount = Conversion.ToDouble(0);
								vd5.CreditAmount = Conversion.ToDouble(item4.Amount);
								vd5.QtyOut = Conversion.ToDouble(item4.Qty);
								vd5.WeightOut = Conversion.ToDouble(item4.Weight);
								vd5.ItemRate = Conversion.ToDouble(item4.Rate);
								vd5.ItemAmount = Conversion.ToDouble(item4.Amount);
								vd5.ItemId = item4.ItemId;
								vd5.JobLotId = item4.JobLotId;
								vd5.BranchesId = obj.BranchedId;
								vd5.IsCGS = 1;
								vh.voucherDetailList.Add(vd5);
							}
							else
							{
								VoucherDetail vd6 = new VoucherDetail();
								vd6.LineId = item4.LineId;
								if (findJob != null && findJob.Count > 0 && findJob != null && findJob[0].AccountId > 0)
								{
									vd6.AccountId = Conversion.ToInt(findJob[0].AccountId);
								}
								else
								{
									vd6.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
								}
								vd6.AgainstAccountId = vd6.AccountId;
								vd6.Comments = Conversion.ToString(Remarks2);
								vd6.CreditAmount = Conversion.ToDouble(0);
								vd6.DebitAmount = Conversion.ToDouble(item4.Amount);
								vd6.QtyIn = Conversion.ToDouble(item4.Qty);
								vd6.WeightIn = Conversion.ToDouble(item4.Weight);
								vd6.ItemRate = Conversion.ToDouble(item4.Rate);
								vd6.ItemAmount = Conversion.ToDouble(item4.Amount);
								vd6.BranchesId = obj.BranchedId;
								vd6.ItemId = item4.ItemId;
								vd6.JobLotId = item4.JobLotId;
								vd6.IsCGS = 1;
								vh.voucherDetailList.Add(vd6);
							}
							continue;
						}
						throw new Exception("Item record not found");
					}
					if (DiffValue != 0.0 && obj.ConversionTypeId == 3)
					{
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(obj.DifferenceAccountId);
						vd4.AgainstAccountId = vd4.AccountId;
						vd4.Comments = "Stock Difference " + DiffValue;
						vd4.DebitAmount = ((DiffValue > 0.0) ? DiffValue : 0.0);
						vd4.CreditAmount = ((DiffValue < 0.0) ? DiffValue : 0.0);
						vd4.BranchesId = obj.BranchedId;
						vh.voucherDetailList.Add(vd4);
					}
				}
				if (obj.invStockConversionPackings.Count > 0)
				{
					foreach (InvStockConversionPackingMaterial item3 in obj.invStockConversionPackings)
					{
						if (dtitem.Count > 0)
						{
							List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item3.ItemId)).ToList();
							if (find.Count > 0)
							{
								VoucherDetail vd3 = new VoucherDetail();
								vd3.LineId = item3.LineId;
								vd3.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd3.AgainstAccountId = vd3.AccountId;
								vd3.CreditAmount = Conversion.ToDouble(item3.ItemAmount);
								vd3.Comments = "ItemName: " + find[0].ItemName + ", " + $"Qty: {item3.ItemQty}, " + $"Rate: {item3.ItemRate}, " + $"ItemAmount: {item3.ItemAmount}";
								vd3.DebitAmount = 0.0;
								vd3.BranchesId = obj.BranchedId;
								vd3.ItemId = item3.ItemId;
								vd3.IsCGS = 1;
								vh.voucherDetailList.Add(vd3);
							}
						}
					}
				}
				if (obj.invStockConversionAddExpenses.Count > 0)
				{
					foreach (InvStockConversionAddExpense item5 in obj.invStockConversionAddExpenses.ToList())
					{
						VoucherDetail vd2 = new VoucherDetail();
						vd2.AccountId = Conversion.ToInt(item5.ChartOfAccountId);
						if (item5.LedgerRemarks != string.Empty)
						{
							vd2.Comments = Conversion.ToString(item5.LedgerRemarks);
						}
						else
						{
							vd2.Comments = "Labour Charge to Product";
						}
						vd2.CreditAmount = Conversion.ToDouble(item5.ExpAmount);
						vd2.DebitAmount = Conversion.ToSingle(0);
						vd2.BranchesId = obj.BranchedId;
						vh.voucherDetailList.Add(vd2);
					}
				}
				if (ContractWagesChargetoProductForStockConversion && obj.InvContractorWagesBillHeaderList != null && obj.InvContractorWagesBillHeaderList.Count > 0)
				{
					foreach (Architecture.Model.ContractorWages.InvContractorWagesBillHeader item11 in obj.InvContractorWagesBillHeaderList.ToList())
					{
						foreach (InvContractorWagesBillDetail item2 in item11.invContractWagesBillDateil.ToList())
						{
							if (item2.FreeOfCost)
							{
								continue;
							}
							List<SupplierCustomerGlAccountId> findSupp = (from c in CommonServices.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId)
								where c.Id == Conversion.ToInt(item2.ContractorId)
								select c).ToList();
							if (findSupp.Count > 0)
							{
								int GlAccountId = Conversion.ToInt(findSupp[0].GlAccountId);
								if (GlAccountId == 0)
								{
									throw new Exception("Contractor GlAccountId Not found");
								}
								string Remarks = "";
								VoucherDetail vd = new VoucherDetail();
								vd.AccountId = GlAccountId;
								vd.AgainstAccountId = GlAccountId;
								Remarks = item11.RefDocument + "  :" + item2.WagesAccountName + " Item Name :" + item2.ItemName + "  Wages Qty :  " + item2.Qty + "  Wages Rate :  " + item2.WageRate + "  Wages Amount :  " + item2.WagesAmount + "  BillWeight :  " + item2.BillWeight;
								vd.Comments = Remarks;
								vd.CreditAmount = Conversion.ToDouble(item2.WagesAmount);
								vd.ItemAmount = Conversion.ToDouble(item2.WagesAmount);
								vd.ItemRate = Conversion.ToDouble(item2.WageRate);
								vd.OrderNo = item11.JobOrderId;
								vh.voucherDetailList.Add(vd);
							}
						}
					}
				}
				returnVoucherID = ((VoucherHeadId != 0) ? Convert.ToInt32(GenericProvider.SetProc(sqlTrn, vh, "Sp_VoucherHead_Update")) : Convert.ToInt32(GenericProvider.SetProc(sqlTrn, vh, "Sp_VoucherHead_Insert")));
				if (returnVoucherID > 0)
				{
					vh.Id = returnVoucherID;
				}
				else
				{
					returnVoucherID = vh.Id;
				}
				foreach (VoucherDetail item in vh.voucherDetailList)
				{
					InvStockConversionDetail sid = obj.invStockConversionDetails.Find((InvStockConversionDetail x) => x.LineId == item.LineId && x.ItemId == item.ItemId && x.LineId > 0 && item.LineId > 0);
					if (sid != null)
					{
						item.RefDocSubIdNo = sid.Id;
					}
					InvStockConversionPackingMaterial pd = obj.invStockConversionPackings.Find((InvStockConversionPackingMaterial x) => x.LineId == item.LineId && x.ItemId == item.ItemId && x.LineId > 0 && item.LineId > 0);
					if (pd != null)
					{
						item.RefDocSubIdNo = pd.Id;
					}
					item.VoucherHeadId = vh.Id;
					item.BranchesId = obj.BranchedId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_Insert"));
				}
				if (vh.Id > 0)
				{
					SqlCommand sqlCommand5 = new SqlCommand();
					sqlCommand5.Connection = sqlcon;
					sqlCommand5.Transaction = sqlTrn;
					sqlCommand5.CommandType = CommandType.StoredProcedure;
					sqlCommand5.CommandText = "USP_VoucherBalanceCheck";
					sqlCommand5.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand5.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand5.Parameters.AddWithValue("@Id", vh.Id);
					sqlCommand5.ExecuteScalar();
				}
			}
			else
			{
				DataTable dtvh2 = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocTypeId, obj.Id);
				if (dtvh2 != null && dtvh2.Rows.Count > 0)
				{
					SqlCommand sqlCommand6 = new SqlCommand();
					sqlCommand6.Connection = sqlcon;
					sqlCommand6.Transaction = sqlTrn;
					sqlCommand6.CommandType = CommandType.StoredProcedure;
					sqlCommand6.CommandText = "usp_VoucherDeleteByVoucherId";
					sqlCommand6.Parameters.AddWithValue("@Id", Conversion.ToInt(dtvh2.Rows[0]["Id"]));
					sqlCommand6.ExecuteNonQuery();
				}
			}
			if (ContractWagesChargetoProductForStockConversion)
			{
				SqlCommand sqlCommand7 = new SqlCommand();
				sqlCommand7.Connection = sqlcon;
				sqlCommand7.Transaction = sqlTrn;
				sqlCommand7.CommandType = CommandType.StoredProcedure;
				sqlCommand7.CommandText = "USP_WagesDeleteByRefDocTypeAndIdForConversion";
				sqlCommand7.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand7.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand7.Parameters.AddWithValue("@RefDocumentTypeId", obj.DocTypeId);
				sqlCommand7.Parameters.AddWithValue("@RefDocId", obj.Id);
				sqlCommand7.ExecuteNonQuery();
				foreach (Architecture.Model.ContractorWages.InvContractorWagesBillHeader invContractorWagesBillHeader in obj.InvContractorWagesBillHeaderList)
				{
					invContractorWagesBillHeader.RefDocNoId = obj.Id;
					WagesSave(invContractorWagesBillHeader, sqlTrn, sqlcon);
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

	public List<Architecture.Model.Production.InvStockConversion> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Production.InvStockConversion> lst = new List<Architecture.Model.Production.InvStockConversion>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Production.InvStockConversion>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Production.InvStockConversion item3 in lst)
					{
						if (item3.DocTypeId == 66 || item3.DocTypeId == 808)
						{
							List<SqlParameter> lstPrm4 = new List<SqlParameter>();
							lstPrm4.Add(new SqlParameter("@Id", item3.Id));
							lstPrm4.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
							item3.invStockConversionDetails = GenericProvider.Get<InvStockConversionDetail>(sqlConnection2, "Sp_InvStockConversionDetail_GetAllMethod", lstPrm4);
						}
						if (item3.DocTypeId == 67)
						{
							List<SqlParameter> lstPrm3 = new List<SqlParameter>();
							lstPrm3.Add(new SqlParameter("@Id", item3.Id));
							lstPrm3.Add(new SqlParameter("@Activity", "ReadByHeaderIdForTrading"));
							item3.invStockConversionDetails = GenericProvider.Get<InvStockConversionDetail>(sqlConnection2, "Sp_InvStockConversionDetail_GetAllMethod", lstPrm3);
						}
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Production.InvStockConversion item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item2.invStockConversionPackings = GenericProvider.Get<InvStockConversionPackingMaterial>(sqlConnection3, "Sp_InvStockConversionPackingMaterial_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Production.InvStockConversion item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
					item.invStockConversionAddExpenses = GenericProvider.Get<InvStockConversionAddExpense>(sqlcon, "Sp_InvStockConversionAddExpense_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public static VoucherHead MakeVoucher(Architecture.Model.ContractorWages.InvContractorWagesBillHeader obj)
	{
		VoucherHead vh = new VoucherHead();
		try
		{
			new List<Architecture.Model.ContractorWages.InvConractorWagesAccounts>();
			List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.DocNo);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = Conversion.ToString(obj.OtherRemarks);
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.BillAmount = 0.0;
			vh.ManualBillNo = Conversion.ToString(obj.RefDocNo);
			vh.DueDate = obj.DocDate;
			vh.DueDays = 0;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtSupplier = CommonServices.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			new List<GetItemGlIdsandItemName>();
			if ((obj.DocumentTypeId == 101 || obj.DocumentTypeId == 810) && obj.invContractWagesBillDateil.Count > 0)
			{
				foreach (InvContractorWagesBillDetail item in obj.invContractWagesBillDateil.ToList())
				{
					if (item.FreeOfCost)
					{
						continue;
					}
					List<SupplierCustomerGlAccountId> findSupp = dtSupplier.Where((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(item.ContractorId)).ToList();
					if (findSupp.Count > 0)
					{
						int GlAccountId = Conversion.ToInt(findSupp[0].GlAccountId);
						if (GlAccountId == 0)
						{
							throw new Exception("Contractor GlAccountId Not found");
						}
						string Remarks = "";
						VoucherDetail vd = new VoucherDetail();
						vd.AccountId = GlAccountId;
						vd.AgainstAccountId = GlAccountId;
						Remarks = obj.RefDocument + "  :" + item.WagesAccountName + " Item Name :" + item.ItemName + "  Wages Qty :  " + item.Qty + "  Wages Rate :  " + item.WageRate + "  Wages Amount :  " + item.WagesAmount + "  BillWeight :  " + item.BillWeight;
						vd.Comments = Remarks;
						vd.CreditAmount = Conversion.ToDouble(item.WagesAmount);
						vd.ItemAmount = Conversion.ToDouble(item.WagesAmount);
						vd.ItemRate = Conversion.ToDouble(item.WageRate);
						vd.OrderNo = obj.JobOrderId;
						vh.voucherDetailList.Add(vd);
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return vh;
	}

	public static int WagesSave(Architecture.Model.ContractorWages.InvContractorWagesBillHeader obj, SqlTransaction defaultSqlTran, SqlConnection defaultSqlCon)
	{
		try
		{
			return new Architecture.DAL.ContractorWages.InvContractorWagesBillHeader().SetData(obj, "Sp_InvContractorWagesBillHeader_Insert", defaultSqlTran, defaultSqlCon);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
