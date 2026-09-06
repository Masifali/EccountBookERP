using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.ModernHistoryModels;

namespace Architecture.BLL.ModernHistories;

public class ModernHistory
{
	public static PurchaseOrderActivityDetail PurchaseOrder_Activity(int OrganizationId, int CompanyId, int FinancialYearId, int PurchaseOrderId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PurchaseOrderId",
				Value = PurchaseOrderId
			});
			DataSet ds = GenericProvider.GetDataSetProc<PurchaseOrderActivityDetail>("[dbo].[Sp_PurchaseOrder_ActivityDetail]", p);
			PurchaseOrderActivityDetail PO_Activity = new PurchaseOrderActivityDetail();
			if (ds.Tables[0].Rows.Count > 0)
			{
				PO_Activity.PurchaseOrderInfoList = Conversion.ConvertDataTableToList<PuchaseOrderActivity_PoInfo>(ds.Tables[0]);
			}
			if (ds.Tables[1].Rows.Count > 0)
			{
				PO_Activity.GatePassInfoList = Conversion.ConvertDataTableToList<PuchaseOrderActivity_GpInfo>(ds.Tables[1]);
			}
			if (ds.Tables[2].Rows.Count > 0)
			{
				PO_Activity.WeighBridgeInfoList = Conversion.ConvertDataTableToList<PurchaseOrderActivity_WbInfo>(ds.Tables[2]);
			}
			if (ds.Tables[3].Rows.Count > 0)
			{
				PO_Activity.GoodsReceiptNote = Conversion.ConvertDataTableToList<PurchaseOrderActivity_GrnInfo>(ds.Tables[3]);
			}
			if (ds.Tables[4].Rows.Count > 0)
			{
				PO_Activity.PurchaseInvoiceInfo = Conversion.ConvertDataTableToList<PurchaseOrderActivity_InvoiceInfo>(ds.Tables[4]);
			}
			if (ds.Tables[5].Rows.Count > 0 && ds.Tables[5] != null)
			{
				PO_Activity.OrderTotalWeight = Conversion.ToDouble(ds.Tables[5].Rows[0]["OrderTotalWeight"]);
				PO_Activity.OrderReceivedWeight = Conversion.ToDouble(ds.Tables[5].Rows[0]["OrderReceivedWeight"]);
				PO_Activity.OrderBalanceWeight = Conversion.ToDouble(ds.Tables[5].Rows[0]["OrderBalanceWeight"]);
				PO_Activity.OrderQty = Conversion.ToDouble(ds.Tables[5].Rows[0]["OrderQty"]);
				PO_Activity.OrderReceivedQty = Conversion.ToDouble(ds.Tables[5].Rows[0]["OrderReceivedQty"]);
				PO_Activity.OrderBalanceQty = Conversion.ToDouble(ds.Tables[5].Rows[0]["OrderBalanceQty"]);
			}
			if (ds.Tables[6].Rows.Count > 0)
			{
				PO_Activity.WagesInfoList = Conversion.ConvertDataTableToList<PurchaseOrderActivity_WagesInfo>(ds.Tables[6]);
			}
			if (ds.Tables[7].Rows.Count > 0)
			{
				PO_Activity.LabInfoList = Conversion.ConvertDataTableToList<PurchaseOrderActivity_LabInfo>(ds.Tables[7]);
			}
			if (ds.Tables[8].Rows.Count > 0)
			{
				PO_Activity.OrderEmptyBagsInfoList = Conversion.ConvertDataTableToList<PurchaseOrderActivity_OrderEmptyBagsInfo>(ds.Tables[8]);
			}
			return PO_Activity;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static SaleOrderActivityDetail SaleOrder_Activity(int OrganizationId, int CompanyId, int FinancialYearId, int SaleOrderId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SaleOrderId",
				Value = SaleOrderId
			});
			DataSet ds = GenericProvider.GetDataSetProc<Purchase_Sale_InvoiceHistory>("[dbo].[Sp_SaleOrder_ActivityDetail]", p);
			SaleOrderActivityDetail SO_Activity = new SaleOrderActivityDetail();
			if (ds.Tables.Count > 0)
			{
				SO_Activity.SaleOrderInfoList = Conversion.ConvertDataTableToList<SaleOrderActivity_SoInfo>(ds.Tables[0]);
			}
			if (ds.Tables.Count > 1)
			{
				SO_Activity.DeliveryOrderInfoList = Conversion.ConvertDataTableToList<SaleOrderActivity_DoInfo>(ds.Tables[1]);
			}
			if (ds.Tables.Count > 2)
			{
				SO_Activity.GatePassInfoList = Conversion.ConvertDataTableToList<SaleOrderActivity_GpInfo>(ds.Tables[2]);
			}
			if (ds.Tables.Count > 3)
			{
				SO_Activity.WeighBridgeInfoList = Conversion.ConvertDataTableToList<SaleOrderActivity_WbInfo>(ds.Tables[3]);
			}
			if (ds.Tables.Count > 4)
			{
				SO_Activity.GoodsDispatchtNotesInfoList = Conversion.ConvertDataTableToList<SaleOrderActivity_GdnInfo>(ds.Tables[4]);
			}
			if (ds.Tables.Count > 5)
			{
				SO_Activity.InvoiceInfoList = Conversion.ConvertDataTableToList<SaleOrderActivity_InvoiceInfo>(ds.Tables[5]);
			}
			if (ds.Tables.Count > 6)
			{
				SO_Activity.OrderTotalWeight = Conversion.ToDouble(ds.Tables[6].Rows[0]["OrderTotalWeight"]);
				SO_Activity.OrderReceivedWeight = Conversion.ToDouble(ds.Tables[6].Rows[0]["OrderReceivedWeight"]);
				SO_Activity.OrderBalanceWeight = Conversion.ToDouble(ds.Tables[6].Rows[0]["OrderBalanceWeight"]);
			}
			if (ds.Tables.Count > 7)
			{
				SO_Activity.WagesInfoList = Conversion.ConvertDataTableToList<SaleOrderActivity_WagesInfo>(ds.Tables[7]);
			}
			return SO_Activity;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<RiceOrdersHistory> PurchaseOrderModernHistory(int Id)
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
				Value = "PurchaseOrderModernHistory"
			});
			return RiceOrderModernHistory(GenericProvider.GetDataSetProc<Purchase_Sale_InvoiceHistory>("[dbo].[Sp_PurchaseOrder_GetAllMethod]", p));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<RiceOrdersHistory> SaleOrderModernHistory(int Id)
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
				Value = "SaleOrderModernHistory"
			});
			return RiceOrderModernHistory(GenericProvider.GetDataSetProc<Purchase_Sale_InvoiceHistory>("[dbo].[Sp_SaleOrder_GetAllMethod]", p));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<RiceOrdersHistory> RiceOrderModernHistory(DataSet ds)
	{
		try
		{
			List<RiceOrdersHistory> OrderHistoryList = new List<RiceOrdersHistory>();
			List<OrderHeaderModel> OrderHeaderList = new List<OrderHeaderModel>();
			List<OrderDetailModel> OrderDetailList = new List<OrderDetailModel>();
			List<OrderEmptyBagsModel> OrderEmptyBagsList = new List<OrderEmptyBagsModel>();
			List<OrderExpensesModel> OrderExpensesList = new List<OrderExpensesModel>();
			if (ds.Tables.Count > 0)
			{
				OrderHeaderList = Conversion.ConvertDataTableToList<OrderHeaderModel>(ds.Tables[0]);
			}
			if (ds.Tables.Count > 1)
			{
				OrderDetailList = Conversion.ConvertDataTableToList<OrderDetailModel>(ds.Tables[1]);
			}
			if (ds.Tables.Count > 2)
			{
				OrderEmptyBagsList = Conversion.ConvertDataTableToList<OrderEmptyBagsModel>(ds.Tables[2]);
			}
			if (ds.Tables.Count > 3)
			{
				OrderExpensesList = Conversion.ConvertDataTableToList<OrderExpensesModel>(ds.Tables[3]);
			}
			for (int i = 0; i < OrderHeaderList.Count; i++)
			{
				RiceOrdersHistory Item = new RiceOrdersHistory();
				Item.OrderHeader = OrderHeaderList[i];
				Item.OrderDetail = OrderDetailList?.FindAll((OrderDetailModel detail) => detail.OrderId == Item.OrderHeader.OrderId);
				Item.OrderEmptyBags = OrderEmptyBagsList?.FindAll((OrderEmptyBagsModel bags) => bags.OrderId == Item.OrderHeader.OrderId);
				Item.OrderExpenses = OrderExpensesList?.FindAll((OrderExpensesModel expenses) => expenses.OrderId == Item.OrderHeader.OrderId);
				OrderHistoryList.Add(Item);
			}
			return OrderHistoryList;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static VouchersModernHistory VouchersModernHisotry(int OrganizationId, int CompanyId, int Id, string DocumentTypeIds)
	{
		VouchersModernHistory History = new VouchersModernHistory();
		List<SqlParameter> p = new List<SqlParameter>();
		p.Add(new SqlParameter
		{
			ParameterName = "@OrganizationId",
			Value = OrganizationId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CompanyId",
			Value = CompanyId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@Id",
			Value = Id
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@DocumentTypeName",
			Value = DocumentTypeIds
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@Activity",
			Value = "VouchersModernHisotry"
		});
		DataSet ds = GenericProvider.GetDataSetProc<VouchersModernHistory>("[dbo].[Sp_Vouchers_GetMethods]", p);
		List<VouchersHistory_Header> HeaderInfo = new List<VouchersHistory_Header>();
		List<VoucherHistory_Detail> DetailInfo = new List<VoucherHistory_Detail>();
		if (ds.Tables.Count > 0)
		{
			HeaderInfo = Conversion.ConvertDataTableToList<VouchersHistory_Header>(ds.Tables[0]);
		}
		if (ds.Tables.Count > 1)
		{
			DetailInfo = Conversion.ConvertDataTableToList<VoucherHistory_Detail>(ds.Tables[1]);
		}
		if (HeaderInfo != null && HeaderInfo.Count > 0)
		{
			History.VoucherHistoryHeader = HeaderInfo[0];
			History.VoucherHistoryDetail = DetailInfo;
		}
		return History;
	}

	public static PurchaseInvoiceReverseActivityDetail PurchaseInvoiceReverseActivityDetail(int OrganizationId, int CompanyId, int InvoiceId)
	{
		PurchaseInvoiceReverseActivityDetail ActivityDetail = new PurchaseInvoiceReverseActivityDetail();
		List<SqlParameter> p = new List<SqlParameter>();
		p.Add(new SqlParameter
		{
			ParameterName = "@OrganizationId",
			Value = OrganizationId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CompanyId",
			Value = CompanyId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@PurchaseInvoiceId",
			Value = InvoiceId
		});
		DataSet ds = GenericProvider.GetDataSetProc<Purchase_Sale_InvoiceHistory>("[dbo].[InvPurchaseInvoice_ReverseActivityDetail]", p);
		if (ds.Tables.Count > 0)
		{
			ActivityDetail.InvoiceInfo = Conversion.ConvertDataTableToList<PurchaseInvoiceReverseActivityDetail_InvoiceInfo>(ds.Tables[0]);
		}
		if (ds.Tables.Count > 1)
		{
			ActivityDetail.GrnInfoList = Conversion.ConvertDataTableToList<PurchaseInvoiceReverseActivityDetail_GrnInfo>(ds.Tables[1]);
		}
		if (ds.Tables.Count > 2)
		{
			ActivityDetail.WagesInfoList = Conversion.ConvertDataTableToList<PurchaseInvoiceReverseActivityDetail_WagesInfo>(ds.Tables[2]);
		}
		if (ds.Tables.Count > 3)
		{
			ActivityDetail.LabInfoList = Conversion.ConvertDataTableToList<PurchaseInvoiceReverseActivityDetail_LabInfo>(ds.Tables[3]);
		}
		if (ds.Tables.Count > 4)
		{
			ActivityDetail.WbInfoList = Conversion.ConvertDataTableToList<PurchaseInvoiceReverseActivityDetail_WbId>(ds.Tables[4]);
		}
		if (ds.Tables.Count > 5)
		{
			ActivityDetail.GpInfoList = Conversion.ConvertDataTableToList<PurchaseInvoiceReverseActivityDetail_GpInfo>(ds.Tables[5]);
		}
		if (ds.Tables.Count > 6)
		{
			ActivityDetail.PoInfoList = Conversion.ConvertDataTableToList<PurchaseInvoiceReverseActivityDetail_PoInfo>(ds.Tables[6]);
		}
		return ActivityDetail;
	}

	public static SaleInvoiceReverseActivityDetail SaleInvoiceReverseActivityDetail(int OrganizationId, int CompanyId, int InvoiceId)
	{
		SaleInvoiceReverseActivityDetail ActivityDetail = new SaleInvoiceReverseActivityDetail();
		List<SqlParameter> p = new List<SqlParameter>();
		p.Add(new SqlParameter
		{
			ParameterName = "@OrganizationId",
			Value = OrganizationId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CompanyId",
			Value = CompanyId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@InvSaleInvoiceId",
			Value = InvoiceId
		});
		DataSet ds = GenericProvider.GetDataSetProc<SaleInvoiceReverseActivityDetail>("[dbo].[InvSaleInvoice_ReverseActivityDetail]", p);
		if (ds.Tables.Count > 0)
		{
			ActivityDetail.InvoiceInfo = Conversion.ConvertDataTableToList<SaleInvoiceReverseActivityDetail_InvoiceInfo>(ds.Tables[0]);
		}
		if (ds.Tables.Count > 1)
		{
			ActivityDetail.GdnInfoList = Conversion.ConvertDataTableToList<SaleInvoiceReverseActivityDetail_GdnInfo>(ds.Tables[1]);
		}
		if (ds.Tables.Count > 2)
		{
			ActivityDetail.WagesInfoList = Conversion.ConvertDataTableToList<SaleInvoiceReverseActivityDetail_WagesInfo>(ds.Tables[2]);
		}
		if (ds.Tables.Count > 3)
		{
			ActivityDetail.WbInfoList = Conversion.ConvertDataTableToList<SaleInvoiceReverseActivityDetail_WbInfo>(ds.Tables[3]);
		}
		if (ds.Tables.Count > 4)
		{
			ActivityDetail.GpInfoList = Conversion.ConvertDataTableToList<SaleInvoiceReverseActivityDetail_GpInfo>(ds.Tables[4]);
		}
		if (ds.Tables.Count > 5)
		{
			ActivityDetail.DoInfoList = Conversion.ConvertDataTableToList<SaleInvoiceReverseActivityDetail_DoInfo>(ds.Tables[5]);
		}
		if (ds.Tables.Count > 6)
		{
			ActivityDetail.SoInfoList = Conversion.ConvertDataTableToList<SaleInvoiceReverseActivityDetail_SoInfo>(ds.Tables[6]);
		}
		return ActivityDetail;
	}

	public static List<ContractorWagesBillDirect> ContractorWagesBillDirectModernHistory(int OrganizationId, int CompanyId, int FinancialYearId, int NoOfRecords, int EntryUser, bool CanViewAllRecords, string ApprovedFilter, bool IsApproved)
	{
		List<ContractorWagesBillDirect> History = new List<ContractorWagesBillDirect>();
		List<SqlParameter> p = new List<SqlParameter>();
		p.Add(new SqlParameter
		{
			ParameterName = "@OrganizationId",
			Value = OrganizationId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CompanyId",
			Value = CompanyId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@FinancialYearId",
			Value = FinancialYearId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CanViewAllRecord",
			Value = CanViewAllRecords
		});
		if (!CanViewAllRecords)
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUser",
				Value = EntryUser
			});
		}
		if (NoOfRecords > 0)
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@NoOfRecords",
				Value = NoOfRecords
			});
		}
		if (ApprovedFilter != "All")
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@IsApproved",
				Value = IsApproved
			});
		}
		p.Add(new SqlParameter
		{
			ParameterName = "@Activity",
			Value = "ModernHistory"
		});
		DataSet ds = GenericProvider.GetDataSetProc<Purchase_Sale_InvoiceHistory>("[dbo].[Sp_InvContractorWagesBillDirect_GetAllMethod]", p);
		List<ContractorWagesBillDirect_Header> HeaderList = new List<ContractorWagesBillDirect_Header>();
		List<ContractorWagesBillDirect_Detail> DetailList = new List<ContractorWagesBillDirect_Detail>();
		if (ds.Tables.Count > 0)
		{
			HeaderList = Conversion.ConvertDataTableToList<ContractorWagesBillDirect_Header>(ds.Tables[0]);
		}
		if (ds.Tables.Count > 1)
		{
			DetailList = Conversion.ConvertDataTableToList<ContractorWagesBillDirect_Detail>(ds.Tables[1]);
		}
		if (HeaderList.Count > 0)
		{
			foreach (ContractorWagesBillDirect_Header item in HeaderList)
			{
				ContractorWagesBillDirect HistoryObject = new ContractorWagesBillDirect();
				HistoryObject.BillHeader = item;
				HistoryObject.BillDetaiList = DetailList?.FindAll((ContractorWagesBillDirect_Detail d) => d.HeaderId == item.Id);
				double TotalBags = 0.0;
				double TotalAmount = 0.0;
				foreach (ContractorWagesBillDirect_Detail d2 in HistoryObject.BillDetaiList)
				{
					TotalBags += d2.Qty;
					TotalAmount += d2.Amount;
				}
				HistoryObject.BillHeader.TotalBags = TotalBags;
				HistoryObject.BillHeader.TotalAmount = TotalAmount;
				History.Add(HistoryObject);
			}
		}
		return History;
	}

	public static DataTable ContractorWages_AnalyticsDashboard(int OrganizationId, int CompanyId, string FromDate, string ToDate, string ReportType, int ContractorId = 0, int WagesItemId = 0)
	{
		new List<ContractorWagesBillDirect>();
		List<SqlParameter> p = new List<SqlParameter>();
		p.Add(new SqlParameter
		{
			ParameterName = "@OrganizationId",
			Value = OrganizationId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CompanyId",
			Value = CompanyId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@FromDate",
			Value = Conversion.ToDateTime(FromDate)
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@ToDate",
			Value = Conversion.ToDateTime(ToDate)
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@ReportType",
			Value = ReportType
		});
		if (ContractorId > 0)
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@ContractorId",
				Value = ContractorId
			});
		}
		if (WagesItemId > 0)
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@WagesItemId",
				Value = WagesItemId
			});
		}
		return GenericProvider.GetDataTableProc<Architecture.Model.Company>("[dbo].[Sp_ContractorWages_Analytics]", p);
	}

	public static DataTable PurchaseOrderModernHistory_HeaderData(int OrganizationId, int CompanyId, int FinancialYearId, int DocumentTypeId, int NoOfRecords, int EntryUser, bool CanViewAllRecords, string ApprovedFilter, bool IsApproved)
	{
		List<SqlParameter> p = new List<SqlParameter>();
		p.Add(new SqlParameter
		{
			ParameterName = "@OrganizationId",
			Value = OrganizationId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CompanyId",
			Value = CompanyId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@FinancialYearId",
			Value = FinancialYearId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@DocumentTypeId",
			Value = DocumentTypeId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CanViewAllRecord",
			Value = CanViewAllRecords
		});
		if (!CanViewAllRecords)
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUser",
				Value = EntryUser
			});
		}
		if (NoOfRecords > 0)
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@NoOfRecords",
				Value = NoOfRecords
			});
		}
		if (ApprovedFilter != "All")
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@IsApproved",
				Value = IsApproved
			});
		}
		p.Add(new SqlParameter
		{
			ParameterName = "@Activity",
			Value = "PurchaseOrderModernHistory_HeaderData"
		});
		return GenericProvider.GetDataTableProc<RiceOrdersHistory>("[dbo].[Sp_PurchaseOrder_GetAllMethod]", p);
	}

	public static DataTable SaleOrderModernHistory_HeaderData(int OrganizationId, int CompanyId, int FinancialYearId, int DocumentTypeId, int NoOfRecords, int EntryUser, bool CanViewAllRecords, string ApprovedFilter, bool IsApproved)
	{
		List<SqlParameter> p = new List<SqlParameter>();
		p.Add(new SqlParameter
		{
			ParameterName = "@OrganizationId",
			Value = OrganizationId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CompanyId",
			Value = CompanyId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@FinancialYearId",
			Value = FinancialYearId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@DocumentTypeId",
			Value = DocumentTypeId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CanViewAllRecord",
			Value = CanViewAllRecords
		});
		if (!CanViewAllRecords)
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUser",
				Value = EntryUser
			});
		}
		if (NoOfRecords > 0)
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@NoOfRecords",
				Value = NoOfRecords
			});
		}
		if (ApprovedFilter != "All")
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@IsApproved",
				Value = IsApproved
			});
		}
		p.Add(new SqlParameter
		{
			ParameterName = "@Activity",
			Value = "SaleOrderModernHistory_HeaderData"
		});
		return GenericProvider.GetDataTableProc<RiceOrdersHistory>("[dbo].[Sp_SaleOrder_GetAllMethod]", p);
	}

	public static DataTable VouchersModernHisotry_Header(int OrganizationId, int CompanyId, int FinancialYearId, string DocumentTypeIds, bool CanViewAllRecords, int EntryUser, int NoOfRecords, string ApprovedFilter, bool IsApproved)
	{
		new List<VouchersModernHistory>();
		List<SqlParameter> p = new List<SqlParameter>();
		p.Add(new SqlParameter
		{
			ParameterName = "@OrganizationId",
			Value = OrganizationId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CompanyId",
			Value = CompanyId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@FinancialYearId",
			Value = FinancialYearId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@DocumentTypeName",
			Value = DocumentTypeIds
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CanViewAllRecord",
			Value = CanViewAllRecords
		});
		if (!CanViewAllRecords)
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUser",
				Value = EntryUser
			});
		}
		if (NoOfRecords > 0)
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@NoOfRecords",
				Value = NoOfRecords
			});
		}
		if (ApprovedFilter != "All")
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@IsApproved",
				Value = IsApproved
			});
		}
		p.Add(new SqlParameter
		{
			ParameterName = "@Activity",
			Value = "VoucherModernHistory_CardData"
		});
		return GenericProvider.GetDataTableProc<VouchersHistory_Header>("[dbo].[Sp_Vouchers_GetMethods]", p);
	}
}
