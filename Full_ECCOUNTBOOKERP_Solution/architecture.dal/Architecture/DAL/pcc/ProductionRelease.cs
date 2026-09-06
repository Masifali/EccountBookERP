using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;

namespace Architecture.DAL.pcc;

public class ProductionRelease
{
	public int SetData(Architecture.Model.pcc.ProductionRelease objMain, string ProcName)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		string RefIds = "";
		List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.pcc.ProductionRelease obj in objMain.ProductionReleaseslist)
			{
				RefIds = RefIds + "," + obj.RefDocId;
				if (dtitem.Count == 0)
				{
					dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
				}
				objMain.OrganizationId = obj.OrganizationId;
				objMain.CompanyId = obj.CompanyId;
				obj.EntryDate = DateTime.Now;
				obj.ModifyDate = DateTime.Now;
				obj.DocumentTypeId = 1857;
				obj.ActionId = 1;
				if (obj.Id > 0)
				{
					ProcName = "[pcc].[USP_ProductionRelease_Update]";
					obj.ActionId = 2;
				}
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
				if (returnID > 0)
				{
					obj.Id = returnID;
				}
				else
				{
					returnID = obj.Id;
				}
				InventoryTransactions transactions = new InventoryTransactions();
				transactions.OrganizationId = obj.OrganizationId;
				transactions.CompanyId = obj.CompanyId;
				transactions.RefDocumentTypeId = obj.DocumentTypeId;
				transactions.RefDocIdNo = returnID;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
				InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
				inventoryTransactions.OrganizationId = obj.OrganizationId;
				inventoryTransactions.CompanyId = obj.CompanyId;
				inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
				inventoryTransactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
				obj.VoucherHeadInvoices = MakeVoucher(obj, dtitem);
				if (obj.ActionId == 2)
				{
					DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
					if (dtvh.Rows.Count > 0)
					{
						VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
						obj.VoucherHeadInvoices.Id = VoucherHeadIdGet;
					}
				}
				if (VoucherHeadIdGet == 0)
				{
					obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Insert"));
				}
				else
				{
					obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Update"));
				}
				if (VoucherHeadId > 0)
				{
					obj.VoucherHeadInvoices.Id = VoucherHeadId;
				}
				else
				{
					VoucherHeadId = obj.VoucherHeadInvoices.Id;
				}
				if (obj.VoucherHeadInvoices.voucherDetailList.Count <= 0 || obj.VoucherHeadInvoices.voucherDetailList == null)
				{
					continue;
				}
				foreach (VoucherDetail item in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					item.VoucherHeadId = VoucherHeadId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_Insert"));
				}
				if (obj.VoucherHeadInvoices.Id > 0)
				{
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "USP_VoucherBalanceCheck";
					sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand.Parameters.AddWithValue("@Id", obj.VoucherHeadInvoices.Id);
					sqlCommand.ExecuteNonQuery();
				}
			}
			SqlCommand sqlCommand2 = new SqlCommand();
			sqlCommand2.Connection = sqlcon;
			sqlCommand2.Transaction = sqlTrn;
			sqlCommand2.CommandType = CommandType.StoredProcedure;
			sqlCommand2.CommandText = "[pcc].[Usp_ProductionReleaseValidation]";
			sqlCommand2.Parameters.AddWithValue("@OrganizationId", objMain.OrganizationId);
			sqlCommand2.Parameters.AddWithValue("@CompanyId", objMain.CompanyId);
			sqlCommand2.Parameters.AddWithValue("@RefDocIds", RefIds);
			sqlCommand2.ExecuteNonQuery();
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static VoucherHead MakeVoucher(Architecture.Model.pcc.ProductionRelease obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead voucherHead = new VoucherHead();
		double voucheramt = 0.0;
		try
		{
			Architecture.Model.pcc.ProductionHeader objPd = new Architecture.Model.pcc.ProductionHeader();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.RefDocId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			List<Architecture.Model.pcc.ProductionHeader> lst = new List<Architecture.Model.pcc.ProductionHeader>();
			lst = new ProductionHeader().GetData("[pcc].[USP_ProductionHeader_GetAllMethod]", p);
			if (lst.Count <= 0)
			{
				throw new Exception("Production output object Not found");
			}
			objPd = lst[0];
			if (objPd.WIPGLAccountID == 0)
			{
				throw new Exception("WipAccountId not found");
			}
			voucherHead.DocumentTypeSrNo = obj.Id;
			voucherHead.VoucherDate = obj.DocDate;
			voucherHead.VoucherCode = obj.DocNo;
			voucherHead.DocumentTypeId = obj.DocumentTypeId;
			voucherHead.EntryDate = DateTime.Now;
			voucherHead.ModifyDate = DateTime.Now;
			voucherHead.EntryUser = obj.EntryUserId;
			voucherHead.ModifyUser = obj.ModifyUserId;
			voucherHead.OrganizationId = obj.OrganizationId;
			voucherHead.CompanyId = obj.CompanyId;
			voucherHead.FinancialYearId = obj.FinancialYearId;
			voucherHead.BranchId = obj.BranchesId;
			voucherHead.IsApproved = true;
			voucherHead.voucherDetailList = new List<VoucherDetail>();
			int LineId = 1;
			foreach (ProductionOutPutDetail item in objPd.ProductionOutPutDetailList.ToList())
			{
				string Remarks = "";
				if (item.ActionTypeId != 3)
				{
					if (dtitem.Count <= 0)
					{
						throw new Exception("Item GL Account Not Found");
					}
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find.Count > 0)
					{
						Remarks = ((!(item.Remarks == string.Empty)) ? (item.Remarks + " ItemName: " + find[0].ItemName + " Qty  " + item.ItemQty + " Rate  " + item.Rate) : ("ItemName: " + find[0].ItemName + " Qty  " + item.ItemQty + " Rate  " + item.Rate));
						VoucherDetail vd1 = new VoucherDetail();
						vd1.LineId = LineId;
						vd1.AccountId = Conversion.ToInt(Conversion.ToInt(find[0].PurchaseGLAC));
						vd1.AgainstAccountId = Conversion.ToInt(objPd.WIPGLAccountID);
						vd1.Comments = Conversion.ToString(Remarks);
						vd1.DebitAmount = Conversion.ToDouble(item.Amount);
						vd1.QtyIn = Conversion.ToDouble(item.ItemQty);
						vd1.WeightIn = Conversion.ToDouble(item.NetWeight);
						vd1.ItemRate = Conversion.ToDouble(item.Rate);
						vd1.ItemAmount = Conversion.ToDouble(item.Amount);
						vd1.OrderNo = Conversion.ToInt(item.SaleOrderId);
						vd1.JobLotId = Conversion.ToInt(item.JobLotId);
						voucheramt += vd1.DebitAmount;
						voucherHead.voucherDetailList.Add(vd1);
						VoucherDetail vd2 = new VoucherDetail();
						vd2.LineId = LineId;
						vd2.AccountId = Conversion.ToInt(objPd.WIPGLAccountID);
						vd2.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd2.Comments = Conversion.ToString(Remarks);
						vd2.CreditAmount = Conversion.ToDouble(item.Amount);
						vd2.QtyIn = Conversion.ToDouble(item.ItemQty);
						vd2.WeightIn = Conversion.ToDouble(item.NetWeight);
						vd2.ItemRate = Conversion.ToDouble(item.Rate);
						vd2.ItemAmount = Conversion.ToDouble(item.Amount);
						vd2.OrderNo = Conversion.ToInt(item.SaleOrderId);
						vd2.JobLotId = Conversion.ToInt(item.JobLotId);
						voucherHead.voucherDetailList.Add(vd2);
						LineId++;
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		voucherHead.VoucherAmount = voucheramt;
		return voucherHead;
	}
}
