using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.FeedMill.Production;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.FeedMill.Production;
using Architecture.Model.Inventory;

namespace Architecture.BLL.FeedMill.Production;

public class ProductionHeader
{
	public static VoucherHead MakeVoucher(Architecture.Model.FeedMill.Production.ProductionHeader obj)
	{
		VoucherHead voucherHead = new VoucherHead();
		double voucheramt = 0.0;
		try
		{
			List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
			List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
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
			int TollManufacturingAccountId = 0;
			if (obj.DocumentTypeId == 1125)
			{
				dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
				if (dtSupplier.Count <= 0)
				{
					throw new Exception("Party List not found...");
				}
				List<SupplierCustomerGlAccountId> find3 = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(obj.SupplierCustomerId)).ToList();
				if (find3.Count <= 0)
				{
					throw new Exception("Party GL AccountId not Found");
				}
				voucherHead.RefAccountId = Conversion.ToInt(find3[0].GlAccountId);
				TollManufacturingAccountId = Conversion.ToInt(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "TollManufacturingIncomeAccount"));
				if (TollManufacturingAccountId == 0)
				{
					throw new Exception("Toll Manufacturing Income account not configured properly in configuration.");
				}
			}
			int LineId = 1;
			foreach (ProductionOutPutDetail item3 in obj.ProductionOutPutDetailList.ToList())
			{
				string Remarks3 = "";
				if (item3.ActionTypeId == 3)
				{
					continue;
				}
				if (obj.DocumentTypeId == 1125)
				{
					Remarks3 = ((!(item3.Remarks == string.Empty)) ? (" Qty  " + Conversion.ToDouble(item3.ItemQty) + " Rate  " + Conversion.ToDouble(item3.Rate)) : (" Qty  " + Conversion.ToDouble(item3.ItemQty) + " Rate  " + Conversion.ToDouble(item3.Rate)));
					VoucherDetail vd3 = new VoucherDetail();
					vd3.LineId = LineId;
					vd3.AccountId = voucherHead.RefAccountId;
					vd3.AgainstAccountId = TollManufacturingAccountId;
					vd3.Comments = Conversion.ToString(Remarks3);
					vd3.DebitAmount = Conversion.ToDouble(item3.Amount);
					vd3.ItemRate = Conversion.ToDouble(item3.Rate);
					vd3.ItemAmount = Conversion.ToDouble(item3.Amount);
					voucherHead.voucherDetailList.Add(vd3);
					voucheramt += vd3.DebitAmount;
					VoucherDetail vd6 = new VoucherDetail();
					vd6.LineId = LineId;
					vd6.AccountId = TollManufacturingAccountId;
					vd6.AgainstAccountId = voucherHead.RefAccountId;
					vd6.Comments = Conversion.ToString(Remarks3);
					vd6.CreditAmount = Conversion.ToDouble(item3.Amount);
					vd6.ItemRate = Conversion.ToDouble(item3.Rate);
					vd6.ItemAmount = Conversion.ToDouble(item3.Amount);
					voucherHead.voucherDetailList.Add(vd6);
					continue;
				}
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find4 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item3.ItemId)).ToList();
					if (find4.Count > 0)
					{
						Remarks3 = ((!(item3.Remarks == string.Empty)) ? (item3.Remarks + " ItemName: " + find4[0].ItemName + " Qty  " + Conversion.ToDouble(item3.NetWeight) + " Rate  " + Conversion.ToDouble(item3.Rate)) : ("ItemName: " + find4[0].ItemName + " Qty  " + Conversion.ToDouble(item3.NetWeight) + " Rate  " + Conversion.ToDouble(item3.Rate)));
						VoucherDetail vd4 = new VoucherDetail();
						vd4.LineId = LineId;
						vd4.AccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
						vd4.AgainstAccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
						vd4.Comments = Conversion.ToString(Remarks3);
						vd4.DebitAmount = Conversion.ToDouble(item3.Amount);
						vd4.QtyIn = Conversion.ToDouble(item3.ItemQty);
						vd4.WeightIn = Conversion.ToDouble(item3.NetWeight);
						vd4.ItemRate = Conversion.ToDouble(item3.Rate);
						vd4.ItemAmount = Conversion.ToDouble(item3.Amount);
						voucherHead.voucherDetailList.Add(vd4);
						voucheramt += vd4.DebitAmount;
						LineId++;
					}
					continue;
				}
				throw new Exception("Item GL Account Not Found");
			}
			if (obj.DocumentTypeId != 1125)
			{
				LineId = 1000;
				foreach (ProductionInputDetail item2 in obj.ProductionInputDetailList.ToList())
				{
					string Remarks2 = "";
					if (item2.ActionTypeId != 3)
					{
						if (dtitem.Count <= 0)
						{
							throw new Exception("Item GL Account Not Found");
						}
						List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
						if (find2.Count > 0)
						{
							Remarks2 = ((!(item2.Remarks == string.Empty)) ? (item2.Remarks + " ItemName: " + find2[0].ItemName + " Qty  " + Conversion.ToDouble(item2.NetWeight) + " Rate  " + Conversion.ToDouble(item2.Rate)) : ("ItemName: " + find2[0].ItemName + " Qty  " + Conversion.ToDouble(item2.NetWeight) + " Rate  " + Conversion.ToDouble(item2.Rate)));
							VoucherDetail vd2 = new VoucherDetail();
							vd2.LineId = LineId;
							vd2.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
							vd2.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
							vd2.Comments = Conversion.ToString(Remarks2);
							vd2.CreditAmount = Conversion.ToDouble(item2.Amount);
							vd2.QtyOut = Conversion.ToDouble(item2.ItemQty);
							vd2.WeightOut = Conversion.ToDouble(item2.NetWeight);
							vd2.ItemRate = Conversion.ToDouble(item2.Rate);
							vd2.ItemAmount = Conversion.ToDouble(item2.Amount);
							voucherHead.voucherDetailList.Add(vd2);
							voucheramt += vd2.CreditAmount;
							LineId++;
						}
					}
				}
				foreach (ProductionOverHeadsDetail item4 in obj.ProductionOverHeadsDetailList.ToList())
				{
					if (item4.OhRemarks == string.Empty)
					{
						item4.OhRemarks = "OverHeads ";
						if (item4.Qty > 0m)
						{
							item4.OhRemarks = item4.OhRemarks + "Qty " + item4.Qty;
						}
						if (item4.Rate > 0m)
						{
							item4.OhRemarks = item4.OhRemarks + " Rate " + Conversion.ToDouble(item4.Rate);
						}
						if (item4.Amount > 0m)
						{
							item4.OhRemarks = item4.OhRemarks + " Amount " + Conversion.ToDouble(item4.Amount);
						}
					}
					VoucherDetail vd5 = new VoucherDetail();
					vd5.AccountId = Conversion.ToInt(item4.OverHeadAccountId);
					vd5.AgainstAccountId = Conversion.ToInt(item4.OverHeadAccountId);
					vd5.Comments = Conversion.ToString(item4.OhRemarks);
					vd5.CreditAmount = Conversion.ToDouble(item4.Amount);
					vd5.QtyOut = Conversion.ToDouble(item4.Qty);
					vd5.WeightOut = Conversion.ToDouble(item4.Qty);
					vd5.ItemRate = Conversion.ToDouble(item4.Rate);
					vd5.ItemAmount = Conversion.ToDouble(item4.Amount);
					voucherHead.voucherDetailList.Add(vd5);
				}
				LineId = 2000;
				foreach (ProductionConsumptionDetail item in obj.ProductionConsumptionDetailList.ToList())
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
							Remarks = ((!(item.Remarks == string.Empty)) ? (item.Remarks + " ItemName: " + find[0].ItemName + " Qty  " + Conversion.ToDouble(item.ItemQty) + " Rate  " + Conversion.ToDouble(item.Rate)) : ("ItemName: " + find[0].ItemName + " Qty  " + Conversion.ToDouble(item.ItemQty) + " Rate  " + Conversion.ToDouble(item.Rate)));
							VoucherDetail vd1 = new VoucherDetail();
							vd1.LineId = LineId;
							vd1.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd1.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd1.Comments = Conversion.ToString(Remarks);
							vd1.CreditAmount = Conversion.ToDouble(item.Amount);
							vd1.QtyOut = Conversion.ToDouble(item.ItemQty);
							vd1.WeightOut = Conversion.ToDouble(item.NetWeight);
							vd1.ItemRate = Conversion.ToDouble(item.Rate);
							vd1.ItemAmount = Conversion.ToDouble(item.Amount);
							voucherHead.voucherDetailList.Add(vd1);
							voucheramt += vd1.CreditAmount;
							LineId++;
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

	public static int Save(Architecture.Model.FeedMill.Production.ProductionHeader obj)
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
				return new Architecture.DAL.FeedMill.Production.ProductionHeader().SetData(obj, "[fed].[USP_ProductionHeader_Insert]");
			}
			obj.ActionId = 2;
			obj.EntryUserId = 0;
			return new Architecture.DAL.FeedMill.Production.ProductionHeader().SetData(obj, "[fed].[USP_ProductionHeader_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.FeedMill.Production.ProductionHeader ReadById(int Id)
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
			new List<Architecture.Model.FeedMill.Production.ProductionHeader>();
			return new Architecture.DAL.FeedMill.Production.ProductionHeader().GetData("[fed].[USP_ProductionHeader_GetAllMethod]", p)[0];
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
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
			if (obj.Activity != string.Empty && obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[fed].[USP_GetDataForDropDownFromProductionHeader]", p);
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
			if (obj.ProductionType != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductTypeId",
					Value = obj.ProductionType
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Production.ProductionHeader>("[fed].[USP_ProductionHeader_GetAllMethod]", p);
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
			if (obj.ProductionType != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductionTypeId",
					Value = obj.ProductionType
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
				ParameterName = "@IsApproved",
				Value = obj.IsApproved
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Production.ProductionHeader>("fed.USP_PendingProductionsForApproval", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.FeedMill.Production.ProductionHeader obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Production.ProductionHeader>("[fed].[USP_ProductionHeader_GetAllMethod]", p);
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
			GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Production.ProductionHeader>("[fed].[USP_ProductionHeader_GetAllMethod]", p);
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
			if (obj.ProductionType != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductTypeId",
					Value = obj.ProductionType
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
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Production.ProductionHeader>("[fed].[USP_Production_SlipAndRegister]", p);
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
			return GenericProvider.GetDataSetProc<Architecture.Model.FeedMill.Production.ProductionHeader>("[fed].[USP_Production_Report]", p);
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
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Production.ProductionHeader>("[fed].[USP_ProductionAllDetails_SubReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ProductionApprovalAndUnApproval(ApprovalList obj)
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
					ParameterName = "@EntryUserId",
					Value = item.EntryUser
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ReqType",
					Value = item.ReqType
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "ProductionApprovalAndUnApproval"
				});
				GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Production.ProductionHeader>("[fed].[USP_ProductionHeader_GetAllMethod]", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetProductionIdByItem(int OrganizationId, int CompanyId, int ItemId)
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
				ParameterName = "@ItemId",
				Value = ItemId
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("[FED].[USP_GetProductionIdByItem]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInPutDetailByProductionId(int ProductionId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = ProductionId
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("[FED].[usp_getInPutByHeaderId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Production_WoodConsumptionMonthWise(ReportsParameters obj)
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
				ParameterName = "@FromDate",
				Value = obj.FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			if (obj.ShiftId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShiftId",
					Value = obj.ShiftId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Production.ProductionHeader>("[fed].[USP_Production_WoodConsumptionMonthWise]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Production_WoodConsumptionDateWise(ReportsParameters obj)
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
				ParameterName = "@FromDate",
				Value = obj.FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Production.ProductionHeader>("[fed].[USP_Production_WoodConsumptionDateWise]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Production_OverAllCosting(ReportsParameters obj)
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Production.ProductionHeader>("[fed].[USP_Production_OverAllCosting]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PartyProduction_Register(ReportsParameters obj)
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
			if (obj.ShiftId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShiftId",
					Value = obj.ShiftId
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Production.ProductionHeader>("[fed].[usp_PartyProduction_Register]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PartyProductionPendingForUpload(ReportsParameters obj)
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
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
				});
			}
			if (obj.ShiftId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShiftId",
					Value = obj.ShiftId
				});
			}
			if (obj.ProductTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductionTypeId",
					Value = obj.ProductTypeId
				});
			}
			if (obj.PriceTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PriceTypeId",
					Value = obj.PriceTypeId
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
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.FeedMill.Production.ProductionHeader>("USP_PendingPartyProductionForUpload", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateProductionUploadStatus(int UploadedById, int Id, bool IsUploaded = true)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "[dbo].[USP_ProductionHeader_UpdateUploadStatus]";
			sqlCommand.Parameters.AddWithValue("@UploadedById", UploadedById);
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.Parameters.AddWithValue("@IsUploaded", IsUploaded);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
