using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Pos;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.POS;

namespace Architecture.BLL.Pos;

public class WsRmStockTransferNotes
{
	public static VoucherHead MakeVoucher(Architecture.Model.POS.WsRmStockTransferNotes obj)
	{
		List<InventoryItemsOther> dtItemOther = new List<InventoryItemsOther>();
		List<GetItemGlIdsandItemName> dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
		VoucherHead vh = new VoucherHead();
		try
		{
			int FinancialYearId = 0;
			DataTable dt = CommonServies.GetActiveFinancialYear(obj.OrganizationId, obj.CompanyId);
			if (dt.Rows.Count > 0)
			{
				FinancialYearId = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.DocNo);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = Conversion.ToString(obj.RemarksHeader);
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
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
			if (obj.DocumentTypeId == 107)
			{
				int LocationDebitAcId = 0;
				string ConfigKey = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.SourceLocationId, "LocationAccount");
				if (!(ConfigKey != string.Empty))
				{
					throw new Exception("Location Account Not Map in Configuration Please Check!");
				}
				LocationDebitAcId = Conversion.ToInt(ConfigKey);
				if (obj.WsRmStockTransferNotesDetailsList.Count > 0)
				{
					string Remarks = string.Empty;
					int LineId = 0;
					foreach (WsRmStockTransferNotesDetail item2 in obj.WsRmStockTransferNotesDetailsList.ToList())
					{
						LineId++;
						if (dtitem.Count > 0)
						{
							List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
							if (find2.Count > 0)
							{
								VoucherDetail vd3 = new VoucherDetail();
								Remarks = "Item: " + Conversion.ToString(find2[0].ItemName) + ",   Qty: " + item2.IssuedQty + ",   Rate:" + item2.IssuedRate + "  Item Amount: " + item2.IssuedAmount;
								VoucherDetail vd5 = new VoucherDetail();
								vd5.LineId = LineId;
								vd5.AccountId = Conversion.ToInt(LocationDebitAcId);
								vd5.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
								vd5.Comments = Conversion.ToString(Remarks);
								vd5.DebitAmount = Conversion.ToDouble(item2.IssuedAmount);
								vd5.ItemId = Conversion.ToInt(item2.ItemId);
								vd5.QtyOut = Conversion.ToDouble(item2.IssuedQty);
								vd5.ItemRate = Conversion.ToDouble(item2.IssuedRate);
								vd5.WeightOut = Conversion.ToDouble(item2.IssuedQty);
								vd5.ItemAmount = Conversion.ToDouble(item2.IssuedAmount);
								vd5.SupplierCustomerId = Conversion.ToInt(LocationDebitAcId);
								vh.voucherDetailList.Add(vd5);
								vd3.LineId = LineId;
								vd3.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
								vd3.AgainstAccountId = Conversion.ToInt(LocationDebitAcId);
								vd3.Comments = Conversion.ToString(Remarks);
								vd3.CreditAmount = Conversion.ToDouble(item2.IssuedAmount);
								vd3.ItemId = Conversion.ToInt(item2.ItemId);
								vd3.QtyOut = Conversion.ToDouble(item2.IssuedQty);
								vd3.ItemRate = Conversion.ToDouble(item2.IssuedRate);
								vd3.ItemAmount = Conversion.ToDouble(item2.IssuedAmount);
								vd3.SupplierCustomerId = Conversion.ToInt(LocationDebitAcId);
								vh.voucherDetailList.Add(vd3);
								continue;
							}
							throw new Exception("Item Record Not found");
						}
						throw new Exception("Item Record Not found");
					}
				}
				if (obj.WsRmStockTransferNotesExpensesList.Count > 0)
				{
					dtItemOther = CommonServies.GetOtherItemForFinancialEffects(obj.OrganizationId, obj.CompanyId);
					string OtherExpRemarks = "";
					foreach (WsRmStockTransferNotesExpenses item in obj.WsRmStockTransferNotesExpensesList.ToList())
					{
						if (dtItemOther.Count > 0)
						{
							List<InventoryItemsOther> find = dtItemOther.Where((InventoryItemsOther c) => c.Id == Conversion.ToInt(item.ItemId)).ToList();
							if (find.Count > 0)
							{
								VoucherDetail vd2 = new VoucherDetail();
								OtherExpRemarks = ((!(item.Remarks == string.Empty) && item.Remarks != null) ? item.Remarks : ("Item: " + Conversion.ToString(find[0].OtherItemName) + ",   Qty: " + item.Qty + ",   Rate:" + item.Rate + "  Amount: " + item.Amount));
								VoucherDetail vd4 = new VoucherDetail();
								vd4.AccountId = Conversion.ToInt(LocationDebitAcId);
								vd4.AgainstAccountId = Conversion.ToInt(find[0].SaleGLAcId);
								vd4.Comments = Conversion.ToString(OtherExpRemarks);
								vd4.DebitAmount = Conversion.ToDouble(item.Amount);
								vd4.ItemId = Conversion.ToInt(item.ItemId);
								vd4.QtyOut = Conversion.ToDouble(item.Qty);
								vd4.ItemRate = Conversion.ToDouble(item.Rate);
								vd4.WeightOut = Conversion.ToDouble(item.Qty);
								vd4.ItemAmount = Conversion.ToDouble(item.Amount);
								vd4.SupplierCustomerId = Conversion.ToInt(LocationDebitAcId);
								vh.voucherDetailList.Add(vd4);
								vd2.AccountId = Conversion.ToInt(find[0].SaleGLAcId);
								vd2.AgainstAccountId = Conversion.ToInt(LocationDebitAcId);
								vd2.Comments = Conversion.ToString(OtherExpRemarks);
								vd2.CreditAmount = Conversion.ToDouble(item.Amount);
								vd2.ItemId = Conversion.ToInt(item.ItemId);
								vd2.QtyOut = Conversion.ToDouble(item.Qty);
								vd2.ItemRate = Conversion.ToDouble(item.Rate);
								vd2.ItemAmount = Conversion.ToDouble(item.Amount);
								vd2.SupplierCustomerId = Conversion.ToInt(LocationDebitAcId);
								vh.voucherDetailList.Add(vd2);
								continue;
							}
							throw new Exception("OtherItems Record Not found");
						}
						throw new Exception("OtherItems Record Not found");
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		return vh;
	}

	public static int Save(Architecture.Model.POS.WsRmStockTransferNotes obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
				return new Architecture.DAL.Pos.WsRmStockTransferNotes().SetData(obj, "Sp_WsRmStockTransferNotes_Insert");
			}
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			return new Architecture.DAL.Pos.WsRmStockTransferNotes().SetData(obj, "Sp_WsRmStockTransferNotes_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.POS.WsRmStockTransferNotes GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.POS.WsRmStockTransferNotes>();
			return new Architecture.DAL.Pos.WsRmStockTransferNotes().GetData("Sp_WsRmStockTransferNotes_GetAllMethod", p)[0];
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmStockTransferNotes>("Sp_WsRmStockTransferNotes_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateDocCode(Architecture.Model.POS.WsRmStockTransferNotes obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateDocNoByDocumentTypeId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmStockTransferNotes>("Sp_WsRmStockTransferNotes_GetAllMethod", p);
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

	public static DataTable GetLoaderDataForWsRmStockTransferNotes(ReportsParameters obj)
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
				ParameterName = "@Activity",
				Value = "GetLoaderDataForWsRmStockTransferNotes"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmStockTransferNotes>("Sp_WsRmStockTransferNotes_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ApproveWsRmStockTransferNote(Architecture.Model.POS.WsRmStockTransferNotes obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUser",
				Value = obj.ApprovedUserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ApproveWsRmStockTransferNote"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmStockTransferNotes>("Sp_WsRmStockTransferNotes_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
