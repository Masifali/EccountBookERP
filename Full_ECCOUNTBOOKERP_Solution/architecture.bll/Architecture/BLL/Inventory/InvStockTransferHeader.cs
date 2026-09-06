using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.lgstcm;

namespace Architecture.BLL.Inventory;

public class InvStockTransferHeader
{
	public static DataTable GetBranchDataFromStockTransfer(int OrganizationId, int CompanyId, int GpInwardId)
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
				ParameterName = "@GpInwardId",
				Value = GpInwardId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockTransferHeader>("USP_GetBranchDataFromStockTransfer", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDeliveryOrderDataForStockTransfer(int OrganizationId, int CompanyId, int GpId, int DoId)
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
				ParameterName = "@GpId",
				Value = GpId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DeliverOrderId",
				Value = DoId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockTransferHeader>("[dbo].[USP_GetDeliveryOrderDataForStockTransfer]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static VoucherHead MakeVoucher(Architecture.Model.Inventory.InvStockTransferHeader obj)
	{
		List<Architecture.Model.Inventory.GetItemGlIdsandItemName> dtitem = new List<Architecture.Model.Inventory.GetItemGlIdsandItemName>();
		new DataTable();
		VoucherHead vh = new VoucherHead();
		try
		{
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
			vh.ProjectId = obj.ProjectsId;
			vh.ManualBillNo = Conversion.ToString(obj.WorkingReportNo);
			vh.DueDate = DateTime.Now;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (obj.InvStockTransferDetail.Count > 0 && obj.InvStockTransferExpense.Count > 0)
			{
				string Remarks = string.Empty;
				double TotalAmount = 0.0;
				foreach (InvStockTransferDetail item2 in obj.InvStockTransferDetail.ToList())
				{
					if (dtitem.Count > 0)
					{
						List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find = dtitem.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
						if (find.Count > 0)
						{
							Remarks = "Item: " + Conversion.ToString(find[0].ItemName) + ",   Qty: " + item2.Qty + ",   Weight: " + item2.NetWeight + ",   Rate:" + item2.ItemRate + "  Item Amount: " + item2.ItemAmount + "  ExpenseAmount: " + item2.ExpenseAmount;
							VoucherDetail vd3 = new VoucherDetail();
							vd3.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd3.AgainstAccountId = vd3.AccountId;
							vd3.Comments = Conversion.ToString(Remarks);
							vd3.DebitAmount = Conversion.ToDouble(item2.ExpenseAmount);
							vd3.ItemId = Conversion.ToInt(item2.ItemId);
							vd3.QtyIn = Conversion.ToDouble(item2.Qty);
							vd3.ItemRate = Conversion.ToDouble(item2.ItemRate);
							vd3.WeightIn = Conversion.ToDouble(item2.NetWeight);
							vd3.ItemAmount = Conversion.ToDouble(item2.ItemAmount);
							vd3.BranchesId = obj.BranchesId;
							vh.voucherDetailList.Add(vd3);
						}
						continue;
					}
					throw new Exception("Item Record Not found");
				}
				foreach (InvStockTransferExpense item in obj.InvStockTransferExpense.ToList())
				{
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = Conversion.ToInt(item.AccountId);
					vd2.AgainstAccountId = Conversion.ToInt(vd2.AccountId);
					vd2.Comments = Conversion.ToString(item.Remarks);
					vd2.QtyIn = Conversion.ToDouble(item.Qty);
					vd2.ItemRate = Conversion.ToDouble(item.Rate);
					vd2.CreditAmount = Conversion.ToDouble(item.Amount);
					vd2.BranchesId = obj.BranchesId;
					TotalAmount += item.Amount;
					vh.voucherDetailList.Add(vd2);
				}
				vh.VoucherAmount = TotalAmount;
				vh.BillAmount = TotalAmount;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		return vh;
	}

	public static int Save(Architecture.Model.Inventory.InvStockTransferHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			if (obj.Id == 0)
			{
				obj.ModifyUser = 0;
				return new Architecture.DAL.Inventory.InvStockTransferHeader().SetData(obj, "Sp_InvStockTransferHeader_Insert");
			}
			obj.EntryUser = 0;
			obj.PreviousDate = GetRecordsById(obj.Id);
			return new Architecture.DAL.Inventory.InvStockTransferHeader().SetData(obj, "Sp_InvStockTransferHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvStockTransferHeader GetByID(int Id)
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
			new List<Architecture.Model.Inventory.InvStockTransferHeader>();
			return new Architecture.DAL.Inventory.InvStockTransferHeader().GetData("Sp_InvStockTransferHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Inventory.InvStockTransferHeader obj)
	{
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenrateCode"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockTransferHeader>("Sp_InvStockTransferHeader_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
				});
			}
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocDateTo",
					Value = obj.ToDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryFromDate",
					Value = obj.EntryDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryToDate",
					Value = obj.EntryDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyFromDate",
					Value = obj.ModifyDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyToDate",
					Value = obj.ModifyDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedFromDate",
					Value = obj.ApprovedDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedToDate",
					Value = obj.ApprovedDateTo
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDocNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDocNo",
					Value = obj.ToDocNo
				});
			}
			if (obj.TransferType != null && obj.TransferType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TransferType",
					Value = obj.TransferType
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockTransferHeader>("Sp_InvStockTransferHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable StockTransferSlipandRegister(ReportsParameters obj)
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			if (obj.FromWarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromWarehouseId",
					Value = obj.FromWarehouseId
				});
			}
			if (obj.ToWarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToWarehouseId",
					Value = obj.ToWarehouseId
				});
			}
			if (obj.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
				});
			}
			if (obj.ToJobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToJobLotId",
					Value = obj.ToJobLotId
				});
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockTransferHeader>("Sp_InvStockTransfer_SlipandRegister", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetIdByDocNo(Architecture.Model.Inventory.InvStockTransferHeader obj)
	{
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
				ParameterName = "@DocNo",
				Value = obj.DocNo
			});
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadIdByDocNo"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockTransferHeader>("Sp_InvStockTransferHeader_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingRecordsForStockTransfer(Architecture.Model.Inventory.InvStockTransferHeader obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockTransferHeader>("Sp_InvStockTransferHeader_PendingRecordsForStockTransfer", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingRecordsForStockTransfer(ReportsParameters obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchIds",
				Value = obj.BranchIds
			});
			if (obj.TransactionTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TransferTypeId",
					Value = obj.TransactionTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockTransferHeader>("Sp_InvStockTransferHeader_PendingRecordsForStockTransferNew", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemIdFromDeliveryOrderForStockTransfer(Architecture.Model.Inventory.InvStockTransferHeader obj)
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
				Value = obj.GatePassId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetItemIdFromDeliveryOrderForStockTransfer"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockTransferHeader>("Sp_InvStockTransferHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DateTime GetRecordsById(int Id)
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
				Value = "GetRecordId"
			});
			return Conversion.ToDateTime(GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("Sp_InvStockTransferHeader_GetAllMethod", p).Rows[0][0]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable StockTransferPackingMaterialAndStore_SlipandRegister(ReportsParameters obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			if (obj.FromWarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromWarehouseId",
					Value = obj.FromWarehouseId
				});
			}
			if (obj.ToWarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToWarehouseId",
					Value = obj.ToWarehouseId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockTransferHeader>("Sp_InvStockTransferPackingMaterialAndStore_SlipandRegister", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable StockTransferHistoryForApproval(ReportsParameters obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "TransferType",
					Value = obj.Activity
				});
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			if (obj.FromWarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromWarehouseId",
					Value = obj.FromWarehouseId
				});
			}
			if (obj.ToWarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToWarehouseId",
					Value = obj.ToWarehouseId
				});
			}
			if (obj.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
				});
			}
			if (obj.ToJobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToJobLotId",
					Value = obj.ToJobLotId
				});
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = obj.PageSize
				});
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = obj.PageNumber
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockTransferHeader>("USP_InvStockTransferHistoryForApproval", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AllComboAgainstStockTransfer(ReportsParameters obj)
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
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockTransferHeader>("Usp_AllComboAgainstStockTransfer", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ApproveStockTransfer(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = item.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedUserId",
					Value = item.EntryUser
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "ApproveStockTransfer"
				});
				GenericProvider.GetDataTableProc<Architecture.Model.Inventory.PurchaseOrder>("Sp_InvStockTransferHeader_GetAllMethod", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SpecialApproveStockTransfer(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = item.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = item.EntryUser
				});
				GenericProvider.GetDataTableProc<Architecture.Model.Inventory.PurchaseOrder>("usp_StockTransferSpecialApproval", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBranchDataFromStockTransfer(int OrganizationId, int CompanyId, int BranchId, int GpInwardId)
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
				ParameterName = "@BranchesId",
				Value = BranchId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@GpInwardId",
				Value = GpInwardId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockTransferHeader>("USP_GetBranchDataFromStockTransfer", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBranchesAllocatedToUserFromStockTransfer(int OrganizationId, int CompanyId, int UserId, int DocumentTypeId = 0)
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
				ParameterName = "@UserId",
				Value = UserId
			});
			if (DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = DocumentTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.BranchesAllocationToUser>("[dbo].[USP_GetBranchsAllocatedToUserFromSTockTransfer]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet StockTransferWeightAudit(int OrganizationId, int CompanyId, DateTime FromDate, DateTime ToDate, int ActionId = 0)
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
				ParameterName = "@FromDate",
				Value = FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = ToDate
			});
			if (ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = ActionId
				});
			}
			return GenericProvider.GetDataSetProc<Architecture.Model.Inventory.InvStockTransferHeader>("usp_StockTransferWeightAudit", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable MoveOrder_PendingData(ReportsParameters obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesIds",
				Value = obj.BranchesIds
			});
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDocNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDocNo",
					Value = obj.ToDocNo
				});
			}
			if (!string.IsNullOrWhiteSpace(obj.VehicleNo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VehicleNo",
					Value = obj.VehicleNo
				});
			}
			if (!string.IsNullOrWhiteSpace(obj.BiltyNo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BiltyNo",
					Value = obj.BiltyNo
				});
			}
			return GenericProvider.GetDataTableProc<PurchaseOrderHeader>("[dbo].[USP_MoveOrder_PendingData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
