using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Import;
using Architecture.Model.Accounts;
using Architecture.Model.Import;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Import;

public class ImInvoice
{
	public static VoucherHead MakeVoucher(Architecture.Model.Import.ImInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		try
		{
			double ConversionAmount = 0.0;
			int StockAgainstAcId = 0;
			List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
			List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.DocCode);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = obj.RemarksHeader;
			vh.MultiCurrencyId = obj.FcurrencyId;
			vh.FcAmount = obj.FCurrencyAmount;
			vh.ExchangeCurrencyRate = obj.ConversionRate;
			if (obj.ConversionRate > 0.0 && obj.FCurrencyAmount > 0.0)
			{
				ConversionAmount = obj.FCurrencyAmount * obj.ConversionRate;
			}
			vh.VoucherAmount = ConversionAmount;
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (dtSupplier.Count > 0)
			{
				List<SupplierCustomerGlAccountId> find2 = dtSupplier.Where((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(obj.SupplierCustomerId)).ToList();
				if (find2.Count <= 0)
				{
					throw new Exception("Supplier GLAccountId not Found");
				}
				vh.RefAccountId = Conversion.ToInt(find2[0].GlAccountId);
			}
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.BillAmount = obj.TotalAmount;
			vh.ManualBillNo = obj.InvoiceNo;
			vh.DueDate = obj.DocDate;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.FinancialYearId = obj.FinancialYearId;
			string ConfigKey = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "StockAgainstAccount");
			if (!(ConfigKey != string.Empty))
			{
				throw new Exception("Stock AgainstAc Configuration Not Found");
			}
			StockAgainstAcId = Conversion.ToInt(ConfigKey);
			vh.voucherDetailList = new List<VoucherDetail>();
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (obj.DocumentTypeId == 233 || obj.DocumentTypeId == 237)
			{
				if (obj.ExImInvoiceDetail.Count <= 0)
				{
					throw new Exception("Detail list is Empty Please Check");
				}
				string Remarks = string.Empty;
				foreach (ImInvoicePackingDetail item2 in obj.ExImInvoiceDetail.ToList())
				{
					if (dtitem.Count > 0)
					{
						List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item2.ItemId)).ToList();
						if (find.Count > 0)
						{
							VoucherDetail vd3 = new VoucherDetail();
							if (obj.DocumentTypeId == 233 || obj.DocumentTypeId == 237)
							{
								Remarks = ((!(item2.AddLessAmount > 0.0) && !(item2.AddLessAmount < 0.0)) ? ("ItemName  " + Conversion.ToString(find[0].ItemName) + "  M.Ton: " + item2.NetWeight / 1000.0 + "  NetWeight: " + item2.NetWeight + "    @Rate:" + item2.RatePrice + "    Item Amount: " + item2.FcAmount) : ("ItemName  " + Conversion.ToString(find[0].ItemName) + "  M.Ton: " + item2.NetWeight / 1000.0 + "  NetWeight: " + item2.NetWeight + "    @Rate:" + item2.RatePrice + "    Item Amount: " + item2.FcAmount + "    AddLessAmount: " + item2.AddLessAmount));
							}
							double FcAmountwithAddLess = (item2.FcAmount + Conversion.ToDouble(item2.AddLessAmount)) * obj.ConversionRate;
							vd3.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd3.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd3.Comments = Conversion.ToString(Remarks);
							vd3.DebitAmount = Conversion.ToDouble(FcAmountwithAddLess);
							vd3.DMultiCurrencyId = obj.FcurrencyId;
							vd3.DExchangeCurrencyRate = obj.ConversionRate;
							vd3.DCurrencyAmount = item2.FcAmount;
							vd3.CreditAmount = Conversion.ToSingle(0);
							vd3.ItemId = Conversion.ToInt(item2.ItemId);
							vd3.QtyIn = Conversion.ToDouble(item2.OuterQty);
							vd3.ItemRate = Conversion.ToDouble(item2.RatePrice);
							vd3.WeightIn = Conversion.ToDouble(item2.OuterQty) * 1000.0;
							vd3.ItemAmount = Conversion.ToDouble(FcAmountwithAddLess);
							vd3.OrderNo = Conversion.ToInt(obj.LcOrderNoId);
							vd3.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd3);
							VoucherDetail vd4 = new VoucherDetail();
							vd4.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd4.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd4.Comments = Conversion.ToString(Remarks);
							vd4.DebitAmount = Conversion.ToDouble(0);
							vd4.CreditAmount = Conversion.ToDouble(FcAmountwithAddLess);
							vd4.DMultiCurrencyId = obj.FcurrencyId;
							vd4.DExchangeCurrencyRate = obj.ConversionRate;
							vd4.DCurrencyAmount = item2.FcAmount;
							vd4.ItemId = Conversion.ToInt(item2.ItemId);
							vd4.QtyIn = Conversion.ToDouble(item2.OuterQty);
							vd4.ItemRate = Conversion.ToDouble(item2.RatePrice);
							vd4.WeightIn = Conversion.ToDouble(item2.OuterQty) * 1000.0;
							vd4.ItemAmount = Conversion.ToDouble(FcAmountwithAddLess);
							vd4.OrderNo = Conversion.ToInt(obj.LcOrderNoId);
							vd4.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd4);
							if (item2.Expenses > 0.0)
							{
								VoucherDetail vd5 = new VoucherDetail();
								vd5.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd5.AgainstAccountId = Conversion.ToInt(StockAgainstAcId);
								vd5.Comments = Conversion.ToString("ExpensesAmount : " + item2.Expenses);
								vd5.DebitAmount = Conversion.ToDouble(item2.Expenses) * obj.ConversionRate;
								vd5.DMultiCurrencyId = obj.FcurrencyId;
								vd5.DExchangeCurrencyRate = obj.ConversionRate;
								vd5.DCurrencyAmount = item2.Expenses;
								vd5.CreditAmount = Conversion.ToSingle(0);
								vh.voucherDetailList.Add(vd5);
							}
							continue;
						}
						throw new Exception("ItemGlAccount Not found");
					}
					throw new Exception("Item Not found");
				}
				if (obj.ImInvoiceExpenseslst.Count > 0)
				{
					foreach (ImInvoiceExpenses item in obj.ImInvoiceExpenseslst.ToList())
					{
						VoucherDetail vd2 = new VoucherDetail();
						vd2.AccountId = Conversion.ToInt(item.AccountId);
						vd2.AgainstAccountId = Conversion.ToInt(StockAgainstAcId);
						vd2.Comments = Conversion.ToString(item.Remarks);
						vd2.CreditAmount = Conversion.ToDouble(item.LocalAmount);
						vd2.DMultiCurrencyId = obj.FcurrencyId;
						vd2.DExchangeCurrencyRate = obj.ConversionRate;
						vd2.DCurrencyAmount = item.FcAmount;
						vd2.DebitAmount = Conversion.ToSingle(0);
						vh.voucherDetailList.Add(vd2);
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return vh;
	}

	public static int Save(Architecture.Model.Import.ImInvoice obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
				return new Architecture.DAL.Import.ImInvoice().SetDate(obj, "Sp_ImInvoice_Insert");
			}
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			return new Architecture.DAL.Import.ImInvoice().SetDate(obj, "Sp_ImInvoice_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetData(Architecture.Model.Import.ImInvoice obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImInvoice>("Sp_ImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Import.ImInvoice obj)
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
				Value = "GenerateDocNo"
			});
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.Import.ImInvoice>("Sp_ImInvoice_GetAllMethod", p).Rows[0]["DocNo"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Import.ImInvoice GetByID(int Id)
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
			new List<Architecture.Model.Import.ImInvoice>();
			return new Architecture.DAL.Import.ImInvoice().GetDate("Sp_ImInvoice_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExImInvoicePackingDetailForVouchers(int Id)
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
				Value = "GetExImInvoicePackingDetailForVouchers"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImInvoice>("Sp_ImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceDetailByHeaderId(Architecture.Model.Import.ImInvoice obj)
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
				ParameterName = "@Activity",
				Value = "GetInvoiceDetailByHeaderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImInvoice>("Sp_ImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierForFcyPaymentVoucher(Architecture.Model.Import.ImInvoice obj)
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
				ParameterName = "@Activity",
				Value = "GetSupplierForFcyPaymentVoucher"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImInvoice>("Sp_ImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceNoForFcyPaymentVoucher(Architecture.Model.Import.ImInvoice obj)
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
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetInvoiceNoForFcyPaymentVoucher"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImInvoice>("Sp_ImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceAmountAndPaidAmount(Architecture.Model.Import.ImInvoice obj)
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
				ParameterName = "@Activity",
				Value = "GetInvoiceAmountAndPaidAmount"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImInvoice>("Sp_ImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExchangeRateandCurrencyFromVoucher(Architecture.Model.Import.ImInvoice obj)
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
				ParameterName = "@Activity",
				Value = "GetExchangeRateandCurrencyFromVoucher"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImInvoice>("Sp_ImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ApproveImportInvoice(Architecture.Model.Import.ImInvoice obj)
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
				ParameterName = "@ApprovedDate",
				Value = obj.ApprovedDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ApprovedUser",
				Value = obj.ApprovedUser
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@IsApproved",
				Value = obj.IsApproved
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ApproveImportInvoice"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Import.ImInvoice>("Sp_ImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
