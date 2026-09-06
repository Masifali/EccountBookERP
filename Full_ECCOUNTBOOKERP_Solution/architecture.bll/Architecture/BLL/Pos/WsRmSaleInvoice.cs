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

public class WsRmSaleInvoice
{
	public static VoucherHead MakeVoucher(Architecture.Model.POS.WsRmSaleInvoice obj)
	{
		List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
		List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
		DataTable dtCoa = new DataTable();
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
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (dtSupplier.Count > 0)
			{
				List<SupplierCustomerGlAccountId> find2 = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(obj.SupplierCustomerId)).ToList();
				if (find2.Count <= 0)
				{
					throw new Exception("Supplier GLAccountId not Found");
				}
				vh.RefAccountId = Conversion.ToInt(find2[0].GlAccountId);
				Conversion.ToString(find2[0].CompanyName);
			}
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
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (obj.PaymentTermsId == 1)
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
					Value = vh.RefAccountId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@CoaType",
					Value = "GetAccountTypeId"
				});
				dtCoa = GenericProvider.GetDataTableProc<ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
			}
			if (obj.DocumentTypeId == 106)
			{
				bool CgsAllow = false;
				if (obj.WsRmSaleInvoiceDetailsList.Count > 0)
				{
					string Remarks2 = string.Empty;
					foreach (WsRmSaleInvoiceDetail item2 in obj.WsRmSaleInvoiceDetailsList.ToList())
					{
						if (dtitem.Count > 0)
						{
							List<GetItemGlIdsandItemName> find3 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
							if (find3.Count <= 0)
							{
								continue;
							}
							VoucherDetail vd2 = new VoucherDetail();
							Remarks2 = ((!(item2.DiscountAmount > 0.0)) ? ("Item: " + Conversion.ToString(find3[0].ItemName) + ",   Qty: " + item2.ItemQty + ",   Rate:" + item2.ItemRate + "  Item Amount: " + item2.ItemAmount) : ("Item: " + Conversion.ToString(find3[0].ItemName) + ",   Qty: " + item2.ItemQty + ",   Rate:" + item2.ItemRate + "  Item Amount: " + item2.ItemAmount + "  Discount: " + item2.Discount));
							VoucherDetail vd3 = new VoucherDetail();
							vd3.LineId = 0;
							vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd3.AgainstAccountId = Conversion.ToInt(find3[0].SaleGLAC);
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
							vd2.AccountId = Conversion.ToInt(find3[0].SaleGLAC);
							vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
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
								string DiscountAllowed = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "WsRmDiscountAllowed");
								if (!(DiscountAllowed != string.Empty))
								{
									throw new Exception("WsRmDiscountAllowed Account Not Map in Configuration Please Check!");
								}
								int DiscountAccountId2 = Conversion.ToInt(DiscountAllowed);
								if (DiscountAccountId2 <= 0)
								{
									throw new Exception("WsRmDiscountAllowed Account Not Map in Configuration Please Check!");
								}
								string DiscountRemarks = "DiscountPerItem    " + item2.Discount + "   DiscountAmount    " + item2.DiscountAmount;
								VoucherDetail vd6 = new VoucherDetail();
								vd6.AccountId = Conversion.ToInt(vh.RefAccountId);
								vd6.AgainstAccountId = Conversion.ToInt(DiscountAccountId2);
								vd6.Comments = Conversion.ToString(DiscountRemarks);
								vd6.CreditAmount = Conversion.ToDouble(item2.DiscountAmount);
								vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vh.voucherDetailList.Add(vd6);
								VoucherDetail vd9 = new VoucherDetail();
								vd9.AccountId = Conversion.ToInt(DiscountAccountId2);
								vd9.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								vd9.Comments = Conversion.ToString(DiscountRemarks);
								vd9.DebitAmount = Conversion.ToDouble(item2.DiscountAmount);
								vd9.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vh.voucherDetailList.Add(vd9);
							}
							if (obj.PaymentTermsId == 1 && dtCoa.Rows.Count > 0 && Conversion.ToInt(dtCoa.Rows[0]["AccountTypeId"]) != 2)
							{
								string CashAc = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "PosCashInHandAc");
								if (!(CashAc != string.Empty))
								{
									throw new Exception("Cash Account Not Map in Configuration Please Check!");
								}
								int CashInHandAc = Conversion.ToInt(CashAc);
								if (CashInHandAc <= 0)
								{
									throw new Exception("Cash Account Not Map in Configuration Please Check!");
								}
								VoucherDetail vd5 = new VoucherDetail();
								vd5.AccountId = CashInHandAc;
								vd5.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								vd5.Comments = Conversion.ToString(Remarks2);
								vd5.DebitAmount = Conversion.ToDouble(item2.NetItemAmount);
								vd5.ItemId = Conversion.ToInt(item2.ItemId);
								vd5.QtyOut = Conversion.ToDouble(item2.ItemQty);
								vd5.ItemRate = Conversion.ToDouble(item2.ItemRate);
								vd5.ItemAmount = Conversion.ToDouble(item2.NetItemAmount);
								vd5.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vh.voucherDetailList.Add(vd5);
								VoucherDetail vd8 = new VoucherDetail();
								vd8.AccountId = vh.RefAccountId;
								vd8.AgainstAccountId = Conversion.ToInt(CashInHandAc);
								vd8.Comments = Conversion.ToString(Remarks2);
								vd8.CreditAmount = Conversion.ToDouble(item2.NetItemAmount);
								vd8.ItemId = Conversion.ToInt(item2.ItemId);
								vd8.QtyOut = Conversion.ToDouble(item2.ItemQty);
								vd8.ItemRate = Conversion.ToDouble(item2.ItemRate);
								vd8.ItemAmount = Conversion.ToDouble(item2.NetItemAmount);
								vd8.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vh.voucherDetailList.Add(vd8);
							}
							string ConfigKey2 = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CGSEntryAllow");
							if (ConfigKey2 != string.Empty)
							{
								CgsAllow = Conversion.ToBool(ConfigKey2);
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
								CgsAmount = item2.ItemQty * item2.ItemCgsRate;
								RemarksCgs = "Item: " + Conversion.ToString(find3[0].ItemName) + ",   Qty: " + item2.ItemQty + ",   Rate:" + item2.ItemCgsRate + "  Item Amount: " + CgsAmount;
								VoucherDetail vd10 = new VoucherDetail();
								vd10.LineId = item2.LineId;
								vd10.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
								vd10.AgainstAccountId = Conversion.ToInt(find3[0].COGSGLAC);
								vd10.Comments = Conversion.ToString(RemarksCgs);
								vd10.CreditAmount = Conversion.ToDouble(CgsAmount);
								vd10.ItemId = Conversion.ToInt(item2.ItemId);
								vd10.QtyOut = Conversion.ToDouble(item2.ItemQty);
								vd10.WeightOut = Conversion.ToDouble(item2.ItemQty);
								vd10.ItemRate = Conversion.ToDouble(item2.ItemCgsRate);
								vd10.ItemCgsRate = Conversion.ToDouble(item2.ItemCgsRate);
								vd10.ItemAmount = Conversion.ToDouble(CgsAmount);
								vd10.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vh.voucherDetailList.Add(vd10);
								VoucherDetail vd11 = new VoucherDetail();
								vd11.LineId = item2.LineId;
								vd11.AccountId = Conversion.ToInt(find3[0].COGSGLAC);
								vd11.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
								vd11.Comments = Conversion.ToString(RemarksCgs);
								vd11.DebitAmount = Conversion.ToDouble(CgsAmount);
								vd11.ItemId = Conversion.ToInt(item2.ItemId);
								vd11.QtyOut = Conversion.ToDouble(item2.ItemQty);
								vd11.WeightOut = Conversion.ToDouble(item2.ItemQty);
								vd11.ItemRate = Conversion.ToDouble(item2.ItemCgsRate);
								vd11.ItemCgsRate = Conversion.ToDouble(item2.ItemCgsRate);
								vd11.ItemAmount = Conversion.ToDouble(CgsAmount);
								vd11.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vh.voucherDetailList.Add(vd11);
							}
							continue;
						}
						throw new Exception("Item Record Not found");
					}
				}
				if (CgsAllow && obj.WsRmSaleInvoiceDetailXGetYFreeslist.Count > 0)
				{
					int FreeLineId = obj.WsRmSaleInvoiceDetailsList.Count;
					foreach (WsRmSaleInvoiceDetailXGetYFree item in obj.WsRmSaleInvoiceDetailXGetYFreeslist.ToList())
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
									string ConfigKey = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "WsRmDiscountAllowed");
									if (ConfigKey != string.Empty)
									{
										int DiscountAccountId = Conversion.ToInt(ConfigKey);
										if (DiscountAccountId > 0)
										{
											VoucherDetail vd7 = new VoucherDetail();
											vd7.LineId = FreeLineId;
											vd7.AccountId = Conversion.ToInt(DiscountAccountId);
											vd7.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
											vd7.Comments = Conversion.ToString(Remarks);
											vd7.DebitAmount = Conversion.ToDouble(YItemAmount);
											vd7.ItemId = Conversion.ToInt(item.YItemId);
											vd7.QtyOut = Conversion.ToDouble(item.YItemFactorQty);
											vd7.ItemRate = Conversion.ToDouble(YRate);
											vd7.WeightOut = Conversion.ToDouble(item.YItemFactorQty);
											vd7.ItemAmount = Conversion.ToDouble(YItemAmount);
											vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
											vh.voucherDetailList.Add(vd7);
											VoucherDetail vd4 = new VoucherDetail();
											vd4.LineId = FreeLineId;
											vd4.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
											vd4.AgainstAccountId = Conversion.ToInt(DiscountAccountId);
											vd4.Comments = Conversion.ToString(Remarks);
											vd4.CreditAmount = Conversion.ToDouble(YItemAmount);
											vd4.ItemId = Conversion.ToInt(item.YItemId);
											vd4.QtyOut = Conversion.ToDouble(item.YItemFactorQty);
											vd4.ItemRate = Conversion.ToDouble(YRate);
											vd4.WeightOut = Conversion.ToDouble(item.YItemFactorQty);
											vd4.ItemAmount = Conversion.ToDouble(YItemAmount);
											vd4.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
											vh.voucherDetailList.Add(vd4);
											continue;
										}
										throw new Exception("DiscountAllowed Account Not Map in Configuration Please Check!");
									}
									throw new Exception("DiscountAllowed Account Configuration not found!");
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

	public static int Save(Architecture.Model.POS.WsRmSaleInvoice obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
				return new Architecture.DAL.Pos.WsRmSaleInvoice().SetData(obj, "Sp_WsRmSaleInvoice_Insert");
			}
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			return new Architecture.DAL.Pos.WsRmSaleInvoice().SetData(obj, "Sp_WsRmSaleInvoice_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.POS.WsRmSaleInvoice GetByID(int Id)
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
			new List<Architecture.Model.POS.WsRmSaleInvoice>();
			return new Architecture.DAL.Pos.WsRmSaleInvoice().GetData("Sp_WsRmSaleInvoice_GetAllMethod", p)[0];
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.PosSaleInvoice>("Sp_WsRmSaleInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateDocCode(Architecture.Model.POS.WsRmSaleInvoice obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmSaleInvoice>("Sp_WsRmSaleInvoice_GetAllMethod", p);
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

	public static DataTable GetLoaderDataForSaleInvoice(ReportsParameters obj)
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
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetLoaderDataForSaleInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmSaleInvoice>("Sp_WsRmSaleInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ApproveWsRmSaleInvoice(Architecture.Model.POS.WsRmSaleInvoice obj)
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
				ParameterName = "@PostDate",
				Value = obj.PostDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PostUser",
				Value = obj.PostUser
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@IsApproved",
				Value = obj.IsApproved
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ApproveWsRmSaleInvoice"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmSaleInvoice>("Sp_WsRmSaleInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GenerateDiscountAgainstItemIdAndRateForWholeSale(ReportsParameters obj)
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
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PriceTypeId",
				Value = obj.PriceTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemQty",
				Value = obj.ItemQty
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemRate",
				Value = obj.ItemRate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PaymenetTermId",
				Value = obj.PaymenetTermId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmSaleInvoice>("Sp_GenerateDiscountAgainstItemIdAndRateForWholeSale", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
