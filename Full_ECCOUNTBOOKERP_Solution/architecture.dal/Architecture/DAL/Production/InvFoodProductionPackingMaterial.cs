using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Production;

namespace Architecture.DAL.Production;

public class InvFoodProductionPackingMaterial
{
	public int SetData(Architecture.Model.Production.InvFoodProductionPackingMaterial obj)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		VoucherHead voucherHead = new VoucherHead();
		voucherHead.voucherDetailList = new List<VoucherDetail>();
		List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			bool InventoryFinancialsEffectsInActive = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "InventoryFinancialsEffectsInActive"));
			bool FIFOCGS = false;
			foreach (Architecture.Model.Production.InvFoodProductionPackingMaterial PmItem in obj.invFoodProductionPackingMaterialList)
			{
				returnID = 0;
				VoucherHeadIdGet = 0;
				VoucherHeadId = 0;
				string ProcName = ((PmItem.Id == 0) ? "Sp_InvFoodProductionPackingMaterial_Insert" : "Sp_InvFoodProductionPackingMaterial_Update");
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, PmItem, ProcName));
				if (returnID > 0)
				{
					PmItem.ModifyUser = 0;
					PmItem.Id = Convert.ToInt32(returnID);
				}
				else
				{
					PmItem.EntryUser = 0;
					returnID = PmItem.Id;
				}
				int WipAccountId = 0;
				dtitem = CommonServices.GetItemGlIdsandItemName(PmItem.OrganizationId, PmItem.CompanyId);
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = PmItem.OrganizationId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = PmItem.CompanyId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = PmItem.InvProductionJobOrderId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "GetGlAccountsByJobOrderId"
				});
				DataTable dtJobOrder = GenericProvider.GetDataTableProc<Architecture.Model.Production.InvProductionJobOrder>("Sp_InvProductionJobOrder_GetAllMethod", p);
				if (dtJobOrder != null && dtJobOrder.Rows.Count > 0)
				{
					WipAccountId = Conversion.ToInt(dtJobOrder.Rows[0]["WorkInProccessAcId"]);
					if (FIFOCGS)
					{
						string RemarksCgs = "";
						PmItem.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
						List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(PmItem.ItemId)).ToList();
						if (find2.Count <= 0)
						{
							throw new Exception("Itm GL Id Not Found...");
						}
						ReportsParameters stock = new ReportsParameters();
						stock.OrganizationId = PmItem.OrganizationId;
						stock.CompanyId = PmItem.CompanyId;
						stock.ItemId = PmItem.ItemId;
						stock.WarehouseId = PmItem.WarehouseId;
						stock.DocDate = PmItem.DocDate;
						if (PmItem.ModifyUser > 0)
						{
							stock.DocumentTypeId = PmItem.DocumentTypeId;
							stock.Id = PmItem.Id;
						}
						stock.ItemQty = PmItem.Qty;
						stock.NetWeight = PmItem.Qty;
						stock.RateEqvailent = 1.0;
						stock.LineId = 1;
						foreach (InventoryStockEvalautionDetail stockdetail in CommonServices.FIFOImplemention(stock, PmItem.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
						{
							RemarksCgs = "ItemQty: " + stockdetail.QtyOut + "   " + Conversion.ToString(find2[0].ItemName) + "  CGS Rate:" + stockdetail.CgsRate;
							VoucherDetail vd4 = new VoucherDetail();
							vd4.LineId = 1;
							vd4.AccountId = Conversion.ToInt(WipAccountId);
							vd4.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
							vd4.Comments = Conversion.ToString(RemarksCgs);
							vd4.DebitAmount = Conversion.ToDouble(stockdetail.CgsAmount);
							vd4.ItemId = Conversion.ToInt(PmItem.ItemId);
							vd4.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
							vd4.WeightOut = Conversion.ToDouble(stockdetail.QtyOut);
							vd4.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
							vd4.RateCut = Conversion.ToDouble(0);
							vd4.RateCutAmount = Conversion.ToDouble(0);
							vd4.ItemAmount = Conversion.ToDouble(stockdetail.CgsAmount);
							vd4.BranchesId = obj.BranchesId;
							vd4.OrderNo = PmItem.InvProductionJobOrderId;
							voucherHead.voucherDetailList.Add(vd4);
							VoucherDetail vd3 = new VoucherDetail();
							vd3.LineId = 1;
							vd3.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
							vd3.AgainstAccountId = Conversion.ToInt(WipAccountId);
							vd3.Comments = Conversion.ToString(RemarksCgs);
							vd3.CreditAmount = Conversion.ToDouble(stockdetail.CgsAmount);
							vd3.ItemId = Conversion.ToInt(PmItem.ItemId);
							vd3.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
							vd3.WeightOut = Conversion.ToDouble(stockdetail.QtyOut);
							vd3.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
							vd3.RateCut = Conversion.ToDouble(0);
							vd3.RateCutAmount = Conversion.ToDouble(0);
							vd3.ItemAmount = Conversion.ToDouble(stockdetail.CgsAmount);
							vd3.BranchesId = obj.BranchesId;
							vd3.OrderNo = PmItem.InvProductionJobOrderId;
							voucherHead.voucherDetailList.Add(vd3);
							PmItem.InventoryStockEvalautionDetailslist.Add(stockdetail);
						}
						if (PmItem.InventoryStockEvalautionDetailslist != null && PmItem.InventoryStockEvalautionDetailslist.Count > 0)
						{
							if (PmItem.ModifyUser > 0)
							{
								SqlCommand sqlCommand = new SqlCommand();
								sqlCommand.Connection = sqlcon;
								sqlCommand.Transaction = sqlTrn;
								sqlCommand.CommandType = CommandType.StoredProcedure;
								sqlCommand.CommandText = "[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]";
								sqlCommand.Parameters.AddWithValue("@OrganizationId", PmItem.OrganizationId);
								sqlCommand.Parameters.AddWithValue("@CompanyId", PmItem.CompanyId);
								sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", PmItem.DocumentTypeId);
								sqlCommand.Parameters.AddWithValue("@RefDocIdNo", returnID);
								sqlCommand.ExecuteNonQuery();
							}
							foreach (InventoryStockEvalautionDetail item3 in PmItem.InventoryStockEvalautionDetailslist)
							{
								item3.OrganizationId = PmItem.OrganizationId;
								item3.CompanyId = PmItem.CompanyId;
								item3.DocDate = PmItem.DocDate;
								item3.DocCodeNo = PmItem.DocNo;
								item3.SupplierCustomerId = 0;
								item3.BranchesId = PmItem.BranchesId;
								item3.RefDocumentTypeId = PmItem.DocumentTypeId;
								item3.EntryUser = PmItem.EntryUser;
								item3.ModifyUser = PmItem.ModifyUser;
								item3.CalcType = "Weight";
								item3.ItemRate = item3.CgsRate;
								item3.AmountOut = item3.CgsAmount;
								item3.RefDocIdNo = returnID;
								item3.RefDocSubIdNo = returnID;
								Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "USP_InventoryStockEvalautionDetail_Insert"));
							}
						}
					}
					else
					{
						InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
						inventoryTransactions.OrganizationId = PmItem.OrganizationId;
						inventoryTransactions.CompanyId = PmItem.CompanyId;
						inventoryTransactions.RefDocumentTypeId = PmItem.DocumentTypeId;
						inventoryTransactions.RefDocIdNo = returnID;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
					}
					InventoryTransactions transactions = new InventoryTransactions();
					transactions.OrganizationId = PmItem.OrganizationId;
					transactions.CompanyId = PmItem.CompanyId;
					transactions.RefDocumentTypeId = PmItem.DocumentTypeId;
					transactions.RefDocIdNo = returnID;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
					foreach (Architecture.Model.Production.InvFoodProductionPackingMaterial item2 in obj.invFoodProductionPackingMaterialList)
					{
						SqlCommand sqlCommand2 = new SqlCommand();
						sqlCommand2.Connection = sqlcon;
						sqlCommand2.Transaction = sqlTrn;
						sqlCommand2.CommandType = CommandType.StoredProcedure;
						sqlCommand2.CommandText = "USP_InventoryValidation";
						sqlCommand2.Parameters.AddWithValue("@OrganizationId", item2.OrganizationId);
						sqlCommand2.Parameters.AddWithValue("@CompanyId", item2.CompanyId);
						sqlCommand2.Parameters.AddWithValue("@DocumentTypeId", item2.DocumentTypeId);
						sqlCommand2.Parameters.AddWithValue("@DocDate", item2.DocDate);
						sqlCommand2.Parameters.AddWithValue("@ItemId", item2.ItemId);
						sqlCommand2.Parameters.AddWithValue("@WarehouseId", item2.WarehouseId);
						sqlCommand2.Parameters.AddWithValue("@NetWeight", item2.Qty);
						sqlCommand2.Parameters.AddWithValue("@ItemConditionId", item2.ItemConditionId);
						sqlCommand2.Parameters.AddWithValue("@RackId", item2.RackId);
						sqlCommand2.Parameters.AddWithValue("@Activity", "PM");
						sqlCommand2.ExecuteScalar();
					}
					if (returnID <= 0 || InventoryFinancialsEffectsInActive)
					{
						continue;
					}
					if (PmItem.ModifyUser > 0)
					{
						SqlCommand sqlCommand3 = new SqlCommand();
						sqlCommand3.Connection = sqlcon;
						sqlCommand3.Transaction = sqlTrn;
						sqlCommand3.CommandType = CommandType.StoredProcedure;
						sqlCommand3.CommandText = "Sp_Vouchers_GetMethods";
						sqlCommand3.Parameters.AddWithValue("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
						sqlCommand3.Parameters.AddWithValue("@OrganizationId", PmItem.OrganizationId);
						sqlCommand3.Parameters.AddWithValue("@CompanyId", PmItem.CompanyId);
						sqlCommand3.Parameters.AddWithValue("@DocumentTypeId", PmItem.DocumentTypeId);
						sqlCommand3.Parameters.AddWithValue("@DocumentTypeSrNo", PmItem.Id);
						VoucherHeadIdGet = (voucherHead.Id = Conversion.ToInt(sqlCommand3.ExecuteScalar()));
					}
					voucherHead.DocumentTypeSrNo = returnID;
					voucherHead.VoucherDate = PmItem.DocDate;
					voucherHead.VoucherCode = PmItem.DocNo;
					voucherHead.DocumentTypeId = PmItem.DocumentTypeId;
					voucherHead.BaseDocumentTypeId = PmItem.BaseDocumentTypeId;
					voucherHead.RefAccountId = Conversion.ToInt(WipAccountId);
					voucherHead.AgainstAccountId = Conversion.ToInt(WipAccountId);
					voucherHead.EntryDate = DateTime.Now;
					voucherHead.ModifyDate = DateTime.Now;
					voucherHead.VoucherAmount = Conversion.ToDouble(PmItem.Amount);
					voucherHead.EntryUser = PmItem.EntryUser;
					voucherHead.ModifyUser = PmItem.ModifyUser;
					voucherHead.OrganizationId = PmItem.OrganizationId;
					voucherHead.CompanyId = PmItem.CompanyId;
					voucherHead.FinancialYearId = PmItem.FinancialYearId;
					voucherHead.BranchId = PmItem.BranchesId;
					if (!FIFOCGS)
					{
						if (dtitem.Count <= 0)
						{
							throw new Exception("Item list Not Found");
						}
						List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(PmItem.ItemId)).ToList();
						if (find.Count <= 0)
						{
							throw new Exception("Item GlAccountId Not Found");
						}
						VoucherDetail vd = new VoucherDetail();
						vd.AccountId = Conversion.ToInt(WipAccountId);
						vd.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd.Comments = Conversion.ToString(PmItem.pmRemarks);
						vd.DebitAmount = Conversion.ToDouble(PmItem.Amount);
						vd.ItemId = PmItem.ItemId;
						vd.ItemAmount = Conversion.ToDouble(PmItem.Amount);
						vd.OrderNo = Conversion.ToInt(PmItem.InvProductionJobOrderId);
						vd.BranchesId = obj.BranchesId;
						voucherHead.voucherDetailList.Add(vd);
						VoucherDetail vd2 = new VoucherDetail();
						vd2.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd2.AgainstAccountId = Conversion.ToInt(WipAccountId);
						vd2.Comments = Conversion.ToString(PmItem.pmRemarks);
						vd2.CreditAmount = Conversion.ToDouble(PmItem.Amount);
						vd2.ItemId = PmItem.ItemId;
						vd2.ItemAmount = Conversion.ToDouble(PmItem.Amount);
						vd2.QtyOut = Conversion.ToDouble(PmItem.Qty);
						vd2.WeightOut = Conversion.ToDouble(PmItem.Qty);
						vd2.OrderNo = Conversion.ToInt(PmItem.InvProductionJobOrderId);
						vd2.BranchesId = obj.BranchesId;
						voucherHead.voucherDetailList.Add(vd2);
					}
					VoucherHeadId = ((VoucherHeadIdGet != 0) ? Convert.ToInt32(GenericProvider.SetProc(sqlTrn, voucherHead, "Sp_VoucherHead_Update")) : Convert.ToInt32(GenericProvider.SetProc(sqlTrn, voucherHead, "Sp_VoucherHead_Insert")));
					if (VoucherHeadId > 0)
					{
						voucherHead.Id = VoucherHeadId;
					}
					else
					{
						VoucherHeadId = voucherHead.Id;
					}
					foreach (VoucherDetail item in voucherHead.voucherDetailList)
					{
						item.VoucherHeadId = voucherHead.Id;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_Insert"));
					}
					if (voucherHead.Id > 0)
					{
						SqlCommand sqlCommand4 = new SqlCommand();
						sqlCommand4.Connection = sqlcon;
						sqlCommand4.Transaction = sqlTrn;
						sqlCommand4.CommandType = CommandType.StoredProcedure;
						sqlCommand4.CommandText = "USP_VoucherBalanceCheck";
						sqlCommand4.Parameters.AddWithValue("@OrganizationId", PmItem.OrganizationId);
						sqlCommand4.Parameters.AddWithValue("@CompanyId", PmItem.CompanyId);
						sqlCommand4.Parameters.AddWithValue("@Id", voucherHead.Id);
						sqlCommand4.ExecuteScalar();
					}
					continue;
				}
				throw new Exception("Work in process Account not found against JobOrder");
			}
			if (obj.PmDetailRowsRemoveIds != null && obj.PmDetailRowsRemoveIds != string.Empty)
			{
				SqlCommand sqlCommand5 = new SqlCommand();
				sqlCommand5.Connection = sqlcon;
				sqlCommand5.Transaction = sqlTrn;
				sqlCommand5.CommandType = CommandType.StoredProcedure;
				sqlCommand5.CommandText = "[dbo].[USP_InvFoodProductionPmRowsDeleteByIds]";
				sqlCommand5.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand5.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand5.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
				sqlCommand5.Parameters.AddWithValue("@DetailIds", obj.PmDetailRowsRemoveIds);
				sqlCommand5.ExecuteNonQuery();
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

	public List<Architecture.Model.Production.InvFoodProductionPackingMaterial> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Production.InvFoodProductionPackingMaterial> lst = new List<Architecture.Model.Production.InvFoodProductionPackingMaterial>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.Production.InvFoodProductionPackingMaterial>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
