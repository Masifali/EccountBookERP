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

public class WsRmStockReceivedNotes
{
	public static VoucherHead MakeVoucher(Architecture.Model.POS.WsRmStockReceivedNotes obj)
	{
		List<InventoryItemsOther> dtItemOther = new List<InventoryItemsOther>();
		List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
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
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (obj.DocumentTypeId == 108)
			{
				int LocationDebitAcId = 0;
				string ConfigKey = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.DestinationLocationId, "LocationAccount");
				if (!(ConfigKey != string.Empty))
				{
					throw new Exception("Location Account Not Map in Configuration Please Check!");
				}
				LocationDebitAcId = Conversion.ToInt(ConfigKey);
				if (obj.WsRmStockReceivedNotesExpensesList.Count > 0)
				{
					dtItemOther = CommonServies.GetOtherItemForFinancialEffects(obj.OrganizationId, obj.CompanyId);
				}
				if (obj.WsRmStockReceivedNotesDetailList.Count > 0)
				{
					string Remarks = string.Empty;
					int LineId = 0;
					foreach (WsRmStockReceivedNotesDetail item in obj.WsRmStockReceivedNotesDetailList.ToList())
					{
						LineId++;
						if (dtitem.Count > 0)
						{
							List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
							if (find.Count > 0)
							{
								VoucherDetail vd2 = new VoucherDetail();
								Remarks = "Item: " + Conversion.ToString(find[0].ItemName) + ",   Qty: " + item.ReceivedQty + ",   Rate:" + item.ReceivedRate + "  Item Amount: " + item.ReceivedAmount;
								VoucherDetail vd3 = new VoucherDetail();
								vd3.LineId = Conversion.ToInt(LineId);
								vd3.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd3.AgainstAccountId = Conversion.ToInt(LocationDebitAcId);
								vd3.Comments = Conversion.ToString(Remarks);
								vd3.DebitAmount = Conversion.ToDouble(item.ReceivedAmount);
								vd3.ItemId = Conversion.ToInt(item.ItemId);
								vd3.QtyIn = Conversion.ToDouble(item.ReceivedQty);
								vd3.ItemRate = Conversion.ToDouble(item.ReceivedRate);
								vd3.WeightIn = Conversion.ToDouble(item.ReceivedQty);
								vd3.ItemAmount = Conversion.ToDouble(item.ReceivedAmount);
								vd3.SupplierCustomerId = Conversion.ToInt(LocationDebitAcId);
								vh.voucherDetailList.Add(vd3);
								vd2.LineId = Conversion.ToInt(LineId);
								vd2.AccountId = Conversion.ToInt(LocationDebitAcId);
								vd2.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd2.Comments = Conversion.ToString(Remarks);
								vd2.CreditAmount = Conversion.ToDouble(item.ReceivedAmount);
								vd2.ItemId = Conversion.ToInt(item.ItemId);
								vd2.QtyIn = Conversion.ToDouble(item.ReceivedQty);
								vd2.ItemRate = Conversion.ToDouble(item.ReceivedRate);
								vd2.ItemAmount = Conversion.ToDouble(item.ReceivedAmount);
								vd2.SupplierCustomerId = Conversion.ToInt(LocationDebitAcId);
								vh.voucherDetailList.Add(vd2);
								if (item.ExpenseAmount > 0.0)
								{
									if (dtItemOther.Count <= 0)
									{
										throw new Exception("OtherItems Record Not found");
									}
									List<InventoryItemsOther> findOther = dtItemOther.Where((InventoryItemsOther c) => c.Id == Conversion.ToInt(item.ItemId)).ToList();
									if (findOther.Count <= 0)
									{
										throw new Exception("OtherItems Record Not found");
									}
									VoucherDetail vd4 = new VoucherDetail();
									vd4.AccountId = Conversion.ToInt(findOther[0].StockGLAcId);
									vd4.AgainstAccountId = Conversion.ToInt(LocationDebitAcId);
									vd4.Comments = Conversion.ToString("Other Expenses");
									vd4.DebitAmount = Conversion.ToDouble(item.ExpenseAmount);
									vh.voucherDetailList.Add(vd4);
									VoucherDetail vd5 = new VoucherDetail();
									vd5.AccountId = Conversion.ToInt(LocationDebitAcId);
									vd5.AgainstAccountId = Conversion.ToInt(findOther[0].StockGLAcId);
									vd5.Comments = Conversion.ToString("Other Expenses");
									vd5.CreditAmount = Conversion.ToDouble(item.ExpenseAmount);
									vh.voucherDetailList.Add(vd5);
								}
								continue;
							}
							throw new Exception("Item Record Not found");
						}
						throw new Exception("Item Record Not found");
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

	public static int Save(Architecture.Model.POS.WsRmStockReceivedNotes obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
				return new Architecture.DAL.Pos.WsRmStockReceivedNotes().SetData(obj, "Sp_WsRmStockReceivedNotes_Insert");
			}
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			return new Architecture.DAL.Pos.WsRmStockReceivedNotes().SetData(obj, "Sp_WsRmStockReceivedNotes_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.POS.WsRmStockReceivedNotes GetByID(int Id)
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
			new List<Architecture.Model.POS.WsRmStockReceivedNotes>();
			return new Architecture.DAL.Pos.WsRmStockReceivedNotes().GetData("Sp_WsRmStockReceivedNotes_GetAllMethod", p)[0];
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
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmStockReceivedNotes>("Sp_WsRmStockReceivedNotes_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateDocCode(Architecture.Model.POS.WsRmStockReceivedNotes obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmStockReceivedNotes>("Sp_WsRmStockReceivedNotes_GetAllMethod", p);
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

	public static DataTable GetLoaderDataForWsRmStockReceivedNotes(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetLoaderDataForWsRmStockReceivedNotes"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmStockReceivedNotes>("Sp_WsRmStockReceivedNotes_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ApproveWsRmStockRecevingNote(Architecture.Model.POS.WsRmStockReceivedNotes obj)
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
				Value = "ApproveWsRmStockRecevingNote"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmStockReceivedNotes>("Sp_WsRmStockReceivedNotes_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
