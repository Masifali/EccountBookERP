using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Mfg;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Mfg;
using Architecture.Model.Production;
using Architecture.Model.pcc;

namespace Architecture.BLL.Mfg;

public class ProductionHeader
{
	public static VoucherHead MakeVoucher(Architecture.Model.Mfg.ProductionHeader obj)
	{
		VoucherHead voucherHead = new VoucherHead();
		double voucheramt = 0.0;
		try
		{
			List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
			new List<SupplierCustomerGlAccountId>();
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			voucherHead.DocumentTypeSrNo = obj.ProductionHeaderId;
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
			voucherHead.IsApproved = obj.IsApproved;
			voucherHead.voucherDetailList = new List<VoucherDetail>();
			if (obj.DocumentTypeId == 1670)
			{
				List<Architecture.Model.Mfg.ProductionOutPutDetail> productionOutPutDetailslist = obj.ProductionOutPutDetailslist;
				if (productionOutPutDetailslist != null && productionOutPutDetailslist.Count > 0)
				{
					int LineId = 1;
					string Remarks = "";
					foreach (Architecture.Model.Mfg.ProductionOutPutDetail item2 in obj.ProductionOutPutDetailslist.ToList())
					{
						if (item2.ActionTypeId != 3)
						{
							if (dtitem == null || dtitem.Count <= 0)
							{
								throw new Exception("Items record not found for accounts...");
							}
							List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
							if (find2.Count <= 0)
							{
								throw new Exception("Item GL Account Not Found Against this ItemId " + item2.ItemId);
							}
							Remarks = "ItemName: " + find2[0].ItemName + " Qty  " + item2.ItemQty + " Rate  " + item2.ItemRate;
							VoucherDetail vd3 = new VoucherDetail();
							vd3.LineId = LineId;
							vd3.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
							vd3.AgainstAccountId = vd3.AccountId;
							vd3.Comments = Conversion.ToString(Remarks);
							vd3.DebitAmount = Conversion.ToDouble(item2.Amount);
							vd3.QtyIn = Conversion.ToDouble(item2.ItemQty);
							vd3.WeightIn = Conversion.ToDouble(item2.NetWeight);
							vd3.ItemRate = Conversion.ToDouble(item2.ItemRate);
							vd3.ItemAmount = Conversion.ToDouble(item2.Amount);
							vd3.JobLotId = Conversion.ToInt(item2.JobLotId);
							voucherHead.voucherDetailList.Add(vd3);
							LineId++;
						}
					}
					LineId = 1;
					foreach (Architecture.Model.Mfg.ProductionInputDetail item in obj.ProductionInputDetailslist.ToList())
					{
						Remarks = "";
						if (item.ActionTypeId != 3)
						{
							if (dtitem == null || dtitem.Count <= 0)
							{
								throw new Exception("Items record not found for accounts...");
							}
							List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
							if (find.Count <= 0)
							{
								throw new Exception("Item GL Account Not Found Against this ItemId " + item.ItemId);
							}
							Remarks = "ItemName: " + find[0].ItemName + " Qty  " + item.Qty + " Rate  " + item.ItemRate;
							VoucherDetail vd2 = new VoucherDetail();
							vd2.LineId = LineId;
							vd2.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd2.AgainstAccountId = vd2.AccountId;
							vd2.Comments = Conversion.ToString(Remarks);
							vd2.CreditAmount = Conversion.ToDouble(item.Amount);
							vd2.QtyOut = Conversion.ToDouble(item.Qty);
							vd2.WeightOut = Conversion.ToDouble(item.Weight);
							vd2.ItemRate = Conversion.ToDouble(item.ItemRate);
							vd2.ItemAmount = Conversion.ToDouble(item.Amount);
							voucherHead.voucherDetailList.Add(vd2);
							voucheramt += vd2.CreditAmount;
							LineId++;
						}
					}
					List<Architecture.Model.Mfg.ProductionOverHeadsDetail> productionOverHeadsDetailList = obj.ProductionOverHeadsDetailList;
					if (productionOverHeadsDetailList != null && productionOverHeadsDetailList.Count > 0)
					{
						foreach (Architecture.Model.Mfg.ProductionOverHeadsDetail item3 in obj.ProductionOverHeadsDetailList.ToList())
						{
							Remarks = "";
							if (string.IsNullOrEmpty(item3.Remarks))
							{
								Remarks = "Account: " + item3.OverHeadAccountTitle;
								if (item3.Qty > 0m)
								{
									Remarks = Remarks + " Qty " + item3.Qty;
								}
								if (item3.Rate > 0m)
								{
									Remarks = Remarks + " Rate " + item3.Rate;
								}
								if (item3.Amount > 0m)
								{
									Remarks = Remarks + " Amount " + item3.Amount;
								}
							}
							else
							{
								Remarks = item3.Remarks;
							}
							VoucherDetail vd1 = new VoucherDetail();
							vd1.LineId = LineId;
							vd1.AccountId = Conversion.ToInt(item3.OverHeadAccountId);
							vd1.AgainstAccountId = vd1.AccountId;
							vd1.Comments = Conversion.ToString(Remarks);
							vd1.CreditAmount = Conversion.ToDouble(item3.Amount);
							voucherHead.voucherDetailList.Add(vd1);
							voucheramt += vd1.CreditAmount;
						}
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

	public static int Save(Architecture.Model.Mfg.ProductionHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.IsSettled == 1)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
			}
			if (obj.ProductionHeaderId == 0)
			{
				if (obj.ProductionOutPutDetailslist.Exists((Architecture.Model.Mfg.ProductionOutPutDetail p) => p.ActionTypeId != 1))
				{
					throw new Exception("Record cannot be inserted because ActionTypeId not equal to 1 in Output Detail");
				}
				obj.ActionId = 1;
				return new Architecture.DAL.Mfg.ProductionHeader().SetData(obj, "Mfg.USP_ProductionHeader_InsertAndUpdate");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.Mfg.ProductionHeader().SetData(obj, "Mfg.USP_ProductionHeader_InsertAndUpdate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Mfg.ProductionHeader ReadById(int Id)
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
			new List<Architecture.Model.Mfg.ProductionHeader>();
			return new Architecture.DAL.Mfg.ProductionHeader().GetData("Mfg.USP_ProductionHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int OrganizationId, int CompanyId, int DocumentTypeId, int EntryUserId, int Id)
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
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUserId",
				Value = EntryUserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeleteById"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Mfg.WorkOrder>("[Mfg].[USP_ProductionHeader_GetAllMethod]", p);
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUserId",
					Value = obj.EntryUser
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ProductionHeader>("Mfg.USP_ProductionHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Mfg.ProductionHeader obj)
	{
		int code = 0;
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Mfg.ProductionHeader>("Mfg.USP_ProductionHeader_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				code = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ProductionMfgSlipAndRegister(ReportsParameters obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.WorkOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WorkOrderId",
					Value = obj.WorkOrderId
				});
			}
			if (obj.OrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SaleOrderId",
					Value = obj.OrderId
				});
			}
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WareHouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
				});
			}
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
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
			if (obj.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ProductionHeader>("[Mfg].[USP_ProductionSlipAndRegister_Engr]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ProductionHeader_Slip(ReportsParameters obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Mfg.ProductionHeader>("[Mfg].[USP_ProductionHeader_Slip]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ProductionAllDetails_SubReport(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ProductionHeaderId",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Mfg.ProductionHeader>("[Mfg].[USP_ProductionAllDetails_SubReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromProductionHeader(ReportsParameters obj)
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
			if (obj.Activity != string.Empty || obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<InvFoodProduction>("[Mfg].[USP_GetDataForDropDownFromProductionHeader]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WorkOrderSemiFinishForProductionOutput(ReportsParameters obj)
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
			if (obj.WorkOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WorkOrderId",
					Value = obj.WorkOrderId
				});
			}
			if (obj.RecId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.RecId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@CastingTypeId",
				Value = obj.CastingTypeId
			});
			return GenericProvider.GetDataTableProc<InvFoodProduction>("[Mfg].[USP_WorkOrderSemiFinishForProductionOutput]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
