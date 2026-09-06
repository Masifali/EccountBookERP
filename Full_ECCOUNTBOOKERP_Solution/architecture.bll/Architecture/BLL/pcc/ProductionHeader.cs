using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.pcc;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;

namespace Architecture.BLL.pcc;

public class ProductionHeader
{
	public static VoucherHead MakeVoucher(Architecture.Model.pcc.ProductionHeader obj)
	{
		VoucherHead voucherHead = new VoucherHead();
		double voucheramt = 0.0;
		try
		{
			if (obj.WIPGLAccountID == 0)
			{
				throw new Exception("WIP AccountId Not Found");
			}
			List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
			List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (dtitem.Count > 0)
			{
				List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(obj.ItemId)).ToList();
				if (find2.Count > 0)
				{
					voucherHead.RefAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
					voucherHead.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
				}
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
			voucherHead.IsApproved = obj.IsApproved;
			voucherHead.voucherDetailList = new List<VoucherDetail>();
			int LineId = 1;
			LineId = 1;
			foreach (ProductionInputDetail item2 in obj.ProductionInputDetailList.ToList())
			{
				string Remarks = "";
				if (item2.ActionTypeId != 3)
				{
					if (dtitem.Count <= 0)
					{
						throw new Exception("Item GL Account Not Found");
					}
					List<GetItemGlIdsandItemName> find3 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
					if (find3.Count > 0)
					{
						Remarks = ((!(item2.Remarks == string.Empty)) ? (item2.Remarks + " ItemName: " + find3[0].ItemName + " Qty  " + item2.ItemQty + " Rate  " + item2.Rate) : ("ItemName: " + find3[0].ItemName + " Qty  " + item2.ItemQty + " Rate  " + item2.Rate));
						VoucherDetail vd6 = new VoucherDetail();
						vd6.LineId = LineId;
						vd6.AccountId = Conversion.ToInt(obj.WIPGLAccountID);
						vd6.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
						vd6.Comments = Conversion.ToString(Remarks);
						vd6.DebitAmount = Conversion.ToDouble(item2.Amount);
						vd6.QtyOut = Conversion.ToDouble(item2.ItemQty);
						vd6.WeightOut = Conversion.ToDouble(item2.NetWeight);
						vd6.ItemRate = Conversion.ToDouble(item2.Rate);
						vd6.ItemAmount = Conversion.ToDouble(item2.Amount);
						vd6.JobLotId = Conversion.ToInt(item2.JobLotId);
						voucherHead.voucherDetailList.Add(vd6);
						VoucherDetail vd3 = new VoucherDetail();
						vd3.LineId = LineId;
						vd3.AccountId = Conversion.ToInt(Conversion.ToInt(find3[0].PurchaseGLAC));
						vd3.AgainstAccountId = Conversion.ToInt(obj.WIPGLAccountID);
						vd3.Comments = Conversion.ToString(Remarks);
						vd3.CreditAmount = Conversion.ToDouble(item2.Amount);
						vd3.QtyOut = Conversion.ToDouble(item2.ItemQty);
						vd3.WeightOut = Conversion.ToDouble(item2.NetWeight);
						vd3.ItemRate = Conversion.ToDouble(item2.Rate);
						vd3.ItemAmount = Conversion.ToDouble(item2.Amount);
						vd3.JobLotId = Conversion.ToInt(item2.JobLotId);
						voucherHead.voucherDetailList.Add(vd3);
						voucheramt += vd3.CreditAmount;
						LineId++;
					}
				}
			}
			foreach (ProductionOverHeadsDetail item3 in obj.ProductionOverHeadsDetailList.ToList())
			{
				if (item3.OhRemarks == string.Empty)
				{
					item3.OhRemarks = "OverHeads ";
					if (item3.Qty > 0m)
					{
						item3.OhRemarks = item3.OhRemarks + "Qty " + item3.Qty;
					}
					if (item3.Rate > 0m)
					{
						item3.OhRemarks = item3.OhRemarks + " Rate " + item3.Rate;
					}
					if (item3.Amount > 0m)
					{
						item3.OhRemarks = item3.OhRemarks + " Amount " + item3.Amount;
					}
				}
				VoucherDetail vd2 = new VoucherDetail();
				vd2.AccountId = Conversion.ToInt(obj.WIPGLAccountID);
				vd2.AgainstAccountId = Conversion.ToInt(item3.OverHeadAccountId);
				vd2.Comments = Conversion.ToString(item3.OhRemarks);
				vd2.DebitAmount = Conversion.ToDouble(item3.Amount);
				vd2.QtyIn = Conversion.ToDouble(item3.Qty);
				vd2.WeightIn = Conversion.ToDouble(item3.Qty);
				vd2.ItemRate = Conversion.ToDouble(item3.Rate);
				vd2.ItemAmount = Conversion.ToDouble(item3.Amount);
				voucheramt += vd2.DebitAmount;
				voucherHead.voucherDetailList.Add(vd2);
				VoucherDetail vd5 = new VoucherDetail();
				vd5.AccountId = Conversion.ToInt(item3.OverHeadAccountId);
				vd5.AgainstAccountId = Conversion.ToInt(obj.WIPGLAccountID);
				vd5.Comments = Conversion.ToString(item3.OhRemarks);
				vd5.CreditAmount = Conversion.ToDouble(item3.Amount);
				vd5.QtyIn = Conversion.ToDouble(item3.Qty);
				vd5.WeightIn = Conversion.ToDouble(item3.Qty);
				vd5.ItemRate = Conversion.ToDouble(item3.Rate);
				vd5.ItemAmount = Conversion.ToDouble(item3.Amount);
				voucherHead.voucherDetailList.Add(vd5);
			}
			if (obj.ProductionWagesDetailList.Count > 0)
			{
				dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
				bool ContractorWagesDebitTotExpenseAcOnProductionConcrete = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ContractorWagesDebitTotExpenseAcOnProductionConcrete"));
				foreach (ProductionWagesDetail item in obj.ProductionWagesDetailList.ToList())
				{
					if (item.Remarks == string.Empty)
					{
						item.Remarks = "Wages ";
						if (item.Qty > 0m)
						{
							ProductionWagesDetail productionWagesDetail = item;
							productionWagesDetail.Remarks = productionWagesDetail.Remarks + "Qty " + item.Qty;
						}
						if (item.Rate > 0m)
						{
							ProductionWagesDetail productionWagesDetail2 = item;
							productionWagesDetail2.Remarks = productionWagesDetail2.Remarks + " Rate " + item.Rate;
						}
						if (item.Amount > 0m)
						{
							ProductionWagesDetail productionWagesDetail3 = item;
							productionWagesDetail3.Remarks = productionWagesDetail3.Remarks + " Amount " + item.Amount;
						}
					}
					VoucherDetail vd1 = new VoucherDetail();
					vd1.AccountId = (ContractorWagesDebitTotExpenseAcOnProductionConcrete ? obj.WagesExpenseAccountId : Conversion.ToInt(obj.WIPGLAccountID));
					List<SupplierCustomerGlAccountId> find = dtSupplier.Where((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(item.ContractorId)).ToList();
					if (find.Count > 0)
					{
						vd1.AgainstAccountId = Conversion.ToInt(find[0].GlAccountId);
						vd1.Comments = Conversion.ToString(item.Remarks);
						vd1.DebitAmount = Conversion.ToDouble(item.Amount);
						vd1.QtyIn = Conversion.ToDouble(item.Qty);
						vd1.WeightIn = Conversion.ToDouble(item.Qty);
						vd1.ItemRate = Conversion.ToDouble(item.Rate);
						vd1.ItemAmount = Conversion.ToDouble(item.Amount);
						voucheramt += vd1.DebitAmount;
						voucherHead.voucherDetailList.Add(vd1);
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(find[0].GlAccountId);
						vd4.AgainstAccountId = (ContractorWagesDebitTotExpenseAcOnProductionConcrete ? obj.WagesExpenseAccountId : Conversion.ToInt(obj.WIPGLAccountID));
						vd4.Comments = Conversion.ToString(item.Remarks);
						vd4.CreditAmount = Conversion.ToDouble(item.Amount);
						vd4.QtyIn = Conversion.ToDouble(item.Qty);
						vd4.WeightIn = Conversion.ToDouble(item.Qty);
						vd4.ItemRate = Conversion.ToDouble(item.Rate);
						vd4.ItemAmount = Conversion.ToDouble(item.Amount);
						vd4.SubsidiaryTypeId = 1;
						vd4.SubsidiaryAccountId = Conversion.ToInt(item.ContractorId);
						vd4.SupplierCustomerId = Conversion.ToInt(item.ContractorId);
						voucherHead.voucherDetailList.Add(vd4);
						continue;
					}
					throw new Exception("Contractor GlAccountId Not Found");
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

	public static int Save(Architecture.Model.pcc.ProductionHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				obj.ModifyUserId = 0;
				return new Architecture.DAL.pcc.ProductionHeader().SetData(obj, "[pcc].[USP_ProductionHeader_Insert]");
			}
			obj.ActionId = 2;
			obj.EntryUserId = 0;
			return new Architecture.DAL.pcc.ProductionHeader().SetData(obj, "[pcc].[USP_ProductionHeader_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.pcc.ProductionHeader ReadById(int Id)
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
			new List<Architecture.Model.pcc.ProductionHeader>();
			return new Architecture.DAL.pcc.ProductionHeader().GetData("[pcc].[USP_ProductionHeader_GetAllMethod]", p)[0];
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
			if (obj.Activity != string.Empty && obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[pcc].[USP_GetDataForDropDownFromProductionHeader]", p);
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
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
			if (obj.BOMHeaderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BOMHeaderId",
					Value = obj.BOMHeaderId
				});
			}
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WorkOrderId",
					Value = obj.JobOrderId
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ProductionHeader>("[pcc].[USP_ProductionHeader_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingProductionsForApproval(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
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
			if (obj.ProductionType != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductionTypeId",
					Value = obj.ProductionType
				});
			}
			if (obj.BOMHeaderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BomHeaderId",
					Value = obj.BOMHeaderId
				});
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductionNo",
					Value = obj.Id
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ProductionHeader>("pcc.USP_PendingProductionsForApproval", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.pcc.ProductionHeader obj)
	{
		DataTable dt = null;
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.pcc.ProductionHeader>("[pcc].[USP_ProductionHeader_GetAllMethod]", p);
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

	public static void DeleteByID(int EntryUserId, int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
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
			GenericProvider.GetDataTableProc<Architecture.Model.pcc.ProductionHeader>("[pcc].[USP_ProductionHeader_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Production_SlipAndRegister(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
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
			if (obj.BOMHeaderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BOMHeaderId",
					Value = obj.BOMHeaderId
				});
			}
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WorkOrderId",
					Value = obj.JobOrderId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ProductionHeader>("[pcc].[USP_Production_SlipAndRegister]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet Production_Report(ReportsParameters obj)
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
			if (obj.ProductionType != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductionTypeId",
					Value = obj.ProductionType
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			return GenericProvider.GetDataSetProc<Architecture.Model.pcc.ProductionHeader>("[pcc].[USP_Production_Report]", p);
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
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ProductionHeader>("[pcc].[USP_ProductionAllDetails_SubReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Production_ExFactoryReport(ReportsParameters obj)
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
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
				});
			}
			if (obj.ItemTypeOfTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeOfTypeId",
					Value = obj.ItemTypeOfTypeId
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
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
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
			if (obj.ProductinTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductionTypeId",
					Value = obj.ProductinTypeId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = obj.Activity
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("[pcc].[USP_Production_ExFactoryReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
