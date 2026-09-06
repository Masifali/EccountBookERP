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

public class PosSaleInvoice
{
	public static VoucherHead MakeVoucher(Architecture.Model.POS.PosSaleInvoice obj)
	{
		List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
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
			vh.VoucherAmount = obj.BillAmount;
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.BillAmount = obj.BillAmount;
			vh.ManualBillNo = obj.ManualBillNo;
			vh.DueDate = DateTime.Now;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.voucherDetailList = new List<VoucherDetail>();
			if (obj.SupplierCustomerId == 0)
			{
				throw new Exception("AccountId Not Found Please Check!");
			}
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (obj.DocumentTypeId == 100)
			{
				bool CgsAllow = false;
				if (obj.PosSaleInvoiceDetailList.Count > 0)
				{
					string Remarks2 = string.Empty;
					foreach (PosSaleInvoiceDetail item2 in obj.PosSaleInvoiceDetailList.ToList())
					{
						if (dtitem.Count > 0)
						{
							List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
							if (find2.Count <= 0)
							{
								continue;
							}
							VoucherDetail vd2 = new VoucherDetail();
							Remarks2 = ((!(item2.DiscountAmount > 0.0)) ? ("Item: " + Conversion.ToString(find2[0].ItemName) + ",   Qty: " + item2.ItemQty + ",   Rate:" + item2.ItemRate + "  Item Amount: " + item2.ItemAmount) : ("Item: " + Conversion.ToString(find2[0].ItemName) + ",   Qty: " + item2.ItemQty + ",   Rate:" + item2.ItemRate + "  Item Amount: " + item2.ItemAmount + "  Discount: " + item2.Discount));
							VoucherDetail vd3 = new VoucherDetail();
							vd3.LineId = 0;
							vd3.AccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd3.AgainstAccountId = Conversion.ToInt(find2[0].SaleGLAC);
							vd3.Comments = Conversion.ToString(Remarks2);
							vd3.DebitAmount = Conversion.ToDouble(item2.ItemAmount);
							vd3.ItemId = Conversion.ToInt(item2.ItemId);
							vd3.QtyOut = Conversion.ToDouble(item2.ItemQty);
							vd3.ItemRate = Conversion.ToDouble(item2.ItemRate);
							vd3.WeightOut = Conversion.ToDouble(item2.ItemQty);
							vd3.ItemAmount = Conversion.ToDouble(item2.ItemAmount);
							vd3.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd3);
							vd2.LineId = 0;
							vd2.AccountId = Conversion.ToInt(find2[0].SaleGLAC);
							vd2.AgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd2.Comments = Conversion.ToString(Remarks2);
							vd2.CreditAmount = Conversion.ToDouble(item2.ItemAmount);
							vd2.ItemId = Conversion.ToInt(item2.ItemId);
							vd2.QtyOut = Conversion.ToDouble(item2.ItemQty);
							vd2.ItemRate = Conversion.ToDouble(item2.ItemRate);
							vd2.ItemAmount = Conversion.ToDouble(item2.ItemAmount);
							vd2.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd2);
							if (item2.DiscountAmount > 0.0)
							{
								string ConfigKey2 = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "PosDiscountAllowed");
								if (!(ConfigKey2 != string.Empty))
								{
									throw new Exception("PosDiscountAllowed Account Configuration not found!");
								}
								int DiscountAccountId2 = Conversion.ToInt(ConfigKey2);
								if (DiscountAccountId2 <= 0)
								{
									throw new Exception("PosDiscountAllowed Account Not Map in Configuration Please Check!");
								}
								string DiscountRemarks = "DiscountPerItem    " + item2.Discount + "   DiscountAmount    " + item2.DiscountAmount;
								VoucherDetail vd7 = new VoucherDetail();
								vd7.AccountId = Conversion.ToInt(DiscountAccountId2);
								vd7.AgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
								vd7.Comments = Conversion.ToString(DiscountRemarks);
								vd7.DebitAmount = Conversion.ToDouble(item2.DiscountAmount);
								vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vh.voucherDetailList.Add(vd7);
								VoucherDetail vd5 = new VoucherDetail();
								vd5.AccountId = Conversion.ToInt(obj.SupplierCustomerId);
								vd5.AgainstAccountId = Conversion.ToInt(DiscountAccountId2);
								vd5.Comments = Conversion.ToString(DiscountRemarks);
								vd5.CreditAmount = Conversion.ToDouble(item2.DiscountAmount);
								vd5.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vh.voucherDetailList.Add(vd5);
							}
							string ConfigKeyCGS = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CGSEntryAllow");
							if (ConfigKeyCGS != string.Empty)
							{
								CgsAllow = Conversion.ToBool(ConfigKeyCGS);
							}
							if (CgsAllow)
							{
								ReportsParameters reports2 = new ReportsParameters();
								reports2.OrganizationId = obj.OrganizationId;
								reports2.CompanyId = obj.CompanyId;
								reports2.ItemId = item2.ItemId;
								reports2.WarehouseId = item2.WarehouseId;
								reports2.DocDate = obj.DocDate;
								if (obj.Id > 0)
								{
									reports2.DocumentTypeId = obj.DocumentTypeId;
									reports2.Id = obj.Id;
								}
								double AvgRate2 = CommonServies.AvgRateOnlyForCGS(reports2);
								if (!(AvgRate2 > 0.0))
								{
									throw new Exception("CostRate Not Found Please Check");
								}
								double BaseUom2 = CommonServies.GetEqvilentByItemId(reports2);
								item2.ItemCgsRate = AvgRate2 * BaseUom2;
								double CgsAmount = 0.0;
								string RemarksCgs = "";
								if (item2.ItemCgsRate > 0.0)
								{
									CgsAmount = item2.ItemQty * item2.ItemCgsRate;
									RemarksCgs = "Item: " + Conversion.ToString(find2[0].ItemName) + ",   Qty: " + item2.ItemQty + ",   Rate:" + item2.ItemCgsRate + "  Item Amount: " + CgsAmount;
									VoucherDetail vd9 = new VoucherDetail();
									vd9.LineId = item2.LineId;
									vd9.AccountId = Conversion.ToInt(find2[0].COGSGLAC);
									vd9.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
									vd9.Comments = Conversion.ToString(RemarksCgs);
									vd9.DebitAmount = Conversion.ToDouble(CgsAmount);
									vd9.ItemId = Conversion.ToInt(item2.ItemId);
									vd9.QtyOut = Conversion.ToDouble(item2.ItemQty);
									vd9.WeightOut = Conversion.ToDouble(item2.ItemQty);
									vd9.ItemRate = Conversion.ToDouble(item2.ItemCgsRate);
									vd9.ItemAmount = Conversion.ToDouble(CgsAmount);
									vd9.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
									vh.voucherDetailList.Add(vd9);
									VoucherDetail vd8 = new VoucherDetail();
									vd8.LineId = item2.LineId;
									vd8.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
									vd8.AgainstAccountId = Conversion.ToInt(find2[0].COGSGLAC);
									vd8.Comments = Conversion.ToString(RemarksCgs);
									vd8.CreditAmount = Conversion.ToDouble(CgsAmount);
									vd8.ItemId = Conversion.ToInt(item2.ItemId);
									vd8.QtyOut = Conversion.ToDouble(item2.ItemQty);
									vd8.WeightOut = Conversion.ToDouble(item2.ItemQty);
									vd8.ItemRate = Conversion.ToDouble(item2.ItemCgsRate);
									vd8.ItemAmount = Conversion.ToDouble(CgsAmount);
									vd8.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
									vh.voucherDetailList.Add(vd8);
								}
							}
							continue;
						}
						throw new Exception("Item Record Not found");
					}
				}
				if (CgsAllow && obj.PosSaleInvoiceDetailXGetYFreeslist.Count > 0)
				{
					int FreeLineId = obj.PosSaleInvoiceDetailList.Count;
					foreach (PosSaleInvoiceDetailXGetYFree item in obj.PosSaleInvoiceDetailXGetYFreeslist.ToList())
					{
						FreeLineId++;
						string Remarks = "";
						if (dtitem.Count > 0)
						{
							List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.YItemId)).ToList();
							if (find.Count > 0)
							{
								double YRate = 0.0;
								double YItemAmount = 0.0;
								double BaseUom = 0.0;
								ReportsParameters reports = new ReportsParameters();
								reports.OrganizationId = obj.OrganizationId;
								reports.CompanyId = obj.CompanyId;
								reports.ItemId = item.YItemId;
								reports.WarehouseId = item.WarehouseId;
								reports.DocDate = obj.DocDate;
								if (obj.Id > 0)
								{
									reports.DocumentTypeId = obj.DocumentTypeId;
									reports.Id = obj.Id;
								}
								double AvgRate = CommonServies.AvgRateOnlyForCGS(reports);
								if (AvgRate > 0.0)
								{
									BaseUom = CommonServies.GetEqvilentByItemId(reports);
									YRate = AvgRate * BaseUom;
									YItemAmount = YRate * item.YItemFactorQty;
									new VoucherDetail();
									Remarks = "Item: " + Conversion.ToString(find[0].ItemName) + ",   Qty: " + item.YItemFactorQty + ",   Rate:" + YRate + "  Item Amount: " + YItemAmount;
									string ConfigKey = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "PosDiscountAllowed");
									if (ConfigKey != string.Empty)
									{
										int DiscountAccountId = Conversion.ToInt(ConfigKey);
										if (DiscountAccountId > 0)
										{
											VoucherDetail vd6 = new VoucherDetail();
											vd6.AccountId = Conversion.ToInt(DiscountAccountId);
											vd6.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
											vd6.ItemId = Conversion.ToInt(item.YItemId);
											vd6.ItemRate = Conversion.ToDouble(YRate);
											vd6.LineId = Conversion.ToInt(FreeLineId);
											vd6.QtyOut = Conversion.ToDouble(item.YItemFactorQty);
											vd6.WeightOut = Conversion.ToDouble(item.YItemFactorQty);
											vd6.Comments = Conversion.ToString(Remarks);
											vd6.DebitAmount = Conversion.ToDouble(YItemAmount);
											vd6.ItemAmount = Conversion.ToDouble(YItemAmount);
											vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
											vh.voucherDetailList.Add(vd6);
											VoucherDetail vd4 = new VoucherDetail();
											vd4.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
											vd4.AgainstAccountId = Conversion.ToInt(DiscountAccountId);
											vd4.Comments = Conversion.ToString(Remarks);
											vd4.ItemId = Conversion.ToInt(item.YItemId);
											vd4.LineId = Conversion.ToInt(FreeLineId);
											vd4.ItemRate = Conversion.ToDouble(YRate);
											vd4.QtyOut = Conversion.ToDouble(item.YItemFactorQty);
											vd4.WeightOut = Conversion.ToDouble(item.YItemFactorQty);
											vd4.CreditAmount = Conversion.ToDouble(YItemAmount);
											vd4.ItemAmount = Conversion.ToDouble(YItemAmount);
											vd4.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
											vh.voucherDetailList.Add(vd4);
											continue;
										}
										throw new Exception("PosDiscountAllowed Account Not Map in Configuration Please Check!");
									}
									throw new Exception("PosDiscountAllowed Account Configuration not found!");
								}
								throw new Exception("CGS Rate not found of discounted Item");
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

	public static int Save(Architecture.Model.POS.PosSaleInvoice obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
				return new Architecture.DAL.Pos.PosSaleInvoice().SetData(obj, "Sp_PosSaleInvoice_Insert");
			}
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			return new Architecture.DAL.Pos.PosSaleInvoice().SetData(obj, "Sp_PosSaleInvoice_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.POS.PosSaleInvoice GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			new List<Architecture.Model.POS.PosSaleInvoice>();
			return new Architecture.DAL.Pos.PosSaleInvoice().GetData("Sp_PosSaleInvoice_ReadById", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetHisoty(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.PosSaleInvoice>("Sp_PosSaleInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateDocCode(Architecture.Model.POS.PosSaleInvoice obj)
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
				ParameterName = "@Activity",
				Value = "GenerateDocCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.POS.PosSaleInvoice>("Sp_PosSaleInvoice_GetAllMethod", p);
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

	public static DataTable GetItemsEvaluationandPricingSch(ReportsParameters obj)
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
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WareHouseId",
					Value = obj.WarehouseId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@DateTo",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PriceTypeId",
				Value = obj.PriceTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetItemsEvaluationandPricingSch"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.PosSaleInvoice>("Sp_PosSaleInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetAvgRateByItemWareHouseId(ReportsParameters obj)
	{
		try
		{
			double AvgRate = 0.0;
			List<SqlParameter> x = new List<SqlParameter>();
			x.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			x.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			x.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			x.Add(new SqlParameter
			{
				ParameterName = "@DateTo",
				Value = obj.DocDate
			});
			if (obj.WarehouseId != 0)
			{
				x.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.JobLotId != 0)
			{
				x.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
				});
			}
			x.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAvgRateByItemWareHouseId"
			});
			DataTable dt = GenericProvider.GetDataTableProc<InvSaleInvoice>("[Sp_PosSaleInvoice_GetAllMethod]", x);
			if (dt.Rows.Count > 0)
			{
				AvgRate = Conversion.ToDouble(dt.Rows[0]["AvgRate"]);
			}
			return AvgRate;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWarehouseListWithAvailableStockAgainstItemId(ReportsParameters obj)
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetWarehouseListWithAvailableStockAgainstItemId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.PosSaleInvoice>("Sp_PosSaleInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
