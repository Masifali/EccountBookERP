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

namespace Architecture.DAL.Inventory;

public class StockMonthlyClosingHeader
{
	public int SetData(Architecture.Model.Inventory.StockMonthlyClosingHeader obj, string ProcName)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		VoucherHead voucherHead = new VoucherHead();
		voucherHead.voucherDetailList = new List<VoucherDetail>();
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int LineId = 1;
			double TotalClosingAmount = 0.0;
			double DiffAmount = 0.0;
			double TotalOpeningAmount = 0.0;
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (StockMonthlyClosing item7 in obj.StockMonthlyClosingList)
			{
				TotalOpeningAmount += item7.OpeningAmount;
				item7.LineId = LineId;
				item7.HeaderId = returnID;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "[dbo].[USP_StockMonthlyClosing_InsertAndUpdate]"));
				item7.Id = code;
				LineId++;
			}
			double TotalDebit = 0.0;
			double TotalCredit = 0.0;
			CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5);
			if (false)
			{
				List<Architecture.Model.Inventory.GetItemGlIdsandItemName> dtitem = (dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
				string RemarksCgs = "";
				obj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
				foreach (StockMonthlyClosing item6 in obj.StockMonthlyClosingList.ToList())
				{
					List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find2 = dtitem.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item6.ItemId)).ToList();
					if (find2.Count > 0)
					{
						RemarksCgs = "ItemQty: " + item6.Qty + "   " + Conversion.ToString(find2[0].ItemName);
						RemarksCgs = RemarksCgs + " Weight: " + item6.Weight;
						RemarksCgs = RemarksCgs + " Opening Rate: " + item6.OpeningRate;
						VoucherDetail vd9 = new VoucherDetail();
						vd9.LineId = item6.LineId;
						vd9.IsCGS = 1;
						vd9.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd9.AgainstAccountId = vd9.AccountId;
						vd9.Comments = Conversion.ToString(RemarksCgs);
						vd9.DebitAmount = Conversion.ToDouble(item6.OpeningAmount);
						vd9.ItemId = Conversion.ToInt(item6.ItemId);
						vd9.QtyIn = Conversion.ToDouble(item6.Qty);
						vd9.WeightIn = Conversion.ToDouble(item6.Weight);
						vd9.ItemRate = Conversion.ToDouble(item6.OpeningRate);
						vd9.JobLotId = Conversion.ToInt(item6.JobLotId);
						voucherHead.voucherDetailList.Add(vd9);
						InventoryStockEvalautionDetail ObjIn = new InventoryStockEvalautionDetail();
						ObjIn.Id = 0;
						ObjIn.LineId = item6.LineId;
						ObjIn.ItemId = item6.ItemId;
						ObjIn.WarehouseId = item6.WarehouseId;
						ObjIn.ItemUom = item6.ItemUomId;
						ObjIn.JobLotId = item6.JobLotId;
						ObjIn.InvPackingTypeId = item6.InvPackingTypeId;
						ObjIn.CropBatch = item6.CropYear;
						ObjIn.RefRefDocumentTypeId = 0;
						ObjIn.RefRefDocIdNo = 0;
						ObjIn.RefRefDocSubIdNo = 0;
						ObjIn.QtyIn = Conversion.ToDouble(item6.Qty);
						ObjIn.BillWeightIn = Conversion.ToDouble(item6.Weight);
						ObjIn.StockWeightIn = Conversion.ToDouble(item6.Weight);
						ObjIn.RateUom = item6.ItemUomId;
						ObjIn.ItemRate = Conversion.ToDouble(item6.OpeningRate);
						ObjIn.AmountIn = Conversion.ToDouble(item6.OpeningAmount);
						obj.InventoryStockEvalautionDetailslist.Add(ObjIn);
					}
				}
				foreach (StockMonthlyClosing item5 in obj.StockMonthlyClosingList.ToList())
				{
					List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find = dtitem.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item5.ItemId)).ToList();
					if (find.Count <= 0)
					{
						continue;
					}
					ReportsParameters stock = new ReportsParameters();
					stock.OrganizationId = obj.OrganizationId;
					stock.CompanyId = obj.CompanyId;
					stock.ItemId = item5.ItemId;
					stock.WarehouseId = item5.WarehouseId;
					stock.DocDate = item5.ClosingDate;
					if (item5.InvPackingTypeId > 0)
					{
						stock.stockUOM = item5.ItemUomId;
						stock.JobLotId = item5.JobLotId;
						stock.CropYear = item5.CropYear;
						stock.PackingTypeId = item5.InvPackingTypeId;
					}
					stock.RateUOM = item5.ItemUomId;
					if (obj.ActionId == 2)
					{
						stock.DocumentTypeId = obj.DocumentTypeId;
						stock.Id = obj.Id;
					}
					stock.ItemQty = Conversion.ToDouble(item5.Qty);
					stock.NetWeight = Conversion.ToDouble(item5.Weight);
					stock.LineId = item5.LineId;
					stock.ItemName = Conversion.ToString(find[0].ItemName);
					foreach (InventoryStockEvalautionDetail stockdetail in CommonServices.FIFOImplemention(stock, obj.InventoryStockEvalautionDetailslist).InventoryStockEvalautionDetailslist.ToList())
					{
						RemarksCgs = "ItemQty: " + stockdetail.QtyOut + "   " + Conversion.ToString(find[0].ItemName);
						RemarksCgs = RemarksCgs + " Weight: " + stockdetail.StockWeightOut;
						RemarksCgs = RemarksCgs + " CGS Rate: " + stockdetail.CgsRate;
						VoucherDetail vd7 = new VoucherDetail();
						vd7.LineId = item5.LineId;
						vd7.IsCGS = 1;
						vd7.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd7.AgainstAccountId = vd7.AccountId;
						vd7.Comments = Conversion.ToString(RemarksCgs);
						vd7.CreditAmount = Conversion.ToDouble(stockdetail.CgsAmount);
						vd7.ItemId = Conversion.ToInt(item5.ItemId);
						vd7.QtyOut = Conversion.ToDouble(stockdetail.QtyOut);
						vd7.WeightOut = Conversion.ToDouble(stockdetail.StockWeightOut);
						vd7.ItemCgsRate = Conversion.ToDouble(stockdetail.CgsRate);
						vd7.ItemAmount = Conversion.ToDouble(stockdetail.AmountOut);
						vd7.JobLotId = Conversion.ToInt(item5.JobLotId);
						voucherHead.voucherDetailList.Add(vd7);
						TotalClosingAmount += Conversion.ToDouble(stockdetail.CgsAmount);
						obj.InventoryStockEvalautionDetailslist.Add(stockdetail);
					}
				}
				if (TotalClosingAmount > 0.0 && TotalOpeningAmount > 0.0)
				{
					DiffAmount = TotalClosingAmount - TotalOpeningAmount;
					VoucherDetail vd6 = new VoucherDetail();
					vd6.IsCGS = 1;
					vd6.AccountId = Conversion.ToInt(obj.AccountId);
					vd6.AgainstAccountId = vd6.AccountId;
					vd6.Comments = "Monthly Stock Difference";
					if (DiffAmount > 0.0)
					{
						vd6.DebitAmount = Conversion.ToDouble(DiffAmount);
					}
					else
					{
						vd6.CreditAmount = Math.Abs(Conversion.ToDouble(DiffAmount));
					}
					voucherHead.voucherDetailList.Add(vd6);
				}
				if (obj.InventoryStockEvalautionDetailslist != null && obj.InventoryStockEvalautionDetailslist.Count > 0)
				{
					if (obj.ActionId == 2)
					{
						SqlCommand sqlCommand = new SqlCommand();
						sqlCommand.Connection = sqlcon;
						sqlCommand.Transaction = sqlTrn;
						sqlCommand.CommandType = CommandType.StoredProcedure;
						sqlCommand.CommandText = "[dbo].[USP_InventoryQtyReverseAndDeleteByDateForClosingStock]";
						sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
						sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
						sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", obj.DocumentTypeId);
						sqlCommand.Parameters.AddWithValue("@ClosingDate", obj.DocDate);
						sqlCommand.ExecuteNonQuery();
					}
					foreach (InventoryStockEvalautionDetail item4 in obj.InventoryStockEvalautionDetailslist)
					{
						item4.OrganizationId = obj.OrganizationId;
						item4.CompanyId = obj.CompanyId;
						item4.DocDate = obj.DocDate;
						item4.DocCodeNo = obj.Id;
						item4.SupplierCustomerId = 0;
						item4.BranchesId = 0;
						item4.RefDocumentTypeId = obj.DocumentTypeId;
						item4.EntryUser = obj.EntryUserId;
						item4.ModifyUser = obj.ModifyUserId;
						item4.CalcType = "Weight";
						item4.AmountOut = item4.CgsAmount;
						StockMonthlyClosing sid = obj.StockMonthlyClosingList.Find((StockMonthlyClosing x) => x.LineId == item4.LineId && x.LineId > 0 && item4.LineId > 0);
						if (sid != null)
						{
							item4.RefDocIdNo = obj.Id;
							item4.RefDocSubIdNo = sid.Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_InventoryStockEvalautionDetail_Insert"));
					}
				}
			}
			else
			{
				List<Architecture.Model.Inventory.GetItemGlIdsandItemName> dtitem2 = (dtitem2 = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId));
				foreach (StockMonthlyClosing item3 in obj.StockMonthlyClosingList.ToList())
				{
					List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find3 = dtitem2.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item3.ItemId)).ToList();
					if (find3.Count <= 0)
					{
						continue;
					}
					string RemarksCgs2 = "";
					RemarksCgs2 = "ItemQty: " + item3.Qty + "   " + Conversion.ToString(find3[0].ItemName);
					RemarksCgs2 = RemarksCgs2 + " Weight: " + item3.Weight;
					if (item3.OpeningRate > 0.0)
					{
						RemarksCgs2 = RemarksCgs2 + " Opening Rate: " + item3.OpeningRate;
					}
					if (item3.OpeningRate > 0.0 && item3.OpeningAmount > 0.0)
					{
						VoucherDetail vd10 = new VoucherDetail();
						vd10.LineId = item3.LineId;
						vd10.IsCGS = 1;
						vd10.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
						vd10.AgainstAccountId = vd10.AccountId;
						vd10.Comments = Conversion.ToString(RemarksCgs2);
						vd10.DebitAmount = Conversion.ToDouble(item3.OpeningAmount);
						vd10.ItemId = Conversion.ToInt(item3.ItemId);
						vd10.QtyIn = Conversion.ToDouble(item3.Qty);
						vd10.WeightIn = Conversion.ToDouble(item3.Weight);
						vd10.ItemRate = Conversion.ToDouble(item3.OpeningRate);
						vd10.JobLotId = Conversion.ToInt(item3.JobLotId);
						voucherHead.voucherDetailList.Add(vd10);
						TotalDebit += vd10.DebitAmount;
					}
					if (item3.Amount != 0.0)
					{
						VoucherDetail vd11 = new VoucherDetail();
						vd11.LineId = item3.LineId;
						vd11.IsCGS = 1;
						vd11.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
						vd11.AgainstAccountId = vd11.AccountId;
						vd11.Comments = Conversion.ToString(RemarksCgs2);
						if (item3.Amount > 0.0)
						{
							vd11.CreditAmount = Conversion.ToDouble(item3.Amount);
						}
						else
						{
							vd11.DebitAmount = Math.Abs(Conversion.ToDouble(item3.Amount));
							TotalDebit += vd11.DebitAmount;
						}
						vd11.ItemId = Conversion.ToInt(item3.ItemId);
						vd11.QtyOut = Conversion.ToDouble(item3.Qty);
						vd11.WeightOut = Conversion.ToDouble(item3.Weight);
						vd11.ItemRate = Conversion.ToDouble(item3.ClosingRate);
						vd11.JobLotId = Conversion.ToInt(item3.JobLotId);
						voucherHead.voucherDetailList.Add(vd11);
						TotalCredit += vd11.CreditAmount;
					}
				}
				DiffAmount = TotalDebit - TotalCredit;
				if (DiffAmount != 0.0)
				{
					VoucherDetail vd8 = new VoucherDetail();
					vd8.IsCGS = 1;
					vd8.AccountId = Conversion.ToInt(obj.AccountId);
					vd8.AgainstAccountId = vd8.AccountId;
					vd8.Comments = "Stock Difference";
					if (DiffAmount > 0.0)
					{
						vd8.CreditAmount = Conversion.ToDouble(DiffAmount);
					}
					else
					{
						vd8.DebitAmount = Math.Abs(Conversion.ToDouble(DiffAmount));
					}
					voucherHead.voucherDetailList.Add(vd8);
				}
				SqlCommand sqlCommand2 = new SqlCommand();
				sqlCommand2.Connection = sqlcon;
				sqlCommand2.Transaction = sqlTrn;
				sqlCommand2.CommandType = CommandType.StoredProcedure;
				sqlCommand2.CommandText = "usp_MonthlyStockClosing_InventoryStockEvalautionDetail";
				sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand2.Parameters.AddWithValue("@ClosingDate", obj.DocDate);
				sqlCommand2.ExecuteNonQuery();
			}
			SqlCommand sqlCommand3 = new SqlCommand();
			sqlCommand3.Connection = sqlcon;
			sqlCommand3.Transaction = sqlTrn;
			sqlCommand3.CommandType = CommandType.StoredProcedure;
			sqlCommand3.CommandText = "usp_MonthlyStockClosing_InventoryTransactions";
			sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
			sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
			sqlCommand3.Parameters.AddWithValue("@ClosingDate", obj.DocDate);
			sqlCommand3.ExecuteNonQuery();
			voucherHead.VoucherDate = obj.DocDate;
			voucherHead.VoucherCode = obj.Id;
			voucherHead.DocumentTypeId = obj.DocumentTypeId;
			voucherHead.RefAccountId = obj.AccountId;
			voucherHead.AgainstAccountId = obj.AccountId;
			voucherHead.EntryDate = DateTime.Now;
			voucherHead.ModifyDate = DateTime.Now;
			voucherHead.VoucherAmount = DiffAmount;
			voucherHead.EntryUser = obj.EntryUserId;
			voucherHead.ModifyUser = obj.ModifyUserId;
			voucherHead.OrganizationId = obj.OrganizationId;
			voucherHead.CompanyId = obj.CompanyId;
			voucherHead.FinancialYearId = obj.FinancialYearId;
			voucherHead.BranchId = 0;
			DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
			if (dtvh.Rows.Count > 0)
			{
				VoucherHeadIdGet = (voucherHead.Id = Conversion.ToInt(dtvh.Rows[0]["Id"]));
			}
			if (VoucherHeadIdGet == 0)
			{
				voucherHead.DocumentTypeSrNo = obj.Id;
				voucherHead.RefDocNoId = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, voucherHead, "Sp_VoucherHead_Insert"));
			}
			else
			{
				voucherHead.DocumentTypeSrNo = obj.Id;
				voucherHead.RefDocNoId = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, voucherHead, "Sp_VoucherHead_Update"));
			}
			if (VoucherHeadId > 0)
			{
				voucherHead.Id = VoucherHeadId;
			}
			else
			{
				VoucherHeadId = voucherHead.Id;
			}
			if (voucherHead.voucherDetailList != null && voucherHead.voucherDetailList.Count > 0)
			{
				foreach (VoucherDetail item2 in voucherHead.voucherDetailList)
				{
					item2.VoucherHeadId = voucherHead.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
				}
				if (voucherHead.Id > 0)
				{
					SqlCommand sqlCommand4 = new SqlCommand();
					sqlCommand4.Connection = sqlcon;
					sqlCommand4.Transaction = sqlTrn;
					sqlCommand4.CommandType = CommandType.StoredProcedure;
					sqlCommand4.CommandText = "USP_VoucherBalanceCheck";
					sqlCommand4.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand4.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand4.Parameters.AddWithValue("@Id", voucherHead.Id);
					sqlCommand4.ExecuteScalar();
				}
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, voucherHead, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item in voucherHead.voucherDetailList)
				{
					item.VoucherHeadId = voucherHead.Id;
					item.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
				}
				sqlTrn.Commit();
				return returnID;
			}
			throw new Exception("VoucherDetail List Not Found");
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<StockMonthlyClosing> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<StockMonthlyClosing> lst = new List<StockMonthlyClosing>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<StockMonthlyClosing>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
