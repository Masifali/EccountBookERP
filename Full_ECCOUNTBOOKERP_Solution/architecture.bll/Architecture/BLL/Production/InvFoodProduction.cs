using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Production;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Production;

namespace Architecture.BLL.Production;

public class InvFoodProduction
{
	public static VoucherHead MakeVoucher(Architecture.Model.Production.InvFoodProduction obj)
	{
		VoucherHead voucherHead = new VoucherHead();
		double voucheramt = 0.0;
		try
		{
			List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			voucherHead.DocumentTypeSrNo = obj.Id;
			voucherHead.VoucherDate = obj.DocDate;
			voucherHead.VoucherCode = obj.DocCode;
			voucherHead.DocumentTypeId = obj.DocumentTypeId;
			voucherHead.BaseDocumentTypeId = obj.BaseDocumentTypeId;
			voucherHead.RefAccountId = obj.WIPAccountId;
			voucherHead.AgainstAccountId = obj.WIPItemId;
			voucherHead.EntryDate = DateTime.Now;
			voucherHead.ModifyDate = DateTime.Now;
			voucherHead.EntryUser = obj.EntryUser;
			voucherHead.ModifyUser = obj.ModifyUser;
			voucherHead.OrganizationId = obj.OrganizationId;
			voucherHead.CompanyId = obj.CompanyId;
			voucherHead.FinancialYearId = obj.FinancialYearId;
			voucherHead.BranchId = obj.BranchId;
			voucherHead.voucherDetailList = new List<VoucherDetail>();
			bool FIFOCgs = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5);
			int LineId = 1;
			foreach (Architecture.Model.Production.InvFoodProductionDetail item2 in obj.invFoodProductionDetails.ToList())
			{
				string Remarks2 = "";
				if ((obj.DocumentTypeId == 80 && !FIFOCgs) || (obj.DocumentTypeId == 80 && item2.RefDocumentTypeId > 0 && item2.RefDocNoId > 0))
				{
					if (dtitem.Count <= 0)
					{
						throw new Exception("Item GL Account Not Found");
					}
					List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
					if (find2.Count > 0)
					{
						Remarks2 = obj.InvJobOrderNo + " " + (string.IsNullOrWhiteSpace(item2.Remarks) ? "" : (item2.Remarks + " ")) + " ItemName: " + find2[0].ItemName + " Qty " + item2.Qty + " Weight " + item2.Weight + " Rate " + item2.Rate;
						VoucherDetail vd5 = new VoucherDetail();
						vd5.LineId = LineId;
						vd5.AccountId = Conversion.ToInt(obj.WIPAccountId);
						vd5.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd5.Comments = Conversion.ToString(Remarks2);
						vd5.DebitAmount = Conversion.ToDouble(item2.Amount);
						vd5.QtyIn = Conversion.ToDouble(item2.Qty);
						vd5.WeightIn = Conversion.ToDouble(item2.Weight);
						vd5.ItemAmount = Conversion.ToDouble(item2.Amount);
						vd5.OrderNo = Conversion.ToInt(item2.InvProductionJobOrderId);
						vd5.BranchesId = obj.BranchId;
						voucheramt += vd5.DebitAmount;
						voucherHead.voucherDetailList.Add(vd5);
						VoucherDetail vd2 = new VoucherDetail();
						vd2.LineId = LineId;
						vd2.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd2.AgainstAccountId = Conversion.ToInt(obj.WIPAccountId);
						vd2.Comments = Conversion.ToString(Remarks2);
						vd2.CreditAmount = Conversion.ToDouble(item2.Amount);
						vd2.QtyOut = Conversion.ToDouble(item2.Qty);
						vd2.WeightOut = Conversion.ToDouble(item2.Weight);
						vd2.ItemId = Conversion.ToInt(item2.ItemId);
						vd2.ItemAmount = Conversion.ToDouble(item2.Amount);
						vd2.OrderNo = Conversion.ToInt(item2.InvProductionJobOrderId);
						vd2.BranchesId = obj.BranchId;
						voucherHead.voucherDetailList.Add(vd2);
						LineId++;
					}
				}
				else
				{
					if (obj.DocumentTypeId != 112)
					{
						continue;
					}
					if (dtitem.Count <= 0)
					{
						throw new Exception("Item GL Account Not Found");
					}
					List<GetItemGlIdsandItemName> find3 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
					if (find3.Count > 0)
					{
						Remarks2 = obj.InvJobOrderNo + " " + (string.IsNullOrWhiteSpace(item2.Remarks) ? "" : (item2.Remarks + " ")) + " ItemName: " + find3[0].ItemName + " Qty " + item2.Qty + " Weight " + item2.Weight + " Rate " + item2.Rate;
						VoucherDetail vd3 = new VoucherDetail();
						if (item2.StockAcId > 0)
						{
							vd3.AccountId = Conversion.ToInt(item2.StockAcId);
						}
						else
						{
							vd3.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
						}
						vd3.LineId = LineId;
						vd3.AgainstAccountId = Conversion.ToInt(obj.WIPAccountId);
						vd3.Comments = Conversion.ToString(Remarks2);
						vd3.DebitAmount = Conversion.ToDouble(item2.Amount);
						vd3.QtyIn = Conversion.ToDouble(item2.Qty);
						vd3.WeightIn = Conversion.ToDouble(item2.Weight);
						vd3.ItemAmount = Conversion.ToDouble(item2.Amount);
						vd3.ItemId = Conversion.ToInt(item2.ItemId);
						vd3.OrderNo = Conversion.ToInt(item2.InvProductionJobOrderId);
						vd3.BranchesId = obj.BranchId;
						voucherHead.voucherDetailList.Add(vd3);
						VoucherDetail vd6 = new VoucherDetail();
						vd6.AccountId = Conversion.ToInt(obj.WIPAccountId);
						if (item2.StockAcId > 0)
						{
							vd6.AgainstAccountId = Conversion.ToInt(item2.StockAcId);
						}
						else
						{
							vd6.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
						}
						vd6.LineId = LineId;
						vd6.Comments = Conversion.ToString(Remarks2);
						vd6.CreditAmount = Conversion.ToDouble(item2.Amount);
						vd6.QtyIn = Conversion.ToDouble(item2.Qty);
						vd6.WeightIn = Conversion.ToDouble(item2.Weight);
						vd6.ItemAmount = Conversion.ToDouble(item2.Amount);
						vd6.OrderNo = Conversion.ToInt(item2.InvProductionJobOrderId);
						vd6.BranchesId = obj.BranchId;
						voucheramt += vd6.DebitAmount;
						voucherHead.voucherDetailList.Add(vd6);
						LineId++;
					}
				}
			}
			foreach (InvFoodProductionConsumptionDetail item in obj.InvFoodProductionConsumptionDetailslist.ToList())
			{
				string Remarks = "";
				if ((obj.DocumentTypeId == 181 && !FIFOCgs) || (obj.DocumentTypeId == 181 && item.RefDocumentTypeId > 0 && item.RefDocNoId > 0))
				{
					if (dtitem.Count <= 0)
					{
						throw new Exception("Item GL Account Not Found");
					}
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find.Count > 0)
					{
						Remarks = obj.InvJobOrderNo + " " + (string.IsNullOrWhiteSpace(item.Remarks) ? "" : (item.Remarks + " ")) + " ItemName: " + find[0].ItemName + " Qty " + item.Qty + " Weight " + item.Weight + " Rate " + item.Rate;
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(obj.WIPAccountId);
						vd4.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd4.Comments = Conversion.ToString(Remarks);
						vd4.DebitAmount = Conversion.ToDouble(item.Amount);
						vd4.QtyIn = Conversion.ToDouble(item.Qty);
						vd4.WeightIn = Conversion.ToDouble(item.Weight);
						vd4.ItemAmount = Conversion.ToDouble(item.Amount);
						vd4.OrderNo = Conversion.ToInt(item.InvProductionJobOrderId);
						vd4.BranchesId = obj.BranchId;
						voucheramt += vd4.DebitAmount;
						voucherHead.voucherDetailList.Add(vd4);
						VoucherDetail vd = new VoucherDetail();
						vd.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd.AgainstAccountId = Conversion.ToInt(obj.WIPAccountId);
						vd.Comments = Conversion.ToString(Remarks);
						vd.CreditAmount = Conversion.ToDouble(item.Amount);
						vd.QtyOut = Conversion.ToDouble(item.Qty);
						vd.WeightOut = Conversion.ToDouble(item.Weight);
						vd.ItemId = Conversion.ToInt(item.ItemId);
						vd.ItemAmount = Conversion.ToDouble(item.Amount);
						vd.OrderNo = Conversion.ToInt(item.InvProductionJobOrderId);
						vd.BranchesId = obj.BranchId;
						voucherHead.voucherDetailList.Add(vd);
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

	public static int Save(Architecture.Model.Production.InvFoodProduction obj)
	{
		try
		{
			bool jobOrderCreateWithoutRates = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "JobOrderCreatewithoutRates"));
			if (Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "InventoryFinancialsEffectsInActive")) || (jobOrderCreateWithoutRates && obj.DocumentTypeId == 112))
			{
				obj.RefDocumentTypeId = 1;
			}
			if ((obj.DocumentTypeId == 80 || obj.DocumentTypeId == 112) && (obj.invFoodProductionDetails == null || obj.invFoodProductionDetails.Count == 0))
			{
				throw new Exception("invFoodProductionDetails List Not Found.");
			}
			if (obj.DocumentTypeId == 181 && (obj.InvFoodProductionConsumptionDetailslist == null || obj.InvFoodProductionConsumptionDetailslist.Count == 0))
			{
				throw new Exception("InvFoodProductionConsumptionDetailslist List Not Found.");
			}
			if (obj.RefDocumentTypeId != 1)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
			}
			bool isNew = obj.Id == 0;
			obj.ActionId = (isNew ? 1 : 2);
			if (isNew)
			{
				obj.ModifyUser = 0;
				return new Architecture.DAL.Production.InvFoodProduction().SetData(obj, "Sp_InvFoodProduction_Insert");
			}
			obj.EntryUser = 0;
			return new Architecture.DAL.Production.InvFoodProduction().SetData(obj, "Sp_InvFoodProduction_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int UpdateSettlement(Architecture.Model.Production.InvFoodProduction obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Production.InvFoodProduction().SetDataForUpdate(obj, "SP_InvFoodProductionDetail_Update");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int InPutUpdateSettlement(Architecture.Model.Production.InvFoodProduction obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Production.InvFoodProduction().SetDataForINPutUpdate(obj, "SP_InvFoodProductionDetail_InPut_Update");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

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

	public static Architecture.Model.Production.InvFoodProduction GetByID(int Id)
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
				Value = "GetById"
			});
			new List<Architecture.Model.Production.InvFoodProduction>();
			return new Architecture.DAL.Production.InvFoodProduction().GetDate("Sp_InvFoodProduction_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetSerialNumber(Architecture.Model.Production.InvFoodProduction obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.BranchId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadSerialNumber"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("Sp_InvFoodProduction_GetAllMethod", p);
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

	public static int InvFoodProduction_GetJobOrderIdByProduction(int OrganizationId, int CompanyId, int DocumentTypeId, int Id)
	{
		DataTable dt = null;
		try
		{
			int Code = 0;
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
				ParameterName = "@Id",
				Value = Id
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[dbo].[USP_InvFoodProduction_GetJobOrderIdByProduction]", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["JobOrderDocumentTypeId"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Production.InvFoodProduction obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.BranchId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("Sp_InvFoodProduction_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ProductionFormHistory(ReportsParameters obj)
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
					ParameterName = "@DocNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvJobOrderId",
					Value = obj.JobOrderId
				});
			}
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
				});
			}
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.RefDocumentTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("Sp_InvFoodProduction_GetAllMethod", p);
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
				ParameterName = "@JobOrderId",
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
			if (obj.NetWeight != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FgNetWeight",
					Value = obj.NetWeight
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
			if (obj.BranchesIds != string.Empty || obj.BranchesIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("Sp_InvFoodProduction_GetSummeryValues", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FoodProductionComparisons_Rpt(ReportsParameters obj)
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
				ParameterName = "@ProductionType",
				Value = obj.ReportType
			});
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.ItemId
				});
			}
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobOrderId",
					Value = obj.JobOrderId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("SpInvFoodProductionComparisons_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInPutTotalQtyandWeightByJobOrderId(ReportsParameters obj)
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
				ParameterName = "@InvJobOrderId",
				Value = obj.JobOrderId
			});
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
				});
			}
			if (obj.ItemUomId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemUomId",
					Value = obj.ItemUomId
				});
			}
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryType",
					Value = obj.Activity
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetInPutTotalQtyandWeightByJobOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("Sp_InvFoodProduction_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBrandItemsForPackingAndOverHeads(Architecture.Model.Production.InvFoodProduction obj)
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
				ParameterName = "@InvJobOrderId",
				Value = obj.InvJobOrderId
			});
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
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
				ParameterName = "@Activity",
				Value = "GetBrandItemsForPackingAndOverHeads"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("Sp_InvFoodProduction_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemUomByItemIdFromFinishGoods(Architecture.Model.Production.InvFoodProduction obj)
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
				ParameterName = "@InvJobOrderId",
				Value = obj.InvJobOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.WIPItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetItemUomByItemIdFromFinishGoods"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("Sp_InvFoodProduction_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetProductionOutPutDataForRatesInput(ReportsParameters obj)
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
				ParameterName = "@BranchId",
				Value = obj.BranchesId
			});
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvProductionJobOrderId",
					Value = obj.JobOrderId
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
			if (obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryType ",
					Value = obj.ReportType
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("USP_GetProductionOutPutDataForRatesInput", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable USP_GetOutPutBalQtyFromPMQty(ReportsParameters obj)
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
				ParameterName = "@InvProductionJobOrderId",
				Value = obj.JobOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BrandId",
				Value = obj.BrandId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemUomId",
				Value = obj.ItemUomId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[dbo].[USP_GetOutPutBalQtyFromPMQty]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetOutPutQtyWeightFoodProductionForPmOh(Architecture.Model.Production.InvFoodProductionPackingMaterial obj)
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
				ParameterName = "@InvJobOrderId",
				Value = obj.InvProductionJobOrderId
			});
			if (obj.BrandId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.BrandId
				});
			}
			if (obj.BrandUomId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemUomId",
					Value = obj.BrandUomId
				});
			}
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[dbo].[USP-GetOutPutQtyWeightFoodProductionForPmOh]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBrandItemsAndUomForPackMaterialAgainstPmItem(ReportsParameters obj)
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
				ParameterName = "@InvProductionJobOrderId",
				Value = obj.JobOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PMDocId",
					Value = obj.Id
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("USP_GetBrandItemsForPackMaterialAgainstPmItem", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvFoodProduction_GetBrandItemsByJobOrder(ReportsParameters obj)
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
				ParameterName = "@JobOrderId",
				Value = obj.JobOrderId
			});
			if (!string.IsNullOrEmpty(obj.DocumentTypeIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (!string.IsNullOrEmpty(obj.EntryType))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryType",
					Value = obj.EntryType
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[dbo].[USP_InvFoodProduction_GetBrandItemsByJobOrder]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DropDownForConsumptionForm(int OrganizationId, int CompanyId, string Activity = null, string BranchesIds = null)
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
				ParameterName = "@ActivityType",
				Value = Activity
			});
			if (BranchesIds != string.Empty || BranchesIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[dbo].[USP_DropDownForConsumptionForm]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetProductionConsumptionDataForPendingRates(ReportsParameters obj)
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
				ParameterName = "@BranchId",
				Value = obj.BranchesId
			});
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvProductionJobOrderId",
					Value = obj.JobOrderId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[dbo].[USP_GetProductionConsumptionDataForPendingRates]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBranchsAllocatedToUserFromProduction(int OrganizationId, int CompanyId, int UserId, int DocumentTypeId = 0, string DocumentTypeIds = "")
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
			if (!string.IsNullOrEmpty(DocumentTypeIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = DocumentTypeIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.BranchesAllocationToUser>("[dbo].[USP_GetBranchsAllocatedToUserFromProduction]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromFoodProduction(ReportsParameters obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (!string.IsNullOrEmpty(obj.Activity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			if (!string.IsNullOrEmpty(obj.DocumentTypeIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			if (obj.ActionId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("USP_GetDataForDropDownFromFoodProduction", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataFromProductionForStockConversion(ReportsParameters obj)
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
			if (obj.DocumentTypeIds != string.Empty || obj.DocumentTypeIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.BranchesIds != string.Empty || obj.BranchesIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("USP_GetDataFromProductionForStockConversion", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromFoodProductionPM(ReportsParameters obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.Activity != string.Empty || obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			if (obj.DocumentTypeIds != string.Empty || obj.DocumentTypeIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.BranchesIds != string.Empty || obj.BranchesIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("USP_GetDataForDropDownFromFoodProductionPM", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getJobOrderFromProduction(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("usp_getJobOrderFromProduction", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ProductionRegister(ReportsParameters obj)
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
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvJobOrderId",
					Value = obj.JobOrderId
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
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
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
			if (obj.StockAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemStockAccountId",
					Value = obj.StockAccountId
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
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCategoriesId",
					Value = obj.ParentCategoryId
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WipAccountId",
					Value = obj.AccountId
				});
			}
			if (obj.ReportType != null && obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryType ",
					Value = obj.ReportType
				});
			}
			if (obj.EntryType != null && obj.EntryType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryTypeDetail ",
					Value = obj.EntryType
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = obj.Activity
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("USP_ProductionRegisterWithActivity", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int JobOrderIdByOverHeadId(int OverHeadId)
	{
		int Id = 0;
		try
		{
			DataTable dt = null;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = OverHeadId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "JobOrderIdByOverHeadId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("Sp_InvFoodProductionOverHeads_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Id = Conversion.ToInt(dt.Rows[0][0]);
			}
			return Id;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetProductionInPutDataByJobAndPlantId(int OrganizationId, int CompanyId, int DocumentTypeId, int JobOrderId, int PlantId, string Activity = null)
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
				ParameterName = "@JobOrderId",
				Value = JobOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PlantId",
				Value = PlantId
			});
			if (Activity != null && Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("usp_GetProductionInPutDataByJobAndPlantId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ProductionSettlementWeightValidations(int OrganizationId, int CompanyId, int JobOrderId)
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
				ParameterName = "@JobOrderId",
				Value = JobOrderId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("usp_ProductionSettlementWeightValidations", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable JobOrderSummary(ReportsParameters obj)
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[dbo].[usp_JobOrderSummary_Report]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getProductionDataForApprovalOrUnApproval(ReportsParameters obj)
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
				ParameterName = "@IsApproved",
				Value = obj.IsApproved
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
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobOrderId",
					Value = obj.JobOrderId
				});
			}
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("usp_getProductionDataForApprovalOrUnApproval", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateStatusandIsApprovedbyId(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = item.OrganizationId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = item.CompanyId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ReqType",
					Value = item.ReqType
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = item.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@DetailId",
					Value = item.DetailId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedUserId",
					Value = item.EntryUser
				});
				if (item.Comments != null && item.Comments != string.Empty)
				{
					p.Add(new SqlParameter
					{
						ParameterName = "@StatusRemarks",
						Value = item.Comments
					});
				}
				GenericProvider.GetDataTableProc<SaleOrder>("[dbo].[usp_ProductionApprovedOrUnApprovedStatusUpdatebyDetailId]", p);
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable FoodProduction_JobOrderWithFinishOutPutPrcntObtained(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[dbo].[USP_InvFoodProduction_JobOrderWithFinishOutPutPrcntObtained]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FoodProduction_InputSummaryByJobOrder(int OrganizationId, int CompanyId, int BranchesId, int FinancialYearId, int JobOrderId)
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
				Value = BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@JobOrderId",
				Value = JobOrderId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[dbo].[USP_InvFoodProduction_InputSummaryByJobOrder]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FoodProduction_ByProductSummaryByJobOrder(int OrganizationId, int CompanyId, int BranchesId, int FinancialYearId, int JobOrderId)
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
				Value = BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@JobOrderId",
				Value = JobOrderId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[dbo].[USP_InvFoodProduction_ByProductSummaryByJobOrder]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getProductionByProductDataByJobOrderId(int CompanyId, int JobOrderId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@JobOrderId",
				Value = JobOrderId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[dbo].[usp_getProductionByProductDataByJobOrderId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ProductionByProductRateUpdateByJobOrderId(ReportsParameters obj)
	{
		try
		{
			for (int i = 0; i < obj.ReportsParameterslist.Count; i++)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = obj.ReportsParameterslist[i].CompanyId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@JobOrderId",
					Value = obj.ReportsParameterslist[i].JobOrderId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ReportsParameterslist[i].ItemId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@RatePerKg",
					Value = obj.ReportsParameterslist[i].ItemRate
				});
				GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProduction>("[dbo].[usp_ProductionByProductRateUpdateByJobOrderId]", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
