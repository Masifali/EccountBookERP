using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Production;
using Architecture.DAL.Steel.Production;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Production;
using Architecture.Model.Steel.Production;

namespace Architecture.BLL.Steel.Production;

public class InvFoodProduction
{
	public static int SetDataForSettlementFinancials(VoucherHead obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Production.InvFoodProduction().SetDataForSettlementFinancials(obj, "Sp_VoucherHead_Insert");
			}
			return new Architecture.DAL.Production.InvFoodProduction().SetDataForSettlementFinancials(obj, "Sp_VoucherHead_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAvailableTransactionsForIssuance_St(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateTo",
					Value = obj.ToDate
				});
			}
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategories",
					Value = obj.InventoryParentCategories
				});
			}
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReferenceDocumentTypeId",
					Value = obj.RefDocumentTypeId
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
			if (obj.PackingTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackingTypeId",
					Value = obj.PackingTypeId
				});
			}
			if (obj.stockUOM != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemUomId",
					Value = obj.stockUOM
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PurchaseAccountId",
					Value = obj.AccountId
				});
			}
			if (obj.WarehouseIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseIds",
					Value = obj.WarehouseIds
				});
			}
			if (obj.ItemIds != null && obj.ItemIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemIds",
					Value = obj.ItemIds
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[ST].[USP_GetAvailableTransactionsForIssuanceFromStockEvaluation]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static VoucherHead MakeVoucher(Architecture.Model.Steel.Production.InvFoodProduction obj)
	{
		VoucherHead voucherHead = new VoucherHead();
		double voucheramt = 0.0;
		try
		{
			List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
			dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
			voucherHead.DocumentTypeSrNo = obj.Id;
			voucherHead.VoucherDate = obj.DocDate;
			voucherHead.VoucherCode = obj.DocCode;
			voucherHead.DocumentTypeId = obj.DocumentTypeId;
			voucherHead.RefAccountId = obj.WIPAccountId;
			voucherHead.AgainstAccountId = obj.WIPItemId;
			voucherHead.EntryDate = DateTime.Now;
			voucherHead.ModifyDate = DateTime.Now;
			voucherHead.EntryUser = obj.EntryUserId;
			voucherHead.ModifyUser = obj.EntryUserId;
			voucherHead.OrganizationId = obj.OrganizationId;
			voucherHead.CompanyId = obj.CompanyId;
			voucherHead.FinancialYearId = obj.FinancialYearId;
			voucherHead.voucherDetailList = new List<VoucherDetail>();
			foreach (Architecture.Model.Steel.Production.InvFoodProductionDetail item in obj.invFoodProductionDetails.ToList())
			{
				string Remarks = "";
				if (item.EntryTypeId == 3 && item.ActionTypeId != 3)
				{
					if (dtitem.Count <= 0)
					{
						throw new Exception("Item GL Account Not Found");
					}
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find.Count <= 0)
					{
						throw new Exception("Item GL Account Not Found");
					}
					Remarks = ((!(item.Remarks == string.Empty)) ? (item.Remarks + "   ItemName: " + find[0].ItemName + "  Qty  " + item.Qty + "   Weight  " + item.Weight + "  Rate  " + item.Rate) : ("ItemName: " + find[0].ItemName + "  Qty  " + item.Qty + "   Weight  " + item.Weight + "  Rate  " + item.Rate));
					VoucherDetail vd3 = new VoucherDetail();
					vd3.AccountId = Conversion.ToInt(obj.WIPAccountId);
					vd3.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
					vd3.Comments = Conversion.ToString(Remarks);
					vd3.DebitAmount = Conversion.ToDouble(item.Amount);
					vd3.QtyIn = Conversion.ToDouble(item.Qty);
					vd3.WeightIn = Conversion.ToDouble(item.Weight);
					vd3.ItemAmount = Conversion.ToDouble(item.Amount);
					vd3.OrderNo = Conversion.ToInt(item.JobOrderInPutId);
					vd3.RefDocSubIdNo = Conversion.ToInt(item.Id);
					voucheramt += vd3.DebitAmount;
					voucherHead.voucherDetailList.Add(vd3);
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
					vd.AgainstAccountId = Conversion.ToInt(obj.WIPAccountId);
					vd.Comments = Conversion.ToString(Remarks);
					vd.CreditAmount = Conversion.ToDouble(item.Amount);
					vd.QtyOut = Conversion.ToDouble(item.Qty);
					vd.WeightOut = Conversion.ToDouble(item.Weight);
					vd.ItemId = Conversion.ToInt(item.ItemId);
					vd.ItemAmount = Conversion.ToDouble(item.Amount);
					vd.OrderNo = Conversion.ToInt(item.JobOrderInPutId);
					vd.RefDocSubIdNo = Conversion.ToInt(item.Id);
					voucherHead.voucherDetailList.Add(vd);
				}
				else if (item.EntryTypeId != 3 && item.ActionTypeId != 3)
				{
					if (dtitem.Count <= 0)
					{
						throw new Exception("Item GL Account Not Found");
					}
					List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find2.Count <= 0)
					{
						throw new Exception("Item GL Account Not Found");
					}
					Remarks = ((!(item.Remarks == string.Empty)) ? (item.Remarks + "   ItemName: " + find2[0].ItemName + "  Qty  " + item.Qty + "   Weight  " + item.Weight + "  Rate  " + item.Rate) : ("ItemName: " + find2[0].ItemName + "  Qty  " + item.Qty + "   Weight  " + item.Weight + "  Rate  " + item.Rate));
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
					vd2.AgainstAccountId = Conversion.ToInt(obj.WIPAccountId);
					vd2.Comments = Conversion.ToString(Remarks);
					vd2.DebitAmount = Conversion.ToDouble(item.Amount);
					vd2.QtyIn = Conversion.ToDouble(item.Qty);
					vd2.WeightIn = Conversion.ToDouble(item.Weight);
					vd2.ItemAmount = Conversion.ToDouble(item.Amount);
					vd2.ItemId = Conversion.ToInt(item.ItemId);
					vd2.OrderNo = Conversion.ToInt(item.JobOrderOutPutId);
					vd2.RefDocSubIdNo = Conversion.ToInt(item.Id);
					voucherHead.voucherDetailList.Add(vd2);
					VoucherDetail vd4 = new VoucherDetail();
					vd4.AccountId = Conversion.ToInt(obj.WIPAccountId);
					vd4.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
					vd4.Comments = Conversion.ToString(Remarks);
					vd4.CreditAmount = Conversion.ToDouble(item.Amount);
					vd4.QtyIn = Conversion.ToDouble(item.Qty);
					vd4.WeightIn = Conversion.ToDouble(item.Weight);
					vd4.ItemAmount = Conversion.ToDouble(item.Amount);
					vd4.OrderNo = Conversion.ToInt(item.JobOrderOutPutId);
					vd4.RefDocSubIdNo = Conversion.ToInt(item.Id);
					voucheramt += vd4.DebitAmount;
					voucherHead.voucherDetailList.Add(vd4);
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

	public static int Save(Architecture.Model.Steel.Production.InvFoodProduction obj)
	{
		try
		{
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				obj.ModifyUserId = 0;
				return new Architecture.DAL.Steel.Production.InvFoodProduction().SetData(obj, "[ST].[USP_InvFoodProduction_Insert]");
			}
			obj.ActionId = 2;
			obj.EntryUserId = 0;
			return new Architecture.DAL.Steel.Production.InvFoodProduction().SetData(obj, "[ST].[USP_InvFoodProduction_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Steel.Production.InvFoodProduction GetByID(int Id)
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
			new List<Architecture.Model.Steel.Production.InvFoodProduction>();
			return new Architecture.DAL.Steel.Production.InvFoodProduction().GetDate("[ST].[USP_FoodProduction_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Steel.Production.InvFoodProduction obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Steel.Production.InvFoodProduction>("[ST].[USP_FoodProduction_GetAllMethod]", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["DocCode"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCodeOverHeaad(Architecture.Model.Steel.Production.InvFoodProduction obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Steel.Production.InvFoodProduction>("[ST].[USP_FoodProductionOverHeads_GetAllMethod]", p);
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
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
					ParameterName = "@EntryUserId",
					Value = obj.EntryUser
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[ST].[USP_FoodProduction_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetJobOrderNoForInvFoodProduction(ReportsParameters obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetJobOrderNoForFoodProduction"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Steel.Production.InvFoodProduction>("[ST].[USP_FoodProduction_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInPutTotalQtyandWeightByJobOrderId(Architecture.Model.Production.InvFoodProduction obj)
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
				ParameterName = "@ProductionJobOrderId",
				Value = obj.InvJobOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetInPutTotalQtyandWeightByJobOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[ST].[USP_FoodProduction_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvFoodProductionRecoverySummeryReport(ReportsParameters obj)
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
				ParameterName = "@ProductionJobOrderId",
				Value = obj.JobOrderId
			});
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[ST].[USP_FoodProduction_Summery_WithValues]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSummeryValues(ReportsParameters obj)
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
				ParameterName = "@ProductionJobOrderId",
				Value = obj.JobOrderId
			});
			if (obj.NetWeight != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FgNetWeight",
					Value = obj.NetWeight
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[ST].[USP_FoodProduction_GetSummeryValues]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int UpdateSettlement(Architecture.Model.Steel.Production.InvFoodProduction obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Steel.Production.InvFoodProduction().SetDataForUpdate(obj, "[ST].[USP_InvFoodProductionDetail_Update]");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
